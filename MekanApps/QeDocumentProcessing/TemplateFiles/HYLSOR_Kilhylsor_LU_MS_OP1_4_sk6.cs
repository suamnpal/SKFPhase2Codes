using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Kilhylsor_LU_MS_OP1_4_sk6 : ITemplateCalculations
    {
        private static readonly string[] Machines = new[] { "Morando/1150" };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;
            List<Bookmark> bm = req != null ? req.Bookmarks : null;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);


            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace('.', ',');
            bool tmpSlash = tmpBet.IndexOf('/') >= 0;
            bool tmpLU = tmpBet.IndexOf("LU", StringComparison.OrdinalIgnoreCase) >= 0;
            bool tmpMS = tmpBet.IndexOf("MS", StringComparison.OrdinalIgnoreCase) >= 0;
            bool tmpLW = tmpBet.IndexOf("LW", StringComparison.OrdinalIgnoreCase) >= 0;
            bool tmpV21 = tmpBet.IndexOf("V21", StringComparison.OrdinalIgnoreCase) >= 0;
            bool tmpSpecial = tmpBet.Length > 10;

            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : "";
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : "";
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : "";
            string tmpBet4 = tokens.Length > 3 ? tokens[3] : "";
            string tmpBet5 = tokens.Length > 4 ? tokens[4] : "";
            string tmpBet6 = tokens.Length > 5 ? tokens[5] : "";

            int tmpCountB2 = tmpBet2.Length;
            string tmpSerie;
            if (tmpLW) tmpSerie = "0";
            else if (tmpSlash && (tmpCountB2 == 3 || tmpCountB2 == 2)) tmpSerie = tmpBet2;
            else if (tmpCountB2 > 4) tmpSerie = Left(tmpBet2, 3);
            else if (tmpCountB2 == 3) tmpSerie = Left(tmpBet2, 1);
            else tmpSerie = Left(tmpBet2, 2);

            string tmpTyp = !tmpSlash ? Right(tmpBet2, 2) : tmpBet3;
            int tmpTypLista = (EqualsI(tmpBet2, "7437986") || EqualsI(tmpBet2, "7434690")) ? 1 : 0;

            double tmpKona = GetDoubleAny(bm, "Kona");
            if (tmpKona == 0) tmpKona = 12;
            string tmpKona2 = Math.Abs(tmpKona - 12) < 0.0001 ? "2°23'" : "0°57'";
            kv["SumKona"] = tmpKona2;
            kv["SumKona2"] = "Kona 1:" + Fmt(tmpKona);

            double tmpL = GetDoubleAny(bm, "Längd (L)", "L鋘gd (L)");
            double tmpLSSK = tmpL + 5;
            kv["SumL1S2"] = "(L1) " + Fmt(tmpLSSK);
            kv["SumL1S2Tol"] = "- 0.5";
            double tmpL2 = tmpL + 30;
            kv["SumL2S2"] = "(L2) min: " + Fmt(tmpL2);
            kv["SumL"] = "(L) " + Fmt(tmpL);
            double tmpLOP3 = tmpL + 5;
            kv["SumLOP3"] = "(L) " + Fmt(tmpLOP3);
            kv["SumLOP3Tol"] = " ± 0.5";
            kv["SumLTol"] = "+ " + Fmt3(0);
            kv["SumLTolN"] = "- " + Fmt3(LengthTolNegative(tmpL, IsMsSpecialFamily(tmpBet2)));

            double aMatt = GetDoubleAny(bm, "a-mått", "a-m錿t", "a-matt");
            double dRaw = GetDoubleAny(bm, "Ytterdiameter lillkona (d)");
            double tmpd = dRaw;
            if (tmpd != 0 && aMatt != 0) tmpd = (aMatt / tmpKona) + dRaw;
            double tmpdSSK = tmpd + 3;
            double tmpdS2 = Math.Round(tmpdSSK, 2);
            kv["SumdS2"] = "(d) " + Fmt(tmpdS2).Replace(".", ",");
            kv["SumdS2Tol"] = "- 0.5";
            double tmpda = Math.Round(tmpd, 2);
            kv["Sumd"] = "(d) " + Fmt(tmpda).Replace(".",",");
            kv["SumdTol"] = "± "+ TolPlain(tmpd);

            double tmpd2 = Math.Round((tmpLOP3 / tmpKona) + tmpd, 2);
            double tmpd2SSK = Math.Round(tmpd2 + 9, 0);
            kv["Sumd2S1"] = "(d2) " + Fmt(tmpd2SSK).Replace(".", ",");
            kv["Sumd2S1Tol"] = "- 0.5";
            double tmpd2SSKS2 = Math.Round((tmpLSSK / tmpKona) + tmpdSSK, 1);
            kv["Sumd2S2"] = "(d2) " + Fmt(tmpd2SSKS2).Replace(".", ",");
            kv["Sumd2S2Tol"] = "- 0.5";
            kv["Sumd2"] = "(d2) " + Fmt(tmpd2).Replace(".", ",");
            kv["Sumd2Tol"] = "± " + TolPlain(tmpd2);
            kv["SumF"] = "80";


            kv["SumL1a"] = "10";
            kv["SumL2a"] = "8";
            kv["SumL3a"] = "8";
            kv["SumRa25a"] = "2.5";
            
            double tmpd1 = GetDoubleAny(bm, "Innerdiameter (d1)");
            double tmpd1SSK = tmpd1 - 2.5;
            kv["Sumd1S1"] = "(d1) " + Fmt(tmpd1SSK).Replace(".", ",");
            kv["Sumd1S1Tol"] = "- 0.5";
            kv["Sumd1"] = "(d1) " + Fmt(tmpd1).Replace(".", ",");
            kv["Sumd1Tol"] = "± " + Fmt3(D1Tol(tmpd1, IsMsSpecialFamily(tmpBet2)));


            double tmpd3 = GetDoubleAny(bm, "Fasdiameter storkona (d3)");
            kv["Sumd3"] = tmpd3 == 0 ? "Ingen fas" : "(d3) " + Fmt(tmpd3).Replace(".", ",");
            kv["Sumd3Tol"] = tmpd3 == 0 ? "" : "± " + D3Tol(tmpd3);

            kv["SumRa25"] = "2.5";
            kv["SumRa25A"] = "2.5";
            kv["SumRa5"] = "5";

            string radieLill = GetStringAny(bm, "Inre Radie lillkona");
            string radieStor = GetStringAny(bm, "Inre Radie storkona");
            kv["SumR"] = StringZeroOrEmpty(radieLill) ? "R2" : radieLill;
            kv["SumR3"] = StringZeroOrEmpty(radieStor) ? "Ingen radie" : radieStor;
            kv["SumRadie"] = StringZeroOrEmpty(radieStor) ? "Ingen radie" : "Med radie";
            kv["SumV"] = tmpd3 == 0 ? "" : "45° ";
            double tmpVm = (tmpd3 - tmpd1) / 2.0;
            kv["SumVm"] = StringZeroOrEmpty(radieStor) ? "(V) " + Fmt(tmpVm) + " x 45° " : "Ingen fas";


            double tmpRHA = StraightnessA(tmpd) / 1000.0;
            kv["SumRHA"] = "max: " + Fmt(tmpRHA).Replace(".",",") + " [2F]";
            double tmpRHB = StraightnessB(tmpd) / 1000.0;
            kv["SumRHB"] = "max: " + Fmt(tmpRHB).Replace(".", ",") + " [2F]"; 
            string tmpRd = Fmt3(RoundnessD1(tmpd1));
            kv["SumRd"] = "max: " + tmpRd + " [3F]";

            kv["SumGodstjocklekTol"] = WallThicknessTolPos(tmpKona, tmpd) + " [3F]";
            kv["SumGodstjocklekTolN"] = WallThicknessTolNeg(tmpKona, tmpd) + " [2F]";
            string tmpGV = Fmt3(GV(tmpd));
            kv["SumGV"] = "max: " + tmpGV + " [2F]";

            double tmpML = tmpL - 4;
            double tmpMLa = tmpML < 110 ? 75 : 100;
            kv["SumML"] = "ML=" + Fmt(tmpMLa);
            double tmpVTa = VTBase(tmpd);
            double tmpVT = (tmpVTa / 1000.0) * tmpMLa;
            kv["SumVT"] = "  ± " + Fmt(tmpVT).Replace(".",",") + " [2F]";

            double tmp8 = Math.Round(((tmpd - tmpd1) / 2.0) + (8 / (2.0 * tmpKona)), 3);
            double tmp83 = Math.Round(((tmpd - tmpd1) / 2.0) + (83 / (2.0 * tmpKona)), 3);
            double tmp108 = Math.Round(((tmpd - tmpd1) / 2.0) + (108 / (2.0 * tmpKona)), 3);
            double tmp40 = Math.Round(((tmpd - tmpd1) / 2.0) + (40 / (2.0 * tmpKona)), 3);
            double tmp140 = Math.Round(((tmpd - tmpd1) / 2.0) + (140 / (2.0 * tmpKona)), 3);
            kv["SumL1"] = Fmt(tmpML < 145 ? 8 : 40);
            kv["SumL2"] = Fmt(tmpML < 110 ? 83 : (tmpML < 145 ? 108 : 140));
            kv["SumE1"] = Fmt(tmpML < 145 ? tmp8 : tmp40).Replace(".",",");
            kv["SumE2"] = (Fmt(tmpML < 110 ? tmp83 : (tmpML < 145 ? tmp108 : tmp140))).Replace(".",",");
            kv["SumBygGV"] = tmpML < 145 ? "SR 7419471" : "SR 7415991";
            kv["SumBygVT"] = tmpML < 145 ? "SR 7419471" : "SR 7415991";
            kv["SumBygGT"] = tmpML < 145 ? "SR 7419471" : "SR 7415991";

            SumMaskinVal(kv, maskinVal);

            kv["SumM_d3"] = tmpd3 == 0 ? "" : "Inre fasdiameter";
            kv["SumB_d3"] = tmpd3 == 0 ? "" : "d3";
            kv["SumM_V"] = tmpd3 == 0 ? "Radie" : "Fasvinkel";
            kv["SumB_V"] = tmpd3 == 0 ? "R" : "V";

            SumFrequencies(kv, maskinVal, tmpd3);
            SumMeasuringDevices(kv, maskinVal, tmpd3);
            SumAF(kv, maskinVal, tmpVT);

            kv["SumTextSpårRub"] = "SPÅR FÖR FÄSTJÄRN";
            kv["SumTextSp錼Rub"] = kv["SumTextSpårRub"]; 
            kv["SumTextSpår"] = "Om hylsan är större än 1000 mm utv. ska spår för fästjärn svarvas inv. annars utv.";
            kv["SumTextSp錼"] = kv["SumTextSpår"];
            kv["SumtextPlan"] = "Planet Rensvarvas";
            kv["SumTextS1"] = "Bryt alla kanter, avlägsna";
            kv["SumTextS2"] = "Bryt alla kanter, avlägsna";
            kv["SumTextS3"] = "Bryt alla kanter, avlägsna. Vid misstänkt form & läges fel så lämna hylsa till mätrum.";
            kv["SumTextS4"] = "Bryt alla kanter, avlägsna";


            string rit = GetStringAny(bm, "Ritningsnummer");
            string tmpRitningsnr = EqualsI(rit, "0") || string.IsNullOrWhiteSpace(rit) ? tmpBet : rit;
            kv["SumRitS1"] = tmpRitningsnr;
            kv["SumRitS2"] = tmpRitningsnr;
            kv["SumRitS3"] = tmpRitningsnr;
            kv["SumRitS4"] = tmpRitningsnr;
            kv["SumRitTol"] = "Toleranser: 1432010";
            kv["SumRitYtjämnhet"] = "Yta: 7430184";
            kv["SumRitYtj鋗nhet"] = kv["SumRitYtjämnhet"];
            kv["SumKlEgenskaper"] = "PRODUCTION NUTS & SLEEVES & HOUSINGSALLMÄNKLASSADE EGENSKAPERKlassade egenskaper kilhylsor";

            SumCuttingData(kv, bm);


            kv["VaLLU"] = (tmpLU || tmpMS) ? "" : "Denna mall avser endast LU eller MS Hylsor vid Skepp6 med 4 operationer";

            return kv;
        }

        private static void SumMaskinVal(Dictionary<string, string> kv, string maskinVal)
        {
            bool morando = EqualsI(maskinVal, "Morando/1150");
            string s1 = morando ? "Morando" : "";
            string s3 = morando ? "1150" : "";
            kv["SumMaskinValS1"] = "Maskin: " + s1 + " - OP1";
            kv["SumMaskinValS2"] = "Maskin: " + s1 + " - OP2";
            kv["SumMaskinValS3"] = "Maskin: " + s3 + " - OP3 ";
            kv["SumMaskinValS4"] = "Maskin: " + s1 + " - OP4";
        }

        private static void SumFrequencies(Dictionary<string, string> kv, string maskinVal, double d3)
        {
            bool m = EqualsI(maskinVal, "Morando/1150");
            bool empty = string.IsNullOrEmpty(maskinVal);
            Func<string> v = () => (m || empty) ? "1/1" : "";

            kv["SumF1_1"] = v(); kv["SumF1_2"] = v(); kv["SumF1_3"] = v();
            kv["SumF2_1"] = v(); kv["SumF2_2"] = v(); kv["SumF2_3"] = v(); kv["SumF2_4"] = v(); kv["SumF2_5"] = "";
            kv["SumF3_1"] = v(); kv["SumF3_2"] = v(); kv["SumF3_3"] = v(); kv["SumF3_4"] = v(); kv["SumF3_5"] = v();
            kv["SumF3_6"] = v(); kv["SumF3_7"] = v(); kv["SumF3_8"] = v(); kv["SumF3_9"] = v(); kv["SumF3_0"] = v();
            kv["SumF4_1"] = m ? "1/1" : "";
            kv["SumF4_2"] = m ? (d3 == 0 ? "" : "1/1") : "";
            kv["SumF4_3"] = m ? "1/1" : "";
            kv["SumF4_4"] = "";
        }
        private string FormatToleranceText(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "";

            var text = value.Trim().Replace(',', '.');

            var match = System.Text.RegularExpressions.Regex.Match(text, @"[-+]?\d+(\.\d+)?");
            if (!match.Success)
                return value;

            if (!double.TryParse(match.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var number))
                return value;

            var formatted = number.ToString("0.000", CultureInfo.InvariantCulture);

            if (text.Contains("[2F]"))
                return "+ " + formatted + " [2F]";

            if (text.Contains("[3F]"))
                return "+ " + formatted + " [3F]";

            return "+ " + formatted;
        }
        private static void SumMeasuringDevices(Dictionary<string, string> kv, string maskinVal, double d3)
        {
            bool m = EqualsI(maskinVal, "Morando/1150");
            string sm = "Skjutmått";
            kv["SumD1_1"] = m ? "Inv. mikrometer" : "";
            kv["SumD1_2"] = m ? sm : "";
            kv["SumD1_3"] = "";
            kv["SumD2_1"] = m ? sm : ""; kv["SumD2_2"] = m ? sm : ""; kv["SumD2_3"] = m ? sm : ""; kv["SumD2_4"] = m ? sm : ""; kv["SumD2_5"] = "";
            kv["SumD3_1"] = m ? sm : ""; kv["SumD3_2"] = m ? sm : ""; kv["SumD3_3"] = m ? sm : "";
            kv["SumD3_4"] = m ? "Egglinjal" : ""; kv["SumD3_5"] = m ? "Egglinjal" : "";
            kv["SumD3_6"] = m ? "Mätbygel " + kvVal(kv, "SumBygGV") : "";
            kv["SumD3_7"] = m ? "Mätbygel " + kvVal(kv, "SumBygGT") : "";
            kv["SumD3_8"] = m ? "Mätbygel " + kvVal(kv, "SumBygVT") : "";
            kv["SumD3_9"] = m ? "Mätmaskin" : "";
            kv["SumD3_0"] = m ? "Ytjämnhetsmätare" : "";
            kv["SumD4_1"] = m ? sm : "";
            kv["SumD4_2"] = m ? (d3 == 0 ? "" : sm) : "";
            kv["SumD4_3"] = m ? (d3 == 0 ? "Radielyra" : "Skjutmått / Fasmall") : "";
            kv["SumD4_4"] = "";
        }

        private static void SumAF(Dictionary<string, string> kv, string maskinVal, double tmpVT)
        {
            bool m = EqualsI(maskinVal, "Morando/1150");
            string val = Convert.ToDouble(tmpVT).ToString("0.000", CultureInfo.InvariantCulture);
            kv["SumAF1_1"] = ""; kv["SumAF1_2"] = ""; kv["SumAF1_3"] = "";
            kv["SumAF2_1"] = ""; kv["SumAF2_2"] = ""; kv["SumAF2_3"] = ""; kv["SumAF2_4"] = ""; kv["SumAF2_5"] = "";
            kv["SumAF3_1"] = ""; kv["SumAF3_2"] = "";
            kv["SumAF3_3"] = m ? "Körs färdigt i OP 4" : "";
            kv["SumAF3_4"] = m ? kvVal(kv, "SumRHA") : "";
            kv["SumAF3_5"] = m ? kvVal(kv, "SumRHB") : "";
            kv["SumAF3_6"] = m ? kvVal(kv, "SumGV") : "";
            kv["SumAF3_7"] = "";
            kv["SumAF3_8"] = m ? "  ± " + tmpVT.ToString("0.000", CultureInfo.InvariantCulture).Replace(".", ",") + " [2F]" + " vid mätlängd: " + ExtractAfter(kvVal(kv, "SumML"), "=") + "mm" : "";
            kv["SumAF3_9"] = m ? kvVal(kv, "SumRd") : "";
            kv["SumAF3_0"] = "";
            kv["SumAF4_1"] = ""; kv["SumAF4_2"] = ""; kv["SumAF4_3"] = ""; kv["SumAF4_4"] = "";
        }

        private static void SumCuttingData(Dictionary<string, string> kv, List<Bookmark> bm)
        {
            SetValueAndLabel(kv, bm, "Chuckbackar_OP1", "SumChuckback1", "SumCB1", "Chuckbackar:", "");
            SetValueAndLabel(kv, bm, "Chuckbackar", "SumChuckback", "SumCB", "Chuckbackar:", "");
            SetValueAndLabel(kv, bm, "Stödbackar_OP1", "SumStödback1", "SumSB1", "Stödbackar:", " mm", "St鰀backar_OP1", "SumSt鰀back1");
            SetValueAndLabel(kv, bm, "Stödbackar", "SumStödback", "SumSB", "Stödbackar:", " mm", "St鰀backar", "SumSt鰀back");
            SetValueAndLabel(kv, bm, "Grader_OP1", "SumGrader1", "SumGR1", "Grader:", " mm");
            SetValueAndLabel(kv, bm, "Grader", "SumGrader", "SumGR", "Grader:", " mm");
            SetValueAndLabel(kv, bm, "Varvtal_OP1", "SumVarv1", "SumVR1", "Varvtal:", " /min");
            SetValueAndLabel(kv, bm, "Varvtal", "SumVarv", "SumVR", "Varvtal:", " /min");
            SetValueAndLabel(kv, bm, "Matning Plan_OP1", "SumMatPl1", "SumMP1", "Matning Plan:", " /min");
            SetValueAndLabel(kv, bm, "Matning Plan", "SumMatPl", "SumMP", "Matning Plan:", " /min");
            SetValueAndLabel(kv, bm, "Matning Utv/Inv_OP1", "SumMatInUt1", "SumMIU1", "Matning Utv/Inv:", " /min");
            SetValueAndLabel(kv, bm, "Matning Utv/Inv", "SumMatInUt", "SumMIU", "Matning Utv/Inv:", " /min");
            SetValueAndLabel(kv, bm, "Färdigmått Utv_OP1", "SumFMUtv1", "SumFMU1", "Utvändig diameter:", " mm", "F鋜digm錿t Utv_OP1");
            SetValueAndLabel(kv, bm, "Linjal Utv_OP1", "SumUtvLin1", "SumUL1", "Motsvarar på linjal:", " mm", "Linjal Utv_OP1");
            SetValueAndLabel(kv, bm, "Färdigmått Inv_OP1", "SumFMInv1", "SumFMI1", "Invändig diameter:", " mm", "F鋜digm錿t Inv_OP1");
            SetValueAndLabel(kv, bm, "Linjal Inv_OP1", "SumInvLin1", "SumIL1", "Motsvarar på linjal:", " mm", "Linjal Inv_OP1");
            SetValueAndLabel(kv, bm, "Färdigmått Utv", "SumFMUtv", "SumFMU", "Utvändig diameter:", " mm", "F鋜digm錿t Utv");
            SetValueAndLabel(kv, bm, "Linjal Utv", "SumUtvLin", "SumUL", "Motsvarar på linjal:", " mm");
            SetValueAndLabel(kv, bm, "Färdigmått Inv", "SumFMInv", "SumFMI", "Invändig diameter:", " mm", "F鋜digm錿t Inv");
            SetValueAndLabel(kv, bm, "Linjal Inv", "SumInvLin", "SumIL", "Motsvarar på linjal:", " mm");

            bool inst2 = AnyNonZero(bm, "Chuckbackar", "Stödbackar", "St鰀backar", "Grader", "Varvtal", "Matning Plan", "Matning Utv/Inv");
            bool inst1 = AnyNonZero(bm, "Chuckbackar_OP1", "Stödbackar_OP1", "St鰀backar_OP1", "Grader_OP1", "Varvtal_OP1", "Matning Plan_OP1", "Matning Utv/Inv_OP1");
            bool fm2 = AnyNonZero(bm, "Färdigmått Utv", "F鋜digm錿t Utv", "Linjal Utv", "Färdigmått Inv", "F鋜digm錿t Inv", "Linjal Inv");
            bool fm1 = AnyNonZero(bm, "Färdigmått Utv_OP1", "F鋜digm錿t Utv_OP1", "Linjal Utv_OP1", "Färdigmått Inv_OP1", "F鋜digm錿t Inv_OP1", "Linjal Inv_OP1");
            kv["SumIN"] = inst2 ? "Inställning" : "";
            kv["SumIN1"] = inst1 ? "Inställning" : "";
            kv["SumFM"] = fm2 ? "Färdigmått" : "";
            kv["SumFM1"] = fm1 ? "Färdigmått" : "";
        }

        private static void SetValueAndLabel(Dictionary<string, string> kv, List<Bookmark> bm, string field, string valueKey, string labelKey, string label, string suffix, params string[] aliases)
        {
            var keys = new List<string>(); keys.Add(field); if (aliases != null) keys.AddRange(aliases);
            string raw = GetStringAny(bm, keys.ToArray());
            string value = StringZeroOrEmpty(raw) ? "" : raw + suffix;
            kv[valueKey] = value;
            kv[labelKey] = StringZeroOrEmpty(raw) ? "" : label;
            if (aliases != null)
            {
                for (int i = 0; i < aliases.Length; i++)
                {
                    if (aliases[i].StartsWith("Sum", StringComparison.OrdinalIgnoreCase)) kv[aliases[i]] = value;
                }
            }
        }

        private static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published)) return "";
            DateTime pubDt;
            if (!DateTime.TryParse(published, out pubDt)) return "";
            int dagar = 14;
            DateTime validTill = pubDt.AddDays(dagar);
            if (DateTime.Today <= validTill.Date)
                return "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom " + dagar.ToString(CultureInfo.InvariantCulture) +
                       " dagar)\n\n\n\nPopupruta aktiv till " + validTill.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return "";
        }

        private static bool IsMsSpecialFamily(string bet2) => EqualsI(bet2, "240765") || EqualsI(bet2, "335884");

        private static double LengthTolNegative(double L, bool specialFamily)
        {
            if (specialFamily)
            {
                if (L < 3.01) return 0.400; if (L < 6.01) return 0.480; if (L < 10.01) return 0.580; if (L < 18.01) return 0.700;
                if (L < 30.01) return 0.840; if (L < 50.01) return 1.000; if (L < 80.01) return 1.200; if (L < 120.01) return 1.400;
                if (L < 180.01) return 1.600; if (L < 250.01) return 1.850; if (L < 315.01) return 2.100; if (L < 400.01) return 2.300;
                if (L < 500.01) return 2.500; if (L < 630.01) return 2.800; if (L < 800.01) return 3.200; if (L < 1000.01) return 3.600;
                if (L < 1250.01) return 4.200; if (L < 1600.01) return 5.000; if (L < 2000.01) return 6.000; if (L < 2500.01) return 7.000; return 8.600;
            }
            if (L < 3.01) return 0.140; if (L < 6.01) return 0.180; if (L < 10.01) return 0.220; if (L < 18.01) return 0.270;
            if (L < 30.01) return 0.330; if (L < 50.01) return 0.390; if (L < 80.01) return 0.460; if (L < 120.01) return 0.540;
            if (L < 180.01) return 0.630; if (L < 250.01) return 0.720; if (L < 315.01) return 0.810; if (L < 400.01) return 0.890;
            if (L < 500.01) return 0.970; if (L < 630.01) return 1.100; if (L < 800.01) return 1.250; if (L < 1000.01) return 1.400;
            if (L < 1250.01) return 1.650; if (L < 1600.01) return 1.950; if (L < 2000.01) return 2.300; if (L < 2500.01) return 2.800; return 3.300;
        }

        private static double D1Tol(double d1, bool specialFamily)
        {
            if (specialFamily)
            {
                if (d1 < 3.01) return 0.020; if (d1 < 6.01) return 0.024; if (d1 < 10.01) return 0.029; if (d1 < 18.01) return 0.035;
                if (d1 < 30.01) return 0.042; if (d1 < 50.01) return 0.050; if (d1 < 80.01) return 0.060; if (d1 < 120.01) return 0.070;
                if (d1 < 180.01) return 0.080; if (d1 < 250.01) return 0.092; if (d1 < 315.01) return 0.105; if (d1 < 400.01) return 0.115;
                if (d1 < 500.01) return 0.125; if (d1 < 630.01) return 0.140; if (d1 < 800.01) return 0.160; if (d1 < 1000.01) return 0.180;
                if (d1 < 1250.01) return 0.210; if (d1 < 1600.01) return 0.250; if (d1 < 2000.01) return 0.300; if (d1 < 2500.01) return 0.350; return 0.430;
            }
            if (d1 < 3.01) return 0.012; if (d1 < 6.01) return 0.015; if (d1 < 10.01) return 0.018; if (d1 < 18.01) return 0.021;
            if (d1 < 30.01) return 0.026; if (d1 < 50.01) return 0.031; if (d1 < 80.01) return 0.037; if (d1 < 120.01) return 0.043;
            if (d1 < 180.01) return 0.050; if (d1 < 250.01) return 0.057; if (d1 < 315.01) return 0.065; if (d1 < 400.01) return 0.070;
            if (d1 < 500.01) return 0.077; if (d1 < 630.01) return 0.087; if (d1 < 800.01) return 0.100; if (d1 < 1000.01) return 0.115;
            if (d1 < 1250.01) return 0.130; if (d1 < 1600.01) return 0.155; if (d1 < 2000.01) return 0.185; if (d1 < 2500.01) return 0.220; return 0.270;
        }

        private static string D3Tol(double d3)
        {
            if (d3 < 315.01) return "  0.405"; if (d3 < 400.01) return "  0.445"; if (d3 < 500.01) return "  0.485";
            if (d3 < 630.01) return "  0.550"; if (d3 < 800.01) return "  0.625"; if (d3 < 1000.01) return "  0.700";
            if (d3 < 1250.01) return "  0.825"; if (d3 < 1600.01) return "  0.975"; if (d3 < 2000.01) return "  1.150";
            if (d3 < 2500.01) return "  1.400"; return "  1.650";
        }

        private static string TolPlain(double v)
        {
            if (v < 6.01) return "  0.1"; if (v < 30.01) return "  0.2"; if (v < 120.01) return "  0.3";
            if (v < 400.01) return "  0.5"; if (v < 1000.01) return "  0.8"; if (v < 2000.01) return "  1.2"; return "  2.0";
        }

        private static double StraightnessA(double d) { if (d < 100.1) return 8; if (d < 280.1) return 10; if (d < 480.1) return 12; if (d < 600.1) return 14; if (d < 900.1) return 16; if (d < 1250.1) return 20; if (d < 1600.1) return 25; return 30; }
        private static double StraightnessB(double d) { if (d < 100.1) return 12; if (d < 280.1) return 15; if (d < 480.1) return 18; if (d < 600.1) return 21; if (d < 900.1) return 24; if (d < 1250.1) return 30; if (d < 1600.1) return 37; return 45; }
        private static double RoundnessD1(double d1) { if (d1 < 30.01) return 0.042; if (d1 < 50.01) return 0.050; if (d1 < 80.01) return 0.060; if (d1 < 120.01) return 0.070; if (d1 < 180.01) return 0.080; if (d1 < 250.01) return 0.092; if (d1 < 315.01) return 0.105; if (d1 < 400.01) return 0.115; if (d1 < 500.01) return 0.125; if (d1 < 630.01) return 0.140; if (d1 < 800.01) return 0.160; if (d1 < 1000.01) return 0.180; if (d1 < 1250.01) return 0.210; if (d1 < 1600.01) return 0.250; return 0.300; }

        private static string WallThicknessTolPos(double kona, double d)
        {
            if (Math.Abs(kona - 12) < 0.0001)
            {
                if (d > 1250) return "+ 0.100"; if (d > 1000) return "+ 0.095"; if (d > 800) return "+ 0.085"; if (d > 630) return "+ 0.075"; if (d > 500) return "+ 0.070"; if (d > 400) return "+ 0.065"; if (d > 315) return "+ 0.060"; if (d > 250) return "+ 0.055"; if (d > 180) return "+ 0.050"; if (d > 120) return "+ 0.040"; if (d > 80) return "+ 0.035"; if (d > 50) return "+ 0.030"; if (d > 30) return "+ 0.025"; return "+ 0.020";
            }
            if (Math.Abs(kona - 30) < 0.0001)
            {
                if (d > 1600) return "+ 0.070"; if (d > 1250) return "+ 0.065"; if (d > 1000) return "+ 0.060"; if (d > 800) return "+ 0.055"; if (d > 630) return "+ 0.050"; if (d > 500) return "+ 0.045"; if (d > 400) return "+ 0.040"; if (d > 315) return "+ 0.035"; if (d > 250) return "+ 0.035"; if (d > 180) return "+ 0.030"; if (d > 120) return "+ 0.025"; if (d > 80) return "+ 0.022"; if (d > 50) return "+ 0.019"; if (d > 30) return "+ 0.016"; return "+ 0.013 ";
            }
            return "Fel Kona";
        }

        private static string WallThicknessTolNeg(double kona, double d)
        {
            if (Math.Abs(kona - 12) < 0.0001)
            {
                if (d > 1250) return "- 0.310"; if (d > 1000) return "- 0.280"; if (d > 800) return "- 0.250"; if (d > 630) return "- 0.225"; if (d > 500) return "- 0.200"; if (d > 400) return "- 0.190"; if (d > 315) return "- 0.175"; if (d > 250) return "- 0.160"; if (d > 180) return "- 0.140"; if (d > 120) return "- 0.120"; if (d > 80) return "- 0.105"; if (d > 50) return "- 0.090"; if (d > 30) return "- 0.075"; return "- 0.070";
            }
            if (Math.Abs(kona - 30) < 0.0001)
            {
                if (d > 1250) return "- 0.195"; if (d > 1000) return "- 0.170"; if (d > 800) return "- 0.155"; if (d > 630) return "- 0.140"; if (d > 500) return "- 0.125"; if (d > 400) return "- 0.115"; if (d > 315) return "- 0.105"; if (d > 251) return "- 0.095"; if (d > 180) return "- 0.085"; if (d > 120) return "- 0.075"; if (d > 80) return "- 0.065"; if (d > 50) return "- 0.055"; if (d > 30) return "- 0.046"; return "- 0.039";
            }
            return "Fel Kona";
        }

        private static double GV(double d) { if (d > 1250) return 0.055; if (d > 1000) return 0.050; if (d > 800) return 0.045; if (d > 630) return 0.040; if (d > 500) return 0.035; if (d > 315) return 0.030; if (d > 250) return 0.025; if (d > 180) return 0.020; if (d > 120) return 0.015; if (d > 50) return 0.010; return 0.008; }
        private static double VTBase(double d) { if (d < 50.1) return 0.6; if (d < 80.1) return 0.5; if (d < 120.1) return 0.45; if (d < 150.1) return 0.3; if (d < 180.1) return 0.18; if (d < 400.1) return 0.15; if (d < 500.1) return 0.13; if (d < 630.1) return 0.12; if (d < 800.1) return 0.11; if (d < 1000.1) return 0.10; if (d < 1250.1) return 0.09; if (d < 1600.1) return 0.08; return 0.07; }

        private static bool AnyNonZero(List<Bookmark> bm, params string[] keys)
        {
            for (int i = 0; i < keys.Length; i++)
            {
                string s = GetStringAny(bm, keys[i]);
                if (!StringZeroOrEmpty(s)) return true;
            }
            return false;
        }

        private static string GetStringAny(List<Bookmark> bm, params string[] keys)
        {
            if (bm == null || keys == null) return "";
            for (int k = 0; k < keys.Length; k++)
            {
                string key = keys[k];
                if (string.IsNullOrWhiteSpace(key)) continue;
                for (int i = 0; i < bm.Count; i++)
                {
                    Bookmark b = bm[i];
                    if (b != null && string.Equals(b.BookmarkName ?? "", key, StringComparison.OrdinalIgnoreCase))
                        return b.BookmarkValue ?? "";
                }
            }
            return "";
        }

        
        private static double GetDoubleAny(List<Bookmark> bm, params string[] keys)
        {
            string raw = GetStringAny(bm, keys);
            if (string.IsNullOrWhiteSpace(raw)) return 0;
            double v;
            return double.TryParse(raw.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static string kvVal(Dictionary<string, string> kv, string key) => kv != null && kv.ContainsKey(key) ? kv[key] : "";
        private static string ExtractAfter(string s, string marker) { int p = (s ?? "").IndexOf(marker, StringComparison.Ordinal); return p >= 0 ? s.Substring(p + marker.Length) : s; }
        private static bool StringZeroOrEmpty(string s) => string.IsNullOrWhiteSpace(s) || EqualsI(s.Trim(), "0");
        private static bool EqualsI(string a, string b) => string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);
        private static string Left(string s, int n) => string.IsNullOrEmpty(s) ? "" : (s.Length <= n ? s : s.Substring(0, n));
        private static string Right(string s, int n) => string.IsNullOrEmpty(s) ? "" : (s.Length <= n ? s : s.Substring(s.Length - n));
        private static string Fmt(double v) => v.ToString(CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt3(double v) => v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
    }
}
