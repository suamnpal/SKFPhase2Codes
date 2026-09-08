using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_CG_serie_293: ITemplateCalculations
    {
        private const string LB = "<<LineBreak>>";

        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var kv = new Dictionary<string, string>();

            string subject = request?.ProductDesignation ?? "";
            string maskinVal = request?.MachineNumber ?? "";

            DateTime now = DateTime.Now;

            int tmpWeekday = (int)now.DayOfWeek;
            bool tmpHelg = tmpWeekday == 0 || tmpWeekday == 6;

            kv["VaLPopUp"] = ComputePopup(request?.Published);

            string tmpKon0 = "0.0";
            string tmpKon01 = "0.100";
            string tmpKon02 = "0.200";
            string tmpKon03 = "0.300";
            string tmpKon04 = "0.400";
            string tmpKon05 = "0.500";
            string tmpKon1 = "1.000";

            string tmpFormat = subject.Trim().ToUpperInvariant();
            string tmpBet = tmpFormat.Replace(".", ",");

            int tmpCount = tmpBet.Length;

            bool tmpSlash = tmpBet.Contains("/");
            bool tmpE = tmpBet.Contains("E");

            string[] parts = tmpBet.Split(new[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);

            string tmpBet1 = parts.Length > 0 ? parts[0] : "";
            string tmpBet2 = parts.Length > 1 ? parts[1] : "";

            object tmpBet3 = !tmpE ? (object)ParseDouble(parts.Length > 2 ? parts[2] : "") : (parts.Length > 2 ? parts[2] : "");
            object tmpBet4 = !tmpE ? (object)ParseDouble(parts.Length > 3 ? parts[3] : "") : (parts.Length > 3 ? parts[3] : "");

            double tmpBet5 = ParseDouble(parts.Length > 4 ? parts[4] : "");
            double tmpBet6 = ParseDouble(parts.Length > 5 ? parts[5] : "");
            double tmpBet7 = ParseDouble(parts.Length > 6 ? parts[6] : "");

            string tmpSerie = tmpBet2.Length >= 3 ? tmpBet2.Substring(0, 3) : tmpBet2;

            double tmpTyp = !tmpSlash ? ParseDouble(Mid(tmpBet2, 3, 2)) : ParseDouble(tmpBet3.ToString());

            string tmpRit = "Windchill.skf.net - " + tmpFormat;

            kv["SumRitS1"] = tmpRit;
            kv["SumRitS2"] = tmpRit;

            string[] tmpTypValues = { "44", "48", "52", "56", "60", "64", "68", "72", "76", "80", "84", "88", "92", "96", "500", "530", "560", "600", "630", "670", "710", "750", "800", "1000", "1250", "1600" };

            int tmpTypLista = Array.IndexOf(tmpTypValues,Convert.ToInt32(tmpTyp).ToString(CultureInfo.InvariantCulture)) + 1;

            string[] tmpALista = tmpE ? new[] { "254", "274", "298", "318", "343", "0", "387", "407", "432", "452", "474", "496", "518", "538", "558", "590", "0", "0", "704", "0", "791", "835", "886", "1107", "1376", "1755" } : new[] { "254", "274", "298", "318", "343", "0", "387", "407", "432", "452", "474", "496", "518", "538", "558", "593", "0", "0", "704", "0", "791", "835", "888", "1107", "1376", "1755" };

            double tmpA = ParseDouble(tmpALista[tmpTypLista - 1]);

            kv["SumA"] = "(A) " + Smart(tmpA);
            kv["SumATol"] = "+ " + tmpKon0;

            double tmpATolN = !tmpE && (tmpTyp == 530 || tmpTyp == 800) ? (tmpTyp == 530 ? 0.430 : 0.520) : (tmpA < 3.01 ? 0.060 : tmpA < 6.01 ? 0.075 : tmpA < 10.01 ? 0.090 : tmpA < 18.01 ? 0.110 : tmpA < 30.01 ? 0.130 : tmpA < 50.01 ? 0.160 : tmpA < 80.01 ? 0.190 : tmpA < 120.01 ? 0.220 : tmpA < 180.01 ? 0.250 : tmpA < 250.01 ? 0.290 : tmpA < 315.01 ? 0.320 : tmpA < 400.01 ? 0.360 : tmpA < 500.01 ? 0.400 : tmpA < 630.01 ? 0.440 : tmpA < 800.01 ? 0.500 : tmpA < 1000.01 ? 0.560 : tmpA < 1380 ? 0.660 : tmpA < 1600.01 ? 0.780 : tmpA < 2000.01 ? 0.920 : tmpA < 2500.01 ? 1.100 : 1.350);

            kv["SumATolN"] = "- " + F3(tmpATolN);

            double tmpB = !tmpSlash ? tmpTyp / 2 * 10 : ParseDouble(tmpBet3.ToString());

            double tmpBAvMattU = tmpB < 165 ? 0.28 : tmpB < 185 ? 0.31 : tmpB < 210 ? 0.34 : tmpB < 230 ? 0.38 : tmpB < 250 ? 0.42 : tmpB < 290 ? 0.48 : tmpB < 310 ? 0.54 : tmpB < 350 ? 0.60 : tmpB < 410 ? 0.68 : tmpB < 450 ? 0.76 : tmpB < 510 ? 0.84 : tmpB < 570 ? 0.96 : tmpB < 640 ? 1.05 : tmpB < 711 ? 1.20 : tmpB < 801 ? 1.35 : tmpB < 1001 ? 1.70 : tmpB < 1251 ? 2.10 : 2.70;

            double tmpBAvMattO = tmpE && (tmpTyp == 530 || tmpTyp == 800) ? (tmpTyp == 530 ? 1.4 : 1.85) : (tmpB < 165 ? 0.53 : tmpB < 185 ? 0.56 : tmpB < 210 ? 0.63 : tmpB < 230 ? 0.67 : tmpB < 250 ? 0.71 : tmpB < 290 ? 0.80 : tmpB < 310 ? 0.86 : tmpB < 350 ? 0.96 : tmpB < 410 ? 1.04 : tmpB < 450 ? 1.16 : tmpB < 510 ? 1.24 : tmpB < 570 ? 1.39 : tmpB < 640 ? 1.48 : tmpB < 711 ? 1.70 : tmpB < 801 ? 1.82 : tmpB < 1001 ? 2.26 : tmpB < 1251 ? 2.76 : 3.48);

            double tmpUtrakningB = tmpB + tmpBAvMattU;
            double tmpUtrakningBTol = tmpBAvMattO - tmpBAvMattU;

            kv["SumB"] = "(B) " + Smart(tmpUtrakningB);
            kv["SumBTol"] = "+ " + TrimZeros(tmpUtrakningBTol);
            kv["SumBTolN"] = "- " + tmpKon0;

            kv["SumB1"] = kv["SumB"];
            kv["SumB1Tol"] = kv["SumBTol"];
            kv["SumB1TolN"] = kv["SumBTolN"];

            string[] tmpCLista = tmpE ? new[] { "46.3", "46", "51", "53", "60", "0", "67.5", "67.5", "72", "74.2", "77", "80", "82.3", "82", "82.5", "88", "0", "0", "102", "0", "117", "124", "129", "156", "155", "208" } : new[] { "46.3", "46", "51", "53", "60", "0", "67.5", "67.5", "72", "74.2", "77", "80", "82.3", "82", "82.5", "88.8", "0", "0", "102", "0", "117", "124", "129", "156", "155", "208" };

            double tmpC = ParseDouble(tmpCLista[tmpTypLista - 1]);

            kv["SumC"] = "(C) " + Smart(tmpC);
            kv["SumCTol"] = "+ " + tmpKon0;

            double tmpCTolN = tmpE && (tmpTyp == 530 || tmpTyp == 800) ? (tmpTyp == 530 ? 0.44 : 0.50) : (tmpTyp < 37 ? 0.25 : tmpTyp < 50 ? 0.29 : tmpTyp < 62 ? 0.32 : tmpTyp < 82 ? 0.36 : tmpTyp < 540 ? (!tmpE ? 0.40 : 0.44) : tmpTyp < 640 ? (!tmpE ? 0.43 : 0.44) : tmpTyp < 801 ? (!tmpE ? 0.47 : 0.50) : tmpTyp < 1001 ? 0.56 : 0.66);

            kv["SumCTolN"] = "- " + F3(tmpCTolN);

            string[] tmpDLista = tmpE ? new[] { "240", "258", "282", "302", "327", "0", "369", "388", "412", "430", "453", "466", "494", "518", "538", "559", "0", "0", "661", "0", "746", "800", "840", "1049", "1303", "1665" } : new[] { "240", "258", "282", "302", "327", "0", "369", "388", "412", "430", "453", "466", "494", "518", "538", "568", "0", "0", "661", "0", "746", "800", "853", "1049", "1303", "1665" };

            double tmpD = ParseDouble(tmpDLista[tmpTypLista - 1]);

            double tmpDAvMattO = tmpE && (tmpTyp == 530 || tmpTyp == 800) ? (tmpTyp == 530 ? 1.85 : 3.00) : (tmpD < 245 ? 0.82 : tmpD < 260 ? 0.92 : tmpD < 310 ? 1.05 : tmpD < 340 ? 1.20 : tmpD < 389 ? 1.35 : tmpD < 446 ? 1.50 : tmpD < 500 ? 1.65 : tmpD < 552 ? 1.85 : tmpD < 631 ? 2.10 : tmpD < 670 ? 2.30 : tmpD < 799 ? 2.60 : tmpD < 880 ? 3.00 : tmpD < 1050 ? 3.70 : tmpD < 1400 ? 4.60 : 5.90);

            double tmpDAvMattU = tmpE && (tmpTyp == 530 || tmpTyp == 800) ? (tmpTyp == 530 ? 2.13 : 3.30) : (tmpD < 245 ? 1.005 : tmpD < 260 ? 1.13 : tmpD < 310 ? 1.26 : tmpD < 340 ? 1.43 : tmpD < 389 ? 1.58 : tmpD < 446 ? 1.75 : tmpD < 500 ? 1.90 : tmpD < 552 ? 2.13 : tmpD < 631 ? 2.37 : tmpD < 670 ? 2.60 : tmpD < 750 ? 2.92 : tmpD < 799 ? 2.90 : tmpD < 860 ? 3.33 : tmpD < 1050 ? 4.12 : tmpD < 1400 ? 5.10 : 6.50);

            double tmpUtrakningD = tmpD - tmpDAvMattO;
            double tmpUtrakningDTol = Math.Abs(tmpDAvMattO - tmpDAvMattU);

            kv["SumD"] = "(D) " + Smart(tmpUtrakningD);
            kv["SumDTol"] = "+ " + tmpKon0;
            kv["SumDTolN"] = "- " + F3(tmpUtrakningDTol);

            string[] tmpD1Lista = tmpE ? new[] { "228", "248", "269", "289", "310", "0", "351", "371", "392", "412", "433", "452", "474", "494", "514", "543", "0", "0", "645", "0", "727", "770", "817", "1021", "1272", "1628" } : new[] { "228", "248", "269", "289", "310", "0", "351", "371", "392", "412", "433", "452", "474", "494", "514", "545", "0", "0", "645", "0", "727", "770", "820", "1021", "1272", "1628" };

            double tmpD1 = ParseDouble(tmpD1Lista[tmpTypLista - 1]);

            double tmpD1Tol = tmpE && (tmpTyp == 530 || tmpTyp == 800) ? (tmpTyp == 530 ? 0.114 : 0.146) : (tmpD1 < 175 ? 0.055 : tmpD1 < 250 ? 0.077 : tmpD1 < 311 ? 0.086 : tmpD1 < 395 ? 0.094 : tmpD1 < 495 ? 0.103 : tmpD1 < 613 ? 0.112 : tmpD1 < 780 ? 0.125 : tmpD1 < 830 ? 0.138 : tmpD1 < 1022 ? 0.171 : tmpD1 < 1280 ? 0.203 : 0.242);

            double tmpD1TolN = tmpE && (tmpTyp == 530 || tmpTyp == 800) ? (tmpTyp == 530 ? 0.044 : 0.056) : (tmpD1 < 175 ? 0.015 : tmpD1 < 250 ? 0.031 : tmpD1 < 311 ? 0.034 : tmpD1 < 395 ? 0.037 : tmpD1 < 495 ? 0.040 : tmpD1 < 613 ? 0.044 : tmpD1 < 780 ? 0.049 : tmpD1 < 830 ? 0.054 : tmpD1 < 1022 ? 0.066 : tmpD1 < 1280 ? 0.078 : 0.092);

            double tmpUtrakningD1 = tmpD1 + tmpD1Tol;
            double tmpUtrakningD1Tol = Math.Abs(tmpD1Tol - tmpD1TolN);

            kv["SumD1"] = "(D1) " + TrimZeros(tmpUtrakningD1);
            kv["SumD1Tol"] = "+ " + tmpKon0;
            kv["SumD1TolN"] = "- " + F3(tmpUtrakningD1Tol);
            kv["SumD1klove"] = TrimZeros(tmpD1);

            string[] tmpFLista = tmpE ? new[] { "226", "245.8", "266.7", "286.5", "307.5", "0", "348.2", "368.1", "389", "409", "429.5", "448.5", "470.6", "490", "510.5", "538", "0", "0", "640", "0", "722", "765", "810", "1013", "1263", "1616" } : new[] { "226", "245.8", "266.7", "286.5", "307.5", "0", "348.2", "368.1", "389", "409", "429.5", "448.5", "470.6", "490", "510.5", "541.3", "0", "0", "640", "0", "722", "765", "815", "1013", "1263", "1616" };

            double tmpF = ParseDouble(tmpFLista[tmpTypLista - 1]);

            kv["SumF"] = "(F) " + Smart(tmpF);

            double tmpFTol = tmpE ? (tmpTyp < 53 ? 0.32 : tmpTyp < 89 ? 0.40 : tmpTyp < 531 ? 0.44 : tmpTyp < 711 ? 0.50 : tmpTyp < 801 ? 0.56 : tmpTyp < 1001 ? 0.66 : tmpTyp < 1251 ? 0.78 : 0.92) : (tmpTyp < 77 ? 0.36 : tmpTyp < 97 ? 0.40 : tmpTyp < 531 ? 0.43 : tmpTyp < 751 ? 0.47 : 0.52);

            kv["SumFTol"] = "+ " + F3(tmpFTol);
            kv["SumFTolN"] = "- " + tmpKon0;

            string[] tmpGLista = tmpE ? new[] { "224", "244", "264.5", "284.5", "305", "0", "345.5", "365.5", "386", "406", "426", "446", "467", "487", "507", "537", "0", "0", "637", "0", "718", "760", "809", "1010", "1261", "1614" } : new[] { "224", "244", "264.5", "284.5", "305", "0", "345.5", "365.5", "386", "406", "426", "446", "467", "487", "507", "537.5", "0", "0", "637", "0", "718", "760", "810", "1010", "1261", "1614" };

            double tmpG = ParseDouble(tmpGLista[tmpTypLista - 1]);

            kv["SumG"] = "(G) " + Smart(tmpG);

            double tmpGTol = tmpFTol * 2;

            kv["SumGTol"] = "+ " + F3(tmpGTol);
            kv["SumGTolN"] = "- " + tmpKon0;

            string[] tmpHLista = tmpE ? new[] { "27.3", "27.5", "30.5", "31.5", "35", "0", "41", "41", "43.5", "44", "47", "49", "50", "51", "51.5", "53", "0", "0", "61", "0", "71", "77", "79", "96", "106", "136" } : new[] { "27.3", "27.5", "30.5", "31.5", "35", "0", "41", "41", "43.5", "44", "47", "49", "50", "51", "51.5", "51.5", "0", "0", "61", "0", "71", "77", "78", "96", "106", "136" };

            double tmpH = ParseDouble(tmpHLista[tmpTypLista - 1]);

            kv["SumH"] = "(H) " + Smart(tmpH);
            kv["SumHTol"] = "+ " + tmpKon0;
            kv["SumHTolN"] = kv["SumCTolN"];

            double tmpJ = tmpE && (tmpTyp == 530 || tmpTyp == 800) ? (tmpTyp == 530 ? tmpH - 10 : tmpH - 13) : (tmpTyp < 49 ? tmpH - 5.5 : tmpTyp < 57 ? tmpH - 6.5 : tmpTyp < 65 ? tmpH - 7 : tmpTyp < 73 ? tmpH - 7.5 : tmpTyp < 81 ? tmpH - 8.5 : tmpTyp < 89 ? tmpH - 9 : tmpTyp < 501 ? tmpH - 10 : tmpTyp < 601 ? tmpH - 11 : tmpTyp < 631 ? tmpH - 12 : tmpTyp < 711 ? tmpH - 13 : tmpTyp < 801 ? tmpH - 14 : tmpTyp == 1000 ? tmpH - 17 : tmpTyp < 1251 ? tmpH - 20 : tmpH - 25);

            kv["SumJ"] = "(J) " + Smart(tmpJ);
            kv["SumJTol"] = "+ " + F3(tmpCTolN);
            kv["SumJTolN"] = "- " + tmpKon0;

            double tmpK = tmpTyp < 49 ? 4 : tmpTyp < 57 ? 4.5 : tmpTyp < 65 ? 5 : tmpTyp < 73 ? 5.5 : (tmpTyp == 76 || tmpTyp == 80 || tmpTyp == 88) ? 6 : tmpTyp == 84 ? 6.5 : tmpTyp < 501 ? 7 : tmpTyp == 530 ? (tmpE ? 6.5 : 8) : (tmpTyp == 630 || tmpTyp == 1250 || tmpTyp == 1600) ? 8 : tmpTyp == 710 ? 9 : tmpTyp == 800 ? (tmpE ? 8.5 : 10) : tmpTyp == 1000 ? 10.5 : 10;

            kv["SumK"] = "(K) " + Smart(tmpK);
            kv["SumKTol"] = "+ " + tmpKon1;
            kv["SumKTolN"] = "- " + tmpKon0;

            string[] tmpLLista = tmpE ? new[] { "18", "18", "21", "21", "22", "0", "27", "27", "28", "28", "31", "35", "33", "34", "34", "42", "0", "0", "42", "0", "50", "46", "64", "77", "85", "111" } : new[] { "18", "18", "21", "21", "22", "0", "27", "27", "28", "28", "31", "35", "33", "34", "34", "34", "0", "0", "42", "0", "50", "46", "50", "77", "85", "111" };

            double tmpL = ParseDouble(tmpLLista[tmpTypLista - 1]);

            kv["SumL"] = "(L) " + Smart(tmpL);
            kv["SumLTol"] = "+ " + F3(tmpCTolN);
            kv["SumLTolN"] = "- " + tmpKon0;

            double tmpLx = tmpC - tmpL;

            kv["SumLx"] = "(L) " + Smart(tmpLx);
            kv["SumLxTol"] = "+ " + tmpKon0;
            kv["SumLxTolN"] = "- " + F3(tmpCTolN * 2);

            string[] tmpMLista = tmpE ? new[] { "5.5", "5.5", "6.5", "6.5", "7", "0", "7.5", "7.5", "8.5", "8.5", "9", "9", "10", "10", "10", "10.5", "0", "0", "12", "0", "13", "14", "13.5", "17", "20", "25" } : new[] { "5.5", "5.5", "6.5", "6.5", "7", "0", "7.5", "7.5", "8.5", "8.5", "9", "9", "10", "10", "10", "11", "0", "0", "12", "0", "13", "14", "14", "17", "20", "25" };

            double tmpM = ParseDouble(tmpMLista[tmpTypLista - 1]);

            kv["SumM"] = "(M) " + Smart(tmpM);
            kv["SumMTol"] = "+ " + tmpKon0;
            kv["SumMTolN"] = kv["SumCTolN"];

            double tmpMx = tmpC - tmpM;

            kv["SumMx"] = "(M) " + Smart(tmpMx);
            kv["SumMxTol"] = "+ " + F3(tmpCTolN);
            kv["SumMxTolN"] = "- " + F3(tmpCTolN);

            double tmpN = tmpTyp < 61 ? 0.5 : tmpTyp < 711 ? 1 : (tmpTyp == 750 || tmpTyp == 800 || tmpTyp == 1000 || tmpTyp == 1600) ? 1.5 : 1;

            kv["SumN"] = "(N) " + Smart(tmpN);
            kv["SumNTol"] = "+ " + tmpKon03;
            kv["SumNTolN"] = "- " + tmpKon0;

            kv["SumN1"] = kv["SumN"];
            kv["SumN1Tol"] = kv["SumNTol"];
            kv["SumN1TolN"] = kv["SumNTolN"];

            double tmpT = tmpTyp < 57 ? 2 : tmpTyp < 89 ? 2.5 : tmpTyp == 530 ? (tmpE ? 3.5 : 3) : tmpTyp < 631 ? 3 : tmpTyp < 751 ? 4 : tmpTyp == 800 ? (tmpE ? 4.5 : 5) : tmpTyp == 1000 ? 3 : tmpTyp == 1250 ? 7 : 8;

            kv["SumT"] = "(T) " + Smart(tmpT);
            kv["SumTTol"] = "+ " + tmpKon02;
            kv["SumTTolN"] = "- " + tmpKon0;

            string[] tmpOLista = tmpE ? new[] { "2.2", "2.3", "2.3", "2.7", "3", "0", "3", "3", "3.2", "3.2", "3.4", "3.4", "3.5", "3.6", "3.6", "4.3", "0", "0", "4.5", "0", "5.1", "5.2", "5.9", "7", "8.5", "10.4" } : new[] { "2.2", "2.3", "2.3", "2.7", "3", "0", "3", "3", "3.2", "3.2", "3.4", "3.4", "3.5", "3.6", "3.6", "3.8", "0", "0", "4.5", "0", "5.1", "5.2", "5.4", "7", "8.5", "10.4" };

            double tmpO = ParseDouble(tmpOLista[tmpTypLista - 1]);

            kv["SumO"] = "(O) " + Smart(tmpO);
            kv["SumOTol"] = kv["SumJTol"];
            kv["SumOTolN"] = "- " + tmpKon0;

            double tmpR = tmpTyp < 49 ? 3 : tmpTyp < 73 ? 4 : tmpTyp < 501 ? 5 : tmpTyp < 531 ? 6 : tmpTyp < 801 ? 7 : tmpTyp < 1251 ? 9 : 12;

            kv["SumR"] = "(R) " + F2Comma(tmpR);

            double tmpRTol = tmpE && (tmpTyp == 530 || tmpTyp == 800) ? 2 : (tmpR < 5.5 ? 1 : tmpR < 6.5 ? 1.5 : 2);

            kv["SumRTol"] = "+ " + F3(tmpRTol);
            kv["SumRTolN"] = "- " + tmpKon0;

            double tmpRh = tmpUtrakningB + (tmpUtrakningBTol / 2) + ((tmpRTol / 2 + tmpR) * 2);

            kv["SumRh"] = "(Rh) Ø " + F2Comma(tmpRh);
            kv["SumRhTol"] = "± " + TrimZeros(tmpRTol);

            double tmpR1 = (tmpE && (tmpTyp == 800 || tmpTyp == 1000)) ? 1.5 : (tmpTyp < 77 ? 0.5 : 1);

            kv["SumR1"] = Smart(tmpR1) + "x45º";

            kv["SumR1a"] = kv["SumR1"];

            string sumR2 = "R" + Smart(tmpTyp < 53 ? 1 : tmpTyp < 73 ? 1.2 : tmpTyp == 530 ? (tmpE ? 2.5 : 1.5) : tmpTyp < 531 ? 1.5 : tmpTyp < 631 ? 2 : tmpTyp == 800 ? (tmpE ? 3.5 : 2.5) : tmpTyp < 801 ? 2.5 : tmpTyp == 1000 ? 4 : tmpTyp < 1251 ? 5 : 6);

            kv["SumR2"] = sumR2;
            kv["SumR2sid1"] = sumR2;

            kv["SumR2a"] = tmpTyp == 530 ? Smart(tmpE ? 3 : 1.5) : tmpTyp == 800 ? Smart(tmpE ? 4 : 2.5) : tmpTyp == 1000 ? Smart(4.5) : sumR2;

            kv["SumR2ab"] = kv["SumR2a"];

            double tmpP = tmpE && (tmpTyp == 530 || tmpTyp == 800) ? (tmpTyp == 530 ? 26.5 : 39) : (tmpTyp < 49 ? 11.5 : tmpTyp < 57 ? 13.5 : tmpTyp < 61 ? 14.5 : tmpTyp < 73 ? 17 : tmpTyp < 81 ? 18 : tmpTyp == 84 ? 20 : tmpTyp == 92 ? 21 : tmpTyp < 531 ? 22 : tmpTyp == 630 ? 27 : (tmpTyp == 710 || tmpTyp == 800) ? 32 : tmpTyp < 751 ? 30 : tmpTyp == 1000 ? 47 : tmpTyp == 1250 ? 52 : 68);

            kv["SumP"] = "(P) " + Smart(tmpP);
            kv["SumPTol"] = "+ " + tmpKon0;
            kv["SumPTolN"] = kv["SumCTolN"];

            double tmpPx = tmpC - tmpP;

            kv["SumPx"] = "(P) " + Smart(tmpPx);
            kv["SumPxTol"] = "+ " + F3(tmpCTolN);
            kv["SumPxTolN"] = "- " + F3(tmpCTolN);

            double tmpS = tmpTyp < 49 ? 6 : tmpTyp < 61 ? 8 : tmpTyp < 89 ? 10 : tmpTyp == 530 ? (tmpE ? 8 : 12) : tmpTyp < 531 ? 12 : tmpTyp == 800 ? (tmpE ? 13 : 15) : tmpTyp < 1251 ? 15 : 20;

            kv["SumS"] = "Ø " + Smart(tmpS) + " x 6 st. hål" + LB + " lika delning";

            double tmpHM = tmpC - tmpH;

            kv["SumHM"] = "(HM) " + Smart(tmpHM);
            kv["SumHMTol"] = "± " + F3(tmpCTolN);

            kv["SumHM2"] = kv["SumHM"];
            kv["SumHM2Tol"] = kv["SumHMTol"];

            kv["SumE"] = tmpTyp < 37 ? "0.160" : tmpTyp < 49 ? "0.185" : tmpTyp < 61 ? "0.210" : tmpTyp < 81 ? "0.230" : tmpTyp < 501 ? "0.250" : tmpTyp < 631 ? "0.280" : tmpTyp < 801 ? "0.320" : tmpTyp < 1251 ? "0.420" : "0.500";

            kv["SumE1"] = tmpTyp < 35 ? "0.063" : tmpTyp < 49 ? "0.072" : tmpTyp < 61 ? "0.081" : tmpTyp < 77 ? "0.089" : tmpTyp < 97 ? "0.097" : tmpTyp < 631 ? "0.110" : tmpTyp < 751 ? "0.125" : tmpTyp < 801 ? "0.140" : "0.195";

            kv["SumRa"] = "1";
            kv["SumRa1"] = "1";
            kv["SumRa25"] = "1";

            kv["SumMaskinValS1"] = "Maskin: " + maskinVal + " - Sid.1";
            kv["SumMaskinValS2"] = "Maskin: " + maskinVal + " - Sid.2";

            bool machineEnabled = maskinVal == "LB45" || maskinVal == "MaxMuller" || maskinVal == "Nakamura" || maskinVal == "VTR-160" || maskinVal == "MacTurn 550" || maskinVal == "Skepp6";

            kv["SumF_A"] = machineEnabled ? "1/3" : "";
            kv["SumF_D"] = machineEnabled ? "1/3" : "";
            kv["SumF_D1"] = machineEnabled ? "1/3" : "";
            kv["SumF_FBG"] = machineEnabled ? "1/3" : "";
            kv["SumF_HJC"] = machineEnabled ? "1/3" : "";
            kv["SumF_HM"] = machineEnabled ? "Inst." : "";
            kv["SumF_Fas"] = machineEnabled ? "Inst." : "";
            kv["SumF_R"] = machineEnabled ? "Inst." : "";
            kv["SumF_Ra"] = machineEnabled ? "1/3" : "";

            kv["SumF_N"] = machineEnabled ? "1/1" : "";
            kv["SumF_LMO"] = machineEnabled ? "1/3" : "";
            kv["SumF_HM2"] = machineEnabled ? "Inst." : "";
            kv["SumF_Fas2"] = machineEnabled ? "Inst." : "";
            kv["SumF_TK"] = machineEnabled ? "Inst." : "";
            kv["SumF_R2"] = machineEnabled ? "Inst." : "";
            kv["SumF_Ra2"] = machineEnabled ? "1/3" : "";
            kv["SumF_EE1"] = machineEnabled ? "Inst." : "";

            kv["SumD_A"] = machineEnabled ? "Digitalskjutmått" : "";
            kv["SumD_D"] = machineEnabled ? "Mikrometer/Skjutmått" : "";
            kv["SumD_D1"] = machineEnabled ? "Mikrometer/Mätplatta" : "";
            kv["SumD_FBG"] = machineEnabled ? "Inv. Mikrometer/Skjutmått" : "";
            kv["SumD_N"] = machineEnabled ? "Digital Djup/Hakmått" : "";
            kv["SumD_HJC"] = machineEnabled ? "Digital Djup/Skjutmått" : "";
            kv["SumD_LMO"] = machineEnabled ? "Digital Djup/Hakmått" : "";
            kv["SumD_HM"] = machineEnabled ? "Digital Djupmått" : "";
            kv["SumD_Fas"] = machineEnabled ? "Digitalskjutmått" : "";
            kv["SumD_TK"] = machineEnabled ? "Passbitar/Skjutmått" : "";
            kv["SumD_R"] = machineEnabled ? "Radielyra" : "";
            kv["SumD_Ra"] = machineEnabled ? "Ytjämnhetsmätare" : "";

            kv["SumD_HM2"] = machineEnabled ? "Digital Djupmått" : "";
            kv["SumD_Fas2"] = machineEnabled ? "Digitalskjutmått" : "";
            kv["SumD_R2"] = machineEnabled ? "Radielyra" : "";
            kv["SumD_Ra2"] = machineEnabled ? "Ytjämnhetsmätare" : "";
            kv["SumD_EE1"] = "";

            kv["SumAF_A"] = "";
            kv["SumAF_D"] = "";
            kv["SumAF_D1"] = machineEnabled ? "Klove: " + Smart(tmpD1) + " Utf.2" : "";
            kv["SumAF_FBG"] = "";
            kv["SumAF_N"] = "";
            kv["SumAF_HJC"] = "";
            kv["SumAF_LMO"] = "";
            kv["SumAF_HM"] = "";
            kv["SumAF_Fas"] = "";
            kv["SumAF_TK"] = "";
            kv["SumAF_R"] = "";
            kv["SumAF_Ra"] = "";
            kv["SumAF_EE1"] = "";

            kv["SumAF_HM2"] = "";
            kv["SumAF_Fas2"] = "";
            kv["SumAF_R2"] = "";
            kv["SumAF_Ra2"] = "";

            kv["SumTextS1"] = "";
            kv["SumTextS2"] = "";

            int tmpTillaggTid = 3;

            string tmpTime = now.Hour + "," + now.Minute;

            int tmpDay2 = now.Day + 1;

            int tmpHour2 = (now.Hour == 0 || now.Hour < 6) ? 9 : now.Hour + tmpTillaggTid;

            string tmpTime2 = tmpHour2 + "," + now.Minute;

            string tmpDat2 = now.Year + "-" + now.Month + "-" + tmpDay2;

            bool tmpMDUtf2 = true;

            string tmpMTxtUtf2 = tmpMDUtf2 ? "Passbitklove Utf. 2 med måttet " + kv["SumD1"] : "";

            int tmpGranstid = 10;

            bool tmpFardigMin = tmpHelg ? true : now.Hour > (tmpGranstid - 1);

            string tmpOpen = "Öppettider - mätcenter:" + LB + "Vardagar: 07:00-15:30" + LB + "Helg: Stängt";

            string tmpHelgText = !tmpHelg ? " i morgon " + tmpDat2 + " vid 9:00" : " Måndag vid 9:00";

            string tmpAfter10 = !tmpHelg ? "Din beställning gjordes efter senast 10:00 så leverans kan ske" : "Din beställning gjordes på helgen så leverans kan ske tidigast";

            string tmpLev = "OBS!" + LB + "Minsta möjliga behandlingstid är " + tmpTillaggTid + " timmar under mätcenters öppettider. Föreslagen tid är den tidigaste, variationer kan förekomma." + LB + LB + "Vid snävare behov kontakta personligen mätcenter" + LB + LB + tmpOpen + LB;

            kv["VaLFardig"] = "";
            kv["VaLInfo"] = "";

            return kv;
        }

        static double ParseDouble(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return 0;
            double.TryParse(value.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double result);
            return result;
        }

        static string Smart(double value)
        {
            return Math.Abs(value % 1) < 0.0001 ? value.ToString("F0", CultureInfo.InvariantCulture) : value.ToString("0.###", CultureInfo.InvariantCulture).Replace(".", ",");
        }
        static string F2Comma(double value)
        {
            return value.ToString("F2", CultureInfo.InvariantCulture).Replace(".", ",");
        }

        static string TrimZeros(double value)
        {
            return value.ToString("0.###", CultureInfo.InvariantCulture);
        }

        static string F3(double value)
        {
            return value.ToString("F3", CultureInfo.InvariantCulture);
        }

        static string Mid(string value, int start, int length)
        {
            if (string.IsNullOrWhiteSpace(value) || start >= value.Length) return "";
            if (start + length > value.Length) length = value.Length - start;
            return value.Substring(start, length);
        }

        static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published)) return "";
            if (!DateTime.TryParse(published, out DateTime publishDate)) return "";
            DateTime validTo = publishDate.AddDays(14);
            if (DateTime.Today > validTo) return "";
            return "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)" + LB + LB + "Information om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument" + LB + LB + "Popupruta aktiv till " + validTo.ToString("yyyy-MM-dd");
        }
    }
}
