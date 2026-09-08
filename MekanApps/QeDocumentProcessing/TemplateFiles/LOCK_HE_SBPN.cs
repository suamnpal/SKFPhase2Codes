using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class LOCK_HE_SBPN : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            // ===== CACHE BOOKMARKS =====
            var bm = request.Bookmarks?
                .ToDictionary(b => b.BookmarkName, b => b.BookmarkValue, StringComparer.OrdinalIgnoreCase)
                ?? new Dictionary<string, string>();

            string Get(string key) => bm.TryGetValue(key, out var v) ? v : string.Empty;

            decimal GetDecimal(string key)
            {
                var val = Get(key);
                if (decimal.TryParse(val.Replace(",", "."), System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture, out var d))
                    return d;
                return 0;
            }

            // ===== INPUT =====
            string subjectRaw = request.ProductDesignation ?? string.Empty;
            string machine = request.MachineNumber ?? string.Empty;

            decimal D1 = GetDecimal("Ø (D1)");
            decimal D2 = GetDecimal("Ø (D2) emellan hål");
            decimal H = GetDecimal("Infälld djup (H)");

            DateTime? published = DateTime.TryParse(request.Published, out var dt) ? dt : null;

            // ===== POPUP =====
            int tmpDagar = 14;
            string popup = "";

            if (published.HasValue)
            {
                var validTo = published.Value.AddDays(tmpDagar);

                if (DateTime.Today <= validTo)
                {
                    popup = $"Denna kontrollinstruktion har nyligen blivit uppdaterad (inom {tmpDagar} dagar)\n\n" +
                            $"Popupruta aktiv till {validTo:yyyy-MM-dd}";
                }
            }

            // ===== FORMAT SUBJECT =====
            string formatted = subjectRaw.Trim().ToUpper().Replace(".", ",");
            bool hasSlash = formatted.Contains("/");

            var parts = Regex.Split(formatted, @"[ /\.\-]+");

            string Part(int i) => i < parts.Length ? parts[i] : "0";

            string p1 = Part(0);
            string p2 = Part(1);
            string p3 = Part(2);

            object ParseMixed(string val)
            {
                if (double.TryParse(val, out double n) && n != 0)
                    return n;
                return val;
            }

            var TmpBet1 = ParseMixed(p1);
            var TmpBet2 = ParseMixed(p2);
            var TmpBet3 = ParseMixed(p3);

            // ===== SERIE & TYP =====
            string b3 = TmpBet3.ToString();
            int len = b3.Length;

            string serie =
                hasSlash && (len == 2 || len == 3) ? "NoSerie"
                : len > 4 ? b3.Substring(0, 3)
                : len == 3 ? b3.Substring(0, 1)
                : b3.Substring(0, Math.Min(2, len));

            string typ =
                len >= 2 ? b3.Substring(Math.Max(0, len - 2)) : b3;

            string sumArt = $"{TmpBet1}{TmpBet2}";

            // ===== TOLERANCES =====
            string TolD1(decimal v) =>
                v < 120.01m ? "- 0.140" :
                v < 180.01m ? "- 0.160" :
                v < 250.01m ? "- 0.185" :
                v < 315.01m ? "- 0.210" :
                v < 400.01m ? "- 0.230" :
                v < 500.01m ? "- 0.250" :
                v < 630.01m ? "- 0.280" :
                v < 800.01m ? "- 0.320" :
                v < 1000.01m ? "- 0.360" :
                v < 1250.01m ? "- 0.420" :
                v < 1600.01m ? "- 0.500" :
                v < 2000.01m ? "- 0.600" :
                v < 2500.01m ? "- 0.700" : "- 0.860";

            string TolD2(decimal v) =>
                v < 120.01m ? "± 0.270" :
                v < 180.01m ? "± 0.315" :
                v < 250.01m ? "± 0.360" :
                v < 315.01m ? "± 0.405" :
                v < 400.01m ? "± 0.445" :
                v < 500.01m ? "± 0.485" :
                v < 630.01m ? "± 0.550" :
                v < 800.01m ? "± 0.625" :
                v < 1000.01m ? "± 0.700" :
                v < 1250.01m ? "± 0.825" :
                v < 1600.01m ? "± 0.975" :
                v < 2000.01m ? "± 1.150" :
                v < 2500.01m ? "± 1.400" : "± 1.650";

            string TolH(decimal v) =>
                v < 3.01m ? "- 0.060" :
                v < 6.01m ? "- 0.075" :
                v < 10.01m ? "- 0.090" :
                v < 18.01m ? "- 0.110" :
                v < 30.01m ? "- 0.130" :
                v < 50.01m ? "- 0.160" :
                v < 80.01m ? "- 0.190" :
                v < 120.01m ? "- 0.220" : "- 0.250";

            // ===== MACHINE =====
            bool isMachine = machine == "LB45" || machine == "Nakamura";

            string machineLabel = machine == "LB45" ? "LB45"
                                : machine == "Nakamura" ? "Nakamura"
                                : "";

            // ===== OUTPUT MAP =====
            var map = new Dictionary<string, string>
            {
                ["VaLPopUp"] = popup,

                ["SumArt"] = sumArt,
                ["SumSerie"] = serie,
                ["SumTyp"] = typ,

                ["SumRit"] = formatted,
                ["SumStampling"] = $"Stämplas: {formatted}",

                ["SumD1"] = $"(D1) {D1}",
                ["SumD1Tol"] = "0",
                ["SumD1TolN"] = TolD1(D1),

                ["SumD2"] = $"(D2) {D2}",
                ["SumD2Tol"] = TolD2(D2),

                ["SumH"] = $"(H) {H}",
                ["SumHTol"] = "0",
                ["SumHTolN"] = TolH(H),

                ["SumRa125"] = "12.5",
                ["SumRa32"] = "3.2",
                ["SumRa63"] = "6.3",

                ["SumBH"] = "8x Ø 14",
                ["SumP"] = "8x Planas Ø26",

                ["SumMaskinValS1"] = $"Maskin: {machineLabel}",

                ["SumTextS1"] = Get("Övrig Text") == "0" ? " " : Get("Övrig Text"),

                ["SumStämpling"] = $"Stämplas: {formatted}", 

                ["SumRa125a"] = "12.5",
                ["SumRa63a"] = "6.3",

                ["SumGänga"] = "3xM12",
                ["SumG2"] = "M12",

                ["SumPD"] = "(PD) 1",

            };

            // ===== FREQUENCY & DEVICES =====
            for (int i = 1; i <= 5; i++)
            {
                map[$"SumF1_{i}"] = isMachine
                    ? (i == 2 ? "1/5" :
                       i == 4 || i == 5 ? "1/Skift" : "1/2")
                    : "";

                map[$"SumD1_{i}"] = isMachine ? GetDevice(i) : "";
                map[$"SumAF1_{i}"] = "";
            }

            // ===== FINAL RESULT =====
            foreach (var kv in map)
                result[kv.Key] = kv.Value ?? string.Empty;

            return result;
        }

        private string GetDevice(int i)
        {
            return i switch
            {
                1 => "Mikrometer",
                2 => "Skjutmått",
                3 => "Skjutmått",
                4 => "Gängtolk",
                5 => "Skjutmått",
                _ => ""
            };
        }
    }
}