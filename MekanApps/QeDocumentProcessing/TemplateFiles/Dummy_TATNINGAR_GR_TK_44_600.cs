using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class Dummy_TATNINGAR_GR_TK_44_600 : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var keyValues = new Dictionary<string, string>();

            keyValues["DocumentUniqueId"] =
                DateTime.UtcNow.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);

            string productDesignation = (request.ProductDesignation ?? "")
                .Trim().ToUpperInvariant();

            var parts = productDesignation
                .Replace(".", ",")
                .Split(new[] { ' ', '/', '.' }, StringSplitOptions.RemoveEmptyEntries);

            int typeCode = parts.Length > 1 && int.TryParse(parts[1], out int t) ? t : 0;
            int normalizedType = typeCode == 30 ? 500 : typeCode;

            bool isSupportedMachine = IsMachineMatch(request.MachineNumber);

            keyValues["SumMaskinValS1"] =
                $"Maskin: {request.MachineNumber} - Diametrala mått, Form & läge.";
            keyValues["SumMaskinValS2"] =
                $"Maskin: {request.MachineNumber} - Övriga mått.";
            keyValues["SumMaskinvalS1"] = keyValues["SumMaskinValS1"];
            keyValues["SumMaskinvalS2"] = keyValues["SumMaskinValS2"];

            Merge(keyValues, CalculateDiameters(normalizedType));
            Merge(keyValues, CalculateWidths(normalizedType));
            Merge(keyValues, CalculateLength(normalizedType));
            Merge(keyValues, CalculateThreads());
            Merge(keyValues, CalculateChamfersAndRadii(normalizedType));
            Merge(keyValues, CalculatePage1Measurements(isSupportedMachine, normalizedType));
            Merge(keyValues, CalculatePage2Measurements(isSupportedMachine));
            Merge(keyValues, CalculateAdminFields());

            return keyValues;
        }

        /* ---------------- HELPERS ---------------- */

        private static void Merge(Dictionary<string, string> t, Dictionary<string, string> s)
        {
            foreach (var e in s) t[e.Key] = e.Value;
        }

        private static bool IsMachineMatch(string m)
        {
            if (string.IsNullOrWhiteSpace(m)) return false;
            m = m.ToUpperInvariant();
            return m.Contains("NAKAMURA") || m.Contains("LB45") || m.Contains("LT-3000EX");
        }

        private static string FormatNumber(double v)
        {
            if (Math.Abs(v % 1) < 0.000001) return v.ToString("0", CultureInfo.InvariantCulture);
            if (Math.Abs((v * 10) % 1) < 0.000001) return v.ToString("0.0", CultureInfo.InvariantCulture);
            return v.ToString("0.000", CultureInfo.InvariantCulture);
        }

        /* ---------------- DIAMETERS ---------------- */

        private Dictionary<string, string> CalculateDiameters(int typeCode)
        {
            var keyValues = new Dictionary<string, string>();

            int normalizedType = typeCode == 30 ? 500 : typeCode;

            // ============================================================
            // DIAMETER VALUE TABLE (UNCHANGED LOGIC)
            // ============================================================
            var diameterTables = new Dictionary<int, double[]>
            {
                { 44,  new[] { 204, 220, 236.2, 232.8, 234.8, 256, 264, 280, 288, 304, 312 } },
                { 48,  new[] { 224, 240, 256.2, 252.8, 254.8, 276, 284, 300, 308, 324, 332 } },
                { 52,  new[] { 244, 260.2, 276.4, 272.6, 274.6, 299, 307, 325, 333, 351, 359 } },
                { 56,  new[] { 264, 280.2, 296.4, 292.8, 294.8, 319, 327, 345, 353, 371, 379 } },
                { 60,  new[] { 284, 300.2, 316.6, 312.8, 314.8, 339, 347, 365, 373, 391, 399 } },
                { 64,  new[] { 304, 320.4, 336.6, 332.8, 334.8, 359, 367, 385, 393, 410, 419 } },
                { 68,  new[] { 326, 341, 357.2, 353.2, 355.2, 377, 385, 403, 411, 429, 437 } },
                { 72,  new[] { 346, 361, 377.2, 373.2, 375.2, 396, 404, 424, 432, 452, 460 } },
                { 76,  new[] { 366, 381, 397.2, 393.2, 395.2, 416, 424, 444, 452, 472, 480 } },
                { 80,  new[] { 386, 401.4, 417.6, 413.5, 415.5, 436, 444, 464, 472, 492, 500 } },
                { 84,  new[] { 406, 421.4, 437.6, 433.5, 435.5, 463, 471, 491, 499, 519, 527 } },
                { 88,  new[] { 416, 441.4, 457.6, 453.5, 455.5, 473, 481, 501, 509, 529, 537 } },
                { 92,  new[] { 436, 461.6, 477.8, 473.6, 475.6, 491, 499, 519, 527, 547, 555 } },
                { 96,  new[] { 456, 481.6, 497.8, 493.6, 495.6, 517, 525, 545, 553, 573, 581 } },
                { 500, new[] { 476, 501.6, 517.8, 513.6, 515.6, 531, 539, 559, 567, 587, 595 } },
                { 530, new[] { 504, 531.6, 547.8, 543.6, 545.6, 561, 569, 589, 597, 617, 625 } },
                { 560, new[] { 534, 561.6, 577.8, 573.6, 575.6, 591, 599, 619, 627, 647, 655 } },
                { 600, new[] { 564, 601.6, 617.8, 613.6, 615.6, 621, 629, 649, 657, 677, 685 } }
            };

            var diameterToleranceTables = new Dictionary<int, string[]>
    {
        {
            44,
            new[]
            {
                "+ 0.520","- 0.0",
                "+ 0.0","- 0.520",
                "+ 0.520","- 0.0",
                "+ 0.0","- 0.520",
                "+ 0.520","- 0.0",
                "+ 0.0","- 0.520"
            }
        },

        {
            48,
            new[]
            {
                "+ 0.520","- 0.0",
                "+ 0.0","- 0.520",
                "+ 0.520","- 0.0",
                "+ 0.0","- 0.520",
                "+ 0.570","- 0.0",
                "+ 0.0","- 0.570"
            }
        },

        {
            52,
            new[]
            {
                "+ 0.520","- 0.0",
                "+ 0.0","- 0.520",
                "+ 0.570","- 0.0",
                "+ 0.0","- 0.570",
                "+ 0.570","- 0.0",
                "+ 0.0","- 0.570"
            }
        },

        {
            56,
            new[]
            {
                "+ 0.570","- 0.0",
                "+ 0.0","- 0.570",
                "+ 0.570","- 0.0",
                "+ 0.0","- 0.570",
                "+ 0.570","- 0.0",
                "+ 0.0","- 0.570"
            }
        },

        {
            60,
            new[]
            {
                "+ 0.570","- 0.0",
                "+ 0.0","- 0.570",
                "+ 0.570","- 0.0",
                "+ 0.0","- 0.570",
                "+ 0.570","- 0.0",
                "+ 0.0","- 0.570"
            }
        },

        {
            64,
            new[]
            {
                "+ 0.570","- 0.0",
                "+ 0.0","- 0.570",
                "+ 0.570","- 0.0",
                "+ 0.0","- 0.570",
                "+ 0.630","- 0.0",
                "+ 0.0","- 0.630"
            }
        },

        {
            68,
            new[]
            {
                "+ 0.570","- 0.0",
                "+ 0.0","- 0.570",
                "+ 0.630","- 0.0",
                "+ 0.0","- 0.630",
                "+ 0.630","- 0.0",
                "+ 0.0","- 0.630"
            }
        },

        {
            72,
            new[]
            {
                "+ 0.570", "- 0.0",
                "+ 0.0",   "- 0.630",
                "+ 0.630", "- 0.0",
                "+ 0.0",   "- 0.630",
                "+ 0.630", "- 0.0",
                "+ 0.0",   "- 0.630"
            }
        },

        {
            76,
            new[]
            {
                "+ 0.630", "- 0.0",
                "+ 0.0",   "- 0.630",
                "+ 0.630", "- 0.0",
                "+ 0.0",   "- 0.630",
                "+ 0.630", "- 0.0",
                "+ 0.0",   "- 0.630"
            }
        },

        {
            80,
            new[]
            {
                "+ 0.630", "- 0.0",
                "+ 0.0",   "- 0.630",
                "+ 0.630", "- 0.0",
                "+ 0.0",   "- 0.630",
                "+ 0.630", "- 0.0",
                "+ 0.0",   "- 0.630"
            }
        },
        {
            84,
            new[]
            {
                "+ 0.630", "- 0.0",
                "+ 0.0",   "- 0.630",
                "+ 0.630", "- 0.0",
                "+ 0.0",   "- 0.630",
                "+ 0.700", "- 0.0",
                "+ 0.0",   "- 0.700"
            }
        },
        {
            88,
            new[]
            {
                "+ 0.630", "- 0.0",
                "+ 0.0",   "- 0.630",
                "+ 0.700", "- 0.0",
                "+ 0.0",   "- 0.700",
                "+ 0.700", "- 0.0",
                "+ 0.0",   "- 0.700"
            }
        },
        {
            92,
            new[]
            {
                "+ 0.630", "- 0.0",
                "+ 0.0",   "- 0.630",
                "+ 0.700", "- 0.0",
                "+ 0.0",   "- 0.700",
                "+ 0.700", "- 0.0",
                "+ 0.0",   "- 0.700"
            }
        },
        {
            96,
            new[]
            {
                "+ 0.700", "- 0.0",
                "+ 0.0",   "- 0.700",
                "+ 0.700", "- 0.0",
                "+ 0.0",   "- 0.700",
                "+ 0.700", "- 0.0",
                "+ 0.0",   "- 0.700"
            }
        },

        {
                -1,
            new[]
            {
                "+ 0.700","- 0.0",
                "+ 0.0","- 0.700",
                "+ 0.700","- 0.0",
                "+ 0.0","- 0.700",
                "+ 0.700","- 0.0",
                "+ 0.0","- 0.700"
            }
        }
    };

            if (!diameterTables.TryGetValue(normalizedType, out var d))
                d = new double[11];

            var tol = diameterToleranceTables.ContainsKey(normalizedType)
                ? diameterToleranceTables[normalizedType]
                : diameterToleranceTables[-1];

            keyValues["Sumd"] = "(d) " + FormatNumber(d[0]);
            keyValues["SumdTol"] = "+ 0.400";
            keyValues["SumdTolN"] = "- 0.0";

            keyValues["Sumd1"] = "2x (d1) " + FormatNumber(d[1]);
            keyValues["Sumd1Tol"] = "+ 0.0";
            keyValues["Sumd1TolN"] = "- 0.300";

            keyValues["Sumd2"] = "(d2) " + FormatNumber(d[2]);
            keyValues["Sumd2Tol"] = "+ 0.0";
            keyValues["Sumd2TolN"] = "- 0.300";

            keyValues["Sumd3"] = "(d3) " + FormatNumber(d[3]);
            keyValues["Sumd3Tol"] = "± 0.100";

            keyValues["Sumd4"] = "2x (d4) " + FormatNumber(d[4]);
            keyValues["Sumd4Tol"] = "+ 0.0";
            keyValues["Sumd4TolN"] = "- 0.300";

            keyValues["Sumd7"] = "(d7) " + FormatNumber(d[5]);
            keyValues["Sumd7Tol"] = tol[0];
            keyValues["Sumd7TolN"] = tol[1];

            keyValues["Sumd8"] = "(d8) " + FormatNumber(d[6]);
            keyValues["Sumd8Tol"] = tol[2];
            keyValues["Sumd8TolN"] = tol[3];

            keyValues["Sumd9"] = "(d9) " + FormatNumber(d[7]);
            keyValues["Sumd9Tol"] = tol[4];
            keyValues["Sumd9TolN"] = tol[5];

            keyValues["Sumd10"] = "(d10) " + FormatNumber(d[8]);
            keyValues["Sumd10Tol"] = tol[6];
            keyValues["Sumd10TolN"] = tol[7];

            keyValues["Sumd11"] = "(d11) " + FormatNumber(d[9]);
            keyValues["Sumd11Tol"] = tol[8];
            keyValues["Sumd11TolN"] = tol[9];

            keyValues["Sumd12"] = "(d12) " + FormatNumber(d[10]);
            keyValues["Sumd12Tol"] = tol[10];
            keyValues["Sumd12TolN"] = tol[11];

            return keyValues;
        }


        /* ---------------- WIDTHS ---------------- */

        private Dictionary<string, string> CalculateWidths(int type)
        {
            return new Dictionary<string, string>
            {
                ["SumS1"] = "(S1) 1.0",
                ["SumS1Tol"] = "+ 0.500",
                ["SumS1TolN"] = "+ 0.250",
                ["Sumd14"] = "Ø 8",
                ["Sumd14Tol"] = "+ 0.100",
                ["Sumd14TolN"] = "- 0.0",
                ["Sumd15"] = "Ø 3",
                ["Sumd15Tol"] = "± 0.100",

                ["SumH"] = "(H) " + (type == 48 ? "38" : "36"),
                ["SumHTol"] = "+ 0.500",
                ["SumHTolN"] = "- 0.0",

                ["SumB"] = "(B) " +
                    (type == 48 ? "60" :
                     type < 68 ? "58" :
                     type < 76 ? "61.5" :
                     type < 92 ? "62.5" : "64"),
                ["SumBTol"] = "+ 0.500",
                ["SumBTolN"] = "- 0.0",

                ["Sumb1"] = "(b1) " + (type < 68 ? "7.5" : type < 92 ? "10.5" : "12"),
                ["Sumb1Tol"] = "± 0.200",

                ["Sumb2"] = (type < 68 ? "1x " : "2x ") + "(b2) " + (type < 68 ? "16" : type < 92 ? "19" : "20.5"),
                ["Sumb2Tol"] = "+ 0.0",
                ["Sumb2TolN"] = "- 0.500",

                ["Sumb3"] = "(b3) 22",
                ["Sumb3Tol"] = "+ 0.840",
                ["Sumb3TolN"] = "- 0.0",
                ["Sumb4"] = "(b4) " + (type < 68 ? "16" : type < 92 ? "21" : "22.5"),
                ["Sumb4Tol"] = "+ 0.200",
                ["Sumb4TolN"] = "- 0.200",

                ["Sumb5"] = "2x (b5) " + (type < 68 ? "10.5" : type < 92 ? "13.5" : "15"),
                ["Sumb5Tol"] = "+ 0.0",
                ["Sumb5TolN"] = "- 0.500",

                ["Sumb6"] = "(b6) 6.1",
                ["Sumb6Tol"] = "+ 0.200",
                ["Sumb6TolN"] = "- 0.0",
                ["Sumb7"] = "(b7) 10",
                ["Sumb7Tol"] = "+ 0.0",
                ["Sumb7TolN"] = "- 0.500",
                ["Sumb8"] = "(b8) 6",
                ["Sumb8Tol"] = "+ 0.400",
                ["Sumb8TolN"] = "- 0.0"
            };
        }

        /* ---------------- LENGTH ---------------- */

        private Dictionary<string, string> CalculateLength(int type)
        {
            int L = ((type >= 44 && type <= 68) || type == 500) ? 32 : 34;
            return new Dictionary<string, string>
            {
                ["SumL"] = "(L) " + L,
                ["SumLTol"] = "+ 0.500",
                ["SumLTolN"] = "- 0.0"
            };
        }

        /* ---------------- THREADS, CHAMFERS, PAGES, ADMIN ---------------- */

        private Dictionary<string, string> CalculateThreads() => new()
        {
            ["SumG"] = "(G) M6",
            ["SumGa"] = "(G) M6",
            ["SumG1"] = "(G1) 15",
            ["SumG1Tol"] = "+ 0.500",
            ["SumG1TolN"] = "- 0.0",
            ["SumG2"] = "min.23",
            ["SumG3"] = "max.28"
        };

        private static Dictionary<string, string> CalculateChamfersAndRadii(int typ) => new()
        {
            ["SumR08"] = "R max: 0.8 (4x)",
            ["SumR05a"] = "max: R0.5 (4x)",
            ["SumRa"] = typ > 68 ? "max R1.2" : "max R0.5",
            ["SumRa32"] = "3.2",
            ["SumRa32a"] = "3.2",
            ["SumF1"] = "1x45°",
            ["SumF2"] = "1.5x45°",
            ["SumF3"] = "1x45°",
            ["SumF4"] = "0.5x45° (2x)",
            ["SumCo1"] = "0.150",
        };

        /// <summary>
        /// Calculates page 1 control measurements.
        /// </summary>
        private Dictionary<string, string> CalculatePage1Measurements(bool isSupportedMachine, int typ)
        {
            return new Dictionary<string, string>
            {
                ["SumF1_1"] = isSupportedMachine ? "1/5" : "",
                ["SumD1_1"] = isSupportedMachine ? "Mätmaskin Alt.Skjutmått" : "",
                ["SumAF1_1"] = "",

                ["SumF1_2"] = isSupportedMachine ? "1/2" : "",
                ["SumD1_2"] = (typ <= 60) ? "Mätmaskin Alt.UD-Apparat" : "Mätplatta",
                ["SumAF1_2"] = "",

                ["SumF1_3"] = isSupportedMachine ? "1/2" : "",
                ["SumD1_3"] = isSupportedMachine ? "Mätmaskin Alt.Skjutmått" : "",
                ["SumAF1_3"] = "",

                ["SumF1_4"] = isSupportedMachine ? "Inst. alt misstanke" : "",
                ["SumD1_4"] = isSupportedMachine ? "Ytjämnhetsmätare" : "",
                ["SumAF1_4"] = "Övriga bearbetade ytor 6.3",

                ["SumF1_5"] = "",
                ["SumD1_5"] = "",
                ["SumAF1_5"] = "",

                ["SumF1_6"] = isSupportedMachine ? "Inst." : "",
                ["SumD1_6"] = isSupportedMachine ? "Mätmaskin Alt.Mätservice" : "",
                ["SumAF1_6"] = "Vid misstänkt formfel, lämna till mätrum",

                ["SumF1_7"] = isSupportedMachine ? "Inst." : "",
                ["SumD1_7"] = isSupportedMachine ? "Mätmaskin Alt.Skjutmått" : "",
                ["SumAF1_7"] = ""
            };
        }

        /// <summary>
        /// Calculates page 2 control measurements.
        /// </summary>
        private Dictionary<string, string> CalculatePage2Measurements(bool isSupportedMachine)
        {
            return new Dictionary<string, string>
            {
                ["SumF2_1"] = isSupportedMachine ? "1/2" : "",
                ["SumD2_1"] = isSupportedMachine ? "Mätmaskin Alt.Skjutmått" : "",
                ["SumAF2_1"] = "",

                ["SumF2_2"] = isSupportedMachine ? "1/2" : "",
                ["SumD2_2"] = isSupportedMachine ? "Mätmaskin Alt.Skjutmått" : "",
                ["SumAF2_2"] = "",

                ["SumF2_3"] = isSupportedMachine ? "1/2" : "",
                ["SumD2_3"] = isSupportedMachine ? "Mätmaskin Alt.Skjutmått" : "",
                ["SumAF2_3"] = "",

                ["SumF2_4"] = isSupportedMachine ? "1/2" : "",
                ["SumD2_4"] = isSupportedMachine ? "Gängtolk" : "",
                ["SumAF2_4"] = "",

                ["SumF2_5"] = isSupportedMachine ? "1/2" : "",
                ["SumD2_5"] = isSupportedMachine ? "Mätmaskin Alt.Skjutmått/Mall" : "",
                ["SumAF2_5"] = "",

                ["SumF2_6"] = isSupportedMachine ? "Inst. alt misstanke" : "",
                ["SumD2_6"] = isSupportedMachine ? "Ytjämnhetsmätare" : "",
                ["SumAF2_6"] = "Övriga bearbetade ytor 6.3",

                ["SumF2_7"] = isSupportedMachine ? "1/2" : "",
                ["SumD2_7"] = isSupportedMachine ? "Mätmaskin Alt.Skjutmått" : "",
                ["SumAF2_7"] = ""
            };
        }

        private Dictionary<string, string> CalculateAdminFields() => new()
        {
            ["SumTextS1"] = "Okulärkontroll Grader, frifläckar, slagmärken, repor, valkar, ytjämnhet och faser, skarpa kanter avgradas.",
            ["SumTextS2"] = "Okulärkontroll Grader, frifläckar, slagmärken, repor, valkar, ytjämnhet och faser, skarpa kanter avgradas.",
            ["SumRitNr"] = "7433476: senaste utg. märkning: 7433523: senaste utg.",
            ["SumRitNr2"] = "7433476: senaste utg. märkning: 7433523: senaste utg."
        };
    }
}