using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class MUTTRAR_ANN_0083_V21 : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var kv = new Dictionary<string, string>();

            kv["DocumentUniqueId"] =
                DateTime.UtcNow.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);

            string product = Normalize(request.ProductDesignation);
            string machine = Normalize(request.MachineNumber);

            Merge(kv, Admin());
            Merge(kv, Diameters());
            Merge(kv, Widths());
            Merge(kv, ThreadsAndBores());
            Merge(kv, Geometry());
            Merge(kv, Other());
            Merge(kv, Machine(machine));
            Merge(kv, Page1(machine));
            Merge(kv, Page2(machine));

            return kv;
        }

        private void Merge(Dictionary<string, string> t, Dictionary<string, string> s)
        {
            foreach (var e in s)
                t[e.Key] = e.Value ?? "";
        }

        private string Normalize(string v) =>
            string.IsNullOrWhiteSpace(v) ? "" : v.Trim();

        private bool IsMachine(string m, params string[] list)
        {
            foreach (var x in list)
                if (m.Contains(x, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }

        private Dictionary<string, string> Admin() => new()
        {
            ["SumRitS1"] = "produkt: ANN-0083/V21, Stämplas enl. 7433200:senaste utg.",
            ["SumRitS2"] = "produkt: ANN-0083/V21, Stämplas enl. 7433200:senaste utg.",
            ["SumTextS1"] = "Grader, frifläckar, slagmärken, repor, valkar & andra defekter samt ojämnheter kontrolleras med ögonmått, för övriga mått används skjutmått.",
            ["SumTextS2"] = "Övriga mått kontrolleras vid inställning, för ej toleranssatta mått gäller iso 2768 mk",
            ["SumKlEgenskaperS2"] = "PPA & PPH/ALLMÄN/KLASSADE EGENSKAPER/Klassade egenskaper muttrar"
        };

        private Dictionary<string, string> Diameters() => new()
        {
            ["Sumd"] = "(d) 397",
            ["SumdTol"] = "+ 0.89",
            ["SumdTolN"] = "- 0.00",

            ["Sumd1"] = "(d1) 390",
            ["Sumd1Tol"] = "+ 0.000",
            ["Sumd1TolN"] = "- 0.630",

            ["Sumd2"] = "(d2) 408",
            ["Sumd2Tol"] = "+ 0.970",
            ["Sumd2TolN"] = "- 0.000",

            ["Sumdm"] = "(dm) 392.5",
            ["SumdmTol"] = "+ 0.450",
            ["SumdmTolN"] = "- 0.000",

            ["Sumd3"] = "(d3) 520",
            ["Sumd3Tol"] = "+ 0.530",
            ["Sumd3TolN"] = "- 0.000",

            ["Sumd4"] = "(d4) 395.5"
        };

        private Dictionary<string, string> Widths() => new()
        {
            ["Sumb"] = "(b) 145",
            ["SumbTol"] = "± 0.230",
            ["Sumb1"] = "(b1) 72.5",
            ["SumMax30"] = "21 Max 30",
            ["SumMax30Tol"] = "+ 0.2",
            ["SumMax30TolN"] = "- 0"
        };

        private Dictionary<string, string> ThreadsAndBores() => new()
        {
            ["SumThread"] = "Tr 395x5",
            ["SumM"] = "M12",
            ["SumHole"] = "Ø 12.5",
            ["SumTorque"] = "Skruv dras åt med 50nm"
        };

        private Dictionary<string, string> Geometry() => new()
        {
            ["SumF1"] = "45°",
            ["SumF2"] = "2x45°",
            ["SumF3"] = "5x45°",
            ["SumF4"] = "30°",
            ["SumRadius"] = "2.5",
            ["SumR05"] = "max R1",
            ["SumRa"] = "Ra 6.3"
        };

        private Dictionary<string, string> Other() => new()
        {
            ["SumControl"] = "Kontrolleras enlig styrplan.",
            ["SumNote"] = "Gänggradning"
        };

        private Dictionary<string, string> Machine(string m)
        {
            string s1 = "";
            string s2 = "";

            if (IsMachine(m, "LB"))
            {
                s1 = "LB:45 - Svarvning";
                s2 = "LB:45 - Borr & Fräsning";
            }
            else if (IsMachine(m, "DS900"))
            {
                s1 = "DS900 - Svarvning";
                s2 = "DS900 - Borr & Fräsning";
            }

            return new Dictionary<string, string>
            {
                ["SumMaskinValS1"] = "Maskin: " + s1,
                ["SumMaskinValS2"] = "Maskin: " + s2
            };
        }

        private Dictionary<string, string> Page1(string m)
        {
            bool ok = IsMachine(m, "LB", "DS900");

            return new Dictionary<string, string>
            {
                ["SumF1_1"] = ok ? "1/1" : "",
                ["SumF1_2"] = ok ? "1/1" : "",
                ["SumF1_3"] = ok ? "1/1" : "",
                ["SumF1_4"] = ok ? "1/1" : "",
                ["SumF1_5"] = ok ? "1/1" : "",

                ["SumD1_1"] = ok ? "Skjutmått" : "",
                ["SumD1_2"] = ok ? "Skjutmått" : "",
                ["SumD1_3"] = ok ? "Skjutmått" : "",
                ["SumD1_4"] = ok ? "Multimar" : "",
                ["SumD1_5"] = ok ? "Skjutmått" : ""
            };
        }

        private Dictionary<string, string> Page2(string m)
        {
            bool ok = IsMachine(m, "LB", "DS900");

            return new Dictionary<string, string>
            {
                ["SumF2_1"] = ok ? "1/1" : "",
                ["SumF2_2"] = ok ? "1/1" : "",
                ["SumF2_3"] = ok ? "1/1" : "",

                ["SumD2_1"] = ok ? "Gängtolk" : "",
                ["SumD2_2"] = ok ? "Gängtolk" : "",
                ["SumD2_3"] = ok ? "Passbitar" : ""
            };
        }
    }
}