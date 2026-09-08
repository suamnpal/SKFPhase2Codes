using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Net;
using System.Text;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PAHelperFunctionsApp.HelperFunctions;
using static System.Net.Mime.MediaTypeNames;
using Encoder = System.Drawing.Imaging.Encoder;

namespace PAHelperFunctionsApp
{
    public class BarCodeGenerator
    {
        private readonly ILogger _logger;

        public BarCodeGenerator(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<BarCodeGenerator>();
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        [Function("BarCodeGenerator")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            string body = await (new StreamReader(req.Body)).ReadToEndAsync();
            var generator = new Code39Barcode();
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "image/jpeg");
            var bitmap = generator.Create(120, body, 20, true, 2);
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Jpeg);
                return new FileContentResult(ms.ToArray(), "image/png");
            }
        }
    }
}
