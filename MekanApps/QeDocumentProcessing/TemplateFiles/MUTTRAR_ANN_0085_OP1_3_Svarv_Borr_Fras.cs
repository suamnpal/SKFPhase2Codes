using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class MUTTRAR_ANN_0085_OP1_3_Svarv_Borr_Fras : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            if (string.IsNullOrWhiteSpace(subject)) subject = GetStringField(req, "Subject");

            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;
            if (string.IsNullOrWhiteSpace(maskinVal)) maskinVal = GetStringField(req, "MaskinVal");

            string tmpFormat = (subject ?? string.Empty).Trim().ToUpperInvariant();
            string tmpBet = tmpFormat.Replace(".", ",");

            string[] tokens = SplitTokens(tmpBet);
            string tmpBet1a = tokens.Length > 0 ? tokens[0] : string.Empty;
            string tmpBet2a = tokens.Length > 1 ? tokens[1] : string.Empty;

            string tmpArt = tmpBet1a;
            string tmpSerie = Left(tmpBet2a, 2);
            string tmpTyp = Right(tmpBet2a, 2);

            kv["VaLPopUp"] = "";

            kv["SumArt"] = tmpArt;
            kv["SumSerie"] = tmpSerie;
            kv["SumTyp"] = tmpTyp;

            kv["SumKlEgenskaperS2"] = "PPA & PPH/ALLMÄN/KLASSADE EGENSKAPER/Klassade egenskaper muttrar";
            kv["SumKlEgenskaperS3"] = kv["SumKlEgenskaperS2"];

            kv["SumRitS1"] = "produkt: " + tmpArt + ", Stämplas enl. 7430189" + ":senaste utg.";
            kv["SumRitS2"] = kv["SumRitS1"];

            string tmpMaskinVal = "";
            string tmpMaskinValS1 = string.Equals(maskinVal, "VTR-160", StringComparison.OrdinalIgnoreCase) ? "VTR-160" : "";
            kv["SumMaskinValS1"] = "Maskin: " + tmpMaskinValS1 + " - Svarvning " + Environment.NewLine + tmpMaskinVal;

            string tmpMaskinValS2 = string.Equals(maskinVal, "VTR-160", StringComparison.OrdinalIgnoreCase) ? "VTR-160" : "";
            kv["SumMaskinValS2"] = "Maskin: " + tmpMaskinValS2 + " - Borr & Fräsning " + Environment.NewLine + tmpMaskinVal;

            kv["SumTextS1"] = "Grader, frifläckar, slagmärken, repor, valkar & andra defekter samt ojämnheter kontrolleras med ögonmått, för övriga mått används skjutmått.<<LineBreak>>Märkning enligt ritning 7433200";
            kv["SumTextS2"] = "Övriga mått kontrolleras vid inställning, för ej toleranssatta mått gäller iso 2768 mk<<LineBreak>>Rest magnetism kontrolleras vid behov enligt ritning 7433428<<LineBreak>>Renhetskrav enligt ritning 7433430";

            return kv;
        }

        private static string[] SplitTokens(string s)
        {
            if (string.IsNullOrEmpty(s)) return Array.Empty<string>();
            return s.Split(new[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static string Left(string s, int n)
        {
            if (string.IsNullOrEmpty(s) || n <= 0) return string.Empty;
            return s.Length <= n ? s : s.Substring(0, n);
        }

        private static string Right(string s, int n)
        {
            if (string.IsNullOrEmpty(s) || n <= 0) return string.Empty;
            return s.Length <= n ? s : s.Substring(s.Length - n, n);
        }

        private static string GetStringField(APIRequest req, string key)
        {
            if (req == null || string.IsNullOrWhiteSpace(key)) return string.Empty;
            string value;
            if (TryGetFromDictionaries(req, key, out value)) return value ?? string.Empty;
            return string.Empty;
        }

        private static bool TryGetFromDictionaries(object obj, string key, out string value)
        {
            value = null;
            if (obj == null) return false;
            var t = obj.GetType();
            foreach (var p in t.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!p.CanRead) continue;
                object pv;
                try { pv = p.GetValue(obj, null); }
                catch { continue; }
                if (pv == null) continue;

                var dictSS = pv as IDictionary<string, string>;
                if (dictSS != null)
                {
                    if (dictSS.TryGetValue(key, out value)) return true;
                    foreach (var kvp in dictSS)
                        if (string.Equals(kvp.Key ?? string.Empty, key, StringComparison.OrdinalIgnoreCase))
                        { value = kvp.Value; return true; }
                }

                var dictSO = pv as IDictionary<string, object>;
                if (dictSO != null)
                {
                    object ov;
                    if (dictSO.TryGetValue(key, out ov))
                    { value = ov != null ? ov.ToString() : string.Empty; return true; }
                    foreach (var kvp in dictSO)
                        if (string.Equals(kvp.Key ?? string.Empty, key, StringComparison.OrdinalIgnoreCase))
                        { value = kvp.Value != null ? kvp.Value.ToString() : string.Empty; return true; }
                }

                var nonGen = pv as IDictionary;
                if (nonGen != null)
                {
                    foreach (DictionaryEntry de in nonGen)
                    {
                        if (de.Key == null) continue;
                        if (string.Equals(de.Key.ToString(), key, StringComparison.OrdinalIgnoreCase))
                        { value = de.Value != null ? de.Value.ToString() : string.Empty; return true; }
                    }
                }
            }
            return false;
        }
    }
}
