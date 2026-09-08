using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class MUTTRAR_ANN_0094_Svarv_Borr_Fras : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var kv = CreateAllKeys();

            string subject = (request.ProductDesignation ?? "").Trim().ToUpperInvariant();
            string machineRaw = (request.MachineNumber ?? "").Trim().ToUpperInvariant();

            string machineS1 = machineRaw == "VTR-160" ? "VTR-160" : machineRaw == "DS900" ? "VTR-160" : "";
            string machineS2 = machineRaw == "VTR-160" ? "VTR-160" : machineRaw == "DS900" ? "DS900" : "";
            string machineS5 = machineS2;

            kv["DocumentUniqueId"] = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);

            Merge(kv, GetBasic(subject));
            Merge(kv, GetMachine(machineS1, machineS2, machineS5));
            Merge(kv, GetGeometry());
            Merge(kv, GetTolerances());
            Merge(kv, GetThreads());
            Merge(kv, GetScrub());
            Merge(kv, GetHoleAndPositions());
            Merge(kv, GetAdminText());

            return kv;
        }

        private Dictionary<string, string> CreateAllKeys()
        {
            var kv = new Dictionary<string, string>();

            string[] keys =
            {
                "SumArt","SumSerie","SumTyp",
                "SumMaskinValS1","SumMaskinValS2","SumMaskinValS5",
                "SumSSK_B_T1","SumSSK_B_T2","SumSSK_h1_T2","SumSSK_d1_T2","SumSSK_d2_T2",
                "SumRitS1","SumRitS2","SumRitS5",
                "SumKlEgenskaperS2","SumKlEgenskaperS3","SumKlEgenskaperS5",
                "SumTextS1","SumTextS2","SumTextS5",
                "Sumd","Sumd1","Sumd2","Sumd3","Sumd4","Sumdm",
                "SumB","Sumh","Sumh1","SumP",
                "SumA","SumBblock","SumC","SumCblock","Sumt","Sumt1","Sums1",
                "Sumn","Sumn1",
                "Sume","Sume1","Sume2","Sume3","Sume4","Sume5","Sume6","Sume7",
                "Sumk1","Sumk2","Sumk3",
                "SumThread","SumAngle","SumChamfer","SumRadius",
                "SumdTol","SumdTolN","Sumd1Tol","Sumd1TolN","Sumd2Tol","Sumd2TolN",
                "Sumd3Tol","Sumd3TolN","SumBTol","SumBTolN","SumhTol","SumhTolN",
                "Sumh1Tol","Sumh1TolN","SumDmTol","SumDmTolN"
            };

            foreach (var k in keys)
                kv[k] = "";

            return kv;
        }

        private Dictionary<string, string> GetBasic(string subject)
        {
            return new Dictionary<string, string>
            {
                ["SumArt"] = "ANN",
                ["SumSerie"] = "009",
                ["SumTyp"] = "4"
            };
        }

        private Dictionary<string, string> GetMachine(string s1, string s2, string s5)
        {
            return new Dictionary<string, string>
            {
                ["SumMaskinValS1"] = s1 != "" ? $"Maskin: {s1} - Svarvning" : "",
                ["SumMaskinValS2"] = s2 != "" ? $"Maskin: {s2} - Borr & Fräsning" : "",
                ["SumMaskinValS5"] = s5 != "" ? $"Maskin: {s5} - Op.5" : ""
            };
        }

        private Dictionary<string, string> GetGeometry()
        {
            return new Dictionary<string, string>
            {
                ["Sumd"] = "(d) 860",
                ["Sumd1"] = "(d1) 624",
                ["Sumd2"] = "(d2) 740",
                ["Sumd3"] = "(d3) 633",
                ["Sumd4"] = "(d4) 631",
                ["Sumdm"] = "(dm) 627",
                ["SumB"] = "(B) 210",
                ["Sumh"] = "(h) 70.5",
                ["Sumh1"] = "(h1) 60",
                ["SumP"] = "(P) 6"
            };
        }

        private Dictionary<string, string> GetTolerances()
        {
            return new Dictionary<string, string>
            {
                ["SumdTol"] = "+ 0.000",
                ["SumdTolN"] = "- 1.400",
                ["Sumd1Tol"] = "+ 0.500",
                ["Sumd1TolN"] = "- 0.000",
                ["Sumd2Tol"] = "+ 0.000",
                ["Sumd2TolN"] = "- 1.250",
                ["Sumd3Tol"] = "+ 2.000",
                ["Sumd3TolN"] = "- 0.000",
                ["SumBTol"] = "+ 0.000",
                ["SumBTolN"] = "- 0.720",
                ["SumhTol"] = "+ 0.460",
                ["SumhTolN"] = "- 0.000",
                ["Sumh1Tol"] = "+ 0.460",
                ["Sumh1TolN"] = "- 0.000",
                ["SumDmTol"] = "+ 0.600",
                ["SumDmTolN"] = "- 0.000"
            };
        }

        private Dictionary<string, string> GetThreads()
        {
            return new Dictionary<string, string>
            {
                ["SumThread"] = "Tr 630x6",
                ["SumAngle"] = "30°",
                ["SumChamfer"] = "(F) 2x45°",
                ["SumRadius"] = "R5"
            };
        }

        private Dictionary<string, string> GetScrub()
        {
            return new Dictionary<string, string>
            {
                ["SumSSK_B_T1"] = "215.64",
                ["SumSSK_B_T2"] = "211.64",
                ["SumSSK_h1_T2"] = "62.03",
                ["SumSSK_d1_T2"] = "619.3",
                ["SumSSK_d2_T2"] = "747.375"
            };
        }

        private Dictionary<string, string> GetHoleAndPositions()
        {
            return new Dictionary<string, string>
            {
                ["SumA"] = "A",
                ["SumBblock"] = "B",
                ["SumC"] = "C",
                ["SumCblock"] = "C-C",
                ["Sumt"] = "(t) 55",
                ["Sumt1"] = "(t1) 25",
                ["Sums1"] = "(s1) 40",
                ["Sumn"] = "(n) 33.5",
                ["Sumn1"] = "(n1) 118",
                ["Sume"] = "(e) 87",
                ["Sume1"] = "(e1) 3x 39",
                ["Sume2"] = "(e2) Ø16.5",
                ["Sume3"] = "(e3) Ø12",
                ["Sume4"] = "(e4) Ø18",
                ["Sume5"] = "(e5) 11",
                ["Sume6"] = "(e6) 3x 81",
                ["Sume7"] = "(e7) 3x 17",
                ["Sumk1"] = "(k1) 6x 118",
                ["Sumk2"] = "(k2) 6x Ø30",
                ["Sumk3"] = "(k3) 6x 40"
            };
        }

        private Dictionary<string, string> GetAdminText()
        {
            string klass = "PPA & PPH/ALLMÄN/KLASSADE EGENSKAPER/Klassade egenskaper muttrar";

            return new Dictionary<string, string>
            {
                ["SumRitS1"] = "produkt: ANN-0094, Stämplas enl. 7433200:senaste utg.",
                ["SumRitS2"] = "produkt: ANN-0094, Stämplas enl. 7433200:senaste utg.",
                ["SumRitS5"] = "produkt: ANN-0094, Stämplas enl. 7433200:senaste utg.",
                ["SumKlEgenskaperS2"] = klass,
                ["SumKlEgenskaperS3"] = klass,
                ["SumKlEgenskaperS5"] = klass,
                ["SumTextS1"] = "Grader, frifläckar, slagmärken, repor, valkar & andra defekter samt ojämnheter kontrolleras med ögonmått, för övriga mått används skjutmått.",
                ["SumTextS2"] = "Övriga mått kontrolleras vid inställning, för ej toleranssatta mått gäller iso 2768 mk",
                ["SumTextS5"] = "Övriga mått kontrolleras vid inställning, för ej toleranssatta mått gäller iso 2768 mk"
            };
        }

        private void Merge(Dictionary<string, string> target, Dictionary<string, string> source)
        {
            foreach (var kv in source)
                target[kv.Key] = kv.Value ?? "";
        }
    }
}