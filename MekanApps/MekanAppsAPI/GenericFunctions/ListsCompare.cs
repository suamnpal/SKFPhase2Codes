using CommonDocumentProcessing.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace MekanAppsAPI.GenericFunctions;

/// <summary>
/// Flow Extension function : Compare two large lists
/// </summary>
public class ListsCompare
{
    private readonly ILogger<ListsCompare> _logger;

    public ListsCompare(ILogger<ListsCompare> logger)
    {
        _logger = logger;
    }

    [Function("CompareLists")]
    public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req, ClaimsPrincipal principal)
    {
        if (!req.Headers.TryGetValue("X-MS-CLIENT-PRINCIPAL", out var header))
        {
            return new UnauthorizedResult();
        }

        APIResponse response = new APIResponse();
        try
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var body = JsonSerializer.Deserialize<JsonElement>(requestBody);

            var sourceList = body.GetProperty("SourceData").EnumerateArray().ToList();
            var targetList = body.GetProperty("TargetData").EnumerateArray().ToList();
            var compareColumns = body.GetProperty("CompareColumns")
                                     .EnumerateArray()
                                     .Select(c => c.GetString())
                                     .ToList();

            // Create a composite key for each row based on input columns to compare
            string BuildCompositeKey(JsonElement item)
            {
                return string.Join("|", compareColumns.Select(col =>
                {
                    if (!item.TryGetProperty(col, out var value))
                        return ""; // Missing properties become empty strings
                    return value.ToString();
                }));
            }

            var sourceDict = sourceList.ToDictionary(item => BuildCompositeKey(item));
            var targetDict = targetList.ToDictionary(item => BuildCompositeKey(item));

            // Items to add (exist in source but not in target)
            var toAdd = sourceDict
                .Where(x => !targetDict.ContainsKey(x.Key))
                .Select(x => x.Value)
                .ToList();

            // Items to delete (exist in target but not in source)
            var toDelete = targetDict
                .Where(x => !sourceDict.ContainsKey(x.Key))
                .Select(x => x.Value)
                .ToList();

            return new OkObjectResult(new
            {
                ToAdd = toAdd,
                ToDelete = toDelete
            });
        }
        catch (Exception ex)
        {
            response.ResultStatus = "Failure";
            response.ErrorDetails = ex.Message;
        }
        return new OkObjectResult(response);
    }
}