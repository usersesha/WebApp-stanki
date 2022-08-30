using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using stanochki.Models;

namespace stanochki.Controllers
{
    public class HomeController : Controller
    {
        DataBEntities db = new DataBEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult workerG()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Подробнее о нас.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Связь с нами.";

            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Reg()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authorize(LoginModel model)
        {
            using (DataBEntities db = new DataBEntities())
            {
                var userDetais = db.Entry.FirstOrDefault(x => x.login == model.Login && x.pass == model.Pass);

                if (userDetais != null)
                {
                    var userDetails = db.Entry.Single(x => x.login == model.Login && x.pass == model.Pass);
                    Session["UserLogin"] = userDetails.login;
                    Session["UserId"] = userDetails.id;
                    Session["Rights"] = userDetails.role; // Права пользователя
                    var foName = db.Users.Single(x => x.id_user == userDetails.id);
                    Session["UserName"] = foName.name;
                    
                    if (Session["Rights"].Equals("user"))
                    {
                        ViewBag.Message1 = "Добро пожаловать, " + Session["UserName"];
                        return View("~/Views/Home/Index.cshtml");
                    }
                    else
                    {
                        ViewBag.Message1 = "Хорошей смены, " + Session["UserName"];
                        return View("~/Views/Home/workerG.cshtml");
                    }
                    

                }
                else
                {
                    TempData["CustomError"] = "Неверные данные";
                    ModelState.AddModelError(string.Empty, TempData["CustomError"].ToString());
                    //return RedirectToAction("Login", "Home");
                    return View("~/Views/Home/Login.cshtml");
                }
            }
        }


        public ActionResult LogOut()        //Выход из системы
        {
            Session["Rights"] = null;
            Session.Abandon();
            return View("~/Views/Home/Login.cshtml");
        }
        [HttpPost]
        public ActionResult Register(RegisterModel model,Models.Users newuser)
        {
            if (ModelState.IsValid)
            {
                using (DataBEntities db = new DataBEntities())
                {
                    var regInfo = this.db.Insert_User(model.Login, model.name, model.surn, model.middlen, model.address, model.Pass, model.number).ToList();
                    // Verification.    
                    if (regInfo != null && Convert.ToInt32(regInfo[0]) != -1)
                    {
                        //SAVING CHANGES TO DATABASE
                        db.SaveChanges();
                        //    return RedirectToAction("Index", "Home");
                        return RedirectToAction("Login", "Home");

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Логин занят. Попробуйте другой.");
                    }
                   
                }
                ModelState.Clear();
                //ViewBag.ViewBag.InsertionResult = newuser.name + " был зарегестрирован.";
                ViewBag.Message = newuser.name + " зарегестрирован.";
            }
            ViewBag.Message = newuser.name + " зарегестрирован.";
            return View("~/Views/Home/Reg.cshtml");
        }

    }
}