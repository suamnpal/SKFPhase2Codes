using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Klamhylsor_60_1060_MacTurn_550_VTR_160 : ITemplateCalculations
    {
        private const string LB = "<<LineBreak>>";

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            InitializeKeys(kv);

            string subject = (req?.ProductDesignation ?? string.Empty).Trim();
            string machine = (req?.MachineNumber ?? string.Empty).Trim();
            List<Bookmark> bm = req?.Bookmarks;

            string tmpBet = subject.Trim().ToUpperInvariant().Replace('.', ',');
            string[] parts = tmpBet.Split(new[] { ' ', '/', '.', '-', '_' }, StringSplitOptions.RemoveEmptyEntries);
            string bet2 = Word(parts, 2);
            string bet3 = Word(parts, 3);
            bool slash = tmpBet.IndexOf("/", StringComparison.OrdinalIgnoreCase) >= 0;
            bool lw = tmpBet.IndexOf("LW", StringComparison.OrdinalIgnoreCase) >= 0;
            int count2 = bet2.Length;
            int count3 = bet3.Length;
            string serie = lw ? "0" : slash && count2 == 3 ? bet2 : count2 > 4 ? Left(bet2, 3) : count2 == 3 ? Left(bet2, 1) : Left(bet2, 2);
            double typ = !slash ? ToDouble(count2 > 3 ? Right(bet2, 2) : Right(bet2, 2)) : count3 > 4 ? ToDouble(Right(bet2, 2)) : ToDouble(bet3);

            double dBookmark = GetDouble(bm, "Avvikande YDia Gänga (d)");
            double d = dBookmark == 0 ? (!slash || count3 > 4 ? (typ / 2.0) * 10.0 : ToDouble(bet3)) : dBookmark;
            double d1 = GetDouble(bm, "Innerdiameter (d1)");
            double d2Bookmark = GetDouble(bm, "Kona storände diameter (d2)");
            double b = GetDouble(bm, "Gänglängd (b)");
            double l = GetDouble(bm, "Längd (L)");
            double a = GetDouble(bm, "a-mått");
            double konaBookmark = GetDouble(bm, "Kona");
            double kona = konaBookmark == 0 ? (serie == "30" || serie == "31" || serie == "32" || serie == "39" ? 12 : 30) : konaBookmark;
            string productDrawing = GetString(bm, "Produktritning");
            // Radius bookmarks may contain formatted text such as "R5".
            // Keep the raw values to match the Lotus Notes string logic.
            string radiusLargeRaw = GetString(bm, "Radie Storände").Trim();
            string radiusSmallRaw = GetString(bm, "Radie Lillände").Trim();
            double radiusLarge = ToDouble(radiusLargeRaw);
            double radiusSmall = ToDouble(radiusSmallRaw);
            double pitch = d < 301 ? 4 : d < 501 ? 5 : d < 701 ? 6 : d < 901 ? 7 : 8;

            kv["SumGänga"] = "Tr" + FormatSimple(d) + "x" + FormatSimple(pitch);
            kv["SumP"] = "Tr" + FormatSimple(pitch);
            kv["SumP1"] = "(P) " + FormatSimple(pitch);
            kv["SumRullar"] = FormatSimple(pitch) + "mm";
            kv["SumV"] = Math.Abs(kona - 30) < 0.0001 ? "(V) 0º57" : "(V) 2º23";
            kv["SumKonaOP1"] = "Kona 1:" + FormatSimple(kona);
            kv["SumKona"] = kv["SumKonaOP1"];

            kv["SumLOP1"] = "(L) " + FormatSimple(l);
            kv["SumLOP1Tol"] = "0.250";
            kv["SumL"] = "(L) " + FormatSimple(l);
            kv["SumLTol"] = "+ 0.000 [3F]";
            kv["SumLTolN"] = "-  " + F3(H15Negative(l)) + " [3F]";

            kv["Sumb"] = "(b) " + FormatSimple(b);
            kv["SumbTol"] = "+ " + F3(BTolerance(b)) + "  [3F]";
            kv["SumbTolN"] = "-  0.000 [2F]";

            double sl = b + 4;
            kv["SumSL"] = "(SL) " + FormatSimple(sl);
            kv["SumSLTol"] = "± 0.300";

            kv["Sumd1"] = "(d1) " + FormatSimple(d1).Replace(".", ",");
            // Inner-diameter tolerance and roundness are the same JS9 value.
            // Calculate once from d1 so the two outputs cannot drift apart.
            double innerDiameterTolerance = Js9(d1);
            kv["Sumd1Tol"] = "± " + F3(innerDiameterTolerance) + " [3F]";

            kv["SumdOP1"] = "(d) " + FormatSimple(d);
            kv["SumdOP1Tol"] = "+ 0.000";
            kv["SumdOP1TolN"] = "-  " + F3(ThreadOuterNegative(pitch));
            kv["SumdaOP1"] = kv["SumdOP1"];
            kv["SumdaOP1Tol"] = kv["SumdOP1Tol"];
            kv["SumdaOP1TolN"] = kv["SumdOP1TolN"];
            kv["Sumd"] = kv["SumdOP1"];
            kv["SumdTol"] = kv["SumdOP1Tol"];
            kv["SumdTolN"] = kv["SumdOP1TolN"];

            double dm = d - pitch / 2.0;
            kv["Sumdm"] = "(dm) " + FormatSimple(dm);
            kv["SumdmTol"] = "- " + F3(ThreadMeanPlus(pitch)) + " [3F]";
            kv["SumdmTolN"] = "- " + F3(ThreadMeanMinus(pitch)) + " [3F]";

            double d3 = pitch == 4 ? dm - 2.5 : pitch == 5 ? dm - 3 : pitch == 6 ? dm - 4 : pitch == 7 ? dm - 4.5 : dm < 1300 ? dm - 5 : dm - 4.5;
            kv["Sumd3"] = "(d3) " + FormatSimple(d3).Replace(".", ",");
            kv["Sumd3Tol"] = "+ 0.000";
            kv["Sumd3TolN"] = "-  " + F3(ThreadCoreNegative(pitch));

            string g = EqualsI(productDrawing, "MS-7434039") ? "4.4x30º" : d < 301 ? "2.7x45º" : d < 501 ? "3.2x45º" : d < 671 ? "3.8x45º" : d < 901 ? "4.4x45º" : "5x45º";
            kv["Sumg"] = "(g) " + g;
            string smallRadiusText = IsZeroBookmark(radiusSmallRaw)
                ? FormatSimple(d < 421 ? 1 : 2.5)
                : NormalizeRadiusBookmark(radiusSmallRaw, radiusSmall);
            string largeRadiusText = IsZeroBookmark(radiusLargeRaw)
                ? FormatSimple(d < 320 ? 2.5 : d < 530 ? 3.5 : d < 710 ? 5.5 : 7.5)
                : NormalizeRadiusBookmark(radiusLargeRaw, radiusLarge);

            // Lotus Notes always prefixes the bookmark/formula result with "R".
            // Example: Radie Storände = "R5" produces SumR = "RR5".
            kv["Sumr1"] = "R" + smallRadiusText;
            kv["Sumr"] = "R" + largeRadiusText;

            double gtPlus = ThicknessPlus(kona, d);
            double gtMinus = ThicknessMinus(kona, d);
            double toleranceDifference = gtMinus - gtPlus;
            kv["SumGTjTol"] = "+ " + F3(gtPlus);
            kv["SumGTjaTol"] = kv["SumGTjTol"];
            kv["SumGTjTolN"] = "-  " + F3(gtMinus);
            kv["SumGTjaTolN"] = kv["SumGTjTolN"];

            double d2 = d2Bookmark == 0 ? RoundTo(((l - 1 - a) / Safe(kona)) + d - toleranceDifference, 0.01) : d2Bookmark;
            kv["Sumd2"] = "(d2) " + FormatSimple(d2);
            kv["Sumd2Tol"] = "± " + F3(GeneralTolerance(d2));

            kv["SumGVarTol"] = F3(ThicknessVariation(d)) + " [2F]";
            kv["SumRakA"] = "Max: " + FormatSimple(StraightnessA(d));
            kv["SumRakB"] = "Max: " + FormatSimple(StraightnessB(d));
            kv["SumOrund"] = F3(innerDiameterTolerance) + " [3F]";

            double ml = l - b - 4;
            kv["SumML"] = "Mätlängd=" + (ml < 110 ? "75" : "100");
            kv["SumRa"] = "2.5 [2F]";
            kv["SumRa1"] = "2.5 [2F]";
            kv["SumRa5"] = "5 [2F]";
            double vml = ml < 110 ? 75 : 100;
            double angleBase = d > 1000 ? 0.09 : d > 800 ? 0.10 : d > 630 ? 0.11 : d > 500 ? 0.12 : d > 400 ? 0.13 : d > 180 ? 0.15 : d > 150 ? 0.18 : d > 120 ? 0.30 : d > 80 ? 0.45 : d > 50 ? 0.50 : 0.60;
            kv["SumVinkTol"] = ((angleBase * vml) / 1000.0).ToString("0.#####", CultureInfo.InvariantCulture) + " [2F]";

            double e8 = RoundTo(((d - d1) / 2.0) + ((l - 1 - a - 8) / (2 * Safe(kona))), 0.001);
            double e83 = RoundTo(((d - d1) / 2.0) + ((l - 1 - a - 83) / (2 * Safe(kona))), 0.001);
            double e108 = RoundTo(((d - d1) / 2.0) + ((l - 1 - a - 108) / (2 * Safe(kona))), 0.001);
            double e40 = RoundTo(((d - d1) / 2.0) + ((l - 1 - a - 40) / (2 * Safe(kona))), 0.001);
            double e140 = RoundTo(((d - d1) / 2.0) + ((l - 1 - a - 140) / (2 * Safe(kona))), 0.001);
            double sumL2 = ml < 145 ? 8 : 40;
            double sumL1 = ml < 110 ? 83 : ml < 145 ? 108 : 140;
            double sumE1 = ml < 110 ? e83 : ml < 145 ? e108 : e140;
            double sumE2 = ml < 145 ? e8 : e40;
            kv["SumL1"] = FormatSimple(sumL1);
            kv["SumL2"] = FormatSimple(sumL2);
            kv["SumE1"] = sumE1.ToString("0.###", CultureInfo.InvariantCulture).Replace('.', ',');
            kv["SumE2"] = sumE2.ToString("0.###", CultureInfo.InvariantCulture).Replace('.', ',');

            string gauge = ml < 145 ? "SR 7419471" : "SR 7415991";
            kv["SumBygGtj"] = gauge;
            kv["SumBygGVar"] = gauge;
            kv["SumBygVinkTol"] = gauge;

            bool machineMatch = EqualsI(machine, "MacTurn 550") || EqualsI(machine, "VTR-160");
            kv["SumMaskinValS1"] = machineMatch ? "Maskin: " + machine + " - OP1" : "Maskin:  - OP1";
            kv["SumMaskinValS2"] = machineMatch ? "Maskin: " + machine + " - OP2" : "Maskin:  - OP2";

            SetMachineFields(kv, machineMatch, pitch, gauge);

            kv["SumTextS1"] = "Kontrollera rätt märkning" + LB + "Okulärkontroll gjuteridefekter, grader & slagmärken" + LB + "Gjutgodsdefekter: 7433015";
            kv["SumTextS2"] = "Bryt alla kanter, avlägsna. Okulärkontroll märkning, gjuteridefekter, grader & slagmärken";

            string drawing = EqualsI(productDrawing, "0") || string.IsNullOrWhiteSpace(productDrawing) ? serie == "30" ? "7438957" : serie == "31" ? "7438958" : serie == "32" ? "7438955" : serie == "39" ? "7434032" : serie == "240" ? "7432901" : tmpBet : productDrawing;
            kv["SumRitningsnrS1"] = drawing + ":senaste utg.";
            kv["SumRitningsnrS2"] = kv["SumRitningsnrS1"];
            kv["SumRitTolS1"] = "Toleranser: 1432012:7";
            kv["SumRitTolS2"] = kv["SumRitTolS1"];
            kv["SumRitGänga"] = "Gänga: 237359:2, 7430181:2";
            kv["SumKlEgenskaperS1"] = "PRODUCTION NUTS & SLEEVES & HOUSINGS\\ALLMÄNKLASSADE EGENSKAPER\\Klassade egenskaper klämhylsor";
            kv["SumKlEgenskaperS2"] = kv["SumKlEgenskaperS1"];
            kv["VaLPopUp"] = Popup(req?.Published);

            BuildMeasureCenter(kv, machineMatch, subject, machine, gauge, ml, sumL1, sumE1, sumE2);
            return kv;
        }
        private static string F3Comma(double v) { return v.ToString("0.000", CultureInfo.InvariantCulture).Replace('.', ','); }

        private static void SetMachineFields(Dictionary<string, string> kv, bool ok, double pitch, string gauge)
        {
            string[] f1 = { "1/5", "1/2", "1/1", "1/5", "Inst.", "1/5", "1/2", "1/5" };
            string[] f2 = { "1/2", "1/2", "1/1", "1/1", "1/1", "Inst.", "1/5", "1/5", "1/1" };
            string[] d1 = { "Skjutmått", "Skjutmått", "Multimar med " + FormatSimple(pitch) + "mm rullar", "Djupmått", "Radielyra", "Skjutmått/Vinkelmätare", "Gängmall Tr" + FormatSimple(pitch), "Skjutmått" };
            string[] d2 = { "Mikrometerstickmått", "Skjutmått", "Mätbygel " + gauge, "Mätbygel " + gauge, "Mätbygel " + gauge, "Mätmaskin", "Egglinjal", "Egglinjal", "Okulärkontroll" };
            string[] af1 = { "", "", "Kontrolleras med klove utf.2", "", "", "", "", "Hjälpmått" };
            string[] af2 = { "", "", "Tol:", "Tol: " + kv["SumGVarTol"], "Tol: " + kv["SumVinkTol"] + " " + kv["SumML"], "Max: " + kv["SumOrund"], kv["SumRakA"], kv["SumRakB"], "Vid misstänkt fel Ra-mätare" };
            SetSeries(kv, "SumF1_", f1, ok);
            SetSeries(kv, "SumF2_", f2, ok);
            SetSeries(kv, "SumD1_", d1, ok);
            SetSeries(kv, "SumD2_", d2, ok);
            SetSeries(kv, "SumAF1_", af1, ok);
            SetSeries(kv, "SumAF2_", af2, ok);
        }

        private static void SetSeries(Dictionary<string, string> kv, string prefix, string[] values, bool enabled)
        {
            for (int i = 0; i < values.Length; i++) kv[prefix + (i + 1)] = enabled ? values[i] : "";
        }

        private static void BuildMeasureCenter(Dictionary<string, string> kv, bool ok, string subject, string machine, string gauge, double ml, double l1, double e1, double e2)
        {
            kv["VaLFärdig"] = "";
            kv["VaLInfo"] = "";
            if (!ok) return;
            DateTime now = DateTime.Now;
            bool afterNine = now.Hour > 9;
            double constant = l1 == 83 || l1 == 108 ? -5 : l1 == 140 ? -10 : 0;
            string instE1 = F3((l1 == 83 || l1 == 108) ? e1 - 5 : e1 - 10);
            string instE2 = F3((l1 == 83 || l1 == 108) ? e2 - 5 : e2 - 10);
            if (afterNine) kv["VaLFärdig"] = "Din beställning gjordes efter senast 10:00 så leverans kan ske nästkommande helgfria vardag vid 10:00" + LB + "Om du skulle behöva passbitarna tidigare så var god och kontakta mätsevice personligen för överrenskommelse";
            kv["VaLInfo"] = "Du har gjort följande beställning ifrån mätcenter:" + LB + LB + LB + "Passbitar till " + subject + " som körs vid " + machine + LB + LB + "Mätbygel: " + gauge + LB + LB + "Bygelinställningskonstant: " + FormatSimple(constant) + LB + LB + "Mätlängd=" + FormatSimple(ml) + LB + LB + "E1 uträknat mått: " + instE1 + LB + LB + "E2 uträknat mått: " + instE2;
        }

        private static string Popup(string published)
        {
            if (!DateTime.TryParse(published, out DateTime date)) return "";
            DateTime valid = date.AddDays(14);
            if (DateTime.Today > valid.Date) return "";
            return "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)" + LB + LB + LB + LB + "Information om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument" + LB + LB + "Popupruta aktiv till " + valid.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        private static double H15Negative(double v) { return v < 3.01 ? .4 : v < 6.01 ? .48 : v < 10.01 ? .58 : v < 18.01 ? .7 : v < 30.01 ? .84 : v < 50.01 ? 1 : v < 80.01 ? 1.2 : v < 120.01 ? 1.4 : v < 180.01 ? 1.6 : v < 250.01 ? 1.85 : v < 315.01 ? 2.1 : v < 400.01 ? 2.3 : v < 500.01 ? 2.5 : v < 630.01 ? 2.8 : v < 800.01 ? 3.2 : v < 1000.01 ? 3.6 : v < 1250.01 ? 4.2 : v < 1600.01 ? 5 : v < 2000.01 ? 6 : v < 2500.01 ? 7 : 8.6; }
        private static double BTolerance(double v) { return v < 11 ? 1.5 : v < 19 ? 1.8 : v < 31 ? 2.1 : v < 51 ? 2.5 : v < 81 ? 3 : v < 121 ? 3.5 : 4; }
        private static double Js9(double v) { return v < 3.01 ? .012 : v < 6.01 ? .015 : v < 10.01 ? .018 : v < 18.01 ? .021 : v < 30.01 ? .026 : v < 50.01 ? .031 : v < 80.01 ? .037 : v < 120.01 ? .043 : v < 180.01 ? .05 : v < 250.01 ? .057 : v < 315.01 ? .065 : v < 400.01 ? .07 : v < 500.01 ? .077 : v < 630.01 ? .087 : v < 800.01 ? .1 : v < 1000.01 ? .115 : v < 1250.01 ? .13 : v < 1600.01 ? .155 : v < 2000.01 ? .185 : v < 2500.01 ? .22 : .27; }
        private static double ThreadOuterNegative(double p) { return p == 4 ? .3 : p == 5 ? .335 : p == 6 ? .375 : p == 7 ? .425 : .45; }
        private static double ThreadMeanPlus(double p) { return p == 4 ? .19 : p == 5 ? .212 : p == 6 ? .236 : p == 7 ? .25 : .265; }
        private static double ThreadMeanMinus(double p) { return p == 4 ? .63 : p == 5 ? .71 : p == 6 ? .8 : p == 7 ? .85 : .95; }
        private static double ThreadCoreNegative(double p) { return p == 4 ? .75 : p == 5 ? .85 : p == 6 ? .95 : p == 7 ? 1 : 1.12; }
        private static double GeneralTolerance(double v) { return v < 6.01 ? .1 : v < 30.01 ? .2 : v < 120.01 ? .3 : v < 400.01 ? .5 : v < 1000.01 ? .8 : v < 2000.01 ? 1.2 : 2; }
        private static double ThicknessVariation(double d) { return d > 1000 ? .05 : d > 800 ? .045 : d > 630 ? .04 : d > 500 ? .035 : d > 315 ? .03 : d > 250 ? .025 : d > 180 ? .02 : d > 120 ? .015 : d > 50 ? .01 : .008; }
        private static double StraightnessA(double d) { return (d < 101 ? 8 : d < 281 ? 10 : d < 481 ? 12 : d < 601 ? 14 : d < 901 ? 16 : 20) / 1000.0; }
        private static double StraightnessB(double d) { return (d < 101 ? 12 : d < 281 ? 15 : d < 481 ? 18 : d < 601 ? 21 : d < 901 ? 24 : 30) / 1000.0; }

        private static double ThicknessPlus(double k, double d)
        {
            if (Math.Abs(k - 12) < .0001) return d > 1000 ? .095 : d > 800 ? .085 : d > 630 ? .075 : d > 500 ? .07 : d > 400 ? .065 : d > 315 ? .06 : d > 250 ? .055 : d > 180 ? .05 : d > 120 ? .04 : d > 80 ? .035 : d > 50 ? .03 : d > 30 ? .025 : .02;
            if (Math.Abs(k - 30) < .0001) return d > 1000 ? .06 : d > 800 ? .055 : d > 630 ? .05 : d > 500 ? .045 : d > 400 ? .04 : d > 315 ? .035 : d > 250 ? .035 : d > 180 ? .03 : d > 120 ? .025 : d > 80 ? .022 : d > 50 ? .019 : d > 30 ? .016 : .013;
            return 0;
        }

        private static double ThicknessMinus(double k, double d)
        {
            if (Math.Abs(k - 12) < .0001) return d > 1000 ? .28 : d > 800 ? .25 : d > 630 ? .225 : d > 500 ? .2 : d > 400 ? .19 : d > 315 ? .175 : d > 250 ? .16 : d > 180 ? .14 : d > 120 ? .12 : d > 80 ? .105 : d > 50 ? .09 : d > 30 ? .075 : .07;
            if (Math.Abs(k - 30) < .0001) return d > 1000 ? .17 : d > 800 ? .155 : d > 630 ? .14 : d > 500 ? .125 : d > 400 ? .115 : d > 315 ? .105 : d > 251 ? .095 : d > 180 ? .085 : d > 120 ? .075 : d > 80 ? .065 : d > 50 ? .055 : d > 30 ? .046 : .039;
            return 0;
        }

        private static string GetString(List<Bookmark> bm, string key)
        {
            if (bm == null) return "";
            foreach (Bookmark item in bm) if (item != null && EqualsI(item.BookmarkName, key)) return item.BookmarkValue ?? "";
            return "";
        }

        private static double GetDouble(List<Bookmark> bm, string key) { return ToDouble(GetString(bm, key)); }
        private static bool IsZeroBookmark(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return true;
            string normalized = value.Trim().Replace('.', ',');
            return double.TryParse(normalized, NumberStyles.Any, CommonFunctions.Culture, out double parsed)
                && Math.Abs(parsed) < .0000001;
        }
        private static string NormalizeRadiusBookmark(string rawValue, double numericValue)
        {
            string value = (rawValue ?? "").Trim();
            return Math.Abs(numericValue) > .0000001
                ? FormatSimple(numericValue)
                : value.Replace(',', '.');
        }
        private static double ToDouble(string s) { return double.TryParse((s ?? "").Trim().Replace('.', ','), NumberStyles.Any, CommonFunctions.Culture, out double v) ? v : 0; }
        private static string Word(string[] a, int n) { return a != null && n > 0 && n <= a.Length ? a[n - 1] : ""; }
        private static string Left(string s, int n) { return string.IsNullOrEmpty(s) ? "" : s.Length <= n ? s : s.Substring(0, n); }
        private static string Right(string s, int n) { return string.IsNullOrEmpty(s) ? "" : s.Length <= n ? s : s.Substring(s.Length - n); }
        private static bool EqualsI(string a, string b) { return string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase); }
        private static double Safe(double v) { return Math.Abs(v) < .0000001 ? 1 : v; }
        private static double RoundTo(double v, double step) { return Math.Round(v / step, MidpointRounding.AwayFromZero) * step; }
        private static string F3(double v) { return v.ToString("F3", CommonFunctions.Culture).Replace(',', '.'); }
        private static string FormatSimple(double v) { return Math.Abs(v - Math.Round(v)) < .0000001 ? Math.Round(v).ToString(CultureInfo.InvariantCulture) : v.ToString("0.###", CommonFunctions.Culture).Replace(',', '.'); }
        private static void InitializeKeys(Dictionary<string, string> kv) { foreach (string key in Keys) kv[key] = ""; }

        private static readonly string[] Keys =
        {
            "VaLPopUp","VaLFärdig","VaLInfo","SumGänga","SumP","SumP1","SumRullar","SumV","SumKonaOP1","SumKona","SumLOP1","SumLOP1Tol","SumL","SumLTol","SumLTolN","Sumb","SumbTol","SumbTolN","SumSL","SumSLTol","Sumd1","Sumd1Tol","SumdOP1","SumdOP1Tol","SumdOP1TolN","SumdaOP1","SumdaOP1Tol","SumdaOP1TolN","Sumd","SumdTol","SumdTolN","Sumdm","SumdmTol","SumdmTolN","Sumd3","Sumd3Tol","Sumd3TolN","Sumg","Sumr1","Sumr","SumGTjTol","SumGTjaTol","SumGTjTolN","SumGTjaTolN","Sumd2","Sumd2Tol","SumGVarTol","SumRakA","SumRakB","SumOrund","SumML","SumRa","SumRa1","SumRa5","SumVinkTol","SumL1","SumL2","SumE1","SumE2","SumBygGtj","SumBygGVar","SumBygVinkTol","SumMaskinValS1","SumMaskinValS2","SumTextS1","SumTextS2","SumRitningsnrS1","SumRitningsnrS2","SumRitTolS1","SumRitTolS2","SumRitGänga","SumKlEgenskaperS1","SumKlEgenskaperS2",
            "SumF1_1","SumF1_2","SumF1_3","SumF1_4","SumF1_5","SumF1_6","SumF1_7","SumF1_8","SumF2_1","SumF2_2","SumF2_3","SumF2_4","SumF2_5","SumF2_6","SumF2_7","SumF2_8","SumF2_9","SumD1_1","SumD1_2","SumD1_3","SumD1_4","SumD1_5","SumD1_6","SumD1_7","SumD1_8","SumD2_1","SumD2_2","SumD2_3","SumD2_4","SumD2_5","SumD2_6","SumD2_7","SumD2_8","SumD2_9","SumAF1_1","SumAF1_2","SumAF1_3","SumAF1_4","SumAF1_5","SumAF1_6","SumAF1_7","SumAF1_8","SumAF2_1","SumAF2_2","SumAF2_3","SumAF2_4","SumAF2_5","SumAF2_6","SumAF2_7","SumAF2_8","SumAF2_9"
        };
    }
}
