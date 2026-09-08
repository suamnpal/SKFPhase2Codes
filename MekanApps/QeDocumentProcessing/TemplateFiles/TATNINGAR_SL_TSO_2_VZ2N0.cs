using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class TATNINGAR_SL_TSO_2_VZ2N0 : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var kv = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            Merge(kv, Diameters());
            Merge(kv, Widths());
            Merge(kv, Slots());
            Merge(kv, Threads());
            Merge(kv, RadiiAndSurface());
            Merge(kv, Page1Measurements());
            Merge(kv, Page2Measurements());
            Merge(kv, Admin(request));

            return kv;
        }

        private Dictionary<string, string> Diameters()
        {
            return new Dictionary<string, string>
            {
                ["SumD"] = "(d) 80",
                ["SumDTol"] = "+ 0.030",
                ["SumDTolN"] = "+ 0",
                ["SumD1"] = "(D1) 92",
                ["SumD1Tol"] = "+ 0.5",
                ["SumD1TolN"] = "- 0",
                ["SumD2"] = "",
                ["SumD2Tol"] = "",
                ["SumD3"] = "",
                ["SumD3Tol"] = "",
                ["SumD4"] = "(D4) 128",
                ["SumD4Tol"] = "± 0.5",
                ["SumD5"] = "(D5) 100",
                ["SumD5Tol"] = "+ 0",
                ["SumD5TolN"] = "- 0.220",
                ["SumD6"] = "(D6) 114",
                ["SumD6Tol"] = "+ 0.220",
                ["SumD6TolN"] = "- 0",
                ["SumD7"] = "(D7) 84",
                ["SumD7Tol"] = "± 0.5"
            };
        }

        private Dictionary<string, string> Widths()
        {
            return new Dictionary<string, string>
            {
                ["Suma"] = "(a) 55,5",
                ["SumaTol"] = "+ 0",
                ["SumaTolN"] = "- 0.1",
                ["Sumc"] = "(c) 52",
                ["SumcTol"] = "+ 0.2",
                ["SumcTolN"] = "- 0",
                ["Sume"] = "(e) 39",
                ["SumeTol"] = "+ 0.2",
                ["SumeTolN"] = "- 0",
                ["Sumk"] = "(k) 22",
                ["SumkTol"] = "± 0.2",
                ["Sumh"] = "(h) 8",
                ["SumhTol"] = "± 0.2",
                ["Summ"] = "(m) 3",
                ["SummTol"] = "± 0.1",
                ["Sumf"] = "(f) 4",
                ["SumfTol"] = "± 0.1",
                ["Sumn"] = "2x45º"
            };
        }

        private Dictionary<string, string> Slots()
        {
            return new Dictionary<string, string>
            {
                ["SumD8"] = "(D8) 84.8",
                ["SumD8Tol"] = "+ 0.140",
                ["SumD8TolN"] = "- 0.0",
                ["SumB1"] = "2x (B1) 4.0",
                ["SumB1Tol"] = "+ 0.200",
                ["SumB1TolN"] = "- 0.0",
                ["SumL1"] = "(L1) 16",
                ["SumL1Tol"] = "± 0.200",
                ["SumL2"] = "(L2) 39",
                ["SumL2Tol"] = "± 0.300"
            };
        }

        private Dictionary<string, string> Threads()
        {
            return new Dictionary<string, string>
            {
                ["Sumg"] = "(g) M6"
            };
        }

        private Dictionary<string, string> RadiiAndSurface()
        {
            return new Dictionary<string, string>
            {
                ["SumRs"] = "4 x R0.200",
                ["SumRa32"] = "3.2",
                ["SumRa32a"] = "3.2",
                ["SumRa32b"] = "3.2",
                ["SumRa32c"] = "3.2",
                ["SumRa32d"] = "3.2",
                ["SumRa32e"] = "3.2",
                ["SumRa63a"] = "6.3",
                ["SumRa63b"] = "6.3",
                ["SumRa63c"] = "6.3",
                ["SumRa63d"] = "6.3",
                ["SumRa63e"] = "6.3",
                ["Sum60"] = "60º",
                ["Sum8"] = "8º ± 1º"
            };
        }

        private Dictionary<string, string> Page1Measurements()
        {
            return new Dictionary<string, string>
            {
                ["SumF1_1"] = "1/3",
                ["SumD1_1"] = "Digitalt Skjutmått",
                ["SumAF1_1"] = "",
                ["SumF1_2"] = "1/3",
                ["SumD1_2"] = "Digitalt Skjutmått",
                ["SumAF1_2"] = "",
                ["SumF1_3"] = "1/2",
                ["SumD1_3"] = "Digitalt Skjutmått",
                ["SumAF1_3"] = "",
                ["SumF1_4"] = "1/3",
                ["SumD1_4"] = "Digitalt Skjutmått",
                ["SumAF1_4"] = "",
                ["SumF1_5"] = "1/1",
                ["SumD1_5"] = "UD-Apparat",
                ["SumAF1_5"] = "Klove/Ring 80, mät båda sidor",
                ["SumF1_6"] = "1/2",
                ["SumD1_6"] = "Djupmått ev. med klocka",
                ["SumAF1_6"] = "Passbitar",
                ["SumF1_7"] = "1/2",
                ["SumD1_7"] = "Digitalt Skjutmått",
                ["SumAF1_7"] = "",
                ["SumF1_8"] = "1/3",
                ["SumD1_8"] = "Digitalt Skjutmått",
                ["SumAF1_8"] = "",
                ["SumF1_9"] = "1/1",
                ["SumD1_9"] = "Okulärkontroll",
                ["SumAF1_9"] = "60º",
                ["SumF1_0"] = "1/3",
                ["SumD1_0"] = "Ytjämnhetsmätare",
                ["SumAF1_0"] = "",
                ["SumF1_11"] = "1/3",
                ["SumD1_11"] = "Digitalt Skjutmått",
                ["SumAF1_11"] = "",
                ["SumF1_12"] = "1/3",
                ["SumD1_12"] = "Digitalt Skjutmått",
                ["SumAF1_12"] = "",
                ["SumF1_13"] = "1/3",
                ["SumD1_13"] = "M6 Tolk",
                ["SumAF1_13"] = "120º delning"
            };
        }

        private Dictionary<string, string> Page2Measurements()
        {
            return new Dictionary<string, string>
            {
                ["SumF2_1"] = "1/2",
                ["SumD2_1"] = "Digitalt Skjutmått",
                ["SumAF2_1"] = "",
                ["SumF2_2"] = "1/3",
                ["SumD2_2"] = "Digitalt Skjutmått",
                ["SumAF2_2"] = "",
                ["SumF2_3"] = "1/3",
                ["SumD2_3"] = "Digitalt Skjutmått",
                ["SumAF2_3"] = "",
                ["SumF2_4"] = "1/3",
                ["SumD2_4"] = "Digitalt Skjutmått",
                ["SumAF2_4"] = "",
                ["SumF2_5"] = "1/3",
                ["SumD2_5"] = "Ytjämnhetsmätare",
                ["SumAF2_5"] = ""
            };
        }

        private Dictionary<string, string> Admin(APIRequest request)
        {
            var subject = (request?.ProductDesignation ?? "").Replace(".", ",").ToUpperInvariant();

            return new Dictionary<string, string>
            {
                ["SumRit"] = "7438713, 7433489",
                ["SumRit2"] = "7438713, 7433489",
                ["SumStämpel"] = "Stämplas: " + subject,
                ["SumMaskinValS1"] = $"Maskin: {request?.MachineNumber}",
                ["SumMaskinValS2"] = $"Maskin: {request?.MachineNumber}",
                ["SumTextS1"] =
    "Okulärkontroll: Grader, frifläcker, slagmärken, repor, valkar, ytjämnhet och faser\u2028Bearbetas Ra 6.3 där annat ej anges, skarpa kanter avgradas.",
                ["SumTextS2"] =
    "Okulärkontroll: Grader, frifläcker, slagmärken, repor, valkar, ytjämnhet och faser\u2028Bearbetas Ra 6.3 där annat ej anges, skarpa kanter avgradas.",
            };
        }

        private void Merge(Dictionary<string, string> target, Dictionary<string, string> source)
        {
            foreach (var kv in source)
                target[kv.Key] = kv.Value ?? "";
        }
    }
}