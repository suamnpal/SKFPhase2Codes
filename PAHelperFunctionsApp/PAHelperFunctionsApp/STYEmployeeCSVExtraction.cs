using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.Globalization;
using System.Net;
using System.Text;
using Azure.Core.Serialization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using PAHelperFunctionsApp.HelperFunctions;

namespace PAHelperFunctionsApp
{
    public class STYEmployeeCSVExtraction
    {
        private readonly ILogger _logger;
        private readonly JsonObjectSerializer _jsonSerializer;

        public STYEmployeeCSVExtraction(ILoggerFactory loggerFactory, JsonObjectSerializer jsonSerializer)
        {
            _logger = loggerFactory.CreateLogger<STYEmployeeCSVExtraction>();
            _jsonSerializer = jsonSerializer;
        }

        [Function("STYEmployeeCSVExtraction")]
        public static async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            string body = await (new StreamReader(req.Body)).ReadToEndAsync();
            CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture);
            config.DetectDelimiter = true;
            config.HasHeaderRecord = false;
            var response = req.CreateResponse(HttpStatusCode.OK);
            using (var csv = new CsvReader(new StringReader(body), config))
            {
                var records = csv.GetRecords<dynamic>().ToList().Select(d => d as IDictionary<string, object>);
                List<object[]> res = new List<object[]>();
                res.AddRange(records.Select(x => x.Values.Where(y=>y.ToString()!=";").ToArray()));
                await response.WriteAsJsonAsync(res);
            }
            return response;
        }
    }
}
