using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class RINGAR_SV_TSD_3_Oljehal : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();
            string subject = req?.ProductDesignation ?? "";
            string machine = req?.MachineNumber ?? "";
            kv["VaLPopUp"] = ComputePopup(req?.Published);

            string tmpBet = subject.ToUpperInvariant().Trim().Replace(".", ",");
            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.' }, StringSplitOptions.RemoveEmptyEntries);
            double typ = tokens.Length >= 2 ? ToDouble(tokens[1]) : 0;

            bool tmpG = tmpBet.Contains("G");
            bool tmpGU = tmpBet.Contains("GU");
            bool tmpVZ = tmpBet.Contains("VZ861");
            bool tmpV23 = tmpBet.Contains("V23");
            bool tmpV21_2 = tmpBet.Contains("V21-2");
            bool tmpV21_3 = tmpBet.Contains("V21-3");

            double inD = Bookmark(req, "Ø D");
            double inD3 = Bookmark(req, "Ø D3");
            double inD4 = Bookmark(req, "Ø D4");
            double inD5 = Bookmark(req, "Ø D5");
            double inB = Bookmark(req, "Bredd (B)");
            double inB2 = Bookmark(req, "Bredd (B2)");
            double inB3 = Bookmark(req, "Bredd (B3)");
            double inB4 = Bookmark(req, "Bredd (B4)");
            double inBD = Bookmark(req, "Borrdjup (BD)");
            double inDR = Bookmark(req, "Hjälpmått (DR)");
            double inVBM = Bookmark(req, "Vinkel borrhål (BM)");

            var dList = DList(typ, tmpV21_2, tmpV21_3);
            var dgList = DGList(typ);
            var dvzList = DVZList(typ);

            double tmpManIn = inD3 + inD;
            double tmpD3 = inD3 == 0 ? (!tmpG && !tmpVZ ? dList.Item1 : !tmpVZ ? dgList.Item1 : dvzList.Item1) : inD3;
            kv["SumD3"] = tmpManIn == 0 ? "(D3) " + Fmt(tmpD3) : inD3 == 0 ? "Mata in (D3)" : "(D3) " + Fmt(tmpD3);
            kv["SumD3Tol"] = GeneralTol(tmpD3);

            double tmpDMinusD3 = !tmpG
                ? (Any(typ, 3044) ? 21 : Any(typ, 3260, 3076) ? 23 : Any(typ, 3040, 3144, 3138, 3148) ? 21.5 : Any(typ, 3160, 3064, 3164, 3172, 3176, 3276, 3184) ? 23 : 23.5)
                : (Any(typ, 3144, 3140) ? 21 : Any(typ, 3284, 3260) ? 23 : 0);
            double tmpD = inD == 0 ? tmpD3 + tmpDMinusD3 : inD;
            kv["SumD"] = tmpManIn == 0 ? "(D) " + Fmt(tmpD) : inD == 0 ? "Mata in (D)" : "(D) " + Fmt(tmpD);
            bool tmpDTol = !tmpGU && !tmpV23 ? Any(typ, 3040, 3044, 3048, 3056, 3064, 3138, 3144, 3148, 3164, 3172, 3260) : Any(typ, 3140);
            kv["SumDTol"] = tmpDTol ? "+ 0" : GeneralTol(tmpD);
            kv["SumDTolN"] = tmpDTol ? H12Negative(tmpD) : "";

            double tmpD1 = Any(typ, 3044) || (Any(typ, 3144, 3140) && tmpG) ? tmpD - 2 : tmpD;
            kv["SumD1"] = "(D1) " + Fmt(tmpD1);
            kv["SumD1Tol"] = GeneralTol(tmpD1);

            double tmpD2 = Any(typ, 3044) || (Any(typ, 3144, 3140) && tmpG) ? tmpD - 8 : tmpD - 9;
            kv["SumD2"] = "(D2) " + Fmt(tmpD2);
            kv["SumD2Tol"] = "+ 0 [2]";
            kv["SumD2TolN"] = H8Negative(tmpD2) + " [3]";

            double tmpD4 = inD4 == 0 ? (!tmpG && !tmpVZ ? dList.Item2 : !tmpVZ ? dgList.Item2 : dvzList.Item2) : inD4;
            kv["SumD4"] = "(D4) " + Fmt(tmpD4);
            kv["SumD4Tol"] = H12Positive(tmpD4);
            kv["SumD4TolN"] = "- 0";

            double tmpD5 = inD5 == 0 ? (tmpDMinusD3 == 21 ? tmpD3 + 6 : tmpDMinusD3 == 21.5 ? tmpD3 + 3.5 : tmpDMinusD3 == 23 ? tmpD3 + 5 : tmpDMinusD3 == 23.5 ? tmpD3 + 5.5 : 0) : inD5;
            kv["SumD5"] = "(D5) " + Fmt(tmpD5);
            bool tmpD5Tol = !tmpGU ? Any(typ, 3040, 3044, 3048, 3056, 3064, 3138, 3144, 3148, 3164, 3172, 3260) : Any(typ, 3140);
            kv["SumD5Tol"] = tmpD5Tol ? H12Positive(tmpD5) : "";
            kv["SumD5TolN"] = tmpD5Tol ? "- 0" : GeneralTol(tmpD5);

            double tmpB = inB == 0 ? ((typ == 3144 && tmpG) || typ == 3044 ? 22.5 : 26) : inB;
            kv["SumB"] = "(B) " + Fmt(tmpB);
            kv["SumBTol"] = GeneralTol(tmpB);

            double tmpB1 = 10;
            kv["SumB1"] = "(B1) " + Fmt(tmpB1);
            kv["SumB1Tol"] = "+ 0 [2]";
            kv["SumB1TolN"] = H8Negative(tmpB1) + " [3]";

            double tmpB2 = inB2 == 0 ? ((Any(typ, 3144, 3140) && tmpG) || typ == 3044 ? 6 : typ == 3148 ? 7.5 : 6.8) : inB2;
            kv["SumB2"] = "(B2) " + Fmt(tmpB2);
            kv["SumB2Tol"] = GeneralTol(tmpB2);

            double tmpB3 = inB3 == 0 ? ((typ == 3144 && tmpG) || typ == 3044 ? 9 : 11) : inB3;
            kv["SumB3"] = "(B3) " + Fmt(tmpB3);
            kv["SumB3Tol"] = GeneralTol(tmpB3);

            double tmpB4 = inB4 == 0 ? ((typ == 3144 && tmpG) || typ == 3044 ? 8.5 : 9.8) : inB4;
            kv["SumB4"] = "(B4) " + Fmt(tmpB4);
            kv["SumB4Tol"] = GeneralTol(tmpB4);

            double tmpBM = 5;
            kv["SumBM"] = "(BM) " + Fmt(tmpBM);
            kv["SumBMTol"] = tmpBM < 3.1 ? "+ 0.100" : tmpBM < 6.1 ? "+ 0.120" : "+ 0.150";
            kv["SumBMTolN"] = "- 0";

            double tmpBD = (typ == 3144 && tmpG) || typ == 3044 ? 5 : 6;
            kv["SumBD"] = inBD == 0 ? "(BD) " + Fmt(tmpBD) : Fmt(inBD);
            kv["SumBDTol"] = GeneralTol(tmpBD);

            double tmpA = tmpB1 / 2;
            kv["SumA"] = "(BD1) " + Fmt(tmpA);
            kv["SumATol"] = GeneralTol(tmpA);

            double tmpDOB = Any(typ, 3284, 3276, 3184, 3076) ? 12 : Any(typ, 3164, 3040, 3138, 3156, 3060, 3160, 3172, 3176, 3064, 3260) ? 10 : 8;
            kv["SumDOB"] = "(DOB) " + Fmt(tmpDOB);
            kv["SumDOBText"] = "";

            double tmpR = (tmpD5 - tmpDOB) / 2;
            double tmpDR = inDR == 0 ? (tmpD / 2) - tmpR : inDR;
            kv["SumDR"] = "(DR) " + Fmt(tmpDR);

            double tmpVOB = Any(typ, 3036, 3134) ? 6 : Any(typ, 3040, 3138, 3156, 3060, 3160) ? 14 : 5;
            double tmpKonst = Math.Round(Math.Sin((tmpVOB / 2) * Math.PI / 180) * 2, 4, MidpointRounding.AwayFromZero);
            double tmpAOB = tmpVOB > 10 ? tmpVOB - tmpDOB : Math.Round((tmpKonst * tmpR) - tmpDOB, 1, MidpointRounding.AwayFromZero);
            kv["SumHM"] = "Hjälpmått " + Fmt(tmpAOB);

            double tmpM = tmpB - tmpB1 - tmpB2;
            kv["SumM"] = "(M) " + Fmt(tmpM);
            kv["SumMTol"] = tmpM < 6 ? "± 0.1" : tmpM < 30 ? "± 0.2" : tmpM < 120 ? "± 0.3" : "± 0.5";

            kv["SumV45"] = "45º";
            kv["SumV45_2"] = "45º";
            kv["SumF45"] = "(2x) 1x45º";

            double tmpVBM = inVBM == 0 ? Math.Round((Math.Atan2((tmpBM / 2) + 0.3, (tmpD / 2) - tmpBD) * 180) / Math.PI, 1, MidpointRounding.AwayFromZero) : inVBM;
            kv["SumV15"] = tmpVBM.ToString("0.0", CultureInfo.InvariantCulture) + "º";

            double tmpKonstKorda = Math.Round(Math.Sin(((90 - tmpVBM) / 2) * Math.PI / 180) * 2, 4, MidpointRounding.AwayFromZero);
            double tmpKordaBM = Math.Round((tmpD / 2) * tmpKonstKorda, 1, MidpointRounding.AwayFromZero);
            kv["SumKordaBM"] = "Korda kant till kant " + Fmt(tmpKordaBM);

            string tmpRa = "6.3";
            kv["SumRit"] = tmpBet;
            kv["SumRit2"] = tmpBet;

            bool machineValid = IsMachineValid(machine);
            kv["SumMaskinValS1"] = "Maskin: Svarvning - " + MachineNameS1(machine) + (machineValid ? " " : "");
            kv["SumMaskinValS2"] = "Maskin: Borrning - " + MachineNameS2(machine) + (machineValid ? " " : "");

            kv["SumF1_1"] = machineValid ? "1/1" : "";
            kv["SumF1_2"] = machineValid ? "1/1" : "";
            kv["SumF1_3"] = machineValid ? "1/3" : "";
            kv["SumF1_4"] = machineValid ? "1/3" : "";
            kv["SumF1_5"] = machineValid ? "1/5" : "";
            kv["SumF1_6"] = machineValid ? "Inst." : "";
            kv["SumF1_7"] = machineValid ? "Inst." : "";
            kv["SumF1_8"] = machineValid ? "1/3" : "";
            kv["SumF1_9"] = machineValid ? "1/3" : "";

            bool nakamuraOrLb45 = EqualsAny(machine, "Nakamura", "LB45");
            bool maxMullerOrSkepp = EqualsAny(machine, "MaxMuller/Skepp6", "MaxMuller", "Skepp6");
            kv["SumF2_1"] = nakamuraOrLb45 ? "1/5" : maxMullerOrSkepp ? "1/1" : "";
            kv["SumF2_2"] = nakamuraOrLb45 ? "1/5" : maxMullerOrSkepp ? "1/1" : "";
            kv["SumF2_3"] = nakamuraOrLb45 ? "1/5" : maxMullerOrSkepp ? "1/1" : "";
            kv["SumF2_4"] = nakamuraOrLb45 ? "1/5" : maxMullerOrSkepp ? "1/1" : "";
            kv["SumF2_5"] = nakamuraOrLb45 ? "Inst." : maxMullerOrSkepp ? "1/1" : "";

            kv["SumD1_1"] = machineValid ? "Mikrometer" : "";
            kv["SumD1_2"] = machineValid ? "Mikrometer" : "";
            kv["SumD1_3"] = machineValid ? "Digitalt Djup/Hakmått" : "";
            kv["SumD1_4"] = machineValid ? "Digitalt Djup/Hakmått" : "";
            kv["SumD1_5"] = machineValid ? "Skjutmått" : "";
            kv["SumD1_6"] = machineValid ? "Vinkelsystem" : "";
            kv["SumD1_7"] = machineValid ? "Vinkelsystem" : "";
            kv["SumD1_8"] = machineValid ? "Ytjämnhetsmätare" : "";
            kv["SumD1_9"] = machineValid ? "Skjutmått" : "";
            kv["SumD2_1"] = machineValid ? "Skjutmått" : "";
            kv["SumD2_2"] = machineValid ? "Skjutmått" : "";
            kv["SumD2_3"] = machineValid ? "Skjutmått" : "";
            kv["SumD2_4"] = machineValid ? "Skjutmått" : "";
            kv["SumD2_5"] = machineValid ? "Skjutmått" : "";

            for (int i = 1; i <= 9; i++) kv["SumAF1_" + i] = machineValid && i == 8 ? "Bearbetas Ra " + tmpRa + " runt om" : "";
            for (int i = 1; i <= 5; i++) kv["SumAF2_" + i] = "";

            kv["SumTextS1"] = "Okulärkontroll Grader, frifläcker, slagmärken, repor, valkar, ytjämnhet och faser, skarpa kanter avgradas.";
            kv["SumTextS2"] = kv["SumTextS1"];
            return kv;
        }

        private static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published)) return "";
            if (!DateTime.TryParse(published, out var dt)) return "";
            var until = dt.AddDays(14);
            return DateTime.Today <= until.Date ? "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)" + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + "Information om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument" + Environment.NewLine + Environment.NewLine + "Popupruta aktiv till " + until.ToString("yyyy-MM-dd") : "";
        }

        private static double Bookmark(APIRequest req, string name)
        {
            if (req?.Bookmarks == null) return 0;
            foreach (var item in req.Bookmarks)
                if (string.Equals(item.BookmarkName ?? "", name, StringComparison.OrdinalIgnoreCase)) return ToDouble(item.BookmarkValue);
            return 0;
        }

        private static Tuple<double, double> DList(double typ, bool v212, bool v213)
        {
            if (Any(typ, 3276, 3184)) return Tuple.Create(477.0, 445.0);
            if (typ == 3036) return Tuple.Create(202.5, 184.5);
            if (typ == 3038) return Tuple.Create(219.5, 205.0);
            if (typ == 3044) return Tuple.Create(258.0, 224.5);
            if (typ == 3136) return Tuple.Create(219.5, 194.5);
            if (typ == 3140) return Tuple.Create(247.5, 217.5);
            if (typ == 3134) return Tuple.Create(202.5, 180.5);
            if (typ == 3144) return Tuple.Create(273.5, 231.5);
            if (typ == 3148) return Tuple.Create(293.5, 251.5);
            if (typ == 3164) return Tuple.Create(377.0, 347.0);
            if (typ == 3172) return Tuple.Create(437.0, 415.5);
            if (typ == 3176) return Tuple.Create(477.0, 445.0);
            if (Any(typ, 3040, 3138)) return Tuple.Create(243.5, 212.5);
            if (typ == 3056 && v212) return Tuple.Create(336.5, 306.5);
            if (typ == 3056 && v213) return Tuple.Create(336.5, 316.5);
            if (typ == 3152) return Tuple.Create(316.5, 286.5);
            if (typ == 3156) return Tuple.Create(336.5, 306.5);
            if (typ == 3060) return Tuple.Create(356.5, 326.5);
            if (typ == 3160) return Tuple.Create(357.0, 327.0);
            if (typ == 3064) return Tuple.Create(362.0, 334.0);
            if (typ == 3260) return Tuple.Create(377.0, 347.0);
            return Tuple.Create(0.0, 0.0);
        }

        private static Tuple<double, double> DGList(double typ)
        {
            if (typ == 3140) return Tuple.Create(258.0, 240.5);
            if (typ == 3144) return Tuple.Create(278.0, 260.5);
            if (typ == 3284) return Tuple.Create(537.0, 506.0);
            return Tuple.Create(0.0, 0.0);
        }

        private static Tuple<double, double> DVZList(double typ)
        {
            if (typ == 3244) return Tuple.Create(291.5, 264.5);
            return Tuple.Create(0.0, 0.0);
        }

        private static string GeneralTol(double v) { if (v < 6) return "± 0.1"; if (v < 30) return "± 0.2"; if (v < 120) return "± 0.3"; if (v < 400) return "± 0.5"; if (v < 1000) return "± 0.8"; if (v < 2000) return "± 1.2"; return "± 2.0"; }
        private static string H12Positive(double v) { if (v < 3.1) return "+ 0.100"; if (v < 6.1) return "+ 0.120"; if (v < 10.1) return "+ 0.150"; if (v < 18.1) return "+ 0.180"; if (v < 30.1) return "+ 0.210"; if (v < 50.1) return "+ 0.250"; if (v < 80.1) return "+ 0.300"; if (v < 120.1) return "+ 0.350"; if (v < 180.1) return "+ 0.400"; if (v < 250.1) return "+ 0.460"; if (v < 315.1) return "+ 0.520"; if (v < 400.1) return "+ 0.570"; if (v < 500.1) return "+ 0.630"; return "+ 0.700"; }
        private static string H12Negative(double v) { if (v < 18.1) return "- 0.180"; if (v < 30.1) return "- 0.210"; if (v < 50.1) return "- 0.250"; if (v < 80.1) return "- 0.300"; if (v < 120.1) return "- 0.350"; if (v < 180.1) return "- 0.400"; if (v < 250.1) return "- 0.460"; if (v < 315.1) return "- 0.520"; if (v < 400.1) return "- 0.570"; if (v < 500.1) return "- 0.630"; return "- 0.700"; }
        private static string H8Negative(double v) { if (v < 3.1) return "- 0.014"; if (v < 6.1) return "- 0.018"; if (v < 10.1) return "- 0.022"; if (v < 18.1) return "- 0.027"; if (v < 30.1) return "- 0.033"; if (v < 50.1) return "- 0.039"; if (v < 80.1) return "- 0.046"; if (v < 120.1) return "- 0.054"; if (v < 180.1) return "- 0.063"; if (v < 250.1) return "- 0.072"; if (v < 315.1) return "- 0.081"; if (v < 400.1) return "- 0.089"; if (v < 500.1) return "- 0.097"; return "- 0.110"; }

        private static bool Any(double value, params double[] values) { foreach (double item in values) if (Math.Abs(value - item) < 0.000001) return true; return false; }
        private static bool EqualsAny(string value, params string[] values) { foreach (string item in values) if (string.Equals(value ?? "", item, StringComparison.OrdinalIgnoreCase)) return true; return false; }
        private static bool IsMachineValid(string machine) { return EqualsAny(machine, "Nakamura", "MaxMuller/Skepp6", "MaxMuller", "Skepp6", "LB45"); }
        private static string MachineNameS1(string machine) { if (EqualsAny(machine, "Nakamura")) return "Nakamura"; if (EqualsAny(machine, "MaxMuller/Skepp6", "MaxMuller", "Skepp6")) return "MaxMuller"; if (EqualsAny(machine, "LB45")) return "LB45"; return ""; }
        private static string MachineNameS2(string machine) { if (EqualsAny(machine, "Nakamura")) return "Nakamura"; if (EqualsAny(machine, "MaxMuller/Skepp6", "MaxMuller", "Skepp6")) return "Skepp 6"; if (EqualsAny(machine, "LB45")) return "LB45"; return ""; }
        private static double ToDouble(string value) { double.TryParse((value ?? "").Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out var result); return result; }
        private static string Fmt(double value) { return value.ToString("0.################", CommonFunctions.Culture); }
    }
}
