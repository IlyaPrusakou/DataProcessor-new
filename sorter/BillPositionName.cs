using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sorter
{
    [Serializable]
    public class BillPositionName
    {
        public string FullName { get; set; }
        public string GeneralName { get; set; }
        public string SpecialName { get; set; }

        public BillPositionName()
        {

        }

        public BillPositionName(string fullname)
        {
            FullName = fullname;
            List<string> names = DevideFullname(fullname);
            if (names.Count == 2)
            {
                GeneralName = names[0];
                SpecialName = names[1];
            }
            else
            {
                GeneralName = names[0];
                SpecialName = "undefined";
            }
        }

        private List<string> DevideFullname(string fullname)
        {
            string separatorstr = " - ";
            List<string> reslist = new List<string>();
            if (fullname.Contains(separatorstr))
            {
                string[] sep = new string[] { separatorstr };
                string[] res = fullname.Split(sep, StringSplitOptions.None);
                foreach (string str in res)
                {
                    reslist.Add(str);
                }
            }
            else
            {
                reslist.Add(fullname);
            }
            return reslist;
        }
    }
}
