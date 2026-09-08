using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class MUTTRAR_HME_30_31_44_1250_OP1_3 : ITemplateCalculations
    {
        private static readonly string[] MachinesAll = new[] { "LB45", "MaxMuller/K&T", "VTR-160", "MacTurn 550" };
        private static readonly string[] MachinesLabelN = new[] { "LB45", "EMAG", "VTR-160", "MacTurn 550" };

        private static readonly string[] Typ30 = { "44", "48", "52", "72", "76", "80", "84", "88", "96", "500", "530", "560", "600", "630", "710", "750", "800", "850", "900", "950", "1000", "1060" };
        private static readonly string[] Typ31 = { "60", "68", "72", "76", "80", "88", "92", "96", "500", "560", "600", "630", "670", "710", "750", "800", "850", "1000" };

        private static readonly double[] T30List = { 5, 8, 8, 8, 10, 10, 10, 12, 12, 12, 15, 15, 18, 18, 20, 20, 20, 20, 25, 25, 25, 25 };
        private static readonly double[] T31List = { 5, 8, 10, 15, 15, 15, 20, 20, 12, 15, 15, 18, 18, 20, 20, 20, 25, 25 };

        private static readonly double[] D5_30 = { 237, 264, 288, 394, 422, 442, 462, 488, 530, 550, 571, 610, 657, 690, 766, 820, 870, 925, 975, 1025, 1085, 1145 };
        private static readonly double[] D5_31 = { 335, 382, 406, 438, 456, 508, 535, 560, 580, 650, 690, 730, 775, 825, 875, 925, 975, 1140 };

        private static readonly double[] B2_30 = { 0, 0, 0, 0, 0, 0, 0, 30, 31, 34, 42, 37, 40, 40, 50, 46, 46, 48, 58, 58, 58, 58 };
        private static readonly double[] B2_31 = { 0, 0, 35, 39, 43, 42, 45, 49, 38, 50, 50, 55, 57, 61, 61, 61, 69, 73 };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;
            List<Bookmark> bm = req != null ? req.Bookmarks : null;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");

            bool tmpSlash = tmpFormat.IndexOf('/') >= 0;
            bool tmpV21 = tmpFormat.IndexOf("V21", StringComparison.OrdinalIgnoreCase) >= 0;

            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : string.Empty;
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : string.Empty;

            string tmpSerie = tmpBet2.Length >= 2 ? tmpBet2.Substring(0, 2) : tmpBet2;
            string tmpTypStr = tmpSlash ? tmpBet3 : MidSafe(tmpBet2, 2, 2);

            int serieInt = TryParseInt(tmpSerie);
            double typNum = TryParseDouble(tmpTypStr);

            int typLista = serieInt == 30 ? GetMember(tmpTypStr, Typ30) : GetMember(tmpTypStr, Typ31);

            double d4bm = GetDouble(bm, "Ø Nom Gänga (d4)");
            double tmpd4 = d4bm != 0 ? d4bm : (tmpSlash ? TryParseDouble(tmpBet3) : typNum / 2.0 * 10.0);
            int stmm = Stmm(tmpd4);

            kv["SumP"] = "(P) " + stmm.ToString(CultureInfo.InvariantCulture);
            kv["SumGänga"] = "Tr " + Fmt(tmpd4) + "x" + stmm.ToString(CultureInfo.InvariantCulture);
            kv["SumGMall"] = "Tr x " + stmm.ToString(CultureInfo.InvariantCulture);

            double tmpd4a = (stmm == 4 || stmm == 5) ? tmpd4 + 0.5 : tmpd4 + 1.0;
            kv["Sumd4"] = "(d4) " + Fmt(tmpd4a).Replace(".", ",");
            kv["Sumd4Tol"] = "± " + Fmt3(GenTol(tmpd4a));

            double tmpdm = DM(tmpd4, stmm);
            kv["Sumdm"] = "(dm) " + Fmt(tmpdm).Replace(".", ",");
            kv["SumdmTol"] = "+ " + Fmt3(DmTol(tmpd4));
            kv["SumdmTolN"] = "- " + Fmt1(0.0);

            double tmpD1 = D1Val(tmpd4, stmm);
            kv["Sumd1"] = "(D1) " + Fmt(tmpD1);
            kv["Sumd1Tol"] = "+ " + Fmt3(D1Tol(tmpd4));
            kv["Sumd1TolN"] = "- " + Fmt1(0.0);

            double tmpTval = typLista >= 1 ? GetList(serieInt == 30 ? T30List : T31List, typLista) : 0;
            string sumTTol = tmpTval < 6.1 ? "± 0.1" : tmpTval < 30.1 ? "± 0.2" : "± 0.3";
            kv["SumT"] = "(T) " + Fmt(tmpTval);
            kv["SumTTol"] = sumTTol;

            double d3bm = GetDouble(bm, "Ø yttre (d3)");
            double tmpd3 = d3bm != 0 ? d3bm : (serieInt == 30 ? D3_30(tmpd4) : D3_31(tmpd4));
            bool isMaxMuller = EqualsI(maskinVal, "MaxMuller/K&T");
            double tmpd3TolN = isMaxMuller ? H11Tol(tmpd3) : H13Tol(tmpd3);
            kv["Sumd3"] = "(d3) " + Fmt(tmpd3);
            kv["Sumd3Tol"] = "+ " + Fmt1(0.0);
            kv["Sumd3TolN"] = "- " + Fmt3(tmpd3TolN);

            double d5bm = GetDouble(bm, "Ø fas utv. (d5)");
            double tmpd5 = d5bm != 0 ? d5bm : (typLista >= 1 ? GetList(serieInt == 30 ? D5_30 : D5_31, typLista) : 0);
            kv["Sumd5"] = "(d5) " + Fmt(tmpd5);
            kv["Sumd5Tol"] = " 0";
            kv["Sumd5TolN"] = D5TolN(tmpd5);

            double tmpd = tmpd4 < 501 ? tmpd4 + 2.0 : tmpd4 + 3.0;
            kv["Sumd"] = "(d) " + Fmt(tmpd);
            kv["SumdTol"] = "+ " + Fmt3(H13Tol(tmpd));
            kv["SumdTolN"] = "- " + Fmt1(0.0);

            double bBm = GetDouble(bm, "Bredd (B)");
            double tmpBval = bBm != 0 ? bBm : (serieInt == 30 ? B_30(tmpd4) : B_31(tmpd4));
            kv["SumB"] = "(B) " + Fmt(tmpBval);
            kv["SumBTol"] = "+ " + Fmt1(0.0);
            kv["SumBTolN"] = "- " + Fmt3(BTolN(tmpBval));

            kv["SumRadie"] = "R " + Fmt1(RadieVal(tmpd4));

            kv["SumFas"] = "30º";
            kv["SumGF"] = "30º";
            kv["SumIF1"] = "45º";
            kv["SumIF2"] = "45º";

            kv["Sumt1"] = "(t1) " + Fmt3(T1T2Val(tmpd4));
            kv["Sumt2"] = "(t2) " + Fmt3(T1T2Val(tmpd4));
            kv["Sumt4"] = "(t4) " + Fmt3(serieInt == 30 ? T4_30(typNum) : T4_31(typNum));
            kv["Sumt5"] = "(t5) " + Fmt2(serieInt == 30 ? T5_30(typNum) : T5_31(typNum));
            kv["Sumt7"] = "(t7) " + Fmt1(serieInt == 30 ? T7_30(typNum) : T7_31(typNum));

            kv["SumRa"] = "3.2";

            double b2bm = GetDouble(bm, "Placering Öglehål (B2)");
            double tmpB2 = b2bm != 0 ? b2bm : (typLista >= 1 ? GetList(serieInt == 30 ? B2_30 : B2_31, typLista) : 0);
            kv["SumB2"] = tmpB2 == 0 ? "" : "(B2) " + Fmt(tmpB2);

            double g3bm = GetDouble(bm, "Gänga Öglehål (G3)");
            double tmpG3 = g3bm != 0 ? g3bm : (serieInt == 30 ? G3_30(tmpd4) : G3_31(tmpd4));
            bool hasG3 = tmpG3 > 0;
            kv["SumG3"] = !hasG3 ? "Ingen lyftögla" : "(G3) M" + Fmt(tmpG3);

            double l3bm = GetDouble(bm, "Gängdjup Öglehål (L3)");
            double tmpL3 = l3bm != 0 ? l3bm : L3Val(serieInt, tmpG3);
            double tmpLd3 = Ld3Val(tmpL3, tmpG3);
            kv["SumL3"] = !hasG3 ? "" : "(L3) " + Fmt(tmpL3);
            kv["SumL3Tol"] = !hasG3 ? "" : "+ " + Fmt3(2.0);
            kv["SumL3TolN"] = !hasG3 ? "" : "- " + Fmt1(0.0);
            kv["SumLd3"] = !hasG3 ? "" : "(L3) max:" + (tmpLd3 == 0 ? "" : Fmt(tmpLd3));

            double tmph = serieInt == 30 ? H_30(tmpd4) : H_31(tmpd4);
            kv["Sumh"] = "(h) " + Fmt(tmph);
            kv["SumhTol"] = "+ " + Fmt3(HTol(tmph));
            kv["SumhTolN"] = "- " + Fmt1(0.0);

            double tmpSb = serieInt == 30 ? Sb_30(tmpd4) : Sb_31(tmpd4);
            kv["SumSb"] = "(Sb) " + Fmt(tmpSb);
            kv["SumSbTol"] = "± " + Fmt3(SbTol(tmpSb));

            double tmpd2 = D2Val(serieInt, tmpd4, tmph);
            kv["Sumd2"] = "(d2 ) " + Fmt(tmpd2);
            kv["Sumd2Tol"] = "± " + Fmt3(GenTol(tmpd4a));

            double tmpG2 = serieInt == 30 ? G2_30(tmpd4) : G2_31(tmpd4);
            kv["SumG2"] = "(G2) M" + Fmt(tmpG2);

            double tmpL2 = serieInt == 30 ? L2_30(tmpG2, tmpd4) : L2_31(tmpG2, tmpd4);
            kv["SumL2"] = "(L2) " + Fmt(tmpL2);
            kv["SumL2Tol"] = "+ " + Fmt3(2.0);
            kv["SumL2TolN"] = "- " + Fmt1(0.0);

            double tmpLd2 = serieInt == 30 ? Ld2_30(tmpG2) : Ld2_31(tmpG2);
            kv["SumLd2"] = "(Ld2) max:" + Fmt(tmpLd2);

            bool showG3Ganga = serieInt == 30 ? tmpd4 >= 421 : tmpd4 >= 321;
            kv["SumGV"] = "45º";
            kv["SumRHT"] = "16x R1.6 ±0.4";
            kv["SumGG2"] = showG3Ganga ? "Gänga: M" + Fmt(tmpG3) : "";
            kv["SumGGTolk"] = showG3Ganga ? (serieInt == 30 ? "Kontrolleras med tolk  M" : " kontrolleras med tolk  M") + Fmt(tmpG3) + " min/max" : "";
            kv["SumGu2"] = "M" + Fmt(tmpG2);
            kv["SumGuTolk"] = "M" + Fmt(tmpG2) + " min/max";
            kv["SumStämpling"] = "Stämplas enl. ritning 7433462";

            SumMaskinVal(kv, maskinVal);
            SumFrequencies(kv, maskinVal);
            SumMeasuringDevices(kv, maskinVal, kv["SumGuTolk"], kv["SumGGTolk"]);
            SumAF(kv, maskinVal);

            kv["SumTextS1"] = "Grader, frifläckar, slagmärken, repor, valkar & andra defekter samt ojämnheter kontrolleras med ögonmått, för övriga mått används skjutmått.";
            kv["SumTextS2"] = "Övriga mått kontrolleras vid inställning";

            string ritBm = GetString(bm, "Ritningsnummer");
            bool ritZero = string.IsNullOrEmpty(ritBm) || EqualsI(ritBm, "0");
            string sumRit = ritZero ? "7438482:senaste utg." : ritBm;
            kv["SumRitningsnr"] = sumRit;
            kv["SumRitningsnr2"] = sumRit;
            kv["SumGTolRit"] = "7430181";
            kv["SumYtRit"] = "7430184";
            kv["SumYtRit2"] = "7430184";
            kv["SumKlEgenskaper"] = "PRODUCTION NUTS & SLEEVES & HOUSINGS\\ALLMÄNKLASSADE EGENSKAPER\\Klassade egenskaper muttrar";
            kv["SumKlEgenskaper2"] = kv["SumKlEgenskaper"];

            return kv;
        }

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
                       " dagar)<<LineBreak>>Information om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument<<LineBreak>>Popupruta aktiv till " +
                       validTill.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return "";
        }

        private static void SumMaskinVal(Dictionary<string, string> kv, string maskinVal)
        {
            string s1 = MapS1(maskinVal);
            string s2 = MapS2(maskinVal);
            bool isSvarv = IsInGroup(maskinVal, MachinesLabelN);
            kv["SumMaskinValS1"] = "Maskin: " + s1 + (isSvarv ? " - Svarvning" : " - OP1 & 2");
            kv["SumMaskinValS2"] = "Maskin: " + s2 + (isSvarv ? " - Borrning & Fräsning" : " - OP3");
        }

        private static string MapS1(string mv)
        {
            if (EqualsI(mv, "LB45")) return "LB45";
            if (EqualsI(mv, "EMAG")) return "EMAG";
            if (EqualsI(mv, "MaxMuller/K&T")) return "MaxMuller";
            if (EqualsI(mv, "VTR-160")) return "VTR-160";
            if (EqualsI(mv, "MacTurn 550")) return "MacTurn 550";
            return "";
        }

        private static string MapS2(string mv)
        {
            if (EqualsI(mv, "LB45")) return "LB45";
            if (EqualsI(mv, "EMAG")) return "EMAG";
            if (EqualsI(mv, "MaxMuller/K&T")) return "K&T";
            if (EqualsI(mv, "VTR-160")) return "VTR-160";
            if (EqualsI(mv, "MacTurn 550")) return "MacTurn 550";
            return "";
        }

        private static void SumFrequencies(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsInGroup(maskinVal, MachinesAll);
            kv["SumF1_1"] = m ? "1/1" : "";
            kv["SumF1_2"] = m ? "1/2" : "";
            kv["SumF1_3"] = m ? "1/5" : "";
            kv["SumF1_4"] = m ? "1/5" : "";
            kv["SumF1_5"] = m ? "1/5" : "";
            kv["SumF1_6"] = m ? "1/5" : "";
            kv["SumF1_7"] = m ? "1/3" : "";
            kv["SumF1_8"] = m ? "1/5" : "";
            kv["SumF2_1"] = m ? "1/5" : "";
            kv["SumF2_2"] = m ? "1/5" : "";
            kv["SumF2_3"] = m ? "1/5" : "";
            kv["SumF2_4"] = m ? "1/5" : "";
            kv["SumF2_5"] = m ? "inst." : "";
        }

        private static void SumMeasuringDevices(Dictionary<string, string> kv, string maskinVal, string guTolk, string ggTolk)
        {
            bool m = IsInGroup(maskinVal, MachinesAll);
            kv["SumD1_1"] = m ? "Multimar" : "";
            kv["SumD1_2"] = m ? "Mikrometer" : "";
            kv["SumD1_3"] = m ? "Skjutmått" : "";
            kv["SumD1_4"] = m ? "Skjutmått" : "";
            kv["SumD1_5"] = m ? "Skjutmått" : "";
            kv["SumD1_6"] = m ? "Skjutmått" : "";
            kv["SumD1_7"] = m ? "Ytjämnhetsmätare" : "";
            kv["SumD1_8"] = m ? "Planhet med Egglinjal" : "";
            kv["SumD2_1"] = m ? "Skjutmått" : "";
            kv["SumD2_2"] = m ? "Djupmått" : "";
            kv["SumD2_3"] = m ? guTolk : "";
            kv["SumD2_4"] = m ? ggTolk : "";
            kv["SumD2_5"] = m ? "Funktion, muttersäkring" : "";
        }

        private static void SumAF(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsInGroup(maskinVal, MachinesAll);
            bool isLB = EqualsI(maskinVal, "LB45");
            bool isMaxVtrMac = EqualsI(maskinVal, "MaxMuller/K&T") || EqualsI(maskinVal, "VTR-160") || EqualsI(maskinVal, "MacTurn 550");
            kv["SumAF1_1"] = m ? "Kontrolleras med passbitsklove utf. 1" : "";
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = isLB ? "Tol h13" : (isMaxVtrMac ? "Tol h11" : "");
            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = "";
            kv["SumAF1_6"] = m ? "Bearbetning i OP1 + 3mm" : "";
            kv["SumAF1_7"] = m ? "Övriga Ra värden 6,3" : "";
            kv["SumAF1_8"] = m ? "Vid misstänkt formfel lämnas till mätrum" : "";
            kv["SumAF2_1"] = "";
            kv["SumAF2_2"] = "";
            kv["SumAF2_3"] = "";
            kv["SumAF2_4"] = "";
            kv["SumAF2_5"] = m ? "Vid misstänkt lägesfel lämnas till mätrum." : "";
        }

        private static int Stmm(double d4)
        {
            if (d4 < 301) return 4;
            if (d4 < 501) return 5;
            if (d4 < 701) return 6;
            if (d4 < 901) return 7;
            return 8;
        }

        private static double DM(double d4, int s)
        {
            if (s == 4) return d4 - 2.0;
            if (s == 5) return d4 - 2.5;
            if (s == 6) return d4 - 3.0;
            if (s == 7) return d4 - 3.5;
            return d4 - 4.0;
        }

        private static double DmTol(double d4)
        {
            if (d4 < 301) return 0.475;
            if (d4 < 501) return 0.530;
            if (d4 < 701) return 0.600;
            if (d4 < 901) return 0.630;
            return 0.710;
        }

        private static double D1Val(double d4, int s)
        {
            if (s == 4) return d4 - 4.0;
            if (s == 5) return d4 - 5.0;
            if (s == 6) return d4 - 6.0;
            if (s == 7) return d4 - 7.0;
            return d4 - 8.0;
        }

        private static double D1Tol(double d4)
        {
            if (d4 < 301) return 0.375;
            if (d4 < 501) return 0.450;
            if (d4 < 701) return 0.500;
            if (d4 < 901) return 0.560;
            return 0.630;
        }

        private static double D3_30(double d4)
        {
            if (d4 < 221) return d4 + 40;
            if (d4 < 281) return d4 + 50;
            if (d4 < 361) return d4 + 60;
            if (d4 < 421) return d4 + 70;
            if (d4 < 501) return d4 + 80;
            if (Math.Abs(d4 - 560) < 0.001) return d4 + 90;
            if (d4 < 631) return d4 + 100;
            if (d4 < 671) return d4 + 110;
            if (d4 < 801) return d4 + 120;
            if (d4 < 951) return d4 + 130;
            return d4 + 140;
        }

        private static double D3_31(double d4)
        {
            if (Math.Abs(d4 - 350) < 0.001) return d4 + 90;
            if (d4 < 241) return d4 + 60;
            if (d4 < 281) return d4 + 70;
            if (d4 < 321) return d4 + 80;
            if (d4 < 361) return d4 + 100;
            if (d4 < 381) return d4 + 110;
            if (d4 < 461) return d4 + 120;
            if (d4 < 481) return d4 + 140;
            if (d4 < 501) return d4 + 130;
            if (d4 < 531) return d4 + 140;
            if (d4 < 601) return d4 + 150;
            if (d4 < 631) return d4 + 170;
            if (d4 < 671) return d4 + 180;
            if (d4 < 711) return d4 + 190;
            if (d4 < 801) return d4 + 200;
            if (d4 < 851) return d4 + 210;
            if (d4 < 951) return d4 + 220;
            return d4 + 240;
        }

        private static double B_30(double d4)
        {
            if (d4 < 221) return 30;
            if (d4 < 261) return 34;
            if (d4 < 281) return 38;
            if (d4 < 321) return 42;
            if (d4 < 361) return 45;
            if (d4 < 381) return 48;
            if (d4 < 421) return 52;
            if (d4 < 481) return 60;
            if (d4 < 531) return 68;
            if (d4 < 631) return 75;
            if (d4 < 671) return 80;
            if (d4 < 851) return 90;
            if (d4 < 1181) return 100;
            return 110;
        }

        private static double B_31(double d4)
        {
            if (d4 < 221) return 32;
            if (d4 < 241) return 34;
            if (d4 < 261) return 36;
            if (d4 < 281) return 38;
            if (d4 < 301) return 40;
            if (d4 < 321) return 42;
            if (d4 < 351) return 55;
            if (d4 < 361) return 58;
            if (d4 < 381) return 60;
            if (d4 < 401) return 62;
            if (d4 < 441) return 70;
            if (d4 < 481) return 75;
            if (d4 < 531) return 80;
            if (d4 < 601) return 85;
            if (d4 < 631) return 95;
            if (d4 < 711) return 106;
            if (d4 < 801) return 112;
            if (d4 < 851) return 118;
            return 125;
        }

        private static double BTolN(double b)
        {
            if (b < 7) return 0.18;
            if (b < 11) return 0.22;
            if (b < 19) return 0.27;
            if (b < 31) return 0.33;
            if (b < 51) return 0.39;
            if (b < 81) return 0.46;
            if (b < 121) return 0.54;
            return 0.63;
        }

        private static double RadieVal(double d4)
        {
            if (d4 < 221) return 2.5;
            if (d4 < 281) return 3.0;
            if (d4 < 441) return 3.5;
            if (d4 < 601) return 4.0;
            if (d4 < 711) return 5.0;
            return 6.0;
        }

        private static double T1T2Val(double d4)
        {
            if (d4 < 51) return 0.04;
            if (d4 < 121) return 0.05;
            if (d4 < 251) return 0.06;
            if (d4 < 316) return 0.07;
            if (d4 < 401) return 0.08;
            if (d4 < 501) return 0.09;
            if (d4 < 631) return 0.10;
            if (d4 < 801) return 0.12;
            if (d4 < 1001) return 0.14;
            return 0.16;
        }

        private static double T4_30(double typ)
        {
            if (typ < 56) return 0.52;
            if (typ < 72) return 0.57;
            if (typ < 88) return 0.63;
            if (typ < 560) return 0.70;
            if (typ < 710) return 0.80;
            if (typ < 900) return 0.90;
            if (typ < 1120) return 1.05;
            return 1.25;
        }

        private static double T4_31(double typ)
        {
            if (typ < 52) return 0.52;
            if (typ < 68) return 0.57;
            if (typ < 80) return 0.63;
            if (typ < 530) return 0.70;
            if (typ < 670) return 0.80;
            if (typ < 850) return 0.90;
            if (typ < 1060) return 1.05;
            return 1.25;
        }

        private static double T5_30(double typ) { if (typ < 84) return 0.84; if (typ < 750) return 1.00; return 1.90; }
        private static double T5_31(double typ) { if (typ < 76) return 0.84; if (typ < 710) return 1.00; return 1.90; }
        private static double T7_30(double typ) { if (typ < 84) return 1.3; if (typ < 750) return 1.6; return 1.2; }
        private static double T7_31(double typ) { if (typ < 76) return 1.3; if (typ < 710) return 1.6; return 1.2; }

        private static double H_30(double d4)
        {
            if (d4 < 221) return 9;
            if (d4 < 281) return 10;
            if (d4 < 341) return 12;
            if (d4 < 361) return 13;
            if (d4 < 421) return 14;
            if (d4 < 501) return 15;
            if (d4 < 671) return 20;
            return 25;
        }

        private static double H_31(double d4)
        {
            if (d4 < 241) return 10;
            if (d4 < 321) return 12;
            if (d4 < 361) return 15;
            if (d4 < 421) return 18;
            if (d4 < 481) return 20;
            if (d4 < 531) return 23;
            if (d4 < 601) return 25;
            if (d4 < 671) return 28;
            if (d4 < 711) return 30;
            if (d4 < 801) return 34;
            return 38;
        }

        private static double HTol(double h)
        {
            if (h < 7) return 1.2;
            if (h < 11) return 1.5;
            if (h < 19) return 1.8;
            if (h < 31) return 2.1;
            return 2.5;
        }

        private static double Sb_30(double d4)
        {
            if (d4 < 261) return 20;
            if (d4 < 341) return 24;
            if (d4 < 401) return 28;
            if (d4 < 461) return 32;
            if (d4 < 501) return 36;
            if (d4 < 601) return 40;
            if (d4 < 671) return 45;
            if (d4 < 711) return 50;
            if (d4 < 801) return 55;
            return 60;
        }

        private static double Sb_31(double d4)
        {
            if (d4 < 261) return 20;
            if (d4 < 321) return 24;
            if (d4 < 361) return 28;
            if (d4 < 421) return 32;
            if (d4 < 481) return 36;
            if (d4 < 531) return 40;
            if (d4 < 601) return 45;
            if (d4 < 671) return 50;
            if (d4 < 711) return 55;
            if (d4 < 801) return 60;
            return 70;
        }

        private static double SbTol(double sb)
        {
            if (sb < 31) return 0.26;
            if (sb < 51) return 0.31;
            return 0.37;
        }

        private static double D2Val(int serie, double d4, double h)
        {
            if (serie == 30)
            {
                if (Math.Abs(h - 9) < 0.001) return d4 + 9;
                if (Math.Abs(h - 10) < 0.001) return d4 + 13;
                if (Math.Abs(h - 12) < 0.001) return d4 + 16;
                if (Math.Abs(h - 13) < 0.001) return d4 + 15;
                if (Math.Abs(h - 14) < 0.001) return d4 + 19;
                if (Math.Abs(h - 15) < 0.001) return d4 + 23;
                if (Math.Abs(h - 20) < 0.001)
                {
                    if (d4 < 531) return d4 + 28;
                    if (d4 < 561) return d4 + 23;
                    if (d4 < 631) return d4 + 28;
                    return d4 + 33;
                }
                if (d4 < 801) return d4 + 32;
                if (d4 < 901) return d4 + 37;
                if (d4 < 951) return d4 + 35;
                return d4 + 40;
            }
            if (Math.Abs(h - 10) < 0.001) return d4 + 18;
            if (Math.Abs(h - 12) < 0.001) return d4 < 281 ? d4 + 21 : d4 + 26;
            if (Math.Abs(h - 15) < 0.001) return d4 + 33;
            if (Math.Abs(h - 18) < 0.001) return d4 < 381 ? d4 + 35 : d4 + 40;
            if (Math.Abs(h - 20) < 0.001) return d4 < 461 ? d4 + 38 : d4 + 48;
            if (Math.Abs(h - 23) < 0.001) return d4 < 501 ? d4 + 40 : d4 + 45;
            if (Math.Abs(h - 25) < 0.001) return d4 + 48;
            if (Math.Abs(h - 28) < 0.001) return d4 < 631 ? d4 + 55 : d4 + 60;
            if (Math.Abs(h - 30) < 0.001) return d4 + 62;
            if (Math.Abs(h - 34) < 0.001) return d4 + 63;
            if (d4 < 851) return d4 + 64;
            if (d4 < 901) return d4 + 69;
            if (d4 < 951) return d4 + 67;
            return d4 + 77;
        }

        private static double G2_30(double d4)
        {
            if (d4 < 221) return 6;
            if (d4 < 361) return 8;
            if (d4 < 421) return 10;
            if (d4 < 501) return 12;
            if (d4 < 801) return 16;
            return 20;
        }

        private static double G2_31(double d4)
        {
            if (d4 < 241) return 8;
            if (d4 < 321) return 10;
            if (d4 < 381) return 12;
            if (d4 < 501) return 16;
            if (d4 < 671) return 20;
            return 24;
        }

        private static double L2_30(double g2, double d4)
        {
            if (Math.Abs(g2 - 6) < 0.001) return 12;
            if (Math.Abs(g2 - 8) < 0.001) return d4 < 301 ? 17 : 16;
            if (Math.Abs(g2 - 10) < 0.001) return 20;
            if (Math.Abs(g2 - 12) < 0.001) return 23;
            if (Math.Abs(g2 - 16) < 0.001) return 26;
            return 37;
        }

        private static double L2_31(double g2, double d4)
        {
            if (Math.Abs(g2 - 8) < 0.001) return 17;
            if (Math.Abs(g2 - 10) < 0.001) return d4 < 301 ? 21 : 20;
            if (Math.Abs(g2 - 12) < 0.001) return 23;
            if (Math.Abs(g2 - 16) < 0.001) return 28;
            if (Math.Abs(g2 - 20) < 0.001) return 37;
            return 47;
        }

        private static double Ld2_30(double g2)
        {
            if (Math.Abs(g2 - 6) < 0.001) return 17;
            if (Math.Abs(g2 - 8) < 0.001) return 23;
            if (Math.Abs(g2 - 10) < 0.001) return 26.5;
            if (Math.Abs(g2 - 12) < 0.001) return 30;
            if (Math.Abs(g2 - 16) < 0.001) return 34;
            return 47;
        }

        private static double Ld2_31(double g2)
        {
            if (Math.Abs(g2 - 8) < 0.001) return 23;
            if (Math.Abs(g2 - 10) < 0.001) return 27.5;
            if (Math.Abs(g2 - 12) < 0.001) return 30;
            if (Math.Abs(g2 - 16) < 0.001) return 36;
            if (Math.Abs(g2 - 20) < 0.001) return 47;
            return 58;
        }

        private static double G3_30(double d4)
        {
            if (d4 < 421) return 0;
            if (d4 < 672) return 10;
            if (d4 < 901) return 12;
            return 16;
        }

        private static double G3_31(double d4)
        {
            if (d4 < 341) return 0;
            if (d4 < 531) return 10;
            if (d4 < 602) return 12;
            if (d4 < 752) return 16;
            if (d4 < 901) return 20;
            return 24;
        }

        private static double L3Val(int serie, double g3)
        {
            if (g3 <= 0) return 0;
            if (serie == 30)
            {
                if (Math.Abs(g3 - 10) < 0.001) return 17;
                if (Math.Abs(g3 - 12) < 0.001) return 21;
                return 27;
            }
            if (Math.Abs(g3 - 10) < 0.001) return 17;
            if (Math.Abs(g3 - 12) < 0.001) return 21;
            if (Math.Abs(g3 - 24) < 0.001) return 36;
            return 27;
        }

        private static double Ld3Val(double l3, double g3)
        {
            if (g3 <= 0) return 0;
            if (Math.Abs(l3 - 17) < 0.001) return 23.5;
            if (Math.Abs(l3 - 21) < 0.001) return 28;
            if (Math.Abs(l3 - 27) < 0.001) return 35;
            if (Math.Abs(l3 - 30) < 0.001) return 40;
            if (Math.Abs(l3 - 36) < 0.001) return 47;
            return 0;
        }

        private static string D5TolN(double d5)
        {
            if (d5 < 19) return "- 0.270";
            if (d5 < 31) return "- 0.330";
            if (d5 < 51) return "- 0.390";
            if (d5 < 81) return "- 0.460";
            if (d5 < 121) return "- 0.540";
            if (d5 < 181) return "- 0.630";
            if (d5 < 251) return "- 0.720";
            if (d5 < 316) return "- 0.810";
            if (d5 < 401) return "- 0.890";
            if (d5 < 501) return "- 0.970";
            if (d5 < 631) return "- 1.100";
            if (d5 < 801) return "- 1.250";
            if (d5 < 1000) return "- 1.400";
            return "- 1.650";
        }

        private static double GenTol(double v)
        {
            if (v < 6) return 0.1;
            if (v < 30) return 0.2;
            if (v < 120) return 0.3;
            if (v < 400) return 0.5;
            if (v < 1000) return 0.8;
            if (v < 2000) return 1.2;
            return 2.0;
        }

        private static double H11Tol(double v)
        {
            if (v < 3.01) return 0.060;
            if (v < 6.01) return 0.075;
            if (v < 10.01) return 0.090;
            if (v < 18.01) return 0.110;
            if (v < 30.01) return 0.130;
            if (v < 50.01) return 0.160;
            if (v < 80.01) return 0.190;
            if (v < 120.01) return 0.220;
            if (v < 180.01) return 0.250;
            if (v < 250.01) return 0.290;
            if (v < 315.01) return 0.320;
            if (v < 400.01) return 0.360;
            if (v < 500.01) return 0.400;
            if (v < 630.01) return 0.440;
            if (v < 800.01) return 0.500;
            if (v < 1000.01) return 0.560;
            if (v < 1250.01) return 0.660;
            if (v < 1600.01) return 0.780;
            if (v < 2000.01) return 0.920;
            if (v < 2500.01) return 1.100;
            return 1.350;
        }

        private static double H13Tol(double v)
        {
            if (v < 3.01) return 0.140;
            if (v < 6.01) return 0.180;
            if (v < 10.01) return 0.220;
            if (v < 18.01) return 0.270;
            if (v < 30.01) return 0.330;
            if (v < 50.01) return 0.390;
            if (v < 80.01) return 0.460;
            if (v < 120.01) return 0.540;
            if (v < 180.01) return 0.630;
            if (v < 250.01) return 0.720;
            if (v < 315.01) return 0.810;
            if (v < 400.01) return 0.890;
            if (v < 500.01) return 0.970;
            if (v < 630.01) return 1.100;
            if (v < 800.01) return 1.250;
            if (v < 1000.01) return 1.400;
            if (v < 1250.01) return 1.650;
            if (v < 1600.01) return 1.950;
            if (v < 2000.01) return 2.300;
            if (v < 2500.01) return 2.800;
            return 3.300;
        }

        private static double GetList(double[] list, int oneBasedIdx)
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

        private static bool IsInGroup(string mv, string[] group)
        {
            if (string.IsNullOrEmpty(mv)) return false;
            for (int i = 0; i < group.Length; i++)
                if (string.Equals(group[i], mv, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        private static bool EqualsI(string a, string b)
            => string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);

        private static string MidSafe(string s, int offset, int n)
        {
            if (string.IsNullOrEmpty(s) || s.Length <= offset) return string.Empty;
            int len = Math.Min(n, s.Length - offset);
            return s.Substring(offset, len);
        }

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
        private static string Fmt1(double v) => v.ToString("F1", CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt2(double v) => v.ToString("F2", CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt3(double v) => v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
    }
}