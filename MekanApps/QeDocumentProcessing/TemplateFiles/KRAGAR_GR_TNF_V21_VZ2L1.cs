using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class KRAGAR_GR_TNF_V21_VZ2L1 : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            string subject = GetString(req, "Subject", "ProductDesignation");
            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace('.', ',');
            bool tmpSlash = tmpBet.Contains("/");
            bool tmpV21 = tmpBet.IndexOf("V21", StringComparison.OrdinalIgnoreCase) >= 0;
            bool tmpVZ2L1 = tmpBet.IndexOf("VZ2L1", StringComparison.OrdinalIgnoreCase) >= 0;

            string normalizedSubject = NormalizeKey(subject);
            bool isGR3060VZ2L1 = normalizedSubject == "GRTNF3060VZ2L1";
            bool isGR3184_3284_L_VZ2L1 = normalizedSubject == "GRTNF31843284LVZ2L1";
            bool forceFormulaDefaults = isGR3060VZ2L1 || isGR3184_3284_L_VZ2L1;

            string[] bet = (tmpBet ?? string.Empty).Split(new[] { ' ', '/', '.' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = Present(Word(bet, 1));
            string tmpBet2 = Present(Word(bet, 2));
            string tmpBet3 = Present(Word(bet, 3));
            string tmpBet4 = Present(Word(bet, 4));
            string tmpBet5 = Present(Word(bet, 5));
            string tmpBet6 = Present(Word(bet, 6));

            int tmpCountB2 = tmpBet2.Length;
            string tmpSerie = tmpSlash && (tmpCountB2 == 3 || tmpCountB2 == 2)
                ? "NoSerie"
                : tmpCountB2 > 4 ? Left(tmpBet2, 3)
                : tmpCountB2 == 3 ? Left(tmpBet2, 1)
                : Left(tmpBet2, 2);

            string tmpTypParsed = Right(tmpBet2, 2);
            int tmpTypNumParsed = ToInt(tmpTypParsed);

            int tmpV21Idx = MemberIndex(tmpTypParsed, new[] { "68", "76", "80", "88" });
            int tmpVZIdx = MemberIndex(tmpTypParsed, new[] { "84" });

            if (tmpVZ2L1 && tmpVZIdx == 0) tmpVZIdx = 1;
            int typeIdx = tmpVZ2L1 ? tmpVZIdx : tmpV21Idx;
            string tmpTypEffective = tmpVZ2L1 && typeIdx == 1 ? "84" : tmpTypParsed;
            int tmpTypNum = tmpVZ2L1 && typeIdx == 1 ? 84 : tmpTypNumParsed;

            kv["VaLPopUp"] = ComputePopup(GetString(req, "Published"));
            kv["TmpFormat"] = tmpFormat; kv["TmpBet"] = tmpBet;
            kv["Tmp/"] = Bool(tmpSlash); kv["TmpV21"] = Bool(tmpV21); kv["TmpVZ2L1"] = Bool(tmpVZ2L1);
            kv["TmpBet1"] = tmpBet1; kv["TmpBet2"] = tmpBet2; kv["TmpBet3"] = tmpBet3;
            kv["TmpBet4"] = tmpBet4; kv["TmpBet5"] = tmpBet5; kv["TmpBet6"] = tmpBet6;
            kv["SumArt"] = tmpBet1; kv["TmpSerie"] = tmpSerie; kv["SumSerie"] = tmpSerie;
            kv["TmpTyp"] = tmpTypParsed; kv["SumTyp"] = tmpTypEffective;

            double A = InOrDefault(req, typeIdx, forceFormulaDefaults, tmpVZ2L1 ? new[] { isGR3184_3284_L_VZ2L1 ? 534d : 391d } : new[] { 474d, 510d, 505d, 525d }, "Ø A", "O A", "Diameter A", "A");
            SetValue(kv, "A", A, "(A) " + Fmt(A), TolOuter(A));

            double B = InOrDefault(req, typeIdx, forceFormulaDefaults, tmpVZ2L1 ? new[] { isGR3184_3284_L_VZ2L1 ? 472d : 352d } : new[] { 402d, 431d, 451d, 453d }, "Ø B", "O B", "Diameter B", "B");
            SetValue(kv, "B", B, "(B) " + Fmt(B), TolOuter(B));

            double C1 = InOrDefault(req, typeIdx, forceFormulaDefaults, tmpVZ2L1 ? new[] { isGR3184_3284_L_VZ2L1 ? 478d : 343d } : new[] { 437d, 460d, 437d, 468d }, "Ø C1", "O C1", "Diameter C1", "C1");
            SetValue(kv, "C1", C1, "(C1) " + Fmt(C1), "+ 0", "- " + FmtFixed(H11(C1), 3));

            double C2 = InOrDefault(req, typeIdx, forceFormulaDefaults, tmpVZ2L1 ? new[] { isGR3184_3284_L_VZ2L1 ? 468d : 331d } : new[] { 427d, 450d, 427d, 458d }, "Ø C2", "O C2", "Diameter C2", "C2");
            SetValue(kv, "C2", C2, "(C2) " + Fmt(C2), TolOuter(C2));

            double D1 = InOrDefault(req, typeIdx, forceFormulaDefaults, tmpVZ2L1 ? new[] { isGR3184_3284_L_VZ2L1 ? 463d : 343d } : new[] { 393d, 423d, 443d, 444d }, "Ø D1", "O D1", "Diameter D1", "D1");
            SetValue(kv, "D1", D1, "(D1) " + Fmt(D1), "+ " + FmtFixed(H11(D1), 3), "- 0");

            double D2 = InOrDefault(req, typeIdx, forceFormulaDefaults, tmpVZ2L1 ? new[] { isGR3184_3284_L_VZ2L1 ? 451d : 331d } : new[] { 376d, 411d, 431d, 432d }, "Ø D2", "O D2", "Diameter D2", "D2");
            SetValue(kv, "D2", D2, "(D2) " + Fmt(D2), "+ 0", "- " + FmtFixed(H11(D2), 3));

            double D3 = InOrDefault(req, typeIdx, forceFormulaDefaults, tmpVZ2L1 ? new[] { isGR3184_3284_L_VZ2L1 ? 445d : 325d } : new[] { 367d, 405d, 425d, 425d }, "Ø D3", "O D3", "Diameter D3", "D3");
            SetValue(kv, "D3", D3, "(D3) " + Fmt(D3), "+ " + FmtFixed(H11(D3), 3), "- 0");

            double E = InOrDefault(req, typeIdx, forceFormulaDefaults, tmpVZ2L1 ? new[] { isGR3184_3284_L_VZ2L1 ? 404d : 284d } : new[] { 324d, 364d, 384d, 384d }, "Ø E", "O E", "Diameter E", "E");
            SetValue(kv, "E", E, "(E) " + Fmt(E), "+ 0.300", "- 0");

            double F = InOrDefault(req, typeIdx, forceFormulaDefaults, tmpVZ2L1 ? new[] { isGR3184_3284_L_VZ2L1 ? 512d : 371d } : new[] { 450d, 474d, 484d, 495d }, "Ø F", "O F", "Diameter F", "F");
            SetValue(kv, "F", F, "(F) " + Fmt(F), TolJs13(F));

            kv["SumG2"] = "1/4-28 UNF"; kv["SumGmin"] = "min 6"; kv["SumGBD"] = "15";

            double G = InOrDefault(req, typeIdx, forceFormulaDefaults, tmpVZ2L1 ? new[] { 15d } : new[] { 15.5d, 15.5d, 15.5d, 16d }, "Bredd (G)", "Bredd G", "G");
            SetValue(kv, "G", G, "(G) " + Fmt(G), "± 0.5");

            kv["SumR1"] = "max R1"; kv["SumR2"] = "R2"; kv["SumR2a"] = "R2";
            kv["Sum225"] = "22.5°"; kv["Sum45"] = "45°x8"; kv["SumFas"] = "1x45°";

            double BH = isGR3060VZ2L1 ? 6.6 : tmpTypNum < 62 ? 6.6 : 9d;
            double BHTol = tmpVZ2L1 ? 0.36 : tmpTypNum < 62 ? 0.36 : 0.22;
            SetValue(kv, "BH", BH, "(BH) " + Fmt(BH), "+ " + FmtFixed(BHTol, 3), "- 0");
            SetValue(kv, "H", BH, "(BH) " + Fmt(BH), "+ " + FmtFixed(BHTol, 3), "- 0");

            double B1 = InOrDefault(req, typeIdx, forceFormulaDefaults, tmpVZ2L1 ? new[] { isGR3184_3284_L_VZ2L1 ? 43.5d : 38.5d } : new[] { 43.5d, 38.5d, 38.5d, 41d }, "Bredd (B1)", "Bredd B1", "B1");
            SetValue(kv, "B1", B1, "(B1) " + Fmt(B1), "± 0.3");
            double B2 = B1 - 5d; kv["TmpB2Konst"] = "5"; SetValue(kv, "B2", B2, "(B2) " + Fmt(B2), "± 0.3");
            double B3 = InOrDefault(req, typeIdx, forceFormulaDefaults, tmpVZ2L1 ? new[] { isGR3184_3284_L_VZ2L1 ? 28.5d : 23.5d } : new[] { 28.5d, 24.5d, 23.5d, 26d }, "Bredd (B3)", "Bredd B3", "B3");
            SetValue(kv, "B3", B3, "(B3) " + Fmt(B3), "± 0.2");
            double B4 = InOrDefault(req, typeIdx, forceFormulaDefaults, tmpVZ2L1 ? new[] { isGR3184_3284_L_VZ2L1 ? 22d : 21.5d } : new[] { 23.5d, 20d, 21d, 21d }, "Bredd (B4)", "Bredd B4", "B4");
            SetValue(kv, "B4", B4, "(B4) " + Fmt(B4), "+ 0.200", "- 0");
            double B5 = InOrDefault(req, typeIdx, forceFormulaDefaults, tmpVZ2L1 ? new[] { 7.5d } : new[] { 10.5d, 7.5d, 8d, 8d }, "Bredd (B5)", "Bredd B5", "B5");
            SetValue(kv, "B5", B5, "(B5) " + Fmt(B5), "+ 0.200", "- 0");
            double B6 = tmpTypNum == 68 ? 26d : 23.5d; SetValue(kv, "B6", B6, "(B6) " + Fmt(B6), "+ 0.200", "- 0");
            double B7 = InOrDefault(req, typeIdx, forceFormulaDefaults, tmpVZ2L1 ? new[] { isGR3184_3284_L_VZ2L1 ? 13d : 8d } : new[] { 9d, 8d, 8d, 9d }, "Bredd (B7)", "Bredd B7", "B7");
            SetValue(kv, "B7", B7, "(B7) " + Fmt(B7), "+ 0.200", "- 0");

            string yv = GetString(req, "Yttre vinkel (Yv)", "Yttre vinkel Yv", "Yv");
            kv["TmpYv"] = yv; kv["SumYv"] = string.IsNullOrWhiteSpace(yv) ? string.Empty : yv + "°";
            kv["SumRa32"] = "3.2"; kv["SumRa125"] = "12.5";

            string maskinVal = DecodeBasic(GetString(req, "MaskinVal", "Maskin Val", "MachineSelection", "MV", "MachineNumber"));
            string tmpMV = string.Equals(maskinVal, "Nakamura/K&T", StringComparison.OrdinalIgnoreCase) ? "Nakamura" :
                           string.Equals(maskinVal, "MaxMuller/K&T", StringComparison.OrdinalIgnoreCase) ? "MaxMuller" : string.Empty;
            kv["TmpMV"] = tmpMV;
            kv["TmpMaskinValS1"] = tmpMV; kv["SumMaskinValS1"] = string.IsNullOrWhiteSpace(tmpMV) ? string.Empty : "Maskin: " + tmpMV + " - Svarvning";
            kv["TmpMaskinValS2"] = string.IsNullOrWhiteSpace(tmpMV) ? string.Empty : "K&T";
            kv["SumMaskinValS2"] = string.IsNullOrWhiteSpace(tmpMV) ? string.Empty : "Maskin: K&T - Borrning, Fräsning";

            AddMeasurementRows(kv, !string.IsNullOrWhiteSpace(tmpMV));

            kv["SumTextS1"] = "Skarpa kanter avgradas.";
            kv["SumTextS2"] = "Okulär kontroll av bearbetade ytor, vid misstänkt formfel lämnas hylsan till mätrum för kontroll.<<LineBreak>>Skarpa kanter avgradas.";
            string rit = GetString(req, "Ritningsnummer", "DrawingNumber", "Ritning");
            string sumRit = string.IsNullOrWhiteSpace(rit) || rit == "0" ? tmpBet : rit;
            kv["SumRitS1"] = sumRit; kv["SumRitS2"] = sumRit;
            kv["VaLFärdig"] = string.Empty; kv["VaLInfo"] = string.Empty;
            return kv;
        }

        private static void SetValue(Dictionary<string, string> kv, string name, double value, string sum, string tol)
        {
            kv["Tmp" + name] = Fmt(value); kv["Sum" + name] = sum; kv["Sum" + name + "Tol"] = tol;
        }

        private static void SetValue(Dictionary<string, string> kv, string name, double value, string sum, string tol, string tolN)
        {
            kv["Tmp" + name] = Fmt(value); kv["Sum" + name] = sum;
            kv["Tmp" + name + "Tol"] = CleanTol(tol);
            kv["Sum" + name + "Tol"] = tol;
            kv["Tmp" + name + "TolN"] = CleanTol(tolN);
            kv["Sum" + name + "TolN"] = tolN;
        }

        private static string CleanTol(string text)
        {
            return (text ?? string.Empty).Replace("+ ", string.Empty).Replace("- ", string.Empty).Replace("± ", string.Empty);
        }

        private static void AddMeasurementRows(Dictionary<string, string> kv, bool hasMachine)
        {
            string f = hasMachine ? "1/2" : string.Empty;
            string inst = hasMachine ? "Inst." : string.Empty;
            string f15 = hasMachine ? "1/5" : string.Empty;
            kv["SumF1_1"] = f; kv["SumF1_2"] = f; kv["SumF1_3"] = f; kv["SumF1_4"] = f;
            kv["SumF1_5"] = inst; kv["SumF1_6"] = inst; kv["SumF1_7"] = inst; kv["SumF1_8"] = f;
            kv["SumF2_1"] = f; kv["SumF2_2"] = f; kv["SumF2_3"] = f; kv["SumF2_4"] = inst; kv["SumF2_5"] = f15; kv["SumF2_6"] = string.Empty;

            string sk = hasMachine ? "Skjutmått" : string.Empty;
            kv["SumD1_1"] = sk; kv["SumD1_2"] = sk; kv["SumD1_3"] = sk;
            kv["SumD1_4"] = hasMachine ? "Skjutmått/Djupmått" : string.Empty;
            kv["SumD1_5"] = sk; kv["SumD1_6"] = hasMachine ? "Radielyra" : string.Empty;
            kv["SumD1_7"] = hasMachine ? "Vinkelsystem" : string.Empty; kv["SumD1_8"] = hasMachine ? "Ytjämnhetsmätare" : string.Empty;
            kv["SumD2_1"] = hasMachine ? "Gängtolk min/max" : string.Empty; kv["SumD2_2"] = sk; kv["SumD2_3"] = sk;
            kv["SumD2_4"] = hasMachine ? "Mätmaskin" : string.Empty; kv["SumD2_5"] = hasMachine ? "Ytjämnhetsmätare" : string.Empty; kv["SumD2_6"] = string.Empty;

            for (int i = 1; i <= 8; i++) kv["SumAF1_" + i.ToString(CultureInfo.InvariantCulture)] = string.Empty;
            kv["SumAF2_1"] = string.Empty; kv["SumAF2_2"] = string.Empty; kv["SumAF2_3"] = string.Empty;
            kv["SumAF2_4"] = hasMachine ? "Vid misstänkt fel lämna till mätrum." : string.Empty;
            kv["SumAF2_5"] = string.Empty; kv["SumAF2_6"] = string.Empty;

            for (int i = 1; i <= 8; i++)
            {
                kv["SumF3_" + i.ToString(CultureInfo.InvariantCulture)] = string.Empty;
                kv["SumD3_" + i.ToString(CultureInfo.InvariantCulture)] = string.Empty;
                kv["SumAF3_" + i.ToString(CultureInfo.InvariantCulture)] = string.Empty;
            }
        }

        private static double InOrDefault(object req, int idx, bool forceDefault, IReadOnlyList<double> list, params string[] names)
        {
            if (!forceDefault)
            {
                double input = GetDouble(req, names);
                if (Math.Abs(input) > 0.0000001) return input;
            }
            return idx > 0 && idx <= list.Count ? list[idx - 1] : 0d;
        }

        private static string Present(string s)
        {
            if (double.TryParse((s ?? string.Empty).Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double d) && Math.Abs(d) > 0.0000001) return Fmt(d);
            return s ?? string.Empty;
        }

        private static int MemberIndex(string value, IEnumerable<string> items)
        {
            int i = 1;
            foreach (string item in items)
            {
                if (string.Equals((value ?? string.Empty).Trim(), item, StringComparison.OrdinalIgnoreCase)) return i;
                i++;
            }
            return 0;
        }

        private static string TolOuter(double d) { return d < 120.01 ? "± 0.3" : d < 400.01 ? "± 0.5" : "± 0.8"; }
        private static double H11(double d) { return d < 120.01 ? 0.22 : d < 180.01 ? 0.25 : d < 250.01 ? 0.29 : d < 315.01 ? 0.32 : d < 400.01 ? 0.36 : d < 500.01 ? 0.40 : d < 630.01 ? 0.44 : d < 800.01 ? 0.50 : 0.56; }
        private static string TolJs13(double d) { return d < 120.01 ? "± 0.270" : d < 180.01 ? "± 0.315" : d < 250.01 ? "± 0.360" : d < 315.01 ? "± 0.405" : d < 400.01 ? "± 0.445" : d < 500.01 ? "± 0.485" : d < 630.01 ? "± 0.550" : d < 800.01 ? "± 0.625" : "± 0.700"; }
        private static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published) || !DateTime.TryParse(published, out var pubDt)) return string.Empty;
            int dagar = 14; DateTime validTill = pubDt.AddDays(dagar);
            return DateTime.Today <= validTill.Date ? "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom " + dagar.ToString(CultureInfo.InvariantCulture) + " dagar)\n\n\n\nInformation om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument\n\nPopupruta aktiv till " + validTill.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : string.Empty;
        }

        private static string DecodeBasic(string text)
        {
            return (text ?? string.Empty)
                .Replace("&amp;amp;", "&")
                .Replace("&AMP;AMP;", "&")
                .Replace("&amp;", "&")
                .Replace("&AMP;", "&")
                .Trim();
        }

        private static string Bool(bool b) { return b ? "1" : "0"; }
        private static string Word(string[] items, int oneBased) { return items != null && items.Length >= oneBased ? items[oneBased - 1] : string.Empty; }
        private static string Left(string s, int n) { return string.IsNullOrEmpty(s) ? string.Empty : s.Substring(0, Math.Min(n, s.Length)); }
        private static string Right(string s, int n) { return string.IsNullOrEmpty(s) ? string.Empty : s.Substring(Math.Max(0, s.Length - n)); }
        private static int ToInt(string s) { return int.TryParse((s ?? string.Empty).Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out int i) ? i : 0; }
        private static string Fmt(double v) { return Math.Abs(v - Math.Round(v)) < 0.0000001 ? ((long)Math.Round(v)).ToString(CultureInfo.InvariantCulture) : v.ToString("0.###", CultureInfo.InvariantCulture); }
        private static string FmtFixed(double v, int decimals) { return v.ToString("F" + decimals.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture); }

        private static string GetString(object source, params string[] names)
        {
            object value = GetValue(source, names);
            return Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty;
        }

        private static double GetDouble(object source, params string[] names)
        {
            string text = GetString(source, names);
            return double.TryParse((text ?? string.Empty).Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double value) ? value : 0d;
        }

        private static object GetValue(object source, params string[] names)
        {
            if (source == null) return null;

            foreach (string name in names.Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                var prop = source.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .FirstOrDefault(p =>
                        string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase) ||
                        NormalizeKey(p.Name) == NormalizeKey(name));

                if (prop != null)
                    return prop.GetValue(source, null);

                object bookmarkValue = GetBookmarkValue(source, name);
                if (bookmarkValue != null)
                    return bookmarkValue;

                if (source is IDictionary dict)
                {
                    foreach (DictionaryEntry entry in dict)
                    {
                        if (entry.Key != null && NormalizeKey(entry.Key.ToString()) == NormalizeKey(name))
                            return entry.Value;
                    }
                }

                foreach (var nestedProp in source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    object nestedValue = nestedProp.GetValue(source, null);

                    if (nestedValue is IDictionary nestedDict)
                    {
                        foreach (DictionaryEntry entry in nestedDict)
                        {
                            if (entry.Key != null && NormalizeKey(entry.Key.ToString()) == NormalizeKey(name))
                                return entry.Value;
                        }
                    }
                }
            }

            return null;
        }

        private static object GetBookmarkValue(object source, string name)
        {
            if (source == null || string.IsNullOrWhiteSpace(name)) return null;

            var bookmarksProp = source.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(p => NormalizeKey(p.Name) == "BOOKMARKS");

            if (bookmarksProp == null) return null;

            object bookmarksObj = bookmarksProp.GetValue(source, null);
            if (bookmarksObj == null || bookmarksObj is string) return null;

            if (!(bookmarksObj is IEnumerable bookmarks)) return null;

            foreach (object bookmark in bookmarks)
            {
                if (bookmark == null) continue;

                string bookmarkName = string.Empty;
                object bookmarkValue = null;

                if (bookmark is IDictionary dict)
                {
                    foreach (DictionaryEntry entry in dict)
                    {
                        string key = entry.Key == null ? string.Empty : entry.Key.ToString();

                        if (NormalizeKey(key) == "BOOKMARKNAME")
                            bookmarkName = Convert.ToString(entry.Value, CultureInfo.InvariantCulture);

                        if (NormalizeKey(key) == "BOOKMARKVALUE")
                            bookmarkValue = entry.Value;
                    }
                }
                else
                {
                    var nameProp = bookmark.GetType()
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .FirstOrDefault(p => NormalizeKey(p.Name) == "BOOKMARKNAME");

                    var valueProp = bookmark.GetType()
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .FirstOrDefault(p => NormalizeKey(p.Name) == "BOOKMARKVALUE");

                    if (nameProp != null)
                        bookmarkName = Convert.ToString(nameProp.GetValue(bookmark, null), CultureInfo.InvariantCulture);

                    if (valueProp != null)
                        bookmarkValue = valueProp.GetValue(bookmark, null);
                }

                if (NormalizeKey(bookmarkName) == NormalizeKey(name))
                    return bookmarkValue;
            }

            return null;
        }

        private static string NormalizeKey(string key)
        {
            return new string((key ?? string.Empty).Where(char.IsLetterOrDigit).Select(char.ToUpperInvariant).ToArray());
        }
    }
}
