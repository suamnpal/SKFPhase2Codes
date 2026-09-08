using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;
namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Klamhylsor_Repning_OH_30xx_HB_V31 : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();
            string subject = req?.ProductDesignation ?? "";
            string machine = req?.MachineNumber ?? "";

            kv["VaLPopUp"] = ComputePopup(req?.Published);

            string tmpBet = subject.ToUpperInvariant().Trim().Replace(".", ",");
            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet2 = tokens.Length >= 2 ? tokens[1] : "";
            string tmpBet3 = tokens.Length >= 3 ? tokens[2] : "";
            bool hasSlash = tmpBet.Contains("/");
            int tmpCountB2 = tmpBet2.Length;

            string serie = hasSlash && (tmpCountB2 == 3 || tmpCountB2 == 2)
                ? tmpBet2
                : tmpCountB2 > 4
                    ? Left(tmpBet2, 3)
                    : tmpCountB2 == 3
                        ? Left(tmpBet2, 1)
                        : Left(tmpBet2, 2);

            string typ = tmpCountB2 > 2 ? Right(tmpBet2, 2) : tmpBet3;
            double serieNumber = ToDouble(serie);
            string rit = serieNumber == 30 ? "7433785" : serieNumber == 31 ? "7433786" : serieNumber == 39 ? "7433787" : serieNumber == 240 ? "7440344" : serieNumber == 241 ? "7440352" : "Fel Mall";
            kv["SumRitNr"] = rit + ":senaste utg.";

            double[] table = GetTable(serie, typ);
            double n = table[0];
            double v = table[1];
            double lc = table[2];
            double ld = table[3];

            kv["Sumn"] = FmtDot(n);
            kv["SumV"] = FmtDot(v) + "° x" + FmtDot(n - 1);
            kv["SumV12"] = "12°";

            double slitsbredd = GetBookmarkDouble(req, "Slitsbredd");
            double innerdiameter = GetBookmarkDouble(req, "Innerdiameter");
            double diameterStorande = GetBookmarkDouble(req, "Diameter Storände");

            double constantFirstGroove = RoundTo(Math.Sin((12.0 / 2.0) * Math.PI / 180.0) * 2.0, 0.0001);
            double constantDividingGroove = RoundTo(Math.Sin((v / 2.0) * Math.PI / 180.0) * 2.0, 0.0001);

            double kordaFirstInner = RoundTo((innerdiameter / 2.0) * constantFirstGroove, 0.1);
            double kordaFirstOuter = RoundTo((diameterStorande / 2.0) * constantFirstGroove, 0.1);
            double kordaDividingInner = RoundTo((innerdiameter / 2.0) * constantDividingGroove, 0.1);
            double kordaDividingOuter = RoundTo((diameterStorande / 2.0) * constantDividingGroove, 0.1);

            kv["SumKR1inv"] = Fmt(kordaFirstInner - (slitsbredd / 2.0));
            kv["SumKR1utv"] = Fmt(kordaFirstOuter - (slitsbredd / 2.0));
            kv["SumKRDinv"] = Fmt(kordaDividingInner);
            kv["SumKRDutv"] = Fmt(kordaDividingOuter);

            kv["SumLc"] = "(Lc) " + FmtDot(lc);
            kv["SumLcTol"] = "± " + Fmt3Dot(GeneralTolerance(lc));
            kv["SumLd"] = "(Ld) " + FmtDot(ld);
            kv["SumLdTol"] = "± " + Fmt3Dot(GeneralTolerance(ld));

            kv["SumM1"] = "(M) 1.5 x" + FmtDot(n);
            kv["SumM2"] = kv["SumM1"];
            kv["SumM1Tol"] = "± 0,100";
            kv["SumM2Tol"] = kv["SumM1Tol"];
            kv["Sumt1"] = "(t) 0.5";
            kv["Sumt2"] = kv["Sumt1"];
            kv["Sumt1Tol"] = "± 0,100";
            kv["Sumt2Tol"] = kv["Sumt1Tol"];
            kv["SumTextS1"] = "";

            bool machineValid = EqualsAny(machine, "Skepp6", "K&T", "VTR-160", "MacTurn 550");
            bool skepp6 = EqualsAny(machine, "Skepp6");
            bool halfFrequencyMachine = EqualsAny(machine, "K&T", "VTR-160", "MacTurn 550");
            string machineName = machineValid ? GetMachineName(machine) : "";
            kv["SumMaskinValS1"] = "Maskin: " + machineName + " - Repning";

            for (int i = 1; i <= 6; i++)
                kv["SumF1_" + i] = skepp6 ? "1/1" : halfFrequencyMachine ? "1/2" : "";
            kv["SumF1_7"] = "";
            kv["SumF1_8"] = "";
            kv["SumF1_9"] = "";
            kv["SumF1_0"] = "";

            kv["SumD1_1"] = machineValid ? "Visuell kontroll" : "";
            kv["SumD1_2"] = machineValid ? "Visuell kontroll" : "";
            kv["SumD1_3"] = machineValid ? "Skjutmått" : "";
            kv["SumD1_4"] = machineValid ? "Skjutmått" : "";
            kv["SumD1_5"] = machineValid ? "Skjutmått" : "";
            kv["SumD1_6"] = machineValid ? "Djupmått" : "";
            kv["SumD1_7"] = "";
            kv["SumD1_8"] = "";
            kv["SumD1_9"] = "";
            kv["SumD1_0"] = "";

            for (int i = 1; i <= 9; i++)
                kv["SumAF1_" + i] = "";
            kv["SumAF1_0"] = "";

            return kv;
        }

        private static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published))
                return "";
            if (!DateTime.TryParse(published, out var dt))
                return "";
            var until = dt.AddDays(14);
            return DateTime.Today <= until.Date
                ? "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)" + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + "Information om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument" + Environment.NewLine + Environment.NewLine + "Popupruta aktiv till " + until.ToString("yyyy-MM-dd")
                : "";
        }

        private static double[] GetTable(string serie, string typ)
        {
            double serieNumber = ToDouble(serie);
            switch ((typ ?? "").Trim())
            {
                case "44": return new[] { 7.0, 56.0, 65.0, 59.0 };
                case "56": return new[] { 8.0, 48.0, 88.0, 90.0 };
                case "64": return new[] { 10.0, 37.3, 71.0, 60.5 };
                case "84": return new[] { 13.0, 28.0, 86.5, 75.0 };
                case "96": return new[] { 14.0, 25.8, 98.0, 82.5 };
                case "500": return new[] { 15.0, 24.0, 106.0, 83.5 };
                case "530": return new[] { 16.0, 22.4, 110.0, 92.5 };
                case "560": return new[] { 17.0, 21.0, 118.0, 97.5 };
                case "600": return serieNumber == 30 ? new[] { 18.0, 19.8, 120.0, 100.0 } : serieNumber == 31 ? new[] { 18.0, 19.8, 145.0, 150.0 } : new[] { 0.0, 0.0, 0.0, 0.0 };
                case "630": return new[] { 19.0, 18.7, 122.0, 106.0 };
                case "670": return new[] { 20.0, 17.7, 130.5, 115.0 };
                case "750": return serieNumber == 30 ? new[] { 22.0, 16.0, 143.5, 125.0 } : serieNumber == 39 ? new[] { 22.0, 16.0, 139.0, 92.5 } : new[] { 0.0, 0.0, 0.0, 0.0 };
                case "800": return new[] { 24.0, 14.6, 146.0, 129.0 };
                case "900": return new[] { 27.0, 12.9, 160.0, 140.0 };
                default: return new[] { 27.0, 12.9, 160.0, 140.0 };
            }
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

        private static double GetBookmarkDouble(APIRequest req, string name)
        {
            if (req?.Bookmarks == null)
                return 0;
            foreach (var bookmark in req.Bookmarks)
            {
                if (string.Equals(bookmark.BookmarkName ?? "", name, StringComparison.OrdinalIgnoreCase))
                    return ToDouble(bookmark.BookmarkValue);
            }
            return 0;
        }

        private static bool EqualsAny(string value, params string[] values)
        {
            foreach (var item in values)
            {
                if (string.Equals(value ?? "", item, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        private static string GetMachineName(string machine)
        {
            if (string.Equals(machine ?? "", "Skepp6", StringComparison.OrdinalIgnoreCase)) return "Skepp6";
            if (string.Equals(machine ?? "", "K&T", StringComparison.OrdinalIgnoreCase)) return "K&T";
            if (string.Equals(machine ?? "", "VTR-160", StringComparison.OrdinalIgnoreCase)) return "VTR-160";
            if (string.Equals(machine ?? "", "MacTurn 550", StringComparison.OrdinalIgnoreCase)) return "MacTurn 550";
            return "";
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

        private static string FmtDot(double value)
        {
            return value.ToString("0.################", CultureInfo.InvariantCulture);
        }

        private static string Fmt3Dot(double value)
        {
            return value.ToString("0.000", CultureInfo.InvariantCulture);
        }
    }
}
