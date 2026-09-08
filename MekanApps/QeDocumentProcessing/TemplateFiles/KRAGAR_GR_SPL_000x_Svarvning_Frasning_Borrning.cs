using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class KRAGAR_GR_SPL_000x_Svarvning_Frasning_Borrning : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var result = new Dictionary<string, string>();

            var ctx = BuildContext(request);

            Merge(result, GetDiameters(ctx));
            Merge(result, GetThreadsHolesRadii(ctx));
            Merge(result, GetAngles(ctx));
            Merge(result, GetWidths(ctx));
            Merge(result, GetSurfaceAndPlan(ctx));
            Merge(result, GetMachine(ctx));
            Merge(result, GetFrequencies(ctx));
            Merge(result, GetMeasuringTools(ctx));
            Merge(result, GetRemarks(ctx));
            Merge(result, GetTexts(ctx));

            return result;
        }

        private Context BuildContext(APIRequest request)
        {
            var subject = CleanSubject(request?.ProductDesignation ?? "");
            var machineRaw = WebUtility.HtmlDecode(request?.MachineNumber ?? "");

            // Parse from the cleaned raw designation, not from the drawing text.
            // This handles UI/file values like GR-SPL-0006-3180.pdf and still gives size 3180.
            var tokens = subject.ToUpperInvariant()
                .Replace(',', '.')
                .Split(new[] { ' ', '/', '.', '-', ':', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            var article = tokens.FirstOrDefault(t =>
                t == "0001" || t == "0004" || t == "0006") ?? "";

            string sizeToken = "";

            if (!string.IsNullOrEmpty(article))
            {
                var articleIndex = Array.IndexOf(tokens, article);

                if (articleIndex >= 0)
                {
                    sizeToken = tokens
                        .Skip(articleIndex + 1)
                        .FirstOrDefault(IsFourDigitSizeToken) ?? "";
                }
            }

            if (string.IsNullOrEmpty(sizeToken))
            {
                sizeToken = tokens.FirstOrDefault(t =>
                    IsFourDigitSizeToken(t) &&
                    t != "0001" &&
                    t != "0004" &&
                    t != "0006") ?? "";
            }

            return new Context
            {
                Article = article,
                Size = sizeToken,
                DesignationNormalized = NormalizeDesignationForDrawing(subject),
                Serie = GetSerie(sizeToken),
                Typ = GetTyp(sizeToken),
                TypIndex = GetTypIndex(sizeToken),
                Machine = NormalizeMachine(machineRaw)
            };
        }

        private bool IsFourDigitSizeToken(string token)
        {
            return !string.IsNullOrWhiteSpace(token)
                && token.Length == 4
                && token.All(char.IsDigit);
        }

        private int GetSerie(string size)
        {
            if (string.IsNullOrWhiteSpace(size) || size.Length < 2)
                return 0;

            return int.TryParse(size.Substring(0, 2), out var serie)
                ? serie
                : 0;
        }

        private int GetTyp(string size)
        {
            if (string.IsNullOrWhiteSpace(size) || size.Length < 2)
                return 0;

            return int.TryParse(size.Substring(size.Length - 2), out var typ)
                ? typ
                : 0;
        }

        private int GetTypIndex(string size)
        {
            var serie = GetSerie(size);
            var typ = GetTyp(size);

            var map30 = new Dictionary<int, int>
            {
                { 60, 1 },
                { 64, 2 },
                { 80, 3 }
            };

            var map31 = new Dictionary<int, int>
            {
                { 48, 1 },
                { 60, 2 },
                { 64, 3 },
                { 68, 4 },
                { 72, 5 },
                { 80, 6 },
                { 84, 7 }
            };

            var map32 = new Dictionary<int, int>
            {
                { 68, 1 },
                { 72, 2 },
                { 76, 3 },
                { 80, 4 }
            };

            if (serie == 30)
                return map30.ContainsKey(typ) ? map30[typ] : 0;

            if (serie == 31)
                return map31.ContainsKey(typ) ? map31[typ] : 0;

            if (serie == 32)
                return map32.ContainsKey(typ) ? map32[typ] : 0;

            return 0;
        }

        private string CleanSubject(string subject)
        {
            var value = WebUtility.HtmlDecode(subject ?? string.Empty).Trim();

            // The selection UI shows names like GR-SPL-0006-3180.pdf.
            // Lotus Subject is the designation, so remove a trailing file extension before parsing/calculation.
            if (value.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                value = value.Substring(0, value.Length - 4);

            return value;
        }

        private string NormalizeDesignationForDrawing(string subject)
        {
            // Lotus SumRitS1/SumRitS2 uses the complete normalized Subject (TmpBet) for 0006 drawings,
            // not just the size token. Example: GR-SPL-0006-3184:senaste utgåva.
            var value = (subject ?? "").Trim().ToUpperInvariant().Replace('.', ',');
            return string.Join(" ", value.Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
        }

        private string NormalizeMachine(string machine)
        {
            var m = machine?.Trim() ?? "";
            var lower = m.ToLowerInvariant();

            if (lower.Contains("nakamura"))
                return "Nakamura";

            if (lower.Contains("maxmuller") || lower.Contains("maxmüller"))
                return "MaxMuller";

            if (lower.Contains("lb45"))
                return "LB45";

            if (lower.Contains("macturn") || lower.Contains("mac turn"))
                return "MacTurn 550";

            return "";
        }

        private bool Is0006(Context ctx) => ctx.Article == "0006";

        private bool Is0004(Context ctx) => ctx.Article == "0004";

        private int i(Context ctx) => Math.Max(0, ctx.TypIndex - 1);

        private string Pick(Context ctx, string[] s30, string[] s31, string[] s32)
        {
            var values = ctx.Serie == 30
                ? s30
                : ctx.Serie == 31
                    ? s31
                    : s32;

            var x = ctx.TypIndex - 1;
            if (x < 0 || x >= values.Length)
                return string.Empty;

            return values[x];
        }

        private string PickByArticle(
            Context ctx,
            string[] normal30,
            string[] normal31,
            string[] normal32,
            string[] special30,
            string[] special31,
            string[] special32)
        {
            return Is0006(ctx)
                ? Pick(ctx, special30, special31, special32)
                : Pick(ctx, normal30, normal31, normal32);
        }

        private Dictionary<string, string> GetDiameters(Context ctx)
        {
            var a = Pick(
                ctx,
                new[] { "391", "419", "505" },
                new[] { "351", "402", "439", "474", "478", "505", "534" },
                new[] { "474", "478", "510", "525" });

            var b = PickByArticle(
                ctx,
                new[] { "352", "0", "452" },
                new[] { "294", "353", "373", "393", "413", "451", "472" },
                new[] { "402", "411", "431", "453" },
                new[] { "0", "371", "0" },
                new[] { "0", "0", "371", "0", "0", "451", "472" },
                new[] { "0", "0", "0", "0" });

            var c1 = Pick(
                ctx,
                new[] { "343", "346", "437" },
                new[] { "294", "343", "359", "381", "437", "437", "478" },
                new[] { "437", "437", "460", "468" });

            var c2 = PickByArticle(
                ctx,
                new[] { "331", "0", "427" },
                new[] { "284", "331", "349", "371", "427", "427", "468" },
                new[] { "427", "427", "450", "458" },
                new[] { "0", "337", "0" },
                new[] { "0", "0", "350", "0", "0", "427", "468" },
                new[] { "0", "0", "0", "0" });

            var d1 = Pick(
                ctx,
                new[] { "343", "363", "444" },
                new[] { "284", "343", "363", "383", "403", "443", "463" },
                new[] { "393", "403", "423", "444" });

            var d2 = Pick(
                ctx,
                new[] { "331", "351", "432" },
                new[] { "270", "331", "351", "371", "391", "431", "451" },
                new[] { "376", "391", "411", "432" });

            var d3 = Pick(
                ctx,
                new[] { "325", "345", "426" },
                new[] { "265", "325", "345", "365", "385", "425", "445" },
                new[] { "367", "385", "405", "425" });

            var e = Pick(
                ctx,
                new[] { "284", "304", "384" },
                new[] { "224", "284", "304", "324", "344", "384", "404" },
                new[] { "324", "344", "363", "384" });

            var f = PickByArticle(
                ctx,
                new[] { "372", "0", "484" },
                new[] { "327", "382", "410", "450", "454", "484", "512" },
                new[] { "450", "454", "474", "495" },
                new[] { "0", "400", "0" },
                new[] { "0", "0", "420", "0", "0", "484", "0", "512" },
                new[] { "0", "0", "0", "0" });

            return new Dictionary<string, string>
            {
                ["SumA"] = $"(A) {a}",
                ["SumATol"] = GetTolPM(a),

                ["SumB"] = $"(B) {b}",
                ["SumBTol"] = GetTolPMB(b),

                ["SumC1"] = $"(C1) {c1}",
                ["SumC1Tol"] = PlusZeroF1(),
                ["SumC1TolN"] = GetNegTol(c1),

                ["SumC2"] = $"(C2) {c2}",
                ["SumC2Tol"] = GetTolPMB(c2),

                ["SumD1"] = $"(D1) {d1}",
                ["SumD1Tol"] = GetPosTolH(d1),
                ["SumD1TolN"] = MinusZeroF1(),

                ["SumD2"] = $"(D2) {d2}",
                ["SumD2Tol"] = PlusZeroF1(),
                ["SumD2TolN"] = GetNegTol(d2),

                ["SumD3"] = $"(D3) {d3}",
                ["SumD3Tol"] = GetPosTolH(d3),
                ["SumD3TolN"] = MinusZeroF1(),

                ["SumE"] = $"(E) {e}",
                ["SumETol"] = PlusF3(0.300),
                ["SumETolN"] = MinusZeroF1(),

                ["SumF"] = $"(F) {f}",
                ["SumFTol"] = GetTolF(f)
            };
        }

        private Dictionary<string, string> GetThreadsHolesRadii(Context ctx)
        {
            var g = PickByArticle(
                ctx,
                new[] { "15", "0", "16" },
                new[] { "14.5", "15.5", "14.5", "14", "14.5", "15.5", "15" },
                new[] { "18", "15.5", "15.5", "16" },
                new[] { "0", "17.5", "0" },
                new[] { "0", "0", "17.5", "0", "0", "15.5", "15" },
                new[] { "0", "0", "0", "0" });

            var s = PickByArticle(
                ctx,
                new[] { "10", "0", "9" },
                new[] { "12", "12", "12", "12", "0", "15", "15" },
                new[] { "15", "10", "8", "15" },
                new[] { "0", "13", "0" },
                new[] { "0", "13", "0", "0", "0", "0", "15" },
                new[] { "0", "0", "0", "0" });

            var h = Pick(
                ctx,
                new[] { "5", "15", "12" },
                new[] { "15", "12", "15", "15", "13", "13", "22" },
                new[] { "20", "13", "10", "20" });

            var j = ctx.Serie == 31
                ? ctx.Typ == 60 ? "6.6" : "9"
                : ctx.Typ < 62 ? "6.6" : "9";

            var r = PickByArticle(
                ctx,
                new[] { "0", "0" },
                new[] { "100", "37.5", "40", "40", "0", "0", "0" },
                new[] { "0", "0", "0", "0" },
                new[] { "0" },
                new[] { "0", "0", "0", "0", "0", "0", "0" },
                new[] { "0", "0", "0", "0" });

            var x = PickByArticle(
                ctx,
                new[] { "0", "0" },
                new[] { "159", "105", "115", "115", "0", "0", "0" },
                new[] { "0", "0", "0", "0" },
                new[] { "0" },
                new[] { "0", "0", "0", "0", "0", "0", "0" },
                new[] { "0", "0", "0", "0" });

            var y = PickByArticle(
                ctx,
                new[] { "0", "0" },
                new[] { "207,5", "204", "221", "237", "0", "0", "0" },
                new[] { "0", "0", "0", "0" },
                new[] { "0" },
                new[] { "0", "0", "0", "0", "0", "0", "0" },
                new[] { "0", "0", "0", "0" });
          
            return new Dictionary<string, string>
            {
                ["SumG"] = $"(G) {g}",
                ["SumGTol"] = PlusMinusF3(0.500),
                ["SumG2"] = "1/4-28 UNF",

                ["SumS"] = Is0004(ctx) || IsZero(s)
                    ? "Genomgående"
                    : $"(S) {s}",

                ["SumSTol"] = Is0004(ctx) || IsZero(s)
                    ? ""
                    : GetTolPMSimple(s),

                ["SumGmin"] = ctx.Typ == 64 ? "min 7" : "min 6",

                ["SumH"] = $"{h}º",

                ["SumJ"] = $"(J) {j}",

                ["SumJTol"] = Is0006(ctx)
                    ? $"+ {GetJTol(ctx, j)}"
                    : $"± {GetTolNumeric(j)}",

                ["SumJTolN"] = Is0006(ctx) ? "- 0" : "",

                ["SumR08"] = "max R0.8 (x3)",
                ["SumR1"] = "max R1",
                ["SumR2"] = "R2",
                ["SumR2a"] = "R2",
                
                ["SumR"] = IsZero(r) ? "R" : $"x2 (R) {r.Replace(".",",")}",
                ["SumX"] = IsZero(r) ? "X" : $"(X) {x} (x2)",
                ["SumY"] = IsZero(r) ? "Y" : $"(Y) {y} (x2)",

                ["SumRXY"] = IsZero(r)
                    ? "Uttag finns ej, dim. (R), (X) & (Y)"
                    : ""
            };
        }

        private Dictionary<string, string> GetAngles(Context ctx)
        {
            var k = !Is0006(ctx)
                ? ctx.Serie == 30
                    ? ctx.Typ == 60 ? "15" : "22,5"
                    : ctx.Serie == 31
                        ? ctx.Typ == 48
                            ? "22,5"
                            : ctx.Typ < 70
                                ? "15"
                                : "22,5"
                        : "22,5"
                : "22,5";

            return new Dictionary<string, string>
            {
                ["SumK"] = ctx.Size == "3180"
                    ? ""
                    : $"{k}°{(k == "15" ? "x2" : "")}",

                ["Sum225"] = "22.5°",
                ["Sum45"] = Is0006(ctx) ? "45°x8" : "45°x5",
                ["SumFas"] = "1x45°"
            };
        }

        private Dictionary<string, string> GetWidths(Context ctx)
        {
            var n = PickByArticle(
                ctx,
                new[] { "38.5", "40.5", "38.5" },
                new[] { "39.5", "38.5", "38.5", "38.5", "38.5", "38.5", "43.5" },
                new[] { "43.5", "38.5", "38.5", "41" },
                new[] { "0", "40.5", "0" },
                new[] { "0", "0", "40.5", "0", "0", "38.5", "38.5" },
                new[] { "0", "0", "0", "0" });

            var q = ctx.Serie == 30
                ? "23.5"
                : ctx.Serie == 32 && ctx.Typ == 68
                    ? "26"
                    : "23.5";

            var l = PickByArticle(
                ctx,
                new[] { "21.5", "20", "21.5" },
                new[] { "21.5", "20", "20", "20", "20", "21.5", "22" },
                new[] { "23.5", "21", "20", "21" },
                new[] { "21.5", "20", "21.5" },
                new[] { "20", "20", "20", "20", "21", "22", "20" },
                new[] { "23.5", "21", "20", "21" });

            var p = PickByArticle(
                ctx,
                new[] { "7.5", "7.5", "7.5" },
                new[] { "7.5", "7.5", "7.5", "7.5", "7.5", "7.5", "7.5" },
                new[] { "10.5", "8", "7.5", "8" },
                new[] { "0", "7.5" },
                new[] { "7.5", "7.5", "7.5", "7.5", "8", "7.5", "7.5" },
                new[] { "10.5", "8", "7.5", "8" });

            var m = PickByArticle(
                ctx,
                new[] { "7", "9", "7" },
                new[] { "9", "7", "7", "7", "7", "7", "13" },
                new[] { "9", "8", "7", "9" },
                new[] { "0", "9", "0" },
                new[] { "0", "0", "9", "0", "0", "8", "0", "7" },
                new[] { "0", "0", "0", "0" });

            var pTolN = ctx.Size == "3180" ? MinusF3(0.200) : MinusZeroF1();

            var mTol = GetTolPMB(m);

            return new Dictionary<string, string>
            {
                ["SumN"] = $"(N) {n}",
                ["SumNTol"] = GetTolPMB(n),

                ["SumB1"] = "(B1) 10",
                ["SumB1Tol"] = "± 0.2",

                ["SumB2"] = "(B2) 5",
                ["SumB2Tol"] = "± 0.1",

                ["SumQ"] = $"(Q) {q}",
                ["SumQTol"] = PlusF3(0.200),
                ["SumQTolN"] = MinusZeroF1(),

                ["SumL"] = $"(L) {l}",
                ["SumLTol"] = PlusF3(0.200),
                ["SumLTolN"] = MinusZeroF1(),

                ["SumP"] = $"(P) {p}",
                ["SumPTol"] = PlusF3(0.200),
                ["SumPTolN"] = pTolN,

                ["SumM"] = $"(M) {m}",
                ["SumMTol"] = mTol,

                ["SumMToL"] = mTol
            };
        }

        private Dictionary<string, string> GetSurfaceAndPlan(Context ctx)
        {
            var plan = ctx.Serie == 30 && ctx.Typ == 64
                ? "8x bearbetad plan Ø21"
                : "Bara vissa typer";

            return new Dictionary<string, string>
            {
                ["SumRa32"] = "3.2",
                ["SumRa32a"] = "3.2",
                ["SumRa32b"] = "3.2",
                ["SumRa125"] = "12.5",
                ["SumPlan"] = plan
            };
        }

        private Dictionary<string, string> GetMachine(Context ctx)
        {
            if (string.IsNullOrEmpty(ctx.Machine))
                return new Dictionary<string, string>();

            var drilling = ctx.Machine == "Nakamura" || ctx.Machine == "MaxMuller"
                ? "K&T"
                : ctx.Machine;

            return new Dictionary<string, string>
            {
                ["SumMaskinValS1"] = $"Maskin: {ctx.Machine} - Svarvning",
                ["SumMaskinValS2"] = $"Maskin: {drilling} - Borrning, Fräsning"
            };
        }

        private Dictionary<string, string> GetFrequencies(Context ctx)
        {
            var v = ctx.Machine != "";

            return new Dictionary<string, string>
            {
                ["SumF1_1"] = v ? "1/2" : "",
                ["SumF1_2"] = v ? "1/2" : "",
                ["SumF1_3"] = v ? "1/2" : "",
                ["SumF1_4"] = v ? "1/2" : "",
                ["SumF1_5"] = v ? "Inst." : "",
                ["SumF1_6"] = v ? "Inst." : "",
                ["SumF1_7"] = v ? "Inst." : "",
                ["SumF1_8"] = v ? "1/2" : "",

                ["SumF2_1"] = v ? "1/2" : "",
                ["SumF2_2"] = v ? "1/2" : "",
                ["SumF2_3"] = v ? "1/2" : "",
                ["SumF2_4"] = v ? "Inst." : "",
                ["SumF2_5"] = v ? "1/5" : "",
                ["SumF2_6"] = v ? "Inst." : ""
            };
        }

        private Dictionary<string, string> GetMeasuringTools(Context ctx)
        {
            var v = ctx.Machine != "";

            return new Dictionary<string, string>
            {
                ["SumD1_1"] = v ? "Skjutmått" : "",
                ["SumD1_2"] = v ? "Skjutmått" : "",
                ["SumD1_3"] = v ? "Skjutmått" : "",
                ["SumD1_4"] = v ? "Skjutmått/Djupmått" : "",
                ["SumD1_5"] = v ? "Skjutmått/Djupmått" : "",
                ["SumD1_6"] = v ? "Radielyra" : "",
                ["SumD1_7"] = v ? "Vinkelsystem" : "",
                ["SumD1_8"] = v ? "Ytjämnhetsmätare" : "",

                ["SumD2_1"] = v ? "Gängtolk min/max" : "",
                ["SumD2_2"] = v ? "Skjutmått" : "",
                ["SumD2_3"] = v ? "Skjutmått" : "",
                ["SumD2_4"] = v ? "Mätmaskin" : "",
                ["SumD2_5"] = v ? "Ytjämnhetsmätare" : "",
                ["SumD2_6"] = v ? "Skjutmått/djupmått" : ""
            };
        }

        private Dictionary<string, string> GetRemarks(Context ctx)
        {
            var v = ctx.Machine != "";

            return new Dictionary<string, string>
            {
                ["SumAF1_1"] = "",
                ["SumAF1_2"] = "",
                ["SumAF1_3"] = "",
                ["SumAF1_4"] = "",
                ["SumAF1_5"] = "",
                ["SumAF1_6"] = "",
                ["SumAF1_7"] = "",
                ["SumAF1_8"] = "",

                ["SumAF2_1"] = "",
                ["SumAF2_2"] = "",
                ["SumAF2_3"] = "",
                ["SumAF2_4"] = v ? "Vid misstänkt fel lämna till mätrum." : "",
                ["SumAF2_5"] = "",
                ["SumAF2_6"] = ""
            };
        }

        private Dictionary<string, string> GetTexts(Context ctx)
        {
            var drawing = Is0006(ctx)
                ? $"{ctx.DesignationNormalized}:senaste utgåva"
                : "7433525:senaste utgåva";

            return new Dictionary<string, string>
            {
                ["SumTextS1"] = "Skarpa kanter avgradas.",
                ["SumTextS2"] = "Okulär kontroll av bearbetade ytor, vid misstänkt formfel lämnas hylsan till mätrum för kontroll.<<LineBreak>>Skarpa kanter avgradas.",
                ["SumRitS1"] = drawing,
                ["SumRitS2"] = drawing
            };
        }

        private bool IsZero(string value)
        {
            return ToDouble(value) == 0;
        }

        private double ToDouble(string value)
        {
            if (double.TryParse(
                    (value ?? "0").Replace(',', '.'),
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out var d))
            {
                return d;
            }

            return 0;
        }

        private string F3(double value)
        {
            return value.ToString("0.000", CultureInfo.InvariantCulture);
        }

        private string F1(double value)
        {
            return value.ToString("0.0", CultureInfo.InvariantCulture);
        }

        private string PlusF3(double value)
        {
            return "+ " + F3(value);
        }

        private string MinusF3(double value)
        {
            return "- " + F3(value);
        }

        private string PlusMinusF3(double value)
        {
            return "± " + F3(value);
        }

        private string PlusZeroF1()
        {
            return "+ " + F1(0);
        }

        private string MinusZeroF1()
        {
            return "- " + F1(0);
        }

        private string GetTolNumeric(string value)
        {
            var d = ToDouble(value);

            if (d < 6.01) return F3(0.100);
            if (d < 30.01) return F3(0.200);

            return F3(0.300);
        }

        private string GetTolPMSimple(string value)
        {
            var d = ToDouble(value);

            if (d < 6.01) return PlusMinusF3(0.100);
            if (d < 30.01) return PlusMinusF3(0.200);
            if (d < 120.01) return PlusMinusF3(0.300);
            if (d < 315.01) return PlusMinusF3(0.500);
            if (d < 1000.01) return PlusMinusF3(0.800);
            if (d < 2000.01) return PlusMinusF3(1.200);

            return PlusMinusF3(2.000);
        }

        private string GetTolPM(string value)
        {
            var d = ToDouble(value);

            if (d < 6.01) return PlusMinusF3(0.100);
            if (d < 30.01) return PlusMinusF3(0.200);
            if (d < 120.01) return PlusMinusF3(0.300);
            if (d < 400.01) return PlusMinusF3(0.500);
            if (d < 1000.01) return PlusMinusF3(0.800);
            if (d < 2000.01) return PlusMinusF3(1.200);

            return PlusMinusF3(2.000);
        }

        private string GetTolPMB(string value)
        {
            var d = ToDouble(value);

            if (d < 6.01) return PlusMinusF3(0.100);
            if (d < 30.01) return PlusMinusF3(0.200);
            if (d < 120.01) return PlusMinusF3(0.300);
            if (d < 315.01) return PlusMinusF3(0.500);
            if (d < 1000.01) return PlusMinusF3(0.800);
            if (d < 2000.01) return PlusMinusF3(1.200);

            return PlusMinusF3(2.000);
        }

        private string GetNegTol(string value)
        {
            var d = ToDouble(value);

            if (d < 3.01) return MinusF3(0.060);
            if (d < 6.01) return MinusF3(0.075);
            if (d < 10.01) return MinusF3(0.090);
            if (d < 18.01) return MinusF3(0.110);
            if (d < 30.01) return MinusF3(0.130);
            if (d < 50.01) return MinusF3(0.160);
            if (d < 80.01) return MinusF3(0.190);
            if (d < 120.01) return MinusF3(0.220);
            if (d < 180.01) return MinusF3(0.250);
            if (d < 250.01) return MinusF3(0.290);
            if (d < 315.01) return MinusF3(0.320);
            if (d < 400.01) return MinusF3(0.360);
            if (d < 500.01) return MinusF3(0.400);
            if (d < 630.01) return MinusF3(0.440);
            if (d < 800.01) return MinusF3(0.500);
            if (d < 1000.01) return MinusF3(0.560);
            if (d < 1250.01) return MinusF3(0.660);
            if (d < 1600.01) return MinusF3(0.780);
            if (d < 2000.01) return MinusF3(0.920);
            if (d < 2500.01) return MinusF3(1.100);

            return MinusF3(1.350);
        }

        private string GetPosTolH(string value)
        {
            var d = ToDouble(value);

            if (d < 3.01) return PlusF3(0.060);
            if (d < 6.01) return PlusF3(0.075);
            if (d < 10.01) return PlusF3(0.090);
            if (d < 18.01) return PlusF3(0.110);
            if (d < 30.01) return PlusF3(0.130);
            if (d < 50.01) return PlusF3(0.160);
            if (d < 80.01) return PlusF3(0.190);
            if (d < 120.01) return PlusF3(0.220);
            if (d < 180.01) return PlusF3(0.250);
            if (d < 250.01) return PlusF3(0.290);
            if (d < 315.01) return PlusF3(0.320);
            if (d < 400.01) return PlusF3(0.360);
            if (d < 500.01) return PlusF3(0.400);

            return PlusF3(0.440);
        }

        private string GetTolF(string value)
        {
            var d = ToDouble(value);

            if (d < 3.01) return PlusMinusF3(0.070);
            if (d < 6.01) return PlusMinusF3(0.090);
            if (d < 10.01) return PlusMinusF3(0.110);
            if (d < 18.01) return PlusMinusF3(0.135);
            if (d < 30.01) return PlusMinusF3(0.165);
            if (d < 50.01) return PlusMinusF3(0.195);
            if (d < 80.01) return PlusMinusF3(0.230);
            if (d < 120.01) return PlusMinusF3(0.270);
            if (d < 180.01) return PlusMinusF3(0.315);
            if (d < 250.01) return PlusMinusF3(0.360);
            if (d < 315.01) return PlusMinusF3(0.405);
            if (d < 400.01) return PlusMinusF3(0.445);
            if (d < 500.01) return PlusMinusF3(0.485);
            if (d < 630.01) return PlusMinusF3(0.550);
            if (d < 800.01) return PlusMinusF3(0.625);
            if (d < 1000.01) return PlusMinusF3(0.700);
            if (d < 1250.01) return PlusMinusF3(0.825);
            if (d < 1600.01) return PlusMinusF3(0.975);
            if (d < 2000.01) return PlusMinusF3(1.150);
            if (d < 2500.01) return PlusMinusF3(1.400);

            return PlusMinusF3(1.650);
        }

        private string GetJTol(Context ctx, string j)
        {
            return ctx.Typ < 84 ? F3(0.220) : F3(0.360);
        }

        private void Merge(Dictionary<string, string> target, Dictionary<string, string> source)
        {
            foreach (var kv in source)
            {
                target[kv.Key] = kv.Value ?? "";
            }
        }

        private class Context
        {
            public string Article { get; set; }
            public string Size { get; set; }
            public string DesignationNormalized { get; set; }
            public int Serie { get; set; }
            public int Typ { get; set; }
            public int TypIndex { get; set; }
            public string Machine { get; set; }
        }
    }
}