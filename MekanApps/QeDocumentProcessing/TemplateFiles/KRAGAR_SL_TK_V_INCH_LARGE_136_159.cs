using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class KRAGAR_SL_TK_V_INCH_LARGE_136_159 : ITemplateCalculations
    {
        private static readonly string[] Machines = new[] { "Nakamura", "LB45", "LT-3000EX" };
        private static readonly string[] Types = { "136", "140", "148", "155", "159" };

        // D tables: 14 elements [d1, d2, d3, d4, d5, d6, d7, d8, d9, d10, d11, d12, d13, d14]
        // (indices 1-14 via @Word, 0-based array)
        private static readonly Dictionary<string, double[]> DTabs = new Dictionary<string, double[]>(StringComparer.OrdinalIgnoreCase)
        {
            ["136"] = new double[] { 170, 159.22, 149.22, 156, 178, 194, 202, 218, 226, 242, 250, 266, 280, 12 },
            ["140"] = new double[] { 170, 160.81, 150.81, 156, 178, 194, 202, 218, 226, 242, 250, 266, 280, 12 },
            ["148"] = new double[] { 180, 173.51, 163.51, 168.5, 188, 204, 212, 228, 236, 252, 260, 276, 290, 12 },
            ["155"] = new double[] { 200, 186.21, 176.21, 186, 208, 224, 232, 248, 256, 272, 280, 296, 310, 12 },
            ["159"] = new double[] { 200, 192.56, 182.56, 188, 208, 224, 232, 248, 256, 272, 280, 296, 310, 12 },
        };

        // B tables: 8 elements [B, b1, b2, b3, b4, b5, b6, b7]
        // (indices 1-8 via @Word, 0-based array)
        private static readonly Dictionary<string, double[]> BTabs = new Dictionary<string, double[]>(StringComparer.OrdinalIgnoreCase)
        {
            ["136"] = new double[] { 33, 15.5, 10.5, 5.7, 9, 8, 11.5, 25 },
            ["140"] = new double[] { 33, 15.5, 10.5, 5.7, 9, 8, 11.5, 25 },
            ["148"] = new double[] { 33, 15.5, 10.5, 5.7, 9, 8, 11, 25 },
            ["155"] = new double[] { 34, 15.5, 10.5, 5.7, 9, 8, 11.5, 26 },
            ["159"] = new double[] { 34, 15.5, 10.5, 5.7, 9, 8, 11.5, 26 },
        };

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

            // Explode: " /." (space, slash, dot)
            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : string.Empty;
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : string.Empty;

            // TmpTyp = TmpBet2 directly
            string tmpTyp = tmpBet2;
            double typNum = TryParseDouble(tmpTyp);

            double[] dTab = DTabs.ContainsKey(tmpTyp) ? DTabs[tmpTyp] : null;
            double[] bTab = BTabs.ContainsKey(tmpTyp) ? BTabs[tmpTyp] : null;

            // ── Diameters ────────────────────────────────────────────────────────

            // d1: fixed tol +4.0 / -0.0
            double tmpd1 = BmOrTab(bm, "Ø d1", dTab, 1);
            kv["Sumd1"] = "(d1) " + Fmt(tmpd1);
            kv["Sumd1Tol"] = "+ " + Fmt1(4.0);
            kv["Sumd1TolN"] = "- " + Fmt1(0.0);

            // d2: H12 full 21-step positive tol
            double tmpd2 = BmOrTab(bm, "Ø d2", dTab, 2);
            kv["Sumd2"] = "(d2) " + Fmt(tmpd2);
            kv["Sumd2Tol"] = "+ " + Fmt3(H12Tol21(tmpd2));
            kv["Sumd2TolN"] = "- " + Fmt1(0.0);

            // d3: both tols are "+", d3TolN = +0.1 (BOTH POSITIVE — exact Lotus script)
            double tmpd3 = BmOrTab(bm, "Ø d3", dTab, 3);
            kv["Sumd3"] = "(d3) " + Fmt(tmpd3);
            kv["Sumd3Tol"] = "+ " + Fmt3(0.2);
            kv["Sumd3TolN"] = "+ " + Fmt3(0.1);   // NOTE: "+ " not "- " — as per Lotus script

            // d4: h12 14-step negative tol (stops at 0.700)
            double tmpd4 = BmOrTab(bm, "Ø d4", dTab, 4);
            kv["Sumd4"] = "(d4) " + Fmt(tmpd4);
            kv["Sumd4Tol"] = "+ " + Fmt1(0.0);
            kv["Sumd4TolN"] = "- " + Fmt3(H12Tol14(tmpd4));

            // d5: h12 14-step negative tol
            double tmpd5 = BmOrTab(bm, "Ø d5", dTab, 5);
            kv["Sumd5"] = "(d5) " + Fmt(tmpd5);
            kv["Sumd5Tol"] = "+ " + Fmt1(0.0);
            kv["Sumd5TolN"] = "- " + Fmt3(H12Tol14(tmpd5));

            // d6: H12 full 21-step positive tol
            double tmpd6 = BmOrTab(bm, "Ø d6", dTab, 6);
            kv["Sumd6"] = "(d6) " + Fmt(tmpd6);
            kv["Sumd6Tol"] = "+ " + Fmt3(H12Tol21(tmpd6));
            kv["Sumd6TolN"] = "- " + Fmt1(0.0);

            // d7: h12 14-step negative tol
            double tmpd7 = BmOrTab(bm, "Ø d7", dTab, 7);
            kv["Sumd7"] = "(d7) " + Fmt(tmpd7);
            kv["Sumd7Tol"] = "+ " + Fmt1(0.0);
            kv["Sumd7TolN"] = "- " + Fmt3(H12Tol14(tmpd7));

            // d8: H12 full 21-step positive tol
            double tmpd8 = BmOrTab(bm, "Ø d8", dTab, 8);
            kv["Sumd8"] = "(d8) " + Fmt(tmpd8);
            kv["Sumd8Tol"] = "+ " + Fmt3(H12Tol21(tmpd8));
            kv["Sumd8TolN"] = "- " + Fmt1(0.0);

            // d9: h12 14-step negative tol
            double tmpd9 = BmOrTab(bm, "Ø d9", dTab, 9);
            kv["Sumd9"] = "(d9) " + Fmt(tmpd9);
            kv["Sumd9Tol"] = "+ " + Fmt1(0.0);
            kv["Sumd9TolN"] = "- " + Fmt3(H12Tol14(tmpd9));

            // d10: H12 full 21-step positive tol
            double tmpd10 = BmOrTab(bm, "Ø d10", dTab, 10);
            kv["Sumd10"] = "(d10) " + Fmt(tmpd10);
            kv["Sumd10Tol"] = "+ " + Fmt3(H12Tol21(tmpd10));
            kv["Sumd10TolN"] = "- " + Fmt1(0.0);

            // d11: h12 14-step negative tol
            double tmpd11 = BmOrTab(bm, "Ø d11", dTab, 11);
            kv["Sumd11"] = "(d11) " + Fmt(tmpd11);
            kv["Sumd11Tol"] = "+ " + Fmt1(0.0);
            kv["Sumd11TolN"] = "- " + Fmt3(H12Tol14(tmpd11));

            // d12: H12 full 21-step positive tol
            double tmpd12 = BmOrTab(bm, "Ø d12", dTab, 12);
            kv["Sumd12"] = "(d12) " + Fmt(tmpd12);
            kv["Sumd12Tol"] = "+ " + Fmt3(H12Tol21(tmpd12));
            kv["Sumd12TolN"] = "- " + Fmt1(0.0);

            // d13: fixed ± 3.0
            double tmpd13 = BmOrTab(bm, "Ø d13", dTab, 13);
            kv["Sumd13"] = "(d13) " + Fmt(tmpd13);
            kv["Sumd13Tol"] = "± " + Fmt1(3.0);

            // d14: "3x Ø" prefix, no tolerance
            double tmpd14 = BmOrTab(bm, "Ø d14", dTab, 14);
            kv["Sumd14"] = "3x Ø " + Fmt(tmpd14);

            // ── Widths ───────────────────────────────────────────────────────────

            double tmpB = BmOrTab(bm, "Bredd B", bTab, 1);
            kv["SumB"] = "(B) " + Fmt(tmpB);
            kv["SumBTol"] = "+ " + Fmt1(1.0);
            kv["SumBTolN"] = "+ " + Fmt1(0.0);    // NOTE: "+" not "-" — exact Lotus script

            double tmpb1 = BmOrTab(bm, "Bredd b1", bTab, 2);
            kv["Sumb1"] = "(b1) " + Fmt(tmpb1);
            kv["Sumb1Tol"] = "± " + Fmt1(1.0);

            double tmpb2 = BmOrTab(bm, "Bredd b2", bTab, 3);
            kv["Sumb2"] = "(b2) " + Fmt(tmpb2);
            kv["Sumb2Tol"] = "+ " + Fmt3(0.5);
            kv["Sumb2TolN"] = "- " + Fmt1(0.0);

            double tmpb3 = BmOrTab(bm, "Bredd b3", bTab, 4);
            // b3Tol depends on d3 value
            string b3Tol = tmpd3 < 175 ? Fmt3(0.2) : Fmt3(0.3);
            kv["Sumb3"] = "(b3) " + Fmt(tmpb3);
            kv["Sumb3Tol"] = "+ " + b3Tol;
            if(subject== "SL-TK 156 V")
                kv["Sumb3Tol"] = "+ 200";
            kv["Sumb3TolN"] = "- " + Fmt1(0.0);

            double tmpb4 = BmOrTab(bm, "Bredd b4", bTab, 5);
            kv["Sumb4"] = "(b4) " + Fmt(tmpb4);
            kv["Sumb4Tol"] = "+ " + Fmt3(0.2);
            kv["Sumb4TolN"] = "- " + Fmt1(0.0);

            double tmpb5 = BmOrTab(bm, "Bredd b5", bTab, 6);
            kv["Sumb5"] = "(b5) " + Fmt(tmpb5);
            kv["Sumb5Tol"] = "+ " + Fmt3(0.25);
            kv["Sumb5TolN"] = "- " + Fmt1(0.0);

            double tmpb6 = BmOrTab(bm, "Bredd b6", bTab, 7);
            kv["Sumb6"] = "(b6) " + Fmt(tmpb6);
            kv["Sumb6Tol"] = "± " + Fmt3(0.25);

            double tmpb7 = BmOrTab(bm, "Bredd b7", bTab, 8);
            kv["Sumb7"] = "3x (b7) " + Fmt(tmpb7);
            kv["Sumb7Tol"] = "+ " + Fmt1(0.0);
            kv["Sumb7TolN"] = "- " + Fmt3(0.5);

            // b8 — fixed constant 0.5
            kv["Sumb8"] = Fmt3(0.5);

            // ── Fixed features ────────────────────────────────────────────────────

            // G: all types 136-159 < 160 → M8 (3x)
            kv["SumG"] = "(G) M8 (3x)";

            // Radier
            kv["SumR08"] = "R max: 0.8 (8x)";
            kv["SumR08a"] = "R max: 0.8";
            kv["SumR05"] = "R0.5 (2x)";
            kv["SumR05a"] = "max R0.5 (3x)";
            kv["SumR4"] = "R4";

            // Faser
            kv["SumF1"] = "(3x) 1.5x45°";
            kv["SumF2"] = "(2x) 1x45°";

            // Concentricity
            kv["SumCo1"] = " " + Fmt3(0.15);   // leading space preserved from Lotus

            // Surface finish
            kv["SumRa32"] = "3.2";
            kv["SumRa125"] = "12.5";

            // ── Machine ───────────────────────────────────────────────────────────

            kv["SumMaskinvalS1"] = "Maskin: " + maskinVal + " - Diametrala mått.";
            kv["SumMaskinvalS2"] = "Maskin: " + maskinVal + " - Övriga mått.";

            SumFrequencies(kv, maskinVal);
            SumMeasuringDevices(kv, maskinVal, tmpd3);
            SumAF(kv, maskinVal);

            // ── Ritning ───────────────────────────────────────────────────────────

            // NOTE: "senaste utgåva" with å — different from TATNINGAR's "senaste utg,"
            string ritBase = DTabs.ContainsKey(tmpTyp) ? "TK " + tmpTyp + " V" : "";
            string sumRitNr = ritBase + ": senaste utgåva";
            kv["SumRitNr"] = sumRitNr;
            kv["SumRitNr2"] = sumRitNr;

            // ── Text ──────────────────────────────────────────────────────────────

            string sumText = "Okulärkontroll Grader, frifläckar, slagmärken, repor, valkar, ytjämnhet och faser, skarpa kanter avgradas.";
            kv["SumTextS1"] = sumText;
            kv["SumTextS2"] = sumText;

            return kv;
        }

        // ── Popup ──────────────────────────────────────────────────────────────────
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

        // ── Freq / Devices / AF ──────────────────────────────────────────────────
        private static void SumFrequencies(Dictionary<string, string> kv, string mv)
        {
            bool m = IsMachine(mv);
            // F1_1-4 = "1/2", F1_5 = always "", F1_6-7 = "1/2"
            kv["SumF1_1"] = m ? "1/2" : ""; kv["SumF1_2"] = m ? "1/2" : "";
            kv["SumF1_3"] = m ? "1/2" : ""; kv["SumF1_4"] = m ? "1/2" : "";
            kv["SumF1_5"] = "";              // always empty per Lotus script
            kv["SumF1_6"] = m ? "1/2" : ""; kv["SumF1_7"] = m ? "1/2" : "";
            // Sid 2: F2_1-7 = all "1/2"
            for (int i = 1; i <= 7; i++) kv["SumF2_" + i] = m ? "1/2" : "";
        }

        private static void SumMeasuringDevices(Dictionary<string, string> kv, string mv, double d3)
        {
            bool m = IsMachine(mv);
            // D1_2: if d3>300 → "Mätplatta" else "Mätmaskin alt.UD-Apparat"
            // All types 136-159 have d3 < 300, so always the second branch
            string d1_2 = d3 > 300 ? "Mätplatta" : "Mätmaskin alt.UD-Apparat";
            kv["SumD1_1"] = m ? "Mätmaskin alt. Skjutmått" : "";
            kv["SumD1_2"] = m ? d1_2 : "";
            kv["SumD1_3"] = m ? "Mätmaskin alt.Skjutmått" : "";
            kv["SumD1_4"] = m ? "Ytjämnhetsmätare" : "";
            kv["SumD1_5"] = "";
            kv["SumD1_6"] = m ? "Mätmaskin alt.Mätservice" : "";
            kv["SumD1_7"] = m ? "Mätmaskin alt.Skjutmått" : "";
            kv["SumD2_1"] = m ? "Mätmaskin alt.Skjutmått" : "";
            kv["SumD2_2"] = m ? "Mätmaskin alt.Skjutmått" : "";
            kv["SumD2_3"] = m ? "Mätmaskin alt.Skjutmått" : "";
            kv["SumD2_4"] = m ? "Gängtolk" : "";
            kv["SumD2_5"] = m ? "Mätmaskin alt.Skjutmått" : "";
            kv["SumD2_6"] = m ? "Ytjämnhetsmätare" : "";
            kv["SumD2_7"] = m ? "Mätmaskin alt.Skjutmått" : "";
        }

        private static void SumAF(Dictionary<string, string> kv, string mv)
        {
            bool m = IsMachine(mv);
            kv["SumAF1_1"] = ""; kv["SumAF1_2"] = ""; kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = m ? "Övriga bearbetade ytor 6.3" : "";
            kv["SumAF1_5"] = ""; kv["SumAF1_6"] = ""; kv["SumAF1_7"] = "";
            kv["SumAF2_1"] = ""; kv["SumAF2_2"] = ""; kv["SumAF2_3"] = "";
            kv["SumAF2_4"] = ""; kv["SumAF2_5"] = "";
            kv["SumAF2_6"] = m ? "Övriga bearbetade ytor 6.3" : "";
            kv["SumAF2_7"] = "";
        }

        // ── Tolerance tables ─────────────────────────────────────────────────────

        // H12 full 21-step (for d2, d6, d8, d10, d12 — positive direction)
        private static double H12Tol21(double v)
        {
            if (v < 3.01) return 0.100; if (v < 6.01) return 0.120;
            if (v < 10.01) return 0.150; if (v < 18.01) return 0.180;
            if (v < 30.01) return 0.210; if (v < 50.01) return 0.250;
            if (v < 80.01) return 0.300; if (v < 120.01) return 0.350;
            if (v < 180.01) return 0.400; if (v < 250.01) return 0.460;
            if (v < 315.01) return 0.520; if (v < 400.01) return 0.570;
            if (v < 500.01) return 0.630; if (v < 630.01) return 0.700;
            if (v < 800.01) return 0.800; if (v < 1000.01) return 0.900;
            if (v < 1250.01) return 1.050; if (v < 1600.01) return 1.250;
            if (v < 2000.01) return 1.500; if (v < 2500.01) return 1.750;
            return 2.100;
        }

        // h12 14-step (for d4, d5, d7, d9, d11 — negative direction, stops at 0.700)
        private static double H12Tol14(double v)
        {
            if (v < 3.01) return 0.100; if (v < 6.01) return 0.120;
            if (v < 10.01) return 0.150; if (v < 18.01) return 0.180;
            if (v < 30.01) return 0.210; if (v < 50.01) return 0.250;
            if (v < 80.01) return 0.300; if (v < 120.01) return 0.350;
            if (v < 180.01) return 0.400; if (v < 250.01) return 0.460;
            if (v < 315.01) return 0.520; if (v < 400.01) return 0.570;
            if (v < 500.01) return 0.630; return 0.700;
        }

        // ── Utilities ─────────────────────────────────────────────────────────────
        private static double BmOrTab(List<Bookmark> bm, string key, double[] tab, int oneBasedIdx)
        {
            double v = GetDouble(bm, key);
            if (v != 0) return v;
            if (tab == null || oneBasedIdx < 1 || oneBasedIdx > tab.Length) return 0;
            return tab[oneBasedIdx - 1];
        }

        private static bool IsMachine(string mv)
        {
            if (string.IsNullOrEmpty(mv)) return false;
            for (int i = 0; i < Machines.Length; i++)
                if (string.Equals(Machines[i], mv, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
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

        private static string Fmt(double v) => v.ToString(CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt1(double v) => v.ToString("F1", CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt3(double v) => v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
    }
}