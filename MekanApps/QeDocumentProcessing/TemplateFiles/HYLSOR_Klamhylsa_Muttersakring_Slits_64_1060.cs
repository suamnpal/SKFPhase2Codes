using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Klamhylsa_Muttersakring_Slits_64_1060 : ITemplateCalculations
    {
        private static readonly string[] MachineValues = { "Skepp6", "K&T", "VTR-160", "MacTurn 550" };
        private static readonly string[] ValidTypes =
        {
            "64", "68", "72", "76", "80", "84", "88", "92", "96", "500", "530", "560",
            "600", "630", "670", "710", "750", "800", "850", "900", "950", "1000", "1060"
        };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            Dictionary<string, string> keyValues = new Dictionary<string, string>();
            double specialInnerDiameter = CommonFunctions.GetKeyValueDouble(req.Bookmarks, "Special innerdiameter (d1)");

            CalculateDimensions(keyValues, req.ProductDesignation, specialInnerDiameter);
            CalculateMachineValues(keyValues, req.MachineNumber);

            return keyValues;
        }

        private static void CalculateDimensions(
            Dictionary<string, string> keyValues,
            string productDesignation,
            double specialInnerDiameter)
        {
            string tmpBet = (productDesignation ?? string.Empty).ToUpperInvariant().Trim().Replace('.', ',');
            TmpBets bet = CommonFunctions.CalculateBets(tmpBet);
            bool hasSlash = tmpBet.Contains('/');
            bool tmpTecken = hasSlash && bet.TmpCounts > 9;

            string series;
            if (hasSlash && (bet.TmpCountB2 == 3 || bet.TmpCountB2 == 2))
                series = bet.TmpBet2;
            else if (bet.TmpCountB2 > 4)
                series = Left(bet.TmpBet2, 3);
            else if (bet.TmpCountB2 == 3)
                series = Left(bet.TmpBet2, 1);
            else
                series = Left(bet.TmpBet2, 2);

            string type = !hasSlash
                ? Right(bet.TmpBet2, 2)
                : bet.TmpCountB2 > 3
                    ? Right(bet.TmpBet2, 2)
                    : bet.TmpBet3;

            if (!int.TryParse(type, out int numericType))
                throw new ArgumentException($"Unsupported product designation: {productDesignation}", nameof(productDesignation));

            keyValues["SumRitNr"] = series switch
            {
                "30" => "7438957",
                "31" => "7438958",
                "32" => "7438955",
                "39" => "7434032",
                "241" => "7432903",
                "240" => "7432901",
                _ => "Fel Mall"
            };

            string angle = 11.25.ToString(CommonFunctions.Culture) + "º";
            keyValues["SumVa"] = angle;
            keyValues["SumV"] = angle;
            keyValues["SumV30"] = "30º";

            int slit = series == "32"
                ? numericType < 501 ? 8 : 10
                : series == "39"
                    ? numericType < 530 ? 8 : 10
                    : numericType < 501 ? 8 : 10;

            keyValues["SumC"] = $"(c) {slit}";
            keyValues["SumCTol"] = "± 0.2";

            double baseDiameter;
            if (bet.TmpCountB2 > 3)
                baseDiameter = numericType / 2.0 * 10.0;
            else if (!double.TryParse(bet.TmpBet3, System.Globalization.NumberStyles.Any, CommonFunctions.Culture, out baseDiameter))
                throw new ArgumentException($"Inner diameter could not be read from: {productDesignation}", nameof(productDesignation));

            string innerDiameter;
            if (specialInnerDiameter != 0)
            {
                innerDiameter = specialInnerDiameter.ToString(CommonFunctions.Culture);
            }
            else if (tmpTecken)
            {
                innerDiameter = bet.TmpNull4 ? bet.TmpBet3 : bet.TmpBet4;
            }
            else
            {
                double reduction = numericType < 85
                    ? 20
                    : numericType < 561 || numericType == 630
                        ? 30
                        : numericType < 751
                            ? 40
                            : numericType < 1001
                                ? 50
                                : 60;
                innerDiameter = (baseDiameter - reduction).ToString(CommonFunctions.Culture);
            }

            keyValues["Sumd1"] = $"(d1) {innerDiameter}";
            SetInnerDiameterTolerance(keyValues, numericType);

            int typeIndex = Array.IndexOf(ValidTypes, type);
            if (typeIndex < 0)
                typeIndex = 0;

            int[] fValues = series == "240"
                ? new[] { 0, 34, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 50, 0, 53, 0, 0, 0, 0, 60, 0, 0 }
                : series == "241"
                    ? new[] { 0, 35, 36, 0, 0, 36, 0, 0, 0, 42, 34, 0, 50, 50, 52, 53, 0, 0, 0, 0, 0, 0, 0 }
                    : new[] { 25, 26, 26, 26, 27, 27, 27, 28, 28, 28, 34, 34, 35, 35, 36, 38, 38, 39, 39, 40, 42, 44, 44 };

            int f = fValues[typeIndex];
            string fTolerance = f > 50 ? "+ 3.0" : f > 30 ? "+ 2.5" : f > 19 ? "+ 2.1" : "+ 1.8";
            keyValues["SumF"] = $"(f) {f}";
            keyValues["SumF2"] = $"(f) {f}";
            keyValues["SumFTol"] = fTolerance;
            keyValues["SumF2Tol"] = fTolerance;
            keyValues["SumFTolN"] = "  0 \\[3F]";
            keyValues["SumF2TolN"] = "  0 \\[3F]";

            int[] eValues = series == "240"
                ? new[] { 24, 24, 28, 32, 32, 32, 36, 36, 36, 40, 40, 45, 45, 50, 50, 55, 60, 60, 70, 70, 60, 70, 70 }
                : series == "31" || series == "32" || series == "241"
                    ? new[] { 24, 28, 28, 32, 32, 32, 36, 36, 36, 40, 40, 45, 45, 50, 50, 55, 60, 60, 70, 70, 70, 70, 70 }
                    : series == "30" || series == "39"
                        ? new[] { 24, 24, 28, 28, 28, 32, 32, 32, 36, 36, 40, 40, 40, 45, 45, 50, 55, 55, 60, 60, 60, 60, 60 }
                        : new int[23];

            int e = eValues[typeIndex];
            keyValues["SumE"] = $"(e) {e}";
            keyValues["SumETol"] = e > 50 ? "+ 0.740" : e > 30 ? "+ 0.620" : e > 18 ? "+ 0.520" : e > 10 ? "+ 0.430" : e > 6 ? "+ 0.360" : "+ 0.300";
            keyValues["SumETolN"] = "  0 \\[3F]";
        }

        private static void SetInnerDiameterTolerance(Dictionary<string, string> keyValues, int type)
        {
            if (type < 65)
            {
                keyValues["Sumd1Tol"] = "+ 0.210";
                keyValues["Sumd1TolN"] = "- 0.320";
            }
            else if (type < 85)
            {
                keyValues["Sumd1Tol"] = "+ 0.360";
                keyValues["Sumd1TolN"] = "- 0.570";
            }
            else if (type < 531)
            {
                keyValues["Sumd1Tol"] = "+ 0.400";
                keyValues["Sumd1TolN"] = "- 0.630";
            }
            else if (type < 671)
            {
                keyValues["Sumd1Tol"] = "+ 0.440";
                keyValues["Sumd1TolN"] = "- 0.700";
            }
            else if (type < 851)
            {
                keyValues["Sumd1Tol"] = "+ 0.500";
                keyValues["Sumd1TolN"] = "- 0.800";
            }
            else
            {
                keyValues["Sumd1Tol"] = "+ 0.560";
                keyValues["Sumd1TolN"] = "- 0.900";
            }
        }

        private static void CalculateMachineValues(Dictionary<string, string> keyValues, string machineNumber)
        {
            string machine = MachineValues.Contains(machineNumber) ? machineNumber : string.Empty;
            string frequency = machineNumber == "Skepp6" ? "1/1" : MachineValues.Contains(machineNumber) ? "1/2" : string.Empty;
            string gauge = MachineValues.Contains(machineNumber) ? "Skjutmått" : string.Empty;

            keyValues["SumMaskinValS1"] = $"Maskin: {machine} - Muttersäkring, Slits";

            for (int i = 1; i <= 5; i++)
                keyValues[$"SumF1_{i}"] = frequency;

            keyValues["SumD1_1"] = gauge;
            keyValues["SumD1_2"] = gauge;
            keyValues["SumD1_3"] = gauge;
            keyValues["SumD1_4"] = string.Empty;
            keyValues["SumD1_5"] = gauge;

            for (int i = 1; i <= 5; i++)
                keyValues[$"SumAF1_{i}"] = string.Empty;

            keyValues["SumTextS1"] = string.Empty;
        }

        private static string Left(string value, int length)
        {
            value ??= string.Empty;
            return value.Substring(0, Math.Min(length, value.Length));
        }

        private static string Right(string value, int length)
        {
            value ??= string.Empty;
            return value.Substring(Math.Max(0, value.Length - length));
        }
    }
}
