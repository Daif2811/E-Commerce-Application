using BuyOnline.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BuyOnline.Controllers
{
    public class HomeController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var list = db.Categories.ToList();
            return View(list);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }





        public ActionResult Search()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Search(string SearchName)
        {
            var result = db.Products.Where(a =>
            a.ProductName.Contains(SearchName) ||
            a.ProductDescription.Contains(SearchName) ||
            a.Category.CategoryName.Contains(SearchName) ||
            a.Category.CategoryDescription.Contains(SearchName)).ToList();

            return View(result);

        }















    }
}