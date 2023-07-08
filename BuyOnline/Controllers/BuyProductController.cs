using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BuyOnline.Models;
using Microsoft.AspNet.Identity;

namespace BuyOnline.Controllers
{
    public class BuyProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BuyProduct
        public ActionResult Index()
        {
            var buyProducts = db.BuyProducts.Include(b => b.Product).Include(b => b.User);
            return View(buyProducts.ToList());
        }





        // GET: BuyProduct/Details/5

        public ActionResult Details(int id)
        {
            var product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            Session["ProductId"] = id;
            return View(product);
        }











        [Authorize]
        public ActionResult DetailsMyBuy(int id)
        {
            var userId = User.Identity.GetUserId();
            var product = db.BuyProducts.Where(a => a.ProductId == id);


            if (product == null)
            {
                return HttpNotFound();
            }

            var detail = db.BuyProducts.Where(a => a.UserId == userId && a.ProductId == id).SingleOrDefault();

            return View(detail);

        }











        // GET: BuyProduct/Create
        // POST: BuyProduct/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        [Authorize]
        public ActionResult BuyProduct(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            Session["Id"] = id;

            ViewBag.Rating = new SelectList(new[] { 1, 2, 3, 4, 5 });
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult BuyProduct(BuyProduct product)
        {
            ViewBag.Rating = new SelectList(new[] { 1, 2, 3, 4, 5 });

            var userId = User.Identity.GetUserId();

            var thisProduct = db.Products.Find((int)Session["Id"]);


            if (thisProduct == null)
            {
                return HttpNotFound();
            }
            else
            {

                product.ProductId = thisProduct.ProductId;
                product.UserId = userId;
                product.BuyDate = DateTime.Now;
                product.Pay = false;

                // parameters

                if (ModelState.IsValid)
                {
                    // check if the product is already buy
                    var checkExist = db.BuyProducts.Where(b => b.ProductId == thisProduct.ProductId && b.UserId == userId).SingleOrDefault();
                    if (checkExist != null)
                    {
                        TempData["Error"] = "You Already buy this product, You can change your information and quantity";
                        return RedirectToAction("EditBuy", new { checkExist.Id });
                    }




                    if (thisProduct.ProductQuantity < product.Quantity)
                    {
                        ViewBag.Error = "Sorry This Quantity is not Available , Available Quantity is " + thisProduct.ProductQuantity;
                        return View(product);
                    }
                    else
                    {
                        db.BuyProducts.Add(product);
                        db.SaveChanges();
                        thisProduct.ProductQuantity = thisProduct.ProductQuantity - product.Quantity;
                        db.SaveChanges();
                    }

                }


                // Check if the product exist in cart  Delete it from cart  after buying
                var checkCarts = db.AddToCarts.Where(a => a.ProductId == thisProduct.ProductId && a.UserId == userId).SingleOrDefault();
                if (checkCarts != null)
                {
                    db.AddToCarts.Remove(checkCarts);
                    db.SaveChanges();
                }



                return RedirectToAction("MyOrders");
            }
        }











        // GET: BuyProduct/Edit/5
        // POST: BuyProduct/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 

        [Authorize]
        public ActionResult EditBuy(int? id)
        {
            ViewBag.Rating = new SelectList(new[] { 1, 2, 3, 4, 5 });

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BuyProduct product = db.BuyProducts.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            ViewBag.Error = TempData["Error"];
            Session["ProductId"] = product.ProductId;
            return View(product);
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditBuy(BuyProduct product)
        {
            ViewBag.Rating = new SelectList(new[] { 1, 2, 3, 4, 5 });
            var userId = User.Identity.GetUserId();
            var thisProduct = db.Products.Find((int)Session["ProductId"]);

            if (thisProduct == null)
            {
                return HttpNotFound();
            }
            else
            {
                product.ProductId = thisProduct.ProductId;
                product.UserId = userId;
                product.BuyDate = DateTime.Now;
                product.Pay = true;

            }


            if (ModelState.IsValid)
            {

                if (thisProduct.ProductQuantity < product.Quantity)
                {
                    ViewBag.Error = "Sorry This Quantity is not Available , Available Quantity is " + thisProduct.ProductQuantity;
                    return View(product);
                }



                db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                thisProduct.ProductQuantity = thisProduct.ProductQuantity - product.Quantity;
                db.SaveChanges();
                return RedirectToAction("MyOrders");
            }
            return View(product);

        }






        [Authorize]
        public ActionResult MyOrders()
        {
            var userId = User.Identity.GetUserId();

            //var buy  = from car in db.AddToCarts
            //            join pro in db.Products
            //            on car.ProductId equals pro.ProductId
            //            where car.UserId == userId
            //            select car;


            var list = db.BuyProducts.Where(b => b.UserId == userId)/*.Select(a => a.Product)*/;

            return View(list.ToList());
        }
















        [Authorize]
        public ActionResult CancelBuy(int id)
        {
            var userId = User.Identity.GetUserId();
            var productId = db.BuyProducts.Where(a => a.ProductId == id);
            if (productId == null)
            {
                return HttpNotFound();
            }

            var cancel = db.BuyProducts.Where(a => a.UserId == userId && a.ProductId == id).SingleOrDefault();
            if (cancel != null)
            {
                db.BuyProducts.Remove(cancel);
                db.SaveChanges();
            }

            return RedirectToAction("MyOrders");
        }





        [Authorize]
        public ActionResult AddToCart()
        {

            var userId = User.Identity.GetUserId();
            var productId = (int)Session["ProductId"];


            //check if the user applied for this job  or  not
            var check = db.AddToCarts.Where(a => a.ProductId == productId && a.UserId == userId).ToList();
            if (check.Count < 1)
            {

                var product = new AddToCart();
                product.UserId = userId;
                product.ProductId = productId;
                product.AddToCartDate = DateTime.Now;

                db.AddToCarts.Add(product);
                db.SaveChanges();
            }

            return RedirectToAction("MyCart");

        }



        [Authorize]
        public ActionResult MyCart()
        {
            var userId = User.Identity.GetUserId();

            var list = db.AddToCarts.Where(a => a.UserId == userId).Select(a => a.Product);


            return View(list);
        }



        [Authorize]
        public ActionResult RemoveFromCart(int id)
        {
            var userId = User.Identity.GetUserId();
            var productId = db.AddToCarts.Find(id);


            var product = db.AddToCarts.Where(a => a.ProductId == id && a.UserId == userId).SingleOrDefault();

            db.AddToCarts.Remove(product);
            db.SaveChanges();
            return RedirectToAction("MyCart");

        }









        // GET: BuyProduct/Delete/5

        // POST: BuyProduct/Delete/5

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
