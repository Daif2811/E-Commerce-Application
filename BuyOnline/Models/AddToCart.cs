using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuyOnline.Models
{
    public class AddToCart
    {
        public int Id { get; set; }
        public DateTime AddToCartDate { get; set; }


        // FK
        public int ProductId { get; set; }
        public string UserId { get; set; }



        // Navigation property
        public virtual Product Product { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}