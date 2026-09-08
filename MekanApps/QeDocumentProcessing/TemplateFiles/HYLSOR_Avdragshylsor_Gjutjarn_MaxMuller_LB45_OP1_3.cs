using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Avdragshylsor_Gjutjarn_MaxMuller_LB45_OP1_3 : ITemplateCalculations
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

            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : "";
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : "";

            int cntB2 = tmpBet2.Length;

            string tmpSerie;
            if (tmpSlash && (cntB2 == 3 || cntB2 == 2)) tmpSerie = tmpBet2;
            else if (cntB2 > 4) tmpSerie = Left(tmpBet2, 3);
            else if (cntB2 == 3) tmpSerie = Left(tmpBet2, 1);
            else tmpSerie = Left(tmpBet2, 2);

            string tmpTyp = !tmpSlash ? Right(tmpBet2, 2) : tmpBet3;

            double serie = TryParseDouble(tmpSerie);
            double typ = TryParseDouble(tmpTyp);

            double tmpL = GetDouble(bm, "Längd (L)", "Längd", "L");
            double tmpda = GetDouble(bm, "Kona lillände diameter (d)", "Kona lillände diameter", "KonaLillände", "d");
            double tmpÄS = GetFlag(bm, "Äldre standard", "ÄldreStandard");
            double tmpd1 = GetDouble(bm, "Innerdiameter (d1)", "IDia", "d1");
            double tmpd2 = GetDouble(bm, "Ytterdiameter (d2)", "YDia", "d2");
            double tmpb = GetDouble(bm, "Längd till gänga (b)", "Gänglängd (b)", "b");
            double konaBm = GetDouble(bm, "Kona");
            double amatt = GetDouble(bm, "a-mått", "Amått");
            double tmpd4 = GetDouble(bm, "Släppning (d4)", "Släppning", "d4");
            double tmph = GetDouble(bm, "Bredd släppning (h)", "Bredd släppning", "h");

            int tmpStmm = tmpd2 < 301 ? 4 : tmpd2 < 501 ? 5 : tmpd2 < 701 ? 6 : tmpd2 < 901 ? 7 : 8;

            kv["SumGänga"] = "Tr" + FmtComma(tmpd2) + "x" + tmpStmm;
            string tmpP = "Tr" + tmpStmm;
            kv["SumP"] = "(P) " + tmpStmm;
            string tmpRullar = tmpP;

            double tmpKona = konaBm == 0 ? (In(serie, 240, 241) ? 30 : 12) : konaBm;
            kv["SumV"] = tmpKona == 30 ? "0\u00ba57" : "2\u00ba23";
            string sumKona = "Kona 1:" + FmtComma(tmpKona);
            kv["SumKona"] = sumKona;
            kv["SumKonaOP1"] = sumKona;
            kv["SumKonaOP2"] = sumKona;

            double tmpGTjTol = tmpKona == 12
                ? (tmpda < 31 ? 0.033
                 : tmpda < 11 ? 0.039
                 : tmpda < 81 ? 0.046
                 : tmpda < 121 ? 0.054
                 : tmpda < 181 ? 0.063
                 : tmpda < 251 ? 0.072
                 : tmpda < 316 ? 0.081
                 : tmpda < 401 ? 0.089
                 : tmpda < 501 ? 0.097
                 : tmpda < 631 ? 0.105
                 : tmpda < 801 ? 0.115
                 : tmpda < 1001 ? 0.130
                 : 0.145)
                : (tmpda < 121 ? 0.035
                 : tmpda < 181 ? 0.040
                 : tmpda < 251 ? 0.046
                 : tmpda < 316 ? 0.052
                 : tmpda < 401 ? 0.057
                 : tmpda < 501 ? 0.063
                 : tmpda < 631 ? 0.068
                 : tmpda < 801 ? 0.076
                 : tmpda < 1001 ? 0.084
                 : 0.095);

            string sumGTjTol = "+ " + FmtDotWithoutLeadingZero(tmpGTjTol) + " [3F]";
            string sumGTjTolN = "- 0 [2F]";
            kv["SumGTjTol"] = sumGTjTol;
            kv["SumGTjTolN"] = sumGTjTolN;
            kv["SumGTjTol2"] = sumGTjTol;
            kv["SumGTjTolN2"] = sumGTjTolN;

            double tmpd = RoundTo(tmpda + (tmpKona == 0 ? 0 : amatt / tmpKona), 0.001);
            double tmpdOP2 = RoundTo(tmpd + 1 + tmpGTjTol, 0.01);
            kv["SumdOP2"] = "(d) " + FmtDot(tmpdOP2);
            kv["SumdOP2Tol"] = "\u00b1 0.2";

            double tmpbOP2 = tmpb + 1.3;
            kv["SumbOP2"] = "(b) " + FmtComma(tmpbOP2);
            kv["SumbTolOP2"] = "\u00b1 0.3";
            kv["Sumb"] = "(b) " + FmtComma(tmpb);
            kv["SumbTol"] = (tmpb < 11 ? "\u00b1 0.290"
                           : tmpb < 19 ? "\u00b1 0.350"
                           : tmpb < 31 ? "\u00b1 0.420"
                           : tmpb < 51 ? "\u00b1 0.500"
                           : tmpb < 81 ? "\u00b1 0.600"
                           : tmpb < 121 ? "\u00b1 0.700"
                           : tmpb < 181 ? "\u00b1 0.800"
                           : tmpb < 251 ? "\u00b1 0.925"
                           : tmpb < 316 ? "\u00b1 1.050"
                           : tmpb < 401 ? "\u00b1 1.150"
                           : tmpb < 501 ? "\u00b1 1.250"
                           : tmpb < 631 ? "\u00b1 1.400"
                           : tmpb < 801 ? "\u00b1 1.600"
                           : "\u00b1 1.800") + " [3F]";

            string sumg1 = tmpStmm == 8 ? "5.3x45\u00ba"
                         : tmpStmm == 7 ? "4.4x45\u00ba"
                         : tmpStmm == 6 ? "3.8x45\u00ba"
                         : tmpStmm == 5 ? "3.2x45\u00ba"
                         : tmpStmm == 4 ? "2.7x45\u00ba"
                         : tmpStmm == 3 ? "2.4x45\u00ba"
                         : tmpStmm == 2 ? "1.7x45\u00ba"
                         : "1.3x45\u00ba";
            kv["Sumg1"] = sumg1;

            string sumg2;
            if (In(serie, 22, 240, 241, 30)) sumg2 = sumg1;
            else if (tmpÄS == 1) sumg2 = sumg1;
            else if (serie == 31) sumg2 = tmpd2 < 510 ? "3.25x45\u00ba" : sumg1;
            else if (serie == 32)
                sumg2 = tmpd2 < 510 ? "3.25x45\u00ba"
                      : tmpd2 == 600 ? "3.8x45\u00ba"
                      : tmpd2 < 700 ? "4x45\u00ba"
                      : tmpd2 < 790 ? "4.5x45\u00ba"
                      : tmpd2 < 940 ? "4.4x45\u00ba"
                      : tmpd2 == 950 ? "5.5x45\u00ba"
                      : tmpd2 == 1000 ? "5.5x45\u00ba"
                      : tmpd2 == 1060 ? "5.3x45\u00ba"
                      : sumg1;
            else sumg2 = sumg1;
            kv["Sumg2"] = sumg2;

            double tmpmd = tmpStmm == 4 ? tmpd2 - 2
                         : tmpStmm == 5 ? tmpd2 - 2.5
                         : tmpStmm == 6 ? tmpd2 - 3
                         : tmpStmm == 7 ? tmpd2 - 3.5
                         : tmpd2 - 4;
            kv["Summd"] = "(md) " + FmtComma(tmpmd);
            kv["SummdTol"] = (tmpStmm == 4 ? "- 0.190"
                            : tmpStmm == 5 ? "- 0.212"
                            : tmpStmm == 6 ? "- 0.236"
                            : tmpStmm == 7 ? "- 0.250"
                            : "- 0.265") + " [3F]";
            kv["SummdTolN"] = (tmpStmm == 4 ? "- 0.630"
                             : tmpStmm == 5 ? "- 0.710"
                             : tmpStmm == 6 ? "- 0.800"
                             : tmpStmm == 7 ? "- 0.850"
                             : "- 0.950") + " [3F]";

            bool tmpd1OP1Halv = serie == 241 && In(typ, 500, 96);
            bool isMaxMuller = EqualsI(mv, "MaxMuller");
            bool isLB45 = EqualsI(mv, "LB45");

            string tmpd1OP1 = isMaxMuller ? FmtComma(tmpd1 - 3) : isLB45 ? FmtComma(tmpd1 - 1) : "";

            kv["Sumd1OP1"] = isMaxMuller
                ? ((tmpL < 300 || tmpd1OP1Halv) ? "(d1) " + tmpd1OP1 : "Körs i OP 2")
                : "Körs i OP 2";
            kv["Sumd1OP1Tol"] = isMaxMuller
                ? ((tmpL < 300 || tmpd1OP1Halv) ? "\u00b1 0.2" : "")
                : "";
            kv["Sumd1OP2"] = isMaxMuller
                ? (tmpL < 300 ? "Körs i OP 1" : "(d1) " + tmpd1OP1)
                : "(d1) " + tmpd1OP1;
            kv["Sumd1OP2Tol"] = isMaxMuller
                ? (tmpL < 300 ? "" : "\u00b1 0.2")
                : "\u00b1 0.2";
            kv["Sumd1"] = "(d1) " + FmtComma(tmpd1);
            kv["Sumd1Tol"] = (tmpd1 < 31 ? "\u00b1 0.026"
                            : tmpd1 < 51 ? "\u00b1 0.031"
                            : tmpd1 < 81 ? "\u00b1 0.037"
                            : tmpd1 < 121 ? "\u00b1 0.043"
                            : tmpd1 < 181 ? "\u00b1 0.050"
                            : tmpd1 < 251 ? "\u00b1 0.057"
                            : tmpd1 < 316 ? "\u00b1 0.065"
                            : tmpd1 < 401 ? "\u00b1 0.070"
                            : tmpd1 < 501 ? "\u00b1 0.077"
                            : tmpd1 < 631 ? "\u00b1 0.087"
                            : tmpd1 < 801 ? "\u00b1 0.100"
                            : tmpd1 < 1001 ? "\u00b1 0.115"
                            : "\u00b1 0.130") + " [3F]";

            string sumd2 = "(d2) " + FmtComma(tmpd2);
            string sumd2TolN = tmpStmm == 4 ? "- 0.300"
                             : tmpStmm == 5 ? "- 0.335"
                             : tmpStmm == 6 ? "- 0.375"
                             : tmpStmm == 7 ? "- 0.425"
                             : "- 0.450";
            kv["Sumd2"] = sumd2;
            kv["Sumd22"] = sumd2;
            kv["Sumd2Tol"] = "+ 0";
            kv["Sumd2TolN"] = sumd2TolN;
            kv["Sumd22Tol"] = "+ 0";
            kv["Sumd22TolN"] = sumd2TolN;

            double tmpd3 = tmpStmm == 4 ? tmpmd - 2.5
                         : tmpStmm == 5 ? tmpmd - 3
                         : tmpStmm == 6 ? tmpmd - 4
                         : tmpStmm == 7 ? tmpmd - 4.5
                         : (tmpmd < 1300 ? tmpmd - 5 : tmpmd - 4.5);
            kv["Sumd3"] = "(d3) " + FmtComma(tmpd3);
            kv["Sumd3Tol"] = "+ 0";
            kv["Sumd3TolN"] = tmpStmm == 4 ? "- 0.750"
                            : tmpStmm == 5 ? "- 0.850"
                            : tmpStmm == 6 ? "- 0.950"
                            : tmpStmm == 7 ? "- 1.000"
                            : "- 1.120";

            kv["Sumd4"] = "(d4) " + FmtComma(tmpd4);
            kv["Sumd4Tol"] = "+ 0";
            kv["Sumd4TolN"] = serie == 22 ? "- 0.2" : In(serie, 240, 241) ? "- 0.25" : "- 0.3";
            kv["Sumh"] = "(h) " + FmtComma(tmph);
            kv["SumhTol"] = "\u00b1 0.2";

            double tmpd6 = RoundTo((tmpKona == 0 ? 0 : (tmpb - tmph) / tmpKona) + tmpdOP2, 0.01);
            kv["Sumd6"] = "(d6) " + FmtComma(tmpd6);
            kv["Sumd6Tol"] = "\u00b1 0.2";

            bool tmpLPlus5mm = serie == 241 && typ == 500;
            double tmpLOP1 = tmpLPlus5mm ? tmpL + 5 : tmpL + 4;
            kv["SumLOP1"] = "(L) min: " + FmtComma(tmpLOP1);
            double tmpLOP2 = tmpL + 0.2;
            kv["SumLOP2"] = "(L) " + FmtComma(tmpLOP2);
            kv["SumLOP2Tol"] = "\u00b1 0.25";
            kv["SumL"] = "(L) " + FmtComma(tmpL);
            kv["SumLTol"] = "+ 0 [3F]";
            kv["SumLTolN"] = (tmpL < 11 ? "- 0.220"
                            : tmpL < 19 ? "- 0.270"
                            : tmpL < 31 ? "- 0.330"
                            : tmpL < 51 ? "- 0.390"
                            : tmpL < 81 ? "- 0.460"
                            : tmpL < 121 ? "- 0.540"
                            : tmpL < 181 ? "- 0.630"
                            : tmpL < 251 ? "- 0.720"
                            : tmpL < 316 ? "- 0.810"
                            : tmpL < 401 ? "- 0.890"
                            : tmpL < 501 ? "- 0.970"
                            : tmpL < 631 ? "- 1.100"
                            : tmpL < 801 ? "- 1.250"
                            : "- 1.400") + " [3F]";

            double tmpg = tmpL - tmpb - 0.3;
            kv["Sumg"] = "(g) " + FmtComma(tmpg);
            kv["SumgTol"] = "\u00b1 0.3";

            kv["SumrOP1"] = "R 1.5";
            kv["Sumr1"] = "R 1.5";
            double tmpR = tmpd < 125 ? 50 : 60;
            kv["SumR"] = "R " + FmtComma(tmpR);
            kv["Sumr1OP1"] = In(serie, 240, 241, 30) ? ""
                           : serie == 31 ? (In(tmpd2, 200, 420, 440, 460, 480, 500) ? "R max 1.2" : "")
                           : serie == 32 ? (tmpÄS == 1 ? ""
                                          : tmpd2 > 330 ? "R max 1.2"
                                          : In(tmpd2, 170, 180, 190, 200) ? "R max 1.2"
                                          : "")
                           : "";
            double tmpe = tmpR == 50 ? 5 : 6;
            kv["Sume"] = "(e) " + FmtComma(tmpe);

            kv["SumRa"] = "2.5 [2F]";
            kv["SumRa1"] = "2.5 [2F]";
            kv["SumRa2"] = "2.5 [2F]";
            kv["SumRa5"] = "5";

            string sumGVarTol = (tmpd < 51 ? "+ 0.008"
                               : tmpd < 121 ? "+ 0.010"
                               : tmpd < 181 ? "+ 0.015"
                               : tmpd < 251 ? "+ 0.020"
                               : tmpd < 316 ? "+ 0.025"
                               : tmpd < 501 ? "+ 0.030"
                               : tmpd < 631 ? "+ 0.035"
                               : tmpd < 801 ? "+ 0.040"
                               : tmpd < 1001 ? "+ 0.045"
                               : "+ 0.050") + " [2F]";
            kv["SumGVarTol"] = sumGVarTol;

            string tmpKs = (tmpd < 51 ? "+ 0.040"
                          : tmpd < 121 ? "+ 0.050"
                          : tmpd < 251 ? "+ 0.060"
                          : tmpd < 316 ? "+ 0.070"
                          : tmpd < 401 ? "+ 0.080"
                          : tmpd < 501 ? "+ 0.090"
                          : tmpd < 631 ? "+ 0.100"
                          : tmpd < 801 ? "+ 0.120"
                          : tmpd < 1001 ? "+ 0.140"
                          : "+ 0.160") + " [2F]";

            string tmpRd = tmpd1 < 31 ? "0.026"
                         : tmpd1 < 51 ? "0.031"
                         : tmpd1 < 81 ? "0.037"
                         : tmpd1 < 121 ? "0.043"
                         : tmpd1 < 181 ? "0.050"
                         : tmpd1 < 251 ? "0.057"
                         : tmpd1 < 316 ? "0.065"
                         : tmpd1 < 401 ? "0.070"
                         : tmpd1 < 501 ? "0.077"
                         : tmpd1 < 631 ? "0.087"
                         : tmpd1 < 801 ? "0.100"
                         : tmpd1 < 1001 ? "0.115"
                         : "0.130";
            kv["SumRd"] = tmpRd;

            string sumVTol = (tmpd < 51 ? "\u00b1 0.060"
                            : tmpd < 81 ? "\u00b1 0.050"
                            : tmpd < 121 ? "\u00b1 0.045"
                            : tmpd < 151 ? "\u00b1 0.030"
                            : tmpd < 181 ? "\u00b1 0.018"
                            : tmpd < 401 ? "\u00b1 0.015"
                            : tmpd < 501 ? "\u00b1 0.013"
                            : tmpd < 631 ? "\u00b1 0.012"
                            : tmpd < 801 ? "\u00b1 0.011"
                            : tmpd < 1001 ? "\u00b1 0.010"
                            : "\u00b1 0.009") + " [2F]";
            kv["SumVTol"] = sumVTol;

            string tmpRakA = FmtComma((tmpd < 101 ? 8 : tmpd < 281 ? 10 : tmpd < 481 ? 12 : tmpd < 601 ? 14
                           : tmpd < 901 ? 16 : 20) / 1000.0);
            string sumRakA = "Max: " + tmpRakA;
            kv["SumRakA"] = sumRakA;
            string tmpRakB = FmtComma((tmpd < 101 ? 12 : tmpd < 281 ? 15 : tmpd < 481 ? 18 : tmpd < 601 ? 21
                           : tmpd < 901 ? 24 : 30) / 1000.0);
            string sumRakB = "Max: " + tmpRakB;
            kv["SumRakB"] = sumRakB;

            double sumML = tmpb - tmph;
            double konaX2 = 2.0 * tmpKona;
            double Bygel(double x) => RoundTo(((tmpda - tmpd1) / 2.0) + (konaX2 == 0 ? 0 : (amatt + x) / konaX2), 0.001);
            double tmp8 = Bygel(8);
            double tmp108 = Bygel(108);
            double tmp40 = Bygel(40);
            double tmp140 = Bygel(140);

            double sumL1 = sumML < 145 ? 8 : 40;
            double sumL2 = sumML < 145 ? 108 : 140;
            double sumE1 = sumML < 145 ? tmp8 : tmp40;
            double sumE2 = sumML < 145 ? tmp108 : tmp140;

            kv["SumML"] = FmtComma(sumML);
            kv["SumL1"] = FmtComma(sumL1);
            kv["SumL2"] = FmtComma(sumL2);
            kv["SumE1"] = FmtComma(sumE1);
            kv["SumE2"] = FmtComma(sumE2);

            string tmpBygGtj = sumML < 145 ? "SR 7419471" : "SR 7415991";
            string tmpBygGVar = sumML < 145 ? "SR 7419471" : "SR 7415991";
            string tmpBygVinkTol = sumML < 145 ? "SR 7419471" : "SR 7415991";

            string tmpRitnr = serie == 22 ? "7437361"
                            : serie == 240 ? (tmpÄS == 0 ? (tmpd < 379 ? "7437370" : "7437371") : (tmpd < 379 ? "232631" : "232632"))
                            : serie == 241 ? (tmpÄS == 0 ? (tmpd < 379 ? "7437372" : "7437373") : "232636")
                            : serie == 30 ? (tmpd < 529 ? (tmpÄS == 0 ? "7437362" : "234385") : "7437363")
                            : serie == 31 ? (tmpÄS == 0 ? "7437364" : "231282")
                            : serie == 32 ? (tmpÄS == 0 ? "7437366" : "231638")
                            : "Styckritning, samma som typ";

            string sumRitnr = "Produktritning: " + tmpRitnr;
            kv["SumRitnrS1"] = sumRitnr;
            kv["SumRitnrS2"] = sumRitnr;
            kv["SumRitnrS3"] = sumRitnr;
            kv["SumRitTol"] = "TOL: 1432011";
            kv["SumRitTolS3"] = "TOL: 1432011";
            kv["SumRitGänga"] = "Gänga: 237359, 7430181";

            string tmpKlEgenskaper = "PRODUCTION NUTS & SLEEVES & HOUSINGSALLMÄNKLASSADE EGENSKAPERKlassade egenskaper Avdragshylsor";
            kv["SumKlEgenskaper"] = tmpKlEgenskaper;
            kv["SumKlEgenskaperS3"] = tmpKlEgenskaper;

            string tmpMV = isMaxMuller ? "MV1" : isLB45 ? "MV2" : "";
            kv["SumMaskinValS1"] = "Maskin: " + mv + " OP1";
            kv["SumMaskinValS2"] = "Maskin: " + mv + " OP2";
            kv["SumMaskinValS3"] = "Maskin: " + mv + " OP3";

            bool mOK = tmpMV == "MV1" || tmpMV == "MV2";

            kv["SumF1_1"] = mOK ? "1/5" : "";
            kv["SumF1_2"] = mOK ? ((tmpL < 300 || tmpd1OP1Halv) ? "1/2" : "-") : "";
            kv["SumF1_3"] = mOK ? "1/2" : "";
            kv["SumF1_4"] = mOK ? "1/1" : "";
            kv["SumF1_5"] = mOK ? "1/2" : "";
            kv["SumF1_6"] = mOK ? "1/2" : "";
            kv["SumF1_7"] = mOK ? "1/2" : "";
            kv["SumF1_8"] = mOK ? "1/5" : "";
            kv["SumF1_9"] = mOK ? "1/Skift" : "";
            kv["SumF1_0"] = mOK ? "1/Skift" : "";

            kv["SumF2_1"] = mOK ? "1/2" : "";
            kv["SumF2_2"] = mOK ? "1/2" : "";
            kv["SumF2_3"] = mOK ? "Inst." : "";
            kv["SumF2_4"] = mOK ? (tmpL < 300 ? "-" : "1/2") : "";

            kv["SumF3_1"] = mOK ? "1/1" : "";
            kv["SumF3_2"] = mOK ? "1/2" : "";
            kv["SumF3_3"] = mOK ? "1/5" : "";
            kv["SumF3_4"] = mOK ? "1/1" : "";
            kv["SumF3_5"] = mOK ? "1/1" : "";
            kv["SumF3_6"] = mOK ? "1/1" : "";
            kv["SumF3_7"] = mOK ? "Inst." : "";
            kv["SumF3_8"] = mOK ? "1/5" : "";
            kv["SumF3_9"] = mOK ? "1/5" : "";
            kv["SumF3_0"] = mOK ? "Inst." : "";

            kv["SumD1_1"] = mOK ? "Skjutmått" : "";
            kv["SumD1_2"] = mOK ? ((tmpL < 300 || tmpd1OP1Halv) ? "Skjutmått" : "-") : "";
            kv["SumD1_3"] = mOK ? "Skjutmått" : "";
            kv["SumD1_4"] = mOK ? "Multimar med " + tmpRullar + " rullar" : "";
            kv["SumD1_5"] = mOK ? "Skjutmått / Djupmått" : "";
            kv["SumD1_6"] = mOK ? "Skjutmått" : "";
            kv["SumD1_7"] = mOK ? "Skjutmått" : "";
            kv["SumD1_8"] = mOK ? "Skjutmått / Vinkelmätare" : "";
            kv["SumD1_9"] = mOK ? "Gängmall " + tmpP : "";
            kv["SumD1_0"] = mOK ? "Ytjämnhetsmätare" : "";

            kv["SumD2_1"] = mOK ? "Skjutmått" : "";
            kv["SumD2_2"] = mOK ? "Skjutmått" : "";
            kv["SumD2_3"] = mOK ? "Skjutmått/mikrometer" : "";
            kv["SumD2_4"] = mOK ? (tmpL < 300 ? "-" : "Skjutmått") : "";

            kv["SumD3_1"] = mOK ? "Mikrometer/Skjutmått" : "";
            kv["SumD3_2"] = mOK ? "Skjutmått" : "";
            kv["SumD3_3"] = mOK ? "Djupmått/Skjutmått" : "";
            kv["SumD3_4"] = mOK ? "Mätbygel: " + tmpBygGtj : "";
            kv["SumD3_5"] = mOK ? "Mätbygel: " + tmpBygGVar : "";
            kv["SumD3_6"] = mOK ? "Mätbygel: " + tmpBygVinkTol : "";
            kv["SumD3_7"] = mOK ? "Mätmaskin" : "";
            kv["SumD3_8"] = mOK ? "Egglinjal" : "";
            kv["SumD3_9"] = mOK ? "Egglinjal" : "";
            kv["SumD3_0"] = mOK ? "Mätmaskin" : "";

            kv["SumAF1_1"] = "";
            kv["SumAF1_2"] = mOK ? (tmpd1OP1Halv ? "Körs delvis" : (tmpL < 300 ? "" : "Körs i OP 2")) : "";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = mOK ? "Kontrolleras med klove utf. 2" : "";
            kv["SumAF1_5"] = "";
            kv["SumAF1_6"] = "";
            kv["SumAF1_7"] = mOK ? "Hjälpmått" : "";
            kv["SumAF1_8"] = "";
            kv["SumAF1_9"] = "";
            kv["SumAF1_0"] = "";

            kv["SumAF2_1"] = "";
            kv["SumAF2_2"] = "";
            kv["SumAF2_3"] = "";
            kv["SumAF2_4"] = mOK ? (tmpd1OP1Halv ? "Körs delvis" : (tmpL < 300 ? "Körs i OP 1" : "")) : "";

            kv["SumAF3_1"] = "";
            kv["SumAF3_2"] = "";
            kv["SumAF3_3"] = "";
            kv["SumAF3_4"] = mOK ? sumGTjTol + "<<LineBreak>>" + sumGTjTolN : "";
            kv["SumAF3_5"] = mOK ? "Tol: " + sumGVarTol : "";
            kv["SumAF3_6"] = mOK ? "Tol: " + sumVTol : "";
            kv["SumAF3_7"] = mOK ? "Max: " + tmpRd : "";
            kv["SumAF3_8"] = mOK ? sumRakA : "";
            kv["SumAF3_9"] = mOK ? sumRakB : "";
            kv["SumAF3_0"] = mOK ? "Max: " + tmpKs : "";

            string sumText = "Grader, frifläckar, slagmärken, repor, valkar & andra defekter samt ojämnheter kontrolleras visuellt.";
            kv["SumTextS1"] = sumText;
            kv["SumTextS2"] = sumText;
            kv["SumTextS3"] = sumText;

            return kv;
        }

        private static string ComputePopup(string pub)
        {
            if (string.IsNullOrWhiteSpace(pub)) return "";
            DateTime dt; if (!DateTime.TryParse(pub, out dt)) return "";
            DateTime till = dt.AddDays(14);
            return DateTime.Today <= till.Date
                ? "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)<<LineBreak>>Information om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument<<LineBreak>>Popupruta aktiv till " + till.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                : "";
        }

        private static string Left(string s, int n) =>
            string.IsNullOrEmpty(s) ? "" : (s.Length <= n ? s : s.Substring(0, n));

        private static string Right(string s, int n) =>
            string.IsNullOrEmpty(s) ? "" : (s.Length <= n ? s : s.Substring(s.Length - n));

        private static bool In(double val, params double[] list)
        {
            if (list == null) return false;
            for (int i = 0; i < list.Length; i++)
                if (list[i] == val) return true;
            return false;
        }

        private static bool EqualsI(string a, string b) =>
            string.Equals((a ?? "").Trim(), (b ?? "").Trim(), StringComparison.OrdinalIgnoreCase);

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

        private static double GetFlag(List<Bookmark> bm, params string[] keys)
        {
            string s = (GetString(bm, keys) ?? "").Trim();
            if (s.Length == 0) return 0;
            if (EqualsI(s, "ja") || EqualsI(s, "yes") || EqualsI(s, "true") || EqualsI(s, "x")) return 1;
            if (EqualsI(s, "nej") || EqualsI(s, "no") || EqualsI(s, "false")) return 0;
            return TryParseDouble(s);
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
            Math.Round(v, 3, MidpointRounding.AwayFromZero).ToString("0.################", CommonFunctions.Culture).Replace(".", ",");

        private static string FmtDot(double v) =>
            Math.Round(v, 3, MidpointRounding.AwayFromZero).ToString("0.################", CommonFunctions.Culture).Replace(",", ".");
        private static string FmtDotWithoutLeadingZero(double v) =>
            Math.Round(v, 3, MidpointRounding.AwayFromZero).ToString("0.################", CommonFunctions.Culture).Replace(",", ".").Replace("0.", ".");

        private static string Fmt3(double v) =>
            v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
    }
}