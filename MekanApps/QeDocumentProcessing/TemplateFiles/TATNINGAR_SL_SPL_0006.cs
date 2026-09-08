using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class TATNINGAR_SL_SPL_0006 : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var kv = new Dictionary<string, string>();

            kv["DocumentUniqueId"] =
                DateTime.UtcNow.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);

            string drawing =
                (request.ProductDesignation ?? "SL-SPL-0006-3180").ToUpperInvariant();

            string machineVal = (request.MachineNumber ?? "").Trim();
            bool isMachine = machineVal.Equals("NAKAMURA", StringComparison.OrdinalIgnoreCase);

            Merge(kv, GetAdminTexts(drawing));
            Merge(kv, GetDiameters(drawing));

            Merge(kv, GetCommonWidths(drawing));
            Merge(kv, GetTailWidths(drawing));

            Merge(kv, GetRadiiChamfers());
            Merge(kv, GetSurface());
            Merge(kv, GetMachine(machineVal));
            Merge(kv, GetPageMeasurements(isMachine));

            return kv;
        }

        private void Merge(Dictionary<string, string> t, Dictionary<string, string> s)
        {
            foreach (var e in s)
                t[e.Key] = e.Value ?? "";
        }

        // ==================== DIAMETERS ====================
        private Dictionary<string, string> GetDiameters(string drawing) => drawing switch
        {
            "SL-SPL-0006-3064" => new()
            {
                ["SumD"] = "(D) 378,5",
                ["SumDTol"] = "± 0.8",
                ["SumD1"] = "(D1) 362,5",

                ["SumD2"] = "(D2) 334",
                ["SumD2Tol"] = "+ 0",
                ["SumD2TolN"] = "- 0.570",

                ["SumD3"] = "(D3) 322",
                ["SumD3Tol"] = "+ 0",
                ["SumD3TolN"] = "- 0.230",

                ["SumD4"] = "(D4) 317",
                ["SumD4Tol"] = "± 0.8",

                ["SumD5"] = "(D5) 0",
                ["SumD5Tol"] = "± 0.1",

                ["SumD6"] = "(D6) 304,8",
                ["SumD6Tol"] = "+ 0.210",
                ["SumD6TolN"] = "- 0",

                ["SumD7"] = "(D7) 300",
                ["SumD7Tol"] = "+ 0.320",
                ["SumD7TolN"] = "+ 0.110"
            },

            "SL-SPL-0006-3164" => new()
            {
                ["SumD"] = "(D) 393,5",
                ["SumDTol"] = "± 0.8",
                ["SumD1"] = "(D1) 377,5",

                ["SumD2"] = "(D2) 347",
                ["SumD2Tol"] = "+ 0",
                ["SumD2TolN"] = "- 0.570",

                ["SumD3"] = "(D3) 335",
                ["SumD3Tol"] = "+ 0",
                ["SumD3TolN"] = "- 0.230",

                ["SumD4"] = "(D4) 330",
                ["SumD4Tol"] = "± 0.8",

                ["SumD5"] = "(D5) 325",
                ["SumD5Tol"] = "± 0.8",

                ["SumD6"] = "(D6) 304,8",
                ["SumD6Tol"] = "+ 0.210",
                ["SumD6TolN"] = "- 0",

                ["SumD7"] = "(D7) 300",
                ["SumD7Tol"] = "+ 0.320",
                ["SumD7TolN"] = "+ 0.110"
            },


            "SL-SPL-0006-3180" => new()
            {
                ["SumD"] = "(D) 454",
                ["SumDTol"] = "± 0.8",
                ["SumD1"] = "(D1) 437,5",
                ["SumD2"] = "(D2) 415",
                ["SumD2Tol"] = "+ 0",
                ["SumD2TolN"] = "- 0.630",
                ["SumD3"] = "(D3) 400",
                ["SumD3Tol"] = "+ 0",
                ["SumD3TolN"] = "- 0.230",
                ["SumD4"] = "(D4) 395",
                ["SumD4Tol"] = "± 0.8",
                ["SumD5"] = "(D5) 405",
                ["SumD5Tol"] = "± 0.8",
                ["SumD6"] = "(D6) 384,8",
                ["SumD6Tol"] = "+ 0.230",
                ["SumD6TolN"] = "- 0",
                ["SumD7"] = "(D7) 380",
                ["SumD7Tol"] = "+ 0.355",
                ["SumD7TolN"] = "+ 0.125"
            },

            "SL-SPL-0006-3184" => new()
            {
                ["SumD"] = "(D) 494",
                ["SumDTol"] = "± 0.8",
                ["SumD1"] = "(D1) 477,5",
                ["SumD2"] = "(D2) 445",
                ["SumD2Tol"] = "+ 0",
                ["SumD2TolN"] = "- 0.630",
                ["SumD3"] = "(D3) 430",
                ["SumD3Tol"] = "+ 0",
                ["SumD3TolN"] = "- 0.250",
                ["SumD4"] = "(D4) 425",
                ["SumD4Tol"] = "± 0.8",
                ["SumD5"] = "(D5) 425",
                ["SumD5Tol"] = "± 0.8",
                ["SumD6"] = "(D6) 404,8",
                ["SumD6Tol"] = "+ 0.250",
                ["SumD6TolN"] = "- 0",
                ["SumD7"] = "(D7) 400",
                ["SumD7Tol"] = "+ 0.355",
                ["SumD7TolN"] = "+ 0.125"
            },

            "SL-SPL-0006-3276" => new()
            {
                ["SumD"] = "(D) 493,5",
                ["SumDTol"] = "± 0.8",
                ["SumD1"] = "(D1) 477,5",
                ["SumD2"] = "(D2) 445",
                ["SumD2Tol"] = "+ 0",
                ["SumD2TolN"] = "- 0.630",
                ["SumD3"] = "(D3) 430",
                ["SumD3Tol"] = "+ 0",
                ["SumD3TolN"] = "- 0.250",
                ["SumD4"] = "(D4) 425",
                ["SumD4Tol"] = "± 0.8",
                ["SumD5"] = "(D5) 425",
                ["SumD5Tol"] = "± 0.8",
                ["SumD6"] = "(D6) 364,8",
                ["SumD6Tol"] = "+ 0.230",
                ["SumD6TolN"] = "- 0",
                ["SumD7"] = "(D7) 360",
                ["SumD7Tol"] = "+ 0.355",
                ["SumD7TolN"] = "+ 0.125"
            },

            "SL-SPL-0006-3280" => new()
            {
                ["SumD"] = "(D) 520",
                ["SumDTol"] = "± 0.8",
                ["SumD1"] = "(D1) 504",
                ["SumD2"] = "(D2) 452",
                ["SumD2Tol"] = "+ 0",
                ["SumD2TolN"] = "- 0.630",
                ["SumD3"] = "(D3) 440",
                ["SumD3Tol"] = "+ 0",
                ["SumD3TolN"] = "- 0.250",
                ["SumD4"] = "(D4) 435",
                ["SumD4Tol"] = "± 0.8",
                ["SumD5"] = "(D5) 430",
                ["SumD5Tol"] = "± 0.8",
                ["SumD6"] = "(D6) 384,8",
                ["SumD6Tol"] = "+ 0.230",
                ["SumD6TolN"] = "- 0",
                ["SumD7"] = "(D7) 380",
                ["SumD7Tol"] = "+ 0.355",
                ["SumD7TolN"] = "+ 0.125"
            },

            _ => new()
            {
                ["SumD"] = "(D) 514",
                ["SumDTol"] = "± 0.8",
                ["SumD1"] = "(D1) 497,5",
                ["SumD2"] = "(D2) 465",
                ["SumD2Tol"] = "+ 0",
                ["SumD2TolN"] = "- 0.630",
                ["SumD3"] = "(D3) 450",
                ["SumD3Tol"] = "+ 0",
                ["SumD3TolN"] = "- 0.250",
                ["SumD4"] = "(D4) 445",
                ["SumD4Tol"] = "± 0.8",
                ["SumD5"] = "(D5) 488",
                ["SumD5Tol"] = "± 0.8",
                ["SumD6"] = "(D6) 404,8",
                ["SumD6Tol"] = "+ 0.250",
                ["SumD6TolN"] = "- 0",
                ["SumD7"] = "(D7) 400",
                ["SumD7Tol"] = "+ 0.355",
                ["SumD7TolN"] = "+ 0.125"
            }
        };

        // ==================== COMMON VALUES ====================
        private Dictionary<string, string> GetCommonWidths(string drawing) => new()
        {
            ["SumB"] = "(B) 42",
            ["SumBTol"] = "± 0.3",

            ["SumB1"] = "(B1) 4",
            ["SumB1Tol"] = "+ 0.2",
            ["SumB1TolN"] = "0",

            ["SumB2"] = drawing == "SL-SPL-0006-3064" ? "(B2) 0" : "(B2) 2",
            ["SumB2Tol"] = "± 0.1",

            ["SumB3"] = "(B3) 7",
            ["SumB3Tol"] = "± 0.2",

            ["SumB4"] = "(B4) 29",
            ["SumB4Tol"] = "± 0.2",

            ["SumB5"] = "(B5) 33,7",
            ["SumB5Tol"] = "± 0.3",

            ["SumB6"] = "(B6) 4,5",
            ["SumB6Tol"] = "± 0.1"
        };
        private Dictionary<string, string> GetTailWidths(string drawing)
        {
            return drawing switch
            {
                "SL-SPL-0006-3064" => new()
                {
                    ["SumB7"] = "(B7) 16,5",
                    ["SumB7Tol"] = "± 0.2",
                    ["SumB8"] = "(B8) 19,4",
                    ["SumB8Tol"] = "± 0.2",
                    ["SumB9"] = "(B9) 22,3",
                    ["SumB9Tol"] = "± 0.2"                   
                },
                _ => new()
                {
                    ["SumB7"] = "(B7) 22,3",
                    ["SumB7Tol"] = "± 0.2",
                    ["SumB8"] = "(B8) 19,4",
                    ["SumB8Tol"] = "± 0.2",
                    ["SumB9"] = "(B9) 16,5",
                    ["SumB9Tol"] = "± 0.2",
                },
            };
        }

        private Dictionary<string, string> GetRadiiChamfers() => new()
        {
            ["SumG"] = "1x45º",
            ["SumG1"] = "1x45º",
            ["SumG2"] = "1x45º",
            ["SumG3"] = "45º",
            ["SumG4"] = "60º",
            ["SumR"] = "R 0.2",
            ["SumR1"] = "R 4",
           
            ["SumR3"] = "R 0,2"
        };

        private Dictionary<string, string> GetSurface() => new() { ["SumRa"] = "3.2" };

        private Dictionary<string, string> GetMachine(string machine)
        {
            if (string.IsNullOrWhiteSpace(machine))
                return new Dictionary<string, string>
                {
                    ["SumMaskinValS1"] = ""
                };

            string m = machine.Trim().ToUpperInvariant();

            string display = m switch
            {
                "NAKAMURA" => "Nakamura",
                "MAXMULLER" => "MaxMuller",
                "LB45" => "LB45",
                _ => machine   // fallback: show raw value
            };

            return new Dictionary<string, string>
            {
                ["SumMaskinValS1"] = "Maskin: Svarvning - " + display
            };
        }

        private Dictionary<string, string> GetAdminTexts(string drawing) => new()
        {
            ["SumR2"] = drawing == "SL-SPL-0006-3064" ? "R 0" :"R 1,6",
            ["SumTextS1"] =
                "Okulärkontroll Grader, frifläcker, slagmärken, repor, valkar, ytjämnhet och faser, skarpa kanter avgradas. OBS! Ska doppas i olja",
            ["SumRit"] = drawing,
            ["SumStämpel"] = drawing
        };

        private Dictionary<string, string> GetPageMeasurements(bool ok) => new()
        {
            ["SumF1_1"] ="1/1",
            ["SumF1_2"] ="Inst.",
            ["SumF1_3"] ="Inst.",
            ["SumF1_4"] ="1/3",
            ["SumF1_5"] ="1/5",
            ["SumD1_1"] ="UD-Apparat/mikrometer",
            ["SumD1_2"] ="Djupmått",
            ["SumD1_3"] ="Radielyra/mallar",
            ["SumD1_4"] ="Ytjämnhetsmätare",
            ["SumD1_5"] ="Skjutmått",
            ["SumAF1_1"] = "Inställd med tillhörande klove/ring",
            ["SumAF1_2"] = "",
            ["SumAF1_3"] = "",
            ["SumAF1_4"] = "Ytjämnhet övriga ytor 6.3" ,
            ["SumAF1_5"] = ""
        };
    }
}