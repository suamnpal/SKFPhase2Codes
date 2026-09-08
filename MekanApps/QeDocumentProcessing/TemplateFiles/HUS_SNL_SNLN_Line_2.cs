using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HUS_SNL_SNLN_Line_2 : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string mv = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");
            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);

            string tmpBet1 = tokens.Length > 0 ? tokens[0] : string.Empty;
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : string.Empty;
            string tmpSerie = tmpBet2.Length > 3 ? Left(tmpBet2, 2) : Left(tmpBet2, 1);
            string tmpTyp = Right(tmpBet2, 2);
            bool tmpSNLN = tmpBet.IndexOf("SNLN", StringComparison.OrdinalIgnoreCase) >= 0;
            int serieInt = TryParseInt(tmpSerie);
            int typInt = TryParseInt(tmpTyp);
            int tmpTypLista = GetTypIndex(tmpSerie, tmpTyp, tmpSNLN);

            kv["VaLTypLista"] = tmpTypLista == 0 ? "Produktbeteckningen ingår ej i mallen, kontrollera stavning och/eller kontakta Qe-Admin för att lägga till produkten" : string.Empty;

            double tmpAd = Pick(GetAdList(tmpSerie, tmpSNLN), tmpTypLista);
            kv["SumAd"] = "(Ad) " + Num(tmpAd);
            kv["SumAdTol"] = "+ " + F3(H12(tmpAd)) + " [3]";
            kv["SumAdTolN"] = "- 0 [3]";

            double tmpPd = (!tmpSNLN && (tmpSerie == "30" || tmpSerie == "31")) ? tmpAd + 24 : (typInt < 19 ? tmpAd + 8.5 : tmpAd + 10);
            kv["SumPd"] = "(Pd) " + Num(tmpPd);
            kv["SumPdTol"] = "+ " + F3(H12(tmpPd)) + " [3]";
            kv["SumPdTolN"] = "- 0 [3]";

            double tmpLd = Pick(GetLdList(tmpSerie, tmpSNLN), tmpTypLista);
            kv["SumLd"] = "(Ld) " + Num(tmpLd);
            kv["SumLdTol"] = "+ " + F3(G7Pos(tmpLd)) + " [3]";
            kv["SumLdTolN"] = "+ " + F3(G7Neg(tmpLd)) + " [2D]";

            double tmpLb = Pick(GetLbList(tmpSerie, tmpSNLN), tmpTypLista);
            kv["SumLb"] = "(Lb) " + Num(tmpLb);
            kv["SumLbTol"] = "+ " + F3(H12(tmpLb)) + " [3]";
            if(tmpBet2 == "520" || tmpBet2 == "3024" || tmpBet2 == "3026" || tmpBet2 == "3028")
                { kv["SumLbTol"] = "+ 0.300 [3]"; }
            kv["SumLbTolN"] = "+ 0 [2]";

            bool tmpVZTyp = !tmpSNLN && (tmpBet2 == "3038" || tmpBet2 == "3136" || tmpBet2 == "3036" || tmpBet2 == "3134");
            double tmpab = tmpVZTyp ? ((tmpBet2 == "3038" || tmpBet2 == "3136") ? 15 : ((tmpBet2 == "3036" || tmpBet2 == "3134") ? 14 : 0)) : 0;
            kv["Sumab"] = tmpab == 0 ? " *" : "(ab) " + Num(tmpab) + "+a";

            double tmpUd = tmpVZTyp ? Pick(tmpSerie == "30" ? new double[] { 196.4, 206.4 } : new double[] { 186.4, 196.4 }, tmpTypLista) : 0;
            kv["SumUd"] = tmpUd == 0 ? "Endast vissa typer *" : "(Ud) " + Num(tmpUd);
            kv["SumUdTol"] = tmpUd == 0 ? string.Empty : "+ " + F3(H12Full(tmpUd)) + " [3]";
            kv["SumUdTolN"] = tmpUd == 0 ? string.Empty : "+ 0 [3]";

            double tmpS = tmpUd == 0 ? 1.5 : 2;
            kv["SumS1"] = tmpUd == 0 ? F1(tmpS).Replace(".",",") : string.Empty;
            kv["SumS2"] = tmpUd == 0 ? string.Empty : F1(tmpS).Replace(".", ",");

            double tmpUb = 11;
            kv["SumUb"] = !tmpVZTyp ? " *" : "(Ub) " + Num(tmpUb);
            kv["SumUbTol"] = !tmpVZTyp ? string.Empty : "+ " + F3(H13(tmpUb)) + " [3]";
            kv["SumUbTolN"] = !tmpVZTyp ? string.Empty : "-  0 [3]";

            double tmpUb1 = 5.5;
            kv["SumUb1"] = !tmpVZTyp ? " *" : "(Ub1) " + Num(tmpUb1);
            kv["SumUb1Tol"] = !tmpVZTyp ? string.Empty : "+ 0 [3]";
            kv["SumUb1TolN"] = !tmpVZTyp ? string.Empty : "-  0.500 [3]";

            double tmpUb2 = 22;
            kv["SumUb2"] = !tmpVZTyp ? " *" : "(Ub2) " + Num(tmpUb2);
            kv["SumUb2Tol"] = !tmpVZTyp ? string.Empty : "+ 0 [3]";
            kv["SumUb2TolN"] = tmpUd == 0 ? string.Empty : "-  " + F3(h15(tmpUb2)) + " [3]";

            double tmpPb = (!tmpSNLN && (tmpSerie == "30" || tmpSerie == "31")) ? 11 : (typInt < 19 ? 5 : 6);
            kv["SumPb"] = tmpVZTyp ? " " : "2x (Pb) " + Num(tmpPb);
            kv["SumPbTol"] = tmpVZTyp ? " " : "+ " + F3(H13Full(tmpPb)) + " [3]";
            kv["SumPbTolN"] = tmpVZTyp ? " " : "-  0 [3]";

            int tmpG1 = (!tmpSNLN && (tmpSerie == "2" || tmpSerie == "5")) ? (typInt < 20 ? 16 : typInt < 26 ? 20 : 24) : (typInt < 20 ? 16 : typInt < 30 ? 20 : 24);
            kv["SumG1"] = "(G1) M" + tmpG1.ToString(CultureInfo.InvariantCulture) + " 6H [3]";

            int tmpGd = (!tmpSNLN && (tmpSerie == "30" || tmpSerie == "31")) ? 12 : 10;
            kv["SumG"] = "(G) 1/8 - 27NPSF";

            double tmpFb = Pick(GetFbList(tmpSerie, tmpSNLN), tmpTypLista);
            kv["SumFb"] = Eq(tmpFb, 1) ? "Sidoborrhål" : "(Fb) " + Num(tmpFb);
            kv["SumFbTol"] = Eq(tmpFb, 1) ? string.Empty : "± 0.500";

            double tmpHd = Pick(GetHdList(tmpSerie, tmpSNLN), tmpTypLista);
            kv["SumHd"] = "(Hd) " + Num(tmpHd);
            kv["SumHdTol"] = "± " + F3(JS13(tmpHd)) + " [3]";

            double tmpBd = Pick(GetBdList(tmpSerie, tmpSNLN), tmpTypLista);
            kv["SumBd"] = "(Bd) " + Num(tmpBd);
            kv["SumBdTol"] = "+ " + F3(H15(tmpBd)) + " [3]";
            kv["SumBdTolN"] = "-  0 [3]";

            double tmpF = tmpBd + 1;
            kv["SumF"] = "(F) " + Num(tmpF);
            kv["SumFTol"] = "+ " + F3(!tmpSNLN && (tmpSerie == "30" || tmpSerie == "31") ? 0.84 : 0.5);
            kv["SumFTolN"] = "-  " + F3(!tmpSNLN && (tmpSerie == "30" || tmpSerie == "31") ? 0 : 0.5);

            double tmpSd = 9.335;
            kv["SumSd"] = "2x (Sd) " + Num(tmpSd);
            kv["SumSdTol"] = "+ 0.036 [2]";
            kv["SumSdTolN"] = "-  0 [2]";

            double tmpSo = 10;
            kv["SumSö"] = "(Sö) " + Num(tmpSo);
            kv["SumSöTol"] = "± 0.200";

            double tmpSu = 10.5;
            kv["SumSu"] = "(Su) " + Num(tmpSu);
            kv["SumSuTol"] = "+ 0 [3]";
            kv["SumSuTolN"] = "-  0.500 [3]";

            double tmpUh = Pick(GetUhList(tmpSerie, tmpSNLN), tmpTypLista);
            kv["SumUh"] = "(Uh) " + Num(tmpUh);
            kv["SumUhTol"] = "± " + F3(JS11(tmpUh)) + " [3]";

            double tmpFh = Pick(GetFhList(tmpSerie, tmpSNLN), tmpTypLista);
            kv["SumFh"] = "(Fh) " + Num(tmpFh);
            kv["SumFhTol"] = "± 1.0";

            double tmpVh = Pick(GetVhList(tmpSerie, tmpSNLN), tmpTypLista);
            kv["SumVh"] = "(Vh) " + Num(tmpVh);
            kv["SumVhTol"] = "± 1.0";

            double tmpSs = (!tmpSNLN && (tmpSerie == "30" || tmpSerie == "31")) ? 0.5 : 1.25;
            kv["SumSs"] = "(Ss) " + tmpSs.ToString("0.000", CultureInfo.InvariantCulture);
            kv["SumSsTol"] = "+ 0.300";
            kv["SumSsTolN"] = "-  " + ((!tmpSNLN && (tmpBet2 == "3036" || tmpBet2 == "3134" || tmpBet2 == "3038" || tmpBet2 == "3136")) ? "0" : "0.400") + ((!tmpSNLN && (tmpSerie == "30" || tmpSerie == "31")) ? string.Empty : " [2]");

            double tmpPl = (!tmpSNLN && (tmpSerie == "30" || tmpSerie == "31")) ? 0.05 : 0.1;
            kv["SumPl"] = F2(tmpPl);
            kv["SumSkr"] = "8.8 SNL. SSNLD 10.9";

            bool isLine2 = EqualsI(mv, "Line 2");
            string tmpMaskinVal = isLine2 ? string.Empty : "INGET MASKINVAL GJORD";
            string tmpMaskinValS1 = isLine2 ? "Line 2" : string.Empty;
            string tmpMaskinValS2 = isLine2 ? "Line 2" : string.Empty;
            kv["SumMaskinValS1"] = "Maskin: " + tmpMaskinValS1 + tmpMaskinVal + " - Svarvning)";
            kv["SumMaskinValS2"] = "Maskin: " + tmpMaskinValS2 + tmpMaskinVal + " - Borrning, fräsning";

            SumFrequencies(kv, isLine2, tmpFb);
            SumDevices(kv, isLine2, tmpFb);
            SumAF(kv, isLine2, tmpFb, tmpGd);

            kv["SumRa32"] = "3.2";
            kv["SumRa32a"] = "3.2";
            kv["SumRa63"] = "6.3 [3]";
            kv["SumRp8"] = "Rp 8";
            kv["SumWt35"] = "Wt 35";
            kv["SumWt20"] = "Wt 20";

            kv["SumTextS1"] = "Kontrolleras enl. styrplan";
            kv["SumTextS2"] = kv["SumTextS1"];
            kv["SumPrdritS1"] = "Produktritning: " + tmpBet;
            kv["SumPrdritS2"] = kv["SumPrdritS1"];
            kv["SumArbInstGjgS1"] = "Gjutgods: A3.022";
            kv["SumArbInstGjgS2"] = kv["SumArbInstGjgS1"];
            kv["SumKvStPlS1"] = "Kvalitetstyrning: K1.07-17";
            kv["SumKvStPlS2"] = kv["SumKvStPlS1"];

            return kv;
        }

        private static void SumFrequencies(Dictionary<string, string> kv, bool m, double fb)
        {
            kv["SumF1_1"] = m ? "1/tim" : string.Empty;
            kv["SumF1_2"] = m ? "1/tim" : string.Empty;
            kv["SumF1_3"] = m ? "1/1" : string.Empty;
            kv["SumF1_4"] = m ? "1/Skift & inst." : string.Empty;
            kv["SumF1_5"] = m ? "1/Skift & inst." : string.Empty;
            kv["SumF1_6"] = m ? "Inst." : string.Empty;
            kv["SumF1_7"] = m ? "2/Skift & inst." : string.Empty;
            kv["SumF2_1"] = m ? "2/Skift & inst." : string.Empty;
            kv["SumF2_2"] = m ? "2/Skift & inst." : string.Empty;
            kv["SumF2_3"] = m ? (Eq(fb, 1) ? string.Empty : "Inst./borrbyte") : string.Empty;
            kv["SumF2_4"] = m ? "Inst." : string.Empty;
            kv["SumF2_5"] = m ? "Inst." : string.Empty;
            kv["SumF2_6"] = m ? "Inst./borrbyte" : string.Empty;
            kv["SumF2_7"] = m ? "Inst." : string.Empty;
            kv["SumF2_8"] = m ? "Inst." : string.Empty;
            kv["SumF2_9"] = m ? "Inst." : string.Empty;
            kv["SumF2_10"] = m ? "Inst." : string.Empty;
            kv["SumF2_11"] = m ? "Inst." : string.Empty;
            kv["SumF2_12"] = m ? "Inst." : string.Empty;
            kv["SumF2_13"] = m ? "2/Skift & inst." : string.Empty;
        }

        private static void SumDevices(Dictionary<string, string> kv, bool m, double fb)
        {
            kv["SumD1_1"] = m ? "Skjutmått/Tolk" : string.Empty;
            kv["SumD1_2"] = m ? "Skjutmått" : string.Empty;
            kv["SumD1_3"] = m ? "Subito" : string.Empty;
            kv["SumD1_4"] = m ? "Skjutmått/Tolk" : string.Empty;
            kv["SumD1_5"] = m ? "Skjutmått/Tolk" : string.Empty;
            kv["SumD1_6"] = m ? "Skjutmått" : string.Empty;
            kv["SumD1_7"] = m ? "Skjutmått" : string.Empty;
            kv["SumD2_1"] = m ? "Gängtolk" : string.Empty;
            kv["SumD2_2"] = m ? "Gängtolk/skjutmått" : string.Empty;
            kv["SumD2_3"] = m ? (Eq(fb, 1) ? string.Empty : "Skjutmått/Okulärt") : string.Empty;
            kv["SumD2_4"] = m ? "Skjutmått/Okulärt" : string.Empty;
            kv["SumD2_5"] = m ? "Skjutmått" : string.Empty;
            kv["SumD2_6"] = m ? "Skjutmått/Tolk" : string.Empty;
            kv["SumD2_7"] = m ? "Skjutmått" : string.Empty;
            kv["SumD2_8"] = m ? "Skjutmått" : string.Empty;
            kv["SumD2_9"] = m ? "Skjutmått" : string.Empty;
            kv["SumD2_10"] = m ? "Skjutmått" : string.Empty;
            kv["SumD2_11"] = m ? "Kännbleck" : string.Empty;
            kv["SumD2_12"] = m ? "Skjutmått" : string.Empty;
            kv["SumD2_13"] = m ? "Mätmaskin" : string.Empty;
        }

        private static void SumAF(Dictionary<string, string> kv, bool m, double fb, int gd)
        {
            kv["SumAF1_1"] = m ? string.Empty : string.Empty;
            kv["SumAF1_2"] = m ? "(Ud) endast serie 31" : string.Empty;
            kv["SumAF1_3"] = m ? string.Empty : string.Empty;
            kv["SumAF1_4"] = m ? string.Empty : string.Empty;
            kv["SumAF1_5"] = m ? string.Empty : string.Empty;
            kv["SumAF1_6"] = m ? string.Empty : string.Empty;
            kv["SumAF1_7"] = m ? "Mätes på UH vid hopl.yta" : string.Empty;
            kv["SumAF2_1"] = m ? "Samtliga f.b-maskiner" : string.Empty;
            kv["SumAF2_2"] = m ? "Min " + gd.ToString(CultureInfo.InvariantCulture) + " gängor / " + gd.ToString(CultureInfo.InvariantCulture) + "mm, alla f.b" : string.Empty;
            kv["SumAF2_3"] = m ? (Eq(fb, 1) ? "Sidoborrhål" : string.Empty) : string.Empty;
            kv["SumAF2_4"] = m ? string.Empty : string.Empty;
            kv["SumAF2_5"] = m ? string.Empty : string.Empty;
            kv["SumAF2_6"] = m ? string.Empty : string.Empty;
            kv["SumAF2_7"] = m ? string.Empty : string.Empty;
            kv["SumAF2_8"] = m ? string.Empty : string.Empty;
            kv["SumAF2_9"] = m ? string.Empty : string.Empty;
            kv["SumAF2_10"] = m ? string.Empty : string.Empty;
            kv["SumAF2_11"] = m ? "Mått: 0.05 enligt Tb" : string.Empty;
            kv["SumAF2_12"] = m ? string.Empty : string.Empty;
            kv["SumAF2_13"] = m ? "Från Fb maskin 1, 2, 3" : string.Empty;
        }

        private static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published)) return string.Empty;
            DateTime pubDt;
            if (!DateTime.TryParse(published, out pubDt)) return string.Empty;
            int dagar = 14;
            DateTime validTill = pubDt.AddDays(dagar);
            if (DateTime.Today <= validTill.Date)
                return "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom " + dagar.ToString(CultureInfo.InvariantCulture) + " dagar)<<LineBreak>><<LineBreak>><<LineBreak>><<LineBreak>>Information om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument<<LineBreak>><<LineBreak>>Popupruta aktiv till " + validTill.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return string.Empty;
        }

        private static int GetTypIndex(string serie, string typ, bool snln)
        {
            if (snln) return Member(typ, new string[] { "24", "26", "28", "30", "32", "34", "36", "38" });
            if (serie == "30") return Member(typ, new string[] { "36", "38" });
            if (serie == "31") return Member(typ, new string[] { "34", "36" });
            if (serie == "2") return Member(typ, new string[] { "18" });
            return Member(typ, new string[] { "18", "19", "20", "22", "24", "26", "28", "30", "32" });
        }

        private static int Member(string value, string[] values)
        {
            for (int i = 0; i < values.Length; i++)
                if (string.Equals(value, values[i], StringComparison.OrdinalIgnoreCase)) return i + 1;
            return 0;
        }

        private static double[] GetAdList(string serie, bool snln)
        {
            if (snln) return new double[] { 157.5, 167.5, 177.5, 192.5, 202.5, 212.5, 222.5, 232.5 };
            if (serie == "30") return new double[] { 181.2, 191.4 };
            if (serie == "31") return new double[] { 171.2, 181.2 };
            if (serie == "2") return new double[] { 120 };
            return new double[] { 102.5, 131, 137.5, 147.5, 157.5, 167.5, 177.5, 192.5, 202.5 };
        }

        private static double[] GetLdList(string serie, bool snln)
        {
            if (snln) return new double[] { 180, 200, 210, 225, 240, 260, 280, 290 };
            if (serie == "30") return new double[] { 280, 290 };
            if (serie == "31") return new double[] { 280, 300 };
            return new double[] { 160, 170, 180, 200, 215, 230, 250, 270, 290 };
        }

        private static double[] GetLbList(string serie, bool snln)
        {
            if (snln) return new double[] { 70, 79, 79, 86, 90, 87, 94, 95 };
            if (serie == "30") return new double[] { 108, 115 };
            if (serie == "31") return new double[] { 108, 116 };
            return new double[] { 65, 68, 70, 80, 86, 90, 98, 106, 114 };
        }

        private static double[] GetFbList(string serie, bool snln)
        {
            if (snln) return new double[] { 33, 39, 40, 45, 45, 51, 51, 48 };
            if (serie == "30" || serie == "31") return new double[] { 1, 1 };
            return new double[] { 31, 35, 39, 45, 47, 51, 58, 57, 57 };
        }

        private static double[] GetHdList(string serie, bool snln)
        {
            if (snln) return new double[] { 211, 230, 245, 265, 275, 310, 332, 332 };
            if (serie == "30" || serie == "31") return new double[] { 315, 335 };
            return new double[] { 185, 195, 211, 230, 245, 265, 290, 310, 332 };
        }

        private static double[] GetBdList(string serie, bool snln)
        {
            if (snln) return new double[] { 22, 22, 22, 26, 26, 26, 26, 26 };
            if (serie == "30" || serie == "31") return new double[] { 27, 27 };
            return new double[] { 17.5, 17.5, 22, 22, 22, 26, 26, 26, 26 };
        }

        private static double[] GetUhList(string serie, bool snln)
        {
            if (snln) return new double[] { 112, 125, 140, 150, 150, 160, 170, 170 };
            if (serie == "30" || serie == "31") return new double[] { 170, 180 };
            return new double[] { 100, 112, 112, 125, 140, 150, 150, 160, 170 };
        }

        private static double[] GetFhList(string serie, bool snln)
        {
            if (snln) return new double[] { 40, 45, 45, 50, 50, 60, 60, 60 };
            if (serie == "30" || serie == "31") return new double[] { 70, 75 };
            return new double[] { 35, 35, 40, 45, 45, 50, 50, 60, 60 };
        }

        private static double[] GetVhList(string serie, bool snln)
        {
            if (snln) return new double[] { 61, 64, 68, 76, 83, 86, 87, 92 };
            if (serie == "30" || serie == "31") return new double[] { 92, 92 };
            return new double[] { 56, 56, 61, 64, 68, 76, 81, 86, 89 };
        }

        private static double Pick(double[] values, int index)
        {
            if (values == null || values.Length == 0 || index <= 0 || index > values.Length) return 0;
            return values[index - 1];
        }

        private static double H12(double v)
        {
            if (v < 120.01) return 0.350;
            if (v < 180.01) return 0.400;
            if (v < 250.01) return 0.460;
            return 0.520;
        }

        private static double H12Full(double v)
        {
            if (v < 3.01) return 0.100;
            if (v < 6.01) return 0.120;
            if (v < 10.01) return 0.150;
            if (v < 18.01) return 0.180;
            if (v < 30.01) return 0.210;
            if (v < 50.01) return 0.250;
            if (v < 80.01) return 0.300;
            if (v < 120.01) return 0.350;
            if (v < 180.01) return 0.400;
            if (v < 250.01) return 0.460;
            if (v < 315.01) return 0.520;
            if (v < 400.01) return 0.570;
            if (v < 500.01) return 0.630;
            if (v < 630.01) return 0.700;
            if (v < 800.01) return 0.800;
            if (v < 1000.01) return 0.900;
            if (v < 1250.01) return 1.050;
            if (v < 1600.01) return 1.250;
            if (v < 2000.01) return 1.500;
            if (v < 2500.01) return 1.750;
            return 2.100;
        }

        private static double H13(double v)
        {
            if (v < 3.01) return 0.140;
            if (v < 6.01) return 0.180;
            if (v < 10.01) return 0.220;
            if (v < 18.01) return 0.270;
            return 0.330;
        }

        private static double H13Full(double v)
        {
            if (v < 3.01) return 0.140;
            if (v < 6.01) return 0.180;
            if (v < 10.01) return 0.220;
            if (v < 18.01) return 0.270;
            if (v < 30.01) return 0.330;
            if (v < 50.01) return 0.390;
            if (v < 80.01) return 0.460;
            if (v < 120.01) return 0.540;
            if (v < 180.01) return 0.630;
            if (v < 250.01) return 0.720;
            if (v < 315.01) return 0.810;
            if (v < 400.01) return 0.890;
            if (v < 500.01) return 0.970;
            if (v < 630.01) return 1.100;
            if (v < 800.01) return 1.250;
            if (v < 1000.01) return 1.400;
            if (v < 1250.01) return 1.650;
            if (v < 1600.01) return 1.950;
            if (v < 2000.01) return 2.300;
            if (v < 2500.01) return 2.800;
            return 3.300;
        }

        private static double h15(double v)
        {
            if (v < 3.01) return 0.400;
            if (v < 6.01) return 0.480;
            if (v < 10.01) return 0.580;
            if (v < 18.01) return 0.700;
            if (v < 30.01) return 0.840;
            if (v < 50.01) return 1.000;
            return 1.200;
        }

        private static double H15(double v)
        {
            if (v < 10.01) return 0.580;
            if (v < 18.01) return 0.700;
            if (v < 30.01) return 0.840;
            return 1.000;
        }

        private static double G7Pos(double v)
        {
            if (v < 120.01) return 0.047;
            if (v < 180.01) return 0.054;
            if (v < 250.01) return 0.061;
            if (v < 315.01) return 0.069;
            return 0.075;
        }

        private static double G7Neg(double v)
        {
            if (v < 120.01) return 0.012;
            if (v < 180.01) return 0.014;
            if (v < 250.01) return 0.015;
            if (v < 315.01) return 0.017;
            return 0.018;
        }

        private static double JS13(double v)
        {
            if (v < 3.01) return 0.070;
            if (v < 6.01) return 0.090;
            if (v < 10.01) return 0.110;
            if (v < 18.01) return 0.135;
            if (v < 30.01) return 0.165;
            if (v < 50.01) return 0.195;
            if (v < 80.01) return 0.230;
            if (v < 120.01) return 0.270;
            if (v < 180.01) return 0.315;
            if (v < 250.01) return 0.360;
            if (v < 315.01) return 0.405;
            if (v < 400.01) return 0.445;
            if (v < 500.01) return 0.485;
            if (v < 630.01) return 0.550;
            if (v < 800.01) return 0.625;
            if (v < 1000.01) return 0.700;
            if (v < 1250.01) return 0.825;
            if (v < 1600.01) return 0.975;
            if (v < 2000.01) return 1.150;
            if (v < 2500.01) return 1.400;
            return 1.650;
        }

        private static double JS11(double v)
        {
            if (v < 80.01) return 0.095;
            if (v < 120.01) return 0.110;
            if (v < 180.01) return 0.125;
            if (v < 250.01) return 0.145;
            return 0.160;
        }

        private static bool EqualsI(string a, string b)
        {
            return string.Equals(a ?? string.Empty, b ?? string.Empty, StringComparison.OrdinalIgnoreCase);
        }

        private static int TryParseInt(string s)
        {
            int v;
            return int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static string Left(string s, int len)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            if (len <= 0) return string.Empty;
            return s.Length <= len ? s : s.Substring(0, len);
        }

        private static string Right(string s, int len)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            if (len <= 0) return string.Empty;
            return s.Length <= len ? s : s.Substring(s.Length - len);
        }
        
        private static bool Eq(double a, double b)
        {
            return Math.Abs(a - b) < 0.000001;
        }

        private static string Num(double v)
        {
            return v.ToString("0.###", CultureInfo.InvariantCulture);
        }

        private static string F1(double v)
        {
            return v.ToString("0.0", CultureInfo.InvariantCulture);
        }

        private static string F2(double v)
        {
            return v.ToString("0.00", CultureInfo.InvariantCulture);
        }

        private static string F3(double v)
        {
            return v.ToString("0.000", CultureInfo.InvariantCulture);
        }
    }
}
