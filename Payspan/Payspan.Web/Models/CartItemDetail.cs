using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Payspan.Web.Models
{
    public class CartItemDetail
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public string Name { get; set; }
    }
}