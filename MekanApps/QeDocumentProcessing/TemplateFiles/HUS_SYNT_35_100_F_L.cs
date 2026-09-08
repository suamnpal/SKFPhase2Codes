using System;
using System.Collections.Generic;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HUS_SYNT_35_100_F_L : ITemplateCalculations
    {
        private const string LB = "<<LineBreak>>";
        private static readonly int[] Types =
        {
            35,40,45,50,55,60,65,70,75,80,90,100
        };

        private static readonly double[] LL_LIST =
        {
            72,80,85,90,100,110,120,125,130,140,160,180
        };

        private static readonly double[] G_LIST =
        {
            2.1,2.0,2.0,3.7,3.5,3.4,3.7,4.5,4.4,5.0,6.9,6.6
        };

        private static readonly double[] H_LIST =
        {
            75,83,88,94,105,116,126,131,136,146,166,186
        };

        private static readonly double[] D_LIST =
        {
            76.5,84.5,89.5,95.5,106.5,118,128,133,138,148,168,188
        };

        private static readonly double[] C_LIST =
        {
            24,20,17.5,25,20,25,20,32.5,30,30,32,35
        };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req?.ProductDesignation ?? "";
            string machine = req?.MachineNumber ?? "";

            kv["VaLPopUp"] = ComputePopup(req?.Published);

            string tmpBet =
                subject.ToUpperInvariant()
                       .Trim()
                       .Replace(".", ",");

            string[] t =
                tmpBet.Split(
                    new[] { ' ', '/', '.' },
                    StringSplitOptions.RemoveEmptyEntries);

            int typ = ParseInt(t, 1);
            string art = t.Length > 2 ? t[2] : "";

            int idx = IndexOf(typ);

            double LL = LL_LIST[idx];

            kv["SumLL"] = "(LL) " + F(LL);
            kv["SumLLTol"] = "+ " + F3(GetLLTol(LL)) + " [3]";
            kv["SumLLTolN"] = "+ " + F3(GetLLTolN(LL)) + " [2]";

            double K = CalcK(typ, art);

            kv["SumK"] = "(K) " + F2(K);
            kv["SumKTol"] =
                "+ " +
                (art == "F"
                    ? "0.10 [2]"
                    : "0.50 [3]");

            kv["SumKTolN"] =
                "- 0" +
                (art == "F"
                    ? " [2P]"
                    : " [3]");

            double J = CalcJ(typ, art);

            kv["SumJ"] = "(J) " + F2(J);
            kv["SumJTol"] = "± 0.20 [3]";

            kv["SumB4"] =
                "(B4) " + F1(CalcB4(typ));

            kv["SumB4Tol"] =
                "± 0.20 [3]";

            kv["Sumb1"] = "(b1) 2.15";
            kv["Sumb1Tol"] =
                "+ " + F3(H13Tol(2.15)) + " [3]";
            kv["Sumb1TolN"] =
                "- 0 [3]";

            double b2 =
                typ < 100
                    ? 3.15
                    : 3.53;

            kv["Sumb2"] =
                "(b2) " + F2(b2);

            kv["Sumb2Tol"] =
                "+ " +
                F3(Getb2Tol(b2)) +
                " [3]";

            kv["Sumb2TolN"] =
                "- 0 [3]";

            kv["SumG"] =
                "(G) " +
                F1Smart(G_LIST[idx]);

            kv["SumGTol"] =
                "± 0.50";

            kv["SumE"] = "(E) 0.60";
            kv["SumETol"] = "± 0.20";

            kv["SumH"] =
                "(H) " +
                F(H_LIST[idx]);

            kv["SumHa"] = kv["SumH"];

            kv["SumHTol"] =
                "+ 0.20 [3]";

            kv["SumHaTol"] =
                kv["SumHTol"];

            kv["SumHTolN"] =
                "- 0 [3]";

            kv["SumHaTolN"] =
                kv["SumHTolN"];

            kv["SumD"] =
                "(D) " +
                F(D_LIST[idx]);

            kv["SumDTol"] =
                "+ 0.50";

            kv["SumDTolN"] =
                "- 0 [3F]";

            double C = C_LIST[idx];

            kv["SumC"] =
                "(C) " +
                F1Smart(C);

            kv["SumCTol"] =
                "+ " +
                F3(typ < 69 ? 0.07 : 0.08);

            kv["SumCTolN"] =
                "- " +
                F3(typ < 69 ? 0.10 : 0.11);
            double S =
                typ < 55 ? 1.3 :
                typ < 65 ? 1.6 :
                typ < 100 ? 2.0 :
                2.5;

            kv["SumS"] =
                "(S) " + F1(S);

            kv["SumSTol"] =
                "± 0.120 [3]";

            kv["SumFh"] =
                "(Fh) " + FLotus(CalcFh(typ));

            kv["SumFhTol"] =
                "± 0.60 [3]";

            kv["SumP"] =
                typ < 80
                    ? "0.08"
                    : "0.10";

            kv["SumRd"] =
                F3(
                    typ < 50
                        ? 0.015
                        : typ < 70
                            ? 0.017
                            : 0.020);

            kv["SumRa32"] = "3,2";
            kv["SumRa63"] = "6,3";
            kv["SumRp"] = "6";
            kv["SumGn"] = "1/8 NPSF";

            bool validMachine =
                IsSupportedMachine(machine);

            string machineName =
                machine == "LB3000"
                    ? "LB3000"
                    : machine == "OKUMA MA600/Trevisan DS 450"
                        ? "OKUMA MA600"
                        : "";

            kv["SumMaskinValS1"] =
                $"Maskin: {machineName} - Fräsning/Borrning";

            kv["SumMaskinValS2"] =
                $"Maskin: {machineName} - Svarvning";

            SetFrequencies(kv, machine);
            SetDevices(kv, machine, typ);
            SetAF(kv, machine, typ);

            string rit =
                $"Produktritning: HC-{tmpBet}  -  Gjutgodsdefekter: 7436186";

            kv["SumRitNrS1"] = rit;
            kv["SumRitNrS2"] = rit;

            kv["SumKlegenskaper"] =
                @"PRODUCTION NUTS & SLEEVES & HOUSINGS\3. ARBETSINSTRUKTIONER\HUS\Concentra\Klass 2-mått";

            kv["SumTextS1"] =
                "Kontrolleras enlingt styrplan. Skarpa kanter brytes";

            kv["SumTextS2"] =
                "Kontrolleras enlingt styrplan. Skarpa kanter brytes, kontrolleras okulärt";

            return kv;
        }

        static void SetFrequencies(
            Dictionary<string, string> kv,
            string machine)
        {
            bool m =
                machine == "LB3000" ||
                machine == "OKUMA MA600/Trevisan DS 450";

            kv["SumF1_1"] = m ? "1/50" : "";
            kv["SumF1_2"] = m ? "1/Omst." : "";
            kv["SumF1_3"] = m ? "1/Omst." : "";
            kv["SumF1_4"] = m ? "1/50" : "";
            kv["SumF1_5"] = "";

            kv["SumF2_1"] = m ? "1/10" : "";
            kv["SumF2_2"] = m ? "1/Skift" : "";
            kv["SumF2_3"] = m ? "1/Skift" : "";
            kv["SumF2_4"] = m ? "1/Skift" : "";
            kv["SumF2_5"] = m ? "1/Skift" : "";
            kv["SumF2_6"] = m ? "1/Skift" : "";
            kv["SumF2_7"] = m ? "1/Skift" : "";
            kv["SumF2_8"] = m ? "1/Skift" : "";
            kv["SumF2_9"] = m ? "1/25" : "";
            kv["SumF2_0"] = m ? "1/25" : "";
            kv["SumF2_10"] = m ? "1/50" : "";
            kv["SumF2_11"] = m ? "1/50" : "";
            kv["SumF2_12"] = m ? "1/50" : "";
            kv["SumF2_13"] = m ? "1/dygn" : "";
        }

        static void SetDevices(
            Dictionary<string, string> kv,
            string machine,
            int typ)
        {
            bool lb3000 =
                machine == "LB3000";

            bool okuma =
                machine == "OKUMA MA600/Trevisan DS 450";

            kv["SumD1_1"] =
                lb3000 || okuma
                    ? "Skjutmått"
                    : "";

            kv["SumD1_2"] =
                lb3000
                    ? "Scanmax Mätmaskin"
                    : okuma
                        ? "Bladmått"
                        : "";

            kv["SumD1_3"] =
                lb3000 || okuma
                    ? "Ytjämnhetsmätare"
                    : "";

            kv["SumD1_4"] =
                lb3000 || okuma
                    ? "Gängtolk"
                    : "";

            kv["SumD1_5"] = "";

            kv["SumD2_1"] =
                lb3000
                    ? "Eltolk"
                    : okuma
                        ? "Subito"
                        : "";

            kv["SumD2_2"] =
                lb3000 || okuma
                    ? "Spec. mikrometer"
                    : "";

            kv["SumD2_3"] =
                lb3000 || okuma
                    ? "Spec. mikrometer"
                    : "";

            kv["SumD2_4"] =
                lb3000 || okuma
                    ? "Digital Skjutmått"
                    : "";

            kv["SumD2_5"] =
                lb3000 || okuma
                    ? "Digital Skjutmått"
                    : "";

            kv["SumD2_6"] =
                lb3000 || okuma
                    ? "Digital Skjutmått"
                    : "";

            kv["SumD2_7"] =
                lb3000 || okuma
                    ? "Digital Skjutmått"
                    : "";

            kv["SumD2_8"] =
                lb3000 || okuma
                    ? "Okulärt"
                    : "";

            kv["SumD2_9"] =
                lb3000 || okuma
                    ? "Digital Skjutmått"
                    : "";

            kv["SumD2_0"] =
                lb3000 || okuma
                    ? "Special skjutmått"
                    : "";

            kv["SumD2_10"] =
                lb3000
                    ? "SLA-apparat"
                    : okuma
                        ? "Höjdstativ"
                        : "";

            kv["SumD2_11"] =
                lb3000
                    ? "SLA-apparat"
                    : okuma
                        ? "Höjdstativ"
                        : "";

            kv["SumD2_12"] =
                lb3000 || okuma
                    ? "Ytjämnhetsmätare"
                    : "";

            kv["SumD2_13"] =
                lb3000
                    ? "Eltolk, mät 6 snitt"
                    : okuma
                        ? "Subito"
                        : "";
        }

        static void SetAF(
            Dictionary<string, string> kv,
            string machine,
            int typ)
        {
            bool lb3000 =
                machine == "LB3000";

            bool okuma =
                machine == "OKUMA MA600/Trevisan DS 450";

            bool any = lb3000 || okuma;

            kv["SumAF1_1"] =
                lb3000
                    ? "Mäts i alla 4 hörn"+LB+"max avvikelse 0.4 mm"
                    : "";

            kv["SumAF1_2"] =
                lb3000
                    ? "(Ej konvex)"
                    : "";

            kv["SumAF1_3"] = "";

            kv["SumAF1_4"] =
                lb3000
                    ? (typ > 65 ? "Gängdjup 13mm" : "")
                    : "";

            kv["SumAF1_5"] = "";

            kv["SumAF2_1"] = "";
            kv["SumAF2_2"] = "";
            kv["SumAF2_3"] = "";
            kv["SumAF2_4"] = "";
            kv["SumAF2_5"] = "";

            kv["SumAF2_6"] =
                any ? "2 spår" : "";

            kv["SumAF2_7"] =
                any ? "Mätes i överkant" : "";

            kv["SumAF2_8"] =
                any ? "Runt om" : "";

            kv["SumAF2_9"] =
                any ? "Mätes båda sidor" : "";

            kv["SumAF2_0"] = "";

            kv["SumAF2_10"] =
                any
                    ? "Mätes båda sidor, max skillnad 0.1 mm"
                    : "";

            kv["SumAF2_11"] =
                any ? "2 spår" : "";

            kv["SumAF2_12"] =
                any ? "Bärighet Rp 6" : "";

            kv["SumAF2_13"] =
                lb3000
                    ? $"Fastspänd {(typ < 50 ? "80" : typ < 70 ? "150" : "200")}Nm."
                    : "";
        }

        static int IndexOf(int t)
        {
            for (int i = 0; i < Types.Length; i++)
            {
                if (Types[i] == t)
                    return i;
            }

            return 0;
        }

        static int ParseInt(string[] t, int i)
        {
            if (t.Length <= i)
                return 0;

            int.TryParse(t[i], out int v);

            return v;
        }

        static bool IsSupportedMachine(string mv)
        {
            return string.Equals(
                       mv,
                       "LB3000",
                       StringComparison.OrdinalIgnoreCase)
                ||
                   string.Equals(
                       mv,
                       "OKUMA MA600/Trevisan DS 450",
                       StringComparison.OrdinalIgnoreCase);
        }

        static double CalcK(int t, string a)
        {
            if (a == "F")
            {
                return t switch
                {
                    35 => 26.3,
                    40 => 26.3,
                    45 => 26.3,
                    50 => 27.9,
                    55 => 29.9,
                    60 => 32.9,
                    65 => 36.7,
                    70 => 36.7,
                    75 => 36.7,
                    80 => 38.7,
                    90 => 45.7,
                    100 => 52.55,
                    _ => 0
                };
            }

            return t switch
            {
                35 => 31.1,
                40 => 31.1,
                45 => 31.1,
                50 => 32.7,
                55 => 34.7,
                60 => 37.7,
                65 => 41.5,
                70 => 41.5,
                75 => 41.5,
                80 => 43.5,
                90 => 50.6,
                100 => 57.4,
                _ => 0
            };
        }

        static double CalcJ(int t, string a)
        {
            if (a == "F")
            {
                return t switch
                {
                    35 => 10,
                    40 => 10,
                    45 => 10,
                    50 => 12.2,
                    55 => 11.3,
                    60 => 14.4,
                    65 => 12.3,
                    70 => 16.1,
                    75 => 16.1,
                    80 => 19.1,
                    90 => 19.2,
                    100 => 21.1,
                    _ => 0
                };
            }

            return t switch
            {
                35 => 7.5,
                40 => 7.5,
                45 => 7.5,
                50 => 9.8,
                55 => 8.9,
                60 => 11.9,
                65 => 9.9,
                70 => 13.7,
                75 => 13.7,
                80 => 16.7,
                90 => 16.8,
                100 => 18.7,
                _ => 0
            };
        }

        static double CalcB4(int t)
        {
            return t switch
            {
                35 => 48.1,
                40 => 48.0,
                45 => 48.0,
                50 => 53.0,
                55 => 53.0,
                60 => 62.0,
                65 => 62.0,
                70 => 68.8,
                75 => 68.8,
                80 => 76.8,
                90 => 84.0,
                100 => 94.0,
                _ => 0
            };
        }

        static double CalcFh(int t)
        {
            if (t < 50) return 25;
            if (t < 55) return 28;
            if (t < 70) return 30;
            if (t < 80) return 32;
            if (t == 80) return 35;
            if (t == 90) return 40;

            return 45;
        }

        static double GetLLTol(double x)
        {
            if (x < 3.01) return 0.012;
            if (x < 6.01) return 0.016;
            if (x < 10.01) return 0.020;
            if (x < 18.01) return 0.024;
            if (x < 30.01) return 0.028;
            if (x < 50.01) return 0.034;
            if (x < 80.01) return 0.040;
            if (x < 120.01) return 0.047;
            if (x < 180.01) return 0.054;

            return 0.061;
        }

        static double GetLLTolN(double x)
        {
            if (x < 3.01) return 0.002;
            if (x < 6.01) return 0.004;
            if (x < 10.01) return 0.005;
            if (x < 18.01) return 0.006;
            if (x < 30.01) return 0.007;
            if (x < 50.01) return 0.009;
            if (x < 80.01) return 0.010;
            if (x < 120.01) return 0.012;
            if (x < 180.01) return 0.014;

            return 0.015;
        }

        static double H13Tol(double v)
        {
            return v < 3 ? 0.14 : 0.18;
        }

        static double Getb2Tol(double x)
        {
            if (x < 3.01) return 0.14;
            if (x < 6.01) return 0.18;
            if (x < 10.01) return 0.22;
            if (x < 18.01) return 0.27;
            if (x < 30.01) return 0.33;

            return 0.39;
        }

        static string FLotus(double v)
        {
            return v.ToString(CommonFunctions.Culture)
                    .Replace(",", ".");
        }

        static string F(double v)
        {
            return v.ToString(CommonFunctions.Culture)
                    .Replace(",", ".");
        }

        static string F1(double v)
        {
            return v.ToString("F1", CommonFunctions.Culture)
                    .Replace(",", ".");
        }

        static string F2(double v)
        {
            return v.ToString("F2", CommonFunctions.Culture)
                    .Replace(",", ".");
        }

        static string F3(double v)
        {
            return v.ToString("F3", CommonFunctions.Culture)
                    .Replace(",", ".");
        }

        static string F1Smart(double v)
        {
            if (Math.Abs(v % 1) < 0.0001)
            {
                return v.ToString("F0", CommonFunctions.Culture)
                        .Replace(",", ".");
            }

            return v.ToString("F1", CommonFunctions.Culture)
                    .Replace(",", ".");
        }

        static string ComputePopup(string p)
        {
            if (string.IsNullOrWhiteSpace(p))
                return "";

            if (!DateTime.TryParse(p, out DateTime d))
                return "";

            DateTime till = d.AddDays(14);

            return DateTime.Today <= till
                ? $"Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)"+LB+LB+"Information om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument"+LB+LB+"Popupruta aktiv till {till:yyyy-MM-dd}"
                : "";
        }
    }
}