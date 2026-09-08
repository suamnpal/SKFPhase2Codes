using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class KRAGAR_TS_34_96 : ITemplateCalculations
    {
        private const int TmpDagar = 14;

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();
            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = subject.Trim().ToUpperInvariant();
            string tmpBet = tmpFormat.Replace(".", ",");
            bool tmpSlash = tmpBet.Contains("/");
            bool tmpV = tmpBet.Contains("V");
            bool tmpVZ863 = tmpBet.Contains("VZ863");
            bool tmpV21 = tmpBet.Contains("V21");

            string[] tokens = tmpBet.Split(new[] { ' ', '/', '.' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = Token(tokens, 0);
            string tmpBet2Text = Token(tokens, 1);
            string tmpBet3Text = Token(tokens, 2);
            string tmpBet4Text = Token(tokens, 3);
            string tmpBet5Text = Token(tokens, 4);

            double tmpBet2 = ParseNum(tmpBet2Text);
            double tmpBet3 = ParseNum(tmpBet3Text);
            double tmpBet4 = ParseNum(tmpBet4Text);
            double tmpBet5 = ParseNum(tmpBet5Text);
            bool tmpV21Pos4 = tmpBet4Text.Contains("V21");
            bool tmpTum = tmpSlash && !tmpVZ863 && !tmpV21 && tmpBet3 < 20;

            double tmpB1 = tmpVZ863 ? 50 : 42;
            kv["SumB1"] = "(B1) " + Fmt(tmpB1);
            kv["SumB1Tol"] = " 0";
            kv["SumB1TolN"] = H13Negative(tmpB1);

            double tmpB2 = tmpVZ863 ? 18 : 12;
            kv["SumB2"] = "(B2) " + Fmt(tmpB2);
            kv["SumB2Tol"] = Js15(tmpB2);

            double tmpB3 = 24;
            kv["SumB3"] = "(B3) " + Fmt(tmpB3);
            kv["SumB3Tol"] = H13Positive(tmpB3);
            kv["SumB3TolN"] = " 0 [3]";

            double tmpB4 = 9;
            kv["SumB4"] = "(B4) " + Fmt(tmpB4);
            kv["SumB44"] = kv["SumB4"];
            kv["SumB4Tol"] = Js15(tmpB4) + " [3]";
            kv["SumB44Tol"] = kv["SumB4Tol"];

            double tmpB5 = 3.3;
            kv["SumB5"] = "(B5) " + FmtComma(tmpB5);
            kv["SumB5Tol"] = Js15(tmpB5) + " [3]";

            double tmpB6 = 2;
            kv["SumB6"] = "(B6) " + Fmt(tmpB6);
            kv["SumB6Tol"] = Js16(tmpB6) + " [3]";

            double tmpB7 = 2;
            kv["SumB7"] = "(B7) " + Fmt(tmpB7);
            kv["SumB7Tol"] = Js16(tmpB7) + " [3]";

            double tmpB8 = 6;
            kv["SumB8"] = "(B8) " + Fmt(tmpB8);
            kv["SumB8Tol"] = H15Positive(tmpB8);
            kv["SumB8TolN"] = " 0";

            string tmpAError = null;
            double tmpA = 0;
            if (!tmpV21 || (tmpBet2 == 68 && tmpBet3 == 291))
                tmpA = tmpBet2 > 100 ? tmpBet2 : (tmpBet2 / 2) * 10;
            else if (tmpBet2 == 52)
                tmpA = 259;
            else if (tmpBet2 == 56)
                tmpA = 279;
            else
                tmpAError = "Fel inmatning";

            kv["SumA"] = "(A) " + (tmpAError ?? Fmt(tmpA));
            kv["SumATol"] = " 0";
            kv["SumATolN"] = tmpAError == null ? H13Negative(tmpA) : H13Negative(0);

            string tmpD1Error = null;
            double tmpD1 = 0;
            if (tmpTum)
            {
                tmpD1 = tmpBet4 == 0
                    ? tmpBet3 * 25.4
                    : (tmpBet3 * 25.4) + (tmpBet4 / tmpBet5) * 25.4;
            }
            else if (!tmpV)
            {
                if (tmpBet3 > 100)
                    tmpD1 = tmpBet3;
                else if (tmpA < 430)
                    tmpD1 = tmpA - 20;
                else if (tmpA == 600)
                    tmpD1 = tmpA - 40;
                else
                    tmpD1 = tmpA - 30;
            }
            else if (tmpBet2 == 52)
                tmpD1 = 240;
            else if (tmpBet2 == 56)
                tmpD1 = 260;
            else if (tmpBet2 == 68)
                tmpD1 = tmpBet3;
            else
                tmpD1Error = "Fel inmatning";

            tmpD1 = Math.Round(tmpD1, 2, MidpointRounding.AwayFromZero);
            kv["SumD1"] = "(d1) " + (tmpD1Error ?? Fmt(tmpD1));
            kv["SumD1Tol"] = D1Upper(tmpD1, tmpBet2, tmpBet3) + " [2]";
            kv["SumD1TolN"] = D1Lower(tmpD1, tmpBet2, tmpBet3) + " [2]";

            double tmpD1B6 = tmpD1 + (tmpB6 * 2);
            double tmpC = tmpA + 15;
            kv["SumC"] = "(C) " + Fmt(tmpC);
            kv["SumCTol"] = " 0";
            kv["SumCTolN"] = H13Negative(tmpC);

            kv["SumStämpel"] = subject;
            kv["SumR"] = "R 1 ± 0.3";
            kv["SumRit"] = (tmpTum ? "7433532" : "7433529") + ":senaste utgåva";

            bool generalMachine = EqualsI(maskinVal, "Nakamura") || EqualsI(maskinVal, "LB45") || EqualsI(maskinVal, "LT-3000EX");
            kv["SumMaskinValS1"] = "Maskin: " + (generalMachine ? maskinVal : "");

            kv["SumF1_1"] = generalMachine ? "1/5" : string.IsNullOrEmpty(maskinVal) ? "1/1" : "";
            for (int i = 2; i <= 9; i++)
                kv["SumF1_" + i] = generalMachine || string.IsNullOrEmpty(maskinVal) ? "1/5" : "";
            kv["SumF1_0"] = generalMachine || string.IsNullOrEmpty(maskinVal) ? "1/5" : "";
            kv["SumF1_11"] = generalMachine || string.IsNullOrEmpty(maskinVal) ? "1/5" : "";

            string[] instruments =
            {
                "UD-Apparat / Mikrometer",
                "Skjutmått",
                "Skjutmått",
                "Skjutmått",
                "Skjutmått",
                "Mall",
                "Mall",
                "Mall",
                "Digitalt Skjutmått"
            };
            for (int i = 1; i <= 9; i++)
                kv["SumD1_" + i] = generalMachine ? instruments[i - 1] : "";
            kv["SumD1_0"] = generalMachine ? "Digital Djup/hakmått" : "";
            kv["SumD1_11"] = generalMachine ? "Digital Djup/hakmått" : "";

            kv["SumAF1_1"] = generalMachine ? "Tillhörande Ring/Klove" : "";
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = "";
            kv["SumAF1_6"] = generalMachine ? "Ritningsnr. mall: R1557494" : "";
            kv["SumAF1_7"] = generalMachine ? "Ritningsnr. mall: R497258" : "";
            kv["SumAF1_8"] = generalMachine ? "3.3mm JS15" : "";
            kv["SumAF1_9"] = generalMachine ? "Mäts: (d1+4mm) " + Fmt(tmpD1B6) + " ± 0.6" : "";
            kv["SumAF1_0"] = "";
            kv["SumAF1_11"] = "";

            kv["SumTextS1"] = "Trepunktsmätning utförs vid inställning. max variation 0,1mm<<LineBreak>>" +
                              "vid inställning mät innerdiameter d1 i två snitt<<LineBreak>>" +
                              "Bearbetas Ra 12,5 runt om, skarpa kanter avgradas. Max radie i botten på spår (B4) 1mm.";

            return kv;
        }

        private static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published)) return "";
            DateTime publishedDate;
            if (!DateTime.TryParse(published, out publishedDate)) return "";
            DateTime validUntil = publishedDate.AddDays(TmpDagar);
            if (DateTime.Today > validUntil.Date) return "";
            return "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom " + TmpDagar + " dagar)" +
                   "\n\n\n\n" +
                   "Information om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument" +
                   "\n\n\n\n" +
                   "Popupruta aktiv till " + validUntil.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        private static string Token(string[] tokens, int index)
        {
            return tokens.Length > index ? tokens[index] : "";
        }

        private static double ParseNum(string value)
        {
            double result;
            return double.TryParse((value ?? "").Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out result) ? result : 0;
        }

        private static bool EqualsI(string left, string right)
        {
            return string.Equals(left ?? "", right ?? "", StringComparison.OrdinalIgnoreCase);
        }

        private static string Fmt(double value)
        {
            if (Math.Abs(value) < 0.0000001) value = 0;
            return value.ToString("0.################", CultureInfo.InvariantCulture).Replace(".", ",");
        }

        private static string FmtComma(double value)
        {
            if (Math.Abs(value) < 0.0000001) value = 0;
            return value.ToString("0.################", CultureInfo.InvariantCulture).Replace(".", ",");
        }

        private static string H13Negative(double value)
        {
            if (value < 3.1) return "- 0.140";
            if (value < 6.1) return "- 0.180";
            if (value < 10.1) return "- 0.220";
            if (value < 18.1) return "- 0.270";
            if (value < 30.1) return "- 0.330";
            if (value < 50.1) return "- 0.390";
            if (value < 80.1) return "- 0.460";
            if (value < 120.1) return "- 0.540";
            if (value < 180.1) return "- 0.630";
            if (value < 250.1) return "- 0.720";
            if (value < 315.1) return "- 0.810";
            if (value < 400.1) return "- 0.890";
            if (value < 500.1) return "- 0.970";
            return "- 1.100";
        }

        private static string H13Positive(double value)
        {
            if (value < 3.1) return "+ 0.140";
            if (value < 6.1) return "+ 0.180";
            if (value < 10.1) return "+ 0.220";
            if (value < 18.1) return "+ 0.270";
            if (value < 30.1) return "+ 0.330";
            if (value < 50.1) return "+ 0.390";
            return "+ 0.460";
        }

        private static string Js15(double value)
        {
            if (value < 3.1) return "± 0.200";
            if (value < 6.1) return "± 0.240";
            if (value < 10.1) return "± 0.290";
            if (value < 18.1) return "± 0.350";
            if (value < 30.1) return "± 0.420";
            if (value < 50.1) return "± 0.500";
            return "± 0.600";
        }

        private static string Js16(double value)
        {
            return value < 3.1 ? "± 0.300" : "± 0.375";
        }

        private static string H15Positive(double value)
        {
            if (value < 3.1) return "+ 0.400";
            if (value < 6.1) return "+ 0.480";
            if (value < 10.1) return "+ 0.580";
            if (value < 18.1) return "+ 0.700";
            return "+ 0.840";
        }

        private static string D1Upper(double value, double bet2, double bet3)
        {
            if (bet2 == 68 && bet3 == 291) return "+ 0.271";
            if (value < 3.1) return "+ 0.020";
            if (value < 6.1) return "+ 0.028";
            if (value < 10.1) return "+ 0.035";
            if (value < 18.1) return "+ 0.043";
            if (value < 30.1) return "+ 0.053";
            if (value < 50.1) return "+ 0.064";
            if (value < 80.1) return "+ 0.076";
            if (value < 120.1) return "+ 0.090";
            if (value < 180.1) return "+ 0.106";
            if (value < 250.1) return "+ 0.122";
            if (value < 315.1) return "+ 0.137";
            if (value < 400.1) return "+ 0.151";
            if (value < 500.1) return "+ 0.165";
            return "+ 0.186";
        }

        private static string D1Lower(double value, double bet2, double bet3)
        {
            if (bet2 == 68 && bet3 == 291) return "+ 0.190";
            if (value < 3.1) return "+ 0.006";
            if (value < 6.1) return "+ 0.010";
            if (value < 10.1) return "+ 0.013";
            if (value < 18.1) return "+ 0.016";
            if (value < 30.1) return "+ 0.020";
            if (value < 50.1) return "+ 0.025";
            if (value < 80.1) return "+ 0.030";
            if (value < 120.1) return "+ 0.036";
            if (value < 180.1) return "+ 0.043";
            if (value < 250.1) return "+ 0.050";
            if (value < 315.1) return "+ 0.056";
            if (value < 400.1) return "+ 0.062";
            if (value < 500.1) return "+ 0.068";
            return "+ 0.076";
        }
    }
}
