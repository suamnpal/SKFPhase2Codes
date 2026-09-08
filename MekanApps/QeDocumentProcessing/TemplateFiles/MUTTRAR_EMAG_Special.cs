using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class MUTTRAR_EMAG_Special : ITemplateCalculations
    {
        // ── Type lists ──────────────────────────────────────────────────────────
        private static readonly string[] TypHM30 = { "44", "48", "52", "56", "60" };
        private static readonly string[] TypHM31 = { "44", "48", "52", "56", "60" };
        private static readonly string[] TypHMt = { "41", "42", "43", "44", "45", "46", "48", "50", "52", "54", "56", "58", "60" };
        private static readonly string[] TypHML = { "41", "42", "43", "44", "45", "46", "47", "48", "50", "52", "54", "56", "58", "60" };
        private static readonly string[] TypKM = { "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "36", "38", "40" };
        private static readonly string[] TypKML = { "24", "26", "28", "30", "32", "34", "36", "38", "40" };

        // ── d3 (gängfas) lists ──────────────────────────────────────────────────
        private static readonly double[] d3_BM = { 222, 242, 262, 282, 302 };
        private static readonly double[] d3_HM = { 207, 212, 217, 222, 227, 232, 248, 252, 262, 272, 282, 292, 302 };
        private static readonly double[] d3_HML = { 207, 212, 217, 222, 227, 232, 237, 242, 252, 262, 272, 282, 292, 302 };
        private static readonly double[] d3_KM = { 120.4, 125.4, 130.4, 135.4, 140.4, 145.4, 150.4, 155.5, 160.5, 165.5, 170.5, 180.5, 190.5, 200.5 };
        private static readonly double[] d3_KML = { 120.4, 130.4, 140.4, 150.4, 160.5, 170.5, 180.5, 190.5, 200.5 };

        // ── D4 (utvFas) lists ───────────────────────────────────────────────────
        private static readonly double[] D4_HM30 = { 242, 270, 290, 310, 336 };
        private static readonly double[] D4_HM31 = { 250, 270, 300, 320, 340 };
        private static readonly double[] D4_HM = { 238, 238, 248, 250, 258, 260, 270, 290, 300, 310, 320, 330, 340 };
        private static readonly double[] D4_HML = { 232, 232, 242, 242, 252, 252, 262, 270, 280, 290, 300, 310, 320, 336 };
        private static readonly double[] D4_KM = { 138, 148, 149, 160, 160, 171, 171, 182, 182, 193, 193, 203, 214, 226 };
        private static readonly double[] D4_KML = { 135, 145, 155, 170, 180, 190, 200, 210, 222 };

        // ── D5 (ytterdiameter) lists ────────────────────────────────────────────
        private static readonly double[] D5_HM30 = { 260, 290, 310, 330, 360 };
        private static readonly double[] D5_HM31 = { 280, 300, 330, 350, 380 };
        private static readonly double[] D5_HM = { 260, 270, 270, 280, 280, 290, 300, 320, 330, 340, 350, 370, 380 };
        private static readonly double[] D5_HML = { 250, 250, 260, 260, 270, 270, 280, 290, 300, 310, 320, 330, 340, 360 };
        private static readonly double[] D5_KM = { 155, 160, 165, 175, 180, 190, 195, 200, 210, 210, 220, 230, 240, 250 };
        private static readonly double[] D5_KML = { 145, 155, 165, 180, 190, 200, 210, 220, 240 };

        // ── B (bredd) lists ─────────────────────────────────────────────────────
        private static readonly double[] B_HM30 = { 30, 34, 34, 38, 42 };
        private static readonly double[] B_HM31 = { 32, 34, 36, 38, 40 };
        private static readonly double[] B_HM = { 30, 30, 30, 32, 32, 34, 34, 36, 36, 38, 38, 40, 40 };
        private static readonly double[] B_HML = { 30, 30, 30, 30, 30, 30, 34, 34, 34, 34, 38, 38, 38, 42 };
        private static readonly double[] B_KM = { 20, 21, 21, 22, 22, 24, 24, 25, 25, 26, 26, 27, 28, 29 };
        private static readonly double[] B_KML = { 20, 21, 22, 24, 25, 26, 27, 28, 29 };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;
            List<Bookmark> bm = req != null ? req.Bookmarks : null;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");

            bool tmpT = tmpBet.IndexOf('T') >= 0;
            bool tmpSlash = tmpBet.IndexOf('/') >= 0;
            bool tmpVZ483 = tmpBet.IndexOf("VZ483", StringComparison.OrdinalIgnoreCase) >= 0;
            bool tmpUNC = tmpBet.IndexOf("UNC", StringComparison.OrdinalIgnoreCase) >= 0;
            bool tmpSpecial = tmpBet.Length > 10;

            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-', 'T', 'x', 'X' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : string.Empty;
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : string.Empty;
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : string.Empty;
            string tmpBet4 = tokens.Length > 3 ? tokens[3] : string.Empty;
            string tmpBet5 = tokens.Length > 4 ? tokens[4] : string.Empty;

            int cntB2 = tmpBet2.Length;
            string tmpSerie = cntB2 > 3 ? tmpBet2.Substring(0, 2) : "0";
            string tmpTypStr = cntB2 > 3 ? tmpBet2.Substring(tmpBet2.Length - 2)
                             : (tmpT ? (tmpBet2.Length >= 2 ? tmpBet2.Substring(0, 2) : tmpBet2)
                                        : tmpBet2);

            int serieInt = TryParseInt(tmpSerie);
            double typNum = TryParseDouble(tmpTypStr);
            double tmpBet4b = TryParseDouble(tmpBet4);
            double tmpBet5b = TryParseDouble(tmpBet5);

            bool isHM30 = EqualsI(tmpBet1, "HM") && serieInt == 30;
            bool isHM31 = EqualsI(tmpBet1, "HM") && serieInt == 31;
            bool isHM = EqualsI(tmpBet1, "HM") && serieInt == 0;
            bool isHML = EqualsI(tmpBet1, "HML");
            bool isKM = EqualsI(tmpBet1, "KM");
            bool isKML = EqualsI(tmpBet1, "KML");
            bool isKMorL = isKM || isKML;
            bool isBM = isHM30 || isHM31 || tmpVZ483;

            // Determine type list and index
            string[] typeList;
            if (isBM && !tmpVZ483) typeList = isHM30 ? TypHM30 : TypHM31;
            else if (isHM) typeList = tmpT ? TypHMt : TypHMt;
            else if (isHML) typeList = TypHML;
            else if (isKM) typeList = TypKM;
            else if (isKML) typeList = TypKML;
            else typeList = TypHMt;
            int tmpTL = GetMember(tmpTypStr, typeList);

            // TmpD and TmpP
            double tmpD = tmpUNC ? 25.4 * tmpBet4b : typNum / 2.0 * 10.0;
            double tmpP = tmpUNC ? 25.4 / tmpBet5b : CalcP(tmpD);

            kv["SumP"] = "(P) " + Fmt(tmpP).Replace(".",",");
            kv["SumGV"] = tmpP < 4 ? "60°" : "30°";

            // Ritningar
            string prdBase = isKMorL ? "7433440" : isHM30 ? "223015" : isHM31 ? "223016" : isHM ? "224673" : isHML ? "222755" : "";
            string tmpPrdRit = prdBase + ": senaste utgåva";
            string tmpGangRit = (tmpP < 3 ? "239473" : "237359") + ": senaste utgåva";
            string tmpGangTolRit = (tmpP < 3 ? "7430182" : "7430181") + ": senaste utgåva";
            kv["SumPrdRit"] = "Produkt: " + tmpPrdRit + " ";
            kv["SumGängRit"] = "Gänga: " + tmpGangRit + " ";
            kv["SumGängTolRit"] = "Gängtoleranser: " + tmpGangTolRit + " ";
            kv["SumTolRit"] = "Övriga toleranser: 1432008: senaste utgåva ";

            // Gängbeteckning
            string tmp1GBDia = Fmt(tmpD);
            string tmp2GBDia = tmpUNC ? "UNC " + tmpBet4
                             : (tmpP < 4 ? "M" + tmp1GBDia : "Tr" + tmp1GBDia);
            string tmpG = tmp2GBDia + "x" + (tmpUNC ? tmpBet5 : Fmt(tmpP));
            string tmpRullar = tmpUNC ? "UNC " + Fmt(tmpBet5b)
                             : (tmpP < 4 ? "M" + Fmt(tmpP) : "Tr" + Fmt(tmpP));
            kv["SumG"] = tmpG;

            // dm
            double dmBm = GetDouble(bm, "Medeldiameter");
            double tmpdm = dmBm != 0 ? dmBm : CalcDm(tmpD, tmpP);
            kv["Sumdm"] = "(dm) " + Fmt(tmpdm);
            kv["SumdmTol"] = DmTolPos(tmpUNC, isKMorL, isBM, tmpD, tmpP);
            if(!tmpUNC)
                kv["SumdmTol"] = kv["SumdmTol"] + "  [3F]";
            kv["SumdmTolN"] = tmpUNC ? "+ 0.150" : "- 0 [2F]";

            // D1
            double tmpD1 = CalcD1(tmpdm, tmpP);
            kv["SumD1"] = "(D1) " + Fmt(tmpD1);
            kv["SumD1Tol"] = D1TolPos(tmpUNC, isKMorL, isBM, tmpP);
            kv["SumD1TolN"] = tmpUNC ? "+ 0.170" : "- 0 [2F]";

            // d3 (gängfas)
            double d3Bm = GetDouble(bm, "Gängfas");
            double tmpd3;
            if (d3Bm != 0) tmpd3 = d3Bm;
            else
            {
                double[] d3list = isBM && !tmpVZ483 ? d3_BM
                               : isHM ? d3_HM
                               : isHML ? d3_HML
                               : isKM ? d3_KM
                               : isKML ? d3_KML : d3_HM;
                tmpd3 = GetVal(d3list, tmpTL);
            }
            kv["Sumd3"] = "(d3) " + Fmt(tmpd3);
            kv["Sumd3Tol"] = d3TolPos(isKMorL, tmpd3);
            kv["Sumd3TolN"] = "- 0";

            // D4 (utvFas)
            double d4Bm = GetDouble(bm, "UtvFas");
            double tmpD4;
            if (d4Bm != 0) tmpD4 = d4Bm;
            else
            {
                double[] d4list = isHM30 ? D4_HM30 : isHM31 ? D4_HM31 : isHM ? D4_HM
                               : isHML ? D4_HML : isKM ? D4_KM : isKML ? D4_KML : D4_HM;
                tmpD4 = GetVal(d4list, tmpTL);
            }
            kv["SumD4"] = "(D4) " + Fmt(tmpD4);
            kv["SumD4Tol"] = "+ 0 [3F]";
            kv["SumD4TolN"] = H13TolNGt(tmpD4);

            // D5 (ytterdiameter)
            double d5Bm = GetDouble(bm, "Ytterdiameter");
            double tmpD5;
            if (d5Bm != 0) tmpD5 = d5Bm;
            else
            {
                double[] d5list = isHM30 ? D5_HM30 : isHM31 ? D5_HM31 : isHM ? D5_HM
                               : isHML ? D5_HML : isKM ? D5_KM : isKML ? D5_KML : D5_HM;
                tmpD5 = GetVal(d5list, tmpTL);
            }
            kv["SumD5"] = "(D5) " + Fmt(tmpD5);
            kv["SumD5Tol"] = "+ 0 [3F]";
            kv["SumD5TolN"] = H13TolNGt(tmpD5);

            // B (bredd)
            double bBm = GetDouble(bm, "Bredd");
            double tmpBval;
            if (bBm != 0) tmpBval = bBm;
            else
            {
                double[] blist = isHM30 ? B_HM30 : isHM31 ? B_HM31 : isHM ? B_HM
                              : isHML ? B_HML : isKM ? B_KM : isKML ? B_KML : B_HM;
                tmpBval = GetVal(blist, tmpTL);
            }
            kv["SumB"] = "(B) " + Fmt(tmpBval);
            kv["SumBTol"] = "+ 0 [3F]";
            kv["SumBTolN"] = BTolN(tmpBval) + " [3F]";

            kv["SumV30"] = "30°";
            kv["SumV45"] = "45°";
            kv["SumR"] = SumR(tmpD1);

            kv["SumRd"] = isKMorL
                ? (tmpD5 < 185 ? "0.080" : "0.092")
                : SumRd(tmpD5);

            kv["SumKa"] = SumKa(tmpD);

            bool prdIs7433403 = tmpPrdRit.StartsWith("7433403");
            kv["SumRa"] = prdIs7433403 ? "2.5" : (tmpD1 > 110 ? "3.2" : "2.5");

            // Antal spår
            double antalSpar = GetDouble(bm, "AntalSpår");
            if (antalSpar == 0)
                kv["SumSnr"] = isBM ? "(8x) 45°" : "(4x) 90°";
            else
                kv["SumSnr"] = "(" + Fmt(antalSpar) + "x) " + Fmt(Math.Round(360.0 / antalSpar, 4)) + "°";

            // SumBM
            kv["SumBM"] = BuildSumBM(bm, isBM, tmpVZ483, isHM30, isHM31, tmpD5, tmpTL, isHM, isHML);

            // Haktag S (fräsbredd)
            double sBm = GetDouble(bm, "Fräsbredd");
            double tmpS = sBm != 0 ? sBm : CalcS(isHM30, isHM31, isHM, isHML, isKM, isKML, tmpD);
            kv["SumS"] = "(S) " + Fmt(tmpS);
            kv["SumSTol"] = STol(tmpS) + " [3F]";

            // Haktag t (fräsDjup)
            double tBm = GetDouble(bm, "FräsDjup");
            double tmpt = tBm != 0 ? tBm : CalcT(isHM30, isHM31, isHM, isHML, isKM, isKML, tmpD);
            kv["Sumt"] = "(t) " + Fmt(tmpt);
            kv["SumtTol"] = tmpt < 6 ? "+ 1.200" : "+ 1.500";
            kv["SumtTolN"] = "- 0 [3F]";

            kv["SumL"] = tmpS > 30 ? "1.50" : tmpS > 18 ? "1.25" : tmpS > 10 ? "1.00" : tmpS > 6 ? "0.75" : "0.50";

            bool isEMAG = EqualsI(maskinVal, "EMAG");
            kv["SumMaskinValS1"] = "Maskin: " + (isEMAG ? maskinVal : "");

            SumFrequencies(kv, isEMAG);
            SumMeasuringDevices(kv, isEMAG, tmpP, tmpRullar);
            SumAF(kv, isEMAG, tmpP, tmpUNC, isBM, tmpG);

            kv["SumText"] = "Övrigt: Grader, frifläckar, slagmärken, repor, valkar och andra defekter samt ojämnheter kontrolleras visuellt.<<LineBreak>>Övriga mått kontrolleras vid inställning med skjutmått.";
            kv["SumKlEgenskaper"] = "PRODUCTION NUTS & SLEEVES & HOUSINGSALLMÄNKLASSADE EGENSKAPERKlassade egenskaper muttrar";

            return kv;
        }

        // ── Popup ───────────────────────────────────────────────────────────────
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
                       " dagar)\n\nPopupruta aktiv till " +
                       validTill.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return "";
        }

        // ── Freq / Devices / AF ─────────────────────────────────────────────────
        private static void SumFrequencies(Dictionary<string, string> kv, bool emag)
        {
            kv["SumF1_1"] = emag ? "1/tim" : "";
            kv["SumF1_2"] = emag ? "1/10" : "";
            kv["SumF1_3"] = emag ? "1/tim" : "";
            kv["SumF1_4"] = emag ? "1/tim" : "";
            kv["SumF1_5"] = emag ? "1/tim" : "";
            kv["SumF1_6"] = emag ? "1/tim" : "";
            kv["SumF1_7"] = emag ? "Inst." : "";
            kv["SumF1_8"] = emag ? "Inst." : "";
            kv["SumF1_9"] = emag ? "1/tim" : "";
            kv["SumF1_0"] = emag ? "Inst." : "";
            kv["SumF1_11"] = emag ? "1/tim" : "";
            kv["SumF1_12"] = emag ? "Inst." : "";
        }

        private static void SumMeasuringDevices(Dictionary<string, string> kv, bool emag, double p, string rullar)
        {
            kv["SumD1_1"] = emag ? (p < 4 ? "Klump" : "Skjutmått") : "";
            kv["SumD1_2"] = emag ? "UD-Apparat med rullar: " + rullar : "";
            kv["SumD1_3"] = emag ? "Skjutmått" : "";
            kv["SumD1_4"] = emag ? "Skjutmått" : "";
            kv["SumD1_5"] = emag ? "Skjutmått" : "";
            kv["SumD1_6"] = emag ? "Skjutmått" : "";
            kv["SumD1_7"] = emag ? "Skjutmått" : "";
            kv["SumD1_8"] = emag ? "Skjutmått" : "";
            kv["SumD1_9"] = emag ? "min/max-Tolk" : "";
            kv["SumD1_0"] = emag ? "Egglinjal" : "";
            kv["SumD1_11"] = emag ? "Ytjämnhetsmätare" : "";
            kv["SumD1_12"] = emag ? "UD-apparat" : "";
        }

        private static void SumAF(Dictionary<string, string> kv, bool emag, double p, bool unc, bool bm, string g)
        {
            kv["SumAF1_1"] = emag && p < 4 ? "Klump märkt: " + g : "";
            kv["SumAF1_2"] = emag ? (p < 4 && !unc ? "Kontrolleras med gängtolk " + g + " 1/tim"
                                                     : "Kontrolleras med klove 1/tim") : "";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = "";
            kv["SumAF1_6"] = "";
            kv["SumAF1_7"] = "";
            kv["SumAF1_8"] = "";
            kv["SumAF1_9"] = emag && bm ? "Topsning 100% alla borrhål" : "";
            kv["SumAF1_0"] = "";
            kv["SumAF1_11"] = emag ? "Ytjämnhet övriga ytor Ra 6.3" : "";
            kv["SumAF1_12"] = "";
        }

        // ── SumBM ───────────────────────────────────────────────────────────────
        private static string BuildSumBM(List<Bookmark> bm, bool isBM, bool vz483, bool hm30, bool hm31,
            double d5, int tl, bool isHM, bool isHML)
        {
            if (!isBM) return "";

            double antalHal = GetDouble(bm, "AntalHål");
            string tmpUnr = antalHal == 0 ? "8 st. Borrhål, jämn delning, "
                                          : Fmt(antalHal) + " Borrhål, jämn delning, ";

            if (vz483)
            {
                string gTapp483 = GetBmOrDefault(bm, "Gängtapp", d5 < 369 ? "M8" : "M10");
                string gl483 = GetBmOrDefault(bm, "Gänglängd", d5 < 369 ? "17mm" : "21mm");
                string glPfx483 = EqualsI(gl483, d5 < 369 ? "17mm" : "21mm") ? "min: Gänglängd: " : ", min: Gänglängd: ";
                double[] ud483list = isHM ? new double[] { 41, 228, 43, 44, 45, 248, 48, 272, 52, 54, 56, 316, 60 }
                                   : isHML ? new double[] { 41, 42, 226, 44, 45, 46, 246, 48, 50, 52, 54, 56, 58, 60 }
                                   : new double[0];
                string hdBm = GetString(bm, "Håldiameter");
                string hd483 = EqualsI(hdBm, "0") || string.IsNullOrEmpty(hdBm)
                    ? (tl >= 1 && tl <= ud483list.Length ? Fmt(ud483list[tl - 1]) : "")
                    : hdBm;
                return tmpUnr + "Gängtapp: " + gTapp483 + " " + glPfx483 + gl483 + "mm, Håldiameter: " + hd483
                     + "\nPlacering borrhål: funktionskontroll med muttersäkring & kontroll 100%";
            }

            string gTapp = GetBmOrDefault(bm, "Gängtapp",
                hm30 ? (d5 < 265 ? "M6" : "M8") : (d5 < 305 ? "M8" : "M10"));
            string gLength = GetBmOrDefault(bm, "Gänglängd",
                hm30 ? (d5 < 265 ? "12mm" : "17mm") : (d5 < 305 ? "17mm" : "21mm"));
            string hd = GetHalDiameter(bm, hm30, hm31, d5);

            return tmpUnr + "Gängtapp: " + gTapp + ", min: Gänglängd: " + gLength + "mm, Håldiameter: " + hd
                 + "\nPlacering borrhål: funktionskontroll med muttersäkring & kontroll 100%";
        }

        private static string GetBmOrDefault(List<Bookmark> bm, string key, string def)
        {
            string v = GetString(bm, key);
            return string.IsNullOrEmpty(v) || EqualsI(v, "0") ? def : v;
        }

        private static string GetHalDiameter(List<Bookmark> bm, bool hm30, bool hm31, double d5)
        {
            string hdBm = GetString(bm, "Håldiameter");
            if (!string.IsNullOrEmpty(hdBm) && !EqualsI(hdBm, "0")) return hdBm;
            if (hm30)
            {
                if (d5 < 265) return "229"; if (d5 < 300) return "253"; if (d5 < 315) return "273";
                if (d5 < 340) return "293"; if (d5 < 370) return "316"; return "336";
            }
            if (hm31)
            {
                if (d5 < 290) return "238"; if (d5 < 310) return "258"; if (d5 < 340) return "281";
                if (d5 < 360) return "301"; if (d5 < 390) return "326"; return "346";
            }
            return "";
        }

        // ── Dimension helpers ───────────────────────────────────────────────────
        private static double CalcP(double D)
        {
            if (D < 10.1) return 0.75;
            if (D < 20.1) return 1.0;
            if (D < 50.1) return 1.5;
            if (D < 150.1) return 2.0;
            if (D < 200.1) return 3.0;
            if (D < 300.1) return 4.0;
            return 5.0;
        }

        private static double CalcDm(double D, double p)
        {
            if (Math.Abs(p - 0.75) < 0.001) return Math.Round(D - 0.487, 2);
            if (Math.Abs(p - 1) < 0.001) return Math.Round(D - 0.65, 2);
            if (Math.Abs(p - 1.5) < 0.001) return Math.Round(D - 0.974, 2);
            if (Math.Abs(p - 2) < 0.001) return Math.Round(D - 1.299, 2);
            if (Math.Abs(p - 3) < 0.001) return Math.Round(D - 1.949, 2);
            if (Math.Abs(p - 4) < 0.001) return Math.Round(D - 2.0, 2);
            return Math.Round(D - 2.5, 2);
        }

        private static double CalcD1(double dmv, double p)
        {
            if (Math.Abs(p - 3.175) < 0.001) return Math.Round(dmv - 1.375, 2);
            if (Math.Abs(p - 0.75) < 0.001) return Math.Round(dmv - 0.325, 2);
            if (Math.Abs(p - 1) < 0.001) return Math.Round(dmv - 0.433, 2);
            if (Math.Abs(p - 1.5) < 0.001) return Math.Round(dmv - 0.65, 2);
            if (Math.Abs(p - 2) < 0.001) return Math.Round(dmv - 0.866, 2);
            if (Math.Abs(p - 3) < 0.001) return Math.Round(dmv - 1.299, 2);
            if (Math.Abs(p - 4) < 0.001) return Math.Round(dmv - 2.0, 2);
            return Math.Round(dmv - 2.5, 2);
        }

        private static string DmTolPos(bool unc, bool kmL, bool bm, double D, double p)
        {
            if (unc) return "+ 0.307";
            if (kmL)
            {
                if (D > 180) return "+ 0.265";
                if (D > 90) return Math.Abs(p - 3) < 0.001 ? "+ 0.236" : "+ 0.200";
                if (D > 45) return Math.Abs(p - 2) < 0.001 ? "+ 0.190" : "+ 0.170";
                if (D > 22.4) return "+ 0.160";
                if (D > 11.2) return "+ 0.125";
                return "+ 0.106";
            }
            if (bm)
            {
                if (Math.Abs(p - 4) < 0.001) return "+ 0.475";
                if (Math.Abs(p - 5) < 0.001) return "+ 0.530";
                if (Math.Abs(p - 6) < 0.001) return "+ 0.600";
                if (Math.Abs(p - 7) < 0.001) return "+ 0.630";
                return "+ 0.710";
            }
            return "";
        }

        private static string D1TolPos(bool unc, bool kmL, bool bm, double p)
        {
            if (unc) return "+ 0.343";
            if (kmL)
            {
                if (Math.Abs(p - 3) < 0.001) return "+ 0.400";
                if (Math.Abs(p - 2) < 0.001) return "+ 0.300";
                if (Math.Abs(p - 1.5) < 0.001) return "+ 0.236";
                if (Math.Abs(p - 1) < 0.001) return "+ 0.190";
                return "+ 0.150";
            }
            if (bm)
            {
                if (Math.Abs(p - 4) < 0.001) return "+ 0.375";
                if (Math.Abs(p - 5) < 0.001) return "+ 0.450";
                if (Math.Abs(p - 6) < 0.001) return "+ 0.500";
                if (Math.Abs(p - 7) < 0.001) return "+ 0.560";
                return "+ 0.630";
            }
            return "";
        }

        private static string d3TolPos(bool kmL, double d3)
        {
            if (kmL) // H13
            {
                if (d3 < 3.01) return "+ 0.140"; if (d3 < 6.01) return "+ 0.180";
                if (d3 < 10.01) return "+ 0.220"; if (d3 < 18.01) return "+ 0.270";
                if (d3 < 30.01) return "+ 0.330"; if (d3 < 50.01) return "+ 0.390";
                if (d3 < 80.01) return "+ 0.460"; if (d3 < 120.01) return "+ 0.540";
                if (d3 < 180.01) return "+ 0.630"; if (d3 < 250.01) return "+ 0.720";
                if (d3 < 315.01) return "+ 0.810"; if (d3 < 400.01) return "+ 0.890";
                if (d3 < 500.01) return "+ 0.970"; if (d3 < 630.01) return "+ 1.100";
                if (d3 < 800.01) return "+ 1.250"; if (d3 < 1000.01) return "+ 1.400";
                if (d3 < 1250.01) return "+ 1.650"; if (d3 < 1600.01) return "+ 1.950";
                if (d3 < 2000.01) return "+ 2.300"; if (d3 < 2500.01) return "+ 2.800";
                return "+ 3.300";
            }
            // H14
            if (d3 < 3.01) return "+ 0.250"; if (d3 < 6.01) return "+ 0.300";
            if (d3 < 10.01) return "+ 0.360"; if (d3 < 18.01) return "+ 0.430";
            if (d3 < 30.01) return "+ 0.520"; if (d3 < 50.01) return "+ 0.620";
            if (d3 < 80.01) return "+ 0.740"; if (d3 < 120.01) return "+ 0.870";
            if (d3 < 180.01) return "+ 1.000"; if (d3 < 250.01) return "+ 1.150";
            if (d3 < 315.01) return "+ 1.300"; if (d3 < 400.01) return "+ 1.400";
            if (d3 < 500.01) return "+ 1.550"; if (d3 < 630.01) return "+ 1.750";
            if (d3 < 800.01) return "+ 2.000"; if (d3 < 1000.01) return "+ 2.300";
            if (d3 < 1250.01) return "+ 2.600"; if (d3 < 1600.01) return "+ 3.100";
            if (d3 < 2000.01) return "+ 3.700"; if (d3 < 2500.01) return "+ 4.400";
            return "+ 5.400";
        }

        // D4/D5 TolN: uses > (not >=), note reversed direction vs most tables
        private static string H13TolNGt(double v)
        {
            if (v > 1000) return "- 1.650"; if (v > 800) return "- 1.400";
            if (v > 630) return "- 1.250"; if (v > 500) return "- 1.100";
            if (v > 400) return "- 0.970"; if (v > 315) return "- 0.890";
            if (v > 250) return "- 0.810"; if (v > 180) return "- 0.720";
            if (v > 120) return "- 0.630"; if (v > 80) return "- 0.540";
            if (v > 50) return "- 0.460"; if (v > 30) return "- 0.390";
            if (v > 18) return "- 0.330";
            return "- 0.270";
        }

        private static string BTolN(double b)
        {
            if (b > 80) return "- 0.540"; if (b > 50) return "- 0.460";
            if (b > 30) return "- 0.390"; if (b > 18) return "- 0.330";
            if (b > 10) return "- 0.270"; if (b > 6) return "- 0.220";
            return "- 0.180";
        }

        private static string SumR(double d1)
        {
            if (d1 > 295) return "R 3.5"; if (d1 > 235) return "R 3.0";
            if (d1 > 180) return "R 2.5"; if (d1 > 125) return "R 2.0";
            if (d1 > 60) return "R 1.5";
            return "R 1.0";
        }

        private static string SumRd(double d5)
        {
            if (d5 < 19) return "0.035"; if (d5 < 31) return "0.042";
            if (d5 < 51) return "0.050"; if (d5 < 81) return "0.060";
            if (d5 < 121) return "0.070";
            return "0.080";
        }

        private static string SumKa(double D)
        {
            if (D < 51) return "0.040"; if (D < 121) return "0.050";
            if (D < 251) return "0.060"; if (D < 316) return "0.070";
            if (D < 401) return "0.080"; if (D < 501) return "0.090";
            if (D < 631) return "0.100"; if (D < 801) return "0.120";
            if (D < 1001) return "0.140";
            return "0.160";
        }

        private static double CalcS(bool hm30, bool hm31, bool hm, bool hml, bool km, bool kml, double D)
        {
            if (hm30) return D < 261 ? 20 : 24;
            if (hm31) return D < 241 ? 20 : 24;
            if (hm) return D < 251 ? 20 : 24;
            if (hml) return D < 271 ? 20 : 24;
            if (km)
            {
                if (D < 131) return 12; if (D < 151) return 14;
                if (D < 171) return 16; return 18;
            }
            if (kml)
            {
                if (D < 141) return 12; if (D < 161) return 14;
                if (D < 191) return 16; return 18;
            }
            return 0;
        }

        private static string STol(double s)
        {
            if (s < 4) return "± 0.125"; if (s < 7) return "± 0.150";
            if (s < 11) return "± 0.180"; if (s < 19) return "± 0.215";
            if (s < 31) return "± 0.260"; if (s < 51) return "± 0.310";
            return "± 0.370";
        }

        private static double CalcT(bool hm30, bool hm31, bool hm, bool hml, bool km, bool kml, double D)
        {
            if (hm30)
            {
                if (D < 221) return 9; if (D < 281) return 10; return 12;
            }
            if (hm31) return D < 241 ? 10 : 12;
            if (hm) return D < 251 ? 10 : 12;
            if (hml)
            {
                if (D < 211) return 8; if (D < 236) return 9;
                if (D < 291) return 10; return 12;
            }
            if (km)
            {
                if (D < 131) return 5; if (D < 151) return 6;
                if (D < 171) return 7; return 8;
            }
            if (kml) return D < 191 ? 5 : 6;
            return 0;
        }

        // ── Utility ─────────────────────────────────────────────────────────────
        private static double GetVal(double[] list, int oneBasedIdx)
        {
            if (oneBasedIdx < 1 || oneBasedIdx > list.Length) return 0;
            return list[oneBasedIdx - 1];
        }

        private static int GetMember(string val, string[] list)
        {
            for (int i = 0; i < list.Length; i++)
                if (string.Equals(list[i], val, StringComparison.OrdinalIgnoreCase)) return i + 1;
            return 0;
        }

        private static bool EqualsI(string a, string b)
            => string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);

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
    }
}