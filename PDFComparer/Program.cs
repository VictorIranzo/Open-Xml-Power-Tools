﻿using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Linq;
using System.IO;
using System;
using System.Collections.Generic;

namespace PDFComparer
{
    class Program
    {
        static void Main(string[] args)
        {
            string path1 = "./Files/Report_v1.pdf";
            string path2 = "./Files/Report_v2.pdf";
            string path3 = "./Files/Report_v3.pdf";
            string path4 = "./Files/Report_v4.pdf";
            string path5 = "./Files/Report_v5.pdf";

            //CompareTwoPDF(path1,path4);
            //PDFClownComparer.Compare();
            PDFSyncfusion.ComparePDFs(path1, path5);

            //CompareByteArraysPDF(path1, path4);
        }

        private static void CompareByteArraysPDF(string path1, string path2)
        {
            byte[] pdf1= File.ReadAllBytes(path1);
            byte[] pdf2 = File.ReadAllBytes(path2);
            if (pdf1.Length == pdf2.Length) {
                for (int i = 0; i < pdf1.Length; i++) {
                    bool c = pdf1[i].Equals(pdf2[i]);
                    if (!c) {
                        Console.WriteLine(i);
                    }
                }
            }
            bool b = pdf1.Equals(pdf2);
        }

        static string FirstFile, SecondFile;

        public static void CompareTwoPDF(string FirstPDF, string SecondPDF)
        {
            if (File.Exists(FirstPDF) && File.Exists(SecondPDF))
            {
                PdfReader reader = new PdfReader(FirstPDF);
                for (int page = 1; page <= reader.NumberOfPages; page++)
                {
                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    FirstFile += PdfTextExtractor.GetTextFromPage(reader, page, strategy);
                }
                PdfReader reader1 = new PdfReader(SecondPDF);
                for (int page = 1; page <= reader.NumberOfPages; page++)
                {
                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    try
                    {
                        SecondFile += PdfTextExtractor.GetTextFromPage(reader1, page, strategy);
                    }
                    catch (Exception e) { }
                }
            }
            else
            {
                Console.WriteLine("Files does not exist.");
            }

            List<string> File1diff;
            List<string> File2diff;
            IEnumerable<string> file1 = FirstFile.Trim().Split('\r', '\n');
            IEnumerable<string> file2 = SecondFile.Trim().Split('\r', '\n');
            File1diff = file1.ToList();
            File2diff = file2.ToList();

            if (file2.Count() > file1.Count())
            {
                Console.WriteLine("File 1 has less number of lines than File 2.");
                for (int i = 0; i < File1diff.Count; i++)
                {
                    if (!File1diff[i].Equals(File2diff[i]))
                    {
                        Console.WriteLine("File 1 content: " +File1diff[i] + "\r\n" + "File 2 content: " +File2diff[i]);
                    }

                }

                for (int i = File1diff.Count; i < File2diff.Count; i++)
                {
                    Console.WriteLine("File 2 extra content: " +File2diff[i]);
                }

            }
            else if (file2.Count() < file1.Count())
            {
                Console.WriteLine("File 2 has less number of lines than File 1.");

                for (int i = 0; i < File2diff.Count; i++)
                {
                    if (!File1diff[i].Equals(File2diff[i]))
                    {
                        Console.WriteLine("File 1 content: " +File1diff[i] + "\r\n" + "File 2 content: " +File2diff[i]);
                    }

                }

                for (int i = File2diff.Count; i < File1diff.Count; i++)
                {
                    Console.WriteLine("File 1 extra content: " +File1diff[i]);
                }
            }
            else
            {
                Console.WriteLine("File 1 and File 2, both are having same number of lines.");

                for (int i = 0; i < File1diff.Count; i++)
                {
                    if (!File1diff[i].Equals(File2diff[i]))
                    {
                        Console.WriteLine("File 1 content: " +File1diff[i] + "\r\n" + "File 2 Content: " +File2diff[i]);
                    }

                }

            }

        }
    }
}
