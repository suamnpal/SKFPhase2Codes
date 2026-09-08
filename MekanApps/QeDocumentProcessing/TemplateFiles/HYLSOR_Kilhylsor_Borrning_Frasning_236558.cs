using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Kilhylsor_Borrning_Frasning_236558 : ITemplateCalculations
    {
        private static readonly string[] Machines = new[] { "Skepp 6", "VTR-160", "MacTurn 550" };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;
            List<Bookmark> bm = req != null ? req.Bookmarks : null;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.ToUpperInvariant().Trim();

            bool tmpSlash = tmpBet.IndexOf('/') >= 0;
            bool tmpLU = tmpBet.IndexOf("LU", StringComparison.OrdinalIgnoreCase) >= 0;
            bool tmpVZ = tmpBet.IndexOf("VZ", StringComparison.OrdinalIgnoreCase) >= 0;
            bool tmpV21 = tmpBet.IndexOf("V21", StringComparison.OrdinalIgnoreCase) >= 0;

            bool tmpSpecial = tmpBet.Length > 11;

            SumMaskinVal(kv, maskinVal);
            SumFrequencies(kv, maskinVal);
            SumMeasuringDevices(kv, maskinVal);
            SumAF(kv, maskinVal);

            kv["SumTextS1"] = "Bryt alla kanter, avlägsna";

            kv["SumRitningsnr"] = "Styckritning: " + GetString(bm, "Ritningsnummer");
            kv["SumRitTol"] = "Toleranser: 1432010, 7437495";
            kv["SumRitYtjämnhet"] = "Yta: 7430184";
            kv["SumKlEgenskaper"] = "PRODUCTION NUTS & SLEEVES & HOUSINGS\\ALLMÄNKLASSADE EGENSKAPER\\Klassade egenskaper kilhylsor";

            string textBm = GetString(bm, "Text");
            bool textEmpty = string.IsNullOrEmpty(textBm) || EqualsI(textBm, "0");
            string[] textParts = textEmpty ? new string[0] : textBm.Split('§');
            kv["SumText1"] = textEmpty || textParts.Length < 1 ? "" : textParts[0];
            kv["SumText2"] = textEmpty || textParts.Length < 2 ? "" : textParts[1];
            kv["SumText3"] = textEmpty || textParts.Length < 3 ? "" : textParts[2];
            kv["SumText4"] = textEmpty || textParts.Length < 4 ? "" : textParts[3];

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
            kv["SumMaskinValS1"] = ("Maskin: " + s + " - Borrning, Fräsning").TrimStart();
        }

        private static void SumFrequencies(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsMachine(maskinVal);
            bool isEmpty = string.IsNullOrEmpty(maskinVal);
            kv["SumF1_1"] = (m || isEmpty) ? "1/1" : "";
            kv["SumF1_2"] = (m || isEmpty) ? "1/1" : "";
            kv["SumF1_3"] = m ? "1/1" : "";
            kv["SumF1_4"] = m ? "1/1" : "";
        }

        private static void SumMeasuringDevices(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsMachine(maskinVal);
            kv["SumD1_1"] = m ? "Gängtolk" : "";
            kv["SumD1_2"] = m ? "Skjutmått" : "";
            kv["SumD1_3"] = m ? "Skjutmått" : "";
            kv["SumD1_4"] = m ? "Skjutmått" : "";
        }

        private static void SumAF(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsMachine(maskinVal);
            bool isEmpty = string.IsNullOrEmpty(maskinVal);
            kv["SumAF1_1"] = "";
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = m ? "Tolerans d1 efter slits enl. 7437495" : (isEmpty ? "" : "");
            kv["SumAF1_4"] = "";
        }

        private static bool IsMachine(string mv)
        {
            if (string.IsNullOrEmpty(mv)) return false;
            for (int i = 0; i < Machines.Length; i++)
                if (string.Equals(Machines[i], mv, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
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