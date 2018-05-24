using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocToPDFConverter;
using Syncfusion.OfficeChartToImageConverter;
using Syncfusion.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocxToPdf
{
    public class Converter
    {
        public static void ConvertDocxToPdf(string path)
        {
            //Load an existing Word document

            WordDocument wordDocument = new WordDocument(path, FormatType.Docx);

            //Initialize chart to image converter for converting charts during Word to pdf conversion

            wordDocument.ChartToImageConverter = new ChartToImageConverter();

            //Create an instance of DocToPDFConverter

            DocToPDFConverter converter = new DocToPDFConverter();

            //Convert Word document into PDF document

            PdfDocument pdfDocument = converter.ConvertToPDF(wordDocument);

            //Save the PDF file

            pdfDocument.Save("Generated.pdf");

            //Close the instance of document objects

            pdfDocument.Close(true);

            wordDocument.Close();
        }
    }
}
