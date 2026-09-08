using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Klamhylsor_64_84_OP1_3_Gjutjarn : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string mv = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;
            List<Bookmark> bm = req != null ? req.Bookmarks : null;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = subject.ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");
            bool tmpSlash = tmpBet.IndexOf('/') >= 0;
            bool tmpLW = tmpBet.IndexOf("LW", StringComparison.OrdinalIgnoreCase) >= 0;

            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : "";
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : "";

            int cntB2 = tmpBet2.Length;
            int cntB3 = tmpBet3.Length;

            string tmpSerie;
            if (tmpLW) tmpSerie = "0";
            else if (tmpSlash && (cntB2 == 3 || cntB2 == 2)) tmpSerie = tmpBet2;
            else if (cntB2 > 4) tmpSerie = Left(tmpBet2, 3);
            else if (cntB2 == 3) tmpSerie = Left(tmpBet2, 1);
            else tmpSerie = Left(tmpBet2, 2);

            string tmpTyp = !tmpSlash ? Right(tmpBet2, 2) : (cntB3 > 4 ? Right(tmpBet2, 2) : tmpBet3);

            bool serieKona12 = IsMember(tmpSerie, "30", "31", "32", "39");
            bool serie240 = EqualsI(tmpSerie, "240");

            string tmpKon0 = "0.000";
            string tmpKon01 = "0.100";
            string tmpKon02 = "0.200";
            string tmpKon03 = "0.300";
            string tmpKon05 = "0.500";

            string avvikD = GetString(bm, "Avvikande YDia Gänga (d)", "AvvikandeYDia", "YDia");
            double tmpd = (string.IsNullOrWhiteSpace(avvikD) || avvikD.Trim() == "0")
                ? ((!tmpSlash || cntB3 > 4) ? (TryParseDouble(tmpTyp) / 2.0) * 10.0 : TryParseDouble(tmpBet3))
                : TryParseDouble(avvikD);

            double tmpd1 = GetDouble(bm, "Innerdiameter (d1)", "IDia");
            string d2Str = GetString(bm, "Kona storände diameter (d2)", "d2");
            double tmpd2 = TryParseDouble(d2Str);
            double tmpb = GetDouble(bm, "Gänglängd (b)", "Gänglängd");
            double tmpL = GetDouble(bm, "Längd (L)", "Längd");
            double amatt = GetDouble(bm, "a-mått", "Amått");
            double konaBm = GetDouble(bm, "Kona");
            double lOP1Bm = GetDouble(bm, "Längd (L) OP1", "LängdOP1");

            int tmpStmm = tmpd < 301 ? 4 : tmpd < 501 ? 5 : tmpd < 701 ? 6 : tmpd < 901 ? 7 : 8;

            kv["SumGänga"] = "Tr" + FmtComma(tmpd) + "x" + tmpStmm;
            kv["SumP"] = "Tr" + tmpStmm;
            kv["SumP1"] = "(P) " + tmpStmm;
            string sumRullar = tmpStmm + "mm";
            kv["SumRullar"] = sumRullar;

            double tmpKona = konaBm == 0 ? (serieKona12 ? 12 : 30) : konaBm;
            kv["SumV"] = tmpKona == 30 ? "(V) 0\u00ba57" : "(V) 2\u00ba23";
            kv["SumKonaOP1"] = "Kona 1:" + FmtComma(tmpKona);
            kv["SumKona"] = "Kona 1:" + FmtComma(tmpKona);

            double tmpKL = tmpL + 5 - tmpb + 1;
            kv["SumKL"] = "(KL) " + FmtComma(tmpKL);
            kv["SumKLTol"] = "+ " + tmpKon0;
            kv["SumKLTolN"] = "- " + " " + tmpKon01;

            double tmpLOP1 = lOP1Bm == 0 ? tmpL + 3.5 : lOP1Bm;
            kv["SumLOP1"] = "(L) " + FmtComma(tmpLOP1);
            kv["SumLOP1Tol"] = "+ " + tmpKon0;
            kv["SumLOP1TolN"] = "- " + " " + tmpKon05;

            double tmpLOP2 = tmpL - 0.5;
            kv["SumLOP2"] = "(L) " + FmtComma(tmpLOP2);
            kv["SumLOP2Tol"] = "\u00b1 0.25";

            kv["SumL"] = "(L) " + FmtComma(tmpL);
            kv["SumLTol"] = "+ " + tmpKon0 + " [3F]";
            string tmpLTolN = Fmt3(tmpL < 11 ? 0.58 : tmpL < 19 ? 0.7 : tmpL < 31 ? 0.84 : tmpL < 51 ? 1
                             : tmpL < 81 ? 1.2 : tmpL < 121 ? 1.4 : tmpL < 181 ? 1.6 : tmpL < 251 ? 1.85
                             : tmpL < 316 ? 2.1 : tmpL < 401 ? 2.3 : 2.5);
            kv["SumLTolN"] = "- " + " " + tmpLTolN + " [3F]";

            kv["Sumb"] = "(b) " + FmtComma(tmpb);
            string tmpbTol = Fmt3(tmpb < 11 ? 1.5 : tmpb < 19 ? 1.8 : tmpb < 31 ? 2.1 : tmpb < 51 ? 2.5
                           : tmpb < 81 ? 3 : tmpb < 121 ? 3.5 : 4);
            kv["SumbTol"] = "+ " + tmpbTol + "  [3F]";
            kv["SumbTolN"] = "- " + " " + tmpKon0 + " [2F]";

            double tmpSL = tmpb + 4;
            kv["SumSL"] = "(SL) " + FmtComma(tmpSL);
            kv["SumSLTol"] = "\u00b1 " + tmpKon03;

            bool isMaxMuller = EqualsI(mv, "MaxMuller");
            bool isLB45 = EqualsI(mv, "LB45");
            bool mOK = isMaxMuller || isLB45;

            double tmpd1OP1 = tmpd1 - 1;
            kv["Sumd1OP1"] = isMaxMuller ? "(d1) " + FmtComma(tmpd1OP1) : "Körs i OP2";
            kv["Sumd1OP1Tol"] = isMaxMuller ? "\u00b1 " + tmpKon02 : "";
            kv["Sumd1OP2"] = isMaxMuller ? "Körs i OP1" : "(d1) " + FmtComma(tmpd1OP1);
            kv["Sumd1OP2Tol"] = isMaxMuller ? "" : "\u00b1 " + tmpKon02;

            kv["Sumd1"] = "(d1) " + FmtComma(tmpd1);
            string tmpd1Tol = Fmt3(tmpd1 < 31 ? 0.026 : tmpd1 < 51 ? 0.031 : tmpd1 < 81 ? 0.037
                            : tmpd1 < 121 ? 0.043 : tmpd1 < 181 ? 0.05 : tmpd1 < 251 ? 0.057
                            : tmpd1 < 316 ? 0.065 : tmpd1 < 401 ? 0.07 : tmpd1 < 501 ? 0.077
                            : tmpd1 < 631 ? 0.087 : tmpd1 < 801 ? 0.1 : tmpd1 < 1001 ? 0.115 : 0.13);
            kv["Sumd1Tol"] = "\u00b1 " + tmpd1Tol + " [3F]";

            string sumdOP1 = "(d) " + FmtComma(tmpd);
            string tmpdOP1TolN = Fmt3(tmpStmm == 4 ? 0.3 : tmpStmm == 5 ? 0.335 : tmpStmm == 6 ? 0.375
                               : tmpStmm == 7 ? 0.425 : 0.45);
            string sumdOP1TolN = "- " + " " + tmpdOP1TolN;
            kv["SumdOP1"] = sumdOP1;
            kv["SumdOP1Tol"] = "+ " + tmpKon0;
            kv["SumdOP1TolN"] = sumdOP1TolN;
            kv["SumdaOP2"] = sumdOP1;
            kv["SumdaOP2Tol"] = "+ " + tmpKon0;
            kv["SumdaOP2TolN"] = sumdOP1TolN;
            kv["Sumd"] = "(d) " + FmtComma(tmpd);
            kv["SumdTol"] = "+ " + tmpKon0;
            kv["SumdTolN"] = sumdOP1TolN;

            double tmpdm = tmpStmm == 4 ? tmpd - 2 : tmpStmm == 5 ? tmpd - 2.5 : tmpStmm == 6 ? tmpd - 3
                         : tmpStmm == 7 ? tmpd - 3.5 : tmpd - 4;
            kv["Sumdm"] = "(dm) " + FmtComma(tmpdm);
            string tmpdmTol = Fmt3(tmpStmm == 4 ? 0.19 : tmpStmm == 5 ? 0.212 : tmpStmm == 6 ? 0.236
                            : tmpStmm == 7 ? 0.25 : 0.265);
            kv["SumdmTol"] = "- " + " " + tmpdmTol + " [3F]";
            string tmpdmTolN = Fmt3(tmpStmm == 4 ? 0.63 : tmpStmm == 5 ? 0.71 : tmpStmm == 6 ? 0.8
                             : tmpStmm == 7 ? 0.85 : 0.95);
            kv["SumdmTolN"] = "- " + " " + tmpdmTolN + " [3F]";

            double tmpd3 = tmpStmm == 4 ? tmpdm - 2.5 : tmpStmm == 5 ? tmpdm - 3 : tmpStmm == 6 ? tmpdm - 4
                         : tmpStmm == 7 ? tmpdm - 4.5 : (tmpdm < 1300 ? tmpdm - 5 : tmpdm - 4.5);
            kv["Sumd3"] = "(d3) " + FmtComma(tmpd3);
            kv["Sumd3Tol"] = "+ " + tmpKon0;
            string tmpd3TolN = Fmt3(tmpStmm == 4 ? 0.75 : tmpStmm == 5 ? 0.85 : tmpStmm == 6 ? 0.95
                             : tmpStmm == 7 ? 1 : 1.12);
            kv["Sumd3TolN"] = "- " + " " + tmpd3TolN;

            string tmpg = tmpd < 301 ? "2.7x45\u00ba" : tmpd < 501 ? "3.2x45\u00ba" : tmpd < 671 ? "3.8x45\u00ba"
                        : tmpd < 901 ? "4.4x45\u00ba" : "5x45\u00ba";
            kv["Sumg"] = "(g) " + tmpg;
            kv["Sumr1"] = serieKona12 ? (tmpd < 421 ? "R 1" : "R 2.5")
                        : serie240 ? (tmpd < 421 ? "R 1.5" : "R 2.5") : "";
            string sumr = tmpd < 320 ? "R 2.5" : tmpd < 530 ? "R 3.5" : tmpd < 710 ? "R 5.5" : "R 7.5";
            kv["Sumr"] = sumr;
            kv["Sumr2"] = sumr;

            string tmpGTjTol = tmpKona == 12
                ? Fmt3(tmpd > 1000 ? 0.095 : tmpd > 800 ? 0.085 : tmpd > 630 ? 0.075 : tmpd > 500 ? 0.070
                     : tmpd > 400 ? 0.065 : tmpd > 315 ? 0.060 : tmpd > 250 ? 0.055 : tmpd > 180 ? 0.050
                     : tmpd > 120 ? 0.040 : tmpd > 80 ? 0.035 : tmpd > 50 ? 0.030 : tmpd > 30 ? 0.025 : 0.020)
                : tmpKona == 30
                ? Fmt3(tmpd > 1000 ? 0.060 : tmpd > 800 ? 0.055 : tmpd > 630 ? 0.050 : tmpd > 500 ? 0.045
                     : tmpd > 400 ? 0.040 : tmpd > 315 ? 0.035 : tmpd > 250 ? 0.035 : tmpd > 180 ? 0.030
                     : tmpd > 120 ? 0.025 : tmpd > 80 ? 0.022 : tmpd > 50 ? 0.019 : tmpd > 30 ? 0.016 : 0.013)
                : "Fel Kona";
            string tmpGTjTolN = tmpKona == 12
                ? Fmt3(tmpd > 1000 ? 0.280 : tmpd > 800 ? 0.250 : tmpd > 630 ? 0.225 : tmpd > 500 ? 0.200
                     : tmpd > 400 ? 0.190 : tmpd > 315 ? 0.175 : tmpd > 250 ? 0.160 : tmpd > 180 ? 0.140
                     : tmpd > 120 ? 0.120 : tmpd > 80 ? 0.105 : tmpd > 50 ? 0.090 : tmpd > 30 ? 0.075 : 0.070)
                : tmpKona == 30
                ? Fmt3(tmpd > 1000 ? 0.170 : tmpd > 800 ? 0.155 : tmpd > 630 ? 0.140 : tmpd > 500 ? 0.125
                     : tmpd > 400 ? 0.115 : tmpd > 315 ? 0.105 : tmpd > 251 ? 0.095 : tmpd > 180 ? 0.085
                     : tmpd > 120 ? 0.075 : tmpd > 80 ? 0.065 : tmpd > 50 ? 0.055 : tmpd > 30 ? 0.046 : 0.039)
                : "Fel Kona";
            string sumGTjTol = "+ " + tmpGTjTol;
            string sumGTjTolN = "- " + " " + tmpGTjTolN;
            kv["SumGTjTol"] = sumGTjTol;
            kv["SumGTjaTol"] = sumGTjTol;
            kv["SumGTjTolN"] = sumGTjTolN;
            kv["SumGTjaTolN"] = sumGTjTolN;

            double tmpd2OP2 = tmpd2 + 1;
            kv["Sumd2OP2"] = "(d2) " + FmtComma(tmpd2OP2);
            kv["Sumd2OP2Tol"] = "\u00b1 " + tmpKon02;
            string tmpd2a = (string.IsNullOrWhiteSpace(d2Str) || d2Str.Trim() == "0")
                ? FmtComma(RoundTo(((tmpL - amatt) / tmpKona) + tmpd, 0.01))
                : d2Str.Trim();
            kv["Sumd2"] = "(d2) " + tmpd2a;

            string sumGVarTol = (tmpd > 1000 ? "0.050" : tmpd > 800 ? "0.045" : tmpd > 630 ? "0.040"
                              : tmpd > 500 ? "0.035" : tmpd > 315 ? "0.030" : tmpd > 250 ? "0.025"
                              : tmpd > 180 ? "0.020" : tmpd > 120 ? "0.015" : tmpd > 50 ? "0.010" : "0.008") + " [2F]";
            kv["SumGVarTol"] = sumGVarTol;

            string tmpRakA = FmtComma((tmpd < 101 ? 8 : tmpd < 281 ? 10 : tmpd < 481 ? 12 : tmpd < 601 ? 14
                           : tmpd < 901 ? 16 : 20) / 1000.0);
            string sumRakA = "Max: " + tmpRakA;
            string tmpRakB = FmtComma((tmpd < 101 ? 12 : tmpd < 281 ? 15 : tmpd < 481 ? 18 : tmpd < 601 ? 21
                           : tmpd < 901 ? 24 : 30) / 1000.0);
            string sumRakB = "Max: " + tmpRakB;
            kv["SumRakA"] = sumRakA;
            kv["SumRakB"] = sumRakB;

            string sumOrund = (tmpd1 < 31 ? "0.026" : tmpd1 < 51 ? "0.031" : tmpd1 < 81 ? "0.037"
                            : tmpd1 < 121 ? "0.043" : tmpd1 < 181 ? "0.050" : tmpd1 < 251 ? "0.057"
                            : tmpd1 < 316 ? "0.065" : tmpd1 < 401 ? "0.070" : tmpd1 < 501 ? "0.077"
                            : tmpd1 < 631 ? "0.087" : tmpd1 < 801 ? "0.100" : tmpd1 < 1001 ? "0.115" : "0.130") + " [3F]";
            kv["SumOrund"] = sumOrund;

            double tmpML = tmpL - tmpb - 4;
            string sumML = "Mätlängd=" + (tmpML < 110 ? "75" : "100");
            kv["SumML"] = sumML;

            kv["SumRa"] = "2.5 [2F]";
            kv["SumRa1"] = "2.5 [2F]";
            kv["SumRa5"] = "5 [2F]";

            double tmpVML = tmpML < 110 ? 75 : 100;
            double tmpVinkFaktor = tmpd > 1000 ? 0.09 : tmpd > 800 ? 0.10 : tmpd > 630 ? 0.11 : tmpd > 500 ? 0.12
                                 : tmpd > 400 ? 0.13 : tmpd > 180 ? 0.15 : tmpd > 150 ? 0.18 : tmpd > 120 ? 0.30
                                 : tmpd > 80 ? 0.45 : tmpd > 50 ? 0.50 : 0.60;
            string sumVinkTol = FmtDot((tmpVinkFaktor * tmpVML) / 1000.0) + " [2F]";
            kv["SumVinkTol"] = sumVinkTol;

            double Bygel(double x) => RoundTo(((tmpd - tmpd1) / 2.0) + ((tmpL - 1 - amatt - x) / (2.0 * tmpKona)), 0.001);
            double tmp5 = Bygel(5);
            double tmp80 = Bygel(80);
            double tmp8 = Bygel(8);
            double tmp83 = Bygel(83);
            double tmp108 = Bygel(108);
            double tmp40 = Bygel(40);
            double tmp140 = Bygel(140);

            bool is3968 = EqualsI(tmpBet2, "3968");
            double sumL2 = is3968 ? 5 : tmpML < 145 ? 8 : 40;
            double sumL1 = is3968 ? 80 : tmpML < 110 ? 83 : tmpML < 145 ? 108 : 140;
            double sumE1 = is3968 ? tmp80 : tmpML < 110 ? tmp83 : tmpML < 145 ? tmp108 : tmp140;
            double sumE2 = is3968 ? tmp5 : tmpML < 145 ? tmp8 : tmp40;
            kv["SumL2"] = FmtComma(sumL2);
            kv["SumL1"] = FmtComma(sumL1);
            kv["SumE1"] = FmtComma(sumE1);
            kv["SumE2"] = FmtComma(sumE2);

            string sumBygGtj = tmpML < 145 ? "SR 7419471" : "SR 7415991";
            string sumBygGVar = tmpML < 145 ? "SR 7419471" : "SR 7415991";
            string sumBygVinkTol = tmpML < 145 ? "SR 7419471" : "SR 7415991";
            kv["SumBygGtj"] = sumBygGtj;
            kv["SumBygGVar"] = sumBygGVar;
            kv["SumBygVinkTol"] = sumBygVinkTol;

            string tmpMaskinValS = isMaxMuller ? "MaxMuller" : isLB45 ? "LB45" : "";
            kv["SumMaskinValS1"] = "Maskin: " + tmpMaskinValS + " - OP1";
            kv["SumMaskinValS2"] = "Maskin: " + tmpMaskinValS + " - OP2";
            kv["SumMaskinValS3"] = "Maskin: " + tmpMaskinValS + " - OP3";

            kv["SumF1_1"] = mOK ? "1/5" : "";
            kv["SumF1_2"] = mOK ? "1/2" : "";
            kv["SumF1_3"] = mOK ? "1/1" : "";
            kv["SumF1_4"] = mOK ? "1/5" : "";
            kv["SumF1_5"] = mOK ? "Inst." : "";
            kv["SumF1_6"] = mOK ? "1/5" : "";
            kv["SumF1_7"] = mOK ? "1/2" : "";
            kv["SumF1_8"] = mOK ? "1/5" : "";

            kv["SumF2_1"] = mOK ? "1/1" : "";
            kv["SumF2_2"] = mOK ? "1/1" : "";
            kv["SumF2_3"] = mOK ? "1/1" : "";
            kv["SumF2_4"] = mOK ? "Inst" : "";

            kv["SumF3_1"] = mOK ? "1/2" : "";
            kv["SumF3_2"] = mOK ? "1/2" : "";
            kv["SumF3_3"] = mOK ? "1/1" : "";
            kv["SumF3_4"] = mOK ? "1/1" : "";
            kv["SumF3_5"] = mOK ? "1/1" : "";
            kv["SumF3_6"] = mOK ? "Inst." : "";
            kv["SumF3_7"] = mOK ? "1/5" : "";
            kv["SumF3_8"] = mOK ? "1/5" : "";

            kv["SumD1_1"] = mOK ? "Skjutmått" : "";
            kv["SumD1_2"] = mOK ? "Skjutmått" : "";
            kv["SumD1_3"] = mOK ? "Multimar med " + sumRullar + " rullar" : "";
            kv["SumD1_4"] = mOK ? "Djupmått" : "";
            kv["SumD1_5"] = mOK ? "Radielyra" : "";
            kv["SumD1_6"] = mOK ? "Skjutmått/Vinkelmätare" : "";
            kv["SumD1_7"] = mOK ? "Gängmall Tr" + tmpStmm : "";
            kv["SumD1_8"] = mOK ? "Skjutmått" : "";

            kv["SumD2_1"] = mOK ? "Skjutmått" : "";
            kv["SumD2_2"] = mOK ? "Skjutmått" : "";
            kv["SumD2_3"] = mOK ? "Radieyra" : "";
            kv["SumD2_4"] = "";

            kv["SumD3_1"] = mOK ? "Mikrometerstickmått" : "";
            kv["SumD3_2"] = mOK ? "Skjutmått" : "";
            kv["SumD3_3"] = mOK ? (is3968 ? "Mätbygel " + sumBygGtj + " Sh -3mm" : "Mätbygel " + sumBygGtj) : "";
            kv["SumD3_4"] = mOK ? (is3968 ? "Mätbygel " + sumBygGVar + " Sh -3mm" : "Mätbygel " + sumBygGVar) : "";
            kv["SumD3_5"] = mOK ? (is3968 ? "Mätbygel " + sumBygVinkTol + " Sh -3mm" : "Mätbygel " + sumBygVinkTol) : "";
            kv["SumD3_6"] = mOK ? "Mätmaskin" : "";
            kv["SumD3_7"] = mOK ? "Egglinjal" : "";
            kv["SumD3_8"] = mOK ? "Egglinjal" : "";

            kv["SumAF1_1"] = "";
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = mOK ? "Kontrolleras med klove utf.2" : "";
            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = "";
            kv["SumAF1_6"] = "";
            kv["SumAF1_7"] = "";
            kv["SumAF1_8"] = mOK ? "Hjälpmått" : "";

            kv["SumAF2_1"] = "";
            kv["SumAF2_2"] = "";
            kv["SumAF2_3"] = "";
            kv["SumAF2_4"] = mOK ? "Kona 1:" + FmtComma(tmpKona) : "";

            kv["SumAF3_1"] = "";
            kv["SumAF3_2"] = "";
            kv["SumAF3_3"] = mOK ? "Tol:" : "";
            kv["SumAF3_4"] = mOK ? "Tol: " + sumGVarTol : "";
            kv["SumAF3_5"] = mOK ? "Tol: " + sumVinkTol + " " + sumML : "";
            kv["SumAF3_6"] = mOK ? "Max: " + sumOrund : "";
            kv["SumAF3_7"] = mOK ? sumRakA : "";
            kv["SumAF3_8"] = mOK ? sumRakB : "";

            kv["SumTextS1"] = "Gjutgodsdefekter: 7433015";
            kv["SumTextS2"] = "";
            kv["SumTextS3"] = "Bryt alla kanter, avlägsna";

            string tmpRitningsnr = EqualsI(tmpSerie, "30") ? "7438957"
                                 : EqualsI(tmpSerie, "31") ? "7438958"
                                 : EqualsI(tmpSerie, "32") ? "7438955"
                                 : EqualsI(tmpSerie, "39") ? "7434032"
                                 : EqualsI(tmpSerie, "240") ? "7432901"
                                 : "Styckritning, samma som typ";
            kv["SumRitningsnrS1"] = tmpRitningsnr;
            kv["SumRitningsnrS2"] = tmpRitningsnr;
            kv["SumRitningsnrS3"] = tmpRitningsnr;
            kv["SumRitTolS1"] = "TOL: 1432012";
            kv["SumRitTolS2"] = "TOL: 1432012";
            kv["SumRitTolS3"] = "TOL: 1432012";
            kv["SumRitGänga"] = "Gänga: 237359, 7430181";

            string tmpKlEgenskaper = "PRODUCTION NUTS & SLEEVES & HOUSINGSALLMÄNKLASSADE EGENSKAPERKlassade egenskaper klämhylsor";
            kv["SumKlEgenskaperS1"] = "";
            kv["SumKlEgenskaperS2"] = tmpKlEgenskaper;
            kv["SumKlEgenskaperS3"] = tmpKlEgenskaper;

            return kv;
        }

        private static string ComputePopup(string pub)
        {
            if (string.IsNullOrWhiteSpace(pub)) return "";
            DateTime dt; if (!DateTime.TryParse(pub, out dt)) return "";
            DateTime till = dt.AddDays(14);
            return DateTime.Today <= till.Date
                ? "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)\n\nInformation om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument\n\nPopupruta aktiv till " + till.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                : "";
        }

        private static string Left(string s, int n) =>
            string.IsNullOrEmpty(s) ? "" : (s.Length <= n ? s : s.Substring(0, n));

        private static string Right(string s, int n) =>
            string.IsNullOrEmpty(s) ? "" : (s.Length <= n ? s : s.Substring(s.Length - n));

        private static bool IsMember(string val, params string[] list)
        {
            if (list == null) return false;
            for (int i = 0; i < list.Length; i++)
                if (string.Equals(list[i], val ?? "", StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        private static bool EqualsI(string a, string b) =>
            string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);

        private static double RoundTo(double v, double unit)
        {
            if (unit == 0) return v;
            return Math.Round(v / unit, MidpointRounding.AwayFromZero) * unit;
        }

        private static double TryParseDouble(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;
            double v;
            return double.TryParse(s.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static double GetDouble(List<Bookmark> bm, params string[] keys)
        {
            return TryParseDouble(GetString(bm, keys));
        }

        private static string GetString(List<Bookmark> bm, params string[] keys)
        {
            if (bm == null || keys == null) return "";
            for (int k = 0; k < keys.Length; k++)
            {
                string key = keys[k];
                if (string.IsNullOrWhiteSpace(key)) continue;
                for (int i = 0; i < bm.Count; i++)
                {
                    Bookmark b = bm[i];
                    if (b != null && string.Equals(b.BookmarkName ?? "", key, StringComparison.OrdinalIgnoreCase))
                    {
                        string val = b.BookmarkValue ?? "";
                        if (!string.IsNullOrWhiteSpace(val)) return val;
                    }
                }
            }
            return "";
        }

        private static string FmtComma(double v) =>
            v.ToString("0.################", CommonFunctions.Culture).Replace(".", ",");

        private static string FmtDot(double v) =>
            v.ToString("0.################", CommonFunctions.Culture).Replace(",", ".");

        private static string Fmt3(double v) =>
            v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
    }
}