using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class MUTTRAR_24_60_EMAG_LT10M : ITemplateCalculations
    {
        private const string LB = "<<LineBreak>>";
        public Dictionary<string, string> CalculateWordParameters(
            APIRequest request)
        {
            var kv = new Dictionary<string, string>();

            string subject = request?.ProductDesignation ?? "";

            string maskinVal = request?.MachineNumber ?? "";

            kv["VaLPopUp"] = ComputePopup(request?.Published);

            string tmpFormat = subject.Trim().ToUpperInvariant();

            string tmpBet = tmpFormat.Replace(".", ",");

            bool tmpT = tmpBet.Contains("T");

            string[] parts = tmpBet.Split(new[] { ' ', '/', '.', '-' },StringSplitOptions.RemoveEmptyEntries);

            string tmpBet1 = parts.Length > 0 ? parts[0] : "";

            string tmpBet2 = parts.Length > 1 ? parts[1] : "";

            int tmpCount =tmpBet2.Length;

            string tmpArt = tmpBet1;

            string tmpSerie =tmpCount > 3? tmpBet2.Substring(0, 2): "0";

            string tmpTyp =tmpCount > 3 ? tmpBet2.Substring(tmpBet2.Length - 2) : tmpT ? tmpBet2.Substring(0, 2) : tmpBet2;

            int typ = ParseInt(tmpTyp);

            string[] tmpTypListaValues = tmpArt == "HM" &&  (tmpSerie == "30" || tmpSerie == "31") ? new[] { "44", "48", "52", "56", "60" }
                : tmpArt == "HM" && tmpT  ? new[] { "41", "42", "43", "44", "45", "46", "48", "50", "52", "54", "56", "58", "60" }
                : tmpArt == "HML" ? new[] { "41", "42", "43", "44", "45", "46", "47", "48", "50", "52", "54", "56", "58", "60" }
                : tmpArt == "KM" ? new[] { "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "36", "38", "40" }
                : tmpArt == "KML" ? new[] { "24", "26", "28", "30", "32", "34", "36", "38", "40" }
                : Array.Empty<string>();

            int tmpTL = Array.IndexOf(tmpTypListaValues, tmpTyp) + 1;

            bool tmpKMLike = tmpBet.Contains("KM");

            bool tmpKM = tmpArt == "KM";

            bool tmpKML = tmpArt == "KML";

            bool tmpHM30 = tmpArt == "HM" && tmpSerie == "30";

            bool tmpHM31 = tmpArt == "HM" && tmpSerie == "31";

            bool tmpBM = tmpHM30 || tmpHM31;

            bool tmpHM = tmpArt == "HM" && tmpSerie == "0";

            bool tmpHML = tmpArt == "HML";

            double tmpD = typ / 2.0 * 10.0;

            double tmpP = tmpD < 10.1 ? 0.75 : tmpD < 20.1 ? 1 :  tmpD < 50.1 ? 1.5 : tmpD < 150.1 ? 2 : tmpD < 200.1 ? 3 : tmpD < 300.1 ? 4 :  5;

            kv["SumP"] ="(P) " + Smart(tmpP);

            kv["SumGV"] =  tmpP < 4  ? "60°" : "30°";

            string tmpPrdRit = tmpKMLike ? "7433403" : tmpHM30 ? "223015" : tmpHM31 ? "223016" : tmpHM ? "224673" : tmpHML ? "222755" : "";

            kv["SumPrdRit"] ="Produkt: " + tmpPrdRit + " : senaste utgåva";

            string tmpGangRit = (tmpP < 3 ? "239473" : "237359") + ": senaste utgåva";

            kv["SumGängRit"] = "Gänga: " + tmpGangRit;

            kv["SumGängTolRit"] = "Gängtoleranser: " +  (tmpP < 3 ? "7430182": "7430181")+": senaste utgåva";

            kv["SumTolRit"] = "Övriga toleranser: 1432008 : senaste utgåva";

            string tmp1GBDia = Smart(tmpD);

            string tmp2GBDia =tmpP < 4 ? "M" + tmp1GBDia : "Tr" + tmp1GBDia;

            string tmpG =tmp2GBDia + "x" + Smart(tmpP);

            kv["SumG"] = tmpG;

            string tmpRullar =  tmpP < 4  ? "M" + Smart(tmpP) : "Tr" + Smart(tmpP);

            double tmpdm = Math.Round( tmpP == 0.75 ? tmpD - 0.487 : tmpP == 1 ? tmpD - 0.650 :   tmpP == 1.5 ? tmpD - 0.974 :
                    tmpP == 2 ? tmpD - 1.299 : tmpP == 3 ? tmpD - 1.949 : tmpP == 4 ? tmpD - 2.000 : tmpD - 2.500,2);

            kv["Sumdm"] = "(dm) " + Smart(tmpdm);

            double tmpdmTol = tmpP == 0.75 ? 0.106 : tmpP == 1 ? 0.125 : tmpP == 1.5 ? (tmpD < 45 ? 0.16 : 0.17) :
                tmpP == 2 ? (tmpD < 90 ? 0.19 : 0.20) : tmpP == 3 ? (tmpD < 180 ? 0.236 : 0.265) :
                tmpP == 4 ? 0.475 :  tmpP == 5 ? 0.530 : tmpP == 6 ? 0.600 : tmpP == 7 ? 0.630 : 0.710;

            kv["SumdmTol"] = "+ " + F3Smart(tmpdmTol) +  " [3F]";

            kv["SumdmTolN"] = "- 0 [2F]";

            double tmpD1 = Math.Round( tmpP == 0.75 ? tmpdm - 0.325 : tmpP == 1 ? tmpdm - 0.433 :  tmpP == 1.5 ? tmpdm - 0.650 :
                    tmpP == 2 ? tmpdm - 0.866 :tmpP == 3 ? tmpdm - 1.299 :tmpP == 4 ? tmpdm - 2.000 :tmpdm - 2.500, 2);

            kv["SumD1"] = "(D1) " +  Smart(tmpD1);

            double tmpD1Tol = tmpP == 0.75 ? 0.150 : tmpP == 1 ? 0.190 : tmpP == 1.5 ? 0.236 : tmpP == 2 ? 0.300 : tmpP == 3 ? 0.400 :
                tmpP == 4 ? 0.375 : tmpP == 5 ? 0.450 : tmpP == 6 ? 0.500 : tmpP == 7 ? 0.560 : 0.630;

            kv["SumD1Tol"] = "+ " + F3Smart(tmpD1Tol);

            kv["SumD1TolN"] = "- 0 [2F]";

            string[] tmpd3List = tmpBM  ? new[] { "222", "242", "262", "282", "302" }
                : tmpHM ? new[] { "207", "212", "217", "222", "227", "232", "242", "252", "262", "272", "282", "292", "302" }
                : tmpHML ? new[] { "207", "212", "217", "222", "227", "232", "237", "242", "252", "262", "272", "282", "292", "302" }
                : tmpKM ? new[] { "115.4", "120.4", "125.4", "130.4", "135.4", "140.4", "145.4", "150.4", "155.5", "160.5", "165.5", "170.5", "180.5", "190.5", "200.5" }
                : tmpKML ? new[] { "120.4", "130.4", "140.4", "150.4", "160.5", "170.5", "180.5", "190.5", "200.5" }
                : Array.Empty<string>();

            double tmpd3 = ParseDouble( tmpd3List[Math.Max(0, tmpTL - 1)]);

            kv["Sumd3"] = "(d3) " + Smart(tmpd3);

            kv["Sumd3Tol"] = tmpKML || tmpKM ? (tmpd3 < 3.01 ? "+ 0.140" :
                       tmpd3 < 6.01 ? "+ 0.180" :
                       tmpd3 < 10.01 ? "+ 0.220" :
                       tmpd3 < 18.01 ? "+ 0.270" :
                       tmpd3 < 30.01 ? "+ 0.330" :
                       tmpd3 < 50.01 ? "+ 0.390" :
                       tmpd3 < 80.01 ? "+ 0.460" :
                       tmpd3 < 120.01 ? "+ 0.540" :
                       tmpd3 < 180.01 ? "+ 0.630" :
                       tmpd3 < 250.01 ? "+ 0.720" :
                       tmpd3 < 315.01 ? "+ 0.810" :
                       tmpd3 < 400.01 ? "+ 0.890" :
                       tmpd3 < 500.01 ? "+ 0.970" :
                       tmpd3 < 630.01 ? "+ 1.100" :
                       tmpd3 < 800.01 ? "+ 1.250" :
                       tmpd3 < 1000.01 ? "+ 1.400" :
                       tmpd3 < 1250.01 ? "+ 1.650" :
                       tmpd3 < 1600.01 ? "+ 1.950" :
                       tmpd3 < 2000.01 ? "+ 2.300" :
                       tmpd3 < 2500.01 ? "+ 2.800" : "+ 3.300")
                    : (tmpd3 < 3.01 ? "+ 0.250" :
                       tmpd3 < 6.01 ? "+ 0.300" :
                       tmpd3 < 10.01 ? "+ 0.360" :
                       tmpd3 < 18.01 ? "+ 0.430" :
                       tmpd3 < 30.01 ? "+ 0.520" :
                       tmpd3 < 50.01 ? "+ 0.620" :
                       tmpd3 < 80.01 ? "+ 0.740" :
                       tmpd3 < 120.01 ? "+ 0.870" :
                       tmpd3 < 180.01 ? "+ 1.000" :
                       tmpd3 < 250.01 ? "+ 1.150" :
                       tmpd3 < 315.01 ? "+ 1.300" :
                       tmpd3 < 400.01 ? "+ 1.400" :
                       tmpd3 < 500.01 ? "+ 1.550" :
                       tmpd3 < 630.01 ? "+ 1.750" :
                       tmpd3 < 800.01 ? "+ 2.000" :
                       tmpd3 < 1000.01 ? "+ 2.300" :
                       tmpd3 < 1250.01 ? "+ 2.600" :
                       tmpd3 < 1600.01 ? "+ 3.100" :
                       tmpd3 < 2000.01 ? "+ 3.700" :
                       tmpd3 < 2500.01 ? "+ 4.400" : "+ 5.400");

            kv["Sumd3TolN"] = "- 0";

            string[] tmpD4List = tmpHM30  ? new[] { "242", "270", "290", "310", "336" }
                : tmpHM31 ? new[] { "250", "270", "300", "320", "340" }
                : tmpHM  ? new[] { "238", "238", "248", "250", "258", "260", "270", "290", "300", "310", "320", "330", "340" }
                : tmpHML? new[] { "232", "232", "242", "242", "252", "252", "262", "270", "280", "290", "300", "310", "320", "336" }
                : tmpKM ? new[] { "137", "138", "148", "149", "160", "160", "171", "171", "182", "182", "193", "193", "203", "214", "226" }
                : tmpKML ? new[] { "135", "145", "155", "170", "180", "190", "200", "210", "222" }
                : Array.Empty<string>();

            double tmpD4 = ParseDouble(tmpD4List[Math.Max(0, tmpTL - 1)]);

            kv["SumD4"] ="(D4) " + Smart(tmpD4);

            kv["SumD4Tol"] = "+ 0 [3F]";

            kv["SumD4TolN"] = tmpD4 > 1000 ? "- 1.650" :
                tmpD4 > 800 ? "- 1.400" :
                tmpD4 > 630 ? "- 1.250" :
                tmpD4 > 500 ? "- 1.100" :
                tmpD4 > 400 ? "- 0.970" :
                tmpD4 > 315 ? "- 0.890" :
                tmpD4 > 250 ? "- 0.810" :
                tmpD4 > 180 ? "- 0.720" :
                tmpD4 > 120 ? "- 0.630" :
                tmpD4 > 80 ? "- 0.540" :
                tmpD4 > 50 ? "- 0.460" :
                tmpD4 > 30 ? "- 0.390" :
                tmpD4 > 18 ? "- 0.330" : "- 0.270";

            string[] tmpD5List = tmpHM30  ? new[] { "260", "290", "310", "330", "360" }
                : tmpHM31 ? new[] { "280", "300", "330", "350", "380" }
                : tmpHM ? new[] { "260", "270", "270", "280", "280", "290", "300", "320", "330", "340", "350", "370", "380" }
                : tmpHML ? new[] { "250", "250", "260", "260", "270", "270", "280", "290", "300", "310", "320", "330", "340", "360" }
                : tmpKM ? new[] { "150", "155", "160", "165", "175", "180", "190", "195", "200", "210", "210", "220", "230", "240", "250" }
                : tmpKML ? new[] { "145", "155", "165", "180", "190", "200", "210", "220", "240" }
                : Array.Empty<string>();

            double tmpD5 = ParseDouble( tmpD5List[Math.Max(0, tmpTL - 1)]);

            kv["SumD5"] = "(D5) " + Smart(tmpD5);

            kv["SumD5Tol"] = "+ 0 [3F]";

            kv["SumD5TolN"] = tmpD5 > 1000 ? "- 1.650" :
                tmpD5 > 800 ? "- 1.400" :
                tmpD5 > 630 ? "- 1.250" :
                tmpD5 > 500 ? "- 1.100" :
                tmpD5 > 400 ? "- 0.970" :
                tmpD5 > 315 ? "- 0.890" :
                tmpD5 > 250 ? "- 0.810" :
                tmpD5 > 180 ? "- 0.720" :
                tmpD5 > 120 ? "- 0.630" :
                tmpD5 > 80 ? "- 0.540" :
                tmpD5 > 50 ? "- 0.460" :
                tmpD5 > 30 ? "- 0.390" :
                tmpD5 > 18 ? "- 0.330" :  "- 0.270";

            string[] tmpBList =  tmpHM30 ? new[] { "30", "34", "34", "38", "42" }
                : tmpHM31 ? new[] { "32", "34", "36", "38", "40" }
                : tmpHM ? new[] { "30", "30", "30", "32", "32", "34", "34", "36", "36", "38", "38", "40", "40" }
                : tmpHML ? new[] { "30", "30", "30", "30", "30", "30", "34", "34", "34", "34", "38", "38", "38", "42" }
                : tmpKM ? new[] { "19", "20", "21", "21", "22", "22", "24", "24", "25", "25", "26", "26", "27", "28", "29" }
                : tmpKML  ? new[] { "20", "21", "22", "24", "25", "26", "27", "28", "29" }
                : Array.Empty<string>();

            double tmpB = ParseDouble( tmpBList[Math.Max(0, tmpTL - 1)]);

            kv["SumB"] = "(B) " + Smart(tmpB);

            kv["SumBTol"] =  "+ 0 [3F]";

            kv["SumBTolN"] = (tmpB > 80 ? "- 0.540" : tmpB > 50 ? "- 0.460" :
                 tmpB > 30 ? "- 0.390" :tmpB > 18 ? "- 0.330" : tmpB > 10 ? "- 0.270" :  tmpB > 6 ? "- 0.220" : "- 0.180") + " [3F]";

            kv["SumV30"] = "30°";
            kv["SumV45"] = "45°";

            kv["SumR"] = tmpD1 > 295 ? "R 3.5" :  tmpD1 > 235 ? "R 3.0" :tmpD1 > 180 ? "R 2.5" : tmpD1 > 125 ? "R 2.0" : tmpD1 > 60 ? "R 1.5" : "R 1.0";

            kv["SumRd"] = tmpKMLike ? (tmpD5 < 185  ? "0.080" : "0.092")
                    : tmpD5 < 19 ? "0.035": tmpD5 < 31? "0.042" : tmpD5 < 51 ? "0.050" : tmpD5 < 81? "0.060": tmpD5 < 121 ? "0.070" : "0.080";

            double tmpt2 = (tmpArt == "HM" || tmpArt == "HML") ? (tmpD < 51 ? 0.04 :  tmpD < 121 ? 0.05 :tmpD < 251 ? 0.06 : tmpD < 316 ? 0.07 :
                       tmpD < 401 ? 0.08 :tmpD < 501 ? 0.09 :  tmpD < 631 ? 0.10 : tmpD < 801 ? 0.12 :tmpD < 1001 ? 0.14 :0.16) : (typ < 11 ? 0.02 :typ < 25 ? 0.025 : 0.03);

            kv["Sumt2"] = F3(tmpt2);

            kv["SumKM-Kast"] = (typ == 24 || typ == 30) ? "Lämna in mutter för uppmätning av kast (t2)" : "";

            kv["SumRa"] =tmpPrdRit == "7433403" ? "2.5" : tmpD1 > 110 ? "3.2" : "2.5";

            kv["SumSnr"] = tmpBM ? "(8x) 45°" : "(4x) 90°";

            string tmpUnr = tmpBM ? "8 st. Borrhål, jämn delning, " : "";

            string tmpUG = tmpBM ? "Gängtapp: " +  (tmpHM30 ? (tmpD5 < 265 ? "M6" : "M8") : (tmpD5 < 305 ? "M8" : "M10")): "";

            string tmpUGL = tmpBM ? ",min: gänglängd: " + (tmpHM30 ? (tmpD5 < 265 ? "12mm" : "17mm") : (tmpD5 < 305 ? "17mm" : "21mm")) : "";

            string tmpUD = tmpBM ? ", Håldiameter: " + (tmpHM30 ? (tmpD5 < 265 ? "229" : tmpD5 < 300 ? "253" : tmpD5 < 315 ? "273" :  tmpD5 < 340 ? "293" : tmpD5 < 370 ? "316" : "336")
                          : (tmpD5 < 290 ? "238" : tmpD5 < 310 ? "258" :tmpD5 < 340 ? "281" : tmpD5 < 360 ? "301" : tmpD5 < 390 ? "326" : "346")) : "";

            kv["SumBM"] =tmpBM ? tmpUnr + tmpUG + tmpUGL + tmpUD + LB+"Placering borrhål: funktionskontroll med muttersäkring & kontroll 100%": "";

            double tmpS =  tmpHM30 ? (tmpD < 261 ? 20 : 24) : tmpHM31 ? (tmpD < 241 ? 20 : 24) : tmpHM ? (tmpD < 251 ? 20 : 24) :
                tmpHML ? (tmpD < 206 ? 18 : tmpD < 271 ? 20 : 24) :tmpKM ? (tmpD < 131 ? 12 : tmpD < 151 ? 14 : tmpD < 171 ? 16 : 18) :
                tmpKML ? (tmpD < 141 ? 12 : tmpD < 161 ? 14 : tmpD < 191 ? 16 : 18) :0;

            kv["SumS"] = "(S) " + Smart(tmpS);

            kv["SumSTol"] = (tmpS < 4 ? "± 0.125" : tmpS < 7 ? "± 0.150" :  tmpS < 11 ? "± 0.180" : tmpS < 19 ? "± 0.215" : tmpS < 31 ? "± 0.260" :tmpS < 51 ? "± 0.310" : "± 0.370")+ " [3F]";

            double tmpt = tmpHM30 ? (tmpD < 221 ? 9 : tmpD < 281 ? 10 : 12) : tmpHM31 ? (tmpD < 241 ? 10 : 12) : tmpHM ? (tmpD < 251 ? 10 : 12) :
                tmpHML ? (tmpD < 211 ? 8 : tmpD < 236 ? 9 : tmpD < 291 ? 10 : 12) :tmpKM ? (tmpD < 131 ? 5 : tmpD < 151 ? 6 : tmpD < 171 ? 7 : 8) :
                tmpKML ? (tmpD < 191 ? 5 : 8) :  0;

            kv["Sumt"] = "(t) " + Smart(tmpt);

            kv["SumtTol"] = tmpt < 6 ? "+ 1.200" : "+ 1.500";

            kv["SumtTolN"] =  "- 0 [3F]";

            double tmpt1 =(tmpArt == "HM" || tmpArt == "HML") ? (tmpS > 30 ? 1.5 : tmpS > 18 ? 1.25 : tmpS > 10 ? 1.0 :tmpS > 6 ? 0.75 : 0.5)
                    : (typ < 2 ? 0.3 : typ < 11 ? 0.375 : typ < 21 ? 0.045 :  0.55);

            kv["Sumt1"] = F3(tmpt1);

            string tmpt3 =(tmpArt == "HM" || tmpArt == "HML") ? "n/a" : F3(typ < 21 ? 0.15 : 0.2);

            kv["Sumt3"] = tmpt3;

            string tmpMaskinValS1 = maskinVal == "EMAG" ? "EMAG": maskinVal == "LT-10M" ? "LT-10M": "";

            kv["SumMaskinValS1"] = "Maskin: " + tmpMaskinValS1;

            bool machineEnabled = maskinVal == "EMAG" || maskinVal == "LT-10M";

            SetFrequencies(kv, machineEnabled);
            SetDevices(kv, machineEnabled, tmpP, tmpRullar);
            SetAF( kv,  machineEnabled, tmpP, tmpG,tmpBM, kv["SumKM-Kast"]);

            kv["SumText"] = "Grader, frifläckar, slagmärken, repor kontr. okulärt. Sprickor och andra gjutgodsdefekter kontr. okulärt efter inoljning."
                + LB +  "Övriga mått kontrolleras vid inställning med skjutmått, alla muttrarna skall vara rena från spånor/smuts och alla ytor inoljade. Efter skärbyte gänga, skall även den sista muttern innan skärbyte kontrolleras. Vid kontroll av gängan skall även funktionskontroll med gängtolk eller om det finns med hylsa (Trapets) utföras";

            kv["SumKlEgenskaper"] =  @"PRODUCTION NUTS & SLEEVES & HOUSINGS\ALLMÄN\KLASSADE EGENSKAPER\Klassade egenskaper muttrar";

            return kv;
        }

        static void SetFrequencies( Dictionary<string, string> kv,bool machineEnabled)
        {
            kv["SumF1_1"] = machineEnabled ? "1/tim" : "";
            kv["SumF1_2"] = machineEnabled ? "1/10" : "";
            kv["SumF1_3"] = machineEnabled ? "1/tim" : "";
            kv["SumF1_4"] = machineEnabled ? "1/tim" : "";
            kv["SumF1_5"] = machineEnabled ? "1/tim" : "";
            kv["SumF1_6"] = machineEnabled ? "1/tim" : "";
            kv["SumF1_7"] = machineEnabled ? "Inst." : "";
            kv["SumF1_8"] = machineEnabled ? "Inst." : "";
            kv["SumF1_9"] = machineEnabled ? "1/tim" : "";
            kv["SumF1_0"] = machineEnabled ? "Inst." : "";
            kv["SumF1_11"] = machineEnabled ? "1/tim" : "";
            kv["SumF1_12"] = machineEnabled ? "Inst." : "";

            kv["SumF2_1"] = machineEnabled ? "1/1" : "";
            kv["SumF2_2"] = machineEnabled ? "1/Skift" : "";
            kv["SumF2_3"] = machineEnabled ? "1/Skift" : "";
            kv["SumF2_4"] = machineEnabled ? "1/Skift" : "";
            kv["SumF2_5"] = machineEnabled ? "1/Skift" : "";
            kv["SumF2_6"] = machineEnabled ? "1/Skift" : "";
            kv["SumF2_7"] = machineEnabled ? "1/Skift" : "";
            kv["SumF2_8"] = machineEnabled ? "1/Skift" : "";
            kv["SumF2_9"] = machineEnabled ? "1/25" : "";
            kv["SumF2_0"] = machineEnabled ? "1/25" : "";
            kv["SumF2_10"] = machineEnabled ? "1/50" : "";
            kv["SumF2_11"] = machineEnabled ? "1/10" : "";
            kv["SumF2_12"] = machineEnabled ? "1/10" : "";
            kv["SumF2_13"] = machineEnabled ? "1/dygn" : "";
            kv["SumF2_14"] = machineEnabled ? "1/1" : "";
            kv["SumF2_15"] = "";
        }

        static void SetDevices(Dictionary<string, string> kv, bool machineEnabled,double tmpP, string tmpRullar)
        {
            kv["SumD1_1"] =  machineEnabled ? (tmpP < 4? "Klump": "Unimeter")  : "";

            kv["SumD1_2"] = machineEnabled ? "UD-Apparat med rullar: " + tmpRullar  : "";

            kv["SumD1_3"] = machineEnabled ? "Skjutmått" : "";
            kv["SumD1_4"] = machineEnabled ? "Skjutmått" : "";
            kv["SumD1_5"] = machineEnabled ? "Skjutmått" : "";
            kv["SumD1_6"] = machineEnabled ? "Skjutmått" : "";
            kv["SumD1_7"] = machineEnabled ? "Skjutmått" : "";
            kv["SumD1_8"] = machineEnabled ? "Skjutmått" : "";
            kv["SumD1_9"] = machineEnabled ? "min/max-Tolk" : "";
            kv["SumD1_0"] = machineEnabled ? "Egglinjal, mätmaskin" : "";
            kv["SumD1_11"] = machineEnabled ? "Ytjämnhetsmätare" : "";
            kv["SumD1_12"] = machineEnabled ? "MHB" : "";
        }

        static void SetAF( Dictionary<string, string> kv,bool machineEnabled,double tmpP, string tmpG,bool tmpBM, string sumKmKast)
        {
            kv["SumAF1_1"] = machineEnabled ? (tmpP < 4 ? "Klump märkt: " + tmpG : "")  : "";

            kv["SumAF1_2"] = machineEnabled ? ((tmpP < 4 ? "Kontrolleras med gängtolk " + tmpG : "Kontrolleras med klove") + " 1/tim"): "";

            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = "";
            kv["SumAF1_6"] = "";
            kv["SumAF1_7"] = "";
            kv["SumAF1_8"] = "";

            kv["SumAF1_9"] = machineEnabled ? (tmpBM? "Topsning 100% alla borrhål" : "") : "";

            kv["SumAF1_0"] = machineEnabled ? "Vid misstänkt formfel lämna mutter till mätrum " + sumKmKast : "";

            kv["SumAF1_11"] = machineEnabled ? "Ytjämnhet övriga ytor Ra 6.3"  : "";

            kv["SumAF1_12"] = "";
        }

        static int ParseInt(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0;

            int.TryParse(value, out int result);

            return result;
        }

        static double ParseDouble(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0;

            value = value.Replace("*", "");

            double.TryParse( value.Replace(",", "."), NumberStyles.Any,  CultureInfo.InvariantCulture, out double result);

            return result;
        }

        static string Smart(double value)
        {
            if (Math.Abs(value % 1) < 0.0001)
            {
                return value.ToString("F0", CultureInfo.InvariantCulture);
            }

            return value.ToString(CultureInfo.InvariantCulture);
        }

        static string F3(double value)
        {
            return value.ToString("F3", CultureInfo.InvariantCulture);
        }

        static string F3Smart(double value)
        {
            return value.ToString("0.###", CultureInfo.InvariantCulture);
        }

        static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published))
                return "";

            if (!DateTime.TryParse(published, out DateTime publishDate))
            {
                return "";
            }

            DateTime validTo =publishDate.AddDays(14);

            if (DateTime.Today > validTo)
                return "";

            return "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)" + LB + LB + "Popupruta aktiv till " + validTo.ToString("yyyy-MM-dd");
        }
    }
}
