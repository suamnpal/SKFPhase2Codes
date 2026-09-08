using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Klamhylsor_14_22 : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();
            string subject = req?.ProductDesignation ?? "";
            string machine = req?.MachineNumber ?? "";
            string tmpBet = subject.ToUpperInvariant().Trim().Replace(".", ",");
            string[] tokens = tmpBet.Split(new[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string bet1 = tokens.Length > 0 ? tokens[0] : "";
            string bet2 = tokens.Length > 1 ? tokens[1] : "";
            string bet3 = tokens.Length > 2 ? tokens[2] : "";
            string bet4 = tokens.Length > 3 ? tokens[3] : "";
            string bet5 = tokens.Length > 4 ? tokens[4] : "";
            string art = EqualsAny(bet1, "H", "HA", "HE", "HS", "SNW") ? bet1 : "";
            string serie = bet2.Length > 4 ? Left(bet2, 3) : bet2.Length == 3 ? Left(bet2, 1) : Left(bet2, 2);
            double typ = ToDouble(Right(bet2, 2));
            bool isMetric = EqualsAny(art, "H", "HA", "HE", "HS");
            bool isSnw = string.Equals(art, "SNW", StringComparison.OrdinalIgnoreCase);
            bool isE = EqualsAny(bet3, "E") || EqualsAny(bet4, "E") || EqualsAny(bet5, "E") || ContainsAny(tmpBet, "E/V21");
            bool isEV21 = ContainsAny(tmpBet, "E/V21");// || EqualsAny(bet3, "V21") || EqualsAny(bet4, "V21") || EqualsAny(bet5, "V21");
            double yDia = GetBookmarkDouble(req, "YDia");
            double d = RoundTo(yDia == 0 ? (typ / 2.0) * 10.0 : yDia, 0.01);
            double b = RoundTo(GetBookmarkDouble(req, "Gänglängd"), 0.01);
            double d1 = RoundTo(GetBookmarkDouble(req, "IDia"), 0.01);
            double length = RoundTo(GetBookmarkDouble(req, "Längd"), 0.01);
            double passbit = GetBookmarkDouble(req, "Passbit");
            string kilnr = GetBookmarkValue(req, "Kilnr");
            string kilinstallning = GetBookmarkValue(req, "Kilinställning");
            double radieSnw = GetBookmarkDouble(req, "RadieSNW");
            double fasSnw = GetBookmarkDouble(req, "FasSNW");
            double gvlSnw = GetBookmarkDouble(req, "GängsvarvlängdSNW");
            bool machineValid = EqualsAny(machine, "Cell 1", "Cell 2", "LVT-300");

            kv["VaLPopUp"] = ComputePopup(req?.Published);

            double pitch = d < 25 ? 1.0 : d < 55 ? 1.5 : d < 155 ? 2.0 : d < 205 ? 3.0 : d < 305 ? 4.0 : 5.0;
            string tpi = d < 70 ? "18" : d < 150 ? "12" : d < 220 ? "8" : "6";
            double inchDesignation = d < 100 ? d / 25.4 + 0.001 : d < 150 ? d / 25.4 + 0.002 : d < 260 ? d / 25.4 + 0.003 : d / 25.4 + 0.004;
            string threadDesignation = isMetric ? "M" + Fmt(d) : isSnw ? Fmt3(inchDesignation) : "";
            string pitchText = isMetric ? FmtOneComma(pitch) : tpi + "UN";
            kv["SumGängbet"] = threadDesignation;
            kv["SumStigning"] = pitchText;
            kv["SumGxS"] = threadDesignation + "x" + pitchText;
            kv["SumP"] = "(P) " + (isMetric ? FmtOneComma(pitch) : tpi);

            double kona = 12;
            kv["SumKona"] = "Kona 1:" + Fmt(kona);
            double deduction = d < 155 ? 8 : d < 205 ? 9 : 0;
            double ml = Math.Min(100, Math.Round(length - b - deduction, 1, MidpointRounding.AwayFromZero));
            double vt = isMetric ? (d > 180 ? 0.15 : d > 150 ? 0.18 : d > 120 ? 0.30 : d > 80 ? 0.45 : d > 50 ? 0.50 : 0.60) : isSnw ? (d > 205 ? 0.15 : d > 185 ? 0.25 : d > 125 ? 0.30 : d > 85 ? 0.45 : 0.50) : 0;

            kv["Sumd"] = "(d) " + Fmt(d);
            kv["SumdTol"] = isMetric ? (pitch == 5 ? "- 0 " : pitch == 4 ? "- 0" : pitch == 3 ? "- 0.048" : pitch == 2 ? "- 0.038" : pitch == 1.5 ? "- 0.032" : "- 0.026") : isSnw ? "- 0" : "Fel Typ";
            kv["SumdTolN"] = isMetric ? (pitch == 5 ? "- 0.335" : pitch == 4 ? "- 0.300" : pitch == 3 ? "- 0.423" : pitch == 2 ? "- 0.318" : pitch == 1.5 ? "- 0.268" : "- 0.206") : isSnw ? (tpi == "18" ? "- 0.208" : tpi == "12" ? "- 0.285" : tpi == "8" ? "- 0.386" : "- 0.513") : "Fel Typ";

            double wallTol = d > 250 ? 0.055 : d > 180 ? 0.050 : d > 120 ? 0.040 : d > 80 ? 0.035 : d > 50 ? 0.030 : d > 30 ? 0.025 : 0.020;
            double wallTolN = d > 250 ? 0.160 : d > 180 ? 0.140 : d > 120 ? 0.120 : d > 80 ? 0.105 : d > 50 ? 0.090 : d > 30 ? 0.075 : 0.070;
            kv["SumGodstjocklekTol"] = (isMetric ? "+ " + Fmt3(wallTol) : isSnw ? "+ 0.025" : "Fel Typ") + " [3F]";
            kv["SumGodstjocklekTolN"] = (isMetric ? "- " + Fmt3(wallTolN) : isSnw ? "- 0.075" : "Fel Typ") + " [2F]";

            double d1Tol = d1 > 250 ? 0.065 : d1 > 180 ? 0.057 : d1 > 120 ? 0.050 : d1 > 80 ? 0.043 : d1 > 50 ? 0.037 : d1 > 30 ? 0.031 : 0.026;
            kv["Sumd1"] = "(d1) " + Fmt(d1);
            kv["Sumd1Tol"] = isSnw ? "+ 0.102 [3F]" + Environment.NewLine + " - 0 [3F]" : isMetric ? "± " + Fmt3(d1Tol) : "";
            kv["SumVE"] = "Max " + (isMetric ? (d > 250 ? "0.025" : d > 180 ? "0.020" : d > 120 ? "0.015" : d > 50 ? "0.010" : "0.008") : isSnw ? "0.030" : "Fel Typ") + " [2F]";

            double dm = isMetric ? (pitch == 1 ? d - 0.650 : pitch == 1.5 ? d - 0.974 : pitch == 2 ? d - 1.299 : pitch == 3 ? d - 1.949 : pitch == 4 ? d - 2.000 : pitch == 5 ? d - 2.500 : d - 3.000) : isSnw ? (tpi == "18" ? d - 0.917 : tpi == "12" ? d - 1.374 : tpi == "8" ? d - 2.062 : d - 2.751) : 0;
            kv["Sumdm"] = "(dm) " + Fmt(dm);
            kv["SumdmTol"] = (isMetric ? (pitch == 5 ? "- 0.212" : pitch == 4 ? "- 0.190" : pitch == 3 ? "- 0.048" : pitch == 2 ? "- 0.038" : pitch == 1.5 ? "- 0.032" : "- 0.026") : isSnw ? "- 0" : "Fel Typ") + " [3F]";
            string dmTolN = isMetric ? (pitch == 5 ? "- 0.710" : pitch == 4 ? "- 0.630" : pitch == 3 ? (d > 180 ? "- 0.363" : "- 0.328") : pitch == 2 ? (d > 90 ? "- 0.274" : "- 0.262") : pitch == 1.5 ? (d > 45 ? "- 0.232" : "- 0.222") : "- 0.176") : SnwDmTolerance(tpi, d);
            kv["SumdmTolN"] = dmTolN + " [3F]";
            kv["SumGängmall"] = isMetric ? "M" + pitchText : pitchText;
            kv["SumGängring"] = kv["SumGxS"];
            kv["SumGängtolk"] = kv["SumGxS"];
            kv["SumRullar"] = isMetric ? "M" + pitchText : pitchText;

            kv["SumL"] = "(L) " + Fmt(length);
            kv["SumLTol"] = isMetric ? "+ 0 " : "+ 0.254";
            double lTolN = length > 400 ? 2.500 : length > 315 ? 2.300 : length > 250 ? 2.100 : length > 180 ? 1.850 : length > 120 ? 1.600 : length > 80 ? 1.400 : length > 50 ? 1.200 : length > 30 ? 1.000 : length > 18 ? 0.840 : length > 10 ? 0.700 : 0.580;
            kv["SumLTolN"] = (isEV21 ? "- 1.000" : isMetric ? "- " + Fmt3(lTolN) : isSnw ? "- 0.254" : "Fel Typ") + " [3F]";
            kv["Sumb"] = (isSnw ? "(min)" : "") + "(b) " + Fmt(b);
            double bTol = b > 120 ? 4.0 : b > 80 ? 3.5 : b > 50 ? 3.0 : b > 30 ? 2.5 : b > 18 ? 2.1 : b > 10 ? 1.8 : 1.5;
            kv["SumbTol"] = isMetric ? "+ " + FmtOneDot(bTol) + " [3F]" : isSnw ? "" : "Fel Typ";
            kv["SumbTolN"] = isMetric ? "- 0 [2F]" : "";
            kv["SumGVL"] = isSnw ? "Gängsvarvlängd: " + Fmt(gvlSnw) + " ± 0.25" : "";

            kv["SumR1"] = isMetric ? (d < 69 ? "R 0.5" : "R 1") : "R " + Fmt(radieSnw);
            kv["SumR2"] = isMetric ? (d < 69 ? "R 0.5" : d < 109 ? "R 1" : d < 159 ? "R 1.5" : d < 219 ? "R 2" : "R 2.5") : "R " + Fmt(radieSnw);
            double e = isMetric ? (d < 54 ? 7 : d < 79 ? 9 : d < 99 ? 11 : d < 119 ? 13 : d < 139 ? 15 : d < 159 ? 17 : d < 179 ? 19 : d < 210 ? 21 : 25) : d < 75 ? 9.52 : d < 120 ? 11.12 : d < 130 ? 14.30 : d < 160 ? 19.05 : d < 190 ? 22.22 : 25.40;
            kv["Sume"] = isE ? "Inget tungspår" : "(e) " + Fmt(e);
            kv["SumeTol"] = isE ? "" : isMetric ? "+ " + Fmt3(e > 18 ? 0.520 : e > 10 ? 0.430 : e > 6 ? 0.360 : 0.300) : "";
            kv["SumeTolN"] = isE ? "" : isMetric ? "- 0 [3F]" : isSnw ? "± 0.254" : "";

            double f = MetricF(d);
            if (isSnw) f = SnwF(d, length);
            kv["Sumf"] = isE ? "E-Hylsa" : "(f) " + Fmt(f);
            kv["SumfTol"] = isE ? "" : isMetric ? "+ " + FmtOneDot(f > 50 ? 3.0 : f > 30 ? 2.5 : f > 18 ? 2.1 : 1.8) : "+ 2.1";
            kv["SumfTolN"] = isE ? "" : "- 0 [3F]";

            double c = d1 < 61 ? 2.5 : 3.0;
            kv["Sumc"] = "(c) " + Fmt(c);
            kv["SumcTol"] = "± 0.1";
            kv["Sumd1STol"] = isSnw ? "" : "+ " + Fmt3(d > 201 ? 0.185 : d > 131 ? 0.100 : d > 91 ? 0.087 : d > 56 ? 0.074 : 0.062) + " [3F]";
            kv["Sumd1STolN"] = isSnw ? "" : "- " + Fmt3(d > 201 ? 0.290 : d > 131 ? 0.250 : d > 91 ? 0.220 : d > 56 ? 0.120 : 0.100) + " [3F]";
            kv["SumFas"] = isMetric ? (d < 24 ? "0.7x45°" : d < 69 ? "1.1x45°" : d < 159 ? "1.8x45°" : d < 219 ? "2.4x45°" : "2.7x45°") : isSnw ? Fmt(fasSnw) + "x45°" : "Fel Typ";
            kv["SumFT"] = "+ 0.25";
            kv["SumFTN"] = "- 0";
            kv["SumRd"] = "Max " + (isMetric ? Fmt3(d1Tol) : "");
            kv["SumRa"] = "Ra 2.5 [2F]";
            kv["SumRa1"] = "Ra 2.5 [2F]";
            // Lotus Notes formats the cone deviation with a decimal comma.
            kv["SumKA"] = "Max " + Fmt3Comma(ml * vt / 1000.0) + " [2F]";
            kv["SumMätlängd"] = Fmt(ml);
            kv["SumML"] = Fmt(ml);
            kv["SumSA"] = Fmt3(d < 101 ? 0.008 : d < 281 ? 0.010 : d < 481 ? 0.012 : d < 601 ? 0.014 : d < 901 ? 0.016 : 0.020);
            kv["SumSB"] = Fmt3(d < 101 ? 0.012 : d < 281 ? 0.015 : d < 481 ? 0.018 : d < 601 ? 0.021 : d < 901 ? 0.024 : 0.030);
            kv["SumKilritn"] = ml > 100 ? "1579442" : "1509952 el 7450167";

            // These are direct Word-template placeholders. Without these mappings,
            // the generated document displays the placeholder names instead of values.
            kv["Kilnr"] = kilnr;
            kv["Kilinställning"] = kilinstallning;
            // Keep aliases for templates that use the standard Sum prefix.
            kv["SumKilnr"] = kilnr;
            kv["SumKilinställning"] = kilinstallning;

            kv["SumPassbit"] = passbit == 0 ? "" : " +passbit " + Fmt(passbit) + "mm";
            kv["SumKonapp"] = ml > 100 ? "1579440-41" : "1509950";

            kv["SumMaskinValS1"] = "Maskin: " + (machineValid ? machine : "");
            string cellFrequency = EqualsAny(machine, "Cell 1", "Cell 2") ? "cell" : EqualsAny(machine, "LVT-300") ? "lvt" : "";
            kv["SumF1_1"] = cellFrequency == "" ? "" : "1/10";
            kv["SumF1_2"] = cellFrequency == "cell" ? "1/Tim" : cellFrequency == "lvt" ? "1/10" : "";
            kv["SumF1_3"] = cellFrequency == "cell" ? "1/Tim" : cellFrequency == "lvt" ? "1/10" : "";
            kv["SumF1_4"] = cellFrequency == "" ? "" : "1/10";
            kv["SumF1_5"] = cellFrequency == "" ? "" : "1/10";
            kv["SumF1_6"] = cellFrequency == "" ? "" : "1/Tim";
            kv["SumF1_7"] = cellFrequency == "" ? "" : "1/10";
            kv["SumF1_8"] = cellFrequency == "" ? "" : "1/10";
            kv["SumF1_9"] = cellFrequency == "" ? "" : "1/Skift";
            kv["SumF1_0"] = cellFrequency == "" ? "" : "1/Skift";

            kv["SumD1_1"] = machineValid ? "UD-Apparat inställd med hylsa, ring el. klove. Kontroll från båda håll" : "";
            kv["SumD1_2"] = machineValid ? "Skjutmått" : "";
            kv["SumD1_3"] = machineValid ? "Skjutmått" : "";
            kv["SumD1_4"] = machineValid ? "UD-Apparat" : "";
            kv["SumD1_5"] = machineValid ? "Gängring: " + kv["SumGängring"] : "";
            kv["SumD1_6"] = machineValid ? "Ytjämnhetsmätare" : "";
            kv["SumD1_7"] = machineValid ? "Konmätningsapparat" : "";
            kv["SumD1_8"] = machineValid ? "UD-Apparat" : "";
            kv["SumD1_9"] = machineValid ? "Egglinjal" : "";
            kv["SumD1_0"] = machineValid ? "Egglinjal" : "";

            kv["SumAF1_1"] = machineValid ? "Tolerans efter slits:" : "";
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = machineValid ? "Rullar: " + kv["SumRullar"] + ", Gängtolk: " + kv["SumGängtolk"] : "";
            kv["SumAF1_5"] = machineValid ? "Kontrolleras före & efter slits" : "";
            kv["SumAF1_6"] = "";
            kv["SumAF1_7"] = machineValid ? "Inställningskil " + kv["SumKilritn"] : "";
            kv["SumAF1_8"] = machineValid ? kv["SumRd"] : "";
            kv["SumAF1_90"] = machineValid ? "Vid misstänkt formfel kontrollera hylsan i MarSurf Contour XC20" : "";
            kv["SumText"] = "Kontrolleras enlingt styrplan och ALLA mått vid inställning & skiftstart. Obs! kontrollera innerdiameter från båda sidor.";
            kv["SumText1"] = "Okulär kontroll av Grader, frifläckar, slagmärken, repor, valkar etc. Märkning ska vara rätt och tydlig.";
            kv["SumText2"] = "Om dålig gänga upptäcks skall alla hylsor kontrolleras med gängring tills defekta hylsor sorterats bort.";

            string drawing = serie == "2" ? "7438950" : serie == "3" ? "7438951" : serie == "23" ? "7438952" : serie == "31" ? "7438954" : "";
            drawing += art == "HA" ? ", 7440342" : art == "HE" ? ", 7438961" : art == "HS" ? ", 7438962" : "";
            kv["SumRitPr"] = drawing + ":senaste utg.";
            kv["SumRitGänga"] = "Gänga: " + (isMetric ? (pitch < 3.5 ? "7430182:A, 239473" : "7430181:2, 237359:3") : "7431233");
            kv["SumRitRa"] = "Yta: 7430184:2";
            kv["SumRitTol"] = "TOL: 1432012:7";
            kv["SumKlEgenskaper"] = "PRODUCTION NUTS & SLEEVES & HOUSINGSALLMÄNKLASSADE EGENSKAPERKlassade egenskaper klämhylsor";
            return kv;
        }

        private static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published) || !DateTime.TryParse(published, out var dt)) return "";
            var until = dt.AddDays(14);
            return DateTime.Today <= until.Date ? "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)" + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + "Popupruta aktiv till " + until.ToString("yyyy-MM-dd") : "";
        }

        private static string SnwDmTolerance(string tpi, double d)
        {
            if (tpi == "18") return d > 50 ? "- 0.130" : d > 35 ? "- 0.114" : "- 0.102";
            if (tpi == "12") return d > 122 ? "- 0.210" : d > 120 ? "- 0.170" : d > 100 ? "- 0.210" : d > 85 ? "- 0.188" : d > 75 ? "- 0.150" : "- 0.137";
            if (tpi == "8") return d > 210 ? "- 0.307" : d > 200 ? "- 0.249" : d > 197 ? "- 0.290" : "- 0.231";
            return d > 305 ? "- 0.343" : d > 300 ? "- 0.264" : d > 240 ? "- 0.330" : d > 220 ? "- 0.315" : d > 210 ? "- 0.307" : d > 200 ? "- 0.249" : d > 197 ? "- 0.290" : "- 0.231";
        }

        private static double MetricF(double d)
        {
            return d < 54 ? 18.5 : d < 64 ? 19.5 : d < 69 ? 20.5 : d < 74 ? 21 : d < 79 ? 22 : d < 84 ? 24 : d < 94 ? 26 : d < 99 ? 27 : d < 109 ? 28 : d < 119 ? 29 : d < 129 ? 31 : d < 139 ? 32 : d < 149 ? 33 : d < 159 ? 35 : d < 169 ? 37 : d < 179 ? 38 : d < 189 ? 39 : d < 199 ? 41 : d < 219 ? 42 : 45;
        }

        private static double SnwF(double d, double length)
        {
            return d < 80 ? 26.5 : d < 85 ? 27.5 : d < 90 ? 29.5 : d < 100 ? 33 : d < 110 ? 34.5 : d < 120 ? (length < 75 ? 35.5 : 39) : d < 130 ? (length < 83 ? 37 : 41) : d < 140 ? 42.4 : d < 150 ? (length < 90 ? 40.5 : 46.1) : d < 160 ? 48 : d < 170 ? (length < 103 ? 42.9 : 48.8) : d < 180 ? 49.8 : d < 190 ? 51.2 : d < 200 ? (length < 121 ? 47 : 0) : 0;
        }

        private static string GetBookmarkValue(APIRequest req, string name)
        {
            if (req?.Bookmarks == null) return "";
            foreach (var bookmark in req.Bookmarks)
                if (string.Equals(bookmark.BookmarkName ?? "", name, StringComparison.OrdinalIgnoreCase))
                    return (bookmark.BookmarkValue ?? "").Trim();
            return "";
        }
        private static double GetBookmarkDouble(APIRequest req, string name)
        {
            return ToDouble(GetBookmarkValue(req, name));
        }

        private static bool EqualsAny(string value, params string[] values)
        {
            foreach (var item in values)
                if (string.Equals(value ?? "", item, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        private static bool ContainsAny(string value, params string[] values)
        {
            foreach (var item in values)
                if ((value ?? "").IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0) return true;
            return false;
        }

        private static string Left(string value, int length)
        {
            value = value ?? "";
            return value.Length <= length ? value : value.Substring(0, length);
        }

        private static string Right(string value, int length)
        {
            value = value ?? "";
            return value.Length <= length ? value : value.Substring(value.Length - length, length);
        }

        private static double ToDouble(string value)
        {
            double.TryParse((value ?? "").Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out var result);
            return result;
        }

        private static double RoundTo(double value, double step)
        {
            return Math.Round(value / step, MidpointRounding.AwayFromZero) * step;
        }

        private static string Fmt(double value)
        {
            return value.ToString("0.################", CommonFunctions.Culture);
        }

        private static string Fmt3(double value)
        {
            return value.ToString("0.000", CultureInfo.InvariantCulture);
        }
        private static string Fmt3Comma(double value)
        {
            return Fmt3(value).Replace(".", ",");
        }
        private static string NormalizeDecimalComma(string value)
        {
            return (value ?? "").Trim().Replace(".", ",");
        }

        private static string FmtOneDot(double value)
        {
            return value.ToString("0.0", CultureInfo.InvariantCulture);
        }

        private static string FmtOneComma(double value)
        {
            return value.ToString("0.0", CultureInfo.InvariantCulture).Replace(".", ",");
        }
    }
}
