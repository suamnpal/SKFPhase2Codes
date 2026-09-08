using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_CG_serie_292_CGI : ITemplateCalculations
    {
        private const string LB = "<<LineBreak>>";

        private static readonly string[] Machines =
        {
            "LB45", "MaxMuller", "Nakamura",
            "VTR-160", "MacTurn 550", "1150"
        };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req?.ProductDesignation ?? "";
            string maskinVal = req?.MachineNumber ?? "";

            kv["VaLPopUp"] = ComputePopup(req?.Published);

            string tmpFormat = subject.Trim().ToUpperInvariant();
            string[] cgiParts = Split(tmpFormat, " /.");

            string tmpBetCGI1 = Item(cgiParts, 1);
            string tmpBetCGI2 = Item(cgiParts, 2);
            string tmpBetCGI3 = Item(cgiParts, 3);
            string tmpBetCGI4 = Item(cgiParts, 4);
            string tmpBetCGI5 = Item(cgiParts, 5);

            bool tmpCGI = ContainsI(tmpFormat, "CG-I") || ContainsI(tmpFormat, "CGI");
            string tmpCGILista = tmpBetCGI2 == "202337" ? "{CG 29276:371:375}" : "{0:0:0}";
            string tmpCGIRes = ListItem(tmpCGILista, 1);

            bool tmpSlash = tmpFormat.Contains("/");
            bool tmpE = ContainsI(tmpFormat, "E");
            string tmpCGNr = tmpCGI ? tmpCGIRes : tmpFormat;

            string[] parts = Split(tmpCGNr, "- /.");

            string tmpBet1 = Item(parts, 1);
            string tmpBet2 = Item(parts, 2);
            object tmpBet3 = tmpE ? (object)Item(parts, 3) : ParseDouble(Item(parts, 3));
            object tmpBet4 = tmpE ? (object)Item(parts, 4) : ParseDouble(Item(parts, 4));
            double tmpBet5 = ParseDouble(Item(parts, 5));
            double tmpBet6 = ParseDouble(Item(parts, 6));
            double tmpBet7 = ParseDouble(Item(parts, 7));

            string tmpSerie = Left(tmpBet2, 3);
            double tmpTyp = tmpSlash ? ParseDouble(LotusText(tmpBet3)) : ParseDouble(Middle(tmpBet2, 3, 2));

            string tmpRit = "Windchill.skf.net - " + tmpFormat;

            kv["SumRitS1"] = tmpRit;
            kv["SumRitS2"] = tmpRit;

            string[] tmpTypValues =
            {
                "30","32","34","36","38","40","44","48","52","56","60","64","68","72","76","80","84","88","92","96",
                "500","530","560","600","630","670","710","750","800","850","900","950","1000","1060","1120","1180"
            };

            int tmpTypLista = Array.IndexOf(tmpTypValues,SmartInvariant(tmpTyp)) + 1;

            if (tmpTypLista < 1)
                throw new InvalidOperationException("TmpTyp '" + SmartInvariant(tmpTyp) + "' finns inte i TmpTypLista. Subject: " + subject);

            double[] tmpALista ={ 167,177,188,198,210,220,240,265,285,305,329,347,369,394,414,434,458,478,498,520,540,573,605,647, 682,720,766,808,860,913,0,1020,0,1138,0,1260};

            double tmpA = GetArrayItem(tmpALista, tmpTypLista);

            kv["SumA"] = "(A) " + Smart(tmpA);
            kv["SumATol"] = "+ 0.0";
            kv["SumATolN"] = "- " + F3(H11Tolerance(tmpA));

            double tmpBCGI = ParseDouble(ListItem(tmpCGILista, 2));
            double tmpB = tmpCGI ? tmpBCGI : !tmpSlash ? (tmpTyp / 2) * 10 : ParseDouble(LotusText(tmpBet3));

            double tmpBAvMattU =
                tmpB < 165 ? 0.28 : tmpB < 185 ? 0.31 :
                tmpB < 210 ? 0.34 : tmpB < 230 ? 0.38 :
                tmpB < 250 ? 0.42 : tmpB < 290 ? 0.48 :
                tmpB < 310 ? 0.54 : tmpB < 350 ? 0.60 :
                tmpB < 410 ? 0.68 : tmpB < 450 ? 0.76 :
                tmpB < 510 ? 0.84 : tmpB < 570 ? 0.96 :
                tmpB < 640 ? 1.05 : tmpB < 711 ? 1.20 :
                tmpB < 801 ? 1.35 : tmpB < 851 ? 1.55 :
                tmpB < 951 ? 1.70 : tmpB < 1061 ? 1.90 : 2.10;

            double tmpBAvMattO =
                tmpB < 165 ? 0.53 : tmpB < 185 ? 0.56 :
                tmpB < 210 ? 0.63 : tmpB < 230 ? 0.67 :
                tmpB < 250 ? 0.71 : tmpB < 290 ? 0.80 :
                tmpB < 310 ? 0.86 : tmpB < 350 ? 0.96 :
                tmpB < 410 ? 1.04 : tmpB < 450 ? 1.16 :
                tmpB < 510 ? 1.24 : tmpB < 570 ? (tmpE ? 1.40 : 1.39) :
                tmpB < 640 ? (tmpE ? 1.49 : 1.48) :
                tmpB < 711 ? (tmpE ? 1.70 : 1.67) :
                tmpB < 801 ? 1.85 : tmpB < 851 ? 2.11 :
                tmpB < 951 ? 2.26 : tmpB < 1061 ? 2.56 : 2.76;

            double tmpUtrakningB = tmpB + tmpBAvMattU;
            double tmpUtrakningBTol = tmpBAvMattO - tmpBAvMattU;

            kv["SumB"] = "(B) " + Smart(tmpUtrakningB);
            kv["SumBTol"] = "+ " + SmartDot(tmpUtrakningBTol);
            kv["SumBTolN"] = "- 0.0";
            kv["SumB1"] = kv["SumB"];
            kv["SumB1Tol"] = kv["SumBTol"];
            kv["SumB1TolN"] = kv["SumBTolN"];

            string tmpCLista = !tmpE ? "{22,3:22,3:23,5:23,4:0:25,7:25,7:33,2:33,2:33:42,8:42,7:42,5:50,5:47,5:47,5:54:54:54:57,4:57,4:0:64,5:68,5:75,3:79,3}"
                : "{0:0:0:0:26,5:0:0:0:0:0:0:0:0:0:0:0:0:0:0:0:0:63:0:70:71:80:83:85:87:95:0:103:0:120:0:104}";

            double tmpC = ParseDouble(ListItem(tmpCLista, tmpTypLista));
            double tmpCTolN =
                tmpTyp < 37 ? 0.25 : tmpTyp < 50 ? 0.29 :
                tmpTyp < 62 ? 0.32 : tmpTyp < 82 ? 0.36 :
                tmpTyp < 540 ? (tmpE ? 0.44 : 0.40) :
                tmpTyp < 640 ? (tmpE ? 0.44 : 0.43) :
                tmpTyp < 801 ? (tmpE ? 0.50 : 0.47) :
                tmpTyp < 1001 ? 0.56 : 0.66;

            kv["SumC"] = "(C) " + Smart(tmpC);
            kv["SumCTol"] = "+ 0.0";
            kv["SumCTolN"] = "- " + F3(tmpCTolN);

            string tmpDLista = !tmpE ? "{159:170:181:191:0:213:232:256:274:295:319:337:359:382:401:421:445:464:485:507:526:0:590:630:664:706}"
                : "{0:0:0:0:202:0:0:0:0:0:0:0:0:0:0:0:0:0:0:0:0:551:0:623:650:696:736:777:828:879:0:982:0:1096:0:1215}";

            double tmpD = ParseDouble(ListItem(tmpDLista, tmpTypLista));

            double tmpDAvMattO =
                tmpD < 160 ? 0.52 : tmpD < 171 ? 0.58 :
                tmpD < 200 ? 0.66 : tmpD < 214 ? 0.74 :
                tmpD < 233 ? 0.82 : tmpD < 275 ? 0.92 :
                tmpD < 300 ? 1.05 : tmpD < 340 ? 1.20 :
                tmpD < 383 ? 1.35 : tmpD < 446 ? 1.50 :
                tmpD < 486 ? 1.65 : tmpD < 552 ? 1.85 :
                tmpD < 631 ? 2.10 : tmpD < 651 ? 2.30 :
                tmpTyp == 670 && tmpE ? 2.30 :
                tmpD < 778 ? 2.60 : tmpD < 880 ? 3.00 :
                tmpD < 983 ? 3.30 : tmpD < 1097 ? 3.70 : 4.10;

            double tmpDAvMattU =
                tmpD < 160 ? 0.68 : tmpD < 171 ? 0.74 :
                tmpD < 200 ? 0.845 : tmpD < 214 ? 0.925 :
                tmpD < 233 ? 1.005 : tmpD < 275 ? 1.13 :
                tmpD < 300 ? 1.26 : tmpD < 340 ? 1.43 :
                tmpD < 383 ? 1.58 : tmpD < 446 ? 1.75 :
                tmpD < 486 ? 1.90 : tmpD < 552 ? (tmpE ? 2.13 : 2.12) :
                tmpD < 631 ? (tmpE ? 2.38 : 2.37) :
                tmpD < 651 ? (tmpE ? 2.62 : 2.37) :
                tmpTyp == 670 && tmpE ? 2.60 :
                tmpD < 778 ? (tmpE ? 2.92 : 2.60) :
                tmpD < 880 ? 3.36 : tmpD < 983 ? 3.66 :
                tmpD < 1097 ? 3.70 : 4.10;

            double tmpUtrakningD = tmpD - tmpDAvMattO;
            double tmpUtrakningDTol = Math.Abs(tmpDAvMattO - tmpDAvMattU);

            kv["SumD"] = "(D) " + Smart(tmpUtrakningD);
            kv["SumDTol"] = "+ 0.0";
            kv["SumDTolN"] = "- " + F3(tmpUtrakningDTol);
            
            string tmpD1Lista = !tmpE ? "{154,5:164,5:174,5:184,5:0:205:225:246:266:286:307:327:347:368:388:408:429:449:469:490:510:0:571:612:642:683}"
                : "{0:0:0:0:195:0:0:0:0:0:0:0:0:0:0:0:0:0:0:0:0:539:0:610:638:681:722:762:812:864:0:964:0:1074:0:1194}";

            double tmpD1 = ParseDouble(ListItem(tmpD1Lista, tmpTypLista));

            double tmpD1Tol =
                tmpD1 < 175 ? 0.055 :
                tmpD1 < 250 ? 0.077 :
                tmpD1 < 308 ? 0.086 :
                tmpD1 < 390 ? 0.094 :
                tmpD1 < 491 ? 0.103 :
                tmpD1 < 613 ? (tmpE ? 0.114 : 0.112) :
                tmpD1 < 763 ? (tmpE ? 0.130 : 0.125) :
                tmpD1 < 965 ? 0.146 : 0.171;

            double tmpD1TolN =
                tmpD1 < 175 ? 0.015 :
                tmpD1 < 250 ? 0.031 :
                tmpD1 < 308 ? 0.034 :
                tmpD1 < 390 ? 0.037 :
                tmpD1 < 491 ? 0.040 :
                tmpD1 < 613 ? 0.044 :
                tmpD1 < 763 ? (tmpE ? 0.050 : 0.049) :
                tmpD1 < 965 ? 0.056 : 0.066;

            double tmpUtrakningD1 = tmpD1 + tmpD1Tol;
            double tmpUtrakningD1Tol = Math.Abs(tmpD1Tol - tmpD1TolN);

            kv["SumD1"] = "(D1) " + SmartDot(tmpUtrakningD1);
            kv["SumD1Tol"] = "+ 0.0";
            kv["SumD1TolN"] = "- " + F3(tmpUtrakningD1Tol);

            string tmpFLista = !tmpE ? "{152,7:162,7:172,6:182,6:0:203:222,9:243,8:263,7:283,6:304,5:324,4:344,3:365,2:385,1:405:425,9:445,8:465,7:486,6:506,5:0:567:608:637,8:678,6}"
                : "{0:0:0:0:192,5:0:0:0:0:0:0:0:0:0:0:0:0:0:0:0:0:534:0:606:634:675:717:756,5:806:858:0:958:0:1067:0:1186}";

            double tmpF = ParseDouble(ListItem(tmpFLista, tmpTypLista));

            double tmpFTol =
                tmpF < 173 ? 0.250 :
                tmpF < 245 ? 0.290 :
                tmpF < 305 ? 0.320 :
                tmpF < 386 ? 0.360 :
                tmpF < 488 ? 0.400 :
                tmpF < 609 ? (tmpE ? 0.440 : 0.430) :
                tmpF < 760 ? (tmpE ? 0.500 : 0.470) :
                tmpF < 960 ? 0.560 : 0.660;

            kv["SumF"] = "Ø (F) " + Smart(tmpF);
            kv["SumFTol"] = "+ " + F3(tmpFTol);
            kv["SumFTolN"] = "- 0.0";

            string tmpGLista = !tmpE ? "{152,2:162,3:172,3:182,3:0:202,5:222,5:243:263:283:303,5:323,5:343,5:364:384:404:424,5:444,5:464,5:485:505:0:565,5:605,5:636:676,5}"
                : "{0:0:0:0:192:0:0:0:0:0:0:0:0:0:0:0:0:0:0:0:0:535:0:605:634:676:716:756:806:857:0:957:0:1067:0:1187}";

            double tmpG = tmpCGI ? ParseDouble(ListItem(tmpCGILista, 3)) : ParseDouble(ListItem(tmpGLista, tmpTypLista));

            double tmpGTol =
                tmpG < 173 ? 0.500 :
                tmpG < 245 ? 0.580 :
                tmpG < 305 ? 0.640 :
                tmpG < 386 ? 0.720 :
                tmpG < 488 ? 0.800 :
                tmpG < 609 ? (tmpE ? 0.880 : 0.860) :
                tmpF < 760 ? (tmpE ? 1.000 : 0.940) :
                tmpF < 960 ? 1.120 : 1.320;

            kv["SumG"] = "(G) " + Smart(tmpG);
            kv["SumGTol"] = "+ " + F3(tmpGTol);
            kv["SumGTolN"] = "- 0.0";

            string tmpHLista = !tmpE ? "{13:13:14,5:14:0:15:14,8:19,5:19,5:19,5:26,5:26,3:26:31,5:27,5:28:30:30:29,5:36,5:36,5:0:43:43:45:51}"
                : "{0:0:0:0:17:0:0:0:0:0:0:0:0:0:0:0:0:0:0:0:0:39:0:43:42:49:51:51:52:59:0:63:0:74:0:74}";

            double tmpH = ParseDouble(ListItem(tmpHLista, tmpTypLista));

            kv["SumH"] = "(H) " + Smart(tmpH);
            kv["SumHTol"] = "+ 0.0";
            kv["SumHTolN"] = kv["SumCTolN"];

            double tmpJ = !tmpE
                ? tmpTyp < 37 ? tmpH - 3
                : tmpTyp < 46 ? tmpH - 3.5
                : tmpTyp < 57 ? tmpH - 4
                : tmpTyp < 69 ? tmpH - 5
                : tmpTyp < 81 ? tmpH - 5.5
                : tmpTyp < 93 ? tmpH - 6.5
                : tmpTyp < 501 ? tmpH - 7
                : tmpTyp < 601 ? tmpH - 8
                : tmpTyp < 631 ? tmpH - 8.5
                : tmpH - 9
                : tmpTyp < 39 ? tmpH - 4
                : tmpTyp == 600 ? tmpH - 8
                : tmpTyp < 631 ? tmpH - 7
                : tmpTyp < 751 ? tmpH - 9
                : tmpTyp < 851 ? tmpH - 10
                : tmpTyp < 951 ? tmpH - 11
                : tmpH - 13;

            kv["SumJ"] = "(J) " + Smart(tmpJ);
            kv["SumJTol"] = "+ " + F3(tmpCTolN);
            kv["SumJTolN"] = "- 0.0";

            double? tmpK =
                tmpTyp < 46 ? 2.5 :
                tmpTyp < 57 ? 3 :
                tmpTyp < 69 ? 3.5 :
                tmpTyp < 81 ? 4 :
                tmpTyp < 93 ? 4.5 :
                tmpTyp < 531 ? 5 :
                tmpTyp < 601 ? (tmpE ? 5 : 5.5) :
                tmpTyp < 631 ? (tmpE ? 5 : 6) :
                tmpTyp < 680 ? (tmpE ? 5.5 : 6.5) :
                tmpTyp < 751 ? (tmpE ? 6 : 6.5) :
                tmpTyp == 800 || tmpTyp == 850 || tmpTyp == 1180 ? 7 :
                tmpTyp == 950 ? 8 :
                tmpTyp == 1060 ? 9 :
                (double?)null;

            double tmpKValue = tmpK ?? 0;

            kv["SumK"] = "(K) " + (tmpK.HasValue ? Smart(tmpKValue) : "");
            kv["SumKTol"] = "+ 1.000";
            kv["SumKTolN"] = "- 0.0";

            double? tmpL = !tmpE
                ? tmpTyp < 33 ? 10
                : tmpTyp < 46 ? 11
                : tmpTyp < 57 ? 15
                : tmpTyp < 69 ? 19
                : tmpTyp < 81 ? 20
                : tmpTyp < 93 ? 22
                : tmpTyp < 531 ? 25
                : tmpTyp < 561 ? 28
                : tmpTyp < 631 ? 30
                : 33
                : tmpTyp < 39 ? 13
                : tmpTyp == 530 || tmpTyp == 600 ? 31
                : tmpTyp == 630 ? 30
                : tmpTyp == 670 ? 39
                : tmpTyp < 751 ? 37
                : tmpTyp == 800 ? 38
                : tmpTyp == 850 ? 45
                : tmpTyp == 950 ? 48
                : tmpTyp < 1261 ? 58
                : (double?)null;

            double tmpLValue = tmpL ?? 0;
            double tmpLx = tmpC - tmpLValue;

            kv["SumL"] = "(L) " + (tmpL.HasValue ? Smart(tmpLValue) : "");
            kv["SumLTol"] = kv["SumJTol"];
            kv["SumLTolN"] = "- 0.0";
            kv["SumLx"] = "(L) " + Smart(tmpLx);
            kv["SumLxTol"] = "+ 0.0";
            kv["SumLxTolN"] = "- " + F3(tmpCTolN * 2);

            double tmpM = !tmpE
                ? tmpTyp < 37 ? tmpKValue + 0.5
                : tmpTyp < 57 ? tmpKValue + 1
                : tmpTyp < 81 ? tmpKValue + 1.5
                : tmpTyp < 501 ? tmpKValue + 2
                : tmpKValue + 2.5
                : tmpTyp < 39 ? tmpKValue + 1.5
                : tmpTyp < 531 || tmpTyp == 630 ? tmpKValue + 2
                : tmpTyp < 601 ? tmpKValue + 3
                : tmpTyp == 670 ? 9
                : tmpTyp < 951 ? tmpKValue + 3
                : tmpTyp < 1061 ? tmpKValue + 4
                : tmpKValue + 6;

            double tmpMx = tmpC - tmpM;

            kv["SumM"] = "(M) " + Smart(tmpM);
            kv["SumMTol"] = "+ 0.0";
            kv["SumMTolN"] = kv["SumCTolN"];
            kv["SumMx"] = "(M) " + Smart(tmpMx);
            kv["SumMxTol"] = "+ " + F3(tmpCTolN);
            kv["SumMxTolN"] = "- " + F3(tmpCTolN);

            double tmpN =
                tmpTyp < 45 ? 0.3 :
                tmpTyp < 93 ? 0.5 :
                tmpTyp == 670 && tmpE ? 1 :
                tmpTyp < 671 ? (tmpE ? 0.5 : 1) :
                1;

            kv["SumN"] = "(N) " + Smart(tmpN);
            kv["SumNTol"] = "+ 0.300";
            kv["SumNTolN"] = "- 0.0";
            kv["SumN1"] = kv["SumN"];
            kv["SumN1Tol"] = kv["SumNTol"];
            kv["SumN1TolN"] = kv["SumNTolN"];

            double tmpT = tmpTyp < 45 ? 1.5 : tmpTyp < 93 ? 2 : tmpTyp < 561 ? 2.5 : tmpTyp < 951 ? 3 : 4;

            kv["SumT"] = "(T) " + Smart(tmpT);
            kv["SumTTol"] = "+ 0.200";
            kv["SumTTolN"] = "- 0.0";

            string tmpOLista = !tmpE ? "{2:2:2:2:0:2,1:2,2:2,3:2,3:2,6:2,7:2,8:2,9:3:3:3,2:3,3:3,4:3,5:3,6:3,6:0:4,4:4,4:4,4:4,6}"
                : "{0:0:0:0:2,1:0:0:0:0:0:0:0:0:0:0:0:0:0:0:0:0:4,3:0:4,3:4,5:5,1:5,1:5,2:5,6:5,9:0:6,3:0:7:0:8}";

            double tmpO = ParseDouble(ListItem(tmpOLista, tmpTypLista));

            kv["SumO"] = "(O) " + Smart(tmpO);
            kv["SumOTol"] = kv["SumJTol"];
            kv["SumOTolN"] = "- 0.0";

            double tmpR =
                tmpTyp < 37 ? 1.5 :
                tmpTyp < 57 ? 2 :
                tmpTyp < 69 ? 2.5 :
                tmpTyp < 81 ? 3 :
                tmpTyp < 601 ? 4 :
                tmpTyp < 631 ? (tmpE ? 4 : 5) :
                tmpTyp < 751 ? 5 :
                tmpTyp < 951 ? 6 :
                7;

            double tmpRTol = tmpR < 3 ? 0.5 : 1;

            kv["SumR"] = "(R) " + F2Comma(tmpR);
            kv["SumRTol"] = "+ " + F3(tmpRTol);
            kv["SumRTolN"] = "- 0.0";

            double tmpRh = tmpUtrakningB + (tmpUtrakningBTol / 2) + (((tmpRTol / 2) + tmpR) * 2);

            kv["SumRh"] = "(Rh) Ø " + F2Comma(tmpRh);
            kv["SumRhTol"] = "± " + (tmpRTol > 0 && tmpRTol < 1 ? SmartDot(tmpRTol).TrimStart('0') : SmartDot(tmpRTol));

            double tmpR1 =
                tmpTyp < 73 || tmpCGI ? 0.3 :
                tmpTyp == 670 && tmpE ? 1 :
                tmpTyp < 751 ? 0.5 :
                1;

            double tmpR2 =
                tmpTyp < 53 ? 1 :
                tmpTyp < 77 ? 1.2 :
                tmpTyp < 501 ? 1.5 :
                tmpTyp == 670 && tmpE ? 3 :
                tmpTyp < 671 ? 2 :
                tmpTyp < 751 ? 2.5 :
                tmpTyp < 951 ? 3 :
                tmpTyp < 1061 ? 3.4 :
                4;

            kv["SumR1"] = Smart(tmpR1) + "x45º";
            kv["SumR1a"] = kv["SumR1"];
            kv["SumR2"] = "R" + Smart(tmpR2);
            kv["SumR2sid1"] = kv["SumR2"];
            kv["SumR2a"] = tmpE && (tmpTyp == 530 || tmpTyp == 670)
                ? "R2,5"
                : kv["SumR2"];
            kv["SumR2ab"] = kv["SumR2a"];

            double tmpP = !tmpE
                ? tmpTyp < 33 ? 6.5
                : tmpTyp < 46 ? 7
                : tmpTyp < 57 ? 9.5
                : tmpTyp < 69 ? 12
                : tmpTyp < 81 ? 12.5
                : tmpTyp < 93 ? 14
                : tmpTyp < 531 ? 16
                : tmpTyp < 561 ? 18
                : tmpTyp < 631 ? 19
                : 21
                : tmpTyp < 39 ? 8
                : tmpTyp < 601 ? 20
                : tmpTyp == 630 ? 18
                : tmpTyp == 670 || tmpTyp == 710 || tmpTyp == 800 ? 24
                : tmpTyp == 750 ? 23
                : tmpTyp == 850 ? 28
                : tmpTyp == 950 ? 30
                : 36;

            double tmpPx = tmpC - tmpP;

            kv["SumP"] = "(P) " + Smart(tmpP);
            kv["SumPTol"] = "+ 0.0";
            kv["SumPTolN"] = kv["SumCTolN"];
            kv["SumPx"] = "(P) " + Smart(tmpPx);
            kv["SumPxTol"] = "+ " + F3(tmpCTolN);
            kv["SumPxTolN"] = "- " + F3(tmpCTolN);

            double tmpS =
                tmpTyp < 33 ? 3 :
                tmpTyp < 46 ? 4 :
                tmpTyp < 57 ? 6 :
                tmpTyp < 81 ? 8 :
                tmpTyp < 93 ? 9 :
                tmpTyp < 531 ? 10 :
                tmpTyp == 670 && tmpE ? 8 :
                tmpTyp < 631 ? 12 :
                15;

            kv["SumS"] =
                "Ø " + Smart(tmpS) + " x 6 st. hål" +
                LB + "lika delning";

            double tmpHM = tmpC - tmpH;

            kv["SumHM"] = "(HM) " + Smart(tmpHM);
            kv["SumHMTol"] = "± " + F3(tmpCTolN);
            kv["SumHM2"] = kv["SumHM"];
            kv["SumHM2Tol"] = kv["SumHMTol"];

            kv["SumE"] =
                tmpTyp < 37 ? "0.160" :
                tmpTyp < 49 ? "0.185" :
                tmpTyp < 61 ? "0.210" :
                tmpTyp < 81 ? "0.230" :
                tmpTyp < 501 ? "0.250" :
                tmpTyp < 631 ? "0.280" :
                tmpTyp < 801 ? "0.320" :
                tmpTyp < 951 ? "0.360" :
                "0.420";

            kv["SumE1"] =
                tmpTyp < 35 ? "0.063" :
                tmpTyp < 49 ? "0.072" :
                tmpTyp < 61 ? "0.081" :
                tmpTyp < 77 ? "0.089" :
                tmpTyp < 97 ? "0.097" :
                tmpTyp < 601 ? "0.110" :
                tmpTyp < 801 ? "0.125" :
                tmpTyp < 951 ? "0.140" :
                "0.165";

            kv["SumRa"] = "1";
            kv["SumRa1"] = "1";
            kv["SumRa25"] = tmpCGI ? "1" : "2.5";

            bool machineEnabled = IsMachine(maskinVal);

            kv["SumMaskinValS1"] = "Maskin: " + maskinVal + " - Sid.1";
            kv["SumMaskinValS2"] = "Maskin: " + maskinVal + " - Sid.2";

            kv["SumF_A"] = machineEnabled ? "1/3" : "";
            kv["SumF_D"] = machineEnabled ? "1/3" : "";
            kv["SumF_D1"] = machineEnabled ? "1/3" : "";
            kv["SumF_FBG"] = machineEnabled ? "1/3" : "";
            kv["SumF_HJC"] = machineEnabled ? "1/3" : "";
            kv["SumF_HM"] = machineEnabled ? "Inst." : "";
            kv["SumF_Fas"] = machineEnabled ? "Inst." : "";
            kv["SumF_R"] = machineEnabled ? "Inst." : "";
            kv["SumF_Ra"] = machineEnabled ? "1/3" : "";

            kv["SumF_N"] = machineEnabled ? "1/1" : "";
            kv["SumF_LMO"] = machineEnabled ? "1/3" : "";
            kv["SumF_HM2"] = machineEnabled ? "Inst." : "";
            kv["SumF_Fas2"] = machineEnabled ? "Inst." : "";
            kv["SumF_TK"] = machineEnabled ? "Inst." : "";
            kv["SumF_R2"] = machineEnabled ? "Inst." : "";
            kv["SumF_Ra2"] = machineEnabled ? "1/3" : "";
            kv["SumF_EE1"] = machineEnabled ? "Inst." : "";

            kv["SumD_A"] = machineEnabled ? "Digitalskjutmått" : "";
            kv["SumD_D"] = machineEnabled ? "Mikrometer/Skjutmått" : "";
            kv["SumD_D1"] = machineEnabled ? "Mikrometer/Mätplatta" : "";
            kv["SumD_FBG"] = machineEnabled ? "Inv. Mikrometer/Skjutmått" : "";
            kv["SumD_N"] = machineEnabled ? "Digital Djup/Hakmått" : "";
            kv["SumD_HJC"] = machineEnabled ? "Digital Djup/Skjutmått" : "";
            kv["SumD_LMO"] = machineEnabled ? "Digital Djup/Hakmått" : "";
            kv["SumD_HM"] = machineEnabled ? "Digital Djupmått" : "";
            kv["SumD_Fas"] = machineEnabled ? "Digitalskjutmått" : "";
            kv["SumD_TK"] = machineEnabled ? "Passbitar/Skjutmått" : "";
            kv["SumD_R"] = machineEnabled ? "Radielyra" : "";
            kv["SumD_Ra"] = machineEnabled ? "Ytjämnhetsmätare" : "";

            kv["SumD_HM2"] = machineEnabled ? "Digital Djupmått" : "";
            kv["SumD_Fas2"] = machineEnabled ? "Digitalskjutmått" : "";
            kv["SumD_R2"] = machineEnabled ? "Radielyra" : "";
            kv["SumD_Ra2"] = machineEnabled ? "Ytjämnhetsmätare" : "";
            kv["SumD_EE1"] = "";

            kv["SumAF_A"] = "";
            kv["SumAF_D"] = "";
            kv["SumAF_D1"] = machineEnabled ? "Klove: " + Smart(tmpD1) + " Utf.2" : "";
            kv["SumAF_FBG"] = "";
            kv["SumAF_N"] = "";
            kv["SumAF_HJC"] = "";
            kv["SumAF_LMO"] = "";
            kv["SumAF_HM"] = "";
            kv["SumAF_Fas"] = "";
            kv["SumAF_TK"] = "";
            kv["SumAF_R"] = "";
            kv["SumAF_Ra"] = "";
            kv["SumAF_EE1"] = "";

            kv["SumAF_HM2"] = "";
            kv["SumAF_Fas2"] = "";
            kv["SumAF_R2"] = "";
            kv["SumAF_Ra2"] = "";

            kv["SumTextS1"] = "";
            kv["SumTextS2"] = "";

            kv["TmpMTxtUtf2"] = "Passbitklove Utf. 2 med måttet " + kv["SumD1"];

            return kv;
        }
        private static bool IsMachine(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;

            for (int i = 0; i < Machines.Length; i++)
                if (string.Equals(Machines[i], value, StringComparison.Ordinal))
                    return true;

            return false;
        }

        private static double H11Tolerance(double value) =>
            value < 3.01 ? 0.060 : value < 6.01 ? 0.075 : value < 10.01 ? 0.090 :
            value < 18.01 ? 0.110 :value < 30.01 ? 0.130 :value < 50.01 ? 0.160 :
            value < 80.01 ? 0.190 :value < 120.01 ? 0.220 :value < 180.01 ? 0.250 :
            value < 250.01 ? 0.290 : value < 315.01 ? 0.320 :value < 400.01 ? 0.360 :
            value < 500.01 ? 0.400 : value < 630.01 ? 0.440 : value < 800.01 ? 0.500 :
            value < 1000.01 ? 0.560 : value < 1250.01 ? 0.660 :value < 1600.01 ? 0.780 :
            value < 2000.01 ? 0.920 : value < 2500.01 ? 1.100 :1.350;

        private static string[] Split(string value, string separators)
        {
            if (string.IsNullOrEmpty(value)) return Array.Empty<string>();

            return value.Split(separators.ToCharArray(),StringSplitOptions.RemoveEmptyEntries);
        }

        private static string Item(string[] values, int lotusPosition) =>
            values == null || lotusPosition < 1 || lotusPosition > values.Length ? "": values[lotusPosition - 1];

        private static string ListItem(string list, int lotusPosition)
        {
            if (string.IsNullOrWhiteSpace(list) || lotusPosition < 1)
                return "";

            string[] values = list.Trim().TrimStart('{').TrimEnd('}').Split(':');

            return lotusPosition <= values.Length? values[lotusPosition - 1]: "";
        }

        private static double GetArrayItem(double[] values,int lotusPosition) =>
            values == null ||lotusPosition < 1 || lotusPosition > values.Length ? 0: values[lotusPosition - 1];

        private static double ParseDouble(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return 0;

            return double.TryParse( value.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture,out double result) ? result: 0;
        }

        private static string Smart(double value) => Math.Abs(value % 1) < 0.0000001
             ? value.ToString("F0", CultureInfo.InvariantCulture) : value.ToString("0.###", CultureInfo.InvariantCulture).Replace(".", ",");

        private static string SmartDot(double value) => Math.Abs(value % 1) < 0.0000001
             ? value.ToString("F0", CultureInfo.InvariantCulture) : value.ToString("0.###", CultureInfo.InvariantCulture);

        private static string SmartInvariant(double value) => Math.Abs(value % 1) < 0.0000001
             ? value.ToString("F0", CultureInfo.InvariantCulture): value.ToString("0.###", CultureInfo.InvariantCulture);

        private static string F2Comma(double value) => value.ToString( "F2", CultureInfo.InvariantCulture).Replace(".", ",");

        private static string F3(double value) => value.ToString("F3", CultureInfo.InvariantCulture);

        private static string Left(string value, int length) =>
            string.IsNullOrEmpty(value) || length <= 0? "" : value.Substring(0,Math.Min(length, value.Length));

        private static string Middle(string value,int start,int length)
        {
            if (string.IsNullOrEmpty(value) ||start < 0 ||length <= 0 ||start >= value.Length)
                return "";

            return value.Substring(start,Math.Min(length, value.Length - start));
        }

        private static bool ContainsI(string source, string value) => !string.IsNullOrEmpty(source) && !string.IsNullOrEmpty(value) &&
            source.IndexOf(value,StringComparison.OrdinalIgnoreCase) >= 0;

        private static string LotusText(object value)
        {
            if (value == null) return "";
            if (value is double d) return Smart(d);
            if (value is float f) return Smart(f);
            if (value is decimal m) return Smart((double)m);

            return value.ToString() ?? "";
        }

        private static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published)) return "";

            if (!DateTime.TryParse(published, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime publishDate))
            {
                if (!DateTime.TryParse(published, out publishDate))
                    return "";
            }

            DateTime validTo = publishDate.AddDays(14);

            if (DateTime.Today > validTo.Date)
                return "";

            return
                "Denna kontrollinstruktion har nyligen blivit uppdaterad " + "(inom 14 dagar)" + LB + LB +
                "Information om senaste ändring finns under fliken " + "Display & Latest Change eller i Notes Qe i aktivt dokument" +
                LB + LB + "Popupruta aktiv till " + validTo.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
    }
}