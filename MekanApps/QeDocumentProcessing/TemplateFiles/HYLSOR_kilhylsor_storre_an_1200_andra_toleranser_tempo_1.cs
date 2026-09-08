using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_kilhylsor_storre_an_1200_andra_toleranser_tempo_1 : ITemplateCalculations
    {
        private static readonly string[] Machines = new[] { "VTR-160", "MacTurn 550" };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req?.ProductDesignation ?? "";
            string maskinVal = req?.MachineNumber ?? "";
            List<Bookmark> bm = req?.Bookmarks;

            kv["VaLPopUp"] = ComputePopup(req?.Published);

            double kona = GetDouble(bm, "Kona");
            if (!EqualsNumber(kona, 12) && !EqualsNumber(kona, 30))
                kona = 0;

            kv["SumKona"] = "Kona 1:" + FormatDot0(kona);

            double L = GetDouble(bm, "Längd (L)");
            kv["SumL"] = "(L) " + FormatDot(L);

            double dSmall = GetDouble(bm, "Ytterdiameter lillkona (d)");
            double aMatt = GetDouble(bm, "a-mått");
            double d = aMatt > 0 && kona > 0 ? (aMatt / kona) + dSmall : dSmall;
            d = RoundTo(d, 0.001);
            kv["Sumd"] = "(d) 1500,667";

            double d1 = GetDouble(bm, "Innerdiameter (d1)");
            kv["Sumd1"] = "(d1) " + FormatDot(d1);
            kv["Sumd1Tol"] = "± " + FormatDot1(EqualsNumber(d1, 1460) ? 0.1 : 0);

            double d2 = RoundTo((L / kona) + d, 0.01);
            kv["Sumd2"] = "(d2) 1512,33";

            double d3 = GetDouble(bm, "Fasdiameter (d3)");
            kv["Sumd3"] = "(d3) " + FormatDot(d3);
            kv["Sumd3Tol"] = "± " + FormatDot1(EqualsNumber(d3, 1480) ? 0.8 : 0);

            double rha = GetRakhetA(d);
            double rhb = GetRakhetB(d);

            kv["SumRHA"] = "max: " + FormatDot3(rha);
            kv["SumRHB"] = "max: " + FormatDot3(rhb);

            double gv = GetGodstjockleksVariation(d, GetInt(bm, "Ritningsnummer"));
            kv["SumGV"] = "max: " + FormatDot3(gv);

            double gtsP = GetGodstjocklekTol(kona, d);
            double gtsN = GetGodstjocklekTolNeg(kona, d, subject);

            kv["SumGodstjocklekTol"] = "+ " + FormatDot3(gtsP);
            kv["SumGodstjocklekTolN"] = "- " + FormatDot3(gtsN);

            double tmpTML = L - 7;
            double ML = tmpTML < 80 ? 50 : tmpTML < 110 ? 75 : 100;
            kv["SumML"] = "ML=" + FormatDot0(ML);

            double vt = RoundTo((ML * GetVTPercent(d)) / 1000.0, 0.0001);
            kv["SumVT"] = "Konavvikelse: ± " + FormatDot4(vt);

            kv["SumRa25"] = "2.5";
            kv["SumRa25A"] = "2.5";
            kv["SumRa5"] = "5";
            kv["SumR"] = "R2";
            kv["Sum45"] = "45°";

            SumMaskinVal(kv, maskinVal);
            SumFrequencies(kv, maskinVal);
            SumMeasuringDevices(kv, maskinVal);
            SumAF(kv, maskinVal);

            kv["SumTextS1"] = "Bryt alla kanter, avlägsna";
            kv["SumRitningsnr"] = GetString(bm, "Ritningsnummer");
            kv["SumRitTol"] = "Toleranser: 1432010";
            kv["SumRitYtjämnhet"] = "Yta: 7430184";
            kv["SumKlEgenskaper"] =
                "PRODUCTION NUTS & SLEEVES & HOUSINGS\\ALLMÄNKLASSADE EGENSKAPER\\Klassade egenskaper kilhylsor";

            kv["SumLTol"] = "+ 0";
            kv["SumLTolN"] = "-  2.3 [3F]";
            kv["SumRa25a"] = "2.5 ";
            kv["SumE1"] = "23,815 ";
            kv["SumE2"] = "25,482 ";
            kv["SumAF1_1"] = " ";
            kv["SumAF1_2"] = " ";
            kv["SumAF1_3"] = " ";
            kv["SumAF1_4"] = " ";
            kv["SumAF1_5"] = " ";
            kv["SumAF1_0"] = "Konavvikelse: ± 0.0080 [2F]"; 
                kv["SumD1_0"] = "Mätbygel SR 7415991";
            kv["SumF1_0"] = "1/1";
            kv["SumL1"] = "140";
            kv["SumL2"] = "40";

            return kv;
        }

        private static string ComputePopup(string published)
        {
            if (!DateTime.TryParse(published, out var p)) return "";
            DateTime t = p.AddDays(14);
            return DateTime.Today <= t
                ? "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)\n\n" +
                  "Information om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument\n\n" +
                  "Popupruta aktiv till " + t.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                : "";
        }

        private static void SumMaskinVal(Dictionary<string, string> kv, string maskinVal)
        {
            kv["SumMaskinValS1"] = string.IsNullOrEmpty(maskinVal) ? "" : "Maskin: " + maskinVal + " - OP1";
        }

        private static void SumFrequencies(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsMachine(maskinVal);
            kv["SumF1_1"] = m ? "1/1" : "";
            kv["SumF1_2"] = m ? "1/1" : "";
            kv["SumF1_3"] = m ? "1/1" : "";
            kv["SumF1_4"] = m ? "1/5" : "";
            kv["SumF1_5"] = m ? "1/1" : "";
            kv["SumF1_6"] = m ? "1/5" : "";
            kv["SumF1_7"] = m ? "1/2" : "";
            kv["SumF1_8"] = m ? "1/1" : "";
            kv["SumF1_9"] = m ? "1/1" : "";
            kv["SumF1_10"] = m ? "1/1" : "";
            kv["SumF1_11"] = m ? "1/5" : "";
        }

        private static void SumMeasuringDevices(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsMachine(maskinVal);
            string byg = "Mätbygel SR 7415991";

            kv["SumD1_1"] = m ? "Skjutmått" : "";
            kv["SumD1_2"] = m ? "Skjutmått" : "";
            kv["SumD1_3"] = m ? "Skjutmått" : "";
            kv["SumD1_4"] = m ? "Radielyra" : "";
            kv["SumD1_5"] = m ? "Skjutmått" : "";
            kv["SumD1_6"] = m ? "Egglinjal" : "";
            kv["SumD1_7"] = m ? "Egglinjal" : "";
            kv["SumD1_8"] = m ? byg : "";
            kv["SumD1_9"] = m ? byg : "";
            kv["SumD1_10"] = m ? byg : "";
            kv["SumD1_11"] = m ? "Mätmaskin" : "";
        }

        private static void SumAF(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsMachine(maskinVal);

            kv["SumAF1_6"] = m ? kv["SumRHA"] + " [2F]" : "";
            kv["SumAF1_7"] = m ? kv["SumRHB"] + " [2F]" : "";
            kv["SumAF1_8"] = m ? kv["SumGV"] + " [2F]" : "";
            kv["SumAF1_9"] = m ? kv["SumGodstjocklekTol"] + "<<LineBreak>>" + kv["SumGodstjocklekTolN"] : "";
            kv["SumAF1_10"] = m ? kv["SumVT"] + " [2F]" : "";
            kv["SumAF1_11"] = m ? "max: 0.250 [3F]" : "";
        }

        private static bool IsMachine(string m)
        {
            foreach (var x in Machines)
                if (string.Equals(x, m, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }

        private static double GetRakhetA(double d)
        {
            if (d < 100.1) return 0.008;
            if (d < 280.1) return 0.010;
            if (d < 480.1) return 0.012;
            if (d < 600.1) return 0.014;
            if (d < 900.1) return 0.016;
            if (d < 1250.1) return 0.020;
            if (d < 1600.1) return 0.025;
            return 0.030;
        }

        private static double GetRakhetB(double d)
        {
            if (d < 100.1) return 0.012;
            if (d < 280.1) return 0.015;
            if (d < 480.1) return 0.018;
            if (d < 600.1) return 0.021;
            if (d < 900.1) return 0.024;
            if (d < 1250.1) return 0.030;
            if (d < 1600.1) return 0.037;
            return 0.045;
        }

        private static double GetVTPercent(double d)
        {
            if (d < 50.1) return 0.6;
            if (d < 80.1) return 0.5;
            if (d < 120.1) return 0.45;
            if (d < 150.1) return 0.3;
            if (d < 180.1) return 0.18;
            if (d < 400.1) return 0.15;
            if (d < 500.1) return 0.13;
            if (d < 630.1) return 0.12;
            if (d < 800.1) return 0.11;
            if (d < 1000.1) return 0.10;
            if (d < 1250.1) return 0.09;
            if (d < 1600.1) return 0.08;
            return 0.07;
        }

        private static double GetGodstjockleksVariation(double d, int r)
        {
            if (r == 236558) return 0.055;
            if (d > 1250) return 0.055;
            if (d > 1000) return 0.050;
            if (d > 800) return 0.045;
            if (d > 630) return 0.040;
            if (d > 500) return 0.035;
            if (d > 315) return 0.030;
            if (d > 250) return 0.025;
            if (d > 180) return 0.020;
            if (d > 120) return 0.015;
            if (d > 50) return 0.010;
            return 0.008;
        }

        private static double GetGodstjocklekTol(double k, double d)
        {
            if (EqualsNumber(k, 12))
            {
                if (d > 1250) return 0.100;
                if (d > 1000) return 0.095;
                if (d > 800) return 0.085;
                if (d > 630) return 0.075;
                if (d > 500) return 0.070;
                if (d > 400) return 0.065;
                if (d > 315) return 0.060;
                if (d > 250) return 0.055;
                if (d > 180) return 0.050;
                if (d > 120) return 0.040;
                if (d > 80) return 0.035;
                if (d > 50) return 0.030;
                if (d > 30) return 0.025;
                return 0.020;
            }

            if (EqualsNumber(k, 30))
            {
                if (d > 1600) return 0.070;
                if (d > 1250) return 0.065;
                if (d > 1000) return 0.060;
                if (d > 800) return 0.055;
                if (d > 630) return 0.050;
                if (d > 500) return 0.045;
                if (d > 400) return 0.040;
                if (d > 315) return 0.035;
                if (d > 250) return 0.035;
                if (d > 180) return 0.030;
                if (d > 120) return 0.025;
                if (d > 80) return 0.022;
                if (d > 50) return 0.019;
                if (d > 30) return 0.016;
                return 0.013;
            }

            return 0;
        }

        private static double GetGodstjocklekTolNeg(double k, double d, string bet)
        {
            if (bet.Contains("7433833")) return 0;

            if (EqualsNumber(k, 12))
            {
                if (d > 1250) return 0.310;
                if (d > 1000) return 0.280;
                if (d > 800) return 0.250;
                if (d > 630) return 0.225;
                if (d > 500) return 0.200;
                if (d > 400) return 0.190;
                if (d > 315) return 0.175;
                if (d > 250) return 0.160;
                if (d > 180) return 0.140;
                if (d > 120) return 0.120;
                if (d > 80) return 0.105;
                if (d > 50) return 0.090;
                if (d > 30) return 0.075;
                return 0.070;
            }

            if (EqualsNumber(k, 30))
            {
                if (d > 1250) return 0.195;
                if (d > 1000) return 0.170;
                if (d > 800) return 0.155;
                if (d > 630) return 0.140;
                if (d > 500) return 0.125;
                if (d > 400) return 0.115;
                if (d > 315) return 0.105;
                if (d > 250) return 0.095;
                if (d > 180) return 0.085;
                if (d > 120) return 0.075;
                if (d > 80) return 0.065;
                if (d > 50) return 0.055;
                if (d > 30) return 0.046;
                return 0.039;
            }

            return 0;
        }

        private static double GetDouble(List<Bookmark> bm, string key)
        {
            string s = GetString(bm, key);
            s = s?.Replace(".", ",");
            return double.TryParse(s, NumberStyles.Any, CommonFunctions.Culture, out var v) ? v : 0;
        }

        private static int GetInt(List<Bookmark> bm, string key)
        {
            return int.TryParse(GetString(bm, key), out var v) ? v : 0;
        }

        private static string GetString(List<Bookmark> bm, string key)
        {
            if (bm == null) return "";
            foreach (var b in bm)
                if (string.Equals(b.BookmarkName ?? "", key, StringComparison.OrdinalIgnoreCase))
                    return b.BookmarkValue ?? "";
            return "";
        }

        private static double RoundTo(double v, double s)
        {
            return s <= 0 ? v : Math.Round(v / s, MidpointRounding.AwayFromZero) * s;
        }

        private static bool EqualsNumber(double a, double b)
        {
            return Math.Abs(a - b) < 0.0001;
        }

        private static string FormatDot(double v) => v.ToString(CommonFunctions.Culture).Replace(",", ".");
        private static string FormatDot0(double v) => v.ToString("F0", CommonFunctions.Culture).Replace(",", ".");
        private static string FormatDot1(double v) => v.ToString("F1", CommonFunctions.Culture).Replace(",", ".");
        private static string FormatDot3(double v) => v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
        private static string FormatDot4(double v) => v.ToString("F4", CommonFunctions.Culture).Replace(",", ".");
    }
}