using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MainApp.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult CreateAccount(int? userId)
        {
            return View();
        }
    }
}