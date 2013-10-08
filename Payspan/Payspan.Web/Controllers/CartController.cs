using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Payspan.Web.Models;
namespace Payspan.Web.Controllers
{
    public class CartController : Controller
    {
        List<CartItem> cartItems;
        PayspanShopDB _db = new PayspanShopDB();

        public CartController()
        {
            if (System.Web.HttpContext.Current.Session["cartItmes"] != null)
            {
                cartItems = (List<CartItem>)System.Web.HttpContext.Current.Session["cartItmes"];
            }
            else
            {
                cartItems = new List<CartItem>();
            }
        }

        public ActionResult Index()
        {
            List<CartItemDetail> cart = GetCartItemDetails();
            Customer customer = new Customer();
            ViewBag.Customer = customer;
            return View(cart);
        }

        public RedirectToRouteResult Add(int id, int quantity)
        {
            CartItem newItem;

            CartItem item = (from c in cartItems
                       where c.ProductID == id
                       select c).FirstOrDefault();
            
            if (item != null)
            {
                newItem = new CartItem { ProductID = id, Quantity = quantity + item.Quantity };
                cartItems.Remove(item);
            }
            else
            {

                newItem = new CartItem { ProductID = id, Quantity = quantity }; 
            }

            cartItems.Add(newItem);
            System.Web.HttpContext.Current.Session["cartItmes"] = cartItems;
            return RedirectToAction("Index");
        }

        public RedirectToRouteResult Checkout(string firstName, string lastName, string emailAddress)
        {
            List<CartItemDetail> cart = GetCartItemDetails();

            decimal? totalAmount = (from c in cart
                                   select c.UnitPrice * c.Quantity).Sum();
            Customer customer = new Customer
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = emailAddress,
                JoinDate = DateTime.Now
            };

            _db.Customers.AddObject(customer);
            int customerID = _db.SaveChanges();

            Order order = new Order
            {
                CustomerID = customerID,
                OrderDate = DateTime.Now,
                Status = "New",
                TotalAmount = (decimal)totalAmount
            };

            _db.Orders.AddObject(order);
            int orderID = _db.SaveChanges();

            

            List<OrderItem> orderItems = (from c in cart
                                         select new OrderItem
                                         {
                                             OrderID = orderID,
                                             ProductID = c.ProductID,
                                             Quantity = c.Quantity,
                                             UnitPrice = (decimal)c.UnitPrice,
                                             TotalAmount = c.Quantity * (decimal)c.UnitPrice
                                         }).ToList();
            foreach (OrderItem item in orderItems)
            {
                _db.OrderItems.AddObject(item);
                _db.SaveChanges();
            }
            System.Web.HttpContext.Current.Session["cartItmes"] = null;
            return RedirectToAction("Index", "Order");
        }

        private List<CartItemDetail> GetCartItemDetails()
        {
            List<Product> products = (from p in _db.Products
                                      select p).ToList(); ;

            List<CartItemDetail> cart = (from p in products
                                         join c in cartItems on p.ID equals c.ProductID
                                         select new CartItemDetail
                                         {
                                             ProductID = p.ID,
                                             Name = p.Name,
                                             UnitPrice = p.UnitPrice,
                                             Quantity = c.Quantity
                                         }).ToList();
            return cart;
        }

      
    }
}
