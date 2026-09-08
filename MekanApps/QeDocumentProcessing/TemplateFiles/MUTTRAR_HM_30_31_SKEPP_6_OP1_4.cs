using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class MUTTRAR_HM_30_31_SKEPP_6_OP1_4 : ITemplateCalculations
    {
        private static readonly string[] Machines = new[] { "Skepp6/K&T", "VTR-160", "MacTurn 550" };
        private static readonly string[] MachinesNoSkepp = new[] { "VTR-160", "MacTurn 550" };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;
            List<Bookmark> bm = req != null ? req.Bookmarks : null;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            bool tmpSlash = tmpFormat.IndexOf('/') >= 0;
            bool tmpV21 = tmpFormat.IndexOf("V21", StringComparison.OrdinalIgnoreCase) >= 0;

            string[] tokens = tmpFormat.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : string.Empty;
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : string.Empty;
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : string.Empty;

            int cntB2 = tmpBet2.Length;
            string tmpTyp = tmpBet2.Length >= 2 ? tmpBet2.Substring(0, 2) : tmpBet2;   // serie
            string tmpTyp2 = tmpSlash ? tmpBet3                                           // type from Bet3 when slash
                             : (cntB2 > 3 ? (tmpBet2.Length >= 2 ? tmpBet2.Substring(tmpBet2.Length - 2) : tmpBet2) : tmpBet2);

            int serieInt = TryParseInt(tmpTyp);
            double typNum = TryParseDouble(tmpTyp2);

            // TmpTyp2Lista — 14 types
            string[] Typ2List = { "44", "48", "52", "56", "60", "64", "68", "72", "76", "80", "84", "88", "92", "96" };

            // ── Thread geometry (same as HM_30_31_44_1060) ──────────────────────

            // Lotus: Tmpd4 = if(Tmp/=false; TmpTyp2/2*10; TmpBet3)
            // Slash case: d4 IS the value in Bet3 (e.g. "670" -> 670 directly)
            // No-slash: d4 = TmpTyp2/2*10 (e.g. type "68" -> 340)
            double tmpd4 = tmpSlash ? TryParseDouble(tmpBet3) : typNum / 2.0 * 10.0;
            int stmm = Stmm(tmpd4);

            kv["SumP"] = "(P) " + stmm.ToString(CultureInfo.InvariantCulture);
            kv["SumGänga"] = "Tr " + Fmt(tmpd4) + "x" + stmm.ToString(CultureInfo.InvariantCulture);
            kv["SumGMått"] = kv["SumGänga"];
            kv["SumGMall"] = "Tr x " + stmm.ToString(CultureInfo.InvariantCulture);

            // Sumd4 — core diameter: no "(d4)" prefix in this script
            double sumd4val = (stmm == 4 || stmm == 5) ? tmpd4 + 0.5 : tmpd4 + 1.0;
            kv["Sumd4"] = Fmt(sumd4val);
            kv["Sumd4Tol"] = GenTol(sumd4val);

            double tmpdm = DM(tmpd4, stmm);
            kv["Sumdm"] = "(dm) " + Fmt(tmpdm);
            kv["SumdmTol"] = DmTolStr(tmpd4);
            kv["SumdmTolN"] = " 0";

            double tmpD1 = D1Val(tmpd4, stmm);
            double tmpD1SSK = tmpD1 - 1.0;
            kv["SumD1"] = "(D1) " + Fmt(tmpD1);
            kv["SumD1Tol"] = D1TolStr(tmpd4);
            kv["SumD1TolN"] = " 0";
            kv["SumD1SSK"] = "(D1) " + Fmt(tmpD1SSK);

            double tmpD = serieInt == 30 ? D_30(tmpd4) : D_31(tmpd4);
            kv["SumD"] = "(D) " + Fmt(tmpD);
            kv["SumDTol"] = "+ 0";
            kv["SumDTolN"] = H11TolN(tmpD);

            double tmpD2 = serieInt == 30 ? D2_30(tmpd4, tmpD) : D2_31(tmpd4, tmpD);
            double tmpD2SSK = tmpD2 - 1.0;
            kv["SumD2"] = "(D2) " + Fmt(tmpD2);
            kv["SumD2Tol"] = D2TolN(tmpD2);
            kv["SumD2TolN"] = " 0";

            double tmpD3 = tmpd4 < 501 ? tmpd4 + 2.0 : tmpd4 + 3.0;
            kv["SumD3"] = "(D3) " + Fmt(tmpD3);
            kv["SumD3Tol"] = D3TolStr(tmpD3);
            kv["SumD3TolN"] = " 0";
            kv["SumF"] = "5x45° (x2)";

            double tmpHM = (tmpD - tmpD2SSK) / 2.0;
            kv["SumHM"] = "(HM) " + Fmt(tmpHM).Replace(".",",");

            double tmpBval = serieInt == 30 ? B_30(tmpd4) : B_31(tmpd4);
            kv["SumB"] = "(B) " + Fmt(tmpBval);
            kv["SumBTol"] = "+ 0";
            kv["SumBTolN"] = BTolN(tmpBval);
            kv["SumBSSK"] = "(B) " + Fmt(tmpBval);   // TmpBSSK=TmpB (same value)

            kv["SumR"] = SumR(tmpd4);
            kv["SumR3b"] = kv["SumR"];

            string tmpKa = Ka(tmpd4);
            kv["SumPL"] = tmpKa;
            kv["SumK"] = tmpKa;

            kv["SumRa32"] = "3.2";
            kv["SumRa32a"] = "3.2";
            kv["SumRa63"] = "6.3";

            kv["SumF3"] = "30º"; kv["SumGF"] = "30º";
            kv["SumF1"] = "45º"; kv["SumF1S3"] = "45º";
            kv["SumF2"] = "45º"; kv["SumF2S3"] = "45º";
            kv["SumGV"] = "45º"; kv["SumRHT"] = "R1.6";

            // B2 (lyftöga position)
            double tmpBredd = serieInt == 30 ? B_30(tmpd4) : Bredd_31(tmpd4);
            double tmpB2 = tmpBredd / 2.0;
            bool showB2 = serieInt == 30 ? tmpd4 >= 421 : tmpd4 >= 321;
            kv["SumB2"] = showB2 ? Fmt(tmpB2) : "";

            // G (lyftögla) & L
            double tmpGG = serieInt == 30 ? GG_30(tmpd4) : GG_31(tmpd4);
            bool hasGG = (serieInt == 30 ? tmpd4 >= 421 : tmpd4 >= 321);
            kv["SumGG"] = hasGG ? "(G) M" + Fmt(tmpGG) : "";
            kv["SumL"] = hasGG ? LVal(serieInt, tmpGG, tmpd4) : "";
            kv["SumGG2"] = hasGG ? "Gänga: M" + Fmt(tmpGG) : "";
            kv["SumGGTolk"] = hasGG ? "Gängtolk  M" + Fmt(tmpGG) + " min/max" : "";

            // Spårdjup t & spårbredd S
            double tmpt = serieInt == 30 ? T_30(tmpd4) : T_31(tmpd4);
            kv["Sumt"] = "(t) " + Fmt(tmpt);
            kv["SumtTol"] = tmpt < 11 ? "+  1.5" : tmpt < 19 ? "+  1.8" : tmpt < 31 ? "+  2.1" : "+  2.5";
            kv["SumtTolN"] = " 0";

            double tmpS = serieInt == 30 ? S_30(tmpd4) : S_31(tmpd4);
            kv["SumS"] = "(S) " + Fmt(tmpS);
            kv["SumSTol"] = tmpS < 31 ? "± 0.260" : tmpS < 51 ? "± 0.310" : "± 0.370";

            string tmpLR = tmpS < 7 ? "0.5" : tmpS < 10 ? "0.75" : tmpS < 19 ? "1.0" : tmpS < 31 ? "1.25" : "1.5";
            kv["SumLR"] = tmpLR;

            // D5 (hål delning)
            double tmpD5 = serieInt == 30 ? D5_30(tmpd4, tmpt) : D5_31(tmpd4, tmpt);
            kv["SumD5"] = "(D4) " + Fmt(tmpD5);   // NOTE: Lotus output key says "(D4)" — not "(D5)"
            kv["SumD5Tol"] = "± " + Fmt3(GenTolVal(tmpD5));

            // Gu (gänga) & L2
            double tmpGu = serieInt == 30 ? Gu_30(tmpd4) : Gu_31(tmpd4);
            kv["SumGu"] = "(u) M" + Fmt(tmpGu);
            kv["SumGu2"] = "M" + Fmt(tmpGu);
            kv["SumGuTolk"] = "Gängtolk  M" + Fmt(tmpGu) + " min/max";

            double tmpL2 = serieInt == 30 ? L2_30(tmpGu, tmpd4) : L2_31(tmpGu, tmpd4);
            kv["SumL2"] = "(L2) " + Fmt(tmpL2);
            kv["SumL2Tol"] = "+ " + Fmt1(2.0);
            kv["SumL2TolN"] = "- 0";

            double tmpLd2 = Ld2(tmpGu);
            kv["SumLd2"] = "max: " + Fmt(tmpLd2);

            kv["SumStämpling"] = "Stämplas enl. ritning 7430189";

            // Machine
            SumMaskinVal(kv, maskinVal);
            SumFrequencies(kv, maskinVal);
            SumMeasuringDevices(kv, maskinVal, kv["SumGuTolk"], kv["SumGGTolk"]);
            SumAF(kv, maskinVal);

            // Cut data (Skepp6/K&T only — suppressed for VTR/MacTurn)
            bool isSkepp = EqualsI(maskinVal, "Skepp6/K&T");
            bool noSkepp = IsInGroup(maskinVal, MachinesNoSkepp);
            SumCutData(kv, bm, isSkepp, noSkepp);

            kv["SumTextS1"] = "Grader, frifläckar, slagmärken, repor, valkar och andra defekter samt ojämnheter kontrolleras med ögonmått, för övriga mått används skjutmått.";
            kv["SumTextS2"] = "Övriga mått kontrolleras vid inställning";
            kv["SumTextS3"] = "Vid misstänkt formfel lämnas muttern till mätrum";

            // Validation
            kv["VaLArtKontr"] = BuildVaLArt(tmpD, tmpV21, tmpBet1, serieInt);

            // Ritningar — no bookmark override in this script
            string rit = (serieInt == 30 ? "223015" : "223016") + ":senaste utgåva";
            kv["SumRitningsnr"] = rit;
            kv["SumRitningsnr2"] = rit;
            kv["SumRitningsnr3"] = rit;
            kv["SumTolRit"] = "1432008";
            kv["SumTolRit2"] = "1432008";
            kv["SumTolRit3"] = "1432008";
            kv["SumGTolRit"] = "7430181";
            kv["SumYtRit"] = "7430184";
            kv["SumYtRit2"] = "7430184";
            kv["SumYtRit3"] = "7430184";
            kv["SumKlEgenskaper"] = "PRODUCTION NUTS & SLEEVES & HOUSINGS\\ALLMÄNKLASSADE EGENSKAPER\\Klassade egenskaper muttrar";
            kv["SumKlEgenskaper2"] = kv["SumKlEgenskaper"];
            kv["SumKlEgenskaper3"] = kv["SumKlEgenskaper"];

            return kv;
        }

        // ── Popup ─────────────────────────────────────────────────────────────────
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
                       " dagar)<<LineBreak>>Information om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument<<LineBreak>>Popupruta aktiv till " +
                       validTill.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return "";
        }

        // ── Machine labels ────────────────────────────────────────────────────────
        private static void SumMaskinVal(Dictionary<string, string> kv, string mv)
        {
            string s1 = EqualsI(mv, "Skepp6/K&T") ? "Morando" : EqualsI(mv, "VTR-160") ? "VTR-160" : EqualsI(mv, "MacTurn 550") ? "MacTurn 550" : "";
            string s2 = EqualsI(mv, "Skepp6/K&T") ? "K&T" : EqualsI(mv, "VTR-160") ? "VTR-160" : EqualsI(mv, "MacTurn 550") ? "MacTurn 550" : "";
            string s3 = EqualsI(mv, "Skepp6/K&T") ? "1150" : EqualsI(mv, "VTR-160") ? "VTR-160" : EqualsI(mv, "MacTurn 550") ? "MacTurn 550" : "";
            kv["SumMaskinValS1"] = "Maskin: " + s1 + " - OP1 & 2";
            kv["SumMaskinValS2"] = "Maskin: " + s2 + " - OP3";
            kv["SumMaskinValS3"] = "Maskin: " + s3 + " - OP4";
        }

        // ── Frequencies (3 pages, all "1/1") ─────────────────────────────────────
        private static void SumFrequencies(Dictionary<string, string> kv, string mv)
        {
            bool m = IsMachine(mv);
            for (int i = 1; i <= 8; i++) kv["SumF1_" + i] = m ? "1/1" : "";
            for (int i = 1; i <= 4; i++) kv["SumF2_" + i] = m ? "1/1" : "";
            for (int i = 1; i <= 9; i++) kv["SumF3_" + i] = m ? "1/1" : "";
            kv["SumF3_0"] = m ? "1/1" : "";
        }

        // ── Measuring devices (3 pages) ───────────────────────────────────────────
        private static void SumMeasuringDevices(Dictionary<string, string> kv, string mv, string guTolk, string ggTolk)
        {
            bool m = IsMachine(mv);
            kv["SumD1_1"] = m ? "Skjutmått" : "";
            kv["SumD1_2"] = m ? "Mikrometer" : "";
            kv["SumD1_3"] = m ? "Djupmått/Skjutmått" : "";
            kv["SumD1_4"] = m ? "Fasmall/Skjutmått" : "";
            kv["SumD1_5"] = m ? "Djupmått/Skjutmått" : "";
            kv["SumD1_6"] = m ? "Fasmall/Skjutmått" : "";
            kv["SumD1_7"] = m ? "Radiestål" : "";
            kv["SumD1_8"] = m ? "Ytjämnhetsmätare" : "";
            kv["SumD2_1"] = m ? "Skjutmått" : "";
            kv["SumD2_2"] = m ? "Djupmått" : "";
            kv["SumD2_3"] = m ? guTolk : "";
            kv["SumD2_4"] = m ? ggTolk : "";
            kv["SumD3_1"] = m ? "Multimar" : "";
            kv["SumD3_2"] = m ? "Mikrometerstickmått" : "";
            kv["SumD3_3"] = m ? "Skjutmått" : "";
            kv["SumD3_4"] = m ? "Skjutmått" : "";
            kv["SumD3_5"] = m ? "Skjutmått" : "";
            kv["SumD3_6"] = m ? "Fasmall" : "";
            kv["SumD3_7"] = m ? "Gängmall" : "";
            kv["SumD3_8"] = m ? "Ytjämnhetsmätare" : "";
            kv["SumD3_9"] = "";
            kv["SumD3_0"] = m ? "Egglinjal" : "";
        }

        // ── AF (3 pages) ──────────────────────────────────────────────────────────
        private static void SumAF(Dictionary<string, string> kv, string mv)
        {
            bool m = IsMachine(mv);
            kv["SumAF1_1"] = "";
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = m ? "Mall ind.nr. 8520" : "";
            kv["SumAF1_5"] = "";
            kv["SumAF1_6"] = "";
            kv["SumAF1_7"] = "";
            kv["SumAF1_8"] = m ? "Övriga Ra värden 6,3" : "";
            kv["SumAF2_1"] = ""; kv["SumAF2_2"] = ""; kv["SumAF2_3"] = ""; kv["SumAF2_4"] = "";
            kv["SumAF3_1"] = m ? "Kontrolleras med passbitsklove utf. 1" : "";
            kv["SumAF3_2"] = ""; kv["SumAF3_3"] = ""; kv["SumAF3_4"] = "";
            kv["SumAF3_5"] = ""; kv["SumAF3_6"] = ""; kv["SumAF3_7"] = "";
            kv["SumAF3_8"] = "";
            kv["SumAF3_9"] = m ? "Körs i samma uppspänning" : "";
            kv["SumAF3_0"] = "";
        }

        // ── Cut data (Skepp6/K&T only — blank for VTR/MacTurn) ───────────────────
        private static void SumCutData(Dictionary<string, string> kv, List<Bookmark> bm, bool isSkepp, bool noSkepp)
        {
            // All cut data is blank when VTR/MacTurn or bookmark = "0"
            kv["SumChuckback"] = GetCutVal(bm, "Chuckbackar", noSkepp, "");
            kv["SumCB"] = GetCutLabel(bm, "Chuckbackar", noSkepp, "Chuckbackar:");
            kv["SumStödback"] = GetCutVal(bm, "Stödbackar", noSkepp, " mm");
            kv["SumSB"] = GetCutLabel(bm, "Stödbackar", noSkepp, "Stödbackar:");
            kv["SumGrader"] = GetCutVal(bm, "Grader", noSkepp, " mm");
            kv["SumGR"] = GetCutLabel(bm, "Grader", noSkepp, "Grader:");
            kv["SumVarv"] = GetCutVal(bm, "Varvtal", noSkepp, " /min");
            kv["SumVR"] = GetCutLabel(bm, "Varvtal", noSkepp, "Varvtal:");
            kv["SumMatPl"] = GetCutVal(bm, "Matning Plan", noSkepp, " /min");
            kv["SumMP"] = GetCutLabel(bm, "Matning Plan", noSkepp, "Matning Plan:");
            kv["SumMatInUt"] = GetCutVal(bm, "Matning Utv/Inv", noSkepp, " /min");
            kv["SumMIU"] = GetCutLabel(bm, "Matning Utv/Inv", noSkepp, "Matning Utv/Inv:");
            kv["SumFMUtv"] = GetCutVal(bm, "Färdigmått Utv", noSkepp, " mm");
            kv["SumFMU"] = GetCutLabel(bm, "Färdigmått Utv", noSkepp, "Utvändig diameter:");
            kv["SumUtvLin"] = GetCutVal(bm, "Linjal Utv", noSkepp, " mm");
            kv["SumUL"] = GetCutLabel(bm, "Linjal Utv", noSkepp, "Motsvarar på linjal:");
            kv["SumFMInv"] = GetCutVal(bm, "Färdigmått Inv", noSkepp, " mm");
            kv["SumFMI"] = GetCutLabel(bm, "Färdigmått Inv", noSkepp, "Invändig diameter:");
            kv["SumInvLin"] = GetCutVal(bm, "Linjal Inv", noSkepp, " mm");
            kv["SumIL"] = GetCutLabel(bm, "Linjal Inv", noSkepp, "Motsvarar på linjal:");
            kv["SumFM"] = "Färdigmått";
            kv["SumIN"] = "Inställning (Grov / Finskär)";
            // SumIN and SumFM omitted — these require checking all cut data values simultaneously;
            // they depend on the Lotus formula checking multiple bookmarks for non-zero state
        }

        private static string GetCutVal(List<Bookmark> bm, string key, bool noSkepp, string suffix)
        {
            if (noSkepp) return "";
            string v = GetString(bm, key);
            if (string.IsNullOrEmpty(v) || EqualsI(v, "0")) return "";
            return v + suffix;
        }

        private static string GetCutLabel(List<Bookmark> bm, string key, bool noSkepp, string label)
        {
            if (noSkepp) return "";
            string v = GetString(bm, key);
            if (string.IsNullOrEmpty(v) || EqualsI(v, "0")) return "";
            return label;
        }

        // ── Validation ────────────────────────────────────────────────────────────
        private static string BuildVaLArt(double tmpD, bool v21, string bet1, int serie)
        {
            if (tmpD > 750)
            {
                if (!v21)
                {
                    bool isHM30or31 = EqualsI(bet1, "HM") && (serie == 30 || serie == 31);
                    if (!isHM30or31)
                        return "FEL MALL - Denna mall gäller BARA HM 30/31\n\n---------> Kontrollera inmatningsfält <---------";
                    return "";
                }
                return "FEL MALL - Använd SPECIAL MALLEN";
            }
            return "FEL MALL - Använd HM30-31 0-630 OP1-3";
        }

        // ── Dimension helpers (identical to HM_30_31_44_1060) ────────────────────

        private static int Stmm(double d4)
        {
            if (d4 < 301) return 4; if (d4 < 501) return 5; if (d4 < 701) return 6;
            if (d4 < 901) return 7; return 8;
        }

        private static double DM(double d4, int s)
        {
            if (s == 4) return d4 - 2; if (s == 5) return d4 - 2.5; if (s == 6) return d4 - 3;
            if (s == 7) return d4 - 3.5; return d4 - 4;
        }

        private static double D1Val(double d4, int s)
        {
            if (s == 4) return d4 - 4; if (s == 5) return d4 - 5; if (s == 6) return d4 - 6;
            if (s == 7) return d4 - 7; return d4 - 8;
        }

        private static double D_30(double d4)
        {
            if (d4 < 221) return d4 + 40; if (d4 < 281) return d4 + 50; if (d4 < 361) return d4 + 60;
            if (d4 < 421) return d4 + 70; if (d4 < 501) return d4 + 80; if (Math.Abs(d4 - 560) < 0.001) return d4 + 90;
            if (d4 < 631) return d4 + 100; if (d4 < 671) return d4 + 110; if (d4 < 801) return d4 + 120;
            if (d4 < 951) return d4 + 130; return d4 + 140;
        }

        private static double D_31(double d4)
        {
            if (Math.Abs(d4 - 350) < 0.001) return d4 + 90; if (d4 < 241) return d4 + 60; if (d4 < 281) return d4 + 70;
            if (d4 < 321) return d4 + 80; if (d4 < 361) return d4 + 100; if (d4 < 381) return d4 + 110; if (d4 < 461) return d4 + 120;
            if (d4 < 481) return d4 + 140; if (d4 < 501) return d4 + 130; if (d4 < 531) return d4 + 140; if (d4 < 601) return d4 + 150;
            if (d4 < 631) return d4 + 170; if (d4 < 671) return d4 + 180; if (d4 < 711) return d4 + 190; if (d4 < 801) return d4 + 200;
            if (d4 < 851) return d4 + 210; if (d4 < 951) return d4 + 220; return d4 + 240;
        }

        private static double D2_30(double d4, double D)
        {
            if (d4 < 221) return D - 18; if (d4 < 281) return D - 20; if (d4 < 341) return D - 24; if (d4 < 361) return D - 26;
            if (d4 < 421) return D - 28; if (d4 < 501) return D - 30; if (d4 < 671) return D - 40; if (d4 < 801) return D - 50; return D - 55;
        }

        private static double D2_31(double d4, double D)
        {
            if (d4 < 281) return D - 30; if (d4 < 361) return D - 40; if (d4 < 381) return D - 50; if (d4 < 401) return D - 60;
            if (d4 < 441) return D - 50; if (d4 < 461) return D - 40; if (d4 < 481) return D - 60; if (d4 < 501) return D - 50;
            if (d4 < 601) return D - 60; if (d4 < 631) return D - 70; if (d4 < 801) return D - 75; if (d4 < 851) return D - 85;
            if (d4 < 951) return D - 90; if (d4 < 1001) return D - 100; return D - 90;
        }

        private static double B_30(double d4)
        {
            if (d4 < 221) return 30; if (d4 < 261) return 34; if (d4 < 281) return 38; if (d4 < 321) return 42;
            if (d4 < 361) return 45; if (d4 < 381) return 48; if (d4 < 421) return 52; if (d4 < 481) return 60;
            if (d4 < 531) return 68; if (d4 < 631) return 75; if (d4 < 671) return 80; if (d4 < 851) return 90;
            if (d4 < 1181) return 100; return 110;
        }

        private static double B_31(double d4)
        {
            if (d4 < 221) return 32; if (d4 < 241) return 34; if (d4 < 261) return 36; if (d4 < 281) return 38;
            if (d4 < 301) return 40; if (d4 < 321) return 42; if (d4 < 351) return 55; if (d4 < 361) return 58;
            if (d4 < 381) return 60; if (d4 < 401) return 62; if (d4 < 441) return 70; if (d4 < 481) return 75;
            if (d4 < 531) return 80; if (d4 < 601) return 85; if (d4 < 631) return 95; if (d4 < 711) return 106;
            if (d4 < 801) return 112; if (d4 < 851) return 118; return 125;
        }

        private static double Bredd_31(double d4)
        {
            if (d4 < 221) return 32; if (d4 < 241) return 34; if (d4 < 261) return 36; if (d4 < 281) return 38;
            if (d4 < 301) return 40; if (d4 < 321) return 42; if (d4 < 341) return 55; if (d4 < 361) return 58;
            if (d4 < 381) return 60; if (d4 < 401) return 62; if (d4 < 441) return 70; if (d4 < 481) return 75;
            if (d4 < 531) return 80; if (d4 < 601) return 85; if (d4 < 631) return 95; if (d4 < 711) return 106;
            if (d4 < 801) return 112; if (d4 < 851) return 118; return 125;
        }

        private static string SumR(double d4)
        {
            if (d4 < 221) return "R 2.5"; if (d4 < 281) return "R 3"; if (d4 < 441) return "R 3.5";
            if (d4 < 601) return "R 4"; if (d4 < 711) return "R 5"; return "R 6";
        }

        private static string Ka(double d4)
        {
            if (d4 < 51) return "0.04"; if (d4 < 121) return "0.05"; if (d4 < 251) return "0.06";
            if (d4 < 316) return "0.07"; if (d4 < 401) return "0.08"; if (d4 < 501) return "0.09";
            if (d4 < 631) return "0.10"; if (d4 < 801) return "0.12"; if (d4 < 1001) return "0.14"; return "0.16";
        }

        private static double T_30(double d4)
        {
            if (d4 < 221) return 9; if (d4 < 281) return 10; if (d4 < 341) return 12; if (d4 < 361) return 13;
            if (d4 < 421) return 14; if (d4 < 501) return 15; if (d4 < 671) return 20; return 25;
        }

        private static double T_31(double d4)
        {
            if (d4 < 241) return 10; if (d4 < 321) return 12; if (d4 < 361) return 15; if (d4 < 421) return 18;
            if (d4 < 481) return 20; if (d4 < 531) return 23; if (d4 < 601) return 25; if (d4 < 671) return 28;
            if (d4 < 711) return 30; if (d4 < 801) return 34; return 38;
        }

        private static double S_30(double d4)
        {
            if (d4 < 261) return 20; if (d4 < 341) return 24; if (d4 < 401) return 28; if (d4 < 461) return 32;
            if (d4 < 501) return 36; if (d4 < 601) return 40; if (d4 < 671) return 45; if (d4 < 711) return 50;
            if (d4 < 801) return 55; return 60;
        }

        private static double S_31(double d4)
        {
            if (d4 < 261) return 20; if (d4 < 321) return 24; if (d4 < 361) return 28; if (d4 < 421) return 32;
            if (d4 < 481) return 36; if (d4 < 531) return 40; if (d4 < 601) return 45; if (d4 < 671) return 50;
            if (d4 < 711) return 55; if (d4 < 801) return 60; return 70;
        }

        private static double D5_30(double d4, double t)
        {
            if (Math.Abs(t - 9) < 0.001) return d4 + 9; if (Math.Abs(t - 10) < 0.001) return d4 + 13;
            if (Math.Abs(t - 12) < 0.001) return d4 + 16; if (Math.Abs(t - 13) < 0.001) return d4 + 15;
            if (Math.Abs(t - 14) < 0.001) return d4 + 19; if (Math.Abs(t - 15) < 0.001) return d4 + 23;
            if (Math.Abs(t - 20) < 0.001) { if (d4 < 531) return d4 + 28; if (d4 < 561) return d4 + 23; if (d4 < 631) return d4 + 28; return d4 + 33; }
            if (d4 < 801) return d4 + 32; if (d4 < 901) return d4 + 37; if (d4 < 951) return d4 + 35; return d4 + 40;
        }

        private static double D5_31(double d4, double t)
        {
            if (Math.Abs(t - 10) < 0.001) return d4 + 18;
            if (Math.Abs(t - 12) < 0.001) return d4 < 281 ? d4 + 21 : d4 + 26;
            if (Math.Abs(t - 15) < 0.001) return d4 + 33;
            if (Math.Abs(t - 18) < 0.001) return d4 < 381 ? d4 + 35 : d4 + 40;
            if (Math.Abs(t - 20) < 0.001) return d4 < 461 ? d4 + 38 : d4 + 48;
            if (Math.Abs(t - 23) < 0.001) return d4 < 501 ? d4 + 40 : d4 + 45;
            if (Math.Abs(t - 25) < 0.001) return d4 + 48;
            if (Math.Abs(t - 28) < 0.001) return d4 < 631 ? d4 + 55 : d4 + 60;
            if (Math.Abs(t - 30) < 0.001) return d4 + 62;
            if (Math.Abs(t - 34) < 0.001) return d4 + 63;
            if (d4 < 851) return d4 + 64; if (d4 < 901) return d4 + 69; if (d4 < 951) return d4 + 67; return d4 + 77;
        }

        private static double GG_30(double d4) { if (d4 < 671) return 10; if (d4 < 851) return 12; return 16; }
        private static double GG_31(double d4) { if (d4 < 531) return 10; if (d4 < 601) return 12; if (d4 < 751) return 16; if (d4 < 901) return 20; return 24; }

        private static string LVal(int serie, double gg, double d4)
        {
            if (serie == 30)
            {
                if (Math.Abs(gg - 10) < 0.001) return "17"; if (Math.Abs(gg - 12) < 0.001) return "21"; return "27";
            }
            if (Math.Abs(gg - 10) < 0.001) return "17"; if (Math.Abs(gg - 12) < 0.001) return "21";
            if (Math.Abs(gg - 16) < 0.001) return "27"; if (Math.Abs(gg - 20) < 0.001) return "30"; return "36";
        }

        private static double Gu_30(double d4) { if (d4 < 221) return 6; if (d4 < 361) return 8; if (d4 < 421) return 10; if (d4 < 501) return 12; if (d4 < 801) return 16; return 20; }
        private static double Gu_31(double d4) { if (d4 < 241) return 8; if (d4 < 321) return 10; if (d4 < 381) return 12; if (d4 < 501) return 16; if (d4 < 671) return 20; return 24; }

        private static double L2_30(double gu, double d4) { if (Math.Abs(gu - 6) < 0.001) return 12; if (Math.Abs(gu - 8) < 0.001) return d4 < 301 ? 17 : 16; if (Math.Abs(gu - 10) < 0.001) return 20; if (Math.Abs(gu - 12) < 0.001) return 23; if (Math.Abs(gu - 16) < 0.001) return 26; return 37; }
        private static double L2_31(double gu, double d4) { if (Math.Abs(gu - 8) < 0.001) return 17; if (Math.Abs(gu - 10) < 0.001) return d4 < 301 ? 21 : 20; if (Math.Abs(gu - 12) < 0.001) return 23; if (Math.Abs(gu - 16) < 0.001) return 28; if (Math.Abs(gu - 20) < 0.001) return 37; return 47; }

        private static double Ld2(double gu)
        {
            if (Math.Abs(gu - 8) < 0.001) return 23; if (Math.Abs(gu - 10) < 0.001) return 27.5;
            if (Math.Abs(gu - 12) < 0.001) return 30; if (Math.Abs(gu - 16) < 0.001) return 36;
            if (Math.Abs(gu - 20) < 0.001) return 47; return 58;
        }

        // ── Tolerance helpers ─────────────────────────────────────────────────────

        private static string GenTol(double v)
        {
            if (v < 6) return "± 0.1"; if (v < 30) return "± 0.2"; if (v < 120) return "± 0.3";
            if (v < 400) return "± 0.5"; if (v < 1000) return "± 0.8"; if (v < 2000) return "± 1.2"; return "± 2.0";
        }

        private static double GenTolVal(double v)
        {
            if (v < 6.01) return 0.1; if (v < 30.01) return 0.2; if (v < 120.01) return 0.3;
            if (v < 400.01) return 0.5; if (v < 1000.01) return 0.8; if (v < 2000.01) return 1.2; return 2.0;
        }

        private static string DmTolStr(double d4)
        {
            if (d4 < 301) return "+ 0.475"; if (d4 < 501) return "+ 0.530";
            if (d4 < 701) return "+ 0.600"; if (d4 < 901) return "+ 0.630"; return "+ 0.710";
        }

        private static string D1TolStr(double d4)
        {
            if (d4 < 301) return "+ 0.375"; if (d4 < 501) return "+ 0.450";
            if (d4 < 701) return "+ 0.500"; if (d4 < 901) return "+ 0.560"; return "+ 0.630";
        }

        private static string H11TolN(double v)
        {
            if (v < 3.01) return "- 0.060"; if (v < 6.01) return "- 0.075"; if (v < 10.01) return "- 0.090";
            if (v < 18.01) return "- 0.110"; if (v < 30.01) return "- 0.130"; if (v < 50.01) return "- 0.160";
            if (v < 80.01) return "- 0.190"; if (v < 120.01) return "- 0.220"; if (v < 180.01) return "- 0.250";
            if (v < 250.01) return "- 0.290"; if (v < 315.01) return "- 0.320"; if (v < 400.01) return "- 0.360";
            if (v < 500.01) return "- 0.400"; if (v < 630.01) return "- 0.440"; if (v < 800.01) return "- 0.500";
            if (v < 1000.01) return "- 0.560"; if (v < 1250.01) return "- 0.660"; if (v < 1600.01) return "- 0.780";
            if (v < 2000.01) return "- 0.920"; if (v < 2500.01) return "- 1.100"; return "- 1.350";
        }

        private static string D2TolN(double v)
        {
            if (v < 19) return "- 0.270"; if (v < 31) return "- 0.330"; if (v < 51) return "- 0.390";
            if (v < 81) return "- 0.460"; if (v < 121) return "- 0.540"; if (v < 181) return "- 0.630";
            if (v < 251) return "- 0.720"; if (v < 316) return "- 0.810"; if (v < 401) return "- 0.890";
            if (v < 501) return "- 0.970"; if (v < 631) return "- 1.100"; if (v < 801) return "- 1.250";
            if (v < 1000) return "- 1.400"; return "- 1.650";
        }

        private static string D3TolStr(double v)
        {
            if (v < 19) return "+ 0.430"; if (v < 31) return "+ 0.520"; if (v < 51) return "+ 0.620";
            if (v < 81) return "+ 0.740"; if (v < 121) return "+ 0.870"; if (v < 181) return "+ 1.000";
            if (v < 251) return "+ 1.150"; if (v < 316) return "+ 1.300"; if (v < 401) return "+ 1.400";
            if (v < 501) return "+ 1.550"; if (v < 631) return "+ 1.750"; if (v < 801) return "+ 2.000";
            if (v < 1000) return "+ 2.300"; return "+ 2.600";
        }

        private static string BTolN(double b)
        {
            if (b < 7) return "- 0.180"; if (b < 11) return "- 0.220"; if (b < 19) return "- 0.270";
            if (b < 31) return "- 0.330"; if (b < 51) return "- 0.390"; if (b < 81) return "- 0.460";
            if (b < 121) return "- 0.540"; return "- 0.630";
        }

        // ── Utilities ─────────────────────────────────────────────────────────────
        private static bool IsMachine(string mv) { if (string.IsNullOrEmpty(mv)) return false; for (int i = 0; i < Machines.Length; i++) if (string.Equals(Machines[i], mv, StringComparison.OrdinalIgnoreCase)) return true; return false; }
        private static bool IsInGroup(string mv, string[] g) { if (string.IsNullOrEmpty(mv)) return false; for (int i = 0; i < g.Length; i++) if (string.Equals(g[i], mv, StringComparison.OrdinalIgnoreCase)) return true; return false; }
        private static bool EqualsI(string a, string b) => string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);
        private static int TryParseInt(string s) { int v; return int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0; }
        private static double TryParseDouble(string s) { if (string.IsNullOrWhiteSpace(s)) return 0; double v; return double.TryParse(s.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0; }
        private static string GetString(List<Bookmark> bm, string key) { if (bm == null || string.IsNullOrWhiteSpace(key)) return ""; for (int i = 0; i < bm.Count; i++) { Bookmark b = bm[i]; if (b != null && string.Equals(b.BookmarkName ?? "", key, StringComparison.OrdinalIgnoreCase)) return b.BookmarkValue ?? ""; } return ""; }
        private static string Fmt(double v) => v.ToString(CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt1(double v) => v.ToString("F1", CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt3(double v) => v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
    }
}