using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Klamhylsor_4_13 : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();
            string subject = req?.ProductDesignation ?? "";
            string machine = req?.MachineNumber ?? "";
            string tmpBet = subject.ToUpperInvariant().Trim().Replace(".", ",");
            string[] tokens = tmpBet.Split(new[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string art = tokens.Length >= 1 ? tokens[0] : "";
            string bet2 = tokens.Length >= 2 ? tokens[1] : "";
            string bet3 = tokens.Length >= 3 ? tokens[2] : "";
            string serie = bet2.Length == 3 ? Left(bet2, 1) : Left(bet2, 2);
            string typText = Right(bet2, 2);
            double typ = ToDouble(typText);
            double tudelad = GetBookmarkDouble(req, "Tudelad");
            double kona = GetBookmarkDouble(req, "Kona");
            double dBookmark = GetBookmarkDouble(req, "GYDia (d)");
            double d = dBookmark == 0 ? (typ / 2.0) * 10.0 : dBookmark;
            double b = GetBookmarkDouble(req, "Gänglängd");
            double d1 = GetBookmarkDouble(req, "IDia (d1)");
            double d2 = GetBookmarkDouble(req, "YDia (d2)");
            double length = GetBookmarkDouble(req, "Längd");
            double aMatt = GetBookmarkDouble(req, "Amått");
            bool machineValid = EqualsAny(machine, "Okuma 2SP 4631", "Okuma 2SP 4669");
            bool isH = string.Equals(art, "H", StringComparison.OrdinalIgnoreCase);

            kv["VaLPopUp"] = ComputePopup(req?.Published);

            string ritPr = serie == "2" ? "7438950" : serie == "3" ? "7438951" : serie == "23" ? "7438952" : "";
            ritPr += art == "HA" ? ", 7438960" : art == "HE" ? ", 7438961" : art == "HS" ? ", 7438962" : "";
            kv["SumRitPr"] = ritPr + ":senaste utg.";
            kv["SumRitRa"] = "Yta: 7430184:2";
            kv["SumRitTol"] = "Toleranser: 1432012:7, 7437495:4";
            kv["SumKlEgenskaper"] = "PRODUCTION NUTS & SLEEVES & HOUSINGSALLMÄNKLASSADE EGENSKAPERKlassade egenskaper klämhylsor";

            bool bricka = ContainsAny(kv["SumRitPr"], "J425", "j425", "8960", "8961", "8962");
            string sumBricka = "";
            if (bricka || tudelad == 1)
                sumBricka = bet3 == "38" ? "Nr: 9827" : bet3 == "35" ? "Nr: 6039" : bet3 == "30" ? "Nr: 1167" : aMatt == 0 ? "" : "A-mått: " + Fmt(aMatt) + "mm.";
            kv["SumBricka"] = sumBricka;

            double ml = length - b - 6;
            double vt = d > 181 ? 0.15 : d > 150 ? 0.18 : d > 120 ? 0.30 : d > 80 ? 0.45 : d > 50 ? 0.50 : 0.60;
            kv["VaLML"] = "Konavikelse kan avvika vid stämpelyta, vid behov ändra mätlängd. Kom ihåg att ändra mätlängd i Comgage också!";
            kv["SumML"] = Math.Round(ml, 0, MidpointRounding.AwayFromZero).ToString("0", CultureInfo.InvariantCulture);

            double pitch = d < 25 ? 1 : d < 55 ? 1.5 : d < 155 ? 2 : d < 205 ? 3 : d < 305 ? 4 : 5;
            kv["SumP"] = "(P) " + FmtDot(pitch);
            kv["SumKona"] = "Kona 1:" + Fmt(kona);
            kv["SumKA"] = "max " + Fmt3Dot(RoundTo(ml * vt / 1000.0, 0.001)) + " [2F]";
            kv["SumRa"] = string.IsNullOrEmpty(tmpBet) ? "" : "1.6 [2F]";
            kv["SumRa1"] = kv["SumRa"];

            kv["SumFas"] = (d < 24 ? "0.7" : d < 69 ? "1.1" : d < 159 ? "1.8" : d < 219 ? "2.4" : "2.7") + "x45°";
            kv["SumFT"] = "";
            kv["SumFTN"] = "";
            kv["SumFasK"] = "";
            kv["SumFKT"] = "";
            kv["SumFKTN"] = "";
            kv["SumR1"] = "R0.5";
            kv["SumR1T"] = "";
            kv["SumR1TN"] = "";
            kv["SumR2"] = "R0.5";
            kv["SumR2T"] = "";
            kv["SumR2TN"] = "";

            kv["Sumd"] = "(d) " + Fmt(d);
            kv["Sumda"] = kv["Sumd"];
            double dTol = pitch == 0.75 ? 0.022 : pitch == 1 ? 0.026 : pitch == 1.5 ? 0.032 : pitch == 2 ? 0.038 : pitch == 3 ? 0.048 : double.NaN;
            double dTolN = pitch == 0.75 ? 0.162 : pitch == 1 ? 0.206 : pitch == 1.5 ? 0.268 : pitch == 2 ? 0.318 : pitch == 3 ? 0.423 : double.NaN;
            kv["SumdTol"] = "- " + Fmt3OrError(dTol, "Fel Stigning");
            kv["SumdTolN"] = "- " + Fmt3OrError(dTolN, "Fel Stigning");
            kv["SumdaTol"] = kv["SumdTol"];
            kv["SumdaTolN"] = kv["SumdTolN"];

            double dm = pitch == 0.75 ? d - 0.487 : pitch == 1 ? d - 0.65 : pitch == 1.5 ? d - 0.974 : pitch == 2 ? d - 1.299 : pitch == 3 ? d - 1.949 : 0;
            kv["Sumdm"] = "(dm) " + Fmt(dm);
            kv["SumdmTol"] = "- " + Fmt3OrError(dTol, "Fel Stigning") + " [3F]";
            double dmTolN = pitch == 0.75 ? 0.147 : pitch == 1 ? 0.176 : pitch == 1.5 && d < 45.1 ? 0.222 : pitch == 1.5 ? 0.232 : pitch == 2 && d < 90.1 ? 0.262 : pitch == 2 ? 0.274 : pitch == 3 && d < 180.1 ? 0.328 : 0.363;
            kv["SumdmTolN"] = "- " + Fmt3Dot(dmTolN) + " [3F]";

            kv["Sumb"] = "(b) " + Fmt(b);
            double bTol = b > 120 ? 4 : b > 80 ? 3.5 : b > 50 ? 3 : b > 30 ? 2.5 : b > 18 ? 2.1 : b > 10 ? 1.8 : 1.5;
            kv["SumbTol"] = "+ " + Fmt3Dot(bTol) + " [3F]";
            kv["SumbTolN"] = "- 0.000 [2F]";

            double wallTol = kona == 12 ? (d > 250 ? 0.055 : d > 180 ? 0.05 : d > 120 ? 0.04 : d > 80 ? 0.035 : d > 50 ? 0.03 : d > 30 ? 0.025 : 0.02) : kona == 30 ? (d > 250 ? 0.035 : d > 180 ? 0.03 : d > 120 ? 0.025 : d > 80 ? 0.022 : d > 50 ? 0.019 : d > 30 ? 0.016 : 0.013) : double.NaN;
            double wallTolN = kona == 12 ? (d > 250 ? 0.16 : d > 180 ? 0.14 : d > 120 ? 0.12 : d > 80 ? 0.105 : d > 50 ? 0.09 : d > 30 ? 0.075 : 0.07) : kona == 30 ? (d > 250 ? 0.095 : d > 180 ? 0.085 : d > 120 ? 0.075 : d > 80 ? 0.065 : d > 50 ? 0.055 : d > 30 ? 0.046 : 0.039) : double.NaN;
            kv["SumGodstjocklekTol"] = "+ " + Fmt3OrError(wallTol, "Fel Kona") + " [3F]";
            kv["SumGodstjocklekTolN"] = "- " + Fmt3OrError(wallTolN, "Fel Kona") + " [2F]";

            kv["Sumd2"] = "(d2) " + Fmt(d2);
            kv["Sumd2Tol"] = "± " + Fmt3Dot(GeneralTolerance(d2));
            kv["Sumd1"] = "(d1) " + Fmt(d1);
            double d1Tol = d1 > 250 ? 0.065 : d1 > 180 ? 0.057 : d1 > 120 ? 0.05 : d1 > 80 ? 0.043 : d1 > 50 ? 0.037 : d1 > 30 ? 0.031 : 0.026;
            double d1STol = d > 201 ? 0.185 : d > 131 ? 0.1 : d > 91 ? 0.087 : d > 56 ? 0.074 : d > 39 ? 0.062 : 0.052;
            double d1STolN = d > 201 ? 0.29 : d > 131 ? 0.25 : d > 91 ? 0.22 : d > 56 ? 0.12 : d > 39 ? 0.1 : 0.084;
            kv["Sumd1Tol"] = "± " + Fmt3Dot(d1Tol) + " [3F]";
            kv["Sumd1STol"] = "+ " + Fmt3Dot(d1STol) + " [3F]";
            kv["Sumd1STolN"] = "- " + Fmt3Dot(d1STolN) + " [3F]";

            string thread = "M" + Fmt(d);
            string pitchText = Fmt(pitch);
            kv["SumGängbet"] = thread;
            kv["SumStigning"] = pitchText;
            kv["SumGxS"] = thread + "x" + pitchText;
            kv["SumGängring"] = kv["SumGxS"];
            kv["SumGängtolk"] = kv["SumGxS"];
            kv["SumRullar"] = "M" + pitchText;
            kv["SumGängmall"] = kv["SumRullar"];

            kv["SumL"] = "(L) " + Fmt(length);
            kv["SumLTol"] = "+ 0.000";
            double lTolN = tudelad == 0 ? (length > 400 ? 2.5 : length > 315 ? 2.3 : length > 250 ? 2.1 : length > 180 ? 1.85 : length > 120 ? 1.6 : length > 80 ? 1.4 : length > 50 ? 1.2 : length > 30 ? 1 : length > 18 ? 0.84 : length > 10 ? 0.7 : 0.58) : tudelad == 1 ? (length > 630 ? 0.8 : length > 500 ? 0.7 : length > 400 ? 0.63 : length > 315 ? 0.57 : length > 250 ? 0.52 : length > 180 ? 0.46 : length > 120 ? 0.4 : length > 80 ? 0.35 : length > 50 ? 0.3 : length > 30 ? 0.25 : 0.21) : double.NaN;
            kv["SumLTolN"] = "- " + Fmt3OrError(lTolN, "Fel Tudelad") + " [3F]";
            kv["SumRd"] = Fmt3Dot(d1Tol);
            kv["SumSA"] = Fmt(d < 101 ? 0.008 : d < 281 ? 0.010 : d < 481 ? 0.012 : d < 601 ? 0.014 : d < 901 ? 0.016 : 0.020);
            kv["SumSB"] = Fmt(d < 101 ? 0.012 : d < 281 ? 0.015 : d < 481 ? 0.018 : d < 601 ? 0.021 : d < 901 ? 0.024 : 0.030);
            kv["SumVE"] = "max: " + (d > 250 ? "0.025" : d > 180 ? "0.020" : d > 120 ? "0.015" : d > 50 ? "0.010" : "0.008") + " [2F]";

            double c = typ < 5 ? 1.5 : typ < 11 ? 2 : 2.5;
            kv["Sumc"] = "(c) " + Fmt(c);
            kv["SumcTol"] = "+ 0";
            kv["SumcTolN"] = "- 0,014";
            double f = typ < 2 ? 9 : typ < 4 ? 10 : typ == 4 ? 12 : typ < 7 ? 14.5 : typ == 7 ? 15.5 : typ == 8 ? 16.5 : typ == 9 ? 17.5 : typ == 10 ? 18.5 : typ < 13 ? 19.5 : typ == 13 ? 20.5 : typ == 14 ? 21 : 22;
            kv["Sumf"] = "(f) " + FmtDot(f);
            double fTol = f > 51 ? 3 : f > 31 ? 2.5 : f > 19 ? 2.1 : 1.8;
            kv["SumfTol"] = "+ " + Fmt3Dot(fTol);
            kv["SumfTolN"] = "- 0.000 [3F]";
            double e = f == 9 ? 4 : f < 13 ? 5 : f < 15 ? 6 : f < 19 ? 7 : 9;
            kv["Sume"] = "(e) " + FmtDot(e);
            kv["SumeTol"] = "+ 0";
            kv["SumeTolN"] = "- 0,022";
            kv["SumDelHylFrBr"] = tudelad == 0 ? "" : "Fräsbredd delad hylsa 2mm";

            kv["SumMaskinValS1"] = "Maskin: " + (machineValid ? machine : "");
            kv["SumF1_1"] = machineValid ? isH ? "1/75" : "1/20" : "";
            kv["SumF1_2"] = machineValid ? "4/skift" : "";
            kv["SumF1_3"] = machineValid ? "inst." : "";
            kv["SumF1_4"] = machineValid ? isH ? "1/75" : "1/20" : "";
            kv["SumF1_5"] = "";
            kv["SumF1_6"] = machineValid ? "1/50" : "";
            kv["SumF1_7"] = machineValid ? isH ? "1/75" : "1/20" : "";
            kv["SumF1_8"] = machineValid ? "4/skift" : "";
            kv["SumF1_9"] = machineValid ? "inst./1/skift" : "";
            kv["SumF1_0"] = machineValid ? "inst./1/skift" : "";
            kv["SumF1_11"] = machineValid ? "4/skift" : "";

            kv["SumD1_1"] = machineValid ? "UD-Apparat inställd med hylsa, ring el. klove." : "";
            kv["SumD1_2"] = machineValid ? "Höjdmätapparat" : "";
            kv["SumD1_3"] = machineValid ? "Skjutmått" : "";
            kv["SumD1_4"] = machineValid ? "UGM-Apparat" : "";
            kv["SumD1_5"] = "";
            kv["SumD1_6"] = machineValid ? "Ytjämnhetsmätare" : "";
            kv["SumD1_7"] = machineValid ? "HKA MÄTLÄNGD:" + kv["SumML"] + "mm" : "MHB MÄTLÄNGD:" + kv["SumML"] + "mm";
            kv["SumD1_8"] = machineValid ? "UD-Apparat" : "";
            kv["SumD1_9"] = machineValid ? "Mätrum/Alt.Egglinjal" : "";
            kv["SumD1_0"] = machineValid ? "Mätrum/Rakhetsmätare" : "";
            kv["SumD1_11"] = machineValid ? "Skjutmått" : "";

            kv["SumAF1_1"] = machineValid ? "Tolerans efter slits:" : "";
            kv["SumAF1_2"] = machineValid ? "Inställd med passbitar." : "";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = machineValid ? "Spetsar: " + kv["SumRullar"] + ", Gängtolk: " + kv["SumGängtolk"] + " Mäts i 2 snitt med 90° vridning" : "";
            kv["SumAF1_5"] = "";
            kv["SumAF1_6"] = machineValid ? "Rp=5, Rz=8" : "";
            kv["SumAF1_7"] = machineValid ? "Inställningshylsa " + Fmt(d1) + "mm. Mäts i 2 snitt med 90° vridning" + (bricka ? Environment.NewLine + sumBricka : "") : "";
            kv["SumAF1_8"] = "";
            kv["SumAF1_9"] = machineValid ? "Vid misstänkt formfel kontrollera hylsan i MarSurf Contour XC20." : "";
            kv["SumAF1_0"] = "";
            kv["SumAF1_11"] = "";

            kv["SumTextFrekvens_6x3"] = "";
            kv["SumText1"] = machineValid ? "Okulär kontroll av Grader, frifläckar, slagmärken, repor, valkar etc. Märkning ska vara rätt och tydlig. Görs 2ggr/Tim." : "";
            kv["SumText2"] = machineValid ? "Om måttfel upptäcks skall man gå tillbaka och mäta alla hylsor tills godkänd detalj hittats." : "";
            kv["SumText3"] = "";

            return kv;
        }

        private static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published) || !DateTime.TryParse(published, out var dt))
                return "";
            var until = dt.AddDays(14);
            return DateTime.Today <= until.Date
                ? "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)" + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + "Popupruta aktiv till " + until.ToString("yyyy-MM-dd")
                : "";
        }

        private static double GetBookmarkDouble(APIRequest req, string name)
        {
            if (req?.Bookmarks == null)
                return 0;
            foreach (var bookmark in req.Bookmarks)
                if (string.Equals(bookmark.BookmarkName ?? "", name, StringComparison.OrdinalIgnoreCase))
                    return ToDouble(bookmark.BookmarkValue);
            return 0;
        }

        private static bool EqualsAny(string value, params string[] values)
        {
            foreach (var item in values)
                if (string.Equals(value ?? "", item, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }

        private static bool ContainsAny(string value, params string[] values)
        {
            foreach (var item in values)
                if ((value ?? "").IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
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

        private static double GeneralTolerance(double value)
        {
            if (value < 6.01) return 0.1;
            if (value < 30.01) return 0.2;
            if (value < 120.01) return 0.3;
            if (value < 400.01) return 0.5;
            if (value < 1000.01) return 0.8;
            if (value < 2000.01) return 1.2;
            return 2.0;
        }

        private static string Fmt(double value)
        {
            return value.ToString("0.################", CommonFunctions.Culture);
        }

        private static string FmtDot(double value)
        {
            return value.ToString("0.################", CultureInfo.InvariantCulture);
        }

        private static string Fmt3Dot(double value)
        {
            return value.ToString("0.000", CultureInfo.InvariantCulture);
        }

        private static string Fmt3OrError(double value, string error)
        {
            return double.IsNaN(value) ? error : Fmt3Dot(value);
        }
    }
}
