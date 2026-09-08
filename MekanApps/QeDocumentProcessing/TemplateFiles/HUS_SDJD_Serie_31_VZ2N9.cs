using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HUS_SDJD_Serie_31_VZ2N9 : ITemplateCalculations
    {
        private static readonly string[] Machines = new[] { "OKUMA MA600/Trevisan DS 450", "Trevisan DS 900" };

        private static readonly string[] Typ31 = { "68", "72", "76", "80", "84", "88", "92", "96" };

        private static readonly double[] Ad31 = new double[] { 342, 362, 382, 402, 422, 432, 452, 472 };
        private static readonly double[] Td31 = new double[] { 390, 417, 441, 463, 450, 460, 480, 500 };
        private static readonly double[] Ab31 = new double[] { 400, 400, 435, 435, 470, 470, 510, 510 };
        private static readonly double[] Ld31 = new double[] { 580, 600, 620, 650, 700, 720, 760, 790 };
        private static readonly double[] Lb31 = new double[] { 210, 212, 214, 220, 244, 246, 260, 268 };
        private static readonly double[] Hd31 = new double[] { 700, 700, 750, 750, 810, 810, 880, 880 };
        private static readonly double[] Uh31 = new double[] { 360, 360, 390, 390, 440, 440, 470, 470 };
        private static readonly double[] Fh31 = new double[] { 75, 75, 80, 80, 85, 85, 85, 85 };
        private static readonly double[] Fl31 = new double[] { 830, 830, 880, 880, 940, 940, 1040, 1040 };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);
            kv["SumMaskinVal"] = "SDJD Serie 31";
            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");

            bool tmpSlash = tmpBet.IndexOf('/') >= 0;
            bool tmpVZ2N9 = tmpBet.IndexOf("VZ2N9", StringComparison.OrdinalIgnoreCase) >= 0;

            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : string.Empty;
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : string.Empty;

            string tmpSerie = tmpBet2.Length >= 2 ? tmpBet2.Substring(0, 2) : tmpBet2;
            string tmpTypStr = tmpBet2.Length >= 2 ? tmpBet2.Substring(tmpBet2.Length - 2) : tmpBet2;

            int serieInt = TryParseInt(tmpSerie);
            double typNum = TryParseDouble(tmpTypStr);

            int typLista = GetMember(tmpTypStr, Typ31);  // 1-based; 0 = not found
            kv["VaLTypLista"] = typLista == 0
                ? "Produktbeteckningen ingår ej i mallen, kontrollera stavning och/eller kontakta Qe-Admin för att lägga till produkten"
                : "";

            // ── Ra ──────────────────────────────────────────────────────────────
            kv["SumRa32"] = "3.2";
            kv["SumRa32a"] = "3.2";
            kv["SumRa63"] = "6.3 [3]";
            kv["SumRa63a"] = "6.3"; kv["SumRa63b"] = "6.3"; kv["SumRa63c"] = "6.3";
            kv["SumRa63d"] = "6.3"; kv["SumRa63e"] = "6.3"; kv["SumRa63f"] = "6.3";
            kv["SumRa63g"] = "6.3";

            // ── Wt ──────────────────────────────────────────────────────────────
            kv["SumWt35"] = "Wt 35";
            kv["SumWt20"] = "Wt 20";

            // ── Ad (axelhålsdiameter) ────────────────────────────────────────────
            double tmpAd = GetVal(Ad31, typLista);
            kv["SumAd"] = "2x (Ad) " + Fmt(tmpAd);
            kv["SumAdTol"] = "+ " + Fmt1(1.0);
            kv["SumAdTolN"] = "+ " + Fmt0(0);        // NOTE: "+" prefix, TmpKon0=F0

            // ── Td (tätningshålsdiameter) ────────────────────────────────────────
            double tmpTd = GetVal(Td31, typLista);
            kv["SumTd"] = "2x (Td) " + Fmt(tmpTd);
            kv["SumTdTol"] = "+ " + Fmt3(0.5);
            kv["SumTdTolN"] = "+ " + Fmt3(0.2);      // NOTE: "+" prefix

            // ── Tb (tätningshålsbredd) fixed = 6 ────────────────────────────────
            kv["SumTb"] = "2x (Tb) 6";
            kv["SumTbTol"] = "+ " + Fmt3(0.5);
            kv["SumTbTolN"] = "- " + Fmt0(0);

            // ── Chamfers & radii ─────────────────────────────────────────────────
            kv["SumF1"] = "";
            kv["SumF2"] = "2x Rmax 0.5";

            // ── Lö (lyftögla) ────────────────────────────────────────────────────
            double tmpLo = typNum < 92 ? 50 : 60;
            kv["SumLö"] = "2x Ø" + Fmt(tmpLo);

            // ── Borrhål sidor ────────────────────────────────────────────────────
            kv["SumJ"] = "16x M10";
            kv["SumJ1"] = "16x min20";
            kv["SumJ2"] = "16x max24";
            kv["SumJt"] = "8x varje sida";

            // ── C (plan), C1 (plan Ø) ────────────────────────────────────────────
            kv["SumC"] = "2x " + Fmt1(1.5);
            kv["SumCTol"] = "± " + Fmt1(1.0);
            kv["SumC1"] = "2x Ø32";
            kv["SumC1Tol"] = "+ " + Fmt1(1.0);
            kv["SumC1TolN"] = "- " + Fmt0(0);

            // ── D, D1, D2 ────────────────────────────────────────────────────────
            kv["SumD"] = "2x Ø32";
            kv["SumDTol"] = "+ " + Fmt1(1.0);
            kv["SumDTolN"] = "- " + Fmt0(0);
            kv["SumD1"] = "2x " + Fmt1(1.5);
            kv["SumD1Tol"] = "± " + Fmt1(1.0);
            kv["SumD2"] = "2x" + Fmt1(1.0);
            kv["SumD2Tol"] = "+ " + Fmt1(1.0);
            kv["SumD2TolN"] = "- " + Fmt3(0.5);

            // ── Ab (axelhålsbredd) ───────────────────────────────────────────────
            double tmpAb = GetVal(Ab31, typLista);
            kv["SumAb"] = "(Ab) " + Fmt(tmpAb);
            kv["SumAbTol"] = "± " + Fmt1(1.0);

            // ── Ld (lagerlägesdiameter) + G7 tol + styrgränser ──────────────────
            double tmpLd = GetVal(Ld31, typLista);
            double ldPos = G7Pos(tmpLd);
            double ldNeg = G7Neg(tmpLd);
            double p = typNum < 80 ? 0.76 : (typNum < 500 ? 0.79 : 0.81);
            double ldTolS = Math.Round(ldNeg + (ldPos - ldNeg) * p, 3);
            double ldTolNS = Math.Round(ldPos - (ldPos - ldNeg) * p, 3);
            string sg100 = typNum < 80 ? "100-1" : (typNum < 500 ? "100-2" : "100-3");

            kv["SumLd"] = "(Ld) " + Fmt(tmpLd);
            kv["SumLdTolS"] = "+ " + Fmt3(ldTolS) + "*";
            kv["SumLdTolNS"] = "+ " + Fmt3(ldTolNS) + "*";
            kv["SumLdTol"] = "+ " + Fmt3(ldPos) + " [3]";
            kv["SumLdTolN"] = "+ " + Fmt3(ldNeg) + " [2D]";
            kv["SG100-x"] = sg100;

            // ── Lb (bredd lagerläge) + H12 ───────────────────────────────────────
            double tmpLb = GetVal(Lb31, typLista);
            kv["SumLb"] = "(Lb) " + Fmt(tmpLb);
            kv["SumLbTol"] = "+ " + Fmt3(H12Tol(tmpLb));
            kv["SumLbTolN"] = "- " + Fmt0(0);

            // ── Hd (hopslagningshål) + JS13 ──────────────────────────────────────
            double tmpHd = GetVal(Hd31, typLista);
            kv["SumHd"] = "(Hd) " + Fmt(tmpHd);
            kv["SumHdTol"] = "± " + Fmt3(JS13Tol(tmpHd));

            // ── G (nippel) + Gd ──────────────────────────────────────────────────
            kv["SumG"] = "3x G1/4";
            kv["SumGd"] = "3x min 12";

            // ── G1 (hopslagningsbult) ────────────────────────────────────────────
            double tmpG1 = typNum < 84 ? 36 : 42;
            kv["SumG1"] = "(G1) M" + Fmt(tmpG1) + " 6H";

            // ── G1d (gängdjup) ───────────────────────────────────────────────────
            double tmpG1d = typNum < 84 ? 70 : (typNum < 500 ? 85 : 110);
            kv["SumG1d"] = "(G1d) " + Fmt(tmpG1d);
            kv["SumG1dTol"] = "± " + Fmt3(0.3);

            // ── B1d (borrdjup hopslagningsbult) ─────────────────────────────────
            double tmpB1d = Math.Abs(tmpG1d - 70) < 0.001 ? 80 : (Math.Abs(tmpG1d - 85) < 0.001 ? 100 : 130);
            kv["SumB1d"] = "(B1d) " + Fmt(tmpB1d);
            kv["SumB1dTol"] = "± " + Fmt3(0.3);

            // ── Bd (hopslagningshålsdiameter) + H15 ─────────────────────────────
            double tmpBd = tmpG1 == 24 ? 27 : tmpG1 == 30 ? 33 : tmpG1 == 36 ? 39 : 45;
            kv["SumBd"] = "(Bd) " + Fmt(tmpBd);
            kv["SumBdTol"] = "+ " + Fmt3(H15Tol(tmpBd));
            kv["SumBdTolN"] = "- " + Fmt0(0);

            // ── Sd (stifthål) + H12 ──────────────────────────────────────────────
            kv["SumSd"] = "2x (Sd) 16";
            kv["SumSdTol"] = "+ " + Fmt3(0.18);
            kv["SumSdTolN"] = "- " + Fmt0(0);

            // ── Sö / Su (borrdjup stifthål) ──────────────────────────────────────
            kv["SumSö"] = "(Sö) 20";
            kv["SumSöTol"] = "± " + Fmt3(0.42);
            kv["SumSu"] = "(Su) 20";
            kv["SumSuTol"] = "± " + Fmt3(0.42);

            // ── Uh (höjd underhalva) + Ch + JS11 ────────────────────────────────
            double tmpUh = GetVal(Uh31, typLista);
            kv["SumUh"] = "(Uh) " + Fmt(tmpUh);
            kv["SumUhTol"] = "± " + Fmt3(JS11Tol(tmpUh));
            kv["SumCh"] = "(Ch) " + Fmt(tmpUh);
            kv["SumChTol"] = kv["SumUhTol"];

            // ── Fh (fothöjd) ─────────────────────────────────────────────────────
            double tmpFh = GetVal(Fh31, typLista);
            kv["SumFh"] = "(Fh) " + Fmt(tmpFh);
            kv["SumFhTol"] = "± " + Fmt3(0.5);

            // ── Fl (fotlängd) + JS10 ─────────────────────────────────────────────
            double tmpFl = GetVal(Fl31, typLista);
            kv["SumFl"] = "(Fl) " + Fmt(tmpFl);
            kv["SumFlTol"] = "± " + Fmt3(JS10Tol(tmpFl));

            // ── Bp (bultplan) ─────────────────────────────────────────────────────
            double tmpBp = typNum < 84 ? 160 : 210;
            kv["SumBp"] = "(Bp) " + Fmt(tmpBp);
            kv["SumBpTol"] = "± " + Fmt3(0.5);

            // ── Plan / flatness ───────────────────────────────────────────────────
            kv["SumPl"] = Fmt2(0.05);
            kv["SumFp"] = typNum < 92 ? Fmt3(0.120) : Fmt3(0.135);

            // ── Misc ──────────────────────────────────────────────────────────────
            kv["SumSkr"] = "8.8 SNL.";

            // ── Machine ───────────────────────────────────────────────────────────
            string s1 = EqualsI(maskinVal, "OKUMA MA600/Trevisan DS 450") ? "Trevisan DS 450"
                      : EqualsI(maskinVal, "Trevisan DS 900") ? "Trevisan DS 900" : "";
            string s2 = EqualsI(maskinVal, "OKUMA MA600/Trevisan DS 450") ? "OKUMA MA600"
                      : EqualsI(maskinVal, "Trevisan DS 900") ? "Trevisan DS 900" : "";
            kv["SumMaskinValS1"] = "OP 2 - " + s1 + " - Svarvning";
            kv["SumMaskinValS2"] = "OP 1 - " + s2 + " - Borrning, fräsning";
   

            SumFrequencies(kv, maskinVal);
            SumMeasuringDevices(kv, maskinVal);
            SumAF(kv, maskinVal, sg100);

            // ── Text ─────────────────────────────────────────────────────────────
            kv["SumTextS1"] = "Kontrolleras enl. styrplan";
            kv["SumTextS2"] = "Kontrolleras enl. styrplan Alla materialdefekter utsorteras.";

            // ── Ritningar ─────────────────────────────────────────────────────────
            kv["SumPrdritS1"] = "Produktritning: " + tmpBet;
            kv["SumPrdritS2"] = kv["SumPrdritS1"];
            kv["SumArbInstGjgS1"] = "Gjutgods: A3.022";
            kv["SumArbInstGjgS2"] = "Gjutgods: A3.022";
            kv["SumKvStPlS1"] = "Kvalitetstyrning: K1.07-17";
            kv["SumKvStPlS2"] = "Kvalitetstyrning: K1.07-17";

            return kv;
        }

        // ── Popup ─────────────────────────────────────────────────────────────────
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
                       " dagar)\n\nPopupruta aktiv till " +
                       validTill.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return "";
        }

        private static void SumFrequencies(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsMachine(maskinVal);
            kv["SumF1_1"] = m ? "1/1" : "";
            kv["SumF1_2"] = m ? "1/1" : "";
            kv["SumF1_3"] = m ? "1/1" : "";
            kv["SumF1_4"] = m ? "1/1" : "";
            kv["SumF1_5"] = m ? "1/1" : "";
            kv["SumF1_6"] = m ? "inst.1bit" : "";
            kv["SumF1_7"] = m ? "inst.1bit" : "";
            kv["SumF1_8"] = m ? "inst.1bit" : "";
            kv["SumF2_1"] = m ? "inst.1bit" : "";
            kv["SumF2_2"] = "";
            kv["SumF2_3"] = "";
            kv["SumF2_4"] = m ? "inst.1bit" : "";
            kv["SumF2_5"] = "";
            kv["SumF2_6"] = m ? "inst.1bit" : "";
            kv["SumF2_7"] = m ? "inst.1bit" : "";
            kv["SumF2_8"] = m ? "inst.1bit" : "";
            kv["SumF2_9"] = m ? "inst.1bit" : "";
            kv["SumF2_10"] = m ? "inst.1bit" : "";
            kv["SumF2_11"] = m ? "inst.1bit" : "";
            kv["SumF2_12"] = m ? "inst.1bit" : "";
            kv["SumF2_13"] = m ? "inst.1bit" : "";
            kv["SumF2_14"] = m ? "inst.1bit" : "";
        }

        private static void SumMeasuringDevices(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsMachine(maskinVal);
            kv["SumD1_1"] = m ? "Skjutmått/Tolk" : "";
            kv["SumD1_2"] = m ? "Skjutmått" : "";
            kv["SumD1_3"] = m ? "Mikrometerstickmått" : "";
            kv["SumD1_4"] = m ? "Skjutmått/Tolk" : "";
            kv["SumD1_5"] = m ? "Skjutmått/passbitar" : "";
            kv["SumD1_6"] = m ? "Gängtolk" : "";
            kv["SumD1_7"] = m ? "Höjdmätningsapparat" : "";
            kv["SumD1_8"] = m ? "Skjutmått" : "";
            kv["SumD2_1"] = m ? "Gängtolk" : "";
            kv["SumD2_2"] = "";
            kv["SumD2_3"] = "";
            kv["SumD2_4"] = m ? "Skjutmått/Okulärt" : "";
            kv["SumD2_5"] = "";
            kv["SumD2_6"] = m ? "Skjutmått/Tolk" : "";
            kv["SumD2_7"] = m ? "Skjutmått" : "";
            kv["SumD2_8"] = m ? "Skjutmått" : "";
            kv["SumD2_9"] = m ? "Skjutmått" : "";
            kv["SumD2_10"] = m ? "Skjutmått" : "";
            kv["SumD2_11"] = m ? "Kännbleck" : "";
            kv["SumD2_12"] = m ? "Skjutmått" : "";
            kv["SumD2_13"] = m ? "Ytjämnhetsmätare" : "";
            kv["SumD2_14"] = m ? "Kännbleck" : "";
        }

        private static void SumAF(Dictionary<string, string> kv, string maskinVal, string sg100)
        {
            bool m = IsMachine(maskinVal);
            kv["SumAF1_1"] = "";
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = m ? "* styrgränser enl. arbetsinstruktion med dokument nr: " + sg100 : "";
            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = "";
            kv["SumAF1_6"] = "";
            kv["SumAF1_7"] = m ? "Ind. från lagerläge" : "";
            kv["SumAF1_8"] = "";
            kv["SumAF2_1"] = ""; kv["SumAF2_2"] = ""; kv["SumAF2_3"] = "";
            kv["SumAF2_4"] = m ? "Genomgående hål" : "";
            kv["SumAF2_5"] = ""; kv["SumAF2_6"] = ""; kv["SumAF2_7"] = "";
            kv["SumAF2_8"] = ""; kv["SumAF2_9"] = ""; kv["SumAF2_10"] = "";
            kv["SumAF2_11"] = m ? "Infettas, Mått: 0.05 ihopsatt hus." : "";
            kv["SumAF2_12"] = "";
            kv["SumAF2_13"] = "";
            kv["SumAF2_14"] = m ? "kontroll mot planskiva, mått: 0.1" : "";
        }

        // ── Tolerance tables ─────────────────────────────────────────────────────

        private static double G7Pos(double v)
        {
            if (v < 3.01) return 0.012; if (v < 6.01) return 0.016; if (v < 10.01) return 0.020;
            if (v < 18.01) return 0.024; if (v < 30.01) return 0.028; if (v < 50.01) return 0.034;
            if (v < 80.01) return 0.040; if (v < 120.01) return 0.047; if (v < 180.01) return 0.054;
            if (v < 250.01) return 0.061; if (v < 315.01) return 0.069; if (v < 400.01) return 0.075;
            if (v < 500.01) return 0.083; if (v < 630.01) return 0.092; if (v < 800.01) return 0.104;
            if (v < 1000.01) return 0.116; if (v < 1250.01) return 0.133; if (v < 1600.01) return 0.155;
            if (v < 2000.01) return 0.182; if (v < 2500.01) return 0.209;
            return 0.248;
        }

        private static double G7Neg(double v)
        {
            if (v < 3.01) return 0.002; if (v < 6.01) return 0.004; if (v < 10.01) return 0.005;
            if (v < 18.01) return 0.006; if (v < 30.01) return 0.007; if (v < 50.01) return 0.009;
            if (v < 80.01) return 0.010; if (v < 120.01) return 0.012; if (v < 180.01) return 0.014;
            if (v < 250.01) return 0.015; if (v < 315.01) return 0.017; if (v < 400.01) return 0.018;
            if (v < 500.01) return 0.020; if (v < 630.01) return 0.022; if (v < 800.01) return 0.024;
            if (v < 1000.01) return 0.026; if (v < 1250.01) return 0.028; if (v < 1600.01) return 0.030;
            if (v < 2000.01) return 0.032; if (v < 2500.01) return 0.034;
            return 0.038;
        }

        private static double H12Tol(double v)
        {
            if (v < 3.01) return 0.100; if (v < 6.01) return 0.120; if (v < 10.01) return 0.150;
            if (v < 18.01) return 0.180; if (v < 30.01) return 0.210; if (v < 50.01) return 0.250;
            if (v < 80.01) return 0.300; if (v < 120.01) return 0.350; if (v < 180.01) return 0.400;
            if (v < 250.01) return 0.460; if (v < 315.01) return 0.520; if (v < 400.01) return 0.570;
            if (v < 500.01) return 0.630; if (v < 630.01) return 0.700; if (v < 800.01) return 0.800;
            if (v < 1000.01) return 0.900; if (v < 1250.01) return 1.050; if (v < 1600.01) return 1.250;
            if (v < 2000.01) return 1.500; if (v < 2500.01) return 1.750;
            return 2.100;
        }

        private static double JS13Tol(double v)
        {
            if (v < 3.01) return 0.070; if (v < 6.01) return 0.090; if (v < 10.01) return 0.110;
            if (v < 18.01) return 0.135; if (v < 30.01) return 0.165; if (v < 50.01) return 0.195;
            if (v < 80.01) return 0.230; if (v < 120.01) return 0.270; if (v < 180.01) return 0.315;
            if (v < 250.01) return 0.360; if (v < 315.01) return 0.405; if (v < 400.01) return 0.445;
            if (v < 500.01) return 0.485; if (v < 630.01) return 0.550; if (v < 800.01) return 0.625;
            if (v < 1000.01) return 0.700; if (v < 1250.01) return 0.825; if (v < 1600.01) return 0.975;
            if (v < 2000.01) return 1.150; if (v < 2500.01) return 1.400;
            return 1.650;
        }

        private static double JS11Tol(double v)
        {
            if (v < 80.01) return 0.095; if (v < 120.01) return 0.110; if (v < 180.01) return 0.125;
            if (v < 250.01) return 0.145; if (v < 315.01) return 0.160; if (v < 400.01) return 0.180;
            if (v < 500.01) return 0.200;
            return 0.220;
        }

        private static double JS10Tol(double v)
        {
            if (v < 3.01) return 0.020; if (v < 6.01) return 0.024; if (v < 10.01) return 0.029;
            if (v < 18.01) return 0.035; if (v < 30.01) return 0.042; if (v < 50.01) return 0.050;
            if (v < 80.01) return 0.060; if (v < 120.01) return 0.070; if (v < 180.01) return 0.080;
            if (v < 250.01) return 0.092; if (v < 315.01) return 0.105; if (v < 400.01) return 0.115;
            if (v < 500.01) return 0.125; if (v < 630.01) return 0.140; if (v < 800.01) return 0.160;
            if (v < 1000.01) return 0.180; if (v < 1250.01) return 0.210; if (v < 1600.01) return 0.250;
            if (v < 2000.01) return 0.300; if (v < 2500.01) return 0.350;
            return 0.430;
        }

        private static double H15Tol(double v)
        {
            if (v < 10.01) return 0.580; if (v < 18.01) return 0.700; if (v < 30.01) return 0.840;
            return 1.000;
        }

        // ── Utilities ─────────────────────────────────────────────────────────────
        private static double GetVal(double[] list, int oneBasedIdx)
        {
            if (oneBasedIdx < 1 || oneBasedIdx > list.Length) return 0;
            return list[oneBasedIdx - 1];
        }

        private static int GetMember(string val, string[] list)
        {
            for (int i = 0; i < list.Length; i++)
                if (string.Equals(list[i], val, StringComparison.OrdinalIgnoreCase)) return i + 1;
            return 0;
        }

        private static bool IsMachine(string mv)
        {
            if (string.IsNullOrEmpty(mv)) return false;
            for (int i = 0; i < Machines.Length; i++)
                if (string.Equals(Machines[i], mv, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        private static bool EqualsI(string a, string b) => string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);

        private static int TryParseInt(string s)
        {
            int v;
            return int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static double TryParseDouble(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;
            double v;
            return double.TryParse(s.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static string Fmt(double v) => v.ToString(CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt0(double v) => v.ToString("F0", CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt1(double v) => v.ToString("F1", CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt2(double v) => v.ToString("F2", CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt3(double v) => v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
    }
}