namespace CommonDocumentProcessing.Common
{
    public class APIRequest
    {
        public required string InputFileAsBase64 { get; set; }
        public List<Bookmark> Bookmarks { get; set; }
    }
}
