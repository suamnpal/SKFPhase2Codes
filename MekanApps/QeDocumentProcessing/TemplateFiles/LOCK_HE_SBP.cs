using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class LOCK_HE_SBP : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var result = InitializeAll();

            Merge(result, GetHeader(request));
            Merge(result, GetDiameters(request));
            Merge(result, GetDepth(request));
            Merge(result, GetSurface());
            Merge(result, GetHoles());
            Merge(result, GetAngles(request));
            Merge(result, GetMachine(request));
            Merge(result, GetFrequency(request));
            Merge(result, GetTools(request));
            Merge(result, GetRemarks(request));
            Merge(result, GetOtherText(request));

            return result;
        }

        private Dictionary<string, string> InitializeAll()
        {
            return new Dictionary<string, string>
            {
                {"SumRit",""},
                {"SumStämpling",""},
                {"SumD1",""},
                {"SumD1Tol",""},
                {"SumD1TolN",""},
                {"SumD2",""},
                {"SumD2Tol",""},
                {"SumH",""},
                {"SumHTol",""},
                {"SumHTolN",""},
                {"SumRa125",""},
                {"SumRa125a",""},
                {"SumRa63",""},
                {"SumRa63a",""},
                {"SumRa32",""},
                {"SumBH",""},
                {"SumP",""},
                {"SumBHV1",""},
                {"SumBHV2",""},
                {"SumBHV3",""},
                {"SumBHV4",""},
                {"SumMaskinValS1",""},
                {"SumF1_1",""},
                {"SumF1_2",""},
                {"SumF1_3",""},
                {"SumF1_4",""},
                {"SumD1_1",""},
                {"SumD1_2",""},
                {"SumD1_3",""},
                {"SumD1_4",""},
                {"SumAF1_1",""},
                {"SumAF1_2",""},
                {"SumAF1_3",""},
                {"SumAF1_4",""},
                {"SumTextS1",""}
            };
        }

        private Dictionary<string, string> GetHeader(APIRequest request)
        {
            var d = new Dictionary<string, string>();
            string designation = Normalize(GetSafe(request.ProductDesignation));
            d["SumRit"] = designation;
            d["SumStämpling"] = string.IsNullOrEmpty(designation) ? "" : "Stämplas: " + designation;
            return d;
        }

        private Dictionary<string, string> GetDiameters(APIRequest request)
        {
            var d = new Dictionary<string, string>();

            double d1 = GetBookmarkDouble(request, "Ø (D1)");
            double d2 = GetBookmarkDouble(request, "Ø (D2) emellan hål");

            if (d1 > 0)
            {
                d["SumD1"] = "(D1) " + FormatInt(d1);
                d["SumD1Tol"] = " 0";
                d["SumD1TolN"] = GetD1Tol(d1);
            }

            if (d2 > 0)
            {
                d["SumD2"] = "(D2) " + FormatInt(d2);
                d["SumD2Tol"] = GetD2Tol(d2);
            }

            return d;
        }

        private Dictionary<string, string> GetDepth(APIRequest request)
        {
            var d = new Dictionary<string, string>();

            double h = GetBookmarkDouble(request, "Infälld djup (H)");

            if (h > 0)
            {
                d["SumH"] = "(H)  " + FormatInt(h);
                d["SumHTol"] = "  0";
                d["SumHTolN"] = GetHTol(h);
            }

            return d;
        }

        private Dictionary<string, string> GetSurface()
        {
            return new Dictionary<string, string>
            {
                {"SumRa125","12.5"},
                {"SumRa125a","12.5"},
                {"SumRa63","6.3"},
                {"SumRa63a","6.3"},
                {"SumRa32","3.2"}
            };
        }

        private Dictionary<string, string> GetHoles()
        {
            return new Dictionary<string, string>
            {
                {"SumBH","8x Ø 14"},
                {"SumP","8x Planas Ø26"}
            };
        }
        private double GetAngle(APIRequest request, string name, double defaultValue)
        {
            string val = GetBookmark(request, name);

            if (string.IsNullOrWhiteSpace(val) || val.Trim() == "0")
                return defaultValue;

            double d;
            if (double.TryParse(val.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out d))
                return d;

            return defaultValue;
        }

        private Dictionary<string, string> GetAngles(APIRequest request)
        {
            var d = new Dictionary<string, string>();

            double v1 = GetAngle(request, "Vinkel Hål 5-4", 50);
            double v2 = GetAngleV2(request);
            double v3 = GetAngle(request, "Vinkel Hål 3-2", 40);
            double v4 = 180 - v1 - v2 - v3;

            d["SumBHV1"] = FormatInt(v1) + "º";
            d["SumBHV2"] = FormatInt(v2) + "º";
            d["SumBHV3"] = FormatInt(v3) + "º";
            d["SumBHV4"] = FormatInt(v4) + "º";

            return d;
        }

        private Dictionary<string, string> GetMachine(APIRequest request)
        {
            var d = new Dictionary<string, string>();

            string machine = GetSafe(request.MachineNumber);
            bool valid = machine == "MaxMuller/K&T" || machine == "Nakamura";

            if (valid)
                d["SumMaskinValS1"] = "Maskin: " + machine;

            return d;
        }

        private Dictionary<string, string> GetFrequency(APIRequest request)
        {
            var d = new Dictionary<string, string>();

            string machine = GetSafe(request.MachineNumber);
            bool valid = machine == "MaxMuller/K&T" || machine == "Nakamura";

            if (valid)
            {
                d["SumF1_1"] = "1/2";
                d["SumF1_2"] = "1/5";
                d["SumF1_3"] = "1/2";
                d["SumF1_4"] = "1/Skift";
            }

            return d;
        }

        private Dictionary<string, string> GetTools(APIRequest request)
        {
            var d = new Dictionary<string, string>();

            string machine = GetSafe(request.MachineNumber);
            bool valid = machine == "MaxMuller/K&T" || machine == "Nakamura";

            if (valid)
            {
                d["SumD1_1"] = "Mikrometer";
                d["SumD1_2"] = "Skjutmått";
                d["SumD1_3"] = "Skjutmått";
                d["SumD1_4"] = "Skjutmått";
            }

            return d;
        }

        private Dictionary<string, string> GetRemarks(APIRequest request)
        {
            var d = new Dictionary<string, string>();

            string machine = GetSafe(request.MachineNumber);

            d["SumAF1_1"] = "";
            d["SumAF1_2"] = machine == "MaxMuller/K&T" ? "Borras i K&T" : "";
            d["SumAF1_3"] = "";
            d["SumAF1_4"] = "";

            return d;
        }

        private Dictionary<string, string> GetOtherText(APIRequest request)
        {
            var d = new Dictionary<string, string>();
            string text = GetBookmark(request, "Övrig Text");
            d["SumTextS1"] = string.IsNullOrWhiteSpace(text) || text == "0" ? " " : text.Trim();
            return d;
        }

        private double GetAngleV2(APIRequest request)
        {
            string val = GetBookmark(request, "Vinkel Hål 4-3");

            // ✅ Lotus: treat "0" as missing
            if (string.IsNullOrWhiteSpace(val) || val.Trim() == "0")
            {
                string designation = Normalize(GetSafe(request.ProductDesignation));

                // Lotus logic: TmpBet3 = 3140
                if (designation.Contains("3140"))
                    return 45;

                return 50;
            }

            double d;
            if (double.TryParse(val.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out d))
                return d;

            return 50;
        }


        private string GetD1Tol(double v)
        {
            if (v < 120.01) return "- 0.140";
            if (v < 180.01) return "- 0.160";
            if (v < 250.01) return "- 0.185";
            if (v < 315.01) return "- 0.210";
            if (v < 400.01) return "- 0.230";
            if (v < 500.01) return "- 0.250";
            if (v < 630.01) return "- 0.280";
            if (v < 800.01) return "- 0.320";
            if (v < 1000.01) return "- 0.360";
            if (v < 1250.01) return "- 0.420";
            if (v < 1600.01) return "- 0.500";
            if (v < 2000.01) return "- 0.600";
            if (v < 2500.01) return "- 0.700";
            return "- 0.860";
        }

        private string GetD2Tol(double v)
        {
            if (v < 120.01) return "± 0.270";
            if (v < 180.01) return "± 0.315";
            if (v < 250.01) return "± 0.360";
            if (v < 315.01) return "± 0.405";
            if (v < 400.01) return "± 0.445";
            if (v < 500.01) return "± 0.485";
            if (v < 630.01) return "± 0.550";
            if (v < 800.01) return "± 0.625";
            if (v < 1000.01) return "± 0.700";
            if (v < 1250.01) return "± 0.825";
            if (v < 1600.01) return "± 0.975";
            if (v < 2000.01) return "± 1.150";
            if (v < 2500.01) return "± 1.400";
            return "± 1.650";
        }

        private string GetHTol(double v)
        {
            if (v < 3.01) return "- 0.060";
            if (v < 6.01) return "- 0.075";
            if (v < 10.01) return "- 0.090";
            if (v < 18.01) return "- 0.110";
            if (v < 30.01) return "- 0.130";
            if (v < 50.01) return "- 0.160";
            if (v < 80.01) return "- 0.190";
            if (v < 120.01) return "- 0.220";
            return "- 0.250";
        }

        private string Normalize(string input)
        {
            return input == null ? "" : input.Trim().ToUpperInvariant();
        }

        private string GetBookmark(APIRequest request, string name)
        {
            if (request.Bookmarks == null) return "";
            foreach (var b in request.Bookmarks)
                if (b.BookmarkName == name)
                    return b.BookmarkValue ?? "";
            return "";
        }

        private double GetBookmarkDouble(APIRequest request, string name, double fallback = 0)
        {
            var val = GetBookmark(request, name);
            double d;
            if (double.TryParse(val, NumberStyles.Any, CultureInfo.InvariantCulture, out d))
                return d;
            if (double.TryParse(val.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out d))
                return d;
            return fallback;
        }

        private string FormatInt(double v)
        {
            return v.ToString("0", CultureInfo.InvariantCulture);
        }

        private string GetSafe(string v)
        {
            return v ?? "";
        }

        private void Merge(Dictionary<string, string> target, Dictionary<string, string> source)
        {
            foreach (var kv in source)
                target[kv.Key] = kv.Value ?? "";
        }
    }
}