using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sorter.Interfaces
{
    public interface IFurcomModel
    {
        //List<IProduct> SelectedData { get; set; }
        void ShowInfo();
        void GetByName(string name);
    }
}
