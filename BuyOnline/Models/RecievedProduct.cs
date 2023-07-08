using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuyOnline.Models
{
    public class RecievedProduct
    {
        public int Id { get; set; }
        public bool Recieved { get; set; }
        public DateTime RecievedDate { get; set; }

        public string UserId { get; set; }
        // FK
        public int BuyProductId { get; set; }


        // navigation property
        public virtual BuyProduct BuyProduct { get; set; }
        public virtual ApplicationUser User { get; set; }


    }
}