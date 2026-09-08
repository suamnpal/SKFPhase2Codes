using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class TATNINGAR_GR_TK_N_Narrow_42_SMALL : ITemplateCalculations
    {
        private string ProductDesignation = string.Empty;
        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var keyValues = new Dictionary<string, string>();

            ProductDesignation= request?.ProductDesignation ?? string.Empty;
            string product = (request.ProductDesignation ?? "")
                .Replace(".", ",")
                .ToUpperInvariant();

            string[] parts = product.Split(
                new[] { ' ', '/', '.' },
                StringSplitOptions.RemoveEmptyEntries);

            int typeCode = parts.Length > 1 && int.TryParse(parts[1], out int t)
                ? t
                : 0;

            bool isSupportedMachine = IsMachineMatch(request.MachineNumber);

            keyValues["SumMaskinValS1"] =
                $"Maskin: {request.MachineNumber} - Diametrala mått, Form & läge.";
            keyValues["SumMaskinValS2"] =
                $"Maskin: {request.MachineNumber} - Övriga mått.";
            keyValues["SumMaskinvalS1"] = keyValues["SumMaskinValS1"];
            keyValues["SumMaskinvalS2"] = keyValues["SumMaskinValS2"];

            Merge(keyValues, CalculatePage1Measurements(isSupportedMachine));
            Merge(keyValues, CalculatePage2Measurements(isSupportedMachine));

            Merge(keyValues, CalculateDiametersNarrow(typeCode));

            Merge(keyValues, CalculateWidths(typeCode));

            Merge(keyValues, CalculateThreads());

            Merge(keyValues, CalculateGeometry());

            Merge(keyValues, CalculateAdminFields());

            return keyValues;
        }

        private static void Merge(
             Dictionary<string, string> target,
             Dictionary<string, string> source)
        {
            foreach (var entry in source)
            {
                if (!target.ContainsKey(entry.Key) || string.IsNullOrEmpty(target[entry.Key]))
                {
                    target[entry.Key] = entry.Value;
                }
            }
        }

        private static bool IsMachineMatch(string m)
        {
            if (string.IsNullOrWhiteSpace(m)) return false;
            m = m.ToUpperInvariant();
            return m.Contains("NAKAMURA") || m.Contains("LB45") || m.Contains("LT-3000EX");
        }

        private Dictionary<string, string> CalculateDiametersNarrow(int typeCode)
        {
            var kv = new Dictionary<string, string>();

            // -------------------------------------------------
            // ALWAYS initialize ALL diameter placeholders
            // -------------------------------------------------
            for (int i = 1; i <= 15; i++)
            {
                kv[$"Sumd{i}"] = "";
                kv[$"Sumd{i}Tol"] = "";
                kv[$"Sumd{i}TolN"] = "";
            }

            if (typeCode == 34)
            {
                kv["Sumd1"] = "2x (d1) 184.2"; kv["Sumd1Tol"] = "+ 0.0"; kv["Sumd1TolN"] = "- 0.300";

                kv["Sumd2"] = "(d2) 182.2"; kv["Sumd2Tol"] = "± 0.100";

                kv["Sumd3"] = "2x (d3) 170.2"; kv["Sumd3Tol"] = "+ 0.0"; kv["Sumd3TolN"] = "- 0.400";

                kv["Sumd4"] = "(d4) 154"; kv["Sumd4Tol"] = "+ 0.400"; kv["Sumd4TolN"] = "- 0.0";

                kv["Sumd6"] = "(d6) 182"; kv["Sumd6Tol"] = "+ 0.460"; kv["Sumd6TolN"] = "- 0.0";

                kv["Sumd7"] = "(d7) 190"; kv["Sumd7Tol"] = "+ 0.0"; kv["Sumd7TolN"] = "- 0.460";

                kv["Sumd8"] = "(d8) 206"; kv["Sumd8Tol"] = "+ 0.460"; kv["Sumd8TolN"] = "- 0.0";

                kv["Sumd9"] = "(d9) 214"; kv["Sumd9Tol"] = "+ 0.0"; kv["Sumd9TolN"] = "- 0.460";

                kv["Sumd10"] = "(d10) 230"; kv["Sumd10Tol"] = "+ 0.460"; kv["Sumd10TolN"] = "- 0.0";

                kv["Sumd11"] = "(d11) 238"; kv["Sumd11Tol"] = "+ 0.0"; kv["Sumd11TolN"] = "- 0.460";

                kv["Sumd12"] = "(d12) 254"; kv["Sumd12Tol"] = "+ 0.520"; kv["Sumd12TolN"] = "- 0.0";

                kv["Sumd13"] = "(d13) 262"; kv["Sumd13Tol"] = "+ 0.0"; kv["Sumd13TolN"] = "- 0.520";

                kv["Sumd14"] = "(d14) 8"; kv["Sumd14Tol"] = "+ 0.100"; kv["Sumd14TolN"] = "- 0.0";

                kv["Sumd15"] = "(d15) 3"; kv["Sumd15Tol"] = "± 0.100";

            }

            // ---------------- GR‑TK 36 N ----------------
            if (typeCode == 36)
            {
                kv["Sumd1"] = "2x (d1) 194.2"; kv["Sumd1Tol"] = "+ 0.0"; kv["Sumd1TolN"] = "- 0.300";
                kv["Sumd2"] = "(d2) 192.2"; kv["Sumd2Tol"] = "± 0.100";
                kv["Sumd3"] = "2x (d3) 180.2"; kv["Sumd3Tol"] = "+ 0.0"; kv["Sumd3TolN"] = "- 0.400";
                kv["Sumd4"] = "(d4) 164"; kv["Sumd4Tol"] = "+ 0.400"; kv["Sumd4TolN"] = "- 0.0";

                kv["Sumd6"] = "(d6) 192"; kv["Sumd6Tol"] = "+ 0.460"; kv["Sumd6TolN"] = "- 0.0";
                kv["Sumd7"] = "(d7) 200"; kv["Sumd7Tol"] = "+ 0.0"; kv["Sumd7TolN"] = "- 0.460";
                kv["Sumd8"] = "(d8) 216"; kv["Sumd8Tol"] = "+ 0.460"; kv["Sumd8TolN"] = "- 0.0";
                kv["Sumd9"] = "(d9) 224"; kv["Sumd9Tol"] = "+ 0.0"; kv["Sumd9TolN"] = "- 0.460";
                kv["Sumd10"] = "(d10) 240"; kv["Sumd10Tol"] = "+ 0.460"; kv["Sumd10TolN"] = "- 0.0";
                kv["Sumd11"] = "(d11) 248"; kv["Sumd11Tol"] = "+ 0.0"; kv["Sumd11TolN"] = "- 0.460";
                kv["Sumd12"] = "(d12) 264"; kv["Sumd12Tol"] = "+ 0.520"; kv["Sumd12TolN"] = "- 0.0";
                kv["Sumd13"] = "(d13) 272"; kv["Sumd13Tol"] = "+ 0.0"; kv["Sumd13TolN"] = "- 0.520";
            }

            // ---------------- GR‑TK 38 N ----------------
            if (typeCode == 38)
            {
                kv["Sumd1"] = "2x (d1) 204.4"; kv["Sumd1Tol"] = "+ 0.0"; kv["Sumd1TolN"] = "- 0.300";
                kv["Sumd2"] = "(d2) 202.4"; kv["Sumd2Tol"] = "± 0.100";
                kv["Sumd3"] = "2x (d3) 190.4"; kv["Sumd3Tol"] = "+ 0.0"; kv["Sumd3TolN"] = "- 0.400";
                kv["Sumd4"] = "(d4) 174"; kv["Sumd4Tol"] = "+ 0.400"; kv["Sumd4TolN"] = "- 0.0";

                kv["Sumd6"] = "(d6) 202"; kv["Sumd6Tol"] = "+ 0.460"; kv["Sumd6TolN"] = "- 0.0";
                kv["Sumd7"] = "(d7) 210"; kv["Sumd7Tol"] = "+ 0.0"; kv["Sumd7TolN"] = "- 0.460";
                kv["Sumd8"] = "(d8) 226"; kv["Sumd8Tol"] = "+ 0.460"; kv["Sumd8TolN"] = "- 0.0";
                kv["Sumd9"] = "(d9) 234"; kv["Sumd9Tol"] = "+ 0.0"; kv["Sumd9TolN"] = "- 0.460";
                kv["Sumd10"] = "(d10) 250"; kv["Sumd10Tol"] = "+ 0.460"; kv["Sumd10TolN"] = "- 0.0";
                kv["Sumd11"] = "(d11) 258"; kv["Sumd11Tol"] = "+ 0.0"; kv["Sumd11TolN"] = "- 0.520";
                kv["Sumd12"] = "(d12) 274"; kv["Sumd12Tol"] = "+ 0.520"; kv["Sumd12TolN"] = "- 0.0";
                kv["Sumd13"] = "(d13) 282"; kv["Sumd13Tol"] = "+ 0.0"; kv["Sumd13TolN"] = "- 0.520";
            }

            // ---------------- GR‑TK 40 N ----------------
            if (typeCode == 40)
            {
                kv["Sumd1"] = "2x (d1) 214.2"; kv["Sumd1Tol"] = "+ 0.0"; kv["Sumd1TolN"] = "- 0.300";
                kv["Sumd2"] = "(d2) 212.2"; kv["Sumd2Tol"] = "± 0.100";
                kv["Sumd3"] = "2x (d3) 200.2"; kv["Sumd3Tol"] = "+ 0.0"; kv["Sumd3TolN"] = "- 0.400";
                kv["Sumd4"] = "(d4) 184"; kv["Sumd4Tol"] = "+ 0.400"; kv["Sumd4TolN"] = "- 0.0";

                kv["Sumd6"] = "(d6) 212"; kv["Sumd6Tol"] = "+ 0.460"; kv["Sumd6TolN"] = "- 0.0";
                kv["Sumd7"] = "(d7) 220"; kv["Sumd7Tol"] = "+ 0.0"; kv["Sumd7TolN"] = "- 0.460";
                kv["Sumd8"] = "(d8) 236"; kv["Sumd8Tol"] = "+ 0.460"; kv["Sumd8TolN"] = "- 0.0";
                kv["Sumd9"] = "(d9) 244"; kv["Sumd9Tol"] = "+ 0.0"; kv["Sumd9TolN"] = "- 0.460";
                kv["Sumd10"] = "(d10) 260"; kv["Sumd10Tol"] = "+ 0.520"; kv["Sumd10TolN"] = "- 0.0";
                kv["Sumd11"] = "(d11) 268"; kv["Sumd11Tol"] = "+ 0.0"; kv["Sumd11TolN"] = "- 0.520";
                kv["Sumd12"] = "(d12) 284"; kv["Sumd12Tol"] = "+ 0.520"; kv["Sumd12TolN"] = "- 0.0";
                kv["Sumd13"] = "(d13) 292"; kv["Sumd13Tol"] = "+ 0.0"; kv["Sumd13TolN"] = "- 0.520";
            }

            kv["Sumd14"] = "(d14) 8"; kv["Sumd14Tol"] = "+ 0.100"; kv["Sumd14TolN"] = "- 0.0";
            kv["Sumd15"] = "(d15) 3"; kv["Sumd15Tol"] = "± 0.100";

            return kv;
        }


        private Dictionary<string, string> CalculateWidths(int typeCode)
        {
            var kv = new Dictionary<string, string>();

            kv["SumB"] = "";
            kv["SumBTol"] = "+ 0.500";
            kv["SumBTolN"] = "- 0.0";

            for (int i = 1; i <= 9; i++)
            {
                kv[$"Sumb{i}"] = "";
                kv[$"Sumb{i}Tol"] = "";
                kv[$"Sumb{i}TolN"] = "";
            }

            kv["SumS1"] = "";
            kv["SumS1Tol"] = "+ 0.500";
            kv["SumS1TolN"] = "+ 0.250";


            kv["SumB"] = "(B) 46";

            kv["Sumb1"] = "(b1) 22.5";
            kv["Sumb1Tol"] = "+ 0.500";
            kv["Sumb1TolN"] = "- 0.0";

            kv["Sumb2"] = "(b2) 10";
            kv["Sumb2Tol"] = "+ 0.0";
            kv["Sumb2TolN"] = "- 0.500";

            kv["Sumb3"] = "(b3) 16";
            kv["Sumb3Tol"] = "+ 0.0";
            kv["Sumb3TolN"] = "- 0.500";

            kv["Sumb4"] = "(b4) 6";
            kv["Sumb4Tol"] = "+ 0.0";
            kv["Sumb4TolN"] = "- 0.500";

            kv["Sumb5"] = "2x (b5) 10.5";
            kv["Sumb5Tol"] = "+ 0.0";
            kv["Sumb5TolN"] = "- 0.500";

            kv["Sumb6"] = "(b6) 7.5";
            kv["Sumb6Tol"] = "± 0.200";

            kv["Sumb7"] = "(b7) 6";
            kv["Sumb7Tol"] = "+ 0.400";
            kv["Sumb7TolN"] = "- 0.0";

            kv["Sumb9"] = "(b9) 16";
            kv["Sumb9Tol"] = "+ 0.0";
            kv["Sumb9TolN"] = "- 0.200";

            kv["SumS1"] = "(S1) 1.0";


            return kv;
        }

        private Dictionary<string, string> CalculateThreads()
        {
            return new Dictionary<string, string>
            {
                ["SumG"] = "(G) M6",
                ["SumGa"] = "(G) M6",
                ["SumG1"] = "(G1) 15",
                ["SumG1Tol"] = "+ 0.500",
                ["SumG1TolN"] = "- 0.0",
                ["SumG2"] = "min.23",
                ["SumG3"] = "max.28",
                ["SumG4"] = "(G4) 41.5",
                ["SumG4Tol"] = "+ 0.500",
                ["SumG4TolN"] = "- 0.0"
            };
        }

        private Dictionary<string, string> CalculateGeometry()
        {
            return new Dictionary<string, string>
            {
                ["SumR08"] = "R max: 0.8 (6x)",
                ["SumR05a"] = "max R0.5",
                ["SumRa32"] = "3.2",
                ["SumRd"] = "0.100",
                ["SumF1"] = "1x45°",
                ["SumF2"] = "1.5x45°",
                ["SumF3"] = "1x45°",
                ["SumCo1"] = "0.150"
            };
        }

        private Dictionary<string, string> CalculatePage1Measurements(bool ok)
        {
            var kv = new Dictionary<string, string>();

            kv["SumF1_1"] = ok ? "1/5" : "";
            kv["SumD1_1"] = ok ? "Mätmaskin Alt.Skjutmått" : "";
            kv["SumAF1_1"] = "";

            kv["SumF1_2"] = ok ? "1/5" : "";
            kv["SumD1_2"] = ok ? "Mätmaskin Alt.UD-Apparat" : "";
            kv["SumAF1_2"] = "";

            kv["SumF1_3"] = ok ? "1/5" : "";
            kv["SumD1_3"] = ok ? "Mätmaskin Alt.Skjutmått" : "";
            kv["SumAF1_3"] = "";

            kv["SumF1_4"] = ok ? "Inst. / alt misstanke" : "";
            kv["SumD1_4"] = ok ? "Ytjämnhetsmätare" : "";
            kv["SumAF1_4"] = ok ? "Övriga bearbetade ytor 6.3" : "";

            kv["SumF1_5"] = ok ? "Inst." : "";
            kv["SumD1_5"] = ok ? "Mätmaskin Alt.Mätservice" : "";
            kv["SumAF1_5"] = ok ? "Vid misstänkt formfel, lämna till mätrum" : "";

            kv["SumF1_6"] = ok ? "Inst." : "";
            kv["SumD1_6"] = ok ? "Mätmaskin Alt.Mätservice" : "";
            kv["SumAF1_6"] = kv["SumAF1_5"];

            kv["SumF1_7"] = ok ? "Inst." : "";
            kv["SumD1_7"] = ok ? "Mätmaskin Alt.Skjutmått" : "";
            kv["SumAF1_7"] = "";

            return kv;
        }

        private Dictionary<string, string> CalculatePage2Measurements(bool ok)
        {
            var kv = new Dictionary<string, string>();

            kv["SumF2_1"] = ok ? "1/5" : "";
            kv["SumD2_1"] = ok ? "Mätmaskin Alt.Skjutmått" : "";
            kv["SumAF2_1"] = "";

            kv["SumF2_2"] = ok ? "1/5" : "";
            kv["SumD2_2"] = ok ? "Mätmaskin Alt.Skjutmått" : "";
            kv["SumAF2_2"] = "";

            kv["SumF2_3"] = ok ? "1/5" : "";
            kv["SumD2_3"] = "";
            kv["SumAF2_3"] = "";

            kv["SumF2_4"] = ok ? "1/5" : "";
            kv["SumD2_4"] = ok ? "Gängtolk" : "";
            kv["SumAF2_4"] = "";

            kv["SumF2_5"] = ok ? "1/5" : "";
            kv["SumD2_5"] = ok ? "Mätmaskin Alt.Skjutmått/Mall" : "";
            kv["SumAF2_5"] = "";

            kv["SumF2_6"] = ok ? "Inst. / alt misstanke" : "";
            kv["SumD2_6"] = ok ? "Ytjämnhetsmätare" : "";
            kv["SumAF2_6"] = ok ? "Övriga bearbetade ytor 6.3" : "";

            kv["SumF2_7"] = ok ? "1/5" : "";
            kv["SumD2_7"] = ok ? "Mätmaskin Alt.Skjutmått" : "";
            kv["SumAF2_7"] = "";

            return kv;
        }

        private Dictionary<string, string> CalculateAdminFields()
        {
            return new Dictionary<string, string>
            {
                ["SumTextS1"] =
                    "Okulärkontroll Grader, frifläckar, slagmärken, repor, valkar, ytjämnhet och faser, skarpa kanter avgradas.",
                ["SumTextS2"] =
                    "Okulärkontroll Grader, frifläckar, slagmärken, repor, valkar, ytjämnhet och faser, skarpa kanter avgradas.",
                ["SumRitNr"] = $"{ProductDesignation}: senaste utg. märkning: 7433523: senaste utg.",
                ["SumRitNr2"] = $"{ProductDesignation}: senaste utg. märkning: 7433523: senaste utg."
            };
        }
    }
}