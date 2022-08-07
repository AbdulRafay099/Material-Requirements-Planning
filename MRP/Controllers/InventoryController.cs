using MRP.Data;
using MRP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MRP.Controllers
{
    public class InventoryController : Controller
    {
        private readonly MRPDbContext inventoryDB;

        public InventoryController(MRPDbContext db)
        {
            inventoryDB = db;
        }
        //GET - HOME
        public IActionResult Admin()
        {
            IEnumerable<Inventory> inventoryList = inventoryDB.Inventory;
            return View(inventoryList);
        }

        //POST - CREATE
        [HttpPost]
        public IActionResult Admin(IFormFile ImageData, int? id)
        {
            IEnumerable<Inventory> inventoryList = inventoryDB.Inventory;
            foreach (var obj in inventoryList)
            {
                if (obj.ItemId == id)
                {
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
                            obj.ItemImage = p1;

                        }
                    }

                    inventoryDB.Inventory.Update(obj);
                }
            }
            inventoryDB.SaveChanges();

            return RedirectToAction("Admin");
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
            var q = from temp in inventoryDB.Inventory where temp.ItemId == id select temp.ItemImage;
            byte[] cover = q.First();
            return cover;
        }

        //GET - CREATE
        public IActionResult Create()
        {
            return View();
        }

        // POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Inventory obj)
        {
            if (ModelState.IsValid)
            {
                inventoryDB.Inventory.Add(obj);
                inventoryDB.SaveChanges();
                return RedirectToAction("Admin");
            }
            return View(obj);
        }

        // GET - POST
        public IActionResult Edit(int? id)
        {

            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = inventoryDB.Inventory.Find(id);

            if (obj == null)
            {
                return NotFound();
            }

            TempData["EditId"] = id;

            return View(obj);
        }

        // POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Inventory obj)
        {
            if (ModelState.IsValid)
            {
                var q = from temp in inventoryDB.Inventory where temp.ItemId == (int)TempData.Peek("EditId") select temp.ItemImage;
                byte[] image = q.First();
                obj.ItemImage = image;
                inventoryDB.Inventory.Update(obj);
                inventoryDB.SaveChanges();
                return RedirectToAction("Admin");
            }
            return View(obj);
        }

        // GET - Delete
        public IActionResult Delete(int? id)
        {

            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = inventoryDB.Inventory.Find(id);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        // POST - Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(Inventory obj)
        {
            inventoryDB.Inventory.Remove(obj);
            inventoryDB.SaveChanges();
            return RedirectToAction("Admin");
        }
    }
}
