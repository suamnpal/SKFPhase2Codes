using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class LOCK_HL_FNL_505_522_Flanslagerhus_Lock_B : ITemplateCalculations
    {
        private static readonly string[] AllKeys = new[]
        {
            "SumFD", "SumFDTol", "SumFDTolN",
            "SumLD", "SumLDTol",
            "SumFH", "SumFHTol", "SumFHTolN",
            "SumB", "SumBTol",
            "SumLh", "SumLhTol", "SumLhTolN",
            "SumAD", "SumADTol", "SumADTolN",
            "SumTD", "SumTDTol", "SumTDTolN",
            "SumA",
            "SumR1", "SumRm",
            "SumRa63",
            "SumRitNr", "SumKlEgenskaper", "SumGGDRit",
            "SumMaskinValS1",
            "SumF1_1", "SumF1_2", "SumF1_3", "SumF1_4", "SumF1_5",
            "SumF1_6", "SumF1_7", "SumF1_8", "SumF1_9", "SumF1_0",
            "SumD1_1", "SumD1_2", "SumD1_3", "SumD1_4", "SumD1_5",
            "SumD1_6", "SumD1_7", "SumD1_8", "SumD1_9", "SumD1_0",
            "SumAF1_1", "SumAF1_2", "SumAF1_3", "SumAF1_4", "SumAF1_5",
            "SumAF1_6", "SumAF1_7", "SumAF1_8", "SumAF1_9", "SumAF1_0",
            "SumTextS1",
            "VaLPopUp",
        };

        private static readonly double[] ListFD = { 52, 62, 72, 80, 85, 90, 100, 110, 120, 130, 140, 150, 160, 180, 200 };
        private static readonly double[] ListB = { 21.5, 21.8, 22.7, 23.9, 26.6, 28.2, 30.8, 29.8, 31.1, 32.8, 36.4, 37.6, 37.7, 41.0, 43.7 };
        private static readonly double[] ListLD = { 62, 72, 82, 92, 97, 102, 112, 124, 135, 148, 157, 168, 178, 200, 220 };
        private static readonly double[] ListAD = { 21, 26, 31, 36, 41, 46, 51, 56, 61.5, 66.5, 71.5, 76.5, 81.5, 92, 102 };
        private static readonly double[] ListTD = { 31, 38, 43, 48, 53, 58, 67, 72, 77, 82, 89, 94, 99, 111, 125 };

        private static readonly string[] TypKeys = { "05", "06", "07", "08", "09", "10", "11", "12", "13", "15", "16", "17", "18", "20", "22" };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();
            for (int i = 0; i < AllKeys.Length; i++) kv[AllKeys[i]] = "";

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");

            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : string.Empty;

            string tmpTyp = tmpBet3.Length >= 2 ? tmpBet3.Substring(tmpBet3.Length - 2) : tmpBet3;

            int tmpTypIndex = -1;
            for (int i = 0; i < TypKeys.Length; i++)
            {
                if (EqualsI(TypKeys[i], tmpTyp))
                {
                    tmpTypIndex = i;
                    break;
                }
            }

            double tmpFD = tmpTypIndex >= 0 ? ListFD[tmpTypIndex] : 0;
            double tmpB = tmpTypIndex >= 0 ? ListB[tmpTypIndex] : 0;
            double tmpLD = tmpTypIndex >= 0 ? ListLD[tmpTypIndex] : 0;
            double tmpAD = tmpTypIndex >= 0 ? ListAD[tmpTypIndex] : 0;
            double tmpTD = tmpTypIndex >= 0 ? ListTD[tmpTypIndex] : 0;

            kv["SumFD"] = "(FD) " + FormatDot(tmpFD);
            kv["SumFDTol"] = "- " + FormatDot3(FDTolPos(tmpFD)) + " [3]";
            kv["SumFDTolN"] = "- " + FormatDot3(FDTolNeg(tmpFD)) + " [3]";

            kv["SumLD"] = "(LD) " + FormatDot(tmpLD);
            kv["SumLDTol"] = "\u00b1 " + FormatDot3(GeneralTolPM(tmpLD));

            double tmpFH = tmpFD < 81 ? 4 : 5;
            kv["SumFH"] = "(FH) " + FormatDot(tmpFH);
            kv["SumFHTol"] = "+ 0.00 [3]";
            kv["SumFHTolN"] = "- " + FormatDot3(0.18) + " [3]";

            kv["SumB"] = "(B) " + FormatDot(tmpB);
            kv["SumBTol"] = "\u00b1 " + FormatDot3(GeneralTolPM(tmpB));

            string tmpLh = tmpFD < 81 ? "5.5" : tmpFD < 121 ? "6.6" : tmpFD < 161 ? "9" : "11.5";
            string tmpLhCount = tmpFD < 121 ? " (3x)" : " (4x)";
            kv["SumLh"] = "(Lh) " + tmpLh + tmpLhCount;
            kv["SumLhTol"] = "+ " + FormatDot3(LhTolPos(tmpFD)) + " [3]";
            kv["SumLhTolN"] = "- 0.00 [3]";

            kv["SumAD"] = "(AD) " + FormatDot(tmpAD);
            kv["SumADTol"] = "+ " + FormatDot3(ADTolPos(tmpAD)) + " [3]";
            kv["SumADTolN"] = "- 0.00 [3]";

            kv["SumTD"] = "(TD) " + FormatDot(tmpTD);
            kv["SumTDTol"] = "+ " + FormatDot3(TDTolPos(tmpTD)) + " [3]";
            kv["SumTDTolN"] = "- 0.00 [3]";

            kv["SumA"] = "0.2";
            kv["SumR1"] = "1x45\u00b0";
            kv["SumRm"] = "R max 1";
            kv["SumRa63"] = "6.3";

            kv["SumRitNr"] = tmpFormat + ":1";
            kv["SumKlEgenskaper"] = "PRODUCTION NUTS & SLEEVES & HOUSINGS\\ALLMÄNKLASSADE EGENSKAPER\\Flänslagerhus";
            kv["SumGGDRit"] = "7437455";

            string tmpMall = GetMall(tmpBet3);

            bool isMultusOrGenos = EqualsI(maskinVal, "Multus") || EqualsI(maskinVal, "Genos");
            string maskinLabel = EqualsI(maskinVal, "Multus") ? "Multus" : EqualsI(maskinVal, "Genos") ? "Genos" : "";

            kv["SumMaskinValS1"] = "Maskin: " + maskinLabel;

            kv["SumF1_1"] = isMultusOrGenos ? "1/5" : "";
            kv["SumF1_2"] = isMultusOrGenos ? "1:a bit" : "";
            kv["SumF1_3"] = isMultusOrGenos ? "1/30" : "";
            kv["SumF1_4"] = isMultusOrGenos ? "1/Skift" : "";
            kv["SumF1_5"] = isMultusOrGenos ? "1/30" : "";
            kv["SumF1_6"] = "";
            kv["SumF1_7"] = isMultusOrGenos ? "1/Skift" : "";
            kv["SumF1_8"] = isMultusOrGenos ? "1/30" : "";
            kv["SumF1_9"] = isMultusOrGenos ? "1/30" : "";
            kv["SumF1_0"] = isMultusOrGenos ? "1:a bit" : "";

            kv["SumD1_1"] = isMultusOrGenos ? "UD-Apparat" : "";
            kv["SumD1_2"] = isMultusOrGenos ? "M\u00e4tmaskin" : "";
            kv["SumD1_3"] = isMultusOrGenos ? "Skjutm\u00e5tt" : "";
            kv["SumD1_4"] = isMultusOrGenos ? "Max/Min Tolk" : "";
            kv["SumD1_5"] = isMultusOrGenos ? "Djupm\u00e5tt/Skjutm\u00e5tt" : "";
            kv["SumD1_6"] = "";
            kv["SumD1_7"] = isMultusOrGenos ? "Ytj\u00e4mnhetsm\u00e4tare" : "";
            kv["SumD1_8"] = isMultusOrGenos ? "Skjutm\u00e5tt" : "";
            kv["SumD1_9"] = isMultusOrGenos ? "Mall" : "";
            kv["SumD1_0"] = isMultusOrGenos ? "M\u00e4tmaskin" : "";

            kv["SumAF1_1"] = isMultusOrGenos ? "1:a bit M\u00e4tmaskin" : "";
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = isMultusOrGenos ? "1:a bit M\u00e4tmaskin" : "";
            kv["SumAF1_4"] = isMultusOrGenos ? "1:a bit M\u00e4tmaskin" : "";
            kv["SumAF1_5"] = "";
            kv["SumAF1_6"] = "";
            kv["SumAF1_7"] = isMultusOrGenos ? "\u00d6vriga bearbetade ytor Ra 12.5" : "";
            kv["SumAF1_8"] = isMultusOrGenos ? "1:a bit M\u00e4tmaskin" : "";
            kv["SumAF1_9"] = isMultusOrGenos ? "T\u00e4tningssp\u00e5r med mall: " + tmpMall : "";
            kv["SumAF1_0"] = "";

            kv["SumTextS1"] = "Okul\u00e4rkontroll: Grader, frifl\u00e4ckar, slagm\u00e4rken, repor, valkar, ytj\u00e4mnhet och faser\nSkarpa kanter avgradas.";

            return kv;
        }

        private static string GetMall(string bet3)
        {
            if (EqualsI(bet3, "505")) return "7543174/1 FNL 505";
            if (EqualsI(bet3, "506") || EqualsI(bet3, "507") || EqualsI(bet3, "508") || EqualsI(bet3, "509") || EqualsI(bet3, "510")) return "7453174/2  FNL 506-510";
            if (EqualsI(bet3, "511") || EqualsI(bet3, "512")) return "7453175/1 FNL 511-512";
            if (EqualsI(bet3, "513") || EqualsI(bet3, "515")) return "7453175/2 FNL 513-515";
            if (EqualsI(bet3, "516") || EqualsI(bet3, "517") || EqualsI(bet3, "518")) return "7453175/3 FNL 516-518";
            if (EqualsI(bet3, "520")) return "7453175/4 FNL 520";
            if (EqualsI(bet3, "522")) return "7453175/5 FNL 522";
            return "";
        }

        private static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published)) return "";
            DateTime pubDt;
            if (!DateTime.TryParse(published, out pubDt)) return "";
            int dagar = 14;
            DateTime validTill = pubDt.AddDays(dagar);
            if (DateTime.Today <= validTill.Date)
                return "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom " +
                       dagar.ToString(CultureInfo.InvariantCulture) +
                       " dagar)\n\nInformation om senaste \u00e4ndring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument\n\nPopupruta aktiv till " +
                       validTill.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return "";
        }

        private static double FDTolPos(double fd)
        {
            if (fd < 80.1) return 0.060;
            if (fd < 120.1) return 0.072;
            if (fd < 180.1) return 0.085;
            return 0.1;
        }

        private static double FDTolNeg(double fd)
        {
            if (fd < 80.1) return 0.134;
            if (fd < 120.1) return 0.159;
            if (fd < 180.1) return 0.185;
            return 0.215;
        }

        private static double LhTolPos(double fd)
        {
            if (fd < 81) return 0.18;
            if (fd < 121) return 0.22;
            if (fd < 161) return 0.22;
            return 0.27;
        }

        private static double ADTolPos(double ad)
        {
            if (ad < 30.1) return 0.21;
            if (ad < 50.1) return 0.25;
            if (ad < 80.1) return 0.30;
            return 0.35;
        }

        private static double TDTolPos(double td)
        {
            if (td < 50.1) return 0.2;
            if (td < 80.1) return 0.3;
            if (td < 120.1) return 0.35;
            return 0.4;
        }

        private static double GeneralTolPM(double v)
        {
            if (v < 6.01) return 0.1;
            if (v < 30.01) return 0.2;
            if (v < 120.01) return 0.3;
            if (v < 315.01) return 0.5;
            if (v < 1000.01) return 0.8;
            if (v < 2000.01) return 1.2;
            return 2.0;
        }

        private static bool EqualsI(string a, string b)
            => string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);

        private static string FormatDot(double v)
        {
            return v.ToString(CommonFunctions.Culture).Replace(".", ",");
        }

        private static string FormatDot3(double v)
        {
            return v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
        }

        private static string FormatDot1(double v)
        {
            return v.ToString("F1", CommonFunctions.Culture).Replace(",", ".");
        }
    }
}