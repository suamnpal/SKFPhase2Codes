using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class RINGAR_SV_TSD_Frast_Spar : ITemplateCalculations
    {
        private static readonly string[] Machines = new[] { "Nakamura", "MaxMuller/Skepp6", "LB45" };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();
            string subject = req?.ProductDesignation ?? string.Empty;
            string maskinVal = req?.MachineNumber ?? string.Empty;
            List<Bookmark> bm = req?.Bookmarks;

            kv["VaLPopUp"] = ComputePopup(req?.Published);

            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");
            string[] tokens = SplitTokens(tmpBet);

            double tmpBet2 = ParseDoubleOrZero(GetTokenOrZero(tokens, 1));
            double tmpBet3 = ParseDoubleOrZero(GetTokenOrZero(tokens, 2));

            double dDef = 0;
            double d1Def = 0;
            double d2Def = 0;
            double d3Def = 0;
            double d4Def = 0;
            double d5Def = 0;

            if (EqualsNumber(tmpBet2, 3036))
            {
                if (EqualsNumber(tmpBet3, 172))
                {
                    dDef = 230;
                    d1Def = 230;
                    d2Def = 221;
                    d3Def = 207;
                    d4Def = 192.5;
                    d5Def = 215;
                }
                else if (EqualsNumber(tmpBet3, 187))
                {
                    dDef = 230;
                    d1Def = 230;
                    d2Def = 221;
                    d3Def = 207;
                    d4Def = 206.5;
                    d5Def = 215;
                }
            }

            double inD = GetDouble(bm, "Ø D");
            double inD1 = GetDouble(bm, "Ø D1");
            double inD2 = GetDouble(bm, "Ø D2");
            double inD3 = GetDouble(bm, "Ø D3");
            double inD4 = GetDouble(bm, "Ø D4");
            double inD5 = GetDouble(bm, "Ø D5");

            double tmpD = inD == 0 ? dDef : inD;
            double tmpD1 = inD1 == 0 ? d1Def : inD1;
            double tmpD2 = inD2 == 0 ? d2Def : inD2;
            double tmpD3 = inD3 == 0 ? d3Def : inD3;
            double tmpD4 = inD4 == 0 ? d4Def : inD4;
            double tmpD5 = inD5 == 0 ? d5Def : inD5;

            kv["SumD"] = "(D) " + FormatDot(tmpD);
            kv["SumDTol"] = "± " + FormatDot3(GeneralTol(tmpD));

            kv["SumD1"] = "(D1) " + FormatDot(tmpD1);
            kv["SumD1Tol"] = "± " + FormatDot3(GeneralTol(tmpD1));

            kv["SumD2"] = "(D2) " + FormatDot(tmpD2);
            kv["SumD2Tol"] = "+ " + FormatDot1(0);
            kv["SumD2TolN"] = "- " + FormatDot3(H8NegTol(tmpD2));

            kv["SumD3"] = "(D3) " + FormatDot(tmpD3);
            kv["SumD3Tol"] = "± " + FormatDot3(GeneralTol(tmpD3));

            kv["SumD4"] = "(D4) " + FormatDot(tmpD4);
            kv["SumD4Tol"] = "+ " + FormatDot3(H12Tol(tmpD4));
            kv["SumD4TolN"] = "+ " + FormatDot1(0);

            kv["SumD5"] = "(D5) " + FormatDot(tmpD5);
            kv["SumD5Tol"] = "± " + FormatDot3(GeneralTol(tmpD5));

            double inB = GetDouble(bm, "Bredd (B)");
            double inB1 = GetDouble(bm, "Bredd (B1)");
            double inB2 = GetDouble(bm, "Bredd (B2)");
            double inB3 = GetDouble(bm, "Bredd (B3)");

            double tmpB = inB == 0 ? 26 : inB;
            double tmpB1 = inB1 == 0 ? 10 : inB1;
            double tmpB2 = inB2 == 0 ? 9.8 : inB2;
            double tmpB3 = inB3 == 0 ? 11 : inB3;

            kv["SumB"] = "(B) " + FormatDot(tmpB);
            kv["SumBTol"] = "± " + FormatDot3(GeneralTol(tmpB));

            kv["SumB1"] = "(B1) " + FormatDot(tmpB1);
            kv["SumB1Tol"] = "+ " + FormatDot1(0);
            kv["SumB1TolN"] = "- " + FormatDot3(H8NegTol(tmpB1));

            kv["SumB2"] = "(B2) " + FormatDot(tmpB2);
            kv["SumB2Tol"] = "± " + FormatDot3(GeneralTol(tmpB2));

            kv["SumB3"] = "(B3) " + FormatDot(tmpB3);
            kv["SumB3Tol"] = "± " + FormatDot3(GeneralTol(tmpB3));

            double tmpM = 9.2;
            kv["SumM"] = "(M) " + FormatDot(tmpM);
            kv["SumMTol"] = "± " + FormatDot3(GeneralTol(tmpM));

            double tmpBM = 5;
            kv["SumBM"] = "(BM) " + FormatDot(tmpBM);
            kv["SumBMTol"] = "+ " + FormatDot3(BoreH12Tol(tmpBM));
            kv["SumBMTolN"] = "- " + FormatDot1(0);

            double inBD = GetDouble(bm, "Borrdjup (BD)");
            double tmpBD = 6;
            kv["SumBD"] = inBD == 0 ? "(BD) " + FormatDot(tmpBD) : FormatDot(inBD);
            kv["SumBDTol"] = "± " + FormatDot3(GeneralTol(tmpBD));

            double tmpA = 5;
            kv["SumA"] = "(A) " + FormatDot(tmpA);
            kv["SumATol"] = "± " + FormatDot3(GeneralTol(tmpA));

            double inRF = GetDouble(bm, "Radie till fräst spår (RF)");
            double tmpRF = inRF == 0 ? 107.5 : inRF;
            kv["SumRF"] = "(RF) " + FormatDot(tmpRF);
            kv["SumRFTol"] = "± " + FormatDot3(GeneralTol(tmpRF));

            double tmpR = 5;
            kv["SumR"] = "2x R" + FormatDot(tmpR);

            double inLF = GetDouble(bm, "Längd fräst spår (LF)");
            double tmpLF = inLF == 0 ? 50 : inLF;
            kv["SumLF"] = "(LF) " + FormatDot(tmpLF);
            kv["SumLFTol"] = "± " + FormatDot3(GeneralTol(tmpLF));

            kv["SumV45"] = "45º";
            kv["SumV45_2"] = kv["SumV45"];
            kv["SumF45"] = "1x45º";

            double inVBM = GetDouble(bm, "Vinkel borrhål (BM)");
            double tmpVBM = inVBM == 0 ? RoundTo(((Math.Atan2((tmpD / 2) - tmpBD, (tmpBM / 2) + 0.3) * 180.0) / Math.PI), 0.1) : inVBM;
            kv["SumV15"] = FormatDot1(tmpVBM) + "º";

            string tmpRa = "6.3";

            kv["SumRit"] = tmpBet;
            kv["SumRit2"] = kv["SumRit"];

            SumMaskinVal(kv, maskinVal);
            SumFrequencies(kv, maskinVal);
            SumFrequenciesPage2(kv, maskinVal);
            SumMeasuringDevices(kv, maskinVal);
            SumMeasuringDevicesPage2(kv, maskinVal);
            SumAF(kv, maskinVal, tmpRa);
            SumAFPage2(kv, maskinVal);

            kv["SumTextS1"] = "Okulärkontroll Grader, frifläcker, slagmärken, repor, valkar, ytjämnhet och faser, skarpa kanter avgradas.";
            kv["SumTextS2"] = kv["SumTextS1"];

            return kv;
        }

        private static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published)) return "";
            if (!DateTime.TryParse(published, out var pubDt)) return "";
            int dagar = 14;
            DateTime validTill = pubDt.AddDays(dagar);
            if (DateTime.Today <= validTill.Date)
                return "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom " + dagar.ToString(CultureInfo.InvariantCulture) + " dagar)\n\n" +
                       "Information om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument\n\n" +
                       "Popupruta aktiv till " + validTill.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return "";
        }

        private static void SumMaskinVal(Dictionary<string, string> kv, string maskinVal)
        {
            string s1 = EqualsI(maskinVal, "Nakamura") ? "Nakamura" :
                        EqualsI(maskinVal, "MaxMuller/Skepp6") ? "MaxMuller" :
                        EqualsI(maskinVal, "LB45") ? "LB45" : "";

            string s2 = EqualsI(maskinVal, "Nakamura") ? "Nakamura" :
                        EqualsI(maskinVal, "MaxMuller/Skepp6") ? "Skepp 6" :
                        EqualsI(maskinVal, "LB45") ? "LB45" : "";

            kv["SumMaskinValS1"] = ("Maskin: Svarvning - " + s1).Trim();
            kv["SumMaskinValS2"] = ("Maskin: Borrning - " + s2).Trim();
        }

        private static void SumFrequencies(Dictionary<string, string> kv, string maskinVal)
        {
            bool match = IsMachine(maskinVal);
            kv["SumF1_1"] = match ? "1/1" : "";
            kv["SumF1_2"] = match ? "1/1" : "";
            kv["SumF1_3"] = match ? "1/1" : "";
            kv["SumF1_4"] = match ? "1/3" : "";
            kv["SumF1_5"] = match ? "1/5" : "";
            kv["SumF1_6"] = match ? "Inst." : "";
            kv["SumF1_7"] = match ? "Inst." : "";
            kv["SumF1_8"] = match ? "1/3" : "";
            kv["SumF1_9"] = match ? "1/3" : "";
        }

        private static void SumFrequenciesPage2(Dictionary<string, string> kv, string maskinVal)
        {
            bool match = IsMachine(maskinVal);
            bool isNakamura = EqualsI(maskinVal, "Nakamura");
            kv["SumF2_1"] = match ? (isNakamura ? "1/5" : "1/1") : "";
            kv["SumF2_2"] = match ? (isNakamura ? "1/5" : "1/1") : "";
            kv["SumF2_3"] = match ? (isNakamura ? "1/5" : "1/1") : "";
            kv["SumF2_4"] = match ? (isNakamura ? "1/5" : "1/1") : "";
            kv["SumF2_5"] = match ? (isNakamura ? "Inst." : "1/1") : "";
        }

        private static void SumMeasuringDevices(Dictionary<string, string> kv, string maskinVal)
        {
            bool match = IsMachine(maskinVal);
            kv["SumD1_1"] = match ? "Mikrometer" : "";
            kv["SumD1_2"] = match ? "Mikrometer" : "";
            kv["SumD1_3"] = match ? "Digitalt Djup/Hakmått" : "";
            kv["SumD1_4"] = match ? "Digitalt Djup/Hakmått" : "";
            kv["SumD1_5"] = match ? "Skjutmått" : "";
            kv["SumD1_6"] = match ? "Vinkelsystem" : "";
            kv["SumD1_7"] = match ? "Vinkelsystem" : "";
            kv["SumD1_8"] = match ? "Ytjämnhetsmätare" : "";
            kv["SumD1_9"] = match ? "Skjutmått" : "";
        }

        private static void SumMeasuringDevicesPage2(Dictionary<string, string> kv, string maskinVal)
        {
            bool match = IsMachine(maskinVal);
            kv["SumD2_1"] = match ? "Skjutmått" : "";
            kv["SumD2_2"] = match ? "Skjutmått" : "";
            kv["SumD2_3"] = match ? "Skjutmått" : "";
            kv["SumD2_4"] = match ? "Skjutmått" : "";
            kv["SumD2_5"] = match ? "Skjutmått" : "";
        }

        private static void SumAF(Dictionary<string, string> kv, string maskinVal, string tmpRa)
        {
            bool match = IsMachine(maskinVal);
            kv["SumAF1_1"] = "";
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = "";
            kv["SumAF1_6"] = "";
            kv["SumAF1_7"] = "";
            kv["SumAF1_8"] = match ? "Bearbetas Ra " + tmpRa + " runt om" : "";
            kv["SumAF1_9"] = "";
        }

        private static void SumAFPage2(Dictionary<string, string> kv, string maskinVal)
        {
            bool match = IsMachine(maskinVal);
            kv["SumAF2_1"] = match ? "" : "";
            kv["SumAF2_2"] = match ? "" : "";
            kv["SumAF2_3"] = match ? "" : "";
            kv["SumAF2_4"] = match ? "" : "";
            kv["SumAF2_5"] = match ? "" : "";
        }

        private static bool IsMachine(string maskinVal)
        {
            if (string.IsNullOrEmpty(maskinVal)) return false;
            for (int i = 0; i < Machines.Length; i++)
                if (string.Equals(Machines[i], maskinVal, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }

        private static string[] SplitTokens(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return Array.Empty<string>();
            return s.Split(new[] { ' ', '/', '.' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static string GetTokenOrZero(string[] tokens, int index)
        {
            if (tokens == null || index < 0 || index >= tokens.Length) return "0";
            return string.IsNullOrWhiteSpace(tokens[index]) ? "0" : tokens[index];
        }

        private static double ParseDoubleOrZero(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;
            s = s.Trim().Replace(".", ",");
            return double.TryParse(s, NumberStyles.Any, CommonFunctions.Culture, out var v) ? v : 0;
        }

        private static bool EqualsNumber(double a, double b)
        {
            return Math.Abs(a - b) < 0.0001;
        }

        private static double RoundTo(double value, double step)
        {
            if (step <= 0) return value;
            return Math.Round(value / step, MidpointRounding.AwayFromZero) * step;
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

        private static double H8NegTol(double d)
        {
            if (d < 3.01) return 0.014;
            if (d < 6.01) return 0.018;
            if (d < 10.01) return 0.022;
            if (d < 18.01) return 0.027;
            if (d < 30.01) return 0.033;
            if (d < 50.01) return 0.039;
            if (d < 80.01) return 0.046;
            if (d < 120.01) return 0.054;
            if (d < 180.01) return 0.063;
            if (d < 250.01) return 0.072;
            if (d < 315.01) return 0.081;
            if (d < 400.01) return 0.089;
            if (d < 500.01) return 0.097;
            return 0.110;
        }

        private static double H12Tol(double d)
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

        private static double BoreH12Tol(double d)
        {
            if (d < 3.01) return 0.100;
            if (d < 6.01) return 0.120;
            return 0.150;
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
            return double.TryParse(raw, NumberStyles.Any, CommonFunctions.Culture, out var v) ? v : 0;
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
        {
            return string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);
        }
    }
}
