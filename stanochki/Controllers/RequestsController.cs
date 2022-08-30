using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using stanochki.Models;
using Word = Microsoft.Office.Interop.Word;

namespace stanochki.Controllers
{
    public class RequestsController : Controller
    {
        private DataBEntities db = new DataBEntities();

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        // GET: Requests
        public ActionResult Index()
        {
            
                var id = (Int32)Session["UserId"];
                var requests = from s in db.Requests select s;
                requests = requests.Where(s => s.client.Equals(id));
                return View(requests.ToList());
            
        }

        // GET: Requests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requests requests = db.Requests.Find(id);
            if (requests == null)
            {
                return HttpNotFound();
            }
            return View(requests);
        }

        // GET: Requests/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session["NowIdPrice"] = id;
            //var prices = from s in db.Prices select s;
            Prices prices = db.Prices.Find(id);
            //users = from s in db.Users select s;
            Int32 user_id = (Int32)@Session["UserId"];
            Users users = db.Users.First(s => s.id_user.Equals(user_id)); //Find(user_id);
            if (prices == null)
            {
                return HttpNotFound();
            }
            ViewBag.client_name = users.name;
            ViewBag.client_sur = users.surn;
            ViewBag.client_middle = users.middlen;
            ViewBag.client_address = users.address;
            ViewBag.machine = prices.machine;
            ViewBag.price = prices.price;
            return View();
        }
        
                
        
        // POST: Requests/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_req,id_price,client,status,date")] Requests requests)
        {
            if (ModelState.IsValid)
            {
            Int32 price_id = (Int32)Session["NowIdPrice"];
            Prices prices = db.Prices.Find(price_id);
            Int32 user_id = (Int32)@Session["UserId"];
            Users users = db.Users.First(s => s.id_user.Equals(user_id));
            
            requests.id_price = price_id;
            requests.client = user_id;
            db.Requests.Add(requests);
            db.SaveChanges();
            
            string File = @"C:\Users\macbook\Desktop\stanochki\stanochki\repair.docx";
            var number = requests.id_req;
            var machine = prices.machine;
            var repair = prices.repair; 
            var address = users.address;
            var itog = prices.price;
            var buyer = users.name +" "+ users.middlen + " " + users.surn;
            var chislo = DateTime.Today.Date.ToShortDateString();
            var documentWord = new Word.Application();
            documentWord.Visible = false;
            try
            {

                var wordDoc = documentWord.Documents.Open(File); //Open

                ChangeInt("{number}", number, wordDoc);
                Change("{chislo}", chislo, wordDoc);
                Change("{machine}", machine, wordDoc);
                Change("{repair}", repair, wordDoc);
                Change("{buyer}", buyer, wordDoc);
                Change("{address}", address, wordDoc);
                ChangeInt("{itog}", itog, wordDoc);
                    documentWord.Visible = true;
                   }
            catch
            {}
            }
            return RedirectToAction("Index");

        }
        private void Change(string change, string txt, Word.Document wordDoc)  //Замена строковых
        {
            var range = wordDoc.Content;
            range.Find.ClearFormatting();
            range.Find.Execute(FindText: change, ReplaceWith: txt);
        }
        private void ChangeInt(string change, int txt, Word.Document wordDoc)      //Замена на число
        {
            var range = wordDoc.Content;
            range.Find.ClearFormatting();
            range.Find.Execute(FindText: change, ReplaceWith: txt);
        }


        // GET: Requests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requests requests = db.Requests.Find(id);
            if (requests == null)
            {
                return HttpNotFound();
            }
            ViewBag.client = new SelectList(db.Entry, "id", "pass", requests.client);
            ViewBag.status = new SelectList(db.Entry, "id", "pass", requests.status);
            ViewBag.id_price = new SelectList(db.Prices, "id_price", "machine", requests.id_price);
            return View(requests);
        }

        // POST: Requests/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_req,id_price,client,status,date")] Requests requests)
        {
            if (ModelState.IsValid)
            {
                db.Entry(requests).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.client = new SelectList(db.Entry, "id", "pass", requests.client);
            ViewBag.status = new SelectList(db.Entry, "id", "pass", requests.status);
            ViewBag.id_price = new SelectList(db.Prices, "id_price", "machine", requests.id_price);
            return View(requests);
        }

        // GET: Requests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requests requests = db.Requests.Find(id);
            if (requests == null)
            {
                return HttpNotFound();
            }
            return View(requests);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Requests requests = db.Requests.Find(id);
            db.Requests.Remove(requests);
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
