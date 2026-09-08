using CommonDocumentProcessing.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace MekanAppsAPI.GenericFunctions;

/// <summary>
/// Flow Extension function : Compare two large lists
/// </summary>
public class GenerateFileName
{
    private readonly ILogger<GenerateFileName> _logger;

    public GenerateFileName(ILogger<GenerateFileName> logger)
    {
        _logger = logger;
    }

    [Function("GenerateFileName")]
    public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req, ClaimsPrincipal principal)
    {
        if (!req.Headers.TryGetValue("X-MS-CLIENT-PRINCIPAL", out var header))
        {
            return new UnauthorizedResult();
        }

        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        return new OkObjectResult(Slugify(requestBody));
    }

    private static string Slugify(string text)
    {
        // Remove accents (Göteborg ? goteborg)
        var normalized = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (var c in normalized)
        {
            if (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c)
                != System.Globalization.UnicodeCategory.NonSpacingMark)
            {
                sb.Append(c);
            }
        }

        text = sb.ToString().Normalize(NormalizationForm.FormC);

        // Replace non-alphanumeric characters with hyphens
        text = Regex.Replace(text, @"[^A-Za-z0-9]+", "-");

        // Trim hyphens
        return text.Trim('-');
    }
}