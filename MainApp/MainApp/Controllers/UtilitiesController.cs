using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MainApp.Models;
using MainApp.Models.AccountModel;
using MainApp.Models.PaymentModel;
using MainApp.Providers;
using MainApp.ViewModels;

namespace MainApp.Controllers
{

    [CustomAuthorize(Roles = "admin")]
    public class UtilitiesController : Controller
    {
        // GET: Utilities
        public ActionResult Index()
        {
            using (var db = new ConnectionContext())
            {
                var utility = from u in db.Utilities join c in db.UtilityCategories on u.UtilityCategoryId equals c.UtilityCategoryId 
                    select new UtilityViewModel
                    {
                        UtilityAccountNumber = u.UtilityAccountNumber,
                        UtilityName = u.UtilityName,
                        UtilityCategoryName = c.UtilityCategoryName,
                        UtilityDescription = u.UtilityDescription,
                        UtilityImagePath = u.UtilityImagePath,
                        UtilityId = u.UtilityId
                    };

                return View(utility.ToList());
            }
        }

        // GET: Utilities/Create
        public ActionResult Create()
        {
            using (var db = new ConnectionContext())
            {
                var categories = db.UtilityCategories.ToList();

                ViewBag.UtilityCategories = categories.Select(r => new SelectListItem
                {
                    Value = r.UtilityCategoryId.ToString(),
                    Text = r.UtilityCategoryName
                }).ToList();
            }

            return View();
        }

        // POST: Utilities/Create
        [HttpPost]
        public ActionResult Create(UtilityViewModel utility)
        {
            ModelState.Clear();
            Account uAcountNo = null;
            using (var db = new ConnectionContext())
            {
                uAcountNo = db.Accounts.FirstOrDefault(a => a.AccountNumber == utility.UtilityAccountNumber);
                var uCategory =
                    db.UtilityCategories.FirstOrDefault(u => u.UtilityCategoryId == utility.UtilityCategoryId);
                var categories = db.UtilityCategories.ToList();

                ViewBag.UtilityCategories = categories.Select(r => new SelectListItem
                {
                    Value = r.UtilityCategoryId.ToString(),
                    Text = r.UtilityCategoryName
                }).ToList();

            }

            if (uAcountNo == null)
            {
                ModelState.AddModelError(string.Empty, "Счет не найден");
            }

            if (ModelState.IsValid)
            {
                using (var db = new ConnectionContext())
                {
                    var fileName = Path.GetFileNameWithoutExtension(utility.ImageFile.FileName);
                    var extension = Path.GetExtension(utility.ImageFile.FileName);

                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;

                    var newUtility = new Utility
                    {
                        UtilityImagePath = "~/Content/UploadImages/UtilityImages/" + fileName,
                        UtilityAccountNumber = utility.UtilityAccountNumber,
                        UtilityName = utility.UtilityName,
                        UtilityDescription = utility.UtilityDescription,
                        UtilityCategoryId = utility.UtilityCategoryId
                    };

                    using (var transaction = db.Database.BeginTransaction())
                    {
                        db.Utilities.Add(newUtility);
                        db.SaveChanges();
                        transaction.Commit();
                    }

                    fileName = Path.Combine(Server.MapPath("~/Content/UploadImages/UtilityImages/"), fileName);
                    utility.ImageFile.SaveAs(fileName);

                    return RedirectToAction("Index");

                }
            }


            return View(utility);
        }

        // GET: Utilities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            using (var db = new ConnectionContext())
            {
                var categories = db.UtilityCategories.ToList();

                ViewBag.UtilityCategories = categories.Select(r => new SelectListItem
                {
                    Value = r.UtilityCategoryId.ToString(),
                    Text = r.UtilityCategoryName
                }).ToList();

                var utility = from u in db.Utilities
                              where u.UtilityId == id
                              select new UtilityViewModel
                              {
                                  UtilityId = u.UtilityId,
                                  UtilityCategoryId = u.UtilityCategoryId,
                                  UtilityAccountNumber = u.UtilityAccountNumber,
                                  UtilityDescription = u.UtilityDescription,
                                  UtilityName = u.UtilityName,
                                  UtilityImagePath = u.UtilityImagePath
                              };
                return View(utility.First());
            }
        }

        // POST: Utilities/Edit/5
        [HttpPost]
        public ActionResult Edit(UtilityViewModel utility)
        {
            using (var db = new ConnectionContext())
            {
                var Utility = db.Utilities.FirstOrDefault(x => x.UtilityId == utility.UtilityId);

                using (var transaction = db.Database.BeginTransaction())
                {
                    Utility.UtilityAccountNumber = utility.UtilityAccountNumber;
                    Utility.UtilityCategoryId = utility.UtilityCategoryId;
                    Utility.UtilityDescription = utility.UtilityDescription;
                    Utility.UtilityName = utility.UtilityName;
                    
                    db.Utilities.AddOrUpdate(Utility);
                    db.SaveChanges();
                    transaction.Commit();
                    return RedirectToAction("Index");
                }
            }
            return View(utility);
        }
    }
}
