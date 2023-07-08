using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BuyOnline.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required, Display(Name ="Product Name")]
        public string ProductName { get; set; }

        [Required, Display(Name = "Product Description")]
        public string ProductDescription { get; set; }

        [ Display(Name = "Product Image")]
        public string ProductImage { get; set; }

        [Required, Display(Name = "Product Quantity")]
        public int ProductQuantity { get; set; }


        [Required, Display(Name = "Product Price")]
        public string ProductPrice { get; set; }

        // FK
        public int CategoryId { get; set; }
        public string UserId { get; set; }


        // Navigation property
        public virtual ApplicationUser User { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<BuyProduct> BuyProducts { get; set; }
        public virtual ICollection<AddToCart> AddToCarts { get; set; }

    }
}