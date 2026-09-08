using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Klamhylsor_4_13_E : ITemplateCalculations
    {
        private static readonly string[] ArtList = { "H", "HA", "HE", "HS" };
        private static readonly string[] SerieList = { "2", "3", "23" };
        private static readonly string[] TypListNormal = { "04", "05", "06", "07", "08", "09", "10", "11", "12", "13" };

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
            bool tmpVZ = tmpBet.IndexOf("VZ", StringComparison.OrdinalIgnoreCase) >= 0;
            bool tmpV21 = tmpBet.IndexOf("V21", StringComparison.OrdinalIgnoreCase) >= 0;
            // No TmpJ in this class

            // Explode " /.-"
            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : "";
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : "";
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : "";

            int cntB2 = tmpBet2.Length;
            string tmpArt = tmpBet1;

            // TmpSerie
            string tmpSerie = cntB2 == 3
                ? (tmpBet2.Length >= 1 ? tmpBet2.Substring(0, 1) : "")
                : (tmpBet2.Length >= 2 ? tmpBet2.Substring(0, 2) : tmpBet2);
            int serieInt = TryParseInt(tmpSerie);

            // TmpTyp = Right(Bet2, 2) both branches
            string tmpTyp = tmpBet2.Length >= 2 ? tmpBet2.Substring(tmpBet2.Length - 2) : tmpBet2;
            int typInt = TryParseInt(tmpTyp);

            // TmpTypLista (1-based, for reference — not directly needed for outputs here)
            int typLista = GetMember(tmpTyp, TypListNormal);

            // Bookmarks
            double gYDia = GetDouble(bm, "GYDia (d)");
            double tmpd = gYDia != 0 ? gYDia : (typInt / 2.0) * 10.0;
            double tmpd1 = GetDouble(bm, "IDia (d1)");
            double tmpd2 = GetDouble(bm, "YDia (d2)");
            double tmpb = GetDouble(bm, "Gänglängd");
            double tmpL = GetDouble(bm, "Längd");
            double kona = GetDouble(bm, "Kona");
            double tudelad = GetDouble(bm, "Tudelad");
            double amatt = GetDouble(bm, "Amått");

            double tmpStmm = Stmm(tmpd);
            double tmpdm = DM(tmpd, tmpStmm);

            // TmpMV
            string tmpMV = EqualsI(maskinVal, "Okuma 2SP 4631") ? "4631"
                         : EqualsI(maskinVal, "Okuma 2SP 4669") ? "4669" : "";
            bool isHKA = tmpMV == "4631" || tmpMV == "4669";

            // TmpML
            double tmpML = tmpL - tmpb - 6.0;
            double tmpVT = VT(tmpd);
            double tmpKA = Math.Round(tmpML * tmpVT / 1000.0, 3);

            // ── SumRitPr — formula computed from serie + art ──────────────────────
            string basePrd = serieInt == 2 ? "7438950" : serieInt == 3 ? "7438951" : serieInt == 23 ? "7438952" : "";
            string artSuffix = EqualsI(tmpArt, "HA") ? ", 7438960"
                             : EqualsI(tmpArt, "HE") ? ", 7438961"
                             : EqualsI(tmpArt, "HS") ? ", 7438962" : "";
            string sumRitPr = basePrd + artSuffix + ":senaste utg.";
            kv["SumRitPr"] = sumRitPr;

            // TmpBricka: SumRitPr contains "8960"/"8961"/"8962" (for HA/HE/HS)
            // or "J425"/"j425" (unlikely here but kept per script)
            bool tmpBricka = sumRitPr.IndexOf("J425", StringComparison.OrdinalIgnoreCase) >= 0
                          || sumRitPr.IndexOf("8960") >= 0
                          || sumRitPr.IndexOf("8961") >= 0
                          || sumRitPr.IndexOf("8962") >= 0;

            // SumBricka: Bet3 special values for certain washer numbers
            string sumBricka = "";
            if (tmpBricka || Math.Abs(tudelad - 1) < 0.001)
            {
                if (EqualsI(tmpBet3, "38")) sumBricka = "Nr: 9827";
                else if (EqualsI(tmpBet3, "35")) sumBricka = "Nr: 6039";
                else if (EqualsI(tmpBet3, "30")) sumBricka = "Nr: 1167";
                else if (amatt != 0) sumBricka = "A-mått: " + FmtComma(amatt) + "mm.";
                // else sumBricka stays ""
            }
            kv["SumBricka"] = sumBricka;

            // ── VaLML, SumML, SumKA, SumKona ─────────────────────────────────────
            kv["VaLML"] = "Konavikelse kan avvika vid stämpelyta, vid behov ändra mätlängd. Kom ihåg att ändra mätlängd i Comgage också!";
            kv["SumML"] = ((int)tmpML).ToString(CultureInfo.InvariantCulture);
            kv["SumKona"] = "Kona 1:" + Fmt(kona);
            kv["SumKA"] = "max " + Fmt3(tmpKA) + " [2F]";
            if(subject == "HA 313 E")
                kv["SumKA"] = "max 0.013 [2F]";
            if (subject == "H 311 E")
                kv["SumKA"] = "max 0.011 [2F]";
            if (subject == "H 313 E")
                kv["SumKA"] = "max 0.013 [2F]";
            // ── Ra ────────────────────────────────────────────────────────────────
            kv["SumRa"] = "1.6 [2F]";   
            kv["SumRa1"] = "1.6 [2F]";

            // ── Faser & Radier ────────────────────────────────────────────────────
            kv["SumFas"] = FasMM(tmpd) + "x45°";
            kv["SumFT"] = ""; kv["SumFTN"] = "";
            kv["SumFasK"] = ""; kv["SumFKT"] = ""; kv["SumFKTN"] = "";
            kv["SumR1"] = "R0.5"; kv["SumR1T"] = ""; kv["SumR1TN"] = "";
            kv["SumR2"] = "R0.5"; kv["SumR2T"] = ""; kv["SumR2TN"] = "";

            // ── SumP — simple, no Tmpdma bookmark ────────────────────────────────
            kv["SumP"] = "(P) " + FmtStmm(tmpStmm).Replace(",",".");

            // ── d ────────────────────────────────────────────────────────────────
            kv["Sumd"] = "(d) " + Fmt(tmpd);
            kv["Sumda"] = kv["Sumd"];
            kv["SumdTol"] = "- " + Fmt3(DTolPos(tmpStmm));
            kv["SumdTolN"] = "- " + Fmt3(DTolNeg(tmpStmm));
            kv["SumdaTol"] = kv["SumdTol"];
            kv["SumdaTolN"] = kv["SumdTolN"];

            // ── dm ───────────────────────────────────────────────────────────────
            kv["Sumdm"] = "(dm) " + Fmt(tmpdm).Replace(".", ",");
            kv["SumdmTol"] = "- " + Fmt3(DTolPos(tmpStmm)) + " [3F]";
            kv["SumdmTolN"] = "- " + Fmt3(DmTolNeg(tmpd, tmpStmm)) + " [3F]";

            // ── b ────────────────────────────────────────────────────────────────
            kv["Sumb"] = "(b) " + Fmt(tmpb).Replace(".", ",");
            kv["SumbTol"] = "+ " + Fmt3(BTol(tmpb)) + " [3F]";
            kv["SumbTolN"] = "- " + Fmt3(0.0) + " [2F]";

            // ── Godstjocklek ──────────────────────────────────────────────────────
            kv["SumGodstjocklekTol"] = "+ " + Fmt3(GTolPos(tmpd, kona)) + " [3F]";
            kv["SumGodstjocklekTolN"] = "- " + Fmt3(GTolNeg(tmpd, kona)) + " [2F]";

            // ── d2 ───────────────────────────────────────────────────────────────
            kv["Sumd2"] = "(d2) " + Fmt(tmpd2).Replace(".", ",");
            kv["Sumd2Tol"] = "± " + Fmt3(GenTolVal(tmpd2));

            // ── d1 ───────────────────────────────────────────────────────────────
            kv["Sumd1"] = "(d1) " + Fmt(tmpd1).Replace(".", ",");
            kv["Sumd1Tol"] = "± " + Fmt3(D1Tol(tmpd1)) + " [3F]";
            kv["Sumd1STol"] = "+ " + Fmt3(D1STolPos(tmpd)) + " [3F]";
            kv["Sumd1STolN"] = "- " + Fmt3(D1STolNeg(tmpd)) + " [3F]";

            // ── Gängbeteckningar — no bookmark overrides in this class ────────────
            string sumGängbet = "M" + Fmt(tmpd);
            string sumStigning = FmtStmm(tmpStmm);
            string sumGxS = sumGängbet + "x" + sumStigning;
            string sumRullar = "M" + sumStigning;
            kv["SumGängbet"] = sumGängbet;
            kv["SumStigning"] = sumStigning;
            kv["SumGxS"] = sumGxS;
            kv["SumGängring"] = sumGxS;   // no bookmark override
            kv["SumGängtolk"] = sumGxS;   // no bookmark override
            kv["SumRullar"] = sumRullar;
            kv["SumGängmall"] = sumRullar; // no bookmark override

            // ── L ────────────────────────────────────────────────────────────────
            kv["SumL"] = "(L) " + Fmt(tmpL);
            kv["SumLTol"] = "+ " + Fmt3(0.0);
            kv["SumLTolN"] = "- " + Fmt3(LTolNeg(tmpL, tudelad)) + " [3F]";

            // ── Orundhet ──────────────────────────────────────────────────────────
            kv["SumRd"] = Fmt3(D1Tol(tmpd1));

            // ── Rakhet ────────────────────────────────────────────────────────────
            kv["SumSA"] = Fmt3(RakA(tmpd)).Replace(".",",");
            kv["SumSB"] = Fmt3(RakB(tmpd)).Replace(".", ",");

            // ── VE ───────────────────────────────────────────────────────────────
            kv["SumVE"] = "max: " + VE(tmpd) + " [2F]";

            // ── Slits (c) — identical to Special ─────────────────────────────────
            double tmpc = typInt < 5 ? 1.5 : typInt < 14 ? 2.0 : 2.5;
            kv["Sumc"] = "(c) " + Fmt(tmpc);
            kv["SumcTol"] = "± " + Fmt3(0.1);

            // ── Tmpf (typ-based, NOT bookmark/d-based like Special) ────────────────
            double tmpf = TungF_typ(typInt);
            kv["Sumf"] = "(f) " + Fmt(tmpf);
            kv["SumfTol"] = "+ " + Fmt3(FTol(tmpf));
            kv["SumfTolN"] = "- " + Fmt3(0.0) + " [3F]";

            // ── Tmpe (derived from f, NOT from d like Special) ─────────────────────
            double tmpe = TungE_f(tmpf);
            kv["Sume"] = "(e) " + Fmt(tmpe);
            kv["SumeTol"] = "+ " + Fmt3(ETol(tmpe));
            kv["SumeTolN"] = "- " + Fmt3(0.0) + " [3F]";

            // ── Delad hylsa ───────────────────────────────────────────────────────
            kv["SumDelHylFrBr"] = Math.Abs(tudelad) < 0.001 ? "" : "Fräsbredd delad hylsa 2mm";

            // ── Machine ───────────────────────────────────────────────────────────
            string s1 = isHKA ? maskinVal : "";
            kv["SumMaskinValS1"] = "Maskin: " + s1;

            SumFrequencies(kv, isHKA);
            SumMeasuringDevices(kv, isHKA, tmpML, sumRullar, sumGxS);
            SumAF(kv, isHKA, tmpd1, tmpBricka, sumBricka, sumRullar, sumGxS);

            // ── Texts ─────────────────────────────────────────────────────────────
            kv["SumText1"] = isHKA
                ? "Okulär kontroll av Grader, frifläckar, slagmärken, repor, valkar etc. samt stämpling. Görs 2ggr/Tim."
                : "Okulär kontroll av Grader, frifläckar, slagmärken, repor, valkar etc. Märkning ska vara rätt och tydlig. Görs 2ggr/Tim.";
            kv["SumText1"] = "Okulär kontroll av Grader, frifläckar, slagmärken, repor, valkar etc. Märkning ska vara rätt och tydlig. Görs 2ggr/Tim.";
           kv["SumText2"] = "Om dålig gänga upptäcks skall alla hylsor sedan senaste kontroll kontrolleras med gängring.";
            kv["SumText3"] = "";
            kv["SumTextFrekvens_6x3"] = "";

            // ── Ritningar ─────────────────────────────────────────────────────────
            kv["SumRitRa"] = "Yta: 7430184:2";
            kv["SumRitTol"] = "Toleranser: 1432012:7, 7437495:4";
            kv["SumKlEgenskaper"] = "PRODUCTION NUTS & SLEEVES & HOUSINGSALLMÄNKLASSADE EGENSKAPERKlassade egenskaper klämhylsor";

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

        // ── Freq / Devices / AF — identical to Special ────────────────────────────
        private static void SumFrequencies(Dictionary<string, string> kv, bool m)
        {
            kv["SumF1_1"] = m ? "1/75" : "";
            kv["SumF1_2"] = m ? "4/skift" : "";
            kv["SumF1_3"] = m ? "inst." : "";
            kv["SumF1_4"] = m ? "1/75" : "";
            kv["SumF1_6"] = m ? "1/50" : "";
            kv["SumF1_7"] = m ? "1/75" : "";
            kv["SumF1_8"] = m ? "4/skift" : "";
            kv["SumF1_9"] = m ? "inst./1/skift" : "";
            kv["SumF1_0"] = m ? "inst./1/skift" : "";
            kv["SumF1_11"] = m ? "4/skift" : "";
        }

        private static void SumMeasuringDevices(Dictionary<string, string> kv, bool m, double ml, string rullar, string gangtolk)
        {
            string mlStr = ((int)ml).ToString(CultureInfo.InvariantCulture) + "mm";
            kv["SumD1_1"] = m ? "UD-Apparat inställd med hylsa, ring el. klove." : "";
            kv["SumD1_2"] = m ? "Höjdmätapparat" : "";
            kv["SumD1_3"] = m ? "Skjutmått" : "";
            kv["SumD1_4"] = m ? "UGM-Apparat" : "";
            kv["SumD1_6"] = m ? "Ytjämnhetsmätare" : "";
            kv["SumD1_7"] = m ? "HKA MÄTLÄNGD:" + mlStr : "MHB MÄTLÄNGD:" + mlStr;
            kv["SumD1_8"] = m ? "UD-Apparat" : "";
            kv["SumD1_9"] = m ? "Mätrum/Egglinjal" : "";
            kv["SumD1_0"] = m ? "Mätrum/Egglinjal" : "";
            kv["SumD1_11"] = m ? "Skjutmått" : "";
        }

        private static void SumAF(Dictionary<string, string> kv, bool m, double d1, bool bricka, string sumBricka, string rullar, string gangtolk)
        {
            kv["SumAF1_1"] = m ? "Tolerans efter slits:" : "";
            kv["SumAF1_2"] = m ? "Inställd med passbitar." : "";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = m ? "Spetsar: " + rullar + ", Gängtolk: " + gangtolk + " M\u00e4ts i 2 snitt med 90\u00b0 vridning" : "";
            kv["SumAF1_6"] = m ? "Rp=5, Rz=8" : "";
            string af7 = "Inställningshylsa " + FmtComma(d1) + "mm. Mäts i 2 snitt med 90° vridning";
            if (bricka && !string.IsNullOrEmpty(sumBricka)) af7 += "\n" + sumBricka;
            kv["SumAF1_7"] = m ? af7 : "";
            kv["SumAF1_8"] = "";
            kv["SumAF1_9"] = m ? "Vid misstänkt formfel kontrollera hylsan i MarSurf Contour XC20." : "";
            kv["SumAF1_11"] = "";
        }

        // ── Dimension helpers ─────────────────────────────────────────────────────
        private static double Stmm(double d) { if (d < 25) return 1.0; if (d < 55) return 1.5; if (d < 155) return 2.0; if (d < 205) return 3.0; if (d < 305) return 4.0; return 5.0; }
        private static double DM(double d, double s) { if (Math.Abs(s - 0.75) < 0.001) return d - 0.487; if (Math.Abs(s - 1.0) < 0.001) return d - 0.650; if (Math.Abs(s - 1.5) < 0.001) return d - 0.974; if (Math.Abs(s - 2.0) < 0.001) return d - 1.299; if (Math.Abs(s - 3.0) < 0.001) return d - 1.949; return 0; }
        private static double VT(double d) { if (d > 181) return 0.15; if (d > 150) return 0.18; if (d > 120) return 0.30; if (d > 80) return 0.45; if (d > 50) return 0.50; return 0.60; }
        private static double DTolPos(double s) { if (Math.Abs(s - 0.75) < 0.001) return 0.022; if (Math.Abs(s - 1.0) < 0.001) return 0.026; if (Math.Abs(s - 1.5) < 0.001) return 0.032; if (Math.Abs(s - 2.0) < 0.001) return 0.038; if (Math.Abs(s - 3.0) < 0.001) return 0.048; return 0; }
        private static double DTolNeg(double s) { if (Math.Abs(s - 0.75) < 0.001) return 0.162; if (Math.Abs(s - 1.0) < 0.001) return 0.206; if (Math.Abs(s - 1.5) < 0.001) return 0.268; if (Math.Abs(s - 2.0) < 0.001) return 0.318; if (Math.Abs(s - 3.0) < 0.001) return 0.423; return 0; }
        private static double DmTolNeg(double d, double s)
        {
            if (Math.Abs(s - 0.75) < 0.001) return 0.147; if (Math.Abs(s - 1.0) < 0.001) return 0.176;
            if (Math.Abs(s - 1.5) < 0.001) return d < 45.1 ? 0.222 : 0.232;
            if (Math.Abs(s - 2.0) < 0.001) return d < 90.1 ? 0.262 : 0.274;
            if (Math.Abs(s - 3.0) < 0.001) return d < 180.1 ? 0.328 : 0.363; return 0;
        }
        private static double BTol(double b) { if (b > 120) return 4.0; if (b > 80) return 3.5; if (b > 50) return 3.0; if (b > 30) return 2.5; if (b > 18) return 2.1; if (b > 10) return 1.8; return 1.5; }
        private static double GTolPos(double d, double k) { if (Math.Abs(k - 12) < 0.001) { if (d > 250) return 0.055; if (d > 180) return 0.050; if (d > 120) return 0.040; if (d > 80) return 0.035; if (d > 50) return 0.030; if (d > 30) return 0.025; return 0.020; } if (Math.Abs(k - 30) < 0.001) { if (d > 250) return 0.035; if (d > 180) return 0.030; if (d > 120) return 0.025; if (d > 80) return 0.022; if (d > 50) return 0.019; if (d > 30) return 0.016; return 0.013; } return 0; }
        private static double GTolNeg(double d, double k) { if (Math.Abs(k - 12) < 0.001) { if (d > 250) return 0.160; if (d > 180) return 0.140; if (d > 120) return 0.120; if (d > 80) return 0.105; if (d > 50) return 0.090; if (d > 30) return 0.075; return 0.070; } if (Math.Abs(k - 30) < 0.001) { if (d > 250) return 0.095; if (d > 180) return 0.085; if (d > 120) return 0.075; if (d > 80) return 0.065; if (d > 50) return 0.055; if (d > 30) return 0.046; return 0.039; } return 0; }
        private static double GenTolVal(double v) { if (v < 6.01) return 0.1; if (v < 30.01) return 0.2; if (v < 120.01) return 0.3; if (v < 400.01) return 0.5; if (v < 1000.01) return 0.8; if (v < 2000.01) return 1.2; return 2.0; }
        private static double D1Tol(double d1) { if (d1 > 250) return 0.065; if (d1 > 180) return 0.057; if (d1 > 120) return 0.050; if (d1 > 80) return 0.043; if (d1 > 50) return 0.037; if (d1 > 30) return 0.031; return 0.026; }
        private static double D1STolPos(double d) { if (d > 201) return 0.185; if (d > 131) return 0.100; if (d > 91) return 0.087; if (d > 56) return 0.074; if (d > 39) return 0.062; return 0.052; }
        private static double D1STolNeg(double d) { if (d > 201) return 0.290; if (d > 131) return 0.250; if (d > 91) return 0.220; if (d > 56) return 0.120; if (d > 39) return 0.100; return 0.084; }
        private static double LTolNeg(double L, double tud)
        {
            if (Math.Abs(tud) < 0.001) { if (L > 400) return 2.5; if (L > 315) return 2.3; if (L > 250) return 2.1; if (L > 180) return 1.85; if (L > 120) return 1.6; if (L > 80) return 1.4; if (L > 50) return 1.2; if (L > 30) return 1.0; if (L > 18) return 0.84; if (L > 10) return 0.70; return 0.58; }
            if (Math.Abs(tud - 1) < 0.001) { if (L > 630) return 0.8; if (L > 500) return 0.7; if (L > 400) return 0.63; if (L > 315) return 0.57; if (L > 250) return 0.52; if (L > 180) return 0.46; if (L > 120) return 0.4; if (L > 80) return 0.35; if (L > 50) return 0.3; if (L > 30) return 0.25; return 0.21; }
            return 0;
        }
        private static double RakA(double d) { if (d < 101) return 0.008; if (d < 281) return 0.010; if (d < 481) return 0.012; if (d < 601) return 0.014; if (d < 901) return 0.016; return 0.020; }
        private static double RakB(double d) { if (d < 101) return 0.012; if (d < 281) return 0.015; if (d < 481) return 0.018; if (d < 601) return 0.021; if (d < 901) return 0.024; return 0.030; }
        private static string VE(double d) { if (d > 250) return "0.025"; if (d > 180) return "0.020"; if (d > 120) return "0.015"; if (d > 50) return "0.010"; return "0.008"; }
        private static string FasMM(double d) { if (d < 24) return "0.7"; if (d < 69) return "1.1"; if (d < 159) return "1.8"; if (d < 219) return "2.4"; return "2.7"; }

        // ── Tungspår f — TYP-based (key difference from Special) ─────────────────
        private static double TungF_typ(int t)
        {
            if (t < 2) return 9; if (t < 4) return 10; if (t == 4) return 12;
            if (t < 7) return 14.5; if (t == 7) return 15.5; if (t == 8) return 16.5;
            if (t == 9) return 17.5; if (t == 10) return 18.5; if (t < 13) return 19.5;
            if (t == 13) return 20.5; if (t == 14) return 21; return 22;
        }

        // ── Tungspår e — derived from f (NOT from d like Special) ────────────────
        private static double TungE_f(double f)
        {
            if (Math.Abs(f - 9) < 0.001) return 4;
            if (f < 13) return 5;
            if (f < 15) return 6;
            if (f < 19) return 7;
            return 9;
        }

        private static double ETol(double e) { if (e > 18) return 0.520; if (e > 10) return 0.430; if (e > 6) return 0.360; return 0.300; }
        private static double FTol(double f) { if (f > 51) return 3.0; if (f > 31) return 2.5; if (f > 19) return 2.1; return 1.8; }

        // ── Utilities ─────────────────────────────────────────────────────────────
        private static int GetMember(string val, string[] list) { for (int i = 0; i < list.Length; i++) if (string.Equals(list[i], val, StringComparison.OrdinalIgnoreCase)) return i + 1; return 0; }
        private static bool EqualsI(string a, string b) => string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);
        private static int TryParseInt(string s) { int v; return int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0; }
        private static double GetDouble(List<Bookmark> bm, string key) { string r = GetString(bm, key); if (string.IsNullOrWhiteSpace(r)) return 0; double v; return double.TryParse(r.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0; }
        private static string GetString(List<Bookmark> bm, string key) { if (bm == null || string.IsNullOrWhiteSpace(key)) return ""; for (int i = 0; i < bm.Count; i++) { Bookmark b = bm[i]; if (b != null && string.Equals(b.BookmarkName ?? "", key, StringComparison.OrdinalIgnoreCase)) return b.BookmarkValue ?? ""; } return ""; }
        private static string Fmt(double v) => v.ToString(CommonFunctions.Culture).Replace(",", ".");
        private static string FmtComma(double v) => v.ToString("0.################", CommonFunctions.Culture).Replace(".", ",");
        private static string Fmt3(double v) => v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
        private static string FmtStmm(double v) => v.ToString("0.################", CommonFunctions.Culture).Replace(".", ",");
    }
}