using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    /// <summary>
    /// Template calculation logic for TATNINGAR GR TK 207–212.
    /// Generates all Word parameter fields used in the QE dynamic document.
    /// </summary>
    public class TATNINGAR_GR_TK_207_212 : ITemplateCalculations
    {
        /// <summary>
        /// Calculates and returns all Word template parameters based on the API request.
        /// </summary>
        /// <param name="req">API request containing machine and product data.</param>
        /// <returns>Dictionary of Word template keys and calculated values.</returns>
        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var keyValues = new Dictionary<string, string>();

            string product = (req.ProductDesignation ?? string.Empty)
                .Trim()
                .ToUpperInvariant()
                .Replace(".", ",");

            var parts = product.Split(
                new[] { ' ', '/', '.' },
                StringSplitOptions.RemoveEmptyEntries);

            string type = parts.Length > 1 && parts[1].Length >= 2
                ? parts[1].Substring(parts[1].Length - 2)
                : "00";

            bool machineMatch = IsMachineMatch(req.MachineNumber);

            Combine(keyValues, CalculateDiameters(type));
            Combine(keyValues, CalculateWidths(type));
            Combine(keyValues, CalculateChamfersAndOthers(type));
            Combine(keyValues, CalculatePage1Measurements(machineMatch));
            Combine(keyValues, CalculatePage2Measurements(machineMatch));
            Combine(keyValues, CalculateAdminFields());
            Combine(keyValues, CalculateMachineSelection(req.MachineNumber));

            return keyValues;
        }

        /// <summary>
        /// Merges source dictionary entries into target dictionary.
        /// Existing keys will be overwritten.
        /// </summary>
        private static void Combine(
            Dictionary<string, string> target,
            Dictionary<string, string> source)
        {
            foreach (var kv in source)
            {
                target[kv.Key] = kv.Value;
            }
        }

        /// <summary>
        /// Determines whether the provided machine number matches supported machines.
        /// </summary>
        private static bool IsMachineMatch(string machine)
        {
            if (string.IsNullOrWhiteSpace(machine))
                return false;

            machine = machine.ToUpperInvariant();

            return machine.Contains("NAKAMURA") ||
                   machine.Contains("LB45") ||
                   machine.Contains("LT-3000EX");
        }

        /// <summary>
        /// Calculates diameter measurements and tolerances based on product type.
        /// </summary>
        private Dictionary<string, string> CalculateDiameters(string type)
        {
            var kv = new Dictionary<string, string>();

            var diameterTables = new Dictionary<string, double[]>
            {
                { "07", new[]{47.0,56.0,59.0,61.4,84.5,65.5,70.5,79.5} },
                { "08", new[]{52.0,61.5,65.0,67.4,97.0,74.0,80.0,91.0} },
                { "09", new[]{57.0,66.5,70.0,72.4,102.0,79.0,85.0,96.0} },
                { "10", new[]{62.0,71.5,75.0,77.4,100.0,81.0,86.0,95.0} },
                { "11", new[]{68.0,76.5,80.0,82.4,112.0,89.0,95.0,106.0} },
                { "12", new[]{73.0,86.5,90.0,92.4,121.5,98.5,104.5,115.5} }
            };

            double[] D = diameterTables.ContainsKey(type)
                ? diameterTables[type]
                : new double[8];

            double d = D[0], d1 = D[1], d2 = D[2], d3 = D[3];
            double d4 = D[4], d5 = D[5], d6 = D[6], d7 = D[7];

            kv["Sumd"] = "(d) " + d;
            kv["Sumd1"] = "2x (d1) " + d1;
            kv["Sumd2"] = "(d2) " + d2;
            kv["Sumd3"] = "2x (d3) " + d3;
            kv["Sumd4"] = "(d4) " + d4;
            kv["Sumd5"] = "(d5) " + d5;
            kv["Sumd6"] = "(d6) " + d6;
            kv["Sumd7"] = "(d7) " + d7;

            kv["SumdTol"] = "+ " + FormatNumber(0.4);
            kv["SumdTolN"] = "- 0.0";
            kv["Sumd1Tol"] = "+ 0.0";
            kv["Sumd1TolN"] = "- " + FormatNumber(0.4);
            kv["Sumd2Tol"] = "± " + FormatNumber(0.1);
            kv["Sumd3Tol"] = "+ 0.0";
            kv["Sumd3TolN"] = "- " + FormatNumber(0.3);
            kv["Sumd4Tol"] = "+ 0.0";
            kv["Sumd4TolN"] = "- " + FormatNumber(0.2);

            kv["Sumd5Tol"] = "+ " + FormatNumber(IT11(d5));
            kv["Sumd5TolN"] = "- 0.0";

            kv["Sumd6Tol"] = "+ 0.0";
            kv["Sumd6TolN"] = "- " + FormatNumber(IT11(d6));

            kv["Sumd7Tol"] = "+ " + FormatNumber(IT11(d7));
            kv["Sumd7TolN"] = "- 0.0";

            kv["SumRd"] = FormatNumber(0.1);

            return kv;
        }

        /// <summary>
        /// Calculates width measurements and tolerances based on product type.
        /// </summary>
        private Dictionary<string, string> CalculateWidths(string type)
        {
            var kv = new Dictionary<string, string>();

            var widthTables = new Dictionary<string, double[]>
            {
                { "07", new[]{30.0,5.5,12.0,14.5,8.0} },
                { "08", new[]{32.0,6.0,13.0,13.0,9.0} },
                { "09", new[]{32.0,6.0,13.0,13.0,8.5} },
                { "10", new[]{32.0,6.0,13.0,15.25,8.5} },
                { "11", new[]{30.0,5.0,11.0,14.0,7.0} },
                { "12", new[]{35.0,7.5,16.0,16.5,10.5} }
            };

            double[] B = widthTables.ContainsKey(type)
                ? widthTables[type]
                : new double[5];

            double BW = B[0], b1 = B[1], b2 = B[2], b4 = B[3], b5 = B[4];

            kv["SumB"] = "(B) " + BW;
            kv["SumBTol"] = "+ " + FormatNumber(0.5);
            kv["SumBTolN"] = "- 0.0";

            kv["Sumb1"] = "(b1) " + b1;
            kv["Sumb1Tol"] = "± " + FormatNumber(0.2);

            kv["Sumb2"] = "(b2) " + b2;
            kv["Sumb2Tol"] = "+ 0.0";
            kv["Sumb2TolN"] = "- " + FormatNumber(0.2);

            kv["Sumb4"] = "(b4) " + b4;
            kv["Sumb4Tol"] = "+ 0.0";
            kv["Sumb4TolN"] = "- " + FormatNumber(0.2);

            kv["Sumb5"] = "(b5) " + b5;
            kv["Sumb5Tol"] = "+ 0.0";
            kv["Sumb5TolN"] = "- " + FormatNumber(0.2);

            return kv;
        }

        /// <summary>
        /// Calculates chamfers, radii, threads, and related parameters.
        /// </summary>
        private Dictionary<string, string> CalculateChamfersAndOthers(string type)
        {
            var kv = new Dictionary<string, string>();

            double H = 14.0;

            double J = int.Parse(type) < 19 ? 4.8 : 5.8;
            double K = int.Parse(type) < 19 ? 3.0 : 4.0;
            double N = type == "09" ? 2.1 : 2.2;

            kv["SumH"] = "(H) " + H;
            kv["SumHTol"] = "+ " + FormatNumber(0.5);
            kv["SumHTolN"] = "- 0.0";

            kv["SumJ"] = "(J) " + J;
            kv["SumJTol"] = "+ 0.0";
            kv["SumJTolN"] = "- " + FormatNumber(0.2);

            kv["SumK"] = "(K) " + K;
            kv["SumKTol"] = "+ " + FormatNumber(0.4);
            kv["SumKTolN"] = "- 0.0";

            kv["SumN"] = "(N) " + N;
            kv["SumNTol"] = "+ 0.0";
            kv["SumNTolN"] = "- " + FormatNumber(0.5);

            kv["SumRd"] = FormatNumber(0.1);
            kv["SumCo1"] = "0.15";
            kv["SumRa32"] = "3.2";

            kv["SumG"] = "(G) M6";
            kv["SumGn"] = "Min.8";
            kv["SumBn"] = "Min.10";
            kv["SumBhd"] = "Ø 3";

            string L =
                type == "07" ? "11" :
                type == "10" ? "11" :
                (type == "08" || type == "09" || type == "12") ? "12.5" : "13";

            kv["SumL"] = "(L) " + L;

            kv["SumAm"] = "Inriktningsmärkning";
            kv["SumAm1"] = "(Am1) 1";
            kv["SumAm2"] = "(Am2) 2";
            kv["SumAm2Tol"] = "± " + FormatNumber(0.2);

            kv["SumR08"] = "2 x max: R0.8";
            kv["SumR05"] = "max: R0.5";
            kv["SumR05a"] = "2 x R0.5";
            kv["SumR08b"] = "max: R0.8";
            kv["SumR1"] = "R1 alt. 1x45°";
            kv["SumF1"] = "1x45°";
            kv["SumF15"] = "1.5x45°";

            return kv;
        }

        /// <summary>
        /// Calculates measurement frequencies for page 1 based on machine type.
        /// </summary>
        private Dictionary<string, string> CalculatePage1Measurements(bool measurement)
        {
            var kv = new Dictionary<string, string>();

            kv["SumF1_1"] = measurement ? "1/5" : "";
            kv["SumD1_1"] = measurement ? "Mätmaskin Alt.Skjutmått" : "";
            kv["SumAF1_1"] = "";

            kv["SumF1_2"] = measurement ? "1/5" : "";
            kv["SumD1_2"] = measurement ? "Mätmaskin Alt.UD-Apparat" : "";
            kv["SumAF1_2"] = "";

            kv["SumF1_3"] = measurement ? "1/5" : "";
            kv["SumD1_3"] = measurement ? "Mätmaskin Alt.Skjutmått" : "";
            kv["SumAF1_3"] = "";

            kv["SumF1_4"] = "";
            kv["SumD1_4"] = "";
            kv["SumAF1_4"] = "";

            kv["SumF1_5"] = "";
            kv["SumD1_5"] = "";
            kv["SumAF1_5"] = "";

            kv["SumF1_6"] = measurement ? "Inst." : "";
            kv["SumD1_6"] = measurement ? "Mätmaskin Alt.Mätservice" : "";
            kv["SumAF1_6"] = "";

            kv["SumF1_7"] = measurement ? "Inst. eller vid misstanke" : "";
            kv["SumD1_7"] = measurement ? "Ytjämnhetsmätare" : "";
            kv["SumAF1_7"] = "";

            return kv;
        }

        /// <summary>
        /// Calculates measurement frequencies for page 2 based on machine type.
        /// </summary>
        private Dictionary<string, string> CalculatePage2Measurements(bool measurement)
        {
            var kv = new Dictionary<string, string>();

            kv["SumF2_1"] = measurement ? "1/5" : "";
            kv["SumD2_1"] = measurement ? "Mätmaskin Alt.Skjutmått" : "";
            kv["SumAF2_1"] = "";

            kv["SumF2_2"] = measurement ? "1/5" : "";
            kv["SumD2_2"] = measurement ? "Mätmaskin Alt.Skjutmått" : "";
            kv["SumAF2_2"] = "";

            kv["SumF2_3"] = measurement ? "1/5" : "";
            kv["SumD2_3"] = measurement ? "Mätmaskin Alt.Skjutmått" : "";
            kv["SumAF2_3"] = "";

            kv["SumF2_4"] = measurement ? "1/5" : "";
            kv["SumD2_4"] = measurement ? "Gängtolk" : "";
            kv["SumAF2_4"] = measurement ? "Gängfas min 6.5" : "";

            kv["SumF2_5"] = measurement ? "1/5" : "";
            kv["SumD2_5"] = measurement ? "Mätmaskin Alt.Skjutmått" : "";
            kv["SumAF2_5"] = "";

            kv["SumF2_6"] = measurement ? "Inst. eller vid misstanke" : "";
            kv["SumD2_6"] = measurement ? "Ytjämnhetsmätare" : "";
            kv["SumAF2_6"] = "";

            kv["SumF2_7"] = measurement ? "1/5" : "";
            kv["SumD2_7"] = measurement ? "Mätmaskin Alt.Skjutmått" : "";
            kv["SumAF2_7"] = "";

            return kv;
        }

        /// <summary>
        /// Provides static administrative and visual inspection fields.
        /// </summary>
        private Dictionary<string, string> CalculateAdminFields()
        {
            var kv = new Dictionary<string, string>();

            string visualText =
                "Okulärkontroll Grader, frifläckar, slagmärken, " +
                "repor, valkar, ytjämnhet och faser, skarpa kanter avgradas.";

            kv["SumTextS1"] = visualText;
            kv["SumTextS2"] = visualText;

            kv["SumRitNr"] = "7433673:senaste utg. märkning: 7433523:senaste utg.";
            kv["SumRitNr2"] = kv["SumRitNr"];

            return kv;
        }

        /// <summary>
        /// Calculates machine selection text fields.
        /// </summary>
        private Dictionary<string, string> CalculateMachineSelection(string machine)
        {
            return new Dictionary<string, string>
            {
                ["SumMaskinvalS1"] = $"Maskin: {machine} - Diametrala mått",
                ["SumMaskinvalS2"] = $"Maskin: {machine} - Övriga mått"
            };
        }

        /// <summary>
        /// Formats tolerance values according to QE PDF requirements.
        /// Zero is formatted as "0.0", all non-zero values use three decimals.
        /// </summary>
        private static string FormatNumber(double value)
        {
            if (Math.Abs(value) < 0.0000001)
                return "0.0";

            return value.ToString("0.000", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Calculates IT11 tolerance according to diameter range.
        /// </summary>
        private static double IT11(double x)
        {
            if (x < 3.01) return 0.060;
            if (x < 6.01) return 0.075;
            if (x < 10.01) return 0.090;
            if (x < 18.01) return 0.110;
            if (x < 30.01) return 0.130;
            if (x < 50.01) return 0.160;
            if (x < 80.01) return 0.190;
            if (x < 120.01) return 0.220;
            if (x < 180.01) return 0.250;
            if (x < 250.01) return 0.290;
            if (x < 315.01) return 0.320;
            if (x < 400.01) return 0.360;
            if (x < 500.01) return 0.400;
            return 0.440;
        }
    }
}