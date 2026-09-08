using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class LOCK_ETS_3034_3056 : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var result = InitializeAll();
            Merge(result, GetDimensions(request));
            Merge(result, GetDrawing());
            Merge(result, GetMachine(request));
            Merge(result, GetFrequency(request));
            Merge(result, GetTools(request));
            Merge(result, GetRemarks(request));
            Merge(result, GetOtherText());
            return result;
        }

        private Dictionary<string, string> InitializeAll()
        {
            return new Dictionary<string, string>
            {
                {"SumA",""},
                {"SumATol",""},
                {"SumATolN",""},
                {"SumB",""},
                {"SumBTol",""},
                {"SumBTolN",""},
                {"SumC",""},
                {"SumD",""},
                {"SumE",""},
                {"SumETol",""},
                {"SumETolN",""},
                {"SumF",""},
                {"SumFTol",""},
                {"SumFTolN",""},
                {"SumG",""},
                {"SumRa125",""},
                {"SumRit",""},
                {"SumMaskinValS1",""},
                {"SumF1_1",""},
                {"SumF1_2",""},
                {"SumF1_3",""},
                {"SumF1_4",""},
                {"SumF1_5",""},
                {"SumF1_6",""},
                {"SumF1_7",""},
                {"SumF1_8",""},
                {"SumF1_9",""},
                {"SumF1_0",""},
                {"SumD1_1",""},
                {"SumD1_2",""},
                {"SumD1_3",""},
                {"SumD1_4",""},
                {"SumD1_5",""},
                {"SumD1_6",""},
                {"SumD1_7",""},
                {"SumD1_8",""},
                {"SumD1_9",""},
                {"SumD1_0",""},
                {"SumAF1_1",""},
                {"SumAF1_2",""},
                {"SumAF1_3",""},
                {"SumAF1_4",""},
                {"SumAF1_5",""},
                {"SumAF1_6",""},
                {"SumAF1_7",""},
                {"SumAF1_8",""},
                {"SumAF1_9",""},
                {"SumAF1_0",""},
                {"SumTextS1",""}
            };
        }

        private Dictionary<string, string> GetDimensions(APIRequest request)
        {
            var d = new Dictionary<string, string>();
            int typ = GetTyp(request);
            int index = GetTypIndex(typ);
            if (index < 0)
                return d;

            double[] aValues = { 216.5, 226.5, 236.5, 248.5, 268.5, 288.5, 308.5, 328.5 };
            double[] bValues = { 212, 222, 232, 242, 262, 282, 302, 322 };
            double[] cValues = { 176, 196, 196, 216, 236, 256, 276, 296 };

            double a = aValues[index];
            double b = bValues[index];
            double c = cValues[index];
            double dimD = typ < 39 ? 8 : 10;
            double e = typ < 39 ? 8.5 : typ < 45 ? 7 : 5.5;
            double f = typ < 39 ? 6 : 8;
            double g = 21;

            d["SumA"] = "(A) " + FormatDimension(a);
            d["SumATol"] = "+ " + FormatTol(0.2);
            d["SumATolN"] = "-  0";
            d["SumB"] = "(B) " + FormatDimension(b);
            d["SumBTol"] = "+ 0";
            d["SumBTolN"] = "- " + FormatTol(GetBTolN(b));
            d["SumC"] = "(C) " + FormatDimension(c);
            d["SumD"] = "(D) " + FormatDimension(dimD);
            d["SumE"] = "(E) " + FormatDimension(e);
            d["SumETol"] = "+ " + FormatTol(0.5);
            d["SumETolN"] = "- " + FormatTol(0.5);
            d["SumF"] = "(F) " + FormatDimension(f);
            d["SumFTol"] = "+ 0";
            d["SumFTolN"] = "- " + FormatTol(GetFTolN(f));
            d["SumG"] = "(G) " + FormatDimension(g);
            d["SumRa125"] = "12.5";
            return d;
        }

        private Dictionary<string, string> GetDrawing()
        {
            return new Dictionary<string, string>
            {
                {"SumRit", "7439771:2"}
            };
        }

        private Dictionary<string, string> GetMachine(APIRequest request)
        {
            var d = new Dictionary<string, string>();
            string machine = GetSafe(request.MachineNumber);
            if (IsMachineValid(machine))
                d["SumMaskinValS1"] = "Maskin: " + machine;
            return d;
        }

        private Dictionary<string, string> GetFrequency(APIRequest request)
        {
            var d = new Dictionary<string, string>();
            if (IsMachineValid(GetSafe(request.MachineNumber)))
            {
                d["SumF1_1"] = "1/5";
                d["SumF1_2"] = "1/5";
                d["SumF1_3"] = "1/5";
                d["SumF1_4"] = "1/Skift";
                d["SumF1_5"] = "1/Skift";
                d["SumF1_6"] = "1/Skift";
                d["SumF1_7"] = "1/Skift";
                d["SumF1_8"] = "";
                d["SumF1_9"] = "";
                d["SumF1_0"] = "";
            }
            return d;
        }

        private Dictionary<string, string> GetTools(APIRequest request)
        {
            var d = new Dictionary<string, string>();
            if (IsMachineValid(GetSafe(request.MachineNumber)))
            {
                d["SumD1_1"] = "Skjutmått";
                d["SumD1_2"] = "Skjutmått";
                d["SumD1_3"] = "Skjutmått";
                d["SumD1_4"] = "Skjutmått";
                d["SumD1_5"] = "Skjutmått";
                d["SumD1_6"] = "Skjutmått";
                d["SumD1_7"] = "Skjutmått";
                d["SumD1_8"] = "Ytjämnhetsmätare";
                d["SumD1_9"] = "";
                d["SumD1_0"] = "";
            }
            return d;
        }

        private Dictionary<string, string> GetRemarks(APIRequest request)
        {
            var d = new Dictionary<string, string>();
            if (IsMachineValid(GetSafe(request.MachineNumber)))
            {
                d["SumAF1_1"] = "";
                d["SumAF1_2"] = "";
                d["SumAF1_3"] = "";
                d["SumAF1_4"] = "";
                d["SumAF1_5"] = "";
                d["SumAF1_6"] = "";
                d["SumAF1_7"] = "";
                d["SumAF1_8"] = "Alla bearbetade ytor Ra 12.5";
                d["SumAF1_9"] = "";
                d["SumAF1_0"] = "";
            }
            return d;
        }

        private Dictionary<string, string> GetOtherText()
        {
            return new Dictionary<string, string>
            {
                {"SumTextS1", "Okulärkontroll: Grader, frifläckar, slagmärken, repor, valkar, ytjämnhet och faser<<LineBreak>>Skarpa kanter avgradas."}
            };
        }

        private int GetTyp(APIRequest request)
        {
            string designation = Normalize(GetSafe(request.ProductDesignation));
            designation = designation.Replace('.', ',');
            var parts = designation.Split(new[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
                return 0;
            string token = parts[1];
            if (token.Length < 2)
                return 0;
            string typText = token.Substring(token.Length - 2, 2);
            int typ;
            return int.TryParse(typText, NumberStyles.Any, CultureInfo.InvariantCulture, out typ) ? typ : 0;
        }

        private int GetTypIndex(int typ)
        {
            int[] types = { 34, 36, 38, 40, 44, 48, 52, 56 };
            for (int i = 0; i < types.Length; i++)
                if (types[i] == typ)
                    return i;
            return -1;
        }

        private double GetBTolN(double v)
        {
            if (v < 180.1) return 0.63;
            if (v < 250.1) return 0.72;
            if (v < 315.1) return 0.81;
            return 0.89;
        }

        private double GetFTolN(double v)
        {
            if (v < 3.1) return 0.14;
            if (v < 6.1) return 0.18;
            if (v < 10.1) return 0.22;
            return 0.27;
        }

        private bool IsMachineValid(string machine)
        {
            return machine == "Nakamura" || machine == "LC20";
        }

        private string FormatFixedRemoveLastZero(double value, int decimals)
        {
            return value.ToString("0." + new string('#', decimals), CultureInfo.InvariantCulture);

        }
        private string FormatDimension(double v)
        {
            if (Math.Abs(v - Math.Round(v)) < 0.0000001)
                return v.ToString("0", CultureInfo.InvariantCulture);
            return v.ToString("0.#", CultureInfo.InvariantCulture).Replace('.', ',');
        }

        private string FormatTol(double v)
        {
            return v.ToString("0.000", CultureInfo.InvariantCulture).TrimEnd('0').TrimEnd('.');
        }

        private string Normalize(string input)
        {
            return input == null ? "" : input.Trim().ToUpperInvariant();
        }

        private string GetSafe(string v)
        {
            return v ?? "";
        }

        private void Merge(Dictionary<string, string> target, Dictionary<string, string> source)
        {
            foreach (var kv in source)
                target[kv.Key] = kv.Value ?? "";
        }
    }
}
