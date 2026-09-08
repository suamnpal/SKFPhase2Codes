using System.Globalization;

namespace QeDynamicDocumentProcessing.Common
{
    internal static class CommonFunctions
    {
        static char[] delimiters = { ' ', '/', '.', '-', '(', ')' };
        internal static CultureInfo Culture = new CultureInfo("sv-SE");

        private static string? GetKeyValue(List<Bookmark> bookmarks, string key)
        {
            if (bookmarks == null || bookmarks.Count == 0 || string.IsNullOrWhiteSpace(key))
                return null;

            // Case-insensitive search using LINQ
            return bookmarks
                .FirstOrDefault(b => b.BookmarkName.Equals(key, StringComparison.OrdinalIgnoreCase))
                ?.BookmarkValue;
        }

        internal static double GetKeyValueDouble(List<Bookmark> bookmarks, string key)
        {
            double returnValue = 0;
            string? valueStr = GetKeyValue(bookmarks, key);
            double.TryParse(valueStr, Culture, out returnValue);
            return returnValue;
        }

        internal static string GetKeyValueString(List<Bookmark> bookmarks, string key)
        {
            string? valueStr = GetKeyValue(bookmarks, key);
            return valueStr != null ? valueStr : string.Empty;
        }

        internal static TmpBets CalculateBets(string TmpBet)
        {
            TmpBets result = new TmpBets();
            result.TmpCounts = TmpBet.Length;

            //SKAPA LISTA & DELA AV
            string[] TmpBetLista = TmpBet.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            string TmpBet1a = TmpBetLista.Length >= 1 ? TmpBetLista[0] : String.Empty;
            string TmpBet2a = TmpBetLista.Length >= 2 ? TmpBetLista[1] : String.Empty;
            string TmpBet3a = TmpBetLista.Length >= 3 ? TmpBetLista[2] : String.Empty;
            string TmpBet4a = TmpBetLista.Length >= 4 ? TmpBetLista[3] : String.Empty;
            string TmpBet5a = TmpBetLista.Length >= 5 ? TmpBetLista[4] : String.Empty;
            string TmpBet6a = TmpBetLista.Length >= 6 ? TmpBetLista[5] : String.Empty;

            result.TmpNull1 = string.IsNullOrEmpty(TmpBet1a);
            result.TmpNull2 = string.IsNullOrEmpty(TmpBet2a);
            result.TmpNull3 = string.IsNullOrEmpty(TmpBet3a);
            result.TmpNull4 = string.IsNullOrEmpty(TmpBet4a);
            result.TmpNull5 = string.IsNullOrEmpty(TmpBet5a);
            result.TmpNull6 = string.IsNullOrEmpty(TmpBet6a);

            int TmpBet1b = 0;
            int TmpBet2b = 0;
            int TmpBet3b = 0;
            int TmpBet4b = 0;
            int TmpBet5b = 0;
            int TmpBet6b = 0;

            bool TmpBet1c = int.TryParse(TmpBet1a, Culture, out TmpBet1b);
            bool TmpBet2c = int.TryParse(TmpBet2a, Culture, out TmpBet2b);
            bool TmpBet3c = int.TryParse(TmpBet3a, Culture, out TmpBet3b);
            bool TmpBet4c = int.TryParse(TmpBet4a, Culture, out TmpBet4b);
            bool TmpBet5c = int.TryParse(TmpBet5a, Culture, out TmpBet5b);
            bool TmpBet6c = int.TryParse(TmpBet6a, Culture, out TmpBet6b);

            result.TmpBet1 = (TmpBet1c && TmpBet1b > 0) ? TmpBet1b.ToString() : TmpBet1a;
            result.TmpBet2 = (TmpBet2c && TmpBet2b > 0) ? TmpBet2b.ToString() : TmpBet2a;
            result.TmpBet3 = (TmpBet3c && TmpBet3b > 0) ? TmpBet3b.ToString() : TmpBet3a;
            result.TmpBet4 = (TmpBet4c && TmpBet4b > 0) ? TmpBet4b.ToString() : TmpBet4a;
            result.TmpBet5 = (TmpBet5c && TmpBet5b > 0) ? TmpBet5b.ToString() : TmpBet5a;
            result.TmpBet6 = (TmpBet6c && TmpBet6b > 0) ? TmpBet6b.ToString() : TmpBet6a;

            result.TmpCountB1 = result.TmpBet1.Length;
            result.TmpCountB2 = result.TmpBet2.Length;
            result.TmpCountB3 = result.TmpBet3.Length;
            result.TmpCountB4 = result.TmpBet4.Length;
            result.TmpCountB5 = result.TmpBet5.Length;
            result.TmpCountB6 = result.TmpBet6.Length;
            return result;
        }

        internal static Dictionary<string, string> GenerateDocumentHeaders(APIRequest req)
        {
            Dictionary<string, string> keyValues = new Dictionary<string, string>();
            keyValues.Add("DocumentCategory", req.CategoryHierarchy);
            keyValues.Add("Subject", req.ProductDesignation);
            keyValues.Add("Version", req.Version);
            keyValues.Add("Published", req.Published);
            keyValues.Add("Created", req.CreatedBy);
            keyValues.Add("Approved", req.ApprovedBy);
            return keyValues;
        }

        internal static Dictionary<string, string> GenerateDocumentFooters(APIRequest req)
        {
            Dictionary<string, string> keyValues = new Dictionary<string, string>();
            keyValues.Add("ValidStatus", $"Tver: {req.TemplateVersion}");
            return keyValues;
        }
    }
}
