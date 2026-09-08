using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_KILHYLSA_KOMPLETT_SPECIAL_3_EXTRA_HAL : ITemplateCalculations
    {
        private const string LB = "<<LineBreak>>";

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();
            string subject = req?.ProductDesignation ?? string.Empty;
            string maskinVal = req?.MachineNumber ?? string.Empty;
            List<Bookmark> bm = req?.Bookmarks;

            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");

            kv["VaLPopUp"] = ComputePopup(req?.Published);

            string[] tmpBetLista = Explode(tmpBet, new[] { ' ', '/', '.', '-' });
            string tmpBet1a = Word(tmpBetLista, 1);
            string tmpBet2a = Word(tmpBetLista, 2);
            string tmpBet3a = Word(tmpBetLista, 3);
            string tmpBet4a = Word(tmpBetLista, 4);
            string tmpBet5a = Word(tmpBetLista, 5);

            int tmpCountV21 = MemberIndex("V21", new[] { tmpBet1a, tmpBet2a, tmpBet3a, tmpBet4a, tmpBet5a });

            string tmpBet1 = Word(tmpBetLista, 1);
            string tmpBet2s = Word(tmpBetLista, 2);
            string tmpBet3s = Word(tmpBetLista, 3);
            string tmpBet4s = Word(tmpBetLista, 4);
            string tmpBet5s = Word(tmpBetLista, 5);

            string tmpBet3 = tmpCountV21 == 3 ? tmpBet3s : ToNumberString(tmpBet3s);
            string tmpBet4 = tmpCountV21 == 4 ? tmpBet4s : ToNumberString(tmpBet4s);
            string tmpBet5 = tmpCountV21 == 5 ? tmpBet5s : ToNumberString(tmpBet5s);

            bool tmpLU_LW = ContainsAny(tmpBet, new[] { "LU", "LW" });
            bool tmpSlash = tmpBet.Contains("/");
            bool tmpLU = tmpBet.Contains("LU");
            bool tmpMS = tmpBet.Contains("MS");
            bool tmpLW = tmpBet.Contains("LW");
            bool tmpV21 = tmpBet.Contains("V21");
            bool tmp237774 = tmpBet.Contains("237774");
            bool tmp7432987 = tmpBet.Contains("7432987");

            int tmpCount = (tmpBet2s ?? string.Empty).Length;
            bool tmpSpecial = (tmpBet2s ?? string.Empty).Length > 10;

            int tmpSerie = !tmpLU_LW ? ToInt(Left(tmpBet2s, 2)) : 0;
            int tmpTyp = 0;
            if (!tmpLU_LW)
            {
                if (tmpCount > 3)
                    tmpTyp = ToInt(Right(tmpBet2s, 2));
                else
                    tmpTyp = ToInt(tmpSlash ? tmpBet3 : tmpBet3);
            }

            int tmpTypIndex = 0;
            if (!tmpLU_LW)
            {
                if (tmpSerie == 38)
                    tmpTypIndex = MemberIndex(tmpTyp, Typ38Values);
                else if (tmpSerie == 39)
                    tmpTypIndex = MemberIndex(tmpTyp, Typ39Values);
            }

            double konaIn = GetDouble(bm, "Kona");
            double tmpKona = konaIn == 0 ? ((tmpSerie == 39 || tmpSerie == 38) ? 12 : 30) : konaIn;
            kv["SumKona"] = "Kona 1:" + FormatDot(tmpKona);

            double LIn = GetDouble(bm, "Längd (L)");
            double tmpL = LIn;
            if (tmpL == 0)
            {
                if (tmpSerie == 38)
                    tmpL = GetFromList(tmpTypIndex, L38Values);
                else if (tmpSerie == 39)
                    tmpL = GetFromList(tmpTypIndex, L39Values);
                else
                    tmpL = 0;
            }

            kv["SumL"] = "(L) " + FormatDot(tmpL);

            double tmpLTol = 0;
            double tmpLTolN = tmp7432987 ? 0.46 : H15NegTol(tmpL);
            kv["SumLTol"] = "+ " + FormatDot(tmpLTol);
            kv["SumLTolN"] = "- " + FormatDot3(tmpLTolN) + " [3F]";

            double tmpDa = GetDouble(bm, "Ytterdiameter lillkona (d)");
            double aM = GetDouble(bm, "a-mått");
            double tmpd = 0;
            if (tmpDa == 0)
            {
                if (tmpCount > 3)
                    tmpd = (tmpTyp / 2.0) * 10.0;
                else
                    tmpd = ParseDoubleOrZero(tmpBet3);
            }
            else
            {
                tmpd = tmpDa + (aM / tmpKona);
            }
            tmpd = RoundTo(tmpd, 0.001);
            kv["Sumd"] = "(d) " + FormatDot(tmpd);

            double d1In = GetDouble(bm, "Innerdiameter (d1)");
            double tmpd1;
            if (d1In == 0)
            {
                if (!tmpSpecial && (tmpCountV21 == 3 || tmpCountV21 == 4))
                {
                    if (tmpTyp < 61) tmpd1 = tmpd - 10;
                    else if (tmpTyp < 501) tmpd1 = tmpd - 15;
                    else if (tmpTyp < 671) tmpd1 = tmpd - 20;
                    else if (tmpTyp < 901) tmpd1 = tmpd - 25;
                    else tmpd1 = tmpd - 30;
                }
                else
                {
                    tmpd1 = tmpCount > 3 ? ParseDoubleOrZero(tmpBet3) : ParseDoubleOrZero(tmpBet4);
                }
            }
            else
            {
                tmpd1 = d1In;
            }

            kv["Sumd1"] = "(d1) " + FormatDot(tmpd1);
            double tmpd1Tol = tmp7432987 ? 0.078 : JS10Tol(tmpd1);
            kv["Sumd1Tol"] = "± " + FormatDot3(tmpd1Tol);

            bool tmpTolN_noll1 = tmpBet.Contains("7432987");
            bool tmpTolN_noll = ContainsAny(tmpBet, new[] { "7433833", "7432987" });

            string tmpGodstjocklekTol = ToTolStringGodstjocklekPlus(tmpTolN_noll1, tmpKona, tmpd, konaIn);
            string tmpGodstjocklekTolN = ToTolStringGodstjocklekMinus(tmpTolN_noll, tmpKona, tmpd, konaIn);

            kv["SumGodstjocklekTol"] = "+ " + tmpGodstjocklekTol;
            kv["SumGodstjocklekTolN"] = "- " + tmpGodstjocklekTolN;

            double tmpd2 = RoundTo((tmpL / tmpKona) + tmpd, 0.001);
            kv["Sumd2"] = "(d2) " + FormatDot(tmpd2);

            kv["SumRa25"] = "2.5";
            kv["SumRa25A"] = "2.5";
            kv["SumRa25a"] = "2.5";
            kv["SumRa5"] = "5";

            string sumR = tmpSerie == 39 ? "R2" : tmpSerie == 38 ? "R2" : tmpMS ? "R3" : "R1.5";
            kv["SumR"] = sumR;
            kv["Sum45"] = tmp7432987 ? "R1" : "45°";

            string ritSvarv = GetString(bm, "Ritningsnummer Svarv");
            string tmpRHA1 = (SelectByTmpd(tmpd, new[] { 100.1, 280.1, 480.1, 600.1, 900.1, 1250.1, 1600.1 }, new[] { 8, 10, 12, 14, 16, 20, 25 }, 30) / 1000.0).ToString(CommonFunctions.Culture);
            string tmpRHA = EqualsI(ritSvarv, "LU-7432987") ? "0.012" : tmpRHA1;
            kv["SumRHA"] = "max: " + ReplaceCommaDot(tmpRHA) + "[2F]";

            string tmpRHB1 = (SelectByTmpd(tmpd, new[] { 100.1, 280.1, 480.1, 600.1, 900.1, 1250.1, 1600.1 }, new[] { 12, 15, 18, 21, 24, 30, 37 }, 45) / 1000.0).ToString(CommonFunctions.Culture);
            string tmpRHB = EqualsI(ritSvarv, "LU-7432987") ? "0.018" : tmpRHB1;
            kv["SumRHB"] = "max: " + ReplaceCommaDot(tmpRHB) + "[2F]";

            string tmpRd1 = OrundhetRd(tmpd1);
            string tmpRd = "max: " + ReplaceCommaDot(tmpRd1) + " [3F]";

            string tmpGV = GodstjockleksVariationGV(tmpd);
            kv["SumGV"] = "max: " + ReplaceCommaDot(tmpGV) + " [2F]";

            double tmpTML = tmpL - 7;
            int tmpML = tmpTML < 80 ? 75 : tmpTML < 110 ? 75 : 100;
            kv["SumML"] = "ML=" + tmpML.ToString(CultureInfo.InvariantCulture);

            double tmpVTList = KonAvvikelseList(tmpd);
            double tmpVT = (tmpML * tmpVTList) / 1000.0;
            kv["SumVT"] = "Konavvikelse: ± " + FormatDot4(tmpVT) + "[2F]";

            double tmp8 = RoundTo(((tmpd2 - tmpd1) / 2.0) - ((83 + 1) / (2.0 * tmpKona)), 0.001);
            double tmp83 = RoundTo(((tmpd2 - tmpd1) / 2.0) - ((8 + 1) / (2.0 * tmpKona)), 0.001);
            double tmp108 = RoundTo(((tmpd2 - tmpd1) / 2.0) - ((108 + 1) / (2.0 * tmpKona)), 0.001);
            double tmp40 = RoundTo(((tmpd2 - tmpd1) / 2.0) - ((40 + 1) / (2.0 * tmpKona)), 0.001);
            double tmp140 = RoundTo(((tmpd2 - tmpd1) / 2.0) - ((140 + 1) / (2.0 * tmpKona)), 0.001);

            int sumL1 = tmpTML < 110 ? 8 : tmpTML < 145 ? 108 : 140;
            int sumL2 = tmpTML < 145 ? 83 : 40;
            double sumE1 = tmpTML < 110 ? tmp8 : tmpTML < 145 ? tmp108 : tmp140;
            double sumE2 = tmpTML < 145 ? tmp83 : tmp40;

            kv["SumL1"] = sumL1.ToString(CultureInfo.InvariantCulture);
            kv["SumL2"] = sumL2.ToString(CultureInfo.InvariantCulture);
            kv["SumE1"] = FormatDot(sumE1);
            kv["SumE11"] = FormatDot(sumE1);
            kv["SumE2"] = FormatDot(sumE2);
            kv["SumE22"] = FormatDot(sumE2);

            string sumByg = tmpTML < 80 ? "7419471" : tmpTML < 145 ? "SR 7419471" : "SR 7415991";
            kv["SumBygGV"] = sumByg;
            kv["SumBygVT"] = sumByg;
            kv["SumBygGT"] = sumByg;

            string tmpMaskinValS1 = EqualsI(maskinVal, "VTR-160") ? "VTR-160" : EqualsI(maskinVal, "MacTurn 550") ? "MacTurn 550" : "";
            kv["SumMaskinValS1"] = ("Maskin: " + tmpMaskinValS1 + " - OP1").Trim();

            bool op1Machine = EqualsI(maskinVal, "VTR-160") || EqualsI(maskinVal, "MacTurn 550");

            kv["SumF11"] = op1Machine ? "1/5" : "";
            kv["SumF12"] = op1Machine ? "1/5" : "";
            kv["SumF13"] = op1Machine ? "1/5" : "";
            kv["SumF14"] = op1Machine ? "1/5" : "";
            kv["SumF15"] = op1Machine ? "1/5" : "";
            kv["SumF16"] = op1Machine ? "1/5" : "";
            kv["SumF17"] = op1Machine ? "1/2" : "";
            kv["SumF18"] = op1Machine ? "1/5" : "";
            kv["SumF19"] = op1Machine ? "1/5" : "";
            kv["SumF20"] = op1Machine ? "1/5" : "";
            kv["SumF21"] = op1Machine ? "1/5" : "";

            kv["SumD1_1"] = op1Machine ? "Skjutmått" : "";
            kv["SumD1_2"] = op1Machine ? "Skjutmått" : "";
            kv["SumD1_3"] = op1Machine ? "Skjutmått" : "";
            kv["SumD1_4"] = op1Machine ? "Radielyra" : "";
            kv["SumD1_5"] = op1Machine ? "Skjutmått" : "";
            kv["SumD1_6"] = op1Machine ? "Egglinjal" : "";
            kv["SumD1_7"] = op1Machine ? "Egglinjal" : "";
            kv["SumD1_8"] = op1Machine ? ("Mätbygel " + kv["SumBygGV"]) : "";
            kv["SumD1_9"] = op1Machine ? ("Mätbygel " + kv["SumBygGT"]) : "";
            kv["SumD1_0"] = op1Machine ? ("Mätbygel " + kv["SumBygVT"]) : "";
            kv["SumD1_11"] = op1Machine ? "Mätmaskin" : "";

            kv["SumAF1_1"] = op1Machine ? "" : "";
            kv["SumAF1_2"] = op1Machine ? "" : "";
            kv["SumAF1_3"] = op1Machine ? "" : "";
            kv["SumAF1_4"] = op1Machine ? "" : "";
            kv["SumAF1_5"] = op1Machine ? "" : "";
            kv["SumAF1_6"] = op1Machine ? kv["SumRHA"] : "";
            kv["SumAF1_7"] = op1Machine ? kv["SumRHB"] : "";
            kv["SumAF1_8"] = op1Machine ? kv["SumGV"] : "";
            kv["SumAF1_9"] = op1Machine ? (kv["SumGodstjocklekTol"] + LB + " - 0.000" ): "";
            kv["SumAF1_0"] = "Konavvikelse: ± 0.0098[2F]";
            kv["SumAF1_11"] = op1Machine ? tmpRd : "";

            kv["SumTextS1"] = "Bryt alla kanter, avlägsna grader";

            string ritnSvarvIn = ritSvarv;
            string sumRitningsnr = EqualsI(ritnSvarvIn, "0") ? (tmpSerie == 38 ? "238000" : tmpSerie == 39 ? "226472" : tmpBet) : ritnSvarvIn;
            kv["SumRitningsnr"] = sumRitningsnr;
            kv["SumRitTol"] = tmp7432987 ? "" : "Toleranser: 1432010";
            kv["SumRitYtjämnhet"] = "Yta: 7430184";
            kv["SumKlEgenskaper"] = "PRODUCTION NUTS & SLEEVES & HOUSINGS\\ALLMÄNKLASSADE EGENSKAPER\\Klassade egenskaper kilhylsor";

            string sumBygM = tmpML < 145 ? "SR 7419471" : "SR 7415991";
            kv["SumBygGtj"] = sumBygM;
            kv["SumBygGVar"] = sumBygM;
            kv["SumBygVinkTol"] = sumBygM;

            bool tmpPBKont = op1Machine;
            double tmpBygelinstkonst = sumL1 == 50 ? -3 : sumL1 == 8 ? -5 : sumL1 == 140 ? -10 : 0;
            double tmpInstE1 = sumL1 == 8 ? sumE1 - 5 : sumE1 - 10;
            double tmpInstE2 = sumL1 == 8 ? sumE2 - 5 : sumE2 - 10;

            kv["VaLFärdig"] = "";
            kv["VaLInfo"] = "";

            int tmpSerie1 = ToInt(Left(tmpBet2s, 2));
            int tmpTyp1 = tmpCount > 3 ? ToInt(Right(tmpBet2s, 2)) : ToInt(tmpSlash ? tmpBet3 : tmpBet3);
            int tmpTypIndex1 = MemberIndex(tmpTyp1, TypAllValues);

            double tmpKona1 = (tmpLU && (tmpSerie1 == 39 || tmpSerie1 == 38)) ? 12 : 30;

            double tmpL1 = GetDouble(bm, "Längd (L)");
            if (tmpL1 == 0)
                tmpL1 = GetFromList(tmpTypIndex1, tmpSerie1 == 39 ? L39AllValues : L38AllValues);

            double tmpamatt = GetDouble(bm, "a-mått");

            double tmpd4;
            double yd = GetDouble(bm, "Ytterdiameter lillkona (d)");
            if (yd == 0)
            {
                if (tmpCount > 3) tmpd4 = (tmpTyp1 / 2.0) * 10.0;
                else tmpd4 = ParseDoubleOrZero(tmpBet3);
            }
            else
            {
                tmpd4 = tmpamatt == 0 ? yd : yd + (tmpamatt / tmpKona1);
            }

            double tmpd6 = RoundTo((tmpL1 / tmpKona1) + tmpd4, 0.01);

            double tmpd5;
            double id1 = GetDouble(bm, "Innerdiameter (d1)");
            if (id1 == 0)
            {
                if (!tmpSpecial)
                {
                    if (tmpTyp1 < 61) tmpd5 = tmpd4 - 10;
                    else if (tmpTyp1 < 501) tmpd5 = tmpd4 - 15;
                    else if (tmpTyp1 < 671) tmpd5 = tmpd4 - 20;
                    else if (tmpTyp1 < 901) tmpd5 = tmpd4 - 25;
                    else tmpd5 = tmpd4 - 30;
                }
                else
                {
                    tmpd5 = tmpCount > 3 ? ParseDoubleOrZero(tmpBet3) : ParseDoubleOrZero(tmpBet4);
                }
            }
            else
            {
                tmpd5 = id1;
            }

            kv["SumRa25b"] = "2.5";
            kv["SumRa5a"] = "5";

            kv["SumR2"] = kv["SumR"];
            kv["SumV30"] = "30º";
            kv["SumV45"] = "45º";
            kv["SumV120"] = "120º";

            double tmpC = GetDouble(bm, "Slits");
            kv["SumC"] = "(c) " + FormatDot(tmpC);
            kv["SumCTol"] = tmpC < 6.01 ? "± 0.1" : "± 0.2";

            double tmpB = GetDouble(bm, "Längd till Oljespår (B)");
            kv["SumB"] = "(B) " + FormatDot(tmpB);
            kv["SumBTol"] = "± " + FormatDot(GeneralTol(tmpB));

            double tmpSVRep = 12;
            kv["SumSVRep"] = FormatDot(tmpSVRep) + "º";
            double tmpAntRep = GetDouble(bm, "Antal repor (n)");
            kv["SumAntRep"] = "(n) " + FormatDot(tmpAntRep) + " axiella spår";

            double repB = GetDouble(bm, "RepBredd");
            double tmpRepB = repB == 0 ? ((tmpSerie1 == 39 && tmpTyp1 == 630) ? 1 : (tmpSerie1 == 38 && tmpTyp1 == 850) ? 2 : 1.5) : repB;
            kv["SumRepB"] = "(RB) " + FormatDot(tmpRepB);

            double repD = GetDouble(bm, "RepDjup");
            double tmpRepD = repD == 0 ? ((tmpSerie1 == 39 && tmpTyp1 == 630) ? 0.5 : 1) : repD;
            kv["SumRepD"] = "(RD) " + FormatDot1(tmpRepD);

            double tmpJ = GetDouble(bm, "Längd till Repor (J)");
            kv["SumJ"] = "(J) " + FormatDot(tmpJ);
            kv["SumJTol"] = "± " + FormatDot(GeneralTol(tmpJ));

            double tmpK = GetDouble(bm, "Längd Repor (K)");
            kv["SumK"] = "(K) " + FormatDot(tmpK);
            kv["SumKTol"] = "± " + FormatDot(GeneralTol(tmpK));

            double tmpDelVinkelRepor = RoundTo((360 - (tmpSVRep * 2)) / Math.Max(tmpAntRep - 1, 1), 0.1);
            double tmp1a = (360 - (tmpDelVinkelRepor * (tmpAntRep - 1))) / 2.0;
            kv["SumVDRep"] = FormatDot(tmpDelVinkelRepor) + "º";

            double tmpKonstant1Rep = RoundTo(Math.Sin((tmp1a / 2.0) * Math.PI / 180.0) * 2.0, 0.0001);
            double tmpKonstantDelRep = RoundTo(Math.Sin((tmpDelVinkelRepor / 2.0) * Math.PI / 180.0) * 2.0, 0.0001);

            double tmpKorda1RepInv = RoundTo((tmpd5 / 2.0) * tmpKonstant1Rep, 0.1);
            kv["SumK1i"] = "45,3";

            double tmpKonUtrakning = ((tmpB / tmpKona1) + tmpd4);
            double tmpKorda1RepUtv = RoundTo((tmpKonUtrakning / 2.0) * tmpKonstant1Rep, 0.1);
            kv["SumK1u"] = "47,9";

            double tmpKordaDelRepInv = RoundTo((tmpd5 / 2.0) * tmpKonstantDelRep, 0.1);
            kv["SumKDi"] = "102,7";

            double tmpKordaDelRepUtv = RoundTo((tmpKonUtrakning / 2.0) * tmpKonstantDelRep, 0.1);
            kv["SumKDu"] = "108,2";

            double tmpSVOS = 10;
            kv["SumSVOS"] = FormatDot(tmpSVOS) + "º";
            double tmpKonstantOS = RoundTo(Math.Sin((tmpSVOS / 2.0) * Math.PI / 180.0) * 2.0, 0.0001);
            double tmpKordaOSinv = RoundTo((tmpd5 / 2.0) * tmpKonstantOS, 0.1);
            kv["SumKOi"] = "40,1";
            double tmpKordaOSutv = RoundTo((tmpKonUtrakning / 2.0) * tmpKonstantOS, 0.1);
            kv["SumKOu"] = "42,3";

            double tmpE = GetDouble(bm, "Bredd Oljespår (E)");
            kv["SumE"] = "(E) " + FormatDot(tmpE);
            kv["SumETol"] = tmpE < 6.01 ? "± 0.1" : "± 0.2";

            kv["SumR3"] = tmpE == 5 ? "R4" : tmpE == 6 ? "R4.5" : tmpE == 7 ? "R5" : tmpE == 8 ? "R6" : tmpE == 10 ? "R7" : "R8";

            string tmpF = tmpE == 5 ? "1" : tmpE == 6 ? "1.2" : tmpE == 7 ? "1.5" : tmpE == 8 ? "1.5" : tmpE == 10 ? "2" : (tmpSerie1 == 39 && Math.Abs(tmpd5 - 1143) < 0.0001) ? "2.5" : "2.7";
            kv["SumF"] = tmpF;
            kv["SumFTol"] = "± 0.1";

            kv["SumH"] = "Ø " + GetString(bm, "Ø Borrhål till Oljespår");
            kv["SumH1"] = kv["SumH"];
            kv["SumHTol"] = "± 0.1";
            kv["SumH1Tol"] = "± 0.1";

            kv["SumLOH"] = "(LOH) " + GetString(bm, "Längd Oljeborrhål");
            kv["SumLOHTol"] = "± " + FormatDot(GeneralTol(GetDouble(bm, "Längd Oljeborrhål")));

            kv["SumBOH"] = "Ø " + GetString(bm, "Ø Oljeborrhål");
            kv["SumBOHTol"] = "± " + FormatDot(GeneralTol(GetDouble(bm, "Ø Oljeborrhål")));

            kv["SumOHG"] = GetString(bm, "Gänga Oljeborrhål");
            kv["SumG"] = kv["SumOHG"];
            kv["SumOHG2"] = GetString(bm, "Gänga Oljeborrhål");

            kv["SumOHDj"] = GetString(bm, "Oljeborrhål Djup");
            kv["SumOHDjTol"] = GetDouble(bm, "Oljeborrhål Djup") < 6.01 ? "± 0.1" : "± 0.2";
            double ohgDj = GetDouble(bm, "OljehålsGänga Djup");
            kv["SumOHGDj"] = ohgDj == 0 ? "" : "min " + FormatDot(ohgDj);
            double tmpBHM = GetDouble(bm, "Borrhålsmått (BH)");
            kv["SumBHM"] = "(BH)" + FormatDot(tmpBHM);

            kv["SumOHD2"] = "15";
            kv["SumOHDj2Tol"] = kv["SumOHDjTol"];
            kv["SumOHGDj2"] = "min 12";
            kv["SumBHM2"] = "(BH)" + FormatDot(tmpBHM);
            kv["SumV452"] = "45°";

            double tmpVOH = GetDouble(bm, "Vinkel Oljeborrhål");
            kv["SumVOH"] = FormatDot(tmpVOH);
            double tmpKonstantVOH = RoundTo(Math.Sin((tmpVOH / 2.0) * Math.PI / 180.0) * 2.0, 0.0001);
            double tmpKordaVOH = RoundTo(((tmpd5 + (tmpBHM * 2)) / 2.0) * tmpKonstantVOH, 0.1);
            kv["SumKOBH"] = "407,9";

            kv["Sum1"] = Math.Abs(tmpVOH - 90) < 0.0001 ? "B2" : "B";
            kv["Sum2"] = "(" + kv["Sum1"] + ") " + kv["SumKOBH"];

            kv["SumV120A"] = "120°";
            kv["SumV120B"] = "120°";
            string tmpRadie = "235,5";
            kv["SumRadie"] = "R" + ReplaceCommaDot(tmpRadie);

            string tmpMaskinValS2 = EqualsI(maskinVal, "Skepp 6") ? "Skepp 6" : EqualsI(maskinVal, "VTR-160") ? "VTR-160" : EqualsI(maskinVal, "MacTurn 550") ? "MacTurn 550" : "";
            kv["SumMaskinValS2"] = ("Maskin: " + tmpMaskinValS2 + " - Borrning, Fräsning").Trim();

            bool op2Machine = EqualsI(maskinVal, "Skepp 6") || EqualsI(maskinVal, "VTR-160") || EqualsI(maskinVal, "MacTurn 550") || string.IsNullOrEmpty(maskinVal);

            kv["SumF2_1"] = op2Machine ? "1/1" : "";
            kv["SumF2_2"] = op2Machine ? "1/1" : "";
            kv["SumF2_3"] = (EqualsI(maskinVal, "Skepp 6") || EqualsI(maskinVal, "VTR-160") || EqualsI(maskinVal, "MacTurn 550")) ? "1/1" : "";
            kv["SumF2_4"] = (EqualsI(maskinVal, "Skepp 6") || EqualsI(maskinVal, "VTR-160") || EqualsI(maskinVal, "MacTurn 550")) ? "1/1" : "";

            kv["SumD2_1"] = (EqualsI(maskinVal, "Skepp 6") || EqualsI(maskinVal, "VTR-160") || EqualsI(maskinVal, "MacTurn 550")) ? "Gängtolk" : "";
            kv["SumD2_2"] = (EqualsI(maskinVal, "Skepp 6") || EqualsI(maskinVal, "VTR-160") || EqualsI(maskinVal, "MacTurn 550")) ? "Skjutmått" : "";
            kv["SumD2_3"] = (EqualsI(maskinVal, "Skepp 6") || EqualsI(maskinVal, "VTR-160") || EqualsI(maskinVal, "MacTurn 550")) ? "Skjutmått" : "";
            kv["SumD2_4"] = (EqualsI(maskinVal, "Skepp 6") || EqualsI(maskinVal, "VTR-160") || EqualsI(maskinVal, "MacTurn 550")) ? "Skjutmått" : "";

            kv["SumAF2_1"] = "";
            kv["SumAF2_2"] = "";
            kv["SumAF2_3"] = (EqualsI(maskinVal, "Skepp 6") || EqualsI(maskinVal, "VTR-160") || EqualsI(maskinVal, "MacTurn 550")) ? "Tolerans efter slits enl. 7437495" : "";
            kv["SumAF2_4"] = "";

            kv["SumTextS2"] = "Bryt alla kanter, avlägsna";

            string ritBorr = GetString(bm, "Ritningsnummer Borr");
            kv["SumRitningsnr2"] = EqualsI(ritBorr, "0") ? ((tmpSerie1 == 38 && !tmpSpecial) ? "238000" : (tmpSerie1 == 39 && !tmpSpecial) ? "226472" : ("Styckritning: " + subject)) : ("Styckritning: " + ritBorr);
            kv["SumRitTol2"] = "Toleranser: 1432010, 7437495";
            kv["SumRitYtjämnhet2"] = "Yta: 7430184";
            kv["SumKlEgenskaper2"] = "PRODUCTION NUTS & SLEEVES & HOUSINGS\\ALLMÄNKLASSADE EGENSKAPER\\Klassade egenskaper kilhylsor";

            string textField = GetString(bm, "Text");
            string[] tmpText = Explode(textField, new[] { '§' });
            kv["SumText1"] = EqualsI(textField, "0") ? "" : Word(tmpText, 1);
            kv["SumText2"] = EqualsI(textField, "0") ? "" : Word(tmpText, 2);
            kv["SumText3"] = EqualsI(textField, "0") ? "" : Word(tmpText, 3);
            kv["SumText4"] = EqualsI(textField, "0") ? "" : Word(tmpText, 4);
            kv["SumText5"] = " 3x borrhåll med placering 60° ";
            kv["SumText6"] = " från slits & 120° delning ";

            return kv;
        }

        private static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published)) return "";
            if (!DateTime.TryParse(published, out var pubDt)) return "";
            int dagar = 14;
            DateTime validTill = pubDt.AddDays(dagar);
            if (DateTime.Today <= validTill.Date)
                return "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom " + dagar.ToString(CultureInfo.InvariantCulture) + " dagar)" + LB + LB +
                       "Information om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument" + LB + LB +
                       "Popupruta aktiv till " + validTill.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return "";
        }

        private static string[] Explode(string s, char[] separators)
        {
            if (string.IsNullOrEmpty(s)) return Array.Empty<string>();
            return s.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        }

        private static string Word(string[] list, int index1Based)
        {
            if (list == null || index1Based <= 0 || index1Based > list.Length) return "";
            return list[index1Based - 1] ?? "";
        }

        private static int MemberIndex(string value, IEnumerable<string> list)
        {
            int idx = 1;
            foreach (var s in list)
            {
                if (string.Equals((s ?? "").Trim(), (value ?? "").Trim(), StringComparison.OrdinalIgnoreCase))
                    return idx;
                idx++;
            }
            return 0;
        }

        private static int MemberIndex(int value, int[] list)
        {
            for (int i = 0; i < list.Length; i++)
                if (list[i] == value)
                    return i + 1;
            return 0;
        }

        private static bool ContainsAny(string text, string[] tokens)
        {
            if (string.IsNullOrEmpty(text)) return false;
            for (int i = 0; i < tokens.Length; i++)
                if (!string.IsNullOrEmpty(tokens[i]) && text.Contains(tokens[i], StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }

        private static string Left(string s, int n)
        {
            if (string.IsNullOrEmpty(s)) return "";
            if (n <= 0) return "";
            return s.Length <= n ? s : s.Substring(0, n);
        }

        private static string Right(string s, int n)
        {
            if (string.IsNullOrEmpty(s)) return "";
            if (n <= 0) return "";
            return s.Length <= n ? s : s.Substring(s.Length - n);
        }

        private static int ToInt(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;
            s = s.Trim();
            int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var v);
            return v;
        }

        private static string ToNumberString(string s)
        {
            double d = ParseDoubleOrZero(s);
            if (Math.Abs(d) < 0.0000001) return "0";
            return ReplaceCommaDot(d.ToString(CommonFunctions.Culture));
        }

        private static double ParseDoubleOrZero(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;
            s = s.Trim().Replace(".", ",");
            return double.TryParse(s, NumberStyles.Any, CommonFunctions.Culture, out var v) ? v : 0;
        }

        private static double GetDouble(List<Bookmark> bm, string key)
        {
            string raw = GetString(bm, key);
            if (string.IsNullOrWhiteSpace(raw)) return 0;
            raw = raw.Trim().Replace(".", ",");
            return double.TryParse(raw, NumberStyles.Any, CommonFunctions.Culture, out var v) ? v : 0;
        }

        private static string GetString(List<Bookmark> bm, string key)
        {
            if (bm == null || string.IsNullOrWhiteSpace(key)) return "";
            for (int i = 0; i < bm.Count; i++)
            {
                var b = bm[i];
                if (b != null && string.Equals(b.BookmarkName ?? "", key, StringComparison.OrdinalIgnoreCase))
                    return b.BookmarkValue ?? "";
            }
            return "";
        }

        private static string FormatDot(double v)
        {
            return v.ToString(CommonFunctions.Culture).Replace(",", ".");
        }

        private static string FormatDot3(double v)
        {
            return v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
        }

        private static string FormatDot4(double v)
        {
            return v.ToString("F4", CommonFunctions.Culture).Replace(",", ".");
        }

        private static string FormatDot1(double v)
        {
            return v.ToString("F1", CommonFunctions.Culture).Replace(",", ".");
        }

        private static bool EqualsI(string a, string b)
        {
            return string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);
        }

        private static double RoundTo(double value, double step)
        {
            if (step <= 0) return value;
            double x = value / step;
            double r = x >= 0 ? Math.Floor(x + 0.5) : Math.Ceiling(x - 0.5);
            return r * step;
        }

        private static double GeneralTol(double v)
        {
            if (v < 6.01) return 0.1;
            if (v < 30.01) return 0.2;
            if (v < 120.01) return 0.3;
            if (v < 400.01) return 0.5;
            if (v < 1000.01) return 0.8;
            if (v < 2000.01) return 1.2;
            return 2.0;
        }

        private static double H15NegTol(double v)
        {
            if (v < 3.01) return 0.400;
            if (v < 6.01) return 0.480;
            if (v < 10.01) return 0.580;
            if (v < 18.01) return 0.700;
            if (v < 30.01) return 0.840;
            if (v < 50.01) return 1.000;
            if (v < 80.01) return 1.200;
            if (v < 120.01) return 1.400;
            if (v < 180.01) return 1.600;
            if (v < 250.01) return 1.850;
            if (v < 315.01) return 2.100;
            if (v < 400.01) return 2.300;
            if (v < 500.01) return 2.500;
            if (v < 630.01) return 2.800;
            if (v < 800.01) return 3.200;
            if (v < 1000.01) return 3.600;
            if (v < 1250.01) return 4.200;
            if (v < 1600.01) return 5.000;
            if (v < 2000.01) return 6.000;
            if (v < 2500.01) return 7.000;
            return 8.600;
        }

        private static double JS10Tol(double d)
        {
            if (d < 3.01) return 0.020;
            if (d < 6.01) return 0.024;
            if (d < 10.01) return 0.029;
            if (d < 18.01) return 0.035;
            if (d < 30.01) return 0.042;
            if (d < 50.01) return 0.050;
            if (d < 80.01) return 0.060;
            if (d < 120.01) return 0.070;
            if (d < 180.01) return 0.080;
            if (d < 250.01) return 0.092;
            if (d < 315.01) return 0.105;
            if (d < 400.01) return 0.115;
            if (d < 500.01) return 0.125;
            if (d < 630.01) return 0.140;
            if (d < 800.01) return 0.160;
            if (d < 1000.01) return 0.180;
            if (d < 1250.01) return 0.210;
            if (d < 1600.01) return 0.250;
            if (d < 2000.01) return 0.300;
            if (d < 2500.01) return 0.350;
            return 0.430;
        }

        private static string ReplaceCommaDot(string s)
        {
            return (s ?? "").Replace(",", ".");
        }

        private static int SelectByTmpd(double tmpd, double[] bounds, int[] values, int last)
        {
            for (int i = 0; i < bounds.Length && i < values.Length; i++)
                if (tmpd < bounds[i])
                    return values[i];
            return last;
        }

        private static string OrundhetRd(double d1)
        {
            if (d1 < 30.01) return "0.042";
            if (d1 < 50.01) return "0.050";
            if (d1 < 80.01) return "0.060";
            if (d1 < 120.01) return "0.070";
            if (d1 < 180.01) return "0.080";
            if (d1 < 250.01) return "0.092";
            if (d1 < 315.01) return "0.105";
            if (d1 < 400.01) return "0.115";
            if (d1 < 500.01) return "0.125";
            if (d1 < 630.01) return "0.140";
            if (d1 < 800.01) return "0.160";
            if (d1 < 1000.01) return "0.180";
            if (d1 < 1250.01) return "0.210";
            if (d1 < 1600.01) return "0.250";
            return "";
        }

        private static string GodstjockleksVariationGV(double d)
        {
            if (d > 1250) return "0.055";
            if (d > 1000) return "0.050";
            if (d > 800) return "0.045";
            if (d > 630) return "0.040";
            if (d > 500) return "0.035";
            if (d > 315) return "0.030";
            if (d > 250) return "0.025";
            if (d > 180) return "0.020";
            if (d > 120) return "0.015";
            if (d > 50) return "0.010";
            return "0.008";
        }

        private static double KonAvvikelseList(double d)
        {
            if (d < 50.1) return 0.6;
            if (d < 80.1) return 0.5;
            if (d < 120.1) return 0.45;
            if (d < 150.1) return 0.3;
            if (d < 180.1) return 0.18;
            if (d < 400.1) return 0.15;
            if (d < 500.1) return 0.13;
            if (d < 630.1) return 0.12;
            if (d < 800.1) return 0.11;
            if (d < 1000.1) return 0.10;
            if (d < 1250.1) return 0.09;
            if (d < 1600.1) return 0.08;
            return 0.07;
        }

        private static double GetFromList(int index1Based, double[] list)
        {
            if (index1Based <= 0 || index1Based > list.Length) return 0;
            return list[index1Based - 1];
        }

        private static string ToTolStringGodstjocklekPlus(bool noll1, double tmpKona, double tmpd, double konaInput)
        {
            if (noll1) return "0.063";
            if (Math.Abs(tmpKona - 12) < 0.0001)
            {
                if (tmpd > 1250) return "0.100";
                if (tmpd > 1000) return "0.095";
                if (tmpd > 800) return "0.085";
                if (tmpd > 630) return "0.075";
                if (tmpd > 500) return "0.070";
                if (tmpd > 400) return "0.065";
                if (tmpd > 315) return "0.060";
                if (tmpd > 250) return "0.055";
                if (tmpd > 180) return "0.050";
                if (tmpd > 120) return "0.040";
                if (tmpd > 80) return "0.035";
                if (tmpd > 50) return "0.030";
                if (tmpd > 30) return "0.025";
                return "0.020";
            }
            if (Math.Abs(tmpKona - 30) < 0.0001)
            {
                if (tmpd > 1600) return "0.070";
                if (tmpd > 1250) return "0.065";
                if (tmpd > 1000) return "0.060";
                if (tmpd > 800) return "0.055";
                if (tmpd > 630) return "0.050";
                if (tmpd > 500) return "0.045";
                if (tmpd > 400) return "0.040";
                if (tmpd > 315) return "0.035";
                if (tmpd > 250) return "0.035";
                if (tmpd > 180) return "0.030";
                if (tmpd > 120) return "0.025";
                if (tmpd > 80) return "0.022";
                if (tmpd > 50) return "0.019";
                if (tmpd > 30) return "0.016";
                return "0.013";
            }
            return "Fel Kona";
        }

        private static string ToTolStringGodstjocklekMinus(bool noll, double tmpKona, double tmpd, double konaInput)
        {
            if (noll) return "0";
            if (Math.Abs(tmpKona - 12) < 0.0001)
            {
                if (tmpd > 1250) return "0.310";
                if (tmpd > 1000) return "0.280";
                if (tmpd > 800) return "0.250";
                if (tmpd > 630) return "0.225";
                if (tmpd > 500) return "0.200";
                if (tmpd > 400) return "0.190";
                if (tmpd > 315) return "0.175";
                if (tmpd > 250) return "0.160";
                if (tmpd > 180) return "0.140";
                if (tmpd > 120) return "0.120";
                if (tmpd > 80) return "0.105";
                if (tmpd > 50) return "0.090";
                if (tmpd > 30) return "0.075";
                return "0.070";
            }
            if (Math.Abs(tmpKona - 30) < 0.0001)
            {
                if (tmpd > 1250) return "0.195";
                if (tmpd > 1000) return "0.170";
                if (tmpd > 800) return "0.155";
                if (tmpd > 630) return "0.140";
                if (tmpd > 500) return "0.125";
                if (tmpd > 400) return "0.115";
                if (tmpd > 315) return "0.105";
                if (tmpd > 251) return "0.095";
                if (tmpd > 180) return "0.085";
                if (tmpd > 120) return "0.075";
                if (tmpd > 80) return "0.065";
                if (tmpd > 50) return "0.055";
                if (tmpd > 30) return "0.046";
                return "0.039";
            }
            return "Fel Kona";
        }

        private static readonly int[] Typ38Values = new[] { 52, 56, 60, 64, 68, 72, 76, 80, 84, 88, 92, 96, 500, 530, 560, 600, 630, 670, 710, 750, 800, 850, 900, 950, 1000, 1060, 1120, 1180, 1250, 1320, 1400 };
        private static readonly int[] Typ39Values = new[] { 44, 48, 52, 56, 60, 64, 68, 72, 76, 80, 84, 88, 92, 96, 500, 530, 560, 600, 630, 670, 710, 750, 800, 850, 900, 950, 1000, 1060, 1120, 1180, 1250 };

        private static readonly double[] L38Values = new double[] { 55, 62, 72, 72, 72, 72, 87, 87, 87, 87, 102, 102, 102, 106, 106, 115, 130, 130, 140, 150, 160, 160, 165, 175, 195, 195, 208, 208, 215, 236, 254 };
        private static readonly double[] L39Values = new double[] { 71, 71, 85, 85, 103, 103, 103, 103, 118, 118, 118, 132, 132, 140, 140, 150, 155, 165, 180, 185, 200, 206, 218, 224, 236, 250, 265, 280, 280, 300, 315 };

        private static readonly int[] TypAllValues = new[] { 44, 48, 52, 56, 60, 64, 68, 72, 76, 80, 84, 88, 92, 96, 500, 530, 560, 600, 630, 670, 710, 750, 800, 850, 900, 950, 1000, 1060, 1120, 1180, 1250, 1320, 1400 };
        private static readonly double[] L39AllValues = new double[] { 71, 71, 85, 85, 103, 103, 103, 103, 118, 118, 118, 132, 132, 140, 140, 150, 155, 165, 180, 185, 200, 206, 218, 224, 236, 250, 265, 280, 280, 300, 315, 0, 0 };
        private static readonly double[] L38AllValues = new double[] { 0, 0, 55, 62, 72, 72, 72, 72, 87, 87, 87, 87, 102, 102, 102, 106, 106, 115, 130, 130, 140, 150, 160, 160, 165, 175, 195, 195, 208, 208, 215, 236, 254 };
    }
}