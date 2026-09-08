using QeDynamicDocumentProcessing.Common;
using System.Globalization;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    /// <summary>
    /// Class for handling Template: RINGAR_SV_TSD_VZ2L1
    /// </summary>
    public class RINGAR_SV_TSD_VZ2L1 : ITemplateCalculations
    {
        // Machines used by the Lotus/@Formula version of this template
        // (Nakamura/KT, MaxMuller/KT, LB45/KT)
        private static readonly string[] MachineValues =
        {
            "Nakamura/KT", "MaxMuller/KT", "LB45/KT"
        };

        private static readonly char[] Delimiters = { ' ', '/', '.', '-', '(', ')' };

        // IMPORTANT: Culture to support comma decimals if needed
        private static readonly CultureInfo Cult = CommonFunctions.Culture;

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var keyValues = new Dictionary<string, string>();

            // 1) Core calculation (Subject → dimensions, tolerances, text, guidance)
            CrutialParametersCalculation(
                keyValues,
                req.ProductDesignation,             // Subject line
                req.MachineNumber                    // MaskinVal (used downstream)
            );

            // 2) Machine: display + measurement plan + gauges + notes
            SumMaskinVal(keyValues, req.MachineNumber);
            SumF1(keyValues, req.MachineNumber);
            SumD1(keyValues, req.MachineNumber);
            SumAF1(keyValues, req.MachineNumber);
            SumTextS1(keyValues, req.MachineNumber);
            SumTextS2(keyValues, req.MachineNumber);
           

            return keyValues;
        }

        /// <summary>
        /// Reads Subject, parses tokens, finds series/type (and LU path),
        /// selects A..T row (TODO: your tables), computes tolerances and helper dimensions,
        /// sets drawing/roughness, angles, and VaL guidance.
        /// </summary>
        private void CrutialParametersCalculation(
            Dictionary<string, string> kv,
            string subject,
            string maskinVal
        )
        {
            // ------------------------------------------------------------
            // 1) READ & NORMALIZE SUBJECT
            // ------------------------------------------------------------
            string TmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string TmpBet = TmpFormat.Replace(".", ","); // use comma decimals

            // ------------------------------------------------------------
            // 2) DETECT FLAGS / TOKENS
            // ------------------------------------------------------------
            bool TmpLU = TmpBet.Contains("LU", StringComparison.Ordinal);
          
            bool TmpHasHy = TmpBet.Contains("-", StringComparison.Ordinal);
            bool TmpHasSl = TmpBet.Contains("/", StringComparison.Ordinal);
            bool TmpVZ2M4 = TmpBet.Contains("VZ2M4", StringComparison.Ordinal);
            bool TmpVZ2L1 = TmpBet.Contains("VZ2L1", StringComparison.Ordinal);
            int TmpCountB = TmpBet.Length;

            // ------------------------------------------------------------
            // 3) TOKENIZE (first 6 tokens)
            // ------------------------------------------------------------
            var tokens = TmpBet.Split(Delimiters, StringSplitOptions.RemoveEmptyEntries)
                               .Take(6)
                               .ToArray();

            string t1a = tokens.ElementAtOrDefault(0) ?? "0";
            string t2a = tokens.ElementAtOrDefault(1) ?? "0";
            string t3a = tokens.ElementAtOrDefault(2) ?? "0";
            string t4a = tokens.ElementAtOrDefault(3) ?? "0";
            string t5a = tokens.ElementAtOrDefault(4) ?? "0";
            string t6a = tokens.ElementAtOrDefault(5) ?? "0";

            // null/missing → "0" (already handled above)

            // Number vs text cast
            object t1 = ToNumberIfPossible(t1a);
            object t2 = ToNumberIfPossible(t2a);
            object t3 = ToNumberIfPossible(t3a);
            object t4 = ToNumberIfPossible(t4a);
            object t5 = ToNumberIfPossible(t5a);
            object t6 = ToNumberIfPossible(t6a);

            // ------------------------------------------------------------
            // 4) EXTRACT SERIES & TYPE  (primary + optional secondary)
            //    Based on the third (and sometimes the fourth) token
            // ------------------------------------------------------------
            string tmpBet3 = t3a;
            string tmpBet4 = t4a;

            string TmpSerie = (tmpBet3.Length > 3) ? tmpBet3[..2] : tmpBet3;
            string TmpTyp = (tmpBet3.Length >= 2) ? tmpBet3[^2..] : tmpBet3;

            string TmpSerie2 = (TmpCountB > 20)
                ? ((tmpBet4.Length > 3) ? tmpBet4[..2] : tmpBet4)
                : string.Empty;

            string TmpTyp2 = (TmpCountB > 20)
                ? ((tmpBet4.Length >= 2) ? tmpBet4[^2..] : tmpBet4)
                : string.Empty;

            bool IsSeriesOK = new[] { "30", "31", "32" }.Contains(TmpSerie);
            bool IsTypeOK = new[] { "44", "48", "52", "56", "60", "64", "68", "72", "76", "80", "84" }
                              .Contains(TmpTyp);

            int indexofTmpTyp = Array.IndexOf( new[] { "44", "48", "52", "56", "60", "64", "68", "72", "76", "80", "84" },TmpTyp) +1;
            // ------------------------------------------------------------
            // 5) LOOKUP TABLE SELECTION (A..T)
            //    TODO: paste your A..T series/type tables here.
            // ------------------------------------------------------------
            // The Lotus/@Formula uses long strings to map Type → "A:B:C:D:E:F:G:H:J:R:S:T".
            // Here we implement the same as strongly typed dictionaries:

            var row_30 = GetTmpLista30AT(indexofTmpTyp).Split(':');   // returns string[12]
            var row_31 = GetTmpLista31AT(indexofTmpTyp).Split(':'); 
            var row_32 = GetTmpLista32AT(indexofTmpTyp).Split(':');
            var row_LU = GetTmpListaLUAT(indexofTmpTyp).Split(':');
            

            string[] TmpListaA_T = TmpLU ? row_LU : SelectBySeries(row_30, row_31, row_32, TmpSerie);


            // ------------------------------------------------------------
            // 6) EXTRACT DIMENSIONS A..T
            // order per pseudocode: A, B, C, D, E, F, G, H, J, R, S, T
            // ------------------------------------------------------------
            double A = ParseD(TmpListaA_T[0]);
            double B = ParseD(TmpListaA_T[1]);
            double C = ParseD(TmpListaA_T[2]);
            double D = ParseD(TmpListaA_T[3]);
            double E = ParseD(TmpListaA_T[4]);
            double F = ParseD(TmpListaA_T[5]);
            double G = ParseD(TmpListaA_T[6]);
            string H = TmpListaA_T[7]; // count (can keep as text)
            double J = ParseD(TmpListaA_T[8]);
            double R = ParseD(TmpListaA_T[9]);
            string S = (TmpListaA_T != null && TmpListaA_T.Length > 10)
               ? TmpListaA_T[10]
               : "0"; ; // degrees or 0
            string T = (TmpListaA_T != null && TmpListaA_T.Length > 11)
               ? TmpListaA_T[11]
               : "";// degrees

            // ------------------------------------------------------------
            // 7) BUILD DISPLAY STRINGS + TOLERANCES
            // ------------------------------------------------------------
            kv["SumA"] = $"(A) {ToText(A)}";
            kv["SumATol"] = GenTolPM(A);

            kv["SumB"] = $"(B) {ToText(B)}";
            kv["SumBTol"] = "+ 0";
            kv["SumBTolN"] = H8Lower(B);

            kv["SumC"] = $"(C) {ToText(C)}";
            var a  = H12Upper(C);
            kv["SumCTol"] = H12Upper(C);
            kv["SumCTolN"] = "- 0";

            kv["SumD"] = $"(D) {ToText(D)}";
            kv["SumDTol"] = "+ 0.3";
            kv["SumDTolN"] = "- 0";

            kv["SumE"] = $"(E) {ToText(E)}";
            kv["SumETol"] = GenTolPM(E);

            kv["SumF"] = $"(F) {ToText(F)}";
            kv["SumFTol"] = GenTolPM(F);

            kv["SumG"] = $"(G) {ToText(G)}";
            kv["SumGTol"] = (G < 6.01) ? "± 0.1" : (G < 30.01) ? "± 0.2" : "± 0.3";

            kv["SumH"] = $"{H}x";

            kv["SumJ"] = $"(4x)(J) {ToText(J)}";
            kv["SumJTol"] = "± 0.1";

            // RHM helper: (A/2 - R) - (G/2)
            double RHM = (A / 2.0 - R) - (G / 2.0);
            kv["SumRHM"] = $"(Hjälpmått: {ToText(RHM)})";

            // Angles S, T
            if (S == "0" || S == "0,0" || S == "0.0")
                kv["SumS"] = "3 oljehål";
            else
                kv["SumS"] = $"{S}°";

            kv["SumT"] = $"{T}°";

            // ------------------------------------------------------------
            // 8) FIXED WIDTHS & DERIVED DIMENSIONS
            // ------------------------------------------------------------
            double B1 = 26;
            kv["SumB1"] = $"(B1) {ToText(B1)}";
            kv["SumB1Tol"] = (B1 < 6.01) ? "± 0.1" : (B1 < 30.01) ? "± 0.2" : "± 0.3";

            double B2 = 10;
            kv["SumB2"] = $"(B2) {ToText(B2)}";
            kv["SumB2Tol"] = "+ 0";
            kv["SumB2TolN"] = H8LowerSmall(B2);

            double B3 = TmpLU ? 13 : 18;
            kv["SumB3"] = $"(B3) {ToText(B3)}";
            kv["SumB3Tol"] = (B3 < 6.01) ? "± 0.1" : (B3 < 30.01) ? "± 0.2" : "± 0.3";

            double B4 = 5;
            kv["SumB4"] = $"(B4) {ToText(B4)}";
            kv["SumB4Tol"] = (B4 < 6.01) ? "± 0.1" : (B4 < 30.01) ? "± 0.2" : "± 0.3";

            double M = B1 - B2 - 6.8;
            kv["SumM"] = $"(M) {ToText(M)}";
            kv["SumMTol"] = (M < 6) ? "± 0.1" : (M < 30) ? "± 0.2" : (M < 120) ? "± 0.3" : "± 0.5";

            // ------------------------------------------------------------
            // 9) DRILLING PARAMETERS
            // ------------------------------------------------------------
            double BD = 6;
            kv["SumBD"] = $"(BD) {ToText(BD)}";
            kv["SumBDTol"] = (BD < 6) ? "± 0.1" : "± 0.2";

            double BM = 5;
            kv["SumBM"] = $"(BM) {ToText(BM)}";
            kv["SumBMTol"] = (BM < 3.1) ? "+ 0.100" : (BM < 6.1) ? "+ 0.120" : "+ 0.150";
            kv["SumBMTolN"] = "- 0";

            double BM1 = 5;
            kv["SumBM1"] = $"(BM1) {ToText(BM1)}";
            kv["SumBM1Tol"] = (BM1 < 6) ? "± 0.1" : "± 0.2";

            double BM2 = 3.5;
            kv["SumBM2"] = $"(4x)(BM2) {ToText(BM2)}";
            kv["SumBM2Tol"] = (BM2 < 6) ? "± 0.1" : "± 0.2";

            double HM = (A / 2.0) - J;
            kv["SumHM"] = $"(Hjälpmått: {ToText(HM)})";

            double K = 7;
            kv["SumK"] = $"(4x)(K) {ToText(K)}";
            kv["SumKTol"] = (K < 6) ? "± 0.1" : "± 0.2";

            double N = 5;
            kv["SumN"] = $"(4x)(N) {ToText(N)}";
            kv["SumNTol"] = (N < 3.1) ? "+ 0.100" : (N < 6.1) ? "+ 0.120" : "+ 0.150";
            kv["SumNTolN"] = "- 0";

            double P = 14;
            kv["SumP"] = $"(4x)(P) {ToText(P)}";
            kv["SumPTol"] = "± 0.1";

            double F9 = 7;
            kv["SumF9"] = $"(4x) {ToText(F9)}";

            double V90 = 90;
            kv["SumV90"] = $"(4x) {ToText(V90)}°";

            kv["SumSL"] = "Slits: 2.5";

            kv["SumV45_2"] = "45º";
            kv["SumV45"] = "45º";
            kv["SumF45"] = "1x45º";

            // ------------------------------------------------------------
            // 10) SURFACE & DRAWING
            // ------------------------------------------------------------
            string Ra = "6.3";
            string Rit = "7433198:2";
            kv["SumRit"] = Rit;
            kv["SumRit2"] = Rit;
            // (Ra is used in notes; adding for convenience)
            kv["SumRa"] = Ra;

            // ------------------------------------------------------------
            // 11) VaL GUIDANCE (template check)
            // ------------------------------------------------------------
            if (TmpVZ2L1)
                kv["VaLTyp"] = ""; // correct template
            else if (TmpVZ2M4)
                kv["VaLTyp"] = "Använd särskild mall för VZ2M4";
            else
                kv["VaLTyp"] = "Denna mall är för VZ2L1, delad ring med 5 borrhål";
        }

        // ------------------------------------------------------------
        // MACHINE: DISPLAY
        // ------------------------------------------------------------
        private void SumMaskinVal(Dictionary<string, string> kv, string maskinVal)
        {
            string turningName = maskinVal switch
            {
                "Nakamura/KT" => "Svarvning - Nakamura",
                "MaxMuller/KT" => "Svarvning - MaxMuller",
                "LB45/KT" => "Svarvning - LB45",
                _ => string.Empty
            };

            string drillingName = MachineValues.Contains(maskinVal) ? "Borrning - KT" : string.Empty;

            if (!string.IsNullOrEmpty(turningName))
                kv["SumMaskinValS1"] = $"Maskin: {turningName}";
            else
                kv["SumMaskinValS1"] = string.Empty;

            if (!string.IsNullOrEmpty(drillingName))
                kv["SumMaskinValS2"] = $"Maskin: {drillingName}";
            else
                kv["SumMaskinValS2"] = string.Empty;
        }

        // ------------------------------------------------------------
        // MEASUREMENT FREQUENCIES (F1_* and F2_*)
        // ------------------------------------------------------------
        private void SumF1(Dictionary<string, string> kv, string maskinVal)
        {
            bool ok = MachineValues.Contains(maskinVal);

            // Page 1
            kv["SumF1_1"] = ok ? "1/1" : "";
            kv["SumF1_2"] = ok ? "1/1" : "";
            kv["SumF1_3"] = ok ? "1/3" : "";
            kv["SumF1_4"] = ok ? "1/3" : "";
            kv["SumF1_5"] = ok ? "1/5" : "";
            kv["SumF1_6"] = ok ? "Inst." : "";
            kv["SumF1_7"] = ok ? "Inst." : "";
            kv["SumF1_8"] = ok ? "1/3" : "";
            kv["SumF1_9"] = ok ? "1/5" : "";

            // Page 2 (examples replicating the logic from the pseudocode)
            kv["SumF2_1"] = ok ? (maskinVal == "Nakamura/KT" ? "1/3" : "1/5") : "";
            kv["SumF2_2"] = ok ? "1/3" : "";
            kv["SumF2_3"] = ok ? "1/5" : "";
            kv["SumF2_4"] = ok ? "" : "";
            kv["SumF2_5"] = ok ? "Inst." : "";
        }

        // H12 upper tolerance for C (lower is -0)
        // Mirrors the exact breakpoints from your pseudocode/Notes formula
        private static string H12Upper(double valueC)
        {
            if (valueC < 80.01) return "+ 0.300";
            if (valueC < 120.01) return "+ 0.350";
            if (valueC < 180.01) return "+ 0.400";
            if (valueC < 250.01) return "+ 0.460";
            if (valueC < 315.01) return "+ 0.520";
            if (valueC < 400.01) return "+ 0.570";
            if (valueC < 500.01) return "+ 0.630";
            if (valueC < 630.01) return "+ 0.700";
            if (valueC < 800.01) return "+ 0.800";
            if (valueC < 1000.01) return "+ 0.900";
            if (valueC < 1250.01) return "+ 1.050";
            if (valueC < 1600.01) return "+ 1.250";
            if (valueC < 2000.01) return "+ 1.500";
            if (valueC < 2500.01) return "+ 1.750";
            return "+ 2.100";
        }

        // ------------------------------------------------------------
        // GAUGES (Mätdon)
        // ------------------------------------------------------------
        private void SumD1(Dictionary<string, string> kv, string maskinVal)
        {
            bool ok = MachineValues.Contains(maskinVal);

            kv["SumD1_1"] = ok ? "Mikrometer/UD-Apparat" : "";
            kv["SumD1_2"] = ok ? "Mikrometer" : "";
            kv["SumD1_3"] = ok ? "Digitalt Djup/Hakmått" : "";
            kv["SumD1_4"] = ok ? "Digitalt Djup/Hakmått" : "";
            kv["SumD1_5"] = ok ? "Skjutmått" : "";
            kv["SumD1_6"] = ok ? "Vinkelsystem" : "";
            kv["SumD1_7"] = ok ? "Vinkelsystem" : "";
            kv["SumD1_8"] = ok ? "Ytjämnhetsmätare" : "";
            kv["SumD1_9"] = ok ? "Skjutmått" : "";

            // SID 2
            kv["SumD2_1"] = ok ? "Skjutmått" : "";
            kv["SumD2_2"] = ok ? "Skjutmått" : "";
            kv["SumD2_3"] = ok ? "Skjutmått" : "";
            kv["SumD2_4"] = ok ? "" : "";
            kv["SumD2_5"] = ok ? "Skjutmått" : "";
        }

        // ------------------------------------------------------------
        // NOTES (Anmärkningsfält) & General Text
        // ------------------------------------------------------------
        private void SumAF1(Dictionary<string, string> kv, string maskinVal)
        {
            bool ok = MachineValues.Contains(maskinVal);

            kv["SumAF1_1"] = ok ? "" : "";
            kv["SumAF1_2"] = ok ? "" : "";
            kv["SumAF1_3"] = ok ? "" : "";
            kv["SumAF1_4"] = ok ? "" : "";
            kv["SumAF1_5"] = ok ? "" : "";
            kv["SumAF1_6"] = ok ? "" : "";
            kv["SumAF1_7"] = ok ? "" : "";

            // Examples from the @Formula script:
            kv["SumAF1_8"] = ok ? $"Bearbetas Ra {kv.GetValueOrDefault("SumRa", "6.3")} runt om" : "";

            kv["SumAF1_9"] = ok ? "" : "";

            // SID 2
            kv["SumAF2_1"] = ok ? "Placering kontolleras med hjälpmått" : "";
            kv["SumAF2_2"] = ok ? "" : "";
            kv["SumAF2_3"] = ok ? "Kan mätas med hjälpmåtten" : "";
            kv["SumAF2_4"] = ok ? "" : "";
            kv["SumAF2_5"] = ok ? "" : "";
        }

        private void SumTextS1(Dictionary<string, string> kv, string maskinVal)
        {
            kv["SumTextS1"] =
                "Okulärkontroll Grader, frifläcker, slagmärken, repor, valkar, ytjämnhet och faser, skarpa kanter avgradas.";
        }

        private void SumTextS2(Dictionary<string, string> kv, string maskinVal)
        {
            kv.Add("SumTextS2", "Okulärkontroll Grader, frifläcker, slagmärken, repor, valkar, ytjämnhet och faser, skarpa kanter avgradas.");
            
        }

        // ------------------------------------------------------------
        // Helpers
        // ------------------------------------------------------------
        private static object ToNumberIfPossible(string s)
        {
            if (double.TryParse(s, NumberStyles.Any, Cult, out double d) && Math.Abs(d) > double.Epsilon)
                return d;
            return s;
        }

        private static double ParseD(string s)
        {
            return double.TryParse(s, NumberStyles.Any, Cult, out double d) ? d : 0.0;
        }

        private static string ToText(double d) => d.ToString("0.####", Cult);

        // ± tolerance rule used for A, E, F, etc.
        private static string GenTolPM(double value)
        {
            if (value < 6.01) return "± 0.1";
            if (value < 30.01) return "± 0.2";
            if (value < 120.01) return "± 0.3";
            if (value < 315.01) return "± 0.5";
            if (value < 1000.01) return "± 0.8";
            if (value < 2000.01) return "± 1.2";
            return "± 2.0";
        }

        // h8 lower tolerance for B (upper is +0)
        private static string H8Lower(double b)
        {
            if (b < 80.01) return "- 0.046";
            if (b < 120.01) return "- 0.054";
            if (b < 180.01) return "- 0.063";
            if (b < 250.01) return "- 0.072";
            if (b < 315.01) return "- 0.081";
            if (b < 400.01) return "- 0.089";
            if (b < 500.01) return "- 0.097";
            return "- 0.110";
        }

        // h8 table used for small B2
        private static string H8LowerSmall(double b2)
        {
            if (b2 < 3.01) return "- 0.014";
            if (b2 < 6.01) return "- 0.018";
            if (b2 < 10.01) return "- 0.022";
            if (b2 < 18.01) return "- 0.027";
            if (b2 < 30.01) return "- 0.033";
            return "- 0.039";
        }

        private static string[]? SelectBySeries(string[] row30, string[] row31, string[] row32, string series)
        {
            return series switch
            {
                "30" => row30,
                "31" => row31,
                "32" => row32,
                _ => null
            };
        }

        private static string GetTmpLista31AT(int tmpTypLista)
        {

             const string Mapping =
            "44-48-340:331:311:245:330:340:20:3:158,5:145,5:0:25-56-3060-400:391:371:305:390:400:20:5:191:175,5:15-420:411:391:325:410:420:20:5:201:185,5:15:30-460:451:431:345:450:460:20:5:221:205,5:15:30-500:491:471:365:490:500:20:5:241:225,5:15:30-3080-500:491:471:405:490:500:20:5:241:225,5:15:30";

            var parts = Mapping.Split('-');

        // Lotus @Word rule: 0 is equivalent to 1; negative/out-of-range -> empty.  [1](https://dl.booksee.org/genesis/556000/2a580f9bf862f14f7325d1a4b4c57621/_as/%25255BMercer%25255D_Creating_Value_Through_People_Discussion%2528BookSee.org%2529.pdf)
        int index = tmpTypLista <= 0 ? 1 : tmpTypLista;

            if (index > parts.Length) return string.Empty; // out-of-range => ""  [1](https://dl.booksee.org/genesis/556000/2a580f9bf862f14f7325d1a4b4c57621/_as/%25255BMercer%25255D_Creating_Value_Through_People_Discussion%2528BookSee.org%2529.pdf)

            return parts[index - 1];

           
        }


        // Serie 30
        private static string GetTmpLista30AT(int tmpTypLista)
        {
            const string Mapping =
                "44-48-52-56-380:371:350:285:370:380:20:5:181:165:15:30-" +
                "64-68-72-76-460:451:431:385:450:460:20:5:221:205,5:15:30-" +
                "84";

            var parts = Mapping.Split('-');

            // Lotus-like rule: 0 behaves as 1; out-of-range => ""
            int index = tmpTypLista <= 0 ? 1 : tmpTypLista;
            if (index > parts.Length) return string.Empty;

            return parts[index - 1];
        }

        // Serie 32
        private static string GetTmpLista32AT(int tmpTypLista)
        {
            const string Mapping =
                "44-48-52-56-60-64-477:467:447:325:467:477:20:5:228:213,5:15:30-" +
                "3172-3176-526:516:496:385:516:526:20:5:253:238:15:30-" +
                "520:511:491:405:510:520:20:5:251:225,5:15:30";

            var parts = Mapping.Split('-');

            // Lotus-like rule: 0 behaves as 1; out-of-range => ""
            int index = tmpTypLista <= 0 ? 1 : tmpTypLista;
            if (index > parts.Length) return string.Empty;

            return parts[index - 1];
        }

        // LU
        private static string GetTmpListaLUAT(int tmpTypLista)
        {
            const string Mapping =
                "44-48-52-56-60-64-68-72-76-80-520:511:491:405:500:520:20:5:251:225,5:15:30";

            var parts = Mapping.Split('-');

            // Lotus-like rule: 0 behaves as 1; out-of-range => ""
            int index = tmpTypLista <= 0 ? 1 : tmpTypLista;
            if (index > parts.Length) return string.Empty;

            return parts[index - 1];
        }
    }
}