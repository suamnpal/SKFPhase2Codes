using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class LOCK_HE_FNL_505_522_Flanslagerhus_Lock_A : ITemplateCalculations
    {
        private static readonly string[] TypList =
        {
            "505","506","507","508","509",
            "510","511","512","513","515",
            "516","517","518","520","522"
        };

        private static readonly double[] LDList =
        {
            62,72,82,92,97,
            102,112,124,135,148,
            157,168,178,200,220
        };

        private static readonly double[] BList =
        {
            22,22.5,23.5,25,28,
            30,33,32,33,35,
            39,40.5,41,44,47
        };

        private static readonly double[] FDList =
        {
            52,62,72,80,85,
            90,100,110,120,130,
            140,150,160,180,200
        };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req?.ProductDesignation ?? "";
            string machine = req?.MachineNumber ?? "";

            kv["VaLPopUp"] = ComputePopup(req?.Published);

            string tmpFormat = subject.ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");

            string[] tokens =
                tmpBet.Split(
                    new char[] { ' ', '/', '.', '-' },
                    StringSplitOptions.RemoveEmptyEntries);

            string tmpTyp = tokens.Length >= 3 ? tokens[2] : "";

            int index = GetMember(tmpTyp, TypList);

            kv["VaLTypLista"] =
                index == 0
                    ? "Produktbeteckningen ingår ej i mallen"
                    : "";

            double ld = GetTabValue(LDList, index);
            double b = GetTabValue(BList, index);
            double fd = GetTabValue(FDList, index);

            //
            // FD
            //
            kv["SumFD"] = "(FD) " + FmtComma(fd);
            kv["SumFDTol"] =
                "- " + Fmt3(FDTolPlus(fd)) + " [3]";

            kv["SumFDTolN"] =
                "- " + Fmt3(FDTolMinus(fd)) + " [3]";

            //
            // LD
            //
            kv["SumLD"] = "(LD) " + FmtComma(ld);

            kv["SumLDTol"] =
                "± " + Fmt3(GeneralTolerance(ld));

            //
            // FH
            //
            double fh = fd < 81 ? 4 : 5;

            kv["SumFH"] = "(FH) " + FmtComma(fh);

            kv["SumFHTol"] = "+ 0.000 [3]";
            kv["SumFHTolN"] = "- 0.180 [3]";

            //
            // B
            //
            kv["SumB"] = "(B) " + FmtComma(b).Replace(".", ",") ;

            kv["SumBTol"] =
                "± " + Fmt3(GeneralTolerance(b));

            //
            // Lh
            //
            double lh;
            double lhTol;
            string holes;

            if (fd < 81)
            {
                lh = 5.5;
                lhTol = 0.180;
                holes = "(3x)";
            }
            else if (fd < 121)
            {
                lh = 6.6;
                lhTol = 0.220;
                holes = "(3x)";
            }
            else if (fd < 161)
            {
                lh = 9.0;
                lhTol = 0.220;
                holes = "(4x)";
            }
            else
            {
                lh = 11.5;
                lhTol = 0.270;
                holes = "(4x)";
            }

            kv["SumLh"] =
                "(Lh) " + FmtComma(lh) + " " + holes;

            kv["SumLhTol"] =
                "+ " + Fmt3(lhTol) + " [3]";

            kv["SumLhTolN"] =
                "- 0.000 [3]";

            //
            // Radius
            //
            kv["SumR1"] = "1x45°";
            kv["SumRm"] = "R max 1";

            //
            // Surface
            //
            kv["SumRa63"] = "6.3";

            //
            // Drawing
            //
            kv["SumRitNr"] = tmpFormat + ":1";

            kv["SumKlEgenskaper"] =
                "PRODUCTION NUTS & SLEEVES & HOUSINGSALLMÄNKLASSADE EGENSKAPERFlänslagerhus";

            kv["SumGGDRit"] = "7437455";

            //
            // Machine
            //
            bool isMultus =
                EqualsIgnoreCase(machine, "Multus");

            bool isGenos =
                EqualsIgnoreCase(machine, "Genos");

            bool machineValid = isMultus || isGenos;

            kv["SumMaskinValS1"] =
                "Maskin: " +
                (machineValid ? machine : "");

            //
            // Frequency
            //
            kv["SumF1_1"] = machineValid ? "1/5" : "";
            kv["SumF1_2"] = machineValid ? "1:a bit" : "";
            kv["SumF1_3"] = machineValid ? "1/30" : "";
            kv["SumF1_4"] = machineValid ? "1/Skift" : "";
            kv["SumF1_5"] = machineValid ? "1/30" : "";
            kv["SumF1_6"] = "";
            kv["SumF1_7"] = machineValid ? "1/Skift" : "";
            kv["SumF1_8"] = "";
            kv["SumF1_9"] = "";
            kv["SumF1_0"] = "";

            //
            // Gauges
            //
            kv["SumD1_1"] = machineValid ? "UD-Apparat" : "";
            kv["SumD1_2"] = machineValid ? "Mätmaskin" : "";
            kv["SumD1_3"] = machineValid ? "Skjutmått" : "";
            kv["SumD1_4"] = machineValid ? "Max/Min Tolk" : "";
            kv["SumD1_5"] = machineValid ? "Djupmått/Skjutmått" : "";
            kv["SumD1_6"] = "";
            kv["SumD1_7"] = machineValid ? "Ytjämnhetsmätare" : "";
            kv["SumD1_8"] = "";
            kv["SumD1_9"] = "";
            kv["SumD1_0"] = "";

            //
            // Remarks
            //
            kv["SumAF1_1"] =
                machineValid ? "1:a bit Mätmaskin" : "";

            kv["SumAF1_2"] = "";

            kv["SumAF1_3"] =
                machineValid ? "1:a bit Mätmaskin" : "";

            kv["SumAF1_4"] =
                machineValid ? "1:a bit Mätmaskin" : "";

            kv["SumAF1_5"] = "";
            kv["SumAF1_6"] = "";

            kv["SumAF1_7"] =
                machineValid
                    ? "Övriga bearbetade ytor Ra 12.5"
                    : "";

            kv["SumAF1_8"] = "";
            kv["SumAF1_9"] = "";
            kv["SumAF1_0"] = "";

            //
            // Text
            //
            kv["SumTextS1"] =
                "Okulärkontroll: Grader, frifläcker, slagmärken, repor, valkar, ytjämnhet och faser" +
                Environment.NewLine +
                "Skarpa kanter avgradas.";

            return kv;
        }

        private static double FDTolPlus(double fd)
        {
            if (fd < 80.1) return 0.060;
            if (fd < 120.1) return 0.072;
            if (fd < 180.1) return 0.085;

            return 0.100;
        }

        private static double FDTolMinus(double fd)
        {
            if (fd < 80.1) return 0.134;
            if (fd < 120.1) return 0.159;
            if (fd < 180.1) return 0.185;

            return 0.215;
        }

        private static double GeneralTolerance(double value)
        {
            if (value < 6.01) return 0.1;
            if (value < 30.01) return 0.2;
            if (value < 120.01) return 0.3;
            if (value < 315.01) return 0.5;
            if (value < 1000.01) return 0.8;
            if (value < 2000.01) return 1.2;

            return 2.0;
        }

        private static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published))
                return "";

            if (!DateTime.TryParse(published, out var dt))
                return "";

            var until = dt.AddDays(14);

            return DateTime.Today <= until.Date
                ? "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)\n\n" +
                  "Information om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument\n\n" +
                  "Popupruta aktiv till " +
                  until.ToString("yyyy-MM-dd")
                : "";
        }

        private static int GetMember(string value, string[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (string.Equals(
                    list[i],
                    value,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return i + 1;
                }
            }

            return 0;
        }

        private static double GetTabValue(
            double[] table,
            int oneBasedIndex)
        {
            if (oneBasedIndex < 1 ||
                oneBasedIndex > table.Length)
            {
                return 0;
            }

            return table[oneBasedIndex - 1];
        }

        private static bool EqualsIgnoreCase(
            string a,
            string b)
        {
            return string.Equals(
                a ?? "",
                b ?? "",
                StringComparison.OrdinalIgnoreCase);
        }

        private static string FmtComma(double v)
        {
            return v.ToString(
                "0.################",
                CommonFunctions.Culture)
                .Replace(",", ".");
        }

        private static string Fmt3(double v)
        {
            return v.ToString(
                "F3",
                CultureInfo.InvariantCulture);
        }
    }
}