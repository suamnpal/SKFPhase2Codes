using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Avdragshylsor_64_1060_Borrning : ITemplateCalculations
    {
        private const string LB = "<<LineBreak>>";

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();
            var bm = req?.Bookmarks;

            string subject = req?.ProductDesignation ?? "";
            string maskinVal = req?.MachineNumber ?? "";

            kv["VaLPopUp"] = ComputePopup(req?.Published);

            string tmpFormat = subject.Trim().ToUpperInvariant();
            string tmpBet = tmpFormat.Replace(".", ",");

            int tmpCountB = tmpBet.Length;
            bool tmpSpecial = tmpCountB > 10;
            bool tmpV21 = ContainsI(tmpBet, "V21");
            bool tmpV22 = ContainsI(tmpBet, "V22");
            bool tmpGFlag = ContainsI(tmpBet, "G");
            bool tmpSlash = tmpBet.Contains("/");

            string[] parts = Split(tmpBet, " /.-");

            string tmpBet1a = Item(parts, 1);
            string tmpBet2a = Item(parts, 2);
            string tmpBet3a = Item(parts, 3);
            string tmpBet4a = Item(parts, 4);
            string tmpBet5a = Item(parts, 5);
            string tmpBet6a = Item(parts, 6);

            string tmpBet1b = string.IsNullOrEmpty(tmpBet1a) ? "0" : tmpBet1a;
            string tmpBet2b = string.IsNullOrEmpty(tmpBet2a) ? "0" : tmpBet2a;
            string tmpBet3b = string.IsNullOrEmpty(tmpBet3a) ? "0" : tmpBet3a;
            string tmpBet4b = string.IsNullOrEmpty(tmpBet4a) ? "0" : tmpBet4a;
            string tmpBet5b = string.IsNullOrEmpty(tmpBet5a) ? "0" : tmpBet5a;
            string tmpBet6b = string.IsNullOrEmpty(tmpBet6a) ? "0" : tmpBet6a;

            object tmpBet1 = IsNumeric(tmpBet1b) && tmpBet1b != "0" ? ParseDouble(tmpBet1a) : (object)tmpBet1a;
            object tmpBet2 = IsNumeric(tmpBet2b) && tmpBet2b != "0" ? ParseDouble(tmpBet2a) : (object)tmpBet2a;
            object tmpBet3 = IsNumeric(tmpBet3b) && tmpBet3b != "0" ? ParseDouble(tmpBet3a) : (object)tmpBet3a;
            object tmpBet4 = IsNumeric(tmpBet4b) && tmpBet4b != "0" ? ParseDouble(tmpBet4a) : (object)tmpBet4a;
            object tmpBet5 = IsNumeric(tmpBet5b) && tmpBet5b != "0" ? ParseDouble(tmpBet5a) : (object)tmpBet5a;
            object tmpBet6 = IsNumeric(tmpBet6b) && tmpBet6b != "0" ? ParseDouble(tmpBet6a) : (object)tmpBet6a;

            string tmpBet2Text = LotusText(tmpBet2);
            string tmpBet3Text = LotusText(tmpBet3);

            int tmpCountB2 = tmpBet2Text.Length;
            int tmpCountB3 = tmpBet3Text.Length;

            string[] artValues = { "AOH", "AOHX", "LW", "MS" };
            int tmpArtLista = MemberText(LotusText(tmpBet1), artValues);
            string tmpSerie = tmpSlash && (tmpCountB2 == 3 || tmpCountB2 == 2) ? tmpBet2Text : tmpCountB2 > 4 ? Left(tmpBet2Text, 3) : tmpCountB2 == 3 ? Left(tmpBet2Text, 1) : Left(tmpBet2Text, 2);
            string tmpTyp = !tmpSlash ? Right(tmpBet2Text, 2) : tmpBet3Text;

            double tmpD1 = GetDouble(bm, "Innerdiameter (d1)");
            double tmpC = GetDouble(bm, "Längd Oljeborrhål (C)");
            string gangaOlje = GetString(bm, "Gänga Oljeborrhål");
            string gangfasOlje = GetString(bm, "Gängfas Oljeborrhål");
            string ganglangdOljeRaw = GetString(bm, "Gänglängd Oljeborrhål (S1)");
            double diameterG = GetDouble(bm, "Diameter Borrhål (G)");
            double lutningBeta = GetDouble(bm, "Lutning BorrOljehål (ß)");
            double borrhalH = GetDouble(bm, "Borrhål till Oljespår (H)");
            double tmpBHC = GetDouble(bm, "Borrhålscentrum (A)");
            double vinkelBorrOljehal = GetDouble(bm, "Vinkel BorrOljehål");
            string ritningsnummer = GetString(bm, "Ritningsnummer");

            kv["SumC"] = "(c) " + Smart(tmpC);

            string tmpR = gangaOlje.Replace(" ", "").Trim().ToUpperInvariant();

            kv["SumR"] = "(R) " + tmpR;

            bool shortThreadType = tmpR == "R1/4" || tmpR == "G1/4" || tmpR == "M10";

            kv["SumS"] = shortThreadType ? tmpBet2Text == "3088" && tmpV21 ? "(S) 18" : "(S) 15" : "(S) 13";

            string tmpT = gangfasOlje.Replace(" ", "").Trim().ToUpperInvariant();

            kv["SumT"] = tmpT == "0" ? "" : "(T) " + tmpT;

            string tmpS1;

            if (ganglangdOljeRaw == "0")
            {
                if (tmpSerie == "22" || tmpSerie == "23" || tmpSerie == "31" || tmpSerie == "32" || tmpSerie == "39")
                {
                    tmpS1 = "12";
                }
                else if (tmpSerie == "240")
                {
                    tmpS1 = LotusTextLessThan(tmpTyp, "68") ? "11" : "12";
                }
                else if (tmpSerie == "241" || tmpSerie == "30")
                {
                    tmpS1 = LotusTextLessThan(tmpTyp, "48") ? "11" : "12";
                }
                else
                {
                    tmpS1 = "none";
                }
            }
            else
            {
                tmpS1 = ganglangdOljeRaw;
            }

            kv["SumS1"] = ganglangdOljeRaw == "none" ? "-" : "(S1) " + tmpS1;
            kv["SumG"] = "(G) " + Smart(diameterG);
            kv["Sumß"] = Math.Abs(lutningBeta) < 0.0000001 ? "0°" : "(ß) " + Smart(lutningBeta) + "º";
            kv["SumH"] = "(H) " + Smart(borrhalH);
            kv["SumH1"] = "(H) " + Smart(borrhalH);

            double tmpA = tmpBHC < 50 ? tmpBHC : (tmpBHC - tmpD1) / 2;
            double tmpAD = tmpBHC < 50 ? (tmpA * 2) + tmpD1 : tmpBHC;
            double tmpAADTol = tmpD1 < 420 ? 0.15 : tmpD1 < 630 ? 0.20 : 0.30;
            double tmpADTol = 2 * tmpAADTol;

            kv["SumA"] = "(A) " + Smart(tmpA);
            kv["SumAD"] = "(AD) " + Smart(tmpAD);
            kv["SumATol"] = "± " + F2(tmpAADTol);
            kv["SumADTol"] = "± " + F2(tmpADTol);

            double tmpKonstant = LotusRound(Math.Sin((vinkelBorrOljehal / 2) * Math.PI / 180) * 2, 0.0001);
            double tmpKordaOs = tmpBHC < 50 ? LotusRound(((tmpD1 + (tmpBHC * 2)) / 2) * tmpKonstant, 0.1) : LotusRound((tmpBHC / 2) * tmpKonstant, 0.1);

            kv["SumVBOH"] = Smart(vinkelBorrOljehal) + "º";
            kv["SumKordaOs"] = Smart(tmpKordaOs);
            kv["SumRitNr"] = ritningsnummer == "0" ? tmpBet : ritningsnummer;

            bool isKT = maskinVal == "K&T";
            bool isSkepp6 = maskinVal == "Skepp 6";
            bool machineEnabled = isKT || isSkepp6;
            string tmpMaskinVal = machineEnabled ? "" : "INGEN MASKINVAL GJORD";

            kv["SumMaskinVal"] = "Maskin: " + maskinVal + tmpMaskinVal;
            kv["SumF_C"] = isKT ? "1/10" : isSkepp6 ? "1/1" : "";
            kv["SumF_A"] = isKT ? "1/10" : isSkepp6 ? "1/1" : "";
            kv["SumF_G"] = isKT ? "1/tim" : isSkepp6 ? "1/1" : "";
            kv["SumF_H"] = isKT ? "1/10" : isSkepp6 ? "1/1" : "";
            kv["SumF_R"] = isKT ? "1/tim" : isSkepp6 ? "1/1" : "";
            kv["SumD_C"] = machineEnabled ? "Skjutmått" : "";
            kv["SumD_A"] = machineEnabled ? "Skjutmått" : "";
            kv["SumD_G"] = machineEnabled ? "Skjutmått" : "";
            kv["SumD_H"] = machineEnabled ? "Skjutmått" : "";
            kv["SumD_R"] = machineEnabled ? "Gängtolk" : "";
            kv["SumAF_C"] = "";
            kv["SumAF_A"] = machineEnabled ? tmpBHC < 50? "Hjälpmått (AD)" : "Hjälpmått (A)" : "";
            kv["SumAF_G"] = machineEnabled ? "Kontroll av genomströmmning med tryckluft" : "";
            kv["SumAF_H"] = "";
            kv["SumAF_R"] = "";

            return kv;
        }
        private static string GetString(IEnumerable<Bookmark> bookmarks, string name)
        {
            if (bookmarks == null || string.IsNullOrEmpty(name))
                return "";

            foreach (Bookmark bookmark in bookmarks)
            {
                if (bookmark != null &&
                    string.Equals(bookmark.BookmarkName,name, StringComparison.OrdinalIgnoreCase))
                {
                    return bookmark.BookmarkValue?.Trim() ?? "";
                }
            }

            return "";
        }

        private static double GetDouble(IEnumerable<Bookmark> bookmarks, string name) => ParseDouble(GetString(bookmarks, name));

        private static string[] Split(string value, string separators)
        {
            if (string.IsNullOrEmpty(value))
                return Array.Empty<string>();

            return value.Split(separators.ToCharArray(),StringSplitOptions.RemoveEmptyEntries);
        }

        private static string Item(string[] values,int lotusPosition) => values == null || lotusPosition < 1 ||
            lotusPosition > values.Length ? "" : values[lotusPosition - 1];

        private static int MemberText(string value, string[] values)
        {
            if (values == null)
                return 0;

            for (int i = 0; i < values.Length; i++)
            {
                if (string.Equals(value ?? "",values[i],StringComparison.Ordinal))
                {
                    return i + 1;
                }
            }

            return 0;
        }

        private static bool IsNumeric(string value) => !string.IsNullOrWhiteSpace(value) &&
            double.TryParse(value.Trim().Replace(",", "."),NumberStyles.Any, CultureInfo.InvariantCulture, out _);

        private static double ParseDouble(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0;

            return double.TryParse(value.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double result)? result : 0;
        }

        private static string LotusText(object value)
        {
            if (value == null)
                return "";

            if (value is double d)
                return Smart(d);

            if (value is float f)
                return Smart(f);

            if (value is decimal m)
                return Smart((double)m);

            return value.ToString() ?? "";
        }

        private static bool LotusTextLessThan(string left,string right) => string.Compare(left ?? "", right ?? "", StringComparison.Ordinal) < 0;

        private static string Smart(double value) => Math.Abs(value % 1) < 0.0000001 ? value.ToString("F0",CultureInfo.InvariantCulture)
                : value.ToString("0.######", CultureInfo.InvariantCulture).Replace(".", ",");

        private static string F2(double value) => value.ToString("F2", CultureInfo.InvariantCulture);

        private static string Left(string value,int length) => string.IsNullOrEmpty(value) || length <= 0 ? ""
                : value.Substring(0, Math.Min(length, value.Length));

        private static string Right(string value, int length) => string.IsNullOrEmpty(value) || length <= 0 ? ""
                : value.Length <= length? value : value.Substring(value.Length - length);

        private static bool ContainsI(string source, string value) => !string.IsNullOrEmpty(source) && !string.IsNullOrEmpty(value) &&
            source.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
        private static double LotusRound(double value, double factor)
        {
            if (factor == 0)
                return value;

            return Math.Floor((value / factor) + 0.5) * factor;
        }

        private static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published))
                return "";

            if (!DateTime.TryParseExact(published,"yyyy-MM-dd", CultureInfo.InvariantCulture,DateTimeStyles.None, out DateTime publishDate) &&
                !DateTime.TryParse(published, CultureInfo.InvariantCulture, DateTimeStyles.None, out publishDate) &&
                !DateTime.TryParse(published, out publishDate))
            {
                return "";
            }

            DateTime validTo = publishDate.AddDays(14);

            if (DateTime.Today > validTo.Date)
                return "";

            return
                "Denna kontrollinstruktion har nyligen blivit uppdaterad " +"(inom 14 dagar)" + LB + LB +
                LB + LB + "Popupruta aktiv till " + validTo.ToString("yyyy-MM-dd",CultureInfo.InvariantCulture);
        }
    }
}