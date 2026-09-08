using DocumentFormat.OpenXml.Wordprocessing;
using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class TATNINGAR_GR_TK_V_INCH_167_978 : ITemplateCalculations
    {
        
        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var kv = new Dictionary<string, string>();

            kv["DocumentUniqueId"] =
                DateTime.UtcNow.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);

            string product = NormalizeProduct(request.ProductDesignation);
            string machine = NormalizeMachine(request.MachineNumber);

            string type = GetType(product);

            Merge(kv, GetAdminTexts(type));
            Merge(kv, GetDiameters(type));
            Merge(kv, GetWidths(type));
            Merge(kv, Geometry(type));
            Merge(kv, GetThreadsAndBores(type));
            Merge(kv, GetRadiiAndChamfers(type));
            Merge(kv, GetSurface());
            Merge(kv, GetMachineTexts(machine));
            Merge(kv, GetPage1Measurements(machine));
            Merge(kv, GetPage2Measurements(machine));

            return kv;
        }

        // ---------------- COMMON ----------------

        private void Merge(Dictionary<string, string> t, Dictionary<string, string> s)
        {
            foreach (var e in s)
                t[e.Key] = e.Value ?? "";
        }

        private string NormalizeProduct(string v) =>
            string.IsNullOrWhiteSpace(v) ? "GR-TK 167 V" : v.Trim().ToUpperInvariant();

        private string NormalizeMachine(string v) =>
            string.IsNullOrWhiteSpace(v) ? "" : v.Trim();

        private string GetType(string product)
        {
            return product.Split(' ')
                .FirstOrDefault(x => x.All(char.IsDigit)) ?? "167";
        }

        // ---------------- TABLES ----------------
        private decimal GetH12Tolerance(decimal value)
        {
            if (value < 3.01m) return 0.100m;
            if (value < 6.01m) return 0.120m;
            if (value < 10.01m) return 0.150m;
            if (value < 18.01m) return 0.180m;
            if (value < 30.01m) return 0.210m;
            if (value < 50.01m) return 0.250m;
            if (value < 80.01m) return 0.300m;
            if (value < 120.01m) return 0.350m;
            if (value < 180.01m) return 0.400m;
            if (value < 250.01m) return 0.460m;
            if (value < 315.01m) return 0.520m;
            if (value < 400.01m) return 0.570m;
            if (value < 500.01m) return 0.630m;
            if (value < 630.01m) return 0.700m;

            return 0.800m;
        }


        private readonly Dictionary<string, decimal[]> DiameterTable = new()
        {
            ["167"] = new decimal[] { 205, 210.57m, 227.58m, 228.5m, 230.9m, 257, 265, 280, 288, 304, 312 },
            ["513"] = new decimal[] { 232, 236.47m, 252.98m, 253.7m, 256.1m, 286, 294, 310, 318, 334, 342 },
            ["178"] = new decimal[] { 246, 253.95m, 270.46m, 271.1m, 273.5m, 299, 307, 325, 333, 351, 359 },
            ["842"] = new decimal[] { 246, 251.71m, 276.38m, 277.5m, 279.9m, 299, 307, 325, 333, 351, 359 },
            ["606"] = new decimal[] { 269, 275.59m, 292.1m, 293m, 295.4m, 319, 327, 345, 353, 371, 379 },
            ["872"] = new decimal[] { 346m, 356.87m, 385.166m, 381.4m, 383.8m, 396m, 404m,  424m, 432m,  452m, 460m },
            ["884"] = new decimal[] { 309, 318.26m, 346.56m, 343.5m, 345.9m, 359, 367, 385, 393, 410, 419 },
            ["907"] = new decimal[] { 406, 415.04m, 446.94m, 444m, 446.4m, 463, 471, 491, 499, 519, 527 },
            ["888"] = new decimal[] { 464, 473.71m, 505.61m, 502.5m, 504.9m, 517, 525, 545, 553, 573, 581 },
            ["978"] = new decimal[] { 478, 486.92m, 518.82m, 516m, 518.4m, 531, 539, 559, 567, 587, 595 }
        };

        private readonly Dictionary<string, (
     decimal B,
     decimal b1,
     decimal b2,
     decimal b4,
     decimal b5,
     decimal H,
     decimal P,
     decimal E,
     decimal N,
     decimal K,
     decimal L
 )> WidthTable = new()
 {
     ["167"] = (50, 7.5m, 16, 16, 10.5m, 28, 5, 16.129m, 4.2m, 4, 30),
     ["513"] = (50, 7.5m, 16, 16, 10.5m, 29, 5, 16.129m, 4.2m, 4, 29),
     ["178"] = (50, 7.5m, 16, 16, 10.5m, 28, 5, 16.129m, 4.2m, 4, 32),
     ["842"] = (62, 7.5m, 16, 16, 10.5m, 40, 9.8m, 27.1m, 5.6m, 5, 32),
     ["606"] = (53, 7.5m, 16, 16, 10.5m, 31, 5, 16.129m, 4.2m, 4, 31.5m),
     ["872"] = (74.5m, 10.5m, 19.5m, 21m,13.5m, 49m, 10.786m, 31.57m, 6.2m, 6m, 36m ),
     ["884"] = (71, 7.5m, 16, 16, 10.5m, 49, 10, 31.57m, 6.2m, 6, 32),
     ["907"] = (80.5m, 10.5m, 20m, 21, 13.5m, 54.356m, 13m, 36m, 6.2m, 6, 35),
     ["888"] = (81.5m, 12, 20m, 22.5m, 15, 55, 13, 35.992m, 6.2m, 6, 35),
     ["978"] = (81.5m, 12, 21.5m, 22.5m, 15, 53.5m, 13, 36m, 6, 6, 35)
 };

     
        private Dictionary<string, string> GetAdminTexts(string type) => new()
        {
            ["SumRitNr"] = (type == "872" ? "":$"TK {type} V") + ": senaste utg, märkning: 7433523:senaste utg.",
            ["SumRitNr2"] = (type == "872" ? "" : $"TK {type} V") + ": senaste utg, märkning: 7433523:senaste utg.",
            ["SumTextS1"] = "Okulärkontroll Grader, frifläckar, slagmärken, repor, valkar, ytjämnhet och faser, skarpa kanter avgradas.",
            ["SumTextS2"] = "Okulärkontroll Grader, frifläckar, slagmärken, repor, valkar, ytjämnhet och faser, skarpa kanter avgradas.",
            ["SumTextS3"] = "Kontrollera stämpel på förstabit."
        };

        private Dictionary<string, string> GetDiameters(string type)
        {
            var d = DiameterTable.ContainsKey(type)
                ? DiameterTable[type]
                : DiameterTable["167"];

            string f(decimal v) => v.ToString(CultureInfo.InvariantCulture);

            decimal tol5 = GetH12Tolerance(d[5]);
            decimal tol6 = GetH12Tolerance(d[6]);
            decimal tol7 = GetH12Tolerance(d[7]);
            decimal tol8 = GetH12Tolerance(d[8]);
            decimal tol9 = GetH12Tolerance(d[9]);
            decimal tolA = GetH12Tolerance(d[10]);

            return new Dictionary<string, string>
            {
                ["Sumd"] = "(d) " + f(d[0]),
                ["SumdTol"] = "+ 0.400",
                ["SumdTolN"] = "- 0.0",

                ["Sumd1"] = "2x (d1) " + f(d[1]),
                ["Sumd1Tol"] = "+ 0.0",
                ["Sumd1TolN"] = "- 0.300",

                ["Sumd2"] = "2x (d2) " + f(d[2]),
                ["Sumd2Tol"] = "+ 0.0",
                ["Sumd2TolN"] = "- 0.300",

                ["Sumd3"] = "(d3) " + f(d[3]),
                ["Sumd3Tol"] = "+ 0.100",
                ["Sumd3TolN"] = "- 0.100",

                ["Sumd4"] = "(d4) " + f(d[4]),
                ["Sumd4Tol"] = "+ 0.0",
                ["Sumd4TolN"] = "- 0.300",

               
                ["Sumd5"] = "(d5) " + f(d[5]),
                ["Sumd5Tol"] = "+ " + f(tol5),
                ["Sumd5TolN"] = "- 0.0",

                ["Sumd6"] = "(d6) " + f(d[6]),
                ["Sumd6Tol"] = "+ 0.0",
                ["Sumd6TolN"] = "- " + f(tol6),

                ["Sumd7"] = "(d7) " + f(d[7]),
                ["Sumd7Tol"] = "+ " + f(tol7),
                ["Sumd7TolN"] = "- 0.0",

                ["Sumd8"] = "(d8) " + f(d[8]),
                ["Sumd8Tol"] = "+ 0.0",
                ["Sumd8TolN"] = "- " + f(tol8),

                ["Sumd9"] = "(d9) " + f(d[9]),
                ["Sumd9Tol"] = "+ " + f(tol9),
                ["Sumd9TolN"] = "- 0.0",

                ["SumDa"] = "(Da) " + f(d[10]),
                ["SumDaTol"] = "+ 0.0",
                ["SumDaTolN"] = "- " + f(tolA),

                ["SumRd"] = "0.10",
                ["SumCo1"] = "0.15"
            };
        }


        private Dictionary<string, string> GetWidths(string product)
        {
            var type = GetType(product);

            var w = WidthTable.ContainsKey(type)
                ? WidthTable[type]
                : WidthTable["167"];

            string f(decimal v) => v.ToString(CultureInfo.InvariantCulture);

            return new Dictionary<string, string>
            {
                ["SumB"] = "(B) " + f(w.B),
                ["SumBTol"] = "+ 0.500",
                ["SumBTolN"] = "- 0.0",

                ["Sumb1"] = "(b1) " + f(w.b1),
                ["Sumb1Tol"] = "± 0.200",

                ["Sumb2"] = Convert.ToInt32(type) < 872 ? "1x" :"2x" +"(b2) " + f(w.b2),
                ["Sumb2Tol"] = "+ 0.0",
                ["Sumb2TolN"] = "- 0.500",

                ["Sumb4"] = "(b4) " + f(w.b4),
                ["Sumb4Tol"] = "+ 0.200",
                ["Sumb4TolN"] = "- 0.200",

                ["Sumb5"] = "(b5) " + f(w.b5),
                ["Sumb5Tol"] = "+ 0.0",
                ["Sumb5TolN"] = "- 0.500",

                ["SumH"] = "(H) " + f(w.H),
                ["SumHTol"] = "+ 0.500",
                ["SumHTolN"] = "- 0.0",

                ["SumP"] = "2x (P) " + f(w.P),
                ["SumPTol"] = "+ 0.200",
                ["SumPTolN"] = "- 0.0",

                ["SumE"] = "(E) " + f(w.E),
                ["SumETol"] = "+ 0.200",
                ["SumETolN"] = "- 0.0",

                ["SumN"] = "(N) " + f(w.N),
                ["SumNTol"] = "+ 0.0",
                ["SumNTolN"] = "- 0.500",

                ["SumK"] = "(K) " + f(w.K),
                ["SumKTol"] = "+ 0.400",
                ["SumKTolN"] = "- 0.0",

                ["SumL"] = "(L) " + f(w.L)
            };
        }

        private Dictionary<string, string> Geometry(string type)
        {
            var d = DiameterTable.ContainsKey(type) ? DiameterTable[type][0] : 200;

            return new Dictionary<string, string>
            {
                ["SumF3"] = d < 400 ? "1x45°" : "3x45°",
                ["SumF15"] = d < 400 ? "1.5x45°" : "3x45°",
                ["SumR05"] = "max: R0.5",
                ["SumR08"] = "6 x max: R0.8",
                ["SumBorrtext"] = "Genomgående hål, borr får ej beröra tätningsplan"
            };
        }

        private Dictionary<string, string> GetThreadsAndBores(string type) => new()
        {
            ["SumG"] = "(G) M6",
            ["SumMi"] = "Gänga Min.23",
            ["SumMa"] = type == "167" ? "Borr Max.25" : "Borr Max.28",
            ["SumGn"] = "15",
            ["SumGnTol"] = "+0.5",
            ["SumGnTolN"] = "-0.0",
            ["SumBhd"] = "Ø 3",
            ["SumBhd1"] = "Ø 8 +0,1"
        };

        private Dictionary<string, string> GetRadiiAndChamfers(string type ) => new()
        {
            ["SumF1"] = Convert.ToInt32(type) > 872 ? "3x45°" : "1x45°",
            ["SumF25"] = "2.5x45° (2x)",
            ["SumF65"] = "Gängfas Ø 6.5 +0.5",

            ["SumR05a"] = "0.5x45°",
            ["SumR05b"] = Convert.ToInt32(type) > 872 ? "R1.2" :"R0.5",
            ["SumR02"] = "max: R0.2",
            ["SumR1"] =  "1x45°",

            ["SumAm"] = "Inriktningsmärkning",
            ["SumAmb"] = "Bredd 3,0",
            ["SumAmbTol"] = "± 0.200",
            ["SumAmdj"] = "+ 0.5",
            ["SumAmdjN"] = "Djup 1,0 + 0.25"
        };

        private Dictionary<string, string> GetSurface() => new()
        {
            ["SumRa32"] = "3.2"
        };

        private Dictionary<string, string> GetMachineTexts(string machine) => new()
        {
            ["SumMaskinvalS1"] = $"Maskin: {machine} - Diametrala mått",
            ["SumMaskinvalS2"] = $"Maskin: {machine} - Övriga mått"
        };

        private Dictionary<string, string> GetPage1Measurements(string machine)
        {
            bool valid = IsValidMachine(machine);

            return new Dictionary<string, string>
            {
                ["SumF1_1"] = valid ? "1/2" : "",
                ["SumF1_2"] = valid ? "1/2" : "",
                ["SumF1_3"] = valid ? "1/2" : "",
                ["SumF1_4"] = "",
                ["SumF1_5"] = "",
                ["SumF1_6"] = valid ? "Inst." : "",
                ["SumF1_7"] = valid ? "Inst. alt misstanke" : "",

                ["SumD1_1"] = valid ? "Mätmaskin Alt.Skjutmått" : "",
                ["SumD1_2"] = valid ? "Mätmaskin Alt.UD-apparat eller Mikrometer" : "",
                ["SumD1_3"] = valid ? "Mätmaskin Alt.Skjutmått" : "",
                ["SumD1_4"] = "",
                ["SumD1_5"] = "",
                ["SumD1_6"] = valid ? "Mätmaskin Alt.Mätservice" : "",
                ["SumD1_7"] = valid ? "Ytjämnhetsmätare" : "",

                ["SumAF1_1"] = "",
                ["SumAF1_2"] = "",
                ["SumAF1_3"] = "",
                ["SumAF1_4"] = "",
                ["SumAF1_5"] = "",
                ["SumAF1_6"] = "",
                ["SumAF1_7"] = ""
            };
        }
        private Dictionary<string, string> GetPage2Measurements(string machine)
        {
            bool valid = IsValidMachine(machine);

            return new Dictionary<string, string>
            {
                ["SumF2_1"] = valid ? "1/2" : "",
                ["SumF2_2"] = valid ? "1/2" : "",
                ["SumF2_3"] = valid ? "1/2" : "",
                ["SumF2_4"] = valid ? "1/2" : "",
                ["SumF2_5"] = valid ? "1/2" : "",
                ["SumF2_6"] = valid ? "Inst. alt misstanke" : "",
                ["SumF2_7"] = valid ? "1/2" : "",

                ["SumD2_1"] = valid ? "Mätmaskin Alt.Skjutmått" : "",
                ["SumD2_2"] = valid ? "Mätmaskin Alt.Skjutmått" : "",
                ["SumD2_3"] = valid ? "Mätmaskin Alt.Skjutmått" : "",
                ["SumD2_4"] = valid ? "Gängtolk" : "",
                ["SumD2_5"] = valid ? "Mätmaskin Alt.Skjutmått" : "",
                ["SumD2_6"] = valid ? "Ytjämnhetsmätare" : "",
                ["SumD2_7"] = valid ? "Mätmaskin Alt.Skjutmått" : "",

                ["SumAF2_1"] = "",
                ["SumAF2_2"] = "",
                ["SumAF2_3"] = "",
                ["SumAF2_4"] = "",
                ["SumAF2_5"] = "",
                ["SumAF2_6"] = "",
                ["SumAF2_7"] = ""
            };
        }

        private bool IsValidMachine(string machine)
        {
            if (string.IsNullOrWhiteSpace(machine))
                return false;

            return machine.Contains("Nakamura", StringComparison.OrdinalIgnoreCase)
                || machine.Contains("LB45", StringComparison.OrdinalIgnoreCase)
                || machine.Contains("LT-3000EX", StringComparison.OrdinalIgnoreCase);

        }

    }
}