using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class MUTTRAR_ANN_0028_OP1_3_Svarv_Borr_Fras : ITemplateCalculations
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

        private static readonly double[] D5Normal = new double[]
        {
            350,360,380,400,410,420,430,450,460,470,470,490,490,510,530,540,550,560,580,580,590,610,
            610,620,640,650,670,690,730,750,775,800,825,850,875,900,925,950,975,1000,1030
        };

        private static readonly double[] D5L = new double[]
        {
            346,356,366,376,384,384,394,404,414,422,422,432,442,452,462,472,490,490,510,510,530,550,
            550,570,570,590,590,610,610,640,660,690,720,740,760,780,800,820,850,870,900,925,950,975
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

            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : string.Empty;

            string tmpANNTyp = tmpBet2.Length >= 2 ? tmpBet2.Substring(tmpBet2.Length - 2) : tmpBet2;
            string tmpOmvTyp = string.Equals(tmpANNTyp, "28", StringComparison.OrdinalIgnoreCase) ? "82" : "";

            string tmpTypStr = tmpOmvTyp;
            double typNum = TryParseDouble(tmpTypStr);

            int typLista = tmpL
                ? GetMember(tmpTypStr, TypListL)
                : GetMember(tmpTypStr, TypListNormal);

            int tmpP = typNum < 61 ? 4 : typNum < 101 ? 5 : typNum < 139 ? 6 : typNum < 181 ? 7 : 8;
            double tmpkd = Math.Truncate((typNum / 2.0) * 10.0);

            kv["SumP"] = "(P) " + tmpP.ToString(CultureInfo.InvariantCulture);
            kv["SumVP"] = "30º";
            kv["SumGänga"] = "Tr " + Fmt(tmpkd) + "x" + tmpP.ToString(CultureInfo.InvariantCulture);
            kv["SumGMall"] = "Tr x " + tmpP.ToString(CultureInfo.InvariantCulture);

            double tmpd4 = (tmpP == 4 || tmpP == 5) ? tmpkd + 0.5 : tmpkd + 1.0;
            kv["Sumd4"] = "(d4) " + Fmt(tmpd4);
            kv["Sumd4Tol"] = "± " + (GenTol(tmpd4));

            double tmpd2 = DM(tmpkd, tmpP);
            kv["Sumd2"] = "(d2) " + Fmt(tmpd2);
            kv["Sumd2Tol"] = "+ " + (D2Tol(tmpkd));
            kv["Sumd2TolN"] = "- " + (0.0);

            double tmpd1 = D1Val(tmpkd, tmpP);
            kv["Sumd1"] = "(d1) " + Fmt(tmpd1);
            kv["Sumd1Tol"] = "+ " + (D1Tol(tmpkd));
            kv["Sumd1TolN"] = "- " + (0.0);

            double tmpD = GetDouble(bm, "Ytterdiameter (D)");
            kv["Sumd"] = "(D) " + Fmt(tmpD);
            kv["SumdTol"] = "+ " + (0.0);
            kv["SumdTolN"] = "- " + Fmt3(H13Tol(tmpD));

            double[] d5list = tmpL ? D5L : D5Normal;
            double tmpd5 = typLista >= 1 && typLista <= d5list.Length ? d5list[typLista - 1] : 0.0;
            kv["Sumd5"] = "(d5) " + Fmt(tmpd5);
            kv["Sumd5Tol"] = "+ " + (0.0);
            kv["Sumd5TolN"] = "- " + (D5TolN(tmpd5));

            double tmpd3 = tmpkd < 501 ? tmpkd + 2.0 : tmpkd + 3.0;
            kv["Sumd3"] = "(d3) " + Fmt(tmpd3);
            kv["Sumd3Tol"] = "+ " + (D3Tol(tmpd3));
            kv["Sumd3TolN"] = "- " + (0.0);

            double tmpBval = GetDouble(bm, "Bredd (B)");
            kv["SumB"] = "(B) " + Fmt(tmpBval);
            kv["SumBTol"] = "+ " + (0.0);
            kv["SumBTolN"] = "- " + (BTolN(tmpBval));

            double tmpB2 = tmpBval / 2.0;
            kv["SumB2"] = "(B2) " + Fmt(tmpB2);
            kv["SumB2Tol"] = "± " + Fmt3(GenTol(tmpB2));

            kv["SumRadie"] = RadieVal(typNum).ToString(CultureInfo.InvariantCulture) + "x45º";
            kv["SumIF1"] = "45º";

            kv["SumG"] = "(G) M12";

            double tmpv = GetDouble(bm, "Gängdjup (v)");
            kv["Sumv"] = "(v) min:" + Fmt(tmpv);

            double tmpu = GetDouble(bm, "Borrdjup (u)");
            kv["Sumu"] = "(u) " + Fmt(tmpu);

            kv["SumRHT"] = "R1.6 ± 0.4 (2x)";

            double tmpt = GetDouble(bm, "Spårdjup (t)");
            kv["Sumt"] = "(t) " + Fmt(tmpt);
            kv["SumtTol"] = "+ " + (TTol(tmpt));
            kv["SumtTolN"] = "- " + (0.0);

            double tmpS = GetDouble(bm, "Spårbredd (S)");
            kv["SumS"] = "(S) " + Fmt(tmpS);
            kv["SumSTol"] = tmpS < 31 ? "± 0.260" : tmpS < 51 ? "± 0.310" : "± 0.370";

            kv["SumKa"] = "0.050";
            kv["SumPl"] = kv["SumKa"];
            kv["SumPa"] = "0.090";
            kv["SumRa"] = "2.5";

            kv["SumStämpling"] = "Stämplas enl. ritning 7430189";

            SumMaskinVal(kv, maskinVal);
            SumFrequencies(kv, maskinVal);
            SumMeasuringDevices(kv, maskinVal);
            SumAF(kv, maskinVal);

            kv["SumRitningsnr"] = tmpBet;
            kv["SumRitningsnr2"] = tmpBet;

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
            string s1 = MapS1(maskinVal);
            string s2 = MapS2(maskinVal);
            kv["SumMaskinValS1"] = ("Maskin: " + s1 + " - OP1 & 2").Trim();
            kv["SumMaskinValS2"] = ("Maskin: " + s2 + " - OP3").Trim();
        }

        private static string MapS1(string mv)
        {
            if (EqualsI(mv, "LB45")) return "LB45";
            if (EqualsI(mv, "MaxMuller/K&T")) return "MaxMuller";
            if (EqualsI(mv, "VTR-160")) return "VTR-160";
            if (EqualsI(mv, "MacTurn 550")) return "MacTurn 550";
            return "";
        }

        private static string MapS2(string mv)
        {
            if (EqualsI(mv, "LB45")) return "LB45";
            if (EqualsI(mv, "MaxMuller/K&T")) return "K&T";
            if (EqualsI(mv, "VTR-160")) return "VTR-160";
            if (EqualsI(mv, "MacTurn 550")) return "MacTurn 550";
            return "";
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
            for (int i = 1; i <= 5; i++) kv["SumAF1_" + i] = "";
            kv["SumAF1_6"] = m ? "Bearbetning i OP1 + 3mm" : "";
            kv["SumAF1_7"] = "";
            kv["SumAF1_8"] = m ? "Vid tveksamhet lämnas mutter till mätrum" : "";
            for (int i = 1; i <= 6; i++) kv["SumAF2_" + i] = "";
        }

        private static bool IsMachine(string mv)
        {
            if (string.IsNullOrEmpty(mv)) return false;
            for (int i = 0; i < Machines.Length; i++)
                if (string.Equals(Machines[i], mv, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        private static int GetMember(string val, string[] list)
        {
            for (int i = 0; i < list.Length; i++)
                if (string.Equals(list[i], val, StringComparison.OrdinalIgnoreCase)) return i + 1;
            return 0;
        }

        private static double DM(double kd, int p)
        {
            if (p == 4) return kd - 2.0;
            if (p == 5) return kd - 2.5;
            if (p == 6) return kd - 3.0;
            if (p == 7) return kd - 3.5;
            return kd - 4.0;
        }

        private static double D2Tol(double kd)
        {
            if (kd < 301) return 0.475;
            if (kd < 501) return 0.530;
            if (kd < 701) return 0.600;
            if (kd < 901) return 0.630;
            return 0.710;
        }

        private static double D1Val(double kd, int p)
        {
            if (p == 4) return kd - 4.0;
            if (p == 5) return kd - 5.0;
            if (p == 6) return kd - 6.0;
            if (p == 7) return kd - 7.0;
            return kd - 8.0;
        }

        private static double D1Tol(double kd)
        {
            if (kd < 301) return 0.375;
            if (kd < 501) return 0.450;
            if (kd < 701) return 0.500;
            if (kd < 901) return 0.560;
            return 0.630;
        }

        private static double D3Tol(double d3)
        {
            if (d3 < 19) return 0.43;
            if (d3 < 31) return 0.52;
            if (d3 < 51) return 0.62;
            if (d3 < 81) return 0.74;
            if (d3 < 121) return 0.87;
            if (d3 < 181) return 1.00;
            if (d3 < 251) return 1.15;
            if (d3 < 316) return 1.30;
            if (d3 < 401) return 1.40;
            if (d3 < 501) return 1.55;
            if (d3 < 631) return 1.75;
            if (d3 < 801) return 2.00;
            if (d3 < 1000) return 2.30;
            return 2.60;
        }

        private static double H13Tol(double v)
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

        private static double D5TolN(double d5)
        {
            if (d5 < 19) return 0.27;
            if (d5 < 31) return 0.33;
            if (d5 < 51) return 0.39;
            if (d5 < 81) return 0.46;
            if (d5 < 121) return 0.54;
            if (d5 < 181) return 0.63;
            if (d5 < 251) return 0.72;
            if (d5 < 316) return 0.81;
            if (d5 < 401) return 0.89;
            if (d5 < 501) return 0.97;
            if (d5 < 631) return 1.10;
            if (d5 < 801) return 1.25;
            if (d5 < 1000) return 1.40;
            return 1.65;
        }

        private static double BTolN(double b)
        {
            if (b < 7) return 0.18;
            if (b < 11) return 0.22;
            if (b < 19) return 0.27;
            if (b < 31) return 0.33;
            if (b < 51) return 0.39;
            if (b < 81) return 0.46;
            if (b < 121) return 0.54;
            return 0.63;
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

        private static double TTol(double t)
        {
            if (t < 11) return 1.5;
            if (t < 19) return 1.8;
            if (t < 31) return 2.1;
            return 2.5;
        }

        private static int RadieVal(double typ)
        {
            if (typ < 89) return 3;
            if (typ < 121) return 4;
            if (typ < 143) return 5;
            return 6;
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

        private static string Fmt(double v) => v.ToString(CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt1(double v) => v.ToString("F1", CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt3(double v) => v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
    }
}