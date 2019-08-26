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
    [XmlInclude(typeof(Barcode))]
    public class offer: IProduct
    {
        [XmlElement("categoryId")]
        public string CategoryId { get; set; } //use ProductExtractor.Extract(string filepath)
        [XmlElement("articul")]
        public string Articul { get; set; } //use ProductExtractor.Extract(string filepath)
        [XmlElement("brand")]
        public string Brand { get; set; } //use ProductExtractor.Extract(string filepath)
        [XmlAttribute("id")]
        public string OfferId { get; set; } //use ProductExtractor.Extract(string filepath)
        [XmlAttribute("available")]
        public string Availabe { get; set; } //use ProductExtractor.Extract(string filepath)
        [XmlElement("name")]
        public string ProductName { get; set; } //use ProductExtractor.Extract(string filepath)
        [XmlElement("price")]
        public string Price { get; set; } // decimal use ProductExtractor.Extract(string filepath)
        public decimal UnitPrice { get; set; }
        [XmlElement("unitName")]
        public string Unit { get; set; } //use ProductExtractor.Extract(string filepath)
        [XmlElement("koef")]
        public string Koef { get; set; } // decimal use ProductExtractor.Extract(string filepath)
        [XmlElement("imagelist")]
        public string ImagePath { get; set; } //use ProductExtractor.Extract(string filepath)
        public string CheckedImagePath { get; set; }
        public string Description { get; set; } // use Parser.InsertDescriptionToOffer()
        [XmlElement("barcodes")]
        public Barcode Barcodes { get; set; } // надо подумать как десериализовать
        [XmlElement("categoryname")]
        public string CategoryName { get; set; } // use ProductExtractor.GetCategoryName()
        [XmlElement("aggregatename")]
        public string AggregateName { get; set; } //use ProductExtractor.GetAggregateName(string excellfailpath)
        public string RoutUrl { get; set; }
        public string ParentCategoryId { get; set; }
        public string ParentCategoryName { get; set; }
        public offer()
        {

        }
        public void GetUnitPrice()
        {
            string kov = "\"";
            string checkedprice = Price.Replace(kov, "").Replace(".", ",");
            string checkedkoef = Koef.Replace(kov, "").Replace(".", ",");
            try
            {
                UnitPrice = Convert.ToDecimal(checkedprice) / Convert.ToDecimal(checkedkoef);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "--" + OfferId);
            }
             
        }

        public void CheckImagePath()
        {
            //string test = "http://images.firma-gamma.ru/images/9/5/df2310825161l.jpg,http://images.firma-gamma.ru/images/4/8/g2271755511.jpg";
            string test = ImagePath;
            string[] separators = { "http" };
            string[] massresult = test.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            string result = "";
            for (int i = 0; i <= massresult.Length - 1; i++)
            {
                string value = massresult[i];
                if (value.EndsWith(","))
                {
                    value = value.Remove(value.Length - 1);
                }
                if (i == 0)
                {
                    result = "http" + value;
                }
                else if (i > 0)
                {
                    value = "http" + value;
                    result = result + "  " + value;
                }
            }
            CheckedImagePath = result;
        }
        




    }
}
