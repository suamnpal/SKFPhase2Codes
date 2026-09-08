using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class TATNINGAR_SV_SPL_0001_B_C : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var d = new Dictionary<string, string>();

            d["DocumentUniqueId"] = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);

            string machine = (request.MachineNumber ?? "").Trim();
            string machineKey = GetMachineKey(machine);
            bool isMachineType = IsMachineTypeSupported(request.MachineNumber);
            Merge(d, GetDimensions());
            Merge(d, GetWidthsAndLengths());
            Merge(d, GetThreadsAnglesAndRadii());
            Merge(d, GetHolesAndChamfers());
            Merge(d, GetSurface());
            Merge(d, GetPage1Measurements(isMachineType));
            Merge(d, GetPage2Measurements(isMachineType));
            Merge(d, GetAdmin(machine, machineKey));

            return d;
        }

        private static bool IsMachineTypeSupported(string m)
        {
            return m == "Nakamura" || m == "LB45";
        }

        private void Merge(Dictionary<string, string> t, Dictionary<string, string> s)
        {
            foreach (var e in s)
                t[e.Key] = e.Value ?? "";
        }

        private string GetMachineKey(string m)
        {
            var u = m.ToUpperInvariant();
            if (u.Contains("MAC")) return "MACTURN";
            if (u.Contains("MAX")) return "MAXMULLER";
            if (u.Contains("LB45")) return "LB45";
            return "UNKNOWN";
        }

        private Dictionary<string, string> GetDimensions() => new()
        {
            ["SumA"] = "(A) 460",
            ["SumATol"] = "± 0.800",
            ["SumB"] = "(B) 451",
            ["SumBTol"] = "+ 0.0",
            ["SumBTolN"] = "- 0.097",
            ["SumC"] = "(C) 431",
            ["SumCTol"] = "+ 0.630",
            ["SumCTolN"] = "- 0.0",
            ["SumD"] = "(D) 345",
            ["SumDTol"] = "± 0.800",
            ["SumE"] = "(E) 450",
            ["SumETol"] = "± 0.800",
            ["SumF"] = "(F) 460",
            ["SumFTol"] = "± 0.800",
            ["SumR"] = "(R) 205.5",
            ["SumRTol"] = "± 0.800",
            ["SumJ_C1"] = "(J) 221",
            ["SumJ_C2"] = "(J) 221",
            ["SumJ_B1"] = "(J) 221",
            ["SumJ_B2"] = "(J) 221",
            ["SumJ_C1Tol"] = "± 0.100",
            ["SumJ_C2Tol"] = "± 0.100",
            ["SumJ_B1Tol"] = "± 0.100",
            ["SumJ_B2Tol"] = "± 0.100"
        };

        private Dictionary<string, string> GetWidthsAndLengths() => new()
        {
            ["SumB1"] = "(B1) 26",
            ["SumB1Tol"] = "± 0.200",
            ["SumB2"] = "(B2) 10",
            ["SumB2Tol"] = "+ 0.0",
            ["SumB2TolN"] = "- 0.022",
            ["SumB3"] = "(B3) 6.8",
            ["SumB3Tol"] = "± 0.200",
            ["SumB4"] = "(B4) 5",
            ["SumB4Tol"] = "± 0.100",
            ["SumB5"] = "(B5) 18",
            ["SumB5Tol"] = "± 0.200",
            ["SumL1"] = "2x (L1) 7",
            ["SumL1Tol"] = "± 0.200",
            ["SumL2"] = "2x (L2) 7",
            ["SumL2Tol"] = "± 0.200",
            ["SumL3"] = "(L3) 6",
            ["SumL3Tol"] = "± 0.200"
        };

        private Dictionary<string, string> GetThreadsAnglesAndRadii() => new()
        {
            ["SumG"] = "5x (G) 20",
            ["SumGTol"] = "± 0.200",
            ["SumS"] = "15°",
            ["SumT"] = "30°"
        };

        private Dictionary<string, string> GetHolesAndChamfers() => new()
        {
            ["SumH1"] = "2x (H1) 5",
            ["SumH2"] = "2x (H2) 5",
            ["SumH3"] = "(H3) 5",
            ["SumH1Tol"] = "+ 0.120",
            ["SumH1TolN"] = "- 0.100",
            ["SumH2Tol"] = "+ 0.120",
            ["SumH2TolN"] = "- 0.100",
            ["SumH3Tol"] = "+ 0.120",
            ["SumH3TolN"] = "- 0.100",
            ["SumF1"] = "2x (F1) 7",
            ["SumF2"] = "2x (F2) 7",
            ["SumF1Tol"] = "± 0.200",
            ["SumF2Tol"] = "± 0.200",
            ["SumF3"] = "2x 1x45°",
            ["SumMx1"] = "max 1.25",
            ["SumMx2"] = "max 1.25"
        };

        private Dictionary<string, string> GetSurface() => new()
        {
            ["SumRa125"] = "12.5",
            ["SumM1_C"] = "2x (M1) 14",
            ["SumM1_B"] = "2x (M1) 14",
            ["SumM1_CTol"] = "± 0.100",
            ["SumM1_BTol"] = "± 0.100",
            ["SumM2"] = "(M2) 3.5",
            ["SumM2Tol"] = "± 0.100",
            ["SumM3"] = "(M3) 5",
            ["SumM3Tol"] = "± 0.100"
        };

        private Dictionary<string, string> GetPage1Measurements(bool isMachineType) => new()
        {

            ["SumF1_1"] = isMachineType ? "1/1" : "",
            ["SumF1_2"] = isMachineType ? "1/1" : "",
            ["SumF1_3"] = isMachineType ? "1/1" : "",
            ["SumF1_4"] = isMachineType ? "1/5" : "",
            ["SumF1_5"] = "",
            ["SumF1_6"] = isMachineType ? "1/1" : "",
            ["SumF1_7"] = isMachineType ? "1/1" : "",
            ["SumD1_1"] = isMachineType ? "Skjutmått" : "",
            ["SumD1_2"] = isMachineType ? "Skjutmått" : "",
            ["SumD1_3"] = isMachineType ? "Skjutmått" : "",
            ["SumD1_4"] = isMachineType ? "Ytjämnhetsmätare" : "",
            ["SumD1_5"] = "",
            ["SumD1_6"] = isMachineType ? "Mätservice" : "",
            ["SumD1_7"] = isMachineType ? "Skjutmått" : "",
            ["SumAF1_1"] = "",
            ["SumAF1_2"] = "",
            ["SumAF1_3"] = "",
            ["SumAF1_4"] = isMachineType ? "Övriga bearbetade ytor 6.3" : "",
            ["SumAF1_5"] = "",
            ["SumAF1_6"] = "",
            ["SumAF1_7"] = ""
        };

        private Dictionary<string, string> GetPage2Measurements(bool isMachineType) => new()
        {
            ["SumF2_1"] = isMachineType ? "1/1" : "",
            ["SumF2_2"] = isMachineType ? "1/1" : "",
            ["SumF2_3"] = isMachineType ? "1/1" : "",
            ["SumF2_4"] = "",
            ["SumF2_5"] = isMachineType ? "1/1" : "",
            ["SumF2_6"] = isMachineType ? "1/1" : "",
            ["SumF2_7"] = isMachineType ? "1/1" : "",
            ["SumD2_1"] = isMachineType ? "Skjutmått" : "",
            ["SumD2_2"] = isMachineType ? "Skjutmått" : "",
            ["SumD2_3"] = isMachineType ? "Skjutmått" : "",
            ["SumD2_4"] = "",
            ["SumD2_5"] = isMachineType ? "Skjutmått" : "",
            ["SumD2_6"] = isMachineType ? "Ytjämnhetsmätare" : "",
            ["SumD2_7"] = isMachineType ? "Skjutmått" : "",
            ["SumAF2_1"] = "",
            ["SumAF2_2"] = "",
            ["SumAF2_3"] = "",
            ["SumAF2_4"] = "",
            ["SumAF2_5"] = "",
            ["SumAF2_6"] = isMachineType ? "Övriga bearbetade ytor 6.3" : "",
            ["SumAF2_7"] = ""
        };

        private Dictionary<string, string> GetAdmin(string machine, string key) => new()
        {
            ["SumMaskinvalS1"] = "Maskin: " + machine + " - Del C efter klyvning.",
            ["SumMaskinvalS2"] = "Maskin: " + machine + " - Del B efter klyvning.",
            ["SumTextS1"] = "Okulärkontroll Grader, frifläckar, slagmärken, repor, valkar, ytjämnhet och faser, skarpa kanter avgradas. OBS! Ska doppas i olja",
            ["SumTextS2"] = "Okulärkontroll Grader, frifläckar, slagmärken, repor, valkar, ytjämnhet och faser, skarpa kanter avgradas.",
            ["SumRitNr"] = "7433527: senaste utgåva",
            ["SumRitNr2"] = "7433527: senaste utgåva"
        };
    }
}