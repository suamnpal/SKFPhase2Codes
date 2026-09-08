using System;
using System.Collections.Generic;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class TATNINGAR_SL_TSO_2_D2_VZ2N0 : ITemplateCalculations
    {
        private const string LB = "<<LineBreak>>";
        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            Merge(result, GetAdmin(request));
            Merge(result, GetDiameters());
            Merge(result, GetWidthsHeights());
            Merge(result, GetLengths());
            Merge(result, GetThreadsAnglesChamfers());
            Merge(result, GetSurface());
            Merge(result, GetPage1Frequencies());
            Merge(result, GetPage2Frequencies());
            Merge(result, GetPage1Devices());
            Merge(result, GetPage2Devices());
            Merge(result, GetPage1Remarks());
            Merge(result, GetPage2Remarks());
            Merge(result, GetTexts());
            return result;
        }

        private Dictionary<string, string> GetAdmin(APIRequest request)
        {
            var d = new Dictionary<string, string>();
            var subject = string.IsNullOrWhiteSpace(request.ProductDesignation) ? "" : request.ProductDesignation.ToUpperInvariant().Replace(".", ",");
            var machine = request.MachineNumber == "Nakamura" || request.MachineNumber == "LT-3000" ? request.MachineNumber : "";
            d["SumRit"] = "7438713, 7433489";
            d["SumRit2"] = "7438713, 7433489";
            d["SumStämpel"] = "Stämplas: " + subject;
            d["SumMaskinValS1"] = "Maskin: " + machine;
            d["SumMaskinValS2"] = "Maskin: " + machine;
            return d;
        }

        private Dictionary<string, string> GetDiameters()
        {
            var d = new Dictionary<string, string>();
            d["SumD"] = "(d) 110";
            d["SumDTol"] = "+ 0.035";
            d["SumDTolN"] = "+ 0";
            d["SumD1"] = "(D1) 125";
            d["SumD1Tol"] = "+ 0.5";
            d["SumD1TolN"] = "- 0";
            d["SumD2"] = "(D2) 139";
            d["SumD2Tol"] = "± 0.5";
            d["SumD3"] = "";
            d["SumD3Tol"] = "";
            d["SumD4"] = "(D4) 168";
            d["SumD4Tol"] = "± 0.5";
            d["SumD5"] = "(D5) 135";
            d["SumD5Tol"] = "+ 0";
            d["SumD5TolN"] = "- 0.250";
            d["SumD6"] = "(D6) 154";
            d["SumD6Tol"] = "+ 0.250";
            d["SumD6TolN"] = "- 0";
            d["SumD7"] = "(D7) 114";
            d["SumD7Tol"] = "± 0.5";
            d["SumD8"] = "(D8) 114.8";
            d["SumD8Tol"] = "+ 0.140";
            d["SumD8TolN"] = "- 0.0";
            return d;
        }

        private Dictionary<string, string> GetWidthsHeights()
        {
            var d = new Dictionary<string, string>();
            d["Suma"] = "(a) 87";
            d["SumaTol"] = "+ 0";
            d["SumaTolN"] = "- 0.1";
            d["Sumb"] = "(b) 80";
            d["SumbTol"] = "± 0.3";
            d["Sumc"] = "(c) 72";
            d["SumcTol"] = "+ 0.2";
            d["SumcTolN"] = "- 0";
            d["Sume"] = "(e) 47,5";
            d["SumeTol"] = "+ 0.2";
            d["SumeTolN"] = "- 0";
            d["Sumf"] = "(f) 5";
            d["SumfTol"] = "± 0.1";
            d["Sumg"] = "(g) M6";
            d["Sumh"] = "(h) 8";
            d["SumhTol"] = "± 0.2";
            d["Sumk"] = "(k) 26";
            d["SumkTol"] = "± 0.2";
            d["Summ"] = "(m) 3";
            d["SummTol"] = "± 0.1";
            d["Sumn"] = "2x45º";
            d["SumB1"] = "2x (B1) 4.0";
            d["SumB1Tol"] = "+ 0.200";
            d["SumB1TolN"] = "- 0.0";
            return d;
        }

        private Dictionary<string, string> GetLengths()
        {
            var d = new Dictionary<string, string>();
            d["SumL1"] = "(L1) 16";
            d["SumL1Tol"] = "± 0.200";
            d["SumL2"] = "(L2) 61";
            d["SumL2Tol"] = "± 0.300";
            return d;
        }

        private Dictionary<string, string> GetThreadsAnglesChamfers()
        {
            var d = new Dictionary<string, string>();
            d["Sum60"] = "60º";
            d["Sum60a"] = "60º";
            d["Sum8"] = "8º ± 1º";
            d["SumRs"] = "4 x R0.200";
            return d;
        }

        private Dictionary<string, string> GetSurface()
        {
            var d = new Dictionary<string, string>();
            d["SumRa63"] = "6.3";
            d["SumRa63a"] = "6.3";
            d["SumRa63b"] = "6.3";
            d["SumRa63c"] = "6.3";
            d["SumRa63d"] = "6.3";
            d["SumRa63e"] = "6.3";
            d["SumRa32"] = "3.2";
            d["SumRa32a"] = "3.2";
            d["SumRa32b"] = "3.2";
            d["SumRa32c"] = "3.2";
            d["SumRa32d"] = "3.2";
            d["SumRa32e"] = "3.2";
            return d;
        }

        private Dictionary<string, string> GetPage1Frequencies()
        {
            var d = new Dictionary<string, string>();
            d["SumF1_1"] = "1/3";
            d["SumF1_2"] = "1/3";
            d["SumF1_3"] = "1/2";
            d["SumF1_4"] = "1/3";
            d["SumF1_5"] = "1/1";
            d["SumF1_6"] = "1/2";
            d["SumF1_7"] = "1/2";
            d["SumF1_8"] = "1/3";
            d["SumF1_9"] = "1/1";
            d["SumF1_0"] = "1/3";
            d["SumF1_11"] = "1/3";
            d["SumF1_12"] = "1/3";
            d["SumF1_13"] = "1/3";
            return d;
        }

        private Dictionary<string, string> GetPage2Frequencies()
        {
            var d = new Dictionary<string, string>();
            d["SumF2_1"] = "1/2";
            d["SumF2_2"] = "1/3";
            d["SumF2_3"] = "1/3";
            d["SumF2_4"] = "1/3";
            d["SumF2_5"] = "1/3";
            return d;
        }

        private Dictionary<string, string> GetPage1Devices()
        {
            var d = new Dictionary<string, string>();
            d["SumD1_1"] = "Digitalt Skjutmått";
            d["SumD1_2"] = "Digitalt Skjutmått";
            d["SumD1_3"] = "Digitalt Skjutmått";
            d["SumD1_4"] = "Digitalt Skjutmått";
            d["SumD1_5"] = "UD-Apparat";
            d["SumD1_6"] = "Djupmått ev. med klocka";
            d["SumD1_7"] = "Digitalt Skjutmått";
            d["SumD1_8"] = "Digitalt Skjutmått";
            d["SumD1_9"] = "Okulärkontroll";
            d["SumD1_0"] = "Ytjämnhetsmätare";
            d["SumD1_11"] = "Digitalt Skjutmått";
            d["SumD1_12"] = "Digitalt Skjutmått";
            d["SumD1_13"] = "M6 Tolk";
            return d;
        }

        private Dictionary<string, string> GetPage2Devices()
        {
            var d = new Dictionary<string, string>();
            d["SumD2_1"] = "Digitalt Skjutmått";
            d["SumD2_2"] = "Digitalt Skjutmått";
            d["SumD2_3"] = "Digitalt Skjutmått";
            d["SumD2_4"] = "Digitalt Skjutmått";
            d["SumD2_5"] = "Ytjämnhetsmätare";
            return d;
        }

        private Dictionary<string, string> GetPage1Remarks()
        {
            var d = new Dictionary<string, string>();
            d["SumAF1_1"] = "";
            d["SumAF1_2"] = "";
            d["SumAF1_3"] = "";
            d["SumAF1_4"] = "";
            d["SumAF1_5"] = "Klove/Ring 110, mät båda sidor";
            d["SumAF1_6"] = "Passbitar";
            d["SumAF1_7"] = "";
            d["SumAF1_8"] = "";
            d["SumAF1_9"] = "60º";
            d["SumAF1_0"] = "";
            d["SumAF1_11"] = "";
            d["SumAF1_12"] = "";
            d["SumAF1_13"] = "120° delning";
            return d;
        }

        private Dictionary<string, string> GetPage2Remarks()
        {
            var d = new Dictionary<string, string>();
            d["SumAF2_1"] = "";
            d["SumAF2_2"] = "";
            d["SumAF2_3"] = "";
            d["SumAF2_4"] = "";
            d["SumAF2_5"] = "";
            return d;
        }

        private Dictionary<string, string> GetTexts()
        {
            var d = new Dictionary<string, string>();
            var text = "Okulärkontroll: Grader, frifläcker, slagmärken, repor, valkar, ytjämnhet och faser" + LB + LB + "" + LB + LB+ "Bearbetas Ra 6.3 där annat ej anges, skarpa kanter avgradas.";
            d["SumTextS1"] = text;
            d["SumTextS2"] = text;
            return d;
        }

        private void Merge(Dictionary<string, string> target, Dictionary<string, string> source)
        {
            foreach (var kv in source)
                target[kv.Key] = kv.Value ?? "";
        }
    }
}