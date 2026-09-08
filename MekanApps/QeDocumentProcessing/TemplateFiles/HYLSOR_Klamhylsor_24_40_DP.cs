using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{

    public class HYLSOR_Klamhylsor_24_40_DP : ITemplateCalculations
    {
        private static readonly string[] ArtLista = { "H", "HA", "HE", "MA", "MS", "SNW" };
        private static readonly string[] MachinesCell = { "Cell 1", "Cell 2" };

        private const string CellLineBreak = "<<LineBreak>>";
        private const int TmpDagar = 14;

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;
            List<Bookmark> bm = req != null ? req.Bookmarks : null;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");

            bool tmpSlash = Contains(tmpBet, "/");
            bool tmpEV21 = Contains(tmpBet, "E/V21");

            string[] tokens = tmpBet.Split(new char[] { 'X', ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : "";
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : "";

            int tmpArtLista = GetMember(tmpBet1, ArtLista);
            int tmpCountB2 = tmpBet2.Length;
            string tmpSerie = (tmpSlash && (tmpCountB2 == 3 || tmpCountB2 == 2)) ? tmpBet2
                            : tmpCountB2 > 4 ? Left(tmpBet2, 3)
                            : tmpCountB2 == 3 ? Left(tmpBet2, 1)
                            : Left(tmpBet2, 2);
            string tmpTyp = Right(tmpBet2, 2);
            bool mm = tmpArtLista < 6;
            bool snw = EqualsI(tmpBet1, "SNW");

            double tmpL = GetDouble(bm, "Längd (L)");
            double tmpb = GetDouble(bm, "Gänglängd (b)");
            double tmpd = GetDouble(bm, "Ytterdiameter (d)");
            double tmpd1 = GetDouble(bm, "Innerdiameter (d1)");
            double tmpKona = GetDouble(bm, "Kona");
            if (tmpKona == 0) tmpKona = KonaDefault(tmpSerie);
            kv["Kona"] = Fmt(tmpKona);
            kv["SumKona"] = "Kona 1:" + Fmt(tmpKona);
            double tmpTudelad = GetDouble(bm, "Tudelad");
            double tmpPassbit = GetDouble(bm, "Passbit");
            string tmpKilnr = GetString(bm, "Kilnr");
            string tmpKilinst = GetString(bm, "Kilinställning");
            string tmpRitnr = GetString(bm, "Ritningsnummer");
            string tmpGSNW = GetString(bm, "GängsvarvlängdSNW (G)");
            string tmpRadieSNW = GetString(bm, "RadieSNW");
            string tmpFasSNW = GetString(bm, "FasSNW");

            double avdrag = tmpd < 155 ? 8 : tmpd < 205 ? 9 : 0;
            double tmpMLRound = Round0(tmpL - tmpb - avdrag);
            double tmpMLx = tmpMLRound > 100 ? 100 : tmpMLRound;
            double tmpML = tmpL < 135 ? tmpMLx : tmpL - tmpb - 9 - Math.Abs(tmpL - 140);

            double tmpVT = mm ? (tmpd > 180 ? 0.15 : tmpd > 150 ? 0.18 : tmpd > 120 ? 0.30 : tmpd > 80 ? 0.45 : tmpd > 50 ? 0.50 : 0.60)
                         : snw ? (tmpd > 205 ? 0.15 : tmpd > 185 ? 0.25 : tmpd > 125 ? 0.30 : tmpd > 85 ? 0.45 : 0.50)
                         : 0;

            kv["SumPassbit"] = tmpPassbit == 0 ? "" : "+passbit " + Sv(Fmt(tmpPassbit)) + "mm";

            string tmpStmmStr = tmpd < 25 ? "1,0" : tmpd < 55 ? "1,5" : tmpd < 155 ? "2,0" : tmpd < 205 ? "3,0" : tmpd < 305 ? "4,0" : "5,0";
            double tmpStmm = TryParseDouble(tmpStmmStr);
            string tmpSttumStr = tmpd < 70 ? "18" : tmpd < 150 ? "12" : tmpd < 220 ? "8" : "6";
            double tmpSttum = TryParseDouble(tmpSttumStr);
            kv["SumP"] = mm ? "(P) " + tmpStmmStr : "(P) " + tmpSttumStr;

            double tmpGbettum = tmpd < 100 ? (tmpd / 25.4) + 0.001
                              : tmpd < 150 ? (tmpd / 25.4) + 0.002
                              : tmpd < 260 ? (tmpd / 25.4) + 0.003
                              : (tmpd / 25.4) + 0.004;
            string sumGangbet = mm ? "M" + Sv(Fmt(tmpd)) : snw ? Sv(Fmt(Round3(tmpGbettum))) : "";
            string sumStigning = mm ? tmpStmmStr : tmpSttumStr + "UN";
            kv["SumGängbet"] = sumGangbet;
            kv["SumStigning"] = sumStigning;

            kv["Sumd"] = "(d) " + Sv(Fmt(tmpd));
            string tmpdTol = mm ? Fmt3((tmpStmm == 5 || tmpStmm == 4) ? 0 : tmpStmm == 3 ? 0.048 : tmpStmm == 2 ? 0.038 : tmpStmm == 1.5 ? 0.032 : 0.026)
                           : snw ? Fmt3(0) : "Fel Typ";
            string tmpdTolN = mm ? Fmt3(tmpStmm == 5 ? 0.335 : tmpStmm == 4 ? 0.300 : tmpStmm == 3 ? 0.423 : tmpStmm == 2 ? 0.318 : tmpStmm == 1.5 ? 0.268 : 0.206)
                            : snw ? Fmt3(tmpSttum == 18 ? 0.208 : tmpSttum == 12 ? 0.285 : tmpSttum == 8 ? 0.386 : 0.513) : "Fel Typ";
            kv["SumdTol"] = "- " + tmpdTol;
            kv["SumdTolN"] = "- " + tmpdTolN;

            string tmpE1_2Tol = mm ? (tmpKona == 12 ? Fmt3(GTolPos12(tmpd)) : tmpKona == 30 ? Fmt3(GTolPos30(tmpd)) : "Fel Kona")
                              : snw ? Fmt3(0.025) : "Fel Typ";
            string tmpE1_2TolN = mm ? (tmpKona == 12 ? Fmt3(GTolNeg12(tmpd)) : tmpKona == 30 ? Fmt3(GTolNeg30(tmpd)) : "Fel Kona")
                               : snw ? Fmt3(0.075) : "Fel Typ";
            kv["SumE1_2Tol"] = "+ " + tmpE1_2Tol + " [3F]";
            kv["SumE1_2TolN"] = "- " + tmpE1_2TolN + " [2F]";

            kv["Sumd1"] = "(d1) " + Sv(Fmt(tmpd1));
            string sumd1Tol = (mm ? (tmpd1 > 250 ? "± 0.065" : tmpd1 > 180 ? "± 0.057" : tmpd1 > 120 ? "± 0.050" : tmpd1 > 80 ? "± 0.043" : tmpd1 > 50 ? "± 0.037" : tmpd1 > 30 ? "± 0.031" : "± 0.026")
                             : snw ? "+ 0.102" : "Fel Typ") + " [3F]";
            kv["Sumd1Tol"] = sumd1Tol;
            kv["Sumd1TolN"] = snw ? "- 0 [3F]" : "";

            kv["SumVar"] = (mm ? (tmpd > 250 ? "0.025" : tmpd > 180 ? "0.020" : tmpd > 120 ? "0.015" : tmpd > 50 ? "0.010" : "0.008")
                          : snw ? "0.030" : "Fel Typ") + " [2F]";

            double tmpd2 = mm ? (tmpStmm == 1 ? tmpd - 0.650 : tmpStmm == 1.5 ? tmpd - 0.974 : tmpStmm == 2 ? tmpd - 1.299 : tmpStmm == 3 ? tmpd - 1.949 : tmpStmm == 4 ? tmpd - 2.000 : tmpStmm == 5 ? tmpd - 2.500 : tmpd - 3.000)
                         : snw ? (tmpSttum == 18 ? tmpd - 0.917 : tmpSttum == 12 ? tmpd - 1.374 : tmpSttum == 8 ? tmpd - 2.062 : tmpd - 2.751)
                         : 0;
            kv["Sumd2"] = "(d2) " + Sv(Fmt(tmpd2));
            string tmpd2Tol = mm ? Fmt3(tmpStmm == 5.0 ? 0.212 : tmpStmm == 4.0 ? 0.190 : tmpStmm == 3.0 ? 0.048 : tmpStmm == 2.0 ? 0.038 : tmpStmm == 1.5 ? 0.032 : 0.026)
                            : snw ? "0" : "Fel Typ";
            string tmpd2TolN = mm ? Fmt3(tmpStmm == 5.0 ? 710 : tmpStmm == 4.0 ? 0.630 : tmpStmm == 3.0 ? (tmpd > 180 ? 0.363 : 0.328) : tmpStmm == 2.0 ? (tmpd > 90 ? 0.274 : 0.262) : tmpStmm == 1.5 ? (tmpd > 45 ? 0.232 : 0.222) : 0.176)
                             : snw ? D2TolNSnw(tmpSttum, tmpd) : "Fel Typ";
            kv["Sumd2Tol"] = "- " + tmpd2Tol + " [3F]";
            kv["Sumd2TolN"] = "- " + tmpd2TolN + " [3F]";

            kv["SumGängmall"] = mm ? "M" + sumStigning : sumStigning;
            string sumGangring = sumGangbet + "x" + sumStigning;
            string sumGangtolk = sumGangbet + "x" + sumStigning;
            string sumRullar = mm ? "M" + sumStigning : sumStigning;
            kv["SumGängring"] = sumGangring;
            kv["SumGängtolk"] = sumGangtolk;
            kv["SumRullar"] = sumRullar;

            kv["SumL"] = "(L) " + Sv(Fmt(tmpL));
            kv["SumLTol"] = mm ? "+ 0" : "";
            string tmpLTolN = tmpEV21 ? Fmt(1.000)
                            : tmpTudelad == 0 ? Fmt(LTolNHel(tmpL))
                            : tmpTudelad == 1 ? Fmt(LTolNTudelad(tmpL))
                            : "Fel Tudelad";
            kv["SumLTolN"] = (mm ? "- " + tmpLTolN : snw ? "± 0.254" : "Fel Typ") + " [3F]";

            kv["Sumb"] = "(b) " + Sv(Fmt(tmpb));
            kv["SumbTol"] = (mm ? (tmpb > 120 ? "+ 4.0" : tmpb > 80 ? "+ 3.5" : tmpb > 50 ? "+ 3.0" : tmpb > 30 ? "+ 2.5" : tmpb > 18 ? "+ 2.1" : tmpb > 10 ? "+ 1.8" : "+ 1.5")
                           : snw ? "" : "Fel Typ") + (mm ? " [3F]" : "");
            kv["SumbTolN"] = mm ? " 0 [2F]" : snw ? "(min)" : "Fel Typ";

            kv["SumG"] = snw ? "Gängvarvslängd:" + CellLineBreak + "(G) " + tmpGSNW + "  ± 0.25" : "";

            kv["SumR1"] = mm ? (tmpd < 69 ? "Radie 0.5" : "Radie 1") : "Radie " + tmpRadieSNW;
            kv["SumR2"] = tmpBet == "H 2328/VZ440" ? "Radie 7.0"
                        : mm ? (tmpd < 69 ? "Radie 0.5" : tmpd < 109 ? "Radie 1" : tmpd < 159 ? "Radie 1.5" : tmpd < 219 ? "Radie 2" : "Radie 2.5")
                        : "Radie " + tmpRadieSNW;

            string tmpeStr = mm ? (tmpd < 54 ? "7" : tmpd < 79 ? "9" : tmpd < 99 ? "11" : tmpd < 119 ? "13" : tmpd < 139 ? "15" : tmpd < 159 ? "17" : tmpd < 179 ? "19" : tmpd < 210 ? "21" : "25")
                          : snw ? (tmpd < 75 ? "9,52" : tmpd < 120 ? "11,12" : tmpd < 130 ? "14,30" : tmpd < 160 ? "19,05" : tmpd < 190 ? "22,22" : "25,40")
                          : "0";
            double tmpe = TryParseDouble(tmpeStr);
            kv["Sume"] = "(e) " + tmpeStr;
            kv["SumeTol"] = mm ? (tmpe > 18 ? "+ 0.520" : tmpe > 10 ? "+ 0.430" : tmpe > 6 ? "+ 0.360" : "+ 0.300") : "";
            kv["SumeTolN"] = mm ? "- 0  [3F]" : "";
            kv["SumeTolSNW"] = snw ? "± 0.254" : "";

            bool tmpHarF = true;
            double tmpfa;
            if (tmpBet == "H 24038") tmpfa = 50;
            else if (mm) tmpfa = FaMM(tmpd);
            else if (snw) { tmpfa = FaSNW(tmpd, tmpL, out tmpHarF); }
            else { tmpfa = 0; tmpHarF = false; }

            bool isCell = IsInGroup(maskinVal, MachinesCell);
            bool isDP = EqualsI(maskinVal, "Dubbelparet");
            double tmpf = (isCell && tmpHarF) ? tmpfa - 3 : tmpfa;
            kv["Sumf"] = "(f) " + (tmpHarF ? Sv(Fmt(tmpf)) : "");
            kv["SumfTol"] = mm ? (tmpf > 50 ? "+ 3.0" : tmpf > 30 ? "+ 2.5" : tmpf > 19 ? "+ 2.1" : "+ 1.8") : "+ 2.1";
            kv["SumfTolN"] = "- 0 [3F]";

            int tmpC = tmpd1 > 95 ? 4 : 3;
            kv["SumC"] = "(C) " + tmpC.ToString(CultureInfo.InvariantCulture);
            kv["SumTold1efter"] = snw ? "" : "Tolerans efter slitsning: ";
            string tmpCeSTol = snw ? "" : Fmt3(tmpd > 201 ? 0.185 : tmpd > 131 ? 0.100 : tmpd > 91 ? 0.087 : tmpd > 56 ? 0.074 : 0.062);
            string tmpCeSTolN = snw ? "" : Fmt3(tmpd > 201 ? 0.290 : tmpd > 131 ? 0.250 : tmpd > 91 ? 0.220 : tmpd > 56 ? 0.120 : 0.100);
            kv["SumCeSTol"] = snw ? "" : "+ " + tmpCeSTol + " [3F]";
            kv["SumCeSTolN"] = snw ? "" : "- " + tmpCeSTolN + " [3F]";

            kv["SumFas"] = mm ? (tmpd < 24 ? "0.7x45°" : tmpd < 69 ? "1.1x45°" : tmpd < 159 ? "1.8x45°" : tmpd < 219 ? "2.4x45°" : "2.7x45°")
                         : snw ? tmpFasSNW + "x45°" : "Fel Typ";
            kv["SumRa"] = "Ra 2.5 [2F]";
            kv["SumRa1"] = "Ra 2.5 [2F]";

            kv["SumRd"] = Right(sumd1Tol, 10);

            string tmpKonavv = Sv(Fmt3(tmpML * tmpVT / 1000.0));
            kv["SumKonavv"] = "max " + tmpKonavv + " [2F]";
            kv["SumML"] = Sv(Fmt(tmpML));

            double tmpSA = (tmpd < 101 ? 8 : tmpd < 281 ? 10 : tmpd < 481 ? 12 : tmpd < 601 ? 14 : tmpd < 901 ? 16 : 20) / 1000.0;
            double tmpSB = (tmpd < 101 ? 12 : tmpd < 281 ? 15 : tmpd < 481 ? 18 : tmpd < 601 ? 21 : tmpd < 901 ? 24 : 30) / 1000.0;
            string sumSA = "tol.max: " + Fmt3(tmpSA);
            string sumSB = "tol.max: " + Fmt3(tmpSB);
            kv["SumSA"] = sumSA;
            kv["SumSB"] = sumSB;

            string tmpKilritn = tmpML > 100
                ? "1579442-" + tmpKilnr
                : "1509952-" + tmpKilnr + ", 7426071-" + tmpKilnr + ", 7450167-" + tmpKilnr + ", 7454645-" + tmpKilnr + ", 7427362-" + tmpKilnr;
            string tmpKonapp = tmpML > 100 ? "1579440-41" : "1509950";
            kv["Sumkil"] = tmpKilritn;
            kv["SumPB"] = tmpPassbit == 0 ? "" : "+passbit " + Sv(Fmt(tmpPassbit)) + "mm";
            kv["SumKinst"] = tmpKilinst;

            kv["SumPRit"] = tmpRitnr;
            kv["SumGRit"] = mm ? (tmpStmm < 3.5 ? "7430182:A, 239473" : "7430181:2, 237359:3") : "7431233";
            kv["SumYRit"] = "7430184:2";
            kv["SumTolRit"] = "1432012:7";

            kv["SumTxt1"] = "Kontrolleras enlingt styrplan. Vid inställning samtliga mått, Obs! kontrollera innerdiameter från båda sidor.";

            kv["SumKlEgenskaper"] = "PRODUCTION NUTS & SLEEVES & HOUSINGSALLMÄNKLASSADE EGENSKAPERKlassade egenskaper klämhylsor";

            kv["SumMaskinVal"] = "Maskin: " + maskinVal;

            SumFreq(kv, isDP, isCell);
            SumDevices(kv, isDP, isCell, tmpKonapp, tmpKilritn);
            SumAF(kv, isDP, isCell, sumRullar, sumGangtolk, sumSA, sumSB);

            return kv;
        }

        private static void SumFreq(Dictionary<string, string> kv, bool dp, bool cell)
        {
            kv["SumF_d"] = dp ? "1/10" : cell ? "1/10" : "";
            kv["SumF_d1"] = dp ? "1/10" : cell ? "1/10" : "";
            kv["SumF_dm"] = dp ? "1/10" : cell ? "1/10" : "";
            kv["SumF_Ra"] = dp ? "1/tim" : cell ? "1/tim" : "";
            kv["SumF_E1"] = dp ? "1/10" : cell ? "1/10" : "";
            kv["SumF_Rd"] = dp ? " 1/10 " : cell ? " - " : "";
            kv["SumF_A"] = dp ? " 1/Skift " : cell ? " - " : "";
            kv["SumF_B"] = dp ? " 1/Skift " : cell ? " - " : "";
            kv["SumF_Ö"] = dp ? "1/tim" : cell ? "1/tim" : "";
        }

        private static void SumDevices(Dictionary<string, string> kv, bool dp, bool cell, string konapp, string kilritn)
        {
            kv["SumD_d"] = dp ? "Skjutmått" : cell ? "Skjutmått" : "";
            kv["SumD_d1"] = dp ? "UD-Apparat" : cell ? "UD-Apparat" : "";
            kv["SumD_dm"] = dp ? "UD-Apparat" : cell ? "UD-Apparat" : "";
            kv["SumD_Ra"] = dp ? "Ytjämnhetsmätare" : cell ? "Ytjämnhetsmätare" : "";
            kv["SumD_E1"] = (dp || cell) ? "Konapp. " + konapp + CellLineBreak + "Kil " + kilritn : "";
            kv["SumD_Rd"] = (dp || cell) ? "UD-Apparat" : "";
            kv["SumD_A"] = (dp || cell) ? "Egglinjal" : "";
            kv["SumD_B"] = (dp || cell) ? "Egglinjal" : "";
            kv["SumD_Ö"] = dp ? "Skjutmått" : cell ? "Skjutmått" : "";
        }

        private static void SumAF(Dictionary<string, string> kv, bool dp, bool cell, string rullar, string gangtolk, string sa, string sb)
        {
            kv["SumAF_d"] = "";
            kv["SumAF_d1"] = dp ? "Inst. med inställningshylsa, ring eller klove" : cell ? "Inst. med inställningshylsa, ring eller klove" : "";
            kv["SumAF_dm"] = dp ? "Rullar: " + rullar + ", inställd med " + gangtolk
                           : cell ? "rullar: " + rullar + ", inställd med " + gangtolk : "";
            kv["SumAF_Ra"] = "";
            kv["SumAF_Kavv"] = (dp || cell) ? "Tolerans:" : "";
            kv["SumAF_E1_av"] = (dp || cell) ? "Tolerans:" : "";
            kv["SumAF_E1"] = (dp || cell) ? "Tolerans:" : "";
            kv["SumAF_Rd"] = (dp || cell) ? "Tolerans:" : "";
            kv["SumAF_A"] = (dp || cell) ? sa : "";
            kv["SumAF_B"] = (dp || cell) ? sb : "";
            kv["SumA_Ö"] = dp ? "Tungspår (f) mäts utsides" : cell ? "" : "";
        }

        private static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published)) return "";
            DateTime pubDt;
            if (!DateTime.TryParse(published, out pubDt)) return "";
            DateTime validTill = pubDt.AddDays(TmpDagar);
            if (DateTime.Today <= validTill.Date)
                return "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom " + TmpDagar.ToString(CultureInfo.InvariantCulture) + " dagar)\n\n" +
                       "\n\n" +
                       "Popupruta aktiv till " + validTill.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return "";
        }

        private static double KonaDefault(string serie)
        {
            if (EqualsI(serie, "240") || EqualsI(serie, "241")) return 30;
            return 12;
        }

        private static double GTolPos12(double d)
        { if (d > 250) return 0.055; if (d > 180) return 0.050; if (d > 120) return 0.040; if (d > 80) return 0.035; if (d > 50) return 0.030; if (d > 30) return 0.025; return 0.020; }

        private static double GTolPos30(double d)
        { if (d > 250) return 0.035; if (d > 180) return 0.030; if (d > 120) return 0.025; if (d > 80) return 0.022; if (d > 50) return 0.019; if (d > 30) return 0.016; return 0.013; }

        private static double GTolNeg12(double d)
        { if (d > 250) return 0.160; if (d > 180) return 0.140; if (d > 120) return 0.120; if (d > 80) return 0.105; if (d > 50) return 0.090; if (d > 30) return 0.075; return 0.070; }

        private static double GTolNeg30(double d)
        { if (d > 251) return 0.095; if (d > 180) return 0.085; if (d > 120) return 0.075; if (d > 80) return 0.065; if (d > 50) return 0.055; if (d > 30) return 0.046; return 0.039; }

        private static string D2TolNSnw(double p, double d)
        {
            if (p == 18) return Fmt3(d > 50 ? 0.130 : d > 35 ? 0.114 : 0.102);
            if (p == 12) return Fmt3(d > 122 ? 0.210 : d > 120 ? 0.170 : d > 100 ? 0.210 : d > 85 ? 0.188 : d > 75 ? 0.150 : 0.137);
            if (p == 8) return Fmt3(d > 210 ? 0.307 : d > 200 ? 0.249 : d > 197 ? 0.290 : 0.231);
            if (p == 6) return Fmt3(d > 305 ? 0.343 : d > 300 ? 0.264 : d > 240 ? 0.330 : d > 220 ? 0.315 : d > 210 ? 0.307 : d > 200 ? 0.249 : d > 197 ? 0.290 : 0.231);
            return "";
        }

        private static double LTolNHel(double L)
        {
            if (L > 400) return 2.500; if (L > 315) return 2.300; if (L > 250) return 2.100; if (L > 180) return 1.850;
            if (L > 120) return 1.600; if (L > 80) return 1.400; if (L > 50) return 1.200; if (L > 30) return 1.000;
            if (L > 18) return 0.840; if (L > 10) return 0.700; return 0.580;
        }

        private static double LTolNTudelad(double L)
        {
            if (L > 631) return 0.800; if (L > 501) return 0.700; if (L > 400) return 0.630; if (L > 315) return 0.570;
            if (L > 250) return 0.520; if (L > 180) return 0.460; if (L > 120) return 0.400; if (L > 80) return 0.350;
            if (L > 50) return 0.300; if (L > 30) return 0.250; return 0.210;
        }

        private static double FaMM(double d)
        {
            if (d < 54) return 20; if (d < 64) return 21; if (d < 69) return 22; if (d < 74) return 24;
            if (d < 79) return 25; if (d < 84) return 27; if (d < 94) return 29; if (d < 99) return 30;
            if (d < 109) return 31; if (d < 119) return 32; if (d < 129) return 34; if (d < 139) return 36;
            if (d < 149) return 37; if (d < 159) return 39; if (d < 169) return 42; if (d < 179) return 43;
            if (d < 189) return 44; if (d < 199) return 46; if (d < 219) return 47; return 51;
        }

        private static double FaSNW(double d, double L, out bool har)
        {
            har = true;
            if (d < 80) return 26.5;
            if (d < 85) return 27.5;
            if (d < 90) return 29.5;
            if (d < 100) return 33;
            if (d < 110) return 34.5;
            if (d < 120) return L < 75 ? 35.5 : 39;
            if (d < 130) return L < 83 ? 37 : 41;
            if (d < 140) return 42.4;
            if (d < 150) return L < 90 ? 40.5 : 46.1;
            if (d < 160) return 48;
            if (d < 170) return L < 103 ? 42.9 : 48.8;
            if (d < 180) return 49.8;
            if (d < 190) return 51.2;
            if (d < 200 && L < 121) return 47;
            har = false;
            return 0;
        }

        private static bool Contains(string haystack, string needle)
        { return (haystack ?? "").IndexOf(needle, StringComparison.OrdinalIgnoreCase) >= 0; }

        private static int GetMember(string val, string[] list)
        { for (int i = 0; i < list.Length; i++) if (string.Equals(list[i], val, StringComparison.OrdinalIgnoreCase)) return i + 1; return 0; }

        private static bool IsInGroup(string mv, string[] g)
        { if (string.IsNullOrEmpty(mv)) return false; for (int i = 0; i < g.Length; i++) if (string.Equals(g[i], mv, StringComparison.OrdinalIgnoreCase)) return true; return false; }

        private static string Left(string s, int n)
        { if (string.IsNullOrEmpty(s)) return ""; return s.Length <= n ? s : s.Substring(0, n); }

        private static string Right(string s, int n)
        { if (string.IsNullOrEmpty(s)) return ""; return s.Length <= n ? s : s.Substring(s.Length - n); }

        private static bool EqualsI(string a, string b) => string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);

        private static double Round0(double v) => Math.Round(v, MidpointRounding.AwayFromZero);
        private static double Round3(double v) => Math.Round(v, 3, MidpointRounding.AwayFromZero);

        private static double TryParseDouble(string s)
        { if (string.IsNullOrWhiteSpace(s)) return 0; double v; return double.TryParse(s.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0; }

        private static double GetDouble(List<Bookmark> bm, string key)
        { string r = GetString(bm, key); if (string.IsNullOrWhiteSpace(r)) return 0; double v; return double.TryParse(r.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0; }

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

        private static string Sv(string s) => (s ?? "").Replace(".", ",");
        private static string Fmt(double v) => v.ToString(CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt2(double v) => v.ToString("F2", CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt3(double v) => v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");


    }
}