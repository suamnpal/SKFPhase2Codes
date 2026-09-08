using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class MUTTRAR_HMT_HML_62_180_Svarv_Borr_Fras_OP1_3 : ITemplateCalculations
    {
        private const int TmpDagar = 14;

        private static readonly string[] SvarvMachineList = { "LB45", "EMAG", "VTR-160", "MacTurn 550" };
        private static readonly string[] MatMachineList = { "LB45", "MaxMuller/K&T", "VTR-160", "MacTurn 550" };

        private static readonly string[] TypListHM =
        {
            "62","64","66","68","70","72","74","76","78","80",
            "82","84","86","88","90","92","94","96","98","100",
            "102","104","106","108","110","112","116","120","126","130",
            "134","138","142","146","150","155","160","165","170","175",
            "180"
        };

        private static readonly string[] TypListHML =
        {
            "62","64","66","68","69","70","72","73","74","76",
            "77","78","80","82","84","86","88","90","92","94",
            "96","98","100","102","104","106","108","110","112","116",
            "120","126","130","134","138","142","146","150","155","160",
            "165","170","175","180"
        };

        private static readonly double[] DListHM =
        {
            390,400,420,440,450,460,470,490,500,520,
            520,540,540,560,580,580,600,620,630,630,
            650,670,670,680,700,710,730,750,800,820,
            850,870,900,920,950,980,1000,1030,1060,1090,
            1120
        };

        private static readonly double[] DListHML =
        {
            370,380,390,400,410,410,420,430,440,450,
            450,460,470,480,490,500,520,520,540,540,
            560,580,580,600,600,630,630,650,650,680,
            700,730,760,780,800,830,850,870,900,920,
            950,980,1000,1030
        };

        private static readonly double[] D5ListHM =
        {
            350,360,380,400,410,420,430,450,460,470,
            470,490,490,510,530,540,550,560,580,580,
            590,610,610,620,640,650,670,690,730,750,
            775,800,825,850,875,900,925,950,975,1000,
            1030
        };

        private static readonly double[] D5ListHML =
        {
            346,356,366,376,384,384,394,404,414,422,
            422,432,442,452,462,472,490,490,510,510,
            530,550,550,570,570,590,590,610,610,640,
            660,690,720,740,760,780,800,820,850,870,
            900,925,950,975
        };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;
            string breddTempo1 = GetRequestField(req, "BreddTempo1");

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = subject.ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");

            bool tmpL = tmpFormat.IndexOf("L", StringComparison.Ordinal) >= 0;

            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : "";

            string tmpTyp = tmpBet2;
            int typInt = TryParseInt(tmpTyp);

            int typLista = tmpL ? GetMember(tmpTyp, TypListHML) : GetMember(tmpTyp, TypListHM);

            string tmpRit = !tmpL
                ? (typInt < 79 ? "224673" : "222340")
                : (typInt < 95 ? "222755" : "222756");

            kv["SumRitningsnr"] = tmpRit;
            kv["SumRitningsnr2"] = tmpRit;
            kv["SumTolRit"] = "1432008";
            kv["SumTolRit2"] = "1432008";
            kv["SumGTolRit"] = "7430181";
            kv["SumYtRit"] = "7430184";
            kv["SumYtRit2"] = "7430184";

            double tmpkd = (typInt / 2.0) * 10.0;

            int tmpP = typInt < 61 ? 4
                     : typInt < 101 ? 5
                     : typInt < 139 ? 6
                     : typInt < 181 ? 7
                     : 8;

            kv["SumP"] = "(P) " + tmpP.ToString(CultureInfo.InvariantCulture);
            kv["SumGänga"] = "Tr " + Num(tmpkd) + "x" + tmpP.ToString(CultureInfo.InvariantCulture);
            kv["SumGMall"] = "Tr x " + tmpP.ToString(CultureInfo.InvariantCulture);

            double tmpd4 = (tmpP == 4 || tmpP == 5) ? tmpkd + 0.5 : tmpkd + 1;
            kv["Sumd4"] = "(d4) " + Num(tmpd4);
            double tmpd4Tol = tmpd4 < 6 ? 0.1
                            : tmpd4 < 30 ? 0.2
                            : tmpd4 < 120 ? 0.3
                            : tmpd4 < 400 ? 0.5
                            : tmpd4 < 1000 ? 0.8
                            : tmpd4 < 2000 ? 1.2
                            : 2;
            kv["Sumd4Tol"] = "± " + Fmt(tmpd4Tol);

            double tmpd2 = tmpP == 4 ? tmpkd - 2
                         : tmpP == 5 ? tmpkd - 2.5
                         : tmpP == 6 ? tmpkd - 3
                         : tmpP == 7 ? tmpkd - 3.5
                         : tmpkd - 4;
            kv["Sumd2"] = "(d2) " + Num(tmpd2);
            double tmpd2Tol = tmpkd < 301 ? 0.475
                            : tmpkd < 501 ? 0.53
                            : tmpkd < 701 ? 0.6
                            : tmpkd < 901 ? 0.63
                            : 0.71;
            kv["Sumd2Tol"] = "+ " + Fmt(tmpd2Tol);
            kv["Sumd2TolN"] = "-  0";

            double tmpd1 = tmpP == 4 ? tmpkd - 4
                         : tmpP == 5 ? tmpkd - 5
                         : tmpP == 6 ? tmpkd - 6
                         : tmpP == 7 ? tmpkd - 7
                         : tmpkd - 8;
            kv["Sumd1"] = "(d1) " + Num(tmpd1);
            double tmpd1Tol = tmpkd < 301 ? 0.375
                            : tmpkd < 501 ? 0.45
                            : tmpkd < 701 ? 0.5
                            : tmpkd < 901 ? 0.56
                            : 0.63;
            kv["Sumd1Tol"] = "+ " + Fmt(tmpd1Tol);
            kv["Sumd1TolN"] = "-  0";

            double[] dLista = tmpL ? DListHML : DListHM;
            double tmpD = ListValue(dLista, typLista);
            kv["SumD"] = "(D) " + ListText(dLista, typLista);
            kv["SumDTol"] = "+ 0";
            kv["SumDTolN"] = "-  " + H13(tmpD);

            double[] d5Lista = tmpL ? D5ListHML : D5ListHM;
            double tmpd5 = ListValue(d5Lista, typLista);
            kv["Sumd5"] = "(d5) " + ListText(d5Lista, typLista);
            kv["Sumd5Tol"] = "+ 0";
            kv["Sumd5TolN"] = "-  " + H13(tmpd5);

            double tmpd3 = tmpkd < 501 ? tmpkd + 2 : tmpkd + 3;
            kv["Sumd3"] = "(d3) " + Num(tmpd3);
            kv["Sumd3Tol"] = "+ " + H13Fine(tmpd3);
            kv["Sumd3TolN"] = "-  0";

            double tmpB = !tmpL
                ? (typInt < 65 ? 42
                 : typInt < 67 ? 52
                 : typInt < 71 ? 55
                 : typInt < 75 ? 58
                 : typInt < 79 ? 60
                 : typInt < 83 ? 62
                 : typInt < 89 ? 70
                 : typInt < 97 ? 75
                 : typInt < 107 ? 80
                 : typInt < 121 ? 85
                 : typInt == 126 ? 95
                 : typInt == 130 ? 100
                 : typInt < 143 ? 106
                 : typInt < 161 ? 112
                 : typInt < 171 ? 118
                 : 125)
                : (typInt < 65 ? 42
                 : typInt < 73 ? 45
                 : typInt < 79 ? 48
                 : typInt < 87 ? 52
                 : typInt < 99 ? 60
                 : typInt < 109 ? 68
                 : typInt < 127 ? 75
                 : typInt < 139 ? 80
                 : typInt < 176 ? 90
                 : 100);
            kv["SumB"] = "(B) " + Num(tmpB);
            double tmpBTolN = tmpB < 7 ? 0.18
                            : tmpB < 11 ? 0.22
                            : tmpB < 19 ? 0.27
                            : tmpB < 31 ? 0.33
                            : tmpB < 51 ? 0.39
                            : tmpB < 81 ? 0.46
                            : tmpB < 121 ? 0.54
                            : 0.63;
            kv["SumBTol"] = "+ 0";
            kv["SumBTolN"] = "-  " + Fmt(tmpBTolN);

            string tmpR = typInt < 89 ? "3.5"
                        : typInt < 121 ? "4"
                        : typInt < 143 ? "5"
                        : "6";
            kv["SumR"] = "R " + tmpR;

            kv["SumFas"] = "30º";
            kv["SumGF"] = "30º";
            kv["SumIF1"] = "45º";
            kv["SumIF2"] = "45º";

            double tmpt = !tmpL
                ? (tmpkd < 321 ? 12
                 : tmpkd < 371 ? 15
                 : tmpkd < 421 ? 18
                 : tmpkd < 481 ? 20
                 : tmpkd < 551 ? 23
                 : tmpkd < 601 ? 25
                 : tmpkd < 671 ? 28
                 : tmpkd < 731 ? 30
                 : tmpkd < 826 ? 34
                 : 38)
                : (tmpkd < 341 ? 12
                 : tmpkd < 371 ? 13
                 : tmpkd < 431 ? 14
                 : tmpkd < 521 ? 15
                 : tmpkd < 691 ? 20
                 : 25);
            kv["Sumt"] = "(t) " + Num(tmpt);
            double tmptTol = tmpt < 11 ? 1.5
                           : tmpt < 19 ? 1.8
                           : tmpt < 31 ? 2.1
                           : 2.5;
            kv["SumtTol"] = "+ " + Fmt(tmptTol);
            kv["SumtTolN"] = "-  0";

            double tmpS = !tmpL
                ? (tmpkd < 321 ? 24
                 : tmpkd < 371 ? 28
                 : tmpkd < 421 ? 32
                 : tmpkd < 481 ? 36
                 : tmpkd < 551 ? 40
                 : tmpkd < 601 ? 45
                 : tmpkd < 671 ? 50
                 : tmpkd < 731 ? 55
                 : tmpkd < 826 ? 60
                 : 70)
                : (tmpkd < 341 ? 24
                 : tmpkd < 401 ? 28
                 : tmpkd < 471 ? 32
                 : tmpkd < 521 ? 36
                 : tmpkd < 601 ? 40
                 : tmpkd < 671 ? 45
                 : tmpkd < 731 ? 50
                 : tmpkd < 801 ? 55
                 : 60);
            kv["SumS"] = "(S) " + Num(tmpS);
            kv["SumSTol"] = tmpS < 31 ? "± 0.260" : tmpS < 51 ? "± 0.310" : "± 0.370";

            kv["SumRHT"] = "2x R1.6";
            kv["SumRHTTol"] = "± 0.4";

            kv["SumRa32"] = "3.2";

            kv["SumStämpling"] = "Stämplas enl. ritning 7433462";

            string tmpMaskinValS1 = EqualsI(maskinVal, "LB45") ? "LB45"
                                  : EqualsI(maskinVal, "EMAG") ? "EMAG"
                                  : EqualsI(maskinVal, "MaxMuller/K&T") ? "MaxMuller"
                                  : EqualsI(maskinVal, "VTR-160") ? "VTR-160"
                                  : EqualsI(maskinVal, "MacTurn 550") ? "MacTurn 550"
                                  : "";
            string tmpMaskinValS2 = EqualsI(maskinVal, "LB45") ? "LB45"
                                  : EqualsI(maskinVal, "EMAG") ? "EMAG"
                                  : EqualsI(maskinVal, "MaxMuller/K&T") ? "K&T"
                                  : EqualsI(maskinVal, "VTR-160") ? "VTR-160"
                                  : EqualsI(maskinVal, "MacTurn 550") ? "MacTurn 550"
                                  : "";

            bool isSvarv = GetMember(maskinVal, SvarvMachineList) > 0;
            kv["SumMaskinValS1"] = "Maskin: " + tmpMaskinValS1 + (isSvarv ? " - Svarvning" : " - OP1 & 2");
            kv["SumMaskinValS2"] = "Maskin: " + tmpMaskinValS2 + (isSvarv ? " - Borrning & Fräsning" : " - OP3");

            bool isMat = GetMember(maskinVal, MatMachineList) > 0;

            kv["SumF1_1"] = isMat ? "1/1" : "";
            kv["SumF1_2"] = isMat ? "1/2" : "";
            kv["SumF1_3"] = isMat ? "1/5" : "";
            kv["SumF1_4"] = isMat ? "1/5" : "";
            kv["SumF1_5"] = isMat ? "1/5" : "";
            kv["SumF1_6"] = isMat ? "1/5" : "";
            kv["SumF1_7"] = isMat ? "1/3" : "";
            kv["SumF1_8"] = isMat ? "1/5" : "";
            kv["SumF1_9"] = isMat ? "1/3" : "";
            kv["SumF2_1"] = isMat ? "1/5" : "";
            kv["SumF2_2"] = isMat ? "1/5" : "";
            kv["SumF2_3"] = "";

            kv["SumD1_1"] = isMat ? "Multimar" : "";
            kv["SumD1_2"] = isMat ? "Skjutmått/mikrometer" : "";
            kv["SumD1_3"] = isMat ? "Skjutmått" : "";
            kv["SumD1_4"] = isMat ? "Skjutmått" : "";
            kv["SumD1_5"] = isMat ? "Skjutmått" : "";
            kv["SumD1_6"] = isMat ? "Skjutmått" : "";
            kv["SumD1_7"] = isMat ? "Ytjämnhetsmätare" : "";
            kv["SumD1_8"] = isMat ? "Planhet med Egglinjal" : "";
            kv["SumD1_9"] = isMat ? "Gängmall " + kv["SumGMall"] : "";
            kv["SumD2_1"] = isMat ? "Skjutmått" : "";
            kv["SumD2_2"] = isMat ? "Djupmått" : "";
            kv["SumD2_3"] = "";

            string tmpBtempo1 = (breddTempo1 == "0" || string.IsNullOrEmpty(breddTempo1) ? "+ 3" : breddTempo1) + " mm";

            kv["SumAF1_1"] = isMat ? "Kontrolleras i mätbänk " : "";
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = "";
            kv["SumAF1_6"] = isMat ? "Bearbetning i OP1 " + tmpBtempo1 : "";
            if (subject == "HM 66 T" || subject == "HML 77 T")
                kv["SumAF1_6"] = "Bearbetning i OP1 mm";
            if (subject == "HML 82 T" || subject == "HML 94 T")
                kv["SumAF1_6"] = "Bearbetning i OP1 63 mm";
            if (subject == "HML 86 T")
                kv["SumAF1_6"] = "Bearbetning i OP1 65 mm";

            kv["SumAF1_7"] = isMat ? "Övriga Ra värden 6,3 " : "";
            kv["SumAF1_8"] = isMat ? "Vid misstänkt formfel lämnas till mätrum " : "";
            kv["SumAF1_9"] = isMat ? "Mät in/ut-gångar på gängan" : "";
            kv["SumAF2_1"] = "";
            kv["SumAF2_2"] = "";
            kv["SumAF2_3"] = "";

            kv["SumKlEgenskaper"] = "PRODUCTION NUTS & SLEEVES & HOUSINGS\\ALLMÄNKLASSADE EGENSKAPER\\Klassade egenskaper muttrar";
            kv["SumKlEgenskaper2"] = kv["SumKlEgenskaper"];

            kv["SumTextS1"] = "Grader, frifläckar, slagmärken, repor, valkar & andra defekter samt ojämnheter kontrolleras med ögonmått, för övriga mått används skjutmått.";
            kv["SumTextS2"] = "Övriga mått kontrolleras vid inställning";

            return kv;
        }

        private static string ComputePopup(string pub)
        {
            if (string.IsNullOrWhiteSpace(pub)) return "";
            DateTime dt;
            if (!DateTime.TryParse(pub, out dt)) return "";
            DateTime till = dt.AddDays(TmpDagar);
            if (DateTime.Today > till.Date) return "";
            return "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom " + TmpDagar + " dagar)" +
                   "<<LineBreak>><<LineBreak>>" +
                   "<<LineBreak>><<LineBreak>>" +
                   "Information om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument" +
                   "<<LineBreak>><<LineBreak>>" +
                   "Popupruta aktiv till " + till.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        private static string H13(double v)
        {
            if (v < 19) return "0.27";
            if (v < 31) return "0.33";
            if (v < 51) return "0.39";
            if (v < 81) return "0.46";
            if (v < 121) return "0.54";
            if (v < 181) return "0.63";
            if (v < 251) return "0.72";
            if (v < 316) return "0.81";
            if (v < 401) return "0.89";
            if (v < 501) return "0.97";
            if (v < 631) return "1.1";
            if (v < 801) return "1.25";
            if (v < 1000) return "1.4";
            return "1.650";
        }

        private static string H13Fine(double v)
        {
            if (v < 3.01) return "0.140";
            if (v < 6.01) return "0.180";
            if (v < 10.01) return "0.220";
            if (v < 18.01) return "0.270";
            if (v < 30.01) return "0.330";
            if (v < 50.01) return "0.390";
            if (v < 80.01) return "0.460";
            if (v < 120.01) return "0.540";
            if (v < 180.01) return "0.630";
            if (v < 250.01) return "0.720";
            if (v < 315.01) return "0.810";
            if (v < 400.01) return "0.890";
            if (v < 500.01) return "0.970";
            if (v < 630.01) return "1.100";
            if (v < 800.01) return "1.250";
            if (v < 1000.01) return "1.400";
            if (v < 1250.01) return "1.650";
            if (v < 1600.01) return "1.950";
            if (v < 2000.01) return "2.300";
            if (v < 2500.01) return "2.800";
            return "3.300";
        }

        private static string F3(double v)
        {
            return v.ToString("0.000", CultureInfo.InvariantCulture);
        }

        private static double ListValue(double[] list, int index)
        {
            if (list == null || index <= 0 || index > list.Length) return 0;
            return list[index - 1];
        }

        private static string ListText(double[] list, int index)
        {
            if (list == null || index <= 0 || index > list.Length) return "";
            return Num(list[index - 1]);
        }

        private static int GetMember(string val, string[] list)
        {
            if (val == null) return 0;
            for (int i = 0; i < list.Length; i++)
                if (string.Equals(list[i], val, StringComparison.OrdinalIgnoreCase)) return i + 1;
            return 0;
        }

        private static bool EqualsI(string a, string b)
        {
            return string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);
        }

        private static int TryParseInt(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;
            int v;
            return int.TryParse(s.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static string GetRequestField(APIRequest req, string name)
        {
            if (req == null) return "";
            PropertyInfo p = req.GetType().GetProperty(name);
            if (p == null) return "";
            object v = p.GetValue(req, null);
            return v == null ? "" : v.ToString();
        }

        private static string Fmt(double v)
        {
            return Math.Round(v, 4).ToString("0.####", CultureInfo.InvariantCulture);
        }

        private static string Num(double v)
        {
            return Fmt(v).Replace(".", ",");
        }
    }
}