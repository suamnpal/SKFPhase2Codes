using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Kilhylsor_38_39_OP1_4_sk6 : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var result = InitializeAll();
            var c = BuildContext(request);
            Merge(result, GetGeometry(c));
            Merge(result, GetMachineAndInspection(request, c));
            Merge(result, GetCuttingData(request));
            Merge(result, GetTexts(c));
            return result;
        }

        private Dictionary<string, string> InitializeAll()
        {
            var keys = new[]
            {
                "SumKona","SumKona2","SumL1S2","SumL1S2Tol","SumL2S2","SumL","SumLOP3","SumLOP3Tol","SumLTol","SumLTolN",
                "SumdS2","SumdS2Tol","Sumd","SumdTol","Sumd2S1","Sumd2S1Tol","Sumd2S2","Sumd2S2Tol","Sumd2","Sumd2Tol","SumF",
                "SumL1a","SumL2a","SumL3a","Sumd1S1","Sumd1S1Tol","Sumd1","Sumd1Tol","Sumd3","Sumd3Tol",
                "SumRa25","SumRa25A","SumRa5","SumR","SumV","SumVm","SumC","SumRHA","SumRHB","SumRd","SumGodstjocklekTol","SumGodstjocklekTolN","SumGV","SumML","SumVT",
                "SumL1","SumL2","SumE1","SumE2","SumBygGV","SumBygVT","SumBygGT",
                "SumMaskinValS1","SumMaskinValS2","SumMaskinValS3","SumMaskinValS4",
                "SumF1_1","SumF1_2","SumF1_3","SumF2_1","SumF2_2","SumF2_3","SumF2_4","SumF2_5","SumF3_1","SumF3_2","SumF3_3","SumF3_4","SumF3_5","SumF3_6","SumF3_7","SumF3_8","SumF3_9","SumF3_0","SumF4_1","SumF4_2","SumF4_3","SumF4_4",
                "SumD1_1","SumD1_2","SumD1_3","SumD2_1","SumD2_2","SumD2_3","SumD2_4","SumD2_5","SumD3_1","SumD3_2","SumD3_3","SumD3_4","SumD3_5","SumD3_6","SumD3_7","SumD3_8","SumD3_9","SumD3_0","SumD4_1","SumD4_2","SumD4_3","SumD4_4",
                "SumAF1_1","SumAF1_2","SumAF1_3","SumAF2_1","SumAF2_2","SumAF2_3","SumAF2_4","SumAF2_5","SumAF3_1","SumAF3_2","SumAF3_3","SumAF3_4","SumAF3_5","SumAF3_6","SumAF3_7","SumAF3_8","SumAF3_9","SumAF3_0","SumAF4_1","SumAF4_2","SumAF4_3","SumAF4_4",
                "SumTextSpårRub","SumTextSpår","SumtextPlan","SumTextS1","SumTextS2","SumTextS3","SumTextS4","SumRitS1","SumRitS2","SumRitS3","SumRitS4","SumRitTol","SumRitYtjämnhet","SumKlEgenskaper","VaLV21",
                "SumChuckback1","SumCB1","SumChuckback2","SumCB2","SumChuckback","SumCB","SumStödback1","SumSB1","SumStödback2","SumSB2","SumStödback","SumSB","SumGrader1","SumGR1","SumGrader2","SumGR2","SumGrader","SumGR","SumVarv1","SumVR1","SumVarv2","SumVR2","SumVarv","SumVR",
                "SumMatPl1","SumMP1","SumMatPl2","SumMP2","SumMatPl","SumMP","SumMatInUt1","SumMIU1","SumMatInUt2","SumMIU2","SumMatInUt","SumMIU",
                "SumFMUtv1","SumFMU1","SumUtvLin1","SumUL1","SumFMUtv2","SumFMU2","SumUtvLin2","SumUL2","SumFMUtv","SumFMU","SumUtvLin","SumUL",
                "SumFMInv1","SumFMI1","SumInvLin1","SumIL1","SumFMInv2","SumFMI2","SumInvLin2","SumIL2","SumFMInv","SumFMI","SumInvLin","SumIL","SumIN","SumIN1","SumIN2","SumFM1","SumFM2","SumFM"
            };
            return keys.Distinct().ToDictionary(k => k, k => "");
        }

        private Context BuildContext(APIRequest request)
        {
            string subject = Normalize(GetSafe(request.ProductDesignation));
            string tmpBet = subject.Replace('.', ',');
            var parts = Regex.Split(tmpBet, "[ /.-]+")
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            string bet1 = GetPart(parts, 0);
            string bet2 = GetPart(parts, 1);
            string bet3 = GetPart(parts, 2);
            string bet4 = GetPart(parts, 3);
            string bet5 = GetPart(parts, 4);

            bool hasSlash = tmpBet.Contains("/");
            bool isSpecial = tmpBet.Length > 10;
            bool hasV21 = tmpBet.Contains("V21");
            int countV21 = new[] { bet1, bet2, bet3, bet4, bet5 }.ToList().FindIndex(x => x == "V21") + 1;

            double serie = ToDouble(Left(bet2, 2));
            string typString = bet2.Length > 3 ? Right(bet2, 2) : hasSlash ? bet3 : bet3;
            double typ = ToDouble(typString);

            double kona = (serie == 39 || serie == 38) ? 12 : 30;
            string konaText = (serie == 39 || serie == 38) ? "2° 23'" : "0 57'";

            double specialLength = GetBookmarkDouble(request, "Speciallängd");
            if (specialLength == 0) specialLength = GetBookmarkDouble(request, "Speciall鋘gd");
            double l = specialLength == 0 ? LookupLength(serie, typ) : specialLength;
            double d = bet2.Length > 3 ? typ / 2 * 10 : ToDouble(bet3);
            double d1;
            if (!isSpecial || countV21 == 3 || countV21 == 4)
            {
                if (typ < 61) d1 = d - 10;
                else if (typ < 501) d1 = d - 15;
                else if (typ < 671) d1 = d - 20;
                else if (typ < 901) d1 = d - 25;
                else d1 = d - 30;
            }
            else
            {
                d1 = bet2.Length > 3 ? ToDouble(bet3) : ToDouble(bet4);
            }

            double d2 = Round((l / kona) + d, 0.01);
            double lSsk = l + 3;
            double dSsk = d + 3;
            double d2SskS2 = Round((lSsk / kona) + dSsk, 0.1);
            double lOp3 = l + 5;
            double d2S3 = Round((lOp3 / kona) + d, 0.01);
            double d3 = hasV21 ? (d == 1180 ? d - 7 : 0) : (typ == 60 || typ > 601 ? d : typ < 501 ? d - 2 : d - 4);
            double tml = l - 4;
            double ml = tml < 80 ? 50 : tml < 110 ? 75 : 100;

            return new Context
            {
                Subject = subject,
                Serie = serie,
                Typ = typ,
                HasV21 = hasV21,
                Kona = kona,
                KonaText = konaText,
                L = l,
                LSsk = lSsk,
                LOp3 = lOp3,
                D = d,
                DSsk = dSsk,
                D1 = d1,
                D1Ssk = d1 - 2.5,
                D2 = d2,
                D2SskS2 = d2SskS2,
                D2S3 = d2S3,
                D3 = d3,
                C = typ < 48 ? 5 : typ < 72 ? 8 : 10,
                Tml = tml,
                ML = ml,
                RitningsNr = serie == 38 ? "238000" : serie == 39 ? "226472" : subject,
                SpecialLength = specialLength
            };
        }

        private Dictionary<string, string> GetGeometry(Context c)
        {
            var d = new Dictionary<string, string>();
            d["SumKona"] = c.KonaText;
            d["SumKona2"] = "Kona 1:" + FormatNumber(c.Kona);
            d["SumL1S2"] = "(L1) " + FormatNumber(c.LSsk);
            d["SumL1S2Tol"] = "- 0.5";
            d["SumL2S2"] = "(L2) min: " + FormatNumber(c.L + 30);
            d["SumL"] = "(L) " + FormatNumber(c.L);
            d["SumLOP3"] = "(L) " + FormatNumber(c.LOp3);
            d["SumLOP3Tol"] = "± 0.5";
            d["SumLTol"] = "+ 0";
            d["SumLTolN"] = GetLTolN(c.L) + " [3F]";
            d["SumdS2"] = "(d) " + FormatNumber(c.DSsk).Replace(".",",");
            d["SumdS2Tol"] = "- 0.5";
            d["Sumd"] = "(d) " + FormatNumber(c.D).Replace(".", ",");
            d["SumdTol"] = GetDTol(c.D);
            d["Sumd2S1"] = "(d2) " + FormatNumber(Round(c.D2 + 8, 1.0));
            d["Sumd2S1Tol"] = "- 0.5";
            d["Sumd2S2"] = "(d2) " + FormatNumber(c.D2SskS2).Replace(".", ",");
            d["Sumd2S2Tol"] = "- 0.5";
            d["Sumd2"] = "(d2) " + FormatNumber(c.D2S3).Replace(".", ",");
            d["Sumd2Tol"] = GetDTol(c.D2);
            d["SumF"] = "80";
            d["SumL1a"] = "10";
            d["SumL2a"] = "8";
            d["SumL3a"] = "8";
            d["Sumd1S1"] = "(d1) " + FormatNumber(c.D1Ssk).Replace(".", ",");
            d["Sumd1S1Tol"] = "- 0.5";
            d["Sumd1"] = "(d1) " + FormatNumber(c.D1).Replace(".", ",");
            d["Sumd1Tol"] = "" + GetD1Tol(c.D1) + " [3F]";
            d["Sumd3"] = "(d3) " + (c.HasV21 && c.D3 == 0 ? "" : FormatNumber(c.D3)).Replace(".", ",");
            d["Sumd3Tol"] = "±" + GetD3Tol(c.D3);
            d["SumRa25"] = "2.5";
            d["SumRa25a"] = "2.5";
            d["SumRa5"] = "5";
            d["SumR"] = c.Serie == 38 || c.Serie == 39 ? "R2" : "";
            d["SumV"] = c.Serie == 38 || c.Serie == 39 ? "45° " : "";
            d["SumVm"] = "(V) " + FormatNumber((c.D3 - c.D1) / 2) + " x 45° ";
            d["SumC"] = "(C) " + FormatNumber(c.C);
            d["SumRHA"] = "max: " + FormatDecimal(GetRha(c.D)) + " [2F]";
            d["SumRHB"] = "max: " + FormatDecimal(GetRhb(c.D)) + " [2F]";
            d["SumRd"] = "max: " + GetRd(c.D1) + " [3F]";
            d["SumGodstjocklekTol"] = GetGodstjocklekTol(c.Kona, c.D) + " [3F]";
            d["SumGodstjocklekTolN"] = GetGodstjocklekTolN(c.Kona, c.D) + " [2F]";
            d["SumGV"] = "max: " + GetGV(c.D) + " [2F]";
            d["SumML"] = "ML=" + FormatNumber(c.ML);
            double vtList = GetVTList(c.D);
            d["SumVT"] = "Konavvikelse:   ± " + (c.ML * vtList / 1000).ToString("0.0000", CultureInfo.InvariantCulture) + " [2F]";
            double tmp8 = Round(((c.D - c.D1) / 2) + (8 / (2 * c.Kona)), 0.001);
            double tmp83 = Round(((c.D - c.D1) / 2) + (83 / (2 * c.Kona)), 0.001);
            double tmp108 = Round(((c.D - c.D1) / 2) + (108 / (2 * c.Kona)), 0.001);
            double tmp40 = Round(((c.D - c.D1) / 2) + (40 / (2 * c.Kona)), 0.001);
            double tmp140 = Round(((c.D - c.D1) / 2) + (140 / (2 * c.Kona)), 0.001);
            d["SumL1"] = FormatNumber(c.Tml < 145 ? 8 : 40);
            d["SumL2"] = FormatNumber(c.Tml < 110 ? 83 : c.Tml < 145 ? 108 : 140);
            d["SumE1"] = (FormatNumber(c.Tml < 145 ? tmp8 : tmp40)).Replace(".",",");
            d["SumE2"] = FormatNumber(c.Tml < 110 ? tmp83 : c.Tml < 145 ? tmp108 : tmp140).Replace(".",",");
            d["SumBygGV"] = c.ML < 145 ? "SR 7419471" : "SR 7415991";
            d["SumBygVT"] = c.ML < 145 ? "SR 7419471" : "SR 7415991";
            d["SumBygGT"] = c.ML < 145 ? "SR 7419471" : "SR 7415991";
            return d;
        }

        private Dictionary<string, string> GetMachineAndInspection(APIRequest request, Context c)
        {
            var d = new Dictionary<string, string>();
            string machine = GetMachineValue(request);
            bool morando = machine == "Morando/1150";
            bool empty = string.IsNullOrEmpty(machine);
            string m12 = morando ? "Morando" : "";
            string m3 = morando ? "1150" : "";
            d["SumMaskinValS1"] = "Maskin: " + m12 + (morando ? "" : machine) + " - OP1";
            d["SumMaskinValS2"] = "Maskin: " + m12 + (morando ? "" : machine) + " - OP2";
            d["SumMaskinValS3"] = "Maskin: " + m3 + (morando ? "" : machine) + " - OP3 ";
            d["SumMaskinValS4"] = "Maskin: " + m12 + (morando ? "" : machine) + " - OP4";
            SetIf(d, morando || empty, "1/1", "SumF1_1", "SumF1_2", "SumF2_1", "SumF2_2", "SumF2_3", "SumF2_4", "SumF3_1", "SumF3_2", "SumF3_3", "SumF3_4", "SumF3_5", "SumF3_6", "SumF3_7", "SumF3_8", "SumF3_9", "SumF3_0", "SumF4_1", "SumF4_2", "SumF4_3");
            if (morando)
            {
                d["SumD1_1"] = "Inv. mikrometer";
                d["SumD1_2"] = "Skjutmått";
                SetIf(d, true, "Skjutmått", "SumD2_1", "SumD2_2", "SumD2_3", "SumD2_4", "SumD3_1", "SumD3_2", "SumD3_3", "SumD4_1", "SumD4_2");
                d["SumD3_4"] = "Egglinjal";
                d["SumD3_5"] = "Egglinjal";
                d["SumD3_6"] = "Mätbygel " + (c.ML < 145 ? "SR 7419471" : "SR 7415991");
                d["SumD3_7"] = "Mätbygel " + (c.ML < 145 ? "SR 7419471" : "SR 7415991");
                d["SumD3_8"] = "Mätbygel " + (c.ML < 145 ? "SR 7419471" : "SR 7415991");
                d["SumD3_9"] = "Mätmaskin";
                d["SumD3_0"] = "Ytjämnhetsmätare";
                d["SumD4_3"] = "Skjutmått / Fasmall";
                d["SumAF3_3"] = "Körs färdigt i OP 4";
                d["SumAF3_4"] = "max: " + FormatDecimal(GetRha(c.D)).Replace(".",",") + " [2F]";
                d["SumAF3_5"] = "max: " + FormatDecimal(GetRhb(c.D)).Replace(".", ",") + " [2F]";
                d["SumAF3_6"] = "max: " + GetGV(c.D) + " [2F]";
                d["SumAF3_8"] = "Konavvikelse:   " + (c.ML * GetVTList(c.D) / 1000).ToString("0.0000", CultureInfo.InvariantCulture).Replace(".",",") + " [2F]";
                d["SumAF3_9"] = "max: " + GetRd(c.D1) + " [3F]";
            }
            return d;
        }

        private Dictionary<string, string> GetTexts(Context c)
        {
            return new Dictionary<string, string>
            {
                {"SumTextSpårRub", "SPÅR FÖR FÄSTJÄRN"},
                {"SumTextSpår", "Om hylsan är större än 1000 mm utv. ska spår för fästjärn svarvas inv. annars utv."},
                {"SumtextPlan", "Planet Rensvarvas"},
                {"SumTextS1", "Bryt alla kanter, avlägsna"},
                {"SumTextS2", "Bryt alla kanter, avlägsna"},
                {"SumTextS3", "Bryt alla kanter, avlägsna. Vid misstänkt form & läges fel så lämna hylsa till mätrum."},
                {"SumTextS4", "Bryt alla kanter, avlägsna"},
                {"SumRitS1", c.RitningsNr},
                {"SumRitS2", c.RitningsNr},
                {"SumRitS3", c.RitningsNr},
                {"SumRitS4", c.RitningsNr},
                {"SumRitTol", "Toleranser: 1432010"},
                {"SumRitYtjämnhet", "Yta: 7430184"},
                {"SumKlEgenskaper", "PRODUCTION NUTS & SLEEVES & HOUSINGS\\ALLMÄNKLASSADE EGENSKAPER\\Klassade egenskaper kilhylsor"},
                {"VaLV21", c.HasV21 && c.SpecialLength == 0 ? "Kontrollera att hylslängden följer standardmått, annars ange speciallängd" : ""}
            };
        }

        private Dictionary<string, string> GetCuttingData(APIRequest request)
        {
            var d = new Dictionary<string, string>();
            AddValue(d, request, "Chuckbackar_OP1", "SumChuckback1", "SumCB1", "Chuckbackar:", "");
            AddValue(d, request, "Chuckbackar2", "SumChuckback2", "SumCB2", "Chuckbackar:", "");
            AddValue(d, request, "Chuckbackar", "SumChuckback", "SumCB", "Chuckbackar:", "");
            AddValue(d, request, "Stödbackar_OP1", "SumStödback1", "SumSB1", "Stödbackar:", " mm");
            AddValue(d, request, "Stödbackar2", "SumStödback2", "SumSB2", "Stödbackar:", " mm");
            AddValue(d, request, "Stödbackar", "SumStödback", "SumSB", "Stödbackar:", " mm");
            AddValue(d, request, "Grader_OP1", "SumGrader1", "SumGR1", "Grader:", " mm");
            AddValue(d, request, "Grader2", "SumGrader2", "SumGR2", "Grader:", " mm");
            AddValue(d, request, "Grader", "SumGrader", "SumGR", "Grader:", " mm");
            AddValue(d, request, "Varvtal_OP1", "SumVarv1", "SumVR1", "Varvtal:", " /min");
            AddValue(d, request, "Varvtal2", "SumVarv2", "SumVR2", "Varvtal:", " /min");
            AddValue(d, request, "Varvtal", "SumVarv", "SumVR", "Varvtal:", " /min");
            AddValue(d, request, "Matning Plan_OP1", "SumMatPl1", "SumMP1", "Matning Plan:", " /min");
            AddValue(d, request, "Matning Plan2", "SumMatPl2", "SumMP2", "Matning Plan:", " /min");
            AddValue(d, request, "Matning Plan", "SumMatPl", "SumMP", "Matning Plan:", " /min");
            AddValue(d, request, "Matning Utv/Inv_OP1", "SumMatInUt1", "SumMIU1", "Matning Utv/Inv:", " /min");
            AddValue(d, request, "Matning Utv/Inv2", "SumMatInUt2", "SumMIU2", "Matning Utv/Inv:", " /min");
            AddValue(d, request, "Matning Utv/Inv", "SumMatInUt", "SumMIU", "Matning Utv/Inv:", " /min");
            AddValue(d, request, "Färdigmått Utv_OP1", "SumFMUtv1", "SumFMU1", "Utvändig diameter:", " -0.5 mm");
            AddValue(d, request, "Linjal Utv_OP1", "SumUtvLin1", "SumUL1", "Motsvarar på linjal:", " mm");
            AddValue(d, request, "Färdigmått Utv2", "SumFMUtv2", "SumFMU2", "Utvändig diameter:", " -0.5 mm");
            AddValue(d, request, "Linjal Utv2", "SumUtvLin2", "SumUL2", "Motsvarar på linjal:", " mm");
            AddValue(d, request, "Färdigmått Utv", "SumFMUtv", "SumFMU", "Utvändig diameter:", " -0.5 mm");
            AddValue(d, request, "Linjal Utv", "SumUtvLin", "SumUL", "Motsvarar på linjal:", " mm");
            AddValue(d, request, "Färdigmått Inv_OP1", "SumFMInv1", "SumFMI1", "Invändig diameter:", " -0.5 mm");
            AddValue(d, request, "Linjal Inv_OP1", "SumInvLin1", "SumIL1", "Motsvarar på linjal:", " mm");
            AddValue(d, request, "Färdigmått Inv2", "SumFMInv2", "SumFMI2", "Invändig diameter:", " -0.5 mm");
            AddValue(d, request, "Linjal Inv2", "SumInvLin2", "SumIL2", "Motsvarar på linjal:", " mm");
            string fmInv = GetBookmark(request, "Färdigmått Inv");
            if (!IsZeroOrEmpty(fmInv))
            {
                d["SumFMInv"] = d.ContainsKey("SumFMInv1") ? d["SumFMInv1"] : "";
                d["SumFMI"] = "Invändig diameter:";
            }
            AddValue(d, request, "Linjal Inv", "SumInvLin", "SumIL", "Motsvarar på linjal:", " mm");
            d["SumIN"] = AnyNonZero(request, "Chuckbackar", "Stödbackar", "Grader", "Varvtal", "Matning Plan", "Matning Utv/Inv") ? "Inställning" : "";
            d["SumIN1"] = AnyNonZero(request, "Chuckbackar_OP1", "Stödbackar_OP1", "Grader_OP1", "Varvtal_OP1", "Matning Plan_OP1", "Matning Utv/Inv_OP1") ? "Inställning" : "";
            d["SumIN2"] = AnyNonZero(request, "Chuckbackar2", "Stödbackar2", "Grader2", "Varvtal2", "Matning Plan2", "Matning Utv/Inv2") ? "Inställning" : "";
            d["SumFM"] = AnyNonZero(request, "Färdigmått Utv", "Linjal Utv", "Färdigmått Inv", "Linjal Inv") ? "Färdigmått" : "";
            d["SumFM1"] = AnyNonZero(request, "Färdigmått Utv_OP1", "Linjal Utv_OP1", "Färdigmått Inv_OP1", "Linjal Inv_OP1") ? "Färdigmått" : "";
            d["SumFM2"] = AnyNonZero(request, "Färdigmått Utv2", "Linjal Utv2", "Färdigmått Inv2", "Linjal Inv2") ? "Färdigmått" : "";
            return d;
        }

        private void AddValue(Dictionary<string, string> d, APIRequest request, string bookmark, string valueKey, string labelKey, string label, string suffix)
        {
            string value = GetBookmark(request, bookmark);
            if (IsZeroOrEmpty(value)) return;
            d[valueKey] = value + suffix;
            d[labelKey] = label;
        }

        private bool AnyNonZero(APIRequest request, params string[] names)
        {
            foreach (var name in names)
                if (!IsZeroOrEmpty(GetBookmark(request, name)))
                    return true;
            return false;
        }

        private bool IsZeroOrEmpty(string value)
        {
            return string.IsNullOrWhiteSpace(value) || value.Trim() == "0";
        }

        private double LookupLength(double serie, double typ)
        {
            double[] typList = { 44, 48, 52, 56, 60, 64, 68, 72, 76, 80, 84, 88, 92, 96, 500, 530, 560, 600, 630, 670, 710, 750, 800, 850, 900, 950, 1000, 1060, 1120, 1180, 1250, 1320, 1400 };
            double[] l39 = { 71, 71, 85, 85, 103, 103, 103, 103, 118, 118, 118, 132, 132, 140, 140, 150, 155, 165, 180, 185, 200, 206, 218, 224, 236, 250, 265, 280, 280, 300, 315 };
            double[] l38 = { 0, 0, 55, 62, 72, 72, 72, 72, 87, 87, 87, 87, 102, 102, 102, 106, 106, 115, 130, 130, 140, 150, 160, 160, 165, 175, 195, 195, 208, 208, 215, 236, 254 };
            int index = Array.IndexOf(typList, typ);
            if (index < 0) return 0;
            if (serie == 39 && index < l39.Length) return l39[index];
            if (serie == 38 && index < l38.Length) return l38[index];
            return 0;
        }

        private string GetLTolN(double v)
        {
            if (v < 10.01) return "- 0.580";
            if (v < 18.01) return "- 0.700";
            if (v < 30.01) return "- 0.840";
            if (v < 50.01) return "- 1.000";
            if (v < 80.01) return "- 1.200";
            if (v < 120.01) return "- 1.400";
            if (v < 180.01) return "- 1.600";
            if (v < 250.01) return "- 1.850";
            if (v < 315.01) return "- 2.100";
            if (v < 400.01) return "- 2.300";
            if (v < 500.01) return "- 2.500";
            if (v < 630.01) return "- 2.800";
            if (v < 800.01) return "- 3.200";
            if (v < 1000.01) return "- 3.600";
            if (v < 1250.01) return "- 4.200";
            if (v < 1600.01) return "- 5.000";
            return "- 6.000";
        }

        private string GetDTol(double v)
        {
            if (v < 6.01) return "± 0.1";
            if (v < 30.01) return "± 0.2";
            if (v < 120.01) return "± 0.3";
            if (v < 400.01) return "± 0.5";
            if (v < 1000.01) return "± 0.8";
            if (v < 2000.01) return "± 1.2";
            return "± 2.0";
        }

        private string GetD1Tol(double v)
        {
            if (v < 3.01) return "± 0.020";
            if (v < 6.01) return "± 0.024";
            if (v < 10.01) return "± 0.029";
            if (v < 18.01) return "± 0.035";
            if (v < 30.01) return "± 0.042";
            if (v < 50.01) return "± 0.050";
            if (v < 80.01) return "± 0.060";
            if (v < 120.01) return "± 0.070";
            if (v < 180.01) return "± 0.080";
            if (v < 250.01) return "± 0.092";
            if (v < 315.01) return "± 0.105";
            if (v < 400.01) return "± 0.115";
            if (v < 500.01) return "± 0.125";
            if (v < 630.01) return "± 0.140";
            if (v < 800.01) return "± 0.160";
            if (v < 1000.01) return "± 0.180";
            if (v < 1250.01) return "± 0.210";
            if (v < 1600.01) return "± 0.250";
            if (v < 2000.01) return "± 0.300";
            if (v < 2500.01) return "± 0.350";
            return "± 0.430";
        }

        private string GetD3Tol(double v)
        {
            if (v < 315.01) return "  0.405";
            if (v < 400.01) return "  0.445";
            if (v < 500.01) return "  0.485";
            if (v < 630.01) return "  0.550";
            if (v < 800.01) return "  0.625";
            if (v < 1000.01) return "  0.700";
            if (v < 1250.01) return "  0.825";
            if (v < 1600.01) return "  0.975";
            if (v < 2000.01) return "  1.150";
            if (v < 2500.01) return "  1.400";
            return "  1.650";
        }

        private double GetRha(double v)
        {
            if (v < 100.1) return 0.008;
            if (v < 280.1) return 0.010;
            if (v < 480.1) return 0.012;
            if (v < 600.1) return 0.014;
            if (v < 900.1) return 0.016;
            if (v < 1250.1) return 0.020;
            if (v < 1600.1) return 0.025;
            return 0.030;
        }

        private double GetRhb(double v)
        {
            if (v < 100.1) return 0.012;
            if (v < 280.1) return 0.015;
            if (v < 480.1) return 0.018;
            if (v < 600.1) return 0.021;
            if (v < 900.1) return 0.024;
            if (v < 1250.1) return 0.030;
            if (v < 1600.1) return 0.037;
            return 0.045;
        }

        private string GetRd(double v)
        {
            if (v < 30.01) return "0.042";
            if (v < 50.01) return "0.050";
            if (v < 80.01) return "0.060";
            if (v < 120.01) return "0.070";
            if (v < 180.01) return "0.080";
            if (v < 250.01) return "0.092";
            if (v < 315.01) return "0.105";
            if (v < 400.01) return "0.115";
            if (v < 500.01) return "0.125";
            if (v < 630.01) return "0.140";
            if (v < 800.01) return "0.160";
            if (v < 1000.01) return "0.180";
            if (v < 1250.01) return "0.210";
            if (v < 1600.01) return "0.250";
            return "0.300";
        }

        private string GetGodstjocklekTol(double kona, double v)
        {
            if (kona == 12)
            {
                if (v > 1250) return "+ 0.100";
                if (v > 1000) return "+ 0.095";
                if (v > 800) return "+ 0.085";
                if (v > 630) return "+ 0.075";
                if (v > 500) return "+ 0.070";
                if (v > 400) return "+ 0.065";
                if (v > 315) return "+ 0.060";
                if (v > 250) return "+ 0.055";
                if (v > 180) return "+ 0.050";
                if (v > 120) return "+ 0.040";
                if (v > 80) return "+ 0.035";
                if (v > 50) return "+ 0.030";
                if (v > 30) return "+ 0.025";
                return "+ 0.020";
            }
            if (kona == 30)
            {
                if (v > 1600) return "+ 0.070";
                if (v > 1250) return "+ 0.065";
                if (v > 1000) return "+ 0.060";
                if (v > 800) return "+ 0.055";
                if (v > 630) return "+ 0.050";
                if (v > 500) return "+ 0.045";
                if (v > 400) return "+ 0.040";
                if (v > 315) return "+ 0.035";
                if (v > 250) return "+ 0.035";
                if (v > 180) return "+ 0.030";
                if (v > 120) return "+ 0.025";
                if (v > 80) return "+ 0.022";
                if (v > 50) return "+ 0.019";
                if (v > 30) return "+ 0.016";
                return "+ 0.013 ";
            }
            return "Fel Kona";
        }

        private string GetGodstjocklekTolN(double kona, double v)
        {
            if (kona == 12)
            {
                if (v > 1250) return "- 0.310";
                if (v > 1000) return "- 0.280";
                if (v > 800) return "- 0.250";
                if (v > 630) return "- 0.225";
                if (v > 500) return "- 0.200";
                if (v > 400) return "- 0.190";
                if (v > 315) return "- 0.175";
                if (v > 250) return "- 0.160";
                if (v > 180) return "- 0.140";
                if (v > 120) return "- 0.120";
                if (v > 80) return "- 0.105";
                if (v > 50) return "- 0.090";
                if (v > 30) return "- 0.075";
                return "- 0.070";
            }
            if (kona == 30)
            {
                if (v > 1250) return "- 0.195";
                if (v > 1000) return "- 0.170";
                if (v > 800) return "- 0.155";
                if (v > 630) return "- 0.140";
                if (v > 500) return "- 0.125";
                if (v > 400) return "- 0.115";
                if (v > 315) return "- 0.105";
                if (v > 251) return "- 0.095";
                if (v > 180) return "- 0.085";
                if (v > 120) return "- 0.075";
                if (v > 80) return "- 0.065";
                if (v > 50) return "- 0.055";
                if (v > 30) return "- 0.046";
                return "- 0.039";
            }
            return "Fel Kona";
        }

        private string GetGV(double v)
        {
            if (v > 1250) return "0.055";
            if (v > 1000) return "0.050";
            if (v > 800) return "0.045";
            if (v > 630) return "0.040";
            if (v > 500) return "0.035";
            if (v > 315) return "0.030";
            if (v > 250) return "0.025";
            if (v > 180) return "0.020";
            if (v > 120) return "0.015";
            if (v > 50) return "0.010";
            return "0.008";
        }

        private double GetVTList(double v)
        {
            if (v < 50.1) return 0.6;
            if (v < 80.1) return 0.5;
            if (v < 120.1) return 0.45;
            if (v < 150.1) return 0.3;
            if (v < 180.1) return 0.18;
            if (v < 400.1) return 0.15;
            if (v < 500.1) return 0.13;
            if (v < 630.1) return 0.12;
            if (v < 800.1) return 0.11;
            if (v < 1000.1) return 0.10;
            if (v < 1250.1) return 0.09;
            if (v < 1600.1) return 0.08;
            return 0.07;
        }

        private string GetMachineValue(APIRequest request)
        {
            string machine = GetSafe(request.MachineNumber);
            if (!string.IsNullOrWhiteSpace(machine)) return machine;
            return GetBookmark(request, "MaskinVal");
        }

        private string GetBookmark(APIRequest request, string name)
        {
            if (request.Bookmarks == null) return "";
            foreach (var b in request.Bookmarks)
                if (b.BookmarkName == name)
                    return b.BookmarkValue ?? "";
            return "";
        }

        private double GetBookmarkDouble(APIRequest request, string name, double fallback = 0)
        {
            var val = GetBookmark(request, name);
            double d;
            if (double.TryParse(val, NumberStyles.Any, CultureInfo.InvariantCulture, out d)) return d;
            if (double.TryParse(val.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out d)) return d;
            return fallback;
        }

        private double ToDouble(string value)
        {
            double d;
            if (double.TryParse((value ?? "").Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out d)) return d;
            return 0;
        }

        private double Round(double value, double step)
        {
            if (step == 0) return value;
            return Math.Round(value / step, MidpointRounding.AwayFromZero) * step;
        }

        private string FormatNumber(double v)
        {
            if (Math.Abs(v - Math.Round(v)) < 0.0000001)
                return Math.Round(v).ToString("0", CultureInfo.InvariantCulture);
            return v.ToString("0.###", CultureInfo.InvariantCulture);
        }

        private string FormatDecimal(double v)
        {
            return v.ToString("0.###", CultureInfo.InvariantCulture);
        }

        private string Normalize(string input)
        {
            return input == null ? "" : input.Trim().ToUpperInvariant();
        }

        private string GetPart(List<string> parts, int index)
        {
            return index >= 0 && index < parts.Count ? parts[index] : "";
        }

        private string Left(string value, int length)
        {
            value = value ?? "";
            return value.Length <= length ? value : value.Substring(0, length);
        }

        private string Right(string value, int length)
        {
            value = value ?? "";
            return value.Length <= length ? value : value.Substring(value.Length - length);
        }

        private string GetSafe(string v)
        {
            return v ?? "";
        }

        private void SetIf(Dictionary<string, string> d, bool condition, string value, params string[] keys)
        {
            if (!condition) return;
            foreach (var key in keys) d[key] = value;
        }

        private void Merge(Dictionary<string, string> target, Dictionary<string, string> source)
        {
            foreach (var kv in source)
                target[kv.Key] = kv.Value ?? "";
        }

        private class Context
        {
            public string Subject { get; set; }
            public double Serie { get; set; }
            public double Typ { get; set; }
            public bool HasV21 { get; set; }
            public double Kona { get; set; }
            public string KonaText { get; set; }
            public double L { get; set; }
            public double LSsk { get; set; }
            public double LOp3 { get; set; }
            public double D { get; set; }
            public double DSsk { get; set; }
            public double D1 { get; set; }
            public double D1Ssk { get; set; }
            public double D2 { get; set; }
            public double D2SskS2 { get; set; }
            public double D2S3 { get; set; }
            public double D3 { get; set; }
            public double C { get; set; }
            public double Tml { get; set; }
            public double ML { get; set; }
            public string RitningsNr { get; set; }
            public double SpecialLength { get; set; }
        }
    }
}
