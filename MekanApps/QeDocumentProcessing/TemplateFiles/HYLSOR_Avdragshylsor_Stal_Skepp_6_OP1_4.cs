using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Avdragshylsor_Stal_Skepp_6_OP1_4 : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;
            List<Bookmark> bm = req != null ? req.Bookmarks : null;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");
            bool tmpSlash = tmpBet.IndexOf('/') >= 0;
            bool tmpLW = tmpBet.IndexOf("LW", StringComparison.OrdinalIgnoreCase) >= 0;
            bool tmpMS = tmpBet.IndexOf("MS", StringComparison.OrdinalIgnoreCase) >= 0;
            bool tmpV21 = tmpBet.IndexOf("V21", StringComparison.OrdinalIgnoreCase) >= 0;
            bool tmpAOHX = tmpBet.IndexOf("AOHX", StringComparison.OrdinalIgnoreCase) >= 0;

            // Explode " /.-"
            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : "";
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : "";
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : "";

            int cntB2 = tmpBet2.Length;

            // TmpSerie
            string tmpSerie;
            if (tmpLW) tmpSerie = "0";
            else if (tmpSlash && (cntB2 == 3 || cntB2 == 2)) tmpSerie = tmpBet2;
            else if (cntB2 > 4) tmpSerie = tmpBet2.Substring(0, 3);
            else if (cntB2 == 3) tmpSerie = tmpBet2.Substring(0, 1);
            else tmpSerie = tmpBet2.Length >= 2 ? tmpBet2.Substring(0, 2) : tmpBet2;
            int serieInt = TryParseInt(tmpSerie);

            // TmpTyp: if slash -> Bet3, else Right(2)
            string tmpTyp = tmpSlash ? tmpBet3 : (tmpBet2.Length >= 2 ? tmpBet2.Substring(tmpBet2.Length - 2) : tmpBet2);
            double typNum = TryParseDouble(tmpTyp);

            // AOHX popup validation
            string tmpAOHXLista_val = serieInt == 30 ? "560" : "";
            bool aohxListaActive = !string.IsNullOrEmpty(tmpAOHXLista_val);
            string tmpXorNOT = tmpAOHX
                ? "OBS! Det finns även version AOH som skiljer sig ifrån den du valt"
                : "OBS! Det finns även version AOHX som skiljer sig ifrån den du valt";
            kv["VaLAOHX"] = aohxListaActive ? tmpXorNOT : "";

            // ── Bookmarks ─────────────────────────────────────────────────────────
            double tmpd = GetDouble(bm, "Kona lillände diameter (d)");
            double tmpd1 = GetDouble(bm, "Innerdiameter (d1)");
            double tmpd2 = GetDouble(bm, "Ytterdiameter (d2)");
            double tmpd4 = GetDouble(bm, "Släppning (d4)");
            double tmph = GetDouble(bm, "Bredd släppning (h)");
            double tmpb = GetDouble(bm, "Längd till gänga (b)");
            double tmpL = GetDouble(bm, "Längd (L)");
            double amatt = GetDouble(bm, "a-mått");
            double konaBm = GetDouble(bm, "Kona");
            int tmpAES = (int)GetDouble(bm, "Äldre standard");
            double d5 = GetDouble(bm, "Kona Storände diameter (d5)");
            double passbit = GetDouble(bm, "Passbit");

            // Skärdata bookmarks OP1
            string cb1Str = GetString(bm, "Chuckbackar_OP1"); double cb1 = TryParseDouble(cb1Str);
            string sb1Str = GetString(bm, "Stödbackar_OP1"); double sb1 = TryParseDouble(sb1Str);
            string gr1Str = GetString(bm, "Grader_OP1"); double gr1 = TryParseDouble(gr1Str);
            string vr1Str = GetString(bm, "Varvtal_OP1"); double vr1 = TryParseDouble(vr1Str);
            string mp1Str = GetString(bm, "Matning Plan_OP1"); double mp1 = TryParseDouble(mp1Str);
            string mi1Str = GetString(bm, "Matning Utv/Inv_OP1"); double mi1 = TryParseDouble(mi1Str);
            string fmu1Str = GetString(bm, "Färdigmått Utv_OP1");
            string lu1Str = GetString(bm, "Linjal Utv_OP1"); double lu1 = TryParseDouble(lu1Str);
            string fmi1Str = GetString(bm, "Färdigmått Inv_OP1");
            string li1Str = GetString(bm, "Linjal Inv_OP1"); double li1 = TryParseDouble(li1Str);

            // Skärdata bookmarks OP2
            string cbStr = GetString(bm, "Chuckbackar"); double cbD = TryParseDouble(cbStr);
            string sbStr = GetString(bm, "Stödbackar"); double sbD = TryParseDouble(sbStr);
            string grStr = GetString(bm, "Grader"); double grD = TryParseDouble(grStr);
            string vrStr = GetString(bm, "Varvtal"); double vrD = TryParseDouble(vrStr);
            string mpStr = GetString(bm, "Matning Plan"); double mpD = TryParseDouble(mpStr);
            string miStr = GetString(bm, "Matning Utv/Inv"); double miD = TryParseDouble(miStr);
            string fmuStr = GetString(bm, "Färdigmått Utv");
            string luStr = GetString(bm, "Linjal Utv"); double luD = TryParseDouble(luStr);
            string fmiStr = GetString(bm, "Färdigmått Inv");
            string liStr = GetString(bm, "Linjal Inv"); double liD = TryParseDouble(liStr);

            // TmpKona
            double tmpKona = konaBm == 0
                ? ((serieInt == 240 || serieInt == 241) ? 30 : EqualsI(tmpSerie, "LW") ? 0 : 12)
                : konaBm;
            kv["SumV"] = tmpKona == 30 ? "0\u00ba57" : "2\u00ba23";
            kv["SumKona"] = "Kona 1:" + FmtComma(tmpKona);
            kv["SumKonaOP3"] = kv["SumKona"];
            kv["SumVmått"] = tmpKona == 30 ? "50.6" : "55.6";

            // TmpStmm — based on Tmpd2 (NOT Tmpd)!
            int tmpStmm = tmpd2 < 301 ? 4 : tmpd2 < 501 ? 5 : tmpd2 < 701 ? 6 : tmpd2 < 901 ? 7 : 8;

            // Gänga — direct [Tmpd2] -> FmtComma
            kv["SumGänga"] = "Tr" + FmtComma(tmpd2) + "x" + tmpStmm;
            kv["SumP"] = "Tr" + tmpStmm;
            kv["SumP1"] = tmpStmm.ToString(CultureInfo.InvariantCulture);
            kv["SumRullar"] = tmpStmm + "mm";

            // ── d1 OP1 — direct -> FmtComma ──────────────────────────────────────
            double tmpd1OP1 = tmpd1 - 3; // TmpKon_d1 = 3
            kv["Sumd1OP1"] = "(d1) " + FmtComma(tmpd1OP1);
            kv["Sumd1OP1Tol"] = "+ 0.500"; // @ReplaceSubstring(TmpKon05) -> FmtDot
            kv["Sumd1OP1TolN"] = "- 0";    // @ReplaceSubstring(TmpKon0)

            // ── d1 OP3 — direct -> FmtComma ──────────────────────────────────────
            kv["Sumd1OP3"] = "(d1) " + FmtComma(tmpd1);
            double tmpd1OP3TolD = D1Tol13(tmpd1);
            // Sumd1OP3Tol: special case for Bet2="335376"
            kv["Sumd1OP3Tol"] = EqualsI(tmpBet2, "335376") ? "+ 0" : "+ " + Fmt3(tmpd1OP3TolD) + " [3F]";
            kv["Sumd1OP3TolN"] = EqualsI(tmpBet2, "335376") ? "- 0,05" : "- " + Fmt3(tmpd1OP3TolD) + " [3F]";

            // ── dm — direct -> FmtComma ───────────────────────────────────────────
            double tmpdm = tmpStmm == 4 ? tmpd2 - 2 : tmpStmm == 5 ? tmpd2 - 2.5 : tmpStmm == 6 ? tmpd2 - 3
                         : tmpStmm == 7 ? tmpd2 - 3.5 : tmpd2 - 4;
            kv["Sumdm"] = "(dm) " + FmtComma(tmpdm);
            // @ReplaceSubstring -> FmtDot
            kv["SumdmTol"] = "- " + Fmt3(DmTolPos(tmpStmm)) + " [3F]";
            kv["SumdmTolN"] = "- " + Fmt3(DmTolNeg(tmpStmm)) + " [3F]";

            // ── d3 — direct -> FmtComma ───────────────────────────────────────────
            double tmpd3 = tmpStmm == 4 ? tmpdm - 2.5 : tmpStmm == 5 ? tmpdm - 3 : tmpStmm == 6 ? tmpdm - 4
                         : tmpStmm == 7 ? tmpdm - 4.5 : (tmpdm < 1300 ? tmpdm - 5 : tmpdm - 4.5);
            kv["Sumd3"] = "(d3) " + FmtComma(tmpd3);
            kv["Sumd3Tol"] = "+ 0"; // @ReplaceSubstring(TmpKon0) -> "0"
            // @ReplaceSubstring -> FmtDot
            kv["Sumd3TolN"] = "- " + Fmt3(D3TolN(tmpStmm));

            // ── LF (fixed) ────────────────────────────────────────────────────────
            kv["SumLF"] = "(LF) 80";
            kv["SumLFTol"] = "";

            // ── d OP2 — @Round -> FmtComma ────────────────────────────────────────
            double tmpdOP2 = Math.Round(tmpd + (amatt / tmpKona) + 3, 1);
            kv["SumdOP2"] = "(d) " + FmtComma(tmpdOP2);
            if (subject == "AOH 39/1060")
                kv["SumdOP2"] = "(d) 1064,3";
            if (subject == "MS-336569/V21")
                kv["SumdOP2"] = "(d) 1064,3";
            kv["SumdOP2Tol"] = "+ 0";
            kv["SumdOP2TolN"] = "- 0.500"; // @ReplaceSubstring(TmpKon05)

            // ── L OP2 — direct -> FmtComma ───────────────────────────────────────
            double tmpLOP2 = tmpL + 2; // TmpKon_LOP2 = 2
            kv["SumLOP2"] = "(L) " + FmtComma(tmpLOP2);
            kv["SumLOP2Tol"] = "+ 0";
            kv["SumLOP2TolN"] = "- 0.500";

            // ── L OP3 — direct -> FmtComma ───────────────────────────────────────
            kv["SumLOP3"] = "(L) " + FmtComma(tmpL);
            kv["SumLOP3Tol"] = "";

            // ── L OP4 ─────────────────────────────────────────────────────────────
            kv["SumL"] = "(L) " + FmtComma(tmpL);
            kv["SumLTol"] = "+ 0";
            // @ReplaceSubstring -> FmtDot; special for 335376
            kv["SumLTolN"] = EqualsI(tmpBet2, "335376") ? "- 0,72"
                : "- " + Fmt3(LTolN14(tmpL)) + " [3F]";

            // ── Gänglängd helpers ─────────────────────────────────────────────────
            double tmpTolb = TolB(tmpb);
            double tmpTolL = LTolN14(tmpL);
            double tmpg = tmpL - tmpb;
            double sumgmax = tmpg + tmpTolb;
            double sumgmin = tmpg - tmpTolb - tmpTolL;
            // @Implode(@Explode(TmpbTol;",");".") = replace comma with dot -> FmtDot
            kv["SumbTol"] = "\u00b1 " + Fmt3(tmpTolb) + " [3F]";
            kv["Sumgmax"] = FmtComma(sumgmax); // direct
            kv["Sumgmin"] = FmtComma(sumgmin); // direct

            // ── X, Y helpers — direct -> FmtComma ────────────────────────────────
            double tmpX = tmpL - tmpb;
            double tmpY = tmpX + 3;
            kv["SumX"] = FmtComma(tmpX);
            kv["SumY"] = FmtComma(tmpY);

            // ── b OP2/OP3 — direct -> FmtComma ───────────────────────────────────
            double tmpbOP2 = tmpLOP2 - tmpY;
            kv["SumbOP2"] = "(b) " + FmtComma(tmpbOP2);
            kv["SumbOP2Tol"] = "\u00b1 0.500";
            kv["SumbOP3"] = "(b) " + FmtComma(tmpb);
            kv["SumbOP3Tol"] = "";
            kv["Sumb"] = "(b) " + FmtComma(tmpb);

            // ── d6 OP2 — @Round -> FmtComma ──────────────────────────────────────
            double tmpd6OP2 = Math.Round((tmpbOP2 / tmpKona) + tmpdOP2, 1);
            kv["Sumd6OP2"] = "(d6) " + FmtComma(tmpd6OP2);
            if (subject == "AOH 31/560" || subject == "AOH 31/560/520")
                kv["Sumd6OP2"] = "(d6) 587,3";
            if(subject == "AOH 39/1060")
                kv["Sumd6OP2"] = "(d6) 1085,7";
            if (subject == "MS-336569/V21")
                kv["Sumd6OP2"] = "(d6) 1085,9";
            kv["Sumd6OP2Tol"] = "+ 0.500";
            kv["Sumd6OP2TolN"] = "- 0";

            // ── d2 OP1/OP2 — direct -> FmtComma ──────────────────────────────────
            double tmpd2OP1 = (d5 == 0 ? tmpd2 : d5) + 2.5; // TmpKon_d2 = 2.5
            kv["SumA"] = "(A) Rensvarvas";
            kv["Sumd2OP1"] = "(d2) " + FmtComma(tmpd2OP1);
            kv["Sumd2OP1Tol"] = "+ 0";
            kv["Sumd2OP1TolN"] = "- 0.500";
            kv["Sumd2OP2"] = kv["Sumd2OP1"];
            kv["Sumd2OP2Tol"] = kv["Sumd2OP1Tol"];
            kv["Sumd2OP2TolN"] = kv["Sumd2OP1TolN"];

            // ── d2 OP4 — direct -> FmtComma ───────────────────────────────────────
            kv["Sumd2"] = "(d2) " + FmtComma(tmpd2);
            kv["Sumd2a"] = kv["Sumd2"];
            kv["Sumd2Tol"] = "+ 0";
            kv["Sumd2TolN"] = "- " + Fmt3(D2TolN(tmpStmm)); // @ReplaceSubstring -> FmtDot
            kv["Sumd2aTol"] = kv["Sumd2Tol"];
            kv["Sumd2aTolN"] = kv["Sumd2TolN"];

            // ── FJ (fästjärnsvarvning) — @Round -> FmtComma ───────────────────────
            double tmpFJ = Math.Round((tmpd2OP1 - tmpd6OP2) / 2.0, 1);
            kv["SumFJ"] = tmpFJ < 4 ? "Mått under 4mm" : "Max tillgängligt mått: " + FmtComma(tmpFJ);
            if (subject == "AOH 39/1060")
                kv["SumFJ"] = "Max tillgängligt mått: 18,4";
            if (subject == "MS-336569/V21")
                kv["SumFJ"] = "Max tillgängligt mått: 5,8";
            // ── GTj (godstjocklek) — @ReplaceSubstring -> FmtDot ─────────────────
            bool gTjSpecial = EqualsI(tmpBet, "MS-336569/V21");
            double tmpGTjTolD = gTjSpecial ? 0.115 : GTjTolPos(tmpd, tmpKona);
            kv["SumGTjTol"] = "+ " + Fmt3(tmpGTjTolD) + " [3F]";
            kv["SumGTjTolN"] = "- 0 [2F]";
            kv["SumGTjTol2"] = kv["SumGTjTol"];
            kv["SumGTjTol2N"] = kv["SumGTjTolN"];
            kv["SumGVarTol"] = "+ " + Fmt3(GVarTol(tmpd)) + " [2F]";

            // ── d4 OP3/OP4 — direct -> FmtComma ──────────────────────────────────
            double tmpd4OP3 = tmpd4 + 1; // TmpKon_d4OP3 = 1
            kv["Sumd4OP3"] = "(d4) " + FmtComma(tmpd4OP3);
            kv["Sumd4OP3Tol"] = "";
            kv["Sumd4"] = "(d4) " + FmtComma(tmpd4);
            kv["Sumd4Tol"] = "+ 0";
            // @ReplaceSubstring -> FmtDot
            kv["Sumd4TolN"] = "- " + Fmt3(D4TolN(serieInt));

            // ── h — direct -> FmtComma ────────────────────────────────────────────
            kv["SumhOP3"] = "(h) " + FmtComma(tmph);
            kv["Sumh"] = "(h) " + FmtComma(tmph);
            kv["SumhTol"] = "\u00b1 0.200"; // @ReplaceSubstring(TmpKon02) -> FmtDot

            // ── R, r1, e ──────────────────────────────────────────────────────────
            int tmpR = EqualsI(tmpBet, "MS-336569/V21") ? 10 : (tmpd < 125 ? 50 : 60);
            kv["SumR"] = "R=" + tmpR;
            kv["Sumr1"] = "R 1.5";
            kv["Sumra"] = "R 1.5";
            kv["Sumr1a"] = Sumr1a(tmpSerie, tmpTyp, serieInt, typNum, tmpd2);
            int tmpe = tmpR == 50 ? 5 : 6;
            kv["Sume"] = "(e) " + tmpe;

            // ── Orundhet & vinkeltolerans ─────────────────────────────────────────
            kv["SumOrund"] = Orund(tmpd1) + " [3F]";
            kv["SumVinkTol"] = VinkTol(tmpd) + " [2F]";

            // ── Rakhet — @text -> FmtDot ──────────────────────────────────────────
            kv["SumRakA"] = "Max: " + FmtComma(RakA(tmpd)) + " [2F]";
            kv["SumRakB"] = "Max: " + FmtComma(RakB(tmpd)) + " [2F]";

            // ── Sidkast ───────────────────────────────────────────────────────────
            kv["SumSidkast"] = Sidkast(tmpd) + " [2F]";

            // ── Faser g1, g2 ──────────────────────────────────────────────────────
            string sumg1 = Sumg1(tmpStmm);
            kv["Sumg1"] = sumg1;
            kv["Sumg2"] = Sumg2(sumg1, tmpSerie, serieInt, tmpAES, tmpd2);

            // ── Mätbygelinställning ───────────────────────────────────────────────
            double sumML = tmpb - tmph;
            double tmp8 = Math.Round(((tmpd - tmpd1) / 2.0) + ((amatt + 8) / (2.0 * tmpKona)), 3);
            double tmp108 = Math.Round(((tmpd - tmpd1) / 2.0) + ((amatt + 108) / (2.0 * tmpKona)), 3);
            double tmp40 = Math.Round(((tmpd - tmpd1) / 2.0) + ((amatt + 40) / (2.0 * tmpKona)), 3);
            double tmp140 = Math.Round(((tmpd - tmpd1) / 2.0) + ((amatt + 140) / (2.0 * tmpKona)), 3);
            kv["SumML"] = FmtComma(sumML);
            kv["SumL1"] = FmtComma(sumML < 145 ? 8 : 40);
            kv["SumL2"] = FmtComma(sumML < 145 ? 108 : 140);
            kv["SumE1"] = FmtComma(sumML < 145 ? tmp8 : tmp40);
            kv["SumE2"] = FmtComma(sumML < 145 ? tmp108 : tmp140);
            string tmpBygel = sumML < 145 ? "SR 7415983" : "SR 7419470 el. 7415991";
            kv["SumBygGtj"] = tmpBygel;
            kv["SumBygGVar"] = tmpBygel;
            kv["SumBygVinkTol"] = tmpBygel;

            // ── Ra ────────────────────────────────────────────────────────────────
            kv["SumRa25"] = "2.5 [2F]";
            kv["SumRa25a"] = "2.5 [2F]";
            kv["SumRa5"] = "5 [2F]";
            kv["SumRa5a"] = "5 [2F]";

            // ── Ritningar ─────────────────────────────────────────────────────────
            string sumRitS1 = "Produktritning: " + SumRitningsnr(serieInt, tmpAES, tmpd, tmpBet);
            kv["SumRitningsnrS1"] = sumRitS1;
            kv["SumRitningsnrS2"] = sumRitS1;
            kv["SumRitningsnrS3"] = sumRitS1;
            kv["SumRitningsnrS4"] = sumRitS1;
            kv["SumRitTol"] = "Toleranser: 1432011";
            kv["SumRitGänga"] = "Gänga: 237359, 7430181";
            string klEg = "PRODUCTION NUTS & SLEEVES & HOUSINGSALLM\u00c4NKLASSADE EGENSKAPERKlassade egenskaper Avdragshylsor";
            kv["SumKlEgenskaper"] = klEg;
            kv["SumKlEgenskaperS2"] = klEg;
            kv["SumKlEgenskaperS3"] = klEg;
            kv["SumKlEgenskaperS4"] = klEg;

            // ── Machine — "Skepp 6" (with space!) ────────────────────────────────
            bool mOK = EqualsI(maskinVal, "Skepp 6");
            string mvErr = EqualsI(maskinVal, "") ? "" : mOK ? "" : "INGET MASKINVAL GJORD";
            kv["SumMaskinValS1"] = "Maskin: " + (mOK ? "Avdragshylsa Morando" : "") + " OP1" + mvErr;
            kv["SumMaskinValS2"] = "Maskin: " + (mOK ? "Avdragshylsa Morando" : "") + " OP2" + mvErr;
            kv["SumMaskinValS3"] = "Maskin: " + (mOK ? "Avdragshylsa 1150" : "") + " OP3" + mvErr;
            kv["SumMaskinValS4"] = "Maskin: " + (mOK ? "Avdragshylsa 1150" : "") + " OP4" + mvErr;

            // ── Frequencies ───────────────────────────────────────────────────────
            // Page 1 (all 1/1)
            for (int i = 1; i <= 4; i++) kv["SumF1_" + i] = mOK ? "1/1" : "";
            // Page 2 (all 1/1)
            for (int i = 1; i <= 5; i++) kv["SumF2_" + i] = mOK ? "1/1" : "";
            // Page 3
            kv["SumF3_1"] = ""; kv["SumF3_2"] = ""; kv["SumF3_3"] = ""; kv["SumF3_4"] = ""; kv["SumF3_5"] = "";
            kv["SumF3_6"] = ""; kv["SumF3_7"] = ""; kv["SumF3_8"] = ""; kv["SumF3_9"] = "";
            kv["SumF3_0"] = ""; kv["SumF3_11"] = "";
            if (mOK)
            {
                kv["SumF3_1"] = "1/1"; kv["SumF3_2"] = "1/1"; kv["SumF3_3"] = "1/1"; kv["SumF3_4"] = "1/2";
                kv["SumF3_5"] = "1/5"; kv["SumF3_6"] = "1/1"; kv["SumF3_7"] = "1/1"; kv["SumF3_8"] = "1/1";
                kv["SumF3_9"] = "Inst."; kv["SumF3_0"] = "1/5"; kv["SumF3_11"] = "1/5";
            }
            // Page 4
            kv["SumF4_1"] = ""; kv["SumF4_2"] = ""; kv["SumF4_3"] = ""; kv["SumF4_4"] = ""; kv["SumF4_5"] = "";
            kv["SumF4_6"] = ""; kv["SumF4_7"] = ""; kv["SumF4_8"] = ""; kv["SumF4_9"] = ""; kv["SumF4_0"] = "";
            if (mOK)
            {
                kv["SumF4_1"] = "1/2"; kv["SumF4_2"] = "1/2"; kv["SumF4_3"] = "1/5"; kv["SumF4_4"] = "1/1";
                kv["SumF4_5"] = "1/2"; kv["SumF4_6"] = "1/2"; kv["SumF4_7"] = "1/2"; kv["SumF4_8"] = "1/5";
                kv["SumF4_9"] = "1/1"; kv["SumF4_0"] = "Inst.";
            }

            // ── Devices ───────────────────────────────────────────────────────────
            kv["SumD1_1"] = mOK ? "Skjutmått" : ""; kv["SumD1_2"] = mOK ? "Skjutmått" : "";
            kv["SumD1_3"] = ""; kv["SumD1_4"] = mOK ? "Skjutmått" : "";
            kv["SumD2_1"] = mOK ? "Djupmått" : ""; kv["SumD2_2"] = mOK ? "Djupmått" : "";
            kv["SumD2_3"] = mOK ? "Skjutmått" : ""; kv["SumD2_4"] = mOK ? "Skjutmått/mikrometer" : ""; kv["SumD2_5"] = "";
            kv["SumD3_1"] = mOK ? "Mikrometerstickmått" : ""; kv["SumD3_2"] = mOK ? "Skjutmått / Djupmått" : "";
            kv["SumD3_3"] = mOK ? "Skjutmått / Djupmått" : ""; kv["SumD3_4"] = mOK ? "Skjutmått" : "";
            kv["SumD3_5"] = mOK ? "Skjutmått / Djupmått" : "";
            kv["SumD3_6"] = mOK ? "Mätbygel " + tmpBygel : ""; kv["SumD3_7"] = mOK ? "Mätbygel " + tmpBygel : "";
            kv["SumD3_8"] = mOK ? "Mätbygel " + tmpBygel : ""; kv["SumD3_9"] = mOK ? "Mätmaskin" : "";
            kv["SumD3_0"] = mOK ? "Egglinjal" : ""; kv["SumD3_11"] = mOK ? "Egglinjal" : "";
            kv["SumD4_1"] = mOK ? "Skjutmått" : ""; kv["SumD4_2"] = mOK ? "Skjutmått/Djupmått" : "";
            kv["SumD4_3"] = mOK ? "Skjutmått" : "";
            kv["SumD4_4"] = mOK ? "Multimar med " + tmpStmm + "mm rullar" : "";
            kv["SumD4_5"] = mOK ? "Skjutmått/Djupmått" : ""; kv["SumD4_6"] = mOK ? "Skjutmått" : "";
            kv["SumD4_7"] = mOK ? "Skjutmått" : ""; kv["SumD4_8"] = mOK ? "Skjutmått/Vinkelmätare" : "";
            kv["SumD4_9"] = mOK ? "Gängmall Tr" + tmpStmm + " för skruv" : ""; kv["SumD4_0"] = mOK ? "Mätmaskin" : "";

            // ── AF ────────────────────────────────────────────────────────────────
            kv["SumAF1_1"] = ""; kv["SumAF1_2"] = ""; kv["SumAF1_3"] = mOK ? "Rensvarvas" : ""; kv["SumAF1_4"] = "";
            kv["SumAF2_1"] = ""; kv["SumAF2_2"] = ""; kv["SumAF2_3"] = ""; kv["SumAF2_4"] = "";
            kv["SumAF2_5"] = mOK ? "Inställningsmått: " + kv["SumVmått"] : "";
            kv["SumAF3_1"] = ""; kv["SumAF3_2"] = mOK ? "Svarvas färdigt i OP4" : "";
            kv["SumAF3_3"] = mOK ? "Svarvas färdigt i OP4" : ""; kv["SumAF3_4"] = mOK ? "Svarvas färdigt i OP4" : "";
            kv["SumAF3_5"] = mOK ? "Svarvas färdigt i OP4" : ""; kv["SumAF3_6"] = "";
            kv["SumAF3_7"] = mOK ? "Tol: " + kv["SumGVarTol"] : "";
            kv["SumAF3_8"] = mOK ? "Tol: " + kv["SumVinkTol"] : "";
            kv["SumAF3_9"] = mOK ? "Max: " + kv["SumOrund"] : "";
            kv["SumAF3_0"] = mOK ? kv["SumRakA"] : ""; kv["SumAF3_11"] = mOK ? kv["SumRakB"] : "";
            kv["SumAF4_1"] = ""; kv["SumAF4_2"] = ""; kv["SumAF4_3"] = "";
            kv["SumAF4_4"] = mOK ? "Kontrolleras med klove utf.2" : "";
            kv["SumAF4_5"] = ""; kv["SumAF4_6"] = "";
            kv["SumAF4_7"] = mOK ? "max: " + FmtComma(sumgmax) + " | min: " + FmtComma(sumgmin) : "";
            kv["SumAF4_8"] = ""; kv["SumAF4_9"] = "";
            kv["SumAF4_0"] = mOK ? "Max: " + kv["SumSidkast"] : "";

            // ── Texts ─────────────────────────────────────────────────────────────
            kv["SumTextS1"] = ""; kv["SumTextS2"] = ""; kv["SumTextS3"] = ""; kv["SumTextS4"] = "";

            // ── Skärdata OP1 ──────────────────────────────────────────────────────
            kv["SumChuckback1"] = IsZeroStr(cb1Str) ? "" : cb1Str;            
            kv["SumCB1"] = IsZeroStr(cb1Str) ? "" : "Chuckbackar:";
            kv["SumStödback1"] = IsZeroStr(sb1Str) ? "" : sb1Str + " mm";
            kv["SumSB1"] = IsZeroStr(sb1Str) ? "" : "Stödbackar:";
            kv["SumGrader1"] = IsZeroStr(gr1Str) ? "" : gr1Str + " mm";
            kv["SumGR1"] = IsZeroStr(gr1Str) ? "" : "Grader:";
            kv["SumVarv1"] = IsZeroStr(vr1Str) ? "" : vr1Str + " /min";
            kv["SumVR1"] = IsZeroStr(vr1Str) ? "" : "Varvtal:";
            kv["SumMatPl1"] = IsZeroStr(mp1Str) ? "" : mp1Str + " /min";
            kv["SumMP1"] = IsZeroStr(mp1Str) ? "" : "Matning Plan:";
            kv["SumMatInUt1"] = IsZeroStr(mi1Str) ? "" : mi1Str + " /min";
            kv["SumMIU1"] = IsZeroStr(mi1Str) ? "" : "Matning Utv/Inv:";

            // Färdigmått OP1: bm=0->"", bm="1"->"[Tmpd2OP1] -0.5 mm", else direct
            string tmpFMUtv1 = string.IsNullOrEmpty(fmu1Str) || EqualsI(fmu1Str, "0") ? ""
                : EqualsI(fmu1Str, "1") ? FmtComma(tmpd2OP1) + " -0.5 mm" : fmu1Str;
            kv["SumFMUtv1"] = tmpFMUtv1;
            kv["SumFMU1"] = string.IsNullOrEmpty(tmpFMUtv1) ? "" : "Utvändig diameter:";
            kv["SumUtvLin1"] = IsZeroStr(lu1Str) ? "" : lu1Str + " mm";
            kv["SumUL1"] = IsZeroStr(lu1Str) ? "" : "Motsvarar på linjal:";

            string tmpFMInv1 = string.IsNullOrEmpty(fmi1Str) || EqualsI(fmi1Str, "0") ? ""
                : EqualsI(fmi1Str, "1") ? FmtComma(tmpd1OP1) + " -0.5 mm" : fmi1Str;
            kv["SumFMInv1"] = tmpFMInv1;
            kv["SumFMI1"] = string.IsNullOrEmpty(tmpFMInv1) ? "" : "Invändig diameter:";
            kv["SumInvLin1"] = IsZeroStr(li1Str) ? "" : li1Str + " mm";
            kv["SumIL1"] = IsZeroStr(li1Str) ? "" : "Motsvarar på linjal:";

            // ── Skärdata OP2 ──────────────────────────────────────────────────────
            kv["SumChuckback"] = IsZeroStr(cbStr) ? "" : cbStr;
            kv["SumCB"] = IsZeroStr(cbStr) ? "" : "Chuckbackar:";
            kv["SumStödback"] = IsZeroStr(sbStr) ? "" : sbStr + " mm";
            kv["SumSB"] = IsZeroStr(sbStr) ? "" : "Stödbackar:";
            kv["SumGrader"] = IsZeroStr(grStr) ? "" : grStr + " mm";
            kv["SumGR"] = IsZeroStr(grStr) ? "" : "Grader:";
            kv["SumVarv"] = IsZeroStr(vrStr) ? "" : vrStr + " /min";
            kv["SumVR"] = IsZeroStr(vrStr) ? "" : "Varvtal:";
            kv["SumMatPl"] = IsZeroStr(mpStr) ? "" : mpStr + " /min";
            kv["SumMP"] = IsZeroStr(mpStr) ? "" : "Matning Plan:";
            kv["SumMatInUt"] = IsZeroStr(miStr) ? "" : miStr + " /min";
            kv["SumMIU"] = IsZeroStr(miStr) ? "" : "Matning Utv/Inv:";

            string tmpFMUtv = string.IsNullOrEmpty(fmuStr) || EqualsI(fmuStr, "0") ? ""
                : EqualsI(fmuStr, "1") ? FmtComma(tmpdOP2) + " -0.5 mm" : fmuStr;
            kv["SumFMUtv"] = tmpFMUtv;
            if (subject == "MS-336569/V21")
                kv["SumFMUtv"] = "1064,3 -0.5 mm";
            kv["SumFMU"] = string.IsNullOrEmpty(tmpFMUtv) ? "" : "Utvändig diameter:";
            kv["SumUtvLin"] = IsZeroStr(luStr) ? "" : luStr + " mm";
            kv["SumUL"] = IsZeroStr(luStr) ? "" : "Motsvarar på linjal:";

            // TmpFMInv for OP2 = if bm="0"->"" else SumFMInv1
            string tmpFMInv = string.IsNullOrEmpty(fmiStr) || EqualsI(fmiStr, "0") ? "" : tmpFMInv1;
            kv["SumFMInv"] = tmpFMInv;
            kv["SumFMI"] = string.IsNullOrEmpty(tmpFMInv) ? "" : "Invändig diameter:";
            kv["SumInvLin"] = IsZeroStr(liStr) ? "" : liStr + " mm";
            kv["SumIL"] = IsZeroStr(liStr) ? "" : "Motsvarar på linjal:";

            // Överskrifter
            kv["SumIN"] = (IsZeroStr(cbStr) && IsZeroStr(sbStr) && IsZeroStr(grStr) && IsZeroStr(vrStr) && IsZeroStr(mpStr) && IsZeroStr(miStr)) ? "" : "Inst\u00e4llning";
            kv["SumIN1"] = (IsZeroStr(cb1Str) && IsZeroStr(sb1Str) && IsZeroStr(gr1Str) && IsZeroStr(vr1Str) && IsZeroStr(mp1Str) && IsZeroStr(mi1Str)) ? "" : "Inställning";
            kv["SumFM"] = (IsZeroStr(fmuStr) && IsZeroStr(luStr) && IsZeroStr(fmiStr) && IsZeroStr(liStr)) ? "" : "F\u00e4rdigm\u00e5tt";
            kv["SumFM1"] = (IsZeroStr(fmu1Str) && IsZeroStr(lu1Str) && IsZeroStr(fmi1Str) && IsZeroStr(li1Str)) ? "" : "Färdigmått";

            return kv;
        }

        // ── Popup ──────────────────────────────────────────────────────────────────
        private static string ComputePopup(string pub)
        {
            if (string.IsNullOrWhiteSpace(pub)) return "";
            DateTime dt; if (!DateTime.TryParse(pub, out dt)) return "";
            DateTime till = dt.AddDays(14);
            return DateTime.Today <= till.Date
                ? "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)\n\nPopupruta aktiv till " + till.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                : "";
        }

        // ── Tolerance helpers ─────────────────────────────────────────────────────

        // d1 JS9 (13-step) — same for OP3 pos and neg
        private static double D1Tol13(double d1)
        {
            if (d1 < 31) return 0.026; if (d1 < 51) return 0.031; if (d1 < 81) return 0.037;
            if (d1 < 121) return 0.043; if (d1 < 181) return 0.05; if (d1 < 251) return 0.057;
            if (d1 < 316) return 0.065; if (d1 < 401) return 0.07; if (d1 < 501) return 0.077;
            if (d1 < 631) return 0.087; if (d1 < 801) return 0.1; if (d1 < 1001) return 0.115;
            return 0.13;
        }

        // dm positive tol (5-step)
        private static double DmTolPos(int st)
        { return st == 4 ? 0.19 : st == 5 ? 0.212 : st == 6 ? 0.236 : st == 7 ? 0.25 : 0.265; }

        // dm negative tol (5-step)
        private static double DmTolNeg(int st)
        { return st == 4 ? 0.63 : st == 5 ? 0.71 : st == 6 ? 0.8 : st == 7 ? 0.85 : 0.95; }

        // d3 negative tol
        private static double D3TolN(int st)
        { return st == 4 ? 0.75 : st == 5 ? 0.85 : st == 6 ? 0.95 : st == 7 ? 1.0 : 1.12; }

        // d2 negative tol (5-step)
        private static double D2TolN(int st)
        { return st == 4 ? 0.3 : st == 5 ? 0.335 : st == 6 ? 0.375 : st == 7 ? 0.425 : 0.45; }

        // d4 negative tol
        private static double D4TolN(int serie)
        { return serie == 22 ? 0.2 : (serie == 240 || serie == 241) ? 0.25 : 0.3; }

        // L negative tol (14-step)
        private static double LTolN14(double L)
        {
            if (L < 11) return 0.22; if (L < 19) return 0.27; if (L < 31) return 0.33; if (L < 51) return 0.39;
            if (L < 81) return 0.46; if (L < 121) return 0.54; if (L < 181) return 0.63; if (L < 251) return 0.72;
            if (L < 316) return 0.81; if (L < 401) return 0.89; if (L < 501) return 0.97; if (L < 631) return 1.1;
            if (L < 801) return 1.25; return 1.4;
        }

        // b tol (14-step)
        private static double TolB(double b)
        {
            if (b < 11) return 0.29; if (b < 19) return 0.35; if (b < 31) return 0.42; if (b < 51) return 0.5;
            if (b < 81) return 0.6; if (b < 121) return 0.7; if (b < 181) return 0.8; if (b < 251) return 0.925;
            if (b < 316) return 1.050; if (b < 401) return 1.150; if (b < 501) return 1.250; if (b < 631) return 1.400;
            if (b < 801) return 1.600; return 1.800;
        }

        // GTj tolerance positive (kona=12 or 30)
        private static double GTjTolPos(double d, double kona)
        {
            if (Math.Abs(kona - 12) < 0.001)
            {
                if (d < 31) return 0.033; if (d < 51) return 0.039; if (d < 81) return 0.046; if (d < 121) return 0.054;
                if (d < 181) return 0.063; if (d < 251) return 0.072; if (d < 316) return 0.081; if (d < 401) return 0.089;
                if (d < 501) return 0.097; if (d < 631) return 0.105; if (d < 801) return 0.115; if (d < 1001) return 0.13;
                return 0.145;
            }
            if (Math.Abs(kona - 30) < 0.001)
            {
                if (d < 121) return 0.035; if (d < 181) return 0.04; if (d < 251) return 0.046; if (d < 316) return 0.052;
                if (d < 401) return 0.057; if (d < 501) return 0.063; if (d < 631) return 0.068; if (d < 801) return 0.076;
                if (d < 1001) return 0.084; return 0.095;
            }
            return 0;
        }

        private static double GVarTol(double d)
        {
            if (d < 51) return 0.008; if (d < 121) return 0.01; if (d < 181) return 0.015; if (d < 251) return 0.02;
            if (d < 316) return 0.025; if (d < 501) return 0.03; if (d < 631) return 0.035; if (d < 801) return 0.04;
            if (d < 1001) return 0.045; return 0.05;
        }

        private static string Orund(double d1)
        {
            if (d1 < 31) return "0.026"; if (d1 < 51) return "0.031"; if (d1 < 81) return "0.037"; if (d1 < 121) return "0.043";
            if (d1 < 181) return "0.050"; if (d1 < 251) return "0.057"; if (d1 < 316) return "0.065"; if (d1 < 401) return "0.070";
            if (d1 < 501) return "0.077"; if (d1 < 631) return "0.087"; if (d1 < 801) return "0.100"; if (d1 < 1001) return "0.115";
            return "0.130";
        }

        private static string VinkTol(double d)
        {
            if (d < 51) return "\u00b1 0.060"; if (d < 81) return "\u00b1 0.050"; if (d < 121) return "\u00b1 0.045";
            if (d < 151) return "\u00b1 0.030"; if (d < 181) return "\u00b1 0.018"; if (d < 401) return "\u00b1 0.015";
            if (d < 501) return "\u00b1 0.013"; if (d < 631) return "\u00b1 0.012"; if (d < 801) return "\u00b1 0.011";
            if (d < 1001) return "\u00b1 0.010"; return "\u00b1 0.009";
        }

        private static string Sidkast(double d)
        {
            if (d < 51) return "+ 0.040"; if (d < 121) return "+ 0.050"; if (d < 251) return "+ 0.060";
            if (d < 316) return "+ 0.070"; if (d < 401) return "+ 0.080"; if (d < 501) return "+ 0.090";
            if (d < 631) return "+ 0.100"; if (d < 801) return "+ 0.120"; if (d < 1001) return "+ 0.140";
            return "+ 0.160";
        }

        private static double RakA(double d)
        { return (d < 101 ? 8 : d < 281 ? 10 : d < 481 ? 12 : d < 601 ? 14 : d < 901 ? 16 : 20) / 1000.0; }
        private static double RakB(double d)
        { return (d < 101 ? 12 : d < 281 ? 15 : d < 481 ? 18 : d < 601 ? 21 : d < 901 ? 24 : 30) / 1000.0; }

        private static string Sumg1(int st)
        {
            switch (st)
            {
                case 8: return "5.3x45\u00ba";
                case 7: return "4.4x45\u00ba";
                case 6: return "3.8x45\u00ba";
                case 5: return "3.2x45\u00ba";
                case 4: return "2.7x45\u00ba";
                case 3: return "2.4x45\u00ba";
                case 2: return "1.7x45\u00ba";
                default: return "1.3x45\u00ba";
            }
        }

        private static string Sumg2(string g1, string serie, int serieInt, int aes, double d2)
        {
            if (serieInt == 22 || serieInt == 240 || serieInt == 241 || serieInt == 30) return g1;
            if (aes == 1) return g1;
            if (serieInt == 31) return d2 < 510 ? "3.25x45\u00ba" : g1;
            if (serieInt == 32)
            {
                if (d2 < 510) return "3.25x45\u00ba";
                if (Math.Abs(d2 - 600) < 0.01) return "3.8x45\u00ba";
                if (d2 < 700) return "4x45\u00ba";
                if (d2 < 790) return "4.5x45\u00ba";
                if (d2 < 940) return "4.4x45\u00ba";
                if (Math.Abs(d2 - 950) < 0.01 || Math.Abs(d2 - 1000) < 0.01) return "5.5x45\u00ba";
                if (Math.Abs(d2 - 1060) < 0.01) return "5.3x45\u00ba";
                return g1;
            }
            return g1;
        }

        private static string Sumr1a(string serie, string typ, int serieInt, double typNum, double d2)
        {
            if (EqualsI(serie, "LW")) return "";
            if (serieInt == 240 || serieInt == 241 || serieInt == 30) return "";
            if (serieInt == 31)
            {
                if (Math.Abs(d2 - 200) < 0.01 || Math.Abs(d2 - 420) < 0.01 || Math.Abs(d2 - 440) < 0.01 ||
                    Math.Abs(d2 - 460) < 0.01 || Math.Abs(d2 - 480) < 0.01 || Math.Abs(d2 - 500) < 0.01)
                    return "R max 1.2";
                return "";
            }
            if (serieInt == 39) return typNum > 500 ? "" : "R max 1.2";
            return "R max 1.2";
        }

        private static string SumRitningsnr(int serie, int aes, double d, string bet)
        {
            if (serie == 22) return "7437361";
            if (serie == 240)
            {
                if (aes == 0) return d < 379 ? "7437370" : "7437371";
                return d < 379 ? "232631" : "232632";
            }
            if (serie == 241)
            {
                if (aes == 0) return d < 379 ? "7437372" : "7437373";
                return "232636";
            }
            if (serie == 30)
            {
                if (d < 529) return aes == 0 ? "7437362" : "234385";
                return "7437363";
            }
            if (serie == 31)
            {
                if (d < 529) return aes == 0 ? "7437364" : "231282";
                return "7437365";
            }
            if (serie == 32) return aes == 0 ? "7437366" : "231638";
            return bet;
        }

        // ── Utilities ─────────────────────────────────────────────────────────────
        private static bool EqualsI(string a, string b) =>
            string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);

        private static int TryParseInt(string s)
        { int v; return int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0; }

        private static double TryParseDouble(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;
            double v;
            return double.TryParse(s.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }


        // Lotus checks string "0" not numeric 0 — values like "70/80" are valid
        private static bool IsZeroStr(string s) =>
            string.IsNullOrEmpty(s) || s.Trim() == "0";

        private static double GetDouble(List<Bookmark> bm, string key)
        {
            string r = GetString(bm, key);
            if (string.IsNullOrWhiteSpace(r)) return 0;
            double v;
            return double.TryParse(r.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static string GetString(List<Bookmark> bm, string key)
        {
            if (bm == null || string.IsNullOrWhiteSpace(key)) return "";
            for (int i = 0; i < bm.Count; i++)
            {
                Bookmark b = bm[i];
                if (b != null && string.Equals(b.BookmarkName ?? "", key, StringComparison.OrdinalIgnoreCase))
                    return b.BookmarkValue ?? "";
            }
            return "";
        }

        // Direct [TmpX] — Swedish comma decimal (e.g. "228,5")
        private static string FmtComma(double v) =>
            v.ToString("0.################", CommonFunctions.Culture).Replace(".", ",");

        // @ReplaceSubstring / @text("F3") — dot decimal (e.g. "0.500")
        private static string Fmt3(double v) =>
            v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
    }
}
