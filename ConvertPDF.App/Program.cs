using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocxToPdf;

namespace ConvertPDF.App
{
    class Program
    {
        static void Main(string[] args)
        {
            Converter.ConvertDocxToPdf("./Files/Report.docx");
        }
    }
}
