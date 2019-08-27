using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.TypeSystem;
using sorter.Controllers;
using sorter.Interfaces;
using sorter.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace sorter
{
    class Program
    {
       
        static void Main(string[] args)
        {
            CultureInfo cur2 = CultureInfo.CurrentCulture;
            CultureInfo targ = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentCulture = targ;
            CultureInfo.DefaultThreadCurrentUICulture = targ;
            Thread.CurrentThread.CurrentCulture = targ;
            Thread.CurrentThread.CurrentUICulture = targ;

            CultureInfo cur = CultureInfo.CurrentCulture;

            Console.WriteLine($"cur.DisplayName----{cur.DisplayName}");
            Console.WriteLine($"cur.EnglishName----{cur.EnglishName}");
            Console.WriteLine($"cur.Name----{cur.Name}");
            Console.WriteLine($"cur.NativeName----{cur.NativeName}");
            Console.WriteLine("CurrencyDecimalDigits----" + cur.NumberFormat.CurrencyDecimalDigits);
            Console.WriteLine("CurrencyDecimalSeparator----" + cur.NumberFormat.CurrencyDecimalSeparator);
            Console.WriteLine("NumberDecimalDigits----" + cur.NumberFormat.NumberDecimalDigits);
            Console.WriteLine("NumberFormat.NumberDecimalSeparator----" + cur.NumberFormat.NumberDecimalSeparator);
            Console.WriteLine("PercentDecimalDigits----" + cur.NumberFormat.PercentDecimalDigits);
            Console.WriteLine("PercentDecimalSeparator----" + cur.NumberFormat.PercentDecimalSeparator);
            Console.WriteLine("-----  .   --------");




            FurcomModel fur = new FurcomModel(@"D:\FurcomBase.xml");
            GammaModel gam = new GammaModel(@"D:\testbig.yml");
            PriceCheckerController controller = new PriceCheckerController(fur, gam);
            Console.WriteLine("Введите путь к счету"); 
            string filepath = Console.ReadLine();
            controller.MakeTheCheckedBill(@filepath);







            string pathToCategories = "";
            string result = "";
            IdStore store = new IdStore();
            while (true)
            {
                Console.WriteLine("a - получить список категорий ; s - получить эксель для загрузку в joomla ");
                Console.WriteLine("Введите букву");
                string choosenLiter = Console.ReadLine();

                switch (choosenLiter)
                {
                    case "a":
                        Console.WriteLine("введите путь к данным с категориями");
                        pathToCategories = @Console.ReadLine();
                        Console.WriteLine("где сохранить документ эксель (путь+название.xlsx)");
                        result = @Console.ReadLine();
                        store.ExtractIdData(pathToCategories);
                        FileInfo categoryfile = new FileInfo(result);
                        store.CreateListOfCategoriesPath(categoryfile);
                        break;
                    case "s":
                        Console.WriteLine("введите путь к данным с категориями");
                        pathToCategories = @Console.ReadLine();
                        Console.WriteLine("введите путь к данным без категорий(только товары)");
                        string pathToOffers = @Console.ReadLine();
                        Console.WriteLine("где сохранить документ эксель (путь+название.xlsx)");
                        result = @Console.ReadLine();
                        store.ExtractIdData(pathToCategories);
                        ProductExtractor extr = new ProductExtractor(store);
                        extr.Extract(pathToOffers);
                        extr.RemoveUnavalaibleProducts();
                        // из эксел файла я беру offerid и название товара без цвет
                        // в yml  есть только название товара с цветом agregatename = offerid + (offername - color).
                        extr.GetAggregateName(@"D:\c# excel\solution\sorterNew-master\yml\newfull.xlsx"); 
                        extr.GetCategories();
                        ExcelManipulator creator = new ExcelManipulator(extr.Offers);
                        creator.SaveToExcellForJoomla(result);
                        break;
                    default:
                        break;
                }

            }

            //string path2 = @"D:\c# excel\solution\sorterNew-master\yml\full.yml";
            //IdStore store = new IdStore();
            //string path3 = @"D:\c# excel\solution\sorterNew-master\yml\fullcatalog.yml";
           // store.ExtractIdData(path3);
            //FileInfo categoryfile = new FileInfo(@"D:\c# excel\solution\sorterNew-master\yml\categoryfile.xlsx");
            //store.CreateListOfCategoriesPath(categoryfile);
            


            //ProductExtractor extr = new ProductExtractor(store);
            //extr.Extract(path2);
            //extr.RemoveUnavalaibleProducts();
            //extr.GetAggregateName(@"D:\c# excel\solution\sorterNew-master\yml\newfull.xlsx"); // было savetry.xlsx
            //extr.GetCategories();
            //ExcelManipulator creator = new ExcelManipulator(extr.Offers);
            //string MyFile = @"D:\c# excel\target.xlsx";
           // creator.SaveToExcellForJoomla(MyFile);
            //Console.WriteLine("end");
            Console.ReadLine();
        }
    }
}

// ArrayOfOffer и offer  не могу переопределить имя корневого элемента в ProductData
//(так красивее).
// есть еще вопрос с тем как мне десериализовать  два одинаковых тега <barcode> 
// в производный класс Barcode.