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
        public ActionResult Edit(int? id, UtilityViewModel utility)
        {
            using (var db = new ConnectionContext())
            {
                var uAcountNo = db.Accounts.FirstOrDefault(a => a.AccountNumber == utility.UtilityAccountNumber);
                if (uAcountNo == null)
                {
                    ModelState.AddModelError(string.Empty, "Не найден счет.");
                }
                var updateUtility = db.Utilities.FirstOrDefault(a => a.UtilityId == id);

                if (updateUtility == null)
                {
                    ModelState.AddModelError(string.Empty, "Не найден ID услуги");
                }

                var categories = db.UtilityCategories.ToList();
                ViewBag.UtilityCategories = categories.Select(r => new SelectListItem
                {
                    Value = r.UtilityCategoryId.ToString(),
                    Text = r.UtilityCategoryName
                }).ToList();

            }

            if (ModelState.IsValid)
            {
                using (var db = new ConnectionContext())
                {
                    var updateUtility = db.Utilities.FirstOrDefault(u => u.UtilityId == id);
                    if (utility.ImageFile != null)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(utility.ImageFile.FileName);
                        var extension = Path.GetExtension(utility.ImageFile.FileName);

                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;


                        fileName = Path.Combine(Server.MapPath("~/Content/UploadImages/UtilityImages/"), fileName);
                        utility.ImageFile.SaveAs(fileName);

                        updateUtility.UtilityAccountNumber = utility.UtilityAccountNumber;
                        updateUtility.UtilityCategoryId = utility.UtilityCategoryId;
                        updateUtility.UtilityDescription = utility.UtilityDescription;
                        updateUtility.UtilityImagePath = utility.UtilityImagePath;
                        updateUtility.UtilityName = utility.UtilityName;

                        using (var transaction = db.Database.BeginTransaction())
                        {
                            db.Utilities.AddOrUpdate(updateUtility);
                            db.SaveChanges();
                            transaction.Commit();
                        }
                        return RedirectToAction("Index");
                    }

                    updateUtility.UtilityAccountNumber = utility.UtilityAccountNumber;
                    updateUtility.UtilityCategoryId = utility.UtilityCategoryId;
                    updateUtility.UtilityDescription = utility.UtilityDescription;
                    updateUtility.UtilityName = utility.UtilityName;

                    using (var transaction = db.Database.BeginTransaction())
                    {
                        db.Utilities.AddOrUpdate(updateUtility);
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    return RedirectToAction("Index");
                }
            }

            return View(utility);
        }
    }
}
