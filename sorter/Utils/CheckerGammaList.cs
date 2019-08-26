using sorter.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sorter.Utils
{
    public class CheckerGammaList
    {
        public offer FoundOffer { get; set; }

        public CheckerGammaList()
        {

        }


        public void FindOfferInGammaBase(string id, List<offer> listoffer)
        {
            try
            {
                FoundOffer = listoffer.FirstOrDefault<offer>(g => g.ImagePath.Contains(id));
            }
            catch
            {
                FoundOffer = new offer()
                {
                    ProductName = "nomatch"
                };
            }

        }


    }
}
