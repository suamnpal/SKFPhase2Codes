using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Avdragshylsor_8_40 : ITemplateCalculations
    {
        private const string LB = "<<LineBreak>>";

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();
            var bm = req?.Bookmarks;
            string subject = req?.ProductDesignation ?? "";
            string maskinVal = req?.MachineNumber ?? "";
            kv["VaLPopUp"] = ComputePopup(req?.Published);

            string tmpFormat = subject.Trim().ToUpperInvariant();
            string tmpBet = tmpFormat.Replace(".", ",");
            bool tmpSlash = tmpBet.Contains("/");
            bool tmpG = tmpBet.Contains("G");
            bool tmpA51 = tmpBet.Contains("A51");

            string[] parts = Split(tmpBet, " /.-");
            string tmpBet1a = Item(parts, 1);
            string tmpBet2a = Item(parts, 2);
            string tmpBet3a = Item(parts, 3);
            string tmpBet4a = Item(parts, 4);
            string tmpBet5a = Item(parts, 5);
            string tmpBet6a = Item(parts, 6);

            string tmpBet1b = string.IsNullOrEmpty(tmpBet1a) ? "0" : tmpBet1a;
            string tmpBet2b = string.IsNullOrEmpty(tmpBet2a) ? "0" : tmpBet2a;
            string tmpBet3b = string.IsNullOrEmpty(tmpBet3a) ? "0" : tmpBet3a;
            string tmpBet4b = string.IsNullOrEmpty(tmpBet4a) ? "0" : tmpBet4a;
            string tmpBet5b = string.IsNullOrEmpty(tmpBet5a) ? "0" : tmpBet5a;
            string tmpBet6b = string.IsNullOrEmpty(tmpBet6a) ? "0" : tmpBet6a;

            object tmpBet1 = IsNumeric(tmpBet1b) && tmpBet1b != "0" ? ParseDouble(tmpBet1a) : (object)tmpBet1a;
            object tmpBet2 = IsNumeric(tmpBet2b) && tmpBet2b != "0" ? ParseDouble(tmpBet2a) : (object)tmpBet2a;
            object tmpBet3 = IsNumeric(tmpBet3b) && tmpBet3b != "0" ? ParseDouble(tmpBet3a) : (object)tmpBet3a;
            object tmpBet4 = IsNumeric(tmpBet4b) && tmpBet4b != "0" ? ParseDouble(tmpBet4a) : (object)tmpBet4a;
            object tmpBet5 = IsNumeric(tmpBet5b) && tmpBet5b != "0" ? ParseDouble(tmpBet5a) : (object)tmpBet5a;
            object tmpBet6 = IsNumeric(tmpBet6b) && tmpBet6b != "0" ? ParseDouble(tmpBet6a) : (object)tmpBet6a;

            string tmpBet1Text = LotusText(tmpBet1);
            string tmpBet2Text = LotusText(tmpBet2);
            string tmpBet3Text = LotusText(tmpBet3);
            string tmpBet4Text = LotusText(tmpBet4);
            string tmpBet5Text = LotusText(tmpBet5);
            string tmpBet6Text = LotusText(tmpBet6);

            int tmpArtLista = MemberText(tmpBet1Text, new[] { "AH", "AHX", "MS", "T" });
            int tmpCountB = tmpBet.Length;
            int tmpCountB2 = tmpBet2Text.Length;
            string tmpSerie = tmpSlash && (tmpCountB2 == 3 || tmpCountB2 == 2) ? tmpBet2Text : tmpCountB2 > 4 ? Left(tmpBet2Text, 3) : tmpCountB2 == 3 ? Left(tmpBet2Text, 1) : Left(tmpBet2Text, 2);
            string tmpTypText = Right(tmpBet2Text, 2);
            double tmpTyp = ParseDouble(tmpTypText);

            double tmpd = GetDouble(bm, "Konringsdiameter");
            double tmpb = GetDouble(bm, "Längd till gänga");
            double slappning = GetDouble(bm, "Släppning");
            double yDia = GetDouble(bm, "YDia");
            string produktRitning = GetString(bm, "Produktritning");
            double slappningsDia = GetDouble(bm, "SläppningsDia");
            double inreFas = GetDouble(bm, "Inre fas");
            double kona = GetDouble(bm, "Kona");
            double iDia = GetDouble(bm, "IDia");
            double langd = GetDouble(bm, "Längd");
            string kil = GetString(bm, "Kil");
            string kilinst = GetString(bm, "Kilinst");

            double sumML = LotusRound(tmpb - slappning - 7, 1) > 120 ? 120 : tmpd < 50 ? 16 : LotusRound(tmpb - slappning - 10, 1);
            kv["SumML"] = SmartComma(sumML);

            double tmpVT = tmpd > 181 ? 0.15 : tmpd > 151 ? 0.18 : tmpd > 121 ? 0.30 : tmpd > 81 ? 0.45 : tmpd > 51 ? 0.50 : 0.60;

            double tmpP = yDia < 25 ? 1.0 : yDia < 55 ? 1.5 : yDia < 155 ? 2.0 : yDia < 205 ? 3.0 : yDia < 305 ? 4.0 : 5.0;
            string tmpPText = F1Comma(tmpP);
            kv["SumP"] = "(P) " + tmpPText;

            double tmpKonavv = sumML * tmpVT / 1000;
            kv["SumKonavv"] = "max " + F3Comma(tmpKonavv) + " [2F]";

            bool metricThread = tmpP == 1 || tmpP == 1.5 || tmpP == 2 || tmpP == 3;
            kv["SumGängmall"] = metricThread ? "M" + tmpPText : "Tr" + tmpPText;
            kv["SumGängring"] = metricThread ? "M" + SmartComma(yDia) + "x" + tmpPText : "";
            kv["SumGängtolk"] = metricThread ? "gängtolk M" + SmartComma(yDia) + "x" + tmpPText : "klove";
            kv["SumRullar"] = metricThread ? "M" + tmpPText : "Tr" + tmpPText;
            kv["SumGänga"] = (metricThread ? "M" : "Tr") + SmartComma(yDia) + "x" + tmpPText;

            double tmph = slappning;
            kv["Sumh"] = "(h) " + SmartComma(tmph);

            bool tmpd4a = produktRitning.Contains("21");
            bool tmpd4b = produktRitning.Contains("22");
            bool tmpd4c = produktRitning.Contains("23");
            bool tmpd4d = produktRitning.Contains("7437359");
            bool tmpd4e = produktRitning.Contains("7437358");
            double tmpd4sum = BoolNumber(tmpd4a) + BoolNumber(tmpd4b) + BoolNumber(tmpd4c) + BoolNumber(tmpd4d) + BoolNumber(tmpd4e);
            int tmpAnt = tmpd4sum > 0.5 ? 5 : 7;
            string tmpd4TolNa = Left(produktRitning, tmpAnt);
            double tmpd4 = slappningsDia;
            kv["Sumd4"] = "(d4) " + SmartComma(tmpd4);

            string[] d4ZeroDrawings = { "7437362", "7437378", "7437364", "7437366", "7437380", "7437376", "7437360", "7437361", "7437987", "7437372", "7437370", "7437697" };
            string[] d4Tol300Drawings = { "7437362", "7437378", "7437364", "7437366", "7437380", "7437376" };
            string[] d4Tol200Drawings = { "7437360", "7437361", "7437987" };
            string[] d4Tol250Drawings = { "7437372", "7437370", "7437697" };

            string tmpd4Tol = produktRitning == "ASW-0004" ? F3(0) : tmpd4sum > 0.5 ? F3(D4GeneralTolerance(tmpd4)) : ContainsExact(d4ZeroDrawings, tmpd4TolNa) ? F3(0) : "";
            string tmpd4TolN = produktRitning == "ASW-0004" ? F3(0.5) : tmpd4sum > 0.5 ? F3(D4GeneralTolerance(tmpd4)) : ContainsExact(d4Tol300Drawings, tmpd4TolNa) ? F3(0.300) : ContainsExact(d4Tol200Drawings, tmpd4TolNa) ? F3(0.200) : ContainsExact(d4Tol250Drawings, tmpd4TolNa) ? F3(0.250) : "";

            kv["Sumd4Tol"] = tmpd4Tol != "" ? "+ " + tmpd4Tol : "";
            kv["Sumd4TolN"] = tmpd4TolN != "" && ParseDouble(tmpd4TolN) != 0 ? "- " + tmpd4TolN : "";

            kv["SumFasÖ"] = tmpP == 4 ? "2,7x45º" : tmpP == 3 ? "2,4x45º" : tmpP == 2 ? "1,7x45º" : "1,3x45º";
            kv["SumFasN"] = inreFas == 0 ? kv["SumFasÖ"] : SmartComma(inreFas) + "x45º";
            kv["SumR1"] = "R" + (yDia < 71 ? "0.5" : yDia < 136 ? "1" : "1.5");
            kv["SumR2"] = kv["SumR1"];

            double tmpE1_2Tol = kona == 12 ? tmpd > 251 ? 0.081 : tmpd > 181 ? 0.072 : tmpd > 121 ? 0.063 : tmpd > 81 ? 0.054 : tmpd > 51 ? 0.046 : tmpd > 31 ? 0.039 : 0.033 : kona == 30 ? tmpd > 251 ? 0.052 : tmpd > 181 ? 0.046 : tmpd > 121 ? 0.040 : 0.035 : double.NaN;
            kv["SumE1_2Tol"] = "+ " + (double.IsNaN(tmpE1_2Tol) ? "Fel Kona" : F3(tmpE1_2Tol)) + " [3F]";
            kv["SumE1_2TolN"] = "- 0 [2F]";

            double tmpVar = tmpd > 251 ? 0.025 : tmpd > 181 ? 0.020 : tmpd > 121 ? 0.015 : tmpd > 51 ? 0.010 : 0.008;
            kv["SumVar"] = F3(tmpVar) + " [2F]";
            double tmpd1 = iDia;
            kv["Sumd1"] = "(d1) " + SmartComma(tmpd1);
            kv["Sumd1Tol"] = (iDia > 316 ? "± 0.070" : iDia > 251 ? "± 0.065" : iDia > 181 ? "± 0.057" : iDia > 121 ? "± 0.050" : iDia > 81 ? "± 0.043" : iDia > 51 ? "± 0.037" : iDia > 31 ? "± 0.031" : "± 0.026") + " [3F]";

            double tmpd1efTol = tmpTyp > 39 ? 0.185 : tmpTyp > 25 ? 0.160 : tmpTyp > 17 ? 0.140 : tmpTyp > 11 ? 0.074 : 0.062;
            double tmpd1efTolN = tmpTyp > 39 ? 0.290 : tmpTyp > 25 ? 0.250 : tmpTyp > 17 ? 0.220 : tmpTyp > 11 ? 0.120 : 0.100;
            kv["Sumd1efTol"] = "+ " + F3(tmpd1efTol) + " [3F]";
            kv["Sumd1efTolN"] = "- " + F3(tmpd1efTolN) + " [3F]";

            kv["Sumd2"] = "(d2) " + SmartComma(yDia);
            kv["Sumd2Tol"] = tmpP == 5 ? "- 0 " : tmpP == 4 ? "- 0" : tmpP == 3 ? "- 0.048" : tmpP == 2 ? "- 0.038" : tmpP == 1.5 ? "- 0.032" : "- 0.026";
            kv["Sumd2TolN"] = tmpP == 5 ? "- 0.335" : tmpP == 4 ? "- 0.300" : tmpP == 3 ? "- 0.423" : tmpP == 2 ? "- 0.318" : tmpP == 1.5 ? "- 0.268" : "- 0.206";

            double tmpdm = tmpP == 1 ? yDia - 0.650 : tmpP == 1.5 ? yDia - 0.974 : tmpP == 2 ? yDia - 1.299 : tmpP == 3 ? yDia - 1.949 : tmpP == 4 ? yDia - 2.000 : tmpP == 5 ? yDia - 2.500 : yDia - 3.000;
            double tmpdmTol = tmpP == 5 ? 0.212 : tmpP == 4 ? 0.190 : tmpP == 3 ? 0.048 : tmpP == 2 ? 0.038 : tmpP == 1.5 ? 0.032 : 0.026;
            double tmpdmTolN = tmpP == 5 ? 0.710 : tmpP == 4 ? 0.630 : tmpP == 3 ? yDia > 180 ? 0.363 : 0.328 : tmpP == 2 ? yDia > 90 ? 0.274 : 0.262 : tmpP == 1.5 ? yDia > 45 ? 0.232 : 0.222 : 0.176;
            kv["Sumdm"] = "(dm) " + SmartComma(tmpdm);
            kv["SumdmTol"] = "- " + F3(tmpdmTol) + " [3F]";
            kv["SumdmTolN"] = "- " + F3(tmpdmTolN) + " [3F]";

            string tmpRd = Right(kv["Sumd1Tol"], 10);
            kv["SumRd"] = Right(kv["Sumd1Tol"], 10);
            kv["SumKs"] = Smart(tmpd > 251 ? 70 : tmpd > 121 ? 60 : tmpd > 51 ? 50 : 40);

            kv["SumRa"] = "2.5 [2F]";
            kv["SumRa1"] = "2.5 [2F]";

            double tmpL = langd;
            double tmpLTolN = langd > 400 ? 0.970 : langd > 315 ? 0.890 : langd > 250 ? 0.810 : langd > 180 ? 0.720 : langd > 120 ? 0.630 : langd > 80 ? 0.540 : langd > 50 ? 0.460 : langd > 30 ? 0.390 : langd > 18 ? 0.330 : langd > 10 ? 0.270 : 0.220;
            kv["SumL"] = "(L) " + SmartComma(tmpL);
            kv["SumLTol"] = "+ 0 [3F]";
            kv["SumLTolN"] = "- " + F3(tmpLTolN) + " [3F]";

            double tmpbTol = tmpb > 400 ? 1.250 : tmpb > 315 ? 1.150 : tmpb > 250 ? 1.050 : tmpb > 180 ? 0.925 : tmpb > 120 ? 0.800 : tmpb > 80 ? 0.700 : tmpb > 50 ? 0.600 : tmpb > 30 ? 0.500 : tmpb > 18 ? 0.420 : tmpb > 10 ? 0.350 : 0.290;
            kv["Sumb"] = "(b) " + SmartComma(tmpb);
            kv["SumbTol"] = "± " + F3(tmpbTol) + " [3F]";

            double tmpC = maskinVal == "Dubbelparet" ? 4 : 2;
            kv["SumC"] = "(C) " + Smart(tmpC);

            double tmpAxK = tmpd > 251 ? 0.070 : tmpd > 121 ? 0.060 : tmpd > 51 ? 0.050 : 0.040;
            kv["SumAxK"] = F3(tmpAxK) + " [2F]";

            double tmpSA = (tmpd < 101 ? 8 : tmpd < 281 ? 10 : tmpd < 481 ? 12 : tmpd < 601 ? 14 : tmpd < 901 ? 16 : 20) / 1000.0;
            double tmpSB = (tmpd < 101 ? 12 : tmpd < 281 ? 15 : tmpd < 481 ? 18 : tmpd < 601 ? 21 : tmpd < 901 ? 24 : 30) / 1000.0;
            kv["SumSA"] = "tol.max: " + F3(tmpSA);
            kv["SumSB"] = "tol.max: " + F3(tmpSB);

            kv["SumKona"] = "Kona 1:" + SmartComma(kona);

            kv["SumPRit"] = Left(produktRitning, 7) + " :senaste utg.";
            kv["SumTolRit"] = "1432011:6";
            kv["SumYtaRit"] = "7437357:6";
            kv["SumGängRit"] = "239473:1, 7430182:A";

            int tmpCountKil = kil.Length;
            string tmpKil = kil == "1" || kil == "2" ? "1 el 2" : kil;
            string tmpKona30 = tmpd < 160 ? "7426071-2 el 7450167-6" : "1509952-" + tmpKil + " el 7450167-7";
            string sumKilapp = tmpCountKil > 3 ? kil : kona == 30 ? tmpKona30 : tmpd < 160 ? "7450167, 1509952, 7454645-" + tmpKil : "7426071, 7427362, 1509952-" + tmpKil;
            kv["SumKilapp"] = sumKilapp;
            kv["Sumkil"] = sumKilapp;

            string tmpKonapp = "ID: 2766, 3359";
            kv["SumPass"] = "";

            kv["SumTxt1"] = "Kontrolleras enlingt styrplan och ALLA mått vid inställning & skiftstart. Obs! kontrollera innerdiameter från båda sidor.";
            kv["SumKlEgenskaper"] = @"PRODUCTION NUTS & SLEEVES & HOUSINGS\ALLMÄN\KLASSADE EGENSKAPER\Klassade egenskaper Avdragshylsor";

            bool isDubbelparet = maskinVal == "Dubbelparet";
            bool isCell1 = maskinVal == "Cell 1";
            bool isAH28 = maskinVal == "AH<28";
            bool machineEnabled = isDubbelparet || isCell1 || isAH28;
            string tmpMaskinVal = maskinVal == "Dubbelparet" ? "" : maskinVal == "Cell 1" || maskinVal == "AH<28" ? "" : "";
            kv["SumMaskinVal"] = "Maskin: " + maskinVal + tmpMaskinVal;

            kv["SumF_d1"] = machineEnabled ? "1/10" : "";
            kv["SumF_d2"] = machineEnabled ? "1/10" : "";
            kv["SumF_dm"] = machineEnabled ? "1/10" : "";
            kv["SumF_Ra"] = machineEnabled ? "1/tim" : "";
            kv["SumF_E1"] = machineEnabled ? "1/10" : "";
            kv["SumF_A"] = machineEnabled ? "1/Skift" : "";
            kv["SumF_B"] = machineEnabled ? "1/Skift" : "";
            kv["SumF_Rd"] = machineEnabled ? "1/10" : "";
            kv["SumF_Ö"] = machineEnabled ? "1/tim" : "";

            kv["SumD_d1"] = machineEnabled ? "UD-Apparat" : "";
            kv["SumD_d2"] = machineEnabled ? "Skjutmått" : "";
            kv["SumD_dm"] = machineEnabled ? "UD-Apparat" : "";
            kv["SumD_Ra"] = machineEnabled ? "Ytjämnhetsmätare" : "";
            kv["SumD_E1"] = machineEnabled ? "Konapp. " + tmpKonapp + LB + "Konapparat inst. med kil " + sumKilapp : "";
            kv["SumD_A"] = machineEnabled ? "Egglinjal" : "";
            kv["SumD_B"] = machineEnabled ? "Egglinjal" : "";
            kv["SumD_Rd"] = machineEnabled ? "UD-Apparat" : "";
            kv["SumD_Ö"] = machineEnabled ? "Skjutmått" : "";

            kv["SumAF_d1"] = machineEnabled ? "Inst. med inställningshylsa, ring eller klove" : "";
            kv["SumAF_d2"] = "";
            kv["SumAF_dm"] = machineEnabled ? "rullar: " + kv["SumRullar"] + ", inställd med " + kv["SumGängtolk"] : "";
            kv["SumAF_Ra"] = "";
            kv["SumAF_Kavv"] = machineEnabled ? "ID: 3040 fungerar inte med korta hylsor <40" : "";
            kv["SumAF_E1_av"] = "";
            kv["SumAF_E1"] = "";
            kv["SumAF_A"] = machineEnabled ? kv["SumSA"] : "";
            kv["SumAF_B"] = machineEnabled ? kv["SumSB"] : "";
            kv["SumAF_Rd"] = machineEnabled ? "TOL:" : "";
            kv["SumAF_Ö"] = "";
            kv["Kilinst"] = kilinst;

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

        private static int MemberText(string value, string[] values)
        {
            if (values == null) return 0;
            for (int i = 0; i < values.Length; i++) if (string.Equals(value ?? "", values[i], StringComparison.Ordinal)) return i + 1;
            return 0;
        }

        private static bool IsNumeric(string value) => !string.IsNullOrWhiteSpace(value) && double.TryParse(value.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out _);

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

        private static string F1Comma(double value) => value.ToString("0.0", CultureInfo.InvariantCulture).Replace(".", ",");

        private static string F3(double value) => value.ToString("F3", CultureInfo.InvariantCulture);

        private static string F3Comma(double value) => decimal.Round((decimal)value, 3, MidpointRounding.AwayFromZero).ToString("F3", CultureInfo.InvariantCulture).Replace(".", ",");
        private static string Left(string value, int length) => string.IsNullOrEmpty(value) || length <= 0 ? "" : value.Substring(0, Math.Min(length, value.Length));

        private static string Right(string value, int length) => string.IsNullOrEmpty(value) || length <= 0 ? "" : value.Length <= length ? value : value.Substring(value.Length - length);

        private static bool ContainsExact(string[] values, string value)
        {
            if (values == null) return false;
            for (int i = 0; i < values.Length; i++) if (string.Equals(values[i], value ?? "", StringComparison.Ordinal)) return true;
            return false;
        }

        private static double BoolNumber(bool value) => value ? 1 : 0;

        private static double D4GeneralTolerance(double value) => value < 6.01 ? 0.1 : value < 30.01 ? 0.2 : value < 120.01 ? 0.3 : value < 400.01 ? 0.5 : value < 1000.01 ? 0.8 : value < 2000.01 ? 1.2 : 2.0;

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