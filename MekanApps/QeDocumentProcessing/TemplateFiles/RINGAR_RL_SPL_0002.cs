using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class RINGAR_RL_SPL_0002 : ITemplateCalculations
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

            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);

            string tmpBet4Text = tokens.Length > 3 ? tokens[3] : string.Empty;

            string tmpSerie = tmpBet4Text.Length >= 2 ? tmpBet4Text.Substring(0, 2) : tmpBet4Text;
            string tmpTyp = tmpBet4Text.Length >= 2 ? tmpBet4Text.Substring(tmpBet4Text.Length - 2) : tmpBet4Text;

            double inD1 = GetDouble(bm, "Diameter (D1)");
            double inD2 = GetDouble(bm, "Diameter (D2)");
            double inD3 = GetDouble(bm, "Diameter (D3)");
            double inG = GetDouble(bm, "Gänga (G)");
            double inR1 = GetDouble(bm, "Radie x6");
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
            kv["SumD1Tol"] = "± " + FormatDot3(GeneralTol(tmpD1));

            kv["SumD2"] = "(D2) " + FormatDot(tmpD2);
            kv["SumD2Tol"] = "+ " + FormatDot3(D2F8PosTol(tmpD2));
            kv["SumD2TolN"] = "- " + FormatDot3(D2F8NegTol(tmpD2));

            kv["SumD3"] = "(D3) " + FormatDot(tmpD3);
            kv["SumD3Tol"] = "+ " + FormatDot3(D3H12Tol(tmpD3));
            kv["SumD3TolN"] = "- " + FormatDot(0.0);

            string gRaw = GetString(bm, "Gänga (G)");
            string tmpG = string.IsNullOrEmpty(gRaw) || gRaw == "0" ? string.Empty : gRaw;
            kv["SumG"] = "2x " + tmpG;

            double tmpR1val = inR1 == 0 ? 0.5 : inR1;
            kv["SumR1"] = "6x R" + FormatDot1(tmpR1val);

            kv["SumB"] = "(B) " + FormatDot(tmpB);
            kv["SumBTol"] = "± " + FormatDot3(GeneralTol(tmpB));

            kv["SumB1"] = "(B1) " + FormatDot(tmpB1);
            kv["SumB1Tol"] = "± " + FormatDot3(GeneralTol(tmpB1));

            kv["SumB2"] = "(B2) " + FormatDot(tmpB2);
            kv["SumB2Tol"] = "± " + FormatDot3(GeneralTol(tmpB2));

            kv["SumRa32"] = "3.2";
            kv["SumRa32a"] = "3.2";

            SumMaskinVal(kv, maskinVal);
            SumFrequencies(kv, maskinVal);
            SumMeasuringDevices(kv, maskinVal);
            SumAF(kv, maskinVal);

            kv["SumTextS1"] = "Okulär kontroll av bearbetade ytor, vid misstänkt formfel lämnas hylsan till mätrum för kontroll." +
                              "\nSkarpa kanter avgradas.";

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
            kv["SumD1_5"] = match ? "Gängtolk" : "";
            kv["SumD1_6"] = match ? "Radielyra" : "";
            kv["SumD1_7"] = match ? "Ytjämnhetsmätare" : "";
        }

        private static void SumAF(Dictionary<string, string> kv, string maskinVal)
        {
            bool match = IsMachine(maskinVal);
            kv["SumAF1_1"] = "";
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = match ? "Delningsvinkel 120°" : "";
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

        private static double D2F8PosTol(double d)
        {
            if (d < 3.01) return 0.020;
            if (d < 6.01) return 0.028;
            if (d < 10.01) return 0.035;
            if (d < 18.01) return 0.043;
            if (d < 30.01) return 0.053;
            if (d < 50.01) return 0.064;
            if (d < 80.01) return 0.076;
            if (d < 120.01) return 0.090;
            if (d < 180.01) return 0.106;
            if (d < 250.01) return 0.122;
            if (d < 315.01) return 0.137;
            if (d < 400.01) return 0.151;
            if (d < 500.01) return 0.165;
            return 0.186;
        }

        private static double D2F8NegTol(double d)
        {
            if (d < 3.01) return 0.006;
            if (d < 6.01) return 0.010;
            if (d < 10.01) return 0.013;
            if (d < 18.01) return 0.016;
            if (d < 30.01) return 0.020;
            if (d < 50.01) return 0.025;
            if (d < 80.01) return 0.030;
            if (d < 120.01) return 0.036;
            if (d < 180.01) return 0.043;
            if (d < 250.01) return 0.050;
            if (d < 315.01) return 0.056;
            if (d < 400.01) return 0.062;
            if (d < 500.01) return 0.068;
            return 0.076;
        }

        private static double D3H12Tol(double d)
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