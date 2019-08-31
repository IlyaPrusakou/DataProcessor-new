using OfficeOpenXml;
using OfficeOpenXml.Style;
using sorter.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sorter.Utils
{
    public class BillFormer
    {

        public BillFormer()
        {

        }

        public void FormBill(string path, List<BillData> outputbills, RareMutableData raredata)
        {
            FileInfo file = new FileInfo(path);
            using (ExcelPackage exc = new ExcelPackage(file))
            {
                ExcelWorksheet sheet = exc.Workbook.Worksheets.Add($"bill {raredata.Buyer}");
                exc.Workbook.CalcMode = ExcelCalcMode.Manual;
              
                sheet.Cells[1, 1].Value = "Продавец:";

                using (ExcelRange r = sheet.Cells[1, 1, 1, 2])
                {
                    
                    r.Merge = true;
                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    r.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    r.Style.Font.Bold = true;

                }
                sheet.Cells[1, 3].Value = raredata.User;

                using (ExcelRange r = sheet.Cells[1, 3, 1, 9])
                {

                    r.Merge = true;
                    r.Style.WrapText = true;
                    sheet.Row(1).Height = 80;
                }
                sheet.Cells[2, 1].Value = raredata.Date; 
                using (ExcelRange r = sheet.Cells[2, 1, 2, 9])
                {
                    
                    r.Merge = true;
                    r.Style.WrapText = true;
                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    r.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    r.Style.Font.Bold = true;
                }
                sheet.Cells[3, 1].Value = "Покупатель:";
                using (ExcelRange r = sheet.Cells [3, 1, 3, 2])
                {
                    
                    r.Merge = true;
                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    r.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    r.Style.Font.Bold = true;
                }
                sheet.Cells[3, 3].Value = raredata.Buyer;
                using (ExcelRange r = sheet.Cells[3, 3, 3, 9])
                {
                    
                    r.Merge = true;
                    r.Style.WrapText = true;
                    sheet.Row(3).Height = 80;
                }
                 sheet.Cells[4, 1].Value = "Счет действительн в течении" +
                    " 5-ти банковских дней, включая день оформления"; 
                using (ExcelRange r = sheet.Cells[4, 1, 4, 9])
                {
                    
                    r.Merge = true;
                    r.Style.WrapText = true;
                    
                }
                sheet.Cells[5, 1].Value = "Ваш запрос будет обработан в" +
                    " течение 1-3 дней. После проверки менеджером, Вам" +
                    " будет выслан уточненный счет, т.к. указанные цены" +
                    " могут отличаться от отпускных как в большую, так и" +
                    " в меньшую сторону. В случае изменений в Вашем заказе," +
                    " связанных с отсутствием товаров или увеличением сроков" +
                    " доставки, менеджер свяжется с Вами.";
                using (ExcelRange r = sheet.Cells[5, 1, 5, 9])
                {
                    

                    r.Merge = true;
                    r.Style.WrapText = true;
                    sheet.Row(5).Height = 80;
                }
                sheet.Cells[6, 1].Value = "№";
                sheet.Cells[6, 2].Value = "Товар";
                sheet.Cells[6, 3].Value = "Кол-во";
                sheet.Cells[6, 4].Value = "Ед.";
                sheet.Cells[6, 5].Value = "Цена, Белорусский рубль";
                sheet.Cells[6, 6].Value = "Сумма, Белорусский рубль";
                sheet.Cells[6, 7].Value = "% НДС";
                sheet.Cells[6, 8].Value = "Сумма НДС, Белорусский рубль";
                sheet.Cells[6, 9].Value = "Сумма с НДС, Белорусский рубль";
                ExcelRange tableheader = sheet.Cells[6, 1, 6, 9];
                tableheader.Style.WrapText = true;
                tableheader.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                tableheader.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                tableheader.Style.Font.Bold = true;
                int count = 0;
                for (int i = 0; i < outputbills.Count; i++)
                {
                    sheet.Cells[i+7, 1].Value = outputbills[count].BillPosition;
                    sheet.Cells[i+7, 2].Value = outputbills[count].Name.FullName;
                    sheet.Cells[i + 7, 2].AutoFitColumns(86);
                    sheet.Cells[i+7, 3].Value = outputbills[count].Quantity;
                    sheet.Cells[i+7, 4].Value = outputbills[count].Package;
                    sheet.Cells[i+7, 5].Value = outputbills[count].Price;
                    sheet.Cells[i+7, 6].Value = Convert.ToDecimal(outputbills[count].Amount);
                    sheet.Cells[i+7, 7].Value = outputbills[count].VatPercent;
                    sheet.Cells[i+7, 8].Value = Convert.ToDecimal(outputbills[count].AmountOfVat);
                    sheet.Cells[i+7, 9].Value = Convert.ToDecimal(outputbills[count].TotalAmount);
                    ExcelRange row = sheet.Cells[i + 7, 1, i + 7, 9];
                    row.Style.WrapText = true;
                    row.Style.Indent = 1; 
                    row.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    row.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    
                    count = count + 1;
                }
                sheet.Cells[9, 1].Value = "Итого";
                using (ExcelRange r = sheet.Cells[9, 1, 9, 5])
                {
                    
                    r.Merge = true;
                    r.Style.WrapText = true;

                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    r.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    r.Style.Font.Bold = true;

                }
                sheet.Cells[9, 6].Formula = $"=SUM(F7:F{count + 6})";
                sheet.Cells[9, 8].Formula = $"=SUM(H7:H{count + 6})";
                sheet.Cells[9, 9].Formula = $"=SUM(I7:I{count + 6})";
                exc.Workbook.Calculate();
                sheet.Cells[10, 1].Value = $"Всего наименований {outputbills.Count}, на сумму {sheet.Cells[9, 9].Value.ToString()} белорусских рублей";
                using (ExcelRange r = sheet.Cells[10, 1, 10, 9])
                {
                    
                    r.Merge = true;
                    r.Style.WrapText = true;
                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    r.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }
                //skip row = 11
                sheet.Cells[12, 1].Value = "Индивидуальный предприниматель ____________________________________/И.А.Прусаков";
                using (ExcelRange r = sheet.Cells[12, 1, 12, 9])
                {
                    
                    r.Merge = true;
                    r.Style.WrapText = true;
                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    r.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }
                exc.Save();
            }
        }
    }
}
