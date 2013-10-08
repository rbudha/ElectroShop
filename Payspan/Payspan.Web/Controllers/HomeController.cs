using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Payspan.Web.Models;

namespace Payspan.Web.Controllers
{
    public class HomeController : Controller
    {
        PayspanShopDB _db = new PayspanShopDB();

        public ActionResult Index()
        {
            List<Product> products = _db.Products.ToList();

            return View(products);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Detail(int ID)
        {
            Product product = _db.Products.Where(p => p.ID == ID).FirstOrDefault();
            return View(product);
        }
    }
}
