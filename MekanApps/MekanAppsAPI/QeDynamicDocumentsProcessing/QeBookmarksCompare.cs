using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QeDynamicDocumentProcessing;
using QeDynamicDocumentProcessing.Common;
using System.Security.Claims;

namespace MekanAppsAPI.QeBookmarksCompare;

/// <summary>
/// This function is used in Qe Power Apps for Syncing Bookmarks between template and associated documents
/// </summary>
public class QeBookmarksCompare
{
    private readonly ILogger<QeBookmarksCompare> _logger;

    public QeBookmarksCompare(ILogger<QeBookmarksCompare> logger)
    {
        _logger = logger;
    }

    [Function("QeBookmarksCompare")]
    public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req, ClaimsPrincipal principal)
    {
        if (!req.Headers.TryGetValue("X-MS-CLIENT-PRINCIPAL", out var header))
        {
            return new UnauthorizedResult();
        }

        BookmarkCompareResponse response = new BookmarkCompareResponse();
        try
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var compareRequest = JsonConvert.DeserializeObject<BookmarkCompareRequest>(requestBody);
            response = BookmarkCompareHandler.CompareBookmarks(compareRequest);
        }
        catch (Exception ex)
        {
            response.UpdateRequired = false;
            response.ErrorDetails = ex.StackTrace;
        }
        return new OkObjectResult(response);
    }
}