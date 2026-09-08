using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class TATNINGAR_SL_TSO_1_VZ2N0 : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();
            var ci = CultureInfo.InvariantCulture;

            kv["DocumentUniqueId"] =
                DateTime.UtcNow.ToString("yyyyMMddHHmmssfff", ci);

            string product = (req.ProductDesignation ?? "")
                .Trim().ToUpperInvariant().Replace(".", ",");

            int typ = 0;

            if (product.Contains("518"))
                typ = 518;
            else if (product.Contains("524"))
                typ = 524;

            bool measurement = IsMachineMatch(req.MachineNumber);

            kv["SumMaskinValS1"] = $"Maskin: {req.MachineNumber}";
            kv["SumMaskinValS2"] = $"Maskin: {req.MachineNumber}";

            Combine(kv, CalculateDiameters(typ));
            Combine(kv, CalculateWidthsDepthsAngles(typ));
            Combine(kv, CalculateSlotsAndLengths(typ));
            Combine(kv, CalculateThreads());
            Combine(kv, CalculateSurfaceAndRadii());
            Combine(kv, CalculatePage1Measurements(measurement, typ));
            Combine(kv, CalculatePage2Measurements(measurement));
            Combine(kv, CalculateAdminFields(product));

            return kv;
        }

        private static void Combine(Dictionary<string, string> t, Dictionary<string, string> s)
        {
            foreach (var e in s)
                t[e.Key] = e.Value;
        }

        private static bool IsMachineMatch(string m)
        {
            if (string.IsNullOrWhiteSpace(m)) return false;
            m = m.ToUpperInvariant();
            return m.Contains("NAKAMURA") || m.Contains("LT-") || m.Contains("LB");
        }


        private Dictionary<string, string> CalculateDiameters(int typ)
        {
            if (typ == 518)
            {
                return new Dictionary<string, string>
                {
                    ["SumD"] = "(d) 80",
                    ["SumDTol"] = "+ 0.030",
                    ["SumDTolN"] = "+ 0",

                    ["SumD1"] = "(D1) 95",
                    ["SumD1Tol"] = "+ 0.5",
                    ["SumD1TolN"] = "- 0",

                    ["SumD2"] = "(D2) 103",
                    ["SumD2Tol"] = "± 0.3",

                    ["SumD3"] = "(D3) 113",
                    ["SumD3Tol"] = "± 0.3",

                    ["SumD4"] = "(D4) 128",
                    ["SumD4Tol"] = "± 0.5",

                    ["SumD5"] = "(D5) 100",
                    ["SumD5Tol"] = "+ 0",
                    ["SumD5TolN"] = "- 0.220",

                    ["SumD6"] = "(D6) 114",
                    ["SumD6Tol"] = "+ 0.220",
                    ["SumD6TolN"] = "- 0",

                    ["SumD7"] = "(D7) 84",
                    ["SumD7Tol"] = "± 4"
                };
            }

            return new Dictionary<string, string>
            {
                ["SumD"] = "(d) 110",
                ["SumDTol"] = "+ 0.035",
                ["SumDTolN"] = "+ 0",

                ["SumD1"] = "(D1) 128",
                ["SumD1Tol"] = "+ 0.5",
                ["SumD1TolN"] = "- 0",

                ["SumD2"] = "(D2) 139",
                ["SumD2Tol"] = "± 0.5",

                ["SumD3"] = "(D3) 149",
                ["SumD3Tol"] = "± 0.5",

                ["SumD4"] = "(D4) 168",
                ["SumD4Tol"] = "± 0.5",

                ["SumD5"] = "(D5) 135",
                ["SumD5Tol"] = "+ 0",
                ["SumD5TolN"] = "- 0.250",

                ["SumD6"] = "(D6) 154",
                ["SumD6Tol"] = "+ 0.250",
                ["SumD6TolN"] = "- 0",

                ["SumD7"] = "(D7) 114",
                ["SumD7Tol"] = "± 4"
            };
        }


        private Dictionary<string, string> CalculateWidthsDepthsAngles(int typ)
        {
            bool is518 = typ == 518;

            return new Dictionary<string, string>
            {
                ["Suma"] = is518 ? "(a) 75" : "(a) 109",
                ["SumaTol"] = "+ 0",
                ["SumaTolN"] = "- 0.1",

                ["Sumb"] = is518 ? "(b) 55" : "(b) 75",
                ["SumbTol"] = "± 0.3",

                ["Sumc"] = is518 ? "(c) 52" : "(c) 72",
                ["SumcTol"] = "+ 0.2",
                ["SumcTolN"] = "- 0",

                ["Sume"] = is518 ? "(e) 39" : "(e) 47,5",
                ["SumeTol"] = "+ 0.2",
                ["SumeTolN"] = "- 0",

                ["Sump"] = "(p) 10,5",
                ["SumpTol"] = "+ 0.2",
                ["SumpTolN"] = "- 0",

                ["Sumr"] = "(r) 6",
                ["SumrTol"] = "± 0.100",

                ["Summ"] = "(m) 3",
                ["SummTol"] = "± 0.1",

                ["Sumk"] = is518 ? "(k) 22" : "(k) 26",
                ["SumkTol"] = "± 0.2",

                ["Sumh"] = "(h) 8",
                ["SumhTol"] = "± 0.2",

                ["Sumf"] = is518 ? "(f) 10" : "(f) 11",
                ["SumfTol"] = "± 0.2"
            };
        }


        private Dictionary<string, string> CalculateSlotsAndLengths(int typ)
        {
            bool is518 = typ == 518;

            return new Dictionary<string, string>
            {
                ["SumD8"] = is518 ? "(D8) 84.8" : "(D8) 114.8",
                ["SumD8Tol"] = "+ 0.140",
                ["SumD8TolN"] = "- 0.0",

                ["SumB1"] = "2x (B1) 4.0",
                ["SumB1Tol"] = "+ 0.200",
                ["SumB1TolN"] = "- 0.0",

                ["SumL1"] = "(L1) 16",
                ["SumL1Tol"] = "± 0.200",

                ["SumL2"] = is518 ? "(L2) 51" : "(L2) 84",
                ["SumL2Tol"] = "± 0.300"
            };
        }


        private Dictionary<string, string> CalculateThreads() =>
            new Dictionary<string, string>
            {
                ["Sumg"] = "(g) M6"
            };


        private Dictionary<string, string> CalculateSurfaceAndRadii() =>
            new Dictionary<string, string>
            {
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

                ["SumRs"] = "4 x R0.200",
                ["Sum8"] = "8° ± 1°",
                ["Sum60"] = "60°",
                ["Sumn"] = "2x45°",
                ["SumR05a"] = "max R0.5 (4x)"
            };


        private Dictionary<string, string> CalculatePage1Measurements(bool ok, int typ) =>
            new Dictionary<string, string>
            {
                ["SumF1_1"] = ok ? "1/3" : "",
                ["SumD1_1"] = "Digitalt Skjutmått",
                ["SumAF1_1"] = "",

                ["SumF1_2"] = ok ? "1/3" : "",
                ["SumD1_2"] = "Digitalt Skjutmått",
                ["SumAF1_2"] = "",

                ["SumF1_3"] = ok ? "1/2" : "",
                ["SumD1_3"] = "Digitalt Skjutmått",
                ["SumAF1_3"] = "",

                ["SumF1_4"] = ok ? "1/3" : "",
                ["SumD1_4"] = "Digitalt Skjutmått",
                ["SumAF1_4"] = "",

                ["SumF1_5"] = "1/1",
                ["SumD1_5"] = "UD-Apparat",
                ["SumAF1_5"] = typ == 518 ? "Klove/Ring 80, mät båda sidor" : "Klove/Ring 110, mät båda sidor",

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
                ["SumAF1_9"] = "60°",

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
                ["SumAF1_13"] = "120° delning"
            };


        private Dictionary<string, string> CalculatePage2Measurements(bool ok) =>
            new Dictionary<string, string>
            {
                ["SumF2_1"] = ok ? "1/2" : "",
                ["SumD2_1"] = "Digitalt Skjutmått",
                ["SumAF2_1"] = "",

                ["SumF2_2"] = ok ? "1/3" : "",
                ["SumD2_2"] = "Digitalt Skjutmått",
                ["SumAF2_2"] = "",

                ["SumF2_3"] = ok ? "1/3" : "",
                ["SumD2_3"] = "Digitalt Skjutmått",
                ["SumAF2_3"] = "",

                ["SumF2_4"] = ok ? "1/3" : "",
                ["SumD2_4"] = "Digitalt Skjutmått",
                ["SumAF2_4"] = "",

                ["SumF2_5"] = ok ? "1/3" : "",
                ["SumD2_5"] = "Ytjämnhetsmätare",
                ["SumAF2_5"] = ""
            };

        private Dictionary<string, string> CalculateAdminFields(string product) =>

            new Dictionary<string, string>
            {
                ["SumStämpel"] = $"Stämplas: {product}",
                ["SumRit"] = "7438712, 7433488",
                ["SumRit2"] = "7438712, 7433488",
                ["SumTextS1"] =
    "Okulärkontroll: Grader, frifläcker, slagmärken, repor, valkar, ytjämnhet och faser\r" +
    "Bearbetas Ra 6.3 där annat ej anges, skarpa kanter avgradas.",
                ["SumTextS2"] =
    "Okulärkontroll: Grader, frifläcker, slagmärken, repor, valkar, ytjämnhet och faser\r" +
    "Bearbetas Ra 6.3 där annat ej anges, skarpa kanter avgradas.",

            };
    }
}