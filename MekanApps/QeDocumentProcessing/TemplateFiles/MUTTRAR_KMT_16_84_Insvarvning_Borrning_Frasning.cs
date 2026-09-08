using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class MUTTRAR_KMT_16_84_Insvarvning_Borrning_Frasning : ITemplateCalculations
    {
        private const string LB = "<<LineBreak>>";
        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject =req != null? req.ProductDesignation ?? string.Empty: string.Empty;

            string maskinVal =req != null? req.MachineNumber ?? string.Empty: string.Empty;

            kv["VaLPopUp"] =ComputePopup(req != null ? req.Published : null);

            string tmpBet =
                subject.ToUpperInvariant().Trim().Replace(".", ",");

            bool tmpVZ458 =tmpBet.Contains("VZ458");

            string[] parts = tmpBet.Split(new[] { ' ', '/', '.', '-' },StringSplitOptions.RemoveEmptyEntries);

            string tmpBet1 =parts.Length > 0 ? parts[0] : "";

            int tmpTyp = parts.Length > 1? ParseInt(parts[1]): 0;

            double tmpD =tmpTyp == 0 ? 10 : tmpTyp == 1 ? 12 : tmpTyp == 2 ? 15 : tmpTyp == 3 ? 17 :(tmpTyp / 2.0) * 10.0;

            double tmpP = tmpD < 11 ? 0.75 : tmpD < 21 ? 1 : tmpD < 51 ? 1.5 :  tmpD < 151 ? 2 : tmpD < 201 ? 3 : tmpD < 301 ? 4 :5;

            kv["SumP"] ="(P) " + Smart(tmpP);

            kv["SumG"] = (tmpP == 0.75 || tmpP == 1 || tmpP == 1.5 ||tmpP == 2 ||tmpP == 3? "M ": "Tr ") + Smart(tmpD)+ "x"+ Smart(tmpP);

            int tmpG1 =  tmpD < 16 ? 5 : tmpD < 36 ? 6 : tmpD < 81 ? 8 :  tmpD < 221 ? 10 : 12;

            kv["SumG1"] =  "M" + tmpG1 + "-6H [3F]";

            double tmpf1 = tmpD < 16 ? 3 :tmpD < 36 ? 3.4 : tmpD < 81 ? 5 : tmpD < 201 ? 6.1 : tmpD < 221 ? 6.5 : tmpD < 281 ? 11.6 :  tmpD < 301 ? 16.5 :tmpD < 361 ? 21.5 :26.5;

            kv["Sumf1"] = "(f1) " + Smart(tmpf1);

            kv["Sumf1Tol"] = (tmpD < 36 ? "+ 0.6" : tmpD < 81? "+ 1.2" : "+ 1.5")+ " [2F]";

            kv["Sumf1TolN"] = "0 [2F]";

            double tmpd7 = tmpG1 == 5 ? 3.75 : tmpG1 == 6 ? 4.45 : tmpG1 == 8 ? 5.75 : tmpD < 221 ? 7.6 : 9.96;

            kv["Sumd7"] ="(d7) " + tmpd7.ToString(CommonFunctions.Culture).Replace(".", ",");

            kv["SumVd7"] = (tmpTyp < 43 ? 30 : 15)+ "°";

            double tmpf2 =
                tmpG1 == 5 ? 4.7 :
                tmpG1 == 6 ? 5.5 :
                tmpG1 == 8 ? 7.5 :
                tmpD < 221 ? 9.5 :
                tmpD < 281 ? 14.5 :
                tmpD < 301 ? 19.5 :
                tmpD < 361 ? 24.5 :
                29.5;

            kv["Sumf2"] = "(f2) " + tmpf2.ToString(CommonFunctions.Culture).Replace(".", ",");

            kv["Sumf2Tol"] = "0 [2F]";

            kv["Sumf2TolN"] =(tmpD < 81 ? "- 0.3": "- 0.5")+ " [2F]";

            double tmpb =
                tmpD < 16 ? 4 :
                tmpD < 36 ? 5 :
                tmpD < 46 ? 6 :
                tmpD < 56 ? 7 :
                tmpD < 81 ? 8 :
                tmpD < 121 ? 10 :
                tmpD < 131 ? 12 :
                tmpD < 171 ? 14 :
                tmpD < 191 ? 16 :
                tmpD < 201 ? 18 :
                tmpD < 261 ? 20 :
                tmpD < 341 ? 24 :
                tmpD < 381 ? 28 :
                32;

            kv["Sumb"] =  "(b) " + Smart(tmpb);

            kv["SumbTol"] =
                (tmpD < 46
                    ? "± 0.150"
                    : tmpD < 131
                        ? "± 0.180"
                        : tmpD < 201
                            ? "± 0.215"
                            : tmpD < 381
                                ? "± 0.260"
                                : "± 0.310")
                + " [3F]";

            double tmph;

            if (!tmpVZ458)
            {
                tmph =
                    tmpD < 36 ? 2 :
                    tmpD < 46 ? 2.5 :
                    tmpD < 56 ? 3 :
                    tmpD < 81 ? 3.5 :
                    tmpD < 121 ? 4 :
                    tmpD < 201 ? 5 :
                    tmpD < 261 ? 10 :
                    tmpD < 341 ? 12 :
                    tmpD < 381 ? 13 :
                    14;
            }
            else
            {
                tmph = tmpD < 171 ? 6 : tmpD < 191 ? 7 : 8;
            }

            kv["Sumh"] = "(h) " + tmph.ToString(CommonFunctions.Culture).Replace(".", ",");


            kv["SumhTol"] = "+ " + (tmpD < 36 ? "0" : "0.5") +  " [3F]";

            kv["SumhTolN"] = "- " + (tmpD < 36 ? "0.5" : "0") +" [3F]";

            string hd =tmpD < 46? "± 0.25": tmpD < 121 ? "± 0.375": "± 0.50";

            kv["SumHD"] ="(HD) Lika delning:" + LB + hd +" [2F]";

            kv["SumRa125"] = "12.5";

            double tmpd2 =
                tmpD < 16 ? tmpD + 18 :
                tmpD < 23 ? tmpD + 20 :
                tmpD < 36 ? tmpD + 19 :
                tmpD < 51 ? tmpD + 25 :
                tmpD < 84 ? tmpD + 30 :
                tmpD < 201 ? tmpD + 35 :
                tmpD < 221 ? tmpD + 45 :
                tmpD < 281 ? tmpD + 50 :
                tmpD < 301 ? tmpD + 60 :
                tmpD < 361 ? tmpD + 70 :
                tmpD + 80;

            kv["Sumd2"] ="(d2) " + Smart(tmpd2);

            kv["Sumd2Tol"] = "+ 0 [3F]";

            kv["Sumd2TolN"] =
                (tmpd2 < 31 ? "- 0.130" :
                 tmpd2 < 50 ? "- 0.160" :
                 tmpd2 < 80 ? "- 0.190" :
                 tmpd2 < 121 ? "- 0.220" :
                 tmpd2 < 180 ? "- 0.250" :
                 tmpd2 < 235 ? "- 0.290" :
                 tmpd2 < 311 ? "- 0.320" :
                 tmpd2 < 391 ? "- 0.360" :
                 "- 0.400")
                + " [3F]";

            double tmpd3 =
                tmpD < 16 ? tmpD + 13 :
                tmpD < 18 ? tmpD + 16 :
                tmpD < 21 ? tmpD + 15 :
                tmpD < 36 ? tmpD + 14 :
                tmpD < 46 ? tmpD + 19 :
                tmpD < 51 ? tmpD + 18 :
                tmpD < 56 ? tmpD + 23 :
                tmpD < 76 ? tmpD + 22 :
                tmpD < 81 ? tmpD + 20 :
                tmpD < 101 ? tmpD + 25 :
                tmpD < 161 ? tmpD + 24 :
                tmpD < 171 ? tmpD + 22 :
                tmpD < 201 ? tmpD + 24 :
                tmpD < 221 ? tmpD + 34 :
                tmpD < 281 ? tmpD + 39 :
                tmpD < 301 ? tmpD + 49 :
                tmpD < 361 ? tmpD + 59 :
                tmpD + 69;

            kv["Sumd3"] = "(d3) " + Smart(tmpd3);

            kv["Sumd3Tol"] = "+ 0 [3F]";

            kv["Sumd3TolN"] =
                (tmpd3 < 26 ? "- 0.2" :
                 tmpd3 < 36 ? "- 0.3" :
                 tmpd3 < 70 ? "- 0.4" :
                 "- 0.5")
                + " [3F]";

            double tmpd4 =
                tmpD < 26 ? tmpD + 1 :
                tmpD < 31 ? tmpD + 2 :
                tmpD < 36 ? tmpD + 3 :
                tmpD < 41 ? tmpD + 2 :
                tmpD < 46 ? tmpD + 3 :
                tmpD < 51 ? tmpD + 2 :
                tmpD < 56 ? tmpD + 3 :
                tmpD < 61 ? tmpD + 2 :
                tmpD < 66 ? tmpD + 3 :
                tmpD < 76 ? tmpD + 2 :
                tmpD < 101 ? tmpD + 3 :
                tmpD + 2;

            kv["Sumd4"] = "(d4)" + Smart(tmpd4);

            kv["Sumd4Tol"] =
                (tmpd4 < 14 ? "+ 0.2" :
                 tmpd4 < 22 ? "+ 0.3" :
                 tmpd4 < 53 ? "+ 0.4" :
                 "+ 0.5")
                + " [3F]";

            kv["Sumd4TolN"] ="- 0 [3F]";

            double tmpe1 =
                tmpD < 11 ? 1.5 :
                tmpD < 31 ? 2 :
                tmpD < 46 ? 3 :
                tmpD < 76 ? 4 :
                5;

            kv["Sume1"] = "(e1) " + Smart(tmpe1);

            kv["Sume1Tol"] = tmpD < 46 ? "": "± 0.2";

            double tmpe2 =
                tmpD < 21 ? 1 :
                tmpD < 31 ? 2 :
                tmpD < 46 ? 3 :
                tmpD < 76 ? 4 :
                5;

            double tmpe2S1 = tmpe2 + 1;

            kv["Sume2S1"] ="(e2) " + Smart(tmpe2S1);

            kv["Sume2S1Tol"] = "± 0.2";

            kv["Sume2"] ="(e2) " + Smart(tmpe2);

            kv["Sume2Tol"] = "± 0.2";

            double tmpB =
                tmpD < 13 ? 14 :
                tmpD < 16 ? 16 :
                tmpD < 21 ? 18 :
                tmpD < 31 ? 20 :
                tmpD < 46 ? 22 :
                tmpD < 56 ? 25 :
                tmpD < 61 ? 26 :
                tmpD < 76 ? 28 :
                tmpD < 201 ? 32 :
                tmpD < 221 ? 36 :
                tmpD < 261 ? 38 :
                tmpD < 281 ? 40 :
                tmpD < 321 ? 45 :
                tmpD < 341 ? 48 :
                tmpD < 361 ? 50 :
                tmpD < 381 ? 55 :
                tmpD < 401 ? 60 :
                65;

            double tmpBS1 = tmpB + 1;

            kv["SumB"] ="(B) " + Smart(tmpB);

            kv["SumBS1"] ="(B) " + Smart(tmpBS1);

            kv["SumBTol"] =
                (tmpD < 21
                    ? "± 0.15"
                    : tmpD < 76
                        ? "± 0.20"
                        : "± 0.25");

            kv["SumBS1Tol"] = kv["SumBTol"];

            double tmpD1 =
                tmpP == 0.75 ? tmpD - 0.812 :
                tmpP == 1 ? tmpD - 1.083 :
                tmpP == 1.5 ? tmpD - 1.624 :
                tmpP == 2 ? tmpD - 2.165 :
                tmpP == 3 ? tmpD - 3.248 :
                tmpP == 4 ? tmpD - 4 :
                tmpD - 5;

            double tmpD1S1 =
                maskinVal == "LT300"
                    ? tmpD1 - 1
                    : tmpP == 1
                        ? tmpD - 1.5
                        : tmpP == 1.5
                            ? tmpD - 2.5
                            : tmpP == 2
                                ? tmpD - 4
                                : tmpD1 - 0.69;

            tmpD1S1 =Math.Round(tmpD1S1, 2);

            kv["SumD1S1"] ="(D1) " + tmpD1S1.ToString(CommonFunctions.Culture).Replace(".", ",");

            kv["SumD1S1Tol"] = "± 0.2";

            double tmpd5 =
                tmpD < 131 ? tmpD + 1 :
                tmpD < 201 ? tmpD + 0.5 :
                tmpD + 2;

            double tmpd5S1 =
                tmpD > 131
                    ? tmpd5 - 0.5
                    : tmpd5;

            kv["Sumd5S1"] = "(d5) " + Smart(tmpd5S1);

            kv["Sumd5S1Tol"] = tmpD > 131? "+ 0": (tmpD < 76 ? "+ 0.2": tmpD < 91 ? "+ 0.4" : "+ 0.5")+ " [3F]";

            kv["Sumd5S1TolN"] =
                tmpD > 131
                    ? (tmpD < 76
                        ? "- 0.2"
                        : tmpD < 91
                            ? "- 0.4"
                            : "- 0.5")
                    : "- 0 [3F]";

            kv["Sum30"] = tmpD < 201 ? "30º": "15º";

            kv["Sum30a"] = "30º";

            kv["Sum60"] =tmpD < 201 ? "60º": "45º";

            kv["Sum60a"] =tmpD < 46 ? "": tmpD < 201 ? "60º" : "45º";

            kv["SumGFas"] =tmpD < 131? "20º": tmpD < 201 ? "60º" : "45º";

            kv["SumGFas1"] =  kv["SumGFas"];

            double tmpd6 =
                tmpD < 16 ? tmpD + 11 :
                tmpD < 21 ? tmpD + 12 :
                tmpD < 36 ? tmpD + 11 :
                tmpD < 41 ? tmpD + 14 :
                tmpD < 46 ? tmpD + 15 :
                tmpD < 51 ? tmpD + 14 :
                tmpD < 56 ? tmpD + 19 :
                tmpD < 81 ? tmpD + 18 :
                tmpD < 201 ? tmpD + 22 :
                tmpD < 221 ? tmpD + 36.5 :
                tmpD < 251 ? tmpD + 41 :
                tmpD < 261 ? tmpD + 41.5 :
                tmpD < 281 ? tmpD + 40 :
                tmpD < 301 ? tmpD + 50 :
                tmpD < 361 ? tmpD + 59.5 :
                tmpD + 68;

            kv["Sumd6"] = "(d6) " + Smart(tmpd6);

            kv["Sumd6Tol"] = "+ 0 [3P]";

            kv["Sumd6TolN"] =  (tmpd6 < 33 ? "- 0.3" : tmpd6 < 65  ? "- 0.4" : tmpd6 < 143  ? "- 0.5" : "- 0.7")+ " [3F]";

            kv["SumRa32"] ="Ytjämnhet alla färdig-bearbetade ytor 3.2";

            double tmpBOp1 = tmpB + 3;

            kv["SumBOp1"] = tmpD > 201? "Bredd (B) efter första operation" + LB + "i högra spindeln = " +Smart(tmpBOp1) + "mm": "";

            double tmpd3Op1 = tmpd3 + 1;

            string tmpd3Op1a = "Utvändig falsdiameter (d3) efter 1:a OP i H-spindeln: " + Smart(tmpd3Op1) + "mm";

            string tmpe2Op1 ="Utvändig falsdjup (e2) efter 1:a OP i H-spindeln: 5mm";

            string maskinS1 =
                maskinVal == "LVT300" ? "LVT300" :
                maskinVal == "LR15/Kolsva" ? "LR15/Kolsva" :
                maskinVal == "LT300" ? "LT300" :
                "";

            string maskinS2 =
                maskinVal == "LVT300" ? "LVT300" :
                maskinVal == "LR15/Kolsva" ? "Kolsva/Brotch" :
                maskinVal == "LT300" ? "LT300" :
                "";

            kv["SumMaskinValS1"] = "Insvarvning: " + maskinS1;

            kv["SumMaskinValS2"] = "Borrning: " + maskinS2;

            SetFrequencies(kv, maskinVal);
            SetDevices(kv, maskinVal, tmpD);
            SetAF(kv, maskinVal, tmpTyp, tmpe2Op1, tmpd3Op1a);


            kv["SumTextS1"] = "100% okulär kontroll av grader, frifläckar, slagmärken, repor, valkar och andra ojämnheter.";

            kv["SumTextS2"] = "100% okulär kontroll av grader, frifläckar, slagmärken, repor, valkar och andra ojämnheter." + LB +
                "Vid upptäckta felaktiga detaljer skall kontroll av föregående detalj göras tills första godkända detalj hittas.";

            string stampling ="Stämplas enl. ritning: 7430919 alt. 7430189";

            kv["SumRitNrS1"] = tmpTyp < 41 ? "KMT 0-40:8" : "KMT 44-84:8";

            kv["SumRitNrS2"] = kv["SumRitNrS1"] + " " + stampling;

            kv["SumKlEgenskaperS1"] = @"PRODUCTION NUTS & SLEEVES & HOUSINGS\ALLMÄN\KLASSADE EGENSKAPER\Klassade egenskaper muttrar KMT KMTA";

            kv["SumKlEgenskaperS2"] = kv["SumKlEgenskaperS1"];

            return kv;
        }

        static void SetFrequencies(
            Dictionary<string, string> kv,
            string machine)
        {
            bool m = machine == "LVT300" ||  machine == "LR15/Kolsva" || machine == "LT300";

            kv["SumF1_1"] = m ? "1/10" : "";
            kv["SumF1_e"] = "";
            kv["SumF1_2"] = m ? "1/10" : "";
            kv["SumF1_3"] = m ? "1/10" : "";
            kv["SumF1_4"] = m ? "1/10" : "";
            kv["SumF1_5"] = m ? "1/10" : "";
            kv["SumF1_6"] = m ? "1/10" : "";
            kv["SumF1_7"] = m ? "1/10" : "";
            kv["SumF1_8"] = m ? "Inst." : "";

            kv["SumF2_1"] = m ? "1/10" : "";
            kv["SumF2_2"] = m ? "1/10" : "";
            kv["SumF2_3"] = m ? "1/20" : "";
            kv["SumF2_4"] = m ? "Inst." : "";
            kv["SumF2_5"] = m ? "Inst." : "";
            kv["SumF2_6"] = m ? "1/20" : "";
            kv["SumF2_7"] = m ? "1/20" : "";
            kv["SumF2_8"] = m ? "Inst." : "";
            kv["SumF2_9"] = m ? "Inst." : "";
        }

        static void SetDevices(
            Dictionary<string, string> kv,
            string machine,
            double tmpD)
        {
            bool valid =machine == "LVT300" || machine == "LR15/Kolsva" || machine == "LT300";

            kv["SumD1_1"] = machine == "LT300"? "Skjutmått": valid ? "UD-Apparat/Skjutmått" : "";

            kv["SumD1_e"] = "";

            kv["SumD1_2"] =
                valid
                    ? (tmpD < 22
                        ? "mall:7422251/2"
                        : tmpD < 31
                            ? "mall:7422251/3"
                            : tmpD < 46
                                ? "mall:7422251/4"
                                : tmpD < 76
                                    ? "mall:7422251/5"
                                    : "mall:7422251/6")
                    : "";

            kv["SumD1_3"] = valid ? "Skjutmått" : "";
            kv["SumD1_4"] = valid ? "Skjutmått" : "";
            kv["SumD1_5"] = valid ? "Skjutmått" : "";
            kv["SumD1_6"] = valid ? "Ytjämnhetsmätare" : "";
            kv["SumD1_7"] = valid ? "Skjutmått" : "";
            kv["SumD1_8"] = valid ? "Skjutmått" : "";

            kv["SumD2_1"] = valid ? "Gängtolk" : "";
            kv["SumD2_2"] = valid ? "Gängtolk Combi" : "";
            kv["SumD2_3"] = valid ? "Skjutmått" : "";
            kv["SumD2_4"] = valid ? "Skjutmått" : "";
            kv["SumD2_5"] = valid ? "Skjutmått" : "";
            kv["SumD2_6"] = valid ? "Skjutmått" : "";
            kv["SumD2_7"] = valid ? "Skjutmått" : "";
            kv["SumD2_8"] = valid ? "Skjutmått" : "";
            kv["SumD2_9"] = valid ? "Ytjämnhetsmätare" : "";
        }

        static void SetAF(
            Dictionary<string, string> kv,
            string machine,
            int tmpTyp,
            string tmpe2Op1,
            string tmpd3Op1a)
        {
            bool valid = machine == "LVT300" || machine == "LR15/Kolsva" || machine == "LT300";

            kv["SumAF1_1"] = "";
            kv["SumAF1_e"] = "";

            kv["SumAF1_2"] =machine == "LT300"   ? tmpe2Op1: "";

            kv["SumAF1_3"] = machine == "LT300"  ? tmpd3Op1a: "";

            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = "";
            kv["SumAF1_6"] = "";
            kv["SumAF1_7"] = "";
            kv["SumAF1_8"] = "";

            kv["SumAF2_1"] = "";
            kv["SumAF2_2"] = "";

            kv["SumAF2_3"] = valid? "Passas in med skruv, " +(tmpTyp < 17 ? "M8X8" : tmpTyp < 48 ? "M10X10": "M12X14"): "";

            kv["SumAF2_4"] = "";
            kv["SumAF2_5"] = "";
            kv["SumAF2_6"] = "";
            kv["SumAF2_7"] = "";
            kv["SumAF2_8"] = "";
            kv["SumAF2_9"] = "";
        }

        static int ParseInt(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0;

            int.TryParse(value, out int result);

            return result;
        }

        static string Smart(double value)
        {
            if (Math.Abs(value % 1) < 0.0001)
            {
                return value
                    .ToString("F0", CommonFunctions.Culture).Replace(".", ",");
            }

            return value
                .ToString(CommonFunctions.Culture).Replace(".", ",");
        }

        static string F3(double value)
        {
            return value
                .ToString("F3", CommonFunctions.Culture).Replace(",", ".");
        }

        static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published))
                return "";

            if (!DateTime.TryParse(
                    published,
                    out DateTime publishDate))
            {
                return "";
            }

            DateTime validTo =
                publishDate.AddDays(14);

            if (DateTime.Today > validTo)
                return "";

            return
                "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom 14 dagar)" + LB + "Popupruta aktiv till " + validTo.ToString("yyyy-MM-dd");
        }
    }
}