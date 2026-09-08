using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class TATNINGAR_SV_SPL_0006 : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var kv = new Dictionary<string, string>();

            kv["DocumentUniqueId"] =
                DateTime.UtcNow.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);

            string drawing = NormalizeProduct(request.ProductDesignation);
            string machine = NormalizeMachine(request.MachineNumber);

            DrawingType type = GetDrawingType(drawing);

            Merge(kv, Admin(drawing));
            Merge(kv, Diameters(type));
            Merge(kv, Widths(type));
            Merge(kv, Angles(type));
            Merge(kv, Machine(machine));
            Merge(kv, Page1(machine));
            Merge(kv, Page2(machine));
            Merge(kv, Derived(drawing, machine));

            return kv;
        }

        private void Merge(Dictionary<string, string> t, Dictionary<string, string> s)
        {
            foreach (var e in s)
                t[e.Key] = e.Value ?? "";
        }

        private string NormalizeProduct(string v) =>
            string.IsNullOrWhiteSpace(v) ? "SV-SPL-0006 3164" : v.Trim().ToUpperInvariant();

        private string NormalizeMachine(string v) =>
            string.IsNullOrWhiteSpace(v) ? "" : v.Trim();

        private bool IsMachine(string m, params string[] list)
        {
            foreach (var x in list)
                if (m.Equals(x, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        private Dictionary<string, string> Admin(string d) => new()
        {
            ["SumRit"] = d,
            ["SumRit2"] = d,
            ["SumTextS1"] = "Okulärkontroll Grader, frifläcker, slagmärken, repor, valkar, ytjämnhet och faser, skarpa kanter avgradas. OBS! ska doppas i olja",
            ["SumTextS2"] = "Okulärkontroll Grader, frifläcker, slagmärken, repor, valkar, ytjämnhet och faser, skarpa kanter avgradas. OBS! ska doppas i olja"
        };


        private Dictionary<string, string> Diameters(DrawingType type)
        {
            if (type == DrawingType.D3284)
            {
                return new Dictionary<string, string>
                {
                    ["SumD"] = "(D) 520",
                    ["SumDTol"] = "+ 0",
                    ["SumDTolN"] = "- 0.700",

                    ["SumD1"] = "(D1) 520",
                    ["SumD1Tol"] = "± 0.8",

                    ["SumD2"] = "(D2) 511",
                    ["SumD2Tol"] = "+ 0",
                    ["SumD2TolN"] = "- 0.110",

                    ["SumD3"] = "(D3) 497",
                    ["SumD3Tol"] = "± 0.8",

                    ["SumD4"] = "(D4) 465",
                    ["SumD4Tol"] = "+ 0.630",
                    ["SumD4TolN"] = "- 0",

                    ["SumD5"] = "(D5) 502",
                    ["SumD5Tol"] = "+ 0.700",
                    ["SumD5TolN"] = "- 0",

                    ["SumDR"] = "(DR) 15",
                    ["SumDOB"] = "(DOB) 12",
                    ["SumDOBText"] = ""
                };
            }

            if (type == DrawingType.D3280)
            {
                return new Dictionary<string, string>
                {
                    ["SumD"] = "(D) 526",
                    ["SumDTol"] = "+ 0",
                    ["SumDTolN"] = "- 0.700",

                    ["SumD1"] = "(D1) 526",
                    ["SumD1Tol"] = "± 0.8",

                    ["SumD2"] = "(D2) 516",
                    ["SumD2Tol"] = "+ 0",
                    ["SumD2TolN"] = "- 0.110",

                    ["SumD3"] = "(D3) 503",
                    ["SumD3Tol"] = "± 0.8",

                    ["SumD4"] = "(D4) 452,5",
                    ["SumD4Tol"] = "+ 0.630",
                    ["SumD4TolN"] = "- 0",

                    ["SumD5"] = "(D5) 505",
                    ["SumD5Tol"] = "+ 0.700",
                    ["SumD5TolN"] = "- 0",

                    ["SumDR"] = "(DR) 18",
                    ["SumDOB"] = "(DOB) 15",
                    ["SumDOBText"] = ""
                };
            }

            if (type == DrawingType.D3180)
            {
                return new Dictionary<string, string>
                {
                    ["SumD"] = "(D) 460",
                    ["SumDTol"] = "+ 0.800",
                    ["SumDTolN"] = "- 0.800",

                    ["SumD1"] = "(D1) 460",
                    ["SumD1Tol"] = "± 0.8",

                    ["SumD2"] = "(D2) 451",
                    ["SumD2Tol"] = "+ 0",
                    ["SumD2TolN"] = "- 0.097",

                    ["SumD3"] = "(D3) 437",
                    ["SumD3Tol"] = "± 0.8",

                    ["SumD4"] = "(D4) 415,5",
                    ["SumD4Tol"] = "+ 0.630",
                    ["SumD4TolN"] = "- 0",

                    ["SumD5"] = "(D5) 442",
                    ["SumD5Tol"] = "+ 0.800",
                    ["SumD5TolN"] = "- 0.800",

                    ["SumDR"] = "(DR) 14",
                    ["SumDOB"] = "(DOB) 10",
                    ["SumDOBText"] = ""
                };
            }

            if (type == DrawingType.D3064)
            {
                return new Dictionary<string, string>
                {
                    ["SumD"] = "(D) 385",
                    ["SumDTol"] = "+ 0",
                    ["SumDTolN"] = "- 0.570",

                    ["SumD1"] = "(D1) 385",
                    ["SumD1Tol"] = "± 0.5",

                    ["SumD2"] = "(D2) 376",
                    ["SumD2Tol"] = "+ 0",
                    ["SumD2TolN"] = "- 0.089",

                    ["SumD3"] = "(D3) 362",
                    ["SumD3Tol"] = "± 0.5",

                    ["SumD4"] = "(D4) 334",
                    ["SumD4Tol"] = "+ 0.570",
                    ["SumD4TolN"] = "- 0",

                    ["SumD5"] = "(D5) 367",
                    ["SumD5Tol"] = "+ 0.570",
                    ["SumD5TolN"] = "- 0",

                    ["SumDR"] = "(DR) 14",
                    ["SumDOB"] = "(DOB) 10",
                    ["SumDOBText"] = ""
                };
            }

            if (type == DrawingType.D3184 || type == DrawingType.D3276)
            {
                return new Dictionary<string, string>
                {
                    ["SumD"] = "(D) 500",
                    ["SumDTol"] = "+ 0",
                    ["SumDTolN"] = "- 0.630",

                    ["SumD1"] = "(D1) 500",
                    ["SumD1Tol"] = "± 0.8",

                    ["SumD2"] = "(D2) 491",
                    ["SumD2Tol"] = "+ 0",
                    ["SumD2TolN"] = "- 0.097",

                    ["SumD3"] = "(D3) 477",
                    ["SumD3Tol"] = "± 0.8",

                    ["SumD4"] = "(D4) 445",
                    ["SumD4Tol"] = "+ 0.630",
                    ["SumD4TolN"] = "- 0",

                    ["SumD5"] = "(D5) 482",
                    ["SumD5Tol"] = "+ 0.630",
                    ["SumD5TolN"] = "- 0",

                    ["SumDR"] = "(DR) 15",
                    ["SumDOB"] = "(DOB) 12",
                    ["SumDOBText"] = ""
                };
            }

            // DEFAULT = 3164
            return new Dictionary<string, string>
            {
                ["SumD"] = "(D) 400",
                ["SumDTol"] = "+ 0",
                ["SumDTolN"] = "- 0.570",

                ["SumD1"] = "(D1) 400",
                ["SumD1Tol"] = "± 0.8",

                ["SumD2"] = "(D2) 391",
                ["SumD2Tol"] = "+ 0",
                ["SumD2TolN"] = "- 0.089",

                ["SumD3"] = "(D3) 377",
                ["SumD3Tol"] = "± 0.5",

                ["SumD4"] = "(D4) 347",
                ["SumD4Tol"] = "+ 0.570",
                ["SumD4TolN"] = "- 0",

                ["SumD5"] = "(D5) 382",
                ["SumD5Tol"] = "+ 0.570",
                ["SumD5TolN"] = "- 0",

                ["SumDR"] = "(DR) 14",
                ["SumDOB"] = "(DOB) 10",
                ["SumDOBText"] = ""
            };
        }

        private Dictionary<string, string> Widths(DrawingType type)
        {
            var data = new Dictionary<string, string>
            {
                ["SumB"] = "(B) 26",
                ["SumBTol"] = "± 0.2",

                ["SumB1"] = "(B1) 10",
                ["SumB1Tol"] = "+ 0 [2]",
                ["SumB1TolN"] = "- 0.022 [3]",

                ["SumB3"] = "(B3) 11",
                ["SumB3Tol"] = "± 0.2",

                ["SumB4"] = "(B4) 9,8",
                ["SumB4Tol"] = "± 0.2",

                ["SumM"] = "(M) 9,2",
                ["SumMTol"] = "± 0.2",

                ["SumBM"] = "(BM) 5",
                ["SumBMTol"] = "+ 0.120",
                ["SumBMTolN"] = "- 0",

                ["SumBD"] = "(BD) 6",
                ["SumBDTol"] = "± 0.2",

                ["SumA"] = "(BD1) 5",
                ["SumATol"] = "± 0.1"
            };


            if (type == DrawingType.D3180)
            {
                data["SumHM"] = "Hjälpmått 8,8";
                data["SumKordaBM"] = "Korda kant till kant 323,6";
            }
            else if (type == DrawingType.D3184 || type == DrawingType.D3276)
            {
                data["SumHM"] = "Hjälpmått 8,5";
                data["SumKordaBM"] = "Korda kant till kant 351,4";
            }
            else if (type == DrawingType.D3280)
            {
                data["SumM"] = "(M) 6"; 
                data["SumHM"] = "Hjälpmått 6,4";
                data["SumKordaBM"] = "Korda kant till kant 370";
            }
            else if (type == DrawingType.D3284)
            {
                data["SumHM"] = "Hjälpmått 9,4";
                data["SumKordaBM"] = "Korda kant till kant 365,4";
            }
            else if (type == DrawingType.D3064)
            {
                data["SumHM"] = "Hjälpmått 5,6";
                data["SumKordaBM"] = "Korda kant till kant 269,8";
            }
            else
            {
               
                data["SumHM"] = "Hjälpmått 6,2";
                data["SumKordaBM"] = "Korda kant till kant 280,6";
            }

            return data;
        }


        private Dictionary<string, string> Angles(DrawingType type)
        {
            return new Dictionary<string, string>
            {
                ["SumV45"] = "45º",
                ["SumV45_2"] = "45º",
                ["SumF45"] = "(2x) 1x45º",
                ["SumV15"] =
                    type == DrawingType.D3180 ? "0.6º" :
                    type == DrawingType.D3280 ? "0.6º" :
                    (type == DrawingType.D3184 || type == DrawingType.D3276 || type == DrawingType.D3284) ? "0.7º" :
                    type == DrawingType.D3064 ? "1.0º" :
                    "0.9º"

            };
        }


        private Dictionary<string, string> Machine(string m)
        {
            string s1 = "";
            string s2 = "";

            if (IsMachine(m, "Nakamura"))
            {
                s1 = "Svarvning - Nakamura";
                s2 = "Borrning - Nakamura";
            }
            else if (IsMachine(m, "MaxMuller", "MaxMuller/Skepp6"))
            {
                s1 = "Svarvning - MaxMuller";
                s2 = "Borrning - Skepp 6";
            }
            else if (IsMachine(m, "LB45"))
            {
                s1 = "Svarvning - LB45";
                s2 = "Borrning - LB45";
            }

            return new Dictionary<string, string>
            {
                ["SumMaskinValS1"] = "Maskin: " + s1,
                ["SumMaskinValS2"] = "Maskin: " + s2
            };
        }

        private Dictionary<string, string> Page1(string m)
        {
            bool ok = IsMachine(m, "Nakamura", "MaxMuller", "MaxMuller/Skepp6", "LB45");

            return new Dictionary<string, string>
            {
                ["SumF1_1"] = ok ? "1/1" : "",
                ["SumF1_2"] = ok ? "1/1" : "",
                ["SumF1_3"] = ok ? "1/3" : "",
                ["SumF1_4"] = ok ? "1/3" : "",
                ["SumF1_5"] = ok ? "1/5" : "",
                ["SumF1_6"] = ok ? "Inst." : "",
                ["SumF1_7"] = ok ? "Inst." : "",
                ["SumF1_8"] = ok ? "1/3" : "",
                ["SumF1_9"] = ok ? "1/3" : "",

                ["SumD1_1"] = ok ? "Mikrometer" : "",
                ["SumD1_2"] = ok ? "Mikrometer" : "",
                ["SumD1_3"] = ok ? "Digitalt Djup/Hakmått" : "",
                ["SumD1_4"] = ok ? "Digitalt Djup/Hakmått" : "",
                ["SumD1_5"] = ok ? "Skjutmått" : "",
                ["SumD1_6"] = ok ? "Vinkelsystem" : "",
                ["SumD1_7"] = ok ? "Vinkelsystem" : "",
                ["SumD1_8"] = ok ? "Ytjämnhetsmätare" : "",
                ["SumD1_9"] = ok ? "Skjutmått" : "",

                ["SumAF1_1"] = "",
                ["SumAF1_2"] = "",
                ["SumAF1_3"] = "",
                ["SumAF1_4"] = "",
                ["SumAF1_5"] = "",
                ["SumAF1_6"] = "",
                ["SumAF1_7"] = "",
                ["SumAF1_8"] = ok ? "Bearbetas Ra 6.3 runt om" : "",
                ["SumAF1_9"] = ""
            };
        }

        private Dictionary<string, string> Page2(string m)
        {
            bool isMM = IsMachine(m, "MaxMuller", "MaxMuller/Skepp6");
            bool isOther = IsMachine(m, "Nakamura", "LB45");

            return new Dictionary<string, string>
            {
                ["SumF2_1"] = isMM ? "1/1" : isOther ? "1/5" : "",
                ["SumF2_2"] = isMM ? "1/1" : isOther ? "1/5" : "",
                ["SumF2_3"] = isMM ? "1/1" : isOther ? "1/5" : "",
                ["SumF2_4"] = isMM ? "1/1" : isOther ? "1/5" : "",
                ["SumF2_5"] = isMM ? "1/1" : isOther ? "Inst." : "",

                ["SumD2_1"] = "Skjutmått",
                ["SumD2_2"] = "Skjutmått",
                ["SumD2_3"] = "Skjutmått",
                ["SumD2_4"] = "Skjutmått",
                ["SumD2_5"] = "Skjutmått",

                ["SumAF2_1"] = "",
                ["SumAF2_2"] = "",
                ["SumAF2_3"] = "",
                ["SumAF2_4"] = "",
                ["SumAF2_5"] = ""
            };
        }

        private Dictionary<string, string> Derived(string drawing, string machine) => new()
        {
            ["TmpFormat"] = drawing.Replace(".", ","),
            ["MachineInput"] = machine
        };


        private DrawingType GetDrawingType(string drawing)
        {
            if (drawing.Contains("3284")) return DrawingType.D3284;
            if (drawing.Contains("3280")) return DrawingType.D3280;
            if (drawing.Contains("3276")) return DrawingType.D3276;
            if (drawing.Contains("3184")) return DrawingType.D3184;
            if (drawing.Contains("3180")) return DrawingType.D3180;
            if (drawing.Contains("3064")) return DrawingType.D3064;
            if (drawing.Contains("3164")) return DrawingType.D3164;

            return DrawingType.Unknown;
        }

        private enum DrawingType
        {
            Unknown,
            D3064,
            D3164,
            D3180,
            D3184,
            D3276,
            D3280,
            D3284
        }

    }
}