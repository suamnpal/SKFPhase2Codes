using DocumentFormat.OpenXml.Office2016.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class TATNINGAR_SV_SNL_RP_V21_2 : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            Merge(result, GetAdmin(request));
            Merge(result, GetDiameters());
            Merge(result, GetWidths());
            Merge(result, GetAnglesChamfersSurface());
            Merge(result, GetBoringMeasurements());
            Merge(result, GetRowDefinitionsPage1());
            Merge(result, GetRowDefinitionsPage2(request));
            Merge(result, GetTexts());

            return result;
        }

        private Dictionary<string, string> GetAdmin(APIRequest request)
        {
            return new Dictionary<string, string>
            {
                ["SumRit"] = request.ProductDesignation,
                ["SumRit2"] = request.ProductDesignation,
                ["SumMaskinValS1"] = request?.MachineNumber == "Nakamura" ? "Maskin: Svarvning - Nakamura" : "Maskin: Svarvning - MaxMuller",
                ["SumMaskinValS2"] = request?.MachineNumber == "Nakamura" ? "Maskin: Borrning - Nakamura" : "Maskin: Borrning - Skepp 6",
            };
        }

        private Dictionary<string, string> GetDiameters()
        {
            return new Dictionary<string, string>
            {
                ["SumD1"] = "(D1) 284",
                ["SumD1Tol"] = "± 0.500",
                ["SumD2"] = "(D2) 270",
                ["SumD2Tol"] = "+ 0.0",
                ["SumD2TolN"] = "- 0.081",
                ["SumD3"] = "(D3) 260",
                ["SumD3Tol"] = "+ 0.520",
                ["SumD3TolN"] = "- 0.0",
                ["SumD4"] = "(D4) 222",
                ["SumD4Tol"] = "+ 0.460",
                ["SumD4TolN"] = "- 0.0",
                ["SumD5"] = "(D5) 270",
                ["SumD5Tol"] = "± 0.500",
                ["SumD6"] = "(D6) 302",
                ["SumD6Tol"] = "± 0.500"
            };
        }

        private Dictionary<string, string> GetWidths()
        {
            return new Dictionary<string, string>
            {
                ["SumB1"] = "(B1) 41",
                ["SumB1Tol"] = "± 0.300",
                ["SumB2"] = "(B2) 12",
                ["SumB2Tol"] = "+ 0.0",
                ["SumB2TolN"] = "- 0.027",
                ["SumB3"] = "(B3) 21",
                ["SumB3Tol"] = "± 0.200",
                ["SumB4"] = "(B4) 6",
                ["SumB4Tol"] = "± 0.100",
                ["SumB5"] = "(B5) 22",
                ["SumB5Tol"] = "± 0.200",
                ["SumM"] = "(M) 9",
                ["SumMTol"] = "± 0.200"
            };
        }

        private Dictionary<string, string> GetAnglesChamfersSurface()
        {
            return new Dictionary<string, string>
            {
                ["SumV45"] = "45º",
                ["SumV45_2"] = "45º",
                ["SumF45"] = "1x45º",
                ["SumV15"] = "1.4º",
                ["SumRa1"] = "3.2",
                ["SumRa2"] = "3.2",
                ["SumRa3"] = "3.2"
            };
        }

        private Dictionary<string, string> GetBoringMeasurements()
        {
            return new Dictionary<string, string>
            {
                ["SumBM"] = "(BM) 5",
                ["SumBMTol"] = "+ 0.120",
                ["SumBMTolN"] = "- 0.0",
                ["SumBD"] = "(BD) 6",
                ["SumBDTol"] = "± 0.200",
                ["SumA"] = "(A) 6",
                ["SumATol"] = "± 0.200",
                ["SumOH"] = "(OH) 8",
                ["SumDR"] = "(DR) 25",
                ["SumKordaBM"] = "Korda kant till kant 198,3"
            };
        }

        private Dictionary<string, string> GetRowDefinitionsPage1()
        {
            return new Dictionary<string, string>
            {
                ["SumF1_1"] = "1/1",
                ["SumD1_1"] = "Mikrometer",
                ["SumAF1_1"] = "",
                ["SumF1_2"] = "1/3",
                ["SumD1_2"] = "Skjutmått",
                ["SumAF1_2"] = "",
                ["SumF1_3"] = "1/3",
                ["SumD1_3"] = "Skjutmått",
                ["SumAF1_3"] = "",
                ["SumF1_4"] = "1/1",
                ["SumD1_4"] = "Mikrometer",
                ["SumAF1_4"] = "",
                ["SumF1_5"] = "1/3",
                ["SumD1_5"] = "Digitalt Djup/Hakmått",
                ["SumAF1_5"] = "",
                ["SumF1_6"] = "1/5",
                ["SumD1_6"] = "Skjutmått",
                ["SumAF1_6"] = "",
                ["SumF1_7"] = "Inst.",
                ["SumD1_7"] = "Vinkelsystem",
                ["SumAF1_7"] = "",
                ["SumF1_8"] = "Inst.",
                ["SumD1_8"] = "Vinkelsystem",
                ["SumAF1_8"] = "",
                ["SumF1_9"] = "1/3",
                ["SumD1_9"] = "Ytjämnhetsmätare",
                ["SumAF1_9"] = "Övriga ytor 12.5 runt om",
                ["SumF1_10"] = "1/3",
                ["SumD1_10"] = "Skjutmått",
                ["SumAF1_10"] = ""
            };
        }

        private Dictionary<string, string> GetRowDefinitionsPage2(APIRequest request)
        {
            return new Dictionary<string, string>
            {
                ["SumF2_1"] = request?.MachineNumber == "Nakamura" ? "1/5" : "1/1",
                ["SumD2_1"] = "Skjutmått",
                ["SumAF2_1"] = "",
                ["SumF2_2"] = request?.MachineNumber == "Nakamura" ? "1/5" : "1/1",
                ["SumD2_2"] = "Skjutmått",
                ["SumAF2_2"] = "",
                ["SumF2_3"] = request?.MachineNumber == "Nakamura" ? "1/5" : "1/1",
                ["SumD2_3"] = "Skjutmått",
                ["SumAF2_3"] = "",
                ["SumF2_4"] = request?.MachineNumber == "Nakamura" ? "1/5" : "1/1",
                ["SumD2_4"] = "Skjutmått",
                ["SumAF2_4"] = "",
                ["SumF2_5"] = request?.MachineNumber == "Nakamura" ? "Inst." : "1/1",
                ["SumD2_5"] = "Skjutmått",
                ["SumAF2_5"] = ""
            };
        }

        private Dictionary<string, string> GetTexts()
        {
            var text = "Okulärkontroll Grader, frifläcker, slagmärken, repor, valkar, ytjämnhet och faser, skarpa kanter avgradas.";

            return new Dictionary<string, string>
            {
                ["SumTextS1"] = text,
                ["SumTextS2"] = text
            };
        }

        private void Merge(Dictionary<string, string> target, Dictionary<string, string> source)
        {
            foreach (var kv in source)
                target[kv.Key] = kv.Value ?? "";
        }
    }
}
