using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Klamhylsor_23_30_31_32_39_OH_H_Oljeanslutningar_Oljespar_Repning_32_1000 : ITemplateCalculations
    {
        private const string LB = "<<LineBreak>>";

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();
            string subject = req?.ProductDesignation ?? "";
            string maskinVal = req?.MachineNumber ?? "";

            kv["VaLPopUp"] = ComputePopup(req?.Published);

            string tmpFormat = subject.Trim().ToUpperInvariant();
            string tmpBet = tmpFormat.Replace(".", ",");
            string[] tmpBetLista = Split(tmpBet, " /.-");

            string tmpBet1a = Item(tmpBetLista, 1);
            string tmpBet2a = Item(tmpBetLista, 2);
            string tmpBet3a = Item(tmpBetLista, 3);
            string tmpBet4a = Item(tmpBetLista, 4);
            string tmpBet5a = Item(tmpBetLista, 5);

            int tmpCountT1 = MemberText("HB", new[] { tmpBet1a, tmpBet2a, tmpBet3a, tmpBet4a, tmpBet5a });
            int tmpCountT2 = MemberText("V29", new[] { tmpBet1a, tmpBet2a, tmpBet3a, tmpBet4a, tmpBet5a });
            int tmpCountT3 = MemberText("H", new[] { tmpBet1a, tmpBet2a, tmpBet3a, tmpBet4a, tmpBet5a });

            string tmpBet1 = tmpBet1a;
            string tmpBet2 = tmpBet2a;
            object tmpBet3 = tmpCountT1 == 3 || tmpCountT2 == 3 || tmpCountT3 == 3 ? (object)tmpBet3a : ParseDouble(tmpBet3a);
            object tmpBet4 = tmpCountT1 == 4 || tmpCountT2 == 4 || tmpCountT3 == 4 ? (object)tmpBet4a : ParseDouble(tmpBet4a);
            object tmpBet5 = tmpCountT1 == 5 || tmpCountT2 == 5 || tmpCountT3 == 5 ? (object)tmpBet5a : ParseDouble(tmpBet5a);

            string tmpBet3Text = LotusText(tmpBet3);
            string tmpBet4Text = LotusText(tmpBet4);
            string tmpBet5Text = LotusText(tmpBet5);

            int tmpCount = tmpBet.Length;
            int tmpCountB2 = tmpBet2.Length;
            int tmpCountB3 = tmpBet3Text.Length;

            bool tmpSlash = tmpBet.Contains("/");
            bool tmpOH = tmpBet.Contains("OH");
            bool tmpSpecDia = string.Equals(tmpBet5Text, "H", StringComparison.Ordinal);

            string tmpSerie = tmpSlash && (tmpCountB2 == 3 || tmpCountB2 == 2) ? tmpBet2 : tmpCountB2 > 4 ? Left(tmpBet2, 3) : tmpCountB2 == 3 ? Left(tmpBet2, 1) : Left(tmpBet2, 2);
            string tmpTypText = !tmpSlash ? Right(tmpBet2, 2) : tmpCountB3 > 5 ? Right(tmpBet2, 2) : tmpBet3Text;
            double tmpTyp = ParseDouble(tmpTypText);

            string tmpRit = tmpSerie == "23" || tmpSerie == "30" ? "7434151" : tmpSerie == "31" ? "7434152" : tmpSerie == "32" ? "7434153" : tmpSerie == "39" ? "7434170, 7434171" : "Fel Mall";

            kv["SumRitNr"] = tmpRit;

            string[] tmpTyp30Values = { "32", "34", "36", "38", "40", "44", "48", "52", "56", "60", "64", "68", "72", "76", "80", "84", "88", "92", "96", "500", "530", "560", "600", "630", "670", "710", "750", "800", "850", "900", "950", "378", "420", "460" };
            string[] tmpTyp31Values = { "32", "34", "36", "38", "40", "44", "48", "52", "56", "60", "64", "68", "72", "76", "80", "84", "88", "92", "96", "500", "530", "560", "600", "630", "670", "710", "750", "800", "850", "900", "1000", "420", "355,6" };
            string[] tmpTyp23Values = { "32", "34", "36", "38", "40", "44", "48", "52", "56" };
            string[] tmpTyp32Values = { "60", "64", "68", "72", "76", "80", "84", "88", "92", "96", "500", "530", "560", "600", "630", "670", "710", "750", "800", "850" };
            string[] tmpTyp39Values = { "32", "34", "36", "38", "40", "44", "48", "52", "56", "60", "64", "68", "72", "76", "80", "84", "88", "92", "96", "500", "530", "560", "600", "630", "670", "710", "750", "800", "850", "900", "457,2" };

            int tmpTyp30Lista = MemberText(tmpTypText, tmpTyp30Values);
            int tmpTyp31Lista = MemberText(tmpTypText, tmpTyp31Values);
            int tmpTyp23Lista = MemberText(tmpTypText, tmpTyp23Values);
            int tmpTyp32Lista = MemberText(tmpTypText, tmpTyp32Values);
            int tmpTyp39Lista = MemberText(tmpTypText, tmpTyp39Values);
            int tmpTypLista = tmpSerie == "30" ? tmpTyp30Lista : tmpSerie == "31" ? tmpTyp31Lista : tmpSerie == "23" ? tmpTyp23Lista : tmpSerie == "32" ? tmpTyp32Lista : tmpSerie == "39" ? tmpTyp39Lista : 0;

            bool tmpSpecialSmallType = tmpTyp < 85 || IsAny(tmpTyp, 420, 355.6, 378);
            bool tmpSpecialMediumType = tmpTyp < 561 || tmpTyp == 630;

            string tmpG = tmpSpecialSmallType ? "M6" : tmpSpecialMediumType ? "M8" : "G1/8";

            kv["SumG"] = tmpG;

            string tmpBTab30 = "4,2:4,2:4,2:4,2:4,2:3,9:3,9:3,9:3,9:3,9:3,5:3,5:3,5:3,5:3,5:3,5:6,5:6,5:6,5:6,5:6:6:8:6:8:8:8:10:10:10:10:4:3,5:3,5";
            string tmpBTab31 = "4,2:4,2:4,2:4,2:4,2:3,9:3,9:3,9:3,9:3,9:3,5:3,5:3,5:3,5:3,5:3,5:6,5:6,5:6,5:6,5:6:6:8:6:8:8:8:10:10:10:10:3,5:4,5";
            string tmpBTab23 = "4,2:4,2:4,2:4,2:4,2:3,9:3,9:3,9:3,9";
            string tmpBTab32 = "3,9:3,5:3,5:3,5:3,5:3,5:3,5:6,5:6,5:6,5:6:6:6:8:6:8:8:8:10:10";
            string tmpBTab39 = "4,2:4,2:4,2:4,2:4,2:3,9:3,9:3,9:3,9:3,9:3,5:3,5:3,5:3,5:3,5:3,5:6,5:6,5:6,5:6,5:6:6:8:6:8:8:8:10:10:10:4";

            string tmpBLista = tmpSerie == "30" ? tmpBTab30 : tmpSerie == "31" ? tmpBTab31 : tmpSerie == "23" ? tmpBTab23 : tmpSerie == "32" ? tmpBTab32 : tmpSerie == "39" ? tmpBTab39 : "";
            double tmpB = ParseDouble(ListItem(tmpBLista, tmpTypLista));

            kv["SumB"] = "(B) " + SmartComma(tmpB);
            kv["SumBTol"] = tmpB == 4 ? "- 0.1" : "   0";
            kv["SumBTolN"] = tmpB == 4 ? "- 0.3" : "- 0.1";

            double tmpC = tmpSpecialSmallType ? 10 : tmpSpecialMediumType ? 12 : 13;

            kv["SumC"] = "(C) " + SmartComma(tmpC);
            kv["SumCTol"] = GeneralTolerance(tmpC);

            double tmpT = tmpSpecialSmallType ? 6.3 : tmpSpecialMediumType ? 8.3 : 10;

            kv["SumT"] = "(T) " + SmartComma(tmpT);
            kv["SumTTol"] = GeneralTolerance(tmpT);

            double tmpD = tmpSpecialSmallType ? 3 : tmpSpecialMediumType ? 4 : 5;

            kv["SumD"] = "(D) " + SmartComma(tmpD);
            kv["SumDTol"] = GeneralTolerance(tmpD);

            double tmpH30 = tmpTyp < 45 ? 0.8 : tmpTyp < 65 ? 1 : tmpTyp < 85 || tmpTyp == 378 ? 1.2 : tmpTyp < 751 ? 1.5 : 2;
            double tmpH31 = tmpTyp < 45 ? 0.8 : tmpTyp < 65 ? 1 : tmpTyp < 85 || tmpTyp == 355.6 ? 1.2 : tmpTyp < 601 ? 1.5 : tmpTyp < 751 ? 2 : 2.8;
            double tmpH23 = tmpTyp < 45 ? 0.8 : tmpTyp < 65 ? 1 : 1.2;
            double tmpH32 = tmpH30;
            double tmpH39 = tmpTyp < 45 ? 0.8 : tmpTyp < 65 ? 1 : tmpTyp < 85 ? 1.2 : tmpTyp < 631 ? 1.5 : tmpTyp < 751 ? 2 : 2.8;
            double tmpH = tmpSerie == "30" ? tmpH30 : tmpSerie == "31" ? tmpH31 : tmpSerie == "23" ? tmpH23 : tmpSerie == "32" ? tmpH32 : tmpH39;

            kv["SumH"] = "(H) " + SmartComma(tmpH);
            kv["SumHTol"] = tmpH < 6.1 ? "± 0.1" : "± 0.2";
            string tmpETab30 = "57:61:66:68:72:74:79:84:89:98:100:109:109:113:122:123:135:138:139:147:155,5:169:171:176,5:189,5:202:208,5:212:218,5:232:240:122:135:139";
            string tmpETab31 = "69:71:75:81:85:92:98:107:110:115:124:144:148:151:156:175:176:187:191:203:206,5:217:226:243:263:267,5:281,5:287:304:316,5:339:176:151";
            string tmpETab23 = "82:85:89:93:97:104:110:117:123";
            string tmpETab32 = "130:140,5:160:166:172:181:196:200:212:219:235:244:255,5:265,5:286,5:309:314,5:332:337,5:356";
            string tmpETab39 = "51:52:56:57:62:60:64:71:75:86:86:89:89:99:103:103:117:117:122:130:134:143:147,5:154,5:161,5:176:178,5:183:185:197,5:122";

            string tmpELista = tmpSerie == "30" ? tmpETab30 : tmpSerie == "31" ? tmpETab31 : tmpSerie == "23" ? tmpETab23 : tmpSerie == "32" ? tmpETab32 : tmpSerie == "39" ? tmpETab39 : "";
            double tmpE = ParseDouble(ListItem(tmpELista, tmpTypLista));

            kv["SumE"] = "(E) " + SmartComma(tmpE);
            kv["SumETol"] = ExtendedGeneralTolerance(tmpE);

            string tmpJTab30 = "54,5:58,5:63:64,5:68,5:70,5:75,5:81:85,5:95:96,5:105:105,5:109:118,5:119,5:130,5:133,5:134,5:143:151,5:163:165:170,5:183,5:196:202,5:206:212,5:226:235:118,5:130,5:134,5";
            string tmpJTab31 = "66:68:72,5:77,5:82:89:94,5:104:106,5:112:121:140,5:144,5:147,5:152:171:171,5:183:186,5:199:202,5:211:220:237:257:261,5:276,5:281:298:310,5:333:171,5:147,5";
            string tmpJTab23 = "79:82,5:86:90:93,5:100,5:107:113,5:120";
            string tmpJTab32 = "126,5:135,5:156:162,5:168:177:192,5:196:208:214,5:231:240:249,5:259,5:280,5:303:308,5:326:331,5:350";
            string tmpJTab39 = "48:49:53:54:58,5:57:61:68:72:82,5:82,5:85,5:85,5:95,5:99,5:99,5:113:113:117,5:125,5:129:138:142,5:149,5:156,5:171:173,5:178:180:192,5:117,5";

            string tmpJLista = tmpSerie == "30" ? tmpJTab30 : tmpSerie == "31" ? tmpJTab31 : tmpSerie == "23" ? tmpJTab23 : tmpSerie == "32" ? tmpJTab32 : tmpSerie == "39" ? tmpJTab39 : "";
            double tmpJ = ParseDouble(ListItem(tmpJLista, tmpTypLista));

            kv["SumJ"] = "(J) " + SmartComma(tmpJ);
            kv["SumJTol"] = ExtendedGeneralTolerance(tmpJ);

            string tmpFTab30 = "2,5:2,5:2,5:2,5:2,5:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3";
            string tmpFTab31 = "2,5:2,5:2,5:2,5:2,5:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3";
            string tmpFTab23 = "2,5:2,5:2,5:2,5:3:3:3:3:3";
            string tmpFTab32 = "3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3";
            string tmpFTab39 = "2,5:2,5:2,5:2,5:2,5:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3:3";

            string tmpFLista = tmpSerie == "30" ? tmpFTab30 : tmpSerie == "31" ? tmpFTab31 : tmpSerie == "23" ? tmpFTab23 : tmpSerie == "32" ? tmpFTab32 : tmpSerie == "39" ? tmpFTab39 : "";
            double tmpF = ParseDouble(ListItem(tmpFLista, tmpTypLista));

            kv["SumF"] = "(F) " + SmartComma(tmpF);
            kv["SumFTol"] = tmpH < 6.1 ? "± 0.1" : "± 0.2";

            double tmpN = tmpTyp < 45 ? 4 : tmpTyp < 65 ? 5 : tmpTyp < 85 || IsAny(tmpTyp, 378, 355.6) ? 6 : tmpTyp < 601 || IsAny(tmpTyp, 420, 460, 457.2) ? 7 : tmpTyp < 751 ? 8 : 9;

            kv["SumN"] = "(N) " + SmartComma(tmpN);
            kv["SumNTol"] = tmpH < 6.1 ? "± 0.1" : "± 0.2";

            double tmpR1 = tmpTyp < 37 ? 3 : tmpTyp < 65 ? 4 : tmpTyp < 85 || IsAny(tmpTyp, 378, 355.6) ? 4.5 : 5;
            string tmpR = tmpTyp < 85 || IsAny(tmpTyp, 378, 420, 460, 355.6) ? "1" : "2.5";

            kv["SumR1"] = "R" + SmartComma(tmpR1);
            kv["SumR"] = "R" + tmpR;
            kv["SumV120"] = "120º";
            kv["SumV45"] = "45º";

            bool isSkepp6 = maskinVal == "Skepp6";
            bool isKT = maskinVal == "K&T";
            bool isVTR160 = maskinVal == "VTR-160";
            bool isMacTurn550 = maskinVal == "MacTurn 550";
            bool isDubbelparet = maskinVal == "Dubbelparet";
            bool machineEnabled = isSkepp6 || isKT || isVTR160 || isMacTurn550 || isDubbelparet;

            string tmpMaskinVal = isSkepp6 ? "Skepp6" : isKT ? "K&T" : isVTR160 ? "VTR-160" : isMacTurn550 ? "MacTurn 550" : isDubbelparet ? "Dubbelparet" : "";

            kv["SumMaskinValS1"] = "Maskin: " + tmpMaskinVal + " - BorrOljehål & Oljespår";
            kv["SumMaskinValS2"] = "Maskin: " + tmpMaskinVal + " - Oljespår & Repor";

            string tmpFrequency = isSkepp6 ? "1/1" : isDubbelparet ? "1/10" : isKT || isVTR160 || isMacTurn550 ? "1/2" : "";

            kv["SumF1_1"] = tmpFrequency;
            kv["SumF1_2"] = tmpFrequency;
            kv["SumF1_3"] = tmpFrequency;
            kv["SumF1_4"] = tmpFrequency;
            kv["SumF1_5"] = tmpFrequency;
            kv["SumF1_6"] = tmpFrequency;
            kv["SumF1_7"] = tmpFrequency;
            kv["SumF1_8"] = tmpFrequency;
            kv["SumF1_9"] = tmpFrequency;
            kv["SumF1_0"] = tmpFrequency;
            kv["SumF1_11"] = tmpFrequency;
            kv["SumD1_1"] = isSkepp6 ? "Skala på borrmaskin" : isKT || isVTR160 || isMacTurn550 || isDubbelparet ? "pipborr/djupmått" : "";
            kv["SumD1_2"] = machineEnabled ? "Skjutmått" : "";
            kv["SumD1_3"] = machineEnabled ? "Skjutmått" : "";
            kv["SumD1_4"] = machineEnabled ? "Gängtolk" : "";
            kv["SumD1_5"] = machineEnabled ? "Skjutmått" : "";
            kv["SumD1_6"] = machineEnabled ? "Skjutmått/fasmall" : "";
            kv["SumD1_7"] = machineEnabled ? "Skjutmått" : "";
            kv["SumD1_8"] = machineEnabled ? "Skjutmått" : "";
            kv["SumD1_9"] = isSkepp6 ? "Höjdrits" : isKT || isVTR160 || isMacTurn550 || isDubbelparet ? "pipborr/djupmått" : "";
            kv["SumD1_0"] = machineEnabled ? "Skjutmått" : "";
            kv["SumD1_11"] = machineEnabled ? "Radieyra" : "";

            kv["SumAF1_1"] = "";
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = "";
            kv["SumAF1_6"] = "";
            kv["SumAF1_7"] = "";
            kv["SumAF1_8"] = "";
            kv["SumAF1_9"] = "";
            kv["SumAF1_0"] = "";
            kv["SumAF1_11"] = "";

            kv["SumTextS1"] = "Grader, frifläckar, slagmärken, repor, valkar och andra defekter samt ojämnheter kontrolleras med ögonmått.";
            kv["SumTextGängstigning"] = "Max 2x gängstigning";
            kv["SumÖvrigt"] = "1 st. oljeborrhål 180º från slits" + LB + "1 st. utv. oljespår med början 10º från slitscentrum";

            bool hasHVariant = tmpBet3Text == "H" || tmpBet4Text == "H";
            kv["VaLH"] = hasHVariant ? "" : "Denna Mall är endast för typ OH-H" + LB + (tmpBet4Text == "HB" ? "Använd mall för OH-HB" : "Använd mall för OH");

            return kv;
        }
        private static string[] Split(string value, string separators) => string.IsNullOrEmpty(value) ? Array.Empty<string>() : value.Split(separators.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

        private static string Item(string[] values, int lotusPosition) => values == null || lotusPosition < 1 || lotusPosition > values.Length ? "" : values[lotusPosition - 1];

        private static int MemberText(string value, string[] values)
        {
            if (values == null) return 0;
            for (int i = 0; i < values.Length; i++) if (string.Equals(value ?? "", values[i] ?? "", StringComparison.Ordinal)) return i + 1;
            return 0;
        }

        private static string ListItem(string list, int lotusPosition)
        {
            if (string.IsNullOrWhiteSpace(list)) return "";
            string[] values = list.Trim().TrimStart('{').TrimEnd('}').Split(':');
            if (lotusPosition == 0) lotusPosition = 1;
            int index = lotusPosition > 0 ? lotusPosition - 1 : values.Length + lotusPosition;
            return index >= 0 && index < values.Length ? values[index] : "";
        }

        private static double ParseDouble(string value) => string.IsNullOrWhiteSpace(value) ? 0 : double.TryParse(value.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double result) ? result : 0;

        private static string LotusText(object value)
        {
            if (value == null) return "";
            if (value is double doubleValue) return SmartComma(doubleValue);
            if (value is float floatValue) return SmartComma(floatValue);
            if (value is decimal decimalValue) return SmartComma((double)decimalValue);
            return value.ToString() ?? "";
        }

        private static string SmartComma(double value) => Math.Abs(value % 1) < 0.0000001 ? value.ToString("F0", CultureInfo.InvariantCulture) : value.ToString("0.######", CultureInfo.InvariantCulture).Replace(".", ",");

        private static string Left(string value, int length) => string.IsNullOrEmpty(value) || length <= 0 ? "" : value.Substring(0, Math.Min(length, value.Length));

        private static string Right(string value, int length) => string.IsNullOrEmpty(value) || length <= 0 ? "" : value.Length <= length ? value : value.Substring(value.Length - length);

        private static bool IsAny(double value, params double[] values)
        {
            if (values == null) return false;
            for (int i = 0; i < values.Length; i++) if (Math.Abs(value - values[i]) < 0.000001) return true;
            return false;
        }

        private static string GeneralTolerance(double value) => value < 6.1 ? "± 0.1" : value < 30.1 ? "± 0.2" : "± 0.3";

        private static string ExtendedGeneralTolerance(double value) => value < 6.1 ? "± 0.1" : value < 30.1 ? "± 0.2" : value < 120.1 ? "± 0.3" : value < 315.1 ? "± 0.5" : value < 1000.1 ? "± 0.8" : value < 2000.1 ? "± 1.2" : "± 2.0";

        private static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published)) return "";
            if (!DateTime.TryParseExact(published, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime publishDate) && !DateTime.TryParse(published, CultureInfo.InvariantCulture, DateTimeStyles.None, out publishDate) && !DateTime.TryParse(published, out publishDate)) return "";
            DateTime validTo = publishDate.AddDays(14);
            if (DateTime.Today > validTo.Date) return "";
            return "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)" + LB + LB + "Information om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument" + LB + LB + "Popupruta aktiv till " + validTo.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
    }
}