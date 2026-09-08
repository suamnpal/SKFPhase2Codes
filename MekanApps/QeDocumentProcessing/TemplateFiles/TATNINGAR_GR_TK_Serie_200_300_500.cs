using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class TATNINGAR_GR_TK_Serie_200_300_500 : ITemplateCalculations
    {
        private static readonly string[] Machines = { "Nakamura", "LB45", "LT-3000EX" };
        private static readonly string[] Typ2List = { "13", "15", "16", "17", "18", "20", "22", "24", "26", "28", "30", "32" };
        private static readonly string[] Typ5List = { "11", "12", "13", "15", "16", "17", "18", "19", "20", "22", "24", "26", "28", "30", "32" };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();
            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;
            List<Bookmark> bm = req != null ? req.Bookmarks : null;
            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);
            string tmpFormat = subject.ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");
            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : string.Empty;
            int serie = TryParseInt(tmpBet2.Length >= 1 ? tmpBet2.Substring(0, 1) : string.Empty);
            string tmpTyp = tmpBet2.Length >= 2 ? RightSafe(tmpBet2, 2) : string.Empty;
            int typ = TryParseInt(tmpTyp);
            int typIndex = serie == 2 ? GetMember(tmpTyp, Typ2List) : GetMember(tmpTyp, Typ5List);
            double[] d = DiameterList(serie, typ);
            double[] b = WidthList(serie, typ);
            AddDim(kv, "Sumd", "(d) ", V(bm, "Ø d", G(d, 1)), "+ " + F3(0.4), "- " + F1(0));
            AddDim(kv, "Sumd1", "(d1) ", V(bm, "Ø d1", G(d, 2)), "+ " + F1(0), "- " + F3(0.4));
            kv["Sumd2"] = "(d2) " + F(V(bm, "Ø d2", G(d, 3)));
            kv["Sumd2Tol"] = "± " + F3(0.1);
            AddDim(kv, "Sumd3", "2x (d3) ", V(bm, "Ø d3", G(d, 4)), "+ " + F1(0), "- " + F3(0.3));
            double d4 = V(bm, "Ø d4", G(d, 5));
            kv["Sumd4"] = Z(d4) ? "Endast vissa typer" : "(d4) " + F(d4);
            kv["Sumd4Tol"] = Z(d4) ? "" : "+ " + F3(0.5);
            kv["Sumd4TolN"] = Z(d4) ? "" : "- " + F1(0);
            AddDim(kv, "Sumd5", "(d5) ", V(bm, "Ø d5", G(d, 6)), "+ " + F3(H12(V(bm, "Ø d5", G(d, 6)))), "- " + F1(0));
            AddDim(kv, "Sumd6", "(d6) ", V(bm, "Ø d6", G(d, 7)), "+ " + F1(0), "- " + F3(H12S(V(bm, "Ø d6", G(d, 7)))));
            AddDim(kv, "Sumd7", "(d7) ", V(bm, "Ø d7", G(d, 8)), "+ " + F3(H12S(V(bm, "Ø d7", G(d, 8)))), "- " + F1(0));
            AddDim(kv, "Sumd8", "(d8) ", V(bm, "Ø d8", G(d, 9)), "+ " + F1(0), "- " + F3(H12S(V(bm, "Ø d8", G(d, 9)))));
            AddDim(kv, "Sumd9", "(d9) ", V(bm, "Ø d9", G(d, 10)), "+ " + F3(H12S(V(bm, "Ø d9", G(d, 10)))), "- " + F1(0));
            AddDim(kv, "SumDa", "(Da) ", V(bm, "Ø Da", G(d, 11)), "+ " + F1(0), "- " + F3(H12S(V(bm, "Ø Da", G(d, 11)))));
            AddDim(kv, "SumB", "(B) ", V(bm, "Bredd B", G(b, 1)), "+ " + F3(0.5), "- " + F1(0));
            kv["Sumb1"] = "(b1) " + F(V(bm, "Bredd b1", G(b, 2)));
            kv["Sumb1Tol"] = "± " + F3(0.2);
            AddDim(kv, "Sumb2", "(b2) ", V(bm, "Bredd b2", G(b, 3)), "+ " + F1(0), "- " + F3(0.5));
            AddDim(kv, "Sumb4", "(b4) ", V(bm, "Bredd b4", G(b, 4)), "+ " + F1(0), "- " + F3(0.2));
            AddDim(kv, "Sumb5", "(b5) ", V(bm, "Bredd b5", G(b, 5)), "+ " + F1(0), "- " + F3(0.5));
            double b6 = Z(d4) ? 0 : 5;
            kv["Sumb6"] = Z(b6) ? "Endast vissa typer" : "(b6) " + F(b6);
            kv["Sumb6Tol"] = Z(b6) ? "" : "+ " + F3(0.5);
            kv["Sumb6TolN"] = Z(b6) ? "" : "- " + F1(0);
            AddDim(kv, "SumH", "(H) ", V(bm, "Bredd H", HVal(serie, typ)), "+ " + F3(0.5), "- " + F1(0));
            AddDim(kv, "SumJ", "(J) ", V(bm, "Bredd J", typ < 19 ? 4.8 : 5.8), "+ " + F1(0), "- " + F3(0.2));
            AddDim(kv, "SumK", "(K) ", V(bm, "Bredd K", typ < 19 ? 3 : 4), "+ " + F3(0.4), "- " + F1(0));
            AddDim(kv, "SumN", "(N) ", V(bm, "Bredd N", NVal(serie, typIndex)), "+ " + F1(0), "- " + F3(0.5));
            AddDim(kv, "SumL", "(L) ", V(bm, "Bredd L", LVal(serie, typ)), "+ " + F3(0.5), "- " + F1(0));
            kv["SumBhd"] = "Ø 3";
            kv["SumG"] = "(G) M6";
            AddDim(kv, "SumGn", "", GnVal(serie, typ), "+ " + F3(0.5), "- " + F1(0));
            AddDim(kv, "SumS", "Ø", SVal(serie, typ), "+ " + F3(0.1), "- " + F1(0));
            kv["SumMi"] = "Min." + F(MiVal(serie, typ));
            kv["SumMa"] = "Max." + F(MaVal(serie, typ)).Replace(".",",");
            kv["SumF25"] = "2.5x45° (2x)";
            kv["SumF1"] = "1x45°";
            kv["SumF15"] = "1.5x45°";
            kv["SumF65"] = "Gängfas Ø 6.5 +0.5";
            kv["SumR2"] = "R2";
            kv["SumR08"] = "4 x max: R0.8";
            kv["SumR02"] = "max: R0.2";
            kv["SumR05"] = "5x max: R0.5";
            kv["SumR1"] = "R1 alt. 1x45°";
            kv["SumCo1"] = " " + 0.15;
            kv["SumCo2"] = kv["SumCo1"];
            kv["SumRd"] = " 0.10";
            kv["SumRa32"] = "3.2";
            kv["SumAm"] = "Inriktning";
            kv["SumAm1"] = F1(1).Replace(".",",");
            kv["SumAm1Tol"] = (serie == 3 && typ == 16 ? "+ " + F3(0.25) : "+ " + F3(0.1)).Replace(".", ",");
            kv["SumAm1TolN"] = (serie == 3 && typ == 16 ? "+ " + F3(0.5) : "-  " + F3(0.1)).Replace(".", ",");
            kv["SumAM1TolN"] = kv["SumAm1TolN"];
            kv["SumAmb"] = F1(2);
            kv["SumAmbTol"] = "± " + F3(0.2);
            SumMaskinVal(kv, maskinVal);
            SumF(kv, maskinVal);
            SumD(kv, maskinVal);
            SumAF(kv);
            string rit = serie == 2 ? (typ < 28 ? "7433676" : "7433625") : (serie == 3 ? tmpBet : "7433475");
            kv["SumRitNr"] = rit + ":senaste utg. märkning: 7433523:senaste utg.";
            kv["SumRitNr2"] = kv["SumRitNr"];
            kv["SumTextS1"] = "Okulärkontroll Grader, frifläckar, slagmärken, repor, valkar, ytjämnhet och faser, skarpa kanter avgradas,          Kontrollera stämpel på förstabit.";
            kv["SumTextS2"] = kv["SumTextS1"];
            return kv;
        }

        private static void AddDim(Dictionary<string, string> kv, string name, string prefix, double val, string tol, string toln)
        {
            kv[name] = prefix + F(val);
            kv[name + "Tol"] = tol;
            kv[name + "TolN"] = toln;
        }

        private static void SumMaskinVal(Dictionary<string, string> kv, string maskinVal)
        {
            kv["SumMaskinValS1"] = "Maskin: " + (maskinVal ?? "") + " - Diametrala mått ";
            kv["SumMaskinValS2"] = "Maskin: " + (maskinVal ?? "") + " - Övriga mått";
            kv["SumMaskinvalS1"] = kv["SumMaskinValS1"];
            kv["SumMaskinvalS2"] = kv["SumMaskinValS2"];
        }

        private static void SumF(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = In(maskinVal, Machines);
            kv["SumF1_1"] = m ? "1/5" : "";
            kv["SumF1_2"] = m ? "1/5" : "";
            kv["SumF1_3"] = m ? "1/5" : "";
            kv["SumF1_4"] = "";
            kv["SumF1_5"] = "";
            kv["SumF1_6"] = m ? "Inst." : "";
            kv["SumF1_7"] = m ? "Inst. eller vid misstanke" : "";
            kv["SumF2_1"] = m ? "1/5" : "";
            kv["SumF2_2"] = m ? "1/5" : "";
            kv["SumF2_3"] = m ? "1/5" : "";
            kv["SumF2_4"] = m ? "1/5" : "";
            kv["SumF2_5"] = m ? "1/5" : "";
            kv["SumF2_6"] = m ? "Inst. eller vid misstanke" : "";
            kv["SumF2_7"] = m ? "1/5" : "";
        }

        private static void SumD(Dictionary<string, string> kv, string maskinVal)
        {
            bool m = In(maskinVal, Machines);
            kv["SumD1_1"] = m ? "Mätmaskin Alt.Skjutmått" : "";
            kv["SumD1_2"] = m ? "Mätmaskin Alt.UD-Apparat" : "";
            kv["SumD1_3"] = m ? "Mätmaskin Alt.Skjutmått" : "";
            kv["SumD1_4"] = "";
            kv["SumD1_5"] = "";
            kv["SumD1_6"] = m ? "Mätmaskin Alt.Mätservice" : "";
            kv["SumD1_7"] = m ? "Ytjämnhetsmätare" : "";
            kv["SumD2_1"] = m ? "Mätmaskin Alt.Skjutmått" : "";
            kv["SumD2_2"] = m ? "Mätmaskin Alt.Skjutmått" : "";
            kv["SumD2_3"] = m ? "Mätmaskin Alt.Skjutmått" : "";
            kv["SumD2_4"] = m ? "Gängtolk" : "";
            kv["SumD2_5"] = m ? "Mätmaskin Alt.Skjutmått" : "";
            kv["SumD2_6"] = m ? "Ytjämnhetsmätare" : "";
            kv["SumD2_7"] = m ? "Mätmaskin Alt.Skjutmått" : "";
        }

        private static void SumAF(Dictionary<string, string> kv)
        {
            for (int i = 1; i <= 7; i++) kv["SumAF1_" + i.ToString(CultureInfo.InvariantCulture)] = "";
            for (int i = 1; i <= 7; i++) kv["SumAF2_" + i.ToString(CultureInfo.InvariantCulture)] = "";
        }

        private static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published)) return "";
            DateTime d;
            if (!DateTime.TryParse(published, out d)) return "";
            DateTime valid = d.AddDays(14);
            return DateTime.Today <= valid.Date ? "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)<<LineBreak>><<LineBreak>>Information om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument<<LineBreak>><<LineBreak>>Popupruta aktiv till " + valid.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : "";
        }

        private static double[] DiameterList(int serie, int typ)
        {
            switch (typ)
            {
                case 11: return A(56.5, 66.5, 70, 72.4, 0, 74, 80, 91, 97, 108, 114);
                case 12: return A(61.5, 71.5, 75, 77.4, 0, 79, 85, 96, 102, 113, 119);
                case 13: return serie == 2 ? A(78, 92, 95.5, 97.6, 0, 98.5, 104.5, 115.5, 121.5, 132.5, 138.5) : A(66.5, 76.5, 80, 82.4, 0, 84, 90, 101, 107, 118, 124);
                case 15: return serie == 2 ? A(88, 102, 105.6, 108, 0, 107, 112, 121, 126, 135, 140) : A(71.5, 86.5, 90, 92.4, 0, 89, 95, 106, 112, 123, 129);
                case 16: return serie == 2 ? A(93, 107.5, 111, 113.4, 0, 119, 125, 137, 143, 155, 161) : A(78.5, 92, 95.5, 97.6, 0, 98.5, 104.5, 115.5, 121.5, 132.5, 138.5);
                case 17: return serie == 2 ? A(98, 111.5, 115, 117.4, 0, 119, 125, 137, 143, 155, 161) : A(84, 97, 100.5, 102.9, 0, 104, 110, 121, 127, 138, 144);
                case 18: return serie == 2 ? A(104, 119.5, 123, 125.4, 0, 129, 135, 147, 153, 165, 171) : A(90.5, 102, 105.6, 108, 0, 110, 116, 127, 133, 144, 150);
                case 19: return A(93.5, 130.5, 135.7, 138.1, 0, 113, 119, 131, 137, 149, 155);
                case 20: return serie == 2 ? A(119, 137, 142.2, 144.6, 0, 142, 149, 162, 169, 182, 189) : A(100, 137, 142.2, 144.6, 112.5, 119, 125, 137, 143, 155, 161);
                case 22: return serie == 2 ? A(129, 147, 152.2, 154.6, 0, 159, 167, 181, 189, 203, 211) : A(110, 147, 152.2, 154.6, 122, 129, 135, 147, 153, 165, 171);
                case 24: return serie == 2 ? A(139, 157, 162.2, 164.6, 0, 169, 177, 191, 199, 213, 221) : A(120, 157, 162.2, 164.6, 132.5, 142, 149, 162, 169, 182, 189);
                case 26: return serie == 2 ? A(149, 167, 172.2, 174.6, 0, 174, 182, 196, 204, 218, 226) : A(125, 167, 172.2, 174.6, 143.5, 147, 155, 169, 177, 191, 199);
                case 28: return serie == 2 ? A(158, 177, 182.2, 184, 0, 184, 190, 206, 214, 230, 238) : A(136.5, 177, 182.2, 184.6, 0, 159, 167, 181, 189, 203, 211);
                case 30: return serie == 2 ? A(169, 192, 197.2, 199.6, 0, 198, 205, 219, 226, 240, 247) : A(145.5, 192, 197.2, 199.6, 0, 169, 177, 191, 199, 213, 221);
                case 32: return serie == 2 ? A(179, 202, 207.2, 209.6, 0, 208, 215, 229, 236, 250, 257) : A(150.5, 202, 207.2, 209.6, 0, 174, 182, 196, 204, 218, 226);
                default: return A(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            }
        }

        private static double[] WidthList(int serie, int typ)
        {
            switch (typ)
            {
                case 11: return A(32, 6, 13, 13, 9);
                case 12: return A(32, 6, 13, 13, 8.5);
                case 13: return serie == 2 ? A(35, 7.5, 15.5, 16.5, 10.5) : A(32, 6, 13, 13, 8.5);
                case 15: return serie == 2 ? A(35, 7.5, 15.5, 16.5, 10.5) : A(30, 5, 11, 14, 7);
                case 16: return serie == 2 ? A(37, 6, 15, 17.5, 8.5) : A(35, 7.5, 16, 16.5, 10.5);
                case 17: return serie == 2 ? A(37, 6, 15, 17.5, 8.5) : A(35, 7.5, 15.5, 16.5, 10.5);
                case 18: return serie == 2 ? A(38, 7.5, 16, 18.5, 10) : A(35, 7.5, 15.5, 16.5, 10.5);
                case 19: return A(36, 7.5, 14.5, 19, 10);
                case 20: return serie == 2 ? A(40, 7.5, 16, 19, 10) : A(34, 6, 13, 17.5, 8.5);
                case 22: return serie == 2 ? A(41, 7.5, 17, 19, 10) : A(36, 7.5, 15, 18.5, 10);
                case 24: return serie == 2 ? A(41, 7.5, 16.5, 19, 10) : A(37, 7.5, 15, 19, 10);
                case 26: return serie == 2 ? A(41, 7.5, 16, 19, 10) : A(37.5, 7.5, 15, 19, 10);
                case 28: return serie == 2 ? A(41, 7.5, 16.5, 16, 10.5) : A(38, 7.5, 15, 19, 10);
                case 30: return serie == 2 ? A(43, 8, 17.5, 20, 11) : A(37.5, 7.5, 14.5, 19, 10);
                case 32: return serie == 2 ? A(43, 8, 17.5, 20, 11) : A(38, 7.5, 15, 19, 10);
                default: return A(0, 0, 0, 0, 0);
            }
        }

        private static double HVal(int serie, int typ)
        {
            if (serie == 2)
            {
                if (typ == 13) return 14;
                if (typ == 15) return 14.5;
                if (typ < 20) return 16;
                if (typ == 20) return 17;
                if (typ == 22 || typ == 26) return 18;
                if (typ == 24) return 19.5;
                return 19;
            }
            if (typ < 17) return 14;
            if (typ < 19) return 14.5;
            if (typ == 19) return 16.5;
            if (typ < 24) return 16;
            if (typ == 24) return 17;
            if (typ == 26) return 17.5;
            if (typ >= 28 && typ <= 32) return 18;
            return 18.5;
        }

        private static double NVal(int serie, int i)
        {
            double[] s2 = A(2.2, 2.4, 2.8, 2.8, 3, 3, 3.9, 3, 3, 4.2, 3.5, 3.5);
            double[] s5 = A(2.2, 2.1, 2.2, 2.2, 2.2, 2.2, 2.4, 3.2, 2.8, 3, 3, 3.5, 3.9, 4.3, 3.8);
            return G(serie == 2 ? s2 : s5, i);
        }

        private static double LVal(int serie, int typ)
        {
            if (serie == 2) return typ < 20 ? 23 : typ == 20 ? 25 : typ == 28 ? 31 : 27;
            if (serie == 3) return typ == 16 ? 22 : 23;
            return typ < 20 ? 21 : typ < 24 ? 22 : typ == 24 ? 25 : 27;
        }

        private static double GnVal(int serie, int typ) { return serie == 2 ? typ < 17 ? 5 : typ == 28 || typ == 30 || typ == 32 ? 10 : 8 : typ < 24 ? 5 : 10; }
        private static double SVal(int serie, int typ) { return serie == 2 ? typ < 17 ? 6.3 : 8 : typ < 24 ? 6.3 : 8; }
        private static double MiVal(int serie, int typ) { return serie == 2 ? typ < 17 ? 13 : typ < 28 ? 16 : 18 : typ < 24 ? 13 : 18; }
        private static double MaVal(int serie, int typ) { return serie == 2 ? typ < 17 ? 16.5 : typ < 28 ? 19.5 : 21.5 : typ < 24 ? 16.5 : 21.5; }
        private static double H12(double x) { if (x < 3.01) return 0.100; if (x < 6.01) return 0.120; if (x < 10.01) return 0.150; if (x < 18.01) return 0.180; if (x < 30.01) return 0.210; if (x < 50.01) return 0.250; if (x < 80.01) return 0.300; if (x < 120.01) return 0.350; if (x < 180.01) return 0.400; if (x < 250.01) return 0.460; if (x < 315.01) return 0.520; if (x < 400.01) return 0.570; if (x < 500.01) return 0.630; if (x < 630.01) return 0.700; if (x < 800.01) return 0.800; if (x < 1000.01) return 0.900; if (x < 1250.01) return 1.050; if (x < 1600.01) return 1.250; if (x < 2000.01) return 1.500; if (x < 2500.01) return 1.750; return 2.100; }
        private static double H12S(double x) { if (x < 3.01) return 0.100; if (x < 6.01) return 0.120; if (x < 10.01) return 0.150; if (x < 18.01) return 0.180; if (x < 30.01) return 0.210; if (x < 50.01) return 0.250; if (x < 80.01) return 0.300; if (x < 120.01) return 0.350; if (x < 180.01) return 0.400; if (x < 250.01) return 0.460; if (x < 315.01) return 0.520; if (x < 400.01) return 0.570; if (x < 500.01) return 0.630; return 0.700; }
        private static double V(List<Bookmark> bm, string k, double f) { double v = Dbl(bm, k); return Z(v) ? f : v; }
        private static string Str(List<Bookmark> bm, string k) { if (bm == null) return ""; for (int i = 0; i < bm.Count; i++) if (bm[i] != null && string.Equals(bm[i].BookmarkName ?? "", k, StringComparison.OrdinalIgnoreCase)) return bm[i].BookmarkValue ?? ""; return ""; }
        private static double Dbl(List<Bookmark> bm, string k) { double v; return double.TryParse(Str(bm, k).Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0; }
        private static double[] A(params double[] x) { return x; }
        private static double G(double[] x, int i) { return x == null || i < 1 || i > x.Length ? 0 : x[i - 1]; }
        private static int GetMember(string v, string[] a) { for (int i = 0; i < a.Length; i++) if (string.Equals(a[i], v, StringComparison.OrdinalIgnoreCase)) return i + 1; return 0; }
        private static bool In(string v, string[] a) { for (int i = 0; i < a.Length; i++) if (string.Equals(a[i], v, StringComparison.OrdinalIgnoreCase)) return true; return false; }
        private static bool Z(double v) { return Math.Abs(v) < 0.000001; }
        private static string RightSafe(string s, int n) { return string.IsNullOrEmpty(s) ? "" : s.Length <= n ? s : s.Substring(s.Length - n); }
        private static int TryParseInt(string s) { int v; return int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0; }
        private static string F(double v) { return v.ToString("0.###", CommonFunctions.Culture).Replace(",", "."); }
        private static string F1(double v) { return v.ToString("F1", CommonFunctions.Culture).Replace(",", "."); }
        private static string F3(double v) { return v.ToString("F3", CommonFunctions.Culture).Replace(",", "."); }
    }
}
