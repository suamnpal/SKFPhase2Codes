using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;



namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Avdragshylsor_LW_7439219 : ITemplateCalculations
    {
        private static readonly string[] Machines = new[] { "VTR-160", "MacTurn 550" };



        private const double TmpL = 620.0;
        private const double Tmpd = 711.0;
        private const double Tmpd3 = 670.0;
        private const double Amatt = 20.0;
        private const double TmpKona = 30.0;
        private const int SumML = 310;
        private const int SumMLa = 100;
        private const int SumMLb = 100;



        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();



            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;



            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);



            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");



            bool tmpSlash = tmpBet.IndexOf('/') >= 0;
            bool tmpV21 = tmpBet.IndexOf("V21", StringComparison.OrdinalIgnoreCase) >= 0;



            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : string.Empty;
            int cntB2 = tmpBet2.Length;
            string tmpSerie =
            (cntB2 == 3 || cntB2 == 2) ? tmpBet2 :
            cntB2 > 4 ? tmpBet2.Substring(0, 3) :
            (tmpBet2.Length >= 2 ? tmpBet2.Substring(0, 2) : tmpBet2);



            string tmpTyp = "LW-7438844/V21";



            double tmp8 = Gauge(8);
            double tmp108 = Gauge(108);
            double tmp40 = Gauge(40);
            double tmp140 = Gauge(140);



            int sumL1 = SumML < 145 ? 8 : 40;
            int sumL2 = SumML < 145 ? 108 : 140;
            double sumE1 = SumML < 145 ? tmp8 : tmp40;
            double sumE2 = SumML < 145 ? tmp108 : tmp140;



            kv["SumML"] = SumML.ToString(CultureInfo.InvariantCulture);
            kv["SumMLa"] = SumMLa.ToString(CultureInfo.InvariantCulture);
            kv["SumMLb"] = SumMLb.ToString(CultureInfo.InvariantCulture);
            kv["SumL1"] = sumL1.ToString(CultureInfo.InvariantCulture);
            kv["SumL2"] = sumL2.ToString(CultureInfo.InvariantCulture);
            kv["SumE1"] = Fmt1(sumE1).Replace(".", ",");
            kv["SumE2"] = Fmt3(sumE2).Replace(".", ",");



            double tmpKonUtr = ((TmpL + Amatt) / TmpKona) + Tmpd;
            double tmpKonstOS = Math.Round(Math.Sin(10.0 * Math.PI / 180.0) * 2.0, 4);
            double tmpKordaInv = Math.Round((Tmpd3 / 2.0) * tmpKonstOS, 1);
            double tmpKordaUtv = Math.Round((tmpKonUtr / 2.0) * tmpKonstOS, 1);



            kv["SumKOSinv"] = Fmt(tmpKordaInv).Replace(".",",");
            kv["SumKOSutv"] = Fmt(tmpKordaUtv).Replace(".", ",");



            string bygGtj = SumML < 145 ? "SR 7415983" : "SR 7419470 el. 7415991";
            kv["SumBygGtj"] = bygGtj;



            int bygelKonst = SumML < 145 ? -5 : -10;
            double instE1 = sumL1 == 8 ? Math.Round(sumE1 - 5.0, 3) : Math.Round(sumE1 - 10.0, 3);
            double instE2 = sumL2 == 108 ? Math.Round(sumE2 - 5.0, 3) : Math.Round(sumE2 - 10.0, 3);
            kv["SumBygelinstkonst"] = bygelKonst.ToString(CultureInfo.InvariantCulture);
            kv["SumInstE1"] = Fmt3(instE1);
            kv["SumInstE2"] = Fmt3(instE2);



            kv["SumMaskinVal"] = "Maskin: " + maskinVal + " - OP1";
            kv["SumMaskinValS2"] = "Maskin: " + maskinVal + " - OP2";



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



        private static bool IsMachine(string mv)
        {
            if (string.IsNullOrEmpty(mv)) return false;
            for (int i = 0; i < Machines.Length; i++)
                if (string.Equals(Machines[i], mv, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }



        private static double Gauge(double offset)
        {
            return Math.Round(
            ((Tmpd - Tmpd3) / 2.0) + ((Amatt + offset) / (2.0 * TmpKona)),
            3);
        }



        private static string Fmt(double v) => v.ToString(CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt1(double v) => v.ToString(CommonFunctions.Culture).Replace(",", ".");
        private static string Fmt3(double v) => v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
    }
}