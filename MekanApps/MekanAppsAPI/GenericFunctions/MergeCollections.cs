using CommonDocumentProcessing.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace MekanAppsAPI.GenericFunctions;

public class MergeCollections
{
    private readonly ILogger<MergeCollections> _logger;

    public MergeCollections(ILogger<MergeCollections> logger)
    {
        _logger = logger;
    }

    [Function("MergeCollections")]
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

            var list1 = body.GetProperty("List1").EnumerateArray().ToList();
            var list2 = body.GetProperty("List2").EnumerateArray().ToList();

            return new OkObjectResult(list1.Concat(list2));
        }
        catch (Exception ex)
        {
            response.ResultStatus = "Failure";
            response.ErrorDetails = ex.Message;
        }
        return new OkObjectResult(response);
    }
}