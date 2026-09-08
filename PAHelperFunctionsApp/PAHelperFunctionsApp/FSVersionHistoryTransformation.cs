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
    public class FSVersionHistoryTransformation
    {
        private readonly ILogger _logger;
        private readonly JsonObjectSerializer _jsonSerializer;

        public FSVersionHistoryTransformation(ILoggerFactory loggerFactory, JsonObjectSerializer jsonSerializer)
        {
            _logger = loggerFactory.CreateLogger<FSVersionHistoryTransformation>();
            _jsonSerializer = jsonSerializer;
        }

        [Function("FSVersionHistoryTransformation")]
        public static async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            string body = await (new StreamReader(req.Body)).ReadToEndAsync();
            var processDateStr = req.Query["processDate"];
            var response = req.CreateResponse(HttpStatusCode.OK);
            List<FSVersionHistory> res = new List<FSVersionHistory>();

            if (body.Contains(';'))
            {
                string[] dataRows = body.Split(';');
                foreach (string row in dataRows)
                {
                    if(row.Contains('#'))
                    {
                        FSVersionHistory history = new FSVersionHistory();
                        string[] columns = row.Split("#");

                        history.Title = columns[0];
                        DateTime? plannedDate = DateTime.TryParseExact(TakeOnlyDatePart(columns[1]), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt1) ? dt1.Date : null;
                        history.PlannedDate = plannedDate == null? "" : plannedDate.Value.ToString("dd/MM/yyyy");
                        DateTime? actualDate = DateTime.TryParseExact(TakeOnlyDatePart(columns[2]), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt2) ? dt2.Date : null;
                        history.ActualDate = actualDate == null ? "" : actualDate.Value.ToString("dd/MM/yyyy");
                        DateTime? allDate = DateTime.TryParseExact(TakeOnlyDatePart(columns[4]), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt3) ? dt3.Date : null;
                        DateTime? processDate = DateTime.TryParseExact(TakeOnlyDatePart(processDateStr), "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt4) ? dt4.Date : null;

                        string responsible = columns[3];
                        if (!String.IsNullOrEmpty(responsible))
                        {
                            history.Responsible = responsible.Replace("CN=", "").Replace("O=", "").Replace("OU=", "");
                        }

                        history.Difference = CalculateDuration(plannedDate, actualDate);
                        history.Duration = CalculateDuration(allDate, actualDate);
                        history.FullDuration = CalculateDuration(processDate, actualDate);
                        res.Add(history);
                    }
                }
            }
            await response.WriteAsJsonAsync(res);
            return response;
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

        static string TakeOnlyDatePart(string dateTime)
        {
            if (dateTime.Contains(" "))
                return dateTime.Split(' ')[0];
            else return dateTime;
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

    public class FSVersionHistory
    {
        public string Title { get; set; }
        public string PlannedDate { get; set; }
        public string ActualDate { get; set; }
        public int Difference { get; set; }
        public int Duration { get; set; }
        public int FullDuration { get; set; }
        public string Responsible { get; set; }
    }
}
