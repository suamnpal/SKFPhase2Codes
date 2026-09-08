using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Klamhylsor_Stal_Skepp_6_OP1_4 : ITemplateCalculations
    {
        private const string LB = "<<LineBreak>>";

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req?.ProductDesignation ?? "";
            string mv = req?.MachineNumber ?? "";
            List<Bookmark> bm = req?.Bookmarks;

            kv["VaLPopUp"] = ComputePopup(req?.Published);

            string tmpFormat = subject.Trim().ToUpperInvariant();
            string tmpBet = tmpFormat.Replace(".", ",");

            bool tmpSlash = tmpBet.Contains("/");
            bool tmpLW = ContainsI(tmpBet, "LW");
            bool tmpMS = ContainsI(tmpBet, "MS");

            string[] parts = tmpBet.Split(new[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);

            string tmpBet1a = parts.Length > 0 ? parts[0] : "";
            string tmpBet2a = parts.Length > 1 ? parts[1] : "";
            string tmpBet3a = parts.Length > 2 ? parts[2] : "";
            string tmpBet4a = parts.Length > 3 ? parts[3] : "";
            string tmpBet5a = parts.Length > 4 ? parts[4] : "";
            string tmpBet6a = parts.Length > 5 ? parts[5] : "";

            string tmpBet1b = string.IsNullOrEmpty(tmpBet1a) ? "0" : tmpBet1a;
            string tmpBet2b = string.IsNullOrEmpty(tmpBet2a) ? "0" : tmpBet2a;
            string tmpBet3b = string.IsNullOrEmpty(tmpBet3a) ? "0" : tmpBet3a;
            string tmpBet4b = string.IsNullOrEmpty(tmpBet4a) ? "0" : tmpBet4a;
            string tmpBet5b = string.IsNullOrEmpty(tmpBet5a) ? "0" : tmpBet5a;
            string tmpBet6b = string.IsNullOrEmpty(tmpBet6a) ? "0" : tmpBet6a;

            object tmpBet1 = IsNumeric(tmpBet1b) && tmpBet1b != "0" ? ParseDouble(tmpBet1a) : (object)tmpBet1a;
            object tmpBet2 = IsNumeric(tmpBet2b) && tmpBet2b != "0" ? ParseDouble(tmpBet2a) : (object)tmpBet2a;
            object tmpBet3 = IsNumeric(tmpBet3b) && tmpBet3b != "0" ? ParseDouble(tmpBet3a) : (object)tmpBet3a;
            object tmpBet4 = IsNumeric(tmpBet4b) && tmpBet4b != "0" ? ParseDouble(tmpBet4a) : (object)tmpBet4a;
            object tmpBet5 = IsNumeric(tmpBet5b) && tmpBet5b != "0" ? ParseDouble(tmpBet5a) : (object)tmpBet5a;
            object tmpBet6 = IsNumeric(tmpBet6b) && tmpBet6b != "0" ? ParseDouble(tmpBet6a) : (object)tmpBet6a;

            string tmpBet2Text = LotusText(tmpBet2);
            int tmpCountB2 = tmpBet2Text.Length;

            string tmpSerie = tmpLW ? "0": tmpSlash && (tmpCountB2 == 3 || tmpCountB2 == 2)? tmpBet2Text
                    : tmpCountB2 > 4? Left(tmpBet2Text, 3) : tmpCountB2 == 3 ? Left(tmpBet2Text, 1) : Left(tmpBet2Text, 2);

            double tmpTyp = tmpMS || tmpLW ? 0: !tmpSlash ? ParseDouble(Right(tmpBet2Text, 2)) : ParseDouble(LotusText(tmpBet3));

            double tmpd1 = GetDouble(bm, "Innerdiameter (d1)");
            double tmpb = GetDouble(bm, "Gänglängd (b)");
            double tmpL = GetDouble(bm, "Längd (L)");
            double aMatt = GetDouble(bm, "a-mått");

            double avvikandeD = GetDouble(bm, "Avvikande YDia Gänga (d)");

            double tmpd = avvikandeD != 0 ? avvikandeD: !tmpSlash ? (tmpTyp / 2) * 10 : ParseDouble(LotusText(tmpBet3));

            double tmpStmm = tmpd < 301 ? 4 : tmpd < 501 ? 5 : tmpd < 701 ? 6 : tmpd < 901 ? 7 : 8;

            kv["SumGänga"] = "Tr" + Smart(tmpd) + "x" + Smart(tmpStmm);
            kv["SumP"] = "Tr" + Smart(tmpStmm);
            kv["SumP1"] = "(P) " + Smart(tmpStmm);
            kv["SumRullar"] = Smart(tmpStmm) + "mm";

            double tmpKonD2 = 3;
            double tmpKonD = tmpSerie == "32" && tmpTyp == 500 ? 3 : 2;
            double tmpKonD1 = tmpd < 599 ? 2 : 3;
            double tmpKonL = 1;
            double tmpKonB = 1;

            double konaBookmark = GetDouble(bm, "Kona");

            double tmpKona = konaBookmark != 0 ? konaBookmark
                : tmpSerie == "30" || tmpSerie == "31" || tmpSerie == "32" || tmpSerie == "39" ? 12: 30;

            kv["SumV"] = tmpKona == 30 ? "(V) 0º57" : "(V) 2º23";
            kv["SumKonaOP1"] = "Kona 1:" + Smart(tmpKona);
            kv["SumKona"] = "Kona 1:" + Smart(tmpKona);

            double tmpLOP2 = tmpL + tmpKonL;

            kv["SumLOP2"] = "(L) " + Smart(tmpLOP2);
            kv["SumLOP2Tol"] = "± 0.5";
            kv["SumLOP3"] = "(L) " + Smart(tmpL);
            kv["SumLOP3Tol"] = "± 0.25";
            kv["SumL"] = "(L) " + Smart(tmpL);
            kv["SumLTol"] = " 0 [3F]";

            double tmpLTolN = tmpL < 11 ? 0.580 : tmpL < 19 ? 0.700 : tmpL < 31 ? 0.840 : tmpL < 51 ? 1.000 : tmpL < 81 ? 1.200 : tmpL < 121 ? 1.400 : tmpL < 181 ? 1.600 : tmpL < 251 ? 1.850 : tmpL < 316 ? 2.100 : tmpL < 401 ? 2.300 : 2.500;

            kv["SumLTolN"] = "- " + F3(tmpLTolN) + " [3F]";

            double tmpbOP1 = tmpb + tmpKonB;

            kv["SumbOP1"] = "(b) " + Smart(tmpbOP1);
            kv["SumbOP1Tol"] = "+ 1";
            kv["SumbOP1TolN"] = " 0";
            kv["Sumb"] = "(b) " + Smart(tmpb);
            kv["SumbTol"] = (tmpb < 11 ? "+ 1.5" : tmpb < 19 ? "+ 1.8" : tmpb < 31 ? "+ 2.1" : tmpb < 51 ? "+ 2.5" : tmpb < 81 ? "+ 3.0" : tmpb < 121 ? "+ 3.5" : "+ 4.0") + "  [3F]";
            kv["SumbTolN"] = " 0 [2F]";

            double tmpGTjTol = tmpKona == 12 ? (tmpd > 1000 ? 0.095 : tmpd > 800 ? 0.085 : tmpd > 630 ? 0.075 : tmpd > 500 ? 0.070 : tmpd > 400 ? 0.065 : tmpd > 315 ? 0.060 : tmpd > 250 ? 0.055 : tmpd > 180 ? 0.050 : tmpd > 120 ? 0.040 : tmpd > 80 ? 0.035 : tmpd > 50 ? 0.030 : tmpd > 30 ? 0.025 : 0.020)
                : tmpKona == 30 ? (tmpd > 1000 ? 0.060 : tmpd > 800 ? 0.055 : tmpd > 630 ? 0.050 : tmpd > 500 ? 0.045 : tmpd > 400 ? 0.040 : tmpd > 315 ? 0.035 : tmpd > 250 ? 0.035 : tmpd > 180 ? 0.030 : tmpd > 120 ? 0.025 : tmpd > 80 ? 0.022 : tmpd > 50 ? 0.019 : tmpd > 30 ? 0.016 : 0.013)
                : 0;

            double tmpGTjTolN = tmpKona == 12 ? (tmpd > 1000 ? 0.280 : tmpd > 800 ? 0.250 : tmpd > 630 ? 0.225 : tmpd > 500 ? 0.200 : tmpd > 400 ? 0.190 : tmpd > 315 ? 0.175 : tmpd > 250 ? 0.160 : tmpd > 180 ? 0.140 : tmpd > 120 ? 0.120 : tmpd > 80 ? 0.105 : tmpd > 50 ? 0.090 : tmpd > 30 ? 0.075 : 0.070)
                : tmpKona == 30 ? (tmpd > 1000 ? 0.170 : tmpd > 800 ? 0.155 : tmpd > 630 ? 0.140 : tmpd > 500 ? 0.125 : tmpd > 400 ? 0.115 : tmpd > 315 ? 0.105 : tmpd > 251 ? 0.095 : tmpd > 180 ? 0.085 : tmpd > 120 ? 0.075 : tmpd > 80 ? 0.065 : tmpd > 50 ? 0.055 : tmpd > 30 ? 0.046 : 0.039)
                : 0;

            bool validKona = tmpKona == 12 || tmpKona == 30;
            kv["SumGTjTol"] = validKona ? "+ " + F3(tmpGTjTol) : "+ Fel Kona";
            kv["SumGTjTolN"] = validKona ? "- " + F3(tmpGTjTolN) : "- Fel Kona";

            double tmpd2Raw = ((tmpL - aMatt) / tmpKona) + tmpd;
            double tmpd2 = TruncateToFactor(tmpd2Raw, 0.1);
            string tmpd2Text = tmpd2.ToString("F1", CultureInfo.InvariantCulture).Replace(".", ",");

            double tmpd2OP2 = tmpd2 + tmpKonD2;
            double tmpdOP1 = tmpd + tmpKonD;

            double tmpdOP2Raw = tmpd2OP2 - ((tmpL - tmpbOP1) / tmpKona);
            double tmpdOP2 = LotusRound(tmpdOP2Raw, 0.1);

            kv["Sumd2OP2"] = "(d2) " + Smart(tmpd2OP2);
            kv["Sumd2OP2Tol"] = " 0";
            kv["Sumd2OP2TolN"] = "- 0.5";
            kv["Sumd2"] = "(d2) " + tmpd2Text;
            kv["Sumd2Tol"] = tmpd2 < 6.01 ? "± 0.1" : tmpd2 < 30.01 ? "± 0.2" : tmpd2 < 120.01 ? "± 0.3" : tmpd2 < 400.01 ? "± 0.5" : tmpd2 < 1000.01 ? "± 0.8" : tmpd2 < 2000.01 ? "± 1.2" : "± 2.0";

            kv["SumdOP1"] = "(d) " + Smart(tmpdOP1);
            kv["SumdOP1Tol"] = " 0";
            kv["SumdOP1TolN"] = "- 0.3";

            kv["SumdOP2"] = "(d) " + Smart(tmpdOP2);
            kv["SumdOP2Tol"] = " 0";
            kv["SumdOP2TolN"] = "- 0.5";

            kv["SumdOP3"] = "(d) " + Smart(tmpd);
            kv["SumdOP3Tol"] = " 0";
            kv["SumdOP3TolN"] = tmpStmm == 4 ? "- 0.300" : tmpStmm == 5 ? "- 0.335" : tmpStmm == 6 ? "- 0.375" : tmpStmm == 7 ? "- 0.425" : "- 0.450";

            kv["SumdaOP3"] = kv["SumdOP3"];
            kv["SumdaOP3Tol"] = " 0";
            kv["SumdaOP3TolN"] = kv["SumdOP3TolN"];

            kv["Sumd"] = "(d) " + Smart(tmpd);
            kv["SumdTol"] = " 0";
            kv["SumdTolN"] = kv["SumdOP3TolN"];

            double tmpd1OP1 = tmpd1 - tmpKonD1;

            kv["Sumd1OP1"] = "(d1) " + Smart(tmpd1OP1);
            kv["Sumd1OP1Tol"] = "+ 0.5";
            kv["Sumd1OP1TolN"] = " 0";
            kv["Sumd1"] = "(d1) " + Smart(tmpd1);
            kv["Sumd1Tol"] = (tmpd1 < 31 ? "± 0.026" : tmpd1 < 51 ? "± 0.031" : tmpd1 < 81 ? "± 0.037" : tmpd1 < 121 ? "± 0.043" : tmpd1 < 181 ? "± 0.050" : tmpd1 < 251 ? "± 0.057" : tmpd1 < 316 ? "± 0.065" : tmpd1 < 401 ? "± 0.070" : tmpd1 < 501 ? "± 0.077" : tmpd1 < 631 ? "± 0.087" : tmpd1 < 801 ? "± 0.100" : tmpd1 < 1001 ? "± 0.115" : "± 0.130") + " [3F]";

            double tmpKL = tmpL + 3 - tmpb + 1;

            kv["SumKL"] = "(KL) " + Smart(tmpKL);
            kv["SumKLTol"] = " 0";
            kv["SumKLTolN"] = "- 1.0";

            double tmpdm = tmpStmm == 4 ? tmpd - 2 : tmpStmm == 5 ? tmpd - 2.5 : tmpStmm == 6 ? tmpd - 3 : tmpStmm == 7 ? tmpd - 3.5 : tmpd - 4;

            kv["Sumdm"] = "(dm) " + Smart(tmpdm);
            kv["SumdmTol"] = (tmpStmm == 4 ? "- 0.190" : tmpStmm == 5 ? "- 0.212" : tmpStmm == 6 ? "- 0.236" : tmpStmm == 7 ? "- 0.250" : "- 0.265") + " [3F]";
            kv["SumdmTolN"] = (tmpStmm == 4 ? "- 0.630" : tmpStmm == 5 ? "- 0.710" : tmpStmm == 6 ? "- 0.800" : tmpStmm == 7 ? "- 0.850" : "- 0.950") + " [3F]";

            double tmpd3 = tmpStmm == 4 ? tmpdm - 2.5 : tmpStmm == 5 ? tmpdm - 3 : tmpStmm == 6 ? tmpdm - 4 : tmpStmm == 7 ? tmpdm - 4.5 : tmpdm < 1300 ? tmpdm - 5 : tmpdm - 4.5;

            kv["Sumd3OP1"] = "(d3) " + Smart(tmpd3);
            kv["Sumd3"] = "(d3) " + Smart(tmpd3);
            kv["Sumd3OP1Tol"] = " 0";
            kv["Sumd3OP1TolN"] = "- 0.5";
            kv["Sumd3Tol"] = " 0";
            kv["Sumd3TolN"] = tmpStmm == 4 ? "- 0.750" : tmpStmm == 5 ? "- 0.850" : tmpStmm == 6 ? "- 0.950" : tmpStmm == 7 ? "- 1.000" : "- 1.120";

            double tmpHM = (tmpdOP1 - tmpd3) / 2;

            kv["SumSläppning"] = "(S) Max: 10mm";
            kv["SumHM"] = "(HM) " + Smart(tmpHM);
            kv["SumHMTol"] = " 0";
            kv["SumHMTolN"] = "- 0.25";

            string tmpg = tmpd < 301 ? "2.7x45º" : tmpd < 501 ? "3.2x45º" : tmpd < 671 ? "3.8x45º" : tmpd < 901 ? "4.4x45º" : "5x45º";

            kv["Sumg"] = "(g) " + tmpg;
            kv["Sumr1"] = EqualsI(LotusText(tmpBet1), "ASA") ? "R 3.5" : tmpSerie == "30" || tmpSerie == "31" || tmpSerie == "32" || tmpSerie == "39" ? tmpd < 421 ? "R 1" : "R 2.5" : tmpd < 421 ? "R 1" : "R 2.5";
            kv["Sumr"] = tmpd < 320 ? "R 2.5" : tmpd < 530 ? "R 3.5" : tmpd < 710 ? "R 5.5" : "R 7.5";

            kv["SumGVarTol"] = (tmpd > 1000 ? "0.050" : tmpd > 800 ? "0.045" : tmpd > 630 ? "0.040" : tmpd > 500 ? "0.035" : tmpd > 315 ? "0.030" : tmpd > 250 ? "0.025" : tmpd > 180 ? "0.020" : tmpd > 120 ? "0.015" : tmpd > 50 ? "0.010" : "0.008") + " [2F]";

            double tmpRakA = (tmpd < 101 ? 8 : tmpd < 281 ? 10 : tmpd < 481 ? 12 : tmpd < 601 ? 14 : tmpd < 901 ? 16 : 20) / 1000.0;
            double tmpRakB = (tmpd < 101 ? 12 : tmpd < 281 ? 15 : tmpd < 481 ? 18 : tmpd < 601 ? 21 : tmpd < 901 ? 24 : 30) / 1000.0;

            kv["SumRakA"] = "Max: " + Smart(tmpRakA);
            kv["SumRakB"] = "Max: " + Smart(tmpRakB);

            kv["SumOrund"] = (tmpd1 < 31 ? "0.026" : tmpd1 < 51 ? "0.031" : tmpd1 < 81 ? "0.037" : tmpd1 < 121 ? "0.043" : tmpd1 < 181 ? "0.050" : tmpd1 < 251 ? "0.057" : tmpd1 < 316 ? "0.065" : tmpd1 < 401 ? "0.070" : tmpd1 < 501 ? "0.077" : tmpd1 < 631 ? "0.087" : tmpd1 < 801 ? "0.100" : tmpd1 < 1001 ? "0.115" : "0.130") + " [3F]";

            double tmpML = tmpL - tmpb - 4;

            kv["SumML"] = "Mätlängd=" + (tmpML < 110 ? "75" : "100");

            kv["SumRa"] = "2.5 [2F]";
            kv["SumRa1"] = "2.5 [2F]";
            kv["SumRa5"] = "5 [2F]";

            double tmpVML = tmpML < 110 ? 75 : 100;
            double tmpVinkTol = ((tmpd > 1000 ? 0.09 : tmpd > 800 ? 0.10 : tmpd > 630 ? 0.11 : tmpd > 500 ? 0.12 : tmpd > 400 ? 0.13 : tmpd > 180 ? 0.15 : tmpd > 150 ? 0.18 : tmpd > 120 ? 0.30 : tmpd > 80 ? 0.45 : tmpd > 50 ? 0.50 : 0.60) * tmpVML) / 1000;

            kv["SumVinkTol"] = TrimZeros(tmpVinkTol) + " [2F]";

            double tmp8 = Round3(((tmpd - tmpd1) / 2) + ((tmpL - 1 - aMatt - 8) / (2 * tmpKona)));
            double tmp83 = Round3(((tmpd - tmpd1) / 2) + ((tmpL - 1 - aMatt - 83) / (2 * tmpKona)));
            double tmp108 = Round3(((tmpd - tmpd1) / 2) + ((tmpL - 1 - aMatt - 108) / (2 * tmpKona)));
            double tmp40 = Round3(((tmpd - tmpd1) / 2) + ((tmpL - 1 - aMatt - 40) / (2 * tmpKona)));
            double tmp140 = Round3(((tmpd - tmpd1) / 2) + ((tmpL - 1 - aMatt - 140) / (2 * tmpKona)));

            kv["SumL2"] = tmpML < 145 ? "8" : "40";
            kv["SumL1"] = tmpML < 110 ? "83" : tmpML < 145 ? "108" : "140";
            kv["SumE1"] = Smart(tmpML < 110 ? tmp83 : tmpML < 145 ? tmp108 : tmp140);
            kv["SumE2"] = Smart(tmpML < 145 ? tmp8 : tmp40);

            string tmpBygel = tmpML < 145 ? "SR 7419471" : "SR 7415991";

            kv["SumBygGtj"] = tmpBygel;
            kv["SumBygGVar"] = tmpBygel;
            kv["SumBygVinkTol"] = tmpBygel;

            bool skepp6 = EqualsI(mv, "Skepp 6");
            string tmpMaskinVal = skepp6 ? "Klämhylsa " : "";
            string tmpMaskinValS1 = skepp6 ? "Morando " : "";
            string tmpMaskinValS2 = skepp6 ? "Morando " : "";
            string tmpMaskinValS3 = skepp6 ? "1150 " : "";
            string tmpMaskinValS4 = skepp6 ? "1150 " : "";

            kv["SumMaskinValS1"] = "Maskin: " + tmpMaskinValS1 + tmpMaskinVal + " OP1";
            kv["SumMaskinValS2"] = "Maskin: " + tmpMaskinValS2 + tmpMaskinVal + " OP2";
            kv["SumMaskinValS3"] = "Maskin: " + tmpMaskinValS3 + tmpMaskinVal + " OP3";
            kv["SumMaskinValS4"] = "Maskin: " + tmpMaskinValS4 + tmpMaskinVal + " OP4";

            kv["SumF1_1"] = skepp6 ? "1/1" : "";
            kv["SumF1_2"] = skepp6 ? "1/1" : "";
            kv["SumF1_3"] = skepp6 ? "1/1" : "";
            kv["SumF1_4"] = skepp6 ? "1/1" : "";
            kv["SumF1_5"] = skepp6 ? "1/1" : "";

            kv["SumF2_1"] = skepp6 ? "1/5" : "";
            kv["SumF2_2"] = skepp6 ? "1/2" : "";
            kv["SumF2_3"] = skepp6 ? "1/1" : "";

            kv["SumF3_1"] = skepp6 ? "1/1" : "";
            kv["SumF3_2"] = skepp6 ? "1/1" : "";
            kv["SumF3_3"] = skepp6 ? "1/1" : "";
            kv["SumF3_4"] = skepp6 ? "1/1" : "";
            kv["SumF3_5"] = skepp6 ? "1/1" : "";
            kv["SumF3_6"] = skepp6 ? "Inst." : "";
            kv["SumF3_7"] = skepp6 ? "1/2" : "";
            kv["SumF3_8"] = skepp6 ? "1/2" : "";

            kv["SumF4_1"] = skepp6 ? "1/1" : "";
            kv["SumF4_2"] = skepp6 ? "1/1" : "";
            kv["SumF4_3"] = skepp6 ? "1/1" : "";
            kv["SumF4_4"] = skepp6 ? "1/1" : "";
            kv["SumF4_5"] = skepp6 ? "1/1" : "";
            kv["SumF4_6"] = skepp6 ? "Inst." : "";
            kv["SumF4_7"] = skepp6 ? "1/1" : "";
            kv["SumF4_8"] = skepp6 ? "1/1" : "";

            kv["SumD1_1"] = skepp6 ? "Djupmått" : "";
            kv["SumD1_2"] = skepp6 ? "Djupmått" : "";
            kv["SumD1_3"] = skepp6 ? "Skjutmått" : "";
            kv["SumD1_4"] = skepp6 ? "Skjutmått/Mikrometer" : "";
            kv["SumD1_5"] = skepp6 ? "Skjutmått/inv. Mikrometer" : "";

            kv["SumD2_1"] = skepp6 ? "Skjutmått" : "";
            kv["SumD2_2"] = skepp6 ? "Skjutmått" : "";
            kv["SumD2_3"] = skepp6 ? "Skjutmått" : "";

            kv["SumD3_1"] = skepp6 ? "Mikrometerstickmått" : "";
            kv["SumD3_2"] = skepp6 ? "Skjutmått" : "";
            kv["SumD3_3"] = skepp6 ? "Mätbygel " + kv["SumBygGtj"] : "";
            kv["SumD3_4"] = skepp6 ? "Mätbygel " + kv["SumBygGVar"] : "";
            kv["SumD3_5"] = skepp6 ? "Mätbygel " + kv["SumBygVinkTol"] : "";
            kv["SumD3_6"] = skepp6 ? "Mätmaskin" : "";
            kv["SumD3_7"] = skepp6 ? "Egglinjal" : "";
            kv["SumD3_8"] = skepp6 ? "Egglinjal" : "";

            kv["SumD4_1"] = skepp6 ? "Skjutmått" : "";
            kv["SumD4_2"] = skepp6 ? "Skjutmått" : "";
            kv["SumD4_3"] = skepp6 ? "Multimar med " + kv["SumRullar"] + " rullar" : "";
            kv["SumD4_4"] = skepp6 ? "Djupmått" : "";
            kv["SumD4_5"] = skepp6 ? "Radielyra" : "";
            kv["SumD4_6"] = skepp6 ? "Skjutmått/Vinkelmätare" : "";
            kv["SumD4_7"] = skepp6 ? "Gängmall " + kv["SumP"] : "";
            kv["SumD4_8"] = skepp6 ? "Skjutmått" : "";

            kv["SumAF1_1"] = "";
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = "";
            kv["SumAF1_6"] = "";

            kv["SumAF2_1"] = "";
            kv["SumAF2_2"] = "";
            kv["SumAF2_3"] = "";

            kv["SumAF3_1"] = "";
            kv["SumAF3_2"] = "";
            kv["SumAF3_3"] = skepp6 ? "Tol:" : "";
            kv["SumAF3_4"] = skepp6 ? "Tol: " + kv["SumGVarTol"] : "";
            kv["SumAF3_5"] = skepp6 ? "Tol: " + kv["SumVinkTol"] + " " + kv["SumML"] : "";
            kv["SumAF3_6"] = skepp6 ? "Max: " + kv["SumOrund"] : "";
            kv["SumAF3_7"] = skepp6 ? kv["SumRakA"] : "";
            kv["SumAF3_8"] = skepp6 ? kv["SumRakB"] : "";

            double tmpAF4_1 = tmpL - (tmpLTolN / 2);

            kv["SumAF4_1"] = skepp6 ? "Mått mitt i tolerans " + Smart(tmpAF4_1) : "";
            kv["SumAF4_2"] = "";
            kv["SumAF4_3"] = skepp6 ? "Kontrolleras med klove utf.2" : "";
            kv["SumAF4_4"] = "";
            kv["SumAF4_5"] = "";
            kv["SumAF4_6"] = "";
            kv["SumAF4_7"] = "";
            kv["SumAF4_8"] = skepp6 ? "Hjälpmått" : "";

            kv["SumRSS"] = "Rensvarvas";
            kv["SumTextS1"] = "Gjutgodsdefekter: 7433015";
            kv["SumTextS2"] = "Bryt alla kanter";
            kv["SumTextS3"] = "Bryt alla kanter, vid misstänkt formfel lämnas hylsan till mätrum";
            kv["SumTextS4"] = "";

            string tmpRitningsnr = tmpSerie == "30" ? "7438957": tmpSerie == "31" ? "7438958" : tmpSerie == "32" ? "7438955"
                : tmpSerie == "39" ? "7434032": tmpSerie == "241" ? "7432903" : "Styckritning, samma som typ";

            kv["SumRitningsnrS1"] = tmpRitningsnr;
            kv["SumRitningsnrS2"] = tmpRitningsnr;
            kv["SumRitningsnrS3"] = tmpRitningsnr;
            kv["SumRitningsnrS4"] = tmpRitningsnr;
            kv["SumRitTolS2"] = "TOL: 1432012";
            kv["SumRitTolS3"] = kv["SumRitTolS2"];
            kv["SumRitTolS4"] = kv["SumRitTolS2"];
            kv["SumRitGänga"] = "Gänga: 237359, 7430181";

            string tmpKlEgenskaper = @"PRODUCTION NUTS & SLEEVES & HOUSINGSALLMÄNKLASSADE EGENSKAPERKlassade egenskaper klämhylsor";

            kv["SumKlEgenskaperS3"] = tmpKlEgenskaper;
            kv["SumKlEgenskaperS4"] = tmpKlEgenskaper;

            string chuckback1Raw = GetString(bm, "Chuckbackar_OP1");
            string chuckbackRaw = GetString(bm, "Chuckbackar");

            kv["SumChuckback1"] = IsZeroOrEmpty(chuckback1Raw) ? "" : chuckback1Raw;
            kv["SumCB1"] = IsZeroOrEmpty(chuckback1Raw) ? "" : "Chuckbackar:";

            kv["SumChuckback"] = IsZeroOrEmpty(chuckbackRaw) ? "" : chuckbackRaw;
            kv["SumCB"] = IsZeroOrEmpty(chuckbackRaw) ? "" : "Chuckbackar:";

            string stodback1Raw = GetString(bm, "Stödbackar_OP1");
            string stodbackRaw = GetString(bm, "Stödbackar");

            kv["SumStödback1"] = IsZeroOrEmpty(stodback1Raw) ? "" : stodback1Raw + " mm";
            kv["SumSB1"] = IsZeroOrEmpty(stodback1Raw) ? "" : "Stödbackar:";

            kv["SumStödback"] = IsZeroOrEmpty(stodbackRaw) ? "" : stodbackRaw + " mm";
            kv["SumSB"] = IsZeroOrEmpty(stodbackRaw) ? "" : "Stödbackar:";

            string grader1Raw = GetString(bm, "Grader_OP1");
            string graderRaw = GetString(bm, "Grader");

            kv["SumGrader1"] = IsZeroOrEmpty(grader1Raw) ? "" : grader1Raw + " mm";
            kv["SumGR1"] = IsZeroOrEmpty(grader1Raw) ? "" : "Grader:";

            kv["SumGrader"] = IsZeroOrEmpty(graderRaw) ? "" : graderRaw + " mm";
            kv["SumGR"] = IsZeroOrEmpty(graderRaw) ? "" : "Grader:";

            string varvtal1Raw = GetString(bm, "Varvtal_OP1");
            string varvtalRaw = GetString(bm, "Varvtal");

            kv["SumVarv1"] = IsZeroOrEmpty(varvtal1Raw) ? "" : varvtal1Raw + " /min";
            kv["SumVR1"] = IsZeroOrEmpty(varvtal1Raw) ? "" : "Varvtal Plan/Stick:";

            kv["SumVarv"] = IsZeroOrEmpty(varvtalRaw) ? "" : varvtalRaw + " /min";
            kv["SumVR"] = IsZeroOrEmpty(varvtalRaw) ? "" : "Varvtal:";

            string matPlan1Raw = GetString(bm, "Matning Plan_OP1");
            string matPlanRaw = GetString(bm, "Matning Plan");

            kv["SumMatPl1"] = IsZeroOrEmpty(matPlan1Raw) ? "" : matPlan1Raw + " /min";
            kv["SumMP1"] = IsZeroOrEmpty(matPlan1Raw) ? "" : "Matning Plan/Stick:";

            kv["SumMatPl"] = IsZeroOrEmpty(matPlanRaw) ? "" : matPlanRaw + " /min";
            kv["SumMP"] = IsZeroOrEmpty(matPlanRaw) ? "" : "Matning Plan:";

            string matUtvInv1Raw = GetString(bm, "Matning Utv/Inv_OP1");
            string matUtvInvRaw = GetString(bm, "Matning Utv/Inv");

            kv["SumMatInUt1"] = IsZeroOrEmpty(matUtvInv1Raw) ? "" : matUtvInv1Raw + " /min";
            kv["SumMIU1"] = IsZeroOrEmpty(matUtvInv1Raw) ? "" : "Matning Utv/Inv:";

            kv["SumMatInUt"] = IsZeroOrEmpty(matUtvInvRaw) ? "" : matUtvInvRaw + " /min";
            kv["SumMIU"] = IsZeroOrEmpty(matUtvInvRaw) ? "" : "Matning Utv/Inv:";

            string fmUtv1Raw = GetString(bm, "Färdigmått Utv_OP1");
            string utvLin1Raw = GetString(bm, "Linjal Utv_OP1");

            kv["SumFMUtv1"] = IsZeroOrEmpty(fmUtv1Raw) ? "" : EqualsI(fmUtv1Raw, "1") ? Smart(tmpdOP1) + " -0.3 mm" : fmUtv1Raw;
            kv["SumFMU1"] = IsZeroOrEmpty(fmUtv1Raw) ? "" : "Utvändig diameter:";
            kv["SumUtvLin1"] = IsZeroOrEmpty(utvLin1Raw) ? "" : utvLin1Raw + " mm";
            kv["SumUL1"] = IsZeroOrEmpty(utvLin1Raw) ? "" : "Motsvarar på linjal:";

            string fmUtvRaw = GetString(bm, "Färdigmått Utv");
            string utvLinRaw = GetString(bm, "Linjal Utv");

            kv["SumFMUtv"] = IsZeroOrEmpty(fmUtvRaw) ? "" : EqualsI(fmUtvRaw, "1") ? Smart(tmpd2OP2) + " -0.5 mm" : fmUtvRaw;
            kv["SumFMU"] = IsZeroOrEmpty(fmUtvRaw) ? "" : "Utvändig diameter:";
            kv["SumUtvLin"] = IsZeroOrEmpty(utvLinRaw) ? "" : utvLinRaw + " mm";
            kv["SumUL"] = IsZeroOrEmpty(utvLinRaw) ? "" : "Motsvarar på linjal:";

            string fmInv1Raw = GetString(bm, "Färdigmått Inv_OP1");
            string invLin1Raw = GetString(bm, "Linjal Inv_OP1");

            kv["SumFMInv1"] = IsZeroOrEmpty(fmInv1Raw) ? "" : EqualsI(fmInv1Raw, "1") ? Smart(tmpd1OP1) + " +0.5 mm" : fmInv1Raw;
            kv["SumFMI1"] = IsZeroOrEmpty(fmInv1Raw) ? "" : "Invändig diameter:";
            kv["SumInvLin1"] = IsZeroOrEmpty(invLin1Raw) ? "" : invLin1Raw + " mm";
            kv["SumIL1"] = IsZeroOrEmpty(invLin1Raw) ? "" : "Motsvarar på linjal:";

            string fmInvRaw = GetString(bm, "Färdigmått Inv");
            string invLinRaw = GetString(bm, "Linjal Inv");

            kv["SumFMInv"] = IsZeroOrEmpty(fmInvRaw) ? "" : kv["SumFMInv1"];
            kv["SumFMI"] = IsZeroOrEmpty(fmInvRaw) ? "" : "Invändig diameter:";
            kv["SumInvLin"] = IsZeroOrEmpty(invLinRaw) ? "" : kv["SumInvLin1"];
            kv["SumIL"] = IsZeroOrEmpty(invLinRaw) ? "" : "Motsvarar på linjal:";

            bool hasSetupOP1 = !IsZeroOrEmpty(chuckback1Raw) || !IsZeroOrEmpty(stodback1Raw) || !IsZeroOrEmpty(grader1Raw) ||
                !IsZeroOrEmpty(varvtal1Raw) ||!IsZeroOrEmpty(matPlan1Raw) || !IsZeroOrEmpty(matUtvInv1Raw);

            bool hasSetupOP2 = !IsZeroOrEmpty(chuckbackRaw) || !IsZeroOrEmpty(stodbackRaw) || !IsZeroOrEmpty(graderRaw) ||
                !IsZeroOrEmpty(varvtalRaw) || !IsZeroOrEmpty(matPlanRaw) || !IsZeroOrEmpty(matUtvInvRaw);

            kv["SumIN1"] = hasSetupOP1 ? "Inställning" : "";
            kv["SumIN"] = hasSetupOP2 ? "Inställning" : "";

            bool hasFinishedOP1 =!IsZeroOrEmpty(fmUtv1Raw) ||!IsZeroOrEmpty(utvLin1Raw) ||
                !IsZeroOrEmpty(fmInv1Raw) || !IsZeroOrEmpty(invLin1Raw);

            bool hasFinishedOP2 = !IsZeroOrEmpty(fmUtvRaw) || !IsZeroOrEmpty(utvLinRaw) ||
                !IsZeroOrEmpty(fmInvRaw) || !IsZeroOrEmpty(invLinRaw);

            kv["SumFM1"] = hasFinishedOP1 ? "Färdigmått" : "";
            kv["SumFM"] = hasFinishedOP2 ? "Färdigmått" : "";

            return kv;
        }

        private static string GetString(List<Bookmark> bookmarks, string key)
        {
            if (bookmarks == null || string.IsNullOrWhiteSpace(key)) return "";

            for (int i = 0; i < bookmarks.Count; i++)
            {
                Bookmark bookmark = bookmarks[i];

                if (bookmark != null && string.Equals(bookmark.BookmarkName ?? "", key, StringComparison.OrdinalIgnoreCase))
                    return bookmark.BookmarkValue ?? "";
            }

            return "";
        }

        private static double GetDouble(List<Bookmark> bookmarks, string key)
        {
            string raw = GetString(bookmarks, key);

            if (string.IsNullOrWhiteSpace(raw)) return 0;

            return double.TryParse(raw.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double value) ? value : 0;
        }

        private static bool IsZeroOrEmpty(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return true;

            string normalized = value.Trim().Replace(",", ".");

            return double.TryParse(normalized, NumberStyles.Any, CultureInfo.InvariantCulture, out double number)
                ? Math.Abs(number) < 0.0000001 : EqualsI(normalized, "0");
        }

        private static bool IsNumeric(string value) => !string.IsNullOrWhiteSpace(value) &&
            double.TryParse(value.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out _);

        private static double ParseDouble(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return 0;

            return double.TryParse(value.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double result)
                ? result : 0;
        }
        private static string TrimZeros(double value) => value.ToString("0.###", CultureInfo.InvariantCulture);

        private static string Smart(double value) => Math.Abs(value % 1) < 0.0000001 ? value.ToString("F0", CultureInfo.InvariantCulture)
                : value.ToString("0.###", CultureInfo.InvariantCulture).Replace(".", ",");

        private static string F3(double value) => value.ToString("F3", CultureInfo.InvariantCulture).Replace(",", ".");

        private static double Round3(double value) => Math.Round(value, 3, MidpointRounding.AwayFromZero);

        private static double TruncateToFactor(double value, double factor) => factor == 0 ? value : Math.Truncate(value / factor) * factor;
        
        private static double LotusRound(double value, double factor)
        {
            if (factor == 0) return value;
            return Math.Round(value / factor, 0, MidpointRounding.AwayFromZero) * factor;
        }
        private static string LotusText(object value)
        {
            if (value == null) return "";
            if (value is double doubleValue) return Smart(doubleValue);
            if (value is float floatValue) return Smart(floatValue);
            if (value is decimal decimalValue) return Smart((double)decimalValue);

            return value.ToString() ?? "";
        }

        private static string Left(string value, int length) => string.IsNullOrEmpty(value) || length <= 0 ? ""
            : value.Substring(0, Math.Min(length, value.Length));

        private static string Right(string value, int length) => string.IsNullOrEmpty(value) || length <= 0 ? ""
                : value.Length <= length ? value : value.Substring(value.Length - length);

        private static bool EqualsI(string a, string b) => string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);

        private static bool ContainsI(string source, string value) =>!string.IsNullOrEmpty(source) &&
            !string.IsNullOrEmpty(value) && source.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;

        private static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published)) return "";
            if (!DateTime.TryParse(published, out DateTime publishDate)) return "";

            DateTime validTo = publishDate.AddDays(14);

            if (DateTime.Today > validTo.Date) return "";

            return "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)" + LB + LB + "" + LB + LB
                   + "Popupruta aktiv till "  + validTo.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
    }
}