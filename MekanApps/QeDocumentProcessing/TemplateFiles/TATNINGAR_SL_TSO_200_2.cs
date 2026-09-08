using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class TATNINGAR_SL_TSO_200_2 : ITemplateCalculations
    {
        private const string LB = "<<LineBreak>>";

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req?.ProductDesignation ?? "";
            string maskinVal = req?.MachineNumber ?? "";

            string tmpFormat = subject.Trim().ToUpperInvariant();
            string tmpBet = tmpFormat.Replace(".", ",");

            bool tmpSlash = tmpBet.Contains("/");
            bool tmpVZ = tmpBet.Contains("VZ");
            bool tmpVZ2A6 = tmpBet.Contains("VZ2A6");
            bool tmpV21 = tmpBet.Contains("V21");
            bool tmpV22 = tmpBet.Contains("V22");
            bool tmpV212 = tmpBet.Contains("V21-2");
            bool tmpV213 = tmpBet.Contains("V21-3");
            bool tmpV = tmpVZ || tmpV212 || tmpV213 || tmpV22 || tmpV21;

            string[] parts = tmpBet.Split(new[] { ' ', '/', ',', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);

            string tmpBet1a = parts.Length > 0 ? parts[0] : "";
            string tmpBet2a = parts.Length > 1 ? parts[1] : "";
            string tmpBet3a = parts.Length > 2 ? parts[2] : "";
            string tmpBet4a = parts.Length > 3 ? parts[3] : "";
            string tmpBet5a = parts.Length > 4 ? parts[4] : "";
            string tmpBet6a = parts.Length > 5 ? parts[5] : "";
            string tmpBet7a = parts.Length > 6 ? parts[6] : "";

            bool tmpNull1 = string.IsNullOrEmpty(tmpBet1a);
            bool tmpNull2 = string.IsNullOrEmpty(tmpBet2a);
            bool tmpNull3 = string.IsNullOrEmpty(tmpBet3a);
            bool tmpNull4 = string.IsNullOrEmpty(tmpBet4a);
            bool tmpNull5 = string.IsNullOrEmpty(tmpBet5a);
            bool tmpNull6 = string.IsNullOrEmpty(tmpBet6a);
            bool tmpNull7 = string.IsNullOrEmpty(tmpBet7a);

            string tmpBet1b = tmpNull1 ? "0" : tmpBet1a;
            string tmpBet2b = tmpNull2 ? "0" : tmpBet2a;
            string tmpBet3b = tmpNull3 ? "0" : tmpBet3a;
            string tmpBet4b = tmpNull4 ? "0" : tmpBet4a;
            string tmpBet5b = tmpNull5 ? "0" : tmpBet5a;
            string tmpBet6b = tmpNull6 ? "0" : tmpBet6a;
            string tmpBet7b = tmpNull7 ? "0" : tmpBet7a;

            bool tmpBet1c = IsNumeric(tmpBet1b);
            bool tmpBet2c = IsNumeric(tmpBet2b);
            bool tmpBet3c = IsNumeric(tmpBet3b);
            bool tmpBet4c = IsNumeric(tmpBet4b);
            bool tmpBet5c = IsNumeric(tmpBet5b);
            bool tmpBet6c = IsNumeric(tmpBet6b);
            bool tmpBet7c = IsNumeric(tmpBet7b);

            object tmpBet1 = tmpBet1c && tmpBet1b != "0" ? ParseDouble(tmpBet1a) : (object)tmpBet1a;
            object tmpBet2 = tmpBet2c && tmpBet2b != "0" ? ParseDouble(tmpBet2a) : (object)tmpBet2a;
            object tmpBet3 = tmpBet3c && tmpBet3b != "0" ? ParseDouble(tmpBet3a) : (object)tmpBet3a;
            object tmpBet4 = tmpBet4c && tmpBet4b != "0" ? ParseDouble(tmpBet4a) : (object)tmpBet4a;
            object tmpBet5 = tmpBet5c ? ParseDouble(tmpBet5a) : (object)tmpBet5a;
            object tmpBet6 = tmpBet6c ? ParseDouble(tmpBet6a) : (object)tmpBet6a;
            object tmpBet7 = tmpBet7c ? ParseDouble(tmpBet7a) : (object)tmpBet7a;

            string tmpSerie = Left(LotusText(tmpBet3), 1);
            double tmpTyp = ParseDouble(Right(LotusText(tmpBet3), 2));
            string tmpVariant = LotusText(tmpBet4);

            string[] tmpTypValues = { "17", "18", "20", "22", "24", "26", "28", "30", "32", "34", "36", "38", "40", "44", "48" };
            int tmpTypLista = Array.IndexOf(tmpTypValues, Convert.ToInt32(tmpTyp).ToString(CultureInfo.InvariantCulture)) + 1;

            if (tmpTypLista <= 0) throw new InvalidOperationException("TmpTyp '" + Smart(tmpTyp) + "' finns inte i TmpTypLista. Subject: " + subject);

            double tmpBet4Number = ParseDouble(LotusText(tmpBet4));
            double tmpBet5Number = ParseDouble(LotusText(tmpBet5));
            double tmpBet6Number = ParseDouble(LotusText(tmpBet6));
            double tmpBet7Number = ParseDouble(LotusText(tmpBet7));

            bool tmpTum = tmpSlash && !tmpV;
            double tmpTumMm = tmpTum ? Math.Round((tmpBet4Number * 25.4) + ((tmpBet5Number / tmpBet6Number) * 25.4), 2) : 0;

            string tmpListaD13 = tmpTyp == 17 ? "{90:98:108}" : tmpTyp == 18 ? "{95:103:113}" : tmpTyp == 20 ? "{107:115:125}" : tmpTyp == 22 ? "{118:126:136}" : tmpTyp == 24 ? "{128:139:149}" : tmpTyp == 26 ? "{138:149:159}" : tmpTyp == 28 ? "{150:160:170}" : tmpTyp == 30 ? "{160:175:187}" : tmpTyp == 32 ? "{170:187:199}" : tmpTyp == 34 ? "{182:200:212}" : tmpTyp == 36 ? "{192:209:221}" : tmpTyp == 38 ? "{202:220:232}" : tmpTyp == 40 ? "{214:234:246}" : tmpTyp == 44 ? "{233:255:267}" : tmpTyp == 48 ? "{255:280:292}" : "{0:0:0}";

            string tmpListaD46 = tmpTyp == 17 ? "{120:96:110}" : tmpTyp == 18 ? "{128:100:114}" : tmpTyp == 20 ? "{142:112:127}" : tmpTyp == 22 ? "{152:120:137}" : tmpTyp == 24 ? "{168:135:154}" : tmpTyp == 26 ? "{175:145:162}" : tmpTyp == 28 ? "{200:160:182}" : tmpTyp == 30 ? "{210:170:190}" : tmpTyp == 32 ? "{225:178:202}" : tmpTyp == 34 ? "{234:195:216}" : tmpTyp == 36 ? "{250:205:230}" : tmpTyp == 38 ? "{270:220:247}" : tmpTyp == 40 ? "{280:227:257}" : tmpTyp == 44 ? "{305:255:285}" : tmpTyp == 48 ? "{325:270:300}" : "{0:0:0}";

            double tmpd = tmpTyp == 36 ? ((tmpTyp * 10) / 2) - 3 : ((tmpTyp * 10) / 2) - 2;

            kv["SumD"] = "(d) " + Smart(tmpd);
            kv["Sumda"] = Smart(tmpd);
            kv["SumDTol"] = tmpd < 50.1 ? "+ 0.025" : tmpd < 80.1 ? "+ 0.030" : tmpd < 120.1 ? "+ 0.035" : tmpd < 180.1 ? "+ 0.040" : tmpd < 250.1 ? "+ 0.046" : tmpd < 315.1 ? "+ 0.052" : tmpd < 400.1 ? "+ 0.057" : "+ 0.063";
            kv["SumDTolN"] = "+ 0";

            string tmpD1 = ListItem(tmpListaD13, 1);

            kv["SumD1"] = tmpSerie == "2" ? "-" : "(D1) " + tmpD1;
            kv["SumD1Tol"] = "+ 0.5";
            kv["SumD1TolN"] = "- 0";

            string tmpD2 = ListItem(tmpListaD13, 2);

            kv["SumD2"] = tmpD2 == "x" ? "-" : "(D2) " + tmpD2;
            kv["SumD2Tol"] = tmpD2 == "x" ? "-" : ParseDouble(tmpD2) < 120.1 ? "± 0.3" : "± 0.5";

            string tmpD3 = ListItem(tmpListaD13, 3);
            bool hideD3 = tmpBet4Number == 2 && tmpBet7Number != 1;

            kv["SumD3"] = hideD3 ? "-" : tmpD3 == "x" ? "" : "(D3) " + tmpD3;
            kv["SumD3Tol"] = hideD3 ? "-" : tmpD3 == "x" ? "" : ParseDouble(tmpD3) < 120.1 ? "± 0.3" : "± 0.5";

            string tmpD4 = ListItem(tmpListaD46, 1);

            kv["SumD4"] = "(D4) " + tmpD4;
            kv["SumD4Tol"] = ParseDouble(tmpD4) < 120 ? "± 0.3" : "± 0.5";

            string tmpD5 = ListItem(tmpListaD46, 2);
            double tmpD5Number = ParseDouble(tmpD5);

            kv["SumD5"] = "(D5) " + tmpD5;
            kv["SumD5a"] = tmpD5;
            kv["SumD5Tol"] = "+ 0";
            kv["SumD5TolN"] = tmpD5Number < 80.1 ? "- 0.190" : tmpD5Number < 120.1 ? "- 0.220" : tmpD5Number < 180.1 ? "- 0.250" : tmpD5Number < 250.1 ? "- 0.290" : tmpD5Number < 315.1 ? "- 0.320" : "- 0.360";

            string tmpD6 = ListItem(tmpListaD46, 3);
            double tmpD6Number = ParseDouble(tmpD6);

            kv["SumD6"] = "(D6) " + tmpD6;
            kv["SumD6Tol"] = tmpD6Number < 120.1 ? "+ 0.220" : tmpD6Number < 180.1 ? "+ 0.250" : tmpD6Number < 250.1 ? "+ 0.290" : tmpD6Number < 315.1 ? "+ 0.320" : "+ 0.360";
            kv["SumD6TolN"] = "- 0";

            string[] tmpaLista = tmpVZ2A6 ? new[] { "t17", "t18", "t20", "t22", "100", "95", "t28", "t30", "106", "t34", "t36", "t38", "t40", "t44", "t48" } : (tmpBet4Number == 1 || tmpBet7Number == 1 || tmpSerie == "2") ? new[] { "72", "75", "80", "88", "109", "103", "106", "103,5", "118", "132", "137", "139", "140", "148", "150" } : new[] { "52,5", "55,5", "58,5", "66", "87", "80", "82", "77,5", "90", "103,5", "107", "108", "108", "113", "113" };

            string tmpa = GetArrayItem(tmpaLista, tmpTypLista);

            kv["Suma"] = "(a) " + tmpa;
            kv["SumaTol"] = "+ 0";
            kv["SumaTolN"] = "- 0.1";

            string[] tmpbLista = tmpBet4Number == 1 || tmpBet7Number == 1 ? new[] { "52", "55", "56", "60", "75", "71", "74", "73,5", "84", "91,5", "96,5", "98,5", "99,5", "108", "110" } : tmpSerie == "2" && tmpBet4Number == 2 ? new[] { "54", "57", "60", "66", "80", "74", "81", "77", "89", "95", "100", "102", "110", "111", "117" } : new[] { "x", "x", "x", "x", "80", "74", "x", "x", "x", "95", "98,5", "99,5", "x", "x", "x" };

            string tmpb = GetArrayItem(tmpbLista, tmpTypLista);

            kv["Sumb"] = tmpb == "x" ? "" : "(b) " + tmpb;
            kv["SumbTol"] = tmpb == "x" ? "" : ParseDouble(tmpa) < 120.1 ? "± 0.3" : "± 0.5";

            string[] tmpcLista = tmpBet4Number == 1 || tmpBet7Number == 1 ? new[] { "49", "52", "53", "57", "72", "67", "70", "68,5", "80", "87", "92", "94", "95", "101", "103" } : new[] { "49", "52", "54", "60", "72", "67", "70", "68,5", "81", "87", "92", "94", "96", "101", "104" };

            string tmpc = GetArrayItem(tmpcLista, tmpTypLista);

            kv["Sumc"] = "(c) " + tmpc;
            kv["SumcTol"] = "+ 0.2";
            kv["SumcTolN"] = "- 0";

            string[] tmpeLista = tmpSerie == "5" && (tmpBet7Number == 2 || tmpBet4Number == 2) ? new[] { "36", "39", "37", "37,5", "47,5", "44", "47", "46,5", "55", "58", "63", "65", "65,5", "71,5", "74" } : new[] { "36", "39", "38,5", "38,5", "47,5", "44", "47", "46,5", "55,5", "58", "63", "65", "66", "71,5", "74" };

            string tmpe = GetArrayItem(tmpeLista, tmpTypLista);

            kv["Sume"] = "(e) " + tmpe;
            kv["SumeTol"] = "+ 0.2";
            kv["SumeTolN"] = "- 0";

            string[] tmpfLista = tmpBet4Number == 1 || tmpBet7Number == 1 ? new[] { "12", "10", "8", "8", "11", "8", "11", "16", "15", "15", "23", "25", "25", "25", "20" } : new[] { "3", "4", "4", "6", "5", "5", "5", "5", "5", "8", "10", "10", "7", "5", "8" };

            string tmpf = tmpSerie == "2" ? "x" : GetArrayItem(tmpfLista, tmpTypLista);

            kv["Sumf"] = tmpf == "x" ? "" : "(f) " + tmpf;
            kv["SumfTol"] = tmpf == "x" ? "" : ParseDouble(tmpf) < 6.1 ? "± 0.1" : ParseDouble(tmpf) < 30.1 ? "± 0.2" : "± 0.3";

            string tmpg = tmpSerie == "2" ? "" : tmpTyp < 27 ? "M6" : tmpTyp < 33 ? "M8" : "M10";

            kv["Sumg"] = "(g) " + tmpg;
            kv["Sumg1"] = tmpg;

            string tmph = tmpSerie == "2" ? "x" : tmpTyp < 33 ? "8" : "12";

            kv["Sumh"] = tmph == "x" ? "" : "(h) " + tmph;
            kv["SumhTol"] = tmph == "x" ? "" : ParseDouble(tmph) < 6.1 ? "± 0.1" : ParseDouble(tmph) < 30.1 ? "± 0.2" : "± 0.3";

            string[] tmpkLista = { "20", "22", "21", "22", "26", "22", "24", "24", "34", "36", "36,5", "37", "40", "44", "45" };

            string tmpk = GetArrayItem(tmpkLista, tmpTypLista);
            double tmpkNumber = ParseDouble(tmpk);

            kv["Sumk"] = "(k) " + tmpk;
            kv["SumkTol"] = tmpkNumber < 6.1 ? "± 0.1" : tmpkNumber < 30.1 ? "± 0.2" : "± 0.3";

            double tmpm = tmpTyp < 33 ? 3 : tmpTyp == 34 ? 4 : 5;

            kv["Summ"] = "(m) " + Smart(tmpm);
            kv["SummTol"] = tmpm < 6.1 ? "± 0.1" : tmpm < 30.1 ? "± 0.2" : "± 0.3";

            double tmpn = tmpSerie == "5" ? (tmpTyp < 33 ? 2 : tmpTyp == 34 ? 4 : 3) : (tmpTyp < 33 ? 2 : tmpTyp < 35 ? 4 : 5);

            kv["Sumn"] = Smart(tmpn) + "x45º";

            string tmpp = tmpBet4Number == 1 || tmpBet7Number == 1 ? (tmpTyp < 33 ? "10,5" : "13") : "x";

            kv["Sump"] = tmpp == "x" ? "" : "(p) " + tmpp;
            kv["SumpTol"] = tmpp == "x" ? "" : "+ 0.2";
            kv["SumpTolN"] = tmpp == "x" ? "" : "- 0";

            string tmpr = tmpBet4Number == 1 || tmpBet7Number == 1 ? (tmpTyp < 33 ? "6" : "15") : "x";

            kv["Sumr"] = tmpr == "x" ? "" : "(r) " + tmpr;
            kv["SumrTol"] = tmpr == "x" ? "" : ParseDouble(tmpr) < 6.1 ? "± 0.1" : ParseDouble(tmpr) < 30.1 ? "± 0.2" : "± 0.3";

            double tmpD7 = tmpn * 2;
            double tmpD7a = tmpd + tmpD7;

            kv["SumD7"] = "(D7) " + Smart(tmpD7a);
            kv["SumD7Tol"] = tmpD7 < 3.1 ? "± 0.2" : tmpD7 < 6.1 ? "± 0.5" : "± 1.0";

            kv["Sum60"] = "60º";
            kv["Sum60a"] = "60º";
            kv["Sum8"] = "8º ± 1º";
            kv["SumRa63"] = "6.3";
            kv["SumRa32"] = "3.2";
            kv["SumRa32a"] = "3.2";
            kv["SumRa32b"] = "3.2";
            kv["SumRa32c"] = "3.2";
            kv["SumRa32d"] = "3.2";
            kv["SumRa32e"] = "3.2";

            kv["SumRit"] = tmpVZ2A6 ? "7439473" : tmpSlash ? (tmpBet7Number == 1 ? "7439269" : "7439270") : (tmpBet4Number == 1 ? "7438712" : "7438713");
            kv["SumStämpel"] = "Stämplas: " + tmpFormat;

            bool machineEnabled = EqualsI(maskinVal, "Nakamura") || EqualsI(maskinVal, "LT-3000");
            string tmpMaskinValS1 = EqualsI(maskinVal, "Nakamura") ? "Nakamura" : EqualsI(maskinVal, "LT-3000") ? "LT-3000" : "";

            kv["SumMaskinValS1"] = "Maskin: " + tmpMaskinValS1;

            kv["SumF1_1"] = machineEnabled ? "1/3" : "";
            kv["SumF1_2"] = machineEnabled ? "1/3" : "";
            kv["SumF1_3"] = machineEnabled ? "1/2" : "";
            kv["SumF1_4"] = machineEnabled ? "1/3" : "";
            kv["SumF1_5"] = machineEnabled ? "1/1" : "";
            kv["SumF1_6"] = machineEnabled ? "1/2" : "";
            kv["SumF1_7"] = machineEnabled ? "1/2" : "";
            kv["SumF1_8"] = machineEnabled ? "1/3" : "";
            kv["SumF1_9"] = machineEnabled ? "1/1" : "";
            kv["SumF1_0"] = machineEnabled ? "1/3" : "";
            kv["SumF1_11"] = machineEnabled ? "1/3" : "";
            kv["SumF1_12"] = tmpSerie == "5" && machineEnabled ? "1/3" : "";
            kv["SumF1_13"] = tmpSerie == "5" && machineEnabled ? "1/3" : "";

            kv["SumD1_1"] = machineEnabled ? "Digitalt Skjutmått" : "";
            kv["SumD1_2"] = machineEnabled ? "Digitalt Skjutmått" : "";
            kv["SumD1_3"] = machineEnabled ? "Digitalt Skjutmått" : "";
            kv["SumD1_4"] = machineEnabled ? "Digitalt Skjutmått" : "";
            kv["SumD1_5"] = machineEnabled ? "UD-Apparat" : "";
            kv["SumD1_6"] = machineEnabled ? "Djupmått ev. med klocka" : "";
            kv["SumD1_7"] = machineEnabled ? "Digitalt Skjutmått" : "";
            kv["SumD1_8"] = machineEnabled ? "Digitalt Skjutmått" : "";
            kv["SumD1_9"] = machineEnabled ? "Okulärkontroll" : "";
            kv["SumD1_0"] = machineEnabled ? "Ytjämnhetsmätare" : "";
            kv["SumD1_11"] = machineEnabled ? "Digitalt Skjutmått" : "";
            kv["SumD1_12"] = tmpSerie == "5" && machineEnabled ? "Digitalt Skjutmått" : "";
            kv["SumD1_13"] = tmpSerie == "5" && machineEnabled ? kv["Sumg1"] + " Tolk" : "";

            kv["SumAF1_1"] = "";
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = machineEnabled ? "Klove/Ring " + kv["Sumda"] + ", mät båda sidor" : "";
            kv["SumAF1_6"] = machineEnabled ? "Passbitar" : "";
            kv["SumAF1_7"] = "";
            kv["SumAF1_8"] = "";
            kv["SumAF1_9"] = machineEnabled ? kv["Sum60"] : "";
            kv["SumAF1_0"] = "";
            kv["SumAF1_11"] = "";
            kv["SumAF1_12"] = "";
            kv["SumAF1_13"] = tmpSerie == "5" && machineEnabled ? "120° delning" : "";

            kv["SumTextS1"] = "Okulärkontroll: Grader, frifläcker, slagmärken, repor, valkar, ytjämnhet och faser" + LB + "Bearbetas Ra " + kv["SumRa63"] + " där annat ej anges, skarpa kanter avgradas.";

            return kv;
        }

        private static bool EqualsI(string a, string b) => string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);

        private static bool IsNumeric(string value) => !string.IsNullOrWhiteSpace(value) && double.TryParse(value.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out _);

        private static double ParseDouble(string value) => string.IsNullOrWhiteSpace(value) ? 0 : double.TryParse(value.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double result) ? result : 0;

        private static string Smart(double value) => Math.Abs(value % 1) < 0.0000001 ? value.ToString("F0", CultureInfo.InvariantCulture) : value.ToString("0.###", CultureInfo.InvariantCulture).Replace(".", ",");

        private static string LotusText(object value)
        {
            if (value == null) return "";
            if (value is double d) return Smart(d);
            if (value is float f) return Smart(f);
            if (value is decimal m) return Smart((double)m);
            return value.ToString() ?? "";
        }

        private static string Left(string value, int length) => string.IsNullOrEmpty(value) || length <= 0 ? "" : value.Substring(0, Math.Min(length, value.Length));

        private static string Right(string value, int length) => string.IsNullOrEmpty(value) || length <= 0 ? "" : value.Length <= length ? value : value.Substring(value.Length - length);

        private static string ListItem(string list, int position)
        {
            if (string.IsNullOrWhiteSpace(list) || position < 1) return "";
            string[] values = list.Trim().TrimStart('{').TrimEnd('}').Split(':');
            return values.Length >= position ? values[position - 1] : "";
        }

        private static string GetArrayItem(string[] values, int lotusPosition) => values == null || lotusPosition < 1 || lotusPosition > values.Length ? "" : values[lotusPosition - 1];
    }
}
