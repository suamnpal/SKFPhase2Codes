using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class TATNINGAR_GR_TK_V_INCH_SMALL : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var result = Initialize();

            Merge(result, GetAdmin(request));
            Merge(result, GetDiameters(request));
            Merge(result, GetWidths(request));
            Merge(result, GetLengths(request));
            Merge(result, GetThreads(request));
            Merge(result, GetRowDefinitionsPage1(request));
            Merge(result, GetRowDefinitionsPage2(request));
            Merge(result, GetDetails(request));
            Merge(result, GetTexts());

            return result;
        }

        // ================= INITIALIZE =================
        private Dictionary<string, string> Initialize()
        {
            var d = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            string[] keys =
            {
                // Diameters
                "Sumd","SumdTol","SumdTolN","Sumd1","Sumd1Tol","Sumd1TolN",
                "Sumd2","Sumd2Tol","Sumd2TolN","Sumd3","Sumd3Tol","Sumd3TolN",
                "Sumd4","Sumd4Tol","Sumd4TolN","Sumd5","Sumd5Tol","Sumd5TolN",
                "Sumd6","Sumd6Tol","Sumd6TolN","Sumd7","Sumd7Tol","Sumd7TolN",
                "Sumd8","Sumd8Tol","Sumd8TolN","Sumd9","Sumd9Tol","Sumd9TolN",
                "SumDa","SumDaTol","SumDaTolN",

                // Widths
                "SumB","Sumb1","Sumb2","Sumb4","Sumb5","SumH",
                "SumHTol","SumHTolN","Sumb2Tol","Sumb2TolN",
                "Sumb1Tol","Sumb4Tol","Sumb4TolN","Sumb5Tol","Sumb5TolN",
                "SumBTol","SumBTolN",

                // Lengths
                "SumP","SumPTol","SumPTolN","SumE","SumETol","SumETolN",
                "SumN","SumNTol","SumNTolN","SumL",
                "SumK","SumKTol","SumKTolN",

                // Threads
                "SumG","SumGn","SumGnTol","SumGnTolN",
                "SumMi","SumMa","SumBorrtext",

                // Detail
                "SumAm","SumAmb","SumAmbTol","SumBhd","SumBhd1",

                // Geometry + surface
                "SumCo1","SumRd","SumRa32",

                // Radii
                "SumF15","SumF1","SumR1","SumR05","SumR05a","SumR05b","SumR08",

                // Page1
                "SumF1_1","SumF1_2","SumF1_3","SumF1_4","SumF1_5","SumF1_6","SumF1_7",
                "SumD1_1","SumD1_2","SumD1_3","SumD1_4","SumD1_5","SumD1_6","SumD1_7",
                "SumAF1_1","SumAF1_2","SumAF1_3","SumAF1_4","SumAF1_5","SumAF1_6","SumAF1_7",

                // Page2
                "SumF2_1","SumF2_2","SumF2_3","SumF2_4","SumF2_5","SumF2_6","SumF2_7",
                "SumD2_1","SumD2_2","SumD2_3","SumD2_4","SumD2_5","SumD2_6","SumD2_7",
                "SumAF2_1","SumAF2_2","SumAF2_3","SumAF2_4","SumAF2_5","SumAF2_6","SumAF2_7",

                // Text/Admin
                "SumRitNr","SumRitNr2","SumMaskinvalS1","SumMaskinvalS2",
                "Product","SumTextS1","SumTextS2"
            };

            foreach (var k in keys)
                d[k] = "";

            return d;
        }

        // ================= ADMIN =================
        private Dictionary<string, string> GetAdmin(APIRequest request)
        {
            string m = request?.MachineNumber ?? "";

            return new Dictionary<string, string>
            {
                ["SumRitNr"] = "11K 7433541: senaste utg. märkning: 7433523: senaste utg.",
                ["SumRitNr2"] = "11K 7433541: senaste utg. märkning: 7433523: senaste utg.",
                ["SumMaskinvalS1"] = $"Maskin: {m} - Diametrala mått",
                ["SumMaskinvalS2"] = $"Maskin: {m} - Övriga mått",
                ["Product"] = request?.ProductDesignation?.ToUpperInvariant() ?? ""
            };
        }

        // ================= DIAMETERS =================
        private Dictionary<string, string> GetDiameters(APIRequest request)
        {
            var d = new Dictionary<string, string>();

            int typ = GetTyp(request);
            var table = GetDiameterTable();
            if (!table.ContainsKey(typ)) typ = 102;

            var t = table[typ];

            d["Sumd"] = $"(d) {t[0]}";
            d["SumdTol"] = "+ 0.400";
            d["SumdTolN"] = "- 0.0";

            d["Sumd1"] = $"2x (d1) {t[1]}";
            d["Sumd1Tol"] = "+ 0.0";
            d["Sumd1TolN"] = "- 0.300";

            d["Sumd2"] = $"2x (d2) {t[2]}";
            d["Sumd2Tol"] = "+ 0.0";
            d["Sumd2TolN"] = "- 0.300";

            d["Sumd3"] = $"(d3) {t[3]}";
            d["Sumd3TolN"] = "± 0,1";

            d["Sumd4"] = $"(d4) {t[4]}";
            d["Sumd4Tol"] = "+ 0.0";
            d["Sumd4TolN"] = "- 0.300";

            d["Sumd5"] = $"(d5) {t[5]}";
            d["Sumd5Tol"] = "+ " + TolH12(t[5]);
            d["Sumd5TolN"] = "- 0.0";

            d["Sumd6"] = $"(d6) {t[6]}";
            d["Sumd6Tol"] = "+ 0.0";
            d["Sumd6TolN"] = "- " + Tolh12(t[6]);

            d["Sumd7"] = $"(d7) {t[7]}";
            d["Sumd7Tol"] = "+ " + TolH12(t[7]);
            d["Sumd7TolN"] = "- 0.0";

            d["Sumd8"] = $"(d8) {t[8]}";
            d["Sumd8Tol"] = "+ 0.0";
            d["Sumd8TolN"] = "- " + Tolh12(t[8]);

            d["Sumd9"] = $"(d9) {t[9]}";
            d["Sumd9Tol"] = "+ " + TolH12(t[9]);
            d["Sumd9TolN"] = "- 0.0";

            d["SumDa"] = $"(Da) {t[10]}";
            d["SumDaTol"] = "+ 0.0";
            d["SumDaTolN"] = "- " + TolH12(t[10]);

            return d;
        }

        // ================= WIDTHS =================
        private Dictionary<string, string> GetWidths(APIRequest request)
        {
            var d = new Dictionary<string, string>();

            int typ = GetTyp(request);
            var table = GetWidthTable();
            if (!table.ContainsKey(typ)) typ = 102;

            var t = table[typ];

            d["SumB"] = $"(B) {t[0]}";
            d["Sumb1"] = $"(b1) {t[1]}";
            d["Sumb2"] = $"(b2) {t[2]}";
            d["Sumb4"] = $"(b4) {t[3]}";
            d["Sumb5"] = $"(b5) {t[4]}";
            d["SumH"] = $"(H) {t[5]}";

            // ✅ FIXED (Lotus exact)
            d["SumBTol"] = "+ 0.500";
            d["SumBTolN"] = "- 0.0";

            d["Sumb1Tol"] = "± 0.200";

            d["Sumb2Tol"] = "+ 0.0";
            d["Sumb2TolN"] = "- 0.500";

            d["Sumb4Tol"] = "+ 0.0";
            d["Sumb4TolN"] = "- 0.200";

            d["Sumb5Tol"] = "+ 0.0";
            d["Sumb5TolN"] = "- 0.500";

            d["SumHTol"] = "+ 0.500";
            d["SumHTolN"] = "- 0.0";

            return d;
        }

        // ================= LENGTH =================
        private Dictionary<string, string> GetLengths(APIRequest request)
        {
            var d = new Dictionary<string, string>();
            int typ = GetTyp(request);

            double P = typ < 32 ? 2.8 :
                       typ < 43 ? 3.5 :
                       typ < 102 ? 3.8 :
                       typ == 184 ? 4.5 : 5;

            double E = typ < 32 ? 8.509 :
                       new[] { 32, 37, 43, 44, 53, 54 }.Contains(typ) ? 11.76 : 16.129;

            double N =
                typ < 32 ? 2 :
                typ == 102 ? 3.5 :
                new[] { 32, 37, 43, 44, 53, 54, 184 }.Contains(typ) ? 2.7 : 4.2;

            d["SumP"] = $"2x (P) {P}";
            d["SumE"] = $"(E) {E}";
            d["SumN"] = $"(N) {N}";
            d["SumL"] = typ switch
            {
                24 or 29 or 32 or 37 or 43 => "(L) 20.6",
                43 or 44 or 55 => "(L) 21",
                35 or 53 or 54 or 64 or 184 or 188 or 191 => "(L) 21.5",
                < 113 => "(L) 22.5",
                113 => "(L) 25",
                117 => "(L) 26",
                < 131 => "(L) 27",
                _ => "(L) 27.5"
            };


            d["SumPTol"] = "+0.400";
            d["SumPTolN"] = "-0.0";

            d["SumETol"] = "+0.200";
            d["SumETolN"] = "-0.0";

            d["SumHTol"] = "+0.500";
            d["SumHTolN"] = "-0.0";

            d["SumNTol"] = "+0.0";
            d["SumNTolN"] = "-0.500";

            double K =
                typ == 184 ? 4 :
                N == 2 ? 2 :
                N == 2.7 ? 3 :
                4;

            d["SumK"] = $"(K) {K}";
            d["SumKTol"] = "+ 0.400";
            d["SumKTolN"] = "- 0.0";

            return d;
        }

        // ================= THREADS =================
        private Dictionary<string, string> GetThreads(APIRequest request)
        {
            var d = new Dictionary<string, string>();

            int typ = GetTyp(request);
            var table = GetDiameterTable();
            if (!table.ContainsKey(typ)) typ = 102;

            double dVal = table[typ][0];

            d["SumG"] = "(G) M6";
            d["SumGn"] = dVal < 105 ? "(M)5" : "(M)10";
            d["SumGnTol"] = "+0.2";
            d["SumGnTolN"] = "-0.0";
            d["SumMi"] = dVal < 105 ? "Gänga Min.13" : "Gänga Min.18";
            d["SumMa"] = dVal < 105 ? "Borr Max. 15" : "Borr Max. 20";
            d["SumBorrtext"] = "Genomgående hål, borr får ej beröra tätningsplan";

            return d;
        }

        // ================= DETAILS =================
        private Dictionary<string, string> GetDetails(APIRequest request)
        {
            double dVal = GetDiameterTable()[GetTyp(request)][0];
            return new Dictionary<string, string>
            {
                ["SumAm"] = "Inriktningsmärkning",
                ["SumCo1"] = "0.15",
                ["SumRd"] = "0.10",
                ["SumRa32"] = "3.2",
                ["SumF15"] = "1.5x45°",
                ["SumF1"] = "2x45°",
                ["SumR1"] = "1x45°",
                ["SumR05"] = "max: R0.5",
                ["SumR05a"] = "0.5x45°",
                ["SumR05b"] = "R0.5",
                ["SumR08"] = "4 x max: R0.8",
                ["SumBhd"] = "Ø 3",
                ["SumBhd1"] = dVal < 105 ? "Ø 6.3" : "Ø 8",
                ["SumBhTol"] = "+0.2",
                ["SumBhTolN"] = "-0.0",
                ["SumAmb"] = "Djup 1.0   ± 0.1",
                ["SumAmbTol"] = "Bredd 2,0 ± 0.200"
            };
        }

        private int GetTyp(APIRequest request)
        {
            var parts = (request?.ProductDesignation ?? "")
                .ToUpper()
                .Split(new[] { ' ', '-', '.', '/' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var p in parts)
                if (int.TryParse(p, out int t))
                    return t;
            return 102;
        }

        private Dictionary<int, double[]> GetDiameterTable() => new()
{
    {24,new[]{50.8,53.086,60.071,61.2,63.6,74,80,91,97,108,114}},
    {29,new[]{57.15,59.69,66.675,67.8,69,79,85,96,102,113,119}},
    {32,new[]{58.42,60.198,69.596,69.2,71.6,79,85,96,102,113,119}},
    {37,new[]{63.5,66.548,75.946,75.6,78,84,90,101,107,118,124}},
    {43,new[]{69.25,76.073,85.471,85.2,87.6,89,95,106,112,123,129}},
    {44,new[]{70.85,76.073,85.471,85.2,87.6,98.5,104.5,115.5,121.5,132.5,138.5}},
    {53,new[]{77,82.423,91.821,91.5,93.9,104,110,121,127,138,144}},
    {54,new[]{78.5,82.423,91.821,91.5,93.9,104,110,121,127,138,144}},
    {102,new[]{89.9,93.472,106.81,104.5,106.9,119,125,137,143,155,161}},
    {109,new[]{103,106.17,119.51,117,119.4,129,135,147,153,165,171}},
    {113,new[]{107.95,113.03,126.36,124.2,126.6,142,149,162,169,182,189}},
    {117,new[]{114.3,120.98,134.32,132,134.4,142,149,162,169,182,189}},
    {118,new[]{117.48,120.98,134.32,132,134.4,147,155,169,177,191,199}},
    {122,new[]{127,132.08,145.41,143.4,145.8,159,167,181,189,203,211}},
    {125,new[]{135.5,142.11,155.45,153.6,156,159,167,181,189,203,211}},
    {130,new[]{141.5,145.29,161.8,162.6,165,174,182,196,204,218,226}},
            { 184,new[]{77,80.264,93.599,91.6,94,104,110,121,127,138,144}},
{188,new[]{83.55,90.297,103.630,101.8,104.2,110,116,127,133,144,150}

} };

        private Dictionary<int, double[]> GetWidthTable() => new()
        {

        {24,  new[]{36.5, 6,   13,   13,   9,   18.5}},
        {29,  new[]{38,   6,   13,   13,   8.5, 20}},
        {32,  new[]{39.5, 6,   12.5, 13,   8.5, 21.5}},
        {37,  new[]{40,   6,   13,   12.5, 8.5, 22}},
        {43,  new[]{38,   5.0,   11,   14,   7,   22}},
        {44,  new[]{43.2, 7.5, 16,   16.5, 10.5, 22.2}},
        {53,  new[]{44,   7.5, 15.5, 16.5, 10.5, 23.5}},
        {54,  new[]{44,   7.5, 15.5, 16.5, 10.5, 23.5}},
        {102,new[]{46,6,13,15.5,8.5,28}},
        {109,new[]{51.5,7.5,15,16,10,31.5}},
        {113,new[]{51.5,7.5,15,18,10,31.5}},
        {117,new[]{51,7.5,15,18,10,31}},
        {118,new[]{48,7.5,15,18,10,28}},
        {122,new[]{49.5,7.5,15,17,10,29.5}},
        {125,new[]{50.5,7.5,15,18,10,30.5}},
        {130,new[]{50.5,7.5,15,18,10,30.5}},
        {184,new[]{48.5,7.5,15.5,16.5,10.5,28}},
        {188,new[]{50,7.5,15.5,16.5,10.5,29.5}},

        };

        private Dictionary<string, string> GetRowDefinitionsPage1(APIRequest request)
        {
            var d = new Dictionary<string, string>();
            string m = request?.MachineNumber ?? "";

            if (m == "LT-3000EX" || m == "Nakamura" || m == "LB45")
            {
                d["SumF1_1"] = d["SumF1_2"] = d["SumF1_3"] = "1/5";
                d["SumF1_6"] = "Inst.";
                d["SumF1_7"] = "Inst. eller vid misstanke";

                d["SumD1_1"] = d["SumD1_3"] = "Mätmaskin Alt.Skjutmått";
                d["SumD1_2"] = "Mätmaskin Alt.UD-Apparat";
                d["SumD1_6"] = "Mätmaskin Alt.Mätservice";
                d["SumD1_7"] = "Ytjämnhetsmätare";
            }

            d["SumF1_4"] = "";
            d["SumD1_4"] = "";
            d["SumAF1_4"] = "";

            d["SumF1_5"] = "";
            d["SumD1_5"] = "";
            d["SumAF1_5"] = "";

            d["SumAF1_1"] = "";
            d["SumAF1_2"] = "";
            d["SumAF1_3"] = "";
            d["SumAF1_6"] = "";
            d["SumAF1_7"] = "";

            return d;
        }
        private Dictionary<string, string> GetRowDefinitionsPage2(APIRequest request)
        {
            var d = new Dictionary<string, string>();
            string m = request?.MachineNumber ?? "";

            if (m == "LT-3000EX" || m == "Nakamura" || m == "LB45")
            {
                d["SumF2_1"] = d["SumF2_2"] = d["SumF2_3"] =
                d["SumF2_4"] = d["SumF2_5"] = d["SumF2_7"] = "1/5";

                d["SumF2_6"] = "Inst. eller vid misstanke";

                d["SumD2_1"] = d["SumD2_2"] = d["SumD2_3"] =
                d["SumD2_5"] = d["SumD2_7"] = "Mätmaskin Alt.Skjutmått";

                d["SumD2_4"] = "Gängtolk";
                d["SumD2_6"] = "Ytjämnhetsmätare";
            }

            d["SumAF2_1"] = "";
            d["SumAF2_2"] = "";
            d["SumAF2_3"] = "";
            d["SumAF2_4"] = "";
            d["SumAF2_5"] = "";
            d["SumAF2_6"] = "";
            d["SumAF2_7"] = "";

            return d;

        }
        private Dictionary<string, string> GetTexts()
        {
            string text = "Okulärkontroll Grader, frifläckar, slagmärken, repor, valkar, ytjämnhet och faser, skarpa kanter avgradas.";

            return new Dictionary<string, string>
            {
                ["SumTextS1"] = text,
                ["SumTextS2"] = text,
                ["SumTextS3"] = "Kontrollera stämpel på förstabit."
            };
        }

        private string TolH12(double d)
        {
            if (d < 3.01) return "0.060";
            if (d < 6.01) return "0.120";
            if (d < 10.01) return "0.150";
            if (d < 18.01) return "0.180";
            if (d < 30.01) return "0.210";
            if (d < 50.01) return "0.250";
            if (d < 80.01) return "0.300";
            if (d < 120.01) return "0.350";
            if (d < 180.01) return "0.400";
            if (d < 250.01) return "0.460";
            if (d < 315.01) return "0.520";
            if (d < 400.01) return "0.570";
            if (d < 500.01) return "0.630";
            return "0.700";
        }
        private string Tolh12(double d) => TolH12(d);
        private void Merge(Dictionary<string, string> target, Dictionary<string, string> source)
        {
            foreach (var kv in source)
                target[kv.Key] = kv.Value ?? "";
        }
    }
}