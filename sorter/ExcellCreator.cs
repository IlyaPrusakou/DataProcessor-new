using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace sorter
{
    public class ExcelManipulator
    {
        public List<offer> Offers { get; set; }
        public ExcelManipulator()
        {

        }
        public ExcelManipulator(List<offer> ListWithDescr)
        {
            Offers = ListWithDescr;
        }
        private  string FormatStringWithZero(int simpleindex)
        {
            string zeroindex = Convert.ToString(simpleindex).PadLeft(7, '0'); ;
            return zeroindex;
        }
        public void GetExcelFromCSV(string CSVpath, string ExcelForSaving)
        {
            ExcelTextFormat format = new ExcelTextFormat();
            format.Delimiter = '\t';
            format.Culture = new CultureInfo(Thread.CurrentThread.CurrentCulture.ToString());
            format.Culture.DateTimeFormat.ShortDatePattern = "dd-mm-yyyy";
            format.Encoding = Encoding.GetEncoding(1251); // зачем ты ставишь эту кодировку!!! поставь UTF-8

            //FileInfo file = new FileInfo(@"D:\c# excel\solution\sorterNew-master\yml\fullcatalog.csv");
            FileInfo file = new FileInfo(CSVpath);
            //FileInfo openfile = new FileInfo(@"D:\c# excel\solution\sorterNew-master\yml\newfull.xlsx");
            FileInfo openfile = new FileInfo(ExcelForSaving);
            using (ExcelPackage excelPackage = new ExcelPackage(openfile))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("ExcelFromCSV");
                ExcelRange Dimension = worksheet.Cells;
                Dimension.LoadFromText(file, format);

                FileInfo fi = new FileInfo(ExcelForSaving);
                excelPackage.SaveAs(fi);
            }
        }
        private List<string> GetDistinctListOgAggregateNames()
        {
            List<string> list = new List<string>();
            foreach (offer item in Offers)
            {
                list.Add(item.AggregateName);
            }
            List<string> distinctlist  = list.Distinct().ToList();
            return distinctlist;
        }
        public void SaveToExcellForJoomla(string MyFile) 
        {
            Console.WriteLine("To save has begun");
            FileInfo existingFile = new FileInfo(MyFile);
            using (ExcelPackage package = new ExcelPackage(existingFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"ForJoomla {DateTime.Now}");
                List<string> list = GetDistinctListOgAggregateNames();
                list.Remove(null);
                Dictionary<string, List<offer>> dict = new Dictionary<string, List<offer>>();
                foreach (string item in list)
                {
                    List<offer> d = Offers.Where(offer => offer.AggregateName == item).ToList();
                    dict.Add(item, d);
                }
                int childcounter = 0;
                int counter = 0; //если надо продолжить с какого-то номераб то просто вместо нуля ставишь нужное число
                int step = 1;
                foreach (string item in dict.Keys)
                {
                    worksheet.Cells[step, 1].Value = dict[item][0].CategoryId;
                    worksheet.Cells[step, 2].Value = FormatStringWithZero (++counter); //еще надо тестировать
                    //worksheet.Cells[step, 2].Value = dict[item][0].CategoryName;
                    worksheet.Cells[step, 4].Value = item;
                    //dict[item][0].GetUnitPrice();
                    worksheet.Cells[step, 5].Value = dict[item][0].Price;
                    worksheet.Cells[step, 6].Value = "BYN";
                    dict[item][0].CheckImagePath();
                    worksheet.Cells[step, 7].Value = dict[item][0].CheckedImagePath;
                    worksheet.Cells[step, 8].Value = item; // dict[item][0].Description
                    worksheet.Cells[step, 9].Value = "1";
                   
                    worksheet.Cells[step, 10].Value = dict[item][0].CategoryName;
                    worksheet.Cells[step, 11].Value = dict[item][0].ParentCategoryId;
                    worksheet.Cells[step, 12].Value = dict[item][0].ParentCategoryName;
                    worksheet.Cells[step, 13].Value = dict[item][0].CategoryId; ////!!!!!!!!!
                    for (int i = 0; i < dict[item].Count; i++)
                    {
                        int innerstep = step + (i + 1);
                        worksheet.Cells[innerstep, 2].Value = "d" + FormatStringWithZero(++childcounter); // еще надо тестировать
                        worksheet.Cells[innerstep, 3].Value = FormatStringWithZero(counter); // еще надо тестировать
                        worksheet.Cells[innerstep, 4].Value = dict[item][i].ProductName;
                        //dict[item][i].GetUnitPrice();
                        worksheet.Cells[innerstep, 5].Value = dict[item][i].Price;
                        worksheet.Cells[innerstep, 6].Value = "BYN";
                        //worksheet.Cells[innerstep, 8].Value = dict[item][i].ProductName; ;
                        worksheet.Cells[innerstep, 9].Value = "1";
                        worksheet.Cells[innerstep, 10].Value = dict[item][0].CategoryName;
                        worksheet.Cells[innerstep, 11].Value = dict[item][0].ParentCategoryId;
                        worksheet.Cells[innerstep, 12].Value = dict[item][0].ParentCategoryName;
                        worksheet.Cells[innerstep, 13].Value = dict[item][0].CategoryId; ;
                    }
                    step = step + dict[item].Count + 1;
                }
                package.SaveAs(existingFile);
            }

        }
    }
}
