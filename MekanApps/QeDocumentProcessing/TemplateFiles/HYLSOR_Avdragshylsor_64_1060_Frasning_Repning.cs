using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Avdragshylsor_64_1060_Frasning_Repning : ITemplateCalculations
    {
        private static readonly string[] ArtList = { "AOH", "AOHX", "LW", "MS" };

        private static readonly string[] Rit22List = { "7437987", "241214", "222253", "7437361" };
        private static readonly string[] Rit240List = { "7437697", "7437698", "232633", "232634", "232631", "232632", "7437370", "7437371" };
        private static readonly string[] Rit241List = { "7437699", "7437700", "232637", "232638", "232636", "7437373", "7437372" };
        private static readonly string[] Rit30List = { "7437376", "7437377", "236600", "234385", "7437362", "7437363" };
        private static readonly string[] Rit31List = { "7437378", "7437379", "236601", "231282", "7437364", "7437365" };
        private static readonly string[] Rit32List = { "7437380", "7437381", "239444", "231638", "7437366", "7437367" };
        private static readonly string[] Rit39List = { "AOH 39/500", "AOH 39/530", "AOH 39/600", "AOH 39/710", "AOH 39/850" };

        private const string TmpTol05_6 = "± 0.1";
        private const string TmpTol6_30 = "± 0.2";
        private const string TmpTol30_120 = "± 0.3";
        private const string TmpTol120_400 = "± 0.5";
        private const string TmpTol400_1000 = "± 0.8";

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;
            List<Bookmark> bm = req != null ? req.Bookmarks : null;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string ritningsnummer = GetString(bm, "Ritningsnummer");

            bool tmpRit22 = ContainsAny(ritningsnummer, Rit22List);
            bool tmpRit240 = ContainsAny(ritningsnummer, Rit240List);
            bool tmpRit241 = ContainsAny(ritningsnummer, Rit241List);
            bool tmpRit30 = ContainsAny(ritningsnummer, Rit30List);
            bool tmpRit31 = ContainsAny(ritningsnummer, Rit31List);
            bool tmpRit32 = ContainsAny(ritningsnummer, Rit32List);
            bool tmpRit39 = ContainsAny(ritningsnummer, Rit39List);
            bool tmpRitLW = ContainsAny(ritningsnummer, new string[] { "LW" });
            int tmpRitSum = (tmpRit22 ? 1 : 0) + (tmpRit240 ? 1 : 0) + (tmpRit241 ? 1 : 0)
                          + (tmpRit30 ? 1 : 0) + (tmpRit31 ? 1 : 0) + (tmpRit32 ? 1 : 0);

            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");
            bool tmpLW = tmpBet.IndexOf("LW", StringComparison.OrdinalIgnoreCase) >= 0;
            bool tmpMS = tmpBet.IndexOf("MS", StringComparison.OrdinalIgnoreCase) >= 0;

            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : "";
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : "";
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : "";

            int cntB2 = tmpBet2.Length;
            int artLista = GetMember(tmpBet1, ArtList);

            string tmpTypStr;
            if (tmpLW) tmpTypStr = "0";
            else if (tmpMS) tmpTypStr = "";
            else if (cntB2 == 3 || cntB2 == 2) tmpTypStr = tmpBet2;
            else if (cntB2 > 4) tmpTypStr = tmpBet2.Substring(0, 3);
            else tmpTypStr = tmpBet2.Length >= 2 ? tmpBet2.Substring(0, 2) : tmpBet2;

            double tmpTyp = TryParseDouble(tmpTypStr);

            double tmpTyp2;
            if (tmpLW || tmpMS)
                tmpTyp2 = 0;
            else if (cntB2 > 3)
                tmpTyp2 = TryParseDouble(tmpBet2.Length >= 2 ? tmpBet2.Substring(tmpBet2.Length - 2) : tmpBet2);
            else
                tmpTyp2 = TryParseDouble(tmpBet3);

            double aldreStandard = GetDouble(bm, "Äldre standard");
            double konaBm = GetDouble(bm, "Kona");
            double specialIDia = GetDouble(bm, "Special Innerdiameter (d1)");
            double tmpLangdL = GetDouble(bm, "Längd (L)");
            double tmpAmatt = GetDouble(bm, "a-mått (a)");

            double tmpKona;
            if (konaBm != 0) tmpKona = konaBm;
            else if (Is(tmpTyp, 240) || Is(tmpTyp, 241)) tmpKona = 30;
            else if (Is(tmpTyp, 0)) tmpKona = 0;
            else tmpKona = 12;

            string tmpListaIYDia = GetListaIYDia(aldreStandard, tmpTyp, tmpTyp2);
            string[] iy = tmpListaIYDia.Split(':');

            string tmpIDiaStr = specialIDia != 0 ? Fmt(specialIDia) : (iy.Length > 0 ? iy[0] : "");
            double tmpIDia = TryParseDouble(tmpIDiaStr);
            string tmpYDiaStr = iy.Length > 1 ? iy[1] : "";
            double tmpYDia = TryParseDouble(tmpYDiaStr);

            kv["SumIDia"] = tmpIDiaStr.Replace(",", ".");
            kv["SumIDiaTol"] = D1TolPos(tmpTyp2) + " [3F]";
            kv["SumIDiaTolN"] = D1TolNeg(tmpTyp2) + " [3F]";

            double tmpKonringsdiameter = tmpTyp2 > 100 ? tmpTyp2 : (tmpTyp2 / 2.0) * 10.0;

            double tmpC = tmpTyp2 < 530 ? 8 : 10;
            kv["SumC"] = "(c) " + Fmt(tmpC);
            kv["SumCTol"] = TmpTol6_30;

            string tmpLista = GetLista(tmpRit22, tmpRit240, tmpRit241, tmpRit30, tmpRit31, tmpRit32, tmpRit39, tmpRitLW, tmpTyp2);
            string[] lista = tmpLista.Split(':');

            string tmpBStr = lista.Length > 0 ? lista[0] : "";
            double tmpB = TryParseDouble(tmpBStr);
            kv["SumB"] = "(B) " + tmpBStr.Replace(",", ".");
            kv["SumBTol"] = GenTolStr(tmpB);

            string tmpListaRep = GetListaRep(tmpTyp2);
            string[] listaRep = tmpListaRep.Split(':');
            double tmpResListaRep = TryParseDouble(listaRep.Length > 0 ? listaRep[0] : "");
            double tmpAntRep = TryParseDouble(listaRep.Length > 1 ? listaRep[1] : "");
            kv["SumAntRep"] = "(n) " + Fmt(tmpAntRep) + " st.";

            double startVinkelRepor = GetDouble(bm, "Startvinkel Repor");
            double delningsVinkelRepor = GetDouble(bm, "Delningsvinkel Repor");

            double tmpDelVinkelRepor;
            if (startVinkelRepor == 0)
                tmpDelVinkelRepor = delningsVinkelRepor == 0 ? tmpResListaRep : delningsVinkelRepor;
            else
                tmpDelVinkelRepor = (tmpAntRep - 1.0) != 0 ? (360.0 - (startVinkelRepor * 2.0)) / (tmpAntRep - 1.0) : 0;

            double tmp1 = startVinkelRepor == 0
                ? (360.0 - (tmpDelVinkelRepor * (tmpAntRep - 1.0))) / 2.0
                : startVinkelRepor;

            kv["SumV1Rep"] = FmtComma(Round(tmp1, 2));
            kv["SumV1Rep2"] = kv["SumV1Rep"];
            kv["SumVDRep"] = FmtComma(Round(tmpDelVinkelRepor, 2));
            kv["SumVDRep2"] = kv["SumVDRep"];

            double tmpKonstant1Rep = Round(Math.Sin((tmp1 / 2.0) * Math.PI / 180.0) * 2.0, 4);
            double tmpKonstantDelRep = Round(Math.Sin((tmpDelVinkelRepor / 2.0) * Math.PI / 180.0) * 2.0, 4);

            double tmpKonUtrakning = tmpKona != 0
                ? ((((tmpLangdL + tmpAmatt) - tmpB) / tmpKona) + tmpKonringsdiameter)
                : tmpKonringsdiameter;

            double tmpKorda1RepInv = Round((tmpIDia / 2.0) * tmpKonstant1Rep, 1);
            kv["SumKR1inv"] = FmtComma(tmpKorda1RepInv - (tmpC / 2.0));

            double tmpKorda1RepUtv = Round((tmpKonUtrakning / 2.0) * tmpKonstant1Rep, 1);
            kv["SumKR1utv"] = FmtComma(tmpKorda1RepUtv - (tmpC / 2.0));

            double tmpKordaDelRepInv = Round((tmpIDia / 2.0) * tmpKonstantDelRep, 1);
            kv["SumKRDinv"] = FmtComma(tmpKordaDelRepInv);
            double tmpKordaDelRepUtv = Round((tmpKonUtrakning / 2.0) * tmpKonstantDelRep, 1);
            kv["SumKRDutv"] = FmtComma(tmpKordaDelRepUtv);

            double startVinkelOljespar = GetDouble(bm, "Startvinkel Oljespår");
            double tmpVOS = startVinkelOljespar == 0 ? 10 : startVinkelOljespar;
            kv["SumVOS"] = FmtComma(tmpVOS);
            kv["SumVOS1"] = FmtComma(tmpVOS);

            double tmpKonstantOS = Round(Math.Sin((tmpVOS / 2.0) * Math.PI / 180.0) * 2.0, 4);
            double tmpKordaOSinv = Round((tmpIDia / 2.0) * tmpKonstantOS, 1);
            kv["SumKOSinv"] = tmp1 < tmpVOS ? "OjSpår utaför Repa" : FmtComma(tmpKordaOSinv);
            double tmpKordaOSutv = Round((tmpKonUtrakning / 2.0) * tmpKonstantOS, 1);
            kv["SumKOSutv"] = tmp1 < tmpVOS ? "OjSpår utaför Repa" : FmtComma(tmpKordaOSutv);

            double tmpE;
            if (aldreStandard == 0)
                tmpE = tmpYDia < 330 ? 5 : tmpYDia < 430 ? 6 : tmpYDia < 550 ? 7 : tmpYDia < 700 ? 8 : tmpYDia < 860 ? 10 : 12;
            else
                tmpE = tmpYDia < 330 ? 5 : tmpYDia < 441 ? 6 : tmpYDia < 550 ? 7 : tmpYDia < 700 ? 8 : tmpYDia < 860 ? 10 : 12;

            kv["SumE"] = "(E) " + Fmt(tmpE);
            kv["SumETol"] = tmpE < 6 ? TmpTol05_6 : TmpTol6_30;
            kv["Sumr1"] = Is(tmpE, 5) ? "R4"
                        : Is(tmpE, 6) ? "R4,5"
                        : Is(tmpE, 7) ? "R5"
                        : Is(tmpE, 8) ? "R6"
                        : Is(tmpE, 10) ? "R7" : "R8";

            string tmpF = Is(tmpE, 5) ? "1"
                        : Is(tmpE, 6) ? "1.2"
                        : Is(tmpE, 7) ? "1.5"
                        : Is(tmpE, 8) ? "1.5"
                        : Is(tmpE, 10) ? "2" : "2.7";
            kv["SumF"] = "(F) " + tmpF;
            kv["SumFTol"] = "  0";
            kv["SumFTolN"] = "- 0.2";

            string tmpJStr = lista.Length > 1 ? lista[1] : "";
            double tmpJ = TryParseDouble(tmpJStr);
            kv["SumJ"] = "(J) " + tmpJStr.Replace(",", ".");
            kv["SumJTol"] = GenTolStr(tmpJ);

            string tmpKStr = lista.Length > 2 ? lista[2] : "";
            double tmpK = TryParseDouble(tmpKStr);
            kv["SumK"] = "(K) " + tmpKStr.Replace(",", ".");
            kv["SumKTol"] = GenTolStr(tmpK);

            kv["SumM"] = "(M) 1.5";
            kv["SumMTol"] = "±0.1";
            kv["SumN"] = "(N) 0.5";
            kv["SumNTol"] = "±0.1";

            kv["SumM2"] = "(M) 5";
            kv["SumM2Tol"] = "+0.5";
            kv["SumM2TolN"] = " 0.0";

            kv["SumRitning"] = ritningsnummer + " - Huvudritning: 7437373 - Tol. efter slits: 7437495:4";
            kv["SumSTRit"] = tmpRitSum == 0 ? "Styckritning " : "";

            bool isVTR = EqualsI(maskinVal, "VTR");
            bool isMT = EqualsI(maskinVal, "MacTurn");
            string mvSuffix = (isVTR || isMT) ? "" : "INGEN MASKINVAL GJORD";
            kv["SumMaskinVal"] = "Maskin: " + maskinVal + mvSuffix;

            kv["SumF_ID"] = isVTR ? "1/1" : isMT ? "1/5" : "";
            kv["SumF_Ö"] = isVTR ? "1/1" : isMT ? "1/5" : "";
            kv["SumD_ID"] = (isVTR || isMT) ? "Skjutmått" : "";
            kv["SumD_Ö"] = (isVTR || isMT) ? "Skjutmått" : "";
            kv["SumAF_ID"] = "";
            kv["SumAF_Ö"] = "";

            kv["SumTextMät"] = "Alla mått kontrolleras vid inställning ";

            return kv;
        }

        private static string GetListaIYDia(double aldreStandard, double typ, double typ2)
        {
            if (aldreStandard == 0)
            {
                if (Is(typ, 0) || Is(typ, 22) || Is(typ, 30) || Is(typ, 31) || Is(typ, 32) || Is(typ, 39) || Is(typ, 240) || Is(typ, 241))
                    return IYDiaNew(typ2);
                return "MANUELL INMATNING";
            }

            if (Is(typ, 30)) return IYDiaOld30(typ2);
            if (Is(typ, 22) || Is(typ, 31) || Is(typ, 32)) return IYDiaOld223132(typ2);
            if (Is(typ, 240) || Is(typ, 241)) return IYDiaOld240241(typ2);
            return "";
        }

        private static string IYDiaNew(double t)
        {
            if (Is(t, 64)) return "300:340";
            if (Is(t, 68)) return "320:360";
            if (Is(t, 72)) return "340:380";
            if (Is(t, 76)) return "360:400";
            if (Is(t, 80)) return "380:420";
            if (Is(t, 84)) return "400:440";
            if (Is(t, 88)) return "420:460";
            if (Is(t, 92)) return "440:480";
            if (Is(t, 96)) return "460:500";
            if (Is(t, 500)) return "480:530";
            if (Is(t, 530)) return "500:560";
            if (Is(t, 560)) return "530:600";
            if (Is(t, 600)) return "570:630";
            if (Is(t, 630)) return "600:670";
            if (Is(t, 670)) return "630:710";
            if (Is(t, 710)) return "670:750";
            if (Is(t, 750)) return "710:800";
            if (Is(t, 800)) return "750:850";
            if (Is(t, 850)) return "800:900";
            if (Is(t, 900)) return "850:950";
            if (Is(t, 950)) return "900:1000";
            if (Is(t, 1000)) return "950:1060";
            return "1000:1120";
        }

        private static string IYDiaOld30(double t)
        {
            if (Is(t, 64)) return "300:345";
            if (Is(t, 68)) return "320:365";
            if (Is(t, 72)) return "340:385";
            if (Is(t, 76)) return "360:410";
            if (Is(t, 80)) return "380:430";
            if (Is(t, 84)) return "400:450";
            if (Is(t, 88)) return "420:470";
            if (Is(t, 92)) return "440:490";
            if (Is(t, 96)) return "460:520";
            if (Is(t, 500)) return "480:540";
            return "Subject FEL";
        }

        private static string IYDiaOld223132(double t)
        {
            if (Is(t, 64)) return "300:350";
            if (Is(t, 68)) return "320:370";
            if (Is(t, 72)) return "340:400";
            if (Is(t, 76)) return "360:420";
            if (Is(t, 80)) return "380:440";
            if (Is(t, 84)) return "400:460";
            if (Is(t, 88)) return "420:480";
            if (Is(t, 92)) return "440:510";
            if (Is(t, 96)) return "460:530";
            if (Is(t, 500)) return "480:550";
            return "Subject FEL";
        }

        private static string IYDiaOld240241(double t)
        {
            if (Is(t, 64)) return "300:330";
            if (Is(t, 530)) return "500:550";
            if (Is(t, 560)) return "530:580";
            if (Is(t, 630)) return "600:650";
            if (Is(t, 670)) return "630:690";
            if (Is(t, 710)) return "670:730";
            if (Is(t, 750)) return "710:775";
            if (Is(t, 800)) return "750:825";
            return "800:875";
        }

        private static string GetLista(bool r22, bool r240, bool r241, bool r30, bool r31, bool r32, bool r39, bool rLW, double t)
        {
            if (r22) return Is(t, 64) ? "122:22:75" : "FEL Subject";
            if (r240) return Lista240(t);
            if (r241) return Lista241(t);
            if (r30) return Lista30(t);
            if (r31) return Lista31(t);
            if (r32) return Lista32(t);
            if (r39) return Lista39(t);
            if (rLW) return Is(t, 850) ? "389:79:262" : "0:0:0";
            return "";
        }

        private static string Lista240(double t)
        {
            if (Is(t, 64)) return "130:24:80";
            if (Is(t, 68)) return "144:27:90";
            if (Is(t, 72)) return "145:27:90";
            if (Is(t, 76)) return "147:27:90";
            if (Is(t, 80)) return "158:30:100";
            if (Is(t, 84)) return "162:30:100";
            if (Is(t, 88)) return "169:32:106";
            if (Is(t, 92) || Is(t, 96)) return "175:33:109";
            if (Is(t, 500)) return "178:33:109";
            if (Is(t, 530)) return "196:38:125";
            if (Is(t, 560)) return "204:39:129";
            if (Is(t, 600)) return "214:41:136";
            if (Is(t, 630)) return "225:44:145";
            if (Is(t, 670)) return "235:46:154";
            if (Is(t, 710)) return "244:47:158";
            if (Is(t, 750)) return "257:50:168";
            if (Is(t, 800)) return "268:52:173";
            if (Is(t, 850)) return "281:55:183";
            if (Is(t, 900)) return "306:56:188";
            if (Is(t, 950)) return "327:62:206";
            if (Is(t, 1000)) return "334:62:206";
            return "351:66:219";
        }

        private static string Lista241(double t)
        {
            if (Is(t, 64)) return "162:33:109";
            if (Is(t, 68)) return "179:36:122";
            if (Is(t, 72)) return "180:36:122";
            if (Is(t, 76)) return "182:36:122";
            if (Is(t, 80)) return "186:38:125";
            if (Is(t, 84) || Is(t, 88)) return "206:42:140";
            if (Is(t, 92)) return "220:45:150";
            if (Is(t, 96)) return "224:46:154";
            if (Is(t, 500)) return "237:49:162";
            if (Is(t, 530)) return "243:50:168";
            if (Is(t, 560)) return "257:53:178";
            if (Is(t, 600)) return "270:56:188";
            if (Is(t, 630)) return "286:60:200";
            if (Is(t, 670)) return "293:62:206";
            if (Is(t, 710)) return "312:66:219";
            if (Is(t, 750)) return "334:71:238";
            if (Is(t, 800)) return "339:71:238";
            if (Is(t, 850)) return "375:75:250";
            if (Is(t, 900)) return "388:77:258";
            if (Is(t, 950)) return "405:82:272";
            if (Is(t, 1000)) return "434:87:290";
            return "445:90:300";
        }

        private static string Lista30(double t)
        {
            if (Is(t, 64)) return "102:18:61";
            if (Is(t, 68)) return "110:20:67";
            if (Is(t, 72)) return "115:20:67";
            if (Is(t, 76)) return "120:20:68";
            if (Is(t, 80)) return "125:22:74";
            if (Is(t, 84)) return "128:22:75";
            if (Is(t, 88)) return "135:24:79";
            if (Is(t, 92)) return "140:24:82";
            if (Is(t, 96)) return "143:25:83";
            if (Is(t, 500)) return "145:25:84";
            if (Is(t, 530)) return "159:28:92";
            if (Is(t, 560)) return "164:29:98";
            if (Is(t, 600)) return "169:30:100";
            if (Is(t, 630)) return "177:32:106";
            if (Is(t, 670)) return "190:34:115";
            if (Is(t, 710)) return "196:35:118";
            if (Is(t, 750)) return "204:38:125";
            if (Is(t, 800)) return "210:39:129";
            if (Is(t, 850)) return "221:41:136";
            if (Is(t, 900)) return "229:42:140";
            if (Is(t, 950)) return "240:45:150";
            if (Is(t, 1000)) return "248:46:154";
            return "261:49:162";
        }

        private static string Lista31(double t)
        {
            if (Is(t, 64)) return "138:26:88";
            if (Is(t, 68)) return "148:28:95";
            if (Is(t, 72)) return "150:29:96";
            if (Is(t, 76)) return "155:29:97";
            if (Is(t, 80)) return "160:30:100";
            if (Is(t, 84)) return "175:34:112";
            if (Is(t, 88)) return "180:34:113";
            if (Is(t, 92)) return "188:36:120";
            if (Is(t, 96)) return "195:37:124";
            if (Is(t, 500)) return "205:40:132";
            if (Is(t, 530)) return "215:41:136";
            if (Is(t, 560)) return "221:42:140";
            if (Is(t, 600)) return "234:45:150";
            if (Is(t, 630)) return "247:47:158";
            if (Is(t, 670)) return "258:50:168";
            if (Is(t, 710)) return "266:52:172";
            if (Is(t, 750)) return "277:55:182";
            if (Is(t, 800)) return "287:56:188";
            if (Is(t, 850)) return "300:60:200";
            if (Is(t, 900)) return "310:62:206";
            if (Is(t, 950)) return "323:66:219";
            if (Is(t, 1000)) return "339:69:231";
            return "348:71:238";
        }

        private static string Lista32(double t)
        {
            if (Is(t, 64)) return "160:32:104";
            if (Is(t, 68)) return "172:34:112";
            if (Is(t, 72)) return "179:35:116";
            if (Is(t, 76)) return "186:36:120";
            if (Is(t, 80)) return "197:38:128";
            if (Is(t, 84)) return "209:41:136";
            if (Is(t, 88)) return "215:42:140";
            if (Is(t, 92)) return "227:44:148";
            if (Is(t, 96)) return "236:47:155";
            if (Is(t, 500)) return "254:50:168";
            if (Is(t, 530)) return "264:53:178";
            if (Is(t, 560)) return "270:55:182";
            if (Is(t, 600)) return "284:58:194";
            if (Is(t, 630)) return "304:62:206";
            if (Is(t, 670)) return "317:66:219";
            if (Is(t, 710)) return "328:68:225";
            if (Is(t, 750)) return "342:71:238";
            if (Is(t, 800)) return "351:72:242";
            if (Is(t, 850)) return "371:77:258";
            if (Is(t, 900)) return "373:77:258";
            if (Is(t, 950)) return "382:80:265";
            return "400:84:280";
        }

        private static string Lista39(double t)
        {
            if (Is(t, 500)) return "114:19:64";
            if (Is(t, 530)) return "124:20:68";
            if (Is(t, 600)) return "134:27:75";
            if (Is(t, 710)) return "159:32:90";
            return "0:0:0";
        }

        private static string GetListaRep(double t)
        {
            if (Is(t, 64) || Is(t, 68)) return "38:10";
            if (Is(t, 72) || Is(t, 76)) return "34:11";
            if (Is(t, 80)) return "31:12";
            if (Is(t, 84) || Is(t, 88)) return "28:13";
            if (Is(t, 92) || Is(t, 96)) return "26:14";
            if (Is(t, 500)) return "24:15";
            if (Is(t, 530)) return "22:16";
            if (Is(t, 560)) return "21:17";
            if (Is(t, 600)) return "20:18";
            if (Is(t, 630)) return "19:19";
            if (Is(t, 670)) return "18:20";
            if (Is(t, 710)) return "17:21";
            if (Is(t, 750)) return "16:22";
            if (Is(t, 800)) return "15:24";
            if (Is(t, 850)) return "13:26";
            if (Is(t, 900)) return "13:27";
            if (Is(t, 950)) return "12,5:28";
            if (Is(t, 1000)) return "11,5:30";
            return "11:32";
        }

        private static string ComputePopup(string pub)
        {
            if (string.IsNullOrWhiteSpace(pub)) return "";
            DateTime dt; if (!DateTime.TryParse(pub, out dt)) return "";
            DateTime till = dt.AddDays(14);
            return DateTime.Today <= till.Date
                ? "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)<<LineBreak>>Popupruta aktiv till " + till.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                : "";
        }

        private static string D1TolPos(double typ2)
        {
            if (typ2 < 65) return "+ 0.210";
            if (typ2 < 85) return "+ 0.360";
            if (typ2 < 550) return "+ 0.400";
            if (typ2 < 700) return "+ 0.440";
            if (typ2 < 860) return "+ 0.500";
            return "+ 0.560";
        }

        private static string D1TolNeg(double typ2)
        {
            if (typ2 < 65) return "- 0.320";
            if (typ2 < 85) return "- 0.570";
            if (typ2 < 550) return "- 0.630";
            if (typ2 < 700) return "- 0.700";
            if (typ2 < 860) return "- 0.800";
            return "- 0.900";
        }

        private static string GenTolStr(double v)
        {
            if (v < 6) return TmpTol05_6;
            if (v < 30) return TmpTol6_30;
            if (v < 120) return TmpTol30_120;
            if (v < 400) return TmpTol120_400;
            return TmpTol400_1000;
        }

        private static bool Is(double a, double b) => Math.Abs(a - b) < 0.0001;

        private static double Round(double v, int decimals) =>
            Math.Round(v, decimals, MidpointRounding.AwayFromZero);

        private static bool ContainsAny(string value, string[] list)
        {
            if (string.IsNullOrEmpty(value) || list == null) return false;
            for (int i = 0; i < list.Length; i++)
                if (!string.IsNullOrEmpty(list[i]) && value.IndexOf(list[i], StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
            return false;
        }

        private static int GetMember(string val, string[] list)
        {
            if (list == null) return 0;
            for (int i = 0; i < list.Length; i++)
                if (string.Equals(list[i], val, StringComparison.OrdinalIgnoreCase)) return i + 1;
            return 0;
        }

        private static bool EqualsI(string a, string b) =>
            string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);

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

        private static string FmtComma(double v) =>
            v.ToString("0.################", CommonFunctions.Culture).Replace(".", ",");
    }
}