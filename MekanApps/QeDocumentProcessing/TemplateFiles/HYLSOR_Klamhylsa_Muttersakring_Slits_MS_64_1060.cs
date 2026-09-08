using DocumentFormat.OpenXml.Bibliography;
using Microsoft.VisualBasic;
using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Klamhylsa_Muttersakring_Slits_MS_64_1060 : ITemplateCalculations
    {
        private const string LB = "<<LineBreak>>";
        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            if (string.IsNullOrWhiteSpace(subject)) subject = GetStringField(req, "Subject");

            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;
            if (string.IsNullOrWhiteSpace(maskinVal)) maskinVal = GetStringField(req, "MaskinVal");

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = (subject ?? string.Empty).Trim().ToUpperInvariant();
            string tmpBet = tmpFormat.Replace(".", ",");

            string[] tokens = SplitTokens(tmpBet);
            string tmpBet2a = tokens.Length > 1 ? tokens[1] : string.Empty;
            string tmpBet2 = tmpBet2a;

            kv["SumRitNr"] = "11k " + tmpBet2;
            kv["SumRitNrS2"] = kv["SumRitNr"];

            kv["SumV30"] = "30º";

            double tmpAMS = GetDoubleField(req, "Antal Muttersäkringar");
            kv["SumAMS"] = "9x";

            string tmpVms = Math.Abs(tmpAMS - 9.0) < 0.0000001 ? "40" : string.Empty;
            kv["SumVMS"] = "40º";

            string tmpC = GetStringField(req, "Slits (C)");
            kv["SumC"] = "(c) 10";
            kv["SumCTol"] = "± 0.2";

            double tmpF = GetDoubleField(req, "Låsspårslängd (f)");
            kv["SumF"] = "(f) 45";
            double tmpFTol = tmpF > 50.0 ? 3.0 : tmpF > 30.0 ? 2.5 : tmpF > 19.0 ? 2.1 : 1.8;
            kv["SumFTol"] = "+ 2.5";
            kv["SumFTolN"] = "- " + FormatDot1(0.0) + " [3F]";

            double tmpE = GetDoubleField(req, "Låsspårsbredd (e)");
            kv["SumE"] = "(e) 45";
            double tmpETol = tmpE > 50.0 ? 0.74 : tmpE > 30.0 ? 0.62 : tmpE > 18.0 ? 0.52 : tmpE > 10.0 ? 0.43 : tmpE > 6.0 ? 0.36 : 0.3;
            kv["SumETol"] = "+ 0.620";
            kv["SumETolN"] = "- 0.0[3F]";

            string tmpMaskinValS1 = MapMachine(maskinVal);
            kv["SumMaskinValS1"] = "Maskin: " + tmpMaskinValS1 + " - Muttersäkring, Slits";

            bool isSkepp6 = EqualsI(maskinVal, "Skepp6");
            bool isHalf = EqualsI(maskinVal, "K&T") || EqualsI(maskinVal, "VTR-160") || EqualsI(maskinVal, "MacTurn 550");

            kv["SumF1_1"] = isSkepp6 ? "1/1" : isHalf ? "1/2" : "";
            kv["SumF1_2"] = isSkepp6 ? "1/1" : isHalf ? "1/2" : "";
            kv["SumF1_3"] = isSkepp6 ? "1/1" : isHalf ? "1/2" : "";
            kv["SumF1_4"] = isSkepp6 ? "1/1" : isHalf ? "1/2" : "";
            kv["SumF1_5"] = "";

            bool isAnyMachine = isSkepp6 || EqualsI(maskinVal, "K&T") || EqualsI(maskinVal, "VTR-160") || EqualsI(maskinVal, "MacTurn 550");

            kv["SumD1_1"] = isAnyMachine ? "Skjutmått" : "";
            kv["SumD1_2"] = isAnyMachine ? "Skjutmått" : "";
            kv["SumD1_3"] = isAnyMachine ? "Skjutmått" : "";
            kv["SumD1_4"] = "";
            kv["SumD1_5"] = "";

            kv["SumAF1_1"] = "";
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = "";

            kv["SumTextS1"] = "";

            return kv;
        }

        private static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published)) return "";
            if (!DateTime.TryParse(published, out var pubDt)) return "";
            int dagar = 14;
            var validTill = pubDt.AddDays(dagar);
            if (DateTime.Today <= validTill.Date)
                return "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom " + dagar.ToString(CultureInfo.InvariantCulture) + " dagar)" + LB + LB + "" + LB + LB + "Popupruta aktiv till " + validTill.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return "";
        }

        private static string MapMachine(string maskinVal)
        {
            if (EqualsI(maskinVal, "Skepp6")) return "Skepp6";
            if (EqualsI(maskinVal, "K&T")) return "K&T";
            if (EqualsI(maskinVal, "VTR-160")) return "VTR-160";
            if (EqualsI(maskinVal, "MacTurn 550")) return "MacTurn 550";
            return "";
        }

        private static bool EqualsI(string a, string b) => string.Equals(a ?? string.Empty, b ?? string.Empty, StringComparison.OrdinalIgnoreCase);

        private static string[] SplitTokens(string s)
        {
            if (string.IsNullOrEmpty(s)) return Array.Empty<string>();
            return s.Split(new[] { ' ', '/', '.', '-', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static string GetStringField(APIRequest req, string key)
        {
            if (req == null || string.IsNullOrWhiteSpace(key)) return string.Empty;
            string value;
            if (TryGetFromDictionaries(req, key, out value)) return value ?? string.Empty;
            return string.Empty;
        }

        private static double GetDoubleField(APIRequest req, string key)
        {
            string s = GetStringField(req, key);
            if (string.IsNullOrWhiteSpace(s)) return 0.0;
            s = s.Trim().Replace(',', '.');
            return double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var v) ? v : 0.0;
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

        private static string FormatDot(double v) => v.ToString(CommonFunctions.Culture).Replace(",", ".");
        private static string FormatDot0(double v) => v.ToString("F0", CommonFunctions.Culture).Replace(",", ".");
        private static string FormatDot1(double v) => v.ToString("F1", CommonFunctions.Culture).Replace(",", ".");
        private static string FormatDot3(double v) => v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
    }
}
