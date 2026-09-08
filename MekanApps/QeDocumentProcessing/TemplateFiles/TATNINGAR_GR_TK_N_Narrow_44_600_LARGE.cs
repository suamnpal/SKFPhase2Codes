using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class TATNINGAR_GR_TK_N_Narrow_44_600_LARGE : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var keyValues = new Dictionary<string, string>();

            keyValues["DocumentUniqueId"] =
                DateTime.UtcNow.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);

            string productDesignation = (request.ProductDesignation ?? "")
                .Trim().ToUpperInvariant();

            var parts = productDesignation
                .Replace(".", ",")
                .Split(new[] { ' ', '/', '.' }, StringSplitOptions.RemoveEmptyEntries);

            int typeCode = parts.Length > 1 && int.TryParse(parts[1], out int t) ? t : 0;
            int normalizedType = typeCode == 30 ? 500 : typeCode;

            bool isSupportedMachine = IsMachineMatch(request.MachineNumber);

            keyValues["SumMaskinValS1"] =
                $"Maskin: {request.MachineNumber} - Diametrala mått, Form & läge.";
            keyValues["SumMaskinValS2"] =
                $"Maskin: {request.MachineNumber} - Övriga mått.";
            keyValues["SumMaskinvalS1"] = keyValues["SumMaskinValS1"];
            keyValues["SumMaskinvalS2"] = keyValues["SumMaskinValS2"];

            Merge(keyValues, CalculateDiameters(normalizedType));
            Merge(keyValues, CalculateWidths(normalizedType));
            Merge(keyValues, CalculateLength(normalizedType));
            Merge(keyValues, CalculateThreads());
            Merge(keyValues, CalculateChamfersAndRadii());
            Merge(keyValues, CalculatePage1Measurements(isSupportedMachine));
            Merge(keyValues, CalculatePage2Measurements(isSupportedMachine));
            Merge(keyValues, CalculateAdminFields(request));

            return keyValues;
        }

        private static void Merge(Dictionary<string, string> t, Dictionary<string, string> s)
        {
            foreach (var e in s) t[e.Key] = e.Value ?? "";
        }

        private static bool IsMachineMatch(string m)
        {
            if (string.IsNullOrWhiteSpace(m)) return false;
            m = m.ToUpperInvariant();
            return m.Contains("NAKAMURA") || m.Contains("LB45") || m.Contains("LT-3000EX");
        }

        private static string FormatNumber(double v)
        {
            if (Math.Abs(v % 1) < 0.000001) return v.ToString("0", CultureInfo.InvariantCulture);
            if (Math.Abs((v * 10) % 1) < 0.000001) return v.ToString("0.0", CultureInfo.InvariantCulture);
            return v.ToString("0.000", CultureInfo.InvariantCulture);
        }

        private Dictionary<string, string> CalculateDiameters(int typeCode)
        {
            var keyValues = new Dictionary<string, string>();

            int normalizedType = typeCode == 30 ? 500 : typeCode;

            var diameterTables = new Dictionary<int, double[]>
            {
                { 44, new[]{0.0, 234.8, 232.8, 220.8, 204, 256,  264, 280, 288,   304,  312, 320  } },
                { 48, new[] { 224, 254.8, 252.8, 240.8, 224, 276, 284, 300, 308, 324, 332  } },
                { 52, new[] { 244.0,   274.6,   272.6,   260.6,   244.0,    299.0,   307.0,  325.0, 333.0, 351.0, 359.0 } },
                { 56, new[] { 264.0, 294.8,  292.8,  280.8,  264.0,  319.0,   327.0,  345.0,  353.0,  371.0, 379.0  } },
                { 60, new[]{ 284.0,  314.8,  312.8,  300.8,  284.0,  339.0, 347.0,  365.0,  373.0,  391.0, 399.0  } },
                { 64, new[] { 334.8, 334.8, 332.8, 320.8,  309.0,  359.0, 367.0,  385.0,  393.0,  410.0, 419.0  } },
            };

            var diameterToleranceTables = new Dictionary<int, string[]>
            {
                { 44, new[] { "+ 0.520","- 0.0","+ 0.0","- 0.520","+ 0.520","- 0.0","+ 0.0","- 0.520","+ 0.520","- 0.0","+ 0.0","- 0.520" } },
                { 48, new[] { "+ 0.520","- 0.0", "+ 0.0","- 0.520", "+ 0.520","- 0.0", "+ 0.0","- 0.520", "+ 0.570","- 0.0", "+ 0.0","- 0.570" } },
                { 52, new[] { "+ 0.520","- 0.0","+ 0.0","- 0.520","+ 0.570","- 0.0","+ 0.0","- 0.570","+ 0.570","- 0.0","+ 0.0","- 0.570" } },
                { 56, new[] { "+ 0.570","- 0.0","+ 0.0","- 0.570","+ 0.570","- 0.0","+ 0.0","- 0.570","+ 0.570","- 0.0","+ 0.0","- 0.570" } },
                { 60, new[] { "+ 0.570","- 0.0","+ 0.0","- 0.570","+ 0.570","- 0.0","+ 0.0","- 0.570","+ 0.570","- 0.0","+ 0.0","- 0.570" } },
                { 64, new[] { "+ 0.570","- 0.0","+ 0.0","- 0.570","+ 0.570","- 0.0","+ 0.0","- 0.570","+ 0.630","- 0.0","+ 0.0","- 0.630" } }, 
                { -1, new[] { "+ 0.700","- 0.0","+ 0.0","- 0.700","+ 0.700","- 0.0","+ 0.0","- 0.700","+ 0.700","- 0.0","+ 0.0","- 0.700" } }
            };

            if (!diameterTables.TryGetValue(normalizedType, out var d))
                d = new double[11];

            var tol = diameterToleranceTables.ContainsKey(normalizedType)
                ? diameterToleranceTables[normalizedType]
                : diameterToleranceTables[-1];

            keyValues["Sumd"] = "(d) " + FormatNumber(d[0]);
            keyValues["SumdTol"] = "+ 0.400";
            keyValues["SumdTolN"] = "- 0.0";

            keyValues["Sumd1"] = "2x (d1) " + FormatNumber(d[1]);
            keyValues["Sumd1Tol"] = "+ 0.0";
            keyValues["Sumd1TolN"] = "- 0.300";

            keyValues["Sumd2"] = "(d2) " + FormatNumber(d[2]);
            keyValues["Sumd2Tol"] = "± 0.100";
            keyValues["Sumd2TolN"] = "";

            keyValues["Sumd3"] = "2x (d3) " + FormatNumber(d[3]);
            keyValues["Sumd3Tol"] = "+ 0.0";
            keyValues["Sumd3TolN"] = "- 0.400";

            keyValues["Sumd4"] = "(d4) " + FormatNumber(d[4]);
            keyValues["Sumd4Tol"] = "+ 0.400";
            keyValues["Sumd4TolN"] = "- 0.0";

            keyValues["Sumd7"] = "(d7) " + FormatNumber(d[5]);
            keyValues["Sumd7Tol"] = tol[0];
            keyValues["Sumd7TolN"] = tol[1];

            keyValues["Sumd8"] = "(d8) " + FormatNumber(d[6]);
            keyValues["Sumd8Tol"] = tol[2];
            keyValues["Sumd8TolN"] = tol[3];

            keyValues["Sumd9"] = "(d9) " + FormatNumber(d[7]);
            keyValues["Sumd9Tol"] = tol[4];
            keyValues["Sumd9TolN"] = tol[5];

            keyValues["Sumd10"] = "(d10) " + FormatNumber(d[8]);
            keyValues["Sumd10Tol"] = tol[6];
            keyValues["Sumd10TolN"] = tol[7];

            keyValues["Sumd11"] = "(d11) " + FormatNumber(d[9]);
            keyValues["Sumd11Tol"] = tol[8];
            keyValues["Sumd11TolN"] = tol[9];

            keyValues["Sumd12"] = "(d12) " + FormatNumber(d[10]);
            keyValues["Sumd12Tol"] = tol[10];
            keyValues["Sumd12TolN"] = tol[11];

            keyValues["Sumd5"] = "(d5) 8";
            keyValues["Sumd5Tol"] = "+ 0.100";
            keyValues["Sumd5TolN"] = "- 0.0";

            keyValues["Sumd13"] = "(d13) 3";
            keyValues["Sumd13Tol"] = "± 0.100";
            keyValues["Sumd13TolN"] = "";

            return keyValues;
        }

        private Dictionary<string, string> CalculateWidths(int type)
        {

            if (type == 48)
            {
                return new Dictionary<string, string>
                {
                    ["SumS1"] = "(S1) 1.0",
                    ["SumS1Tol"] = "+ 0.500",
                    ["SumS1TolN"] = "+ 0.250",

                    ["SumB"] = "(B) 46",
                    ["SumBTol"] = "+ 0.500",
                    ["SumBTolN"] = "- 0.0",

                    ["Sumb1"] = "(b1) 22.5",
                    ["Sumb1Tol"] = "+ 0.500",
                    ["Sumb1TolN"] = "- 0.0",

                    ["Sumb2"] = "(b2) 10",
                    ["Sumb2Tol"] = "+ 0.0",
                    ["Sumb2TolN"] = "- 0.500",

                    ["Sumb3"] = "(b3) 16",
                    ["Sumb3Tol"] = "+ 0.0",
                    ["Sumb3TolN"] = "- 0.500",

                    ["Sumb4"] = "(b4) 6",
                    ["Sumb4Tol"] = "+ 0.0",
                    ["Sumb4TolN"] = "- 0.500",

                    ["Sumb5"] = "2x (b5) 10.5",
                    ["Sumb5Tol"] = "+ 0.0",
                    ["Sumb5TolN"] = "- 0.500",

                    ["Sumb6"] = "(b6) 7.5",
                    ["Sumb6Tol"] = "± 0.200",
                    ["Sumb6TolN"] = "",

                    ["Sumb7"] = "(b7) 6",
                    ["Sumb7Tol"] = "+ 0.400",
                    ["Sumb7TolN"] = "- 0.0",

                    ["Sumb8"] = "",
                    ["Sumb8Tol"] = "",
                    ["Sumb8TolN"] = "",

                    ["Sumb9"] = "(b9) 16",
                    ["Sumb9Tol"] = "+ 0.0",
                    ["Sumb9TolN"] = "- 0.200"
                };
            }
            if (type == 44)
            {
                return new Dictionary<string, string>
                {
                    ["SumS1"] = "(S1) 1.0",
                    ["SumS1Tol"] = "+ 0.500",
                    ["SumS1TolN"] = "+ 0.250",

                    ["Sumd14"] = "Ø 8",
                    ["Sumd14Tol"] = "+ 0.100",
                    ["Sumd14TolN"] = "- 0.0",

                    ["Sumd15"] = "Ø 3",
                    ["Sumd15Tol"] = "± 0.100",

                    ["SumH"] = "(H) 36",
                    ["SumHTol"] = "+ 0.500",
                    ["SumHTolN"] = "- 0.0",

                    ["SumB"] = "(B) 46",
                    ["SumBTol"] = "+ 0.500",
                    ["SumBTolN"] = "- 0.0",

                    ["Sumb1"] = "(b1) 22.5",
                    ["Sumb1Tol"] = "+ 0.500",
                    ["Sumb1TolN"] = "- 0.0",

                    ["Sumb2"] = "(b2) 10",
                    ["Sumb2Tol"] = "+ 0.0",
                    ["Sumb2TolN"] = "- 0.500",

                    ["Sumb3"] = "(b3) 16",
                    ["Sumb3Tol"] = "+ 0.0",
                    ["Sumb3TolN"] = "- 0.500",

                    ["Sumb4"] = "(b4) 6",
                    ["Sumb4Tol"] = "+ 0.0",
                    ["Sumb4TolN"] = "- 0.500",

                    ["Sumb5"] = "2x (b5) 10.5",
                    ["Sumb5Tol"] = "+ 0.0",
                    ["Sumb5TolN"] = "- 0.500",

                    ["Sumb6"] = "(b6) 7.5",
                    ["Sumb6Tol"] = "± 0.200",
                    ["Sumb6TolN"] = "",

                    ["Sumb7"] = "(b7) 6",
                    ["Sumb7Tol"] = "+ 0.400",
                    ["Sumb7TolN"] = "- 0.0",

                    ["Sumb8"] = "",
                    ["Sumb8Tol"] = "",
                    ["Sumb8TolN"] = "",

                    ["Sumb9"] = "(b9) 16",
                    ["Sumb9Tol"] = "+ 0.0",
                    ["Sumb9TolN"] = "- 0.200"
                };
            }
            return new Dictionary<string, string>
            {
                ["SumS1"] = "(S1) 1.0",
                ["SumS1Tol"] = "+ 0.500",
                ["SumS1TolN"] = "+ 0.250",

                ["Sumd14"] = "Ø 8",
                ["Sumd14Tol"] = "+ 0.100",
                ["Sumd14TolN"] = "- 0.0",

                ["Sumd15"] = "Ø 3",
                ["Sumd15Tol"] = "± 0.100",

                ["SumH"] = "(H) 36",
                ["SumHTol"] = "+ 0.500",
                ["SumHTolN"] = "- 0.0",

                ["SumB"] = "(B) 46",
                ["SumBTol"] = "+ 0.500",
                ["SumBTolN"] = "- 0.0",

                ["Sumb1"] = "(b1) 22.5",
                ["Sumb1Tol"] = "+ 0.500",
                ["Sumb1TolN"] = "- 0.0",

                ["Sumb2"] = "(b2) 10",
                ["Sumb2Tol"] = "+ 0.0",
                ["Sumb2TolN"] = "- 0.500",

                ["Sumb3"] = "(b3) 16",
                ["Sumb3Tol"] = "+ 0.0",
                ["Sumb3TolN"] = "- 0.500",

                ["Sumb4"] = "(b4) 6",
                ["Sumb4Tol"] = "+ 0.0",
                ["Sumb4TolN"] = "- 0.500",

                ["Sumb5"] = "2x (b5) 10.5",
                ["Sumb5Tol"] = "+ 0.0",
                ["Sumb5TolN"] = "- 0.500",

                ["Sumb6"] = "(b6) 7.5",
                ["Sumb6Tol"] = "± 0.200",
                ["Sumb6TolN"] = "",

                ["Sumb7"] = "(b7) 6",
                ["Sumb7Tol"] = "+ 0.400",
                ["Sumb7TolN"] = "- 0.0",

                ["Sumb8"] = "",
                ["Sumb8Tol"] = "",
                ["Sumb8TolN"] = "",

                ["Sumb9"] = "(b9) 16",
                ["Sumb9Tol"] = "+ 0.0",
                ["Sumb9TolN"] = "- 0.200"
            };
        }

        private Dictionary<string, string> CalculateLength(int type)
        {
            int L = (type == 44 || type >= 72) ? 34 : 32;
            return new Dictionary<string, string>
            {
                ["SumL"] = "(L) " + L,
                ["SumLTol"] = "+ 0.500",
                ["SumLTolN"] = "- 0.0"
            };
        }

        private Dictionary<string, string> CalculateThreads() => new()
        {
            ["SumG"] = "(G) M6",
            ["SumGa"] = "(G) M6",
            ["SumG1"] = "(G1) 15",
            ["SumG1Tol"] = "+ 0.500",
            ["SumG1TolN"] = "- 0.0",
            ["SumG2"] = "min.23",
            ["SumG3"] = "max.28",
            ["SumG4"] = "(G4) 32",
            ["SumG4Tol"] = "+ 0.500",
            ["SumG4TolN"] = "- 0.0",
        };

        private Dictionary<string, string> CalculateChamfersAndRadii() => new()
        {
            ["SumRd"] = "0.100",
            ["SumR08"] = "R max: 0.8 (4x)",
            ["SumR05a"] = "max R0.5 (3x)",
            ["SumRa"] = "max R1.2",
            ["SumRa32"] = "3.2",
            ["SumRa32a"] = "3.2",
            ["SumF1"] = "1x45°",
            ["SumF2"] = "1.5x45°",
            ["SumF3"] = "1x45°",
            ["SumF4"] = "0.5x45° (2x)",
            ["SumCo1"] = "0.150"
        };

        private Dictionary<string, string> CalculatePage1Measurements(bool ok) => new()
        {
            ["SumF1_1"] = ok ? "1/2" : "",
            ["SumD1_1"] = ok ? "Mätmaskin Alt.Skjutmått" : "",
            ["SumAF1_1"] = "",
            ["SumF1_2"] = ok ? "1/2" : "",
            ["SumD1_2"] = ok ? " Mätmaskin Alt.UD-apparat eller Mikrometer " : "",
            ["SumAF1_2"] = "",
            ["SumF1_3"] = ok ? "1/2" : "",
            ["SumD1_3"] = ok ? "Mätmaskin Alt.Skjutmått" : "",
            ["SumAF1_3"] = "",
            ["SumF1_4"] = ok ? "Inst. / alt misstanke" : "",
            ["SumD1_4"] = ok ? "Ytjämnhetsmätare" : "",
            ["SumAF1_4"] = "Övriga bearbetade ytor 6.3",
            ["SumF1_5"] = ok ? "Inst." : "",
            ["SumD1_5"] = ok ? "Mätmaskin Alt.Mätservice" : "",
            ["SumAF1_5"] = "Vid misstänkt formfel, lämna till mätrum",
            ["SumF1_6"] = ok ? "Inst." : "",
            ["SumD1_6"] = ok ? "Mätmaskin Alt.Mätservice" : "",
            ["SumAF1_6"] = "Vid misstänkt formfel, lämna till mätrum",
            ["SumF1_7"] = ok ? "Inst." : "",
            ["SumD1_7"] = ok ? "Mätmaskin Alt.Skjutmått" : "",
            ["SumAF1_7"] = ""
        };

        private Dictionary<string, string> CalculatePage2Measurements(bool ok) => new()
        {
            ["SumF2_1"] = ok ? "1/2" : "",
            ["SumD2_1"] = ok ? "Mätmaskin Alt.Skjutmått" : "",
            ["SumAF2_1"] = "",
            ["SumF2_2"] = ok ? "1/2" : "",
            ["SumD2_2"] = ok ? "Mätmaskin Alt.Skjutmått" : "",
            ["SumAF2_2"] = "",
            ["SumF2_3"] = ok ? "1/2" : "",
            ["SumD2_3"] = ok ? "" : "",
            ["SumAF2_3"] = "",
            ["SumF2_4"] = ok ? "1/2" : "",
            ["SumD2_4"] = ok ? "Gängtolk" : "",
            ["SumAF2_4"] = "",
            ["SumF2_5"] = ok ? "1/2" : "",
            ["SumD2_5"] = ok ? "Mätmaskin Alt.Skjutmått/Mall" : "",
            ["SumAF2_5"] = "",
            ["SumF2_6"] = ok ? "Inst. / alt misstanke" : "",
            ["SumD2_6"] = ok ? "Ytjämnhetsmätare" : "",
            ["SumAF2_6"] = "Övriga bearbetade ytor 6.3",
            ["SumF2_7"] = ok ? "1/2" : "",
            ["SumD2_7"] = ok ? "Mätmaskin Alt.Skjutmått" : "",
            ["SumAF2_7"] = ""
        };

        private Dictionary<string, string> CalculateAdminFields(APIRequest request) => new()
        {
            ["SumTextS1"] = "Okulärkontroll Grader, frifläckar, slagmärken, repor, valkar, ytjämnhet och faser, skarpa kanter avgradas.",
            ["SumTextS2"] = "Okulärkontroll Grader, frifläckar, slagmärken, repor, valkar, ytjämnhet och faser, skarpa kanter avgradas.",
            ["SumRitNr"] = $"{request.ProductDesignation}: senaste utg. märkning: 7433523: senaste utg.",
            ["SumRitNr2"] = $"{request.ProductDesignation}: senaste utg. märkning: 7433523: senaste utg."
        };
    }
}