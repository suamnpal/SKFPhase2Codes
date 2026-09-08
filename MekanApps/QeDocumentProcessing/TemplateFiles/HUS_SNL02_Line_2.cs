using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HUS_SNL02_Line_2 : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var product = ParseProduct(request);
            var result = InitializeAll();
            Merge(result, GetHeader(request));
            Merge(result, GetSurface());
            Merge(result, GetDimensions(product));
            Merge(result, GetMachine(request));
            Merge(result, GetFrequency(request, product));
            Merge(result, GetTools(request, product));
            Merge(result, GetRemarks(request, product));
            Merge(result, GetOtherText());
            return result;
        }

        private Dictionary<string, string> InitializeAll()
        {
            var d = new Dictionary<string, string>();
            string[] keys = {
                "SumRa32","SumRa32a","SumRa63","SumRp8","SumWt20","SumWt35","SumC","SumSkr",
                "SumAd","SumAdTol","SumAdTolN","SumPd","SumPdTol","SumPdTolN","SumLd","SumLdTol","SumLdTolN",
                "SumLb","SumLbTol","SumLbTolN","SumPb","SumPbTol","SumPbTolN","SumG1","SumG","SumGd",
                "SumFb","SumFbTol","SumHd","SumHdTol","SumBd","SumBdTol","SumBdTolN",
                "SumSd","SumSdTol","SumSdTolN","SumSö","SumSöTol","SumSöTolN","SumSu","SumSuTol","SumSuTolN",
                "SumUh","SumUhTol","SumFh","SumFhTol","SumVh","SumVhTol","SumSs","SumSsTol","SumSsTolN",
                "SumPl","SumCZ","SumPos","SumMaskinValS1","SumMaskinValS2",
                "SumF1_1","SumF1_2","SumF1_3","SumF1_4","SumF1_5","SumF1_6","SumF1_7",
                "SumF2_1","SumF2_2","SumF2_3","SumF2_4","SumF2_5","SumF2_6","SumF2_7","SumF2_8","SumF2_9","SumF2_10","SumF2_11","SumF2_12","SumF2_13","SumF2_14","SumF2_15",
                "SumD1_1","SumD1_2","SumD1_3","SumD1_4","SumD1_5","SumD1_6","SumD1_7",
                "SumD2_1","SumD2_2","SumD2_3","SumD2_4","SumD2_5","SumD2_6","SumD2_7","SumD2_8","SumD2_9","SumD2_10","SumD2_11","SumD2_12","SumD2_13","SumD2_14","SumD2_15",
                "SumAF1_1","SumAF1_2","SumAF1_3","SumAF1_4","SumAF1_5","SumAF1_6","SumAF1_7","SumAF1_7tb",
                "SumAF2_1","SumAF2_2","SumAF2_3","SumAF2_4","SumAF2_5","SumAF2_6","SumAF2_7","SumAF2_8","SumAF2_9","SumAF2_10","SumAF2_11","SumAF2_12","SumAF2_13","SumAF2_13tb","SumAF2_14","SumAF2_15",
                "SumTextS1","SumTextS2","SumPrdritS1","SumPrdritS2","SumArbInstGjgS1","SumArbInstGjgS2","SumKvStPlS1","SumKvStPlS2"
            };
            foreach (var key in keys) d[key] = "";
            return d;
        }

        private Dictionary<string, string> GetHeader(APIRequest request)
        {
            var bet = Normalize(request.ProductDesignation);
            var drawing = bet.EndsWith(".02", StringComparison.OrdinalIgnoreCase) ? bet : bet + " .02";
            return new Dictionary<string, string>
            {
                {"SumPrdritS1", "Produktritning: " + drawing},
                {"SumPrdritS2", "Produktritning: " + drawing},
                {"SumArbInstGjgS1", "Gjutgods: A3.022"},
                {"SumArbInstGjgS2", "Gjutgods: A3.022"},
                {"SumKvStPlS1", "Kvalitetstyrning: K1.07-17"},
                {"SumKvStPlS2", "Kvalitetstyrning: K1.07-17"}
            };
        }

        private Dictionary<string, string> GetSurface()
        {
            return new Dictionary<string, string>
            {
                {"SumRa32", "3.2"},
                {"SumRa32a", "3.2"},
                {"SumRa63", "6.3 [3]"},
                {"SumRp8", "Rp 8"},
                {"SumWt35", "Wt 35"},
                {"SumWt20", "Wt 20"},
                {"SumC", "1,0"}
            };
        }

        private Dictionary<string, string> GetDimensions(ProductData p)
        {
            var d = new Dictionary<string, string>();
            var ad = Pick(p, p.Serie == 2 ? new double[] { 120, 0, 0, 147.5, 157.5, 167.5, 177.5, 0, 0 } : new double[] { 102.5, 131, 0, 147.5, 157.5, 167.5, 177.5, 192.5, 0 });
            d["SumAd"] = "(Ad) " + Format(ad);
            d["SumAdTol"] = "+ " + F3(TolH12(ad)) + " [3]";
            d["SumAdTolN"] = "- 0 [3]";
            var pd = p.Typ > 18 ? ad + 10 : ad + 8.5;
            d["SumPd"] = "(Pd) " + Format(pd);
            d["SumPdTol"] = "+ " + F3(TolH12(pd)) + " [3]";
            d["SumPdTolN"] = "- 0 [3]";
            var ld = Pick(p, new double[] { 160, 170, 0, 200, 215, 230, 250, 270, 0 });
            d["SumLd"] = "(Ld) " + Format(ld);
            d["SumLdTol"] = "+ " + F3(TolG7Upper(ld)) + " [3]";
            d["SumLdTolN"] = "+ " + F3(TolG7Lower(ld)) + " [2D]";
            var lb = Pick(p, new double[] { 65, 68, 0, 80, 86, 90, 98, 106, 0 });
            d["SumLb"] = "(Lb) " + Format(lb);
            d["SumLbTol"] = "+ " + F3(TolH12Width(lb)) + " [3]";
            d["SumLbTolN"] = "- 0 [2]";
            var pb = p.Typ > 18 ? 6.0 : 5.0;
            d["SumPb"] = "2x (Pb) " + Format(pb);
            d["SumPbTol"] = "+ " + F3(TolH13(pb)) + " [3]";
            d["SumPbTolN"] = "- 0 [3]";
            var g1 = p.Typ < 20 ? 16 : p.Typ < 28 ? 20 : 24;
            d["SumG1"] = "(G1) M" + g1 + "-6H [3]";
            d["SumGd"] = "min 10";
            d["SumG"] = "(G) 1/8 - 27 NPSF";
            var fb = Pick(p, new double[] { 31, 35, 0, 44, 48, 51, 58, 59, 0 });
            d["SumFb"] = fb == 1 ? "Sidoborrhål" : "(Fb) " + Format(fb);
            d["SumFbTol"] = fb == 1 ? "" : "± 0.5";
            var hd = Pick(p, new double[] { 185, 195, 0, 230, 245, 265, 290, 310, 0 });
            d["SumHd"] = "(Hd) " + Format(hd);
            d["SumHdTol"] = "± " + F3(TolHd(hd));
            var bd = Pick(p, new double[] { 17.5, 17.5, 0, 22, 22, 26.5, 26.5, 26.5, 0 });
            d["SumBd"] = "(Bd) " + Format(bd);
            d["SumBdTol"] = "+ " + F3(TolH15Bd(bd)) + " [3]";
            d["SumBdTolN"] = "- 0 [3]";
            d["SumSd"] = "2x (Sd) 9.335";
            d["SumSdTol"] = "+ 0.036 [3]";
            d["SumSdTolN"] = "- 0 [3]";
            d["SumSö"] = "(Sö) 10";
            d["SumSöTol"] = "+ 0 [3]";
            d["SumSöTolN"] = "- 0.5 [3]";
            d["SumSu"] = "(Su) 10.5";
            d["SumSuTol"] = "+ 0 [3]";
            d["SumSuTolN"] = "- 0.5 [3]";
            var uh = Pick(p, new double[] { 100, 112, 0, 125, 140, 150, 150, 160, 0 });
            d["SumUh"] = "(Uh) " + Format(uh);
            d["SumUhTol"] = "± 0.125 [3]";
            var fh = Pick(p, new double[] { 34.5, 34.5, 44.3, 44.3, 44.5, 49.3, 49.3, 59.3, 0 });
            d["SumFh"] = "(Fh) " + Format(fh);
            d["SumFhTol"] = "± 0.500 [3]";
            var vh = Pick(p, new double[] { 59.6, 60.1, 68.6, 68.6, 74.6, 80.6, 86.6, 90.6, 0 });
            d["SumVh"] = "(Vh) " + Format(vh);
            d["SumVhTol"] = "± 0.600 [3]";
            d["SumSs"] = "(Ss) 1.25";
            d["SumSsTol"] = "+ 0.3";
            d["SumSsTolN"] = "- 0.4";
            d["SumPl"] = "0.05";
            d["SumCZ"] = "0.12 CZ";
            d["SumPos"] = "(Pos) " + (p.Typ > 26 ? "50" : p.Typ > 20 ? "40" : "24");
            d["SumSkr"] = "SNL= 8.8, SSNLD= 10.9";
            return d;
        }

        private Dictionary<string, string> GetMachine(APIRequest request)
        {
            var d = new Dictionary<string, string>();
            if (IsLine2(request))
            {
                d["SumMaskinValS1"] = "Maskin: Line 2 - Svarvning";
                d["SumMaskinValS2"] = "Maskin: Line 2 - Borrning, fräsning";
            }
            return d;
        }

        private Dictionary<string, string> GetFrequency(APIRequest request, ProductData p)
        {
            var d = new Dictionary<string, string>();
            if (IsLine2(request))
            {
                d["SumF1_1"] = "1/tim";
                d["SumF1_2"] = "1/tim";
                d["SumF1_3"] = "1/1";
                d["SumF1_4"] = "1/skift & inst.";
                d["SumF1_5"] = "1/skift & inst.";
                d["SumF1_6"] = "Inst.";
                d["SumF1_7"] = "2/skift & inst.";
                d["SumF2_1"] = "2/skift & inst.";
                d["SumF2_2"] = "2/skift & inst.";
                d["SumF2_3"] = p.Fb == 1 ? "" : "Inst./borrbyte";
                d["SumF2_4"] = "Inst.";
                d["SumF2_5"] = "Inst.";
                d["SumF2_6"] = "Inst./borrbyte";
                d["SumF2_7"] = "Inst.";
                d["SumF2_8"] = "Inst.";
                d["SumF2_9"] = "Inst.";
                d["SumF2_10"] = "Inst.";
                d["SumF2_11"] = "Inst.";
                d["SumF2_12"] = "Inst.";
                d["SumF2_13"] = "Inst.";
                d["SumF2_14"] = "2/skift & inst.";
                d["SumF2_15"] = "Inst.";
            }
            return d;
        }

        private Dictionary<string, string> GetTools(APIRequest request, ProductData p)
        {
            var d = new Dictionary<string, string>();
            if (IsLine2(request))
            {
                d["SumD1_1"] = "Skjutmått/Tolk";
                d["SumD1_2"] = "Skjutmått";
                d["SumD1_3"] = "Subito";
                d["SumD1_4"] = "Skjutmått/Tolk";
                d["SumD1_5"] = "Skjutmått/Tolk";
                d["SumD1_6"] = "Skjutmått";
                d["SumD1_7"] = "Skjutmått";
                d["SumD2_1"] = "Gängtolk";
                d["SumD2_2"] = "Gängtolk/skjutmått";
                d["SumD2_3"] = p.Fb == 1 ? "" : "Skjutmått/Okulärt";
                d["SumD2_4"] = "Skjutmått/Okulärt";
                d["SumD2_5"] = "Skjutmått";
                d["SumD2_6"] = "Skjutmått/Tolk";
                d["SumD2_7"] = "Skjutmått";
                d["SumD2_8"] = "Skjutmått";
                d["SumD2_9"] = "Skjutmått";
                d["SumD2_10"] = "Skjutmått";
                d["SumD2_11"] = "Kännbleck";
                d["SumD2_12"] = "Skjutmått";
                d["SumD2_13"] = "Skjutmått";
                d["SumD2_14"] = "Mätmaskin";
                d["SumD2_15"] = "Skjutmått";
            }
            return d;
        }

        private Dictionary<string, string> GetRemarks(APIRequest request, ProductData p)
        {
            var d = new Dictionary<string, string>();
            if (IsLine2(request))
            {
                d["SumAF1_7tb"] = " Enligt TB";
                d["SumAF1_2"] = "(Ud) endast serie 31";
                d["SumAF1_7"] = "Mätes på uh vid hopl.yta ";
                d["SumAF2_13tb"] = " Enligt TB";
                d["SumAF2_1"] = "Samtliga fb-maskiner";
                d["SumAF2_2"] = "Min 10 gängor / 10mm, alla fb-msk";
                d["SumAF2_3"] = p.Fb == 1 ? "Sidoborrhål" : "";
                d["SumAF2_11"] = "Mått: 0.05 ihopsatt hus. ";
                d["SumAF2_13"] = "Centrum till undersida text";
                d["SumAF2_14"] = "Från fb-maskin 1, 2 & 3";
            }
            return d;
        }

        private Dictionary<string, string> GetOtherText()
        {
            return new Dictionary<string, string>
            {
                {"SumTextS1", "Kontrolleras enl. styrplan,Vid misstanke om fel kontrollera med annan mätutrustning mätmaskin 20 grader, Kontrollera bakåt vid misstanke om felaktiga hus. "},
                {"SumTextS2", "Kontrolleras enl. styrplan,Vid misstanke om fel kontrollera med annan mätutrustning mätmaskin 20 grader, Kontrollera bakåt vid misstanke om felaktiga hus. <<LineBreak>>För tillägg V/VU se intruktion under fliken Specialhusgruppen/Okulär kontroll enligt inst."}
            };
        }

        private ProductData ParseProduct(APIRequest request)
        {
            var bet = Normalize(request.ProductDesignation);
            var clean = Regex.Replace(bet, @"\s*\.\s*0?2\s*$", "");
            var tokens = Regex.Split(clean, @"[\s/\.\-]+")
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToArray();
            var number = tokens.FirstOrDefault(x => Regex.IsMatch(x, @"^\d{3,4}$"));
            if (string.IsNullOrEmpty(number))
            {
                var match = Regex.Match(clean, @"(\d{3,4})(?!.*\d)");
                number = match.Success ? match.Groups[1].Value : "218";
            }
            var typ = number.Length >= 2 ? ParseInt(number.Substring(number.Length - 2), 18) : 18;
            var serie = number.Length > 2 ? ParseInt(number.Substring(0, number.Length - 2), 0) : 0;
            var types = new[] { 18, 19, 20, 22, 24, 26, 28, 30, 32 };
            var index = Array.IndexOf(types, typ);
            if (index < 0) index = 0;
            var product = new ProductData { Serie = serie, Typ = typ, Index = index };
            product.Fb = Pick(product, new double[] { 31, 35, 0, 44, 48, 51, 58, 59, 0 });
            return product;
        }

        private double Pick(ProductData p, double[] values)
        {
            return values[Math.Max(0, Math.Min(p.Index, values.Length - 1))];
        }

        private int ParseInt(string value, int fallback)
        {
            return int.TryParse(value, out var parsed) ? parsed : fallback;
        }

        private double TolH12(double value)
        {
            return value < 120.01 ? 0.350 : value < 180.01 ? 0.400 : value < 250.01 ? 0.460 : 0.520;
        }

        private double TolH12Width(double value)
        {
            return value < 50.01 ? 0.250 : value < 80.01 ? 0.300 : value < 120.01 ? 0.350 : 0.400;
        }

        private double TolH13(double value)
        {
            return value < 3.01 ? 0.140 : value < 6.01 ? 0.180 : value < 10.01 ? 0.220 : value < 18.01 ? 0.270 : value < 30.01 ? 0.330 : value < 50.01 ? 0.390 : value < 80.01 ? 0.460 : value < 120.01 ? 0.540 : value < 180.01 ? 0.630 : value < 250.01 ? 0.720 : value < 315.01 ? 0.810 : value < 400.01 ? 0.890 : value < 500.01 ? 0.970 : value < 630.01 ? 1.100 : value < 800.01 ? 1.250 : value < 1000.01 ? 1.400 : value < 1250.01 ? 1.650 : value < 1600.01 ? 1.950 : value < 2000.01 ? 2.300 : value < 2500.01 ? 2.800 : 3.300;
        }

        private double TolG7Upper(double value)
        {
            return value < 120.01 ? 0.047 : value < 180.01 ? 0.054 : value < 250.01 ? 0.061 : value < 315.01 ? 0.069 : 0.075;
        }

        private double TolG7Lower(double value)
        {
            return value < 120.01 ? 0.012 : value < 180.01 ? 0.014 : value < 250.01 ? 0.015 : value < 315.01 ? 0.017 : 0.018;
        }

        private double TolHd(double value)
        {
            return value < 6.01 ? 0.1 : value < 30.01 ? 0.2 : value < 120.01 ? 0.3 : value < 400.01 ? 0.5 : value < 1000.01 ? 0.8 : value < 2000.01 ? 1.2 : 2.0;
        }

        private double TolH15Bd(double value)
        {
            return value < 10.01 ? 0.580 : value < 19.01 ? 0.700 : value < 30.01 ? 0.840 : 1.000;
        }

        private bool IsLine2(APIRequest request)
        {
            return string.Equals(request.MachineNumber ?? "", "Line 2", StringComparison.OrdinalIgnoreCase);
        }

        private string Normalize(string input)
        {
            return input == null ? "" : input.Trim().ToUpperInvariant();
        }

        private string Format(double value)
        {
            return Math.Abs(value % 1) < 0.000001 ? ((int)value).ToString(CultureInfo.InvariantCulture) : value.ToString("0.###", CultureInfo.InvariantCulture);
        }

        private string F3(double value)
        {
            return value.ToString("0.000", CultureInfo.InvariantCulture);
        }

        private void Merge(Dictionary<string, string> target, Dictionary<string, string> source)
        {
            foreach (var kv in source)
                target[kv.Key] = kv.Value ?? "";
        }

        private class ProductData
        {
            public int Serie { get; set; }
            public int Typ { get; set; }
            public int Index { get; set; }
            public double Fb { get; set; }
        }
    }
}
