using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HUS_SNLD_3134_3164_VZ2N7 : ITemplateCalculations
    {
        private static readonly string[] Machines = { "OKUMA MA600/Trevisan DS 450", "Trevisan DS 900" };

        // Serie 31 types [34..64] — 10 elements
        private static readonly string[] Typ31Lista = { "34", "36", "38", "40", "44", "48", "52", "56", "60", "64" };

        // 15-element lookup tables indexed by TmpTypLista (1-based), matching Typ31Lista positions 1-10
        // (Lotus @Word uses ":" separator, 1-based index)
        private static readonly double[] LmLista = { 502, 522, 552, 602, 632, 692, 762, 782, 820, 870 };
        private static readonly double[] A2Lista = { 214, 224, 242, 262, 270, 286, 304, 304, 334, 354 };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpTrim = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpTrim.Replace(".", ",");
            bool tmpSlash = tmpBet.IndexOf('/') >= 0;

            // CRITICAL: Explode " /." — space, slash, dot — DASH is NOT a separator
            // "GF-SNLD 3136/VZ2N7" -> ["GF-SNLD","3136","VZ2N7"]
            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : "";
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : "";
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : "";
            string tmpBet4 = tokens.Length > 3 ? tokens[3] : "";

            // Tmp_VZ2N7 = @Contains(Bet3|Bet4; "VZ2N7")
            bool tmpVZ2N7 = tmpBet3.IndexOf("VZ2N7", StringComparison.OrdinalIgnoreCase) >= 0
                         || tmpBet4.IndexOf("VZ2N7", StringComparison.OrdinalIgnoreCase) >= 0;

            int cntB2 = tmpBet2.Length;

            // TmpSerie = Left(Bet2, 2)
            string tmpSerie = tmpBet2.Length >= 2 ? tmpBet2.Substring(0, 2) : tmpBet2;
            int serieInt = TryParseInt(tmpSerie);

            // TmpTyp = Right(Bet2, 2)
            string tmpTyp = tmpBet2.Length >= 2 ? tmpBet2.Substring(tmpBet2.Length - 2) : tmpBet2;
            int typInt = TryParseInt(tmpTyp);

            // TmpTypLista:
            //   Serie 30 and 32 -> @Member(Typ; "0") -> always 0 (type "0" never matches)
            //   Serie 31        -> 1-based member in Typ31Lista
            int typLista;
            if (serieInt == 31)
            {
                typLista = GetMember(tmpTyp, Typ31Lista);
            }
            else
            {
                // Serie 30 and 32 have only "0" as placeholder -> no match -> 0
                typLista = 0;
            }

            // VaLTypLista — validation message when product not in template
            kv["VaLTypLista"] = typLista == 0
                ? "Produktbeteckningen ingår ej i mallen, kontrollera stavning och/eller kontakta Qe-Admin för att lägga till produkten"
                : "";

            // ── Fixed dimensions ──────────────────────────────────────────────────

            // Ra
            kv["TmpRa32"] = "3.2";
            kv["TmpRa63"] = "6.3";

            // G1, C1
            kv["SumG1"] = "M8";
            kv["SumC1"] = "min 18";

            // G2-G4
            kv["SumG2"] = "G1/4";
            kv["SumG3"] = "G1/4";
            kv["SumG4"] = "G1/4";

            // B1-B4
            kv["SumB1"] = "(B1) max 23";
            kv["SumB2"] = "(B2) min 12";
            kv["SumB3"] = "(B3) min 12";
            kv["SumB4"] = "(B4) min 12";

            // ── Table-driven dimensions ───────────────────────────────────────────

            // Lm (fotlängd) — JS10 tolerance
            double tmpLm = GetTabVal(LmLista, typLista);
            kv["SumLm"] = "(Lm) " + Fmt(tmpLm);
            kv["SumLmTol"] = "± " + Fmt3(JS10Tol(tmpLm));

            // A2 (sidoplan)
            double tmpA2 = GetTabVal(A2Lista, typLista);
            kv["SumA2"] = "(A2) " + Fmt(tmpA2);
            kv["SumA2Tol"] = "± " + Fmt3(GenTolVal(tmpA2));

            // ── Fixed d1 and d2 ───────────────────────────────────────────────────

            // d1 = 32 (fixed), tol +1.0 / -0
            kv["Sumd1"] = "(d1) 32";
            kv["Sumd1Tol"] = "+ " + Fmt1(1.0);
            kv["Sumd1TolN"] = "- " + Fmt0(0);

            // d2 = 30 (fixed), tol ±1.0
            kv["Sumd2"] = "(d2) 30";
            kv["Sumd2Tol"] = "± " + Fmt1(1.0);

            // ── Machine ───────────────────────────────────────────────────────────
            string mv = "";
            if (EqualsI(maskinVal, "OKUMA MA600/Trevisan DS 450"))
                mv = "OKUMA MA600/Trevisan DS 450";
            else if (EqualsI(maskinVal, "Trevisan DS 900"))
                mv = "Trevisan DS 900";
            kv["SumMaskinValS1"] = mv; // no OP prefix, no trailing text

            bool mOk = IsMachine(maskinVal);
            SumFrequencies(kv, mOk);
            SumDevices(kv, mOk);
            for (int i = 1; i <= 8; i++) kv["SumAF1_" + i] = "";

            // ── Texts ─────────────────────────────────────────────────────────────
            string text1 = "Kontrolleras enl. styrplan";
            kv["SumTextS1"] = text1;
            kv["SumTextS2"] = text1 + " Alla materialdefekter utsorteras.";

            // ── Ritning ───────────────────────────────────────────────────────────
            kv["SumPrdritS1"] = "7433267:senaste utg.";

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
                return "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)<LineBreak>>Popupruta aktiv till " +
                       validTill.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return "";
        }

        // ── Freq / Devices ────────────────────────────────────────────────────────
        private static void SumFrequencies(Dictionary<string, string> kv, bool m)
        {
            kv["SumF1_1"] = m ? "1/tim" : "";
            kv["SumF1_2"] = m ? "Inst." : "";
            kv["SumF1_3"] = m ? "Inst." : "";
            kv["SumF1_4"] = m ? "Inst." : "";
            kv["SumF1_5"] = m ? "Inst." : "";
            kv["SumF1_6"] = "";
            kv["SumF1_7"] = "";
            kv["SumF1_8"] = "";
        }

        private static void SumDevices(Dictionary<string, string> kv, bool m)
        {
            kv["SumD1_1"] = m ? "Skjutmått" : "";
            kv["SumD1_2"] = m ? "Gängtolk min/max" : "";
            kv["SumD1_3"] = m ? "Skjutmått" : "";
            kv["SumD1_4"] = m ? "Skjutmått" : "";
            kv["SumD1_5"] = m ? "Skjutmått" : "";
            kv["SumD1_6"] = "";
            kv["SumD1_7"] = "";
            kv["SumD1_8"] = "";
        }

        // ── Tolerance helpers ─────────────────────────────────────────────────────

        // JS10 (21-step)
        private static double JS10Tol(double v)
        {
            if (v < 3.01) return 0.020; if (v < 6.01) return 0.024;
            if (v < 10.01) return 0.029; if (v < 18.01) return 0.035;
            if (v < 30.01) return 0.042; if (v < 50.01) return 0.050;
            if (v < 80.01) return 0.060; if (v < 120.01) return 0.070;
            if (v < 180.01) return 0.080; if (v < 250.01) return 0.092;
            if (v < 315.01) return 0.105; if (v < 400.01) return 0.115;
            if (v < 500.01) return 0.125; if (v < 630.01) return 0.140;
            if (v < 800.01) return 0.160; if (v < 1000.01) return 0.180;
            if (v < 1250.01) return 0.210; if (v < 1600.01) return 0.250;
            if (v < 2000.01) return 0.300; if (v < 2500.01) return 0.350;
            return 0.430;
        }

        // General tolerance (for A2: 7-step ±)
        private static double GenTolVal(double v)
        {
            if (v < 6.01) return 0.1;
            if (v < 30.01) return 0.2;
            if (v < 120.01) return 0.3;
            if (v < 315.01) return 0.5;
            if (v < 1000.01) return 0.8;
            if (v < 2000.01) return 1.2;
            return 2.0;
        }

        // ── Utilities ─────────────────────────────────────────────────────────────
        private static double GetTabVal(double[] tab, int oneBasedIdx)
        {
            if (tab == null || oneBasedIdx < 1 || oneBasedIdx > tab.Length) return 0;
            return tab[oneBasedIdx - 1];
        }

        private static int GetMember(string val, string[] list)
        {
            if (list == null) return 0;
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

        private static bool EqualsI(string a, string b) =>
            string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);

        private static int TryParseInt(string s)
        {
            int v;
            return int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static string Fmt(double v) => v.ToString(CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt0(double v) => ((int)v).ToString(CultureInfo.InvariantCulture);
        private static string Fmt1(double v) => v.ToString("F1", CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt3(double v) => v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
    }
}