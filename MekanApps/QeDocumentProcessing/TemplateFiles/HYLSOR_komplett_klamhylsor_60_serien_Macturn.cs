
using System;
using System.Collections.Generic;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_komplett_klamhylsor_60_serien_Macturn : ITemplateCalculations
    {
        private string machineNumber = string.Empty;
        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var result = new Dictionary<string, string>();
            machineNumber = request?.MachineNumber ?? string.Empty;
            var subject = (request?.ProductDesignation ?? "").ToUpper();

            bool is3160 = subject.Contains("3160") || subject.Contains("31");
            bool is3060 = subject.Contains("3060") || subject.Contains("30");

            Merge(result, Header(is3160));
            Merge(result, OP1(is3160));
            Merge(result, OP2(is3160));
            Merge(result, Threading());
            Merge(result, OilHole(is3160));
            Merge(result, Slits(is3160));

            return result;
        }

        private Dictionary<string, string> Header(bool is3160)
        {
            return new Dictionary<string, string>
            {
                {"SumMaskinValS1",$"Maskin: {machineNumber} - OP1"},
                {"SumRa","2.5 [2F]"},
                {"Sumd2", is3160 ? "(d2) 314" : "(d2) 310.5"},
                {"SumKona","Kona 1:12"},
                {"SumGTjaTol","+ 0.055"},
                {"SumGTjaTolN","- 0.160"},
                {"SumRitningsnrS2", is3160 ? "7438958:senaste utg." : "7436044:senaste utg."},
                {"SumRitTolS2","Toleranser: 1432012:7"},
                {"SumKlEgenskaperS2","PRODUCTION NUTS & SLEEVES & HOUSINGS\\ALLMÄNKLASSADE EGENSKAPER\\Klassade egenskaper klämhylsor"}
            };
        }

        private Dictionary<string, string> OP1(bool is3160)
        {
            return new Dictionary<string, string>
            {
                {"SumL1", is3160 ? "140" : "108"},
                {"SumE2", is3160 ? "15,292" : "14,875"},
                {"SumRa1","2.5 [2F]"},
                {"Sumd1","(d1) 280"},
                {"Sumd1Tol","± 0.065 [3F]"},
                {"SumRa5","5 [2F]"},
                {"Sumr","R2.5"},
                {"SumE1", is3160 ? "11,125" : "10,708"},
                {"SumL2", is3160 ? "40" : "8"},
                {"SumGTjTol","+ 0.055"},
                {"SumGTjTolN","- 0.160"},

                {"SumF4_1","1/2"},{"SumD4_1","Mikrometerstickmått"},{"SumAF4_1",""},
                {"SumF4_2","1/2"},{"SumD4_2","Skjutmått"},{"SumAF4_2",""},
                {"SumF4_3","1/1"},{"SumD4_3", is3160 ? "Mätbygel SR 7415991" : "Mätbygel SR 7419471"},{"SumAF4_3","Tol:"},
                {"SumF4_4","1/1"},{"SumD4_4", is3160 ? "Mätbygel SR 7415991" : "Mätbygel SR 7419471"},{"SumAF4_4","Tol: 0.025 [2F]"},
                {"SumF4_5","1/1"},{"SumD4_5", is3160 ? "Mätbygel SR 7415991" : "Mätbygel SR 7419471"},{"SumAF4_5","Tol: 0.015 [2F] Mätlängd=100"},
                {"SumF4_6","Inst."},{"SumD4_6","Mätmaskin"},{"SumAF4_6","Max: 0.065 [3F]"},
                {"SumF4_7","1/5"},{"SumD4_7","Egglinjal"},{"SumAF4_7","Max: 0.012"},
                {"SumF4_8","1/5"},{"SumD4_8","Egglinjal"},{"SumAF4_8","Max: 0.018"},
                {"SumF4_9","1/1"},{"SumD4_9","Okulärkontroll"},{"SumAF4_9","Vid misstänkt fel Ra-mätare"},
                {"SumTextS2","Bryt alla kanter, avlägsna. Okulärkontroll märkning, gjuteridefekter, grader & slagmärken"},
                {"SumMaskinValS2",$"Maskin: {machineNumber} - OP2"}
            };
        }

        private Dictionary<string, string> OP2(bool is3160)
        {
            return new Dictionary<string, string>
            {
                {"SumP1","(P) 4"},
                {"Sumdm","(dm) 298"},
                {"SumdmTol","- 0.190 [3F]"},
                {"SumdmTolN","- 0.630 [3F]"},
                {"Sumd3","(d3) 295,5"},
                {"Sumd3Tol","+ 0.000"},
                {"Sumd3TolN","- 0.750"},
                {"SumdaOP1","(d) 300"},
                {"SumdaOP1Tol","+ 0.000"},
                {"SumdaOP1TolN","- 0.300"},
                {"Sumg","(g) 2.7x45º"},
                {"Sumr1","R1"},
                {"SumGänga","Tr300x4"},
                {"SumSL", is3160 ? "(SL) 54" : "(SL) 56"},
                {"SumSLTol","± 0.300"},
                {"Sumb", is3160 ? "(b) 50" : "(b) 52"},
                {"SumbTol", is3160 ? "+ 2.500 [3F]" : "+ 3.000 [3F]"},
                {"SumbTolN","- 0.000 [2F]"},
                {"SumRitningsnrS1", is3160 ? "7438958:senaste utg." : "7436044:senaste utg."},
                {"SumRitGänga","Gänga: 237359:2, 7430181:2"},
                {"SumRitTolS1","Toleranser: 1432012:7"},
                {"SumKlEgenskaperS1","PRODUCTION NUTS & SLEEVES & HOUSINGS\\ALLMÄNKLASSADE EGENSKAPER\\Klassade egenskaper klämhylsor"},
                {"SumL", is3160 ? "(L) 208" : "(L) 168"},
                {"SumLTol","+ 0.000 [3F]"},
                {"SumLTolN", is3160 ? "- 1.850 [3F]" : "- 1.600 [3F]"},
                {"Sumd","(d) 300"},
                {"SumdTol","+ 0.000"},
                {"SumdTolN","- 0.300"}
            };
        }

        private Dictionary<string, string> Threading()
        {
            return new Dictionary<string, string>
            {
                {"SumF3_1","1/5"},{"SumD3_1","Skjutmått"},{"SumAF3_1",""},
                {"SumF3_2","1/2"},{"SumD3_2","Skjutmått"},{"SumAF3_2",""},
                {"SumF3_3","1/1"},{"SumD3_3","Multimar med 4mm rullar"},{"SumAF3_3","Kontrolleras med klove utf.2"},
                {"SumF3_4","1/5"},{"SumD3_4","Djupmått"},{"SumAF3_4",""},
                {"SumF3_5","Inst."},{"SumD3_5","Radielyra"},{"SumAF3_5",""},
                {"SumF3_6","1/5"},{"SumD3_6","Skjutmått/Vinkelmätare"},{"SumAF3_6",""},
                {"SumF3_7","1/2"},{"SumD3_7","Gängmall Tr4"},{"SumAF3_7",""},
                {"SumF3_8","1/5"},{"SumD3_8","Skjutmått"},{"SumAF3_8","Hjälpmått"},
                {"SumTextS1","Kontrollera rätt märkning <<LineBreak>><<LineBreak>> Okulärkontroll gjuteridefekter, grader & slagmärken <<LineBreak>><<LineBreak>> Gjutgodsdefekter: 7433015"}
            };
        }

        private Dictionary<string, string> OilHole(bool is3160)
        {
            return new Dictionary<string, string>
            {
                {"SumMaskinValS3",$"Maskin: {machineNumber} - BorrOljehål & Oljespår"},
                {"SumF2_1","1/2"},{"SumD2_1","pipborr/djupmått"},{"SumAF2_1",""},
                {"SumF2_2","1/2"},{"SumD2_2","Skjutmått"},{"SumAF2_2",""},
                {"SumF2_3","1/2"},{"SumD2_3","Skjutmått"},{"SumAF2_3",""},
                {"SumF2_4","1/2"},{"SumD2_4","Gängtolk"},{"SumAF2_4",""},
                {"SumF2_5","1/2"},{"SumD2_5","Skjutmått"},{"SumAF2_5",""},
                {"SumF2_6","1/2"},{"SumD2_6","Skjutmått/fasmall"},{"SumAF2_6",""},
                {"SumF2_7","1/2"},{"SumD2_7","Skjutmått"},{"SumAF2_7",""},
                {"SumF2_8","1/2"},{"SumD2_8","Skjutmått"},{"SumAF2_8",""},
                {"SumF2_9","1/2"},{"SumD2_9","pipborr/djupmått"},{"SumAF2_9",""},
                {"SumF2_0","1/2"},{"SumD2_0","Skjutmått"},{"SumAF2_0",""},
                {"SumF2_11","1/2"},{"SumD2_11","Radieyra"},{"SumAF2_11",""},

                {"SumTextS3","Grader, frifläckar, slagmärken, repor, valkar och andra defekter samt ojämnheter kontrolleras med ögonmått."},
                {"SumRitNr", is3160 ? "7434152" : "7434151"},
                {"SumÖvrigt","1 st. oljeborrhål 180º från slits 1 st. utv. oljespår med början 10º från slitscentrum"},
                {"SumJ", is3160 ? "(J) 112" : "(J) 95"},{"SumJTol","± 0.3"},
                {"SumE", is3160 ? "(E) 117" : "(E) 100"},{"SumETol","± 0.3"},
                {"SumS","(D) 3"},{"SumSTol","± 0.1"},
                {"SumG1","M6"},
                {"SumT","(T) 6,3"},{"SumTTol","± 0.2"},
                {"SumH","(H) 1"},{"SumHTol","± 0.1"},
                {"SumF","(F) 3"},{"SumFTol","± 0.1"},
                {"SumN","(N) 5,3"},{"SumNTol","± 0.1"},
                {"SumC1","(C) 10"},{"SumC1Tol","± 0.2"},
                {"SumV120","120º"},
                {"SumV45","45º"},
                {"SumTextGängstigning","Max 2x gängstigning"},
                {"SumR7","R1"},
                {"SumR8","R4"},
                {"SumB1","(B) 3,9"},
                {"SumB1Tol","0"},
                {"SumB1TolN","- 0.1"}
            };
        }

        private Dictionary<string, string> Slits(bool is3160)
        {
            return new Dictionary<string, string>
            {
                {"SumMaskinValS5",$"Maskin: {machineNumber} - Muttersäkring, Slits"},
                {"SumF1_1","1/2"},{"SumD1_1","Skjutmått"},{"SumAF1_1",""},
                {"SumF1_2","1/2"},{"SumD1_2","Skjutmått"},{"SumAF1_2",""},
                {"SumF1_3","1/2"},{"SumD1_3","Skjutmått"},{"SumAF1_3",""},
                {"SumF1_4","1/2"},{"SumD1_4",""},{"SumAF1_4",""},
                {"SumF1_5","1/2"},{"SumD1_5","Skjutmått"},{"SumAF1_5",""},
                {"SumTextS4",""},
                {"Sumd10","(d1) 280"},
                {"Sumd10Tol","+ 0.210"},
                {"Sumd10TolN","- 0.320"},
                {"SumV1","11,25º"},
                {"SumC", is3160 ? "(B) 4" : "(B) 8"},
                {"SumCTol","± 0.2"},
                {"SumE5","(e) 24"},
                {"SumVa","11,25º"},
                {"SumF02","(f) 22"},
                {"SumF02Tol","+ 2.1"},
                {"SumF02TolN","0 [3F]"},
                {"SumF0","(f) 22"},
                {"SumV30","30º"},
                {"SumF0Tol","+ 2.1"},
                {"SumF0TolN","0 [3F]"},
                {"SumE5Tol","+ 0.520"},
                {"SumE5TolN","0 [3F]"},
                {"SumRitNr4", is3160 ? "7438958" : "7438957"}
            };
        }

        private void Merge(Dictionary<string, string> target, Dictionary<string, string> source)
        {
            foreach (var kv in source)
            {
                if (!target.ContainsKey(kv.Key))
                    target[kv.Key] = kv.Value ?? "";
            }
        }
    }
}