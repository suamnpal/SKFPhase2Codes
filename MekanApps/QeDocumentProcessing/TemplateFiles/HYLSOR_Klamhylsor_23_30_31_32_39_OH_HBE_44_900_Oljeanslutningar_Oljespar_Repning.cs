using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Klamhylsor_23_30_31_32_39_OH_HBE_44_900_Oljeanslutningar_Oljespar_Repning : ITemplateCalculations
    {
        private static readonly Dictionary<string, string[]> Types = new Dictionary<string, string[]>
        {
            ["30"] = new[] { "44", "48", "52", "56", "60", "64", "68", "72", "76", "80", "84", "88", "92", "96", "500", "530", "560", "600", "630", "670", "710", "750", "800", "850", "900" },
            ["31"] = new[] { "44", "48", "52", "56", "60", "64", "68", "72", "76", "80", "84", "88", "92", "96", "500", "530", "560", "600", "630", "670", "710", "750", "800", "850", "900", "1000" },
            ["23"] = new[] { "44", "48", "52", "56" },
            ["32"] = new[] { "60", "64", "68", "72", "76", "80", "84", "88", "92", "96", "500", "530", "560", "600", "630", "670", "710", "750", "800", "850" },
            ["39"] = new[] { "44", "48", "52", "56", "60", "64", "68", "72", "76", "80", "84", "88", "92", "96", "500", "530", "560", "600", "630", "670", "710", "750", "800", "850", "900", "863,6" }
        };

        private static readonly Dictionary<string, string[]> BValues = new Dictionary<string, string[]>
        {
            ["30"] = new[] { "3,9", "3,9", "3,9", "3,9", "3,9", "3,5", "3,5", "3,5", "3,5", "3,5", "3,5", "6,5", "6,5", "6,5", "6,5", "6", "6", "8", "6", "8", "8", "8", "10", "10", "10" },
            ["31"] = new[] { "3,9", "3,9", "3,9", "3,9", "3,9", "3,5", "3,5", "3,5", "3,5", "3,5", "3,5", "6,5", "6,5", "6,5", "6,5", "6", "6", "8", "6", "8", "8", "8", "10", "10", "10", "3,9" },
            ["23"] = new[] { "3,9", "3,9", "3,9", "3,9" },
            ["32"] = new[] { "3,9", "3,5", "3,5", "3,5", "3,5", "3,5", "3,5", "6,5", "6,5", "6,5", "6", "6", "6", "6", "8", "6", "8", "8", "8", "10" },
            ["39"] = new[] { "3,9", "3,9", "3,9", "3,9", "3,9", "3,5", "3,5", "3,5", "3,5", "3,5", "3,5", "6,5", "6,5", "6,5", "6,5", "6", "6", "8", "6", "8", "8", "8", "10", "10", "10", "8" }
        };

        private static readonly Dictionary<string, string[]> EValues = new Dictionary<string, string[]>
        {
            ["30"] = Split("74:79:84:89:98:100:109:109:113:122:123:135:138:139:147:155,5:169:171:176,5:189,5:202:208,5:212:218,5:232"),
            ["31"] = Split("92:98:107:110:115:124:144:148:151:156:175:176:187:191:203:206,5:217:226:243:263:267,5:281,5:287:304:316,5:92"),
            ["23"] = Split("104:110:117:123"),
            ["32"] = Split("130:139:160:166:172:181:196:200:212:219:235:244:255,5:265,5:286,5:309:314,5:332:337,5:356"),
            ["39"] = Split("60:64:71:75:86:86:89:89:99:103:103:117:117:122:130:134:143:147,5:154,5:161,5:176:178,5:183:185:197,5:197,5")
        };

        private static readonly Dictionary<string, string[]> JValues = new Dictionary<string, string[]>
        {
            ["30"] = Split("70,5:75,5:81:85,5:95:96,5:105:105,5:109:118,5:119,5:130,5:133,5:134,5:143:151,5:163:165:170,5:183,5:196:202,5:206:212,5:226"),
            ["31"] = Split("89:94,5:104:106,5:112:121:140,5:144,5:147,5:152:171:171,5:183:186,5:199:202,5:211:220:237:257:261,5:276,5:281:298:310,5:89"),
            ["23"] = Split("100,5:107:113,5:120"),
            ["32"] = Split("126,5:135,5:156:162,5:168:177:192,5:196:208:214,5:231:240:249,5:259,5:280,5:303:308,5:326:331,5:350"),
            ["39"] = Split("57:61:68:72:82,5:82,5:85,5:85,5:95,5:99,5:99,5:113:113:117,5:125,5:129:138:142,5:149,5:156,5:171:173,5:178:180:192,5:192,5")
        };

        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var result = new Dictionary<string, string>();
            var designation = (request?.ProductDesignation ?? string.Empty).Trim().ToUpperInvariant().Replace('.', ',');
            var product = ParseDesignation(designation);
            Merge(result, GetMachine(request));
            Merge(result, GetDimensions(product, designation));
            Merge(result, GetFrequencies(request));
            Merge(result, GetMeasurementTools(request));
            Merge(result, GetRemarks());
            Merge(result, GetMisc(product, designation));
            return result;
        }

        private Dictionary<string, string> GetMachine(APIRequest request)
        {
            var machine = NormalizeMachine(request?.MachineNumber);
            return new Dictionary<string, string>
            {
                ["SumMaskinValS1"] = "Maskin: " + machine + " - BorrOljehål & Oljespår",
                ["SumMaskinValS2"] = "Maskin: " + machine + " - Oljespår & Repor"
            };
        }

        private Dictionary<string, string> GetDimensions(ProductInfo product, string designation)
        {
            var result = new Dictionary<string, string>();
            var typeNumber = ToDouble(product.Type);
            var b = GetKordaBValue(product, designation);
            var e = GetTableValue(EValues, product.Series, product.Type);
            var j = GetTableValue(JValues, product.Series, product.Type);
            var g = typeNumber < 85 || NearlyEqual(typeNumber, 863.6) ? "M6" : typeNumber < 561 || NearlyEqual(typeNumber, 630) ? "M8" : "G1/8";
            var c = g == "M6" ? "9" : g == "M8" ? "12" : "13";
            var t = g == "M6" ? "6,3" : g == "M8" ? "8,3" : "10";
            var d = g == "M6" ? "3" : g == "M8" ? "4" : "5";
            var h = product.Series == "30"
                ? typeNumber < 37 ? "0,8" : typeNumber < 65 ? "1" : typeNumber < 85 ? "1,2" : typeNumber < 751 ? "1,5" : "2"
                : typeNumber < 37 ? "0,8" : typeNumber < 65 ? "1" : typeNumber < 85 ? "1,2" : typeNumber < 601 ? "1,5" : typeNumber < 751 ? "2" : "2,8";
            var f = typeNumber < 85 ? "2" : "3";
            var n = typeNumber < 37 ? "4" : typeNumber < 65 ? "5" : typeNumber < 85 ? "6" : typeNumber < 601 ? "7" : typeNumber < 751 ? "8" : "9";
            var r1 = typeNumber < 37 ? "3" : typeNumber < 65 ? "4" : typeNumber < 85 ? "4,5" : "5";
            var r = typeNumber < 85 ? "1" : "2,5";

            result["SumRitNr"] = GetDrawing(product.Series, typeNumber);
            result["SumG"] = g;
            result["SumB"] = Value("B", b);
            result["SumBTol"] = b == "4" ? "- 0.1" : "   0";
            result["SumBTolN"] = b == "4" ? "- 0.3" : "- 0.1";
            result["SumC"] = Value("C", c);
            result["SumCTol"] = Tolerance(c);
            result["SumT"] = Value("T", t);
            result["SumTTol"] = Tolerance(t);
            result["SumD"] = Value("D", d);
            result["SumDTol"] = Tolerance(d);
            result["SumH"] = Value("H", h);
            result["SumHTol"] = ToDouble(h) < 6.1 ? "± 0.1" : "± 0.2";
            result["SumE"] = Value("E", e);
            result["SumETol"] = Tolerance(e);
            result["SumJ"] = Value("J", j);
            result["SumJTol"] = Tolerance(j);
            result["SumF"] = Value("F", f);
            result["SumF2"] = result["SumF"];
            result["SumFTol"] = ToDouble(h) < 6.1 ? "± 0.1" : "± 0.2";
            result["SumF2Tol"] = result["SumFTol"];
            result["SumN"] = Value("N", n);
            result["SumNTol"] = ToDouble(h) < 6.1 ? "± 0.1" : "± 0.2";
            result["SumR1"] = "R" + r1;
            result["SumR1a"] = "R" + r1;
            result["SumR"] = "R" + r;
            result["SumV120"] = "120º";
            result["SumV45"] = "45º";
            result["SumV30"] = "~30º";
            return result;
        }

        private Dictionary<string, string> GetFrequencies(APIRequest request)
        {
            var machine = NormalizeMachine(request?.MachineNumber);
            var value = machine == "Skepp6" ? "1/1" : IsProductionMachine(machine) ? "1/2" : string.Empty;
            return Indexed("SumF1_", value);
        }

        private Dictionary<string, string> GetMeasurementTools(APIRequest request)
        {
            var machine = NormalizeMachine(request?.MachineNumber);
            var valid = machine == "Skepp6" || IsProductionMachine(machine);
            return new Dictionary<string, string>
            {
                ["SumD1_1"] = machine == "Skepp6" ? "Skala på borrmaskin" : IsProductionMachine(machine) ? "pipborr/djupmått" : "",
                ["SumD1_2"] = valid ? "Skjutmått" : "",
                ["SumD1_3"] = valid ? "Skjutmått" : "",
                ["SumD1_4"] = valid ? "Gängtolk" : "",
                ["SumD1_5"] = valid ? "Skjutmått" : "",
                ["SumD1_6"] = valid ? "Skjutmått/fasmall" : "",
                ["SumD1_7"] = valid ? "Skjutmått" : "",
                ["SumD1_8"] = valid ? "Skjutmått" : "",
                ["SumD1_9"] = machine == "Skepp6" ? "Höjdrits" : IsProductionMachine(machine) ? "pipborr/djupmått" : "",
                ["SumD1_0"] = valid ? "Skjutmått" : "",
                ["SumD1_11"] = valid ? "Radieyra" : ""
            };
        }

        private Dictionary<string, string> GetRemarks()
        {
            return Indexed("SumAF1_", string.Empty);
        }

        private Dictionary<string, string> GetMisc(ProductInfo product, string designation)
        {
            return new Dictionary<string, string>
            {
                ["SumTextS1"] = "",
                ["SumTextS2"] = "Grader, frifläckar, slagmärken, repor, valkar och andra defekter samt ojämnheter kontrolleras med ögonmått.",
                ["SumTextGängstigning"] = "Max 2x gängstigning",
                ["SumÖvrigt"] = "2st. oljeborrhål, Korda (A) = " + CalculateKorda(product, designation),
                ["SumORM"] = "Stämpla Oljeriktningsmarkeringar"
            };
        }

        private static ProductInfo ParseDesignation(string designation)
        {
            var normalized = Regex.Replace(
                (designation ?? string.Empty).Trim().ToUpperInvariant().Replace('.', ','),
                @"\s+",
                " ");

            // Separated formats supported:
            // OH 30/530 HB, OH-30-600-HB-E-V32, OH 31 600 HB.
            var separated = Regex.Match(
                normalized,
                @"\bOH[\s-]*(?<series>23|30|31|32|39)[\s/-]+(?<type>\d+(?:,\d+)?)(?=\s|-|$)");

            if (separated.Success)
            {
                return new ProductInfo(
                    separated.Groups["series"].Value,
                    separated.Groups["type"].Value);
            }

            // Compact formats supported:
            // OH 3064 HB, OH 3176 HB, OH 3964 HB.
            var compact = Regex.Match(
                normalized,
                @"\bOH[\s-]*(?<series>23|30|31|32|39)(?<type>\d{2,3})(?=\s|-|$)");

            return compact.Success
                ? new ProductInfo(
                    compact.Groups["series"].Value,
                    compact.Groups["type"].Value)
                : new ProductInfo(string.Empty, string.Empty);
        }

        private static string CalculateKorda(ProductInfo product, string designation)
        {
            var type = ToDouble(product.Type);
            if (type <= 0) return string.Empty;

            var bText = GetKordaBValue(product, designation);
            if (string.IsNullOrEmpty(bText)) return string.Empty;
            var b = ToDouble(bText);

            var isCompact = IsCompactDesignation(designation, product);

            // Lotus Notes rule:
            // compact OH 3064 HB -> physical diameter = 64 / 2 * 10 = 320
            // separated OH 30/600 HB -> physical diameter = 600
            var diameter = isCompact ? type / 2.0 * 10.0 : type;

            var d1 = type < 85
                ? diameter - 20
                : type < 561 || NearlyEqual(type, 630)
                    ? diameter - 30
                    : type < 751
                        ? diameter - 40
                        : type < 1001
                            ? diameter - 50
                            : diameter - 60;

            var constant = Math.Round(
                Math.Sin(67.5 * Math.PI / 180.0) * 2,
                4,
                MidpointRounding.AwayFromZero);

            var korda = Math.Round(
                ((d1 + b * 2) / 2) * constant,
                1,
                MidpointRounding.AwayFromZero);

            return korda.ToString("0.#", new CultureInfo("sv-SE"));
        }

        private static string GetKordaBValue(ProductInfo product, string designation)
        {
            // Special Lotus Notes V32 rule. This must be applied in both the
            // displayed B dimension and the Korda calculation.
            if (product.Series == "30" &&
                product.Type == "600" &&
                ContainsToken(designation, "V32"))
            {
                return "11";
            }

            var tableValue = GetTableValue(BValues, product.Series, product.Type);
            if (!string.IsNullOrEmpty(tableValue)) return tableValue;


            return string.Empty;
        }

        private static bool IsCompactDesignation(string designation, ProductInfo product)
        {
            var normalized = Regex.Replace(
                (designation ?? string.Empty).Trim().ToUpperInvariant().Replace('.', ','),
                @"\s+",
                " ");

            var pattern = @"\bOH[\s-]*" +
                          Regex.Escape(product.Series) +
                          Regex.Escape(product.Type) +
                          @"(?=\s|-|$)";

            return Regex.IsMatch(normalized, pattern);
        }

        private static bool ContainsToken(string designation, string token)
        {
            return Regex.IsMatch(
                designation ?? string.Empty,
                @"(?:^|[\s/\-()])" + Regex.Escape(token) + @"(?:$|[\s/\-()])",
                RegexOptions.IgnoreCase);
        }
        private static string GetDrawing(string series, double type)
        {
            if (series == "30") return type < 44 ? "7434155" : "7434156";
            if (series == "31") return type < 44 ? "7434157" : "7434158";
            if (series == "23" || series == "32") return type < 44 ? "7434159" : "7434160";
            if (series == "39") return "7434168";
            return "Ingen ritning hittad";
        }

        private static string GetTableValue(Dictionary<string, string[]> table, string series, string type)
        {
            if (!Types.TryGetValue(series, out var types) || !table.TryGetValue(series, out var values)) return string.Empty;
            var index = Array.FindIndex(types, x => string.Equals(x, type, StringComparison.OrdinalIgnoreCase));
            return index >= 0 && index < values.Length ? values[index] : string.Empty;
        }

        private static string Tolerance(string value)
        {
            var number = ToDouble(value);
            if (string.IsNullOrEmpty(value)) return string.Empty;
            if (number < 6.1) return "± 0.1";
            if (number < 30.1) return "± 0.2";
            if (number < 120.1) return "± 0.3";
            if (number < 315.1) return "± 0.5";
            if (number < 1000.1) return "± 0.8";
            if (number < 2000.1) return "± 1.2";
            return "± 2.0";
        }

        private static Dictionary<string, string> Indexed(string prefix, string value)
        {
            return new Dictionary<string, string>
            {
                [prefix + "1"] = value,
                [prefix + "2"] = value,
                [prefix + "3"] = value,
                [prefix + "4"] = value,
                [prefix + "5"] = value,
                [prefix + "6"] = value,
                [prefix + "7"] = value,
                [prefix + "8"] = value,
                [prefix + "9"] = value,
                [prefix + "0"] = value,
                [prefix + "11"] = value
            };
        }

        private static bool IsProductionMachine(string machine)
        {
            return machine == "K&T" || machine == "VTR-160" || machine == "MacTurn 550" || machine == "Dubbelparet";
        }

        private static string NormalizeMachine(string machine)
        {
            var value = (machine ?? string.Empty).Trim();
            var allowed = new[] { "Skepp6", "K&T", "VTR-160", "MacTurn 550", "Dubbelparet" };
            return allowed.FirstOrDefault(x => string.Equals(x, value, StringComparison.OrdinalIgnoreCase)) ?? string.Empty;
        }

        private static string Value(string name, string value)
        {
            return string.IsNullOrEmpty(value) ? string.Empty : "(" + name + ") " + value;
        }

        private static double ToDouble(string value)
        {
            double.TryParse((value ?? string.Empty).Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var number);
            return number;
        }

        private static bool NearlyEqual(double left, double right)
        {
            return Math.Abs(left - right) < 0.0001;
        }

        private static string[] Split(string value)
        {
            return value.Split(':');
        }

        private static void Merge(Dictionary<string, string> target, Dictionary<string, string> source)
        {
            foreach (var item in source)
                target[item.Key] = item.Value ?? string.Empty;
        }

        private sealed class ProductInfo
        {
            public ProductInfo(string series, string type)
            {
                Series = series;
                Type = type;
            }

            public string Series { get; }
            public string Type { get; }
        }
    }
}
