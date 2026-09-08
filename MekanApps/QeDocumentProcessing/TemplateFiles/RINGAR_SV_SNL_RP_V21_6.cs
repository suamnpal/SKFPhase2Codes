using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class RINGAR_SV_SNL_RP_V21_6 : ITemplateCalculations
    {
        private static readonly string[] Machines = new[]
        {
            "Nakamura", "LC-20"
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
            string tmpBet3Text = tokens.Length > 2 ? tokens[2] : string.Empty;

            string tmpSerie = tmpBet3Text.Length >= 2 ? tmpBet3Text.Substring(0, 2) : tmpBet3Text;
            string tmpTyp = tmpBet3Text.Length >= 2 ? tmpBet3Text.Substring(tmpBet3Text.Length - 2) : tmpBet3Text;

            bool tmpTyp48 = string.Equals(tmpTyp, "48", StringComparison.OrdinalIgnoreCase);

            string rawB = GetString(bm, "Bredd (B)");
            double tmpB = IsZeroOrEmpty(rawB) ? 10.0 : ParseDouble(rawB);
            kv["SumB"] = "(B) " + FormatDot(tmpB);
            kv["SumBTol"] = BtolText(tmpB);

            string rawC = GetString(bm, "Ytterdiameter (C)");
            double tmpC = IsZeroOrEmpty(rawC) ? (tmpTyp48 ? 240.0 : 0.0) : ParseDouble(rawC);
            kv["SumC"] = "(C) " + FormatDot(tmpC);
            kv["SumCTol"] = "± " + FormatDot1(GeneralTol(tmpC));

            string rawD = GetString(bm, "Innerdiameter (D)");
            double tmpD = IsZeroOrEmpty(rawD) ? (tmpC - 20.0) : ParseDouble(rawD);
            kv["SumD"] = "(D) " + FormatDot(tmpD);
            kv["SumDTol"] = "+ " + FormatDot3(H10Tol(tmpD));
            kv["SumDTolN"] = "- " + FormatDot1(0.0);

            string rawG = GetString(bm, "Gänga (G)");
            string tmpG = IsZeroOrEmpty(rawG) ? "5" : rawG.Trim();
            kv["SumG"] = "x2 (G) M" + tmpG;

            string rawGL = GetString(bm, "Mått till Gänga (GL)");
            double tmpGL = IsZeroOrEmpty(rawGL) ? (tmpTyp48 ? 5.0 : 0.0) : ParseDouble(rawGL);
            kv["SumGL"] = "x2 (GL) " + FormatDot(tmpGL);
            kv["SumGLTol"] = tmpGL < 6.0 ? "± 0.1" : "± 0.2";
            kv["SumGlgr"] = "120º";

            kv["SumRa"] = "3.2";
            kv["SumRit"] = tmpBet;

            SumMachineSection(kv, maskinVal, tmpD);

            kv["SumTextS1"] = "Okulärkontroll av grader, frifläckar, slagmärken, repor, valkar, ytjämnhet samt faser, skarpa kanter avgradas.";

            return kv;
        }

        private static void SumMachineSection(Dictionary<string, string> kv, string maskinVal, double tmpD)
        {
            bool isKnown = IsMachine(maskinVal);
            bool isEmpty = string.IsNullOrWhiteSpace(maskinVal);

            string machineText = string.Equals(maskinVal ?? "", "Nakamura", StringComparison.OrdinalIgnoreCase) ? "Nakamura" :
                                 string.Equals(maskinVal ?? "", "LC-20", StringComparison.OrdinalIgnoreCase) ? "LC-20" : "";

            kv["SumMaskinValS1"] = ("Maskin: " + machineText).Trim();

            kv["SumF1_1"] = (isKnown ? "1/1" : (isEmpty ? "1/1" : ""));
            kv["SumF1_2"] = (isKnown ? "Skärbyte" : (isEmpty ? "1/1" : ""));
            kv["SumF1_3"] = (isKnown ? "Skärbyte" : (isEmpty ? "1/1" : ""));
            kv["SumF1_4"] = (isKnown ? "1/5" : (isEmpty ? "1/1" : ""));
            kv["SumF1_5"] = (isKnown ? "1/5" : (isEmpty ? "1/1" : ""));
            kv["SumF1_6"] = (isKnown ? "1/tim" : (isEmpty ? "1/1" : ""));

            kv["SumD1_1"] = isKnown ? "Digitalt Skjutmått" : "";
            kv["SumD1_2"] = isKnown ? "UD-Apparat" : "";
            kv["SumD1_3"] = isKnown ? "Digitalt Skjutmått" : "";
            kv["SumD1_4"] = isKnown ? "Gängtolk" : "";
            kv["SumD1_5"] = isKnown ? "Skjutmått" : "";
            kv["SumD1_6"] = isKnown ? "Ytjämnhetsmätare" : "";

            kv["SumAF1_1"] = isKnown ? "" : "";
            kv["SumAF1_2"] = isKnown ? ("Inställningsring: " + FormatDot(tmpD)) : "";
            kv["SumAF1_3"] = isKnown ? "" : "";
            kv["SumAF1_4"] = isKnown ? "" : "";
            kv["SumAF1_5"] = isKnown ? "" : "";
            kv["SumAF1_6"] = isKnown ? "Alla bearbetade mått" : "";
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
                       " dagar)\n\n\n\nPopupruta aktiv till " +
                       validTill.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            return "";
        }

        private static string BtolText(double b)
        {
            if (b < 6.0) return "± 0.1";
            if (b < 30.0) return "± 0.2";
            return "± 0.3";
        }

        private static double GeneralTol(double v)
        {
            if (v < 6.0) return 0.1;
            if (v < 30.0) return 0.2;
            if (v < 120.0) return 0.3;
            if (v < 400.0) return 0.5;
            if (v < 1000.0) return 0.8;
            if (v < 2000.0) return 1.2;
            return 2.0;
        }

        private static double H10Tol(double d)
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
            return 0.280;
        }

        private static bool IsMachine(string maskinVal)
        {
            if (string.IsNullOrWhiteSpace(maskinVal)) return false;
            for (int i = 0; i < Machines.Length; i++)
                if (string.Equals(Machines[i], maskinVal, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }

        private static bool IsZeroOrEmpty(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return true;
            string t = s.Trim();
            return t == "0" || t == "0,0" || t == "0.0";
        }

        private static double ParseDouble(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return 0;
            string t = raw.Trim().Replace(".", ",");
            double v;
            return double.TryParse(t, NumberStyles.Any, CommonFunctions.Culture, out v) ? v : 0;
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
    }
}
