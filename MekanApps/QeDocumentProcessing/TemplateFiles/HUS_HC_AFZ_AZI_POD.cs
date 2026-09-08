using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HUS_HC_AFZ_AZI_POD : ITemplateCalculations
    {
        private static readonly string[] AllKeys = new[]
        {
            "SumD1", "SumD1Tol", "SumD1TolN",
            "SumD2", "SumD2Tol", "SumD2TolN",
            "SumD3", "SumD3Tol", "SumD3TolN",
            "SumD4", "SumD4Tol",
            "SumD5", "SumD5Tol", "SumD5TolN",
            "SumD8", "SumD8Tol",
            "SumD9", "SumD9Tol",
            "SumD10", "SumD10Tol",
            "SumD11", "SumD11Tol",
            "SumD12", "SumD12Tol",
            "SumD13", "SumD13Tol",
            "SumD14", "SumD14Tol",
            "SumD15", "SumD15Tol",
            "SumD16", "SumD16Tol",
            "SumD17", "SumD17Tol",
            "SumD18", "SumD18Tol",
            "SumD19", "SumD19Tol",
            "SumB", "SumBTol",
            "SumB1", "SumB1Tol",
            "SumB2", "SumB2Tol", "SumB2TolN",
            "SumB3", "SumB3Tol",
            "SumB5", "SumB5Tol",
            "SumB6", "SumB7", "SumB8",
            "SumL1", "SumL1Tol",
            "SumL2", "SumL2Tol",
            "SumL3", "SumL3Tol",
            "SumL4", "SumL4Tol",
            "SumL6", "SumL6Tol", "SumL6TolN",
            "SumL7", "SumL7Tol",
            "SumL8", "SumL8Tol",
            "SumL9", "SumL9Tol",
            "SumL10", "SumL10Tol",
            "SumL11", "SumL11Tol",
            "SumL12", "SumL12Tol",
            "SumL13", "SumL13Tol",
            "SumL14", "SumL14Tol",
            "SumE", "SumETol",
            "SumE1", "SumE1Tol",
            "SumG1", "SumG1b",
            "SumG2", "SumG2b",
            "SumG3", "SumG3b",
            "SumG4", "SumG4b",
            "SumG5", "SumG5b",
            "SumG6", "SumG6b",
            "SumG7", "SumG7b",
            "SumR3", "SumR4", "SumR5",
            "SumF1", "SumF2", "SumF3", "SumF4", "SumF5",
            "SumV2", "SumV3", "SumV4", "SumV5", "SumV6",
            "SumV7", "SumV8", "SumV9", "SumV10", "SumV11",
            "SumV12", "SumV13", "SumV14", "SumV15",
            "SumA", "SumA1", "SumA2",
            "SumC",
            "SumP", "SumP1", "SumP2", "SumP3",
            "SumRa16",
            "SumMaskinValS1", "SumMaskinValS2", "SumMaskinValS3", "SumMaskinValS4",
            "SumF1_1", "SumF1_2", "SumF1_3", "SumF1_4", "SumF1_5",
            "SumF1_6", "SumF1_7", "SumF1_8", "SumF1_9", "SumF1_0",
            "SumF2_1", "SumF2_2", "SumF2_3", "SumF2_4", "SumF2_5",
            "SumF2_6", "SumF2_7", "SumF2_8",
            "SumF3_1", "SumF3_2", "SumF3_3", "SumF3_4", "SumF3_5",
            "SumF4_1", "SumF4_2", "SumF4_3", "SumF4_4",
            "SumD1_1", "SumD1_2", "SumD1_3", "SumD1_4", "SumD1_5",
            "SumD1_6", "SumD1_7", "SumD1_8", "SumD1_9", "SumD1_0",
            "SumD2_1", "SumD2_2", "SumD2_3", "SumD2_4", "SumD2_5",
            "SumD2_6", "SumD2_7", "SumD2_8",
            "SumD3_1", "SumD3_2", "SumD3_3", "SumD3_4", "SumD3_5",
            "SumD4_1", "SumD4_2", "SumD4_3", "SumD4_4",
            "SumAF1_1", "SumAF1_2", "SumAF1_3", "SumAF1_4", "SumAF1_5",
            "SumAF1_6", "SumAF1_7", "SumAF1_8", "SumAF1_9", "SumAF1_0",
            "SumAF2_1", "SumAF2_2", "SumAF2_3", "SumAF2_4", "SumAF2_5",
            "SumAF2_6", "SumAF2_7", "SumAF2_8",
            "SumAF3_1", "SumAF3_2", "SumAF3_3", "SumAF3_4", "SumAF3_5",
            "SumAF4_1", "SumAF4_2", "SumAF4_3", "SumAF4_4",
            "SumSpTxt1", "SumSpTxt2", "SumSpTxt3", "SumSpTxt4",
            "SumTextS1", "SumTextS2", "SumTextS3", "SumTextS4",
            "SumRitS1", "SumRitS2", "SumRitS3", "SumRitS4",
            "VaLPopUp",
        };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();
            for (int i = 0; i < AllKeys.Length; i++) kv[AllKeys[i]] = "";

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");

            bool tmpHC = ContainsI(tmpBet, "HC");
            bool tmpAFZ = ContainsI(tmpBet, "AFZ");

            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : string.Empty;
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : string.Empty;
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : string.Empty;
            string tmpBet4 = tokens.Length > 3 ? tokens[3] : string.Empty;
            string tmpBet5 = tokens.Length > 4 ? tokens[4] : string.Empty;
            string tmpBet6 = tokens.Length > 5 ? tokens[5] : string.Empty;

            double tmpBet1Val = ParseDoubleSafe(tmpBet1);
            double tmpBet2Val = ParseDoubleSafe(tmpBet2);
            double tmpBet3Val = ParseDoubleSafe(tmpBet3);
            double tmpBet4Val = ParseDoubleSafe(tmpBet4);
            double tmpBet5Val = ParseDoubleSafe(tmpBet5);
            double tmpBet6Val = ParseDoubleSafe(tmpBet6);

            double tmpD1 = 420;
            kv["SumD1"] = "(D1) " + FormatDot(tmpD1);
            kv["SumD1Tol"] = "+ " + FormatDot3(G6TolPos(tmpD1));
            kv["SumD1TolN"] = "+ " + FormatDot3(G6TolNeg(tmpD1));

            double tmpD2 = 300;
            kv["SumD2"] = "(D2) " + FormatDot(tmpD2);
            kv["SumD2Tol"] = "+ " + FormatDot3(H8Tol(tmpD2));
            kv["SumD2TolN"] = "- " + "0";

            double tmpD3 = 420;
            kv["SumD3"] = "(D3) " + FormatDot(tmpD3);
            kv["SumD3Tol"] = "+ " + FormatDot3(G6TolPos(tmpD3));
            kv["SumD3TolN"] = "+ " + FormatDot3(G6TolNeg(tmpD3));

            double tmpD4 = 740;
            kv["SumD4"] = "(D4) " + FormatDot(tmpD4);
            kv["SumD4Tol"] = "\u00b1 " + FormatDot3(GeneralTolPM(tmpD4));

            double tmpD5 = 28.1;
            kv["SumD5"] = "(D5) " + FormatDot(tmpD5);
            kv["SumD5Tol"] = "+ " + FormatDot3(0.2);
            kv["SumD5TolN"] = "- " + "0";

            double tmpD8 = 30;
            kv["SumD8"] = "(D8) " + FormatDot(tmpD8);
            kv["SumD8Tol"] = "\u00b1 " + FormatDot3(GeneralTolPM(tmpD8));

            double tmpD9 = 19;
            kv["SumD9"] = "(D9) " + FormatDot(tmpD9);
            kv["SumD9Tol"] = "\u00b1 " + FormatDot3(GeneralTolPM(tmpD9));

            double tmpD10 = 20;
            kv["SumD10"] = "(D10) " + FormatDot(tmpD10);
            kv["SumD10Tol"] = "\u00b1 " + FormatDot3(GeneralTolPM(tmpD10));

            double tmpD11 = 335;
            kv["SumD11"] = "(D11) " + FormatDot(tmpD11);
            kv["SumD11Tol"] = "\u00b1 " + FormatDot3(GeneralTolPM(tmpD11));

            double tmpD12 = 500;
            kv["SumD12"] = "(D12) " + FormatDot(tmpD12);
            kv["SumD12Tol"] = "\u00b1 " + FormatDot3(GeneralTolPM(tmpD12));

            double tmpD13 = 374.9;
            kv["SumD13"] = "(D13) " + FormatDot(tmpD13);
            kv["SumD13Tol"] = "\u00b1 " + FormatDot3(GeneralTolPM(tmpD13));

            double tmpD14 = 9;
            kv["SumD14"] = "(D14) " + FormatDot(tmpD14);
            kv["SumD14Tol"] = "\u00b1 " + FormatDot3(GeneralTolPM(tmpD14));

            double tmpD15 = 9;
            kv["SumD15"] = "(D15) " + FormatDot(tmpD15);
            kv["SumD15Tol"] = "\u00b1 " + FormatDot3(GeneralTolPM(tmpD15));

            double tmpD16 = 30;
            kv["SumD16"] = "(D16) " + FormatDot(tmpD16);
            kv["SumD16Tol"] = "\u00b1 " + FormatDot3(GeneralTolPM(tmpD16));

            double tmpD17 = 357;
            kv["SumD17"] = "(D17) " + FormatDot(tmpD17);
            kv["SumD17Tol"] = "\u00b1 " + FormatDot3(GeneralTolPM(tmpD17));

            double tmpD18 = 500;
            kv["SumD18"] = "(D18) " + FormatDot(tmpD18);
            kv["SumD18Tol"] = "\u00b1 " + FormatDot3(GeneralTolPM(tmpD18));

            double tmpD19 = 680;
            kv["SumD19"] = "(D19) " + FormatDot(tmpD19);
            kv["SumD19Tol"] = "\u00b1 " + FormatDot3(GeneralTolPM(tmpD19));

            double tmpB = 359;
            kv["SumB"] = "(B) " + FormatDot(tmpB);
            kv["SumBTol"] = "\u00b1 " + FormatDot3(0.25);

            double tmpB1 = 88;
            kv["SumB1"] = "(B1) " + FormatDot(tmpB1);
            kv["SumB1Tol"] = "\u00b1 " + FormatDot3(GeneralTolPM(tmpB1));

            double tmpB2 = 279;
            kv["SumB2"] = "(B2) " + FormatDot(tmpB2);
            kv["SumB2Tol"] = "- " + FormatDot3(0.3);
            kv["SumB2TolN"] = "- " + FormatDot3(0.4);

            double tmpB3 = 34.4;
            kv["SumB3"] = "(B3) " + FormatDot(tmpB3);
            kv["SumB3Tol"] = "\u00b1 " + FormatDot3(0.2);

            double tmpB5 = 14;
            kv["SumB5"] = "(B5) " + FormatDot(tmpB5);
            kv["SumB5Tol"] = "\u00b1 " + FormatDot3(0.2);

            kv["SumB6"] = "max: " + FormatDot(60.0);
            kv["SumB7"] = "min: " + FormatDot(50.4);
            kv["SumB8"] = "min: " + FormatDot(13.0);

            double tmpL1 = 53.5;
            string tmpL1TolStr = FormatDot3(GeneralTolPM(tmpL1));
            kv["SumL1"] = "(L1) " + FormatDot(tmpL1);
            kv["SumL1Tol"] = "\u00b1 " + tmpL1TolStr;

            double tmpL2 = 42;
            kv["SumL2"] = "(L2) " + FormatDot(tmpL2);
            kv["SumL2Tol"] = "\u00b1 " + FormatDot3(GeneralTolPM(tmpL2));

            double tmpL3 = 38.5;
            kv["SumL3"] = "(L3) " + FormatDot(tmpL3);
            kv["SumL3Tol"] = "\u00b1 " + FormatDot3(GeneralTolPM(tmpL3));

            double tmpL4 = 30;
            kv["SumL4"] = "(L4) " + FormatDot(tmpL4);
            kv["SumL4Tol"] = "\u00b1 " + FormatDot3(GeneralTolPM(tmpL4));

            double tmpL6 = 154;
            kv["SumL6"] = "(L6) " + FormatDot(tmpL6);
            kv["SumL6Tol"] = "+ " + FormatDot1(1.0);
            kv["SumL6TolN"] = "- " + FormatDot3(0.0);

            double tmpL7 = 44;
            kv["SumL7"] = "(L7) " + FormatDot(tmpL7);
            kv["SumL7Tol"] = "\u00b1 " + tmpL1TolStr;

            double tmpL8 = 61.5;
            kv["SumL8"] = "(L8) " + FormatDot(tmpL8);
            kv["SumL8Tol"] = "\u00b1 " + tmpL1TolStr;

            double tmpL9 = 50;
            kv["SumL9"] = "(L9) " + FormatDot(tmpL9);
            kv["SumL9Tol"] = "\u00b1 " + tmpL1TolStr;

            double tmpL10 = 19.5;
            kv["SumL10"] = "(L10) " + FormatDot(tmpL10);
            kv["SumL10Tol"] = "\u00b1 " + tmpL1TolStr;

            double tmpL11 = 13;
            kv["SumL11"] = "(L11) " + FormatDot(tmpL11);
            kv["SumL11Tol"] = "\u00b1 " + tmpL1TolStr;

            double tmpL12 = 110;
            kv["SumL12"] = "(L12) " + FormatDot(tmpL12);
            kv["SumL12Tol"] = "\u00b1 " + tmpL1TolStr;

            double tmpL13 = 300;
            kv["SumL13"] = "(L13) " + FormatDot(tmpL13);
            kv["SumL13Tol"] = "\u00b1 " + tmpL1TolStr;

            double tmpL14 = 19.5;
            kv["SumL14"] = "(L14) " + FormatDot(tmpL14);
            kv["SumL14Tol"] = "\u00b1 " + tmpL1TolStr;

            double tmpE = 1;
            kv["SumE"] = "(E) " + FormatDot(tmpE);
            kv["SumETol"] = "\u00b1 " + tmpL1TolStr;

            double tmpE1 = 63;
            kv["SumE1"] = "(E1) " + FormatDot(tmpE1);
            kv["SumE1Tol"] = "\u00b1 " + tmpL1TolStr;

            double tmpG1 = 24;
            kv["SumG1"] = "M" + FormatDot(tmpG1);
            kv["SumG1b"] = kv["SumG1"];

            double tmpG2 = 8;
            kv["SumG2"] = "M" + FormatDot(tmpG2) + "-6H";
            kv["SumG2b"] = kv["SumG2"];

            double tmpG3 = 12;
            kv["SumG3"] = "M" + FormatDot(tmpG3) + " (6x)";
            kv["SumG3b"] = kv["SumG3"];

            double tmpG4 = 20;
            kv["SumG4"] = "M" + FormatDot(tmpG4) + " (5x)";
            kv["SumG4b"] = kv["SumG4"];

            double tmpG5 = 20;
            kv["SumG5"] = "M" + FormatDot(tmpG5) + "-6H";
            kv["SumG5b"] = kv["SumG5"];

            kv["SumG6"] = "G1/2";
            kv["SumG6b"] = kv["SumG6"];

            double tmpG7 = 8;
            kv["SumG7"] = "M" + FormatDot(tmpG7) + "-6H";
            kv["SumG7b"] = kv["SumG7"];

            kv["SumR3"] = "R" + FormatDot(360.0);
            kv["SumR4"] = "R" + FormatDot(240.0);
            kv["SumR5"] = "R" + FormatDot(255.0) + " (2x)";

            kv["SumF1"] = FormatDot1(0.5) + "x45\u00b0";
            kv["SumF2"] = FormatDot(1.0) + "x45\u00b0";
            kv["SumF3"] = FormatDot(1.0) + "x45\u00b0";
            kv["SumF4"] = FormatDot(1.0) + "x45\u00b0";
            kv["SumF5"] = FormatDot(1.0) + "x45\u00b0";

            kv["SumV2"] = FormatDot(45.0) + "\u00b0";
            kv["SumV3"] = FormatDot(60.0) + "\u00b0 (6x)";
            kv["SumV4"] = FormatDot(18.0) + "\u00b0 (2x)";
            kv["SumV5"] = FormatDot(45.0) + "\u00b0";
            kv["SumV6"] = FormatDot(45.0) + "\u00b0";
            kv["SumV7"] = FormatDot(30.0) + "\u00b0 (4x)";
            kv["SumV8"] = FormatDot(11.25) + "\u00b0";
            kv["SumV9"] = FormatDot(11.25) + "\u00b0";
            kv["SumV10"] = FormatDot(20.0) + "\u00b0";
            kv["SumV11"] = FormatDot(13.75) + "\u00b0";
            kv["SumV12"] = FormatDot(11.25) + "\u00b0";
            kv["SumV13"] = FormatDot(22.5) + "\u00b0 (14x)";
            kv["SumV14"] = FormatDot(22.5) + "\u00b0 (12x)";
            kv["SumV15"] = FormatDot(22.5) + "\u00b0 (16x)";

            kv["SumA"] = FormatDot3(0.1);
            kv["SumA1"] = FormatDot1(1.0);
            kv["SumA2"] = FormatDot3(0.2);
            kv["SumC"] = FormatDot3(0.03);
            kv["SumP"] = FormatDot3(0.1);
            kv["SumP1"] = "Ø 0.10";
            kv["SumP2"] = "Ø 0.10";
            kv["SumP3"] = "Ø 0.10";

            kv["SumRa16"] = FormatDot1(1.6);

            bool isTrevisan = EqualsI(maskinVal, "Trevisan DS 900");

            kv["SumMaskinValS1"] = (isTrevisan ? "Trevisan DS 900" : "") + " - Svarvning/Borrning";
            kv["SumMaskinValS2"] = (isTrevisan ? "Trevisan DS 900" : "") + " - Borrning, fr\u00e4sning";
            kv["SumMaskinvalS3"] = "Trevisan DS 900 - Borrning";
            kv["SumMaskinvalS4"] = "Trevisan DS 900 - Borrning";

            kv["SumF1_1"] = "";
            kv["SumF1_2"] = isTrevisan ? "1/1" : "";
            kv["SumF1_3"] = isTrevisan ? "1/1" : "";
            kv["SumF1_4"] = isTrevisan ? "1/1" : "";
            kv["SumF1_5"] = isTrevisan ? "1/1" : "";
            kv["SumF1_6"] = "";
            kv["SumF1_7"] = "";
            kv["SumF1_8"] = isTrevisan ? "1/1" : "";
            kv["SumF1_9"] = isTrevisan ? "1/1" : "";
            kv["SumF1_0"] = isTrevisan ? "inst." : "";

            kv["SumF2_1"] = isTrevisan ? "inst." : "";
            kv["SumF2_2"] = isTrevisan ? "inst." : "";
            kv["SumF2_3"] = isTrevisan ? "inst." : "";
            kv["SumF2_4"] = isTrevisan ? "inst." : "";
            kv["SumF2_5"] = isTrevisan ? "inst." : "";
            kv["SumF2_6"] = isTrevisan ? "inst." : "";
            kv["SumF2_7"] = isTrevisan ? "inst." : "";
            kv["SumF2_8"] = "";

            kv["SumF3_1"] = isTrevisan ? "inst." : "";
            kv["SumF3_2"] = "";
            kv["SumF3_3"] = isTrevisan ? "inst." : "";
            kv["SumF3_4"] = "";
            kv["SumF3_5"] = isTrevisan ? "inst." : "";

            kv["SumF4_1"] = isTrevisan ? "inst." : "";
            kv["SumF4_2"] = "";
            kv["SumF4_3"] = "";
            kv["SumF4_4"] = isTrevisan ? "inst." : "";

            kv["SumD1_1"] = "";
            kv["SumD1_2"] = isTrevisan ? "Subito" : "";
            kv["SumD1_3"] = isTrevisan ? "Subito" : "";
            kv["SumD1_4"] = isTrevisan ? "inv. mikrometer" : "";
            kv["SumD1_5"] = isTrevisan ? "H\u00f6jdstativ" : "";
            kv["SumD1_6"] = "";
            kv["SumD1_7"] = "";
            kv["SumD1_8"] = isTrevisan ? "M\u00e4tmaskin" : "";
            kv["SumD1_9"] = isTrevisan ? "M\u00e4tmaskin" : "";
            kv["SumD1_0"] = isTrevisan ? "Skjutm\u00e5tt" : "";

            kv["SumD2_1"] = isTrevisan ? "G\u00e4ngtolk min/max" : "";
            kv["SumD2_2"] = isTrevisan ? "G\u00e4ngtolk min/max" : "";
            kv["SumD2_3"] = isTrevisan ? "G\u00e4ngtolk min/max" : "";
            kv["SumD2_4"] = isTrevisan ? "G\u00e4ngtolk min/max" : "";
            kv["SumD2_5"] = isTrevisan ? "G\u00e4ngtolk min/max" : "";
            kv["SumD2_6"] = isTrevisan ? "G\u00e4ngtolk min/max" : "";
            kv["SumD2_7"] = isTrevisan ? "Skjutm\u00e5tt/Tolk" : "";
            kv["SumD2_8"] = "";

            kv["SumD3_1"] = isTrevisan ? "G\u00e4ngtolk min/max" : "";
            kv["SumD3_2"] = "";
            kv["SumD3_3"] = isTrevisan ? "Skjutm\u00e5tt/Tolk" : "";
            kv["SumD3_4"] = "";
            kv["SumD3_5"] = isTrevisan ? "M\u00e4tmaskin" : "";

            kv["SumD4_1"] = isTrevisan ? "M\u00e4tmaskin" : "";
            kv["SumD4_2"] = "";
            kv["SumD4_3"] = "";
            kv["SumD4_4"] = isTrevisan ? "M\u00e4tmaskin" : "";

            kv["SumAF1_1"] = "";
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = isTrevisan ? "Dokumenteras" : "";
            kv["SumAF1_6"] = "";
            kv["SumAF1_7"] = "";
            kv["SumAF1_8"] = "";
            kv["SumAF1_9"] = "";
            kv["SumAF1_0"] = "";

            kv["SumAF2_1"] = "";
            kv["SumAF2_2"] = "";
            kv["SumAF2_3"] = "";
            kv["SumAF2_4"] = "";
            kv["SumAF2_5"] = "";
            kv["SumAF2_6"] = "";
            kv["SumAF2_7"] = "";
            kv["SumAF2_8"] = "";

            kv["SumAF3_1"] = "";
            kv["SumAF3_2"] = "";
            kv["SumAF3_3"] = "";
            kv["SumAF3_4"] = "";
            kv["SumAF3_5"] = "";

            kv["SumAF4_1"] = "";
            kv["SumAF4_2"] = "";
            kv["SumAF4_3"] = "";
            kv["SumAF4_4"] = "";

            string tmpSpTxt = "Kontrolleras enl. styrplan";
            kv["SumSpTxt1"] = tmpSpTxt;
            kv["SumSpTxt2"] = tmpSpTxt;
            kv["SumSpTxt3"] = tmpSpTxt;
            kv["SumSpTxt4"] = tmpSpTxt;

            string sumTextS1 = "Okulärkontroll gjuteridefekter, grader & slagmärken";
            kv["SumTextS1"] = sumTextS1;
            kv["SumTextS2"] = sumTextS1;
            kv["SumTextS3"] = sumTextS1;
            kv["SumTextS4"] = sumTextS1;

            string sumRitS1 = subject + ":senaste utg\u00e5va";
            kv["SumRitS1"] = sumRitS1;
            kv["SumRitS2"] = sumRitS1;
            kv["SumRitS3"] = sumRitS1;
            kv["SumRitS4"] = sumRitS1;

            kv["TmpRa16"] = "1,6";

            return kv;
        }

        private static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published)) return "";
            DateTime pubDt;
            if (!DateTime.TryParse(published, out pubDt)) return "";
            int dagar = 14;
            DateTime validTill = pubDt.AddDays(dagar);
            if (DateTime.Today <= validTill.Date)
                return "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom " +
                       dagar.ToString(CultureInfo.InvariantCulture) +
                       " dagar)\n\nInformation om senaste \u00e4ndring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument\n\nPopupruta aktiv till " +
                       validTill.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return "";
        }

        private static double G6TolPos(double d)
        {
            if (d < 3.01) return 0.008;
            if (d < 6.01) return 0.012;
            if (d < 10.01) return 0.014;
            if (d < 18.01) return 0.017;
            if (d < 30.01) return 0.020;
            if (d < 50.01) return 0.025;
            if (d < 80.01) return 0.029;
            if (d < 120.01) return 0.034;
            if (d < 180.01) return 0.039;
            if (d < 250.01) return 0.044;
            if (d < 315.01) return 0.049;
            if (d < 400.01) return 0.054;
            if (d < 500.01) return 0.060;
            if (d < 630.01) return 0.066;
            if (d < 800.01) return 0.074;
            if (d < 1000.01) return 0.082;
            if (d < 1250.01) return 0.094;
            if (d < 1600.01) return 0.108;
            if (d < 2000.01) return 0.124;
            if (d < 2500.01) return 0.144;
            return 0.173;
        }

        private static double G6TolNeg(double d)
        {
            if (d < 3.01) return 0.002;
            if (d < 6.01) return 0.004;
            if (d < 10.01) return 0.005;
            if (d < 18.01) return 0.006;
            if (d < 30.01) return 0.007;
            if (d < 50.01) return 0.009;
            if (d < 80.01) return 0.010;
            if (d < 120.01) return 0.012;
            if (d < 180.01) return 0.014;
            if (d < 250.01) return 0.015;
            if (d < 315.01) return 0.017;
            if (d < 400.01) return 0.018;
            if (d < 500.01) return 0.020;
            if (d < 630.01) return 0.022;
            if (d < 800.01) return 0.024;
            if (d < 1000.01) return 0.026;
            if (d < 1250.01) return 0.028;
            if (d < 1600.01) return 0.030;
            if (d < 2000.01) return 0.032;
            if (d < 2500.01) return 0.034;
            return 0.038;
        }

        private static double H8Tol(double d)
        {
            if (d < 3.01) return 0.014;
            if (d < 6.01) return 0.018;
            if (d < 10.01) return 0.022;
            if (d < 18.01) return 0.027;
            if (d < 30.01) return 0.033;
            if (d < 50.01) return 0.039;
            if (d < 80.01) return 0.046;
            if (d < 120.01) return 0.054;
            if (d < 180.01) return 0.063;
            if (d < 250.01) return 0.072;
            if (d < 315.01) return 0.081;
            if (d < 400.01) return 0.089;
            if (d < 500.01) return 0.097;
            if (d < 630.01) return 0.110;
            if (d < 800.01) return 0.125;
            if (d < 1000.01) return 0.140;
            if (d < 1250.01) return 0.165;
            if (d < 1600.01) return 0.195;
            if (d < 2000.01) return 0.230;
            if (d < 2500.01) return 0.280;
            return 0.330;
        }

        private static double GeneralTolPM(double v)
        {
            if (v < 6.01) return 0.1;
            if (v < 30.01) return 0.2;
            if (v < 120.01) return 0.3;
            if (v < 315.01) return 0.5;
            if (v < 1000.01) return 0.8;
            if (v < 2000.01) return 1.2;
            return 2.0;
        }

        private static bool EqualsI(string a, string b)
            => string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);

        private static bool ContainsI(string haystack, string needle)
        {
            if (string.IsNullOrEmpty(haystack) || string.IsNullOrEmpty(needle)) return false;
            return haystack.IndexOf(needle, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static double ParseDoubleSafe(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;
            string t = s.Trim().Replace(",", ".");
            double v;
            return double.TryParse(t, NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static string FormatDot(double v)
        {
            return v.ToString(CommonFunctions.Culture).Replace(",", ".");
        }

        private static string FormatDot3(double v)
        {
            return v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
        }

        private static string FormatDot1(double v)
        {
            return v.ToString("F1", CommonFunctions.Culture).Replace(",", ".");
        }
    }
}