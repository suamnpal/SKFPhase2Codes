using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_CG_serie_294 : ITemplateCalculations
    {
        private static readonly string[] Machines = new[] { "LB45", "MaxMuller", "Nakamura", "VTR-160", "MacTurn 550" };

        private static readonly string[] TypStd = new[]
        {
            "44","48","52","56","60","64","68","72","76","80","84","88","92","96",
            "500","530","560","630","710","750","800","850","900"
        };

        private static readonly string[] TypE = new[]
        {
            "72","76","80","84","88","92","96","500","530","560","600","630","670",
            "710","750","800","850","900","950","1000","1060"
        };

        private static readonly double[] AStd = { 267, 287, 312, 336, 356, 381, 405, 425, 447, 472, 492, 519, 539, 565, 585, 620, 0, 735, 827, 870, 928, 0, 1042 };
        private static readonly double[] AE = { 425, 447, 472, 492, 518, 538, 565, 585, 620, 656, 698, 736, 780, 827, 871, 928, 985, 1041, 1098, 1153, 1222 };

        private static readonly double[] CStd = { 66.4, 66.5, 70, 79, 79, 85, 93, 93, 94.2, 98, 99, 112.4, 112, 121.5, 121.5, 128, 0, 153, 168, 172, 182, 0, 203 };
        private static readonly double[] CE = { 91, 91, 97, 96, 110, 110, 116, 118, 126, 129, 134, 148, 153, 164, 165, 182, 181, 192, 204, 212, 199 };

        private static readonly double[] DStd = { 251, 270, 294, 317, 335, 360, 383, 402, 424, 448, 468, 492, 511, 536, 557, 587, 0, 700, 782, 830, 882, 0, 990 };
        private static readonly double[] DE = { 389, 410, 431, 451, 474, 494, 511, 537, 569, 602, 642, 675, 717, 759, 806, 862, 907, 960, 1012, 1065, 1128 };

        private static readonly double[] D1Std = { 231, 251, 272, 293, 313, 334, 355, 375, 396, 417, 437, 458, 478, 500, 520, 550, 0, 654, 736, 780, 829, 0, 932 };
        private static readonly double[] D1E = { 373, 393, 414, 434, 455, 475, 494, 516, 547, 578, 620, 651, 692, 733, 776, 827, 876, 927, 978, 1029, 1090 };

        private static readonly double[] FStd = { 228.8, 248.7, 269.6, 290.5, 310.4, 331.3, 352.2, 372.1, 393, 413.9, 433.8, 454.7, 474.6, 496.5, 516.4, 546, 0, 649.7, 731, 774, 823, 0, 924 };
        private static readonly double[] FE = { 369.5, 389.5, 410.5, 430.5, 451.5, 471.5, 490.5, 512, 543, 574, 615.5, 646, 687, 727.5, 770, 820, 869, 920, 970, 1021, 1081 };

        private static readonly double[] GStd = { 225.5, 245.5, 266, 286.5, 306.5, 327, 347.5, 367.5, 388, 408.5, 428.5, 449, 469, 490, 510, 540, 0, 642, 723, 765, 815, 0, 916 };
        private static readonly double[] GE = { 366, 386, 407, 427, 447, 467, 487, 508, 538, 569, 608, 640, 680, 721, 763, 814, 863, 914, 964, 1014, 1075 };

        private static readonly double[] HStd = { 40, 40, 42, 47, 48, 52, 57.5, 57, 57, 61, 60, 69, 69, 75, 75.5, 78, 0, 94, 103, 105, 111, 0, 124 };
        private static readonly double[] HE = { 55, 54, 58, 57, 66, 66, 69, 71, 76, 76, 79, 89, 92, 99, 98, 111, 106, 113, 122, 127, 138 };

        private static readonly double[] JStd = { 32, 32, 34, 38, 39, 42, 46.5, 46, 46, 49, 48, 56, 56, 61, 59.5, 64, 0, 77, 85, 85, 91, 0, 102 };
        private static readonly double[] JE = { 45, 43, 46, 45, 53, 53, 55, 57, 62, 61, 64, 72, 74, 80, 79, 91, 85, 92, 99, 104, 112 };

        private static readonly double[] LStd = { 27, 27, 28, 31, 31, 34, 37, 37, 37, 40, 40, 47, 47, 50, 50, 57, 0, 73, 80, 80, 86, 0, 97 };
        private static readonly double[] LE = { 41, 40, 43, 42, 50, 50, 52, 53, 57, 56, 58, 68, 70, 76, 74, 86, 84, 90, 95, 102, 110 };

        private static readonly double[] MStd = { 8, 8, 8, 9, 9, 10, 11, 11, 11, 12, 12, 13, 13, 14, 14, 14, 0, 17, 18, 20, 20, 0, 22 };
        private static readonly double[] ME = { 10, 11, 12, 12, 13, 13, 14, 14, 14, 15, 15, 17, 18, 19, 19, 20, 21, 21.5, 23, 23, 26 };

        private static readonly double[] OStd = { 2.2, 2.3, 2.6, 2.7, 3, 3, 3, 3, 3.2, 3.3, 3.3, 3.4, 3.5, 3.6, 3.7, 4.1, 0, 4.5, 5, 5.5, 5.7, 0, 6.6 };
        private static readonly double[] OE = { 3.1, 3.2, 3.3, 3.3, 3.4, 3.5, 3.6, 4, 4.1, 4.2, 4.4, 4.8, 5, 5.1, 5.5, 6, 6.2, 6.4, 7, 7, 8.5 };

        private static readonly double[] PStd = { 17, 17, 18, 20, 20, 22, 24, 24, 24, 26, 26, 30, 30, 32, 32, 36, 0, 45, 49, 50, 53, 0, 60 };
        private static readonly double[] PE = { 25, 25, 27, 27, 31, 31, 33, 34, 36, 35, 36, 42, 44, 48, 46, 53, 52.5, 56, 59, 62.5, 68 };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");

            bool tmpSlash = tmpBet.IndexOf('/') >= 0;
            bool tmpE = tmpBet.IndexOf('E') >= 0;

            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : string.Empty;
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : string.Empty;

            string tmpSerie = Left(tmpBet2, 3);
            string tmpTyp = (!tmpSlash || tmpBet2.Length > 3) ? Middle(tmpBet2, 3, 2) : tmpBet3;

            double typ = ToNum(tmpTyp);
            int idx = MemberIndex(tmpE ? TypE : TypStd, tmpTyp);

            double a = ListNum(tmpE ? AE : AStd, idx);
            double c = ListNum(tmpE ? CE : CStd, idx);
            double d = ListNum(tmpE ? DE : DStd, idx);
            double d1 = ListNum(tmpE ? D1E : D1Std, idx);
            double f = ListNum(tmpE ? FE : FStd, idx);
            double g = ListNum(tmpE ? GE : GStd, idx);
            double h = ListNum(tmpE ? HE : HStd, idx);
            double j = ListNum(tmpE ? JE : JStd, idx);
            double l = ListNum(tmpE ? LE : LStd, idx);
            double m = ListNum(tmpE ? ME : MStd, idx);
            double o = ListNum(tmpE ? OE : OStd, idx);
            double p = ListNum(tmpE ? PE : PStd, idx);

            string kon0 = Dot1(0.0);
            string kon02 = Dot3(0.2);
            string kon03 = Dot3(0.3);
            string kon1 = Dot3(1.0);

            kv["SumA"] = "(A) " + ListTxt(tmpE ? AE : AStd, idx);
            kv["SumATol"] = "+ " + kon0;
            kv["SumATolN"] = "- " + Dot3(H11Tol(a));

            double b = tmpSlash ? ToNum(tmpBet3) : typ / 2 * 10;
            double bAvU = BAvMattU(b, tmpE);
            double bAvO = BAvMattO(b, tmpE);
            double utrB = Round(b + bAvU);
            double utrBTol = Round(bAvO - bAvU);

            kv["SumB"] = "(B) " + Com(utrB);
            kv["SumBTol"] = "+ " + Dot(utrBTol);
            kv["SumBTolN"] = "- " + kon0;
            kv["SumB1"] = kv["SumB"];
            kv["SumB1Tol"] = kv["SumBTol"];
            kv["SumB1TolN"] = kv["SumBTolN"];

            double cTolN = CTolN(typ, tmpE);
            string sumCTolN = "- " + Dot3(cTolN);

            kv["SumC"] = "(C) " + ListTxt(tmpE ? CE : CStd, idx);
            kv["SumCTol"] = "+ " + kon0;
            kv["SumCTolN"] = sumCTolN;

            double dAvO = DAvMattO(d);
            double dAvU = DAvMattU(d);
            double utrD = Round(d - dAvO);
            double utrDTol = Round(Math.Abs(dAvO - dAvU));

            kv["SumD"] = "(D) " + Com(utrD);
            kv["SumDTol"] = "+ " + kon0;
            kv["SumDTolN"] = "- " + Dot3(utrDTol);

            double d1Tol = D1TolPlus(d1, tmpE);
            double d1TolN = D1TolMinus(d1, tmpE);
            double utrD1 = Round(d1 + d1Tol);
            double utrD1Tol = Round(Math.Abs(d1Tol - d1TolN));
            string d1Txt = ListTxt(tmpE ? D1E : D1Std, idx);

            kv["SumD1"] = "(D1) " + Dot(utrD1);
            kv["SumD1Tol"] = "+ " + kon0;
            kv["SumD1TolN"] = "- " + Dot3(utrD1Tol);
            kv["SumD1klove"] = d1Txt;

            kv["SumF"] = "(F) " + ListTxt(tmpE ? FE : FStd, idx);
            kv["SumFTol"] = "+ " + Dot3(FTol(f, tmpE));
            kv["SumFTolN"] = "- " + kon0;

            kv["SumG"] = "(G) " + ListTxt(tmpE ? GE : GStd, idx);
            kv["SumGTol"] = "+ " + Dot3(GTol(g, f, tmpE));
            kv["SumGTolN"] = "- " + kon0;

            kv["SumH"] = "(H) " + ListTxt(tmpE ? HE : HStd, idx);
            kv["SumHTol"] = "+ " + kon0;
            kv["SumHTolN"] = sumCTolN;

            string sumJTol = "+ " + Dot3(cTolN);
            kv["SumJ"] = "(J) " + ListTxt(tmpE ? JE : JStd, idx);
            kv["SumJTol"] = sumJTol;
            kv["SumJTolN"] = "- " + kon0;

            double k = KVal(typ, tmpE);
            kv["SumK"] = "(K) " + Com(k);
            kv["SumKTol"] = "+ " + kon1;
            kv["SumKTolN"] = "- " + kon0;

            kv["SumL"] = "(L) " + ListTxt(tmpE ? LE : LStd, idx);
            kv["SumLTol"] = sumJTol;
            kv["SumLTolN"] = "- " + kon0;
            kv["SumLx"] = "(L) " + Com(Round(c - l));
            kv["SumLxTol"] = "+ " + kon0;
            kv["SumLxTolN"] = "- " + Dot3(Round(cTolN * 2));

            kv["SumM"] = "(M) " + ListTxt(tmpE ? ME : MStd, idx);
            kv["SumMTol"] = "+ " + kon0;
            kv["SumMTolN"] = sumCTolN;
            kv["SumMx"] = "(M) " + Com(Round(c - m));
            kv["SumMxTol"] = "+ " + Dot3(cTolN);
            kv["SumMxTolN"] = "- " + Dot3(cTolN);

            double n = NVal(typ);
            kv["SumN"] = "(N) " + Com(n);
            kv["SumNTol"] = "+ " + kon03;
            kv["SumNTolN"] = "- " + kon0;
            kv["SumN1"] = kv["SumN"];
            kv["SumN1Tol"] = kv["SumNTol"];
            kv["SumN1TolN"] = kv["SumNTolN"];

            double tVal = TVal(typ);
            kv["SumT"] = "(T) " + Com(tVal);
            kv["SumTTol"] = "+ " + kon02;
            kv["SumTTolN"] = "- " + kon0;

            kv["SumO"] = "(O) " + ListTxt(tmpE ? OE : OStd, idx);
            kv["SumOTol"] = sumJTol;
            kv["SumOTolN"] = "- " + kon0;

            double r = RVal(typ, tmpE);
            double rTol = RTolVal(r);
            kv["SumR"] = "(R) " + Com2(r);
            kv["SumRTol"] = "+ " + Dot3(rTol);
            kv["SumRTolN"] = "- " + kon0;

            double rh = Round(utrB + (utrBTol / 2) + ((rTol / 2 + r) * 2));
            kv["SumRh"] = "(Rh) Ø " + Com2(rh);
            kv["SumRhTol"] = "± " + FmtDot(rTol);

            string sumR1 = Com(R1Val(typ)) + "x45º";
            kv["SumR1"] = sumR1;
            kv["SumR1a"] = sumR1;

            string sumR2 = "R" + Com(R2Val(typ, tmpE));
            kv["SumR2"] = sumR2;
            kv["SumR2sid1"] = sumR2;
            kv["SumR2a"] = sumR2;
            kv["SumR2ab"] = sumR2;

            kv["SumP"] = "(P) " + ListTxt(tmpE ? PE : PStd, idx);
            kv["SumPTol"] = "+ " + kon0;
            kv["SumPTolN"] = sumCTolN;
            kv["SumPx"] = "(P) " + Com(Round(c - p));
            kv["SumPxTol"] = "+ " + Dot3(cTolN);
            kv["SumPxTolN"] = "- " + Dot3(cTolN);

            kv["SumS"] = "Ø " + Com(SVal(typ)) + " x 6 st. hål" + "\n" + " lika delning";

            string sumHM = "(HM) " + Com(Round(c - h));
            string sumHMTol = "± " + Dot3(cTolN);
            kv["SumHM"] = sumHM;
            kv["SumHMTol"] = sumHMTol;
            kv["SumHM2"] = sumHM;
            kv["SumHM2Tol"] = sumHMTol;

            kv["SumE"] = EVal(typ);
            kv["SumE1"] = E1Val(typ);

            kv["SumRa"] = "1";
            kv["SumRa1"] = "1";
            kv["SumRa25"] = "1";

            kv["SumMaskinValS1"] = "Maskin: " + maskinVal + " - Sid.1";
            kv["SumMaskinValS2"] = "Maskin: " + maskinVal + " - Sid.2";

            SumFrequencies(kv, maskinVal);
            SumMeasuringDevices(kv, maskinVal);
            SumAF(kv, maskinVal, d1Txt);

            kv["SumTextS1"] = "";
            kv["SumTextS2"] = "";

            kv["SumRitS1"] = "Windchill.skf.net - " + tmpFormat;
            kv["SumRitS2"] = "Windchill.skf.net - " + tmpFormat;

            kv["VaLSerie"] = ToNum(tmpSerie) != 294 ? "Denna mall avser endast serie 294 CG." : "";
            kv["VaLFärdig"] = "";
            kv["VaLInfo"] = "";

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
                       " dagar)\n\n\n\nInformation om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument\n\nPopupruta aktiv till " +
                       validTill.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return "";
        }

        private static void SumFrequencies(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsMachine(maskinVal);
            kv["SumF_A"] = m ? "1/3" : "";
            kv["SumF_D"] = m ? "1/3" : "";
            kv["SumF_D1"] = m ? "1/3" : "";
            kv["SumF_FBG"] = m ? "1/3" : "";
            kv["SumF_HJC"] = m ? "1/3" : "";
            kv["SumF_HM"] = m ? "Inst." : "";
            kv["SumF_Fas"] = m ? "Inst." : "";
            kv["SumF_R"] = m ? "Inst." : "";
            kv["SumF_Ra"] = m ? "1/3" : "";

            kv["SumF_N"] = m ? "1/1" : "";
            kv["SumF_LMO"] = m ? "1/3" : "";
            kv["SumF_HM2"] = m ? "Inst." : "";
            kv["SumF_Fas2"] = m ? "Inst." : "";
            kv["SumF_TK"] = m ? "Inst." : "";
            kv["SumF_R2"] = m ? "Inst." : "";
            kv["SumF_Ra2"] = m ? "1/3" : "";
            kv["SumF_EE1"] = m ? "Inst." : "";
        }

        private static void SumMeasuringDevices(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsMachine(maskinVal);
            kv["SumD_A"] = m ? "Digitalskjutmått" : "";
            kv["SumD_D"] = m ? "Mikrometer/Skjutmått" : "";
            kv["SumD_D1"] = m ? "Mikrometer/Mätplatta" : "";
            kv["SumD_FBG"] = m ? "Inv. Mikrometer/Skjutmått" : "";
            kv["SumD_HJC"] = m ? "Digital Djup/Skjutmått" : "";
            kv["SumD_HM"] = m ? "Digital Djupmått" : "";
            kv["SumD_Fas"] = m ? "Digitalskjutmått" : "";
            kv["SumD_R"] = m ? "Radielyra" : "";
            kv["SumD_Ra"] = m ? "Ytjämnhetsmätare" : "";

            kv["SumD_N"] = m ? "Digital Djup/Hakmått" : "";
            kv["SumD_LMO"] = m ? "Digital Djup/Hakmått" : "";
            kv["SumD_TK"] = m ? "Passbitar/Skjutmått" : "";
            kv["SumD_HM2"] = m ? "Digital Djupmått" : "";
            kv["SumD_Fas2"] = m ? "Digitalskjutmått" : "";
            kv["SumD_R2"] = m ? "Radielyra" : "";
            kv["SumD_Ra2"] = m ? "Ytjämnhetsmätare" : "";
            kv["SumD_EE1"] = "";
        }

        private static void SumAF(Dictionary<string, string> kv, string maskinVal, string d1Txt)
        {
            bool m = IsMachine(maskinVal);
            kv["SumAF_A"] = "";
            kv["SumAF_D"] = "";
            kv["SumAF_D1"] = m ? "Klove: " + d1Txt + " Utf.2" : "";
            kv["SumAF_FBG"] = "";
            kv["SumAF_HJC"] = "";
            kv["SumAF_HM"] = "";
            kv["SumAF_Fas"] = "";
            kv["SumAF_R"] = "";
            kv["SumAF_Ra"] = "";

            kv["SumAF_N"] = "";
            kv["SumAF_LMO"] = "";
            kv["SumAF_TK"] = "";
            kv["SumAF_HM2"] = "";
            kv["SumAF_Fas2"] = "";
            kv["SumAF_R2"] = "";
            kv["SumAF_Ra2"] = "";
            kv["SumAF_EE1"] = "";
        }

        private static bool IsMachine(string maskinVal)
        {
            if (string.IsNullOrEmpty(maskinVal)) return false;
            for (int i = 0; i < Machines.Length; i++)
                if (string.Equals(Machines[i], maskinVal, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }

        private static int MemberIndex(string[] list, string value)
        {
            if (string.IsNullOrEmpty(value)) return 0;
            for (int i = 0; i < list.Length; i++)
                if (string.Equals(list[i], value, StringComparison.OrdinalIgnoreCase))
                    return i + 1;
            return 0;
        }

        private static double ListNum(double[] list, int index)
        {
            return index >= 1 && index <= list.Length ? list[index - 1] : 0.0;
        }

        private static string ListTxt(double[] list, int index)
        {
            return index >= 1 && index <= list.Length ? Com(list[index - 1]) : "";
        }

        private static string Left(string s, int count)
        {
            if (string.IsNullOrEmpty(s)) return "";
            return s.Length <= count ? s : s.Substring(0, count);
        }

        private static string Middle(string s, int offset, int count)
        {
            if (string.IsNullOrEmpty(s) || s.Length <= offset) return "";
            int len = Math.Min(count, s.Length - offset);
            return s.Substring(offset, len);
        }

        private static double ToNum(string s)
        {
            if (string.IsNullOrEmpty(s)) return 0.0;
            double v;
            if (double.TryParse(s.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v))
                return v;
            return 0.0;
        }

        private static double Round(double v)
        {
            return Math.Round(v, 3, MidpointRounding.AwayFromZero);
        }

        private static double BAvMattU(double b, bool e)
        {
            if (e)
            {
                if (b < 410) return 0.68;
                if (b < 450) return 0.76;
                if (b < 510) return 0.84;
                if (b < 570) return 0.96;
                if (b < 640) return 1.05;
                if (b < 711) return 1.20;
                if (b < 801) return 1.35;
                if (b < 905) return 1.55;
                if (b < 1001) return 1.70;
                return 1.90;
            }
            if (b < 165) return 0.28;
            if (b < 185) return 0.31;
            if (b < 210) return 0.34;
            if (b < 230) return 0.38;
            if (b < 250) return 0.42;
            if (b < 290) return 0.48;
            if (b < 310) return 0.54;
            if (b < 350) return 0.60;
            if (b < 410) return 0.68;
            if (b < 450) return 0.76;
            if (b < 510) return 0.84;
            if (b < 570) return 0.96;
            if (b < 640) return 1.05;
            if (b < 711) return 1.20;
            if (b < 801) return 1.35;
            if (b < 1251) return 2.10;
            return 2.70;
        }

        private static double BAvMattO(double b, bool e)
        {
            if (e)
            {
                if (b < 410) return 1.04;
                if (b < 450) return 1.16;
                if (b < 510) return 1.24;
                if (b < 570) return 1.40;
                if (b < 640) return 1.49;
                if (b < 711) return 1.70;
                if (b < 801) return 1.85;
                if (b < 905) return 2.11;
                if (b < 1001) return 2.26;
                return 2.56;
            }
            if (b < 165) return 0.53;
            if (b < 185) return 0.56;
            if (b < 210) return 0.63;
            if (b < 230) return 0.67;
            if (b < 250) return 0.71;
            if (b < 290) return 0.80;
            if (b < 310) return 0.86;
            if (b < 350) return 0.96;
            if (b < 410) return 1.04;
            if (b < 450) return 1.16;
            if (b < 510) return 1.24;
            if (b < 570) return 1.39;
            if (b < 640) return 1.48;
            if (b < 711) return 1.70;
            if (b < 801) return 1.82;
            if (b < 1251) return 2.76;
            return 3.48;
        }

        private static double DAvMattO(double d)
        {
            if (d < 271) return 0.92;
            if (d < 310) return 1.05;
            if (d < 340) return 1.20;
            if (d < 390) return 1.35;
            if (d < 450) return 1.50;
            if (d < 500) return 1.65;
            if (d < 560) return 1.85;
            if (d < 631) return 2.10;
            if (d < 701) return 2.30;
            if (d < 783) return 2.60;
            if (d < 890) return 3.00;
            if (d < 1000) return 3.30;
            if (d < 1100) return 3.70;
            return 4.10;
        }

        private static double DAvMattU(double d)
        {
            if (d < 271) return 1.13;
            if (d < 310) return 1.26;
            if (d < 340) return 1.43;
            if (d < 390) return 1.58;
            if (d < 450) return 1.75;
            if (d < 500) return 1.90;
            if (d < 560) return 2.13;
            if (d < 631) return 2.38;
            if (d < 701) return 2.62;
            if (d < 783) return 2.92;
            if (d < 890) return 3.36;
            if (d < 1000) return 3.66;
            if (d < 1100) return 4.12;
            return 4.52;
        }

        private static double D1TolPlus(double d1, bool e)
        {
            if (d1 < 175) return 0.055;
            if (d1 < 250) return 0.077;
            if (d1 < 314) return 0.086;
            if (d1 < 397) return 0.094;
            if (d1 < 501) return 0.103;
            if (d1 < 621) return e ? 0.114 : 0.112;
            if (d1 < 781) return e ? 0.129 : 0.125;
            if (d1 < 830) return e ? 0.146 : 0.138;
            if (d1 < 1000) return 0.146;
            if (d1 < 1280) return e ? 0.171 : 0.203;
            return 0.242;
        }

        private static double D1TolMinus(double d1, bool e)
        {
            if (d1 < 175) return 0.015;
            if (d1 < 250) return 0.031;
            if (d1 < 314) return 0.034;
            if (d1 < 397) return 0.037;
            if (d1 < 501) return 0.040;
            if (d1 < 621) return 0.044;
            if (d1 < 781) return 0.049;
            if (d1 < 830) return e ? 0.056 : 0.054;
            if (d1 < 1000) return 0.056;
            return 0.066;
        }

        private static double CTolN(double typ, bool e)
        {
            if (typ < 37) return 0.25;
            if (typ < 50) return 0.29;
            if (typ < 62) return 0.32;
            if (typ < 82) return 0.36;
            if (typ < 540) return e ? 0.44 : 0.40;
            if (typ < 640) return e ? 0.44 : 0.43;
            if (typ < 801) return e ? 0.50 : 0.47;
            if (typ < 1001) return 0.56;
            return 0.66;
        }

        private static double FTol(double f, bool e)
        {
            if (f < 173) return 0.25;
            if (f < 245) return 0.29;
            if (f < 305) return 0.32;
            if (f < 386) return 0.36;
            if (f < 488) return 0.40;
            if (f < 609) return e ? 0.44 : 0.43;
            if (f < 760) return e ? 0.50 : 0.47;
            if (f < 960) return 0.56;
            return 0.66;
        }

        private static double GTol(double g, double f, bool e)
        {
            if (g < 173) return 0.5;
            if (g < 245) return 0.58;
            if (g < 305) return 0.64;
            if (g < 386) return 0.72;
            if (g < 488) return 0.8;
            if (g < 609) return e ? 0.88 : 0.86;
            if (f < 760) return e ? 1.0 : 0.94;
            if (f < 960) return 1.12;
            return 1.32;
        }

        private static double KVal(double typ, bool e)
        {
            if (!e)
            {
                if (typ < 53) return 6;
                if (typ < 65) return 7;
                if (typ < 77) return 8;
                if (typ < 93) return 9;
                if (typ < 601) return 10;
                if (typ < 671) return 12;
                if (typ < 751) return 13;
                if (typ < 851) return 14;
                if (typ < 901) return 16;
                return 17;
            }
            if (typ < 85) return 8;
            if (typ < 93) return 9;
            if (typ < 531) return 10;
            if (typ < 601) return 11;
            if (typ < 671) return 12;
            if (typ < 711) return 13;
            if (typ < 851 || typ == 1060) return 14;
            if (typ == 900 || typ == 1000) return 16;
            return 17;
        }

        private static double NVal(double typ)
        {
            if (typ < 53) return 0.5;
            if (typ < 601) return 1;
            return 1.5;
        }

        private static double TVal(double typ)
        {
            if (typ < 57) return 2;
            if (typ < 85) return 2.5;
            if (typ < 601) return 3;
            if (typ < 751) return 4;
            if (typ < 851) return 5;
            if (typ < 1001) return 6;
            return 7;
        }

        private static double RVal(double typ, bool e)
        {
            if (typ < 61) return 5;
            if (typ < 85) return 6;
            if (typ < 531) return 7;
            if (typ < 631) return 9;
            if (typ == 850 || (typ == 900 && e)) return 12;
            if (typ == 1000) return 13;
            return 11;
        }

        private static double RTolVal(double r)
        {
            if (r < 5.5) return 1;
            if (r < 6.5) return 1.5;
            if (r < 11) return 2;
            return 4;
        }

        private static double R1Val(double typ)
        {
            if (typ < 57) return 0.5;
            if (typ < 851) return 1;
            return 1.5;
        }

        private static double R2Val(double typ, bool e)
        {
            if (!e)
            {
                if (typ < 49) return 1;
                if (typ < 73) return 1.2;
                if (typ < 501) return 1.5;
                if (typ < 631) return 2;
                if (typ < 711) return 2.5;
                if (typ < 851) return 3;
                return 4;
            }
            if (typ < 97) return 1.5;
            if (typ < 601) return 2;
            if (typ < 711) return 2.5;
            if (typ < 801) return 3;
            if (typ < 901) return 3.5;
            if (typ < 1001) return 4;
            return 5;
        }

        private static double SVal(double typ)
        {
            if (typ < 85) return 10;
            if (typ < 97) return 12;
            return 15;
        }

        private static string EVal(double typ)
        {
            if (typ < 37) return "0.160";
            if (typ < 49) return "0.185";
            if (typ < 61) return "0.210";
            if (typ < 81) return "0.230";
            if (typ < 501) return "0.250";
            if (typ < 631) return "0.280";
            if (typ < 801) return "0.320";
            if (typ < 1001) return "0.360";
            return "0.420";
        }

        private static string E1Val(double typ)
        {
            if (typ < 61) return "0.072";
            if (typ < 77) return "0.089";
            if (typ < 97) return "0.097";
            if (typ < 601) return "0.110";
            if (typ < 751) return "0.125";
            if (typ < 1001) return "0.140";
            return "0.165";
        }

        private static double H11Tol(double d)
        {
            if (d < 3.01) return 0.060;
            if (d < 6.01) return 0.075;
            if (d < 10.01) return 0.090;
            if (d < 18.01) return 0.110;
            if (d < 30.01) return 0.130;
            if (d < 50.01) return 0.160;
            if (d < 80.01) return 0.190;
            if (d < 120.01) return 0.220;
            if (d < 180.01) return 0.250;
            if (d < 250.01) return 0.290;
            if (d < 315.01) return 0.320;
            if (d < 400.01) return 0.360;
            if (d < 500.01) return 0.400;
            if (d < 630.01) return 0.440;
            if (d < 800.01) return 0.500;
            if (d < 1000.01) return 0.560;
            if (d < 1250.01) return 0.660;
            if (d < 1600.01) return 0.780;
            if (d < 2000.01) return 0.920;
            if (d < 2500.01) return 1.100;
            return 1.350;
        }

        private static string Com(double v)
        {
            return v.ToString(CultureInfo.InvariantCulture).Replace(".", ",");
        }

        private static string Com2(double v)
        {
            return v.ToString("F2", CultureInfo.InvariantCulture).Replace(".", ",");
        }

        private static string Dot(double v)
        {
            return v.ToString(CultureInfo.InvariantCulture);
        }

        private static string Dot1(double v)
        {
            return v.ToString("F1", CultureInfo.InvariantCulture);
        }

        private static string Dot3(double v)
        {
            return v.ToString("F3", CultureInfo.InvariantCulture);
        }
        private static string FmtDot(double v) =>
           Math.Round(v, 3, MidpointRounding.AwayFromZero).ToString("0.################", CommonFunctions.Culture).Replace(",", ".");
    }
}