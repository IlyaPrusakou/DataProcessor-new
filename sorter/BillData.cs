using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sorter
{
    public class BillData
    {
        public int Position { get; set; }
        public BillPositionName Name { get; set; }
        public decimal Quantity { get; set; }
        public string Package { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public string VatPercent { get; set; } 
        public decimal AmountOfVat { get; set; }
        public decimal TotalAmount { get; set; }

        public BillData()
        {

        }

    }
}
