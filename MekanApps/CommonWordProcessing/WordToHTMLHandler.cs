using CommonDocumentProcessing.Common;
using DocumentFormat.OpenXml.Packaging;
using OpenXmlPowerTools;
using System.Drawing.Imaging;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static OpenXmlPowerTools.FlatOpc;
using HtmlConverter = OpenXmlPowerTools.HtmlConverter;

namespace CommonDocumentProcessing
{
    /// <summary>
    /// This function is used by Test Casting App to convert word RTE into HTML equivalent (to show in the UI)
    /// </summary>
    public static class WordToHTMLHandler
    {
        public static string ProcessWordToHTML(APIRequest req)
        {
            string resultHTMLBase64 = string.Empty;
            byte[] byteArray = Convert.FromBase64String(req.InputFileAsBase64);

            using (var stream = new MemoryStream())
            {
                stream.Write(byteArray, 0, byteArray.Length);
                stream.Position = 0;

                using (var wordDoc = WordprocessingDocument.Open(stream, true))
                {
                    HtmlConverterSettings settings = new HtmlConverterSettings()
                    {
                        PageTitle = "Converted Document",
                        CssClassPrefix = "doc",

                        FabricateCssClasses = false,
                        GeneralCss = "",
                        AdditionalCss = "",

                        ImageHandler = imageInfo =>
                        {
                            byte[] imageBytes;

                            // ✅ Extract raw bytes safely
                            using (var ms = new MemoryStream())
                            {
                                imageInfo.Bitmap.Save(ms, ImageFormat.Png); // fallback
                                imageBytes = ms.ToArray();
                            }

                            // ✅ Convert to Base64
                            string base64 = Convert.ToBase64String(imageBytes);

                            return new XElement(Xhtml.img,
                                new XAttribute(NoNamespace.src,
                                    $"data:{imageInfo.ContentType};base64,{base64}"),
                                imageInfo.ImgStyleAttribute,
                                new XAttribute(NoNamespace.alt, imageInfo.AltText ?? "")
                            );
                        }
                    };

                    XElement html = HtmlConverter.ConvertToHtml(wordDoc, settings);

                    // ✅ Extract ONLY BODY
                    var body = html.Descendants(Xhtml.body).FirstOrDefault();

                    string htmlString = body != null
                        ? string.Concat(body.Nodes())
                        : html.ToString(SaveOptions.DisableFormatting);

                    // ✅ Normalize for Power Apps
                    htmlString = NormalizeForPowerApps(htmlString);

                    // ✅ Fix table rendering
                    htmlString = htmlString.Replace(
                        "<table",
                        "<table style='border-collapse:collapse;width:100%;table-layout:fixed;'"
                    );

                    htmlString = htmlString.Replace(
                        "border:none",
                        "border:1px solid #000"
                    );

                    byte[] htmlBytes = System.Text.Encoding.UTF8.GetBytes(htmlString);
                    resultHTMLBase64 = Convert.ToBase64String(htmlBytes);
                }
            }
            return resultHTMLBase64;
        }

        private static string NormalizeForPowerApps(string html)
        {
            // ✅ Convert pt → px
            html = Regex.Replace(html,
                    @"(?<=[:\s])(\d+(\.\d+)?)pt(?=[;""'])",
                    m =>
                    {
                        double value = double.Parse(
                            m.Groups[1].Value,
                            CultureInfo.InvariantCulture);

                        return $"{Math.Round(value * 1.333)}px";
                    },
                    RegexOptions.IgnoreCase);

            // ✅ Remove problematic styles
            html = html.Replace("display:inline-block;", "");
            html = html.Replace("margin-bottom:.001pt;", "");

            return html;
        }
    }
}