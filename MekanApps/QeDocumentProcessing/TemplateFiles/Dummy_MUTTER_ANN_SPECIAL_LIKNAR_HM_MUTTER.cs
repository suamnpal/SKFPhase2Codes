using DocumentFormat.OpenXml.Office2016.Excel;
using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class Dummy_MUTTER_ANN_SPECIAL_LIKNAR_HM_MUTTER : ITemplateCalculations
    {
        private static string machine = string.Empty;
        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            machine = request?.MachineNumber ?? string.Empty;
            var result = new Dictionary<string, string>();

            Merge(result, GetHeader());
            Merge(result, GetDiameters());
            Merge(result, GetThreads());
            Merge(result, GetBasic());
            Merge(result, GetTolerances());
            Merge(result, GetForm());
            Merge(result, GetSurface());
            Merge(result, GetPage1());
            Merge(result, GetPage2());
            Merge(result, GetMachine());

            return result;
        }

        private (double d4, double d3, double d5, double dVal) GetFixed()
        {
            return (570, 660, 620, 573);
        }

        private Dictionary<string, string> GetHeader()
        {
            var d = new Dictionary<string, string>();
            d["SumRitningsnr"] = "ANN-0087";
            d["SumRitningsnr2"] = "ANN-0087";
            d["SumGTolRit"] = "7430181";
            d["SumGTolRit2"] = "7430181";
            d["SumYtRit"] = "7430184";
            d["SumYtRit2"] = "7430184";
            d["SumKlEgenskaper"] = "PRODUCTION NUTS & SLEEVES & HOUSINGS\\ALLMÄNKLASSADE EGENSKAPER\\Klassade egenskaper muttrar";
            d["SumKlEgenskaper2"] = d["SumKlEgenskaper"];
            return d;
        }

        private Dictionary<string, string> GetDiameters()
        {
            var d = new Dictionary<string, string>();
            var v = GetFixed();

            d["Sumd4"] = "(d4) 571";
            d["Sumdm"] = "(dm) 567";
            d["Sumd1"] = "(D1) 564";

            d["Sumd3"] = "(d3) 660";
            d["Sumd5"] = "(d5) 620";
            d["Sumd"] = "(d) 573";
            d["Sumd1"] = "";

            return d;
        }

        private Dictionary<string, string> GetThreads()
        {
            var d = new Dictionary<string, string>();
            var v = GetFixed();

            d["SumP"] = "(P) 6";
            d["SumGänga"] = "Tr 570x6";
            d["SumGF"] = "30º";

            return d;
        }

        private Dictionary<string, string> GetBasic()
        {
            var d = new Dictionary<string, string>();

            d["SumB"] = "(B) 75";
            d["SumB2"] = "37.5";
            d["Sumd2"] = "(d2 ) 593";

            return d;
        }

        private Dictionary<string, string> GetTolerances()
        {
            var d = new Dictionary<string, string>();

            d["Sumd4Tol"] = "± 0.800";
            d["SumdmTol"] = "+ 0.600";
            d["SumdmTolN"] = "- 0.0";

            d["SumD1Tol"] = "+ 0.500";
            d["SumD1TolN"] = "- 0.0";

            d["Sumd3Tol"] = "+ 0.0";
            d["Sumd3TolN"] = "- 1.250";

            d["Sumd5Tol"] = "+ 0.0";
            d["Sumd5TolN"] = "- 1.100";

            d["SumdTol"] = "+ 0.0";
            d["SumdTolN"] = "- 1.100";

            d["SumBTol"] = "+ 0.0";
            d["SumBTolN"] = "- 0.460";

            d["Sumd1Tol"] = "+ 0.500";
            d["Sumd1TolN"] = "- 0.0";

            d["SumSbTol"] = "± 0.310";
            d["Sumd2Tol"] = "± 0.800";

            d["SumL2Tol"] = "+ 2.000";
            d["SumL2TolN"] = "- 0.0";

            d["SumL3Tol"] = "+ 2.100";
            d["SumL3TolN"] = "- 0.0";

            d["SumhTol"] = "+ 2.000";
            d["SumhTolN"] = "- 0.0";

            return d;
        }

        private Dictionary<string, string> GetForm()
        {
            var d = new Dictionary<string, string>();

            d["Sumt1"] = "(t1) 0.100";
            d["Sumt2"] = "(t2) 0.100";
            d["Sumt4"] = "(t4) 0.800";
            d["Sumt5"] = "(t5) 1.6";
            d["Sumt7"] = "(t7) 1.00";

            return d;
        }

        private Dictionary<string, string> GetSurface()
        {
            var d = new Dictionary<string, string>();

            d["SumRa"] = "3.2";
            d["SumFas"] = "30º";
            d["SumIF1"] = "45º";
            d["SumIF2"] = "45º";
            d["SumRadie"] = "R 4.0";

            return d;
        }

        private Dictionary<string, string> GetPage1()
        {
            var d = new Dictionary<string, string>();

            d["SumF1_1"] = "1/1";
            d["SumF1_2"] = "1/2";
            d["SumF1_3"] = "1/5";
            d["SumF1_4"] = "1/5";
            d["SumF1_5"] = "1/5";
            d["SumF1_6"] = "1/5";
            d["SumF1_7"] = "1/3";
            d["SumF1_8"] = "1/5";

            d["SumD1_1"] = "Multimar";
            d["SumD1_2"] = "Mikrometer";
            d["SumD1_3"] = "Skjutmått";
            d["SumD1_4"] = "Skjutmått";
            d["SumD1_5"] = "Skjutmått";
            d["SumD1_6"] = "Skjutmått";
            d["SumD1_7"] = "Ytjämnhetsmätare";
            d["SumD1_8"] = "Planhet med Egglinjal";

            d["SumAF1_1"] = "Kontrolleras 1/tim med passbitsklove utf.1 ";
            d["SumAF1_2"] = "";
            d["SumAF1_3"] = machine == "LB45" ? "Tol h13" : "Tol h11";
            d["SumAF1_4"] = "";
            d["SumAF1_5"] = "";
            d["SumAF1_6"] = "Bearbetning i OP1 + 3mm";
            d["SumAF1_7"] = "Övriga Ra värden 6,3";
            d["SumAF1_8"] = "Vid misstänkt formfel lämnas till mätrum";

            d["SumTextS1"] = "Grader, frifläckar, slagmärken, repor, valkar & andra defekter samt ojämnheter kontrolleras med ögonmått, för övriga mått används skjutmått.";

            return d;
        }

        private Dictionary<string, string> GetPage2()
        {
            var d = new Dictionary<string, string>();

            d["SumF2_1"] = "1/5";
            d["SumF2_2"] = "1/5";
            d["SumF2_3"] = "1/5";
            d["SumF2_4"] = "1/5";
            d["SumF2_5"] = "inst.";

            d["SumD2_1"] = "Skjutmått";
            d["SumD2_2"] = "Djupmått";
            d["SumD2_3"] = "M16 min/max";
            d["SumD2_4"] = "Kontrolleras med tolk  M10 min/max";
            d["SumD2_5"] = "Funktion, muttersäkring";
       
            d["SumAF2_1"] = "";
            d["SumAF2_2"] = "";
            d["SumAF2_3"] = "";
            d["SumAF2_4"] = "";
            d["SumAF2_5"] = "Vid misstänkt lägesfel lämnas till mätrum.";

            d["SumTextS2"] = "Övriga mått kontrolleras vid inställning";
          
            d["SumSb"] = "(Sb) 40";
            d["Sumh"] = "(h) 20";
            d["SumL2"] = "(L2) 26";
            d["SumL3"] = "(L3) 17";
            d["SumLd2"] = "(Ld2) max:34";
            d["SumLd3"] = "(Ld3) max:23.5";

            d["SumG2"] = "(G2) M16";
            d["SumG3"] = "(G3) M10";

            d["SumGV"] = "45º";
            d["SumRHT"] = "16x R1.6 ±0.4";
            d["SumStämpling"] = "Stämplas enl. ritning 7430189";

            d["SumN1_4"] = "Gänga Lyftögla";
            d["SumB1_4"] = "G3";

            return d;
        }

        private Dictionary<string, string> GetMachine()
        {
            var d = new Dictionary<string, string>();
            
            d["SumMaskinValS1"] = "Maskin: " + machine + " - Svarvning";
            d["SumMaskinValS2"] = "Maskin: " + machine + " - Borrning & Fräsning";

            return d;
        }

        private void Merge(Dictionary<string, string> target, Dictionary<string, string> source)
        {
            foreach (var kv in source)
            {
                if (!target.ContainsKey(kv.Key))
                    target.Add(kv.Key, kv.Value ?? "");
            }
        }
    }
}