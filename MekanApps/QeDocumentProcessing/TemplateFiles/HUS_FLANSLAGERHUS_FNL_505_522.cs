using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HUS_FLANSLAGERHUS_FNL_505_522 : ITemplateCalculations
    {
        private static string subject = string.Empty;

        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var result = new Dictionary<string, string>();

            double ID = GetBookmark(request, "Innerdiameter");
            double B = GetBookmark(request, "Bredd");
            double AD = GetBookmark(request, "Axeldiameter");
            double TD = GetBookmark(request, "Tätningsdiameter");
            double FD = GetBookmark(request, "Fothålsdiameter");
            double LD = GetBookmark(request, "Lockhålsdiameter");

            subject = (request?.ProductDesignation ?? "").ToUpper();

            var match = Regex.Match(subject, @"\d+");
            int typ = match.Success ? int.Parse(match.Value) % 100 : 0;

            bool isV22 = subject.Contains("V22");

            Merge(result, EnsureAllKeys());
            Merge(result, Dimensions(ID, B, AD, TD, FD, LD));
            Merge(result, HoleLogic(ID));
            Merge(result, HoleListLogic(typ, isV22));
            Merge(result, Machine(request?.MachineNumber ?? ""));
            Merge(result, TableValues(request?.MachineNumber ?? ""));
            Merge(result, Static());

            return result;
        }

        private double GetBookmark(APIRequest request, string name)
        {
            var value = request?.Bookmarks?
                .FirstOrDefault(x => x.BookmarkName.Equals(name, StringComparison.OrdinalIgnoreCase))
                ?.BookmarkValue;

            return double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var v) ? v : 0;
        }

        private Dictionary<string, string> EnsureAllKeys()
        {
            var kv = new Dictionary<string, string>();

            var keys = new[]
            {
                "SumID","SumIDTol","SumIDTolN","SumB","SumBTol",
                "SumAD","SumADTol","SumADTolN",
                "SumTD","SumTDTol","SumTDTolN",
                "SumFD","SumFDTol",
                "SumLD","SumLDTol",
                "SumFH","SumFHTol",
                "SumFB","SumFBTol","SumFBTolN",
                "Sum6","Sum7",
                "Sum1","Sum1Tol","Sum2","Sum2Tol",
                "SumLLB","SumLLBTol","SumLLBTolN",
                "SumFHL","SumFHG","SumFas",
                "SumMaskinValS1",
                "SumF1_0","SumD1_0","SumAF1_0",
                "SumRa32","SumPh","SumVr","SumKa","SumC",
                "SumKlEgenskaper","SumTextS1"
            };

            foreach (var k in keys) kv[k] = "";

            for (int i = 0; i <= 14; i++)
            {
                kv[$"SumF1_{i}"] = "";
                kv[$"SumD1_{i}"] = "";
                kv[$"SumAF1_{i}"] = "";
            }

            return kv;
        }

        private Dictionary<string, string> Dimensions(double ID, double B, double AD, double TD, double FD, double LD)
        {
            var kv = new Dictionary<string, string>();

            string f(double v) => v.ToString("0.###", CultureInfo.InvariantCulture);

            kv["SumID"] = "(ID) " + f(ID);

            kv["SumIDTol"] = (ID < 80 ? 0.040 : ID < 120 ? 0.047 : ID < 180 ? 0.054 : 0.061)
                .ToString("0.000", CultureInfo.InvariantCulture);

            kv["SumIDTolN"] = "+ " + (ID < 80 ? 0.010 : ID < 120 ? 0.012 : ID < 180 ? 0.014 : 0.015).ToString("0.000", CultureInfo.InvariantCulture) + " [2]";

            kv["SumB"] = "(B) " + f(B);

            kv["SumBTol"] = $"± {(B < 6 ? 0.1 :
                                  B < 30 ? 0.2 :
                                  B < 120 ? 0.300 :
                                  B < 315 ? 0.5 :
                                  B < 1000 ? 0.8 :
                                  B < 2000 ? 1.2 : 2)
                                 .ToString("0.000", CultureInfo.InvariantCulture)}";


            kv["SumAD"] = "(AD) " + f(AD);

            kv["SumADTol"] = "+ " +
                (AD < 30 ? 0.21 : AD < 50 ? 0.25 : AD < 80 ? 0.3 : 0.35)
                    .ToString("0.000", CultureInfo.InvariantCulture) +
                " [3]";

            kv["SumADTolN"] = "- 0.000 [3]";

            kv["SumTD"] = "(TD) " + f(TD);

            kv["SumTDTol"] = "+ " +
                (TD < 50 ? 0.250 : TD < 80 ? 0.30 : TD < 120 ? 0.35 : 0.40)
                    .ToString("0.000", CultureInfo.InvariantCulture) +
                " [3]";

            kv["SumTDTolN"] = "- 0.000 [3]";

            kv["SumFD"] = "(FD) " + f(FD);
            kv["SumFDTol"] = FD < 120 ? "± 0.3" : "± 0.5";

            kv["SumLD"] = "(LD) " + f(LD);
            kv["SumLDTol"] = "± 0.3";

            kv["SumRitNr"] = $"{subject}:2";

            return kv;
        }
        private Dictionary<string, string> HoleLogic(double ID)
        {
            var kv = new Dictionary<string, string>();

            kv["SumFH"] = "(FH) " +
                (ID < 60 ? "10" :
                 ID < 89 ? "12" :
                 ID < 121 ? "15" :
                 ID < 165 ? "25" : "30");

            kv["SumFHTol"] = "± 0.6";

            kv["SumFB"] =
                ID < 71 ? "(FB) 11.5" :
                ID < 121 ? "(FB) 14.0" :
                ID < 165 ? "(FB) 18.0" : "(FB) 22.0";

            kv["SumFBTol"] = "+ 0.430 [3]";
            kv["SumFBTolN"] = "- 0.000 [3]";

            kv["Sum6"] =
                ID < 84 ? "M5 (3x) [3]" :
                ID < 121 ? "M6 (3x) [3]" :
                ID < 165 ? "M8 (4x) [3]" : "M10 (4x) [3]";

            kv["Sum7"] =
                ID < 84 ? "min 12" :
                ID < 121 ? "min 14" :
                ID < 130 ? "min 16" :
                ID < 180 ? "min 18" : "min 20";

            return kv;
        }

        private Dictionary<string, string> HoleListLogic(int typ, bool isV22)
        {
            var kv = new Dictionary<string, string>();

            var hcMap = new Dictionary<int, string>
            {
                {5,"13,6:23,6:24:2º"},
                {6,"14,6:25,6:26:2º"},
                {7,"13,7:28,7:29:2º"},
                {8,"13,6:28,5:29:2º"},
                {9,"147:29,6:30:5º"},
                {10,"16,2:31,5:30:2º"},
                {11,"18,33,3:32:2º"},
                {12,"19,5:36,3:35:2º"},
                {13,"20,9:38,9:38:2º"},
                {15,"21,7:45,8:46:11º"},
                {16,"23,4:48,7:48:10º"},
                {17,"24,4:49,51:9º"},
                {18,"28,3:58,5:55:5º"},
                {20,"28,6:55,3:61:12º"},
                {22,"31,7:60,4:68:12º"}
            };              

            if (!hcMap.ContainsKey(typ)) return kv;

            var p = hcMap[typ].Split(':');

            kv["Sum1"] = p[0];
            kv["Sum1Tol"] = "± 0.2";

            kv["Sum2"] = p[1];
            kv["Sum2Tol"] = "± 0.2";

            kv["SumLLB"] = "(LLB) " + p[2];
            kv["SumLLBTol"] = "+ 0.210 [3]";
            kv["SumLLBTolN"] = "- 0.000 [2]";

            kv["SumFHL"] = p[3];
            kv["SumFHG"] = "1/8” – 27 NPSF (2X)";
            kv["SumFas"] = "1x45º";

            return kv;
        }

        private Dictionary<string, string> Machine(string machine)
        {
            var kv = new Dictionary<string, string>();

            kv["SumMaskinValS1"] = "Maskin: " + machine;

            if (machine == "Multus")
            {
                kv["SumF1_0"] = "1/3";
                kv["SumD1_0"] = "Mätmaskin";
                kv["SumAF1_0"] = "";
                kv["SumAF1_4"] = "100% Okulärt";
            }

            return kv;
        }

        private Dictionary<string, string> TableValues(string machine)
        {
            var kv = new Dictionary<string, string>();

            if (machine == "Multus")
            {
                for (int i = 1; i <= 14; i++)
                {
                    kv[$"SumF1_{i}"] = (i == 3 || i == 4 || i >= 6) ? "2/Skift" : "1/3";
                    kv[$"SumD1_{i}"] = (i == 6) ? "Skjutmått" : "Mätmaskin";
                }

                kv["SumD1_11"] = "Digitalt Skjutmått/spårmall";
                kv["SumD1_12"] = "Gängtolk";
                kv["SumD1_13"] = "Gängtolk";
                kv["SumD1_14"] = "Ytjämnhetsmätare";

                kv["SumAF1_4"] = "100% Okulärt";
                kv["SumAF1_11"] = "Mallnr: 7543174/1 FNL 505";
                kv["SumAF1_14"] = "Övriga bearbetade ytor Ra 6.3";
            }

            return kv;
        }

        private Dictionary<string, string> Static()
        {
            return new Dictionary<string, string>
            {
                {"SumRa32","3.2"},
                {"SumPh","0.08"},
                {"SumVr","0.1"},
                {"SumKa","0.2"},
                {"SumC","0.02"},
                {"SumKlEgenskaper","PRODUCTION NUTS & SLEEVES & HOUSINGS\\ALLMÄN\\KLASSADE EGENSKAPER\\Flänslagerhus"},
                {"SumTextS1","Okulärkontroll: Grader, frifläcker, slagmärken, repor, valkar, ytjämnhet och faser <<LineBreak>>Skarpa kanter avgradas."}
            };
        }

        private void Merge(Dictionary<string, string> target, Dictionary<string, string> source)
        {
            foreach (var kv in source)
                target[kv.Key] = kv.Value ?? "";
        }
    }
}