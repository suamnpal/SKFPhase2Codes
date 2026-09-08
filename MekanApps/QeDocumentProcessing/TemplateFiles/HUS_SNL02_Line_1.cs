using DocumentFormat.OpenXml.Office2016.Word.Symex;
using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HUS_SNL02_Line_1 : ITemplateCalculations
    {
        private static readonly HashSet<string> ValidTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            { "07","08","09","10","11","12","13","14","15","16","17" };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string mv = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;
            List<Bookmark> bm = req != null ? req.Bookmarks : null;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");
            bool tmpSlash = tmpBet.IndexOf('/') >= 0;

            // Explode: " /.-"
            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet1 = tokens.Length > 0 ? tokens[0] : string.Empty;
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : string.Empty;
            string tmpBet3 = tokens.Length > 2 ? tokens[2] : string.Empty;

            int cntB2 = tmpBet2.Length;
            string tmpSerie = tmpBet2.Length >= 1 ? tmpBet2.Substring(0, 1) : string.Empty;
            string tmpTyp = tmpBet2.Length >= 2 ? tmpBet2.Substring(tmpBet2.Length - 2) : tmpBet2;
            int serieInt = TryParseInt(tmpSerie);
            int typInt = TryParseInt(tmpTyp);

            // TmpTypLista: @Contains — just boolean membership check
            bool typInList = ValidTypes.Contains(tmpTyp);
            // TmpArtLista: @Member("SE":"SNL") — 1-based position (not used in outputs)

            // Validation
            kv["VaLTyp"] = (cntB2 != 3 || !typInList)
                ? "Fel i beteckning eller fel mall" : "";

            // ── All dimensions come from bookmarks ───────────────────────────────

            double tmpLB = GetDouble(bm, "LagerBredd (LB)");
            double tmpAd = GetDouble(bm, "Axelhålsdiameter (Ad)");
            double tmpL = GetDouble(bm, "LagerLäge (L)");
            double tmpHuh = GetDouble(bm, "Höjd underhalva(Huh)");
            double tmpE = GetDouble(bm, "Fothöjd (E)");
            double tmpR = GetDouble(bm, "Hålavstånd (R)");
            double tmpSa = GetDouble(bm, "Stiftavstånd (Sa)");
            double tmpTd = GetDouble(bm, "Tätningsspårsdiameter (Td)");
            double tmpTa = GetDouble(bm, "Tätningsspårsavstånd (Ta)");
            double tmpVhBm = GetDouble(bm, "Vårthöjd (Vh)");
            double tmpVh = tmpVhBm + 0.4;   // bookmark + 0.4 offset

            // TmpAdSSK: if serie=5 AND bm=0: TmpAd-1 else bm
            double adOp = GetDouble(bm, "Axelhåls (Ad) OP10,20");
            double tmpAdSSK = (serieInt == 5 && adOp == 0) ? tmpAd - 1.0 : adOp;

            double tmpAr = tmpAdSSK / 2.0;
            double tmpLSSK = tmpL - 1.0;
            double tmpFa = tmpHuh - tmpAr;
            double tmpUs = tmpHuh - (tmpLSSK / 2.0);
            double tmpLBSSK = tmpLB - 1.6;

            // G7 tol for L (lagerläge)
            double tmpLTol = G7Pos(tmpL);
            double tmpLTolN = G7Neg(tmpL);
            double tmpLur = tmpL + ((tmpLTol - tmpLTolN) / 2.0 + tmpLTolN);
            double tmpSLval = Math.Round(tmpLur + 0.6, 2);

            // ── LB (Lagerbredd) ──────────────────────────────────────────────────
            kv["SumLBÖH"] = "(LB) " + Fmt(tmpLBSSK).Replace(".", ",");
            kv["SumLBÖHTol"] = "± 0.15";
            kv["SumLBUH"] = kv["SumLBÖH"];
            kv["SumLBUHTol"] = "± 0.15";
            kv["SumLBOP4"] = "(LB) " + Fmt(tmpLB).Replace(".",",");
            kv["SumLBOP4Tol"] = H12TolStr(tmpLB) + " [3]";
            kv["SumLBOP4TolN"] = " 0 [2]";

            // ── Ad (Axelhålsdiameter) ────────────────────────────────────────────
            kv["SumAdÖH"] = "(Ad) " + Fmt(tmpAdSSK).Replace(".", ",");
            kv["SumAdÖHTol"] = "+ 0.3";
            kv["SumAdÖHTolN"] = " 0";
            kv["SumAdUH"] = kv["SumAdÖH"];
            kv["SumAdUHTol"] = "+ 0.3";
            kv["SumAdUHTolN"] = " 0";
            kv["SumAd"] = "(Ad) " + Fmt(tmpAd).Replace(".", ",");
            kv["SumAdTol"] = H12TolStr(tmpAd) + " [3]";
            kv["SumAdTolN"] = " 0 [3]";

            // ── Ar (Axelhål hopläggningsyta) ─────────────────────────────────────
            kv["SumArÖH"] = "(Ar) " + Fmt(tmpAr).Replace(".", ",");
            kv["SumArÖHTol"] = "+ 0.15";
            kv["SumArÖHTolN"] = " 0";
            kv["SumArUH"] = kv["SumArÖH"];
            kv["SumArUHTol"] = "+ 0.15";
            kv["SumArUHTolN"] = " 0";

            // ── L (Lagerläge) ────────────────────────────────────────────────────
            kv["SumLÖH"] = "(L) " + Fmt(tmpLSSK).Replace(".", ",");
            kv["SumLÖHTol"] = "+ 0.3";
            kv["SumLÖHTolN"] = " 0";
            kv["SumLUH"] = kv["SumLÖH"];
            kv["SumLUHTol"] = "+ 0.3";
            kv["SumLUHTolN"] = " 0";
            kv["SumL"] = "(L) " + Fmt(tmpL).Replace(".", ",");
            kv["SumLTol"] = "+ " + Fmt3(tmpLTol);
            kv["SumLTolN"] = "+ " + Fmt3(tmpLTolN);

            // ── SL (Släppningsdia) ───────────────────────────────────────────────
            //kv["SumSL"] = "(SL) " + Fmt2(tmpSLval).Replace(".", ",");
            if (tmpBet2 == "213" || tmpBet2 == "215" || tmpBet2 == "509" || tmpBet2 == "510" || tmpBet2 == "511" || tmpBet2 == "512" || tmpBet2 == "513")
            {
                kv["SumSL"] = "(SL) " + (Math.Round(tmpSLval, 2, MidpointRounding.ToZero)).ToString().Replace(".", ",");
            }
            else
            {
                kv["SumSL"] = "(SL) " + (Math.Round(tmpSLval + 0.01, 2, MidpointRounding.ToZero)).ToString().Replace(".", ",");
            }
           // kv["SumSL"] = "(SL) " + (Math.Round(tmpSLval + 0.01, 2, MidpointRounding.ToZero)).ToString().Replace(".", ",");


            kv["SumSLTol"] = " 0";
            kv["SumSLTolN"] = "- 0.1 [2]";

            // ── Sb (Släppningsbredd) ─────────────────────────────────────────────
            double tmpSb = typInt < 12 ? 5.0 : 6.0;
            kv["SumSb"] = "(2X)(Sb) " + Fmt(tmpSb);
            kv["SumSbTol"] = "+ 0.5";
            kv["SumSbTolN"] = "- 0.1";

            // ── J / I (bredd labels only — no values) ────────────────────────────
            kv["SumJÖH"] = "(J)"; kv["SumJ2ÖH"] = "(J)";
            kv["SumJUH"] = "(J)"; kv["SumJ2UH"] = "(J)";
            kv["SumIÖH"] = "(I)"; kv["SumI2ÖH"] = "(I)";
            kv["SumIUH"] = "(I)"; kv["SumI2UH"] = "(I)";
            kv["SumIOP4"] = "(2X)(I)";

            // ── Planhet ───────────────────────────────────────────────────────────
            kv["SumPÖH"] = "0.05"; kv["SumPUH"] = "0.05";
            kv["SumPf"] = "0.08"; kv["SumPf2"] = "0.08";

            // ── Vh (Vårthöjd) ────────────────────────────────────────────────────
            kv["SumVh"] = "(Vh) " + Fmt(tmpVh).Replace(".", ",");
            kv["SumVhTol"] = "± 0.4 [3]";

            // ── Huh (Höjd underhalva) ────────────────────────────────────────────
            string tmpHuhTolRitning = JS13TolStr(tmpHuh) + " [3]";
            kv["SumHuh"] = "(Huh) " + Fmt(tmpHuh);
            kv["SumHuhTol"] = " 0";
            kv["SumHuhTolN"] = "- 0.1";
            kv["TmpHuhTolRitning"] = tmpHuhTolRitning;   // used in AF2_7

            // ── Ch (Centrumhöjd) ─────────────────────────────────────────────────
            kv["SumCh"] = "(Ch) " + Fmt(tmpHuh);
            kv["SumChTol"] = JS11TolStr(tmpHuh) + " [3]";

            // ── Fa (Fotplan axelhål) ──────────────────────────────────────────────
            kv["SumFa"] = "(Fa) " + Fmt(tmpFa).Replace(".", ",");
            kv["SumFaTol"] = "± 0.10";

            // ── Us (Fotplan lagerläge) ────────────────────────────────────────────
            kv["SumUs"] = "(Us) " + Fmt(tmpUs);
            kv["SumUsTol"] = "± 0.10";

            // ── E (Fothöjd) ───────────────────────────────────────────────────────
            kv["SumE"] = "(E) " + Fmt(tmpE).Replace(".", ",");
            kv["SumETol"] = "± 0.40 [3]";

            // ── R (Hålavstånd) ────────────────────────────────────────────────────
            kv["SumRÖH"] = "(R) " + Fmt(tmpR).Replace(".", ",");
            kv["SumRÖHTol"] = GenTolStr(tmpR);
            kv["SumRUH"] = kv["SumRÖH"];
            kv["SumRUHTol"] = kv["SumRÖHTol"];

            // ── Sa (Stiftavstånd) ─────────────────────────────────────────────────
            kv["SumSaÖH"] = "(Sa) " + Fmt(tmpSa);
            kv["SumSaÖHTol"] = "± 0.30";
            kv["SumSaUH"] = kv["SumSaÖH"];
            kv["SumSaUHTol"] = "± 0.30";

            // ── Sd (Stifthålsdiameter) ────────────────────────────────────────────
            // if TmpTyp<18 AND TmpTyp>06: 5 else "" — all valid types (07-17) match
            string tmpSd = (typInt < 18 && typInt > 6) ? "5" : "";
            kv["SumSdÖH"] = "(Sd) " + tmpSd;
            kv["SumSdÖHTol"] = "+ 0.03";
            kv["SumSdÖHTolN"] = " 0";
            kv["SumSdUH"] = kv["SumSdÖH"];
            kv["SumSdUHTol"] = "+ 0.03";
            kv["SumSdUHTolN"] = " 0";

            // ── Km ───────────────────────────────────────────────────────────────
            kv["SumKm"] = "(Km)";
            kv["SumKm2"] = "(Km)";

            // ── H (Frigående håldia) ──────────────────────────────────────────────
            double tmpH = typInt < 11 ? 11.0 : 13.5;
            kv["SumH"] = "(H) " + Fmt(tmpH);
            kv["SumHTol"] = "+ 0.7 [3]";
            kv["SumHTolN"] = " 0 [3]";

            // ── Gänga / Nippel ────────────────────────────────────────────────────
            string gnBm = GetString(bm, "Gänga Nippel (Gn)");
            kv["SumGn"] = "(Gn) " + gnBm;
            kv["SumGn2"] = "(Gn2) " + gnBm;
            kv["SumGn3"] = "(Gn2) " + gnBm;

            string borrdjupN1 = GetString(bm, "Borrdjup N1");
            kv["SumGnB"] = (string.IsNullOrEmpty(borrdjupN1) || EqualsI(borrdjupN1, "0"))
                ? "Genomgående borrhål"
                : "(N1) " + borrdjupN1 + " ± 0.5";
            kv["SumGnd"] = "min 7";

            string bgnRaw = GetString(bm, "Borrdjup Nippelhål (BGn)");
            string tmpBGn = (bgnRaw ?? "").Replace(",", ".");
            bool bgnZero = string.IsNullOrEmpty(tmpBGn) || EqualsI(tmpBGn, "0");
            kv["SumBGn"] = bgnZero ? "Genomgående borrhål" : "(BGn) " + tmpBGn;
            kv["SumBGnTol"] = bgnZero ? "" : "± 0.5";

            kv["SumGh"] = (Math.Abs(tmpH - 11) < 0.001 ? "M10 - 6H" : "M12 - 6H") + " (2X)";

            // ── Stift borrdjup S1/S2 ──────────────────────────────────────────────
            kv["SumS1"] = "(S1) 7.7"; kv["SumS1Tol"] = " 0"; kv["SumS1TolN"] = "- 0.5";
            kv["SumS2"] = "(S2) 7.7"; kv["SumS2Tol"] = " 0"; kv["SumS2TolN"] = "- 0.5";

            // ── F (Fasdiameter) ───────────────────────────────────────────────────
            string fBm = GetString(bm, "Fasdiameter (F)");
            bool fZero = string.IsNullOrEmpty(fBm) || EqualsI(fBm, "0");
            string tmpF = fZero ? "2x45°" : fBm;
            kv["SumF"] = "(F) " + tmpF;
            kv["SumFTol"] = fZero ? "" : "+ 0.5";
            kv["SumFTolN"] = fZero ? "" : "- 0.8";

            // ── Td (Tätningsspårsdiameter) ────────────────────────────────────────
            kv["SumTd"] = "(Td) " + Fmt(tmpTd).Replace(".",",");
            kv["SumTdTol"] = TdTolStr(tmpTd) + " [3]";
            kv["SumTdTolN"] = " 0 [3]";

            // ── Tb (Tätningsspårsbredd) ───────────────────────────────────────────
            kv["SumTb"] = "(Tb) 5";
            kv["SumTbTol"] = "+ 0.18";
            kv["SumTbTolN"] = " 0";

            // ── Ta (Tätningsspårsavstånd) ─────────────────────────────────────────
            kv["SumTa"] = "(Ta) " + Fmt(tmpTa);
            kv["SumTaTol"] = GenTolStr(tmpTa);

            // ── LT (Lagerlägestoleranser) ─────────────────────────────────────────
            kv["TmpLT"] = typInt < 9 ? "+ 0.01" : (typInt < 15 ? "+ 0.012" : "+ 0.014");

            // ── Machine (key is [MV] not [MaskinVal]) ─────────────────────────────
            bool isLine1 = EqualsI(mv, "Line1");
            string tmpMaskinVal = isLine1 ? "" : "INGET MASKINVAL GJORD";
            string s1 = isLine1 ? "Line1" : "";
            kv["SumMaskinValS1"] = "Maskin: " + s1 + tmpMaskinVal + " - Övre halva OP10.20 (Förfräsning, Slipning)";
            kv["SumMaskinValS2"] = "Maskin: " + s1 + tmpMaskinVal + " - Undre halva OP10.20 (Förfräsning, Slipning)";
            kv["SumMaskinValS3"] = "Maskin: " + s1 + tmpMaskinVal + " - Övre halva OP30 (Borrning)";
            kv["SumMaskinValS4"] = "Maskin: " + s1 + tmpMaskinVal + " - Undre halva OP30 (Borrning)";
            kv["SumMaskinValS5"] = "Maskin: " + s1 + tmpMaskinVal + " - Sambearbetning OP40";

            SumFrequencies(kv, isLine1);
            SumMeasuringDevices(kv, isLine1, Fmt(tmpFa), tmpSd);
            SumAF(kv, isLine1, tmpHuhTolRitning, tmpSd);

            // ── Text ──────────────────────────────────────────────────────────────
            kv["SumTextS1"] = "Okulärbesiktning: Alla materialdefekter utsorteras, slipad märkyta 1ggr/tim";
            kv["SumTextS2"] = kv["SumTextS1"];
            kv["SumTextS3"] = "Okulärbesiktning: Alla materialdefekter utsorteras.<<LineBreak>>Kontrollparning 2ggr/skift, utvändigt inom 1.0mm & invändigt inom 0.1mm vid hopsatt hus.";
            kv["SumTextS4"] = "";
            kv["SumTextS5"] = "Kontrolleras med lagerlägestolk 1 ggr/skift med ihopdraget hus.<<LineBreak>>TB = Tekniska bestämmelser.<<LineBreak>>För tillägg V/VU se intruktion under fliken Specialhusgruppen";

            // ── Ritning ───────────────────────────────────────────────────────────
            string prdrit = "Produktritning: " + tmpBet + ":2";
            kv["SumPrdrit"] = prdrit;
            kv["SumPrdritS2"] = prdrit;
            kv["SumPrdritS3"] = prdrit;
            kv["SumPrdritS4"] = prdrit;
            kv["SumPrdritS5"] = prdrit;
            kv["SumArbInstGjg"] = "Gjutgods: A3.022";
            kv["SumArbInstGjgS2"] = "Gjutgods: A3.022";
            kv["SumArbInstGjgS3"] = "Gjutgods: A3.022";
            kv["SumArbInstGjgS4"] = "Gjutgods: A3.022";
            kv["SumArbInstGjgS5"] = "Gjutgods: A3.022";
            kv["SumKvStPl"] = "Kvalitetstyrning: K1.07-17";
            kv["SumKvStPlS2"] = "Kvalitetstyrning: K1.07-17";
            kv["SumKvStPlS3"] = "Kvalitetstyrning: K1.07-17";
            kv["SumKvStPlS4"] = "Kvalitetstyrning: K1.07-17";
            kv["SumKvStPlS5"] = "Kvalitetstyrning: K1.07-17";

            return kv;
        }

        // ── Popup ──────────────────────────────────────────────────────────────────
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
                       " dagar)<<LineBreak>>Popupruta aktiv till " +
                       validTill.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return "";
        }

        // ── Frequencies (5 pages) ────────────────────────────────────────────────
        private static void SumFrequencies(Dictionary<string, string> kv, bool m)
        {
            // Sid 1
            kv["SumF1_a"] = m ? "1/skift" : ""; kv["SumF1_b"] = m ? "Inst." : "";
            kv["SumF1_1"] = m ? "1/tim" : ""; kv["SumF1_2"] = m ? "1/tim" : "";
            kv["SumF1_3"] = m ? "1/tim" : ""; kv["SumF1_4"] = m ? "1/skift" : "";
            kv["SumF1_5"] = m ? "1/skift" : ""; kv["SumF1_6"] = m ? "1/tim" : "";
            kv["SumF1_7"] = m ? "1/tim" : ""; kv["SumF1_8"] = m ? "1/tim" : "";
            kv["SumF1_9"] = "";
            // Sid 2
            kv["SumF2_1"] = m ? "1/tim" : ""; kv["SumF2_2"] = m ? "1/tim" : "";
            kv["SumF2_3"] = m ? "1/tim" : ""; kv["SumF2_4"] = m ? "1/skift" : "";
            kv["SumF2_5"] = m ? "1/skift" : ""; kv["SumF2_6"] = m ? "1/tim" : "";
            kv["SumF2_7"] = m ? "1/tim" : ""; kv["SumF2_8"] = m ? "1/tim" : "";
            kv["SumF2_9"] = ""; kv["SumF2_0"] = "";
            kv["SumF2_11"] = m ? "1/tim" : ""; kv["SumF2_12"] = m ? "1/tim" : "";
            kv["SumF2_13"] = m ? "1/tim" : "";
            // Sid 3
            kv["SumF3_1"] = ""; kv["SumF3_2"] = "";
            kv["SumF3_3"] = m ? "Inst." : ""; kv["SumF3_4"] = m ? "Inst." : "";
            kv["SumF3_5"] = m ? "Inst." : ""; kv["SumF3_6"] = m ? "Inst." : "";
            kv["SumF3_7"] = m ? "Inst." : ""; kv["SumF3_8"] = "";
            // Sid 4
            kv["SumF4_1"] = m ? "Inst." : ""; kv["SumF4_2"] = m ? "2/skift" : "";
            kv["SumF4_3"] = m ? "Inst./Borrbyte" : ""; kv["SumF4_4"] = m ? "Inst./Borrbyte" : "";
            kv["SumF4_5"] = m ? "Inst." : ""; kv["SumF4_6"] = m ? "Inst." : "";
            // Sid 5
            kv["SumF5_1"] = m ? "1/1" : ""; kv["SumF5_2"] = m ? "1/tim" : "";
            kv["SumF5_3"] = "";
            kv["SumF5_4"] = m ? "1/skift" : ""; kv["SumF5_5"] = m ? "1/skift" : "";
            kv["SumF5_6"] = m ? "1/tim" : ""; kv["SumF5_7"] = m ? "1/tim" : "";
            kv["SumF5_8"] = m ? "1/tim" : ""; kv["SumF5_9"] = m ? "1/tim" : "";
            kv["SumF5_0"] = m ? "1/tim" : ""; kv["SumF5_11"] = m ? "1/tim" : "";
            kv["SumF5_12"] = m ? "Inst/skärb." : ""; kv["SumF5_13"] = m ? "1/skift" : "";
            kv["SumF5_14"] = m ? "4/skift" : ""; kv["SumF5_15"] = "";
            kv["SumF5_16"] = m ? "1/tim" : ""; kv["SumF5_17"] = m ? "1/skift" : "";
            kv["SumF5_18"] = m ? "4/skift/Inst." : "";
        }

        // ── Measuring devices (5 pages) ──────────────────────────────────────────
        private static void SumMeasuringDevices(Dictionary<string, string> kv, bool m, string fa, string sd)
        {
            // Sid 1
            kv["SumD1_a"] = m ? "Gängtolk" : ""; kv["SumD1_b"] = m ? "Skjutmått" : "";
            kv["SumD1_1"] = m ? "Skjutmått" : ""; kv["SumD1_2"] = m ? "Skjutmått" : "";
            kv["SumD1_3"] = m ? "Skjutmått" : ""; kv["SumD1_4"] = m ? "Skjutmått" : "";
            kv["SumD1_5"] = m ? "Skjutmått" : ""; kv["SumD1_6"] = m ? "Djupmått" : "";
            kv["SumD1_7"] = m ? "Skjutmått" : ""; kv["SumD1_8"] = m ? "Kännbleck" : "";
            kv["SumD1_9"] = "";
            // Sid 2
            kv["SumD2_1"] = m ? "Skjutmått" : ""; kv["SumD2_2"] = m ? "Skjutmått" : "";
            kv["SumD2_3"] = m ? "Skjutmått" : ""; kv["SumD2_4"] = m ? "Skjutmått" : "";
            kv["SumD2_5"] = m ? "Skjutmått" : ""; kv["SumD2_6"] = m ? "Djupmått" : "";
            kv["SumD2_7"] = m ? "Djupmått" : ""; kv["SumD2_8"] = m ? "Passbit " + fa.Replace(".",",") : "";
            kv["SumD2_9"] = ""; kv["SumD2_0"] = "";
            kv["SumD2_11"] = m ? "Skjutmått" : ""; kv["SumD2_12"] = m ? "Kännbleck" : "";
            kv["SumD2_13"] = m ? "Kännbleck" : "";
            // Sid 3
            kv["SumD3_1"] = ""; kv["SumD3_2"] = "";
            kv["SumD3_3"] = m ? "Centrumavståndsmätare" : "";
            kv["SumD3_4"] = m ? "Tolk" : "";
            kv["SumD3_5"] = m ? "Skjutmått" : "";
            kv["SumD3_6"] = m ? "Skjutmått" : "";
            kv["SumD3_7"] = m ? "Centrumavståndsmätare" : "";
            kv["SumD3_8"] = "";
            // Sid 4
            kv["SumD4_1"] = m ? "Centrumavståndsmätare" : "";
            kv["SumD4_2"] = m ? "Tolk/hålindikator" : "";
            kv["SumD4_3"] = m ? "Skjutmått" : "";
            kv["SumD4_4"] = m ? "Skjutmått" : "";
            kv["SumD4_5"] = m ? "Gängtolk/Tolk kärndiameter" : "";
            kv["SumD4_6"] = m ? "Centrumavståndsmätare" : "";
            // Sid 5
            kv["SumD5_1"] = m ? "Subito/hålindikator/*M" : "";
            kv["SumD5_2"] = m ? "Skjutmått/*M" : "";
            kv["SumD5_3"] = "";
            kv["SumD5_6"] = m ? "Axelhåltolk 67 H12/*M" : "";
            kv["SumD5_7"] = m ? "Skjutmått/*M" : "";
            kv["SumD5_8"] = m ? "Skjutmått/*M/Tolk" : "";
            kv["SumD5_9"] = m ? "Skjutmått" : "";
            kv["SumD5_0"] = m ? "Skjutmått" : "";
            kv["SumD5_11"] = m ? "Skjutmått" : "";
            kv["SumD5_12"] = m ? "Skjutmått" : "";
            kv["SumD5_13"] = m ? "Skjutmått" : "";
            kv["SumD5_14"] = m ? "min bricka" : "";
            kv["SumD5_15"] = "";
            kv["SumD5_16"] = m ? "Kännbleck" : "";
            kv["SumD5_17"] = m ? "Ytjämnhetsmätare" : "";
            kv["SumD5_18"] = m ? "Mätmaskin" : "";
        }

        // ── AF (5 pages) ─────────────────────────────────────────────────────────
        private static void SumAF(Dictionary<string, string> kv, bool m, string huhTolRit, string sd)
        {
            // Sid 1
            kv["SumAF1_a"] = m ? "Gängdjup: min 7mm" : "";
            kv["SumAF1_b"] = m ? "(Gn2) Genomgående borrhål. Vid inst. lägeskontroll av hålen (Gn) & (Gn2) i mätrum" : "";
            kv["SumAF1_1"] = m ? "Digitalt" : ""; kv["SumAF1_2"] = m ? "Digitalt" : "";
            kv["SumAF1_3"] = m ? "Digitalt" : "";
            kv["SumAF1_4"] = m ? "Lika inom 0.15 & 0.10 emellan kyrka 1 & 2" : "";
            kv["SumAF1_5"] = m ? "Lika inom 0.15 & 0.10 emellan kyrka 1 & 2" : "";
            kv["SumAF1_6"] = m ? "Digitalt" : "";
            kv["SumAF1_7"] = m ? "Lika inom 0.5" : "";
            kv["SumAF1_8"] = m ? "lös halva mot planskiva" : "";
            kv["SumAF1_9"] = "";
            // Sid 2
            kv["SumAF2_1"] = m ? "Digital" : ""; kv["SumAF2_2"] = m ? "Digital" : "";
            kv["SumAF2_3"] = m ? "Digital" : "";
            kv["SumAF2_4"] = m ? "Lika inom 0.15 & 0.10 emellan kyrka 1 & 2" : "";
            kv["SumAF2_5"] = m ? "Lika inom 0.15 & 0.10 emellan kyrka 1 & 2" : "";
            kv["SumAF2_6"] = m ? "Digital" : "";
            kv["SumAF2_7"] = m ? "Ritningsmått: " + huhTolRit : "";
            kv["SumAF2_8"] = m ? "Indikator Klocka med magnetisk fot" : "";
            kv["SumAF2_9"] = ""; kv["SumAF2_0"] = "";
            kv["SumAF2_11"] = m ? "Max avvikelse mellan 4 hörn: 0.5" : "";
            kv["SumAF2_12"] = m ? "lös halva mot planskiva" : "";
            kv["SumAF2_13"] = m ? "Omonterat & monterat" : "";
            // Sid 3
            kv["SumAF3_1"] = ""; kv["SumAF3_2"] = "";
            kv["SumAF3_3"] = m ? "Max skillnad mellan ÖH & UH ± 0.015 [2P]" : "";
            kv["SumAF3_4"] = "";
            kv["SumAF3_5"] = m ? "Digitalt" : ""; kv["SumAF3_6"] = m ? "Digitalt" : "";
            kv["SumAF3_7"] = ""; kv["SumAF3_8"] = "";
            // Sid 4
            kv["SumAF4_1"] = m ? "Max skillnad mellan ÖH & UH ± 0.015 [2P]" : "";
            kv["SumAF4_2"] = m ? "Inställningsring " + sd : "";
            kv["SumAF4_3"] = m ? "Digitalt" : ""; kv["SumAF4_4"] = m ? "Digitalt" : "";
            kv["SumAF4_5"] = m ? "Gängan genomgående prov med skruv." : "";
            kv["SumAF4_6"] = "";
            // Sid 5
            kv["SumAF5_1"] = m ? "Metem 551 / Inst. ring min-max" : "";
            kv["SumAF5_2"] = m ? "Digital" : "";
            kv["SumAF5_3"] = "";
            kv["SumAF5_6"] = "";
            kv["SumAF5_7"] = m ? "Digitalt" : ""; kv["SumAF5_8"] = m ? "Digitalt" : "";
            kv["SumAF5_9"] = m ? "Digitalt" : "";
            kv["SumAF5_0"] = m ? "UH = inom 0.35, 10mm från hopläggningsplan" : "";
            kv["SumAF5_11"] = m ? "Digitalt" : ""; kv["SumAF5_12"] = m ? "Digitalt" : "";
            kv["SumAF5_13"] = m ? "Lika inom 0.2" : "";
            kv["SumAF5_14"] = m ? "Lossat hus från balk" : "";
            kv["SumAF5_15"] = "";
            kv["SumAF5_16"] = m ? "Gör enligt TB, kontrollera 10 hus i rad." : "";
            kv["SumAF5_17"] = m ? "Kontrollera sista hus vid skärbyte." : "";
            kv["SumAF5_18"] = m ? "4x/maskin" : "";
        }

        // ── Tolerance tables ─────────────────────────────────────────────────────

        private static double G7Pos(double v)
        {
            if (v < 3.01) return 0.012; if (v < 6.01) return 0.016; if (v < 10.01) return 0.020;
            if (v < 18.01) return 0.024; if (v < 30.01) return 0.028; if (v < 50.01) return 0.034;
            if (v < 80.01) return 0.040; if (v < 120.01) return 0.047; if (v < 180.01) return 0.054;
            if (v < 250.01) return 0.061; if (v < 315.01) return 0.069; if (v < 400.01) return 0.075;
            if (v < 500.01) return 0.083; if (v < 630.01) return 0.092; if (v < 800.01) return 0.104;
            if (v < 1000.01) return 0.116; if (v < 1250.01) return 0.133; if (v < 1600.01) return 0.155;
            if (v < 2000.01) return 0.182; if (v < 2500.01) return 0.209; return 0.248;
        }

        private static double G7Neg(double v)
        {
            if (v < 3.01) return 0.002; if (v < 6.01) return 0.004; if (v < 10.01) return 0.005;
            if (v < 18.01) return 0.006; if (v < 30.01) return 0.007; if (v < 50.01) return 0.009;
            if (v < 80.01) return 0.010; if (v < 120.01) return 0.012; if (v < 180.01) return 0.014;
            if (v < 250.01) return 0.015; if (v < 315.01) return 0.017; if (v < 400.01) return 0.018;
            if (v < 500.01) return 0.020; if (v < 630.01) return 0.022; if (v < 800.01) return 0.024;
            if (v < 1000.01) return 0.026; if (v < 1250.01) return 0.028; if (v < 1600.01) return 0.030;
            if (v < 2000.01) return 0.032; if (v < 2500.01) return 0.034; return 0.038;
        }

        private static string H12TolStr(double v)
        {
            if (v < 3.01) return "+ 0.100"; if (v < 6.01) return "+ 0.120"; if (v < 10.01) return "+ 0.150";
            if (v < 18.01) return "+ 0.180"; if (v < 30.01) return "+ 0.210"; if (v < 50.01) return "+ 0.250";
            if (v < 80.01) return "+ 0.300"; if (v < 120.01) return "+ 0.350"; if (v < 180.01) return "+ 0.400";
            if (v < 250.01) return "+ 0.460"; if (v < 315.01) return "+ 0.520"; if (v < 400.01) return "+ 0.570";
            if (v < 500.01) return "+ 0.630"; if (v < 630.01) return "+ 0.700"; if (v < 800.01) return "+ 0.800";
            if (v < 1000.01) return "+ 0.900"; if (v < 1250.01) return "+ 1.050"; if (v < 1600.01) return "+ 1.250";
            if (v < 2000.01) return "+ 1.500"; if (v < 2500.01) return "+ 1.750"; return "+ 2.100";
        }

        private static string JS13TolStr(double v)
        {
            if (v < 3.01) return "± 0.070"; if (v < 6.01) return "± 0.090"; if (v < 10.01) return "± 0.110";
            if (v < 18.01) return "± 0.135"; if (v < 30.01) return "± 0.165"; if (v < 50.01) return "± 0.195";
            if (v < 80.01) return "± 0.230"; if (v < 120.01) return "± 0.270"; if (v < 180.01) return "± 0.315";
            if (v < 250.01) return "± 0.360"; if (v < 315.01) return "± 0.405"; if (v < 400.01) return "± 0.445";
            if (v < 500.01) return "± 0.485"; if (v < 630.01) return "± 0.550"; if (v < 800.01) return "± 0.625";
            if (v < 1000.01) return "± 0.700"; if (v < 1250.01) return "± 0.825"; if (v < 1600.01) return "± 0.975";
            if (v < 2000.01) return "± 1.150"; if (v < 2500.01) return "± 1.400"; return "± 1.650";
        }

        private static string JS11TolStr(double v)
        {
            if (v < 3.01) return "± 0.030"; if (v < 6.01) return "± 0.037"; if (v < 10.01) return "± 0.045";
            if (v < 18.01) return "± 0.055"; if (v < 30.01) return "± 0.065"; if (v < 50.01) return "± 0.080";
            if (v < 80.01) return "± 0.095"; if (v < 120.01) return "± 0.110"; if (v < 180.01) return "± 0.125";
            if (v < 250.01) return "± 0.145"; if (v < 315.01) return "± 0.160"; if (v < 400.01) return "± 0.180";
            if (v < 500.01) return "± 0.200"; if (v < 630.01) return "± 0.220"; if (v < 800.01) return "± 0.250";
            if (v < 1000.01) return "± 0.280"; if (v < 1250.01) return "± 0.330"; if (v < 1600.01) return "± 0.390";
            if (v < 2000.01) return "± 0.460"; if (v < 2500.01) return "± 0.550"; return "± 0.675";
        }

        // Td H12 — stops at 0.900 (no further steps beyond 800)
        private static string TdTolStr(double v)
        {
            if (v < 3.01) return "+ 0.100"; if (v < 6.01) return "+ 0.120"; if (v < 10.01) return "+ 0.150";
            if (v < 18.01) return "+ 0.180"; if (v < 30.01) return "+ 0.210"; if (v < 50.01) return "+ 0.250";
            if (v < 80.01) return "+ 0.300"; if (v < 120.01) return "+ 0.350"; if (v < 180.01) return "+ 0.400";
            if (v < 250.01) return "+ 0.460"; if (v < 315.01) return "+ 0.520"; if (v < 400.01) return "+ 0.570";
            if (v < 500.01) return "+ 0.630"; if (v < 630.01) return "+ 0.700"; if (v < 800.01) return "+ 0.800";
            return "+ 0.900";
        }

        private static string GenTolStr(double v)
        {
            if (v < 6.01) return "± 0.1"; if (v < 30.01) return "± 0.2"; if (v < 120.01) return "± 0.3";
            if (v < 400.01) return "± 0.5"; if (v < 1000.01) return "± 0.8"; if (v < 2000.01) return "± 1.2";
            return "± 2.0";
        }

        // ── Utilities ────────────────────────────────────────────────────────────
        private static bool EqualsI(string a, string b)
            => string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);
        private static int TryParseInt(string s)
        { int v; return int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0; }
        private static double GetDouble(List<Bookmark> bm, string key)
        { string r = GetString(bm, key); if (string.IsNullOrWhiteSpace(r)) return 0; double v; return double.TryParse(r.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out v) ? v : 0; }
        private static string GetString(List<Bookmark> bm, string key)
        { if (bm == null || string.IsNullOrWhiteSpace(key)) return ""; for (int i = 0; i < bm.Count; i++) { Bookmark b = bm[i]; if (b != null && string.Equals(b.BookmarkName ?? "", key, StringComparison.OrdinalIgnoreCase)) return b.BookmarkValue ?? ""; } return ""; }
        private static string Fmt(double v) => v.ToString(CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt2(double v) => v.ToString("F2", CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt3(double v) => v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
    }
}