using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    
    public class HYLSOR_Kilhylsor_38_39_44_1400_svarv_OP1 : ITemplateCalculations
    {
        private static readonly string[] MachinesPage1 = { "VTR-160", "MacTurn 550" };

        private static readonly string[] Typ38 = { "52", "56", "60", "64", "68", "72", "76", "80", "84", "88", "92", "96", "500", "530", "560", "600", "630", "670", "710", "750", "800", "850", "900", "950", "1000", "1060", "1120", "1180", "1250", "1320", "1400" };
        private static readonly string[] Typ39 = { "44", "48", "52", "56", "60", "64", "68", "72", "76", "80", "84", "88", "92", "96", "500", "530", "560", "600", "630", "670", "710", "750", "800", "850", "900", "950", "1000", "1060", "1120", "1180", "1250" };

        private static readonly double[] L38Lista = { 55, 62, 72, 72, 72, 72, 87, 87, 87, 87, 102, 102, 102, 106, 106, 115, 130, 130, 140, 150, 160, 160, 165, 175, 195, 195, 208, 208, 215, 236, 254 };
        private static readonly double[] L39Lista = { 71, 71, 85, 85, 103, 103, 103, 103, 118, 118, 118, 132, 132, 140, 140, 150, 155, 165, 180, 185, 200, 206, 218, 224, 236, 250, 265, 280, 280, 300, 315 };

        private static readonly double[] D3_39Lista = { 218, 238, 258, 278, 300, 318, 338, 358, 378, 398, 418, 438, 458, 478, 498, 526, 556, 596, 630, 670, 710, 750, 800, 850, 900, 950, 1000, 1060, 1120, 1180, 1250 };
        // Tmpd3_38Lista in the original Lotus script is all zeros AND length-mismatched
        // (28 vs 31) — preserved here as a null lookup so d3 resolves to 0 for serie 38.
        private static readonly double[] D3_38Lista = null;

        // The Lotus @NewLine used inside Word table cells is emitted by this engine's
        // line-break token (matches the reference template's AF cell handling).
        private const string CellLineBreak = "<<LineBreak>>";

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;
            List<Bookmark> bm = req != null ? req.Bookmarks : null;

            // ---- POPUP-INFO (VaLPopUp) ----
            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            // ---- FORMATERING ----
            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");

            // ---- SKAPA LISTA & DELA AV ----
            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : "";
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : "";
            string tmpBet4 = tokens.Length > 3 ? tokens[3] : "";

            // TmpCountV21 = @Member("V21"; Bet1a:Bet2a:Bet3a:Bet4a:Bet5a)  (1-based, 0 if absent)
            string[] tok5 = new string[5];
            for (int i = 0; i < 5; i++) tok5[i] = i < tokens.Length ? tokens[i] : "";
            int tmpCountV21 = GetMember("V21", tok5);

            // ---- KONTROLL AV SPECIELLA EGENSKAPER ----
            bool tmpLULW = Contains(tmpBet, "LU") || Contains(tmpBet, "LW");
            bool tmpMS = Contains(tmpBet, "MS");
            bool tmp237774 = Contains(tmpBet, "237774");
            bool tmp7432987 = Contains(tmpBet, "7432987");

            // ---- RÄKNARE ----
            int tmpCount = tmpBet2.Length;                 // @Length(TmpBet2)
            bool tmpSpecial = tmpBet.Length > 10;          // @Length("[TmpBet]2") > 10 (see reference note)

            // ---- EXTRAHERA SERIE & TYP ----
            string tmpSerie = tmpLULW ? "0" : (tmpBet2.Length >= 2 ? tmpBet2.Substring(0, 2) : tmpBet2);
            int serieInt = TryParseInt(tmpSerie);
            // TmpTyp: original inner @if(Tmp/;Bet3;Bet3) returns Bet3 in both branches — only Count>3 matters.
            string tmpTyp = tmpLULW ? "0" : (tmpCount > 3 ? (tmpBet2.Length >= 2 ? tmpBet2.Substring(tmpBet2.Length - 2) : tmpBet2) : tmpBet3);
            int typInt = TryParseInt(tmpTyp);

            // ---- SERIELISTA & TYPLISTA ----
            string[] serieTypList = serieInt == 38 ? Typ38 : Typ39;
            int typLista = GetMember(tmpTyp, serieTypList);

            // ---- KONA ----
            double tmpKona = GetDouble(bm, "Kona");
            if (tmpKona == 0) tmpKona = (serieInt == 39 || serieInt == 38) ? 12 : 30;
            kv["SumKona"] = "Kona  1:" + Fmt(tmpKona);

            // ---- LÄNGD (L) & TOL h15 ----
            double[] lLista = serieInt == 38 ? L38Lista : L39Lista;
            double lBm = GetDouble(bm, "Längd (L)");
            double tmpL = lBm != 0 ? lBm : GetTabVal(lLista, typLista);
            kv["SumL"] = "(L) " + Fmt(tmpL);
            kv["SumLTol"] = "+ 0";                          // TmpLTol = 0
            double tmpLTolN = tmp7432987 ? 0.46 : H15TolN(tmpL);
            kv["SumLTolN"] = "- " + Fmt3(tmpLTolN) + " [3F]";

            // ---- YTTERDIAMETER LILLKONA (d) ----
            double amatt = GetDouble(bm, "a-mått");
            double tmpda = GetDouble(bm, "Ytterdiameter lillkona (d)");
            double tmpd;
            if (tmpda == 0)
                tmpd = tmpCount > 3 ? (typInt / 2.0) * 10.0 : TryParseDouble(tmpBet3);
            else
                tmpd = tmpda + (amatt / tmpKona);
            tmpd = Math.Round(tmpd, 3);
            kv["Sumd"] = "(d) " + Fmt(tmpd).Replace(".",",");

            // ---- INNERDIAMETER (d1) & TOL JS10 ----
            double d1Bm = GetDouble(bm, "Innerdiameter (d1)");
            double tmpd1;
            if (d1Bm == 0)
            {
                // @if(TmpSpecial=False | TmpCountV21=3:4; <typ-formel>; <Bet3/Bet4>)
                bool useTypFormula = !tmpSpecial || tmpCountV21 == 3 || tmpCountV21 == 4;
                if (useTypFormula)
                    tmpd1 = D1FromTyp(typInt, tmpd);
                else
                    tmpd1 = tmpCount > 3 ? TryParseDouble(tmpBet3) : TryParseDouble(tmpBet4);
            }
            else tmpd1 = d1Bm;
            kv["Sumd1"] = "(d1) " + Fmt(tmpd1).Replace(".", ",");
            double d1Tol = tmp7432987 ? 0.078 : JS10Tol(tmpd1);
            kv["Sumd1Tol"] = "\u00b1 " + Fmt3(d1Tol);

            // ---- GODSTJOCKLEKSTOLERANSER ----
            bool tmpTolNNoll = Contains(tmpBet, "7433833"); // undre tolerans = 0 för denna artikel
            double gTol = GTolPos(tmpd, tmpKona);
            double gTolN = tmpTolNNoll ? 0 : GTolNeg(tmpd, tmpKona);
            kv["SumGodstjocklekTol"] = "+ " + Fmt3(gTol);
            kv["SumGodstjocklekTolN"] = "- " + Fmt3(gTolN);
            double tmpToleransskillnad = (gTolN - gTol) / 2.0; // beräknas i originalet men används ej

            // ---- YTTERDIAMETER STORKONA (d2) ----
            double tmpd2 = Math.Round((tmpL / tmpKona) + tmpd, 3);
            kv["Sumd2"] = "(d2) " + Fmt(tmpd2).Replace(".", ",");

            // ---- INNERDIAMETER/FAS (d3) & TOL JS13 ----
            double[] d3Lista = serieInt == 38 ? D3_38Lista : D3_39Lista;
            double d3Bm = GetDouble(bm, "Fasdiameter (d3)");
            double tmpd3 = d3Bm != 0 ? d3Bm : GetTabVal(d3Lista, typLista);
            kv["Sumd3"] = "(d3) " + Fmt(tmpd3).Replace(".", ",");
            kv["Sumd3Tol"] = tmp237774 ? "+ 2.3\n- 0" : "\u00b1 " + Fmt3(JS13Tol(tmpd3));

            // ---- YTJÄMNHET (Ra) ----
            kv["SumRa25"] = "2.5";
            kv["SumRa25a"] = "2.5";
            kv["SumRa5"] = "5";

            // ---- RADIE & VINKLAR ----
            kv["SumR"] = (serieInt == 39) ? "R2"
                       : (serieInt == 38) ? "R2"
                       : tmpMS ? "R3" : "R1.5";
            kv["Sum45"] = tmp7432987 ? "R1" : "45\u00b0";

            // ---- RAKHET (A) & (B) ----  (@Text utan format => allmän)
            double rhA = RakA(tmpd);
            double rhB = RakB(tmpd);
            kv["SumRHA"] = "max: " + Fmt(rhA) + " [2F]";
            kv["SumRHB"] = "max: " + Fmt(rhB) + " [2F]";

            // ---- ORUNDHET (Rd) ----  (används i anmärkningsfält AF1_11)
            string tmpRd = "max: " + OrundhetStr(tmpd1) + " [3F]";

            // ---- GODSTJOCKLEKSVARIATION (GV) ----
            kv["SumGV"] = "max: " + GVarStr(tmpd) + " [2F]";

            // ---- MÄTLÄNGD ----
            double tmpTML = tmpL - 7.0;
            double tmpML = tmpTML < 80 ? 50 : tmpTML < 110 ? 75 : 100;
            kv["SumML"] = "ML=" + Fmt(tmpML);

            // ---- VINKELTOLERANS (VT) ----
            double vtList = VTList(tmpd);
            double tmpVT = Math.Round(tmpML * vtList / 1000.0, 4);
            kv["SumVT"] = "Konavvikelse: \u00b1 " + Fmt4(tmpVT) + " [2F]";

            // ---- MÄTBYGELINSTÄLLNING ----
            double tmp8 = Math.Round(((tmpd2 - tmpd1) / 2.0) - ((8.0 + 1.0) / (2.0 * tmpKona)), 3);
            double tmp83 = Math.Round(((tmpd2 - tmpd1) / 2.0) - ((83.0 + 1.0) / (2.0 * tmpKona)), 3);
            double tmp108 = Math.Round(((tmpd2 - tmpd1) / 2.0) - ((108.0 + 1.0) / (2.0 * tmpKona)), 3);
            double tmp40 = Math.Round(((tmpd2 - tmpd1) / 2.0) - ((40.0 + 1.0) / (2.0 * tmpKona)), 3);
            double tmp140 = Math.Round(((tmpd2 - tmpd1) / 2.0) - ((140.0 + 1.0) / (2.0 * tmpKona)), 3);

            double sumL1 = tmpTML < 110 ? 83 : tmpTML < 145 ? 108 : 140;
            double sumL2 = tmpTML < 145 ? 8 : 40;
            double sumE1 = tmpTML < 110 ? tmp83 : tmpTML < 145 ? tmp108 : tmp140;
            double sumE2 = tmpTML < 145 ? tmp8 : tmp40;
            kv["SumL1"] = Fmt(sumL1);
            kv["SumL2"] = Fmt(sumL2);
            kv["SumE1"] = Fmt(sumE1).Replace(".", ",");
            kv["SumE2"] = Fmt(sumE2).Replace(".", ",");

            // ---- BYGLAR (3-grenad efter mätlängd TML) ----
            string bygel3 = tmpTML < 80 ? "7419469" : tmpTML < 145 ? "SR 7419471" : "SR 7415991";
            kv["SumBygGV"] = bygel3;
            kv["SumBygVT"] = bygel3;
            kv["SumBygGT"] = bygel3;

            // ---- MASKINVAL ----
            string s1 = EqualsI(maskinVal, "VTR-160") ? "VTR-160" : EqualsI(maskinVal, "MacTurn 550") ? "MacTurn 550" : "";
            kv["SumMaskinValS1"] = "Maskin: " + s1 + " - OP1";

            bool mP1 = IsInGroup(maskinVal, MachinesPage1);

            // ---- MÄTFREKVENSER (F) ----
            SumFreqPage1(kv, mP1);
            // ---- MÄTDON (D) ----
            SumDevicesPage1(kv, mP1, bygel3);
            // ---- ANMÄRKNINGSFÄLT (AF) ----
            SumAFPage1(kv, mP1, kv["SumRHA"], kv["SumRHB"], kv["SumGV"], kv["SumGodstjocklekTol"], kv["SumGodstjocklekTolN"], kv["SumVT"], tmpRd);

            // ---- ÖVRIG TEXT ----
            kv["SumTextS1"] = "Bryt alla kanter, avlägsna";

            // ---- RITNINGAR & KLASSADE EGENSKAPER ----
            string ritnr = GetString(bm, "Ritningsnummer");
            string sumRitningsnr;
            if (string.IsNullOrEmpty(ritnr) || EqualsI(ritnr, "0"))
                sumRitningsnr = serieInt == 38 ? "238000" : serieInt == 39 ? "226472" : tmpBet;
            else
                sumRitningsnr = ritnr;
            kv["SumRitningsnr"] = sumRitningsnr;
            kv["SumRitTol"] = "Toleranser: 1432010";
            kv["SumRitYtjämnhet"] = "Yta: 7430184";
            kv["SumKlEgenskaper"] = "PRODUCTION NUTS & SLEEVES & HOUSINGS\\ALLMÄNKLASSADE EGENSKAPER\\Klassade egenskaper kilhylsor";

            // ---- BYGLAR (mätdonsbeställningssektionen) ----
            string bygelGtj = tmpML < 145 ? "SR 7419471" : "SR 7415991";
            kv["SumBygGtj"] = bygelGtj;
            kv["SumBygGVar"] = bygelGtj;
            kv["SumBygVinkTol"] = bygelGtj;

            return kv;
        }

        // ======================= POPUP =======================

        private static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published)) return "";
            DateTime pubDt;
            if (!DateTime.TryParse(published, out pubDt)) return "";
            DateTime validTill = pubDt.AddDays(14);   // TmpDagar = 14
            if (DateTime.Today <= validTill.Date)
                return "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)\n\n" +
                       "Information om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument\n\n" +
                       "Popupruta aktiv till " + validTill.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return "";
        }

        // ================ FREKVENS / MÄTDON / AF ================

        private static void SumFreqPage1(Dictionary<string, string> kv, bool m)
        {
            kv["SumF1_1"] = m ? "1/5" : ""; kv["SumF1_2"] = m ? "1/5" : ""; kv["SumF1_3"] = m ? "1/5" : "";
            kv["SumF1_4"] = m ? "1/5" : ""; kv["SumF1_5"] = m ? "1/5" : ""; kv["SumF1_6"] = m ? "1/5" : "";
            kv["SumF1_7"] = m ? "1/2" : ""; kv["SumF1_8"] = m ? "1/5" : ""; kv["SumF1_9"] = m ? "1/5" : "";
            kv["SumF1_0"] = m ? "1/5" : ""; kv["SumF1_11"] = m ? "1/5" : "";
        }

        private static void SumDevicesPage1(Dictionary<string, string> kv, bool m, string bygel)
        {
            kv["SumD1_1"] = m ? "Skjutmått" : ""; kv["SumD1_2"] = m ? "Skjutmått" : ""; kv["SumD1_3"] = m ? "Skjutmått" : "";
            kv["SumD1_4"] = m ? "Radielyra" : ""; kv["SumD1_5"] = m ? "Skjutmått" : "";
            kv["SumD1_6"] = m ? "Egglinjal" : ""; kv["SumD1_7"] = m ? "Egglinjal" : "";
            kv["SumD1_8"] = m ? "Mätbygel " + bygel : ""; kv["SumD1_9"] = m ? "Mätbygel " + bygel : "";
            kv["SumD1_0"] = m ? "Mätbygel " + bygel : ""; kv["SumD1_11"] = m ? "Mätmaskin" : "";
        }

        private static void SumAFPage1(Dictionary<string, string> kv, bool m, string rha, string rhb, string gv, string gTol, string gTolN, string vt, string rd)
        {
            kv["SumAF1_1"] = ""; kv["SumAF1_2"] = ""; kv["SumAF1_3"] = ""; kv["SumAF1_4"] = ""; kv["SumAF1_5"] = "";
            kv["SumAF1_6"] = m ? rha.Replace(".",",") : "";
            kv["SumAF1_7"] = m ? rhb.Replace(".", ",") : "";
            kv["SumAF1_8"] = m ? gv : "";
            kv["SumAF1_9"] = m ? gTol + CellLineBreak + " " + gTolN : "";
            kv["SumAF1_0"] = m ? vt.Replace(".", ",") : "";
            kv["SumAF1_11"] = m ? rd : "";
        }

        // ================ TOLERANS-/TABELLFUNKTIONER ================

        private static double D1FromTyp(int typ, double d)
        {
            if (typ < 61) return d - 10;
            if (typ < 501) return d - 15;
            if (typ < 671) return d - 20;
            if (typ < 901) return d - 25;
            return d - 30;
        }

        private static double H15TolN(double L)
        {
            if (L < 3.01) return 0.400; if (L < 6.01) return 0.480; if (L < 10.01) return 0.580;
            if (L < 18.01) return 0.700; if (L < 30.01) return 0.840; if (L < 50.01) return 1.000;
            if (L < 80.01) return 1.200; if (L < 120.01) return 1.400; if (L < 180.01) return 1.600;
            if (L < 250.01) return 1.850; if (L < 315.01) return 2.100; if (L < 400.01) return 2.300;
            if (L < 500.01) return 2.500; if (L < 630.01) return 2.800; if (L < 800.01) return 3.200;
            if (L < 1000.01) return 3.600; if (L < 1250.01) return 4.200; if (L < 1600.01) return 5.000;
            if (L < 2000.01) return 6.000; if (L < 2500.01) return 7.000; return 8.600;
        }

        private static double JS10Tol(double v)
        {
            if (v < 3.01) return 0.020; if (v < 6.01) return 0.024; if (v < 10.01) return 0.029;
            if (v < 18.01) return 0.035; if (v < 30.01) return 0.042; if (v < 50.01) return 0.050;
            if (v < 80.01) return 0.060; if (v < 120.01) return 0.070; if (v < 180.01) return 0.080;
            if (v < 250.01) return 0.092; if (v < 315.01) return 0.105; if (v < 400.01) return 0.115;
            if (v < 500.01) return 0.125; if (v < 630.01) return 0.140; if (v < 800.01) return 0.160;
            if (v < 1000.01) return 0.180; if (v < 1250.01) return 0.210; if (v < 1600.01) return 0.250;
            if (v < 2000.01) return 0.300; if (v < 2500.01) return 0.350; return 0.430;
        }

        private static double JS13Tol(double v)
        {
            if (v < 3.01) return 0.070; if (v < 6.01) return 0.090; if (v < 10.01) return 0.110;
            if (v < 18.01) return 0.135; if (v < 30.01) return 0.165; if (v < 50.01) return 0.195;
            if (v < 80.01) return 0.230; if (v < 120.01) return 0.270; if (v < 180.01) return 0.315;
            if (v < 250.01) return 0.360; if (v < 315.01) return 0.405; if (v < 400.01) return 0.445;
            if (v < 500.01) return 0.485; if (v < 630.01) return 0.550; if (v < 800.01) return 0.625;
            if (v < 1000.01) return 0.700; if (v < 1250.01) return 0.825; if (v < 1600.01) return 0.975;
            if (v < 2000.01) return 1.150; if (v < 2500.01) return 1.400; return 1.650;
        }

        // Godstjocklekstolerans, övre (+). Kona 12 resp. 30. Övrigt => 0 ("Fel Kona").
        private static double GTolPos(double d, double k)
        {
            if (Math.Abs(k - 12) < 0.001) { if (d > 1250) return 0.100; if (d > 1000) return 0.095; if (d > 800) return 0.085; if (d > 630) return 0.075; if (d > 500) return 0.070; if (d > 400) return 0.065; if (d > 315) return 0.060; if (d > 250) return 0.055; if (d > 180) return 0.050; if (d > 120) return 0.040; if (d > 80) return 0.035; if (d > 50) return 0.030; if (d > 30) return 0.025; return 0.020; }
            if (Math.Abs(k - 30) < 0.001) { if (d > 1600) return 0.070; if (d > 1250) return 0.065; if (d > 1000) return 0.060; if (d > 800) return 0.055; if (d > 630) return 0.050; if (d > 500) return 0.045; if (d > 400) return 0.040; if (d > 315) return 0.035; if (d > 250) return 0.035; if (d > 180) return 0.030; if (d > 120) return 0.025; if (d > 80) return 0.022; if (d > 50) return 0.019; if (d > 30) return 0.016; return 0.013; }
            return 0;
        }

        // Godstjocklekstolerans, undre (-). Kona 12 resp. 30. Övrigt => 0 ("Fel Kona").
        private static double GTolNeg(double d, double k)
        {
            if (Math.Abs(k - 12) < 0.001) { if (d > 1250) return 0.310; if (d > 1000) return 0.280; if (d > 800) return 0.250; if (d > 630) return 0.225; if (d > 500) return 0.200; if (d > 400) return 0.190; if (d > 315) return 0.175; if (d > 250) return 0.160; if (d > 180) return 0.140; if (d > 120) return 0.120; if (d > 80) return 0.105; if (d > 50) return 0.090; if (d > 30) return 0.075; return 0.070; }
            if (Math.Abs(k - 30) < 0.001) { if (d > 1250) return 0.195; if (d > 1000) return 0.170; if (d > 800) return 0.155; if (d > 630) return 0.140; if (d > 500) return 0.125; if (d > 400) return 0.115; if (d > 315) return 0.105; if (d > 251) return 0.095; if (d > 180) return 0.085; if (d > 120) return 0.075; if (d > 80) return 0.065; if (d > 50) return 0.055; if (d > 30) return 0.046; return 0.039; }
            return 0;
        }

        // Rakhet (A) & (B) — mikrometer / 1000 = mm. Trösklar X.1 enligt originalet.
        private static double RakA(double d) { if (d < 100.1) return 0.008; if (d < 280.1) return 0.010; if (d < 480.1) return 0.012; if (d < 600.1) return 0.014; if (d < 900.1) return 0.016; if (d < 1250.1) return 0.020; if (d < 1600.1) return 0.025; return 0.030; }
        private static double RakB(double d) { if (d < 100.1) return 0.012; if (d < 280.1) return 0.015; if (d < 480.1) return 0.018; if (d < 600.1) return 0.021; if (d < 900.1) return 0.024; if (d < 1250.1) return 0.030; if (d < 1600.1) return 0.037; return 0.045; }

        // Orundhet (Rd) — returneras som text (obs: "0,060" har komma i originalet, bevarat).
        private static string OrundhetStr(double d1)
        {
            if (d1 < 30.01) return "0.042"; if (d1 < 50.01) return "0.050"; if (d1 < 80.01) return "0,060";
            if (d1 < 120.01) return "0.070"; if (d1 < 180.01) return "0.080"; if (d1 < 250.01) return "0.092";
            if (d1 < 315.01) return "0.105"; if (d1 < 400.01) return "0.115"; if (d1 < 500.01) return "0.125";
            if (d1 < 630.01) return "0.140"; if (d1 < 800.01) return "0.160"; if (d1 < 1000.01) return "0.180";
            if (d1 < 1250) return "0.210"; if (d1 < 1600.01) return "0.250"; return "";
        }

        // Godstjockleksvariation (GV) — trösklar på d (>).
        private static string GVarStr(double d)
        {
            if (d > 1250) return "0.055"; if (d > 1000) return "0.050"; if (d > 800) return "0.045";
            if (d > 630) return "0.040"; if (d > 500) return "0.035"; if (d > 315) return "0.030";
            if (d > 250) return "0.025"; if (d > 180) return "0.020"; if (d > 120) return "0.015";
            if (d > 50) return "0.010"; return "0.008";
        }

        // Vinkeltoleransfaktor — trösklar X.1 enligt originalet (t.ex. d<50,1 => 0,6).
        private static double VTList(double d)
        {
            if (d < 50.1) return 0.6; if (d < 80.1) return 0.5; if (d < 120.1) return 0.45;
            if (d < 150.1) return 0.3; if (d < 180.1) return 0.18; if (d < 400.1) return 0.15;
            if (d < 500.1) return 0.13; if (d < 630.1) return 0.12; if (d < 800.1) return 0.11;
            if (d < 1000.1) return 0.10; if (d < 1250.1) return 0.09; if (d < 1600.1) return 0.08;
            return 0.07;
        }

        // ===================== HJÄLPFUNKTIONER =====================

        private static bool Contains(string haystack, string needle)
        { return (haystack ?? "").IndexOf(needle, StringComparison.OrdinalIgnoreCase) >= 0; }

        private static double GetTabVal(double[] tab, int oneBasedIdx)
        {
            if (tab == null || oneBasedIdx < 1 || oneBasedIdx > tab.Length) return 0;
            return tab[oneBasedIdx - 1];
        }

        private static int GetMember(string val, string[] list)
        { for (int i = 0; i < list.Length; i++) if (string.Equals(list[i], val, StringComparison.OrdinalIgnoreCase)) return i + 1; return 0; }

        private static bool IsInGroup(string mv, string[] g)
        { if (string.IsNullOrEmpty(mv)) return false; for (int i = 0; i < g.Length; i++) if (string.Equals(g[i], mv, StringComparison.OrdinalIgnoreCase)) return true; return false; }

        private static bool EqualsI(string a, string b) => string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);
        private static int TryParseInt(string s) { int v; return int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0; }
        private static double TryParseDouble(string s) { if (string.IsNullOrWhiteSpace(s)) return 0; double v; return double.TryParse(s.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0; }
        private static double GetDouble(List<Bookmark> bm, string key) { string r = GetString(bm, key); if (string.IsNullOrWhiteSpace(r)) return 0; double v; return double.TryParse(r.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0; }

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
        private static string Fmt2(double v) => v.ToString("F2", CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt3(double v) => v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt4(double v) => v.ToString("F4", CommonFunctions.Culture).Replace(",", ".");
    }
}