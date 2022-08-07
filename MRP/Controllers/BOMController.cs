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
    public class BOMController : Controller
    {
        private readonly MRPDbContext bomDB;

        public BOMController(MRPDbContext db)
        {
            bomDB = db;
        }
        //GET - HOME
        public IActionResult Admin()
        {
            IEnumerable<BOM> bomList = bomDB.BOM;
            return View(bomList);
        }

        //POST - CREATE
        [HttpPost]
        public IActionResult Admin(IFormFile ImageData, int? id)
        {
            IEnumerable<BOM> bomList = bomDB.BOM;
            foreach (var obj in bomList)
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

                    bomDB.BOM.Update(obj);
                }
            }
            bomDB.SaveChanges();

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
            var q = from temp in bomDB.BOM where temp.ItemId == id select temp.ItemImage;
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
        public IActionResult Create(BOM obj)
        {
            if (ModelState.IsValid)
            {
                bomDB.BOM.Add(obj);
                bomDB.SaveChanges();
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

            var obj = bomDB.BOM.Find(id);

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
        public IActionResult Edit(BOM obj)
        {
            if (ModelState.IsValid)
            {
                var q = from temp in bomDB.BOM where temp.ItemId == (int)TempData.Peek("EditId") select temp.ItemImage;
                byte[] image = q.First();
                obj.ItemImage = image;
                bomDB.BOM.Update(obj);
                bomDB.SaveChanges();
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

            var obj = bomDB.BOM.Find(id);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        // POST - Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(BOM obj)
        {
            bomDB.BOM.Remove(obj);
            bomDB.SaveChanges();
            return RedirectToAction("Admin");
        }
    }
}
