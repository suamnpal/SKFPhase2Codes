using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class KRAGAR_TS_5_32 : ITemplateCalculations
    {
        private static readonly string[] GeneralMachines = new[] { "Nakamura", "LC-20", "MaxMuller" };

        private static readonly string[] TypeList = new[]
            { "5","6","7","8","9","10","11","12","13","15","16","17","18","20","22","24","26","28","30","32" };

        private static readonly double[] CList = new[]
            { 3.5, 4, 4.5, 4, 4, 4, 5.5, 5, 5, 5, 5.5, 5.5, 5.5, 6, 7, 7, 7, 7.5, 7.5, 8.5 };

        private static readonly double[] FList = new[]
            { 4.5, 5, 5.3, 4.8, 4.8, 4.8, 6.5, 6, 6, 6, 6.5, 6.5, 6.5, 7.3, 8, 8, 8, 8.5, 8.5, 9.5 };

        private static readonly double[] HList = new double[]
            { 36, 41, 51, 55, 63, 68, 73, 79, 84, 94, 100, 105, 110, 127, 138, 148, 158, 168, 180, 188 };

        private const double TmpB1 = 3.3;
        private const double TmpB2v = 2.0;
        private const double TmpB3 = 2.0;
        private const double TmpB4 = 6.0;

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");

            bool tmpSlash = tmpBet.IndexOf('/') >= 0;

            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet2str = tokens.Length > 1 ? tokens[1] : string.Empty;
            double tmpBet2num = TryParseDouble(tmpBet2str);
            string tmpTypStr = RightSafe(tmpBet2num.ToString("0", CultureInfo.InvariantCulture), 2);

            double typNum = TryParseDouble(tmpTypStr);
            int typLista = GetMember(tmpTypStr, TypeList);

            double tmpBval = BVal(typNum);
            kv["SumB"] = "(B) " + Fmt(tmpBval).Replace(".", ",");
            kv["SumBTol"] = "+ 0";
            kv["SumBTolN"] = BTolN(tmpBval);

            double tmpC = typLista >= 1 && typLista <= CList.Length ? CList[typLista - 1] : 0;
            kv["SumC"] = "(C) " + Fmt(tmpC).Replace(".", ",");
            kv["SumCTol"] = CTol(tmpC);

            double tmpE = EVal(typNum);
            kv["SumE"] = "(E) " +  Fmt(tmpE).Replace(".", ",");
            kv["SumETol"] = "+ 0";
            kv["SumETolN"] = H13TolN(tmpE);

            double tmpF = typLista >= 1 && typLista <= FList.Length ? FList[typLista - 1] : 0;
            kv["SumF"] = "(F) " + Fmt(tmpF).Replace(".", ",");
            kv["SumFTol"] = FTol(tmpF);

            double tmpG = GVal(typNum);
            kv["SumG"] = "(G) " + Fmt(tmpG).Replace(".", ",");
            kv["SumGTol"] = tmpG < 6.01 ? "± 0.1" : "± 0.2";

            double tmpH = typLista >= 1 && typLista <= HList.Length ? HList[typLista - 1] : 0;
            kv["SumH"] = "(H) " + Fmt(tmpH).Replace(".", ",");
            kv["SumHTol"] = "+ 0";
            kv["SumHTolN"] = H13TolN(tmpH);

            kv["SumB1"] = "(B1) " + Fmt(TmpB1).Replace(".", ",");
            kv["SumB1Tol"] = B1Tol(TmpB1);

            kv["SumB2"] = "(B2) " + Fmt(TmpB2v).Replace(".", ",");
            kv["SumB2Tol"] = "± 0.1";

            kv["SumB3"] = "(B3) " + Fmt(TmpB3).Replace(".", ",");
            kv["SumB3Tol"] = "± 0.1";

            kv["SumB4"] = "(B4) " + Fmt(TmpB4).Replace(".", ",");
            kv["SumB4Tol"] = "± 0.1";

            double tmpd1 = D1Val(typNum);
            double tmpd1B2 = tmpd1 + (TmpB2v * 2.0);
            kv["Sumd1"] = "(d1) " + Fmt(tmpd1).Replace(".", ",");
            kv["Sumd1Tol"] = D1TolPos(tmpd1);
            kv["Sumd1TolN"] = D1TolNeg(tmpd1);

            double tmpd2 = D2Val(typNum, tmpd1);
            kv["Sumd2"] = "(d2) " + Fmt(tmpd2).Replace(".", ",");
            kv["Sumd2Tol"] = "+ 0";
            kv["Sumd2TolN"] = H13TolN(tmpd2);

            kv["SumStämpel"] = subject;

            kv["SumR"] = "R 1 ± 0.3";
            kv["SumV"] = "14º";
            kv["SumRit"] = (typNum > 22 ? "7433664" : "347403") + ":senaste utgåva";

            bool isGeneral = IsGeneralMachine(maskinVal);
            string tmpMaskinValS1 = isGeneral ? maskinVal : "";
            kv["SumMaskinValS1"] = "Maskin: " + tmpMaskinValS1;

            SumFrequencies(kv, isGeneral, maskinVal);
            SumMeasuringDevices(kv, isGeneral);
            SumAF(kv, isGeneral, maskinVal, tmpd1B2);

            kv["SumTextS1"] = "Trepunktsmätning utförs vid inställning. max variation 0,1mm\nBearbetas Ra 12,5 runt om, skarpa kanter avgradas. Max radie i botten på spår (F) 1mm.";

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
                       " dagar)<<LineBreak>>Information om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument\n\nPopupruta aktiv till " +
                       validTill.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return "";
        }

        private static void SumFrequencies(Dictionary<string, string> kv, bool isGeneral, string maskinVal)
        {
            bool isEmpty = string.IsNullOrEmpty(maskinVal);
            kv["SumF1_1"] = (isGeneral || isEmpty) ? "1/1" : "";
            for (int i = 2; i <= 9; i++)
                kv["SumF1_" + i] = isGeneral ? "1/1" : (isEmpty ? "1/5" : "");
            kv["SumF1_0"] = isGeneral ? "1/1" : (isEmpty ? "1/5" : "");
            kv["SumF1_11"] = isGeneral ? "1/1" : (isEmpty ? "1/5" : "");
        }

        private static void SumMeasuringDevices(Dictionary<string, string> kv, bool isGeneral)
        {
            kv["SumD1_1"] = isGeneral ? "UD-Apparat / Mikrometer" : "";
            kv["SumD1_2"] = isGeneral ? "Skjutmått" : "";
            kv["SumD1_3"] = isGeneral ? "Skjutmått" : "";
            kv["SumD1_4"] = isGeneral ? "Skjutmått" : "";
            kv["SumD1_5"] = isGeneral ? "Skjutmått" : "";
            kv["SumD1_6"] = isGeneral ? "Mall" : "";
            kv["SumD1_7"] = isGeneral ? "Mall" : "";
            kv["SumD1_8"] = isGeneral ? "Mall" : "";
            kv["SumD1_9"] = isGeneral ? "Digitalt Skjutmått" : "";
            kv["SumD1_0"] = isGeneral ? "Digital Djup/hakmått" : "";
            kv["SumD1_11"] = isGeneral ? "Digital Djup/hakmått" : "";
        }

        private static void SumAF(Dictionary<string, string> kv, bool isGeneral, string maskinVal, double d1B2)
        {
            bool isEmpty = string.IsNullOrEmpty(maskinVal);
            kv["SumAF1_1"] = isGeneral ? "Tillhörande Ring/Klove. + Fas 1mm" : (isEmpty ? "" : "");
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = "";
            kv["SumAF1_6"] = "";
            kv["SumAF1_7"] = "";
            kv["SumAF1_8"] = isGeneral ? "3.3mm JS15" : (isEmpty ? "" : "");
            kv["SumAF1_9"] = isGeneral ? "Mäts: (d1+4mm) " + Fmt(d1B2) + " ± 0.6" : (isEmpty ? "" : "");
            kv["SumAF1_0"] = "";
            kv["SumAF1_11"] = "";
        }

        private static bool IsGeneralMachine(string mv)
        {
            if (string.IsNullOrEmpty(mv)) return false;
            for (int i = 0; i < GeneralMachines.Length; i++)
                if (string.Equals(GeneralMachines[i], mv, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        private static double BVal(double typ)
        {
            if (typ < 6) return 17;
            if (typ < 7) return 18;
            if (typ < 11) return 19;
            if (typ < 16) return 24;
            if (typ < 20) return 26;
            if (typ < 22) return 28.5;
            if (typ < 28) return 31.5;
            if (typ < 32) return 33.5;
            return 35.5;
        }

        private static double EVal(double typ)
        {
            if (typ < 8) return 4;
            if (typ < 12) return 5;
            if (typ < 16) return 6;
            if (typ < 20) return 7;
            if (Math.Abs(typ - 20) < 0.001) return 8;
            if (typ < 28) return 9;
            return 10;
        }

        private static double GVal(double typ)
        {
            if (typ < 7) return 5.0;
            if (typ < 11) return 5.2;
            if (typ < 20) return 7.2;
            return 7.5;
        }

        private static double D1Val(double typ)
        {
            if (typ < 15) return (typ * 10.0 / 2.0) - 5.0;
            if (typ < 26) return (typ * 10.0 / 2.0) - 10.0;
            if (typ < 32) return (typ * 10.0 / 2.0) - 15.0;
            return (typ * 10.0 / 2.0) - 20.0;
        }

        private static double D2Val(double typ, double d1)
        {
            if (typ < 7) return d1 + 10;
            if (typ < 15) return d1 + 15;
            if (typ < 20) return d1 + 20;
            if (typ < 26) return d1 + 25;
            if (typ < 32) return d1 + 30;
            return d1 + 35;
        }

        private static string BTolN(double b)
        {
            if (b < 3.1) return "- 0.140";
            if (b < 6.1) return "- 0.180";
            if (b < 10.1) return "- 0.220";
            if (b < 18.1) return "- 0.270";
            if (b < 30.1) return "- 0.330";
            if (b < 50.1) return "- 0.390";
            return "- 0.460";
        }

        private static string CTol(double c)
        {
            if (c < 3.01) return "± 0.200";
            if (c < 6.01) return "± 0.240";
            if (c < 10.01) return "± 0.290";
            return "± 0.350";
        }

        private static string B1Tol(double b1)
        {
            if (b1 < 3.1) return "± 0.200";
            if (b1 < 6.1) return "± 0.240";
            return "± 0.290";
        }

        private static string FTol(double f)
        {
            if (f < 3.01) return "± 0.070";
            if (f < 6.01) return "± 0.090";
            if (f < 10.01) return "± 0.110";
            if (f < 18.01) return "± 0.135";
            if (f < 30.01) return "± 0.165";
            if (f < 50.01) return "± 0.195";
            if (f < 80.01) return "± 0.230";
            if (f < 120.01) return "± 0.270";
            if (f < 180.01) return "± 0.315";
            if (f < 250.01) return "± 0.360";
            if (f < 315.01) return "± 0.405";
            if (f < 400.01) return "± 0.445";
            if (f < 500.01) return "± 0.485";
            if (f < 630.01) return "± 0.550";
            if (f < 800.01) return "± 0.625";
            if (f < 1000.01) return "± 0.700";
            if (f < 1250.01) return "± 0.825";
            if (f < 1600.01) return "± 0.975";
            if (f < 2000.01) return "± 1.150";
            if (f < 2500.01) return "± 1.400";
            return "± 1.650";
        }

        private static string H13TolN(double v)
        {
            if (v < 3.1) return "- 0.140";
            if (v < 6.1) return "- 0.180";
            if (v < 10.1) return "- 0.220";
            if (v < 18.1) return "- 0.270";
            if (v < 30.1) return "- 0.330";
            if (v < 50.1) return "- 0.390";
            if (v < 80.1) return "- 0.460";
            if (v < 120.1) return "- 0.540";
            if (v < 180.1) return "- 0.630";
            if (v < 250.1) return "- 0.720";
            if (v < 315.1) return "- 0.810";
            if (v < 400.1) return "- 0.890";
            if (v < 500.1) return "- 0.970";
            return "- 1.100";
        }

        private static string D1TolPos(double d1)
        {
            if (d1 < 30.01) return "+ 0.053";
            if (d1 < 50.01) return "+ 0.064";
            if (d1 < 80.01) return "+ 0.076";
            if (d1 < 120.01) return "+ 0.090";
            return "+ 0.106";
        }

        private static string D1TolNeg(double d1)
        {
            if (d1 < 30.01) return "+ 0.020";
            if (d1 < 50.01) return "+ 0.025";
            if (d1 < 80.01) return "+ 0.030";
            if (d1 < 120.01) return "+ 0.036";
            return "+ 0.043";
        }

        private static int GetMember(string val, string[] list)
        {
            for (int i = 0; i < list.Length; i++)
                if (string.Equals(list[i], val, StringComparison.OrdinalIgnoreCase)) return i + 1;
            return 0;
        }

        private static string RightSafe(string s, int n)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            return s.Length <= n ? s : s.Substring(s.Length - n);
        }

        private static double TryParseDouble(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;
            double v;
            return double.TryParse(s.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static string Fmt(double v) => v.ToString(CommonFunctions.Culture).Replace(",", ".");
    }
}