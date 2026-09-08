using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class RINGAR_RJ_OP1_3 : ITemplateCalculations
    {
        private const string LB = "<<LineBreak>>";

        private static readonly string[] TypList =
            { "12", "15", "16", "17", "18", "19", "20", "22", "24", "26", "28", "30", "32", "34", "36", "38", "40", "44", "48" };

        private static readonly string[] A30S222 = { "0", "8,767" };
        private static readonly string[] A30List =
            { "13,53", "13,78", "13,67", "12,97", "13,28", "13,2", "13,2", "13,03", "13,65", "13,67", "13,73", "13,58", "13,58", "13,53", "13,71", "13,6", "13,52", "12,7", "12,47" };
        private static readonly string[] A30VXList =
            { "12", "15", "16", "17", "18", "19", "20", "22", "24", "26", "13,818", "13,72", "32", "34", "13,783", "38", "40", "44", "48" };

        private static readonly string[] D01S222 = { "0", "104,28" };
        private static readonly string[] D01List =
            { "102,34", "126,53", "133,62", "143,52", "151,23", "158,08", "172,6", "190,99", "208,02", "224,92", "238,32", "257,28", "272,58", "290,38", "305,38", "321,16", "337,36", "370,3", "402,57" };

        private static readonly string[] D10S222 = { "0", "118,84" };
        private static readonly string[] D10List =
            { "118,83", "144,91", "153,29", "164,29", "172,28", "180,56", "197,52", "218,12", "231,7", "249,75", "266,7", "286,7", "303,6", "322,65", "340,65", "357,65", "376,6", "414,3", "449,25" };

        private static readonly string[] L01S222 = { "0", "5,65" };
        private static readonly string[] L01List =
            { "8,806", "11,355", "11,654", "11,511", "12,438", "12,829", "12,522", "15,48", "18,72", "19,751", "20,752", "21,946", "23,74", "25,549", "27,529", "28,83", "30,518", "31,472", "33,838" };
        private static readonly string[] L01VXList =
            { "12", "15", "16", "17", "18", "19", "20", "22", "24", "26", "18,802", "19,46", "32", "34", "23,643", "38", "40", "44", "48" };

        private static readonly string[] L02S222 = { "0", "5,2" };
        private static readonly string[] L02List =
            { "8,1", "10,5", "10,7", "10,6", "11,4", "11,8", "11,4", "14,2", "16,2", "17,3", "17,6", "18,6", "19,9", "21,8", "23,3", "24,4", "25,8", "25,585", "27,875" };
        private static readonly string[] L02VXList =
            { "12", "15", "16", "17", "18", "19", "20", "22", "24", "26", "14,695", "15,695", "32", "34", "18,895", "38", "40", "44", "48" };

        private static readonly string[] R15S222 = { "0", "56,53" };
        private static readonly string[] R15List =
            { "56,4", "68,8", "72,7", "77,5", "81,5", "85,4", "92,9", "103,5", "111,4", "120,3", "128,3", "138", "146,5", "155,7", "164,5", "172,9", "182,1", "201,9", "218,6" };
        private static readonly string[] R15VXList =
            { "12", "15", "16", "17", "18", "19", "20", "22", "24", "26", "28", "137,6", "32", "34", "164,5", "38", "40", "44", "48" };

        private static readonly string[] L19S222 = { "0", "114,85" };
        private static readonly string[] L19List =
            { "109,75", "134,25", "141,6", "152,35", "160,2", "167,5", "183,4", "202,1", "215,7", "232,6", "248,0", "267,2", "283,0", "301,1", "317,1", "333,2", "351,0", "387,0", "422,0" };
        private static readonly string[] L19VXList =
            { "12", "15", "16", "17", "18", "19", "20", "22", "24", "26", "28", "267", "32", "34", "318", "38", "40", "44", "48" };

        private static readonly string[] L18List =
            { "12", "20,25", "22,04", "23,97", "23,98", "24,81", "28,12", "30,52", "33,84", "36,38", "39,24", "41,57", "43,97", "46,38", "49,78", "51,99", "54,59", "59,16", "61,63" };
        private static readonly string[] L18VXList =
            { "12", "15", "16", "17", "18", "19", "20", "22", "24", "26", "28", "41,78", "32", "34", "49,78", "38", "40", "44", "48" };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string mv = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpBet = subject.ToUpperInvariant().Trim();
            bool tmpVX216 = tmpBet.IndexOf("VX216", StringComparison.Ordinal) >= 0;

            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : "";
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : "";

            string tmpSerie = Left(tmpBet2, 3);
            string tmpTyp = Right(tmpBet2, 2);

            bool serie222 = TryParseDouble(tmpSerie) == 222;
            double typNum = TryParseDouble(tmpTyp);
            int idx = Member(tmpTyp, TypList);

            bool betC = EqualsI(tmpBet3, "C");
            bool betE = EqualsI(tmpBet3, "E");

            string tmpA30 = Word(tmpVX216 ? A30VXList : (serie222 ? A30S222 : A30List), idx).Replace(",", ".");
            kv["SumA30"] = "(A30) " + tmpA30 + "\u00ba";

            kv["SumA30Tol"] = In(typNum, 12, 15) ? "+ 0.032"
                            : In(typNum, 16, 17, 18, 24, 26, 28) ? "+ 0.040"
                            : In(typNum, 19) ? "+ 0.048"
                            : In(typNum, 20, 22) ? "+ 0.056"
                            : In(typNum, 30, 32, 34) ? "+ 0.042"
                            : In(typNum, 36, 38, 40) ? "+ 0.049"
                            : In(typNum, 44, 48) ? "+ 0.035"
                            : "";

            kv["SumA30TolN"] = In(typNum, 12, 15) ? "- 0.008"
                             : In(typNum, 16, 17, 18, 24, 26, 28) ? "- 0.010"
                             : In(typNum, 19) ? "- 0.012"
                             : In(typNum, 20, 22) ? "- 0.014"
                             : In(typNum, 30, 32, 34) ? "- 0.006"
                             : In(typNum, 36, 38, 40, 44, 48) ? "- 0.007"
                             : "";

            string tmpML = (In(typNum, 12, 15) ? "4"
                          : In(typNum, 16, 17, 18, 24, 26, 28) ? "5"
                          : In(typNum, 19, 30, 32, 34) ? "6"
                          : In(typNum, 20, 22, 36, 38, 40, 44, 48) ? "7"
                          : "Fel Typ") + " mm";

            string tmpND01Str = Word(serie222 ? D01S222 : D01List, idx);
            double tmpND01 = TryParseDouble(tmpND01Str);

            double tmpND01Tol = tmpND01 < 180.01 ? 0.18
                              : tmpND01 < 250.01 ? 0.25
                              : tmpND01 < 315.01 ? 0.31
                              : tmpND01 < 400.01 ? 0.34
                              : 0.46;
            kv["SumND01Tol"] = "+ " + FmtGenDot(tmpND01Tol);

            double tmpND01TolN = tmpND01 < 180.01 ? 0.08
                               : tmpND01 < 250.01 ? 0.135
                               : tmpND01 < 315.01 ? 0.18
                               : tmpND01 < 400.01 ? 0.2
                               : 0.31;
            kv["SumND01TolN"] = "+ " + FmtGenDot(tmpND01TolN);

            double tmpD01 = Math.Round(tmpND01 + tmpND01TolN, 3, MidpointRounding.AwayFromZero);
            kv["SumD01"] = "(D01) " + FmtComma(tmpD01);
            kv["SumD01SK"] = kv["SumD01"];

            string tmpKompD01 = tmpD01 < 226 ? "0.01-0.06" : "0-0.05";

            kv["SumD01Tol"] = tmpD01 < 173 ? "+ 0.040"
                            : tmpD01 < 226 ? "+ 0.055"
                            : tmpD01 < 239 ? "+ 0.065"
                            : tmpD01 < 306 ? "+ 0.080"
                            : tmpD01 < 371 ? "+ 0.090"
                            : "+ 0.100";

            kv["SumD01TolN"] = tmpD01 < 173 ? "- 0.010"
                             : tmpD01 < 226 ? "- 0.010"
                             : "- 0";

            double tmpUtRD01Tol = Math.Round(tmpND01Tol - tmpND01TolN, 3, MidpointRounding.AwayFromZero);
            kv["SumD01SKTol"] = "+ " + FmtGenDot(tmpUtRD01Tol);
            kv["SumD01SKTolN"] = "- 0";

            string tmpND10Str = Word(serie222 ? D10S222 : D10List, idx);
            double tmpND10 = TryParseDouble(tmpND10Str);

            kv["SumND10Tol"] = "+ 0";

            double tmpND10TolN = tmpND10 < 180.01 ? 0.1
                               : tmpND10 < 250.01 ? 0.115
                               : tmpND10 < 315.01 ? 0.13
                               : tmpND10 < 400.01 ? 0.14
                               : 0.15;
            kv["SumND10TolN"] = "- " + FmtGenDot(tmpND10TolN);

            double tmpD10 = Math.Round(tmpND10 - 0.1, 3, MidpointRounding.AwayFromZero);
            kv["SumD10"] = "(D10) " + FmtComma(tmpD10);
            kv["SumD10SK"] = "(D10) " + tmpND10Str;

            string tmpKompD10 = tmpD10 < 180.01 ? "0.07-0.12"
                              : tmpD10 < 219 ? "0.04-0.09"
                              : tmpD10 < 232 ? "0.07-0.12"
                              : tmpD10 < 250 ? "0.06-0.12"
                              : tmpD10 < 377 ? "0.05-0.11"
                              : "0.04-0.11";

            kv["SumD10Tol"] = In(typNum, 20, 22) ? "+ 0.010"
                            : tmpD10 < 181 ? "- 0.020"
                            : tmpD10 < 219 ? "- 0.010"
                            : tmpD10 < 250 ? "- 0.020"
                            : "- 0.010";

            kv["SumD10TolN"] = tmpD10 < 173 ? "- 0.070"
                             : tmpD10 < 181 ? "- 0.085"
                             : tmpD10 < 219 ? "- 0.055"
                             : tmpD10 < 232 ? "- 0.085"
                             : tmpD10 < 250 ? "- 0.075"
                             : tmpD10 < 304 ? "- 0.080"
                             : "- 0.090";

            kv["SumD10SKTol"] = kv["SumND10Tol"];
            kv["SumD10SKTolN"] = kv["SumND10TolN"];

            string tmpL01 = Word(tmpVX216 ? L01VXList : (serie222 ? L01S222 : L01List), idx).Replace(",", ".");
            kv["SumL01"] = "(L01) " + tmpL01;

            kv["SumL01Tol"] = tmpD01 < 180.01 ? "- 0.080"
                            : tmpD01 < 250.01 ? "- 0.100"
                            : tmpD01 < 315.01 ? "- 0.120"
                            : tmpD01 < 400.01 ? "- 0.130"
                            : "- 0.150";

            kv["SumL01TolN"] = tmpD01 < 180.01 ? "- 0.180"
                             : tmpD01 < 250.01 ? "- 0.215"
                             : tmpD01 < 315.01 ? "- 0.250"
                             : tmpD01 < 400.01 ? "- 0.270"
                             : "- 0.305";

            string tmpL02 = Word(tmpVX216 ? L02VXList : (serie222 ? L02S222 : L02List), idx).Replace(",", ".");
            kv["SumL02"] = "(L02) " + tmpL02;

            kv["SumL02Tol"] = typNum == 44 ? "\u00b1 0.115"
                            : typNum == 48 ? "\u00b1 0.125"
                            : tmpD01 < 180.01 ? "\u00b1 0.080"
                            : tmpD01 < 250.01 ? "\u00b1 0.092"
                            : tmpD01 < 315.01 ? "\u00b1 0.105"
                            : tmpD01 < 400.01 ? "\u00b1 0.115"
                            : "\u00b1 0.125";

            string tmpR15 = Word(tmpVX216 ? R15VXList : (serie222 ? R15S222 : R15List), idx).Replace(",", ".");
            kv["SumR15"] = "(R15)" + LB + Environment.NewLine + " " + tmpR15;
            kv["SumR15Tol"] = "+ 0";
            kv["SumR15TolN"] = typNum == 48 ? "- 3"
                             : tmpND10 < 180.01 ? "- 4"
                             : tmpND10 < 250.01 ? "- 5"
                             : tmpND10 < 315.01 ? "- 6"
                             : "- 7";

            kv["SumR10"] = (typNum < 27 || In(typNum, 44, 48)) ? "R 0.4" : "R 0.6";

            kv["SumV3dg"] = tmpND01 < 180.01 ? "0.065"
                          : tmpND01 < 250.01 ? "0.070"
                          : tmpND01 < 315.01 ? "0.080"
                          : tmpND01 < 400.01 ? "0.090"
                          : "0.100";

            string tmpAdg = tmpND10 < 180.01 ? "0.043"
                          : tmpND10 < 250.01 ? "0.047"
                          : tmpND10 < 315.01 ? "0.054"
                          : tmpND10 < 400.01 ? "0.064"
                          : "0.075";
            kv["SumAdg"] = "(Adg)";

            string tmpEg = tmpND10 < 181 ? "0.20"
                         : tmpND10 < 251 ? "0.22"
                         : tmpND10 < 316 ? "0.24"
                         : tmpND10 < 401 ? "0.26"
                         : "0.26";

            string tmpL19 = Word(tmpVX216 ? L19VXList : (serie222 ? L19S222 : L19List), idx).Replace(",", ".");
            kv["SumL19"] = "(L19) " + tmpL19;
            kv["SumL19SK"] = kv["SumL19"];

            string tmpIL19Tol = typNum == 48 ? "- 0.260" : "- 0.200";

            kv["SumL19Tol"] = typNum < 49 ? "- 0.100" : "- 0";
            kv["SumL19TolN"] = typNum < 20 ? "- 0.250"
                             : typNum < 48 ? "- 0.300"
                             : "- 0.315";
            kv["SumL19SKTol"] = "+ 0";
            kv["SumL19SKTolN"] = tmpD10 < 180.01 ? "- 0.200"
                               : tmpD10 < 250.01 ? "- 0.230"
                               : tmpD10 < 315.01 ? "- 0.260"
                               : tmpD10 < 400.01 ? "- 0.285"
                               : "- 0.315";

            string tmpL18 = Word(tmpVX216 ? L18VXList : L18List, idx).Replace(",", ".");
            kv["SumL18"] = serie222 ? "" : betE ? "" : "(L18) " + tmpL18;
            kv["SumL18Tol"] = betC ? "\u00b1 1" : "";

            string tmpA18 = betC
                ? (typNum == 15 ? "3.23"
                 : typNum == 16 ? "3.01"
                 : typNum == 17 ? "3.99"
                 : typNum == 18 ? "3.95"
                 : typNum == 19 ? "3.83"
                 : typNum == 20 ? "3.48"
                 : typNum == 24 ? "2.96"
                 : typNum == 22 ? "3.23"
                 : typNum == 26 ? "2.77"
                 : typNum == 28 ? "2.6"
                 : (typNum == 30 && tmpVX216) ? "2.45"
                 : typNum == 30 ? "2.46"
                 : typNum == 32 ? "2.34"
                 : typNum == 34 ? "2.23"
                 : typNum == 36 ? "2.71"
                 : typNum == 38 ? "2.61"
                 : typNum == 40 ? "2.5"
                 : typNum == 44 ? "2.32"
                 : typNum == 48 ? "2.22"
                 : "0")
                : "0";
            kv["SumA18"] = betC ? tmpA18 + "\u00b0 \u00b10.25\u00b0" : "";
            kv["SumA18txt"] = betC ? "(A18) 4x" : "";

            string tmpSpg = tmpD01 < 181 ? "0.100"
                          : tmpD01 < 251 ? "0.115"
                          : tmpD01 < 316 ? "0.130"
                          : tmpD01 < 401 ? "0.140"
                          : "0.155";

            kv["SumYtD10"] = "1.6";
            kv["SumYtD01"] = "1.6";
            kv["SumYtBg"] = "2.0";
            kv["SumYtB2"] = "3.5";
            kv["SumRa63"] = "6.3";

            bool tmpAllMsk = IsMember(mv.Trim(),
                "LB-4000 MY",
                "LB-4000",
                "LU-45",
                "Okuma LB3000 4580");

            string tmpMaskinValS1 = tmpAllMsk ? mv : "";
            kv["SumMaskinValS1"] = "Maskin: " + tmpMaskinValS1 + " - OP1";
            string tmpMaskinValS2 = tmpAllMsk ? mv : "";
            kv["SumMaskinValS2"] = "Maskin: " + tmpMaskinValS2 + " - OP2";
            string tmpMaskinValS3 = tmpAllMsk ? "M\u00e4tb\u00e4nk" : "";
            kv["SumMaskinValS3"] = "Maskin: " + tmpMaskinValS3 + " - OP3";

            kv["SumF1_1"] = tmpAllMsk ? "1/\u00e4mne" : "";
            kv["SumF1_2"] = tmpAllMsk ? "1/\u00e4mne" : "";
            kv["SumF1_3"] = tmpAllMsk ? "1/\u00e4mne" : "";
            kv["SumF1_4"] = tmpAllMsk ? "Sk\u00e4rbyte" : "";
            kv["SumF1_5"] = tmpAllMsk ? "1/\u00e4mne" : "";
            kv["SumF1_6"] = tmpAllMsk ? "Sk\u00e4rbyte" : "";
            kv["SumF1_7"] = tmpAllMsk ? "1/\u00e4mne" : "";
            kv["SumF1_8"] = tmpAllMsk ? "Sk\u00e4rbyte" : "";
            kv["SumF1_9"] = tmpAllMsk ? "1/Skift" : "";
            kv["SumF1_0"] = tmpAllMsk ? "Sk\u00e4rbyte" : "";

            kv["SumF2_1"] = tmpAllMsk ? "1/tim" : "";
            kv["SumF2_2"] = tmpAllMsk ? "1/tim" : "";
            kv["SumF2_3"] = tmpAllMsk ? "1/skift" : "";
            kv["SumF2_4"] = tmpAllMsk ? (betC ? "1/skift" : "") : "";
            kv["SumF2_5"] = "";

            kv["SumF3_1"] = tmpAllMsk ? "1/tim" : "";
            kv["SumF3_2"] = tmpAllMsk ? "1/1" : "";
            kv["SumF3_3"] = tmpAllMsk ? "1/1" : "";
            kv["SumF3_4"] = tmpAllMsk ? "1" : "";

            kv["SumD1_1"] = tmpAllMsk ? "UD-Apparat" : "";
            kv["SumD1_2"] = tmpAllMsk ? "UD-Apparat" : "";
            kv["SumD1_3"] = tmpAllMsk ? "M\u00e4tplatta" : "";
            kv["SumD1_4"] = tmpAllMsk ? "Mikrometer" : "";
            kv["SumD1_5"] = tmpAllMsk ? "M\u00e4tplatta" : "";
            kv["SumD1_6"] = tmpAllMsk ? "Mall" : "";
            kv["SumD1_7"] = tmpAllMsk ? "UD-Apparat" : "";
            kv["SumD1_8"] = tmpAllMsk ? "UD-Apparat" : "";
            kv["SumD1_9"] = tmpAllMsk ? "Ytj\u00e4mnhetsm\u00e4tare" : "";
            kv["SumD1_0"] = tmpAllMsk ? "Egglinjal" : "";

            kv["SumD2_1"] = tmpAllMsk ? "Digital h\u00f6jdm\u00e4tare" : "";
            kv["SumD2_2"] = tmpAllMsk ? "Mikrometer med avrundad m\u00e4tkolv" : "";
            kv["SumD2_3"] = tmpAllMsk ? "Ytj\u00e4mnhetsm\u00e4tare" : "";
            kv["SumD2_4"] = tmpAllMsk ? (betC ? "Vinkelsystem" : "") : "";
            kv["SumD2_5"] = "";

            kv["SumD3_1"] = tmpAllMsk ? "Digital h\u00f6jdm\u00e4tare" : "";
            kv["SumD3_2"] = tmpAllMsk ? "UD-Apparat" : "";
            kv["SumD3_3"] = tmpAllMsk ? "UD-Apparat" : "";
            kv["SumD3_4"] = tmpAllMsk ? "Vippindikator" : "";

            kv["SumAF1_1"] = tmpAllMsk ? "Ritn.m\u00e5tt efter h\u00e4rdning " + tmpND10Str : "";
            kv["SumAF1_2"] = tmpAllMsk ? "Ritn.m\u00e5tt efter h\u00e4rdning " + tmpND01Str : "";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = tmpAllMsk ? "M\u00e4tl\u00e4ngd: " + tmpML : "";
            kv["SumAF1_6"] = tmpAllMsk ? "Om underk\u00e4nd enl. mall s\u00e5 l\u00e4mna ring till m\u00e4trum f\u00f6r m\u00e4tning i Marsurf LD120." : "";
            kv["SumAF1_7"] = tmpAllMsk ? "Rikta ringen vid orundhet." : "";
            kv["SumAF1_8"] = tmpAllMsk ? "Max: " + tmpAdg + " \u2013 Ideal = 0  M\u00e4th\u00f6jd: 1mm fr\u00e5n (L02) planet" : "";
            kv["SumAF1_9"] = tmpAllMsk ? "Ytj\u00e4mheten \u00e4r f\u00f6re h\u00e4rdning." : "";
            kv["SumAF1_0"] = "";

            kv["SumAF2_1"] = tmpAllMsk ? "Idealtolerans " + tmpIL19Tol : "";
            kv["SumAF2_2"] = tmpAllMsk ? "Max: " + tmpEg + " - Ideal = 0" : "";
            kv["SumAF2_3"] = tmpAllMsk ? "Utf\u00f6r okul\u00e4rkontroll kontinuerligt" : "";
            kv["SumAF2_4"] = tmpAllMsk ? (betC ? "G\u00e4ller Ringar typ C" : "") : "";
            kv["SumAF2_5"] = "";

            kv["SumAF3_1"] = "";
            kv["SumAF3_2"] = tmpAllMsk ? "Variationen f\u00f6r inv. & utv. diameter ska vara inom tolerans annars riktas ringen" : "";
            kv["SumAF3_3"] = "";
            kv["SumAF3_4"] = tmpAllMsk ? "Max skevhet: " + tmpSpg : "";

            kv["SumAnmExtra"] = "M\u00e4tning enligt RJ5 i styrplan" + LB + Environment.NewLine
                + "Vid inst\u00e4llning kontrolleras att samtliga m\u00e5tt ligger lika, alla ringar p\u00e5 \u00e4mnet. Kolla \u00e4ven v\u00e4ndpunkten." + LB + Environment.NewLine
                + "Okul\u00e4rkontroll utf\u00f6res avseende materialfel, slagm\u00e4rken och grader.";

            kv["SumTextS1"] = "Ytterdiameter (D10) kompenserad med " + tmpKompD10 + " beroende p\u00e5 till\u00e5ten h\u00e4rd\u00f6kning." + LB + Environment.NewLine
                + "Innerdiameter (D01) kompenserad med " + tmpKompD01 + " beroende p\u00e5 till\u00e5ten h\u00e4rd\u00f6kning.";
            kv["SumTextS2"] = "";
            kv["SumTextS3"] = "Okul\u00e4rkontroll utf\u00f6res avseende materialfel, slagm\u00e4rken och grader.";

            string sumRit = "Produktritning: Windchill.skf.net - " + tmpBet + "  Toleransritning: Windchill.skf.net - parameters";
            kv["SumRitS1"] = sumRit;
            kv["SumRitS2"] = sumRit;
            kv["SumRitS3"] = sumRit;

            return kv;
        }

        private static string ComputePopup(string pub)
        {
            if (string.IsNullOrWhiteSpace(pub)) return "";
            DateTime dt; if (!DateTime.TryParse(pub, out dt)) return "";
            DateTime till = dt.AddDays(14);
            return DateTime.Today <= till.Date
                ? "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)" + LB + LB
                  + "Om m\u00e4tdonsbest\u00e4llning g\u00f6rs s\u00e5 \u00e4r minutavgr\u00e4nsningen (skiljetecknet) numera med komma INTE kolon" + LB + LB
                  + "Information om senaste \u00e4ndring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument" + LB + LB
                  + "Popupruta aktiv till " + till.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                : "";
        }

        private static string Word(string[] list, int index)
        {
            if (list == null || index < 1 || index > list.Length) return "";
            return list[index - 1];
        }

        private static int Member(string val, string[] list)
        {
            if (list == null) return 0;
            for (int i = 0; i < list.Length; i++)
                if (string.Equals(list[i], val ?? "", StringComparison.OrdinalIgnoreCase)) return i + 1;
            return 0;
        }

        private static bool In(double val, params double[] list)
        {
            if (list == null) return false;
            for (int i = 0; i < list.Length; i++)
                if (list[i] == val) return true;
            return false;
        }

        private static bool IsMember(string val, params string[] list)
        {
            if (list == null) return false;
            for (int i = 0; i < list.Length; i++)
                if (string.Equals(list[i], val ?? "", StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        private static bool EqualsI(string a, string b) =>
            string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);

        private static string Left(string s, int n) =>
            string.IsNullOrEmpty(s) ? "" : (n <= 0 ? "" : (s.Length <= n ? s : s.Substring(0, n)));

        private static string Right(string s, int n) =>
            string.IsNullOrEmpty(s) ? "" : (s.Length <= n ? s : s.Substring(s.Length - n));

        private static double TryParseDouble(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;
            double v;
            return double.TryParse(s.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static string FmtComma(double v) =>
            v.ToString("0.################", CommonFunctions.Culture).Replace(".", ",");

        private static string FmtGenDot(double v) =>
            v.ToString("0.################", CommonFunctions.Culture).Replace(",", ".");

        private static string Fmt3(double v) =>
            v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
    }
}