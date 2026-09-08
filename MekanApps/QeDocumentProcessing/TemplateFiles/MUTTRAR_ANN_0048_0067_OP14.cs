using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class MUTTRAR_ANN_0048_0067_OP14 : ITemplateCalculations
    {
        private const int TmpDagar = 14;

        private static readonly string[] TypLista = { "48", "51", "52", "53", "54", "55", "63", "67" };

        private static readonly double[] DLista = { 870, 520, 540, 670, 720, 750, 450, 580 };

        private static readonly double[] D2Lista = { 820, 462, 490, 610, 660, 690, 394, 530 };

        private static readonly string[] AllaMaskiner = { "Morando/K&T/1150", "MaxMuller/K&T", "VTR-160", "MacTurn 550" };

        private static readonly string[] MMKTMaskiner = { "MaxMuller/K&T", "VTR-160", "MacTurn 550" };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;
            List<Bookmark> bm = req != null ? req.Bookmarks : null;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = subject.ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");

            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet2 = Present(tokens.Length > 1 ? tokens[1] : "");

            int tmpCount = tmpBet2.Length;
            string tmpTyp = tmpCount > 3 && tmpBet2.Length >= 2 ? tmpBet2.Substring(tmpBet2.Length - 2) : tmpBet2;
            int typ = TryParseInt(tmpTyp);

            int typLista = GetMember(tmpTyp, TypLista);
            bool typKnown = typLista > 0;

            double tmpD1 = GetDouble(req, bm, "Innerdiameter (D1)");

            int tmpStmm = tmpD1 < 300 ? 4
                        : tmpD1 < 500 ? 5
                        : tmpD1 < 700 ? 6
                        : tmpD1 < 900 ? 7
                        : tmpD1 < 1300 ? 8
                        : 9;
            kv["SumP"] = "(P) " + tmpStmm.ToString(CultureInfo.InvariantCulture);
            string tmpGR = "Tr " + tmpStmm.ToString(CultureInfo.InvariantCulture);

            double tmpd4 = tmpStmm == 4 ? tmpD1 + 4.5
                         : tmpStmm == 5 ? tmpD1 + 5.5
                         : tmpStmm == 6 ? tmpD1 + 7
                         : tmpStmm == 7 ? tmpD1 + 8
                         : tmpStmm == 8 ? tmpD1 + 9
                         : tmpD1 + 10;

            double tmpGanga = (tmpStmm == 4 || tmpStmm == 5) ? tmpd4 - 0.5 : tmpd4 - 1;
            kv["SumGänga"] = "Tr " + Num(tmpGanga) + "x" + tmpStmm.ToString(CultureInfo.InvariantCulture);

            double tmpdm = tmpStmm == 4 ? tmpD1 + 2
                         : tmpStmm == 5 ? tmpD1 + 2.5
                         : tmpStmm == 6 ? tmpD1 + 3
                         : tmpStmm == 7 ? tmpD1 + 3.5
                         : tmpD1 + 4.5;
            kv["Sumdm"] = "(dm) " + Num(tmpdm);
            double tmpdmTol = tmpd4 < 301 ? 0.475
                            : tmpd4 < 501 ? 0.53
                            : tmpd4 < 701 ? 0.6
                            : tmpd4 < 901 ? 0.63
                            : 0.71;
            kv["SumdmTol"] = "+ " + Fmt(tmpdmTol);
            kv["SumdmTolN"] = "- 0";

            double tmpD1SSK = tmpD1 - 1;
            kv["SumD1SSK"] = "(D1) " + Num(tmpD1SSK);
            kv["SumD1"] = "(D1) " + Num(tmpD1);
            double tmpD1Tol = tmpd4 < 301 ? 0.375
                            : tmpd4 < 501 ? 0.45
                            : tmpd4 < 701 ? 0.5
                            : tmpd4 < 901 ? 0.56
                            : 0.63;
            kv["SumD1Tol"] = "+ " + Fmt(tmpD1Tol);
            kv["SumD1TolN"] = "- 0";

            double tmpD = ListValue(DLista, typLista);
            kv["SumD"] = "(D) " + ListText(DLista, typLista);
            kv["SumDTol"] = "+ 0";
            kv["SumDTolN"] = "- " + Fmt(h13(tmpD));

            double tmpD2 = ListValue(D2Lista, typLista);
            kv["SumD2"] = "(D2) " + ListText(D2Lista, typLista);
            kv["SumD2Tol"] = tmpD2 < 6.01 ? "± 0.1"
                           : tmpD2 < 30.01 ? "± 0.2"
                           : tmpD2 < 120.01 ? "± 0.3"
                           : tmpD2 < 400.01 ? "± 0.5"
                           : tmpD2 < 1000.01 ? "± 0.8"
                           : tmpD2 < 2000.01 ? "± 1.2"
                           : "± 2.0";

            double tmpD3 = tmpd4 + 1.5;
            kv["SumD3"] = "(D3) " + Num(tmpD3);
            double tmpD3S1 = tmpD3 + 2;
            kv["SumD3S1"] = "(D3) " + Num(tmpD3S1);
            kv["SumD3Tol"] = "+ " + Fmt(H14(tmpD3));
            kv["SumD3TolN"] = "- 0";
            kv["SumD3S1Tol"] = kv["SumD3Tol"];
            kv["SumD3S1TolN"] = kv["SumD3TolN"];

            double tmpB = typ == 51 ? 57
                        : typ == 63 ? 50
                        : typ == 48 ? 90
                        : (typ == 52 || typ == 67) ? 65
                        : 80;
            double tmpBSSK = tmpB + 1;
            kv["SumBSSK"] = "(B) " + Num(tmpBSSK);
            kv["SumB"] = "(B) " + Num(tmpB);
            kv["SumBTol"] = "+ 0";
            kv["SumBTolN"] = "- " + Fmt(h13(tmpB));

            kv["SumF1"] = "45º";
            kv["SumF1S3"] = "45º";
            kv["SumF2"] = "45º";
            kv["SumF2S3"] = "45º";

            kv["SumR16"] = typKnown ? "R1.6" : "";
            kv["SumR16Tol"] = "± 0.4";
            kv["SumR2"] = "R2";
            kv["SumR2a"] = "R2";
            kv["SumR2b"] = "R2";
            kv["SumRm2"] = In(typ, 48, 51, 55, 63, 67) ? "max R1" : "max R2";
            kv["SumR8"] = "max R8";
            kv["SumR3"] = "R3";
            kv["SumR3a"] = "R3";
            kv["SumR3b"] = "R3";

            string tmpKa = In(typ, 51, 63, 67) ? "0.05"
                         : tmpd4 < 51 ? "0.04"
                         : tmpd4 < 121 ? "0.05"
                         : tmpd4 < 251 ? "0.06"
                         : tmpd4 < 316 ? "0.07"
                         : tmpd4 < 401 ? "0.08"
                         : tmpd4 < 501 ? "0.09"
                         : tmpd4 < 631 ? "0.10"
                         : tmpd4 < 801 ? "0.12"
                         : tmpd4 < 1001 ? "0.14"
                         : "0.16";
            kv["SumPL"] = tmpKa;
            kv["SumKa"] = tmpKa;
            kv["SumLR"] = typKnown ? "1.5" : "";

            kv["SumRa5"] = "5";
            kv["SumRa25"] = "2.5";
            kv["SumRa25a"] = "2.5";

            kv["SumH"] = "(H) 3";

            string tmpS = typ == 48 ? "55"
                        : (typ == 51 || typ == 52) ? "32"
                        : (typ == 53 || typ == 54) ? "40"
                        : typ == 55 ? "45"
                        : typ == 63 ? "28"
                        : typ == 67 ? "36"
                        : "";
            double tmpSVal = TryParseDouble(tmpS);
            kv["SumS"] = "(S) " + tmpS;
            kv["SumSTol"] = tmpSVal < 10.01 ? "± 0.180"
                          : tmpSVal < 18.01 ? "± 0.215"
                          : tmpSVal < 30.01 ? "± 0.260"
                          : tmpSVal < 50.01 ? "± 0.310"
                          : "± 0.370";

            string tmpT = typ == 48 ? "25"
                        : In(typ, 51, 52, 67) ? "15"
                        : In(typ, 53, 54, 55) ? "20"
                        : typ == 63 ? "14"
                        : "";
            double tmpTVal = TryParseDouble(tmpT);
            kv["SumT"] = "(T) " + tmpT;
            kv["SumTTol"] = "+ " + Fmt(tmpTVal < 16 ? 1.8 : 2.1);
            kv["SumTTolN"] = "- 0";

            kv["SumK5"] = In(typ, 48, 51, 55, 63, 67) ? "(3)" : "(5)";
            kv["SumK1"] = "(1)";

            double tmpKB = In(typ, 48, 53, 54, 55) ? 50 : 40;
            kv["SumKB"] = "(KB) " + Num(tmpKB);
            double tmpKBTol = tmpKB < 30.01 ? 0.084 : tmpKB < 50.01 ? 0.1 : 0.12;
            kv["SumKBTol"] = "+ " + Fmt(tmpKBTol) + " [2]";
            kv["SumKBTolN"] = "- 0";

            string tmpK = typ == 63 ? "40"
                        : (typ == 51 || typ == 67) ? "50"
                        : (typ == 53 || typ == 54) ? "49"
                        : (typ == 48 || typ == 55) ? "61"
                        : "";
            double tmpKVal = TryParseDouble(tmpK);
            kv["SumK"] = "(K) " + tmpK;
            double tmpKTol = tmpKVal < 18.01 ? 0.27
                           : tmpKVal < 30.01 ? 0.33
                           : tmpKVal < 50.01 ? 0.39
                           : tmpKVal < 80.01 ? 0.46
                           : tmpKVal < 120.01 ? 0.54
                           : 0.63;
            kv["SumKTol"] = "+ " + Fmt(tmpKTol);
            kv["SumKTolN"] = "- 0";

            double tmpKS = In(typ, 48, 53, 54, 55) ? 44 : typ == 63 ? 30 : 35;
            kv["SumKS"] = "(KS) " + Num(tmpKS) + "<<LineBreak>>" + " (3x)";
            kv["SumKSTol"] = tmpKS < 6.01 ? "± 0.1"
                           : tmpKS < 30.01 ? "± 0.2"
                           : tmpKS < 120.01 ? "± 0.3"
                           : tmpKS < 400.01 ? "± 0.5"
                           : tmpKS < 1000.01 ? "± 0.8"
                           : tmpKS < 2000.01 ? "± 1.2"
                           : "± 2.0";
            kv["SumKG"] = (In(typ, 48, 53, 54, 55) ? "M20-6H" : "M16-6H") + "<<LineBreak>>" + " (3x)";

            double tmpL = In(typ, 48, 53, 54, 55) ? 16 : 12;
            kv["SumL"] = "(L) " + Num(tmpL) + " (x3)";
            double tmpLH = In(typ, 48, 53, 54, 55) ? 14 : 12;
            kv["SumLH"] = "(LH) Ø " + Num(tmpLH) + " (x3)";
            kv["SumLD"] = "(LD) " + (In(typ, 48, 54, 55) ? "40" : In(typ, 52, 53) ? "35" : "");

            double tmpO = typ == 48 ? 45
                        : typ == 51 ? 28.5
                        : In(typ, 53, 54, 55) ? 40
                        : typ == 63 ? 25
                        : typ == 67 ? 32.5
                        : 0;
            bool harO = In(typ, 48, 51, 53, 54, 55, 63, 67);
            kv["SumÖ"] = harO ? "(Ö) " + Num(tmpO) : "";
            kv["SumÖTol"] = harO
                ? (tmpO < 6.01 ? "± 0.1" : tmpO < 30.01 ? "± 0.2" : tmpO < 120.01 ? "± 0.3" : "± 0.5")
                : "";
            kv["SumNonÖ"] = typ == 52 ? "OBS!" + "<<LineBreak>>" + "Ingen lyftögla på denna typ" : "";
            kv["SumÖG"] = (typ == 48 || typ == 55) ? "M12" : In(typ, 51, 53, 54, 63, 67) ? "M10" : "";
            kv["SumÖD"] = (typ == 48 || typ == 55) ? "min 21" : In(typ, 51, 53, 54, 63, 67) ? "min 17" : "";

            kv["SumKlEgenskaperS2"] = "PRODUCTION NUTS & SLEEVES & HOUSINGS\\ALLMÄNKLASSADE EGENSKAPER\\Klassade egenskaper muttrar";
            kv["SumKlEgenskaperS3"] = kv["SumKlEgenskaperS2"];

            string sumRitS1 = "11K " + tmpBet + (In(typ, 48, 53, 54) ? ":3" : typ == 52 ? ":1" : ":2");
            kv["SumRitS1"] = sumRitS1;
            kv["SumRitS2"] = sumRitS1;
            kv["SumRitS3"] = sumRitS1;

            bool morando = EqualsI(maskinVal, "Morando/K&T/1150");
            bool maxMuller = EqualsI(maskinVal, "MaxMuller/K&T");
            bool tmpMMKT = GetMember(maskinVal, MMKTMaskiner) > 0;
            bool mAll = GetMember(maskinVal, AllaMaskiner) > 0;

            string tmpMaskinValS1 = morando ? "Morando"
                                  : maxMuller ? "MaxMuller"
                                  : EqualsI(maskinVal, "VTR-160") ? "VTR-160"
                                  : EqualsI(maskinVal, "MacTurn 550") ? "MacTurn 550"
                                  : "";
            kv["SumMaskinValS1"] = "Maskin: " + tmpMaskinValS1 + " - OP1 & 2" + "<<LineBreak>>";

            string tmpMaskinValS2 = morando ? "K&T"
                                  : maxMuller ? "K&T"
                                  : EqualsI(maskinVal, "VTR-160") ? "VTR-160"
                                  : EqualsI(maskinVal, "MacTurn 550") ? "MacTurn 550"
                                  : "";
            kv["SumMaskinValS2"] = "Maskin: " + tmpMaskinValS2 + " - OP3" + "<<LineBreak>>";

            string tmpMaskinValS3 = morando ? "1150"
                                  : maxMuller ? "MaxMuller"
                                  : EqualsI(maskinVal, "VTR-160") ? "VTR-160"
                                  : EqualsI(maskinVal, "MacTurn 550") ? "MacTurn 550"
                                  : "";
            kv["SumMaskinValS3"] = "Maskin: " + tmpMaskinValS3 + " - OP4" + "<<LineBreak>>";

            string f13 = morando ? "1/1" : tmpMMKT ? "1/3" : "";
            string f15 = morando ? "1/1" : tmpMMKT ? "1/5" : "";

            kv["SumF1_1"] = f13;
            kv["SumF1_2"] = f15;
            kv["SumF1_4"] = f15;
            kv["SumF1_5"] = f15;
            kv["SumF1_7"] = f15;
            kv["SumF1_8"] = f15;
            kv["SumF1_9"] = f15;

            kv["SumF2_1"] = mAll ? "1/5" : "";
            kv["SumF2_2"] = mAll ? "1/5" : "";
            kv["SumF2_3"] = mAll ? "1/5" : "";
            kv["SumF2_4"] = mAll ? "1/5" : "";
            kv["SumF2_5"] = mAll ? "1/5" : "";
            kv["SumF2_6"] = mAll ? "1/1" : "";
            kv["SumF2_7"] = "";

            kv["SumF3_1"] = f13;
            kv["SumF3_2"] = f15;
            kv["SumF3_3"] = f15;
            kv["SumF3_4"] = f15;
            kv["SumF3_5"] = f15;
            kv["SumF3_6"] = f15;
            kv["SumF3_7"] = f15;
            kv["SumF3_8"] = f15;
            kv["SumF3_9"] = f15;
            kv["SumF3_0"] = f15;

            kv["SumD1_1"] = mAll ? "Skjutmått" : "";
            kv["SumD1_2"] = mAll ? "Mikrometer/Skjutmått" : "";
            kv["SumD1_4"] = mAll ? "Skjutmått" : "";
            kv["SumD1_5"] = mAll ? "Skjutmått" : "";
            kv["SumD1_7"] = mAll ? "Fasmall" : "";
            kv["SumD1_8"] = mAll ? "Radielyra" : "";
            kv["SumD1_9"] = mAll ? "Ra-mätare" : "";

            kv["SumD2_1"] = mAll ? "Skjutmått" : "";
            kv["SumD2_2"] = mAll ? "Gängtolkar" : "";
            kv["SumD2_3"] = mAll ? "Skjutmått" : "";
            kv["SumD2_4"] = mAll ? "Skjutmått" : "";
            kv["SumD2_5"] = mAll ? "Radielyra" : "";
            kv["SumD2_6"] = mAll ? "Passbitar" : "";
            kv["SumD2_7"] = "";

            kv["SumD3_1"] = mAll ? "Multimar " + tmpGR + " A-Rullar" : "";
            kv["SumD3_2"] = mAll ? "Mikrometer" : "";
            kv["SumD3_3"] = mAll ? tmpGR + " Gängmall" : "";
            kv["SumD3_4"] = mAll ? "Skjutmått" : "";
            kv["SumD3_5"] = mAll ? "Skjutmått" : "";
            kv["SumD3_6"] = mAll ? "Skjutmått" : "";
            kv["SumD3_7"] = mAll ? "Radielyra" : "";
            kv["SumD3_8"] = mAll ? "Ra-mätare" : "";
            kv["SumD3_9"] = mAll ? "Mätmaskin" : "";
            kv["SumD3_0"] = mAll ? "Mätmaskin/Egglinjal" : "";

            kv["SumAF1_1"] = maxMuller ? "Bearbetas i OP1" : "";
            kv["SumAF1_2"] = maxMuller ? "Bearbetas i OP2" : "";
            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = maxMuller ? "Bearbetning i OP1 + 3mm" : "";
            kv["SumAF1_7"] = "";
            kv["SumAF1_8"] = "";
            kv["SumAF1_9"] = (morando || maxMuller) ? "FärdigBearbetade ytor Ra 5" : "";

            kv["SumAF2_1"] = "";
            kv["SumAF2_2"] = mAll ? " Gängdjup på lyftöglegänga" : "";
            kv["SumAF2_3"] = "";
            kv["SumAF2_4"] = "";
            kv["SumAF2_5"] = "";
            kv["SumAF2_6"] = mAll ? "Passbitar monteras på alla" : "";
            kv["SumAF2_7"] = "";

            kv["SumAF3_1"] = mAll ? "" : "Kontrolleras med passbitsklove utf. 1";
            kv["SumAF3_2"] = "";
            kv["SumAF3_3"] = "";
            kv["SumAF3_4"] = "";
            kv["SumAF3_5"] = "";
            kv["SumAF3_6"] = "";
            kv["SumAF3_7"] = "";
            kv["SumAF3_8"] = "";
            kv["SumAF3_9"] = mAll ? "" : "Vid misstänkt formfel lämnas muttern till mätrum";
            kv["SumAF3_0"] = "";

            string chuckbackar = GetField(req, bm, "Chuckbackar");
            string stodbackar = GetField(req, bm, "Stödbackar");
            string grader = GetField(req, bm, "Grader");
            string varvtal = GetField(req, bm, "Varvtal");
            string matningPlan = GetField(req, bm, "Matning Plan");
            string matningUtvInv = GetField(req, bm, "Matning Utv/Inv");
            string fardigmattUtv = GetField(req, bm, "Färdigmått Utv");
            string linjalUtv = GetField(req, bm, "Linjal Utv");
            string fardigmattInv = GetField(req, bm, "Färdigmått Inv");
            string linjalInv = GetField(req, bm, "Linjal Inv");

            bool skipCB = chuckbackar == "0" || tmpMMKT;
            kv["SumChuckback"] = skipCB ? "" : chuckbackar;
            kv["SumCB"] = skipCB ? "" : "Chuckbackar:";

            bool skipSB = stodbackar == "0" || tmpMMKT;
            kv["SumStödback"] = skipSB ? "" : stodbackar + " mm";
            kv["SumSB"] = skipSB ? "" : "Stödbackar:";

            bool skipGR = grader == "0" || tmpMMKT;
            kv["SumGrader"] = skipGR ? "" : grader + " mm";
            kv["SumGR"] = skipGR ? "" : "Grader:";

            bool skipVR = varvtal == "0" || tmpMMKT;
            kv["SumVarv"] = skipVR ? "" : varvtal + " /min";
            kv["SumVR"] = skipVR ? "" : "Varvtal:";

            bool skipMP = matningPlan == "0" || tmpMMKT;
            kv["SumMatPl"] = skipMP ? "" : matningPlan + " /min";
            kv["SumMP"] = skipMP ? "" : "Matning Plan:";

            bool skipMIU = matningUtvInv == "0" || tmpMMKT;
            kv["SumMatInUt"] = skipMIU ? "" : matningUtvInv + " /min";
            kv["SumMIU"] = skipMIU ? "" : "Matning Utv/Inv:";

            bool skipFMU = fardigmattUtv == "0" || tmpMMKT;
            kv["SumFMUtv"] = skipFMU ? "" : fardigmattUtv + " mm";
            kv["SumFMU"] = skipFMU ? "" : "Utvändig diameter:";

            bool skipUL = linjalUtv == "0" || tmpMMKT;
            kv["SumUtvLin"] = skipUL ? "" : linjalUtv + " mm";
            kv["SumUL"] = skipUL ? "" : "Motsvarar på linjal:";

            bool skipFMI = fardigmattInv == "0" || tmpMMKT;
            kv["SumFMInv"] = skipFMI ? "" : fardigmattInv + " mm";
            kv["SumFMI"] = skipFMI ? "" : "Invändig diameter:";

            bool skipIL = linjalInv == "0" || tmpMMKT;
            kv["SumInvLin"] = skipIL ? "" : linjalInv + " mm";
            kv["SumIL"] = skipIL ? "" : "Motsvarar på linjal:";

            bool allaInst = TryParseDouble(chuckbackar) == 0
                         && TryParseDouble(stodbackar) == 0
                         && TryParseDouble(grader) == 0
                         && TryParseDouble(varvtal) == 0
                         && TryParseDouble(matningPlan) == 0
                         && TryParseDouble(matningUtvInv) == 0;
            kv["SumIN"] = tmpMMKT ? "" : (allaInst ? "" : "Inställning");

            bool allaFM = TryParseDouble(fardigmattUtv) == 0
                       && TryParseDouble(linjalUtv) == 0
                       && TryParseDouble(fardigmattInv) == 0
                       && TryParseDouble(linjalInv) == 0;
            kv["SumFM"] = tmpMMKT ? "" : (allaFM ? "" : "Färdigmått");

            kv["SumTextS1"] = "";
            kv["SumTextS2"] = "Övriga mått kontrolleras vid inställning";
            kv["SumTextS3"] = "Grader, frifläckar, slagmärken, repor, valkar och andra defekter samt ojämnheter kontrolleras med ögonmått, för övriga mått används skjutmått.";
            kv["SumMark"] = "Märkes med ID 1,2,3 för klack & position";

            kv["VaL750"] = (tmpD < 751 && maxMuller) ? ""
                         : (tmpD > 751 && morando) ? ""
                         : "OBS Kontrollera att rätt flöde valts" + "<<LineBreak>>" + "Ytterdiameter överrensstämmer inte med maskinval";

            return kv;
        }

        private static string ComputePopup(string pub)
        {
            if (string.IsNullOrWhiteSpace(pub)) return "";
            DateTime dt;
            if (!DateTime.TryParse(pub, out dt)) return "";
            DateTime till = dt.AddDays(TmpDagar);
            if (DateTime.Today > till.Date) return "";
            return "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom " + TmpDagar + " dagar)" +
                   "<<LineBreak>><<LineBreak>>" +
                   "<<LineBreak>><<LineBreak>>" +
                   "Information om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument" +
                   "<<LineBreak>><<LineBreak>>" +
                   "Popupruta aktiv till " + till.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        private static double h13(double v)
        {
            if (v < 18.01) return 0.27;
            if (v < 30.01) return 0.33;
            if (v < 50.01) return 0.39;
            if (v < 80.01) return 0.46;
            if (v < 120.01) return 0.54;
            if (v < 180.01) return 0.63;
            if (v < 250.01) return 0.72;
            if (v < 315.01) return 0.81;
            if (v < 400.01) return 0.89;
            if (v < 500.01) return 0.97;
            if (v < 630.01) return 1.1;
            if (v < 800.01) return 1.25;
            if (v < 1000.01) return 1.4;
            if (v < 1250.01) return 1.65;
            if (v < 1600.01) return 1.95;
            if (v < 2000.01) return 2.3;
            if (v < 2500.01) return 2.8;
            return 3.3;
        }

        private static double H14(double v)
        {
            if (v < 19) return 0.43;
            if (v < 31) return 0.52;
            if (v < 51) return 0.62;
            if (v < 81) return 0.74;
            if (v < 121) return 0.87;
            if (v < 181) return 1;
            if (v < 251) return 1.15;
            if (v < 316) return 1.3;
            if (v < 401) return 1.4;
            if (v < 501) return 1.55;
            if (v < 631) return 1.75;
            if (v < 801) return 2;
            if (v < 1000) return 2.3;
            return 2.6;
        }

        private static double ListValue(double[] list, int index)
        {
            if (list == null || index <= 0 || index > list.Length) return 0;
            return list[index - 1];
        }

        private static string ListText(double[] list, int index)
        {
            if (list == null || index <= 0 || index > list.Length) return "";
            return Num(list[index - 1]);
        }

        private static int GetMember(string val, string[] list)
        {
            if (val == null || list == null) return 0;
            for (int i = 0; i < list.Length; i++)
                if (string.Equals(list[i], val, StringComparison.OrdinalIgnoreCase)) return i + 1;
            return 0;
        }

        private static bool In(int value, params int[] list)
        {
            if (list == null) return false;
            for (int i = 0; i < list.Length; i++)
                if (value == list[i]) return true;
            return false;
        }

        private static string Present(string token)
        {
            if (string.IsNullOrEmpty(token)) return "";
            double v;
            if (double.TryParse(token.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) && v != 0)
                return v.ToString("0.################", CultureInfo.InvariantCulture);
            return token;
        }

        private static bool EqualsI(string a, string b)
        {
            return string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);
        }

        private static int TryParseInt(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;
            int v;
            return int.TryParse(s.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static double TryParseDouble(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;
            double v;
            return double.TryParse(s.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        private static string GetField(APIRequest req, List<Bookmark> bm, string name)
        {
            string r = GetString(bm, name);
            if (!string.IsNullOrEmpty(r)) return r;
            return GetRequestField(req, name);
        }

        private static double GetDouble(APIRequest req, List<Bookmark> bm, string name)
        {
            return TryParseDouble(GetField(req, bm, name));
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

        private static string GetRequestField(APIRequest req, string name)
        {
            if (req == null) return "";
            PropertyInfo p = req.GetType().GetProperty(name);
            if (p == null) return "";
            object v = p.GetValue(req, null);
            return v == null ? "" : v.ToString();
        }

        private static string Fmt(double v)
        {
            return Math.Round(v, 4).ToString("0.####", CultureInfo.InvariantCulture);
        }

        private static string Num(double v)
        {
            return Fmt(v).Replace(".", ",");
        }
    }
}