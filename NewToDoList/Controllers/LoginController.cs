using NewToDoList.code;
using NewToDoList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NewToDoList.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginModel model)
        {
            

            if (ModelState.IsValid && Membership.ValidateUser(model.id, MD5Hash.hash(model.password) ))
            {
                //SessionHelper.SetSession(new UserSession() { id = Convert.ToInt16(model.id)});
                FormsAuthentication.SetAuthCookie(model.id, model.remember);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Email hoặc password không đúng");
            }
            return View(model);
        }
        public ActionResult Logout()
        {
            
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }

    }
}