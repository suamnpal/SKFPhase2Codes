using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class Dummy_HUS_SNL01_CELLER : ITemplateCalculations
    {
        private static readonly string[] AllKeys = new[]
        {
            "SumAF1_1","SumAF1_2","SumAF1_3","SumAF1_4","SumAF1_5","SumAF1_6","SumAF1_7",
            "SumAF2_1","SumAF2_2","SumAF2_3","SumAF2_4","SumAF2_5","SumAF2_6","SumAF2_7","SumAF2_8","SumAF2_9","SumAF2_10","SumAF2_11","SumAF2_12","SumAF2_13",
            "SumAd","SumAdTol","SumAdTolN",
            "SumArbInstGjgS1","SumArbInstGjgS2",
            "SumBd","SumBdTol","SumBdTolN",
            "SumD1_1","SumD1_2","SumD1_3","SumD1_4","SumD1_5","SumD1_6","SumD1_7",
            "SumD2_1","SumD2_2","SumD2_3","SumD2_4","SumD2_5","SumD2_6","SumD2_7","SumD2_8","SumD2_9","SumD2_10","SumD2_11","SumD2_12","SumD2_13",
            "SumF",
            "SumF1_1","SumF1_2","SumF1_3","SumF1_4","SumF1_5","SumF1_6","SumF1_7",
            "SumF2_1","SumF2_2","SumF2_3","SumF2_4","SumF2_5","SumF2_6","SumF2_7","SumF2_8","SumF2_9","SumF2_10","SumF2_11","SumF2_12","SumF2_13",
            "SumFTol","SumFTolN",
            "SumFb","SumFbTol",
            "SumFh","SumFhTol",
            "SumG","SumG1",
            "SumHd","SumHdTol",
            "SumKvStPlS1","SumKvStPlS2",
            "SumLb","SumLbTol","SumLbTolN",
            "SumLd","SumLdTol","SumLdTolN",
            "SumMaskinValS1","SumMaskinValS2",
            "SumPb","SumPbTol","SumPbTolN",
            "SumPd","SumPdTol","SumPdTolN",
            "SumPl","SumPl2",
            "SumPrdritS1","SumPrdritS2",
            "SumRa32","SumRa32a","SumRa63",
            "SumRp8",
            "SumS1","SumS2",
            "SumSd","SumSdTol","SumSdTolN",
            "SumSkr",
            "SumSs","SumSsTol","SumSsTolN",
            "SumSu","SumSuTol","SumSuTolN",
            "SumTextS1","SumTextS2",
            "SumUb","SumUbTol","SumUbTolN",
            "SumUb1","SumUb1Tol","SumUb1TolN",
            "SumUb2","SumUb2Tol","SumUb2TolN",
            "SumUd","SumUdTol","SumUdTolN",
            "SumUh","SumUhTol",
            "SumVh","SumVhTol",
            "SumWt20","SumWt35",
            "Sumab",
            "VaLPopUp","VaLTypLista"
        };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            List<Bookmark> bm = req?.Bookmarks;
            var kv = new Dictionary<string, string>();

            for (int i = 0; i < AllKeys.Length; i++) kv[AllKeys[i]] = "";

            kv["SumSkr"] = GetString(bm, "SumSkr");

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = (subject ?? string.Empty).Trim().ToUpperInvariant();
            string tmpBet = tmpFormat.Replace(".", ",");

            string[] tokens = SplitTokens(tmpBet);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : string.Empty;
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : string.Empty;
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : string.Empty;
            string tmpBet4 = tokens.Length > 3 ? tokens[3] : string.Empty;
            string tmpBet5 = tokens.Length > 4 ? tokens[4] : string.Empty;

            int tmpCountB2 = (tmpBet2 ?? string.Empty).Length;

            bool tmpSNLN = ContainsI(tmpBet, "SNLN");

            string tmpSerieStr = tmpCountB2 > 3 ? Left(tmpBet2, 2) : Left(tmpBet2, 1);
            int tmpSerie = ParseIntSafe(tmpSerieStr);

            string tmpTypStr = Right(tmpBet2, 2);
            int tmpTyp = ParseIntSafe(tmpTypStr);

            int tmpTypLista;
            if (tmpSNLN)
                tmpTypLista = MemberIndex(tmpTypStr, new[] { "24", "26", "28", "30", "32", "34", "36", "38" });
            else if (tmpSerie == 30)
                tmpTypLista = MemberIndex(tmpTypStr, new[] { "36", "38" });
            else if (tmpSerie == 31)
                tmpTypLista = MemberIndex(tmpTypStr, new[] { "34", "36" });
            else if (tmpSerie == 2)
                tmpTypLista = MemberIndex(tmpTypStr, new[] { "16", "17", "18" });
            else
                tmpTypLista = MemberIndex(tmpTypStr, new[] { "16", "17", "18", "19", "20", "22", "24", "26", "28", "30", "32" });

            kv["VaLTypLista"] = tmpTypLista == 0
                ? "Produktbeteckningen ingår ej i mallen, kontrollera stavning och/eller kontakta Qe-Admin för att lägga till produkten"
                : "";

            const string Ref3 = " [3]";
            const string Ref2 = " [2]";
            const string Ref2D = " [2D]";

            string tmpRa32 = "3.2";
            string tmpRa63 = "6.3";
            kv["SumRa32"] = tmpRa32;
            kv["SumRa32a"] = tmpRa32;
            kv["SumRa63"] = tmpRa63 + Ref3;

            string tmpRp8 = "8";
            kv["SumRp8"] = "Rp " + tmpRp8;

            double tmpWt35 = 35;
            kv["SumWt35"] = "Wt " + FormatDot(tmpWt35);
            double tmpWt20 = 20;
            kv["SumWt20"] = "Wt " + FormatDot(tmpWt20);

            double tmpAd = SelectByIndex(
                tmpSNLN
                    ? new[] { 157.5, 167.5, 177.5, 192.5, 202.5, 212.5, 222.5, 232.5 }
                    : (tmpSerie == 30 ? new[] { 181.2, 191.4 }
                    : (tmpSerie == 31 ? new[] { 171.2, 181.2 }
                    : (tmpSerie == 2 ? new[] { 108.0, 112.0, 120.0 }
                    : new[] { 92.5, 97.5, 102.5, 131.0, 137.5, 147.5, 157.5, 167.5, 177.5, 192.5, 202.5 }))),
                tmpTypLista);

            kv["SumAd"] = "(Ad) " + FormatDot(tmpAd);
            double tmpAdTol = H12Tol(tmpAd);
            kv["SumAdTol"] = "+ " + FormatDot3(tmpAdTol) + Ref3;
            kv["SumAdTolN"] = "- " + FormatDot0(0) + Ref3;

            double tmpPd = (!tmpSNLN && (tmpSerie == 30 || tmpSerie == 31))
                ? tmpAd + 24.0
                : (tmpTyp < 19 ? tmpAd + 8.5 : tmpAd + 10.0);

            kv["SumPd"] = "(Pd) " + FormatDot(tmpPd);
            double tmpPdTol = H12Tol(tmpPd);
            kv["SumPdTol"] = "+ " + FormatDot3(tmpPdTol) + Ref3;
            kv["SumPdTolN"] = "- " + FormatDot0(0) + Ref3;

            double tmpLd = SelectByIndex(
                tmpSNLN
                    ? new[] { 180.0, 200.0, 210.0, 225.0, 240.0, 260.0, 280.0, 290.0 }
                    : (tmpSerie == 30 ? new[] { 280.0, 290.0 }
                    : (tmpSerie == 31 ? new[] { 280.0, 300.0 }
                    : new[] { 140.0, 150.0, 160.0, 170.0, 180.0, 200.0, 215.0, 230.0, 250.0, 270.0, 290.0 })),
                tmpTypLista);

            kv["SumLd"] = "(Ld) " + FormatDot(tmpLd);
            double tmpLdTol = LdTolPos(tmpLd);
            kv["SumLdTol"] = "+ " + FormatDot3(tmpLdTol) + Ref3;
            double tmpLdTolN = LdTolNeg(tmpLd);
            kv["SumLdTolN"] = "+ " + FormatDot3(tmpLdTolN) + Ref2D;

            double tmpLb = SelectByIndex(
                tmpSNLN
                    ? new[] { 70.0, 79.0, 79.0, 86.0, 90.0, 87.0, 94.0, 95.0 }
                    : (tmpSerie == 30 ? new[] { 108.0, 115.0 }
                    : (tmpSerie == 31 ? new[] { 108.0, 116.0 }
                    : new[] { 58.0, 61.0, 65.0, 68.0, 70.0, 80.0, 86.0, 90.0, 98.0, 106.0, 114.0 })),
                tmpTypLista);

            kv["SumLb"] = "(Lb) " + FormatDot(tmpLb);
            double tmpLbTol = LbTol(tmpLb);
            kv["SumLbTol"] = "+ " + FormatDot3(tmpLbTol) + Ref3;
            kv["SumLbTolN"] = "+ " + FormatDot0(0) + Ref2;

            bool tmpVZTyp = (!tmpSNLN) && (EqualsI(tmpBet2, "3038") || EqualsI(tmpBet2, "3136") || EqualsI(tmpBet2, "3036") || EqualsI(tmpBet2, "3134"));

            int tmpab = tmpVZTyp
                ? ((EqualsI(tmpBet2, "3038") || EqualsI(tmpBet2, "3136")) ? 15
                    : ((EqualsI(tmpBet2, "3036") || EqualsI(tmpBet2, "3134")) ? 14 : 0))
                : 0;

            kv["Sumab"] = tmpab == 0 ? " *" : "(ab) " + tmpab.ToString(CultureInfo.InvariantCulture) + "+a";

            double tmpUd = 0;
            if (tmpVZTyp)
            {
                double[] udList = tmpSerie == 30 ? new[] { 196.4, 206.4 } : new[] { 186.4, 196.4 };
                tmpUd = SelectByIndex(udList, tmpTypLista);
            }

            kv["SumUd"] = tmpUd == 0 ? "Endast vissa typer *" : "(Ud) " + FormatDot(tmpUd);
            double tmpUdTol = UdTol(tmpUd);
            kv["SumUdTol"] = tmpUd == 0 ? "" : "+ " + FormatDot3(tmpUdTol) + Ref3;
            kv["SumUdTolN"] = tmpUd == 0 ? "" : "+ " + FormatDot3(0) + Ref3;

            double tmpS = tmpUd == 0 ? 1.5 : 2.0;
            kv["SumS1"] = tmpUd == 0 ? FormatCommaF1(tmpS) : "";
            kv["SumS2"] = tmpUd == 0 ? "" : FormatCommaF1(tmpS);

            double tmpUb = 11.0;
            kv["SumUb"] = tmpVZTyp ? "(Ub) " + FormatDot(tmpUb) : " *";
            double tmpUbTol = UbTol(tmpUb);
            kv["SumUbTol"] = tmpVZTyp ? "+ " + FormatDot3(tmpUbTol) + Ref3 : "";
            kv["SumUbTolN"] = tmpVZTyp ? "- " + FormatDot0(0) + Ref3 : "";

            double tmpUb1 = 5.5;
            kv["SumUb1"] = tmpVZTyp ? "(Ub1) " + FormatDot(tmpUb1) : " *";
            kv["SumUb1Tol"] = tmpVZTyp ? "+ " + FormatDot0(0) + Ref3 : "";
            kv["SumUb1TolN"] = tmpVZTyp ? "- " + FormatDot3(0.5) + Ref3 : "";

            double tmpUb2 = 22.0;
            kv["SumUb2"] = tmpVZTyp ? "(Ub2) " + FormatDot(tmpUb2) : " *";
            kv["SumUb2Tol"] = tmpVZTyp ? "+ " + FormatDot0(0) + Ref3 : "";
            double tmpUb2TolN = Ub2TolNeg(tmpUb2);
            kv["SumUb2TolN"] = tmpUd == 0 ? "" : "- " + FormatDot3(tmpUb2TolN) + Ref3;

            double tmpPb = (!tmpSNLN && (tmpSerie == 30 || tmpSerie == 31)) ? 11.0 : (tmpTyp < 19 ? 5.0 : 6.0);
            kv["SumPb"] = tmpVZTyp ? " " : "2x (Pb) " + FormatDot(tmpPb);
            double tmpPbTol = PbTol(tmpPb);
            kv["SumPbTol"] = tmpVZTyp ? " " : "+ " + FormatDot3(tmpPbTol) + Ref3;
            kv["SumPbTolN"] = tmpVZTyp ? " " : "- " + FormatDot0(0) + Ref3;

            int tmpG1 = (!tmpSNLN && (tmpSerie == 2 || tmpSerie == 5))
                ? (tmpTyp < 18 ? 12 : (tmpTyp < 20 ? 16 : (tmpTyp < 26 ? 20 : 24)))
                : (tmpTyp < 20 ? 16 : (tmpTyp < 30 ? 20 : 24));

            kv["SumG1"] = "(G1) M" + tmpG1.ToString(CultureInfo.InvariantCulture) + " 6H" + Ref3;

            int tmpGd = (!tmpSNLN && (tmpSerie == 30 || tmpSerie == 31)) ? 12 : 10;
            kv["SumG"] = "(G) 1/8 - 27NPSF";

            double tmpFb = SelectByIndex(
                tmpSNLN
                    ? new[] { 33.0, 39.0, 40.0, 45.0, 45.0, 51.0, 51.0, 48.0 }
                    : ((!tmpSNLN && (tmpSerie == 30 || tmpSerie == 31)) ? new[] { 1.0, 1.0 } : new[] { 27.0, 26.0, 31.0, 35.0, 39.0, 45.0, 47.0, 51.0, 58.0, 57.0, 57.0 }),
                tmpTypLista);

            bool isSideBore = Math.Abs(tmpFb - 1.0) < 0.0000001;
            kv["SumFb"] = isSideBore ? "Sidoborrhål" : "(Fb) " + FormatDot(tmpFb);
            kv["SumFbTol"] = isSideBore ? "" : "± " + FormatDot3(0.5);

            double tmpHd = SelectByIndex(
                tmpSNLN
                    ? new[] { 211.0, 230.0, 245.0, 265.0, 275.0, 310.0, 332.0, 332.0 }
                    : ((!tmpSNLN && (tmpSerie == 30 || tmpSerie == 31)) ? new[] { 315.0, 335.0 } : new[] { 165.0, 175.0, 185.0, 195.0, 211.0, 230.0, 245.0, 265.0, 290.0, 310.0, 332.0 }),
                tmpTypLista);

            kv["SumHd"] = "(Hd) " + FormatDot(tmpHd);
            double tmpHdTol = HdTol(tmpHd);
            kv["SumHdTol"] = "± " + FormatDot3(tmpHdTol) + Ref3;

            double tmpBd = SelectByIndex(
                tmpSNLN
                    ? new[] { 22.0, 22.0, 22.0, 26.0, 26.0, 26.0, 26.0, 26.0 }
                    : (tmpSerie == 30 ? new[] { 27.0, 27.0 }
                    : (tmpSerie == 31 ? new[] { 27.0, 27.0 }
                    : new[] { 13.5, 13.5, 17.5, 17.5, 22.0, 22.0, 22.0, 26.0, 26.0, 26.0, 26.0 })),
                tmpTypLista);

            kv["SumBd"] = "(Bd) " + FormatDot(tmpBd);
            double tmpBdTol = BdTolPos(tmpBd);
            kv["SumBdTol"] = "+ " + FormatDot3(tmpBdTol) + Ref3;
            kv["SumBdTolN"] = "- " + FormatDot0(0) + Ref3;

            double tmpF = tmpBd + 1.0;
            kv["SumF"] = "(F) " + FormatDot(tmpF);
            double tmpFTol = (!tmpSNLN && (tmpSerie == 30 || tmpSerie == 31)) ? 0.84 : 0.5;
            double tmpFTolN = (!tmpSNLN && (tmpSerie == 30 || tmpSerie == 31)) ? 0.0 : 0.5;
            kv["SumFTol"] = "+ " + FormatDot3(tmpFTol);
            kv["SumFTolN"] = "- " + FormatDot3(tmpFTolN);

            double tmpSd = tmpTyp < 18 ? 5.0 : 9.335;
            kv["SumSd"] = "2x (Sd) " + FormatDot(tmpSd);
            string tmpSdTol = tmpTyp < 18 ? FormatF2Dot(0.03) : FormatDot3(0.036);
            kv["SumSdTol"] = "+ " + tmpSdTol + Ref2;
            kv["SumSdTolN"] = "- " + FormatDot0(0) + Ref2;

            double tmpSo = tmpTyp < 18 ? 7.0 : 10.0;
            kv["SumSu"] = "";
            kv["SumSuTol"] = "";
            kv["SumSuTolN"] = "";
            kv["SumUh"] = "";
            kv["SumUhTol"] = "";
            kv["SumFh"] = "";
            kv["SumFhTol"] = "";
            kv["SumVh"] = "";
            kv["SumVhTol"] = "";
            kv["SumSs"] = "";
            kv["SumSsTol"] = "";
            kv["SumSsTolN"] = "";
            kv["SumPl"] = "";
            kv["SumPl2"] = "";

            kv["SumSu"] = "(Su) " + FormatDot(tmpTyp < 18 ? 7.7 : 10.5);
            kv["SumSuTol"] = "+ " + FormatDot0(0) + Ref3;
            kv["SumSuTolN"] = "- " + FormatDot3(0.5) + Ref3;

            kv["SumUh"] = "(Uh) " + FormatDot(SelectByIndex(
                tmpSNLN
                    ? new[] { 112.0, 125.0, 140.0, 150.0, 150.0, 160.0, 170.0, 170.0 }
                    : ((!tmpSNLN && (tmpSerie == 30 || tmpSerie == 31)) ? new[] { 170.0, 180.0 } : new[] { 95.0, 95.0, 100.0, 112.0, 112.0, 125.0, 140.0, 150.0, 150.0, 160.0, 170.0 }),
                tmpTypLista));
            kv["SumUhTol"] = "± " + FormatDot3(UhTol(ExtractValue(kv["SumUh"]))) + Ref3;

            double tmpFh = SelectByIndex(
                tmpSNLN
                    ? new[] { 40.0, 45.0, 45.0, 50.0, 50.0, 60.0, 60.0, 60.0 }
                    : ((!tmpSNLN && (tmpSerie == 30 || tmpSerie == 31)) ? new[] { 70.0, 75.0 } : new[] { 32.0, 32.0, 35.0, 35.0, 40.0, 45.0, 45.0, 50.0, 50.0, 60.0, 60.0 }),
                tmpTypLista);
            kv["SumFh"] = "(Fh) " + FormatDot(tmpFh);
            kv["SumFhTol"] = "± " + (tmpTyp < 18 ? FormatDot3(0.6) : FormatF1Dot(1.0));

            double tmpVh = SelectByIndex(
                tmpSNLN
                    ? new[] { 61.0, 64.0, 68.0, 76.0, 83.0, 86.0, 87.0, 92.0 }
                    : ((!tmpSNLN && (tmpSerie == 30 || tmpSerie == 31)) ? new[] { 92.0, 92.0 } : new[] { 46.0, 51.0, 56.0, 56.0, 61.0, 64.0, 68.0, 76.0, 81.0, 86.0, 89.0 }),
                tmpTypLista);
            kv["SumVh"] = "(Vh) " + FormatDot(tmpVh);
            kv["SumVhTol"] = "± " + FormatF1Dot(1.0);

            double tmpSs = (!tmpSNLN && (tmpSerie == 30 || tmpSerie == 31)) ? 0.5 : (tmpTyp < 18 ? 0.3 : 1.25);
            kv["SumSs"] = "(Ss) " + FormatDot3(tmpSs);
            double tmpSsTol = tmpTyp < 18 ? 0.0 : 0.3;
            kv["SumSsTol"] = "+ " + (tmpTyp < 18 ? FormatDot0(0) : FormatDot3(tmpSsTol));
            double tmpSsTolN = (!tmpSNLN && (EqualsI(tmpBet2, "3036") || EqualsI(tmpBet2, "3134") || EqualsI(tmpBet2, "3038") || EqualsI(tmpBet2, "3136")))
                ? 0.0
                : (tmpTyp < 18 ? 0.1 : 0.4);
            string ssSuffix = (!tmpSNLN && (tmpSerie == 30 || tmpSerie == 31)) ? "" : Ref2;
            kv["SumSsTolN"] = "- " + (tmpSsTolN == 0.0 ? FormatDot0(0) : (tmpTyp < 18 ? FormatDot3(0.1) : FormatDot3(0.4))) + ssSuffix;

            double tmpPl = (!tmpSNLN && (tmpSerie == 30 || tmpSerie == 31)) ? 0.05 : (tmpTyp < 18 ? 0.05 : 0.1);
            kv["SumPl"] = FormatF2Dot(tmpPl);
            double tmpPl2 = (!tmpSNLN && (tmpSerie == 30 || tmpSerie == 31)) ? 0.05 : (tmpTyp < 18 ? 0.08 : 0.1);
            kv["SumPl2"] = FormatF2Dot(tmpPl2);

            string tmpMaskinVal = EqualsI(maskinVal, "Celler") ? "" : "INGET MASKINVAL GJORD";
            string tmpMaskinValS1 = EqualsI(maskinVal, "Celler") ? "Celler" : "";
            kv["SumMaskinValS1"] = "Maskin: " + tmpMaskinValS1 + tmpMaskinVal + " - Svarvning";
            string tmpMaskinValS2 = EqualsI(maskinVal, "Celler") ? "Celler" : "";
            kv["SumMaskinValS2"] = "Maskin: " + tmpMaskinValS2 + tmpMaskinVal + " - Borrning, fräsning";

            bool isCeller = EqualsI(maskinVal, "Celler");

            kv["SumF1_1"] = isCeller ? "1/tim" : "";
            kv["SumF1_2"] = isCeller ? "1/tim" : "";
            kv["SumF1_3"] = isCeller ? "2/Skift" : "";
            kv["SumF1_4"] = isCeller ? "1/Skift & inst." : "";
            kv["SumF1_5"] = isCeller ? "1/Skift & inst." : "";
            kv["SumF1_6"] = isCeller ? "Inst." : "";
            kv["SumF1_7"] = isCeller ? "2/Skift & inst." : "";

            kv["SumF2_1"] = isCeller ? "2/Skift & inst." : "";
            kv["SumF2_2"] = isCeller ? "2/Skift & inst." : "";
            kv["SumF2_3"] = isCeller ? (isSideBore ? "" : "Inst.") : "";
            kv["SumF2_4"] = isCeller ? "Inst." : "";
            kv["SumF2_5"] = isCeller ? "Inst." : "";
            kv["SumF2_6"] = isCeller ? "Inst." : "";
            kv["SumF2_7"] = isCeller ? "Inst." : "";
            kv["SumF2_8"] = isCeller ? "Inst." : "";
            kv["SumF2_9"] = isCeller ? "Inst." : "";
            kv["SumF2_10"] = isCeller ? "Inst." : "";
            kv["SumF2_11"] = isCeller ? "Inst." : "";
            kv["SumF2_12"] = isCeller ? "Inst." : "";
            kv["SumF2_13"] = isCeller ? "2/Skift & inst." : "";

            kv["SumD1_1"] = isCeller ? "Skjutmått/Tolk" : "";
            kv["SumD1_2"] = isCeller ? "Skjutmått" : "";
            kv["SumD1_3"] = isCeller ? "Mätmaskin" : "";
            kv["SumD1_4"] = isCeller ? "Skjutmått/Tolk" : "";
            kv["SumD1_5"] = isCeller ? "Skjutmått/Tolk" : "";
            kv["SumD1_6"] = isCeller ? "Skjutmått" : "";
            kv["SumD1_7"] = isCeller ? "Skjutmått" : "";

            kv["SumD2_1"] = isCeller ? "Gängtolk" : "";
            kv["SumD2_2"] = isCeller ? "Gängtolk/skjutmått" : "";
            kv["SumD2_3"] = isCeller ? (isSideBore ? "" : "Skjutmått/Okulärt") : "";
            kv["SumD2_4"] = isCeller ? "Skjutmått/Okulärt" : "";
            kv["SumD2_5"] = isCeller ? "Skjutmått" : "";
            kv["SumD2_6"] = isCeller ? "Skjutmått/Tolk" : "";
            kv["SumD2_7"] = isCeller ? "Skjutmått" : "";
            kv["SumD2_8"] = isCeller ? "Skjutmått" : "";
            kv["SumD2_9"] = isCeller ? "Skjutmått" : "";
            kv["SumD2_10"] = isCeller ? "Skjutmått" : "";
            kv["SumD2_11"] = isCeller ? "Kännbleck" : "";
            kv["SumD2_12"] = isCeller ? "Skjutmått" : "";
            kv["SumD2_13"] = isCeller ? "Mätmaskin" : "";

            kv["SumAF1_1"] = "";
            kv["SumAF1_2"] = isCeller ? "(Ud) endast serie 31" : "";
            kv["SumAF1_3"] = isCeller ? "10 hus irad cell1" : "";
            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = "";
            kv["SumAF1_6"] = "";
            kv["SumAF1_7"] = isCeller ? "Mätes på UH vid hopl.yta" : "";

            kv["SumAF2_1"] = isCeller ? "Samtliga TP1-maskin 1,2" : "";
            kv["SumAF2_2"] = isCeller ? ("Min " + tmpGd.ToString(CultureInfo.InvariantCulture) + " gängor / " + tmpGd.ToString(CultureInfo.InvariantCulture) + "mm, Maskin 1,2") : "";
            kv["SumAF2_3"] = isCeller ? (isSideBore ? "Sidoborrhål" : "") : "";
            kv["SumAF2_4"] = "";
            kv["SumAF2_5"] = "";
            kv["SumAF2_6"] = "";
            kv["SumAF2_7"] = "";
            kv["SumAF2_8"] = "";
            kv["SumAF2_9"] = "";
            kv["SumAF2_10"] = "";
            kv["SumAF2_11"] = isCeller ? "Mått: 0.05 enligt Tb" : "";
            kv["SumAF2_12"] = "";
            kv["SumAF2_13"] = isCeller ? "Från TP1 maskin 1, 2, " : "";

            string sumTextS1 = "Kontrolleras enl. styrplan";
            kv["SumTextS1"] = sumTextS1;
            kv["SumTextS2"] = sumTextS1 + "\n";

            string sumPrdritS1 = "Produktritning: " + tmpBet;
            kv["SumPrdritS1"] = sumPrdritS1;
            kv["SumPrdritS2"] = sumPrdritS1;

            kv["SumArbInstGjgS1"] = "Gjutgods: A3.022";
            kv["SumArbInstGjgS2"] = kv["SumArbInstGjgS1"];

            kv["SumKvStPlS1"] = "Kvalitetstyrning: K1.07-17";
            kv["SumKvStPlS2"] = kv["SumKvStPlS1"];

            kv["SumSö"] = "(Sö) " + FormatDot(tmpSo);
            kv["SumSöTol"] = "± " + FormatDot3(0.2);

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
                       " dagar)\n\n\n\nInformation om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument\n\n\n\nPopupruta aktiv till " +
                       validTill.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return "";
        }

        private static string GetString(List<Bookmark> bm, string key)
        {
            if (bm == null || string.IsNullOrWhiteSpace(key))
                return "";
            for (int i = 0; i < bm.Count; i++)
            {
                var b = bm[i];
                if (b != null && string.Equals(b.BookmarkName ?? "", key, StringComparison.OrdinalIgnoreCase))
                    return b.BookmarkValue ?? "";
            }
            return "";
        }

        private static string[] SplitTokens(string s)
        {
            if (string.IsNullOrEmpty(s)) return Array.Empty<string>();
            return s.Split(new[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static int MemberIndex(string value, string[] list)
        {
            if (list == null) return 0;
            for (int i = 0; i < list.Length; i++)
                if (string.Equals(list[i], value ?? "", StringComparison.OrdinalIgnoreCase))
                    return i + 1;
            return 0;
        }

        private static double SelectByIndex(double[] list, int oneBasedIndex)
        {
            if (list == null) return 0;
            int idx = oneBasedIndex - 1;
            if (idx < 0 || idx >= list.Length) return 0;
            return list[idx];
        }

        private static bool EqualsI(string a, string b)
            => string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);

        private static bool ContainsI(string haystack, string needle)
        {
            if (string.IsNullOrEmpty(haystack) || string.IsNullOrEmpty(needle)) return false;
            return haystack.IndexOf(needle, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static int ParseIntSafe(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;
            int v;
            return int.TryParse(s.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static string Left(string s, int n)
        {
            if (string.IsNullOrEmpty(s) || n <= 0) return "";
            return s.Length <= n ? s : s.Substring(0, n);
        }

        private static string Right(string s, int n)
        {
            if (string.IsNullOrEmpty(s) || n <= 0) return "";
            return s.Length <= n ? s : s.Substring(s.Length - n, n);
        }

        private static string FormatDot(double v)
            => v.ToString(CommonFunctions.Culture).Replace(",", ".");

        private static string FormatDot3(double v)
            => v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");

        private static string FormatDot0(double v)
            => v.ToString("F0", CommonFunctions.Culture).Replace(",", ".");

        private static string FormatF2Dot(double v)
            => v.ToString("F2", CommonFunctions.Culture).Replace(",", ".");

        private static string FormatF1Dot(double v)
            => v.ToString("F1", CommonFunctions.Culture).Replace(",", ".");

        private static string FormatCommaF1(double v)
            => v.ToString("F1", CommonFunctions.Culture);

        private static double H12Tol(double d)
        {
            if (d < 120.01) return 0.350;
            if (d < 180.01) return 0.400;
            if (d < 250.01) return 0.460;
            return 0.520;
        }

        private static double LdTolPos(double d)
        {
            if (d < 120.01) return 0.047;
            if (d < 180.01) return 0.054;
            if (d < 250.01) return 0.061;
            if (d < 315.01) return 0.069;
            return 0.075;
        }

        private static double LdTolNeg(double d)
        {
            if (d < 120.01) return 0.012;
            if (d < 180.01) return 0.014;
            if (d < 250.01) return 0.015;
            if (d < 315.01) return 0.017;
            return 0.018;
        }

        private static double LbTol(double d)
        {
            if (d < 50.01) return 0.250;
            if (d < 80.01) return 0.300;
            if (d < 120.01) return 0.350;
            return 0.400;
        }

        private static double UdTol(double d)
        {
            if (d <= 0) return 0;
            if (d < 3.01) return 0.100;
            if (d < 6.01) return 0.120;
            if (d < 10.01) return 0.150;
            if (d < 18.01) return 0.180;
            if (d < 30.01) return 0.210;
            if (d < 50.01) return 0.250;
            if (d < 80.01) return 0.300;
            if (d < 120.01) return 0.350;
            if (d < 180.01) return 0.400;
            if (d < 250.01) return 0.460;
            if (d < 315.01) return 0.520;
            if (d < 400.01) return 0.570;
            if (d < 500.01) return 0.630;
            if (d < 630.01) return 0.700;
            if (d < 800.01) return 0.800;
            if (d < 1000.01) return 0.900;
            if (d < 1250.01) return 1.050;
            if (d < 1600.01) return 1.250;
            if (d < 2000.01) return 1.500;
            if (d < 2500.01) return 1.750;
            return 2.100;
        }

        private static double UbTol(double d)
        {
            if (d < 3.01) return 0.140;
            if (d < 6.01) return 0.180;
            if (d < 10.01) return 0.220;
            if (d < 18.01) return 0.270;
            if (d < 30.01) return 0.330;
            return 0.390;
        }

        private static double Ub2TolNeg(double d)
        {
            if (d < 3.01) return 0.400;
            if (d < 6.01) return 0.480;
            if (d < 10.01) return 0.580;
            if (d < 18.01) return 0.700;
            if (d < 30.01) return 0.840;
            if (d < 50.01) return 1.000;
            return 1.200;
        }

        private static double PbTol(double d)
        {
            if (d < 3.01) return 0.140;
            if (d < 6.01) return 0.480;
            if (d < 10.01) return 0.220;
            if (d < 18.01) return 0.270;
            if (d < 30.01) return 0.330;
            if (d < 50.01) return 0.390;
            if (d < 80.01) return 0.460;
            if (d < 120.01) return 0.540;
            if (d < 180.01) return 0.630;
            if (d < 250.01) return 0.720;
            if (d < 315.01) return 0.810;
            if (d < 400.01) return 0.890;
            if (d < 500.01) return 0.970;
            if (d < 630.01) return 1.100;
            if (d < 800.01) return 1.250;
            if (d < 1000.01) return 1.400;
            if (d < 1250.01) return 1.650;
            if (d < 1600.01) return 1.950;
            if (d < 2000.01) return 2.300;
            if (d < 2500.01) return 2.800;
            return 3.300;
        }

        private static double HdTol(double d)
        {
            if (d < 3.01) return 0.070;
            if (d < 6.01) return 0.090;
            if (d < 10.01) return 0.110;
            if (d < 18.01) return 0.135;
            if (d < 30.01) return 0.165;
            if (d < 50.01) return 0.195;
            if (d < 80.01) return 0.230;
            if (d < 120.01) return 0.270;
            if (d < 180.01) return 0.315;
            if (d < 250.01) return 0.360;
            if (d < 315.01) return 0.405;
            if (d < 400.01) return 0.445;
            if (d < 500.01) return 0.485;
            if (d < 630.01) return 0.550;
            if (d < 800.01) return 0.625;
            if (d < 1000.01) return 0.700;
            if (d < 1250.01) return 0.825;
            if (d < 1600.01) return 0.975;
            if (d < 2000.01) return 1.150;
            if (d < 2500.01) return 1.400;
            return 1.650;
        }

        private static double BdTolPos(double d)
        {
            if (d < 10.01) return 0.580;
            if (d < 18.01) return 0.700;
            if (d < 30.01) return 0.840;
            return 1.000;
        }

        private static double UhTol(double d)
        {
            if (d < 80.01) return 0.095;
            if (d < 120.01) return 0.110;
            if (d < 180.01) return 0.125;
            if (d < 250.01) return 0.145;
            return 0.160;
        }

        private static double ExtractValue(string labeled)
        {
            if (string.IsNullOrWhiteSpace(labeled)) return 0;
            int idx = labeled.IndexOf(')');
            if (idx < 0 || idx + 1 >= labeled.Length) return 0;
            string rest = labeled.Substring(idx + 1).Trim();
            double v;
            string norm = rest.Replace(",", ".");
            return double.TryParse(norm, NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }
    }
}