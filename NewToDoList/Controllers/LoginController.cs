﻿using NewToDoList.code;
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
            

            if (ModelState.IsValid && Membership.ValidateUser(model.id, model.password))
            {
                SessionHelper.SetSession(new UserSession() { id = model.id });
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
            return RedirectToAction("Index", "Login");
        }

    }
}