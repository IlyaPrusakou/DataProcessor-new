using sorter.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace sorter
{
    public class GammaModel: IGammaModel
    {
        protected List<offer> BufferedData;
        public List<offer> SelectedData { get; set; }

        public GammaModel(string filepath)
        {
            GetBufferedData(filepath);
            //SelectedData = BufferedData;
        }

        protected void GetBufferedData(string filepath)
        {

            XmlSerializer xs = new XmlSerializer(typeof(List<offer>));

            using (FileStream fs = new FileStream(filepath, FileMode.Open))
            {
                using (XmlReader str = XmlReader.Create(fs))  // обязательно добавь  <ArrayOfOffer>
                {
                    BufferedData = (List<offer>)xs.Deserialize(str);
                }
            }
        }

        public void GetAll()
        {
            SelectedData = BufferedData;
        }


        public void ShowInfo()
        {
            Console.WriteLine("This is GammaModel");
        }




    }
}
