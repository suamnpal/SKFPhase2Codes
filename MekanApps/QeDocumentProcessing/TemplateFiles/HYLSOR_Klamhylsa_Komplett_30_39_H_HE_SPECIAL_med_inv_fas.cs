using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Klamhylsa_Komplett_30_39_H_HE_SPECIAL_med_inv_fas : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var kv = new Dictionary<string, string>();

            string machine = string.IsNullOrWhiteSpace(request.MachineNumber) ? "" : request.MachineNumber.Trim();

            Merge(kv, GetAdmin(machine));
            Merge(kv, GetOp1());
            Merge(kv, GetOp2());
            Merge(kv, GetOp3());
            Merge(kv, GetOp4());
            Merge(kv, GetMeasurements(machine));
            Merge(kv, GetAdditionalPdfVariables());
            Merge(kv, GetDefaults());

            return kv;
        }

        private void Merge(Dictionary<string, string> t, Dictionary<string, string> s)
        {
            foreach (var x in s)
                t[x.Key] = x.Value ?? "";
        }

        private Dictionary<string, string> GetAdmin(string machine) => new()
        {
            ["SumRitningsnrS1"] = "MS-OH 3068/315 H/V21:senaste utg.",
            ["SumRitningsnrS2"] = "MS-OH 3068/315 H/V21:senaste utg.",
            ["SumRitNr"] = "MS-OH 3068/315 H/V21",
            ["SumRitNr4"] = "MS-OH 3068/315 H/V21",

            ["SumMaskinValS1"] = $"Maskin: {machine} - OP1",
            ["SumMaskinValS2"] = $"Maskin: {machine} - OP2",
            ["SumMaskinValS3"] = $"Maskin: {machine} - BorrOljehål & Oljespår",
            ["SumMaskinValS5"] = $"Maskin: {machine} - Muttersäkring, Slits",

            ["SumTextS1"] = "Kontrollera rätt märkning Okulärkontroll gjuteridefekter, grader & slagmärken Gjutgodsdefekter: 7433015",
            ["SumTextS2"] = "Bryt alla kanter, avlägsna. Okulärkontroll märkning, gjuteridefekter, grader & slagmärken",
            ["SumTextS3"] = "Grader, frifläckar, slagmärken, repor, valkar och andra defekter samt ojämnheter kontrolleras med ögonmått.",
            ["SumTextS4"] = "",


            ["Sumg"] = "(g) 3.2x45º",
            ["Sumr1"] = "R1",

            ["SumdaOP1"] = "(d) 340",
            ["SumdaOP1Tol"] = "+ 0.000",
            ["SumdaOP1TolN"] = "- 0.335",

            ["SumP1"] = "(P) 5",

            ["SumÖvrigt"] = "1 st. oljeborrhål 180º från slits 1 st. utv. oljespår med början 10º från slitscentrum",
            ["SumTextGängstigning"] = "Max 2x gängstigning",

            ["SumG1"] = "M6",
            ["SumR7"] = "R1",

            ["SumF02"] = "(f) 26",
            ["SumF02Tol"] = "+ 2.1",
            ["SumF02TolN"] = "0",

            ["SumSTol"] = "± 0.1"

        };

        private Dictionary<string, string> GetOp1() => new()
        {
            ["Sumd1"] = "(d1) 315",
            ["Sumd1Tol"] = "± 0.065 [3F]",
            ["Sumd2"] = "(d2) 351.833",
                        
            ["Sumr"] = "3x45º",
            ["SumRa"] = "2.5 [2F]",
            ["SumRa1"] = "2.5 [2F]",
            ["SumRa5"] = "5 [2F]",

            ["SumKona"] = "Kona 1:12",

            ["SumL1"] = "108",
            ["SumL2"] = "8",
            ["SumE1"] = "13,875",
            ["SumE2"] = "18,042",

            ["SumGTjTol"] = "+ 0.060",
            ["SumGTjTolN"] = "- 0.175"
        };

        private Dictionary<string, string> GetOp2() => new()
        {
            ["Sumd"] = "(d) 340",
            ["SumdTol"] = "+ 0.000",
            ["SumdTolN"] = "- 0.335",

            ["Sumdm"] = "(dm) 337.5",
            ["SumdmTol"] = "- 0.212 [3F]",
            ["SumdmTolN"] = "- 0.710 [3F]",

            ["Sumd3"] = "(d3) 334.5",
            ["Sumd3Tol"] = "+ 0.000",
            ["Sumd3TolN"] = "- 0.850",

            ["SumGänga"] = "Tr340x5",
            ["SumP"] = "(P) 5",

            ["Sumb"] = "(b) 56",
            ["SumbTol"] = "+ 3.000 [3F]",
            ["SumbTolN"] = "- 0.000 [2F]",

            ["SumSL"] = "(SL) 60",
            ["SumSLTol"] = "± 0.300",
            ["SumL"] = "(L) 187",

            ["SumLTol"] = "+ 0.000 [3F]",
            ["SumLTolN"] = "- 1.850 [3F]",
        };

        private Dictionary<string, string> GetOp3() => new()
        {
            ["SumE"] = "(E) 110",
            ["SumETol"] = "± 0.3",

            ["SumD"] = "(D) 3",
            ["SumSTol"] = "± 0.1",

            ["SumF"] = "(F) 3",
            ["SumFTol"] = "± 0.1",

            ["SumC1"] = "(C) 10",
            ["SumT"] = "(T) 6.3",

            ["SumB"] = "(B) 3.5",

            ["SumH"] = "(H) 1.257",
            ["SumHTol"] = "± 0.1",

            ["SumJ"] = "(J) 105",
            ["SumJTol"] = "± 0.3",

            ["SumN"] = "(N) 6.225",
            ["SumNTol"] = "± 0.1"
        };

        private Dictionary<string, string> GetOp4() => new()
        {
            ["SumC"] = "(c) 8",
            ["SumCTol"] = "± 0.2",

            ["SumE5"] = "(e) 24",

            ["SumF0"] = "(f) 26",
            ["SumF0Tol"] = "+ 2.1",
            ["SumF0TolN"] = "0",

            ["Sumd10"] = "(d1) 315",
            ["Sumd10Tol"] = "+ 0.400",
            ["Sumd10TolN"] = "- 0.630"
        };

        private Dictionary<string, string> GetMeasurements(string machine)
        {
            return new Dictionary<string, string>
            {
                ["SumF4_1"] = "1/2",
                ["SumF4_2"] = "1/2",
                ["SumF4_3"] = "1/1",
                ["SumF4_4"] = "1/1",
                ["SumF4_5"] = "1/1",
                ["SumF4_6"] = "Inst",
                ["SumF4_7"] = "1/5",
                ["SumF4_8"] = "1/5",
                ["SumF4_9"] = "1/1",

                ["SumD4_1"] = "Mikrometerstickmått",
                ["SumD4_2"] = "Skjutmått" ,
                ["SumD4_3"] = "Mätbygel SR 7419471",
                ["SumD4_4"] = "Mätbygel SR 7419471",
                ["SumD4_5"] = "Mätbygel SR 7419471",
                ["SumD4_6"] = "Mätmaskin",
                ["SumD4_7"] = "Egglinjal",
                ["SumD4_8"] = "Egglinjal",
                ["SumD4_9"] = "Okulärkontroll",

                ["SumAF4_1"] = "",
                ["SumAF4_2"] = "",

                ["SumAF4_3"] = "Tol:",
                ["SumGTjTol"] = "+ 0.060",
                ["SumGTjTolN"] = "- 0.175",

                ["SumAF4_4"] = "Tol: 0.030 [2F]",

                ["SumAF4_5"] = "Tol: 0.015 [2F] Mätlängd=100",

                ["SumAF4_6"] = "Max: 0.065 [3F]",

                ["SumAF4_7"] = "Max: 0.012",
                ["SumAF4_8"] = "Max: 0.018",
                ["SumAF4_9"] = "Vid misstänkt fel Ra-mätare",

                ["SumF3_1"] ="1/5" ,
                ["SumF3_2"] ="1/2" ,
                ["SumF3_3"] ="1/1" ,
                ["SumF3_4"] ="1/5" ,
                ["SumF3_5"] = "Inst.",
                ["SumF3_6"] ="1/5" ,
                ["SumF3_7"] ="1/2" ,
                ["SumF3_8"] ="1/5" ,

                ["SumD3_1"] ="Skjutmått",
                ["SumD3_2"] ="Skjutmått",
                ["SumD3_3"] ="Multimar med 5mm rullar",
                ["SumD3_4"] ="Djupmått",
                ["SumD3_5"] ="Radielyra",
                ["SumD3_6"] ="Skjutmått/Vinkelmätare",
                ["SumD3_7"] ="Gängmall Tr5",
                ["SumD3_8"] ="Skjutmått",

                ["SumAF3_1"] = "",
                ["SumAF3_2"] = "",
                ["SumAF3_3"] = "Kontrolleras med klove utf.2",
                ["SumAF3_4"] = "",
                ["SumAF3_5"] = "",
                ["SumAF3_6"] = "",
                ["SumAF3_7"] = "",
                ["SumAF3_8"] = "Hjälpmått",

                ["SumF2_1"] = "1/2" ,
                ["SumF2_2"] = "1/2" ,
                ["SumF2_3"] = "1/2" ,
                ["SumF2_4"] = "1/2" ,
                ["SumF2_5"] = "1/2" ,
                ["SumF2_6"] = "1/2" ,
                ["SumF2_7"] = "1/2" ,
                ["SumF2_8"] = "1/2" ,
                ["SumF2_9"] = "1/2" ,
                ["SumF2_0"] = "1/2" ,
                ["SumF2_11"] = "1/2",

                ["SumD2_1"] = "pipborr/djupmått",
                ["SumD2_2"] = "Skjutmått" ,
                ["SumD2_3"] = "Skjutmått",
                ["SumD2_4"] = "Gängtolk" ,
                ["SumD2_5"] = "Skjutmått" ,
                ["SumD2_6"] = "Skjutmått/fasmall",
                ["SumD2_7"] = "Skjutmått" ,
                ["SumD2_8"] = "Skjutmått" ,
                ["SumD2_9"] = "pipborr/djupmått" ,
                ["SumD2_0"] = "Skjutmått" ,
                ["SumD2_11"] = "Radieyra" ,

                ["SumAF2_1"] = "",
                ["SumAF2_2"] = "",
                ["SumAF2_3"] = "",
                ["SumAF2_4"] = "",
                ["SumAF2_5"] = "",
                ["SumAF2_6"] = "",
                ["SumAF2_7"] = "",
                ["SumAF2_8"] = "",
                ["SumAF2_9"] = "",
                ["SumAF2_0"] = "",
                ["SumAF2_11"] = "",

                ["SumF1_1"] =  "1/2",
                ["SumF1_2"] =  "1/2",
                ["SumF1_3"] =  "1/2",
                ["SumF1_4"] =  "1/2",
                ["SumF1_5"] =  "1/2",
                      
                ["SumD1_1"] =  "Skjutmått",
                ["SumD1_2"] =  "Skjutmått",
                ["SumD1_3"] =  "Skjutmått",
                ["SumD1_4"] = "",
                ["SumD1_5"] = "Skjutmått",
              
                ["SumAF1_1"] = "",
                ["SumAF1_2"] = "",
                ["SumAF1_3"] = "",
                ["SumAF1_4"] = "",
                ["SumAF1_5"] = "",
            };
        }

        private Dictionary<string, string> GetDefaults() => new()
        {
            ["SumGTjaTol"] = "+ 0.060",
            ["SumGTjaTolN"] = "- 0.175"
        };

        private Dictionary<string, string> GetAdditionalPdfVariables() => new()
        {
            
            ["SumRitTolS2"] = "Toleranser: 1432012:7",
            ["SumKlEgenskaperS1"] =
    "PRODUCTION NUTS & SLEEVES & HOUSINGS\nALLMÄNKLASSADE EGENSKAPER\nKlassade egenskaper klämhylsor",

            ["SumKlEgenskaperS2"] =
    "PRODUCTION NUTS & SLEEVES & HOUSINGS\nALLMÄNKLASSADE EGENSKAPER\nKlassade egenskaper klämhylsor",

            ["SumV45"] = "45",
            ["SumV120"] = "120º",

            ["SumTextGängstigning"] = "Max 2x gängstigning",

            ["SumC1Tol"] = "± 0.2",
            ["SumTTol"] = "± 0.2",

            ["SumVa"] = "11.25º",
            ["SumV1"] = "11.25º",
            ["SumV30"] = "30",

            ["SumF02"] = "(f) 26",
            ["SumF02Tol"] = "+ 2.1",
            ["SumF02TolN"] = "0 [3F]",

            ["SumRitTolS1"] = "Toleranser: 1432012:7",
            ["SumRitTolS2"] = "Toleranser: 1432012:7",

            ["SumRitGänga"] = "Gänga: 237359:2, 7430181:2",


            ["SumR8"] = "R4.5",
            ["SumR7"] = "R1",

            ["SumB1"] = "(B) 3.5",
            ["SumB1Tol"] = "0",
            ["SumB1TolN"] = "-0.1",

            ["SumS"] = "(D) 3",
            ["SumSTol"] = "± 0.1",

            ["SumE5Tol"] = "+ 0.520",
            ["SumE5TolN"] = "0 [3F]",
        };
    }
}