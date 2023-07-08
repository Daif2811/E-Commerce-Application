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
using System.IO;

namespace BuyOnline.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Product
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category).Include(p => p.User);
            return View(products.ToList());
        }

        // GET: Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Product/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName");
            //ViewBag.UserId = new SelectList(db.Users, "Id", "UserType");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,ProductName,ProductDescription,ProductImage,ProductQuantity,ProductPrice,CategoryId")] Product product , HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                string path = Path.Combine(Server.MapPath("~/Images/Products"), upload.FileName);
                upload.SaveAs(path);
                product.ProductImage = upload.FileName;
                product.UserId = User.Identity.GetUserId();

                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);
           
            return View(product);
        }

        // GET: Product/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);
            //ViewBag.UserId = new SelectList(db.Users, "Id", "UserType", product.UserId);
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,ProductName,ProductDescription,ProductImage,ProductQuantity,ProductPrice,CategoryId")] Product product , HttpPostedFileBase upload)
        {

            var userId = User.Identity.GetUserId();
            product.UserId = userId;
            if (ModelState.IsValid)
            {


                string oldPath = Path.Combine(Server.MapPath("~/Images/Products"), product.ProductImage);

                if (upload != null)
                {
                    System.IO.File.Delete(oldPath);
                    string path = Path.Combine(Server.MapPath("~/Images/Products"), upload.FileName);
                    upload.SaveAs(path);
                    product.ProductImage = upload.FileName;
                } 



                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);
            //ViewBag.UserId = new SelectList(db.Users, "Id", "UserType", product.UserId);
            return View(product);
        }


        // GET: Product/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }






        // All product by Seller
        // =====================================================
        [Authorize]
        public ActionResult AllProductsBySeller()
        {
            var userId = User.Identity.GetUserId();
            var products = db.Products.Where(a => a.UserId == userId).ToList();


            return View(products);
        }



        // All sold product by Seller
        [Authorize]
        public ActionResult PendingOrders()
        {
            var userId = User.Identity.GetUserId();

            var products = from x in db.Products
                           join y in db.BuyProducts
                           on x.ProductId equals y.ProductId
                           where x.UserId == userId
                           select y;


            var groop = from pro in products
                        group pro by pro.Product.ProductName into gro
                        select new BuyViewModel
                        {
                            ProductName = gro.Key,
                            Persons = gro


                        };

            return View(groop.ToList());
        }





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
