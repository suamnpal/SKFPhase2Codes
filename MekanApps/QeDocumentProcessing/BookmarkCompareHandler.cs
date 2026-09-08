using QeDynamicDocumentProcessing.Common;
using System.Text.Json;

namespace QeDynamicDocumentProcessing;

public class BookmarkCompareHandler
{
    public static BookmarkCompareResponse CompareBookmarks(BookmarkCompareRequest req)
    {
        BookmarkCompareResponse response = new()
        {
            UpdateRequired = false
        };

        var templateBookmarks = req.TemplateBookmark;
        var documentBookmarks = req.DocumentBookmark;

        // --------------------------------------------------
        // ✅ 1. ADD missing (template → document)
        // --------------------------------------------------
        var documentBookmarkKeys = documentBookmarks
            .Select(d => d.BookmarkName?.Trim())
            .Where(name => !string.IsNullOrEmpty(name))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var templateBookmark in templateBookmarks.Where(t => t != null && !string.IsNullOrWhiteSpace(t.BookmarkName)))
        {
            var name = templateBookmark.BookmarkName.Trim();
            if (!documentBookmarkKeys.Contains(name))
            {
                documentBookmarks.Add(new Bookmark
                {
                    BookmarkName = name,
                    BookmarkValue = templateBookmark.BookmarkValue?.Trim() ?? string.Empty
                });

                documentBookmarkKeys.Add(name);
                response.UpdateRequired = true;
            }
        }

        // --------------------------------------------------
        // ✅ 2. REMOVE extra (document → not in template)
        // --------------------------------------------------

        // ✅ Build template name set
        var templateNames = templateBookmarks
            .Where(t => t != null && !string.IsNullOrWhiteSpace(t.BookmarkName))
            .Select(t => t.BookmarkName.Trim())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var filteredDocumentBookmarks = documentBookmarks
            .Where(d =>
                !string.IsNullOrWhiteSpace(d.BookmarkName) &&
                templateNames.Contains(d.BookmarkName.Trim()))
            .ToList();

        // ✅ Detect if anything was removed
        if (filteredDocumentBookmarks.Count != documentBookmarks.Count)
        {
            response.UpdateRequired = true;
            documentBookmarks = filteredDocumentBookmarks;
        }

        // --------------------------------------------------
        // ✅ Final serialization
        // --------------------------------------------------
        response.NewDocumentBookmark = response.UpdateRequired
            ? JsonSerializer.Serialize(documentBookmarks)
            : string.Empty;

        return response;
    }

    public static BookmarkCompareResponse CompareBookmarksAndValues(BookmarkCompareRequest req)
    {
        BookmarkCompareResponse response = CompareBookmarks(req);

        if(response.UpdateRequired)
        {
            return response;
        }
        // ✅ Detect if any Value was changed
        response.UpdateRequired = req.TemplateBookmark.Any(tb =>
             req.DocumentBookmark.Any(db =>
                 string.Equals(
                     db.BookmarkName?.Trim(),
                     tb.BookmarkName?.Trim()) &&
                 !string.Equals(
                     db.BookmarkValue,
                     tb.BookmarkValue)));
        return response;
    }
}
