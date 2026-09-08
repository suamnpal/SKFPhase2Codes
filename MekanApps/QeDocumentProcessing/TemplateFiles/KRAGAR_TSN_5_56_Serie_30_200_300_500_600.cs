using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class KRAGAR_TSN_5_56_Serie_30_200_300_500_600 : ITemplateCalculations
    {
        private const string LB = "<<LineBreak>>";

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();
            var bm = req?.Bookmarks;
            string subject = req?.ProductDesignation ?? "";
            string maskinVal = req?.MachineNumber ?? "";
            DateTime now = DateTime.Now;
            bool tmpHelg = now.DayOfWeek == DayOfWeek.Saturday || now.DayOfWeek == DayOfWeek.Sunday;
            bool tmp0To6 = now.Hour == 0 || now.Hour < 6;
            int tmpTillaggTid = 3;
            int tmpDay2 = now.Day + 1;
            int tmpHour2 = tmp0To6 ? 9 : now.Hour + tmpTillaggTid;
            string tmpTime = now.Hour + "," + now.Minute;
            string tmpTime2 = tmpHour2 + "," + now.Minute;
            string tmpDat2 = now.Year + "-" + now.Month + "-" + tmpDay2;
            kv["VaLPopUp"] = ComputePopup(req?.Published);

            string tmpKon0 = "0.0";
            string tmpKon01 = "0.100";
            string tmpKon02 = "0.200";
            string tmpKon03 = "0.300";
            string tmpKon04 = "0.400";
            string tmpKon05 = "0.500";

            string tmpTol05To6 = "± 0.1";
            string tmpTol6To30 = "± 0.2";
            string tmpTol30To120 = "± 0.3";
            string tmpTol120To400 = "± 0.5";
            string tmpTol400To1000 = "± 0.8";
            string tmpRadieTol05To3 = "± 0.2";
            string tmpRadieTol3To6 = "± 0.5";
            string tmpRadieTol6Plus = "± 1";

            string tmpFormat = subject.Trim().ToUpperInvariant();
            string tmpBet = tmpFormat.Replace(".", ",");
            bool tmpSlash = tmpBet.Contains("/");
            bool tmpS = tmpBet.Contains("S");
            bool tmpSS = tmpBet.Contains("SS");
            bool tmpA = tmpBet.Contains("A");
            bool tmpE = tmpBet.Contains("E");

            string[] tmpBetLista = Split(tmpBet, " /.");
            string tmpBet1 = Item(tmpBetLista, 1);
            double tmpBet2 = ParseDouble(Item(tmpBetLista, 2));
            object tmpBet3 = tmpSlash ? (object)ParseDouble(Item(tmpBetLista, 3)) : Item(tmpBetLista, 3);
            bool tmpTum = tmpSlash && ParseDouble(LotusText(tmpBet3)) < 16;
            object tmpBet4 = tmpTum ? (object)ParseDouble(Item(tmpBetLista, 4)) : Item(tmpBetLista, 4);
            object tmpBet5 = tmpTum ? (object)ParseDouble(Item(tmpBetLista, 5)) : Item(tmpBetLista, 5);

            string tmpBet2Text = Smart(tmpBet2);
            string tmpSerieText = tmpBet2Text.Length > 2 ? Left(tmpBet2Text, tmpBet2Text.Length - 2) : "";
            double tmpSerie = ParseDouble(tmpSerieText);
            double tmpTyp = ParseDouble(Right(tmpBet2Text, 2));

            bool tmpOP2 = tmpBet == "TSN 616 S" || tmpBet == "TSN 616 SA" || tmpBet == "TSN 619 S" || tmpBet == "TSN 620 S";

            double innerdiameter = GetDouble(bm, "Innerdiameter (d)");
            double tmpd = LotusRound(innerdiameter, 0.01);
            double tmpdH = tmpd + 4;

            kv["Sumd"] = "(d) " + SmartComma(tmpd);
            kv["SumdTol"] = (tmpd < 3.1 ? "+ 0.020" : tmpd < 6.1 ? "+ 0.028" : tmpd < 10.1 ? "+ 0.035" : tmpd < 18.1 ? "+ 0.043" : tmpd < 30.1 ? "+ 0.053" : tmpd < 50.1 ? "+ 0.064" : tmpd < 80.1 ? "+ 0.076" : tmpd < 120.1 ? "+ 0.090" : tmpd < 180.1 ? "+ 0.106" : tmpd < 250.1 ? "+ 0.122" : tmpd < 315.1 ? "+ 0.137" : tmpd < 400.1 ? "+ 0.151" : tmpd < 500.1 ? "+ 0.165" : "+ 0.186") + " [2]";
            kv["SumdTolN"] = (tmpd < 3.1 ? "+ 0.006" : tmpd < 6.1 ? "+ 0.010" : tmpd < 10.1 ? "+ 0.013" : tmpd < 18.1 ? "+ 0.016" : tmpd < 30.1 ? "+ 0.020" : tmpd < 50.1 ? "+ 0.025" : tmpd < 80.1 ? "+ 0.030" : tmpd < 120.1 ? "+ 0.036" : tmpd < 180.1 ? "+ 0.043" : tmpd < 250.1 ? "+ 0.050" : tmpd < 315.1 ? "+ 0.056" : tmpd < 400.1 ? "+ 0.062" : tmpd < 500.1 ? "+ 0.068" : "+ 0.076") + " [2]";

            string tmpListaD1 = tmpSerie == 2 ? "0:0:0:0:35,5:45,5:55,5:61:66:71:76:85,5:91:0:101:106,5:110,5:118,5:0:136:0:146:0:156:0:166:0:176:0:191:0:201" : tmpSerie == 3 ? "0:0:0:0:45,5:45,5:61:61:66:71:76:85,5:91:96:101:129,5:136:0:146:156:0:0:0:0:0:0:0:0:0:0:0:0" : tmpSerie == 5 ? "0:0:0:0:30,5:35,5:45,5:50,5:55,5:61:66:71:76:0:85,5:91:96:101:129,5:136:0:146:0:156:0:166:0:176:0:191:0:201:0:0:0:0" : tmpSerie == 6 ? "0:0:0:0:35,5:45,5:50,5:61:66:71:76:85,5:91:0:101:129,5:136:0:146:156:0:0:0:0:0:0:0:0:0:0:0:0" : tmpSerie == 30 ? "1:2:3:4:5:6:7:8:9:10:11:12:13:14:15:16:17:18:19:20:21:22:23:156:0:166:0:176:0:191:0:201:0:211:0:221:0:231:0:241:0:0:0:261:0:0:0:281:0:0:0:301:0:0:0:321" : "";

            double tmpD1 = ParseDouble(ListItem(tmpListaD1, Convert.ToInt32(tmpTyp)));
            kv["SumD1"] = "(D1) " + SmartComma(tmpD1);
            kv["SumD1Tol"] = "+ 0";
            kv["SumD1TolN"] = H13NegativeTolerance(tmpD1);

            double tmpD2 = tmpD1 < 61 ? tmpD1 + 7 : tmpD1 < 129.5 ? tmpD1 + 7.5 : tmpD1 < 241 ? tmpD1 + 9 : tmpD1 + 11.5;
            kv["SumD2"] = "(D2) " + SmartComma(tmpD2);
            kv["SumD2Tol"] = "+ 0";
            kv["SumD2TolN"] = H13NegativeTolerance(tmpD2);

            double tmpB = tmpSerie == 2 || tmpSerie == 30 ? tmpTyp < 6 ? 23 : tmpTyp < 7 ? 18 : tmpTyp < 13 ? 19 : tmpTyp < 15 ? 22 : tmpTyp < 20 ? 23 : tmpTyp < 26 ? 24 : tmpTyp < 30 ? 26 : tmpTyp < 40 ? 27 : tmpTyp < 48 ? 32 : 37 : tmpSerie == 3 ? tmpTyp < 7 ? 18 : tmpTyp < 13 ? 19 : tmpTyp < 15 ? 22 : tmpTyp < 16 ? 23 : 24 : tmpSerie == 5 ? tmpTyp < 9 ? 18 : tmpTyp < 16 ? 19 : tmpTyp < 18 ? 22 : tmpTyp < 19 ? 23 : tmpTyp < 26 ? 24 : tmpTyp < 30 ? 26 : 27 : tmpSerie == 6 ? tmpTyp < 8 ? 18 : tmpTyp < 13 ? 19 : tmpTyp < 15 ? 22 : tmpTyp < 16 ? 23 : 24 : double.NaN;
            kv["SumB"] = double.IsNaN(tmpB) ? "(B) FEL SERIE" : "(B) " + SmartComma(tmpB);
            kv["SumBTol"] = "+ 0";
            kv["SumBTolN"] = double.IsNaN(tmpB) ? "FEL SERIE" : H13NegativeTolerance(tmpB);

            double tmpC = tmpSerie == 2 || tmpSerie == 30 ? tmpTyp < 6 ? 15.5 : tmpTyp < 7 ? 10 : tmpTyp < 13 ? 11 : tmpTyp < 26 ? 14 : tmpTyp < 30 ? 16 : tmpTyp < 40 ? 16.5 : tmpTyp < 48 ? 19 : 21.5 : tmpSerie == 3 ? tmpTyp < 7 ? 10 : tmpTyp < 13 ? 11 : 14 : tmpSerie == 5 ? tmpTyp < 9 ? 10 : tmpTyp < 16 ? 11 : tmpTyp < 26 ? 14 : tmpTyp < 30 ? 16 : 16.5 : tmpSerie == 6 ? tmpTyp < 8 ? 10 : tmpTyp < 13 ? 11 : 14 : double.NaN;
            kv["SumC"] = double.IsNaN(tmpC) ? "(c) FEL SERIE" : "(c) " + SmartComma(tmpC);
            kv["SumCTol"] = double.IsNaN(tmpC) ? "FEL SERIE" : Js13Tolerance(tmpC) + " [3]";
            double tmpB1 = tmpSerie == 2 || tmpSerie == 30 ? tmpTyp < 6 ? 10 : tmpTyp < 13 ? 4.5 : tmpTyp < 26 ? 5 : tmpTyp < 40 ? 5.5 : 6 : tmpSerie == 3 ? tmpTyp < 13 ? 4.5 : 5 : tmpSerie == 5 ? tmpTyp < 16 ? 4.5 : tmpTyp < 26 ? 5 : 5.5 : tmpSerie == 6 ? tmpTyp < 13 ? 4.5 : 5 : double.NaN;
            kv["SumB1"] = double.IsNaN(tmpB1) ? "(B1) FEL SERIE" : "(B1) " + SmartComma(tmpB1);
            kv["SumB1Tol"] = double.IsNaN(tmpB1) ? "FEL SERIE" : Js15Tolerance(tmpB1) + " [3]";

            double tmpB2 = 2;
            kv["SumB2"] = "(B2) " + SmartComma(tmpB2);
            kv["SumB2Tol"] = tmpB2 < 3.1 ? "± 0.300" : tmpB2 < 6.1 ? "± 0.375" : "± 0.450";

            double tmpa = tmpSerie == 2 ? tmpTyp < 20 ? 3 : 4 : tmpSerie == 3 || tmpSerie == 6 || tmpSerie == 30 ? tmpTyp < 16 ? 3 : tmpTyp < 40 ? 4 : 6 : tmpSerie == 5 ? tmpTyp < 19 ? 3 : 4 : double.NaN;
            kv["Suma"] = double.IsNaN(tmpa) ? "(a) FEL SERIE" : "(a) " + SmartComma(tmpa);
            kv["SumaTol"] = "+ 0";
            kv["SumaTolN"] = double.IsNaN(tmpa) ? "FEL SERIE" : (tmpa < 3.1 ? "- 0.140" : tmpa < 6.1 ? "- 0.180" : "- 0.220") + " [3]";

            double tmpB3 = 6;
            kv["SumB3"] = "(B3) " + SmartComma(tmpB3);
            kv["SumB3Tol"] = tmpB3 < 3.1 ? "+ 0.400" : tmpB3 < 6.1 ? "+ 0.480" : tmpB3 < 10.1 ? "+ 0.580" : "+ 0.700";
            kv["SumB3TolN"] = "+ 0";

            double tmpH = 3.3;
            kv["SumH"] = "(H) " + SmartComma(tmpH);
            kv["SumHTol"] = (tmpH < 3.1 ? "± 0.200" : tmpH < 6.1 ? "± 0.240" : "± 0.290") + " [3]";

            double tmpH1 = 2;
            kv["SumH1"] = "(H1) " + SmartComma(tmpH1);
            kv["SumH1Tol"] = (tmpH1 < 3.1 ? "± 0.300" : tmpH1 < 6.1 ? "± 0.375" : "± 0.450") + " [3]";

            double tmpBTolValue = H13NegativeToleranceValue(tmpB);
            double tmpaTolValue = tmpa < 3.1 ? 0.140 : tmpa < 6.1 ? 0.180 : 0.220;
            double tmpBTolNMitt = tmpB - (tmpBTolValue / 2);
            double tmpaTolNMitt = tmpa - (tmpaTolValue / 2);
            double tmpL = LotusRound(tmpBTolNMitt - tmpC - tmpaTolNMitt, 0.01);
            double tmpK = LotusRound(tmpBTolNMitt - tmpB1, 0.01);
            kv["SumL"] = tmpOP2 ? "(" + SmartComma(tmpL) + " ± 0.050)" : "";
            kv["SumK"] = tmpOP2 ? "(" + SmartComma(tmpK) + " ± 0.100)" : "";

            kv["SumStämpel"] = tmpBet;
            kv["SumR"] = "R 1 ± 0.3";
            kv["SumF"] = "1x45°";

            string tmpRit = !tmpSlash ? tmpSerie == 2 ? "TSN 2 S" : tmpSerie == 3 ? "TSN 3 S" : tmpSerie == 5 ? "TSN 5 S" : tmpSerie == 6 ? "TSN 6 S" : "TSN 30 S" : tmpSerie == 30 ? "TSN 30 S" : "Senaste utg. Windchill";
            kv["SumRit"] = tmpRit;

            string tmpMaskinVal = tmpOP2 ? "Hjälpmått OP1 inom parantes" : "";
            string tmpMaskinValS1 = maskinVal == "Nakamura" ? "Nakamura" : maskinVal == "LT-3000EX" ? "LT-3000EX" : "";
            kv["SumMaskinValS1"] = "Maskin: " + tmpMaskinValS1 + " - " + tmpMaskinVal;

            bool machineEnabled = maskinVal == "Nakamura" || maskinVal == "LT-3000EX";

            kv["SumF1_1"] = machineEnabled ? "1/1" : "";
            kv["SumF1_2"] = machineEnabled ? "1/5" : "";
            kv["SumF1_3"] = machineEnabled ? "1/5" : "";
            kv["SumF1_4"] = machineEnabled ? "1/5" : "";
            kv["SumF1_5"] = machineEnabled ? "1/5" : "";
            kv["SumF1_6"] = machineEnabled ? "1/5" : "";
            kv["SumF1_7"] = machineEnabled ? "1/5" : "";
            kv["SumF1_8"] = machineEnabled ? "Inst." : "";
            kv["SumF1_9"] = machineEnabled ? "Inst." : "";
            kv["SumF1_0"] = machineEnabled ? "Inst." : "";
            kv["SumF1_11"] = machineEnabled ? "Inst." : "";
            kv["SumF1_12"] = machineEnabled ? "Inst." : "";
            kv["SumF1_13"] = machineEnabled ? "1/tim" : "";

            kv["SumD1_1"] = machineEnabled ? "UD-Apparat och Tebotolk om det finns någon" : "";
            kv["SumD1_2"] = machineEnabled ? "Digitalt Skjutmått" : "";
            kv["SumD1_3"] = machineEnabled ? "Digitalt Skjutmått" : "";
            kv["SumD1_4"] = machineEnabled ? "Digitalt Skjutmått" : "";
            kv["SumD1_5"] = machineEnabled ? "Digitalt djup/hakmått" : "";
            kv["SumD1_6"] = machineEnabled ? "Digitalt djup/hakmått" : "";
            kv["SumD1_7"] = machineEnabled ? "Mall" : "";
            kv["SumD1_8"] = machineEnabled ? "Digitalt djup/hakmått" : "";
            kv["SumD1_9"] = machineEnabled ? "Digitalt djup/hakmått" : "";
            kv["SumD1_0"] = machineEnabled ? "Digitalt djup/hakmått" : "";
            kv["SumD1_11"] = machineEnabled ? "Digitalt Skjutmått" : "";
            kv["SumD1_12"] = machineEnabled ? "Radielyra" : "";
            kv["SumD1_13"] = machineEnabled ? "Ytjämnhetsmätare" : "";

            kv["SumAF1_1"] = machineEnabled ? "Inst. tillhörande ring/klove. + Fas 1mm" : "";
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = machineEnabled && tmpOP2 ? "OP 1 = +2mm" : "";
            kv["SumAF1_6"] = "";
            kv["SumAF1_7"] = machineEnabled ? "Mall = 3,3 mm JS15" : "";
            kv["SumAF1_8"] = "";
            kv["SumAF1_9"] = "";
            kv["SumAF1_0"] = "";
            kv["SumAF1_11"] = machineEnabled ? "Mäts: (d+4mm) " + SmartComma(tmpdH) + " ± 0,600" : "";
            kv["SumAF1_12"] = "";
            kv["SumAF1_13"] = machineEnabled ? "Bearbetas Ra 12.5 runt om" : "";

            kv["SumTextS1"] = "Vid inställning: Trepunktsmätning, max variation 0.1 mm, mät innerdiameter d i två snitt" + LB + "Okulär kontroll 100% - Märkning, repor, frifläckar e.t.c" + LB + "Max radie i botten på spår 1.0 mm, skarpa kanter avgradas.";

            kv["VaLTyp"] = tmpTyp > 56 ? "Mallen täcker endast storlek 5-56" + LB + "Kontakta Admin 74181" : "";
            kv["VaLSerie"] = tmpSerie == 2 || tmpSerie == 3 || tmpSerie == 5 || tmpSerie == 6 || tmpSerie == 30 ? "" : "Mallen täcker endast serie 200, 300, 500, 600, 30" + LB + "Kontakta Admin 74181";

            kv["TmpMTxtUtf1"] = "Passbitklove Utf. 1 med diametern: " + SmartComma(tmpd);
            kv["VaLFärdig"] = "";
            kv["VaLInfo"] = "";

            return kv;
        }
        private static string GetString(IEnumerable<Bookmark> bookmarks, string name)
        {
            if (bookmarks == null || string.IsNullOrEmpty(name)) return "";
            foreach (Bookmark bookmark in bookmarks) if (bookmark != null && string.Equals(bookmark.BookmarkName, name, StringComparison.OrdinalIgnoreCase)) return bookmark.BookmarkValue?.Trim() ?? "";
            return "";
        }

        private static double GetDouble(IEnumerable<Bookmark> bookmarks, string name) => ParseDouble(GetString(bookmarks, name));

        private static string[] Split(string value, string separators) => string.IsNullOrEmpty(value) ? Array.Empty<string>() : value.Split(separators.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

        private static string Item(string[] values, int lotusPosition) => values == null || lotusPosition < 1 || lotusPosition > values.Length ? "" : values[lotusPosition - 1];

        private static string ListItem(string list, int lotusPosition)
        {
            if (string.IsNullOrWhiteSpace(list)) return "";
            string[] values = list.Split(':');
            if (lotusPosition == 0) lotusPosition = 1;
            int index = lotusPosition > 0 ? lotusPosition - 1 : values.Length + lotusPosition;
            return index >= 0 && index < values.Length ? values[index] : "";
        }

        private static double ParseDouble(string value) => string.IsNullOrWhiteSpace(value) ? 0 : double.TryParse(value.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double result) ? result : 0;

        private static string LotusText(object value)
        {
            if (value == null) return "";
            if (value is double d) return SmartComma(d);
            if (value is float f) return SmartComma(f);
            if (value is decimal m) return SmartComma((double)m);
            return value.ToString() ?? "";
        }

        private static string Smart(double value) => Math.Abs(value % 1) < 0.0000001 ? value.ToString("F0", CultureInfo.InvariantCulture) : value.ToString("0.######", CultureInfo.InvariantCulture);

        private static string SmartComma(double value) => Math.Abs(value % 1) < 0.0000001 ? value.ToString("F0", CultureInfo.InvariantCulture) : value.ToString("0.######", CultureInfo.InvariantCulture).Replace(".", ",");

        private static string F3(double value) => value.ToString("F3", CultureInfo.InvariantCulture);

        private static string Left(string value, int length) => string.IsNullOrEmpty(value) || length <= 0 ? "" : value.Substring(0, Math.Min(length, value.Length));

        private static string Right(string value, int length) => string.IsNullOrEmpty(value) || length <= 0 ? "" : value.Length <= length ? value : value.Substring(value.Length - length);

        private static double H13NegativeToleranceValue(double value) => value < 3.1 ? 0.140 : value < 6.1 ? 0.180 : value < 10.1 ? 0.220 : value < 18.1 ? 0.270 : value < 30.1 ? 0.330 : value < 50.1 ? 0.390 : value < 80.1 ? 0.460 : value < 120.1 ? 0.540 : value < 180.1 ? 0.630 : value < 250.1 ? 0.720 : value < 315.1 ? 0.810 : value < 400.1 ? 0.890 : value < 500.1 ? 0.970 : 1.100;

        private static string H13NegativeTolerance(double value) => "- " + F3(H13NegativeToleranceValue(value));

        private static string Js13Tolerance(double value) => value < 3.1 ? "± 0.070" : value < 6.1 ? "± 0.090" : value < 10.1 ? "± 0.110" : value < 18.1 ? "± 0.135" : value < 30.1 ? "± 0.165" : value < 50.1 ? "± 0.195" : value < 80.1 ? "± 0.230" : value < 120.1 ? "± 0.270" : value < 180.1 ? "± 0.315" : value < 250.1 ? "± 0.360" : value < 315.1 ? "± 0.405" : value < 400.1 ? "± 0.445" : value < 500.1 ? "± 0.485" : "± 0.550";

        private static string Js15Tolerance(double value) => value < 3.1 ? "± 0.200" : value < 6.1 ? "± 0.240" : value < 10.1 ? "± 0.290" : "± 0.350";

        private static double LotusRound(double value, double factor)
        {
            if (factor == 0) return value;
            return Math.Floor((value / factor) + 0.5) * factor;
        }
        private static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published)) return "";
            if (!DateTime.TryParseExact(published, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime publishDate) && !DateTime.TryParse(published, CultureInfo.InvariantCulture, DateTimeStyles.None, out publishDate) && !DateTime.TryParse(published, out publishDate)) return "";
            DateTime validTo = publishDate.AddDays(14);
            if (DateTime.Today > validTo.Date) return "";
            return "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)" + LB + LB + LB + LB + "Popupruta aktiv till " + validTo.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
    }
}