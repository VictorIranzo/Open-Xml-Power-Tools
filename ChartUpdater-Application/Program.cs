using Newtonsoft.Json;
using OpenXmlPowerTools;
using System;
using System.Collections.Generic;
using System.Xml;

namespace ChartUpdater_Application
{
    class Program
    {
        static void Main(string[] args)
        {
            MyDTO myDTO = new MyDTO()
            {
                MySecondaryDTOs = new List<MySecondaryDTO>() {
                    new MySecondaryDTO() { cuenta = 1, fecha = new DateTime(), Nombre = "Sec" },
                    new MySecondaryDTO() { cuenta = 3, fecha = new DateTime(), Nombre = "Sec2" },
                    new MySecondaryDTO() { cuenta = 2, fecha = new DateTime(), Nombre = "Sec3" },
                }
            };

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.None,
            };

            string jsonMyDTO = JsonConvert.SerializeObject(myDTO, settings);
            XmlDocument xmlDTODoc = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonMyDTO, "rootNode", true);

            WmlDocument wml = DocumentAssembler.AssembleDocument(new WmlDocument(@"D:\INFORMATICA\ADD\Open-Xml-PowerTools\ChartUpdater-Application\Documento_Grafico.docx"), xmlDTODoc, out bool e);
            wml.SaveAs("W.docx");
        }
    }
}
