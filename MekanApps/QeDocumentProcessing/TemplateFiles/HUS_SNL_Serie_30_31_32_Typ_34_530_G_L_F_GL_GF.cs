using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HUS_SNL_Serie_30_31_32_Typ_34_530_G_L_F_GL_GF : ITemplateCalculations
    {
        private const string LB = "<<LineBreak>>";

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req?.ProductDesignation ?? "";
            string maskinVal = req?.MachineNumber ?? "";

            kv["VaLPopUp"] = ComputePopup(req?.Published);

            string tmpTrim = subject.Trim().ToUpperInvariant();
            string tmpBet32 = tmpTrim.Replace(".", ",");
            string[] tmpBetLista32 = Split(tmpBet32, " /.-");

            string tmpBet2Dash32 = Item(tmpBetLista32, 2);
            bool tmpG32 = ContainsI(tmpBet32, "G");

            string tmpSerie32 = Left(tmpBet2Dash32, 2);
            string tmpTyp32Text = Right(tmpBet2Dash32, 2);
            double tmpTyp32 = ParseDouble(tmpTyp32Text);

            string tmpBet1_32 = Item(tmpBetLista32, 1);
            string tmpBet2_32 = Item(tmpBetLista32, 2);
            string tmpBet3_32 = Item(tmpBetLista32, 3);
            string tmpBet4_32 = Item(tmpBetLista32, 4);

            string converted32 = tmpBet2_32 == "3234" ? "3040" : tmpBet2_32 == "3236" ? "3138" :
                tmpBet2_32 == "3238" ? "3140" : tmpBet2_32 == "3240" ? "3048" : tmpBet2_32 == "3244" ? "3148" :
                tmpBet2_32 == "3248" ? "3152" : tmpBet2_32 == "3252" ? "3064" : tmpBet2_32 == "3256" ? "3160" :
                tmpBet2_32 == "3260" ? "3164" : tmpBet2_32 == "3264" ? "3168" : tmpBet2_32 == "3268" ? "3176" :
                tmpBet2_32 == "3272" ? "3180" : tmpBet2_32 == "3276" ? "3092" : tmpBet2_32 == "3280" ? "3188" :
                tmpBet2_32 == "3284" ? "3192" : tmpBet2_32 == "3288" ? "3196" : "0";

            string converted32G = tmpBet2_32 == "3234" ? "3040" : tmpBet2_32 == "3236" ? "3138 G" :
                tmpBet2_32 == "3238" ? "3140 G" : tmpBet2_32 == "3240" ? "3048" : tmpBet2_32 == "3244" ? "3148 G" :
                tmpBet2_32 == "3248" ? "3152 G" : tmpBet2_32 == "3252" ? "3064" : tmpBet2_32 == "3256" ? "3160 G" :
                tmpBet2_32 == "3260" ? "3164 G" : tmpBet2_32 == "3264" ? "3168 G" : tmpBet2_32 == "3268" ? "3176" :
                tmpBet2_32 == "3272" ? "3180" : tmpBet2_32 == "3276" ? "3092" :  tmpBet2_32 == "3280" ? "3188 G" :
                tmpBet2_32 == "3284" ? "3192 G" : tmpBet2_32 == "3288" ? "3196 G" : "0";

            string tmpKonLista = "SNL " + converted32 + " " + tmpBet3_32;

            string tmpKonListaG = "SNL " + converted32G;

            string tmpFormat = tmpSerie32 == "32" ? tmpG32 ? tmpKonListaG : tmpKonLista : tmpBet32;

            string tmpBet = tmpFormat.Replace(".", ",");
            string[] tmpBetLista = Split(tmpBet, " /.-");

            string tmpBet1 = Item(tmpBetLista, 1);
            string tmpBet2 = Item(tmpBetLista, 2);
            string tmpBet3 = Item(tmpBetLista, 3);
            string tmpBet4 = Item(tmpBetLista, 4);

            bool tmpSlash = tmpBet.Contains("/");

            string variantText = tmpBet3 + " | " + tmpBet4;

            bool tmpAllF = ContainsI(variantText, "F");
            bool tmpAllL = ContainsI(variantText, "L");
            bool tmpAllG = ContainsI(variantText, "G");
            bool tmpNone = !tmpAllL && !tmpAllF && !tmpAllG;

            string tmpART = tmpBet1;
            string tmpSerie = Left(tmpBet2, 2);
            double tmpTyp = tmpSlash ? ParseDouble(tmpBet3) : ParseDouble(Right(tmpBet2, 2));

            string[] tmp30TypValues =
            {
                "36","38","40","44","48","52","56","60","64","68", "72","76","80","84","88","92","96","500","530"
            };

            string[] tmp31TypValues =
            {
                "34","36","38","40","44","48","52","56","60", "64","68","72","76","80","84","88","92","96"
            };

            string[] tmp32TypValues =
            {
                "34","36","38","40","44","48","52","56", "60","64","68","72","76","80","84","88"
            };

            int tmp30TypLista = Member(tmpTyp, tmp30TypValues);
            int tmp31TypLista = Member(tmpTyp, tmp31TypValues);
            int tmp32TypLista = Member(tmpTyp32, tmp32TypValues);

            int tmpTypLista = tmpSerie == "30" ? tmp30TypLista : tmpSerie == "31" ? tmp31TypLista : tmp32TypLista;

            int tmpTyp32Lista = tmp32TypLista;

            kv["VaLTypLista"] = tmpTypLista == 0 ? "Produktbeteckningen ingår ej i mallen, kontrollera stavning och/eller kontakta Qe-Admin för att lägga till produkten" : "";

            if (tmpTypLista == 0)
                return kv;

            kv["SumRa32"] = "3.2";
            kv["SumRa32a"] = "3.2";
            kv["SumRa63"] = "6.3 [3]";
            kv["SumWt35"] = "Wt 35";
            kv["SumWt20"] = "Wt 20";

            string tmp30AdLista = "{181,2:191,4:201,4:221,4:241,4:261,6:281,6:301,6:321,8:342,4:362,4:382,4:402,8:422,8:442,8:463:483:503:533}";
            string tmp30AdListaG = "{221,4:221,4:241,4:261,6:281,6:301,6:321,8:342,4:362,4:382,4:402,8:422,8:463:483:503:533:533:563:603}";
            string tmp31AdLista = "{171,2:181,2:191,4:201,4:221,4:241,4:261,6:281,6:301,6:321,8:342,4:362,4:382,4:402,8:422,8:442,8:463:483}";
            string tmp31AdListaG = "{201,4:221,4:221,4:241,4:261,6:281,6:301,6:321,8:342,4:362,4:382,4:402,8:422,8:463:483:503:533:563}";

            string tmpAdLista = tmpAllG ? tmpSerie == "30" ? tmp30AdListaG : tmp31AdListaG: tmpSerie == "30" ? tmp30AdLista : tmp31AdLista;

            double tmpAd = ParseDouble(ListItem(tmpAdLista, tmpTypLista));

            double tmpAdTol = H12Tolerance(tmpAd);

            kv["SumAd"] = "(Ad) " + SmartDot(tmpAd);
            kv["SumAdTol"] = "+ " + F3(tmpAdTol) + " [3]";
            kv["SumAdTolN"] = "- 0 [3]";

            string tmpAbLista = "{214:224:242:262:270:286:304:304:334:354:384:384:384:414:444:444:454:454}";

            int tmpAbPosition = tmpTypLista;

            if (tmpSerie == "30")
            {
                tmpAbPosition = tmpTyp > 92 && tmpTyp < 529 ? tmp31TypLista - 3 :
                    tmpTyp > 68 && tmpTyp < 529 ? tmp31TypLista - 2 : tmp31TypLista - 1;
            }

            double tmpAb = ParseDouble(ListItem(tmpAbLista, tmpAbPosition));

            double tmpAbTol = tmpAb < 6.01 ? 0.1 : tmpAb < 30.01 ? 0.2 : tmpAb < 120.01 ? 0.3 :
                tmpAb < 400.01 ? 0.5 : tmpAb < 1000.01 ? 0.8 : tmpAb < 2000.01 ? 1.2 : 2.0;

            kv["SumAb"] = "(Ab) " + SmartDot(tmpAb);
            kv["SumAbTol"] = "± " + F3(tmpAbTol);

            double tmpPd = tmpAd + 24;
            double tmpPdTol = H12Tolerance(tmpPd);

            kv["SumPd"] = "(Pd) " + SmartDot(tmpPd);
            kv["SumPdTol"] = "+ " + F3(tmpPdTol) + " [3]";
            kv["SumPdTolN"] = "- 0 [3]";

            string tmp30LdLista = "{280:290:310:340:360:400:420:460:480:520:540:560:600:620:650:680:700:720:780}";

            string tmp31LdLista = "{280:300:320:340:370:400:440:460:500:540:580:600:620:650:700:720:760:790}";

            string tmpLdLista = tmpSerie == "30" ? tmp30LdLista : tmp31LdLista;

            double tmpLd = ParseDouble(ListItem(tmpLdLista, tmpTypLista));

            bool useG7 = tmpSerie == "30" ? tmpTyp < 76 : tmpTyp < 68;

            double tmpLdTol = useG7 ? G7UpperTolerance(tmpLd) : F7UpperTolerance(tmpLd);

            double tmpLdTolN = useG7 ? G7LowerTolerance(tmpLd) : F7LowerTolerance(tmpLd);

            kv["SumLd"] = "(Ld) " + SmartDot(tmpLd);
            kv["SumLdTol"] = "+ " + F3(tmpLdTol) + " [3]";
            kv["SumLdTolN"] = "+ " + F3(tmpLdTolN) + " [2]";
    
            string tmp30LbLista = tmpAllF ? "{108:115:122:130:140:148:166:168:181:197:198:135:148:150:157:163:165:167:185}"
                : "{108:115:122:130:140:148:166:168:181:197:198:180:192:194:200:224:224:226:248}";

            string tmp31LbLista = tmpAllF ? "{108:116:124:132:140:148:164:166:180:196:190:192:194:200:224:226:240:248}"
                : "{108:116:124:132:140:148:164:166:180:196:210:212:214:220:244:246:260:268}";

            string tmp32LbLista = tmpAllF ? "{122:124:132:140:164:180:194:196:212:208:224:232:240:256:272:280}"
                : "{122:124:132:140:164:180:194:196:212:228:234:252:260:276:282:290}";

            string tmpLbLista = tmpSerie32 == "32" ? tmp32LbLista : tmpSerie == "30" ? tmp30LbLista :
                tmpSerie == "31" ? tmp31LbLista : "No List";

            int tmpLbPosition = tmpSerie32 == "32" ? tmpTyp32Lista : tmpTypLista;
            double tmpLb = ParseDouble(ListItem(tmpLbLista, tmpLbPosition));
            double tmpLbTol = H12Tolerance(tmpLb);

            kv["SumLb"] = "(Lb) " + SmartDot(tmpLb);
            kv["SumLbTol"] = "+ " + F3(tmpLbTol) + " [3]";
            kv["SumLbTolN"] = "- 0 [2]";

            string tmp30UdLista = tmpAllG ? "{236,4:236,4:256,4:276,6:296,6:316,6:336,8:357,4:377,4:397,4:417,8:437,8:478:498:518:548:548:578:618}"
                : "{196,4:206,4:216,4:236,4:256,4:276,6:296,6:316,6:336,8:357,4:377,4:397,4:417,8:437,8:457,8:478:498:518:548}";

            string tmp31UdLista = tmpAllG ? "{216,4:236,4:236,4:256,4:276,6:296,6:316,6:336,8:357,4:377,4:397,4:417,8:437,8:478:498:518:548:578}"
                : "{186,4:196,4:206,4:216,4:236,4:256,4:276,6:296,6:316,8:336,8:357,4:377,4:397,4:417,8:437,8:457,8:478:498}";

            string tmpUdLista = tmpSerie == "30" ? tmp30UdLista : tmp31UdLista;
            double tmpUd = ParseDouble(ListItem(tmpUdLista, tmpTypLista));
            double tmpUdTol = H12Tolerance(tmpUd);

            kv["SumUd"] = "(Ud) " + SmartDot(tmpUd);
            kv["SumUdTol"] = "+ " + F3(tmpUdTol) + " [3]";
            kv["SumUdTolN"] = "- 0 [3]";

            double tmpUb = 11;
            double tmpUbTol = tmpUb < 3.01 ? 0.140 :  tmpUb < 6.01 ? 0.180 : tmpUb < 10.01 ? 0.220 :
                tmpUb < 18.01 ? 0.270 : tmpUb < 30.01 ? 0.330 : 0.390;

            kv["SumUb"] = "(Ub) " + SmartDot(tmpUb);
            kv["SumUbTol"] = "+ " + F3(tmpUbTol) + " [3]";
            kv["SumUbTolN"] = "- 0 [3]";

            double tmpUb1 = 5.5;

            kv["SumUb1"] = "(Ub1) " + SmartDot(tmpUb1);
            kv["SumUb1Tol"] = "+ 0 [3]";
            kv["SumUb1TolN"] = "- 0.500 [3]";

            double tmpUb2 = 22;
            double tmpUb2TolN = tmpUb2 < 3.01 ? 0.400 : tmpUb2 < 6.01 ? 0.480 : tmpUb2 < 10.01 ? 0.580 :
                tmpUb2 < 18.01 ? 0.700 : tmpUb2 < 30.01 ? 0.840 : tmpUb2 < 50.01 ? 1.000 : 1.200;

            kv["SumUb2"] = "(Ub2) " + SmartDot(tmpUb2);
            kv["SumUb2Tol"] = "+ 0 [3]";
            kv["SumUb2TolN"] = "- " + F3(tmpUb2TolN) + " [3]";

            string tmpHdLista = "{315:335:360:378:408:450:490:510:550:590:640:660:680:730:780:800:840:870}";

            int tmpHdPosition = tmpTypLista;

            if (tmpSerie == "30")
            {
                tmpHdPosition = tmpTyp > 92 && tmpTyp < 529 ? tmp31TypLista - 3 :
                    tmpTyp > 68 && tmpTyp < 529 ? tmp31TypLista - 2 : tmp31TypLista - 1;
            }

            double tmpHd = ParseDouble(ListItem(tmpHdLista, tmpHdPosition));

            double tmpHdTol = tmpHd < 3.01 ? 0.070 : tmpHd < 6.01 ? 0.090 : tmpHd < 10.01 ? 0.110 :
                tmpHd < 18.01 ? 0.135 : tmpHd < 30.01 ? 0.165 : tmpHd < 50.01 ? 0.195 : tmpHd < 80.01 ? 0.230 :
                tmpHd < 120.01 ? 0.270 : tmpHd < 180.01 ? 0.315 : tmpHd < 250.01 ? 0.360 : tmpHd < 315.01 ? 0.405 :
                tmpHd < 400.01 ? 0.445 : tmpHd < 500.01 ? 0.485 : tmpHd < 630.01 ? 0.550 : tmpHd < 800.01 ? 0.625 :
                tmpHd < 1000.01 ? 0.700 : tmpHd < 1250.01 ? 0.825 : tmpHd < 1600.01 ? 0.975 : tmpHd < 2000.01 ? 1.150 :
                tmpHd < 2500.01 ? 1.400 : 1.650;

            kv["SumHd"] = "(Hd) " + SmartDot(tmpHd);
            kv["SumHdTol"] = "± " + F3(tmpHdTol) + " [3]";

            double tmpGd = 12;

            kv["SumG"] = "3x (G) 1/8 - 27NPSF";

            double tmpG1 = tmpSerie == "31" ? tmpTyp < 48 ? 24 : tmpTyp < 68 ? 30 : tmpTyp < 84 ? 36 : 42
                : tmpTyp < 52 ? 24 : tmpTyp < 76 ? 30 : tmpTyp < 92 ? 36 : 42;

            kv["SumG1"] = "(G1) M" + SmartDot(tmpG1) + " 6H";

            string tmpG2Lista = "{16:16:20:20:20:24:24:24:30:30:30:36:36:42:42:42:48:48}";

            int tmpG2Position = tmpTypLista;

            if (tmpSerie == "30")
            {
                tmpG2Position = tmpTyp > 92 && tmpTyp < 529 ? tmp31TypLista - 3 : tmpTyp > 68 && tmpTyp < 529 ? tmp31TypLista - 2 : tmp31TypLista - 1;
            }

            double tmpG2 = ParseDouble(ListItem(tmpG2Lista, tmpG2Position));
            double tmpG2d = tmpG2 == 16 ? 28 : tmpG2 == 20 ? 31 : tmpG2 == 24 ? 37 : 46;

            kv["SumG2"] = "(G2) M" + SmartDot(tmpG2);

            double tmpBd = tmpG1 == 24 ? 27 : tmpG1 == 30 ? 33 : tmpG1 == 36 ? 39 : 45;
            double tmpBdTol = tmpBd < 10.01 ? 0.580 : tmpBd < 18.01 ? 0.700 : tmpBd < 30.01 ? 0.840 : 1.000;

            kv["SumBd"] = "(Bd) " + SmartDot(tmpBd);
            kv["SumBdTol"] = "+ " + F3(tmpBdTol) + " [3]";
            kv["SumBdTolN"] = "- 0 [3]";

            double tmpFDimension = tmpBd + 1;
            double tmpFTol = tmpFDimension < 10.01 ? 0.580 : tmpFDimension < 18.01 ? 0.700 : tmpFDimension < 30.01 ? 0.840 : 1.000;

            kv["SumF"] = "(F) " + SmartDot(tmpFDimension);
            kv["SumFTol"] = "+ " + F3(tmpFTol);
            kv["SumFTolN"] = "- 0";

            double tmpSd = tmpSerie == "30" && tmpTyp > 38 ? 16 : tmpSerie == "31" && tmpTyp > 36 ? 16 : 9.335;

            bool smallPin = Math.Abs(tmpSd - 9.335) < 0.000001;
            double tmpSdTol = smallPin ? 0.036 : 0.180;
            string tmpSdClass = smallPin ? " [2]" : "";

            kv["SumSd"] = "2x (Sd) " + SmartDot(tmpSd);
            kv["SumSdTol"] = "+ " + F3(tmpSdTol) + tmpSdClass;
            kv["SumSdTolN"] = "- 0" + tmpSdClass;

            bool specialSo = tmpBet2 == "3134" || tmpBet2 == "3136" || tmpBet2 == "3036" || tmpBet2 == "3038";

            double tmpSo = specialSo ? 10 : smallPin ? tmpSerie == "30" || tmpSerie == "31" ? 10.5 : 10 : 20;

            double tmpSoTol = smallPin ? 0 : 0.420;
            double tmpSoTolN = smallPin ? 0.500 : 0.420;

            kv["SumSö"] = "(Sö) " + SmartDot(tmpSo);
            kv["SumSöTol"] = "+ " + FormatLotusConstant(tmpSoTol);
            kv["SumSöTolN"] = "- " + FormatLotusConstant(tmpSoTolN);

            double tmpSu = smallPin ? 10.5 : 20;
            double tmpSuTol = smallPin ? 0 : 0.420;
            double tmpSuTolN = smallPin ? 0.500 : 0.420;

            kv["SumSu"] = "(Su) " + SmartDot(tmpSu);
            kv["SumSuTol"] = "+ " + FormatLotusConstant(tmpSuTol) + " [3]";
            kv["SumSuTolN"] = "- " + FormatLotusConstant(tmpSuTolN) + " [3]";

            string tmpUhLista = "{170:180:190:210:220:240:260:280:300:320:340:350:360:380:410:420:440:460}";

            int tmpUhPosition = tmpTypLista;

            if (tmpSerie == "30")
            {
                tmpUhPosition = tmpTyp > 92 && tmpTyp < 529 ? tmp31TypLista - 3 : tmpTyp > 68 && tmpTyp < 529 ? tmp31TypLista - 2 : tmp31TypLista - 1;
            }

            double tmpUh = ParseDouble(ListItem(tmpUhLista, tmpUhPosition));

            double tmpUhTol = tmpUh < 80.01 ? 0.095 : tmpUh < 120.01 ? 0.110 : tmpUh < 180.01 ? 0.125 :
                tmpUh < 250.01 ? 0.145 : tmpUh < 315.01 ? 0.160 : tmpUh < 400.01 ? 0.180 : tmpUh < 500.01 ? 0.200 : 0.220;

            kv["SumUh"] = "(Uh) " + SmartDot(tmpUh);
            kv["SumUhTol"] = "± " + F3(tmpUhTol) + " [3]";
            kv["SumCh"] = "(Ch) " + SmartDot(tmpUh);
            kv["SumChTol"] = kv["SumUhTol"];

            string tmpFhLista = "{70:75:80:85:90:95:100:105:110:115:120:120:120:125:130:135:145:155}";

            int tmpFhPosition = tmpTypLista;

            if (tmpSerie == "30")
            {
                tmpFhPosition = tmpTyp > 92 && tmpTyp < 529 ? tmp31TypLista - 3 : tmpTyp > 68 && tmpTyp < 529 ? tmp31TypLista - 2 : tmp31TypLista - 1;
            }

            double tmpFh = ParseDouble(ListItem(tmpFhLista, tmpFhPosition));

            kv["SumFh"] = "(Fh) " + SmartDot(tmpFh);
            kv["SumFhTol"] = "± 1.0";

            string tmpBpLista = "{92:92:102:112:112:127:137:147:152:152:176:179:179:179:195:203:223:238}";

            int tmpBpPosition = tmpTypLista;

            if (tmpSerie == "30")
            {
                tmpBpPosition = tmpTyp > 92 && tmpTyp < 529 ? tmp31TypLista - 3 : tmpTyp > 68 && tmpTyp < 529 ? tmp31TypLista - 2 : tmp31TypLista - 1;
            }

            double tmpBp = ParseDouble(ListItem(tmpBpLista, tmpBpPosition));

            kv["SumBp"] = "(Bp) " + SmartDot(tmpBp);
            kv["SumBpTol"] = "± 1.0";
            kv["SumSs"] = "(Ss) 0.500";
            kv["SumSsTol"] = "+ 0.300";
            kv["SumSsTolN"] = "- 0 [2]";

            double tmpPl = 0.05;
            double tmpFp = tmpTyp < 60 ? 0.10 : 0.12;

            kv["SumPl"] = F2(tmpPl);
            kv["SumFp"] = F2(tmpFp);

            kv["SumSkr"] = "8.8 SNL.";
           
            bool machineEnabled = maskinVal == "OKUMA MA600/Trevisan DS 450" || maskinVal == "Trevisan DS 900";

            string tmpMaskinValS1 = maskinVal == "OKUMA MA600/Trevisan DS 450" ? "Trevisan DS 450" :
                maskinVal == "Trevisan DS 900" ? "Trevisan DS 900" : "";

            string tmpMaskinValS2 = maskinVal == "OKUMA MA600/Trevisan DS 450" ? "OKUMA MA600" : 
                maskinVal == "Trevisan DS 900" ? "Trevisan DS 900" : "";

            kv["SumMaskinValS1"] = "OP 2 - " + tmpMaskinValS1 + " - Svarvning";

            kv["SumMaskinValS2"] = "OP 1 - " + tmpMaskinValS2 + " - Borrning, fräsning";

            kv["SumF1_1"] = machineEnabled ? "1/tim" : "";
            kv["SumF1_2"] = machineEnabled ? "1/tim" : "";
            kv["SumF1_3"] = machineEnabled ? "1/1" : "";
            kv["SumF1_4"] = machineEnabled ? "1/1" : "";
            kv["SumF1_5"] = machineEnabled ? "1/tim & inst." : "";
            kv["SumF1_6"] = machineEnabled ? "Inst." : "";
            kv["SumF1_7"] = machineEnabled ? "Inst." : "";
            kv["SumF1_8"] = machineEnabled ? "1/tim & Inst." : "";

            kv["SumF2_1"] = machineEnabled ? "Inst." : "";
            kv["SumF2_2"] = machineEnabled ? "Inst." : "";
            kv["SumF2_3"] = machineEnabled ? "Inst./borrbyte" : "";
            kv["SumF2_4"] = machineEnabled ? "Inst." : "";
            kv["SumF2_5"] = machineEnabled ? "Inst." : "";
            kv["SumF2_6"] = machineEnabled ? "Inst./borrbyte" : "";
            kv["SumF2_7"] = machineEnabled ? "Inst." : "";
            kv["SumF2_8"] = machineEnabled ? "Inst." : "";
            kv["SumF2_9"] = machineEnabled ? "Inst." : "";
            kv["SumF2_10"] = machineEnabled ? "Inst." : "";
            kv["SumF2_11"] = machineEnabled ? "Inst." : "";
            kv["SumF2_12"] = machineEnabled ? "Inst." : "";
            kv["SumF2_13"] = machineEnabled ? "2/skift" : "";
            kv["SumF2_14"] = machineEnabled ? "Inst." : "";

            kv["SumD1_1"] = machineEnabled ? "Skjutmått/Tolk" : "";
            kv["SumD1_2"] = machineEnabled ? "Skjutmått" : "";
            kv["SumD1_3"] = machineEnabled ? "Subito" : "";
            kv["SumD1_4"] = machineEnabled ? "Skjutmått/Tolk" : "";
            kv["SumD1_5"] = machineEnabled ? "Skjutmått/passbitar" : "";
            kv["SumD1_6"] = machineEnabled ? "Skjutmått" : "";
            kv["SumD1_7"] = machineEnabled ? "Höjdmätningsapparat" : "";
            kv["SumD1_8"] = machineEnabled ? "Skjutmått" : "";

            kv["SumD2_1"] = machineEnabled ? "Gängtolk" : "";
            kv["SumD2_2"] = machineEnabled ? "Gängtolk/Skjutmått" : "";
            kv["SumD2_3"] = machineEnabled ? "Gängtolk min/max" : "";
            kv["SumD2_4"] = machineEnabled ? "Skjutmått/Okulärt" : "";
            kv["SumD2_5"] = machineEnabled ? "Skjutmått" : "";
            kv["SumD2_6"] = machineEnabled ? "Skjutmått/Tolk" : "";
            kv["SumD2_7"] = machineEnabled ? "Skjutmått" : "";
            kv["SumD2_8"] = machineEnabled ? "Skjutmått" : "";
            kv["SumD2_9"] = machineEnabled ? "Skjutmått" : "";
            kv["SumD2_10"] = machineEnabled ? "Skjutmått" : "";
            kv["SumD2_11"] = machineEnabled ? "Kännbleck" : "";
            kv["SumD2_12"] = machineEnabled ? "Skjutmått" : "";
            kv["SumD2_13"] = machineEnabled ? "Ytjämnhetsmätare" : "";
            kv["SumD2_14"] = machineEnabled ? "Kännbleck" : "";

            kv["SumAF1_1"] = "";
            kv["SumAF1_2"] = "";
            kv["SumAF1_3"] = maskinVal == "OKUMA MA600/Trevisan DS 450" ? "Kontrolleras i mätbänk": "";

            kv["SumAF1_4"] = "";
            kv["SumAF1_5"] = "";
            kv["SumAF1_6"] = "";
            kv["SumAF1_7"] = machineEnabled ? "Ind. från lagerläge" : "";
            kv["SumAF1_8"] = "";

            kv["SumAF2_1"] = machineEnabled ? "Genomgående hål" : "";
            kv["SumAF2_2"] = machineEnabled ? "Min. gängdjup: " + SmartDot(tmpGd) : "";
            kv["SumAF2_3"] = machineEnabled ? "Min. gängdjup: " + SmartDot(tmpG2d) : "";
            kv["SumAF2_4"] = "";
            kv["SumAF2_5"] = "";
            kv["SumAF2_6"] = "";
            kv["SumAF2_7"] = "";
            kv["SumAF2_8"] = "";
            kv["SumAF2_9"] = "";
            kv["SumAF2_10"] = "";
            kv["SumAF2_11"] = machineEnabled ? "Infettas, Mått: 0.05 ihopsatt hus." : "";
            kv["SumAF2_12"] = "";
            kv["SumAF2_13"] = "";
            kv["SumAF2_14"] = machineEnabled ? "kontroll mot planskiva, mått: 0.1" : "";

            kv["SumTextS1"] = "Kontrolleras enl. styrplan.";
            kv["SumTextS2"] = kv["SumTextS1"] + " För varianter se HLS planningtool för ritningsnummer. " + "Alla materialdefekter utsorteras.";

            string productDrawing;

            if (tmpNone)
            {
                productDrawing = tmpSerie32 == "30" ? tmpTyp < 76 ? "7435405" : "7438730" : tmpSerie32 == "32"
                            ? "7439179" : tmpBet;
            }
            else
            {
                productDrawing = tmpSerie32 == "30" ? tmpTyp < 76 ? "7435405" : "7438732" : tmpSerie32 == "31"
                            ? tmpTyp < 68 ? "7435406" : "7438731" : "7435407";
            }

            kv["SumPrdritS1"] = "Produktritning: " + productDrawing;
            kv["SumPrdritS2"] = kv["SumPrdritS1"];

            kv["SumArbInstGjgS1"] = "Gjutgods: A3.022";
            kv["SumArbInstGjgS2"] = kv["SumArbInstGjgS1"];

            kv["SumKvStPlS1"] = "Kvalitetstyrning: K1.07-17";
            kv["SumKvStPlS2"] = kv["SumKvStPlS1"];

            kv["TmpMTxtUtf1"] = "Passbitklove Utf. 1 med diametern: " + SmartDot(tmpLd);

            kv["VaLFärdig"] = "";
            kv["VaLInfo"] = "";

            return kv;
        }
        private static double H12Tolerance(double value) => value < 3.01 ? 0.100 : value < 6.01 ? 0.120 :
            value < 10.01 ? 0.150 : value < 18.01 ? 0.180 : value < 30.01 ? 0.210 : value < 50.01 ? 0.250 :
            value < 80.01 ? 0.300 : value < 120.01 ? 0.350 : value < 180.01 ? 0.400 : value < 250.01 ? 0.460 :
            value < 315.01 ? 0.520 : value < 400.01 ? 0.570 : value < 500.01 ? 0.630 : value < 630.01 ? 0.700 :
            value < 800.01 ? 0.800 : value < 1000.01 ? 0.900 : value < 1250.01 ? 1.050 : value < 1600.01 ? 1.250 :
            value < 2000.01 ? 1.500 : value < 2500.01 ? 1.750 : 2.100;

        private static double G7UpperTolerance(double value) => value < 3.01 ? 0.012 : value < 6.01 ? 0.016 :
            value < 10.01 ? 0.020 : value < 18.01 ? 0.024 : value < 30.01 ? 0.028 : value < 50.01 ? 0.034 :
            value < 80.01 ? 0.040 : value < 120.01 ? 0.047 : value < 180.01 ? 0.054 : value < 250.01 ? 0.061 :
            value < 315.01 ? 0.069 : value < 400.01 ? 0.075 : value < 500.01 ? 0.083 : value < 630.01 ? 0.092 :
            value < 800.01 ? 0.104 : value < 1000.01 ? 0.116 : value < 1250.01 ? 0.133 : value < 1600.01 ? 0.155 :
            value < 2000.01 ? 0.182 : value < 2500.01 ? 0.209 : 0.248;

        private static double G7LowerTolerance(double value) => value < 3.01 ? 0.002 : value < 6.01 ? 0.004 :
            value < 10.01 ? 0.005 : value < 18.01 ? 0.006 : value < 30.01 ? 0.007 : value < 50.01 ? 0.009 :
            value < 80.01 ? 0.010 : value < 120.01 ? 0.012 : value < 180.01 ? 0.014 : value < 250.01 ? 0.015 :
            value < 315.01 ? 0.017 : value < 400.01 ? 0.018 : value < 500.01 ? 0.020 : value < 630.01 ? 0.022 :
            value < 800.01 ? 0.024 : value < 1000.01 ? 0.026 : value < 1250.01 ? 0.028 : value < 1600.01 ? 0.030 :
            value < 2000.01 ? 0.032 : value < 2500.01 ? 0.034 : 0.038;

        private static double F7UpperTolerance(double value) => value < 3.01 ? 0.016 : value < 6.01 ? 0.022 :
            value < 10.01 ? 0.028 : value < 18.01 ? 0.034 : value < 30.01 ? 0.041 : value < 50.01 ? 0.050 :
            value < 80.01 ? 0.060 : value < 120.01 ? 0.071 : value < 180.01 ? 0.083 : value < 250.01 ? 0.096 :
            value < 315.01 ? 0.108 : value < 400.01 ? 0.119 : value < 500.01 ? 0.131 : value < 630.01 ? 0.146 :
            value < 800.01 ? 0.160 : value < 1000.01 ? 0.176 : value < 1250.01 ? 0.203 : value < 1600.01 ? 0.235 : 0.270;

        private static double F7LowerTolerance(double value) => value < 3.01 ? 0.006 : value < 6.01 ? 0.010 :
            value < 10.01 ? 0.013 : value < 18.01 ? 0.016 : value < 30.01 ? 0.020 : value < 50.01 ? 0.025 :
            value < 80.01 ? 0.030 : value < 120.01 ? 0.036 : value < 180.01 ? 0.043 : value < 250.01 ? 0.050 :
            value < 315.01 ? 0.056 : value < 400.01 ? 0.062 : value < 500.01 ? 0.068 : value < 630.01 ? 0.076 :
            value < 800.01 ? 0.080 : value < 1000.01 ? 0.086 : value < 1250.01 ? 0.098 : value < 1600.01 ? 0.110 : 0.120;

        private static int Member(double value, string[] values)
        {
            if (values == null) return 0;

            string searchValue = SmartInvariant(value);

            for (int i = 0; i < values.Length; i++)
                if (string.Equals(values[i], searchValue,StringComparison.Ordinal))
                    return i + 1;

            return 0;
        }

        private static string[] Split(string value, string separators)
        {
            if (string.IsNullOrEmpty(value))
                return Array.Empty<string>();

            return value.Split(separators.ToCharArray(),StringSplitOptions.RemoveEmptyEntries);
        }

        private static string Item(string[] values,int lotusPosition) => values == null || lotusPosition < 1 ||
            lotusPosition > values.Length ? "" : values[lotusPosition - 1];

        private static string ListItem(string list, int lotusPosition)
        {
            if (string.IsNullOrWhiteSpace(list))
                return "";

            string[] values = list.Trim().TrimStart('{').TrimEnd('}').Split(':');

            if (lotusPosition == 0)
                lotusPosition = 1;

            if (lotusPosition > 0)
            {
                return lotusPosition <= values.Length ? values[lotusPosition - 1] : "";
            }
            int index = values.Length + lotusPosition;

            return index >= 0 && index < values.Length ? values[index] : "";
        }

        private static double ParseDouble(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0;

            return double.TryParse(value.Trim().Replace(",", "."),NumberStyles.Any,CultureInfo.InvariantCulture,out double result) ? result: 0;
        }

        private static string SmartDot(double value) => Math.Abs(value % 1) < 0.0000001 ? 
            value.ToString("F0", CultureInfo.InvariantCulture): value.ToString("0.###", CultureInfo.InvariantCulture);

        private static string SmartInvariant(double value) => Math.Abs(value % 1) < 0.0000001 ? 
            value.ToString("F0",CultureInfo.InvariantCulture): value.ToString("0.###", CultureInfo.InvariantCulture);

        private static string F2(double value) => value.ToString("F2",CultureInfo.InvariantCulture);

        private static string F3(double value) => value.ToString("F3",CultureInfo.InvariantCulture);

        private static string FormatLotusConstant(double value)
        {
            if (Math.Abs(value) < 0.0000001)
                return "0";

            return F3(value);
        }

        private static string Left(string value,int length) => string.IsNullOrEmpty(value) || length <= 0 ? ""
                : value.Substring(0, Math.Min(length, value.Length));

        private static string Right(string value,int length) => string.IsNullOrEmpty(value) || length <= 0 ? ""
                : value.Length <= length ? value : value.Substring(value.Length - length);

        private static bool ContainsI(string source, string value) =>!string.IsNullOrEmpty(source) && 
            !string.IsNullOrEmpty(value) && source.IndexOf(value,StringComparison.OrdinalIgnoreCase) >= 0;
        private static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published))
                return "";

            if (!DateTime.TryParseExact(published, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None,out DateTime publishDate) &&
                !DateTime.TryParse(published,CultureInfo.InvariantCulture, DateTimeStyles.None,out publishDate) &&
                !DateTime.TryParse(published,out publishDate))
                return "";

            DateTime validTo = publishDate.AddDays(14);

            if (DateTime.Today > validTo.Date)
                return "";

            return
                "Denna kontrollinstruktion har nyligen blivit uppdaterad " + "(inom 14 dagar)" + LB + LB + LB + LB +
                "Popupruta aktiv till " + validTo.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
    }
}