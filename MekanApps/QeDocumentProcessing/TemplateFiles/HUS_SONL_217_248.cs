using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;
using System.Reflection;
using System.Collections;
namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HUS_SONL_217_248 : ITemplateCalculations
    {
        private static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            if (string.IsNullOrWhiteSpace(subject)) subject = GetStringField(req, "Subject");

            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;
            if (string.IsNullOrWhiteSpace(maskinVal)) maskinVal = GetStringField(req, "MaskinVal");

            string publishedRaw = GetStringField(req, "Published");
            DateTime published;
            DateTime? publishedDt = DateTime.TryParse(publishedRaw, out published) ? (DateTime?)published : null;

            DateTime now = DateTime.Now;

            // -----------------------------------------------------------------
            // RADBRYTNINGAR / POPUP
            // -----------------------------------------------------------------
            string tmpCR2 = Environment.NewLine + Environment.NewLine;
            string tmpCR3 = tmpCR2 + Environment.NewLine;
            string tmpCR4 = tmpCR3 + Environment.NewLine;

            kv["TmpCR2"] = tmpCR2;
            kv["TmpCR3"] = tmpCR3;
            kv["TmpCR4"] = tmpCR4;

            int tmpDagar = 14;
            bool tmpPublish = publishedDt.HasValue;
            string tmpSpecInfo = "";
            string tmpGiltigTill = tmpPublish ? publishedDt.Value.AddDays(tmpDagar).ToString("yyyy-MM-dd HH:mm:ss", Inv) : "";
            string vaLPopUp = "";

            if (tmpPublish && DateTime.Today <= publishedDt.Value.Date.AddDays(tmpDagar))
            {
                vaLPopUp =
                    "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom " + tmpDagar.ToString(Inv) + " dagar)" +
                    tmpCR2 +
                    tmpSpecInfo +
                    tmpCR2 +
                    "Information om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument" +
                    tmpCR2 +
                    "Popupruta aktiv till " + tmpGiltigTill;
            }

            kv["TmpDagar"] = tmpDagar.ToString(Inv);
            kv["TmpPublish"] = tmpPublish ? "1" : "0";
            kv["TmpSpecInfo"] = tmpSpecInfo;
            kv["TmpGiltig_Till"] = tmpGiltigTill;
            kv["VaLPopUp"] = vaLPopUp;

            // -----------------------------------------------------------------
            // USER / DATE
            // -----------------------------------------------------------------
            kv["TmpUser"] = GetStringField(req, "UserName");
            kv["TmpServerName"] = GetStringField(req, "ServerName");
            kv["TmpNow"] = now.ToString("yyyy:MM:dd:HH:mm:ss", Inv);

            int tmpWeekday = ((int)now.DayOfWeek) + 1; // Lotus-like weekday
            bool tmpHelg = !(tmpWeekday == 2 || tmpWeekday == 3 || tmpWeekday == 4 || tmpWeekday == 5);
            bool tmp0_6 = now.Hour == 0 || now.Hour < 6;

            kv["TmpWeekday"] = tmpWeekday.ToString(Inv);
            kv["TmpHelg"] = tmpHelg ? "1" : "0";
            kv["TmpYear"] = now.Year.ToString(Inv);
            kv["TmpMonth"] = now.Month.ToString("00", Inv);
            kv["TmpDay"] = now.Day.ToString("00", Inv);
            kv["TmpHour"] = now.Hour.ToString(Inv);
            kv["TmpMinute"] = now.Minute.ToString("00", Inv);
            kv["Tmp0_6"] = tmp0_6 ? "1" : "0";

            // -----------------------------------------------------------------
            // KONSTANTER
            // -----------------------------------------------------------------
            string TmpKon0 = FmtFixed(0m, 0);
            string TmpKon005 = FmtFixed(0.05m, 3);
            string TmpKon01 = FmtFixed(0.1m, 3);
            string TmpKon012 = FmtFixed(0.12m, 3);
            string TmpKon015 = FmtFixed(0.15m, 3);
            string TmpKon02 = FmtFixed(0.2m, 3);
            string TmpKon03 = FmtFixed(0.3m, 3);
            string TmpKon04 = FmtFixed(0.4m, 3);
            string TmpKon042 = FmtFixed(0.42m, 3);
            string TmpKon05 = FmtFixed(0.5m, 3);
            string TmpKon06 = FmtFixed(0.6m, 3);
            string TmpKon063 = FmtFixed(0.63m, 3);
            string TmpKon075 = FmtFixed(0.75m, 3);
            string TmpKon08 = FmtFixed(0.8m, 3);
            string TmpKon1 = FmtFixed(1.0m, 1);
            string TmpKon125 = FmtFixed(1.25m, 3);
            string TmpKon15 = FmtFixed(1.5m, 1);

            kv["TmpKon0"] = TmpKon0;
            kv["TmpKon005"] = TmpKon005;
            kv["TmpKon01"] = TmpKon01;
            kv["TmpKon012"] = TmpKon012;
            kv["TmpKon015"] = TmpKon015;
            kv["TmpKon02"] = TmpKon02;
            kv["TmpKon03"] = TmpKon03;
            kv["TmpKon04"] = TmpKon04;
            kv["TmpKon042"] = TmpKon042;
            kv["TmpKon05"] = TmpKon05;
            kv["TmpKon06"] = TmpKon06;
            kv["TmpKon063"] = TmpKon063;
            kv["TmpKon075"] = TmpKon075;
            kv["TmpKon08"] = TmpKon08;
            kv["TmpKon1"] = TmpKon1;
            kv["TmpKon125"] = TmpKon125;
            kv["TmpKon15"] = TmpKon15;

            // -----------------------------------------------------------------
            // FORMAT / PARSE SUBJECT
            // -----------------------------------------------------------------
            string tmpFormat = (subject ?? string.Empty).Trim().ToUpperInvariant();
            string tmpBet = tmpFormat.Replace(".", ",");
            string[] tokens = SplitTokens(tmpBet);

            string tmpBet1 = tokens.Length > 0 ? tokens[0] : string.Empty;
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : string.Empty;
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : string.Empty;
            string tmpBet4 = tokens.Length > 3 ? tokens[3] : string.Empty;
            string tmpBet5 = tokens.Length > 4 ? tokens[4] : string.Empty;

            kv["TmpFormat"] = tmpFormat;
            kv["TmpBet"] = tmpBet;
            kv["TmpBet1"] = tmpBet1;
            kv["TmpBet2"] = tmpBet2;
            kv["TmpBet3"] = tmpBet3;
            kv["TmpBet4"] = tmpBet4;
            kv["TmpBet5"] = tmpBet5;
            kv["TmpCountB"] = tmpBet.Length.ToString(Inv);
            kv["TmpCountB1"] = tmpBet1.Length.ToString(Inv);
            kv["TmpCountB2"] = tmpBet2.Length.ToString(Inv);

            bool tmpSlash = tmpBet.Contains("/");
            bool tmp_G = !string.IsNullOrEmpty(tmpBet3) && tmpBet3.IndexOf("G", StringComparison.OrdinalIgnoreCase) >= 0;

            kv["TmpSlash"] = tmpSlash ? "1" : "0";
            kv["Tmp_G"] = tmp_G ? "1" : "0";

            string tmpART = tmpBet1;
            string tmpSerie = !string.IsNullOrEmpty(tmpBet2) ? Left(tmpBet2, 1) : string.Empty;
            string tmpTyp = !string.IsNullOrEmpty(tmpBet2) ? Right(tmpBet2, 2) : string.Empty;

            kv["TmpART"] = tmpART;
            kv["TmpSerie"] = tmpSerie;
            kv["TmpTyp"] = tmpTyp;

            string[] typLista = { "17", "18", "20", "22", "24", "26", "28", "30", "32", "34", "36", "38", "40", "44", "48" };
            int tmpTypLista = IndexOf(typLista, tmpTyp);
            if (tmpTypLista <= 0)
            {
                kv["Error"] = "Ogiltig typ för HUS_SONL_217_248. Tillåtna typer: 17,18,20,22,24,26,28,30,32,34,36,38,40,44,48";
                return kv;
            }

            int tmpTypNum;
            if (!int.TryParse(tmpTyp, NumberStyles.Integer, Inv, out tmpTypNum))
            {
                kv["Error"] = "TmpTyp kunde inte tolkas som tal.";
                return kv;
            }

            kv["TmpTypLista"] = tmpTypLista.ToString(Inv);

            // -----------------------------------------------------------------
            // BASIC SUM VALUES
            // -----------------------------------------------------------------
            kv["SumArt"] = tmpART;
            kv["SumSerie"] = tmpSerie;
            kv["SumTyp"] = tmpTyp;

            // -----------------------------------------------------------------
            // DIMENSIONER
            // -----------------------------------------------------------------

            // S1/S2 Centrumhöjd (A)
            decimal[] TmpS1ALista = { 125, 135, 145, 160, 170, 180, 190, 200, 215, 235, 245, 260, 275, 305, 340 };
            decimal TmpS1A = Pick(TmpS1ALista, tmpTypLista);
            string SumS1A = "(A) " + Num(TmpS1A);
            string TmpS1ATol = FmtFixed(TolJs11(TmpS1A), 3);
            string SumS1ATol = "± " + TmpS1ATol + " [3]";
            string SumS3A = SumS1A;
            string SumS3ATol = SumS1ATol;

            kv["TmpS1A"] = Num(TmpS1A);
            kv["SumS1A"] = SumS1A;
            kv["TmpS1ATol"] = TmpS1ATol;
            kv["SumS1ATol"] = SumS1ATol;
            kv["SumS3A"] = SumS3A;
            kv["SumS3ATol"] = SumS3ATol;

            // Fothöjd (B)
            decimal[] TmpS1BLista = { 35, 45, 50, 50, 55, 60, 65, 65, 65, 70, 75, 85, 85, 95, 100 };
            decimal TmpS1B = Pick(TmpS1BLista, tmpTypLista);
            kv["TmpS1B"] = Num(TmpS1B);
            kv["SumS1B"] = "(B) " + Num(TmpS1B);
            kv["TmpS1BTol"] = TmpKon1;
            kv["SumS1BTol"] = "± " + TmpKon1;

            // Hålbild (C)
            decimal[] TmpS1CLista = { 80, 86, 94, 108, 124, 130, 138, 138, 150, 160, 170, 180, 184, 204, 210 };
            decimal TmpS1C = Pick(TmpS1CLista, tmpTypLista);
            kv["TmpS1C"] = Num(TmpS1C);
            kv["SumS1C"] = "(C) " + Num(TmpS1C);
            kv["TmpS1CTol"] = FmtFixed(TolGeneral(TmpS1C), 3);
            kv["SumS1CTol"] = "± " + kv["TmpS1CTol"];

            // Hålbild (D)
            decimal[] TmpS1DLista = { 179, 195, 214, 234, 248, 266, 290, 314, 340, 371, 387, 409, 425, 475, 525 };
            decimal TmpS1D = Pick(TmpS1DLista, tmpTypLista);
            kv["TmpS1D"] = Num(TmpS1D);
            kv["SumS1D"] = "(D) " + Num(TmpS1D);
            kv["TmpS1DTol"] = FmtFixed(TolJs13(TmpS1D), 3);
            kv["SumS1DTol"] = "± " + kv["TmpS1DTol"] + " [3]";

            // Gänga hopslagningsbult (G)
            int TmpS1G = tmpTypNum < 18 ? 10 :
                         tmpTypNum < 24 ? 12 :
                         tmpTypNum < 28 ? 16 :
                         tmpTypNum < 34 ? 20 :
                         tmpTypNum < 48 ? 24 : 30;
            kv["TmpS1G"] = TmpS1G.ToString(Inv);
            kv["SumS1G"] = "(G) M" + TmpS1G.ToString(Inv) + "-6H [3]";

            // FAS BORRHÅL (I)
            decimal TmpS1I = TmpS1G == 10 ? 12m :
                             TmpS1G == 12 ? 14.5m :
                             TmpS1G == 16 ? 18.5m :
                             TmpS1G == 20 ? 23m :
                             TmpS1G == 24 ? 27m : 33m;
            kv["TmpS1I"] = Num(TmpS1I);
            kv["SumS1I"] = "(I) " + Num(TmpS1I);
            kv["TmpS1ITol"] = FmtFixed(TolH15(TmpS1I), 3);
            kv["SumS1ITol"] = "+ " + kv["TmpS1ITol"] + " [3]";
            kv["TmpS1ITolN"] = TmpKon0;
            kv["SumS1ITolN"] = "- " + TmpKon0 + " [3]";

            // Gängdjup hopslagningsbult (H)
            decimal TmpS1H =
                TmpS1G == 10 ? 25 :
                TmpS1G == 12 ? 30 :
                tmpTypNum == 24 ? 42 :
                tmpTypNum == 26 ? 40 :
                (tmpTypNum == 28 || tmpTypNum == 32) ? 45 :
                tmpTypNum == 30 ? 50 :
                tmpTypNum < 40 ? 48 :
                tmpTypNum == 40 ? 46 :
                tmpTypNum == 44 ? 55 :
                tmpTypNum == 48 ? 65 : 0;
            kv["TmpS1H"] = Num(TmpS1H);
            kv["SumS1H"] = "(H) " + Num(TmpS1H);
            kv["TmpS1HTol"] = TmpKon03;
            kv["SumS1HTol"] = "± " + TmpKon03;

            // Borrdjup hopslagningsbult (F)
            decimal TmpS1F =
                TmpS1H == 25 ? 30 :
                TmpS1H == 30 ? 35 :
                TmpS1H == 42 ? 48 :
                TmpS1H == 40 ? 46 :
                TmpS1H == 45 ? 53 :
                TmpS1H == 50 ? 58 :
                TmpS1H == 48 ? 57 :
                TmpS1H == 46 ? 55 :
                TmpS1H == 55 ? 64 :
                TmpS1H == 65 ? 80 : 0;
            kv["TmpS1F"] = Num(TmpS1F);
            kv["SumS1F"] = "(F) " + Num(TmpS1F);
            kv["TmpS1FTol"] = TmpKon03;
            kv["SumS1FTol"] = "± " + TmpKon03;

            // Stifthålsdiameter S1/S2 (M)
            decimal TmpS1M = tmpTypNum < 44 ? 9.31m : 16m;
            kv["TmpS1M"] = Num(TmpS1M);
            kv["SumS1M"] = "2x (M) " + Num(TmpS1M);
            kv["TmpS1MTol"] = TmpS1M < 16m ? FmtFixed(0.036m, 3) : FmtFixed(0.18m, 2);
            kv["SumS1MTol"] = "+ " + kv["TmpS1MTol"] + (TmpS1M == 9.31m ? " [2]" : " [3]");
            kv["TmpS1MTolN"] = TmpKon0;
            kv["SumS1MTolN"] = "- " + TmpKon0 + (TmpS1M == 9.31m ? " [2]" : " [3]");

            kv["SumS2M"] = kv["SumS1M"];
            kv["SumS2MTol"] = kv["SumS1MTol"];
            kv["SumS2MTolN"] = kv["SumS1MTolN"];

            // Stifthålsdjup S1/S2 (L)
            decimal TmpS1L = TmpS1M == 9.31m ? 10.5m : 20m;
            kv["TmpS1L"] = Num(TmpS1L);
            kv["SumS1L"] = "(L) " + Num(TmpS1L);
            kv["TmpS1LTol"] = TmpS1M == 9.31m ? TmpKon0 : TmpKon042;
            kv["SumS1LTol"] = "+ " + kv["TmpS1LTol"] + " [3]";
            kv["TmpS1LTolN"] = TmpS1M == 9.31m ? TmpKon05 : TmpKon042;
            kv["SumS1LTolN"] = "- " + kv["TmpS1LTolN"] + " [3]";

            decimal TmpS2L = TmpS1M == 9.31m ? 10m : 20m;
            kv["TmpS2L"] = Num(TmpS2L);
            kv["SumS2L"] = "(L) " + Num(TmpS2L);
            kv["TmpS2LTol"] = TmpS1M == 9.31m ? TmpKon02 : TmpKon042;
            kv["SumS2LTol"] = "± " + kv["TmpS2LTol"];

            // Stifthål avstånd mellan hål (N)
            decimal[] TmpS1NLista = { 167, 180, 202, 222, 238, 256, 276, 298, 322, 352, 362, 382, 400, 440, 492 };
            decimal TmpS1N = Pick(TmpS1NLista, tmpTypLista);
            kv["TmpS1N"] = Num(TmpS1N);
            kv["SumS1N"] = "(N) " + Num(TmpS1N);
            kv["TmpS1NTol"] = TmpKon03;
            kv["SumS1NTol"] = "± " + TmpKon03;

            // Stifthål placering (E)
            decimal TmpS1E = tmpTypNum < 44 ? 11 : 21;
            kv["TmpS1E"] = Num(TmpS1E);
            kv["SumS1E"] = "(E) " + Num(TmpS1E);
            kv["TmpS1ETol"] = TmpKon015;
            kv["SumS1ETol"] = "± " + TmpKon015;

            // Plan hopläggningsytor S1/S2 (P)
            kv["TmpS1P"] = TmpKon005;
            kv["SumS1P"] = TmpKon005;
            kv["SumS2P"] = TmpKon005.Replace(".",",");

            // Plan fot (Pf)
            kv["TmpS1Pf"] = TmpKon01;
            kv["SumS1Pf"] = TmpKon01;

            // Bultplan (A)
            decimal[] TmpS2ALista = { 53, 55, 62, 65, 81, 84, 88, 94, 98, 117, 117, 119, 119, 130, 147 };
            decimal TmpS2A = Pick(TmpS2ALista, tmpTypLista);
            kv["TmpS2A"] = Num(TmpS2A);
            kv["SumS2A"] = "(A) " + Num(TmpS2A);
            kv["TmpS2ATol"] = TmpKon08;
            kv["SumS2ATol"] = "± " + TmpKon08;

            // Bulthål (E)
            decimal TmpS2E = TmpS1G == 10 ? 11m :
                             TmpS1G == 12 ? 13.5m :
                             TmpS1G == 16 ? 17.5m :
                             TmpS1G == 20 ? 22m :
                             TmpS1G == 24 ? 26m :
                             TmpS1G == 30 ? 33m : 0m;
            kv["TmpS2E"] = Num(TmpS2E);
            kv["SumS2E"] = "(E) " + Num(TmpS2E);
            kv["TmpS2ETol"] = FmtFixed(TolH15Wide(TmpS2E), 3);
            kv["SumS2ETol"] = "+ " + kv["TmpS2ETol"];
            kv["TmpS2ETolN"] = TmpKon0;
            kv["SumS2ETolN"] = "- " + TmpKon0;

            // Borrdjup lyftögla (B)
            decimal TmpS2B = tmpTypNum < 24 ? 26.5m :
                             tmpTypNum < 32 ? 35m :
                             tmpTypNum == 32 ? 37.5m :
                             tmpTypNum < 44 ? 44.5m : 55m;
            kv["TmpS2B"] = Num(TmpS2B);
            kv["SumS2B"] = "(B) " + Num(TmpS2B);
            string tmpBTol = FmtFixed(TolGeneral(TmpS2B), 3);
            kv["TmpBTol"] = tmpBTol;
            kv["TmpS2BTol"] = (tmpTypNum == 24 || tmpTypNum == 26) ? TmpKon0 : tmpBTol;
            kv["SumS2BTol"] = "+ " + kv["TmpS2BTol"];
            kv["TmpS2BTolN"] = (tmpTypNum == 24 || tmpTypNum == 26) ? TmpKon1 : tmpBTol;
            kv["SumS2BTolN"] = "- " + kv["TmpS2BTolN"];

            // Lyftögla gänga (G1)
            int TmpS2G1 = tmpTypNum < 24 ? 12 :
                          tmpTypNum < 34 ? 20 :
                          tmpTypNum < 44 ? 24 : 30;
            kv["TmpS2G1"] = TmpS2G1.ToString(Inv);
            kv["SumS2G1"] = "(G1) M" + TmpS2G1.ToString(Inv) + "-6H [3]";

            // Lyftögla gängdjup (C)
            decimal TmpS2C = TmpS2G1 == 12 ? 20.5m :
                             TmpS2G1 == 20 ? 30m :
                             TmpS2G1 == 24 ? 36m : 45m;
            kv["TmpS2C"] = Num(TmpS2C);
            kv["SumS2C"] = "min " + Num(TmpS2C);

            // Nippel gänga (G)
            string TmpS2G = tmpTypNum < 34 ? "3/8" : "3/4";
            kv["TmpS2G"] = TmpS2G;
            kv["SumS2G"] = "2x (G) " + TmpS2G;

            // Nippel gängdjup (C/F)
            decimal TmpS2F = TmpS2G == "3/8" ? 15m : 20m;
            kv["TmpS2F"] = Num(TmpS2F);
            kv["SumS2F"] = "min " + Num(TmpS2F);

            // Mått från centrum till nippel borrhål (J)
            decimal[] TmpS2JLista = { 47.5m, 50, 50, 55, 55, 60, 65, 65, 65, 85, 85, 85, 85, 105, 110 };
            decimal TmpS2J = Pick(TmpS2JLista, tmpTypLista);
            kv["TmpS2J"] = Num(TmpS2J);
            kv["SumS2J"] = "(J) " + Num(TmpS2J);
            kv["TmpS2JTol"] = TmpKon08;
            kv["SumS2JTol"] = "± " + TmpKon08;

            // Mått mellan nippel borrhål (H)
            decimal[] TmpS2HLista = { 40, 41, 47, 53, 65, 69, 72, 73, 78, 88, 93, 98, 100, 108, 115 };
            decimal TmpS2H = Pick(TmpS2HLista, tmpTypLista);
            kv["TmpS2H"] = Num(TmpS2H);
            kv["SumS2H"] = "(H) " + Num(TmpS2H);
            kv["TmpS2HTol"] = TmpKon08;
            kv["SumS2HTol"] = "± " + TmpKon08;

            // Längdmått mellan hopläggnings borrhål (K)
            decimal[] TmpS2KLista = { 179, 195, 214, 234, 248, 266, 290, 314, 340, 371, 387, 409, 425, 475, 525 };
            decimal TmpS2K = Pick(TmpS2KLista, tmpTypLista);
            kv["TmpS2K"] = Num(TmpS2K);
            kv["SumS2K"] = "(K) " + Num(TmpS2K);
            kv["TmpS2KTol"] = FmtFixed(TolJs13(TmpS2K), 3);
            kv["SumS2KTol"] = "± " + kv["TmpS2KTol"] + " [3]";

            // Breddmått mellan hopläggnings borrhål (D)
            decimal[] TmpS2DLista = { 80, 86, 94, 108, 124, 130, 138, 138, 150, 160, 170, 180, 184, 204, 210 };
            decimal TmpS2D = Pick(TmpS2DLista, tmpTypLista);
            kv["TmpS2D"] = Num(TmpS2D);
            kv["SumS2D"] = "(D) " + Num(TmpS2D);
            kv["TmpS2DTol"] = FmtFixed(TolGeneral(TmpS2D), 3);
            kv["SumS2DTol"] = "± " + kv["TmpS2DTol"];

            // Oljecirkulationshål borrhöjd (B)
            decimal[] TmpS3BLista = { 103, 113, 123, 137, 145, 155, 164, 174, 189, 194, 204, 219, 234, 264, 299 };
            decimal TmpS3Ba = Pick(TmpS3BLista, tmpTypLista);
            decimal TmpS3B = TmpS1A - TmpS3Ba;
            kv["TmpS3Ba"] = Num(TmpS3Ba);
            kv["TmpS3B"] = Num(TmpS3B);
            kv["SumS3B"] = "(B) " + Num(TmpS3B);
            kv["TmpS3BTol"] = TmpKon05;
            kv["SumS3BTol"] = "± " + TmpKon05;

            // Längd mellan oljecirkulationshål (C)
            decimal[] TmpS3CLista = { 118, 128, 144, 162, 178, 192, 200, 220, 252, 268, 278, 298, 320, 360, 400 };
            decimal TmpS3C = Pick(TmpS3CLista, tmpTypLista);
            kv["TmpS3C"] = Num(TmpS3C);
            kv["SumS3C"] = "(C) " + Num(TmpS3C);
            kv["TmpS3CTol"] = tmpTypNum < 40 ? TmpKon05 : TmpKon08;
            kv["SumS3CTol"] = "± " + kv["TmpS3CTol"];

            // Gänga M8 sidohål (G)
            kv["TmpS3G"] = "8";
            kv["SumS3G"] = "M8-6H";

            // Höjd till M8 hål (D)
            decimal[] TmpS3DLista = { 59, 64, 70, 73, 80, 87, 90, 92, 95, 105, 110, 130, 125, 135, 170 };
            decimal TmpS3D = Pick(TmpS3DLista, tmpTypLista);
            kv["TmpS3D"] = Num(TmpS3D);
            kv["SumS3D"] = "(D) " + Num(TmpS3D);
            kv["TmpS3DTol"] = TmpKon05;
            kv["SumS3DTol"] = "± " + TmpKon05;

            // Borrdjup till M8 hål (E)
            kv["TmpS3E"] = "17";
            kv["SumS3E"] = "(E) 17";
            kv["TmpS3ETol"] = TmpKon05;
            kv["SumS3ETol"] = "± " + TmpKon05;

            // Gängdjup till M8 hål (F)
            kv["TmpS3F"] = "13";
            kv["SumS3F"] = "(F) min: 13";
            kv["TmpS3FTol"] = TmpKon05;
            kv["SumS3FTol"] = "± " + TmpKon05;

            // Fas till M8 hål (H)
            kv["TmpS3H"] = "10";
            kv["SumS3H"] = "(H) 10";
            kv["TmpS3HTol"] = TmpKon02;
            kv["SumS3HTol"] = "± " + TmpKon02;

            // Gänga G3/4 (G1)
            string TmpS3G1 = tmpTypNum < 34 ? "3/4" : "1.1/2";
            kv["TmpS3G1"] = TmpS3G1;
            kv["SumS3G1"] = "G " + TmpS3G1;

            // Gängdjup till G3/4 hål (R)
            kv["TmpS3R"] = "28";
            kv["SumS3R"] = "(R) 28";
            kv["TmpS3RTol"] = TmpKon05;
            kv["SumS3RTol"] = "± " + TmpKon05;

            // Lagerlägesdiameter (O)
            decimal[] TmpS4OLista = { 150, 160, 180, 200, 215, 230, 250, 270, 290, 310, 320, 340, 360, 400, 440 };
            decimal TmpS4O = Pick(TmpS4OLista, tmpTypLista);
            kv["TmpS4O"] = Num(TmpS4O);
            kv["SumS4O"] = "(O) " + Num(TmpS4O);
            kv["TmpS4OTol"] = FmtFixed(TolS4OPlus(TmpS4O), 3);
            kv["SumS4OTol"] = "+ " + kv["TmpS4OTol"] + " [3]";
            kv["TmpS4OTolN"] = FmtFixed(TolS4OMinus(TmpS4O), 3);
            kv["SumS4OTolN"] = "+ " + kv["TmpS4OTolN"] + " [2]";

            // Lagerlägesbredd (J)
            decimal[] TmpS4JLista = { 46, 50, 60, 71, 82, 86, 90, 93, 104, 114, 114, 120, 126, 136, 148 };
            decimal TmpS4J = Pick(TmpS4JLista, tmpTypLista);
            kv["TmpS4J"] = Num(TmpS4J);
            kv["SumS4J"] = "(J) " + Num(TmpS4J);
            kv["TmpS4JTol"] = FmtFixed(TolH12(TmpS4J), 3);
            kv["SumS4JTol"] = "+ " + kv["TmpS4JTol"] + " [3]";
            kv["TmpS4JTolN"] = TmpKon0;
            kv["SumS4JTolN"] = "- " + TmpKon0 + " [2]";

            // Släppningsdiameter lagerläge (S)
            decimal TmpS4S = tmpTypNum < 20 ? 3m : 5m;
            kv["TmpS4S"] = Num(TmpS4S);
            kv["SumS4S"] = "(S) " + Num(TmpS4S);
            kv["TmpS4STol"] = TmpKon1;
            kv["SumS4STol"] = "+ " + TmpKon1;
            kv["TmpS4STolN"] = TmpKon0;
            kv["SumS4STolN"] = "- " + TmpKon0;

            // Släppning lagerläge (T)
            kv["TmpS4T"] = FmtFixed(0.5m, 3);
            kv["SumS4T"] = "(T) " + kv["TmpS4T"];
            kv["TmpS4TTol"] = TmpKon03;
            kv["SumS4TTol"] = "+ " + TmpKon03;
            kv["TmpS4TTolN"] = TmpKon0;
            kv["SumS4TTolN"] = "- " + TmpKon0;

            // Bredd profil (I)
            decimal[] TmpS4ILista = { 118, 122, 144, 171, 206, 206, 210, 210, 230, 263, 263, 269, 275, 290, 301 };
            decimal TmpS4I = Pick(TmpS4ILista, tmpTypLista);
            kv["TmpS4I"] = Num(TmpS4I);
            kv["SumS4I"] = "(I) " + Num(TmpS4I);
            kv["TmpS4ITol"] = tmpTypNum < 40 ? TmpKon03 : TmpKon05;
            kv["SumS4ITol"] = "± " + kv["TmpS4ITol"];

            // Bredd profil (Q)
            decimal[] TmpS4QLista = { 15.3m, 16.3m, 13.3m, 13.8m, 12.8m, 16.3m, 16.3m, 16.3m, 14.8m, 14m, 19m, 20m, 19.9m, 19.9m, 19.9m };
            decimal TmpS4Q = Pick(TmpS4QLista, tmpTypLista);
            kv["TmpS4Q"] = Num(TmpS4Q);
            kv["SumS4Q"] = "(Q) " + Num(TmpS4Q);
            kv["TmpS4QTol"] = TmpKon02;
            kv["SumS4QTol"] = "± " + TmpKon02;

            // Bredd spår (P)
            kv["TmpS4P"] = Num(4.9m);
            kv["SumS4P"] = "(P) " + Num(4.9m);
            kv["TmpS4PTol"] = TmpKon02;
            kv["SumS4PTol"] = "+ " + TmpKon02 + " [3]";
            kv["TmpS4PTolN"] = TmpKon0;
            kv["SumS4PTolN"] = "- " + TmpKon0 + " [3]";

            // Axeldiameter profil (K)
            decimal[] TmpS4KLista = { 98, 102, 114, 122, 137, 147, 162, 172, 180, 197, 207, 223, 230, 258, 273 };
            decimal TmpS4K = Pick(TmpS4KLista, tmpTypLista);
            kv["TmpS4K"] = Num(TmpS4K);
            kv["SumS4K"] = "(K) " + Num(TmpS4K);
            kv["TmpS4KTol"] = FmtFixed(TolH10(TmpS4K), 3);
            kv["SumS4KTol"] = "+ " + kv["TmpS4KTol"] + " [3]";
            kv["TmpS4KTolN"] = TmpKon0;
            kv["SumS4KTolN"] = "- " + TmpKon0 + " [3]";

            // Diameter profil (L)
            decimal[] TmpS4LLista = { 120, 130, 144, 148, 164, 171, 196, 206, 221, 231, 241, 261, 277, 305, 320 };
            decimal TmpS4L = Pick(TmpS4LLista, tmpTypLista);
            kv["TmpS4L"] = Num(TmpS4L);
            kv["SumS4L"] = "(L) " + Num(TmpS4L);
            kv["TmpS4LTol"] = FmtFixed(TolH12(TmpS4L), 3);
            kv["SumS4LTol"] = "+ " + kv["TmpS4LTol"] + " [3]";
            kv["TmpS4LTolN"] = TmpKon0;
            kv["SumS4LTolN"] = "- " + TmpKon0 + " [3]";

            // Diameter profil (M)
            decimal[] TmpS4MLista = { 126, 130, 144, 154, 170, 177, 202, 212, 227, 237, 247, 267, 283, 311, 326 };
            decimal TmpS4M = Pick(TmpS4MLista, tmpTypLista);
            kv["TmpS4M"] = Num(TmpS4M);
            kv["SumS4M"] = "(M) " + Num(TmpS4M);
            kv["TmpS4MTol"] = FmtFixed(TolH12(TmpS4M), 3);
            kv["SumS4MTol"] = "+ " + kv["TmpS4MTol"] + " [3]";
            kv["TmpS4MTolN"] = TmpKon0;
            kv["SumS4MTolN"] = "- " + TmpKon0 + " [3]";

            // Diameter profil (N)
            decimal[] TmpS4NLista = { 141, 145, 159, 169, 185, 192, 217, 227, 242, 262, 272, 292, 308, 336, 351 };
            decimal TmpS4N = Pick(TmpS4NLista, tmpTypLista);
            kv["TmpS4N"] = Num(TmpS4N);
            kv["SumS4N"] = "(N) " + Num(TmpS4N);
            kv["TmpS4NTol"] = FmtFixed(TolH10(TmpS4N), 3);
            kv["SumS4NTol"] = "+ " + kv["TmpS4NTol"] + " [3]";
            kv["TmpS4NTolN"] = TmpKon0;
            kv["SumS4NTolN"] = "- " + TmpKon0 + " [3]";

            // -----------------------------------------------------------------
            // RA / WT / FAS / RADIER / KAST
            // -----------------------------------------------------------------
            kv["TmpRa32"] = "3.2";
            kv["TmpRa63"] = "6.3";
            kv["SumS1Ra32"] = "3.2";
            kv["SumS1Ra63"] = "6.3 [3]";
            kv["SumS2Ra32"] = "3.2";
            kv["SumS3Ra32"] = "3.2";
            kv["SumS3Ra32a"] = "3.2";
            kv["SumS4Ra32"] = "3.2";

            kv["TmpWt35"] = "35";
            kv["SumS1Wt35"] = "Wt 35";
            kv["TmpWt20"] = "20";
            kv["SumS1Wt20"] = "Wt 20";
            kv["SumS2Wt20"] = "Wt 20";

            kv["SumV45"] = "45°";
            kv["SumV35"] = "35°";
            kv["TmpFas1"] = "1";
            kv["SumFas1"] = "1x45°";
            kv["SumS4Rmax"] = "R max 0.5";
            kv["SumS4R12"] = "R max 1.2 (2x)";
            kv["SumS4R12a"] = "R max 1.2 (4x)";

            kv["TmpS4Ks"] = TmpKon02;
            kv["SumS4Ks"] = TmpKon02;

            kv["SumM4_10"] = tmpTypNum < 33 ? "" : "";
            kv["SumB4_10"] = tmpTypNum < 33 ? "" : "";

            // -----------------------------------------------------------------
            // MASKINVAL
            // -----------------------------------------------------------------
            bool okumaTrevisan = string.Equals(maskinVal, "OKUMA MA600/Trevisan DS 450", StringComparison.OrdinalIgnoreCase);

            string tmpMaskinVal = okumaTrevisan ? "" : "INGET MASKINVAL GJORD";
            string tmpMaskinValS1 = okumaTrevisan ? "OKUMA MA600" : "";
            string tmpMaskinValS2 = okumaTrevisan ? "OKUMA MA600" : "";
            string tmpMaskinValS3 = okumaTrevisan ? "Trevisan DS 450" : "";
            string tmpMaskinValS4 = okumaTrevisan ? "Trevisan DS 450" : "";

            kv["TmpMaskinVal"] = tmpMaskinVal;
            kv["TmpMaskinValS1"] = tmpMaskinValS1;
            kv["TmpMaskinValS2"] = tmpMaskinValS2;
            kv["TmpMaskinValS3"] = tmpMaskinValS3;
            kv["TmpMaskinValS4"] = tmpMaskinValS4;

            kv["SumMaskinValS1"] = "OP10 - " + tmpMaskinValS1 + tmpMaskinVal + " - Fräsning underhalva";
            kv["SumMaskinValS2"] = "OP20 - " + tmpMaskinValS2 + tmpMaskinVal + " - Fräsning överhalva";
            kv["SumMaskinValS3"] = "OP30 - " + tmpMaskinValS3 + tmpMaskinVal + " - Svarvning sid. 1";
            kv["SumMaskinValS4"] = "OP30 - " + tmpMaskinValS4 + tmpMaskinVal + " - Svarvning sid. 2";

            // -----------------------------------------------------------------
            // MÄTFREKVENSER
            // -----------------------------------------------------------------
            Set(kv, "SumF1_1", okumaTrevisan ? "Inst." : "");
            Set(kv, "SumF1_2", okumaTrevisan ? "Inst." : "");
            Set(kv, "SumF1_3", okumaTrevisan ? "Inst." : "");
            Set(kv, "SumF1_4", okumaTrevisan ? "1/Skift" : "");
            Set(kv, "SumF1_5", okumaTrevisan ? "Inst." : "");
            Set(kv, "SumF1_6", okumaTrevisan ? "Inst." : "");
            Set(kv, "SumF1_7", okumaTrevisan ? "Inst." : "");
            Set(kv, "SumF1_8", okumaTrevisan ? "1/1" : "");
            Set(kv, "SumF1_9", okumaTrevisan ? "Inst." : "");
            Set(kv, "SumF1_10", okumaTrevisan ? "Inst." : "");
            Set(kv, "SumF1_11", okumaTrevisan ? "Inst." : "");
            Set(kv, "SumF1_12", okumaTrevisan ? "1/Skift" : "");

            Set(kv, "SumF2_1", okumaTrevisan ? "Inst." : "");
            Set(kv, "SumF2_2", okumaTrevisan ? "Inst." : "");
            Set(kv, "SumF2_3", okumaTrevisan ? "Inst." : "");
            Set(kv, "SumF2_4", okumaTrevisan ? "1/Skift & inst." : "");
            Set(kv, "SumF2_5", okumaTrevisan ? "1/Skift & inst." : "");
            Set(kv, "SumF2_6", okumaTrevisan ? "1/Skift & inst." : "");
            Set(kv, "SumF2_7", okumaTrevisan ? "1/Skift & inst." : "");
            Set(kv, "SumF2_8", okumaTrevisan ? "Inst." : "");
            Set(kv, "SumF2_9", okumaTrevisan ? "1/1" : "");
            Set(kv, "SumF2_10", okumaTrevisan ? "Inst." : "");
            Set(kv, "SumF2_11", okumaTrevisan ? "Inst." : "");
            Set(kv, "SumF2_12", okumaTrevisan ? "1/Skift" : "");
            Set(kv, "SumF2_13", okumaTrevisan ? "" : "");

            Set(kv, "SumF3_1", okumaTrevisan ? "Inst." : "");
            Set(kv, "SumF3_2", okumaTrevisan ? "Inst." : "");
            Set(kv, "SumF3_3", okumaTrevisan ? "Inst." : "");
            Set(kv, "SumF3_4", okumaTrevisan ? "Inst." : "");
            Set(kv, "SumF3_5", okumaTrevisan ? "Inst." : "");
            Set(kv, "SumF3_6", okumaTrevisan ? "1/Skift" : "");
            Set(kv, "SumF3_7", okumaTrevisan ? "Inst." : "");
            Set(kv, "SumF3_8", okumaTrevisan ? "1/Skift" : "");
            Set(kv, "SumF3_9", okumaTrevisan ? "1/Skift & inst." : "");
            Set(kv, "SumF3_10", okumaTrevisan ? "1/Skift & inst." : "");

            Set(kv, "SumF4_1", okumaTrevisan ? "1/1" : "");
            Set(kv, "SumF4_2", okumaTrevisan ? "1/1" : "");
            Set(kv, "SumF4_3", okumaTrevisan ? "Inst." : "");
            Set(kv, "SumF4_4", okumaTrevisan ? "Inst." : "");
            Set(kv, "SumF4_5", okumaTrevisan ? "1/Skift" : "");
            Set(kv, "SumF4_6", okumaTrevisan ? "1/Skift" : "");
            Set(kv, "SumF4_7", okumaTrevisan ? "1/Skift" : "");
            Set(kv, "SumF4_8", okumaTrevisan ? "1/5" : "");
            Set(kv, "SumF4_9", okumaTrevisan ? "1/Skift & inst." : "");
            Set(kv, "SumF4_10", okumaTrevisan ? "" : "");

            // -----------------------------------------------------------------
            // MÄTDON
            // -----------------------------------------------------------------
            Set(kv, "SumD1_1", okumaTrevisan ? "Höjdmätningsapparat" : "");
            Set(kv, "SumD1_2", okumaTrevisan ? "Skjutmått" : "");
            Set(kv, "SumD1_3", okumaTrevisan ? "Skjutmått" : "");
            Set(kv, "SumD1_4", okumaTrevisan ? "Gängtolk min/max" : "");
            Set(kv, "SumD1_5", okumaTrevisan ? "Skjutmått" : "");
            Set(kv, "SumD1_6", okumaTrevisan ? "Skjutmått/gängtolk" : "");
            Set(kv, "SumD1_7", okumaTrevisan ? "Skjutmått" : "");
            Set(kv, "SumD1_8", okumaTrevisan ? "Håltolk" : "");
            Set(kv, "SumD1_9", okumaTrevisan ? "Stiftdjupsmätare" : "");
            Set(kv, "SumD1_10", okumaTrevisan ? "Höjdmätningsapparat" : "");
            Set(kv, "SumD1_11", okumaTrevisan ? "Bladmått" : "");
            Set(kv, "SumD1_12", okumaTrevisan ? "Ytjämnhetsmätare" : "");

            Set(kv, "SumD2_1", okumaTrevisan ? "Skjutmått" : "");
            Set(kv, "SumD2_2", okumaTrevisan ? "Skjutmått" : "");
            Set(kv, "SumD2_3", okumaTrevisan ? "Gängtolk" : "");
            Set(kv, "SumD2_4", okumaTrevisan ? "Skjutmått" : "");
            Set(kv, "SumD2_5", okumaTrevisan ? "Skjutmått/Tolk" : "");
            Set(kv, "SumD2_6", okumaTrevisan ? "Gängtolk" : "");
            Set(kv, "SumD2_7", okumaTrevisan ? "Skjutmått/Tolk" : "");
            Set(kv, "SumD2_8", okumaTrevisan ? "Skjutmått" : "");
            Set(kv, "SumD2_9", okumaTrevisan ? "Håltolk" : "");
            Set(kv, "SumD2_10", okumaTrevisan ? "Skjutmått" : "");
            Set(kv, "SumD2_11", okumaTrevisan ? "Bladmått" : "");
            Set(kv, "SumD2_12", okumaTrevisan ? "Ytjämnhetsmätare" : "");
            Set(kv, "SumD2_13", okumaTrevisan ? "" : "");

            Set(kv, "SumD3_1", okumaTrevisan ? "Höjdmätningsapparat" : "");
            Set(kv, "SumD3_2", okumaTrevisan ? "Skjutmått" : "");
            Set(kv, "SumD3_3", okumaTrevisan ? "Skjutmått" : "");
            Set(kv, "SumD3_4", okumaTrevisan ? "Höjdmätningsapparat" : "");
            Set(kv, "SumD3_5", okumaTrevisan ? "Skjutmått" : "");
            Set(kv, "SumD3_6", okumaTrevisan ? "Skjutmått/gängtolk" : "");
            Set(kv, "SumD3_7", okumaTrevisan ? "Skjutmått" : "");
            Set(kv, "SumD3_8", okumaTrevisan ? "Skjutmått/gängtolk" : "");
            Set(kv, "SumD3_9", okumaTrevisan ? "Gängtolk min/max" : "");
            Set(kv, "SumD3_10", okumaTrevisan ? "Ytjämnhetsmätare" : "");

            Set(kv, "SumD4_1", okumaTrevisan ? "Subito" : "");
            Set(kv, "SumD4_2", okumaTrevisan ? "Breddmått min/max" : "");
            Set(kv, "SumD4_3", okumaTrevisan ? "Passbit" : "");
            Set(kv, "SumD4_4", okumaTrevisan ? "Okulärt" : "");
            Set(kv, "SumD4_5", okumaTrevisan ? "Passbit/Skjutmått/Djupmått" : "");
            Set(kv, "SumD4_6", okumaTrevisan ? "Passbit" : "");
            Set(kv, "SumD4_7", okumaTrevisan ? "Skjutmått/Inv. mikrometer" : "");
            Set(kv, "SumD4_8", okumaTrevisan ? "Ytjämnhetsmätare" : "");
            Set(kv, "SumD4_9", okumaTrevisan ? "Skjutmått" : "");
            Set(kv, "SumD4_10", okumaTrevisan ? "" : "");

            // -----------------------------------------------------------------
            // ANMÄRKNINGSFÄLT
            // -----------------------------------------------------------------
            Set(kv, "SumAF1_1", okumaTrevisan ? "" : "");
            Set(kv, "SumAF1_2", okumaTrevisan ? "" : "");
            Set(kv, "SumAF1_3", okumaTrevisan ? "" : "");
            Set(kv, "SumAF1_4", okumaTrevisan ? "" : "");
            Set(kv, "SumAF1_5", okumaTrevisan ? "" : "");
            Set(kv, "SumAF1_6", okumaTrevisan ? "" : "");
            Set(kv, "SumAF1_7", okumaTrevisan ? "" : "");
            Set(kv, "SumAF1_8", okumaTrevisan ? "Får ej försänkas" : "");
            Set(kv, "SumAF1_9", okumaTrevisan ? "" : "");
            Set(kv, "SumAF1_10", okumaTrevisan ? "Med U.H på planskiva" : "");
            Set(kv, "SumAF1_11", okumaTrevisan ? "Med U.H på planskiva" : "");
            Set(kv, "SumAF1_12", okumaTrevisan ? "" : "");

            Set(kv, "SumAF2_1", okumaTrevisan ? "Genomgående hål" : "");
            Set(kv, "SumAF2_2", okumaTrevisan ? "" : "");
            Set(kv, "SumAF2_3", okumaTrevisan ? "" : "");
            Set(kv, "SumAF2_4", okumaTrevisan ? "" : "");
            Set(kv, "SumAF2_5", okumaTrevisan ? "" : "");
            Set(kv, "SumAF2_6", okumaTrevisan ? "" : "");
            Set(kv, "SumAF2_7", okumaTrevisan ? "" : "");
            Set(kv, "SumAF2_8", okumaTrevisan ? "" : "");
            Set(kv, "SumAF2_9", okumaTrevisan ? "" : "");
            Set(kv, "SumAF2_10", okumaTrevisan ? "" : "");
            Set(kv, "SumAF2_11", okumaTrevisan ? "Med Ö.H på planskiva" : "");
            Set(kv, "SumAF2_12", okumaTrevisan ? "" : "");
            Set(kv, "SumAF2_13", okumaTrevisan ? "" : "");

            Set(kv, "SumAF3_1", okumaTrevisan ? "Indikeras från LL" : "");
            Set(kv, "SumAF3_2", okumaTrevisan ? "" : "");
            Set(kv, "SumAF3_3", okumaTrevisan ? "" : "");
            Set(kv, "SumAF3_4", okumaTrevisan ? "" : "");
            Set(kv, "SumAF3_5", okumaTrevisan ? "" : "");
            Set(kv, "SumAF3_6", okumaTrevisan ? "" : "");
            Set(kv, "SumAF3_7", okumaTrevisan ? "" : "");
            Set(kv, "SumAF3_8", okumaTrevisan ? "" : "");
            Set(kv, "SumAF3_9", okumaTrevisan ? "" : "");
            Set(kv, "SumAF3_10", okumaTrevisan ? "" : "");

            Set(kv, "SumAF4_1", okumaTrevisan ? "Kontrolleras i mätbänk" : "");
            Set(kv, "SumAF4_2", okumaTrevisan ? tmpBet + " " + Num(TmpS4J) + " min/max" : "");
            Set(kv, "SumAF4_3", okumaTrevisan ? "Mäts med isärtagna halvor" : "");
            Set(kv, "SumAF4_4", okumaTrevisan ? "Mäts med isärtagna halvor" : "");
            Set(kv, "SumAF4_5", okumaTrevisan ? "Passbit sätts i spår, isärtagning" : "");
            Set(kv, "SumAF4_6", okumaTrevisan ? "Isärtagning" : "");
            Set(kv, "SumAF4_7", okumaTrevisan ? "" : "");
            Set(kv, "SumAF4_8", okumaTrevisan ? "" : "");
            Set(kv, "SumAF4_9", okumaTrevisan ? "" : "");
            Set(kv, "SumAF4_10", okumaTrevisan ? "" : "");

            // -----------------------------------------------------------------
            // ÖVRIG TEXT
            // -----------------------------------------------------------------
            kv["SumTS2FS"] = "Får ej försänkas";
            kv["SumTextS1"] = "Kontrolleras enl. styrplan. Vid skärbyte ska alla mått kontrollers";
            kv["SumTextS2"] = kv["SumTextS1"];
            kv["SumTextS3"] = kv["SumTextS1"];
            kv["SumTextS4"] = kv["SumTextS1"];

            // -----------------------------------------------------------------
            // RITNINGSNUMMER / DOKUMENT
            // -----------------------------------------------------------------
            kv["SumPrdritS1"] = "Produktritning: " + tmpBet;
            kv["SumPrdritS2"] = kv["SumPrdritS1"];
            kv["SumPrdritS3"] = kv["SumPrdritS1"];
            kv["SumPrdritS4"] = kv["SumPrdritS1"];

            kv["SumArbInstGjgS1"] = "Gjutgods: A3.022";
            kv["SumArbInstGjgS2"] = kv["SumArbInstGjgS1"];
            kv["SumArbInstGjgS3"] = kv["SumArbInstGjgS1"];
            kv["SumArbInstGjgS4"] = kv["SumArbInstGjgS1"];

            kv["SumKvStPlS1"] = "Kvalitetstyrning: K1.07-17";
            kv["SumKvStPlS2"] = kv["SumKvStPlS1"];
            kv["SumKvStPlS3"] = kv["SumKvStPlS1"];
            kv["SumKvStPlS4"] = kv["SumKvStPlS1"];

            return kv;
        }

        // ---------------------------------------------------------------------
        // HELPERS
        // ---------------------------------------------------------------------

        private static void Set(Dictionary<string, string> kv, string key, string value)
        {
            kv[key] = value ?? string.Empty;
        }

        private static string[] SplitTokens(string s)
        {
            if (string.IsNullOrEmpty(s)) return Array.Empty<string>();
            return s.Split(new[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static string Left(string s, int n)
        {
            if (string.IsNullOrEmpty(s) || n <= 0) return string.Empty;
            return s.Length <= n ? s : s.Substring(0, n);
        }

        private static string Right(string s, int n)
        {
            if (string.IsNullOrEmpty(s) || n <= 0) return string.Empty;
            return s.Length <= n ? s : s.Substring(s.Length - n, n);
        }

        private static int IndexOf(string[] arr, string value)
        {
            if (arr == null || arr.Length == 0) return -1;
            for (int i = 0; i < arr.Length; i++)
            {
                if (string.Equals(arr[i], value, StringComparison.OrdinalIgnoreCase))
                    return i + 1; // Lotus style 1-based
            }
            return -1;
        }

        private static decimal Pick(decimal[] source, int oneBasedIndex)
        {
            int idx = oneBasedIndex - 1;
            if (source == null || idx < 0 || idx >= source.Length)
                throw new IndexOutOfRangeException("List index outside valid range.");
            return source[idx];
        }

        private static string Num(decimal value)
        {
            return value.ToString("0.###", Inv);
        }

        private static string FmtFixed(decimal value, int decimals)
        {
            return value.ToString("F" + decimals.ToString(Inv), Inv);
        }

        private static decimal TolGeneral(decimal value)
        {
            if (value < 6.01m) return 0.1m;
            if (value < 30.01m) return 0.2m;
            if (value < 120.01m) return 0.3m;
            if (value < 400.01m) return 0.5m;
            if (value < 1000.01m) return 0.8m;
            if (value < 2000.01m) return 1.2m;
            return 2.0m;
        }

        private static decimal TolJs11(decimal value)
        {
            if (value < 3.01m) return 0.030m;
            if (value < 6.01m) return 0.037m;
            if (value < 10.01m) return 0.045m;
            if (value < 18.01m) return 0.055m;
            if (value < 30.01m) return 0.065m;
            if (value < 50.01m) return 0.080m;
            if (value < 80.01m) return 0.095m;
            if (value < 120.01m) return 0.110m;
            if (value < 180.01m) return 0.125m;
            if (value < 250.01m) return 0.145m;
            if (value < 315.01m) return 0.160m;
            if (value < 400.01m) return 0.180m;
            if (value < 500.01m) return 0.200m;
            if (value < 630.01m) return 0.220m;
            if (value < 800.01m) return 0.250m;
            if (value < 1000.01m) return 0.280m;
            if (value < 1250.01m) return 0.330m;
            if (value < 1600.01m) return 0.390m;
            if (value < 2000.01m) return 0.460m;
            if (value < 2500.01m) return 0.550m;
            return 0.675m;
        }

        private static decimal TolJs13(decimal value)
        {
            if (value < 3.01m) return 0.070m;
            if (value < 6.01m) return 0.090m;
            if (value < 10.01m) return 0.110m;
            if (value < 18.01m) return 0.135m;
            if (value < 30.01m) return 0.165m;
            if (value < 50.01m) return 0.195m;
            if (value < 80.01m) return 0.230m;
            if (value < 120.01m) return 0.270m;
            if (value < 180.01m) return 0.315m;
            if (value < 250.01m) return 0.360m;
            if (value < 315.01m) return 0.405m;
            if (value < 400.01m) return 0.445m;
            if (value < 500.01m) return 0.485m;
            if (value < 630.01m) return 0.550m;
            if (value < 800.01m) return 0.625m;
            if (value < 1000.01m) return 0.700m;
            if (value < 1250.01m) return 0.825m;
            if (value < 1600.01m) return 0.975m;
            if (value < 2000.01m) return 1.150m;
            if (value < 2500.01m) return 1.400m;
            return 1.650m;
        }

        private static decimal TolH15(decimal value)
        {
            if (value < 10.01m) return 0.580m;
            if (value < 18.01m) return 0.700m;
            if (value < 30.01m) return 0.840m;
            return 1.000m;
        }

        private static decimal TolH15Wide(decimal value)
        {
            if (value < 3.01m) return 0.400m;
            if (value < 6.01m) return 0.480m;
            if (value < 10.01m) return 0.580m;
            if (value < 18.01m) return 0.700m;
            if (value < 30.01m) return 0.840m;
            if (value < 50.01m) return 1.000m;
            if (value < 80.01m) return 1.200m;
            if (value < 120.01m) return 1.400m;
            if (value < 180.01m) return 1.600m;
            if (value < 250.01m) return 1.850m;
            if (value < 315.01m) return 2.100m;
            if (value < 400.01m) return 2.300m;
            if (value < 500.01m) return 2.500m;
            return 2.800m;
        }

        private static decimal TolH10(decimal value)
        {
            if (value < 3.01m) return 0.040m;
            if (value < 6.01m) return 0.048m;
            if (value < 10.01m) return 0.058m;
            if (value < 18.01m) return 0.070m;
            if (value < 30.01m) return 0.084m;
            if (value < 50.01m) return 0.100m;
            if (value < 80.01m) return 0.120m;
            if (value < 120.01m) return 0.140m;
            if (value < 180.01m) return 0.160m;
            if (value < 250.01m) return 0.185m;
            if (value < 315.01m) return 0.210m;
            if (value < 400.01m) return 0.230m;
            if (value < 500.01m) return 0.250m;
            return 0.280m;
        }

        private static decimal TolH12(decimal value)
        {
            if (value < 3.01m) return 0.100m;
            if (value < 6.01m) return 0.120m;
            if (value < 10.01m) return 0.150m;
            if (value < 18.01m) return 0.180m;
            if (value < 30.01m) return 0.210m;
            if (value < 50.01m) return 0.250m;
            if (value < 80.01m) return 0.300m;
            if (value < 120.01m) return 0.350m;
            if (value < 180.01m) return 0.400m;
            if (value < 250.01m) return 0.460m;
            if (value < 315.01m) return 0.520m;
            if (value < 400.01m) return 0.570m;
            if (value < 500.01m) return 0.630m;
            if (value < 630.01m) return 0.700m;
            if (value < 800.01m) return 0.800m;
            if (value < 1000.01m) return 0.900m;
            if (value < 1250.01m) return 1.050m;
            if (value < 1600.01m) return 1.250m;
            if (value < 2000.01m) return 1.500m;
            if (value < 2500.01m) return 1.750m;
            return 2.100m;
        }

        private static decimal TolS4OPlus(decimal value)
        {
            if (value < 3.01m) return 0.016m;
            if (value < 6.01m) return 0.020m;
            if (value < 10.01m) return 0.024m;
            if (value < 18.01m) return 0.029m;
            if (value < 30.01m) return 0.035m;
            if (value < 50.01m) return 0.042m;
            if (value < 80.01m) return 0.051m;
            if (value < 120.01m) return 0.059m;
            if (value < 180.01m) return 0.068m;
            if (value < 250.01m) return 0.079m;
            if (value < 315.01m) return 0.088m;
            if (value < 400.01m) return 0.098m;
            if (value < 500.01m) return 0.108m;
            if (value < 630.01m) return 0.148m;
            if (value < 800.01m) return 0.168m;
            if (value < 1000.01m) return 0.190m;
            if (value < 1250.01m) return 0.225m;
            if (value < 1600.01m) return 0.265m;
            if (value < 2000.01m) return 0.320m;
            if (value < 2500.01m) return 0.370m;
            return 0.450m;
        }

        private static decimal TolS4OMinus(decimal value)
        {
            if (value < 3.01m) return 0.006m;
            if (value < 6.01m) return 0.008m;
            if (value < 10.01m) return 0.009m;
            if (value < 18.01m) return 0.011m;
            if (value < 30.01m) return 0.014m;
            if (value < 50.01m) return 0.017m;
            if (value < 80.01m) return 0.021m;
            if (value < 120.01m) return 0.024m;
            if (value < 180.01m) return 0.028m;
            if (value < 250.01m) return 0.033m;
            if (value < 315.01m) return 0.036m;
            if (value < 400.01m) return 0.041m;
            if (value < 500.01m) return 0.045m;
            if (value < 630.01m) return 0.078m;
            if (value < 800.01m) return 0.088m;
            if (value < 1000.01m) return 0.100m;
            if (value < 1250.01m) return 0.120m;
            if (value < 1600.01m) return 0.140m;
            if (value < 2000.01m) return 0.170m;
            if (value < 2500.01m) return 0.195m;
            return 0.240m;
        }

        private static string GetStringField(APIRequest req, string key)
        {
            if (req == null || string.IsNullOrWhiteSpace(key)) return string.Empty;

            string value;
            if (TryGetFromDictionaries(req, key, out value))
                return value ?? string.Empty;

            return string.Empty;
        }

        private static bool TryGetFromDictionaries(object obj, string key, out string value)
        {
            value = null;
            if (obj == null) return false;

            var t = obj.GetType();
            foreach (var p in t.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!p.CanRead) continue;

                object pv;
                try { pv = p.GetValue(obj, null); }
                catch { continue; }

                if (pv == null) continue;

                var dictSS = pv as IDictionary<string, string>;
                if (dictSS != null)
                {
                    if (dictSS.TryGetValue(key, out value)) return true;

                    foreach (var kvp in dictSS)
                    {
                        if (string.Equals(kvp.Key ?? string.Empty, key, StringComparison.OrdinalIgnoreCase))
                        {
                            value = kvp.Value;
                            return true;
                        }
                    }
                }

                var dictSO = pv as IDictionary<string, object>;
                if (dictSO != null)
                {
                    object ov;
                    if (dictSO.TryGetValue(key, out ov))
                    {
                        value = ov != null ? ov.ToString() : string.Empty;
                        return true;
                    }

                    foreach (var kvp in dictSO)
                    {
                        if (string.Equals(kvp.Key ?? string.Empty, key, StringComparison.OrdinalIgnoreCase))
                        {
                            value = kvp.Value != null ? kvp.Value.ToString() : string.Empty;
                            return true;
                        }
                    }
                }

                var nonGen = pv as IDictionary;
                if (nonGen != null)
                {
                    foreach (DictionaryEntry de in nonGen)
                    {
                        if (de.Key == null) continue;
                        if (string.Equals(de.Key.ToString(), key, StringComparison.OrdinalIgnoreCase))
                        {
                            value = de.Value != null ? de.Value.ToString() : string.Empty;
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
