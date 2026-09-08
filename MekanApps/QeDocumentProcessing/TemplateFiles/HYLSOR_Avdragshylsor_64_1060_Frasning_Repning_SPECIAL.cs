using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Avdragshylsor_64_1060_Frasning_Repning_SPECIAL : ITemplateCalculations
    {
        private static readonly string[] ArtList = { "AOH", "AOHX", "LW", "MS" };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;
            List<Bookmark> bm = req != null ? req.Bookmarks : null;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");
            bool tmpLW = tmpBet.IndexOf("LW", StringComparison.OrdinalIgnoreCase) >= 0;
            bool tmpMS = tmpBet.IndexOf("MS", StringComparison.OrdinalIgnoreCase) >= 0;
            bool tmpSlash = tmpBet.IndexOf('/') >= 0;

            // Explode " /.-"
            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : "";
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : "";
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : "";

            int cntB2 = tmpBet2.Length;

            // ArtLista validation (not directly used in any output, but preserved)
            int artLista = GetMember(tmpBet1, ArtList);

            // TmpTyp — used only for LW/MS classification (not for table lookup; no tables exist)
            string tmpTyp;
            if (tmpLW || tmpMS) tmpTyp = "0";
            else if (cntB2 == 3 || cntB2 == 2) tmpTyp = tmpBet2;
            else if (cntB2 > 4) tmpTyp = tmpBet2.Substring(0, 3);
            else tmpTyp = tmpBet2.Length >= 2 ? tmpBet2.Substring(0, 2) : tmpBet2;

            // ── Bookmarks ─────────────────────────────────────────────────────────
            double tmpKona = GetDouble(bm, "Kona");
            double tmpIDia = GetDouble(bm, "Innerdiameter (d1)");
            double tmpYDia = GetDouble(bm, "Ytterdiameter (d2)");
            double tmpKonring = GetDouble(bm, "Konringsdiameter");
            double tmpC = GetDouble(bm, "Slits (c)");
            double tmpB = GetDouble(bm, "Längd till Oljespår (B)");
            double tmpAntRep = GetDouble(bm, "Antal repor (n)");
            double tmpLängdL = GetDouble(bm, "Längd (L)");
            double tmpAmatt = GetDouble(bm, "a-mått (a)");

            // TmpTyp2 — key numeric used for d1 tolerance thresholds
            double tmpTyp2;
            if (tmpLW || tmpMS)
                tmpTyp2 = tmpKonring > 490 ? tmpKonring : (tmpKonring * 2.0 / 10.0);
            else
                tmpTyp2 = cntB2 > 3
                    ? TryParseDouble(tmpBet2.Length >= 2 ? tmpBet2.Substring(tmpBet2.Length - 2) : tmpBet2)
                    : TryParseDouble(tmpBet3);

            // ── d1 (innerdiameter) & tol (Kläm & Avdragshylsor 7437495:4) ─────────
            kv["SumIDia"] = "(d1) " + Fmt(tmpIDia);
            kv["SumIDiaTol"] = D1TolPos(tmpTyp2) + " [3F]";
            kv["SumIDiaTolN"] = D1TolNeg(tmpTyp2) + " [3F]";

            // ── c (slits) ─────────────────────────────────────────────────────────
            kv["SumC"] = "(c) " + Fmt(tmpC);
            kv["SumCTol"] = "± 0.2";

            // ── B (längd till oljespår) ──────────────────────────────────────────
            kv["SumB"] = "(B) " + Fmt(tmpB);
            kv["SumBTol"] = GenTolStr(tmpB);

            // ── Repor (n) ─────────────────────────────────────────────────────────
            kv["SumAntRep"] = "(n) " + Fmt(tmpAntRep) + " st.";

            // ── Delningsvinkel repor ──────────────────────────────────────────────
            string delVinkelBm = GetString(bm, "Delningsvinkel Repor");
            bool delVinkelZero = string.IsNullOrEmpty(delVinkelBm) || EqualsI(delVinkelBm, "0");
            double tmpDelVinkelRepor = delVinkelZero
                ? (tmpAntRep - 1 != 0 ? (360.0 - (12.0 * 2.0)) / (tmpAntRep - 1.0) : 0)
                : TryParseDouble(delVinkelBm);
            kv["SumVDRep"] = FmtComma(Math.Round(tmpDelVinkelRepor, 2));
            kv["SumVDRep2"] = kv["SumVDRep"];

            // ── Kvarvarande vinkel för 1:a repa ───────────────────────────────────
            string startVinkelBm = GetString(bm, "Startvinkel Repor");
            double tmp1;
            if (delVinkelZero) tmp1 = 12.0;
            else
            {
                bool svZero = string.IsNullOrEmpty(startVinkelBm) || EqualsI(startVinkelBm, "0");
                tmp1 = svZero
                    ? (360.0 - (tmpDelVinkelRepor * (tmpAntRep - 1.0))) / 2.0
                    : TryParseDouble(startVinkelBm);
            }
            kv["SumV1Rep"] = FmtComma(Math.Round(tmp1, 2));
            kv["SumV1Rep2"] = kv["SumV1Rep"];

            // ── Konstanter 1:a & delningsrepor ────────────────────────────────────
            double tmpKonstant1Rep = Math.Round(Math.Sin((tmp1 / 2.0) * Math.PI / 180.0) * 2.0, 4);
            double tmpKonstantDelRep = Math.Round(Math.Sin((tmpDelVinkelRepor / 2.0) * Math.PI / 180.0) * 2.0, 4);

            // ── Korda första repa inv & utv ───────────────────────────────────────
            double tmpKorda1RepInv = Math.Round((tmpIDia / 2.0) * tmpKonstant1Rep, 1);
            double sumKR1inv = tmpKorda1RepInv - (tmpC / 2.0);
            kv["SumKR1inv"] = Fmt(sumKR1inv).Replace(".", ",");

            double tmpKonUträkning = (((tmpLängdL + tmpAmatt) - tmpB) / tmpKona) + tmpKonring;
            double tmpKorda1RepUtv = Math.Round((tmpKonUträkning / 2.0) * tmpKonstant1Rep, 1);
            double sumKR1utv = tmpKorda1RepUtv - (tmpC / 2.0);
            kv["SumKR1utv"] = Fmt(sumKR1utv).Replace(".", ",");

            // ── Korda delning repor inv & utv ─────────────────────────────────────
            double tmpKordaDelRepInv = Math.Round((tmpIDia / 2.0) * tmpKonstantDelRep, 1);
            kv["SumKRDinv"] = Fmt(tmpKordaDelRepInv).Replace(".", ",");
            double tmpKordaDelRepUtv = Math.Round((tmpKonUträkning / 2.0) * tmpKonstantDelRep, 1);
            kv["SumKRDutv"] = Fmt(tmpKordaDelRepUtv).Replace(".", ",");

            // ── Startvinkel oljespår & korda ──────────────────────────────────────
            double tmpVOS = GetDouble(bm, "Startvinkel Oljespår");
            kv["SumVOS"] = FmtComma(tmpVOS);
            kv["SumVOS1"] = FmtComma(tmpVOS);
            double tmpKonstantOS = Math.Round(Math.Sin((tmpVOS / 2.0) * Math.PI / 180.0) * 2.0, 4);
            double tmpKordaOSinv = Math.Round((tmpIDia / 2.0) * tmpKonstantOS, 1);
            kv["SumKOSinv"] = tmp1 < tmpVOS ? "OjSpår utaför Repa" : Fmt(tmpKordaOSinv).Replace(".",",");
            double tmpKordaOSutv = Math.Round((tmpKonUträkning / 2.0) * tmpKonstantOS, 1);
            kv["SumKOSutv"] = tmp1 < tmpVOS ? "OjSpår utaför Repa" : Fmt(tmpKordaOSutv).Replace(".",",");

            // ── E (bredd & radie oljespår) ────────────────────────────────────────
            double tmpE = GetDouble(bm, "Bredd Oljespår (E)");
            kv["SumE"] = "(E) " + Fmt(tmpE);
            kv["SumETol"] = tmpE < 6 ? "± 0.1" : "± 0.2";

            string tmpr1 = Math.Abs(tmpE - 5) < 0.001 ? "R4"
                         : Math.Abs(tmpE - 6) < 0.001 ? "R4,5"
                         : Math.Abs(tmpE - 7) < 0.001 ? "R5"
                         : Math.Abs(tmpE - 8) < 0.001 ? "R6"
                         : Math.Abs(tmpE - 10) < 0.001 ? "R7" : "R8";
            double r1Bm = GetDouble(bm, "Radie Oljespår (r1)");
            kv["Sumr1"] = r1Bm == 0 ? tmpr1.Replace(",", ".") : "R" + Fmt(r1Bm);

            // ── F (djup oljespår) ─────────────────────────────────────────────────
            string tmpFStr;
            if (Math.Abs(tmpE - 5) < 0.001) tmpFStr = "1";
            else if (Math.Abs(tmpE - 6) < 0.001) tmpFStr = "1.2";
            else if (Math.Abs(tmpE - 7) < 0.001) tmpFStr = "1.5";
            else if (tmpE < 9) tmpFStr = "1.5";
            else if (Math.Abs(tmpE - 10) < 0.001) tmpFStr = "2";
            else tmpFStr = "2.7";
            string fBm = GetString(bm, "Oljespårsdjup (F)");
            kv["SumF"] = (string.IsNullOrEmpty(fBm) || EqualsI(fBm, "0")) ? "(F) " + tmpFStr : fBm;
            kv["SumFTol"] = "  0";
            kv["SumFTolN"] = "- 0.2";

            // ── J (längd till repor) ──────────────────────────────────────────────
            double tmpJ = GetDouble(bm, "Längd till Repor (J)");
            kv["SumJ"] = "(J) " + Fmt(tmpJ);
            kv["SumJTol"] = GenTolStr(tmpJ);

            // ── K (längd på repor) — sub-fields separated by '§' ─────────────────
            string kRaw = GetString(bm, "Längd Repor (K)");
            string[] kTokens = string.IsNullOrEmpty(kRaw) ? new string[0] : kRaw.Split('§');
            string kStr = kTokens.Length > 0 ? kTokens[0].Trim() : "";
            double tmpK = TryParseDouble(kStr);
            kv["SumK"] = "(K) " + (string.IsNullOrEmpty(kStr) ? "" : kStr.Replace(",", "."));
            kv["SumKTol"] = GenTolStr(tmpK);

            // ── M (bredd repspår) ─────────────────────────────────────────────────
            string mListaStr = kTokens.Length > 1 ? kTokens[1].Trim() : "";
            string tmpM = string.IsNullOrEmpty(mListaStr) ? "1.5" : mListaStr;
            kv["SumM"] = "(M) " + tmpM.Replace(",", ".");
            kv["SumMTol"] = "±0.1";

            // ── N (djup repspår) ──────────────────────────────────────────────────
            string nListaStr = kTokens.Length > 2 ? kTokens[2].Trim() : "";
            string nBm = GetString(bm, "Repspårsdjup (N)");
            string tmpN;
            if (string.IsNullOrEmpty(nBm) || EqualsI(nBm, "0"))
                tmpN = string.IsNullOrEmpty(nListaStr) ? "0.5" : nListaStr;
            else
                tmpN = nBm;
            kv["SumN"] = "(N) " + tmpN.Replace(",", ".");
            kv["SumNTol"] = "±0.1";

            // ── M2 (alternativ metod repning) — fixed values ─────────────────────
            kv["SumM2"] = "(M) 5";
            kv["SumM2Tol"] = "+0.5";
            kv["SumM2TolN"] = "0.0";

            // ── Ritning ───────────────────────────────────────────────────────────
            kv["SumRitning"] = tmpBet + " - Toleranser efter slits: 7437495:4";
            kv["SumSTRit"] = "Styckritning: ";

            // ── Machine (simple, single-row F/D/AF — no numbered series) ─────────
            bool isVTR = EqualsI(maskinVal, "VTR");
            bool isMT = EqualsI(maskinVal, "MacTurn");
            string mvSuffix = (isVTR || isMT) ? "" : "INGEN MASKINVAL GJORD";
            kv["SumMaskinVal"] = "Maskin: " + maskinVal + mvSuffix;

            kv["SumF_ID"] = isVTR ? "1/1" : isMT ? "1/5" : "";
            kv["SumF_Ö"] = isVTR ? "1/1" : isMT ? "1/5" : "";
            kv["SumD_ID"] = (isVTR || isMT) ? "Skjutmått" : "";
            kv["SumD_Ö"] = (isVTR || isMT) ? "Skjutmått" : "";
            kv["SumAF_ID"] = "";
            kv["SumAF_Ö"] = "";

            // ── Footer text ───────────────────────────────────────────────────────
            kv["SumTextMät"] = "Alla mått kontrolleras vid inställning ";

            return kv;
        }

        // ── Popup ──────────────────────────────────────────────────────────────────
        private static string ComputePopup(string pub)
        {
            if (string.IsNullOrWhiteSpace(pub)) return "";
            DateTime dt; if (!DateTime.TryParse(pub, out dt)) return "";
            DateTime till = dt.AddDays(14);
            return DateTime.Today <= till.Date
                ? "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)<<LineBreak>>Popupruta aktiv till " + till.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                : "";
        }

        // ── Tolerance helpers ─────────────────────────────────────────────────────

        // d1 positive tolerance, 11-step (SS-ISO/Kläm & Avdragshylsor 7437495:4)
        private static string D1TolPos(double typ2)
        {
            if (typ2 > 851) return "+ 0.560";
            if (typ2 > 671) return "+ 0.500";
            if (typ2 > 531) return "+ 0.440";
            if (typ2 > 85) return "+ 0.400";
            if (typ2 > 65) return "+ 0.360";
            if (typ2 > 53) return "+ 0.210";
            if (typ2 > 39) return "+ 0.185";
            if (typ2 > 25) return "+ 0.160";
            if (typ2 > 17) return "+ 0.140";
            if (typ2 > 11) return "+ 0.074";
            return "+ 0.062";
        }

        // d1 negative tolerance, 11-step
        private static string D1TolNeg(double typ2)
        {
            if (typ2 > 851) return "- 0.900";
            if (typ2 > 671) return "- 0.800";
            if (typ2 > 531) return "- 0.700";
            if (typ2 > 85) return "- 0.630";
            if (typ2 > 65) return "- 0.570";
            if (typ2 > 53) return "- 0.320";
            if (typ2 > 39) return "- 0.290";
            if (typ2 > 25) return "- 0.250";
            if (typ2 > 17) return "- 0.220";
            if (typ2 > 11) return "- 0.120";
            return "- 0.100";
        }

        // SS-ISO 2768-m general tolerance (5-step)
        private static string GenTolStr(double v)
        {
            if (v < 6) return "± 0.1";
            if (v < 30) return "± 0.2";
            if (v < 120) return "± 0.3";
            if (v < 400) return "± 0.5";
            return "± 0.8";
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
        private static string Fmt2(double v) => v.ToString("F2", CommonFunctions.Culture).Replace(",", ".");

        private static string FmtComma(double v) =>
    v.ToString("0.################", CommonFunctions.Culture)
     .Replace(".", ",");
    }
}