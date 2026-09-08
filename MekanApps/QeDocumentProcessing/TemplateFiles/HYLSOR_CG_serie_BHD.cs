using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_CG_serie_BHD : ITemplateCalculations
    {
        private static readonly string[] Machines = new[] { "VTR-160", "MacTurn 550" };

        private const double D1Val = 803.0;
        private const double D2Val = 756.129;
        private const double D3Val = 731.35;
        private const double D4Val = 743.0;
        private const double D5Val = 871.0;
        private const double D6Val = 5.0;
        private const double B1Val = 79.0;
        private const double B2Val = 70.0;
        private const double B3Val = 98.0;
        private const double B4Val = 137.0;
        private const double B5Val = 19.0;
        private const double B6Val = 11.0;
        private const double B7Val = 1.0;
        private const double B71Val = 1.0;
        private const double B8Val = 31.5;
        private const double B9Val = 19.0;
        private const double H1Val = 67.0;
        private const double H2Val = 118.0;
        private const double H3Val = 39.0;
        private const double B10Val = 4.0;
        private const double GVal = 5.0;
        private const double F1Val = 1.0;
        private const double R3Val = 3.0;

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");

            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : string.Empty;

            string tmpSerie = tmpBet3.Length >= 2 ? tmpBet3.Substring(0, 2) : tmpBet3;
            string tmpTyp = tmpBet3.Length >= 2 ? tmpBet3.Substring(tmpBet3.Length - 2) : tmpBet3;

            bool typMatch = string.Equals(tmpTyp, "08", StringComparison.OrdinalIgnoreCase);

            kv["SumD1"] = "(D1) " + Fmt(D1Val);
            kv["SumD1Tol"] = "+ " + Fmt3(0.0);
            kv["SumD1TolN"] = "- " + Fmt3(H10TolN(D1Val));

            kv["SumD2"] = "(D2) " + Fmt(D2Val);
            kv["SumD2Tol"] = "+ " + Fmt3(0.0);
            kv["SumD2TolN"] = "- " + Fmt3(0.080);

            string sumD3Tol = "+ " + Fmt3(H11Tol(D3Val));
            string sumD3TolN = "- " + Fmt3(0.0);
            kv["SumD3"] = "(D3) " + Fmt(D3Val);
            kv["SumD3Tol"] = sumD3Tol;
            kv["SumD3TolN"] = "-0.0";
            kv["SumD3a"] = "(D3) " + Fmt(D3Val);
            kv["SumD3aTol"] = sumD3Tol;
            kv["SumD3aTolN"] = "-0.0";

            kv["SumD4"] = "(D4) " + Fmt(D4Val);
            kv["SumD4Tol"] = "+ " + Fmt3(1.0);
            kv["SumD4TolN"] = "- " + Fmt3(0.0);

            kv["SumD5"] = "(D5) " + Fmt(D5Val);
            kv["SumD5Tol"] = "+ 0.0";
            kv["SumD5TolN"] = "- " + Fmt3(H11Tol(D5Val));

            kv["SumD6"] = "(D6) 1x Ø" + Fmt(D6Val);

            kv["SumB1"] = "(B1) " + Fmt(B1Val);
            kv["SumB1Tol"] = "+ " + Fmt3(0.5);
            kv["SumB1TolN"] = "- " + Fmt3(0.0);

            kv["SumB2"] = "(B2) " + Fmt(B2Val);
            kv["SumB2Tol"] = "+ " + Fmt3(0.5);
            kv["SumB2TolN"] = "- " + Fmt3(0.0);

            kv["SumB3"] = "(B3) " + Fmt(B3Val);
            kv["SumB3Tol"] = "+ " + Fmt3(0.0);
            kv["SumB3TolN"] = "- " + Fmt3(0.5);

            kv["SumB4"] = "(B4) " + Fmt(B4Val);
            kv["SumB4Tol"] = "+ " + Fmt3(0.0);
            kv["SumB4TolN"] = "- " + Fmt3(0.5);

            kv["SumB5"] = "(B5) " + Fmt(B5Val);
            kv["SumB5Tol"] = "+ " + Fmt3(0.0);
            kv["SumB5TolN"] = "- " + Fmt3(0.5);

            kv["SumB6"] = "(B6) " + Fmt(B6Val);
            kv["SumB6Tol"] = "+ " + Fmt3(4.0);
            kv["SumB6TolN"] = "- " + Fmt3(0.0);

            kv["SumB7"] = "(B7) " + Fmt(B7Val);
            kv["SumB7Tol"] = "+ " + Fmt3(0.15);
            kv["SumB7TolN"] = "- " + Fmt3(0.0);

            kv["SumB71"] = Fmt(B71Val);
            kv["SumB71Tol"] = "+ " + Fmt3(0.15);
            kv["SumB71TolN"] = "- " + Fmt3(0.0);

            kv["SumB8"] = "(B8) " + Fmt(B8Val);
            kv["SumB8Tol"] = "+ " + Fmt3(0.0);
            kv["SumB8TolN"] = "- " + Fmt3(0.5);

            kv["SumB9"] = "(B9) " + Fmt(B9Val);
            kv["SumB9Tol"] = "+ " + Fmt3(0.5);
            kv["SumB9TolN"] = "- " + Fmt3(0.0);

            kv["SumH3"] = "(H3) " + Fmt(H3Val);
            kv["SumH3Tol"] = "+ " + Fmt3(0.0);
            kv["SumH3TolN"] = "- " + Fmt3(0.5);

            kv["SumB10"] = "(B10) " + Fmt(B10Val);
            kv["SumB10Tol"] = "+ " + Fmt3(0.0);
            kv["SumB10TolN"] = "- " + Fmt3(1.0);

            kv["SumG"] = "6x M" + Fmt(GVal) + " Lika delning";

            kv["SumH1"] = "(H1) " + Fmt(H1Val);
            kv["SumH1Tol"] = "+ " + Fmt3(0.0);
            kv["SumH1TolN"] = "- " + Fmt3(1.0);

            kv["SumH2"] = "(H2) " + Fmt(H2Val);
            kv["SumH2Tol"] = "+ " + Fmt3(0.5);
            kv["SumH2TolN"] = "- " + Fmt3(0.5);

            kv["SumF1"] = Fmt(F1Val);
            kv["SumF1a"] = Fmt(F1Val);

            kv["SumR3"] = "R " + Fmt(R3Val);
            kv["SumR3a"] = kv["SumR3"];
            kv["SumR3b"] = kv["SumR3"];
            kv["SumR3C"] = kv["SumR3"];

            kv["SumRa1"] = "1";
            kv["SumRa1a"] = "1";

            kv["SumPl"] = "0.030";
            kv["SumP1"] = "0.050";
            kv["SumP2"] = "0.050";

            kv["SumMaskinValS1"] = "Maskin: " + maskinVal + " - Svarvning";
            kv["SumMaskinValS2"] = "Maskin: " + maskinVal + " - Svarv, Borr & Fräsning";

            SumFrequencies(kv, maskinVal);
            SumMeasuringDevices(kv, maskinVal);
            SumAF(kv, maskinVal);

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
            kv["SumF1_1"] = m ? "1/1" : "";
            kv["SumF1_2"] = m ? "1/1" : "";
            kv["SumF1_3"] = m ? "Inst." : "";
            kv["SumF1_4"] = m ? "1/1" : "";
            kv["SumF1_5"] = m ? "1/1" : "";
            kv["SumF1_6"] = m ? "" : "";
            kv["SumF1_7"] = m ? "" : "";
            kv["SumF1_8"] = m ? "" : "";
            kv["SumF1_9"] = m ? "vid behov" : "";
            kv["SumF1_0"] = m ? "vid behov" : "";

            kv["SumF2_1"] = m ? "1/1" : "";
            kv["SumF2_2"] = m ? "1/1" : "";
            kv["SumF2_3"] = m ? "1/1" : "";
            kv["SumF2_4"] = m ? "Inst." : "";
            kv["SumF2_5"] = m ? "Inst." : "";
            kv["SumF2_6"] = m ? "Inst." : "";
            kv["SumF2_7"] = m ? "" : "";
            kv["SumF2_8"] = m ? "" : "";
            kv["SumF2_9"] = m ? "" : "";
            kv["SumF2_0"] = m ? "" : "";
        }

        private static void SumMeasuringDevices(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsMachine(maskinVal);
            kv["SumD1_1"] = m ? "Skjutmått" : "";
            kv["SumD1_2"] = m ? "Mikrometerstickmått" : "";
            kv["SumD1_3"] = m ? "Skjutmått" : "";
            kv["SumD1_4"] = m ? "Bygelmikrometer" : "";
            kv["SumD1_5"] = m ? "Skjutmått" : "";
            kv["SumD1_6"] = m ? "" : "";
            kv["SumD1_7"] = m ? "" : "";
            kv["SumD1_8"] = m ? "" : "";
            kv["SumD1_9"] = m ? "Mätmaskin" : "";
            kv["SumD1_0"] = m ? "Mätmaskin" : "";

            kv["SumD2_1"] = m ? "Skjutmått" : "";
            kv["SumD2_2"] = m ? "Skjutmått" : "";
            kv["SumD2_3"] = m ? "Skjutmått" : "";
            kv["SumD2_4"] = m ? "Djuphakmått" : "";
            kv["SumD2_5"] = m ? "Djuphakmått" : "";
            kv["SumD2_6"] = m ? "Djuphakmått" : "";
            kv["SumD2_7"] = m ? "" : "";
            kv["SumD2_8"] = m ? "" : "";
            kv["SumD2_9"] = m ? "" : "";
            kv["SumD2_0"] = m ? "" : "";
        }

        private static void SumAF(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsMachine(maskinVal);
            kv["SumAF1_1"] = "";
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = "";
            kv["SumAF1_6"] = "";
            kv["SumAF1_7"] = "";
            kv["SumAF1_8"] = "";
            kv["SumAF1_9"] = m ? "Vid misstänkt formfel lämna till mätrum" : "";
            kv["SumAF1_0"] = m ? "Vid misstänkt formfel lämna till mätrum" : "";

            kv["SumAF2_1"] = "";
            kv["SumAF2_2"] = "";
            kv["SumAF2_3"] = "";
            kv["SumAF2_4"] = "";
            kv["SumAF2_5"] = "";
            kv["SumAF2_6"] = "";
            kv["SumAF2_7"] = "";
            kv["SumAF2_8"] = "";
            kv["SumAF2_9"] = "";
            kv["SumAF2_0"] = "";
        }

        private static bool IsMachine(string maskinVal)
        {
            if (string.IsNullOrEmpty(maskinVal)) return false;
            for (int i = 0; i < Machines.Length; i++)
                if (string.Equals(Machines[i], maskinVal, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }

        private static double H10TolN(double d)
        {
            if (d < 3.01) return 0.040;
            if (d < 6.01) return 0.048;
            if (d < 10.01) return 0.058;
            if (d < 18.01) return 0.070;
            if (d < 30.01) return 0.084;
            if (d < 50.01) return 0.100;
            if (d < 80.01) return 0.120;
            if (d < 120.01) return 0.140;
            if (d < 180.01) return 0.160;
            if (d < 250.01) return 0.185;
            if (d < 315.01) return 0.210;
            if (d < 400.01) return 0.230;
            if (d < 500.01) return 0.250;
            if (d < 630.01) return 0.280;
            if (d < 800.01) return 0.320;
            if (d < 1000.01) return 0.360;
            if (d < 1250.01) return 0.420;
            if (d < 1600.01) return 0.500;
            if (d < 2000.01) return 0.600;
            if (d < 2500.01) return 0.700;
            return 0.860;
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

        private static string Fmt(double v)
        {
            return v.ToString(CommonFunctions.Culture).Replace(",", ".");
        }

        private static string Fmt3(double v)
        {
            return v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
        }
    }
}