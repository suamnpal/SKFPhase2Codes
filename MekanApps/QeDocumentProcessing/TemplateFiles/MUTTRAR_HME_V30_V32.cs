using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class MUTTRAR_HME_V30_V32 : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var kv = new Dictionary<string, string>();

            kv["DocumentUniqueId"] =
                DateTime.UtcNow.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);

            string machine = (request.MachineNumber ?? "").ToUpperInvariant();


            string size = request.ProductDesignation.Contains("3096") ? "3096" : "3076";

            Merge(kv, Core(size));
            Merge(kv, Calculations(size));

            Merge(kv, Classification());
            Merge(kv, Texts(size));
            Merge(kv, Machine(machine));
            Merge(kv, Page1(size));
            Merge(kv, Page2(size));
            Merge(kv, Remarks(size));

            return kv;
        }

        private void Merge(Dictionary<string, string> t, Dictionary<string, string> s)
        {
            foreach (var e in s)
                t[e.Key] = e.Value ?? "";
        }

        private Dictionary<string, string> Core(string size)
        {
            if (size.Contains("3096"))
            {
                return new()
                {
                    ["SumRa32"] = "3.2 [2F]",
                    ["Sumr1"] = "R 4",
                    ["SumFas"] = "30º",
                    ["SumFas2"] = "30º",
                    ["SumGF"] = "45º",
                    ["SumGF1"] = "45º",
                    ["SumP"] = "(P) 5",
                    ["SumGänga"] = "Tr 480x5",
                    ["SumStämpling"] = "Stämplas enl. ritning Stämplas enl. 7433462"
                };
            }

            return new()
            {
                ["SumRa32"] = "3.2 [2F]",
                ["Sumr1"] = "R 3.5",
                ["SumFas"] = "30º",
                ["SumFas2"] = "30º",
                ["SumGF"] = "45º",
                ["SumGF1"] = "45º",
                ["SumP"] = "(P) 5",
                ["SumGänga"] = "Tr 380x5",
                ["SumStämpling"] = "Stämplas enl. ritning Stämplas enl. 7433462"
            };
        }

        private Dictionary<string, string> Calculations(string size)
        {
            if (size.Contains("3096"))
                return Calculations_3096();

            return Calculations_3076();
        }

        private Dictionary<string, string> Calculations_3076() => new()
        {
            ["SumDm"] = "(Dm) 377.5",
            ["SumDmTol"] = "+ 0.530 [3F]",
            ["SumDmTolN"] = "- 0.0 [2F]",

            ["SumDi"] = "(Di) 375",
            ["SumDiTol"] = "+ 0.450 [3F]",
            ["SumDiTolN"] = "- 0.0 [2F]",

            ["Sumd3"] = "(d3) 450",
            ["Sumd3Tol"] = "+ 0.0 [3F]",
            ["Sumd3TolN"] = "- 0.970",

            ["Sumd1"] = "(d1) 422",
            ["Sumd1Tol"] = "+ 0.0 [3F]",
            ["Sumd1TolN"] = "- 0.970",

            ["Sumd"] = "(d) 382",
            ["SumdTol"] = "+ 0.890 [3F]",
            ["SumdTolN"] = "- 0.0 [3F]",

            ["SumB"] = "(B) 44",
            ["SumBTol"] = "+ 0.0",
            ["SumBTolN"] = "- 0.390 [3F]",

            ["SumB3"] = "(B3) 10",
            ["SumB3Tol"] = "± 0.200",

            ["Sumt1"] = "0.08",
            ["Sumt2"] = "0.08",

            ["Sumd2"] = "(d2) 416",
            ["Sumd2Tol"] = "± 0.800",

            ["SumG2"] = "(G2) M12",
            ["SumL2"] = "(L2) 22",
            ["SumL2Tol"] = "+ 2.0",
            ["SumL2TolN"] = "- 0.0",
            ["SumLd2"] = "(Ld2) max:26.5"
        };

        private Dictionary<string, string> Calculations_3096() => new()
        {
            ["SumDm"] = "(Dm) 477.5",
            ["SumDmTol"] = "+ 0.530 [3F]",
            ["SumDmTolN"] = "- 0.0 [2F]",

            ["SumDi"] = "(Di) 475",
            ["SumDiTol"] = "+ 0.450 [3F]",
            ["SumDiTolN"] = "- 0.0 [2F]",

            ["Sumd3"] = "(d3) 560",
            ["Sumd3Tol"] = "+ 0.0 [3F]",
            ["Sumd3TolN"] = "- 1.100",

            ["Sumd1"] = "(d1) 530",
            ["Sumd1Tol"] = "+ 0.0 [3F]",
            ["Sumd1TolN"] = "- 1.100",

            ["Sumd"] = "(d) 482",
            ["SumdTol"] = "+ 0.970 [3F]",
            ["SumdTolN"] = "- 0.0 [3F]",

            ["SumB"] = "(B) 49",
            ["SumBTol"] = "+ 0.0",
            ["SumBTolN"] = "- 0.390 [3F]",

            ["SumB3"] = "(B3) 12",
            ["SumB3Tol"] = "± 0.200",

            ["Sumt1"] = "0.09",
            ["Sumt2"] = "0.09",

            ["Sumd2"] = "(d2) 522",
            ["Sumd2Tol"] = "± 0.800",

            ["SumG2"] = "(G2) M12",
            ["SumL2"] = "(L2) 27",
            ["SumL2Tol"] = "+ 2.0",
            ["SumL2TolN"] = "- 0.0",

            ["SumLd2"] = "(Ld2) max:32"
        };
        private Dictionary<string, string> Classification() => new()
        {
            ["SumKlEgenskaperS1"] =
                "PRODUCTION NUTS & SLEEVES & HOUSINGS\\ALLMÄNKLASSADE EGENSKAPER\\Klassade egenskaper muttrar",
            ["SumKlEgenskaperS2"] =
                "PRODUCTION NUTS & SLEEVES & HOUSINGS\\ALLMÄNKLASSADE EGENSKAPER\\Klassade egenskaper muttrar"
        };

        private Dictionary<string, string> Texts(string size) => new()
        {
            ["SumRitningsnrS1"] = size == "3096" ? "Produkt: 7433049" : "Produkt: 7433729",
            ["SumRitningsnrS2"] = size == "3096" ? "Produkt: 7433049" : "Produkt: 7433729",
            ["SumTextS1"] = "Grader, frifläckar, slagmärken, repor, valkar och andra defekter samt ojämnheter kontrolleras med ögonmått, för övriga mått används skjutmått.",
            ["SumTextS2"] = "Grader, frifläckar, slagmärken, repor, valkar och andra defekter samt ojämnheter kontrolleras med ögonmått, för övriga mått används skjutmått."
        };

        private Dictionary<string, string> Machine(string m)
        {
            if (m.Contains("LB45"))
                return MachineValues("LB45", "Svarvning");
            if (m.Contains("MAXMULLER"))
                return new()
                {
                    ["SumMaskinValS1"] = "Maskin: MaxMuller - OP1 & 2",
                    ["SumMaskinValS2"] = "Maskin: K&T - OP3"
                };
            if (m.Contains("VTR"))
                return MachineValues("VTR-160", "Svarvning");
            if (m.Contains("MACTURN"))
                return MachineValues("MacTurn 550", "Svarvning");

            return new();
        }

        private Dictionary<string, string> MachineValues(string m, string op) => new()
        {
            ["SumMaskinValS1"] = $"Maskin: {m} - Svarvning",
            ["SumMaskinValS2"] = $"Maskin: {m} - Borrning & Fräsning"            
        };

        private Dictionary<string, string> Page1(string size) => new()
        {
            ["SumF1_1"] = "1/1",
            ["SumF1_2"] = "1/2",
            ["SumF1_3"] = "1/5",
            ["SumF1_4"] = "1/5",
            ["SumF1_5"] = "1/5",
            ["SumF1_6"] = "1/5",
            ["SumF1_7"] = "1/3",
            ["SumF1_8"] = "1/5",
            ["SumF1_9"] = "1/2",

            ["SumD1_1"] = "Multimar",
            ["SumD1_2"] = "Mikrometer",
            ["SumD1_3"] = "Skjutmått",
            ["SumD1_4"] = "Skjutmått",
            ["SumD1_5"] = "Skjutmått",
            ["SumD1_6"] = "Skjutmått",
            ["SumD1_7"] = "Ytjämnhetsmätare",
            ["SumD1_8"] = "Egglinjal",
            ["SumD1_9"] = size == "3076" ? "Gängmall Tr 380x5" : "Gängmall Tr 480x5",
        };

        private Dictionary<string, string> Page2(string size) => new()
        {
            ["SumF2_1"] = "1/5",
            ["SumF2_2"] = "1/5",
            ["SumF2_3"] = size == "3096" ? "1/5" : "",
            ["SumF2_4"] = size == "3096" ? "1/5" : "",
            ["SumF2_5"] = "1/5",
            ["SumF2_6"] = "Inst.",

            ["SumD2_1"] = "Tolk: M12 min/max",
            ["SumD2_2"] = "Djupmått/Tolk: M12 min/max",
            ["SumD2_3"] = size == "3096" ? "Tolk: M 10 min/max" : "",
            ["SumD2_4"] = size == "3096" ? "Djupmått" :"",
            ["SumD2_5"] = "KMM-Mätmaskin",
            ["SumD2_6"] = "Skjutmått",
        };

        private Dictionary<string, string> Remarks(string size)
        {
            if (size.Contains("3096"))
            {
                return new()
                {
                    ["SumAF1_1"] = "Kontrolleras med passbitsklove utf. 1",
                    ["SumAF1_2"] = "",
                    ["SumAF1_3"] = "",
                    ["SumAF1_4"] = "",
                    ["SumAF1_5"] = "",
                    ["SumAF1_6"] = "Bredd (B) OP1 +14mm",
                    ["SumAF1_7"] = "Övriga Ra värden 6,3",
                    ["SumAF1_8"] = "Vid misstänkt formfel lämnas till mätrum",
                    ["SumAF2_5"] = "Vid misstänkt formfel lämnas till mätrum",
                    ["SumAF1_9"]="",
                    ["SumM2_3"] = "Gänga Lyftögla",
                    ["SumM2_4"] = "Gäng & borrdjup Lyftögla G3",
                    ["SumB2_3"] = "G3",
                    ["SumB2_4"] = "L2, & Ld2",
                    ["SumAF2_1"] = "",
                    ["SumAF2_2"] = "",
                    ["SumAF2_3"] = "",
                    ["SumAF2_4"] = "",
                    ["SumAF2_5"] = "Vid misstänkt formfel lämnas till mätrum",
                    ["SumAF2_6"]="",
                    ["SumG3"] = "(G3) M10",
                    ["SumC"] = "Ø 0.5",
                    ["SumL3"] = "(L3) 17",
                    ["SumL4"] = "(L4) 31",
                    ["SumLd3"] = "(Ld3) max:23.5",
                    ["SumL3Tol"] = "+2.0",
                    ["SumL3TolN"] = "-0.0",
                    ["SumL4Tol"] = "±0.300",
                    ["SumL4txt"] = ""
                };
            }

            return new()
            {
                ["SumAF1_1"] = "Kontrolleras med passbitsklove utf. 1",
                ["SumAF1_2"] = "",
                ["SumAF1_3"] = "",
                ["SumAF1_4"] = "",
                ["SumAF1_5"] = "",
                ["SumAF1_6"] = "Bredd (B) OP1 +7mm",
                ["SumAF1_7"] = "Övriga Ra värden 6,3",
                ["SumAF1_8"] = "Vid misstänkt formfel lämnas till mätrum",
                ["SumAF2_5"] = "Vid misstänkt formfel lämnas till mätrum",
                ["SumAF1_9"] = "",

                ["SumM2_3"] = "",
                ["SumM2_4"] = "",
                ["SumB2_3"] = "",
                ["SumB2_4"] = "",
                ["SumAF2_1"] = "",
                ["SumAF2_2"] = "",
                ["SumAF2_3"] = "",
                ["SumAF2_4"] = "",
                ["SumAF2_6"] = "",

                ["SumG3"] = "- *",
                ["SumC"] = "Ø 0.5",
                ["SumL3"] = "- *",
                ["SumL4"] = "- *",
                ["SumLd3"] = "- *",
                ["SumL3Tol"] = "",
                ["SumL3TolN"] = "",
                ["SumL4Tol"] = "",
                ["SumL4txt"] = "* - Ingen lyftögla på produkten."
            };
        }
    }
}