using DocumentFormat.OpenXml.Bibliography;
using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Avdragshylsor_60_1060_special_VZ413_MacTurn_550_VTR_160 : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var kv = CreateAllKeys();

            string machineRaw = (request.MachineNumber ?? "").Trim().ToUpperInvariant();

            string machine =
                machineRaw == "VTR-160" ? "VTR-160" :
                machineRaw == "MACTURN 550" ? "MacTurn 550" :
                "";

            kv["DocumentUniqueId"] =
                DateTime.UtcNow.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);

            Merge(kv, GetAdmin());
            Merge(kv, GetMachine(machine));
            Merge(kv, GetGeometry());
            Merge(kv, GetTolerances());
            Merge(kv, GetThread());
            Merge(kv, GetOilHole());
            Merge(kv, GetPage1());
            Merge(kv, GetPage2());
            Merge(kv, GetPage3(machine));
            Merge(kv, GetDerived());

            kv["Sumd2a"] = kv["Sumd2"];
            kv["Sumd2aTol"] = kv["Sumd2Tol"];
            kv["Sumd2aTolN"] = kv["Sumd2TolN"];

            return kv;
        }

        private Dictionary<string, string> CreateAllKeys()
        {
            var kv = new Dictionary<string, string>();

            string[] keys =
            {
                "Sumd2","Sumd2Tol","Sumd2TolN","Sumd2a","Sumd2aTol","Sumd2aTolN",
                "Sumdm","SumdmTol","SumdmTolN",
                "Sumd4","Sumd4Tol","Sumd4TolN",
                "Sumd1","Sumd1Tol","Sumd1TolN","Sumd1Toles","Sumd1TolNes",
                "SumL","SumLTol","SumLTolN",
                "Sumb2","Sumb2Tol",
                "SumHm","SumHmTol","SumHmTolN",
                "Sumdl","Sumds",
                "SumMaskinVal","SumMaskinValS2","SumMaskinValS3",
                "SumP","SumGänga","SumSg","Sumß",
                "SumA","SumDB","SumC","SumZ",
                "SumRa","SumRa1","SumRa5",
                "SumR15","SumR15a","Sumr1",
                "SumRakA","SumRakB","SumKs","SumRd",
                "SumML","SumBML","SumL1","SumL2","SumE1","SumE2",
                "SumVink1","SumVink2","SumVink3",
                "SumLsida3","SumLsida3Tol","SumL1sida3","SumL1sida3Tol",
                "SumGsida3","SumD1sida3",
                "Sumh","Sumg1","Sumg2","SumC2",
                "SumR2","SumBorrHAnt","SumG",
                "SumLSSK","Sumb2SSK",
                "SumF1_1","SumF1_2","SumF1_3","SumF1_4","SumF1_5","SumF1_6","SumF1_7","SumF1_8","SumF1_9","SumF1_0",
                "SumD1_1","SumD1_2","SumD1_3","SumD1_4","SumD1_5","SumD1_6","SumD1_7","SumD1_8","SumD1_9","SumD1_0",
                "SumAF1_1","SumAF1_2","SumAF1_3","SumAF1_4","SumAF1_5","SumAF1_6","SumAF1_7","SumAF1_8","SumAF1_9","SumAF1_0",
                "SumF2_1","SumF2_2","SumF2_3","SumF2_4","SumF2_5","SumF2_6","SumF2_7","SumF2_8","SumF2_9","SumF2_0",
                "SumD2_1","SumD2_2","SumD2_3","SumD2_4","SumD2_5","SumD2_6","SumD2_7","SumD2_8","SumD2_9","SumD2_0",
                "SumAF2_1","SumAF2_2","SumAF2_3","SumAF2_4","SumAF2_5","SumAF2_6","SumAF2_7","SumAF2_8","SumAF2_9","SumAF2_0",
                "SumF2_1sida3","SumF2_2sida3","SumD2_1sida3","SumD2_2sida3",
                "SumRitNr","SumRitNr2","SumRitNr3",
                "SumTolRit","SumTolRit2","SumTolRit3",
                "SumGängRit","SumGGD",
                "SumKlEgenskaperS1","SumKlEgenskaperS2","SumKlEgenskaperS3",
                "SumT","SumS",
                "SumTextS1","SumTextS2","SumTextS3",

                "SumV","SumKona",
                "SumBx","SumBxTol","SumB","SumBTol",
                "SumNn","Sumn","SumJ","SumK","SumEE","SumF","SumM","SumGH",
                "SumR",
            };

            foreach (var k in keys)
                kv[k] = "";

            return kv;
        }

        private Dictionary<string, string> GetAdmin()
        {
            string klass = "PRODUCTION NUTS & SLEEVES & HOUSINGS\\ALLMÄNKLASSADE EGENSKAPER\\Klassade egenskaper Avdragshylsor";
            return new Dictionary<string, string>
            {

                ["SumRitNr"] = "7437377",
                ["SumRitNr2"] = "7437377",
                ["SumRitNr3"] = "VZ 413",

                ["SumTolRit"] = "1432011:6, 7437495:4",
                ["SumTolRit2"] = "1432011:6, 7437495:4",
                ["SumTolRit3"] = "1432011:6, 7437495:4",

                ["SumGängRit"] = "237359:3, 7430181:2",
                ["SumGGD"] = "7433015",

                ["SumKlEgenskaperS1"] = klass,
                ["SumKlEgenskaperS2"] = klass,
                ["SumKlEgenskaperS3"] = klass,

                ["SumT"] = "(T) 13.5",
                ["SumS"] = "(S) 15",

                ["SumRa"] = "2,5 [2F]",
                ["SumRa1"] = "2,5 [2F]",
                ["SumRa5"] = "5 [2F]",
                ["SumR15"] = "R 1,5",
                ["SumR15a"] = "R 1,5",
                ["Sumr1"] = "R8",
                
            };
        }

        private Dictionary<string, string> GetMachine(string m)
        {
            var kv = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(m))
            {
                kv["SumMaskinVal"] = $"Maskin: {m} - OP1";
                kv["SumMaskinValS2"] = $"Maskin: {m} - OP2";
                kv["SumMaskinValS3"] = $"Maskin: {m} - OP1";
            }
            return kv;
        }

        private Dictionary<string, string> GetGeometry()
        {
            return new Dictionary<string, string>
            {
                ["Sumd2"] = "(d2) 900",
                ["Sumdm"] = "(dm) 896,5",
                ["Sumd4"] = "(d4) 870",
                ["Sumd1"] = "(d1) 800",
                ["SumL"] = "(L) 325",
                ["Sumb2"] = "(b) 272",
                ["SumHm"] = "(Hm) 53",
                ["Sumdl"] = "Minsta kondiameter (dl): 851,500",
                ["Sumds"] = "Största kondiameter (ds): 872,167",
                ["SumZ"] = "(Z) Ø796",
                ["SumKs"] = "0.140",
                ["SumRd"] = "0.100",
                ["SumRakA"] = "0.016",
                ["SumRakB"] = "0.024",


            };
        }

        private Dictionary<string, string> GetTolerances()
        {
            return new Dictionary<string, string>
            {

                ["Sumd2Tol"] = "+ 0.0",
                ["Sumd2TolN"] = "- 0.425",
                ["SumdmTol"] = "- 0.250 [3F]",
                ["SumdmTolN"] = "- 0.850 [3F]",
                ["Sumd4Tol"] = "+ 0.000",
                ["Sumd4TolN"] = "- 0.300",
                ["SumLTol"] = "+ 0.0 [3F]",
                ["SumLTolN"] = "- 0.890 [3F]",
                ["Sumb2Tol"] = "± 1.050 [3F]",
                ["SumHmTol"] = "+ 1.05",
                ["SumHmTolN"] = "- 1.94",
                ["Sumd1Tol"] = "+ 0.100 [3F]",
                ["Sumd1TolN"] = "- 0.100 [3F]",
                ["Sumd1Toles"] = "+ 0.500 [3F]",
                ["Sumd1TolNes"] = "- 0.800 [3F]",
                ["SumExTol"] = "+ 0.130 [3F]",
                ["SumExTolN"] = "- 0.0 [2F]",
                ["SumFTol"] = "+ 0.000",
                ["SumFTolN"] = "- 0.200",
                ["SumKonavv"] = "0.010 [2F]",
                ["SumKr"] = "+ 0.045 [2F]",


            };
        }

        private Dictionary<string, string> GetThread()
        {
            return new Dictionary<string, string>
            {
                ["SumP"] = "(P) 7,0",
                ["SumGänga"] = "Tr 900x 7,0",
                ["SumSg"] = "(Sg) min:12"
            };
        }

        private Dictionary<string, string> GetOilHole()
        {
            return new Dictionary<string, string>
            {
                ["SumA"] = "(A) 15",
                ["SumDB"] = "(DB) 830",
                ["SumC"] = "(C) 226",
                ["SumG"] = "(G) 8",
                ["Sumh"] = "(h) 24",
                ["Sumg1"] = "4,4x45º",
                ["Sumg2"] = "4,4x45º",
                ["SumC2"] = "(C2) 10",
                ["SumR2"] = "G1/4",
                ["SumBorrHAnt"] = "",
                ["SumLSSK"] = "(L) ~329",
                ["Sumb2SSK"] = "(b) ~276",
                ["SumKOSinv"] = "138,9",
                ["SumKOSutv"] = "149,4"                
            };
        }

        private Dictionary<string, string> GetPage1()
        {
            return new Dictionary<string, string>
            {
                ["SumF1_1"] = "1/1",
                ["SumD1_1"] = "Skjutmått",
                ["SumF1_2"] = "1/1",
                ["SumD1_2"] = "Multimar",
                ["SumAF1_2"] = "Rullar: Tr 7,0, inställd med klove",
                ["SumF1_3"] = "Inst.",
                ["SumD1_3"] = "Gängmall",
                ["SumF1_4"] = "1/1",
                ["SumD1_4"] = "Skjutmått",
                ["SumF1_5"] = "1/1",
                ["SumD1_5"] = "Gängtolk",
                ["SumF1_6"] = "Inst.",
                ["SumD1_6"] = "Skjutmått",
                ["SumF1_7"] = "Inst.",
                ["SumD1_7"] = "Skjutmått",
                ["SumF1_8"] = "Inst.",
                ["SumD1_8"] = "Skjutmått",
                ["SumAF2_6"] = "GodstjockleksTOL:",
                ["SumAF2_7"] = "Tolerans: 0,010 [2F]",
                ["SumAF2_8"] = "Max variation: 0,045",
                ["SumAF2_9"] = "Vid misstänkt fel Ra-mätare",
                ["SumAF2_0"] = "Vid misstänkt formfel lämna till mätrum",
                ["SumBorrhålsgänga"] = "Borrhålsgänga"
            };
        }

        private Dictionary<string, string> GetPage2()
        {
            return new Dictionary<string, string>
            {
                ["SumF2_1"] = "1/1",
                ["SumD2_1"] = "Klove",
                ["SumAF2_1"] = "TOL efter slits:",
                ["SumF2_2"] = "1/1",
                ["SumD2_2"] = "Skjutmått",
                ["SumF2_3"] = "1/1",
                ["SumD2_3"] = "Skjutmått",
                ["SumF2_4"] = "1/1",
                ["SumD2_4"] = "Skjutmått",
                ["SumF2_5"] = "Inst.",
                ["SumD2_5"] = "Egglinjal",
                ["SumF2_6"] = "1/1",
                ["SumD2_6"] = "Mätbygel: SR 7419470 el. 7415991",
                ["SumF2_7"] = "1/1",
                ["SumD2_7"] = "Mätbygel: SR 7419470 el. 7415991",
                ["SumF2_8"] = "1/1",
                ["SumD2_8"] = "Mätbygel: SR 7419470 el. 7415991",
                ["SumF2_9"] = "1/1",
                ["SumD2_9"] = "Okulärkontroll",
                ["SumF2_0"] = "vid behov",
                ["SumD2_0"] = "Mätmaskin"
            };
        }

        private Dictionary<string, string> GetPage3(string m)
        {
            return new Dictionary<string, string>
            {
                ["SumVink1"] = "90°",
                ["SumVink2"] = "45°",
                ["SumVink3"] = "60° (6x)",
                ["SumLsida3"] = "35",
                ["SumLsida3Tol"] = "±0.3",
                ["SumL1sida3"] = "28",
                ["SumL1sida3Tol"] = "±0.2",
                ["SumGsida3"] = "17",
                ["SumD1sida3"] = "(G1) M16",
                ["SumF2_1sida3"] = "1/6",
                ["SumF2_2sida3"] = "1/6",
                ["SumD2_1sida3"] = "Gängtolk",
                ["SumD2_2sida3"] = "Skjutmått",
                ["SumL1"]="40",
                ["SumL2"]="140",
                ["SumE1"] ="27,417",
                ["SumE2"] ="31,583",
                ["SumAAdia"]= "Ø 840",
                ["SumAAdiaTol"] = "±0.5",

            };
        }

        private void Merge(Dictionary<string, string> t, Dictionary<string, string> s)
        {
            foreach (var kv in s)
                t[kv.Key] = kv.Value ?? "";
        }

        private Dictionary<string, string> GetDerived()
        {
            return new Dictionary<string, string>
            {
                ["SumTextS1"] = "Rätt märkning samt övriga mått kontrolleras vid inställning.<<LineBreak>> Okulär kontroll av Grader, gjuteridefekter, frifläckar, slagmärken, repor, valkar etc.",
                ["SumTextS2"] = "Rätt märkning samt övriga mått kontrolleras vid inställning.<<LineBreak>> Okulär kontroll av Grader, gjuteridefekter, frifläckar, slagmärken, repor, valkar etc.",
                ["SumTextS3"] = "Rätt märkning samt övriga mått kontrolleras vid inställning.<<LineBreak>> Okulär kontroll av Grader, gjuteridefekter, frifläckar, slagmärken, repor, valkar etc.",

                ["SumV"] = "30º",
                ["SumKona"] = "Kona 1:12",

                ["SumBx"] = "(B) 104",
                ["SumBxTol"] = "± 0.5",
                ["SumB"] = "(B) 221",
                ["SumBTol"] = "± 0,5",

                ["SumNn"] = "(Nn) 0.5",
                ["Sumn"] = "(n) 26st.",
                ["SumJ"] = "(J) 41",
                ["SumK"] = "(K) 136",
                ["SumEE"] = "(EE) 12",
                ["SumF"] = "(F) 2.7",
                ["SumM"] = "(M) 1.5",
                ["SumGH"] = "(GH) 5",

                ["SumR"] = "G1/4",

                ["SumR2"] = "G1/4",
                ["Sumß"] = "",
                ["SumML"]="230",
                ["SumBML"] ="100",
            };
        }
    }
}