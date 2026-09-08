using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class KRAGAR_GR_TNF_34_600_VZ2D8_0005 : ITemplateCalculations
    {
        private const int TmpDagar = 14;

        private static readonly double[] ZeroTab = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        private static readonly Dictionary<string, double[]> Tab7440243 = new Dictionary<string, double[]>
        {
            ["34"] = new[] { 186.2, 185.6, 170, 152, 172, 176, 186, 195, 205.5, 186.6 },
            ["36"] = new[] { 196.2, 195.6, 179.8, 162, 184, 188, 198, 207, 218, 196.6 },
            ["38"] = new[] { 206.2, 205.8, 190, 172, 194, 198, 208, 217, 228, 206.6 },
            ["40"] = new[] { 216.2, 215.8, 200, 183, 204, 208, 218, 227, 237, 216.6 },
            ["44"] = new[] { 236.2, 235.8, 220, 203, 224, 229, 241, 250, 260, 236.6 },
            ["48"] = new[] { 256.2, 255.8, 240, 223, 265, 271, 283, 291, 305.5, 256.6 },
            ["52"] = new[] { 276.4, 276, 260.2, 243, 285, 291, 303, 311, 325.5, 276.8 },
            ["56"] = new[] { 296.4, 296, 280.2, 263, 305, 311, 323, 331, 345.5, 296.8 },
            ["60"] = new[] { 316.4, 316, 300.2, 283, 325, 331, 343, 351, 365.5, 316.8 },
            ["64"] = new[] { 336.6, 336.2, 320.4, 303, 345, 351, 363, 371, 385.5, 337 },
            ["68"] = new[] { 357.2, 356.8, 341, 323, 365, 371, 383, 391, 405, 357.6 },
            ["72"] = new[] { 377.2, 376.8, 361, 343, 385, 391, 403, 411, 425, 377.6 },
            ["76"] = new[] { 397.2, 396.8, 381, 363, 405, 411, 423, 431, 445, 397.6 },
            ["80"] = new[] { 417.6, 417.2, 401.4, 383, 425, 431, 443, 451, 465, 418 },
            ["84"] = new[] { 437.6, 437.2, 421.4, 403, 445, 451, 463, 471, 485, 438 },
            ["88"] = new[] { 457.6, 457.2, 441.4, 413, 455, 461, 473, 481, 495, 458 },
            ["92"] = new[] { 477.8, 477.4, 461.6, 433, 475, 481, 493, 501, 515, 478.2 },
            ["96"] = new[] { 497.8, 497.4, 481.6, 453, 495, 501, 513, 521, 535, 498.2 },
            ["500"] = new[] { 517.8, 517.4, 501.6, 473, 515, 521, 533, 541, 555, 518.2 },
            ["530"] = new[] { 547.8, 547.4, 531.6, 503, 545, 551, 563, 571, 585, 548.2 },
            ["560"] = new[] { 577.8, 577.4, 561.6, 533, 575, 581, 593, 601, 615, 578.2 },
            ["600"] = new[] { 617.8, 617.4, 601.6, 563, 615, 621, 633, 641, 655, 618.2 },
            ["630"] = new[] { 647.8, 648.8, 631.6, 603, 655, 661, 673, 681, 695, 649.6 },
        };

        private static readonly Dictionary<string, double[]> Tab7439487 = new Dictionary<string, double[]>
        {
            ["34/115"] = new[] { 186.2, 185.6, 170, 117, 138, 152, 166, 184, 194.5, 186.6 },
            ["68/12,7"] = new[] { 357.2, 356.8, 341, 319, 365, 371, 383, 391, 405, 357.6 },
            ["36/6,1"] = new[] { 196.2, 195.2, 179.8, 167.1, 184, 188, 198, 207, 218, 196.6 },
            ["38/140"] = new[] { 206.2, 205.8, 190, 142, 176, 186, 196, 205, 216, 206.6 },
            ["38/160"] = new[] { 206.2, 205.8, 190, 162, 194, 198, 208, 217, 228, 206.6 },
            ["38/6,15"] = new[] { 206.2, 205.8, 190, 178, 200, 204, 214, 223, 234, 206.6 },
            ["38/180"] = new[] { 206.2, 205.8, 190, 182, 204, 208, 218, 227, 238, 206.6 },
            ["40/5,15"] = new[] { 216.2, 215.8, 200, 154, 172, 176, 186, 212, 222.5, 216.6 },
            ["40/7,3"] = new[] { 216.2, 215.8, 200, 185.5, 206.5, 210.5, 220.5, 229.5, 239.3, 216.6 },
            ["40/150"] = new[] { 216.2, 215.8, 200, 152, 172, 176, 186, 195, 206.5, 216.6 },
            ["40/170"] = new[] { 216.2, 215.8, 200, 173, 194, 198, 208, 217, 227, 216.6 },
            ["40/190"] = new[] { 216.2, 215.8, 200, 193, 214, 218, 228, 237, 247, 216.6 },
            ["48/8"] = new[] { 256.2, 255.8, 240, 206.2, 245, 251, 263, 271, 285.5, 256.6 },
            ["48/7,3"] = new[] { 256.2, 255.8, 240, 185.5, 215, 228, 240, 254, 265, 256.6 },
            ["48/180"] = new[] { 256.2, 255.8, 240, 183, 204, 208, 218, 228, 244, 256.6 },
            ["48/200"] = new[] { 256.2, 255.8, 240, 203, 245, 251, 263, 271, 285.5, 256.6 },
            ["48/228,6"] = new[] { 256.2, 255.8, 240, 231.5, 273.5, 279.5, 291.5, 299.5, 314, 256.6 },
            ["52/220"] = new[] { 276.4, 276, 260.2, 223, 265, 271, 283, 291, 305.5, 276.8 },
            ["52/8,15"] = new[] { 276.4, 276, 260.2, 230, 272, 278, 290, 298, 312.5, 276.8 },
            ["52/9,7"] = new[] { 276.4, 276, 260.2, 243, 285, 291, 303, 311, 325.5, 276.8 },
            ["56/10"] = new[] { 296.4, 296, 280.2, 257, 305, 311, 323, 331, 345.5, 296.8 },
            ["56/10,7"] = new[] { 296.4, 296, 280.2, 268, 310, 316, 328, 336, 350.5, 296.8 },
            ["56/200"] = new[] { 296.4, 296, 280.2, 203, 224, 229, 241, 291, 303, 296.8 },
            ["56/220"] = new[] { 296.4, 296, 280.2, 223, 265, 271, 283, 291, 305.5, 296.8 },
            ["56/240"] = new[] { 296.4, 296, 280.2, 243, 285, 291, 303, 311, 325.5, 296.8 },
            ["60/11"] = new[] { 316.4, 316, 300.2, 282.4, 325, 331, 343, 351, 365.5, 316.8 },
            ["60/220"] = new[] { 316.4, 316, 300.2, 223, 265, 271, 283, 308, 223, 316.8 },
            ["60/260"] = new[] { 316.4, 316, 300.2, 263, 305, 311, 323, 333, 347.5, 316.8 },
            ["60/10,15"] = new[] { 316.4, 316, 300.2, 280.81, 325, 331, 343, 351, 365.5, 316.8 },
            ["64/240"] = new[] { 336.6, 336.2, 320.4, 243, 285, 291, 303, 311, 327.1, 337 },
            ["64/280"] = new[] { 336.6, 336.2, 320.4, 283, 325, 331, 343, 351, 365.5, 337 },
            ["68/260"] = new[] { 357.2, 356.8, 341, 263, 305, 311, 323, 356, 370, 357.6 },
            ["68/300"] = new[] { 357.2, 356.8, 341, 303, 345, 351, 363, 371, 385, 357.6 },
            ["68/310"] = new[] { 357.2, 356.8, 341, 313, 355, 361, 373, 381, 395, 357.6 },
            ["76/320"] = new[] { 397.2, 396.8, 381, 323, 365, 376, 393, 401, 415, 397.6 },
            ["76/340"] = new[] { 397.2, 396.8, 381, 343, 385, 391, 403, 411, 425, 397.6 },
            ["76/370"] = new[] { 397.2, 396.8, 381, 373, 415, 421, 433, 441, 454.7, 397.6 },
            ["80/340"] = new[] { 417.6, 417.2, 401.4, 343, 385, 391, 403, 411, 425, 418 },
            ["80/360"] = new[] { 417.6, 417.2, 401.4, 363, 405, 411, 423, 431, 445, 418 },
            ["80/15"] = new[] { 417.6, 417.2, 401.4, 384, 425, 431, 443, 451, 465, 418 },
            ["84/355,6"] = new[] { 437.6, 437.2, 421.4, 359, 406, 414, 428, 436, 450, 438 },
            ["92/360"] = new[] { 477.8, 477.4, 461.6, 363, 405, 411, 423, 472, 486, 478.2 },
            ["92/400"] = new[] { 477.8, 477.4, 461.6, 403, 445, 451, 463, 501, 515, 478.2 },
            ["96/410"] = new[] { 497.8, 497.4, 481.6, 413, 455, 461, 473, 495, 509, 498.2 },
            ["530/460"] = new[] { 547.8, 547.4, 531.6, 463, 505, 511, 523, 541, 554.5, 548.2 },
            ["530/480"] = new[] { 547.8, 547.4, 531.6, 483, 525, 531, 543, 551, 565, 548.2 },
            ["530/490"] = new[] { 547.8, 547.4, 531.6, 493, 535, 541, 553, 561, 575, 548.2 },
            ["530/510"] = new[] { 547.8, 547.4, 531.6, 513, 555, 561, 573, 581, 595, 548.2 },
        };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = subject.ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");

            bool tmpSlash = tmpBet.IndexOf('/') >= 0;
            bool tmpVZ = tmpBet.IndexOf("VZ", StringComparison.OrdinalIgnoreCase) >= 0;

            string[] tokens = tmpBet.Split(new[] { ' ', '/', '.' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : "";
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : "";

            string tmpTyp = tmpBet2;
            string tmpTyp2 = tmpBet2 + "/" + tmpBet3;
            int typInt = TryParseInt(tmpTyp);

            string tmpRitning;
            if (tmpTyp2 == "40/5,15" || tmpTyp2 == "40/7,3" || tmpTyp2 == "56/10,7" || tmpTyp2 == "80/360")
                tmpRitning = "7440247";
            else if (tmpSlash && !tmpVZ)
                tmpRitning = "7439487";
            else if (tmpTyp2 == "76/320")
                tmpRitning = "7439487";
            else
                tmpRitning = "7440243";

            if (tmpVZ) tmpRitning = tmpRitning + ", 7439694";

            kv["SumRitS1"] = " " + tmpRitning;
            kv["SumRitS2"] = " " + tmpRitning;

            bool tmp7440243 = tmpRitning.IndexOf("7440243", StringComparison.Ordinal) >= 0;
            bool tmp7439487 = tmpRitning.IndexOf("7439487", StringComparison.Ordinal) >= 0;

            double tmpSpecIDia = 0;
            if (tmp7439487 || tmpTyp2 == "76/320")
                tmpSpecIDia = ParseNum(WordBySeparator(tmpBet, '/', 2));

            double[] tab;
            if (tmp7440243)
            {
                if ((tmpTyp == "60" || tmpTyp == "76") && tmpSpecIDia != 0)
                    tab = ZeroTab;
                else
                    tab = Tab7440243.ContainsKey(tmpTyp) ? Tab7440243[tmpTyp] : ZeroTab;
            }
            else
            {
                tab = Tab7439487.ContainsKey(tmpTyp2) ? Tab7439487[tmpTyp2] : ZeroTab;
            }

            double d1 = tab[0];
            double d2 = tab[1];
            double d3 = tab[2];
            double d4 = tab[3];
            double d5 = tab[4];
            double d6 = tab[5];
            double d7 = tab[6];
            double d8 = tab[7];
            double d9 = tab[8];
            double d10 = tab[9];

            bool d9LessD1 = d9 < d1;

            kv["SumD10"] = "(D10) " + FmtDot(d10);
            kv["SumD10Tol"] = "+ " + F3Dot(0);
            kv["SumD10TolN"] = "- " + " " + F3Dot(0.3);

            kv["SumD9"] = "(D9) " + FmtDot(d9);
            kv["SumD9a"] = d9LessD1 ? kv["SumD9"] : "";
            kv["SumD9Tol"] = "\u00b1 " + F3Dot(d9 < 401 ? 0.5 : 0.8);
            kv["SumD9aTol"] = d9LessD1 ? kv["SumD9Tol"] : "";

            kv["SumD8"] = "(D8) " + FmtDot(d8);
            kv["SumD8Tol"] = "\u00b1 " + F3Dot(d8 < 401 ? 0.5 : 0.8);

            kv["SumD7"] = "(D7) " + FmtDot(d7);
            kv["SumD7Tol"] = "+ " + F3Dot(D7TolPos(d7));
            kv["SumD7TolN"] = "- " + " " + F3Dot(0);

            kv["SumD6"] = "(D6) " + FmtDot(d6);
            kv["SumD6Tol"] = "+ " + F3Dot(0);
            kv["SumD6TolN"] = "- " + " " + F3Dot(D6TolNeg(d6));

            kv["SumD5"] = "(D5) " + FmtDot(d5);
            kv["SumD5Tol"] = "+ " + F3Dot(D5TolPos(d5));
            kv["SumD5TolN"] = "- " + " " + F3Dot(0);

            kv["SumD4"] = "(D4) " + FmtDot(d4);
            kv["SumD4Tol"] = "+ " + F3Dot(0.3);
            kv["SumD4TolN"] = "- " + " " + F3Dot(0);

            kv["SumD3"] = "(D3) " + FmtDot(d3);
            kv["SumD3Tol"] = "\u00b1 " + F3Dot(d3 < 401 ? 0.5 : 0.8);

            kv["SumD2"] = "(D2) " + FmtDot(d2);
            kv["SumD2Tol"] = "+ " + F3Dot(0);
            kv["SumD2TolN"] = "- " + " " + F3Dot(0.3);

            kv["SumD1"] = "(D1) " + FmtDot(d1);
            kv["SumD1a"] = d9LessD1 ? kv["SumD1"] : "";
            kv["SumD1Tol"] = "+ " + F3Dot(0);
            kv["SumD1TolN"] = "- " + " " + F3Dot(0.3);
            kv["SumD1aTol"] = d9LessD1 ? kv["SumD1Tol"] : "";
            kv["SumD1aTolN"] = d9LessD1 ? kv["SumD1TolN"] : "";

            double tmpA;
            if (typInt < 37 || tmpTyp2 == "40/150") tmpA = 39;
            else if (typInt < 41) tmpA = 40;
            else if (typInt == 44) tmpA = 41;
            else if (typInt == 48) tmpA = 43;
            else if (typInt == 52 && tmpTyp2 == "52/220") tmpA = 39;
            else if (typInt == 52) tmpA = 42;
            else if (typInt < 65) tmpA = 38;
            else tmpA = 39;
            kv["SumA"] = "(A) " + FmtG(tmpA);
            kv["SumATol"] = "\u00b1 " + F3Dot(0.3);

            double tmpB;
            if (typInt == 34 || tmpTyp2 == "40/150") tmpB = 60.5;
            else if (typInt == 36 || tmpTyp2 == "36/6,1") tmpB = 60.8;
            else if (typInt == 38) tmpB = 62;
            else if (typInt == 40) tmpB = 60.3;
            else if (typInt == 44) tmpB = 61.3;
            else if (typInt == 48 && tmpSpecIDia == 7.3) tmpB = 65;
            else if (typInt == 48) tmpB = 73;
            else if (typInt == 52 && tmpTyp2 == "52/220") tmpB = 69;
            else if (typInt == 52) tmpB = 72;
            else if (typInt == 56 && tmpSpecIDia == 200) tmpB = 62;
            else if (typInt < 65) tmpB = 68;
            else tmpB = 67.5;
            kv["SumB"] = "(B) " + FmtG(tmpB);
            kv["SumBTol"] = "\u00b1 " + F3Dot(0.3);

            double tmpC;
            if (typInt < 39 || tmpSpecIDia == 5.15 || tmpTyp2 == "40/150" || tmpTyp2 == "36/6,1" || tmpTyp2 == "40/5,15" || tmpTyp2 == "38/6,15") tmpC = 12;
            else if (typInt < 45 || tmpTyp2 == "48/180") tmpC = 10.5;
            else if (typInt == 48 && tmpSpecIDia == 7.3) tmpC = 12;
            else if (typInt == 56 && tmpSpecIDia == 200) tmpC = 10.5;
            else if (typInt == 60 && tmpSpecIDia == 220) tmpC = 14;
            else if (typInt < 53 || tmpTyp2 == "64/240") tmpC = 14;
            else tmpC = 12.5;
            kv["SumC"] = "(C) " + FmtG(tmpC);
            kv["SumCTol"] = "+ " + F3Dot(0.2);
            kv["SumCTolN"] = "- " + " " + F3Dot(0);

            double tmpE;
            if (tmpTyp2 == "40/5,15" || tmpTyp2 == "40/150" || typInt == 34) tmpE = 9;
            else if (tmpTyp2 == "36/6,1") tmpE = 10.5;
            else if (typInt == 38 && tmpSpecIDia == 140) tmpE = 9;
            else if (typInt == 38 || tmpSpecIDia == 7.3) tmpE = 10;
            else if (typInt == 40 && tmpSpecIDia == 5.15) tmpE = 9;
            else if ((typInt == 56 && tmpSpecIDia == 200) || tmpTyp2 == "48/180") tmpE = 10.5;
            else if (typInt < 45) tmpE = 10.5;
            else tmpE = 16;
            kv["SumE"] = "(E) " + FmtG(tmpE);
            kv["SumETol"] = "+ " + F3Dot(0.2);
            kv["SumETolN"] = "- " + " " + F3Dot(0);

            double tmpK;
            if (tmpTyp2 == "40/5,15" || typInt == 34 || tmpTyp2 == "40/150") tmpK = 4.5;
            else if (typInt == 36) tmpK = 4.8;
            else if ((typInt == 38 || typInt == 40) && (tmpSpecIDia == 140 || tmpSpecIDia == 5.15)) tmpK = 4.5;
            else if (typInt == 38) tmpK = 5;
            else if (typInt == 48 && tmpSpecIDia == 7.3) tmpK = 5;
            else if ((typInt == 56 && tmpSpecIDia == 200) || tmpTyp2 == "48/180") tmpK = 4.3;
            else if (typInt < 45) tmpK = 4.3;
            else tmpK = 7.5;
            kv["SumK"] = "(K) " + FmtG(tmpK);
            kv["SumKTol"] = "+ " + F3Dot(0.2);
            kv["SumKTolN"] = "- " + " " + F3Dot(0);

            kv["SumI"] = "(I) 4.8";
            kv["SumITol"] = "\u00b1 " + F3Dot(0.1);

            kv["SumJ"] = "(J) " + FmtDot(8);
            kv["SumJ1"] = kv["SumJ"];
            kv["SumJTol"] = "\u00b1 " + F3Dot(0.2);
            kv["SumJ1Tol"] = kv["SumJTol"];

            kv["SumH"] = "(H) " + FmtDot(22.2);
            kv["SumHTol"] = "+ " + F3Dot(0.84);
            kv["SumHTolN"] = "- " + " " + F3Dot(0);

            double tmpF;
            if (typInt == 34 || tmpTyp2 == "40/150") tmpF = 12.5;
            else if (typInt == 36) tmpF = 12.8;
            else if (typInt == 38 || tmpTyp2 == "92/400") tmpF = 13;
            else if (typInt == 40 && tmpSpecIDia == 5.15) tmpF = 11;
            else if (typInt == 40 && tmpSpecIDia == 170) tmpF = 12.3;
            else if (typInt == 48 && tmpSpecIDia == 7.3) tmpF = 13;
            else if (typInt == 52 && tmpSpecIDia == 220) tmpF = 19.6;
            else if (typInt == 56 && tmpSpecIDia == 200) tmpF = 7;
            else if ((typInt == 60 || typInt == 530) && (tmpSpecIDia == 220 || tmpSpecIDia == 460)) tmpF = 15;
            else if ((typInt == 60 || typInt == 68) && (tmpSpecIDia == 260 || tmpSpecIDia == 310)) tmpF = 16;
            else if (tmpTyp2 == "40/5,15") tmpF = 11;
            else if (typInt < 45 || tmpTyp2 == "48/180") tmpF = 11.8;
            else tmpF = 17.5;
            kv["SumF"] = "(F) " + FmtG(tmpF);
            kv["SumFTol"] = "\u00b1 " + F3Dot(0.5);

            double tmpL;
            if ((typInt == 34 || typInt == 530) && (tmpSpecIDia == 115 || tmpSpecIDia == 460)) tmpL = 18;
            else if (typInt == 38 && tmpSpecIDia == 140) tmpL = 11;
            else if ((typInt == 40 || typInt == 60) && (tmpSpecIDia == 5.15 || tmpSpecIDia == 220)) tmpL = 21;
            else if (typInt == 40 && (tmpSpecIDia == 170 || tmpSpecIDia == 150)) tmpL = 13;
            else if (typInt == 48 && tmpSpecIDia == 7.3) tmpL = 15;
            else if (typInt == 56 && tmpSpecIDia == 200) tmpL = 32;
            else if ((typInt == 60 || typInt == 84) && (tmpSpecIDia == 260 || tmpSpecIDia == 355.6)) tmpL = 14;
            else if (typInt == 68 && tmpSpecIDia == 260) tmpL = 25;
            else if (tmpTyp2 == "92/400") tmpL = 27;
            else if (typInt == 92 && tmpSpecIDia == 360) tmpL = 35;
            else if (typInt == 96 && tmpSpecIDia == 410) tmpL = 20;
            else if (tmpTyp2 == "40/5,15") tmpL = 21;
            else if (tmpTyp2 == "40/7,3") tmpL = 11.8;
            else if (typInt == 36 || typInt == 38 || typInt == 40 || typInt == 44) tmpL = 12;
            else tmpL = 13;
            kv["SumL"] = "(L) " + FmtG(tmpL);
            kv["SumLTol"] = "\u00b1 " + F3Dot(0.2);

            string sumN = tmpVZ ? "1/8-27 NPSF" : (typInt < 65 ? "1/4-28 UNF" : "1/8-27 NPSF");
            kv["SumN"] = sumN;
            kv["SumN1"] = sumN;

            double tmpM;
            if (typInt == 56 && (tmpSpecIDia == 220 || tmpSpecIDia == 240)) tmpM = 7;
            else if (tmpVZ) tmpM = 7;
            else if (typInt < 65) tmpM = 6;
            else tmpM = 7;
            kv["SumM"] = "Min:" + FmtG(tmpM);

            double tmpG;
            if (typInt == 48 && tmpSpecIDia == 7.3) tmpG = 2;
            else if (typInt < 45) tmpG = 2;
            else tmpG = 3;
            kv["SumG"] = d9LessD1 ? "" : FmtG(tmpG) + "x45\u00ba";
            kv["SumGTol"] = d9LessD1 ? "" : "\u00b1 " + F3Dot(0.1);

            double tmpHM = Math.Round(tmpK + tmpC + 0.2, 4);
            kv["SumHM"] = "(HM) " + FmtG(tmpHM) + " *";
            kv["SumHMText"] = "* Hj\u00e4lpm\u00e5tt ber\u00e4knad mitt i tolerans p\u00e5 m\u00e5tten (K) och (C)";

            kv["SumRa"] = "Ra=12.5";

            double tmpVZA;
            if (typInt == 34) tmpVZA = 96.5;
            else if (typInt == 36) tmpVZA = 102.5;
            else if (typInt == 38) tmpVZA = 107.5;
            else if (typInt == 40) tmpVZA = 112.5;
            else if (typInt == 44) tmpVZA = 123.5;
            else if (typInt == 48) tmpVZA = 146.5;
            else if (typInt == 52) tmpVZA = 156.5;
            else if (typInt == 56) tmpVZA = 166.5;
            else if (typInt == 60) tmpVZA = 176.5;
            else if (typInt == 64) tmpVZA = 186.5;
            else if (typInt == 68) tmpVZA = 196.5;
            else tmpVZA = 201.5;

            double tmpVZC = RoundTo(d9 / 2 - tmpVZA, 0.1);

            kv["SumVZA"] = tmpVZ ? "(A) " + FmtG(tmpVZA) : "";
            kv["SumVZATol"] = tmpVZ ? " 0" : "";
            kv["SumVZATolN"] = tmpVZ ? "- 0.2" : "";
            kv["SumVZC"] = tmpVZ ? "(C) " + FmtG(tmpVZC) : "";
            kv["SumVZCTol"] = tmpVZ ? "+ 0.2" : "";
            kv["SumVZCTolN"] = tmpVZ ? " 0" : "";
            kv["SumVZB"] = tmpVZ ? "(B) 8.5  \u00b1 0.2" : "";
            kv["SumVZText"] = tmpVZ
                ? "VZ2D8: Fr\u00e4st sp\u00e5r 180\u00ba fr\u00e5n sm\u00f6rjnippelh\u00e5l med bredd " + kv["SumVZB"] +
                  " och djup m\u00e4tt fr\u00e5n centrum av t\u00e4tning " + kv["SumVZA"] +
                  " -0.2, infr\u00e4st fr\u00e5n kant med hj\u00e4lpm\u00e5tt (C) " + FmtG(tmpVZC)
                : "";

            kv["SumRd15"] = "0.15";
            kv["SumRd15a"] = "0.15";
            kv["SumRd2"] = "0.2";

            kv["SumR08"] = d9LessD1 ? "" : "max: R0.8";
            kv["SumR08a"] = d9LessD1 ? "max: R0.8" : "";
            kv["SumR08x3"] = "max: R0.8 (3x)";
            kv["SumR08x4"] = "max: R0.8 (4x)";
            kv["SumR3"] = "R 3";

            kv["SumV15"] = "15\u00ba";
            kv["SumRa32"] = "3.2";

            bool isNak = EqualsI(maskinVal, "Nakamura");
            bool isLb = EqualsI(maskinVal, "LB45");
            bool mAny = isNak || isLb;
            string mvS = isNak ? "Nakamura" : isLb ? "LB45" : "";
            kv["SumMaskinValS1"] = "Maskin: " + mvS;
            kv["SumMaskinValS2"] = "Maskin: " + mvS;

            kv["SumF1_1"] = mAny ? "1/2" : "";
            kv["SumF1_2"] = mAny ? "1/2" : "";
            kv["SumF1_3"] = mAny ? "1/2" : "";
            kv["SumF1_4"] = mAny ? "1/2" : "";
            kv["SumF1_5"] = isNak ? "1/5" : isLb ? "1/2" : "";
            kv["SumF1_6"] = "";
            kv["SumF2_1"] = mAny ? (tmpVZ ? "1/2" : "") : "";
            kv["SumF2_2"] = mAny ? (tmpVZ ? "1/2" : "") : "";
            kv["SumF2_3"] = "";

            kv["SumD1_1"] = mAny ? "Skjutm\u00e5tt" : "";
            kv["SumD1_2"] = mAny ? "min/max G\u00e4ngtolk" : "";
            kv["SumD1_3"] = mAny ? "Skjutm\u00e5tt/Djupm\u00e5tt" : "";
            kv["SumD1_4"] = mAny ? "Ra-m\u00e4tare" : "";
            kv["SumD1_5"] = mAny ? "M\u00e4tmaskin" : "";
            kv["SumD1_6"] = "";
            kv["SumD2_1"] = mAny ? (tmpVZ ? "Skjutm\u00e5tt/Djupm\u00e5tt" : "") : "";
            kv["SumD2_2"] = mAny ? (tmpVZ ? "Skjutm\u00e5tt" : "") : "";
            kv["SumD2_3"] = mAny ? "Ra-m\u00e4tare" : "";

            kv["SumAF1_1"] = mAny ? "Alt. UD-Apparat inst. med Ring/Klove" : "";
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = mAny ? "Vid misst\u00e4nkt formfel l\u00e4mnas kragen till m\u00e4trummet" : "";
            kv["SumAF1_6"] = "";
            kv["SumAF2_1"] = mAny ? (tmpVZ ? "" : "M\u00e4ts inte") : "";
            kv["SumAF2_2"] = mAny ? (tmpVZ ? "" : "M\u00e4ts inte") : "";
            kv["SumAF2_3"] = "";

            kv["SumTextS1"] = "Skarpa kanter avgradas.";
            kv["SumTextS2"] = "Skarpa kanter avgradas.";

            kv["SumProfil"] = (d9LessD1 || tmpVZ)
                ? "Profil n\u00e4r D9 \u00e4r mindre \u00e4n D1 samt layout p\u00e5 fr\u00e4st sp\u00e5r i VZ2D8 finns p\u00e5 sid.2"
                : "";

            if (d9LessD1)
                kv["VaLProfil"] = "2 sidor ing\u00e5r i kontrollinstruktionen\nProfil n\u00e4r D9 \u00e4r mindre \u00e4n D1";
            else if (tmpVZ)
                kv["VaLProfil"] = "2 sidor ing\u00e5r i kontrollinstruktionen\nLayout p\u00e5 fr\u00e4st sp\u00e5r i VZ2D8";
            else
                kv["VaLProfil"] = "";

            return kv;
        }

        private static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published)) return "";
            DateTime pubDt;
            if (!DateTime.TryParse(published, out pubDt)) return "";
            DateTime validTill = pubDt.AddDays(TmpDagar);
            if (DateTime.Today <= validTill.Date)
                return "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom " + TmpDagar + " dagar)" +
                       "\n\n" + "" + "\n\n" +
                       "Information om senaste \u00e4ndring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument" +
                       "\n\n" +
                       "Popupruta aktiv till " + validTill.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return "";
        }

        private static double D7TolPos(double v)
        {
            if (v < 3.01) return 0.060;
            if (v < 6.01) return 0.075;
            if (v < 10.01) return 0.090;
            if (v < 18.01) return 0.110;
            if (v < 30.01) return 0.130;
            if (v < 50.01) return 0.160;
            if (v < 80.01) return 0.190;
            if (v < 120.01) return 0.220;
            if (v < 180.01) return 0.250;
            if (v < 250.01) return 0.290;
            if (v < 315.01) return 0.320;
            if (v < 400.01) return 0.360;
            if (v < 500.01) return 0.400;
            return 0.440;
        }

        private static double D6TolNeg(double v)
        {
            if (v < 181) return 0.25;
            if (v < 251) return 0.29;
            if (v < 316) return 0.32;
            if (v < 401) return 0.36;
            if (v < 501) return 0.4;
            return 0.44;
        }

        private static double D5TolPos(double v)
        {
            if (v < 180) return 0.25;
            if (v < 250) return 0.29;
            if (v < 310) return 0.32;
            if (v < 390) return 0.36;
            if (v < 496) return 0.4;
            return 0.44;
        }

        private static string WordBySeparator(string value, char separator, int index)
        {
            if (string.IsNullOrEmpty(value)) return "";
            string[] parts = value.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            return parts.Length >= index ? parts[index - 1] : "";
        }

        private static double ParseNum(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;
            double v;
            return double.TryParse(s.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static int TryParseInt(string s)
        {
            int v;
            return int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static bool EqualsI(string a, string b)
        {
            return string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);
        }

        private static double RoundTo(double value, double step)
        {
            if (step == 0) return value;
            return Math.Round(Math.Round(value / step, MidpointRounding.AwayFromZero) * step, 4);
        }

        private static string FmtG(double v)
        {
            return v.ToString(CommonFunctions.Culture).Replace(".", ",");
        }

        private static string FmtDot(double v)
        {
            return v.ToString(CultureInfo.InvariantCulture);
        }

        private static string F3Dot(double v)
        {
            return v.ToString("F3", CultureInfo.InvariantCulture).Replace(",", ".");
        }
    }
}