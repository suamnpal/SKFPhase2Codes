
using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;



namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class KRAGAR_SL_TK_V_INCH_LARGE_TVA_FINGRAR : ITemplateCalculations
    {
        private static readonly string[] Machines = new[] { "Nakamura", "LB45", "LT-3000EX" };



        // D tables: [d1, d2, d3, d4, d7, d8, d9, d10, d11, d12, Da(unused)]
        private static readonly double[] DTab606 = { 284, 275.11, 265.11, 281, 332, 340, 358, 366, 384, 398, 16 };
        private static readonly double[] DTab884 = { 320, 314.8, 304.8, 321, 372, 380, 398, 406, 424, 438, 16 };



        // B tables: [B, b1, b2, b3, b4, b5, b6] — both types identical
        private static readonly double[] BTabBoth = { 36, 15.5, 10.5, 4, 8, 9, 26 };



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



            // NOTE: Explode delimiter is " /." — NO minus, NO dash
            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : string.Empty;
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : string.Empty;
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : string.Empty;



            int cntB3 = tmpBet3.Length;
            string tmpTypStr = cntB3 > 2 ? tmpBet3 : tmpBet2;
            double typNum = TryParseDouble(tmpTypStr);



            double[] dList = typNum == 606 ? DTab606 : typNum == 884 ? DTab884 : null;
            double[] bList = BTabBoth;



            // ── Diameters ────────────────────────────────────────────────────────



            double tmpd1 = BmOrList(bm, "Ø d1", dList, 1);
            kv["Sumd1"] = "(d1) " + Fmt(tmpd1);
            kv["Sumd1Tol"] = "+ " + Fmt1(4.0);
            kv["Sumd1TolN"] = "- " + Fmt1(0.0);



            double tmpd2 = BmOrList(bm, "Ø d2", dList, 2);
            kv["Sumd2"] = "(d2) " + Fmt(tmpd2);
            kv["Sumd2Tol"] = "+ " + Fmt3(H12Tol(tmpd2));
            kv["Sumd2TolN"] = "- " + Fmt1(0.0);



            double tmpd3 = BmOrList(bm, "Ø d3", dList, 3);
            kv["Sumd3"] = "(d3) " + Fmt(tmpd3);
            kv["Sumd3Tol"] = "+ " + Fmt3(0.2);
            kv["Sumd3TolN"] = "+ " + Fmt3(0.1);



            double tmpd4 = BmOrList(bm, "Ø d4", dList, 4);
            kv["Sumd4"] = "(d4) " + Fmt(tmpd4);
            kv["Sumd4Tol"] = "+ " + Fmt1(0.0);
            kv["Sumd4TolN"] = "- " + Fmt3(H12TolShort(tmpd4));



            double tmpd7 = BmOrList(bm, "Ø d7", dList, 5);
            kv["Sumd7"] = "(d7) " + Fmt(tmpd7);
            kv["Sumd7Tol"] = "+ " + Fmt3(H12Tol(tmpd7));
            kv["Sumd7TolN"] = "- " + Fmt1(0.0);



            double tmpd8 = BmOrList(bm, "Ø d8", dList, 6);
            kv["Sumd8"] = "(d8) " + Fmt(tmpd8);
            kv["Sumd8Tol"] = "+ " + Fmt1(0.0);
            kv["Sumd8TolN"] = "- " + Fmt3(H12TolShort(tmpd8));



            double tmpd9 = BmOrList(bm, "Ø d9", dList, 7);
            kv["Sumd9"] = "(d9) " + Fmt(tmpd9);
            kv["Sumd9Tol"] = "+ " + Fmt3(H12Tol(tmpd9));
            kv["Sumd9TolN"] = "- " + Fmt1(0.0);



            double tmpd10 = BmOrList(bm, "Ø d10", dList, 8);
            kv["Sumd10"] = "(d10) " + Fmt(tmpd10);
            kv["Sumd10Tol"] = "+ " + Fmt1(0.0);
            kv["Sumd10TolN"] = "- " + Fmt3(H12TolShort(tmpd10));



            double tmpd11 = BmOrList(bm, "Ø d11", dList, 9);
            kv["Sumd11"] = "(d11) " + Fmt(tmpd11);
            kv["Sumd11Tol"] = "+ " + Fmt3(H12Tol(tmpd11));
            kv["Sumd11TolN"] = "- " + Fmt1(0.0);



            double tmpd12 = BmOrList(bm, "Ø d12", dList, 10);
            kv["Sumd12"] = "(d12) " + Fmt(tmpd12);
            kv["Sumd12Tol"] = "± " + Fmt1(3.0);



            double tmpd13 = typNum < 888 ? 16 : 18;
            kv["Sumd13"] = typNum > 1000 ? "n/a" : "3x Ø " + Fmt(tmpd13);



            // ── Widths ───────────────────────────────────────────────────────────



            double tmpB = BmOrList(bm, "Bredd B", bList, 1);
            kv["SumB"] = "(B) " + Fmt(tmpB);
            kv["SumBTol"] = "+ " + Fmt1(1.0);
            kv["SumBTolN"] = "+ " + Fmt1(0.0);  // NOTE: "+" prefix for TolN — Lotus quirk



            double tmpb1 = BmOrList(bm, "Bredd b1", bList, 2);
            kv["Sumb1"] = "(b1) " + Fmt(tmpb1);
            kv["Sumb1Tol"] = "± " + Fmt1(1.0);



            double tmpb2 = BmOrList(bm, "Bredd b2", bList, 3);
            kv["Sumb2"] = "(b2) " + Fmt(tmpb2);
            kv["Sumb2Tol"] = "+ " + Fmt3(0.5);
            kv["Sumb2TolN"] = "- " + Fmt1(0.0);



            double tmpb3 = BmOrList(bm, "Bredd b3", bList, 4);
            kv["Sumb3"] = "(b3) " + Fmt(tmpb3);
            kv["Sumb3Tol"] = "+ " + Fmt3(0.3);
            kv["Sumb3TolN"] = "- " + Fmt1(0.0);



            double tmpb4 = BmOrList(bm, "Bredd b4", bList, 5);
            kv["Sumb4"] = "(b4) " + Fmt(tmpb4);
            kv["Sumb4Tol"] = "+ " + Fmt3(0.25);
            kv["Sumb4TolN"] = "- " + Fmt1(0.0);



            double tmpb5 = BmOrList(bm, "Bredd b5", bList, 6);
            kv["Sumb5"] = "(b5) " + Fmt(tmpb5);
            kv["Sumb5Tol"] = "± " + Fmt1(1.0);



            double tmpb6 = BmOrList(bm, "Bredd b6", bList, 7);
            kv["Sumb6"] = "3x (b6) " + Fmt(tmpb6);
            kv["Sumb6Tol"] = "+ " + Fmt1(0.0);
            kv["Sumb6TolN"] = "- " + Fmt3(0.5);



            double b7Bm = GetDouble(bm, "Bredd b7");
            double tmpb7 = b7Bm != 0 ? b7Bm : (typNum < 44 ? 9.0 : 14.5);
            kv["Sumb7"] = "(b7) " + Fmt(tmpb7);
            kv["Sumb7Tol"] = "+ " + Fmt3(0.4);
            kv["Sumb7TolN"] = "- " + Fmt1(0.0);



            // ── Thread, radii, chamfers ──────────────────────────────────────────



            double tmpG = typNum < 44 ? 8 : (typNum < 888 ? 10 : 12);
            kv["SumG"] = "(G) M" + Fmt(tmpG) + " (3x)";



            kv["SumR08"] = "R max: 0.8 (6x)";
            kv["SumR05"] = "R0.5 (2x)";
            kv["SumR05a"] = "max R0.5 (3x)";
            kv["SumR4"] = "R4";



            double tmpF1 = typNum < 978 ? 1.5 : 2.0;
            kv["SumF1"] = "(2x) " + Fmt(tmpF1) + " x45°";
            kv["SumF2"] = "(2x) 1x45°";



            // ── Form & position ──────────────────────────────────────────────────



            kv["SumCo"] = Fmt3(0.15);
            kv["SumCo1"] = tmpd1 < 301 ? Fmt3(0.15) : Fmt3(0.25);



            // ── Ra ───────────────────────────────────────────────────────────────



            kv["SumRa32"] = "3.2";
            kv["SumRa125"] = tmpd1 > 400 ? "n/a" : "12.5";



            // ── Machine / freq / devices / AF ────────────────────────────────────



            kv["SumMaskinvalS1"] = "Maskin: " + maskinVal + " - Diametrala mått.";
            kv["SumMaskinvalS2"] = "Maskin: " + maskinVal + " - Övriga mått.";



            SumFrequencies(kv, maskinVal);
            SumMeasuringDevices(kv, maskinVal);
            SumAF(kv, maskinVal);



            // ── Ritning ──────────────────────────────────────────────────────────



            string ritBase = typNum == 606 ? "TK 606 V" : (typNum == 884 ? "TK 884 V" : "");
            kv["SumRitNr"] = ritBase + ": senaste utgåva";
            kv["SumRitNr2"] = kv["SumRitNr"];



            // ── Text ─────────────────────────────────────────────────────────────



            string sumText = "Okulärkontroll Grader, frifläckar, slagmärken, repor, valkar, ytjämnhet och faser, skarpa kanter avgradas.";
            kv["SumTextS1"] = sumText;
            kv["SumTextS2"] = sumText;



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



        // ── Freq / Devices / AF ──────────────────────────────────────────────────
        private static void SumFrequencies(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsMachine(maskinVal);
            kv["SumF1_1"] = m ? "1/2" : "";
            kv["SumF1_2"] = m ? "1/2" : "";
            kv["SumF1_3"] = m ? "1/2" : "";
            kv["SumF1_4"] = m ? "1/2" : "";
            kv["SumF1_5"] = "";
            kv["SumF1_6"] = m ? "1/2" : "";
            kv["SumF1_7"] = m ? "1/2" : "";
            kv["SumF2_1"] = m ? "1/2" : "";
            kv["SumF2_2"] = m ? "1/2" : "";
            kv["SumF2_3"] = m ? "1/2" : "";
            kv["SumF2_4"] = m ? "1/2" : "";
            kv["SumF2_5"] = m ? "1/2" : "";
            kv["SumF2_6"] = m ? "1/2" : "";
            kv["SumF2_7"] = m ? "1/2" : "";
        }



        private static void SumMeasuringDevices(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsMachine(maskinVal);
            kv["SumD1_1"] = m ? "Mätmaskin alt.Skjutmått" : "";
            kv["SumD1_2"] = m ? "Mätmaskin alt.UD-apparat eller Mikrometer" : "";
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



        private static void SumAF(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsMachine(maskinVal);
            kv["SumAF1_1"] = "";
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = m ? "Övriga bearbetade ytor 6.3" : "";
            kv["SumAF1_5"] = "";
            kv["SumAF1_6"] = "";
            kv["SumAF1_7"] = "";
            kv["SumAF2_1"] = "";
            kv["SumAF2_2"] = "";
            kv["SumAF2_3"] = "";
            kv["SumAF2_4"] = "";
            kv["SumAF2_5"] = "";
            kv["SumAF2_6"] = m ? "Övriga bearbetade ytor 6.3" : "";
            kv["SumAF2_7"] = "";
        }



        // ── Tolerance helpers ────────────────────────────────────────────────────



        // H12 / h12 full 21-step table (for d2, d7, d9, d11 + prefix)
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



        // h12 short 14-step table (for d4, d8, d10 — - prefix, stops at 630)
        private static double H12TolShort(double v)
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



        // ── Utilities ────────────────────────────────────────────────────────────



        private static double BmOrList(List<Bookmark> bm, string key, double[] list, int oneBasedIdx)
        {
            double v = GetDouble(bm, key);
            if (v != 0) return v;
            if (list == null || oneBasedIdx < 1 || oneBasedIdx > list.Length) return 0;
            return list[oneBasedIdx - 1];
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

