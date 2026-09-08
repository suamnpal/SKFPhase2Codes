using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class RINGAR_SV_TSD_VZ2M4_SPECIAL_OP1_2 : ITemplateCalculations
    {
        private static readonly string[] Machines = { "Nakamura", "MaxMuller/Skepp6" };

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
            bool tmpVZ2M4 = tmpBet.IndexOf("VZ2M4", StringComparison.OrdinalIgnoreCase) >= 0;
            bool tmpVZ2L1 = tmpBet.IndexOf("VZ2L1", StringComparison.OrdinalIgnoreCase) >= 0;
            bool tmpVZ = tmpBet.IndexOf("VZ861", StringComparison.OrdinalIgnoreCase) >= 0;
            bool tmpV21 = tmpBet.IndexOf("V21", StringComparison.OrdinalIgnoreCase) >= 0;
            bool tmpG = tmpBet.IndexOf("G", StringComparison.OrdinalIgnoreCase) >= 0;

            // Tmp5OB — exact product check (original uses @Contains on Subject, not TmpBet)
            bool tmp5OB = subject.IndexOf("SV-TSD 3040-2 U/VZ2M4", StringComparison.OrdinalIgnoreCase) >= 0;
            kv["SumDOBText"] = tmp5OB ? "OBS Produkten har 5 borrade hål" : "";

            // Explode " /." (space, slash, dot) — dash is NOT a separator
            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : "";
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : "";
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : "";
            string tmpBet4 = tokens.Length > 3 ? tokens[3] : "";

            // ── All dimensions from bookmarks ─────────────────────────────────────
            double tmpD = GetDouble(bm, "Ø D");
            double tmpD1 = GetDouble(bm, "Ø D1");
            double tmpD2 = GetDouble(bm, "Ø D2");
            double tmpD3 = GetDouble(bm, "Ø D3");
            double tmpD4 = GetDouble(bm, "Ø D4");
            double tmpD5 = GetDouble(bm, "Ø D5");
            double tmpBv = GetDouble(bm, "Bredd (B)");
            double tmpB1 = GetDouble(bm, "Bredd (B1)");
            double tmpB2 = GetDouble(bm, "Bredd (B2)");
            double tmpB3 = GetDouble(bm, "Bredd (B3)");
            double tmpB4 = GetDouble(bm, "Bredd (B4)");
            double tmpBD = GetDouble(bm, "Borrdjup (BD)");
            double tmpDOB = GetDouble(bm, "Ø Oljeborrhål (DOB)");
            double tmpVOB = GetDouble(bm, "Vinkel Oljeborrhål (VOB)");
            double tmpBMbm = GetDouble(bm, "Vinkel borrhål (BM)");

            // TmpBM = 5 (fixed)
            const double tmpBM = 5.0;

            // ── D (utvändig diameter) — symmetric GenTol ─────────────────────────
            kv["SumD"] = "(D) " + Fmt(tmpD);
            kv["SumDTol"] = "+ " + Fmt(GenTol(tmpD));
            kv["SumDTolN"] = "- " + Fmt(GenTol(tmpD));

            // ── D1 — ± GenTol ─────────────────────────────────────────────────────
            kv["SumD1"] = "(D1) " + Fmt(tmpD1);
            kv["SumD1Tol"] = "± " + Fmt(GenTol(tmpD1));

            // ── D2 (invändig) — h8 tol ───────────────────────────────────────────
            kv["SumD2"] = "(D2) " + Fmt(tmpD2);
            kv["SumD2Tol"] = "+ 0 [2]";
            kv["SumD2TolN"] = "- " + Fmt(H8Tol(tmpD2)) + " [3]";

            // ── D3 — ± GenTol ─────────────────────────────────────────────────────
            kv["SumD3"] = "(D3) " + Fmt(tmpD3);
            kv["SumD3Tol"] = "± " + Fmt(GenTol(tmpD3));

            // ── D4 — H12 tol ──────────────────────────────────────────────────────
            kv["SumD4"] = "(D4) " + Fmt(tmpD4);
            kv["SumD4Tol"] = "+ " + Fmt(H12Tol(tmpD4));
            kv["SumD4TolN"] = "- 0" ;

            // ── D5 — symmetric GenTol ─────────────────────────────────────────────
            kv["SumD5"] = "(D5) " + Fmt(tmpD5);
            kv["SumD5Tol"] = "+ " + Fmt(GenTol(tmpD5));
            kv["SumD5TolN"] = "- " + Fmt(GenTol(tmpD5));

            // ── B (bredd) — ± GenTol ──────────────────────────────────────────────
            kv["SumB"] = "(B) " + Fmt(tmpBv);
            kv["SumBTol"] = "± " + Fmt(GenTol(tmpBv));

            // ── B1 — h8 tol ───────────────────────────────────────────────────────
            kv["SumB1"] = "(B1) " + Fmt(tmpB1);
            kv["SumB1Tol"] = "+ 0 [2]";
            kv["SumB1TolN"] = "- " + Fmt(H8Tol(tmpB1)) + " [3]";

            // ── B2 — ± GenTol ─────────────────────────────────────────────────────
            kv["SumB2"] = "(B2) " + Fmt(tmpB2);
            kv["SumB2Tol"] = "± " + Fmt(GenTol(tmpB2));

            // ── B3 — ± GenTol ─────────────────────────────────────────────────────
            kv["SumB3"] = "(B3) " + Fmt(tmpB3);
            kv["SumB3Tol"] = "± " + Fmt(GenTol(tmpB3));

            // ── B4 — ± GenTol ─────────────────────────────────────────────────────
            kv["SumB4"] = "(B4) " + Fmt(tmpB4);
            kv["SumB4Tol"] = "± " + Fmt(GenTol(tmpB4));

            // ── BM (borrdiameter) = 5 fixed, H12 ─────────────────────────────────
            kv["SumBM"] = "(BM) 5";
            kv["SumBMTol"] = "+ " + Fmt(H12Tol(tmpBM));
            kv["SumBMTolN"] = "- 0";

            // ── BD (borrdjup) — ± GenTol ──────────────────────────────────────────
            kv["SumBD"] = "(BD) " + Fmt(tmpBD);
            kv["SumBDTol"] = "± " + Fmt(GenTol(tmpBD));

            // ── A (= BD1 = B1/2) — ± GenTol ──────────────────────────────────────
            double tmpA = tmpB1 / 2.0;
            kv["SumA"] = "(BD1) " + Fmt(tmpA);
            kv["SumATol"] = "± " + Fmt(GenTol(tmpA));

            // ── DOB (diameter oljeborrhål) ────────────────────────────────────────
            kv["SumDOB"] = "(DOB) " + Fmt(tmpDOB);

            // ── R and DR ──────────────────────────────────────────────────────────
            double tmpR = (tmpD5 - tmpDOB) / 2.0;
            double tmpDR = (tmpD / 2.0) - tmpR;
            kv["SumDR"] = "(DR) " + Fmt(tmpDR).Replace(".",",");

            // ── AOB / HM (hjälpmått) ──────────────────────────────────────────────
            // if VOB > 10: TmpAOB = VOB - DOB (direct subtraction, not rounded)
            // else: TmpAOB = Round(Sin(VOB/2 * pi/180)*2 * R - DOB, 0.1)
            string tmpAOBStr;
            if (tmpVOB > 10)
            {
                double aob = tmpVOB - tmpDOB;
                tmpAOBStr = Fmt(aob);
            }
            else
            {
                double kOnst = Math.Round(Math.Sin((tmpVOB / 2.0) * Math.PI / 180.0) * 2.0, 4);
                double aob = Math.Round(kOnst * tmpR - tmpDOB, 1);
                tmpAOBStr = Fmt(aob);
            }
            kv["SumHM"] = "Hjälpmått " + tmpAOBStr + " (" + Fmt(tmpVOB).Replace(".",",") + "\u00ba)";

            // ── M (hjälpmått B-B1-B2) ─────────────────────────────────────────────
            double tmpM = tmpBv - tmpB1 - tmpB2;
            kv["SumM"] = "(M) " + Fmt(tmpM).Replace(".", ",");
            kv["SumMTol"] = "± " + Fmt(GenTol(tmpM));

            // ── Angles & chamfers ─────────────────────────────────────────────────
            kv["SumV45"] = "45\u00ba";
            kv["SumV45_2"] = "45\u00ba";
            kv["SumF45"] = "1x45\u00ba";

            // ── VBM (vinkel borrhål) ──────────────────────────────────────────────
            // Lotus @ATan2(y,x): y = (D/2 - BD), x = (BM/2 + 0.3)
            double tmpVBM;
            if (tmpBMbm == 0)
                tmpVBM = Math.Round(Math.Atan2((tmpD / 2.0) - tmpBD, (tmpBM / 2.0) + 0.3) * 180.0 / Math.PI, 1);
            else
                tmpVBM = tmpBMbm;
            if (subject == "SV-TSD 3040-3 U/VZ2M4")
                tmpVBM = 1.3;
            else if (subject == "SV-TSD 3240 U/VZ2M4")
                tmpVBM = 1.2;
            else if (subject == "SV-TSD 3040-2 U/VZ2M4")
                tmpVBM = 1.3;
            kv["SumV15"] = Fmt1(tmpVBM) + "\u00ba";

            // ── Korda BM ──────────────────────────────────────────────────────────
            double tmpKonstKorda = Math.Round(Math.Sin(((90.0 - tmpVBM) / 2.0) * Math.PI / 180.0) * 2.0, 4);
            double tmpKordaBM = Math.Round((tmpD / 2.0) * tmpKonstKorda, 1);
            if (subject == "SV-TSD 3040-3 U/VZ2M4")
                tmpKordaBM = 185.9;
            else if (subject == "SV-TSD 3240 U/VZ2M4")
                tmpKordaBM = 200.1;
            else if (subject == "SV-TSD 3040-2 U/VZ2M4")
                tmpKordaBM = 185.9;
            kv["SumKordaBM"] = "Korda kant till kant " + Fmt(tmpKordaBM).Replace(".",",");

            // ── Ra & Ritning ──────────────────────────────────────────────────────
            kv["SumRit"] = tmpBet;
            kv["SumRit2"] = tmpBet;

            // ── Machine ───────────────────────────────────────────────────────────
            bool isNak = EqualsI(maskinVal, "Nakamura");
            bool isMM = EqualsI(maskinVal, "MaxMuller/Skepp6");
            bool mAny = isNak || isMM;

            string mvS1 = isNak ? "Nakamura" : isMM ? "MaxMuller" : "";
            string mvS2 = isNak ? "Nakamura" : isMM ? "Skepp 6" : "";
            kv["SumMaskinValS1"] = "Maskin: Svarvning - " + mvS1;
            kv["SumMaskinValS2"] = "Maskin: Borrning - " + mvS2;

            SumFrequencies(kv, mAny, isNak, isMM);
            SumDevices(kv, mAny);
            SumAF(kv, mAny, "6.3");

            // ── Texts ─────────────────────────────────────────────────────────────
            string sumText = "Okulärkontroll Grader, frifläcker, slagmärken, repor, valkar, ytjämnhet och faser, skarpa kanter avgradas.";
            kv["SumTextS1"] = sumText;
            kv["SumTextS2"] = sumText;

            // ── Validation ────────────────────────────────────────────────────────
            kv["VaLVZ2L1"] = tmpVZ2L1
                ? "Det finns en särskild mall för Ring med tillägg VZ2L1" : "";

            return kv;
        }

        // ── Popup ──────────────────────────────────────────────────────────────────
        private static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published)) return "";
            DateTime pubDt;
            if (!DateTime.TryParse(published, out pubDt)) return "";
            DateTime validTill = pubDt.AddDays(14);
            if (DateTime.Today <= validTill.Date)
                return "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)\n\nInformation om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument\n\nPopupruta aktiv till " +
                       validTill.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return "";
        }

        // ── Freq / Devices / AF ───────────────────────────────────────────────────
        private static void SumFrequencies(Dictionary<string, string> kv, bool mAny, bool isNak, bool isMM)
        {
            // Page 1 — same for both machines
            kv["SumF1_1"] = mAny ? "1/1" : "";
            kv["SumF1_2"] = mAny ? "1/1" : "";
            kv["SumF1_3"] = mAny ? "1/3" : "";
            kv["SumF1_4"] = mAny ? "1/3" : "";
            kv["SumF1_5"] = mAny ? "1/5" : "";
            kv["SumF1_6"] = mAny ? "Inst." : "";
            kv["SumF1_7"] = mAny ? "Inst." : "";
            kv["SumF1_8"] = mAny ? "1/3" : "";
            kv["SumF1_9"] = mAny ? "1/3" : "";

            // Page 2 — Nakamura and MaxMuller/Skepp6 differ
            kv["SumF2_1"] = isNak ? "1/5" : isMM ? "1/1" : "";
            kv["SumF2_2"] = isNak ? "1/5" : isMM ? "1/1" : "";
            kv["SumF2_3"] = isNak ? "1/5" : isMM ? "1/1" : "";
            kv["SumF2_4"] = isNak ? "1/5" : isMM ? "1/1" : "";
            kv["SumF2_5"] = isNak ? "Inst." : isMM ? "1/1" : "";
        }

        private static void SumDevices(Dictionary<string, string> kv, bool m)
        {
            // Page 1 — same for both machines
            kv["SumD1_1"] = m ? "Mikrometer" : "";
            kv["SumD1_2"] = m ? "Mikrometer" : "";
            kv["SumD1_3"] = m ? "Digitalt Djup/Hakmått" : "";
            kv["SumD1_4"] = m ? "Digitalt Djup/Hakmått" : "";
            kv["SumD1_5"] = m ? "Skjutmått" : "";
            kv["SumD1_6"] = m ? "Vinkelsystem" : "";
            kv["SumD1_7"] = m ? "Vinkelsystem" : "";
            kv["SumD1_8"] = m ? "Ytjämnhetsmätare" : "";
            kv["SumD1_9"] = m ? "Skjutmått" : "";
            // Page 2 — same for both machines
            kv["SumD2_1"] = m ? "Skjutmått" : "";
            kv["SumD2_2"] = m ? "Skjutmått" : "";
            kv["SumD2_3"] = m ? "Skjutmått" : "";
            kv["SumD2_4"] = m ? "Skjutmått" : "";
            kv["SumD2_5"] = m ? "Skjutmått" : "";
        }

        private static void SumAF(Dictionary<string, string> kv, bool m, string ra)
        {
            // Page 1
            for (int i = 1; i <= 7; i++) kv["SumAF1_" + i] = "";
            kv["SumAF1_8"] = m ? "Bearbetas Ra " + ra + " runt om" : "";
            kv["SumAF1_9"] = "";
            // Page 2
            for (int i = 1; i <= 5; i++) kv["SumAF2_" + i] = "";
        }

        // ── Tolerance helpers ─────────────────────────────────────────────────────

        // General tolerance (7-step) — used for D, D1, D3, D5 (sym), B, B2, B3, B4, BD, A, M
        private static double GenTol(double v)
        {
            if (v < 6) return 0.1;
            if (v < 30) return 0.2;
            if (v < 120) return 0.3;
            if (v < 400) return 0.5;
            if (v < 1000) return 0.8;
            if (v < 2000) return 1.2;
            return 2.0;
        }

        // h8 (14-step, for D2 and B1 negative tolerance)
        private static double H8Tol(double v)
        {
            if (v < 3.1) return 0.014; if (v < 6.1) return 0.018;
            if (v < 10.1) return 0.022; if (v < 18.1) return 0.027;
            if (v < 30.1) return 0.033; if (v < 50.1) return 0.039;
            if (v < 80.1) return 0.046; if (v < 120.1) return 0.054;
            if (v < 180.1) return 0.063; if (v < 250.1) return 0.072;
            if (v < 315.1) return 0.081; if (v < 400.1) return 0.089;
            if (v < 500.1) return 0.097; return 0.110;
        }

        // H12 (14-step, for D4 and BM)
        private static double H12Tol(double v)
        {
            if (v < 3.1) return 0.100; if (v < 6.1) return 0.120;
            if (v < 10.1) return 0.150; if (v < 18.1) return 0.180;
            if (v < 30.1) return 0.210; if (v < 50.1) return 0.250;
            if (v < 80.1) return 0.300; if (v < 120.1) return 0.350;
            if (v < 180.1) return 0.400; if (v < 250.1) return 0.460;
            if (v < 315.1) return 0.520; if (v < 400.1) return 0.570;
            if (v < 500.1) return 0.630; return 0.700;
        }

        // ── Utilities ─────────────────────────────────────────────────────────────
        private static bool EqualsI(string a, string b) =>
            string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);

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