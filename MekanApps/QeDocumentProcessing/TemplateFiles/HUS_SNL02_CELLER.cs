using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HUS_SNL02_CELLER : ITemplateCalculations
    {
        private static readonly string[] TypList = { "13", "15", "16", "17", "18", "19", "20", "22", "24", "26", "28", "30", "32" };

        // AdLista: serie "2" has 12 elements (length mismatch in original Lotus — preserved as-is)
        private static readonly double[] AdListaSerie2 = { 92.5, 102.5, 108, 112, 120, 0, 0, 147.5, 0, 177.5, 0, 0 };
        private static readonly double[] AdListaSerie5 = { 77, 87, 92.5, 97.5, 102.5, 131, 137.5, 147.5, 157.5, 0, 177.5, 0, 0 };

        private static readonly double[] LdLista = { 120, 130, 140, 150, 160, 170, 180, 200, 215, 0, 250, 0, 0 };
        private static readonly double[] LbLista = { 51, 56, 58, 61, 65, 68, 70, 80, 86, 0, 98, 0, 0 };
        private static readonly double[] FbLista = { 20, 24, 28, 29.5, 31, 35, 42, 44, 49, 58, 0, 0, 0 };
        private static readonly double[] HdLista = { 142, 153, 165, 174, 185, 195, 211, 230, 245, 0, 290, 0, 0 };
        private static readonly double[] BdLista = { 13.5, 13.5, 13.5, 13.5, 17.5, 17.5, 22, 22, 22, 0, 26.5, 0, 0 };
        private static readonly double[] UhLista = { 80, 80, 95, 95, 100, 112, 112, 125, 140, 0, 150, 0, 0 };
        private static readonly double[] FhLista = { 29.6, 29.6, 31.6, 31.5, 34.5, 34.5, 39.5, 44.3, 44.5, 0, 49.3, 0, 0 };
        private static readonly double[] VhLista = { 40.4, 42.9, 47.4, 54.5, 59.6, 60.1, 63.1, 68.6, 74.6, 0, 86.6, 0, 0 };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");
            bool tmpSlash = tmpBet.IndexOf('/') >= 0;
            bool tmpSNL = tmpBet.IndexOf("SNL", StringComparison.OrdinalIgnoreCase) >= 0;

            // Explode " /.-"
            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : "";
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : "";

            int cntB2 = tmpBet2.Length;

            // TmpSerie = if cnt>3: Left(2) else Left(1)
            string tmpSerie = cntB2 > 3
                ? (tmpBet2.Length >= 2 ? tmpBet2.Substring(0, 2) : tmpBet2)
                : (tmpBet2.Length >= 1 ? tmpBet2.Substring(0, 1) : tmpBet2);

            // TmpTyp = Right(2)
            string tmpTyp = tmpBet2.Length >= 2 ? tmpBet2.Substring(tmpBet2.Length - 2) : tmpBet2;
            int typInt = TryParseInt(tmpTyp);

            // TmpTypLista (1-based)
            int typLista = GetMember(tmpTyp, TypList);

            kv["VaLTypLista"] = typLista == 0
                ? "Produktbeteckningen ingår ej i mallen, kontrollera stavning och/eller kontakta Qe-Admin för att lägga till produkten"
                : "";

            // ── Ra, Rp, Wt ───────────────────────────────────────────────────────
            kv["SumRa32"] = "3.2";
            kv["SumRa32a"] = "3.2";
            kv["SumRa63"] = "6.3 [3]";
            kv["SumRp8"] = "Rp 8";
            kv["SumWt35"] = "Wt 35";
            kv["SumWt20"] = "Wt 20";

            // ── Ad (axelhålsdiameter) — H12 ───────────────────────────────────────
            // Serie "2" has 12-element table (Lotus bug preserved)
            double[] adTab = EqualsI(tmpSerie, "2") ? AdListaSerie2 : AdListaSerie5;
            double tmpAd = GetTabVal(adTab, typLista);
            kv["SumAd"] = "(Ad) " + FmtG(tmpAd);
            kv["SumAdTol"] = "+ " + Fmt3(AdTol(tmpAd)) + " [3]";
            kv["SumAdTolN"] = "- " + Fmt0(0) + " [3]";

            // ── Pd (tätningsspårsdiameter) — H12 ─────────────────────────────────
            double tmpPd = typInt > 18 ? tmpAd + 10.0 : tmpAd + 8.5;
            kv["SumPd"] = "(Pd) " + FmtG(tmpPd);
            kv["SumPdTol"] = "+ " + Fmt3(PdTol(tmpPd)) + " [3]";
            kv["SumPdTolN"] = "- " + Fmt0(0) + " [3]";

            // ── Ld (lagerlägesdiameter) — G7 ─────────────────────────────────────
            double tmpLd = GetTabVal(LdLista, typLista);
            kv["SumLd"] = "(Ld) " + FmtG(tmpLd);
            kv["SumLdTol"] = "+ " + Fmt3(LdTolPos(tmpLd)) + " [3]";
            kv["SumLdTolN"] = "+ " + Fmt3(LdTolNeg(tmpLd)) + " [2D]";

            // ── Lb (lagerbredd) — H12 ────────────────────────────────────────────
            double tmpLb = GetTabVal(LbLista, typLista);
            kv["SumLb"] = "(Lb) " + FmtG(tmpLb);
            kv["SumLbTol"] = "+ " + Fmt3(LbTol(tmpLb)) + " [3]";
            kv["SumLbTolN"] = "- " + Fmt0(0) + " [2]";

            // ── C (symmetri) ──────────────────────────────────────────────────────
            kv["SumC"] = typInt < 18 ? "0,7" : "1";

            // ── Pb (tätningsspår bredd) — H13 ────────────────────────────────────
            double tmpPb = typInt > 18 ? 6 : 5;
            kv["SumPb"] = "2x (Pb) " + FmtG(tmpPb);
            kv["SumPbTol"] = "+ " + Fmt3(H13Tol(tmpPb)) + " [3]";
            kv["SumPbTolN"] = "- " + Fmt0(0) + " [3]";

            // ── G1 (gänga hopslagningsbult) ───────────────────────────────────────
            int tmpG1 = typInt < 18 ? 12 : typInt < 20 ? 16 : typInt < 28 ? 20 : 24;
            kv["SumG1"] = "(G1) M" + tmpG1 + "-6H [3]";

            // ── G (nippelgänga) & Gd (gängdjup) ──────────────────────────────────
            int tmpGd = typInt > 18 ? 10 : 7;
            kv["SumGd"] = "min " + tmpGd;
            kv["SumG"] = "(G) 1/8 - 27 NPSF";

            // ── Fb (borrdjup fettbula) ────────────────────────────────────────────
            double tmpFb = GetTabVal(FbLista, typLista);
            string tmpFbStr = tmpFb.ToString(CommonFunctions.Culture).Replace(",", ".");
            bool fbIsSido = EqualsI(tmpFbStr, "1");
            kv["SumFb"] = fbIsSido ? "Sidoborrhål" : "(Fb) " + FmtG(tmpFb);
            kv["SumFbTol"] = fbIsSido ? "" : "± 0.5";

            // ── Hd (avstånd hopslagningshål) ─────────────────────────────────────
            double tmpHd = GetTabVal(HdLista, typLista);
            kv["SumHd"] = "(Hd) " + FmtG(tmpHd);
            kv["SumHdTol"] = "± " + Fmt3(GenTolVal(tmpHd));

            // ── Bd (diameter hopslagningshål) — H15 ──────────────────────────────
            double tmpBd = GetTabVal(BdLista, typLista);
            kv["SumBd"] = "(Bd) " + FmtG(tmpBd);
            kv["SumBdTol"] = "+ " + Fmt3(H15Tol(tmpBd)) + " [3]";
            kv["SumBdTolN"] = "- " + Fmt0(0) + " [3]";

            // ── Sd (stifthålsdiameter) — H9 ──────────────────────────────────────
            double tmpSd = typInt < 18 ? 5 : 9.335;
            kv["SumSd"] = "2x (Sd) " + FmtG(tmpSd);
            kv["SumSdTol"] = "+ " + Fmt3(tmpSd < 9 ? 0.030 : 0.036) + " [3]";
            kv["SumSdTolN"] = "- " + Fmt0(0) + " [3]";

            // ── Sö (borrdjup stifthål övre halva) ────────────────────────────────
            double tmpSö = typInt < 18 ? 7.7 : 10;
            kv["SumSö"] = "(Sö) " + FmtG(tmpSö);
            kv["SumSöTol"] = "+ " + Fmt0(0) + " [3]";
            kv["SumSöTolN"] = "- 0.5 [3]";

            // ── Su (borrdjup stifthål undre halva) ───────────────────────────────
            double tmpSu = typInt < 18 ? 7.7 : 10.5;
            kv["SumSu"] = "(Su) " + FmtG(tmpSu);
            kv["SumSuTol"] = "+ " + Fmt0(0) + " [3]";
            kv["SumSuTolN"] = "- 0.5 [3]";

            // ── Uh (höjd underhalva) — boxat mått ────────────────────────────────
            double tmpUh = GetTabVal(UhLista, typLista);
            kv["SumUh"] = "(Uh) " + FmtG(tmpUh);
            kv["SumUhTol"] = "± " + Fmt3(typInt < 16 ? 0.23 : typInt < 22 ? 0.27 : 0.315) + " [3]";

            // ── Fh (fothöjd) — boxat mått ────────────────────────────────────────
            double tmpFh = GetTabVal(FhLista, typLista);
            kv["SumFh"] = "(Fh) " + FmtG(tmpFh);
            kv["SumFhTol"] = "± " + Fmt3(typInt < 16 ? 0.4 : typInt < 22 ? 0.5 : 0.7) + " [3]";

            // ── Vh (vårthöjd) — boxat mått ───────────────────────────────────────
            double tmpVh = GetTabVal(VhLista, typLista);
            kv["SumVh"] = "(Vh) " + FmtG(tmpVh);
            kv["SumVhTol"] = "± " + Fmt3(typInt < 16 ? 0.4 : typInt < 18 ? 0.5 : 0.6) + " [3]";

            // ── Ss (släppningsspår) ───────────────────────────────────────────────
            bool ssSmall = typInt < 18;
            string ssVal = ssSmall ? "0.3" : "1.25";
            string ssTolPos = ssSmall ? Fmt0(0) : "0.3";
            string ssTolNeg = ssSmall ? "0.1" : "0.4";
            string ssSuffix = ssSmall ? " [2]" : "";
            kv["SumSs"] = "(Ss) " + ssVal;
            kv["SumSsTol"] = "+ " + ssTolPos + ssSuffix;
            kv["SumSsTolN"] = "- " + ssTolNeg + ssSuffix;

            // ── Pl, CZ, Fp ───────────────────────────────────────────────────────
            kv["SumPl"] = "0.05";
            kv["SumCZ"] = (typInt < 18 ? "0.09" : "0.12") + " CZ";
            kv["SumFp"] = typInt < 16 ? "0.08" : "0.1";

            // ── Pos (position märkning) ───────────────────────────────────────────
            int tmpPos = typInt > 26 ? 50 : typInt > 24 ? 44 : typInt > 20 ? 40 : typInt > 18 ? 23 : typInt > 17 ? 24 : typInt > 11 ? 15 : 12;
            kv["SumPos"] = "(Pos) " + tmpPos;

            // ── Skruv ─────────────────────────────────────────────────────────────
            kv["SumSkr"] = "SNL 8.8, SSNLD 10.9";

            // ── Machine ───────────────────────────────────────────────────────────
            string mvStr = EqualsI(maskinVal, "Celler") ? "Celler" : "";
            string mvErr = EqualsI(maskinVal, "Celler") ? "" : "INGET MASKINVAL GJORD";
            kv["SumMaskinValS1"] = "Maskin: " + mvStr + mvErr + " - Svarvning";
            kv["SumMaskinValS2"] = "Maskin: " + mvStr + mvErr + " - Borrning, fräsning";

            bool m = EqualsI(maskinVal, "Celler");
            SumFrequencies(kv, m, fbIsSido);
            SumDevices(kv, m, fbIsSido);
            SumAF(kv, m, tmpGd, fbIsSido);

            // ── Texts ─────────────────────────────────────────────────────────────
            kv["SumTextS1"] = "Kontrolleras enl. styrplan";
            kv["SumTextS11"] = "Kontrollera ytor på 10 hus i rad per skift";
            kv["SumTextS2"] = "Kontrolleras enl. styrplan<<LineBreak>>Kontrollera ytor på 10 hus i rad per skift<<LineBreak>>För tillägg V/VU se intruktion under fliken Specialhusgruppen";

            // ── Ritning ───────────────────────────────────────────────────────────
            // if serie="2": "Produktritning: [TmpART] 5[TmpTyp] .02 & 7433803"
            // else: "Produktritning: [TmpBet] .02"
            string sumPrdrit = EqualsI(tmpSerie, "2")
                ? "Produktritning: " + tmpBet1 + " 5" + tmpTyp + " .02 & 7433803"
                : "Produktritning: " + tmpBet + " .02";
            kv["SumPrdritS1"] = sumPrdrit;
            kv["SumPrdritS2"] = sumPrdrit;
            kv["SumArbInstGjgS1"] = "Gjutgods: A3.022";
            kv["SumArbInstGjgS2"] = "Gjutgods: A3.022";
            kv["SumKvStPlS1"] = "Kvalitetstyrning: K1.07-17";
            kv["SumKvStPlS2"] = "Kvalitetstyrning: K1.07-17";

            // Fixed AF labels with tab suffix
            kv["SumAF1_7tb"] = " Enligt TB";
            kv["SumAF2_13tb"] = " Enligt TB";

            return kv;
        }

        // ── Popup ──────────────────────────────────────────────────────────────────
        private static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published)) return "";
            DateTime pubDt;
            if (!DateTime.TryParse(published, out pubDt)) return "";
            DateTime validTill = pubDt.AddDays(14);
            if (DateTime.Today <= validTill.Date)
                return "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)\n\nInformation om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument\n\nPopupruta aktiv till " +
                       validTill.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return "";
        }

        // ── Freq / Devices / AF ───────────────────────────────────────────────────
        private static void SumFrequencies(Dictionary<string, string> kv, bool m, bool fbIsSido)
        {
            kv["SumF1_1"] = m ? "1/tim" : "";
            kv["SumF1_2"] = m ? "1/tim" : "";
            kv["SumF1_3"] = m ? "2/skift" : "";
            kv["SumF1_4"] = m ? "1/skift & inst." : "";
            kv["SumF1_5"] = m ? "1/skift & inst." : "";
            kv["SumF1_6"] = m ? "Inst." : "";
            kv["SumF1_7"] = m ? "2/skift & inst." : "";

            kv["SumF2_1"] = m ? "2/skift & inst." : "";
            kv["SumF2_2"] = m ? "2/skift & inst." : "";
            kv["SumF2_3"] = m ? (fbIsSido ? "" : "Inst./borrbyte") : "";
            kv["SumF2_4"] = m ? "Inst." : "";
            kv["SumF2_5"] = m ? "Inst." : "";
            kv["SumF2_6"] = m ? "Inst./borrbyte" : "";
            kv["SumF2_7"] = m ? "Inst." : "";
            kv["SumF2_8"] = m ? "Inst." : "";
            kv["SumF2_9"] = m ? "Inst." : "";
            kv["SumF2_10"] = m ? "Inst." : "";
            kv["SumF2_11"] = m ? "Inst." : "";
            kv["SumF2_12"] = m ? "Inst." : "";
            kv["SumF2_13"] = m ? "Inst." : "";
            kv["SumF2_14"] = m ? "2/skift & inst." : "";
            kv["SumF2_15"] = m ? "2/skift & inst." : "";
        }

        private static void SumDevices(Dictionary<string, string> kv, bool m, bool fbIsSido)
        {
            kv["SumD1_1"] = m ? "Skjutmått/Tolk" : "";
            kv["SumD1_2"] = m ? "Skjutmått" : "";
            kv["SumD1_3"] = m ? "Mätmaskin" : "";
            kv["SumD1_4"] = m ? "Skjutmått/Tolk" : "";
            kv["SumD1_5"] = m ? "Skjutmått/Tolk" : "";
            kv["SumD1_6"] = m ? "Skjutmått" : "";
            kv["SumD1_7"] = m ? "Skjutmått" : "";

            kv["SumD2_1"] = m ? "Gängtolk" : "";
            kv["SumD2_2"] = m ? "Gängtolk/skjutmått" : "";
            kv["SumD2_3"] = m ? (fbIsSido ? "" : "Skjutmått/Okulärt") : "";
            kv["SumD2_4"] = m ? "Skjutmått/Okulärt" : "";
            kv["SumD2_5"] = m ? "Skjutmått" : "";
            kv["SumD2_6"] = m ? "Skjutmått/Tolk" : "";
            kv["SumD2_7"] = m ? "Skjutmått" : "";
            kv["SumD2_8"] = m ? "Skjutmått" : "";
            kv["SumD2_9"] = m ? "Skjutmått" : "";
            kv["SumD2_10"] = m ? "Skjutmått" : "";
            kv["SumD2_11"] = m ? "Kännbleck" : "";
            kv["SumD2_12"] = m ? "Skjutmått" : "";
            kv["SumD2_13"] = m ? "Skjutmått" : "";
            kv["SumD2_14"] = m ? "Mätmaskin" : "";
            kv["SumD2_15"] = m ? "Skjutmått/Mätmaskin" : "";
        }

        private static void SumAF(Dictionary<string, string> kv, bool m, int gd, bool fbIsSido)
        {
            kv["SumAF1_1"] = "";
            kv["SumAF1_2"] = m ? "(Ud) endast serie 31" : "";
            kv["SumAF1_3"] = m ? "10 hus irad" : "";
            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = "";
            kv["SumAF1_6"] = "";
            kv["SumAF1_7"] = m ? "Mätes på uh vid hopl.yta " : "";

            kv["SumAF2_1"] = m ? "Samtliga TP1-maskin 1,2" : "";
            kv["SumAF2_2"] = m ? "Min " + gd + "mm, Maskin 1,2" : "";
            kv["SumAF2_3"] = m ? (fbIsSido ? "Sidoborrhål" : "") : "";
            kv["SumAF2_4"] = "";
            kv["SumAF2_5"] = "";
            kv["SumAF2_6"] = "";
            kv["SumAF2_7"] = "";
            kv["SumAF2_8"] = "";
            kv["SumAF2_9"] = "";
            kv["SumAF2_10"] = "";
            kv["SumAF2_11"] = m ? "Mått: 0.05 ihopsatt hus. " : "";
            kv["SumAF2_12"] = "";
            kv["SumAF2_13"] = m ? "Centrum till undersida text" : "";
            kv["SumAF2_14"] = m ? "Från TP1-maskin 1, 2" : "";
            kv["SumAF2_15"] = m ? "Från TP1-maskin 1, 2" : "";
        }

        // ── Tolerance tables ──────────────────────────────────────────────────────

        // Ad H12 (4-step, stops at 520)
        private static double AdTol(double v)
        {
            if (v < 80.01) return 0.300;
            if (v < 120.01) return 0.350;
            if (v < 180.01) return 0.400;
            if (v < 250.01) return 0.460;
            return 0.520;
        }

        // Pd H12 (4-step)
        private static double PdTol(double v)
        {
            if (v < 120.01) return 0.350;
            if (v < 180.01) return 0.400;
            if (v < 250.01) return 0.460;
            return 0.520;
        }

        // Ld G7 positive (5-step)
        private static double LdTolPos(double v)
        {
            if (v < 120.01) return 0.047;
            if (v < 180.01) return 0.054;
            if (v < 250.01) return 0.061;
            if (v < 315.01) return 0.069;
            return 0.075;
        }

        // Ld G7 negative (5-step) — NOTE: both Ld tols are "+", as per Lotus script
        private static double LdTolNeg(double v)
        {
            if (v < 120.01) return 0.012;
            if (v < 180.01) return 0.014;
            if (v < 250.01) return 0.015;
            if (v < 315.01) return 0.017;
            return 0.018;
        }

        // Lb H12 (4-step)
        private static double LbTol(double v)
        {
            if (v < 50.01) return 0.250;
            if (v < 80.01) return 0.300;
            if (v < 120.01) return 0.350;
            return 0.400;
        }

        // Pb H13 (21-step)
        private static double H13Tol(double v)
        {
            if (v < 3.01) return 0.140; if (v < 6.01) return 0.180; if (v < 10.01) return 0.220;
            if (v < 18.01) return 0.270; if (v < 30.01) return 0.330; if (v < 50.01) return 0.390;
            if (v < 80.01) return 0.460; if (v < 120.01) return 0.540; if (v < 180.01) return 0.630;
            if (v < 250.01) return 0.720; if (v < 315.01) return 0.810; if (v < 400.01) return 0.890;
            if (v < 500.01) return 0.970; if (v < 630.01) return 1.100; if (v < 800.01) return 1.250;
            if (v < 1000.01) return 1.400; if (v < 1250.01) return 1.650; if (v < 1600.01) return 1.950;
            if (v < 2000.01) return 2.300; if (v < 2500.01) return 2.800; return 3.300;
        }

        // Bd H15 (4-step)
        private static double H15Tol(double v)
        {
            if (v < 10.01) return 0.580;
            if (v < 19.01) return 0.700;
            if (v < 30.01) return 0.840;
            return 1.000;
        }

        // General tolerance (for Hd)
        private static double GenTolVal(double v)
        {
            if (v < 6.01) return 0.1; if (v < 30.01) return 0.2; if (v < 120.01) return 0.3;
            if (v < 400.01) return 0.5; if (v < 1000.01) return 0.8; if (v < 2000.01) return 1.2; return 2.0;
        }

        // ── Utilities ─────────────────────────────────────────────────────────────
        private static double GetTabVal(double[] tab, int oneBasedIdx)
        {
            if (tab == null || oneBasedIdx < 1 || oneBasedIdx > tab.Length) return 0;
            return tab[oneBasedIdx - 1];
        }

        private static int GetMember(string val, string[] list)
        {
            if (list == null) return 0;
            for (int i = 0; i < list.Length; i++)
                if (string.Equals(list[i], val, StringComparison.OrdinalIgnoreCase)) return i + 1;
            return 0;
        }

        private static bool EqualsI(string a, string b) =>
            string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);

        private static int TryParseInt(string s)
        {
            int v;
            return int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static string FmtG(double v) => v.ToString(CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt0(double v) => ((int)v).ToString(CultureInfo.InvariantCulture);
        private static string Fmt3(double v) => v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
    }
}