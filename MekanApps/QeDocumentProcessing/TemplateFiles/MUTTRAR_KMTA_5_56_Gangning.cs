using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class MUTTRAR_KMTA_5_56_Gangning: ITemplateCalculations
    {
        private const string LB = "<<LineBreak>>";

        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var kv = new Dictionary<string, string>();

            string subject = request?.ProductDesignation ?? "";
            string maskinVal = request?.MachineNumber ?? "";

            kv["VaLPopUp"] = ComputePopup(request?.Published);

            string tmpFormat = subject.Trim().ToUpperInvariant();
            string tmpBet = tmpFormat.Replace(".", ",");

            int tmpCountB = tmpBet.Length;

            bool tmpSpecial = tmpCountB > 7;
            bool tmpSlash = tmpBet.Contains("/");
            bool tmpM24 = tmpBet.Contains("M24");
            bool tmpM80 = tmpBet.Contains("M80");

            string[] parts = tmpBet.Split(new[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);

            string tmpBet1a = parts.Length > 0 ? parts[0] : "";
            string tmpBet2a = parts.Length > 1 ? parts[1] : "";
            string tmpBet3a = parts.Length > 2 ? parts[2] : "";
            string tmpBet4a = parts.Length > 3 ? parts[3] : "";
            string tmpBet5a = parts.Length > 4 ? parts[4] : "";
            string tmpBet6a = parts.Length > 5 ? parts[5] : "";

            object tmpBet1 = ParseLotusValue(tmpBet1a);
            object tmpBet2 = ParseLotusValue(tmpBet2a);
            object tmpBet3 = ParseLotusValue(tmpBet3a);
            object tmpBet4 = ParseLotusValue(tmpBet4a);
            object tmpBet5 = ParseLotusValue(tmpBet5a);
            object tmpBet6 = ParseLotusValue(tmpBet6a);

            double tmpTyp = ToDouble(tmpBet2);

            double tmpD = tmpM80 ? 80 : tmpTyp / 2.0 * 10.0;

            string[] typList = { "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "22", "24", "26", "28", "30", "32", "34", "36", "38", "40", "42", "44", "48", "50", "56" };

            double tmpStmm = (tmpM24 || tmpM80) ? 1.5 : (tmpSpecial || tmpSlash) ? ParseDouble(Right(tmpBet3.ToString(), 1)) : (tmpD < 11 ? 0.75 : tmpD < 21 ? 1 : tmpD < 76 ? 1.5 : tmpD < 121 ? 2 : tmpD < 201 ? 3 : tmpD < 301 ? 4 : 5);

            kv["SumP"] = "(P) " + Smart(tmpStmm);

            string tmpG = tmpM24 ? $"M 24x{Smart(tmpStmm)}" : (tmpStmm == 0.75 || tmpStmm == 1 || tmpStmm == 1.5 || tmpStmm == 2 || tmpStmm == 3) ? $"M {Smart(tmpD)}x{Smart(tmpStmm)}" : $"Tr {Smart(tmpD)}x{Smart(tmpStmm)}";

            kv["SumG"] = tmpG;

            double tmpd2 = tmpD < 26 ? tmpD + 17 : tmpD < 41 ? tmpD + 18 : tmpD < 46 ? tmpD + 23 : tmpD < 56 ? tmpD + 20 : tmpD < 61 ? tmpD + 24 : tmpD < 66 ? tmpD + 23 : tmpD < 76 ? tmpD + 25 : tmpD < 111 ? tmpD + 30 : tmpD < 131 ? tmpD + 35 : tmpD < 151 ? tmpD + 40 : tmpD < 171 ? tmpD + 45 : tmpD < 191 ? tmpD + 50 : tmpD < 221 ? tmpD + 45 : tmpD < 281 ? tmpD + 50 : tmpD < 301 ? tmpD + 60 : tmpD < 361 ? tmpD + 70 : tmpD + 80;

            kv["Sumd2"] = "(d2) " + Smart(tmpd2);
            kv["Sumd2Tol"] = "+ 0 [3F]";
            kv["Sumd2TolN"] = (tmpd2 < 31 ? "- 0.130" : tmpd2 < 50 ? "- 0.160" : tmpd2 < 80 ? "- 0.190" : tmpd2 < 121 ? "- 0.220" : tmpd2 < 180 ? "- 0.250" : tmpd2 < 235 ? "- 0.290" : tmpd2 < 311 ? "- 0.320" : tmpd2 < 391 ? "- 0.360" : "- 0.400") + " [3F]";

            double tmpd3 = tmpD < 31 ? tmpD + 10 : tmpD < 41 ? tmpD + 12 : tmpD < 51 ? tmpD + 13 : tmpD < 66 ? tmpD + 15 : tmpD < 76 ? tmpD + 16 : tmpD < 86 ? tmpD + 17 : tmpD < 91 ? tmpD + 20 : tmpD < 96 ? tmpD + 19 : tmpD < 101 ? tmpD + 20 : tmpD < 121 ? tmpD + 22 : tmpD < 141 ? tmpD + 26 : tmpD < 161 ? tmpD + 30 : tmpD < 191 ? tmpD + 35 : tmpD < 201 ? tmpD + 37 : tmpD < 221 ? tmpD + 34 : tmpD < 281 ? tmpD + 39 : tmpD < 301 ? tmpD + 49 : tmpD < 361 ? tmpD + 59 : tmpD + 69;

            kv["Sumd3"] = "(d3) " + Smart(tmpd3);
            kv["Sumd3Tol"] = "+ 0 [3F]";
            kv["Sumd3TolN"] = (tmpD < 140 ? "- 0.3" : "- 0.5") + " [3F]";

            double tmpd4 = tmpM24 ? tmpD : (tmpD < 26 ? tmpD + 1 : tmpD < 31 ? tmpD + 2 : tmpD < 36 ? tmpD + 3 : tmpD < 41 ? tmpD + 2 : tmpD < 46 ? tmpD + 3 : tmpD < 51 ? tmpD + 2 : tmpD < 56 ? tmpD + 3 : tmpD < 61 ? tmpD + 2 : tmpD < 66 ? tmpD + 3 : tmpD < 76 ? tmpD + 2 : tmpD < 101 ? tmpD + 3 : tmpD + 2);

            kv["Sumd4"] = "(d4) " + Smart(tmpd4);
            kv["Sumd4Tol"] = (tmpd4 < 14 ? "+ 0.2" : tmpd4 < 22 ? "+ 0.3" : tmpd4 < 53 ? "+ 0.4" : "+ 0.5") + " [3F]";
            kv["Sumd4TolN"] = "- 0 [3F]";

            double tmpe1 = tmpD < 11 ? 1.5 : tmpD < 31 ? 2 : tmpD < 46 ? 3 : tmpD < 76 ? 4 : 5;

            kv["Sume1"] = "(e1) " + Smart(tmpe1);
            kv["Sume1Tol"] = "± 0.2";

            double tmpe2 = tmpD < 21 ? 1 : tmpD < 31 ? 2 : tmpD < 46 ? 3 : tmpD < 86 ? 4 : 5;
            double tmpe2S1 = tmpe2 + 1;

            kv["Sume2S1"] = "(e2) " + Smart(tmpe2S1);
            kv["Sume2S1Tol"] = "± 0.2";

            kv["Sume2"] = "(e2) " + Smart(tmpe2);
            kv["Sume2Tol"] = "± 0.2";

            double tmpB = tmpD < 36 ? 20 : tmpD < 46 ? 22 : tmpD < 61 ? 24 : tmpD < 66 ? 25 : tmpD < 76 ? 26 : tmpD < 81 ? 30 : tmpD < 201 ? 32 : tmpD < 211 ? 34 : tmpD < 221 ? 36 : tmpD < 261 ? 38 : tmpD < 281 ? 40 : tmpD < 321 ? 45 : tmpD < 341 ? 48 : tmpD < 361 ? 50 : tmpD < 381 ? 55 : tmpD < 401 ? 60 : 65;

            double tmpBS1 = tmpB + 1;

            kv["SumBS1"] = "(B) " + Smart(tmpBS1);
            kv["SumB"] = "(B) " + Smart(tmpB);

            string sumBTol = tmpD < 21 ? "± 0.15" : tmpD < 76 ? "± 0.20" : "± 0.25";

            kv["SumBTol"] = sumBTol;
            kv["SumBS1Tol"] = sumBTol;

            double tmpD1 = tmpM24 ? tmpD - 2.624 : tmpStmm == 0.75 ? tmpD - 0.812 : tmpStmm == 1 ? tmpD - 1.083 : tmpStmm == 1.5 ? tmpD - 1.624 : tmpStmm == 2 ? tmpD - 2.165 : tmpStmm == 3 ? tmpD - 3.248 : tmpStmm == 4 ? tmpD - 4 : tmpD - 5;

            double tmpD1S1 = Math.Round(maskinVal == "LT300" ? tmpD1 - 1 : (tmpStmm == 1 ? tmpD - 2 : tmpStmm == 1.5 ? tmpD - 2.5 : tmpStmm == 2 ? tmpD - 4 : tmpD1 - 1), 2);

            kv["SumD1S1"] = "(D1) " + Smart(tmpD1S1);
            kv["SumD1S1Tol"] = " ± 0.2";

            kv["SumD1"] = "(D1) " + Smart(tmpD1);

            kv["SumD1Tol"] = (tmpStmm == 0.75 ? "+ 0.150" : tmpStmm == 1 ? "+ 0.190" : tmpStmm == 1.5 ? "+ 0.236" : tmpStmm == 2 ? "+ 0.300" : tmpStmm == 3 ? "+ 0.400" : tmpStmm == 4 ? "+ 0.375" : "+ 0.450") + " [3F]";

            kv["SumD1TolN"] = "- 0 [2F]";

            double tmpd5 = tmpM24 ? tmpD : (tmpD < 131 ? tmpD + 1 : tmpD < 201 ? tmpD + 0.5 : tmpD + 2);

            double tmpd5S1 = tmpD > 131 ? tmpd5 - 0.5 : tmpd5;

            kv["Sumd5S1"] = "(d5) " + Smart(tmpd5S1);
            kv["Sumd5"] = "(d5) " + Smart(tmpd5);

            string sumd5Tol = (tmpD < 76 ? "+ 0.2" : tmpD < 91 ? "+ 0.4" : "+ 0.5") + " [3F]";

            kv["Sumd5Tol"] = sumd5Tol;
            kv["Sumd5TolN"] = "- 0 [3F]";

            kv["Sumd5S1Tol"] = tmpD > 131 ? "+ 0" : sumd5Tol;

            kv["Sumd5S1TolN"] = tmpD > 131 ? (tmpD < 76 ? "- 0.2" : tmpD < 91 ? "- 0.4" : "- 0.5") : "- 0 [3F]";

            kv["Sum30"] = tmpD < 201 ? "30º" : "15º";
            kv["Sum30a"] = "20º";

            kv["Sum60"] = tmpD < 201 ? "60º" : "45º";

            kv["Sum60a"] = tmpD < 46 ? "" : tmpD < 201 ? "60º" : "45º";

            kv["SumGFas"] = tmpD < 131 ? "20º" : tmpD < 201 ? "60º" : "45º";
            kv["SumGFas1"] = kv["SumGFas"];

            double tmpd6 = tmpD < 16 ? tmpD + 11 : tmpD < 21 ? tmpD + 12 : tmpD < 36 ? tmpD + 11 : tmpD < 41 ? tmpD + 14 : tmpD < 46 ? tmpD + 15 : tmpD < 51 ? tmpD + 14 : tmpD < 56 ? tmpD + 19 : tmpD < 81 ? tmpD + 18 : tmpD < 201 ? tmpD + 22 : tmpD < 221 ? tmpD + 36.5 : tmpD < 251 ? tmpD + 41 : tmpD < 261 ? tmpD + 41.5 : tmpD < 281 ? tmpD + 40 : tmpD < 301 ? tmpD + 50 : tmpD < 361 ? tmpD + 59.5 : tmpD + 68;

            kv["Sumd6"] = "(d6) " + Smart(tmpd6);

            kv["Sumd6Tol"] = "+ 0 [3P]";

            kv["Sumd6TolN"] = (tmpd6 < 33 ? "- 0.3" : tmpd6 < 65 ? "- 0.4" : tmpd6 < 143 ? "- 0.5" : "- 0.7") + " [3F]";

            double tmpdm = tmpM24 ? tmpD - 1.974 : tmpStmm == 0.75 ? tmpD - 0.487 : tmpStmm == 1 ? tmpD - 0.650 : tmpStmm == 1.5 ? tmpD - 0.974 : tmpStmm == 2 ? tmpD - 1.299 : tmpStmm == 3 ? tmpD - 1.949 : tmpStmm == 4 ? tmpD - 2.000 : tmpD - 2.500;

            kv["Sumdm"] = "(dm) " + Smart(tmpdm);

            kv["SumdmTol"] = (tmpD < 46 ? "+ 0.140" : tmpD < 76 ? "+ 0.150" : tmpD < 91 ? "+ 0.170" : tmpD < 121 ? "+ 0.180" : tmpD < 131 ? "+ 0.216" : tmpD < 181 ? "+ 0.236" : tmpD < 201 ? "+ 0.265" : "+ 0.475") + " [2F]";

            kv["SumdmTolN"] = (tmpD < 46 ? "+ 0.080" : tmpD < 76 ? "+ 0.090" : tmpD < 91 ? "+ 0.110" : tmpD < 121 ? "+ 0.120" : tmpD < 131 ? "+ 0.156" : "+ 0") + " [2F]";

            kv["Sumt1"] = tmpD < 201 ? "5µ" : "50µ";
            kv["Sumt2"] = kv["Sumt1"];

            kv["SumRa32"] = "Ytjämnhet alla övriga färdig-bearbetade ytor 3.2";
            kv["SumRa08"] = "0.8 [2F]";
            kv["SumRa25"] = "2.5";

            double tmpBOp1 = tmpB + 3;

            kv["SumBOp1"] = tmpD > 201 ? "Bredd (B) efter första operation" + LB + "i högra spindeln = " + Smart(tmpBOp1) + "mm" : "";

            double tmpd3Op1 = tmpd3 + 1;

            string tmpMaskinValS2 = maskinVal == "LVT300" ? "LVT300" : maskinVal == "LT300" ? "LT300" : "";

            kv["SumMaskinValS2"] = "Gängning: " + tmpMaskinValS2;

            kv["SumM2_3"] = tmpStmm < 4 ? "Gänga Funktionskontroll" : "";

            bool machineEnabled = maskinVal == "LVT300" || maskinVal == "LT300";

            string standardFrequency = tmpTyp < 16 ? "1/20" : tmpTyp < 26 ? "1/10" : "1/5";

            kv["SumF2_1"] = machineEnabled ? standardFrequency : "";
            kv["SumF2_2"] = machineEnabled ? standardFrequency : "";
            kv["SumF2_3"] = machineEnabled ? standardFrequency : "";
            kv["SumF2_4"] = machineEnabled ? standardFrequency : "";
            kv["SumF2_5"] = machineEnabled ? standardFrequency : "";
            kv["SumF2_6"] = machineEnabled ? standardFrequency : "";
            kv["SumF2_7"] = machineEnabled ? "1/50" : "";
            kv["SumF2_8"] = machineEnabled ? "-" : "";
            kv["SumF2_9"] = machineEnabled ? "Inst." : "";
            kv["SumF2_0"] = machineEnabled ? standardFrequency : "";

            kv["SumD2_1"] = machineEnabled ? (tmpStmm < 2 ? "INWI " + Smart(tmpStmm) + "mm backar" : "UD-Apparat " + Smart(tmpStmm) + "mm rullar") : "";

            kv["SumD2_2"] = machineEnabled ? "Skjutmått / kärnhålstolk" : "";

            kv["SumD2_3"] = tmpStmm < 4 ? (machineEnabled ? "GängTolk: " + tmpG : "") : "";

            kv["SumD2_4"] = machineEnabled ? "Djupmått" : "";

            kv["SumD2_5"] = machineEnabled ? (tmpD < 22 ? "mall:7422251/1" : tmpD < 31 ? "mall:7422251/2" : tmpD < 46 ? "mall:7422251/3" : tmpD < 76 ? "mall:7422251/4" : "mall:7424073/2") : "";

            kv["SumD2_6"] = machineEnabled ? "Skjutmått" : "";

            kv["SumD2_7"] = machineEnabled ? "Ytjämnhetsmätare" : "";

            kv["SumD2_8"] = machineEnabled ? "Mätrum" : "";

            kv["SumD2_9"] = machineEnabled ? ((tmpTyp == 44 || tmpTyp == 48) ? "Egglinjal" : "Mätmaskin") : "";

            kv["SumD2_0"] = machineEnabled ? "Skjutmått" : "";

            kv["SumAF2_1"] = machineEnabled ? (tmpStmm < 4 ? "Kontrolleras med gängring " + tmpG + " 1/tim" : "Kontrolleras med Klove") : "";

            kv["SumAF2_2"] = machineEnabled ? "Vid inställning kontrollera innan gängning" : "";

            kv["SumAF2_3"] = tmpStmm < 4 ? (machineEnabled ? "" : "") : "";

            kv["SumAF2_4"] = machineEnabled ? "" : "";
            kv["SumAF2_5"] = machineEnabled ? "" : "";
            kv["SumAF2_6"] = machineEnabled ? "" : "";

            kv["SumAF2_7"] = machineEnabled ? "Övrig ytjämnhet 3.2" : "";

            kv["SumAF2_8"] = machineEnabled ? "Vid misstänkt formfel lämnas mutter till mätrum" : "";

            kv["SumAF2_9"] = machineEnabled ? "Kontrollera & justera till godkänd detalj" : "";

            kv["SumAF2_0"] = machineEnabled ? "" : "";

            kv["SumTextS2"] = "100% okulär kontroll av grader, frifläckar, slagmärken, repor, valkar, sprickor och andra ojämnheter." + LB + "Vid upptäckta felaktiga detaljer skall kontroll av föregående detalj göras tills första godkända detalj hittas.";

            string tmpStampling = "Stämplas enl. ritning: 7430920 alt. 7430189";

            kv["SumRitNrS2"] = (tmpTyp == 48 ? "7436445" : tmpTyp < 41 ? "KMTA 5-40:6" : subject) + " " + tmpStampling;

            kv["SumKlEgenskaperS2"] = @"PPA & PPH\ALLMÄN\KLASSADE EGENSKAPER\Klassade egenskaper muttrar KMT KMTA";

            return kv;
        }

        static object ParseLotusValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return "";

            double d;

            return double.TryParse(value.Replace(".", ","), NumberStyles.Any, CultureInfo.InvariantCulture, out d) ? d : (object)value;
        }

        static double ToDouble(object value)
        {
            if (value == null) return 0;

            double.TryParse(value.ToString().Replace(".", ","), NumberStyles.Any, CultureInfo.InvariantCulture, out double result);

            return result;
        }

        static double ParseDouble(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return 0;

            double.TryParse(value.Replace(".", ","), NumberStyles.Any, CultureInfo.InvariantCulture, out double result);

            return result;
        }

        static string Right(string value, int length)
        {
            if (string.IsNullOrEmpty(value)) return "";

            return value.Length <= length ? value : value.Substring(value.Length - length);
        }

        static string Smart(double value)
        {
            return Math.Abs(value % 1) < 0.0001 ? value.ToString("F0") : value.ToString("0.###").Replace(".", ","); ;
        }

        static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published)) return "";

            if (!DateTime.TryParse(published, out DateTime publishDate)) return "";

            DateTime validTo = publishDate.AddDays(14);

            if (DateTime.Today > validTo) return "";

            return "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)" + LB + LB + "Popupruta aktiv till " + validTo.ToString("yyyy-MM-dd");
        }
    }
}
