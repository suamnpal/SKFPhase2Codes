using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HUS_FYNT_35_100_F_L : ITemplateCalculations
    {
        private const string LB = "<<LineBreak>>";
        private static readonly int[] Types = { 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 90, 100 };

        private static readonly double[] LL_LIST = { 72, 80, 85, 90, 100, 110, 120, 125, 130, 140, 160, 180 };
        private static readonly string[] LL_DISPLAY = { "72", "80", "85", "90", "100", "110", "120", "125", "130", "140*", "160", "180" };
        private static readonly double[] G_LIST = { 13.1, 13, 13, 11.3, 11.3, 10.4, 10.4, 8.4, 8.4, 10, 11.7, 11 };
        private static readonly double[] H_LIST = { 75, 83, 88, 94, 105, 116, 126, 131, 136, 146, 166, 186 };
        private static readonly double[] D_LIST = { 76.5, 84.5, 89.5, 95.5, 106.5, 118, 128, 133, 138, 148, 168, 188 };
        private static readonly double[] D2_LIST = { 90, 100, 100, 105, 120, 130, 150, 150, 170, 170, 200, 220 };
        private static readonly double[] F_LIST = { 38.6, 38.9, 38.5, 38.7, 38.5, 42.2, 44.2, 45.2, 45.5, 46, 55.9, 58.4 };
        private static readonly double[] D1_LIST = { 140, 160, 160, 170, 180, 190, 215, 215, 240, 240, 280, 310 };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req?.ProductDesignation ?? "";
            string maskinVal = req?.MachineNumber ?? "";

            kv["VaLPopUp"] = ComputePopup(req?.Published);

            string tmpBet = subject.ToUpperInvariant().Trim().Replace(".", ",");
            string[] t = tmpBet.Split(new char[] { ' ', '/', '.' }, StringSplitOptions.RemoveEmptyEntries);

            int typ = ParseInt(t, 1);
            string art = t.Length > 2 ? t[2] : "";

            int idx = IndexOf(typ);

            double LL = LL_LIST[idx];
            kv["SumLL"] = "(LL) " + LL_DISPLAY[idx];
            kv["SumLLTol"] = "+ " + F3(GetLLTol(LL)) + " [3]";
            kv["SumLLTolN"] = "+ " + F3(GetLLTolN(LL)) + " [2]";

            double K = CalcK(typ, art);
            kv["SumK"] = "(K) " + F2(K);
            kv["SumKTol"] = "+ " + (art == "F" ? "0.10 [2]" : "0.50 [3]");
            kv["SumKTolN"] = "- 0" + (art == "F" ? " [2P]" : " [3]");

            double J = CalcJ(typ, art);
            kv["SumJ"] = "(J) " + F1(J);
            kv["SumJTol"] = "± 0.20 [3]";

            kv["SumB4"] = "(B4) " + F1(CalcB4(typ));
            kv["SumB4Tol"] = "± 0.20 [3]";

            kv["Sumb1"] = "(b1) 2.15";
            kv["Sumb1Tol"] = "+ " + F3(H13Tol(2.15)) + " [3]";
            kv["Sumb1TolN"] = "- 0 [3]";

            kv["Sumb2"] = "(b2) 3.15";
            kv["Sumb2Tol"] = "+ " + F3(Getb2Tol(3.15)) + " [3]";
            kv["Sumb2TolN"] = "- 0 [3]";

            kv["SumG"] = "(G) " + F1Smart(G_LIST[idx]);
            kv["SumGTol"] = "± 0.50";

            kv["SumE"] = "(E) 0.60";
            kv["SumETol"] = "± 0.20";

            kv["SumH"] = "(H) " + F(H_LIST[idx]);
            kv["SumHTol"] = "± 0.20 [3]";
            kv["SumHa"] = kv["SumH"];
            kv["SumHaTol"] = kv["SumHTol"];

            kv["SumD"] = "(D) " + F(D_LIST[idx]);
            kv["SumDTol"] = "+ 0.50";
            kv["SumDTolN"] = "- 0 [3F]";

            double S = typ < 55 ? 1.3 : typ < 65 ? 1.6 : typ < 100 ? 2 : 2.5;
            kv["SumS"] = "(S) " + F1(S);
            kv["SumSTol"] = "± 0.120 [3]";

            double D2 = D2_LIST[idx];
            kv["SumD2"] = "(D2) " + F1Smart(D2);
            kv["SumD2Tol"] = "+ " + F3(GetD2Tol(D2));
            kv["SumD2TolN"] = "- 0 [3F]";

            kv["SumG1"] = "(G1) " + F1(CalcG1(typ));
            kv["SumFh"] = "(Fh) " + F1(CalcFh(typ));
            kv["SumFhTol"] = "± 0.60 [3]";

            kv["SumTh"] = "(Th) " + F1(CalcTh(typ));
            kv["SumF"] = "(F) " + F1Smart(F_LIST[idx]);
            kv["SumV"] = "(V) " + F0(CalcV(typ)) + "°";

            double M = CalcM(typ);
            kv["SumM"] = (typ < 65 ? "3x " : "4x ") + "(M) " + F1Comma(M);
            kv["SumMTol"] = "+ " + F3(GetMTol(M)) + " [3F]";
            kv["SumMTolN"] = "- 0 [3F]";

            kv["SumD1"] = "(D1) " + F1Smart(D1_LIST[idx]);

            kv["SumP"] = "0.08";
            kv["SumRd"] = F3(typ < 45 ? 0.015 : typ < 70 ? 0.017 : 0.020);

            kv["SumRa32"] = "3,2";
            kv["SumRa63"] = "6,3";
            kv["SumRp"] = "6";
            kv["SumGn"] = "1/8 NPSF";

            bool m = IsMultus(maskinVal);

            string tmp = m ? "Multus" : "";

            kv["SumMaskinValS1"] = "Maskin: " + tmp + " - Fräsning & Borrning";
            kv["SumMaskinValS2"] = "Maskin: " + tmp + " - Svarvning";

            SetFrequencies(kv, m);
            SetDevices(kv, m);
            SetAF(kv, m);

            string rit = $"Produktritning: HC-{tmpBet}  -  Gjutgodsdefekter: 7436186";
            kv["SumRitNrS1"] = rit;
            kv["SumRitNrS2"] = rit;

            kv["SumKlegenskaper"] = @"PRODUCTION NUTS & SLEEVES & HOUSINGS\3. ARBETSINSTRUKTIONER\HUS\Concentra\Klass 2-mått";

            string txt = "Kontrolleras enlingt styrplan. Skarpa kanter brytes, kontrolleras okulärt";
            kv["SumTextS1"] = txt;
            kv["SumTextS2"] = txt;

            return kv;
        }

        static void SetFrequencies(Dictionary<string, string> kv, bool m)
        {
            kv["SumF1_1"] = m ? "1/10" : ""; kv["SumF1_2"] = m ? "1/Omst." : ""; kv["SumF1_3"] = m ? "1/50" : "";
            kv["SumF1_4"] = m ? "1/50" : ""; kv["SumF1_5"] = m ? "1/25" : ""; kv["SumF1_6"] = m ? "1/50" : "";
            kv["SumF1_7"] = m ? "1/50" : ""; kv["SumF1_8"] = m ? "1/50" : ""; kv["SumF1_9"] = m ? "1/50" : "";
            kv["SumF1_0"] = "";

            kv["SumF2_1"] = m ? "1/1" : ""; kv["SumF2_2"] = m ? "1/Skift" : ""; kv["SumF2_3"] = m ? "1/Skift" : "";
            kv["SumF2_4"] = m ? "1/Skift" : ""; kv["SumF2_5"] = m ? "1/Skift" : ""; kv["SumF2_6"] = m ? "1/Skift" : "";
            kv["SumF2_7"] = m ? "1/Skift" : ""; kv["SumF2_8"] = m ? "1/Skift" : ""; kv["SumF2_9"] = m ? "1/25" : "";
            kv["SumF2_0"] = m ? "1/25" : ""; kv["SumF2_10"] = m ? "1/50" : ""; kv["SumF2_11"] = m ? "1/10" : "";
            kv["SumF2_12"] = m ? "1/10" : ""; kv["SumF2_13"] = m ? "1/dygn" : ""; kv["SumF2_14"] = m ? "1/1" : "";
            kv["SumF2_15"] = "";
        }

        static void SetDevices(Dictionary<string, string> kv, bool m)
        {
            kv["SumD1_1"] = m ? "Digitalskjutmått" : ""; kv["SumD1_2"] = m ? "Mätmaskin" : ""; kv["SumD1_3"] = m ? "Ytjämnhetsmätare" : "";
            kv["SumD1_4"] = m ? "Gängtolk" : ""; kv["SumD1_5"] = m ? "Digitalskjutmått" : ""; kv["SumD1_6"] = m ? "Vinkelmätare" : "";
            kv["SumD1_7"] = m ? "Mätmaskin" : ""; kv["SumD1_8"] = m ? "Mätmaskin" : ""; kv["SumD1_9"] = m ? "Digitalskjutmått" : "";
            kv["SumD1_0"] = "";

            kv["SumD2_1"] = m ? "Mätmaskin" : ""; kv["SumD2_2"] = m ? "Spec. mikrometer" : ""; kv["SumD2_3"] = m ? "Spec. mikrometer" : "";
            kv["SumD2_4"] = m ? "Digital Skjutmått" : ""; kv["SumD2_5"] = m ? "Digital Skjutmått" : ""; kv["SumD2_6"] = m ? "Digital Skjutmått" : "";
            kv["SumD2_7"] = m ? "Digital Skjutmått" : ""; kv["SumD2_8"] = m ? "Okulärt" : ""; kv["SumD2_9"] = m ? "Digital Skjutmått" : "";
            kv["SumD2_0"] = m ? "Special skjutmått" : ""; kv["SumD2_10"] = m ? "Special skjutmått" : ""; kv["SumD2_11"] = m ? "Mätmaskin" : "";
            kv["SumD2_12"] = m ? "Mätmaskin" : ""; kv["SumD2_13"] = m ? "Ytjämnhetsmätare" : ""; kv["SumD2_14"] = m ? "Mätmaskin" : "";
            kv["SumD2_15"] = "";
        }

        static void SetAF(Dictionary<string, string> kv, bool m)
        {
            kv["SumAF1_1"] = m ? "Mäts i alla 4 hörn"+LB+"max avvikelse 0.4 mm" : ""; kv["SumAF1_2"] = m ? "(Ej konvex)" : "";
            kv["SumAF1_3"] = ""; kv["SumAF1_4"] = ""; kv["SumAF1_5"] = ""; kv["SumAF1_6"] = "";
            kv["SumAF1_7"] = ""; kv["SumAF1_8"] = ""; kv["SumAF1_9"] = ""; kv["SumAF1_0"] = "";

            kv["SumAF2_1"] = ""; kv["SumAF2_2"] = ""; kv["SumAF2_3"] = ""; kv["SumAF2_4"] = ""; kv["SumAF2_5"] = "";
            kv["SumAF2_6"] = m ? "2 spår" : ""; kv["SumAF2_7"] = ""; kv["SumAF2_8"] = m ? "Runt om" : "";
            kv["SumAF2_9"] = m ? "Mätes båda sidor" : ""; kv["SumAF2_0"] = "";
            kv["SumAF2_10"] = m ? "Mätes båda sidor, max skillnad 0.1 mm" : "";
            kv["SumAF2_11"] = ""; kv["SumAF2_12"] = ""; kv["SumAF2_13"] = m ? "Bärighet: 6" : "";
            kv["SumAF2_14"] = m ? "Dokumenteras" : ""; kv["SumAF2_15"] = "";
        }
        static int IndexOf(int t) { for (int i = 0; i < Types.Length; i++) if (Types[i] == t) return i; return 0; }

        static int ParseInt(string[] t, int i)
        {
            if (t.Length <= i) return 0;
            int.TryParse(t[i], out int v);
            return v;
        }

        private static bool IsMultus(string mv)
        {
            return string.Equals(mv?.Trim(), "Multus", StringComparison.OrdinalIgnoreCase);
        }

        static double CalcK(int t, string a) => a == "F"
            ? (t < 50 ? 26.3 : t == 50 ? 27.9 : t == 55 ? 29.9 : t == 60 ? 32.9 : t < 80 ? 36.7 : t == 80 ? 38.7 : t == 90 ? 45.7 : 52.55)
            : (t < 50 ? 31.1 : t == 50 ? 32.7 : t == 55 ? 34.7 : t == 60 ? 37.7 : t < 80 ? 41.5 : t == 80 ? 43.5 : t == 90 ? 50.6 : 57.4);

        static double CalcJ(int t, string a) => a == "F"
            ? (t < 50 ? 10 : t == 50 ? 12.2 : t == 55 ? 11.3 : t == 60 ? 14.4 : t == 65 ? 12.3 : t < 80 ? 16.1 : t == 80 ? 14 : t == 90 ? 14.4 : 14.5)
            : (t < 50 ? 7.5 : t == 50 ? 9.8 : t == 55 ? 8.9 : t == 60 ? 11.9 : t == 65 ? 9.9 : t < 80 ? 13.7 : t == 80 ? 11.6 : t == 90 ? 12 : 12.1);

        static double CalcB4(int t) => t == 35 ? 48.1 : (t < 50 ? 48 : (t < 60 ? 53 : (t < 70 ? 62 : (t < 80 ? 68.8 : (t == 80 ? 66.8 : (t == 90 ? 74.6 : 81.5))))));

        static double CalcG1(int t) => t < 45 ? 4 : t < 65 ? 5 : t == 80 ? 7 : 6;
        static double CalcFh(int t) => t < 45 ? 12 : t < 65 ? 15 : t < 90 ? 25 : 30;
        static double CalcTh(int t) => t < 50 ? 66 : t < 60 ? 70 : t < 70 ? 78 : t < 80 ? 82 : t == 80 ? 82.5 : t == 90 ? 92 : 98;
        static double CalcV(int t) => t == 40 ? 4 : t < 65 ? 2 : t < 90 ? 5 : 12;
        static double CalcM(int t) => t < 65 ? 14 : t < 90 ? 18 : 22;

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

        static double H13Tol(double v) => v < 3 ? 0.14 : 0.18;
        static double GetD2Tol(double x)
        {
            if (x < 3.01) return 0.014;
            if (x < 6.01) return 0.018;
            if (x < 10.01) return 0.022;
            if (x < 18.01) return 0.027;
            if (x < 30.01) return 0.033;
            if (x < 50.01) return 0.039;
            if (x < 80.01) return 0.046;
            if (x < 120.01) return 0.054;
            if (x < 180.01) return 0.063;
            if (x < 250.01) return 0.072;
            return 0.081;
        }
        static double GetMTol(double x)
        {
            if (x < 3.01) return 0.250;
            if (x < 6.01) return 0.300;
            if (x < 10.01) return 0.360;
            if (x < 18.01) return 0.430;
            if (x < 30.01) return 0.520;
            if (x < 50.01) return 0.620;
            if (x < 80.01) return 0.740;
            if (x < 120.01) return 0.870;
            if (x < 180.01) return 1.000;
            if (x < 250.01) return 1.150;
            if (x < 315.01) return 1.300;
            if (x < 400.01) return 1.400;
            if (x < 500.01) return 1.550;
            if (x < 630.01) return 1.750;
            if (x < 800.01) return 2.000;
            if (x < 1000.01) return 2.300;
            if (x < 1250.01) return 2.600;
            if (x < 1600.01) return 3.100;
            if (x < 2000.01) return 3.700;
            if (x < 2500.01) return 4.400;
            return 5.400;
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

        static string F(double v) => v.ToString(CommonFunctions.Culture).Replace(",", ".");

        static string F0(double v) =>v.ToString("F0", CommonFunctions.Culture).Replace(",", ".");

        static string F1(double v) => v.ToString("F1", CommonFunctions.Culture).Replace(",", ".");

        static string F1Comma(double v) => v.ToString("F1", CommonFunctions.Culture);

        static string F1Smart(double v)
        {
            if (Math.Abs(v % 1) < 0.0001)
                return v.ToString("F0", CommonFunctions.Culture).Replace(",", ".");

            return v.ToString("F1", CommonFunctions.Culture).Replace(",", ".");
        }

        static string F2(double v) => v.ToString("F2", CommonFunctions.Culture).Replace(",", ".");

        static string F3(double v) => v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");


        static string ComputePopup(string p)
        {
            if (string.IsNullOrEmpty(p)) return "";
            if (!DateTime.TryParse(p, out DateTime d)) return "";
            var till = d.AddDays(14);
            return DateTime.Today <= till
                ? $"Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)"+LB+LB+"Popupruta aktiv till {till:yyyy-MM-dd}"
                : "";
        }
    }
}
