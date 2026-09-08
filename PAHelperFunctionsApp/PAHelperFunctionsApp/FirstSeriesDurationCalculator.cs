using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.Globalization;
using System.Net;
using System.Text;
using Azure.Core;
using Azure.Core.Serialization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PAHelperFunctionsApp.HelperFunctions;

namespace PAHelperFunctionsApp
{
    public class FirstSeriesDurationCalculator
    {
        private readonly ILogger _logger;
        private readonly JsonObjectSerializer _jsonSerializer;

        public FirstSeriesDurationCalculator(ILoggerFactory loggerFactory, JsonObjectSerializer jsonSerializer)
        {
            _logger = loggerFactory.CreateLogger<FirstSeriesDurationCalculator>();
            _jsonSerializer = jsonSerializer;
        }

        [Function("FirstSeriesDurationCalculator")]
        public static async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            string body = await (new StreamReader(req.Body)).ReadToEndAsync();
            FirstSeriesVersionHistoryInput? request = JsonConvert.DeserializeObject<FirstSeriesVersionHistoryInput>(body);

            var res = req.CreateResponse(HttpStatusCode.OK);
            FirstSeriesVersionHistoryOutput result = new FirstSeriesVersionHistoryOutput();

            if (request != null)
            {
                DateTime? plannedEndDate = ConvertToDate(request.PlannedEndDate);
                DateTime? actualEndDate = ConvertToDate(request.ActualEndDate);
                DateTime? allDate = ConvertToDate(request.StartDate);
                DateTime? processDate = ConvertToDate(request.ProcessDate);

                result.Difference = CalculateDuration(plannedEndDate, actualEndDate);
                result.Duration = CalculateDuration(allDate, actualEndDate);
                result.FullDuration = CalculateDuration(processDate, actualEndDate);
            }

            await res.WriteAsJsonAsync(result);
            return res;
        }

        static DateTime? ConvertToDate(string dateInput)
        {
            return DateTime.TryParseExact(dateInput, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt1) ? dt1.Date : null;
        }

        static int CalculateDuration(DateTime? date1, DateTime? date2)
        {
            if (date1 == null || date2 == null)
                return 0;

            if (date1.Value == date2.Value)
                return 0;

            var weekdays = 0;
            if (date1.Value < date2.Value)
            {
                for (var currentDate = date1.Value.Date; currentDate < date2.Value.Date; currentDate = currentDate.AddDays(1))
                {
                    if (IsWorkDay(currentDate))
                    {
                        weekdays++;
                    }
                }
            }
            else
            {
                for (var currentDate = date2.Value.Date; currentDate < date1.Value.Date; currentDate = currentDate.AddDays(1))
                {
                    if (IsWorkDay(currentDate))
                    {
                        weekdays--;
                    }
                }
            }
            return weekdays;
        }

        static bool IsWorkDay(DateTime date)
        {
            return (date.DayOfWeek is not DayOfWeek.Saturday
                and not DayOfWeek.Sunday)
                && !(date.Month == 1 && date.Day == 1)
                && !(date.Month == 5 && date.Day == 1)
                && !(date.Month == 12 && date.Day == 25)
                && !(date.Month == 12 && date.Day == 31);
        }
    }

    public class FirstSeriesVersionHistoryInput
    {
        public string PlannedEndDate { get; set; }
        public string ActualEndDate { get; set; }
        public string StartDate { get; set; }
        public string ProcessDate { get; set; }
    }

    public class FirstSeriesVersionHistoryOutput
    {
        public int Difference { get; set; }
        public int Duration { get; set; }
        public int FullDuration { get; set; }
    }
}
