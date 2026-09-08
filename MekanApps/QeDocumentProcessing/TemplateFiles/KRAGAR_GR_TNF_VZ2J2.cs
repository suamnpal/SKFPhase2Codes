using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class KRAGAR_GR_TNF_VZ2J2 : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var result = new Dictionary<string, string>();
            var ctx = BuildContext(request);

            Merge(result, GetDiameters(ctx));
            Merge(result, GetWidths(ctx));
            Merge(result, GetRadiiChamfers(ctx));
            Merge(result, GetThreadsAndHole(ctx));
            Merge(result, GetAngles());
            Merge(result, GetSurface());
            Merge(result, GetMachine(ctx));
            Merge(result, GetFrequencies(ctx));
            Merge(result, GetMeasuringTools(ctx));
            Merge(result, GetRemarks());
            Merge(result, GetTexts());

            return result;
        }

        private Context BuildContext(APIRequest request)
        {
            var subject = request?.ProductDesignation ?? "";
            var machineRaw = WebUtility.HtmlDecode(request?.MachineNumber ?? "");

            var tokens = subject.ToUpper()
                .Split(new[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);

            string sizeToken = tokens.FirstOrDefault(t => t.StartsWith("31")) ?? "";

            return new Context
            {
                TypIndex = GetTypIndex(sizeToken),
                Machine = NormalizeMachine(machineRaw)
            };
        }

        private int GetTypIndex(string size)
        {
            var map = new Dictionary<string, int>
            {
                {"3148",1},{"3152",2},{"3156",3},
                {"3160",4},{"3164",5},{"3168",6},{"3172",7}
            };
            return map.ContainsKey(size) ? map[size] : 1;
        }

        private string NormalizeMachine(string machine)
        {
            var m = machine?.ToLowerInvariant() ?? "";

            if (m.Contains("nakamura")) return "Nakamura";
            if (m.Contains("maxmuller")) return "MaxMuller";

            return "";
        }

        private int i(Context ctx) => Math.Max(0, Math.Min(ctx.TypIndex - 1, 6));

        private Dictionary<string, string> GetDiameters(Context ctx)
        {
            int x = i(ctx);

            string[] A = { "342", "362", "382", "402", "439", "474", "478" };
            string[] B = { "293", "313", "333", "353", "373", "393", "413" };
            string[] C1 = { "294", "299", "319", "343", "359", "381", "437" };
            string[] C2 = { "284", "289", "309", "331", "349", "371", "427" };
            string[] D1 = { "283", "303", "323", "343", "363", "383", "403" };
            string[] D2 = { "271", "291", "311", "331", "351", "371", "391" };
            string[] D3 = { "265", "285", "305", "325", "345", "365", "385" };
            string[] E = { "224", "244", "264", "284", "304", "324", "344" };
            string[] F = { "325", "343", "360", "382", "410", "450", "454" };

            var d = new Dictionary<string, string>
            {
                ["SumA"] = $"(A) {A[x]}",
                ["SumATol"] = GetTolPM(A[x]),

                ["SumB"] = $"(B) {B[x]}",
                ["SumBTol"] = GetTolPM(B[x]),

                ["SumC1"] = $"(C1) {C1[x]}",
                ["SumC1Tol"] = "+ 0",
                ["SumC1TolN"] = GetNegTol(C1[x]),

                ["SumC2"] = $"(C2) {C2[x]}",
                ["SumC2Tol"] = GetTolPM(C2[x]),

                ["SumD1"] = $"(D1) {D1[x]}",
                ["SumD1Tol"] = GetPosTolH(D1[x]),
                ["SumD1TolN"] = "- 0",

                ["SumD2"] = $"(D2) {D2[x]}",
                ["SumD2Tol"] = "+ 0",
                ["SumD2TolN"] = GetNegTol(D2[x]),

                ["SumD3"] = $"(D3) {D3[x]}",
                ["SumD3Tol"] = GetPosTolH(D3[x]),
                ["SumD3TolN"] = "- 0",

                ["SumE"] = $"(E) {E[x]}",
                ["SumETol"] = "+ 0.3",
                ["SumETolN"] = "- 0",

                ["SumF"] = $"(F) {F[x]}",
                ["SumFTol"] = GetTolF(F[x])
            };

            return d;
        }

        private Dictionary<string, string> GetWidths(Context ctx)
        {
            return new Dictionary<string, string>
            {
                ["SumB1"] = "(B1) 38.5",
                ["SumB1Tol"] = "± 0.3",

                ["SumB2"] = "(B2) 33.5",
                ["SumB2Tol"] = "± 0.3",

                ["SumB3"] = "(B3) 23.5",
                ["SumB3Tol"] = "± 0.2",

                ["SumB4"] = "(B4) 20",
                ["SumB4Tol"] = "+ 0.2",
                ["SumB4TolN"] = "- 0",

                ["SumB5"] = "(B5) 7.5",
                ["SumB5Tol"] = "+ 0.2",
                ["SumB5TolN"] = "- 0",

                ["SumB6"] = "(B6) 23.5",
                ["SumB6Tol"] = "+ 0.2",
                ["SumB6TolN"] = "- 0",

                ["SumB7"] = "(B7) 7",
                ["SumB7Tol"] = "+ 0.2",
                ["SumB7TolN"] = "- 0"
            };
        }

        private Dictionary<string, string> GetRadiiChamfers(Context ctx)
        {
            return new Dictionary<string, string>
            {
                ["SumR"] = "(R) 40 (x2)",
                ["SumR08"] = "max R0.8 (x3)",
                ["SumR1"] = "max R1",
                ["SumR2"] = "R2",
                ["SumR2a"]="R2",
                ["SumFas"] = "1x45°",
                ["SumH"] = "15°"
            };
        }

        private Dictionary<string, string> GetThreadsAndHole(Context ctx)
        {
            int x = i(ctx);

            string[] G = { "14.5", "14.5", "14.5", "14.5", "14.5", "14", "14" };
            string[] Y = { "197", "205", "213", "221", "221", "237", "245" };

            return new Dictionary<string, string>
            {
                ["SumG"] = $"(G) {G[x]}",
                ["SumGTol"] = "± 0.5",

                ["SumG2"] = "1/4-28 UNF",
                ["SumG3"] = "12",
                ["SumGmin"] = "min 6",

                ["SumBH"] = "(BH) 9",
                ["SumBHTol"] = "+ 0.220",
                ["SumBHTolN"] = "- 0",

                ["SumX"] = "(X) 115 (x2)",
                ["SumY"] = $"(Y) {Y[x]} (x2)"
            };
        }

        private Dictionary<string, string> GetAngles()
        {
            return new Dictionary<string, string>
            {
                ["Sum15"] = "15°x2",
                ["Sum225"] = "22.5°",
                ["Sum45"] = "45°x6"
            };
        }

        private Dictionary<string, string> GetSurface()
        {
            return new Dictionary<string, string>
            {
                ["SumRa32"] = "3.2",
                ["SumRa125"] = "12.5"
            };
        }

        private Dictionary<string, string> GetMachine(Context ctx)
        {
            if (string.IsNullOrEmpty(ctx.Machine))
                return new Dictionary<string, string>();

            return new Dictionary<string, string>
            {
                ["SumMaskinValS1"] = $"Maskin: {ctx.Machine} - Svarvning",
                ["SumMaskinValS2"] = "Maskin: K&T - Borrning, Fräsning"
            };
        }

        private Dictionary<string, string> GetFrequencies(Context ctx)
        {
            bool v = ctx.Machine != "";

            return new Dictionary<string, string>
            {
                ["SumF1_1"] = v ? "1/2" : "",
                ["SumF1_2"] = v ? "1/2" : "",
                ["SumF1_3"] = v ? "1/2" : "",
                ["SumF1_4"] = v ? "1/2" : "",
                ["SumF1_5"] = v ? "Inst." : "",
                ["SumF1_6"] = v ? "Inst." : "",
                ["SumF1_7"] = v ? "Inst." : "",
                ["SumF1_8"] = v ? "1/2" : "",

                ["SumF2_1"] = v ? "1/2" : "",
                ["SumF2_2"] = v ? "1/2" : "",
                ["SumF2_3"] = v ? "1/2" : "",
                ["SumF2_4"] = v ? "Inst." : "",
                ["SumF2_5"] = v ? "1/5" : "",
                ["SumF2_6"] = ""
            };
        }

        private Dictionary<string, string> GetMeasuringTools(Context ctx)
        {
            bool v = ctx.Machine != "";

            return new Dictionary<string, string>
            {
                ["SumD1_1"] = v ? "Skjutmått" : "",
                ["SumD1_2"] = v ? "Skjutmått" : "",
                ["SumD1_3"] = v ? "Skjutmått" : "",
                ["SumD1_4"] = v ? "Skjutmått/Djupmått" : "",
                ["SumD1_5"] = v ? "Skjutmått" : "",
                ["SumD1_6"] = v ? "Radielyra" : "",
                ["SumD1_7"] = v ? "Vinkelsystem" : "",
                ["SumD1_8"] = v ? "Ytjämnhetsmätare" : "",

                ["SumD2_1"] = v ? "Gängtolk min/max" : "",
                ["SumD2_2"] = v ? "Skjutmått" : "",
                ["SumD2_3"] = v ? "Skjutmått" : "",
                ["SumD2_4"] = v ? "Mätmaskin" : "",
                ["SumD2_5"] = v ? "Ytjämnhetsmätare" : "",
                ["SumD2_6"] = ""
            };
        }

        private Dictionary<string, string> GetRemarks()
        {
            return new Dictionary<string, string>
            {
                ["SumAF1_1"] = "",
                ["SumAF1_2"] = "",
                ["SumAF1_3"] = "",
                ["SumAF1_4"] = "",
                ["SumAF1_5"] = "",
                ["SumAF1_6"] = "",
                ["SumAF1_7"] = "",
                ["SumAF1_8"] = "",

                ["SumAF2_1"] = "",
                ["SumAF2_2"] = "",
                ["SumAF2_3"] = "",
                ["SumAF2_4"] = "Vid misstänkt fel lämna till mätrum.",
                ["SumAF2_5"] = "",
                ["SumAF2_6"] = ""
            };
        }

        private Dictionary<string, string> GetTexts()
        {
            return new Dictionary<string, string>
            {
                ["SumTextS1"] = "Skarpa kanter avgradas.",
                ["SumTextS2"] = "Okulär kontroll av bearbetade ytor, vid misstänkt formfel lämnas hylsan till mätrum för kontroll.<<LineBreak>><<LineBreak>> Skarpa kanter avgradas.",
                ["SumRitS1"] = "7433041:5",
                ["SumRitS2"] = "7433041:5"
            };
        }

        private string GetTolPM(string v)
        {
            var d = double.Parse(v, CultureInfo.InvariantCulture);
            if (d < 120.01) return "± 0.3";
            if (d < 400.01) return "± 0.5";
            return "± 0.8";
        }

        private string GetNegTol(string v)
        {
            var d = double.Parse(v, CultureInfo.InvariantCulture);
            if (d < 180.01) return "- 0.250";
            if (d < 250.01) return "- 0.290";
            if (d < 315.01) return "- 0.320";
            if (d < 400.01) return "- 0.360";
            return "- 0.400";
        }

        private string GetPosTolH(string v)
        {
            var d = double.Parse(v, CultureInfo.InvariantCulture);
            if (d < 180.01) return "+ 0.400";
            if (d < 250.01) return "+ 0.460";
            if (d < 315.01) return "+ 0.520";
            if (d < 400.01) return "+ 0.570";
            return "+ 0.630";
        }

        private string GetTolF(string v)
        {
            var d = double.Parse(v, CultureInfo.InvariantCulture);
            if (d < 180.01) return "± 0.315";
            if (d < 250.01) return "± 0.360";
            if (d < 315.01) return "± 0.405";
            if (d < 400.01) return "± 0.445";
            if (d < 500.01) return "± 0.485";
            return "± 0.550";
        }

        private void Merge(Dictionary<string, string> target, Dictionary<string, string> source)
        {
            foreach (var kv in source)
                target[kv.Key] = kv.Value ?? "";
        }

        private class Context
        {
            public int TypIndex { get; set; }
            public string Machine { get; set; }
        }
    }
}