using System;
using System.Linq;
using System.Web.Mvc;
using MainApp.Enums;
using MainApp.Models;
using MainApp.Models.AccountModel;

namespace MainApp.Controllers
{
    public class AccountController : Controller
    {
        [HttpPost]
        public ActionResult CreateAccount(int? userId, int? currency)
        {
            if (userId == null) return Json(new {StatusCode = 404, Message = "Не найден пользователь"}); 
            if (currency == null) return Json(new { StatusCode = 404, Message = "Не найдена валюта" }); 
            using (var db = new ConnectionContext())
            {
                var user = db.Users.Find(userId);
                db.Accounts.Add(new Account
                {
                    AccountName = user.FullName,
                    Currency = (CurrencyId)currency,
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

                return Json(new {StatusCode = 201, Message = "Счет успешно создан."});
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