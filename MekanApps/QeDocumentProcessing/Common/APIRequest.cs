namespace QeDynamicDocumentProcessing.Common
{
    public class APIRequest
    {
        public string TemplateType { get; set; }
        public string MachineNumber { get; set; }
        public string ProductDesignation { get; set; }
        public string Version { get; set; }
        public string TemplateVersion { get; set; }
        public string Published { get; set; }
        public string CreatedBy { get; set; }
        public string ApprovedBy { get; set; }
        public string CategoryHierarchy { get; set; }
        public required string WordFileAsBase64 { get; set; }
        public List<Bookmark> Bookmarks { get; set; }
    }
}
