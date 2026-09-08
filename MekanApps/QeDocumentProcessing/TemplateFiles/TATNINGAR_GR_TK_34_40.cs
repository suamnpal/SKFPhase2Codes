using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class TATNINGAR_GR_TK_34_40 : ITemplateCalculations
    {
        /// <summary>
        /// Calculate Word Parameters
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var keyValues = new Dictionary<string, string>();
            var ci = CultureInfo.InvariantCulture;

            // UNIQUE DOCUMENT KEY (avoid overwrite)
            keyValues["DocumentUniqueId"] = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");

            // PRODUCT DESIGNATION (TmpBet)
            string product = (req.ProductDesignation ?? "").Trim().ToUpperInvariant();
            var parts = product.Replace(".", ",")
                               .Split(new[] { ' ', '/', '.' }, StringSplitOptions.RemoveEmptyEntries);

            int typ = parts.Length > 1 && int.TryParse(parts[1], out var t) ? t : 0;
            bool measurement = IsMachineMatch(req.MachineNumber);

            // MACHINE TEXTS (Lotus compatible names)
            keyValues["SumMaskinValS1"] = $"Maskin: {req.MachineNumber} - Diametrala mått, Form & läge.";
            keyValues["SumMaskinValS2"] = $"Maskin: {req.MachineNumber} - Övriga mått.";

            // template also uses lowercase variant
            keyValues["SumMaskinvalS1"] = keyValues["SumMaskinValS1"];
            keyValues["SumMaskinvalS2"] = keyValues["SumMaskinValS2"];

            // CALCULATIONS
            Combine(keyValues, CalculateDiameters(typ));
            Combine(keyValues, CalculateWidths());
            Combine(keyValues, CalculateThreads());
            Combine(keyValues, CalculateLength());
            Combine(keyValues, CalculateChamfersAndRadii());
            Combine(keyValues, CalculatePage1Measurements(measurement));
            Combine(keyValues, CalculatePage2Measurements(measurement));
            Combine(keyValues, CalculateAdminFields());

            return keyValues;
        }

    
        /// <summary>
        /// HELPERS
        /// </summary>
        /// <param name="t"></param>
        /// <param name="s"></param>
        private void Combine(Dictionary<string, string> t, Dictionary<string, string> s)
        {
            foreach (var kv in s)
                t[kv.Key] = kv.Value;
        }

        /// <summary>
        /// Match machines
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private bool IsMachineMatch(string machineName)
        {
            if (string.IsNullOrWhiteSpace(machineName)) return false;
            machineName = machineName.ToUpperInvariant();
            return machineName.Contains("NAKAMURA") || machineName.Contains("LB45") || machineName.Contains("LT-3000EX");
        }

        /// <summary>
        /// Calculate floating value with decimal points
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private string FloatCalculation(double val)
        {
            if (Math.Abs(val % 1) < 0.000001)
                return val.ToString("0", CultureInfo.InvariantCulture);
            if (Math.Abs((val * 10) % 1) < 0.000001)
                return val.ToString("0.0", CultureInfo.InvariantCulture);
            return val.ToString("0.000", CultureInfo.InvariantCulture);
        }

        private static double H_IT(double dVal)
        {
            if (dVal <= 250) return 0.460;
            if (dVal <= 315) return 0.520;
            if (dVal <= 400) return 0.570;
            return 0.630;
        }

        /// <summary>
        /// DIAMETERS (TmpDTab34–40)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private Dictionary<string, string> CalculateDiameters(int type)
        {
            var keyValues = new Dictionary<string, string>();

            var tables = new Dictionary<int, double[]>
            {
                { 34, new[]{154,169.8,186.2,182.2,184.2,182,190,206,214,230,238,254,262} },
                { 36, new[]{164,179.8,196.2,192.2,194.2,192,200,216,224,240,248,264,272} },
                { 38, new[]{174,190,206.2,202.4,204.4,202,210,226,234,250,258,274,282} },
                { 40, new[]{184,200,216.2,212.2,214.2,212,220,236,244,260,268,284,292} }
            };

            var val = tables.ContainsKey(type) ? tables[type] : new double[13];

            keyValues["Sumd"] = "(d) " + FloatCalculation(val[0]);
            keyValues["Sumd1"] = "2x (d1) " + FloatCalculation(val[1]);
            keyValues["Sumd2"] = "(d2) " + FloatCalculation(val[2]);
            keyValues["Sumd3"] = "(d3) " + FloatCalculation(val[3]);
            keyValues["Sumd4"] = "2x (d4) " + FloatCalculation(val[4]);
            keyValues["Sumd5"] = "(d5) " + FloatCalculation(val[5]);
            keyValues["Sumd6"] = "(d6) " + FloatCalculation(val[6]);
            keyValues["Sumd7"] = "(d7) " + FloatCalculation(val[7]);
            keyValues["Sumd8"] = "(d8) " + FloatCalculation(val[8]);
            keyValues["Sumd9"] = "(d9) " + FloatCalculation(val[9]);
            keyValues["Sumd10"] = "(d10) " + FloatCalculation(val[10]);
            keyValues["Sumd11"] = "(d11) " + FloatCalculation(val[11]);
            keyValues["Sumd12"] = "(d12) " + FloatCalculation(val[12]);

            keyValues["SumdTol"] = "+ 0.400"; keyValues["SumdTolN"] = "- 0.0";
            keyValues["Sumd1Tol"] = "+ 0.0"; keyValues["Sumd1TolN"] = "- 0.300";
            keyValues["Sumd2Tol"] = "+ 0.0"; keyValues["Sumd2TolN"] = "- 0.300";
            keyValues["Sumd3Tol"] = "± 0.100";
            keyValues["Sumd4Tol"] = "+ 0.0"; keyValues["Sumd4TolN"] = "- 0.300";

            keyValues["Sumd5Tol"] = "+ 0.460"; keyValues["Sumd5TolN"] = "- 0.0";
            keyValues["Sumd6Tol"] = "+ 0.0"; keyValues["Sumd6TolN"] = "- 0.460";
            keyValues["Sumd7Tol"] = "+ 0.460"; keyValues["Sumd7TolN"] = "- 0.0";
            keyValues["Sumd8Tol"] = "+ 0.0"; keyValues["Sumd8TolN"] = "- 0.460";

            double it9 = H_IT(val[9]);
            double it10 = H_IT(val[10]);

            keyValues["Sumd9Tol"] = "+ " + FloatCalculation(it9);
            keyValues["Sumd9TolN"] = "- 0.0";

            keyValues["Sumd10Tol"] = "+ 0.0";
            keyValues["Sumd10TolN"] = "- " + FloatCalculation(it10);

            keyValues["Sumd11Tol"] = "+ 0.520"; keyValues["Sumd11TolN"] = "- 0.0";
            keyValues["Sumd12Tol"] = "+ 0.0"; keyValues["Sumd12TolN"] = "- 0.520";

            keyValues["Sumd14"] = "Ø 8";
            keyValues["Sumd14Tol"] = "+ 0.100";
            keyValues["Sumd14TolN"] = "- 0.0";

            keyValues["Sumd15"] = "Ø 3";
            keyValues["Sumd15Tol"] = "± 0.100";

            return keyValues;
        }


        /// <summary>
        /// WIDTHS
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> CalculateWidths()
        {
            return new Dictionary<string, string>
            {
                ["SumB"] = "(B) 58",
                ["SumBTol"] = "+ 0.500",
                ["SumBTolN"] = "- 0.0",

                ["Sumb1"] = "(b1) 7.5",
                ["Sumb1Tol"] = "± 0.200",

                ["Sumb2"] = "(b2) 16",
                ["Sumb2Tol"] = "+ 0.0",
                ["Sumb2TolN"] = "- 0.500",

                ["Sumb3"] = "(b3) 22",
                ["Sumb3Tol"] = "+ 0.840",
                ["Sumb3TolN"] = "- 0.0",

                ["Sumb4"] = "(b4) 16",
                ["Sumb4Tol"] = "+ 0.0",
                ["Sumb4TolN"] = "- 0.200",

                ["Sumb5"] = "2x (b5) 10.5",
                ["Sumb5Tol"] = "+ 0.0",
                ["Sumb5TolN"] = "- 0.500",

                ["Sumb6"] = "(b6) 6.1",
                ["Sumb6Tol"] = "+ 0.200",
                ["Sumb6TolN"] = "- 0.0",

                ["Sumb7"] = "(b7) 10",
                ["Sumb7Tol"] = "+ 0.0",
                ["Sumb7TolN"] = "- 0.500",

                ["Sumb8"] = "(b8) 6",
                ["Sumb8Tol"] = "+ 0.400",
                ["Sumb8TolN"] = "- 0.0",

                ["SumH"] = "(H) 36",
                ["SumHTol"] = "+ 0.500",
                ["SumHTolN"] = "- 0.0",

                ["SumS1"] = "(S1) 1",
                ["SumS1Tol"] = "+ 0.500",
                ["SumS1TolN"] = "+ 0.250"
            };
        }


        /// <summary>
        /// THREADS
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> CalculateThreads()
        {
            return new Dictionary<string, string>
            {
                ["SumG"] = "(G) M6",
                ["SumGa"] = "(G) M6",
                ["SumG1"] = "(G1) 15",
                ["SumG1Tol"] = "+ 0.500",
                ["SumG1TolN"] = "- 0.0",
                ["SumG2"] = "min.23",
                ["SumG3"] = "max.28"
            };
        }

      
        /// <summary>
        /// LENGTH (L)
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> CalculateLength()
        {
            return new Dictionary<string, string>
            {
                ["SumL"] = "(L) 41.5",
                ["SumLTol"] = "+ 0.500",
                ["SumLTolN"] = "- 0.0"
            };
        }

        /// <summary>
        /// RADII & CHAMFERS
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> CalculateChamfersAndRadii()
        {
            return new Dictionary<string, string>
            {
                ["SumR08"] = "R max: 0.8 (6x)",
                ["SumR05a"] = "max R0.5 (4x)",
                ["SumRa"] = "max R0.5",
                ["SumRa32"] = "3.2",
                ["SumRa32a"] = "3.2",
                ["SumF1"] = "1x45°",
                ["SumF2"] = "1.5x45°",
                ["SumF3"] = "1x45°",
                ["SumF4"] = "0.5x45° (2x)",
                ["SumCo1"] = "0.150"
            };
        }

        /// <summary>
        /// Page 1 Measurements
        /// </summary>
        /// <param name="measurement"></param>
        /// <returns></returns>
        private Dictionary<string, string> CalculatePage1Measurements(bool measurement)
        {
            return new Dictionary<string, string>
            {
                ["SumF1_1"] = measurement ? "1/5" : "",
                ["SumD1_1"] = measurement ? "Mätmaskin Alt.Skjutmått" : "",
                ["SumAF1_1"] = "",

                ["SumF1_2"] = measurement ? "1/5" : "",
                ["SumD1_2"] = measurement ? "Mätmaskin Alt.UD-Apparat" : "",
                ["SumAF1_2"] = "",

                ["SumF1_3"] = measurement ? "1/5" : "",
                ["SumD1_3"] = measurement ? "Mätmaskin Alt.Skjutmått" : "",
                ["SumAF1_3"] = "",

                ["SumF1_4"] = measurement ? "Inst. eller vid misstanke" : "",
                ["SumD1_4"] = measurement ? "Ytjämnhetsmätare" : "",
                ["SumAF1_4"] = measurement ? "Övriga bearbetade ytor 6.3" : "",

                ["SumF1_5"] = "",
                ["SumD1_5"] = "",
                ["SumAF1_5"] = "",

                ["SumF1_6"] = measurement ? "Inst." : "",
                ["SumD1_6"] = measurement ? "Mätmaskin Alt.Mätservice" : "",
                ["SumAF1_6"] = measurement ? "Vid misstänkt formfel, lämna till mätrum" : "",

                ["SumF1_7"] = measurement ? "Inst." : "",
                ["SumD1_7"] = measurement ? "Mätmaskin Alt.Skjutmått" : "",
                ["SumAF1_7"] = ""
            };
        }

        /// <summary>
        /// Page 2 Measurements
        /// </summary>
        /// <param name="measurement"></param>
        /// <returns></returns>
        private Dictionary<string, string> CalculatePage2Measurements(bool measurement)
        {
            return new Dictionary<string, string>
            {
                ["SumF2_1"] = measurement ? "1/5" : "",
                ["SumD2_1"] = measurement ? "Mätmaskin Alt.Skjutmått" : "",
                ["SumAF2_1"] = "",

                ["SumF2_2"] = measurement ? "1/5" : "",
                ["SumD2_2"] = measurement ? "Mätmaskin Alt.Skjutmått" : "",
                ["SumAF2_2"] = "",

                ["SumF2_3"] = measurement ? "1/5" : "",
                ["SumD2_3"] = measurement ? "Mätmaskin Alt.Skjutmått" : "",
                ["SumAF2_3"] = "",

                ["SumF2_4"] = measurement ? "1/5" : "",
                ["SumD2_4"] = measurement ? "Gängtolk" : "",
                ["SumAF2_4"] = "",

                ["SumF2_5"] = measurement ? "1/5" : "",
                ["SumD2_5"] = measurement ? "Mätmaskin Alt.Skjutmått/Mall" : "",
                ["SumAF2_5"] = "",

                ["SumF2_6"] = measurement ? "Inst. eller vid misstanke" : "",
                ["SumD2_6"] = measurement ? "Ytjämnhetsmätare" : "",
                ["SumAF2_6"] = measurement ? "Övriga bearbetade ytor 6.3" : "",

                ["SumF2_7"] = measurement ? "1/5" : "",
                ["SumD2_7"] = measurement ? "Mätmaskin Alt.Skjutmått" : "",
                ["SumAF2_7"] = ""
            };
        }

        /// <summary>
        /// ADMIN Fields
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> CalculateAdminFields()
        {
            return new Dictionary<string, string>
            {
                ["SumTextS1"] = "Okulärkontroll Grader, frifläckar, slagmärken, repor, valkar, ytjämnhet och faser, skarpa kanter avgradas.",
                ["SumTextS2"] = "Okulärkontroll Grader, frifläckar, slagmärken, repor, valkar, ytjämnhet och faser, skarpa kanter avgradas.",
                ["SumRitNr"] = "7433476: senaste utg. märkning: 7433523: senaste utg.",
                ["SumRitNr2"] = "7433476: senaste utg. märkning: 7433523: senaste utg."
            };
        }
    }
}