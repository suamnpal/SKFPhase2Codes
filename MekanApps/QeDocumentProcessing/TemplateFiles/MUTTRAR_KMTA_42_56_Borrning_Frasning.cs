using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class MUTTRAR_KMTA_42_56_Borrning_Frasning : ITemplateCalculations
    {
        private const string LB = "<<LineBreak>>";
        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var kv = new Dictionary<string, string>();

            kv["DocumentUniqueId"] =
                DateTime.UtcNow.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);

            string product = request.ProductDesignation ?? "";
            string machine = request.MachineNumber ?? "";

            int size = GetSize(product);

            Merge(kv, Admin(size));
            Merge(kv, Core(size));
            Merge(kv, S1(size));
            Merge(kv, S2(size));
            Merge(kv, Remarks(size));
            Merge(kv, Machine(machine));
            Merge(kv, Page1(machine));
            Merge(kv, Page2(machine));

            return kv;
        }

        private int GetSize(string product)
        {
            foreach (var p in product.Split(' '))
                if (int.TryParse(p, out int v))
                    return v;
            return 44;
        }

        private void Merge(Dictionary<string, string> t, Dictionary<string, string> s)
        {
            foreach (var e in s)
                t[e.Key] = e.Value ?? "";
        }

        private Dictionary<string, string> Admin(int size) => new()
        {
            ["SumRitNrS1"] = size == 48 ? "7436445" : "KMTA 44",
            ["SumRitNrS2"] = size == 48
                ? "7436445 Stämplas enl. ritning: 7430920 alt. 7430189"
                : "KMTA 44 Stämplas enl. ritning: 7430920 alt. 7430189",

            ["SumTextS1"] = "100% okulär kontroll av grader, frifläckar, slagmärken, repor, valkar och andra ojämnheter.",

            ["SumTextS2"] = "100% okulär kontroll av grader, frifläckar, slagmärken, repor, valkar och andra ojämnheter." + LB + LB + "" + LB + LB +
            "Vid upptäckta felaktiga detaljer skall kontroll av föregående detalj göras tills första godkända detalj hittas."

        };

        private Dictionary<string, string> Core(int size)
        {
            if (size == 44)
                return new()
                {
                    ["Sumd2"] = "(d2) 265",
                    ["Sumd2Tol"] = "+ 0[3F]",
                    ["Sumd2TolN"] = "- 0.320[3F]",

                    ["Sumd3"] = "(d3) 254",
                    ["Sumd3Tol"] = "+ 0[3F]",
                    ["Sumd3TolN"] = "- 0.5[3F]",

                    ["Sumd5S1"] = "(d5) 221,5",
                    ["Sumd5S1Tol"]="+ 0",
                    ["Sumd5S1TolN"]="- 0.5",

                    ["SumBS1"] = "(B) 37",
                    ["SumBS1Tol"] = "± 0.25",

                    ["SumRa32"] = "Ytjämnhet alla övriga färdig-bearbetade ytor 3.2",
                    ["SumRa25"] = "2.5"
                };

            return new()
            {
                ["Sumd2"] = "(d2) 290",
                ["Sumd2Tol"] = "+ 0[3F]",
                ["Sumd2TolN"] = "- 0.320[3F]",

                ["Sumd3"] = "(d3) 279",
                ["Sumd3Tol"] = "+ 0[3F]",
                ["Sumd3TolN"] = "- 0.5[3F]",

                ["Sumd5S1"] = "(d5) 241,5",
                ["Sumd5S1Tol"] = "+ 0",
                ["Sumd5S1TolN"] = "- 0.5",

                ["SumBS1"] = "(B) 39",
                ["SumBS1Tol"] = "± 0.25",

                ["SumRa32"] = "Ytjämnhet alla övriga färdig-bearbetade ytor 3.2",
                ["SumRa25"] = "2.5"
            };
        }

        private Dictionary<string, string> S1(int size)
        {
            if (size == 44)
                return new()
                {
                    ["SumD1S1"] = "(D1) 215",
                    ["SumD1S1Tol"] = "± 0.2",

                    ["Sume2S1"] = "(e2) 6",
                    ["Sume2S1Tol"] = "± 0.2",

                    ["Sum30a"] = "20º",
                    ["Sum60a"] = "45º",
                    ["Sum60"] = "45º",

                    ["SumBOp1"] = "Bredd (B) efter första operation i högra spindeln = 39mm"
                };

            return new()
            {
                ["SumD1S1"] = "(D1) 235",
                ["SumD1S1Tol"] = "± 0.2",

                ["Sume2S1"] = "(e2) 6",
                ["Sume2S1Tol"] = "± 0.2",

                ["Sum30a"] = "20º",
                ["Sum60a"] = "30º",
                ["Sum60"] = "30º",
                ["SumBOp1"] = "Bredd (B) efter första operation i högra spindeln = 41mm"
            };
        }

        private Dictionary<string, string> S2(int size)
        {
            if (size == 44)
                return new()
                {
                    ["SumG1"] = "M10-6H[3F]",
                    ["Sumf1"] = "(f1) 14",
                    ["Sumf1Tol"] = "+ 1.5[2F]",
                    ["Sumf1TolN"] = "0[2F]",

                    ["Sumf2"] = "(f2) 16",
                    ["Sumf2Tol"] = "0",
                    ["Sumf2TolN"] = "- 1.0",

                    ["Sumd7"] = "(d7) 7.6",

                    ["SumN1"] = "(N1) 8.4",
                    ["SumN1Tol"] = "+ 0.220[2F]",
                    ["SumN1TolN"] = "0[2F]",

                    ["SumN2"] = "(N2) 10",
                    ["SumN2Tol"] = "+ 0.220[2F]",
                    ["SumN2TolN"] = "0[2F]",

                    ["SumHD"] = "251.5 ± 0.360[2F]",
                    ["SumM1"] = "14 ± 0.25",
                    
                    ["Sumf4"] = "(f4) 14.8",
                    ["Sumf4Tol"] = "±0.2[3P]",

                    ["Sumb1"] = "(b1)8.5",
                    ["Sumb1Tol"] = "± 0.25[3F]",

                    ["Sumb2"] = "(b2)15",
                    ["Sumb2Tol"] = "± 0.25[3F]",

                    ["Sumf5"] = "(f5) 15",
                    ["Sumf5Tol"] = "+ 0.5",
                    ["Sumf5TolN"] = "0",

                    ["Sumt3"] = "1.25"
                };

            return new()
            {

                ["SumG1"] = "M12-6H[3F]",
                ["Sumf1"] = "(f1) 17.5",
                ["Sumf1Tol"] = "+ 1.5[2F]",
                ["Sumf1TolN"] = "0[2F]",

                ["Sumf2"] = "(f2) 19.5",
                ["Sumf2Tol"] = "0",
                ["Sumf2TolN"] = "- 1.0",

                ["Sumd7"] = "(d7) 9.96",

                ["SumN1"] = "(N1) 8.4",
                ["SumN1Tol"] = "+ 0.220[2F]",
                ["SumN1TolN"] = "0[2F]",

                ["SumN2"] = "(N2) 10",
                ["SumN2Tol"] = "+ 0.220[2F]",
                ["SumN2TolN"] = "0[2F]",

                ["SumHD"] = "273.5 ± 0.360[2F]",
                ["SumM1"] = "16 ± 0.25",

                ["Sumb1"] = "(b1)8.5",
                ["Sumb1Tol"] = "± 0.25[3F]",


                ["Sumb2"] = "(b2)15",
                ["Sumb2Tol"] = "± 0.25[3F]",

                ["Sumf4"] = "",
                ["Sumf4Tol"] = "",

                ["Sumf5"] = "(f5) 15",
                ["Sumf5Tol"] = "+ 0.5",
                ["Sumf5TolN"] = "0",

                ["Sumt3"] = "1.25"
            };
        }

        private Dictionary<string, string> Remarks(int size)
        {
            var kv = new Dictionary<string, string>();

            for (int i = 1; i <= 7; i++)
            {
                kv[$"SumAF1_{i}"] = "";
                kv[$"SumAF2_{i}"] = "";
                kv[$"SumD1_{i}"] = "";
                kv[$"SumD2_{i}"] = "";
            }

            kv["SumAF2_3"] = "Skruv dras till botten och sedan tillbaks 1½ varv";
            kv["SumD1_e"] = "";
            kv["SumAF1_e"] = "";
            kv["SumAF1_2"] = "Utvändig falsdiameter (d3) efter 1:a OP i H-spindeln: 5mm";
            kv["SumAF1_3"] = size == 44
                ? "Utvändig falsdiameter (d3) efter 1:a OP i H-spindeln: 255mm"
                : "Utvändig falsdiameter (d3) efter 1:a OP i H-spindeln: 280mm";

            kv["SumAF1_4"] = "";

            kv["SumAF2_3"] = "Skruv dras till botten och sedan tillbaks 1½ varv";

            kv["SumKlEgenskaperS1"] = "PPA & PPHALLMÄNKLASSADE EGENSKAPERKlassade egenskaper muttrar KMT KMTA";
            kv["SumKlEgenskaperS2"] = "PPA & PPHALLMÄNKLASSADE EGENSKAPERKlassade egenskaper muttrar KMT KMTA";

            return kv;
        }

        private Dictionary<string, string> Machine(string m)
        {
            var val = m.Contains("LT300", StringComparison.OrdinalIgnoreCase) ? "LT300" : "";

            return new Dictionary<string, string>
            {
                ["SumMaskinValS1"] = "Insvarvning: " + val,
                ["SumMaskinValS2"] = "Borrning: " + val
            };
        }

        private Dictionary<string, string> Page1(string m)
        {
            var ok = m.Contains("LT300", StringComparison.OrdinalIgnoreCase);

            return new Dictionary<string, string>
            {
                ["SumF1_1"] = ok ? "1/10" : "",
                ["SumF1_e"] =  "",
                ["SumF1_2"] = ok ? "1/10" : "",
                ["SumF1_3"] = ok ? "1/10" : "",
                ["SumF1_4"] = ok ? "1/10" : "",
                ["SumF1_5"] = ok ? "1/10" : "",
                ["SumF1_6"] = ok ? "1/10" : "",
                ["SumF1_7"] = ok ? "Inst." : "",


                ["SumF1_1"] = ok ? "1/10" : "",
                ["SumF1_2"] = ok ? "1/10" : "",
                ["SumF1_3"] = ok ? "1/10" : "",
                ["SumF1_4"] = ok ? "1/10" : "",
                ["SumF1_5"] = ok ? "1/10" : "",
                ["SumF1_6"] = ok ? "1/10" : "",
                ["SumF1_7"] = ok ? "Inst." : "",

                ["SumD1_1"] = ok ? "Skjutmått" : "",
                ["SumD1_2"] = ok ? "mall:7424073/3" : "",
                ["SumD1_3"] = ok ? "Skjutmått" : "",
                ["SumD1_4"] = ok ? "Skjutmått" : "",
                ["SumD1_5"] = ok ? "Skjutmått" : "",
                ["SumD1_6"] = ok ? "Ytjämnhetsmätare" : "",
                ["SumD1_7"] = ok ? "Skjutmått" : ""

            };
        }

        private Dictionary<string, string> Page2(string m)
        {
            var ok = m.Contains("LT300", StringComparison.OrdinalIgnoreCase);

            return new Dictionary<string, string>
            {
                ["SumF2_1"] = ok ? "1/10" : "",
                ["SumF1_e"] = "",
                ["SumF2_2"] = ok ? "1/10" : "",
                ["SumF2_3"] = ok ? "1/20" : "",
                ["SumF2_4"] = ok ? "Inst." : "",
                ["SumF2_5"] = ok ? "Inst." : "",
                ["SumF2_6"] = ok ? "Inst." : "",
                ["SumF2_7"] = ok ? "Inst." : "",


                ["SumF2_1"] = ok ? "1/10" : "",
                ["SumF2_2"] = ok ? "1/10" : "",
                ["SumF2_3"] = ok ? "1/20" : "",
                ["SumF2_4"] = ok ? "Inst." : "",
                ["SumF2_5"] = ok ? "Inst." : "",
                ["SumF2_6"] = ok ? "Inst." : "",
                ["SumF2_7"] = ok ? "Inst." : "",

                ["SumD2_1"] = ok ? "Gängtolk" : "",
                ["SumD2_2"] = ok ? "Gängtolk Combi" : "",
                ["SumD2_3"] = ok ? "Kontrollskruv" : "",
                ["SumD2_4"] = ok ? "Håltolk" : "",
                ["SumD2_5"] = ok ? "Håltolk" : "",
                ["SumD2_6"] = ok ? "Skjutmått" : "",
                ["SumD2_7"] = ok ? "Skjutmått" : ""

            };
        }
    }
}