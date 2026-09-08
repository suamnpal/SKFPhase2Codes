using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class LOCK_ETS_30_630 : ITemplateCalculations
    {
        private static readonly string[] TypList = { "34", "36", "38", "40", "44", "48", "52", "56", "60", "64", "68", "72", "76", "80", "84", "88", "92", "96", "500", "530", "560", "600", "630" };
        private static readonly double[] AList = { 186.0, 196.0, 206.0, 216.0, 236.0, 256.0, 276.0, 296.0, 316.0, 336.0, 356.0, 376.0, 396.0, 417.0, 437.0, 457.0, 477.0, 497.0, 517.0, 547.0, 577.0, 617.0, 647.0 };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();
            string subject = req?.ProductDesignation ?? "";
            string machine = req?.MachineNumber ?? "";

            kv["VaLPopUp"] = ComputePopup(req?.Published);

            string tmpBet = subject.ToUpperInvariant().Trim().Replace(".", ",");
            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string typ = tokens.Length >= 2 ? tokens[1] : tokens.Length == 1 ? tokens[0] : "";
            int typIndex = GetMember(typ, TypList);
            double a = typIndex > 0 && typIndex <= AList.Length ? AList[typIndex - 1] : 0;

            kv["SumA"] = "(A) " + Fmt(a);
            kv["SumATol"] = "+ 0";
            kv["SumATolN"] = H13NegativeTolerance(a);

            kv["SumC"] = "(C) 8";
            kv["SumCTol"] = "± 0.5";

            kv["SumE"] = "(E) 5,5";
            kv["SumETol"] = "+ 0.480";
            kv["SumETolN"] = "- 0";

            double typNumber = ToDouble(typ);
            string f = typNumber < 49 ? "1,26" : typNumber < 77 ? "1,24" : "1,28";
            kv["SumF"] = "(F) " + f;

            double g = typNumber < 49 ? 10.3 : typNumber < 77 ? 10.1 : 10.4;
            kv["SumG"] = "(G) " + Fmt(g);
            kv["SumGTol"] = g < 10.1 ? "± 0.110" : "± 0.135";

            kv["SumV"] = "7º";
            kv["SumRit"] = "716150:senaste utg.";
            kv["SumRa"] = "12,5";

            bool nakamuraOrLb45 = EqualsAny(machine, "Nakamura", "LB45");
            bool maxMullerOrSkepp6 = EqualsAny(machine, "MaxMuller", "Skepp6");
            bool machineValid = EqualsAny(machine, "Nakamura", "MaxMuller", "Skepp6", "LB45");
            string machineS1 = machineValid ? GetMachineName(machine) : "";
            kv["SumMaskinValS1"] = "Maskin: " + machineS1;

            kv["SumF1_1"] = nakamuraOrLb45 ? "1/5" : maxMullerOrSkepp6 ? "1/1" : "";
            kv["SumF1_2"] = nakamuraOrLb45 ? "1/5" : maxMullerOrSkepp6 ? "1/1" : "";
            kv["SumF1_3"] = nakamuraOrLb45 ? "1/10" : maxMullerOrSkepp6 ? "1/1" : "";
            kv["SumF1_4"] = nakamuraOrLb45 ? "1/skift" : maxMullerOrSkepp6 ? "1/1" : "";
            kv["SumF1_5"] = nakamuraOrLb45 ? "1/skift" : maxMullerOrSkepp6 ? "1/1" : "";
            kv["SumF1_6"] = nakamuraOrLb45 ? "1/skift" : maxMullerOrSkepp6 ? "1/1" : "";
            kv["SumF1_7"] = nakamuraOrLb45 ? "1/skift" : maxMullerOrSkepp6 ? "1/1" : "";
            kv["SumF1_8"] = "";
            kv["SumF1_9"] = "";
            kv["SumF1_0"] = "";

            kv["SumD1_1"] = machineValid ? "Skjutmått" : "";
            kv["SumD1_2"] = machineValid ? "Djupmått" : "";
            kv["SumD1_3"] = machineValid ? "Skjutmått" : "";
            kv["SumD1_4"] = machineValid ? "Skjutmått" : "";
            kv["SumD1_5"] = machineValid ? "Skjutmått" : "";
            kv["SumD1_6"] = machineValid ? "Vinkelmätare" : "";
            kv["SumD1_7"] = machineValid ? "Ytjämnhetsmätare" : "";
            kv["SumD1_8"] = "";
            kv["SumD1_9"] = "";
            kv["SumD1_0"] = "";

            kv["SumAF1_1"] = "";
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = "";
            kv["SumAF1_6"] = "";
            kv["SumAF1_7"] = machineValid ? "Alla bearbetade ytor Ra 12.5" : "";
            kv["SumAF1_8"] = "";
            kv["SumAF1_9"] = "";
            kv["SumAF1_0"] = "";

            kv["SumTextS1"] = "Okulärkontroll: Grader, frifläckar, slagmärken, repor, valkar, ytjämnhet och faser <<LineBreak>>Skarpa kanter avgradas.";

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
                ? "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)" + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + "Popupruta aktiv till " + until.ToString("yyyy-MM-dd")
                : "";
        }

        private static string H13NegativeTolerance(double value)
        {
            if (value < 180.1) return "- 0.630";
            if (value < 250.1) return "- 0.720";
            if (value < 315.1) return "- 0.810";
            if (value < 400.1) return "- 0.890";
            if (value < 500.1) return "- 0.970";
            return "- 1.100";
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
            if (string.Equals(machine ?? "", "Nakamura", StringComparison.OrdinalIgnoreCase)) return "Nakamura";
            if (string.Equals(machine ?? "", "MaxMuller", StringComparison.OrdinalIgnoreCase)) return "MaxMuller";
            if (string.Equals(machine ?? "", "Skepp6", StringComparison.OrdinalIgnoreCase)) return "Skepp6";
            if (string.Equals(machine ?? "", "LB45", StringComparison.OrdinalIgnoreCase)) return "LB45";
            return "";
        }

        private static double ToDouble(string value)
        {
            double.TryParse((value ?? "").Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out var result);
            return result;
        }

        private static string Fmt(double value)
        {
            return value.ToString("0.################", CommonFunctions.Culture);
        }
    }
}
