using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class KRAGAR_SL_TK_N_Narrow_44_600_LARGE : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;
            List<Bookmark> bm = req != null ? req.Bookmarks : null;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = (subject ?? string.Empty).Trim().ToUpperInvariant();
            string tmpBet = tmpFormat.Replace(".", ",");

            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet2 = Present(tokens.Length > 1 ? tokens[1] : "");
            string tmpBet3 = Present(tokens.Length > 2 ? tokens[2] : "");

            string tmpTyp = tmpBet3.Length > 2 ? tmpBet3 : tmpBet2;
            double typ = TryParseDouble(tmpTyp);

            string Kon0 = Fmt1(0);
            string Kon01 = Fmt3(0.1);
            string Kon015 = Fmt3(0.15);
            string Kon02 = Fmt3(0.2);
            string Kon025 = Fmt3(0.25);
            string Kon03 = Fmt3(0.3);
            string Kon04 = Fmt3(0.4);
            string Kon05 = Fmt3(0.5);
            string Kon10 = Fmt1(1);
            string Kon30 = Fmt1(3);
            string Kon40 = Fmt1(4);

            double[] dLista = DTab(typ);
            double[] bLista = BTab(typ);

            double tmpd1 = Pick(GetDouble(bm, "Ø d1"), dLista, 1);
            double tmpd2 = Pick(GetDouble(bm, "Ø d2"), dLista, 2);
            double tmpd3 = Pick(GetDouble(bm, "Ø d3"), dLista, 3);
            double tmpd4 = Pick(GetDouble(bm, "Ø d4"), dLista, 4);
            double tmpd5 = Pick(GetDouble(bm, "Ø d5"), dLista, 5);
            double tmpd6 = Pick(GetDouble(bm, "Ø d6"), dLista, 6);
            double tmpd7 = Pick(GetDouble(bm, "Ø d7"), dLista, 7);
            double tmpd8 = Pick(GetDouble(bm, "Ø d8"), dLista, 8);
            double tmpd9 = Pick(GetDouble(bm, "Ø d9"), dLista, 9);
            double tmpd10 = Pick(GetDouble(bm, "Ø d10"), dLista, 10);
            double tmpd11 = Pick(GetDouble(bm, "Ø d11"), dLista, 11);
            double tmpd12 = Pick(GetDouble(bm, "Ø d12"), dLista, 12);

            kv["Sumd1"] = "(d1) " + FmtDot(tmpd1);
            kv["Sumd1Tol"] = "+ " + Kon40;
            kv["Sumd1TolN"] = "- " + " " + Kon0;

            kv["Sumd2"] = "(d2) " + FmtDot(tmpd2);
            kv["Sumd2Tol"] = "+ " + Fmt3(H12(tmpd2));
            kv["Sumd2TolN"] = "- " + " " + Kon0;

            kv["Sumd3"] = "(d3) " + FmtDot(tmpd3);
            kv["Sumd3Tol"] = "+ " + Kon02;
            kv["Sumd3TolN"] = "+ " + Kon01;

            kv["Sumd4"] = "(d4) " + FmtDot(tmpd4);
            kv["Sumd4Tol"] = "+ " + Kon0;
            kv["Sumd4TolN"] = "- " + " " + Fmt3(h12(tmpd4));

            kv["Sumd5"] = "(d5) " + FmtDot(tmpd5);
            kv["Sumd5Tol"] = "+ " + Fmt3(H12(tmpd5));
            kv["Sumd5TolN"] = "- " + " " + Kon0;

            kv["Sumd6"] = "(d6) " + FmtDot(tmpd6);
            kv["Sumd6Tol"] = "+ " + Kon0;
            kv["Sumd6TolN"] = "- " + " " + Fmt3(h12(tmpd6));

            kv["Sumd7"] = "(d7) " + FmtDot(tmpd7);
            kv["Sumd7Tol"] = "+ " + Fmt3(H12(tmpd7));
            kv["Sumd7TolN"] = "- " + " " + Kon0;

            kv["Sumd8"] = "(d8) " + FmtDot(tmpd8);
            kv["Sumd8Tol"] = "+ " + Kon0;
            kv["Sumd8TolN"] = "- " + " " + Fmt3(h12(tmpd8));

            kv["Sumd9"] = "(d9) " + FmtDot(tmpd9);
            kv["Sumd9Tol"] = "+ " + Fmt3(H12(tmpd9));
            kv["Sumd9TolN"] = "- " + " " + Kon0;

            kv["Sumd10"] = "(d10) " + FmtDot(tmpd10);
            kv["Sumd10Tol"] = "+ " + Kon0;
            kv["Sumd10TolN"] = "- " + " " + Fmt3(h12(tmpd10));

            kv["Sumd11"] = "(d11) " + FmtDot(tmpd11);
            kv["Sumd11Tol"] = "+ " + Fmt3(H12(tmpd11));
            kv["Sumd11TolN"] = "- " + " " + Kon0;

            kv["Sumd12"] = "(d12) " + FmtDot(tmpd12);
            kv["Sumd12Tol"] = "± " + Kon30;

            double tmpd13 = typ < 68 ? 16 : 18;
            kv["Sumd13"] = typ > 76 ? "n/a" : "3x Ø " + FmtDot(tmpd13);

            double tmpB = Pick(GetDouble(bm, "Bredd B"), bLista, 1);
            double tmpb1 = Pick(GetDouble(bm, "Bredd b1"), bLista, 2);
            double tmpb2 = Pick(GetDouble(bm, "Bredd b2"), bLista, 3);
            double tmpb3 = Pick(GetDouble(bm, "Bredd b3"), bLista, 4);
            double tmpb4 = Pick(GetDouble(bm, "Bredd b4"), bLista, 5);
            double tmpb5 = Pick(GetDouble(bm, "Bredd b5"), bLista, 6);
            double tmpb6 = Pick(GetDouble(bm, "Bredd b6"), bLista, 7);
            double bmb7 = GetDouble(bm, "Bredd b7");
            double tmpb7 = bmb7 == 0 ? (typ < 44 ? 9 : 14.5) : bmb7;

            kv["SumB"] = "(B) " + FmtDot(tmpB);
            kv["SumBTol"] = "+ " + Kon10;
            kv["SumBTolN"] = "+ " + Kon0;

            kv["Sumb1"] = "(b1) " + FmtDot(tmpb1);
            kv["Sumb1Tol"] = "± " + Kon10;

            kv["Sumb2"] = "(b2) " + FmtDot(tmpb2);
            kv["Sumb2Tol"] = "+ " + Kon05;
            kv["Sumb2TolN"] = "- " + " " + Kon0;

            kv["Sumb3"] = "(b3) " + FmtDot(tmpb3);
            kv["Sumb3Tol"] = "+ " + Kon03;
            kv["Sumb3TolN"] = "- " + " " + Kon0;

            kv["Sumb4"] = "(b4) " + FmtDot(tmpb4);
            kv["Sumb4Tol"] = "+ " + Kon025;
            kv["Sumb4TolN"] = "- " + " " + Kon0;

            kv["Sumb5"] = "(b5) " + FmtDot(tmpb5);
            kv["Sumb5Tol"] = "± " + Kon10;

            kv["Sumb6"] = "3x (b6) " + FmtDot(tmpb6);
            kv["Sumb6Tol"] = "+ " + Kon0;
            kv["Sumb6TolN"] = "- " + " " + Kon05;

            kv["Sumb7"] = "(b7) " + FmtDot(tmpb7);
            kv["Sumb7Tol"] = "+ " + Kon04;
            kv["Sumb7TolN"] = "- " + " " + Kon0;

            int tmpG = typ < 44 ? 8 : typ < 68 ? 10 : 12;
            kv["SumG"] = "(G) M" + tmpG.ToString(CultureInfo.InvariantCulture) + " (3x)";

            kv["SumR08"] = "R max: 0.8 (8x)";
            kv["SumR05"] = "R0.5 (2x)";
            kv["SumR05a"] = "max R0.5 (3x)";
            kv["SumR4"] = "R4";

            double tmpF1 = typ < 68 ? 1.5 : 2;
            kv["SumF1"] = "3x " + FmtDot(tmpF1) + " x45°";
            kv["SumF2"] = "2x 1x45°";

            kv["SumCo"] = " " + Kon015;
            kv["SumCo1"] = " " + (typ < 68 ? Kon015 : Kon025);

            kv["SumRa32"] = "3.2";
            kv["SumRa125"] = typ > 76 ? "n/a" : "12.5";

            kv["SumMaskinvalS1"] = "Maskin: " + maskinVal + " - Diametrala mått.";
            kv["SumMaskinvalS2"] = "Maskin: " + maskinVal + " - Övriga mått.";

            bool mOK = EqualsI(maskinVal, "Nakamura") || EqualsI(maskinVal, "LB45") || EqualsI(maskinVal, "LT-3000EX");

            kv["SumF1_1"] = mOK ? "1/2" : "";
            kv["SumF1_2"] = mOK ? "1/2" : "";
            kv["SumF1_3"] = mOK ? "1/2" : "";
            kv["SumF1_4"] = mOK ? "1/2" : "";
            kv["SumF1_5"] = "";
            kv["SumF1_6"] = mOK ? "1/2" : "";
            kv["SumF1_7"] = mOK ? "1/2" : "";

            kv["SumF2_1"] = mOK ? "1/2" : "";
            kv["SumF2_2"] = mOK ? "1/2" : "";
            kv["SumF2_3"] = mOK ? "1/2" : "";
            kv["SumF2_4"] = mOK ? "1/2" : "";
            kv["SumF2_5"] = mOK ? "1/2" : "";
            kv["SumF2_6"] = mOK ? "1/2" : "";
            kv["SumF2_7"] = mOK ? "1/2" : "";

            kv["SumD1_1"] = mOK ? "Mätmaskin alt. Skjutmått" : "";
            kv["SumD1_2"] = mOK ? "Mätmaskin alt.UD-apparat eller Mikrometer" : "";
            kv["SumD1_3"] = mOK ? "Mätmaskin alt.Skjutmått" : "";
            kv["SumD1_4"] = mOK ? "Ytjämnhetsmätare" : "";
            kv["SumD1_5"] = "";
            kv["SumD1_6"] = mOK ? "Mätmaskin alt.Mätservice" : "";
            kv["SumD1_7"] = mOK ? "Mätmaskin alt.Skjutmått" : "";

            kv["SumD2_1"] = mOK ? "Mätmaskin alt.Skjutmått" : "";
            kv["SumD2_2"] = mOK ? "Mätmaskin alt.Skjutmått" : "";
            kv["SumD2_3"] = mOK ? "Mätmaskin alt.Skjutmått" : "";
            kv["SumD2_4"] = mOK ? "Gängtolk" : "";
            kv["SumD2_5"] = mOK ? "Mätmaskin alt.Skjutmått" : "";
            kv["SumD2_6"] = mOK ? "Ytjämnhetsmätare" : "";
            kv["SumD2_7"] = mOK ? "Mätmaskin alt.Skjutmått" : "";

            kv["SumAF1_1"] = "";
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = mOK ? "Övriga bearbetade ytor 6.3" : "";
            kv["SumAF1_5"] = "";
            kv["SumAF1_6"] = "";
            kv["SumAF1_7"] = "";

            kv["SumAF2_1"] = "";
            kv["SumAF2_2"] = "";
            kv["SumAF2_3"] = "";
            kv["SumAF2_4"] = "";
            kv["SumAF2_5"] = "";
            kv["SumAF2_6"] = mOK ? "Övriga bearbetade ytor 6.3" : "";
            kv["SumAF2_7"] = "";

            string sumRitNr = "7440047: senaste utgåva";
            kv["SumRitNr"] = sumRitNr;
            kv["SumRitNr2"] = sumRitNr;

            string sumText = "Okulärkontroll Grader, frifläckar, slagmärken, repor, valkar, ytjämnhet och faser, skarpa kanter avgradas.";
            kv["SumTextS1"] = sumText;
            kv["SumTextS2"] = sumText;

            return kv;
        }

        private static string ComputePopup(string pub)
        {
            if (string.IsNullOrWhiteSpace(pub)) return "";
            DateTime dt;
            if (!DateTime.TryParse(pub, out dt)) return "";
            DateTime till = dt.AddDays(14);
            if (DateTime.Today > till.Date) return "";
            return "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)\n\n"
                 + "\n\n"
                 + "Information om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument\n\n"
                 + "Popupruta aktiv till " + till.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        private static double[] DTab(double typ)
        {
            if (typ == 34) return new double[] { 0, 160, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            if (typ == 52) return new double[] { 261, 250, 240, 254, 286, 294, 312, 320, 338, 346, 364, 378 };
            if (typ == 36 || typ == 38 || typ == 40 || typ == 44 || typ == 48 || typ == 56 || typ == 60 ||
                typ == 64 || typ == 68 || typ == 72 || typ == 76 || typ == 80 || typ == 84 || typ == 88 ||
                typ == 92 || typ == 96 || typ == 500 || typ == 530 || typ == 560 || typ == 600)
                return new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            return new double[0];
        }

        private static double[] BTab(double typ)
        {
            if (typ == 52) return new double[] { 36, 14.5, 10.5, 4.5, 8, 9, 28 };
            if (typ == 34 || typ == 36 || typ == 38 || typ == 40 || typ == 44 || typ == 48 || typ == 56 ||
                typ == 60 || typ == 64 || typ == 68 || typ == 72 || typ == 76 || typ == 80 || typ == 84 ||
                typ == 88 || typ == 92 || typ == 96 || typ == 500 || typ == 530 || typ == 560 || typ == 600)
                return new double[] { 0 };
            return new double[0];
        }

        private static double Pick(double bmValue, double[] lista, int index)
        {
            if (bmValue != 0) return bmValue;
            return (lista != null && index >= 1 && index <= lista.Length) ? lista[index - 1] : 0;
        }

        private static double H12(double d)
        {
            if (d < 3.01) return 0.100; if (d < 6.01) return 0.120; if (d < 10.01) return 0.150;
            if (d < 18.01) return 0.180; if (d < 30.01) return 0.210; if (d < 50.01) return 0.250;
            if (d < 80.01) return 0.300; if (d < 120.01) return 0.350; if (d < 180.01) return 0.400;
            if (d < 250.01) return 0.460; if (d < 315.01) return 0.520; if (d < 400.01) return 0.570;
            if (d < 500.01) return 0.630; if (d < 630.01) return 0.700; if (d < 800.01) return 0.800;
            if (d < 1000.01) return 0.900; if (d < 1250.01) return 1.050; if (d < 1600.01) return 1.250;
            if (d < 2000.01) return 1.500; if (d < 2500.01) return 1.750;
            return 2.100;
        }

        private static double h12(double d)
        {
            if (d < 3.01) return 0.100; if (d < 6.01) return 0.120; if (d < 10.01) return 0.150;
            if (d < 18.01) return 0.180; if (d < 30.01) return 0.210; if (d < 50.01) return 0.250;
            if (d < 80.01) return 0.300; if (d < 120.01) return 0.350; if (d < 180.01) return 0.400;
            if (d < 250.01) return 0.460; if (d < 315.01) return 0.520; if (d < 400.01) return 0.570;
            if (d < 500.01) return 0.630;
            return 0.700;
        }

        private static string Present(string token)
        {
            if (string.IsNullOrEmpty(token)) return "";
            double v;
            if (double.TryParse(token.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) && v != 0)
                return v.ToString("0.################", CultureInfo.InvariantCulture).Replace(".", ",");
            return token;
        }

        private static bool EqualsI(string a, string b)
        {
            return string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);
        }

        private static double TryParseDouble(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;
            double v;
            return double.TryParse(s.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static double GetDouble(List<Bookmark> bm, string key)
        {
            string r = GetString(bm, key);
            if (string.IsNullOrWhiteSpace(r)) return 0;
            double v;
            return double.TryParse(r.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
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

        private static string FmtDot(double v)
        {
            return v.ToString("0.################", CommonFunctions.Culture).Replace(",", ".");
        }

        private static string Fmt3(double v)
        {
            return v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
        }

        private static string Fmt1(double v)
        {
            return v.ToString("F1", CommonFunctions.Culture).Replace(",", ".");
        }
    }
}