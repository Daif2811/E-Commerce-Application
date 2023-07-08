using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuyOnline.Models
{
    public class BuyViewModel
    {
        public string ProductName { get; set; }
        public IEnumerable<BuyProduct> Persons { get; set; }
    }
}