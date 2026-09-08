using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Avdragshylsa_40_60_LU25 : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();
            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string mv = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;
            List<Bookmark> bm = req != null ? req.Bookmarks : null;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpBet = subject.ToUpperInvariant().Trim().Replace(".", ",");
            bool tmpSlash = tmpBet.Contains("/");
            bool tmpO = tmpBet.Contains("O");
            string[] tokens = tmpBet.Split(new[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : "";
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : "";
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : "";
            string tmpBet4 = tokens.Length > 3 ? tokens[3] : "";
            string tmpBet5 = tokens.Length > 4 ? tokens[4] : "";

            int countB2 = tmpBet2.Length;
            string tmpSerie = countB2 == 2 || countB2 == 3
                ? tmpBet2
                : countB2 > 4 ? Left(tmpBet2, 3) : Left(tmpBet2, 2);
            string tmpTyp = countB2 > 3
                ? Right(tmpBet2, 2)
                : tmpSlash ? tmpBet3 : countB2 < 2 ? "0" : tmpBet3;

            double tmpL = GetDouble(bm, "Längd");
            double tmpb = GetDouble(bm, "Längd till gänga");
            double tmph = GetDouble(bm, "Släppning");
            double tmpd4 = GetDouble(bm, "Släppningsdiameter");
            double tmpA = GetDouble(bm, "Mått inv till borrcentrum");
            double tmpBeta = GetDouble(bm, "Borrvinkel");
            double tmpB2 = GetDouble(bm, "Längd till oljespår");
            double tmpd2 = GetDouble(bm, "YDia");
            double tmpd1 = GetDouble(bm, "IDia");
            double konringsdiameter = GetDouble(bm, "Konringsdiameter");
            double amatt = GetDouble(bm, "Amått");
            string gangtapp = GetString(bm, "Gängtapp");
            double tmpS = GetDouble(bm, "Längd gänghål");
            double antalRepor = GetDouble(bm, "Antal repor");
            double langdTillRepor = GetDouble(bm, "Längd till repor");
            double replangd = GetDouble(bm, "Replängd");
            string kilnr = NormalizeBookmarkValue(
                GetFirstString(bm, "Kil nr.", "Kil nr", "Kilnr"),
                "5",
                "Kilnr", "Kil nr", "Kil nr.");
            string kilinstallning = NormalizeBookmarkValue(
                GetFirstString(bm, "Kilens inställning", "Kil inställning", "Kilinställning"),
                "-3,8",
                "Kilinställning", "Kilens inställning", "Kil inställning");
            string passbit = GetString(bm, "Passbit");
            string ritningsnummer = GetString(bm, "Ritningsnummer").Trim().ToUpperInvariant();

            double tmpTypNum = TryParseDouble(tmpTyp);
            double tmpd = konringsdiameter == 0 ? tmpTypNum / 2.0 * 10.0 : konringsdiameter;

            bool ritAHA = ContainsI(ritningsnummer, "AHA");
            bool ritAOH = ContainsI(ritningsnummer, "AOH");
            bool ritLW943 = ContainsI(ritningsnummer, "7433943");
            bool ritASW0002 = ContainsI(ritningsnummer, "ASW-0002");
            bool rit3 = ContainsI(ritningsnummer, "7437359");
            bool rit22 = ContainsI(ritningsnummer, "7437361");
            bool rit23 = ContainsI(ritningsnummer, "7437360");
            bool rit30 = ContainsI(ritningsnummer, "7437362");
            bool rit31 = ContainsI(ritningsnummer, "7437364");
            bool rit32 = ContainsI(ritningsnummer, "7437366") || ContainsI(ritningsnummer, "7437367");

            double sumML = RoundTo(tmpb - tmph - 16.0, 5.0);
            double tmpStmm = tmpd2 < 25 ? 1.0 : tmpd2 < 55 ? 1.5 : tmpd2 < 155 ? 2.0
                : tmpd2 < 205 ? 3.0 : tmpd2 < 301 ? 4.0 : tmpd2 < 501 ? 5.0
                : tmpd2 < 701 ? 6.0 : tmpd2 < 901 ? 7.0 : 8.0;
            string sumPValue = ritAHA || ritLW943 ? "6" : FmtComma(tmpStmm);
            double tmpVT = tmpd > 181 ? 0.15 : tmpd > 151 ? 0.18 : tmpd > 121 ? 0.30
                : tmpd > 81 ? 0.45 : tmpd > 51 ? 0.50 : 0.60;
            double tmpKona = tmpSerie == "240" || tmpSerie == "241" ? 30 : 12;

            kv["SumML"] = FmtComma(sumML);
            kv["SumP"] = "(P) " + sumPValue;
            kv["SumKona"] = "Kona 1:" + FmtComma(tmpKona);
            // Use decimal arithmetic and explicit midpoint rounding to avoid binary floating-point
            // errors such as 0.0195 being formatted as 0.019 instead of 0.020.
            decimal konavvikelse = ((decimal)sumML * (decimal)tmpVT) / 1000m;
            kv["SumKonavv"] = "max " + FmtCommaFixed(konavvikelse, 3) + " [2F]";

            double tmpT = EqualsI(gangtapp, "1/4") ? 13.5 : 10;
            kv["SumT"] = tmpO ? "(T) " + tmpT : "";
            kv["SumC2"] = "(C2) 4";

            bool gThread = gangtapp.Contains("/");
            string sumR = tmpO ? (gThread ? "G" : "M") + gangtapp : "";
            kv["SumR"] = sumR;
            kv["SumR2"] = sumR;
            kv["SumS"] = tmpO ? "(S) " + FmtComma(tmpS) : "";

            string sumRullar = ritAHA || ritLW943
                ? sumPValue + " UN"
                : tmpStmm <= 3 ? "M" + FmtComma(tmpStmm) : "Tr" + FmtComma(tmpStmm);
            string sumGanga = ritAHA ? "11,004x6UN"
                : ritLW943 ? "12.938x6UN"
                : (tmpStmm <= 3 ? "M" : "Tr") + FmtComma(tmpd2) + "x" + FmtComma(tmpStmm);
            kv["SumRullar"] = sumRullar;
            kv["SumGänga"] = sumGanga;

            kv["SumZ"] = "(Z) Ø" + FmtComma(tmpd1 - 4);

            bool sdia1 = ritningsnummer.Contains("21");
            bool sdia2 = ritningsnummer.Contains("22");
            bool sdia3 = ritningsnummer.Contains("23");
            bool hasSDia = sdia1 || sdia2 || sdia3;
            int ant = hasSDia ? 5 : 7;
            string sdiaTolKey = ritAOH || ritLW943 || ritASW0002 ? "0" : Left(ritningsnummer, ant);
            string[] tol300 = { "7437362", "7437378", "7437364", "7437366", "7437380", "7437376" };
            string[] tol200 = { "7437360", "7437361", "7437987" };
            string[] tol250 = { "7437372", "7437370", "7437697" };
            kv["Sumd4"] = "(d4) " + FmtComma(tmpd4);
            kv["Sumd4Tol"] = ritASW0002 || ritLW943 ? "+ 0" : hasSDia ? "" : In(sdiaTolKey, tol300) || In(sdiaTolKey, tol200) || In(sdiaTolKey, tol250) ? " 0" : "";
            kv["Sumd4TolN"] = ritLW943 ? "- 1" : hasSDia ? "" : In(sdiaTolKey, tol300) ? "- 0.300" : In(sdiaTolKey, tol200) || ritASW0002 ? "- 0.200" : In(sdiaTolKey, tol250) ? "- 0.250" : "";

            kv["Sumd2"] = "(d2) " + FmtComma(tmpd2);
            kv["Sumd2a"] = kv["Sumd2"];
            string d2Tol = tmpStmm == 5 ? "- 0 " : tmpStmm == 4 ? "- 0" : tmpStmm == 3 ? "- 0.048"
                : tmpStmm == 2 ? "- 0.038" : tmpStmm == 1.5 ? "- 0.032" : "- 0.026";
            string d2TolN = ritLW943 ? "- 0,508" : tmpStmm == 5 ? "- 0.335" : tmpStmm == 4 ? "- 0.300"
                : tmpStmm == 3 ? "- 0.423" : tmpStmm == 2 ? "- 0.318" : tmpStmm == 1.5 ? "- 0.268" : "- 0.206";
            kv["Sumd2Tol"] = d2Tol;
            kv["Sumd2TolN"] = d2TolN;
            kv["Sumd2aTol"] = d2Tol;
            kv["Sumd2aTolN"] = d2TolN;

            double tmpdm = ritAHA ? 276.65 : ritLW943 ? 325.882
                : tmpStmm == 1 ? tmpd2 - 0.650 : tmpStmm == 1.5 ? tmpd2 - 0.974
                : tmpStmm == 2 ? tmpd2 - 1.299 : tmpStmm == 3 ? tmpd2 - 1.949
                : tmpStmm == 4 ? tmpd2 - 2.000 : tmpStmm == 5 ? tmpd2 - 2.500 : tmpd2 - 3.000;
            kv["Sumdm"] = "(dm) " + FmtComma(tmpdm);
            string dmTol = ritAHA || ritLW943 ? "+ 0" : tmpStmm == 5 ? "- 0.212" : tmpStmm == 4 ? "- 0.190"
                : tmpStmm == 3 ? "- 0.048" : tmpStmm == 2 ? "- 0.038" : tmpStmm == 1.5 ? "- 0.032" : "- 0.026";
            string dmTolN = ritAHA ? "- 0.330" : ritLW943 ? "- 0.152" : tmpStmm == 5 ? "- 0.710"
                : tmpStmm == 4 ? "- 0.630" : tmpStmm == 3 ? (tmpd2 > 180 ? "- 0.363" : "- 0.328")
                : tmpStmm == 2 ? (tmpd2 > 90 ? "- 0.274" : "- 0.262")
                : tmpStmm == 1.5 ? (tmpd2 > 45 ? "- 0.232" : "- 0.222") : "- 0.176";
            kv["SumdmTol"] = dmTol + " [3F]";
            kv["SumdmTolN"] = dmTolN + " [3F]";

            kv["SumBorrHAnt"] = tmpO && tmpd1 < 195 ? "OBS! OBS! OBS!\nBara 1 borrhål med genomgående hål till inv. & utv. oljespår" : "";
            // Match the reference drawing format: 0,5 is displayed as ,5.
            kv["Sumß"] = tmpO ? "(ß)" + FmtCommaWithoutLeadingZero(tmpBeta) + "º" : "";
            kv["SumV"] = tmpO ? (tmpd1 < 195 ? "Rakt genomgående" : "30º") : "";
            kv["SumGH"] = "(GH) 3";

            double tmpC = tmpB2 + (tmpd < 421 ? 3 : 4);
            double tmpG = tmpd > 439 ? 6 : tmpd > 319 ? 5 : tmpd > 219 ? 4 : 3;
            kv["SumC"] = tmpO ? "(C) " + FmtComma(tmpC) : "";
            kv["SumG"] = tmpO ? "(G) " + FmtComma(tmpG) : "";
            kv["SumA"] = tmpO ? "(A) " + FmtComma(tmpA) : "";
            kv["SumDB"] = tmpO ? "(DB) " + FmtComma(tmpd1 + tmpA * 2) : "--< Inga Oljeborrhål >--";

            kv["SumbSSK"] = "(b) " + FmtComma(tmpb + 4);
            kv["Sumb"] = "(b) " + FmtComma(tmpb);
            kv["SumbTol"] = SymTol(tmpb) + " [3F]";

            double tmpLSSK = konringsdiameter == 220 ? 229 : konringsdiameter == 240 ? 242 : konringsdiameter == 260 ? 254 : tmpL + 4;
            kv["SumLSSK"] = "(L) " + FmtComma(tmpLSSK);
            kv["SumL"] = "(L) " + FmtComma(tmpL);
            kv["SumLTol"] = " 0 [3F]";
            kv["SumLTolN"] = NegativeLengthTol(tmpL) + " [3F]";
            kv["Sumh"] = "(h) " + FmtComma(tmph);

            string sumg1 = tmpStmm == 8 ? "5.3x45º" : tmpStmm == 7 ? "4.4x45º" : tmpStmm == 6 ? "3.8x45º"
                : tmpStmm == 5 ? "3.2x45º" : tmpStmm == 4 ? "2.7x45º" : tmpStmm == 3 ? "2.4x45º"
                : tmpStmm == 2 ? "1.7x45º" : "1.3x45º";
            string sumg2 = sumg1;
            if (rit3 && tmpd2 == 80) sumg2 = "1.5x45º";
            else if (rit22 && tmpd2 < 210) sumg2 = "2.5x45º";
            else if (rit23) sumg2 = tmpd2 < 69 ? sumg1 : tmpd2 < 76 ? "1.75x45º" : (tmpd2 == 80 || tmpd2 == 140 || tmpd2 == 150) ? "1.5x45º" : tmpd2 < 155 ? "1.7x45º" : tmpd2 < 210 ? "2.25x45º" : sumg1;
            else if (rit30) sumg2 = tmpd2 < 195 ? sumg1 : tmpd2 == 200 ? "2.5x45º" : tmpd2 < 250 ? "2.75x45º" : tmpd2 < 310 ? "2.7x45º" : sumg1;
            else if (rit31) sumg2 = tmpd2 < 160 ? "1.7x45º" : tmpd2 < 210 ? "2.25x45º" : tmpd2 < 310 ? "2.7x45º" : tmpd2 == 320 ? sumg1 : tmpd2 < 490 ? "3.25x45º" : sumg1;
            else if (rit32) sumg2 = (tmpd2 == 140 || tmpd2 == 150) ? "1.5x45º" : tmpd2 < 135 ? "1.7x45º" : tmpd2 < 210 ? "2.25x45º" : tmpd2 == 220 ? "2.7x45º" : tmpd2 < 490 ? "3.25x45º" : tmpd2 < 580 ? "4x45º" : tmpd2 == 600 ? "3.8x45º" : tmpd2 < 700 ? "4x45º" : tmpd2 < 790 ? "4.5x45º" : tmpd2 < 890 ? "4.4x45º" : tmpd2 == 950 ? "5.5x45º" : tmpd2 == 1000 ? "5x45º" : "5.3x45º";
            kv["Sumg1"] = sumg1;
            kv["Sumg2"] = sumg2;

            double tmpX = tmpd > 290 ? 62 : tmpd > 270 ? 58 : tmpd > 250 ? 54 : tmpd > 230 ? 50 : 46;
            kv["SumX"] = tmpO ? "(X) " + FmtComma(tmpX) : "";
            kv["SumXTol"] = tmpO ? " 0" : "";
            kv["SumXTolN"] = tmpO ? (tmpd > 270 ? "- 15" : tmpd > 230 ? "- 12" : "- 11") : "";
            double tmpEE = tmpTypNum < 64 ? 5 : tmpTypNum < 84 ? 6 : 7;
            kv["SumEE"] = tmpO ? "(EE) " + FmtComma(tmpEE) : "";
            kv["SumF"] = tmpO ? "(F) " + (tmpEE == 5 ? "1" : tmpEE == 6 ? "1.2" : "1.5") : "";

            kv["Sumn"] = tmpO ? "(n) " + FmtComma(antalRepor) + "st." : "";
            kv["SumJ"] = tmpO ? "(J) " + FmtComma(langdTillRepor) : "";
            kv["SumK"] = tmpO ? "(K) " + FmtComma(replangd) : "";
            kv["SumM"] = tmpO ? "(M) 4" : "";
            kv["SumMTol"] = "+ 0.5";
            kv["SumMTolN"] = "+ 0.0";

            string exTol = tmpKona == 12
                ? tmpd > 316 ? "+ 0.089" : tmpd > 251 ? "+ 0.081" : tmpd > 181 ? "+ 0.072" : tmpd > 121 ? "+ 0.063" : tmpd > 81 ? "+ 0.054" : tmpd > 51 ? "+ 0.046" : tmpd > 31 ? "+ 0.039" : "+ 0.033"
                : tmpd > 316 ? "+ 0.057" : tmpd > 251 ? "+ 0.052" : tmpd > 181 ? "+ 0.046" : tmpd > 121 ? "+ 0.040" : "+ 0.035";
            kv["SumExTol"] = exTol + " [3F]";
            kv["SumExTolN"] = " - 0 [2F]";
            kv["SumR15"] = "R 1,5";
            kv["SumR15a"] = "R 1,5";
            kv["SumROS"] = tmpEE == 5 ? "R4" : tmpEE == 6 ? "R4.5" : "R5";

            string d1Tol = tmpd1 > 316 ? "± 0.070" : tmpd1 > 251 ? "± 0.065" : tmpd1 > 181 ? "± 0.057"
                : tmpd1 > 121 ? "± 0.050" : tmpd1 > 81 ? "± 0.043" : tmpd1 > 51 ? "± 0.037"
                : tmpd1 > 31 ? "± 0.031" : "± 0.026";
            kv["Sumd1Tol"] = d1Tol + " [3F]";
            kv["Sumd1"] = "(d1) " + FmtComma(tmpd1);
            kv["Sumd1Toles"] = (tmpd1 > 305 ? "+ 0.360" : tmpd1 > 245 ? "+ 0.210" : tmpd1 > 185 ? "+ 0.185" : "+ 0.160") + " [3F]";
            kv["Sumd1TolNes"] = (tmpd1 > 305 ? " - 0.570" : tmpd1 > 245 ? " - 0.320" : tmpd1 > 185 ? " - 0.290" : " - 0.250") + " [3F]";

            string sumKil = (tmpd < 201 ? "1509952-" : "1579442-") + kilnr;
            string sumKonapp = tmpd < 201 ? "1509950" : "1579440-41";
            string sumBygel = tmpb - tmph > 85 ? "7419471" : "7419469";
            kv["SumKil"] = sumKil;

            // Support both the calculated bookmark names and the literal template
            // placeholders found in the current Word template.
            kv["SumKilNr"] = kilnr;
            kv["Kilnr"] = kilnr;
            kv["Kil nr"] = kilnr;
            kv["Kil nr."] = kilnr;

            kv["SumKilinställning"] = kilinstallning;
            kv["Kilinställning"] = kilinstallning;
            kv["Kil inställning"] = kilinstallning;
            kv["Kilens inställning"] = kilinstallning;

            kv["SumKonapp"] = sumKonapp;
            kv["SumBygel"] = sumBygel;
            kv["SumB2"] = tmpO ? "(B2) " + FmtComma(tmpB2) : "Inga Oljespår";
            kv["SumB2Tol"] = tmpO ? GenTolB2(tmpB2) : "";

            kv["SumRakA"] = FmtComma(tmpd < 101 ? 8 : tmpd < 281 ? 10 : tmpd < 481 ? 12 : tmpd < 601 ? 14 : tmpd < 901 ? 16 : 20);
            kv["SumRakB"] = FmtComma(tmpd < 101 ? 12 : tmpd < 281 ? 15 : tmpd < 481 ? 18 : tmpd < 601 ? 21 : tmpd < 901 ? 24 : 30);

            // Reference drawing prints the tolerance in thousandths: 0.057 -> 57.
            double sumRdValue = TryParseDouble(d1Tol.Replace("± ", "")) * 1000.0;
            kv["SumRd"] = Math.Round(sumRdValue, 0, MidpointRounding.AwayFromZero)
                .ToString("0", CultureInfo.InvariantCulture);
            kv["SumKs"] = FmtComma(tmpd > 251 ? 70 : tmpd > 121 ? 60 : tmpd > 51 ? 50 : 40);
            string tmpKr = (tmpd > 316 ? "0.030" : tmpd > 251 ? "0.025" : tmpd > 181 ? "0.020" : tmpd > 121 ? "0.015" : tmpd > 51 ? "0.010" : "0.008") + " [2F]";
            kv["TmpKr"] = tmpKr;

            double tmpL2 = tmpb - tmph > 110 ? 100 : tmpb - tmph > 85 ? 75 : 50;
            double rawE1 = ((8 + amatt) / tmpKona + tmpd - tmpd1) / 2.0;
            double rawE2 = ((8 + tmpL2 + amatt) / tmpKona + tmpd - tmpd1) / 2.0;
            double fractionE1 = rawE1 - Math.Truncate(100 * rawE1) / 100.0;
            double fractionE2 = rawE2 - Math.Truncate(100 * rawE2) / 100.0;
            double sumE1 = Math.Abs(fractionE2 - fractionE1) < 0.005
                ? (fractionE1 + fractionE2 < 0.01 ? Math.Truncate(100 * rawE1) / 100.0 : Math.Truncate(1 + 100 * rawE1) / 100.0)
                : rawE1;
            double sumE2 = Math.Abs(fractionE2 - fractionE1) < 0.005
                ? (fractionE1 + fractionE2 < 0.01 ? Math.Truncate(100 * rawE2) / 100.0 : Math.Truncate(1 + 100 * rawE2) / 100.0)
                : rawE2;
            kv["SumE1"] = FmtComma(Math.Round(sumE1, 3));
            kv["SumE2"] = FmtComma(Math.Round(sumE2, 3));
            kv["SumL1_1"] = "8";
            kv["SumL2"] = FmtComma(tmpL2);
            kv["SumPassbit"] = TryParseDouble(passbit) == 0 ? "" : " + passbit " + passbit + "mm";
            kv["SumRa"] = "2.5 [2F]";
            kv["SumRa1"] = "2.5 [2F]";

            string tmpRitNr = tmpSerie == "22" ? "7437361:3" : tmpSerie == "23" ? "7437360:3"
                : tmpSerie == "30" ? (tmpTypNum == 40 && tmpO ? "7435633:A" : "7437362:2")
                : tmpSerie == "31" ? "7437364:3" : tmpSerie == "32" ? (EqualsI(tmpBet3, "G") ? "7437366:3" : "231638")
                : tmpSerie == "240" ? "7437370:4" : tmpSerie == "241" ? "7437372:2" : tmpBet;
            if (tmpO)
                tmpRitNr += tmpSerie == "22" || tmpSerie == "23" ? ", 7440340:1"
                    : tmpSerie == "30" && tmpTypNum > 41 ? ", 7437376:1" : tmpSerie == "31" ? ", 7437378:1"
                    : tmpSerie == "32" ? ", 7437380:1" : tmpSerie == "240" ? ", 7437397:1"
                    : tmpSerie == "241" ? ", 7437399:1" : "";
            string sumRitNr = ritningsnummer == "0" ? tmpRitNr : ritningsnummer;
            kv["SumRitNr"] = sumRitNr;
            kv["SumRitNr2"] = sumRitNr;
            kv["SumGängRit"] = "237359:3, 7430181:2";
            kv["SumTolRit"] = "1432011:6, 7437495:4";
            kv["SumTolRit2"] = kv["SumTolRit"];
            kv["SumGGD"] = "7433015";
            kv["SumKlEgenskaper"] = "PRODUCTION NUTS & SLEEVES & HOUSINGSALLMÄNKLASSADE EGENSKAPERKlassade egenskaper Avdragshylsor";
            kv["SumKlEgenskaperS1"] = kv["SumKlEgenskaper"];
            kv["SumKlEgenskaperS2"] = kv["SumKlEgenskaper"];

            bool machineOk = EqualsI(mv, "LU25") || EqualsI(mv, "LU-4000M");
            string maskinSuffix = machineOk && !tmpO ? " - UTAN BORRHÅL & SPÅR" : "";
            kv["SumMaskinVal"] = "Maskin: " + mv + maskinSuffix + " - OP1";
            kv["SumMaskinValS2"] = "Maskin: " + mv + maskinSuffix + " - OP2";

            string[] f1 = { "1/10", "1/1", "Inst.", "1/10", tmpO ? "1/2" : "", "Inst.", "Inst.", "Inst.", "", "" };
            string[] f2 = { "1/2", "1/1", "1/10", tmpO ? "1/10" : "", "Inst.", "1/2", "1/10", "1/10", "1/10", "Inst." };
            string[] d1 = { "Skjutmått", "Multimar", "Gängmall", "Skjutmått", tmpO ? "Gängtolk" : "", "Skjutmått", "Skjutmått", "Skjutmått", "", "" };
            string[] d2 = { "UD-Apparat, inst-ring el. klove", "Höjdmätningsapp.", "Skjutmått", tmpO ? "Skjutmått" : "", "Egglinjal", "Mätbygel inst. enl nedan eller Konmätningsapparat", "App.nr:" + sumKonapp + " kil " + sumKil, "Konmätningsapparat", "Ra-mätare", "Mätmaskin" };
            string[] af1 = { "", "Rullar: " + sumRullar + ", inställd i längdmätbänk", "", "", "", "", "", "", "", "" };
            string[] af2 = { "TOL efter slits:", "Kontrollera runt hela hylsan", "", "", "Vid misstänkt formfel kontrollera hylsan i MarSurf Contour XC20", "GodstjockleksTOL:", "Tolerans: " + kv["SumKonavv"], "Max variation: " + tmpKr, "", "Vid misstänkt kast lämnas hylsan till mätrummet" };
            SetIndexed(kv, "SumF1_", f1, machineOk);
            SetIndexed(kv, "SumF2_", f2, machineOk);
            SetIndexed(kv, "SumD1_", d1, machineOk);
            SetIndexed(kv, "SumD2_", d2, machineOk);
            SetIndexed(kv, "SumAF1_", af1, machineOk);
            SetIndexed(kv, "SumAF2_", af2, machineOk);

            kv["SumBorrhålsgänga"] = tmpO ? "Borrhålsgänga" : "";
            kv["SumLängdtilloljespår"] = tmpO ? "Längd till oljespår" : "";
            kv["SumB2_4"] = tmpO ? "B2" : "";
            kv["SumTextS1"] = "Övriga mått kontrolleras vid inställning.\nOkulär kontroll av Grader, frifläckar, slagmärken, repor, valkar etc. Märkning ska vara rätt och tydlig. Oljespåret: Inget skorr och max 6,3Ra.\nVid blåsning av kanalerna, kontrollera så inte bubblor uppstår";
            kv["SumTextS2"] = kv["SumTextS1"];
            kv["VaLTyp"] = tmpd < 200 || tmpd > 300 ? "Denna mall är anpassad för Avdragshylsor\nstorlek 40-60 med 2 operationer" : "";

            return kv;
        }

        private static void SetIndexed(Dictionary<string, string> kv, string prefix, string[] values, bool enabled)
        {
            for (int i = 0; i < 10; i++)
            {
                string suffix = i == 9 ? "0" : (i + 1).ToString(CultureInfo.InvariantCulture);
                kv[prefix + suffix] = enabled ? values[i] : "";
            }
        }

        private static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published)) return "";
            DateTime date;
            if (!DateTime.TryParse(published, CultureInfo.InvariantCulture, DateTimeStyles.None, out date) && !DateTime.TryParse(published, out date)) return "";
            DateTime until = date.AddDays(14);
            return DateTime.Today <= until.Date
                ? "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)\n\nPopupruta aktiv till " + until.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                : "";
        }

        private static string SymTol(double value)
        {
            return value > 400 ? "± 1.250" : value > 315 ? "± 1.150" : value > 250 ? "± 1.050"
                : value > 180 ? "± 0.925" : value > 120 ? "± 0.800" : value > 80 ? "± 0.700"
                : value > 50 ? "± 0.600" : value > 30 ? "± 0.500" : value > 18 ? "± 0.420"
                : value > 10 ? "± 0.350" : "± 0.290";
        }

        private static string NegativeLengthTol(double value)
        {
            return value > 400 ? "- 0.970" : value > 315 ? "- 0.890" : value > 250 ? "- 0.810"
                : value > 180 ? "- 0.720" : value > 120 ? "- 0.630" : value > 80 ? "- 0.540"
                : value > 50 ? "- 0.460" : value > 30 ? "- 0.390" : value > 18 ? "- 0.330"
                : value > 10 ? "- 0.270" : "- 0.220";
        }

        private static string GenTolB2(double value)
        {
            return value < 30.01 ? "± 0.2" : value < 120.01 ? "± 0.3" : value < 400.01 ? "± 0.5" : "± 0.8";
        }

        private static double RoundTo(double value, double increment)
        {
            return Math.Round(value / increment, MidpointRounding.AwayFromZero) * increment;
        }

        private static string Left(string value, int length)
        {
            if (string.IsNullOrEmpty(value)) return "";
            return value.Length <= length ? value : value.Substring(0, length);
        }

        private static string Right(string value, int length)
        {
            if (string.IsNullOrEmpty(value)) return "";
            return value.Length <= length ? value : value.Substring(value.Length - length);
        }

        private static bool In(string value, string[] list)
        {
            for (int i = 0; i < list.Length; i++)
                if (EqualsI(value, list[i])) return true;
            return false;
        }

        private static bool ContainsI(string value, string part)
        {
            return (value ?? "").IndexOf(part ?? "", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool EqualsI(string a, string b)
        {
            return string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);
        }

        private static double TryParseDouble(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return 0;
            double result;
            return double.TryParse(value.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out result) ? result : 0;
        }

        private static string GetFirstString(List<Bookmark> bookmarks, params string[] keys)
        {
            if (keys == null) return "";
            for (int i = 0; i < keys.Length; i++)
            {
                string value = GetString(bookmarks, keys[i]);
                if (!string.IsNullOrWhiteSpace(value)) return value.Trim();
            }
            return "";
        }

        private static string NormalizeBookmarkValue(
            string value,
            string fallback,
            params string[] placeholderValues)
        {
            string normalized = (value ?? "").Trim();
            if (string.IsNullOrWhiteSpace(normalized)) return fallback ?? "";

            if (placeholderValues != null)
            {
                for (int i = 0; i < placeholderValues.Length; i++)
                {
                    if (EqualsI(normalized, placeholderValues[i])) return fallback ?? "";
                }
            }

            return normalized;
        }

        private static double GetDouble(List<Bookmark> bookmarks, string key)
        {
            return TryParseDouble(GetString(bookmarks, key));
        }

        private static string GetString(List<Bookmark> bookmarks, string key)
        {
            if (bookmarks == null || string.IsNullOrWhiteSpace(key)) return "";
            for (int i = 0; i < bookmarks.Count; i++)
            {
                Bookmark bookmark = bookmarks[i];
                if (bookmark != null && EqualsI(bookmark.BookmarkName, key))
                    return bookmark.BookmarkValue ?? "";
            }
            return "";
        }

        private static string FmtDotFixed(double value, int decimals)
        {
            if (decimals < 0) decimals = 0;
            return value.ToString("F" + decimals.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
        }

        private static string FmtCommaFixed(double value, int decimals)
        {
            return FmtCommaFixed((decimal)value, decimals);
        }

        private static string FmtCommaFixed(decimal value, int decimals)
        {
            if (decimals < 0) decimals = 0;
            decimal rounded = Math.Round(value, decimals, MidpointRounding.AwayFromZero);
            return rounded.ToString(
                "F" + decimals.ToString(CultureInfo.InvariantCulture),
                CultureInfo.InvariantCulture).Replace(".", ",");
        }

        private static string FmtCommaWithoutLeadingZero(double value)
        {
            string formatted = FmtComma(value);

            if (formatted.StartsWith("0,", StringComparison.Ordinal))
                return formatted.Substring(1);
            if (formatted.StartsWith("-0,", StringComparison.Ordinal))
                return "-" + formatted.Substring(2);

            return formatted;
        }

        private static string FmtComma(double value)
        {
            return value.ToString("0.################", CultureInfo.InvariantCulture).Replace(".", ",");
        }
    }
}
