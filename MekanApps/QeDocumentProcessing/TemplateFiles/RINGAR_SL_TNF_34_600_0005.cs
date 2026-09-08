using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class RINGAR_SL_TNF_34_600_0005 : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();
            string designation = req?.ProductDesignation ?? "";
            string templateType = req?.TemplateType ?? "";
            string machine = req?.MachineNumber ?? "";
            string normalized = designation.Trim().ToUpperInvariant().Replace(".", ",");
            bool slash = normalized.Contains("/");
            bool v21 = normalized.Contains("V21");
            string[] parts = normalized.Split(new[] { ' ', '/', '-' }, StringSplitOptions.RemoveEmptyEntries);
            double type = GetTypeValue(normalized, templateType, parts, slash);
            double variant = GetVariant(parts, slash);
            double[] d = GetDimensions(type, variant, slash);
            double d1 = d[0], d2 = d[1], d3 = d[2], d4 = d[3];
            double d2Tol = d2 < 120 ? .090 : d2 < 182 ? .106 : d2 < 245 ? .122 : d2 < 315 ? .137 : d2 < 405 ? .151 : d2 < 510 ? .165 : .186;
            double d2TolN = d2 < 120 ? .036 : d2 < 182 ? .043 : d2 < 245 ? .050 : d2 < 315 ? .056 : d2 < 405 ? .062 : d2 < 510 ? .068 : .076;
            bool nakamura = Eq(machine, "Nakamura");
            kv["SumD1"] = "(D1) " + N(d1);
            kv["SumD1Tol"] = d1 < 400 ? "± 0.5" : "± 0.8";
            kv["SumD2"] = "(D2) " + N(nakamura ? d2 : d2 + d2TolN);
            kv["SumD2Tol"] = "+ " + F3(nakamura ? d2Tol : d2Tol - d2TolN);
            kv["SumD2TolN"] = nakamura ? "+ " + F3(d2TolN) : "- 0";
            kv["SumD3"] = "(D3) " + N(d3);
            kv["SumD3Tol"] = d3 < 180 ? "+ 0.25" : d3 < 255 ? "+ 0.29" : d3 < 315 ? "+ 0.32" : d3 < 395 ? "+ 0.36" : d3 < 500 ? "+ 0.40" : "+ 0.44";
            kv["SumD3TolN"] = "  0";
            kv["SumD4"] = "(D4) " + N(d4);
            kv["SumD4Tol"] = "  0";
            kv["SumD4TolN"] = d4 < 170 ? "- 0.25" : d4 < 270 ? "- 0.29" : d4 < 305 ? "- 0.32" : d4 < 405 ? "- 0.36" : d4 < 500 ? "- 0.40" : "- 0.44";
            double chord = Math.Round((d1 / 2d) * Math.Round(Math.Sin(60d * Math.PI / 180d) * 2d, 4), 1);
            kv["SumF"] = "M6 (2x) 120°  Korda mellan gänghål: " + N(chord);
            bool small = slash && (Near(variant, 5.15) || Near(variant, 6.15));
            double a = type < 39 || small ? 24 : type < 45 ? 22 : type < 55 ? 26.5 : 25;
            double b = type < 39 || small ? 15 : type < 45 ? 13 : type < 55 ? 17.5 : 16;
            double c = type < 39 || small ? 12 : type < 45 ? 10.5 : type < 55 ? 14 : 12.5;
            kv["SumA"] = "(A) " + N(a); kv["SumATol"] = "± 0.2";
            kv["SumB"] = "(B) " + N(b); kv["SumBTol"] = "± 0.2"; kv["SumbTol"] = "± 0.2";
            kv["SumC"] = "(C) " + N(c); kv["SumCTol"] = " 0"; kv["SumCTolN"] = "- 0.2";
            kv["SumE"] = "(E) 5"; kv["SumETol"] = "± 0.1";
            kv["SumG"] = "1x45º"; kv["SumG1"] = "1x45º"; kv["SumG2"] = "1x45º";
            kv["SumR"] = "Max R 0.8"; kv["SumRa1"] = "3.2";
            bool enabled = Eq(machine, "Nakamura") || Eq(machine, "MaxMuller") || Eq(machine, "LB45");
            string operation = Eq(machine, "MaxMuller") || Eq(machine, "LB45") ? "Borring i skepp 6" : "";
            kv["SumMaskinValS1"] = "Maskin: " + (enabled ? machine : "") + " - " + operation;
            kv["SumF1_1"] = enabled ? "1/1" : ""; kv["SumF1_2"] = enabled ? "1/3" : ""; kv["SumF1_3"] = enabled ? "1/3" : ""; kv["SumF1_4"] = enabled ? "1/3" : "";
            kv["SumD1_1"] = enabled ? "Skjutmått" : ""; kv["SumD1_2"] = enabled ? "Gängtolk" : ""; kv["SumD1_3"] = enabled ? "Ytjämnhetsmätare" : ""; kv["SumD1_4"] = enabled ? "Skjutmått/Djupmått" : "";
            kv["SumAF1_1"] = ""; kv["SumAF1_2"] = ""; kv["SumAF1_3"] = enabled ? "Övrig ytjämnhet: Ra=12.5" : ""; kv["SumAF1_4"] = "";
            kv["SumTextS1"] = "Skarpa kanter avgradas";
            kv["SumRit"] = slash ? "7440248" : "7440244";
            kv["VaLV21"] = v21 ? "Använd särskild mall för V21 varianterna" : "";
            return kv;
        }

        private static double GetTypeValue(string designation, string templateType, string[] parts, bool slash)
        {
            bool is0005 = designation.Contains("0005") || templateType.IndexOf("0005", StringComparison.OrdinalIgnoreCase) >= 0;
            if (is0005)
            {
                double explicitValue = FindStandaloneType(designation);
                if (explicitValue > 0) return explicitValue;
                if (designation.StartsWith("7433526", StringComparison.OrdinalIgnoreCase)) return 600;
                double configuredValue = FindConfiguredEndType(templateType);
                if (configuredValue > 0) return configuredValue;
                return 0;
            }
            if (parts.Length > 1 && parts[1].Length > 3) return P(parts[1].Substring(parts[1].Length - 2));
            return parts.Length > 2 ? P(parts[2]) : 0;
        }

        private static double GetVariant(string[] parts, bool slash)
        {
            if (!slash) return parts.Length > 3 ? P(parts[3]) : 0;
            return parts.Length > 3 ? P(parts[3]) : 0;
        }

        private static double FindStandaloneType(string value)
        {
            double[] allowed = { 34, 36, 38, 40, 44, 48, 52, 56, 60, 64, 68, 72, 76, 80, 84, 88, 92, 96, 500, 530, 560, 600 };
            string[] tokens = (value ?? "").Replace("/", " ").Replace("-", " ").Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string token in tokens)
            {
                double n = P(token);
                if (allowed.Any(x => Near(x, n))) return n;
            }
            return 0;
        }

        private static double FindConfiguredEndType(string value)
        {
            string[] tokens = (value ?? "").Replace("-", "_").Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = tokens.Length - 1; i >= 0; i--)
            {
                double n = P(tokens[i]);
                if (Near(n, 0005)) continue;
                if (n == 500 || n == 530 || n == 560 || n == 600) return n;
            }
            return 0;
        }

        private static double[] GetDimensions(double t, double v, bool slash)
        {
            if (!slash)
            {
                string map = "34=170,150,178,184;36=180,160,190,196;38=190,170,200,206;40=200,180,211,215;44=220,200,232,238;48=260,220,274,280;52=280,240,294,300;56=290,260,314,320;60=310,280,334,340;64=340,300,354,360;68=360,320,374,380;72=380,340,394,400;76=400,360,414,420;80=420,380,434,440;84=440,400,454,460;88=450,410,464,470;92=470,430,484,490;96=490,450,504,510;500=510,470,524,530;530=540,500,554,560;560=570,530,584,590;600=610,560,624,630";
                foreach (string row in map.Split(';'))
                {
                    string[] pair = row.Split('=');
                    if (Near(P(pair[0]), t)) return pair[1].Split(',').Select(P).ToArray();
                }
                return A(0, 0, 0, 0);
            }
            string special = "34|115=147,115,154,164;36|6.1=180,165.1,190,196;38|140=172,140,188,194;38|160=190,160,200,206;38|6.15=196,176.212,206,212;38|180=200,180,210,216;40|5.15=170,150.81,178,184;40|7.3=202.5,182.56,213.5,217.5;40|170=190,170,201,205;40|190=210,190,221,225;48|8=240,203.2,254,260;48|7.3=220,182.56,231,237;48|200=240,200,254,260;48|228.6=270,228.6,282.5,288.5;52|220=260,220,274,280;52|8.15=267,227,281,287;52|9.7=280,239.71,294,300;56|10=290,254,314,320;56|10.7=295,265.11,319,325;56|220=260,220,274,280;56|240=280,240,294,300;60|11=310,279.4,334,340;60|10.15=310,277.8,334,340;68|310=350,310,364,370;68|12.7=360,315.913,374,380;76|320=360,320,379,390;76|340=380,340,394,400;76|370=410,370,424,430;80|15=420,381,434,440;84|355.6=395,355.6,417,425;530|460=500,460,514,520;530|480=520,480,534,540;530|490=530,490,544,550;530|510=550,510,564,570";
            foreach (string row in special.Split(';'))
            {
                string[] pair = row.Split('='); string[] key = pair[0].Split('|');
                if (Near(P(key[0]), t) && Near(P(key[1]), v)) return pair[1].Split(',').Select(P).ToArray();
            }
            return A(0, 0, 0, 0);
        }

        private static double[] A(params double[] x) => x;
        private static double P(string s) => double.TryParse((s ?? "").Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double x) ? x : 0;
        private static bool Eq(string a, string b) => string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);
        private static bool Near(double a, double b) => Math.Abs(a - b) < .000001;
        private static string N(double x) => Near(x, Math.Round(x)) ? x.ToString("0", CultureInfo.InvariantCulture) : x.ToString("0.###", CultureInfo.InvariantCulture).Replace(".", ",");
        private static string F3(double x) => x.ToString("0.000", CultureInfo.InvariantCulture);
    }
}
