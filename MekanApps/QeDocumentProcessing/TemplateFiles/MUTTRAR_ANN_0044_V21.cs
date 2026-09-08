using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;


namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class MUTTRAR_ANN_0044_V21 : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var kv = new Dictionary<string, string>();

            kv["DocumentUniqueId"] =
                DateTime.UtcNow.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);

            string drawing = (request.ProductDesignation ?? "ANN-0044").ToUpperInvariant();
            string machine = (request.MachineNumber ?? "").Trim().ToUpperInvariant();

            bool isVTR = machine.Contains("VTR-160");

            Merge(kv, GetAdmin(request, drawing));
            Merge(kv, GetMachineBlock(machine, isVTR));
            Merge(kv, GetSvarvBlock());
            Merge(kv, GetBorrBlock());
            Merge(kv, GetDerivedBlock(drawing, machine));

            return kv;
        }

        private Dictionary<string, string> GetAdmin(APIRequest req, string drawing)
        {
            var kv = new Dictionary<string, string>();

            kv["Subject"] = drawing;
            kv["Version"] = req.Version ?? "V21";
            kv["Published"] = req.Published ?? DateTime.UtcNow.ToString("yyyy-MM-dd");
            kv["Created"] = DateTime.UtcNow.ToString("yyyy-MM-dd");
            kv["Approved"] = req.ApprovedBy ?? "";
            kv["CreatedBy"] = req.CreatedBy ?? "";
            kv["ApprovedBy"] = req.ApprovedBy ?? "";
            kv["CategoryHierarchy"] = req.CategoryHierarchy ?? "";

            return kv;
        }

        private Dictionary<string, string> GetMachineBlock(string machine, bool isVTR)
        {
            var kv = new Dictionary<string, string>();

            string svarv = isVTR ? "VTR-160" : "";
            string borr = isVTR ? "VTR-160" : "";

            kv["SumMaskinValS1"] = "Maskin: " + svarv + " - Svarvning";
            kv["SumMaskinValS2"] = "Maskin: " + borr + " - Borr & Fräsning";

            return kv;
        }

        private Dictionary<string, string> GetSvarvBlock()
        {
            var kv = new Dictionary<string, string>();

            kv["d"] = "d";
            kv["d1"] = "d1";
            kv["d2"] = "d2";
            kv["d3"] = "d3";
            kv["d3_alt"] = "d3_alt";
            kv["dm"] = "dm";
            kv["d4"] = "d4";

            kv["b"] = "b";
            kv["b1"] = "b1";
            kv["b2"] = "b2";
            kv["b3"] = "b3";

            kv["P"] = "6";
            kv["Thread"] = "Tr 630x6";

            kv["Chamfer_F"] = "2X45°";
            kv["Radius_R"] = "R5";

            kv["Ra"] = "Ra";

            kv["Tempo1_b"] = "215.74";
            kv["Tempo2_b"] = "211.74";
            kv["Tempo_d1"] = "622.5";

            kv["SumTextS1"] =
                "Grader, frifläckar, slagmärken, repor, valkar & andra defekter samt ojämnheter kontrolleras med ögonmått, för övriga mått används skjutmått.";

            kv["SumRitS1"] =
                "produkt: C, Stämplas enl. 7430189:senaste utg.";

            return kv;
        }

        private Dictionary<string, string> GetBorrBlock()
        {
            var kv = new Dictionary<string, string>();

            kv["s1"] = "s1";
            kv["s"] = "97";

            kv["n"] = "33.5";
            kv["n1"] = "118";

            kv["t"] = "55";
            kv["t1"] = "25";

            kv["k2"] = "k2";
            kv["hole"] = "30";

            kv["e"] = "61";
            kv["e1"] = "39";
            kv["e2"] = "18";
            kv["e3"] = "12";
            kv["e4"] = "17";
            kv["e5"] = "11";
            kv["e6"] = "80";
            kv["e7"] = "61";
            kv["e8"] = "e8";

            kv["Thread_M"] = "M16";
            kv["Thread_Count"] = "4";

            kv["Torque"] = "50Nm";

            kv["SumTextS2"] =
                "Övriga mått kontrolleras vid inställning, för ej toleranssatta mått gäller iso 2768 mk";

            kv["SumKlEgenskaperS2"] =
                "PPA & PPH/ALLMÄN/KLASSADE EGENSKAPER/Klassade egenskaper muttrar";

            kv["SumRitS2"] =
                "produkt: C, Stämplas enl. 7430189:senaste utg.";

            return kv;
        }

        private Dictionary<string, string> GetDerivedBlock(string drawing, string machine)
        {
            var kv = new Dictionary<string, string>();

            string formatted = drawing.Replace(".", ",").ToUpperInvariant();
            kv["TmpFormat"] = formatted;

            string[] parts = drawing.Split('-', '/', ' ');
            string art = parts.Length > 0 ? parts[0] : "ANN";
            string serie = parts.Length > 1 ? parts[1] : "0044";

            kv["SumArt"] = art;
            kv["SumSerie"] = serie;
            kv["SumTyp"] = "V21";

            kv["MachineInput"] = machine;

            return kv;
        }

        private void Merge(Dictionary<string, string> target, Dictionary<string, string> src)
        {
            foreach (var kvp in src)
            {
                target[kvp.Key] = kvp.Value;
            }
        }
    }
}