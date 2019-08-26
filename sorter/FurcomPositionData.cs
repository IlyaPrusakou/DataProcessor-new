using sorter.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace sorter
{
    [Serializable]
    public class FurcomPositionData: IProduct
    {
        [XmlElement("product_name")]
        public string ProductName { get; set; }
        [XmlElement("product_desc")]
        public string Description { get; set; }
        

        public FurcomPositionData()
        {

        }





    }
}
