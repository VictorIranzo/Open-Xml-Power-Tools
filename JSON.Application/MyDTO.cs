using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JSON.Application
{
    public class MyDTO
    {
        public string Nombre { get; set; }
        public DateTime fecha { get; set; }
        public int cuenta { get; set; }

        public HeaderDTO headerDTO { get; set; }

        public IEnumerable<MySecondaryDTO> MySecondaryDTOs { get; set; }
    }
}
