using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class TATNINGAR_HL_AFZ_AZI_POD : ITemplateCalculations
    {
        private static readonly string[] Machines = new[] { "LB45", "MacTurn 550" };

        private const double TmpD1 = 204.0;
        private const double TmpD2 = 230.0;
        private const double TmpD3 = 290.4;
        private const double TmpD4 = 300.0;
        private const double TmpD5 = 335.0;
        private const double TmpD6 = 25.0;
        private const double TmpD7 = 13.5;
        private const double TmpD8 = 210.0;
        private const double TmpD9 = 335.0;
        private const double TmpB = 42.0;
        private const double TmpB1 = 42.0;
        private const double TmpB2 = 13.0;
        private const double TmpB3 = 18.0;
        private const double TmpB4 = 6.4;
        private const double TmpB5 = 4.0;
        private const double TmpB6 = 5.6;
        private const double TmpG1 = 12.0;
        private const double TmpR1 = 0.5;
        private const double TmpR2 = 0.2;
        private const double TmpR3 = 0.8;
        private const double TmpRa125 = 12.5;
        private const double TmpRa16 = 1.6;

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");

            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : string.Empty;
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : string.Empty;
            string tmpBet4 = tokens.Length > 3 ? tokens[3] : string.Empty;

            bool tmpHL = tmpBet.IndexOf("HL", StringComparison.OrdinalIgnoreCase) >= 0;
            bool tmpAFZ = tmpBet3.IndexOf("AFZ", StringComparison.OrdinalIgnoreCase) >= 0;
            bool tmp4 = tmpBet4.IndexOf("4", StringComparison.OrdinalIgnoreCase) >= 0;

            kv["SumD1"] = "(D1) " + Fmt(TmpD1);
            kv["SumD1Tol"] = "+ " + Fmt3(H12Tol(TmpD1));
            kv["SumD1TolN"] = "+ 0";

            kv["SumD2"] = "(D2) " + Fmt(TmpD2);
            kv["SumD2Tol"] = "+ " + Fmt3(H8Tol(TmpD2));
            kv["SumD2TolN"] = "+ 0";

            kv["SumD3"] = "(D3) " + Fmt(TmpD3);
            kv["SumD3Tol"] = "- 0" ;
            kv["SumD3TolN"] = "- " + Fmt3(H11Tol(TmpD3));

            kv["SumD4"] = "(D4) " + Fmt(TmpD4);
            kv["SumD4Tol"] = "- " + Fmt3(F7Pos(TmpD4));
            kv["SumD4TolN"] = "- " + Fmt3(F7Neg(TmpD4));

            kv["SumD5"] = "(D5) " + Fmt(TmpD5);
            kv["SumD5Tol"] = "± " + Fmt3(GenTol(TmpD5));

            kv["SumD6"] = "(D6) " + Fmt(TmpD6);
            kv["SumD6Tol"] = "± " + Fmt3(GenTol(TmpD6));

            kv["SumD7"] = "(D7) " + Fmt(TmpD7);
            kv["SumD7Tol"] = "± " + Fmt3(GenTol(TmpD7));

            kv["SumD8"] = "(D8) " + Fmt(TmpD8);
            kv["SumD8Tol"] = "± " + Fmt3(GenTol(TmpD8));

            kv["SumD9"] = "(D9) " + Fmt(TmpD9);
            kv["SumD9Tol"] = "± " + Fmt3(GenTol(TmpD9));

            kv["SumB"] = "(B) " + Fmt(TmpB);
            kv["SumBTol"] = "± " + Fmt3(GenTol(TmpB));

            kv["SumB1"] = "(B1) " + Fmt(TmpB1);
            kv["SumB1Tol"] = "± " + Fmt3(GenTol(TmpB1));

            kv["SumB2"] = "(B2) " + Fmt(TmpB2);
            kv["SumB2Tol"] = "± " + Fmt3(GenTol(TmpB2));

            kv["SumB3"] = "(B3) " + Fmt(TmpB3);
            kv["SumB3Tol"] = "± " + Fmt3(GenTol(TmpB3));

            kv["SumB4"] = "(B4) " + Fmt(TmpB4);
            kv["SumB4Tol"] = "+ " + Fmt3(0.2);
            kv["SumB4TolN"] = "+ 0" ;

            kv["SumB5"] = "(B5) " + Fmt(TmpB5);
            kv["SumB5Tol"] = "± " + Fmt3(GenTol(TmpB5));

            kv["SumB6"] = "(B6) " + Fmt(TmpB6);
            kv["SumB6Tol"] = "+ " + Fmt3(0.2);
            kv["SumB6TolN"] = "+ 0";

            kv["SumG1"] = "M" + Fmt(TmpG1) + " (3x) Genomgående";
            kv["SumG1b"] = "M" + Fmt(TmpG1);

            kv["SumR1"] = "max R.5" ;
            kv["SumR2"] = "R.2 (2x)";
            kv["SumR3"] = "max R.8" ;
            kv["SumR4"] = "R.2 (4x)";

            kv["SumF1"] = Fmt1(1.5);
            kv["SumF2"] = Fmt0(5);
            kv["SumF3"] = Fmt2(0.2) + "x45º (2x)";
            kv["SumF4"] = Fmt1(0.5) + "x45º (18x)";
            kv["SumF5"] = Fmt1(0.5);

            kv["SumV1"] = Fmt(120.0) + "º (3x)";
            kv["SumV2"] = Fmt(10.0) + "º";
            kv["SumV3"] = Fmt(15.0) + "º";
            kv["SumV4"] = Fmt(60.0) + "º (6x)";

            kv["SumA1"] = "0.050";
            kv["SumA2"] = Fmt2(0.2);
            kv["SumP"] = Fmt2(0.2);

            kv["SumRa125"] = Fmt(TmpRa125);
            kv["SumRa125a"] = Fmt(TmpRa125);
            kv["SumRa16"] = Fmt(TmpRa16);

            SumMaskinVal(kv, maskinVal);
            SumFrequencies(kv, maskinVal);
            SumMeasuringDevices(kv, maskinVal);
            SumAF(kv, maskinVal);

            kv["SumSpTxt1"] = "Kontrolleras enl. styrplan";
            kv["SumSpTxt2"] = "Kontrolleras enl. styrplan";
            kv["SumTextS1"] = "Okulärkontroll gjuteridefekter, grader & slagmärken";
            kv["SumTextS2"] = kv["SumTextS1"];

            kv["SumRitS1"] = " " + tmpBet + ":senaste utgåva";
            kv["SumRitS2"] = kv["SumRitS1"];

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
            string s = IsMachine(maskinVal) ? maskinVal : "";
            kv["SumMaskinValS1"] = (s + " - Svarvning/Borrning").TrimStart();
            kv["SumMaskinValS2"] = (s + " - Svarvning/Gängning").TrimStart();
        }

        private static void SumFrequencies(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsMachine(maskinVal);
            kv["SumF1_1"] = m ? "1/1" : "";
            kv["SumF1_2"] = m ? "1/1" : "";
            kv["SumF1_3"] = m ? "" : "";
            kv["SumF1_4"] = m ? "1/1" : "";
            kv["SumF1_5"] = m ? "" : "";
            kv["SumF1_6"] = m ? "1/1" : "";
            kv["SumF1_7"] = m ? "1/1" : "";
            kv["SumF1_8"] = m ? "inst." : "";
            kv["SumF2_1"] = m ? "1/1" : "";
            kv["SumF2_2"] = m ? "" : "";
            kv["SumF2_3"] = m ? "" : "";
            kv["SumF2_4"] = m ? "inst." : "";
            kv["SumF2_5"] = m ? "" : "";
        }

        private static void SumMeasuringDevices(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsMachine(maskinVal);
            kv["SumD1_1"] = m ? "Multimar" : "";
            kv["SumD1_2"] = m ? "Multimar" : "";
            kv["SumD1_3"] = m ? "" : "";
            kv["SumD1_4"] = m ? "Gängtolk min/max" : "";
            kv["SumD1_5"] = m ? "" : "";
            kv["SumD1_6"] = m ? "Spårmikrometer" : "";
            kv["SumD1_7"] = m ? "Mätmaskin" : "";
            kv["SumD1_8"] = m ? "Skjutmått" : "";
            kv["SumD2_1"] = m ? "Skjutmått" : "";
            kv["SumD2_2"] = m ? "" : "";
            kv["SumD2_3"] = m ? "" : "";
            kv["SumD2_4"] = m ? "Skjutmått" : "";
            kv["SumD2_5"] = m ? "" : "";
        }

        private static void SumAF(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsMachine(maskinVal);
            kv["SumAF1_1"] = "";
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = m ? "Genomgående" : "";
            kv["SumAF1_5"] = "";
            kv["SumAF1_6"] = "";
            kv["SumAF1_7"] = m ? "Dokumenteras" : "";
            kv["SumAF1_8"] = "";
            kv["SumAF2_1"] = "";
            kv["SumAF2_2"] = "";
            kv["SumAF2_3"] = "";
            kv["SumAF2_4"] = "";
            kv["SumAF2_5"] = "";
        }

        private static bool IsMachine(string mv)
        {
            if (string.IsNullOrEmpty(mv)) return false;
            for (int i = 0; i < Machines.Length; i++)
                if (string.Equals(Machines[i], mv, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        private static double H12Tol(double v)
        {
            if (v < 3.01) return 0.100;
            if (v < 6.01) return 0.120;
            if (v < 10.01) return 0.150;
            if (v < 18.01) return 0.180;
            if (v < 30.01) return 0.210;
            if (v < 50.01) return 0.250;
            if (v < 80.01) return 0.300;
            if (v < 120.01) return 0.350;
            if (v < 180.01) return 0.400;
            if (v < 250.01) return 0.460;
            if (v < 315.01) return 0.520;
            if (v < 400.01) return 0.570;
            if (v < 500.01) return 0.630;
            if (v < 630.01) return 0.700;
            if (v < 800.01) return 0.800;
            if (v < 1000.01) return 0.900;
            if (v < 1250.01) return 1.050;
            if (v < 1600.01) return 1.250;
            if (v < 2000.01) return 1.500;
            if (v < 2500.01) return 1.750;
            return 2.100;
        }

        private static double H8Tol(double v)
        {
            if (v < 3.01) return 0.014;
            if (v < 6.01) return 0.018;
            if (v < 10.01) return 0.022;
            if (v < 18.01) return 0.027;
            if (v < 30.01) return 0.033;
            if (v < 50.01) return 0.039;
            if (v < 80.01) return 0.046;
            if (v < 120.01) return 0.054;
            if (v < 180.01) return 0.063;
            if (v < 250.01) return 0.072;
            if (v < 315.01) return 0.081;
            if (v < 400.01) return 0.089;
            if (v < 500.01) return 0.097;
            if (v < 630.01) return 0.110;
            if (v < 800.01) return 0.125;
            if (v < 1000.01) return 0.140;
            if (v < 1250.01) return 0.165;
            if (v < 1600.01) return 0.195;
            if (v < 2000.01) return 0.230;
            if (v < 2500.01) return 0.280;
            return 0.330;
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

        private static double F7Pos(double v)
        {
            if (v < 3.01) return 0.006;
            if (v < 6.01) return 0.010;
            if (v < 10.01) return 0.013;
            if (v < 18.01) return 0.016;
            if (v < 30.01) return 0.020;
            if (v < 50.01) return 0.025;
            if (v < 80.01) return 0.030;
            if (v < 120.01) return 0.036;
            if (v < 180.01) return 0.043;
            if (v < 250.01) return 0.050;
            if (v < 315.01) return 0.056;
            if (v < 400.01) return 0.062;
            if (v < 500.01) return 0.068;
            if (v < 630.01) return 0.076;
            if (v < 800.01) return 0.080;
            if (v < 1000.01) return 0.086;
            if (v < 1250.01) return 0.098;
            if (v < 1600.01) return 0.110;
            return 0.120;
        }

        private static double F7Neg(double v)
        {
            if (v < 3.01) return 0.016;
            if (v < 6.01) return 0.022;
            if (v < 10.01) return 0.028;
            if (v < 18.01) return 0.034;
            if (v < 30.01) return 0.041;
            if (v < 50.01) return 0.050;
            if (v < 80.01) return 0.060;
            if (v < 120.01) return 0.071;
            if (v < 180.01) return 0.083;
            if (v < 250.01) return 0.096;
            if (v < 315.01) return 0.108;
            if (v < 400.01) return 0.119;
            if (v < 500.01) return 0.131;
            if (v < 630.01) return 0.146;
            if (v < 800.01) return 0.160;
            if (v < 1000.01) return 0.176;
            if (v < 1250.01) return 0.203;
            if (v < 1600.01) return 0.235;
            return 0.270;
        }

        private static double GenTol(double v)
        {
            if (v < 6.01) return 0.1;
            if (v < 30.01) return 0.2;
            if (v < 120.01) return 0.3;
            if (v < 315.01) return 0.5;
            if (v < 1000.01) return 0.8;
            if (v < 2000.01) return 1.2;
            return 2.0;
        }

        private static string Fmt(double v) => v.ToString(CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt0(double v) => v.ToString("F0", CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt1(double v) => v.ToString("F1", CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt2(double v) => v.ToString("F2", CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt3(double v) => v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
    }
}