using CommonDocumentProcessing.Common;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace CommonDocumentProcessing
{
    /// <summary>
    /// This function is used by Print function which prepares a word template (with placeholders) 
    /// with the incoming values (text/images)
    /// </summary>
    public static class CommonExcelHandler
    {
        public static string ProcessExcelDocument(APIRequest req)
        {
            string resultWordDoc = string.Empty;
            byte[] byteArray = Convert.FromBase64String(req.InputFileAsBase64);

            using (var stream = new MemoryStream())
            {
                stream.Write(byteArray, 0, byteArray.Length);
                stream.Position = 0;

                using (SpreadsheetDocument doc = SpreadsheetDocument.Open(stream, true))
                {
                    var workbookPart = doc.WorkbookPart;
                    var sharedStringPart = workbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();

                    if (sharedStringPart != null)
                    {
                        foreach (SharedStringItem item in sharedStringPart.SharedStringTable.Elements<SharedStringItem>())
                        {
                            foreach (Bookmark bookmark in req.Bookmarks)
                            {
                                if (bookmark.BookmarkType == "Image")
                                {
                                    throw new NotImplementedException();
                                }
                                else
                                {
                                    if (item.InnerText.Contains(bookmark.BookmarkName))
                                    {
                                        item.Text = new Text(item.InnerText.Replace(bookmark.BookmarkName, bookmark.BookmarkValue));
                                    }
                                }
                            }
                        }
                        sharedStringPart.SharedStringTable.Save();
                    }
                }

                stream.Position = 0;
                resultWordDoc = Convert.ToBase64String(stream.ToArray());
            }
            return resultWordDoc;
        }
    }
}