using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class KRAGAR_SL_TK_N_Narrow_42_SMALL : ITemplateCalculations
    {
        private static readonly string[] Machines = new[] { "Nakamura", "LB45", "LT-3000EX" };
        private static readonly string[] Types = { "34", "36", "38", "40" };

        // D tables: 14 elements [d1..d13, d14]  (only typ36 has real data; 34/38/40 = placeholder {0})
        private static readonly Dictionary<string, double[]> DTabs = new Dictionary<string, double[]>(StringComparer.OrdinalIgnoreCase)
        {
            ["34"] = null,  // {0} placeholder — all dimensions from bookmarks
            ["36"] = new double[] { 180, 169.5, 160, 166, 188, 204, 212, 228, 236, 252, 260, 276, 290, 12 },
            ["38"] = null,
            ["40"] = null,
        };

        // B tables: 8 elements [B, b1, b2, b3, b4, b5, b6, b7]  (only typ36 has real data)
        private static readonly Dictionary<string, double[]> BTabs = new Dictionary<string, double[]>(StringComparer.OrdinalIgnoreCase)
        {
            ["34"] = null,
            ["36"] = new double[] { 33, 15.5, 10.5, 5.7, 9, 5.5, 11.5, 26.5 },
            ["38"] = null,
            ["40"] = null,
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

            // Explode: " /." (space, slash, dot)
            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : string.Empty;

            // TmpTyp = TmpBet2 directly
            string tmpTyp = tmpBet2;
            double typNum = TryParseDouble(tmpTyp);

            double[] dTab = DTabs.ContainsKey(tmpTyp) ? DTabs[tmpTyp] : null;
            double[] bTab = BTabs.ContainsKey(tmpTyp) ? BTabs[tmpTyp] : null;

            // ── Diameters ────────────────────────────────────────────────────────

            // d1: fixed +4.0 / -0.0
            double tmpd1 = BmOrTab(bm, "Ø d1", dTab, 1);
            kv["Sumd1"] = "(d1) " + Fmt(tmpd1);
            kv["Sumd1Tol"] = "+ " + Fmt1(4.0);
            kv["Sumd1TolN"] = "- " + Fmt1(0.0);

            // d2: H12 21-step positive
            double tmpd2 = BmOrTab(bm, "Ø d2", dTab, 2);
            kv["Sumd2"] = "(d2) " + Fmt(tmpd2);
            kv["Sumd2Tol"] = "+ " + Fmt3(H12Tol21(tmpd2));
            kv["Sumd2TolN"] = "- " + Fmt1(0.0);

            // d3: +0.2 / +0.1 (BOTH positive — same as Large)
            double tmpd3 = BmOrTab(bm, "Ø d3", dTab, 3);
            kv["Sumd3"] = "(d3) " + Fmt(tmpd3);
            kv["Sumd3Tol"] = "+ " + Fmt3(0.2);
            kv["Sumd3TolN"] = "+ " + Fmt3(0.1);   // NOTE: "+" not "-"

            // d4: h12 14-step negative
            double tmpd4 = BmOrTab(bm, "Ø d4", dTab, 4);
            kv["Sumd4"] = "(d4) " + Fmt(tmpd4);
            kv["Sumd4Tol"] = "+ " + Fmt1(0.0);
            kv["Sumd4TolN"] = "- " + Fmt3(H12Tol14(tmpd4));

            // d5: h12 14-step negative
            double tmpd5 = BmOrTab(bm, "Ø d5", dTab, 5);
            kv["Sumd5"] = "(d5) " + Fmt(tmpd5);
            kv["Sumd5Tol"] = "+ " + Fmt1(0.0);
            kv["Sumd5TolN"] = "- " + Fmt3(H12Tol14(tmpd5));

            // d6: H12 21-step positive
            double tmpd6 = BmOrTab(bm, "Ø d6", dTab, 6);
            kv["Sumd6"] = "(d6) " + Fmt(tmpd6);
            kv["Sumd6Tol"] = "+ " + Fmt3(H12Tol21(tmpd6));
            kv["Sumd6TolN"] = "- " + Fmt1(0.0);

            // d7: h12 14-step negative
            double tmpd7 = BmOrTab(bm, "Ø d7", dTab, 7);
            kv["Sumd7"] = "(d7) " + Fmt(tmpd7);
            kv["Sumd7Tol"] = "+ " + Fmt1(0.0);
            kv["Sumd7TolN"] = "- " + Fmt3(H12Tol14(tmpd7));

            // d8: H12 21-step positive
            double tmpd8 = BmOrTab(bm, "Ø d8", dTab, 8);
            kv["Sumd8"] = "(d8) " + Fmt(tmpd8);
            kv["Sumd8Tol"] = "+ " + Fmt3(H12Tol21(tmpd8));
            kv["Sumd8TolN"] = "- " + Fmt1(0.0);

            // d9: h12 14-step negative
            double tmpd9 = BmOrTab(bm, "Ø d9", dTab, 9);
            kv["Sumd9"] = "(d9) " + Fmt(tmpd9);
            kv["Sumd9Tol"] = "+ " + Fmt1(0.0);
            kv["Sumd9TolN"] = "- " + Fmt3(H12Tol14(tmpd9));

            // d10: H12 21-step positive
            double tmpd10 = BmOrTab(bm, "Ø d10", dTab, 10);
            kv["Sumd10"] = "(d10) " + Fmt(tmpd10);
            kv["Sumd10Tol"] = "+ " + Fmt3(H12Tol21(tmpd10));
            kv["Sumd10TolN"] = "- " + Fmt1(0.0);

            // d11: h12 14-step negative
            double tmpd11 = BmOrTab(bm, "Ø d11", dTab, 11);
            kv["Sumd11"] = "(d11) " + Fmt(tmpd11);
            kv["Sumd11Tol"] = "+ " + Fmt1(0.0);
            kv["Sumd11TolN"] = "- " + Fmt3(H12Tol14(tmpd11));

            // d12: H12 21-step positive
            double tmpd12 = BmOrTab(bm, "Ø d12", dTab, 12);
            kv["Sumd12"] = "(d12) " + Fmt(tmpd12);
            kv["Sumd12Tol"] = "+ " + Fmt3(H12Tol21(tmpd12));
            kv["Sumd12TolN"] = "- " + Fmt1(0.0);

            // d13: fixed ±3.0
            double tmpd13 = BmOrTab(bm, "Ø d13", dTab, 13);
            kv["Sumd13"] = "(d13) " + Fmt(tmpd13);
            kv["Sumd13Tol"] = "± " + Fmt1(3.0);

            // d14: "3x Ø", no tol
            double tmpd14 = BmOrTab(bm, "Ø d14", dTab, 14);
            kv["Sumd14"] = "3x Ø " + Fmt(tmpd14);

            // ── Widths ───────────────────────────────────────────────────────────

            double tmpB = BmOrTab(bm, "Bredd B", bTab, 1);
            kv["SumB"] = "(B) " + Fmt(tmpB);
            kv["SumBTol"] = "+ " + Fmt1(1.0);
            kv["SumBTolN"] = "+ " + Fmt1(0.0);   // BOTH "+" — same as Large

            double tmpb1 = BmOrTab(bm, "Bredd b1", bTab, 2);
            kv["Sumb1"] = "(b1) " + Fmt(tmpb1);
            kv["Sumb1Tol"] = "± " + Fmt1(1.0);

            double tmpb2 = BmOrTab(bm, "Bredd b2", bTab, 3);
            kv["Sumb2"] = "(b2) " + Fmt(tmpb2);
            kv["Sumb2Tol"] = "+ " + Fmt3(0.5);
            kv["Sumb2TolN"] = "- " + Fmt1(0.0);

            double tmpb3 = BmOrTab(bm, "Bredd b3", bTab, 4);
            // b3Tol: FIXED +0.3 (TmpKon03) — NOT d3-dependent like Large class
            kv["Sumb3"] = "(b3) " + Fmt(tmpb3);
            kv["Sumb3Tol"] = "+ " + Fmt3(0.3);
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

            // b8: fixed 0.500
            kv["Sumb8"] = Fmt3(0.5);

            // ── Fixed features ────────────────────────────────────────────────────

            // G: all types 34/36/38/40 < 52 → always M8 (3x)
            kv["SumG"] = "(G) M8 (3x)";

            // Radier — identical to Large
            kv["SumR08"] = "R max: 0.8 (8x)";
            kv["SumR08a"] = "R max: 0.8";
            kv["SumR05"] = "R0.5 (2x)";
            kv["SumR05a"] = "max R0.5 (3x)";
            kv["SumR4"] = "R4";

            // Faser — no parentheses (differs from Large: "(3x)" → "3x")
            kv["SumF1"] = "3x 1.5x45°";
            kv["SumF2"] = "2x 1x45°";

            kv["SumCo1"] = " " + Fmt3(0.15);  // leading space preserved
            kv["SumRa32"] = "3.2";
            kv["SumRa125"] = "12.5";

            // ── Machine ───────────────────────────────────────────────────────────

            kv["SumMaskinvalS1"] = "Maskin: " + maskinVal + " - Diametrala mått.";
            kv["SumMaskinvalS2"] = "Maskin: " + maskinVal + " - Övriga mått.";

            SumFrequencies(kv, maskinVal);
            SumMeasuringDevices(kv, maskinVal, tmpd3);
            SumAF(kv, maskinVal);

            // ── Ritning — FIXED number (not typ-based like Large) ─────────────────
            kv["SumRitNr"] = "7440047: senaste utgåva";
            kv["SumRitNr2"] = "7440047: senaste utgåva";

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
                       " dagar)\n\nInformation om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument\n\nPopupruta aktiv till " +
                       validTill.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return "";
        }

        // ── Freq / Devices / AF ──────────────────────────────────────────────────
        private static void SumFrequencies(Dictionary<string, string> kv, string mv)
        {
            bool m = IsMachine(mv);
            // All "1/5" (vs "1/2" in Large class); F1_5 always empty
            kv["SumF1_1"] = m ? "1/5" : ""; kv["SumF1_2"] = m ? "1/5" : "";
            kv["SumF1_3"] = m ? "1/5" : ""; kv["SumF1_4"] = m ? "1/5" : "";
            kv["SumF1_5"] = "";
            kv["SumF1_6"] = m ? "1/5" : ""; kv["SumF1_7"] = m ? "1/5" : "";
            for (int i = 1; i <= 7; i++) kv["SumF2_" + i] = m ? "1/5" : "";
        }

        private static void SumMeasuringDevices(Dictionary<string, string> kv, string mv, double d3)
        {
            bool m = IsMachine(mv);
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

        // ── Tolerance tables (identical to Large class) ───────────────────────────

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