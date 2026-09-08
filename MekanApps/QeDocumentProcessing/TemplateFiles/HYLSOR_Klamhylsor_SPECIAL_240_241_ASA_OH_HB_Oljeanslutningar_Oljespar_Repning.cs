using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Klamhylsor_SPECIAL_240_241_ASA_OH_HB_Oljeanslutningar_Oljespar_Repning : ITemplateCalculations
    {
        private static readonly string[] Machines = new[] { "Skepp6", "K&T", "VTR-160", "MacTurn 550" };
        private static readonly string[] MachinesKAT = new[] { "K&T", "VTR-160", "MacTurn 550" };

        private static readonly string[] Typ240 = { "750", "900" };
        private static readonly string[] Typ241 = { "68", "600", "630", "710" };
        private static readonly string[] TypASA = { "24" };

        private static readonly double[] B240 = { 8, 10 };
        private static readonly double[] B241 = { 3.5, 8, 6, 8 };
        private static readonly double[] BASA = { 4 };

        private static readonly double[] E240 = { 246, 274 };
        private static readonly double[] E241 = { 169, 259, 280, 308 };
        private static readonly double[] EASA = { 183 };

        private static readonly double[] J240 = { 241, 269 };
        private static readonly double[] J241 = { 164, 254, 275, 303 };
        private static readonly double[] JASA = { 178 };

        private static readonly double[] K240 = { 168, 188 };
        private static readonly double[] K241 = { 121.5, 187.5, 200, 219 };
        private static readonly double[] KASA = { 140 };

        private static readonly int[] AR240 = { 22, 27 };
        private static readonly int[] AR241 = { 10, 18, 19, 21 };
        private static readonly int[] ARASA = { 13 };

        private static readonly double[] K1_240 = { 175, 201 };
        private static readonly double[] K1_241 = { 111.5, 171, 185, 205 };
        private static readonly double[] K1_ASA = { 117 };

        private const double TmpKona = 30.0;
        private const int TmpSV = 12;
        private const int TmpVOS = 10;

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;
            List<Bookmark> bm = req != null ? req.Bookmarks : null;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");

            bool tmpSlash = tmpBet.IndexOf('/') >= 0;
            bool tmpOH = tmpBet.IndexOf("OH", StringComparison.OrdinalIgnoreCase) >= 0;
            bool tmpASA = tmpBet.IndexOf("ASA", StringComparison.OrdinalIgnoreCase) >= 0;

            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : string.Empty;
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : string.Empty;
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : string.Empty;
            string tmpBet4 = tokens.Length > 3 ? tokens[3] : string.Empty;
            string tmpBet5 = tokens.Length > 4 ? tokens[4] : string.Empty;

            bool tmpCountT1 = ContainsToken("HB", tokens);
            bool tmpCountT2 = ContainsToken("V29", tokens);
            bool tmpCountT3 = ContainsToken("H", tokens);
            bool tmpSpecDia = EqualsI(tmpBet5, "HB");

            int cntB2 = tmpBet2.Length;
            string tmpSerie;
            if (tmpSlash && (cntB2 == 3 || cntB2 == 2))
                tmpSerie = tmpBet2;
            else if (cntB2 > 4)
                tmpSerie = tmpBet2.Substring(0, 3);
            else if (cntB2 == 3)
                tmpSerie = tmpBet2.Substring(0, 1);
            else
                tmpSerie = tmpBet2.Length >= 2 ? tmpBet2.Substring(0, 2) : tmpBet2;

            string tmpTypStr = tmpSlash
                ? (tmpSpecDia ? tmpBet4 : tmpBet3)
                : (tmpBet2.Length >= 2 ? tmpBet2.Substring(tmpBet2.Length - 2) : tmpBet2);

            int serieInt = TryParseInt(tmpSerie);
            double typNum = TryParseDouble(tmpTypStr);

            int typLista = serieInt == 240 ? GetMember(tmpTypStr, Typ240)
                         : serieInt == 241 ? GetMember(tmpTypStr, Typ241)
                         : GetMember(tmpTypStr, TypASA);

            string ritBm = GetString(bm, "Ritningsnummer");
            bool ritZero = string.IsNullOrEmpty(ritBm) || EqualsI(ritBm, "0");
            string tmpRitBase;
            if (ritZero)
            {
                if (tmpASA) tmpRitBase = tmpBet;
                else if (serieInt == 241) tmpRitBase = "7432909, 7432903";
                else if (serieInt == 240) tmpRitBase = "7432908, 7432901";
                else tmpRitBase = "";
            }
            else
            {
                tmpRitBase = ritBm;
            }
            kv["SumRitNr"] = tmpRitBase + ":senaste utg.";
            kv["SumRitNrS2"] = kv["SumRitNr"];

            double tmpSlitsVal = GetDouble(bm, "Slits");
            if (tmpSlitsVal == 0) tmpSlitsVal = typNum < 600 ? 8 : 10;

            string gBm = GetString(bm, "Gänga oljeborrhål");
            string tmpG = BuildG(serieInt, typNum, gBm);
            kv["SumG"] = tmpG;

            double tmpBval = GetDouble(bm, "Mått till borrhål (B)");
            if (tmpBval == 0)
            {
                double[] blist = serieInt == 240 ? B240 : serieInt == 241 ? B241 : BASA;
                tmpBval = GetVal(blist, typLista);
            }
            string tmpBTol = tmpASA ? Fmt1(0.1) : Fmt1(0.0);
            string tmpBTolN = tmpASA ? Fmt1(0.3) : Fmt3(0.1);
            kv["SumB"] = "(B) " + Fmt(tmpBval);
            kv["SumBTol"] = "+ " + tmpBTol;
            kv["SumBTolN"] = "- " + tmpBTolN;

            double tmpC = C_val(tmpG);
            kv["SumC"] = "(C) " + (tmpC == 0 ? "ingen gänga angiven" : Fmt(tmpC));
            kv["SumCTol"] = GenTol(tmpC);

            string tBm = GetString(bm, "Borrgänghålfasdiameter (T)");
            bool tNone = EqualsI(tBm, "none");
            double tmpT = tNone || string.IsNullOrEmpty(tBm) || EqualsI(tBm, "0") ? T_default(tmpG) : TryParseDouble(tBm);
            kv["SumT"] = tNone ? "ej angivet" : "(T) " + Fmt(tmpT);
            kv["SumTTol"] = tNone ? "" : GenTol(tmpT);

            string tmpDstr = D_val(tmpG);
            kv["SumD"] = "(D) " + tmpDstr;
            kv["SumDTol"] = "± 0.1";

            kv["SumH"] = "(H) " + H_val(serieInt, typNum);
            kv["SumHTol"] = "± 0.1";

            double tmpEval = GetDouble(bm, "Oljeborrhålslängd (E)");
            if (tmpEval == 0)
            {
                double[] elist = serieInt == 240 ? E240 : serieInt == 241 ? E241 : EASA;
                tmpEval = GetVal(elist, typLista);
            }
            kv["SumE"] = "(E) " + Fmt(tmpEval);
            kv["SumETol"] = GenTol(tmpEval);

            double tmpJval = GetDouble(bm, "Längd till oljespår (J)");
            if (tmpJval == 0)
            {
                double[] jlist = serieInt == 240 ? J240 : serieInt == 241 ? J241 : JASA;
                tmpJval = GetVal(jlist, typLista);
            }
            kv["SumJ"] = "(J) " + Fmt(tmpJval);
            kv["SumJS2"] = kv["SumJ"];
            kv["SumJTol"] = GenTol(tmpJval);
            kv["SumJS2Tol"] = kv["SumJTol"];

            double tmpFval = F_val(tmpASA, typNum);
            kv["SumF"] = "(F) " + Fmt(tmpFval);
            kv["SumF2"] = kv["SumF"];
            kv["SumFTol"] = "± 0.1";
            kv["SumF2Tol"] = "± 0.1";

            double tmpNval = N_val(tmpASA, typNum);
            kv["SumN"] = "(N) " + Fmt(tmpNval);
            kv["SumNTol"] = tmpNval < 6.1 ? "± 0.1" : "± 0.2";

            double tmpRo = Ro_val(tmpASA, typNum);
            kv["SumRo"] = "R" + Fmt(tmpRo);

            string rsBm = GetString(bm, "Radie innerdiameter storkona (Rs)");
            double tmpRs = (string.IsNullOrEmpty(rsBm) || EqualsI(rsBm, "0"))
                ? (tmpASA ? (Math.Abs(typNum - 24) < 0.001 ? 3.5 : 4.0) : 0.0)
                : TryParseDouble(rsBm);
            kv["SumRs"] = "R" + Fmt(tmpRs).Replace(".", ",");

            string rlBm = GetString(bm, "Radie innerdiameter lillkona (Rl)");
            double tmpRl;
            if (string.IsNullOrEmpty(rlBm) || EqualsI(rlBm, "0"))
                tmpRl = tmpASA
                    ? (Math.Abs(typNum - 24) < 0.001 ? 1.0 : 2.0)
                    : (typNum < 85 ? 1.0 : 2.5);
            else
                tmpRl = TryParseDouble(rlBm);
            kv["SumRl"] = "R" + Fmt(tmpRl);

            kv["SumV120"] = "120º";
            kv["SumV45"] = "45º";
            kv["SumV30"] = "~30º";

            double tmpKval = GetDouble(bm, "Längd på repor (K)");
            if (tmpKval == 0)
            {
                double[] klist = serieInt == 240 ? K240 : serieInt == 241 ? K241 : KASA;
                tmpKval = GetVal(klist, typLista);
            }
            kv["SumK"] = "(K) " + Fmt(tmpKval);
            kv["SumKTol"] = GenTol(tmpKval);

            kv["SumSV"] = TmpSV.ToString(CultureInfo.InvariantCulture) + "°";
            kv["SumSV2"] = kv["SumSV"];

            int tmpARval = (int)GetDouble(bm, "Antal repor (Ar)");
            if (tmpARval == 0)
            {
                int[] arlist = serieInt == 240 ? AR240 : serieInt == 241 ? AR241 : ARASA;
                tmpARval = typLista >= 1 && typLista <= arlist.Length ? arlist[typLista - 1] : 0;
            }
            kv["SumAR"] = "(n) inv/utv " + tmpARval.ToString(CultureInfo.InvariantCulture) + " st.";
            kv["SumAR1"] = "(n) " + tmpARval.ToString(CultureInfo.InvariantCulture) + " st.";
            kv["SumAR2"] = kv["SumAR1"];

            double tmpVR = tmpARval > 1 ? Math.Round((360.0 - (TmpSV * 2.0)) / (tmpARval - 1), 1) : 0;
            kv["SumVR"] = "";
            kv["SumVR1"] = "";
            kv["SumVR2"] = "";

            double tmpK1val = GetDouble(bm, "Längd till repor (K1)");
            if (tmpK1val == 0)
            {
                double[] k1list = serieInt == 240 ? K1_240 : serieInt == 241 ? K1_241 : K1_ASA;
                tmpK1val = GetVal(k1list, typLista);
            }
            kv["SumK1"] = "(K1) " + Fmt(tmpK1val);
            kv["SumK1Tol"] = GenTol(tmpK1val);

            string tmpLG = GetString(bm, "Gänglängd stora gängan (b)");
            if (EqualsI(tmpLG, "0")) tmpLG = "";
            kv["VaLLG"] = string.IsNullOrEmpty(tmpLG)
                ? "Du har inte angett gänglängd så korda räknas fel"
                : "";

            kv["SumM"] = "(M) 1.5";
            kv["SumM1"] = "(M1) 0.5";

            double tmpd = tmpSlash
                ? (tmpSpecDia ? TryParseDouble(tmpBet4) : TryParseDouble(tmpBet3))
                : typNum / 2.0 * 10.0;
            double tmpd1 = D1_val(tmpd, typNum);

            double tmpKonstBOH = Math.Round(Math.Sin((135.0 / 2.0) * Math.PI / 180.0) * 2.0, 4);
            double tmpKonstant1Rep = Math.Round(Math.Sin((TmpSV / 2.0) * Math.PI / 180.0) * 2.0, 4);
            double tmpKonstantDel = Math.Round(Math.Sin((tmpVR / 2.0) * Math.PI / 180.0) * 2.0, 4);
            double tmpKonstantOS = Math.Round(Math.Sin((TmpVOS / 2.0) * Math.PI / 180.0) * 2.0, 4);

            double tmpKordaBOH = Math.Round(((tmpd1 + (tmpBval * 2.0)) / 2.0) * tmpKonstBOH, 1);

            double tmpKorda1RepInv = Math.Round((tmpd1 / 2.0) * tmpKonstant1Rep, 1);
            double tmpLGval = TryParseDouble(tmpLG);
            double tmpKonUtr = ((tmpJval - tmpLGval) / TmpKona) + tmpd;
            double tmpKorda1RepUtv = Math.Round((tmpKonUtr / 2.0) * tmpKonstant1Rep, 1);
            kv["SumKR1inv"] = Fmt(tmpKorda1RepInv - (tmpSlitsVal / 2.0)).Replace(".", ",");
            kv["SumKR1utv"] = Fmt(tmpKorda1RepUtv - (tmpSlitsVal / 2.0)).Replace(".", ",");

            double tmpKordaDelInv = Math.Round((tmpd1 / 2.0) * tmpKonstantDel, 1);
            double tmpKordaDelUtv = Math.Round((tmpKonUtr / 2.0) * tmpKonstantDel, 1);
            kv["SumKRDinv"] = Fmt(tmpKordaDelInv).Replace(".", ",");
            kv["SumKRDutv"] = Fmt(tmpKordaDelUtv).Replace(".", ",");

            double tmpKordaOSinv = Math.Round((tmpd / 2.0) * tmpKonstantOS, 1);
            double tmpKordaOSutv = Math.Round((tmpKonUtr / 2.0) * tmpKonstantOS, 1);
            kv["SumVOS"] = TmpVOS.ToString(CultureInfo.InvariantCulture);
            kv["SumVOS1"] = kv["SumVOS"];
            kv["SumKOSinv"] = TmpSV < TmpVOS ? "OjSpår utaför Repa" : Fmt(tmpKordaOSinv).Replace(".", ",");
            kv["SumKOSutv"] = TmpSV < TmpVOS ? "OjSpår utaför Repa" : Fmt(tmpKordaOSutv).Replace(".", ",");

            SumMaskinVal(kv, maskinVal);
            SumFrequencies(kv, maskinVal);
            SumMeasuringDevices(kv, maskinVal);
            SumAF(kv, maskinVal);

            kv["SumTextS1"] = "";
            kv["SumTextS2"] = "Grader, frifläckar, slagmärken, repor, valkar och andra defekter samt ojämnheter kontrolleras med ögonmått.";
            kv["SumTextGängstigning"] = "Max 2x gängstigning";
            kv["SumÖvrigt"] = "2st. oljeborrhål, Korda (A) = " + Fmt(tmpKordaBOH).Replace(".",",");
            kv["SumORM"] = "Stämpla Oljeriktningsmarkeringar";

            return kv;
        }

        private static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published)) return "";
            DateTime pubDt;
            if (!DateTime.TryParse(published, out pubDt)) return "";
            int dagar = 14;
            DateTime validTill = pubDt.AddDays(dagar);
            if (DateTime.Today <= validTill.Date)
                return "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom " +
                       dagar.ToString(CultureInfo.InvariantCulture) +
                       " dagar)<<LineBreak>>Information om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument<<LineBreak>>Popupruta aktiv till " +
                       validTill.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return "";
        }

        private static void SumMaskinVal(Dictionary<string, string> kv, string maskinVal)
        {
            string s = IsMachine(maskinVal) ? maskinVal : "";
            kv["SumMaskinValS1"] = ("Maskin: " + s + " - BorrOljehål & Oljespår").TrimStart();
            kv["SumMaskinValS2"] = ("Maskin: " + s + " - Oljespår & Repor").TrimStart();
        }

        private static void SumFrequencies(Dictionary<string, string> kv, string maskinVal)
        {
            bool isSkepp = EqualsI(maskinVal, "Skepp6");
            bool isKAT = IsKAT(maskinVal);
            string[] keys = { "SumF1_1","SumF1_2","SumF1_3","SumF1_4","SumF1_5","SumF1_6",
                               "SumF1_7","SumF1_8","SumF1_9","SumF1_0","SumF1_11" };
            foreach (string k in keys)
                kv[k] = isSkepp ? "1/1" : isKAT ? "1/2" : "";
        }

        private static void SumMeasuringDevices(Dictionary<string, string> kv, string maskinVal)
        {
            bool isSkepp = EqualsI(maskinVal, "Skepp6");
            bool isKAT = IsKAT(maskinVal);
            bool any = isSkepp || isKAT;
            kv["SumD1_1"] = isSkepp ? "Skala på borrmaskin" : (isKAT ? "pipborr/djupmått" : "");
            kv["SumD1_2"] = any ? "Skjutmått" : "";
            kv["SumD1_3"] = any ? "Skjutmått" : "";
            kv["SumD1_4"] = any ? "Gängtolk" : "";
            kv["SumD1_5"] = any ? "Skjutmått" : "";
            kv["SumD1_6"] = any ? "Skjutmått/fasmall" : "";
            kv["SumD1_7"] = any ? "Skjutmått" : "";
            kv["SumD1_8"] = any ? "Skjutmått" : "";
            kv["SumD1_9"] = isSkepp ? "Höjdrits" : (isKAT ? "pipborr/djupmått" : "");
            kv["SumD1_0"] = any ? "Skjutmått" : "";
            kv["SumD1_11"] = any ? "Radieyra" : "";
        }

        private static void SumAF(Dictionary<string, string> kv, string maskinVal)
        {
            string[] keys = { "SumAF1_1","SumAF1_2","SumAF1_3","SumAF1_4","SumAF1_5","SumAF1_6",
                               "SumAF1_7","SumAF1_8","SumAF1_9","SumAF1_0","SumAF1_11" };
            foreach (string k in keys) kv[k] = "";
        }

        private static string BuildG(int serie, double typ, string gBm)
        {
            string raw = (gBm ?? "").Trim().ToUpperInvariant().Replace(".", ",");
            if (!string.IsNullOrEmpty(raw) && !EqualsI(raw, "0")) return raw;
            if (serie == 240) return typ > 710 ? "G1/8" : "";
            if (serie == 241)
            {
                if (Math.Abs(typ - 68) < 0.001) return "M6";
                if (Math.Abs(typ - 600) < 0.001 || Math.Abs(typ - 710) < 0.001) return "G1/8";
                if (Math.Abs(typ - 630) < 0.001 || Math.Abs(typ - 670) < 0.001) return "M8";
                return "";
            }
            return "";
        }

        private static double C_val(string g)
        {
            if (EqualsI(g, "M6")) return 9;
            if (EqualsI(g, "G1/8")) return 13;
            if (EqualsI(g, "M8")) return 12;
            return 0;
        }

        private static double T_default(string g)
        {
            if (EqualsI(g, "M6")) return 6.3;
            if (EqualsI(g, "G1/8")) return 10;
            if (EqualsI(g, "M8")) return 8.3;
            return 0;
        }

        private static string D_val(string g)
        {
            if (EqualsI(g, "M6")) return "3";
            if (EqualsI(g, "G1/8")) return "5";
            if (EqualsI(g, "M8")) return "4";
            return "";
        }

        private static string H_val(int serie, double typ)
        {
            if (serie == 0) return "1.5";
            if (serie == 240) return typ < 900 ? "2" : "2.8";
            if (serie == 241)
            {
                if (typ < 500) return "1.2";
                if (typ < 601) return "1.5";
                return "2";
            }
            return "";
        }

        private static double F_val(bool asa, double typ)
        {
            if (!asa) return typ < 85 ? 2 : 3;
            return Math.Abs(typ - 24) < 0.001 ? 3 : 4;
        }

        private static double N_val(bool asa, double typ)
        {
            if (!asa)
            {
                if (typ < 65) return 5;
                if (typ < 85) return 6;
                if (typ < 601) return 7;
                if (typ < 751) return 8;
                return 9;
            }
            return Math.Abs(typ - 24) < 0.001 ? 7 : 8;
        }

        private static double Ro_val(bool asa, double typ)
        {
            if (!asa) { if (typ < 65) return 4; if (typ < 85) return 4.5; return 5; }
            return Math.Abs(typ - 24) < 0.001 ? 5 : 6;
        }

        private static double D1_val(double d, double typ)
        {
            if (typ < 85) return d - 20;
            if (typ < 561 || Math.Abs(typ - 630) < 0.001) return d - 30;
            if (typ < 751) return d - 40;
            if (typ < 1001) return d - 50;
            return d - 60;
        }

        private static string GenTol(double v)
        {
            if (v < 6.1) return "± 0.1";
            if (v < 30.1) return "± 0.2";
            if (v < 120.1) return "± 0.3";
            if (v < 315.1) return "± 0.5";
            if (v < 1000.1) return "± 0.8";
            if (v < 2000.1) return "± 1.2";
            return "± 2.0";
        }

        private static double GetVal(double[] list, int oneBasedIdx)
        {
            if (oneBasedIdx < 1 || oneBasedIdx > list.Length) return 0;
            return list[oneBasedIdx - 1];
        }

        private static int GetMember(string val, string[] list)
        {
            for (int i = 0; i < list.Length; i++)
                if (string.Equals(list[i], val, StringComparison.OrdinalIgnoreCase)) return i + 1;
            return 0;
        }

        private static bool ContainsToken(string needle, string[] tokens)
        {
            for (int i = 0; i < tokens.Length; i++)
                if (string.Equals(tokens[i], needle, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        private static bool IsMachine(string mv)
        {
            if (string.IsNullOrEmpty(mv)) return false;
            for (int i = 0; i < Machines.Length; i++)
                if (string.Equals(Machines[i], mv, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        private static bool IsKAT(string mv)
        {
            if (string.IsNullOrEmpty(mv)) return false;
            for (int i = 0; i < MachinesKAT.Length; i++)
                if (string.Equals(MachinesKAT[i], mv, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        private static bool EqualsI(string a, string b)
            => string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);

        private static int TryParseInt(string s)
        {
            int v;
            return int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static double TryParseDouble(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;
            double v;
            return double.TryParse(s.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static double GetDouble(List<Bookmark> bm, string key)
        {
            string raw = GetString(bm, key);
            if (string.IsNullOrWhiteSpace(raw)) return 0;
            double v;
            return double.TryParse(raw.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static string GetString(List<Bookmark> bm, string key)
        {
            if (bm == null || string.IsNullOrWhiteSpace(key)) return "";
            for (int i = 0; i < bm.Count; i++)
            {
                Bookmark b = bm[i];
                if (b != null && string.Equals(b.BookmarkName ?? "", key, StringComparison.OrdinalIgnoreCase))
                    return b.BookmarkValue ?? "";
            }
            return "";
        }

        private static string Fmt(double v) => v.ToString(CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt1(double v) => v.ToString("F1", CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt3(double v) => v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
    }
}