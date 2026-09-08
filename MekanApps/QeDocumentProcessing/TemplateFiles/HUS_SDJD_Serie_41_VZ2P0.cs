using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HUS_SDJD_Serie_41_VZ2P0 : ITemplateCalculations
    {
        private const string LB = "<<LineBreak>>";

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            InitializeTemplateKeys(kv);

            string subject = (req?.ProductDesignation ?? string.Empty).Trim();
            string machine = (req?.MachineNumber ?? string.Empty).Trim();
            string formatted = NormalizeSubject(subject);
            string[] parts = Explode(formatted, new[] { ' ', '/', '.', '-', '_' });

            string tmpBet1 = Word(parts, 1);
            string tmpBet2 = Word(parts, 2);
            string tmpBet3 = Word(parts, 3);
            string tmpBet4 = Word(parts, 4);
            string tmpBet5 = Word(parts, 5);

            string tmpArt = "";
            string tmpSerie = "";
            string tmpTyp = "";
            string tmpVariant = "";
            bool valid = false;

            if (EqualsI(formatted, "HUS_SDJD_SERIE_41_VZ2P0") || EqualsI(formatted, "HUS SDJD SERIE 41 VZ2P0"))
            {
                tmpArt = "SDJD";
                tmpSerie = "41";
                tmpTyp = "500";
                tmpVariant = "VZ2P0";
                valid = true;
            }
            else
            {
                tmpArt = tmpBet1;
                if (EqualsI(tmpBet1, "HUS") && EqualsI(tmpBet2, "SDJD") && EqualsI(tmpBet3, "SERIE") && EqualsI(tmpBet4, "41") && EqualsI(tmpBet5, "VZ2P0"))
                {
                    tmpArt = "SDJD";
                    tmpSerie = "41";
                    tmpTyp = "500";
                    tmpVariant = "VZ2P0";
                    valid = true;
                }
                else
                {
                    tmpSerie = tmpBet2;
                    tmpTyp = tmpBet3;
                    tmpVariant = tmpBet4;
                    valid = EqualsI(tmpArt, "SDJD") && EqualsI(tmpSerie, "41") && EqualsI(tmpTyp, "500") && EqualsI(tmpVariant, "VZ2P0");
                }
            }

            kv["VaLPopUp"] = ComputePopup(req?.Published);
            kv["VaLTypLista"] = valid ? "" : "Produktbeteckningen ingår ej i mallen, kontrollera stavning och/eller kontakta Qe-Admin för att lägga till produkten";
            kv["VaLFärdig"] = "";
            kv["VaLInfo"] = "";

            string tmpRa32 = "3.2";
            string tmpRa63 = "6.3";
            kv["SumRa32"] = tmpRa32;
            kv["SumRa32a"] = tmpRa32;
            kv["SumRa63"] = tmpRa63 + " [3]";
            kv["SumRa63a"] = tmpRa63;
            kv["SumRa63b"] = tmpRa63;
            kv["SumRa63c"] = tmpRa63;
            kv["SumRa63d"] = tmpRa63;
            kv["SumRa63e"] = tmpRa63;
            kv["SumRa63f"] = tmpRa63;
            kv["SumRa63g"] = tmpRa63;
            kv["SumWt35"] = "Wt 35";
            kv["SumWt20"] = "Wt 20";

            kv["SumAd"] = "2x (Ad) 492";
            kv["SumAdTol"] = "+ 1.0";
            kv["SumAdTolN"] = "+ 0";

            kv["SumTd"] = "2x (Td) 520";
            kv["SumTdTol"] = "+ 0.500";
            kv["SumTdTolN"] = "+ 0.200";

            kv["SumId"] = "2x (Id) 576";
            kv["SumIdTol"] = "+ 2.0";
            kv["SumIdTolN"] = "- 0";

            kv["SumIb"] = "2x (Ib) 25";
            kv["SumIbTol"] = "+ 0";
            kv["SumIbTolN"] = "- 1.0";

            kv["SumTb"] = "2x (Tb) 6";
            kv["SumTbTol"] = "+ 0.500";
            kv["SumTbTolN"] = "- 0";

            kv["SumLö"] = "2x Ø70";
            kv["SumJ"] = "8x M12";
            kv["SumJ1"] = "8x min25";
            kv["SumJt"] = "8x varje sida";

            kv["SumC"] = "2x 1.5";
            kv["SumCTol"] = "± 1.0";
            kv["SumC1"] = "2x Ø32";
            kv["SumC1Tol"] = "+ 1.0";
            kv["SumC1TolN"] = "- 0";

            kv["SumD"] = "2x Ø32";
            kv["SumDTol"] = "+ 1.0";
            kv["SumDTolN"] = "- 0";
            kv["SumD1"] = "2x 1.5";
            kv["SumD1Tol"] = "± 1.0";
            kv["SumD2"] = "2x1.0";
            kv["SumD2Tol"] = "+ 1.0";
            kv["SumD2TolN"] = "- 0.500";

            kv["SumAb"] = "(Ab) 540";
            kv["SumAbTol"] = "± 1.0";

            double tmpLdTol = 0.116;
            double tmpLdTolN = 0.026;
            double tmpLdTolPct = (tmpLdTol - tmpLdTolN) * 0.81;
            double tmpLdTolS = tmpLdTolN + tmpLdTolPct;
            double tmpLdTolNS = tmpLdTol - tmpLdTolPct;
            kv["SumLd"] = "(Ld) 830";
            kv["SumLdTolS"] = "+ " + FormatDot3(tmpLdTolS) + "*";
            kv["SumLdTolNS"] = "+ " + FormatDot3(tmpLdTolNS) + "*";
            kv["SumLdTol"] = "+ " + FormatDot3(tmpLdTol) + " [3]";
            kv["SumLdTolN"] = "+ " + FormatDot3(tmpLdTolN) + " [2D]";

            kv["SumLb"] = "(Lb) 345";
            kv["SumLbTol"] = "+ 0.570";
            kv["SumLbTolN"] = "- 0";

            kv["SumHd"] = "(Hd) 940";
            kv["SumHdTol"] = "± 0.700";

            kv["SumGd"] = "3x min 12";
            kv["SumG"] = "3x G1/4";
            kv["SumG1"] = "(G1) M42 6H";
            kv["SumG1d"] = "(G1d) 110";
            kv["SumG1dTol"] = "± 0.300";
            kv["SumB1d"] = "(B1d) 130";
            kv["SumB1dTol"] = "± 0.300";

            kv["SumBd"] = "(Bd) 45";
            kv["SumBdTol"] = "+ 1.000";
            kv["SumBdTolN"] = "- 0";

            kv["SumSd"] = "2x (Sd) 16";
            kv["SumSdTol"] = "+ 0.180";
            kv["SumSdTolN"] = "- 0";
            kv["SumSö"] = "(Sö) 20";
            kv["SumSöTol"] = "± 0.420";
            kv["SumSu"] = "(Su) 20";
            kv["SumSuTol"] = "± 0.420";

            kv["SumUh"] = "(Uh) 500";
            kv["SumUhTol"] = "± 0.200";
            kv["SumCh"] = "(Ch) 500";
            kv["SumChTol"] = kv["SumUhTol"];

            kv["SumFh"] = "(Fh) 100";
            kv["SumFhTol"] = "± 0.500";
            kv["SumFl"] = "(Fl) 1100";
            kv["SumFlTol"] = "± 0.210";
            kv["SumBp"] = "(Bp) 230";
            kv["SumBpTol"] = "± 0.500";
            kv["SumPl"] = "0.05";
            kv["SumFp"] = "0.14";

            kv["SumG2"] = "G1/2";
            kv["Sumn1"] = "max 38";
            kv["Sumn2"] = "min 30";
            kv["Sumn3"] = "Ø 45";
            kv["Sumn3Tol"] = "+ 5";
            kv["Sumn3TolN"] = "- 0";
            kv["Sumn4"] = "Ø 2";
            kv["Sumn4Tol"] = "± 1.0";
            kv["Summ1"] = "3";
            kv["Summ2"] = "Ø 64";
            kv["Summ2Tol"] = "± 0.300";

            string tmpMaskinValS1 = EqualsI(machine, "OKUMA MA600/Trevisan DS 450") ? "Trevisan DS 450" : EqualsI(machine, "Trevisan DS 900") ? "Trevisan DS 900" : "";
            string tmpMaskinValS2 = EqualsI(machine, "OKUMA MA600/Trevisan DS 450") ? "OKUMA MA600" : EqualsI(machine, "Trevisan DS 900") ? "Trevisan DS 900" : "";
            kv["SumMaskinValS1"] = ("OP 2 - " + tmpMaskinValS1 + " - Svarvning").Trim();
            kv["SumMaskinValS2"] = ("OP 1 - " + tmpMaskinValS2 + " - Borrning, fräsning").Trim();

            bool machineMatch = EqualsI(machine, "OKUMA MA600/Trevisan DS 450") || EqualsI(machine, "Trevisan DS 900");

            kv["SumF1_1"] = machineMatch ? "1/1" : "";
            kv["SumF1_2"] = machineMatch ? "1/1" : "";
            kv["SumF1_3"] = machineMatch ? "1/1" : "";
            kv["SumF1_4"] = machineMatch ? "1/1" : "";
            kv["SumF1_5"] = machineMatch ? "1/1" : "";
            kv["SumF1_6"] = machineMatch ? "inst.1bit" : "";
            kv["SumF1_7"] = machineMatch ? "inst.1bit" : "";
            kv["SumF1_8"] = machineMatch ? "inst.1bit" : "";

            kv["SumF2_1"] = machineMatch ? "inst.1bit" : "";
            kv["SumF2_2"] = machineMatch ? "" : "";
            kv["SumF2_3"] = machineMatch ? "" : "";
            kv["SumF2_4"] = machineMatch ? "inst.1bit" : "";
            kv["SumF2_5"] = machineMatch ? "" : "";
            kv["SumF2_6"] = machineMatch ? "inst.1bit" : "";
            kv["SumF2_7"] = machineMatch ? "inst.1bit" : "";
            kv["SumF2_8"] = machineMatch ? "inst.1bit" : "";
            kv["SumF2_9"] = machineMatch ? "inst.1bit" : "";
            kv["SumF2_10"] = machineMatch ? "inst.1bit" : "";
            kv["SumF2_11"] = machineMatch ? "inst.1bit" : "";
            kv["SumF2_12"] = machineMatch ? "inst.1bit" : "";
            kv["SumF2_13"] = machineMatch ? "inst.1bit" : "";
            kv["SumF2_14"] = machineMatch ? "inst.1bit" : "";

            kv["SumD1_1"] = machineMatch ? "Skjutmått/Tolk" : "";
            kv["SumD1_2"] = machineMatch ? "Skjutmått" : "";
            kv["SumD1_3"] = machineMatch ? "Mikrometerstickmått" : "";
            kv["SumD1_4"] = machineMatch ? "Skjutmått/Tolk" : "";
            kv["SumD1_5"] = machineMatch ? "Skjutmått/passbitar" : "";
            kv["SumD1_6"] = machineMatch ? "Gängtolk" : "";
            kv["SumD1_7"] = machineMatch ? "Höjdmätningsapparat" : "";
            kv["SumD1_8"] = machineMatch ? "Skjutmått" : "";

            kv["SumD2_1"] = machineMatch ? "Gängtolk" : "";
            kv["SumD2_2"] = machineMatch ? "" : "";
            kv["SumD2_3"] = machineMatch ? "" : "";
            kv["SumD2_4"] = machineMatch ? "Skjutmått/Okulärt" : "";
            kv["SumD2_5"] = machineMatch ? "" : "";
            kv["SumD2_6"] = machineMatch ? "Skjutmått/Tolk" : "";
            kv["SumD2_7"] = machineMatch ? "Skjutmått" : "";
            kv["SumD2_8"] = machineMatch ? "Skjutmått" : "";
            kv["SumD2_9"] = machineMatch ? "Skjutmått" : "";
            kv["SumD2_10"] = machineMatch ? "Skjutmått" : "";
            kv["SumD2_11"] = machineMatch ? "Kännbleck" : "";
            kv["SumD2_12"] = machineMatch ? "Skjutmått" : "";
            kv["SumD2_13"] = machineMatch ? "Ytjämnhetsmätare" : "";
            kv["SumD2_14"] = machineMatch ? "Kännbleck" : "";

            kv["SumAF1_1"] = machineMatch ? "" : "";
            kv["SumAF1_2"] = machineMatch ? "" : "";
            kv["SumAF1_3"] = machineMatch ? "* styrgränser enl. arbetsinstruktion med dokument nr: 100-3" : "";
            kv["SumAF1_4"] = machineMatch ? "" : "";
            kv["SumAF1_5"] = machineMatch ? "" : "";
            kv["SumAF1_6"] = machineMatch ? "" : "";
            kv["SumAF1_7"] = machineMatch ? "Ind. från lagerläge" : "";
            kv["SumAF1_8"] = machineMatch ? "" : "";

            kv["SumAF2_1"] = machineMatch ? "" : "";
            kv["SumAF2_2"] = machineMatch ? "" : "";
            kv["SumAF2_3"] = machineMatch ? "" : "";
            kv["SumAF2_4"] = machineMatch ? "Genomgående hål" : "";
            kv["SumAF2_5"] = machineMatch ? "" : "";
            kv["SumAF2_6"] = machineMatch ? "" : "";
            kv["SumAF2_7"] = machineMatch ? "" : "";
            kv["SumAF2_8"] = machineMatch ? "" : "";
            kv["SumAF2_9"] = machineMatch ? "" : "";
            kv["SumAF2_10"] = machineMatch ? "" : "";
            kv["SumAF2_11"] = machineMatch ? "Infettas, Mått: 0.05 ihopsatt hus." : "";
            kv["SumAF2_12"] = machineMatch ? "" : "";
            kv["SumAF2_13"] = machineMatch ? "" : "";
            kv["SumAF2_14"] = machineMatch ? "Kontroll mot planskiva, mått: 0.1" : "";

            kv["SumTextS1"] = "Kontrolleras enl. styrplan";
            kv["SumTextS2"] = kv["SumTextS1"] + " Alla materialdefekter utsorteras.";

            string prodText = string.IsNullOrWhiteSpace(subject) ? "SDJD 41/500 VZ2P0" : subject;
            kv["SumPrdritS1"] = "Produktritning: " + prodText;
            kv["SumPrdritS2"] = kv["SumPrdritS1"];
            kv["SumArbInstGjgS1"] = "Gjutgods: A3.022";
            kv["SumArbInstGjgS2"] = kv["SumArbInstGjgS1"];
            kv["SumKvStPlS1"] = "Kvalitetstyrning: K1.07-17";
            kv["SumKvStPlS2"] = kv["SumKvStPlS1"];
            kv["SumSkr"] = "8.8 SNL.";

            return kv;
        }

        private static void InitializeTemplateKeys(Dictionary<string, string> kv)
        {
            foreach (var key in TemplateKeys)
                kv[key] = "";
        }

        private static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published)) return "";
            if (!DateTime.TryParse(published, out var pubDt)) return "";
            int dagar = 14;
            DateTime validTill = pubDt.AddDays(dagar);
            if (DateTime.Today <= validTill.Date)
                return "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom " + dagar.ToString(CultureInfo.InvariantCulture) + " dagar)" + LB + LB + "" + LB + LB + "Popupruta aktiv till " + validTill.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return "";
        }

        private static string NormalizeSubject(string subject)
        {
            return (subject ?? string.Empty).Trim().ToUpperInvariant().Replace('.', ',');
        }

        private static string[] Explode(string s, char[] separators)
        {
            if (string.IsNullOrEmpty(s)) return Array.Empty<string>();
            return s.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        }

        private static string Word(string[] list, int index1Based)
        {
            if (list == null || index1Based <= 0 || index1Based > list.Length) return "";
            return list[index1Based - 1] ?? "";
        }

        private static bool EqualsI(string a, string b)
        {
            return string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);
        }

        private static string FormatDot3(double value)
        {
            return value.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
        }

        private static readonly string[] TemplateKeys = new[]
        {
            "VaLPopUp", "VaLTypLista", "VaLFärdig", "VaLInfo",
            "SumIbTol", "SumIbTolN", "SumIb", "SumRa63e", "SumIdTol", "SumIdTolN", "SumId", "SumAd", "SumAdTol", "SumAdTolN",
            "SumTdTol", "SumTdTolN", "SumTd", "SumTb", "SumTbTol", "SumTbTolN", "SumSkr",
            "SumF1_1", "SumD1_1", "SumAF1_1", "SumF1_2", "SumD1_2", "SumAF1_2", "SumF1_3", "SumD1_3", "SumAF1_3", "SumF1_4", "SumD1_4", "SumAF1_4",
            "SumF1_5", "SumD1_5", "SumAF1_5", "SumF1_6", "SumD1_6", "SumAF1_6", "SumF1_7", "SumD1_7", "SumAF1_7", "SumF1_8", "SumD1_8", "SumAF1_8",
            "SumTextS1", "SumD1Tol", "SumD1", "SumD2Tol", "SumD2TolN", "SumD", "SumDTol", "SumDTolN", "SumD2", "SumRa63g", "SumRa32",
            "SumLbTol", "SumLbTolN", "SumLb", "SumG", "SumGd", "SumRa63a", "SumRa63b", "SumRa63c", "SumRa63d", "SumCh", "SumChTol",
            "SumAb", "SumAbTol", "SumLdTol", "SumLdTolN", "SumLd", "SumLdTolS", "SumLdTolNS", "SumPrdritS1", "SumArbInstGjgS1", "SumKvStPlS1", "SumMaskinValS1",
            "SumG1", "SumRa63", "SumWt35", "SumRa32a", "SumWt20", "SumG1d", "SumB1d", "SumG1dTol", "SumB1dTol", "SumC1Tol", "SumC1TolN", "SumCTol", "SumC", "SumC1",
            "SumRa63f", "SumJ1", "SumUh", "SumUhTol", "SumFhTol", "SumFh", "SumJ", "SumJt", "SumBd", "SumBdTol", "SumBdTolN", "SumBpTol", "SumBp", "SumLö", "SumSöTol",
            "SumSö", "SumSu", "SumSuTol", "SumSdTol", "SumSdTolN", "SumSd", "SumPl", "Sumn2", "Sumn1", "Sumn3Tol", "Sumn3TolN", "Sumn3", "SumG2", "Sumn4Tol", "Sumn4",
            "SumHdTol", "SumHd", "SumMaskinValS2", "Summ2Tol", "Summ2", "Summ1", "SumFl", "SumFlTol", "SumFp",
            "SumF2_1", "SumD2_1", "SumAF2_1", "SumF2_2", "SumD2_2", "SumAF2_2", "SumF2_3", "SumD2_3", "SumAF2_3", "SumF2_4", "SumD2_4", "SumAF2_4",
            "SumF2_5", "SumD2_5", "SumAF2_5", "SumF2_6", "SumD2_6", "SumAF2_6", "SumF2_7", "SumD2_7", "SumAF2_7", "SumF2_8", "SumD2_8", "SumAF2_8",
            "SumF2_9", "SumD2_9", "SumAF2_9", "SumF2_10", "SumD2_10", "SumAF2_10", "SumF2_11", "SumD2_11", "SumAF2_11", "SumF2_12", "SumD2_12", "SumAF2_12",
            "SumF2_13", "SumD2_13", "SumAF2_13", "SumF2_14", "SumD2_14", "SumAF2_14", "SumTextS2", "SumPrdritS2", "SumArbInstGjgS2", "SumKvStPlS2"
        };
    }
}
