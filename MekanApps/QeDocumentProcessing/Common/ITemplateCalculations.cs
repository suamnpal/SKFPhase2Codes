namespace QeDynamicDocumentProcessing.Common
{
    internal interface ITemplateCalculations
    {
        Dictionary<string, string> CalculateWordParameters(APIRequest req);
    }
}
