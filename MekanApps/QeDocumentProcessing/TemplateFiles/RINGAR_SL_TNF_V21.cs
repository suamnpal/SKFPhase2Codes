using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    /// <summary>
    /// Class for handling Template: RINGAR SL-TNF V21
    /// Mirrors Lotus Notes logic from _Qe_SL-TNF_V21_Formler.txt
    /// </summary>
    public class RINGAR_SL_TNF_V21 : ITemplateCalculations
    {
        private static readonly string[] Machines = { "Nakamura", "MaxMuller", "LB45" };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var keyValues = new Dictionary<string, string>();

            string subject = req?.ProductDesignation ?? string.Empty;
            string maskinVal = req?.MachineNumber ?? string.Empty;

            ComputeCore(keyValues, subject, maskinVal);
            SumMaskinVal(keyValues, maskinVal);
            SumFrequencies(keyValues, maskinVal);
            SumMeasuringDevices(keyValues, maskinVal);
            SumAF(keyValues, maskinVal);
            SumText(keyValues);

            return keyValues;
        }

        /// <summary>
        /// Core geometry, tolerances, and drawing selection
        /// </summary>
        private void ComputeCore(Dictionary<string, string> kv, string subject, string maskinVal)
        {
            // --- Formatting and tokenization (uppercase+trim, '.'->',') ---
            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");
            var bets = CommonFunctions.CalculateBets(tmpBet); // provides TmpBet2..TmpBet6 etc., as in your other class

            bool hasSlash = tmpBet.Contains("/");
            bool hasV21 = tmpBet.Contains("V21");

            // Extract Typ (third token in Notes)
            string tmpTypText = bets.TmpBet3 ?? string.Empty;
            int tmpTyp = 0;
            int.TryParse(tmpTypText, NumberStyles.Any, CommonFunctions.Culture, out tmpTyp);

            // ===== V21 dimension lists (only Typ 68 or 88) =====
            // Notes mapping:
            // Typ 68 -> { D1=360, D2=320, D3=379, D4=390 }
            // Typ 88 -> { D1=420, D2=380, D3=435, D4=441 }
            double? D1 = null, D2 = null, D3 = null, D4 = null;

            if (tmpTyp == 68)
            {
                D1 = 360; D2 = 320; D3 = 379; D4 = 390;
            }
            else if (tmpTyp == 88)
            {
                D1 = 420; D2 = 380; D3 = 435; D4 = 441;
            }

            // ===== Validation: Only for V21 variants =====
            kv["VaLV21"] = hasV21 ? "" : "Denna mall avser bara V21 varianterna";

            // ===== D1 =====
            if (D1.HasValue)
            {
                kv["SumD1"] = $"(D1) {D1.Value.ToString(CommonFunctions.Culture)}";
                kv["SumD1Tol"] = (D1.Value < 400) ? "± 0.5" : "± 0.8";
            }
            else
            {
                kv["SumD1"] = ""; kv["SumD1Tol"] = "";
            }

            // ===== D2 & tolerances (machine dependent output) =====
            if (D2.HasValue)
            {
                var d2 = D2.Value;
                // tol table (Notes numbers rounded to 3 decimals)
                double tol = d2 < 120 ? 0.090 :
                             d2 < 185 ? 0.106 :
                             d2 < 245 ? 0.122 :
                             d2 < 315 ? 0.137 :
                             d2 < 405 ? 0.151 :
                             d2 < 510 ? 0.165 : 0.186;
                double tolN = d2 < 120 ? 0.036 :
                              d2 < 185 ? 0.043 :
                              d2 < 245 ? 0.050 :
                              d2 < 315 ? 0.056 :
                              d2 < 405 ? 0.062 :
                              d2 < 510 ? 0.068 : 0.076;

                bool isNakamura = string.Equals(maskinVal, "Nakamura", StringComparison.OrdinalIgnoreCase);

                double displayD2 = isNakamura ? d2 : d2 + tolN;
                double displayTol = isNakamura ? tol : (tol - tolN);

                kv["SumD2"] = $"(D2) {displayD2.ToString("0.###", CommonFunctions.Culture)}";
                kv["SumD2Tol"] = $"+ {displayTol.ToString("0.###", CommonFunctions.Culture)}";
                kv["SumD2TolN"] = isNakamura ? $"+ {tolN.ToString("0.###", CommonFunctions.Culture)}" : " - 0";
            }
            else
            {
                kv["SumD2"] = ""; kv["SumD2Tol"] = ""; kv["SumD2TolN"] = "";
            }

            // ===== D3 =====
            if (D3.HasValue)
            {
                var d3 = D3.Value;
                string tolP = d3 < 180 ? "+ 0.25" :
                              d3 < 255 ? "+ 0.29" :
                              d3 < 315 ? "+ 0.32" :
                              d3 < 395 ? "+ 0.36" :
                              d3 < 500 ? "+ 0.40" : "+ 0.44";
                kv["SumD3"] = $"(D3) {d3.ToString(CommonFunctions.Culture)}";
                kv["SumD3Tol"] = tolP;
                kv["SumD3TolN"] = " - 0";
            }
            else
            {
                kv["SumD3"] = ""; kv["SumD3Tol"] = ""; kv["SumD3TolN"] = "";
            }

            // ===== D4 =====
            if (D4.HasValue)
            {
                var d4 = D4.Value;
                string tolN = d4 < 170 ? "- 0.25" :
                              d4 < 270 ? "- 0.29" :
                              d4 < 305 ? "- 0.32" :
                              d4 < 405 ? "- 0.36" :
                              d4 < 500 ? "- 0.40" : "- 0.44";

                kv["SumD4"] = $"(D4) {d4.ToString(CommonFunctions.Culture)}";
                kv["SumD4Tol"] = "+ 0";
                kv["SumD4TolN"] = tolN;
            }
            else
            {
                kv["SumD4"] = ""; kv["SumD4Tol"] = ""; kv["SumD4TolN"] = "";
            }

            // ===== Thread & chord text (F) =====
            // F = "M6 (2x) 120°", chord = round((D1/2) * 2*sin(60°), 0.1)
            if (D1.HasValue)
            {
                double sin60 = Math.Sin(Math.PI / 3.0); // 60°
                double konst = Math.Round(2 * sin60, 4); // Notes: Round(...;0,0001) -> 1.7321
                double korda = Math.Round((D1.Value / 2.0) * konst, 1);
                string kordaText = $" Korda mellan gänghål: {korda.ToString("0.0", CommonFunctions.Culture)}";
                kv["SumF"] = $"M6 (2x) 120°{kordaText}";
            }
            else
            {
                kv["SumF"] = "";
            }

            // ===== A / B / C / E =====
            // A: 68 -> 27.5, 88 -> 25; ±0.2
            // B: 68 -> 18.5, 88 -> 16; ±0.2
            // C: 68|88 -> 12.5; +0 / -0.2
            // E: 5; ±0.1
            if (tmpTyp == 68 || tmpTyp == 88)
            {
                double A = (tmpTyp == 68) ? 27.5 : 25.0;
                double B = (tmpTyp == 68) ? 18.5 : 16.0;
                double C = 12.5;
                double E = 5.0;

                kv["SumA"] = $"(A) {A.ToString(CommonFunctions.Culture)}";
                kv["SumATol"] = "± 0.2";

                kv["SumB"] = $"(B) {B.ToString(CommonFunctions.Culture)}";
                kv["SumbTol"] = "± 0.2";

                kv["SumC"] = $"(C) {C.ToString(CommonFunctions.Culture)}";
                kv["SumCTol"] = " 0";
                kv["SumCTolN"] = "- 0.2";

                kv["SumE"] = $"(E) {E.ToString(CommonFunctions.Culture)}";
                kv["SumETol"] = "± 0.1";
            }
            else
            {
                kv["SumA"] = ""; kv["SumATol"] = "";
                kv["SumB"] = ""; kv["SumbTol"] = "";
                kv["SumC"] = ""; kv["SumCTol"] = ""; kv["SumCTolN"] = "";
                kv["SumE"] = ""; kv["SumETol"] = "";
            }

            // ===== Chamfers / radii & surface =====
            kv["SumG"] = "1x45°";
            kv["SumG1"] = "1x45°";
            kv["SumG2"] = "1x45°";
            kv["SumR"] = "Max R 0.8";

            kv["SumRa1"] = "3.2";  // general surface
            string tmpRa = "Ra=12.5"; // used in AF field
            // store for AF method via kv so we don't recompute:
            kv["__TmpRa"] = tmpRa;

            // ===== Drawing number selection =====
            // Notes: if no slash -> "7439486"; else if Typ=68 -> "7439503"; else "7439488"
            string rit;
            if (!hasSlash)
                rit = "7439486";
            else if (tmpTyp == 68)
                rit = "7439503";
            else
                rit = "7439488";
            kv["SumRit"] = rit;
        }

        /// <summary>
        /// Maskinval line ("Maskin: ... - ...")
        /// </summary>
        private void SumMaskinVal(Dictionary<string, string> kv, string maskinVal)
        {
            string mv = maskinVal ?? string.Empty;
            string tmpMaskinVal =
                string.Equals(mv, "MaxMuller", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(mv, "LB45", StringComparison.OrdinalIgnoreCase)
                    ? "Borring i skepp 6"
                    : string.Empty;

            string tmpMaskinValS1 =
                string.Equals(mv, "Nakamura", StringComparison.OrdinalIgnoreCase) ? "Nakamura" :
                string.Equals(mv, "MaxMuller", StringComparison.OrdinalIgnoreCase) ? "MaxMuller" :
                string.Equals(mv, "LB45", StringComparison.OrdinalIgnoreCase) ? "LB45" : string.Empty;

            kv["SumMaskinValS1"] = string.IsNullOrEmpty(tmpMaskinValS1)
                ? string.Empty
                : $"Maskin: {tmpMaskinValS1}" + (string.IsNullOrEmpty(tmpMaskinVal) ? "" : $" - {tmpMaskinVal}");
        }

        /// <summary>
        /// Measuring frequencies (F)
        /// </summary>
        private void SumFrequencies(Dictionary<string, string> kv, string maskinVal)
        {
            bool match = Machines.Contains(maskinVal ?? string.Empty, StringComparer.OrdinalIgnoreCase);

            kv["SumF1_1"] = match ? "1/1" : "";
            kv["SumF1_2"] = match ? "1/3" : "";
            kv["SumF1_3"] = match ? "1/3" : "";
            kv["SumF1_4"] = match ? "1/3" : "";
            // Notes script didn't set a 5th frequency line for this template
        }

        /// <summary>
        /// Measuring devices (D)
        /// </summary>
        private void SumMeasuringDevices(Dictionary<string, string> kv, string maskinVal)
        {
            bool match = Machines.Contains(maskinVal ?? string.Empty, StringComparer.OrdinalIgnoreCase);

            kv["SumD1_1"] = match ? "Skjutmått" : "";
            kv["SumD1_2"] = match ? "Gängtolk" : "";
            kv["SumD1_3"] = match ? "Ytjämnhetsmätare" : "";
            kv["SumD1_4"] = match ? "Skjutmått/Djupmått" : "";
        }

        /// <summary>
        /// Remarks (AF)
        /// </summary>
        private void SumAF(Dictionary<string, string> kv, string maskinVal)
        {
            bool match = Machines.Contains(maskinVal ?? string.Empty, StringComparer.OrdinalIgnoreCase);
            kv["SumAF1_1"] = match ? "" : "";
            kv["SumAF1_2"] = match ? "" : "";
            kv["SumAF1_3"] = match ? $"Övrig ytjämnhet: {kv.GetValueOrDefault("__TmpRa", "Ra=12.5")}" : "";
            kv["SumAF1_4"] = match ? "" : "";

            // clean up temp
            kv.Remove("__TmpRa");
        }

        /// <summary>
        /// Other text line
        /// </summary>
        private void SumText(Dictionary<string, string> kv)
        {
            kv["SumTextS1"] = "Skarpa kanter avgradas";
        }
    }
}