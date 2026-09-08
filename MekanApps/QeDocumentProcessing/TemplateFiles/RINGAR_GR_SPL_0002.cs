using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class RINGAR_GR_SPL_0002 : ITemplateCalculations
    {
        private static readonly string[] Machines = new[]
        {
            "Nakamura/K&T", "MaxMuller/K&T", "LB45", "MacTurn 550"
        };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;
            List<Bookmark> bm = req != null ? req.Bookmarks : null;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");

            double inD1 = GetDouble(bm, "Diameter (D1)");
            double inD2 = GetDouble(bm, "Diameter (D2)");
            double inD3 = GetDouble(bm, "Diameter (D3)");
            double inR1 = GetDouble(bm, "Radie x8");
            double inB = GetDouble(bm, "Bredd (B)");
            double inB1 = GetDouble(bm, "Bredd (B1)");
            double inB2 = GetDouble(bm, "Bredd (B2)");

            double tmpD1 = inD1 == 0 ? 0 : inD1;
            double tmpD2 = inD2 == 0 ? 0 : inD2;
            double tmpD3 = inD3 == 0 ? 0 : inD3;
            double tmpB = inB == 0 ? 0 : inB;
            double tmpB1 = inB1 == 0 ? 0 : inB1;
            double tmpB2 = inB2 == 0 ? 0 : inB2;

            kv["SumD1"] = "(D1) " + FormatDot(tmpD1);
            kv["SumD1Tol"] = "+ " + FormatDot3(D1H13PosTol(tmpD1));
            kv["SumD1TolN"] = "- 0.0";

            kv["SumD2"] = "(D2) " + FormatDot(tmpD2);
            kv["SumD2Tol"] = "+ 0.0";
            kv["SumD2TolN"] = "- " + FormatDot3(D2h11NegTol(tmpD2));

            kv["SumD3"] = "(D3) " + FormatDot(tmpD3);
            kv["SumD3Tol"] = "+ 0.0";
            kv["SumD3TolN"] = "- " + FormatDot3(D3h13NegTol(tmpD3));

            double tmpR1val = inR1 == 0 ? 0.5 : inR1;
            kv["SumR1"] = "8x R" + FormatDot1(tmpR1val);

            kv["SumB"] = "(B) " + FormatDot(tmpB);
            kv["SumBTol"] = "± " + FormatDot3(GeneralTol(tmpB));

            kv["SumB1"] = "(B1) " + FormatDot(tmpB1);
            kv["SumB1Tol"] = "+ " + FormatDot3(B1H12PosTol(tmpB1));
            kv["SumB1TolN"] = "- " + FormatDot(0.0);

            kv["SumB2"] = "(B2) " + FormatDot(tmpB2);
            kv["SumB2Tol"] = "± " + FormatDot3(GeneralTol(tmpB2));

            kv["SumRa32"] = "3.2";
            kv["SumRa18"] = "1.8";

            SumMaskinVal(kv, maskinVal);
            SumFrequencies(kv, maskinVal);
            SumMeasuringDevices(kv, maskinVal);
            SumAF(kv, maskinVal);

            kv["SumTextS1"] = "Okulär kontroll av bearbetade ytor, vid misstänkt formfel lämnas hylsan till mätrum för kontroll." +
                              "\nSkarpa kanter avgradas." +
                              "\nKontrollera att märkningen är rätt & tydlig";

            kv["SumRitS1"] = tmpBet + ": senaste utgåva";

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
            string s1 =
                EqualsI(maskinVal, "Nakamura/K&T") ? "Nakamura" :
                EqualsI(maskinVal, "MaxMuller/K&T") ? "MaxMuller" :
                EqualsI(maskinVal, "LB45") ? "LB45" :
                EqualsI(maskinVal, "MacTurn 550") ? "MacTurn 550" : "";

            string s2 =
                EqualsI(maskinVal, "Nakamura/K&T") ? "K&T" :
                EqualsI(maskinVal, "MaxMuller/K&T") ? "K&T" :
                EqualsI(maskinVal, "LB45") ? "LB45" :
                EqualsI(maskinVal, "MacTurn 550") ? "MacTurn 550" : "";

            kv["SumMaskinValS1"] = ("Maskin: " + s1 + " - Svarvning").Trim();
            kv["SumMaskinValS2"] = ("Maskin: " + s2 + " - Borrning, Fräsning").Trim();
        }

        private static void SumFrequencies(Dictionary<string, string> kv, string maskinVal)
        {
            bool match = IsMachine(maskinVal);
            kv["SumF1_1"] = match ? "1/2" : "";
            kv["SumF1_2"] = match ? "1/2" : "";
            kv["SumF1_3"] = match ? "1/2" : "";
            kv["SumF1_4"] = match ? "1/2" : "";
            kv["SumF1_5"] = match ? "Inst." : "";
            kv["SumF1_6"] = match ? "Inst." : "";
            kv["SumF1_7"] = match ? "Inst." : "";
        }

        private static void SumMeasuringDevices(Dictionary<string, string> kv, string maskinVal)
        {
            bool match = IsMachine(maskinVal);
            kv["SumD1_1"] = match ? "Skjutmått" : "";
            kv["SumD1_2"] = match ? "Skjutmått" : "";
            kv["SumD1_3"] = match ? "Skjutmått" : "";
            kv["SumD1_4"] = match ? "Skjutmått" : "";
            kv["SumD1_5"] = match ? "Skjutmått" : "";
            kv["SumD1_6"] = match ? "Radielyra" : "";
            kv["SumD1_7"] = match ? "Ytjämnhetsmätare" : "";
        }

        private static void SumAF(Dictionary<string, string> kv, string maskinVal)
        {
            kv["SumAF1_1"] = "";
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = "";
            kv["SumAF1_6"] = "";
            kv["SumAF1_7"] = "";
        }

        private static bool IsMachine(string maskinVal)
        {
            if (string.IsNullOrEmpty(maskinVal)) return false;
            for (int i = 0; i < Machines.Length; i++)
                if (string.Equals(Machines[i], maskinVal, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }

        private static double GeneralTol(double v)
        {
            if (v < 6.01) return 0.1;
            if (v < 30.01) return 0.2;
            if (v < 120.01) return 0.3;
            if (v < 400.01) return 0.5;
            if (v < 1000.01) return 0.8;
            if (v < 2000.01) return 1.2;
            return 2.0;
        }

        private static double D1H13PosTol(double d)
        {
            if (d < 3.01) return 0.140;
            if (d < 6.01) return 0.180;
            if (d < 10.01) return 0.220;
            if (d < 18.01) return 0.270;
            if (d < 30.01) return 0.330;
            if (d < 50.01) return 0.390;
            if (d < 80.01) return 0.460;
            if (d < 120.01) return 0.540;
            if (d < 180.01) return 0.630;
            if (d < 250.01) return 0.720;
            if (d < 315.01) return 0.810;
            if (d < 400.01) return 0.890;
            if (d < 500.01) return 0.970;
            if (d < 630.01) return 1.100;
            if (d < 800.01) return 1.250;
            if (d < 1000.01) return 1.400;
            if (d < 1250.01) return 1.650;
            if (d < 1600.01) return 1.950;
            if (d < 2000.01) return 2.300;
            if (d < 2500.01) return 2.800;
            return 3.300;
        }

        private static double D2h11NegTol(double d)
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

        private static double D3h13NegTol(double d)
        {
            return D1H13PosTol(d);
        }

        private static double B1H12PosTol(double d)
        {
            if (d < 3.01) return 0.100;
            if (d < 6.01) return 0.120;
            if (d < 10.01) return 0.150;
            if (d < 18.01) return 0.180;
            if (d < 30.01) return 0.210;
            if (d < 50.01) return 0.250;
            if (d < 80.01) return 0.300;
            if (d < 120.01) return 0.350;
            if (d < 180.01) return 0.400;
            if (d < 250.01) return 0.460;
            if (d < 315.01) return 0.520;
            if (d < 400.01) return 0.570;
            if (d < 500.01) return 0.630;
            if (d < 630.01) return 0.700;
            if (d < 800.01) return 0.800;
            if (d < 1000.01) return 0.900;
            if (d < 1250.01) return 1.050;
            if (d < 1600.01) return 1.250;
            if (d < 2000.01) return 1.500;
            if (d < 2500.01) return 1.750;
            return 2.100;
        }

        private static string FormatDot(double v)
        {
            return v.ToString(CommonFunctions.Culture).Replace(",", ".");
        }

        private static string FormatDot3(double v)
        {
            return v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
        }

        private static string FormatDot1(double v)
        {
            return v.ToString("F1", CommonFunctions.Culture).Replace(",", ".");
        }

        private static double GetDouble(List<Bookmark> bm, string key)
        {
            string raw = GetString(bm, key);
            if (string.IsNullOrWhiteSpace(raw)) return 0;
            raw = raw.Trim().Replace(".", ",");
            double v;
            return double.TryParse(raw, NumberStyles.Any, CommonFunctions.Culture, out v) ? v : 0;
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

        private static bool EqualsI(string a, string b)
            => string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);
    }
}