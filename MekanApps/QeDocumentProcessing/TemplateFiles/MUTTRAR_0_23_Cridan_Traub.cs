using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class MUTTRAR_0_23_Cridan_Traub : ITemplateCalculations
    {
        private static readonly string[] ArtList = { "HM", "KM", "N" };
        private static readonly string[] MachinesFast = { "3862", "3924", "3602", "Cridan" };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;
            List<Bookmark> bm = req != null ? req.Bookmarks : null;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            // CRITICAL: TmpBet = @Trim(@UpperCase([Subject])) — NO dot->comma replacement!
            string tmpBet = (subject ?? string.Empty).ToUpperInvariant().Trim();
            bool tmpSlash = tmpBet.IndexOf('/') >= 0;
            bool tmpTum = tmpBet.IndexOf("AN", StringComparison.OrdinalIgnoreCase) >= 0
                           || tmpBet.StartsWith("N", StringComparison.OrdinalIgnoreCase);

            // Explode " /.-"
            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : "";
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : "";
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : "";
            string tmpBet4 = tokens.Length > 3 ? tokens[3] : "";
            string tmpBet5 = tokens.Length > 4 ? tokens[4] : "";

            // TmpCountT = position of "T" among Bet1..Bet5
            int tmpCountT = 0;
            string[] betArr = { tmpBet1, tmpBet2, tmpBet3, tmpBet4, tmpBet5 };
            for (int i = 0; i < betArr.Length; i++)
                if (EqualsI(betArr[i], "T")) { tmpCountT = i + 1; break; }

            int tmpCount = tmpBet2.Length;
            int artLista = GetMember(tmpBet1, ArtList);
            string tmpArt = tmpBet1;

            // TmpSerie = if cnt>3: Left(2) else 0
            string tmpSerie = tmpCount > 3 ? tmpBet2.Substring(0, 2) : "0";
            int serieInt = TryParseInt(tmpSerie);

            // TmpTyp = if cnt>3: Right(2) else Bet2
            string tmpTyp = tmpCount > 3
                ? (tmpBet2.Length >= 2 ? tmpBet2.Substring(tmpBet2.Length - 2) : tmpBet2)
                : tmpBet2;
            double typNum = TryParseDouble(tmpTyp);

            // ── IDia bookmark ─────────────────────────────────────────────────────
            double iDia = GetDouble(bm, "IDia");
            double bRedBm = GetDouble(bm, "Bredd"); // Bredd bookmark

            // ── Thread pitch (mm and tum) ─────────────────────────────────────────
            // TmpStmm — based on IDia, stored as Swedish comma string
            string tmpStmmStr;
            double tmpStmmD;
            if (iDia < 10) { tmpStmmStr = ",75"; tmpStmmD = 0.75; }
            else if (iDia < 23) { tmpStmmStr = "1"; tmpStmmD = 1.0; }
            else if (iDia < 50) { tmpStmmStr = "1,5"; tmpStmmD = 1.5; }
            else if (iDia < 150) { tmpStmmStr = "2"; tmpStmmD = 2.0; }
            else { tmpStmmStr = "3"; tmpStmmD = 3.0; }

            // TmpSttum — based on IDia
            string tmpSttum = iDia < 25 ? "32" : iDia < 70 ? "18" : iDia < 150 ? "12"
                            : iDia < 220 ? "8" : "6";
            int sttumI = TryParseInt(tmpSttum);

            // TmpGbemm — direct computed, output in Swedish comma via FmtComma
            double tmpGbemm = tmpStmmD == 0.75 ? iDia + 0.812 : tmpStmmD == 1.0 ? iDia + 1.083
                            : tmpStmmD == 1.5 ? iDia + 1.624 : tmpStmmD == 2.0 ? iDia + 2.165
                            : iDia + 3.248;

            // TmpGbettum
            double tmpGbettum = sttumI == 32 ? (iDia / 25.4) + 0.034 : sttumI == 18 ? (iDia / 25.4) + 0.06
                              : sttumI == 12 ? (iDia / 25.4) + 0.09 : (iDia / 25.4) + 0.098;

            // ── SumGängbet — @text() outputs in Swedish locale -> FmtComma ─────────
            string sumGängbet;
            if (EqualsI(tmpArt, "KM"))
                sumGängbet = "M" + FmtComma(tmpGbemm);
            else if (EqualsI(tmpArt, "N") || EqualsI(tmpArt, "N0"))
                sumGängbet = FmtComma(Math.Round(tmpGbettum, 3));
            else
                sumGängbet = "55";

            // SumStigning — direct output, Swedish comma for stmm decimal
            string sumStigning;
            if (EqualsI(tmpArt, "KM"))
                sumStigning = tmpStmmStr;          // e.g. "1,5" (Swedish comma)
            else if (EqualsI(tmpArt, "N") || EqualsI(tmpArt, "N0"))
                sumStigning = tmpSttum + "UN";     // e.g. "18UN"
            else
                sumStigning = "1/19";

            string sumGänga = sumGängbet + "X" + sumStigning;
            kv["SumGängbet"] = sumGängbet;
            kv["SumStigning"] = sumStigning;
            kv["SumGänga"] = sumGänga;
            kv["SumGängring"] = sumGänga;
            kv["SumGängring2"] = sumGänga;
            kv["SumTolk"] = sumGänga;
            kv["SumKlump"] = sumGänga;
            kv["SumFasmall"] = sumGänga;

            // ── B (bredd) — direct [TmpB] -> FmtComma ────────────────────────────
            kv["SumB"] = "(B) " + FmtComma(bRedBm);
            kv["SumBTol"] = "+ 0";
            // SumBTolN: KM/HM branch uses Bredd thresholds; N branch uses 19 threshold
            if (EqualsI(tmpArt, "KM") || EqualsI(tmpArt, "HM"))
            {
                kv["SumBTolN"] = (bRedBm > 30 ? "- 0.390" : bRedBm > 18 ? "- 0.330"
                                : bRedBm > 10.5 ? "- 0.270" : bRedBm > 6.5 ? "- 0.220"
                                : "- 0.180") + " [3F]";
            }
            else
            {
                kv["SumBTolN"] = (bRedBm > 19 ? "- 0.640" : "- 0.510") + " [3F]";
            }

            // ── d (gängfasdiameter) — direct/Round -> FmtComma ────────────────────
            double tmpd;
            if (EqualsI(tmpArt, "KM") || EqualsI(tmpArt, "HM"))
            {
                double offs = iDia < 20 ? 0.2 : iDia < 50 ? 0.3 : iDia < 150 ? 0.4 : 0.5;
                tmpd = tmpGbemm + offs;
            }
            else
            {
                // N/HM fallthrough — tum branch
                if (sttumI == 32) tmpd = Math.Round(iDia + 1.55, 1);
                else if (sttumI == 18) tmpd = Math.Round(iDia + 2.35, 1);
                else if (sttumI == 12) tmpd = Math.Round(iDia + 3.955, 1);
                else if (sttumI == 8) tmpd = Math.Round(iDia + 4.8, 1);
                else if (sttumI == 11) tmpd = Math.Round(iDia + 3.506, 2);
                else tmpd = 0;
            }
            kv["Sumd"] = "(d) " + FmtComma(tmpd);
            // SumdTol: hardcoded dot strings
            if (EqualsI(tmpArt, "KM") || EqualsI(tmpArt, "HM"))
            {
                kv["SumdTol"] = (tmpd > 180 ? "+ 1.150" : tmpd > 120 ? "+ 1.000"
                               : tmpd > 80 ? "+ 0.870" : tmpd > 50 ? "+ 0.740"
                               : tmpd > 30 ? "+ 0.620" : tmpd > 20 ? "+ 0.520"
                               : "+ 0.430") + " [3F]";
            }
            else
            {
                kv["SumdTol"] = (tmpd > 160 ? "+ 0.750" : tmpd > 75 ? "+ 0.600"
                               : tmpd > 27 ? "+ 0.450" : "+ 0.300") + " [3F]";
            }
            kv["SumdTolN"] = "- 0 [3F]";

            kv["SumV"] = "45\u00ba";

            // ── D1 (innerdiameter) — direct [IDia] -> FmtComma ────────────────────
            kv["SumD1"] = "(D1) " + FmtComma(iDia);
            if (EqualsI(tmpArt, "KM"))
            {
                kv["SumD1Tol"] = (iDia > 150 ? "+ 0.400" : iDia > 50 ? "+ 0.300"
                                : iDia > 20 ? "+ 0.236" : iDia > 10 ? "+ 0.190"
                                : "+ 0.150") + " [3F]";
            }
            else if (EqualsI(tmpArt, "HM"))
            {
                kv["SumD1Tol"] = "+ 0.460  [3F]";
            }
            else if (EqualsI(tmpArt, "N") || EqualsI(tmpArt, "N0"))
            {
                kv["SumD1Tol"] = (iDia > 150 ? "+ 0.343" : iDia > 70 ? "+ 0.229"
                                : iDia > 25 ? "+ 0.152" : "+ 0.086") + " [3F]";
            }
            else kv["SumD1Tol"] = "Fel Typ [3F]";
            kv["SumD1TolN"] = "- 0 [2F]";

            // ── TmpMVL / TmpMV (D2 target values for warm nut) ───────────────────
            string tmpMVL;
            if (EqualsI(tmpBet2, "6") || EqualsI(tmpBet2, "7")) tmpMVL = "140";
            else if (EqualsI(tmpBet2, "8") || EqualsI(tmpBet2, "9")) tmpMVL = "150";
            else if (EqualsI(tmpBet2, "10") || EqualsI(tmpBet2, "11")) tmpMVL = "160";
            else if (EqualsI(tmpBet2, "12") || EqualsI(tmpBet2, "13")) tmpMVL = "180";
            else tmpMVL = "";
            bool isFast3 = EqualsI(maskinVal, "3862") || EqualsI(maskinVal, "3924") || EqualsI(maskinVal, "3602");
            string tmpMV = isFast3
                ? " Målvärde varm mutter: " + tmpMVL : "";

            // ── D2 (medelgängdiameter) — direct -> FmtComma ───────────────────────
            double tmpD2;
            if (EqualsI(tmpArt, "KM"))
            {
                tmpD2 = tmpStmmD == 0.75 ? iDia + 0.325 : tmpStmmD == 1.0 ? iDia + 0.433
                      : tmpStmmD == 1.5 ? iDia + 0.65 : tmpStmmD == 2.0 ? iDia + 0.866
                      : iDia + 1.299;
            }
            else if (EqualsI(tmpArt, "N"))
            {
                tmpD2 = sttumI == 32 ? iDia + 0.343 : sttumI == 18 ? iDia + 0.609
                      : sttumI == 12 ? iDia + 0.917 : sttumI == 11 ? iDia + 0.991
                      : sttumI == 8 ? iDia + 1.374 : iDia + 1.831;
            }
            else
            {
                tmpD2 = 54.144; // HM fixed
            }
            kv["SumD2"] = "(D2) " + FmtComma(tmpD2);
            // SumD2Tol — hardcoded dot strings
            string sumD2Tol;
            if (EqualsI(tmpArt, "KM"))
            {
                sumD2Tol = iDia < 10 ? "+ 0.106" : iDia < 20 ? "+ 0.125" : iDia < 45 ? "+ 0.160"
                         : iDia < 50 ? "+ 0.170" : iDia < 90 ? "+ 0.190" : iDia < 150 ? "+ 0.200"
                         : "+ 0.236";
            }
            else if (EqualsI(tmpArt, "N") || EqualsI(tmpArt, "N0"))
            {
                sumD2Tol = iDia < 12 ? "+ 0.066" : iDia < 17 ? "+ 0.076" : iDia < 24 ? "+ 0.086"
                         : iDia < 35 ? "+ 0.102" : iDia < 50 ? "+ 0.114" : iDia < 70 ? "+ 0.130"
                         : iDia < 75 ? "+ 0.137" : iDia < 85 ? "+ 0.150" : iDia < 100 ? "+ 0.188"
                         : iDia < 150 ? "+ 0.211" : iDia < 190 ? "+ 0.231" : iDia < 200 ? "+ 0.290"
                         : "+ 0.0307";
            }
            else sumD2Tol = "+ 0.300";

            kv["SumD2Tol"] = sumD2Tol + " [3F] " + tmpMV;
            kv["SumD2TolN"] = "- 0 [2F]";

            // ── Ra — NOTE: Swedish comma in "2,5" ─────────────────────────────────
            kv["SumRa"] = "2,5 [2F]";

            // ── Machine ───────────────────────────────────────────────────────────
            bool isFastAll = IsInGroup(maskinVal, MachinesFast);
            bool isTraub = EqualsI(maskinVal, "Traub");
            bool mAny = isFastAll || isTraub;
            string mvErr = mAny ? "" : "INGET MASKINVAL GJORD";
            kv["SumMaskinVal"] = "Maskin: " + maskinVal + mvErr;

            // ── Frequencies ───────────────────────────────────────────────────────
            string fMain = isFastAll ? "6/tim" : isTraub ? "1/10" : "";
            string fRa = mAny ? "1/tim" : "";
            kv["SumF_D2"] = fMain;
            kv["SumF_D1"] = fMain;
            kv["SumF_d"] = fMain;
            kv["SumF_B"] = fMain;
            kv["SumF_Ra"] = fRa;

            // ── Devices ───────────────────────────────────────────────────────────
            // SumD_D2: TmpTyp<6 -> INWI else UD-Apparat
            // For KM 0-5: typNum<6 -> INWI
            string d2DevMain = mAny
                ? (typNum < 6
                    ? "* INWI " + tmpStmmStr + "mm backar"
                    : "* UD-Apparat " + tmpStmmStr + "mm rullar")
                : "";
            kv["SumD_D2"] = d2DevMain;
            kv["SumD_D1"] = isFastAll ? "klump 1570691"
                          : isTraub ? "klump 1570691 & Min, Max mätstickor" : "";
            kv["SumD_d"] = isFastAll ? "Fasmall" : isTraub ? "Fasmall/Skjutmått" : "";
            kv["SumD_B"] = isFastAll ? "Haktolk" : isTraub ? "Skjutmått" : "";
            kv["SumD_Ra"] = mAny ? "Ytjämnhetsmätare" : "";

            // ── AF ────────────────────────────────────────────────────────────────
            // SumAF_D2: INWI vs UD-Apparat based on TmpTyp<6
            if (mAny)
            {
                string d2AfPrefix = typNum < 6
                    ? "INWI inställd med gängring " + sumGänga
                    : "UD-apparat inställd med gängring " + sumGänga + ",";
                kv["SumAF_D2"] = d2AfPrefix + " muttern kontrolleras med gängtolk " + sumGänga + " från båda hållen.";
            }
            else kv["SumAF_D2"] = "";
            kv["SumAF_D1"] = mAny ? "Klump märkt med " + sumGänga + " från båda håll" : "";
            kv["SumAF_d"] = mAny ? "Fasmall märkt " + sumGänga : "";
            kv["SumAF_B"] = "";
            kv["SumAF_Ra"] = "";

            // ── Texts ─────────────────────────────────────────────────────────────
            kv["SumText"] = "Grader, frifläckar, slagmärken, repor, valkar och andra ojämnheter kontrolleras med ögonmått. För övriga mått används skjutmått som också används när fasmall saknas.";
            kv["SumText2"] = "* Gängklockan kontrolleras emot gängring " + sumGänga + " varje timme.<<LineBreak>>Kontrollera stämpling, beteckning, datum och kvalitet";
            kv["SumText3"] = (isFast3 && EqualsI(tmpArt, "KM"))
                ? "Mätning av medelgängdiameter (D2) MINST var 10:e minut på VARM mutter" : "";
            kv["SumText4"] = (isFast3 && EqualsI(tmpArt, "KM"))
                ? "Varje \u00bd tim mäts 5 muttrar som fått kallna samt kontrolleras emot gängtolk & klump från båda håll" : "";
            kv["SumText5"] = "Vid sammanstötning med gängbom lämna in mutter till mätrum för kontrollmätning";
            kv["SumText6"] = isFast3 ? "Kontrolleras enligt styrplan KM 6-13" : "";

            // ── Ritning ───────────────────────────────────────────────────────────
            string ritNr;
            if (EqualsI(tmpArt, "KM")) ritNr = "7439009:4";
            else if (EqualsI(tmpArt, "HM")) ritNr = "224673:3";
            else if (EqualsI(tmpArt, "HML")) ritNr = "222755:3";
            else if (EqualsI(tmpArt, "KML")) ritNr = "7439009:4";
            else if (EqualsI(tmpArt, "AN")) ritNr = "7430363:7";
            else if (EqualsI(tmpArt, "N")) ritNr = "7430363:7";
            else if (EqualsI(tmpArt, "N0")) ritNr = "7431662:1";
            else ritNr = "224673:3";
            kv["SumRitning"] = "Produktritning: " + ritNr;

            // ── Klassade egenskaper — NOTE: backslashes preserved as Lotus stores them
            kv["SumKlEgenskaper"] = "PRODUCTION NUTS & SLEEVES & HOUSINGSALLMÄNKLASSADE EGENSKAPERKlassade egenskaper muttrar";

            return kv;
        }

        // ── Popup ──────────────────────────────────────────────────────────────────
        private static string ComputePopup(string pub)
        {
            if (string.IsNullOrWhiteSpace(pub)) return "";
            DateTime dt; if (!DateTime.TryParse(pub, out dt)) return "";
            DateTime till = dt.AddDays(14);
            return DateTime.Today <= till.Date
                ? "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)<<LineBreak>>Information om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument<<LineBreak>>Popupruta aktiv till " + till.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                : "";
        }

        // ── Utilities ─────────────────────────────────────────────────────────────
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

        // Direct [TmpX] / @text() / @Round — Swedish comma decimal (e.g. "1,5", "0,812")
        private static string FmtComma(double v) =>
            v.ToString("0.################", CommonFunctions.Culture).Replace(".", ",");
    }
}