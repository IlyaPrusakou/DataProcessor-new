using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sorter
{
    public class FurcomIdReceiver
    {


        public FurcomIdReceiver()
        {

        }


        public string GetIdFromFurcomProductDescr(string inputDescr)
        {
            string res = "";
            string[] separator = new string[] { "ID", "<br" };
            string[] resultmass = inputDescr.Split(separator, StringSplitOptions.None);
            res = resultmass.FirstOrDefault(str => str.Contains("товара")).Remove(0, 8);
            return res;
        }


    }
}
