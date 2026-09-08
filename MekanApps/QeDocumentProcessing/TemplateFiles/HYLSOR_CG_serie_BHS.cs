using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_CG_serie_BHS : ITemplateCalculations
    {
        private static readonly string[] Machines = new[]
            { "LB45", "MaxMuller", "Nakamura", "VTR-160", "MacTurn 550", "Skepp6" };

        private static readonly double[] AList = new double[] { 682, 565 };
        private static readonly double[] BList = new double[] { 630, 480 };
        private static readonly double[] CList = new double[] { 73, 116 };
        private static readonly double[] DList = new double[] { 650, 511 };
        private static readonly double[] D1List = new double[] { 638, 494 };
        private static readonly double[] FList = new double[] { 633, 490.5 };
        private static readonly double[] GList = new double[] { 634, 487 };
        private static readonly double[] HList = new double[] { 42, 69 };
        private static readonly double[] JList = new double[] { 36.5, 55 };
        private static readonly double[] KList = new double[] { 5, 10 };
        private static readonly double[] LList = new double[] { 30, 52 };
        private static readonly double[] MList = new double[] { 7, 14 };
        private static readonly double[] NList = new double[] { 0.5, 1 };
        private static readonly double[] TList = new double[] { 4.5, 3 };
        private static readonly double[] OList = new double[] { 6.5, 3.6 };
        private static readonly double[] RList = new double[] { 4, 7 };
        private static readonly double[] R1List = new double[] { 0.5, 1 };
        private static readonly double[] R2List = new double[] { 2, 1.5 };
        private static readonly double[] PList = new double[] { 18, 33 };
        private static readonly string[] EList = new string[] { "-", "250" };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");

            bool tmpSlash = tmpBet.IndexOf('/') >= 0;
            bool tmpE = tmpBet.IndexOf("8034", StringComparison.OrdinalIgnoreCase) >= 0
                         || tmpBet.IndexOf("8032", StringComparison.OrdinalIgnoreCase) >= 0;
            bool tmpBHS = tmpBet.IndexOf("BHS", StringComparison.OrdinalIgnoreCase) >= 0;

            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : string.Empty;
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : string.Empty;
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : string.Empty;

            string tmpSerie = tmpBet3.Length >= 2 ? tmpBet3.Substring(0, 2) : tmpBet3;
            string tmpTyp = tmpBet3.Length >= 2 ? tmpBet3.Substring(tmpBet3.Length - 2) : tmpBet3;

            int serieInt = TryParseInt(tmpSerie);
            int typLista = GetMember(tmpTyp, new[] { "32", "34" });

            kv["VaLSerie"] = serieInt != 80 ? "Denna mall avser endast serie 80 BHS." : "";

            double tmpA = GetVal(AList, typLista);
            double tmpB = GetVal(BList, typLista);
            double tmpC = GetVal(CList, typLista);
            double tmpD = GetVal(DList, typLista);
            double tmpD1 = GetVal(D1List, typLista);
            double tmpF = GetVal(FList, typLista);
            double tmpG = GetVal(GList, typLista);
            double tmpH = GetVal(HList, typLista);
            double tmpJ = GetVal(JList, typLista);
            double tmpK = GetVal(KList, typLista);
            double tmpL = GetVal(LList, typLista);
            double tmpM = GetVal(MList, typLista);
            double tmpN = GetVal(NList, typLista);
            double tmpT = GetVal(TList, typLista);
            double tmpO = GetVal(OList, typLista);
            double tmpR = GetVal(RList, typLista);
            double tmpP = GetVal(PList, typLista);

            kv["SumA"] = "(A) " + Fmt(tmpA).Replace(".",",");
            kv["SumATol"] = "+ " + ("0.0");
            kv["SumATolN"] = "- " + Fmt3(H11Tol(tmpA));

            double tmpBAvMattU = BAvMattU(tmpB, tmpE);
            double tmpBAvMattO = BAvMattO(tmpB, tmpE);
            double sumBval = tmpB + tmpBAvMattU;
            double sumBTolWidth = tmpBAvMattO - tmpBAvMattU;
            kv["SumB"] = "(B) " + Fmt(sumBval).Replace(".", ",");
            kv["SumBTol"] = "+ " + Fmt2(sumBTolWidth);
            kv["SumBTolN"] = "- " + ("0.0");
            kv["SumB1"] = kv["SumB"];
            kv["SumB1Tol"] = kv["SumBTol"];
            kv["SumB1TolN"] = kv["SumBTolN"];

            double tmpCTolN = tmpE ? 0.44 : 0.40;
            string sumCTolN = "- " + Fmt3(tmpCTolN);
            kv["SumC"] = "(C) " + Fmt(tmpC).Replace(".", ",");
            kv["SumCTol"] = "+ " + ("0.0");
            kv["SumCTolN"] = sumCTolN;

            double tmpDAvMattO = DAvMattO(tmpD);
            double tmpDAvMattU = DAvMattU(tmpD);
            double sumDval = tmpD - tmpDAvMattO;
            double sumDTolN = Math.Abs(tmpDAvMattO - tmpDAvMattU);
            kv["SumD"] = "(D) " + Fmt(sumDval).Replace(".", ",");
            kv["SumDTol"] = "+ " + ("0.0");
            kv["SumDTolN"] = "- " + Fmt3(sumDTolN);

            double tmpD1TolPos = D1TolPos(tmpD1);
            double tmpD1TolNeg = D1TolNeg(tmpD1);
            double sumD1val = tmpD1 + tmpD1TolPos;
            double sumD1TolN = Math.Abs(tmpD1TolPos - tmpD1TolNeg);
            kv["SumD1"] = "(D1) " + Fmt(sumD1val);
            kv["SumD1Tol"] = "+ " + ("0.0");
            kv["SumD1TolN"] = "- " + Fmt3(sumD1TolN);
            kv["SumD1klove"] = Fmt(tmpD1);

            double tmpFTol = FTol(tmpF, tmpE);
            kv["SumF"] = "(F) " + Fmt(tmpF).Replace(".", ",");
            kv["SumFTol"] = "+ " + Fmt3(tmpFTol);
            kv["SumFTolN"] = "- " + ("0.0");

            double tmpGTol = GTol(tmpG, tmpF, tmpE);
            kv["SumG"] = "(G) " + Fmt(tmpG).Replace(".", ",");
            kv["SumGTol"] = "+ " + Fmt3(tmpGTol);
            kv["SumGTolN"] = "- " + ("0.0");

            kv["SumH"] = "(H) " + Fmt(tmpH).Replace(".", ",");
            kv["SumHTol"] = "+ " + ("0.0");
            kv["SumHTolN"] = sumCTolN;

            string sumJTol = "+ " + Fmt3(tmpCTolN);
            kv["SumJ"] = "(J) " + Fmt(tmpJ).Replace(".", ",");
            kv["SumJTol"] = sumJTol;
            kv["SumJTolN"] = "- " + ("0.0");

            kv["SumK"] = "(K) " + Fmt(tmpK).Replace(".", ",");
            kv["SumKTol"] = "+ " + Fmt3(1.0);
            kv["SumKTolN"] = "- " + ("0.0");

            kv["SumL"] = "(L) " + Fmt(tmpL).Replace(".", ",");
            kv["SumLTol"] = sumJTol;
            kv["SumLTolN"] = "- " + ("0.0");

            kv["SumM"] = "(M) " + Fmt(tmpM).Replace(".", ",");
            kv["SumMTol"] = "+ " + ("0.0");
            kv["SumMTolN"] = sumCTolN;

            kv["SumN"] = "(N) " + Fmt(tmpN).Replace(".", ",");
            kv["SumNTol"] = "+ " + Fmt3(0.3);
            kv["SumNTolN"] = "- " + ("0.0");
            kv["SumN1"] = kv["SumN"];
            kv["SumN1Tol"] = kv["SumNTol"];
            kv["SumN1TolN"] = kv["SumNTolN"];

            kv["SumT"] = "(T) " + Fmt(tmpT).Replace(".", ",");
            kv["SumTTol"] = "+ " + Fmt3(0.2);
            kv["SumTTolN"] = "- " + ("0.0");

            kv["SumO"] = "(O) " + Fmt(tmpO).Replace(".", ",");
            kv["SumOTol"] = sumJTol;
            kv["SumOTolN"] = "- " + ("0.0");

            double tmpRTol = RTol(tmpR);
            kv["SumR"] = "(R) " + Fmt(tmpR).Replace(".", ",");
            kv["SumRTol"] = "+ " + Fmt3(tmpRTol);
            kv["SumRTolN"] = "- " + ("0.0");

            double tmpRh = Math.Round(sumBval + (sumBTolWidth / 2.0) + ((tmpRTol / 2.0 + tmpR) * 2.0), 2);
            kv["SumRh"] = "(Rh) Ø " + Fmt2(tmpRh).Replace(".", ",");
            //kv["SumRhTol"] = "± " + Fmt3(tmpRTol);
            kv["SumRhTol"] = "± 1";

            string r1Str = GetVal(R1List, typLista).ToString(CommonFunctions.Culture).Replace(",", ".");
            kv["SumR1"] = r1Str.Replace(".",",") + "x45º";
            kv["SumR1a"] = kv["SumR1"];

            string r2Str = GetVal(R2List, typLista).ToString(CommonFunctions.Culture).Replace(",", ".");
            kv["SumR2"] = "R" + r2Str;
            kv["SumR2sid1"] = kv["SumR2"];
            kv["SumR2a"] = kv["SumR2"];
            kv["SumR2ab"] = kv["SumR2"];

            kv["SumP"] = "(P) " + Fmt(tmpP).Replace(".", ",");
            kv["SumPTol"] = "+ " + ("0.0");
            kv["SumPTolN"] = sumCTolN;

            kv["SumS"] = "Ø 12 x 6 st. hål\n lika delning";

            double tmpHM = tmpC - tmpH;
            kv["SumHM"] = "(HM) " + Fmt(tmpHM).Replace(".", ",");
            kv["SumHM2"] = kv["SumHM"];

            string sumE = typLista >= 1 && typLista <= EList.Length ? EList[typLista - 1] : "-";
            kv["SumE"] = sumE;
            kv["SumE1"] = sumE;

            kv["SumRa"] = "1";
            kv["SumRa1"] = "1";
            kv["SumRa25"] = "1";

            kv["SumMaskinValS1"] = "Maskin: " + maskinVal + " - Sid.1";
            kv["SumMaskinValS2"] = "Maskin: " + maskinVal + " - Sid.2";

            
                SumFrequencies(kv, maskinVal);
                SumMeasuringDevices(kv, maskinVal);
                SumAF(kv, maskinVal, Fmt(tmpD1));
          

            kv["SumTextS1"] = "";
            kv["SumTextS2"] = "";

            kv["SumRitS1"] = "Windchill.skf.net - " + tmpFormat;
            kv["SumRitS2"] = "Windchill.skf.net - " + tmpFormat;

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

        private static void SumFrequencies(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsMachine(maskinVal);
            if (maskinVal == "Skepp6")
                m= false;
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
            if (maskinVal == "Skepp6")
                m = false;
            kv["SumD_A"] = m ? "Digitalskjutmått" : "";
            kv["SumD_D"] = m ? "Mikrometer/Skjutmått" : "";
            kv["SumD_D1"] = m ? "Mikrometer/Mätplatta" : "";
            kv["SumD_FBG"] = m ? "Inv. Mikrometer/Skjutmått" : "";
            kv["SumD_N"] = m ? "Digital Djup/Hakmått" : "";
            kv["SumD_HJC"] = m ? "Digital Djup/Skjutmått" : "";
            kv["SumD_LMO"] = m ? "Digital Djup/Hakmått" : "";
            kv["SumD_HM"] = m ? "Digital Djupmått" : "";
            kv["SumD_Fas"] = m ? "Digitalskjutmått" : "";
            kv["SumD_TK"] = m ? "Passbitar/Skjutmått" : "";
            kv["SumD_R"] = m ? "Radielyra" : "";
            kv["SumD_Ra"] = m ? "Ytjämnhetsmätare" : "";
            kv["SumD_HM2"] = m ? "Digital Djupmått" : "";
            kv["SumD_Fas2"] = m ? "Digitalskjutmått" : "";
            kv["SumD_R2"] = m ? "Radielyra" : "";
            kv["SumD_Ra2"] = m ? "Ytjämnhetsmätare" : "";
            kv["SumD_EE1"] = "";
        }

        private static void SumAF(Dictionary<string, string> kv, string maskinVal, string tmpD1Fmt)
        {
            bool m = IsMachine(maskinVal);
            if (maskinVal == "Skepp6")
                m = false;
            kv["SumAF_A"] = "";
            kv["SumAF_D"] = "";
            kv["SumAF_D1"] = m ? "Klove: " + tmpD1Fmt + " Utf.2" : "";
            kv["SumAF_FBG"] = "";
            kv["SumAF_N"] = "";
            kv["SumAF_HJC"] = "";
            kv["SumAF_LMO"] = "";
            kv["SumAF_HM"] = "";
            kv["SumAF_Fas"] = "";
            kv["SumAF_TK"] = "";
            kv["SumAF_R"] = "";
            kv["SumAF_Ra"] = "";
            kv["SumAF_EE1"] = "";
            kv["SumAF_HM2"] = "";
            kv["SumAF_Fas2"] = "";
            kv["SumAF_R2"] = "";
            kv["SumAF_Ra2"] = "";
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

        private static double GetVal(double[] list, int oneBasedIdx)
        {
            if (oneBasedIdx < 1 || oneBasedIdx > list.Length) return 0;
            return list[oneBasedIdx - 1];
        }

        private static double BAvMattU(double b, bool tmpE)
        {
            if (tmpE)
            {
                if (b < 410) return 0.68; if (b < 450) return 0.76; if (b < 510) return 0.84;
                if (b < 570) return 0.96; if (b < 640) return 1.05; if (b < 711) return 1.20;
                if (b < 801) return 1.35; if (b < 851) return 1.55; if (b < 1001) return 1.70;
                return 1.90;
            }
            if (b < 165) return 0.28; if (b < 185) return 0.31; if (b < 210) return 0.34;
            if (b < 230) return 0.38; if (b < 250) return 0.42; if (b < 290) return 0.48;
            if (b < 310) return 0.54; if (b < 350) return 0.60; if (b < 410) return 0.68;
            if (b < 450) return 0.76; if (b < 510) return 0.84; if (b < 570) return 0.96;
            if (b < 640) return 1.05; if (b < 711) return 1.20; if (b < 801) return 1.35;
            if (b < 1251) return 2.10;
            return 2.70;
        }

        private static double BAvMattO(double b, bool tmpE)
        {
            if (tmpE)
            {
                if (b < 410) return 1.04; if (b < 450) return 1.16; if (b < 510) return 1.24;
                if (b < 570) return 1.40; if (b < 640) return 1.49; if (b < 711) return 1.70;
                if (b < 801) return 1.85; if (b < 851) return 2.11; if (b < 1001) return 2.26;
                return 2.56;
            }
            if (b < 165) return 0.53; if (b < 185) return 0.56; if (b < 210) return 0.63;
            if (b < 230) return 0.67; if (b < 250) return 0.71; if (b < 290) return 0.80;
            if (b < 310) return 0.86; if (b < 350) return 0.96; if (b < 410) return 1.04;
            if (b < 450) return 1.16; if (b < 510) return 1.24; if (b < 570) return 1.39;
            if (b < 640) return 1.49; if (b < 711) return 1.70; if (b < 801) return 1.82;
            if (b < 1251) return 2.76;
            return 3.48;
        }

        private static double DAvMattO(double d)
        {
            if (d < 271) return 0.92; if (d < 310) return 1.05; if (d < 340) return 1.20;
            if (d < 390) return 1.35; if (d < 450) return 1.50; if (d < 500) return 1.65;
            if (d < 560) return 1.85; if (d < 631) return 2.10; if (d < 701) return 2.30;
            if (d < 783) return 2.60; if (d < 890) return 3.00; if (d < 1000) return 3.30;
            if (d < 1100) return 3.70;
            return 4.10;
        }

        private static double DAvMattU(double d)
        {
            if (d < 271) return 1.13; if (d < 310) return 1.26; if (d < 340) return 1.43;
            if (d < 390) return 1.58; if (d < 450) return 1.75; if (d < 500) return 1.90;
            if (d < 560) return 2.13; if (d < 631) return 2.38; if (d < 701) return 2.62;
            if (d < 783) return 2.92; if (d < 890) return 3.36; if (d < 1000) return 3.66;
            if (d < 1100) return 4.12;
            return 4.52;
        }

        private static double D1TolPos(double d1)
        {
            if (d1 < 3.01) return 0.014; if (d1 < 6.01) return 0.020; if (d1 < 10.01) return 0.025;
            if (d1 < 18.01) return 0.030; if (d1 < 30.01) return 0.036; if (d1 < 50.01) return 0.042;
            if (d1 < 80.01) return 0.050; if (d1 < 120.01) return 0.058; if (d1 < 180.01) return 0.067;
            if (d1 < 250.01) return 0.077; if (d1 < 315.01) return 0.086; if (d1 < 400.01) return 0.094;
            if (d1 < 500.01) return 0.103; if (d1 < 630.01) return 0.114; if (d1 < 800.01) return 0.130;
            if (d1 < 1000.01) return 0.146; if (d1 < 1250.01) return 0.171; if (d1 < 1600.01) return 0.203;
            if (d1 < 2000.01) return 0.242; if (d1 < 2500.01) return 0.285;
            return 0.345;
        }

        private static double D1TolNeg(double d1)
        {
            if (d1 < 3.01) return 0.004; if (d1 < 6.01) return 0.008; if (d1 < 10.01) return 0.010;
            if (d1 < 18.01) return 0.012; if (d1 < 30.01) return 0.015; if (d1 < 50.01) return 0.017;
            if (d1 < 80.01) return 0.020; if (d1 < 120.01) return 0.023; if (d1 < 180.01) return 0.027;
            if (d1 < 250.01) return 0.031; if (d1 < 315.01) return 0.034; if (d1 < 400.01) return 0.037;
            if (d1 < 500.01) return 0.040; if (d1 < 630.01) return 0.044; if (d1 < 800.01) return 0.050;
            if (d1 < 1000.01) return 0.056; if (d1 < 1250.01) return 0.066; if (d1 < 1600.01) return 0.078;
            if (d1 < 2000.01) return 0.092; if (d1 < 2500.01) return 0.110;
            return 0.135;
        }

        private static double FTol(double f, bool tmpE)
        {
            if (f < 173) return 0.25;
            if (f < 245) return 0.29;
            if (f < 305) return 0.32;
            if (f < 386) return 0.36;
            if (f < 488) return 0.40;
            if (f < 609) return tmpE ? 0.44 : 0.43;
            if (f < 760) return tmpE ? 0.50 : 0.47;
            if (f < 960) return 0.56;
            return 0.66;
        }

        private static double GTol(double g, double f, bool tmpE)
        {
            if (g < 173) return 0.50;
            if (g < 245) return 0.58;
            if (g < 305) return 0.64;
            if (g < 386) return 0.72;
            if (g < 488) return 0.80;
            if (g < 609) return tmpE ? 0.88 : 0.86;
            if (f < 760) return tmpE ? 1.00 : 0.94;
            if (f < 960) return 1.12;
            return 1.32;
        }

        private static double RTol(double r)
        {
            if (r < 5.5) return 1.0;
            if (r < 6.5) return 1.5;
            if (r < 11) return 2.0;
            return 4.0;
        }

        private static double H11Tol(double v)
        {
            if (v < 3.01) return 0.060; if (v < 6.01) return 0.075; if (v < 10.01) return 0.090;
            if (v < 18.01) return 0.110; if (v < 30.01) return 0.130; if (v < 50.01) return 0.160;
            if (v < 80.01) return 0.190; if (v < 120.01) return 0.220; if (v < 180.01) return 0.250;
            if (v < 250.01) return 0.290; if (v < 315.01) return 0.320; if (v < 400.01) return 0.360;
            if (v < 500.01) return 0.400; if (v < 630.01) return 0.440; if (v < 800.01) return 0.500;
            if (v < 1000.01) return 0.560; if (v < 1250.01) return 0.660; if (v < 1600.01) return 0.780;
            if (v < 2000.01) return 0.920; if (v < 2500.01) return 1.100;
            return 1.350;
        }

        private static int TryParseInt(string s)
        {
            int v;
            return int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static string Fmt(double v) => v.ToString(CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt2(double v) => v.ToString("F2", CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt3(double v) => v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
    }
}