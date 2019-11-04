using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            if (AccountFrom == null) return Json(new { StatusCode = 404, Message = "Счет отправителя не найден" });
            if (AccountTo == null) return Json(new { StatusCode = 404, Message = "Счет получателя не найден" });
            if (TransferSum <= 0) return Json(new { StatusCode = 405, Message = "Сумма не может быть меньше или равна нулю" });

            using (var db = new ConnectionContext())
            {
                var accountFrom = db.Accounts.Find(AccountFrom);
                var accountTo = db.Accounts.Find(AccountTo);

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
                    Comission = 1
                };

                // Формирование истории
                var history = new History
                {
                    CtAccount = accountFrom.AccountNumber,
                    DtAccount = accountTo.AccountNumber,
                    Comment = "Перевод",
                    Sum = TransferSum,
                    OperationDate = DateTime.Now,
                    UserId = accountFrom.UserId
                };

                using (var transactions = db.Database.BeginTransaction())
                {
                    // Добавление перевода в таблицу
                    db.Transfers.Add(transfer);

                    // Добавление истории в таблицу
                    db.Histories.Add(history);

                    // Сохранение данных
                    db.SaveChanges();

                    // Добавление данных в промежуточную таблицу
                    db.TransferHistories.Add(new TransferHistory
                    {
                        TransferId = transfer.TransferId,
                        HistoryId = history.HistoryId
                    });

                    accountFrom.Balance -= TransferSum;
                    accountTo.Balance += TransferSum;
                    db.SaveChanges();
                    transactions.Commit();
                }
            }
            return Json(new { StatusCode = 200, Message = "Перевод успешно выполнен" });
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