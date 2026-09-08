using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QeDynamicDocumentProcessing;
using QeDynamicDocumentProcessing.Common;
using System.Security.Claims;

namespace MekanAppsAPI.QeDynamicDocumentsProcessing;

/// <summary>
/// This function is only used for generating final dynamic word document out of Qe templates
/// There are several templates and logics - but the function is common for all
/// </summary>
public class QeDynamicDocsProcessing
{
    private readonly ILogger<QeDynamicDocsProcessing> _logger;

    public QeDynamicDocsProcessing(ILogger<QeDynamicDocsProcessing> logger)
    {
        _logger = logger;
    }

    [Function("GenerateQeDynamicDocument")]
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
            response.WordFileAsBase64 = WordHandler.ProcessWordTemplate(convertRequest);
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