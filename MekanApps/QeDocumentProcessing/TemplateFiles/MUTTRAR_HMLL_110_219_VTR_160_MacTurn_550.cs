using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class MUTTRAR_HMLL_110_219_VTR_160_MacTurn_550 : ITemplateCalculations
    {
        private static readonly string[] MachinesFreq = new[] { "VTR-160", "MacTurn 550" };
        private static readonly string[] MachinesLabel = new[] { "LB45", "EMAG", "MaxMuller/K&T", "VTR-160", "MacTurn 550" };
        private static readonly string[] MachinesLabelN = new[] { "LB45", "EMAG", "VTR-160", "MacTurn 550" };
        private static readonly string[] MachinesNB = new[] { "LB45", "MaxMuller/K&T", "VTR-160", "MacTurn 550" };
        private static readonly int[] ValidTypes = new[] { 110, 166, 219 };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");

            bool tmpSlash = tmpFormat.IndexOf('/') >= 0;
            bool tmpT = tmpFormat.IndexOf('T') >= 0;
            bool tmpB60 = tmpFormat.IndexOf("B60", StringComparison.OrdinalIgnoreCase) >= 0;

            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : string.Empty;

            string tmpTypStr = tmpBet2;
            int typInt = TryParseInt(tmpTypStr);
            int typLista = GetMember(typInt, ValidTypes);

            double tmpd4 = (double)typInt / 2.0 * 10.0;
            int stmm = Stmm(tmpd4);

            double tmpd4a = (stmm == 4 || stmm == 5) ? tmpd4 + 0.5 : tmpd4 + 1.0;
            kv["Sumd4"] = "(d4) " + Fmt(tmpd4a);
            kv["Sumd4Tol"] = "± " + Fmt3(GenTol(tmpd4a));

            double tmpdm = DM(tmpd4, stmm);
            kv["Sumdm"] = "(dm) " + Fmt(tmpdm);
            kv["SumdmTol"] = "+ " + Fmt3(DmTol(tmpd4)) + " [3F]";
            kv["SumdmTolN"] = "- " + Fmt1(0.0) + " [2F]";

            double tmpd1 = D1(tmpd4, stmm);
            kv["Sumd1"] = "(d1) " + Fmt(tmpd1);
            kv["Sumd1Tol"] = "+ " + Fmt3(D1Tol(tmpd4)) + " [3F]";
            kv["Sumd1TolN"] = "- " + Fmt1(0.0) + " [2F]";

            kv["SumP"] = "(P) " + stmm.ToString(CultureInfo.InvariantCulture);
            kv["SumGänga"] = "Tr " + Fmt(tmpd4) + "x" + stmm.ToString(CultureInfo.InvariantCulture);
            kv["SumGMall"] = "Tr x " + stmm.ToString(CultureInfo.InvariantCulture);

            double tmpd3 = D3Val(typInt);
            bool useH11 = typInt > 166;
            double tmpd3TolN = useH11 ? H11Tol(tmpd3) : H13Tol(tmpd3);
            kv["Sumd3"] = "(d3) " + Fmt(tmpd3);
            kv["Sumd3Tol"] = "+ " + Fmt1(0.0) + " [3F]";
            kv["Sumd3TolN"] = "- " + Fmt3(tmpd3TolN);

            double tmpd5 = D5Val(typInt);
            double tmpd5TolN = H13Tol(tmpd5);
            kv["Sumd5"] = "(d5) " + Fmt(tmpd5);
            kv["Sumd5Tol"] = "+ " + Fmt1(0.0) + " [3F]";
            kv["Sumd5TolN"] = "- " + Fmt3(tmpd5TolN);

            double tmpd = tmpd4 < 501 ? tmpd4 + 2.0 : tmpd4 + 3.0;
            double tmpdTolN = H13Tol(tmpd);
            kv["Sumd"] = "(d) " + Fmt(tmpd);
            kv["SumdTol"] = "+ " + Fmt3(tmpdTolN) + " [3F]";
            kv["SumdTolN"] = "- " + Fmt1(0.0) + " [3F]";

            double tmpBval = tmpB60 ? 60.0 : BVal(typInt);
            double tmpBTolN = H13Tol(tmpBval);
            kv["SumB"] = "(B) " + Fmt(tmpBval);
            kv["SumBTol"] = "+ " + Fmt1(0.0);
            kv["SumBTolN"] = "- " + Fmt3(tmpBTolN) + " [3F]";

            kv["SumRadie"] = "R " + Fmt(RadieVal(typInt));

            kv["SumFas"] = "30º";
            kv["SumGF"] = "30º";
            kv["SumIF1"] = "45º";
            kv["SumIF2"] = "45º";

            kv["Sumt1"] = Fmt3(T1T2Val(typInt));
            kv["Sumt2"] = Fmt3(T1T2Val(typInt));
            kv["Sumt4"] = Fmt3(T4Val(typInt));
            kv["Sumt5"] = Fmt1(1.6);

            kv["SumRa"] = "3.2 [2F]";

            double tmph = HVal(tmpd4);
            double tmphTol = HTol(tmph);
            kv["Sumh"] = "(h) " + Fmt(tmph);
            kv["SumhTol"] = "+ " + Fmt3(tmphTol) + " [3F]";
            kv["SumhTolN"] = "- " + Fmt1(0.0) + " [3F]";

            double tmpSb = SbVal(typInt);
            double tmpSbTol = SbTol(tmpSb);
            kv["Sumsb"] = "(Sb) 50";
            kv["SumsbTol"] = "± 0.310 [3F]";

            kv["SumGV"] = "45º";
            kv["SumRHT"] = "8x R1.6 ±0.4";
            kv["SumStämpling"] = "Stämplas enl. ritning 7430189";

            SumMaskinVal(kv, maskinVal);
            SumNB(kv, maskinVal);
            SumFrequencies(kv, maskinVal);
            SumMeasuringDevices(kv, maskinVal);
            SumAF(kv, maskinVal);

            kv["SumTextS1"] = "Grader, frifläckar, slagmärken, repor, valkar & andra defekter samt ojämnheter kontrolleras med ögonmått, för övriga mått används skjutmått.";

            kv["SumRitningsnr"] = tmpBet;
            kv["SumGTolRit"] = "7430181";
            kv["SumKlEgenskaper"] = "PRODUCTION NUTS & SLEEVES & HOUSINGS\\ALLMÄNKLASSADE EGENSKAPER\\Klassade egenskaper muttrar";

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
            bool svarvning = IsInGroup(maskinVal, MachinesLabelN);
            kv["SumMaskinValS1"] = "Maskin: " + s1 + (svarvning ? " - Svarvning" : " - OP1 & 2");
            kv["SumMaskinValS2"] = "Maskin: " + s2 + (svarvning ? " - Borrning & Fräsning" : " - OP3");
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

        private static void SumNB(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsInGroup(maskinVal, MachinesNB);
            kv["SumN1_4"] = m ? "Gänga Lyftögla" : "";
            kv["SumB1_4"] = m ? "G3" : "";
        }

        private static void SumFrequencies(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsInGroup(maskinVal, MachinesFreq);
            for (int i = 1; i <= 8; i++)
                kv["SumF1_" + i] = m ? "1/1" : "";
        }

        private static void SumMeasuringDevices(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsInGroup(maskinVal, MachinesFreq);
            kv["SumD1_1"] = m ? "Multimar" : "";
            kv["SumD1_2"] = m ? "Mikrometer" : "";
            kv["SumD1_3"] = m ? "Skjutmått" : "";
            kv["SumD1_4"] = m ? "Skjutmått" : "";
            kv["SumD1_5"] = m ? "Skjutmått" : "";
            kv["SumD1_6"] = m ? "Skjutmått" : "";
            kv["SumD1_7"] = m ? "Ytjämnhetsmätare" : "";
            kv["SumD1_8"] = m ? "Planhet med Egglinjal" : "";
        }

        private static void SumAF(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsInGroup(maskinVal, MachinesFreq);
            kv["SumAF1_1"] = m ? "Kontrolleras 1/tim med passbitsklove utf.1 " : "";
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = "";
            kv["SumAF1_6"] = m ? "Bearbetning i OP1 + 5mm" : "";
            kv["SumAF1_7"] = m ? "Övriga Ra värden 6,3" : "";
            kv["SumAF1_8"] = m ? "Vid misstänkt formfel lämnas till mätrum" : "";
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

        private static double D1(double d4, int s)
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

        private static double D3Val(int typ)
        {
            if (typ == 110) return 625.0;
            if (typ == 166) return 930.0;
            if (typ == 219) return 1215.0;
            return 0.0;
        }

        private static double D5Val(int typ)
        {
            if (typ == 110) return 595.0;
            if (typ == 166) return 890.0;
            if (typ == 219) return 1165.0;
            return 0.0;
        }

        private static double BVal(int typ)
        {
            if (typ == 110) return 47.0;
            if (typ == 166) return 57.0;
            if (typ == 219) return 67.0;
            return 0.0;
        }

        private static double RadieVal(int typ)
        {
            if (typ == 110) return 4.0;
            if (typ == 166) return 6.0;
            if (typ == 219) return 4.0;
            return 0.0;
        }

        private static double T1T2Val(int typ)
        {
            if (typ == 110) return 0.1;
            if (typ == 166) return 0.14;
            if (typ == 219) return 0.1;
            return 0.0;
        }

        private static double T4Val(int typ)
        {
            if (typ == 110) return 0.7;
            if (typ == 166) return 0.9;
            if (typ == 219) return 1.05;
            return 0.0;
        }

        private static double HVal(double d4)
        {
            if (d4 < 221) return 9.0;
            if (d4 < 281) return 10.0;
            if (d4 < 341) return 12.0;
            if (d4 < 361) return 13.0;
            if (d4 < 421) return 14.0;
            if (d4 < 501) return 15.0;
            if (d4 < 671) return 20.0;
            return 25.0;
        }

        private static double HTol(double h)
        {
            if (h < 7) return 1.2;
            if (h < 11) return 1.5;
            if (h < 19) return 1.8;
            if (h < 31) return 2.1;
            return 2.5;
        }

        private static double SbVal(int typ)
        {
            if (typ == 110) return 32.0;
            if (typ == 166) return 40.0;
            if (typ == 219) return 50.0;
            return 0.0;
        }

        private static double SbTol(double sb)
        {
            if (sb < 31) return 0.26;
            if (sb < 51) return 0.31;
            return 0.37;
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

        private static int GetMember(int val, int[] list)
        {
            for (int i = 0; i < list.Length; i++)
                if (list[i] == val) return i + 1;
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

        private static int TryParseInt(string s)
        {
            int v;
            return int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static string Fmt(double v) => v.ToString(CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt1(double v) => v.ToString("F1", CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt3(double v) => v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
    }
}