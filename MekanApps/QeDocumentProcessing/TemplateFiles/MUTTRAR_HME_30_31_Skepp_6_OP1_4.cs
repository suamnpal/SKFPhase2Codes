
using DocumentFormat.OpenXml.Office2016.Excel;
using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class MUTTRAR_HME_30_31_Skepp_6_OP1_4 : ITemplateCalculations
    {
        private static readonly string[] Machines = new[] { "Skepp6", "SKEPP6", "Skepp 6", "SKEPP 6" };

        private static readonly int[] Serie30TypValues = new[]
        {
            44, 48, 52, 72, 76, 80, 84, 88, 96, 500, 530, 560, 600, 630, 710, 750, 800, 850, 900, 950, 1000, 1060
        };

        private static readonly int[] Serie31TypValues = new[]
        {
            60, 68, 72, 76, 80, 88, 92, 96, 500, 560, 600, 630, 670, 710, 750, 800, 850, 1000
        };

        private static readonly double[] Serie30D1List = new[]
        {
            237, 264, 288, 394, 422, 442, 462, 488, 530, 550, 571, 610, 657, 690, 766, 820, 870, 925, 975, 1025, 1085, 1145.0
        };

        private static readonly double[] Serie31D1List = new[]
        {
            335, 382, 406, 438, 456, 508, 535, 560, 580, 650, 690, 730, 775, 825, 875, 925, 975, 1140.0
        };

        private static readonly double[] Serie30TList = new[]
        {
            5, 8, 8, 8, 10, 10, 10, 12, 12, 12, 15, 15, 18, 18, 20, 20, 20, 20, 25, 25, 25, 25.0
        };

        private static readonly double[] Serie31TList = new[]
        {
            5, 8, 10, 15, 15, 15, 20, 20, 12, 15, 15, 18, 18, 20, 20, 20, 25, 25.0
        };

        private static readonly double[] Serie30B2List = new[]
        {
            0, 0, 0, 0, 0, 0, 0, 30, 31, 34, 42, 37, 40, 40, 50, 46, 46, 48, 58, 58, 58, 58.0
        };

        private static readonly double[] Serie31B2List = new[]
        {
            0, 0, 35, 39, 43, 42, 45, 49, 38, 50, 50, 55, 57, 61, 61, 61, 69, 73.0
        };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = GetRequestValue(req,
                "Subject", "ProductDesignation", "Designation", "Produktbeteckning", "Product", "Item", "Artikel");
            string machineRaw = GetRequestValue(req,
                "MaskinVal", "MachineNumber", "Machine", "Maskin", "MachineNo", "MaskinNr");
            string machineNorm = NormalizeMachine(machineRaw);

            string published = GetRequestValue(req, "Published", "PublishDate", "Publiserad", "Publicerad");
            kv["VaLPopUp"] = ComputePopup(published);

            string tmpFormat = (subject ?? string.Empty).Trim().ToUpperInvariant();
            string tmpBet = tmpFormat.Replace('.', ',');
            string[] tokens = SplitSubject(tmpBet);

            string tmpBet1 = TokenAt(tokens, 0);
            string tmpBet2 = TokenAt(tokens, 1);
            string tmpBet3 = TokenAt(tokens, 2);
            string tmpBet4 = TokenAt(tokens, 3);
            string tmpBet5 = TokenAt(tokens, 4);
            string tmpBet6 = TokenAt(tokens, 5);

            bool tmpSlash = tmpFormat.Contains("/");
            int tmpSerie = ToIntSafe(Left(tmpBet2, 2));
            string tmpTypText = tmpSlash ? tmpBet3 : Middle(tmpBet2, 2, 2);
            int tmpTyp = ToIntSafe(tmpTypText);
            int tmpTypIndex = GetTypIndex(tmpSerie, tmpTyp);
            double tmpkd = tmpSlash ? ToDoubleSafe(tmpBet3) : (ToDoubleSafe(tmpTypText) / 2.0) * 10.0;
            int tmpStmm = tmpTyp < 61 ? 4 : tmpTyp < 501 ? 5 : tmpTyp < 671 ? 6 : tmpTyp < 901 ? 7 : 8;

            kv["SumP"] = "(P) " + Fmt(tmpStmm);
            kv["SumGänga"] = "Tr " + Fmt(tmpkd) + "x" + Fmt(tmpStmm);
            kv["SumGMått"] = kv["SumGänga"];
            kv["SumGMall"] = "Tr x " + Fmt(tmpStmm);

            kv["SumF3"] = "30º";
            kv["SumGF"] = "30º";
            kv["SumF1"] = "45º";
            kv["SumF1S3"] = "45º";
            kv["SumF2"] = "45º";
            kv["SumF2S3"] = "45º";
            kv["SumGV"] = "45º";

            double tmpkda = (tmpStmm == 4 || tmpStmm == 5) ? tmpkd + 0.5 : tmpkd + 1.0;
            kv["Sumkd"] = "(kd) " + Fmt(tmpkda);
            kv["SumkdTol"] = KdTol(tmpkd);

            double tmpdm = tmpStmm == 4 ? tmpkd - 2 : tmpStmm == 5 ? tmpkd - 2.5 : tmpStmm == 6 ? tmpkd - 3 : tmpStmm == 7 ? tmpkd - 3.5 : tmpkd - 4;
            kv["Sumdm"] = "(dm) " + Fmt(tmpdm).Replace(".", ",");
            kv["SumdmTol"] = "+ " + Fmt3(DmTolValue(tmpkd));
            kv["SumdmTolN"] = " 0";

            double tmpid = tmpStmm == 4 ? tmpkd - 4 : tmpStmm == 5 ? tmpkd - 5 : tmpStmm == 6 ? tmpkd - 6 : tmpStmm == 7 ? tmpkd - 7 : tmpkd - 8;
            kv["Sumid"] = "(id) " + Fmt(tmpid);
            kv["SumidTol"] = "+ " + Fmt3(IdTolValue(tmpkd));
            kv["SumidTolN"] = " 0";
            kv["SumidSSK"] = "(id) " + Fmt(tmpid - 1.0);

            double tmpD = ComputeOuterDiameter(tmpSerie, tmpkd);
            kv["SumD"] = "(D) " + Fmt(tmpD);
            kv["SumDTol"] = " 0";
            kv["SumDTolN"] = "- " + Fmt3(H11Tol(tmpD));

            double tmpD1 = GetIndexedValue(tmpSerie == 30 ? Serie30D1List : Serie31D1List, tmpTypIndex);
            kv["SumD1"] = "(D1) " + Fmt(tmpD1);
            kv["SumD1Tol"] = " 0";
            kv["SumD1TolN"] = "- " + Fmt3(ExternalDiameterTolN(tmpD1));
            kv["SumD1S1"] = kv["SumD1"];
            kv["SumD1S1Tol"] = kv["SumD1Tol"];
            kv["SumD1S1TolN"] = kv["SumD1TolN"];

            double tmpD3 = tmpkd < 501 ? tmpkd + 2 : tmpkd + 3;
            kv["SumD3"] = "(D3) " + Fmt(tmpD3);
            kv["SumD3Tol"] = "+ " + Fmt3(D3Tol(tmpD3));
            kv["SumD3TolN"] = " 0";
            kv["SumD3SSK"] = "(D3) " + Fmt(tmpD3 + 2) + " (x2)";

            double tmpB = ComputeB(tmpSerie, tmpkd);
            kv["SumB"] = "(B) " + Fmt(tmpB);
            kv["SumBTol"] = " 0";
            double tmpBTolN = BTolNValue(tmpB);
            kv["SumBTolN"] = "- " + Fmt3(tmpBTolN);
            kv["SumBSSK"] = "(B) " + Fmt(tmpB);

            double tmpT = GetIndexedValue(tmpSerie == 30 ? Serie30TList : Serie31TList, tmpTypIndex);
            kv["SumT"] = "(T) " + Fmt(tmpT);
            kv["SumTTol"] = TTol(tmpT);
            double tmpTSSK = Round2(tmpT + (tmpBTolN / 2.0));
            kv["SumTSSK"] = "(T) " + Fmt(tmpTSSK).Replace(".", ",");

            double tmpHM = (tmpD - tmpD1) / 2.0;
            kv["SumHM"] = "(HM) " + Fmt(tmpHM).Replace(".", ",");
            double tmpHM1 = Round2((tmpHM * Math.Tan(30 * Math.PI / 180.0)) + tmpTSSK);
            double tmpHM1a = Round2((tmpHM * Math.Tan(30 * Math.PI / 180.0)) + tmpT);
            kv["SumHM1"] = "(HM) " + Fmt(tmpHM1).Replace(".", ",");
            kv["SumHM1a"] = "(HM) " + Fmt(tmpHM1a).Replace(".", ",");

            string sumR = tmpkd < 221 ? "R 2.5" : tmpkd < 281 ? "R 3" : tmpkd < 441 ? "R 3.5" : tmpkd < 601 ? "R 4" : tmpkd < 711 ? "R 5" : "R 6";
            kv["SumR"] = sumR;
            kv["SumR3b"] = sumR;
            kv["SumRHT"] = "R1.6";
            kv["SumRa32"] = "3.2";

            double tmpB2 = GetIndexedValue(tmpSerie == 30 ? Serie30B2List : Serie31B2List, tmpTypIndex);
            kv["SumB2"] = tmpB2 == 0 ? string.Empty : "(X) " + Fmt(tmpB2);

            double tmpGG = ComputeLiftEyeThread(tmpSerie, tmpkd);
            kv["SumGG"] = tmpB2 == 0 ? string.Empty : "Gänga: M" + Fmt(tmpGG);
            double tmpL = tmpB2 == 0 ? 0 : (tmpGG == 10 ? 17 : tmpGG == 12 ? 21 : 27);
            kv["SumL"] = tmpB2 == 0 ? string.Empty : "(L) " + Fmt(tmpL);
            kv["SumGGTolk"] = tmpB2 == 0 ? string.Empty : "Kontrolleras med min/max tolk M" + Fmt(tmpGG);

            double tmpta = ComputeGrooveDepth(tmpSerie, tmpkd);
            kv["Sumta"] = "(t) " + Fmt(tmpta);
            kv["SumtaTol"] = TaTol(tmpta);
            kv["SumtaTolN"] = " 0";

            double tmpS = ComputeGrooveWidth(tmpSerie, tmpkd);
            kv["SumS"] = "(S) " + Fmt(tmpS);
            kv["SumSTol"] = STol(tmpS);

            string tmpLR = tmpS < 7 ? "0.5" : tmpS < 10 ? "0.75" : tmpS < 19 ? "1.0" : tmpS < 31 ? "1.25" : "1.5";
            kv["SumLR"] = tmpLR;

            string tmpKa = FlatnessRunout(tmpkd);
            kv["SumPL"] = tmpKa;
            kv["SumK"] = tmpKa;

            double tmpd4 = ComputeD4(tmpSerie, tmpkd, tmpta);
            kv["Sumd4"] = "(d4) " + Fmt(tmpd4);
            kv["Sumd4Tol"] = "± " + Fmt3(D4TolValue(tmpd4));

            double tmpu = ComputeU(tmpSerie, tmpkd);
            kv["Sumu"] = "M" + Fmt(tmpu);
            double tmpv = ComputeV(tmpSerie, tmpkd, tmpu);
            kv["Sumv"] = "(v) min:" + Fmt(tmpv);
            kv["SumGuTolk"] = "M" + Fmt(tmpu) + " min/max";

            kv["SumTextS1"] = "Grader, frifläckar, slagmärken, repor, valkar och andra defekter samt ojämnheter kontrolleras med ögonmått <<LineBreak>> För övriga mått används skjutmått.";
            kv["SumTextS2"] = "Övriga mått kontrolleras vid inställning <<LineBreak>>" + kv["SumTextS1"];
            kv["SumTextS3"] = kv["SumTextS1"];
            kv["SumStämpling"] = "Stämplas enl. ritning 7430189";

            string tmpRit = tmpSerie == 30 ? "223015, 7438482" : "223016, 7438482";
            kv["SumRitningsnrS1"] = "Produkt: " + tmpRit;
            kv["SumRitningsnrS2"] = kv["SumRitningsnrS1"];
            kv["SumRitningsnrS3"] = kv["SumRitningsnrS1"];
            kv["SumTolRitS1"] = "Toleranser: 1432008";
            kv["SumTolRitS2"] = kv["SumTolRitS1"];
            kv["SumTolRitS3"] = kv["SumTolRitS1"];
            kv["SumGTolRitS3"] = "Gänga: 7430181";
            kv["SumYtRitS1"] = "Yta: 7430184";
            kv["SumYtRitS2"] = kv["SumYtRitS1"];
            kv["SumYtRitS3"] = kv["SumYtRitS1"];
            kv["SumKlEgenskaperS1"] = @"PRODUCTION NUTS & SLEEVES & HOUSINGS\ALLMÄN\KLASSADE EGENSKAPER\Klassade egenskaper muttrar";
            kv["SumKlEgenskaperS2"] = kv["SumKlEgenskaperS1"];
            kv["SumKlEgenskaperS3"] = kv["SumKlEgenskaperS1"];

            // EXACT Lotus validation logic:
            // VaLArtKontr = @if("[TmpBet1]"="HME" & [TmpSerie]=30:31;"";"FEL MALL ...")
            bool validArticle = string.Equals(tmpBet1, "HME", StringComparison.OrdinalIgnoreCase) && (tmpSerie == 30 || tmpSerie == 31);
            kv["VaLArtKontr"] = validArticle
                ? string.Empty
                : "FEL MALL - Denna mall gäller BARA HME 30/31" + Environment.NewLine + Environment.NewLine + "---------> Kontrollera inmatningsfält <---------";

            bool isSkepp6 = IsMachine(machineNorm);
            kv["SumMaskinValS1"] = "Maskin: " + (isSkepp6 ? "Morando" : string.Empty) + " - OP1 & 2";
            kv["SumMaskinValS2"] = "Maskin: " + (isSkepp6 ? "K&T" : string.Empty) + " - OP3";
            kv["SumMaskinValS3"] = "Maskin: " + (isSkepp6 ? "1150" : string.Empty) + " - OP4";

            SetWhenMachine(kv, isSkepp6, "SumF1_1", "1/1");
            SetWhenMachine(kv, isSkepp6, "SumF1_2", "1/1");
            SetWhenMachine(kv, isSkepp6, "SumF1_3", "1/1");
            SetWhenMachine(kv, isSkepp6, "SumF1_4", "1/1");
            SetWhenMachine(kv, isSkepp6, "SumF1_5", "1/1");
            SetWhenMachine(kv, isSkepp6, "SumF1_6", "1/1");
            SetWhenMachine(kv, isSkepp6, "SumF1_7", "1/1");
            SetWhenMachine(kv, isSkepp6, "SumF1_8", "1/1");
            SetWhenMachine(kv, isSkepp6, "SumF1_9", "1/1");
            SetWhenMachine(kv, isSkepp6, "SumF2_1", "1/1");
            SetWhenMachine(kv, isSkepp6, "SumF2_2", "1/1");
            SetWhenMachine(kv, isSkepp6, "SumF2_3", "1/1");
            SetWhenMachine(kv, isSkepp6, "SumF2_4", "1/1");
            SetWhenMachine(kv, isSkepp6, "SumF3_1", "1/1");
            SetWhenMachine(kv, isSkepp6, "SumF3_2", "1/1");
            SetWhenMachine(kv, isSkepp6, "SumF3_3", "1/1");
            SetWhenMachine(kv, isSkepp6, "SumF3_4", "1/1");
            SetWhenMachine(kv, isSkepp6, "SumF3_5", "1/1");
            SetWhenMachine(kv, isSkepp6, "SumF3_6", "1/1");
            SetWhenMachine(kv, isSkepp6, "SumF3_7", "1/1");
            SetWhenMachine(kv, isSkepp6, "SumF3_8", "1/1");
            SetWhenMachine(kv, isSkepp6, "SumF3_9", "1/1");
            SetWhenMachine(kv, isSkepp6, "SumF3_0", "1/1");

            SetWhenMachine(kv, isSkepp6, "SumD1_1", "Skjutmått");
            SetWhenMachine(kv, isSkepp6, "SumD1_2", "Mikrometer");
            SetWhenMachine(kv, isSkepp6, "SumD1_3", "Djupmått/Skjutmått");
            SetWhenMachine(kv, isSkepp6, "SumD1_4", "Skjutmått");
            SetWhenMachine(kv, isSkepp6, "SumD1_5", "Djupmått/Skjutmått");
            SetWhenMachine(kv, isSkepp6, "SumD1_6", "Fasmall/Skjutmått");
            SetWhenMachine(kv, isSkepp6, "SumD1_7", "Radiestål");
            SetWhenMachine(kv, isSkepp6, "SumD1_8", "Djupmått/Skjutmått");
            SetWhenMachine(kv, isSkepp6, "SumD1_9", "Ytjämnhetsmätare");
            SetWhenMachine(kv, isSkepp6, "SumD2_1", "Skjutmått");
            SetWhenMachine(kv, isSkepp6, "SumD2_2", "Djupmått");
            SetWhenMachine(kv, isSkepp6, "SumD2_3", kv["SumGuTolk"]);
            SetWhenMachine(kv, isSkepp6, "SumD2_4", kv["SumGGTolk"]);
            SetWhenMachine(kv, isSkepp6, "SumD3_1", "Multimar");
            SetWhenMachine(kv, isSkepp6, "SumD3_2", "Mikrometerstickmått");
            SetWhenMachine(kv, isSkepp6, "SumD3_3", "Skjutmått");
            SetWhenMachine(kv, isSkepp6, "SumD3_4", "Skjutmått");
            SetWhenMachine(kv, isSkepp6, "SumD3_5", "Skjutmått");
            SetWhenMachine(kv, isSkepp6, "SumD3_6", "Fasmall");
            SetWhenMachine(kv, isSkepp6, "SumD3_7", "Gängmall");
            SetWhenMachine(kv, isSkepp6, "SumD3_8", "Ytjämnhetsmätare");
            SetWhenMachine(kv, isSkepp6, "SumD3_9", string.Empty);
            SetWhenMachine(kv, isSkepp6, "SumD3_0", "Egglinjal");

            for (int i = 1; i <= 9; i++) kv["SumAF1_" + i.ToString(CultureInfo.InvariantCulture)] = string.Empty;
            kv["SumAF2_1"] = string.Empty;
            kv["SumAF2_2"] = string.Empty;
            kv["SumAF2_3"] = string.Empty;
            kv["SumAF2_4"] = string.Empty;
            kv["SumAF3_1"] = isSkepp6 ? "Kontrolleras med passbitsklove utf. 1" : string.Empty;
            kv["SumAF3_2"] = string.Empty;
            kv["SumAF3_3"] = string.Empty;
            kv["SumAF3_4"] = string.Empty;
            kv["SumAF3_5"] = string.Empty;
            kv["SumAF3_6"] = string.Empty;
            kv["SumAF3_7"] = string.Empty;
            kv["SumAF3_8"] = string.Empty;
            kv["SumAF3_9"] = isSkepp6 ? "Körs i samma uppspänning" : string.Empty;
            kv["SumAF3_0"] = isSkepp6 ? "Vid misstänkt formfel lämnas muttern till mätrum" : string.Empty;
            kv["SumAF1_9"] = isSkepp6 ? "Övriga Ra värden 6,3" : string.Empty;

            // Optional input logic - mirrors Lotus Notes exactly.
            //ApplyOptionalInputs(kv, req);
            var bm = req.Bookmarks?.ToDictionary(
                b => b.BookmarkName,
                b => b.BookmarkValue,
                StringComparer.OrdinalIgnoreCase)
                ?? new Dictionary<string, string>();

            string Get(string k) => bm.TryGetValue(k, out var v) ? v : "";
            AddSetupAndMeasurementGroups(kv,Get);

            _ = tmpBet4;
            _ = tmpBet5;
            _ = tmpBet6;

            return kv;
        }
        


        private void AddSetupAndMeasurementGroups(
    Dictionary<string, string> r,
    Func<string, string> Get)
        {
            string Val(string key, string unit = "")
            {
                var v = Get(key);
                return v == "0" || string.IsNullOrWhiteSpace(v) ? "" : $"{v}{unit}";
            }

            // =====================================================
            // ✅ FÄRDIGMÅTT (Finished Measurement)
            // =====================================================
            bool hasFM =
                Get("Färdigmått Utv") != "0" ||
                Get("Linjal Utv") != "0" ||
                Get("Färdigmått Inv") != "0" ||
                Get("Linjal Inv") != "0";

            r["SumFM"] = hasFM ? "Färdigmått" : "";

            // ✅ OUTER
            r["SumFMU"] = Get("Färdigmått Utv") == "0" ? "" : "Utvändig diameter:";
            r["SumFMUtv"] = Val("Färdigmått Utv", " mm");

            r["SumUL"] = Get("Linjal Utv") == "0" ? "" : "Motsvarar på linjal:";
            r["SumUtvLin"] = Val("Linjal Utv", " mm");

            // ✅ INNER
            r["SumFMI"] = Get("Färdigmått Inv") == "0" ? "" : "Invändig diameter:";
            r["SumFMInv"] = Val("Färdigmått Inv", " mm");

            r["SumIL"] = Get("Linjal Inv") == "0" ? "" : "Motsvarar på linjal:";
            r["SumInvLin"] = Val("Linjal Inv", " mm");

            // =====================================================
            // ✅ INSTÄLLNING SSK/FINSKÄR (Machine Setup)
            // =====================================================
            bool hasIN =
                Get("Chuckbackar") != "0" ||
                Get("Stödbackar") != "0" ||
                Get("Grader") != "0" ||
                Get("Varvtal") != "0" ||
                Get("Matning Plan") != "0" ||
                Get("Matning Utv/Inv") != "0";

            r["SumIN"] = hasIN ? "Inställning SSK/Finskär" : "";

            // ✅ Chuckbackar
            r["SumCB"] = Get("Chuckbackar") == "0" ? "" : "Chuckbackar:";
            r["SumChuckback"] = Val("Chuckbackar");

            // ✅ Stödbackar
            r["SumSB"] = Get("Stödbackar") == "0" ? "" : "Stödbackar:";
            r["SumStödback"] = Val("Stödbackar", " mm");

            // ✅ Grader
            r["SumGR"] = Get("Grader") == "0" ? "" : "Grader:";
            r["SumGrader"] = Val("Grader", " mm");

            // ✅ Varvtal
            r["SumVR"] = Get("Varvtal") == "0" ? "" : "Varvtal:";
            r["SumVarv"] = Val("Varvtal", " /min");

            // ✅ Matning Plan
            r["SumMP"] = Get("Matning Plan") == "0" ? "" : "Matning Plan:";
            r["SumMatPl"] = Val("Matning Plan", " /min");

            // ✅ Matning Utv/Inv
            r["SumMIU"] = Get("Matning Utv/Inv") == "0" ? "" : "Matning Utv/Inv:";
            r["SumMatInUt"] = Val("Matning Utv/Inv", " /min");
        }


        private static void ApplyOptionalInputs(Dictionary<string, string> kv, object req)
        {
            // CHUCKBACKAR
            string chuckbackar = GetRequestValue(req, "Chuckbackar", "Chuck", "ChuckBackar");
            bool hasChuckbackar = !IsZeroLike(chuckbackar);
            kv["SumChuckback"] = hasChuckbackar ? CleanValue(chuckbackar) : string.Empty;
            kv["SumCB"] = hasChuckbackar ? "Chuckbackar:" : string.Empty;

            // STÖDBACKAR
            string stodbackar = GetRequestValue(req, "Stödbackar", "Stodbackar", "Stöd", "Stod");
            bool hasStodbackar = !IsZeroLike(stodbackar);
            kv["SumStödback"] = hasStodbackar ? CleanValue(stodbackar) + " mm" : string.Empty;
            kv["SumStodback"] = kv["SumStödback"]; // fallback alias if template uses ascii key
            kv["SumSB"] = hasStodbackar ? "Stödbackar:" : string.Empty;

            // GRADER
            string grader = GetRequestValue(req, "Grader", "Grade");
            bool hasGrader = !IsZeroLike(grader);
            kv["SumGrader"] = hasGrader ? CleanValue(grader) + " mm" : string.Empty;
            kv["SumGR"] = hasGrader ? "Grader:" : string.Empty;

            // VARVTAL
            string varvtal = GetRequestValue(req, "Varvtal", "RPM", "SpindleSpeed");
            bool hasVarvtal = !IsZeroLike(varvtal);
            kv["SumVarv"] = hasVarvtal ? CleanValue(varvtal) + " /min" : string.Empty;
            kv["SumVR"] = hasVarvtal ? "Varvtal:" : string.Empty;

            // MATNING PLAN
            string matningPlan = GetRequestValue(req, "Matning Plan", "MatningPlan", "MatPl", "FeedPlan");
            bool hasMatningPlan = !IsZeroLike(matningPlan);
            kv["SumMatPl"] = hasMatningPlan ? CleanValue(matningPlan) + " /min" : string.Empty;
            kv["SumMP"] = hasMatningPlan ? "Matning Plan:" : string.Empty;

            // MATNING UTV/INV
            string matningUtvInv = GetRequestValue(req, "Matning Utv/Inv", "MatningUtvInv", "MatInUt", "FeedOuterInner");
            bool hasMatningUtvInv = !IsZeroLike(matningUtvInv);
            kv["SumMatInUt"] = hasMatningUtvInv ? CleanValue(matningUtvInv) + " /min" : string.Empty;
            kv["SumMIU"] = hasMatningUtvInv ? "Matning Utv/Inv:" : string.Empty;

            // FÄRDIGMÅTT & LINJAL UTVÄNDIGT
            string fmu = GetRequestValue(req, "Färdigmått Utv", "FardigmattUtv", "FardigMattUtv", "FMUtv", "FinishedOuterDiameter");
            bool hasFmu = !IsZeroLike(fmu);
            kv["SumFMUtv"] = hasFmu ? CleanValue(fmu) + " mm" : string.Empty;
            kv["SumFMU"] = hasFmu ? "Utvändig diameter:" : string.Empty;

            string utvLin = GetRequestValue(req, "Linjal Utv", "LinjalUtv", "UtvLin", "OuterScale");
            bool hasUtvLin = !IsZeroLike(utvLin);
            kv["SumUtvLin"] = hasUtvLin ? CleanValue(utvLin) + " mm" : string.Empty;
            kv["SumUL"] = hasUtvLin ? "Motsvarar på linjal:" : string.Empty;

            // FÄRDIGMÅTT & LINJAL INVÄNDIGT
            string fmi = GetRequestValue(req, "Färdigmått Inv", "FardigmattInv", "FardigMattInv", "FMInv", "FinishedInnerDiameter");
            bool hasFmi = !IsZeroLike(fmi);
            kv["SumFMInv"] = hasFmi ? CleanValue(fmi) + " mm" : string.Empty;
            kv["SumFMI"] = hasFmi ? "Invändig diameter:" : string.Empty;

            string invLin = GetRequestValue(req, "Linjal Inv", "LinjalInv", "InvLin", "InnerScale");
            bool hasInvLin = !IsZeroLike(invLin);
            kv["SumInvLin"] = hasInvLin ? CleanValue(invLin) + " mm" : string.Empty;
            kv["SumIL"] = hasInvLin ? "Motsvarar på linjal:" : string.Empty;

            // HEADERS - exactly like Lotus:
            kv["SumIN"] = (hasChuckbackar || hasStodbackar || hasGrader || hasVarvtal || hasMatningPlan || hasMatningUtvInv)
                ? "Inställning SSK/Finskär"
                : string.Empty;

            kv["SumFM"] = (hasFmu || hasUtvLin || hasFmi || hasInvLin)
                ? "Färdigmått"
                : string.Empty;
        }

        private static string[] SplitSubject(string input)
        {
            return (input ?? string.Empty).Split(new[] { ' ', '/', '.', '-', ',' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static string TokenAt(string[] tokens, int index)
        {
            return tokens != null && index >= 0 && index < tokens.Length ? tokens[index] : string.Empty;
        }

        private static string Left(string value, int length)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            return value.Length <= length ? value : value.Substring(0, length);
        }

        private static string Middle(string value, int startIndexZeroBased, int length)
        {
            if (string.IsNullOrEmpty(value) || startIndexZeroBased < 0 || startIndexZeroBased >= value.Length) return string.Empty;
            if (startIndexZeroBased + length > value.Length) length = value.Length - startIndexZeroBased;
            return value.Substring(startIndexZeroBased, length);
        }

        private static string GetRequestValue(object req, params string[] names)
        {
            if (req == null || names == null || names.Length == 0) return string.Empty;

            // 1) req itself as dictionary
            string value = TryGetFromDictionaryObject(req, names);
            if (!string.IsNullOrEmpty(value)) return value;

            Type t = req.GetType();

            // 2) public properties
            foreach (PropertyInfo p in t.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                foreach (string n in names)
                {
                    if (NormalizeKey(p.Name) == NormalizeKey(n))
                    {
                        return ToStringValue(p.GetValue(req, null));
                    }
                }
            }

            // 3) public fields
            foreach (FieldInfo f in t.GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                foreach (string n in names)
                {
                    if (NormalizeKey(f.Name) == NormalizeKey(n))
                    {
                        return ToStringValue(f.GetValue(req));
                    }
                }
            }

            // 4) nested dictionaries on public properties
            foreach (PropertyInfo p in t.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                object container = p.GetValue(req, null);
                value = TryGetFromDictionaryObject(container, names);
                if (!string.IsNullOrEmpty(value)) return value;
            }

            // 5) nested dictionaries on public fields
            foreach (FieldInfo f in t.GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                object container = f.GetValue(req);
                value = TryGetFromDictionaryObject(container, names);
                if (!string.IsNullOrEmpty(value)) return value;
            }

            return string.Empty;
        }

        private static string TryGetFromDictionaryObject(object obj, params string[] names)
        {
            if (obj == null) return string.Empty;

            if (obj is IDictionary dict)
            {
                foreach (object keyObj in dict.Keys)
                {
                    string key = Convert.ToString(keyObj, CultureInfo.InvariantCulture) ?? string.Empty;
                    foreach (string n in names)
                    {
                        if (NormalizeKey(key) == NormalizeKey(n))
                        {
                            return ToStringValue(dict[keyObj]);
                        }
                    }
                }
            }

            return string.Empty;
        }

        private static string ToStringValue(object raw)
        {
            if (raw == null) return string.Empty;
            return Convert.ToString(raw, CultureInfo.InvariantCulture) ?? string.Empty;
        }

        private static bool IsZeroLike(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return true;
            string trimmed = value.Trim();
            if (trimmed == "0" || trimmed == "0.0" || trimmed == "0,0" || trimmed == "0.00" || trimmed == "0,00") return true;

            double number;
            if (double.TryParse(trimmed.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out number))
            {
                return Math.Abs(number) < 0.0000001;
            }

            return false;
        }

        private static string CleanValue(string value)
        {
            return (value ?? string.Empty).Trim();
        }

        private static string NormalizeKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return string.Empty;
            string normalized = key.Normalize(NormalizationForm.FormD);
            var chars = new List<char>(normalized.Length);
            foreach (char c in normalized)
            {
                UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(c);
                if (category == UnicodeCategory.NonSpacingMark) continue;
                if (char.IsLetterOrDigit(c)) chars.Add(char.ToUpperInvariant(c));
            }
            return new string(chars.ToArray());
        }

        private static string NormalizeMachine(string machine)
        {
            return (machine ?? string.Empty).Replace(" ", string.Empty).Trim();
        }

        private static bool IsMachine(string machineNorm)
        {
            if (string.IsNullOrWhiteSpace(machineNorm)) return false;
            foreach (string m in Machines)
            {
                if (string.Equals(NormalizeMachine(m), machineNorm, StringComparison.OrdinalIgnoreCase)) return true;
            }
            return false;
        }

        private static void SetWhenMachine(Dictionary<string, string> kv, bool isMachine, string key, string value)
        {
            kv[key] = isMachine ? value : string.Empty;
        }

        private static int GetTypIndex(int serie, int typ)
        {
            int[] list = serie == 30 ? Serie30TypValues : Serie31TypValues;
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] == typ) return i + 1;
            }
            return 0;
        }

        private static double GetIndexedValue(double[] list, int oneBasedIndex)
        {
            if (list == null || oneBasedIndex < 1 || oneBasedIndex > list.Length) return 0;
            return list[oneBasedIndex - 1];
        }

        private static int ToIntSafe(string value)
        {
            int result;
            if (int.TryParse((value ?? string.Empty).Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out result)) return result;
            return 0;
        }

        private static double ToDoubleSafe(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return 0;
            value = value.Trim().Replace(',', '.');
            double result;
            if (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result)) return result;
            return 0;
        }

        private static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published)) return string.Empty;
            DateTime pubDt;
            if (!DateTime.TryParse(published, out pubDt)) return string.Empty;
            int days = 14;
            DateTime validUntil = pubDt.AddDays(days);
            if (DateTime.Today <= validUntil.Date)
            {
                return "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom " + days.ToString(CultureInfo.InvariantCulture) + " dagar)"
                    + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine
                    + "Popupruta aktiv till " + validUntil.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            return string.Empty;
        }

        private static string KdTol(double tmpkd)
        {
            if (tmpkd < 6) return "± 0.1";
            if (tmpkd < 30) return "± 0.2";
            if (tmpkd < 120) return "± 0.3";
            if (tmpkd < 400) return "± 0.5";
            if (tmpkd < 1000) return "± 0.8";
            if (tmpkd < 2000) return "± 1.2";
            return "± 2.0";
        }

        private static double DmTolValue(double tmpkd)
        {
            if (tmpkd < 301) return 0.475;
            if (tmpkd < 501) return 0.530;
            if (tmpkd < 701) return 0.600;
            if (tmpkd < 901) return 0.630;
            return 0.710;
        }

        private static double IdTolValue(double tmpkd)
        {
            if (tmpkd < 301) return 0.375;
            if (tmpkd < 501) return 0.450;
            if (tmpkd < 701) return 0.500;
            if (tmpkd < 901) return 0.560;
            return 0.630;
        }

        private static double ComputeOuterDiameter(int serie, double kd)
        {
            if (serie == 30)
            {
                if (kd < 221) return kd + 40;
                if (kd < 281) return kd + 50;
                if (kd < 361) return kd + 60;
                if (kd < 421) return kd + 70;
                if (kd < 501) return kd + 80;
                if (Math.Abs(kd - 560) < 0.0001) return kd + 90;
                if (kd < 631) return kd + 100;
                if (kd < 671) return kd + 110;
                if (kd < 801) return kd + 120;
                if (kd < 951) return kd + 130;
                return kd + 140;
            }

            if (kd < 241) return kd + 60;
            if (kd < 281) return kd + 70;
            if (kd < 321) return kd + 80;
            if (kd < 361) return kd + 100;
            if (kd < 381) return kd + 110;
            if (kd < 461) return kd + 120;
            if (kd < 481) return kd + 140;
            if (kd < 501) return kd + 130;
            if (kd < 531) return kd + 140;
            if (kd < 601) return kd + 150;
            if (kd < 631) return kd + 170;
            if (kd < 671) return kd + 180;
            if (kd < 711) return kd + 190;
            if (kd < 801) return kd + 200;
            if (kd < 851) return kd + 210;
            if (kd < 951) return kd + 220;
            return kd + 240;
        }

        private static double ExternalDiameterTolN(double d1)
        {
            if (d1 < 19) return 0.270;
            if (d1 < 31) return 0.330;
            if (d1 < 51) return 0.390;
            if (d1 < 81) return 0.460;
            if (d1 < 121) return 0.540;
            if (d1 < 181) return 0.630;
            if (d1 < 251) return 0.720;
            if (d1 < 316) return 0.810;
            if (d1 < 401) return 0.890;
            if (d1 < 501) return 0.970;
            if (d1 < 631) return 1.100;
            if (d1 < 801) return 1.250;
            if (d1 < 1000) return 1.400;
            return 1.650;
        }

        private static double D3Tol(double d3)
        {
            if (d3 < 19) return 0.430;
            if (d3 < 31) return 0.520;
            if (d3 < 51) return 0.620;
            if (d3 < 81) return 0.740;
            if (d3 < 121) return 0.870;
            if (d3 < 181) return 1.000;
            if (d3 < 251) return 1.150;
            if (d3 < 316) return 1.300;
            if (d3 < 401) return 1.400;
            if (d3 < 501) return 1.550;
            if (d3 < 631) return 1.750;
            if (d3 < 801) return 2.000;
            if (d3 < 1000) return 2.300;
            return 2.600;
        }

        private static double ComputeB(int serie, double kd)
        {
            if (serie == 30)
            {
                if (kd < 221) return 30;
                if (kd < 261) return 34;
                if (kd < 281) return 38;
                if (kd < 321) return 42;
                if (kd < 361) return 45;
                if (kd < 381) return 48;
                if (kd < 421) return 52;
                if (kd < 481) return 60;
                if (kd < 531) return 68;
                if (kd < 631) return 75;
                if (kd < 671) return 80;
                if (kd < 851) return 90;
                if (kd < 1181) return 100;
                return 110;
            }

            if (kd < 221) return 32;
            if (kd < 241) return 34;
            if (kd < 261) return 36;
            if (kd < 281) return 38;
            if (kd < 301) return 40;
            if (kd < 321) return 42;
            if (kd < 341) return 55;
            if (kd < 361) return 58;
            if (kd < 381) return 60;
            if (kd < 401) return 62;
            if (kd < 441) return 70;
            if (kd < 481) return 75;
            if (kd < 531) return 80;
            if (kd < 601) return 85;
            if (kd < 631) return 95;
            if (kd < 711) return 106;
            if (kd < 801) return 112;
            if (kd < 851) return 118;
            return 125;
        }

        private static double BTolNValue(double b)
        {
            if (b < 7) return 0.180;
            if (b < 11) return 0.220;
            if (b < 19) return 0.270;
            if (b < 31) return 0.330;
            if (b < 51) return 0.390;
            if (b < 81) return 0.460;
            if (b < 121) return 0.540;
            return 0.630;
        }

        private static string TTol(double t)
        {
            if (t < 6.1) return "± 0.1";
            if (t < 30.1) return "± 0.2";
            return "± 0.3";
        }

        private static double ComputeLiftEyeThread(int serie, double kd)
        {
            if (serie == 30)
            {
                if (kd < 671) return 10;
                if (kd < 901) return 12;
                return 16;
            }

            if (kd < 531) return 10;
            if (kd < 601) return 12;
            if (kd < 751) return 16;
            if (kd < 901) return 20;
            return 24;
        }

        private static double ComputeGrooveDepth(int serie, double kd)
        {
            if (serie == 30)
            {
                if (kd < 221) return 9;
                if (kd < 281) return 10;
                if (kd < 341) return 12;
                if (kd < 361) return 13;
                if (kd < 421) return 14;
                if (kd < 501) return 15;
                if (kd < 671) return 20;
                return 25;
            }

            if (kd < 241) return 10;
            if (kd < 321) return 12;
            if (kd < 361) return 15;
            if (kd < 421) return 18;
            if (kd < 481) return 20;
            if (kd < 531) return 23;
            if (kd < 601) return 25;
            if (kd < 671) return 28;
            if (kd < 711) return 30;
            if (kd < 801) return 34;
            return 38;
        }

        private static string TaTol(double ta)
        {
            if (ta < 11) return "+ 1.5";
            if (ta < 19) return "+ 1.8";
            if (ta < 31) return "+ 2.1";
            return "+ 2.5";
        }

        private static double ComputeGrooveWidth(int serie, double kd)
        {
            if (serie == 30)
            {
                if (kd < 261) return 20;
                if (kd < 341) return 24;
                if (kd < 401) return 28;
                if (kd < 461) return 32;
                if (kd < 501) return 36;
                if (kd < 601) return 40;
                if (kd < 671) return 45;
                if (kd < 711) return 50;
                if (kd < 801) return 55;
                return 60;
            }

            if (kd < 261) return 20;
            if (kd < 321) return 24;
            if (kd < 361) return 28;
            if (kd < 421) return 32;
            if (kd < 481) return 36;
            if (kd < 531) return 40;
            if (kd < 601) return 45;
            if (kd < 671) return 50;
            if (kd < 711) return 55;
            if (kd < 801) return 60;
            return 70;
        }

        private static string STol(double s)
        {
            if (s < 31) return "± 0.260";
            if (s < 51) return "± 0.310";
            return "± 0.370";
        }

        private static string FlatnessRunout(double kd)
        {
            if (kd < 51) return "0.04";
            if (kd < 121) return "0.05";
            if (kd < 251) return "0.06";
            if (kd < 316) return "0.07";
            if (kd < 401) return "0.08";
            if (kd < 501) return "0.09";
            if (kd < 631) return "0.10";
            if (kd < 801) return "0.12";
            if (kd < 1001) return "0.14";
            return "0.16";
        }

        private static double ComputeD4(int serie, double kd, double ta)
        {
            if (serie == 30)
            {
                if (Math.Abs(ta - 9) < 0.001) return kd + 9;
                if (Math.Abs(ta - 10) < 0.001) return kd + 13;
                if (Math.Abs(ta - 12) < 0.001) return kd + 16;
                if (Math.Abs(ta - 13) < 0.001) return kd + 15;
                if (Math.Abs(ta - 14) < 0.001) return kd + 19;
                if (Math.Abs(ta - 15) < 0.001) return kd + 23;
                if (Math.Abs(ta - 20) < 0.001)
                {
                    if (kd < 531) return kd + 28;
                    if (kd < 561) return kd + 23;
                    if (kd < 631) return kd + 28;
                    return kd + 33;
                }
                if (kd < 801) return kd + 32;
                if (kd < 901) return kd + 37;
                if (kd < 951) return kd + 35;
                return kd + 40;
            }

            if (Math.Abs(ta - 10) < 0.001) return kd + 18;
            if (Math.Abs(ta - 12) < 0.001) return kd < 281 ? kd + 21 : kd + 26;
            if (Math.Abs(ta - 15) < 0.001) return kd + 33;
            if (Math.Abs(ta - 18) < 0.001) return kd < 381 ? kd + 35 : kd + 40;
            if (Math.Abs(ta - 20) < 0.001) return kd < 461 ? kd + 38 : kd + 48;
            if (Math.Abs(ta - 23) < 0.001) return kd < 501 ? kd + 40 : kd + 45;
            if (Math.Abs(ta - 25) < 0.001) return kd + 48;
            if (Math.Abs(ta - 28) < 0.001) return kd < 631 ? kd + 55 : kd + 60;
            if (Math.Abs(ta - 30) < 0.001) return kd + 62;
            if (Math.Abs(ta - 34) < 0.001) return kd + 63;
            if (kd < 851) return kd + 64;
            if (kd < 901) return kd + 69;
            if (kd < 951) return kd + 67;
            return kd + 77;
        }

        private static double D4TolValue(double d4)
        {
            if (d4 < 6.01) return 0.100;
            if (d4 < 30.01) return 0.200;
            if (d4 < 120.01) return 0.300;
            if (d4 < 400.01) return 0.500;
            if (d4 < 1000.01) return 0.800;
            if (d4 < 2000.01) return 1.200;
            return 2.000;
        }

        private static double ComputeU(int serie, double kd)
        {
            if (serie == 30)
            {
                if (kd < 221) return 6;
                if (kd < 361) return 8;
                if (kd < 421) return 10;
                if (kd < 501) return 12;
                if (kd < 801) return 16;
                return 20;
            }

            if (kd < 241) return 8;
            if (kd < 321) return 10;
            if (kd < 381) return 12;
            if (kd < 501) return 16;
            if (kd < 671) return 20;
            return 24;
        }

        private static double ComputeV(int serie, double kd, double u)
        {
            if (serie == 30)
            {
                if (Math.Abs(u - 6) < 0.001) return 12;
                if (Math.Abs(u - 8) < 0.001) return kd < 301 ? 17 : 16;
                if (Math.Abs(u - 10) < 0.001) return 20;
                if (Math.Abs(u - 12) < 0.001) return 23;
                if (Math.Abs(u - 16) < 0.001) return 26;
                return 37;
            }

            if (Math.Abs(u - 8) < 0.001) return 17;
            if (Math.Abs(u - 10) < 0.001) return kd < 301 ? 21 : 20;
            if (Math.Abs(u - 12) < 0.001) return 23;
            if (Math.Abs(u - 16) < 0.001) return 28;
            if (Math.Abs(u - 20) < 0.001) return 37;
            return 47;
        }

        private static double H11Tol(double d)
        {
            if (d < 3.01) return 0.060;
            if (d < 6.01) return 0.075;
            if (d < 10.01) return 0.090;
            if (d < 18.01) return 0.110;
            if (d < 30.01) return 0.130;
            if (d < 50.01) return 0.160;
            if (d < 80.01) return 0.190;
            if (d < 120.01) return 0.220;
            if (d < 180.01) return 0.250;
            if (d < 250.01) return 0.290;
            if (d < 315.01) return 0.320;
            if (d < 400.01) return 0.360;
            if (d < 500.01) return 0.400;
            if (d < 630.01) return 0.440;
            if (d < 800.01) return 0.500;
            if (d < 1000.01) return 0.560;
            if (d < 1250.01) return 0.660;
            if (d < 1600.01) return 0.780;
            if (d < 2000.01) return 0.920;
            if (d < 2500.01) return 1.100;
            return 1.350;
        }

        private static double Round2(double value)
        {
            return Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }

        private static string Fmt(double value)
        {
            return value.ToString("0.###", CommonFunctions.Culture).Replace(",", ".");
        }

        private static string Fmt3(double value)
        {
            return value.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
        }
    }
}
