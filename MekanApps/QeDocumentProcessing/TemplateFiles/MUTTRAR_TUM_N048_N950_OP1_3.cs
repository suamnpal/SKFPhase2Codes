using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class MUTTRAR_TUM_N048_N950_OP1_3 : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var result = new Dictionary<string, string>();

            string product = NormalizeProduct(request.ProductDesignation);
            string machine = NormalizeMachine(request.MachineNumber);

            Merge(result, GetAdminTexts());
            Merge(result, GetDiameters(product));
            Merge(result, GetWidthsAndDepths(product));
            Merge(result, GetThreads(product));
            Merge(result, GetChamfersAndRadii());
            Merge(result, GetSurface());
            Merge(result, GetKordaAndAngles());
            Merge(result, GetMachineTexts(machine));
            Merge(result, GetPage1Measurements(machine));
            Merge(result, GetPage2Measurements(machine));

            return result;
        }

        private void Merge(Dictionary<string, string> target, Dictionary<string, string> source)
        {
            foreach (var kv in source)
                target[kv.Key] = kv.Value ?? "";
        }

        private string NormalizeProduct(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? "N 500" : value.Trim().ToUpperInvariant();
        }

        private string NormalizeMachine(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? "" : value.Trim();
        }

        private Dictionary<string, string> GetAdminTexts() => new()
        {
            ["SumRitningsnr"] = "11K 7431012",
            ["SumRitningsnr2"] = "11K 7431012",
            ["SumKlEgenskaper"] = "PRODUCTION NUTS & SLEEVES & HOUSINGSALLMÄNKLASSADE EGENSKAPERKlassade egenskaper muttrar",
            ["SumKlEgenskaper2"] = "PRODUCTION NUTS & SLEEVES & HOUSINGSALLMÄNKLASSADE EGENSKAPERKlassade egenskaper muttrar",
            ["SumTextS1"] = "Grader, frifläckar, slagmärken, repor, valkar och andra defekter samt ojämnheter kontrolleras med ögonmått, för övriga mått används skjutmått.",
            ["SumTextS2"] = "Övriga mått kontrolleras vid inställning",
            ["SumStämpling"] = "Stämplas enl. ritning 7430189"
        };

        private Dictionary<string, string> GetDiameters(string product) => new()
        {
            ["SumD"] = "(D) 579.58",
            ["SumDTol"] = "+ 0.0",
            ["SumDTolN"] = "- 0.510",

            ["Sumd1"] = "(d1) 494.335",
            ["Sumd1Tol"] = "+ 0.406",
            ["Sumd1TolN"] = "- 0.0",

            ["Sumd2"] = "(d2) 496.875",
            ["Sumd2Tol"] = "+ 0.406",
            ["Sumd2TolN"] = "- 0.0",

            ["Sumd3"] = "(d3) 500.33",
            ["Sumd3a"] = "(d3) 500.33",
            ["Sumd3Tol"] = "+ 0.787",
            ["Sumd3TolN"] = "- 0.0",
            ["Sumd3Tola"] = "+ 0.787",
            ["Sumd3TolNa"] = "- 0.0",

            ["Sumd4"] = "(d4) 548.89",
            ["Sumd4Tol"] = "+ 0.760",
            ["Sumd4TolN"] = "- 0.0",

            ["Sumd5"] = "(d5) 550.88",
            ["Sumd5Tol"] = "+ 0.0",
            ["Sumd5TolN"] = "- 1.270",

            ["SumP"] = "(P) 5,08"
        };

        private Dictionary<string, string> GetWidthsAndDepths(string product) => new()
        {
            ["SumB"] = "(B) 68.66",
            ["SumBTol"] = "+ 0.0",
            ["SumBTolN"] = "- 1.020",

            ["SumS"] = "(S) 37.59",
            ["SumSTol"] = "+ 0.760",
            ["SumSTolN"] = "- 0.0",

            ["Sumt"] = "(t) 14.33",
            ["SumtTol"] = "+ 1.020",
            ["SumtTolN"] = "- 0.000",

            ["Sume"] = "(e) 9.520",
            ["SumeTol"] = "+ 1.520",
            ["SumeTolN"] = "- 0.0",

            ["SumL"] = "(L) max 44.45",

            ["Sumv"] = "(v) min31.75",
            ["SumV"] = "(v) min31.75",

            ["SumHm"] = "15,35"
        };

        private Dictionary<string, string> GetThreads(string product) => new()
        {
            ["SumG"] = "Acme 19.682x5",
            ["Sumu"] = "UNC 5/8-11",
            ["SumuTolk"] = "UNC 5/8-11 min/max"
        };

        private Dictionary<string, string> GetChamfersAndRadii() => new()
        {
            ["SumKa"] = "0.250",
            ["SumFas"] = "30º",
            ["SumFas2"] = "45º",
            ["SumFas2a"] = "45º",
            ["SumR3"] = "R 3",
            ["SumRHT"] = "R1.6 (4x)",
            ["SumGV"] = "45º",
            ["SumGv"] = "45º",
            ["Sum15"] = "15º",
            ["Sum30"] = "30º"
        };

        private Dictionary<string, string> GetKordaAndAngles() => new()
        {
            ["SumKorda30"] = "142,1",
            ["SumKorda15"] = "71,7"
        };

        private Dictionary<string, string> GetSurface() => new()
        {
            ["SumRa"] = "3.2",
            ["SumRa1"] = "3.2",
            ["SumRa2"] = "3.2",
            ["SumRa3"] = "3.2"
        };

        private Dictionary<string, string> GetMachineTexts(string machine) => new()
        {
            ["SumMaskinValS1"] = machine == "LB45" ? "Maskin: LB45 - OP1 & 2" : machine == "MaxMuller/K&T" ? "Maskin: MaxMuller - OP1 & 2" : "",
            ["SumMaskinValS2"] = machine == "LB45" ? "Maskin: LB45 - OP3" : machine == "MaxMuller/K&T" ? "Maskin: K&T - OP3" : ""
        };

        private Dictionary<string, string> GetPage1Measurements(string machine)
        {
            bool ok = machine == "LB45" || machine == "MaxMuller/K&T";
            return new Dictionary<string, string>
            {
                ["SumF1_1"] = ok ? "1/1" : "",
                ["SumF1_2"] = ok ? "1/2" : "",
                ["SumF1_3"] = ok ? "1/5" : "",
                ["SumF1_4"] = ok ? "1/5" : "",
                ["SumF1_5"] = ok ? "1/5" : "",
                ["SumF1_6"] = ok ? "1/5" : "",
                ["SumF1_7"] = ok ? "1/3" : "",
                ["SumF1_8"] = ok ? "1/5" : "",

                ["SumD1_1"] = ok ? "Multimar" : "",
                ["SumD1_2"] = ok ? "Mikrometer" : "",
                ["SumD1_3"] = ok ? "Skjutmått" : "",
                ["SumD1_4"] = ok ? "Skjutmått" : "",
                ["SumD1_5"] = ok ? "Skjutmått" : "",
                ["SumD1_6"] = ok ? "Skjutmått" : "",
                ["SumD1_7"] = ok ? "Ytjämnhetsmätare" : "",
                ["SumD1_8"] = ok ? "Egglinjal" : "",

                ["SumAF1_1"] = ok ? "Kontrolleras med passbitsklove utf. 1" : "",
                ["SumAF1_2"] = "",
                ["SumAF1_3"] = "",
                ["SumAF1_4"] = "",
                ["SumAF1_5"] = "",
                ["SumAF1_6"] = ok ? "Bearbetning i OP1 + 3mm" : "",
                ["SumAF1_7"] = ok ? "Övriga Ra värden 6,3" : "",
                ["SumAF1_8"] = ok ? "Vid misstänkt formfel lämnas till mätrum" : ""
            };
        }

        private Dictionary<string, string> GetPage2Measurements(string machine)
        {
            bool ok = machine == "LB45" || machine == "MaxMuller/K&T";
            return new Dictionary<string, string>
            {
                ["SumF2_1"] = ok ? "1/5" : "",
                ["SumF2_2"] = ok ? "1/5" : "",
                ["SumF2_3"] = ok ? "1/5" : "",
                ["SumF2_4"] = ok ? "1/5" : "",

                ["SumD2_1"] = ok ? "Skjutmått" : "",
                ["SumD2_2"] = ok ? "Djupmått" : "",
                ["SumD2_3"] = ok ? "UNC 5/8-11 min/max" : "",
                ["SumD2_4"] = "",

                ["SumAF2_1"] = "",
                ["SumAF2_2"] = "",
                ["SumAF2_3"] = "",
                ["SumAF2_4"] = ""
            };
        }
    }
}