using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MainApp.Enums;
using MainApp.Models;
using MainApp.Models.AccountModel;
using MainApp.Models.Histories;
using MainApp.Models.PaymentModel;
using MainApp.Providers;
using MainApp.ViewModels;

namespace MainApp.Controllers
{
    [CustomAuthorize(Roles = "admin, user")]
    public class PaymentsController : Controller
    {
        // GET: Payments
        public ActionResult Index()
        {
            using (var db = new ConnectionContext())
            {
                var categories = db.UtilityCategories.ToList();
                return View(categories);
            }
        }

        public ActionResult GetUtility(int? categoryId)
        {
            using (var db = new ConnectionContext())
            {
                var utilities = db.Utilities.Where(x => x.UtilityCategoryId == categoryId).ToList();
                if (utilities.Any() == false)
                {
                    return Json(new { StatusCode = 404, Message = "Информация не найдена" }, JsonRequestBehavior.AllowGet);
                }
                return Json(utilities, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Pay(int? id, string userName)
        {
            IQueryable<Account> accounts;
            using (var db = new ConnectionContext())
            {
                var User = db.Users.FirstOrDefault(u => u.Login == userName);
                if (User == null)
                {
                    return HttpNotFound("Не удалось найти страницу");
                }

                if (id == null)
                {
                    return HttpNotFound("Не удалось найти страницу");
                }

                var utility = from u in db.Utilities
                              join c in db.UtilityCategories on u.UtilityCategoryId equals c.UtilityCategoryId
                              where u.UtilityId == id
                              select new PaymentViewModel
                              {
                                  UtilityCategoryName = c.UtilityCategoryName,
                                  UtilityId = u.UtilityId,
                                  UtilityName = u.UtilityName,
                                  UtilityImagePath = u.UtilityImagePath,
                                  UtilityAccountNumber = u.UtilityAccountNumber
                              };
                accounts = db.Accounts.Where(a => a.UserId == User.UserId);
                ViewBag.AccountFrom = new SelectList(accounts.Select(x => x.AccountNumber).ToList(), "AccountNumber");

                return View(utility.ToList().First());

            }
        }

        [HttpPost]
        public ActionResult Pay(int accountFrom, string personalAccount, decimal paySum, int? utilityId)
        {
            using (var db = new ConnectionContext())
            {
                var utility = db.Utilities.FirstOrDefault(x => x.UtilityId == utilityId);
                var account = db.Accounts.FirstOrDefault(x => x.AccountNumber == accountFrom);
                var accountTo = db.Accounts.FirstOrDefault(x => x.AccountNumber == utility.UtilityAccountNumber);

                if (account == null) return Json(new { StatusCode = 404, Message = "Не найден счет." });

                if (paySum <= 0) return Json(new { StatusCode = 405, Message = "Сумма не может быть меньше или равна нулю." });

                if (account.Balance < paySum) return Json(new { StatusCode = 405, Message = "Недостаточно средств на счете" });

                if (utility == null) return Json(new { StatusCode = 404, Message = "Не найдена услуга" });

                var newPayment = new Payment
                {
                    PaymentComission = 0,
                    UtilityId = utility.UtilityId,
                    PaymentComment = "Оплата за: " + utility.UtilityName,
                    PaymentDate = DateTime.Now,
                    PaymentSum = paySum,
                    UserId = account.UserId,
                    PaymentStatus = PaymentStatus.Successful
                };

                var newHistory = new History
                {
                    Comment = "Оплата за: " + utility.UtilityName,
                    DtAccount = accountFrom,
                    CtAccount = utility.UtilityAccountNumber,
                    OperationDate = DateTime.Now,
                    Sum = paySum,
                    UserId = account.UserId
                };

                using (var transaction = db.Database.BeginTransaction())
                {
                    account.Balance -= paySum;
                    accountTo.Balance += paySum;
                    db.Payments.Add(newPayment);
                    db.Histories.Add(newHistory);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception)
                    {
                        return Json(new { StatusCode = 204, message = "Что-то пошло не так, при сохранении данных." });
                    }

                    var newPaymentHistory = new PaymentHistory
                    {
                        PaymentId = newPayment.PaymentId,
                        HistoryId = newHistory.HistoryId
                    };

                    db.PaymentHistories.Add(newPaymentHistory);

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception)
                    {
                        return Json(new { StatusCode = 204, message = "Что-то пошло не так, при сохранении данных." });
                    }

                    transaction.Commit();
                }
                return Json(new { StatusCode = 200, Message = "Операция успешно выполнена. " });
            }
        }


        public ActionResult CheckPersonalAccount(string personalAccount)
        {
            using (var db = new ConnectionContext())
            {
                var personalA = db.PersonalAccounts.FirstOrDefault(x => x.ID == personalAccount);
                if (personalA != null)
                {
                    return Json(personalA, JsonRequestBehavior.AllowGet);
                }
                return Json(new { StatusCode = 404, Message = "Не найден лицевой счет" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PaymentHistories(string userName)
        {
            IQueryable<PaymentViewModelShow> histories = null;
            using (var db = new ConnectionContext())
            {
                var UserName = db.Users.FirstOrDefault(x => x.Login == userName);
                if (UserName == null)
                {
                    return HttpNotFound("Не удалось найти страницу");
                }

                if (User.IsInRole("admin"))
                {
                    histories = from p in db.Payments
                                join dbUtility in db.Utilities on p.UtilityId equals dbUtility.UtilityId
                                join dbUser in db.Users on p.UserId equals dbUser.UserId
                                select new PaymentViewModelShow
                                {
                                    userName = dbUser.FullName,
                                    PaymentComment = p.PaymentComment,
                                    PaymentDate = p.PaymentDate,
                                    PaymentStatus = p.PaymentStatus == PaymentStatus.Successful ? "Успех" : "Неуспешно",
                                    PaymentSum = p.PaymentSum,
                                    UtilityName = dbUtility.UtilityName
                                };
                    return View(histories.ToList());
                }
                else
                {
                    histories = from p in db.Payments
                                join dbUtility in db.Utilities on p.UtilityId equals dbUtility.UtilityId
                                join dbUser in db.Users on p.UserId equals dbUser.UserId
                                where dbUser.UserId == UserName.UserId
                                select new PaymentViewModelShow
                                {
                                    userName = dbUser.FullName,
                                    PaymentComment = p.PaymentComment,
                                    PaymentDate = p.PaymentDate,
                                    PaymentStatus = p.PaymentStatus == PaymentStatus.Successful ? "Успех" : "Неуспешно",
                                    PaymentSum = p.PaymentSum,
                                    UtilityName = dbUtility.UtilityName
                                };
                    return View(histories.ToList());
                }
            }
        }
    }
}