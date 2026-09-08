using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class TATNINGAR_GR_TSO : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var values = new Dictionary<string, string>();
            values["DocumentUniqueId"] = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");

            string product = (req.ProductDesignation ?? "").Trim().ToUpperInvariant().Replace(".", ",");
            string[] parts = product.Split(new[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            int type = parts.Length > 2 && int.TryParse(parts[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedType) ? parsedType : 0;
            bool vz = product.Contains("VZ");
            bool v22 = product.Contains("V22");
            bool v21 = product.Contains("V21") && !product.Contains("V21-2") && !product.Contains("V21-3");
            bool v = v21 || v22 || product.Contains("V21-2") || product.Contains("V21-3");

            var standard = new Dictionary<int, double[]>
            {
                [217] = new[] { 135.5, 98.0 },
                [218] = new[] { 139.5, 102.0 },
                [220] = new[] { 153.5, 114.0 },
                [222] = new[] { 163.5, 122.0 },
                [224] = new[] { 179.5, 137.0 },
                [226] = new[] { 186.5, 147.0 },
                [228] = new[] { 211.5, 162.0 },
                [230] = new[] { 221.5, 172.0 },
                [232] = new[] { 236.5, 180.0 },
                [234] = new[] { 256.5, 199.0 },
                [236] = new[] { 266.5, 209.0 },
                [238] = new[] { 286.5, 224.0 },
                [240] = new[] { 302.5, 231.0 },
                [244] = new[] { 330.5, 259.0 },
                [248] = new[] { 345.5, 274.0 }
            };
            var v21Diameters = new Dictionary<int, double[]>
            {
                [222] = new[] { 176.5, 135.0 },
                [232] = new[] { 256.5, 200.0 },
                [240] = new[] { 302.5, 248.0 }
            };

            double[] diameters = vz
                ? new[] { 0.0, 0.0 }
                : v21 && v21Diameters.TryGetValue(type, out var selectedV21)
                    ? selectedV21
                    : standard.TryGetValue(type, out var selectedStandard)
                        ? selectedStandard
                        : new[] { 0.0, 0.0 };

            double e = GetNumber(req, "DiameterE", "E", "O_E", "ØE") ?? diameters[0];
            double c = GetNumber(req, "DiameterC", "C", "O_C", "ØC") ?? diameters[1];
            double d = e - 1;

            values["SumE"] = "(E) " + Format(e).Replace(".",",");
            values["SumETol"] = "+ 0.2";
            values["SumETolN"] = "- 0";
            values["SumD"] = "(D) " + Format(d).Replace(".", ",");
            values["SumDTol"] = "+ 0.2";
            values["SumDTolN"] = "- 0";
            values["SumC"] = "(C) " + Format(c).Replace(".", ",");
            values["SumCTol"] = "+ 0.2";
            values["SumCTolN"] = "- 0";

            double? a = type < 219 ? 17 : type == 220 ? 19.3 : type == 222 ? 23.3 : type == 224 || type == 232 ? 29.3 : type == 226 || type == 228 ? 28.3 : type == 230 ? 27.3 : type > 233 && type < 249 ? 33.9 : null;
            double? b = type < 223 ? 5 : type == 226 ? 6 : type == 224 || type == 230 || type == 234 || type == 236 ? 7 : type == 228 || type == 232 || type == 238 || type == 240 || type == 244 ? 8 : type == 248 ? 9 : null;
            values["Suma"] = "(a) " + (a.HasValue ? Format(a.Value).Replace(".",",") : "Fel");
            values["SumaTol"] = a.HasValue ? GeneralTolerance(a.Value) : "± 0.5";
            values["Sumb"] = "(b) " + (b.HasValue ? Format(b.Value) : "Fel");
            values["SumbTol"] = b.HasValue ? GeneralTolerance(b.Value) : "± 0.5";
            values["SumB1"] = "(B1) 3";
            values["SumB1Tol"] = "± 0.1";
            values["SumH2"] = "(H2) 4,9";
            values["SumH2Tol"] = "- 0.2";
            values["SumH2TolN"] = "- 0.4";
            values["SumH3"] = "(H3) 4";
            values["SumH3Tol"] = "± 0.1";
            values["SumR1"] = "R2";
            values["SumR2"] = "R max 1.2";
            values["SumRa"] = "6.3";

            bool machineMatch = IsMachineMatch(req.MachineNumber);
            values["SumMaskinValS1"] = "Maskin: " + (machineMatch ? req.MachineNumber ?? "" : "");
            string[] frequencies = { "1/5", "1/5", "1/5", "1/5", "1/5", "1/5", "1/5", "1/5", "Inst.", "½/tim" };
            string[] instruments = { "Digitalt Skjutmått", "Digitalt Skjutmått", "UD-Apparat", "Digitalt Skjutmått", "Mikrometer 0-25", "Digitalt Skjutmått", "Digitalt Skjutmått", "Digitalt Skjutmått", "Vinkelsystem", "Ytjämnhetsmätare" };
            string[] remarks = { "", "", "Inställningsring märkt " + Format(c), "", "Med sfärisk mätspets", "", "", "", "", "" };
            for (int i = 0; i < 10; i++)
            {
                string suffix = i == 9 ? "0" : (i + 1).ToString(CultureInfo.InvariantCulture);
                values["SumF1_" + suffix] = machineMatch ? frequencies[i] : "";
                values["SumD1_" + suffix] = machineMatch ? instruments[i] : "";
                values["SumAF1_" + suffix] = machineMatch ? remarks[i] : "";
            }

            values["SumTextS1"] = "";
            values["SumRit"] = !v && !vz ? "7438714" : v21 ? "7433013" : "";
            values["SumStämpling"] = "Stämplas: " + product;
            return values;
        }

        private static bool IsMachineMatch(string machineName) => machineName == "LT-3000EX" || machineName == "Nakamura" || machineName == "LC-20";
        private static string GeneralTolerance(double value) => value < 6 ? "± 0.1" : value < 30 ? "± 0.2" : value < 120 ? "± 0.3" : "± 0.5";
        private static string Format(double value) => value.ToString("0.###", CultureInfo.InvariantCulture);

        private static double? GetNumber(object source, params string[] propertyNames)
        {
            foreach (string propertyName in propertyNames)
            {
                PropertyInfo property = source.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (property == null) continue;
                object raw = property.GetValue(source);
                if (raw == null) continue;
                string text = Convert.ToString(raw, CultureInfo.InvariantCulture)?.Replace(',', '.');
                if (double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out double value) && value != 0) return value;
            }
            return null;
        }
    }
}
