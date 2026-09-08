using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Klamhylsor_240_241_OH_HB_Oljeanslutningar_Oljespar_Repning : ITemplateCalculations
    {
        private static readonly string[] MachinesAll = { "Skepp6", "K&T", "VTR-160", "MacTurn 550" };

        private static readonly string[] Typ240 = { "750", "900" };
        private static readonly string[] Typ241 = { "68", "600", "630", "710" };

        private static readonly double[] BTab240 = { 8, 10 };
        private static readonly double[] BTab241 = { 3.5, 8, 6, 8 };
        private static readonly double[] ETab240 = { 246, 274 };
        private static readonly double[] ETab241 = { 169, 259, 280, 308 };
        private static readonly double[] JTab240 = { 241, 269 };
        private static readonly double[] JTab241 = { 164, 254, 275, 303 };
        private static readonly double[] KTab240 = { 168, 188 };
        private static readonly double[] KTab241 = { 121.5, 187.5, 200, 219 };
        private static readonly double[] K1Tab240 = { 175, 201 };
        private static readonly double[] K1Tab241 = { 111.5, 171, 185, 205 };
        private static readonly double[] ARTab240 = { 22, 27 };
        private static readonly double[] ARTab241 = { 10, 18, 19, 21 };
        // L and LG only exist for serie 241
        private static readonly double[] LTab241 = { 317, 490, 525, 0, 577 };
        private static readonly double[] LGTab241 = { 75, 115, 125, 0, 139 };

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
            bool tmpOH = tmpBet.IndexOf("OH", StringComparison.OrdinalIgnoreCase) >= 0;

            // Explode " /.-"
            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : "";
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : "";
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : "";
            string tmpBet4 = tokens.Length > 3 ? tokens[3] : "";
            string tmpBet5 = tokens.Length > 4 ? tokens[4] : "";

            // TmpSpecDia = @Matches("HB"; TmpBet5)
            bool tmpSpecDia = EqualsI(tmpBet5, "HB");

            int cntB2 = tmpBet2.Length;

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
            else tmpTyp = tmpSpecDia ? tmpBet4 : tmpBet3;
            int typInt = TryParseInt(tmpTyp);

            // Select tables
            string[] typList = serieInt == 240 ? Typ240 : Typ241;
            int typLista = GetMember(tmpTyp, typList);

            double[] bTab = serieInt == 240 ? BTab240 : BTab241;
            double[] eTab = serieInt == 240 ? ETab240 : ETab241;
            double[] jTab = serieInt == 240 ? JTab240 : JTab241;
            double[] kTab = serieInt == 240 ? KTab240 : KTab241;
            double[] k1Tab = serieInt == 240 ? K1Tab240 : K1Tab241;
            double[] arTab = serieInt == 240 ? ARTab240 : ARTab241;

            // Ritningar
            string tmpRit = serieInt == 241 ? "7432909, 7432903"
                          : serieInt == 240 ? "7432908, 7432901" : "";
            kv["SumRitNr"] = tmpRit;
            kv["SumRitNrS2"] = tmpRit;

            // TmpSlits
            int tmpSlitsVal = typInt < 600 ? 8 : 10;

            // TmpG (gänga)
            string tmpG = "";
            if (serieInt == 240)
                tmpG = typInt > 710 ? "G1/8" : "";
            else if (serieInt == 241)
            {
                if (typInt == 68) tmpG = "M6";
                else if (typInt == 600 || typInt == 710) tmpG = "G1/8";
                else if (typInt == 630 || typInt == 670) tmpG = "M8";
            }
            kv["SumG"] = tmpG;

            // B (mått)
            double tmpB = GetTabVal(bTab, typLista);
            kv["SumB"] = "(B) " + FmtG(tmpB).Replace(".", ",");
            kv["SumBTol"] = "   0";
            kv["SumBTolN"] = "- 0.1";

            // C (gänghålslängd)
            string tmpCStr = EqualsI(tmpG, "M6") ? "9"
                           : EqualsI(tmpG, "G1/8") ? "13"
                           : EqualsI(tmpG, "M8") ? "12" : "";
            double tmpC = TryParseDouble(tmpCStr);
            kv["SumC"] = "(C) " + tmpCStr.Replace(".", ",");
            kv["SumCTol"] = tmpC < 6.1 ? "± 0.1" : tmpC < 30.1 ? "± 0.2" : "± 0.3";

            // T (gänghålsbredd)
            string tmpTStr = EqualsI(tmpG, "M6") ? "6.3"
                           : EqualsI(tmpG, "G1/8") ? "10"
                           : EqualsI(tmpG, "M8") ? "8.3" : "";
            double tmpT = TryParseDouble(tmpTStr);
            kv["SumT"] = "(T) " + tmpTStr.Replace(".", ",");
            kv["SumTTol"] = tmpT < 6.1 ? "± 0.1" : tmpT < 30.1 ? "± 0.2" : "± 0.3";

            // D (oljeborrhål)
            string tmpDStr = EqualsI(tmpG, "M6") ? "3"
                           : EqualsI(tmpG, "G1/8") ? "5"
                           : EqualsI(tmpG, "M8") ? "4" : "";
            kv["SumD"] = "(D) " + tmpDStr.Replace(".", ",");
            kv["SumDTol"] = "± 0.1";

            // H (oljespårsdjup)
            string tmpHStr;
            if (serieInt == 240)
                tmpHStr = typInt < 900 ? "2" : "2.8";
            else if (serieInt == 241)
            {
                if (typInt < 500) tmpHStr = "1.2";
                else if (typInt < 601) tmpHStr = "1.5";
                else tmpHStr = "2";
            }
            else tmpHStr = "";
            kv["SumH"] = "(H) " + tmpHStr;
            kv["SumHTol"] = "± 0.1";

            // E (oljeborrhål längd)
            double tmpE = GetTabVal(eTab, typLista);
            kv["SumE"] = "(E) " + FmtG(tmpE).Replace(".", ",");
            kv["SumETol"] = GenTolStr(tmpE);

            // J (längd till oljespår)
            double tmpJ = GetTabVal(jTab, typLista);
            kv["SumJ"] = "(J) " + FmtG(tmpJ).Replace(".", ",");
            kv["SumJS2"] = kv["SumJ"];
            kv["SumJTol"] = GenTolStr(tmpJ);
            kv["SumJS2Tol"] = kv["SumJTol"];

            // F (diameter oljegenomföringshål)
            string tmpFStr = typInt < 85 ? "2" : "3";
            kv["SumF"] = "(F) " + tmpFStr;
            kv["SumF2"] = kv["SumF"];
            kv["SumFTol"] = "± 0.1";
            kv["SumF2Tol"] = "± 0.1";

            // N (oljespårsbredd)
            double tmpN = typInt < 65 ? 5 : typInt < 85 ? 6 : typInt < 601 ? 7 : typInt < 751 ? 8 : 9;
            kv["SumN"] = "(N) " + FmtG(tmpN);
            kv["SumNTol"] = tmpN < 6.1 ? "± 0.1" : "± 0.2";

            // Radier
            double tmpR1 = typInt < 65 ? 4 : typInt < 85 ? 4.5 : 5;
            double tmpR = typInt < 85 ? 1 : 2.5;
            kv["SumR1"] = "R" + FmtG(tmpR1).Replace(".", ",");
            kv["SumR"] = "R" + FmtG(tmpR).Replace(".", ",");

            // Vinklar
            kv["SumV120"] = "120\u00ba";
            kv["SumV45"] = "45\u00ba";
            kv["SumV30"] = "~30\u00ba";

            // K (längd på repor)
            double tmpK = GetTabVal(kTab, typLista);
            kv["SumK"] = "(K) " + FmtG(tmpK).Replace(".", ",");
            kv["SumKTol"] = GenTolStr(tmpK);

            // SV (startvinkel) — fixed 12
            kv["SumSV"] = "12°";
            kv["SumSV2"] = "12°";

            // AR (antal repor)
            double tmpAR = GetTabVal(arTab, typLista);
            kv["SumAR"] = "(n) inv/utv " + FmtG(tmpAR) + " st.";
            kv["SumAR1"] = "(n) " + FmtG(tmpAR) + " st.";
            kv["SumAR2"] = "(n) " + FmtG(tmpAR) + " st.";

            // VR (vinkel mellan repor)
            double tmpSV = 12.0;
            double tmpVR = tmpAR > 1 ? Math.Round((360.0 - tmpSV * 2.0) / (tmpAR - 1.0), 1) : 0;
            kv["SumVR"] = FmtG(tmpVR).Replace(".",",") + "°";
            kv["SumVR1"] = kv["SumVR"];
            kv["SumVR2"] = kv["SumVR"];

            // K1 (längd till repor)
            double tmpK1 = GetTabVal(k1Tab, typLista);
            kv["SumK1"] = "(K1) " + FmtG(tmpK1).Replace(".", ",");
            kv["SumK1Tol"] = GenTolStr(tmpK1);

            // L, LG — only serie 241 has data
            double tmpL = (serieInt == 241 && typLista >= 1 && typLista <= LTab241.Length) ? LTab241[typLista - 1] : 0;
            double tmpLG = (serieInt == 241 && typLista >= 1 && typLista <= LGTab241.Length) ? LGTab241[typLista - 1] : 0;

            // M, M1 (djup & bredd på repor) — same for both serie
            kv["SumM"] = "(M) " + (serieInt == 240 || serieInt == 241 ? "1.5" : "");
            kv["SumM1"] = "(M1) " + (serieInt == 240 || serieInt == 241 ? "0.5" : "");

            // ── Chord calculations ────────────────────────────────────────────────
            const double kona = 30.0;

            // Tmpd
            double tmpd;
            if (!tmpSlash)
                tmpd = typInt / 2.0 * 10.0;
            else
                tmpd = tmpSpecDia ? TryParseDouble(tmpBet4) : TryParseDouble(tmpBet3);

            // Tmpd1
            double tmpd1;
            if (typInt < 85) tmpd1 = tmpd - 20;
            else if (typInt < 561 || typInt == 630) tmpd1 = tmpd - 30;
            else if (typInt < 751) tmpd1 = tmpd - 40;
            else if (typInt < 1001) tmpd1 = tmpd - 50;
            else tmpd1 = tmpd - 60;

            // Constants
            double tmpKonstBOH = Math.Round(Math.Sin((135.0 / 2.0) * Math.PI / 180.0) * 2.0, 4);
            double tmpKonstant1Rep = Math.Round(Math.Sin((tmpSV / 2.0) * Math.PI / 180.0) * 2.0, 4);
            double tmpKonstantDelRep = Math.Round(Math.Sin((tmpVR / 2.0) * Math.PI / 180.0) * 2.0, 4);

            // Korda BOH
            double tmpKordaBOH = Math.Round(((tmpd1 + tmpB * 2.0) / 2.0) * tmpKonstBOH, 1);

            // Korda 1st Repa inv & utv
            double tmpKordaUtv = ((tmpJ - tmpLG) / kona) + tmpd;
            double tmpKorda1RepInv = Math.Round((tmpd1 / 2.0) * tmpKonstant1Rep, 1);
            double tmpKorda1RepUtv = Math.Round((tmpKordaUtv / 2.0) * tmpKonstant1Rep, 1);
            double sumKR1inv = tmpKorda1RepInv - (tmpSlitsVal / 2.0);
            double sumKR1utv = tmpKorda1RepUtv - (tmpSlitsVal / 2.0);
            kv["SumKR1inv"] = FmtG(sumKR1inv).Replace(".", ",");
            if (subject == "OH 241/630 HB")
                kv["SumKR1utv"] = "61,4";
            else
                kv["SumKR1utv"] = FmtG(sumKR1utv).Replace(".", ",");

            // Korda delning repor inv & utv
            double tmpKordaDelRepInv = Math.Round((tmpd1 / 2.0) * tmpKonstantDelRep, 1);
            double tmpKordaDelRepUtv = Math.Round((tmpKordaUtv / 2.0) * tmpKonstantDelRep, 1);
            kv["SumKRDinv"] = FmtG(tmpKordaDelRepInv).Replace(".", ",");
            kv["SumKRDutv"] = FmtG(tmpKordaDelRepUtv).Replace(".", ",");

            // Oljespår start (VOS=10 fixed)
            const double tmpVOS = 10.0;
            kv["SumVOS"] = FmtG(tmpVOS);
            kv["SumVOS1"] = FmtG(tmpVOS);
            double tmpKonstantOS = Math.Round(Math.Sin((tmpVOS / 2.0) * Math.PI / 180.0) * 2.0, 4);
            double tmpKordaOSinv = Math.Round((tmpd / 2.0) * tmpKonstantOS, 1);
            double tmpKordaOSutv = Math.Round((tmpKordaUtv / 2.0) * tmpKonstantOS, 1);
            bool osOK = tmpSV >= tmpVOS; // always true (12>=10)
            kv["SumKOSinv"] = osOK ? FmtG(tmpKordaOSinv).Replace(".", ",") : "OjSpår utaför Repa";
            kv["SumKOSutv"] = osOK ? FmtG(tmpKordaOSutv).Replace(".", ",") : "OjSpår utaför Repa";

            // ── Machine ───────────────────────────────────────────────────────────
            string mv = EqualsI(maskinVal, "Skepp6") ? "Skepp6"
                      : EqualsI(maskinVal, "K&T") ? "K&T"
                      : EqualsI(maskinVal, "VTR-160") ? "VTR-160"
                      : EqualsI(maskinVal, "MacTurn 550") ? "MacTurn 550" : "";
            kv["SumMaskinValS1"] = "Maskin: " + mv + " - BorrOljehål & Oljespår";
            kv["SumMaskinValS2"] = "Maskin: " + mv + " - Oljespår & Repor";

            bool isSkepp = EqualsI(maskinVal, "Skepp6");
            bool isOther = IsInGroup(maskinVal, new[] { "K&T", "VTR-160", "MacTurn 550" });
            bool isAll = isSkepp || isOther;

            SumFrequencies(kv, isSkepp, isOther);
            SumDevices(kv, isSkepp, isOther, isAll);
            for (int i = 1; i <= 9; i++) kv["SumAF1_" + i] = "";
            kv["SumAF1_0"] = "";
            kv["SumAF1_11"] = "";

            // ── Texts ─────────────────────────────────────────────────────────────
            kv["SumTextS1"] = "";
            kv["SumTextS2"] = "Grader, frifläckar, slagmärken, repor, valkar och andra defekter samt ojämnheter kontrolleras med ögonmått.";
            kv["SumTextGängstigning"] = "Max 2x gängstigning";
            kv["SumÖvrigt"] = "2st. oljeborrhål, Korda (A) = " + FmtG(tmpKordaBOH).Replace(".", ",");
            kv["SumORM"] = "Stämpla Oljeriktningsmarkeringar";

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

        // ── Freq / Devices ────────────────────────────────────────────────────────
        private static void SumFrequencies(Dictionary<string, string> kv, bool isSkepp, bool isOther)
        {
            string f = isSkepp ? "1/1" : isOther ? "1/2" : "";
            for (int i = 1; i <= 9; i++) kv["SumF1_" + i] = f;
            kv["SumF1_0"] = f;
            kv["SumF1_11"] = f;
        }

        private static void SumDevices(Dictionary<string, string> kv, bool isSkepp, bool isOther, bool isAll)
        {
            kv["SumD1_1"] = isSkepp ? "Skala på borrmaskin" : isOther ? "pipborr/djupmått" : "";
            kv["SumD1_2"] = isAll ? "Skjutmått" : "";
            kv["SumD1_3"] = isAll ? "Skjutmått" : "";
            kv["SumD1_4"] = isAll ? "Gängtolk" : "";
            kv["SumD1_5"] = isAll ? "Skjutmått" : "";
            kv["SumD1_6"] = isAll ? "Skjutmått/fasmall" : "";
            kv["SumD1_7"] = isAll ? "Skjutmått" : "";
            kv["SumD1_8"] = isAll ? "Skjutmått" : "";
            kv["SumD1_9"] = isSkepp ? "Höjdrits" : isOther ? "pipborr/djupmått" : "";
            kv["SumD1_0"] = isAll ? "Skjutmått" : "";
            kv["SumD1_11"] = isAll ? "Radieyra" : "";
        }

        // ── Tolerance helpers ─────────────────────────────────────────────────────
        private static string GenTolStr(double v)
        {
            if (v < 6.1) return "± 0.1";
            if (v < 30.1) return "± 0.2";
            if (v < 120.1) return "± 0.3";
            if (v < 315.1) return "± 0.5";
            if (v < 1000.1) return "± 0.8";
            if (v < 2000.1) return "± 1.2";
            return "± 2.0";
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

        private static bool IsInGroup(string mv, string[] g)
        {
            if (string.IsNullOrEmpty(mv)) return false;
            for (int i = 0; i < g.Length; i++)
                if (string.Equals(g[i], mv, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        private static bool EqualsI(string a, string b) =>
            string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);

        private static int TryParseInt(string s)
        {
            int v;
            return int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static double TryParseDouble(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;
            double v;
            return double.TryParse(s.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        // General format: no trailing decimals for integers, preserve decimals otherwise
        private static string FmtG(double v) => v.ToString(CommonFunctions.Culture).Replace(",", ".");
    }
}