using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MainApp.Enums;
using MainApp.Models;
using MainApp.Models.Histories;
using MainApp.Models.TransferModel;

namespace MainApp.Controllers
{
    public class TransferController : Controller
    {
        // GET: Transfer
        public ActionResult Index()
        {
            using (var db = new ConnectionContext())
            {
                var transfers = db.Transfers.ToList();
                return View(transfers);
            }
        }

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

        [HttpPost]
        public ActionResult ConfirmTransfer(int? transferId)
        {
            using (var db = new ConnectionContext())
            {
                var Transfer = db.Transfers.FirstOrDefault(t => t.TransferId == transferId);

                if (Transfer == null) return Json(new {StatusCode = 404, Message = "Перевод не найден"});

                var accountFrom = db.Accounts.FirstOrDefault(a => a.AccountNumber == Transfer.AccountFrom);
                var accountTo = db.Accounts.FirstOrDefault(a => a.AccountNumber == Transfer.AccountTo);

                if (accountFrom == null) return Json(new { StatusCode = 404, Message = "Счет не найден" });
                if (accountTo == null) return Json(new { StatusCode = 404, Message = "Счет не найден" });


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
                        return Json(new { StatusCode = 204, message = "При подтверждении перевода, произошла ошибка. Попробуйте позже." });
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
                        return Json(new { StatusCode = 204, message = "При подтверждении перевода, произошла ошибка. Попробуйте позже." });
                    }
                    transactions.Commit();
                }

                return Json(new {StatusCode = 200, Message = "Перевод успешно принят"});
            }
        }

        [HttpPost]
        public ActionResult CancelTransfer(int? transferId)
        {
            using (var db = new ConnectionContext())
            {
                var TransferId = db.Transfers.FirstOrDefault(x => x.TransferId == transferId);

                if (TransferId == null) return Json(new { StatusCode = 404, Message = "Перевод не найден" });

                var TransferHistory = db.TransferHistories.First(x => x.TransferId == transferId);
                var HistoryId = db.Histories.First(x => x.HistoryId == TransferHistory.TransferId);

                var accountFrom = db.Accounts.Find(TransferId.AccountFrom);
                var accountTo = db.Accounts.Find(TransferId.AccountTo);
                var transferSum = TransferId.TransferSum;

                using (var transaction = db.Database.BeginTransaction())
                {
                    accountFrom.Balance += transferSum;
                    accountTo.Balance -= transferSum;
                    db.TransferHistories.Remove(TransferHistory);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception)
                    {
                        return Json(new { StatusCode = 400, Message = "Не удалось отменить перевод" });
                    }

                    db.Histories.Remove(HistoryId);
                    db.Transfers.Remove(TransferId);

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception)
                    {
                        return Json(new { StatusCode = 400, Message = "Не удалось отменить перевод" });
                    }
                    transaction.Commit();
                }
            }
            return Json(new { StatusCode = 200, Message = "Перевод успешно отеменен" });
        }
    }
}