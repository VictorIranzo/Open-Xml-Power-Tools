using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ChartUpdater_Application
{
    public class MyDTO
    {
        public IEnumerable<MySecondaryDTO> MySecondaryDTOs { get; set; }
    }
}
