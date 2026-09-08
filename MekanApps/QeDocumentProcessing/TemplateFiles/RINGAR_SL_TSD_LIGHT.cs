using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class RINGAR_SL_TSD_LIGHT : ITemplateCalculations
    {
        private static readonly string[] Machines = new[]
        {
            "Nakamura", "LB:45"
        };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;
            List<Bookmark> bm = req != null ? req.Bookmarks : null;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null, subject);

            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");

            bool tmpV21 = ContainsI(tmpBet, "V21");
            bool tmpV22 = ContainsI(tmpBet, "V22");
            bool tmpV21_2 = ContainsI(tmpBet, "V21-2");
            bool tmpV21_3 = ContainsI(tmpBet, "V21-3");
            bool tmpV = tmpV21_2 || tmpV21_3 || tmpV22 || tmpV21;

            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string bet3 = tokens.Length > 2 ? tokens[2] : string.Empty;
            string bet4 = tokens.Length > 3 ? tokens[3] : string.Empty;
            string bet6 = tokens.Length > 5 ? tokens[5] : string.Empty;

            double bet3Num = ParseTokenDouble(bet3);
            double bet4Num = ParseTokenDouble(bet4);
            double bet6Num = ParseTokenDouble(bet6);

            double[] dListV = null;
            if (Math.Abs(bet3Num - 3160) < 0.0001 && Math.Abs(bet6Num - 230) < 0.0001)
                dListV = new[] { 358.5, 342.5, 311.0, 300.0, 295.0, 284.0, 246.0, 230.0, 234.8, 305.0 };

            double[] dList = null;
            if (Math.Abs(bet3Num - 3168) < 0.0001 && Math.Abs(bet4Num - 260) < 0.0001)
                dList = new[] { 398.0, 381.5, 350.0, 339.0, 333.8, 324.0, 276.0, 260.0, 264.8, 0.0 };

            double[] bList = new[] { 42.0, 4.0, 30.0, 7.0, 29.0, 33.7, 4.5, 22.5, 19.5, 16.5, 2.0 };

            double[] rList = (Math.Abs(bet3Num - 3168) < 0.0001 || Math.Abs(bet3Num - 3160) < 0.0001)
                ? new[] { 4.0, 1.0, 0.2, 1.6 }
                : new[] { 1.0, 2.0, 3.0 };

            kv["SumG"] = "1x45º";
            kv["SumG1"] = "1x45º";
            kv["SumG2"] = "1x45º";
            kv["SumG3"] = "45º";
            kv["SumG4"] = "60º";

            kv["SumR"] = "R 0.2";

            double inR1 = GetDouble(bm, "R1");
            double inR2 = GetDouble(bm, "R2");
            double inR3 = GetDouble(bm, "R3");
            double inR4 = GetDouble(bm, "R4");

            double tmpR1 = rList.Length > 0 ? rList[0] : 0;
            double tmpR2 = rList.Length > 1 ? rList[1] : 0;
            double tmpR3 = rList.Length > 2 ? rList[2] : 0;
            double tmpR4 = rList.Length > 3 ? rList[3] : 0;

            kv["SumR1"] = inR1 == 0 ? ("R " + FormatDot(tmpR1)) : ("R " + FormatDot(inR1));
            kv["SumR1Tol"] = inR1 == 0 ? RadiusTol(tmpR1) : "";

            kv["SumR2"] = inR2 == 0 ? ("R" + FormatDot(tmpR2) + " max (2X)") : ("R " + FormatDot(inR2));

            kv["SumR3"] = inR3 == 0 ? ("R " + FormatDot(tmpR3)) : ("R " + FormatDot(inR3));
            kv["SumR3Tol"] = inR3 == 0 ? RadiusTol(tmpR3) : "";

            kv["SumR4"] = inR4 == 0 ? ("R " + FormatDot(tmpR4)) : ("R " + FormatDot(inR4));

            double inD = GetDouble(bm, "Ø D");
            double inD1 = GetDouble(bm, "Ø D1");
            double inD2 = GetDouble(bm, "Ø D2");
            double inD3 = GetDouble(bm, "Ø D3");
            double inD4 = GetDouble(bm, "Ø D4");
            double inD5 = GetDouble(bm, "Ø D5");
            double inD6 = GetDouble(bm, "Ø D6");
            double inD7 = GetDouble(bm, "Ø D7");
            double inD8 = GetDouble(bm, "Ø D8");
            double inD9 = GetDouble(bm, "Ø D9");
            double inD10 = GetDouble(bm, "Ø D10");

            double tmpD = inD == 0 ? (tmpV ? GetListValue(dListV, 0) : GetListValue(dList, 0)) : inD;
            double tmpD1v = inD1 == 0 ? (tmpV ? GetListValue(dListV, 1) : GetListValue(dList, 1)) : inD1;
            double tmpD2v = inD2 == 0 ? (tmpV ? GetListValue(dListV, 2) : GetListValue(dList, 2)) : inD2;
            double tmpD3v = inD3 == 0 ? (tmpV ? GetListValue(dListV, 3) : GetListValue(dList, 3)) : inD3;
            double tmpD4v = inD4 == 0 ? (tmpV ? GetListValue(dListV, 4) : GetListValue(dList, 4)) : inD4;
            double tmpD5v = inD5 == 0 ? (tmpV ? GetListValue(dListV, 5) : GetListValue(dList, 5)) : inD5;
            double tmpD6v = inD6 == 0 ? (tmpV ? GetListValue(dListV, 6) : GetListValue(dList, 6)) : inD6;
            double tmpD7v = inD7 == 0 ? (tmpV ? GetListValue(dListV, 7) : GetListValue(dList, 7)) : inD7;
            double tmpD9v = inD9 == 0 ? (tmpV ? GetListValue(dListV, 8) : GetListValue(dList, 8)) : inD9;
            double tmpD10v = inD10 == 0 ? (tmpV ? GetListValue(dListV, 9) : GetListValue(dList, 9)) : inD10;

            double tmpD8v = inD8 == 0 ? ((tmpD5v - tmpD6v) / 2.0) : inD8;

            kv["SumD"] = "(D) " + FormatDot(tmpD);
            kv["SumDTol"] = "± " + FormatDot1(GeneralTolSL(tmpD));

            kv["SumD1"] = tmpD1v == 0 ? "Ritningsmått saknas" : "(D1) " + FormatDot(tmpD1v);
            kv["SumD1Tol"] = "± " + FormatDot1(GeneralTolSL(tmpD1v));

            kv["SumD2"] = "(D2) " + FormatDot(tmpD2v);
            kv["SumD2Tol"] = "+ 0";
            kv["SumD2TolN"] = D2h12NegTolText(tmpD2v);

            kv["SumD3"] = "(D3) " + FormatDot(tmpD3v);
            kv["SumD3Tol"] = "+ 0";
            kv["SumD3TolN"] = D3h10NegTolText(tmpD3v);

            kv["SumD4"] = "(D4) " + FormatDot(tmpD4v);
            kv["SumD4Tol"] = "± " + FormatDot1(GeneralTolSL(tmpD4v));

            kv["SumD5"] = "(D5) " + FormatDot(tmpD5v);
            kv["SumD5Tol"] = GeneralTolSLTextWithEmptyBelow05(tmpD5v);

            kv["SumD6"] = "(D6) " + FormatDot(tmpD6v);
            kv["SumD6Tol"] = "± " + FormatDot1(GeneralTolSL(tmpD6v));

            kv["SumD7"] = "(D7) " + FormatDot(tmpD7v);
            kv["SumD7Tol"] = D7E10PosTolText(tmpD7v);
            kv["SumD7TolN"] = D7E10NegTolText(tmpD7v);

            kv["SumD8"] = "(D8) " + FormatDot(tmpD8v);
            kv["SumD8Tol"] = kv["SumD6Tol"];

            kv["SumD9"] = "2x Ø (D9) " + FormatDot(tmpD9v);
            kv["SumD9Tol"] = "+ " + FormatDot3(D9H10Tol(tmpD9v));
            kv["SumD9TolN"] = "- " + FormatDot3(0.0);

            kv["SumD10"] = tmpD10v == 0 ? "Inget mått" : "(D10) " + FormatDot(tmpD10v);
            kv["SumD10Tol"] = tmpD10v == 0 ? "Inget mått" : ("± " + FormatDot1(GeneralTolSL(tmpD10v)));

            double inB = GetDouble(bm, "B");
            double inB1 = GetDouble(bm, "B1");
            double inB2 = GetDouble(bm, "B2");
            double inB3 = GetDouble(bm, "B3");
            double inB4 = GetDouble(bm, "B4");
            double inB5 = GetDouble(bm, "B5");
            double inB6 = GetDouble(bm, "B6");
            double inB7 = GetDouble(bm, "B7");
            double inB8 = GetDouble(bm, "B8");
            double inB9 = GetDouble(bm, "B9");
            double inB10 = GetDouble(bm, "B10");

            double tmpB = inB == 0 ? GetListValue(bList, 0) : inB;
            double tmpB1 = inB1 == 0 ? GetListValue(bList, 1) : inB1;
            double tmpB2 = inB2 == 0 ? GetListValue(bList, 2) : inB2;
            double tmpB3 = inB3 == 0 ? GetListValue(bList, 3) : inB3;
            double tmpB4 = inB4 == 0 ? GetListValue(bList, 4) : inB4;
            double tmpB5 = inB5 == 0 ? GetListValue(bList, 5) : inB5;
            double tmpB6 = inB6 == 0 ? GetListValue(bList, 6) : inB6;
            double tmpB7 = inB7 == 0 ? GetListValue(bList, 7) : inB7;
            double tmpB8 = inB8 == 0 ? GetListValue(bList, 8) : inB8;
            double tmpB9 = inB9 == 0 ? GetListValue(bList, 9) : inB9;
            double tmpB10 = inB10 == 0 ? GetListValue(bList, 10) : inB10;

            kv["SumB"] = "(B) " + FormatDot(tmpB);
            kv["SumBTol"] = "± " + FormatDot1(GeneralTolSL(tmpB));

            kv["SumB1"] = "2x (B1) " + FormatDot(tmpB1);
            kv["SumB1Tol"] = "+ 0.2";
            kv["SumB1TolN"] = "- 0";

            kv["SumB2"] = "(B2) " + FormatDot(tmpB2);
            kv["SumB2Tol"] = GeneralTolSLTextWithEmptyBelow05(tmpB2);

            kv["SumB3"] = "(B3) " + FormatDot(tmpB3);
            kv["SumB3Tol"] = "± " + FormatDot1(GeneralTolSL(tmpB3));

            kv["SumB4"] = "(B4) " + FormatDot(tmpB4);
            kv["SumB4Tol"] = "± " + FormatDot1(GeneralTolSL(tmpB4));

            kv["SumB5"] = "(B5) " + FormatDot(tmpB5);
            kv["SumB5Tol"] = "± " + FormatDot1(GeneralTolSL(tmpB5));

            kv["SumB6"] = "(B6) " + FormatDot(tmpB6);
            kv["SumB6Tol"] = "± " + FormatDot1(GeneralTolSL(tmpB6));

            kv["SumB7"] = "(B7) " + FormatDot(tmpB7);
            kv["SumB7Tol"] = "± " + FormatDot1(GeneralTolSL(tmpB7));

            kv["SumB8"] = "(B8) " + FormatDot(tmpB8);
            kv["SumB8Tol"] = "± " + FormatDot1(GeneralTolSL(tmpB8));

            kv["SumB9"] = "(B9) " + FormatDot(tmpB9);
            kv["SumB9Tol"] = "± " + FormatDot1(GeneralTolSL(tmpB9));

            kv["SumB10"] = "(B10) " + FormatDot(tmpB10);
            kv["SumB10Tol"] = "± " + FormatDot1(GeneralTolSL(tmpB10));

            kv["SumStämpel"] = tmpBet;

            kv["SumRa"] = "3.2";
            kv["SumRa1"] = "6.3";

            SumMachineSection(kv, maskinVal);

            kv["SumTextS1"] = "Okulärkontroll Grader, frifläcker, slagmärken, repor, valkar, ytjämnhet och faser, skarpa kanter avgradas.";
            kv["SumRit"] = tmpBet;

            return kv;
        }

        private static void SumMachineSection(Dictionary<string, string> kv, string maskinVal)
        {
            bool match = IsMachine(maskinVal);

            string machineText = EqualsI(maskinVal, "Nakamura") ? "Nakamura" :
                                 EqualsI(maskinVal, "LB:45") ? "LB:45" : "";

            string tmpMaskinValS1 = "Svarvning - " + machineText;
            kv["SumMaskinValS1"] = ("Maskin: " + tmpMaskinValS1).Trim();

            kv["SumF1_1"] = match ? "1/1" : "";
            kv["SumF1_2"] = match ? "Inst." : "";
            kv["SumF1_3"] = match ? "Inst." : "";
            kv["SumF1_4"] = match ? "1/3" : "";
            kv["SumF1_5"] = match ? "1/5" : "";

            kv["SumD1_1"] = match ? "UD-Apparat/mikrometer" : "";
            kv["SumD1_2"] = match ? "Djupmått" : "";
            kv["SumD1_3"] = match ? "Radielyra/mallar" : "";
            kv["SumD1_4"] = match ? "Ytjämnhetsmätare" : "";
            kv["SumD1_5"] = match ? "Skjutmått" : "";

            kv["SumAF1_1"] = match ? "Inställd med tillhörande klove/ring" : "";
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = match ? ("Ytjämnhet övriga ytor " + (kv.ContainsKey("SumRa1") ? kv["SumRa1"] : "")) : "";
            kv["SumAF1_5"] = "";
        }

        private static string ComputePopup(string published, string subject)
        {
            if (string.IsNullOrWhiteSpace(published)) return "";

            DateTime pubDt;
            if (!DateTime.TryParse(published, out pubDt)) return "";

            int dagar = 14;
            DateTime validTill = pubDt.AddDays(dagar);

            if (DateTime.Today <= validTill.Date)
                return "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom " +
                       dagar.ToString(CultureInfo.InvariantCulture) +
                       " dagar)\n\nInformation om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument\n\nPopupruta aktiv till " +
                       validTill.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            return "";
        }

        private static bool IsMachine(string maskinVal)
        {
            if (string.IsNullOrEmpty(maskinVal)) return false;
            for (int i = 0; i < Machines.Length; i++)
                if (string.Equals(Machines[i], maskinVal, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }

        private static double GeneralTolSL(double v)
        {
            if (v > 400) return 0.8;
            if (v > 120) return 0.5;
            if (v > 30) return 0.3;
            if (v > 6) return 0.2;
            return 0.1;
        }

        private static string GeneralTolSLTextWithEmptyBelow05(double v)
        {
            if (v > 400) return "± 0.8";
            if (v > 120) return "± 0.5";
            if (v > 30) return "± 0.3";
            if (v > 6) return "± 0.2";
            if (v > 0.5) return "± 0.1";
            return "";
        }

        private static string RadiusTol(double r)
        {
            if (r > 6) return "± 1";
            if (r > 3) return "± 0.5";
            if (r > 0.5) return "± 0.2";
            return "";
        }

        private static string D2h12NegTolText(double d)
        {
            if (d < 181) return "- 0.400";
            if (d < 251) return "- 0.460";
            if (d < 316) return "- 0.520";
            if (d < 401) return "- 0.570";
            return "- 0.630";
        }

        private static string D3h10NegTolText(double d)
        {
            if (d < 181) return "- 0.160";
            if (d < 251) return "- 0.185";
            if (d < 316) return "- 0.210";
            if (d < 401) return "- 0.230";
            return "- 0.250";
        }

        private static string D7E10PosTolText(double d)
        {
            if (d < 181) return "+ 0.245";
            if (d < 251) return "+ 0.285";
            if (d < 316) return "+ 0.320";
            if (d < 401) return "+ 0.355";
            return "+ 0.385";
        }

        private static string D7E10NegTolText(double d)
        {
            if (d < 181) return "+ 0.085";
            if (d < 251) return "+ 0.100";
            if (d < 316) return "+ 0.110";
            if (d < 401) return "+ 0.125";
            return "+ 0.135";
        }

        private static double D9H10Tol(double d)
        {
            if (d < 181) return 0.16;
            if (d < 251) return 0.185;
            if (d < 316) return 0.21;
            if (d < 401) return 0.23;
            return 0.25;
        }

        private static double GetListValue(double[] list, int index)
        {
            if (list == null) return 0;
            if (index < 0 || index >= list.Length) return 0;
            return list[index];
        }

        private static string FormatDot(double v)
        {
            return v.ToString(CommonFunctions.Culture).Replace(",", ".");
        }

        private static string FormatDot3(double v)
        {
            return v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
        }

        private static string FormatDot1(double v)
        {
            return v.ToString("F1", CommonFunctions.Culture).Replace(",", ".");
        }

        private static double GetDouble(List<Bookmark> bm, string key)
        {
            string raw = GetString(bm, key);
            if (string.IsNullOrWhiteSpace(raw)) return 0;
            raw = raw.Trim().Replace(".", ",");
            double v;
            return double.TryParse(raw, NumberStyles.Any, CommonFunctions.Culture, out v) ? v : 0;
        }

        private static string GetString(List<Bookmark> bm, string key)
        {
            if (bm == null || string.IsNullOrWhiteSpace(key)) return "";
            for (int i = 0; i < bm.Count; i++)
            {
                Bookmark b = bm[i];
                if (b != null && string.Equals(b.BookmarkName ?? "", key, StringComparison.OrdinalIgnoreCase))
                    return b.BookmarkValue ?? "";
            }
            return "";
        }

        private static bool EqualsI(string a, string b)
            => string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);

        private static bool ContainsI(string haystack, string needle)
        {
            if (string.IsNullOrEmpty(haystack) || string.IsNullOrEmpty(needle)) return false;
            return haystack.IndexOf(needle, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static double ParseTokenDouble(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;
            string raw = s.Trim().Replace(".", ",");
            double v;
            return double.TryParse(raw, NumberStyles.Any, CommonFunctions.Culture, out v) ? v : 0;
        }
    }
}