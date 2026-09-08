using CommonDocumentProcessing;
using CommonDocumentProcessing.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Security.Claims;

namespace MekanAppsAPI.GenericFunctions;

/// <summary>
/// This function is used by App Print function & also by Qe Static word document generation function which prepares a word template (with placeholders) 
/// with the incoming values (text/images)
/// </summary>
public class WordProcessing
{
    private readonly ILogger<WordProcessing> _logger;

    public WordProcessing(ILogger<WordProcessing> logger)
    {
        _logger = logger;
    }

    [Function("WordDocumentProcessing")]
    public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req, ClaimsPrincipal principal)
    {
        if (!req.Headers.TryGetValue("X-MS-CLIENT-PRINCIPAL", out var header))
        {
            return new UnauthorizedResult();
        }

        APIResponse response = new APIResponse();
        try
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var convertRequest = JsonConvert.DeserializeObject<APIRequest>(requestBody);
            response.OutputFileAsBase64 = CommonWordHandler.ProcessWordDocument(convertRequest);
            response.ResultStatus = "Success";
        }
        catch (Exception ex)
        {
            response.ResultStatus = "Failure";
            response.ErrorDetails = ex.Message;
        }
        return new OkObjectResult(response);
    }
}