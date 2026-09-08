using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class RINGAR_GH_TSO : ITemplateCalculations
    {
        private const int TmpDagar = 14;

        private static readonly string[] MaskinList = { "LT-3000EX", "Nakamura", "LC-20" };

        private static readonly int[] TypListC = { 217, 218, 220, 222, 224, 226, 228, 230, 232, 234, 236, 238, 240, 244, 248 };

        private static readonly double[] CLista = { 152, 162, 175, 195, 210, 226, 240, 262, 280, 301, 318, 334, 356, 394, 438 };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = subject.ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");

            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : "";
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : "";
            int tmpBet3 = tokens.Length > 2 ? TryParseInt(tokens[2]) : 0;
            int tmpBet4 = tokens.Length > 3 ? TryParseInt(tokens[3]) : 0;
            string tmpBet5 = tokens.Length > 4 ? tokens[4] : "";

            double tmpa = tmpBet3 < 233 ? 10 : 12;
            kv["Suma"] = "(a) " + Num(tmpa);
            kv["SumaTol"] = tmpa < 6 ? "± 0.1" : tmpa < 30 ? "± 0.2" : "± 0.3";

            int tmpCUtrLista = GetMember(tmpBet3, TypListC);

            double tmpC = ListValue(CLista, tmpCUtrLista);
            string tmpCText = ListText(CLista, tmpCUtrLista);
            kv["SumC"] = "(C) " + tmpCText;
            kv["SumCTol"] = PmTol(tmpC);

            double tmpB = tmpCUtrLista <= 0 ? 0 : (tmpBet3 < 229 ? tmpC - 10 : tmpC - 12);
            string tmpBText = tmpCUtrLista <= 0 ? "" : Num(tmpB);
            kv["SumB"] = "(B) " + tmpBText;
            kv["SumBTol"] = PmTol(tmpB);

            kv["SumRd"] = "0.4";

            kv["SumRa"] = "3.2";

            kv["SumRit"] = "7438715";
            kv["SumStämpel"] = tmpBet;

            string tmpMaskinValS1 = EqualsI(maskinVal, "LT-3000EX") ? "LT-3000EX"
                                  : EqualsI(maskinVal, "Nakamura") ? "Nakamura"
                                  : EqualsI(maskinVal, "LC-20") ? "LC-20"
                                  : "";
            kv["SumMaskinValS1"] = "Maskin: " + tmpMaskinValS1;

            bool isMaskin = GetMember(maskinVal, MaskinList) > 0;
            bool isTom = string.IsNullOrWhiteSpace(maskinVal);

            kv["SumF1_1"] = isMaskin ? "1/1" : isTom ? "1/1" : "";
            kv["SumF1_2"] = isMaskin ? "Skärbyte" : isTom ? "1/1" : "";
            kv["SumF1_3"] = isMaskin ? "Skärbyte" : isTom ? "1/1" : "";
            kv["SumF1_4"] = isMaskin ? "Skärbyte" : isTom ? "1/1" : "";
            kv["SumF1_5"] = isMaskin ? "½/tim" : isTom ? "1/1" : "";

            kv["SumD1_1"] = isMaskin ? "Digitalt Skjutmått" : "";
            kv["SumD1_2"] = isMaskin ? "UD-Apparat" : "";
            kv["SumD1_3"] = isMaskin ? "Digitalt Skjutmått" : "";
            kv["SumD1_4"] = isMaskin ? "UD-Apparat" : "";
            kv["SumD1_5"] = isMaskin ? "Ytjämnhetsmätare" : "";

            kv["SumAF1_1"] = "";
            kv["SumAF1_2"] = isMaskin ? "Inställningsring: " + tmpBText : "";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = isMaskin ? "Alla bearbetade mått" : "";

            kv["SumTextS1"] = "Okulärkontroll av grader, frifläcker, slagmärken, repor, valkar, ytjämnhet samt faser, skarpa kanter avgradas.";

            return kv;
        }

        private static string ComputePopup(string pub)
        {
            if (string.IsNullOrWhiteSpace(pub)) return "";
            DateTime dt;
            if (!DateTime.TryParse(pub, out dt)) return "";
            DateTime till = dt.AddDays(TmpDagar);
            if (DateTime.Today > till.Date) return "";
            return "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom " + TmpDagar + " dagar)" +
                   "<<LineBreak>><<LineBreak>>" +
                   "<<LineBreak>><<LineBreak>>" +
                   "Information om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument" +
                   "<<LineBreak>><<LineBreak>>" +
                   "Popupruta aktiv till " + till.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        private static string PmTol(double v)
        {
            if (v < 6) return "± 0.1";
            if (v < 30) return "± 0.2";
            if (v < 120) return "± 0.3";
            if (v < 400) return "± 0.5";
            if (v < 1000) return "± 0.8";
            if (v < 2000) return "± 1.2";
            return "± 2.0";
        }

        private static double ListValue(double[] list, int index)
        {
            if (list == null || index <= 0 || index > list.Length) return 0;
            return list[index - 1];
        }

        private static string ListText(double[] list, int index)
        {
            if (list == null || index <= 0 || index > list.Length) return "";
            return Num(list[index - 1]);
        }

        private static int GetMember(string val, string[] list)
        {
            if (val == null) return 0;
            for (int i = 0; i < list.Length; i++)
                if (string.Equals(list[i], val, StringComparison.OrdinalIgnoreCase)) return i + 1;
            return 0;
        }

        private static int GetMember(int val, int[] list)
        {
            if (list == null) return 0;
            for (int i = 0; i < list.Length; i++)
                if (list[i] == val) return i + 1;
            return 0;
        }

        private static bool EqualsI(string a, string b)
        {
            return string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);
        }

        private static int TryParseInt(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;
            int v;
            return int.TryParse(s.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static double TryParseDouble(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;
            double v;
            return double.TryParse(s.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static string Fmt(double v)
        {
            return Math.Round(v, 4).ToString("0.####", CultureInfo.InvariantCulture);
        }

        private static string Num(double v)
        {
            return Fmt(v).Replace(".", ",");
        }
    }
}