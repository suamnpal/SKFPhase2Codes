using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class RINGAR_7433526_xxxx : ITemplateCalculations
    {
        private static readonly string[] Machines = new[] { "Nakamura", "MaxMuller", "LB45" };

        // D-lists: [D1, D2, D3, D4]
        private static readonly Dictionary<int, double[]> DList31 = new Dictionary<int, double[]>
        {
            { 60, new double[] { 310, 280, 334, 340 } },
            { 64, new double[] { 340, 300, 354, 360 } },
            { 68, new double[] { 360, 320, 374, 380 } },
            { 72, new double[] { 380, 340, 394, 400 } },
            { 76, new double[] { 400, 360, 414, 420 } },
            { 80, new double[] { 420, 380, 434, 440 } },
            { 84, new double[] { 440, 400, 454, 460 } },
        };

        private static readonly Dictionary<int, double[]> DList32 = new Dictionary<int, double[]>
        {
            { 68, new double[] { 360, 320, 379, 390 } },
            { 80, new double[] { 420, 380, 435, 441 } },
        };

        private const double TmpFKonst = 1.7321; // sin(60°)*2 = sin(120/2 * pi/180)*2

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");

            bool tmpSlash = tmpBet.IndexOf('/') >= 0;
            bool tmpV21 = tmpBet.IndexOf("V21", StringComparison.OrdinalIgnoreCase) >= 0;
            bool tmp7433526 = tmpBet.IndexOf("7433526", StringComparison.OrdinalIgnoreCase) >= 0;

            // NOTE: Explode delimiter is " /-" — no dot
            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : string.Empty;
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : string.Empty;

            string tmpSerie = tmpBet2.Length >= 2 ? tmpBet2.Substring(0, 2) : tmpBet2;
            string tmpTypStr = tmpBet2.Length >= 2 ? tmpBet2.Substring(tmpBet2.Length - 2) : tmpBet2;

            int serieInt = TryParseInt(tmpSerie);
            int typInt = TryParseInt(tmpTypStr);

            double[] dims = GetDims(serieInt, typInt);
            double tmpD1 = dims[0];
            double tmpD2 = dims[1];
            double tmpD3 = dims[2];
            double tmpD4 = dims[3];

            bool isNakamura = EqualsI(maskinVal, "Nakamura");

            // D1
            kv["SumD1"] = "(D1) " + Fmt(tmpD1).Replace(".", ",");
            kv["SumD1Tol"] = tmpD1 < 400 ? "± 0.5" : "± 0.8";

            // D2 — Nakamura shows raw value; others show D2+TolN (compensation)
            double tmpD2Tol = D2TolPos(tmpD2);
            double tmpD2TolN = D2TolNeg(tmpD2);
            double tmpD2Diff = Math.Round(tmpD2 + tmpD2TolN, 3);
            double tmpD2TolDiff = Math.Round(tmpD2Tol - tmpD2TolN, 3);
            kv["SumD2"] = "(D2) " + (isNakamura ? Fmt(tmpD2) : Fmt(tmpD2Diff)).Replace(".",",");
            kv["SumD2Tol"] = "+ " + (isNakamura ? Fmt3(tmpD2Tol) : Fmt3(tmpD2TolDiff));
            kv["SumD2TolN"] = isNakamura ? ("+ " + Fmt3(tmpD2TolN)) : "- 0";

            // D3
            kv["SumD3"] = "(D3) " + Fmt(tmpD3).Replace(".", ",");
            kv["SumD3Tol"] = D3TolPos(tmpD3);
            kv["SumD3TolN"] = "  0";

            // D4
            kv["SumD4"] = "(D4) " + Fmt(tmpD4).Replace(".", ",");
            kv["SumD4Tol"] = "  0";
            kv["SumD4TolN"] = D4TolNeg(tmpD4);

            // Thread F — M6 (2x) 120° with chord
            double tmpFKUtr = Math.Round((tmpD1 / 2.0) * TmpFKonst, 1);
            kv["SumF"] = "M6 (2x) 120°  Korda mellan gänghål: " + Fmt(tmpFKUtr).Replace(".",",");

            // A
            double tmpA = serieInt == 31 ? 25.0 : (serieInt == 32 ? (typInt == 68 ? 27.5 : 25.0) : 0.0);
            kv["SumA"] = "(A) " + Fmt(tmpA).Replace(".", ",");
            kv["SumATol"] = "± 0.2";

            // B
            double tmpB = serieInt == 31 ? 16.0 : (serieInt == 32 ? (typInt == 68 ? 18.5 : 16.0) : 0.0);
            kv["SumB"] = "(B) " + Fmt(tmpB).Replace(".", ",");
            kv["SumbTol"] = "± 0.2";

            // C
            kv["SumC"] = "(C) 12,5";
            kv["SumCTol"] = " 0";
            kv["SumCTolN"] = "- 0.2";

            // E
            kv["SumE"] = "(E) 5";
            kv["SumETol"] = "± 0.1";

            // Chamfers & radii
            kv["SumG"] = "1x45º";
            kv["SumG1"] = "1x45º";
            kv["SumG2"] = "1x45º";
            kv["SumR"] = "Max R 0.8";

            // Ra
            kv["SumRa1"] = "3.2";

            // Machine
            string tmpMaskinVal = EqualsI(maskinVal, "Nakamura") ? ""
                                  : (EqualsI(maskinVal, "MaxMuller") || EqualsI(maskinVal, "LB45") ? "Borring i skepp 6" : "");
            string tmpMaskinValS1 = EqualsI(maskinVal, "Nakamura") ? "Nakamura"
                                  : EqualsI(maskinVal, "MaxMuller") ? "MaxMuller"
                                  : EqualsI(maskinVal, "LB45") ? "LB45" : "";
            kv["SumMaskinValS1"] = "Maskin: " + tmpMaskinValS1 + " - " + tmpMaskinVal;

            SumFrequencies(kv, maskinVal);
            SumMeasuringDevices(kv, maskinVal);
            SumAF(kv, maskinVal);

            kv["SumTextS1"] = "Skarpa kanter avgradas";
            kv["SumRit"] = "7433526:senaste utg.";

            return kv;
        }

        private static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published)) return "";
            DateTime pubDt;
            if (!DateTime.TryParse(published, out pubDt)) return "";
            int dagar = 14;
            DateTime validTill = pubDt.AddDays(dagar);
            if (DateTime.Today <= validTill.Date)
                return "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom " +
                       dagar.ToString(CultureInfo.InvariantCulture) +
                       " dagar)<<LineBreak>>Information om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument<<LineBreak>>Popupruta aktiv till " +
                       validTill.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return "";
        }

        private static void SumFrequencies(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsMachine(maskinVal);
            kv["SumF1_1"] = m ? "1/1" : "";
            kv["SumF1_2"] = m ? "1/3" : "";
            kv["SumF1_3"] = m ? "1/3" : "";
            kv["SumF1_4"] = m ? "1/3" : "";
        }

        private static void SumMeasuringDevices(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsMachine(maskinVal);
            kv["SumD1_1"] = m ? "Skjutmått" : "";
            kv["SumD1_2"] = m ? "Gängtolk" : "";
            kv["SumD1_3"] = m ? "Ytjämnhetsmätare" : "";
            kv["SumD1_4"] = m ? "Skjutmått/Djupmått" : "";
        }

        private static void SumAF(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsMachine(maskinVal);
            kv["SumAF1_1"] = "";
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = m ? "Övrig ytjämnhet: Ra=12.5" : "";
            kv["SumAF1_4"] = "";
        }

        private static double[] GetDims(int serie, int typ)
        {
            if (serie == 31 && DList31.ContainsKey(typ)) return DList31[typ];
            if (serie == 32 && DList32.ContainsKey(typ)) return DList32[typ];
            return new double[] { 0, 0, 0, 0 };
        }

        private static double D2TolPos(double v)
        {
            if (v < 120) return 0.090; if (v < 185) return 0.106;
            if (v < 245) return 0.122; if (v < 315) return 0.137;
            if (v < 405) return 0.151; if (v < 510) return 0.165;
            return 0.186;
        }

        private static double D2TolNeg(double v)
        {
            if (v < 120) return 0.036; if (v < 185) return 0.043;
            if (v < 245) return 0.050; if (v < 315) return 0.056;
            if (v < 405) return 0.062; if (v < 510) return 0.068;
            return 0.076;
        }

        private static string D3TolPos(double v)
        {
            if (v < 180) return "+ 0.25";
            if (v < 255) return "+ 0.29";
            if (v < 315) return "+ 0.32";
            if (Math.Abs(v - 379) < 0.001) return "+ 0.40";  // exact match D3=379 (serie32/typ68)
            if (v < 395) return "+ 0.36";
            if (v < 500) return "+ 0.40";
            return "+ 0.44";
        }

        private static string D4TolNeg(double v)
        {
            if (v < 170) return "- 0.25";
            if (v < 270) return "- 0.29";
            if (v < 305) return "- 0.32";
            if (v < 405) return "- 0.36";
            if (v < 500) return "- 0.40";
            return "- 0.44";
        }

        private static bool IsMachine(string mv)
        {
            if (string.IsNullOrEmpty(mv)) return false;
            for (int i = 0; i < Machines.Length; i++)
                if (string.Equals(Machines[i], mv, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        private static bool EqualsI(string a, string b)
            => string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);

        private static int TryParseInt(string s)
        {
            int v;
            return int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static string Fmt(double v) => v.ToString(CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt3(double v) => v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
    }
}