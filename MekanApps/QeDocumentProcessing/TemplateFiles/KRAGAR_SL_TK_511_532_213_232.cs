using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class KRAGAR_SL_TK_511_532_213_232 : ITemplateCalculations
    {
        private static readonly string[] Typ2List = { "13", "15", "17", "20", "26", "28", "30", "32" };
        private static readonly string[] Typ5List = { "11", "12", "13", "15", "16", "17", "18", "19", "20", "22", "24", "26", "28", "30", "32" };

        private static readonly Dictionary<string, double[]> DTables = new Dictionary<string, double[]>(StringComparer.OrdinalIgnoreCase)
        {
            { "511", new[] { 50.0, 58.0, 56.0, 67.0, 71.5, 82.5, 88.5, 99.5, 105.5, 116.5, 123.0 } },
            { "512", new[] { 55.0, 60.0, 61.0, 72.0, 76.5, 87.5, 93.5, 104.5, 110.5, 121.5, 128.0 } },
            { "513", new[] { 60.0, 65.0, 66.0, 77.0, 81.5, 92.5, 98.5, 109.5, 115.5, 126.5, 133.0 } },
            { "515", new[] { 65.0, 70.0, 71.0, 82.0, 86.5, 97.5, 103.5, 114.5, 120.5, 131.5, 138.0 } },
            { "516", new[] { 70.0, 76.0, 76.0, 91.0, 96.0, 107.0, 113.0, 124.0, 130.0, 141.0, 149.0 } },
            { "517", new[] { 75.0, 82.0, 81.0, 96.0, 101.5, 112.5, 118.5, 129.5, 135.5, 146.5, 154.5 } },
            { "518", new[] { 80.0, 88.0, 86.0, 101.0, 107.5, 118.5, 124.5, 135.5, 141.5, 152.5, 160.5 } },
            { "519", new[] { 85.0, 91.0, 91.0, 106.0, 110.0, 122.0, 128.0, 140.0, 146.0, 158.0, 166.0 } },
            { "520", new[] { 90.0, 97.0, 96.0, 111.0, 116.0, 128.0, 134.0, 146.0, 152.0, 164.0, 172.0 } },
            { "522", new[] { 100.0, 107.0, 106.0, 121.0, 126.0, 138.0, 144.0, 156.0, 162.0, 174.0, 182.0 } },
            { "524", new[] { 110.0, 117.0, 116.0, 131.0, 139.0, 152.0, 159.0, 172.0, 179.0, 192.0, 201.0 } },
            { "526", new[] { 115.0, 122.0, 121.0, 136.0, 144.0, 158.0, 166.0, 180.0, 188.0, 202.0, 212.0 } },
            { "528", new[] { 125.0, 133.0, 131.0, 146.0, 156.0, 170.0, 178.0, 192.0, 200.0, 214.0, 224.0 } },
            { "530", new[] { 135.0, 142.0, 141.0, 156.0, 166.0, 180.0, 188.0, 202.0, 210.0, 224.0, 234.0 } },
            { "532", new[] { 140.0, 147.0, 146.0, 161.0, 171.0, 185.0, 193.0, 207.0, 215.0, 229.0, 239.0 } },
            { "213", new[] { 75.0, 80.0, 81.0, 91.0, 96.0, 107.0, 113.0, 124.0, 130.0, 141.0, 149.0 } },
            { "215", new[] { 85.0, 90.0, 91.0, 101.0, 105.0, 114.0, 119.0, 128.0, 133.0, 142.0, 149.0 } },
            { "217", new[] { 95.0, 100.0, 101.0, 111.0, 116.0, 128.0, 134.0, 146.0, 152.0, 164.0, 172.0 } },
            { "220", new[] { 115.0, 122.0, 121.0, 131.0, 139.0, 152.0, 159.0, 172.0, 179.0, 192.0, 201.0 } },
            { "226", new[] { 145.0, 152.0, 151.0, 161.0, 171.0, 185.0, 193.0, 207.0, 215.0, 229.0, 239.0 } },
            { "228", new[] { 155.0, 159.0, 161.0, 170.0, 178.0, 194.0, 202.0, 218.0, 226.0, 242.0, 254.0 } },
            { "230", new[] { 165.0, 170.0, 171.0, 184.5, 194.5, 208.5, 215.5, 229.5, 236.5, 250.5, 261.0 } },
            { "232", new[] { 175.0, 180.0, 181.0, 194.5, 204.5, 218.5, 225.5, 239.5, 246.5, 260.5, 271.0 } }
        };

        private static readonly Dictionary<string, double[]> BTables = new Dictionary<string, double[]>(StringComparer.OrdinalIgnoreCase)
        {
            { "511", new[] { 9.5, 5.0, 5.0, 20.0, 9.0, 13.5, 25.0 } },
            { "512", new[] { 9.2, 5.0, 5.0, 19.5, 8.5, 13.0, 25.0 } },
            { "513", new[] { 9.2, 5.0, 5.0, 19.5, 8.5, 13.0, 25.0 } },
            { "515", new[] { 9.5, 6.8, 6.8, 18.5, 7.0, 11.5, 23.5 } },
            { "516", new[] { 12.0, 6.8, 6.8, 27.0, 10.5, 15.0, 33.0 } },
            { "517", new[] { 12.0, 6.8, 6.8, 27.0, 10.5, 15.0, 33.0 } },
            { "518", new[] { 12.0, 6.8, 6.8, 27.0, 10.5, 15.0, 33.0 } },
            { "519", new[] { 13.5, 6.8, 9.3, 25.5, 10.0, 14.5, 31.5 } },
            { "520", new[] { 12.0, 6.8, 9.3, 24.0, 8.5, 13.0, 30.0 } },
            { "522", new[] { 13.0, 8.0, 8.5, 25.5, 10.0, 14.5, 31.5 } },
            { "524", new[] { 13.5, 8.0, 9.0, 26.5, 10.0, 14.5, 32.5 } },
            { "526", new[] { 13.5, 7.9, 9.0, 26.0, 10.0, 14.5, 32.0 } },
            { "528", new[] { 13.5, 7.9, 8.9, 28.5, 10.0, 14.5, 34.5 } },
            { "530", new[] { 13.5, 7.9, 8.9, 25.5, 10.0, 14.5, 31.5 } },
            { "532", new[] { 13.5, 7.9, 9.0, 28.5, 10.0, 14.5, 34.5 } },
            { "213", new[] { 12.0, 6.8, 6.8, 27.0, 10.5, 15.0, 33.0 } },
            { "215", new[] { 12.0, 6.8, 6.8, 27.0, 10.5, 15.0, 33.0 } },
            { "217", new[] { 12.0, 6.8, 9.3, 24.0, 8.5, 13.0, 30.0 } },
            { "220", new[] { 13.5, 8.0, 9.0, 26.5, 10.0, 15.5, 32.5 } },
            { "226", new[] { 13.5, 7.9, 9.0, 28.5, 10.0, 14.5, 34.5 } },
            { "228", new[] { 11.0, 9.0, 6.5, 27.0, 10.5, 15.5, 33.0 } },
            { "230", new[] { 13.5, 9.0, 9.5, 29.0, 11.0, 16.0, 35.0 } },
            { "232", new[] { 13.5, 9.0, 9.5, 29.0, 11.0, 16.0, 35.0 } }
        };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            List<Bookmark> bookmarks = req.Bookmarks;
            var kv = new Dictionary<string, string>();
            string subject = req?.ProductDesignation ?? "";
            string machine = req?.MachineNumber ?? "";
            kv["VaLPopUp"] = ComputePopup(req?.Published);

            string tmpBet = subject.ToUpperInvariant().Trim().Replace(".", ",");
            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.' }, StringSplitOptions.RemoveEmptyEntries);
            string product = tokens.Length >= 2 ? tokens[1] : tokens.Length == 1 ? tokens[0] : "";
            string serie = product.Length > 0 ? product.Substring(0, 1) : "";
            string typ = product.Length >= 2 ? product.Substring(product.Length - 2, 2) : "";
            string tableTyp = serie + typ;
            int typIndex = serie == "2" ? GetMember(typ, Typ2List) : GetMember(typ, Typ5List);

            kv["VaLTypLista"] = typIndex == 0 ? "Produktbeteckningen ingår ej i mallen" : "";

            double d = ReadDouble(bookmarks, "Ø d", GetD(tableTyp, 1));
            double d1 = ReadDouble(bookmarks, "Ø d1", GetD(tableTyp, 2));
            double d2 = ReadDouble(bookmarks, "Ø d2", GetD(tableTyp, 3));
            double d3 = ReadDouble(bookmarks, "Ø d3", GetD(tableTyp, 4));
            double d4 = ReadDouble(bookmarks, "Ø d4", GetD(tableTyp, 5));
            double d5 = ReadDouble(bookmarks, "Ø d5", GetD(tableTyp, 6));
            double d6 = ReadDouble(bookmarks, "Ø d6", GetD(tableTyp, 7));
            double d7 = ReadDouble(bookmarks, "Ø d7", GetD(tableTyp, 8));
            double d8 = ReadDouble(bookmarks, "Ø d8", GetD(tableTyp, 9));
            double d9 = ReadDouble(bookmarks, "Ø d9", GetD(tableTyp, 10));
            double da = ReadDouble(bookmarks, "Ø Da", GetD(tableTyp, 11));
            double b = ReadDouble(bookmarks, "Bredd b", 4.3);
            double b1 = ReadDouble(bookmarks, "Bredd b1", GetB(tableTyp, 1));
            double b2 = ReadDouble(bookmarks, "Bredd b2", GetB(tableTyp, 2));
            double b3 = ReadDouble(bookmarks, "Bredd b3", GetB(tableTyp, 3));
            double b4 = ReadDouble(bookmarks, "Bredd b4", GetB(tableTyp, 4));
            double b5 = ReadDouble(bookmarks, "Bredd b5", GetB(tableTyp, 5));
            double b6 = ReadDouble(bookmarks, "Bredd b6", GetB(tableTyp, 6));
            double ba = ReadDouble(bookmarks, "Bredd Ba", GetB(tableTyp, 7));
            double typNumber = ToDouble(typ);
            int d10 = GetD10(serie, typ, typIndex);
            bool machineValid = EqualsIgnoreCase(machine, "Nakamura") || EqualsIgnoreCase(machine, "LB45") || EqualsIgnoreCase(machine, "LT-3000EX");

            kv["Sumd"] = "(d) " + ToDouble(Fmt(d));
            kv["SumdTol"] = "+ 0.200";
            kv["SumdTolN"] = "+ 0.100";
            kv["Sumd1"] = "(d1) " + ToDouble(Fmt(d1));
            kv["Sumd1Tol"] = "+ 0.0";
            kv["Sumd1TolN"] = "- " + Fmt3(H12Tolerance(d1));
            kv["Sumd2"] = "(d2) " + ToDouble(Fmt(d2));
            kv["Sumd2Tol"] = "+ " + Fmt3(H12Tolerance(d2));
            kv["Sumd2TolN"] = "- 0.0";
            kv["Sumd3"] = "(d3) " + ToDouble(Fmt(d3));
            kv["Sumd3Tol"] = "+ 4.000";
            kv["Sumd3TolN"] = "- 0.0";
            kv["Sumd4"] = "(d4) " + ToDouble(Fmt(d4));
            kv["Sumd4Tol"] = "+ 0.0";
            kv["Sumd4TolN"] = "- " + Fmt3(H12Tolerance(d4));
            kv["Sumd5"] = "(d5) " + ToDouble(Fmt(d5));
            kv["Sumd5Tol"] = "+ " + Fmt3(H12Tolerance(d5));
            kv["Sumd5TolN"] = "- 0.0";
            kv["Sumd6"] = "(d6) " + ToDouble(Fmt(d6));
            kv["Sumd6Tol"] = "+ 0.0";
            kv["Sumd6TolN"] = "- " + Fmt3(H12Tolerance(d6));
            kv["Sumd7"] = "(d7) " + ToDouble(Fmt(d7));
            kv["Sumd7Tol"] = "+ " + Fmt3(H12Tolerance(d7));
            kv["Sumd7TolN"] = "- 0.0";
            kv["Sumd8"] = "(d8) " + ToDouble(Fmt(d8));
            kv["Sumd8Tol"] = "+ 0.0";
            kv["Sumd8TolN"] = "- " + Fmt3(H12Tolerance(d8));
            kv["Sumd9"] = "(d9) " + ToDouble(Fmt(d9));
            kv["Sumd9Tol"] = "+ " + Fmt3(H12Tolerance(d9));
            kv["Sumd9TolN"] = "- 0.0";
            kv["SumDa"] = "(Da) " + ToDouble(Fmt(da));
            kv["SumDaTol"] = "± 3.000";
            kv["Sumb"] = "(b) " + ToDouble(Fmt(b));
            kv["SumbTol"] = "+ 0.250";
            kv["SumbTolN"] = "- 0.0";
            kv["Sumb1"] = "(b1) " + ToDouble(Fmt(b1));
            kv["Sumb1Tol"] = "± 1.000";
            kv["Sumb2"] = "(b2) " + ToDouble(Fmt(b2));
            kv["Sumb2Tol"] = "+ 0.200";
            kv["Sumb2TolN"] = "- 0.0";
            kv["Sumb3"] = "(b3) " + ToDouble(Fmt(b3));
            kv["Sumb3Tol"] = "+ 0.200";
            kv["Sumb3TolN"] = "- 0.0";
            kv["Sumb4"] = "3x (b4) " + ToDouble(Fmt(b4));
            kv["Sumb4Tol"] = "+ 0.0";
            kv["Sumb4TolN"] = "- 0.500";
            kv["Sumb5"] = "(b5) " + ToDouble(Fmt(b5));
            kv["Sumb5Tol"] = "+ 0.500";
            kv["Sumb5TolN"] = "- 0.0";
            kv["Sumb6"] = "(b6) " + ToDouble(Fmt(b6));
            kv["Sumb6Tol"] = "± 1.000";
            kv["SumBa"] = "(Ba) " + ToDouble(Fmt(ba));
            kv["SumBaTol"] = "+ 1.000";
            kv["SumBaTolN"] = "- 0.0";
            kv["SumG1"] = "3x (G1) M" + Fmt(typNumber < 13 ? 5.0 : 6.0);
            kv["SumR08"] = "R max: 0.8 (7x)";
            kv["SumR05"] = "R0.5 (2x)";
            kv["SumR3"] = "R3";
            kv["SumF15"] = "1.5x45° (3x)";
            kv["SumF1"] = "1x45° (2x)";
            kv["Sumd10"] = d10 == 0 ? "" : "3 x Ø" + d10;
            kv["SumCo1"] = "0.15";
            kv["SumCo2"] = kv["SumCo1"];
            kv["SumRa32"] = "3.2";
            kv["SumAm"] = "* 1";
            kv["SumMaskinValS1"] = "Maskin: " + machine + " - Diametrala mått.";
            kv["SumMaskinValS2"] = "Maskin: " + machine + " - Bredd mått.";
            kv["SumMaskinvalS1"] = kv["SumMaskinValS1"];
            kv["SumMaskinvalS2"] = kv["SumMaskinValS2"];
            kv["SumM2_8"] = machineValid && d10 != 0 ? "Fräsning" : "";
            kv["SumB2_8"] = machineValid && d10 != 0 ? "d10" : "";

            kv["SumF1_1"] = machineValid ? "1/5" : "";
            kv["SumF1_2"] = machineValid ? "1/5" : "";
            kv["SumF1_3"] = machineValid ? "1/5" : "";
            kv["SumF1_4"] = "";
            kv["SumF1_5"] = "";
            kv["SumF1_6"] = machineValid ? "Inst." : "";
            kv["SumF1_7"] = machineValid ? "1/5" : "";
            kv["SumF2_1"] = machineValid ? "1/5" : "";
            kv["SumF2_2"] = machineValid ? "1/5" : "";
            kv["SumF2_3"] = machineValid ? "1/5" : "";
            kv["SumF2_4"] = machineValid ? "1/5" : "";
            kv["SumF2_5"] = machineValid ? "1/5" : "";
            kv["SumF2_6"] = machineValid ? "1/5" : "";
            kv["SumF2_7"] = machineValid ? "1/5" : "";
            kv["SumF2_8"] = machineValid && d10 != 0 ? "Inst." : "";

            kv["SumD1_1"] = machineValid ? "Mätmaskin alt.Skjutmått" : "";
            kv["SumD1_2"] = machineValid ? "Mätmaskin alt. UD-Apparat" : "";
            kv["SumD1_3"] = machineValid ? "Mätmaskin alt. Skjutmått" : "";
            kv["SumD1_4"] = "";
            kv["SumD1_5"] = "";
            kv["SumD1_6"] = machineValid ? "Mätmaskin alt. Mätservice" : "";
            kv["SumD1_7"] = machineValid ? "Ytjämnhetsmätare" : "";
            kv["SumD2_1"] = machineValid ? "Mätmaskin alt. Skjutmått" : "";
            kv["SumD2_2"] = machineValid ? "Mätmaskin alt. Skjutmått" : "";
            kv["SumD2_3"] = machineValid ? "Mätmaskin alt. Skjutmått" : "";
            kv["SumD2_4"] = machineValid ? "Gängtolk" : "";
            kv["SumD2_5"] = machineValid ? "Mätmaskin alt. Skjutmått" : "";
            kv["SumD2_6"] = machineValid ? "Ytjämnhetsmätare" : "";
            kv["SumD2_7"] = machineValid ? "Mätmaskin alt. Skjutmått" : "";
            kv["SumD2_8"] = machineValid && d10 != 0 ? "Mätmaskin alt. Skjutmått" : "";

            kv["SumAF1_1"] = "";
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = "";
            kv["SumAF1_6"] = "";
            kv["SumAF1_7"] = "";
            kv["SumAF2_1"] = "";
            kv["SumAF2_2"] = "";
            kv["SumAF2_3"] = "";
            kv["SumAF2_4"] = "";
            kv["SumAF2_5"] = "";
            kv["SumAF2_6"] = "";
            kv["SumAF2_7"] = "";
            kv["SumAF2_8"] = machineValid && d10 != 0 ? "Fräsningen skall endast tangera råytan" : "";

            string drawing = GetDrawing(bookmarks, tmpBet, serie, typ);
            kv["SumRitNr"] = drawing + ":senaste utgåva";
            kv["SumRitNr2"] = kv["SumRitNr"];
            kv["SumTextS1"] = "Okulärkontroll Grader, frifläckar, slagmärken, repor, valkar, ytjämnhet och faser, skarpa kanter avgradas.";
            kv["SumTextS2"] = "Okulärkontroll Grader, frifläckar, slagmärken, repor, valkar, ytjämnhet och faser, skarpa kanter avgradas.";

            return kv;
        }

        private static double ReadDouble(List<Bookmark> bookmarks, string key, double fallback)
        {
            return double.TryParse(GetBookMarkValue(bookmarks, key), NumberStyles.Any, CultureInfo.InvariantCulture, out var value) && value != 0 ? value : fallback;
        }

        private static string GetBookMarkValue(List<Bookmark> bookmarks, string key)
        {
            foreach (var bookmark in bookmarks)
            {
                if (bookmark != null && string.Equals(bookmark.BookmarkName, key, StringComparison.OrdinalIgnoreCase))
                {
                    return (bookmark.BookmarkValue ?? "").Replace(",", ".");
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

        private static int GetD10(string serie, string typ, int typIndex)
        {
            int[] serie2 = { 12, 12, 8, 10, 14, 14, 12, 12 };
            int[] serie5 = { 6, 6, 6, 6, 14, 14, 14, 8, 8, 8, 8, 8, 12, 8, 12 };
            if (typIndex < 1)
                return 0;
            if (serie == "2")
                return typIndex <= serie2.Length ? serie2[typIndex - 1] : 0;
            return typIndex <= serie5.Length ? serie5[typIndex - 1] : 0;
        }

        private static string GetDrawing(List<Bookmark> bookmarks, string tmpBet, string serie, string typ)
        {
            string drawing = GetBookMarkValue(bookmarks, "Ritningsnummer");
            if (drawing == "0" || string.IsNullOrWhiteSpace(drawing))
            {
                if (serie == "2")
                    return ToDouble(typ) < 28 ? "7433675" : "7433626";
                return "7433473";
            }
            if (drawing == "1")
                return tmpBet;
            return drawing;
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
            return 0.700;
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
                  "Popupruta aktiv till " + until.ToString("yyyy-MM-dd")
                : "";
        }

        private static int GetMember(string value, string[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (string.Equals(list[i], value, StringComparison.OrdinalIgnoreCase))
                    return i + 1;
            }
            return 0;
        }

        private static bool EqualsIgnoreCase(string a, string b)
        {
            return string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);
        }

        private static double ToDouble(string value)
        {
            double.TryParse((value ?? "").Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out var result);
            return result;
        }

        private static string Fmt(double v)
        {
            return v.ToString("0.################", CommonFunctions.Culture);
        }

        private static string Fmt3(double v)
        {
            return v.ToString("F3", CultureInfo.InvariantCulture);
        }
    }
}
