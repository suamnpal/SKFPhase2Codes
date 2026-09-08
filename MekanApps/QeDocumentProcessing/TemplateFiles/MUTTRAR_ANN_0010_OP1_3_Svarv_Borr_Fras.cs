using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class MUTTRAR_ANN_0010_OP1_3_Svarv_Borr_Fras : ITemplateCalculations
    {
        private static readonly string[] Machines = new[] { "LB45", "MaxMuller/K&T", "VTR-160", "MacTurn 550" };

        private static readonly string[] TypList = new[]
        {
            "44","48","52","56","60","64","68","72","76","80","84","88","92","96","500","530","560",
            "600","630","670","710","750","800","850","900","950","1000","1060"
        };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");

            bool tmpSlash = tmpFormat.IndexOf('/') >= 0;
            bool tmpV21 = tmpFormat.IndexOf("V21", StringComparison.OrdinalIgnoreCase) >= 0;

            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : string.Empty;
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : string.Empty;

            string tmpANNTyp = tmpBet2.Length >= 2 ? tmpBet2.Substring(tmpBet2.Length - 2) : tmpBet2;
            string tmpOmvSerie = string.Equals(tmpANNTyp, "10", StringComparison.OrdinalIgnoreCase) ? "30" : "";
            string tmpOmvTyp = string.Equals(tmpANNTyp, "10", StringComparison.OrdinalIgnoreCase) ? "88" : "";

            string tmpSerieStr = tmpOmvSerie;
            string tmpTypStr = tmpOmvTyp;
            int serieInt = TryParseInt(tmpSerieStr);
            double typNum = TryParseDouble(tmpTypStr);

            int typLista = GetMember(tmpTypStr, TypList);

            double tmpKd = (!tmpSlash || tmpV21) ? Math.Truncate((typNum / 2.0) * 10.0) : TryParseDouble(tmpBet3);
            int tmpP = tmpKd < 301 ? 4 : tmpKd < 501 ? 5 : tmpKd < 701 ? 6 : tmpKd < 901 ? 7 : 8;

            kv["SumP"] = "(P) " + tmpP.ToString(CultureInfo.InvariantCulture);
            kv["SumGänga"] = "Tr " + Fmt(tmpKd) + "x" + tmpP.ToString(CultureInfo.InvariantCulture);
            kv["SumGMall"] = "Tr x " + tmpP.ToString(CultureInfo.InvariantCulture);

            double tmpKda = (tmpP == 4 || tmpP == 5) ? tmpKd + 0.5 : tmpKd + 1.0;
            kv["Sumkd"] = "(Kd) " + Fmt(tmpKda).Replace(".", ",");
            kv["SumkdTol"] = "± " + (GenTol(tmpKda));

            double tmpd2 = DM(tmpKd, tmpP);
            kv["Sumd2"] = "(d2) " + Fmt(tmpd2).Replace(".", ",");
            kv["Sumd2Tol"] = "+ " + (D2Tol(tmpKd));
            kv["Sumd2TolN"] = "+ " + (0.1);

            double tmpd1 = D1Val(tmpKd, tmpP);
            kv["Sumd1"] = "(d1) " + Fmt(tmpd1).Replace(".", ",");
            kv["Sumd1Tol"] = "+ " + (D1Tol(tmpKd));
            kv["Sumd1TolN"] = "+ " + (0.1);

            double tmpd = serieInt == 30 ? D_30(tmpKd) : D_31(tmpKd);
            bool isMaxMuller = EqualsI(maskinVal, "MaxMuller/K&T");
            double tmpdTolN = isMaxMuller ? H11Tol(tmpd) : H13Tol(tmpd);
            kv["Sumd"] = "(d) " + Fmt(tmpd).Replace(".", ",");
            kv["SumdTol"] = "+ " + (0);
            kv["SumdTolN"] = "- " + (tmpdTolN);

            double tmpd5 = serieInt == 30 ? D5_30(tmpKd, tmpd) : D5_31(tmpKd, tmpd);
            kv["Sumd5"] = "(d5) " + Fmt(tmpd5).Replace(".", ",");
            kv["Sumd5Tol"] = "+ " + (0);
            kv["Sumd5TolN"] = "- " + (D5TolN(tmpd5));

            double tmpd3 = tmpKd < 501 ? tmpKd + 2.0 : tmpKd + 3.0;
            kv["Sumd3"] = "(d3) " + Fmt(tmpd3).Replace(".", ",");
            kv["Sumd3Tol"] = "+ " + (D3Tol(tmpd3));
            kv["Sumd3TolN"] = "- " + (0);

            double tmpBval = serieInt == 30 ? B_30(tmpKd) : B_31(tmpKd);
            kv["SumB"] = "(B) " + Fmt(tmpBval).Replace(".", ",");
            kv["SumBTol"] = "+ " + (0);
            kv["SumBTolN"] = "- " + (BTolN(tmpBval));

            kv["SumRadie"] = "R " + (RadieVal(tmpKd));

            kv["SumFas"] = "30º";
            kv["SumGF"] = "30º";
            kv["SumIF1"] = "45º";
            kv["SumIF2"] = "45º";

            kv["SumKa"] = KaVal(tmpKd);
            kv["SumRa"] = "3.2";

            double tmpBredd = serieInt == 30 ? B_30(tmpKd) : Bredd_31(tmpKd);
            double tmpB2val = tmpBredd / 2.0;
            bool showB2 = serieInt == 30 ? tmpKd >= 421 : tmpKd >= 321;
            kv["SumB2"] = "(B/2) " + (showB2 ? Fmt(tmpB2val) : "");

            double tmpt = serieInt == 30 ? T_30(tmpKd) : T_31(tmpKd);
            kv["Sumt"] = "(t) " + Fmt(tmpt);
            kv["SumtTol"] = "+ " + Fmt1(TTol(tmpt));
            kv["SumtTolN"] = "- " + Fmt0(0);

            double tmpS = serieInt == 30 ? S_30(tmpKd) : S_31(tmpKd);
            kv["SumS"] = "(S) " + Fmt(tmpS);
            kv["SumSTol"] = "± " + Fmt3(STol(tmpS));

            double tmpd4 = D4Val(serieInt, tmpKd, tmpt);
            kv["Sumd4"] = "(d4) " + Fmt(tmpd4);
            kv["Sumd4Tol"] = "± " + Fmt3(GenTol(tmpd4));

            double tmpG = 12;
            kv["SumG"] = "(G) M" + Fmt(tmpG) + " (x3)";

            double tmpU = serieInt == 30 ? G2_30(tmpKd) : G2_31(tmpKd);
            kv["Sumu"] = "(u) M" + Fmt(tmpU) + " (x8)";

            double tmpV = serieInt == 30 ? V_30(tmpU, tmpKd) : V_31(tmpU, tmpKd);
            kv["Sumv"] = "(v) " + Fmt(tmpV);

            kv["SumGV"] = "45º";
            kv["SumRHT"] = "R1.6 ± 0.4 (2x)";

            kv["SumL3"] = "(L3) " + Fmt(17.0);
            kv["SumL3Tol"] = "+ " + (2);
            kv["SumL3TolN"] = "- " + Fmt0(0);
            kv["SumLd3"] = "max " + Fmt(23.5);
            kv["SumG3"] = "(G3) M10";

            bool showGTolk = serieInt == 30 ? tmpKd >= 421 : tmpKd >= 321;
            string tmpGTolk = showGTolk
                ? (serieInt == 30 ? "Gängtolk: M" : "Gängtolk  M") + Fmt(tmpG) + " min/max"
                : "";
            string tmpuTolk = "Gängtolk: M" + Fmt(tmpU) + " min/max";
            kv["Sumu2"] = "M" + Fmt(tmpU);
            kv["SumGTolk"] = tmpGTolk;
            kv["SumuTolk"] = tmpuTolk;

            kv["SumStämpling"] = "Stämplas enl. ritning 7430189";

            SumMaskinVal(kv, maskinVal);
            SumFrequencies(kv, maskinVal);
            SumMeasuringDevices(kv, maskinVal, tmpuTolk, tmpGTolk);
            SumAF(kv, maskinVal);

            kv["SumTextS1"] = "Grader, frifläckar, slagmärken, repor, valkar & andra defekter samt ojämnheter kontrolleras med ögonmått, för övriga mått används skjutmått.";
            kv["SumTextS2"] = "Övriga mått kontrolleras vid inställning";

            string sumRit = (serieInt == 30 ? "223015" : "223016") + " & " + subject;
            kv["SumRitningsnr"] = sumRit;
            kv["SumRitningsnr2"] = sumRit;
            kv["SumTolRit"] = "1432008";
            kv["SumTolRit2"] = "1432008";
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
                       " dagar)\n\nInformation om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument\n\nPopupruta aktiv till " +
                       validTill.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return "";
        }

        private static void SumMaskinVal(Dictionary<string, string> kv, string maskinVal)
        {
            string s1 = MapS1(maskinVal);
            string s2 = MapS2(maskinVal);
            kv["SumMaskinValS1"] = ("Maskin: " + s1 + " - OP1 & 2").Trim();
            kv["SumMaskinValS2"] = ("Maskin: " + s2 + " - OP3").Trim();
        }

        private static string MapS1(string mv)
        {
            if (EqualsI(mv, "LB45")) return "LB45";
            if (EqualsI(mv, "MaxMuller/K&T")) return "MaxMuller";
            if (EqualsI(mv, "VTR-160")) return "VTR-160";
            if (EqualsI(mv, "MacTurn 550")) return "MacTurn 550";
            return "";
        }

        private static string MapS2(string mv)
        {
            if (EqualsI(mv, "LB45")) return "LB45";
            if (EqualsI(mv, "MaxMuller/K&T")) return "K&T";
            if (EqualsI(mv, "VTR-160")) return "VTR-160";
            if (EqualsI(mv, "MacTurn 550")) return "MacTurn 550";
            return "";
        }

        private static void SumFrequencies(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsMachine(maskinVal);
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
        }

        private static void SumMeasuringDevices(Dictionary<string, string> kv, string maskinVal, string uTolk, string gTolk)
        {
            bool m = IsMachine(maskinVal);
            kv["SumD1_1"] = m ? "Multimar" : "";
            kv["SumD1_2"] = m ? "Mikrometer" : "";
            kv["SumD1_3"] = m ? "Skjutmått" : "";
            kv["SumD1_4"] = m ? "Skjutmått" : "";
            kv["SumD1_5"] = m ? "Skjutmått" : "";
            kv["SumD1_6"] = m ? "Skjutmått" : "";
            kv["SumD1_7"] = m ? "Ytjämnhetsmätare" : "";
            kv["SumD1_8"] = m ? "Egglinjal" : "";
            kv["SumD2_1"] = m ? "Skjutmått" : "";
            kv["SumD2_2"] = m ? "Djupmått" : "";
            kv["SumD2_3"] = m ? uTolk : "";
            kv["SumD2_4"] = m ? gTolk : "";
        }

        private static void SumAF(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsMachine(maskinVal);
            bool isLB = EqualsI(maskinVal, "LB45");
            bool isMax = EqualsI(maskinVal, "MaxMuller/K&T");
            kv["SumAF1_1"] = m ? "Kontrolleras med passbitsklove utf. 1" : "";
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = isLB ? "Tol h13" : isMax ? "Tol h11" : "";
            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = "";
            kv["SumAF1_6"] = m ? "Bearbetning i OP1 + 3mm" : "";
            kv["SumAF1_7"] = m ? "Övriga Ra värden 6,3" : "";
            kv["SumAF1_8"] = m ? "Vid misstänkt formfel lämnas till mätrum" : "";
            kv["SumAF2_1"] = "";
            kv["SumAF2_2"] = "";
            kv["SumAF2_3"] = "";
            kv["SumAF2_4"] = "";
        }

        private static bool IsMachine(string mv)
        {
            if (string.IsNullOrEmpty(mv)) return false;
            for (int i = 0; i < Machines.Length; i++)
                if (string.Equals(Machines[i], mv, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        private static int GetMember(string val, string[] list)
        {
            for (int i = 0; i < list.Length; i++)
                if (string.Equals(list[i], val, StringComparison.OrdinalIgnoreCase)) return i + 1;
            return 0;
        }

        private static double DM(double kd, int p)
        {
            if (p == 4) return kd - 2.0;
            if (p == 5) return kd - 2.5;
            if (p == 6) return kd - 3.0;
            if (p == 7) return kd - 3.5;
            return kd - 4.0;
        }

        private static double D2Tol(double kd)
        {
            if (kd < 301) return 0.475;
            if (kd < 501) return 0.530;
            if (kd < 701) return 0.600;
            if (kd < 901) return 0.630;
            return 0.710;
        }

        private static double D1Val(double kd, int p)
        {
            if (p == 4) return kd - 4.0;
            if (p == 5) return kd - 5.0;
            if (p == 6) return kd - 6.0;
            if (p == 7) return kd - 7.0;
            return kd - 8.0;
        }

        private static double D1Tol(double kd)
        {
            if (kd < 301) return 0.375;
            if (kd < 501) return 0.450;
            if (kd < 701) return 0.500;
            if (kd < 901) return 0.560;
            return 0.630;
        }

        private static double D_30(double kd)
        {
            if (kd < 221) return kd + 40;
            if (kd < 281) return kd + 50;
            if (kd < 361) return kd + 60;
            if (kd < 421) return kd + 70;
            if (kd < 501) return kd + 80;
            if (Math.Abs(kd - 560) < 0.001) return kd + 90;
            if (kd < 631) return kd + 100;
            if (kd < 671) return kd + 110;
            if (kd < 801) return kd + 120;
            if (kd < 951) return kd + 130;
            return kd + 140;
        }

        private static double D_31(double kd)
        {
            if (Math.Abs(kd - 350) < 0.001) return kd + 90;
            if (kd < 241) return kd + 60;
            if (kd < 281) return kd + 70;
            if (kd < 321) return kd + 80;
            if (kd < 361) return kd + 100;
            if (kd < 381) return kd + 110;
            if (kd < 461) return kd + 120;
            if (kd < 481) return kd + 140;
            if (kd < 501) return kd + 130;
            if (kd < 531) return kd + 140;
            if (kd < 601) return kd + 150;
            if (kd < 631) return kd + 170;
            if (kd < 671) return kd + 180;
            if (kd < 711) return kd + 190;
            if (kd < 801) return kd + 200;
            if (kd < 851) return kd + 210;
            if (kd < 951) return kd + 220;
            return kd + 240;
        }

        private static double D5_30(double kd, double d)
        {
            if (kd < 221) return d - 18;
            if (kd < 281) return d - 20;
            if (kd < 341) return d - 24;
            if (kd < 361) return d - 26;
            if (kd < 421) return d - 28;
            if (kd < 501) return d - 30;
            if (kd < 671) return d - 40;
            if (kd < 801) return d - 50;
            return d - 55;
        }

        private static double D5_31(double kd, double d)
        {
            if (kd < 281) return d - 30;
            if (kd < 361) return d - 40;
            if (kd < 381) return d - 50;
            if (kd < 401) return d - 60;
            if (kd < 441) return d - 50;
            if (kd < 461) return d - 40;
            if (kd < 481) return d - 60;
            if (kd < 501) return d - 50;
            if (kd < 601) return d - 60;
            if (kd < 631) return d - 70;
            if (kd < 801) return d - 75;
            if (kd < 851) return d - 85;
            if (kd < 951) return d - 90;
            if (kd < 1001) return d - 100;
            return d - 90;
        }

        private static double D5TolN(double d5)
        {
            if (d5 < 19) return 0.27;
            if (d5 < 31) return 0.33;
            if (d5 < 51) return 0.39;
            if (d5 < 81) return 0.46;
            if (d5 < 121) return 0.54;
            if (d5 < 181) return 0.63;
            if (d5 < 251) return 0.72;
            if (d5 < 316) return 0.81;
            if (d5 < 401) return 0.89;
            if (d5 < 501) return 0.97;
            if (d5 < 631) return 1.10;
            if (d5 < 801) return 1.25;
            if (d5 < 1000) return 1.40;
            return 1.65;
        }

        private static double D3Tol(double d3)
        {
            if (d3 < 19) return 0.43;
            if (d3 < 31) return 0.52;
            if (d3 < 51) return 0.62;
            if (d3 < 81) return 0.74;
            if (d3 < 121) return 0.87;
            if (d3 < 181) return 1.00;
            if (d3 < 251) return 1.15;
            if (d3 < 316) return 1.30;
            if (d3 < 401) return 1.40;
            if (d3 < 501) return 1.55;
            if (d3 < 631) return 1.75;
            if (d3 < 801) return 2.00;
            if (d3 < 1000) return 2.30;
            return 2.60;
        }

        private static double B_30(double kd)
        {
            if (kd < 221) return 30;
            if (kd < 261) return 34;
            if (kd < 281) return 38;
            if (kd < 321) return 42;
            if (kd < 361) return 45;
            if (kd < 381) return 48;
            if (kd < 421) return 52;
            if (kd < 481) return 60;
            if (kd < 531) return 68;
            if (kd < 631) return 75;
            if (kd < 671) return 80;
            if (kd < 851) return 90;
            if (kd < 1181) return 100;
            return 110;
        }

        private static double B_31(double kd)
        {
            if (kd < 221) return 32;
            if (kd < 241) return 34;
            if (kd < 261) return 36;
            if (kd < 281) return 38;
            if (kd < 301) return 40;
            if (kd < 321) return 42;
            if (kd < 351) return 55;
            if (kd < 361) return 58;
            if (kd < 381) return 60;
            if (kd < 401) return 62;
            if (kd < 441) return 70;
            if (kd < 481) return 75;
            if (kd < 531) return 80;
            if (kd < 601) return 85;
            if (kd < 631) return 95;
            if (kd < 711) return 106;
            if (kd < 801) return 112;
            if (kd < 851) return 118;
            return 125;
        }

        private static double Bredd_31(double kd)
        {
            if (kd < 221) return 32;
            if (kd < 241) return 34;
            if (kd < 261) return 36;
            if (kd < 281) return 38;
            if (kd < 301) return 40;
            if (kd < 321) return 42;
            if (kd < 341) return 55;
            if (kd < 361) return 58;
            if (kd < 381) return 60;
            if (kd < 401) return 62;
            if (kd < 441) return 70;
            if (kd < 481) return 75;
            if (kd < 531) return 80;
            if (kd < 601) return 85;
            if (kd < 631) return 95;
            if (kd < 711) return 106;
            if (kd < 801) return 112;
            if (kd < 851) return 118;
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

        private static double RadieVal(double kd)
        {
            if (kd < 221) return 2.5;
            if (kd < 281) return 3.0;
            if (kd < 441) return 3.5;
            if (kd < 601) return 4.0;
            if (kd < 711) return 5.0;
            return 6.0;
        }

        private static string KaVal(double kd)
        {
            if (kd < 51) return "0.04";
            if (kd < 121) return "0.05";
            if (kd < 251) return "0.06";
            if (kd < 316) return "0.07";
            if (kd < 401) return "0.08";
            if (kd < 501) return "0.09";
            if (kd < 631) return "0.10";
            if (kd < 801) return "0.12";
            if (kd < 1001) return "0.14";
            return "0.16";
        }

        private static double T_30(double kd)
        {
            if (kd < 221) return 9;
            if (kd < 281) return 10;
            if (kd < 341) return 12;
            if (kd < 361) return 13;
            if (kd < 421) return 14;
            if (kd < 501) return 15;
            if (kd < 671) return 20;
            return 25;
        }

        private static double T_31(double kd)
        {
            if (kd < 241) return 10;
            if (kd < 321) return 12;
            if (kd < 361) return 15;
            if (kd < 421) return 18;
            if (kd < 481) return 20;
            if (kd < 531) return 23;
            if (kd < 601) return 25;
            if (kd < 671) return 28;
            if (kd < 711) return 30;
            if (kd < 801) return 34;
            return 38;
        }

        private static double TTol(double t)
        {
            if (t < 11) return 1.5;
            if (t < 19) return 1.8;
            if (t < 31) return 2.1;
            return 2.5;
        }

        private static double S_30(double kd)
        {
            if (kd < 261) return 20;
            if (kd < 341) return 24;
            if (kd < 401) return 28;
            if (kd < 461) return 32;
            if (kd < 501) return 36;
            if (kd < 601) return 40;
            if (kd < 671) return 45;
            if (kd < 711) return 50;
            if (kd < 801) return 55;
            return 60;
        }

        private static double S_31(double kd)
        {
            if (kd < 261) return 20;
            if (kd < 321) return 24;
            if (kd < 361) return 28;
            if (kd < 421) return 32;
            if (kd < 481) return 36;
            if (kd < 531) return 40;
            if (kd < 601) return 45;
            if (kd < 671) return 50;
            if (kd < 711) return 55;
            if (kd < 801) return 60;
            return 70;
        }

        private static double STol(double s)
        {
            if (s < 31) return 0.26;
            if (s < 51) return 0.31;
            return 0.37;
        }

        private static double D4Val(int serie, double kd, double t)
        {
            if (serie == 30)
            {
                if (Math.Abs(t - 9) < 0.001) return kd + 9;
                if (Math.Abs(t - 10) < 0.001) return kd + 13;
                if (Math.Abs(t - 12) < 0.001) return kd + 16;
                if (Math.Abs(t - 13) < 0.001) return kd + 15;
                if (Math.Abs(t - 14) < 0.001) return kd + 19;
                if (Math.Abs(t - 15) < 0.001) return kd + 23;
                if (Math.Abs(t - 20) < 0.001)
                {
                    if (kd < 531) return kd + 28;
                    if (kd < 561) return kd + 23;
                    if (kd < 631) return kd + 28;
                    return kd + 33;
                }
                if (kd < 801) return kd + 32;
                if (kd < 901) return kd + 37;
                if (kd < 951) return kd + 35;
                return kd + 40;
            }
            if (Math.Abs(t - 10) < 0.001) return kd + 18;
            if (Math.Abs(t - 12) < 0.001) return kd < 281 ? kd + 21 : kd + 26;
            if (Math.Abs(t - 15) < 0.001) return kd + 33;
            if (Math.Abs(t - 18) < 0.001) return kd < 381 ? kd + 35 : kd + 40;
            if (Math.Abs(t - 20) < 0.001) return kd < 461 ? kd + 38 : kd + 48;
            if (Math.Abs(t - 23) < 0.001) return kd < 501 ? kd + 40 : kd + 45;
            if (Math.Abs(t - 25) < 0.001) return kd + 48;
            if (Math.Abs(t - 28) < 0.001) return kd < 631 ? kd + 55 : kd + 60;
            if (Math.Abs(t - 30) < 0.001) return kd + 62;
            if (Math.Abs(t - 34) < 0.001) return kd + 63;
            if (kd < 851) return kd + 64;
            if (kd < 901) return kd + 69;
            if (kd < 951) return kd + 67;
            return kd + 77;
        }

        private static double G2_30(double kd)
        {
            if (kd < 221) return 6;
            if (kd < 361) return 8;
            if (kd < 421) return 10;
            if (kd < 501) return 12;
            if (kd < 801) return 16;
            return 20;
        }

        private static double G2_31(double kd)
        {
            if (kd < 241) return 8;
            if (kd < 321) return 10;
            if (kd < 381) return 12;
            if (kd < 501) return 16;
            if (kd < 671) return 20;
            return 24;
        }

        private static double V_30(double u, double kd)
        {
            if (Math.Abs(u - 6) < 0.001) return 12;
            if (Math.Abs(u - 8) < 0.001) return kd < 301 ? 17 : 16;
            if (Math.Abs(u - 10) < 0.001) return 20;
            if (Math.Abs(u - 12) < 0.001) return 23;
            if (Math.Abs(u - 16) < 0.001) return 26;
            return 37;
        }

        private static double V_31(double u, double kd)
        {
            if (Math.Abs(u - 8) < 0.001) return 17;
            if (Math.Abs(u - 10) < 0.001) return kd < 301 ? 21 : 20;
            if (Math.Abs(u - 12) < 0.001) return 23;
            if (Math.Abs(u - 16) < 0.001) return 28;
            if (Math.Abs(u - 20) < 0.001) return 37;
            return 47;
        }

        private static double GenTol(double v)
        {
            if (v < 6.01) return 0.1;
            if (v < 30.01) return 0.2;
            if (v < 120.01) return 0.3;
            if (v < 400.01) return 0.5;
            if (v < 1000.01) return 0.8;
            if (v < 2000.01) return 1.2;
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

        private static string Fmt(double v) => v.ToString(CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt0(double v) => v.ToString("F0", CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt1(double v) => v.ToString("F1", CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt3(double v) => v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
    }
}