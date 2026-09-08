using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using QeDynamicDocumentProcessing.Common;


namespace QeDynamicDocumentProcessing.TemplateFiles
{

    /// <summary>
    /// Port of Lotus Notes script: TÄTNINGAR___GR_TSN template, _Qe_GR-TSN_Formler.txt
    /// Recreates all computed parameters (Sum*) for use in PDF generation.
    /// </summary>
    public class TÄTNINGAR___GR_TSN : ITemplateCalculations
    {
        // Machines defined in the Lotus script
        private static readonly string[] MaskinSupported = { "Nakamura", "LC-20" };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            // 1) FORMATTERING (TmpFormat, TmpBet etc.) – mirror Lotus
            //    TmpFormat = UPPER(TRIM(Subject))
            var tmpFormat = (req.ProductDesignation ?? string.Empty).ToUpperInvariant().Trim();

            // Lotus: TmpBet = replace '.' with ',' – we keep it for parity
            var tmpBet = tmpFormat.Replace(".", ",");

            // Use your existing tokenization helper so we stay consistent with your other class
            // Returns pieces like TmpBet2, TmpBet3... (series/type handling below mimics your sample)
            TmpBets bet = CommonFunctions.CalculateBets(tmpBet);

            // Compute Serie & Typ as Lotus logic (mirrors your prior C# approach)
            string tmpSerie;
            if (tmpBet.Contains("/") && (bet.TmpCountB2 == 3 || bet.TmpCountB2 == 2))
                tmpSerie = bet.TmpBet2;
            else if (bet.TmpCountB2 > 4)
                tmpSerie = bet.TmpBet2.Substring(0, Math.Min(3, bet.TmpBet2.Length));
            else if (bet.TmpCountB2 == 3)
                tmpSerie = bet.TmpBet2.Substring(0, Math.Min(1, bet.TmpBet2.Length));
            else
                tmpSerie = bet.TmpBet2.Substring(0, Math.Min(2, bet.TmpBet2.Length));

            // Lotus ends up taking the right-most 2 chars for type in practice
            string tmpTyp = bet.TmpBet2.Length >= 2 ? bet.TmpBet2[^2..] : bet.TmpBet2;

            // 2) TABELLER – per Lotus @Select logic (explicitly specified for Serie "30" + Typ "34")
            //    If not matched, tables are zeros.
            //    D-table holds [D, D1, D2, D3]
            //    B-table holds [B, B1, B2, B3, B4, B5]
            double[] tabD = TableD(tmpSerie, tmpTyp);
            double[] tabB = TableB(tmpSerie, tmpTyp);

            // 3) (D) & toleranser h13
            var D = tabD.ElementAtOrDefault(0);
            kv["SumD"] = "(D) " + FormatNum(D);
            kv["SumDTol"] = " 0";
            kv["SumDTolN"] = TolH13Minus(D); // negative tolerance series per Lotus
            // 4) (D1) & toleranser h15
            var D1 = tabD.ElementAtOrDefault(1);
            kv["SumD1"] = "(D1) " + FormatNum(D1);
            kv["SumD1Tol"] = " 0";
            kv["SumD1TolN"] = TolH15Minus(D1);
            // 5) (D2) & toleranser H12
            var D2 = tabD.ElementAtOrDefault(2);
            kv["SumD2"] = "(D2) " + FormatNum(D2);
            kv["SumD2Tol"] = TolH12Plus(D2);
            kv["SumD2TolN"] = " 0";
            // 6) (D3) & toleranser H12
            var D3 = tabD.ElementAtOrDefault(3);
            kv["SumD3"] = "(D3) " + FormatNum(D3);
            kv["SumD3Tol"] = TolH12Plus(D3);
            kv["SumD3TolN"] = " 0";

            // 7) (B) & toleranser
            var B = tabB.ElementAtOrDefault(0);
            kv["SumB"] = "(B) " + FormatNum(B);
            kv["SumBTol"] = "+ 1.0";
            kv["SumBTolN"] = "+ 0.3";

            var B1 = tabB.ElementAtOrDefault(1);
            kv["SumB1"] = "(B1) (" + FormatNum(B1) + ")";
            kv["SumB1Tol"] = TolSymmetricSteps(B1);

            var B2 = tabB.ElementAtOrDefault(2);
            kv["SumB2"] = "(B2) " + FormatNum(B2);
            kv["SumB2Tol"] = " 0";
            kv["SumB2TolN"] = "- 0.5";

            var B3 = tabB.ElementAtOrDefault(3);
            kv["SumB3"] = "(B3) (" + FormatNum(B3) + ")";
            kv["SumB3Tol"] = TolSymmetricSteps(B3);

            var B4 = tabB.ElementAtOrDefault(4);
            kv["SumB4"] = "(B4) " + FormatNum(B4);
            kv["SumB4Tol"] = TolSymmetricSteps(B4);

            var B5 = tabB.ElementAtOrDefault(5);
            kv["SumB5"] = "(B5) " + FormatNum(B5);
            kv["SumB5Tol"] = TolH13Plus(B5);
            kv["SumB5TolN"] = " 0";

            // 8) (L) & tol – only present for Serie "30" + Typ "34" => 2.5 per Lotus
            double? L = LengthL(tmpSerie, tmpTyp);
            if (L.HasValue)
            {
                kv["SumL"] = "(L) " + FormatNum(L.Value).Replace(",", "."); // Lotus replaces , -> .
                kv["SumLTol"] = TolSymmetricSteps(L.Value);
            }

            // 9) (V) – angle in degrees (14°) for Serie "30" + Typ "34"
            double? V = AngleV(tmpSerie, tmpTyp);
            if (V.HasValue)
            {
                kv["SumV"] = "(v) " + FormatNum(V.Value) + "º";
            }

            // 10) YTJÄMNHET
            kv["SumRa32"] = "3.2";
            kv["SumRa32a"] = "3.2";
            kv["SumRa125"] = "12.5";
            kv["SumRa125a"] = "12.5";
            kv["SumRa125b"] = "12.5";

            // 11) MASKINVAL & MÄTFREKVENSER (F) & MÄTDON (D) & ANMÄRKNINGSFÄLT (AF)
            SumMaskinVal(kv, req.MachineNumber);
            SumF(kv, req.MachineNumber);
            SumD(kv, req.MachineNumber);
            SumAF(kv, req.MachineNumber);

            // 12) ÖVRIG TEXT
            kv["SumText"] = "Skarpa kanter avgradas";

            // 13) RITNINGSNUMMER
            // Lotus sets SumRitNr to TmpFormat (UPPER(TRIM(Subject)))
            kv["SumRitNr"] = tmpFormat;

            return kv;
        }

        #region Tables (from Lotus @Select logic)

        private static double[] TableD(string serie, string typ)
        {
            // Lotus:
            // If Serie == "30" and Typ == "34" -> {218,5 : 212,5 : 183 : 152}
            // Else -> zeros
            if (serie == "30" && typ == "34")
                return new[] { 218.5, 212.5, 183.0, 152.0 };
            return new[] { 0.0, 0.0, 0.0, 0.0 };
        }

        private static double[] TableB(string serie, string typ)
        {
            // Lotus:
            // If Serie == "30" and Typ == "34" -> {18 : 4 : 6 : 6 : 3 : 10}
            // Else -> zeros
            if (serie == "30" && typ == "34")
                return new[] { 18.0, 4.0, 6.0, 6.0, 3.0, 10.0 };
            return new[] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
        }

        private static double? LengthL(string serie, string typ)
        {
            // Lotus: L = 2,5 for Serie "30" + Typ "34"
            if (serie == "30" && typ == "34") return 2.5;
            return null;
        }

        private static double? AngleV(string serie, string typ)
        {
            // Lotus: V = 14 for Serie "30" + Typ "34"
            if (serie == "30" && typ == "34") return 14.0;
            return null;
        }

        #endregion

        #region Masks, Frequencies, Devices, Remarks

        private static void SumMaskinVal(Dictionary<string, string> kv, string maskinVal)
        {
            // Lotus:
            // TmpMaskinVal = "" for Nakamura and LC-20; otherwise "INGET MASKINVAL GJORD"
            var suffix = (maskinVal == "Nakamura" || maskinVal == "LC-20") ? "" : "INGET MASKINVAL GJORD";
            kv["SumMaskinVal"] = "Maskin: " + (maskinVal ?? "") + suffix;
        }

        private static void SumF(Dictionary<string, string> kv, string maskinVal)
        {
            string f = MaskinSupported.Contains(maskinVal) ? "1/5" : "";
            kv["SumF_D"] = f;
            kv["SumF_D1"] = f;
            kv["SumF_D2"] = f;
            kv["SumF_D3"] = f;
            kv["SumF_B"] = f;
            kv["SumF_B1"] = f;
            kv["SumF_B24"] = f; // Lotus uses B24 as a combined label
            kv["SumF_B5"] = f;
            kv["SumF_L"] = f;
            kv["SumF_V"] = f;
            kv["SumF_Ra"] = f;
        }

        private static void SumD(Dictionary<string, string> kv, string maskinVal)
        {
            // Measurement devices per Lotus
            string dm = MaskinSupported.Contains(maskinVal) ? "Skjutmått" : "";
            string vm = MaskinSupported.Contains(maskinVal) ? "Vinkelmätare" : "";
            string rm = MaskinSupported.Contains(maskinVal) ? "Ytjämnhetsmätare" : "";

            kv["SumD_D"] = dm;
            kv["SumD_D1"] = dm;
            kv["SumD_D2"] = dm;
            kv["SumD_D3"] = dm;

            kv["SumD_B"] = dm;
            kv["SumD_B1"] = dm;
            kv["SumD_B24"] = dm;
            kv["SumD_B5"] = dm;

            kv["SumD_L"] = dm;
            kv["SumD_V"] = vm;
            kv["SumD_Ra"] = rm;
        }

        private static void SumAF(Dictionary<string, string> kv, string maskinVal)
        {
            // All AF fields blank in Lotus
            kv["SumAF_D"] = "";
            kv["SumAF_D1"] = "";
            kv["SumAF_D2"] = "";
            kv["SumAF_D3"] = "";
            kv["SumAF_B"] = "";
            kv["SumAF_B1"] = "";
            kv["SumAF_B24"] = "";
            kv["SumAF_B5"] = "";
            kv["SumAF_L"] = "";
            kv["SumAF_V"] = "";
            kv["SumAF_Ra"] = "";
        }

        #endregion

        #region Tolerance helpers (mirroring Lotus thresholds)

        private static string TolH13Minus(double x)
        {
            // Negative tolerances for h13 (D and B5 use the same list but with + for B5)
            if (x < 3.01) return "- 0.140";
            if (x < 6.01) return "- 0.180";
            if (x < 10.01) return "- 0.220";
            if (x < 18.01) return "- 0.270";
            if (x < 30.01) return "- 0.330";
            if (x < 50.01) return "- 0.390";
            if (x < 80.01) return "- 0.460";
            if (x < 120.01) return "- 0.540";
            if (x < 180.01) return "- 0.630";
            if (x < 250.01) return "- 0.720";
            if (x < 315.01) return "- 0.810";
            if (x < 400.01) return "- 0.890";
            if (x < 500.01) return "- 0.970";
            if (x < 630.01) return "- 1.100";
            if (x < 800.01) return "- 1.250";
            if (x < 1000.01) return "- 1.400";
            if (x < 1250.01) return "- 1.650";
            if (x < 1600.01) return "- 1.950";
            if (x < 2000.01) return "- 2.300";
            if (x < 2500.01) return "- 2.800";
            return "- 3.300";
        }

        private static string TolH13Plus(double x)
        {
            // Positive version used for B5 per Lotus
            return TolH13Minus(x).Replace("-", "+");
        }

        private static string TolH15Minus(double x)
        {
            if (x < 3.01) return "- 0.400";
            if (x < 6.01) return "- 0.480";
            if (x < 10.01) return "- 0.580";
            if (x < 18.01) return "- 0.700";
            if (x < 30.01) return "- 0.840";
            if (x < 50.01) return "- 1.000";
            if (x < 80.01) return "- 1.200";
            if (x < 120.01) return "- 1.400";
            if (x < 180.01) return "- 1.600";
            if (x < 250.01) return "- 1.850";
            if (x < 315.01) return "- 2.100";
            if (x < 400.01) return "- 2.300";
            if (x < 500.01) return "- 2.500";
            if (x < 630.01) return "- 2.800";
            if (x < 800.01) return "- 3.200";
            if (x < 1000.01) return "- 3.600";
            if (x < 1250.01) return "- 4.200";
            if (x < 1600.01) return "- 5.000";
            if (x < 2000.01) return "- 6.000";
            if (x < 2500.01) return "- 7.000";
            return "- 8.600";
        }

        private static string TolH12Plus(double x)
        {
            if (x < 3.01) return "+ 0.100";
            if (x < 6.01) return "+ 0.120";
            if (x < 10.01) return "+ 0.150";
            if (x < 18.01) return "+ 0.180";
            if (x < 30.01) return "+ 0.210";
            if (x < 50.01) return "+ 0.250";
            if (x < 80.01) return "+ 0.300";
            if (x < 120.01) return "+ 0.350";
            if (x < 180.01) return "+ 0.400";
            if (x < 250.01) return "+ 0.460";
            if (x < 315.01) return "+ 0.520";
            if (x < 400.01) return "+ 0.570";
            if (x < 500.01) return "+ 0.630";
            if (x < 630.01) return "+ 0.700";
            if (x < 800.01) return "+ 0.800";
            if (x < 1000.01) return "+ 0.900";
            if (x < 1250.01) return "+ 1.050";
            if (x < 1600.01) return "+ 1.250";
            if (x < 2000.01) return "+ 1.500";
            if (x < 2500.01) return "+ 1.750";
            return "+ 2.100";
        }

        private static string TolSymmetricSteps(double x)
        {
            // For B1/B3/B4/L (± tolerance bands)
            if (x < 6.01) return "± 0.1";
            if (x < 30.01) return "± 0.2";
            if (x < 120.01) return "± 0.3";
            if (x < 400.01) return "± 0.5";
            if (x < 1000.01) return "± 0.8";
            if (x < 2000.01) return "± 1.2";
            return "± 2.0";
        }

        #endregion

        private static string FormatNum(double d)
        {
            // Use your custom culture to mirror your other class
            return d.ToString("0.###", CommonFunctions.Culture);
        }
    }

}
