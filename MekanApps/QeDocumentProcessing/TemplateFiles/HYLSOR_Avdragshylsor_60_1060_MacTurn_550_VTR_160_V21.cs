using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Avdragshylsor_60_1060_MacTurn_550_VTR_160_V21 : ITemplateCalculations
    {
        private const string LB = "<<LineBreak>>";

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            InitializeKeys(kv);

            string subject = (req?.ProductDesignation ?? string.Empty).Trim();
            string machine = (req?.MachineNumber ?? string.Empty).Trim();
            List<Bookmark> bm = req?.Bookmarks;

            string tmpFormat = (subject ?? string.Empty).Trim().ToUpperInvariant();
            string tmpBet = tmpFormat.Replace('.', ',');
            string[] tmpBetLista = Explode(tmpBet, new[] { ' ', '/', '.', '-', '_' });
            string tmpBet1a = Word(tmpBetLista, 1);
            string tmpBet2a = Word(tmpBetLista, 2);
            string tmpBet3a = Word(tmpBetLista, 3);
            string tmpBet4a = Word(tmpBetLista, 4);
            string tmpBet5a = Word(tmpBetLista, 5);
            string tmpBet6a = Word(tmpBetLista, 6);

            string tmpBet1 = NormalizeToken(tmpBet1a);
            string tmpBet2 = NormalizeToken(tmpBet2a);
            string tmpBet3 = NormalizeToken(tmpBet3a);
            string tmpBet4 = NormalizeToken(tmpBet4a);
            string tmpBet5 = NormalizeToken(tmpBet5a);
            string tmpBet6 = NormalizeToken(tmpBet6a);

            bool tmpSlash = ContainsI(tmpBet, "/");
            bool tmpMS = ContainsI(tmpBet, "MS");
            bool tmpO = EqualsI(tmpBet1, "MS") || ContainsI(tmpBet, "O");
            int tmpCountB = (tmpBet ?? string.Empty).Length;
            int tmpCountB2 = (tmpBet2 ?? string.Empty).Length;
            string tmpSerie = tmpCountB2 == 3 || tmpCountB2 == 2 ? tmpBet2 : tmpCountB2 > 4 ? Left(tmpBet2, 3) : Left(tmpBet2, 2);

            double konringsDiameterBookmark = GetDouble(bm, "Konringsdiameter (d)");
            double tmpTyp = 0;
            if (tmpMS)
                tmpTyp = konringsDiameterBookmark;
            else if (tmpCountB2 > 3)
                tmpTyp = ToDouble(Right(tmpBet2, 2));
            else if (tmpSlash)
                tmpTyp = ToDouble(tmpBet3);
            else if (tmpCountB2 < 2)
                tmpTyp = 0;
            else
                tmpTyp = ToDouble(tmpBet3);

            double tmpL = GetDouble(bm, "Längd (L)");
            double tmpb2 = GetDouble(bm, "Längd till gänga (b)");
            double tmph = GetDouble(bm, "Släppning (h)");
            double tmpd4 = GetDouble(bm, "Släppningsdiameter (d4)");
            double tmpBeta = GetDouble(bm, "Borrvinkel (ß)");
            double tmpB = GetDouble(bm, "Längd till oljespår (B)");
            double tmpd2 = GetDouble(bm, "Ytterdiameter (d2)");
            double tmpd1 = GetDouble(bm, "Innerdiameter (d1)");
            double tmpKonaBookmark = GetDouble(bm, "Kona");
            double tmpAmaatt = GetDouble(bm, "Amått");
            string ritningsnummer = GetString(bm, "Ritningsnummer");
            string gaengtappR = GetString(bm, "Gängtapp (R)");
            double tmpSVal = GetDouble(bm, "Längd gänghål (S)");
            string tmpSgRaw = GetString(bm, "Gänglängd gänghål (Sg)");
            double tmpGenomg = GetDouble(bm, "Genomgående borrhål (GH)");
            string tmpCBookmarkRaw = GetString(bm, "Längd Borrhål (C)");
            double tmpCBookmark = GetDouble(bm, "Längd Borrhål (C)");
            string tmpGBookmark = GetString(bm, "Diameter Borrhål (G)");
            double tmpAInput = GetDouble(bm, "Mått inv till borrcentrum (A)");
            double antalRepor = GetDouble(bm, "Antal repor (n)");
            double repLaengd = GetDouble(bm, "Replängd (K)");
            double laengdTillRepor = GetDouble(bm, "Längd till repor (J)");
            string ritningsnummerInput = (ritningsnummer ?? string.Empty).Trim();
            string maskinVal = machine;

            double tmpKona = tmpKonaBookmark == 0 ? ((tmpSerie == "240" || tmpSerie == "241") ? 30 : (tmpSerie == "LW" ? 0 : 12)) : tmpKonaBookmark;
            kv["SumV"] = "30º";
            kv["SumKona"] = tmpSerie == "LW" && tmpKonaBookmark == 0 ? "Kona 1:Ange Kona" : "Kona 1:" + FormatSimple(tmpKona);
            kv["SumVmått"] = Math.Abs(tmpKona - 30) < 0.0001 ? "50.6" : "55.6";

            double tmpd = konringsDiameterBookmark == 0 ? (tmpCountB2 > 3 ? (tmpTyp / 2.0) * 10.0 : tmpTyp) : konringsDiameterBookmark;
            double tmpdl = RoundTo(((1.0 / SafeDivisor(tmpKona)) * tmpAmaatt) + tmpd, 0.001);
            kv["Sumdl"] = "Minsta kondiameter (dl): " + FormatF3(tmpdl).Replace(".", ",");

            string tmpRitningsnr = (ritningsnummerInput ?? string.Empty).Trim().ToUpperInvariant();
            bool tmpRitAHA = ContainsI(tmpRitningsnr, "AHA");
            bool tmpRitAOH = ContainsI(tmpRitningsnr, "AOH");
            bool tmpRit3 = ContainsI(tmpRitningsnr, "7437359");
            bool tmpRit22 = ContainsI(tmpRitningsnr, "7437361");
            bool tmpRit23 = ContainsI(tmpRitningsnr, "7437360");
            bool tmpRit30 = ContainsI(tmpRitningsnr, "7437362");
            bool tmpRit31 = ContainsI(tmpRitningsnr, "7437364");
            bool tmpRit32 = ContainsI(tmpRitningsnr, "7437366") || ContainsI(tmpRitningsnr, "7437367");
            int tmpRitSum = (tmpRit3 ? 1 : 0) + (tmpRit22 ? 1 : 0) + (tmpRit23 ? 1 : 0) + (tmpRit30 ? 1 : 0) + (tmpRit31 ? 1 : 0) + (tmpRit32 ? 1 : 0);

            double sumML = RoundTo(tmpb2 - tmph - 16, 5);
            kv["SumML"] = FormatSimple(sumML);
            double tmpStmm = tmpRitAHA ? 6 : ComputeStigning(tmpd2);
            kv["SumP"] = "(P) 6,0";

            double tmpVT = tmpd > 1000 ? 0.09 : tmpd > 800 ? 0.10 : tmpd > 630 ? 0.11 : tmpd > 500 ? 0.12 : tmpd > 400 ? 0.13 : tmpd > 181 ? 0.15 : tmpd > 151 ? 0.18 : tmpd > 121 ? 0.30 : tmpd > 81 ? 0.45 : tmpd > 51 ? 0.50 : 0.60;
            string tmpTum = tmpd < 201 ? "" : "-tum";
            kv["SumKonavv"] = FormatF3((100 * tmpVT) / 1000.0) + " [2F]";

            bool tmpTa = ContainsI(gaengtappR, "1/4");
            string tmpT = tmpTa ? "13.5" : "10";
            kv["SumT"] = tmpO ? "(T) " + tmpT : "";

            double tmpC2 = tmpTyp < 501 ? 8 : 10;
            kv["SumC2"] = "(C2) " + FormatSimple(tmpC2);
            kv["SumR"] = tmpO ? gaengtappR : "";
            kv["SumR2"] = kv["SumR"];
            kv["SumS"] = tmpO ? "(S) " + FormatSimple(tmpSVal) : "";

            string tmpSg;
            if (EqualsI(tmpSgRaw, "none"))
                tmpSg = "";
            else if (EqualsI(tmpSgRaw, "0") || string.IsNullOrWhiteSpace(tmpSgRaw))
                tmpSg = FormatSimple(tmpSVal - 2);
            else
                tmpSg = NormalizeToken(tmpSgRaw);
            kv["SumSg"] = EqualsI(tmpSgRaw, "none") ? "" : "(Sg) min:" + tmpSg;

            kv["SumRullar"] = !tmpRitAHA ? (((Math.Abs(tmpStmm - 1) < 0.0001) || (Math.Abs(tmpStmm - 1.5) < 0.0001) || (Math.Abs(tmpStmm - 2) < 0.0001) || (Math.Abs(tmpStmm - 3) < 0.0001)) ? "M " + FormatSimple(tmpStmm) : "Tr " + FormatSimple(tmpStmm)) : kv["SumP"] + " UN";
            kv["SumGänga"] = "Tr 580x 6,0";

            double tmpZ = tmpd1 - 4;
            kv["SumZ"] = "(Z) Ø" + FormatSimple(tmpZ);

            kv["Sumd4"] = "(d4) " + FormatSimple(tmpd4);
            double tmpd4Tol = tmpSerie == "39" ? GeneralPlusTol(tmpd4) : 0;
            double tmpd4TolN = tmpSerie == "39" ? tmpd4Tol : tmpSerie == "22" ? 0.2 : (tmpSerie == "240" || tmpSerie == "241") ? 0.25 : 0.3;
            kv["Sumd4Tol"] = "+ " + FormatF3(tmpd4Tol);
            kv["Sumd4TolN"] = "- " + FormatF3(tmpd4TolN);

            kv["Sumd2"] = "(d2) " + FormatSimple(tmpd2);
            kv["Sumd2a"] = kv["Sumd2"];
            kv["Sumd2Tol"] = "+ " + FormatF1(0);
            double tmpd2TolN = Math.Abs(tmpStmm - 4) < 0.0001 ? 0.3 : Math.Abs(tmpStmm - 5) < 0.0001 ? 0.335 : Math.Abs(tmpStmm - 6) < 0.0001 ? 0.375 : Math.Abs(tmpStmm - 7) < 0.0001 ? 0.425 : 0.45;
            kv["Sumd2TolN"] = "- " + FormatF3(tmpd2TolN);
            kv["Sumd2aTol"] = kv["Sumd2Tol"];
            kv["Sumd2aTolN"] = kv["Sumd2TolN"];

            double tmpdm = Math.Abs(tmpStmm - 4) < 0.0001 ? tmpd2 - 2 : Math.Abs(tmpStmm - 5) < 0.0001 ? tmpd2 - 2.5 : Math.Abs(tmpStmm - 6) < 0.0001 ? tmpd2 - 3 : Math.Abs(tmpStmm - 7) < 0.0001 ? tmpd2 - 3.5 : tmpd2 - 4;
            kv["Sumdm"] = "(dm) " + FormatSimple(tmpdm);
            double tmpdmTol = Math.Abs(tmpStmm - 4) < 0.0001 ? 0.19 : Math.Abs(tmpStmm - 5) < 0.0001 ? 0.212 : Math.Abs(tmpStmm - 6) < 0.0001 ? 0.236 : Math.Abs(tmpStmm - 7) < 0.0001 ? 0.25 : 0.265;
            double tmpdmTolN = Math.Abs(tmpStmm - 4) < 0.0001 ? 0.63 : Math.Abs(tmpStmm - 5) < 0.0001 ? 0.71 : Math.Abs(tmpStmm - 6) < 0.0001 ? 0.8 : Math.Abs(tmpStmm - 7) < 0.0001 ? 0.85 : 0.95;
            kv["SumdmTol"] = "- " + FormatF3(tmpdmTol) + " [3F]";
            kv["SumdmTolN"] = "- " + FormatF3(tmpdmTolN) + " [3F]";

            kv["SumBorrHAnt"] = tmpO && tmpd1 < 195 ? "OBS! OBS! OBS!" + LB + "Bara 1 borrhål med genomgående hål till inv. & utv. oljespår" : "";
            kv["Sumß"] = EqualsI(GetString(bm, "Borrvinkel (ß)"), "0") || tmpBeta == 0 ? "" : "(ß) " + FormatF2(tmpBeta) + "º";
            kv["SumGH"] = "(GH) " + FormatSimple(tmpGenomg == 0 ? 3 : tmpGenomg);

            double tmpC = (EqualsI(tmpCBookmarkRaw, "0") || string.IsNullOrWhiteSpace(tmpCBookmarkRaw)) ? (tmpd < 421 ? tmpB + 3 : tmpB + 4) : tmpCBookmark;
            kv["SumC"] = tmpO ? "(C) " + FormatSimple(tmpC) : "";
            kv["SumG"] = tmpO ? "(G) " + tmpGBookmark : "";

            double tmpA = tmpAInput < 30 ? tmpAInput : ((tmpAInput - tmpd1) / 2.0);
            double tmpDB = tmpAInput < 30 ? tmpd1 + (tmpAInput * 2.0) : tmpAInput;
            kv["SumA"] = tmpO ? "(A) " + FormatSimple(tmpA) : "";
            kv["SumDB"] = tmpO ? "(DB) " + FormatSimple(tmpDB) : "--< Inga Oljeborrhål >--";

            double tmpb2SSK = tmpb2 + 4;
            kv["Sumb2SSK"] = "(b) ~" + FormatSimple(tmpb2SSK);
            kv["Sumb2"] = "(b) " + FormatSimple(tmpb2);
            double tmpb2Tol = Js15Tol(tmpb2);
            kv["Sumb2Tol"] = "± " + FormatF3(tmpb2Tol) + " [3F]";

            double tmpLSSK = tmpL + 4;
            kv["SumLSSK"] = "(L) ~" + FormatSimple(tmpLSSK);
            kv["SumL"] = "(L) " + FormatSimple(tmpL);
            kv["SumLTol"] = "+ " + FormatF1(0) + " [3F]";
            double tmpLTolN = H13NegTol(tmpL);
            kv["SumLTolN"] = "- " + FormatF3(tmpLTolN) + " [3F]";

            double tmpHm = tmpL - tmpb2;
            kv["SumHm"] = "(Hm) " + FormatSimple(tmpHm);
            kv["SumHmTol"] = "+ 1.15";
            kv["SumHmTolN"] = "- 2.04";

            kv["Sumh"] = "(h) " + FormatSimple(tmph);
            string sumg1 = EqualsI(tmpBet2, "336569") ? "4.4x45º" : ComputeG1(tmpStmm);
            kv["Sumg1"] = sumg1;
            kv["Sumg2"] = ComputeG2(sumg1, tmpRitSum, tmpRit3, tmpRit22, tmpRit23, tmpRit30, tmpRit31, tmpRit32, tmpd2);

            double tmpEE = tmpTyp < 64 ? 5 : tmpTyp < 84 ? 6 : tmpTyp < 530 ? 7 : tmpTyp < 670 ? 8 : tmpTyp < 850 ? 10 : 12;
            kv["SumEE"] = tmpO ? "(EE) " + FormatSimple(tmpEE) : "";
            string tmpF = tmpEE == 5 ? "1" : tmpEE == 6 ? "1.2" : (tmpEE == 7 || tmpEE == 8) ? "1.5" : tmpEE == 10 ? "2" : "2.7";
            kv["SumF"] = tmpO ? "(F) " + tmpF : "";
            kv["SumFTol"] = "+ " + FormatF3(0.1);
            kv["SumFTolN"] = "- " + FormatF3(0.1);

            kv["Sumn"] = tmpO ? "(n) " + FormatSimple(antalRepor) + "st." : "";
            kv["SumK"] = tmpO ? "(K) " + FormatSimple(repLaengd) : "";
            kv["SumJ"] = tmpO ? "(J) " + FormatSimple(laengdTillRepor) : "";
            string tmpM = tmpTyp < 64 ? "1" : "1.5";
            string tmpNn = tmpTyp < 64 ? "0.3" : "0.5";
            kv["SumM"] = tmpO ? "(M) " + tmpM : "";
            kv["SumNn"] = tmpO ? "(Nn) " + tmpNn : "";

            double tmpExTol = ComputeExTol(tmpKona, tmpd);
            kv["SumExTol"] = "+ " + FormatF3(tmpExTol) + " [3F]";
            kv["SumExTolN"] = "- " + FormatF1(0) + " [2F]";

            kv["SumR15"] = "R 1,5";
            kv["SumR15a"] = "R 1,5";
            kv["Sumr1"] = tmpEE == 5 ? "R4" : tmpEE == 6 ? "R4.5" : tmpEE == 7 ? "R5" : tmpEE == 8 ? "R6" : tmpEE == 10 ? "R7" : "R8";

            double tmpd1Tol = EqualsI(tmpBet2, "335376") ? 0 : Js9Tol(tmpd1);
            double tmpd1TolN = EqualsI(tmpBet2, "335376") ? 0.05 : Js9Tol(tmpd1);
            kv["Sumd1Tol"] = "+ " + FormatF3(tmpd1Tol) + " [3F]";
            kv["Sumd1TolN"] = "- " + FormatF3(tmpd1TolN) + " [3F]";
            kv["Sumd1"] = "(d1) " + FormatSimple(tmpd1);
            double tmpd1Toles = InnerAfterSplitPlus(tmpTyp);
            double tmpd1TolNes = InnerAfterSplitMinus(tmpTyp);
            kv["Sumd1Toles"] = "+ " + FormatF3(tmpd1Toles) + " [3F]";
            kv["Sumd1TolNes"] = "- " + FormatF3(tmpd1TolNes) + " [3F]";

            kv["SumBygel"] = ((tmpb2 - tmph) > 85 ? 7419471 : 7419469).ToString(CultureInfo.InvariantCulture);

            kv["SumB"] = tmpO ? "(B) " + FormatSimple(tmpB) : "Inga Oljespår";
            kv["SumBTol"] = tmpO ? GeneralTolText(tmpB) : "";
            double tmpBx = tmpL - tmpB;
            kv["SumBx"] = tmpO ? "(B) " + FormatSimple(tmpBx) : "Inga Oljespår";
            kv["SumBxTol"] = tmpO ? GeneralTolText(tmpB) : "";

            double tmpVOS = 20;
            double tmpds = RoundTo(((1.0 / SafeDivisor(tmpKona)) * (tmpAmaatt + tmpL - (tmpL - tmpb2) - tmph)) + tmpd, 0.001);
            kv["Sumds"] = "Största kondiameter (ds): " + FormatF3(tmpds).Replace(".",",");
            double tmpKonUtrakning = (((tmpL + tmpAmaatt - tmpB) / SafeDivisor(tmpKona)) + tmpd);
            double tmpKonstantOS = RoundTo(Math.Sin((tmpVOS / 2.0) * Math.PI / 180.0) * 2.0, 0.0001);
            double tmpKordaOSinv = RoundTo((tmpd1 / 2.0) * tmpKonstantOS, 0.1);
            double tmpKordaOSutv = RoundTo((tmpKonUtrakning / 2.0) * tmpKonstantOS, 0.1);
            kv["SumKOSinv"] = FormatSimple(tmpKordaOSinv);
            kv["SumKOSutv"] = FormatSimple(tmpKordaOSutv);

            double tmpRakA = StraightnessA(tmpdl);
            double tmpRakB = StraightnessB(tmpdl);
            kv["SumRakA"] = FormatF3(tmpRakA);
            kv["SumRakB"] = FormatF3(tmpRakB);
            kv["SumRd"] = FormatF3(tmpd1Tol);
            double tmpKs = AxialCast(tmpdl);
            kv["SumKs"] = FormatF3(tmpKs);
            double tmpKr = tmpSerie == "39" ? 0.082 : ThicknessVariation(tmpdl);
            kv["SumKr"] = "+ " + FormatF3(tmpKr) + " [2F]";

            double tmp8 = RoundTo(((tmpd - tmpd1) / 2.0) + ((tmpAmaatt + 8) / (2.0 * SafeDivisor(tmpKona))), 0.001);
            double tmp108 = RoundTo(((tmpd - tmpd1) / 2.0) + ((tmpAmaatt + 108) / (2.0 * SafeDivisor(tmpKona))), 0.001);
            double tmp40 = RoundTo(((tmpd - tmpd1) / 2.0) + ((tmpAmaatt + 40) / (2.0 * SafeDivisor(tmpKona))), 0.001);
            double tmp140 = RoundTo(((tmpd - tmpd1) / 2.0) + ((tmpAmaatt + 140) / (2.0 * SafeDivisor(tmpKona))), 0.001);
            double sumL1 = sumML < 145 ? 8 : 40;
            double sumL2 = sumML < 145 ? 108 : 140;
            double sumE1 = sumML < 145 ? tmp8 : tmp40;
            double sumE2 = sumML < 145 ? tmp108 : tmp140;
            kv["SumL1"] = FormatSimple(sumL1);
            kv["SumL2"] = FormatSimple(sumL2);
            kv["SumE1"] = FormatF3(sumE1).Replace(".", ",");
            kv["SumE2"] = FormatF3(sumE2).Replace(".", ",");
            kv["SumBML"] = "100";

            string tmpBygGtj = sumML < 145 ? "SR 7415983" : "SR 7419470 el. 7415991";
            kv["SumRa"] = "2.5 [2F]";
            kv["SumRa1"] = "2.5 [2F]";
            kv["SumRa5"] = "5 [2F]";

            string tmpRitNr = ComputeDrawingNumber(tmpSerie, tmpTyp, tmpO, tmpBet, tmpBet3);
            kv["SumRitNr"] = EqualsI(tmpRitningsnr, "0") || string.IsNullOrWhiteSpace(tmpRitningsnr) ? tmpRitNr : tmpRitningsnr;
            kv["SumRitNr2"] = kv["SumRitNr"];
            kv["SumGängRit"] = "237359:3, 7430181:2";
            kv["SumTolRit"] = "1432011:6, 7437495:4";
            kv["SumTolRit2"] = kv["SumTolRit"];
            kv["SumGGD"] = "7433015";
            kv["SumKlEgenskaperS1"] = "PRODUCTION NUTS & SLEEVES & HOUSINGS\\ALLMÄNKLASSADE EGENSKAPER\\Klassade egenskaper Avdragshylsor";
            kv["SumKlEgenskaperS2"] = kv["SumKlEgenskaperS1"];

            string tmpMaskinVal = (EqualsI(maskinVal, "VTR-160") || EqualsI(maskinVal, "MacTurn 550")) ? (tmpO ? "" : " - UTAN BORRHÅL & SPÅR") : "";
            kv["SumMaskinVal"] = ("Maskin: " + maskinVal + tmpMaskinVal + " - OP1").Trim();
            kv["SumMaskinValS2"] = ("Maskin: " + maskinVal + tmpMaskinVal + " - OP2").Trim();

            bool machineMatch = EqualsI(maskinVal, "VTR-160") || EqualsI(maskinVal, "MacTurn 550");

            kv["SumF1_1"] = machineMatch ? "1/1" : "";
            kv["SumF1_2"] = machineMatch ? "1/1" : "";
            kv["SumF1_3"] = machineMatch ? "Inst." : "";
            kv["SumF1_4"] = machineMatch ? "1/1" : "";
            kv["SumF1_5"] = tmpO && machineMatch ? "1/1" : "";
            kv["SumF1_6"] = machineMatch ? "Inst." : "";
            kv["SumF1_7"] = machineMatch ? "Inst." : "";
            kv["SumF1_8"] = machineMatch ? "Inst." : "";
            kv["SumF1_9"] = "";
            kv["SumF1_0"] = "";

            kv["SumF2_1"] = machineMatch ? "1/1" : "";
            kv["SumF2_2"] = machineMatch ? "1/1" : "";
            kv["SumF2_3"] = machineMatch ? "1/1" : "";
            kv["SumF2_4"] = tmpO && machineMatch ? "1/1" : "";
            kv["SumF2_5"] = machineMatch ? "Inst." : "";
            kv["SumF2_6"] = machineMatch ? "1/1" : "";
            kv["SumF2_7"] = machineMatch ? "1/1" : "";
            kv["SumF2_8"] = machineMatch ? "1/1" : "";
            kv["SumF2_9"] = machineMatch ? "1/1" : "";
            kv["SumF2_0"] = machineMatch ? "vid behov" : "";

            kv["SumD1_1"] = machineMatch ? "Skjutmått" : "";
            kv["SumD1_2"] = machineMatch ? "Multimar" : "";
            kv["SumD1_3"] = machineMatch ? "Gängmall" : "";
            kv["SumD1_4"] = machineMatch ? "Skjutmått" : "";
            kv["SumD1_5"] = tmpO && machineMatch ? "Gängtolk" : "";
            kv["SumD1_6"] = machineMatch ? "Skjutmått" : "";
            kv["SumD1_7"] = machineMatch ? "Skjutmått" : "";
            kv["SumD1_8"] = machineMatch ? "Skjutmått" : "";
            kv["SumD1_9"] = "";
            kv["SumD1_0"] = "";

            kv["SumD2_1"] = machineMatch ? "Klove" : "";
            kv["SumD2_2"] = machineMatch ? "Skjutmått" : "";
            kv["SumD2_3"] = machineMatch ? "Skjutmått" : "";
            kv["SumD2_4"] = tmpO && machineMatch ? "Skjutmått" : "";
            kv["SumD2_5"] = machineMatch ? "Egglinjal" : "";
            kv["SumD2_6"] = machineMatch ? "Mätbygel: " + tmpBygGtj : "";
            kv["SumD2_7"] = machineMatch ? "Mätbygel: " + tmpBygGtj : "";
            kv["SumD2_8"] = machineMatch ? "Mätbygel: " + tmpBygGtj : "";
            kv["SumD2_9"] = machineMatch ? "Okulärkontroll" : "";
            kv["SumD2_0"] = machineMatch ? "Mätmaskin" : "";

            kv["SumAF1_1"] = "";
            kv["SumAF1_2"] = "Rullar: Tr 6,0, inställd med klove";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = "";
            kv["SumAF1_6"] = "";
            kv["SumAF1_7"] = "";
            kv["SumAF1_8"] = "";
            kv["SumAF1_9"] = "";
            kv["SumAF1_0"] = "";

            kv["SumAF2_1"] = machineMatch ? "TOL efter slits:" : "";
            kv["SumAF2_2"] = "";
            kv["SumAF2_3"] = "";
            kv["SumAF2_4"] = "";
            kv["SumAF2_5"] = "";
            kv["SumAF2_6"] = machineMatch ? "GodstjockleksTOL:" : "";
            kv["SumAF2_7"] = "Tolerans: 0,012 [2F]";
            kv["SumAF2_8"] = "Max variation: 0,035";
            kv["SumAF2_9"] = machineMatch ? "Vid misstänkt fel Ra-mätare" : "";
            kv["SumAF2_0"] = machineMatch ? "Vid misstänkt formfel lämna till mätrum" : "";

            kv["SumBorrhålsgänga"] = tmpO ? "Borrhålsgänga" : "";
            kv["SumLängdtilloljespår"] = tmpO ? "Längd till oljespår" : "";
            kv["SumB2_4"] = tmpO ? "B2" : "";

            kv["SumTextS1"] = "Rätt märkning samt övriga mått kontrolleras vid inställning." + LB + "Okulär kontroll av Grader, gjuteridefekter, frifläckar, slagmärken, repor, valkar etc.";
            kv["SumTextS2"] = kv["SumTextS1"];

            kv["VaLPopUp"] = ComputePopup(req?.Published);
            BuildMeasureCenterMessages(kv, req, machineMatch, subject, maskinVal, tmpBygGtj, sumML, sumL1, sumL2, sumE1, sumE2);

            return kv;
        }

        private static void BuildMeasureCenterMessages(Dictionary<string, string> kv, APIRequest req, bool machineMatch, string subject, string maskinVal, string tmpBygGtj, double sumML, double sumL1, double sumL2, double sumE1, double sumE2)
        {
            kv["VaLFärdig"] = "";
            kv["VaLInfo"] = "";
            if (!machineMatch)
                return;

            DateTime now = DateTime.Now;
            bool afterNine = now.Hour > 9;
            double tmpBygelinstkonst = sumML < 145 ? -5 : -10;
            string tmpInstE1 = FormatF3(sumL1 == 8 ? sumE1 - 5 : sumE1 - 10);
            string tmpInstE2 = FormatF3(sumL2 == 108 ? sumE2 - 5 : sumE2 - 10);
            string readyText = afterNine ? "i morgon 10:00" : now.AddHours(3).ToString("H:mm", CultureInfo.InvariantCulture);
            kv["VaLFärdig"] = afterNine ? "Din beställning gjordes efter senast 10:00 så leverans kan ske i morgon vid 10:00" + LB + "Om du skulle behöva passbitarna tidigare så var god och kontakta mätsevice personligen för överrenskommelse" : "";
            kv["VaLInfo"] = "Du har gjort följande beställning ifrån mätcenter:" + LB + LB + LB +
                             "Passbitar till " + subject + " som körs vid " + maskinVal + LB + LB +
                             "Mätbygel: " + tmpBygGtj + LB + LB +
                             "Bygelinställningskonstant: " + FormatSimple(tmpBygelinstkonst) + LB + LB +
                             "E1 uträknat mått: " + tmpInstE1 + LB + LB +
                             "E2 uträknat mått: " + tmpInstE2 + LB + LB +
                             "Din beställning finns att hämta " + readyText;
        }

        private static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published))
                return "";
            if (!DateTime.TryParse(published, out var pubDt))
                return "";
            int dagar = 14;
            DateTime validTill = pubDt.AddDays(dagar);
            if (DateTime.Today > validTill.Date)
                return "";
            return "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom " + dagar.ToString(CultureInfo.InvariantCulture) + " dagar)" +
                   LB + LB +
                   LB + LB +
                   "Information om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument" +
                   LB + LB +
                   "Popupruta aktiv till " + validTill.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        private static string ComputeDrawingNumber(string serie, double typ, bool tmpO, string tmpBet, string tmpBet3)
        {
            string baseNo = serie == "22" ? "7437361" :
                            serie == "23" ? "7437360" :
                            serie == "30" ? ((Math.Abs(typ - 40) < 0.0001 && tmpO) ? "7435633" : "7437362") :
                            serie == "31" ? "7437364" :
                            serie == "32" ? (EqualsI(tmpBet3, "G") ? "7437366" : "231638") :
                            serie == "240" ? "7437370" :
                            serie == "241" ? "7437372" : tmpBet;
            if (!tmpO)
                return baseNo;
            if (serie == "22" || serie == "23")
                return baseNo + ", 7437987";
            if (serie == "30")
                return baseNo + (typ > 41 ? ", 7437376" : "");
            if (serie == "31")
                return baseNo + ", 7437378";
            if (serie == "32")
                return baseNo + ", 7437380";
            if (serie == "240")
                return baseNo + ", 7437397";
            if (serie == "241")
                return baseNo + ", 7437399";
            return baseNo;
        }

        private static string ComputeG1(double tmpStmm)
        {
            if (Math.Abs(tmpStmm - 8) < 0.0001) return "5.3x45º";
            if (Math.Abs(tmpStmm - 7) < 0.0001) return "4.4x45º";
            if (Math.Abs(tmpStmm - 6) < 0.0001) return "3.8x45º";
            if (Math.Abs(tmpStmm - 5) < 0.0001) return "3.2x45º";
            if (Math.Abs(tmpStmm - 4) < 0.0001) return "2.7x45º";
            if (Math.Abs(tmpStmm - 3) < 0.0001) return "2.4x45º";
            if (Math.Abs(tmpStmm - 2) < 0.0001) return "1.7x45º";
            return "1.3x45º";
        }

        private static string ComputeG2(string sumg1, int tmpRitSum, bool tmpRit3, bool tmpRit22, bool tmpRit23, bool tmpRit30, bool tmpRit31, bool tmpRit32, double tmpd2)
        {
            if (tmpRitSum == 0)
                return sumg1;
            if (tmpRit3)
                return Math.Abs(tmpd2 - 80) < 0.0001 ? "1.5x45º" : sumg1;
            if (tmpRit22)
                return tmpd2 < 210 ? "2.5x45º" : sumg1;
            if (tmpRit23)
            {
                if (tmpd2 < 69) return sumg1;
                if (tmpd2 < 76) return "1.75x45º";
                if (Math.Abs(tmpd2 - 80) < 0.0001 || Math.Abs(tmpd2 - 140) < 0.0001 || Math.Abs(tmpd2 - 150) < 0.0001) return "1.5x45º";
                if (tmpd2 < 155) return "1.7x45º";
                if (tmpd2 < 210) return "2.25x45º";
                return sumg1;
            }
            if (tmpRit30)
            {
                if (tmpd2 < 195) return sumg1;
                if (Math.Abs(tmpd2 - 200) < 0.0001) return "2.5x45º";
                if (tmpd2 < 250) return "2.75x45º";
                if (tmpd2 < 310) return "2.7x45º";
                return sumg1;
            }
            if (tmpRit31)
            {
                if (tmpd2 < 160) return "1.7x45º";
                if (tmpd2 < 210) return "2.25x45º";
                if (tmpd2 < 310) return "2.7x45º";
                if (Math.Abs(tmpd2 - 320) < 0.0001) return sumg1;
                if (tmpd2 < 490) return "3.25x45º";
                return sumg1;
            }
            if (tmpRit32)
            {
                if (Math.Abs(tmpd2 - 140) < 0.0001 || Math.Abs(tmpd2 - 150) < 0.0001) return "1.5x45º";
                if (tmpd2 < 135) return "1.7x45º";
                if (tmpd2 < 210) return "2.25x45º";
                if (Math.Abs(tmpd2 - 220) < 0.0001) return "2.7x45º";
                if (tmpd2 < 501) return "3.25x45º";
                if (tmpd2 < 580) return "4x45º";
                if (Math.Abs(tmpd2 - 600) < 0.0001) return "3.8x45º";
                if (tmpd2 < 700) return "4x45º";
                if (tmpd2 < 790) return "4.5x45º";
                if (tmpd2 < 890) return "4.4x45º";
                if (Math.Abs(tmpd2 - 950) < 0.0001) return "5.5x45º";
                if (Math.Abs(tmpd2 - 1000) < 0.0001) return "5x45º";
                return "5.3x45º";
            }
            return "Fel Typ";
        }

        private static double ComputeExTol(double kona, double d)
        {
            if (Math.Abs(kona - 12) < 0.0001)
            {
                if (d < 31) return 0.033;
                if (d < 51) return 0.039;
                if (d < 81) return 0.046;
                if (d < 121) return 0.054;
                if (d < 181) return 0.063;
                if (d < 251) return 0.072;
                if (d < 316) return 0.081;
                if (d < 401) return 0.089;
                if (d < 501) return 0.097;
                if (d < 631) return 0.105;
                if (d < 801) return 0.115;
                if (d < 1001) return 0.130;
                return 0.145;
            }
            if (Math.Abs(kona - 30) < 0.0001)
            {
                if (d < 121) return 0.035;
                if (d < 181) return 0.040;
                if (d < 251) return 0.046;
                if (d < 316) return 0.052;
                if (d < 401) return 0.057;
                if (d < 501) return 0.063;
                if (d < 631) return 0.068;
                if (d < 801) return 0.076;
                if (d < 1001) return 0.084;
                return 0.095;
            }
            return 0;
        }

        private static double InnerAfterSplitPlus(double typ)
        {
            if (typ < 12) return 0.062;
            if (typ < 18) return 0.074;
            if (typ < 26) return 0.140;
            if (typ < 40) return 0.160;
            if (typ < 56) return 0.185;
            if (typ < 68) return 0.210;
            if (typ < 88) return 0.360;
            if (typ < 560) return 0.400;
            if (typ < 710) return 0.440;
            if (typ < 900) return 0.500;
            return 0.560;
        }

        private static double InnerAfterSplitMinus(double typ)
        {
            if (typ < 12) return 0.100;
            if (typ < 18) return 0.120;
            if (typ < 26) return 0.220;
            if (typ < 40) return 0.250;
            if (typ < 56) return 0.290;
            if (typ < 68) return 0.320;
            if (typ < 88) return 0.570;
            if (typ < 560) return 0.630;
            if (typ < 710) return 0.700;
            if (typ < 900) return 0.800;
            return 0.900;
        }

        private static string GeneralTolText(double value)
        {
            if (value < 30.01) return "± 0.2";
            if (value < 120.01) return "± 0.3";
            if (value < 400.01) return "± 0.5";
            return "± 0.8";
        }

        private static double StraightnessA(double dl)
        {
            if (dl < 101) return 0.008;
            if (dl < 281) return 0.010;
            if (dl < 481) return 0.012;
            if (dl < 601) return 0.014;
            if (dl < 901) return 0.016;
            return 0.020;
        }

        private static double StraightnessB(double dl)
        {
            if (dl < 101) return 0.012;
            if (dl < 281) return 0.015;
            if (dl < 481) return 0.018;
            if (dl < 601) return 0.021;
            if (dl < 901) return 0.024;
            return 0.030;
        }

        private static double AxialCast(double dl)
        {
            if (dl > 1001) return 0.160;
            if (dl > 801) return 0.140;
            if (dl > 631) return 0.120;
            if (dl > 501) return 0.100;
            if (dl > 401) return 0.090;
            if (dl > 316) return 0.080;
            if (dl > 251) return 0.070;
            if (dl > 121) return 0.060;
            if (dl > 51) return 0.050;
            return 0.040;
        }

        private static double ThicknessVariation(double dl)
        {
            if (dl < 51) return 0.008;
            if (dl < 121) return 0.010;
            if (dl < 181) return 0.015;
            if (dl < 251) return 0.020;
            if (dl < 316) return 0.025;
            if (dl < 501) return 0.030;
            if (dl < 631) return 0.035;
            if (dl < 801) return 0.040;
            if (dl < 1001) return 0.045;
            return 0.050;
        }

        private static double ComputeStigning(double d2)
        {
            if (d2 < 25) return 1;
            if (d2 < 55) return 1.5;
            if (d2 < 155) return 2;
            if (d2 < 205) return 3;
            if (d2 < 301) return 4;
            if (d2 < 501) return 5;
            if (d2 < 701) return 6;
            if (d2 < 901) return 7;
            return 8;
        }

        private static double GeneralPlusTol(double value)
        {
            if (value < 6.01) return 0.1;
            if (value < 30.01) return 0.2;
            if (value < 120.01) return 0.3;
            if (value < 315.01) return 0.5;
            if (value < 1000.01) return 0.8;
            if (value < 2000.01) return 1.2;
            return 2.0;
        }

        private static double Js15Tol(double value)
        {
            if (value < 3.01) return 0.200;
            if (value < 6.01) return 0.240;
            if (value < 10.01) return 0.290;
            if (value < 18.01) return 0.350;
            if (value < 30.01) return 0.420;
            if (value < 50.01) return 0.500;
            if (value < 80.01) return 0.600;
            if (value < 120.01) return 0.700;
            if (value < 180.01) return 0.800;
            if (value < 250.01) return 0.925;
            if (value < 315.01) return 1.050;
            if (value < 400.01) return 1.150;
            if (value < 500.01) return 1.250;
            return 1.400;
        }

        private static double H13NegTol(double value)
        {
            if (value < 3.01) return 0.140;
            if (value < 6.01) return 0.180;
            if (value < 10.01) return 0.220;
            if (value < 18.01) return 0.270;
            if (value < 30.01) return 0.330;
            if (value < 50.01) return 0.390;
            if (value < 80.01) return 0.460;
            if (value < 120.01) return 0.540;
            if (value < 180.01) return 0.630;
            if (value < 250.01) return 0.720;
            if (value < 315.01) return 0.810;
            if (value < 400.01) return 0.890;
            if (value < 500.01) return 0.970;
            if (value < 630.01) return 1.100;
            if (value < 800.01) return 1.250;
            if (value < 1000.01) return 1.400;
            if (value < 1250.01) return 1.650;
            if (value < 1600.01) return 1.950;
            if (value < 2000.01) return 2.300;
            if (value < 2500.01) return 2.800;
            return 3.300;
        }

        private static double Js9Tol(double value)
        {
            if (value < 31) return 0.026;
            if (value < 51) return 0.031;
            if (value < 81) return 0.037;
            if (value < 121) return 0.043;
            if (value < 181) return 0.050;
            if (value < 251) return 0.057;
            if (value < 316) return 0.065;
            if (value < 401) return 0.070;
            if (value < 501) return 0.077;
            if (value < 631) return 0.087;
            if (value < 801) return 0.100;
            if (value < 1001) return 0.115;
            return 0.130;
        }

        private static string NormalizeToken(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return "0";
            if (double.TryParse(s.Replace('.', ','), NumberStyles.Any, CommonFunctions.Culture, out var d))
                return d % 1 == 0 ? ((int)d).ToString(CultureInfo.InvariantCulture) : d.ToString(CommonFunctions.Culture).Replace(',', '.');
            return s.Trim();
        }

        private static string[] Explode(string s, char[] separators)
        {
            if (string.IsNullOrWhiteSpace(s))
                return Array.Empty<string>();
            return s.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        }

        private static string Word(string[] list, int index1Based)
        {
            if (list == null || index1Based <= 0 || index1Based > list.Length)
                return "";
            return list[index1Based - 1] ?? "";
        }

        private static bool EqualsI(string a, string b)
        {
            return string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);
        }

        private static bool ContainsI(string a, string b)
        {
            return (a ?? "").IndexOf(b ?? "", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static string Left(string s, int n)
        {
            if (string.IsNullOrEmpty(s) || n <= 0)
                return "";
            return s.Length <= n ? s : s.Substring(0, n);
        }

        private static string Right(string s, int n)
        {
            if (string.IsNullOrEmpty(s) || n <= 0)
                return "";
            return s.Length <= n ? s : s.Substring(s.Length - n);
        }

        private static double ToDouble(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return 0;
            s = s.Replace('.', ',');
            return double.TryParse(s, NumberStyles.Any, CommonFunctions.Culture, out var v) ? v : 0;
        }

        private static double GetDouble(List<Bookmark> bm, string key)
        {
            string raw = GetString(bm, key);
            if (string.IsNullOrWhiteSpace(raw))
                return 0;
            raw = raw.Trim().Replace('.', ',');
            return double.TryParse(raw, NumberStyles.Any, CommonFunctions.Culture, out var v) ? v : 0;
        }

        private static string GetString(List<Bookmark> bm, string key)
        {
            if (bm == null || string.IsNullOrWhiteSpace(key))
                return "";
            for (int i = 0; i < bm.Count; i++)
            {
                var b = bm[i];
                if (b != null && string.Equals(b.BookmarkName ?? "", key, StringComparison.OrdinalIgnoreCase))
                    return b.BookmarkValue ?? "";
            }
            return "";
        }

        private static double RoundTo(double value, double step)
        {
            if (step <= 0)
                return value;
            double x = value / step;
            double r = x >= 0 ? Math.Floor(x + 0.5) : Math.Ceiling(x - 0.5);
            return r * step;
        }

        private static double SafeDivisor(double value)
        {
            return Math.Abs(value) < 0.0000001 ? 1 : value;
        }

        private static string FormatSimple(double value)
        {
            if (Math.Abs(value - Math.Round(value)) < 0.0000001)
                return Math.Round(value).ToString(CultureInfo.InvariantCulture);
            return value.ToString("0.###", CommonFunctions.Culture).Replace(',', '.');
        }

        private static string FormatF1(double value)
        {
            return value.ToString("F1", CommonFunctions.Culture).Replace(',', '.');
        }

        private static string FormatF2(double value)
        {
            return value.ToString("F2", CommonFunctions.Culture).Replace(',', '.');
        }

        private static string FormatF3(double value)
        {
            return value.ToString("F3", CommonFunctions.Culture).Replace(',', '.');
        }

        private static void InitializeKeys(Dictionary<string, string> kv)
        {
            foreach (var key in Keys)
                kv[key] = "";
        }

        private static readonly string[] Keys = new[]
        {
            "VaLPopUp","VaLFärdig","VaLInfo",
            "SumV","SumKona","SumVmått","Sumdl","SumML","SumP","SumKonavv","SumT","SumC2","SumR","SumR2","SumS","SumSg","SumRullar","SumGänga","SumZ",
            "Sumd4","Sumd4Tol","Sumd4TolN","Sumd2","Sumd2a","Sumd2Tol","Sumd2TolN","Sumd2aTol","Sumd2aTolN","Sumdm","SumdmTol","SumdmTolN",
            "SumBorrHAnt","Sumß","SumGH","SumC","SumG","SumA","SumDB","Sumb2SSK","Sumb2","Sumb2Tol","SumLSSK","SumL","SumLTol","SumLTolN","SumHm","SumHmTol","SumHmTolN","Sumh","Sumg1","Sumg2",
            "SumEE","SumF","SumFTol","SumFTolN","Sumn","SumK","SumJ","SumM","SumNn","SumExTol","SumExTolN","SumR15","SumR15a","Sumr1","Sumd1Tol","Sumd1TolN","Sumd1","Sumd1Toles","Sumd1TolNes",
            "SumBygel","SumB","SumBTol","SumBx","SumBxTol","Sumds","SumKOSinv","SumKOSutv","SumRakA","SumRakB","SumRd","SumKs","SumKr","SumL1","SumL2","SumE1","SumE2","SumBML","SumRa","SumRa1","SumRa5",
            "SumRitNr","SumRitNr2","SumGängRit","SumTolRit","SumTolRit2","SumGGD","SumKlEgenskaperS1","SumKlEgenskaperS2","SumMaskinVal","SumMaskinValS2",
            "SumF1_1","SumF1_2","SumF1_3","SumF1_4","SumF1_5","SumF1_6","SumF1_7","SumF1_8","SumF1_9","SumF1_0",
            "SumF2_1","SumF2_2","SumF2_3","SumF2_4","SumF2_5","SumF2_6","SumF2_7","SumF2_8","SumF2_9","SumF2_0",
            "SumD1_1","SumD1_2","SumD1_3","SumD1_4","SumD1_5","SumD1_6","SumD1_7","SumD1_8","SumD1_9","SumD1_0",
            "SumD2_1","SumD2_2","SumD2_3","SumD2_4","SumD2_5","SumD2_6","SumD2_7","SumD2_8","SumD2_9","SumD2_0",
            "SumAF1_1","SumAF1_2","SumAF1_3","SumAF1_4","SumAF1_5","SumAF1_6","SumAF1_7","SumAF1_8","SumAF1_9","SumAF1_0",
            "SumAF2_1","SumAF2_2","SumAF2_3","SumAF2_4","SumAF2_5","SumAF2_6","SumAF2_7","SumAF2_8","SumAF2_9","SumAF2_0",
            "SumBorrhålsgänga","SumLängdtilloljespår","SumB2_4","SumTextS1","SumTextS2"
        };
    }
}
