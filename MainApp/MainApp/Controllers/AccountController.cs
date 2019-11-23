using System;
using System.Linq;
using System.Web.Mvc;
using MainApp.Enums;
using MainApp.Models;
using MainApp.Models.AccountModel;
using MainApp.Models.UserModel;
using MainApp.Providers;
using MainApp.ViewModels;

namespace MainApp.Controllers
{
    [CustomAuthorize(Roles = "admin, user")]

    public class AccountController : Controller
    {
        public ActionResult Index(string userName)
        {
            User UserName;
            using (var db = new ConnectionContext())
            {
                UserName = db.Users.FirstOrDefault(x => x.Login == userName);
                if (UserName == null)
                {
                    return HttpNotFound("Не удалось найти страницу");
                }

                if (User.IsInRole("admin"))
                {
                    var accounts = from a in db.Accounts
                        select new AccountViewModel
                        {
                            AccountName = a.AccountName,
                            AccountNumber = a.AccountNumber,
                            AccountOpenDate = a.AccountOpenDate,
                            Balance = a.Balance,
                            Currency = "KGS"
                        };
                    return View(accounts.ToList());
                }
                else
                {
                    var accounts = from a in db.Accounts
                        where a.UserId == UserName.UserId
                        select new AccountViewModel
                        {
                            AccountName = a.AccountName,
                            AccountNumber = a.AccountNumber,
                            AccountOpenDate = a.AccountOpenDate,
                            Balance = a.Balance,
                            Currency = "KGS"
                        };
                    return View(accounts.ToList());
                }
            }


        }

        [HttpGet]
        public ActionResult CreateAccount()
        {

            return View();
        }

        [HttpPost]
        public ActionResult CreateAccount(CreateAccountView createAccount)
        {
            using (var db = new ConnectionContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Login == createAccount.userName);
                db.Accounts.Add(new Account
                {
                    AccountName = user.FullName,
                    Currency = CurrencyId.KGS,
                    AccountOpenDate = DateTime.Now,
                    UserId = user.UserId
                });

                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    return Json(new {StatusCode = 406, Message = "Не удалось созать счет."});
                }

                return RedirectToAction("Index", new { createAccount.userName});
            }
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Refill(int? accountNum, decimal refillSum)
        {
            if (accountNum == null) return Json(new {StatusCode = 404, Message = "Счет не найден"});
            if (refillSum <= 0) return Json(new {StatusCode = 405, Message = "Сумма не может быть меньше или равна нулю"});
            using (var db = new ConnectionContext())
            {
                var accountNumber = db.Accounts.Find(accountNum);
                accountNumber.Balance = refillSum;
                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    return Json(new {StausCode = 304, Message = "Не удалось пополнить счет"});
                }
            }
            return Json(new {StatusCode = 200, Message = "Счет успешно пополнен на сумму " + refillSum});
        }

    }
}