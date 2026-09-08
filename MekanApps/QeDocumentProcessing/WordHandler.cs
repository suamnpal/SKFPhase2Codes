using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using QeDynamicDocumentProcessing.Common;
using System.Reflection;
using System.Text.RegularExpressions;

namespace QeDynamicDocumentProcessing
{
    /// <summary>
    /// This function is only used for generating final dynamic word document out of Qe templates
    /// There are several templates and logics - but the function is common for all
    /// </summary>
    public static class WordHandler
    {
        public static string ProcessWordTemplate(APIRequest req)
        {
            string resultBase64 = string.Empty;
            byte[] byteArray = Convert.FromBase64String(req.WordFileAsBase64);
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(byteArray, 0, byteArray.Length);
                stream.Position = 0;

                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(stream, true))
                {
                    Type? wordProcessor = Type.GetType("QeDynamicDocumentProcessing.TemplateFiles." + req.TemplateType);
                    if (wordProcessor != null)
                    {
                        Dictionary<string, string> headerKeyValues = CommonFunctions.GenerateDocumentHeaders(req);
                        var headerParts = wordDoc.MainDocumentPart.GetPartsOfType<HeaderPart>().ToList();
                        foreach (var headerPart in headerParts)
                        {
                            var headerTexts = headerPart.Header.Descendants<Text>().ToList();
                            for (int i = 0; i < headerTexts.Count; i++)
                            {
                                var headerText = headerTexts[i];
                                //Special handling for DocumentCategory since this requires line breaks
                                if (headerText.Text == "DocumentCategory" && !string.IsNullOrEmpty(headerKeyValues[headerText.Text]))
                                {
                                    string[] categories = headerKeyValues[headerText.Text].Split('\\', StringSplitOptions.RemoveEmptyEntries);
                                    if (categories.Length > 0)
                                    {
                                        headerText.Text = categories[0];
                                        foreach (var category in categories.Skip(1))
                                        {
                                            (headerText.Parent as Run).Append(new Break(), new Text(category));
                                        }
                                    }
                                    continue;
                                }
                                //Duplicate Headers handling
                                if (i + 1 < headerTexts.Count)
                                {
                                    var nextHeaderText = headerTexts[i + 1];
                                    if (headerText.Text == nextHeaderText.Text)
                                        continue;
                                }
                                if (headerKeyValues.ContainsKey(headerText.Text))
                                {
                                    headerText.Text = headerKeyValues[headerText.Text];
                                }
                            }
                            headerPart.Header.Save();
                        }

                        Dictionary<string, string> footerKeyValues = CommonFunctions.GenerateDocumentFooters(req);
                        var footerParts = wordDoc.MainDocumentPart.GetPartsOfType<FooterPart>().ToList();
                        foreach (var footerPart in footerParts)
                        {
                            var footerTexts = footerPart.Footer.Descendants<Text>().ToList();
                            foreach (var footerText in footerTexts)
                            {
                                if (footerKeyValues.ContainsKey(footerText.Text))
                                {
                                    footerText.Text = footerKeyValues[footerText.Text];
                                    continue;
                                }
                                //Date placeholder handling -> automatic date calculation never works in WordML code
                                if (Regex.IsMatch(footerText.Text, @"^\d{4}-\d{2}-\d{2}$"))
                                    footerText.Text = DateTime.Today.ToString("yyyy-MM-dd");
                            }
                            footerPart.Footer.Save();
                        }

                        object? instance = Activator.CreateInstance(wordProcessor);
                        MethodInfo? calculateWordParameters = wordProcessor.GetMethod("CalculateWordParameters");
                        object? result = calculateWordParameters?.Invoke(instance, new object[] { req });
                        if (result != null)
                        {
                            Dictionary<string, string> keyValues = (Dictionary<string, string>)result;
                            var body = wordDoc.MainDocumentPart.Document.Body;

                            // Get all TextBox elements
                            var textBoxContents = body.Descendants<TextBoxContent>().ToList();
                            foreach (var tbc in textBoxContents)
                            {
                                if (keyValues.ContainsKey(tbc.InnerText))
                                {
                                    TextReplacementInRun(tbc.Descendants<Run>().ToList(), keyValues[tbc.InnerText]);
                                }
                            }

                            foreach (var para in body.Descendants<Paragraph>())
                            {
                                if (keyValues.ContainsKey(para.InnerText))
                                {
                                    TextReplacementInRun(para.Descendants<Run>().ToList(), keyValues[para.InnerText]);
                                }
                            }

                            // Get all bookmarks
                            var bookmarks = body.Descendants<BookmarkStart>().ToList();

                            foreach (var bookmark in bookmarks)
                            {
                                if (bookmark.Name != null && keyValues.ContainsKey(bookmark.Name))
                                {
                                    string replacement = keyValues[bookmark.Name];

                                    var parent = bookmark.Parent;

                                    if (parent == null)
                                        continue;

                                    // find matching end
                                    var end = parent.Descendants<BookmarkEnd>()
                                                    .FirstOrDefault(b => b.Id.Value == bookmark.Id.Value);

                                    if (end == null)
                                        continue;

                                    // gather all elements between start & end
                                    var between = bookmark
                                        .ElementsAfter()
                                        .TakeWhile(n => n != end)
                                        .ToList();


                                    // ❗ Capture the first run's formatting (if any)
                                    RunProperties runPr = null;
                                    foreach (var el in between)
                                    {
                                        if (el is Run r)
                                        {
                                            var rp = r.GetFirstChild<RunProperties>();
                                            if (rp != null)
                                            {
                                                runPr = (RunProperties)rp.CloneNode(true);
                                            }
                                            break;
                                        }
                                    }

                                    // remove all runs
                                    foreach (var n in between)
                                    {
                                        n.Remove();
                                    }

                                    // create new run with preserved formatting
                                    var newRun = new Run();
                                    if (runPr != null)
                                        newRun.Append(runPr);

                                    if (replacement.Contains("<<LineBreak>>"))
                                    {
                                        string[] lines = replacement.Split("<<LineBreak>>", StringSplitOptions.RemoveEmptyEntries);
                                        if (lines.Length > 0)
                                        {
                                            newRun.Append(new Text(lines[0]));
                                            foreach (var line in lines.Skip(1))
                                            {
                                                newRun.Append(new Break(), new Text(line));
                                            }
                                        }
                                    }
                                    else
                                        newRun.Append(new Text(replacement));

                                    // insert before bookmarkEnd
                                    end.InsertBeforeSelf(newRun);
                                }
                            }

                            wordDoc.MainDocumentPart.Document.Save();
                        }
                    }
                    else
                    {
                        // Handle the case where the type is not found
                        throw new Exception("Type not found for template: " + req.TemplateType);
                    }
                }
                
                stream.Position = 0;
                resultBase64 = Convert.ToBase64String(stream.ToArray());
            }
            return resultBase64;
        }

        static void HandleParagraph(Paragraph para, string key, string value)
        {
            var runs = para.Descendants<Run>().ToList();

            //First do a Run checks - single para but different formating as runs
            var matchingRun = runs.FirstOrDefault(x => x.InnerText.Contains(key));
            if (matchingRun != null)
            {
                HandleRun(matchingRun, matchingRun.InnerText.Replace(key, value));
            }
            else
            {
                HandleRun(runs.First(), para.InnerText.Replace(key, value));
                // Remove all other runs if more than 1
                if (runs.Count > 1)
                {
                    for (int i = 1; i < runs.Count; i++)
                    {
                        runs[i].Remove();
                    }
                }
            }
        }

        static void HandleRun(Run run, string value)
        {
            var texts = run.Descendants<Text>().ToList();
            var text = texts.FirstOrDefault();
            if (text != null)
            {
                text.Text = value;
            }
            // Remove all other Texts in the same run if more than 1
            if (texts.Count > 1)
            {
                for (int i = 1; i < texts.Count; i++)
                {
                    texts[i].Remove();
                }
            }
        }

        static void TextReplacementInRun(List<Run> runs, string textToReplace)
        {
            // Remove all other runs if more than 1
            if (runs.Count > 1)
            {
                for (int i = 1; i < runs.Count; i++)
                {
                    runs[i].Remove();
                }
            }

            var firstText = runs.First().Descendants<Text>().FirstOrDefault();
            if (firstText != null)
            {
                if (textToReplace.Contains("<<LineBreak>>"))
                {
                    string[] lines = textToReplace.Split("<<LineBreak>>", StringSplitOptions.RemoveEmptyEntries);
                    if (lines.Length > 0)
                    {
                        firstText.Text = lines[0];
                        foreach (var line in lines.Skip(1))
                        {
                            runs.First().Append(new Break(), new Text(line));
                        }
                    }
                }
                else
                    firstText.Text = textToReplace;
            }
        }
    }
}
