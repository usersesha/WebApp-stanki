using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using stanochki.Models;

namespace stanochki.Controllers
{
    public class OfficesController : Controller
    {
        private DataBEntities db = new DataBEntities();

        // GET: Offices
        public ActionResult Index()
        {
            return View(db.Offices.ToList());
        }

        // GET: Offices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offices offices = db.Offices.Find(id);
            if (offices == null)
            {
                return HttpNotFound();
            }
            return View(offices);
        }

        // GET: Offices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Offices/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_office,region,address,schedule,number")] Offices offices)
        {
            if (ModelState.IsValid)
            {
                db.Offices.Add(offices);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(offices);
        }

        // GET: Offices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offices offices = db.Offices.Find(id);
            if (offices == null)
            {
                return HttpNotFound();
            }
            return View(offices);
        }

        // POST: Offices/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_office,region,address,schedule,number")] Offices offices)
        {
            if (ModelState.IsValid)
            {
                db.Entry(offices).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(offices);
        }

        // GET: Offices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offices offices = db.Offices.Find(id);
            if (offices == null)
            {
                return HttpNotFound();
            }
            return View(offices);
        }

        // POST: Offices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Offices offices = db.Offices.Find(id);
            db.Offices.Remove(offices);
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
