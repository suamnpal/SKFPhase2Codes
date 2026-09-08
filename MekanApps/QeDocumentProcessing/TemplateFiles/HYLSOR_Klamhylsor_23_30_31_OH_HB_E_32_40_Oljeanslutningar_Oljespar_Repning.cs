using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Klamhylsor_23_30_31_OH_HB_E_32_40_Oljeanslutningar_Oljespar_Repning : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var result = new Dictionary<string, string>();
            Merge(result, GetMachine(request));
            Merge(result, GetDimensions(request));
            Merge(result, GetTolerances(request));
            Merge(result, GetFrequencies(request));
            Merge(result, GetMeasurementTools(request));
            Merge(result, GetRemarks());
            Merge(result, GetMisc());
            return result;
        }

        private Dictionary<string, string> GetMachine(APIRequest request)
        {
            var dict = new Dictionary<string, string>();
            var machine = request?.MachineNumber ?? "";
            dict["SumMaskinValS1"] = string.IsNullOrEmpty(machine) ? "" : "Maskin: " + machine + " - BorrOljehål & Oljespår";
            return dict;
        }

        private Dictionary<string, string> GetDimensions(APIRequest request)
        {
            var dict = new Dictionary<string, string>();

            var designation = (request?.ProductDesignation ?? "").ToUpperInvariant();
            var type = ExtractType(designation);


            dict["SumRitNr"] = type.StartsWith("23") ? "7434159"
                                : type.StartsWith("30") ? "7434155"
                                : type.StartsWith("31") ? "7434157"
                                : "";

            dict["SumG"] = "M6";
            dict["SumB"] = "(B) 4,2";
            dict["SumC"] = "(C) 10";
            dict["SumT"] = "(T) 6,3";
            dict["SumD"] = "(D) 3";
            dict["SumH"] = type == "3140" ? "(H) 1" : "(H) 0,8";
            dict["SumN"] = "(N) 4";
            dict["SumR1"] = type == "3140" ? "R4" : "R3";
            dict["SumR1a"] = type == "3140" ? "R4" : "R3";
            dict["SumR"] = "R1";
            dict["SumV120"] = "120º";
            dict["SumV45"] = "45º";

            dict["SumF"] = "(F) 2";

            if (type == "2332") { dict["SumE"] = "(E) 82"; dict["SumJ"] = "(J) 79"; }
            else if (type == "2334") { dict["SumE"] = "(E) 85"; dict["SumJ"] = "(J) 82,5"; }
            else if (type == "2336") { dict["SumE"] = "(E) 89"; dict["SumJ"] = "(J) 86"; }
            else if (type == "2338") { dict["SumE"] = "(E) 93"; dict["SumJ"] = "(J) 90"; }
            else if (type == "2340") { dict["SumE"] = "(E) 97"; dict["SumJ"] = "(J) 93,5"; }
            else if (type == "3132") { dict["SumE"] = "(E) 69"; dict["SumJ"] = "(J) 66"; }
            else if (type == "3134") { dict["SumE"] = "(E) 71"; dict["SumJ"] = "(J) 68"; }
            else if (type == "3136") { dict["SumE"] = "(E) 75"; dict["SumJ"] = "(J) 72,5"; }
            else if (type == "3138") { dict["SumE"] = "(E) 81"; dict["SumJ"] = "(J) 77,5"; }
            else if (type == "3140") { dict["SumE"] = "(E) 85"; dict["SumJ"] = "(J) 82"; }
            else if (type == "3032") { dict["SumE"] = "(E) 57"; dict["SumJ"] = "(J) 54,5"; }
            else if (type == "3034") { dict["SumE"] = "(E) 61"; dict["SumJ"] = "(J) 58,5"; }
            else if (type == "3036") { dict["SumE"] = "(E) 66"; dict["SumJ"] = "(J) 63"; }
            else if (type == "3038") { dict["SumE"] = "(E) 68"; dict["SumJ"] = "(J) 64,5"; }
            else if (type == "3040") { dict["SumE"] = "(E) 72"; dict["SumJ"] = "(J) 68,5"; }
            else { dict["SumE"] = ""; dict["SumJ"] = ""; }

            return dict;
        }

        private Dictionary<string, string> GetTolerances(APIRequest request)
        {
            var dict = new Dictionary<string, string>();

            var designation = (request?.ProductDesignation ?? "").ToUpperInvariant();
            var type = ExtractType(designation);

            dict["SumDTol"] = "± 0.1";
            dict["SumTTol"] = "± 0.2";
            dict["SumCTol"] = "± 0.2";
            dict["SumHTol"] = "± 0.1";
            dict["SumNTol"] = "± 0.1";
            dict["SumBTol"] = "  0";
            dict["SumBTolN"] = "- 0.1";

            dict["SumFTol"] = "± 0.1";

            if (type.StartsWith("23") || type.StartsWith("30") || type.StartsWith("31"))
            {
                dict["SumETol"] = "± 0.3";
                dict["SumJTol"] = "± 0.3";
            }
            else
            {
                dict["SumETol"] = "";
                dict["SumJTol"] = "";
            }

            return dict;
        }

        private Dictionary<string, string> GetFrequencies(APIRequest request)
        {
            var dict = new Dictionary<string, string>();
            var val = request?.MachineNumber == "Dubbelparet" ? "1/10" : "";

            dict["SumF1_1"] = val;
            dict["SumF1_2"] = val;
            dict["SumF1_3"] = val;
            dict["SumF1_4"] = val;
            dict["SumF1_5"] = val;
            dict["SumF1_6"] = val;
            dict["SumF1_7"] = val;
            dict["SumF1_8"] = val;
            dict["SumF1_9"] = val;
            dict["SumF1_0"] = val;
            dict["SumF1_11"] = val;

            return dict;
        }

        private Dictionary<string, string> GetMeasurementTools(APIRequest request)
        {
            var dict = new Dictionary<string, string>();

            if (request?.MachineNumber == "Dubbelparet")
            {
                dict["SumD1_1"] = "pipborr/djupmått";
                dict["SumD1_2"] = "Skjutmått";
                dict["SumD1_3"] = "Skjutmått";
                dict["SumD1_4"] = "Gängtolk";
                dict["SumD1_5"] = "Skjutmått";
                dict["SumD1_6"] = "Skjutmått/fasmall";
                dict["SumD1_7"] = "Skjutmått";
                dict["SumD1_8"] = "Skjutmått";
                dict["SumD1_9"] = "pipborr/djupmått";
                dict["SumD1_0"] = "Skjutmått";
                dict["SumD1_11"] = "Radieyra";
            }
            else
            {
                dict["SumD1_1"] = "";
                dict["SumD1_2"] = "";
                dict["SumD1_3"] = "";
                dict["SumD1_4"] = "";
                dict["SumD1_5"] = "";
                dict["SumD1_6"] = "";
                dict["SumD1_7"] = "";
                dict["SumD1_8"] = "";
                dict["SumD1_9"] = "";
                dict["SumD1_0"] = "";
                dict["SumD1_11"] = "";
            }

            return dict;
        }

        private Dictionary<string, string> GetRemarks()
        {
            var dict = new Dictionary<string, string>();

            dict["SumAF1_1"] = "";
            dict["SumAF1_2"] = "";
            dict["SumAF1_3"] = "";
            dict["SumAF1_4"] = "";
            dict["SumAF1_5"] = "";
            dict["SumAF1_6"] = "";
            dict["SumAF1_7"] = "";
            dict["SumAF1_8"] = "";
            dict["SumAF1_9"] = "";
            dict["SumAF1_0"] = "";
            dict["SumAF1_11"] = "";

            return dict;
        }

        private Dictionary<string, string> GetMisc()
        {
            var dict = new Dictionary<string, string>();

            dict["SumTextS1"] = "";
            dict["SumTextS2"] = "Grader, frifläckar, slagmärken, repor, valkar och andra defekter samt ojämnheter kontrolleras med ögonmått.";
            dict["SumTextGängstigning"] = "Max 2x gängstigning";
            dict["SumÖvrigt"] = "";

            return dict;
        }

        private string ExtractType(string designation)
        {
            if (string.IsNullOrEmpty(designation)) return "";

            var digits = "";
            foreach (var c in designation)
            {
                if (char.IsDigit(c)) digits += c;
            }

            if (digits.Length >= 4) return digits.Substring(0, 4);
            return digits;
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