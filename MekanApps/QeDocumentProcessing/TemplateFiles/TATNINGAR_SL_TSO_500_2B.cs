using DocumentFormat.OpenXml.Office2016.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class TATNINGAR_SL_TSO_500_2B : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var c = Build(request);
            Merge(result, GetDimensions(c));
            Merge(result, GetWidths(c));
            Merge(result, GetAnglesAndDrawing(c));
            Merge(result, GetMachineAndRows(request, c));
            Merge(result, GetTexts(c));
            return result;
        }

        private CalcData Build(APIRequest request)
        {
            var subject = request?.ProductDesignation ?? string.Empty;
            var tmpFormat = subject.Trim().ToUpperInvariant();
            var tmpBet = tmpFormat.Replace('.', ',');
            var parts = Regex.Split(tmpBet, @"[\s/,\.\-]+")
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            string Word(int index) => parts.Count >= index ? parts[index - 1] : string.Empty;
            decimal Number(string value)
            {
                if (decimal.TryParse(value.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var number))
                    return number;
                return 0m;
            }

            var tmpBet1 = Word(1);
            var tmpBet2 = Word(2);
            var tmpBet3 = Word(3);
            var tmpBet4 = Word(4);
            var tmpBet5 = Word(5);
            var tmpBet6 = Word(6);
            var tmpBet7 = Word(7);
            var serieText = tmpBet3.Length > 0 ? tmpBet3.Substring(0, 1) : string.Empty;
            var typText = tmpBet3.Length >= 2 ? tmpBet3.Substring(tmpBet3.Length - 2, 2) : tmpBet3;
            var serie = (int)Number(serieText);
            var typ = (int)Number(typText);
            var bet4 = Number(tmpBet4);
            var bet5 = Number(tmpBet5);
            var bet6 = Number(tmpBet6);
            var bet7 = Number(tmpBet7);
            var hasSlash = tmpBet.Contains("/");
            var hasV21 = tmpBet.Contains("V21");
            var hasV22 = tmpBet.Contains("V22");
            var hasV212 = tmpBet.Contains("V21-2");
            var hasV213 = tmpBet.Contains("V21-3");
            var hasV = hasV212 || hasV213 || hasV22 || hasV21;
            var tum = hasSlash && !hasV;
            var typIndex = GetTypIndex(typ);
            var tmpTumMm = tum && bet6 != 0 ? Round2((bet4 * 25.4m) + ((bet5 / bet6) * 25.4m)) : 0m;

            return new CalcData
            {
                ProductDesignation = subject,
                TmpFormat = tmpFormat,
                TmpBet = tmpBet,
                Serie = serie,
                Typ = typ,
                Bet4 = bet4,
                Bet5 = bet5,
                Bet6 = bet6,
                Bet7 = bet7,
                HasSlash = hasSlash,
                HasV21 = hasV21,
                HasV = hasV,
                Tum = tum,
                TypIndex = typIndex,
                TmpTumMm = tmpTumMm,
                Machine = request?.MachineNumber ?? string.Empty
            };
        }

        private Dictionary<string, string> GetDimensions(CalcData c)
        {
            var listaD13 = GetListaD13(c);
            var listaD46 = GetListaD46(c);
            var tmpd = c.Tum ? c.TmpTumMm : c.Typ < 25 ? ((c.Typ * 10m) / 2m) - 10m : c.Typ < 31 ? ((c.Typ * 10m) / 2m) - 15m : ((c.Typ * 10m) / 2m) - 20m;
            var tmpD1 = Word(listaD13, 1);
            var tmpD2 = Word(listaD13, 2);
            var tmpD3 = Word(listaD13, 3);
            var tmpD4 = Word(listaD46, 1);
            var tmpD5 = Word(listaD46, 2);
            var tmpD6 = Word(listaD46, 3);
            var n = c.Serie == 5 ? c.Typ < 33 ? 2m : c.Typ == 34 ? 4m : 3m : c.Typ < 33 ? 2m : c.Typ < 35 ? 4m : 5m;
            var tmpD7 = n * 2m;
            var tmpD7a = tmpd + tmpD7;

            return new Dictionary<string, string>
            {
                ["Sumd"] = $"(d) {Fmt(tmpd)}",
                ["Sumda"] = Fmt(tmpd),
                ["SumdTol"] = SumdTol(c, tmpd),
                ["SumdTolN"] = SumdTolN(c, tmpd),
                ["SumD1"] = c.Serie == 2 ? "-" : $"(D1) {tmpD1}",
                ["SumD1Tol"] = "+ 0.5",
                ["SumD1TolN"] = "- 0",
                ["SumD2"] = tmpD2 == "x" ? "-" : $"(D2) {tmpD2}",
                ["SumD2Tol"] = tmpD2 == "x" ? "-" : Number(tmpD2) < 120m ? "± 0.3" : "± 0.5",
                ["SumD3"] = c.Bet4 == 2m && c.Bet7 != 1m ? "-" : tmpD3 == "x" ? "" : $"(D3) {tmpD3}",
                ["SumD3Tol"] = c.Bet4 == 2m && c.Bet7 != 1m ? "-" : tmpD3 == "x" ? "" : Number(tmpD3) < 120m ? "± 0.3" : "± 0.5",
                ["SumD4"] = $"(D4) {tmpD4}",
                ["SumD4Tol"] = Number(tmpD4) < 120m ? "± 0.3" : "± 0.5",
                ["SumD5"] = $"(D5) {tmpD5}",
                ["SumD5a"] = tmpD5,
                ["SumD5Tol"] = "+ 0",
                ["SumD5TolN"] = TolD5N(Number(tmpD5)),
                ["SumD6"] = $"(D6) {tmpD6}",
                ["SumD6Tol"] = TolD6(Number(tmpD6)),
                ["SumD6TolN"] = "- 0",
                ["SumD7"] = $"(D7) {Fmt(tmpD7a)}",
                ["SumD7Tol"] = tmpD7 < 3.1m ? "± 0.2" : tmpD7 < 6.1m ? "± 0.5" : "± 1.0"
            };
        }

        private Dictionary<string, string> GetWidths(CalcData c)
        {
            var aList = AnyEq1(c) || c.Serie == 2 ? "72:75:80:88:109:103:106:103,5:118:132:137:139:140:148:150" : "52,5:55,5:58,5:66:87:80:82:77,5:90:103,5:107:108:108:113:113";
            var bList = AnyEq1(c) ? "52:55:56:60:75:71:74:73,5:84:91,5:96,5:98,5:99,5:108:110" : c.Serie == 2 && c.Bet4 == 2m ? "54:57:60:66:80:74:81:77:89:95:100:102:110:111:117" : "x:x:x:x:80:74:x:x:x:95:98,5:99,5:x:x:x";
            var cList = AnyEq1(c) ? "49:52:53:57:72:67:70:68,5:80:87:92:94:95:101:103" : "49:52:54:60:72:67:70:68,5:81:87:92:94:96:101:104";
            var eList = c.Serie == 5 && (c.Bet7 == 2m || c.Bet4 == 2m) ? "36:39:37:37,5:47,5:44:47:46,5:55:58:63:65:65,5:71,5:74" : "36:39:38,5:38,5:47,5:44:47:46,5:55,5:58:63:65:66:71,5:74";
            var fList = AnyEq1(c) ? "12:10:8:8:11:8:11:16:15:15:23:25:25:25:20" : "3:4:4:6:5:5:5:5:5:8:10:10:7:5:8";
            var kList = "20:22:21:22:26:22:24:24:34:36:36,5:37:40:44:45";
            var tmpa = Word(aList, c.TypIndex);
            var tmpb = Word(bList, c.TypIndex);
            var tmpc = Word(cList, c.TypIndex);
            var tmpe = Word(eList, c.TypIndex);
            var tmpf = c.Serie == 2 ? "x" : Word(fList, c.TypIndex);
            var tmpg = c.Serie == 2 ? "" : c.Typ < 27 ? "M6" : c.Typ < 33 ? "M8" : "M10";
            var tmph = c.Serie == 2 ? "x" : c.Typ < 33 ? "8" : "12";
            var tmpk = Word(kList, c.TypIndex);
            var tmpm = c.Typ < 33 ? 3m : c.Typ == 34 ? 4m : 5m;
            var tmpn = c.Serie == 5 ? c.Typ < 33 ? 2m : c.Typ == 34 ? 4m : 3m : c.Typ < 33 ? 2m : c.Typ < 35 ? 4m : 5m;
            var tmpp = AnyEq1(c) ? c.Typ < 33 ? "10,5" : "13" : "x";
            var tmpr = AnyEq1(c) ? c.Typ < 33 ? "6" : "15" : "x";

            return new Dictionary<string, string>
            {
                ["Suma"] = $"(a) {tmpa}",
                ["SumaTol"] = "+ 0",
                ["SumaTolN"] = "- 0.1",
                ["Sumb"] = tmpb == "x" ? "" : $"(b) {tmpb}",
                ["SumbTol"] = tmpb == "x" ? "" : Number(tmpa) < 120m ? "± 0.3" : "± 0.5",
                ["Sumc"] = $"(c) {tmpc}",
                ["SumcTol"] = "+ 0.2",
                ["SumcTolN"] = "- 0",
                ["Sume"] = $"(e) {tmpe}",
                ["SumeTol"] = "+ 0.2",
                ["SumeTolN"] = "- 0",
                ["Sumf"] = tmpf == "x" ? "" : $"(f) {tmpf}",
                ["SumfTol"] = tmpf == "x" ? "" : TolByValue(Number(tmpf)),
                ["Sumg"] = $"(g) {tmpg}",
                ["Sumg1"] = tmpg,
                ["Sumh"] = tmph == "x" ? "" : $"(h) {tmph}",
                ["SumhTol"] = tmph == "x" ? "" : TolByValue(Number(tmph)),
                ["Sumk"] = $"(k) {tmpk}",
                ["SumkTol"] = TolByValue(Number(tmpk)),
                ["Summ"] = $"(m) {Fmt(tmpm)}",
                ["SummTol"] = TolByValue(tmpm),
                ["Sumn"] = $"{Fmt(tmpn)}x45º",
                ["Sump"] = tmpp == "x" ? "" : $"(p) {tmpp}",
                ["SumpTol"] = tmpp == "x" ? "" : "+ 0.2",
                ["SumpTolN"] = tmpp == "x" ? "" : "- 0",
                ["Sumr"] = tmpr == "x" ? "" : $"(r) {tmpr}",
                ["SumrTol"] = tmpr == "x" ? "" : TolByValue(Number(tmpr))
            };
        }

        private Dictionary<string, string> GetAnglesAndDrawing(CalcData c)
        {
            var drawing = c.HasSlash ? c.Bet7 == 1m ? "7439269" : "7439270" : c.Bet4 == 1m ? "7438712" : "7438713";
            return new Dictionary<string, string>
            {
                ["Sum60"] = "60º",
                ["Sum60a"] = "60º",
                ["Sum8"] = "8º ± 1º",
                ["SumRa63"] = "6.3",
                ["SumRa32"] = "3.2",
                ["SumRa32a"] = "3.2",
                ["SumRa32b"] = "3.2",
                ["SumRa32c"] = "3.2",
                ["SumRa32d"] = "3.2",
                ["SumRa32e"] = "3.2",
                ["SumRit"] = drawing,
                ["SumStämpel"] = $"Stämplas: {c.TmpFormat}"
            };
        }

        private Dictionary<string, string> GetMachineAndRows(APIRequest request, CalcData c)
        {
            var isMachine = IsMachine(c.Machine);
            var machineName = c.Machine == "Nakamura" ? "Nakamura" : c.Machine == "LT-3000" ? "LT-3000" : string.Empty;
            var result = new Dictionary<string, string>
            {
                ["SumMaskinValS1"] = $"Maskin: {machineName}"
            };

            var f = new[] { "1/3", "1/3", "1/2", "1/3", "1/1", "1/2", "1/2", "1/3", "1/1", "1/3", "1/3", "1/3", "1/3" };
            var d = new[] { "Digitalt Skjutmått", "Digitalt Skjutmått", "Digitalt Skjutmått", "Digitalt Skjutmått", "UD-Apparat", "Djupmått ev. med klocka", "Digitalt Skjutmått", "Digitalt Skjutmått", "Okulärkontroll", "Ytjämnhetsmätare", "Digitalt Skjutmått", "Digitalt Skjutmått", GetWidths(c)["Sumg1"] + " Tolk" };
            var af = new[] { "", "", "", "", $"Klove/Ring {GetDimensions(c)["Sumda"]}, mät båda sidor", "Passbitar", "", "", "60º", "", "", "", "120° delning" };

            for (var i = 1; i <= 13; i++)
            {
                var suffix = i == 10 ? "0" : i.ToString(CultureInfo.InvariantCulture);
                var serie5Only = i == 12 || i == 13;
                result[$"SumF1_{suffix}"] = isMachine && (!serie5Only || c.Serie == 5) ? f[i - 1] : string.Empty;
                result[$"SumD1_{suffix}"] = isMachine && (!serie5Only || c.Serie == 5) ? d[i - 1] : string.Empty;
                result[$"SumAF1_{suffix}"] = isMachine && (!serie5Only || c.Serie == 5) ? af[i - 1] : string.Empty;
            }

            return result;
        }

        private Dictionary<string, string> GetTexts(CalcData c)
        {
            return new Dictionary<string, string>
            {
                ["SumTextS1"] = $"Okulärkontroll: Grader, frifläcker, slagmärken, repor, valkar, ytjämnhet och faser <<LineBreak>> Bearbetas Ra 6.3 där annat ej anges, skarpa kanter avgradas."
            };
        }

        private string GetListaD13(CalcData c)
        {
            if (c.HasV21)
                return AnyEq1(c) && c.Serie == 5 && c.Typ == 40 ? "214:234:246" : string.Empty;

            if (AnyEq1(c) || c.Serie == 2)
            {
                return c.Typ switch
                {
                    17 => "90:98:108",
                    18 => "95:103:113",
                    20 => "107:115:125",
                    22 => "118:126:136",
                    24 => "128:139:149",
                    26 => "138:149:159",
                    28 => "150:160:170",
                    30 => "160:175:187",
                    32 => "170:187:199",
                    34 => "182:200:212",
                    36 => "192:209:221",
                    38 => "202:220:232",
                    40 => "214:234:246",
                    44 => "233:255:267",
                    48 => "255:280:292",
                    _ => "0:0:0"
                };
            }

            if (c.Bet4 == 2m || c.Bet7 == 2m)
            {
                return c.Typ switch
                {
                    17 => "88:x:120",
                    18 => "92:x:128",
                    20 => "102:x:142",
                    22 => "112:x:152",
                    24 => "125:139:168",
                    26 => "135:149:175",
                    28 => "145:x:200",
                    30 => "155:x:210",
                    32 => "165:x:225",
                    34 => "175:200:234",
                    36 => "185:209:250",
                    38 => "195:220:270",
                    40 => "205:x:280",
                    44 => "224:x:305",
                    48 => "250:x:325",
                    _ => "0:0:0"
                };
            }

            return string.Empty;
        }

        private string GetListaD46(CalcData c)
        {
            if (c.HasV21)
                return AnyEq1(c) && c.Serie == 5 && c.Typ == 40 ? "297:244:274" : string.Empty;

            return c.Typ switch
            {
                17 => "120:96:110",
                18 => "128:100:114",
                20 => "142:112:127",
                22 => "152:120:137",
                24 => "168:135:154",
                26 => "175:145:162",
                28 => "200:160:182",
                30 => "210:170:190",
                32 => "225:178:202",
                34 => "234:195:216",
                36 => "250:205:230",
                38 => "270:220:247",
                40 => "280:227:257",
                44 => "305:255:285",
                48 => "325:270:300",
                _ => "0:0:0"
            };
        }

        private static int GetTypIndex(int typ)
        {
            var values = new[] { 17, 18, 20, 22, 24, 26, 28, 30, 32, 34, 36, 38, 40, 44, 48 };
            var index = Array.IndexOf(values, typ);
            return index < 0 ? 1 : index + 1;
        }

        private static bool AnyEq1(CalcData c) => c.Bet4 == 1m || c.Bet7 == 1m;
        private static bool IsMachine(string machine) => machine == "Nakamura" || machine == "LT-3000";
        private static decimal Round2(decimal value) => Math.Round(value, 2, MidpointRounding.AwayFromZero);
        private static string Word(string list, int index) => (list ?? string.Empty).Split(':').ElementAtOrDefault(index - 1) ?? string.Empty;
        private static decimal Number(string value) => decimal.TryParse((value ?? string.Empty).Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var number) ? number : 0m;
        private static string Fmt(decimal value) => value.ToString("0.##", CultureInfo.InvariantCulture).Replace('.', ',');
        private static string SumdTol(CalcData c, decimal d) => c.Bet4 == 1m && c.Serie == 2 ? d < 80.1m ? "+ 0.060" : d < 120.1m ? "+ 0.071" : d < 180.1m ? "+ 0.083" : d < 250.1m ? "+ 0.096" : d < 315.1m ? "+ 0.108" : d < 400.1m ? "+ 0.119" : "+ 0.131" : d < 50.1m ? "+ 0.025" : d < 80.1m ? "+ 0.030" : d < 120.1m ? "+ 0.035" : d < 180.1m ? "+ 0.040" : d < 250.1m ? "+ 0.046" : d < 315.1m ? "+ 0.052" : d < 400.1m ? "+ 0.057" : "+ 0.063";
        private static string SumdTolN(CalcData c, decimal d) => c.Bet4 == 1m && c.Serie == 2 ? d < 80.1m ? "+ 0.030" : d < 120.1m ? "+ 0.036" : d < 180.1m ? "+ 0.043" : d < 250.1m ? "+ 0.050" : d < 315.1m ? "+ 0.056" : d < 400.1m ? "+ 0.062" : d < 500.1m ? "+ 0.068" : "+ 0.076" : "+ 0";
        private static string TolD5N(decimal d) => d < 80.1m ? "- 0.190" : d < 120.1m ? "- 0.220" : d < 180.1m ? "- 0.250" : d < 250.1m ? "- 0.290" : d < 315.1m ? "- 0.320" : "- 0.360";
        private static string TolD6(decimal d) => d < 120.1m ? "+ 0.220" : d < 180.1m ? "+ 0.250" : d < 250.1m ? "+ 0.290" : d < 315.1m ? "+ 0.320" : "+ 0.360";
        private static string TolByValue(decimal value) => value < 6.1m ? "± 0.1" : value < 30.1m ? "± 0.2" : "± 0.3";

        private void Merge(Dictionary<string, string> target, Dictionary<string, string> source)
        {
            foreach (var kv in source)
                target[kv.Key] = kv.Value ?? string.Empty;
        }

        private class CalcData
        {
            public string ProductDesignation { get; set; }
            public string TmpFormat { get; set; }
            public string TmpBet { get; set; }
            public int Serie { get; set; }
            public int Typ { get; set; }
            public decimal Bet4 { get; set; }
            public decimal Bet5 { get; set; }
            public decimal Bet6 { get; set; }
            public decimal Bet7 { get; set; }
            public bool HasSlash { get; set; }
            public bool HasV21 { get; set; }
            public bool HasV { get; set; }
            public bool Tum { get; set; }
            public int TypIndex { get; set; }
            public decimal TmpTumMm { get; set; }
            public string Machine { get; set; }
        }
    }
}
