using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFComparer
{
    class PDFSyncfusion
    {
        public static string ExtractSync(string fileName) {
            //Load an existing PDF.

            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(fileName);
            string extractedText = "";
            //Load the first page.
            foreach (PdfPageBase page in loadedDocument.Pages)
            {

                //Extract text from first page.
                extractedText += page.ExtractText();

                //Close the document
            }
            loadedDocument.Close(true);

            return extractedText;
        }
    }
}
