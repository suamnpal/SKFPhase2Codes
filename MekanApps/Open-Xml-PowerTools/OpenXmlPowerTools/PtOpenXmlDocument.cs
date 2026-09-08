/*
Here is modification of a WmlDocument:
    public static WmlDocument SimplifyMarkup(WmlDocument doc, SimplifyMarkupSettings settings)
    {
        using (OpenXmlMemoryStreamDocument streamDoc = new OpenXmlMemoryStreamDocument(doc))
        {
            using (WordprocessingDocument document = streamDoc.GetWordprocessingDocument())
            {
                SimplifyMarkup(document, settings);
            }
            return streamDoc.GetModifiedWmlDocument();
        }
    }

Here is read-only of a WmlDocument:

    public static string GetBackgroundColor(WmlDocument doc)
    {
        using (OpenXmlMemoryStreamDocument streamDoc = new OpenXmlMemoryStreamDocument(doc))
        using (WordprocessingDocument document = streamDoc.GetWordprocessingDocument())
        {
            XDocument mainDocument = document.MainDocumentPart.GetXDocument();
            XElement backgroundElement = mainDocument.Descendants(W.background).FirstOrDefault();
            return (backgroundElement == null) ? string.Empty : backgroundElement.Attribute(W.color).Value;
        }
    }

Here is creating a new WmlDocument:

    private OpenXmlPowerToolsDocument CreateSplitDocument(WordprocessingDocument source, List<XElement> contents, string newFileName)
    {
        using (OpenXmlMemoryStreamDocument streamDoc = OpenXmlMemoryStreamDocument.CreateWordprocessingDocument())
        {
            using (WordprocessingDocument document = streamDoc.GetWordprocessingDocument())
            {
                DocumentBuilder.FixRanges(source.MainDocumentPart.GetXDocument(), contents);
                PowerToolsExtensions.SetContent(document, contents);
            }
            OpenXmlPowerToolsDocument newDoc = streamDoc.GetModifiedDocument();
            newDoc.FileName = newFileName;
            return newDoc;
        }
    }
*/

using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Packaging;
using static OpenXmlPowerTools.FlatOpc;

namespace OpenXmlPowerTools
{
    public class PowerToolsDocumentException : Exception
    {
        public PowerToolsDocumentException(string message) : base(message) { }
    }
    public class PowerToolsInvalidDataException : Exception
    {
        public PowerToolsInvalidDataException(string message) : base(message) { }
    }

    public class OpenXmlPowerToolsDocument
    {
        public string FileName { get; set; }
        public byte[] DocumentByteArray { get; set; }

        public static OpenXmlPowerToolsDocument FromFileName(string fileName)
        {
            byte[] bytes = File.ReadAllBytes(fileName);
            Type type;

            try
            {
                type = GetDocumentType(bytes);
            }
            catch (FileFormatException)
            {
                throw new PowerToolsDocumentException("Not an Open XML document.");
            }

            if (type == typeof(WordprocessingDocument))
                return new WmlDocument(fileName, bytes);

            if (type == typeof(SpreadsheetDocument))
                return new SmlDocument(fileName, bytes);

            if (type == typeof(PresentationDocument))
                return new PmlDocument(fileName, bytes);

            // ✅ REMOVED legacy Package handling (not supported in SDK 3.x)

            throw new PowerToolsDocumentException("Unsupported or invalid Open XML document.");
        }

        public static OpenXmlPowerToolsDocument FromDocument(OpenXmlPowerToolsDocument doc)
        {
            Type type = doc.GetDocumentType();
            if (type == typeof(WordprocessingDocument))
                return new WmlDocument(doc);
            if (type == typeof(SpreadsheetDocument))
                return new SmlDocument(doc);
            if (type == typeof(PresentationDocument))
                return new PmlDocument(doc);
            return null;    // This should not be possible from a valid OpenXmlPowerToolsDocument object
        }

        public OpenXmlPowerToolsDocument(OpenXmlPowerToolsDocument original)
        {
            DocumentByteArray = new byte[original.DocumentByteArray.Length];
            Array.Copy(original.DocumentByteArray, DocumentByteArray, original.DocumentByteArray.Length);
            FileName = original.FileName;
        }

        public OpenXmlPowerToolsDocument(OpenXmlPowerToolsDocument original, bool convertToTransitional)
        {
            if (convertToTransitional)
            {
                ConvertToTransitional(original.FileName, original.DocumentByteArray);
            }
            else
            {
                DocumentByteArray = new byte[original.DocumentByteArray.Length];
                Array.Copy(original.DocumentByteArray, DocumentByteArray, original.DocumentByteArray.Length);
                FileName = original.FileName;
            }
        }

        public OpenXmlPowerToolsDocument(string fileName)
        {
            this.FileName = fileName;
            DocumentByteArray = File.ReadAllBytes(fileName);
        }

        public OpenXmlPowerToolsDocument(string fileName, bool convertToTransitional)
        {
            this.FileName = fileName;

            if (convertToTransitional)
            {
                var tempByteArray = File.ReadAllBytes(fileName);
                ConvertToTransitional(fileName, tempByteArray);
            }
            else
            {
                this.FileName = fileName;
                DocumentByteArray = File.ReadAllBytes(fileName);
            }
        }

        private void ConvertToTransitional(string fileName, byte[] tempByteArray)
        {
            Type type;
            try
            {
                type = GetDocumentType(tempByteArray);
            }
            catch (FileFormatException)
            {
                throw new PowerToolsDocumentException("Not an Open XML document.");
            }

            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(tempByteArray, 0, tempByteArray.Length);
                if (type == typeof(WordprocessingDocument))
                {
                    using (WordprocessingDocument sDoc = WordprocessingDocument.Open(ms, true))
                    {
                        // following code forces the SDK to serialize
                        foreach (var part in sDoc.Parts)
                        {
                            try
                            {
                                var z = part.OpenXmlPart.RootElement;
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                        }
                    }
                }
                else if (type == typeof(SpreadsheetDocument))
                {
                    using (SpreadsheetDocument sDoc = SpreadsheetDocument.Open(ms, true))
                    {
                        // following code forces the SDK to serialize
                        foreach (var part in sDoc.Parts)
                        {
                            try
                            {
                                var z = part.OpenXmlPart.RootElement;
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                        }
                    }
                }
                else if (type == typeof(PresentationDocument))
                {
                    using (PresentationDocument sDoc = PresentationDocument.Open(ms, true))
                    {
                        // following code forces the SDK to serialize
                        foreach (var part in sDoc.Parts)
                        {
                            try
                            {
                                var z = part.OpenXmlPart.RootElement;
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                        }
                    }
                }
                this.FileName = fileName;
                DocumentByteArray = ms.ToArray();
            }
        }

        public OpenXmlPowerToolsDocument(byte[] byteArray)
        {
            DocumentByteArray = new byte[byteArray.Length];
            Array.Copy(byteArray, DocumentByteArray, byteArray.Length);
            this.FileName = null;
        }

        public OpenXmlPowerToolsDocument(byte[] byteArray, bool convertToTransitional)
        {
            if (convertToTransitional)
            {
                ConvertToTransitional(null, byteArray);
            }
            else
            {
                DocumentByteArray = new byte[byteArray.Length];
                Array.Copy(byteArray, DocumentByteArray, byteArray.Length);
                this.FileName = null;
            }
        }

        public OpenXmlPowerToolsDocument(string fileName, MemoryStream memStream)
        {
            FileName = fileName;
            DocumentByteArray = new byte[memStream.Length];
            Array.Copy(memStream.GetBuffer(), DocumentByteArray, memStream.Length);
        }

        public OpenXmlPowerToolsDocument(string fileName, MemoryStream memStream, bool convertToTransitional)
        {
            if (convertToTransitional)
            {
                ConvertToTransitional(fileName, memStream.ToArray());
            }
            else
            {
                FileName = fileName;
                DocumentByteArray = new byte[memStream.Length];
                Array.Copy(memStream.GetBuffer(), DocumentByteArray, memStream.Length);
            }
        }

        public string GetName()
        {
            if (FileName == null)
                return "Unnamed Document";
            FileInfo file = new FileInfo(FileName);
            return file.Name;
        }

        public void SaveAs(string fileName)
        {
            File.WriteAllBytes(fileName, DocumentByteArray);
        }

        public void Save()
        {
            if (this.FileName == null)
                throw new InvalidOperationException("Attempting to Save a document that has no file name.  Use SaveAs instead.");
            File.WriteAllBytes(this.FileName, DocumentByteArray);
        }

        public void WriteByteArray(Stream stream)
        {
            stream.Write(DocumentByteArray, 0, DocumentByteArray.Length);
        }

        public Type GetDocumentType()
        {
            return GetDocumentType(DocumentByteArray);
        }

        private static Type GetDocumentType(byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            {
                // ✅ Try Word
                try
                {
                    using (var doc = WordprocessingDocument.Open(stream, false))
                        return typeof(WordprocessingDocument);
                }
                catch { }

                stream.Position = 0;

                // ✅ Try Excel
                try
                {
                    using (var doc = SpreadsheetDocument.Open(stream, false))
                        return typeof(SpreadsheetDocument);
                }
                catch { }

                stream.Position = 0;

                // ✅ Try PowerPoint
                try
                {
                    using (var doc = PresentationDocument.Open(stream, false))
                        return typeof(PresentationDocument);
                }
                catch { }

                throw new PowerToolsDocumentException("Not an Open XML document.");
            }
        }

        public static void SavePartAs(OpenXmlPart part, string filePath)
        {
            Stream partStream = part.GetStream(FileMode.Open, FileAccess.Read);
            byte[] partContent = new byte[partStream.Length];
            partStream.Read(partContent, 0, (int)partStream.Length);

            File.WriteAllBytes(filePath, partContent);
        }
    }

    public partial class WmlDocument : OpenXmlPowerToolsDocument
    {
        public WmlDocument(OpenXmlPowerToolsDocument original)
            : base(original)
        {
            if (GetDocumentType() != typeof(WordprocessingDocument))
                throw new PowerToolsDocumentException("Not a Wordprocessing document.");
        }

        public WmlDocument(OpenXmlPowerToolsDocument original, bool convertToTransitional)
            : base(original, convertToTransitional)
        {
            if (GetDocumentType() != typeof(WordprocessingDocument))
                throw new PowerToolsDocumentException("Not a Wordprocessing document.");
        }

        public WmlDocument(string fileName)
            : base(fileName)
        {
            if (GetDocumentType() != typeof(WordprocessingDocument))
                throw new PowerToolsDocumentException("Not a Wordprocessing document.");
        }

        public WmlDocument(string fileName, bool convertToTransitional)
            : base(fileName, convertToTransitional)
        {
            if (GetDocumentType() != typeof(WordprocessingDocument))
                throw new PowerToolsDocumentException("Not a Wordprocessing document.");
        }

        public WmlDocument(string fileName, byte[] byteArray)
            : base(byteArray)
        {
            FileName = fileName;
            if (GetDocumentType() != typeof(WordprocessingDocument))
                throw new PowerToolsDocumentException("Not a Wordprocessing document.");
        }

        public WmlDocument(string fileName, byte[] byteArray, bool convertToTransitional)
            : base(byteArray, convertToTransitional)
        {
            FileName = fileName;
            if (GetDocumentType() != typeof(WordprocessingDocument))
                throw new PowerToolsDocumentException("Not a Wordprocessing document.");
        }

        public WmlDocument(string fileName, MemoryStream memStream)
            : base(fileName, memStream)
        {
        }

        public WmlDocument(string fileName, MemoryStream memStream, bool convertToTransitional)
            : base(fileName, memStream, convertToTransitional)
        {
        }
    }

    public partial class SmlDocument : OpenXmlPowerToolsDocument
    {
        public SmlDocument(OpenXmlPowerToolsDocument original)
            : base(original)
        {
            if (GetDocumentType() != typeof(SpreadsheetDocument))
                throw new PowerToolsDocumentException("Not a Spreadsheet document.");
        }

        public SmlDocument(OpenXmlPowerToolsDocument original, bool convertToTransitional)
            : base(original, convertToTransitional)
        {
            if (GetDocumentType() != typeof(SpreadsheetDocument))
                throw new PowerToolsDocumentException("Not a Spreadsheet document.");
        }

        public SmlDocument(string fileName)
            : base(fileName)
        {
            if (GetDocumentType() != typeof(SpreadsheetDocument))
                throw new PowerToolsDocumentException("Not a Spreadsheet document.");
        }

        public SmlDocument(string fileName, bool convertToTransitional)
            : base(fileName, convertToTransitional)
        {
            if (GetDocumentType() != typeof(SpreadsheetDocument))
                throw new PowerToolsDocumentException("Not a Spreadsheet document.");
        }

        public SmlDocument(string fileName, byte[] byteArray)
            : base(byteArray)
        {
            FileName = fileName;
            if (GetDocumentType() != typeof(SpreadsheetDocument))
                throw new PowerToolsDocumentException("Not a Spreadsheet document.");
        }

        public SmlDocument(string fileName, byte[] byteArray, bool convertToTransitional)
            : base(byteArray, convertToTransitional)
        {
            FileName = fileName;
            if (GetDocumentType() != typeof(SpreadsheetDocument))
                throw new PowerToolsDocumentException("Not a Spreadsheet document.");
        }

        public SmlDocument(string fileName, MemoryStream memStream)
            : base(fileName, memStream)
        {
        }

        public SmlDocument(string fileName, MemoryStream memStream, bool convertToTransitional)
            : base(fileName, memStream, convertToTransitional)
        {
        }
    }

    public partial class PmlDocument : OpenXmlPowerToolsDocument
    {
        public PmlDocument(OpenXmlPowerToolsDocument original)
            : base(original)
        {
            if (GetDocumentType() != typeof(PresentationDocument))
                throw new PowerToolsDocumentException("Not a Presentation document.");
        }

        public PmlDocument(OpenXmlPowerToolsDocument original, bool convertToTransitional)
            : base(original, convertToTransitional)
        {
            if (GetDocumentType() != typeof(PresentationDocument))
                throw new PowerToolsDocumentException("Not a Presentation document.");
        }

        public PmlDocument(string fileName)
            : base(fileName)
        {
            if (GetDocumentType() != typeof(PresentationDocument))
                throw new PowerToolsDocumentException("Not a Presentation document.");
        }

        public PmlDocument(string fileName, bool convertToTransitional)
            : base(fileName, convertToTransitional)
        {
            if (GetDocumentType() != typeof(PresentationDocument))
                throw new PowerToolsDocumentException("Not a Presentation document.");
        }

        public PmlDocument(string fileName, byte[] byteArray)
            : base(byteArray)
        {
            FileName = fileName;
            if (GetDocumentType() != typeof(PresentationDocument))
                throw new PowerToolsDocumentException("Not a Presentation document.");
        }

        public PmlDocument(string fileName, byte[] byteArray, bool convertToTransitional)
            : base(byteArray, convertToTransitional)
        {
            FileName = fileName;
            if (GetDocumentType() != typeof(PresentationDocument))
                throw new PowerToolsDocumentException("Not a Presentation document.");
        }

        public PmlDocument(string fileName, MemoryStream memStream)
            : base(fileName, memStream)
        {
        }

        public PmlDocument(string fileName, MemoryStream memStream, bool convertToTransitional)
            : base(fileName, memStream, convertToTransitional)
        {
        }
    }

    public class OpenXmlMemoryStreamDocument : IDisposable
    {
        private OpenXmlPowerToolsDocument _document;
        private MemoryStream _memoryStream;

        public OpenXmlMemoryStreamDocument(OpenXmlPowerToolsDocument doc)
        {
            _document = doc;
            _memoryStream = new MemoryStream();

            _memoryStream.Write(doc.DocumentByteArray, 0, doc.DocumentByteArray.Length);
            _memoryStream.Position = 0;
        }

        internal OpenXmlMemoryStreamDocument(MemoryStream stream)
        {
            _memoryStream = stream;
            _memoryStream.Position = 0;
        }

        // ✅ Create Word document
        public static OpenXmlMemoryStreamDocument CreateWordprocessingDocument()
        {
            var stream = new MemoryStream();

            using (var doc = WordprocessingDocument.Create(stream, DocumentFormat.OpenXml.WordprocessingDocumentType.Document, true))
            {
                doc.AddMainDocumentPart();
                doc.MainDocumentPart.PutXDocument(new XDocument(
                    new XElement(W.document,
                        new XAttribute(XNamespace.Xmlns + "w", W.w),
                        new XAttribute(XNamespace.Xmlns + "r", R.r),
                        new XElement(W.body))));
            }

            stream.Position = 0;
            return new OpenXmlMemoryStreamDocument(stream);
        }

        // ✅ Spreadsheet
        public static OpenXmlMemoryStreamDocument CreateSpreadsheetDocument()
        {
            var stream = new MemoryStream();

            using (var doc = SpreadsheetDocument.Create(stream, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook, true))
            {
                doc.AddWorkbookPart();

                XNamespace ns = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
                XNamespace rel = "http://schemas.openxmlformats.org/officeDocument/2006/relationships";

                doc.WorkbookPart.PutXDocument(new XDocument(
                    new XElement(ns + "workbook",
                        new XAttribute("xmlns", ns),
                        new XAttribute(XNamespace.Xmlns + "r", rel),
                        new XElement(ns + "sheets"))));
            }

            stream.Position = 0;
            return new OpenXmlMemoryStreamDocument(stream);
        }

        public static OpenXmlMemoryStreamDocument CreatePresentationDocument()
        {
            var stream = new MemoryStream();

            using (var doc = PresentationDocument.Create(stream, DocumentFormat.OpenXml.PresentationDocumentType.Presentation, true))
            {
                doc.AddPresentationPart();

                XNamespace ns = "http://schemas.openxmlformats.org/presentationml/2006/main";
                XNamespace rel = "http://schemas.openxmlformats.org/officeDocument/2006/relationships";
                XNamespace drawing = "http://schemas.openxmlformats.org/drawingml/2006/main";

                doc.PresentationPart.PutXDocument(new XDocument(
                    new XElement(ns + "presentation",
                        new XAttribute(XNamespace.Xmlns + "a", drawing),
                        new XAttribute(XNamespace.Xmlns + "r", rel),
                        new XAttribute(XNamespace.Xmlns + "p", ns),
                        new XElement(ns + "sldMasterIdLst"),
                        new XElement(ns + "sldIdLst"),
                        new XElement(ns + "notesSz",
                            new XAttribute("cx", "6858000"),
                            new XAttribute("cy", "9144000")))));
            }

            stream.Position = 0;
            return new OpenXmlMemoryStreamDocument(stream);
        }

        // ❌ Removed: CreatePackage (no longer valid)

        // ✅ Open documents directly from stream
        public WordprocessingDocument GetWordprocessingDocument(bool isEditable = true)
        {
            _memoryStream.Position = 0;
            return WordprocessingDocument.Open(_memoryStream, isEditable);
        }

        public SpreadsheetDocument GetSpreadsheetDocument(bool isEditable = true)
        {
            _memoryStream.Position = 0;
            return SpreadsheetDocument.Open(_memoryStream, isEditable);
        }

        public PresentationDocument GetPresentationDocument(bool isEditable = true)
        {
            _memoryStream.Position = 0;
            return PresentationDocument.Open(_memoryStream, isEditable);
        }

        // ✅ Modern type detection
        public Type GetDocumentType()
        {
            _memoryStream.Position = 0;

            try
            {
                using (var w = WordprocessingDocument.Open(_memoryStream, false)) return typeof(WordprocessingDocument);
            }
            catch { }

            try
            {
                using (var s = SpreadsheetDocument.Open(_memoryStream, false)) return typeof(SpreadsheetDocument);
            }
            catch { }

            try
            {
                using (var p = PresentationDocument.Open(_memoryStream, false)) return typeof(PresentationDocument);
            }
            catch { }

            throw new PowerToolsDocumentException("Not an Open XML Document.");
        }

        // ✅ Save back
        public OpenXmlPowerToolsDocument GetModifiedDocument()
        {
            return new OpenXmlPowerToolsDocument(
                _document?.FileName,
                new MemoryStream(_memoryStream.ToArray())
            );
        }

        public WmlDocument GetModifiedWmlDocument()
        {
            return new WmlDocument(
                _document?.FileName,
                new MemoryStream(_memoryStream.ToArray())
            );
        }

        public SmlDocument GetModifiedSmlDocument()
        {
            return new SmlDocument(
                _document?.FileName,
                new MemoryStream(_memoryStream.ToArray())
            );
        }

        public PmlDocument GetModifiedPmlDocument()
        {
            return new PmlDocument(
                _document?.FileName,
                new MemoryStream(_memoryStream.ToArray())
            );
        }

        public void Dispose()
        {
            _memoryStream?.Dispose();
        }
    }
}
