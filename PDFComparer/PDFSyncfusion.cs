using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UITesting;
using static PDFComparer.Diff;

/*
 * https://stackoverflow.com/questions/35151067/algorithm-to-compare-two-images-in-c-sharp
Microsoft.VisualStudio.TestTools.UITest.Common
Microsoft.VisualStudio.TestTools.UITest.Extension
Microsoft.VisualStudio.TestTools.UITest.ExtensionUtilities
Microsoft.VisualStudio.TestTools.UITest.WindowsStoreUtility
Microsoft.VisualStudio.TestTools.UITest.Framework
Microsoft.VisualStudio.TestTools.UITest.Playback 
 */

namespace PDFComparer
{
    class PDFSyncfusion
    {
        public static bool ComparePDFs(string path1, string path2)
        {
            PdfLoadedDocument loadedDocument1 = new PdfLoadedDocument(path1);
            PdfLoadedDocument loadedDocument2 = new PdfLoadedDocument(path2);

            if (loadedDocument1.Pages.Count != loadedDocument2.Pages.Count) return false;

            PdfLoadedPageEnumerator enumeratorDoc1 = (PdfLoadedPageEnumerator) loadedDocument1.Pages.GetEnumerator();
            PdfLoadedPageEnumerator enumeratorDoc2 = (PdfLoadedPageEnumerator) loadedDocument2.Pages.GetEnumerator();

            enumeratorDoc1.MoveNext();
            enumeratorDoc2.MoveNext();

            for (int i = 0; i < loadedDocument1.Pages.Count; i++) 
            {
                PdfPageBase pageDoc1 = (PdfPageBase) enumeratorDoc1.Current;
                PdfPageBase pageDoc2 = (PdfPageBase) enumeratorDoc2.Current;

                Image[] images1 = pageDoc1.ExtractImages();
                Image[] images2 = pageDoc2.ExtractImages();

                if (images1.Count() > 0 && images1[0].Size.Equals(images2[0].Size))
                {
                    // Una imagen en distinto tamaño la reconoce como igual.
                    bool b = ImageComparer.Compare(images1[0], images2[0]);
                }
                string textDocPage1 = pageDoc1.ExtractText();
                string textDocPage2 = pageDoc2.ExtractText();

                Item[] comparison = Diff.DiffText(textDocPage1,textDocPage2, true, true, false);

                if (!textDocPage1.Equals(textDocPage2))
                {
                    // Obtain differences.
                    List<string> wordsPageDoc1 = textDocPage1.Split(' ').ToList();
                    List<string> wordsPageDoc2 = textDocPage2.Split(' ').ToList();

                    int maxNumberOfWords = wordsPageDoc1.Count > wordsPageDoc2.Count ? wordsPageDoc1.Count : wordsPageDoc2.Count;

                    for (int j = 0; j < maxNumberOfWords; j++)
                    {
                       string word1 = wordsPageDoc1.Skip(j).FirstOrDefault();
                       string word2 = wordsPageDoc2.Skip(j).FirstOrDefault();

                        if (word1 == null || word2 == null || !word1.Equals(word2))
                        {
                            Console.WriteLine(word1 + " not equal to " + word2);
                        }
                    }

                }

                enumeratorDoc1.MoveNext();
                enumeratorDoc2.MoveNext();
            }

            return true;
        }

        public static string ExtractSync(string fileName) {
            //Load an existing PDF.

            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(fileName);
            string extractedText = "";
            //Load the first page.
            foreach (PdfPageBase page in loadedDocument.Pages)
            {
                //Extract text from first page.
                extractedText += page.ExtractText();
                if (extractedText.Contains("Product"))
                {
                    int indexStart = extractedText.IndexOf("Product");
                    int indexEnd = extractedText.IndexOf("\n",indexStart);
                    extractedText = extractedText.Remove(indexStart, indexEnd-indexStart);
                }
                //Close the document
            }
            loadedDocument.Close(true);

            return extractedText;
        }
    }
}
