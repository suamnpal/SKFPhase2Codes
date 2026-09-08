using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class RINGAR_RG : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string mv = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;
            List<Bookmark> bm = req != null ? req.Bookmarks : null;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = subject.ToUpperInvariant().Trim();
            string tmpBet0 = tmpFormat.Replace(".", ",");
            bool tmpEC = tmpBet0.IndexOf("EC", StringComparison.Ordinal) >= 0;
            bool tmp72H = tmpBet0.IndexOf("72H", StringComparison.Ordinal) >= 0;

            string tmpBet = (tmp72H && tmpBet0.Length > 0) ? Left(tmpBet0, tmpBet0.Length - 1) : tmpBet0;

            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : "";
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : "";
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : "";
            string tmpBet4 = tokens.Length > 3 ? tokens[3] : "";

            int cntB2 = tmpBet2.Length;
            int cntB3 = tmpBet3.Length;

            bool xx = EqualsI(tmpBet1, "XX");

            string tmpArt = xx ? tmpBet2 : tmpBet1;
            kv["SumArt"] = tmpArt;

            string tmpSerie = xx ? Left(tmpBet3, 3) : Left(tmpBet2, 3);
            kv["SumSerie"] = tmpSerie;

            string tmpTyp = xx
                ? (cntB3 > 4 ? Right(tmpBet3, 2) : tmpBet4)
                : (cntB2 > 4 ? Right(tmpBet2, 2) : tmpBet3);
            kv["SumTyp"] = tmpTyp;

            double typNum = TryParseDouble(tmpTyp);

            kv["SumPrRit"] = "Produktritning: Windchill.skf.net - " + tmpBet + " ";
            kv["SumTolRit"] = " Toleransritning: Windchill.skf.net - parameters";

            bool hardring = EqualsI(GetString(bm, "Härdring").Trim(), "1");

            string tmpD10 = GetString(bm, "Ytterdiameter (Dg)", "Ytterdiameter", "YtterdiameterDg");
            string[] d10Parts = tmpD10.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            double tmpD10Rm = d10Parts.Length > 0 ? TryParseDouble(d10Parts[0]) : 0;
            double tmpD10Hm = (hardring && d10Parts.Length > 1) ? TryParseDouble(d10Parts[1]) : 0;
            double tmpD10a = hardring ? tmpD10Hm : tmpD10Rm;

            kv["SumD10"] = "(Dg) " + FmtComma(tmpD10a);
            kv["SumD10eh"] = hardring ? "(Dg) " + FmtComma(tmpD10Rm) : "";
            kv["SumD10Tol"] = "+ 0";

            double tmpTolND10 = tmpD10a;
            double d10TolNRmVal = tmp72H ? 0.254
                : tmpTolND10 < 50.01 ? 0.062
                : tmpTolND10 < 80.01 ? 0.074
                : tmpTolND10 < 120.01 ? 0.087
                : tmpTolND10 < 180.01 ? 0.1
                : tmpTolND10 < 250.01 ? 0.115
                : tmpTolND10 < 315.01 ? 0.13
                : tmpTolND10 < 400.01 ? 0.14
                : tmpTolND10 < 500.01 ? 0.15
                : tmpTolND10 < 630.01 ? 0.17
                : tmpTolND10 < 800.01 ? 0.19
                : tmpTolND10 < 1000.01 ? 0.21
                : tmpTolND10 < 1250.01 ? 0.23
                : tmpTolND10 < 1600.01 ? 0.26
                : 0.29;
            string tmpD10TolNRm = Fmt3(d10TolNRmVal);
            string tmpD10TolN = Fmt3(hardring ? GetDouble(bm, "Härd MinTolerans (Dg)", "HärdMinToleransDg") : d10TolNRmVal);

            kv["SumD10TolN"] = "- " + " " + tmpD10TolN;
            kv["SumD10Toleh"] = hardring ? "+ 0" : "";
            kv["SumD10TolNeh"] = hardring ? "- " + " " + tmpD10TolNRm : "";

            bool tmpSpecTol = ContainsAny(tmpBet,
                "RG-241/560 EC/H5VX224",
                "RG-241/600 ECA/H5VX224",
                "RG-231/530 CA/H5VX224",
                "RG-232/500 CA/H5VX224",
                "RG-232/530 CA/H5VX224",
                "RG-232/560 CA/H5VX224",
                "RG-231/560 CA/VX224");

            string tmpdg = GetString(bm, "Innerdiameter (dg)", "Innerdiameter", "Innerdiameterdg");
            string[] dgParts = tmpdg.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            double[] dgValues = new double[dgParts.Length];
            for (int i = 0; i < dgParts.Length; i++) dgValues[i] = TryParseDouble(dgParts[i]);
            double tmpdgRm = dgParts.Length > 0 ? TryParseDouble(dgParts[0]) : 0;
            double tmpdgHm = dgParts.Length > 1 ? TryParseDouble(dgParts[1]) : 0;

            double tmpTolNdg = Math.Round(hardring ? tmpdgHm : tmpdgRm, 2, MidpointRounding.AwayFromZero);

            double tmpdgTol = tmp72H
                ? (IsMember(tmpSerie, "232", "231")
                    ? (typNum < 48 ? 0.127 : 0.254)
                    : (typNum < 60 ? 0.127 : 0.254))
                : tmpSpecTol ? 0.820
                : tmpTolNdg < 50.01 ? 0.1
                : tmpTolNdg < 80.01 ? 0.12
                : tmpTolNdg < 120.01 ? 0.15
                : tmpTolNdg < 180.01 ? 0.18
                : tmpTolNdg < 250.01 ? 0.25
                : tmpTolNdg < 315.01 ? 0.31
                : tmpTolNdg < 400.01 ? 0.34
                : tmpTolNdg < 500.01 ? 0.46
                : tmpTolNdg < 630.01 ? 0.51
                : tmpTolNdg < 800.01 ? 0.63
                : tmpTolNdg < 1000.01 ? 0.81
                : tmpTolNdg < 1250.01 ? 0.98
                : tmpTolNdg < 1600.01 ? 1.20
                : 1.50;

            double tmpdgTolN = tmp72H ? 0
                : tmpSpecTol ? 0.700
                : tmpTolNdg < 50.01 ? 0.035
                : tmpTolNdg < 80.01 ? 0.045
                : tmpTolNdg < 120.01 ? 0.055
                : tmpTolNdg < 180.01 ? 0.08
                : tmpTolNdg < 250.01 ? 0.135
                : tmpTolNdg < 315.01 ? 0.18
                : tmpTolNdg < 400.01 ? 0.2
                : tmpTolNdg < 500.01 ? 0.31
                : tmpTolNdg < 630.01 ? 0.34
                : tmpTolNdg < 800.01 ? 0.44
                : tmpTolNdg < 1000.01 ? 0.60
                : tmpTolNdg < 1250.01 ? 0.75
                : tmpTolNdg < 1600.01 ? 0.90
                : 1.10;

            double tmpUtRdg = RoundTo(tmpdgRm + tmpdgTolN, 0.001);
            kv["Sumdg"] = "(dg) " + (hardring ? FmtComma(tmpdgHm) : FmtComma(tmpUtRdg));
            kv["Sumdgeh"] = hardring ? "(dg) " + FmtComma(tmpUtRdg) : "";

            double tmpUtRdgTolVal = hardring
                ? GetDouble(bm, "Härd MaxTolerans (dg)", "HärdMaxToleransdg")
                : Math.Round(tmpdgTol - tmpdgTolN, 3, MidpointRounding.AwayFromZero);
            kv["SumdgTol"] = "+ " + FmtGenDot(tmpUtRdgTolVal);
            kv["SumdgTolN"] = "+ 0";
            kv["SumdgToleh"] = hardring ? "+ " + Fmt3(Math.Round(tmpdgTol - tmpdgTolN, 3, MidpointRounding.AwayFromZero)) : "";
            kv["SumdgTolNeh"] = hardring ? "+ 0" : "";

            string tmpA30 = GetString(bm, "Fasvinkel (A30) i grader", "Fasvinkel (A30)", "Fasvinkel");
            kv["SumA30"] = "(A30) " + tmpA30 + "\u00ba";

            string sumA30Tol;
            if (tmp72H)
            {
                sumA30Tol = typNum == 48 ? "+ 3\u00b5m/mm"
                          : typNum == 52 ? "+ 2\u00b5m/mm"
                          : (EqualsI(tmpSerie, "230") && typNum == 56) ? "+ 1\u00b5m/mm"
                          : typNum == 56 ? "+ 2\u00b5m/mm"
                          : typNum == 60 ? "+ 2\u00b5m/mm"
                          : typNum == 64 ? "+ 3\u00b5m/mm"
                          : (typNum == 68 || typNum == 72 || typNum == 76) ? "+ 2\u00b5m/mm"
                          : "";
            }
            else
            {
                sumA30Tol = AnyLess(dgValues, 120.01) ? "+ 10\u00b5m/mm"
                          : AnyLess(dgValues, 250.01) ? "+ 8\u00b5m/mm"
                          : AnyLess(dgValues, 400.01) ? "+ 7\u00b5m/mm"
                          : "+ 5\u00b5m/mm";
            }
            kv["SumA30Tol"] = sumA30Tol;

            string sumA30TolN;
            if (tmp72H)
            {
                sumA30TolN = typNum == 48 ? "- 3\u00b5m/mm"
                           : typNum == 52 ? "- 2\u00b5m/mm"
                           : (EqualsI(tmpSerie, "230") && typNum == 56) ? "- 4\u00b5m/mm"
                           : typNum == 56 ? "- 2\u00b5m/mm"
                           : typNum == 60 ? "- 2\u00b5m/mm"
                           : typNum == 64 ? "- 3\u00b5m/mm"
                           : (typNum == 68 || typNum == 72 || typNum == 76) ? "- 2\u00b5m/mm"
                           : "";
            }
            else
            {
                sumA30TolN = AnyLess(dgValues, 80.01) ? "- 5\u00b5m/mm"
                           : AnyLess(dgValues, 120.01) ? "- 3\u00b5m/mm"
                           : AnyLess(dgValues, 250.01) ? "- 2\u00b5m/mm"
                           : "- 1\u00b5m/mm";
            }
            kv["SumA30TolN"] = sumA30TolN;

            string tmpML = GetString(bm, "Mätlängd Fasvinkel", "MätlängdFasvinkel") + " mm";

            string tmpBgTolNKey = Right(tmpSerie, 2);
            string tmpBg = GetString(bm, "Bredd (Bg)", "BreddBg");
            kv["SumBg"] = "(Bg) " + tmpBg;
            kv["SumBgTol"] = "+ 0";

            double bgTolNVal;
            if (tmp72H) bgTolNVal = 0.05;
            else if (IsMember(tmpBgTolNKey, "38", "48", "39", "49"))
            {
                bgTolNVal = tmpTolNdg < 80.01 ? 0.046
                          : tmpTolNdg < 120.01 ? 0.054
                          : tmpTolNdg < 180.01 ? 0.063
                          : tmpTolNdg < 250.01 ? 0.072
                          : tmpTolNdg < 315.01 ? 0.081
                          : tmpTolNdg < 400.01 ? 0.089
                          : tmpTolNdg < 500.01 ? 0.097
                          : tmpTolNdg < 630.01 ? 0.11
                          : tmpTolNdg < 800.01 ? 0.125
                          : tmpTolNdg < 1000.01 ? 0.14
                          : tmpTolNdg < 1250.01 ? 0.165
                          : tmpTolNdg < 1600.01 ? 0.195
                          : 0.23;
            }
            else
            {
                bgTolNVal = tmpTolNdg < 50.01 ? 0.062
                          : tmpTolNdg < 80.01 ? 0.074
                          : tmpTolNdg < 120.01 ? 0.087
                          : tmpTolNdg < 180.01 ? 0.1
                          : tmpTolNdg < 250.01 ? 0.115
                          : tmpTolNdg < 315.01 ? 0.13
                          : tmpTolNdg < 400.01 ? 0.14
                          : tmpTolNdg < 500.01 ? 0.155
                          : tmpTolNdg < 630.01 ? 0.175
                          : tmpTolNdg < 800.01 ? 0.2
                          : tmpTolNdg < 1000.01 ? 0.23
                          : tmpTolNdg < 1250.01 ? 0.26
                          : tmpTolNdg < 1600.01 ? 0.31
                          : 0.37;
            }
            kv["SumBgTolN"] = "- " + " " + Fmt3(bgTolNVal);

            string tmpBminStr = GetString(bm, "Bredd (Bmin)", "BreddBmin");
            double tmpBmin = TryParseDouble(tmpBminStr);
            bool hasBmin = tmpBmin != 0;
            kv["SumBmin"] = hasBmin ? "(Bmin) " + tmpBminStr : "";
            kv["SumBm1"] = hasBmin ? "Bredd min" : "";
            kv["SumBm2"] = hasBmin ? "Bmin" : "";
            string tmpBm3 = hasBmin ? "Vid Skärbyte" : "";
            string tmpBm4 = hasBmin ? "Mikrometer" : "";

            kv["TmpV3dg"] = "Max: " + (tmpTolNdg < 50.01 ? "0.035"
                          : tmpTolNdg < 80.01 ? "0.045"
                          : tmpTolNdg < 120.01 ? "0.055"
                          : tmpTolNdg < 180.01 ? "0.065"
                          : tmpTolNdg < 250.01 ? "0.070"
                          : tmpTolNdg < 315.01 ? "0.080"
                          : tmpTolNdg < 400.01 ? "0.090"
                          : tmpTolNdg < 500.01 ? "0.100"
                          : tmpTolNdg < 630.01 ? "0.110"
                          : tmpTolNdg < 800.01 ? "0.120"
                          : tmpTolNdg < 1000.01 ? "0.130"
                          : tmpTolNdg < 1250.01 ? "0.150"
                          : tmpTolNdg < 1600.01 ? "0.170"
                          : "0.200");

            kv["TmpSpg"] = "Max: " + (tmpTolNdg < 50.01 ? "0.062"
                         : tmpTolNdg < 80.01 ? "0.074"
                         : tmpTolNdg < 120.01 ? "0.087"
                         : tmpTolNdg < 180.01 ? "0.100"
                         : tmpTolNdg < 250.01 ? "0.115"
                         : tmpTolNdg < 315.01 ? "0.130"
                         : tmpTolNdg < 400.01 ? "0.140"
                         : tmpTolNdg < 500.01 ? "0.155"
                         : tmpTolNdg < 630.01 ? "0.175"
                         : tmpTolNdg < 800.01 ? "0.200"
                         : tmpTolNdg < 1000.01 ? "0.230"
                         : tmpTolNdg < 1250.01 ? "0.260"
                         : tmpTolNdg < 1600.01 ? "0.310"
                         : "0.370");

            string anmYta = GetString(bm, "Anmärkning Yta", "AnmärkningYta");
            bool ytaEtt = IsMember(anmYta.Trim(), "1", "1,0", "1.0");
            bool ecSpec = tmpEC && EqualsI(tmpBet2, "240") && IsMember(tmpBet3, "600", "630");

            kv["SumYtD10"] = ytaEtt ? "1.0" : tmp72H ? "4" : ecSpec ? "1.6" : "2.5";
            kv["SumYtD01"] = ytaEtt ? "1.0" : tmp72H ? "2.5" : ecSpec ? "1.6" : "4";
            kv["SumYtBg"] = ytaEtt ? "1.0" : ecSpec ? "1.6" : "2.5";

            bool tmpAllMsk = IsMember(mv.Trim(),
                "Okuma LB 4000 4515",
                "Okuma LB4000MY 4555",
                "Okuma LU45 4317",
                "Okuma LU35 4454",
                "Okuma LB3000 4580",
                "Okuma LB3000 4636");

            kv["SumMaskinValS1"] = "Maskin: " + mv;

            kv["SumF1_1"] = tmpAllMsk ? "1/5" : "";
            kv["SumF1_2"] = tmpAllMsk ? "1/5" : "";
            kv["SumF1_3"] = tmpAllMsk ? "1/20" : "";
            kv["SumF1_4"] = tmpAllMsk ? tmpBm3 : "";
            kv["SumF1_5"] = tmpAllMsk ? "1/20" : "";
            kv["SumF1_6"] = "";
            kv["SumF1_7"] = "";
            kv["SumF1_8"] = tmpAllMsk ? "Inst." : "";
            kv["SumF1_9"] = "";
            kv["SumF1_0"] = "";

            kv["SumD1_1"] = EqualsI(mv.Trim(), "Okuma LB4000MY 4555") ? "UD-Apparat/Mätplatta" : "Mätplatta";
            kv["SumD1_2"] = (EqualsI(mv.Trim(), "Okuma LB3000 4636") && AnyLess(dgValues, 300)) ? "Mätstation 1" : "UD-Apparat/Mätplatta";
            kv["SumD1_3"] = tmpAllMsk ? "UD-Apparat" : "";
            kv["SumD1_4"] = tmpAllMsk ? tmpBm4 : "";
            kv["SumD1_5"] = tmpAllMsk ? "Mätplatta" : "";
            kv["SumD1_6"] = "";
            kv["SumD1_7"] = "";
            kv["SumD1_8"] = tmpAllMsk ? "Ytjämnhetsmätare" : "";
            kv["SumD1_9"] = "";
            kv["SumD1_0"] = "";

            string anmDg = GetString(bm, "Anmärkning (Dg)");
            string anmdg = GetString(bm, "Anmärkning (dg)");
            string anmExtra = GetString(bm, "Anmärkning extra", "Anmärkningextra");

            kv["SumAF1_1"] = tmpAllMsk ? (EqualsI(anmDg.Trim(), "0") ? "" : anmDg) : "";
            kv["SumAF1_2"] = (tmpAllMsk ? (EqualsI(anmdg.Trim(), "0") ? "" : anmdg) : "") + ", Mät första & sista på ämnet";
            kv["SumAF1_3"] = "";
            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = tmpAllMsk ? tmpML : "";
            kv["SumAF1_6"] = "";
            kv["SumAF1_7"] = "";
            kv["SumAF1_8"] = (EqualsI(anmYta.Trim(), "1") ? "" : EqualsI(anmYta.Trim(), "0") ? "" : anmYta) + ", Mät vid misstänkt ytfel";
            kv["SumAF1_9"] = "";
            kv["SumAF1_0"] = "";

            kv["SumMeh"] = hardring ? "Ritningsmått efter härdning:" : "";
            kv["SumAnmExtra"] = "Mätning enligt RG5 i styrplan.<<LineBreak>>" + Environment.NewLine
                + "Där tolk finnes skall 100% mätning utföras med tebo-tolk, ringen ska gå lätt över tolk.<<LineBreak>>" + Environment.NewLine
                + (EqualsI(anmExtra.Trim(), "0") ? "" : anmExtra);
            kv["SumTextS1"] = "Mätutrustningen kontrolleras minst 1 gång/tim mot passbitsklove alt. Inställd ring.<<LineBreak>>" + Environment.NewLine
                + "Vid inställning kontrolleras att samtliga mått ligger lika, alla ringar på ämnet.<<LineBreak>>" + Environment.NewLine
                + "Okulärkontroll utföres avseende ytdefekter/materialfel, slagmärken och grader.";

            return kv;
        }

        private static string ComputePopup(string pub)
        {
            if (string.IsNullOrWhiteSpace(pub)) return "";
            DateTime dt; if (!DateTime.TryParse(pub, out dt)) return "";
            DateTime till = dt.AddDays(14);
            return DateTime.Today <= till.Date
                ? "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)<<LineBreak>>"
                  + "Om mätdonsbeställning görs så är minutavgränsningen (skiljetecknet) numera med komma INTE kolon<<LineBreak>>"
                  + "Information om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument<<LineBreak>>"
                  + "Popupruta aktiv till " + till.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                : "";
        }

        private static string Left(string s, int n) =>
            string.IsNullOrEmpty(s) ? "" : (n <= 0 ? "" : (s.Length <= n ? s : s.Substring(0, n)));

        private static string Right(string s, int n) =>
            string.IsNullOrEmpty(s) ? "" : (s.Length <= n ? s : s.Substring(s.Length - n));

        private static bool IsMember(string val, params string[] list)
        {
            if (list == null) return false;
            for (int i = 0; i < list.Length; i++)
                if (string.Equals(list[i], val ?? "", StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        private static bool ContainsAny(string src, params string[] list)
        {
            if (string.IsNullOrEmpty(src) || list == null) return false;
            for (int i = 0; i < list.Length; i++)
                if (!string.IsNullOrEmpty(list[i]) && src.IndexOf(list[i], StringComparison.OrdinalIgnoreCase) >= 0) return true;
            return false;
        }

        private static bool AnyLess(double[] values, double limit)
        {
            if (values == null || values.Length == 0) return 0 < limit;
            for (int i = 0; i < values.Length; i++)
                if (values[i] < limit) return true;
            return false;
        }

        private static bool EqualsI(string a, string b) =>
            string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);

        private static double RoundTo(double v, double unit)
        {
            if (unit == 0) return v;
            return Math.Round(v / unit, MidpointRounding.AwayFromZero) * unit;
        }

        private static double TryParseDouble(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;
            double v;
            return double.TryParse(s.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static double GetDouble(List<Bookmark> bm, params string[] keys)
        {
            return TryParseDouble(GetString(bm, keys));
        }

        private static string GetString(List<Bookmark> bm, params string[] keys)
        {
            string exact = Lookup(bm, keys, StringComparison.Ordinal);
            if (!string.IsNullOrWhiteSpace(exact)) return exact;
            return Lookup(bm, keys, StringComparison.OrdinalIgnoreCase);
        }

        private static string Lookup(List<Bookmark> bm, string[] keys, StringComparison cmp)
        {
            if (bm == null || keys == null) return "";
            for (int k = 0; k < keys.Length; k++)
            {
                string key = keys[k];
                if (string.IsNullOrWhiteSpace(key)) continue;
                for (int i = 0; i < bm.Count; i++)
                {
                    Bookmark b = bm[i];
                    if (b != null && string.Equals((b.BookmarkName ?? "").Trim(), key, cmp))
                    {
                        string val = b.BookmarkValue ?? "";
                        if (!string.IsNullOrWhiteSpace(val)) return val;
                    }
                }
            }
            return "";
        }

        private static string FmtComma(double v) =>
            v.ToString("0.################", CommonFunctions.Culture).Replace(".", ",");

        private static string FmtGenDot(double v) =>
            v.ToString("0.################", CommonFunctions.Culture).Replace(",", ".");

        private static string Fmt3(double v) =>
            v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
    }
}