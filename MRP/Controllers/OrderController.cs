using MRP.Data;
using MRP.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MRP.Controllers
{
    public class OrderController : Controller
    {
        private readonly MRPDbContext orderDB;

        public OrderController(MRPDbContext db)
        {
            orderDB = db;
        }

        // GET - PlaceOrder
        public IActionResult PlaceOrder(int? id)
        {

            if (id == null || id == 0)
            {
                return NotFound();
            }

            IEnumerable<Order> orderList = orderDB.Order;

            foreach (var obj in orderList) {
                if (obj.ProductId == id && obj.UserId == (int)TempData.Peek("Id")) {
                    TempData["OrderError"] = "This order has already been placed";
                    return RedirectToAction("Customer", "Product");
                }
            }
            

            var obj1 = orderDB.Product.Find(id);
            var obj2 = orderDB.User.Find(TempData.Peek("Id"));

            if (obj1 == null || obj2 == null)
            {
                return NotFound();
            }

            Order orderObj = new Order();
            orderObj.UserId = obj2.UserId;
            orderObj.ProductId = obj1.ProductId;
            orderObj.placedDate = DateTime.Today;
            orderObj.deliveryDate = DateTime.Today.AddDays(30);

            orderDB.Order.Add(orderObj);
            orderDB.SaveChanges();

            return RedirectToAction("Customer", "Product");
        }

        public IActionResult deleteOrder(int? id) {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var orderObj = orderDB.Order.Find(id);

            orderDB.Order.Remove(orderObj);
            orderDB.SaveChanges();

            return RedirectToAction("Profile", "User");
        }
    }
}
