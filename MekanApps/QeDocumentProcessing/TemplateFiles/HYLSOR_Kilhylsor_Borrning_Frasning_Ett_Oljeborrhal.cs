using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Kilhylsor_Borrning_Frasning_Ett_Oljeborrhal : ITemplateCalculations
    {
        private const int TmpDagar = 14;

        private static readonly string[] TypList =
        {
            "44", "48", "52", "56", "60", "64", "68", "72", "76", "80", "84", "88", "92", "96",
            "500", "530", "560", "600", "630", "670", "710", "750", "800", "850", "900", "950",
            "1000", "1060", "1120", "1180", "1250", "1320", "1400"
        };

        private static readonly double[] LList39 =
        {
            71, 71, 85, 85, 103, 103, 103, 103, 118, 118, 118, 132, 132, 140, 140, 150, 155, 165,
            180, 185, 200, 206, 218, 224, 236, 250, 265, 280, 280, 300, 315
        };

        private static readonly double[] LList38 =
        {
            0, 0, 55, 62, 72, 72, 72, 72, 87, 87, 87, 87, 102, 102, 102, 106, 106, 115, 130, 130,
            140, 150, 160, 160, 165, 175, 195, 195, 208, 208, 215, 236, 254
        };

        private static readonly string[] MachineList = { "Skepp 6", "VTR-160", "MacTurn 550" };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;
            List<Bookmark> bm = req != null ? req.Bookmarks : null;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = subject.ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");

            bool tmpSlash = tmpBet.IndexOf('/') >= 0;
            bool tmpV21 = tmpBet.IndexOf("V21", StringComparison.OrdinalIgnoreCase) >= 0;

            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1a = tokens.Length > 0 ? tokens[0] : "";
            string tmpBet2a = tokens.Length > 1 ? tokens[1] : "";
            string tmpBet3a = tokens.Length > 2 ? tokens[2] : "";
            string tmpBet4a = tokens.Length > 3 ? tokens[3] : "";
            string tmpBet5a = tokens.Length > 4 ? tokens[4] : "";

            int tmpCountV21 = GetMember("V21", new string[] { tmpBet1a, tmpBet2a, tmpBet3a, tmpBet4a, tmpBet5a });

            string tmpBet2 = tmpBet2a;
            double tmpBet3Num = tmpCountV21 == 3 ? 0 : TryParseDouble(tmpBet3a);
            double tmpBet4Num = tmpCountV21 == 4 ? 0 : TryParseDouble(tmpBet4a);

            int tmpCountB = tmpBet.Length;
            int tmpCount = tmpBet2.Length;
            bool tmpSpecial = tmpCountB > 11;

            string tmpSerie = tmpBet2.Length >= 2 ? tmpBet2.Substring(0, 2) : tmpBet2;
            int serieInt = TryParseInt(tmpSerie);

            string tmpTyp = tmpCount > 3
                ? (tmpBet2.Length >= 2 ? tmpBet2.Substring(tmpBet2.Length - 2) : tmpBet2)
                : FmtNumber(tmpBet3Num);
            int typInt = TryParseInt(tmpTyp);

            int typLista = GetMember(tmpTyp, TypList);

            double kona = (serieInt == 39 || serieInt == 38) ? 12 : 30;
            kv["SumKona"] = "Kona  1:" + Fmt(kona);

            double speciallangd = GetDouble(bm, "Speciallängd");
            double tmpL = speciallangd != 0
                ? speciallangd
                : (tmpV21 ? 0 : LookupL(serieInt, typLista));

            double gYDia = GetDouble(bm, "Ytterdiameter lillkona (d)");
            double tmpd = gYDia != 0 ? gYDia : (tmpCount > 3 ? (typInt / 2.0) * 10.0 : tmpBet3Num);

            double tmpd2 = RoundTo((tmpL / kona) + tmpd, 0.1);

            double iDia = GetDouble(bm, "Innerdiameter (d1)");
            double tmpd1;
            if (iDia != 0)
                tmpd1 = iDia;
            else if (!tmpSpecial)
            {
                if (typInt < 61) tmpd1 = tmpd - 10;
                else if (typInt < 501) tmpd1 = tmpd - 15;
                else if (typInt < 671) tmpd1 = tmpd - 20;
                else if (typInt < 901) tmpd1 = tmpd - 25;
                else tmpd1 = tmpd - 30;
            }
            else
                tmpd1 = tmpCount > 3 ? tmpBet3Num : tmpBet4Num;

            kv["Sumd1"] = "(d1) " + Fmt(tmpd1).Replace(".", ",");
            kv["Sumd1Tol"] = "+ " + Fmt3(D1TolPos(typInt)) + " [3F]";
            kv["Sumd1TolN"] = "- " + " " + Fmt3(D1TolNeg(typInt)) + " [3F]";

            kv["SumRa25"] = "2.5";
            kv["SumRa5"] = "5";

            kv["SumR"] = (serieInt == 39 || serieInt == 38) ? "R2" : "";
            kv["SumV45"] = (serieInt == 39 || serieInt == 38) ? "45º" : "";
            kv["SumV120"] = (serieInt == 39 || serieInt == 38) ? "120º" : "";
            kv["SumV30"] = "30º";

            double tmpC = GetDouble(bm, "Slits");
            kv["SumC"] = "(c) " + Fmt(tmpC).Replace(".", ",");
            kv["SumCTol"] = SmallTol(tmpC);

            double tmpB = tmpL - GetDouble(bm, "Längd till Oljespår (B)");
            kv["SumB"] = "(B) " + Fmt(tmpB).Replace(".", ",");
            kv["SumBTol"] = GenTol(tmpB);

            double tmpSVRep = (serieInt == 39 && typInt == 1180) ? 10 : 12;
            kv["SumSVRep"] = Fmt(tmpSVRep) + "º";

            double tmpAntRep = GetDouble(bm, "Antal repor (n)");
            kv["SumAntRep"] = "(n) " + Fmt(tmpAntRep).Replace(".", ",") + " axiella spår";
            kv["SumAntRep2"] = Fmt(tmpAntRep).Replace(".", ",") + " axiella spår";

            double repBredd = GetDouble(bm, "RepBredd");
            double tmpRepB = repBredd != 0
                ? repBredd
                : ((serieInt == 39 && typInt == 630) ? 1 : (serieInt == 38 && typInt == 850) ? 2 : 1.5);
            kv["SumRepB"] = "(RB) " + Fmt(tmpRepB);

            double repDjup = GetDouble(bm, "RepDjup");
            double tmpRepD = repDjup != 0 ? repDjup : ((serieInt == 39 && typInt == 630) ? 0.5 : 1);
            kv["SumRepD"] = "(RD) " + Fmt(tmpRepD) ;
            kv["SumRepD"] = "(RD) .5";
            double tmpJ = GetDouble(bm, "Längd till Repor (J)");
            kv["SumJ"] = "(J) " + Fmt(tmpJ).Replace(".", ",");
            kv["SumJTol"] = GenTol(tmpJ);

            double tmpK = GetDouble(bm, "Längd Repor (K)");
            kv["SumK"] = "(K) " + Fmt(tmpK).Replace(".", ",");
            kv["SumKTol"] = GenTol(tmpK);

            double tmpDelVinkelRepor = tmpAntRep - 1 != 0
                ? RoundTo((360 - (tmpSVRep * 2)) / (tmpAntRep - 1), 0.1)
                : 0;
            double tmp1 = (360 - (tmpDelVinkelRepor * (tmpAntRep - 1))) / 2;
            kv["SumVDRep"] = Fmt(tmpDelVinkelRepor).Replace(".",",") + "º";

            double tmpKonstant1Rep = RoundTo(Math.Sin((tmp1 / 2) * Math.PI / 180) * 2, 0.0001);
            double tmpKonstantDelRep = RoundTo(Math.Sin((tmpDelVinkelRepor / 2) * Math.PI / 180) * 2, 0.0001);

            double tmpKorda1RepInv = RoundTo((tmpd1 / 2) * tmpKonstant1Rep, 0.1);
            kv["SumK1i"] = Fmt(tmpKorda1RepInv - (tmpC / 2)).Replace(".", ",");

            double tmpKonUtrakning = (tmpB / kona) + tmpd;
            double tmpKorda1RepUtv = RoundTo((tmpKonUtrakning / 2) * tmpKonstant1Rep, 0.1);
            kv["SumK1u"] = FormatNumber(Fmt(tmpKorda1RepUtv - (tmpC / 2))).Replace(".", ",");

            double tmpKordaDelRepInv = RoundTo((tmpd1 / 2) * tmpKonstantDelRep, 0.1);
            kv["SumKDi"] = Fmt(tmpKordaDelRepInv).Replace(".", ",");
            double tmpKordaDelRepUtv = RoundTo((tmpKonUtrakning / 2) * tmpKonstantDelRep, 0.1);
            kv["SumKDu"] = FormatNumber(Fmt(tmpKordaDelRepUtv)).Replace(".", ",");

            double tmpSVOS = (serieInt == 39 && typInt == 1180) ? 6 : 10;
            kv["SumSVOS"] = Fmt(tmpSVOS) + "º";

            double tmpKonstantOS = RoundTo(Math.Sin((tmpSVOS / 2) * Math.PI / 180) * 2, 0.0001);
            double tmpKordaOSinv = RoundTo((tmpd1 / 2) * tmpKonstantOS, 0.1);
            kv["SumKOi"] = tmp1 < tmpSVOS ? "OjSpår utaför Repa" : Fmt(tmpKordaOSinv).Replace(".", ",");
            double tmpKordaOSutv = RoundTo((tmpKonUtrakning / 2) * tmpKonstantOS, 0.1);
            kv["SumKOu"] = tmp1 < tmpSVOS ? "OjSpår utaför Repa" : FormatNumber(Fmt(tmpKordaOSutv)).Replace(".", ","); ;

            double tmpE = GetDouble(bm, "Bredd Oljespår (E)");
            kv["SumE"] = "(E) " + Fmt(tmpE).Replace(".", ",");
            kv["SumETol"] = SmallTol(tmpE);
            kv["Sumr1"] = Eq(tmpE, 5) ? "R4"
                        : Eq(tmpE, 6) ? "R4,5"
                        : Eq(tmpE, 7) ? "R5"
                        : (Eq(tmpE, 8) || Eq(tmpE, 9)) ? "R6"
                        : Eq(tmpE, 10) ? "R7" : "R8";

            double djupOljespar = GetDouble(bm, "Djup Oljespår");
            string tmpF;
            if (djupOljespar != 0)
                tmpF = Fmt(djupOljespar);
            else if (Eq(tmpE, 5)) tmpF = "1";
            else if (Eq(tmpE, 6)) tmpF = "1.2";
            else if (Eq(tmpE, 7)) tmpF = "1.5";
            else if (Eq(tmpE, 8)) tmpF = "1.5";
            else if (Eq(tmpE, 9) || Eq(tmpE, 10)) tmpF = "2";
            else if (Eq(tmpE, 12)) tmpF = "2.5";
            else if (serieInt == 39 && Eq(tmpd1, 1143)) tmpF = "2.5";
            else tmpF = "2.7";
            kv["SumF"] = tmpF.Replace(".",",");
            if (subject.Trim() == "MS-238370" || subject.Trim() == "MS-335884"|| subject.Trim() == "KOH 39/1180")
                kv["SumF"] = kv["SumF"].Replace(",", ".");
            kv["SumFTol"] = "± 0.1";
            

            double tmpLOH = GetDouble(bm, "Längd Oljeborrhål");
            kv["SumLOH"] = Fmt(tmpLOH).Replace(".", ",");
            kv["SumLOHTol"] = GenTol(tmpLOH);

            double tmpBOH = GetDouble(bm, "Ø Oljeborrhål");
            kv["SumBOH"] = "Ø " + Fmt(tmpBOH).Replace(".", ",");
            kv["SumBOHTol"] = GenTol(tmpBOH);

            string sumOHG = GetString(bm, "Gänga Oljeborrhål");
            kv["SumOHG"] = sumOHG;
            kv["SumG"] = sumOHG;

            double tmpOHDj = GetDouble(bm, "Oljeborrhål Djup");
            kv["SumOHDj"] = Fmt(tmpOHDj).Replace(".", ",");
            kv["SumOHDjTol"] = SmallTol(tmpOHDj);

            double ohGDj = GetDouble(bm, "OljehålsGänga Djup");
            kv["SumOHGDj"] = ohGDj == 0 ? "" : "min " + Fmt(ohGDj).Replace(".", ",");

            double tmpBHM = GetDouble(bm, "Borrhålsmått (BH)");
            kv["SumBHM"] = "(BH) " + Fmt(tmpBHM).Replace(".", ",");

            kv["SumH"] = "Ø " + Fmt(GetDouble(bm, "Ø Borrhål till Oljespår")).Replace(".", ",");
            kv["SumHTol"] = "± 0.1";

            string tmpMaskinValS1 = GetMember(maskinVal, MachineList) > 0 ? maskinVal : "";
            bool isMachine = tmpMaskinValS1.Length > 0;
            bool isEmptyMachine = string.IsNullOrEmpty(maskinVal);
            kv["SumMaskinValS1"] = "Maskin: " + tmpMaskinValS1 + "" + " - Borrning, Fräsning";

            kv["SumF1_1"] = isMachine ? "1/1" : isEmptyMachine ? "1/1" : "";
            kv["SumF1_2"] = isMachine ? "1/1" : isEmptyMachine ? "1/1" : "";
            kv["SumF1_3"] = isMachine ? "1/1" : "";
            kv["SumF1_4"] = isMachine ? "1/1" : "";

            kv["SumD1_1"] = isMachine ? "Gängtolk" : "";
            kv["SumD1_2"] = isMachine ? "Skjutmått" : "";
            kv["SumD1_3"] = isMachine ? "Skjutmått" : "";
            kv["SumD1_4"] = isMachine ? "Skjutmått" : "";

            kv["SumAF1_1"] = "";
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = isMachine ? "Tolerans efter slits enl. 7437495" : "";
            kv["SumAF1_4"] = "";

            kv["SumTextS1"] = "Bryt alla kanter, avlägsna";
            kv["SumExtraTxt"] = tmpBet.IndexOf("7433833", StringComparison.OrdinalIgnoreCase) >= 0
                ? "OBS! Extern instruktion för fräs, radie i slits"
                : "";

            string ritningsnummer = GetString(bm, "Ritningsnummer");
            if (EqualsI(ritningsnummer, "0"))
            {
                kv["SumRitningsnr"] = (serieInt == 38 && !tmpSpecial) ? "238000"
                                    : (serieInt == 39 && !tmpSpecial) ? "226472"
                                    : "Styckritning: " + tmpBet;
            }
            else
            {
                kv["SumRitningsnr"] = "Styckritning: " + ritningsnummer;
            }

            kv["SumRitTol"] = "Toleranser: 1432010, 7437495";
            kv["SumRitYtjämnhet"] = "Yta: 7430184";
            kv["SumKlEgenskaper"] = "PRODUCTION NUTS & SLEEVES & HOUSINGS\\ALLMÄNKLASSADE EGENSKAPER\\Klassade egenskaper kilhylsor";

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

        private static double LookupL(int serie, int index)
        {
            if (index <= 0) return 0;
            if (serie == 39) return index <= LList39.Length ? LList39[index - 1] : 0;
            if (serie == 38) return index <= LList38.Length ? LList38[index - 1] : 0;
            return 0;
        }

        private static double D1TolPos(int typ)
        {
            if (typ < 53) return 0.185;
            if (typ < 65) return 0.21;
            if (typ < 85) return 0.36;
            if (typ < 535) return 0.4;
            if (typ < 675) return 0.44;
            if (typ < 855) return 0.5;
            return 0.5;
        }

        private static double D1TolNeg(int typ)
        {
            if (typ < 53) return 0.29;
            if (typ < 65) return 0.32;
            if (typ < 85) return 0.57;
            if (typ < 535) return 0.63;
            if (typ < 675) return 0.7;
            if (typ < 855) return 0.8;
            return 0.9;
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

        private static string SmallTol(double v)
        {
            return v < 6.01 ? "± 0.1" : "± 0.2";
        }

        private static double RoundTo(double value, double unit)
        {
            if (unit == 0) return value;
            return Math.Round(Math.Round(value / unit, MidpointRounding.AwayFromZero) * unit, 6);
        }

        private static bool Eq(double a, double b)
        {
            return Math.Abs(a - b) < 0.0001;
        }

        private static int GetMember(string val, string[] list)
        {
            for (int i = 0; i < list.Length; i++)
                if (string.Equals(list[i], val, StringComparison.OrdinalIgnoreCase)) return i + 1;
            return 0;
        }

        private static bool EqualsI(string a, string b)
        {
            return string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);
        }

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

        private static string FmtNumber(double v)
        {
            return v.ToString("G", CultureInfo.InvariantCulture);
        }

        private static string Fmt(double v)
        {
            return v.ToString(CommonFunctions.Culture).Replace(",", ".");
        }

        private static string Fmt3(double v)
        {
            return v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
        }

        public static string FormatNumber(string value)
        {
            if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out double number))
            {
                return Math.Round(number, 1)
                           .ToString("0.#", new CultureInfo("nl-NL"));
            }

            return value; // or throw an exception
        }

    }
}


