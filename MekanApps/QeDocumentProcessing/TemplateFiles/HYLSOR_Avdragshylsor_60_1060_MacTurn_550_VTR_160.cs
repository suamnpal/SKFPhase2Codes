using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Avdragshylsor_60_1060_MacTurn_550_VTR_160 : ITemplateCalculations
    {
        private static readonly string[] Machines = new[] { "VTR-160", "MacTurn 550" };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;
            List<Bookmark> bm = req != null ? req.Bookmarks : null;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");

            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : string.Empty;
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : string.Empty;
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : string.Empty;

            bool tmpSlash = tmpBet.IndexOf('/') >= 0;
            bool tmpMS = ContainsI(tmpBet, "MS");
            bool tmpO = EqualsI(tmpBet1, "MS") || ContainsI(tmpBet, "O");

            int tmpCountB2 = tmpBet2.Length;

            string tmpSerie;
            if (tmpCountB2 == 3 || tmpCountB2 == 2)
                tmpSerie = tmpBet2;
            else if (tmpCountB2 > 4)
                tmpSerie = tmpBet2.Substring(0, 3);
            else
                tmpSerie = tmpBet2.Length >= 2 ? tmpBet2.Substring(0, 2) : tmpBet2;

            double tmpKonringsdiameter = GetDouble(bm, "Konringsdiameter (d)");
            string tmpTypStr;
            if (tmpMS)
                tmpTypStr = tmpKonringsdiameter.ToString(CommonFunctions.Culture);
            else if (tmpCountB2 > 3)
                tmpTypStr = tmpBet2.Length >= 2 ? tmpBet2.Substring(tmpBet2.Length - 2) : tmpBet2;
            else if (tmpSlash)
                tmpTypStr = tmpBet3;
            else if (tmpCountB2 < 2)
                tmpTypStr = "0";
            else
                tmpTypStr = tmpBet3;

            int serieInt = TryParseInt(tmpSerie);
            double typNum = TryParseDouble(tmpTypStr);

            string tmpRitningsnr = GetString(bm, "Ritningsnummer").ToUpperInvariant().Trim();
            bool tmpRitAHA = tmpRitningsnr.IndexOf("AHA", StringComparison.Ordinal) >= 0;
            bool tmpRitAOH = tmpRitningsnr.IndexOf("AOH", StringComparison.Ordinal) >= 0;
            bool tmpRit3 = tmpRitningsnr.IndexOf("7437359", StringComparison.Ordinal) >= 0;
            bool tmpRit22 = tmpRitningsnr.IndexOf("7437361", StringComparison.Ordinal) >= 0;
            bool tmpRit23 = tmpRitningsnr.IndexOf("7437360", StringComparison.Ordinal) >= 0;
            bool tmpRit30 = tmpRitningsnr.IndexOf("7437362", StringComparison.Ordinal) >= 0;
            bool tmpRit31 = tmpRitningsnr.IndexOf("7437364", StringComparison.Ordinal) >= 0;
            bool tmpRit32 = tmpRitningsnr.IndexOf("7437366", StringComparison.Ordinal) >= 0 ||
                              tmpRitningsnr.IndexOf("7437367", StringComparison.Ordinal) >= 0;
            int tmpRitSum = B2I(tmpRit3) + B2I(tmpRit22) + B2I(tmpRit23) + B2I(tmpRit30) + B2I(tmpRit31) + B2I(tmpRit32);

            double tmpL = GetDouble(bm, "Längd (L)");
            double tmpb2 = GetDouble(bm, "Längd till gänga (b)");
            double tmph = GetDouble(bm, "Släppning (h)");
            double tmpd4 = GetDouble(bm, "Släppningsdiameter (d4)");
            double tmpBeta = GetDouble(bm, "Borrvinkel (ß)");
            double tmpB = GetDouble(bm, "Längd till oljespår (B)");
            double tmpd2 = GetDouble(bm, "Ytterdiameter (d2)");
            double tmpd1 = GetDouble(bm, "Innerdiameter (d1)");
            double amatt = GetDouble(bm, "Amått");

            double tmpKona = GetDouble(bm, "Kona");
            if (tmpKona == 0)
            {
                if (serieInt == 240 || serieInt == 241) tmpKona = 30;
                else if (EqualsI(tmpSerie, "LW")) tmpKona = 0;
                else tmpKona = 12;
            }
            kv["SumV"] = Math.Abs(tmpKona - 30) < 0.001 ? "0º57" : "2º23";
            kv["SumKona"] = "Kona 1:" + Fmt(tmpKona);
            kv["SumVmått"] = Math.Abs(tmpKona - 30) < 0.001 ? "50.6" : "55.6";

            double tmpd;
            double dBm = GetDouble(bm, "Konringsdiameter (d)");
            if (dBm != 0)
                tmpd = dBm;
            else if (tmpCountB2 > 3)
                tmpd = (typNum / 2.0) * 10.0;
            else
                tmpd = typNum;

            double tmpdl = Math.Round((1.0 / tmpKona) * amatt + tmpd, 3);
            kv["Sumdl"] = "Minsta kondiameter (dl): " + FmtFixedComma(tmpdl, 3);

            double tmpHm = tmpL - tmpb2;
            double tmpds = Math.Round((1.0 / tmpKona * (amatt + tmpL - tmpHm - tmph)) + tmpd, 3);
            kv["Sumds"] = "Största kondiameter (ds): " + FmtFixedComma(tmpds, 3);

            double sumML = Math.Round((tmpb2 - tmph - 16) / 5.0, MidpointRounding.AwayFromZero) * 5;
            kv["SumML"] = Fmt(sumML);

            double stmm = Stmm(tmpd2);
            kv["SumP"] = "(P) " + (tmpRitAHA ? "6" : Fmt1(stmm));

            double tmpVT = VTFactor(tmpd);
            double konavv = Math.Round(100.0 * tmpVT / 1000.0, 3);
            kv["SumKonavv"] = FmtFixedComma(konavv, 3) + " [2F]";

            bool tmpTa = GetString(bm, "Gängtapp (R)").IndexOf("1/4", StringComparison.Ordinal) >= 0;
            string tmpT = tmpTa ? "13.5" : "10";
            kv["SumT"] = tmpO ? "(T) " + tmpT : "";

            int tmpC2val = typNum < 501 ? 8 : 10;
            kv["SumC2"] = "(C2) " + tmpC2val.ToString(CultureInfo.InvariantCulture);

            string tmpRStr = GetString(bm, "Gängtapp (R)");
            kv["SumR"] = tmpO ? tmpRStr : "";
            kv["SumR2"] = kv["SumR"];

            double tmpS = GetDouble(bm, "Längd gänghål (S)");
            kv["SumS"] = tmpO ? "(S) " + Fmt(tmpS) : "";

            string sgRaw = GetString(bm, "Gänglängd gänghål (Sg)");
            string tmpSg;
            if (EqualsI(sgRaw, "0") || string.IsNullOrEmpty(sgRaw))
                tmpSg = Fmt(tmpS - 2);
            else if (EqualsI(sgRaw, "none"))
                tmpSg = "";
            else
                tmpSg = sgRaw;
            kv["SumSg"] = EqualsI(sgRaw, "none") ? "" : "(Sg) min:" + tmpSg;

            string sumRullar;
            if (tmpRitAHA)
                sumRullar = "(P) 6 UN";
            else if (stmm == 1 || stmm == 1.5 || stmm == 2 || stmm == 3)
                sumRullar = "M " + Fmt1(stmm);
            else
                sumRullar = "Tr " + Fmt1(stmm);
            kv["SumRullar"] = sumRullar;

            string sumGanga;
            if (tmpRitAHA)
                sumGanga = "11,004x6UN";
            else if (stmm == 1 || stmm == 1.5 || stmm == 2 || stmm == 3)
                sumGanga = "M " + Fmt(tmpd2) + "x " + Fmt1(stmm);
            else
                sumGanga = "Tr " + Fmt(tmpd2) + "x " + Fmt1(stmm);
            kv["SumGänga"] = sumGanga;

            kv["SumZ"] = "(Z) Ø" + Fmt(tmpd1 - 4.0);

            kv["Sumd4"] = "(d4) " + Fmt(tmpd4);
            kv["Sumd4Tol"] = "+ " + Fmt3(D4TolPos(serieInt, tmpd4));
            kv["Sumd4TolN"] = "- " + Fmt3(D4TolNeg(serieInt, tmpd4));

            kv["Sumd2"] = "(d2) " + Fmt(tmpd2);
            kv["Sumd2a"] = "(d2) " + Fmt(tmpd2);
            kv["Sumd2Tol"] = "+ 0.0";
            kv["Sumd2TolN"] = "- " + Fmt3(D2TolN(stmm));
            kv["Sumd2aTol"] = kv["Sumd2Tol"];
            kv["Sumd2aTolN"] = kv["Sumd2TolN"];

            double tmpdm = DM(tmpd2, stmm);
            kv["Sumdm"] = "(dm) " + Fmt(tmpdm);
            kv["SumdmTol"] = "- " + Fmt3(DmTol(stmm)) + " [3F]";
            kv["SumdmTolN"] = "- " + Fmt3(DmTolN(stmm)) + " [3F]";

            double tmpd3 = D3Val(tmpdm, stmm);
            kv["Sumd3"] = Fmt(tmpd3);

            string sumBorrhAnt = (tmpO && tmpd1 < 195)
                ? "OBS! OBS! OBS!\nBara 1 borrhål med genomgående hål till inv. & utv. oljespår"
                : "";
            kv["SumBorrHAnt"] = sumBorrhAnt;

            string betaStr = FmtFixedComma(tmpBeta, 2);
            kv["Sumß"] = (Math.Abs(tmpBeta) < 0.0001) ? "" : "(ß) " + betaStr + "º";
            kv["SumV"] = tmpO ? (tmpd1 < 195 ? "Rakt genomgående" : "30º") : "";

            string tmpGH = GH(serieInt, tmpd1);
            kv["SumGH"] = "(GH) " + tmpGH;

            double cBm = GetDouble(bm, "Längd Borrhål (C)");
            double tmpCval = cBm != 0 ? cBm : (tmpd < 421 ? tmpB + 3 : tmpB + 4);
            kv["SumC"] = tmpO ? "(C) " + Fmt(tmpCval) : "";

            string tmpGstr = GetString(bm, "Diameter Borrhål (G)");
            kv["SumG"] = tmpO ? "(G) " + tmpGstr : "";

            double invBmRaw = GetDouble(bm, "Mått inv till borrcentrum (A)");
            double tmpAval = invBmRaw < 30 ? invBmRaw : (invBmRaw - tmpd1) / 2.0;
            double tmpDB = invBmRaw < 30 ? tmpd1 + (invBmRaw * 2.0) : invBmRaw;
            kv["SumA"] = tmpO ? "(A) " + Fmt(tmpAval) : "";
            kv["SumDB"] = tmpO ? "(DB) " + Fmt(tmpDB) : "--< Inga Oljeborrhål >--";

            double tmpb2Tol = B2Tol(tmpb2);
            kv["Sumb2SSK"] = "(b) ~" + Fmt(tmpb2 + 4);
            kv["Sumb2"] = "(b) " + Fmt(tmpb2);
            kv["Sumb2Tol"] = "± " + Fmt3(tmpb2Tol) + " [3F]";

            double lTolN = LTolN(tmpL);
            kv["SumLSSK"] = "(L) ~" + Fmt(tmpL + 4);
            kv["SumL"] = "(L) " + Fmt(tmpL);
            kv["SumLTol"] = "+ 0.0 [3F]";
            kv["SumLTolN"] = "- " + Fmt3(lTolN) + " [3F]";

            kv["SumHm"] = "(Hm) " + Fmt(tmpHm);
            kv["SumHmTol"] = "+ " + FmtDotRemoveLeadingZero(tmpb2Tol);
            kv["SumHmTolN"] = "- " + FmtDotRemoveLeadingZero(tmpb2Tol + lTolN);

            kv["Sumh"] = "(h) " + Fmt(tmph);

            kv["Sumg1"] = Sumg1(tmpBet2, stmm);
            kv["Sumg2"] = Sumg2(tmpRitSum, tmpRit3, tmpRit22, tmpRit23, tmpRit30, tmpRit31, tmpRit32, tmpd2, kv["Sumg1"]);

            int tmpEE = EE(typNum);
            kv["SumEE"] = tmpO ? "(EE) " + tmpEE.ToString(CultureInfo.InvariantCulture) : "";

            string tmpFstr = FDepth(tmpEE, serieInt);
            double tmpFTolPos = serieInt == 240 ? 0.0 : 0.1;
            double tmpFTolNeg = serieInt == 240 ? 0.2 : 0.1;
            kv["SumF"] = tmpO ? "(F) " + tmpFstr : "";
            kv["SumFTol"] = "+ " + Fmt3(tmpFTolPos);
            kv["SumFTolN"] = "- " + Fmt3(tmpFTolNeg);

            kv["Sumn"] = tmpO ? "(n) " + GetString(bm, "Antal repor (n)") + "st." : "";
            kv["SumK"] = tmpO ? "(K) " + GetString(bm, "Replängd (K)") : "";
            kv["SumJ"] = tmpO ? "(J) " + GetString(bm, "Längd till repor (J)") : "";
            kv["SumM"] = tmpO ? "(M) " + RepM(typNum) : "";
            kv["SumNn"] = tmpO ? "(Nn) " + RepNn(typNum) : "";

            kv["SumExTol"] = "+ " + Fmt3(ExTol(tmpKona, tmpd)) + " [3F]";
            kv["SumExTolN"] = "- 0.0 [2F]";

            kv["SumR15"] = serieInt == 336 ? "1,0 x 45°" : "R 1,5";
            kv["SumR15a"] = "R 1,5";
            kv["Sumr1"] = R1(tmpEE);

            bool is335376 = EqualsI(tmpBet2, "335376");
            double tmpd1Tol = D1Tol(tmpd1, is335376);
            double tmpd1TolN = D1TolN(tmpd1, is335376);
            kv["Sumd1Tol"] = "+ " + Fmt3(tmpd1Tol) + " [3F]";
            kv["Sumd1TolN"] = "- " + Fmt3(tmpd1TolN) + " [3F]";
            kv["Sumd1"] = "(d1) " + Fmt(tmpd1);
            kv["Sumd1Toles"] = "+ " + Fmt3(D1TolES(typNum)) + " [3F]";
            kv["Sumd1TolNes"] = "- " + Fmt3(D1TolNES(typNum)) + " [3F]";

            kv["SumBygel"] = ((tmpb2 - tmph) > 85 ? 7419471 : 7419469).ToString(CultureInfo.InvariantCulture);

            kv["SumB"] = tmpO ? "(B) " + Fmt(tmpB) : "Inga Oljespår";
            kv["SumBTol"] = tmpO ? BTol(tmpB) : "";

            double tmpBx = tmpL - tmpB;
            kv["SumBx"] = tmpO ? "(B) " + Fmt(tmpBx) : "Inga Oljespår";
            kv["SumBxTol"] = tmpO ? BTol(tmpB) : "";

            double tmpKonUtr = ((tmpL + amatt - tmpB) / tmpKona) + tmpd;
            double tmpKonstOS = Math.Round(Math.Sin((20.0 / 2.0) * Math.PI / 180.0) * 2.0, 4);
            double tmpKordaOSinv = Math.Round((tmpd1 / 2.0) * tmpKonstOS, 1);
            double tmpKordaOSutv = Math.Round((tmpKonUtr / 2.0) * tmpKonstOS, 1);
            kv["SumKOSinv"] = Fmt(tmpKordaOSinv);
            kv["SumKOSutv"] = Fmt(tmpKordaOSutv);

            kv["SumRakA"] = FmtDiv3(RakA(tmpdl));
            kv["SumRakB"] = FmtDiv3(RakB(tmpdl));

            kv["SumRd"] = Fmt3(tmpd1Tol);

            kv["SumKs"] = FmtDiv3(Ks(tmpdl));

            kv["SumKr"] = "+ " + Fmt3(Kr(serieInt, tmpdl)) + " [2F]";

            double tmp8val = Math.Round(((tmpd - tmpd1) / 2.0) + ((amatt + 8) / (2.0 * tmpKona)), 3);
            double tmp108val = Math.Round(((tmpd - tmpd1) / 2.0) + ((amatt + 108) / (2.0 * tmpKona)), 3);
            double tmp40val = Math.Round(((tmpd - tmpd1) / 2.0) + ((amatt + 40) / (2.0 * tmpKona)), 3);
            double tmp140val = Math.Round(((tmpd - tmpd1) / 2.0) + ((amatt + 140) / (2.0 * tmpKona)), 3);
            kv["SumL1"] = sumML < 145 ? "8" : "40";
            kv["SumL2"] = sumML < 145 ? "108" : "140";
            kv["SumE1"] = Fmt(sumML < 145 ? tmp8val : tmp40val);
            kv["SumE2"] = Fmt(sumML < 145 ? tmp108val : tmp140val);

            kv["SumBML"] = "100";

            string tmpBygGtj = sumML < 145 ? "SR 7415983" : "SR 7419470 el. 7415991";
            kv["SumBygGtj"] = tmpBygGtj;

            kv["SumRa"] = "2.5 [2F]";
            kv["SumRa1"] = "2.5 [2F]";
            kv["SumRa5"] = "5 [2F]";

            kv["SumRitNr"] = BuildRitNr(tmpRitningsnr, tmpSerie, serieInt, typNum, tmpO, tmpBet3, tmpBet);
            kv["SumRitNr2"] = kv["SumRitNr"];
            kv["SumGängRit"] = "237359:3, 7430181:2";
            kv["SumTolRit"] = "1432011:6, 7437495:4";
            kv["SumTolRit2"] = kv["SumTolRit"];
            kv["SumGGD"] = "7433015";
            kv["SumKlEgenskaperS1"] = "PRODUCTION NUTS & SLEEVES & HOUSINGS\\ALLMÄNKLASSADE EGENSKAPER\\Klassade egenskaper Avdragshylsor";
            kv["SumKlEgenskaperS2"] = kv["SumKlEgenskaperS1"];

            string tmpMaskinValSuffix = IsMachine(maskinVal) ? (tmpO ? "" : " - UTAN BORRHÅL & SPÅR") : "";
            kv["SumMaskinVal"] = "Maskin: " + maskinVal + tmpMaskinValSuffix + " - OP1";
            kv["SumMaskinValS2"] = "Maskin: " + maskinVal + tmpMaskinValSuffix + " - OP2";

            SumFrequencies(kv, maskinVal, tmpO);
            SumMeasuringDevices(kv, maskinVal, tmpO, sumRullar, tmpBygGtj);
            SumAF(kv, maskinVal, tmpO, sumRullar, kv["SumKonavv"], Fmt3(Kr(serieInt, tmpdl)));

            kv["SumBorrhålsgänga"] = tmpO ? "Borrhålsgänga" : "";
            kv["SumLängdtilloljespår"] = tmpO ? "Längd till oljespår" : "";
            kv["SumB2_4"] = tmpO ? "B2" : "";

            kv["SumTextS1"] = "Rätt märkning samt övriga mått kontrolleras vid inställning.<<LineBreak>>Okulär kontroll av Grader, gjuteridefekter, frifläckar, slagmärken, repor, valkar etc.";
            kv["SumTextS2"] = kv["SumTextS1"];

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
                       " dagar)\n\nInformation om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument\n\nPopupruta aktiv till " +
                       validTill.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return "";
        }

        private static void SumFrequencies(Dictionary<string, string> kv, string maskinVal, bool tmpO)
        {
            bool m = IsMachine(maskinVal);
            kv["SumF1_1"] = m ? "1/1" : "";
            kv["SumF1_2"] = m ? "1/1" : "";
            kv["SumF1_3"] = m ? "Inst." : "";
            kv["SumF1_4"] = m ? "1/1" : "";
            kv["SumF1_5"] = tmpO ? (m ? "1/1" : "") : "";
            kv["SumF1_6"] = m ? "Inst." : "";
            kv["SumF1_7"] = m ? "Inst." : "";
            kv["SumF1_8"] = m ? "Inst." : "";
            kv["SumF1_9"] = "";
            kv["SumF1_0"] = "";
            kv["SumF2_1"] = m ? "1/1" : "";
            kv["SumF2_2"] = m ? "1/1" : "";
            kv["SumF2_3"] = m ? "1/1" : "";
            kv["SumF2_4"] = tmpO ? (m ? "1/1" : "") : "";
            kv["SumF2_5"] = m ? "Inst." : "";
            kv["SumF2_6"] = m ? "1/1" : "";
            kv["SumF2_7"] = m ? "1/1" : "";
            kv["SumF2_8"] = m ? "1/1" : "";
            kv["SumF2_9"] = m ? "1/1" : "";
            kv["SumF2_0"] = m ? "vid behov" : "";
        }

        private static void SumMeasuringDevices(Dictionary<string, string> kv, string maskinVal, bool tmpO, string sumRullar, string bygGtj)
        {
            bool m = IsMachine(maskinVal);
            kv["SumD1_1"] = m ? "Skjutmått" : "";
            kv["SumD1_2"] = m ? "Multimar" : "";
            kv["SumD1_3"] = m ? "Gängmall" : "";
            kv["SumD1_4"] = m ? "Skjutmått" : "";
            kv["SumD1_5"] = tmpO ? (m ? "Gängtolk" : "") : "";
            kv["SumD1_6"] = m ? "Skjutmått" : "";
            kv["SumD1_7"] = m ? "Skjutmått" : "";
            kv["SumD1_8"] = m ? "Skjutmått" : "";
            kv["SumD1_9"] = "";
            kv["SumD1_0"] = "";
            kv["SumD2_1"] = m ? "Klove" : "";
            kv["SumD2_2"] = m ? "Skjutmått" : "";
            kv["SumD2_3"] = m ? "Skjutmått" : "";
            kv["SumD2_4"] = tmpO ? (m ? "Skjutmått" : "") : "";
            kv["SumD2_5"] = m ? "Egglinjal" : "";
            kv["SumD2_6"] = m ? "Mätbygel: " + bygGtj : "";
            kv["SumD2_7"] = m ? "Mätbygel: " + bygGtj : "";
            kv["SumD2_8"] = m ? "Mätbygel: " + bygGtj : "";
            kv["SumD2_9"] = m ? "Okulärkontroll" : "";
            kv["SumD2_0"] = m ? "Mätmaskin" : "";
        }

        private static void SumAF(Dictionary<string, string> kv, string maskinVal, bool tmpO, string sumRullar, string konavv, string kr)
        {
            bool m = IsMachine(maskinVal);
            kv["SumAF1_1"] = "";
            kv["SumAF1_2"] = m ? "Rullar: " + sumRullar + ", inställd med klove" : "";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = tmpO ? "" : "";
            kv["SumAF1_6"] = "";
            kv["SumAF1_7"] = "";
            kv["SumAF1_8"] = "";
            kv["SumAF1_9"] = "";
            kv["SumAF1_0"] = "";
            kv["SumAF2_1"] = m ? "TOL efter slits:" : "";
            kv["SumAF2_2"] = "";
            kv["SumAF2_3"] = "";
            kv["SumAF2_4"] = tmpO ? "" : "";
            kv["SumAF2_5"] = "";
            kv["SumAF2_6"] = m ? "GodstjockleksTOL:" : "";
            kv["SumAF2_7"] = m ? "Tolerans: " + konavv : "";
            kv["SumAF2_8"] = m ? "Max variation: " + kr.Replace(".",",") : "";
            kv["SumAF2_9"] = m ? "Vid misstänkt fel Ra-mätare" : "";
            kv["SumAF2_0"] = m ? "Vid misstänkt formfel lämna till mätrum" : "";
        }

        private static string BuildRitNr(string ritningsnr, string tmpSerie, int serieInt, double typNum, bool tmpO, string tmpBet3b, string tmpBet)
        {
            string baseNr;
            if (serieInt == 22) baseNr = "7437361";
            else if (serieInt == 23) baseNr = "7437360";
            else if (serieInt == 30) baseNr = (Math.Abs(typNum - 40) < 0.001 && tmpO) ? "7435633" : "7437362";
            else if (serieInt == 31) baseNr = "7437364";
            else if (serieInt == 32) baseNr = EqualsI(tmpBet3b, "G") ? "7437366" : "231638";
            else if (serieInt == 240) baseNr = "7437370";
            else if (serieInt == 241) baseNr = "7437372";
            else baseNr = tmpBet;

            string oilSuffix = "";
            if (tmpO)
            {
                if (serieInt == 22 || serieInt == 23)
                    oilSuffix = ", 7437987";
                else if (serieInt == 30 && typNum > 41)
                    oilSuffix = ", 7437376";
                else if (serieInt == 31)
                    oilSuffix = ", 7437378";
                else if (serieInt == 32)
                    oilSuffix = ", 7437380";
                else if (serieInt == 240)
                    oilSuffix = ", 7437397";
                else if (serieInt == 241)
                    oilSuffix = ", 7437399";
            }

            string ritNr = baseNr + oilSuffix;
            return EqualsI(ritningsnr, "0") ? ritNr : ritningsnr;
        }

        private static string Sumg1(string bet2, double stmm)
        {
            if (EqualsI(bet2, "336569")) return "4.4x45º";
            if (stmm >= 8) return "5.3x45º";
            if (Math.Abs(stmm - 7) < 0.001) return "4.4x45º";
            if (Math.Abs(stmm - 6) < 0.001) return "3.8x45º";
            if (Math.Abs(stmm - 5) < 0.001) return "3.2x45º";
            if (Math.Abs(stmm - 4) < 0.001) return "2.7x45º";
            if (Math.Abs(stmm - 3) < 0.001) return "2.4x45º";
            if (Math.Abs(stmm - 2) < 0.001) return "1.7x45º";
            return "1.3x45º";
        }

        private static string Sumg2(int ritSum, bool rit3, bool rit22, bool rit23, bool rit30, bool rit31, bool rit32, double d2, string g1)
        {
            if (ritSum == 0) return g1;
            if (rit3) return Math.Abs(d2 - 80) < 0.001 ? "1.5x45º" : g1;
            if (rit22) return d2 < 210 ? "2.5x45º" : g1;
            if (rit23)
            {
                if (d2 < 69) return g1;
                if (d2 < 76) return "1.75x45º";
                if (Math.Abs(d2 - 80) < 0.001 || Math.Abs(d2 - 140) < 0.001 || Math.Abs(d2 - 150) < 0.001) return "1.5x45º";
                if (d2 < 155) return "1.7x45º";
                if (d2 < 210) return "2.25x45º";
                return g1;
            }
            if (rit30)
            {
                if (d2 < 195) return g1;
                if (Math.Abs(d2 - 200) < 0.001) return "2.5x45º";
                if (d2 < 250) return "2.75x45º";
                if (d2 < 310) return "2.7x45º";
                return g1;
            }
            if (rit31)
            {
                if (d2 < 160) return "1.7x45º";
                if (d2 < 210) return "2.25x45º";
                if (d2 < 310) return "2.7x45º";
                if (Math.Abs(d2 - 320) < 0.001) return g1;
                if (d2 < 490) return "3.25x45º";
                return g1;
            }
            if (rit32)
            {
                if (Math.Abs(d2 - 140) < 0.001 || Math.Abs(d2 - 150) < 0.001) return "1.5x45º";
                if (d2 < 135) return "1.7x45º";
                if (d2 < 210) return "2.25x45º";
                if (Math.Abs(d2 - 220) < 0.001) return "2.7x45º";
                if (d2 < 501) return "3.25x45º";
                if (d2 < 580) return "4x45º";
                if (Math.Abs(d2 - 600) < 0.001) return "3.8x45º";
                if (d2 < 700) return "4x45º";
                if (d2 < 790) return "4.5x45º";
                if (d2 < 890) return "4.4x45º";
                if (Math.Abs(d2 - 950) < 0.001) return "5.5x45º";
                if (Math.Abs(d2 - 1000) < 0.001) return "5x45º";
                return "5.3x45º";
            }
            return "Fel Typ";
        }

        private static string GH(int serie, double d1)
        {
            if (serie == 30)
            {
                if (d1 < 401) return "2";
                if (d1 < 481) return "3";
                if (d1 < 501) return "4";
                return "5";
            }
            if (serie == 31) { if (d1 < 301) return "3"; if (d1 < 631) return "4"; return "5"; }
            if (serie == 32)
            {
                if (d1 < 401) return "2";
                if (d1 < 501) return "3";
                if (d1 < 631) return "4";
                return "5";
            }
            if (serie == 39) { return d1 < 671 ? "4" : "5"; }
            if (serie == 240)
            {
                if (d1 < 301) return "3";
                if (d1 < 481) return "4";
                if (d1 < 601) return "3";
                if (d1 < 751) return "4";
                return "5";
            }
            if (serie == 241 || serie == 336) { if (d1 < 381) return "3"; if (d1 < 631) return "4"; return "5"; }
            if (serie == 22) { if (d1 < 301) return "2"; if (d1 < 631) return "4"; return "5"; }
            return serie.ToString(CultureInfo.InvariantCulture);
        }

        private static bool IsMachine(string mv)
        {
            if (string.IsNullOrEmpty(mv)) return false;
            for (int i = 0; i < Machines.Length; i++)
                if (string.Equals(Machines[i], mv, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        private static double Stmm(double d2)
        {
            if (d2 < 25) return 1.0;
            if (d2 < 55) return 1.5;
            if (d2 < 155) return 2.0;
            if (d2 < 205) return 3.0;
            if (d2 < 301) return 4.0;
            if (d2 < 501) return 5.0;
            if (d2 < 701) return 6.0;
            if (d2 < 901) return 7.0;
            return 8.0;
        }

        private static double D2TolN(double s)
        {
            if (Math.Abs(s - 4) < 0.001) return 0.300;
            if (Math.Abs(s - 5) < 0.001) return 0.335;
            if (Math.Abs(s - 6) < 0.001) return 0.375;
            if (Math.Abs(s - 7) < 0.001) return 0.425;
            return 0.450;
        }

        private static double DM(double d2, double s)
        {
            if (Math.Abs(s - 4) < 0.001) return d2 - 2.0;
            if (Math.Abs(s - 5) < 0.001) return d2 - 2.5;
            if (Math.Abs(s - 6) < 0.001) return d2 - 3.0;
            if (Math.Abs(s - 7) < 0.001) return d2 - 3.5;
            return d2 - 4.0;
        }

        private static double DmTol(double s)
        {
            if (Math.Abs(s - 4) < 0.001) return 0.190;
            if (Math.Abs(s - 5) < 0.001) return 0.212;
            if (Math.Abs(s - 6) < 0.001) return 0.236;
            if (Math.Abs(s - 7) < 0.001) return 0.250;
            return 0.265;
        }

        private static double DmTolN(double s)
        {
            if (Math.Abs(s - 4) < 0.001) return 0.630;
            if (Math.Abs(s - 5) < 0.001) return 0.710;
            if (Math.Abs(s - 6) < 0.001) return 0.800;
            if (Math.Abs(s - 7) < 0.001) return 0.850;
            return 0.950;
        }

        private static double D3Val(double dmv, double s)
        {
            if (Math.Abs(s - 4) < 0.001) return dmv - 2.5;
            if (Math.Abs(s - 5) < 0.001) return dmv - 3.0;
            if (Math.Abs(s - 6) < 0.001) return dmv - 4.0;
            if (Math.Abs(s - 7) < 0.001) return dmv - 4.5;
            return dmv < 1300 ? dmv - 5.0 : dmv - 4.5;
        }

        private static double D4TolPos(int serie, double d4)
        {
            if (serie != 39) return 0.0;
            if (d4 < 6.01) return 0.1;
            if (d4 < 30.01) return 0.2;
            if (d4 < 120.01) return 0.3;
            if (d4 < 315.01) return 0.5;
            if (d4 < 1000.01) return 0.8;
            if (d4 < 2000.01) return 1.2;
            return 2.0;
        }

        private static double D4TolNeg(int serie, double d4)
        {
            if (serie == 39) return D4TolPos(serie, d4);
            if (serie == 22) return 0.2;
            if (serie == 240 || serie == 241) return 0.25;
            return 0.3;
        }

        private static double B2Tol(double b)
        {
            if (b < 3.01) return 0.200;
            if (b < 6.01) return 0.240;
            if (b < 10.01) return 0.290;
            if (b < 18.01) return 0.350;
            if (b < 30.01) return 0.420;
            if (b < 50.01) return 0.500;
            if (b < 80.01) return 0.600;
            if (b < 120.01) return 0.700;
            if (b < 180.01) return 0.800;
            if (b < 250.01) return 0.925;
            if (b < 315.01) return 1.050;
            if (b < 400.01) return 1.150;
            if (b < 500.01) return 1.250;
            return 1.400;
        }

        private static double LTolN(double L)
        {
            if (L < 3.01) return 0.140;
            if (L < 6.01) return 0.180;
            if (L < 10.01) return 0.220;
            if (L < 18.01) return 0.270;
            if (L < 30.01) return 0.330;
            if (L < 50.01) return 0.390;
            if (L < 80.01) return 0.460;
            if (L < 120.01) return 0.540;
            if (L < 180.01) return 0.630;
            if (L < 250.01) return 0.720;
            if (L < 315.01) return 0.810;
            if (L < 400.01) return 0.890;
            if (L < 500.01) return 0.970;
            if (L < 630.01) return 1.100;
            if (L < 800.01) return 1.250;
            if (L < 1000.01) return 1.400;
            if (L < 1250.01) return 1.650;
            if (L < 1600.01) return 1.950;
            if (L < 2000.01) return 2.300;
            if (L < 2500.01) return 2.800;
            return 3.300;
        }

        private static double D1Tol(double d1, bool is335376)
        {
            if (is335376) return 0.0;
            if (d1 < 31) return 0.026;
            if (d1 < 51) return 0.031;
            if (d1 < 81) return 0.037;
            if (d1 < 121) return 0.043;
            if (d1 < 181) return 0.050;
            if (d1 < 251) return 0.057;
            if (d1 < 316) return 0.065;
            if (d1 < 401) return 0.070;
            if (d1 < 501) return 0.077;
            if (d1 < 631) return 0.087;
            if (d1 < 801) return 0.100;
            if (d1 < 1001) return 0.115;
            return 0.130;
        }

        private static double D1TolN(double d1, bool is335376)
        {
            if (is335376) return 0.050;
            return D1Tol(d1, false);
        }

        private static double D1TolES(double typ)
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

        private static double D1TolNES(double typ)
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

        private static double ExTol(double kona, double d)
        {
            if (Math.Abs(kona - 12) < 0.001)
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
            if (Math.Abs(kona - 30) < 0.001)
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
            return 0.0;
        }

        private static double VTFactor(double d)
        {
            if (d > 1000) return 0.09;
            if (d > 800) return 0.10;
            if (d > 630) return 0.11;
            if (d > 500) return 0.12;
            if (d > 400) return 0.13;
            if (d > 181) return 0.15;
            if (d > 151) return 0.18;
            if (d > 121) return 0.30;
            if (d > 81) return 0.45;
            if (d > 51) return 0.50;
            return 0.60;
        }

        private static int EE(double typ)
        {
            if (typ < 64) return 5;
            if (typ < 84) return 6;
            if (typ < 530) return 7;
            if (typ < 670) return 8;
            if (typ < 850) return 10;
            return 12;
        }

        private static string FDepth(int ee, int serie)
        {
            if (ee == 5) return "1";
            if (ee == 6) return "1.2";
            if (ee == 7 || ee == 8) return "1.5";
            if (ee == 10) return "2";
            if (serie == 240) return "2.7";
            return "2.5";
        }

        private static string RepM(double typ) { return typ < 64 ? "1" : "1.5"; }
        private static string RepNn(double typ) { return typ < 64 ? "0.3" : "0.5"; }

        private static string R1(int ee)
        {
            if (ee == 5) return "R4";
            if (ee == 6) return "R4.5";
            if (ee == 7) return "R5";
            if (ee == 8) return "R6";
            if (ee == 10) return "R7";
            return "R8";
        }

        private static int RakA(double dl)
        {
            if (dl < 101) return 8;
            if (dl < 281) return 10;
            if (dl < 481) return 12;
            if (dl < 601) return 14;
            if (dl < 901) return 16;
            return 20;
        }

        private static int RakB(double dl)
        {
            if (dl < 101) return 12;
            if (dl < 281) return 15;
            if (dl < 481) return 18;
            if (dl < 601) return 21;
            if (dl < 901) return 24;
            return 30;
        }

        private static int Ks(double dl)
        {
            if (dl > 1001) return 160;
            if (dl > 801) return 140;
            if (dl > 631) return 120;
            if (dl > 501) return 100;
            if (dl > 401) return 90;
            if (dl > 316) return 80;
            if (dl > 251) return 70;
            if (dl > 121) return 60;
            if (dl > 51) return 50;
            return 40;
        }

        private static double Kr(int serie, double dl)
        {
            if (serie == 39) return 0.082;
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

        private static string BTol(double B)
        {
            if (B < 30.01) return "± 0.2";
            if (B < 120.01) return "± 0.3";
            if (B < 400.01) return "± 0.5";
            return "± 0.8";
        }

        private static string Fmt(double v) => v.ToString("0.################", CultureInfo.InvariantCulture).Replace(".", ",");
        private static string FmtDot(double v) => v.ToString("0.################", CultureInfo.InvariantCulture);
        private static string FmtDotRemoveLeadingZero(double v) => v.ToString("0.################", CultureInfo.InvariantCulture).Replace("0.",".");

        private static string Fmt1(double v) => FmtFixedComma(v, 1);
        private static string FmtFixedComma(double v, int decimals) => v.ToString("F" + decimals.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture).Replace(".", ",");
        private static string Fmt3(double v) => v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
        private static string FmtDiv3(int v) => (v / 1000.0).ToString("F3", CommonFunctions.Culture).Replace(",", ".");
        private static int B2I(bool b) => b ? 1 : 0;

        private static int TryParseInt(string s)
        {
            int v;
            return int.TryParse(s, NumberStyles.Any, CommonFunctions.Culture, out v) ? v : 0;
        }

        private static double TryParseDouble(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;
            double v;
            return double.TryParse(s.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static double GetDouble(List<Bookmark> bm, string key)
        {
            string raw = GetString(bm, key);
            if (string.IsNullOrWhiteSpace(raw)) return 0;
            double v;
            return double.TryParse(raw.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        //private static string GetString(List<Bookmark> bm, string key)
        //{
        //    if (bm == null || string.IsNullOrWhiteSpace(key)) return "";
        //    for (int i = 0; i < bm.Count; i++)
        //    {
        //        Bookmark b = bm[i];
        //        if (b != null && string.Equals(b.BookmarkName ?? "", key, StringComparison.OrdinalIgnoreCase))
        //            return b.BookmarkValue ?? "";
        //    }
        //    return "";
        //}
        private static string GetString(List<Bookmark> bm, string key)
        {
            if (bm == null || string.IsNullOrWhiteSpace(key))
                return "";

            string value = "";

            for (int i = 0; i < bm.Count; i++)
            {
                Bookmark b = bm[i];

                if (b != null &&
                    string.Equals(
                        (b.BookmarkName ?? "").Trim(),
                        key.Trim(),
                        StringComparison.OrdinalIgnoreCase))
                {
                    value = b.BookmarkValue ?? "";
                }
            }

            return value.Trim();
        }
        private static bool EqualsI(string a, string b)
            => string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);

        private static bool ContainsI(string src, string val)
        {
            if (src == null || val == null) return false;
            return src.IndexOf(val, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}