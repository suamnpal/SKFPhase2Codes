using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class KRAGAR_SL_TK_V_INCH_SMALL_24_130 : ITemplateCalculations
    {
        private static readonly string[] Machines = new[] { "Nakamura", "LB45", "LT-3000EX" };

        private static readonly string[] Types = {
            "24","29","37","43","44","53","53/75","54","188","102","109","113","117","118","119","122","125","130"
        };

        // D tables: [d, d1, d2, d3, d4, d5, d6, d7, d8, d9, d12, Da]  (12 elements, index 1-12)
        private static readonly Dictionary<string, double[]> DTabs = new Dictionary<string, double[]>(StringComparer.OrdinalIgnoreCase)
        {
            ["24"] = new double[] { 49.21, 58, 55.2, 67, 71.5, 82.5, 88.5, 99.5, 105.5, 116.5, 8, 123 },
            ["29"] = new double[] { 55.56, 60, 61.6, 72, 76.5, 87.5, 93.5, 104.5, 110.5, 121.5, 8, 128 },
            ["37"] = new double[] { 61.91, 65, 67.9, 77, 81.5, 92.5, 98.5, 109.5, 115.5, 126.5, 8, 133 },
            ["43"] = new double[] { 66.68, 70.5, 72.7, 82, 86.5, 97.5, 103.5, 114.5, 120.5, 131.5, 6, 138 },
            ["44"] = new double[] { 68.26, 76, 74.3, 91, 96, 107, 113, 124, 130, 141, 14, 149 },
            ["53"] = new double[] { 74.61, 82, 80.6, 96, 101.5, 112.5, 118.5, 129.5, 135.5, 146.5, 14, 154.5 },
            ["53/75"] = new double[] { 75, 82, 81, 96, 101.5, 112.5, 118.5, 129.5, 135.5, 146.5, 14, 154.5 },
            ["54"] = new double[] { 76.2, 82, 82.2, 96, 101.5, 112.5, 118.5, 129.5, 135.5, 146.5, 14, 154.5 },
            ["188"] = new double[] { 80.96, 88, 87, 101, 107.5, 118.5, 124.5, 135.5, 141.5, 152.5, 12, 160.5 },
            ["102"] = new double[] { 87.31, 97, 93.3, 111, 116, 128, 134, 146, 152, 164, 8, 172 },
            ["109"] = new double[] { 100.01, 107, 106, 121, 126, 138, 144, 156, 162, 174, 10, 182 },
            ["113"] = new double[] { 106.36, 117, 112.4, 131, 139, 152, 159, 172, 179, 192, 10, 201 },
            ["117"] = new double[] { 112.71, 118.5, 118.7, 131, 139, 152, 159, 172, 179, 192, 8, 201 },
            ["118"] = new double[] { 114.3, 122, 120.3, 136, 144, 158, 166, 180, 188, 202, 8, 212 },
            ["119"] = new double[] { 115.89, 122, 122, 136, 144, 158, 166, 180, 188, 202, 8, 212 },
            ["122"] = new double[] { 125.41, 133, 131.4, 146, 156, 170, 178, 192, 200, 214, 12, 224 },
            ["125"] = new double[] { 131.76, 137, 137.8, 146, 156, 170, 178, 192, 200, 214, 12, 224 },
            ["130"] = new double[] { 138.11, 147, 144.1, 161, 171, 185, 193, 207, 215, 229, 10, 239 },
        };

        // B tables: [b1, b2, b3, b4, b5, b6, Ba]  (7 elements, index 1-7)
        private static readonly Dictionary<string, double[]> BTabs = new Dictionary<string, double[]>(StringComparer.OrdinalIgnoreCase)
        {
            ["24"] = new double[] { 9, 5, 5, 20, 9, 13.5, 25 },
            ["29"] = new double[] { 9, 5.5, 5.5, 19.5, 8.5, 13, 25 },
            ["37"] = new double[] { 9, 5.5, 5, 19.5, 8.5, 13, 25 },
            ["43"] = new double[] { 9, 6.8, 6.8, 18.5, 7, 11.5, 23.5 },
            ["44"] = new double[] { 12, 6.8, 6.8, 27, 10.5, 15, 33 },
            ["53"] = new double[] { 12, 6.8, 6.8, 27, 10.5, 15, 33 },
            ["53/75"] = new double[] { 12, 6.8, 6.8, 27, 10.5, 15, 33 },
            ["54"] = new double[] { 12, 6.8, 6.8, 27, 10.5, 15, 33 },
            ["188"] = new double[] { 12, 6.8, 6.8, 27, 10.5, 15, 31.5 },
            ["102"] = new double[] { 10, 6.8, 7.3, 22, 8.5, 13, 28 },
            ["109"] = new double[] { 12.5, 8, 6.5, 25, 10, 14.5, 31 },
            ["113"] = new double[] { 12.5, 8, 8, 25.5, 10, 14.5, 31.5 },
            ["117"] = new double[] { 12, 8, 8, 25.5, 10, 14.5, 31.5 },
            ["118"] = new double[] { 12, 7.9, 7.9, 25, 10, 14.5, 31 },
            ["119"] = new double[] { 12, 7.9, 7.9, 25, 10, 14.5, 31 },
            ["122"] = new double[] { 12, 7.9, 7.9, 27.5, 10, 14.5, 33.5 },
            ["125"] = new double[] { 12.5, 7.9, 7.9, 27.5, 10, 14.5, 33.5 },
            ["130"] = new double[] { 12.5, 7.9, 8, 27.5, 10, 14.5, 34.5 },
        };

        // V (fräsning depth) list: 18 values, 1-based matching Types array
        private static readonly double[] VList = { 1, 1, 1, 2, 0, 0, 0, 0, 0, 1, 1, 0.1, 0.1, 0.5, 0.5, 0, 0, 0 };

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

            // NOTE: Explode delimiter is " /." — no minus, no dash
            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : string.Empty;
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : string.Empty;
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : string.Empty;

            // TmpTyp: if Bet3 IS a non-zero number -> "Bet2/Bet3" (e.g. "53/75")
            //         else -> just "Bet2" (normal case, Bet3 empty or missing)
            // Lotus: "[TmpBet3c]"="0" means IsNumber=false (not a number) -> use just Bet2
            //        "[TmpBet3c]"!="0" means IsNumber=true (is a number) -> "Bet2/Bet3"
            // Null Bet3 produces IsNumber(0)=true BUT Bet3a="" so TmpBet3="" -> "53/" breaks lookup
            // Correct guard: Bet3 must be non-empty AND parse as non-zero number
            bool bet3IsNum = !string.IsNullOrEmpty(tmpBet3) && TryParseDouble(tmpBet3) != 0;
            string tmpTyp = bet3IsNum ? tmpBet2 + "/" + tmpBet3 : tmpBet2;

            int typLista = GetMember(tmpTyp, Types);  // 1-based; 0 = not found

            double[] dTab = DTabs.ContainsKey(tmpTyp) ? DTabs[tmpTyp] : null;
            double[] bTab = BTabs.ContainsKey(tmpTyp) ? BTabs[tmpTyp] : null;
            double tmpV = (typLista >= 1 && typLista <= VList.Length) ? VList[typLista - 1] : 0;
            bool hasV = tmpV != 0;

            // ── Diameters ────────────────────────────────────────────────────────

            double tmpd = BmOrTab(bm, "Ø d", dTab, 1);
            kv["Sumd"] = "(d) " + Fmt(tmpd);
            kv["SumdTol"] = "+ " + Fmt3(0.2);
            kv["SumdTolN"] = "+ " + Fmt3(0.1);  // NOTE: "+" prefix — both tol and tolN use "+"

            double tmpd1 = BmOrTab(bm, "Ø d1", dTab, 2);
            kv["Sumd1"] = "(d1) " + Fmt(tmpd1);
            kv["Sumd1Tol"] = "+ " + Fmt1(0.0);
            kv["Sumd1TolN"] = "- " + Fmt3(H12ShortTol(tmpd1));

            double tmpd2 = BmOrTab(bm, "Ø d2", dTab, 3);
            kv["Sumd2"] = "(d2) " + Fmt(tmpd2);
            kv["Sumd2Tol"] = "+ " + Fmt3(H12Tol14(tmpd2));
            kv["Sumd2TolN"] = "- " + Fmt1(0.0);

            double tmpd3 = BmOrTab(bm, "Ø d3", dTab, 4);
            kv["Sumd3"] = "(d3) " + Fmt(tmpd3);
            kv["Sumd3Tol"] = "+ " + Fmt3(4.0);
            kv["Sumd3TolN"] = "- " + Fmt1(0.0);

            double tmpd4 = BmOrTab(bm, "Ø d4", dTab, 5);
            kv["Sumd4"] = "(d4) " + Fmt(tmpd4);
            kv["Sumd4Tol"] = "+ " + Fmt1(0.0);
            kv["Sumd4TolN"] = "- " + Fmt3(H12TolShort14(tmpd4));

            double tmpd5 = BmOrTab(bm, "Ø d5", dTab, 6);
            kv["Sumd5"] = "(d5) " + Fmt(tmpd5);
            kv["Sumd5Tol"] = "+ " + Fmt3(H12TolShort14(tmpd5));
            kv["Sumd5TolN"] = "- " + Fmt1(0.0);

            double tmpd6 = BmOrTab(bm, "Ø d6", dTab, 7);
            kv["Sumd6"] = "(d6) " + Fmt(tmpd6);
            kv["Sumd6Tol"] = "+ " + Fmt1(0.0);
            kv["Sumd6TolN"] = "- " + Fmt3(H12TolShort14(tmpd6));

            double tmpd7 = BmOrTab(bm, "Ø d7", dTab, 8);
            kv["Sumd7"] = "(d7) " + Fmt(tmpd7);
            kv["Sumd7Tol"] = "+ " + Fmt3(H12TolShort14(tmpd7));
            kv["Sumd7TolN"] = "- " + Fmt1(0.0);

            double tmpd8 = BmOrTab(bm, "Ø d8", dTab, 9);
            kv["Sumd8"] = "(d8) " + Fmt(tmpd8);
            kv["Sumd8Tol"] = "+ " + Fmt1(0.0);
            kv["Sumd8TolN"] = "- " + Fmt3(H12TolShort14(tmpd8));

            double tmpd9 = BmOrTab(bm, "Ø d9", dTab, 10);
            kv["Sumd9"] = "(d9) " + Fmt(tmpd9);
            kv["Sumd9Tol"] = "+ " + Fmt3(H12TolShort14(tmpd9));
            kv["Sumd9TolN"] = "- " + Fmt1(0.0);

            double tmpd12 = GetTabVal(dTab, 11);  // no bookmark override
            kv["Sumd12"] = "(d12) 3x Ø" + Fmt(tmpd12);

            double tmpDa = BmOrTab(bm, "Ø Da", dTab, 12);
            kv["SumDa"] = "(Da) " + Fmt(tmpDa);
            kv["SumDaTol"] = "± " + Fmt3(3.0);

            // ── Widths ───────────────────────────────────────────────────────────

            double tmpb = GetDouble(bm, "Bredd b");
            if (tmpb == 0) tmpb = 4.3;   // fixed default
            kv["Sumb"] = "(b) " + Fmt(tmpb);
            kv["SumbTol"] = "+ " + Fmt3(0.25);
            kv["SumbTolN"] = "- " + Fmt1(0.0);

            double tmpb1 = BmOrTab(bm, "Bredd b1", bTab, 1);
            kv["Sumb1"] = "(b1) " + Fmt(tmpb1);
            kv["Sumb1Tol"] = "± " + Fmt3(1.0);

            double tmpb2 = BmOrTab(bm, "Bredd b2", bTab, 2);
            kv["Sumb2"] = "(b2) " + Fmt(tmpb2);
            kv["Sumb2Tol"] = "+ " + Fmt3(0.2);
            kv["Sumb2TolN"] = "- " + Fmt1(0.0);

            double tmpb3 = BmOrTab(bm, "Bredd b3", bTab, 3);
            kv["Sumb3"] = "(b3) " + Fmt(tmpb3);
            kv["Sumb3Tol"] = "+ " + Fmt3(0.2);
            kv["Sumb3TolN"] = "- " + Fmt1(0.0);

            double tmpb4 = BmOrTab(bm, "Bredd b4", bTab, 4);
            kv["Sumb4"] = "3x (b4) " + Fmt(tmpb4);
            kv["Sumb4Tol"] = "+ " + Fmt1(0.0);
            kv["Sumb4TolN"] = "- " + Fmt3(0.5);

            double tmpb5 = BmOrTab(bm, "Bredd b5", bTab, 5);
            kv["Sumb5"] = "(b5) " + Fmt(tmpb5);
            kv["Sumb5Tol"] = "+ " + Fmt3(0.5);
            kv["Sumb5TolN"] = "- " + Fmt1(0.0);

            double tmpb6 = BmOrTab(bm, "Bredd b6", bTab, 6);
            kv["Sumb6"] = "(b6) " + Fmt(tmpb6);
            kv["Sumb6Tol"] = "± " + Fmt3(1.0);

            double tmpBa = BmOrTab(bm, "Bredd Ba", bTab, 7);
            kv["SumBa"] = "(Ba) " + Fmt(tmpBa);
            kv["SumBaTol"] = "+ " + Fmt3(1.0);
            kv["SumBaTolN"] = "- " + Fmt1(0.0);

            // ── Thread, radii, chamfers ──────────────────────────────────────────

            double typNum = TryParseDouble(tmpBet2);
            double tmpG1 = typNum < 44 ? 5 : 6;
            if (subject == "SL-TK 53/75 V")
                tmpG1 = 5;
            kv["SumG1"] = "3x (G1) M" + Fmt(tmpG1);
            kv["SumR08"] = "R max: 0.8 (7x)";
            kv["SumR05"] = "R0.5 (2x)";
            kv["SumR3"] = "R3";
            kv["SumF15"] = "1.5x45° (3x)";
            kv["SumF1"] = "1x45° (2x)";

            // ── Fräsning ─────────────────────────────────────────────────────────

            string tmpVstr = hasV ? Fmt(tmpV) : "0";
            kv["SumV"] = hasV ? "(V) " + tmpVstr : "";
            kv["SumVf"] = hasV ? "3 x Ø10" : "";

            // ── Concentricity & Ra ───────────────────────────────────────────────

            kv["SumCo1"] = "0.15";   // NOTE: leading space preserved from Lotus
            kv["SumCo2"] = kv["SumCo1"];
            kv["SumRa32"] = "3.2";

            // ── Alignment mark ───────────────────────────────────────────────────

            kv["SumAm"] = "* 1";

            // ── Machine ───────────────────────────────────────────────────────────

            kv["SumMaskinvalS1"] = "Maskin: " + maskinVal + " - Diametrala mått.";
            kv["SumMaskinvalS2"] = "Maskin: " + maskinVal + " - Bredd mått.";

            bool m = IsMachine(maskinVal);
            kv["SumM2_8"] = m ? (hasV ? "Fräsning" : "") : "";
            kv["SumB2_8"] = m ? (hasV ? "V" : "") : "";

            SumFrequencies(kv, maskinVal, hasV);
            SumMeasuringDevices(kv, maskinVal, hasV);
            SumAF(kv, maskinVal, hasV);

            // ── Ritning & text ────────────────────────────────────────────────────

            kv["SumRitNr"] = "11K 7433542: senaste utgåva";
            kv["SumRitNr2"] = kv["SumRitNr"];

            string sumText = "Okulärkontroll Grader, frifläckar, slagmärken, repor, valkar, ytjämnhet och faser, skarpa kanter avgradas.";
            kv["SumTextS1"] = sumText;
            kv["SumTextS2"] = sumText;

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

        private static void SumFrequencies(Dictionary<string, string> kv, string maskinVal, bool hasV)
        {
            bool m = IsMachine(maskinVal);
            kv["SumF1_1"] = m ? "1/5" : "";
            kv["SumF1_2"] = m ? "1/5" : "";
            kv["SumF1_3"] = m ? "1/5" : "";
            kv["SumF1_4"] = "";
            kv["SumF1_5"] = "";
            kv["SumF1_6"] = m ? "Inst." : "";
            kv["SumF1_7"] = m ? "1/5" : "";
            kv["SumF2_1"] = m ? "1/5" : "";
            kv["SumF2_2"] = m ? "1/5" : "";
            kv["SumF2_3"] = m ? "1/5" : "";
            kv["SumF2_4"] = m ? "1/5" : "";
            kv["SumF2_5"] = m ? "1/5" : "";
            kv["SumF2_6"] = m ? "1/5" : "";
            kv["SumF2_7"] = m ? "1/5" : "";
            kv["SumF2_8"] = m ? (hasV ? "Inst." : "") : "";
        }

        private static void SumMeasuringDevices(Dictionary<string, string> kv, string maskinVal, bool hasV)
        {
            bool m = IsMachine(maskinVal);
            kv["SumD1_1"] = m ? "Mätmaskin Alt.Skjutmått" : "";
            kv["SumD1_2"] = m ? "Mätmaskin Alt.UD-Apparat" : "";
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
            kv["SumD2_8"] = m ? (hasV ? "Mätmaskin Alt.Skjutmått" : "") : "";
        }

        private static void SumAF(Dictionary<string, string> kv, string maskinVal, bool hasV)
        {
            bool m = IsMachine(maskinVal);
            for (int i = 1; i <= 7; i++) kv["SumAF1_" + i] = "";
            for (int i = 1; i <= 7; i++) kv["SumAF2_" + i] = "";
            kv["SumAF2_8"] = m ? (hasV ? "Fräsningen skall endast tangera råytan" : "") : "";
        }

        // ── Tolerance tables ─────────────────────────────────────────────────────

        // h12 for d1 — full 21-step (note: quirk at 630 range drops back to 0.440)
        private static double H12ShortTol(double v)
        {
            if (v < 3.01) return 0.100; if (v < 6.01) return 0.120;
            if (v < 10.01) return 0.150; if (v < 18.01) return 0.180;
            if (v < 30.01) return 0.210; if (v < 50.01) return 0.250;
            if (v < 80.01) return 0.300; if (v < 120.01) return 0.350;
            if (v < 180.01) return 0.400; if (v < 250.01) return 0.460;
            if (v < 315.01) return 0.520; if (v < 400.01) return 0.570;
            if (v < 500.01) return 0.630;
            if (v < 630.01) return 0.440;  // Lotus quirk: drops back for 500-630 range
            if (v < 800.01) return 0.500; if (v < 1000.01) return 0.560;
            if (v < 1250.01) return 0.660; if (v < 1600.01) return 0.780;
            if (v < 2000.01) return 0.920; if (v < 2500.01) return 1.100;
            return 1.350;
        }

        // H12 for d2 — 14-step table (stops at 0.720)
        private static double H12Tol14(double v)
        {
            if (v < 3.01) return 0.100; if (v < 6.01) return 0.120;
            if (v < 10.01) return 0.150; if (v < 18.01) return 0.180;
            if (v < 30.01) return 0.210; if (v < 50.01) return 0.250;
            if (v < 80.01) return 0.300; if (v < 120.01) return 0.350;
            if (v < 180.01) return 0.400; if (v < 250.01) return 0.460;
            if (v < 315.01) return 0.520; if (v < 400.01) return 0.570;
            if (v < 500.01) return 0.630;
            return 0.720;
        }

        // h12 for d4/d5/d6/d7/d8/d9 — 14-step (stops at 0.700)
        private static double H12TolShort14(double v)
        {
            if (v < 3.01) return 0.100; if (v < 6.01) return 0.120;
            if (v < 10.01) return 0.150; if (v < 18.01) return 0.180;
            if (v < 30.01) return 0.210; if (v < 50.01) return 0.250;
            if (v < 80.01) return 0.300; if (v < 120.01) return 0.350;
            if (v < 180.01) return 0.400; if (v < 250.01) return 0.460;
            if (v < 315.01) return 0.520; if (v < 400.01) return 0.570;
            if (v < 500.01) return 0.630;
            return 0.700;
        }

        // ── Utilities ─────────────────────────────────────────────────────────────

        private static double BmOrTab(List<Bookmark> bm, string key, double[] tab, int oneBasedIdx)
        {
            double v = GetDouble(bm, key);
            if (v != 0) return v;
            return GetTabVal(tab, oneBasedIdx);
        }

        private static double GetTabVal(double[] tab, int oneBasedIdx)
        {
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