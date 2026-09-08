using System;
using System.Collections.Generic;
using QeDynamicDocumentProcessing.Common;

namespace QeDynamicDocumentProcessing.TemplateFiles
{
    public class HYLSOR_EXTRA_BORRHAL : ITemplateCalculations
    {
        public Dictionary<string, string> CalculateWordParameters(APIRequest request)
        {
            var result = new Dictionary<string, string>();

            Merge(result, Header(request));
            Merge(result, Borrhal());
            Merge(result, Dimensions());
            Merge(result, Angles());
            Merge(result, Texts());
            Merge(result, Tables());
            Merge(result, EmptyGroups());

            return result;
        }

        private Dictionary<string, string> Header(APIRequest request)
        {
            return new Dictionary<string, string>
            {
                { "SumMaskinVal", "Maskin: "+ request?.MachineNumber },
                { "SumRitningsnr", "4716520" }
            };
        }

        private Dictionary<string, string> Borrhal()
        {
            return new Dictionary<string, string>
            {
                { "SumAntBH","6" },
                { "SumAntBHa","6st borrhål" },
                { "SumK1BH","114,9" },
                { "SumKDBH","222" },
                { "LblK1BH","(K1BH)" },
                { "LblKDBH","(KDBH)" }
            };
        }

        private Dictionary<string, string> Dimensions()
        {
            return new Dictionary<string, string>
            {
                { "SumA","(A) 27" },
                { "SumS","(S) 21" },
                { "SumC","(C) 12,3x45°" },
                { "SumAD","(AD) 444 ±0.4" }
            };
        }

        private Dictionary<string, string> Angles()
        {
            return new Dictionary<string, string>
            {
                { "SumV6","60°x6" },
                { "SumV3","30°" }
            };
        }

        private Dictionary<string, string> Texts()
        {
            return new Dictionary<string, string>
            {
                { "SumText","Grader, frifläckar, slagmärken, repor, valkar och andra defekter samt ojämnheter kontrolleras med ögonmått, för övriga mått används skjutmått." }
            };
        }

        private Dictionary<string, string> Tables()
        {
            return new Dictionary<string, string>
            {
                { "SumF_A","1/1" },
                { "SumD_A","Skjutmått" },
                { "SumAF_A","" },

                { "SumF_S","1/1" },
                { "SumD_S","Gängtolk" },
                { "SumAF_S","" },

                { "SumF_C","1/1" },
                { "SumD_C","Skjutmått, Gradverktyg" },
                { "SumAF_C","" },

                { "SumF_AD","1/1" },
                { "SumD_AD","Skjutmått" },
                { "SumAF_AD","" }
            };
        }

        private Dictionary<string, string> EmptyGroups()
        {
            return new Dictionary<string, string>
            {
                { "SumG","" },
                { "SumADTol","" },
                { "SumKlEgenskaper","" }
            };
        }

        private void Merge(Dictionary<string, string> target, Dictionary<string, string> source)
        {
            foreach (var kv in source)
                if (!target.ContainsKey(kv.Key))
                    target.Add(kv.Key, kv.Value ?? "");
        }

        private string GetValue(APIRequest request, string key)
        {
            if (request == null || string.IsNullOrEmpty(key)) return "";
            var prop = request.GetType().GetProperty(key);
            if (prop == null) return "";
            var value = prop.GetValue(request);
            return value?.ToString() ?? "";
        }
    }
}