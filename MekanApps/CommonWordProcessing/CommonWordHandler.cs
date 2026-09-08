using CommonDocumentProcessing.Common;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using HtmlToOpenXml;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;

namespace CommonDocumentProcessing
{
    /// <summary>
    /// This function is used by Print function which prepares a word template (with placeholders) 
    /// with the incoming values (text/images)
    /// </summary>
    public static class CommonWordHandler
    {
        public static string ProcessWordDocument(APIRequest req)
        {
            string resultWordDoc = string.Empty;
            byte[] byteArray = Convert.FromBase64String(req.InputFileAsBase64);

            using (var stream = new MemoryStream())
            {
                stream.Write(byteArray, 0, byteArray.Length);
                stream.Position = 0;

                using (var wordDoc = WordprocessingDocument.Open(stream, true))
                {
                    var body = wordDoc.MainDocumentPart.Document.Body;

                    var headerParts = wordDoc.MainDocumentPart.GetPartsOfType<HeaderPart>().ToList();
                    foreach (var headerPart in headerParts)
                    {
                        foreach (Bookmark bookmark in req.Bookmarks.Where(x => x.BookmarkType == "Text"))
                        {
                            if (headerPart.Header.InnerText.Contains(bookmark.BookmarkName))
                            {
                                foreach (var para in headerPart.Header.Descendants<Paragraph>())
                                {
                                    HandleParagraph(para, bookmark.BookmarkName, bookmark.BookmarkValue);
                                }
                                headerPart.Header.Save();
                            }
                        }
                    }

                    var footerParts = wordDoc.MainDocumentPart.GetPartsOfType<FooterPart>().ToList();
                    foreach (var footerPart in footerParts)
                    {
                        foreach (Bookmark bookmark in req.Bookmarks.Where(x => x.BookmarkType == "Text"))
                        {
                            if (footerPart.Footer.InnerText.Contains(bookmark.BookmarkName))
                            {
                                foreach (var para in footerPart.Footer.Descendants<Paragraph>())
                                {
                                    HandleParagraph(para, bookmark.BookmarkName, bookmark.BookmarkValue);
                                }
                                footerPart.Footer.Save();
                            }
                        }
                    }

                    var paragraphs = body
                        .Descendants<Paragraph>()
                        .ToList(); // ✅ CRITICAL FIX

                    foreach (var para in paragraphs)
                    {
                        foreach (Bookmark bookmark in req.Bookmarks)
                        {
                            if (para.InnerText.Contains(bookmark.BookmarkName))
                            {
                                switch (bookmark.BookmarkType)
                                {
                                    case "Image":
                                        HandleImage(para, bookmark.BookmarkName, bookmark.BookmarkValue, wordDoc.MainDocumentPart);
                                        break;
                                    case "HTML":
                                        HandleHTML(para, bookmark.BookmarkName, bookmark.BookmarkValue, wordDoc.MainDocumentPart);
                                        break;
                                    case "WordFileContent":
                                        HandleWordFileContent(para, bookmark.BookmarkName, bookmark.BookmarkValue, wordDoc.MainDocumentPart);
                                        break;
                                    case "Table":
                                        HandleTable(para, bookmark.BookmarkName, bookmark.BookmarkValue);
                                        break;
                                    default:
                                        HandleParagraph(para, bookmark.BookmarkName, bookmark.BookmarkValue);
                                        break;
                                }
                            }
                        }
                    }
                    wordDoc.MainDocumentPart.Document.Save();
                }
                stream.Position = 0;
                resultWordDoc = Convert.ToBase64String(stream.ToArray());
            }

            return resultWordDoc;
        }

        // -------------------- Helpers --------------------

        static void HandleImage(Paragraph para, string key, string value, MainDocumentPart mainPart)
        {
            string base64Image = CleanBase64(value);
            // Center the image in the cell using paragraph justification
            para.ParagraphProperties ??= new ParagraphProperties();
            para.ParagraphProperties.Justification = new Justification
            {
                Val = JustificationValues.Center
            };

            // Locate the table cell (if any)
            var cell = para.Ancestors<TableCell>().FirstOrDefault();

            // Decode base64
            byte[] imgBytes = Convert.FromBase64String(base64Image);

            bool isWebp = false;

            if (imgBytes.Length > 12 && Encoding.ASCII.GetString(imgBytes, 0, 4) == "RIFF" && Encoding.ASCII.GetString(imgBytes, 8, 4) == "WEBP")
            {
                isWebp = true;
            }
            else
            {
                isWebp = false;
            }

            // Add image part
            var imgPart = mainPart.AddImagePart(ImagePartType.Png); // change if PNG etc.
            using (var s = imgPart.GetStream(FileMode.Create, FileAccess.Write))
                s.Write(imgBytes, 0, imgBytes.Length);

            string relId = mainPart.GetIdOfPart(imgPart);

            // Compute target width: inner width of cell (safe for Word Online)
            // Subtract a little padding so it doesn't overflow even if borders/margins exist.
            long innerWidthEmu = GetSafeCellInnerWidthEmu(cell);
            long cx = (long)(innerWidthEmu * 0.98); // 98% of cell inner width

            var (width, height, verRes, horRes) = (100, 100, 96f, 96f);

            if (isWebp)
            {
                (width, height, verRes, horRes) = GetWebPDimensions(imgBytes);
            }
            else
            {
                (width, height, verRes, horRes) = GetImageDimensions(imgBytes);
            }

            //If Image size is smaller then lets not scale it up, just use the original size
            if (width > 0 && height > 0)
            {
                long originalWidthEmu = InchesToEmu(width / horRes);
                long originalHeightEmu = InchesToEmu(height / verRes);
                if (originalWidthEmu < cx)
                {
                    cx = originalWidthEmu;
                }
            }

            // Preserve aspect ratio based on image intrinsic size
            double ar = (width > 0 && height > 0) ? (double)width / height : 1.0; // width/height
            long cy = (long)(cx / ar);

            // Remove placeholder text and get insertion position
            var insertBeforeRun = RemovePlaceholderAndGetInsertionRun(para, key);

            // Insert inline image
            var imageRun = BuildImageRun(relId, cx, cy);
            if (insertBeforeRun != null)
                insertBeforeRun.InsertBeforeSelf(imageRun);
            else
                para.AppendChild(imageRun);
        }

        private static (int Width, int Height, float verRes, float horRes) GetWebPDimensions(byte[] bytes)
        {
            if (bytes.Length < 30)
                throw new InvalidOperationException("Invalid WebP");

            string riff = Encoding.ASCII.GetString(bytes, 0, 4);
            string webp = Encoding.ASCII.GetString(bytes, 8, 4);
            string chunk = Encoding.ASCII.GetString(bytes, 12, 4);

            if (chunk == "VP8X")
            {
                int width =
                    1 +
                    bytes[24] +
                    (bytes[25] << 8) +
                    (bytes[26] << 16);

                int height =
                    1 +
                    bytes[27] +
                    (bytes[28] << 8) +
                    (bytes[29] << 16);

                return (width, height, 96f, 96f);
            }

            // VP8 (lossy)
            if (chunk == "VP8 ")
            {
                int start = 20;

                // Verify frame header
                if (bytes[start + 3] != 0x9D ||
                    bytes[start + 4] != 0x01 ||
                    bytes[start + 5] != 0x2A)
                {
                    throw new InvalidOperationException("Invalid VP8 frame");
                }

                int width =
                    bytes[start + 6] |
                    ((bytes[start + 7] & 0x3F) << 8);

                int height =
                    bytes[start + 8] |
                    ((bytes[start + 9] & 0x3F) << 8);

                return (width, height, 96f, 96f);
            }

            throw new NotSupportedException($"WebP type {chunk} not supported");
        }

        private static (int Width, int Height, float verRes, float horRes) GetImageDimensions(byte[] imgBytes)
        {
            using var ms = new MemoryStream(imgBytes);
            using var img = System.Drawing.Image.FromStream(ms, useEmbeddedColorManagement: false, validateImageData: false);
            return (img.Width, img.Height, img.VerticalResolution, img.HorizontalResolution);
        }

        static void HandleWordFileContent(Paragraph para, string key, string value, MainDocumentPart mainPart)
        {
            if (para == null || string.IsNullOrEmpty(value) || mainPart == null)
                return;

            byte[] byteArray;

            try
            {
                byteArray = Convert.FromBase64String(value);
            }
            catch
            {
                // Invalid Base64, just exit safely
                return;
            }

            // Create unique AltChunk Id
            string altChunkId = "AltChunkId_" + Guid.NewGuid().ToString("N");

            // Add AltChunk part
            var altPart = mainPart.AddAlternativeFormatImportPart(
                AlternativeFormatImportPartType.WordprocessingML,
                altChunkId);

            using (var stream = new MemoryStream(byteArray))
            {
                altPart.FeedData(stream);
            }

            // Create AltChunk element
            var altChunk = new AltChunk() { Id = altChunkId };

            // Insert after current paragraph
            para.Parent.InsertAfter(altChunk, para);

            // Remove placeholder paragraph
            para.Remove();
        }


        static void HandleHTML(Paragraph para, string key, string value, MainDocumentPart mainPart)
        {
            var converter = new HtmlConverter(mainPart)
            {
                ImageProcessing = ImageProcessingMode.EmbedDataUriOnly,
                SupportsAnchorLinks = true
            };

            string html = Encoding.UTF8.GetString(Convert.FromBase64String(value));
            if (html.Contains("=\"/sites/O365-", StringComparison.OrdinalIgnoreCase))
                html = html.Replace("=\"/sites/O365-", "=\"https://skfgroup.sharepoint.com/sites/O365-", StringComparison.OrdinalIgnoreCase);

            string decodedHtml = WebUtility.HtmlDecode(html);
            decodedHtml = decodedHtml.Replace("</a><a", "</a><br/><a", StringComparison.OrdinalIgnoreCase);
            var elements = converter.Parse(decodedHtml);
            para.RemoveAllChildren();
            OpenXmlElement last = para;
            foreach (var el in elements)
            {
                var clone = el.CloneNode(true);

                // Resize image based on cell width
                try
                {
                    foreach (var drawing in clone.Descendants<Drawing>())
                    {
                        var cell = para.Ancestors<TableCell>().FirstOrDefault();

                        if (cell != null)
                        {
                            var tcWidth = cell.TableCellProperties?.GetFirstChild<TableCellWidth>();
                            if (tcWidth != null && tcWidth.Type != null && tcWidth.Type == TableWidthUnitValues.Dxa)
                            {
                                long cellWidthTwips = long.Parse(tcWidth.Width.Value);
                                long cellWidthEmu = cellWidthTwips * 600;

                                var wpExtent = drawing.Descendants<DW.Extent>().FirstOrDefault();
                                var aExtent = drawing.Descendants<A.Extents>().FirstOrDefault();

                                if (wpExtent.Cx > cellWidthEmu)
                                {
                                    double ratio = (double)cellWidthEmu / wpExtent.Cx;

                                    long newCx = cellWidthEmu;
                                    long newCy = (long)(wpExtent.Cy * ratio);

                                    wpExtent.Cx = newCx;
                                    wpExtent.Cy = newCy;

                                    if (aExtent != null)
                                    {
                                        aExtent.Cx = newCx;
                                        aExtent.Cy = newCy;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                para.Parent.InsertAfter(clone, last);
                last = clone;
            }
        }

        public static void HandleTable(Paragraph para, string key, string value)
        {
            string json = Encoding.UTF8.GetString(Convert.FromBase64String(value));
            JArray tableData = JArray.Parse(json);

            if (!tableData.Any())
            {
                para.InsertAfterSelf(new Paragraph(new Run(new Text("No data available"))));
                para.Remove();
                return;
            }

            Table table = new Table();

            // ✅ Table properties
            TableProperties tblProps = new TableProperties(
                new TableWidth { Width = "5000", Type = TableWidthUnitValues.Pct },
                new TableBorders(
                    new TopBorder { Val = BorderValues.Single, Size = 6 },
                    new BottomBorder { Val = BorderValues.Single, Size = 6 },
                    new LeftBorder { Val = BorderValues.Single, Size = 6 },
                    new RightBorder { Val = BorderValues.Single, Size = 6 },
                    new InsideHorizontalBorder { Val = BorderValues.Single, Size = 6 },
                    new InsideVerticalBorder { Val = BorderValues.Single, Size = 6 }
                )
            );

            table.AppendChild(tblProps);

            // ✅ Dynamic columns
            var columns = tableData
                .SelectMany(x => ((JObject)x).Properties().Select(p => p.Name))
                .Distinct()
                .ToList();

            int colCount = columns.Count;

            // ✅ TableGrid (REQUIRED for stability)
            TableGrid grid = new TableGrid();
            int totalTableWidth = 9000;

            // Calculate column weights
            var columnWeights = new List<int>();

            foreach (var col in columns)
            {
                // Prevent extremely small columns - 6 characters minimum for header or data
                int maxLength = 6;
                // Check data rows for max length skiping for hearder row
                foreach (JObject item in tableData.Skip(1))
                {
                    string rowValue = item[col]?.ToString() ?? "";
                    maxLength = Math.Max(maxLength, rowValue.Length);
                }
                // Ensures that a big text should not make the column too wide, so we limit it to 20 characters
                maxLength = Math.Min(maxLength, 20);
                columnWeights.Add(maxLength);
            }

            int totalWeight = columnWeights.Sum();

            for (int i = 0; i < columns.Count; i++)
            {
                int width = (columnWeights[i] * totalTableWidth) / totalWeight;

                grid.Append(new GridColumn()
                {
                    Width = width.ToString()
                });
            }
            table.Append(grid);

            // ✅ Header row
            TableRow headerRow = new TableRow();
            foreach (var col in columns)
            {
                headerRow.Append(CreateSafeCell(col, true));
            }
            table.AppendChild(headerRow);

            // ✅ Data rows
            foreach (JObject item in tableData)
            {
                TableRow row = new TableRow();

                foreach (var col in columns)
                {
                    string val = item[col]?.ToString() ?? "";
                    row.Append(CreateSafeCell(val, false));
                }

                table.AppendChild(row);
            }

            var parent = para.Parent;
            parent.InsertAfter(table, para);
            para.Remove();
        }

        private static TableCell CreateSafeCell(string text, bool isHeader)
        {
            // ✅ Sanitize text (VERY IMPORTANT)
            text = CleanInvalidXmlChars(text);

            Text txt = new Text(text)
            {
                Space = SpaceProcessingModeValues.Preserve
            };

            Run run = new Run(txt);

            if (isHeader)
            {
                run.RunProperties = new RunProperties(new Bold());
            }

            Paragraph para = new Paragraph(run);

            // ✅ REQUIRED properties (prevents corruption)
            TableCellProperties cellProps = new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Auto }
            );

            TableCell cell = new TableCell();
            cell.Append(cellProps);
            cell.Append(para);

            return cell;
        }

        private static string CleanInvalidXmlChars(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";

            return new string(text.Where(c =>
                XmlConvert.IsXmlChar(c)
            ).ToArray());
        }

        static void HandleParagraph(Paragraph para, string key, string value)
        {
            var runs = para.Descendants<Run>().ToList();
            if (runs.Count == 0) { return; }
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

        // Removes data URI prefix + whitespace/newlines
        static string CleanBase64(string b64)
        {
            if (string.IsNullOrWhiteSpace(b64)) return b64;

            int idx = b64.IndexOf("base64,", StringComparison.OrdinalIgnoreCase);
            if (idx >= 0)
                b64 = b64[(idx + "base64,".Length)..];

            return Regex.Replace(b64, @"\s+", "");
        }

        static long CmToEmu(double cm) => (long)(cm * 360000.0); // 1 cm = 360,000 EMUs
        static long InchesToEmu(double inches) => (long)(inches * 914400.0);

        /// <summary>
        /// Returns a conservative inner width of the cell (in EMUs), avoiding tcMar mutation (Word Online-friendly).
        /// If the cell width is not specified, falls back to table grid column width or a safe default.
        /// </summary>
        static long GetSafeCellInnerWidthEmu(TableCell? cell)
        {
            // Fallback: 8 inches (safe for most cases)
            long fallback = InchesToEmu(8);

            if (cell == null)
                return fallback;

            long bestWidthEmu = long.MaxValue;

            // 1. Cell's own TCW (most accurate)
            var tcW = cell.TableCellProperties?.TableCellWidth;
            if (tcW != null && tcW.Width != null && tcW.Type == TableWidthUnitValues.Dxa)
            {
                if (int.TryParse(tcW.Width, out int wTwips))
                {
                    double inches = wTwips / 1440.0;
                    long emu = InchesToEmu(inches);
                    bestWidthEmu = Math.Min(bestWidthEmu, emu);
                }
            }

            // 2. Table grid column width (alternative source)
            var row = cell.Ancestors<TableRow>().FirstOrDefault();
            var table = cell.Ancestors<Table>().FirstOrDefault();
            if (row != null && table?.TableGrid != null)
            {
                int index = row.Elements<TableCell>().ToList().IndexOf(cell);
                var cols = table.TableGrid.Elements<GridColumn>().ToList();

                if (index >= 0 && index < cols.Count)
                {
                    if (cols[index].Width != null && int.TryParse(cols[index].Width, out int wTwips))
                    {
                        double inches = wTwips / 1440.0;
                        long emu = InchesToEmu(inches);
                        bestWidthEmu = Math.Max(bestWidthEmu, emu);
                    }
                }
            }

            // 3. Nothing found: fallback
            if (bestWidthEmu == long.MaxValue)
                bestWidthEmu = fallback;

            // FINAL: apply safe shrink factor
            return (long)(bestWidthEmu * 0.96);   // <-- CRITICAL: prevents overflow
        }

        /// <summary>
        /// Removes the first occurrence of 'placeholder' (across runs) and returns the run before which to insert the image.
        /// </summary>
        static Run? RemovePlaceholderAndGetInsertionRun(Paragraph paragraph, string placeholder)
        {
            var runs = paragraph.Elements<Run>().ToList();
            if (!runs.Any()) return null;

            // Collect text nodes
            var textNodes = new List<(Run run, Text text)>();
            foreach (var run in runs)
                foreach (var text in run.Elements<Text>())
                    textNodes.Add((run, text));

            // Build flat string and index map
            var sb = new StringBuilder();
            var indexMap = new List<(Run run, Text text, int charIndex)>();
            foreach (var (run, text) in textNodes)
            {
                var val = text.Text ?? string.Empty;
                for (int i = 0; i < val.Length; i++)
                {
                    sb.Append(val[i]);
                    indexMap.Add((run, text, i));
                }
            }

            var flat = sb.ToString();
            int idx = flat.IndexOf(placeholder, StringComparison.Ordinal);
            if (idx < 0) return null;

            // Remove placeholder characters from their respective Text nodes
            int length = placeholder.Length;
            var involved = indexMap.Skip(idx).Take(length).ToList();
            var byText = involved.GroupBy(x => x.text);
            foreach (var group in byText)
            {
                var t = group.Key;
                var original = t.Text ?? string.Empty;
                var removePositions = new HashSet<int>(group.Select(g => g.charIndex));
                var rebuilt = new StringBuilder(original.Length - removePositions.Count);
                for (int i = 0; i < original.Length; i++)
                    if (!removePositions.Contains(i))
                        rebuilt.Append(original[i]);
                t.Text = rebuilt.ToString();
            }

            // Insert image before the run that contained the first char of the placeholder
            return involved.First().run;
        }

        /// <summary>
        /// Build a Run that contains an inline Drawing referencing the image relationship.
        /// </summary>
        static Run BuildImageRun(string relationshipId, long cx, long cy)
        {
            var drawing =
                new Drawing(
                    new DW.Inline(
                        new DW.Extent() { Cx = cx, Cy = cy },
                        new DW.EffectExtent() { LeftEdge = 0, TopEdge = 0, RightEdge = 0, BottomEdge = 0 },
                        new DW.DocProperties() { Id = 1U, Name = "Inserted Picture" },
                        new DW.NonVisualGraphicFrameDrawingProperties(new A.GraphicFrameLocks() { NoChangeAspect = true }),
                        new A.Graphic(
                            new A.GraphicData(
                                new PIC.Picture(
                                    new PIC.NonVisualPictureProperties(
                                        new PIC.NonVisualDrawingProperties() { Id = 0U, Name = "Picture" },
                                        new PIC.NonVisualPictureDrawingProperties()),
                                    new PIC.BlipFill(
                                        new A.Blip() { Embed = relationshipId, CompressionState = A.BlipCompressionValues.Print },
                                        new A.Stretch(new A.FillRectangle())),
                                    new PIC.ShapeProperties(
                                        new A.Transform2D(
                                            new A.Offset() { X = 0, Y = 0 },
                                            new A.Extents() { Cx = cx, Cy = cy }),
                                        new A.PresetGeometry(new A.AdjustValueList())
                                        { Preset = A.ShapeTypeValues.Rectangle })))
                            { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                    )
                    {
                        DistanceFromTop = 0U,
                        DistanceFromBottom = 0U,
                        DistanceFromLeft = 0U,
                        DistanceFromRight = 0U
                    });

            return new Run(drawing);
        }

    }
}