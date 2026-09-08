using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class Kilhylsa_Komplett_Svarv_Ett_Borrhal : ITemplateCalculations
    {
        private string subject = string.Empty;
        private string originalSubject = string.Empty;
        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var result = new Dictionary<string, string>();


            originalSubject = (request?.ProductDesignation ?? "").ToUpper().Trim();
            subject = originalSubject;
            string calcSubject = subject;

            if (calcSubject == "211629-9/V01" || calcSubject == "211629-9/V03")
            {
                calcSubject = "KOH 39/630/600";
            }

            var parts = Regex.Split(calcSubject, @"[ /.\-]+")
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToArray();
            var isSlashFormat = calcSubject.Contains("/");
            var isSpecialSubject = calcSubject.Length > 10;
            int serie = GetSerieFromParts(parts);
            int typ = GetTypFromParts(parts, isSlashFormat);
            int typIndex = GetTypIndex(serie, typ);
            if (calcSubject == "MS-238370")
            {
                serie = 39;
                typ = 630;
                typIndex = GetTypIndex(serie, typ);
            }
            if (calcSubject == "ASU-0002")
            {
                serie = 39;
                typ = 750;
                typIndex = GetTypIndex(serie, typ);
            }
            double kona = GetVal(request, "Kona", (serie == 38 || serie == 39 || calcSubject == "MS-238370" || calcSubject == "ASU-0002") ? 12 : GetKona(serie));
            // Lotus mirror: Tmpd = Round(if([Ytterdiameter lillkona (d)] = 0; default; [Ytterdiameter lillkona (d)] + ([a-mått] / TmpKona)); 0.001)
            double d = CalculateD(request, calcSubject, parts, typ, kona);

            double d1 = GetVal(request, "Innerdiameter (d1)", GetDefaultD1(parts, d, typ, isSpecialSubject, isSlashFormat, calcSubject));
            double inputL = GetVal(request, "Längd (L)", 0);

            double TmpL1 = GetTmpL1(serie, typIndex, inputL);

            double L = TmpL1;
            double d3 = GetVal(request, "Fasdiameter (d3)", GetDefaultD3(serie, typIndex, d, calcSubject));
            // Lotus mirror: Tmpd2 = Round((TmpL / TmpKona) + Tmpd; 0.001)
            double d2 = CalculateD2(L, kona, d);

            double ML = GetML(L);
            double VT = GetVT(d, ML);
            double RHA = GetRHA(d);
            double RHB = GetRHB(d);

            double C = GetVal(request, "Slits", 10);

            double oilOffset = GetVal(request, "Längd till Oljespår (B)", 0);

            double B = TmpL1 - oilOffset;

            double J = GetVal(request, "Längd till Repor (J)", 10);
            double K = GetVal(request, "Längd Repor (K)", 15);
            int n = (int)GetVal(request, "Antal repor (n)", 6);
            double repB = GetVal(request, "RepBredd", GetDefaultRepB(serie, typ));
            double repD = GetVal(request, "RepDjup", 0.5);

            double E = GetVal(request, "Bredd Oljespår (E)", 8);

            double oilLen = GetVal(request, "Längd Oljeborrhål", 20);
            double oilDiam = GetVal(request, "Ø Oljeborrhål", 4);
            double oilDepth = GetVal(request, "Oljeborrhål Djup", 10);
            double bh = GetVal(request, "Borrhålsmått (BH)", 10);
            double hole2 = GetVal(request, "Ø Borrhål till Oljespår", 2);


            Merge(result, Dimensions(L, d, d1, d2));
            Merge(result, Tolerances(d1, L));
            Merge(result, Geometry(kona));
            Merge(result, Shape(d, d1));
            Merge(result, Precision(RHA, RHB, VT, ML));
            string machine = GetMachineValue(request);
            Merge(result, Machine(calcSubject, machine, RHA, RHB, VT, d, d1));
            Merge(result, Static());
            Merge(result, MachinePage2(machine));

            Merge(result, SlotAndBasic(C, B, J, K));
            Merge(result, Repor(n, serie, typ, repB, repD));
            Merge(result, OilGroove(E));
            Merge(result, OilHole(oilLen, oilDiam, oilDepth, bh, hole2));
            Merge(result, PostSlot(d1, ResolvePostSlotTyp(calcSubject, typ, d)));

            Merge(result, SurfaceBlock());
            Merge(result, D3Block(d3));
            Merge(result, Gauge(L, d, d1, d2, kona));
            Merge(result, DrawingBlock(serie, request));
            Merge(result, AngleBlock(serie, calcSubject));
            Merge(result, SVOS());
            Merge(result, Page2Text());
            Merge(result, Thread());
            Merge(result, Generic(d));
            Merge(result, GaugeBlock(L, d, d1, d2, kona));

            return result;
        }


        private double GetVal(APIRequest req, string name, double fallback)
        {
            var val = req?.Bookmarks?
                .FirstOrDefault(x => x.BookmarkName.Equals(name, StringComparison.OrdinalIgnoreCase))
                ?.BookmarkValue;

            if (string.IsNullOrWhiteSpace(val)) return fallback;

            val = val.Replace(",", ".");
            return double.TryParse(val, NumberStyles.Any, CultureInfo.InvariantCulture, out double d)
                ? d : fallback;
        }

        private int GetSerie(string txt)
        {
            var m = Regex.Match(txt ?? "", @"\d+");
            if (!m.Success) return 0;
            int v = int.Parse(m.Value);
            return v >= 100 ? v / 10 : v;
        }

        private int GetTyp(string bet2, string bet3)
        {
            if (int.TryParse(bet3, out int t3))
                return t3;

            var m = Regex.Match(bet2 ?? "", @"\d+");
            if (m.Success)
                return int.Parse(m.Value);

            return 0;
        }


        private int GetSerieFromParts(string[] parts)
        {
            if (parts.Length > 1 && int.TryParse(Left(parts[1], 2), out var serie)) return serie;
            return 0;
        }
        private int GetTypFromParts(string[] parts, bool slashFormat)
        {
            if (parts.Length < 3) return 0;
            if (parts[1].Length > 3 && int.TryParse(Right(parts[1], 2), out var compactTyp)) return compactTyp;
            if (slashFormat && int.TryParse(parts[2], out var slashTyp)) return slashTyp;
            return int.TryParse(parts[2], out var typ) ? typ : 0;
        }
        private bool TryGetVal(APIRequest req, string name, out double value)
        {
            value = 0;
            var val = req?.Bookmarks?
                .FirstOrDefault(x => x.BookmarkName.Equals(name, StringComparison.OrdinalIgnoreCase))
                ?.BookmarkValue;
            if (string.IsNullOrWhiteSpace(val)) return false;
            val = val.Replace(",", ".");
            return double.TryParse(val, NumberStyles.Any, CultureInfo.InvariantCulture, out value);
        }

        private string GetBookmarkText(APIRequest req, string name)
        {
            return req?.Bookmarks?
                .FirstOrDefault(x => x.BookmarkName.Equals(name, StringComparison.OrdinalIgnoreCase))
                ?.BookmarkValue?.Trim() ?? string.Empty;
        }

        private string GetMachineValue(APIRequest req)
        {
            // Lotus uses [MaskinVal]. API sometimes sends the same value in MachineNumber.
            if (!string.IsNullOrWhiteSpace(req?.MachineNumber)) return req.MachineNumber.Trim();
            return GetBookmarkText(req, "MaskinVal");
        }

        private int GetTypIndex(int serie, int typ)
        {
            int[] list38 = { 52, 56, 60, 64, 68, 72, 76, 80, 84, 88, 92, 96, 500, 530, 560, 600, 630, 670, 710, 750, 800, 850, 900, 950, 1000, 1060, 1120, 1180, 1250, 1320, 1400 };
            int[] list39 = { 44, 48, 52, 56, 60, 64, 68, 72, 76, 80, 84, 88, 92, 96, 500, 530, 560, 600, 630, 670, 710, 750, 800, 850, 900, 950, 1000, 1060, 1120, 1180, 1250 };
            var list = serie == 38 ? list38 : list39;
            var index = Array.IndexOf(list, typ);
            return index < 0 ? 1 : index + 1;
        }
        private double CalculateD(APIRequest request, string calcSubject, string[] parts, int typ, double kona)
        {
            if (TryGetVal(request, "Ytterdiameter lillkona (d)", out double enteredD) && Math.Abs(enteredD) > 0.0000001)
            {
                double aMatt = GetVal(request, "a-mått", 0);
                return Math.Round(enteredD + (kona == 0 ? 0 : aMatt / kona), 3, MidpointRounding.AwayFromZero);
            }

            if (calcSubject == "ASU-0002") return 750.3;

            return Math.Round(GetDefaultD(calcSubject, parts, typ), 3, MidpointRounding.AwayFromZero);
        }

        private double CalculateD2(double L, double kona, double d)
        {
            return Math.Round((kona == 0 ? 0 : L / kona) + d, 3, MidpointRounding.AwayFromZero);
        }

        private double GetDefaultD(string calcSubject, string[] parts, int typ)
        {
            if (calcSubject == "MS-238370") return 630;
            if (calcSubject == "ASU-0002") return 750.3;
            if (parts.Length > 2 && double.TryParse(parts[2].Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out var d)) return d;
            return typ > 0 ? typ / 2.0 * 10.0 : 0;
        }
        private double GetDefaultD1(string[] parts, double d, int typ, bool isSpecialSubject, bool isSlashFormat, string calcSubject)
        {
            if (calcSubject == "MS-238370") return 600;
            if (calcSubject == "ASU-0002") return 700;
            if ((isSpecialSubject || isSlashFormat) && parts.Length > 3 && double.TryParse(parts[3].Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out var d1)) return d1;
            return Getd1(d, typ);
        }
        private double GetDefaultD3(int serie, int typIndex, double d, string calcSubject)
        {
            if (calcSubject == "MS-238370") return 630;
            if (serie == 39)
            {
                double[] list = { 218, 238, 258, 278, 300, 318, 338, 358, 378, 398, 418, 438, 458, 478, 498, 526, 556, 596, 630, 670, 710, 750, 800, 850, 900, 950, 1000, 1060, 1120, 1180, 1250 };
                return list[Math.Max(0, Math.Min(typIndex - 1, list.Length - 1))];
            }
            return d;
        }
        private string Left(string input, int count)
        {
            if (string.IsNullOrEmpty(input)) return "";
            return input.Length <= count ? input : input.Substring(0, count);
        }
        private string Right(string input, int count)
        {
            if (string.IsNullOrEmpty(input)) return "";
            return input.Length <= count ? input : input.Substring(input.Length - count);
        }
        private bool IsTurningMachine(string machine)
        {
            return !string.IsNullOrWhiteSpace(machine) && (machine.Equals("VTR-160", StringComparison.OrdinalIgnoreCase) || machine.Equals("MacTurn 550", StringComparison.OrdinalIgnoreCase));
        }
        private bool IsBorrMachine(string machine)
        {
            return !string.IsNullOrWhiteSpace(machine) && (machine.Equals("Skepp 6", StringComparison.OrdinalIgnoreCase) || machine.Equals("VTR-160", StringComparison.OrdinalIgnoreCase) || machine.Equals("MacTurn 550", StringComparison.OrdinalIgnoreCase));
        }
        private double GetKona(int s) => (s == 38 || s == 39) ? 12 : 30;

        private double GetTmpL1(int serie, int typIndex, double inputL)
        {
            if (inputL != 0)
                return inputL;

            var map = new Dictionary<int, double[]>
    {
        {
            39, new double[]
            {
                71,71,85,85,103,103,103,103,118,118,118,
                132,132,140,140,150,155,165,180,185,
                200,206,218,224,236,250,265,280,280,300,315
            }
        },
        {
            38, new double[]
            {
                0,0,55,62,72,72,72,72,87,87,87,87,
                102,102,102,106,106,115,130,130,
                140,150,160,160,165,175,195,195,
                208,208,215,236,254
            }
        }
    };

            if (!map.ContainsKey(serie)) return 0;

            var arr = map[serie];

            int index = Math.Max(0, Math.Min(typIndex - 1, arr.Length - 1));

            return arr[index];
        }

        private double Getd(int t) => t > 0 ? t * 5 : 0;

        private double Getd1(double d, int t)
        {
            if (t < 61) return d - 10;
            if (t < 501) return d - 15;
            if (t < 671) return d - 20;
            if (t < 901) return d - 25;
            return d - 30;
        }

        private double GetML(double L) => L < 80 ? 50 : L < 110 ? 75 : 100;

        private double GetVT(double d, double ML)
        {
            double factor = d < 50.1 ? 0.6 : d < 80.1 ? 0.5 : d < 120.1 ? 0.45 : d < 150.1 ? 0.3 : d < 180.1 ? 0.18 : d < 400.1 ? 0.15 : d < 500.1 ? 0.13 : d < 630.1 ? 0.12 : d < 800.1 ? 0.11 : d < 1000.1 ? 0.10 : d < 1250.1 ? 0.09 : d < 1600.1 ? 0.08 : 0.07;
            return Math.Round((ML * factor) / 1000, 4, MidpointRounding.AwayFromZero);
        }

        private double GetRHA(double d) => d >= 600 ? 0.016 : 0.008;
        private double GetRHB(double d) => d >= 600 ? 0.024 : 0.012;

        private double GetD1Tol(double d1)
        {
            if (d1 < 3.01) return 0.020;
            if (d1 < 6.01) return 0.024;
            if (d1 < 10.01) return 0.029;
            if (d1 < 18.01) return 0.035;
            if (d1 < 30.01) return 0.042;
            if (d1 < 50.01) return 0.050;
            if (d1 < 80.01) return 0.060;
            if (d1 < 120.01) return 0.070;
            if (d1 < 180.01) return 0.080;
            if (d1 < 250.01) return 0.092;
            if (d1 < 315.01) return 0.105;
            if (d1 < 400.01) return 0.115;
            if (d1 < 500.01) return 0.125;
            if (d1 < 630.01) return 0.140;
            if (d1 < 800.01) return 0.160;
            if (d1 < 1000.01) return 0.180;
            if (d1 < 1250.01) return 0.210;
            if (d1 < 1600.01) return 0.250;
            if (d1 < 2000.01) return 0.300;
            if (d1 < 2500.01) return 0.350;
            return 0.430;
        }

        private double GetD3Tol(double d3)
        {
            if (d3 < 3.01) return 0.070;
            if (d3 < 6.01) return 0.090;
            if (d3 < 10.01) return 0.110;
            if (d3 < 18.01) return 0.135;
            if (d3 < 30.01) return 0.165;
            if (d3 < 50.01) return 0.195;
            if (d3 < 80.01) return 0.230;
            if (d3 < 120.01) return 0.270;
            if (d3 < 180.01) return 0.315;
            if (d3 < 250.01) return 0.360;
            if (d3 < 315.01) return 0.405;
            if (d3 < 400.01) return 0.445;
            if (d3 < 500.01) return 0.485;
            if (d3 < 630.01) return 0.550;
            if (d3 < 800.01) return 0.625;
            if (d3 < 1000.01) return 0.700;
            if (d3 < 1250.01) return 0.825;
            if (d3 < 1600.01) return 0.975;
            if (d3 < 2000.01) return 1.150;
            if (d3 < 2500.01) return 1.400;
            return 1.650;
        }

        private string GetGV(double d) => d >= 600 ? "0.040" : "0.008";

        private Dictionary<string, string> Dimensions(double L, double d, double d1, double d2)
            => new()
            {
                ["SumL"] = "(L) " + L,
                ["Sumd"] = "(d) " + d.ToString("0.###", CultureInfo.InvariantCulture),
                ["Sumd1"] = "(d1) " + d1,
                ["Sumd2"] = "(d2) " + d2.ToString("0.###", CultureInfo.InvariantCulture)
            };

        private Dictionary<string, string> Tolerances(double d1, double L)
            => new()
            {
                ["SumLTol"] = "+ 0",
                ["SumLTolN"] = CalculateSumLTolN(L),
                ["Sumd1Tol"] = "± " + GetD1Tol(d1).ToString("0.000", CultureInfo.InvariantCulture)
            };
        public string CalculateSumLTolN(double value)
        {
            double rate;

            if (value <= 180)
            {
                rate = 1.600;
            }
            else if (value <= 360)
            {
                rate = 1.850;
            }
            else
            {
                rate = 2.300;
            }

            string result = $"- {rate:F3} [3F]";
            return result;
        }

        private Dictionary<string, string> Geometry(double kona)
            => new()
            {
                ["SumKona"] = "Kona 1:" + kona,
                ["SumR"] = "R2",
                ["Sum45"] = "45°"
            };

        private Dictionary<string, string> Shape(double d, double d1)
            => new()
            {
                ["SumRa25"] = "2.5",
                ["SumRa5"] = "5",
                ["Sum1Ra5"] = "5",
                ["SumGV"] = "max: " + GetGV(d),
                ["SumRd"] = "max: " + GetD1Tol(d1)
            };

        private Dictionary<string, string> Precision(double rha, double rhb, double vt, double ml)
            => new()
            {
                ["SumRHA"] = $"max: {rha} [2F]",
                ["SumRHB"] = $"max: {rhb} [2F]",
                ["SumVT"] = $"Konavvikelse: ± {vt.ToString("0.###", CultureInfo.InvariantCulture)} [2F]",
                ["SumML"] = "ML=" + ml
            };

        private Dictionary<string, string> Machine(string calcSubject,string machine, double RHA, double RHB, double VT, double d, double d1)
        {
            var kv = new Dictionary<string, string>();
            string displayMachine = GetDisplayMachineForPage1(machine);
            kv["SumMaskinValS1"] = "Maskin: " + displayMachine + " - OP1";

            for (int i = 0; i <= 11; i++)
            {
                kv[$"SumF1_{i}"] = "";
                kv[$"SumD1_{i}"] = "";
                kv[$"SumAF1_{i}"] = "";
            }

            if (!IsTurningMachine(machine)) return kv;

            kv["SumF1_1"] = "1/5";
            kv["SumF1_2"] = "1/5";
            kv["SumF1_3"] = "1/5";
            kv["SumF1_4"] = "1/5";
            kv["SumF1_5"] = "1/5";
            kv["SumF1_6"] = "1/5";
            kv["SumF1_7"] = "1/2";
            kv["SumF1_8"] = "1/5";
            kv["SumF1_9"] = "1/5";
            kv["SumF1_0"] = "1/5";
            kv["SumF1_11"] = "1/5";

            kv["SumD1_1"] = "Skjutmått";
            kv["SumD1_2"] = "Skjutmått";
            kv["SumD1_3"] = "Skjutmått";
            kv["SumD1_4"] = "Radielyra";
            kv["SumD1_5"] = "Skjutmått";
            kv["SumD1_6"] = "Egglinjal";
            kv["SumD1_7"] = "Egglinjal";
            kv["SumD1_8"] = "Mätbygel SR 7415991";
            kv["SumD1_9"] = "Mätbygel SR 7415991";
            kv["SumD1_0"] = "Mätbygel SR 7415991";
            kv["SumD1_11"] = "Mätmaskin";

            kv["SumAF1_6"] = $"max: {RHA.ToString("0.###", CultureInfo.InvariantCulture)} [2F]";
            kv["SumAF1_7"] = $"max: {RHB.ToString("0.###", CultureInfo.InvariantCulture)} [2F]";
            kv["SumAF1_8"] = $"max: {(calcSubject == "ASU-0002" ? "0.040" : "0.035")} [2F]";
            kv["SumAF1_9"] = "+ " + GetGodstjocklekTol(d).ToString("0.000", CultureInfo.InvariantCulture) + " <<LineBreak>> - " + GetGodstjocklekTolN(d).ToString("0.000", CultureInfo.InvariantCulture);
            kv["SumAF1_0"] = $"Konavvikelse: ± {VT.ToString("0.0000", CultureInfo.InvariantCulture)} [2F]";
            kv["SumAF1_11"] = $"max: {GetD1Tol(d1).ToString("0.000", CultureInfo.InvariantCulture)} [3F]";
            return kv;
        }

        private Dictionary<string, string> Static()
            => new()
            {
                ["SumTextS1"] = "Bryt alla kanter, avlägsna",
                ["SumKlEgenskaper"] =
 "PRODUCTION NUTS & SLEEVES & HOUSINGS\\ALLMÄNKLASSADE EGENSKAPER\\Klassade egenskaper kilhylsor",
            };

        private Dictionary<string, string> MachinePage2(string machine)
        {
            var kv = new Dictionary<string, string>();
            for (int i = 1; i <= 4; i++)
            {
                kv[$"SumF2_{i}"] = "";
                kv[$"SumD2_{i}"] = "";
                kv[$"SumAF2_{i}"] = "";
            }

            string displayMachine = GetDisplayMachineForPage2(machine);
            kv["SumMaskinValS2"] = "Maskin: " + displayMachine + " - Borrning, Fräsning";

            if (IsBorrMachine(machine))
            {
                kv["SumF2_1"] = "1/1";
                kv["SumF2_2"] = "1/1";
                kv["SumF2_3"] = "1/1";
                kv["SumF2_4"] = "1/1";
                kv["SumD2_1"] = "Gängtolk";
                kv["SumD2_2"] = "Skjutmått";
                kv["SumD2_3"] = "Skjutmått";
                kv["SumD2_4"] = "Skjutmått";
                kv["SumAF2_3"] = "Tolerans efter slits enl. 7437495";
            }
            else if (string.IsNullOrWhiteSpace(machine))
            {
                kv["SumF2_1"] = "1/1";
                kv["SumF2_2"] = "1/1";
            }
            return kv;
        }

        private string GetGeneralTol(double value)
        {
            double tol =
                value < 6.01 ? 0.1 :
                value < 30.01 ? 0.2 :
                value < 120.01 ? 0.3 :
                value < 400.01 ? 0.5 :
                value < 1000.01 ? 0.8 :
                value < 2000.01 ? 1.2 : 2.0;

            return "± " + tol.ToString("0.0", CultureInfo.InvariantCulture);
        }

        private Dictionary<string, string> SlotAndBasic(double C, double B, double J, double K)
    => new()
    {
        ["SumC"] = "(c) " + C.ToString("0.###", CultureInfo.InvariantCulture),
        ["SumB"] = "(B) " + B.ToString("0.###", CultureInfo.InvariantCulture),
        ["SumJ"] = "(J) " + J.ToString("0.###", CultureInfo.InvariantCulture),
        ["SumK"] = "(K) " + K.ToString("0.###", CultureInfo.InvariantCulture),

        ["SumBTol"] = GetGeneralTol(B),
        ["SumJTol"] = GetGeneralTol(J),
        ["SumKTol"] = GetGeneralTol(K),

        ["SumCTol"] = C < 6 ? "± 0.1" : "± 0.2"
    };
        private Dictionary<string, string> Repor(int n, int serie, int typ, double repB, double repD)
        {
            var kv = new Dictionary<string, string>();
            double svRep = (serie == 39 && typ == 1180) ? 10 : 12;
            kv["SumAntRep"] = "(n) " + n + " axiella spår";
            kv["SumAntRep2"] = n + " axiella spår";
            kv["SumSVRep"] = svRep.ToString("0.#", CultureInfo.InvariantCulture) + "º";
            kv["SumRepB"] = "(RB) " + repB.ToString("0.###", CultureInfo.InvariantCulture);
            kv["SumRepD"] = "(RD) " + repD.ToString("0.###", CultureInfo.InvariantCulture);
            double vd = (n > 1) ? Math.Round((360.0 - (svRep * 2)) / (n - 1), 1, MidpointRounding.AwayFromZero) : 0;
            kv["SumVDRep"] = vd.ToString("0.#", CultureInfo.InvariantCulture) + "º";
            return kv;
        }

        private double GetDefaultRepB(int serie, int typ)
        {
            if (serie == 39 && typ == 630) return 1;
            if (serie == 38 && typ == 850) return 2;
            return 1.5;
        }

        private Dictionary<string, string> OilGroove(double E)
        {
            double eNew = Convert.ToDouble(E.ToString(CultureInfo.InvariantCulture).Split('.')[0]);
            var kv = new Dictionary<string, string>();
            kv["SumE"] = "(E) " + E.ToString("0.###", CultureInfo.InvariantCulture);
            kv["SumETol"] = E < 6.01 ? "± 0.1" : "± 0.2";
            kv["Sumr1"] = E == 5 ? "R4" : E == 6 ? "R4,5" : E == 7 ? "R5" : E == 8 || E == 9 ? "R6" : E == 10 ? "R7" : "R8";
            kv["SumF"] = eNew == 5 ? "1" : eNew == 6 ? "1.2" : eNew == 7 || eNew == 8 ? "1.5" : eNew == 9 || eNew == 10 ? "2" : eNew == 12 ? "2.5" : "2";
            kv["SumFTol"] = "± 0.1";
            return kv;
        }

        private Dictionary<string, string> OilHole(double length, double diameter, double depth, double bh, double hole2)
        {
            var kv = new Dictionary<string, string>();

            kv["SumLOH"] = length.ToString();
            kv["SumLOHTol"] = GetGeneralTol(length);

            kv["SumOHDj"] = depth.ToString();

            kv["SumOHDjTol"] = "± 0.2";

            kv["SumBOH"] = "Ø " + diameter;
            kv["SumBOHTol"] = GetGeneralTol(diameter);

            kv["SumOHG"] = "G 1/8";

            kv["SumOHGDj"] = (subject == "ASU-0002" || subject == "KOH 39/630/600") ? "min 10" : "min 11";

            kv["SumBHM"] = "(BH) " + bh;

            kv["SumH"] = "Ø " + hole2;
            kv["SumHTol"] = "± 0.1";

            return kv;
        }
        private Dictionary<string, string> PostSlot(double d1, int typ)
        {
            double tolPlus = GetD4Tol(typ);
            double tolMinus = GetD4TolNegative(typ);

            return new Dictionary<string, string>
            {
                ["Sumd4"] = "(d1) " + d1.ToString("0.###", CultureInfo.InvariantCulture),
                ["Sumd4Tol"] = "+ " + tolPlus.ToString("0.000", CultureInfo.InvariantCulture) + " [3F]",
                ["Sumd4TolN"] = "- " + tolMinus.ToString("0.000", CultureInfo.InvariantCulture) + " [3F]"
            };
        }

        private int ResolvePostSlotTyp(string calcSubject, int typ, double d)
        {
            // Mirrors Lotus page-2 TmpTyp1, not the normalized calculation typ.
            // Examples:
            //   MS-238370  -> parts[1] = 238370 -> Right(...,2) = 70 -> +0.360/-0.570
            //   ASU-0002   -> parts[1] = 0002   -> Right(...,2) = 2  -> +0.185/-0.290
            //   KOH 39/630 -> parts[2] = 630 -> +0.440/-0.700
            var p = Regex.Split(calcSubject ?? string.Empty, @"[ /\.\-]+")
                         .Where(x => !string.IsNullOrWhiteSpace(x))
                         .ToArray();

            if (p.Length > 1 && p[1].Length > 3 && int.TryParse(Right(p[1], 2), out int compactTyp))
                return compactTyp;

            if (p.Length > 2 && int.TryParse(p[2], out int parsedTyp))
                return parsedTyp;

            if (typ > 0) return typ;

            // Last-resort inference prevents falling into the <53 bucket when typ parsing fails.
            if (d >= 700 && d < 800) return 750;
            if (d >= 600 && d < 670) return 630;
            return typ;
        }

        private double GetD4Tol(int typ)
        {
            if (typ < 53) return 0.185;
            if (typ < 65) return 0.210;
            if (typ < 85) return 0.360;
            if (typ < 535) return 0.400;
            if (typ < 675) return 0.440;
            if (typ < 855) return 0.500;
            return 0.500;
        }

        private double GetD4TolNegative(int typ)
        {
            if (typ < 53) return 0.290;
            if (typ < 65) return 0.320;
            if (typ < 85) return 0.570;
            if (typ < 535) return 0.630;
            if (typ < 675) return 0.700;
            if (typ < 855) return 0.800;
            return 0.900;
        }

        private Dictionary<string, string> SurfaceBlock()
        {
            var kv = new Dictionary<string, string>();

            kv["SumRa25a"] = "2.5";
            kv["SumRa25b"] = "2.5";

            return kv;
        }
        private string GetDisplayMachineForPage1(string machine)
        {
            if (string.IsNullOrWhiteSpace(machine)) return "";
            machine = machine.Trim();
            return IsTurningMachine(machine) ? machine : "";
        }

        private string GetDisplayMachineForPage2(string machine)
        {
            if (string.IsNullOrWhiteSpace(machine)) return "";
            machine = machine.Trim();
            return IsBorrMachine(machine) ? machine : "";
        }

        private Dictionary<string, string> D3Block(double d3)
        {
            var kv = new Dictionary<string, string>();

            bool isV03 = subject.Contains("V03");

            if (isV03)
            {
                kv["Sumd3"] = "Radie10";
                kv["Sumd3Tol"] = "";
            }
            else
            {
                kv["Sumd3"] = "(d3) " + d3.ToString("0.###", CultureInfo.InvariantCulture);
                kv["Sumd3Tol"] = "± " + GetD3Tol(d3).ToString("0.000", CultureInfo.InvariantCulture);
            }

            return kv;
        }


        private Dictionary<string, string> Gauge(double L, double d, double d1, double d2, double k)
            => new() { ["SumL1"] = "140", };
        private Dictionary<string, string> GaugeBlock(double L, double d, double d1, double d2, double kona)
        {
            var kv = new Dictionary<string, string>();

            double TML = L - 7;

            double L1 = TML < 110 ? 83 :
                        TML < 145 ? 108 : 140;

            double L2 = TML < 145 ? 8 : 40;

            double E1 = Math.Round(((d2 - d1) / 2) - ((L1 + 1) / (2 * kona)), 3);
            double E2 = Math.Round(((d2 - d1) / 2) - ((L2 + 1) / (2 * kona)), 3);

            kv["SumL1"] = L1.ToString();
            kv["SumL2"] = L2.ToString();

            kv["SumE11"] = E1.ToString("0.###", CultureInfo.InvariantCulture);
            kv["SumE22"] = E2.ToString("0.###", CultureInfo.InvariantCulture);

            return kv;
        }


        private Dictionary<string, string> DrawingBlock(int serie, APIRequest request)
        {
            var kv = new Dictionary<string, string>();

            string ritnSvarv = request?.Bookmarks?
                .FirstOrDefault(x => x.BookmarkName.Equals("Ritningsnummer Svarv", StringComparison.OrdinalIgnoreCase))
                ?.BookmarkValue;

            if (!string.IsNullOrWhiteSpace(ritnSvarv) && ritnSvarv != "0")
                kv["SumRitningsnr"] = ritnSvarv;
            else if (subject == "MS-238370")
                kv["SumRitningsnr"] = "238370";
            else if (serie == 38)
                kv["SumRitningsnr"] = "238000";
            else if (serie == 39)
                kv["SumRitningsnr"] = "226472";
            else
                kv["SumRitningsnr"] = subject;

            kv["SumRitTol"] = "Toleranser: 1432010";

            string ritnBorr = request?.Bookmarks?
                .FirstOrDefault(x => x.BookmarkName.Equals("Ritningsnummer Borr", StringComparison.OrdinalIgnoreCase))
                ?.BookmarkValue;

            if (!string.IsNullOrWhiteSpace(ritnBorr) && ritnBorr != "0")
                kv["SumRitningsnr2"] = "Styckritning: " + ritnBorr;
            else if (subject == "MS-238370")
                kv["SumRitningsnr2"] = "Styckritning: 238370";
            else
                kv["SumRitningsnr2"] = "Styckritning: " + subject;

            kv["SumRitTol2"] = "Toleranser: 1432010, 7437495";
            kv["SumRitYtjämnhet"] = "Yta: 7430184";
            return kv;
        }

        private Dictionary<string, string> AngleBlock(int serie, string calcSubject)
        {
            
            var kv = new Dictionary<string, string>();
            kv["SumR2"] = "R2";
            if (calcSubject == "MS-238370" || calcSubject == "ASU-0002")
            {
                kv["SumR2"] = "";
            }

            kv["SumV45"] = (subject == "ASU-0002" || subject == "MS-238370") ? "" : "45º";
            //kv["SumV120"] = "120º";
            kv["SumV120"] = (subject == "ASU-0002" || subject == "MS-238370") ? "" : "120º";

            return kv;
        }


        private Dictionary<string, string> SVOS()
            => new() { ["SumSVOS"] = "10º" };

        private Dictionary<string, string> Page2Text()
        {
            var kv = new Dictionary<string, string>();

            kv["SumTextS2"] = "Bryt alla kanter, avlägsna";


            kv["SumKlEgenskaper2"] =
                "PRODUCTION NUTS & SLEEVES & HOUSINGS\\ALLMÄNKLASSADE EGENSKAPER\\Klassade egenskaper kilhylsor";

            return kv;
        }

        private Dictionary<string, string> Thread()
            => new() { ["SumG"] = "G 1/8" };

        private Dictionary<string, string> Generic(double d)
            => new() { ["Sum1"] = d.ToString("0.###") };
        private double GetGodstjocklekTol(double d)
        {
            if (subject == "ASU-0002") return d > 1600 ? 0.070 : d > 1250 ? 0.065 : d > 1000 ? 0.060 : d > 800 ? 0.055 : d > 630 ? 0.050 : d > 500 ? 0.045 : d > 400 ? 0.040 : d > 315 ? 0.035 : d > 250 ? 0.035 : d > 180 ? 0.030 : d > 120 ? 0.025 : d > 80 ? 0.022 : d > 50 ? 0.019 : d > 30 ? 0.016 : 0.013;
            return d > 1250 ? 0.100 : d > 1000 ? 0.095 : d > 800 ? 0.085 : d > 630 ? 0.075 : d > 500 ? 0.070 : d > 400 ? 0.065 : d > 315 ? 0.060 : d > 250 ? 0.055 : d > 180 ? 0.050 : d > 120 ? 0.040 : d > 80 ? 0.035 : d > 50 ? 0.030 : d > 30 ? 0.025 : 0.020;
        }
        private double GetGodstjocklekTolN(double d)
        {
            if (subject == "ASU-0002") return d > 1250 ? 0.195 : d > 1000 ? 0.170 : d > 800 ? 0.155 : d > 630 ? 0.140 : d > 500 ? 0.125 : d > 400 ? 0.115 : d > 315 ? 0.105 : d > 251 ? 0.095 : d > 180 ? 0.085 : d > 120 ? 0.075 : d > 80 ? 0.065 : d > 50 ? 0.055 : d > 30 ? 0.046 : 0.039;
            return d > 1250 ? 0.310 : d > 1000 ? 0.280 : d > 800 ? 0.250 : d > 630 ? 0.225 : d > 500 ? 0.200 : d > 400 ? 0.190 : d > 315 ? 0.175 : d > 250 ? 0.160 : d > 180 ? 0.140 : d > 120 ? 0.120 : d > 80 ? 0.105 : d > 50 ? 0.090 : d > 30 ? 0.075 : 0.070;
        }
        private string GetTolerance(double v)
        {
            if (v >= 600) return "0.140";

            return v < 30 ? "0.042" :
                   v < 50 ? "0.050" :
                   v < 80 ? "0.060" :
                   v < 120 ? "0.070" : "0.080";
        }

        private void Merge(Dictionary<string, string> t, Dictionary<string, string> s)
        {
            foreach (var kv in s) t[kv.Key] = kv.Value ?? "";
        }
    }
}
