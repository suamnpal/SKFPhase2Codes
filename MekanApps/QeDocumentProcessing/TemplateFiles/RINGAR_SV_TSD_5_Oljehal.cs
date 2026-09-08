using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class RINGAR_SV_TSD_5_Oljehal : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();
            string subject = req?.ProductDesignation ?? "";
            string maskinVal = req?.MachineNumber ?? "";
            string tmpFormat = subject.Trim().ToUpperInvariant();
            string tmpBet = tmpFormat.Replace(".", ",");
            bool tmpSlash = tmpBet.Contains("/");
            bool tmpVZ2M4 = tmpBet.Contains("VZ2M4");
            bool tmpG = tmpBet.Contains("G");
            bool tmpVZ = tmpBet.Contains("VZ861");
            bool tmpVZ2L1 = tmpBet.Contains("VZ2L1");
            bool tmpV21 = tmpBet.Contains("V21");
            bool tmpV212 = tmpBet.Contains("V21-2");
            bool tmpV213 = tmpBet.Contains("V21-3");
            bool tmpV22Dash = tmpBet.Contains("V22-");
            string[] parts = tmpBet.Split(new[] { ' ', '/', '.' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1a = parts.Length > 0 ? parts[0] : "";
            string tmpBet2a = parts.Length > 1 ? parts[1] : "";
            string tmpBet3a = parts.Length > 2 ? parts[2] : "";
            string tmpBet4a = parts.Length > 3 ? parts[3] : "";
            string tmpBet5a = parts.Length > 4 ? parts[4] : "";
            string tmpBet6a = parts.Length > 5 ? parts[5] : "";
            string tmpBet1b = string.IsNullOrEmpty(tmpBet1a) ? "0" : tmpBet1a;
            string tmpBet2b = string.IsNullOrEmpty(tmpBet2a) ? "0" : tmpBet2a;
            string tmpBet3b = string.IsNullOrEmpty(tmpBet3a) ? "0" : tmpBet3a;
            string tmpBet4b = string.IsNullOrEmpty(tmpBet4a) ? "0" : tmpBet4a;
            string tmpBet5b = string.IsNullOrEmpty(tmpBet5a) ? "0" : tmpBet5a;
            string tmpBet6b = string.IsNullOrEmpty(tmpBet6a) ? "0" : tmpBet6a;
            object tmpBet1 = IsNumeric(tmpBet1b) && ParseDouble(tmpBet1b) != 0 ? ParseDouble(tmpBet1a) : (object)tmpBet1a;
            object tmpBet2 = IsNumeric(tmpBet2b) && ParseDouble(tmpBet2b) != 0 ? ParseDouble(tmpBet2a) : (object)tmpBet2a;
            object tmpBet3 = IsNumeric(tmpBet3b) && ParseDouble(tmpBet3b) != 0 ? ParseDouble(tmpBet3a) : (object)tmpBet3a;
            object tmpBet4 = IsNumeric(tmpBet4b) && ParseDouble(tmpBet4b) != 0 ? ParseDouble(tmpBet4a) : (object)tmpBet4a;
            object tmpBet5 = IsNumeric(tmpBet5b) && ParseDouble(tmpBet5b) != 0 ? ParseDouble(tmpBet5a) : (object)tmpBet5a;
            object tmpBet6 = IsNumeric(tmpBet6b) && ParseDouble(tmpBet6b) != 0 ? ParseDouble(tmpBet6a) : (object)tmpBet6a;
            double bet2 = ParseDouble(LotusText(tmpBet2));
            double bet3 = ParseDouble(LotusText(tmpBet3));
            double dBookmark = BookmarkDouble(req, "Ø D");
            double d3Bookmark = BookmarkDouble(req, "Ø D3");
            double d4Bookmark = BookmarkDouble(req, "Ø D4");
            double d5Bookmark = BookmarkDouble(req, "Ø D5");
            double bBookmark = BookmarkDouble(req, "Bredd (B)");
            double b2Bookmark = BookmarkDouble(req, "Bredd (B2)");
            double b3Bookmark = BookmarkDouble(req, "Bredd (B3)");
            double b4Bookmark = BookmarkDouble(req, "Bredd (B4)");
            double bdBookmark = BookmarkDouble(req, "Borrdjup (BD)");
            double bmAngleBookmark = BookmarkDouble(req, "Vinkel borrhål (BM)");
            double dobBookmark = BookmarkDouble(req, "Ø Borroljehål (DOB)");
            double vobBookmark = BookmarkDouble(req, "Vinkel oljeborrhål (VOB)");
            string rtoBookmarkText = Bookmark(req, "Radie till Oljeborrhål");
            double rtoBookmark = ParseDouble(rtoBookmarkText);
            string tmpDLista = bet2 == 3134 && tmpV21 ? "{226:202,5:184,5}" : bet2 == 3092 && bet3 == 3284 ? "{520:497:465}" : bet2 == 3080 || bet2 == 3180 ? "{460:437:415,5}" : bet2 == 3260 ? "{400:377:341}" : "{0:0:0}";
            double tmpManIn = d3Bookmark + dBookmark;
            double tmpD3 = d3Bookmark == 0 ? ParseDouble(ListItem(tmpDLista, 2)) : d3Bookmark;
            kv["SumD3"] = tmpManIn == 0 ? "(D3) " + Smart(tmpD3) : d3Bookmark == 0 ? "Mata in (D3)" : "(D3) " + Smart(tmpD3);
            kv["SumD3Tol"] = IsoTol(tmpD3);
            double tmpD = dBookmark == 0 ? ParseDouble(ListItem(tmpDLista, 1)) : dBookmark;
            kv["SumD"] = tmpManIn == 0 ? "(D) " + Smart(tmpD) : dBookmark == 0 ? "Mata in (D)" : "(D) " + Smart(tmpD);
            bool tmpDTol = In(bet2, 3040, 3044, 3056, 3064, 3080, 3138, 3144, 3148, 3164, 3172, 3180, 3256, 3272);
            kv["SumDTol"] = tmpDTol ? "+ 0" : IsoTol(tmpD);
            kv["SumDTolN"] = tmpDTol ? H12Minus(tmpD) : "";
            double tmpDD3 = tmpD - tmpD3;
            double tmpD1 = (bet2 == 3044 && !tmpV22Dash) || (bet2 == 3144 && tmpG) ? tmpD - 2 : tmpD;
            kv["SumD1"] = "(D1) " + Smart(tmpD1);
            kv["SumD1Tol"] = IsoTol(tmpD1);
            double tmpD2 = (bet2 == 3044 && !tmpV22Dash) || (bet2 == 3144 && tmpG) ? tmpD - 8 : tmpD - 9;
            kv["SumD2"] = "(D2) " + Smart(tmpD2);
            kv["SumD2Tol"] = "+ 0 [2]";
            kv["SumD2TolN"] = H8Minus(tmpD2) + " [3]";
            double tmpD4 = d4Bookmark == 0 ? ParseDouble(ListItem(tmpDLista, 3)) : d4Bookmark;
            kv["SumD4"] = "(D4) " + Smart(tmpD4);
            kv["SumD4Tol"] = H12Plus(tmpD4);
            kv["SumD4TolN"] = "- 0";
            double tmpD5 = d5Bookmark == 0 ? Nearly(tmpDD3, 21) ? tmpD3 + 6 : Nearly(tmpDD3, 21.5) ? tmpD3 + 3.5 : Nearly(tmpDD3, 23) ? tmpD3 + 5 : Nearly(tmpDD3, 23.5) ? tmpD3 + 5.5 : 0 : d5Bookmark;
            bool tmpD5Missing = d5Bookmark == 0 && !Nearly(tmpDD3, 21) && !Nearly(tmpDD3, 21.5) && !Nearly(tmpDD3, 23) && !Nearly(tmpDD3, 23.5);
            kv["SumD5"] = "(D5) " + (tmpD5Missing ? "SAKNAS" : Smart(tmpD5));
            bool tmpD5Tol = In(bet2, 3040, 3044, 3056, 3064, 3080, 3138, 3144, 3148, 3164, 3172, 3180, 3256, 3272);
            kv["SumD5Tol"] = tmpD5Tol ? H12Plus(tmpD5) : "";
            kv["SumD5TolN"] = tmpD5Tol ? "- 0" : IsoTol(tmpD5);
            double tmpB = bBookmark == 0 ? bet2 == 3134 && tmpV21 ? 24 : (bet2 == 3144 && tmpG) || bet2 == 3044 ? 22.5 : 26 : bBookmark;
            kv["SumB"] = "(B) " + Smart(tmpB);
            kv["SumBTol"] = IsoTol(tmpB);
            double tmpB1 = 10;
            kv["SumB1"] = "(B1) " + Smart(tmpB1);
            kv["SumB1Tol"] = "+ 0 [2]";
            kv["SumB1TolN"] = H8Minus(tmpB1) + " [3]";
            double tmpB2 = (bet2 == 3144 && tmpG) || (bet2 == 3044 && !tmpV22Dash) ? 6 : bet2 == 3148 ? 7.5 : bet2 == 3134 && (tmpV21 || bet3 == 160) ? 4.8 : 6.8;
            kv["SumB2"] = b2Bookmark == 0 ? "(B2) " + Smart(tmpB2) : Smart(b2Bookmark);
            kv["SumB2Tol"] = IsoTol(tmpB2);
            double tmpB3 = (bet2 == 3144 && tmpG) || bet2 == 3044 ? 9 : 11;
            kv["SumB3"] = b3Bookmark == 0 ? "(B3) " + Smart(tmpB3) : Smart(b3Bookmark);
            kv["SumB3Tol"] = IsoTol(tmpB3);
            double tmpB4 = (bet2 == 3144 && tmpG) || bet2 == 3044 ? 8.5 : 9.8;
            kv["SumB4"] = b4Bookmark == 0 ? "(B4) " + Smart(tmpB4) : Smart(b4Bookmark);
            kv["SumB4Tol"] = IsoTol(tmpB4);
            double tmpBM = 5;
            kv["SumBM"] = "(BM) " + Smart(tmpBM);
            kv["SumBMTol"] = tmpBM < 3.1 ? "+ 0.100" : tmpBM < 6.1 ? "+ 0.120" : "+ 0.150";
            kv["SumBMTolN"] = "- 0";
            double tmpBD = (bet2 == 3144 && tmpG) || bet2 == 3044 ? 5 : 6;
            kv["SumBD"] = bdBookmark == 0 ? "(BD) " + Smart(tmpBD) : Smart(bdBookmark);
            kv["SumBDTol"] = IsoTolLotus(tmpBD);
            double tmpA = tmpB1 / 2;
            kv["SumA"] = "(BD1) " + Smart(tmpA);
            kv["SumATol"] = IsoTol(tmpA);
            double tmpDOB = dobBookmark == 0 ? bet2 == 3134 && tmpV21 ? 8 : bet2 == 3134 ? 8 : bet2 == 3092 && bet3 == 3284 ? 15 : In(bet2, 3080, 3172, 3272, 3180) ? 10 : bet2 == 3260 || bet2 == 3168 ? 12 : 0 : dobBookmark;
            kv["SumDOB"] = "(DOB) " + Smart(tmpDOB);
            double tmpR = rtoBookmarkText == "0" || string.IsNullOrWhiteSpace(rtoBookmarkText) ? (tmpD5 - tmpDOB) / 2 : rtoBookmark;
            double tmpDR = (tmpD / 2) - tmpR;
            kv["SumDR"] = "(DR) " + Smart(tmpDR);
            double tmpVOB = vobBookmark;
            kv["SumVOB"] = Smart(tmpVOB) + "º";
            double tmpKonst = Math.Round(Math.Sin((tmpVOB / 2) * Math.PI / 180) * 2, 4);
            string tmpAOB = Smart(tmpVOB > 10 ? tmpVOB - tmpDOB : Math.Round((tmpKonst * tmpR) - tmpDOB, 1));
            kv["SumHM"] = "Hjälpmått " + tmpAOB + " (" + Smart(tmpVOB) + "º)";
            double tmpM = tmpB - tmpB1 - tmpB2;
            kv["SumM"] = "(M) " + Smart(tmpM);
            kv["SumMTol"] = tmpM < 6 ? "± 0.1" : tmpM < 30 ? "± 0.2" : tmpM < 120 ? "± 0.3" : "± 0.5";
            kv["SumV45"] = "45º";
            kv["SumV45_2"] = kv["SumV45"];
            kv["SumF45"] = "1x45º";
            double tmpVBMCalc = bmAngleBookmark == 0
                ? Math.Round((Math.Atan2((tmpD / 2.0) - tmpBD, (tmpBM / 2.0) + 0.3) * 180.0) / Math.PI, 1)
                : bmAngleBookmark;

            bool useComplementaryVBMAngle =
                EqualsI(tmpFormat, "SV-TSD 3180 U") ||
                EqualsI(tmpFormat, "SV-TSD 3160 U/V21") ||
                EqualsI(tmpFormat, "SV-TSD 3080 U") ||
                EqualsI(tmpFormat, "SV-TSD 3068 U");

            double tmpVBMDisplay = useComplementaryVBMAngle
                ? Math.Round(90.0 - tmpVBMCalc, 1)
                : tmpVBMCalc;

            kv["SumV15"] = Smart1(tmpVBMDisplay).Replace(",", ".") + "º";

            // Use the same effective angle for both SumV15 and the chord calculation.
            double tmpKonstKorda = Math.Sin(((90.0 - tmpVBMDisplay) / 2.0) * Math.PI / 180.0) * 2.0;
            double tmpKordaBM = Math.Round((tmpD / 2.0) * tmpKonstKorda, 1);
            kv["SumKordaBM"] = "Korda kant till kant " + Smart(tmpKordaBM);
            string tmpRa = "6.3";
            kv["SumRit"] = tmpBet;
            kv["SumRit2"] = kv["SumRit"];
            bool machineEnabled = EqualsI(maskinVal, "Nakamura") || EqualsI(maskinVal, "MaxMuller/Skepp6") || EqualsI(maskinVal, "LB45");
            string svarvning = EqualsI(maskinVal, "Nakamura") ? "Nakamura" : EqualsI(maskinVal, "MaxMuller/Skepp6") ? "MaxMuller" : EqualsI(maskinVal, "LB45") ? "LB45" : "";
            string borrning = EqualsI(maskinVal, "Nakamura") ? "Nakamura" : EqualsI(maskinVal, "MaxMuller/Skepp6") ? "Skepp 6" : EqualsI(maskinVal, "LB45") ? "LB45" : "";
            kv["SumMaskinValS1"] = "Maskin: Svarvning - " + svarvning + " ";
            kv["SumMaskinValS2"] = "Maskin: Borrning - " + borrning + " ";
            kv["SumF1_1"] = machineEnabled ? "1/1" : "";
            kv["SumF1_2"] = machineEnabled ? "1/1" : "";
            kv["SumF1_3"] = machineEnabled ? "1/3" : "";
            kv["SumF1_4"] = machineEnabled ? "1/3" : "";
            kv["SumF1_5"] = machineEnabled ? "1/5" : "";
            kv["SumF1_6"] = machineEnabled ? "Inst." : "";
            kv["SumF1_7"] = machineEnabled ? "Inst." : "";
            kv["SumF1_8"] = machineEnabled ? "1/3" : "";
            kv["SumF1_9"] = machineEnabled ? "1/3" : "";
            kv["SumF2_1"] = EqualsI(maskinVal, "Nakamura") ? "1/5" : EqualsI(maskinVal, "MaxMuller/Skepp6") || EqualsI(maskinVal, "LB45") ? "1/1" : "";
            kv["SumF2_2"] = kv["SumF2_1"];
            kv["SumF2_3"] = kv["SumF2_1"];
            kv["SumF2_4"] = kv["SumF2_1"];
            kv["SumF2_5"] = EqualsI(maskinVal, "Nakamura") ? "Inst." : EqualsI(maskinVal, "MaxMuller/Skepp6") || EqualsI(maskinVal, "LB45") ? "1/1" : "";
            kv["SumD1_1"] = machineEnabled ? "Mikrometer" : "";
            kv["SumD1_2"] = machineEnabled ? "Mikrometer" : "";
            kv["SumD1_3"] = machineEnabled ? "Digitalt Djup/Hakmått" : "";
            kv["SumD1_4"] = machineEnabled ? "Digitalt Djup/Hakmått" : "";
            kv["SumD1_5"] = machineEnabled ? "Skjutmått" : "";
            kv["SumD1_6"] = machineEnabled ? "Vinkelsystem" : "";
            kv["SumD1_7"] = machineEnabled ? "Vinkelsystem" : "";
            kv["SumD1_8"] = machineEnabled ? "Ytjämnhetsmätare" : "";
            kv["SumD1_9"] = machineEnabled ? "Skjutmått" : "";
            kv["SumD2_1"] = machineEnabled ? "Skjutmått" : "";
            kv["SumD2_2"] = machineEnabled ? "Skjutmått" : "";
            kv["SumD2_3"] = machineEnabled ? "Skjutmått" : "";
            kv["SumD2_4"] = machineEnabled ? "Skjutmått" : "";
            kv["SumD2_5"] = machineEnabled ? "Skjutmått" : "";
            kv["SumAF1_1"] = "";
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = "";
            kv["SumAF1_6"] = "";
            kv["SumAF1_7"] = "";
            kv["SumAF1_8"] = machineEnabled ? "Bearbetas Ra " + tmpRa + " runt om" : "";
            kv["SumAF1_9"] = "";
            kv["SumAF2_1"] = "";
            kv["SumAF2_2"] = "";
            kv["SumAF2_3"] = "";
            kv["SumAF2_4"] = "";
            kv["SumAF2_5"] = "";
            kv["SumTextS1"] = "Okulärkontroll Grader, frifläcker, slagmärken, repor, valkar, ytjämnhet och faser, skarpa kanter avgradas.";
            kv["SumTextS2"] = kv["SumTextS1"];
            return kv;
        }
        private static string IsoTolLotus(double value) => value < 6 ? "± 0.1" : value < 30 ? "± 0.2" : value < 120 ? "± 0.3" : value < 400 ? "± 0.5" : value < 1000 ? "± 0.8" : value < 2000 ? "± 1.2" : "± 2.0";

        private static string Bookmark(APIRequest req, string name)
        {
            if (req == null) return "";
            PropertyInfo prop = req.GetType().GetProperty("Bookmarks");
            object bookmarks = prop == null ? null : prop.GetValue(req, null);
            if (bookmarks is IEnumerable enumerable)
            {
                foreach (object item in enumerable)
                {
                    if (item == null) continue;
                    if (item is Dictionary<string, string> dict)
                    {
                        if (dict.TryGetValue("BookmarkName", out string n) && EqualsI(n, name)) return dict.TryGetValue("BookmarkValue", out string v) ? v ?? "" : "";
                    }
                    PropertyInfo nameProp = item.GetType().GetProperty("BookmarkName");
                    PropertyInfo valueProp = item.GetType().GetProperty("BookmarkValue");
                    string bookmarkName = nameProp == null ? "" : Convert.ToString(nameProp.GetValue(item, null), CultureInfo.InvariantCulture);
                    if (EqualsI(bookmarkName, name)) return valueProp == null ? "" : Convert.ToString(valueProp.GetValue(item, null), CultureInfo.InvariantCulture) ?? "";
                }
            }
            return "";
        }

        private static double BookmarkDouble(APIRequest req, string name) => ParseDouble(Bookmark(req, name));
        private static bool EqualsI(string a, string b) => string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);
        private static bool IsNumeric(string value) => !string.IsNullOrWhiteSpace(value) && double.TryParse(value.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out _);
        private static double ParseDouble(string value) => string.IsNullOrWhiteSpace(value) ? 0 : double.TryParse(value.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double result) ? result : 0;
        private static string Smart(double value) => Math.Abs(value % 1) < 0.0000001 ? value.ToString("F0", CultureInfo.InvariantCulture) : value.ToString("0.###", CultureInfo.InvariantCulture).Replace(".", ",");
        private static string Smart1(double value) => value.ToString("0.0", CultureInfo.InvariantCulture).Replace(".", ",");
        private static string LotusText(object value) => value == null ? "" : value is double d ? Smart(d) : value is float f ? Smart(f) : value is decimal m ? Smart((double)m) : value.ToString() ?? "";
        private static string ListItem(string list, int position)
        {
            if (string.IsNullOrWhiteSpace(list) || position < 1) return "";
            string[] values = list.Trim().TrimStart('{').TrimEnd('}').Split(':');
            return values.Length >= position ? values[position - 1] : "";
        }
        private static bool In(double value, params double[] values)
        {
            foreach (double item in values) if (Nearly(value, item)) return true;
            return false;
        }
        private static bool Nearly(double a, double b) => Math.Abs(a - b) < 0.0000001;
        private static string IsoTol(double value) => value <= 6 ? "± 0.1" : value < 30 ? "± 0.2" : value < 120 ? "± 0.3" : value < 400 ? "± 0.5" : value < 1000 ? "± 0.8" : value < 2000 ? "± 1.2" : "± 2.0";
        private static string H12Plus(double value) => value < 3.1 ? "+ 0.100" : value < 6.1 ? "+ 0.120" : value < 10.1 ? "+ 0.150" : value < 18.1 ? "+ 0.180" : value < 30.1 ? "+ 0.210" : value < 50.1 ? "+ 0.250" : value < 80.1 ? "+ 0.300" : value < 120.1 ? "+ 0.350" : value < 180.1 ? "+ 0.400" : value < 250.1 ? "+ 0.460" : value < 315.1 ? "+ 0.520" : value < 400.1 ? "+ 0.570" : value < 500.1 ? "+ 0.630" : "+ 0.700";
        private static string H12Minus(double value) => value < 18.1 ? "- 0.180" : value < 30.1 ? "- 0.210" : value < 50.1 ? "- 0.250" : value < 80.1 ? "- 0.300" : value < 120.1 ? "- 0.350" : value < 180.1 ? "- 0.400" : value < 250.1 ? "- 0.460" : value < 315.1 ? "- 0.520" : value < 400.1 ? "- 0.570" : value < 500.1 ? "- 0.630" : "- 0.700";
        private static string H8Minus(double value) => value < 3.1 ? "- 0.014" : value < 6.1 ? "- 0.018" : value < 10.1 ? "- 0.022" : value < 18.1 ? "- 0.027" : value < 30.1 ? "- 0.033" : value < 50.1 ? "- 0.039" : value < 80.1 ? "- 0.046" : value < 120.1 ? "- 0.054" : value < 180.1 ? "- 0.063" : value < 250.1 ? "- 0.072" : value < 315.1 ? "- 0.081" : value < 400.1 ? "- 0.089" : value < 500.1 ? "- 0.097" : "- 0.110";
    }
}
