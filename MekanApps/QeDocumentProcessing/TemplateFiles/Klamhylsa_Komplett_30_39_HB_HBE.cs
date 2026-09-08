using QeDynamicDocumentProcessing.Common;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class Klamhylsa_Komplett_30_39_HB_HBE : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var result = new Dictionary<string, string>();
            var ctx = BuildContext(request);
            Merge(result, GetTurning(ctx));
            Merge(result, GetOilHoles(ctx));
            Merge(result, GetSlot(ctx));
            Merge(result, GetValidation(ctx));

            return result;
        }

        private Context BuildContext(APIRequest request)
        {
            var subjectRaw = request?.ProductDesignation ?? "";
            var machineRaw = FirstNonEmpty(GetString(request, "MaskinVal"), request?.MachineNumber ?? "");
            var subject = NormalizeSubject(subjectRaw);
            var tokens = subject.Split(new[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var hasSlash = subject.Contains("/");
            var hasV32 = subject.Contains("V32");
            var b1 = GetToken(tokens, 0);
            var b2 = GetToken(tokens, 1);
            var b3 = GetToken(tokens, 2);
            var b4 = GetToken(tokens, 3);
            var b5 = GetToken(tokens, 4);
            var serieText = ExtractSerie(hasSlash, b2);
            var typText = ExtractTyp(hasSlash, b2, b3);
            var serie = ToInt(serieText);
            var typ = ToDouble(typText);
            var dDefault = !hasSlash || b3.Length > 4 ? typ / 2.0 * 10.0 : ToDouble(b3);
            var d = GetDouble(request, "Avvikande YDia Gänga (d)", dDefault);

            var d1 = GetDouble(request, "Innerdiameter (d1)", double.NaN);
            var d2InputProvided = TryGetDouble(request, "Kona storände diameter (d2)", out var d2);
            var b = GetDouble(request, "Gänglängd (b)", double.NaN);
            var l = GetDouble(request, "Längd (L)", double.NaN);
            var konaInput = GetDouble(request, "Kona", 0);
            var aMatt = GetDouble(request, "a-mått", double.NaN);
            var productDrawing = FirstNonEmpty(GetString(request, "Produktritning"), "0");
            var threadType = FirstNonEmpty(GetString(request, "Gäng typ"), "");
            var radiusSmall = FirstNonEmpty(GetString(request, "Radie Lillände"), "0");
            var radiusBig = FirstNonEmpty(GetString(request, "Radie Storände"), "0");

            var missingInputs = new List<string>();

            if (double.IsNaN(d1) || Math.Abs(d1) < 0.000001)
            {
                d1 = CalculateDefaultInnerDiameterForHb(hasSlash, typ, b3, b4, b5);

                if (Math.Abs(d1) < 0.000001)
                    missingInputs.Add("Innerdiameter (d1)");
            }

            if (!d2InputProvided)
            {
                missingInputs.Add("Kona storände diameter (d2)");
                d2 = 0;
            }

            if (double.IsNaN(b) || Math.Abs(b) < 0.000001)
            {
                missingInputs.Add("Gänglängd (b)");
                b = 0;
            }

            if (double.IsNaN(l) || Math.Abs(l) < 0.000001)
            {
                missingInputs.Add("Längd (L)");
                l = 0;
            }

            if (double.IsNaN(aMatt) || Math.Abs(aMatt) < 0.000001)
            {
                missingInputs.Add("a-mått");
                aMatt = 0;
            }

            var kona = Math.Abs(konaInput) < 0.000001 ? (serie == 30 || serie == 31 || serie == 32 || serie == 39 ? 12.0 : 30.0) : konaInput;

            return new Context
            {
                Subject = subject,
                Tokens = tokens,
                HasSlash = hasSlash,
                HasV32 = hasV32,
                Bet1 = b1,
                Bet2 = b2,
                Bet3 = b3,
                Bet4 = b4,
                Bet5 = b5,
                Serie = serie,
                Typ = typ,
                D = d,
                D1 = d1,
                D2Input = d2,
                D2InputProvided = d2InputProvided,
                B = b,
                L = l,
                Kona = kona,
                AMatt = aMatt,
                Machine = NormalizeMachine(machineRaw),
                ProductDrawing = productDrawing,
                ThreadType = threadType,
                RadiusSmall = radiusSmall,
                RadiusBig = radiusBig,
                IsHb = new[] { b3, b4, b5 }.Any(IsHbToken),
                MissingInputs = missingInputs
            };
        }

        private Dictionary<string, string> GetTurning(Context ctx)
        {
            var d = ctx.D;
            var stmm = d < 301 ? 4 : d < 501 ? 5 : d < 701 ? 6 : d < 901 ? 7 : 8;
            var tmpKona = ctx.Kona;
            var lTolN = TolH15Negative(ctx.L);
            var bTol = ctx.B < 11 ? 1.5 : ctx.B < 19 ? 1.8 : ctx.B < 31 ? 2.1 : ctx.B < 51 ? 2.5 : ctx.B < 81 ? 3.0 : ctx.B < 121 ? 3.5 : 4.0;
            var sl = ctx.B + 4;
            var d1Tol = TolJs9(ctx.D1);
            var dOpTolN = stmm == 4 ? 0.3 : stmm == 5 ? 0.335 : stmm == 6 ? 0.375 : stmm == 7 ? 0.425 : 0.45;
            var dm = stmm == 4 ? d - 2 : stmm == 5 ? d - 2.5 : stmm == 6 ? d - 3 : stmm == 7 ? d - 3.5 : d - 4;
            var dmTol = stmm == 4 ? 0.19 : stmm == 5 ? 0.212 : stmm == 6 ? 0.236 : stmm == 7 ? 0.250 : 0.265;
            var dmTolN = stmm == 4 ? 0.63 : stmm == 5 ? 0.71 : stmm == 6 ? 0.8 : stmm == 7 ? 0.85 : 0.95;
            var d3 = stmm == 4 ? dm - 2.5 : stmm == 5 ? dm - 3 : stmm == 6 ? dm - 4 : stmm == 7 ? dm - 4.5 : dm < 1300 ? dm - 5 : dm - 4.5;
            var d3TolN = stmm == 4 ? 0.75 : stmm == 5 ? 0.85 : stmm == 6 ? 0.95 : stmm == 7 ? 1.0 : 1.12;
            var g = ctx.ProductDrawing == "MS-7434039" ? "4.4x30º" : d < 301 ? "2.7x45º" : d < 501 ? "3.2x45º" : d < 671 ? "3.8x45º" : d < 901 ? "4.4x45º" : "5x45º";
            var r1Value = ctx.RadiusSmall == "0" ? (d < 421 ? 1.0 : 2.5) : ToDouble(ctx.RadiusSmall);
            var rBigValue = ctx.RadiusBig == "0" ? (d < 320 ? 2.5 : d < 530 ? 3.5 : d < 710 ? 5.5 : 7.5) : ToDouble(ctx.RadiusBig);
            var gtjTol = GtjTol(ctx.Kona, d, true);
            var gtjTolN = GtjTol(ctx.Kona, d, false);
            var toleransSkillnad = gtjTolN - gtjTol;
            var d2a = ResolveD2(ctx, d, toleransSkillnad);
            var d2Tol = d2a < 6.01 ? 0.1 : d2a < 30.01 ? 0.2 : d2a < 120.01 ? 0.3 : d2a < 400.01 ? 0.5 : d2a < 1000.01 ? 0.8 : d2a < 2000.01 ? 1.2 : 2.0;
            var gVarTol = d > 1000 ? 0.05 : d > 800 ? 0.045 : d > 630 ? 0.04 : d > 500 ? 0.035 : d > 315 ? 0.03 : d > 250 ? 0.025 : d > 180 ? 0.02 : d > 120 ? 0.015 : d > 50 ? 0.01 : 0.008;
            var rakA = (d < 101 ? 8 : d < 281 ? 10 : d < 481 ? 12 : d < 601 ? 14 : d < 901 ? 16 : 20) / 1000.0;
            var rakB = (d < 101 ? 12 : d < 281 ? 15 : d < 481 ? 18 : d < 601 ? 21 : d < 901 ? 24 : 30) / 1000.0;
            var orund = ctx.D1 < 31 ? 0.026 : ctx.D1 < 51 ? 0.031 : ctx.D1 < 81 ? 0.037 : ctx.D1 < 121 ? 0.043 : ctx.D1 < 181 ? 0.05 : ctx.D1 < 251 ? 0.057 : ctx.D1 < 316 ? 0.065 : ctx.D1 < 401 ? 0.07 : ctx.D1 < 501 ? 0.077 : ctx.D1 < 631 ? 0.087 : ctx.D1 < 801 ? 0.1 : ctx.D1 < 1001 ? 0.115 : 0.13;
            var ml = ctx.L - ctx.B - 4;
            var vml = ml < 110 ? 75 : 100;
            var vinkTol = ((d > 1000 ? 0.09 : d > 800 ? 0.10 : d > 630 ? 0.11 : d > 500 ? 0.12 : d > 400 ? 0.13 : d > 180 ? 0.15 : d > 150 ? 0.18 : d > 120 ? 0.30 : d > 80 ? 0.45 : d > 50 ? 0.50 : 0.60) * vml) / 1000.0;
            var e1 = ml < 110 ? Bygel(ctx, 83) : ml < 145 ? Bygel(ctx, 108) : Bygel(ctx, 140);
            var e2 = ml < 145 ? Bygel(ctx, 8) : Bygel(ctx, 40);
            var l1 = ml < 110 ? 83 : ml < 145 ? 108 : 140;
            var l2 = ml < 145 ? 8 : 40;
            var bygel = ml < 145 ? "SR 7419471" : "SR 7415991";
            var machineOk = IsTurningMachine(ctx.Machine);
            var ritning = ctx.ProductDrawing == "0" ? DrawingBySerie(ctx) : ctx.ProductDrawing;
           
            return new Dictionary<string, string>
            {
                ["SumGänga"] = $"Tr{FmtPlainComma(d)}x{stmm}",
                ["SumP"] = $"Tr{stmm}",
                ["SumP1"] = $"(P) {stmm}",
                ["SumRullar"] = $"{stmm}mm",
                ["SumV"] = Math.Abs(tmpKona - 30) < 0.000001 ? "(V) 0º57" : "(V) 2º23",
                ["SumKonaOP1"] = $"Kona 1:{FmtPlain(tmpKona)}",
                ["SumKona"] = $"Kona 1:{FmtPlain(tmpKona)}",
                ["SumLOP1"] = $"(L) {FmtPlainComma(ctx.L)}",
                ["SumLOP1Tol"] = "0.250",
                ["SumL"] = $"(L) {FmtPlainComma(ctx.L)}",
                ["SumLTol"] = "+ 0.000 [3F]",
                ["SumLTolN"] = $"- {Fmt3(lTolN)} [3F]",
                ["Sumb"] = $"(b) {FmtPlainComma(ctx.B)}",
                ["SumbTol"] = $"+ {Fmt3(bTol)}  [3F]",
                ["SumbTolN"] = "- 0.000 [2F]",
                ["SumSL"] = $"(SL) {FmtPlainComma(sl)}",
                ["SumSLTol"] = "± 0.300",
                ["Sumd1"] = $"(d1) {FmtPlainComma(ctx.D1)}",
                ["Sumd1Tol"] = $"± {Fmt3(d1Tol)} [3F]",
                ["SumdOP1"] = $"(d) {FmtPlainComma(d)}",
                ["SumdOP1Tol"] = "+ 0.000",
                ["SumdOP1TolN"] = $"- {Fmt3(dOpTolN)}",
                ["SumdaOP1"] = $"(d) {FmtPlainComma(d)}",
                ["SumdaOP1Tol"] = "+ 0.000",
                ["SumdaOP1TolN"] = $"- {Fmt3(dOpTolN)}",
                ["Sumd"] = $"(d) {FmtPlainComma(d)}",
                ["SumdTol"] = "+ 0.000",
                ["SumdTolN"] = $"- {Fmt3(dOpTolN)}",
                ["Sumdm"] = $"(dm) {FmtPlain(dm)}",
                ["SumdmTol"] = $"- {Fmt3(dmTol)} [3F]",
                ["SumdmTolN"] = $"- {Fmt3(dmTolN)} [3F]",
                ["Sumd3"] = $"(d3) {FmtPlain(d3)}",
                ["Sumd3Tol"] = "+ 0.000",
                ["Sumd3TolN"] = $"- {Fmt3(d3TolN)}",
                ["Sumg"] = $"(g) {g}",
                ["Sumr1"] = $"R{FmtPlain(r1Value)}",
                ["Sumr"] = $"R{FmtPlain(rBigValue)}",
                ["SumGTjTol"] = $"+ {Fmt3(gtjTol)}",
                ["SumGTjaTol"] = $"+ {Fmt3(gtjTol)}",
                ["SumGTjTolN"] = $"- {Fmt3(gtjTolN)}",
                ["SumGTjaTolN"] = $"- {Fmt3(gtjTolN)}",
                ["Sumd2"] = $"(d2) {FmtPlain(d2a)}",
                ["Sumd2Tol"] = $"± {Fmt3(d2Tol)}",
                ["SumGVarTol"] = $"{Fmt3(gVarTol)} [2F]",
                ["SumRakA"] = $"Max: {FmtPlain(rakA)}",
                ["SumRakB"] = $"Max: {FmtPlain(rakB)}",
                ["SumOrund"] = $"{Fmt3(orund)} [3F]",
                ["SumML"] = $"Mätlängd={(ml < 110 ? "75" : "100")}",
                ["SumRa"] = "2.5 [2F]",
                ["SumRa1"] = "2.5 [2F]",
                ["SumRa5"] = "5 [2F]",
                ["SumVinkTol"] = $"{FmtPlain(vinkTol)} [2F]",
                ["SumL1"] = l1.ToString(CultureInfo.InvariantCulture),
                ["SumL2"] = l2.ToString(CultureInfo.InvariantCulture),
                ["SumE1"] = FmtPlainComma(e1),
                ["SumE2"] = FmtPlainComma(e2),
                ["SumBygGtj"] = bygel,
                ["SumBygGVar"] = bygel,
                ["SumBygVinkTol"] = bygel,
                ["SumMaskinValS1"] = machineOk ? $"Maskin: {ctx.Machine} - OP1" : "Maskin:  - OP1",
                ["SumMaskinValS2"] = machineOk ? $"Maskin: {ctx.Machine} - OP2" : "Maskin:  - OP2",
                ["SumF3_1"] = machineOk ? "1/5" : "",
                ["SumF3_2"] = machineOk ? "1/2" : "",
                ["SumF3_3"] = machineOk ? "1/1" : "",
                ["SumF3_4"] = machineOk ? "1/5" : "",
                ["SumF3_5"] = machineOk ? "Inst." : "",
                ["SumF3_6"] = machineOk ? "1/5" : "",
                ["SumF3_7"] = machineOk ? "1/2" : "",
                ["SumF3_8"] = machineOk ? "1/5" : "",
                ["SumF4_1"] = machineOk ? "1/2" : "",
                ["SumF4_2"] = machineOk ? "1/2" : "",
                ["SumF4_3"] = machineOk ? "1/1" : "",
                ["SumF4_4"] = machineOk ? "1/1" : "",
                ["SumF4_5"] = machineOk ? "1/1" : "",
                ["SumF4_6"] = machineOk ? "Inst." : "",
                ["SumF4_7"] = machineOk ? "1/5" : "",
                ["SumF4_8"] = machineOk ? "1/5" : "",
                ["SumF4_9"] = machineOk ? "1/1" : "",
                ["SumD3_1"] = machineOk ? "Skjutmått" : "",
                ["SumD3_2"] = machineOk ? "Skjutmått" : "",
                ["SumD3_3"] = machineOk ? $"Multimar med {stmm}mm rullar" : "",
                ["SumD3_4"] = machineOk ? "Djupmått" : "",
                ["SumD3_5"] = machineOk ? "Radielyra" : "",
                ["SumD3_6"] = machineOk ? "Skjutmått/Vinkelmätare" : "",
                ["SumD3_7"] = machineOk ? $"Gängmall Tr{stmm}" : "",
                ["SumD3_8"] = machineOk ? "Skjutmått" : "",
                ["SumD4_1"] = machineOk ? "Mikrometerstickmått" : "",
                ["SumD4_2"] = machineOk ? "Skjutmått" : "",
                ["SumD4_3"] = machineOk ? $"Mätbygel {bygel}" : "",
                ["SumD4_4"] = machineOk ? $"Mätbygel {bygel}" : "",
                ["SumD4_5"] = machineOk ? $"Mätbygel {bygel}" : "",
                ["SumD4_6"] = machineOk ? "Mätmaskin" : "",
                ["SumD4_7"] = machineOk ? "Egglinjal" : "",
                ["SumD4_8"] = machineOk ? "Egglinjal" : "",
                ["SumD4_9"] = machineOk ? "Okulärkontroll" : "",
                ["SumAF3_1"] = "",
                ["SumAF3_2"] = "",
                ["SumAF3_3"] = machineOk ? "Kontrolleras med klove utf.2" : "",
                ["SumAF3_4"] = "",
                ["SumAF3_5"] = "",
                ["SumAF3_6"] = "",
                ["SumAF3_7"] = "",
                ["SumAF3_8"] = machineOk ? "Hjälpmått" : "",
                ["SumAF4_1"] = "",
                ["SumAF4_2"] = "",
                ["SumAF4_3"] = machineOk ? "Tol:" : "",
                ["SumAF4_4"] = machineOk ? $"Tol: {Fmt3(gVarTol)} [2F]" : "",
                ["SumAF4_5"] = machineOk ? $"Tol: {(ctx.Bet2 == "3976" ? "0.01125" : FmtPlain(vinkTol))} [2F] Mätlängd={(ml < 110 ? "75" : "100")}" : "",
                ["SumAF4_6"] = machineOk ? $"Max: {Fmt3(orund)} [3F]" : "",
                ["SumAF4_7"] = machineOk ? $"Max: {FmtPlain(rakA)}" : "",
                ["SumAF4_8"] = machineOk ? $"Max: {FmtPlain(rakB)}" : "",
                ["SumAF4_9"] = machineOk ? "Vid misstänkt fel Ra-mätare" : "",
                ["SumTextS1"] = "Kontrollera rätt märkning<<LineBreak>>Okulärkontroll gjuteridefekter, grader & slagmärken<<LineBreak>>Gjutgodsdefekter: 7433015",
                ["SumTextS2"] = "Bryt alla kanter, avlägsna. Okulärkontroll märkning, gjuteridefekter, grader & slagmärken",
                ["SumRitningsnrS1"] = ritning + ":senaste utg.",
                ["SumRitningsnrS2"] = ritning + ":senaste utg.",
                ["SumRitTolS1"] = "Toleranser: 1432012:7",
                ["SumRitTolS2"] = "Toleranser: 1432012:7",
                ["SumRitGänga"] = "Gänga: 237359:2, 7430181:2",
                ["SumKlEgenskaperS1"] = "PRODUCTION NUTS & SLEEVES & HOUSINGS\\ALLMÄNKLASSADE EGENSKAPER\\Klassade egenskaper klämhylsor",
                ["SumKlEgenskaperS2"] = "PRODUCTION NUTS & SLEEVES & HOUSINGS\\ALLMÄNKLASSADE EGENSKAPER\\Klassade egenskaper klämhylsor"
            };
        }

        private Dictionary<string, string> GetOilHoles(Context ctx)
        {
            var idx = TypOilIndex(ctx);
            var b1 = Pick(BTable(ctx), idx);
            var c1 = ctx.ThreadType == "M6" ? 10.0 : ctx.ThreadType == "M8" ? 12.0 : ctx.ThreadType == "G1/8" ? 12.0 : 0.0;
            var t = ctx.ThreadType == "M6" ? 6.3 : ctx.ThreadType == "M8" ? 8.3 : ctx.ThreadType == "G1/8" ? 10.0 : 0.0;
            var s = ctx.ThreadType == "M6" ? 3.0 : ctx.ThreadType == "M8" ? 4.0 : ctx.ThreadType == "G1/8" ? 5.0 : 0.0;
            var h = ctx.Serie == 30 ? (ctx.Typ < 37 ? 0.8 : ctx.Typ < 65 ? 1 : ctx.Typ < 85 ? 1.2 : ctx.Typ < 751 ? 1.5 : 2) : (ctx.Typ < 37 ? 0.8 : ctx.Typ < 65 ? 1 : ctx.Typ < 85 ? 1.2 : ctx.Typ < 601 ? 1.5 : ctx.Typ < 751 ? 2 : 2.8);
            var e = Pick(ETable(ctx), idx);
            var j = Pick(JTable(ctx), idx);
            var f = ctx.Typ < 55 ? 2.0 : 3.0;
            var n = ctx.Typ < 37 ? 4.0 : ctx.Typ < 63 ? 5.3 : ctx.Typ < 85 ? 6.2 : ctx.Typ < 601 ? 7.0 : ctx.Typ < 751 ? 8.0 : 9.0;
            var r8 = ctx.Typ < 37 ? 3.0 : ctx.Typ < 65 ? 4.0 : ctx.Typ < 85 ? 4.5 : 5.0;
            var r7 = ctx.Typ < 85 ? 1.0 : 2.5;
            var d4 = DiameterFromSubjectForHb(ctx.HasSlash, ctx.Typ, ctx.Bet3, ctx.Bet4, ctx.Bet5);
            var d5 = InnerDiameterFromD4AndTyp(d4, ctx.Typ);
            var korda = Round(((d5 + (b1 * 2)) / 2) * Round(Math.Sin((135.0 / 2.0) * Math.PI / 180.0) * 2, 4), 1);
            var machineOk = IsOilMachine(ctx.Machine);
            var skepp = ctx.Machine == "Skepp6";
            var rit = ctx.Serie == 30 ? (ctx.Typ < 44 ? "7434155" : "7434156") : ctx.Serie == 31 ? (ctx.Typ < 44 ? "7434157" : "7434158") : ctx.Serie == 23 || ctx.Serie == 32 ? (ctx.Typ < 44 ? "7434159" : "7434160") : ctx.Serie == 39 ? "7434168" : "Ingen ritning hittad";

            return new Dictionary<string, string>
            {
                ["SumRitNr"] = rit,
                ["SumG1"] = $" {ctx.ThreadType} ",
                ["SumB1"] = $"(B) {FmtPlain(b1)}",
                ["SumB1Tol"] = Math.Abs(b1 - 4) < 0.000001 ? "- 0.1" : "   0",
                ["SumB1TolN"] = Math.Abs(b1 - 4) < 0.000001 ? "- 0.3" : "- 0.1",
                ["SumC1"] = $"(C) {(c1 == 0 ? "n/a" : FmtPlain(c1))}",
                ["SumC1Tol"] = LinearTol(c1),
                ["SumT"] = $"(T) {(t == 0 ? "n/a" : FmtPlain(t))}",
                ["SumTTol"] = LinearTol(t),
                ["SumS"] = $"(D) {(s == 0 ? "n/a" : FmtPlain(s))}",
                ["SumSTol"] = LinearTol(s),
                ["SumH"] = $"(H) {FmtPlain(h)}",
                ["SumHTol"] = h < 6.1 ? "± 0.1" : "± 0.2",
                ["SumE"] = $"(E) {FmtPlainComma(e)}",
                ["SumETol"] = TolByLength(e),
                ["SumJ"] = $"(J) {FmtPlainComma(j)}",
                ["SumJTol"] = TolByLength(j),
                ["SumF"] = $"(F) {FmtPlain(f)}",
                ["SumF3"] = $"(F) {FmtPlain(f)}",
                ["SumFTol"] = h < 6.1 ? "± 0.1" : "± 0.2",
                ["SumF3Tol"] = h < 6.1 ? "± 0.1" : "± 0.2",
                ["SumN"] = $"(N) {FmtPlainComma(n)}",
                ["SumNTol"] = h < 6.1 ? "± 0.1" : "± 0.2",
                ["SumR8"] = $"R{FmtPlain(r8)}",
                ["SumR7"] = $"R{FmtPlain(r7)}",
                ["SumV120"] = "120º",
                ["SumV45"] = "45º",
                ["Sum1V30"] = "~30º",
                ["SumMaskinValS3"] = $"Maskin: {(machineOk ? ctx.Machine : "")} - BorrOljehål & Oljespår",
                ["SumMaskinValS4"] = $"Maskin: {(machineOk ? ctx.Machine : "")} - Oljespår & Repor",
                ["SumF2_1"] = machineOk ? (skepp ? "1/1" : "1/2") : "",
                ["SumF2_2"] = machineOk ? (skepp ? "1/1" : "1/2") : "",
                ["SumF2_3"] = machineOk ? (skepp ? "1/1" : "1/2") : "",
                ["SumF2_4"] = machineOk ? (skepp ? "1/1" : "1/2") : "",
                ["SumF2_5"] = machineOk ? (skepp ? "1/1" : "1/2") : "",
                ["SumF2_6"] = machineOk ? (skepp ? "1/1" : "1/2") : "",
                ["SumF2_7"] = machineOk ? (skepp ? "1/1" : "1/2") : "",
                ["SumF2_8"] = machineOk ? (skepp ? "1/1" : "1/2") : "",
                ["SumF2_9"] = machineOk ? (skepp ? "1/1" : "1/2") : "",
                ["SumF2_0"] = machineOk ? (skepp ? "1/1" : "1/2") : "",
                ["SumF2_11"] = machineOk ? (skepp ? "1/1" : "1/2") : "",
                ["SumD2_1"] = machineOk ? (skepp ? "Skala på borrmaskin" : "Pipborr/Djupmått") : "",
                ["SumD2_2"] = machineOk ? "Skjutmått" : "",
                ["SumD2_3"] = machineOk ? "Skjutmått" : "",
                ["SumD2_4"] = machineOk ? "Gängtolk" : "",
                ["SumD2_5"] = machineOk ? "Skjutmått" : "",
                ["SumD2_6"] = machineOk ? "Skjutmått/fasmall" : "",
                ["SumD2_7"] = machineOk ? "Skjutmått" : "",
                ["SumD2_8"] = machineOk ? "Skjutmått" : "",
                ["SumD2_9"] = machineOk ? (skepp ? "Höjdrits" : "Pipborr/Djupmått") : "",
                ["SumD2_0"] = machineOk ? "Skjutmått" : "",
                ["SumD2_11"] = machineOk ? "Radieyra" : "",
                ["SumAF2_1"] = "",
                ["SumAF2_2"] = "",
                ["SumAF2_3"] = "",
                ["SumAF2_4"] = "",
                ["SumAF2_5"] = "",
                ["SumAF2_6"] = "",
                ["SumAF2_7"] = "",
                ["SumAF2_8"] = "",
                ["SumAF2_9"] = "",
                ["SumAF2_0"] = "",
                ["SumAF2_11"] = "",
                ["SumTextS3"] = "",
                ["SumTextS4"] = "Grader, frifläckar, slagmärken, repor, valkar och andra defekter samt ojämnheter kontrolleras med ögonmått.",
                ["SumTextGängstigning"] = "Max 2x gängstigning",
                ["SumÖvrigt"] = $"2st. oljeborrhål, Korda (A) = {FmtPlainComma(korda)}",
                ["SumORM"] = "Stämpla Oljeriktningsmarkeringar"
            };
        }

        private Dictionary<string, string> GetSlot(Context ctx)
        {
            var idx = TypSlotIndex(ctx.Typ);
            var c = ctx.Serie == 31 ? (ctx.Typ < 64 ? 4 : ctx.Typ < 93 ? 8 : 10) : ctx.Serie == 39 ? (ctx.Typ < 530 ? 8 : 10) : ctx.Typ < 501 ? 8 : 10;
            var d10Tol = ctx.Typ < 65 ? "+ 0.210" : ctx.Typ < 85 ? "+ 0.360" : ctx.Typ < 531 ? "+ 0.400" : ctx.Typ < 671 ? "+ 0.440" : ctx.Typ < 851 ? "+ 0.500" : "+ 0.560";
            var d10TolN = ctx.Typ < 65 ? "- 0.320" : ctx.Typ < 85 ? "- 0.570" : ctx.Typ < 531 ? "- 0.630" : ctx.Typ < 671 ? "- 0.700" : ctx.Typ < 851 ? "- 0.800" : "- 0.900";
            var f0 = Pick(F0Table(ctx), idx);
            var e5 = Pick(E5Table(ctx), idx);
            var f0Tol = f0 > 50 ? "+ 3.0" : f0 > 30 ? "+ 2.5" : f0 > 19 ? "+ 2.1" : "+ 1.8";
            var e5Tol = e5 > 50 ? "+ 0.740" : e5 > 30 ? "+ 0.620" : e5 > 18 ? "+ 0.520" : e5 > 10 ? "+ 0.430" : e5 > 6 ? "+ 0.360" : "+ 0.300";
            var machineOk = ctx.Machine == "Skepp6" || ctx.Machine == "K&T" || ctx.Machine == "VTR-160" || ctx.Machine == "MacTurn 550";
            var rit = ctx.Serie == 30 ? "7438957" : ctx.Serie == 31 ? "7438958" : ctx.Serie == 32 ? "7438955" : ctx.Serie == 39 ? "7434032" : ctx.Serie == 241 ? "7432903" : ctx.Serie == 240 ? "7432901" : "Fel Mall";

            return new Dictionary<string, string>
            {
                ["SumVa"] = "11.25º",
                ["SumV1"] = "11.25º",
                ["SumV30"] = "30º",
                ["SumC"] = $"(c) {FmtPlain(c)}",
                ["SumCTol"] = "± 0.2",
                ["Sumd10"] = $"(d1) {FmtPlainComma(ctx.D1)}",
                ["Sumd10Tol"] = d10Tol,
                ["Sumd10TolN"] = d10TolN,
                ["SumF0"] = $"(f) {FmtPlain(f0)}",
                ["SumF02"] = $"(f) {FmtPlain(f0)}",
                ["SumF0Tol"] = f0Tol,
                ["SumF02Tol"] = f0Tol,
                ["SumF0TolN"] = "  0 [3F]",
                ["SumF02TolN"] = "  0 [3F]",
                ["SumE5"] = $"(e) {FmtPlain(e5)}",
                ["SumE5Tol"] = e5Tol,
                ["SumE5TolN"] = " 0 [3F]",
                ["SumMaskinValS5"] = $"Maskin: {(machineOk ? ctx.Machine : "")} - Muttersäkring, Slits",
                ["SumF1_1"] = machineOk ? (ctx.Machine == "Skepp6" ? "1/1" : "1/2") : "",
                ["SumF1_2"] = machineOk ? (ctx.Machine == "Skepp6" ? "1/1" : "1/2") : "",
                ["SumF1_3"] = machineOk ? (ctx.Machine == "Skepp6" ? "1/1" : "1/2") : "",
                ["SumF1_4"] = machineOk ? (ctx.Machine == "Skepp6" ? "1/1" : "1/2") : "",
                ["SumF1_5"] = machineOk ? (ctx.Machine == "Skepp6" ? "1/1" : "1/2") : "",
                ["SumD1_1"] = machineOk ? "Skjutmått" : "",
                ["SumD1_2"] = machineOk ? "Skjutmått" : "",
                ["SumD1_3"] = machineOk ? "Skjutmått" : "",
                ["SumD1_4"] = "",
                ["SumD1_5"] = machineOk ? "Skjutmått" : "",
                ["SumAF1_1"] = "",
                ["SumAF1_2"] = "",
                ["SumAF1_3"] = "",
                ["SumAF1_4"] = "",
                ["SumAF1_5"] = "",
                ["SumTextS4"] = "",
                ["SumRitNr4"] = rit
            };
        }

        private Dictionary<string, string> GetValidation(Context ctx)
        {
            var missing = ctx.MissingInputs != null && ctx.MissingInputs.Count > 0
                ? "Saknade indata: " + string.Join(", ", ctx.MissingInputs)
                : "";

            return new Dictionary<string, string>
            {
                ["VaL241"] = ctx.Serie == 30 || ctx.Serie == 31 || ctx.Serie == 32 || ctx.Serie == 39 ? "" : "Denna Mall är endast för OH 23, 30,31,32,39 serien",
                ["VaLHB"] = ctx.IsHb ? "" : "Denna Mall är endast för typ OH-HB(E)",
                ["VaLMissingInputs"] = missing
            };
        }

        private string NormalizeSubject(string value)
        {
            return WebUtility.HtmlDecode(value ?? "").ToUpperInvariant().Replace('.', ',').Trim();
        }

        private string ExtractSerie(bool hasSlash, string bet2)
        {
            if (!hasSlash)
            {
                if (bet2.Length > 4) return Left(bet2, 3);
                if (bet2.Length == 3) return Left(bet2, 1);
                return Left(bet2, 2);
            }

            if (bet2.Length == 3 || bet2.Length == 2) return bet2;
            if (bet2.Length > 4) return Left(bet2, 3);
            return Left(bet2, 2);
        }

        private string ExtractTyp(bool hasSlash, string bet2, string bet3)
        {
            if (!hasSlash) return Right(bet2, 2);
            return bet3.Length > 4 ? Right(bet2, 2) : bet3;
        }

        private string NormalizeMachine(string machine)
        {
            var m = WebUtility.HtmlDecode(machine ?? "").Trim();
            return m == "MacTurn 550" ? "MacTurn 550" :
                   m == "VTR-160" ? "VTR-160" :
                   m == "Skepp6" ? "Skepp6" :
                   m == "K&T" ? "K&T" :
                   m == "Dubbelparet" ? "Dubbelparet" :
                   "";
        }

        private bool IsTurningMachine(string m) => m == "MacTurn 550" || m == "VTR-160";
        private bool IsOilMachine(string m) => m == "Skepp6" || m == "K&T" || m == "VTR-160" || m == "MacTurn 550" || m == "Dubbelparet";

        private int TypOilIndex(Context ctx)
        {
            var list = ctx.Serie == 30 || ctx.Serie == 31 ? new[] { "44", "48", "52", "56", "60", "64", "68", "72", "76", "80", "84", "88", "92", "96", "500", "530", "560", "600", "630", "670", "710", "750", "800", "850", "900" }
                : ctx.Serie == 23 ? new[] { "44", "48", "52", "56" }
                : ctx.Serie == 32 ? new[] { "60", "64", "68", "72", "76", "80", "84", "88", "92", "96", "500", "530", "560", "600", "630", "670", "710", "750", "800", "850" }
                : ctx.Serie == 39 ? new[] { "44", "48", "52", "56", "60", "64", "68", "72", "76", "80", "84", "88", "92", "96", "500", "530", "560", "600", "630", "670", "710", "750", "800", "850", "900", "863,6" }
                : Array.Empty<string>();
            var typText = FmtPlain(ctx.Typ).Replace('.', ',');
            var idx = Array.IndexOf(list, typText);
            return idx < 0 ? 0 : idx;
        }

        private int TypSlotIndex(double typ)
        {
            var list = new[] { 60.0, 64.0, 68.0, 72.0, 76.0, 80.0, 84.0, 88.0, 92.0, 96.0, 500.0, 530.0, 560.0, 600.0, 630.0, 670.0, 710.0, 750.0, 800.0, 850.0, 900.0, 950.0, 1000.0, 1060.0 };
            for (var x = 0; x < list.Length; x++)
                if (Math.Abs(list[x] - typ) < 0.000001) return x;
            return 0;
        }

        private double[] BTable(Context ctx)
        {
            if (ctx.Serie == 30) return ctx.HasV32 ? A(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 11, 0, 0, 0, 0, 0, 0, 0) : A(3.9, 3.9, 3.9, 3.9, 3.9, 3.5, 3.5, 3.5, 3.5, 3.5, 6.5, 6.5, 6.5, 6.5, 6.5, 6, 8, 6, 8, 8, 8, 10, 10, 10);
            if (ctx.Serie == 31) return A(3.9, 3.9, 3.9, 3.9, 3.9, 3.5, 3.5, 3.5, 3.5, 3.5, 6.5, 6.5, 6.5, 6.5, 6.5, 6, 8, 6, 8, 8, 8, 10, 10, 10);
            if (ctx.Serie == 23) return A(3.9, 3.9, 3.9, 3.9);
            if (ctx.Serie == 32) return A(3.9, 3.5, 3.5, 3.5, 3.5, 3.5, 6.5, 6.5, 6.5, 6.5, 6, 6, 8, 6, 8, 8, 8, 10, 10);
            if (ctx.Serie == 39) return A(3.9, 3.9, 3.9, 3.9, 3.9, 3.5, 3.5, 3.5, 3.5, 3.5, 6.5, 6.5, 6.5, 6.5, 6.5, 6, 8, 6, 8, 8, 8, 10, 10, 10, 8);
            return A(0);
        }

        private double[] ETable(Context ctx)
        {
            if (ctx.Serie == 30) return A(74, 79, 84, 89, 98, 100, 109, 109, 113, 122, 123, 135, 138, 139, 147, 155.5, 169, 171, 176.5, 189.5, 202, 208.5, 212, 218.5, 232);
            if (ctx.Serie == 31) return A(92, 98, 107, 110, 115, 124, 144, 148, 151, 156, 175, 176, 187, 191, 203, 206.5, 217, 226, 243, 263, 267.5, 281.5, 287, 304, 316.5);
            if (ctx.Serie == 23) return A(104, 110, 117, 123);
            if (ctx.Serie == 32) return A(130, 139, 160, 166, 172, 181, 196, 200, 212, 219, 235, 244, 255.5, 265.5, 286.5, 309, 314.5, 332, 337.5, 356);
            if (ctx.Serie == 39) return A(60, 64, 71, 75, 86, 86, 89, 89, 99, 103, 103, 117, 117, 122, 130, 134, 143, 147.5, 154.5, 161.5, 176, 178.5, 183, 185, 197.5, 197.5);
            return A(0);
        }

        private double[] JTable(Context ctx)
        {
            if (ctx.Serie == 30) return A(70.5, 75.5, 81, 85.5, 95, 96.5, 105, 105.5, 109, 118.5, 119.5, 130.5, 133.5, 134.5, 143, 151.5, 163, 165, 170.5, 183.5, 196, 202.5, 206, 212.5, 226);
            if (ctx.Serie == 31) return A(89, 94.5, 104, 106.5, 112, 121, 140.5, 144.5, 147.5, 152, 171, 171.5, 183, 186.5, 199, 202.5, 211, 220, 237, 257, 261.5, 276.5, 281, 298, 310.5);
            if (ctx.Serie == 23) return A(100.5, 107, 113.5, 120);
            if (ctx.Serie == 32) return A(126.5, 135.5, 156, 162.5, 168, 177, 192.5, 196, 208, 214.5, 231, 240, 249.5, 259.5, 280.5, 303, 308.5, 326, 331.5, 350);
            if (ctx.Serie == 39) return A(57, 61, 68, 72, 82.5, 82.5, 85.5, 85.5, 95.5, 99.5, 99.5, 113, 113, 117.5, 125.5, 129, 138, 142.5, 149.5, 156.5, 171, 173.5, 178, 180, 192.5, 192.5);
            return A(0);
        }

        private double[] F0Table(Context ctx)
        {
            if (ctx.Serie == 240) return A(0, 34, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 50, 0, 53, 0, 0, 0, 0, 60, 0, 0);
            if (ctx.Serie == 241) return A(0, 35, 36, 0, 0, 36, 0, 0, 0, 42, 34, 0, 50, 50, 52, 53, 0, 0, 0, 0, 0, 0, 0);
            return A(22, 25, 26, 26, 26, 27, 27, 27, 28, 28, 28, 34, 34, 35, 35, 36, 38, 38, 39, 39, 40, 42, 44, 44);
        }

        private double[] E5Table(Context ctx)
        {
            if (ctx.Serie == 240) return A(24, 24, 28, 32, 32, 32, 36, 36, 36, 40, 40, 45, 45, 50, 50, 55, 60, 60, 70, 70, 60, 70, 70);
            if (ctx.Serie == 31 || ctx.Serie == 32 || ctx.Serie == 241) return A(24, 24, 28, 28, 32, 32, 32, 36, 36, 36, 40, 40, 45, 45, 50, 50, 55, 60, 60, 70, 70, 70, 70, 70);
            if (ctx.Serie == 30 || ctx.Serie == 39) return A(24, 24, 28, 28, 28, 32, 32, 32, 36, 36, 40, 40, 40, 45, 45, 50, 55, 55, 60, 60, 60, 60, 60);
            return A(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        }

        private double ResolveD2(Context ctx, double d, double toleransSkillnad)
        {
            if (ctx.D2InputProvided && Math.Abs(ctx.D2Input) > 0.000001)
                return ctx.D2Input;

            if (ctx.D2InputProvided)
                return Round(((ctx.L - 1 - ctx.AMatt) / ctx.Kona) + d - toleransSkillnad, 2);

            return 0;
        }

        private double Bygel(Context ctx, double x) => Round(((ctx.D - ctx.D1) / 2.0) + ((ctx.L - 1.0 - ctx.AMatt - x) / (2.0 * ctx.Kona)), 3);
        private double Round(double v, int decimals) => Math.Round(v, decimals, MidpointRounding.AwayFromZero);
        private double Pick(double[] values, int index)
        {
            if (values == null || values.Length == 0) return 0;
            if (index < 0 || index >= values.Length) return 0;
            return values[index];
        }
        private double[] A(params double[] values) => values;

        private string LinearTol(double v) => v < 6.1 ? "± 0.1" : v < 30.1 ? "± 0.2" : "± 0.3";
        private string TolByLength(double v) => v < 6.1 ? "± 0.1" : v < 30.1 ? "± 0.2" : v < 120.1 ? "± 0.3" : v < 315.1 ? "± 0.5" : v < 1000.1 ? "± 0.8" : v < 2000.1 ? "± 1.2" : "± 2.0";

        private double TolH15Negative(double v)
        {
            return v < 3.01 ? 0.400 : v < 6.01 ? 0.480 : v < 10.01 ? 0.580 : v < 18.01 ? 0.700 : v < 30.01 ? 0.840 : v < 50.01 ? 1.000 : v < 80.01 ? 1.200 : v < 120.01 ? 1.400 : v < 180.01 ? 1.600 : v < 250.01 ? 1.850 : v < 315.01 ? 2.100 : v < 400.01 ? 2.300 : v < 500.01 ? 2.500 : v < 630.01 ? 2.800 : v < 800.01 ? 3.200 : v < 1000.01 ? 3.600 : v < 1250.01 ? 4.200 : v < 1600.01 ? 5.000 : v < 2000.01 ? 6.000 : v < 2500.01 ? 7.000 : 8.600;
        }

        private double TolJs9(double v)
        {
            return v < 3.01 ? 0.012 : v < 6.01 ? 0.015 : v < 10.01 ? 0.018 : v < 18.01 ? 0.021 : v < 30.01 ? 0.026 : v < 50.01 ? 0.031 : v < 80.01 ? 0.037 : v < 120.01 ? 0.043 : v < 180.01 ? 0.050 : v < 250.01 ? 0.057 : v < 315.01 ? 0.065 : v < 400.01 ? 0.070 : v < 500.01 ? 0.077 : v < 630.01 ? 0.087 : v < 800.01 ? 0.100 : v < 1000.01 ? 0.115 : v < 1250.01 ? 0.130 : v < 1600.01 ? 0.155 : v < 2000.01 ? 0.185 : v < 2500.01 ? 0.220 : 0.270;
        }

        private double GtjTol(double kona, double d, bool positive)
        {
            if (Math.Abs(kona - 12) < 0.000001)
                return positive ? (d > 1000 ? 0.095 : d > 800 ? 0.085 : d > 630 ? 0.075 : d > 500 ? 0.070 : d > 400 ? 0.065 : d > 315 ? 0.060 : d > 250 ? 0.055 : d > 180 ? 0.050 : d > 120 ? 0.040 : d > 80 ? 0.035 : d > 50 ? 0.030 : d > 30 ? 0.025 : 0.020)
                                : (d > 1000 ? 0.280 : d > 800 ? 0.250 : d > 630 ? 0.225 : d > 500 ? 0.200 : d > 400 ? 0.190 : d > 315 ? 0.175 : d > 250 ? 0.160 : d > 180 ? 0.140 : d > 120 ? 0.120 : d > 80 ? 0.105 : d > 50 ? 0.090 : d > 30 ? 0.075 : 0.070);
            if (Math.Abs(kona - 30) < 0.000001)
                return positive ? (d > 1000 ? 0.060 : d > 800 ? 0.055 : d > 630 ? 0.050 : d > 500 ? 0.045 : d > 400 ? 0.040 : d > 315 ? 0.035 : d > 250 ? 0.035 : d > 180 ? 0.030 : d > 120 ? 0.025 : d > 80 ? 0.022 : d > 50 ? 0.019 : d > 30 ? 0.016 : 0.013)
                                : (d > 1000 ? 0.170 : d > 800 ? 0.155 : d > 630 ? 0.140 : d > 500 ? 0.125 : d > 400 ? 0.115 : d > 315 ? 0.105 : d > 251 ? 0.095 : d > 180 ? 0.085 : d > 120 ? 0.075 : d > 80 ? 0.065 : d > 50 ? 0.055 : d > 30 ? 0.046 : 0.039);
            return 0;
        }

        private string DrawingBySerie(Context ctx)
        {
            return ctx.Serie == 30 ? "7438957" : ctx.Serie == 31 ? "7438958" : ctx.Serie == 32 ? "7438955" : ctx.Serie == 39 ? "7434032" : ctx.Serie == 240 ? "7432901" : ctx.Subject;
        }

        private double CalculateDefaultInnerDiameterForHb(bool hasSlash, double typ, string bet3, string bet4, string bet5)
        {
            var d4 = DiameterFromSubjectForHb(hasSlash, typ, bet3, bet4, bet5);
            return InnerDiameterFromD4AndTyp(d4, typ);
        }

        private double DiameterFromSubjectForHb(bool hasSlash, double typ, string bet3, string bet4, string bet5)
        {
            return !hasSlash
                ? typ / 2.0 * 10.0
                : IsHbToken(bet5)
                    ? ToDouble(bet4)
                    : ToDouble(bet3);
        }

        private double InnerDiameterFromD4AndTyp(double d4, double typ)
        {
            if (Math.Abs(d4) < 0.000001) return 0;
            if (typ < 85) return d4 - 20;
            if (typ < 561 || Math.Abs(typ - 630.0) < 0.000001) return d4 - 30;
            if (typ < 751) return d4 - 40;
            if (typ < 1001) return d4 - 50;
            return d4 - 60;
        }

        private bool IsHbToken(string value)
        {
            return string.Equals(value, "HB", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(value, "HBE", StringComparison.OrdinalIgnoreCase);
        }

        private string GetToken(List<string> tokens, int index) => index >= 0 && index < tokens.Count ? tokens[index] : "";
        private string Left(string value, int length) => string.IsNullOrEmpty(value) ? "" : value.Substring(0, Math.Min(length, value.Length));
        private string Right(string value, int length) => string.IsNullOrEmpty(value) ? "" : value.Substring(Math.Max(0, value.Length - length));

        private int ToInt(string value)
        {
            return int.TryParse(OnlyNumberText(value), NumberStyles.Any, CultureInfo.InvariantCulture, out var v) ? v : 0;
        }

        private double ToDouble(string value)
        {
            return double.TryParse(OnlyNumberText(value), NumberStyles.Any, CultureInfo.InvariantCulture, out var v) ? v : 0;
        }

        private string OnlyNumberText(string value)
        {
            return (value ?? "").Trim().Replace(',', '.');
        }

        private string Fmt3(double value) => value.ToString("0.000", CultureInfo.InvariantCulture);
        private string FmtPlain(double value) => Math.Abs(value - Math.Round(value)) < 0.000001 ? Math.Round(value).ToString("0", CultureInfo.InvariantCulture) : value.ToString("0.###", CultureInfo.InvariantCulture);
        private string FmtPlainComma(double value) => FmtPlain(value).Replace('.', ',');

        private string FirstNonEmpty(params string[] values)
        {
            return values.FirstOrDefault(v => !string.IsNullOrWhiteSpace(v)) ?? "";
        }

        private string GetString(object source, string name)
        {
            var value = GetValue(source, name);
            return value == null ? "" : Convert.ToString(value, CultureInfo.InvariantCulture) ?? "";
        }

        private bool TryGetDouble(object source, string name, out double value)
        {
            var raw = GetValue(source, name);
            if (raw == null)
            {
                value = 0;
                return false;
            }

            if (raw is double d)
            {
                value = d;
                return true;
            }

            if (raw is decimal dec)
            {
                value = (double)dec;
                return true;
            }

            if (raw is int i)
            {
                value = i;
                return true;
            }

            if (raw is long l)
            {
                value = l;
                return true;
            }

            if (raw is float f)
            {
                value = f;
                return true;
            }

            var text = Convert.ToString(raw, CultureInfo.InvariantCulture)?.Replace(',', '.') ?? "";
            if (double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsed))
            {
                value = parsed;
                return true;
            }

            var matches = System.Text.RegularExpressions.Regex.Matches(text, @"[-+]?\d+(?:\.\d+)?");
            if (matches.Count > 0)
            {
                var candidate = text.Contains(":") ? matches[matches.Count - 1].Value : matches[0].Value;
                if (double.TryParse(candidate, NumberStyles.Any, CultureInfo.InvariantCulture, out parsed))
                {
                    value = parsed;
                    return true;
                }
            }

            value = 0;
            return false;
        }

        private double GetDouble(object source, string name, double defaultValue)
        {
            return TryGetDouble(source, name, out var value) ? value : defaultValue;
        }

        private object GetValue(object source, string name)
        {
            var value = GetValueRecursive(source, name, new HashSet<object>());
            if (value != null) return value;

            foreach (var alias in FieldAliases(name))
            {
                value = GetValueRecursive(source, alias, new HashSet<object>());
                if (value != null) return value;
            }

            return null;
        }

        private IEnumerable<string> FieldAliases(string name)
        {
            var n = NormalizeLookupName(name);
            if (n == NormalizeLookupName("Avvikande YDia Gänga (d)")) return new[] { "Avvikande YDia Gänga (d)", "Avvikande YDia Ganga (d)", "YDIA", "YDia", "d", "Tmpd" };
            if (n == NormalizeLookupName("Innerdiameter (d1)")) return new[] { "Innerdiameter (d1)", "d1", "Innerdiameter", "Inner diameter", "InnerDia", "InnerDiameter", "Tmpd1", "Sumd1", "Sumd10" };
            if (n == NormalizeLookupName("Kona storände diameter (d2)")) return new[] { "Kona storände diameter (d2)", "d2", "D2", "Tmpd2", "Tmpd2a", "Kona storände diameter", "Kona storande diameter", "Kona storande diameter d2", "KonaStorandeDiameter", "KonaStoraendeDiameter", "Storände diameter", "Storande diameter", "StorandeDiameter", "Large end diameter", "LargeEndDiameter", "Big end diameter", "BigEndDiameter", "Sumd2" };
            if (n == NormalizeLookupName("Gänglängd (b)")) return new[] { "Gänglängd (b)", "b", "Gänglängd", "Ganglangd", "Gäng längd", "Gang langd", "Thread length", "ThreadLength", "Tmpb", "Sumb" };
            if (n == NormalizeLookupName("Längd (L)")) return new[] { "Längd (L)", "L", "Längd", "Langd", "Length", "Total length", "TotalLength", "TmpL", "SumL", "SumLOP1" };
            if (n == NormalizeLookupName("Kona")) return new[] { "Kona", "Taper", "TmpKona" };
            if (n == NormalizeLookupName("Gäng typ")) return new[] { "Gäng typ", "Gang typ", "Gänga", "Ganga", "Thread type", "ThreadType", "G", "TmpG1", "SumG1" };
            if (n == NormalizeLookupName("Produktritning")) return new[] { "Produktritning", "Ritningsnummer", "Ritning", "Product drawing", "ProductDrawing", "DrawingNumber" };
            if (n == NormalizeLookupName("a-mått")) return new[] { "a-mått", "a mått", "amått", "amatt", "aMatt", "a", "A measure", "AMeasure" };
            if (n == NormalizeLookupName("Radie Lillände")) return new[] { "Radie Lillände", "Radie Lillande", "Radius small end", "Small end radius", "RadiusSmall" };
            if (n == NormalizeLookupName("Radie Storände")) return new[] { "Radie Storände", "Radie Storande", "Radius big end", "Large end radius", "Big end radius", "RadiusBig" };
            if (n == NormalizeLookupName("MaskinVal")) return new[] { "MaskinVal", "Maskin", "Machine", "MachineNumber" };
            return Array.Empty<string>();
        }

        private object GetValueRecursive(object source, string name, HashSet<object> visited)
        {
            if (source == null || string.IsNullOrWhiteSpace(name)) return null;
            if (source is string || IsSimpleType(source.GetType())) return null;
            if (!visited.Add(source)) return null;

            var wanted = NormalizeLookupName(name);

            if (source is IDictionary dictionary)
            {
                foreach (DictionaryEntry entry in dictionary)
                {
                    var key = Convert.ToString(entry.Key, CultureInfo.InvariantCulture) ?? "";
                    if (NormalizeLookupName(key) == wanted) return entry.Value;
                }

                foreach (DictionaryEntry entry in dictionary)
                {
                    var nested = GetValueRecursive(entry.Value, name, visited);
                    if (nested != null) return nested;
                }

                return null;
            }

            if (source is IEnumerable enumerable && !(source is string))
            {
                foreach (var item in enumerable)
                {
                    var nested = GetValueRecursive(item, name, visited);
                    if (nested != null) return nested;
                }

                return null;
            }

            var type = source.GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.GetIndexParameters().Length == 0)
                .ToArray();

            var directProperty = properties.FirstOrDefault(p => NormalizeLookupName(p.Name) == wanted);
            if (directProperty != null) return directProperty.GetValue(source);

            foreach (var nameProperty in properties.Where(p => IsNameCarrierProperty(p.Name)))
            {
                var candidateName = Convert.ToString(nameProperty.GetValue(source), CultureInfo.InvariantCulture) ?? "";
                if (NormalizeLookupName(candidateName) != wanted) continue;

                var carrierValue = GetBestValueFromCarrier(source, properties);
                if (carrierValue != null) return carrierValue;
            }

            foreach (var property in properties.Where(p => IsLikelyContainerProperty(p.Name)))
            {
                var nested = GetValueRecursive(property.GetValue(source), name, visited);
                if (nested != null) return nested;
            }

            foreach (var property in properties.Where(p => !IsLikelyContainerProperty(p.Name)))
            {
                if (property.PropertyType == typeof(string) || IsSimpleType(property.PropertyType)) continue;

                var nested = GetValueRecursive(property.GetValue(source), name, visited);
                if (nested != null) return nested;
            }

            return null;
        }

        private object GetBestValueFromCarrier(object source, PropertyInfo[] properties)
        {
            var priority = new[]
            {
                "VALUE",
                "BOOKMARKVALUE",
                "FIELDVALUE",
                "PARAMETERVALUE",
                "DECIMALVALUE",
                "DOUBLEVALUE",
                "NUMERICVALUE",
                "NUMBERVALUE",
                "INTVALUE",
                "RESULT",
                "RESULTVALUE",
                "CALCULATEDVALUE",
                "SELECTEDVALUE",
                "DEFAULTVALUE",
                "TEXT",
                "STRINGVALUE",
                "DISPLAYVALUE"
            };

            foreach (var wanted in priority)
            {
                var prop = properties.FirstOrDefault(p => NormalizeLookupName(p.Name) == wanted);
                if (prop == null) continue;

                var value = prop.GetValue(source);
                if (value == null) continue;
                if (value is string s && string.IsNullOrWhiteSpace(s)) continue;
                return value;
            }

            return null;
        }

        private bool IsNameCarrierProperty(string propertyName)
        {
            var n = NormalizeLookupName(propertyName);
            return n == "NAME" || n == "KEY" || n == "FIELDNAME" || n == "PARAMETERNAME" || n == "DISPLAYNAME" || n == "LABEL" || n == "CAPTION" || n == "TITLE" || n == "BOOKMARK" || n == "BOOKMARKNAME" || n == "PROMPT";
        }

        private bool IsValueCarrierProperty(string propertyName)
        {
            var n = NormalizeLookupName(propertyName);
            return n == "VALUE" || n == "BOOKMARKVALUE" || n == "FIELDVALUE" || n == "PARAMETERVALUE" || n == "TEXT" || n == "NUMBER" || n == "DECIMALVALUE" || n == "DOUBLEVALUE" || n == "INTVALUE" || n == "NUMERICVALUE" || n == "NUMBERVALUE" || n == "RESULT" || n == "RESULTVALUE" || n == "CALCULATEDVALUE" || n == "SELECTEDVALUE" || n == "DEFAULTVALUE" || n == "STRINGVALUE" || n == "DISPLAYVALUE";
        }

        private bool IsLikelyContainerProperty(string propertyName)
        {
            var n = NormalizeLookupName(propertyName);
            return n.Contains("BOOKMARK") || n.Contains("PARAMETER") || n.Contains("CUSTOM") || n.Contains("FIELD") || n.Contains("INPUT") || n.Contains("PROPERTY") || n.Contains("ATTRIBUTE") || n.Contains("DATA") || n.Contains("DICTIONARY") || n.Contains("VALUE");
        }

        private string NormalizeLookupName(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return "";

            var decoded = WebUtility.HtmlDecode(value).Trim();
            var normalized = decoded.Normalize(NormalizationForm.FormD);
            var chars = normalized
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .Where(char.IsLetterOrDigit)
                .Select(char.ToUpperInvariant)
                .ToArray();

            return new string(chars);
        }

        private bool IsSimpleType(Type type)
        {
            var t = Nullable.GetUnderlyingType(type) ?? type;
            return t.IsPrimitive || t.IsEnum || t == typeof(decimal) || t == typeof(DateTime) || t == typeof(Guid);
        }

        private void Merge(Dictionary<string, string> target, Dictionary<string, string> source)
        {
            foreach (var kv in source)
                target[kv.Key] = kv.Value ?? "";
        }

        private class Context
        {
            public string Subject { get; set; }
            public List<string> Tokens { get; set; }
            public bool HasSlash { get; set; }
            public bool HasV32 { get; set; }
            public string Bet1 { get; set; }
            public string Bet2 { get; set; }
            public string Bet3 { get; set; }
            public string Bet4 { get; set; }
            public string Bet5 { get; set; }
            public int Serie { get; set; }
            public double Typ { get; set; }
            public double D { get; set; }
            public double D1 { get; set; }
            public double D2Input { get; set; }
            public bool D2InputProvided { get; set; }
            public double B { get; set; }
            public double L { get; set; }
            public double Kona { get; set; }
            public double AMatt { get; set; }
            public string Machine { get; set; }
            public string ProductDrawing { get; set; }
            public string ThreadType { get; set; }
            public string RadiusSmall { get; set; }
            public string RadiusBig { get; set; }
            public bool IsHb { get; set; }
            public List<string> MissingInputs { get; set; }
        }
    }
}
