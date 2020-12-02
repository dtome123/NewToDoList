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
    public class AccountController : Controller
    {
        // GET: Account
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid && Membership.ValidateUser(model.email, model.password))
            {
                SessionHelper.SetSession(new UserSession() { email = model.email });
                FormsAuthentication.SetAuthCookie(model.email, model.remember);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Email hoặc password không đúng");
            }
            return View(model);
        }
    }
}