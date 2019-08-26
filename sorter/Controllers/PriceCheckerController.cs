using OfficeOpenXml;
using sorter.Interfaces;
using sorter.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sorter.Controllers
{
    public class PriceCheckerController
    {
        private List<BillData> bills;


        public IFurcomModel FurcomMod {get; private set;}
        public IGammaModel GammaMod { get; private set; }
        public List<BillData> OutputBills { get; set; }


        public PriceCheckerController(IFurcomModel furmod, IGammaModel gammod)
        {
            FurcomMod = furmod;
            GammaMod = gammod;


        }
        //Action
        public void MakeTheCheckedBill(string path)
        {
            DeepCopier copier = new DeepCopier();
            GetDataFromBill(path);
            GammaMod.GetAll();

            foreach (var billposition in bills) 
            {
                FurcomMod.GetByName(billposition.Name.GeneralName);
            
                FurcomIdReceiver IdReceiver = new FurcomIdReceiver();
                FurcomModel loc = (FurcomModel)FurcomMod;
                string id = IdReceiver.GetIdFromFurcomProductDescr(loc.SelectedData[0].Description);
                CheckerGammaList GetterFromGamma = new CheckerGammaList();
                GammaModel loc2 = (GammaModel)GammaMod;
                GetterFromGamma.FindOfferInGammaBase(id, loc2.SelectedData);
                offer result = GetterFromGamma.FoundOffer;

                BillData newbilldata = copier.DeepClone<BillData>(billposition); //  это надо проверить
                newbilldata.Price = result.Price; // convert to decimal
                OutputBills.Add(newbilldata);
            }
            // private void PrintOutputBillsToExcellBill();

        }
        private void GetDataFromBill(string path)
        {
            int beginrow = 0;
            int endrow = 0;
            bool beginflag = false;
            bool endflag = false;
            bills = new List<BillData>();
            FileInfo file = new FileInfo(path);
            using (ExcelPackage exc = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = exc.Workbook.Worksheets[1];
                ExcelRange Dimension = worksheet.Cells;
                for (int i = 1; i < Dimension.End.Row; i++)
                {
                    if (worksheet.Cells[i, 1].Value.ToString() == "№")
                    {
                        beginrow = i;
                        beginflag = true;

                    }
                    else if (worksheet.Cells[i, 1].Value.ToString() == "Итого")
                    {
                        endrow = i;
                        endflag = true;
                    }
                    else if (beginflag & endflag)
                    {
                        break;
                    }
                }
                if (beginflag && endflag)
                {
                    for (int i = beginrow + 1; i < endrow; i++)
                    {
                        BillData loc = new BillData();

                        loc.Position = i;
                        loc.BillPosition = Convert.ToInt32(worksheet.Cells[i, 1].Value);
                        loc.Name = new BillPositionName(worksheet.Cells[i, 2].Value.ToString());
                        loc.Quantity = worksheet.Cells[i, 3].Value.ToString();
                        loc.Package = worksheet.Cells[i, 4].Value.ToString();
                        loc.Price = worksheet.Cells[i, 5].Value.ToString();
                        loc.Amount = worksheet.Cells[i, 6].Value.ToString();
                        loc.VatPercent = worksheet.Cells[i, 7].Value.ToString();
                        loc.AmountOfVat = worksheet.Cells[i, 8].Value.ToString();
                        loc.TotalAmount =worksheet.Cells[i, 9].Value.ToString();
                        
                        bills.Add(loc);
                    }
                }
            }
        }
    }
}
