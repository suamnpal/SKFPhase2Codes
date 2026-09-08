using DocumentFormat.OpenXml.Bibliography;
using Microsoft.VisualBasic;
using QeDynamicDocumentProcessing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_Avdragshylsor_ASW_0001 : ITemplateCalculations
    {
        private static readonly string[] Machines = new[] { "VTR-160", "MacTurn 550" };

        public Dictionary<string, string> CalculateWordParameters(APIRequest req)
        {
            var kv = new Dictionary<string, string>();

            string subject = req != null ? (req.ProductDesignation ?? string.Empty) : string.Empty;
            string maskinVal = req != null ? (req.MachineNumber ?? string.Empty) : string.Empty;

            kv["VaLPopUp"] = ComputePopup(req != null ? req.Published : null);

            string tmpFormat = (subject ?? string.Empty).ToUpperInvariant().Trim();
            string tmpBet = tmpFormat.Replace(".", ",");
            kv["TmpFormat"] = tmpFormat;
            kv["TmpBet"] = tmpBet;

            kv["TmpSerie"] = "ASW";
            kv["TmpTyp"] = "ASW-0001";

            const double SumML = 250.0;
            const double SumMLb = 100.0;
            const double SumMLa = 100.0;

            const double TmpL = 302.0;
            const double Tmpd = 530.0;
            const double Tmpd3 = 500.0;
            const double Amaatt = 24.0;
            const double TmpKona = 30.0;

            kv["SumML"] = Fmt0(SumML);
            kv["SumMLb"] = Fmt0(SumMLb);
            kv["SumMLa"] = Fmt0(SumMLa);

            double tmp8 = RoundTo(((Tmpd - Tmpd3) / 2.0) + ((Amaatt + 8.0) / (2.0 * TmpKona)), 0.001);
            double tmp108 = RoundTo(((Tmpd - Tmpd3) / 2.0) + ((Amaatt + 108.0) / (2.0 * TmpKona)), 0.001);
            double tmp40 = RoundTo(((Tmpd - Tmpd3) / 2.0) + ((Amaatt + 40.0) / (2.0 * TmpKona)), 0.001);
            double tmp140 = RoundTo(((Tmpd - Tmpd3) / 2.0) + ((Amaatt + 140.0) / (2.0 * TmpKona)), 0.001);

            int sumL1 = SumML < 145.0 ? 8 : 40;
            int sumL2 = SumML < 145.0 ? 108 : 140;
            double sumE1 = SumML < 145.0 ? tmp8 : tmp40;
            double sumE2 = SumML < 145.0 ? tmp108 : tmp140;

            kv["SumL1"] = sumL1.ToString(CultureInfo.InvariantCulture);
            kv["SumL2"] = sumL2.ToString(CultureInfo.InvariantCulture);
            kv["SumE1"] = "16,067";
            kv["SumE2"] = "17,733 ";

            double tmpKonUtrakning = ((TmpL + Amaatt) / TmpKona) + Tmpd;
            double tmpKonstantOS = RoundTo(Math.Sin(10.0 * Math.PI / 180.0) * 2.0, 0.0001);

            double tmpKordaOSinv = RoundTo((Tmpd3 / 2.0) * tmpKonstantOS, 0.1);
            double tmpKordaOSutv = RoundTo((tmpKonUtrakning / 2.0) * tmpKonstantOS, 0.1);

            kv["SumKOSinv"] = Fmt1(tmpKordaOSinv);
            kv["SumKOSutv"] = Fmt1(tmpKordaOSutv);

            kv["TmpBygGtj"] = SumML < 145.0 ? "SR 7415983" : "SR 7419470 el. 7415991";
            kv["SumBygGtj"] = kv["TmpBygGtj"];

            kv["SumMaskinVal"] = "Maskin: " + maskinVal + " - Svarvning";
            kv["SumMaskinValS2"] = "Maskin: " + maskinVal + " - Borrning & fräsning";

            kv["VaLFärdig"] = "";
            kv["VaLInfo"] = "";

            return kv;
        }

        private static string ComputePopup(string published)
        {
            if (string.IsNullOrWhiteSpace(published)) return "";
            if (!DateTime.TryParse(published, out var pubDt)) return "";
            int dagar = 14;
            var validTill = pubDt.AddDays(dagar);
            if (DateTime.Today <= validTill.Date)
            {
                return "Denna kontrollinstruktion har nyligen blivit uppdaterad (inom " +
                       dagar.ToString(CultureInfo.InvariantCulture) +
                       " dagar) Information om senaste ändring finns under fliken Display &Latest Change eller i Notes Qe i aktivt dokument Popupruta aktiv till " +
                       validTill.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            return "";
        }

        private static double RoundTo(double value, double step)
        {
            if (step <= 0.0) return value;
            return Math.Round(value / step, MidpointRounding.AwayFromZero) * step;
        }

        private static string Fmt0(double v)
        {
            if (Math.Abs(v - Math.Round(v)) < 0.0000001)
                return ((long)Math.Round(v)).ToString(CultureInfo.InvariantCulture);
            return v.ToString(CultureInfo.InvariantCulture).Replace(",", ".");
        }

        private static string Fmt1(double v)
        {
            return v.ToString("F1", CommonFunctions.Culture).Replace(",", ".");
        }

        private static string Fmt3(double v)
        {
            return v.ToString("F3", CommonFunctions.Culture).Replace(",", ".");
        }
    }
}
