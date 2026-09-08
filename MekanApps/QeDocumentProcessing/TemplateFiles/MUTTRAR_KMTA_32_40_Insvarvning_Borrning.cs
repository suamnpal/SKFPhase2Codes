
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class MUTTRAR_KMTA_32_40_Insvarvning_Borrning : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var result = new Dictionary<string, string>();

            int size = ExtractSize(request?.ProductDesignation);
            string machine = request?.MachineNumber ?? "";

            Merge(result, Header(request));
            Merge(result, Geometry());
            Merge(result, Widths());
            Merge(result, Threads());
            Merge(result, Details());
            Merge(result, Radii());
            Merge(result, Texts());
            Merge(result, Tables());
            Merge(result, MissingVariablesFilled());
            Merge(result, EnsureAllKeys());

            Merge(result, OverrideBySize(size));
            Merge(result, OverrideByMachine(machine, size));

            return result;
        }

        private int ExtractSize(string product)
        {
            var m = Regex.Match(product ?? "", @"\d+");
            return m.Success ? int.Parse(m.Value) : 32;
        }

        private Dictionary<string, string> OverrideBySize(int size)
        {
            var r = new Dictionary<string, string>();
            double D = size * 5;

            double f1 =
                D < 56 ? 6.8 :
                D < 81 ? 9 :
                D < 131 ? 11.2 :
                D < 171 ? 12.7 :
                D < 191 ? 15 : 12.7;

            r["Sumf1"] = "(f1) " + f1;
            r["Sumf1Tol"] = (D < 56 ? "+ 0.6" : "+ 1.0") + " [2F]";
            r["Sumf1TolN"] = "0 [2F]";

            double f2 =
                D < 56 ? 8.7 :
                D < 66 ? 11.2 :
                D < 81 ? 11.7 :
                D < 131 ? 14.7 :
                D < 171 ? 16.2 :
                D < 191 ? 18.5 : 16.2;

            r["Sumf2"] = "(f2) " + f2;

            double f4 =
                (D == 180 || D == 190) ? 11.8 :
                D < 56 ? 5.7 :
                D < 81 ? 7.9 :
                D < 131 ? 9.8 :
                11;

            r["Sumf4"] = "(f4) " + f4;
            r["Sumf4Tol"] = (f4 == 5.7 ? " ± 0.1" : " ± 0.2") + " [3P]";

            double f5 =
                (D == 180 || D == 190) ? 15 :
                D < 56 ? 6.5 :
                D < 81 ? 9.3 :
                D < 131 ? 11.4 :
                13;

            r["Sumf5"] = "(f5) " + f5;
            r["Sumf5Tol"] = "+ 0.5";
            r["Sumf5TolN"] = "0";

            int g1 = size < 12 ? 6 : size < 17 ? 8 : 10;
            r["SumG1"] = "M" + g1 + "-6H [3F]";

            double d7 =
                g1 == 5 ? 3.75 :
                g1 == 6 ? 4.45 :
                g1 == 8 ? 5.75 : 7.6;

            r["Sumd7"] = "(d7) " + d7.ToString().Replace(".", ",");

            double N1 =
                D < 56 ? 4.3 :
                D < 71 ? 5.3 :
                D < 151 ? 6.5 : 8.4;

            r["SumN1"] = "(N1) " + N1;
            r["SumN1Tol"] = (D < 71 ? "+ 0.180" : "+ 0.220") + " [2F]";
            r["SumN1TolN"] = "0 [2F]";

            int N2 =
                D < 26 ? 4 :
                D < 41 ? 5 :
                D < 66 ? 6 :
                D < 131 ? 8 : 10;

            r["SumN2"] = "(N2) " + N2;
            r["SumN2Tol"] = (D < 66 ? "+ 0.180" : "+ 0.220") + " [2F]";
            r["SumN2TolN"] = "0 [2F]";

            double d2 =
                D < 26 ? D + 17 :
                D < 41 ? D + 18 :
                D < 46 ? D + 23 :
                D < 56 ? D + 20 :
                D < 61 ? D + 24 :
                D < 66 ? D + 23 :
                D < 76 ? D + 25 :
                D < 111 ? D + 30 :
                D < 131 ? D + 35 :
                D < 151 ? D + 40 :
                D < 171 ? D + 45 :
                D < 191 ? D + 50 : D + 50;

            r["Sumd2"] = "(d2) " + Math.Round(d2, 0);
            r["Sumd2Tol"] = "+0 [3F]";
            r["Sumd2TolN"] = "-0.290 [3F]";

            double d3 =
                D < 31 ? D + 10 :
                D < 41 ? D + 12 :
                D < 51 ? D + 13 :
                D < 66 ? D + 15 :
                D < 76 ? D + 16 :
                D < 86 ? D + 17 :
                D < 91 ? D + 20 :
                D < 96 ? D + 19 :
                D < 101 ? D + 20 :
                D < 121 ? D + 22 :
                D < 141 ? D + 26 :
                D < 161 ? D + 30 : D + 35;

            r["Sumd3"] = "(d3) " + Math.Round(d3, 0);
            r["Sumd3Tol"] = "+0 [3F]";
            r["Sumd3TolN"] = "-0.5 [3F]";

            double d5 =
                D < 131 ? D + 1 :
                D < 201 ? D + 0.5 : D + 2;

            r["Sumd5S1"] = "(d5) " + Math.Round(d5, 0);
            r["Sumd5S1Tol"] = "+ 0";
            r["Sumd5S1TolN"] = "- 0.5";

            double e2 = D < 21 ? 1 : D < 31 ? 2 : D < 46 ? 3 : D < 86 ? 4 : 5;
            r["Sume2S1"] = "(e2) " + (e2 + 1);
            r["Sume2S1Tol"] = "± 0.2";

            double B =
                D < 36 ? 20 :
                D < 46 ? 22 :
                D < 61 ? 24 :
                D < 66 ? 25 :
                D < 76 ? 26 :
                D < 81 ? 30 :
                32;

            r["SumBS1"] = "(B) " + (B + 1);
            r["SumBS1Tol"] = D < 76 ? "± 0.20" : "± 0.25";

            double pitch =
                D < 11 ? 0.75 :
                D < 21 ? 1 :
                D < 76 ? 1.5 :
                D < 121 ? 2 :
                D < 201 ? 3 :
                D < 301 ? 4 : 5;

            double tmpD1 =
                pitch == 0.75 ? D - 0.812 :
                pitch == 1 ? D - 1.083 :
                pitch == 1.5 ? D - 1.624 :
                pitch == 2 ? D - 2.165 :
                pitch == 3 ? D - 3.248 :
                pitch == 4 ? D - 4 :
                D - 5;

            double tmpD1S1 = tmpD1 - 1;
            string d1Formatted = tmpD1S1.ToString("0.00").Replace(".", ",");

            r["SumD1S1"] = "(D1) " + d1Formatted;
            r["SumD1S1Tol"] = "± 0.2";

            double HD =
                D < 131 ? D + 23 + N1 :
                D < 171 ? D + 25 + N1 : D + 29 + N1;
            HD = Math.Abs(HD - 193.4) < 0.0001 ? 193.5 : HD;
            r["SumHD"] = ("(HD) " + Math.Round(HD, 1)).Replace(".",",") + " ± 0.360 [2F]";

            r["Sumt3"] =
                D < 41 ? "0.3" :
                D < 66 ? "0.5" :
                D < 131 ? "0.75" : "1.0";

            r["SumM1"] = "10" + " ±0.25";

            return r;
        }

        private Dictionary<string, string> OverrideByMachine(string machine, int size)
        {
            var r = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(machine))
                return r;

            string m = machine.Contains("LVT") ? "LVT300" : "LT300";

            r["SumMaskinValS1"] = "Insvarning: " + m;
            r["SumMaskinValS2"] = "Borrning: " + m;

            r["SumD1_1"] = m == "LVT300" ? "UD-Apparat/Skjutmått" : "Skjutmått";
            r["SumAF2_3"] = m == "LT300"
                ? "Skruv dras till botten och sedan tillbaks 1½ varv, M10X10"
                : "Passas in med skruv, M10X10";
            r["SumAF1_2"] = m == "LVT300" ? "" : "Utvändig falsdjup (e2) efter 1:a OP i H-spindeln: 5mm";
            r["SumAF1_3"] = m == "LVT300" ? "" : "Utvändig falsdiameter (d3) efter 1:a OP i H-spindeln: 191mm";
       
            return r;
        }

        private void Merge(Dictionary<string, string> target, Dictionary<string, string> source)
        {
            foreach (var kv in source)
                target[kv.Key] = kv.Value ?? "";
        }

        private Dictionary<string, string> Header(APIRequest request) => new Dictionary<string, string>
        {
            {"SumMaskinValS1","Insvarning: " + (request?.MachineNumber ?? "") },
            {"SumMaskinValS2","Borrning: " + (request?.MachineNumber ?? "") },
            {"SumRitNr","11K 7433541"},
            {"SumRitNr2","11K 7433541"}
        };

        private Dictionary<string, string> Geometry() => new Dictionary<string, string>
        {
            {"SumE","16.129"},{"SumETol","+0.200"},{"SumETolN","-0.0"},
            {"SumH","28"},{"SumHTol","+0.500"},{"SumHTolN","-0.0"},
            {"SumP","5"},{"SumPTol","+0.400"},{"SumPTolN","-0.0"},
            {"SumK","4"},{"SumKTol","+0.400"},{"SumKTolN","-0.0"},
            {"SumN","3.5"},{"SumNTol","+0.0"},{"SumNTolN","-0.500"},
            {"SumL","22.5"}
        };

        private Dictionary<string, string> Widths() => new Dictionary<string, string>
        {
            {"SumB","46"},{"SumBTol","+0.500"},{"SumBTolN","-0.0"},
            {"Sumb1","(b1) 8.5"},{"Sumb1Tol","±0.25[3F]"},
            {"Sumb2","(b2) 15"},{"Sumb2Tol","± 0.25 [3F]"},
            {"Sumb4","15.5"},{"Sumb4Tol","+0.0"},{"Sumb4TolN","-0.200"},
            {"Sumb5","8.5"},{"Sumb5Tol","+0.0"},{"Sumb5TolN","-0.500"}
        };

        private Dictionary<string, string> Threads() => new Dictionary<string, string>
        {
            {"SumG","M6"},{"SumGn","Min.13"},
            {"SumBorrtext","Genomgående hål, borr får ej beröra tätningsplan"}
        };

        private Dictionary<string, string> Details() => new Dictionary<string, string>
        {
            {"SumAm","1.0"},{"SumAmTol","±0.1"},
            {"SumAmb","2.0"},{"SumAmbTol","±0.200"},
            {"SumBhd","3"},{"SumBhdTol",""},{"SumBhdTolN",""},
            {"SumBhd1","6.3"},{"SumBhd1Tol","+0.2"},{"SumBhd1TolN","-0.0"},
            {"SumF1","2x45°"}
        };

        private Dictionary<string, string> Radii() => new Dictionary<string, string>
        {
            {"SumR05","0.5"},{"SumR05a",""},
            {"SumR08","0.8"},{"SumF15","1.5x45°"},
            {"SumR1","1x45°"},{"SumRd","0.10"},
            {"SumRdText","Brytna kanter: Min. 0.2"},
            {"SumCo1","0.15"}
        };

        private Dictionary<string, string> Texts() => new Dictionary<string, string>
        {
            {"SumTextS1","100% okulär kontroll av grader, frittfläckar, slagmärken, repor, valkar och andra ojämnheter."},
            {"SumTextS2","100% okulär kontroll av grader, frittfläckar, slagmärken, repor, valkar och andra ojämnheter. <<LineBreak>> Vid upptäckta felaktiga detaljer skall kontroll av föregående detalj göras tills första godkända detalj hittas."},
            {"SumRa32","Ytjämnhet alla övriga färdig-bearbetade ytor 3.2"},
            {"SumKlEgenskaperS2","PPA & PPH ALLMÄNNA KLASSADE EGENSKAPER Klassade egenskaper muttrar KMT KMTA"},
            {"SumKlEgenskaperS1","PPA & PPH ALLMÄNNA KLASSADE EGENSKAPER Klassade egenskaper muttrar KMT KMTA"}
        };

        private Dictionary<string, string> Tables() => new Dictionary<string, string>
        {
            {"SumF1_1","1/10"},{"SumF1_e",""},{"SumF1_2","1/10"},{"SumF1_3","1/10"},
            {"SumF1_4","1/10"},{"SumF1_5","1/10"},{"SumF1_6","1/10"},{"SumF1_7","Inst."},

            {"SumD1_1","Skjutmått"},{"SumD1_e",""},
            {"SumD1_2","Mall: 7424073/3"},{"SumD1_3","Skjutmått"},
            {"SumD1_4","Skjutmått"},{"SumD1_5","Skjutmått"},
            {"SumD1_6","Ytjämnhetsmätare"},{"SumD1_7","Skjutmått"},{"SumD2_8","Skjutmått"},

            {"SumF2_1","1/10"},{"SumF2_2","1/10"},{"SumF2_3","1/20"},
            {"SumF2_4","Inst."},{"SumF2_5","Inst."},{"SumF2_6","1/50"},{"SumF2_7","Inst."},
            {"SumF2_8","1/5"}
        };

        private Dictionary<string, string> EnsureAllKeys()
        {
            return new Dictionary<string, string>
    {
        {"SumBOp1",""},

        {"SumAF1_1",""},
        {"SumAF1_e",""},
        {"SumAF1_4",""},
        {"SumAF1_5",""},
        {"SumAF1_6",""},
        {"SumAF1_7",""},

        {"SumAF2_1",""},
        {"SumAF2_2",""},
      
        {"SumAF2_4",""},
        {"SumAF2_5",""},
        {"SumAF2_6",""},
        {"SumAF2_7",""},
        {"SumAF2_8",""},
        {"SumD2_1","Gängtolk"},
        {"SumD2_2","Gängtolk Combi"},
        {"SumD2_3","Skjutmått"},
        {"SumD2_4","Haltolk"},
        {"SumD2_5","Haltolk"},
        {"SumD2_6","Skjutmått"},
        {"SumD2_7","Skjutmått"},
    };
        }
        private Dictionary<string, string> MissingVariablesFilled() => new Dictionary<string, string>
        {
            {"SumRa25","2.5"},
            {"Sum60","60°"},
            {"Sum60a","60°"},
            {"Sum30a","20°"},
            {"SumRitNrS1","KMTA 5-40:6"},
            {"SumRitNrS2","KMTA 5-40:6  Stämplas enl. ritning: 7430920 alt. 7430189"}
        };
    }
}
