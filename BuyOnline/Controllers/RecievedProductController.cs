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
    public class RecievedProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: RecievedProduct
        public ActionResult Index()
        {
            var recievedProducts = db.RecievedProducts.Include(r => r.BuyProduct).Include(r => r.User);
            return View(recievedProducts.ToList());
        }




        // index for customers
        public ActionResult RecievedForCustomer()
        {
            var userId = User.Identity.GetUserId();

            var products = db.RecievedProducts.Where(a => a.BuyProduct.UserId == userId);
            return View(products.ToList());
        }


        // index for sellers
        public ActionResult RecievedForSeller()
        {
            var userId = User.Identity.GetUserId();

            var products = db.RecievedProducts.Where(a => a.BuyProduct.Product.UserId == userId);
            return View(products.ToList());
        }


        // GET: RecievedProduct/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecievedProduct recievedProduct = db.RecievedProducts.Find(id);
            if (recievedProduct == null)
            {
                return HttpNotFound();
            }
            return View(recievedProduct);
        }

        // GET: RecievedProduct/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = db.BuyProducts.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            Session["BuyProductId"] = id;
            return View();
        }

        // POST: RecievedProduct/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Recieved,RecievedDate,UserId,BuyProductId")] RecievedProduct recievedProduct)
        {
            var userId = User.Identity.GetUserId();

            recievedProduct.UserId = userId;
            recievedProduct.RecievedDate = DateTime.Now;
            recievedProduct.BuyProductId = (int)Session["BuyProductId"];

            if (ModelState.IsValid)
            {
                var buy = db.BuyProducts.Where(b => b.Id == recievedProduct.BuyProductId).SingleOrDefault();

                if (recievedProduct.Recieved == true)
                {
                    db.RecievedProducts.Add(recievedProduct);
                    db.SaveChanges();

                    buy.Pay = true;
                    db.SaveChanges();
                    return RedirectToAction("RecievedForSeller");
                }
                else
                {
                    db.BuyProducts.Remove(buy);
                }

            }

            ViewBag.BuyProductId = new SelectList(db.BuyProducts, "Id", "Address", recievedProduct.BuyProductId);
            //ViewBag.UserId = new SelectList(db.ApplicationUsers, "Id", "UserType", recievedProduct.UserId);
            return View(recievedProduct);
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
