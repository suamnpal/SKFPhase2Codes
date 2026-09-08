using QeDynamicDocumentProcessing.Common;
using System;
using System.Globalization;


namespace QeDynamicDocumentProcessing.TemplateFiles
    {
        public class KRAGAR_SL_TS_Kragar_V21 : ITemplateCalculations
        {
            public Dictionary<string, string> CalculateWordParameters(APIRequest request)
            {
                var result = new Dictionary<string, string>();
                var subjectRaw = request?.ProductDesignation ?? "";
                var subject = subjectRaw.Trim().ToUpper().Replace(".", ",");
                var machine = string.IsNullOrWhiteSpace(request?.MachineNumber) ? "LT-3000EX" : request.MachineNumber.Trim();

                Merge(result, Diameters(subject));
                Merge(result, Widths());
                Merge(result, Radii());
                Merge(result, Machine(machine));
                Merge(result, Frequencies());
                Merge(result, MeasurementTools());
                Merge(result, Remarks());
                Merge(result, Headers(subject));
                Merge(result, Misc());

                return result;
            }

            private Dictionary<string, string> Diameters(string subject)
            {
                var dict = new Dictionary<string, string>();
                var parts = subject.Split(new[] { '/', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                double d4 = parts.Length > 1 && double.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out var tmpD4) ? tmpD4 : 600;
                double d1 = parts.Length > 2 && double.TryParse(parts[2], NumberStyles.Any, CultureInfo.InvariantCulture, out var tmpD1) ? tmpD1 : 500;
                double d5 = d4 + 15;

                dict["SumD1"] = "(D1) " + Format(d1);
                dict["SumD1Tol"] = "+ 0.165";
                dict["SumD1TolN"] = "+ 0.068 [2]";

                dict["SumD2"] = "(D2) 516";
                dict["SumD2Tol"] = "+ 1";
                dict["SumD2TolN"] = " 0";

                dict["SumD3"] = "(D3) 584";
                dict["SumD3Tol"] = " 0";
                dict["SumD3TolN"] = "- 1";

                dict["SumD4"] = "(D4) " + Format(d4);
                dict["SumD4Tol"] = " 0";
                dict["SumD4TolN"] = "- 1.100";

                dict["SumD5"] = "(D5) " + Format(d5);
                dict["SumD5Tol"] = " 0";
                dict["SumD5TolN"] = "- 1.100";

                return dict;
            }

            private Dictionary<string, string> Widths()
            {
                var dict = new Dictionary<string, string>();

                dict["SumB1"] = "(B1) 42";
                dict["SumB1Tol"] = " 0";
                dict["SumB1TolN"] = "- 0.390";

                dict["SumB2"] = "(B2) 12";
                dict["SumB2Tol"] = "± 0.350";

                dict["SumB3"] = "(B3) 24";
                dict["SumB3Tol"] = "+ 0.330";
                dict["SumB3TolN"] = " 0 [3]";

                dict["SumB4"] = "(B4) 9";
                dict["SumB4Tol"] = "± 0.290 [3]";

                dict["SumB44"] = "(B4) 9";
                dict["SumB44Tol"] = "± 0.290 [3]";

                dict["SumB5"] = "(B5) 3,3";
                dict["SumB5Tol"] = "± 0.240 [3]";

                dict["SumB6"] = "(B6) 2";
                dict["SumB6Tol"] = "± 0.300 [3]";

                dict["SumB7"] = "(B7) 2";
                dict["SumB7Tol"] = "± 0.300 [3]";

                dict["SumB8"] = "(B8) 6";
                dict["SumB8Tol"] = "+ 0.480";
                dict["SumB8TolN"] = " 0";

                dict["SumB9"] = "(B9) 34";
                dict["SumB9Tol"] = " 0";
                dict["SumB9TolN"] = "- 1";

                return dict;
            }

            private Dictionary<string, string> Radii()
            {
                var dict = new Dictionary<string, string>();

                dict["SumR"] = "R1";
                dict["SumR1"] = "max R1";
                dict["Sum4R1"] = "4x R1";
                dict["SumR5"] = "2x R5";

                return dict;
            }

            private Dictionary<string, string> Machine(string machine)
            {
                var dict = new Dictionary<string, string>();
                dict["SumMaskinValS1"] = "Maskin: " + machine;
                return dict;
            }

            private Dictionary<string, string> Frequencies()
            {
                var dict = new Dictionary<string, string>();

                dict["SumF1_1"] = "1/1";
                dict["SumF1_2"] = "1/3";
                dict["SumF1_3"] = "1/3";
                dict["SumF1_4"] = "1/3";
                dict["SumF1_5"] = "1/3";
                dict["SumF1_6"] = "1/3";
                dict["SumF1_7"] = "1/3";
                dict["SumF1_8"] = "1/3";
                dict["SumF1_9"] = "1/3";
                dict["SumF1_0"] = "1/3";
                dict["SumF1_11"] = "1/3";

                return dict;
            }

            private Dictionary<string, string> MeasurementTools()
            {
                var dict = new Dictionary<string, string>();

                dict["SumD1_1"] = "UD-Apparat/Mikrometer";
                dict["SumD1_2"] = "Skjutmått";
                dict["SumD1_3"] = "Skjutmått";
                dict["SumD1_4"] = "Skjutmått";
                dict["SumD1_5"] = "Skjutmått";
                dict["SumD1_6"] = "Skjutmått";
                dict["SumD1_7"] = "Mall";
                dict["SumD1_8"] = "Mall";
                dict["SumD1_9"] = "Mall";
                dict["SumD1_0"] = "Skjutmått";
                dict["SumD1_11"] = "Skjutmått";

                return dict;
            }

            private Dictionary<string, string> Remarks()
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

            private Dictionary<string, string> Headers(string subject)
            {
                var dict = new Dictionary<string, string>();

                dict["SumStämpel"] = subject;
                dict["SumRit"] = subject + ":senaste utgåva";

                return dict;
            }

            private Dictionary<string, string> Misc()
            {
                var dict = new Dictionary<string, string>();

                dict["SumTextS1"] = "Trepunktsmätning utförs vid inställning. max variation 0,1mm<<LineBreak>><<LineBreak>> vid inställning mät innerdiameter d1 i två snitt<<LineBreak>><<LineBreak>> Bearbetas Ra 12,5 runt om, skarpa kanter avgradas. Max radie i botten på spår (B4) 1mm.";

                return dict;
            }

            private void Merge(Dictionary<string, string> target, Dictionary<string, string> source)
            {
                foreach (var kv in source)
                {
                    if (!target.ContainsKey(kv.Key))
                        target.Add(kv.Key, kv.Value ?? "");
                }
            }

            private string Format(double value)
            {
                return value.ToString("0.###", CultureInfo.InvariantCulture).Replace(".", ",");
            }
        }
    }