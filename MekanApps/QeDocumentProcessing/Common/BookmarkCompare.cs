using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QeDynamicDocumentProcessing.Common
{
    public class BookmarkCompareRequest
    {
        public List<Bookmark> TemplateBookmark { get; set; }
        public List<Bookmark> DocumentBookmark { get; set; }
    }

    public class BookmarkCompareResponse
    {
        public bool UpdateRequired { get; set; }
        public string NewDocumentBookmark { get; set; }

        public string ErrorDetails { get; set; }
    }
}
