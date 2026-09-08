using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2013.Excel;
using DocumentFormat.OpenXml.Office2016.Excel;
using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class MUTTRAR_HM_T_HML_Skepp6_SPECIAL_OP1_4 : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var result = new Dictionary<string, string>();

            string machine = NormalizeMachine(request.MachineNumber);

            Merge(result, GetAdmin());
            Merge(result, GetDiameters());
            Merge(result, GetMachiningProcessBlock(machine));
            Merge(result, GetMachiningValuesBlock(machine));
            Merge(result, GetWidths());
            Merge(result, GetThreads());
            Merge(result, GetChamfers());
            Merge(result, GetSurface());
            Merge(result, GetMachine(machine));
            Merge(result, GetPage1(machine));
            Merge(result, GetPage2(machine));
            Merge(result, GetPage3(machine));
            Merge(result, GetFunctionalTolerances(request));

            return result;
        }

        private void Merge(Dictionary<string, string> t, Dictionary<string, string> s)
        {
            foreach (var kv in s)
                t[kv.Key] = kv.Value ?? "";
        }

        private string NormalizeMachine(string v)
        {
            return string.IsNullOrWhiteSpace(v) ? "" : v.Trim();
        }

        private Dictionary<string, string> GetAdmin() => new()
        {
            ["SumRitningsnr"] = "222756",
            ["SumRitningsnr2"] = "222756",
            ["SumRitningsnr3"] = "222756",
            ["SumYtRit"] = "7430184",
            ["SumYtRit2"] = "7430184",
            ["SumYtRit3"] = "7430184",
            ["SumKlEgenskaper"] = "PRODUCTION NUTS & SLEEVES & HOUSINGSALLMÄNKLASSADE EGENSKAPERKlassade egenskaper muttrar",
            ["SumKlEgenskaper2"] = "PRODUCTION NUTS & SLEEVES & HOUSINGSALLMÄNKLASSADE EGENSKAPERKlassade egenskaper muttrar",
            ["SumKlEgenskaper3"] = "PRODUCTION NUTS & SLEEVES & HOUSINGSALLMÄNKLASSADE EGENSKAPERKlassade egenskaper muttrar",
            ["SumTextS1"] = "Grader, frifläckar, slagmärken, repor, valkar och andra defekter samt ojämnheter kontrolleras med ögonmått, för övriga mått används skjutmått.",
            ["SumTextS2"] = "Övriga mått kontrolleras vid inställning",
            ["SumTextS3"] = "Vid misstänkt formfel lämnas muttern till mätrum <<LineBreak>> Stämplas enl. ritning 7433462",
            ["SumStämpling"] = "Stämplas enl. ritning 7433462",
            ["SumGTolRit"] = "7430181",
            ["SumGF"]= "30º"
        };

        private Dictionary<string, string> GetMachiningProcessBlock(string machine)
        {
            var r = new Dictionary<string, string>();

            if (machine != "Skepp6")
                return r;

            r["SumFM"] = "Färdigmått";
            r["SumFMU"] = "Matning inåt";
            r["SumFMUtv"] = "Matning utåt";

            r["SumUL"] = "Längd";
            r["SumUtvLin"] = "Yttre längd";

            r["SumFMI"] = "Intern matning";
            r["SumFMInv"] = "Intern matning bakåt";

            r["SumIL"] = "Intern längd";
            r["SumInvLin"] = "Intern längd bakåt";

            r["SumFMU"] = "Utvändig diameter:";
            r["SumUL"] = "Motsvarar på linjal:";

            r["SumFMI"] = "Invändig diameter:";
            r["SumIL"] = "Motsvarar på linjal:";

            r["SumFMUtv"] = "1215,00 mm";
            r["SumUtvLin"] = "1 mm";

            r["SumFMInv"] = "1086,00 mm";
            r["SumInvLin"] = "1 mm";

            return r;
        }

        private Dictionary<string, string> GetMachiningValuesBlock(string machine)
        {
            var r = new Dictionary<string, string>();

            double chuckback = 1;
            double stodback = 1;
            double grader = 1;
            double varvtal = 1;
            double matningPlan = 1;
            double matningIU = 1;

            r["SumCB"] = "Chuckbackar:";
            r["SumSB"] = "Stödbackar:";
            r["SumGR"] = "Grader:";
            r["SumVR"] = "Varvtal:";
            r["SumMP"] = "Matning Plan:";
            r["SumMIU"] = "Matning Utv/Inv:";

            r["SumChuckback"] = chuckback.ToString("0");
            r["SumStödback"] = stodback.ToString("0") + " mm";
            r["SumGrader"] = grader.ToString("0") + " mm";
            r["SumVarv"] = varvtal.ToString("0") + " /min";
            r["SumMatPl"] = matningPlan.ToString("0") + " /min";
            r["SumMatInUt"] = matningIU.ToString("0") + " /min";

            r["SumIN"] = "Inställning";

            return r;
        }

        private Dictionary<string, string> GetDiameters() => new()
        {
            ["SumD"] = "(D) 1215",
            ["SumDTol"] = "+ 0",
            ["SumDTolN"] = "- 0.660",

            ["SumD1"] = "(D1) 1087",
            ["SumD1Tol"] = "+ 0.630",
            ["SumD1TolN"] = "0",

            ["SumD2"] = "(D2) 1165",
            ["SumD2Tol"] = "+ 0",
            ["SumD2TolN"] = "- 1.650",

            ["SumD3"] = "(D3) 1098",
            ["SumD3Tol"] = "+ 1.650",
            ["SumD3TolN"] = "- 0",

            ["Sumd4"] = "(d4) 1095",
            ["Sumd4Tol"] = "± 1.2",

            ["Sumdm"] = "(dm) 1091",
            ["SumdmTol"] = "+ 0.710",
            ["SumdmTolN"] = "0",

            ["SumD1SSK"] = "(D1) 1086"
        };

        private Dictionary<string, string> GetWidths() => new()
        {
            ["SumB"] = "(B) 60",
            ["SumBTol"] = "+ 0",
            ["SumBTolN"] = "- 0.460",

            ["SumBSSK"] = "(B) 60",

            ["SumS"] = "(S) 50",
            ["SumSTol"] = "± 0.310",

            ["Sumt"] = "(t) 25",
            ["SumtTol"] = "+ 2.1",
            ["SumtTolN"] = "0",

            ["SumHM"] = "(HM) 25,5",

            ["SumLR"] = "1.6"
        };

        private Dictionary<string, string> GetThreads() => new()
        {
            ["SumGänga"] = "Tr 1095x8",
            ["SumGMått"] = "Tr 1095x8",
            ["SumP"] = "(P) 8",
        };

        private Dictionary<int, double[]> DiaTables = new()
        {
            {24, new[]{50.8,53.086,60.071,61.2,63.6,74,80,91,97,108,114}},
            {32, new[]{58.42,60.198,69.596,69.2,71.6,79,85,96,102,113,119}},
            {53, new[]{77,82.423,91.821,91.5,93.9,104,110,121,127,138,144}},
            {54, new[]{78.5,82.423,91.821,91.5,93.9,104,110,121,127,138,144}},
            {184,new[]{77,80.264,93.599,91.6,94,104,110,121,127,138,144}}
        };

        private Dictionary<string, string> GetFunctionalTolerances(APIRequest request)
        {
            string subject = (request.ProductDesignation ?? "").ToUpper();
            var parts = subject.Split(new[] { ' ', '/', '-' }, StringSplitOptions.RemoveEmptyEntries);
            int typ = parts.Length > 1 && int.TryParse(parts[1], out var t) ? t : 0;

            var dia = DiaTables.ContainsKey(typ) ? DiaTables[typ] : new double[11];
            double d4 = dia.Length > 4 ? dia[4] : 0;

            bool tmpB60 = subject.Contains("B60");

            string tmpKa = tmpB60 ? "0.1" :
                           d4 < 51 ? "0.04" :
                           d4 < 121 ? "0.05" :
                           d4 < 251 ? "0.06" :
                           d4 < 316 ? "0.07" :
                           d4 < 401 ? "0.08" :
                           d4 < 501 ? "0.09" :
                           d4 < 631 ? "0.10" :
                           d4 < 801 ? "0.12" :
                           d4 < 1001 ? "0.14" :
                                       "0.16";

            var r = new Dictionary<string, string>();

            r["SumPL"] = tmpKa;
            r["SumK"] = tmpKa;

            return r;
        }

        private Dictionary<string, string> GetChamfers() => new()
        {
            ["SumF"] = "5x45° (x2)",
            ["SumF1"] = "45º",
            ["SumF2"] = "45º",
            ["SumF3"] = "30º",
            ["SumR"] = "R 4",
            ["SumR3b"] = "R 4",
            ["SumRHT"] = "R1.6",
            ["SumF2S3"] = "45º",
            ["SumF1S3"] = "45º",
        };

        private Dictionary<string, string> GetSurface() => new()
        {
            ["SumRa32"] = "3.2"
        };

        private Dictionary<string, string> GetMachine(string machine)
        {
            if (machine == "Skepp6")
            {
                return new Dictionary<string, string>
                {
                    ["SumMaskinValS1"] = "Maskin: Morando - OP1 & 2",
                    ["SumMaskinValS2"] = "Maskin: K&T - OP3",
                    ["SumMaskinValS3"] = "Maskin: 1150 - OP4"
                };
            }

            return new Dictionary<string, string>
            {
                ["SumMaskinValS1"] = "",
                ["SumMaskinValS2"] = "",
                ["SumMaskinValS3"] = ""
            };
        }

        private Dictionary<string, string> GetPage1(string machine)
        {
            bool ok = machine == "Skepp6";
            return new Dictionary<string, string>
            {
                ["SumF1_1"] = ok ? "1/1" : "",
                ["SumF1_2"] = ok ? "1/1" : "",
                ["SumF1_3"] = ok ? "1/1" : "",
                ["SumF1_4"] = ok ? "1/1" : "",
                ["SumF1_5"] = ok ? "1/1" : "",
                ["SumF1_6"] = ok ? "1/1" : "",
                ["SumF1_7"] = ok ? "1/1" : "",
                ["SumF1_8"] = ok ? "1/1" : "",

                ["SumD1_1"] = ok ? "Skjutmått" : "",
                ["SumD1_2"] = ok ? "Mikrometer" : "",
                ["SumD1_3"] = ok ? "Djupmått/Skjutmått" : "",
                ["SumD1_4"] = ok ? "Fasmall/Skjutmått" : "",
                ["SumD1_5"] = ok ? "Djupmått/Skjutmått" : "",
                ["SumD1_6"] = ok ? "Fasmall/Skjutmått" : "",
                ["SumD1_7"] = ok ? "Radiestål" : "",
                ["SumD1_8"] = ok ? "Ytjämnhetsmätare" : "",

                ["SumAF1_1"] = "",
                ["SumAF1_2"] = "",
                ["SumAF1_3"] = "",
                ["SumAF1_4"] = ok ? "Mall ind.nr. 8520" : "",
                ["SumAF1_5"] = "",
                ["SumAF1_6"] = "",
                ["SumAF1_7"] = "",
                ["SumAF1_8"] = ok ? "Övriga Ra värden 6,3" : ""
            };
        }

        private Dictionary<string, string> GetPage2(string machine)
        {
            bool ok = machine == "Skepp6";
            return new Dictionary<string, string>
            {
                ["SumF2_1"] = ok ? "1/1" : "",
                ["SumF2_2"] = ok ? "1/1" : "",

                ["SumD2_1"] = ok ? "Skjutmått" : "",
                ["SumD2_2"] = ok ? "Djupmått" : "",

                ["SumAF2_1"] = "",
                ["SumAF2_2"] = ""
            };
        }

        private Dictionary<string, string> GetPage3(string machine)
        {
            bool ok = machine == "Skepp6";
            return new Dictionary<string, string>
            {
                ["SumF3_1"] = ok ? "1/1" : "",
                ["SumF3_2"] = ok ? "1/1" : "",
                ["SumF3_3"] = ok ? "1/1" : "",
                ["SumF3_4"] = ok ? "1/1" : "",
                ["SumF3_5"] = ok ? "1/1" : "",
                ["SumF3_6"] = ok ? "1/1" : "",
                ["SumF3_7"] = ok ? "1/1" : "",
                ["SumF3_8"] = ok ? "1/1" : "",
                ["SumF3_9"] = ok ? "1/1" : "",
                ["SumF3_0"] = ok ? "1/1" : "",

                ["SumD3_1"] = ok ? "Multimar" : "",
                ["SumD3_2"] = ok ? "Mikrometerstickmått" : "",
                ["SumD3_3"] = ok ? "Skjutmått" : "",
                ["SumD3_4"] = ok ? "Skjutmått" : "",
                ["SumD3_5"] = ok ? "Skjutmått" : "",
                ["SumD3_6"] = ok ? "Fasmall" : "",
                ["SumD3_7"] = ok ? "Gängmall" : "",
                ["SumD3_8"] = ok ? "Ytjämnhetsmätare" : "",
                ["SumD3_9"] = "",
                ["SumD3_0"] = ok ? "Egglinjal" : "",

                ["SumAF3_1"] = ok ? "Kontrolleras med passbitsklove utf. 1" : "",
                ["SumAF3_2"] = "",
                ["SumAF3_3"] = "",
                ["SumAF3_4"] = "",
                ["SumAF3_5"] = "",
                ["SumAF3_6"] = "",
                ["SumAF3_7"] = "",
                ["SumAF3_8"] = "",
                ["SumAF3_9"] = ok ? "Körs i samma uppspänning" : "",
                ["SumAF3_0"] = ""
            };
        }
    }
}