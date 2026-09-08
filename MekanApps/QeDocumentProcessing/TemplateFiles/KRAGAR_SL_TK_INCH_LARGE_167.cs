using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class KRAGAR_SL_TK_INCH_LARGE_167 : ITemplateCalculations
    {
        private static readonly string[] TypList =
        {
            "167", "168", "178", "513", "553",
            "825", "872", "888", "907", "978"
        };

        private static readonly Dictionary<string, double[]> DTables =
            new Dictionary<string, double[]>(StringComparer.OrdinalIgnoreCase)
            {
                { "167", new[] { 220.0, 211.61, 201.61, 215.0, 246.0, 254.0, 268.0, 276.0, 292.0, 300.0, 316.0, 330.0 } },
                { "168", new[] { 220.0, 213.2, 203.2, 217.0, 248.0, 254.0, 268.0, 276.0, 292.0, 300.0, 316.0, 330.0 } },
                { "178", new[] { 260.0, 251.3, 241.3, 254.0, 286.0, 294.0, 312.0, 320.0, 338.0, 346.0, 364.0, 378.0 } },
                { "513", new[] { 249.0, 238.6, 228.6, 243.0, 274.0, 282.0, 298.0, 306.0, 322.0, 330.0, 346.0, 360.0 } },
                { "553", new[] { 260.0, 249.71, 239.71, 254.0, 286.0, 294.0, 312.0, 320.0, 338.0, 346.0, 364.0, 378.0 } },
                { "888", new[] { 474.0, 467.2, 457.2, 467.0, 503.0, 511.0, 531.0, 539.0, 559.0, 567.0, 587.0, 601.0 } },
                { "907", new[] { 424.0, 410.05, 400.05, 415.0, 449.0, 457.0, 477.0, 485.0, 505.0, 513.0, 533.0, 547.0 } },
                { "978", new[] { 494.0, 479.9, 469.9, 478.5, 517.0, 525.0, 545.0, 553.0, 573.0, 581.0, 601.0, 615.0 } }
            };

        private static readonly Dictionary<string, double[]> BTables =
            new Dictionary<string, double[]>(StringComparer.OrdinalIgnoreCase)
            {
                { "167", new[] { 36.0, 15.5, 10.5, 4.0, 8.0, 9.0, 26.0 } },
                { "168", new[] { 36.0, 15.5, 10.5, 4.0, 8.0, 9.0, 26.0 } },
                { "178", new[] { 36.0, 15.5, 10.5, 4.0, 8.0, 9.0, 26.0 } },
                { "513", new[] { 36.0, 15.5, 10.5, 4.0, 8.0, 9.0, 26.0 } },
                { "553", new[] { 36.0, 15.5, 10.5, 4.0, 8.0, 9.0, 26.0 } },
                { "888", new[] { 49.5, 22.5, 15.0, 6.0, 8.0, 19.0, 37.5 } },
                { "907", new[] { 47.5, 21.5, 13.5, 6.0, 8.0, 18.0, 35.5 } },
                { "978", new[] { 49.5, 22.5, 15.0, 6.0, 8.0, 19.0, 37.5 } }
            };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            List<Bookmark> bookmarks = req.Bookmarks;
            var kv = new Dictionary<string, string>();

            string subject = req?.ProductDesignation ?? "";
            string machine = req?.MachineNumber ?? "";

            kv["VaLPopUp"] = ComputePopup(req?.Published);

            string tmpFormat = subject.ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");

            string[] tokens =
                tmpBet.Split(
                    new char[] { ' ', '/', '.', '-' },
                    StringSplitOptions.RemoveEmptyEntries);

            string originalTyp = tokens.Length >= 3 ? tokens[2] : "";
            string tableTyp = GetMirrorTyp(originalTyp);

            int index = GetMember(originalTyp, TypList);

            kv["VaLTypLista"] =
                index == 0
                    ? "Produktbeteckningen ingår ej i mallen"
                    : "";

            double d1 = double.TryParse(GetBookMarkValue(bookmarks, "Ø d1"),  out var d1Value) && d1Value != 0 ? d1Value : GetD(tableTyp, 1);
            double d2 = double.TryParse(GetBookMarkValue(bookmarks, "Ø d2"), out var d2Value) && d2Value != 0 ? d2Value : GetD(tableTyp, 2);
            double d3 = double.TryParse(GetBookMarkValue(bookmarks, "Ø d3"),  out var d3Value) && d3Value != 0 ? d3Value : GetD(tableTyp, 3);
            double d4 = double.TryParse(GetBookMarkValue(bookmarks, "Ø d4"),  out var d4Value) && d4Value != 0 ? d4Value : GetD(tableTyp, 4);
            double d5 = double.TryParse(GetBookMarkValue(bookmarks, "Ø d5"),  out var d5Value) && d5Value != 0 ? d5Value : GetD(tableTyp, 5);
            double d6 = double.TryParse(GetBookMarkValue(bookmarks, "Ø d6"),  out var d6Value) && d6Value != 0 ? d6Value : GetD(tableTyp, 6);
            double d7 = double.TryParse(GetBookMarkValue(bookmarks, "Ø d7"),  out var d7Value) && d7Value != 0 ? d7Value : GetD(tableTyp, 7);
            double d8 = double.TryParse(GetBookMarkValue(bookmarks, "Ø d8"),  out var d8Value) && d8Value != 0 ? d8Value : GetD(tableTyp, 8);
            double d9 = double.TryParse(GetBookMarkValue(bookmarks, "Ø d9"),  out var d9Value) && d9Value != 0 ? d9Value : GetD(tableTyp, 9);
            double d10 = double.TryParse(GetBookMarkValue(bookmarks, "Ø d10"),  out var d10Value) && d10Value != 0 ? d10Value : GetD(tableTyp, 10);
            double d11 = double.TryParse(GetBookMarkValue(bookmarks, "Ø d11"),  out var d11Value) && d11Value != 0 ? d11Value : GetD(tableTyp, 11);
            double d12 = double.TryParse(GetBookMarkValue(bookmarks, "Ø d12"),  out var d12Value) && d12Value != 0 ? d12Value : GetD(tableTyp, 12);

            double b = double.TryParse(GetBookMarkValue(bookmarks, "Bredd B"),  out var bValue) && bValue != 0 ? bValue : GetB(tableTyp, 1);
            double b1 = double.TryParse(GetBookMarkValue(bookmarks, "Bredd b1"),  out var b1Value) && b1Value != 0 ? b1Value : GetB(tableTyp, 2);
            double b2 = double.TryParse(GetBookMarkValue(bookmarks, "Bredd b2"),  out var b2Value) && b2Value != 0 ? b2Value : GetB(tableTyp, 3);
            double b3 = double.TryParse(GetBookMarkValue(bookmarks, "Bredd b3"),  out var b3Value) && b3Value != 0 ? b3Value : GetB(tableTyp, 4);
            double b4 = double.TryParse(GetBookMarkValue(bookmarks, "Bredd b4"),  out var b4Value) && b4Value != 0 ? b4Value : GetB(tableTyp, 5);
            double b5 = double.TryParse(GetBookMarkValue(bookmarks, "Bredd b5"),  out var b5Value) && b5Value != 0 ? b5Value : GetB(tableTyp, 6);
            double b6 = double.TryParse(GetBookMarkValue(bookmarks, "Bredd b6"),   out var b6Value) && b6Value != 0 ? b6Value : GetB(tableTyp, 7);

            // Diameter d1
            kv["Sumd1"] = "(d1) " + (d1);
            kv["Sumd1Tol"] = "+ 4.0";
            kv["Sumd1TolN"] = "- 0.0";

            // Diameter d2 - H12
            kv["Sumd2"] = "(d2) " + (d2);
            kv["Sumd2Tol"] = "+ " + Fmt3(H12Tolerance(d2));
            kv["Sumd2TolN"] = "- 0.0";

            // Diameter d3
            kv["Sumd3"] = "(d3) " + (d3);
            kv["Sumd3Tol"] = "+ 0.200";
            kv["Sumd3TolN"] = "+ 0.100";

            // Diameter d4 - h12
            kv["Sumd4"] = "(d4) " + (d4);
            kv["Sumd4Tol"] = "+ 0.0";
            kv["Sumd4TolN"] = "- " + Fmt3(H12Tolerance(d4));

            // Diameter d5 - H12
            kv["Sumd5"] = "(d5) " + (d5);
            kv["Sumd5Tol"] = "+ " + Fmt3(H12Tolerance(d5));
            kv["Sumd5TolN"] = "- 0.0";

            // Diameter d6 - h12
            kv["Sumd6"] = "(d6) " + (d6);
            kv["Sumd6Tol"] = "+ 0.0";
            kv["Sumd6TolN"] = "- " + Fmt3(H12Tolerance(d6));

            // Diameter d7 - H12
            kv["Sumd7"] = "(d7) " + (d7);
            kv["Sumd7Tol"] = "+ " + Fmt3(H12Tolerance(d7));
            kv["Sumd7TolN"] = "- 0.0";

            // Diameter d8 - h12
            kv["Sumd8"] = "(d8) " + (d8);
            kv["Sumd8Tol"] = "+ 0.0";
            kv["Sumd8TolN"] = "- " + Fmt3(H12Tolerance(d8));

            // Diameter d9 - H12
            kv["Sumd9"] = "(d9) " + (d9);
            kv["Sumd9Tol"] = "+ " + Fmt3(H12Tolerance(d9));
            kv["Sumd9TolN"] = "- 0.0";

            // Diameter d10 - h12
            kv["Sumd10"] = "(d10) " + (d10);
            kv["Sumd10Tol"] = "+ 0.0";
            kv["Sumd10TolN"] = "- " + Fmt3(H12Tolerance(d10));

            // Diameter d11 - H12
            kv["Sumd11"] = "(d11) " + (d11);
            kv["Sumd11Tol"] = "+ " + Fmt3(H12Tolerance(d11));
            kv["Sumd11TolN"] = "- 0.0";

            // Diameter d12
            kv["Sumd12"] = "(d12) " + (d12);
            kv["Sumd12Tol"] = "± 3.0";

            // Diameter d13
            double d13 = d1 < 321 ? 16.0 : 18.0;
            kv["Sumd13"] =
                d1 > 400
                    ? "n/a"
                    : "3x Ø " + (d13);

            // B
            kv["SumB"] = "(B) " + (b);
            kv["SumBTol"] = "+ 1.0";
            kv["SumBTolN"] = "+ 0.0";

            // b1
            kv["Sumb1"] = "(b1) " + (b1);
            kv["Sumb1Tol"] = "± 1.0";

            // b2
            kv["Sumb2"] = "(b2) " + (b2);
            kv["Sumb2Tol"] = "+ 0.500";
            kv["Sumb2TolN"] = "- 0.0";

            // b3
            kv["Sumb3"] = "(b3) " + (b3);
            kv["Sumb3Tol"] = "+ 0.300";
            kv["Sumb3TolN"] = "- 0.0";

            // b4
            kv["Sumb4"] = "(b4) " + (b4);
            kv["Sumb4Tol"] = "+ 0.250";
            kv["Sumb4TolN"] = "- 0.0";

            // b5
            kv["Sumb5"] = "(b5) " + (b5);
            kv["Sumb5Tol"] = "± 1.0";

            // b6
            kv["Sumb6"] = "3x (b6) " + (b6);
            kv["Sumb6Tol"] = "+ 0.0";
            kv["Sumb6TolN"] = "- 0.500";

            // b7
            double typNumber;
            double.TryParse(originalTyp, NumberStyles.Any, CultureInfo.InvariantCulture, out typNumber);

            double b7 = typNumber < 44 ? 9.0 : 14.5;
            kv["Sumb7"] = "(b7) " + (b7);
            kv["Sumb7Tol"] = "+ 0.400";
            kv["Sumb7TolN"] = "- 0.0";

            // Thread G
            double g = typNumber < 44 ? 8.0 : d1 < 315 ? 10.0 : 12.0;
            kv["SumG"] = "(G) M" + Fmt(g) + " (3x)";

            // Radius
            kv["SumR08"] = "R max: 0.8 (8x)";
            kv["SumR05"] = "R0.5 (2x)";
            kv["SumR05a"] = "max R0.5 (3x)";
            kv["SumR4"] = "R4";

            // Chamfers
            double f1 = d1 < 350 ? 1.5 : 2.0;
            kv["SumF1"] = "(2x) " + Fmt(f1).Replace(",",".") + " x45°";
            kv["SumF2"] = "(2x) 1x45°";

            // Concentricity
            kv["SumCo"] = "0.150";
            kv["SumCo1"] = d1 < 301 ? "0.150" : "0.250";

            // Surface
            kv["SumRa32"] = "3.2";
            kv["SumRa125"] = d1 > 400 ? "n/a" : "12.5";

            // Machine
            bool machineValid =
                EqualsIgnoreCase(machine, "Nakamura") ||
                EqualsIgnoreCase(machine, "LB45") ||
                EqualsIgnoreCase(machine, "LT-3000EX");

            kv["SumMaskinvalS1"] =
                "Maskin: " + (machineValid ? machine : "") + " - Diametrala mått.";

            kv["SumMaskinvalS2"] =
                "Maskin: " + (machineValid ? machine : "") + " - Övriga mått.";

            // Frequencies page 1
            kv["SumF1_1"] = machineValid ? "1/2" : "";
            kv["SumF1_2"] = machineValid ? "1/2" : "";
            kv["SumF1_3"] = machineValid ? "1/2" : "";
            kv["SumF1_4"] = machineValid ? "1/2" : "";
            kv["SumF1_5"] = "";
            kv["SumF1_6"] = machineValid ? "1/2" : "";
            kv["SumF1_7"] = machineValid ? "1/2" : "";

            // Frequencies page 2
            kv["SumF2_1"] = machineValid ? "1/2" : "";
            kv["SumF2_2"] = machineValid ? "1/2" : "";
            kv["SumF2_3"] = machineValid ? "1/2" : "";
            kv["SumF2_4"] = machineValid ? "1/2" : "";
            kv["SumF2_5"] = machineValid ? "1/2" : "";
            kv["SumF2_6"] = machineValid ? "1/2" : "";
            kv["SumF2_7"] = machineValid ? "1/2" : "";

            // Gauges page 1
            kv["SumD1_1"] = machineValid ? "Mätmaskin alt. Skjutmått" : "";
            kv["SumD1_2"] = machineValid ? "Mätmaskin alt.UD-apparat eller Mikrometer" : "";
            kv["SumD1_3"] = machineValid ? "Mätmaskin alt.Skjutmått" : "";
            kv["SumD1_4"] = machineValid ? "Ytjämnhetsmätare" : "";
            kv["SumD1_5"] = "";
            kv["SumD1_6"] = machineValid ? "Mätmaskin alt.Mätservice" : "";
            kv["SumD1_7"] = machineValid ? "Mätmaskin alt.Skjutmått" : "";

            // Gauges page 2
            kv["SumD2_1"] = machineValid ? "Mätmaskin alt.Skjutmått" : "";
            kv["SumD2_2"] = machineValid ? "Mätmaskin alt.Skjutmått" : "";
            kv["SumD2_3"] = machineValid ? "Mätmaskin alt.Skjutmått" : "";
            kv["SumD2_4"] = machineValid ? "Gängtolk" : "";
            kv["SumD2_5"] = machineValid ? "Mätmaskin alt.Skjutmått" : "";
            kv["SumD2_6"] = machineValid ? "Ytjämnhetsmätare" : "";
            kv["SumD2_7"] = machineValid ? "Mätmaskin alt.Skjutmått" : "";

            // Remarks page 1
            kv["SumAF1_1"] = "";
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = machineValid ? "Övriga bearbetade ytor 6.3" : "";
            kv["SumAF1_5"] = "";
            kv["SumAF1_6"] = "";
            kv["SumAF1_7"] = "";

            // Remarks page 2
            kv["SumAF2_1"] = "";
            kv["SumAF2_2"] = "";
            kv["SumAF2_3"] = "";
            kv["SumAF2_4"] = "";
            kv["SumAF2_5"] = "";
            kv["SumAF2_6"] = machineValid ? "Övriga bearbetade ytor 6.3" : "";
            kv["SumAF2_7"] = "";

            // Drawing
            kv["SumRitNr"] =
                index == 0
                    ? ""
                    : "TK " + originalTyp + " V: senaste utgåva";

            kv["SumRitNr2"] = kv["SumRitNr"];

            if (subject == "SL-TK 825 V" || subject == "SL-TK 872 V")
            {

                kv["SumRitNr"] = ": senaste utgåva";
                kv["SumRitNr2"] = ": senaste utgåva";
            }
           // Text
           kv["SumTextS1"] =
                "Okulärkontroll Grader, frifläckar, slagmärken, repor, valkar, ytjämnhet och faser, skarpa kanter avgradas.";

            kv["SumTextS2"] =
                "Okulärkontroll Grader, frifläckar, slagmärken, repor, valkar, ytjämnhet och faser, skarpa kanter avgradas.";

            return kv;
        }

        private static string GetMirrorTyp(string typ)
        {
            if (EqualsIgnoreCase(typ, "825"))
                return "888";

            if (EqualsIgnoreCase(typ, "872"))
                return "907";

            return typ ?? "";
        }

        private static string GetBookMarkValue(List<Bookmark> bookmarks, string key)
        {
            foreach (var bookmark in bookmarks)
            {
                if (bookmark != null && string.Equals(bookmark.BookmarkName, key, StringComparison.OrdinalIgnoreCase))
                {
                    return bookmark.BookmarkValue.Replace(",",".");
                }
            }
            return string.Empty;
        }

        private static double GetD(string typ, int oneBasedIndex)
        {
            if (!DTables.TryGetValue(typ ?? "", out var table))
                return 0;

            if (oneBasedIndex < 1 || oneBasedIndex > table.Length)
                return 0;

            return table[oneBasedIndex - 1];
        }

        private static double GetB(string typ, int oneBasedIndex)
        {
            if (!BTables.TryGetValue(typ ?? "", out var table))
                return 0;

            if (oneBasedIndex < 1 || oneBasedIndex > table.Length)
                return 0;

            return table[oneBasedIndex - 1];
        }

        private static double H12Tolerance(double value)
        {
            if (value < 3.01) return 0.100;
            if (value < 6.01) return 0.120;
            if (value < 10.01) return 0.150;
            if (value < 18.01) return 0.180;
            if (value < 30.01) return 0.210;
            if (value < 50.01) return 0.250;
            if (value < 80.01) return 0.300;
            if (value < 120.01) return 0.350;
            if (value < 180.01) return 0.400;
            if (value < 250.01) return 0.460;
            if (value < 315.01) return 0.520;
            if (value < 400.01) return 0.570;
            if (value < 500.01) return 0.630;
            if (value < 630.01) return 0.700;
            if (value < 800.01) return 0.800;
            if (value < 1000.01) return 0.900;
            if (value < 1250.01) return 1.050;
            if (value < 1600.01) return 1.250;
            if (value < 2000.01) return 1.500;
            if (value < 2500.01) return 1.750;

            return 2.100;
        }

        private static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published))
                return "";

            if (!DateTime.TryParse(published, out var dt))
                return "";

            var until = dt.AddDays(14);

            return DateTime.Today <= until.Date
                ? "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)\n\n" +
                  "Information om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument\n\n" +
                  "Popupruta aktiv till " +
                  until.ToString("yyyy-MM-dd")
                : "";
        }

        private static int GetMember(string value, string[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (string.Equals(
                    list[i],
                    value,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return i + 1;
                }
            }

            return 0;
        }

        private static bool EqualsIgnoreCase(string a, string b)
        {
            return string.Equals(
                a ?? "",
                b ?? "",
                StringComparison.OrdinalIgnoreCase);
        }

        private static string Fmt(double v)
        {
            return v.ToString(
                "0.################",
                CommonFunctions.Culture);
          
        }

        private static string Fmt3(double v)
        {
            return v.ToString(
                "F3",
                CultureInfo.InvariantCulture);
        }
    }
}