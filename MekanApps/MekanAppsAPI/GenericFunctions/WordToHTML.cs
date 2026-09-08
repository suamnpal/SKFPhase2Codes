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
/// This function is used by Test Casting Apps to covert word document content into HTML equivalent
/// </summary>
public class WordToHTML
{
    private readonly ILogger<WordToHTML> _logger;

    public WordToHTML(ILogger<WordToHTML> logger)
    {
        _logger = logger;
    }

    [Function("WordToHTML")]
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
            response.OutputFileAsBase64 = WordToHTMLHandler.ProcessWordToHTML(convertRequest);
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