using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace BuyOnline.Models
{
    //public enum Rating : byte
    //{
    //    Bad = 1, Accepted = 2, Good = 3, Very_Good = 4, Excellent = 5
    //}

    public class BuyProduct
    {
        public int Id { get; set; }


        [Required]
        public int Quantity { get; set; }


        public DateTime BuyDate { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        [Display(Name ="Recieved")]
        public bool Pay { get; set; }
        public string Message { get; set; }
        public byte Rating { get; set; }



        public int ProductId { get; set; }
        public string UserId { get; set; }



        public virtual  Product Product { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<RecievedProduct> Recieved { get; set; }


    }
}