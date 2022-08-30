using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using stanochki.Models;

namespace stanochki.Controllers
{
    public class PricesController : Controller
    {
        private DataBEntities db = new DataBEntities();

        // GET: Prices
        public ActionResult Index(string sortOrder, string searchbymachine, string searchbyrepair)
        {
            ViewBag.machineSortParm = String.IsNullOrEmpty(sortOrder) ? "machine_desc" : "";
            ViewBag.repairSortParm = sortOrder == "repair" ? "repair_desc" : "repair";
            ViewBag.priceSortParm = sortOrder == "price" ? "price_desc" : "price";
            var prices = from s in db.Prices select s;
            if (!String.IsNullOrEmpty(searchbymachine))
            {
                prices = prices.Where(s => s.machine.Contains(searchbymachine));
            }
            if (!String.IsNullOrEmpty(searchbyrepair))
            {
                prices = prices.Where(s => s.repair.Contains(searchbyrepair));
            }

            switch (sortOrder)
            {
                case "machine_desc":
                    prices = prices.OrderByDescending(s => s.machine);
                    break;
                case "repair":
                    prices = prices.OrderBy(s => s.repair);
                    break;
                case "repair_desc":
                    prices = prices.OrderByDescending(s => s.repair);
                    break;
                case "price":
                    prices = prices.OrderBy(s => s.price);
                    break;
                case "price_desc":
                    prices = prices.OrderByDescending(s => s.price);
                    break;
                default:
                    prices = prices.OrderBy(s => s.id_price);
                    break;
            }
            return View(prices.ToList());
        }

        // GET: Prices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prices prices = db.Prices.Find(id);
            if (prices == null)
            {
                return HttpNotFound();
            }
            return View(prices);
        }

        // GET: Prices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Prices/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_price,machine,repair,price,descript,photo")] Prices prices, HttpPostedFileBase upload)
        {

            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        prices.photo = reader.ReadBytes(upload.ContentLength);
                    }
                }
                db.Prices.Add(prices);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.repair = new SelectList(db.Prices, "id_price,machine,repair,price,descript,photo");
            return View(prices);
        }

        // GET: Prices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prices prices = db.Prices.Find(id);
            if (prices == null)
            {
                return HttpNotFound();
            }
            return View(prices);
        }

        // POST: Prices/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_price,machine,repair,price,descript,photo")] Prices prices, HttpPostedFileBase upload)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(prices).State = EntityState.Modified;
                    if (upload != null && upload.ContentLength > 0)
                    {
                        using (var reader = new System.IO.BinaryReader(upload.InputStream))
                        {
                            prices.photo = reader.ReadBytes(upload.ContentLength);
                        }
                        db.SaveChanges();
                    }

                    else
                    {
                        db.Entry(prices).Property(m => m.photo).IsModified = false;
                        db.SaveChanges();
                    }

                    return RedirectToAction("Index");
                }

                return View(prices);
            }
            catch (Exception)
            {
                return View(prices);
            }

        }

        // GET: Prices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prices prices = db.Prices.Find(id);
            if (prices == null)
            {
                return HttpNotFound();
            }
            return View(prices);
        }

        // POST: Prices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Prices prices = db.Prices.Find(id);
            db.Prices.Remove(prices);
            db.SaveChanges();
            return RedirectToAction("Index");
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
