using sorter.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sorter
{
    [Serializable]
    public class BillData: IProduct
    {
        public int Position { get; set; }
        public int BillPosition { get; set; }
        public BillPositionName Name { get; set; }
        public string Quantity { get; set; }
        public string Package { get; set; }
        public string Price { get; set; }
        public string Amount { get; set; }
        public string VatPercent { get; set; } 
        public string AmountOfVat { get; set; }
        public string TotalAmount { get; set; }

        public string Description { get; set; }

        public BillData()
        {

        }

    }
}
