using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BuyOnline.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace BuyOnline.Controllers
{
    public class RoleController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();


        // GET: Role
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Roles.ToList());
        }

        // GET: Role/Details/5
        [Authorize]
        public ActionResult Details(string id)
        {
            var role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }

            return View(role);
        }

        // GET: Role/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Role/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(IdentityRole role)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    db.Roles.Add(role);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View(role);
        }

        // GET: Role/Edit/5
        [Authorize]
        public ActionResult Edit(string id)
        {
            var role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();

            }

            return View(role);
        }

        // POST: Role/Edit/5
        [HttpPost]
        [Authorize]
        public ActionResult Edit(IdentityRole role)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    db.Entry(role).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View(role);
        }

        // GET: Role/Delete/5
        [Authorize]
        public ActionResult Delete(string id)
        {
            var role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // POST: Role/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(IdentityRole role)
        {
            try
            {
                // TODO: Add delete logic here
                var del = db.Roles.Find(role.Id);
                db.Roles.Remove(role);
                db.SaveChanges();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return RedirectToAction("Index");
        }










        // Admins
        // ==============================================================================


         // All Seller
         //============
        [Authorize(Roles ="Admins")]
        public ActionResult Sellers()
        {
            //var userId = User.Identity.GetUserId();
            var sellers = db.Users.Where(a => a.UserType == "Seller").ToList();
            return View(sellers);
        }



        // All Product of Seller
        // ====================
        public ActionResult SellerProducts(string id)
        {
            //var userId = User.Identity.GetUserId();
            var products = db.Products.Where(a => a.UserId == id ).ToList();
            return View(products);
        }







        // All Customers
        // ====================
        [Authorize(Roles ="Admins")]
        public ActionResult Customers()
        {
            
            var customers = db.Users.Where(a => a.UserType == "Customer");
            return View(customers);
        }





        // All Buying of Customers
        // ====================
        public ActionResult CustomerBuyings(string id)
        {

            //var products = from buy in db.BuyProducts
            //               join pro in db.Products
            //               on buy.ProductId equals pro.ProductId
            //               where (buy.UserId == id)
            //               select new { buy.Quantity, buy.Rating,buy.BuyDate , buy.Address, buy.PhoneNumber , pro.ProductName , pro.ProductImage , pro.ProductPrice  };

            var products = db.BuyProducts.Where(a => a.UserId == id).ToList();

                
            return View(products);
        }






        // All Admins
        // ====================
        [Authorize(Roles = "Admins")]
        public ActionResult Admins()
        {
            var userId = User.Identity.GetUserId();
            var admins = db.Users.Where(a => a.Id == userId || a.UserType == "Admins");
            return View(admins);
        }













        // For Sellers
        // =====================================================================================




        // All Customers of seller
        // =======================
        public ActionResult SellerCustomers()
        {
            var userId = User.Identity.GetUserId();

            var products = from seller in db.Products
                           join buy in db.BuyProducts
                           on seller.ProductId equals buy.ProductId
                           where seller.UserId == userId
                           select buy.User;

            //var products = db.Products.Where(u => u.UserId == userId);



            return View(products.ToList());
        }







        // All Buying of Customers from this seller
        // ================================
        public ActionResult HistoryOfCustomer(string id)
        {
            var SellerId = User.Identity.GetUserId();
            if (id == null)
            {
                return HttpNotFound();
            }

            var list = db.BuyProducts.Where(a => a.UserId == id && a.Product.UserId == SellerId );

            return View(list.ToList());
        }






    }
}
