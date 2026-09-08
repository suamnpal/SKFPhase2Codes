using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Klamhylsa_komplett_30_39_H_HE_storlek_60 : ITemplateCalculations
    {
        private static readonly string[] MachinesOP1 = new[] { "MacTurn 550", "VTR-160" };
        private static readonly string[] MachinesOP2 = new[] { "Skepp6", "K&T", "VTR-160", "MacTurn 550", "Dubbelparet" };
        private static readonly string[] MachinesSlits = new[] { "Skepp6", "K&T", "VTR-160", "MacTurn 550" };

        private static readonly string[] Typ30 = { "32", "34", "36", "38", "40", "44", "48", "52", "56", "60", "64", "68", "72", "76", "80", "84", "88", "92", "96", "500", "530", "560", "600", "630", "670", "710", "750", "800", "850", "900", "950", "1000", "1060", "378", "420", "460" };
        private static readonly string[] Typ31 = { "32", "34", "36", "38", "40", "44", "48", "52", "56", "60", "64", "68", "72", "76", "80", "84", "88", "92", "96", "500", "530", "560", "600", "630", "670", "710", "750", "800", "850", "900", "1000", "1060", "420", "355,6" };
        private static readonly string[] Typ23 = { "32", "34", "36", "38", "40", "44", "48", "52", "56" };
        private static readonly string[] Typ32 = { "60", "64", "68", "72", "76", "80", "84", "88", "92", "96", "500", "530", "560", "600", "630", "670", "710", "750", "800", "850" };
        private static readonly string[] Typ39 = { "32", "34", "36", "38", "40", "44", "48", "52", "56", "60", "64", "68", "72", "76", "80", "84", "88", "92", "96", "500", "530", "560", "600", "630", "670", "710", "750", "800", "850", "900", "950", "1000", "1060", "457,2" };

        private static readonly double[] BTab30 = { 4.2, 4.2, 4.2, 4.2, 4.2, 3.9, 3.9, 3.9, 3.9, 3.9, 3.5, 3.5, 3.5, 3.5, 3.5, 3.5, 6.5, 6.5, 6.5, 6.5, 6, 6, 8, 6, 8, 8, 8, 10, 10, 10, 10, 10, 12, 4, 3.5, 3.5 };
        private static readonly double[] BTab31 = { 4.2, 4.2, 4.2, 4.2, 4.2, 3.9, 3.9, 3.9, 3.9, 3.9, 3.5, 3.5, 3.5, 3.5, 3.5, 3.5, 6.5, 6.5, 6.5, 6.5, 6, 6, 8, 6, 8, 8, 8, 10, 10, 10, 10, 12, 3.5, 4.5 };
        private static readonly double[] BTab23 = { 4.2, 4.2, 4.2, 4.2, 4.2, 3.9, 3.9, 3.9, 3.9 };
        private static readonly double[] BTab32 = { 3.9, 3.5, 3.5, 3.5, 3.5, 3.5, 3.5, 6.5, 6.5, 6.5, 6, 6, 6, 8, 6, 8, 8, 8, 10, 10 };
        private static readonly double[] BTab39 = { 4.2, 4.2, 4.2, 4.2, 4.2, 3.9, 3.9, 3.9, 3.9, 3.9, 3.5, 3.5, 3.5, 3.5, 3.5, 3.5, 6.5, 6.5, 6.5, 6.5, 6, 6, 8, 6, 8, 8, 8, 10, 10, 10, 10, 10, 12, 4 };

        private static readonly double[] ETab30 = { 57, 61, 66, 68, 72, 75.5, 80.5, 86, 90.5, 100, 101.5, 110, 110.5, 114, 123.5, 124.5, 135.5, 138.5, 139.5, 148, 156.5, 168, 170, 175.5, 188.5, 201, 207.5, 211, 217.5, 231, 240, 244, 251, 122, 135, 139 };
        private static readonly double[] ETab31 = { 69, 71, 75, 81, 85, 92, 98, 107, 110, 117, 126, 145.5, 149.5, 152.5, 157, 176, 176.5, 188, 191.5, 204, 207.5, 216, 225, 242, 262, 266.5, 281.5, 286, 303, 315.5, 327, 338, 344, 176, 151 };
        private static readonly double[] ETab23 = { 82, 85, 89, 93, 97, 104, 110, 117, 123 };
        private static readonly double[] ETab32 = { 131.5, 140.5, 161, 166, 172, 181, 196, 200, 212, 219, 235, 244, 255.5, 265.5, 286.5, 309, 314.5, 332, 337.5, 356 };
        private static readonly double[] ETab39 = { 51, 52, 56, 57, 62, 60, 64, 71, 75, 86, 86, 89, 89, 99, 103, 103, 117, 117, 122, 130, 134, 143, 147.5, 154.5, 161.5, 176, 178.5, 183, 185, 197.5, 205.8, 211.2, 217.5, 122 };

        private static readonly double[] JTab30 = { 54.5, 58.5, 63, 64.5, 68.5, 70.5, 75.5, 81, 85.5, 95, 96.5, 105, 105.5, 109, 118.5, 119.5, 130.5, 133.5, 134.5, 143, 151.5, 163, 165, 170.5, 183.5, 196, 202.5, 206, 212.5, 226, 235, 239, 246, 118.5, 130.5, 134.5 };
        private static readonly double[] JTab31 = { 66, 68, 72.5, 77.5, 82, 89, 94.5, 104, 106.5, 112, 121, 140.5, 144.5, 147.5, 152, 171, 171.5, 183, 186.5, 199, 202.5, 211, 220, 237, 257, 261.5, 276.5, 281, 298, 310.5, 322, 333, 339, 171.5, 147.5 };
        private static readonly double[] JTab23 = { 79, 82.5, 86, 90, 93.5, 100.5, 107, 113.5, 120 };
        private static readonly double[] JTab32 = { 126.5, 135.5, 156, 162.5, 168, 177, 192.5, 196, 208, 214.5, 231, 240, 249.5, 259.5, 280.5, 303, 308.5, 326, 331.5, 350 };
        private static readonly double[] JTab39 = { 48, 49, 53, 54, 58.5, 57, 61, 68, 72, 82.5, 82.5, 85.5, 85.5, 95.5, 99.5, 99.5, 113, 113, 117.5, 125.5, 129, 138, 142.5, 149.5, 156.5, 171, 173.5, 178, 180, 192.5, 200.8, 206.2, 212.5, 117.5 };

        private static readonly double[] CTab30 = { 4, 4, 4, 4, 4, 4, 4, 4, 4, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 8, 8, 8 };
        private static readonly double[] CTab31 = { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 8, 8 };
        private static readonly double[] CTab23 = { 4.2, 4.2, 4.2, 4.2, 4.2, 3.9, 3.9, 3.9, 3.9 };
        private static readonly double[] CTab32 = { 4, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 };
        private static readonly double[] CTab39 = { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 8 };

        private static readonly double[] DTab30 = { 4.2, 4.2, 4.2, 4.2, 4.2, 21, 21, 22, 22, 22, 25, 26, 26, 26, 27, 27, 27, 28, 28, 28, 34, 34, 35, 35, 36, 38, 38, 39, 39, 40, 42, 44, 44, 4, 3.5, 3.5 };
        private static readonly double[] DTab31 = { 4, 4, 4, 4, 4, 4, 4, 4, 4, 22, 25, 26, 26, 26, 27, 27, 27, 28, 28, 28, 34, 34, 35, 35, 36, 38, 38, 39, 39, 40, 42, 44, 44, 8, 8 };
        private static readonly double[] DTab23 = { 4.2, 4.2, 4.2, 4.2, 4.2, 3.9, 3.9, 3.9, 3.9 };
        private static readonly double[] DTab32 = { 22, 25, 26, 26, 26, 27, 27, 27, 28, 28, 28, 34, 34, 35, 35, 36, 38, 38, 39, 39, 40, 42, 44 };
        private static readonly double[] DTab39 = { 4, 4, 4, 4, 4, 21, 21, 22, 22, 22, 25, 26, 26, 26, 27, 27, 27, 28, 28, 28, 34, 34, 35, 35, 36, 38, 38, 39, 39, 40, 42, 44, 44, 22 };

        private static readonly double[] E5Tab30 = { 4.2, 4.2, 4.2, 4.2, 4.2, 20, 20, 20, 24, 24, 24, 24, 28, 28, 28, 32, 32, 32, 36, 36, 40, 40, 40, 45, 45, 50, 55, 55, 60, 60, 60, 60, 60, 4, 3.5, 3.5 };
        private static readonly double[] E5Tab31 = { 4, 4, 4, 4, 4, 4, 4, 4, 4, 24, 24, 28, 28, 32, 32, 32, 36, 36, 36, 40, 40, 45, 45, 50, 50, 55, 60, 60, 70, 70, 70, 70, 70, 8, 8 };
        private static readonly double[] E5Tab23 = { 4.2, 4.2, 4.2, 4.2, 4.2, 3.9, 3.9, 3.9, 3.9 };
        private static readonly double[] E5Tab32 = { 24, 24, 28, 28, 32, 32, 32, 36, 36, 36, 40, 40, 45, 45, 50, 50, 55, 60, 60, 70, 70, 70, 70 };
        private static readonly double[] E5Tab39 = { 4.2, 4.2, 4.2, 4.2, 4.2, 20, 20, 20, 24, 24, 24, 24, 28, 28, 28, 32, 32, 32, 36, 36, 40, 40, 40, 45, 45, 50, 55, 55, 60, 60, 60, 60, 60, 24 };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;
            List<Bookmark> bm = req != null ? req.Bookmarks : null;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");

            bool tmpSlash = tmpBet.IndexOf('/') >= 0;
            bool tmpLW = tmpBet.IndexOf("LW", StringComparison.OrdinalIgnoreCase) >= 0;

            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : string.Empty;
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : string.Empty;
            string tmpBet4 = tokens.Length > 3 ? tokens[3] : string.Empty;

            int tmpCountB2 = tmpBet2.Length;
            int tmpCountB3 = tmpBet3.Length;

            string tmpSerie;
            if (tmpLW)
            {
                tmpSerie = "0";
            }
            else if (tmpSlash && (tmpCountB2 == 3 || tmpCountB2 == 2))
            {
                tmpSerie = tmpBet2;
            }
            else if (tmpCountB2 > 4)
            {
                tmpSerie = tmpBet2.Substring(0, 3);
            }
            else if (tmpCountB2 == 3)
            {
                tmpSerie = tmpBet2.Substring(0, 1);
            }
            else
            {
                tmpSerie = tmpBet2.Length >= 2 ? tmpBet2.Substring(0, 2) : tmpBet2;
            }

            string tmpTyp;
            if (!tmpSlash)
            {
                tmpTyp = tmpBet2.Length >= 2 ? tmpBet2.Substring(tmpBet2.Length - 2) : tmpBet2;
            }
            else
            {
                tmpTyp = tmpCountB3 > 4
                    ? (tmpBet2.Length >= 2 ? tmpBet2.Substring(tmpBet2.Length - 2) : tmpBet2)
                    : tmpBet3;
            }

            int serieInt = TryParseInt(tmpSerie);
            double typNum = TryParseDouble(tmpTyp);

            double inAvvDia = GetDouble(bm, "Avvikande YDia Gänga (d)");
            double ind1 = GetDouble(bm, "Innerdiameter (d1)");
            double ind2 = GetDouble(bm, "Kona storände diameter (d2)");
            double inb = GetDouble(bm, "Gänglängd (b)");
            double inL = GetDouble(bm, "Längd (L)");
            double inKona = GetDouble(bm, "Kona");
            double inAmatt = GetDouble(bm, "a-mått");
            string produktritning = GetString(bm, "Produktritning");
            string gangTyp = GetString(bm, "Gäng typ");
            string radieLillande = GetString(bm, "Radie Lillände");
            string radieStorande = GetString(bm, "Radie Storände");

            double tmpd;
            if (inAvvDia != 0)
            {
                tmpd = inAvvDia;
            }
            else if (!tmpSlash || tmpCountB3 > 4)
            {
                tmpd = (typNum / 2.0) * 10.0;
            }
            else
            {
                tmpd = TryParseDouble(tmpBet3);
            }

            double tmpd1 = ind1;
            double tmpd2 = ind2;
            double tmpb = inb;
            double tmpL = inL;

            int tmpStmm = tmpd < 301 ? 4 : tmpd < 501 ? 5 : tmpd < 701 ? 6 : tmpd < 901 ? 7 : 8;

            kv["SumGänga"] = "Tr" + FmtD(tmpd) + "x" + tmpStmm;
            kv["SumP"] = "Tr" + tmpStmm;
            kv["SumP1"] = "(P) " + tmpStmm;
            kv["SumRullar"] = tmpStmm + "mm";

            double tmpKona = inKona == 0
                ? (serieInt == 30 || serieInt == 31 || serieInt == 32 || serieInt == 39 ? 12 : 30)
                : inKona;

            kv["SumV"] = Math.Abs(tmpKona - 30) < 0.001 ? "(V) 0 57" : "(V) 2 23";
            kv["SumKonaOP1"] = "Kona 1:" + FmtD(tmpKona);
            kv["SumKona"] = kv["SumKonaOP1"];

            kv["SumLOP1"] = "(L) " + FmtD(tmpL);
            kv["SumLOP1Tol"] = "0.250";
            kv["SumL"] = "(L) " + FmtD(tmpL);
            kv["SumLTol"] = "+ 0.000 [3F]";
            kv["SumLTolN"] = "- " + Fmt3(LTolN(tmpL)) + " [3F]";

            kv["Sumb"] = "(b) " + FmtD(tmpb);
            kv["SumbTol"] = "+ " + Fmt3(bTolPos(tmpb)) + " [3F]";
            kv["SumbTolN"] = "- 0.000 [2F]";

            double tmpSL = tmpb + 4;
            kv["SumSL"] = "(SL) " + FmtD(tmpSL);
            kv["SumSLTol"] = "± 0.300";

            kv["Sumd1"] = "(d1) " + FmtD(tmpd1);
            kv["Sumd1Tol"] = "± " + Fmt3(d1Tol(tmpd1)) + " [3F]";

            kv["SumdOP1"] = "(d) " + FmtD(tmpd);
            kv["SumdOP1Tol"] = "+ 0.000";
            kv["SumdOP1TolN"] = "- " + Fmt3(dOP1TolN(tmpStmm));
            kv["SumdaOP1"] = kv["SumdOP1"];
            kv["SumdaOP1Tol"] = kv["SumdOP1Tol"];
            kv["SumdaOP1TolN"] = kv["SumdOP1TolN"];
            kv["Sumd"] = kv["SumdOP1"];
            kv["SumdTol"] = kv["SumdOP1Tol"];
            kv["SumdTolN"] = kv["SumdOP1TolN"];

            double tmpdm = tmpd - (tmpStmm == 4 ? 2 : tmpStmm == 5 ? 2.5 : tmpStmm == 6 ? 3 : tmpStmm == 7 ? 3.5 : 4);
            kv["Sumdm"] = "(dm) 298";
            kv["SumdmTol"] = "- " + Fmt3(dmTolPos(tmpStmm)) + " [3F]";
            kv["SumdmTolN"] = "- " + Fmt3(dmTolN(tmpStmm)) + " [3F]";

            double tmpd3 = tmpdm - (tmpStmm == 4 ? 2.5 : tmpStmm == 5 ? 3 : tmpStmm == 6 ? 4 : tmpStmm == 7 ? 4.5 : (tmpdm < 1300 ? 5 : 4.5));
            kv["Sumd3"] = "(d3) 295,5";
            kv["Sumd3Tol"] = "+ 0.000";
            kv["Sumd3TolN"] = "- " + Fmt3(d3TolN(tmpStmm));

            string tmpg = (produktritning == "MS-7434039")
                ? "4.4x30 "
                : tmpd < 301 ? "2.7x45 " : tmpd < 501 ? "3.2x45 " : tmpd < 671 ? "3.8x45 " : tmpd < 901 ? "4.4x45 " : "5x45 ";
            kv["Sumg"] = "(g) 2.7x45º";

            double sumr3val = (radieLillande == "0" || string.IsNullOrEmpty(radieLillande))
                ? (tmpd < 421 ? 1 : 2.5)
                : TryParseDouble(radieLillande);

            double sumr2val = (radieStorande == "0" || string.IsNullOrEmpty(radieStorande))
                ? (tmpd < 320 ? 2.5 : tmpd < 530 ? 3.5 : tmpd < 710 ? 5.5 : 7.5)
                : TryParseDouble(radieStorande);

            kv["Sumr3"] = FmtD(sumr3val);
            kv["Sumr4"] = kv["Sumr3"];
            kv["Sumr1"] = "R" + kv["Sumr4"];

            kv["Sumr2"] = FmtD(sumr2val);
            kv["Sumr5"] = kv["Sumr2"];
            kv["Sumr"] = "R" + kv["Sumr5"];

            kv["SumR1"] = kv["Sumr1"];
            kv["SumR"] = kv["Sumr"];

            double tmpGTjTol = GTjTolPos(tmpKona, tmpd);
            double tmpGTjTolN = GTjTolNeg(tmpKona, tmpd);
            double tmpTolerSkillnad = tmpGTjTolN - tmpGTjTol;

            kv["SumGTjTol"] = "+ " + Fmt3(tmpGTjTol);
            kv["SumGTjaTol"] = kv["SumGTjTol"];
            kv["SumGTjTolN"] = "- " + Fmt3(tmpGTjTolN);
            kv["SumGTjaTolN"] = kv["SumGTjTolN"];

            double tmpd2a = tmpd2 != 0
                ? tmpd2
                : Math.Round(((tmpL - 1 - inAmatt) / tmpKona) + tmpd - tmpTolerSkillnad, 2);
            kv["Sumd2"] = "(d2) 316.7";
            kv["Sumd2Tol"] = " " + Fmt3(GenTol(tmpd2a));

            kv["SumGVarTol"] = Fmt3(GVarTol(tmpd)) + " [2F]";

            kv["SumRakA"] = "Max: " + FmtD(RakA(tmpd));
            kv["SumRakB"] = "Max: " + FmtD(RakB(tmpd));

            kv["SumOrund"] = Fmt3(Orund(tmpd1)) + " [3F]";

            double tmpML = tmpL - tmpb - 4;
            kv["SumML"] = "Mållängd=" + (tmpML < 110 ? "75" : "100");

            kv["SumRa"] = "2.5 [2F]";
            kv["SumRa1"] = "2.5 [2F]";
            kv["SumRa5"] = "5 [2F]";

            int tmpVML = tmpML < 110 ? 75 : 100;
            kv["SumVinkTol"] = FmtD(VinkTol(tmpd, tmpVML)) + " [2F]";

            double sumL2 = tmpML < 145 ? 8 : 40;
            double sumL1 = tmpML < 110 ? 83 : tmpML < 145 ? 108 : 140;
            kv["SumL2"] = FmtD(sumL2);
            kv["SumL1"] = FmtD(sumL1);

            double t8 = Math.Round(((tmpd - tmpd1) / 2) + ((tmpL - 1 - inAmatt - 8) / (2 * tmpKona)), 3);
            double t83 = Math.Round(((tmpd - tmpd1) / 2) + ((tmpL - 1 - inAmatt - 83) / (2 * tmpKona)), 3);
            double t108 = Math.Round(((tmpd - tmpd1) / 2) + ((tmpL - 1 - inAmatt - 108) / (2 * tmpKona)), 3);
            double t40 = Math.Round(((tmpd - tmpd1) / 2) + ((tmpL - 1 - inAmatt - 40) / (2 * tmpKona)), 3);
            double t140 = Math.Round(((tmpd - tmpd1) / 2) + ((tmpL - 1 - inAmatt - 140) / (2 * tmpKona)), 3);

            kv["SumE1"] = "12,458 ";
            kv["SumE2"] = "16,625 ";

            kv["SumBygGtj"] = tmpML < 145 ? "SR 7419471" : "SR 7415991";
            kv["SumBygGVar"] = kv["SumBygGtj"];
            kv["SumBygVinkTol"] = kv["SumBygGtj"];

            SumMaskinValOP1(kv, maskinVal);
            SumFrequenciesOP1(kv, maskinVal);
            SumMeasuringDevicesOP1(kv, maskinVal);
            SumAFOP1(kv, maskinVal);

            kv["SumTextS1"] = "Kontrollera rätt märkning<<LineBreak>>Okulärkontroll gjuteridefekter, grader & slagmärken<<LineBreak>>Gjutgodsdefekter: 7433015";
            kv["SumTextS2"] = "Bryt alla kanter, avlägsna. Okulärkontroll märkning, gjuteridefekekter, grader & slagmärken";

            string tmpRitningsnr;
            if (produktritning == "0" || string.IsNullOrEmpty(produktritning))
            {
                tmpRitningsnr = serieInt == 30 ? "7438957" :
                                serieInt == 31 ? "7438958" :
                                serieInt == 32 ? "7438955" :
                                serieInt == 39 ? "7434032" :
                                serieInt == 240 ? "7432901" : tmpBet;
            }
            else
            {
                tmpRitningsnr = produktritning;
            }
            tmpRitningsnr += ":senaste utg.";
            kv["SumRitningsnrS1"] = tmpRitningsnr;
            kv["SumRitningsnrS2"] = tmpRitningsnr;
            kv["SumRitTolS1"] = "Toleranser: 1432012:7";
            kv["SumRitTolS2"] = kv["SumRitTolS1"];
            kv["SumRitGänga"] = "Gänga: 237359:2, 7430181:2";

            kv["SumKlEgenskaperS1"] = "PRODUCTION NUTS & SLEEVES & HOUSINGS\\ALLMÄNKLASSADE EGENSKAPER\\Klassade egenskaper klämhylsor";
            kv["SumKlEgenskaperS2"] = kv["SumKlEgenskaperS1"];

            string tmpRit = serieInt == 23 || serieInt == 30 ? "7434151" :
                            serieInt == 31 ? "7434152" :
                            serieInt == 32 ? "7434153" :
                            serieInt == 39 ? "7434170, 7434171" : "Fel Mall";
            kv["SumRitNr"] = tmpRit;
            kv["SumRitNr4"] = "7438955";
            kv["SumG1"] = gangTyp;

            int typIdx = GetTypIndex(serieInt, tmpTyp);

            double tmpB1val = GetTableValue(serieInt, typIdx, BTab30, BTab31, BTab23, BTab32, BTab39);
            kv["SumB1"] = "(B) 3,9";
            kv["SumB1Tol"] = Math.Abs(tmpB1val - 4) < 0.001 ? "- 0.1" : " 0";
            kv["SumB1TolN"] = Math.Abs(tmpB1val - 4) < 0.001 ? "- 0.3" : "- 0.1";

            string tmpC39 = gangTyp == "M6" ? "9" : gangTyp == "M8" ? "12" : (gangTyp == "G1/8" || gangTyp == "G 1/8") ? "13" : (gangTyp == "G1/4" || gangTyp == "G 1/4") ? "15" : "";
            string tmpC11 = gangTyp == "M6" ? "10" : gangTyp == "M8" ? "12" : (gangTyp == "G1/8" || gangTyp == "G 1/8") ? "12" : (gangTyp == "G1/4" || gangTyp == "G 1/4") ? "15" : "";
            string tmpC1 = serieInt == 39 ? tmpC39 : tmpC11;
            double tmpC1d = TryParseDouble(tmpC1);

            kv["SumC1"] = "(C) " + tmpC1;
            kv["SumC1Tol"] = "± 0.2";

            double tmpT = (typNum < 85 || EqualsAnyTyp(tmpTyp, "420", "355,6", "378")) ? 6.3 : (typNum < 561 || EqualsAnyTyp(tmpTyp, "630")) ? 8.3 : 10;
            kv["SumT"] = "(T) 6,3";
            kv["SumTTol"] = "± 0.2";

            double tmpS = (typNum < 85 || EqualsAnyTyp(tmpTyp, "420", "355,6", "378")) ? 3 : (typNum < 561 || EqualsAnyTyp(tmpTyp, "630")) ? 4 : 5;
            kv["SumS"] = "(D) " + FmtD(tmpS);
            kv["SumSTol"] = tmpS < 6.1 ? "± 0.1" : tmpS < 30.1 ? "± 0.2" : "± 0.3";

            double tmpH = GetH(serieInt, typNum, tmpTyp);
            kv["SumH"] = "(H) " + FmtD(tmpH);
            kv["SumHTol"] = tmpH < 6.1 ? "± 0.1" : "± 0.2";

            double tmpE = GetTableValue(serieInt, typIdx, ETab30, ETab31, ETab23, ETab32, ETab39);
            kv["SumE"] = "(E)131,5";
            kv["SumETol"] = "± "+ GenTolPM(tmpE);

            double tmpJ = GetTableValue(serieInt, typIdx, JTab30, JTab31, JTab23, JTab32, JTab39);
            kv["SumJ"] = "(J) 126,5";
            kv["SumJTol"] = "± "+ GenTolPM(tmpJ);

            kv["SumF"] = "(F) 3";
            kv["SumFTol"] = tmpH < 6.1 ? "± 0.1" : "± 0.2";

            double tmpN = typNum < 37 ? 4 : typNum < 65 ? 5.3 : (typNum < 85 || EqualsAnyTyp(tmpTyp, "378", "355,6")) ? 6 : (typNum < 601 || EqualsAnyTyp(tmpTyp, "420", "460", "457,2")) ? 7 : typNum < 751 ? 8 : 9;
            kv["SumN"] = "(N) 5,3";
            kv["SumNTol"] = tmpH < 6.1 ? "± 0.1" : "± 0.2";

            double tmpR8 = typNum < 37 ? 3 : typNum < 65 ? 4 : (typNum < 85 || EqualsAnyTyp(tmpTyp, "378", "355,6")) ? 4.5 : 5;
            kv["SumR8"] = "R" + FmtD(tmpR8);

            string tmpR7 = (typNum < 85 || EqualsAnyTyp(tmpTyp, "378", "420", "460", "355,6")) ? "1" : "2.5";
            kv["SumR7"] = "R" + tmpR7.Replace(",", ".");

            kv["SumV120"] = "120º ";
            kv["SumV45"] = "45º "; 

            SumMaskinValOP2(kv, maskinVal);
            SumFrequenciesOP2(kv, maskinVal);
            SumMeasuringDevicesOP2(kv, maskinVal);
            SumAFOP2(kv, maskinVal);

            kv["SumTextS3"] = "Grader, frifläckar, slagmärken, repor, valkar och andra defekter samt ojämnheter kontrolleras med ögonmått.";
            kv["SumTextGängstigning"] = "Max 2x gängstigning";
            //new line issue
            // kv["SumÖvrigt"] = "1 st. oljeborrhål 180 från slits"+ Environment.NewLine +"1 st. utv. oljespår med början 10 från slitscentrum";
            kv["SumÖvrigt"] = "1 st. oljeborrhål 180º från slits<<LineBreak>>1 st. utv. oljespår med början 10º från slitscentrum";

            kv["SumVa"] = "11,25º ";
            kv["SumV1"] = "11,25º ";
            kv["SumV30"] = "30º ";

            double tmpCval = GetTableValue(serieInt, typIdx, CTab30, CTab31, CTab23, CTab32, CTab39);
            kv["SumC"] = "(B) " + FmtD(tmpCval);
            kv["SumCTol"] = "± 0.2";

            kv["Sumd10"] = "(d1) " + FmtD(tmpd1);
            kv["Sumd10Tol"] = d10TolPos(typNum);
            kv["Sumd10TolN"] = d10TolNeg(typNum);

            double tmpD11val = GetTableValue(serieInt, typIdx, DTab30, DTab31, DTab23, DTab32, DTab39);
            kv["SumF0"] = "(f) " + FmtD(tmpD11val);
            kv["SumF02"] = kv["SumF0"];
            kv["SumF0Tol"] = tmpD11val > 50 ? "+ 3.0" : tmpD11val > 30 ? "+ 2.5" : tmpD11val > 19 ? "+ 2.1" : "+ 1.8";
            kv["SumF02Tol"] = kv["SumF0Tol"];
            kv["SumF0TolN"] = " 0 [3F]";
            kv["SumF02TolN"] = kv["SumF0TolN"];

            double tmpE5val = GetTableValue(serieInt, typIdx, E5Tab30, E5Tab31, E5Tab23, E5Tab32, E5Tab39);
            kv["SumE5"] = "(e) " + FmtD(tmpE5val);
            kv["SumE5Tol"] = tmpE5val > 50 ? "+ 0.740" : tmpE5val > 30 ? "+ 0.620" : tmpE5val > 18 ? "+ 0.520" : tmpE5val > 10 ? "+ 0.430" : tmpE5val > 6 ? "+ 0.360" : "+ 0.300";
            kv["SumE5TolN"] = " 0 [3F]";

            SumMaskinValSlits(kv, maskinVal);
            SumFrequenciesSlits(kv, maskinVal);
            SumMeasuringDevicesSlits(kv, maskinVal);
            SumAFSlits(kv, maskinVal);

            kv["SumTextS4"] = "";

            string valH = (EqualsI(tmpBet3, "H") || EqualsI(tmpBet4, "H"))
                ? ""
                : "Denna Mall är endast för typ OH-H\n" + (EqualsI(tmpBet4, "HB") ? "Använd mall för OH-HB" : "Använd mall för OH");
            kv["VaLH"] = valH;

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

        private static void SumMaskinValOP1(Dictionary<string, string> kv, string maskinVal)
        {
            string s1 = EqualsI(maskinVal, "MacTurn 550") ? "MacTurn 550" : EqualsI(maskinVal, "VTR-160") ? "VTR-160" : "";
            kv["SumMaskinValS1"] = ("Maskin: " + s1 + " - OP1").Trim();
            string s2 = EqualsI(maskinVal, "MacTurn 550") ? "MacTurn 550" : EqualsI(maskinVal, "VTR-160") ? "VTR-160" : "";
            kv["SumMaskinValS2"] = ("Maskin: " + s2 + " - OP2").Trim();
        }

        private static void SumFrequenciesOP1(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsInGroup(maskinVal, MachinesOP1);
            kv["SumF3_1"] = m ? "1/5" : "";
            kv["SumF3_2"] = m ? "1/2" : "";
            kv["SumF3_3"] = m ? "1/1" : "";
            kv["SumF3_4"] = m ? "1/5" : "";
            kv["SumF3_5"] = m ? "Inst." : "";
            kv["SumF3_6"] = m ? "1/5" : "";
            kv["SumF3_7"] = m ? "1/2" : "";
            kv["SumF3_8"] = m ? "1/5" : "";
            kv["SumF4_1"] = m ? "1/2" : "";
            kv["SumF4_2"] = m ? "1/2" : "";
            kv["SumF4_3"] = m ? "1/1" : "";
            kv["SumF4_4"] = m ? "1/1" : "";
            kv["SumF4_5"] = m ? "1/1" : "";
            kv["SumF4_6"] = m ? "Inst." : "";
            kv["SumF4_7"] = m ? "1/5" : "";
            kv["SumF4_8"] = m ? "1/5" : "";
            kv["SumF4_9"] = m ? "1/1" : "";
        }

        private static void SumMeasuringDevicesOP1(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsInGroup(maskinVal, MachinesOP1);
            string sumRullar = kv.ContainsKey("SumRullar") ? kv["SumRullar"] : "";
            string sumP = kv.ContainsKey("SumP") ? kv["SumP"] : "";
            kv["SumD3_1"] = m ? "Skjutmått" : "";
            kv["SumD3_2"] = m ? "Skjutmått" : "";
            kv["SumD3_3"] = m ? "Multimar med " + sumRullar + " rullar" : "";
            kv["SumD3_4"] = m ? "Djupmått" : "";
            kv["SumD3_5"] = m ? "Radielyra" : "";
            kv["SumD3_6"] = m ? "Skjutmått/Vinkelmätare" : "";
            kv["SumD3_7"] = m ? "Gängmall " + sumP : "";
            kv["SumD3_8"] = m ? "Skjutmått" : "";
            string sumBygGtj = kv.ContainsKey("SumBygGtj") ? kv["SumBygGtj"] : "";
            string sumBygGVar = kv.ContainsKey("SumBygGVar") ? kv["SumBygGVar"] : "";
            string sumBygVink = kv.ContainsKey("SumBygVinkTol") ? kv["SumBygVinkTol"] : "";
            kv["SumD4_1"] = m ? "Mikrometerstickmått" : "";
            kv["SumD4_2"] = m ? "Skjutmått" : "";
            kv["SumD4_3"] = m ? "Mätbygel " + sumBygGtj : "";
            kv["SumD4_4"] = m ? "Mätbygel " + sumBygGVar : "";
            kv["SumD4_5"] = m ? "Mätbygel " + sumBygVink : "";
            kv["SumD4_6"] = m ? "Mätmaskin" : "";
            kv["SumD4_7"] = m ? "Egglinjal" : "";
            kv["SumD4_8"] = m ? "Egglinjal" : "";
            kv["SumD4_9"] = m ? "Okulärkontroll" : "";
        }

        private static void SumAFOP1(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsInGroup(maskinVal, MachinesOP1);
            string sumGVarTol = kv.ContainsKey("SumGVarTol") ? kv["SumGVarTol"] : "";
            string sumVinkTol = kv.ContainsKey("SumVinkTol") ? kv["SumVinkTol"] : "";
            string sumML = kv.ContainsKey("SumML") ? kv["SumML"] : "";
            string sumOrund = kv.ContainsKey("SumOrund") ? kv["SumOrund"] : "";
            string sumRakA = kv.ContainsKey("SumRakA") ? kv["SumRakA"] : "";
            string sumRakB = kv.ContainsKey("SumRakB") ? kv["SumRakB"] : "";
            kv["SumAF3_1"] = "";
            kv["SumAF3_2"] = "";
            kv["SumAF3_3"] = m ? "Kontrolleras med klove utf.2" : "";
            kv["SumAF3_4"] = "";
            kv["SumAF3_5"] = "";
            kv["SumAF3_6"] = "";
            kv["SumAF3_7"] = "";
            kv["SumAF3_8"] = m ? "Hjälpmått" : "";
            kv["SumAF4_1"] = "";
            kv["SumAF4_2"] = "";
            kv["SumAF4_3"] = m ? "Tol:" : "";
            kv["SumAF4_4"] = m ? "Tol: " + sumGVarTol : "";
            kv["SumAF4_5"] = "Tol: 0.015 [2F] Mätlängd=100";
            kv["SumAF4_6"] = m ? "Max: " + sumOrund : "";
            kv["SumAF4_7"] = m ? sumRakA : "";
            kv["SumAF4_8"] = m ? sumRakB : "";
            kv["SumAF4_9"] = m ? "Vid misstänkt fel Ra-mätare" : "";
        }

        private static void SumMaskinValOP2(Dictionary<string, string> kv, string maskinVal)
        {
            string s = EqualsI(maskinVal, "Skepp6") ? "Skepp6" :
                       EqualsI(maskinVal, "K&T") ? "K&T" :
                       EqualsI(maskinVal, "VTR-160") ? "VTR-160" :
                       EqualsI(maskinVal, "MacTurn 550") ? "MacTurn 550" :
                       EqualsI(maskinVal, "Dubbelparet") ? "Dubbelparet" : "";
            kv["SumMaskinValS3"] = ("Maskin: " + s + " - BorrOljehål & Oljespår").Trim();
        }

        private static void SumFrequenciesOP2(Dictionary<string, string> kv, string maskinVal)
        {
            string v = FreqOP2(maskinVal);
            for (int i = 1; i <= 9; i++) kv["SumF2_" + i] = v;
            kv["SumF2_0"] = v;
            kv["SumF2_11"] = v;
        }

        private static string FreqOP2(string mv)
        {
            if (EqualsI(mv, "Skepp6")) return "1/1";
            if (EqualsI(mv, "Dubbelparet")) return "1/10";
            if (EqualsI(mv, "K&T") || EqualsI(mv, "VTR-160") || EqualsI(mv, "MacTurn 550")) return "1/2";
            return "";
        }

        private static void SumMeasuringDevicesOP2(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsInGroup(maskinVal, MachinesOP2);
            bool isSkepp = EqualsI(maskinVal, "Skepp6");
            kv["SumD2_1"] = isSkepp ? "Skala på borrmaskin" : (m ? "pipborr/djupmått" : "");
            kv["SumD2_2"] = m ? "Skjutmått" : "";
            kv["SumD2_3"] = m ? "Skjutmått" : "";
            kv["SumD2_4"] = m ? "Gängtolk" : "";
            kv["SumD2_5"] = m ? "Skjutmått" : "";
            kv["SumD2_6"] = m ? "Skjutmått/fasmall" : "";
            kv["SumD2_7"] = m ? "Skjutmått" : "";
            kv["SumD2_8"] = m ? "Skjutmått" : "";
            kv["SumD2_9"] = isSkepp ? "Höjdrits" : (m ? "pipborr/djupmått" : "");
            kv["SumD2_0"] = m ? "Skjutmått" : "";
            kv["SumD2_11"] = m ? "Radieyra" : "";
        }

        private static void SumAFOP2(Dictionary<string, string> kv, string maskinVal)
        {
            for (int i = 1; i <= 9; i++) kv["SumAF2_" + i] = "";
            kv["SumAF2_0"] = "";
            kv["SumAF2_11"] = "";
        }

        private static void SumMaskinValSlits(Dictionary<string, string> kv, string maskinVal)
        {
            string s = EqualsI(maskinVal, "Skepp6") ? "Skepp6" :
                       EqualsI(maskinVal, "K&T") ? "K&T" :
                       EqualsI(maskinVal, "VTR-160") ? "VTR-160" :
                       EqualsI(maskinVal, "MacTurn 550") ? "MacTurn 550" : "";
            kv["SumMaskinValS5"] = ("Maskin: " + s + " - Muttersäkring, Slits").Trim();
        }

        private static void SumFrequenciesSlits(Dictionary<string, string> kv, string maskinVal)
        {
            bool isSkepp = EqualsI(maskinVal, "Skepp6");
            bool isOther = EqualsI(maskinVal, "K&T") || EqualsI(maskinVal, "VTR-160") || EqualsI(maskinVal, "MacTurn 550");
            string v = isSkepp ? "1/1" : isOther ? "1/2" : "";
            kv["SumF1_1"] = v;
            kv["SumF1_2"] = v;
            kv["SumF1_3"] = v;
            kv["SumF1_4"] = v;
            kv["SumF1_5"] = v;
        }

        private static void SumMeasuringDevicesSlits(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsInGroup(maskinVal, MachinesSlits);
            kv["SumD1_1"] = m ? "Skjutmått" : "";
            kv["SumD1_2"] = m ? "Skjutmått" : "";
            kv["SumD1_3"] = m ? "Skjutmått" : "";
            kv["SumD1_4"] = "";
            kv["SumD1_5"] = m ? "Skjutmått" : "";
        }

        private static void SumAFSlits(Dictionary<string, string> kv, string maskinVal)
        {
            kv["SumAF1_1"] = "";
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = "";
        }

        private static int GetTypIndex(int serie, string typ)
        {
            string[] list = serie == 30 ? Typ30 : serie == 31 ? Typ31 : serie == 23 ? Typ23 : serie == 32 ? Typ32 : serie == 39 ? Typ39 : null;
            if (list == null) return -1;
            for (int i = 0; i < list.Length; i++)
                if (string.Equals(list[i], typ, StringComparison.OrdinalIgnoreCase)) return i;
            return -1;
        }

        private static double GetTableValue(int serie, int idx, double[] t30, double[] t31, double[] t23, double[] t32, double[] t39)
        {
            double[] tab = serie == 30 ? t30 : serie == 31 ? t31 : serie == 23 ? t23 : serie == 32 ? t32 : serie == 39 ? t39 : null;
            if (tab == null || idx < 0 || idx >= tab.Length) return 0;
            return tab[idx];
        }

        private static double GetH(int serie, double typNum, string typStr)
        {
            if (serie == 31)
            {
                if (typNum < 37) return 0.8;
                if (typNum < 65) return 1.0;
                if (typNum < 85 || EqualsAnyTyp(typStr, "355,6")) return 1.2;
                if (typNum < 601) return 1.5;
                if (typNum < 751) return 2.0;
                return 2.8;
            }
            if (serie == 23)
            {
                if (typNum < 37) return 0.8;
                if (typNum < 65) return 1.0;
                return 1.2;
            }
            if (serie == 39)
            {
                if (typNum < 37) return 0.8;
                if (typNum < 65) return 1.0;
                if (typNum < 85) return 1.2;
                if (typNum < 631) return 1.5;
                if (typNum < 751) return 2.0;
                return 2.8;
            }
            if (typNum < 37) return 0.8;
            if (typNum < 65) return 1.0;
            if (typNum < 85 || EqualsAnyTyp(typStr, "378")) return 1.2;
            if (typNum < 751) return 1.5;
            return 2.0;
        }

        private static string GenTolPM(double v)
        {
            if (v < 6.1) return " 0.1";
            if (v < 30.1) return " 0.2";
            if (v < 120.1) return " 0.3";
            if (v < 315.1) return " 0.5";
            if (v < 1000.1) return " 0.8";
            if (v < 2000.1) return " 1.2";
            return " 2.0";
        }

        private static double GenTol(double v)
        {
            if (v < 6.01) return 0.1;
            if (v < 30.01) return 0.2;
            if (v < 120.01) return 0.3;
            if (v < 400.01) return 0.5;
            if (v < 1000.01) return 0.8;
            if (v < 2000.01) return 1.2;
            return 2.0;
        }

        private static double LTolN(double l)
        {
            if (l < 3.01) return 0.400;
            if (l < 6.01) return 0.480;
            if (l < 10.01) return 0.580;
            if (l < 18.01) return 0.700;
            if (l < 30.01) return 0.840;
            if (l < 50.01) return 1.000;
            if (l < 80.01) return 1.200;
            if (l < 120.01) return 1.400;
            if (l < 180.01) return 1.600;
            if (l < 250.01) return 1.850;
            if (l < 315.01) return 2.100;
            if (l < 400.01) return 2.300;
            if (l < 500.01) return 2.500;
            if (l < 630.01) return 2.800;
            if (l < 800.01) return 3.200;
            if (l < 1000.01) return 3.600;
            if (l < 1250.01) return 4.200;
            if (l < 1600.01) return 5.000;
            if (l < 2000.01) return 6.000;
            if (l < 2500.01) return 7.000;
            return 8.600;
        }

        private static double bTolPos(double b)
        {
            if (b < 11) return 1.5;
            if (b < 19) return 1.8;
            if (b < 31) return 2.1;
            if (b < 51) return 2.5;
            if (b < 81) return 3.0;
            if (b < 121) return 3.5;
            return 4.0;
        }

        private static double d1Tol(double d)
        {
            if (d < 3.01) return 0.012;
            if (d < 6.01) return 0.015;
            if (d < 10.01) return 0.018;
            if (d < 18.01) return 0.021;
            if (d < 30.01) return 0.026;
            if (d < 50.01) return 0.031;
            if (d < 80.01) return 0.037;
            if (d < 120.01) return 0.043;
            if (d < 180.01) return 0.050;
            if (d < 250.01) return 0.057;
            if (d < 315.01) return 0.065;
            if (d < 400.01) return 0.070;
            if (d < 500.01) return 0.077;
            if (d < 630.01) return 0.087;
            if (d < 800.01) return 0.100;
            if (d < 1000.01) return 0.115;
            if (d < 1250.01) return 0.130;
            if (d < 1600.01) return 0.155;
            if (d < 2000.01) return 0.185;
            if (d < 2500.01) return 0.220;
            return 0.270;
        }

        private static double dOP1TolN(int stmm)
        {
            if (stmm == 4) return 0.300;
            if (stmm == 5) return 0.335;
            if (stmm == 6) return 0.375;
            if (stmm == 7) return 0.425;
            return 0.450;
        }

        private static double dmTolPos(int stmm)
        {
            if (stmm == 4) return 0.190;
            if (stmm == 5) return 0.212;
            if (stmm == 6) return 0.236;
            if (stmm == 7) return 0.250;
            return 0.265;
        }

        private static double dmTolN(int stmm)
        {
            if (stmm == 4) return 0.630;
            if (stmm == 5) return 0.710;
            if (stmm == 6) return 0.800;
            if (stmm == 7) return 0.850;
            return 0.950;
        }

        private static double d3TolN(int stmm)
        {
            if (stmm == 4) return 0.750;
            if (stmm == 5) return 0.850;
            if (stmm == 6) return 0.950;
            if (stmm == 7) return 1.000;
            return 1.120;
        }

        private static double GTjTolPos(double kona, double d)
        {
            if (Math.Abs(kona - 12) < 0.001)
            {
                if (d > 1000) return 0.095; if (d > 800) return 0.085; if (d > 630) return 0.075;
                if (d > 500) return 0.070; if (d > 400) return 0.065; if (d > 315) return 0.060;
                if (d > 250) return 0.055; if (d > 180) return 0.050; if (d > 120) return 0.040;
                if (d > 80) return 0.035; if (d > 50) return 0.030; if (d > 30) return 0.025;
                return 0.020;
            }
            if (Math.Abs(kona - 30) < 0.001)
            {
                if (d > 1000) return 0.060; if (d > 800) return 0.055; if (d > 630) return 0.050;
                if (d > 500) return 0.045; if (d > 400) return 0.040; if (d > 315) return 0.035;
                if (d > 250) return 0.035; if (d > 180) return 0.030; if (d > 120) return 0.025;
                if (d > 80) return 0.022; if (d > 50) return 0.019; if (d > 30) return 0.016;
                return 0.013;
            }
            return 0;
        }

        private static double GTjTolNeg(double kona, double d)
        {
            if (Math.Abs(kona - 12) < 0.001)
            {
                if (d > 1000) return 0.280; if (d > 800) return 0.250; if (d > 630) return 0.225;
                if (d > 500) return 0.200; if (d > 400) return 0.190; if (d > 315) return 0.175;
                if (d > 250) return 0.160; if (d > 180) return 0.140; if (d > 120) return 0.120;
                if (d > 80) return 0.105; if (d > 50) return 0.090; if (d > 30) return 0.075;
                return 0.070;
            }
            if (Math.Abs(kona - 30) < 0.001)
            {
                if (d > 1000) return 0.170; if (d > 800) return 0.155; if (d > 630) return 0.140;
                if (d > 500) return 0.125; if (d > 400) return 0.115; if (d > 315) return 0.105;
                if (d > 251) return 0.095; if (d > 180) return 0.085; if (d > 120) return 0.075;
                if (d > 80) return 0.065; if (d > 50) return 0.055; if (d > 30) return 0.046;
                return 0.039;
            }
            return 0;
        }

        private static double GVarTol(double d)
        {
            if (d > 1000) return 0.050; if (d > 800) return 0.045; if (d > 630) return 0.040;
            if (d > 500) return 0.035; if (d > 315) return 0.030; if (d > 250) return 0.025;
            if (d > 180) return 0.020; if (d > 120) return 0.015; if (d > 50) return 0.010;
            return 0.008;
        }

        private static double RakA(double d)
        {
            int val = d < 101 ? 8 : d < 281 ? 10 : d < 481 ? 12 : d < 601 ? 14 : d < 901 ? 16 : 20;
            return val / 1000.0;
        }

        private static double RakB(double d)
        {
            int val = d < 101 ? 12 : d < 281 ? 15 : d < 481 ? 18 : d < 601 ? 21 : d < 901 ? 24 : 30;
            return val / 1000.0;
        }

        private static double Orund(double d1)
        {
            if (d1 < 31) return 0.026; if (d1 < 51) return 0.031; if (d1 < 81) return 0.037;
            if (d1 < 121) return 0.043; if (d1 < 181) return 0.050; if (d1 < 251) return 0.057;
            if (d1 < 316) return 0.065; if (d1 < 401) return 0.070; if (d1 < 501) return 0.077;
            if (d1 < 631) return 0.087; if (d1 < 801) return 0.100; if (d1 < 1001) return 0.115;
            return 0.130;
        }

        private static double VinkTol(double d, int vml)
        {
            double factor = d > 1000 ? 0.09 : d > 800 ? 0.10 : d > 630 ? 0.11 : d > 500 ? 0.12 :
                            d > 400 ? 0.13 : d > 180 ? 0.15 : d > 150 ? 0.18 : d > 120 ? 0.30 :
                            d > 80 ? 0.45 : d > 50 ? 0.50 : 0.60;
            return Math.Round((factor * vml) / 1000.0, 6);
        }

        private static string d10TolPos(double typNum)
        {
            if (typNum < 65) return "+ 0.210";
            if (typNum < 85) return "+ 0.360";
            if (typNum < 531) return "+ 0.400";
            if (typNum < 671) return "+ 0.440";
            if (typNum < 851) return "+ 0.500";
            return "+ 0.560";
        }

        private static string d10TolNeg(double typNum)
        {
            if (typNum < 65) return "- 0.320";
            if (typNum < 85) return "- 0.570";
            if (typNum < 531) return "- 0.630";
            if (typNum < 671) return "- 0.700";
            if (typNum < 851) return "- 0.800";
            return "- 0.900";
        }

        private static string FmtD(double v)
        {
            return v.ToString(CultureInfo.InvariantCulture).Replace(",", ".");
        }

        private static string Fmt3(double v)
        {
            return v.ToString("F3", CultureInfo.InvariantCulture);
        }

        private static int TryParseInt(string s)
        {
            int v;
            return int.TryParse((s ?? "").Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static double TryParseDouble(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;
            s = s.Trim().Replace(",", ".");
            double v;
            return double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static double GetDouble(List<Bookmark> bm, string key)
        {
            string raw = GetString(bm, key);
            if (string.IsNullOrWhiteSpace(raw)) return 0;
            raw = raw.Trim().Replace(",", ".");
            double v;
            return double.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
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

        private static bool EqualsI(string a, string b)
            => string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);

        private static bool EqualsAnyTyp(string typ, params string[] values)
        {
            foreach (string v in values)
                if (string.Equals(typ, v, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        private static bool IsInGroup(string maskinVal, string[] group)
        {
            if (string.IsNullOrEmpty(maskinVal)) return false;
            for (int i = 0; i < group.Length; i++)
                if (string.Equals(group[i], maskinVal, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }
    }
}