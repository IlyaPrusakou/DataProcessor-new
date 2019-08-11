using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace sorter
{
    [Serializable]
    public class Description
    {
        [XmlElement("offerid")]
        public string OfferId { get; set; }
        [XmlElement("content")]
        public string Content { get; set; }
        public Description()
        {

        }
    }
}
