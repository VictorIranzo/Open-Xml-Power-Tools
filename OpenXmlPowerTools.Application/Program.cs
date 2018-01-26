
namespace OpenXmlPowerTools.Application
{
    using DocumentFormat.OpenXml.Packaging;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using System.Xml.Linq;

    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            // Read the template to be generated.
            WmlDocument template = new WmlDocument("../../Template.docx");

            WmlDocument resolvedDocument = ResolveDocument(template);
            resolvedDocument.SaveAs("../../MacroTemplate.docx");

            // Fills the template with the XML data.
            bool error;
            XElement xmldata = XElement.Load("../../Data.xml");
            WmlDocument assembledDoc = DocumentAssembler.AssembleDocument(resolvedDocument, xmldata, out error);

            assembledDoc.SaveAs("../../Assembled.docx");
        }

        private static WmlDocument ResolveDocument(WmlDocument document)
        {
            List<Source> sources = new List<Source>();

            // Write the document to a memory stream to transform it to a WordProcessingDocument.
            MemoryStream memoryStreamTemplate = new MemoryStream();

            memoryStreamTemplate.Write(document.DocumentByteArray, 0, document.DocumentByteArray.Length);

            WordprocessingDocument wordProcessingDoc = WordprocessingDocument.Open(memoryStreamTemplate, true);

            // Add the document passed as a parameter discarding its header and footer.
            sources.Add(new Source(document) { KeepSections = true, DiscardHeadersAndFootersInKeptSections = true});

            // Find header and footer
            List<HeaderPart> headers = wordProcessingDoc.MainDocumentPart.HeaderParts.ToList();

            // Go through all headers looking for links. Normally, it's not necessary as it's in the first header.
            foreach (HeaderPart header in headers)
            {
                XElement linkInHeader = header.GetXDocument().Descendants(W.p).FirstOrDefault(wt => wt.Value.Contains("<# link"));
                if (linkInHeader != null) {
                    sources.Add(new Source(GetReferencedDocument(linkInHeader)) { KeepSections = true });
                    
                    break;
                }
            }

            // TODO: Do the same for all footers.

            // Find all subreports links in the body.
            XDocument body = wordProcessingDoc.MainDocumentPart.GetXDocument();
            List<XElement> linksInBody = body.Descendants(W.p).Where(wp=> wp.Value.Contains("<# link")).ToList();
            int i = 0;

            foreach (XElement link in linksInBody)
            {
                // Gets the referenced document and resolve it recursively.
                WmlDocument referencedDocResolved = ResolveDocument(GetReferencedDocument(link));
                string nodeName = "node_" + i++;

                // Replaces the link by a node with a identifier and adds the subreport as a source. 
                // The DocumentBuilder will recognize this identifier and will insert in its position 
                // at the template the subreport.
                link.ReplaceWith(new XElement(PtOpenXml.Insert,
                        new XAttribute("Id", nodeName)));
                sources.Add(new Source(referencedDocResolved) { KeepSections = false, InsertId = nodeName });
            }

            if (sources.Count < 2) return document;
             
            wordProcessingDoc.MainDocumentPart.PutXDocument();
            document.DocumentByteArray = memoryStreamTemplate.ToArray();

            // Return the document with all its subreports incorpored.
            return DocumentBuilder.BuildDocument(sources);
        }

        private static WmlDocument GetReferencedDocument(XElement link)
        {
            int startReference = link.Value.IndexOf('"') + 1;
            int endReference = link.Value.LastIndexOf('"');
            string documentPath = "../../" + link.Value.Substring(startIndex: startReference, length: endReference - startReference);

            return new WmlDocument(documentPath);
        }
    }
}
