using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace sorter
{
    [Serializable]
    public class Barcode
    {
        //[XmlElement("barcode")]
        //[XmlAttribute("opt")]
        public string Opt { get; set; }
        //[XmlElement("barcode")]
        //[XmlAttribute("producer")]
        public string Producer { get; set; }
        
    }
}
