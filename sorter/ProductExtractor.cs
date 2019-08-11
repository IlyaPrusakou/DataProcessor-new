using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace sorter
{
    class ProductExtractor
    {
        private IdStore _idstore;
        public List<offer> Offers {get;set;}
        public List<string> Log { get; set; } //= new List<string>();
        public ProductExtractor(IdStore idstore)
        {
            _idstore = idstore;
            Log = new List<string>();


        }
        public void GetCategories() 
        {
            Dictionary<string, List<string>> dict = _idstore.CreateDictionary();
            foreach (offer item in Offers)
            {
                if (item.CategoryId != "" && item.CategoryId != " " && item.CategoryId != null)
                {
                    item.CategoryName = dict[item.CategoryId][0];
                    item.ParentCategoryId = dict[item.CategoryId][1];
                    item.ParentCategoryName = dict[item.CategoryId][2];
                }
                
            }
        }
        private decimal RemoveAndConvertKey(string keystr)
        {
            string onekov = "\"";
            string keystringwtihkov = keystr;
            decimal key = Convert.ToDecimal(keystringwtihkov.Replace(onekov, ""));
            return key;
        }
        public void GetAggregateName(string excellfailpath)
        {
            Dictionary<decimal, string> IdNamePair = new Dictionary<decimal, string>();
            List<offer> newlist = new List<offer>();
            List<string> s = new List<string>();
            FileInfo FullCatalogCSV = new FileInfo(excellfailpath);
            using (ExcelPackage excelPackage = new ExcelPackage(FullCatalogCSV))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["ExcelFromCSV"];
                ExcelRange range = worksheet.Cells[1, 1, worksheet.Cells.End.Row, 4];
                for (int i = worksheet.Dimension.Start.Row; i <= worksheet.Dimension.End.Row; i++)
                {
                    try
                    {
                        if (range[i, 1].Value != null && range[i, 4].Value != null)
                        {
                            decimal key = RemoveAndConvertKey(range[i, 1].Value.ToString());
                            IdNamePair.Add(key, range[i, 4].GetValue<string>());
                        }
                        else if (range[i, 1].Value == null)
                        {
                            //IdNamePair.Add(i, range[i, 4].GetValue<string>());
                            string errormessage = $"row {i}, keynull, GetAggregateName";
                            Log.Add(errormessage);
                        }
                        else if (range[i, 4].Value == null)
                        {
                            decimal key = RemoveAndConvertKey(range[i, 1].Value.ToString());
                            //IdNamePair.Add(key, $"nullName {i}");
                            string errormessage2 = $"row {i}, {key}, nullname, GetAggregateName";
                            Log.Add(errormessage2);
                        }
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine(ex.Message + "  " + i);
                        string errormessage3 = $"row {i}, {ex.Message}, GetAggregateName";
                        Log.Add(errormessage3);
                    }
                    
                }
                foreach (decimal key in IdNamePair.Keys)
                {
                    foreach (offer item in Offers)
                    {
                        if (key == Convert.ToDecimal(item.OfferId))
                        {
                            item.AggregateName = IdNamePair[key];
                            break;
                        }
                        
                    }
                }
                Console.WriteLine("AggregateName end"  +  Offers.Count + "  " + IdNamePair.Keys.Count);
            }
        }
        public  void Extract(string filepath)
        {

            XmlSerializer xs = new XmlSerializer(typeof(List<offer>));
            
            using (FileStream fs = new FileStream(filepath, FileMode.Open))
            {
                using (XmlReader str = XmlReader.Create(fs))  // обязательно добавь  <ArrayOfOffer>
                {
                    Offers = (List<offer>)xs.Deserialize(str);
                }
            }
            
            Console.WriteLine("Extract end" + "  " + Offers.Count);  
        }
        public void RemoveUnavalaibleProducts() // в этом метода надо поменять логику
            //эти товары мне нужны, но их поля дают null =>  надо их заменить на заглушки
            // или все таки надо их убрать.
        {
            Console.WriteLine(Offers.Count);
            
            for (int i = 0; i < Offers.Count; i++)
            {

                //if (Offers[i].CategoryId == null || Offers[i].CategoryId == "" || Offers[i].CategoryId == " ")
                if(Offers[i].Availabe == "false")
                {
                    Offers.Remove(Offers[i]);
                }
            }
            
        }

    }
}
