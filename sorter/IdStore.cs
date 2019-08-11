using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace sorter
{
     public class IdStore
     {
        public List<IdData> IdDataList { get; set; }

        public IdStore()
        {
            IdDataList = new List<IdData>();
        }

        public void ExtractIdData(string filepath) 
        {
            using (Stream fs = File.Open(filepath, FileMode.Open, FileAccess.ReadWrite))
            {
                using (XmlReader rdr = XmlReader.Create(fs))
                {
                    XDocument doc = XDocument.Load(rdr);
                    XElement rootYmlElement = doc.Root;
                    IEnumerable<XElement> Categories = rootYmlElement.Descendants("category");
                    Dictionary<string, string> parentnames = new Dictionary<string, string>();
                    var col = Categories.Where(element => element.FirstAttribute == element.LastAttribute);
                    foreach (var item in col)
                    {
                        parentnames.Add(item.FirstAttribute.Value, item.Value);
                    }
                    foreach (XElement elem in Categories)
                    {
                        if (elem.FirstAttribute.Value != elem.LastAttribute.Value)
                        {
                            IdData localdata = new IdData();
                            localdata.CategoryName = elem.Value;
                            localdata.CategoryNumber = elem.FirstAttribute.Value;
                            localdata.ParentCategoryNumber = elem.LastAttribute.Value;
                            foreach (var p in parentnames.Keys)
                            {
                                if (elem.LastAttribute.Value == p)
                                {
                                    localdata.ParentCategoryName = parentnames[p]; // add return
                                    break;
                                }
                            }

                            
                            IdDataList.Add(localdata);
                        }
                    }
                }
                Console.WriteLine("ExtractIdData end");
            }
        }
        public Dictionary<string, List<string>> CreateDictionary()
        {
            Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
            if (IdDataList.Count > 0)
            {
                foreach (IdData item in IdDataList)
                {
                    List<string> list = new List<string>();
                    list.Add(item.CategoryName);
                    list.Add(item.ParentCategoryNumber);
                    list.Add(item.ParentCategoryName);
                    dict.Add(item.CategoryNumber, list);
                }
            }
            else
            {
                Console.WriteLine("IdDataList is empty. Invoke ExtractIdData()");
            }
            return dict;
        }
        public void CreateListOfCategoriesPath(FileInfo openfile)
        {
           
           

            using (ExcelPackage excelPackage = new ExcelPackage(openfile))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("ListOfCategories");
                for (int i = 0; i < IdDataList.Count; i++)
                {
                    
                    StringBuilder localbuilder = new StringBuilder(" ");
                    localbuilder.Append(IdDataList[i].ParentCategoryName + "/" + IdDataList[i].CategoryName.Trim());
                    string resultBulider = localbuilder.ToString().Remove(0, 1);
                    worksheet.Cells[i + 1, 1].Value = resultBulider;
                    worksheet.Cells[i + 1, 2].Value = IdDataList[i].CategoryNumber;

                }

                excelPackage.Save();
                Console.WriteLine("CreateListOfCategoriesPath has finished");
            }
        }
    }
}
