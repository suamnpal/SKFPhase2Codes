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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PAHelperFunctionsApp
{
    public class FirstSeriesPlannedEndDateCalculator
    {
        private readonly ILogger _logger;
        private readonly JsonObjectSerializer _jsonSerializer;

        public FirstSeriesPlannedEndDateCalculator(ILoggerFactory loggerFactory, JsonObjectSerializer jsonSerializer)
        {
            _logger = loggerFactory.CreateLogger<FirstSeriesPlannedEndDateCalculator>();
            _jsonSerializer = jsonSerializer;
        }

        [Function("FirstSeriesPlannedEndDateCalculator")]
        public static async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            string body = await (new StreamReader(req.Body)).ReadToEndAsync();
            List<FSPlannedEndDateCalculatorInput>? request = JsonConvert.DeserializeObject<List<FSPlannedEndDateCalculatorInput>>(body);

            var res = req.CreateResponse(HttpStatusCode.OK);
            List<FSPlannedEndDateCalculatorOutput> result = new List<FSPlannedEndDateCalculatorOutput>();

            if (request != null && request.Count > 0)
            {
                List<IGrouping<int, FSPlannedEndDateCalculatorInput>> groupResult = request.OrderBy(x => x.StepOrder).GroupBy(x => x.StepOrder).ToList();

                DateTime stepEndDate = DateTime.Now.Date;
                foreach (var group in groupResult)
                {
                    DateTime stepStartDate = stepEndDate;
                    List<FSPlannedEndDateCalculatorInput> stepRoles = group.ToList();
                    foreach (var stepRole in stepRoles)
                    {
                        DateTime roleEndDate = AddDays(stepStartDate, stepRole.Days);
                        result.Add(new FSPlannedEndDateCalculatorOutput { PlannedEndDate = roleEndDate.ToUniversalTime().ToString("o"), RoleID = stepRole.RoleID, RoleName = stepRole.RoleName });
                        if (roleEndDate > stepEndDate)
                        {
                            stepEndDate = roleEndDate;
                        }
                    }
                }
            }

            await res.WriteAsJsonAsync(result);
            return res;
        }

        static DateTime AddDays(DateTime dateInput, int duration)
        {
            if(duration == 0)
                return dateInput;
            else
            return AddDays(NextWorkDay(dateInput.AddDays(1)), duration-1);
        }

        static DateTime NextWorkDay(DateTime date)
        {
            var isNotHoliday = (date.DayOfWeek is not DayOfWeek.Saturday
                 and not DayOfWeek.Sunday)
                 && !(date.Month == 1 && date.Day == 1)
                 && !(date.Month == 5 && date.Day == 1)
                 && !(date.Month == 12 && date.Day == 25)
                 && !(date.Month == 12 && date.Day == 31);
            if (isNotHoliday)
                return date;
            else
                return NextWorkDay(date.AddDays(1)); ;
        }
    }

    public class FSPlannedEndDateCalculatorBase
    {
        public string RoleID { get; set; }
        public string RoleName { get; set; }
    }

    public class FSPlannedEndDateCalculatorInput : FSPlannedEndDateCalculatorBase
    {
        public int Days { get; set; }
        public int RoleOrder { get; set; }
        public int StepOrder { get; set; }
    }

    public class FSPlannedEndDateCalculatorOutput : FSPlannedEndDateCalculatorBase
    {
        public string PlannedEndDate { get; set; }
    }
}
