using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class MUTTRAR_ANN_0024_OP1_2_Svarv_Borr_Fras : ITemplateCalculations
    {
        private static readonly string[] Machines = new[] { "Nakamura", "LC20" };

        private const double TmpD = 160.0;
        private const double TmpP = 3.0;
        private const double Tmpd3 = 212.1;
        private const double Tmpd5 = 160.5;
        private const double Tmpd6 = 8.4;
        private const double Tmpd6p = 1.0;
        private const double Tmpd6d = 8.5;
        private const double Tmpd7 = 7.6;
        private const double Tmpd8 = 194.0;
        private const double Tmpd9 = 171.0;
        private const double Tmpd10 = 191.3;
        private const double TmpB = 40.0;
        private const double Tmpb1 = 5.0;
        private const double Tmpb2v = 5.0;
        private const double Tmpb3 = 4.8;
        private const double Tmpb4 = 4.8;
        private const double Tmpb5 = 12.7;
        private const double Tmpb6 = 4.5;
        private const double Tmpb7 = 3.0;
        private const double Tmpb8 = 5.2;
        private const double Tmpb9 = 10.6;
        private const double TmpHB = 180.0;
        private const double TmpG = 10.0;
        private const double TmpH = 15.4;
        private const double TmpK = 12.9;
        private const double TmpKa = 0.005;

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");

            bool tmpSlash = tmpBet.IndexOf('/') >= 0;
            bool tmpL = tmpFormat.IndexOf('L') >= 0;
            bool tmpANN = tmpFormat.IndexOf("ANN", StringComparison.OrdinalIgnoreCase) >= 0;

            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : string.Empty;
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : string.Empty;

            string tmpSerie = tmpBet2.Length >= 2 ? tmpBet2.Substring(0, 2) : tmpBet2;
            string tmpTyp = tmpBet2.Length >= 2 ? tmpBet2.Substring(tmpBet2.Length - 2) : tmpBet2;

            kv["SumRitningsnr"] = tmpBet;
            kv["SumRitningsnr2"] = tmpBet;

            kv["SumP"] = "(P) " + Fmt1(TmpP);
            kv["SumVP"] = "60º";

            kv["SumGänga"] = "M " + Fmt(TmpD) + "x" + Fmt1(TmpP);
            kv["SumGMall"] = "M x " + Fmt1(TmpP);

            double tmpd4 = TmpD;
            kv["Sumd4"] = "(d4) " + Fmt(tmpd4);
            kv["Sumd4Tol"] = "± " + Fmt3(GenTol(tmpd4));

            double tmpd2 = D2(TmpD, TmpP);
            kv["Sumd2"] = "(d2) " + Fmt(tmpd2);
            kv["Sumd2Tol"] = "+ " + Fmt3(D2Tol(TmpD, TmpP));
            kv["Sumd2TolN"] = "- 0";

            double tmpd1 = D1(tmpd2, TmpP);
            kv["Sumd1"] = "(d1) " + Fmt(tmpd1);
            kv["Sumd1Tol"] = "+ " + Fmt3(0.4);
            kv["Sumd1TolN"] = "- 0";

            kv["Sumd3"] = "(d3) " + Fmt(Tmpd3);
            kv["Sumd3Tol"] = "+ 0";
            kv["Sumd3TolN"] = "- " + Fmt3(0.1);

            kv["Sumd5"] = "(d5) " + Fmt(Tmpd5);
            kv["Sumd5Tol"] = "+ " + Fmt3(0.5);
            kv["Sumd5TolN"] = "- 0";

            kv["Sumd6"] = "Ø " + Fmt(Tmpd6);
            kv["Sumd6Tol"] = "+ " + Fmt3(0.22);
            kv["Sumd6TolN"] = "- 0";

            kv["Sumd6p"] = Fmt1(Tmpd6p);

            kv["Sumd6d"] = Fmt(Tmpd6d);
            kv["Sumd6dTol"] = "+ " + Fmt3(0.25);

            kv["Sumd7"] = "Ø " + Fmt(Tmpd7);

            kv["Sumd8"] = "(d8) " + Fmt(Tmpd8);
            kv["Sumd8Tol"] = "± " + Fmt3(0.15);

            kv["Sumd9"] = "(d9) " + Fmt(Tmpd9);
            kv["Sumd9Tol"] = "± " + Fmt3(0.15);

            kv["Sumd10"] = "(d10) " + Fmt(Tmpd10);
            kv["Sumd10Tol"] = "+ 0" ;
            kv["Sumd10TolN"] = "- " + Fmt3(0.5);

            kv["SumB"] = "(B) " + Fmt(TmpB);
            kv["SumBTol"] = "+ 0" ;
            kv["SumBTolN"] = "- " + Fmt3(0.15);

            kv["Sumb1"] = "(b1) " + Fmt(Tmpb1);
            kv["Sumb1Tol"] = "± " + Fmt3(0.1);

            kv["Sumb2"] = "(b2) " + Fmt(Tmpb2v);
            kv["Sumb2Tol"] = "± " + Fmt3(0.1);

            kv["Sumb3"] = "(b3) " + Fmt(Tmpb3);
            kv["Sumb3Tol"] = "± " + Fmt3(0.1);

            kv["Sumb4"] = "(b4) " + Fmt(Tmpb4);
            kv["Sumb4Tol"] = "± " + Fmt3(0.1);

            kv["Sumb5"] = "(b5) " + Fmt(Tmpb5);
            kv["Sumb5Tol"] = "± " + Fmt3(0.2);

            kv["Sumb6"] = "(b6) " + Fmt(Tmpb6);
            kv["Sumb6Tol"] = "± " + Fmt3(0.2);

            kv["Sumb7"] = "(b7) " + Fmt(Tmpb7);
            kv["Sumb7Tol"] = "± " + Fmt3(0.2);

            kv["Sumb8"] = Fmt(Tmpb8);
            kv["Sumb8Tol"] = "± " + Fmt3(0.1);

            kv["Sumb9"] = Fmt(Tmpb9);
            kv["Sumb9Tol"] = "± " + Fmt3(0.2);

            kv["SumHB"] = "(HB) " + Fmt(TmpHB);
            kv["SumHBTol"] = "± " + Fmt3(JS13Tol(TmpHB));

            kv["SumG"] = "(G) M" + Fmt(TmpG);

            kv["SumH"] = "(H) " + Fmt(TmpH);
            kv["SumHTol"] = "+ 0.0";
            kv["SumHTolN"] = "- " + Fmt1(1.0);

            kv["SumK"] = "(K) " + Fmt(TmpK);
            kv["SumKTol"] = "+ " + Fmt1(1.0);
            kv["SumKTolN"] = "- " + Fmt1(0.0);

            kv["SumKa"] = Fmt3(TmpKa);
            kv["SumPl"] = kv["SumKa"];

            kv["SumStämpling"] = "Stämplas enl. ritning 7430189";

            SumMaskinVal(kv, maskinVal);
            SumFrequencies(kv, maskinVal);
            SumMeasuringDevices(kv, maskinVal);
            SumAF(kv, maskinVal);

            kv["SumKlEgenskaper"] = "PRODUCTION NUTS & SLEEVES & HOUSINGS\\ALLMÄN\\KLASSADE EGENSKAPER\\Klassade egenskaper muttrar";
            kv["SumKlEgenskaper2"] = kv["SumKlEgenskaper"];

            string tmpText = "Övriga mått kontrolleras vid inställning.<<LineBreak>>Okulärkontroll: Grader, slagmärken, ojämnheter & andra ytdefekter. Rätt & tydlig märkning.";
            kv["SumTextS1"] = tmpText;
            kv["SumTextS2"] = tmpText;

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
            string s = IsMachine(maskinVal) ? maskinVal : "";
            kv["SumMaskinValS1"] = ("Maskin: " + s + " - Svarning").Trim();
            kv["SumMaskinValS2"] = ("Maskin: " + s + " - Fräs Borrning").Trim();
        }

        private static void SumFrequencies(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsMachine(maskinVal);
            for (int i = 1; i <= 8; i++) kv["SumF1_" + i] = m ? "1/1" : "";
            for (int i = 1; i <= 6; i++) kv["SumF2_" + i] = m ? "1/5" : "";
        }

        private static void SumMeasuringDevices(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsMachine(maskinVal);
            kv["SumD1_1"] = m ? "Multimar" : "";
            kv["SumD1_2"] = m ? "Skjutmått" : "";
            kv["SumD1_3"] = m ? "Skjutmått" : "";
            kv["SumD1_4"] = m ? "Skjutmått" : "";
            kv["SumD1_5"] = m ? "Skjutmått" : "";
            kv["SumD1_6"] = m ? "Skjutmått" : "";
            kv["SumD1_7"] = m ? "Ytjämnhetsmätare" : "";
            kv["SumD1_8"] = m ? "Egglinjal" : "";
            kv["SumD2_1"] = m ? "Skjutmått" : "";
            kv["SumD2_2"] = m ? "Djupmått" : "";
            kv["SumD2_3"] = m ? "Gängtolk" : "";
            kv["SumD2_4"] = m ? "Djupmått" : "";
            kv["SumD2_5"] = m ? "Skjutmått" : "";
            kv["SumD2_6"] = m ? "Gängtolk min/max" : "";
        }

        private static void SumAF(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsMachine(maskinVal);
            for (int i = 1; i <= 7; i++) kv["SumAF1_" + i] = "";
            kv["SumAF1_8"] = m ? "Vid tveksamhet lämnas mutter till mätrum" : "";
            for (int i = 1; i <= 6; i++) kv["SumAF2_" + i] = "";
        }

        private static bool IsMachine(string maskinVal)
        {
            if (string.IsNullOrEmpty(maskinVal)) return false;
            for (int i = 0; i < Machines.Length; i++)
                if (string.Equals(Machines[i], maskinVal, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }

        private static double D2(double d, double p)
        {
            if (Math.Abs(p - 0.75) < 0.001) return Math.Round(d - 0.487, 3);
            if (Math.Abs(p - 1.0) < 0.001) return Math.Round(d - 0.65, 3);
            if (Math.Abs(p - 1.5) < 0.001) return Math.Round(d - 0.974, 3);
            if (Math.Abs(p - 2.0) < 0.001) return Math.Round(d - 1.299, 3);
            if (Math.Abs(p - 3.0) < 0.001) return Math.Round(d - 1.949, 3);
            if (Math.Abs(p - 4.0) < 0.001) return Math.Round(d - 2.0, 3);
            return Math.Round(d - 2.5, 3);
        }

        private static double D2Tol(double d, double p)
        {
            if (Math.Abs(p - 0.75) < 0.001) return 0.106;
            if (Math.Abs(p - 1.0) < 0.001) return 0.125;
            if (Math.Abs(p - 1.5) < 0.001) return d < 45 ? 0.16 : 0.17;
            if (Math.Abs(p - 2.0) < 0.001) return d < 90 ? 0.19 : 0.20;
            if (Math.Abs(p - 3.0) < 0.001) return d < 180 ? 0.236 : 0.265;
            if (Math.Abs(p - 4.0) < 0.001) return 0.475;
            if (Math.Abs(p - 5.0) < 0.001) return 0.530;
            if (Math.Abs(p - 6.0) < 0.001) return 0.600;
            if (Math.Abs(p - 7.0) < 0.001) return 0.630;
            return 0.710;
        }

        private static double D1(double d2, double p)
        {
            if (Math.Abs(p - 0.75) < 0.001) return Math.Round(d2 - 0.325, 3);
            if (Math.Abs(p - 1.0) < 0.001) return Math.Round(d2 - 0.433, 3);
            if (Math.Abs(p - 1.5) < 0.001) return Math.Round(d2 - 0.65, 3);
            if (Math.Abs(p - 2.0) < 0.001) return Math.Round(d2 - 0.866, 3);
            if (Math.Abs(p - 3.0) < 0.001) return Math.Round(d2 - 1.299, 3);
            if (Math.Abs(p - 4.0) < 0.001) return Math.Round(d2 - 2.0, 3);
            return Math.Round(d2 - 2.5, 3);
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

        private static double JS13Tol(double v)
        {
            if (v < 3.01) return 0.070;
            if (v < 6.01) return 0.090;
            if (v < 10.01) return 0.110;
            if (v < 18.01) return 0.135;
            if (v < 30.01) return 0.165;
            if (v < 50.01) return 0.195;
            if (v < 80.01) return 0.230;
            if (v < 120.01) return 0.270;
            if (v < 180.01) return 0.315;
            if (v < 250.01) return 0.360;
            if (v < 315.01) return 0.405;
            if (v < 400.01) return 0.445;
            if (v < 500.01) return 0.485;
            if (v < 630.01) return 0.550;
            if (v < 800.01) return 0.625;
            if (v < 1000.01) return 0.700;
            if (v < 1250.01) return 0.825;
            if (v < 1600.01) return 0.975;
            if (v < 2000.01) return 1.150;
            if (v < 2500.01) return 1.400;
            return 1.650;
        }

        private static string Fmt(double v) => v.ToString(CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt1(double v) => v.ToString("F1", CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt3(double v) => v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
    }
}