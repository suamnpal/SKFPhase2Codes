using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace OpenXmlPowerTools
{
    public static partial class WmlComparer
    {
        private static XElement MoveRelatedPartsToDestination(
            OpenXmlPart partOfDeletedContent,
            OpenXmlPart partInNewDocument,
            XElement contentElement)
        {
            List<XElement> elementsToUpdate = contentElement
                .Descendants()
                .Where(d => d.Attributes().Any(a => ComparisonUnitWord.RelationshipAttributeNames.Contains(a.Name)))
                .ToList();

            foreach (XElement element in elementsToUpdate)
            {
                List<XAttribute> attributesToUpdate = element
                    .Attributes()
                    .Where(a => ComparisonUnitWord.RelationshipAttributeNames.Contains(a.Name))
                    .ToList();

                foreach (XAttribute att in attributesToUpdate)
                {
                    var rId = (string) att;

                    // ✅ Get the related OpenXmlPart directly via relationship Id
                    OpenXmlPart relatedPart = partOfDeletedContent.GetPartById(rId);

                    // ✅ Get its URI directly (no PackUriHelper needed)
                    Uri targetUri = relatedPart.Uri;

                    // ✅ Continue with your existing logic
                    string[] uriSplit = targetUri.ToString().Split('/');
                    string[] last = uriSplit[uriSplit.Length - 1].Split('.');
                    string uriString;
                    if (last.Length == 2)
                    {
                        uriString = uriSplit.SkipLast(1).Select(p => p + "/").StringConcatenate() +
                                    "P" + Guid.NewGuid().ToString().Replace("-", "") + "." + last[1];
                    }
                    else
                    {
                        uriString = uriSplit.SkipLast(1).Select(p => p + "/").StringConcatenate() +
                                    "P" + Guid.NewGuid().ToString().Replace("-", "");
                    }

                    // ✅ Get source related part (already retrieved earlier via GetPartById)
                    OpenXmlPart sourcePart = relatedPart;

                    // ✅ Add the part to the target document
                    OpenXmlPart newPart = partInNewDocument.AddPart(sourcePart);

                    // ✅ Get new relationship Id
                    string newRid = partInNewDocument.GetIdOfPart(newPart);

                    // ✅ Assign back to attribute
                    att.Value = newRid;

                    if (newPart.ContentType.EndsWith("xml"))
                    {
                        XDocument newPartXDoc;

                        using (Stream stream = newPart.GetStream())
                        {
                            newPartXDoc = XDocument.Load(stream);
                        }

                        // ✅ FIX: use OpenXmlPart instead of PackagePart
                        MoveRelatedPartsToDestination(relatedPart, newPart, newPartXDoc.Root);

                        using (Stream stream = newPart.GetStream(FileMode.Create, FileAccess.Write))
                        {
                            newPartXDoc.Save(stream);
                        }
                    }
                }
            }

            return contentElement;
        }

        private static XAttribute GetXmlSpaceAttribute(string textOfTextElement)
        {
            if (char.IsWhiteSpace(textOfTextElement[0]) ||
                char.IsWhiteSpace(textOfTextElement[textOfTextElement.Length - 1]))
                return new XAttribute(XNamespace.Xml + "space", "preserve");

            return null;
        }
    }
}
