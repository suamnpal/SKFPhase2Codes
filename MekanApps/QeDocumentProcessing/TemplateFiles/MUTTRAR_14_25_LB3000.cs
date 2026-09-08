using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class MUTTRAR_14_25_LB3000 : ITemplateCalculations
    {
        private const string LB = "<<LineBreak>>";
        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var kv = new Dictionary<string, string>();

            kv["DocumentUniqueId"] =
                DateTime.UtcNow.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);

            string product = (request.ProductDesignation ?? "KM14").ToUpper();
            int type = ExtractType(product);

            double D = type / 2.0 * 10.0;
            double P = GetPitch(D);

            double dm = Math.Round(GetDM(D, P), 3);
            double id = Math.Round(GetID(dm, P), 3);

            double d3 = GetFromList(type, d3List);
            double D1 = GetFromList(type, D1List);
            double D4 = GetFromList(type, D4List);
            if(product.Contains("KML"))
            {
                D1 = 135;
                D4 = 145;
            }
            double B = GetFromList(type, BList);

            double S = GetS(D);
            string h = GetH(D);

            Merge(kv, GetAdmin(request));
            Merge(kv, GetMachine());
            Merge(kv, GetMain(product, D, P, dm, id, d3, D1, D4, B));
            Merge(kv, GetSecondary(S, h, D, D4));
            Merge(kv, GetTable(product, P));

            return kv;
        }

        private int ExtractType(string product)
        {
            foreach (var part in product.Split(' ', '-', '/'))
                if (int.TryParse(part, out int t))
                    return t;

            return 14;
        }

        private double GetPitch(double D)
        {
            if (D < 10) return 0.75;
            if (D < 20) return 1;
            if (D < 50) return 1.5;
            if (D < 150) return 2;
            if (D < 200) return 3;
            if (D < 300) return 4;
            return 5;
        }

        private double GetDM(double D, double P)
        {
            return P switch
            {
                0.75 => D - 0.487,
                1 => D - 0.65,
                1.5 => D - 0.974,
                2 => D - 1.299,
                3 => D - 1.949,
                4 => D - 2,
                _ => D - 2.5
            };
        }

        private double GetID(double dm, double P)
        {
            return P switch
            {
                0.75 => dm - 0.325,
                1 => dm - 0.433,
                1.5 => dm - 0.65,
                2 => dm - 0.866,
                3 => dm - 1.299,
                4 => dm - 2,
                _ => dm - 2.5
            };
        }

        private readonly double[] d3List =
            { 70.4, 75.4, 80.4, 85.4, 90.4, 95.4, 100.4, 105.4, 110.4, 115.4, 120.4, 125.4 };

        private readonly double[] D1List =
            { 85, 90, 95, 102, 108, 113, 120, 126, 133, 137, 138, 148 };

        private readonly double[] D4List =
            { 92, 98, 105, 110, 120, 125, 130, 140, 145, 150, 155, 160 };

        private readonly double[] BList =
            { 12, 13, 15, 16, 16, 17, 18, 18, 19, 19, 20, 21 };

        private double GetFromList(int type, double[] list)
        {
            int index = type - 14;
            if (index < 0 || index >= list.Length) index = 0;
            return list[index];
        }

        private double GetS(double D)
        {
            if (D < 13) return 3;
            if (D < 21) return 4;
            if (D < 36) return 5;
            if (D < 51) return 6;
            if (D < 66) return 7;
            if (D < 86) return 8;
            if (D < 101) return 10;
            if (D < 131) return 12;
            if (D < 151) return 14;
            return 16;
        }

        private string GetH(double D)
        {
            if (D < 40) return "2";
            if (D < 55) return "2,5";
            if (D < 70) return "3";
            if (D < 90) return "3,5";
            if (D < 105) return "4";
            if (D < 135) return "5";
            if (D < 155) return "6";
            return "7";
        }

        private Dictionary<string, string> GetAdmin(APIRequest r) => new()
        {
            ["Subject"] = "KM",
            ["Version"] = r.Version ?? "V1",
            ["Published"] = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            ["Created"] = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            ["Approved"] = r.ApprovedBy ?? "",
            ["SumRit"] = "7439009:3",
            ["SumTolRit"] = "1432008:7",
            ["SumKlEgenskaper"] = "PRODUCTION NUTS & SLEEVES & HOUSINGS ALLMÄN KLASSADE EGENSKAPER Klassade egenskaper muttrar"
        };

        private Dictionary<string, string> GetMachine() => new()
        {
            ["SumMaskinValS1"] = "Maskin: LB3000"
        };

        private Dictionary<string, string> GetMain(
      string product, double D, double P, double dm, double id,
      double d3, double D1, double D4, double B)
        {
            string f(double v) => v.ToString("0.###", CultureInfo.InvariantCulture);
            string thread = $"M{f(D)}x{f(P)}";

            string dmTol = D > 90 ? "+ 0.200 [3F]" : "+ 0.190 [3F]";
            string d4TolN = D4 >= 125 ? "- 0.630" : "- 0.540";

            return new Dictionary<string, string>
            {
                ["SumGänga"] = thread,
                ["SumP"] = "(P) " + f(P),
                ["SumGV"] = P < 4 ? "60°" : "30°",

                ["Sumdm"] = "(dm) " + f(dm),
                ["SumdmTol"] = dmTol,
                ["SumdmTolN"]= "- 0 [2F]",
                

                ["Sumid"] = "(id) " + f(id),
                ["SumidTol"] = "+ 0.300 [3F]",
                ["SumidTolN"] = "- 0 [2F]",

                ["Sumd3"] = "(2x) (d3) " + f(d3),
                ["Sumd3Tol"] = d3 > 120 ? "+1.000" : (d3 > 75.4 ? "+ 0.870" : "+ 0.740"),
                ["Sumd3TolN"] = "- 0",

                ["SumD1"] = "(D1) " + f(D1),
                ["SumD1Tol"] = "+ 0 [3F]",
                ["SumD1TolN"] = D1 > 120 ? "- 0.630" :" - 0.540",

                ["SumD4"] = "(D4) " + f(D4),
                ["SumD4Tol"] = "+ 0 [3F]",
                ["SumD4TolN"] = d4TolN,

                ["SumB"] = "(B) " + f(B),
                ["SumBTol"] = "+ 0 [3F]",
                ["SumBTolN"] = B > 18 ? "- 0.330 [3F]" : "- 0.270 [3F]"
            };
        }


        private Dictionary<string, string> GetSecondary(double S, string h, double D, double D4)
        {
            string f(double v) => v.ToString("0.###", CultureInfo.InvariantCulture);
            string rd = D4 >= 125 ? "0.080" : "0.070";
            return new Dictionary<string, string>
            {
                ["SumS"] = "(S) " + f(S),
                ["SumSTol"] = S == 12 ? "± 0.215[3F]":"± 0.180[3F]",

                ["Sumh"] = "(h) " + h,
                ["SumhTol"] = "+ 1.200",
                ["SumhTolN"] = "- 0[3F]",

                ["Sumt1"] = S > 10 ? "1.00" : "0.75",
                ["Sumt2"] = D4 == 160 ? "0.060" : "0.050",

                ["SumRa"] = D > 116 ? "3.2" : "2.5",
                ["SumR"] = "R 1.5",
                ["SumRd"] = rd,
                ["SumV45"] = "45°",
                ["SumV30"] = "30°",
                ["SumSnr"] = "(4x) 90°"
            };
        }

        private Dictionary<string, string> GetTable(string product, double P)
        {
            string thread = GetThread(product);

            return new Dictionary<string, string>
            {
                // -------- FREQUENCY --------
                ["SumF1_1"] = "1/5",
                ["SumF1_2"] = "1/5",
                ["SumF1_3"] = "Skärbyte",
                ["SumF1_4"] = "Skärbyte",
                ["SumF1_5"] = "Skärbyte",
                ["SumF1_6"] = "Skärbyte",
                ["SumF1_7"] = "1/tim",
                ["SumF1_8"] = "1/tim",
                ["SumF1_9"] = "Skärbyte",
                ["SumF1_0"] = "",   // ✅ FIXED

                // -------- MEASUREMENT --------
                ["SumD1_1"] = P < 4 ? "Klump" : "Skjutmått",
                ["SumD1_2"] = $"UD-Apparat med rullar: M{P}",
                ["SumD1_3"] = "Skjutmått",
                ["SumD1_4"] = "Skjutmått",
                ["SumD1_5"] = "Skjutmått",
                ["SumD1_6"] = "Skjutmått",
                ["SumD1_7"] = "Egglinjal",
                ["SumD1_8"] = "Ytjämnhetsmätare",
                ["SumD1_9"] = "Skjutmått",
                ["SumD1_0"] = "",   // ✅ FIXED

                // -------- REMARKS (IMPORTANT FIX) --------
                ["SumAF1_1"] = $"Klump märkt: {thread}",

                ["SumAF1_2"] =
                    $"Kontrolleras med gängtolk {thread}" + LB +
                    $"Gängklocka kontrolleras mot gängring: {thread}",

                ["SumAF1_3"] = $"Fasmall uppmärkt: {thread}",

                ["SumAF1_4"] = "",
                ["SumAF1_5"] = "",
                ["SumAF1_6"] = "Mäts i botten på haktag.",
                ["SumAF1_7"] = "Vid misstänkt formfel lämna mutter till mätrum",
                ["SumAF1_8"] = "Ytjämnhet övriga ytor Ra 6.3",
                ["SumAF1_9"] = "Gäller alla bearbetade ytor.",
                ["SumAF1_0"] = "",   // ✅ FIXED

                // -------- GENERAL TEXT --------
                ["SumText"] =
                    "Övrigt: Grader, frifläckar, slagmärken, repor, valkar och andra defekter samt ojämnheter kontrolleras visuellt."
            };
        }

        private string GetThread(string product)
        {
            int type = ExtractType(product);
            double D = type / 2.0 * 10.0;
            double P = GetPitch(D);

            return $"M{D.ToString("0", CultureInfo.InvariantCulture)}x{P}";
        }

        private void Merge(Dictionary<string, string> t, Dictionary<string, string> s)
        {
            foreach (var kv in s)
                t[kv.Key] = kv.Value ?? "";
        }
    }
}
