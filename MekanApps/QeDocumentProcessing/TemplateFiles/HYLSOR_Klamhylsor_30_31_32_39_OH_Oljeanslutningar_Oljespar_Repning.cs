using DocumentFormat.OpenXml.Office2016.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Klamhylsor_30_31_32_39_OH_Oljeanslutningar_Oljespar_Repning : ITemplateCalculations
    {
            public Dictionary<string, string> CalculateWordParameters(APIRequest request)
            {
                var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                var subjectRaw = request?.ProductDesignation ?? string.Empty;
                var formatted = subjectRaw.Trim().ToUpper().Replace(".", ",");


                var parts = formatted.Split(new[] { ' ', '/', '-', '.' }, StringSplitOptions.RemoveEmptyEntries);


                var p2 = parts.ElementAtOrDefault(1) ?? string.Empty;
                var p3 = parts.ElementAtOrDefault(2) ?? string.Empty;


                var hasSlash = formatted.Contains("/");
                var len = p2.Length;


                var serieStr = formatted.Contains("MS") ? "0"
                    : hasSlash && (len == 2 || len == 3) ? p2
                    : len > 4 ? p2.Substring(0, 3)
                    : len == 3 ? p2.Substring(0, 1)
                    : p2.Substring(0, Math.Min(2, len));


                var typStr = !hasSlash
                    ? p2.Substring(Math.Max(0, p2.Length - 2))
                    : p3;


                int.TryParse(serieStr, out int serie);


                decimal.TryParse(typStr.Replace(',', '.'), System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture, out decimal typDec);


                int typ = (int)Math.Truncate(typDec);


                var drawing = !string.IsNullOrEmpty(subjectRaw) && subjectRaw != "0"
                    ? subjectRaw
                    : serie == 30 ? "7434164"
                    : serie == 31 ? "7434165"
                    : serie == 32 ? "7434166"
                    : serie == 39 ? "7434097, 7434098"
                    : formatted;

            //decimal B = serie == 30 ? (typ < 65 ? 7.5m : typ < 77 ? 8m : typ < 85 ? 8.5m : 11m)
            //            : serie == 31 ? (typ < 81 ? 9.5m : typ < 85 ? 10.5m : 14m)
            //            : serie == 32 ? (typ < 65 ? 9m : typ < 73 ? 10m : typ < 85 ? 11m : 16m)
            //            : 7m;

            decimal B = serie == 31 ? 7.5m : 10.5m;
                         

            decimal C = 15m;
                decimal T = 13.5m;
                decimal D = typ < 85 ? 5 : 6;
                decimal H = typ < 65 ? 1 : typ < 85 ? 1.2m : 1.5m;
                decimal F = typ < 85 ? 2 : 3;
                decimal N = typ < 65 ? 5 : typ < 85 ? 6 : 7;
                string Tol(decimal v) => v < 6.1m ? "± 0.1" : v < 30.1m ? "± 0.2" : v < 120.1m ? "± 0.3" : v < 315.1m ? "± 0.5" : "± 0.8";


                var eVal = GetListValue(serie, typ, true);
                var jVal = GetListValue(serie, typ, false);

                var machine = request?.MachineNumber ?? string.Empty;
                bool isQE = machine == "VTR-160" || machine == "MacTurn 550";
                var G = "G1/4";
                var angle = "2º";
                if (isQE)
                {
                    if (serie == 31)
                    {
                        C = 10; T = 6.3m; D = 3; H = 1.25m; N = 6.2m; F = 3;
                        G = "M6"; angle = "1º";
                        eVal = 120; jVal = 147.5;
                    }
                    else if (serie == 32)
                    {
                        C = 15; T = 13.5m; D = 5; H = 1.2m; N = 6; F = 2;
                        G = "G1/4"; angle = "2º";
                        eVal = 146; jVal = 168;
                    }
                }
            var map = new Dictionary<string, string>
            {
                ["SumRit"] = drawing,

                ["SumRitNr"] = serie == 31 ? "7433954"
              : serie == 32 ? "7434166"
              : drawing,

                ["SumB"] = ($"(B) {B}").Replace(".", ","),
                ["SumBTol"] = Tol(B),
                ["SumC"] = ($"(C) {C}").Replace(".", ","),
                ["SumCTol"] = Tol(C),
                ["SumT"] = ($"(T) {T} ").Replace(".",","),
                ["SumTTol"] = Tol(T),
                ["SumD"] = ($"(D) {D}").Replace(".", ","),
                ["SumDTol"] = Tol(D),
                ["SumH"] = ($"(H) {H}").Replace(".", ","),
                ["SumHTol"] = isQE ? "± 0.1" : (H < 6.1m ? "± 0.1" : "± 0.2"),
                ["SumF"] = ($"(F) {F}").Replace(".", ","),
                ["SumFTol"] = isQE ? "± 0.1" : "± 0.2",
                ["SumN"] = ($"(N) {N}").Replace(".",","),
                ["SumNTol"] = isQE ? "± 0.1" : "± 0.2",
                ["SumR1"] = $"R4,5",
                ["SumR"] = $"R1",
                ["SumG"] = G,
                ["SumE"] = ($"(E) {eVal}").Replace(".", ","),
                ["SumETol"] = Tol((decimal)eVal),
                ["SumJ"] = ($"(J) {jVal}").Replace(".", ","),
                ["SumJTol"] = Tol((decimal)jVal),
                ["SumV45"] = "45º",
                ["SumV120"] = "120º",
                ["SumV2"] = angle,
                ["SumMaskinValS1"] = $"Maskin: {machine} - BorrOljehål & Oljespår",
                ["SumMaskinValS2"] = $"Maskin: {machine} - Oljespår & Repor",
                ["SumGangstigning"] = "Max 2x gängstigning",
                ["SumOvrigt"] = "1 st. oljeborrhål 180º från slits 1 st.utv.oljespår med början 10º från slitscentrum",
                ["SumOvrigt2"] = string.Empty,
                ["SumTextS1"] = "Grader, fritfläckar, slagmärken, repor, valkar och andra defekter samt ojämnheter kontrolleras med ögonmått.",
                ["SumTextS2"] = "Grader, fritfläckar, slagmärken, repor, valkar och andra defekter samt ojämnheter kontrolleras med ögonmått.",
                ["SumTextGängstigning"] = "Max 2x gängstigning",
                ["SumÖvrigt"] = "1 st. oljeborrhål 180º från slits <<LineBreak>>1 st. utv. oljespår med början 10º från slitscentrum",
                ["SumÖvrigt2"] = ""
            };

                var freq = isQE ? "1/2" : string.Empty;


                for (int i = 0; i <= 11; i++)
                {
                    map[$"SumF1_{i}"] = freq;
                    map[$"SumD1_{i}"] = isQE ? GetDevice(i) : string.Empty;
                    map[$"SumAF1_{i}"] = string.Empty;
                }

                for (int i = 0; i <= 5; i++)
                {
                    map[$"SumF2_{i}"] = string.Empty;
                    map[$"SumD2_{i}"] = string.Empty;
                    map[$"SumAF2_{i}"] = string.Empty;
                }

                foreach (var kv in map)
                    result[kv.Key] = kv.Value ?? string.Empty;


                return result;
            }

            private double GetListValue(int serie, int typ, bool isE)
            {
                var typList = new[] { 64, 68, 72, 76, 80, 84, 88, 92, 96, 500, 530, 560, 600, 630, 670, 710, 750, 800, 850, 900, 863 };

                var eLists = new Dictionary<int, double[]>
                {
                    [30] = new[] { 78d, 86, 87, 88, 95, 97, 101, 105, 107, 108, 118, 125, 130, 137, 146, 152, 160, 166, 174, 0 },
                    [31] = new[] { 109, 118, 119, 120, 123, 137, 139, 147, 152, 161, 166, 172, 185, 193, 205, 212, 223, 230, 244, 253, 0.0 },
                    [32] = new[] { 126, 136, 140, 146, 155, 163, 169, 178, 187, 201, 211, 217.5, 232.5, 245.5, 260, 268.5, 282, 291.5, 306, 0 },
                    [39] = new[] { 61, 62, 62, 72, 72, 72, 80, 80, 87, 87, 92, 94, 102, 110, 113, 120, 123, 130, 133, 139, 139, 0.0 }
                };

                var jLists = new Dictionary<int, double[]>
                {
                    [30] = new[] { 96.5, 105, 105.5, 109, 118.5, 119.5, 130.5, 133, 134, 143, 151.5, 163, 165, 170.5, 183.5, 196, 202.5, 206, 212.5, 0 },
                    [31] = new[] { 121, 140.5, 144.5, 147.5, 152, 171, 171.5, 183, 186.5, 199, 202.5, 211, 220, 237, 257, 261.5, 276.5, 281, 298, 310.5, 0 },
                    [32] = new[] { 135.5, 156, 162.5, 168, 177, 192.5, 196, 208, 214.5, 231, 240, 249.5, 259.5, 280.5, 303, 308.5, 326, 331.5, 350, 0 },
                    [39] = new[] { 82.5, 85.5, 85.5, 95.5, 99.5, 99.5, 113, 113, 117.5, 125, 129, 138, 142.5, 149, 156.5, 171, 173, 178, 180, 192.5, 192.5 }
                };


                var index = Array.IndexOf(typList, typ);
                if (index < 0) return 0;


                var list = isE ? (eLists.ContainsKey(serie) ? eLists[serie] : null)
                               : (jLists.ContainsKey(serie) ? jLists[serie] : null);


                if (list == null || index >= list.Length) return 0;


                return list[index];
            }

            private string GetDevice(int i)
            {
                return i switch
                {
                    0 => "Skjutmått",
                    1 => "pipborr/djupmått",
                    2 => "Skjutmått",
                    3 => "Skjutmått",
                    4 => "Gängtolk",
                    5 => "Skjutmått",
                    6 => "Skjutmått/fasmall",
                    7 => "Skjutmått",
                    8 => "Skjutmått",
                    9 => "pipborr/djupmått",
                    10 => "Skjutmått",
                    11 => "Radieyra",
                    _ => string.Empty
                };
            }
        }
    }