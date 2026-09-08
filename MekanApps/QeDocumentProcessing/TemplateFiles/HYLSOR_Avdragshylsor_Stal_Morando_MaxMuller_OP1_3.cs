using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Avdragshylsor_Stal_Morando_MaxMuller_OP1_3 : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;
            List<Bookmark> bm = req != null ? req.Bookmarks : null;

            // No popup formula in this script

            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");
            bool tmpSlash = tmpBet.IndexOf('/') >= 0;

            // Explode " /.-"
            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : "";
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : "";
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : "";

            int cntB2 = tmpBet2.Length;

            // TmpSerie
            string tmpSerie;
            if (tmpSlash && (cntB2 == 3 || cntB2 == 2)) tmpSerie = tmpBet2;
            else if (cntB2 > 4) tmpSerie = tmpBet2.Substring(0, 3);
            else if (cntB2 == 3) tmpSerie = tmpBet2.Substring(0, 1);
            else tmpSerie = tmpBet2.Length >= 2 ? tmpBet2.Substring(0, 2) : tmpBet2;
            int serieInt = TryParseInt(tmpSerie);

            // TmpTyp: slash -> Bet3, else Right(2)
            string tmpTyp = tmpSlash ? tmpBet3 : (tmpBet2.Length >= 2 ? tmpBet2.Substring(tmpBet2.Length - 2) : tmpBet2);

            // ── Bookmarks ─────────────────────────────────────────────────────────
            double tmpda = GetDouble(bm, "Kona lillände diameter (d)");
            double tmpd1a = GetDouble(bm, "Innerdiameter (d1)");
            double tmpd2a = GetDouble(bm, "Ytterdiameter (d2)");
            double tmpd4bm = GetDouble(bm, "Släppning (d4)");
            double tmphbm = GetDouble(bm, "Bredd släppning (h)");
            double tmpb = GetDouble(bm, "Längd till gänga (b)");
            double tmpLbm = GetDouble(bm, "Längd (L)");
            double amatt = GetDouble(bm, "a-mått");
            double konaBm = GetDouble(bm, "Kona");
            int tmpAES = (int)GetDouble(bm, "Äldre standard");
            string bOP1bm = GetString(bm, "OP1 Längd till gänga (b)");

            // Skärdata bookmarks
            string cbStr = GetString(bm, "Chuckbackar"); bool cbD = !IsZeroStr(cbStr);
            string sbStr = GetString(bm, "Stödbackar"); bool sbD = !IsZeroStr(sbStr);
            string grStr = GetString(bm, "Grader"); bool grD = !IsZeroStr(grStr);
            string vrStr = GetString(bm, "Varvtal"); bool vrD = !IsZeroStr(vrStr);
            string mpStr = GetString(bm, "Matning Plan"); bool mpD = !IsZeroStr(mpStr);
            string miStr = GetString(bm, "Matning Utv/Inv"); bool miD = !IsZeroStr(miStr);
            string fmuStr = GetString(bm, "Färdigmått Utv");
            string luStr = GetString(bm, "Linjal Utv"); bool luD = !IsZeroStr(luStr);
            string fmiStr = GetString(bm, "Färdigmått Inv");
            string liStr = GetString(bm, "Linjal Inv"); bool liD = !IsZeroStr(liStr);

            // TmpKona
            double tmpKona = konaBm == 0
                ? ((serieInt == 240 || serieInt == 241) ? 30 : EqualsI(tmpSerie, "LW") ? 0 : 12)
                : konaBm;
            kv["SumV"] = tmpKona == 30 ? "0\u00ba57" : "2\u00ba23";
            kv["SumKona"] = "Kona 1:" + FmtComma(tmpKona);
            kv["SumKonaOP3"] = kv["SumKona"];

            // TmpStmm — based on Tmpd2a (NOT Tmpda)!
            int tmpStmm = tmpd2a < 301 ? 4 : tmpd2a < 501 ? 5 : tmpd2a < 701 ? 6 : tmpd2a < 901 ? 7 : 8;

            // Gänga — direct [Tmpd2a] -> FmtComma
            kv["SumGänga"] = "Tr" + FmtComma(tmpd2a) + "x" + tmpStmm;
            kv["SumP"] = "Tr" + tmpStmm;
            kv["SumP1"] = tmpStmm.ToString(CultureInfo.InvariantCulture);
            kv["SumRullar"] = tmpStmm + "mm";

            // ── b formulas ────────────────────────────────────────────────────────
            // bOP1: if bookmark "0" or empty -> b-3, else use bookmark
            double tmpbOP1 = (string.IsNullOrEmpty(bOP1bm) || EqualsI(bOP1bm, "0"))
                ? tmpb - 3.0 : TryParseDouble(bOP1bm);
            kv["SumbOP1"] = "(b) " + FmtComma(tmpbOP1);
            kv["SumbOP1Tol"] = " 0";
            kv["SumbOP1TolN"] = "- 1.0";

            // bOP2 = b + 1.3 -> direct -> FmtComma
            double tmpbOP2 = tmpb + 1.3;
            kv["SumbOP2"] = "(b) " + FmtComma(tmpbOP2);
            kv["SumbOP2Tol"] = "\u00b1 0.3";

            // bOP3 = b -> direct -> FmtComma
            kv["SumbOP3"] = "(b) " + FmtComma(tmpb);
            kv["SumbOP3Tol"] = BOP3Tol(tmpb) + " [3F]";

            // ── Faser g1, g2 ──────────────────────────────────────────────────────
            string sumg1 = Sumg1(tmpStmm);
            kv["Sumg1"] = sumg1;
            kv["Sumg2"] = Sumg2(sumg1, serieInt, tmpAES, tmpd2a);

            // ── md (medelgängdiameter) — direct -> FmtComma ────────────────────────
            double tmpmd = tmpStmm == 4 ? tmpd2a - 2 : tmpStmm == 5 ? tmpd2a - 2.5
                         : tmpStmm == 6 ? tmpd2a - 3 : tmpStmm == 7 ? tmpd2a - 3.5 : tmpd2a - 4;
            kv["Summd"] = "(md) " + FmtComma(tmpmd);
            // Hardcoded dot strings in Lotus -> as-is
            kv["SummdTol"] = MdTolPos(tmpStmm) + " [3F]";
            kv["SummdTolN"] = MdTolNeg(tmpStmm) + " [3F]";

            // ── d (kona lillände) — @Round -> FmtComma ────────────────────────────
            double tmpd = Math.Round(tmpda + (amatt / tmpKona) + 2.0, 1);
            kv["Sumd"] = "(d) " + FmtComma(tmpd);
            kv["SumdTol"] = "+ 0.5";
            kv["SumdTolN"] = " 0";

            // ── d1 — direct -> FmtComma ───────────────────────────────────────────
            double tmpd1 = tmpd1a - 2.0;
            kv["Sumd1"] = "(d1) " + FmtComma(tmpd1);
            kv["Sumd1Tol"] = " 0";
            kv["Sumd1TolN"] = "- 0.5";
            // d1 OP3 = Tmpd1a -> direct -> FmtComma
            kv["Sumd1OP3"] = "(d1) " + FmtComma(tmpd1a);
            kv["Sumd1OP3Tol"] = D1OP3Tol(tmpd1a) + " [3F]";

            // ── d2 — direct -> FmtComma ───────────────────────────────────────────
            kv["Sumd2"] = "(d2) " + FmtComma(tmpd2a);
            kv["Sumd22"] = "(d2) " + FmtComma(tmpd2a);
            kv["Sumd2Tol"] = " 0";
            kv["Sumd2TolN"] = D2TolN(tmpStmm);
            kv["Sumd22Tol"] = " 0";
            kv["Sumd22TolN"] = kv["Sumd2TolN"];

            // ── d3 — direct -> FmtComma ───────────────────────────────────────────
            double tmpd3 = tmpStmm == 4 ? tmpmd - 2.5 : tmpStmm == 5 ? tmpmd - 3
                         : tmpStmm == 6 ? tmpmd - 4 : tmpStmm == 7 ? tmpmd - 4.5
                         : (tmpmd < 1300 ? tmpmd - 5 : tmpmd - 4.5);
            kv["Sumd3"] = "(d3) " + FmtComma(tmpd3);
            kv["Sumd3Tol"] = " 0";
            kv["Sumd3TolN"] = D3TolN(tmpStmm);

            // ── d4, h — direct bookmark -> FmtComma ──────────────────────────────
            kv["Sumd4"] = "(d4) " + FmtComma(tmpd4bm).Replace(".",",");
            kv["Sumd4Tol"] = " 0";
            kv["Sumd4TolN"] = D4TolN(serieInt);
            kv["Sumh"] = "(h) " + FmtComma(tmphbm);
            kv["SumhTol"] = "\u00b1 0.2";

            // ── d6 (kona storände) — @Round -> FmtComma ───────────────────────────
            double tmpd6 = Math.Round((tmpbOP1 / tmpKona) + tmpd, 1);
            kv["Sumd6"] = "(d6) " + FmtComma(tmpd6);
            if(subject == "AOH 3280 G" || subject == "AOH 3280/350 G")
                kv["Sumd6"] = "(d6) 424,1";
            if (subject == "MS-233762/1")
                kv["Sumd6"] = "(d6) 615,5";

            kv["Sumd6Tol"] = "+ 0.5";
            kv["Sumd6TolN"] = " 0";

            // ── L formulas — direct -> FmtComma ──────────────────────────────────
            double tmpLOP1 = tmpLbm + 5;
            double tmpLOP2 = tmpLbm + 1;
            kv["SumLOP1"] = "(L) " + FmtComma(tmpLOP1);
            kv["SumLOP1Tol"] = "\u00b1 0.5";
            kv["SumLOP2"] = "(L) " + FmtComma(tmpLOP2);
            kv["SumLOP2Tol"] = " 0";
            kv["SumLOP2TolN"] = "- 0.5";
            kv["SumLOP3"] = "(L) " + FmtComma(tmpLbm);
            kv["SumLOP3Tol"] = " 0 [3F]";
            kv["SumLOP3TolN"] = LOP3TolN(tmpLbm) + " [3F]";

            // ── g (gänglängd hjälpmått) — direct -> FmtComma ─────────────────────
            // NOTE: Tmpg = L - b - 0.3 (0.3 subtracted!)
            double tmpg = tmpLbm - tmpb - 0.3;
            kv["Sumg"] = "(g) " + FmtComma(tmpg);
            kv["SumgTol"] = "\u00b1 0.3";

            // ── Radier ────────────────────────────────────────────────────────────
            kv["Sumr"] = "R 1.5";
            kv["Sumr1OP3"] = "R 1.5";
            int tmpROP3 = tmpda < 125 ? 50 : 60;
            kv["SumROP3"] = "R " + tmpROP3;
            kv["Sumr1"] = Sumr1(serieInt, tmpd2a);
            int tmpe = tmpROP3 == 50 ? 5 : 6;
            kv["Sume"] = "(e) " + tmpe;

            // ── Ra ────────────────────────────────────────────────────────────────
            kv["SumRa5"] = "5 [2F]";
            kv["SumRa"] = "2.5 [2F]";
            kv["SumRa1"] = "2.5 [2F]";
            kv["SumRa5a"] = "5";

            // ── GTj — based on Tmpda and Kona — hardcoded dot strings ─────────────
            kv["SumGTjTol"] = GTjTolPos(tmpda, tmpKona) + " [3F]";
            kv["SumGTjTolN"] = " 0 [2F]";
            kv["SumGTjTol2"] = kv["SumGTjTol"];
            kv["SumGTjTolN2"] = kv["SumGTjTolN"];

            // ── GVar, Sidkast — based on Tmpda ────────────────────────────────────
            kv["SumGVarTol"] = GVarTol(tmpda) + " [2F]";
            kv["SumSidkast"] = Sidkast(tmpda) + " [2F]";

            // ── Orundhet, VinkTol — based on Tmpd1a / Tmpda ───────────────────────
            kv["SumOrund"] = Orund(tmpd1a) + " [3F]";
            kv["SumVTol"] = VinkTol(tmpda) + " [2F]";

            // ── Rakhet — based on Tmpda ───────────────────────────────────────────
            kv["SumRakA"] = "Max: " + FmtComma(RakA(tmpda));
            kv["SumRakB"] = "Max: " + FmtComma(RakB(tmpda));

            // ── Mätbygelinställning ───────────────────────────────────────────────
            double sumML = tmpb - tmphbm;
            double tmp8 = Math.Round(((tmpda - tmpd1a) / 2.0) + ((amatt + 8) / (2.0 * tmpKona)), 3);
            double tmp108 = Math.Round(((tmpda - tmpd1a) / 2.0) + ((amatt + 108) / (2.0 * tmpKona)), 3);
            double tmp40 = Math.Round(((tmpda - tmpd1a) / 2.0) + ((amatt + 40) / (2.0 * tmpKona)), 3);
            double tmp140 = Math.Round(((tmpda - tmpd1a) / 2.0) + ((amatt + 140) / (2.0 * tmpKona)), 3);
            kv["SumML"] = FmtComma(sumML);
            kv["SumL1"] = FmtComma(sumML < 145 ? 8 : 40);
            kv["SumL2"] = FmtComma(sumML < 145 ? 108 : 140);
            kv["SumE1"] = FmtComma(sumML < 145 ? tmp8 : tmp40);
            kv["SumE2"] = FmtComma(sumML < 145 ? tmp108 : tmp140);
            // Byglar: 7419471 (different from Skepp6 class which uses 7415983)
            string bygel = sumML < 145 ? "SR 7419471" : "SR 7415991";
            kv["SumBygGtj"] = bygel;
            kv["SumBygGVar"] = bygel;
            kv["SumBygVinkTol"] = bygel;

            // ── Ritningar ─────────────────────────────────────────────────────────
            // NOTE: serie 31 has NO d-split (unlike Skepp6 class)
            string ritNr = SumRitningsnr(serieInt, tmpAES, tmpda, tmpBet);
            string sumRitS1 = "Produktritning: " + ritNr;
            kv["SumRitningsnrS1"] = sumRitS1;
            kv["SumRitningsnrS2"] = sumRitS1;
            kv["SumRitningsnrS3"] = sumRitS1;
            kv["SumRitTol"] = "TOL: 1432011";
            kv["SumRitGänga"] = "Gänga: 237359, 7430181";
            string klEg = "PRODUCTION NUTS & SLEEVES & HOUSINGSALLM\u00c4NKLASSADE EGENSKAPERKlassade egenskaper Avdragshylsor";
            kv["SumKlEgenskaper"] = klEg;
            kv["SumKlEgenskaperS2"] = klEg;
            kv["SumKlEgenskaperS3"] = klEg;

            // ── Machine — single machine string, TmpMV flag ───────────────────────
            bool mOK = EqualsI(maskinVal, "Morando OP1 - MaxMuller OP2-3");
            // TmpMV = "MV1" if match, else ""
            string tmpMV = mOK ? "MV1" : "";
            bool mv1 = EqualsI(tmpMV, "MV1");

            kv["SumMaskinValS1"] = "Maskin: " + (mv1 ? "Morando OP1" : "");
            kv["SumMaskinValS2"] = "Maskin: " + (mv1 ? "Max Muller OP2" : "");
            kv["SumMaskinValS3"] = "Maskin: " + (mv1 ? "Max Muller OP3" : "");

            // ── Frequencies (named keys) ──────────────────────────────────────────
            kv["SumF_L"] = mv1 ? "1/1" : "";
            kv["SumF_LOP2"] = mv1 ? "1/2" : "";
            kv["SumF_b"] = mv1 ? "1/1" : "";
            kv["SumF_bOP2"] = mv1 ? "1/2" : "";
            kv["SumF_d2"] = mv1 ? "1/5" : "";
            kv["SumF_md"] = mv1 ? "1/1" : "";
            kv["SumF_d4"] = mv1 ? "1/2" : "";
            kv["SumF_d"] = mv1 ? "1/1" : "";
            kv["SumF_d6"] = mv1 ? "1/1" : "";
            kv["SumF_d1"] = mv1 ? "1/1" : "";
            kv["SumF_h"] = mv1 ? "1/2" : "";
            kv["SumF_g"] = mv1 ? "1/2" : "";
            kv["SumF_F"] = mv1 ? "1/5" : "";
            kv["SumF_P"] = mv1 ? "1/5" : "";
            kv["SumF_d1OP3"] = mv1 ? "1/1" : "";
            kv["SumF_LOP3"] = mv1 ? "1/2" : "";
            kv["SumF_bOP3"] = mv1 ? "1/5" : "";
            kv["SumF_GTj"] = mv1 ? "1/1" : "";
            kv["SumF_GVar"] = mv1 ? "1/1" : "";
            kv["SumF_VTol"] = mv1 ? "1/1" : "";
            kv["SumF_Rd"] = mv1 ? "Inst." : "";
            kv["SumF_Ar"] = mv1 ? "1/5" : "";
            kv["SumF_Br"] = mv1 ? "1/5" : "";
            kv["SumF_Ks"] = mv1 ? "Inst." : "";

            // ── Devices (named keys) ──────────────────────────────────────────────
            kv["SumD_L"] = mv1 ? "Djupmått" : "";
            kv["SumD_LOP2"] = mv1 ? "Skjutmått" : "";
            kv["SumD_b"] = mv1 ? "Djupmått" : "";
            kv["SumD_bOP2"] = mv1 ? "Skjutmått" : "";
            kv["SumD_d2"] = mv1 ? "Skjutmått" : "";
            kv["SumD_md"] = mv1 ? "Multimar med " + tmpStmm + "mm rullar" : "";
            kv["SumD_d4"] = mv1 ? "Skjutmått alt. djupmått" : "";
            kv["SumD_d"] = mv1 ? "Skjutmått" : "";
            kv["SumD_d6"] = mv1 ? "Skjutmått / mikrometer" : "";
            kv["SumD_d1"] = mv1 ? "Skjutmått / inv. mikrometer" : "";
            kv["SumD_h"] = mv1 ? "Skjutmått" : "";
            kv["SumD_g"] = mv1 ? "Skjutmått" : "";
            kv["SumD_F"] = mv1 ? "Skjutmått / Vinkelmätare" : "";
            kv["SumD_P"] = mv1 ? "Gängmall Tr" + tmpStmm + " för skruv" : "";
            kv["SumD_d1OP3"] = mv1 ? "Mikrometerstickmått" : "";
            kv["SumD_LOP3"] = mv1 ? "Skjutmått" : "";
            kv["SumD_bOP3"] = mv1 ? "Djupmått / Skjutmått" : "";
            kv["SumD_GTj"] = mv1 ? "Mätbygel " + bygel : "";
            kv["SumD_GVar"] = mv1 ? "Mätbygel " + bygel : "";
            kv["SumD_VTol"] = mv1 ? "Mätbygel " + bygel : "";
            kv["SumD_Rd"] = mv1 ? "Mätmaskin" : "";
            kv["SumD_Ar"] = mv1 ? "Egglinjal" : "";
            kv["SumD_Br"] = mv1 ? "Egglinjal" : "";
            kv["SumD_Ks"] = mv1 ? "Mätmaskin" : "";

            // ── AF (named keys) ───────────────────────────────────────────────────
            kv["SumAF_L"] = ""; kv["SumAF_LOP2"] = ""; kv["SumAF_b"] = "";
            kv["SumAF_bOP2"] = ""; kv["SumAF_d2"] = "";
            kv["SumAF_md"] = mv1 ? "Kontrolleras med klove utf.2" : "";
            kv["SumAF_d4"] = ""; kv["SumAF_d"] = ""; kv["SumAF_d1"] = "";
            kv["SumAF_d6"] = ""; kv["SumAF_h"] = "";
            kv["SumAF_g"] = mv1 ? "Hjälpmått" : "";
            kv["SumAF_F"] = ""; kv["SumAF_P"] = "";
            kv["SumAF_d1OP3"] = ""; kv["SumAF_LOP3"] = ""; kv["SumAF_bOP3"] = "";
            kv["SumAF_GVar"] = mv1 ? "Tol: " + kv["SumGVarTol"] : "";
            kv["SumAF_VTol"] = mv1 ? "Tol: " + kv["SumVTol"] : "";
            kv["SumAF_Rd"] = mv1 ? "Max: " + kv["SumOrund"] : "";
            kv["SumAF_Ar"] = mv1 ? kv["SumRakA"] : "";
            kv["SumAF_Br"] = mv1 ? kv["SumRakB"] : "";
            kv["SumAF_Ks"] = mv1 ? "Max: " + kv["SumSidkast"] : "";

            // ── Texts ─────────────────────────────────────────────────────────────
            string sumText = "Grader, frifläckar, slahmärken, repor, valkar & andra defekter samt ojämnheter kontrolleras visuellt.";
            kv["SumTextS1"] = sumText;
            kv["SumTextS2"] = sumText;
            kv["SumTextS3"] = sumText;

            // ── Skärdata ──────────────────────────────────────────────────────────
            kv["SumChuckback"] = cbD ? cbStr : "";
            kv["SumCB"] = cbD ? "Chuckbackar:" : "";
            kv["SumStödback"] = sbD ? sbStr + " mm" : "";
            kv["SumSB"] = sbD ? "Stödbackar:" : "";
            kv["SumGrader"] = grD ? grStr + " mm" : "";
            kv["SumGR"] = grD ? "Grader:" : "";
            kv["SumVarv"] = vrD ? vrStr + " /min" : "";
            kv["SumVR"] = vrD ? "Varvtal:" : "";
            kv["SumMatPl"] = mpD ? mpStr + " /min" : "";
            kv["SumMP"] = mpD ? "Matning Plan:" : "";
            kv["SumMatInUt"] = miD ? miStr + " /min" : "";
            kv["SumMIU"] = miD ? "Matning Utv/Inv:" : "";

            // FMUtv: if bm=0 -> "" else always "[Tmpd] +0.5 mm" (no bm="1" check)
            kv["SumFMUtv"] = IsZeroStr(fmuStr) ? "" : FmtComma(tmpd) + " +0.5 mm";
            kv["SumFMU"] = IsZeroStr(fmuStr) ? "" : "Utvändig diameter:";
            kv["SumUtvLin"] = luD ? luStr + " mm" : "";
            kv["SumUL"] = luD ? "Motsvarar på linjal:" : "";

            // FMInv: if bm=0 -> "" else always "[Tmpd1] -0.5 mm"
            kv["SumFMInv"] = IsZeroStr(fmiStr) ? "" : FmtComma(tmpd1) + " -0.5 mm";
            kv["SumFMI"] = IsZeroStr(fmiStr) ? "" : "Invändig diameter:";
            kv["SumInvLin"] = liD ? liStr + " mm" : "";
            kv["SumIL"] = liD ? "Motsvarar på linjal:" : "";

            kv["SumIN"] = (cbD || sbD || grD || vrD || mpD || miD) ? "Inställning" : "";
            kv["SumFM"] = (!IsZeroStr(fmuStr) || luD || !IsZeroStr(fmiStr) || liD) ? "Färdigmått" : "";

            return kv;
        }

        // ── Tolerance helpers (all return dot-decimal strings) ────────────────────

        private static string BOP3Tol(double b)
        {
            if (b < 11) return "\u00b1 0.290"; if (b < 19) return "\u00b1 0.350"; if (b < 31) return "\u00b1 0.420";
            if (b < 51) return "\u00b1 0.500"; if (b < 81) return "\u00b1 0.600"; if (b < 121) return "\u00b1 0.700";
            if (b < 181) return "\u00b1 0.800"; if (b < 251) return "\u00b1 0.925"; if (b < 316) return "\u00b1 1.050";
            if (b < 401) return "\u00b1 1.150"; if (b < 501) return "\u00b1 1.250"; if (b < 631) return "\u00b1 1.400";
            if (b < 801) return "\u00b1 1.600"; return "\u00b1 1.800";
        }

        private static string LOP3TolN(double L)
        {
            if (L < 11) return "- 0.220"; if (L < 19) return "- 0.270"; if (L < 31) return "- 0.330";
            if (L < 51) return "- 0.390"; if (L < 81) return "- 0.460"; if (L < 121) return "- 0.540";
            if (L < 181) return "- 0.630"; if (L < 251) return "- 0.720"; if (L < 316) return "- 0.810";
            if (L < 401) return "- 0.890"; if (L < 501) return "- 0.970"; if (L < 631) return "- 1.100";
            if (L < 801) return "- 1.250"; return "- 1.400";
        }

        private static string D1OP3Tol(double d1)
        {
            if (d1 < 31) return "\u00b1 0.026"; if (d1 < 51) return "\u00b1 0.031"; if (d1 < 81) return "\u00b1 0.037";
            if (d1 < 121) return "\u00b1 0.043"; if (d1 < 181) return "\u00b1 0.050"; if (d1 < 251) return "\u00b1 0.057";
            if (d1 < 316) return "\u00b1 0.065"; if (d1 < 401) return "\u00b1 0.070"; if (d1 < 501) return "\u00b1 0.077";
            if (d1 < 631) return "\u00b1 0.087"; if (d1 < 801) return "\u00b1 0.100"; if (d1 < 1001) return "\u00b1 0.115";
            return "\u00b1 0.130";
        }

        private static string MdTolPos(int st)
        { return st == 4 ? "- 0.190" : st == 5 ? "- 0.212" : st == 6 ? "- 0.236" : st == 7 ? "- 0.250" : "- 0.265"; }
        private static string MdTolNeg(int st)
        { return st == 4 ? "- 0.630" : st == 5 ? "- 0.710" : st == 6 ? "- 0.800" : st == 7 ? "- 0.850" : "- 0.950"; }
        private static string D2TolN(int st)
        { return st == 4 ? "- 0.300" : st == 5 ? "- 0.335" : st == 6 ? "- 0.375" : st == 7 ? "- 0.425" : "- 0.450"; }
        private static string D3TolN(int st)
        { return st == 4 ? "- 0.750" : st == 5 ? "- 0.850" : st == 6 ? "- 0.950" : st == 7 ? "- 1.000" : "- 1.120"; }
        private static string D4TolN(int serie)
        { return serie == 22 ? "- 0.2" : (serie == 240 || serie == 241) ? "- 0.25" : "- 0.3"; }

        private static string GTjTolPos(double d, double kona)
        {
            if (Math.Abs(kona - 12) < 0.001)
            {
                if (d < 31) return "+ 0.033"; if (d < 51) return "+ 0.039"; if (d < 81) return "+ 0.046";
                if (d < 121) return "+ 0.054"; if (d < 181) return "+ 0.063"; if (d < 251) return "+ 0.072";
                if (d < 316) return "+ 0.081"; if (d < 401) return "+ 0.089"; if (d < 501) return "+ 0.097";
                if (d < 631) return "+ 0.105"; if (d < 801) return "+ 0.115"; if (d < 1001) return "+ 0.130";
                return "+ 0.145";
            }
            if (Math.Abs(kona - 30) < 0.001)
            {
                if (d < 121) return "+ 0.035"; if (d < 181) return "+ 0.040"; if (d < 251) return "+ 0.046";
                if (d < 316) return "+ 0.052"; if (d < 401) return "+ 0.057"; if (d < 501) return "+ 0.063";
                if (d < 631) return "+ 0.068"; if (d < 801) return "+ 0.076"; if (d < 1001) return "+ 0.084";
                return "+ 0.095";
            }
            return "Fel Kona";
        }

        private static string GVarTol(double d)
        {
            if (d < 51) return "+ 0.008"; if (d < 121) return "+ 0.010"; if (d < 181) return "+ 0.015";
            if (d < 251) return "+ 0.020"; if (d < 316) return "+ 0.025"; if (d < 501) return "+ 0.030";
            if (d < 631) return "+ 0.035"; if (d < 801) return "+ 0.040"; if (d < 1001) return "+ 0.045";
            return "+ 0.050";
        }

        private static string Sidkast(double d)
        {
            if (d < 51) return "+ 0.040"; if (d < 121) return "+ 0.050"; if (d < 251) return "+ 0.060";
            if (d < 316) return "+ 0.070"; if (d < 401) return "+ 0.080"; if (d < 501) return "+ 0.090";
            if (d < 631) return "+ 0.100"; if (d < 801) return "+ 0.120"; if (d < 1001) return "+ 0.140";
            return "+ 0.160";
        }

        private static string Orund(double d1)
        {
            if (d1 < 31) return "0.026"; if (d1 < 51) return "0.031"; if (d1 < 81) return "0.037";
            if (d1 < 121) return "0.043"; if (d1 < 181) return "0.050"; if (d1 < 251) return "0.057";
            if (d1 < 316) return "0.065"; if (d1 < 401) return "0.070"; if (d1 < 501) return "0.077";
            if (d1 < 631) return "0.087"; if (d1 < 801) return "0.100"; if (d1 < 1001) return "0.115";
            return "0.130";
        }

        private static string VinkTol(double d)
        {
            if (d < 51) return "\u00b1 0.060"; if (d < 81) return "\u00b1 0.050"; if (d < 121) return "\u00b1 0.045";
            if (d < 151) return "\u00b1 0.030"; if (d < 181) return "\u00b1 0.018"; if (d < 401) return "\u00b1 0.015";
            if (d < 501) return "\u00b1 0.013"; if (d < 631) return "\u00b1 0.012"; if (d < 801) return "\u00b1 0.011";
            if (d < 1001) return "\u00b1 0.010"; return "\u00b1 0.009";
        }

        private static double RakA(double d) { return (d < 101 ? 8 : d < 281 ? 10 : d < 481 ? 12 : d < 601 ? 14 : d < 901 ? 16 : 20) / 1000.0; }
        private static double RakB(double d) { return (d < 101 ? 12 : d < 281 ? 15 : d < 481 ? 18 : d < 601 ? 21 : d < 901 ? 24 : 30) / 1000.0; }

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

        private static string Sumg2(string g1, int serie, int aes, double d2a)
        {
            if (serie == 22 || serie == 240 || serie == 241 || serie == 30) return g1;
            if (aes == 1) return g1;
            if (serie == 31) return d2a < 510 ? "3.25x45\u00ba" : g1;
            if (serie == 32)
            {
                if (d2a < 510) return "3.25x45\u00ba";
                if (Math.Abs(d2a - 600) < 0.01) return "3.8x45\u00ba";
                if (d2a < 700) return "4x45\u00ba"; if (d2a < 790) return "4.5x45\u00ba";
                if (d2a < 940) return "4.4x45\u00ba";
                if (Math.Abs(d2a - 950) < 0.01 || Math.Abs(d2a - 1000) < 0.01) return "5.5x45\u00ba";
                if (Math.Abs(d2a - 1060) < 0.01) return "5.3x45\u00ba";
                return g1;
            }
            return g1;
        }

        private static string Sumr1(int serie, double d2a)
        {
            if (serie == 240 || serie == 241 || serie == 30) return "";
            if (serie == 31)
            {
                if (Math.Abs(d2a - 200) < 0.01 || Math.Abs(d2a - 420) < 0.01 || Math.Abs(d2a - 440) < 0.01 ||
                    Math.Abs(d2a - 460) < 0.01 || Math.Abs(d2a - 480) < 0.01 || Math.Abs(d2a - 500) < 0.01)
                    return "R max 1.2";
                return "";
            }
            return "R max 1.2";
        }

        private static string SumRitningsnr(int serie, int aes, double da, string bet)
        {
            if (serie == 22) return "7437361";
            if (serie == 240)
            {
                if (aes == 0) return da < 379 ? "7437370" : "7437371";
                return da < 379 ? "232631" : "232632";
            }
            if (serie == 241)
            {
                if (aes == 0) return da < 379 ? "7437372" : "7437373";
                return "232636";
            }
            if (serie == 30)
            {
                if (da < 529) return aes == 0 ? "7437362" : "234385";
                return "7437363";
            }
            // NOTE: serie 31 has NO da-split in this script (unlike Skepp6 class)
            if (serie == 31) return aes == 0 ? "7437364" : "231282";
            if (serie == 32) return aes == 0 ? "7437366" : "231638";
            return "Styckritning, samma som typ";
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

        // Direct [TmpX] / @Round — Swedish comma decimal (e.g. "228,5")
        private static string FmtComma(double v) =>
            v.ToString("0.################", CommonFunctions.Culture).Replace(".", ",");

        // @text("F3") / hardcoded dot strings — dot decimal
        private static string Fmt3(double v) =>
            v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
    }
}