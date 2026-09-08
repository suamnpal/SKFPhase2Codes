using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Klamhylsa_Muttersakring_Slits_SPECIAL : ITemplateCalculations
    {
        private const string LB = "<<LineBreak>>";

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req?.ProductDesignation ?? "";
            if (string.IsNullOrWhiteSpace(subject)) subject = GetStringField(req, "Subject");

            string maskinVal = req?.MachineNumber ?? "";
            if (string.IsNullOrWhiteSpace(maskinVal)) maskinVal = GetStringField(req, "MaskinVal");

            kv["VaLPopUp"] = ComputePopup(req?.Published);

            string tmpKon0 = "0.0";
            string tmpKon01 = "0.100";
            string tmpKon015 = "0.150";
            string tmpKon02 = "0.200";
            string tmpKon03 = "0.300";
            string tmpKon04 = "0.400";
            string tmpKon05 = "0.500";
            string tmpKon06 = "0.600";
            string tmpKon09 = "0.900";
            string tmpKon50 = "5.0";
            string tmpKon10 = "1.0";

            string motsvararStdTyp = GetStringField(req, "Motsvarar std. typ");
            if (string.IsNullOrWhiteSpace(motsvararStdTyp)) motsvararStdTyp = "0";

            string tmpFormat = motsvararStdTyp == "0" ? subject.Trim().ToUpperInvariant() : motsvararStdTyp.Trim().ToUpperInvariant();

            string tmpBet = tmpFormat.Replace(".", ",");

            bool tmpSlash = tmpBet.Contains("/");
            bool tmpOH = tmpBet.Contains("OH");
            bool tmpVZ = tmpBet.Contains("VZ");
            bool tmpV21 = tmpBet.Contains("V21");
            bool tmpASA = tmpBet.Contains("ASA");
            bool tmpMS = tmpBet.Contains("MS");

            string[] parts = tmpBet.Split(new[] { ' ', '/', '.', '-', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);

            string tmpBet1a = parts.Length > 0 ? parts[0] : "";
            string tmpBet2a = parts.Length > 1 ? parts[1] : "";
            string tmpBet3a = parts.Length > 2 ? parts[2] : "";
            string tmpBet4a = parts.Length > 3 ? parts[3] : "";
            string tmpBet5a = parts.Length > 4 ? parts[4] : "";
            string tmpBet6a = parts.Length > 5 ? parts[5] : "";

            string tmpBet1b = string.IsNullOrWhiteSpace(tmpBet1a) ? "0" : tmpBet1a;
            string tmpBet2b = string.IsNullOrWhiteSpace(tmpBet2a) ? "0" : tmpBet2a;
            string tmpBet3b = string.IsNullOrWhiteSpace(tmpBet3a) ? "0" : tmpBet3a;
            string tmpBet4b = string.IsNullOrWhiteSpace(tmpBet4a) ? "0" : tmpBet4a;
            string tmpBet5b = string.IsNullOrWhiteSpace(tmpBet5a) ? "0" : tmpBet5a;
            string tmpBet6b = string.IsNullOrWhiteSpace(tmpBet6a) ? "0" : tmpBet6a;

            bool tmpBet1c = double.TryParse(tmpBet1b.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out _);
            bool tmpBet2c = double.TryParse(tmpBet2b.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out _);
            bool tmpBet3c = double.TryParse(tmpBet3b.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out _);
            bool tmpBet4c = double.TryParse(tmpBet4b.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out _);
            bool tmpBet5c = double.TryParse(tmpBet5b.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out _);
            bool tmpBet6c = double.TryParse(tmpBet6b.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out _);

            object tmpBet1 = tmpBet1c && tmpBet1b != "0" ? (object)ParseDouble(tmpBet1a) : tmpBet1a;
            object tmpBet2 = tmpBet2c && tmpBet2b != "0" ? (object)ParseDouble(tmpBet2a) : tmpBet2a;
            object tmpBet3 = tmpBet3c && tmpBet3b != "0" ? (object)ParseDouble(tmpBet3a) : tmpBet3a;
            object tmpBet4 = tmpBet4c && tmpBet4b != "0" ? (object)ParseDouble(tmpBet4a) : tmpBet4a;
            object tmpBet5 = tmpBet5c && tmpBet5b != "0" ? (object)ParseDouble(tmpBet5a) : tmpBet5a;
            object tmpBet6 = tmpBet6c && tmpBet6b != "0" ? (object)ParseDouble(tmpBet6a) : tmpBet6a;

            int tmpCountB = tmpBet.Length;
            int tmpCountB1 = tmpBet1.ToString().Length;
            int tmpCountB2 = tmpBet2.ToString().Length;
            int tmpCountB3 = tmpBet3.ToString().Length;

            bool tmpTecken = tmpSlash ? tmpCountB > 9 : false;

            double innerdiaOverride = GetDoubleField(req, "Innerdiameter (d1)");

            bool tmpSpecDia = tmpTecken || innerdiaOverride != 0 || string.Equals(tmpBet5.ToString(), "HB", StringComparison.OrdinalIgnoreCase);

            string tmpSerie = tmpSlash && (tmpCountB2 == 3 || tmpCountB2 == 2) ? tmpBet2.ToString() : tmpCountB2 > 4 ? tmpBet2.ToString().Substring(0, 3) : tmpCountB2 == 3 ? tmpBet2.ToString().Substring(0, 1) : tmpBet2.ToString().Substring(0, Math.Min(2, tmpBet2.ToString().Length));

            double tmpTyp = !tmpSlash ? ParseDouble(Right(tmpBet2.ToString(), 2)) : tmpCountB2 > 3 ? ParseDouble(Right(tmpBet2.ToString(), 2)) : ParseDouble(tmpBet3.ToString());

            string[] tmpTypValues = { "64", "68", "72", "76", "80", "84", "88", "92", "96", "500", "530", "560", "600", "630", "670", "710", "750", "800", "850", "900", "950", "1000", "1060" };

            int tmpTypLista = Array.IndexOf(tmpTypValues, Convert.ToInt32(tmpTyp).ToString(CultureInfo.InvariantCulture)) + 1;

            double vinkelLasSpar = GetDoubleField(req, "Vinkel till låsspår");
            double tmpVa = vinkelLasSpar == 0 ? 11.25 : vinkelLasSpar;

            kv["SumVa"] = Smart(tmpVa) + "º";
            kv["SumV"] = kv["SumVa"];

            double vinkelFrasSpar = GetDoubleField(req, "Vinkel frässpår");
            double tmpV30 = vinkelFrasSpar == 0 ? 30 : vinkelFrasSpar;

            kv["SumV30"] = Smart(tmpV30) + "º";

            double tmpC = tmpSerie == "32" ? (tmpTyp < 93 ? 8 : 10) : tmpSerie == "39" ? (tmpTyp < 530 ? 8 : 10) : tmpTyp < 501 ? 8 : 10;

            string cRaw = GetStringField(req, "Slits (c)");

            kv["SumC"] = string.IsNullOrWhiteSpace(cRaw) || cRaw == "0"? "(c) " + Smart(tmpC): cRaw;

            kv["SumCTol"] = "± 0.2";

            double tmpd = tmpCountB2 > 3 ? tmpTyp / 2 * 10 : ParseDouble(tmpBet3.ToString());

            bool tmpNull4 = string.IsNullOrWhiteSpace(tmpBet4a);

            double tmpd1 = innerdiaOverride == 0? (!tmpTecken ? (tmpTyp < 85 ? tmpd - 20 :
                (tmpTyp < 561 || tmpTyp == 630) ? tmpd - 30 : tmpTyp < 751 ? tmpd - 40 : tmpTyp < 1001 ? tmpd - 50 : tmpd - 60)
                : (tmpNull4 ? ParseDouble(tmpBet3.ToString()): ParseDouble(tmpBet4.ToString()))) : innerdiaOverride;

            kv["Sumd1"] = "(d1) " + Smart(tmpd1);

            double tmpd1Tol = tmpTyp < 65 ? 0.21 : tmpTyp < 85 ? 0.36 : tmpTyp < 531 ? 0.40 : tmpTyp < 671 ? 0.44 : tmpTyp < 851 ? 0.50 : 0.56;
            double tmpd1TolN = tmpTyp < 65 ? 0.32 : tmpTyp < 85 ? 0.57 : tmpTyp < 531 ? 0.63 : tmpTyp < 671 ? 0.70 : tmpTyp < 851 ? 0.80 : 0.90;

            kv["Sumd1Tol"] = "+ " + F3(tmpd1Tol);
            kv["Sumd1TolN"] = "- " + F3(tmpd1TolN);

            string[] tmpFLista = tmpSerie == "240" ? new[] { "0", "34", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "50", "50", "0", "53", "0", "0", "0", "0", "60", "0", "0" }
                              : tmpSerie == "241" ? new[] { "0", "35", "36", "0", "0", "0", "0", "0", "0", "42", "34", "0", "50", "50", "0", "53", "0", "0", "0", "0", "0", "0", "0" }
                              : new[] { "25", "26", "26", "26", "27", "27", "27", "28", "28", "28", "34", "34", "35", "35", "36", "38", "38", "39", "39", "40", "42", "44", "44" };

            string fRaw = GetStringField(req, "Låsspårslängd (f)");

            double tmpF = string.IsNullOrWhiteSpace(fRaw) || fRaw == "0" ? ParseDouble(tmpFLista[tmpTypLista - 1]) : ParseDouble(fRaw);

            kv["SumF"] = "(f) " + Smart(tmpF);
            kv["SumF2"] = kv["SumF"];

            double tmpFTol = tmpF > 50 ? 3 : tmpF > 30 ? 2.5 : tmpF > 19 ? 2.1 : 1.8;

            kv["SumFTol"] = "+ " + F2(tmpFTol);
            kv["SumF2Tol"] = kv["SumFTol"];
            kv["SumFTolN"] = "- " + tmpKon0 + " [3F]";
            kv["SumF2TolN"] = kv["SumFTolN"];

            string[] tmpELista = tmpSerie == "240" ? new[] { "24", "24", "28", "32", "32", "32", "36", "36", "36", "40", "40", "45", "45", "50", "50", "55", "60", "60", "70", "70", "60", "70", "70" }
                      : (tmpSerie == "31" || tmpSerie == "32" || tmpSerie == "241") ? new[] { "24", "28", "28", "32", "32", "32", "36", "36", "36", "40", "40", "45", "45", "50", "50", "55", "60", "60", "70", "70", "70", "70", "70" }
                      : (tmpSerie == "30" || tmpSerie == "39") ? new[] { "24", "24", "28", "28", "28", "32", "32", "32", "36", "36", "40", "40", "40", "45", "45", "50", "55", "55", "60", "60", "60", "60", "60" }
                      : new[] { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };

            string eRaw = GetStringField(req, "Låsspårsbredd (e)");

            double tmpE = string.IsNullOrWhiteSpace(eRaw) || eRaw == "0" ? ParseDouble(tmpELista[tmpTypLista - 1])  : ParseDouble(eRaw);

            kv["SumE"] = "(e) " + Smart(tmpE);

            double tmpETol = tmpE > 50 ? 0.74 : tmpE > 30 ? 0.62 : tmpE > 18 ? 0.52 : tmpE > 10 ? 0.43 : tmpE > 6 ? 0.36 : 0.30;

            kv["SumETol"] = "+ " + F3(tmpETol);
            kv["SumETolN"] = "- " + tmpKon0 + " [3F]";

            string tmpMaskinValS1 = maskinVal == "Skepp6" ? "Skepp6" : maskinVal == "K&T" ? "K&T" : maskinVal == "VTR-160" ? "VTR-160" : maskinVal == "MacTurn 550" ? "MacTurn 550" : "";

            kv["SumMaskinValS1"] = "Maskin: " + tmpMaskinValS1 + " - Muttersäkring, Slits";

            bool isSkepp6 = EqualsI(maskinVal, "Skepp6");
            bool isHalf = EqualsI(maskinVal, "K&T") || EqualsI(maskinVal, "VTR-160") || EqualsI(maskinVal, "MacTurn 550");

            kv["SumF1_1"] = isSkepp6 ? "1/1" : isHalf ? "1/2" : "";
            kv["SumF1_2"] = kv["SumF1_1"];
            kv["SumF1_3"] = kv["SumF1_1"];
            kv["SumF1_4"] = kv["SumF1_1"];
            kv["SumF1_5"] = isSkepp6 ? "1/1" : isHalf ? "1/2" : "";

            bool machineEnabled = isSkepp6 || EqualsI(maskinVal, "K&T") || EqualsI(maskinVal, "VTR-160") || EqualsI(maskinVal, "MacTurn 550");

            kv["SumD1_1"] = machineEnabled ? "Skjutmått" : "";
            kv["SumD1_2"] = machineEnabled ? "Skjutmått" : "";
            kv["SumD1_3"] = machineEnabled ? "Skjutmått" : "";
            kv["SumD1_4"] = "";
            kv["SumD1_5"] = machineEnabled ? "Skjutmått" : "";

            kv["SumAF1_1"] = "";
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = "";

            kv["SumTextS1"] = "Okulärkontroll Grader, frifläckar, slagmärken, repor, valkar, ytjämnhet och faser, skarpa kanter avgradas.";

            string tmpRit = tmpSerie == "30" ? "7438957" : tmpSerie == "31" ? "7438958" : tmpSerie == "32" ? "7438955" :
                            tmpSerie == "39" ? "7434032" : tmpSerie == "241" ? "7432903" : tmpSerie == "240" ? "7432901" : tmpBet + ":senaste utg.";

            string ritningsnummer = GetStringField(req, "Ritningsnummer");

            if (string.IsNullOrWhiteSpace(ritningsnummer)) ritningsnummer = "0";

            kv["SumRitNr"] = ritningsnummer == "0" ? tmpRit : ritningsnummer == "1" ? subject : ritningsnummer;

            return kv;
        }
        private static bool EqualsI(string a, string b) => string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);

        private static string GetStringField(APIRequest req, string key)
        {
            if (req == null || string.IsNullOrWhiteSpace(key)) return "";

            string value;
            return TryGetFromDictionaries(req, key, out value) ? value ?? "" : "";
        }

        private static double GetDoubleField(APIRequest req, string key)
        {
            string s = GetStringField(req, key);

            if (string.IsNullOrWhiteSpace(s)) return 0;

            s = s.Trim().Replace(',', '.');

            return double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var v) ? v : 0;
        }

        private static bool TryGetFromDictionaries(object obj, string key, out string value)
        {
            value = null;

            if (obj == null) return false;

            foreach (var p in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!p.CanRead) continue;

                object pv;

                try
                {
                    pv = p.GetValue(obj, null);
                }
                catch
                {
                    continue;
                }

                if (pv == null) continue;

                if (pv is IDictionary<string, string> dictSS)
                {
                    if (dictSS.TryGetValue(key, out value))
                        return true;

                    foreach (var kvp in dictSS)
                    {
                        if (string.Equals(kvp.Key ?? "", key, StringComparison.OrdinalIgnoreCase))
                        {
                            value = kvp.Value;
                            return true;
                        }
                    }
                }

                if (pv is IDictionary<string, object> dictSO)
                {
                    if (dictSO.TryGetValue(key, out object ov))
                    {
                        value = ov?.ToString() ?? "";
                        return true;
                    }

                    foreach (var kvp in dictSO)
                    {
                        if (string.Equals(kvp.Key ?? "", key, StringComparison.OrdinalIgnoreCase))
                        {
                            value = kvp.Value?.ToString() ?? "";
                            return true;
                        }
                    }
                }

                if (pv is IDictionary nonGen)
                {
                    foreach (DictionaryEntry de in nonGen)
                    {
                        if (de.Key == null) continue;

                        if (string.Equals(de.Key.ToString(), key, StringComparison.OrdinalIgnoreCase))
                        {
                            value = de.Value?.ToString() ?? "";
                            return true;
                        }
                    }
                }

                if (pv is IEnumerable enumerable && !(pv is string))
                {
                    foreach (var item in enumerable)
                    {
                        if (item == null) continue;

                        var itemType = item.GetType();

                        var nameProp = itemType.GetProperty("BookmarkName");
                        var valueProp = itemType.GetProperty("BookmarkValue");

                        if (nameProp != null && valueProp != null)
                        {
                            string bookmarkName = nameProp.GetValue(item)?.ToString();

                            if (string.Equals(bookmarkName, key, StringComparison.OrdinalIgnoreCase))
                            {
                                value = valueProp.GetValue(item)?.ToString() ?? "";
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
        private static double ParseDouble(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return 0;

            double.TryParse(value.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double result);

            return result;
        }

        private static string Smart(double value) => Math.Abs(value % 1) < 0.0001 ? value.ToString("F0", CultureInfo.InvariantCulture)
                : value.ToString("0.###", CultureInfo.InvariantCulture).Replace(".", ",");

        private static string F2(double value) => value.ToString("F2", CultureInfo.InvariantCulture).Replace(",", ".");

        private static string F3(double value) => value.ToString("F3", CultureInfo.InvariantCulture).Replace(",", ".");

        private static string Right(string value, int length) => string.IsNullOrEmpty(value) ? "" : value.Length <= length ? value : value.Substring(value.Length - length);

        private static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published)) return "";

            if (!DateTime.TryParse(published, out DateTime publishDate)) return "";

            DateTime validTill = publishDate.AddDays(14);

            if (DateTime.Today > validTill.Date) return "";

            return "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)" + LB + LB + "" + LB + LB
                   + "Information om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument"
                   + LB + LB+ "Popupruta aktiv till " + validTill.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
    }
}
