using System;
using System.Collections.Generic;
using System.Globalization;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Klamhylsa_ASA_0036_MacTurn_550_VTR_160 : ITemplateCalculations
    {
        private static readonly string[] Machines = new[] { "VTR-160", "MacTurn 550" };

        private const double TmpKona = 30.0;
        private const double TmpL = 885.0;
        private const double Tmpd = 1250.0;
        private const double Tmpd1 = 1180.0;
        private const double Amatt = 125.0;
        private const int TmpML = 700;
        private const int SumMLa = 100;

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");

            string[] tokens = tmpBet.Split(new char[] { ' ', '/', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string tmpBet2 = tokens.Length > 1 ? tokens[1] : string.Empty;
            int cntB2 = tmpBet2.Length;

            bool tmpSlash = tmpBet.IndexOf('/') >= 0;
            bool tmpASA = tmpBet.IndexOf("ASA", StringComparison.OrdinalIgnoreCase) >= 0;

            string tmpSerie =
                (cntB2 == 3 || cntB2 == 2) ? tmpBet2 :
                cntB2 > 4 ? tmpBet2.Substring(0, 3) :
                                              (tmpBet2.Length >= 2 ? tmpBet2.Substring(0, 2) : tmpBet2);

            string tmpTyp = "ASA-0036";

            double tmp8 = Gauge(8);
            double tmp83 = Gauge(83);
            double tmp108 = Gauge(108);
            double tmp40 = Gauge(40);
            double tmp140 = Gauge(140);

            int sumL1 = TmpML < 110 ? 83 : TmpML < 145 ? 108 : 140;
            int sumL2 = TmpML < 145 ? 8 : 40;
            double sumE1 = TmpML < 110 ? tmp83 : TmpML < 145 ? tmp108 : tmp140;
            double sumE2 = TmpML < 145 ? tmp8 : tmp40;

            kv["SumMLa"] = SumMLa.ToString(CultureInfo.InvariantCulture);
            kv["SumML"] = SumMLa.ToString(CultureInfo.InvariantCulture);
            kv["SumL1"] = sumL1.ToString(CultureInfo.InvariantCulture);
            kv["SumL2"] = sumL2.ToString(CultureInfo.InvariantCulture);
            kv["SumE1"] = sumE1.ToString("F3", CommonFunctions.Culture);
            kv["SumE2"] = sumE2.ToString("F3", CommonFunctions.Culture);

            string bygGtj = TmpML < 145 ? "SR 7419471" : "SR 7415991";
            kv["SumBygGtj"] = bygGtj;

            int bygelKonst = sumL1 == 50 ? -3 : (sumL1 == 83 || sumL1 == 108) ? -5 : sumL1 == 140 ? -10 : 0;
            double instE1 = (sumL1 == 83 || sumL1 == 108) ? sumE1 - 5.0 : sumE1 - 10.0;
            double instE2 = (sumL1 == 83 || sumL1 == 108) ? sumE2 - 5.0 : sumE2 - 10.0;
            kv["SumBygelinstkonst"] = bygelKonst.ToString(CultureInfo.InvariantCulture);
            kv["SumInstE1"] = Fmt3(instE1);
            kv["SumInstE2"] = Fmt3(instE2);

            kv["SumMaskinVal"] = "Maskin: " + maskinVal + " - Svarvning";
            kv["SumMaskinValS2"] = "Maskin: " + maskinVal + " - Borrning & Fräsning";
            kv["SumMaskinValS3"] = "Maskin: " + maskinVal;

            kv["SumRit1"] = tmpBet;
            kv["SumRit2"] = tmpBet;
            kv["SumRit3"] = tmpBet;

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

        private static double Gauge(double offset)
        {
            return Math.Round(
                ((Tmpd - Tmpd1) / 2.0) + ((TmpL - 1.0 - Amatt - offset) / (2.0 * TmpKona)),
                3);
        }

        private static bool IsMachine(string mv)
        {
            if (string.IsNullOrEmpty(mv)) return false;
            for (int i = 0; i < Machines.Length; i++)
                if (string.Equals(Machines[i], mv, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        private static string Fmt3(double v) =>
            v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
    }
}