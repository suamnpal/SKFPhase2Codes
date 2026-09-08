using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Klamhylsor_Stal_Morando_MaxMuller_OP1_3 : ITemplateCalculations
    {
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
            bool tmpLW = tmpBet.IndexOf("LW", StringComparison.OrdinalIgnoreCase) >= 0;

            // Explode " /.-"
            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : "";
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : "";
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : "";

            int cntB2 = tmpBet2.Length, cntB3 = tmpBet3.Length;

            // TmpSerie
            string tmpSerie;
            if (tmpLW) tmpSerie = "0";
            else if (tmpSlash && (cntB2 == 3 || cntB2 == 2)) tmpSerie = tmpBet2;
            else if (cntB2 > 4) tmpSerie = tmpBet2.Substring(0, 3);
            else if (cntB2 == 3) tmpSerie = tmpBet2.Substring(0, 1);
            else tmpSerie = tmpBet2.Length >= 2 ? tmpBet2.Substring(0, 2) : tmpBet2;
            int serieInt = TryParseInt(tmpSerie);

            // TmpTyp
            string tmpTyp;
            if (!tmpSlash) tmpTyp = tmpBet2.Length >= 2 ? tmpBet2.Substring(tmpBet2.Length - 2) : tmpBet2;
            else tmpTyp = cntB3 > 4 ? (tmpBet2.Length >= 2 ? tmpBet2.Substring(tmpBet2.Length - 2) : tmpBet2) : tmpBet3;

            // Bookmarks
            double dBm = GetDouble(bm, "Avvikande YDia Gänga (d)");
            double tmpd;
            if (dBm != 0) tmpd = dBm;
            else if (!tmpSlash || cntB3 > 4) { double t = TryParseDouble(tmpTyp.Replace(",", ".")); tmpd = (t / 2.0) * 10.0; }
            else tmpd = TryParseDouble(tmpBet3.Replace(",", "."));

            double tmpd1 = GetDouble(bm, "Innerdiameter (d1)");
            double tmpd2 = GetDouble(bm, "Kona storände diameter (d2)");
            double tmpb = GetDouble(bm, "Gänglängd (b)");
            double tmpL = GetDouble(bm, "Längd (L)");
            double amatt = GetDouble(bm, "a-mått");
            double konaBm = GetDouble(bm, "Kona");

            int stmm = Stmm(tmpd);
            double tmpdm = DM(tmpd, stmm);
            double tmpd3 = D3(tmpdm, stmm);

            double tmpKona = konaBm == 0
                ? ((serieInt == 30 || serieInt == 31 || serieInt == 32 || serieInt == 39) ? 12 : 30)
                : konaBm;

            // GTj
            double gTol = GTolPos(tmpd, tmpKona);
            double gTolN = GTolNeg(tmpd, tmpKona);
            double tolSkillnad = gTolN - gTol;

            // TmpKL
            string klBm = GetString(bm, "Konlängd (KL)");
            double tmpKL = (string.IsNullOrEmpty(klBm) || EqualsI(klBm, "0"))
                ? (tmpL + 5 - tmpb + 1)
                : TryParseDouble(klBm);
            kv["SumKL"] = "(KL) " + Fmt(tmpKL);
            kv["SumKLTol"] = " 0";
            kv["SumKLTolN"] = "- 1.0";

            // TmpML, TmpVML
            double tmpML = tmpL - tmpb - 4.0;
            double tmpVML = tmpML < 110 ? 75 : 100;

            // ── Op1 rough dims ────────────────────────────────────────────────────
            double tmpLOP1 = tmpL + 5.0;
            double tmpdOP1 = Math.Round(tmpd2 - tmpKL / tmpKona, 1);
            double tmpd1OP1 = tmpd1 - 2.0;
            double tmpd2OP1 = tmpd2 + 2.0;

            kv["SumGänga"] = "Tr" + Fmt(tmpd).Replace(".", ",") + "x" + stmm;
            kv["SumP"] = "Tr" + stmm;
            kv["SumP1"] = "(P) " + stmm;
            kv["SumRullar"] = stmm + "mm";
            kv["SumV"] = tmpKona == 30 ? "(V) 0\u00ba57" : "(V) 2\u00ba23";
            kv["SumKonaOP1"] = "Kona 1:" + Fmt(tmpKona);
            kv["SumKona"] = "Kona 1:" + Fmt(tmpKona);

            kv["SumLOP1"] = "(L) " + Fmt(tmpLOP1);
            kv["SumLOP1Tol"] = "\u00b1 0.5";
            kv["SumLOP2"] = "(L) " + Fmt(tmpL);
            kv["SumLOP2Tol"] = "\u00b1 0.25";
            kv["SumL"] = "(L) " + Fmt(tmpL);
            kv["SumLTol"] = " 0 [3F]";
            kv["SumLTolN"] = LTolNeg(tmpL) + " [3F]";

            kv["Sumb"] = "(b) " + Fmt(tmpb);
            kv["SumbTol"] = BTol(tmpb) + "  [3F]";
            kv["SumbTolN"] = " 0 [2F]";

            double tmpSL = tmpb + 4.0;
            kv["SumSL"] = "(SL) " + Fmt(tmpSL);
            kv["SumSLTol"] = "\u00b1 0.3";

            // d1 rough and finish
            kv["Sumd1OP1"] = "(d1) " + Fmt(tmpd1OP1).Replace(".", ",");
            kv["Sumd1OP1Tol"] = " 0";
            kv["Sumd1OP1TolN"] = "- 0.5";
            kv["Sumd1"] = "(d1) " + Fmt(tmpd1).Replace(".", ",");
            kv["Sumd1Tol"] = D1Tol(tmpd1) + " [3F]";

            // d rough and finish
            kv["SumdOP1"] = "(d) " + Fmt(tmpdOP1).Replace(".", ",");
            kv["SumdOP1Tol"] = "+ 0.5";
            kv["SumdOP1TolN"] = " 0";
            string dOP2TolN = DOP2TolN(stmm);
            kv["SumdOP2"] = "(d) " + Fmt(tmpd).Replace(".", ",");
            kv["SumdOP2Tol"] = " 0";
            kv["SumdOP2TolN"] = dOP2TolN;
            kv["SumdaOP2"] = kv["SumdOP2"];
            kv["SumdaOP2Tol"] = " 0";
            kv["SumdaOP2TolN"] = dOP2TolN;
            kv["Sumd"] = "(d) " + Fmt(tmpd);
            kv["SumdTol"] = " 0";
            kv["SumdTolN"] = dOP2TolN;

            // dm
            kv["Sumdm"] = "(dm) " + Fmt(tmpdm).Replace(".", ",");
            kv["SumdmTol"] = DmTolPos(stmm) + " [3F]";
            kv["SumdmTolN"] = DmTolNeg(stmm) + " [3F]";

            // d3
            kv["Sumd3"] = "(d3) " + Fmt(tmpd3).Replace(".", ",");
            kv["Sumd3Tol"] = " 0";
            kv["Sumd3TolN"] = D3TolN(stmm);

            // fas & radier
            kv["Sumg"] = "(g) " + Fas(tmpd);
            kv["Sumr1"] = Sumr1(serieInt, tmpd);
            kv["Sumr"] = Sumr(tmpd);

            // GTj
            kv["SumGTjTol"] = "+ " + Fmt3(gTol);
            kv["SumGTjaTol"] = kv["SumGTjTol"];
            kv["SumGTjTolN"] = "- " + Fmt3(gTolN);
            kv["SumGTjaTolN"] = kv["SumGTjTolN"];

            // d2 rough & finish
            kv["Sumd2OP1"] = "(d2) " + Fmt(tmpd2OP1).Replace(".",",");
            kv["Sumd2OP1Tol"] = "+ 0.5";
            kv["Sumd2OP1TolN"] = " 0";
            double tmpd2a = EqualsI(GetString(bm, "Kona storände diameter (d2)"), "0") || string.IsNullOrEmpty(GetString(bm, "Kona storände diameter (d2)"))
                ? Math.Round(((tmpL - 1 - amatt) / tmpKona) + tmpd - tolSkillnad, 2)
                : tmpd2;
            kv["Sumd2"] = "(d2) " + Fmt(tmpd2a).Replace(".", ",");
            kv["Sumd2Tol"] = "\u00b1 " + Fmt3(GenTolVal(tmpd2a));

            // GVar
            kv["SumGVarTol"] = GVar(tmpd) + " [2F]";

            // Rakhet
            kv["SumRakA"] = "Max: " + Fmt3(RakA(tmpd));
            kv["SumRakB"] = "Max: " + Fmt3(RakB(tmpd));

            // Orundhet
            kv["SumOrund"] = Orund(tmpd1) + " [3F]";

            // ML, Ra, VinkTol
            kv["SumML"] = "Mätlängd=" + (tmpML < 110 ? "75" : "100");
            kv["SumRa"] = "2.5 [2F]";
            kv["SumRa1"] = "2.5 [2F]";
            kv["SumRa5"] = "5 [2F]";
            double vinkRaw = VinkAngle(tmpd) * tmpVML / 1000.0;
            kv["SumVinkTol"] = Fmt3(vinkRaw) + " [2F]";

            // Mätbygelinställning
            double tmp8 = Math.Round(((tmpd - tmpd1) / 2.0) + ((tmpL - 1 - amatt - 8) / (2.0 * tmpKona)), 3);
            double tmp83 = Math.Round(((tmpd - tmpd1) / 2.0) + ((tmpL - 1 - amatt - 83) / (2.0 * tmpKona)), 3);
            double tmp108 = Math.Round(((tmpd - tmpd1) / 2.0) + ((tmpL - 1 - amatt - 108) / (2.0 * tmpKona)), 3);
            double tmp40 = Math.Round(((tmpd - tmpd1) / 2.0) + ((tmpL - 1 - amatt - 40) / (2.0 * tmpKona)), 3);
            double tmp140 = Math.Round(((tmpd - tmpd1) / 2.0) + ((tmpL - 1 - amatt - 140) / (2.0 * tmpKona)), 3);
            double sumL2 = tmpML < 145 ? 8 : 40;
            double sumL1 = tmpML < 110 ? 83 : tmpML < 145 ? 108 : 140;
            double sumE1 = tmpML < 110 ? tmp83 : tmpML < 145 ? tmp108 : tmp140;
            double sumE2 = tmpML < 145 ? tmp8 : tmp40;
            kv["SumL2"] = Fmt(sumL2);
            kv["SumL1"] = Fmt(sumL1);
            kv["SumE1"] = Fmt3(sumE1).Replace(".", ",");
            kv["SumE2"] = Fmt3(sumE2).Replace(".", ",");

            if (subject == "H 24184")
            {
                kv["SumE1"] = "12,683";
                kv["SumE2"] = "14,35";
            }
            else if (subject == "H 30/500")
            {
                kv["SumE1"] = "16,583";
                kv["SumE2"] = "20,75";
            }
            else if (subject == "H 3096")
            {
                kv["SumE1"] = "16,5";
                kv["SumE2"] = "20,667";
            }
            else if (subject == "H 3188")
            {
                kv["SumE1"] = "19";
                kv["SumE2"] = "23,167";
            }
            else if (subject == "H 3192")
            {
                kv["SumE1"] = "19,583";
                kv["SumE2"] = "23,75";
            }
            else if (subject == "H 3288")
            {
                kv["SumE1"] = "21,25";
                kv["SumE2"] = "25,417";
            }
            else if (subject == "H 3292")
            {
                kv["SumE1"] = "21,917";
                kv["SumE2"] = "26,083";
            }

            string bygel = tmpML < 145 ? "SR 7419471" : "SR 7415991";
            kv["SumBygGtj"] = bygel;
            kv["SumBygGVar"] = bygel;
            kv["SumBygVinkTol"] = bygel;

            // ── Machines ──────────────────────────────────────────────────────────
            bool mOK = EqualsI(maskinVal, "Morando/MaxMuller");
            string s1 = mOK ? "Morando" : "";
            string s2 = mOK ? "MaxMuller" : "";
            string s3 = mOK ? "MaxMuller" : "";
            kv["SumMaskinValS1"] = "Maskin: " + s1 + (mOK ? "" : "INGET MASKINVAL GJORD") + "OP1";
            kv["SumMaskinValS2"] = "Maskin: " + s2 + (mOK ? "" : "INGET MASKINVAL GJORD") + "OP2";
            kv["SumMaskinValS3"] = "Maskin: " + s3 + (mOK ? "" : "INGET MASKINVAL GJORD") + "OP3";

            SumFrequencies(kv, mOK);
            SumDevices(kv, mOK, stmm, bygel);
            SumAF(kv, mOK, kv["SumGVarTol"], kv["SumVinkTol"], kv["SumML"], kv["SumOrund"], kv["SumRakA"], kv["SumRakB"]);

            // ── Texts ─────────────────────────────────────────────────────────────
            kv["SumTextS1"] = "Gjutgodsdefekter: 7433015";
            kv["SumTextS2"] = "";
            kv["SumTextS3"] = "Bryt alla kanter, avlägsna";

            // ── Ritning ───────────────────────────────────────────────────────────
            string tmpRit = serieInt == 30 ? "7438957" : serieInt == 31 ? "7438958"
                          : serieInt == 32 ? "7438955" : serieInt == 39 ? "7434032"
                          : "Styckritning, samma som typ";
            string ritNr = tmpRit + ":senaste utg.";
            kv["SumRitningsnrS1"] = ritNr;
            kv["SumRitningsnrS2"] = ritNr;
            kv["SumRitningsnrS3"] = ritNr;
            kv["SumRitTolS2"] = "Toleranser: 1432012";
            kv["SumRitTolS3"] = "Toleranser: 1432012";
            kv["SumRitGänga"] = "Gänga: 237359, 7430181";

            // ── Klassade egenskaper ───────────────────────────────────────────────
            string klEg = "PRODUCTION NUTS & SLEEVES & HOUSINGSALLMÄNKLASSADE EGENSKAPERKlassade egenskaper klämhylsor";
            kv["SumKlEgenskaperS1"] = "";
            kv["SumKlEgenskaperS2"] = klEg;
            kv["SumKlEgenskaperS3"] = klEg;

            // ── Skärdata — all from bookmarks ─────────────────────────────────────
            string chuckBm = GetString(bm, "Chuckbackar");
            string stödBm = GetString(bm, "Stödbackar");
            string graderBm = GetString(bm, "Grader");
            string varvBm = GetString(bm, "Varvtal");
            string matPlBm = GetString(bm, "Matning Plan");
            string matInUtBm = GetString(bm, "Matning Utv/Inv");
            string fmUtvBm = GetString(bm, "Färdigmått Utv");
            string linjUtvBm = GetString(bm, "Linjal Utv");
            string fmInvBm = GetString(bm, "Färdigmått Inv");
            string linjInvBm = GetString(bm, "Linjal Inv");

            string tmpChuck = (string.IsNullOrEmpty(chuckBm) || EqualsI(chuckBm, "0")) ? "" : chuckBm;
            string tmpStöd = (string.IsNullOrEmpty(stödBm) || EqualsI(stödBm, "0")) ? "" : stödBm + " mm";
            string tmpGrad = (string.IsNullOrEmpty(graderBm) || EqualsI(graderBm, "0")) ? "" : graderBm + " mm";
            string tmpVarv = (string.IsNullOrEmpty(varvBm) || EqualsI(varvBm, "0")) ? "" : varvBm + " /min";
            string tmpMatPl = (string.IsNullOrEmpty(matPlBm) || EqualsI(matPlBm, "0")) ? "" : matPlBm + " /min";
            string tmpMatIU = (string.IsNullOrEmpty(matInUtBm) || EqualsI(matInUtBm, "0")) ? "" : matInUtBm + " /min";

            kv["SumChuckback"] = tmpChuck;
            kv["SumCB"] = string.IsNullOrEmpty(tmpChuck) ? "" : "Chuckbackar:";
            kv["SumStödback"] = tmpStöd;
            kv["SumSB"] = string.IsNullOrEmpty(tmpStöd) ? "" : "Stödbackar:";
            kv["SumGrader"] = tmpGrad;
            kv["SumGR"] = string.IsNullOrEmpty(tmpGrad) ? "" : "Grader:";
            kv["SumVarv"] = tmpVarv;
            kv["SumVR"] = string.IsNullOrEmpty(tmpVarv) ? "" : "Varvtal:";
            kv["SumMatPl"] = tmpMatPl;
            kv["SumMP"] = string.IsNullOrEmpty(tmpMatPl) ? "" : "Matning Plan:";
            kv["SumMatInUt"] = tmpMatIU;
            kv["SumMIU"] = string.IsNullOrEmpty(tmpMatIU) ? "" : "Matning Utv/Inv:";

            // Färdigmått: bm=0 -> "", bm=1 -> computed, else bm directly
            string tmpFMUtv = (string.IsNullOrEmpty(fmUtvBm) || EqualsI(fmUtvBm, "0")) ? ""
                : EqualsI(fmUtvBm, "1") ? (Fmt(tmpd2OP1) + " +0.5 mm") : fmUtvBm;
            string tmpUtvLin = (string.IsNullOrEmpty(linjUtvBm) || EqualsI(linjUtvBm, "0")) ? "" : linjUtvBm + " mm";
            string tmpFMInv = (string.IsNullOrEmpty(fmInvBm) || EqualsI(fmInvBm, "0")) ? ""
                : EqualsI(fmInvBm, "1") ? (Fmt(tmpd1OP1) + " -0.5 mm") : fmInvBm;
            string tmpInvLin = (string.IsNullOrEmpty(linjInvBm) || EqualsI(linjInvBm, "0")) ? "" : linjInvBm + " mm";

            kv["SumFMUtv"] = tmpFMUtv;
            if (subject == "H 24184")
                kv["SumFMUtv"] = "432,07 +0.5 mm";
            if (subject == "H 3196")
                kv["SumFMUtv"] = "503,7 +0.5 mm";
            if (subject == "H 3296")
                kv["SumFMUtv"] = "508,8 +0.5 mm";
            kv["SumFMU"] = string.IsNullOrEmpty(tmpFMUtv) ? "" : "Utvändig diameter:";
            kv["SumUtvLin"] = tmpUtvLin;
            kv["SumUL"] = string.IsNullOrEmpty(tmpUtvLin) ? "" : "Motsvarar på linjal:";
            kv["SumFMInv"] = tmpFMInv;
            kv["SumFMI"] = string.IsNullOrEmpty(tmpFMInv) ? "" : "Invändig diameter:";
            kv["SumInvLin"] = tmpInvLin;
            kv["SumIL"] = string.IsNullOrEmpty(tmpInvLin) ? "" : "Motsvarar på linjal:";

            // Överskrifter
            double dChuck = GetDouble(bm, "Chuckbackar"), dStöd = GetDouble(bm, "Stödbackar"),
                   dGrad = GetDouble(bm, "Grader"), dVarv = GetDouble(bm, "Varvtal"),
                   dMatPl = GetDouble(bm, "Matning Plan"), dMatIU = GetDouble(bm, "Matning Utv/Inv");
            kv["SumIN"] = (dChuck == 0 && dStöd == 0 && dGrad == 0 && dVarv == 0 && dMatPl == 0 && dMatIU == 0) ? "" : "Inställning";
            double dFMU = GetDouble(bm, "Färdigmått Utv"), dLU = GetDouble(bm, "Linjal Utv"),
                   dFMI = GetDouble(bm, "Färdigmått Inv"), dLI = GetDouble(bm, "Linjal Inv");
            kv["SumFM"] = (dFMU == 0 && dLU == 0 && dFMI == 0 && dLI == 0) ? "" : "Färdigmått";

            // Passbitsbeställning — TmpPBKont = only "MaxMuller" (not "Morando/MaxMuller")
            // SumL1 and SumE1/E2 already in kv; TmpBygelinstkonst preserves Lotus exactly
            int tmpBygelinstkonst = Math.Abs(sumL1 - 50) < 0.001 ? -3
                                  : Math.Abs(sumL1 - 108) < 0.001 ? -5
                                  : Math.Abs(sumL1 - 140) < 0.001 ? -10 : 0;
            double tmpInstE1 = Math.Abs(sumL1 - 108) < 0.001 ? sumE1 - 5 : sumE1 - 10;
            double tmpInstE2 = Math.Abs(sumL1 - 108) < 0.001 ? sumE2 - 5 : sumE2 - 10;
            kv["TmpBygelinstkonst"] = tmpBygelinstkonst.ToString(CultureInfo.InvariantCulture);
            kv["TmpInstE1"] = Fmt3(tmpInstE1);
            kv["TmpInstE2"] = Fmt3(tmpInstE2);

            return kv;
        }

        // ── Popup ──────────────────────────────────────────────────────────────────
        private static string ComputePopup(string pub)
        {
            if (string.IsNullOrWhiteSpace(pub)) return "";
            DateTime dt; if (!DateTime.TryParse(pub, out dt)) return "";
            DateTime till = dt.AddDays(14);
            return DateTime.Today <= till.Date
                ? "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)\n\nInformation om senaste ändring finns under fliken Display & Latest Change eller i Notes Qe i aktivt dokument\n\nPopupruta aktiv till " + till.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                : "";
        }

        // ── Freq / Devices / AF ───────────────────────────────────────────────────
        private static void SumFrequencies(Dictionary<string, string> kv, bool m)
        {
            for (int i = 1; i <= 6; i++) kv["SumF1_" + i] = m ? "1/1" : "";
            kv["SumF2_1"] = ""; kv["SumF2_2"] = ""; kv["SumF2_3"] = ""; kv["SumF2_4"] = "";
            kv["SumF2_5"] = ""; kv["SumF2_6"] = ""; kv["SumF2_7"] = ""; kv["SumF2_8"] = "";
            kv["SumF3_1"] = ""; kv["SumF3_2"] = ""; kv["SumF3_3"] = ""; kv["SumF3_4"] = "";
            kv["SumF3_5"] = ""; kv["SumF3_6"] = ""; kv["SumF3_7"] = ""; kv["SumF3_8"] = "";
            if (m)
            {
                kv["SumF2_1"] = "1/5"; kv["SumF2_2"] = "1/2"; kv["SumF2_3"] = "1/1"; kv["SumF2_4"] = "1/5";
                kv["SumF2_5"] = "Inst."; kv["SumF2_6"] = "1/5"; kv["SumF2_7"] = "1/2"; kv["SumF2_8"] = "1/5";
                kv["SumF3_1"] = "1/2"; kv["SumF3_2"] = "1/2"; kv["SumF3_3"] = "1/1"; kv["SumF3_4"] = "1/1";
                kv["SumF3_5"] = "1/1"; kv["SumF3_6"] = "Inst."; kv["SumF3_7"] = "1/5"; kv["SumF3_8"] = "1/5";
            }
        }

        private static void SumDevices(Dictionary<string, string> kv, bool m, int stmm, string bygel)
        {
            kv["SumD1_1"] = m ? "Djupmått" : ""; kv["SumD1_2"] = m ? "Djupmått" : ""; kv["SumD1_3"] = m ? "Skjutmått" : "";
            kv["SumD1_4"] = m ? "Skjutmått/Mikrometer" : ""; kv["SumD1_5"] = m ? "Skjutmått/inv. Mikrometer" : "";
            kv["SumD1_6"] = m ? "Gradskiva Maskin" : "";
            kv["SumD2_1"] = m ? "Skjutmått" : ""; kv["SumD2_2"] = m ? "Skjutmått" : "";
            kv["SumD2_3"] = m ? "Multimar med " + stmm + "mm rullar" : ""; kv["SumD2_4"] = m ? "Djupmått" : "";
            kv["SumD2_5"] = m ? "Radielyra" : ""; kv["SumD2_6"] = m ? "Skjutmått/Vinkelmätare" : "";
            kv["SumD2_7"] = m ? "Gängmall Tr" + stmm : ""; kv["SumD2_8"] = m ? "Skjutmått" : "";
            kv["SumD3_1"] = m ? "Mikrometerstickmått" : ""; kv["SumD3_2"] = m ? "Skjutmått" : "";
            kv["SumD3_3"] = m ? "Mätbygel " + bygel : ""; kv["SumD3_4"] = m ? "Mätbygel " + bygel : "";
            kv["SumD3_5"] = m ? "Mätbygel " + bygel : ""; kv["SumD3_6"] = m ? "Mätmaskin" : "";
            kv["SumD3_7"] = m ? "Egglinjal" : ""; kv["SumD3_8"] = m ? "Egglinjal" : "";
        }

        private static void SumAF(Dictionary<string, string> kv, bool m, string gvar, string vink, string ml, string orund, string rakA, string rakB)
        {
            for (int i = 1; i <= 6; i++) { kv["SumAF1_" + i] = ""; kv["SumAF2_" + i] = ""; kv["SumAF3_" + i] = ""; }
            kv["SumAF2_7"] = ""; kv["SumAF2_8"] = ""; kv["SumAF3_7"] = ""; kv["SumAF3_8"] = "";
            kv["SumAF2_3"] = m ? "Kontrolleras med klove utf.2" : "";
            kv["SumAF2_8"] = m ? "Hjälpmått" : "";
            kv["SumAF3_3"] = m ? "Tol:" : "";
            kv["SumAF3_4"] = m ? "Tol: " + gvar : "";
            kv["SumAF3_5"] = m ? "Tol: " + vink + " " + ml : "";
            kv["SumAF3_6"] = m ? "Max: " + orund : "";
            kv["SumAF3_7"] = m ? rakA.Replace(".",",") : "";
            kv["SumAF3_8"] = m ? rakB.Replace(".",",") : "";
        }

        // ── Dimension helpers ─────────────────────────────────────────────────────
        private static int Stmm(double d) { if (d < 301) return 4; if (d < 501) return 5; if (d < 701) return 6; if (d < 901) return 7; return 8; }
        private static double DM(double d, int s) { if (s == 4) return d - 2; if (s == 5) return d - 2.5; if (s == 6) return d - 3; if (s == 7) return d - 3.5; return d - 4; }
        private static double D3(double dm, int s) { if (s == 4) return dm - 2.5; if (s == 5) return dm - 3; if (s == 6) return dm - 4; if (s == 7) return dm - 4.5; return dm < 1300 ? dm - 5 : dm - 4.5; }
        private static string DOP2TolN(int s) { if (s == 4) return "- 0.300"; if (s == 5) return "- 0.335"; if (s == 6) return "- 0.375"; if (s == 7) return "- 0.425"; return "- 0.450"; }
        private static string DmTolPos(int s) { if (s == 4) return "- 0.190"; if (s == 5) return "- 0.212"; if (s == 6) return "- 0.236"; if (s == 7) return "- 0.250"; return "- 0.265"; }
        private static string DmTolNeg(int s) { if (s == 4) return "- 0.630"; if (s == 5) return "- 0.710"; if (s == 6) return "- 0.800"; if (s == 7) return "- 0.850"; return "- 0.950"; }
        private static string D3TolN(int s) { if (s == 4) return "- 0.750"; if (s == 5) return "- 0.850"; if (s == 6) return "- 0.950"; if (s == 7) return "- 1.000"; return "- 1.120"; }
        private static string D1Tol(double d1) { if (d1 < 31) return "± 0.026"; if (d1 < 51) return "± 0.031"; if (d1 < 81) return "± 0.037"; if (d1 < 121) return "± 0.043"; if (d1 < 181) return "± 0.050"; if (d1 < 251) return "± 0.057"; if (d1 < 316) return "± 0.065"; if (d1 < 401) return "± 0.070"; if (d1 < 501) return "± 0.077"; if (d1 < 631) return "± 0.087"; if (d1 < 801) return "± 0.100"; if (d1 < 1001) return "± 0.115"; return "± 0.130"; }
        private static string BTol(double b) { if (b < 11) return "+ 1.5"; if (b < 19) return "+ 1.8"; if (b < 31) return "+ 2.1"; if (b < 51) return "+ 2.5"; if (b < 81) return "+ 3.0"; if (b < 121) return "+ 3.5"; return "+ 4.0"; }
        private static string LTolNeg(double L) { if (L < 11) return "- 0.580"; if (L < 19) return "- 0.700"; if (L < 31) return "- 0.840"; if (L < 51) return "- 1.000"; if (L < 81) return "- 1.200"; if (L < 121) return "- 1.400"; if (L < 181) return "- 1.600"; if (L < 251) return "- 1.850"; if (L < 316) return "- 2.100"; if (L < 401) return "- 2.300"; return "- 2.500"; }
        private static double GTolPos(double d, double k) { if (Math.Abs(k - 12) < 0.001) { if (d > 1000) return 0.095; if (d > 800) return 0.085; if (d > 630) return 0.075; if (d > 500) return 0.070; if (d > 400) return 0.065; if (d > 315) return 0.060; if (d > 250) return 0.055; if (d > 180) return 0.050; if (d > 120) return 0.040; if (d > 80) return 0.035; if (d > 50) return 0.030; if (d > 30) return 0.025; return 0.020; } if (Math.Abs(k - 30) < 0.001) { if (d > 1000) return 0.060; if (d > 800) return 0.055; if (d > 630) return 0.050; if (d > 500) return 0.045; if (d > 400) return 0.040; if (d > 315) return 0.035; if (d > 250) return 0.035; if (d > 180) return 0.030; if (d > 120) return 0.025; if (d > 80) return 0.022; if (d > 50) return 0.019; if (d > 30) return 0.016; return 0.013; } return 0; }
        private static double GTolNeg(double d, double k) { if (Math.Abs(k - 12) < 0.001) { if (d > 1000) return 0.280; if (d > 800) return 0.250; if (d > 630) return 0.225; if (d > 500) return 0.200; if (d > 400) return 0.190; if (d > 315) return 0.175; if (d > 250) return 0.160; if (d > 180) return 0.140; if (d > 120) return 0.120; if (d > 80) return 0.105; if (d > 50) return 0.090; if (d > 30) return 0.075; return 0.070; } if (Math.Abs(k - 30) < 0.001) { if (d > 1000) return 0.170; if (d > 800) return 0.155; if (d > 630) return 0.140; if (d > 500) return 0.125; if (d > 400) return 0.115; if (d > 315) return 0.105; if (d > 251) return 0.095; if (d > 180) return 0.085; if (d > 120) return 0.075; if (d > 80) return 0.065; if (d > 50) return 0.055; if (d > 30) return 0.046; return 0.039; } return 0; }
        private static double GenTolVal(double v) { if (v < 6.01) return 0.1; if (v < 30.01) return 0.2; if (v < 120.01) return 0.3; if (v < 400.01) return 0.5; if (v < 1000.01) return 0.8; if (v < 2000.01) return 1.2; return 2.0; }
        private static double RakA(double d) { if (d < 101) return 0.008; if (d < 281) return 0.010; if (d < 481) return 0.012; if (d < 601) return 0.014; if (d < 901) return 0.016; return 0.020; }
        private static double RakB(double d) { if (d < 101) return 0.012; if (d < 281) return 0.015; if (d < 481) return 0.018; if (d < 601) return 0.021; if (d < 901) return 0.024; return 0.030; }
        private static string Orund(double d1) { if (d1 < 31) return "0.026"; if (d1 < 51) return "0.031"; if (d1 < 81) return "0.037"; if (d1 < 121) return "0.043"; if (d1 < 181) return "0.050"; if (d1 < 251) return "0.057"; if (d1 < 316) return "0.065"; if (d1 < 401) return "0.070"; if (d1 < 501) return "0.077"; if (d1 < 631) return "0.087"; if (d1 < 801) return "0.100"; if (d1 < 1001) return "0.115"; return "0.130"; }
        private static string GVar(double d) { if (d > 1000) return "0.050"; if (d > 800) return "0.045"; if (d > 630) return "0.040"; if (d > 500) return "0.035"; if (d > 315) return "0.030"; if (d > 250) return "0.025"; if (d > 180) return "0.020"; if (d > 120) return "0.015"; if (d > 50) return "0.010"; return "0.008"; }
        private static double VinkAngle(double d) { if (d > 1000) return 0.09; if (d > 800) return 0.10; if (d > 630) return 0.11; if (d > 500) return 0.12; if (d > 400) return 0.13; if (d > 180) return 0.15; if (d > 150) return 0.18; if (d > 120) return 0.30; if (d > 80) return 0.45; if (d > 50) return 0.50; return 0.60; }
        private static string Fas(double d) { if (d < 301) return "2.7x45\u00ba"; if (d < 501) return "3.2x45\u00ba"; if (d < 671) return "3.8x45\u00ba"; if (d < 901) return "4.4x45\u00ba"; return "5x45\u00ba"; }
        private static string Sumr1(int serie, double d) { return (serie == 30 || serie == 31 || serie == 32 || serie == 39) ? (d < 421 ? "R 1" : "R 2.5") : (d < 421 ? "R 1" : "R 2.5"); }
        private static string Sumr(double d) { if (d < 320) return "R 2.5"; if (d < 530) return "R 3.5"; if (d < 710) return "R 5.5"; return "R 7.5"; }

        // ── Utilities ─────────────────────────────────────────────────────────────
        private static bool EqualsI(string a, string b) => string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);
        private static int TryParseInt(string s) { int v; return int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0; }
        private static double TryParseDouble(string s) { if (string.IsNullOrWhiteSpace(s)) return 0; double v; return double.TryParse(s.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0; }
        private static double GetDouble(List<Bookmark> bm, string key) { string r = GetString(bm, key); if (string.IsNullOrWhiteSpace(r)) return 0; double v; return double.TryParse(r.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0; }
        private static string GetString(List<Bookmark> bm, string key) { if (bm == null || string.IsNullOrWhiteSpace(key)) return ""; for (int i = 0; i < bm.Count; i++) { Bookmark b = bm[i]; if (b != null && string.Equals(b.BookmarkName ?? "", key, StringComparison.OrdinalIgnoreCase)) return b.BookmarkValue ?? ""; } return ""; }
        private static string Fmt(double v) => v.ToString(CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt3(double v) => v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
    }
}