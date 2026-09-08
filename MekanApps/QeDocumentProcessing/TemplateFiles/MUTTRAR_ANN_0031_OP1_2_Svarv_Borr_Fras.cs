using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class MUTTRAR_ANN_0031_OP1_2_Svarv_Borr_Fras : ITemplateCalculations
    {
        private static readonly string[] Machines = new[] { "LB45", "MaxMuller/K&T", "VTR-160", "MacTurn 550" };

        private static readonly string[] TypListNormal = new[]
        {
            "62","64","66","68","70","72","74","76","78","80","82","84","86","88","90","92","94","96",
            "98","100","102","104","106","108","110","112","116","120","126","130","134","138","142",
            "146","150","155","160","165","170","175","180"
        };

        private static readonly string[] TypListL = new[]
        {
            "62","64","66","68","69","70","72","73","74","76","77","78","80","82","84","86","88","90",
            "92","94","96","98","100","102","104","106","108","110","112","116","120","126","130","134",
            "138","142","146","150","155","160","165","170","175","180"
        };

        private static readonly int[] D5ListNormal = new[]
        {
            350,360,380,400,410,420,430,450,460,470,470,490,490,510,530,540,550,560,
            580,580,590,610,610,620,640,650,670,690,730,750,775,800,825,850,875,900,
            925,950,975,1000,1030
        };

        private static readonly int[] D5ListL = new[]
        {
            346,356,366,376,384,384,394,404,414,422,422,432,442,452,462,472,490,490,
            510,510,530,550,550,570,570,590,590,610,610,640,660,690,720,740,760,780,
            800,820,850,870,900,925,950,975
        };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;
            List<Bookmark> bm = req != null ? req.Bookmarks : null;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");

            bool tmpSlash = tmpBet.IndexOf('/') >= 0;
            bool tmpL = tmpFormat.IndexOf('L') >= 0;
            bool tmpANN = tmpFormat.IndexOf("ANN", StringComparison.OrdinalIgnoreCase) >= 0;

            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : string.Empty;

            string tmpANNTyp = tmpBet2.Length >= 2 ? tmpBet2.Substring(tmpBet2.Length - 2) : tmpBet2;

            string tmpOmvTyp = string.Equals(tmpANNTyp, "31", StringComparison.OrdinalIgnoreCase) ? "96" : string.Empty;

            string tmpTyp = tmpOmvTyp;

            double tmpTypNum = TryParseDouble(tmpTyp);

            kv["SumRitningsnr"] = tmpBet;
            kv["SumRitningsnr2"] = tmpBet;

            int tmpTypLista;
            if (tmpANN)
            {
                tmpTypLista = GetMember(tmpTyp, new[] { "96" });
            }
            else if (!tmpL)
            {
                tmpTypLista = GetMember(tmpTyp, TypListNormal);
            }
            else
            {
                tmpTypLista = GetMember(tmpTyp, TypListL);
            }

            int tmpP = tmpTypNum < 61 ? 4 : tmpTypNum < 101 ? 5 : tmpTypNum < 139 ? 6 : tmpTypNum < 181 ? 7 : 8;
            kv["SumP"] = "(P) " + tmpP.ToString(CultureInfo.InvariantCulture);
            kv["SumVP"] = "30º";

            double tmpkd = Math.Truncate((tmpTypNum / 2.0) * 10.0);
            kv["SumGänga"] = "Tr " + FormatNum(tmpkd) + "x" + tmpP.ToString(CultureInfo.InvariantCulture);
            kv["SumGMall"] = "Tr x " + tmpP.ToString(CultureInfo.InvariantCulture);

            double tmpd4 = (tmpP == 4 || tmpP == 5) ? tmpkd + 0.5 : tmpkd + 1.0;
            kv["Sumd4"] = "(d4) " + FormatNum(tmpd4);
            kv["Sumd4Tol"] = "± " + Fmt3(GenTol(tmpd4));

            double tmpd2 = tmpP == 4 ? tmpkd - 2.0 : tmpP == 5 ? tmpkd - 2.5 : tmpP == 6 ? tmpkd - 3.0 : tmpP == 7 ? tmpkd - 3.5 : tmpkd - 4.0;
            kv["Sumd2"] = "(d2) " + FormatNum(tmpd2);
            kv["Sumd2Tol"] = "+ 0.53";
            kv["Sumd2TolN"] = "- 0";

            double tmpd1 = tmpP == 4 ? tmpkd - 4.0 : tmpP == 5 ? tmpkd - 5.0 : tmpP == 6 ? tmpkd - 6.0 : tmpP == 7 ? tmpkd - 7.0 : tmpkd - 8.0;
            kv["Sumd1"] = "(d1) " + FormatNum(tmpd1);
            kv["Sumd1Tol"] = "+ 0.45";
            kv["Sumd1TolN"] = "- 0";

            double tmpD = GetDouble(bm, "Ytterdiameter (D)");
            kv["Sumd"] = "(D) " + FormatNum(tmpD);
            kv["SumdTol"] = "+ 0";
            kv["SumdTolN"] = "- " + Fmt3(DTolN(tmpD));

            double tmpd5;
            if (tmpANN)
            {
                tmpd5 = 522.0;
            }
            else
            {
                int[] d5list = tmpL ? D5ListL : D5ListNormal;
                tmpd5 = tmpTypLista >= 1 && tmpTypLista <= d5list.Length ? d5list[tmpTypLista - 1] : 0.0;
            }
            kv["Sumd5"] = "(d5) " + FormatNum(tmpd5);
            kv["Sumd5Tol"] = "+ 0";
            kv["Sumd5TolN"] = "- 1.1";

            double tmpd3 = tmpANN ? tmpkd + 3.0 : (tmpkd < 501 ? tmpkd + 2.0 : tmpkd + 3.0);
            kv["Sumd3"] = "(d3) " + FormatNum(tmpd3);
            kv["Sumd3Tol"] = "+ 1.55";
            kv["Sumd3TolN"] = "- 0";

            double tmpB = GetDouble(bm, "Bredd (B)");
            kv["SumB"] = "(B) " + FormatNum(tmpB);
            kv["SumBTol"] = "+ 0";
            kv["SumBTolN"] = "- 0.39";

            double tmpB2 = tmpB / 2.0;
            kv["SumB2"] = "(B2) " + FormatNum(tmpB2);
            kv["SumB2Tol"] = "± " + Fmt3(GenTol(tmpB2));

            kv["SumRadie"] = "2x30º";
            kv["SumIF1"] = "45º";

            kv["SumG"] = "(G) M12";

            double tmpv = GetDouble(bm, "Gängdjup (v)");
            kv["Sumv"] = "(v) min:" + FormatNum(tmpv);

            double tmpu = GetDouble(bm, "Borrdjup (u)");
            kv["Sumu"] = "(u) " + FormatNum(tmpu);

            double tmpHB = 507.0;
            kv["SumHB"] = "(HB) 507";
            kv["SumHBTol"] = "± " + Fmt3(JS13Tol(tmpHB));

            kv["SumRHT"] = "R1.6 ± 0.4 (2x)";
            kv["SumM"] = "1.5";

            double tmpt = GetDouble(bm, "Spårdjup (t)");
            kv["Sumt"] = "(t) " + FormatNum(tmpt);
            kv["SumtTol"] = "+ 1.5";
            kv["SumtTolN"] = "- 0";

            double tmpS = GetDouble(bm, "Spårbredd (S)");
            kv["SumS"] = "(S) " + FormatNum(tmpS);
            kv["SumSTol"] = tmpS < 31.0 ? "± 0.260" : tmpS < 51.0 ? "± 0.310" : "± 0.370";

            kv["SumKa"] = "0.090";
            kv["SumPl"] = "0.090";
            kv["SumRa"] = "2.5";

            kv["SumStämpling"] = "Stämplas enl. ritning 7430189";

            SumMaskinVal(kv, maskinVal);
            SumFrequencies(kv, maskinVal);
            SumMeasuringDevices(kv, maskinVal);
            SumAF(kv, maskinVal);

            kv["SumKlEgenskaper"] = "PRODUCTION NUTS & SLEEVES & HOUSINGS\\ALLMÄNKLASSADE EGENSKAPER\\Klassade egenskaper muttrar";
            kv["SumKlEgenskaper2"] = kv["SumKlEgenskaper"];

            string tmpText = "Okulärkontroll: Grader, slagmärken, ojämnheter & andra ytdefekter. Rätt & tydlig märkning.";
            kv["SumTextS1"] = tmpText;
            kv["SumTextS2"] = tmpText;

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
                       " dagar)\n\nInformation om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument\n\nPopupruta aktiv till " +
                       validTill.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return "";
        }

        private static void SumMaskinVal(Dictionary<string, string> kv, string maskinVal)
        {
            string s1 =
                EqualsI(maskinVal, "LB45") ? "LB45" :
                EqualsI(maskinVal, "MaxMuller/K&T") ? "MaxMuller" :
                EqualsI(maskinVal, "VTR-160") ? "VTR-160" :
                EqualsI(maskinVal, "MacTurn 550") ? "MacTurn 550" : "";

            string s2 =
                EqualsI(maskinVal, "LB45") ? "LB45" :
                EqualsI(maskinVal, "MaxMuller/K&T") ? "K&T" :
                EqualsI(maskinVal, "VTR-160") ? "VTR-160" :
                EqualsI(maskinVal, "MacTurn 550") ? "MacTurn 550" : "";

            kv["SumMaskinValS1"] = ("Maskin: " + s1 + " - OP1 & 2").Trim();
            kv["SumMaskinValS2"] = ("Maskin: " + s2 + " - OP3").Trim();
        }

        private static void SumFrequencies(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsMachine(maskinVal);
            for (int i = 1; i <= 8; i++) kv["SumF1_" + i] = m ? "1/1" : "";
            for (int i = 1; i <= 6; i++) kv["SumF2_" + i] = m ? "1/5" : "";
        }

        private static void SumMeasuringDevices(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsMachine(maskinVal);
            kv["SumD1_1"] = m ? "Multimar" : "";
            kv["SumD1_2"] = m ? "Skjutmått" : "";
            kv["SumD1_3"] = m ? "Skjutmått" : "";
            kv["SumD1_4"] = m ? "Skjutmått" : "";
            kv["SumD1_5"] = m ? "Skjutmått" : "";
            kv["SumD1_6"] = m ? "Skjutmått" : "";
            kv["SumD1_7"] = m ? "Ytjämnhetsmätare" : "";
            kv["SumD1_8"] = m ? "Egglinjal" : "";
            kv["SumD2_1"] = m ? "Skjutmått" : "";
            kv["SumD2_2"] = m ? "Djupmått" : "";
            kv["SumD2_3"] = m ? "Gängtolk" : "";
            kv["SumD2_4"] = m ? "Djupmått" : "";
            kv["SumD2_5"] = m ? "Skjutmått" : "";
            kv["SumD2_6"] = m ? "Gängtolk min/max" : "";
        }

        private static void SumAF(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = IsMachine(maskinVal);
            kv["SumAF1_1"] = "";
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = "";
            kv["SumAF1_6"] = m ? "Bearbetning i OP1 + 3mm" : "";
            kv["SumAF1_7"] = "";
            kv["SumAF1_8"] = m ? "Vid tveksamhet lämnas mutter till mätrum" : "";
            kv["SumAF2_1"] = "";
            kv["SumAF2_2"] = "";
            kv["SumAF2_3"] = "";
            kv["SumAF2_4"] = "";
            kv["SumAF2_5"] = "";
            kv["SumAF2_6"] = "";
        }

        private static bool IsMachine(string maskinVal)
        {
            if (string.IsNullOrEmpty(maskinVal)) return false;
            for (int i = 0; i < Machines.Length; i++)
                if (string.Equals(Machines[i], maskinVal, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }

        private static int GetMember(string val, string[] list)
        {
            for (int i = 0; i < list.Length; i++)
                if (string.Equals(list[i], val, StringComparison.OrdinalIgnoreCase)) return i + 1;
            return 0;
        }

        private static double GenTol(double v)
        {
            if (v < 6) return 0.1;
            if (v < 30) return 0.2;
            if (v < 120) return 0.3;
            if (v < 400) return 0.5;
            if (v < 1000) return 0.8;
            if (v < 2000) return 1.2;
            return 2.0;
        }

        private static double D2PosTol(double kd)
        {
            if (kd < 301) return 0.475;
            if (kd < 501) return 0.530;
            if (kd < 701) return 0.600;
            if (kd < 901) return 0.630;
            return 0.710;
        }

        private static double D1PosTol(double kd)
        {
            if (kd < 301) return 0.375;
            if (kd < 501) return 0.450;
            if (kd < 701) return 0.500;
            if (kd < 901) return 0.560;
            return 0.630;
        }

        private static double DTolN(double d)
        {
            if (d < 3.01) return 0.140;
            if (d < 6.01) return 0.180;
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

        private static double D5TolN(double d5)
        {
            if (d5 < 19) return 0.270;
            if (d5 < 31) return 0.330;
            if (d5 < 51) return 0.390;
            if (d5 < 81) return 0.460;
            if (d5 < 121) return 0.540;
            if (d5 < 181) return 0.630;
            if (d5 < 251) return 0.720;
            if (d5 < 316) return 0.810;
            if (d5 < 401) return 0.890;
            if (d5 < 501) return 0.970;
            if (d5 < 631) return 1.100;
            if (d5 < 801) return 1.250;
            if (d5 < 1000) return 1.400;
            return 1.650;
        }

        private static double D3PosTol(double d3)
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

        private static double BTolN(double b)
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

        private static double JS13Tol(double hb)
        {
            if (hb < 3.01) return 0.070;
            if (hb < 6.01) return 0.090;
            if (hb < 10.01) return 0.110;
            if (hb < 18.01) return 0.135;
            if (hb < 30.01) return 0.165;
            if (hb < 50.01) return 0.195;
            if (hb < 80.01) return 0.230;
            if (hb < 120.01) return 0.270;
            if (hb < 180.01) return 0.315;
            if (hb < 250.01) return 0.360;
            if (hb < 315.01) return 0.405;
            if (hb < 400.01) return 0.445;
            if (hb < 500.01) return 0.485;
            if (hb < 630.01) return 0.550;
            if (hb < 800.01) return 0.625;
            if (hb < 1000.01) return 0.700;
            if (hb < 1250.01) return 0.825;
            if (hb < 1600.01) return 0.975;
            if (hb < 2000.01) return 1.150;
            if (hb < 2500.01) return 1.400;
            return 1.650;
        }

        private static double tTolPos(bool isANN, double t)
        {
            if (isANN) return 1.5;
            if (t < 11) return 1.5;
            if (t < 19) return 1.8;
            if (t < 31) return 2.1;
            return 2.5;
        }

        private static string FormatNum(double v)
        {
            return v.ToString(CommonFunctions.Culture).Replace(",", ".");
        }

        private static string Fmt3(double v)
        {
            return v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
        }

        private static double TryParseDouble(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;
            double v;
            return double.TryParse(s.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static double GetDouble(List<Bookmark> bm, string key)
        {
            string raw = GetString(bm, key);
            if (string.IsNullOrWhiteSpace(raw)) return 0;
            double v;
            return double.TryParse(raw.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static string GetString(List<Bookmark> bm, string key)
        {
            if (bm == null || string.IsNullOrWhiteSpace(key)) return "";
            for (int i = 0; i < bm.Count; i++)
            {
                Bookmark b = bm[i];
                if (b != null && string.Equals(b.BookmarkName ?? "", key, StringComparison.OrdinalIgnoreCase))
                    return b.BookmarkValue ?? "";
            }
            return "";
        }

        private static bool EqualsI(string a, string b)
            => string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);
    }
}