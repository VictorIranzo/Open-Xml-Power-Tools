using org.pdfclown.documents;
using org.pdfclown.documents.contents;
using org.pdfclown.documents.contents.fonts;
using org.pdfclown.documents.contents.objects;
using org.pdfclown.documents.interaction.annotations;
using org.pdfclown.files;
using org.pdfclown.tools;
using org.pdfclown.util.math;
using org.pdfclown.util.math.geom;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PDFComparer
{
    class PDFClownComparer
    {
            public static void Compare()
            {
                // 1. Opening the PDF file...
                string filePath = @"D:\INFORMATICA\ADD\Open-Xml-PowerTools\FilesPDF\mes.pdf";
            TextExtractor extractor = new TextExtractor();
                using (File file = new File(filePath))
                {
                    // 2. Iterating through the document pages...
                    TextExtractor textExtractor = new TextExtractor(true, true);
                    foreach (Page page in file.Document.Pages)
                    {
                    try
                    {
                        var a = extractor.Extract(page);
                    }
                    catch (Exception e) { }
                }
            }
        } 
   }
    }