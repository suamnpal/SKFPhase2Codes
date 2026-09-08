using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Packaging;
using static OpenXmlPowerTools.FlatOpc;

namespace OpenXmlPowerTools
{
    public class PtMainDocumentPart : XElement
    {
        private WmlDocument ParentWmlDocument;

        public PtWordprocessingCommentsPart WordprocessingCommentsPart
        {
            get
            {
                using (MemoryStream ms = new MemoryStream(ParentWmlDocument.DocumentByteArray))
                using (WordprocessingDocument wDoc = WordprocessingDocument.Open(ms, false))
                {
                    WordprocessingCommentsPart commentsPart = wDoc.MainDocumentPart.WordprocessingCommentsPart;
                    if (commentsPart == null)
                        return null;
                    XElement partElement = commentsPart.GetXDocument().Root;
                    var childNodes = partElement.Nodes().ToList();
                    foreach (var item in childNodes)
                        item.Remove();
                    return new PtWordprocessingCommentsPart(this.ParentWmlDocument, commentsPart.Uri, partElement.Name, partElement.Attributes(), childNodes);
                }
            }
        }

        public PtMainDocumentPart(WmlDocument wmlDocument, Uri uri, XName name, params object[] values)
            : base(name, values)
        {
            ParentWmlDocument = wmlDocument;
            this.Add(
                new XAttribute(PtOpenXml.Uri, uri),
                new XAttribute(XNamespace.Xmlns + "pt", PtOpenXml.pt)
            );
        }
    }

    public class PtWordprocessingCommentsPart : XElement
    {
        private WmlDocument ParentWmlDocument;

        public PtWordprocessingCommentsPart(WmlDocument wmlDocument, Uri uri, XName name, params object[] values)
            : base(name, values)
        {
            ParentWmlDocument = wmlDocument;
            this.Add(
                new XAttribute(PtOpenXml.Uri, uri),
                new XAttribute(XNamespace.Xmlns + "pt", PtOpenXml.pt)
            );
        }
    }

    public partial class WmlDocument
    {
        public PtMainDocumentPart MainDocumentPart
        {
            get
            {
                using (MemoryStream ms = new MemoryStream(this.DocumentByteArray))
                using (WordprocessingDocument wDoc = WordprocessingDocument.Open(ms, false))
                {
                    XElement partElement = wDoc.MainDocumentPart.GetXDocument().Root;
                    var childNodes = partElement.Nodes().ToList();
                    foreach (var item in childNodes)
                        item.Remove();
                    return new PtMainDocumentPart(this, wDoc.MainDocumentPart.Uri, partElement.Name, partElement.Attributes(), childNodes);
                }
            }
        }

        public WmlDocument(WmlDocument other, params XElement[] replacementParts)
    : base(other)
        {
            using (var ms = new MemoryStream(this.DocumentByteArray))
            {
                using (var doc = WordprocessingDocument.Open(ms, true))
                {
                    foreach (var replacementPart in replacementParts)
                    {
                        XAttribute uriAttribute = replacementPart.Attribute(PtOpenXml.Uri);
                        if (uriAttribute == null)
                            throw new OpenXmlPowerToolsException(
                                "Replacement part does not contain a Uri as an attribute");

                        string uri = uriAttribute.Value;

                        // ✅ Find matching OpenXmlPart by URI
                        var allParts = doc
                            .Parts
                            .Select(p => p.OpenXmlPart)
                            .Concat(new[] { doc.MainDocumentPart })
                            .Where(p => p != null)
                            .Distinct();

                        var part = allParts.FirstOrDefault(p => p.Uri.ToString() == uri);

                        if (part == null)
                            throw new OpenXmlPowerToolsException($"Part not found: {uri}");

                        // ✅ Write XML content into the part
                        using (var stream = part.GetStream(FileMode.Create, FileAccess.Write))
                        using (var writer = XmlWriter.Create(stream))
                        {
                            replacementPart.Save(writer);
                        }
                    }
                }

                // ✅ Update the document byte array
                this.DocumentByteArray = ms.ToArray();
            }
        }
    }
}
