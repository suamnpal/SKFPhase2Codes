using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class TATNINGAR_GR_TK_V_INCH_LARGE_136_159 : ITemplateCalculations
    {
        private static readonly string[] Machines = new[] { "Nakamura", "LB45", "LT-3000EX" };
        private static readonly string[] Types = { "136", "140", "148", "155", "156", "159" };

        // D tables: [d, d1, d2, d3, d4, d5, d6, d7, d8, d9, d10, d11, Da]  (13 elements, index 1-13)
        private static readonly Dictionary<string, double[]> DTabs = new Dictionary<string, double[]>(StringComparer.OrdinalIgnoreCase)
        {
            ["136"] = new double[] { 152, 156.41, 169.75, 167.8, 170.2, 182, 190, 206, 214, 230, 238, 254, 262 },
            ["140"] = new double[] { 155.5, 160.35, 176.86, 177.5, 179.9, 182, 190, 206, 214, 230, 238, 254, 262 },
            ["148"] = new double[] { 167, 171.45, 187.96, 188.7, 191.1, 194, 200, 216, 224, 240, 248, 264, 272 },
            ["155"] = new double[] { 180, 186.77, 203.28, 204, 206.4, 212, 220, 236, 244, 260, 268, 284, 292 },
            ["156"] = new double[] { 181, 186.77, 203.28, 204, 206.4, 212, 220, 236, 244, 260, 268, 284, 292 },
            ["159"] = new double[] { 186, 191.52, 208.03, 208.6, 211, 212, 220, 236, 244, 260, 268, 284, 292 },
        };

        // B tables: [B, b1, b2, b4, b5, H, (7th unused — L is hardcoded)]  (7 elements, index 1-7)
        private static readonly Dictionary<string, double[]> BTabs = new Dictionary<string, double[]>(StringComparer.OrdinalIgnoreCase)
        {
            ["136"] = new double[] { 50.5, 7.5, 15.5, 16, 10.5, 29.5, 41.5 },
            ["140"] = new double[] { 50.5, 7.5, 15.5, 16, 10.5, 29.5, 41 },
            ["148"] = new double[] { 50, 7.5, 16, 16, 10.5, 28, 40 },
            ["155"] = new double[] { 50, 7.5, 16, 16, 10.5, 28, 41.5 },
            ["156"] = new double[] { 50, 7.5, 16, 16, 10.5, 28, 41.5 },
            ["159"] = new double[] { 51, 7.5, 16, 16, 10.5, 29, 41 },
        };

        // TmpL — hardcoded per type (NOT from BTabs — independent formula)
        private static readonly Dictionary<string, double> TmpLMap = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase)
        {
            ["136"] = 41.5,
            ["140"] = 41,
            ["148"] = 40,
            ["155"] = 41.5,
            ["156"] = 41.5,
            ["159"] = 41
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
            bool tmpV = tmpBet.IndexOf('V') >= 0;

            // NOTE: Explode delimiter is " /." — no minus, no dash
            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : string.Empty;
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : string.Empty;

            // TmpTyp = TmpBet2 directly — no Left/Right manipulation
            string tmpTyp = tmpBet2;
            double typNum = TryParseDouble(tmpTyp);

            int typLista = GetMember(tmpTyp, Types);  // 1-based; 0 = not found

            double[] dTab = DTabs.ContainsKey(tmpTyp) ? DTabs[tmpTyp] : null;
            double[] bTab = BTabs.ContainsKey(tmpTyp) ? BTabs[tmpTyp] : null;

            // ── Diameters ────────────────────────────────────────────────────────

            double tmpd = BmOrTab(bm, "Ø d", dTab, 1);
            kv["Sumd"] = "(d) " + Fmt(tmpd);
            kv["SumdTol"] = "+ " + Fmt3(0.4);
            kv["SumdTolN"] = "- " + Fmt1(0.0);

            double tmpd1 = BmOrTab(bm, "Ø d1", dTab, 2);
            kv["Sumd1"] = "2x (d1) " + Fmt(tmpd1);
            kv["Sumd1Tol"] = "+ " + Fmt1(0.0);
            kv["Sumd1TolN"] = "- " + Fmt3(0.3);

            double tmpd2 = BmOrTab(bm, "Ø d2", dTab, 3);
            kv["Sumd2"] = "2x (d2) " + Fmt(tmpd2);
            kv["Sumd2Tol"] = "+ " + Fmt1(0.0);
            kv["Sumd2TolN"] = "- " + Fmt3(0.3);

            double tmpd3 = BmOrTab(bm, "Ø d3", dTab, 4);
            kv["Sumd3"] = "(d3) " + Fmt(tmpd3);
            kv["Sumd3Tol"] = "+ " + Fmt3(0.1);
            kv["Sumd3TolN"] = "- " + Fmt3(0.1);

            double tmpd4 = BmOrTab(bm, "Ø d4", dTab, 5);
            kv["Sumd4"] = "(d4) " + Fmt(tmpd4);
            kv["Sumd4Tol"] = "+ " + Fmt1(0.0);
            kv["Sumd4TolN"] = "- " + Fmt3(0.3);

            double tmpd5 = BmOrTab(bm, "Ø d5", dTab, 6);
            kv["Sumd5"] = "(d5) " + Fmt(tmpd5);
            kv["Sumd5Tol"] = "+ " + Fmt3(H12Tol(tmpd5));
            kv["Sumd5TolN"] = "- " + Fmt1(0.0);

            double tmpd6 = BmOrTab(bm, "Ø d6", dTab, 7);
            kv["Sumd6"] = "(d6) " + Fmt(tmpd6);
            kv["Sumd6Tol"] = "+ " + Fmt1(0.0);
            kv["Sumd6TolN"] = "- " + Fmt3(H12Tol(tmpd6));

            double tmpd7 = BmOrTab(bm, "Ø d7", dTab, 8);
            kv["Sumd7"] = "(d7) " + Fmt(tmpd7);
            kv["Sumd7Tol"] = "+ " + Fmt3(H12Tol(tmpd7));
            kv["Sumd7TolN"] = "- " + Fmt1(0.0);

            double tmpd8 = BmOrTab(bm, "Ø d8", dTab, 9);
            kv["Sumd8"] = "(d8) " + Fmt(tmpd8);
            kv["Sumd8Tol"] = "+ " + Fmt1(0.0);
            kv["Sumd8TolN"] = "- " + Fmt3(H12Tol(tmpd8));

            double tmpd9 = BmOrTab(bm, "Ø d9", dTab, 10);
            kv["Sumd9"] = "(d9) " + Fmt(tmpd9);
            kv["Sumd9Tol"] = "+ " + Fmt3(H12Tol(tmpd9));
            kv["Sumd9TolN"] = "- " + Fmt1(0.0);

            double tmpd10 = BmOrTab(bm, "Ø d10", dTab, 11);
            kv["Sumd10"] = "(d10) " + Fmt(tmpd10);
            kv["Sumd10Tol"] = "+ " + Fmt1(0.0);
            kv["Sumd10TolN"] = "- " + Fmt3(H12Tol(tmpd10));

            double tmpd11 = BmOrTab(bm, "Ø d11", dTab, 12);
            kv["Sumd11"] = "(d11) " + Fmt(tmpd11);
            kv["Sumd11Tol"] = "+ " + Fmt3(H12Tol(tmpd11));
            kv["Sumd11TolN"] = "- " + Fmt1(0.0);

            double tmpDa = BmOrTab(bm, "Ø Da", dTab, 13);
            kv["SumDa"] = "(Da) " + Fmt(tmpDa);
            kv["SumDaTol"] = "+ " + Fmt1(0.0);
            kv["SumDaTolN"] = "- " + Fmt3(H12Tol(tmpDa));

            // ── Widths ───────────────────────────────────────────────────────────

            double tmpB = BmOrTab(bm, "Bredd B", bTab, 1);
            kv["SumB"] = "(B) " + Fmt(tmpB);
            kv["SumBTol"] = "+ " + Fmt3(0.5);
            kv["SumBTolN"] = "- " + Fmt1(0.0);

            double tmpb1 = BmOrTab(bm, "Bredd b1", bTab, 2);
            kv["Sumb1"] = "(b1) " + Fmt(tmpb1);
            kv["Sumb1Tol"] = "± " + Fmt3(0.2);

            double tmpb2 = BmOrTab(bm, "Bredd b2", bTab, 3);
            kv["Sumb2"] = "(b2) " + Fmt(tmpb2);
            kv["Sumb2Tol"] = "+ " + Fmt1(0.0);
            kv["Sumb2TolN"] = "- " + Fmt3(0.5);

            double tmpb4 = BmOrTab(bm, "Bredd b4", bTab, 4);
            string b4Tol = tmpd < 168 ? Fmt1(0.0) : Fmt3(0.2);
            kv["Sumb4"] = "(b4) " + Fmt(tmpb4);
            kv["Sumb4Tol"] = "+ " + b4Tol;
            kv["Sumb4TolN"] = "- " + Fmt3(0.2);

            double tmpb5 = BmOrTab(bm, "Bredd b5", bTab, 5);
            kv["Sumb5"] = "(b5) " + Fmt(tmpb5);
            kv["Sumb5Tol"] = "+ " + Fmt1(0.0);
            kv["Sumb5TolN"] = "- " + Fmt3(0.5);

            double tmpH = BmOrTab(bm, "Bredd H", bTab, 6);
            kv["SumH"] = "(H) " + Fmt(tmpH);
            kv["SumHTol"] = "+ " + Fmt3(0.5);
            kv["SumHTolN"] = "- " + Fmt1(0.0);

            // P — default 5
            double tmpP = GetDouble(bm, "Bredd P");
            if (tmpP == 0) tmpP = 5;
            kv["SumP"] = "2x (P) " + Fmt(tmpP);
            kv["SumPTol"] = "+ " + Fmt3(0.2);
            kv["SumPTolN"] = "- " + Fmt1(0.0);

            // E — default 16.129
            double tmpE = GetDouble(bm, "Bredd E");
            if (tmpE == 0) tmpE = 16.129;
            kv["SumE"] = "(E) " + Fmt(tmpE);
            kv["SumETol"] = "+ " + Fmt3(0.2);
            kv["SumETolN"] = "- " + Fmt1(0.0);

            // N — default 4.2
            double tmpN = GetDouble(bm, "Bredd N");
            if (tmpN == 0) tmpN = 4.2;
            kv["SumN"] = "(N) " + Fmt(tmpN);
            kv["SumNTol"] = "+ " + Fmt1(0.0);
            kv["SumNTolN"] = "- " + Fmt3(0.5);

            // K — always 4 (both branches in Lotus return 4)
            kv["SumK"] = "(K) 4";
            kv["SumKTol"] = "+ " + Fmt3(0.4);
            kv["SumKTolN"] = "- " + Fmt1(0.0);

            // L — hardcoded per type, NOT from BTabs
            double tmpL = TmpLMap.ContainsKey(tmpTyp) ? TmpLMap[tmpTyp] : 0;
            kv["SumL"] = "(L) " + (subject == "GR-TK 151 V" ? " " : Fmt(tmpL));

            // ── Fixed dimensions ──────────────────────────────────────────────────

            kv["SumBhd"] = "Ø 3";                       // TmpBhd = "3"
            kv["SumG"] = "(G) M6";                     // TmpG = 6
            kv["SumMi"] = "Gänga Min." + (tmpd < 105 ? "13" : "23");
            kv["SumMa"] = "Borr Max." + (tmpd < 105 ? "15" : "28");
            kv["SumBorrtext"] = "Genomgående hål, borr får ej beröra tätningsplan";
            kv["SumBhd1"] = "Ø 8 +0,1";                  // TmpBhd1 = 8
            kv["SumGn"] = (tmpd < 105 ? "5" : "15") + " ";
            kv["SumGnTol"] = "+0.5";
            kv["SumGnTolN"] = "-0.0";

            // ── Chamfers & radii ──────────────────────────────────────────────────

            kv["SumF25"] = "2.5x45° (2x)";
            kv["SumF1"] = "1x45°";
            kv["SumF3"] = tmpd < 400 ? "1X45°" : "3.0X45°";
            kv["SumF15"] = tmpd < 400 ? "1.5X45°" : "3.0X45°";
            kv["SumF65"] = "Gängfas Ø 6.5 +0.5";
            kv["SumR3"] = "R3";    // typ is 136-159 — not 24 or 29
            kv["SumR08"] = "6 x max: R0.8";
            kv["SumR05a"] = "0.5x45°";
            kv["SumR05b"] = tmpd < 400 ? "R0.5" : "R1.2";
            kv["SumR02"] = "max: R0.2";
            string r05bm = GetString(bm, "Radie R05");
            kv["SumR05"] = (string.IsNullOrEmpty(r05bm) || EqualsI(r05bm, "0")) ? "max: R0.5" : r05bm;
            kv["SumR1"] = "1x45°";

            // ── Form & surface ────────────────────────────────────────────────────

            kv["SumCo1"] = "0.15";   // leading space preserved from Lotus
            kv["SumCo2"] = kv["SumCo1"];
            kv["SumRd"] = "0.10";
            kv["SumRa32"] = "3.2";

            // ── Alignment marks ───────────────────────────────────────────────────

            kv["SumAm"] = "Inriktningsmärkning";
            kv["SumAmb"] = Fmt1(3.0).Replace(".",",");                  // TmpKon09 = @text(3.0;"F1") = "3.0"
            kv["SumAmbTol"] = "± " + Fmt3(0.2);
            kv["SumAmdjN"] = "Djup 1,0 + 0.25";         // Swedish comma preserved in output
            kv["SumAmdj"] = "+0.5";

            // ── Machine ───────────────────────────────────────────────────────────

            kv["SumMaskinvalS1"] = "Maskin: " + maskinVal + " - Diametrala mått ";
            kv["SumMaskinvalS2"] = "Maskin: " + maskinVal + " - Övriga mått";

            SumFrequencies(kv, maskinVal);
            SumMeasuringDevices(kv, maskinVal);
            SumAF(kv, maskinVal);

            // ── Ritning & text ────────────────────────────────────────────────────

            string ritBase = DTabs.ContainsKey(tmpTyp) ? "TK " + tmpTyp + " V" : "";
            kv["SumRitNr"] = ritBase + ": senaste utg,  märkning: 7433523:senaste utg.";
            kv["SumRitNr2"] = kv["SumRitNr"];

            string sumText = "Okulärkontroll Grader, frifläckar, slagmärken, repor, valkar, ytjämnhet och faser, skarpa kanter avgradas.";
            kv["SumTextS1"] = sumText;
            kv["SumTextS2"] = sumText;
            kv["SumTextS3"] = "Kontrollera stämpel på förstabit.";

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
            kv["SumF1_1"] = m ? "1/5" : "";
            kv["SumF1_2"] = m ? "1/5" : "";
            kv["SumF1_3"] = m ? "1/5" : "";
            kv["SumF1_4"] = "";
            kv["SumF1_5"] = "";
            kv["SumF1_6"] = m ? "Inst." : "";
            kv["SumF1_7"] = m ? "Inst. alt misstanke" : "";
            kv["SumF2_1"] = m ? "1/5" : "";
            kv["SumF2_2"] = m ? "1/5" : "";
            kv["SumF2_3"] = m ? "1/5" : "";
            kv["SumF2_4"] = m ? "1/5" : "";
            kv["SumF2_5"] = m ? "1/5" : "";
            kv["SumF2_6"] = m ? "Inst. alt misstanke" : "";
            kv["SumF2_7"] = m ? "1/5" : "";
        }

        private static void SumMeasuringDevices(Dictionary<string, string> kv, string mv)
        {
            bool m = IsMachine(mv);
            kv["SumD1_1"] = m ? "Mätmaskin Alt.Skjutmått" : "";
            kv["SumD1_2"] = m ? "Mätmaskin Alt.UD-apparat eller Mikrometer" : "";
            kv["SumD1_3"] = m ? "Mätmaskin Alt.Skjutmått" : "";
            kv["SumD1_4"] = "";
            kv["SumD1_5"] = "";
            kv["SumD1_6"] = m ? "Mätmaskin Alt.Mätservice" : "";
            kv["SumD1_7"] = m ? "Ytjämnhetsmätare" : "";
            kv["SumD2_1"] = m ? "Mätmaskin Alt.Skjutmått" : "";
            kv["SumD2_2"] = m ? "Mätmaskin Alt.Skjutmått" : "";
            kv["SumD2_3"] = m ? "Mätmaskin Alt.Skjutmått" : "";
            kv["SumD2_4"] = m ? "Gängtolk" : "";
            kv["SumD2_5"] = m ? "Mätmaskin Alt.Skjutmått" : "";
            kv["SumD2_6"] = m ? "Ytjämnhetsmätare" : "";
            kv["SumD2_7"] = m ? "Mätmaskin Alt.Skjutmått" : "";
        }

        private static void SumAF(Dictionary<string, string> kv, string mv)
        {
            for (int i = 1; i <= 7; i++) { kv["SumAF1_" + i] = ""; kv["SumAF2_" + i] = ""; }
        }

        // ── H12 tolerance (21 steps) ──────────────────────────────────────────────
        private static double H12Tol(double v)
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

        // ── Utilities ─────────────────────────────────────────────────────────────
        private static double BmOrTab(List<Bookmark> bm, string key, double[] tab, int oneBasedIdx)
        {
            double v = GetDouble(bm, key);
            if (v != 0) return v;
            if (tab == null || oneBasedIdx < 1 || oneBasedIdx > tab.Length) return 0;
            return tab[oneBasedIdx - 1];
        }

        private static int GetMember(string val, string[] list)
        {
            for (int i = 0; i < list.Length; i++)
                if (string.Equals(list[i], val, StringComparison.OrdinalIgnoreCase)) return i + 1;
            return 0;
        }

        private static bool IsMachine(string mv)
        {
            if (string.IsNullOrEmpty(mv)) return false;
            for (int i = 0; i < Machines.Length; i++)
                if (string.Equals(Machines[i], mv, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        private static bool EqualsI(string a, string b)
            => string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);

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