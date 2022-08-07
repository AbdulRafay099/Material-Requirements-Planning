using MRP.Data;
using MRP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using MySql.Data.MySqlClient.Memcached;
using Microsoft.EntityFrameworkCore;

namespace MRP.Controllers
{
    public class UserController : Controller 
    {


        private readonly MRPDbContext userDB;

        public UserController(MRPDbContext db) {
            userDB = db;
        }

        //GET - Register
        public IActionResult Register()
        {
            return View();
        }

        //POST - Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User obj) {

            if (ModelState.IsValid)
            {
                var objUser = userDB.User.Where(a => a.Email.Equals(obj.Email)).FirstOrDefault();
                if (objUser == null)
                {
                    userDB.User.Add(obj);
                    userDB.SaveChanges();
                    return RedirectToAction("Login");
                }
                else {
                    ViewBag.RegistrationError = "This email has already been registered.";
                }
            }

            return View();
        }

        //GET - Login
        public IActionResult Login()
        {
            return View();
        }

        // POST - Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(User obj) {
            IEnumerable<User> objList = userDB.User;
            IEnumerable<Order> orderList = userDB.Order;
            IEnumerable<Product> productList = userDB.Product;
            UserOrder userOrderObj = new UserOrder();
            userOrderObj.order = new List<Order>();
            userOrderObj.product = new List<Product>();
            var objUser = userDB.User.Where(a => a.Email.Equals(obj.Email) && a.Password.Equals(obj.Password)).FirstOrDefault();
            if (objUser != null)
            {
                HttpContext.Session.SetString("Name", objUser.UserName.ToString());
                HttpContext.Session.SetString("MobileNo", objUser.MobileNo.ToString());
                HttpContext.Session.SetString("Email", objUser.Email.ToString());
                TempData["Id"] = objUser.UserId;
                userOrderObj.user = objUser;

                foreach (var order in orderList)
                {
                    if (order.UserId == (int)TempData.Peek("Id"))
                    {
                        userOrderObj.order.Add(order);
                        TempData["ProductId"] = order.ProductId;
                        foreach (var product in productList)
                        {
                            if (product.ProductId == (int)TempData.Peek("ProductId"))
                            {
                                userOrderObj.product.Add(product);
                            }
                        }
                    }
                }
                
                return View("Profile", userOrderObj);

            }
            else
            {
                ViewBag.ErrorMessage = "Email or Password is not correct.";
                return View();
            }  
        }
        public IActionResult Profile(UserOrder objUserOrder) {

            IEnumerable<User> objList = userDB.User;
            IEnumerable<Order> orderList = userDB.Order;
            IEnumerable<Product> productList = userDB.Product;
            UserOrder userOrderObj = new UserOrder();
            userOrderObj.order = new List<Order>();
            userOrderObj.product = new List<Product>();

            foreach (var obj in objList) {
                if (obj.UserId == (int)TempData.Peek("Id")) {
                    userOrderObj.user = obj;
                }
            }

            foreach (var order in orderList) {
                if (order.UserId == (int)TempData.Peek("Id")) {
                    userOrderObj.order.Add(order);
                    TempData["ProductId"] = order.ProductId;
                    foreach (var product in productList)
                    {
                        if (product.ProductId == (int)TempData.Peek("ProductId"))
                        {
                            userOrderObj.product.Add(product);
                        }
                    }
                }
            }

            return View(userOrderObj);
        }

        [HttpPost]
        public IActionResult Profile(IFormFile ImageData)
        {
            IEnumerable<User> objList = userDB.User;
            IEnumerable<Order> orderList = userDB.Order;
            IEnumerable<Product> productList = userDB.Product;
            UserOrder userOrderObj = new UserOrder();
            userOrderObj.order = new List<Order>();
            userOrderObj.product = new List<Product>();

            foreach (var obj in objList)
            {
                if (obj.UserId == (int)TempData.Peek("Id"))
                {
                    userOrderObj.user = obj;
                }
            }

            foreach (var order in orderList)
            {
                if (order.UserId == (int)TempData.Peek("Id"))
                {
                    userOrderObj.order.Add(order);
                    TempData["ProductId"] = order.ProductId;
                    foreach (var product in productList)
                    {
                        if (product.ProductId == (int)TempData.Peek("ProductId"))
                        {
                            userOrderObj.product.Add(product);
                        }
                    }
                }
            }
            if (ImageData != null)

            {
                if (ImageData.Length > 0)

                //Convert Image to byte and save to database

                {

                    byte[] p1 = null;
                    using (var fs1 = ImageData.OpenReadStream())
                    using (var ms1 = new MemoryStream())
                    {
                        fs1.CopyTo(ms1);
                        p1 = ms1.ToArray();
                    }
                    userOrderObj.user.Image = p1;

                }
            }

            userDB.User.Update(userOrderObj.user);
            userDB.SaveChanges();


            return View("Profile",userOrderObj);
        }

        public ActionResult GetImage(int id)
        {
            byte[] cover = GetImageFromDataBase(id);
            if (cover != null)
            {
                return File(cover, "image/jpg");
            }
            else
            {
                return null;
            }
        }
        public byte[] GetImageFromDataBase(int id)
        {
            var q = from temp in userDB.User where temp.UserId == id select temp.Image;
            byte[] cover = q.First();
            return cover;
        }

        public ActionResult DeletePicture(int? id) {
            IEnumerable<User> objList = userDB.User;
            IEnumerable<Order> orderList = userDB.Order;
            IEnumerable<Product> productList = userDB.Product;
            UserOrder userOrderObj = new UserOrder();
            userOrderObj.order = new List<Order>();
            userOrderObj.product = new List<Product>();

            foreach (var obj in objList)
            {
                if (obj.UserId == id)
                {
                    userOrderObj.user = obj;
                }
            }

            foreach (var order in orderList)
            {
                if (order.UserId == (int)TempData.Peek("Id"))
                {
                    userOrderObj.order.Add(order);
                    TempData["ProductId"] = order.ProductId;
                    foreach (var product in productList)
                    {
                        if (product.ProductId == (int)TempData.Peek("ProductId"))
                        {
                            userOrderObj.product.Add(product);
                        }
                    }
                }
            }

            userOrderObj.user.Image = null;
            userDB.User.Update(userOrderObj.user);
            userDB.SaveChanges();

            return View("Profile", userOrderObj);
        }

        // GET - UPDATE
        public IActionResult Update(int? id) {

            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = userDB.User.Find(id);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(User obj) {

            if (ModelState.IsValid)
            {
                var q = from temp in userDB.User where temp.UserId == (int)TempData.Peek("Id") select temp.Image;
                var admin = from temp in userDB.User where temp.UserId == (int)TempData.Peek("Id") select temp.Admin;
                byte[] image = q.First();
                obj.Image = image;
                obj.Admin = admin.First();
                userDB.User.Update(obj);
                userDB.SaveChanges();
                UserOrder userOrderObj = new UserOrder();
                userOrderObj.order = new List<Order>();
                userOrderObj.product = new List<Product>();
                userOrderObj.user = obj;

                IEnumerable<Order> orderList = userDB.Order;
                IEnumerable<Product> productList = userDB.Product;

                foreach (var order in orderList)
                {
                    if (order.UserId == (int)TempData.Peek("Id"))
                    {
                        userOrderObj.order.Add(order);
                        TempData["ProductId"] = order.ProductId;
                        foreach (var product in productList)
                        {
                            if (product.ProductId == (int)TempData.Peek("ProductId"))
                            {
                                userOrderObj.product.Add(product);
                            }
                        }
                    }
                }

                return View("Profile", userOrderObj);
            }
            return View(obj);
        }

    }
}
