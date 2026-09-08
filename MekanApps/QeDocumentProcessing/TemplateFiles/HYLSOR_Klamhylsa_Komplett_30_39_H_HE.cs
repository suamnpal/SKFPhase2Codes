using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Klamhylsa_Komplett_30_39_H_HE : ITemplateCalculations
    {
        // ── Machine groups ────────────────────────────────────────────────────────
        private static readonly string[] MachinesOP1 = { "MacTurn 550", "VTR-160" };
        private static readonly string[] MachinesOP3 = { "Skepp6", "K&T", "VTR-160", "MacTurn 550", "Dubbelparet" };
        private static readonly string[] MachinesOP4 = { "Skepp6", "K&T", "VTR-160", "MacTurn 550" };

        // ── Type lists (1-based @Member) ──────────────────────────────────────────
        private static readonly string[] Typ30 = { "32", "34", "36", "38", "40", "44", "48", "52", "56", "60", "64", "68", "72", "76", "80", "84", "88", "92", "96", "500", "530", "560", "600", "630", "670", "710", "750", "800", "850", "900", "950", "378", "420", "460" };
        private static readonly string[] Typ31 = { "32", "34", "36", "38", "40", "44", "48", "52", "56", "60", "64", "68", "72", "76", "80", "84", "88", "92", "96", "500", "530", "560", "600", "630", "670", "710", "750", "800", "850", "900", "1000", "420", "355,6" };
        private static readonly string[] Typ23 = { "32", "34", "36", "38", "40", "44", "48", "52", "56" };
        private static readonly string[] Typ32 = { "60", "64", "68", "72", "76", "80", "84", "88", "92", "96", "500", "530", "560", "600", "630", "670", "710", "750", "800", "850" };
        private static readonly string[] Typ39 = { "32", "34", "36", "38", "40", "44", "48", "52", "56", "60", "64", "68", "72", "76", "80", "84", "88", "92", "96", "500", "530", "560", "600", "630", "670", "710", "750", "800", "850", "900", "457,2", "1060" };
        private static readonly string[] TypL1 = { "64", "68", "72", "76", "80", "84", "88", "92", "96", "500", "530", "560", "600", "630", "670", "710", "750", "800", "850", "900", "950", "1000", "1060" };

        // ── B tables (index 0-based, 1-based via TmpTypLista-1) ───────────────────
        private static readonly double[] BTab30 = { 4.2, 4.2, 4.2, 4.2, 4.2, 3.9, 3.9, 3.9, 3.9, 3.9, 3.5, 3.5, 3.5, 3.5, 3.5, 3.5, 6.5, 6.5, 6.5, 6.5, 6, 6, 8, 6, 8, 8, 8, 10, 10, 10, 10, 4, 3.5, 3.5 };
        private static readonly double[] BTab31 = { 4.2, 4.2, 4.2, 4.2, 4.2, 3.9, 3.9, 3.9, 3.9, 3.9, 3.5, 3.5, 3.5, 3.5, 3.5, 3.5, 6.5, 6.5, 6.5, 6.5, 6, 6, 8, 6, 8, 8, 8, 10, 10, 10, 10, 3.5, 4.5 };
        private static readonly double[] BTab23 = { 4.2, 4.2, 4.2, 4.2, 4.2, 3.9, 3.9, 3.9, 3.9 };
        private static readonly double[] BTab32 = { 3.9, 3.5, 3.5, 3.5, 3.5, 3.5, 3.5, 6.5, 6.5, 6.5, 6, 6, 6, 8, 6, 8, 8, 8, 10, 10 };
        private static readonly double[] BTab39 = { 4.2, 4.2, 4.2, 4.2, 4.2, 3.9, 3.9, 3.9, 3.9, 3.9, 3.5, 3.5, 3.5, 3.5, 3.5, 3.5, 6.5, 6.5, 6.5, 6.5, 6, 6, 8, 6, 8, 8, 8, 10, 10, 10, 4, 12 };

        // ── E tables (oljeborrhål E) ───────────────────────────────────────────────
        private static readonly double[] ETab30 = { 57, 61, 66, 68, 72, 74, 79, 84, 89, 98, 100, 109, 109, 113, 122, 123, 135, 138, 139, 147, 155.5, 169, 171, 176.5, 189.5, 202, 208.5, 212, 218.5, 232, 240, 122, 135, 139 };
        private static readonly double[] ETab31 = { 69, 71, 75, 81, 85, 92, 98, 107, 110, 115, 124, 144, 148, 151, 156, 175, 176, 187, 191, 203, 206.5, 217, 226, 243, 263, 267.5, 281.5, 287, 304, 316.5, 339, 176, 151 };
        private static readonly double[] ETab23 = { 82, 85, 89, 93, 97, 104, 110, 117, 123 };
        private static readonly double[] ETab32 = { 130, 139, 160, 166, 172, 181, 196, 200, 212, 219, 235, 244, 255.5, 265.5, 286.5, 309, 314.5, 332, 337.5, 356 };
        private static readonly double[] ETab39 = { 51, 52, 56, 57, 62, 60, 64, 71, 75, 86, 86, 89, 89, 99, 103, 103, 117, 117, 122, 130, 134, 143, 147.5, 154.5, 161.5, 176, 178.5, 183, 185, 197.5, 122, 217.5 };

        // ── J tables (oljeborrhål J) ───────────────────────────────────────────────
        private static readonly double[] JTab30 = { 54.5, 58.5, 63, 64.5, 68.5, 70.5, 75.5, 81, 85.5, 95, 96.5, 105, 105.5, 109, 118.5, 119.5, 130.5, 133.5, 134.5, 143, 151.5, 163, 165, 170.5, 183.5, 196, 202.5, 206, 212.5, 226, 235, 118.5, 130.5, 134.5 };
        private static readonly double[] JTab31 = { 66, 68, 72.5, 77.5, 82, 89, 94.5, 104, 106.5, 112, 121, 140.5, 144.5, 147.5, 152, 171, 171.5, 183, 186.5, 199, 202.5, 211, 220, 237, 257, 261.5, 276.5, 281, 298, 310.5, 333, 171.5, 147.5 };
        private static readonly double[] JTab23 = { 79, 82.5, 86, 90, 93.5, 100.5, 107, 113.5, 120 };
        private static readonly double[] JTab32 = { 126.5, 135.5, 156, 162.5, 168, 177, 192.5, 196, 208, 214.5, 231, 240, 249.5, 259.5, 280.5, 303, 308.5, 326, 331.5, 350 };
        private static readonly double[] JTab39 = { 48, 49, 53, 54, 58.5, 57, 61, 68, 72, 82.5, 82.5, 85.5, 85.5, 95.5, 99.5, 99.5, 113, 113, 117.5, 125.5, 129, 138, 142.5, 149.5, 156.5, 171, 173.5, 178, 180, 192.5, 117.5, 212.5 };

        // ── F list (låsspårslängd, TypLista1-indexed) ─────────────────────────────
        // default (series 30/31/32/39): {25:26:26:26:27:27:27:28:28:28:34:34:35:35:36:38:38:39:39:40:42:44:44}
        private static readonly double[] FListDefault = { 25, 26, 26, 26, 27, 27, 27, 28, 28, 28, 34, 34, 35, 35, 36, 38, 38, 39, 39, 40, 42, 44, 44 };
        // series 240: {0:34:0:0:0:0:0:0:0:0:0:0:50:50:0:53:0:0:0:0:60:0:0}
        private static readonly double[] FList240 = { 0, 34, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 50, 0, 53, 0, 0, 0, 0, 60, 0, 0 };
        // series 241: {0:35:36:0:0:36:0:0:0:42:34:0:50:50:52:53:0:0:0:0:0:0:0}
        private static readonly double[] FList241 = { 0, 35, 36, 0, 0, 36, 0, 0, 0, 42, 34, 0, 50, 50, 52, 53, 0, 0, 0, 0, 0, 0, 0 };

        // ── E list (låsspårsbredd, TypLista1-indexed) ─────────────────────────────
        // series 30/39: {24:24:28:28:28:32:32:32:36:36:40:40:40:45:45:50:55:55:60:60:60:60:60}
        private static readonly double[] EList30_39 = { 24, 24, 28, 28, 28, 32, 32, 32, 36, 36, 40, 40, 40, 45, 45, 50, 55, 55, 60, 60, 60, 60, 60 };
        // series 31/32/241: {24:28:28:32:32:32:36:36:36:40:40:45:45:50:50:55:60:60:70:70:70:70:70}
        private static readonly double[] EList31_32_241 = { 24, 28, 28, 32, 32, 32, 36, 36, 36, 40, 40, 45, 45, 50, 50, 55, 60, 60, 70, 70, 70, 70, 70 };
        // series 240: {24:24:28:32:32:32:36:36:36:40:40:45:45:50:50:55:60:60:70:70:60:70:70}
        private static readonly double[] EList240 = { 24, 24, 28, 32, 32, 32, 36, 36, 36, 40, 40, 45, 45, 50, 50, 55, 60, 60, 70, 70, 60, 70, 70 };

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
            bool tmpLW = tmpBet.IndexOf("LW", StringComparison.OrdinalIgnoreCase) >= 0;

            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : "";
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : "";
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : "";
            string tmpBet4 = tokens.Length > 3 ? tokens[3] : "";
            string tmpBet5 = tokens.Length > 4 ? tokens[4] : "";

            int cntB2 = tmpBet2.Length, cntB3 = tmpBet3.Length;

            // TmpSerie
            int serieInt;
            string tmpSerie;
            if (tmpLW) { tmpSerie = "0"; serieInt = 0; }
            else if (tmpSlash && (cntB2 == 3 || cntB2 == 2)) { tmpSerie = tmpBet2; serieInt = TryParseInt(tmpSerie); }
            else if (cntB2 > 4) { tmpSerie = tmpBet2.Substring(0, 3); serieInt = TryParseInt(tmpSerie); }
            else if (cntB2 == 3) { tmpSerie = tmpBet2.Substring(0, 1); serieInt = TryParseInt(tmpSerie); }
            else { tmpSerie = tmpBet2.Length >= 2 ? tmpBet2.Substring(0, 2) : tmpBet2; serieInt = TryParseInt(tmpSerie); }

            // TmpTyp
            string tmpTyp;
            if (!tmpSlash) tmpTyp = tmpBet2.Length >= 2 ? tmpBet2.Substring(tmpBet2.Length - 2) : tmpBet2;
            else tmpTyp = cntB3 > 4 ? (tmpBet2.Length >= 2 ? tmpBet2.Substring(tmpBet2.Length - 2) : tmpBet2) : tmpBet3;
            double typNum = TryParseDouble(tmpTyp.Replace(",", "."));

            // Tmpd
            double tmpd_bm = GetDouble(bm, "Avvikande YDia Gänga (d)");
            double tmpd;
            if (tmpd_bm != 0) tmpd = tmpd_bm;
            else if (!tmpSlash || cntB3 > 4) tmpd = (typNum / 2.0) * 10.0;
            else tmpd = TryParseDouble(tmpBet3.Replace(",", "."));

            double tmpd1 = GetDouble(bm, "Innerdiameter (d1)");
            double tmpb = GetDouble(bm, "Gänglängd (b)");
            double tmpL = GetDouble(bm, "Längd (L)");
            double amatt = GetDouble(bm, "a-mått");

            int stmm = Stmm(tmpd);
            double tmpdm = DM(tmpd, stmm);
            double tmpd3 = D3(tmpdm, stmm);

            // TmpKona
            double konaBm = GetDouble(bm, "Kona");
            double tmpKona = konaBm == 0
                ? ((serieInt == 30 || serieInt == 31 || serieInt == 32 || serieInt == 39) ? 12 : 30)
                : konaBm;

            // GTj tolerances
            double gTjTol = GTjTolPos(tmpd, tmpKona);
            double gTjTolN = GTjTolNeg(tmpd, tmpKona);
            double tolSkillnad = gTjTolN - gTjTol;

            // TmpML, TmpVML
            double tmpML = tmpL - tmpb - 4.0;
            double tmpVML = tmpML < 110 ? 75 : 100;

            // TmpTypLista (1-based)
            string[] serieTypList = GetSerieTypList(serieInt);
            int typLista = GetMember(tmpTyp, serieTypList);    // for pages 3
            int typLista1 = GetMember(tmpTyp, TypL1);           // for page 4

            // ── PAGE 1 & 2: MacTurn 550 / VTR-160 ────────────────────────────────
            bool mOP1 = IsInGroup(maskinVal, MachinesOP1);
            string s1 = EqualsI(maskinVal, "MacTurn 550") ? "MacTurn 550" : EqualsI(maskinVal, "VTR-160") ? "VTR-160" : "";
            kv["SumMaskinValS1"] = "Maskin: " + s1 + " - OP1";
            kv["SumMaskinValS2"] = "Maskin: " + s1 + " - OP2";

            kv["SumGänga"] = "Tr" + Fmt(tmpd) + "x" + stmm.ToString(CultureInfo.InvariantCulture);
            kv["SumP"] = "Tr" + stmm.ToString(CultureInfo.InvariantCulture);
            kv["SumP1"] = "(P) " + stmm.ToString(CultureInfo.InvariantCulture);
            kv["SumRullar"] = stmm.ToString(CultureInfo.InvariantCulture) + "mm";
            kv["SumV"] = tmpKona == 30 ? "(V) 0º57" : "(V) 2º23";
            kv["SumKonaOP1"] = "Kona 1:" + Fmt(tmpKona);
            kv["SumKona"] = kv["SumKonaOP1"];

            // L
            kv["SumLOP1"] = "(L) " + Fmt(tmpL);
            kv["SumLOP1Tol"] = Fmt3(0.25);
            kv["SumL"] = "(L) " + Fmt(tmpL);
            kv["SumLTol"] = "+ " + Fmt3(0.0) + " [3F]";
            kv["SumLTolN"] = "- " + Fmt3(H15TolN(tmpL)) + " [3F]";

            // b (gänglängd)
            kv["Sumb"] = "(b) " +  (int)tmpb;
            kv["SumbTol"] = "+ " + Fmt3(BTol(tmpb)) + "  [3F]";
            kv["SumbTolN"] = "- " + Fmt3(0.0) + " [2F]";

            // SL
            double tmpSL = tmpb + 4.0;
            kv["SumSL"] = "(SL) " + Fmt(tmpSL);
            kv["SumSLTol"] = "± " + Fmt3(0.3);

            // d1 (JS9)
            kv["Sumd1"] = "(d1) " + Fmt(tmpd1);
            kv["Sumd1Tol"] = "± " + Fmt3(JS9Tol(tmpd1)) + " [3F]";

            // d (ytterdiameter gänga)
            double dOP1TolN = DOP1TolN(stmm);
            string sumDOP1Tol = "+ " + Fmt3(0.0);
            string sumDOP1TolN = "- " + Fmt3(dOP1TolN);
            kv["SumdOP1"] = "(d) " + Fmt(tmpd);
            kv["SumdOP1Tol"] = sumDOP1Tol;
            kv["SumdOP1TolN"] = sumDOP1TolN;
            kv["SumdaOP1"] = kv["SumdOP1"];
            kv["SumdaOP1Tol"] = sumDOP1Tol;
            kv["SumdaOP1TolN"] = sumDOP1TolN;
            kv["Sumd"] = "(d) " + Fmt(tmpd);
            kv["SumdTol"] = sumDOP1Tol;
            kv["SumdTolN"] = sumDOP1TolN;

            // dm (medelgängdiameter)
            kv["Sumdm"] = "(dm) " + Fmt(tmpdm).Replace(",", ".");
            kv["SumdmTol"] = "- " + Fmt3(DmTolPos(stmm)) + " [3F]";
            kv["SumdmTolN"] = "- " + Fmt3(DmTolNeg(stmm)) + " [3F]";

            // d3 (kärndiameter)
            kv["Sumd3"] = "(d3) " + Fmt(tmpd3).Replace(",", ".");
            kv["Sumd3Tol"] = "+ " + Fmt3(0.0);
            kv["Sumd3TolN"] = "- " + Fmt3(D3TolN(stmm));

            // Fas & radier
            string prodRit = GetString(bm, "Produktritning");
            kv["Sumg"] = "(g) " + Fas(tmpd, prodRit);
            string r3Raw = GetString(bm, "Radie Lillände");
            double r3Val = (string.IsNullOrEmpty(r3Raw) || EqualsI(r3Raw, "0"))
                ? (tmpd < 421 ? 1.0 : 2.5) : TryParseDouble(r3Raw.Replace(",", "."));
            kv["Sumr1"] = "R" + Fmt(r3Val).Replace(",", ".");
            string r2Raw = GetString(bm, "Radie Storände");
            double r2Val = (string.IsNullOrEmpty(r2Raw) || EqualsI(r2Raw, "0"))
                ? (tmpd < 320 ? 2.5 : tmpd < 530 ? 3.5 : tmpd < 710 ? 5.5 : 7.5)
                : TryParseDouble(r2Raw.Replace(",", "."));
            kv["Sumr"] = "R" + Fmt(r2Val).Replace(",", ".");

            // GTj
            kv["SumGTjTol"] = "+ " + Fmt3(gTjTol);
            kv["SumGTjaTol"] = kv["SumGTjTol"];
            kv["SumGTjTolN"] = "- " + Fmt3(gTjTolN);
            kv["SumGTjaTolN"] = kv["SumGTjTolN"];

            // d2 (kona storände)
            double d2bm = GetDouble(bm, "Kona storände diameter (d2)");
            double tmpd2a = d2bm != 0 ? d2bm
                : Math.Round(((tmpL - 1 - amatt) / tmpKona) + tmpd - tolSkillnad, 2);
            kv["Sumd2"] = "(d2) " + Fmt(tmpd2a).Replace(",", ".");
            kv["Sumd2Tol"] = "± " + Fmt3(GenTolVal(tmpd2a));

            // GVar
            kv["SumGVarTol"] = Fmt3(GVar(tmpd)).Replace(",", ".") + " [2F]";

            // Rakhet
            kv["SumRakA"] = "Max: " + Fmt3(RakA(tmpd)).Replace(",", ".");
            kv["SumRakB"] = "Max: " + Fmt3(RakB(tmpd)).Replace(",", ".");

            // Orundhet (JS9 on d1)
            kv["SumOrund"] = Fmt3(JS9Tol(tmpd1)).Replace(",", ".") + " [3F]";

            // Mätlängd
            kv["SumML"] = "Mätlängd=" + (tmpML < 110 ? "75" : "100");

            // Ra
            kv["SumRa"] = "2.5 [2F]";
            kv["SumRa1"] = "2.5 [2F]";
            kv["SumRa5"] = "5 [2F]";

            // Vinkeltolerans
            double vinkRaw = VinkAngle(tmpd) * tmpVML / 1000.0;
            kv["SumVinkTol"] = Fmt3(vinkRaw).Replace(",", ".") + " [2F]";

            // Bygel gauge
            double tmp8 = Math.Round(((tmpd - tmpd1) / 2.0) + ((tmpL - 1 - amatt - 8) / (2.0 * tmpKona)), 3);
            double tmp83 = Math.Round(((tmpd - tmpd1) / 2.0) + ((tmpL - 1 - amatt - 83) / (2.0 * tmpKona)), 3);
            double tmp108 = Math.Round(((tmpd - tmpd1) / 2.0) + ((tmpL - 1 - amatt - 108) / (2.0 * tmpKona)), 3);
            double tmp40 = Math.Round(((tmpd - tmpd1) / 2.0) + ((tmpL - 1 - amatt - 40) / (2.0 * tmpKona)), 3);
            double tmp140 = Math.Round(((tmpd - tmpd1) / 2.0) + ((tmpL - 1 - amatt - 140) / (2.0 * tmpKona)), 3);
            double sumL2 = tmpML < 145 ? 8 : 40;
            double sumL1 = tmpML < 110 ? 83 : tmpML < 145 ? 108 : 140;
            double sumE1 = tmpML < 110 ? tmp83 : tmpML < 145 ? tmp108 : tmp140;
            double sumE2 = tmpML < 145 ? tmp8 : tmp40;
            kv["SumL2"] = Fmt(sumL2);
            kv["SumL1"] = Fmt(sumL1);
            kv["SumE1"] = Fmt3(sumE1).Replace(".",",");
            if(serieInt ==30 && tokens[2]=="530" )
            kv["SumE2"] = "21,5";
            else
                kv["SumE2"] = Fmt3(sumE2).Replace(".", ",");

            if (subject == "OH 39/1060 H")
            {
                kv["SumE1"] = "35,458";
                kv["SumE2"] = "39,625"; 
            } else if (subject == "OH 3964 H")
            {
                kv["SumE1"] = "10,583";
                kv["SumE2"] = "13,708";
            }
            else if (subject == "OH 3968 H")
            {
                kv["SumE1"] = "10,625";
                kv["SumE2"] = "13,75";
            }
            else if (subject == "OH 3972 H")
            {
                kv["SumE1"] = "10,625";
                kv["SumE2"] = "13,75";
            }

            string bygel = tmpML < 145 ? "SR 7419471" : "SR 7415991";
            kv["SumBygGtj"] = bygel;
            kv["SumBygGVar"] = bygel;
            kv["SumBygVinkTol"] = bygel;

            // Freq / Devices / AF pages 1-2 (MacTurn/VTR)
            SumFreqOP1(kv, mOP1);
            SumDevicesOP1(kv, mOP1, stmm, bygel);
            SumAFOP1(kv, mOP1, kv["SumGVarTol"], kv["SumVinkTol"], kv["SumML"], kv["SumOrund"], kv["SumRakA"], kv["SumRakB"]);

            if (subject == "OH 39/1060 H")
            {
                kv["SumAF4_5"] = "Tol: 0.009 [2F] Mätlängd=100";
                kv["SumAF4_7"] = "Max: 0.02";
                kv["SumAF4_8"] = "Max: 0.03";
            }
            else if (subject == "OH 3964 H")
            {
                kv["SumAF4_5"] = "Tol: 0.01125 [2F] Mätlängd=75";
                kv["SumAF4_7"] = "Max: 0.012";
                kv["SumAF4_8"] = "Max: 0.018";
            }
            else if (subject == "OH 3968 H")
            {
                kv["SumAF4_5"] = "Tol: 0.01125 [2F] Mätlängd=75";
                kv["SumAF4_7"] = "Max: 0.012";
                kv["SumAF4_8"] = "Max: 0.018";
            }
            else if (subject == "OH 3972 H")
            {
                kv["SumAF4_5"] = "Tol: 0.01125 [2F] Mätlängd=75";
                kv["SumAF4_7"] = "Max: 0.012";
                kv["SumAF4_8"] = "Max: 0.018";
            }

            // Text pages 1-2
            kv["SumTextS1"] = "Kontrollera rätt märkning<<LineBreak>>Okulärkontroll gjuteridefekter, grader & slagmärken<<LineBreak>>Gjutgodsdefekter: 7433015";
            kv["SumTextS2"] = "Bryt alla kanter, avlägsna. Okulärkontroll märkning, gjuteridefekter, grader & slagmärken";

            // Ritningar pages 1-2
            string prodRitBm = GetString(bm, "Produktritning");
            string tmpRitningsnr = (!string.IsNullOrEmpty(prodRitBm) && !EqualsI(prodRitBm, "0"))
                ? prodRitBm
                : RitningsnrPage12(serieInt, tmpBet);
            tmpRitningsnr += ":senaste utg.";
            kv["SumRitningsnrS1"] = tmpRitningsnr;
            kv["SumRitningsnrS2"] = tmpRitningsnr;
            kv["SumRitTolS1"] = "Toleranser: 1432012:7";
            kv["SumRitTolS2"] = "Toleranser: 1432012:7";
            kv["SumRitGänga"] = "Gänga: 237359:2, 7430181:2";

            kv["SumKlEgenskaperS1"] = "PRODUCTION NUTS & SLEEVES & HOUSINGS\\ALLMÄNKLASSADE EGENSKAPER\\Klassade egenskaper klämhylsor";
            kv["SumKlEgenskaperS2"] = kv["SumKlEgenskaperS1"];

            // ── PAGE 3: BorrOljehål & Oljespår ────────────────────────────────────
            bool mOP3 = IsInGroup(maskinVal, MachinesOP3);
            string mv3 = EqualsI(maskinVal, "Skepp6") ? "Skepp6" : EqualsI(maskinVal, "K&T") ? "K&T" :
                         EqualsI(maskinVal, "VTR-160") ? "VTR-160" : EqualsI(maskinVal, "MacTurn 550") ? "MacTurn 550" :
                         EqualsI(maskinVal, "Dubbelparet") ? "Dubbelparet" : "";
            kv["SumMaskinValS3"] = "Maskin: " + mv3 + " - BorrOljehål & Oljespår";

            // Ritning page 3
            kv["SumRitNr"] = RitNrPage3(serieInt);

            // G1 (gänga)
            kv["SumG1"] = G1(typNum);

            // B1 (oljespårsbredd B)
            double[] bTab = GetBTab(serieInt);
            double b1val = (bTab != null && typLista >= 1 && typLista <= bTab.Length) ? bTab[typLista - 1] : 0;
            kv["SumB1"] = "(B) " + Fmt(b1val).Replace(",", ".");
            kv["SumB1Tol"] = Math.Abs(b1val - 4) < 0.001 ? "- 0.1" : "   0";
            kv["SumB1TolN"] = Math.Abs(b1val - 4) < 0.001 ? "- 0.3" : "- 0.1";

            // C1 (gänghålslängd)
            double c1 = C1(typNum);
            kv["SumC1"] = "(C) " + Fmt(c1);
            kv["SumC1Tol"] = c1 < 6.1 ? "± 0.1" : c1 < 30.1 ? "± 0.2" : "± 0.3";

            // T (gänghålsbredd)
            double tVal = TVal(typNum);
            kv["SumT"] = "(T) " + Fmt(tVal).Replace(",", ".");
            kv["SumTTol"] = tVal < 6.1 ? "± 0.1" : tVal < 30.1 ? "± 0.2" : "± 0.3";

            // S (oljeborrhål D)
            double sVal = SVal(typNum);
            kv["SumS"] = "(D) " + Fmt(sVal);
            kv["SumSTol"] = sVal < 6.1 ? "± 0.1" : sVal < 30.1 ? "± 0.2" : "± 0.3";

            // H (oljespårsdjup)
            double hVal = HVal(serieInt, typNum);
            kv["SumH"] = "(H) " + Fmt(hVal).Replace(",", ".");
            kv["SumHTol"] = hVal < 6.1 ? "± 0.1" : "± 0.2";

            // E (oljeborrhål E)
            double[] eTab = GetETab(serieInt);
            double eVal = (eTab != null && typLista >= 1 && typLista <= eTab.Length) ? eTab[typLista - 1] : 0;
            kv["SumE"] = "(E) " + Fmt(eVal).Replace(".", ",");
            kv["SumETol"] = GenTolStrE(eVal);

            // J (oljeborrhål J)
            double[] jTab = GetJTab(serieInt);
            double jVal = (jTab != null && typLista >= 1 && typLista <= jTab.Length) ? jTab[typLista - 1] : 0;
            kv["SumJ"] = "(J) " + Fmt(jVal).Replace(".", ",");
            kv["SumJTol"] = GenTolStrE(jVal);

            // F (oljegenomföringshål diameter)
            bool isSpecF = Math.Abs(typNum - 378) < 0.1 || Math.Abs(typNum - 355.6) < 0.1;
            double fDia = (typNum < 85 || isSpecF) ? 2 : 3;
            kv["SumF"] = "(F) " + Fmt(fDia);
            kv["SumFTol"] = hVal < 6.1 ? "± 0.1" : "± 0.2";

            // N (oljespårsbredd)
            double nVal = NVal(typNum);
            kv["SumN"] = "(N) " + Fmt(nVal);
            kv["SumNTol"] = hVal < 6.1 ? "± 0.1" : "± 0.2";

            // Radier page 3
            bool isSpecR78 = Math.Abs(typNum - 378) < 0.1 || Math.Abs(typNum - 355.6) < 0.1;
            double r8 = typNum < 37 ? 3 : typNum < 65 ? 4 : (typNum < 85 || isSpecR78) ? 4.5 : 5;
            bool isSpecR7 = Math.Abs(typNum - 378) < 0.1 || Math.Abs(typNum - 420) < 0.1 || Math.Abs(typNum - 460) < 0.1 || Math.Abs(typNum - 355.6) < 0.1;
            string r7str = (typNum < 85 || isSpecR7) ? "1" : "2.5";
            kv["SumR8"] = "R" + Fmt(r8).Replace(".", ",");
            kv["SumR7"] = "R" + r7str;
            kv["SumV120"] = "120º";
            kv["SumV45"] = "45º";

            // Freq / Devices / AF page 3
            SumFreqOP3(kv, maskinVal);
            SumDevicesOP3(kv, maskinVal);
            for (int i = 1; i <= 11; i++) kv["SumAF2_" + (i <= 9 ? i.ToString() : (i == 10 ? "0" : "11"))] = "";

            kv["SumTextS3"] = "Grader, frifläckar, slagmärken, repor, valkar och andra defekter samt ojämnheter kontrolleras med ögonmått.";
            kv["SumTextGängstigning"] = "Max 2x gängstigning";
            kv["SumÖvrigt"] = "1 st. oljeborrhål 180º från slits<<LineBreak>>1 st. utv. oljespår med början 10º från slitscentrum";
            kv["VaLH"] = (EqualsI(tmpBet3, "H") || EqualsI(tmpBet4, "H")) ? ""
                : "Denna Mall är endast för typ OH-H<<LineBreak>>" + (EqualsI(tmpBet4, "HB") ? "Använd mall för OH-HB" : "Använd mall för OH");

            // ── PAGE 4: Slits & Muttersäkring ─────────────────────────────────────
            string mv5 = EqualsI(maskinVal, "Skepp6") ? "Skepp6" : EqualsI(maskinVal, "K&T") ? "K&T" :
                         EqualsI(maskinVal, "VTR-160") ? "VTR-160" : EqualsI(maskinVal, "MacTurn 550") ? "MacTurn 550" : "";
            kv["SumMaskinValS5"] = "Maskin: " + mv5 + " - Muttersäkring, Slits";

            // Vinklar page 4
            kv["SumVa"] = " 11.25º";
            kv["SumV1"] = " 11.25º";
            kv["SumV30"] = "30º";

            // C (slits)
            double cSlits = serieInt == 32 ? (typNum < 93 ? 8 : 10)
                          : serieInt == 39 ? (typNum < 530 ? 8 : 10)
                          : (typNum < 501 ? 8 : 10);
            kv["SumC"] = "(c) " + Fmt(cSlits);
            kv["SumCTol"] = "± 0.2";

            // d10 (innerdiameter efter slits)
            kv["Sumd10"] = "(d1) " + Fmt(tmpd1);
            kv["Sumd10Tol"] = D10Tol(typNum);
            kv["Sumd10TolN"] = D10TolN(typNum);

            // f (låsspårslängd) from TypLista1
            double[] fListPage4 = serieInt == 240 ? FList240 : serieInt == 241 ? FList241 : FListDefault;
            double f0 = (typLista1 >= 1 && typLista1 <= fListPage4.Length) ? fListPage4[typLista1 - 1] : 0;
            kv["SumF0"] = "(f) " + Fmt(f0);
            kv["SumF02"] = kv["SumF0"];
            kv["SumF0Tol"] = f0 > 50 ? "+ 3.0" : f0 > 30 ? "+ 2.5" : f0 > 19 ? "+ 2.1" : "+ 1.8";
            kv["SumF02Tol"] = kv["SumF0Tol"];
            kv["SumF0TolN"] = "  0 [3F]";
            kv["SumF02TolN"] = kv["SumF0TolN"];

            // e (låsspårsbredd) from TypLista1
            double[] eLista;
            if (serieInt == 240) eLista = EList240;
            else if (serieInt == 31 || serieInt == 32 || serieInt == 241) eLista = EList31_32_241;
            else if (serieInt == 30 || serieInt == 39) eLista = EList30_39;
            else eLista = new double[23];
            double e5 = (typLista1 >= 1 && typLista1 <= eLista.Length) ? eLista[typLista1 - 1] : 0;
            kv["SumE5"] = "(e) " + Fmt(e5);
            kv["SumE5Tol"] = E5Tol(e5);
            kv["SumE5TolN"] = " 0 [3F]";

            // Freq / Devices / AF page 4
            SumFreqOP4(kv, maskinVal);
            SumDevicesOP4(kv, maskinVal);
            for (int i = 1; i <= 5; i++) kv["SumAF1_" + i] = "";

            kv["SumTextS4"] = "";
            kv["SumRitNr4"] = RitNrPage4(serieInt);

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

        // ── Freq / Device / AF helpers ────────────────────────────────────────────
        private static void SumFreqOP1(Dictionary<string, string> kv, bool m)
        {
            kv["SumF3_1"] = ""; kv["SumF3_2"] = ""; kv["SumF3_3"] = ""; kv["SumF3_4"] = "";
            kv["SumF3_5"] = ""; kv["SumF3_6"] = ""; kv["SumF3_7"] = ""; kv["SumF3_8"] = "";
            if (m)
            {
                kv["SumF3_1"] = "1/5"; kv["SumF3_2"] = "1/2"; kv["SumF3_3"] = "1/1";
                kv["SumF3_4"] = "1/5"; kv["SumF3_5"] = "Inst."; kv["SumF3_6"] = "1/5";
                kv["SumF3_7"] = "1/2"; kv["SumF3_8"] = "1/5";
            }
            kv["SumF4_1"] = ""; kv["SumF4_2"] = ""; kv["SumF4_3"] = ""; kv["SumF4_4"] = "";
            kv["SumF4_5"] = ""; kv["SumF4_6"] = ""; kv["SumF4_7"] = ""; kv["SumF4_8"] = ""; kv["SumF4_9"] = "";
            if (m)
            {
                kv["SumF4_1"] = "1/2"; kv["SumF4_2"] = "1/2"; kv["SumF4_3"] = "1/1";
                kv["SumF4_4"] = "1/1"; kv["SumF4_5"] = "1/1"; kv["SumF4_6"] = "Inst.";
                kv["SumF4_7"] = "1/5"; kv["SumF4_8"] = "1/5"; kv["SumF4_9"] = "1/1";
            }
        }

        private static void SumDevicesOP1(Dictionary<string, string> kv, bool m, int stmm, string bygel)
        {
            string p = m ? "Tr" + stmm : "";
            kv["SumD3_1"] = m ? "Skjutmått" : ""; kv["SumD3_2"] = m ? "Skjutmått" : "";
            kv["SumD3_3"] = m ? "Multimar med " + stmm + "mm rullar" : ""; kv["SumD3_4"] = m ? "Djupmått" : "";
            kv["SumD3_5"] = m ? "Radielyra" : ""; kv["SumD3_6"] = m ? "Skjutmått/Vinkelmätare" : "";
            kv["SumD3_7"] = m ? "Gängmall Tr" + stmm : ""; kv["SumD3_8"] = m ? "Skjutmått" : "";
            kv["SumD4_1"] = m ? "Mikrometerstickmått" : ""; kv["SumD4_2"] = m ? "Skjutmått" : "";
            kv["SumD4_3"] = m ? "Mätbygel " + bygel : ""; kv["SumD4_4"] = m ? "Mätbygel " + bygel : "";
            kv["SumD4_5"] = m ? "Mätbygel " + bygel : ""; kv["SumD4_6"] = m ? "Mätmaskin" : "";
            kv["SumD4_7"] = m ? "Egglinjal" : ""; kv["SumD4_8"] = m ? "Egglinjal" : ""; kv["SumD4_9"] = m ? "Okulärkontroll" : "";
        }

        private static void SumAFOP1(Dictionary<string, string> kv, bool m, string gVar, string vink, string ml, string orund, string rakA, string rakB)
        {
            kv["SumAF3_1"] = ""; kv["SumAF3_2"] = "";
            kv["SumAF3_3"] = m ? "Kontrolleras med klove utf.2" : "";
            kv["SumAF3_4"] = ""; kv["SumAF3_5"] = ""; kv["SumAF3_6"] = ""; kv["SumAF3_7"] = "";
            kv["SumAF3_8"] = m ? "Hjälpmått" : "";
            kv["SumAF4_1"] = ""; kv["SumAF4_2"] = "";
            kv["SumAF4_3"] = m ? "Tol:" : "";
            kv["SumAF4_4"] = m ? "Tol: " + gVar : "";
            kv["SumAF4_5"] = m ? "Tol: " + vink + " " + ml : "";
            kv["SumAF4_6"] = m ? "Max: " + orund : "";
            kv["SumAF4_7"] = m ? rakA : ""; kv["SumAF4_8"] = m ? rakB : "";
            kv["SumAF4_9"] = m ? "Vid misstänkt fel Ra-mätare" : "";
        }

        private static void SumFreqOP3(Dictionary<string, string> kv, string mv)
        {
            bool isSk = EqualsI(mv, "Skepp6"), isDubb = EqualsI(mv, "Dubbelparet");
            bool isOther = EqualsI(mv, "K&T") || EqualsI(mv, "VTR-160") || EqualsI(mv, "MacTurn 550");
            string f = isSk ? "1/1" : isDubb ? "1/10" : isOther ? "1/2" : "";
            for (int i = 1; i <= 9; i++) kv["SumF2_" + i] = f;
            kv["SumF2_0"] = f;
            kv["SumF2_11"] = f;
        }

        private static void SumDevicesOP3(Dictionary<string, string> kv, string mv)
        {
            bool isSk = EqualsI(mv, "Skepp6");
            bool isAll = isSk || EqualsI(mv, "K&T") || EqualsI(mv, "VTR-160") || EqualsI(mv, "MacTurn 550") || EqualsI(mv, "Dubbelparet");
            bool isOther = !isSk && isAll;
            kv["SumD2_1"] = isSk ? "Skala på borrmaskin" : isOther ? "pipborr/djupmått" : "";
            kv["SumD2_2"] = isAll ? "Skjutmått" : "";
            kv["SumD2_3"] = isAll ? "Skjutmått" : "";
            kv["SumD2_4"] = isAll ? "Gängtolk" : "";
            kv["SumD2_5"] = isAll ? "Skjutmått" : "";
            kv["SumD2_6"] = isAll ? "Skjutmått/fasmall" : "";
            kv["SumD2_7"] = isAll ? "Skjutmått" : "";
            kv["SumD2_8"] = isAll ? "Skjutmått" : "";
            kv["SumD2_9"] = isSk ? "Höjdrits" : isOther ? "pipborr/djupmått" : "";
            kv["SumD2_0"] = isAll ? "Skjutmått" : "";
            kv["SumD2_11"] = isAll ? "Radieyra" : "";
        }

        private static void SumFreqOP4(Dictionary<string, string> kv, string mv)
        {
            bool isSk = EqualsI(mv, "Skepp6");
            bool isOther = EqualsI(mv, "K&T") || EqualsI(mv, "VTR-160") || EqualsI(mv, "MacTurn 550");
            string f = isSk ? "1/1" : isOther ? "1/2" : "";
            for (int i = 1; i <= 5; i++) kv["SumF1_" + i] = f;
        }

        private static void SumDevicesOP4(Dictionary<string, string> kv, string mv)
        {
            bool isAll = EqualsI(mv, "Skepp6") || EqualsI(mv, "K&T") || EqualsI(mv, "VTR-160") || EqualsI(mv, "MacTurn 550");
            kv["SumD1_1"] = isAll ? "Skjutmått" : ""; kv["SumD1_2"] = isAll ? "Skjutmått" : "";
            kv["SumD1_3"] = isAll ? "Skjutmått" : ""; kv["SumD1_4"] = "";
            kv["SumD1_5"] = isAll ? "Skjutmått" : "";
        }

        // ── Dimension helpers ─────────────────────────────────────────────────────
        private static int Stmm(double d) { if (d < 301) return 4; if (d < 501) return 5; if (d < 701) return 6; if (d < 901) return 7; return 8; }
        private static double DM(double d, int s) { if (s == 4) return d - 2; if (s == 5) return d - 2.5; if (s == 6) return d - 3; if (s == 7) return d - 3.5; return d - 4; }
        private static double D3(double dm, int s) { if (s == 4) return dm - 2.5; if (s == 5) return dm - 3; if (s == 6) return dm - 4; if (s == 7) return dm - 4.5; return dm < 1300 ? dm - 5 : dm - 4.5; }
        private static double DOP1TolN(int s) { if (s == 4) return 0.300; if (s == 5) return 0.335; if (s == 6) return 0.375; if (s == 7) return 0.425; return 0.450; }
        private static double DmTolPos(int s) { if (s == 4) return 0.190; if (s == 5) return 0.212; if (s == 6) return 0.236; if (s == 7) return 0.250; return 0.265; }
        private static double DmTolNeg(int s) { if (s == 4) return 0.630; if (s == 5) return 0.710; if (s == 6) return 0.800; if (s == 7) return 0.850; return 0.950; }
        private static double D3TolN(int s) { if (s == 4) return 0.750; if (s == 5) return 0.850; if (s == 6) return 0.950; if (s == 7) return 1.000; return 1.120; }
        private static double JS9Tol(double v) { if (v < 3.01) return 0.012; if (v < 6.01) return 0.015; if (v < 10.01) return 0.018; if (v < 18.01) return 0.021; if (v < 30.01) return 0.026; if (v < 50.01) return 0.031; if (v < 80.01) return 0.037; if (v < 120.01) return 0.043; if (v < 180.01) return 0.050; if (v < 250.01) return 0.057; if (v < 315.01) return 0.065; if (v < 400.01) return 0.070; if (v < 500.01) return 0.077; if (v < 630.01) return 0.087; if (v < 800.01) return 0.100; if (v < 1000.01) return 0.115; if (v < 1250.01) return 0.130; if (v < 1600.01) return 0.155; if (v < 2000.01) return 0.185; if (v < 2500.01) return 0.220; return 0.270; }
        private static double H15TolN(double L) { if (L < 3.01) return 0.400; if (L < 6.01) return 0.480; if (L < 10.01) return 0.580; if (L < 18.01) return 0.700; if (L < 30.01) return 0.840; if (L < 50.01) return 1.000; if (L < 80.01) return 1.200; if (L < 120.01) return 1.400; if (L < 180.01) return 1.600; if (L < 250.01) return 1.850; if (L < 315.01) return 2.100; if (L < 400.01) return 2.300; if (L < 500.01) return 2.500; if (L < 630.01) return 2.800; if (L < 800.01) return 3.200; if (L < 1000.01) return 3.600; if (L < 1250.01) return 4.200; if (L < 1600.01) return 5.000; if (L < 2000.01) return 6.000; if (L < 2500.01) return 7.000; return 8.600; }
        private static double BTol(double b) { if (b < 11) return 1.5; if (b < 19) return 1.8; if (b < 31) return 2.1; if (b < 51) return 2.5; if (b < 81) return 3.0; if (b < 121) return 3.5; return 4.0; }
        private static double GTjTolPos(double d, double k) { if (k == 12) { if (d > 1000) return 0.095; if (d > 800) return 0.085; if (d > 630) return 0.075; if (d > 500) return 0.070; if (d > 400) return 0.065; if (d > 315) return 0.060; if (d > 250) return 0.055; if (d > 180) return 0.050; if (d > 120) return 0.040; if (d > 80) return 0.035; if (d > 50) return 0.030; if (d > 30) return 0.025; return 0.020; } if (k == 30) { if (d > 1000) return 0.060; if (d > 800) return 0.055; if (d > 630) return 0.050; if (d > 500) return 0.045; if (d > 400) return 0.040; if (d > 315) return 0.035; if (d > 250) return 0.035; if (d > 180) return 0.030; if (d > 120) return 0.025; if (d > 80) return 0.022; if (d > 50) return 0.019; if (d > 30) return 0.016; return 0.013; } return 0; }
        private static double GTjTolNeg(double d, double k) { if (k == 12) { if (d > 1000) return 0.280; if (d > 800) return 0.250; if (d > 630) return 0.225; if (d > 500) return 0.200; if (d > 400) return 0.190; if (d > 315) return 0.175; if (d > 250) return 0.160; if (d > 180) return 0.140; if (d > 120) return 0.120; if (d > 80) return 0.105; if (d > 50) return 0.090; if (d > 30) return 0.075; return 0.070; } if (k == 30) { if (d > 1000) return 0.170; if (d > 800) return 0.155; if (d > 630) return 0.140; if (d > 500) return 0.125; if (d > 400) return 0.115; if (d > 315) return 0.105; if (d > 251) return 0.095; if (d > 180) return 0.085; if (d > 120) return 0.075; if (d > 80) return 0.065; if (d > 50) return 0.055; if (d > 30) return 0.046; return 0.039; } return 0; }
        private static double GVar(double d) { if (d > 1000) return 0.050; if (d > 800) return 0.045; if (d > 630) return 0.040; if (d > 500) return 0.035; if (d > 315) return 0.030; if (d > 250) return 0.025; if (d > 180) return 0.020; if (d > 120) return 0.015; if (d > 50) return 0.010; return 0.008; }
        private static double GenTolVal(double v) { if (v < 6.01) return 0.1; if (v < 30.01) return 0.2; if (v < 120.01) return 0.3; if (v < 400.01) return 0.5; if (v < 1000.01) return 0.8; if (v < 2000.01) return 1.2; return 2.0; }
        private static double RakA(double d) { if (d < 101) return 0.008; if (d < 281) return 0.010; if (d < 481) return 0.012; if (d < 601) return 0.014; if (d < 901) return 0.016; return 0.020; }
        private static double RakB(double d) { if (d < 101) return 0.012; if (d < 281) return 0.015; if (d < 481) return 0.018; if (d < 601) return 0.021; if (d < 901) return 0.024; return 0.030; }
        private static double VinkAngle(double d) { if (d > 1000) return 0.09; if (d > 800) return 0.10; if (d > 630) return 0.11; if (d > 500) return 0.12; if (d > 400) return 0.13; if (d > 180) return 0.15; if (d > 150) return 0.18; if (d > 120) return 0.30; if (d > 80) return 0.45; if (d > 50) return 0.50; return 0.60; }
        private static string Fas(double d, string pr) { if (EqualsI(pr, "MS-7434039")) return "4.4x30º"; if (d < 301) return "2.7x45º"; if (d < 501) return "3.2x45º"; if (d < 671) return "3.8x45º"; if (d < 901 || Math.Abs(d - 1060) < 0.1) return "4.4x45º"; return "5x45º"; }
        private static string G1(double t) { bool sp = Math.Abs(t - 420) < 0.1 || Math.Abs(t - 355.6) < 0.1 || Math.Abs(t - 378) < 0.1; if (t < 85 || sp) return "M6"; if (t < 561 || Math.Abs(t - 630) < 0.1) return "M8"; if (Math.Abs(t - 1060) < 0.1) return "G1/4"; return "G1/8"; }
        private static double C1(double t) { bool sp = Math.Abs(t - 420) < 0.1 || Math.Abs(t - 355.6) < 0.1 || Math.Abs(t - 378) < 0.1; if (t < 85 || sp) return 9; if (t < 561 || Math.Abs(t - 630) < 0.1) return 12; if (Math.Abs(t - 1060) < 0.1) return 15; return 13; }
        private static double TVal(double t) { bool sp = Math.Abs(t - 420) < 0.1 || Math.Abs(t - 355.6) < 0.1 || Math.Abs(t - 378) < 0.1; if (t < 85 || sp) return 6.3; if (t < 561 || Math.Abs(t - 630) < 0.1) return 8.3; if (Math.Abs(t - 1060) < 0.1) return 13.5; return 10; }
        private static double SVal(double t) { bool sp = Math.Abs(t - 420) < 0.1 || Math.Abs(t - 355.6) < 0.1 || Math.Abs(t - 378) < 0.1; if (t < 85 || sp) return 3; if (t < 561 || Math.Abs(t - 630) < 0.1) return 4; if (Math.Abs(t - 1060) < 0.1) return 8; return 5; }
        private static double HVal(int serie, double t)
        {
            if (serie == 30) { if (t < 37) return 0.8; if (t < 65) return 1.0; if (t < 85 || Math.Abs(t - 378) < 0.1) return 1.2; if (t < 751) return 1.5; return 2.0; }
            if (serie == 31) { if (t < 37) return 0.8; if (t < 65) return 1.0; if (t < 85 || Math.Abs(t - 355.6) < 0.1) return 1.2; if (t < 601) return 1.5; if (t < 751) return 2.0; return 2.8; }
            if (serie == 23) { if (t < 37) return 0.8; if (t < 65) return 1.0; return 1.2; }
            if (serie == 32) { if (t < 37) return 0.8; if (t < 65) return 1.0; if (t < 85 || Math.Abs(t - 378) < 0.1) return 1.2; if (t < 751) return 1.5; return 2.0; } // TmpH32=[TmpH30]
            // serie 39
            if (t < 37) return 0.8; if (t < 65) return 1.0; if (t < 85) return 1.2; if (t < 631) return 1.5; if (t < 751) return 2.0; return 2.8;
        }
        private static double NVal(double t) { bool sp378 = Math.Abs(t - 378) < 0.1 || Math.Abs(t - 355.6) < 0.1; bool sp420 = Math.Abs(t - 420) < 0.1 || Math.Abs(t - 460) < 0.1 || Math.Abs(t - 457.2) < 0.1; if (t < 37) return 4; if (t < 65) return 5; if (t < 85 || sp378) return 6; if (t < 601 || sp420) return 7; if (t < 751) return 8; return 9; }
        private static string D10Tol(double t) { if (t < 65) return "+ 0.210"; if (t < 85) return "+ 0.360"; if (t < 531) return "+ 0.400"; if (t < 671) return "+ 0.440"; if (t < 851) return "+ 0.500"; return "+ 0.560"; }
        private static string D10TolN(double t) { if (t < 65) return "- 0.320"; if (t < 85) return "- 0.570"; if (t < 531) return "- 0.630"; if (t < 671) return "- 0.700"; if (t < 851) return "- 0.800"; return "- 0.900"; }
        private static string E5Tol(double e) { if (e > 50) return "+ 0.740"; if (e > 30) return "+ 0.620"; if (e > 18) return "+ 0.520"; if (e > 10) return "+ 0.430"; if (e > 6) return "+ 0.360"; return "+ 0.300"; }
        private static string GenTolStrE(double v) { if (v < 6.1) return "± 0.1"; if (v < 30.1) return "± 0.2"; if (v < 120.1) return "± 0.3"; if (v < 315.1) return "± 0.5"; if (v < 1000.1) return "± 0.8"; if (v < 2000.1) return "± 1.2"; return "± 2.0"; }

        // ── Table lookups ─────────────────────────────────────────────────────────
        private static string[] GetSerieTypList(int s) { if (s == 30) return Typ30; if (s == 31) return Typ31; if (s == 23) return Typ23; if (s == 32) return Typ32; if (s == 39) return Typ39; return new string[0]; }
        private static double[] GetBTab(int s) { if (s == 30) return BTab30; if (s == 31) return BTab31; if (s == 23) return BTab23; if (s == 32) return BTab32; if (s == 39) return BTab39; return null; }
        private static double[] GetETab(int s) { if (s == 30) return ETab30; if (s == 31) return ETab31; if (s == 23) return ETab23; if (s == 32) return ETab32; if (s == 39) return ETab39; return null; }
        private static double[] GetJTab(int s) { if (s == 30) return JTab30; if (s == 31) return JTab31; if (s == 23) return JTab23; if (s == 32) return JTab32; if (s == 39) return JTab39; return null; }

        // ── Ritning helpers ───────────────────────────────────────────────────────
        private static string RitningsnrPage12(int s, string bet) { if (s == 30) return "7438957"; if (s == 31) return "7438958"; if (s == 32) return "7438955"; if (s == 39) return "7434032"; if (s == 240) return "7432901"; return bet; }
        private static string RitNrPage3(int s) { if (s == 23 || s == 30) return "7434151"; if (s == 31) return "7434152"; if (s == 32) return "7434153"; if (s == 39) return "7434170, 7434171"; return "Fel Mall"; }
        private static string RitNrPage4(int s) { if (s == 30) return "7438957"; if (s == 31) return "7438958"; if (s == 32) return "7438955"; if (s == 39) return "7434032"; if (s == 241) return "7432903"; if (s == 240) return "7432901"; return "Fel Mall"; }

        // ── Utilities ─────────────────────────────────────────────────────────────
        private static int GetMember(string val, string[] list) { for (int i = 0; i < list.Length; i++) if (string.Equals(list[i], val, StringComparison.OrdinalIgnoreCase)) return i + 1; return 0; }
        private static bool IsInGroup(string mv, string[] g) { if (string.IsNullOrEmpty(mv)) return false; for (int i = 0; i < g.Length; i++) if (string.Equals(g[i], mv, StringComparison.OrdinalIgnoreCase)) return true; return false; }
        private static bool EqualsI(string a, string b) => string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);
        private static int TryParseInt(string s) { int v; return int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0; }
        private static double TryParseDouble(string s) { if (string.IsNullOrWhiteSpace(s)) return 0; double v; return double.TryParse(s.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0; }
        private static double GetDouble(List<Bookmark> bm, string key) { string r = GetString(bm, key); if (string.IsNullOrWhiteSpace(r)) return 0; double v; return double.TryParse(r.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0; }
        private static string GetString(List<Bookmark> bm, string key) { if (bm == null || string.IsNullOrWhiteSpace(key)) return ""; for (int i = 0; i < bm.Count; i++) { Bookmark b = bm[i]; if (b != null && string.Equals(b.BookmarkName ?? "", key, StringComparison.OrdinalIgnoreCase)) return b.BookmarkValue ?? ""; } return ""; }
        private static string Fmt(double v) => v.ToString(CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt3(double v) => v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
    }
}