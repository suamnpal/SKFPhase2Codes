using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class MUTTRAR_ANN_0086_OP1_3_Svarv_Borr_Fras : ITemplateCalculations
    {
        private static readonly string[] Machines = new[] { "LT300" };
        private const string LB = "<<LineBreak>>";
        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");

            bool tmpSlash = tmpBet.IndexOf('/') >= 0;
            bool tmp0086 = tmpBet.IndexOf("0086", StringComparison.OrdinalIgnoreCase) >= 0;

            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : string.Empty;
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : string.Empty;

            int tmpCountB2 = tmpBet2.Length;

            string tmpArt = tmpBet1;
            string tmpSerie = tmpBet2.Length >= 2 ? tmpBet2.Substring(0, 2) : tmpBet2;
            string tmpTyp = tmpBet2.Length >= 2 ? tmpBet2.Substring(tmpBet2.Length - 2) : tmpBet2;

            kv["SumArt"] = tmpArt;
            kv["SumSerie"] = tmpSerie;
            kv["SumTyp"] = tmpTyp;

            kv["SumKlEgenskaperS2"] = "PPA & PPH/ALLMÄN/KLASSADE EGENSKAPER/Klassade egenskaper muttrar";
            kv["SumKlEgenskaperS3"] = kv["SumKlEgenskaperS2"];

            kv["SumRitS1"] = "produkt: " + tmpBet + ", Märkning enl. 7433462:senaste utg.";
            kv["SumRitS2"] = kv["SumRitS1"];

            string tmpMaskinValS1 = IsMachine(maskinVal) ? maskinVal : "";
            string tmpMaskinValS2 = IsMachine(maskinVal) ? maskinVal : "";
            kv["SumMaskinValS1"] = ("Maskin: " + tmpMaskinValS1 + " - Svarvning ").TrimEnd();
            kv["SumMaskinValS2"] = ("Maskin: " + tmpMaskinValS2 + " - Borr & Fräsning ").TrimEnd();

            kv["SumTextS1"] = "Grader, frifläckar, slagmärken, repor, valkar & andra defekter samt ojämnheter kontrolleras med ögonmått, för övriga mått används skjutmått.";
            kv["SumTextS2"] = "Övriga mått kontrolleras vid inställning, för ej toleranssatta mått gäller iso 2768 mk"+ LB + "Rest magnetism kontrolleras vid behov enligt ritning 7433428"+ LB +"Renhetskrav enligt ritning 7433430";

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
                       " dagar)"+ LB +"Information om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument" + LB +"Popupruta aktiv till " +
                       validTill.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return "";
        }

        private static bool IsMachine(string maskinVal)
        {
            if (string.IsNullOrEmpty(maskinVal)) return false;
            for (int i = 0; i < Machines.Length; i++)
                if (string.Equals(Machines[i], maskinVal, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }

        private static double GetDouble(List<Bookmark> bm, string key)
        {
            string raw = GetString(bm, key);
            if (string.IsNullOrWhiteSpace(raw)) return 0;
            double v;
            return double.TryParse(raw.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
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