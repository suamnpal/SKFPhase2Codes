using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Klamhylsor_40_60 : ITemplateCalculations
    {
        private const string LB = "<<LineBreak>>";

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();
            var bm = req?.Bookmarks;
            string subject = req?.ProductDesignation ?? "";
            string mv = req?.MachineNumber ?? "";
            kv["VaLPopUp"] = ComputePopup(req?.Published);

            string tmpFormat = subject.Trim().ToUpperInvariant();
            string tmpBet = tmpFormat.Replace(".", ",");
            string[] parts = Split(tmpBet, "X* /.-");
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
            string tmpBet1Text = LotusText(tmpBet1);
            string tmpBet2Text = LotusText(tmpBet2);
            string tmpBet3Text = LotusText(tmpBet3);
            string tmpBet4Text = LotusText(tmpBet4);

            bool tmpSlash = tmpBet.Contains("/");
            bool tmpH = ContainsI(tmpBet3Text + " | " + tmpBet4Text, "H");
            bool tmpDFlag = ContainsI(tmpBet, "D");
            bool tmpHD = ContainsI(tmpBet, "HD");
            bool tmpHB = ContainsI(tmpBet, "HB");
            bool tmpHTL = ContainsI(tmpBet, "HTL");
            bool tmpTum = ContainsI(tmpBet1Text, "SNW") || ContainsI(tmpBet1Text, "SNP");
            string tmpArt = tmpBet1Text == "SNW" || tmpBet1Text == "SNP" ? "TUM" : "MM";
            bool tmpOH = ContainsI(tmpBet, "OH");
            int tmpCountB = tmpBet.Length;
            int tmpCountB2 = tmpBet2Text.Length;
            int tmpArtLista = MemberText(tmpBet1Text, new[] { "H", "OH", "HA", "HE", "MA", "SNW", "SNP" });
            string tmpSerie = tmpSlash && (tmpCountB2 == 3 || tmpCountB2 == 2) ? tmpBet2Text : tmpCountB2 > 4 ? 
                Left(tmpBet2Text, 3) : tmpCountB2 == 3 ? Left(tmpBet2Text, 1) : Left(tmpBet2Text, 2);
            string tmpTyp = Right(tmpBet2Text, 2);
            double tmpTypNumber = ParseDouble(tmpTyp);
            double tmpKona = tmpSerie == "240" || tmpSerie == "241" ? 30 : 12;
            kv["SumKona"] = "Kona 1:" + Smart(tmpKona);

            double tmpd = GetDouble(bm, "YDia");
            double tmpL = GetDouble(bm, "Längd");
            double tmpd1 = GetDouble(bm, "IDia");
            double tmpATS = GetDouble(bm, "Antal Tungspår");
            double tudelad = GetDouble(bm, "Tudelad");
            double tmpb = GetDouble(bm, "Gänglängd");
            double gangsvarvlangdSNW = GetDouble(bm, "GängsvarvlängdSNW");
            string radieSNW = GetString(bm, "RadieSNW");
            double tungspArsbredd = GetDouble(bm, "Tungspårsbredd");
            double tungspArsbreddSNW = GetDouble(bm, "TungspårsbreddSNW");
            double tungspArslangd = GetDouble(bm, "Tungspårslängd");
            double tungspArslangdSNW = GetDouble(bm, "TungspårslängdSNW");
            string fasSNW = GetString(bm, "FasSNW");
            double tmpBo = GetDouble(bm, "Längd till oljespår");
            string pRitning = GetString(bm, "PRitning");
            double antalBorrhal = GetDouble(bm, "Antal Borrhål");
            double borrningFranStorande = GetDouble(bm, "Borrning från storände");
            double aMatt = GetDouble(bm, "Amått");
            string kilnr = GetString(bm, "Kilnr");
            string kilinstallning = GetString(bm, "Kilinställning");
            double passbit = GetDouble(bm, "Passbit");

            double tmpStmm = tmpd < 25 ? 1.0 : tmpd < 55 ? 1.5 : tmpd < 155 ? 2.0 : tmpd < 205 ? 3.0 : tmpd < 305 ? 4.0 : 5.0;
            double tmpSttum = tmpd < 70 ? 18 : tmpd < 150 ? 12 : tmpd < 220 ? 8 : 6;

            kv["Sumd"] = "(d) " + SmartComma(tmpd);
            kv["SumdTol"] = tmpArt == "MM" ? tmpStmm == 5.0 ? "- 0 " : tmpStmm == 4.0 ? "- 0" : tmpStmm == 3.0 ? 
                "- 0.048" : tmpStmm == 2.0 ? "- 0.038" : tmpStmm == 1.5 ? "- 0.032" : "- 0.026" : tmpArt == "TUM" ? "- 0" : "Fel Typ";
            kv["SumdTolN"] = tmpArt == "MM" ? tmpStmm == 5.0 ? "- 0.335" : tmpStmm == 4.0 ? "- 0.300" : tmpStmm == 3.0 ? 
                "- 0.423" : tmpStmm == 2.0 ? "- 0.318" : tmpStmm == 1.5 ? "- 0.268" : "- 0.206" : tmpArt == "TUM" ? 
                tmpSttum == 18 ? " - 0.208" : tmpSttum == 12 ? " - 0.285" : tmpSttum == 8 ? " - 0.386" : "- 0.513" : "Fel Typ";

            kv["Sumd1"] = "(d1) " + SmartComma(tmpd1);
            kv["Sumd1Tol"] = (tmpArt == "MM" ? tmpd1 > 250 ? "± 0.065" : tmpd1 > 180 ? "± 0.057" : tmpd1 > 120 ? 
                "± 0.050" : tmpd1 > 80 ? "± 0.043" : tmpd1 > 50 ? "± 0.037" : tmpd1 > 30 ? "± 0.031" : "± 0.026" 
                : tmpArt == "TUM" ? "+ 0.102" : "Fel Typ") + " [3F]";
            kv["Sumd1TolN"] = tmpArt == "MM" ? "" : "- 0 [3F]";

            kv["Sumd1a"] = tmpArt == "MM" ? "Toleranser (d1) efter slits" : "";
            string tmpd1aTol = tmpd > 319 ? "+ 0.360" : tmpd > 259 ? "+ 0.210" : tmpd > 201 ? "+ 0.185" : tmpd > 131 ? 
                "+ 0.100" : tmpd > 91 ? "+ 0.087" : tmpd > 56 ? "+ 0.074" : "+ 0.062";
            string tmpd1aTolN = tmpd > 319 ? "- 0.570" : tmpd > 259 ? "- 0.320" : tmpd > 201 ? "- 0.290" : tmpd > 131 ? 
                "- 0.250" : tmpd > 91 ? "- 0.220" : tmpd > 56 ? "- 0.120" : "- 0.100";
            kv["Sumd1aTol"] = tmpArt == "MM" ? tmpd1aTol + " [3F]" : "";
            kv["Sumd1aTolN"] = tmpArt == "MM" ? tmpd1aTolN + " [3F]" : "";

            kv["SumATS"] = SmartComma(tmpATS) + " st. tungspår";
            double tmpML = LotusRound(tmpL - 62, 5);
            double tmpVT = tmpArt == "MM" ? tmpd > 180 ? 0.15 : tmpd > 150 ? 0.18 : tmpd > 120 ? 0.30 : tmpd > 80 ? 
                0.45 : tmpd > 50 ? 0.50 : 0.60 : tmpArt == "TUM" ? tmpd > 205 ? 0.15 : tmpd > 185 ? 0.25 : tmpd > 125 ? 
                0.30 : tmpd > 85 ? 0.45 : 0.50 : 0;

            double tmpGbettum = tmpd < 100 ? (tmpd / 25.4) + 0.001 : tmpd < 150 ? (tmpd / 25.4) + 0.002 : tmpd < 260 ? 
                (tmpd / 25.4) + 0.003 : (tmpd / 25.4) + 0.004;
            string sumGangbet2 = tmpStmm < 4 ? "M" : "Tr";
            kv["SumGängbet2"] = sumGangbet2;
            kv["SumGängbet"] = tmpArt == "MM" ? sumGangbet2 + SmartComma(tmpd) : tmpArt == "TUM" ? SmartComma(LotusRound(tmpGbettum, 0.001)) : "";
            kv["SumStigning"] = tmpArt == "MM" ? F1Comma(tmpStmm) : Smart(tmpSttum) + "UN";
            kv["SumP"] = "(P) " + (tmpArt == "MM" ? F1Comma(tmpStmm) : Smart(tmpSttum)); 
            string tmpRullar = tmpArt == "MM" ? sumGangbet2 + kv["SumStigning"] : kv["SumStigning"];

            kv["SumGTLTol"] = (tmpArt == "MM" ? tmpKona == 12 ? tmpd > 251 ? "+ 0.055" : tmpd > 180 ? "+ 0.050" 
                : tmpd > 120 ? "+ 0.040" : tmpd > 80 ? "+ 0.035" : tmpd > 50 ? "+ 0.030" : tmpd > 30 ? "+ 0.025" : "+ 0.020" 
                : tmpKona == 30 ? tmpd > 250 ? "+ 0.035" : tmpd > 180 ? "+ 0.030" : tmpd > 120 ? "+ 0.025" 
                : tmpd > 80 ? "+ 0.022" : tmpd > 50 ? "+ 0.019" : tmpd > 30 ? "+ 0.016" : "+ 0.013 " : "Fel Kona" 
                : tmpArt == "TUM" ? "+ 0.025" : "Fel Typ") + " [3F]";
            kv["SumGTLTolN"] = (tmpArt == "MM" ? tmpKona == 12 ? tmpd > 251 ? "- 0.160" : tmpd > 180 ? "- 0.140" 
                : tmpd > 120 ? "- 0.120" : tmpd > 80 ? "- 0.105" : tmpd > 50 ? "- 0.090" : tmpd > 30 ? "- 0.075" : "- 0.070" 
                : tmpKona == 30 ? tmpd > 251 ? "- 0.095" : tmpd > 180 ? "- 0.085" : tmpd > 120 ? "- 0.075" 
                : tmpd > 80 ? "- 0.065" : tmpd > 50 ? "- 0.055" : tmpd > 30 ? "- 0.046" : "- 0.039" : "Fel Kona" 
                : tmpArt == "TUM" ? "- 0.075" : "Fel Typ") + " [2F]";
            string tmpVe = tmpArt == "MM" ? tmpd > 250 ? "0.025" : tmpd > 180 ? "0.020" : tmpd > 120 ? "0.015" 
                : tmpd > 50 ? "0.010" : "0.008" : tmpArt == "TUM" ? "0.025" : "Fel Typ";
            kv["SumVe"] = "Max: " + tmpVe + " [2F]";

            double sumdm = tmpArt == "MM" ? tmpStmm == 1 ? tmpd - 0.650 : tmpStmm == 1.5 ? tmpd - 0.974 : tmpStmm == 2 ?
                tmpd - 1.299 : tmpStmm == 3 ? tmpd - 1.949 : tmpStmm == 4 ? tmpd - 2.000 : tmpStmm == 5 ? tmpd - 2.500 
                : tmpd - 3.000 : tmpArt == "TUM" ? tmpSttum == 18 ? tmpd - 0.917 : tmpSttum == 12 ? tmpd - 1.374 
                : tmpSttum == 8 ? tmpd - 2.062 : tmpd - 2.751 : 0;
            kv["Sumdm"] = tmpArt == "MM" || tmpArt == "TUM" ? SmartComma(sumdm) : "Fel Typ";
            kv["SumdmTol"] = (tmpArt == "MM" ? tmpStmm == 5 ? "- 0.212" : tmpStmm == 4 ? "- 0.190" : tmpStmm == 3 ? 
                "- 0.048" : tmpStmm == 2 ? "- 0.038" : tmpStmm == 1.5 ? "- 0.032" : "- 0.026" : tmpArt == "TUM" ? "- 0" 
                : "Fel Typ") + " [3F]";
            kv["SumdmTolN"] = (tmpArt == "MM" ? tmpStmm == 5 ? "- 0710" : tmpStmm == 4 ? "- 0.630" : tmpStmm == 3 ? 
                tmpd > 180 ? "- 0.363" : "- 0.328" : tmpStmm == 2 ? tmpd > 90 ? "- 0.274" : "- 0.262" : tmpStmm == 1.5 ? 
                tmpd > 45 ? "- 0.232" : "- 0.222" : "- 0.176" : tmpArt == "TUM" ? tmpSttum == 18 ? tmpd > 50 ? "- 0.130" 
                : tmpd > 35 ? "- 0.114" : "- 0.102" : tmpSttum == 12 ? tmpd > 122 ? "- 0.210" : tmpd > 120 ? "- 0.170" 
                : tmpd > 100 ? "- 0.210" : tmpd > 85 ? "- 0.188" : tmpd > 75 ? "- 0.150" : "- 0.137" : tmpSttum == 8 ? 
                tmpd > 210 ? "- 0.307" : tmpd > 200 ? "- 0.249" : tmpd > 197 ? "- 0.290" : "- 0.231" : tmpSttum == 6 ? 
                tmpd > 305 ? "- 0.343" : tmpd > 300 ? "- 0.264" : tmpd > 240 ? "- 0.330" : tmpd > 220 ? "- 0.315" 
                : tmpd > 210 ? "- 0.307" : tmpd > 200 ? "- 0.249" : tmpd > 197 ? "- 0.290" : "- 0.231" : "" : "Fel Typ") + " [3F]";

            kv["SumL"] = "(L) " + SmartComma(tmpL);
            kv["SumLTol"] = tmpArt == "MM" ? "+ 0 " : "";
            kv["SumLTolN"] = (tmpArt == "MM" ? tudelad == 0 ? tmpL > 400 ? "- 2.500" : tmpL > 315 ? "- 2.300" 
                : tmpL > 250 ? "- 2.100" : tmpL > 180 ? "- 1.850" : tmpL > 120 ? "- 1.600" : tmpL > 80 ? "- 1.400"
                : tmpL > 50 ? "- 1.200" : tmpL > 30 ? "- 1.000" : tmpL > 18 ? "- 0.840" : tmpL > 10 ? "- 0.700" 
                : "- 0.580" : tudelad == 1 ? tmpL > 630 ? "- 0.800" : tmpL > 500 ? "- 0.700" : tmpL > 400 ? "- 0.630" 
                : tmpL > 315 ? "- 0.570" : tmpL > 250 ? "- 0.520" : tmpL > 180 ? "- 0.460" : tmpL > 120 ? "- 0.400" 
                : tmpL > 80 ? "- 0.350" : tmpL > 50 ? "- 0.300" : tmpL > 30 ? "- 0.250" : "- 0.210" : "Fel Tudelad" 
                : tmpArt == "TUM" ? "± 0.254" : "Fel Typ") + " [3F]";

            double tmpRakA = (tmpd < 101 ? 8 : tmpd < 281 ? 10 : tmpd < 481 ? 12 : tmpd < 601 ? 14 : tmpd < 901 ? 16 : 20) / 1000.0;
            double tmpRakB = (tmpd < 101 ? 12 : tmpd < 281 ? 15 : tmpd < 481 ? 18 : tmpd < 601 ? 21 : tmpd < 901 ? 24 : 30) / 1000.0;
            kv["SumRakA"] = SmartComma(tmpRakA);
            kv["SumRakB"] = SmartComma(tmpRakB);

            kv["Sumb"] = "(b) " + SmartComma(tmpb);
            string tmpbTol = tmpArt == "MM" ? tmpb > 120 ? "+ 4.0" : tmpb > 80 ? "+ 3.5" : tmpb > 50 ? "+ 3.0" 
                : tmpb > 30 ? "+ 2.5" : tmpb > 18 ? "+ 2.1" : tmpb > 10 ? "+ 1.8" : "+ 1.5" : "";
            string tmpbTolN = tmpArt == "MM" ? "- 0 [2F]" : tmpArt == "TUM" ? "(min)" : "Fel Typ";
            kv["SumbTol"] = tmpArt == "MM" ? tmpbTol + " [3F]" : "";
            kv["SumbTolN"] = tmpbTolN;

            kv["SumGVL"] = tmpArt == "TUM" ? "Gängsvarvlängd: " + SmartComma(gangsvarvlangdSNW) + "  ± 0.25" : "";
            kv["SumR1"] = tmpArt == "MM" ? tmpd < 69 ? "Radie 0.5" : "Radie 1" : radieSNW;
            kv["SumR2"] = radieSNW == "0" ? tmpd < 69 ? "Radie 0.5" : tmpd < 109 ? "Radie 1" : tmpd < 159 ? 
                "Radie 1.5" : tmpd < 219 ? "Radie 2" : "Radie 2.5" : radieSNW;

            double tmpe = tungspArsbredd == 0 ? tmpArt == "MM" ? tmpATS == 1 ? tmpd < 54 ? 7 : tmpd < 79 ? 9 
                : tmpd < 99 ? 11 : tmpd < 119 ? 13 : tmpd < 139 ? 15 : tmpd < 159 ? 17 : tmpd < 179 ? 19 
                : tmpd < 219 ? 21 : tmpd < 259 ? 25 : 29 : tmpd > 279 ? 24 : 20 : tungspArsbreddSNW : tungspArsbredd;
            kv["Sume"] = "(e) " + SmartComma(tmpe);
            kv["SumeTol"] = tmpArt == "MM" ? tmpe > 18 ? "+ 0.520" : tmpe > 10 ? "+ 0.430" : tmpe > 6 ? "+ 0.360" : "+ 0.300" : tmpArt == "TUM" ? "± 0.254" : "Fel Typ";
            kv["SumeTolN"] = tmpArt == "MM" ? "- 0  [3F]" : "";

            double tmpf = tungspArslangd == 0 ? tmpArt == "MM" ? tmpATS == 1 ? tmpd < 54 ? 20 : tmpd < 64 ? 21 
                : tmpd < 69 ? 22 : tmpd < 74 ? 24 : tmpd < 79 ? 25 : tmpd < 84 ? 27 : tmpd < 94 ? 29 : tmpd < 99 ?
                30 : tmpd < 109 ? 31 : tmpd < 119 ? 32 : tmpd < 129 ? 34 : tmpd < 139 ? 36 : tmpd < 149 ? 37 
                : tmpd < 159 ? 39 : tmpd < 169 ? 42 : tmpd < 179 ? 43 : tmpd < 189 ? 44 : tmpd < 199 ? 46 
                : tmpd < 219 ? 47 : tmpd < 239 ? 51 : tmpd < 259 ? 53 : tmpd < 279 ? 56 : 58 : tmpd > 339 ? 26 
                : tmpd > 319 ? 25 : tmpd > 259 ? 22 : 21 : tungspArslangdSNW : tungspArslangd;
            double tmpfMinus6 = tmpATS == 1 ? tmpf - 6 : tmpf;
            kv["Sumf"] = "(f) " + SmartComma(tmpfMinus6);
            kv["SumfTol"] = tmpArt == "MM" ? tmpf > 50 ? "+ 3.0" : tmpf > 30 ? "+ 2.5" : tmpf > 19 ? "+ 2.1" 
                : "+ 1.8" : tmpArt == "SNW" ? "+ 2.1" : "+ 0.51";
            kv["SumfTolN"] = "- 0 [3F]";

            double tmpc = tmpd1 > 95 ? 4 : 3;
            kv["Sumc"] = "(c) " + SmartComma(tmpc);
            kv["SumFas"] = tmpArt == "MM" ? tmpd < 24 ? "0.7x45°" : tmpd < 69 ? "1.1x45°" : tmpd < 159 ? 
                "1.8x45°" : tmpd < 219 ? "2.4x45°" : "2.7x45°" : tmpArt == "TUM" ? fasSNW + "x45°" : "Fel Typ";

            string tmpRd = kv["Sumd1Tol"].Replace("±", "max: ");
            double tmpKonavv = tmpML * tmpVT / 1000;
            kv["SumKonavv"] = "max " + F3Comma(tmpKonavv) + " [2F]";
            kv["SumMätlängd"] = SmartComma(tmpML);

            kv["SumBo"] = "(B) " + SmartComma(tmpBo);
            kv["SumBoTol"] = tmpBo < 6.01 ? "± 0.1" : tmpBo < 30.01 ? "± 0.2" : tmpBo < 120.01 ? "± 0.3" 
                : tmpBo < 400.01 ? "± 0.5" : tmpBo < 1000.01 ? "± 0.8" : tmpBo < 2000.01 ? "± 1.2" : "± 2.0";

            double tmpGFH = 3;
            kv["SumGFH"] = pRitning == "OH24048/8.1/2" ? "" : tmpOH ? "Oljegenomföringshål till oljespår " + 
                Smart(tmpGFH) + "mm" : "";

            double tmpBOS = tmpOH ? tmpd1 < 165 ? 4 : 5 : 0;
            kv["SumOinvändigt"] = antalBorrhal == 2 ? " OBS: " + LB + "Oljespår även invändigt" : "";
            kv["SumStorända"] = borrningFranStorande == 1 || borrningFranStorande == 2 ? "OBS: Borras från storänden" : "";
            kv["SumSlitsbredd"] = tmpOH ? tmpd1 < 170 ? "Oljespår 4 mm med lucka kring slitsen:" 
                : "Oljespår 5 mm med lucka kring slitsen: " + (tmpd > 290 ? "62" : tmpd > 270 ? "58" 
                : tmpd > 250 ? "54" : tmpd > 230 ? "50" : "46") : "";
            kv["SumLucka"] = tmpOH ? "Oljespår: " + Smart(tmpBOS) + LB + "Lucka kring slits: " + LB + 
                (tmpd > 290 ? "62" : tmpd > 270 ? "58" : tmpd > 250 ? "54" : tmpd > 230 ? "50" : "46") + 
                (tmpd > 270 ? " -15" : tmpd > 230 ? " -12" : " -11") : "";

            double tmpBLangd = borrningFranStorande == 1 || borrningFranStorande == 2 ? tmpL - tmpBo + 3 : tmpBo + 5;
            double tmpBDia = borrningFranStorande == 1 || borrningFranStorande == 2 ? tmpd1 + 15 : tmpd1 + 8.3;
            string tmpBorrvinkel = borrningFranStorande == 1 ? " borras i 2°," : borrningFranStorande == 2 ? " borras i 0°," : "";
            string tmpBorrplacering = antalBorrhal == 1 ? " 180° ifrån slits" : " 135° & 225° ifrån slits";
            string tmpBorrdiameter = borrningFranStorande == 1 ? " 4mm, " : " 3mm, ";
            string tmpBorrgang = borrningFranStorande == 1 ? "G1/8 Borrlängd 12mm Gänglängd min 10mm" : "M6 Borrlängd 10mm, Gänglängd min 8mm";
            kv["SumBorrhål"] = antalBorrhal == 1 || antalBorrhal == 2 ? Smart(antalBorrhal) + " borrhål," + tmpBorrvinkel + tmpBorrplacering + LB + "Ø borrhål:" + tmpBorrdiameter + LB + "Borrlängd: " + Smart(tmpBLangd) + " mm" + LB + tmpBorrgang : "";
            double tmpTumYDia = LotusRound(tmpd, 10);
            double tmpL1 = 50 + (tmpb > 46 ? 5 : 0);
            double tmpL2 = tmpL - tmpL1 > 54 ? 75 : 50;
            bool tmpIsSnpSnw = tmpBet1Text == "SNP" || tmpBet1Text == "SNW";
            double tmpBaseDiameter = tmpIsSnpSnw ? tmpTumYDia : tmpd;
            double tmpE1Raw = ((tmpL1 - aMatt) / tmpKona + tmpBaseDiameter - tmpd1) / 2;
            double tmpE2Raw = ((tmpL1 + (tmpL - tmpL1 > 54 ? 75 : 50) - aMatt) / tmpKona + tmpBaseDiameter - tmpd1) / 2;
            double tmpE1 = tmpE1Raw - Math.Truncate(100 * tmpE1Raw) / 100;
            double tmpE2 = tmpE2Raw - Math.Truncate(100 * tmpE2Raw) / 100;
            double tmpSkillnad = Math.Abs(tmpE2 - tmpE1);
            double tmpSumE1Base = ((tmpL1 - aMatt) / tmpKona + tmpBaseDiameter - tmpd1) / 2;
            double tmpSumE2Base = ((tmpL1 + tmpL2 - aMatt) / tmpKona + tmpBaseDiameter - tmpd1) / 2;
            double sumE1Value = tmpSkillnad < 0.005 ? tmpE1 + tmpE2 < 0.01 ? Math.Truncate(100 * tmpSumE1Base) 
                / 100 : Math.Truncate(1 + 100 * tmpSumE1Base) / 100 : tmpSumE1Base;
            double sumE2Value = tmpSkillnad < 0.005 ? tmpE1 + tmpE2 < 0.01 ? Math.Truncate(100 * tmpSumE2Base) 
                / 100 : Math.Truncate(1 + 100 * tmpSumE2Base) / 100 : tmpSumE2Base;
            kv["_SumE1"] = Smart(LotusRound(sumE1Value, 0.001));
            kv["_SumE2"] = Smart(LotusRound(sumE2Value, 0.001));
            kv["_SumL1_1"] = Smart(tmpL1);
            kv["_SumL2"] = Smart(tmpL2);

            kv["SumRa"] = "2.5 [2F]";
            kv["SumRa1"] = "2.5 [2F]";

            string tmpKonMApp = "1579440-41";
            string tmpKil = tmpd - tmpd1 > 10 ? "1579442-" + kilnr : "1509952-" + kilnr;
            string tmpBygel = "7419465";
            kv["SumPB"] = passbit == 0 ? "" : " +passbit " + Smart(passbit) + "mm";

            bool machineEnabled = mv == "LU25" || mv == "LU-4000M";
            string tmpMaskinVal = machineEnabled ? "" : "";
            string tmpMaskinValS1 = mv == "LU25" ? "LU25" : mv == "LU-4000M" ? "LU-4000M" : "";
            kv["SumMaskinValS1"] = "Maskin: " + tmpMaskinValS1 + tmpMaskinVal + " - OP1";

            kv["SumF1_1"] = machineEnabled ? "1/2" : "";
            kv["SumF1_2"] = machineEnabled ? "1/2" : "";
            kv["SumF1_3"] = machineEnabled ? "1/1" : "";
            kv["SumF1_4"] = machineEnabled ? "1/10" : "";
            kv["SumF1_5"] = machineEnabled ? "1/1" : "";
            kv["SumF1_6"] = machineEnabled ? "1/1" : "";
            kv["SumF1_7"] = machineEnabled ? "1/1" : "";
            kv["SumF1_8"] = machineEnabled ? "1/2" : "";
            kv["SumF1_9"] = machineEnabled ? "1/10" : "";
            kv["SumF1_0"] = machineEnabled ? "1/10" : "";

            kv["SumD1_1"] = machineEnabled ? "Skjutmått" : "";
            kv["SumD1_2"] = machineEnabled ? "UD-Apparat" : "";
            kv["SumD1_3"] = machineEnabled ? "Multimar " + tmpRullar : "";
            kv["SumD1_4"] = machineEnabled ? "Konmätningsapprat enl. nedan" : "";
            kv["SumD1_5"] = machineEnabled ? "Ytjämnhetsmätare" : "";
            kv["SumD1_6"] = machineEnabled ? "UD-Apparat" : "";
            kv["SumD1_7"] = machineEnabled ? "Konmätningsapprat:" + tmpKonMApp + LB + "Inst. med kil " + tmpKil + LB + "Vid skärbyte Mätbygel:" + tmpBygel : "";
            kv["SumD1_8"] = "";
            kv["SumD1_9"] = machineEnabled ? "Egglinjal" : "";
            kv["SumD1_0"] = machineEnabled ? "Skjutmått" : "";

            kv["SumAF1_1"] = "";
            kv["SumAF1_2"] = machineEnabled ? "Inställd med ring/klove" : "";
            kv["SumAF1_3"] = machineEnabled ? "Inställd i längdmätbänk. Kontroll med gängmall" : "";
            kv["SumAF1_4"] = machineEnabled ? "Tolerans: " + kv["SumKonavv"] : "";
            kv["SumAF1_5"] = "";
            kv["SumAF1_6"] = machineEnabled ? "Tolerans: " + tmpRd : "";
            kv["SumAF1_7"] = machineEnabled ? kv["SumVe"] : "";
            kv["SumAF1_8"] = machineEnabled ? "Tolerans:" : "";
            kv["SumAF1_9"] = machineEnabled ? "Vid misstänkt formfel kontrollera hylsan i MarSurf Contour XC20" : "";
            kv["SumAF1_0"] = machineEnabled ? "Samtliga mått kontrolleras vid inställning" : "";

            kv["SumTextS1"] = "Okulär kontroll: Grader, frifläckar, slagmärken, repor, valkar etc. " +
                "Märkning ska vara rätt och tydlig. Oljespåret: Inget skorr och max 6,3Ra." + LB + 
                "240 & 241 serierna körs i samråd med tekniker";

            kv["SumPRit"] = "Produkt: " + pRitning;
            kv["SumGRit"] = "Gänga: " + (tmpArt == "MM" ? tmpStmm < 4 ? "239473:1, 7430182:A" : "237359:3, 7430181:2" : "7431233:2");
            kv["SumTolRit"] = "Toleranser: 1432012:7, 7437495:4 Gjutgodsdefekter: 7433015";
            kv["SumKlEgenskaper"] = @"PRODUCTION NUTS & SLEEVES & HOUSINGS\ALLMÄN\KLASSADE EGENSKAPER\Klassade egenskaper klämhylsor";
            kv["Kilinställning"] = kilinstallning;
            kv["Kilnr"] = kilnr;

            return kv;
        }
        private static string GetString(IEnumerable<Bookmark> bookmarks, string name)
        {
            if (bookmarks == null || string.IsNullOrEmpty(name)) return "";
            foreach (Bookmark bookmark in bookmarks) if (bookmark != null 
                && string.Equals(bookmark.BookmarkName, name, StringComparison.OrdinalIgnoreCase)) 
                return bookmark.BookmarkValue?.Trim() ?? "";
            return "";
        }

        private static double GetDouble(IEnumerable<Bookmark> bookmarks, string name) 
            => ParseDouble(GetString(bookmarks, name));

        private static string[] Split(string value, string separators) => string.IsNullOrEmpty(value) 
            ? Array.Empty<string>() : value.Split(separators.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

        private static string Item(string[] values, int lotusPosition) => values == null || lotusPosition < 1 
            || lotusPosition > values.Length ? "" : values[lotusPosition - 1];

        private static int MemberText(string value, string[] values)
        {
            if (values == null) return 0;
            for (int i = 0; i < values.Length; i++) if (string.Equals(value ?? "", values[i], 
                StringComparison.Ordinal)) return i + 1;
            return 0;
        }

        private static bool IsNumeric(string value) => !string.IsNullOrWhiteSpace(value) 
            && double.TryParse(value.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out _);

        private static double ParseDouble(string value) => string.IsNullOrWhiteSpace(value) ? 0 
            : double.TryParse(value.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double result) ? result : 0;

        private static string LotusText(object value)
        {
            if (value == null) return "";
            if (value is double d) return Smart(d);
            if (value is float f) return Smart(f);
            if (value is decimal m) return Smart((double)m);
            return value.ToString() ?? "";
        }

        private static string Smart(double value) => Math.Abs(value % 1) < 0.0000001 ? value.ToString("F0", CultureInfo.InvariantCulture) : value.ToString("0.######", CultureInfo.InvariantCulture);

        private static string SmartComma(double value) => Math.Abs(value % 1) < 0.0000001 ? value.ToString("F0", CultureInfo.InvariantCulture) : value.ToString("0.######", CultureInfo.InvariantCulture).Replace(".", ",");
        private static string F1Comma(double value) => value.ToString("0.0", CultureInfo.InvariantCulture).Replace(".", ",");
        private static string F3Comma(double value) => value.ToString("F3", CultureInfo.InvariantCulture).Replace(".", ",");
        private static string Left(string value, int length) 
            => string.IsNullOrEmpty(value) || length <= 0 ? "" : value.Substring(0, Math.Min(length, value.Length));

        private static string Right(string value, int length) 
            => string.IsNullOrEmpty(value) || length <= 0 ? "" : value.Length <= length ? value : value.Substring(value.Length - length);

        private static bool ContainsI(string source, string value) 
            => !string.IsNullOrEmpty(source) && !string.IsNullOrEmpty(value) && source.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;

        private static double LotusRound(double value, double factor)
        {
            if (factor == 0) return value;
            return Math.Floor((value / factor) + 0.5) * factor;
        }

        private static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published)) return "";
            if (!DateTime.TryParseExact(published, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime publishDate) 
                && !DateTime.TryParse(published, CultureInfo.InvariantCulture, DateTimeStyles.None, out publishDate) && !DateTime.TryParse(published, out publishDate)) return "";
            DateTime validTo = publishDate.AddDays(14);
            if (DateTime.Today > validTo.Date) return "";
            return "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)" + LB + LB + LB + LB + 
                "Popupruta aktiv till " + validTo.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
    }
}