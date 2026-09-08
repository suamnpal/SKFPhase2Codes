using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class RINGAR_SL_TSD : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var k = new Dictionary<string, string>();
            string q = (req == null ? "" : req.ProductDesignation ?? "").Trim().ToUpperInvariant().Replace('.', ',');
            string m = (req == null ? "" : req.MachineNumber ?? "").Trim();
            List<Bookmark> bm = req == null ? null : req.Bookmarks;
            string body = q.StartsWith("SL-TSD", StringComparison.OrdinalIgnoreCase) ? q.Substring(6).Trim() : q;
            string first = body.Split(new[] { ' ', '/' }, StringSplitOptions.RemoveEmptyEntries).Length == 0 ? "" : body.Split(new[] { ' ', '/' }, StringSplitOptions.RemoveEmptyEntries)[0];
            string t = Digits(first);
            bool slash = q.Contains("/"), vz = q.Contains("VZ"), vz512 = q.Contains("VZ512"), vz803 = q.Contains("VZ803"), tum = q.Contains("7,3/16"), g = q.Contains("G"), u = q.Contains("U"), m4 = first.Contains("-4");
            bool v212 = q.Contains("V21-2"), v213 = q.Contains("V21-3"), v232 = q.Contains("V23-2"), v233 = q.Contains("V23-3"), v22 = q.Contains("V22"), v22d = q.Contains("V22-");
            bool v21 = !v212 && !v213 && q.Contains("V21"), v = v212 || v213 || v232 || v233 || v22 || v21;
            string p3 = Part(body, 1), p4 = Part(body, 2);
            bool specialSide = tum || vz || v;
            string[] d = DList(t, p3, slash, vz, vz512, vz803, v, v212, v213, v21, v22, m4, g, tum);
            string[] b = BList(t, p3, slash, specialSide, vz, v21, m4, g);
            string[] r = RList(t, p3, slash, vz, vz512, vz803, v21, g, tum);
            bool r2v = !v22d && Zero(Get(bm, "Ø D5", "D5")) && (v21 || Has(q, "30/500", "3040/190", "G", "3064", "3092", "3138/180", "3044", "VZ2M4"));
            double B = Val(bm, b[0], "B"), B1 = Val(bm, b[1], "B1"), B2 = Val(bm, b[2], "B2"), B3 = Val(bm, b[3], "B3"), B4 = Val(bm, b[4], "B4"), B5 = Val(bm, b[5], "B5"), B6 = Val(bm, b[6], "B6"), B7 = Val(bm, b[7], "B7"), B8 = Val(bm, b[8], "B8"), B9 = Val(bm, b[9], "B9");
            double D = Val(bm, d[0], "Ø D", "D"), D2 = Val(bm, d[2], "Ø D2", "D2"), D3 = Val(bm, d[3], "Ø D3", "D3"), D4 = Val(bm, d[4], "Ø D4", "D4"), D5 = Val(bm, d[5], "Ø D5", "D5"), D6 = Val(bm, d[6], "Ø D6", "D6"), D7 = Val(bm, d[7], "Ø D7", "D7");
            double D1in = Get(bm, "Ø D1", "D1"), D1 = Zero(D1in) ? D - ((B - B5) * 2) : D1in;
            k["VaLPopUp"] = Popup(req == null ? null : req.Published);
            k["SumG"] = "1x45º"; k["SumG1"] = "1x45º"; k["SumG2"] = "1x45º"; k["SumG3"] = "45º"; k["SumG4"] = "60º"; k["SumR"] = "R 0.2";
            double R1i = Get(bm, "R1"), R2i = Get(bm, "R2"), R3i = Get(bm, "R3"), R1 = Num(r[0]), R2 = Num(r[1]), R3 = Num(r[2]);
            k["SumR1"] = Zero(R1i) ? (vz512 ? "" : "R " + F(R1)) : "R " + F(R1i); k["SumR1Tol"] = Zero(R1i) ? RT(R1) : "";
            k["SumR2"] = Zero(R2i) ? ((v212 || v213 || (!r2v && !vz512)) ? "R " + F(R2) : "") : "R " + F(R2i); k["SumR2Tol"] = r2v ? "" : RT(R2);
            k["SumR3"] = "R " + F(Zero(R3i) ? R3 : R3i); k["SumR3Tol"] = Zero(R3i) ? RT(R3) : ""; k["SumD5v"] = t == "3268" ? "15º" : "0º";
            Sym(k, "B", B); BOne(k, B1); Opt(k, "B2", B2, r2v); Sym(k, "B3", B3); Sym(k, "B4", B4); Sym(k, "B5", B5); Sym(k, "B6", B6); Sym(k, "B7", B7); Sym(k, "B8", B8); Sym(k, "B9", B9);
            k["SumD"] = "(D) " + F(D); k["SumDTol"] = GT(D); k["SumD1"] = "Uträknat hjälpmått: (D1) " + F(D1);
            bool d2gen = (t == "3134" && vz) || (!r2v && Member(t, "3038", "3136", "3138", "3040", "3140"));
            k["SumD2"] = "(D2) " + F(D2); k["SumD2Tol"] = d2gen ? GT(D2) : "+ 0"; k["SumD2TolN"] = d2gen ? "" : H12(D2);
            string hk = (t + " " + p3 + " " + p4).Trim(); bool h10 = Member(hk, "3040 U", "3040 7,3 16", "3048 U", "3138 U", "3138 U VZ803", "3144 U", "3148 U", "3244 U");
            k["SumD3"] = "(D3) " + F(D3); k["SumD3Tol"] = h10 ? GT(D3) : "+ 0"; k["SumD3TolN"] = h10 ? "" : H10(D3);
            k["SumD4"] = "(D4) " + F(D4); k["SumD4Tol"] = GT(D4); k["SumD5"] = r2v ? "" : "(D5) " + F(D5); k["SumD5Tol"] = r2v ? "" : GT(D5);
            k["SumD6"] = "(D6) " + F(D6); k["SumD6Tol"] = D6T(D6); k["SumD6TolN"] = "- 0"; k["SumD7"] = "(D7) " + F(D7); k["SumD7Tol"] = D7U(D7); k["SumD7TolN"] = D7L(D7);
            string last4 = q.Length <= 4 ? q : q.Substring(q.Length - 4), stamp = q.Length <= 13 ? q : q.Substring(0, 13), sd = slash ? Part(body, 1) : "0";
            k["SumStämpel"] = (last4.Contains("U") || v) ? ((!vz && !u) ? q + " U" : q) : vz ? stamp + " U/" + sd : q + " U";
            k["SumRa"] = "3.2"; k["SumRa1"] = "6.3";
            bool mm = Member(m, "Nakamura", "MaxMuller", "LB45"); k["SumMaskinValS1"] = "Maskin: Svarvning - " + (mm ? m : "") + " ";
            k["SumF1_1"] = mm ? "1/1" : ""; k["SumF1_2"] = mm ? "Inst." : ""; k["SumF1_3"] = mm ? "Inst." : ""; k["SumF1_4"] = mm ? "1/3" : ""; k["SumF1_5"] = mm ? "1/5" : "";
            k["SumD1_1"] = mm ? "UD-Apparat/mikrometer" : ""; k["SumD1_2"] = mm ? "Djupmått" : ""; k["SumD1_3"] = mm ? "Radielyra/mallar" : ""; k["SumD1_4"] = mm ? "Ytjämnhetsmätare" : ""; k["SumD1_5"] = mm ? "Skjutmått" : "";
            k["SumAF1_1"] = mm ? "Inställd med tillhörande klove/ring" : ""; k["SumAF1_2"] = ""; k["SumAF1_3"] = ""; k["SumAF1_4"] = mm ? "Ytjämnhet övriga ytor 6.3" : ""; k["SumAF1_5"] = "";
            k["SumTextS1"] = "Okulärkontroll Grader, frifläcker, slagmärken, repor, valkar, ytjämnhet och faser, skarpa kanter avgradas."; k["SumRit"] = q;
            k["SumB2"] = r2v ? "" : "(B2) " + F(B2);
            k["SumB2Tol"] = B2Tol_LotusStringCompare(B2, r2v);
            k["SumStämpel"] = CalculateSumStampel(req);
            return k;
        }

        private static string[] DList(string t, string p, bool slash, bool vz, bool z512, bool z803, bool v, bool v212, bool v213, bool v21, bool v22, bool m4, bool g, bool tum)
        {
            if (vz) { if (t == "3036" && m4) return A8("224:208:206:198:193:202:192,8:188"); if (t == "3138" && z803) return A8("260:244,5:212:200:195:200:182,8:177,8"); if (t == "3036") return A8("224:208:206:198:193:202:179,8:175"); if (t == "3056" && z512) return A8("330::278:270:265::264,8:260"); if (t == "3244" && !m4) return A8("308:292:264:250:245:241:214,8:210"); if (t == "3244" && m4) return A8("308:292:264:250:245:254:238,8:234"); }
            if (v) { if (t == "3256" && v212) return A8("371,5:357,5:325:315:310:305:299,8:295"); if (t == "3056" && v212) return A8("353,5:337,5:306:295:290:295:274,8:270"); if (t == "3056" && v213) return A8("353,5:337,5:316:305:300:305:294,8:290"); if (t == "3134" && v21) return A8("216::184:173:168::154,8:150"); if (t == "3148" && v213) return A8("290,5:274,5:251:240:235:226:206,8:202"); if (t == "3148" && v212) return A8("290,5:274,5:251:240:235:214:184,8:180"); if (t == "3260" && v212) return A8("393,5:377,5:347:335:330:355:294,8:290"); if (t == "3260" && v213) return A8("393,5:377,5:347:335:330:342:321,8:317"); if (t == "3260" && v22) return A8("406,5:390,5:360:348:343:355:334,8:330"); }
            if (t == "3040") return tum ? A8("260:244,5:212:200:195:202:187,36:182,56") : p == "190" ? A8("260:244,5:217:205:200::194,8:190") : A8("260:244,5:212:200:195:202:184,8:180");
            if (t == "3138") return p == "180" ? A8("260:244,5:212:200:195::184,8:180") : A8("260:244:212:200:195:192:174,8:170");
            if (t == "3134") return p == "160" ? A8("216::184:173:168:197:164,8:160") : A8("219,5:203,5:180:169:164:175:154,8:150");
            if (t == "3140") return g ? A8("271:255:240:232:227::224,8:220") : A8("264,5:248,5:217:206:201:205:184,8:180");
            if (t == "3144") return g ? A8("291:275:260:252:247::244,8:240") : A8("290,5:274,5:231:220:215:224:204,8:200");
            if (t == "3064") return g ? A8("398,5:382,5:363:352:347::344,8:340") : A8("378,5:362,5:334:322:317::304,8:300");
            if (t == "3164") return g ? A8("413,5:397,5:373:361:356::344,8:340") : A8("393:377,5:347:335:330:325:304,8:300");
            if (t == "3284") return g ? A8("554:537,5:505:490:485:528:464,8:460") : A8("514:497,5:465:450:445:488:404,8:400");
            var x = new Dictionary<string, string> { { "3036", "219,5:203,5:184:173:168:190:164,8:160" }, { "3044", "271::224:216:211::204,8:200" }, { "3136", "236,5:220,5:194:181:176:192:164,8:160" }, { "3038", "236,5:220,5:204:191:186:192:174,8:170" }, { "3048", "294,5:278,5:251:240:235:244:224,8:220" }, { "3148", "310,5:294,5:251:240:235:244:224,8:220" }, { "3052", "310:293,5:271:260:255:265:244,8:240" }, { "3152", "333:317:285:275:270:265:244,8:240" }, { "3056", "333:317,5:286:275:270:285:264,8:260" }, { "3060", "373,5:357,5:326:315:310:305:284,8:280" }, { "3156", "353:337,5:305:295:290:285:264,8:260" }, { "3160", "373:357,5:325:315:310:305:284,8:280" }, { "3068", "398,5:382,5:351:340:335:345:324,8:320" }, { "3168", "414:397,5:366:355:350:360:324,8:320" }, { "3276", "493:477,5:445:430:425:425:364,8:360" }, { "3176", "493:477,5:445:430:425:425:364,8:360" }, { "3180", "454:437,5:415:400:395:405:384,8:380" }, { "3244", "310,5:294,5:251:240:235:244:204,8:200" }, { "3272", "454:437,5:415:400:395:405:344,8:340" }, { "3080", "454:437,5:415:400:395:405:384,8:380" }, { "3184", "493:477,5:445:430:425:425:404,8:400" } };
            return x.ContainsKey(t) ? A8(x[t]) : A8("0:0:0:0:0:0:0:0");
        }

        private static string[] BList(string t, string p, bool slash, bool special, bool vz, bool v21, bool m4, bool g)
        {
            if (slash && !special) { if (t == "3134" && p == "160") return A10("40:4:4:5:27:30,7:4,5:22,3:19,4:16,5"); if ((t == "3138" && p == "180") || (t == "3040" && p == "190")) return A10("42:4::7:29:33,7:4,5:22,3:19,4:16,5"); }
            if (t == "3138" && p != "180") return A10("42:4:4:7:29:33,7:4,5:22,3:19,4:16,5"); if (t == "3134" && v21) return A10("40:4::7:29:33,7:4,5:22,3:19,4:16,5");
            if (t == "3044" || ((t == "3140" || t == "3144") && g)) return A10("40:4::7:29:33,5:4,5:22,3:19,4:16,5"); if (t == "3140" && !g) return A10("41,8:4:2:7:29:33,7:4,5:22,3:19,4:16,5");
            if (t == "3156") return A10("42:4:2:7:29:33,7:4,5:22,25:19,4:16,5"); if ((t == "3064" && !slash) || (t == "3164" && g)) return A10("42:4::7:29:33,7:4,5:22,3:19,4:16,5");
            if (t == "3244" && vz && !m4) return A10("42:4:10:7:22:33,7:4:22,3:19,4:16,5"); if (t == "3244") return A10("42:4:2:7:29:33,7:4:22,3:19,4:16,5"); if (t == "3256") return A10("42:4:2:7:29:35:4,5:22,3:19,4:16,5");
            return A10("42:4:2:7:29:33,7:4,5:22,3:19,4:16,5");
        }

        private static string[] RList(string t, string p, bool slash, bool vz, bool z512, bool z803, bool v21, bool g, bool tum)
        {
            if (vz)
            {
                if (t == "3036") return A3("2:1,6:0,2");
                if (t == "3134" && v21) return A3("4::0,2");
                if (t == "3138" && z803) return A3("4:4:0,2");
                if (t == "3056" && z512) return A3("::0,2");
                return A3("4:1,6:0,2");
            }

            bool bigList = Member(t, "3036", "3038", "3134", "3136", "3048", "3148", "3052", "3056", "3060",
                                      "3152", "3156", "3160", "3260", "3068", "3168", "3276", "3180", "3272",
                                      "3176", "3080", "3184", "3244", "3284")
                           || ((t == "3140" || t == "3164") && !g)
                           || (t == "3144" && !g);

            if (bigList) return A3("4:1,6:0,2");
            if (t == "3040" && (!slash || tum)) return A3("4:2:0,2");
            if (t == "3138" && !slash) return A3("4:4:0,2");
            if ((t == "3040" && p == "190") || (t == "3138" && p == "180") || (t == "3064" && !slash) || (t == "3164" && g)) return A3("4::0,2");
            if (t == "3044" || ((t == "3140" || t == "3144") && g)) return A3("3::0,2");

            return A3("1:2:3");   // ← correct true fallback, matching Lotus
        }

        private static string Digits(string s) { int i = 0; while (i < (s ?? "").Length && char.IsDigit(s[i])) i++; return i == 0 ? (s ?? "") : s.Substring(0, i); }
        private static string Part(string s, int n) { string[] a = (s ?? "").Split(new[] { ' ', '/' }, StringSplitOptions.RemoveEmptyEntries); return n < a.Length ? a[n] : ""; }
        private static string[] Arr(string s, int n) { string[] a = s.Split(':'); string[] r = new string[n]; for (int i = 0; i < n; i++) r[i] = i < a.Length ? a[i] : ""; return r; }
        private static string[] A8(string s) { return Arr(s, 8); }
        private static string[] A10(string s) { return Arr(s, 10); }
        private static string[] A3(string s) { return Arr(s, 3); }
        private static void Sym(Dictionary<string, string> k, string n, double x) { k["Sum" + n] = "(" + n + ") " + F(x); k["Sum" + n + "Tol"] = GT(x); }
        private static void BOne(Dictionary<string, string> k, double x) { k["SumB1"] = "(B1) " + F(x); k["SumB1Tol"] = x < 5 ? "+ 0.2" : ""; k["SumB1TolN"] = " 0"; }
        private static void Opt(Dictionary<string, string> k, string n, double x, bool h) { k["Sum" + n] = h ? "" : "(" + n + ") " + F(x); k["Sum" + n + "Tol"] = h ? "" : GT(x); }
        private static string GT(double x) { return x > 400 ? "± 0.8" : x > 120 ? "± 0.5" : x > 30 ? "± 0.3" : x > 6 ? "± 0.2" : "± 0.1"; }
        private static string RT(double x) { return x > 6 ? "± 1" : x > 3 ? "± 0.5" : x > 0.5 ? "± 0.2" : ""; }
        private static string H12(double x) { return x < 181 ? "- 0.400" : x < 251 ? "- 0.460" : x < 316 ? "- 0.520" : x < 401 ? "- 0.570" : x < 501 ? "- 0.630" : x < 631 ? "- 0.700" : "- 0.800"; }
        private static string H10(double x) { return x < 181 ? "- 0.160" : x < 251 ? "- 0.185" : x < 316 ? "- 0.210" : x < 401 ? "- 0.230" : "- 0.250"; }
        private static string D6T(double x) { return x < 181 ? "+ 0.160" : x < 251 ? "+ 0.185" : x < 316 ? "+ 0.210" : x < 401 ? "+ 0.230" : "+ 0.250"; }
        private static string D7U(double x) { return x < 181 ? "+ 0.245" : x < 251 ? "+ 0.285" : x < 316 ? "+ 0.320" : x < 401 ? "+ 0.355" : "+ 0.385"; }
        private static string D7L(double x) { return x < 181 ? "+ 0.085" : x < 251 ? "+ 0.100" : x < 316 ? "+ 0.110" : x < 401 ? "+ 0.125" : "+ 0.135"; }
        private static bool Member(string x, params string[] a) { foreach (string y in a) if (string.Equals(x ?? "", y, StringComparison.OrdinalIgnoreCase)) return true; return false; }
        private static bool Has(string x, params string[] a) { foreach (string y in a) if ((x ?? "").IndexOf(y, StringComparison.OrdinalIgnoreCase) >= 0) return true; return false; }
        private static double Val(List<Bookmark> b, string d, params string[] n) { double x = Get(b, n); return Zero(x) ? Num(d) : x; }
        private static double Get(List<Bookmark> b, params string[] n) { if (b != null) foreach (string x in n) foreach (Bookmark z in b) if (z != null && string.Equals((z.BookmarkName ?? "").Trim(), x, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(z.BookmarkValue)) return Num(z.BookmarkValue); return 0; }
        private static double Num(string s) { double x; return double.TryParse((s ?? "").Trim().Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out x) ? x : 0; }
        private static bool Zero(double x) { return Math.Abs(x) < 0.0000001; }
        private static string F(double x) { return x.ToString("0.################", CultureInfo.InvariantCulture).Replace('.', ','); }
        private static string Popup(string p) { DateTime d; if (string.IsNullOrWhiteSpace(p) || !DateTime.TryParse(p, out d)) return ""; DateTime e = d.AddDays(14); return DateTime.Today <= e.Date ? "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)<<LineBreak>>" + Environment.NewLine + "Popupruta aktiv till " + e.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : ""; }

        private static string B2Tol(double b2, bool r2v)
        {
            if (r2v)
                return "";

            if (b2 > 400)
                return "± 0.8";

            if (b2 > 120)
                return "± 0.5";

            if (b2 > 30)
                return "± 0.3";

            if (b2 > 6)
                return "± 0.2";

            if (b2 > 0.5)
                return "± 0.1";

            return "";
        }

        private static string B2Tol_LotusStringCompare(double b2, bool r2v)
        {
            if (r2v)
                return "";

            string s = F(b2);

            if (StrGt(s, "400")) return "± 0.8";
            if (StrGt(s, "120")) return "± 0.5";
            if (StrGt(s, "30")) return "± 0.3";
            if (StrGt(s, "6")) return "± 0.2";
            if (StrGt(s, "0,5")) return "± 0.1";

            return "";
        }

        
        private static bool StrGt(string a, string b)
        {
            return string.CompareOrdinal(a, b) > 0;
        }

        private string CalculateSumStampel(APIRequest req)
        {
            string q = (req == null ? "" : req.ProductDesignation ?? "").Trim().ToUpperInvariant().Replace('.', ',');

            bool slash = q.Contains("/");
            bool vz = q.Contains("VZ");
            bool u = q.Contains("U");
            bool v212 = q.Contains("V21-2"), v213 = q.Contains("V21-3");
            bool v232 = q.Contains("V23-2"), v233 = q.Contains("V23-3");
            bool v22 = q.Contains("V22");
            bool v21 = !v212 && !v213 && q.Contains("V21");
            bool v = v212 || v213 || v232 || v233 || v22 || v21;

            string last4 = q.Length <= 4 ? q : q.Substring(q.Length - 4);
            string stamp = q.Length <= 13 ? q : q.Substring(0, 13);
            string sd = slash ? WordBySlash(q, 2) : "0";

            return (last4.Contains("U") || v)
                ? ((!vz && !u) ? q + " U" : q)
                : vz ? stamp + " U/" + sd
                     : q + " U";
        }

        private static string WordBySlash(string s, int n)
        {
            string[] parts = (s ?? "").Split('/');
            return n >= 1 && n <= parts.Length ? parts[n - 1].Trim() : "";
        }
    }
}
