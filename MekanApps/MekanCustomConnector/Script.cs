using JsonExtensionDataAttribute = Newtonsoft.Json.JsonExtensionDataAttribute;
//Only copy the entire below portion...

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Web;

public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        string op = this.Context.OperationId;

        // Call the backend as configured in the connector operation
        var backendResponse = await this.Context.SendAsync(this.Context.Request, this.CancellationToken);
        string raw = await backendResponse.Content.ReadAsStringAsync();

        switch (op)
        {
            case "EdbSearchQuery":
                return HandleSearchQuery(raw);

            case "EdbGetDistinctValues":
            case "CCGetDistinctValues":
            case "IpdGetDistinctValues":
            case "PPCGetDistinctValues":
                return HandleDistinctValues(raw);

            default:
                // Passthrough for any other operations
                var passthrough = new HttpResponseMessage(backendResponse.StatusCode);
                passthrough.Content = CreateJsonContent(raw);
                return passthrough;
        }
    }

    // -------------------------------------------------
    // A) Search API handler: extracts only Ids from inner 'response'
    // -------------------------------------------------
    private HttpResponseMessage HandleSearchQuery(string raw)
    {
        var outer = SafeDeserialize<SearchOuter>(raw);
        if (outer == null || string.IsNullOrWhiteSpace(outer.response))
            return CreateOk(new { Ids = new string[0] });

        var inner = SafeDeserialize<SearchInner>(outer.response);
        if (inner?.Value == null)
            return CreateOk(new { Ids = new string[0] });

        var ids = inner.Value
                       .Where(v => v != null && !string.IsNullOrWhiteSpace(v.Id))
                       .Select(v => v.Id)
                       .Distinct()
                       .ToList();

        return CreateOk(new { Ids = ids });
    }

    // -------------------------------------------------
    // B) Distinct values handler (OData groupby): handles ANY column
    //    Option A: Generic rows via JsonExtensionData
    // -------------------------------------------------
    private HttpResponseMessage HandleDistinctValues(string raw)
    {
        var payload = SafeDeserialize<ODataGroupByResponse>(raw);
        if (payload?.value == null)
            return CreateOk(new { Values = new string[0] });

        // Optional: Allow caller to specify preferred column names via header:
        //   x-preferred-columns: mekan_product,mekan_departmenttext
        var preferred = ReadPreferredColumnsFromHeader(); // may be empty

        // Extract values from whichever column(s) exist per row
        var values = ExtractColumnValues(payload, preferred.ToArray())
                        .Select(CleanValue)  // remove {crmhit} markers, unescape quotes, normalize spaces
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .Distinct()
                        .ToList();

        return CreateOk(new { 
            Values = values,
            NextLink = string.IsNullOrEmpty(payload?.NextLink)? null : ExtractSkipToken(payload.NextLink)
        });
    }

    // -------------------------------------------------
    // Helpers
    // -------------------------------------------------

    // Extract text values from generic rows using preferred names if provided;
    // Otherwise, fall back to the "first non-empty" field in each row.
    private static IEnumerable<string> ExtractColumnValues(ODataGroupByResponse payload, params string[] preferredColumnNames)
    {
        if (payload?.value == null)
            yield break;

        var preferred = new HashSet<string>(
            (preferredColumnNames ?? Enumerable.Empty<string>())
                .Where(s => !string.IsNullOrWhiteSpace(s)),
            StringComparer.OrdinalIgnoreCase
        );

        foreach (var row in payload.value)
        {
            if (row?.Fields == null || row.Fields.Count == 0)
                continue;

            // 1) Try preferred names (case-insensitive)
            if (preferred.Count > 0)
            {
                var kv = row.Fields.FirstOrDefault(kv2 => preferred.Contains(kv2.Key));
                if (kv.Value != null && kv.Value.Type != JTokenType.Null)
                {
                    var s = kv.Value.Type == JTokenType.String ? (string)kv.Value : kv.Value.ToString();
                    if (!string.IsNullOrWhiteSpace(s))
                    {
                        yield return s;
                        continue; // go to next row
                    }
                }
            }

            // 2) Fallback: first non-empty textual representation in the row
            var first = row.Fields
                           .Select(kv =>
                               kv.Value?.Type == JTokenType.String
                                   ? (string)kv.Value
                                   : kv.Value?.ToString())
                           .FirstOrDefault(s => !string.IsNullOrWhiteSpace(s));

            if (!string.IsNullOrWhiteSpace(first))
                yield return first;
        }
    }

    public static string ExtractSkipToken(string url)
    {
        var uri = new Uri(url);
        var query = HttpUtility.ParseQueryString(uri.Query);

        // This returns the raw encoded skiptoken
        string skipTokenEncoded = query["$skiptoken"];

        if (string.IsNullOrEmpty(skipTokenEncoded))
            return null;

        return skipTokenEncoded;
    }


    // Header "x-preferred-columns": comma-separated list of candidate names
    private IEnumerable<string> ReadPreferredColumnsFromHeader()
    {
        const string headerName = "x-preferred-columns";
        if (this.Context.Request.Headers.Contains(headerName))
        {
            var headerValues = this.Context.Request.Headers.GetValues(headerName);

            if (headerValues != null)
            {
                var raw = headerValues.FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(raw))
                {
                    return raw
                        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrWhiteSpace(s));
                }
            }
        }
        return Enumerable.Empty<string>();
    }

    // Cleaners: remove highlight markers, unescape quotes, normalize whitespace
    private string CleanValue(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        string v = input.Trim();

        // Remove Dataverse highlight markers (Search/Suggest APIs may include them)
        v = v.Replace("{crmhit}", "")
             .Replace("{/crmhit}", "")
             .Replace("<crmhit>", "")
             .Replace("</crmhit>", "");

        // Unescape \" → "
        v = v.Replace("\\\"", "\"");

        // Remove surrounding quotes if present
        if ((v.StartsWith("\"") && v.EndsWith("\"")) ||
            (v.StartsWith("\\\"") && v.EndsWith("\\\"")))
        {
            v = v.Trim('\"').Trim('\\').Trim();
        }

        // Normalize multi-space to single space
        v = System.Text.RegularExpressions.Regex.Replace(v, @"\s+", " ").Trim();

        return v;
    }

    private T SafeDeserialize<T>(string json)
    {
        if (string.IsNullOrWhiteSpace(json)) return default(T);
        try { return JsonConvert.DeserializeObject<T>(json); }
        catch { return default(T); }
    }

    private HttpResponseMessage CreateOk(object obj)
    {
        string json = JsonConvert.SerializeObject(obj);
        var resp = new HttpResponseMessage(HttpStatusCode.OK);
        resp.Content = CreateJsonContent(json);
        return resp;
    }

    // -------------------------------------------------
    // Models
    // -------------------------------------------------

    // SEARCH OUTER: { "@odata.context": "...", "response": "<JSON STRING>" }
    public class SearchOuter
    {
        public string response { get; set; }
    }

    // SEARCH INNER: { "Error": ..., "Value": [{ "Id": "...", ... }], "Count": n }
    public class SearchInner
    {
        public object Error { get; set; }
        public List<SearchValue> Value { get; set; }
        public int Count { get; set; }
    }

    public class SearchValue
    {
        public string Id { get; set; }
    }

    // ODATA GROUPBY RESPONSE: { "value": [ { <ANY_FIELD>: "...", "Count": 123? }, ... ] }
    public class ODataGroupByResponse
    {
        public List<GenericRow> value { get; set; }

        // OPTIONAL: may be missing
        [JsonProperty("@odata.nextLink")]
        public string NextLink { get; set; }

    }

    public class GenericRow
    {
        // Captures ANY unknown properties into a dictionary
        [JsonExtensionData]
        public IDictionary<string, JToken> Fields { get; set; } = new Dictionary<string, JToken>(StringComparer.OrdinalIgnoreCase);
    }
}
