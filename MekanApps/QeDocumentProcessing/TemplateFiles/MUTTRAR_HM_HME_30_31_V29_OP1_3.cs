using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class MUTTRAR_HM_HME_30_31_V29_OP1_3 : ITemplateCalculations
    {
        private const int TmpDagar = 14;

        private static readonly string[] SvarvMachineList = { "LB45", "EMAG", "VTR-160", "MacTurn 550" };
        private static readonly string[] MatMachineList = { "LB45", "MaxMuller/K&T", "VTR-160", "MacTurn 550" };

        private static readonly string[] TypListHM30 = { "64", "96", "500", "560", "670", "900" };
        private static readonly string[] TypListHM31 = { "0" };
        private static readonly string[] TypListHME30 = { "84", "96", "600", "630", "750", "800", "900" };
        private static readonly string[] TypListHME31 = { "96", "600" };

        private static readonly double[] D1ListHM30 = { 356, 530, 550, 610, 740, 975 };
        private static readonly double[] D1ListHM31 = { 0 };
        private static readonly double[] D1ListHME30 = { 462, 530, 657, 690, 820, 870, 975 };
        private static readonly double[] D1ListHME31 = { 560, 690 };

        private static readonly double[] TList30 = { 10, 12, 18, 18, 20, 20, 25 };
        private static readonly double[] TList31 = { 20, 15 };

        private static readonly double[] BListHM30 = { 39, 56, 64, 71, 76, 96 };
        private static readonly double[] BListHM31 = { 0 };
        private static readonly double[] BListHME30 = { 48, 56, 71, 71, 86, 86, 96 };
        private static readonly double[] BListHME31 = { 71, 81 };

        private static readonly double[] B2List30 = { 0, 31, 40, 40, 46, 46, 58 };
        private static readonly double[] B2List31 = { 49, 50 };

        private static readonly double[] D4ListHM30 = { 352, 522, 542, 607, 727, 962 };
        private static readonly double[] D4ListHM31 = { 0 };
        private static readonly double[] D4ListHME30 = { 457, 522, 652, 682, 807, 862, 962 };
        private static readonly double[] D4ListHME31 = { 535, 677 };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = subject.ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");

            bool tmpV29 = tmpBet.IndexOf("V29", StringComparison.OrdinalIgnoreCase) >= 0;

            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : "";
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : "";
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : "";

            bool isHM = string.Equals(tmpBet1, "HM", StringComparison.Ordinal);
            bool isHME = string.Equals(tmpBet1, "HME", StringComparison.Ordinal);

            int tmpCountB2 = tmpBet2.Length;
            string tmpSerie = tmpBet2.Length >= 2 ? tmpBet2.Substring(0, 2) : tmpBet2;
            int serieInt = TryParseInt(tmpSerie);

            string tmpTyp = tmpCountB2 > 3
                ? tmpBet2.Substring(2, Math.Min(2, tmpBet2.Length - 2))
                : tmpBet3;
            int typInt = TryParseInt(tmpTyp);

            int typLista = isHM
                ? (serieInt == 30 ? GetMember(tmpTyp, TypListHM30) : GetMember(tmpTyp, TypListHM31))
                : (serieInt == 30 ? GetMember(tmpTyp, TypListHME30) : GetMember(tmpTyp, TypListHME31));

            string tmpRit = isHM
                ? (serieInt == 30 ? "223015, 7439860" : "223016, 7439860")
                : "7438482, 7433771";

            kv["SumRitningsnrS1"] = "Produkt: " + tmpRit;
            kv["SumRitningsnrS2"] = "Produkt: " + tmpRit;
            kv["SumTolRitS1"] = "Toleranser: 1432008";
            kv["SumTolRitS2"] = "Toleranser: 1432008";
            kv["SumGTolRitS1"] = "Gänga: 7430181";
            kv["SumGTolRitS2"] = "Gänga: 7430181";
            kv["SumYtRitS1"] = "Yta: 7430184";
            kv["SumYtRitS2"] = "Yta: 7430184";

            double tmpkd = tmpCountB2 > 3 ? (typInt / 2.0) * 10.0 : TryParseDouble(tmpBet3);

            int tmpStmm = typInt < 61 ? 4
                        : typInt < 501 ? 5
                        : typInt < 671 ? 6
                        : typInt < 901 ? 7
                        : 8;

            kv["SumP"] = "(P) " + tmpStmm.ToString(CultureInfo.InvariantCulture);
            kv["SumGänga"] = "Tr " + Num(tmpkd) + "x" + tmpStmm.ToString(CultureInfo.InvariantCulture);

            double tmpkda = (tmpStmm == 4 || tmpStmm == 5) ? tmpkd + 0.5 : tmpkd + 1;
            kv["Sumkd"] = "(kd) " + Num(tmpkda);
            kv["SumkdTol"] = tmpkd < 6 ? "± 0.1"
                           : tmpkd < 30 ? "± 0.2"
                           : tmpkd < 120 ? "± 0.3"
                           : tmpkd < 400 ? "± 0.5"
                           : tmpkd < 1000 ? "± 0.8"
                           : tmpkd < 2000 ? "± 1.2"
                           : "± 2.0";

            double tmpdm = tmpStmm == 4 ? tmpkd - 2
                         : tmpStmm == 5 ? tmpkd - 2.5
                         : tmpStmm == 6 ? tmpkd - 3
                         : tmpStmm == 7 ? tmpkd - 3.5
                         : tmpkd - 4;
            kv["Sumdm"] = "(dm) " + Num(tmpdm);
            kv["SumdmTol"] = tmpkd < 301 ? "+ 0.475"
                           : tmpkd < 501 ? "+ 0.530"
                           : tmpkd < 701 ? "+ 0.600"
                           : tmpkd < 901 ? "+ 0.630"
                           : "+ 0.710";
            kv["SumdmTolN"] = " 0";

            double tmpid = tmpStmm == 4 ? tmpkd - 4
                         : tmpStmm == 5 ? tmpkd - 5
                         : tmpStmm == 6 ? tmpkd - 6
                         : tmpStmm == 7 ? tmpkd - 7
                         : tmpkd - 8;
            kv["Sumid"] = "(id) " + Num(tmpid);
            kv["SumidTol"] = tmpkd < 301 ? "+ 0.375"
                           : tmpkd < 501 ? "+ 0.450"
                           : tmpkd < 701 ? "+ 0.500"
                           : tmpkd < 901 ? "+ 0.560"
                           : "+ 0.630";
            kv["SumidTolN"] = " 0";

            double tmpD;
            if (serieInt == 30)
            {
                if (tmpkd < 221) tmpD = tmpkd + 40;
                else if (tmpkd < 281) tmpD = tmpkd + 50;
                else if (tmpkd < 361) tmpD = tmpkd + 60;
                else if (tmpkd < 421) tmpD = tmpkd + 70;
                else if (tmpkd < 501) tmpD = tmpkd + 80;
                else if (Eq(tmpkd, 560)) tmpD = tmpkd + 90;
                else if (tmpkd < 631) tmpD = tmpkd + 100;
                else if (tmpkd < 671) tmpD = tmpkd + 110;
                else if (tmpkd < 801) tmpD = tmpkd + 120;
                else if (tmpkd < 951) tmpD = tmpkd + 130;
                else tmpD = tmpkd + 140;
            }
            else
            {
                if (tmpkd < 241) tmpD = tmpkd + 60;
                else if (tmpkd < 281) tmpD = tmpkd + 70;
                else if (tmpkd < 321) tmpD = tmpkd + 80;
                else if (tmpkd < 361) tmpD = tmpkd + 100;
                else if (tmpkd < 381) tmpD = tmpkd + 110;
                else if (tmpkd < 461) tmpD = tmpkd + 120;
                else if (tmpkd < 481) tmpD = tmpkd + 140;
                else if (tmpkd < 501) tmpD = tmpkd + 130;
                else if (tmpkd < 531) tmpD = tmpkd + 140;
                else if (tmpkd < 601) tmpD = tmpkd + 150;
                else if (tmpkd < 631) tmpD = tmpkd + 170;
                else if (tmpkd < 671) tmpD = tmpkd + 180;
                else if (tmpkd < 711) tmpD = tmpkd + 190;
                else if (tmpkd < 801) tmpD = tmpkd + 200;
                else if (tmpkd < 851) tmpD = tmpkd + 210;
                else if (tmpkd < 951) tmpD = tmpkd + 220;
                else tmpD = tmpkd + 240;
            }

            string tmpDTol = " 0";
            string tmpDTolN = H13Neg(tmpD);
            kv["SumD"] = isHM ? "(D) " + Num(tmpD) : "";
            kv["SumDe"] = isHME ? "(D) " + Num(tmpD) : "";
            kv["SumDTol"] = isHM ? tmpDTol : "";
            kv["SumDTolN"] = isHM ? tmpDTolN : "";
            kv["SumDeTol"] = isHME ? tmpDTol : "";
            kv["SumDeTolN"] = isHME ? tmpDTolN : "";

            double[] d1Lista = isHM
                ? (serieInt == 30 ? D1ListHM30 : D1ListHM31)
                : (serieInt == 30 ? D1ListHME30 : D1ListHME31);
            double tmpD1 = ListValue(d1Lista, typLista);

            string tmpD1Tol = " 0";
            string tmpD1TolN = tmpD1 < 19 ? "- 0.270"
                             : tmpD1 < 31 ? "- 0.330"
                             : tmpD1 < 51 ? "- 0.390"
                             : tmpD1 < 81 ? "- 0.460"
                             : tmpD1 < 121 ? "- 0.540"
                             : tmpD1 < 181 ? "- 0.630"
                             : tmpD1 < 251 ? "- 0.720"
                             : tmpD1 < 316 ? "- 0.810"
                             : tmpD1 < 401 ? "- 0.890"
                             : tmpD1 < 501 ? "- 0.970"
                             : tmpD1 < 631 ? "- 1.100"
                             : tmpD1 < 801 ? "- 1.250"
                             : tmpD1 < 1000 ? "- 1.400"
                             : "- 1.650";

            kv["SumD1"] = isHM ? "(D1) " + Num(tmpD1) : "";
            kv["SumD1e"] = isHME ? "(D1) " + Num(tmpD1) : "";
            kv["SumD1Tol"] = isHM ? tmpD1Tol : "";
            kv["SumD1TolN"] = isHM ? tmpD1TolN : "";
            kv["SumD1eTol"] = isHME ? tmpD1Tol : "";
            kv["SumD1eTolN"] = isHME ? tmpD1TolN : "";

            double[] tLista = serieInt == 30 ? TList30 : TList31;
            double tmpT = ListValue(tLista, typLista);
            string tmpTText = ListText(tLista, typLista);
            kv["SumT"] = isHM ? "" : "(T) " + tmpTText;
            kv["SumTTol"] = isHM ? "" : (tmpT < 6.1 ? "± 0.1" : tmpT < 30.1 ? "± 0.2" : "± 0.3");

            double tmpd3 = tmpkd < 501 ? tmpkd + 2 : tmpkd + 3;
            string tmpd3Tol = H13Pos(tmpd3);
            string tmpd3TolN = " 0";
            kv["Sumd3"] = isHM ? "(d3) " + Num(tmpd3) : "";
            kv["Sumd3e"] = isHME ? "(d3) " + Num(tmpd3) : "";
            kv["SumD3Tol"] = isHM ? tmpd3Tol : "";
            kv["SumD3TolN"] = isHM ? tmpd3TolN : "";
            kv["SumD3eTol"] = isHME ? tmpd3Tol : "";
            kv["SumD3eTolN"] = isHME ? tmpd3TolN : "";

            double[] bLista = isHM
                ? (serieInt == 30 ? BListHM30 : BListHM31)
                : (serieInt == 30 ? BListHME30 : BListHME31);
            double tmpB = ListValue(bLista, typLista);
            string tmpBText = ListText(bLista, typLista);

            string tmpBTol = " 0";
            string tmpBTolN = tmpB < 7 ? "- 0.180"
                            : tmpB < 11 ? "- 0.220"
                            : tmpB < 19 ? "- 0.270"
                            : tmpB < 31 ? "- 0.330"
                            : tmpB < 51 ? "- 0.390"
                            : tmpB < 81 ? "- 0.460"
                            : tmpB < 121 ? "- 0.540"
                            : "- 0.630";

            kv["SumB"] = isHM ? "(B) " + tmpBText : "";
            kv["SumBe"] = isHME ? "(B) " + tmpBText : "";
            kv["SumBTol"] = isHM ? tmpBTol : "";
            kv["SumBTolN"] = isHM ? tmpBTolN : "";
            kv["SumBeTol"] = isHME ? tmpBTol : "";
            kv["SumBeTolN"] = isHME ? tmpBTolN : "";

            string tmpRadie = tmpkd < 221 ? "R 2.5"
                            : tmpkd < 281 ? "R 3"
                            : tmpkd < 441 ? "R 3.5"
                            : tmpkd < 601 ? "R 4"
                            : tmpkd < 711 ? "R 5"
                            : "R 6";
            kv["SumRadie2"] = isHM ? tmpRadie : "";
            kv["SumRadie"] = isHME ? tmpRadie : "";
            kv["SumRHT"] = "R1.6";

            kv["SumFas2"] = isHM ? "30º" : "";
            kv["SumFas"] = isHME ? "30º" : "";
            kv["SumGF"] = "30º";
            kv["SumIF1"] = isHM ? "45º" : "";
            kv["SumIF2"] = isHM ? "45º" : "";
            kv["SumIF1e"] = isHME ? "45º" : "";
            kv["SumIF2e"] = isHME ? "45º" : "";

            string tmpKa = tmpkd < 51 ? "0.04"
                         : tmpkd < 121 ? "0.05"
                         : tmpkd < 251 ? "0.06"
                         : tmpkd < 316 ? "0.07"
                         : tmpkd < 401 ? "0.08"
                         : tmpkd < 501 ? "0.09"
                         : tmpkd < 631 ? "0.10"
                         : tmpkd < 801 ? "0.12"
                         : tmpkd < 1001 ? "0.14"
                         : "0.16";
            kv["SumKa"] = isHM ? tmpKa : "";
            kv["SumKae"] = isHME ? tmpKa : "";

            kv["SumRa32"] = isHM ? "3.2" : "";
            kv["SumRa32e"] = isHME ? "3.2" : "";

            double[] b2Lista = serieInt == 30 ? B2List30 : B2List31;
            string tmpB2a = isHM ? Num(tmpB / 2) : ListText(b2Lista, typLista);
            kv["SumB2"] = typInt < 88 ? "" : (isHM ? "(X) " + tmpB2a : "");
            kv["SumB2e"] = typInt < 88 ? "" : (isHME ? "(X) " + tmpB2a : "");

            int tmpG3;
            if (serieInt == 30)
                tmpG3 = tmpkd < 671 ? 10 : tmpkd < 901 ? 12 : 16;
            else
                tmpG3 = tmpkd < 531 ? 10 : tmpkd < 601 ? 12 : tmpkd < 751 ? 16 : tmpkd < 901 ? 20 : 24;

            string tmpG3a = serieInt == 30
                ? (tmpkd < 421 ? "" : "Gänga: M" + tmpG3.ToString(CultureInfo.InvariantCulture))
                : (tmpkd < 321 ? "" : "Gänga: M" + tmpG3.ToString(CultureInfo.InvariantCulture));
            kv["SumG3"] = isHM ? tmpG3a : "";
            kv["SumG3e"] = isHME ? tmpG3a : "";

            string tmpL3 = serieInt == 30
                ? (tmpkd < 421 ? "" : (tmpG3 == 10 ? "17" : tmpG3 == 12 ? "21" : "27"))
                : (tmpkd < 321 ? "" : (tmpG3 == 10 ? "17" : tmpG3 == 12 ? "21" : "27"));
            kv["SumL3"] = isHM ? "(L3) " + tmpL3 : "";
            kv["SumL3e"] = isHME ? "(L3) " + tmpL3 : "";
            kv["SumL3Tol"] = isHM ? "+ 2.0" : "";
            kv["SumL3TolN"] = isHM ? " 0" : "";
            kv["SumL3eTol"] = isHME ? "+ 2.0" : "";
            kv["SumL3eTolN"] = isHME ? " 0" : "";

            double tmpLd3 = tmpG3 == 10 ? 23.5
                          : tmpG3 == 12 ? 28
                          : tmpG3 == 16 ? 35
                          : tmpG3 == 20 ? 40
                          : 47;
            kv["SumLd3"] = isHM ? "max: " + Num(tmpLd3) : "";
            kv["SumLd3e"] = isHME ? "max: " + Num(tmpLd3) : "";

            double tmpta;
            if (serieInt == 30)
            {
                if (tmpkd < 221) tmpta = 9;
                else if (tmpkd < 281) tmpta = 10;
                else if (tmpkd < 341) tmpta = 12;
                else if (tmpkd < 361) tmpta = 13;
                else if (tmpkd < 421) tmpta = 14;
                else if (tmpkd < 501) tmpta = 15;
                else if (tmpkd < 671) tmpta = 20;
                else tmpta = 25;
            }
            else
            {
                if (tmpkd < 241) tmpta = 10;
                else if (tmpkd < 321) tmpta = 12;
                else if (tmpkd < 361) tmpta = 15;
                else if (tmpkd < 421) tmpta = 18;
                else if (tmpkd < 481) tmpta = 20;
                else if (tmpkd < 531) tmpta = 23;
                else if (tmpkd < 601) tmpta = 25;
                else if (tmpkd < 671) tmpta = 28;
                else if (tmpkd < 711) tmpta = 30;
                else if (tmpkd < 801) tmpta = 34;
                else tmpta = 38;
            }
            kv["Sumta"] = "(t) " + Num(tmpta);
            kv["SumtaTol"] = tmpta < 11 ? "+  1.5" : tmpta < 19 ? "+  1.8" : tmpta < 31 ? "+  2.1" : "+  2.5";
            kv["SumtaTolN"] = " 0";

            double tmpS;
            if (serieInt == 30)
            {
                if (tmpkd < 261) tmpS = 20;
                else if (tmpkd < 341) tmpS = 24;
                else if (tmpkd < 401) tmpS = 28;
                else if (tmpkd < 461) tmpS = 32;
                else if (tmpkd < 501) tmpS = 36;
                else if (tmpkd < 601) tmpS = 40;
                else if (tmpkd < 671) tmpS = 45;
                else if (tmpkd < 711) tmpS = 50;
                else if (tmpkd < 801) tmpS = 55;
                else tmpS = 60;
            }
            else
            {
                if (tmpkd < 261) tmpS = 20;
                else if (tmpkd < 321) tmpS = 24;
                else if (tmpkd < 361) tmpS = 28;
                else if (tmpkd < 421) tmpS = 32;
                else if (tmpkd < 481) tmpS = 36;
                else if (tmpkd < 531) tmpS = 40;
                else if (tmpkd < 601) tmpS = 45;
                else if (tmpkd < 671) tmpS = 50;
                else if (tmpkd < 711) tmpS = 55;
                else if (tmpkd < 801) tmpS = 60;
                else tmpS = 70;
            }
            kv["SumS"] = "(S) " + Num(tmpS);
            kv["SumSTol"] = tmpS < 31 ? "± 0.260" : tmpS < 51 ? "± 0.310" : "± 0.370";

            double[] d4Lista = isHM
                ? (serieInt == 30 ? D4ListHM30 : D4ListHM31)
                : (serieInt == 30 ? D4ListHME30 : D4ListHME31);
            double tmpd4 = ListValue(d4Lista, typLista);
            string tmpd4Text = ListText(d4Lista, typLista);

            kv["Sumd4"] = isHM ? "(d4) " + tmpd4Text : "";
            kv["Sumd4Tol"] = isHM ? "± 0.30" : "";
            kv["Sumd4e"] = isHME ? "(d4) " + tmpd4Text : "";
            kv["Sumd4eTol"] = isHME ? "± 0.30" : "";

            int tmpG2 = serieInt == 30
                ? (typInt < 84 ? 10 : typInt < 560 ? 12 : 16)
                : (typInt < 600 ? 16 : 20);
            kv["SumG2"] = isHM ? "M" + tmpG2.ToString(CultureInfo.InvariantCulture) : "";
            kv["SumG2e"] = isHME ? "M" + tmpG2.ToString(CultureInfo.InvariantCulture) : "";

            int tmpL2 = tmpG2 == 10 ? 20 : tmpG2 == 12 ? 27 : tmpG2 == 16 ? 35 : 37;
            kv["SumL2"] = isHM ? "(L2) " + tmpL2.ToString(CultureInfo.InvariantCulture) : "";
            kv["SumL2e"] = isHME ? "(L2) " + tmpL2.ToString(CultureInfo.InvariantCulture) : "";
            kv["SumL2Tol"] = isHM ? "+ 2.0" : "";
            kv["SumL2TolN"] = isHM ? " 0" : "";
            kv["SumL2eTol"] = isHME ? "+ 2.0" : "";
            kv["SumL2eTolN"] = isHME ? " 0" : "";

            int tmpLd2 = tmpG2 == 10 ? 24 : tmpG2 == 12 ? 31 : tmpG2 == 16 ? 40 : 47;
            kv["SumLd2"] = isHM ? "max " + tmpLd2.ToString(CultureInfo.InvariantCulture) : "";
            kv["SumLd2e"] = isHME ? "max " + tmpLd2.ToString(CultureInfo.InvariantCulture) : "";

            string sumG3Tolk = serieInt == 30
                ? (tmpd4 < 421 ? "" : "Tolk: M" + tmpG3.ToString(CultureInfo.InvariantCulture) + " min/max")
                : (tmpd4 < 321 ? "" : "Tolk: M" + tmpG3.ToString(CultureInfo.InvariantCulture) + " min/max");
            string sumG2Tolk = "Tolk: M" + tmpG2.ToString(CultureInfo.InvariantCulture) + " min/max";
            kv["SumG3Tolk"] = sumG3Tolk;
            kv["SumG2Tolk"] = sumG2Tolk;

            kv["SumKlEgenskaperS1"] = "PRODUCTION NUTS & SLEEVES & HOUSINGS\\ALLMÄNKLASSADE EGENSKAPER\\Klassade egenskaper muttrar";
            kv["SumKlEgenskaperS2"] = "PRODUCTION NUTS & SLEEVES & HOUSINGS\\ALLMÄNKLASSADE EGENSKAPER\\Klassade egenskaper muttrar";

            kv["SumGMall"] = "Tr x " + tmpStmm.ToString(CultureInfo.InvariantCulture);

            kv["SumTextS1"] = "Grader, frifläckar, slagmärken, repor, valkar och andra defekter samt ojämnheter kontrolleras med ögonmått, för övriga mått används skjutmått.";
            kv["SumTextS2"] = kv["SumTextS1"];
            kv["SumStämpling"] = "Stämplas enl. ritning 7433462";

            string tmpMaskinValS1 = EqualsI(maskinVal, "LB45") ? "LB45"
                                  : EqualsI(maskinVal, "EMAG") ? "EMAG"
                                  : EqualsI(maskinVal, "MaxMuller/K&T") ? "MaxMuller"
                                  : EqualsI(maskinVal, "VTR-160") ? "VTR-160"
                                  : EqualsI(maskinVal, "MacTurn 550") ? "MacTurn 550"
                                  : "";
            string tmpMaskinValS2 = EqualsI(maskinVal, "LB45") ? "LB45"
                                  : EqualsI(maskinVal, "EMAG") ? "EMAG"
                                  : EqualsI(maskinVal, "MaxMuller/K&T") ? "K&T"
                                  : EqualsI(maskinVal, "VTR-160") ? "VTR-160"
                                  : EqualsI(maskinVal, "MacTurn 550") ? "MacTurn 550"
                                  : "";

            bool isSvarv = GetMember(maskinVal, SvarvMachineList) > 0;
            kv["SumMaskinValS1"] = "Maskin: " + tmpMaskinValS1 + (isSvarv ? " - Svarvning" : " - OP1 & 2");
            kv["SumMaskinValS2"] = "Maskin: " + tmpMaskinValS2 + (isSvarv ? " - Borrning & Fräsning" : " - OP3");

            bool isMat = GetMember(maskinVal, MatMachineList) > 0;

            kv["SumF1_1"] = isMat ? "1/1" : "";
            kv["SumF1_2"] = isMat ? "1/2" : "";
            kv["SumF1_3"] = isMat ? "1/5" : "";
            kv["SumF1_4"] = isMat ? "1/5" : "";
            kv["SumF1_5"] = isMat ? "1/5" : "";
            kv["SumF1_6"] = isMat ? "1/5" : "";
            kv["SumF1_7"] = isMat ? "1/3" : "";
            kv["SumF1_8"] = isMat ? "1/5" : "";
            kv["SumF1_9"] = isMat ? "1/2" : "";
            kv["SumF2_1"] = isMat ? "1/5" : "";
            kv["SumF2_2"] = isMat ? "1/5" : "";
            kv["SumF2_3"] = isMat ? "1/5" : "";
            kv["SumF2_4"] = isMat ? "1/5" : "";

            kv["SumD1_1"] = isMat ? "Multimar" : "";
            kv["SumD1_2"] = isMat ? "Mikrometer" : "";
            kv["SumD1_3"] = isMat ? "Skjutmått" : "";
            kv["SumD1_4"] = isMat ? "Skjutmått" : "";
            kv["SumD1_5"] = isMat ? "Skjutmått" : "";
            kv["SumD1_6"] = isMat ? "Skjutmått" : "";
            kv["SumD1_7"] = isMat ? "Ytjämnhetsmätare" : "";
            kv["SumD1_8"] = isMat ? "Egglinjal" : "";
            kv["SumD1_9"] = isMat ? "Gängmall " + kv["SumGänga"] : "";
            kv["SumD2_1"] = isMat ? "Skjutmått" : "";
            kv["SumD2_2"] = isMat ? "Djupmått" : "";
            kv["SumD2_3"] = isMat ? sumG2Tolk : "";
            kv["SumD2_4"] = isMat ? sumG3Tolk : "";

            kv["SumAF1_1"] = isMat ? "Kontrolleras med passbitsklove utf. 1" : "";
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = "";
            kv["SumAF1_6"] = isMat ? "Bredd (B) OP1 + 7mm" : "";
            kv["SumAF1_7"] = isMat ? "Övriga Ra värden 6,3" : "";
            kv["SumAF1_8"] = isMat ? "Vid misstänkt formfel lämnas till mätrum" : "";
            kv["SumAF1_9"] = "";
            kv["SumAF2_1"] = "";
            kv["SumAF2_2"] = "";
            kv["SumAF2_3"] = "";
            kv["SumAF2_4"] = "";

            kv["VaLArtKontr"] = tmpV29
                ? ""
                : "FEL MALL - Denna mall gäller BARA HME 30/31 V29" +
                  "<<LineBreak>><<LineBreak>>" +
                  "---------> Kontrollera inmatningsfält <---------";

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

        private static string H13Neg(double v)
        {
            if (v < 3.01) return "- 0.140";
            if (v < 6.01) return "- 0.180";
            if (v < 10.01) return "- 0.220";
            if (v < 18.01) return "- 0.270";
            if (v < 30.01) return "- 0.330";
            if (v < 50.01) return "- 0.390";
            if (v < 80.01) return "- 0.460";
            if (v < 120.01) return "- 0.540";
            if (v < 180.01) return "- 0.630";
            if (v < 250.01) return "- 0.720";
            if (v < 315.01) return "- 0.810";
            if (v < 400.01) return "- 0.890";
            if (v < 500.01) return "- 0.970";
            if (v < 630.01) return "- 1.100";
            if (v < 800.01) return "- 1.250";
            if (v < 1000.01) return "- 1.400";
            if (v < 1250.01) return "- 1.650";
            if (v < 1600.01) return "- 1.950";
            if (v < 2000.01) return "- 2.300";
            if (v < 2500.01) return "- 2.800";
            return "- 3.300";
        }

        private static string H13Pos(double v)
        {
            if (v < 3.01) return "+ 0.140";
            if (v < 6.01) return "+ 0.180";
            if (v < 10.01) return "+ 0.220";
            if (v < 18.01) return "+ 0.270";
            if (v < 30.01) return "+ 0.330";
            if (v < 50.01) return "+ 0.390";
            if (v < 80.01) return "+ 0.460";
            if (v < 120.01) return "+ 0.540";
            if (v < 180.01) return "+ 0.630";
            if (v < 250.01) return "+ 0.720";
            if (v < 315.01) return "+ 0.810";
            if (v < 400.01) return "+ 0.890";
            if (v < 500.01) return "+ 0.970";
            if (v < 630.01) return "+ 1.100";
            if (v < 800.01) return "+ 1.250";
            if (v < 1000.01) return "+ 1.400";
            if (v < 1250.01) return "+ 1.650";
            if (v < 1600.01) return "+ 1.950";
            if (v < 2000.01) return "+ 2.300";
            if (v < 2500.01) return "+ 2.800";
            return "+ 3.300";
        }

        private static double ListValue(double[] list, int index)
        {
            if (list == null || index <= 0 || index > list.Length) return 0;
            return list[index - 1];
        }

        private static string ListText(double[] list, int index)
        {
            if (list == null || index <= 0 || index > list.Length) return "";
            return Num(list[index - 1]);
        }

        private static int GetMember(string val, string[] list)
        {
            if (val == null) return 0;
            for (int i = 0; i < list.Length; i++)
                if (string.Equals(list[i], val, StringComparison.OrdinalIgnoreCase)) return i + 1;
            return 0;
        }

        private static bool EqualsI(string a, string b)
        {
            return string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);
        }

        private static bool Eq(double a, double b)
        {
            return Math.Abs(a - b) < 0.0001;
        }

        private static int TryParseInt(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;
            int v;
            return int.TryParse(s.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static double TryParseDouble(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;
            double v;
            return double.TryParse(s.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static string Fmt(double v)
        {
            return Math.Round(v, 4).ToString("0.####", CultureInfo.InvariantCulture);
        }

        private static string Num(double v)
        {
            return Fmt(v).Replace(".", ",");
        }
    }
}