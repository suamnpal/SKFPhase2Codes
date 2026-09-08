using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class LOCK_ETS_30_31_32_R : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var result = InitializeAll();

            try
            {
                ParsedDesignation parsed = ParseDesignation(request);
                Merge(result, GetValidation(parsed));
                Merge(result, GetDimensions(parsed));
                Merge(result, GetDrawing());
                Merge(result, GetSurface());
                Merge(result, GetMachine(request));
                Merge(result, GetFrequency(request));
                Merge(result, GetTools(request));
                Merge(result, GetRemarks(request));
                Merge(result, GetOtherText());
            }
            catch
            {
                throw;
            }

            return result;
        }

        private Dictionary<string, string> InitializeAll()
        {
            return new Dictionary<string, string>
            {
                {"VaLSerieLista", ""},
                {"VaLTypLista", ""},
                {"SumA", ""},
                {"SumATol", ""},
                {"SumATolN", ""},
                {"SumB", ""},
                {"SumBTol", ""},
                {"SumE", ""},
                {"SumETol", ""},
                {"SumETolN", ""},
                {"SumC", ""},
                {"SumCTol", ""},
                {"SumG", ""},
                {"SumGTol", ""},
                {"SumR", ""},
                {"SumRit", ""},
                {"SumRa63", ""},
                {"SumRa63a", ""},
                {"SumRa125", ""},
                {"SumMaskinValS1", ""},
                {"SumF1_1", ""},
                {"SumF1_2", ""},
                {"SumF1_3", ""},
                {"SumF1_4", ""},
                {"SumF1_5", ""},
                {"SumF1_6", ""},
                {"SumF1_7", ""},
                {"SumF1_8", ""},
                {"SumF1_9", ""},
                {"SumF1_0", ""},
                {"SumD1_1", ""},
                {"SumD1_2", ""},
                {"SumD1_3", ""},
                {"SumD1_4", ""},
                {"SumD1_5", ""},
                {"SumD1_6", ""},
                {"SumD1_7", ""},
                {"SumD1_8", ""},
                {"SumD1_9", ""},
                {"SumD1_0", ""},
                {"SumAF1_1", ""},
                {"SumAF1_2", ""},
                {"SumAF1_3", ""},
                {"SumAF1_4", ""},
                {"SumAF1_5", ""},
                {"SumAF1_6", ""},
                {"SumAF1_7", ""},
                {"SumAF1_8", ""},
                {"SumAF1_9", ""},
                {"SumAF1_0", ""},
                {"SumTextS1", ""}
            };
        }

        private Dictionary<string, string> GetValidation(ParsedDesignation parsed)
        {
            var d = new Dictionary<string, string>();
            d["VaLSerieLista"] = parsed.SerieIndex == 0 ? "Produktbeteckningen ingår ej i mallen, kontrollera serie" : "";
            d["VaLTypLista"] = parsed.TypeIndex == 0 ? "Produktbeteckningen ingår ej i mallen, kontrollera typ" : "";
            return d;
        }

        private Dictionary<string, string> GetDimensions(ParsedDesignation parsed)
        {
            var d = new Dictionary<string, string>();

            double a = GetA(parsed.Serie, parsed.TypeIndex);
            double b = a == 215 || a == 235 ? 9.5 : 9;
            double e = 6.5;
            double c = 21 - b - e;
            double g1 = GetG1(a);
            double g = (a - g1) / 2;
            double gTolA = H11NegativeAbs(g1);

            d["SumA"] = "(A) " + FormatNumber(a).Replace(".", ",");
            d["SumATol"] = "+ 0";
            d["SumATolN"] = "- " + Format3(H11NegativeAbs(a));
            d["SumB"] = "(B) " + FormatNumber(b).Replace(".", ",");
            d["SumBTol"] = "± 0.5";
            d["SumE"] = "(E) " + FormatNumber(e).Replace(".",",");
            d["SumETol"] = "+ 0.090";
            d["SumETolN"] = "- 0";
            d["SumC"] = "(C) " + FormatNumber(c).Replace(".", ",");
            d["SumCTol"] = GeneralTolerance(c);
            d["SumG"] = "(G) " + FormatNumber(g).Replace(".", ",");
            d["SumGTol"] = "+ " + (gTolA / 2).ToString("0.###", CultureInfo.InvariantCulture);
            d["SumR"] = "R 0.5 (x2)";

            return d;
        }

        private Dictionary<string, string> GetDrawing()
        {
            return new Dictionary<string, string>
            {
                {"SumRit", "7438266"}
            };
        }

        private Dictionary<string, string> GetSurface()
        {
            return new Dictionary<string, string>
            {
                {"SumRa63", "6.3"},
                {"SumRa63a", "6.3"},
                {"SumRa125", "12,5"}
            };
        }

        private Dictionary<string, string> GetMachine(APIRequest request)
        {
            var d = new Dictionary<string, string>();
            string machine = GetSafe(request != null ? request.MachineNumber : "");
            string machineValue = EqualsIgnoreCase(machine, "Nakamura") ? "Nakamura" : "";
            d["SumMaskinValS1"] = "Maskin: " + machineValue;
            return d;
        }

        private Dictionary<string, string> GetFrequency(APIRequest request)
        {
            var d = new Dictionary<string, string>();
            bool isNakamura = IsNakamura(request);
            d["SumF1_1"] = isNakamura ? "1/5" : "";
            d["SumF1_2"] = isNakamura ? "1/5" : "";
            d["SumF1_3"] = isNakamura ? "1/10" : "";
            d["SumF1_4"] = isNakamura ? "1/Skift" : "";
            d["SumF1_5"] = isNakamura ? "1/Skift" : "";
            d["SumF1_6"] = isNakamura ? "1/Skift" : "";
            d["SumF1_7"] = "";
            d["SumF1_8"] = "";
            d["SumF1_9"] = "";
            d["SumF1_0"] = "";
            return d;
        }

        private Dictionary<string, string> GetTools(APIRequest request)
        {
            var d = new Dictionary<string, string>();
            bool isNakamura = IsNakamura(request);
            d["SumD1_1"] = isNakamura ? "Skjutmått" : "";
            d["SumD1_2"] = isNakamura ? "Djupmått" : "";
            d["SumD1_3"] = isNakamura ? "Skjutmått" : "";
            d["SumD1_4"] = isNakamura ? "Skjutmått" : "";
            d["SumD1_5"] = isNakamura ? "Skjutmått" : "";
            d["SumD1_6"] = isNakamura ? "Ytjämnhetsmätare" : "";
            d["SumD1_7"] = "";
            d["SumD1_8"] = "";
            d["SumD1_9"] = "";
            d["SumD1_0"] = "";
            return d;
        }

        private Dictionary<string, string> GetRemarks(APIRequest request)
        {
            var d = new Dictionary<string, string>();
            bool isNakamura = IsNakamura(request);
            d["SumAF1_1"] = "";
            d["SumAF1_2"] = "";
            d["SumAF1_3"] = "";
            d["SumAF1_4"] = "";
            d["SumAF1_5"] = isNakamura ? "Hjälpmått" : "";
            d["SumAF1_6"] = isNakamura ? "Övriga bearbetade ytor Ra 12.5" : "";
            d["SumAF1_7"] = "";
            d["SumAF1_8"] = "";
            d["SumAF1_9"] = "";
            d["SumAF1_0"] = "";
            return d;
        }

        private Dictionary<string, string> GetOtherText()
        {
            return new Dictionary<string, string>
            {
                {"SumTextS1", "Okulärkontroll: Grader, frifläckar, slagmärken, repor, valkar, ytjämnhet och faser<<LineBreak>>\u00A0<<LineBreak>>Skarpa kanter avgradas.<<LineBreak>> <<LineBreak>>"}
            };
        }

        private ParsedDesignation ParseDesignation(APIRequest request)
        {
            string designation = Normalize(GetSafe(request != null ? request.ProductDesignation : ""));
            designation = designation.Replace(".", ",");
            string[] parts = designation.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);

            string bet2 = parts.Length > 1 ? parts[1] : "";
            string bet3 = parts.Length > 2 ? parts[2] : "";
            int count = bet2.Length;
            string serie = count > 3 ? Left(bet2, 2) : bet2;
            string typ = count > 3 ? Right((bet2).Replace(",",""), 2) : Right(bet3, 2);

            return new ParsedDesignation
            {
                Designation = designation,
                Serie = serie,
                Typ = typ,
                SerieIndex = Member(serie, new string[] { "30", "31", "32" }),
                TypeIndex = Member(typ, new string[] { "34", "36", "38", "40", "44", "48", "52", "56", "60", "64", "68", "72", "76", "80", "84" })
            };
        }

        private double GetA(string serie, int typeIndex)
        {
            if (typeIndex <= 0) return 0;

            if (serie == "30")
                return Pick(new double[] { 34, 186, 204, 215, 231, 255, 275, 286, 326, 335, 351, 72, 76, 411, 436 }, typeIndex);

            if (serie == "31")
                return Pick(new double[] { 184, 195, 215, 216, 235, 255, 286, 306, 326, 346, 368, 385, 436, 411, 436 }, typeIndex);

            return Pick(new double[] { 34, 36, 38, 40, 265, 48, 52, 56, 346, 346, 68, 411, 436, 446, 456 }, typeIndex);
        }

        private double GetG1(double a)
        {
            if (a < 187) return a - 18;
            if (a < 196) return a - 19;
            if (a == 368) return a - 18;
            if (a == 204 || a == 286 || a == 306 || a == 326 || a == 346 || a == 351 || a == 366 || a == 385) return a - 16;
            if (a == 215 || a == 231 || a == 235 || a == 265 || a == 255 || a == 275) return a - 20.5;
            if (a == 216) return a - 15;
            if (a == 335) return a - 18;
            if (a == 436 || a == 446 || a == 456) return a - 11;
            return 0;
        }

        private double H11NegativeAbs(double v)
        {
            if (v < 180.01) return 0.250;
            if (v < 250.01) return 0.290;
            if (v < 315.01) return 0.320;
            if (v < 400.01) return 0.360;
            if (v < 500.01) return 0.400;
            if (v < 630.01) return 0.440;
            if (v < 800.01) return 0.500;
            return 0.560;
        }

        private string GeneralTolerance(double v)
        {
            if (v < 6.01) return "± 0.1";
            if (v < 30.01) return "± 0.2";
            if (v < 120.01) return "± 0.3";
            if (v < 315.01) return "± 0.5";
            if (v < 1000.01) return "± 0.8";
            if (v < 2000.01) return "± 1.2";
            return "± 2.0";
        }

        private bool IsNakamura(APIRequest request)
        {
            return EqualsIgnoreCase(GetSafe(request != null ? request.MachineNumber : ""), "Nakamura");
        }

        private int Member(string value, string[] values)
        {
            for (int i = 0; i < values.Length; i++)
                if (EqualsIgnoreCase(value, values[i]))
                    return i + 1;
            return 0;
        }

        private double Pick(double[] values, int index)
        {
            if (values == null || index <= 0 || index > values.Length) return 0;
            return values[index - 1];
        }

        private string Normalize(string input)
        {
            return input == null ? "" : input.Trim().ToUpperInvariant();
        }

        private string GetSafe(string value)
        {
            return value ?? "";
        }

        private bool EqualsIgnoreCase(string a, string b)
        {
            return string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);
        }

        private string Left(string value, int length)
        {
            if (string.IsNullOrEmpty(value)) return "";
            if (length <= 0) return "";
            return value.Length <= length ? value : value.Substring(0, length);
        }

        private string Right(string value, int length)
        {
            if (string.IsNullOrEmpty(value)) return "";
            if (length <= 0) return "";
            return value.Length <= length ? value : value.Substring(value.Length - length);
        }

        private string FormatNumber(double value)
        {
            return value.ToString("0.###", CultureInfo.InvariantCulture);
        }

        private string Format3(double value)
        {
            return value.ToString("0.000", CultureInfo.InvariantCulture);
        }

        private void Merge(Dictionary<string, string> target, Dictionary<string, string> source)
        {
            foreach (var item in source)
                target[item.Key] = item.Value ?? "";
        }

        private class ParsedDesignation
        {
            public string Designation { get; set; }
            public string Serie { get; set; }
            public string Typ { get; set; }
            public int SerieIndex { get; set; }
            public int TypeIndex { get; set; }
        }
    }
}