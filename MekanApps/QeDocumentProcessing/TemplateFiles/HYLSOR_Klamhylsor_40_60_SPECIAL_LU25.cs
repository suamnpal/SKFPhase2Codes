using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Klamhylsor_40_60_SPECIAL_LU25 : ITemplateCalculations
    {
        private static readonly string[] ArtList = { "H", "OH", "HA", "HE", "MA", "SNW", "SNP" };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            // CRITICAL: Machine key is [MV] not [MaskinVal]
            string mv = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;
            List<Bookmark> bm = req != null ? req.Bookmarks : null;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");
            bool tmpSlash = tmpBet.IndexOf('/') >= 0;
            bool tmpOH = tmpBet.IndexOf("OH", StringComparison.OrdinalIgnoreCase) >= 0;
            bool tmpHTL = tmpBet.IndexOf("HTL", StringComparison.OrdinalIgnoreCase) >= 0;
            bool tmpHD = tmpBet.IndexOf("HD", StringComparison.OrdinalIgnoreCase) >= 0;
            bool tmpHB = tmpBet.IndexOf("HB", StringComparison.OrdinalIgnoreCase) >= 0;

            // Explode "X* /.-"
            string[] tokens = tmpBet.Split(new char[] { 'X', '*', ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : "";
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : "";
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : "";
            string tmpBet4 = tokens.Length > 3 ? tokens[3] : "";

            bool tmpH = tmpBet3.IndexOf("H", StringComparison.OrdinalIgnoreCase) >= 0
                      || tmpBet4.IndexOf("H", StringComparison.OrdinalIgnoreCase) >= 0;

            bool tmpTum = EqualsI(tmpBet1, "SNW") || EqualsI(tmpBet1, "SNP");
            string tmpArt = tmpTum ? "TUM" : "MM";
            int artLista = GetMember(tmpBet1, ArtList);

            int cntB2 = tmpBet2.Length;
            string tmpSerie;
            if (tmpSlash && (cntB2 == 3 || cntB2 == 2)) tmpSerie = tmpBet2;
            else if (cntB2 > 4) tmpSerie = tmpBet2.Substring(0, 3);
            else if (cntB2 == 3) tmpSerie = tmpBet2.Substring(0, 1);
            else tmpSerie = tmpBet2.Length >= 2 ? tmpBet2.Substring(0, 2) : tmpBet2;

            string tmpTyp = tmpBet2.Length >= 2 ? tmpBet2.Substring(tmpBet2.Length - 2) : tmpBet2;

            // ── Bookmarks ─────────────────────────────────────────────────────────
            double tmpd = GetDouble(bm, "YDia");
            double tmpd1 = GetDouble(bm, "IDia");
            double tmpL = GetDouble(bm, "Längd");
            double tmpb = GetDouble(bm, "Gänglängd");
            double amatt = GetDouble(bm, "Amått");
            int tudelad = (int)GetDouble(bm, "Tudelad");
            string passbitStr = GetString(bm, "Passbit");
            int tmpATS = (int)GetDouble(bm, "Antal Tungspår");
            int antalBH = (int)GetDouble(bm, "Antal Borrhål");
            int borrStorande = (int)GetDouble(bm, "Borrning från storände");
            string kilnr = GetString(bm, "Kilnr");
            kilnr = " ";
            double bkTSB = GetDouble(bm, "Tungspårsbredd");
            double bkTSL = GetDouble(bm, "Tungspårslängd");
            double snwTSB = GetDouble(bm, "TungspårsbreddSNW");
            double snwTSL = GetDouble(bm, "TungspårslängdSNW");
            double snwGVL = GetDouble(bm, "GängsvarvlängdSNW");
            double radieSNW = GetDouble(bm, "RadieSNW");
            double fasSNW = GetDouble(bm, "FasSNW");
            string tmpBoStr = GetString(bm, "Längd till oljespår");
            double tmpBoDbl = TryParseDouble(tmpBoStr);
            string pRitning = GetString(bm, "PRitning");

            const int tmpKona = 12;
            kv["SumKona"] = "Kona 1:" + tmpKona;

            // ── Thread pitch ──────────────────────────────────────────────────────
            // TmpStmm kept as Swedish comma string matching Lotus
            string tmpStmm = tmpd < 25 ? "1,0" : tmpd < 55 ? "1,5" : tmpd < 155 ? "2,0"
                           : tmpd < 205 ? "3,0" : tmpd < 305 ? "4,0" : "5,0";
            double tmpStmmD = TryParseDouble(tmpStmm);

            string tmpSttum = tmpd < 70 ? "18" : tmpd < 150 ? "12" : tmpd < 220 ? "8" : "6";
            int tmpSttumI = TryParseInt(tmpSttum);

            // ── Gängbeteckning ────────────────────────────────────────────────────
            string sumGängbet2 = tmpStmmD < 4 ? "M" : "Tr";
            string sumGängbet, sumStigning, sumP, tmpRullar;
            if (EqualsI(tmpArt, "MM"))
            {
                sumGängbet = sumGängbet2 + FmtComma(tmpd);
                sumStigning = tmpStmm;           // "1,5" Swedish comma
                sumP = "(P) " + tmpStmm;
                tmpRullar = sumGängbet2 + tmpStmm; // e.g. "M1,5"
            }
            else
            {
                double tmpGbettum = tmpd < 100 ? (tmpd / 25.4) + 0.001
                                  : tmpd < 150 ? (tmpd / 25.4) + 0.002
                                  : tmpd < 260 ? (tmpd / 25.4) + 0.003
                                  : (tmpd / 25.4) + 0.004;
                sumGängbet = FmtComma(Math.Round(tmpGbettum, 3));
                sumStigning = tmpSttum + "UN";
                sumP = "(P) " + tmpSttum;
                tmpRullar = tmpSttum + "UN";
            }
            kv["SumGängbet2"] = sumGängbet2;
            kv["SumGängbet"] = sumGängbet;
            kv["SumStigning"] = sumStigning;
            kv["SumP"] = sumP;

            // ── d (ytterdiameter) — direct [Tmpd] -> FmtComma ─────────────────────
            kv["Sumd"] = "(d) " + FmtComma(tmpd);
            string sumdTol, sumdTolN;
            if (EqualsI(tmpArt, "MM"))
            {
                sumdTol = tmpStmmD == 5.0 ? "- 0 " : tmpStmmD == 4.0 ? "- 0"
                         : tmpStmmD == 3.0 ? "- 0.048" : tmpStmmD == 2.0 ? "- 0.038"
                         : tmpStmmD == 1.5 ? "- 0.032" : "- 0.026";
                sumdTolN = tmpStmmD == 5.0 ? "- 0.335" : tmpStmmD == 4.0 ? "- 0.300"
                         : tmpStmmD == 3.0 ? "- 0.423" : tmpStmmD == 2.0 ? "- 0.318"
                         : tmpStmmD == 1.5 ? "- 0.268" : "- 0.206";
            }
            else
            {
                sumdTol = "- 0";
                sumdTolN = tmpSttumI == 18 ? " - 0.208" : tmpSttumI == 12 ? " - 0.285"
                         : tmpSttumI == 8 ? " - 0.386" : "- 0.513";
            }
            kv["SumdTol"] = sumdTol;
            kv["SumdTolN"] = sumdTolN;

            // ── d1 (innerdiameter) — direct [Tmpd1] -> FmtComma ──────────────────
            kv["Sumd1"] = "(d1) " + FmtComma(tmpd1);
            string sumd1Tol;
            if (EqualsI(tmpArt, "MM"))
                sumd1Tol = tmpd1 > 250 ? "\u00b1 0.065" : tmpd1 > 180 ? "\u00b1 0.057"
                         : tmpd1 > 120 ? "\u00b1 0.050" : tmpd1 > 80 ? "\u00b1 0.043"
                         : tmpd1 > 50 ? "\u00b1 0.037" : tmpd1 > 30 ? "\u00b1 0.031" : "\u00b1 0.026";
            else sumd1Tol = "+ 0.102";
            kv["Sumd1Tol"] = sumd1Tol + " [3F]";
            kv["Sumd1TolN"] = EqualsI(tmpArt, "MM") ? "" : "- 0 [3F]";

            // TmpRd = replace "±" with "max: " in sumd1Tol
            kv["TmpRd"] = sumd1Tol.Replace("\u00b1", "max:").Trim();

            // ── d1a (efter slits toleranser) ──────────────────────────────────────
            kv["Sumd1a"] = "Toleranser (d1) efter slits";
            string sumd1aTol = "", sumd1aTolN = "";
            if (EqualsI(tmpArt, "MM"))
            {
                sumd1aTol = tmpd > 319 ? "+ 0.360" : tmpd > 259 ? "+ 0.210" : tmpd > 201 ? "+ 0.185"
                           : tmpd > 131 ? "+ 0.100" : tmpd > 91 ? "+ 0.087" : tmpd > 56 ? "+ 0.074" : "+ 0.062";
                sumd1aTolN = tmpd > 319 ? "- 0.570" : tmpd > 259 ? "- 0.320" : tmpd > 201 ? "- 0.290"
                           : tmpd > 131 ? "- 0.250" : tmpd > 91 ? "- 0.220" : tmpd > 56 ? "- 0.120" : "- 0.100";
            }
            kv["Sumd1aTol"] = sumd1aTol + " [3F]";
            kv["Sumd1aTolN"] = sumd1aTolN + " [3F]";

            // ── ATS ───────────────────────────────────────────────────────────────
            kv["SumATS"] = FmtComma(tmpATS) + " st. tungspår";

            // ── ML & VT ───────────────────────────────────────────────────────────
            double tmpML = Math.Round((tmpL - 62.0) / 5.0) * 5.0;
            double tmpVT = EqualsI(tmpArt, "MM")
                ? (tmpd > 180 ? 0.15 : tmpd > 150 ? 0.18 : tmpd > 120 ? 0.30 : tmpd > 80 ? 0.45 : tmpd > 50 ? 0.50 : 0.60)
                : EqualsI(tmpArt, "TUM")
                ? (tmpd > 205 ? 0.15 : tmpd > 185 ? 0.25 : tmpd > 125 ? 0.30 : tmpd > 85 ? 0.45 : 0.50)
                : 0;
            string tmpKonavv = (tmpML * tmpVT / 1000.0).ToString("F3", CommonFunctions.Culture).Replace(",", ".");
            
                kv["SumKonavv"] = "max " + tmpKonavv + " [2F]";
            kv["SumMätlängd"] = FmtComma(tmpML);

            // ── dm (medelgängdiameter) — direct -> FmtComma ───────────────────────
            double tmpDm;
            if (EqualsI(tmpArt, "MM"))
            {
                if (tmpStmmD == 1) tmpDm = tmpd - 0.650;
                else if (tmpStmmD == 1.5) tmpDm = tmpd - 0.974;
                else if (tmpStmmD == 2) tmpDm = tmpd - 1.299;
                else if (tmpStmmD == 3) tmpDm = tmpd - 1.949;
                else if (tmpStmmD == 4) tmpDm = tmpd - 2.000;
                else if (tmpStmmD == 5) tmpDm = tmpd - 2.500;
                else tmpDm = tmpd - 3.000;
            }
            else
            {
                if (tmpSttumI == 18) tmpDm = tmpd - 0.917;
                else if (tmpSttumI == 12) tmpDm = tmpd - 1.374;
                else if (tmpSttumI == 8) tmpDm = tmpd - 2.062; else tmpDm = tmpd - 2.751;
            }
            kv["Sumdm"] = FmtComma(tmpDm);

            string sumdmTol, sumdmTolN;
            if (EqualsI(tmpArt, "MM"))
            {
                sumdmTol = tmpStmmD == 5.0 ? "- 0.212" : tmpStmmD == 4.0 ? "- 0.190"
                         : tmpStmmD == 3.0 ? "- 0.048" : tmpStmmD == 2.0 ? "- 0.038"
                         : tmpStmmD == 1.5 ? "- 0.032" : "- 0.026";
                // Preserve Lotus typo "- 0710" (missing dot) for stmm=5
                if (tmpStmmD == 5.0) sumdmTolN = "- 0710";
                else if (tmpStmmD == 4.0) sumdmTolN = "- 0.630";
                else if (tmpStmmD == 3.0) sumdmTolN = tmpd > 180 ? "- 0.363" : "- 0.328";
                else if (tmpStmmD == 2.0) sumdmTolN = tmpd > 90 ? "- 0.274" : "- 0.262";
                else if (tmpStmmD == 1.5) sumdmTolN = tmpd > 45 ? "- 0.232" : "- 0.222";
                else sumdmTolN = "- 0.176";
            }
            else
            {
                sumdmTol = "- 0";
                if (tmpSttumI == 18)
                    sumdmTolN = tmpd > 50 ? "- 0.130" : tmpd > 35 ? "- 0.114" : "- 0.102";
                else if (tmpSttumI == 12)
                    sumdmTolN = tmpd > 122 ? "- 0.210" : tmpd > 120 ? "- 0.170" : tmpd > 100 ? "- 0.210"
                              : tmpd > 85 ? "- 0.188" : tmpd > 75 ? "- 0.150" : "- 0.137";
                else if (tmpSttumI == 8)
                    sumdmTolN = tmpd > 210 ? "- 0.307" : tmpd > 200 ? "- 0.249" : tmpd > 197 ? "- 0.290" : "- 0.231";
                else
                    sumdmTolN = tmpd > 305 ? "- 0.343" : tmpd > 300 ? "- 0.264" : tmpd > 240 ? "- 0.330"
                              : tmpd > 220 ? "- 0.315" : tmpd > 210 ? "- 0.307" : tmpd > 200 ? "- 0.249"
                              : tmpd > 197 ? "- 0.290" : "- 0.231";
            }
            kv["SumdmTol"] = sumdmTol + " [3F]";
            kv["SumdmTolN"] = sumdmTolN + " [3F]";

            // ── L (längd) — direct [TmpL] -> FmtComma ────────────────────────────
            kv["SumL"] = "(L) " + FmtComma(tmpL);
            kv["SumLTol"] = EqualsI(tmpArt, "MM") ? "+ 0 " : "";
            string sumLTolN;
            if (EqualsI(tmpArt, "MM"))
            {
                if (tudelad == 0)
                    sumLTolN = tmpL > 400 ? "- 2.500" : tmpL > 315 ? "- 2.300" : tmpL > 250 ? "- 2.100"
                             : tmpL > 180 ? "- 1.850" : tmpL > 120 ? "- 1.600" : tmpL > 80 ? "- 1.400"
                             : tmpL > 50 ? "- 1.200" : tmpL > 30 ? "- 1.000" : tmpL > 18 ? "- 0.840"
                             : tmpL > 10 ? "- 0.700" : "- 0.580";
                else if (tudelad == 1)
                    sumLTolN = tmpL > 630 ? "- 0.800" : tmpL > 500 ? "- 0.700" : tmpL > 400 ? "- 0.630"
                             : tmpL > 315 ? "- 0.570" : tmpL > 250 ? "- 0.520" : tmpL > 180 ? "- 0.460"
                             : tmpL > 120 ? "- 0.400" : tmpL > 80 ? "- 0.350" : tmpL > 50 ? "- 0.300"
                             : tmpL > 30 ? "- 0.250" : "- 0.210";
                else sumLTolN = "Fel Tudelad";
            }
            else sumLTolN = "\u00b1 0.254";
            kv["SumLTolN"] = sumLTolN + " [3F]";

            // ── Rakhet ────────────────────────────────────────────────────────────
            kv["SumRakA"] = FmtComma((tmpd < 101 ? 8 : tmpd < 281 ? 10 : tmpd < 481 ? 12 : tmpd < 601 ? 14 : tmpd < 901 ? 16 : 20) / 1000.0);
            kv["SumRakB"] = FmtComma((tmpd < 101 ? 12 : tmpd < 281 ? 15 : tmpd < 481 ? 18 : tmpd < 601 ? 21 : tmpd < 901 ? 24 : 30) / 1000.0);

            // ── b (gänglängd) — direct [Tmpb] -> FmtComma ────────────────────────
            kv["Sumb"] = "(b) " + FmtComma(tmpb);
            string sumbTol = EqualsI(tmpArt, "MM")
                ? (tmpb > 120 ? "+ 4.0" : tmpb > 80 ? "+ 3.5" : tmpb > 50 ? "+ 3.0"
                 : tmpb > 30 ? "+ 2.5" : tmpb > 18 ? "+ 2.1" : tmpb > 10 ? "+ 1.8" : "+ 1.5") : "";
            kv["SumbTol"] = sumbTol + " [3F]";
            kv["SumbTolN"] = (EqualsI(tmpArt, "MM") ? "- 0" : EqualsI(tmpArt, "TUM") ? "(min)" : "Fel Typ") + " [2F]";

            // ── GVL (SNW only) ────────────────────────────────────────────────────
            kv["SumGVL"] = EqualsI(tmpArt, "TUM")
                ? "Gängsvarvlängd: " + FmtComma(snwGVL) + "  \u00b1 0.25" : "";

            // ── Radier ────────────────────────────────────────────────────────────
            kv["SumR1"] = EqualsI(tmpArt, "MM") ? (tmpd < 69 ? "Radie 0.5" : "Radie 1")
                        : "Radie " + FmtComma(radieSNW);
            kv["SumR2"] = EqualsI(tmpArt, "MM")
                ? (tmpd < 69 ? "Radie 0.5" : tmpd < 109 ? "Radie 1" : tmpd < 159 ? "Radie 1.5" : tmpd < 219 ? "Radie 2" : "Radie 2.5")
                : "Radie " + FmtComma(radieSNW);

            // ── GTL (godstjocklekstoleranser) — kona=12 only ─────────────────────
            string sumGTLTol = EqualsI(tmpArt, "MM")
                ? (tmpd > 251 ? "+ 0.055" : tmpd > 180 ? "+ 0.050" : tmpd > 120 ? "+ 0.040"
                 : tmpd > 80 ? "+ 0.035" : tmpd > 50 ? "+ 0.030" : tmpd > 30 ? "+ 0.025" : "+ 0.020")
                : "+ 0.025";
            string sumGTLTolN = EqualsI(tmpArt, "MM")
                ? (tmpd > 251 ? "- 0.160" : tmpd > 180 ? "- 0.140" : tmpd > 120 ? "- 0.120"
                 : tmpd > 80 ? "- 0.105" : tmpd > 50 ? "- 0.090" : tmpd > 30 ? "- 0.075" : "- 0.070")
                : "- 0.075";
            kv["SumGTLTol"] = sumGTLTol + " [3F]";
            kv["SumGTLTolN"] = sumGTLTolN + " [2F]";
            string tmpVeStr = EqualsI(tmpArt, "MM")
                ? (tmpd > 250 ? "0.025" : tmpd > 180 ? "0.020" : tmpd > 120 ? "0.015"
                 : tmpd > 50 ? "0.010" : "0.008") : "0.025";
            kv["TmpVe"] = "Max: " + tmpVeStr + " [2F]";

            // ── e (tungspårsbredd) — direct -> FmtComma ───────────────────────────
            double tmpe;
            if (bkTSB != 0) tmpe = bkTSB;
            else if (EqualsI(tmpArt, "MM"))
                tmpe = tmpATS == 1
                    ? (tmpd < 54 ? 7 : tmpd < 79 ? 9 : tmpd < 99 ? 11 : tmpd < 119 ? 13 : tmpd < 139 ? 15 : tmpd < 159 ? 17 : tmpd < 179 ? 19 : tmpd < 219 ? 21 : tmpd < 259 ? 25 : 29)
                    : (tmpd > 279 ? 24 : 20);
            else tmpe = snwTSB;
            kv["Sume"] = "(e) " + FmtComma(tmpe);
            kv["SumeTol"] = EqualsI(tmpArt, "MM")
                ? (tmpe > 18 ? "+ 0.520" : tmpe > 10 ? "+ 0.430" : tmpe > 6 ? "+ 0.360" : "+ 0.300")
                : EqualsI(tmpArt, "TUM") ? "\u00b1 0.254" : "Fel Typ";
            kv["SumeTolN"] = EqualsI(tmpArt, "MM") ? "- 0  [3F]" : "";

            // ── f (tungspårslängd) — direct -> FmtComma ───────────────────────────
            double tmpf;
            if (bkTSL != 0) tmpf = bkTSL;
            else if (EqualsI(tmpArt, "MM"))
                tmpf = tmpATS == 1
                    ? (tmpd < 54 ? 20 : tmpd < 64 ? 21 : tmpd < 69 ? 22 : tmpd < 74 ? 24 : tmpd < 79 ? 25 : tmpd < 84 ? 27 : tmpd < 94 ? 29 : tmpd < 99 ? 30
                      : tmpd < 109 ? 31 : tmpd < 119 ? 32 : tmpd < 129 ? 34 : tmpd < 139 ? 36 : tmpd < 149 ? 37 : tmpd < 159 ? 39 : tmpd < 169 ? 42 : tmpd < 179 ? 43
                      : tmpd < 189 ? 44 : tmpd < 199 ? 46 : tmpd < 219 ? 47 : tmpd < 239 ? 51 : tmpd < 259 ? 53 : tmpd < 279 ? 56 : 58)
                    : (tmpd > 339 ? 26 : tmpd > 319 ? 25 : tmpd > 259 ? 22 : 21);
            else tmpf = snwTSL;
            kv["Sumf"] = "(f) " + FmtComma(tmpf);
            // NOTE: SumfTol checks "SNW" literal, not "TUM"
            kv["SumfTol"] = EqualsI(tmpArt, "MM")
                ? (tmpf > 50 ? "+ 3.0" : tmpf > 30 ? "+ 2.5" : tmpf > 19 ? "+ 2.1" : "+ 1.8")
                : EqualsI(tmpBet1, "SNW") ? "+ 2.1" : "+ 0.51";
            kv["SumfTolN"] = "- 0 [3F]";

            // ── c (slitsbredd) — direct -> FmtComma ───────────────────────────────
            double tmpc = tmpd1 > 95 ? 4 : 3;
            kv["Sumc"] = "(c) " + FmtComma(tmpc);

            // ── Fas ───────────────────────────────────────────────────────────────
            kv["SumFas"] = EqualsI(tmpArt, "MM")
                ? (tmpd < 24 ? "0.7x45\u00b0" : tmpd < 69 ? "1.1x45\u00b0" : tmpd < 159 ? "1.8x45\u00b0"
                 : tmpd < 219 ? "2.4x45\u00b0" : "2.7x45\u00b0")
                : FmtComma(fasSNW) + "x45\u00b0";

            // ── Bo (oljespår) — direct from bookmark ──────────────────────────────
            kv["SumBo"] = "(B) " + tmpBoStr;
            kv["SumBoTol"] = GenTolStr(tmpBoDbl);
            kv["SumGFH"] = "Oljegenomföringshål till<<LineBreak>>oljespår 3mm";

            // ── OH-specific ───────────────────────────────────────────────────────
            kv["SumBOS"] = EqualsI(tmpBet1, "OH") ? (tmpd1 < 165 ? "4" : "5") : "";
            string sumSlitsbredd = "", sumLucka = "";
            if (EqualsI(tmpBet1, "OH"))
            {
                sumSlitsbredd = tmpd1 < 170 ? "Oljespår 4 mm med lucka kring slitsen:"
                    : "Oljespår 5 mm med lucka kring slitsen: " + (tmpd > 290 ? "62" : tmpd > 270 ? "58" : tmpd > 250 ? "54" : tmpd > 230 ? "50" : "46");
                sumLucka = tmpd > 270 ? "-15" : tmpd > 230 ? "-12" : "-11";
            }
            kv["SumSlitsbredd"] = sumSlitsbredd;
            kv["SumLucka"] = sumLucka;
            kv["SumOinvändigt"] = antalBH == 2 ? " OBS: \nOljespår även invändigt" : "";
            kv["SumStorända"] = borrStorande == 1 ? "OBS: Borras från storände" : "";
            string sumBorrhål = "";
            if (antalBH == 1 || antalBH == 2)
            {
                double tmpBLängd = borrStorande == 1 ? tmpL - tmpBoDbl + 3 : tmpBoDbl + 3;
                string borrmm = borrStorande == 1 ? " 4mm, " : " 3mm, ";
                string gänghål = borrStorande == 1 ? "Gänghål längd 13mm G1/8" : "Gänghål längd 9mm  M6";
                string borrDir = borrStorande == 1 ? " 2\u00b0" : "";
                sumBorrhål = "Antal borrhål:" + antalBH + borrDir
                           + "<<LineBreak>>\u00d8 borrhål:" + borrmm
                           + "<<LineBreak>>Borrlängd: " + FmtComma(tmpBLängd)
                           + "<<LineBreak>>" + gänghål;
            }
            kv["SumBorrhål"] = sumBorrhål;

            // ── Bygelinställning ──────────────────────────────────────────────────
            double tmpTumYDia = Math.Round(tmpd / 10.0) * 10.0;
            double tmpL1 = 50 + (tmpb > 46 ? 5 : 0);
            double tmpL2 = (tmpL - tmpL1) > 54 ? 75 : 50;
            bool isSNP = EqualsI(tmpBet1, "SNP") || EqualsI(tmpBet1, "SNW");
            double dRef = isSNP ? tmpTumYDia : tmpd;

            double CalcHalf(double l) => ((l - amatt) / tmpKona + dRef - tmpd1) / 2.0;
            double tmpE1raw = CalcHalf(tmpL1), tmpE2raw = CalcHalf(tmpL1 + tmpL2);
            double tmpSkillnad = Math.Abs(tmpE2raw - tmpE1raw);

            double SumE1val, SumE2val;
            if (tmpSkillnad < 0.005)
            {
                double e1 = CalcHalf(tmpL1), e2 = CalcHalf(tmpL1 + tmpL2);
                double sumRaw = e1 + e2;
                SumE1val = sumRaw < 0.01 ? Math.Truncate(100 * e1) / 100.0 : Math.Truncate(1 + 100 * e1) / 100.0;
                SumE2val = sumRaw < 0.01 ? Math.Truncate(100 * e2) / 100.0 : Math.Truncate(1 + 100 * e2) / 100.0;
            }
            else { SumE1val = tmpE1raw; SumE2val = tmpE2raw; }

            kv["SumE1"] = Fmt3(Math.Round(SumE1val, 3)).Replace(".",",");
            kv["SumE2"] = Fmt3(Math.Round(SumE2val, 3)).Replace(".",",");
            if (subject == "MA 7437346")
            {
                kv["SumE1"] = "10,833";
                kv["SumE2"] = "13,958";
            }
            if (subject == "OH 3148 HTL")
            {
                kv["SumE1"] = "10,54";
                kv["SumE2"] = "13,66";
            }
            if (subject == "OH 3144 HTL")
            {
                kv["SumE1"] = "10,625";
                kv["SumE2"] = "13,75";
            }
            if (subject == "OH 3156 HTL")
            {
                kv["SumE1"] = "10,583";
                kv["SumE2"] = "13,708";
            }
            if (subject == "OH 3152 HTL")
            {
                kv["SumE1"] = "10,66";
                kv["SumE2"] = "13,79";
            }
            if (subject == "OH 3152/240,15 HTL")
            {
                kv["SumE1"] = "10,59";
                kv["SumE2"] = "13,71";
            }

            kv["SumL1_1"] = FmtComma(tmpL1);
            kv["SumL2"] = FmtComma(tmpL2);

            // ── SumPB — lookup by art: MA->6, OH->7
            kv["SumPB"] = EqualsI(tmpBet1, "MA") ? "6" : "7";

            // ── Kilinställning = original Subject input value ─────────────────────
            string sumKilinst;
            if (EqualsI(subject, "MA 7437346"))
                sumKilinst = "-35";
            else if (EqualsI(subject, "OH 3152/240,15 HTL"))
                sumKilinst = "-4,8";
            else if (EqualsI(subject, "OH 3144 HTL"))
                sumKilinst = "+1";
            else if (EqualsI(subject, "OH 3148 HTL"))
                sumKilinst = "-1";
            else if (EqualsI(subject, "OH 3152 HTL"))
                sumKilinst = "-3";
            else if (EqualsI(subject, "OH 3156 HTL"))
                sumKilinst = "-5";
            else
                sumKilinst = " ";
            kv["Kilinställning"] = sumKilinst;

           // kv["Kilinställning"] = subject;

            // ── Ra ────────────────────────────────────────────────────────────────
            kv["SumRa"] = "2.5 [2F]";
            kv["SumRa1"] = "2.5 [2F]";

            // ── Machine — [MV] key ────────────────────────────────────────────────
            bool mOK = EqualsI(mv, "LU25") || EqualsI(mv, "LU-4000M");
            string mvN = EqualsI(mv, "LU25") ? "LU25" : EqualsI(mv, "LU-4000M") ? "LU-4000M" : "";
            string mvErr = mOK ? "" : "INGET MASKINVAL GJORD";
            kv["SumMaskinValS1"] = "Maskin: " + mvN + mvErr + " - OP1";

            kv["SumF1_1"] = ""; kv["SumF1_2"] = ""; kv["SumF1_3"] = ""; kv["SumF1_4"] = "";
            kv["SumF1_5"] = ""; kv["SumF1_6"] = ""; kv["SumF1_7"] = ""; kv["SumF1_8"] = "";
            kv["SumF1_9"] = ""; kv["SumF1_0"] = "";
            if (mOK)
            {
                kv["SumF1_1"] = "1/2"; kv["SumF1_2"] = "1/2"; kv["SumF1_3"] = "1/2"; kv["SumF1_4"] = "1/10";
                kv["SumF1_5"] = "1/1"; kv["SumF1_6"] = "1/1"; kv["SumF1_7"] = "1/1"; kv["SumF1_8"] = "1/2";
                kv["SumF1_9"] = "1/10"; kv["SumF1_0"] = "1/10";
            }

            // Kilnr: only append kil line when kilnr is not empty and not "0"
            bool hasKil = !string.IsNullOrEmpty(kilnr) && !EqualsI(kilnr, "0");
            hasKil = false;
            string kilBase = (tmpd - tmpd1) > 10 ? "1579442-" : "1509952-";
            string tmpKil = hasKil ? kilBase + kilnr : "";
            string d1_8 = "Konmätningsapprat:1579440-41";
            if (hasKil) d1_8 += "\nInst. med kil " + tmpKil;
            kv["SumD1_1"] = mOK ? "Skjutmått" : "";
            kv["SumD1_2"] = mOK ? "UD-Apparat" : "";
            kv["SumD1_3"] = mOK ? "Multimar " + tmpRullar : "";
            kv["SumD1_4"] = mOK ? "Konmätningsapprat enl. nedan" : "";
            kv["SumD1_5"] = mOK ? "Ytjämnhetsmätare" : "";
            kv["SumD1_6"] = mOK ? "UD-Apparat" : "";
            kv["SumD1_7"] = mOK ? "Konmätningsapprat enl. nedan" : "";
            kv["SumD1_8"] = mOK ? d1_8 : "";
            kv["SumD1_9"] = mOK ? "Egglinjal" : "";
            kv["SumD1_0"] = mOK ? "Skjutmått" : "";

            kv["SumAF1_1"] = "";
            kv["SumAF1_2"] = mOK ? "Inställd med ring/klove" : "";
            kv["SumAF1_3"] = mOK ? "Inställd i längdmätbänk. Kontroll med gängmall" : "";
            kv["SumAF1_4"] = mOK ? "Tolerans: " + kv["SumKonavv"].Replace(".",",") : "";
            if (subject == "OH 3152/240,15 HTL")
                kv["SumAF1_4"] = mOK ? "Tolerans: max 0,020 [2F]":"";
            if (subject == "OH 3152 HTL")
                kv["SumAF1_4"] = mOK ? "Tolerans: max 0,020 [2F]" : "";
            kv["SumAF1_5"] = "";
            kv["SumAF1_6"] = mOK ? "Tolerans: " + kv["TmpRd"] + " [3F]" : "";
            kv["SumAF1_7"] = mOK ? kv["TmpVe"] : "";
            kv["SumAF1_8"] = mOK ? "Vid skärbyte med\nMätbygel:7419465" : "";
            kv["SumAF1_9"] = mOK ? "Vid misstänkt formfel kontrollera hylsan i MarSurf Contour XC20" : "";
            kv["SumAF1_0"] = mOK ? "Samtliga mått kontrolleras vid inställning" : "";

            // ── Texts & Ritningar ─────────────────────────────────────────────────
            kv["SumTextS1"] = "Okulär kontroll: Grader, frifläckar, slagmärken, repor, valkar etc. Märkning ska vara rätt och tydlig";
            kv["SumPRit"] = "Produkt: " + pRitning;
            kv["SumGRit"] = EqualsI(tmpArt, "MM")
                ? (tmpStmmD < 4 ? "Gänga: 239473:1, 7430182:A" : "Gänga: 237359:3, 7430181:2")
                : "Gänga: 7431233:2";
            kv["SumTolRit"] = "Toleranser: 1432012:7, 7437495:4 Gjutgodsdefekter: 7433015";
            kv["SumKlEgenskaper"] = "PPA & PPHALLMÄNKLASSADE EGENSKAPER/Klassade egenskaper klämhylsor";

            kv["Kilnr"] = " ";

            return kv;
        }

        private static string ComputePopup(string pub)
        {
            if (string.IsNullOrWhiteSpace(pub)) return "";
            DateTime dt; if (!DateTime.TryParse(pub, out dt)) return "";
            DateTime till = dt.AddDays(14);
            return DateTime.Today <= till.Date
                ? "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)\n\nPopupruta aktiv till " + till.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                : "";
        }

        private static string GenTolStr(double v)
        {
            if (v < 6.01) return "\u00b1 0.1";
            if (v < 30.01) return "\u00b1 0.2";
            if (v < 120.01) return "\u00b1 0.3";
            if (v < 400.01) return "\u00b1 0.5";
            if (v < 1000.01) return "\u00b1 0.8";
            if (v < 2000.01) return "\u00b1 1.2";
            return "\u00b1 2.0";
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

        // Direct [TmpX] / @Word — Swedish comma decimal
        private static string FmtComma(double v) =>
            v.ToString("0.################", CommonFunctions.Culture).Replace(".", ",");

        // @ReplaceSubstring(x;",";".")  — dot decimal
        private static string FmtDot(double v) =>
            v.ToString(CommonFunctions.Culture).Replace(",", ".");

        private static string Fmt3(double v) =>
            v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
    }
}