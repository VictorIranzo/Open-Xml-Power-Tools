using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Runtime.Serialization.Json;
using System.Xml.Serialization;
using System.Web.Script.Serialization;
using OpenXmlPowerTools;

namespace JSON.Application
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            MyDTO myDTO = new MyDTO()
            {
                cuenta = 10,
                fecha = new DateTime(),
                headerDTO = new HeaderDTO() { Empresa = "Emp", template = new byte[] { 1,2,3} },
                Nombre = "Nombre",
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
                Formatting = Newtonsoft.Json.Formatting.Indented
            };

           string jsonMyDTO = JsonConvert.SerializeObject(myDTO,settings);
           JsonConvert.DeserializeObject(jsonMyDTO);
           XmlDocument xmlDTODoc = (XmlDocument) JsonConvert.DeserializeXmlNode(jsonMyDTO,"rootNode",true);
           string xmlDTO = xmlDTODoc.OuterXml;
           //string xmlDTO2 = Program.JsonToXml(jsonMyDTO).Root.Value;
           //XmlSerializer serializer = new XmlSerializer(myDTO.GetType());
            JavaScriptSerializer jsserializer = new JavaScriptSerializer();
            string json2 = jsserializer.Serialize(myDTO);

            bool e = true;
            WmlDocument wml = DocumentAssembler.AssembleDocument(new WmlDocument(@"D:\INFORMATICA\ADD\Open-Xml-PowerTools\JSON.Application\Pruebas.docx"), XElement.Parse(xmlDTODoc.OuterXml), out e);
            wml.SaveAs("W.docx");
        }

        static XDocument JsonToXml(string jsonString)
        {
            using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(jsonString)))
            {
                var quotas = new XmlDictionaryReaderQuotas();
                return XDocument.Load(JsonReaderWriterFactory.CreateJsonReader(stream, quotas));
            }
        }
    }
}
