using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MainApp.Models;
using MainApp.Providers;
using MainApp.ViewModels;

namespace MainApp.Controllers
{
    [CustomAuthorize(Roles = "admin")]
    public class HistoriesController : Controller
    {
        // GET: Histories
        public ActionResult Index()
        {
            using (var db = new ConnectionContext())
            {
                var histories = from h in db.Histories
                    join dbUser in db.Users on h.UserId equals dbUser.UserId
                    select new HistoriesViewModel
                    {
                        UserName = dbUser.FullName,
                        Comment = h.Comment,
                        CtAccount = h.CtAccount,
                        DtAccount = h.DtAccount,
                        HistoryId = h.HistoryId,
                        OperationDate = h.OperationDate,
                        Sum = h.Sum
                    };
                return View(histories.ToList());
            }
        }
    }
}