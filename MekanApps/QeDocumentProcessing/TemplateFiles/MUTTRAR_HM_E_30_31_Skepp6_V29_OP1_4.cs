using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class MUTTRAR_HM_E_30_31_Skepp6_V29_OP1_4 : ITemplateCalculations
    {
        private static readonly int[] SupportedTypes =
        {
            44, 48, 52, 56, 60, 64, 68, 72, 76, 80, 84, 88, 92, 96,
            500, 530, 560, 600, 630, 670, 710, 750, 800, 850, 900,
            950, 1000, 1060, 1120, 1180, 1250
        };

        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var kv = new Dictionary<string, string>();
            kv["DocumentUniqueId"] = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);

            string subjectInput = (request.ProductDesignation ?? string.Empty).Trim();
            string machineInput = (request.MachineNumber ?? string.Empty).Trim();
            ParseInput(subjectInput, machineInput, out string subject, out string machine);

            Merge(kv, GetAdmin(request, subject));
            Merge(kv, GetCalculatedBlock(subject, machine));

            return kv;
        }

        private Dictionary<string, string> GetAdmin(APIRequest req, string subject)
        {
            var kv = new Dictionary<string, string>();
            kv["Subject"] = subject;
            kv["Version"] = req.Version ?? "V29";
            kv["Published"] = req.Published ?? DateTime.UtcNow.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            kv["Created"] = DateTime.UtcNow.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            kv["Approved"] = req.ApprovedBy ?? string.Empty;
            kv["CreatedBy"] = req.CreatedBy ?? string.Empty;
            kv["ApprovedBy"] = req.ApprovedBy ?? string.Empty;
            kv["CategoryHierarchy"] = req.CategoryHierarchy ?? string.Empty;
            return kv;
        }

        private Dictionary<string, string> GetCalculatedBlock(string subject, string machine)
        {
            var kv = new Dictionary<string, string>();
            string tmpFormat = (subject ?? string.Empty).Trim().ToUpperInvariant();
            string tmpBet = tmpFormat.Replace('.', ',');

            string[] parts = Regex.Split(tmpBet, "[ /.-]+").Where(p => !string.IsNullOrWhiteSpace(p)).ToArray();
            string bet1 = parts.Length > 0 ? parts[0] : string.Empty;
            string bet2 = parts.Length > 1 ? parts[1] : string.Empty;
            string bet3 = parts.Length > 2 ? parts[2] : string.Empty;
            string bet4 = parts.Length > 3 ? parts[3] : string.Empty;
            string bet5 = parts.Length > 4 ? parts[4] : string.Empty;
            string bet6 = parts.Length > 5 ? parts[5] : string.Empty;

            bool isHM = bet1 == "HM";
            bool isHME = bet1 == "HME" || (bet1 == "HM" && subject.Contains("HM E", StringComparison.OrdinalIgnoreCase));
            if (!isHM && !isHME && tmpFormat.StartsWith("HME", StringComparison.OrdinalIgnoreCase))
            {
                isHME = true;
            }

            int lenBet2 = bet2.Length;
            int serie = ParseIntSafe(Left(bet2, 2));
            int typ = lenBet2 > 3 ? ParseIntSafe(Mid(bet2, 2, 2)) : ParseIntSafe(bet3);
            int typIndex = Array.IndexOf(SupportedTypes, typ) + 1;

            decimal kd = lenBet2 > 3 ? (typ / 2m) * 10m : ParseDecimalSafe(bet3);
            int p = typ < 61 ? 4 : typ < 501 ? 5 : typ < 671 ? 6 : typ < 901 ? 7 : 8;

            kv["TmpFormat"] = tmpFormat;
            kv["TmpBet"] = tmpBet;
            kv["TmpBet1"] = bet1;
            kv["TmpBet2"] = bet2;
            kv["TmpBet3"] = bet3;
            kv["TmpBet4"] = bet4;
            kv["TmpBet5"] = bet5;
            kv["TmpBet6"] = bet6;
            kv["TmpSerie"] = serie > 0 ? serie.ToString(CultureInfo.InvariantCulture) : string.Empty;
            kv["TmpTyp"] = typ > 0 ? typ.ToString(CultureInfo.InvariantCulture) : string.Empty;
            kv["TmpV29"] = tmpBet.Contains("V29", StringComparison.OrdinalIgnoreCase) ? "1" : "0";

            kv["SumP"] = $"(P) {p}";
            kv["SumGänga"] = $"Tr {FmtDec(kd)}x{p}";
            kv["SumGMått"] = kv["SumGänga"];
            kv["SumGMall"] = $"Tr x {p}";

            kv["SumF3"] = isHM ? "30º" : string.Empty;
            kv["SumF3a"] = isHME ? "30º" : string.Empty;
            kv["SumF32"] = isHM ? "30º" : string.Empty;
            kv["SumF3a2"] = isHME ? "30º" : string.Empty;
            kv["SumGF"] = "30º";

            kv["SumF1"] = isHM ? "45º" : string.Empty;
            kv["SumF2"] = isHM ? "45º" : string.Empty;
            kv["SumF1a"] = isHME ? "45º" : string.Empty;
            kv["SumF2a"] = isHME ? "45º" : string.Empty;
            kv["SumF1S3"] = isHM ? "45º" : string.Empty;
            kv["SumF2S3"] = isHM ? "45º" : string.Empty;
            kv["SumF1S3a"] = isHME ? "45º" : string.Empty;
            kv["SumF2S3a"] = isHME ? "45º" : string.Empty;

            decimal r = kd < 221 ? 2.5m : kd < 281 ? 3m : kd < 441 ? 3.5m : kd < 601 ? 4m : kd < 711 ? 5m : 6m;
            string rText = "R " + FmtDecDot(r);
            kv["SumR"] = isHM ? rText : string.Empty;
            kv["SumRa"] = isHME ? rText : string.Empty;
            kv["SumR3b"] = isHM ? kv["SumR"] : string.Empty;
            kv["SumR3ba"] = isHME ? kv["SumR"] : string.Empty;
            kv["SumRHT"] = "R1.6";

            decimal kda = (p == 4 || p == 5) ? kd + 0.5m : kd + 1m;
            decimal kdTol = kd < 6 ? 0.1m : kd < 30 ? 0.2m : kd < 120 ? 0.3m : kd < 400 ? 0.5m : kd < 1000 ? 0.8m : kd < 2000 ? 1.2m : 2m;
            kv["Sumkd"] = $"(kd) {FmtDec(kda)}";
            kv["SumkdTol"] = "± " + FmtDecDot(kdTol);

            decimal dm = p == 4 ? kd - 2m : p == 5 ? kd - 2.5m : p == 6 ? kd - 3m : p == 7 ? kd - 3.5m : kd - 4m;
            decimal dmTol = kd < 301 ? 0.475m : kd < 501 ? 0.530m : kd < 701 ? 0.600m : kd < 901 ? 0.630m : 0.710m;
            kv["Sumdm"] = $"(dm) {FmtDec(dm).Replace(".", ",")}";
            kv["SumdmTol"] = "+ " + FmtDecDotKeep3(dmTol);
            kv["SumdmTolN"] = "- 0";

            decimal id = p == 4 ? kd - 4m : p == 5 ? kd - 5m : p == 6 ? kd - 6m : p == 7 ? kd - 7m : kd - 8m;
            decimal idTol = kd < 301 ? 0.375m : kd < 501 ? 0.45m : kd < 701 ? 0.5m : kd < 901 ? 0.56m : 0.63m;
            decimal idSsk = id - 1m;
            kv["Sumid"] = $"(id) {FmtDec(id)}";
            kv["SumidTol"] = "+ " + FmtDecDotKeep3(idTol);
            kv["SumidTolN"] = "- 0";
            kv["SumidSSK"] = isHM ? $"(id) {FmtDec(idSsk)}" : string.Empty;
            kv["SumidSSKa"] = isHME ? $"(id) {FmtDec(idSsk)}" : string.Empty;

            decimal d = CalcD(serie, kd);
            decimal dTolN = TolH11(d);
            kv["SumD"] = isHM ? $"(D) {FmtDec(d)}" : string.Empty;
            kv["SumDa"] = isHME ? $"(D) {FmtDec(d)}" : string.Empty;
            kv["SumDTol"] = isHM ? "+ 0" : string.Empty;
            kv["SumDTolN"] = isHM ? "- " + FmtDecDotKeep2orMore(dTolN) : string.Empty;
            kv["SumDaTol"] = isHME ? "+ 0" : string.Empty;
            kv["SumDaTolN"] = isHME ? "- " + FmtDecDotKeep2orMore(dTolN) : string.Empty;

            decimal d1Hm = CalcD1HM(serie, kd, d);
            decimal d1Hme = GetArrayValueDecimal(GetD1HMEList(serie), typIndex);
            decimal d1 = isHM ? d1Hm : d1Hme;
            decimal d1TolN = TolGeneralShaft(d1);
            kv["SumD1"] = isHM ? $"(D1) {FmtDec(d1)}" : string.Empty;
            kv["SumD1S1"] = kv["SumD1"];
            kv["SumD1a"] = isHME ? $"(D1) {FmtDec(d1)}" : string.Empty;
            kv["SumD1aS1"] = kv["SumD1a"];
            kv["SumD1Tol"] = isHM ? "+ 0" : string.Empty;
            kv["SumD1TolN"] = isHM ? "- " + FmtDecDotKeep2orMore(d1TolN) : string.Empty;
            kv["SumD1aTol"] = isHME ? "+ 0" : string.Empty;
            kv["SumD1aTolN"] = isHME ? "- " + FmtDecDotKeep2orMore(d1TolN) : string.Empty;
            kv["SumD1S1Tol"] = kv["SumD1Tol"];
            kv["SumD1S1TolN"] = kv["SumD1TolN"];
            kv["SumD1aS1Tol"] = kv["SumD1aTol"];
            kv["SumD1aS1TolN"] = kv["SumD1aTolN"];

            decimal hm = (d - d1) / 2m;
            kv["SumHM"] = isHM ? $"(HM) {FmtDec(hm)}" : string.Empty;
            kv["SumHMa"] = isHME ? $"(HM) {FmtDec(hm)}" : string.Empty;

            decimal d3 = kd < 501 ? kd + 2m : kd + 3m;
            decimal d3Tol = TolDiamInternal(d3);
            decimal d3Ssk = d3 + 2m;
            kv["SumD3"] = isHM ? $"(D3) {FmtDec(d3)}" : string.Empty;
            kv["SumD3S1"] = kv["SumD3"];
            kv["SumD3a"] = isHME ? $"(D3) {FmtDec(d3)}" : string.Empty;
            kv["SumD3aS1"] = kv["SumD3a"];
            kv["SumD3Tol"] = isHM ? "+ " + FmtDecDotKeep2orMore(d3Tol) : string.Empty;
            kv["SumD3TolN"] = isHM ? "- 0" : string.Empty;
            kv["SumD3aTol"] = isHME ? "+ " + FmtDecDotKeep2orMore(d3Tol) : string.Empty;
            kv["SumD3aTolN"] = isHME ? "- 0" : string.Empty;
            kv["SumD3S1Tol"] = kv["SumD3Tol"];
            kv["SumD3S1TolN"] = kv["SumD3TolN"];
            kv["SumD3aS1Tol"] = kv["SumD3aTol"];
            kv["SumD3aS1TolN"] = kv["SumD3aTolN"];
            kv["SumD3SSK"] = isHM ? $"(D3) {FmtDec(d3Ssk)}" : string.Empty;
            kv["SumD3SSKa"] = isHME ? $"(D3) {FmtDec(d3Ssk)}" : string.Empty;

            string bRaw = GetArrayValue(GetBList(serie), typIndex);
            decimal b = ParseDecimalSafe(bRaw);
            decimal bTolN = TolWidth(b);
            kv["SumB"] = isHM && !string.IsNullOrWhiteSpace(bRaw) ? $"(B) {FmtDec(b)}" : string.Empty;
            kv["SumBa"] = isHME && !string.IsNullOrWhiteSpace(bRaw) ? $"(B) {FmtDec(b)}" : string.Empty;
            kv["SumBTol"] = isHM && !string.IsNullOrWhiteSpace(bRaw) ? "+ 0" : string.Empty;
            kv["SumBTolN"] = isHM && !string.IsNullOrWhiteSpace(bRaw) ? "- " + FmtDecDotKeep2orMore(bTolN) : string.Empty;
            kv["SumBaTol"] = isHME && !string.IsNullOrWhiteSpace(bRaw) ? "+ 0" : string.Empty;
            kv["SumBaTolN"] = isHME && !string.IsNullOrWhiteSpace(bRaw) ? "- " + FmtDecDotKeep2orMore(bTolN) : string.Empty;
            kv["SumBSSK"] = isHM && !string.IsNullOrWhiteSpace(bRaw) ? $"(B) {FmtDec(b)}" : string.Empty;
            kv["SumBSSKa"] = isHME && !string.IsNullOrWhiteSpace(bRaw) ? $"(B) {FmtDec(b)}" : string.Empty;

            string tRaw = GetArrayValue(GetTList(serie), typIndex);
            decimal t = ParseDecimalSafe(tRaw);
            decimal tTol = t < 6.1m ? 0.1m : t < 30.1m ? 0.2m : 0.3m;
            decimal tSsk = t + (bTolN / 2m);
            kv["SumT"] = isHME && !string.IsNullOrWhiteSpace(tRaw) ? $"(T) {FmtDec(t).Replace(".", ",")}" : string.Empty;
            kv["SumTTol"] = isHME && !string.IsNullOrWhiteSpace(tRaw) ? "± " + FmtDecDot(tTol) : string.Empty;
            kv["SumTSSK"] = isHME && !string.IsNullOrWhiteSpace(tRaw) ? $"(T) {FmtDec(tSsk).Replace(".", ",")}" : string.Empty;

            kv["SumRa32"] = isHM ? "3.2" : string.Empty;
            kv["SumRa32a"] = isHME ? "3.2" : string.Empty;

            decimal ka = kd < 51 ? 0.04m : kd < 121 ? 0.05m : kd < 251 ? 0.06m : kd < 316 ? 0.07m : kd < 401 ? 0.08m : kd < 501 ? 0.09m : kd < 631 ? 0.10m : kd < 801 ? 0.12m : kd < 1001 ? 0.14m : 0.16m;
            kv["SumKa"] = FmtDecDotKeep2orMore(ka);

            string b2Raw = isHME ? GetArrayValue(GetB2List(serie), typIndex) : FmtDec(b / 2m);
            decimal b2 = ParseDecimalSafe(b2Raw);
            kv["SumB2"] = isHM && b2 != 0 ? $"(X) {FmtDec(b2)}" : string.Empty;
            kv["SumB2a"] = isHME && b2 != 0 ? $"(X) {FmtDec(b2)}" : string.Empty;

            int gg = serie == 30 ? (kd < 671 ? 10 : kd < 901 ? 12 : 16) : (kd < 531 ? 10 : kd < 601 ? 12 : kd < 751 ? 16 : kd < 901 ? 20 : 24);
            string ggText = b2 == 0 ? string.Empty : $"Gänga: M{gg}";
            decimal l = b2 == 0 ? 0 : (gg == 10 ? 17 : gg == 12 ? 21 : 27);
            kv["SumGG"] = isHM ? ggText : string.Empty;
            kv["SumGGa"] = isHME ? ggText : string.Empty;
            kv["SumL"] = isHM && b2 != 0 ? $"(L) {FmtDec(l)}" : string.Empty;
            kv["SumLa"] = isHME && b2 != 0 ? $"(L) {FmtDec(l)}" : string.Empty;
            kv["SumGGTolk"] = b2 == 0 ? string.Empty : $" kontrolleras med min/max tolk M{gg}";

            decimal ta = CalcTa(serie, kd);
            decimal taTol = ta < 11 ? 1.5m : ta < 19 ? 1.8m : ta < 31 ? 2.1m : 2.5m;
            kv["Sumta"] = $"(t) {FmtDec(ta)}";
            kv["SumtaTol"] = "+ " + FmtDecDot(taTol);
            kv["SumtaTolN"] = "- 0";

            decimal s = CalcS(serie, kd);
            kv["SumS"] = $"(S) {FmtDec(s)}";
            kv["SumSTol"] = s < 31 ? "± 0.260" : s < 51 ? "± 0.310" : "± 0.370";

            string lr = s < 7 ? "0.5" : s < 10 ? "0.75" : s < 19 ? "1.0" : s < 31 ? "1.25" : "1.5";
            kv["SumLR"] = lr;

            string d4Raw = GetArrayValue(GetD4List(serie), typIndex);
            decimal d4 = ParseDecimalSafe(d4Raw);
            kv["Sumd4"] = isHM && !string.IsNullOrWhiteSpace(d4Raw) ? $"(d4) {FmtDec(d4)}" : string.Empty;
            kv["Sumd4a"] = isHME && !string.IsNullOrWhiteSpace(d4Raw) ? $"(d4) {FmtDec(d4)}" : string.Empty;
            kv["Sumd4Tol"] = isHM && !string.IsNullOrWhiteSpace(d4Raw) ? "± 0.4" : string.Empty;
            kv["Sumd4aTol"] = isHME && !string.IsNullOrWhiteSpace(d4Raw) ? "± 0.4" : string.Empty;

            kv["SumVB"] = "22.5º (16x)";
            kv["SumVH"] = "11.25º";

            int u = serie == 30 ? (kd < 559 ? 12 : 16) : 16;
            int v = u == 12 ? 27 : 35;
            int l3 = u == 12 ? 31 : 40;
            kv["Sumu"] = isHM ? $"M{u}" : string.Empty;
            kv["Sumua"] = isHME ? $"M{u}" : string.Empty;
            kv["Sumv"] = isHM ? $"(v) min:{v}" : string.Empty;
            kv["Sumva"] = isHME ? $"(v) min:{v}" : string.Empty;
            kv["SumL3"] = isHM ? $"(L3) {l3}" : string.Empty;
            kv["SumL3a"] = isHME ? $"(L3) {l3}" : string.Empty;
            kv["SumGuTolk"] = $"min/max";

            kv["SumStämpling"] = "Stämplas enl. ritning 7430189";
            kv["SumTextS1"] = "Grader, frifläckar, slagmärken, repor, valkar och andra defekter samt ojämnheter kontrolleras med ögonmått <<LineBreak>> För övriga mått används skjutmått.";
            kv["SumTextS2"] = "Övriga mått kontrolleras vid inställning <<LineBreak>>" + kv["SumTextS1"];
            kv["SumTextS3"] = "Kontrollera Grader, frifläckar, slagmärken, repor, valkar och andra defekter <<LineBreak>> Vid misstänkt formfel lämnas muttern till mätrum";

            string normalizedMachine = NormalizeMachine(machine, subject);
            string tmpMaskinVal = normalizedMachine == "Skepp6" ? string.Empty : string.Empty;
            string tmpMaskinValS1 = normalizedMachine == "Skepp6" ? "Morando" : string.Empty;
            string tmpMaskinValS2 = normalizedMachine == "Skepp6" ? "K&T" : string.Empty;
            string tmpMaskinValS3 = normalizedMachine == "Skepp6" ? "1150" : string.Empty;
            kv["SumMaskinValS1"] = $"Maskin: {tmpMaskinValS1}{tmpMaskinVal} - OP1 & 2";
            kv["SumMaskinValS2"] = $"Maskin: {tmpMaskinValS2}{tmpMaskinVal} - OP3";
            kv["SumMaskinValS3"] = $"Maskin: {tmpMaskinValS3}{tmpMaskinVal} - OP4";

            SetRepeated(kv, "SumF1_", 9, normalizedMachine == "Skepp6" ? "1/1" : string.Empty);
            SetRepeated(kv, "SumF2_", 4, normalizedMachine == "Skepp6" ? "1/1" : string.Empty);
            SetRepeated(kv, "SumF3_", 8, normalizedMachine == "Skepp6" ? "1/1" : string.Empty);
            kv["SumF3_9"] = normalizedMachine == "Skepp6" ? "-" : string.Empty;
            kv["SumF3_0"] = normalizedMachine == "Skepp6" ? "1/1" : string.Empty;

            kv["SumD1_1"] = normalizedMachine == "Skepp6" ? "Skjutmått" : string.Empty;
            kv["SumD1_2"] = normalizedMachine == "Skepp6" ? "Mikrometer" : string.Empty;
            kv["SumD1_3"] = normalizedMachine == "Skepp6" ? "Djupmått/Skjutmått" : string.Empty;
            kv["SumD1_4"] = normalizedMachine == "Skepp6" ? "Skjutmått" : string.Empty;
            kv["SumD1_5"] = normalizedMachine == "Skepp6" ? "Djupmått/Skjutmått" : string.Empty;
            kv["SumD1_6"] = normalizedMachine == "Skepp6" ? "Fasmall/Skjutmått" : string.Empty;
            kv["SumD1_7"] = normalizedMachine == "Skepp6" ? "Radiestål" : string.Empty;
            kv["SumD1_8"] = normalizedMachine == "Skepp6" ? "Djupmått/Skjutmått" : string.Empty;
            kv["SumD1_9"] = normalizedMachine == "Skepp6" ? "Ytjämnhetsmätare" : string.Empty;
            kv["SumD2_1"] = normalizedMachine == "Skepp6" ? "Skjutmått" : string.Empty;
            kv["SumD2_2"] = normalizedMachine == "Skepp6" ? "Djupmått" : string.Empty;
            kv["SumD2_3"] = normalizedMachine == "Skepp6" ? kv["SumGuTolk"] : string.Empty;
            kv["SumD2_4"] = normalizedMachine == "Skepp6" ? kv["SumGGTolk"] : string.Empty;
            kv["SumD3_1"] = normalizedMachine == "Skepp6" ? "Multimar" : string.Empty;
            kv["SumD3_2"] = normalizedMachine == "Skepp6" ? "Mikrometerstickmått" : string.Empty;
            kv["SumD3_3"] = normalizedMachine == "Skepp6" ? "Skjutmått" : string.Empty;
            kv["SumD3_4"] = normalizedMachine == "Skepp6" ? "Skjutmått" : string.Empty;
            kv["SumD3_5"] = normalizedMachine == "Skepp6" ? "Skjutmått" : string.Empty;
            kv["SumD3_6"] = normalizedMachine == "Skepp6" ? "Fasmall" : string.Empty;
            kv["SumD3_7"] = normalizedMachine == "Skepp6" ? "Gängmall" : string.Empty;
            kv["SumD3_8"] = normalizedMachine == "Skepp6" ? "Ytjämnhetsmätare" : string.Empty;
            kv["SumD3_9"] = normalizedMachine == "Skepp6" ? "Mätmaskin" : string.Empty;
            kv["SumD3_0"] = normalizedMachine == "Skepp6" ? "Egglinjal" : string.Empty;

            for (int i = 1; i <= 8; i++) kv[$"SumAF1_{i}"] = string.Empty;
            kv["SumAF1_9"] = normalizedMachine == "Skepp6" ? "Övriga Ra värden 6,3" : string.Empty;
            for (int i = 1; i <= 4; i++) kv[$"SumAF2_{i}"] = string.Empty;
            kv["SumAF3_1"] = normalizedMachine == "Skepp6" ? "Kontrolleras med passbitsklove utf. 1" : string.Empty;
            for (int i = 2; i <= 8; i++) kv[$"SumAF3_{i}"] = string.Empty;
            kv["SumAF3_9"] = normalizedMachine == "Skepp6" ? "Tolerans: " + kv["SumKa"] : string.Empty;
            kv["SumAF3_0"] = normalizedMachine == "Skepp6" ? "Tolerans: " + kv["SumKa"] : string.Empty;

            string tmpRit = serie == 30 ? "223015, 7438482" : "223016, 7438482";
            kv["SumRitningsnrS1"] = "Produkt: " + tmpRit;
            kv["SumRitningsnrS2"] = kv["SumRitningsnrS1"];
            kv["SumRitningsnrS3"] = kv["SumRitningsnrS1"];
            kv["SumTolRitS1"] = "Toleranser: 1432008";
            kv["SumTolRitS2"] = kv["SumTolRitS1"];
            kv["SumTolRitS3"] = kv["SumTolRitS1"];
            kv["SumGTolRitS3"] = "Gänga: 7430181";
            kv["SumYtRitS1"] = "Yta: 7430184";
            kv["SumYtRitS2"] = kv["SumYtRitS1"];
            kv["SumYtRitS3"] = kv["SumYtRitS1"];

            kv["SumKlEgenskaperS1"] = "PRODUCTION NUTS & SLEEVES & HOUSINGSALLMÄNKLASSADE EGENSKAPER\\Klassade egenskaper muttrar";
            kv["SumKlEgenskaperS2"] = kv["SumKlEgenskaperS1"];
            kv["SumKlEgenskaperS3"] = kv["SumKlEgenskaperS1"];

            kv["VaLTyp"] = GetValidation(isHM, isHME, serie, typ);
            kv["VaLPopUp"] = string.Empty;

            SetManualInputFields(kv, serie, typ, normalizedMachine, d);
            return kv;
        }

        private static void ParseInput(string subjectInput, string machineInput, out string subject, out string machine)
        {
            string input = (subjectInput ?? string.Empty).Trim();
            string machineFromSubject = string.Empty;
            string subjectOnly = input;
            var match = Regex.Match(input, @"^(.*?)(?:\s*-\s*(SKEPP\s*6))$", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                subjectOnly = match.Groups[1].Value.Trim();
                machineFromSubject = match.Groups[2].Value.Trim();
            }

            subject = subjectOnly;
            machine = !string.IsNullOrWhiteSpace(machineInput) ? machineInput.Trim() : machineFromSubject;
        }

        private static string NormalizeMachine(string machine, string subject)
        {
            string src = (machine ?? string.Empty) + " " + (subject ?? string.Empty);
            if (src.IndexOf("SKEPP 6", StringComparison.OrdinalIgnoreCase) >= 0 ||
                src.IndexOf("SKEPP6", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "Skepp6";
            }
            return machine?.Trim() ?? string.Empty;
        }

        private static decimal CalcD(int serie, decimal kd)
        {
            if (serie == 30)
            {
                if (kd < 221) return kd + 40;
                if (kd < 281) return kd + 50;
                if (kd < 361) return kd + 60;
                if (kd < 421) return kd + 70;
                if (kd < 501) return kd + 80;
                if (kd == 560) return kd + 90;
                if (kd < 631) return kd + 100;
                if (kd < 671) return kd + 110;
                if (kd < 801) return kd + 120;
                if (kd < 951) return kd + 130;
                return kd + 140;
            }

            if (kd < 241) return kd + 60;
            if (kd < 281) return kd + 70;
            if (kd < 321) return kd + 80;
            if (kd < 361) return kd + 100;
            if (kd < 381) return kd + 110;
            if (kd < 461) return kd + 120;
            if (kd < 481) return kd + 140;
            if (kd < 501) return kd + 130;
            if (kd < 531) return kd + 140;
            if (kd < 601) return kd + 150;
            if (kd < 631) return kd + 170;
            if (kd < 671) return kd + 180;
            if (kd < 711) return kd + 190;
            if (kd < 801) return kd + 200;
            if (kd < 851) return kd + 210;
            if (kd < 951) return kd + 220;
            return kd + 240;
        }

        private static decimal CalcD1HM(int serie, decimal kd, decimal d)
        {
            if (serie == 30)
            {
                if (kd < 221) return d - 18;
                if (kd < 281) return d - 20;
                if (kd < 341) return d - 24;
                if (kd < 361) return d - 26;
                if (kd < 421) return d - 28;
                if (kd < 501) return d - 30;
                if (kd < 671) return d - 40;
                if (kd < 801) return d - 50;
                return d - 55;
            }

            if (kd < 281) return d - 30;
            if (kd < 361) return d - 40;
            if (kd < 381) return d - 50;
            if (kd < 401) return d - 60;
            if (kd < 441) return d - 50;
            if (kd < 461) return d - 40;
            if (kd < 481) return d - 60;
            if (kd < 501) return d - 50;
            if (kd < 601) return d - 60;
            if (kd < 631) return d - 70;
            if (kd < 801) return d - 75;
            if (kd < 851) return d - 85;
            if (kd < 951) return d - 90;
            if (kd < 1001) return d - 100;
            return d - 90;
        }

        private static decimal CalcTa(int serie, decimal kd)
        {
            if (serie == 30)
            {
                if (kd < 221) return 9;
                if (kd < 281) return 10;
                if (kd < 341) return 12;
                if (kd < 361) return 13;
                if (kd < 421) return 14;
                if (kd < 501) return 15;
                if (kd < 671) return 20;
                return 25;
            }

            if (kd < 241) return 10;
            if (kd < 321) return 12;
            if (kd < 361) return 15;
            if (kd < 421) return 18;
            if (kd < 481) return 20;
            if (kd < 531) return 23;
            if (kd < 601) return 25;
            if (kd < 671) return 28;
            if (kd < 711) return 30;
            if (kd < 801) return 34;
            return 38;
        }

        private static decimal CalcS(int serie, decimal kd)
        {
            if (serie == 30)
            {
                if (kd < 261) return 20;
                if (kd < 341) return 24;
                if (kd < 401) return 28;
                if (kd < 461) return 32;
                if (kd < 501) return 36;
                if (kd < 601) return 40;
                if (kd < 671) return 45;
                if (kd < 711) return 50;
                if (kd < 801) return 55;
                return 60;
            }

            if (kd < 261) return 20;
            if (kd < 321) return 24;
            if (kd < 361) return 28;
            if (kd < 421) return 32;
            if (kd < 481) return 36;
            if (kd < 531) return 40;
            if (kd < 601) return 45;
            if (kd < 671) return 50;
            if (kd < 711) return 55;
            if (kd < 801) return 60;
            return 70;
        }

        private static decimal TolH11(decimal value)
        {
            if (value < 3.01m) return 0.06m;
            if (value < 6.01m) return 0.075m;
            if (value < 10.01m) return 0.09m;
            if (value < 18.01m) return 0.11m;
            if (value < 30.01m) return 0.13m;
            if (value < 50.01m) return 0.16m;
            if (value < 80.01m) return 0.19m;
            if (value < 120.01m) return 0.22m;
            if (value < 180.01m) return 0.25m;
            if (value < 250.01m) return 0.29m;
            if (value < 315.01m) return 0.32m;
            if (value < 400.01m) return 0.36m;
            if (value < 500.01m) return 0.4m;
            if (value < 630.01m) return 0.44m;
            if (value < 800.01m) return 0.5m;
            if (value < 1000.01m) return 0.56m;
            if (value < 1250.01m) return 0.66m;
            if (value < 1600.01m) return 0.78m;
            if (value < 2000.01m) return 0.92m;
            if (value < 2500.01m) return 1.1m;
            return 1.35m;
        }

        private static decimal TolGeneralShaft(decimal value)
        {
            if (value < 19) return 0.27m;
            if (value < 31) return 0.33m;
            if (value < 51) return 0.39m;
            if (value < 81) return 0.46m;
            if (value < 121) return 0.54m;
            if (value < 181) return 0.63m;
            if (value < 251) return 0.72m;
            if (value < 316) return 0.81m;
            if (value < 401) return 0.89m;
            if (value < 501) return 0.97m;
            if (value < 631) return 1.1m;
            if (value < 801) return 1.25m;
            if (value < 1000) return 1.4m;
            return 1.65m;
        }

        private static decimal TolDiamInternal(decimal value)
        {
            if (value < 19) return 0.43m;
            if (value < 31) return 0.52m;
            if (value < 51) return 0.62m;
            if (value < 81) return 0.74m;
            if (value < 121) return 0.87m;
            if (value < 181) return 1m;
            if (value < 251) return 1.15m;
            if (value < 316) return 1.3m;
            if (value < 401) return 1.4m;
            if (value < 501) return 1.55m;
            if (value < 631) return 1.75m;
            if (value < 801) return 2m;
            if (value < 1000) return 2.3m;
            return 2.6m;
        }

        private static decimal TolWidth(decimal value)
        {
            if (value < 7) return 0.18m;
            if (value < 11) return 0.22m;
            if (value < 19) return 0.27m;
            if (value < 31) return 0.33m;
            if (value < 51) return 0.39m;
            if (value < 81) return 0.46m;
            if (value < 121) return 0.54m;
            return 0.63m;
        }

        private static string[] GetD1HMEList(int serie) => serie == 30
            ? new[] { "237", "264", "288", "", "", "", "", "394", "422", "442", "462", "488", "", "530", "550", "571", "610", "657", "690", "", "766", "820", "870", "925", "975", "1025", "1085", "1145", "", "", "" }
            : new[] { "", "", "", "", "335", "", "382", "406", "438", "456", "", "508", "535", "560", "580", "", "650", "690", "730", "775", "825", "875", "925", "975", "", "", "1140", "", "", "", "" };

        private static string[] GetBList(int serie) => serie == 30
            ? new[] { "", "", "", "", "", "", "", "", "", "", "48", "", "", "56", "", "", "71", "71", "71", "76", "", "86", "86", "", "", "", "", "", "", "", "" }
            : new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "71", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

        private static string[] GetTList(int serie) => serie == 30
            ? new[] { "5", "8", "8", "", "", "", "", "8", "10", "10", "10", "12", "", "12", "12", "15", "15", "18", "18", "", "20", "20", "20", "20", "25", "25", "25", "25", "", "", "" }
            : new[] { "", "", "", "", "5", "", "8", "10", "15", "15", "", "15", "20", "20", "12", "", "15", "15", "18", "18", "20", "20", "20", "25", "", "", "25", "", "", "", "" };

        private static string[] GetB2List(int serie) => serie == 30
            ? new[] { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "30", "0", "31", "31", "42", "37", "40", "40", "0", "50", "46", "46", "48", "58", "58", "58", "58", "0", "0", "0" }
            : new[] { "0", "0", "0", "0", "0", "0", "0", "35", "39", "0", "43", "42", "45", "49", "38", "0", "50", "50", "55", "57", "61", "61", "61", "69", "0", "0", "73", "0", "0", "0", "0" };

        private static string[] GetD4List(int serie) => serie == 30
            ? new[] { "", "", "", "", "", "", "", "", "", "", "457", "", "", "522", "", "", "607", "652", "682", "727", "", "807", "862", "", "", "", "", "", "", "", "" }
            : new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "535", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

        private static string GetValidation(bool isHM, bool isHME, int serie, int typ)
        {
            if (isHM)
            {
                return (serie == 30 && (typ == 96 || typ == 560 || typ == 670))
                    ? string.Empty
                    : "Typen måste tillföras i mallen";
            }

            if (isHME)
            {
                if (serie == 30 && (typ == 84 || typ == 600 || typ == 630 || typ == 750 || typ == 800)) return string.Empty;
                if (serie == 31 && typ == 96) return string.Empty;
                return "Typen måste tillföras i mallen";
            }

            return "Typen måste tillföras i mallen";
        }

        private static void SetManualInputFields(Dictionary<string, string> kv, int serie, int typ, string machine, decimal outerDiameter)
        {


            kv["SumChuckback"] = typ == 750 ? "5" : "1";
            kv["SumCB"] = "Chuckbackar:";
            kv["SumStödback"] = typ == 750 ? "90 mm" : "1 mm";
            kv["SumSB"] = "Stödbackar:";
            kv["SumGrader"] = typ == 750 ? "47 mm" : "1 mm";
            kv["SumGR"] = "Grader:";
            kv["SumVarv"] = typ == 750 ? "46/72 /min" : "1 /min";
            kv["SumVR"] = "Varvtal:";
            kv["SumMatPl"] = typ == 750 ? "0,62/0,20 /min" : "1 /min";
            kv["SumMP"] = "Matning Plan:";
            kv["SumMatInUt"] = typ == 750 ? "0,83/0,20 /min" : "1 /min";
            kv["SumMIU"] = "Matning Utv/Inv:";

            kv["SumFMUtv"] = typ == 750 ? FmtDec(outerDiameter) + " mm" : "1" + " mm";
            kv["SumFMU"] = "Utvändig diameter:";
            kv["SumUtvLin"] = typ == 750 ? "123 mm" : "1 mm";
            kv["SumUL"] = "Motsvarar på linjal:";
            kv["SumFMInv"] = typ == 750 ? "742  mm" : "1 mm";
            kv["SumFMI"] = "Invändig diameter:";
            kv["SumInvLin"] = typ == 750 ? "607 mm" : "1 mm";
            kv["SumIL"] = "Motsvarar på linjal:";

            kv["SumIN"] = "Inställning";
            kv["SumFM"] = "Färdigmått";
        }


        private static void SetRepeated(Dictionary<string, string> kv, string prefix, int count, string value)
        {
            for (int i = 1; i <= count; i++)
            {
                kv[$"{prefix}{i}"] = value;
            }
        }

        private static string GetArrayValue(string[] values, int oneBasedIndex)
        {
            if (oneBasedIndex <= 0 || oneBasedIndex > values.Length) return string.Empty;
            return values[oneBasedIndex - 1] ?? string.Empty;
        }

        private static decimal GetArrayValueDecimal(string[] values, int oneBasedIndex)
        {
            return ParseDecimalSafe(GetArrayValue(values, oneBasedIndex));
        }

        private static int ParseIntSafe(string value)
        {
            return int.TryParse((value ?? string.Empty).Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out int result)
                ? result
                : 0;
        }

        private static decimal ParseDecimalSafe(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return 0m;
            string normalized = value.Trim().Replace(',', '.');
            return decimal.TryParse(normalized, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result)
                ? result
                : 0m;
        }

        private static string Left(string value, int length)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            return value.Length <= length ? value : value.Substring(0, length);
        }

        private static string Mid(string value, int start, int length)
        {
            if (string.IsNullOrEmpty(value) || start >= value.Length) return string.Empty;
            int actualLength = Math.Min(length, value.Length - start);
            return value.Substring(start, actualLength);
        }

        private static string FmtDec(decimal value)
        {
            return value % 1m == 0m
                ? value.ToString("0", CultureInfo.InvariantCulture)
                : value.ToString("0.###", CultureInfo.InvariantCulture);
        }

        private static string FmtDecDot(decimal value)
        {
            return FmtDec(value).Replace(',', '.');
        }

        private static string FmtDecDotKeep3(decimal value)
        {
            return value.ToString("0.###", CultureInfo.InvariantCulture).Replace(',', '.');
        }

        private static string FmtDecDotKeep2orMore(decimal value)
        {
            return value.ToString("0.##", CultureInfo.InvariantCulture).Replace(',', '.');
        }

        private static void Merge(Dictionary<string, string> target, Dictionary<string, string> src)
        {
            foreach (var kvp in src)
            {
                target[kvp.Key] = kvp.Value;
            }
        }
    }
}
