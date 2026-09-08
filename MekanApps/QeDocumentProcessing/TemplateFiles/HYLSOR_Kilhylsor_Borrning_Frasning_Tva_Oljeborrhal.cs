using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Kilhylsor_Borrning_Frasning_Tva_Oljeborrhal : ITemplateCalculations
    {
        private static readonly string[] Machines = new[] { "Skepp 6", "VTR-160", "MacTurn 550" };

        private static readonly string[] Typ38 = { "44", "48", "52", "56", "60", "64", "68", "72", "76", "80", "84", "88", "92", "96", "500", "530", "560", "600", "630", "670", "710", "750", "800", "850", "900", "950", "1000", "1060", "1120", "1180", "1250", "1320", "1400" };
        private static readonly string[] Typ39 = { "44", "48", "52", "56", "60", "64", "68", "72", "76", "80", "84", "88", "92", "96", "500", "530", "560", "600", "630", "670", "710", "750", "800", "850", "900", "950", "1000", "1060", "1120", "1180", "1250" };

        private static readonly double[] L38 = { 0, 0, 55, 62, 72, 72, 72, 72, 87, 87, 87, 87, 102, 102, 102, 106, 106, 115, 130, 130, 140, 150, 160, 160, 165, 175, 195, 195, 208, 208, 215, 236, 254 };
        private static readonly double[] L39 = { 71, 71, 85, 85, 103, 103, 103, 103, 118, 118, 118, 132, 132, 140, 140, 150, 155, 165, 180, 185, 200, 206, 218, 224, 236, 250, 265, 280, 280, 300, 315 };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;
            List<Bookmark> bm = req != null ? req.Bookmarks : null;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.ToUpperInvariant().Trim();

            bool tmpSlash = tmpBet.IndexOf('/') >= 0;
            bool tmpLU = tmpBet.IndexOf("LU", StringComparison.OrdinalIgnoreCase) >= 0;
            bool tmpVZ = tmpBet.IndexOf("VZ", StringComparison.OrdinalIgnoreCase) >= 0;
            bool tmpV21 = tmpBet.IndexOf("V21", StringComparison.OrdinalIgnoreCase) >= 0;

            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : string.Empty;
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : string.Empty;
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : string.Empty;
            string tmpBet4 = tokens.Length > 3 ? tokens[3] : string.Empty;

            int tmpCount = tmpBet2.Length;
            bool tmpSpecial = tmpBet.Length > 11;

            string tmpSerie = tmpBet2.Length >= 2 ? tmpBet2.Substring(0, 2) : tmpBet2;
            string tmpTypStr = tmpCount > 3
                ? (tmpBet2.Length >= 2 ? tmpBet2.Substring(tmpBet2.Length - 2) : tmpBet2)
                : (tmpSlash ? tmpBet3 : tmpBet3);

            int serieInt = TryParseInt(tmpSerie);
            double typNum = TryParseDouble(tmpTypStr);

            int typLista = serieInt == 38 ? GetMember(tmpTypStr, Typ38) : GetMember(tmpTypStr, Typ39);

            double tmpKona = (tmpLU || serieInt == 39 || serieInt == 38) ? 12.0 : 30.0;
            kv["SumKona"] = "Kona  1:" + Fmt(tmpKona);

            double specialBm = GetDouble(bm, "Speciallängd");
            double tmpL;
            if (specialBm != 0)
            {
                tmpL = specialBm;
            }
            else if (tmpV21 || EqualsI(tmpBet1, "LU"))
            {
                tmpL = 0;
            }
            else
            {
                double[] lList = serieInt == 39 ? L39 : L38;
                tmpL = typLista >= 1 && typLista <= lList.Length ? lList[typLista - 1] : 0;
            }

            double amattBm = GetDouble(bm, "a-mått");

            double dBm = GetDouble(bm, "Ø Utvändigt Lillkona (d)");
            double tmpd;
            if (dBm != 0)
                tmpd = amattBm == 0 ? dBm : dBm + (amattBm / tmpKona);
            else if (tmpCount > 3)
                tmpd = typNum / 2.0 * 10.0;
            else
                tmpd = TryParseDouble(tmpBet3);

            double tmpd2 = Math.Round((tmpL / tmpKona) + tmpd, 2);

            double d1Bm = GetDouble(bm, "Ø Invändigt (d1)");
            double tmpd1;
            if (d1Bm != 0)
            {
                tmpd1 = d1Bm;
            }
            else if (!tmpSpecial)
            {
                tmpd1 = typNum < 61 ? tmpd - 10 : typNum < 501 ? tmpd - 15 : typNum < 671 ? tmpd - 20 : typNum < 901 ? tmpd - 25 : tmpd - 30;
            }
            else
            {
                tmpd1 = tmpCount > 3 ? TryParseDouble(tmpBet3) : TryParseDouble(tmpBet4);
            }

            kv["Sumd1"] = "(d1) " + Fmt(tmpd1);
            kv["Sumd1Tol"] = D1TolPos(typNum) + " \u003F";
            kv["Sumd1TolN"] = D1TolNeg(typNum) + " \u003F";

            kv["SumRa25"] = "2.5";
            kv["SumRa5"] = "5";

            bool showAngles = tmpLU || serieInt == 39 || serieInt == 38;
            kv["SumR"] = showAngles ? "R2" : "";
            kv["SumV30"] = "30º";
            kv["SumV45"] = showAngles ? "45º" : "";
            kv["SumV120"] = showAngles ? "120º" : "";

            double tmpC = GetDouble(bm, "Slits");
            kv["SumC"] = "(c) " + Fmt(tmpC);
            kv["SumCTol"] = tmpC < 6.01 ? "± 0.1" : "± 0.2";

            double tmpB = GetDouble(bm, "Längd till Oljespår (B)");
            kv["SumB"] = "(B) " + Fmt(tmpB).Replace(".",",");
            kv["SumBTol"] = GenTol(tmpB);

            int tmpSVRep = 12;
            double tmpAntRep = GetDouble(bm, "Antal repor (n)");
            kv["SumSVRep"] = tmpSVRep.ToString(CultureInfo.InvariantCulture) + "º";
            kv["SumAntRep"] = "(n) " + Fmt(tmpAntRep) + " axiella spår";

            double repBreddBm = GetDouble(bm, "RepBredd");
            double tmpRepB = repBreddBm != 0 ? repBreddBm
                : (serieInt == 39 && Math.Abs(typNum - 630) < 0.001 ? 1.0
                : (serieInt == 38 && Math.Abs(typNum - 850) < 0.001 ? 2.0 : 1.5));
            kv["SumRepB"] = "(RB) " + Fmt(tmpRepB);

            double repDjupBm = GetDouble(bm, "RepDjup");
            double tmpRepDval = repDjupBm != 0 ? repDjupBm
                : (serieInt == 39 && Math.Abs(typNum - 630) < 0.001 ? 0.5 : 1.0);
            kv["SumRepD"] = "(RD) " + Fmt1(tmpRepDval);

            double tmpJ = GetDouble(bm, "Längd till Repor (J)");
            kv["SumJ"] = "(J) " + Fmt(tmpJ);
            kv["SumJTol"] = GenTol(tmpJ);

            double tmpK = GetDouble(bm, "Längd Repor (K)");
            kv["SumK"] = "(K) " + Fmt(tmpK).Replace(".", ",");
            kv["SumKTol"] = GenTol(tmpK);

            double tmpDelVinkel = Math.Round((360.0 - (tmpSVRep * 2.0)) / (tmpAntRep - 1), 1);
            double tmp1val = (360.0 - (tmpDelVinkel * (tmpAntRep - 1))) / 2.0;
            kv["SumVDRep"] = Fmt(tmpDelVinkel).Replace(".",",") + "º";

            double tmpKonst1Rep = Math.Round(Math.Sin((tmp1val / 2.0) * Math.PI / 180.0) * 2.0, 4);
            double tmpKonstDelRep = Math.Round(Math.Sin((tmpDelVinkel / 2.0) * Math.PI / 180.0) * 2.0, 4);

            double tmpKorda1RepInv = Math.Round((tmpd1 / 2.0) * tmpKonst1Rep, 1);
            kv["SumK1i"] = Fmt(tmpKorda1RepInv - (tmpC / 2.0)).Replace(".", ",");

            double tmpKonUtr = (tmpB / tmpKona) + tmpd;
            double tmpKorda1RepUtv = Math.Round((tmpKonUtr / 2.0) * tmpKonst1Rep, 1);
            kv["SumK1u"] = Fmt(tmpKorda1RepUtv - (tmpC / 2.0)).Replace(".", ",");

            double tmpKordaDelRepInv = Math.Round((tmpd1 / 2.0) * tmpKonstDelRep, 1);
            kv["SumKDi"] = Fmt(tmpKordaDelRepInv).Replace(".",",").Replace(".", ",");

            double tmpKordaDelRepUtv = Math.Round((tmpKonUtr / 2.0) * tmpKonstDelRep, 1);
            kv["SumKDu"] = Fmt(tmpKordaDelRepUtv).Replace(".", ",").Replace(".", ",");

            int tmpSVOS = 10;
            kv["SumSVOS"] = tmpSVOS.ToString(CultureInfo.InvariantCulture) + "º";
            double tmpKonstOS = Math.Round(Math.Sin((tmpSVOS / 2.0) * Math.PI / 180.0) * 2.0, 4);
            double tmpKordaOSinv = Math.Round((tmpd1 / 2.0) * tmpKonstOS, 1);
            double tmpKordaOSutv = Math.Round((tmpKonUtr / 2.0) * tmpKonstOS, 1);
            kv["SumKOi"] = tmp1val < tmpSVOS ? "OjSpår utaför Repa" : Fmt(tmpKordaOSinv).Replace(".", ",");
            kv["SumKOu"] = tmp1val < tmpSVOS ? "OjSpår utaför Repa" : Fmt(tmpKordaOSutv).Replace(".", ",");

            double tmpE = GetDouble(bm, "Bredd Oljespår (E)");
            kv["SumE"] = "(E) " + Fmt(tmpE);
            kv["SumETol"] = tmpE < 6.01 ? "± 0.1" : "± 0.2";

            kv["Sumr1"] = OilR1(tmpE);

            string tmpFstr = FDepth(tmpE, serieInt, tmpd1);
            kv["SumF"] = tmpFstr;
            kv["SumFTol"] = "± 0.1";

            double borrhalBm = GetDouble(bm, "Ø Borrhål till Oljespår");
            kv["SumH"] = "Ø " + Fmt(borrhalBm);
            kv["SumH1"] = kv["SumH"];
            kv["SumHTol"] = "± 0.1";
            kv["SumH1Tol"] = "± 0.1";

            double tmpLOH = GetDouble(bm, "Längd Oljeborrhål");
            kv["SumLOH"] = "(LOH) " + Fmt(tmpLOH);
            kv["SumLOHTol"] = GenTol(tmpLOH);

            double tmpBOH = GetDouble(bm, "Ø Oljeborrhål");
            kv["SumBOH"] = "Ø " + Fmt(tmpBOH);
            kv["SumBOHTol"] = GenTol(tmpBOH);

            string sumOHG = GetString(bm, "Gänga Oljeborrhål");
            kv["SumOHG"] = sumOHG;
            kv["SumG"] = sumOHG;

            double tmpOHDj = GetDouble(bm, "Oljeborrhål Djup");
            double tmpOHGDj = GetDouble(bm, "OljehålsGänga Djup");
            double tmpBHM = GetDouble(bm, "Borrhålsmått (BH)");
            kv["SumOHDj"] = Fmt(tmpOHDj);
            kv["SumOHDjTol"] = tmpOHDj < 6.01 ? "± 0.1" : "± 0.2";
            kv["SumOHGDj"] = tmpOHGDj == 0 ? "" : "min " + Fmt(tmpOHGDj);
            kv["SumBHM"] = "(BH) " + Fmt(tmpBHM);

            double tmpVOH = GetDouble(bm, "Vinkel Oljeborrhål");
            kv["SumVOH"] = Fmt(tmpVOH);
            double tmpKonstVOH = Math.Round(Math.Sin((tmpVOH / 2.0) * Math.PI / 180.0) * 2.0, 4);
            double tmpKordaVOH = Math.Round(((tmpd1 + (tmpBHM * 2.0)) / 2.0) * tmpKonstVOH, 1);
            kv["SumKOBH"] = Fmt(tmpKordaVOH).Replace(".", ",");

            string sum1 = Math.Abs(tmpVOH - 90.0) < 0.001 ? "B2" : "B";
            kv["Sum1"] = sum1;
            kv["Sum2"] = "(" + sum1 + ") " + Fmt(tmpKordaVOH).Replace(".", ",");

            SumMaskinVal(kv, maskinVal);
            SumFrequencies(kv, maskinVal);
            SumMeasuringDevices(kv, maskinVal);
            SumAF(kv, maskinVal);

            kv["SumTextS1"] = "Bryt alla kanter, avlägsna";

            string ritBm = GetString(bm, "Ritningsnummer");
            bool ritZero = string.IsNullOrEmpty(ritBm) || EqualsI(ritBm, "0");
            string sumRit;
            if (ritZero)
            {
                if (serieInt == 38 && !tmpSpecial) sumRit = "238000";
                else if (serieInt == 39 && !tmpSpecial) sumRit = "226472";
                else sumRit = "Styckritning: " + subject;
            }
            else
            {
                sumRit = "Styckritning: " + ritBm;
            }
            kv["SumRitningsnr"] = sumRit;
            kv["SumRitTol"] = "Toleranser: 1432010, 7437495";
            kv["SumRitYtjämnhet"] = "Yta: 7430184";
            kv["SumKlEgenskaper"] = "PRODUCTION NUTS & SLEEVES & HOUSINGS\\ALLMÄNKLASSADE EGENSKAPER\\Klassade egenskaper kilhylsor";

            string textBm = GetString(bm, "Text");
            bool textEmpty = string.IsNullOrEmpty(textBm) || EqualsI(textBm, "0");
            string[] textParts = textEmpty ? new string[0] : textBm.Split('§');
            kv["SumText1"] = textEmpty || textParts.Length < 1 ? "" : textParts[0];
            kv["SumText2"] = textEmpty || textParts.Length < 2 ? "" : textParts[1];
            kv["SumText3"] = textEmpty || textParts.Length < 3 ? "" : textParts[2];
            kv["SumText4"] = textEmpty || textParts.Length < 4 ? "" : textParts[3];

            return kv;
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
                       " dagar)\n\nInformation om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument\n\nPopupruta aktiv till " +
                       validTill.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return "";
        }

        private static void SumMaskinVal(Dictionary<string, string> kv, string maskinVal)
        {
            string s = IsMachine(maskinVal) ? maskinVal : "";
            kv["SumMaskinValS1"] = ("Maskin: " + s + " - Borrning, Fräsning").TrimStart();
        }

        private static void SumFrequencies(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsMachine(maskinVal);
            bool orEmpty = m || string.IsNullOrEmpty(maskinVal);
            kv["SumF1_1"] = m ? "1/1" : (string.IsNullOrEmpty(maskinVal) ? "1/1" : "");
            kv["SumF1_2"] = m ? "1/1" : (string.IsNullOrEmpty(maskinVal) ? "1/1" : "");
            kv["SumF1_3"] = m ? "1/1" : "";
            kv["SumF1_4"] = m ? "1/1" : "";
        }

        private static void SumMeasuringDevices(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsMachine(maskinVal);
            kv["SumD1_1"] = m ? "Gängtolk" : "";
            kv["SumD1_2"] = m ? "Skjutmått" : "";
            kv["SumD1_3"] = m ? "Skjutmått" : "";
            kv["SumD1_4"] = m ? "Skjutmått" : "";
        }

        private static void SumAF(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsMachine(maskinVal);
            bool orEmpty = m || string.IsNullOrEmpty(maskinVal);
            kv["SumAF1_1"] = m ? "" : (string.IsNullOrEmpty(maskinVal) ? "" : "");
            kv["SumAF1_2"] = m ? "" : (string.IsNullOrEmpty(maskinVal) ? "" : "");
            kv["SumAF1_3"] = m ? "Tolerans efter slits enl. 7437495" : (string.IsNullOrEmpty(maskinVal) ? "" : "");
            kv["SumAF1_4"] = m ? "" : (string.IsNullOrEmpty(maskinVal) ? "" : "");
        }

        private static bool IsMachine(string mv)
        {
            if (string.IsNullOrEmpty(mv)) return false;
            for (int i = 0; i < Machines.Length; i++)
                if (string.Equals(Machines[i], mv, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        private static string D1TolPos(double typ)
        {
            if (typ < 53) return "+ 0.185";
            if (typ < 65) return "+ 0.210";
            if (typ < 85) return "+ 0.360";
            if (typ < 535) return "+ 0.400";
            if (typ < 675) return "+ 0.440";
            if (typ < 855) return "+ 0.500";
            return "+ 0.500";
        }

        private static string D1TolNeg(double typ)
        {
            if (typ < 53) return "- 0.290";
            if (typ < 65) return "- 0.320";
            if (typ < 85) return "- 0.570";
            if (typ < 535) return "- 0.630";
            if (typ < 675) return "- 0.700";
            if (typ < 855) return "- 0.800";
            return "- 0.900";
        }

        private static string OilR1(double E)
        {
            if (E == 5) return "R4";
            if (E == 6) return "R4,5";
            if (E == 7) return "R5";
            if (E == 8) return "R6";
            if (E == 10) return "R7";
            return "R8";
        }

        private static string FDepth(double E, int serie, double d1)
        {
            if (E == 5) return "1";
            if (E == 6) return "1.2";
            if (E == 7) return "1.5";
            if (E == 8) return "1.5";
            if (E == 10) return "2";
            if (serie == 39 && Math.Abs(d1 - 1143) < 0.001) return "2.5";
            return "2.7";
        }

        private static string GenTol(double v)
        {
            if (v < 6.01) return "± 0.1";
            if (v < 30.01) return "± 0.2";
            if (v < 120.01) return "± 0.3";
            if (v < 400.01) return "± 0.5";
            if (v < 1000.01) return "± 0.8";
            if (v < 2000.01) return "± 1.2";
            return "± 2.0";
        }

        private static int GetMember(string val, string[] list)
        {
            for (int i = 0; i < list.Length; i++)
                if (string.Equals(list[i], val, StringComparison.OrdinalIgnoreCase)) return i + 1;
            return 0;
        }

        private static bool EqualsI(string a, string b)
            => string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);

        private static int TryParseInt(string s)
        {
            int v;
            return int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static double TryParseDouble(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;
            double v;
            return double.TryParse(s.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static double GetDouble(List<Bookmark> bm, string key)
        {
            string raw = GetString(bm, key);
            if (string.IsNullOrWhiteSpace(raw)) return 0;
            double v;
            return double.TryParse(raw.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
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

        private static string Fmt(double v) => v.ToString(CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt1(double v) => v.ToString("F1", CommonFunctions.Culture).Replace(",", ".");
    }
}