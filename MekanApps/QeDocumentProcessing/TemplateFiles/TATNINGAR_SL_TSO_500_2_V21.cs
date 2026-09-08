using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class TATNINGAR_SL_TSO_500_2_V21 : ITemplateCalculations
    {
        private const string LB = "<<LineBreak>>";
        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var kv = new Dictionary<string, string>();

            kv["DocumentUniqueId"] = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);

            string drawing = "SL-TSO 522-2/V21";
            string machine = (request.MachineNumber ?? "").Trim().ToUpperInvariant();

            Merge(kv, GetAdminTexts(drawing));
            Merge(kv, GetDiameters());
            Merge(kv, GetWidths());
            Merge(kv, GetThreads());
            Merge(kv, GetAnglesAndSurface());
            Merge(kv, GetMachine(machine));
            Merge(kv, GetPageMeasurements(machine));

            return kv;
        }

        private void Merge(Dictionary<string, string> t, Dictionary<string, string> s)
        {
            foreach (var e in s)
                t[e.Key] = e.Value ?? "";
        }

        private Dictionary<string, string> GetAdminTexts(string drawing) => new()
        {
            ["SumRit"] = "7433012",
            ["SumStämpel"] = "Stämplas: " + drawing,
            ["SumTextS1"] = "Okulärkontroll: Grader, frifläcker, slagmärken, repor, valkar, ytjämnhet och faser" + LB + "" + LB + "Bearbetas Ra 6.3 där annat ej anges, skarpa kanter avgradas."
        };

        private Dictionary<string, string> GetDiameters() => new()
        {
            
            ["Sumn"] = "2x45º",
            ["SumD"] = "(D) 105",
            ["SumDTol"] = "+ 0.035",
            ["SumDTolN"] = "- 0",

            ["SumD1"] = "(D1) 115",
            ["SumD1Tol"] = "+ 0.5",
            ["SumD1TolN"] = "- 0",

            ["SumD2"] = "(D2) 162",
            ["SumD2Tol"] = "± 0.5",

            ["SumD3"] = "(D3) 133",
            ["SumD3Tol"] = "+ 0",
            ["SumD3TolN"] = "- 0.250",

            ["SumD4"] = "(D4) 150",
            ["SumD4Tol"] = "+ 0.250",
            ["SumD4TolN"] = "- 0",

            ["SumD5"] = "",
            ["SumD5Tol"] = "",

            ["SumD6"] = "",
            ["SumD6Tol"] = "",
            ["SumD6TolN"] = "",

            ["SumD7"] = "(D7) 109",
            ["SumD7Tol"] = "± 0.2"
        };

        private Dictionary<string, string> GetWidths() => new()
        {
            ["Suma"] = "(a) 66",
            ["SumaTol"] = "+ 0",
            ["SumaTolN"] = "- 0.1",

            ["Sumc"] = "(c) 60",
            ["SumcTol"] = "+ 0.2",
            ["SumcTolN"] = "- 0",

            ["Sume"] = "(e) 40,5",
            ["SumeTol"] = "+ 0.2",
            ["SumeTolN"] = "- 0",

            ["Sumk"] = "(k) 22",
            ["SumkTol"] = "± 0.2",

            ["Summ"] = "(m) 3",
            ["SummTol"] = "± 0.1",

            ["Sumh"] = "(h) 8",
            ["SumhTol"] = "± 0.2",

            ["Sumf"] = "(f) 37",
            ["SumfTol"] = "± 0.3"
        };

        private Dictionary<string, string> GetThreads() => new()
        {
            ["Sumg"] = "(g) M8",
            ["Sumg1"] = "M8"
        };

        private Dictionary<string, string> GetAnglesAndSurface() => new()
        {
            ["Sum60"] = "60º",
            ["Sum60a"] = "60º",
            ["Sum8"] = "8º ± 1º",
            ["SumRa32"] = "3.2",
            ["SumRa32a"] = "3.2",
            ["SumRa32b"] = "3.2",
            ["SumRa63"] = "6.3"
        };

        private Dictionary<string, string> GetMachine(string machine)
        {
            string display = machine == "LT-3000" ? "LT-3000" : "Nakamura";

            return new Dictionary<string, string>
            {
                ["SumMaskinValS1"] = "Maskin: " + display
            };
        }

        private Dictionary<string, string> GetPageMeasurements(string machine)
        {
            bool ok = machine == "NAKAMURA" || machine == "LT-3000";

            return new Dictionary<string, string>
            {
                ["SumF1_1"] = ok ? "1/3" : "",
                ["SumF1_2"] = ok ? "1/3" : "",
                ["SumF1_3"] = ok ? "1/2" : "",
                ["SumF1_4"] = ok ? "1/3" : "",
                ["SumF1_5"] = ok ? "1/1" : "",
                ["SumF1_6"] = ok ? "1/2" : "",
                ["SumF1_7"] = ok ? "1/2" : "",
                ["SumF1_8"] = ok ? "1/3" : "",
                ["SumF1_9"] = ok ? "1/1" : "",
                ["SumF1_0"] = ok ? "1/3" : "",
                ["SumF1_11"] = ok ? "1/3" : "",
                ["SumF1_12"] = ok ? "1/3" : "",
                ["SumF1_13"] = ok ? "1/3" : "",

                ["SumD1_1"] = ok ? "Digitalt Skjutmått" : "",
                ["SumD1_2"] = ok ? "Digitalt Skjutmått" : "",
                ["SumD1_3"] = ok ? "Digitalt Skjutmått" : "",
                ["SumD1_4"] = ok ? "Digitalt Skjutmått" : "",
                ["SumD1_5"] = ok ? "UD-Apparat" : "",
                ["SumD1_6"] = ok ? "Djupmått ev. med klocka" : "",
                ["SumD1_7"] = ok ? "Digitalt Skjutmått" : "",
                ["SumD1_8"] = ok ? "Digitalt Skjutmått" : "",
                ["SumD1_9"] = ok ? "Okulärkontroll" : "",
                ["SumD1_0"] = ok ? "Ytjämnhetsmätare" : "",
                ["SumD1_11"] = ok ? "Digitalt Skjutmått" : "",
                ["SumD1_12"] = ok ? "Digitalt Skjutmått" : "",
                ["SumD1_13"] = ok ? "M8 Tolk" : "",

                ["SumAF1_1"] = "",
                ["SumAF1_2"] = "",
                ["SumAF1_3"] = "",
                ["SumAF1_4"] = "",
                ["SumAF1_5"] = ok ? "Klove/Ring 105, mät båda sidor" : "",
                ["SumAF1_6"] = ok ? "Passbitar" : "",
                ["SumAF1_7"] = "",
                ["SumAF1_8"] = "",
                ["SumAF1_9"] = ok ? "60º" : "",
                ["SumAF1_0"] = "",
                ["SumAF1_11"] = "",
                ["SumAF1_12"] = "",
                ["SumAF1_13"] = ok ? "120° delning" : ""
            };
        }
    }
}