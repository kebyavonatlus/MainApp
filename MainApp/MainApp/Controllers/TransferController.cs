using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MainApp.Enums;
using MainApp.Models;
using MainApp.Models.Histories;
using MainApp.Models.TransferModel;
using MainApp.ViewModels;
using Microsoft.SqlServer.Server;

namespace MainApp.Controllers
{
    public class TransferController : Controller
    {
        // GET: Transfer
        public ActionResult Index(string userName)
        {
            using (var db = new ConnectionContext())
            {
                var User = db.Users.FirstOrDefault(u => u.Login == userName);
                if (User == null) return Json(new { StatusCode = 404, Message = "Пользователь не найден" }, JsonRequestBehavior.AllowGet);

                var userTransfers = from tranfers in db.Transfers
                    join sentUser in db.Users on tranfers.SenderUserId equals sentUser.UserId
                    where tranfers.ReceiverUserId == User.UserId || tranfers.SenderUserId == User.UserId
                    select new TransferViewModel
                    {
                        TransferId = tranfers.TransferId,
                        AccountFrom = tranfers.AccountFrom,
                        AccountTo = tranfers.AccountTo,
                        SenderName = sentUser.FullName,
                        Comment = tranfers.Comment,
                        TransferSum = tranfers.TransferSum,
                        TransferDate = tranfers.TransferDate,
                        TransferStatus = tranfers.TransferStatus == TransferStatus.Created ? "Создан" : "Принят"
                    };
                return View(userTransfers.ToList());
            }
        }

        /// <summary>
        /// Создание перевода
        /// </summary>
        /// <param name="AccountFrom">int</param>
        /// <param name="AccountTo">int</param>
        /// <param name="TransferSum">decimal</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Transfer(int? AccountFrom, int? AccountTo, decimal TransferSum)
        {
            using (var db = new ConnectionContext())
            {
                var accountFrom = db.Accounts.FirstOrDefault(a => a.AccountNumber == AccountFrom);
                var accountTo = db.Accounts.FirstOrDefault(a => a.AccountNumber == AccountTo);

                if (TransferSum <= 0) return Json(new { StatusCode = 405, Message = "Сумма не может быть меньше или равна нулю" });
                if (AccountFrom == null) return Json(new { StatusCode = 404, Message = "Счет отправителя не найден" });
                if (AccountTo == null) return Json(new { StatusCode = 404, Message = "Счет получателя не найден" });

                if (accountFrom.Balance < TransferSum)
                {
                    return Json(new { StatusCode = 405, Message = "Недостаточно денег на счете: " + accountFrom.AccountNumber });
                }

                // Формирование перевода
                var transfer = new Transfer
                {
                    AccountFrom = accountFrom.AccountNumber,
                    AccountTo = accountTo.AccountNumber,
                    SenderUserId = accountFrom.UserId,
                    ReceiverUserId = accountTo.UserId,
                    Comment = accountFrom.AccountName + " выполнил перевод на счет " + accountTo.AccountName,
                    TransferDate = DateTime.Now,
                    TransferSum = TransferSum,
                    Comission = 1,
                    TransferStatus = TransferStatus.Created
                };
                
                // Добавление перевода в таблицу
                db.Transfers.Add(transfer);

                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    return Json(new { StatusCode = 204, message = "Что-то пошло не так!" });
                }

            }
            return Json(new { StatusCode = 200, Message = "Перевод успешно создан" });
        }

        /// <summary>
        /// Подтверждение перевода
        /// </summary>
        /// <param name="transferId">int</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ConfirmTransfer(int? transferId, string userName)
        {
            using (var db = new ConnectionContext())
            {
                var Transfer = db.Transfers.FirstOrDefault(t => t.TransferId == transferId);
                var User = db.Users.FirstOrDefault(u => u.Login == userName);

                if (Transfer == null) return Json(new {StatusCode = 404, Message = "Перевод не найден"});
                if (User == null) return Json(new { StatusCode = 404, Message = "Пользователь не найден" });

                if (Transfer.ReceiverUserId != User.UserId) return Json(new { StatusCode = 403, Message = "Невозможно принять перевод. Не соотвествует пользователь" });
                if (Transfer.TransferStatus == TransferStatus.Confirmed) return Json(new { StatusCode = 203, Message = "Перевод уже принят." });

                var accountFrom = db.Accounts.FirstOrDefault(a => a.AccountNumber == Transfer.AccountFrom);
                var accountTo = db.Accounts.FirstOrDefault(a => a.AccountNumber == Transfer.AccountTo);

                if (accountFrom == null) return Json(new { StatusCode = 404, Message = "Счет отправителя не найден" });
                if (accountTo == null) return Json(new { StatusCode = 404, Message = "Счет получателя не найден" });


                // Формирование истории
                var history = new History
                {
                    CtAccount = accountFrom.AccountNumber,
                    DtAccount = accountTo.AccountNumber,
                    Comment = "Перевод",
                    Sum = Transfer.TransferSum,
                    OperationDate = DateTime.Now,
                    UserId = accountFrom.UserId
                };

                using (var transactions = db.Database.BeginTransaction())
                {
                    // Добавление истории
                    db.Histories.Add(history);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception)
                    {
                        return Json(new { StatusCode = 204, message = "При подтверждении перевода произошла ошибка. Попробуйте позже." });
                    }

                    // Добавление данных в промежуточную таблицу
                    db.TransferHistories.Add(new TransferHistory
                    {
                        TransferId = Transfer.TransferId,
                        HistoryId = history.HistoryId
                    });

                    accountFrom.Balance -= Transfer.TransferSum;
                    accountTo.Balance += Transfer.TransferSum;

                    // Меняем статус перевода
                    Transfer.TransferStatus = TransferStatus.Confirmed;

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception)
                    {
                        return Json(new { StatusCode = 204, message = "При подтверждении перевода произошла ошибка. Попробуйте позже." });
                    }
                    transactions.Commit();
                }

                return Json(new {StatusCode = 202, Message = "Перевод успешно принят"});
            }
        }

        /// <summary>
        /// Отмена перевода
        /// </summary>
        /// <param name="transferId">int</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CancelTransfer(int? transferId, string userName)
        {
            using (var db = new ConnectionContext())
            {
                var User = db.Users.FirstOrDefault(u => u.Login == userName);
                var TransferId = db.Transfers.FirstOrDefault(x => x.TransferId == transferId);

                if (TransferId == null) return Json(new { StatusCode = 404, Message = "Перевод не найден" });
                if (User == null) return Json(new { StatusCode = 404, Message = "Пользователь не найден" });

                if (TransferId.TransferStatus != TransferStatus.Created) return Json(new { StatusCode = 203, Message = "Невозможно отменить перевод. Перевод уже подтвержден." });

                if (TransferId.SenderUserId != User.UserId) return Json(new { StatusCode = 203, Message = "Невозможно отменить перевод. Только отправитель может отменить перевод" });

                db.Transfers.Remove(TransferId);

                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    return Json(new { StatusCode = 204, message = "Невозможно отменить перевод. Произошла непредвиденная ошибка" });
                }

            }
            return Json(new { StatusCode = 200, Message = "Перевод успешно отеменен" });
        }


        [HttpGet]
        public ActionResult UserTransfers(string userName)
        {
            using (var db = new ConnectionContext())
            {
                var User = db.Users.FirstOrDefault(u => u.Login == userName);
                if (User == null) return Json(new {StatusCode = 404, Message = "Пользователь не найден"}, JsonRequestBehavior.AllowGet);

                var userTransfers = from tranfers in db.Transfers
                    join sentUser in db.Users on tranfers.SenderUserId equals sentUser.UserId
                                    where tranfers.ReceiverUserId == User.UserId
                    select new TransferViewModel
                    {
                        TransferId = tranfers.TransferId,
                        AccountFrom = tranfers.AccountFrom,
                        AccountTo = tranfers.AccountTo,
                        SenderName = sentUser.FullName,
                        Comment = tranfers.Comment,
                        TransferSum = tranfers.TransferSum,
                        TransferDate = tranfers.TransferDate,
                        TransferStatus = tranfers.TransferStatus == TransferStatus.Created ? "Создан" : "Принят"
                    };
                return Json(userTransfers.ToList(), JsonRequestBehavior.AllowGet);
            }
        }
    }
}