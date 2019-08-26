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
    public class FurcomModel: IFurcomModel
    {
        protected List<FurcomPositionData> BufferedData;
        public List<FurcomPositionData> SelectedData { get; set; }

        public FurcomModel(string filepath)
        {
            GetBufferedData(filepath);
            SelectedData = new List<FurcomPositionData>();
        }

        protected void GetBufferedData(string filepath)
        {

            XmlSerializer xs = new XmlSerializer(typeof(List<FurcomPositionData>));

            using (FileStream fs = new FileStream(filepath, FileMode.Open))
            {
                using (XmlReader str = XmlReader.Create(fs))  // обязательно добавь  <ArrayOfOffer>
                {
                    BufferedData = (List<FurcomPositionData>)xs.Deserialize(str);
                }
            }
        }

        public void GetByName(string name)
        {

                SelectedData.Clear();
                FurcomPositionData loc = BufferedData.FirstOrDefault(b => b.ProductName == name);
                SelectedData.Add(loc);
            
            
        }


        public void ShowInfo()
        {
            Console.WriteLine("This is FurcomModel");
        }

    }
}
