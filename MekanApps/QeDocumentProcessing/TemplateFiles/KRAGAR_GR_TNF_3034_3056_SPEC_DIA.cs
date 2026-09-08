using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class KRAGAR_GR_TNF_3034_3056_SPEC_DIA : ITemplateCalculations
    {
        private static readonly string[] TypList = { "34", "36", "38", "40", "44", "48", "52", "56" };
        private static readonly string[] SerieList = { "30" };

        // D-tables for no-slash case: [D2, D3, D4, D5, D6, D7, D8, D9, D10] — 9 elements
        // Indexed by TmpTypLista (1-based = position in TypList)
        private static readonly Dictionary<string, double[]> NoSlashTab = new Dictionary<string, double[]>
        {
            ["34"] = new[] { 217.2, 212, 152, 172, 176, 186, 210, 218, 218.2 },
            ["36"] = new[] { 227.2, 222, 162, 184, 188, 198, 219, 227, 228.2 },
            ["38"] = new[] { 237.2, 232, 172, 194, 198, 208, 229, 237, 238.2 },
            ["40"] = new[] { 249.2, 242, 183, 204, 208, 218, 239, 247, 250.2 },
            ["44"] = new[] { 269.2, 262, 203, 224, 229, 241, 259, 267, 270.2 },
            ["48"] = new[] { 289.2, 282, 223, 265, 271, 283, 287, 302.5, 290.2 },
            ["52"] = new[] { 309.2, 302, 243, 285, 291, 303, 307, 322.5, 310.2 },
            ["56"] = new[] { 329.2, 322, 263, 305, 311, 323, 327, 342.5, 330.2 },
        };

        // D-tables for slash (special diameter) case
        private static readonly Dictionary<string, double[]> SlashTab = new Dictionary<string, double[]>
        {
            ["34"] = new[] { 217.2, 212, 188, 209, 213, 223, 230, 241.5, 218.2 },
            ["36"] = new[] { 227.2, 222, 198, 219, 223, 235, 242, 253, 228.2 },
            ["38"] = new[] { 237.2, 232, 208, 229, 233, 245, 252, 263, 238.2 },
            ["40"] = new[] { 249.2, 242, 218, 260, 266, 278, 282, 297, 250.2 },
            ["44"] = new[] { 269.2, 262, 238, 280, 286, 298, 302, 316.5, 270.2 },
            ["48"] = new[] { 289.2, 282, 258, 300, 306, 318, 322, 338, 290.2 },
            ["52"] = new[] { 309.2, 302, 278, 320, 326, 338, 342, 357.5, 310.2 },
            ["56"] = new[] { 329.2, 322, 298, 340, 346, 358, 362, 377, 330.2 },
        };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            // CRITICAL: Machine key is [MV] not [MaskinVal]
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");
            bool tmpSlash = tmpBet.IndexOf('/') >= 0;
            bool tmpVZ = tmpBet.IndexOf("VZ", StringComparison.OrdinalIgnoreCase) >= 0;
            bool tmpV21 = tmpBet.IndexOf("V21", StringComparison.OrdinalIgnoreCase) >= 0;

            // Explode " /." — dash is NOT a separator
            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : "";
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : "";

            int cntB2 = tmpBet2.Length;
            // TmpSerie = Left(2), TmpTyp = Right(2)
            string tmpSerie = tmpBet2.Length >= 2 ? tmpBet2.Substring(0, 2) : tmpBet2;
            string tmpTyp = tmpBet2.Length >= 2 ? tmpBet2.Substring(tmpBet2.Length - 2) : tmpBet2;
            int typInt = TryParseInt(tmpTyp);

            int artLista = GetMember(tmpBet1, new[] { "GR-TNF" });
            int serieLista = GetMember(tmpSerie, SerieList);
            int typLista = GetMember(tmpTyp, TypList);

            kv["SumArt"] = tmpBet1;
            kv["SumSerie"] = tmpSerie;
            kv["SumTyp"] = tmpTyp;

            // ── Ritning ───────────────────────────────────────────────────────────
            kv["SumRitS1"] = " 7439889";   // leading space as in Lotus
            kv["SumRitS2"] = " 7439889";

            // ── D-table selection ─────────────────────────────────────────────────
            var tab = tmpSlash
                ? (SlashTab.ContainsKey(tmpTyp) ? SlashTab[tmpTyp] : new double[9])
                : (NoSlashTab.ContainsKey(tmpTyp) ? NoSlashTab[tmpTyp] : new double[9]);
            // Indices: [0]=D2 [1]=D3 [2]=D4 [3]=D5 [4]=D6 [5]=D7 [6]=D8 [7]=D9 [8]=D10

            double tmpD2 = tab.Length > 0 ? tab[0] : 0;
            double tmpD3 = tab.Length > 1 ? tab[1] : 0;
            double tmpD4 = tab.Length > 2 ? tab[2] : 0;
            double tmpD5 = tab.Length > 3 ? tab[3] : 0;
            double tmpD6 = tab.Length > 4 ? tab[4] : 0;
            double tmpD7 = tab.Length > 5 ? tab[5] : 0;
            double tmpD8 = tab.Length > 6 ? tab[6] : 0;
            double tmpD9 = tab.Length > 7 ? tab[7] : 0;
            double tmpD10 = tab.Length > 8 ? tab[8] : 0;

            // ── D10 ───────────────────────────────────────────────────────────────
            kv["SumD10"] = "(D10) " + FmtG(tmpD10);
            kv["SumD10Tol"] = "+ 0";
            kv["SumD10TolN"] = " - 0.3";

            // ── D9 ───────────────────────────────────────────────────────────────
            kv["SumD9"] = "(D9) " + FmtG(tmpD9);
            kv["SumD9Tol"] = tmpD9 < 401 ? "± 0.5" : "± 0.8";

            // ── D8 ───────────────────────────────────────────────────────────────
            kv["SumD8"] = "(D8) " + FmtG(tmpD8);
            kv["SumD8Tol"] = tmpD8 < 401 ? "± 0.5" : "± 0.8";

            // ── D7 (JS11-like positive tolerance) ─────────────────────────────────
            kv["SumD7"] = "(D7) " + FmtG(tmpD7);
            kv["SumD7Tol"] = D7TolPos(tmpD7);
            kv["SumD7TolN"] = " - 0";

            // ── D6 (p6-like negative tolerance) ───────────────────────────────────
            kv["SumD6"] = "(D6) " + FmtG(tmpD6);
            kv["SumD6Tol"] = "+ 0";
            kv["SumD6TolN"] = D6TolNeg(tmpD6);

            // ── D5 (positive tolerance same scale as D7) ───────────────────────────
            kv["SumD5"] = "(D5) " + FmtG(tmpD5);
            kv["SumD5Tol"] = D5TolPos(tmpD5);
            kv["SumD5TolN"] = " - 0";

            // ── D4 — fixed +0.3/-0 ────────────────────────────────────────────────
            kv["SumD4"] = "(D4) " + FmtG(tmpD4);
            kv["SumD4Tol"] = "+ 0.3";
            kv["SumD4TolN"] = " - 0";

            // ── D3 — +0/-0.4 ──────────────────────────────────────────────────────
            kv["SumD3"] = "(D3) " + FmtG(tmpD3);
            kv["SumD3Tol"] = "+  0";
            kv["SumD3TolN"] = "- 0.400";

            // ── D2 — +0/-0.3 ──────────────────────────────────────────────────────
            kv["SumD2"] = "(D2) " + FmtG(tmpD2);
            kv["SumD2Tol"] = "+  0";
            kv["SumD2TolN"] = " - 0.3";

            // ── A (bredd) ─────────────────────────────────────────────────────────
            double tmpA = typInt < 40 ? 19 : typInt < 48 ? 24 : 29;
            kv["SumA"] = "(A) " + FmtG(tmpA);
            kv["SumATol"] = "± 0.3";

            // ── B (bredd) — depends on slash ─────────────────────────────────────
            double tmpB;
            if (!tmpSlash) tmpB = typInt < 40 ? 35 : typInt < 48 ? 40 : 60;
            else tmpB = typInt < 40 ? 41 : typInt < 48 ? 53 : 60;
            kv["SumB"] = "(B) " + FmtG(tmpB);
            kv["SumBTol"] = "± 0.3";

            // ── C (bredd) — depends on slash ──────────────────────────────────────
            double tmpC;
            if (!tmpSlash)
            {
                if (typInt < 40) tmpC = 12;
                else if (typInt < 48) tmpC = 10.5;
                else if (typInt < 56) tmpC = 14;
                else tmpC = 12.5;
            }
            else
            {
                if (typInt < 40) tmpC = 10.5;
                else if (typInt < 48) tmpC = 14;
                else tmpC = 12.5;
            }
            kv["SumC"] = "(C) " + FmtG(tmpC);
            kv["SumCTol"] = "+ 0.2";
            kv["SumCTolN"] = " - 0";

            // ── E (bredd) — depends on slash ──────────────────────────────────────
            double tmpE;
            if (!tmpSlash)
            {
                if (typInt == 34) tmpE = 9;
                else if (typInt == 36 || typInt == 40 || typInt == 44) tmpE = 10.5;
                else if (typInt == 38) tmpE = 10;
                else tmpE = 16;
            }
            else
            {
                tmpE = typInt < 40 ? 10.5 : 16;
            }
            kv["SumE"] = "(E) " + FmtG(tmpE);
            kv["SumETol"] = "+ 0.2";
            kv["SumETolN"] = " - 0";

            // ── F (borrhål) — depends on slash ────────────────────────────────────
            double tmpF;
            if (!tmpSlash)
            {
                if (typInt == 36) tmpF = 10;
                else if (typInt == 34 || typInt == 38) tmpF = 10.5;
                else if (typInt < 48) tmpF = 9;
                else if (typInt < 56) tmpF = 19.5;
                else tmpF = 18;
            }
            else
            {
                if (typInt < 38) tmpF = 11.5;
                else if (typInt == 38) tmpF = 13;
                else if (typInt < 48) tmpF = 17;
                else if (typInt == 48) tmpF = 15.5;
                else tmpF = 16;
            }
            kv["SumF"] = "(F) " + FmtG(tmpF);
            kv["SumFTol"] = "± 0.5";

            // ── G (fas) ───────────────────────────────────────────────────────────
            double tmpG = typInt < 48 ? 2 : 3;
            kv["SumG"] = FmtG(tmpG) + "x45\u00ba";
            kv["SumGTol"] = "± 0.1";

            // ── K (bredd) — depends on slash ──────────────────────────────────────
            double tmpK;
            if (!tmpSlash)
            {
                if (typInt == 34) tmpK = 4.5;
                else if (typInt == 36) tmpK = 4.8;
                else if (typInt == 38) tmpK = 5;
                else if (typInt < 48) tmpK = 4.3;
                else tmpK = 7.5;
            }
            else
            {
                tmpK = typInt < 40 ? 4.3 : 7.5;
            }
            kv["SumK"] = "(K) " + FmtG(tmpK);
            kv["SumKTol"] = "+ 0.2";
            kv["SumKTolN"] = " - 0";

            // ── L (borrdjup) — depends on slash ───────────────────────────────────
            double tmpL;
            if (!tmpSlash)
            {
                if (typInt == 34) tmpL = 19;
                else if (typInt == 36) tmpL = 17.5;
                else if (typInt == 38) tmpL = 17;
                else if (typInt < 48) tmpL = 15.5;
                else tmpL = 12;
            }
            else
            {
                tmpL = (typInt == 36 || typInt == 38) ? 12 : 11;
            }
            kv["SumL"] = "(L) " + FmtG(tmpL);
            kv["SumLTol"] = "± 0.2";

            // ── M (gängdjup) — fixed ──────────────────────────────────────────────
            kv["SumM"] = "Min:6";

            // ── O (bredd) ─────────────────────────────────────────────────────────
            double tmpO = typInt < 40 ? 5.8 : 7.8;
            kv["SumO"] = "(O) " + FmtG(tmpO);
            kv["SumOTol"] = "+ 0";
            kv["SumOTolN"] = " - 0.2";

            // ── P (bredd) ─────────────────────────────────────────────────────────
            double tmpP = typInt < 40 ? 9 : typInt < 48 ? 10 : 12.5;
            kv["SumP"] = "(P) " + FmtG(tmpP);
            kv["SumPTol"] = "+ 0.5";
            kv["SumPTolN"] = " - 0";

            // ── N (gänga) ─────────────────────────────────────────────────────────
            kv["SumN"] = "1/4-28 UNF";
            kv["SumN1"] = "1/4-28 UNF";

            // ── HM (hjälpmått = K + C + 0.2) ─────────────────────────────────────
            double tmpHM = tmpK + tmpC + 0.2;
            kv["SumHM"] = "(HM) " + FmtG(tmpHM) + " *";
            kv["SumHMText"] = "* Hjälpmått (HM) beräknad mitt i tolerans på måtten (K) och (C)";

            // ── Surface finish ────────────────────────────────────────────────────
            kv["SumRa"] = "Ra=12.5";
            kv["SumRa32"] = "3.2";
            kv["SumRd15a"] = "0.15";
            kv["SumRd2"] = "0.2";

            // ── Radier ────────────────────────────────────────────────────────────
            kv["SumR08x2"] = "max:R0.8 (2x)";
            kv["SumR08"] = "max:R0.8";
            kv["SumR12"] = "max:R1.2";
            kv["SumR2"] = "R 2";

            // ── Vinklar ───────────────────────────────────────────────────────────
            kv["SumV15"] = "15\u00ba";

            // ── Machine — NOTE: key is [MV] not [MaskinVal] ───────────────────────
            bool isNak = EqualsI(maskinVal, "Nakamura");
            bool isMM = EqualsI(maskinVal, "MaxMuller");
            bool mAny = isNak || isMM;
            string mvS = isNak ? "Nakamura" : isMM ? "MaxMuller" : "";
            kv["SumMaskinValS1"] = "Maskin: " + mvS;
            kv["SumMaskinValS2"] = "Maskin: " + mvS;

            SumFrequencies(kv, mAny, isNak, isMM);
            SumDevices(kv, mAny);
            SumAF(kv, mAny);

            // ── Texts ─────────────────────────────────────────────────────────────
            kv["SumTextS1"] = "Skarpa kanter avgradas.";
            kv["SumTextS2"] = "Skarpa kanter avgradas.";

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
                return "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)\n\nPopupruta aktiv till " +
                       validTill.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return "";
        }

        // ── Freq / Devices / AF ───────────────────────────────────────────────────
        private static void SumFrequencies(Dictionary<string, string> kv, bool mAny, bool isNak, bool isMM)
        {
            // F1_1-4: both machines = "1/2"
            kv["SumF1_1"] = mAny ? "1/2" : "";
            kv["SumF1_2"] = mAny ? "1/2" : "";
            kv["SumF1_3"] = mAny ? "1/2" : "";
            kv["SumF1_4"] = mAny ? "1/2" : "";
            // F1_5: Nakamura="1/5", MaxMuller="1/2"
            kv["SumF1_5"] = isNak ? "1/5" : isMM ? "1/2" : "";
            // F1_6: both empty
            kv["SumF1_6"] = "";
        }

        private static void SumDevices(Dictionary<string, string> kv, bool m)
        {
            kv["SumD1_1"] = m ? "Skjutmått" : "";
            kv["SumD1_2"] = m ? "min/max Gängtolk" : "";
            kv["SumD1_3"] = m ? "Skjutmått/Djupmått" : "";
            kv["SumD1_4"] = m ? "Ra-mätare" : "";
            kv["SumD1_5"] = m ? "Mätmaskin" : "";
            kv["SumD1_6"] = "";
        }

        private static void SumAF(Dictionary<string, string> kv, bool m)
        {
            kv["SumAF1_1"] = m ? "Alt. UD-Apparat inst. med Ring/Klove" : "";
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = m ? "Vid misstänkt formfel lämnas kragen till mätrummet" : "";
            kv["SumAF1_6"] = "";
        }

        // ── Tolerance helpers ─────────────────────────────────────────────────────

        // D7 positive tolerance (7-step, same thresholds as D5)
        private static string D7TolPos(double v)
        {
            if (v < 170) return "+ 0.25";
            if (v < 250) return "+ 0.29";
            if (v < 315) return "+ 0.32";
            if (v < 390) return "+ 0.36";
            if (v < 500) return "+ 0.40";
            if (v < 600) return "+ 0.44";
            return "+ 0.50";
        }

        // D6 negative tolerance (5-step)
        private static string D6TolNeg(double v)
        {
            if (v < 180) return " - 0.25";
            if (v < 255) return " - 0.29";
            if (v < 310) return " - 0.32";
            if (v < 380) return " - 0.36";
            if (v < 500) return " - 0.40";
            return " - 0.44";
        }

        // D5 positive tolerance (6-step)
        private static string D5TolPos(double v)
        {
            if (v < 180) return "+ 0.25";
            if (v < 250) return "+ 0.29";
            if (v < 310) return "+ 0.32";
            if (v < 390) return "+ 0.36";
            if (v < 490) return "+ 0.40";
            return "+ 0.44";
        }

        // ── Utilities ─────────────────────────────────────────────────────────────
        private static int GetMember(string val, string[] list)
        {
            if (list == null) return 0;
            for (int i = 0; i < list.Length; i++)
                if (string.Equals(list[i], val, StringComparison.OrdinalIgnoreCase)) return i + 1;
            return 0;
        }

        private static bool EqualsI(string a, string b) =>
            string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);

        private static int TryParseInt(string s)
        {
            int v;
            return int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static string FmtG(double v) => v.ToString(CommonFunctions.Culture).Replace(".", ",");
        private static string Fmt3(double v) => v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
    }
}