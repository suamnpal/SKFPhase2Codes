using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using Azure.Core.Serialization;
using CsvHelper;
using CsvHelper.Configuration;
using ExcelDataReader;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using PAHelperFunctionsApp.HelperFunctions;

namespace PAHelperFunctionsApp
{
    public class ProductReleaseTemplateExtraction
    {
        private readonly ILogger _logger;
        private readonly JsonObjectSerializer _jsonSerializer;

        public ProductReleaseTemplateExtraction(ILoggerFactory loggerFactory, JsonObjectSerializer jsonSerializer)
        {
            _logger = loggerFactory.CreateLogger<STYEmployeeCSVExtraction>();
            _jsonSerializer = jsonSerializer;
        }

        [Function("ProductReleaseTemplateExtraction")]
        public static async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            string body = await (new StreamReader(req.Body)).ReadToEndAsync();
            CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture);
            config.DetectDelimiter = true;
            config.HasHeaderRecord = false;
            var response = req.CreateResponse(HttpStatusCode.OK);

            byte[] data = System.Convert.FromBase64String(body);
            MemoryStream ms = new MemoryStream(data);
            EvaluationResult evaluationResult = new EvaluationResult();

            using (var reader = ExcelReaderFactory.CreateReader(ms))
            {
                var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    UseColumnDataType = true,
                    FilterSheet = (tableReader, sheetIndex) => true,
                    ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                    {                       
                        FilterRow = (rowReader) =>
                        {
                            return rowReader.Depth != 0 && (bool)rowReader[0];
                        }
                    }
                });
                var statisticallyEvaluated = result.Tables[0];
                foreach (DataRow row in statisticallyEvaluated.Rows)
                {
                    evaluationResult.statisticallyEvaluated.Add(new StatisticallyEvaluated
                    {
                        Parameter = row[1].ToString() ?? string.Empty,
                        Description = row[2].ToString() ?? string.Empty,
                        Cp = row[3].ToString() ?? string.Empty,
                        CpkReq = row[4].ToString() ?? string.Empty
                    });
                }
                var parametersToBeVerified = result.Tables[1];
                foreach (DataRow row in parametersToBeVerified.Rows)
                {
                    evaluationResult.parametersToBeVerified.Add(new ParametersToBeVerified
                    {
                        Parameter = row[1].ToString() ?? string.Empty,
                        Description = row[2].ToString() ?? string.Empty
                    });
                }
            }
            await response.WriteAsJsonAsync(evaluationResult);
            return response;
        }

        public class EvaluationResult
        {
            public  EvaluationResult()
            {
                statisticallyEvaluated = new List<StatisticallyEvaluated>();
                parametersToBeVerified = new List<ParametersToBeVerified>();
            }

            public List<StatisticallyEvaluated> statisticallyEvaluated { get; set; }
            public List<ParametersToBeVerified> parametersToBeVerified { get; set; }
        }

        public class StatisticallyEvaluated
        {
            public string Parameter { get; set; }
            public string Description { get; set; }
            public string Cp { get; set; }
            public string CpkReq { get; set; }
        }

        public class ParametersToBeVerified
        {
            public string Parameter { get; set; }
            public string Description { get; set; }
        }
    }
}
