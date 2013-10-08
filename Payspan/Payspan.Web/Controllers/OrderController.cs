using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Payspan.Web.Models;

namespace Payspan.Web.Controllers
{
    public class OrderController : Controller
    {
        PayspanShopDB _db = new PayspanShopDB();

        public ActionResult Index()
        {
            List<Order> order = _db.Orders.ToList();

            return View(order);
        }

        public ActionResult Detail(int id)
        {
            var orderDetail = (from o in _db.Orders
                               
                               where o.ID == id
                               select o.OrderItems).ToList();
            return View(orderDetail);
        }

    }
}
