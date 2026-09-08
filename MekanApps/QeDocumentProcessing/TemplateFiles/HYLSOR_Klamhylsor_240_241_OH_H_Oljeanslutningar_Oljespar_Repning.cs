using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Klamhylsor_240_241_OH_H_Oljeanslutningar_Oljespar_Repning : ITemplateCalculations
    {
        private static readonly string[] Typ240 = { "64", "68", "72", "630", "750", "900", "950" };
        private static readonly string[] Typ241 = { "68", "72", "84", "500", "530", "600", "630", "670", "710" };

        private static readonly double[] ELista240 = { 0, 131, 131, 235.5, 246, 274, 290 };
        private static readonly double[] ELista241 = { 169, 172, 201, 231, 226, 259, 280, 296, 308 };
        private static readonly double[] JLista240 = { 0, 126, 126, 228.5, 241, 269, 285 };
        private static readonly double[] JLista241 = { 164, 167, 196, 226, 222, 254, 275, 291, 303 };
        private static readonly double[] KLista240 = { 0, 90, 90, 145, 168, 188, 206 };
        private static readonly double[] KLista241 = { 121.5, 122, 140, 162, 177, 187.5, 200, 206, 219 };
        private static readonly double[] ARLista240 = { 0, 10, 11, 19, 22, 27, 28 };
        private static readonly double[] ARLista241 = { 10, 11, 13, 15, 17, 18, 19, 20, 21 };
        private static readonly double[] K1Lista240 = { 0, 92, 93, 175.5, 175, 201, 207 };
        private static readonly double[] K1Lista241 = { 111.5, 116, 137, 154, 141, 171, 185, 200, 205 };
        private static readonly double[] LLista240 = { 0, 244, 245, 416, 460, 520, 557 };
        private static readonly double[] LLista241 = { 317, 321, 372, 430, 416, 490, 525, 548, 577 };
        private static readonly double[] LGLista240 = { 0, 65, 66, 128, 125, 145, 145 };
        private static readonly double[] LGLista241 = { 75, 79, 95, 105, 88, 115, 125, 138, 139 };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;
            List<Bookmark> bm = req != null ? req.Bookmarks : null;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            // TmpFormat: use [Motsv. std. Typ] bookmark if not "0", else Subject
            string motsvBm = GetString(bm, "Motsv. std. Typ");
            string tmpFormat = (!string.IsNullOrEmpty(motsvBm) && !EqualsI(motsvBm, "0"))
                ? motsvBm.ToUpperInvariant().Trim()
                : (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");

            // TmpMS checks original Subject (not TmpBet/TmpFormat)
            bool tmpMS = subject.IndexOf("MS", StringComparison.OrdinalIgnoreCase) >= 0;
            bool tmpSlash = tmpBet.IndexOf('/') >= 0;
            bool tmpOH = tmpBet.IndexOf("OH", StringComparison.OrdinalIgnoreCase) >= 0;

            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : "";
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : "";
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : "";
            string tmpBet4 = tokens.Length > 3 ? tokens[3] : "";
            string tmpBet5 = tokens.Length > 4 ? tokens[4] : "";

            bool tmpSpecDia = EqualsI(tmpBet5, "HB");
            int cntB2 = tmpBet2.Length, cntB3 = tmpBet3.Length;

            // TmpSerie
            string tmpSerie;
            if (tmpSlash && (cntB2 == 3 || cntB2 == 2)) tmpSerie = tmpBet2;
            else if (cntB2 > 4) tmpSerie = tmpBet2.Substring(0, 3);
            else if (cntB2 == 3) tmpSerie = tmpBet2.Substring(0, 1);
            else tmpSerie = tmpBet2.Length >= 2 ? tmpBet2.Substring(0, 2) : tmpBet2;
            int serieInt = TryParseInt(tmpSerie);

            // TmpTyp
            string tmpTyp;
            if (!tmpSlash) tmpTyp = tmpBet2.Length >= 2 ? tmpBet2.Substring(tmpBet2.Length - 2) : tmpBet2;
            else tmpTyp = cntB3 > 4 ? (tmpBet2.Length >= 2 ? tmpBet2.Substring(tmpBet2.Length - 2) : tmpBet2) : tmpBet3;
            double typNum = TryParseDouble(tmpTyp);

            string[] serieTypList = serieInt == 240 ? Typ240 : Typ241;
            int typLista = GetMember(tmpTyp, serieTypList);

            kv["VaLTyp"] = typLista == 0 ? "Produkten finns inte inlagd i mallen" : "";

            // ── TmpG ─────────────────────────────────────────────────────────────
            string tmpG = "";
            if (serieInt == 240)
            {
                if (typNum == 68 || typNum == 72) tmpG = "M6";
                else if (typNum == 630) tmpG = "M8";
                else if (typNum == 750 || typNum == 900 || typNum == 950) tmpG = "G1/8";
            }
            else if (serieInt == 241)
            {
                if (typNum == 68 || typNum == 72 || typNum == 84) tmpG = "M6";
                else if (typNum == 600 || typNum == 670 || typNum == 710) tmpG = "G1/8";
                else if (typNum == 500 || typNum == 530 || typNum == 630) tmpG = "M8";
            }
            kv["SumG"] = tmpG;

            int tmpSlitsVal = typNum < 501 ? 8 : 10;

            // ── B: has @ReplaceSubstring -> FmtDot ───────────────────────────────
            double tmpB;
            if (serieInt == 240)
            {
                if (EqualsI(tmpG, "G1/8")) tmpB = 10;
                else if (EqualsI(tmpG, "M8")) tmpB = 6;
                else if (EqualsI(tmpG, "M6")) tmpB = 3.5;
                else tmpB = 0;
            }
            else
            {
                if (EqualsI(tmpG, "G1/8")) tmpB = 8;
                else if (EqualsI(tmpG, "M8")) tmpB = typNum == 500 ? 6.5 : 6;
                else if (EqualsI(tmpG, "M6")) tmpB = 3.5;
                else tmpB = 0;
            }
            // Lotus: @ReplaceSubstring("[TmpB]";",";".")  -> dot decimal
            kv["SumB"] = "(B) " + FmtDot(tmpB);
            kv["SumBTol"] = "+ 0";
            kv["SumBTolN"] = "- 0.1";

            // ── C: direct [TmpC] -> FmtComma (integers, same either way) ─────────
            double tmpC = EqualsI(tmpG, "G1/8") ? 13 : EqualsI(tmpG, "M8") ? 12 : EqualsI(tmpG, "M6") ? 9 : 0;
            kv["SumC"] = "(C) " + FmtComma(tmpC);
            kv["SumCTol"] = tmpC < 6.1 ? "\u00b1 0.1" : tmpC < 30.1 ? "\u00b1 0.2" : "\u00b1 0.3";

            // ── T: has @ReplaceSubstring -> FmtDot ───────────────────────────────
            double tmpT = EqualsI(tmpG, "G1/8") ? 10 : EqualsI(tmpG, "M8") ? 8.3 : EqualsI(tmpG, "M6") ? 6.3 : 0;
            kv["SumT"] = "(T) " + FmtDot(tmpT);
            kv["SumTTol"] = tmpT < 6.1 ? "\u00b1 0.1" : tmpT < 30.1 ? "\u00b1 0.2" : "\u00b1 0.3";

            // ── D: direct [TmpD] -> FmtComma (integer) ───────────────────────────
            double tmpD = EqualsI(tmpG, "G1/8") ? 5 : EqualsI(tmpG, "M8") ? 4 : EqualsI(tmpG, "M6") ? 3 : 0;
            kv["SumD"] = "(D) " + FmtComma(tmpD);
            kv["SumDTol"] = tmpD < 6.1 ? "\u00b1 0.1" : tmpD < 30.1 ? "\u00b1 0.2" : "\u00b1 0.3";

            // ── H: hardcoded string literals (already correct format) ─────────────
            string tmpH;
            if (serieInt == 240)
                tmpH = typNum < 500 ? "1.2" : typNum < 751 ? "1.5" : "2";
            else if (serieInt == 241)
                tmpH = typNum < 500 ? "1.2" : typNum < 670 ? "1.5" : "2";
            else
                tmpH = typNum < 500 ? "1.2" : typNum < 751 ? "1.5" : "2";
            kv["SumH"] = "(H) " + tmpH;
            kv["SumHTol"] = "\u00b1 0.1";

            // ── E: direct from @Word table -> FmtComma ────────────────────────────
            double[] eLista = serieInt == 240 ? ELista240 : ELista241;
            double tmpE = GetTabVal(eLista, typLista);
            kv["SumE"] = "(E) " + FmtComma(tmpE);
            kv["SumETol"] = GenTolStr(tmpE);

            // ── J: direct from @Word table -> FmtComma ────────────────────────────
            double[] jLista = serieInt == 240 ? JLista240 : JLista241;
            double tmpJ = GetTabVal(jLista, typLista);
            kv["SumJ"] = "(J) " + FmtComma(tmpJ);
            kv["SumJa"] = kv["SumJ"];
            kv["SumJTol"] = GenTolStr(tmpJ);
            kv["SumJaTol"] = kv["SumJTol"];

            // ── F: direct [TmpF] -> FmtComma (integer) ───────────────────────────
            double tmpF = typNum < 85 ? 2 : 3;
            kv["SumF"] = "(F) " + FmtComma(tmpF);
            kv["SumFTol"] = "\u00b1 0.1";

            // ── N: direct [TmpN] -> FmtComma (integer) ───────────────────────────
            double tmpN = typNum < 65 ? 5 : typNum < 85 ? 6 : typNum < 601 ? 7 : typNum < 751 ? 8 : 9;
            kv["SumN"] = "(N) " + FmtComma(tmpN);
            kv["SumNTol"] = tmpN < 6.1 ? "\u00b1 0.1" : "\u00b1 0.2";

            // ── R1: direct "R[TmpR1]" -> FmtComma (4,5 must stay comma) ──────────
            string tmpR1;
            if (tmpMS) tmpR1 = typNum == 630 ? "6" : "";
            else tmpR1 = typNum < 65 ? "4" : typNum < 85 ? "4,5" : "5";
            // "4,5" is already Swedish comma; integers need no conversion
            kv["SumR1"] = "R" + tmpR1;

            // ── R: direct "R[TmpR]" -> string literal ────────────────────────────
            string tmpR = (serieInt == 240 && typNum < 85) ? "1.5"
                : (typNum < 85 || typNum == 378 || typNum == 420 || typNum == 355.6 || typNum == 500) ? "1" : "2.5";
            kv["SumR"] = "R" + tmpR;

            kv["SumV120"] = "120\u00ba";
            kv["SumV45"] = "45\u00ba";

            // ── K: direct from @Word table -> FmtComma ────────────────────────────
            double[] kLista = serieInt == 240 ? KLista240 : KLista241;
            double tmpK = GetTabVal(kLista, typLista);
            kv["SumK"] = "(K) " + FmtComma(tmpK);
            kv["SumKTol"] = GenTolStr(tmpK);

            // ── SV: integer literal ───────────────────────────────────────────────
            const int tmpSV = 12;
            kv["SumSV"] = tmpSV + "\u00b0";
            kv["SumSV2"] = kv["SumSV"];

            // ── AR: direct from @Word table -> FmtComma ───────────────────────────
            double[] arLista = serieInt == 240 ? ARLista240 : ARLista241;
            double tmpAR = GetTabVal(arLista, typLista);
            kv["SumAR"] = "(n) inv/utv " + FmtComma(tmpAR) + " st.";
            kv["SumAR1"] = "(n) " + FmtComma(tmpAR) + " st.";
            kv["SumAR2"] = "(n) " + FmtComma(tmpAR) + " st.";

            // ── VR: @Round(x; 0,1) -> FmtComma ───────────────────────────────────
            double tmpVR = tmpAR > 1 ? Math.Round((360.0 - tmpSV * 2.0) / (tmpAR - 1.0), 1) : 0;
            kv["SumVR"] = FmtComma(tmpVR) + "\u00b0";
            kv["SumVR1"] = kv["SumVR"];
            kv["SumVR2"] = kv["SumVR"];

            // ── K1: direct from @Word table -> FmtComma ───────────────────────────
            double[] k1Lista = serieInt == 240 ? K1Lista240 : K1Lista241;
            double tmpK1 = GetTabVal(k1Lista, typLista);
            kv["SumK1"] = "(K1) " + FmtComma(tmpK1);
            kv["SumK1Tol"] = GenTolStr(tmpK1);

            // L, LG (for chord calc)
            double tmpL = GetTabVal(serieInt == 240 ? LLista240 : LLista241, typLista);
            double tmpLG = GetTabVal(serieInt == 240 ? LGLista240 : LGLista241, typLista);

            // M, M1
            kv["SumM"] = "(M) " + ((serieInt == 240 || serieInt == 241) ? "1.5" : "");
            kv["SumM1"] = "(M1) " + ((serieInt == 240 || serieInt == 241) ? "0.5" : "");

            // ── Chord calculations (all -> FmtComma) ─────────────────────────────
            const double kona = 30.0;

            double tmpd = !tmpSlash ? typNum / 2.0 * 10.0
                : (tmpSpecDia ? TryParseDouble(tmpBet4) : TryParseDouble(tmpBet3));

            double tmpd1;
            if (typNum < 85) tmpd1 = tmpd - 20;
            else if (typNum < 561 || typNum == 630) tmpd1 = tmpd - 30;
            else if (typNum < 751) tmpd1 = tmpd - 40;
            else if (typNum < 1001) tmpd1 = tmpd - 50;
            else tmpd1 = tmpd - 60;

            double konstBOH = Math.Round(Math.Sin((135.0 / 2.0) * Math.PI / 180.0) * 2.0, 4);
            double konst1Rep = Math.Round(Math.Sin((tmpSV / 2.0) * Math.PI / 180.0) * 2.0, 4);
            double konstDelRep = Math.Round(Math.Sin((tmpVR / 2.0) * Math.PI / 180.0) * 2.0, 4);

            double tmpKordaBOH = Math.Round(((tmpd1 + tmpB * 2.0) / 2.0) * konstBOH, 1);
            kv["TmpKordaBOH"] = FmtComma(tmpKordaBOH);

            double tmpKonUtrakning = ((tmpJ - tmpLG) / kona) + tmpd;

            // Only utv (no inv) in this class
            double tmpKorda1RepUtv = Math.Round((tmpKonUtrakning / 2.0) * konst1Rep, 1);
            kv["SumKR1utv"] = FmtComma(tmpKorda1RepUtv - tmpSlitsVal / 2.0);

            double tmpKordaDelRepUtv = Math.Round((tmpKonUtrakning / 2.0) * konstDelRep, 1);
            kv["SumKRDutv"] = FmtComma(tmpKordaDelRepUtv);

            // ── VOS: direct [TmpVOS] = 10 -> FmtComma (integer) ──────────────────
            const int tmpVOS = 10;
            kv["SumVOS"] = FmtComma(tmpVOS);
            kv["SumVOS1"] = FmtComma(tmpVOS);
            double konstOS = Math.Round(Math.Sin((tmpVOS / 2.0) * Math.PI / 180.0) * 2.0, 4);
            double tmpKordaOSutv = Math.Round((tmpKonUtrakning / 2.0) * konstOS, 1);
            kv["SumKOSutv"] = tmpSV < tmpVOS ? "OjSpår utaför Repa" : FmtComma(tmpKordaOSutv);

            // ── Machine ───────────────────────────────────────────────────────────
            string mv = EqualsI(maskinVal, "Skepp6") ? "Skepp6" : EqualsI(maskinVal, "K&T") ? "K&T"
                : EqualsI(maskinVal, "VTR-160") ? "VTR-160" : EqualsI(maskinVal, "MacTurn 550") ? "MacTurn 550" : "";
            kv["SumMaskinValS1"] = "Maskin: " + mv + " - BorrOljehål & Oljespår";
            kv["SumMaskinValS2"] = "Maskin: " + mv + " - Oljespår & Repor";

            bool isSkepp = EqualsI(maskinVal, "Skepp6");
            bool isOther = EqualsI(maskinVal, "K&T") || EqualsI(maskinVal, "VTR-160") || EqualsI(maskinVal, "MacTurn 550");
            bool isAll = isSkepp || isOther;
            string f1 = isSkepp ? "1/1" : isOther ? "1/2" : "";

            for (int i = 1; i <= 9; i++) kv["SumF1_" + i] = f1;
            kv["SumF1_0"] = f1; kv["SumF1_11"] = f1;
            for (int i = 1; i <= 4; i++) kv["SumF2_" + i] = f1;

            kv["SumD1_1"] = isSkepp ? "Skala på borrmaskin" : isOther ? "" : "";
            kv["SumD1_2"] = isAll ? "Skjutmått" : "";
            kv["SumD1_3"] = isAll ? "Djupmått" : "";
            kv["SumD1_4"] = isAll ? "Gängtolk" : "";
            kv["SumD1_5"] = isAll ? "Skjutmått" : "";
            kv["SumD1_6"] = isAll ? "Skjutmått/fasmall" : "";
            kv["SumD1_7"] = isAll ? "Skjutmått" : "";
            kv["SumD1_8"] = isAll ? "Skjutmått" : "";
            kv["SumD1_9"] = isSkepp ? "Höjdrits" : isOther ? "" : "";
            kv["SumD1_0"] = isAll ? "Skjutmått" : "";
            kv["SumD1_11"] = isAll ? "Radieyra" : "";
            kv["SumD2_1"] = isAll ? "Skjutmått" : "";
            kv["SumD2_2"] = isAll ? "Skjutmått" : "";
            kv["SumD2_3"] = isAll ? "Djupmått" : "";
            kv["SumD2_4"] = "";

            foreach (var k in new[]{"SumAF1_1","SumAF1_2","SumAF1_3","SumAF1_4","SumAF1_5",
                                    "SumAF1_6","SumAF1_7","SumAF1_8","SumAF1_9","SumAF1_0","SumAF1_11",
                                    "SumAF2_1","SumAF2_2","SumAF2_3","SumAF2_4"}) kv[k] = "";

            kv["SumTextS1"] = "Grader, frifläckar, slagmärken, repor, valkar och andra defekter samt ojämnheter kontrolleras med ögonmått.";
            kv["SumTextS2"] = "";
            kv["SumTextGängstigning"] = "Max 2x gängstigning";
            kv["SumÖvrigt"] = "1 st. oljeborrhål 180\u00ba från slits\n1 st. utv. oljespår med början 10\u00ba från slitscentrum";

            // ── Ritning ───────────────────────────────────────────────────────────
            string tmpRit = serieInt == 240 ? "7432905"
                : serieInt == 241 ? (tmpTyp == "530" ? "7434589" : "7432906") : tmpBet;
            tmpRit += ":senaste utgåva";
            kv["SumRitNr"] = tmpMS ? subject : tmpRit;
            kv["SumRitNrS2"] = kv["SumRitNr"];

            // ── Validation ────────────────────────────────────────────────────────
            bool hOk = EqualsI(tmpBet3, "H") || EqualsI(tmpBet4, "H");
            kv["VaLH"] = hOk ? ""
                : "Denna Mall är endast för typ OH-H\n" + (EqualsI(tmpBet4, "HB") ? "Använd mall för OH-HB" : "Använd mall för OH");

            return kv;
        }

        private static string ComputePopup(string pub)
        {
            if (string.IsNullOrWhiteSpace(pub)) return "";
            DateTime dt; if (!DateTime.TryParse(pub, out dt)) return "";
            DateTime till = dt.AddDays(14);
            return DateTime.Today <= till.Date
                ? "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)\n\nInformation om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument\n\nPopupruta aktiv till " + till.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                : "";
        }

        private static string GenTolStr(double v)
        {
            if (v < 6.1) return "\u00b1 0.1";
            if (v < 30.1) return "\u00b1 0.2";
            if (v < 120.1) return "\u00b1 0.3";
            if (v < 315.1) return "\u00b1 0.5";
            if (v < 1000.1) return "\u00b1 0.8";
            if (v < 2000.1) return "\u00b1 1.2";
            return "\u00b1 2.0";
        }

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

        private static bool EqualsI(string a, string b) =>
            string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);

        private static int TryParseInt(string s)
        { int v; return int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0; }

        private static double TryParseDouble(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;
            double v;
            return double.TryParse(s.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static double GetDouble(List<Bookmark> bm, string key)
        {
            string r = GetString(bm, key);
            if (string.IsNullOrWhiteSpace(r)) return 0;
            double v;
            return double.TryParse(r.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
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

        // @ReplaceSubstring(x;",";"."): Swedish comma -> dot decimal — used when Lotus explicitly converts
        private static string FmtDot(double v) =>
            v.ToString(CommonFunctions.Culture).Replace(",", ".");

        // Direct [TmpX] / @Word table value: keep Swedish comma — used for all direct substitutions
        private static string FmtComma(double v) =>
            v.ToString("0.################", CommonFunctions.Culture).Replace(".", ",");
    }
}