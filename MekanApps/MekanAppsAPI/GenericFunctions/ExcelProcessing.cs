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
/// This function is used by Qe Static excel document generation function which prepares a excel template (with placeholders) 
/// with the incoming values (text only)
/// </summary>
public class ExcelProcessing
{
    private readonly ILogger<ExcelProcessing> _logger;

    public ExcelProcessing(ILogger<ExcelProcessing> logger)
    {
        _logger = logger;
    }

    [Function("ExcelDocumentProcessing")]
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
            response.OutputFileAsBase64 = CommonExcelHandler.ProcessExcelDocument(convertRequest);
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