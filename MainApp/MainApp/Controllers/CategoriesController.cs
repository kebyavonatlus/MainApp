using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Mvc;
using MainApp.Models;
using MainApp.Models.PaymentModel;
using MainApp.Providers;

namespace MainApp.Controllers
{
    [CustomAuthorize(Roles = "admin")]

    public class CategoriesController : Controller
    {
        // GET: Categories
        public ActionResult Index()
        {
            using (var db = new ConnectionContext())
            {
                var allCategories = db.UtilityCategories.ToList();
                return View(allCategories);
            }
        }

        public ActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCategory(UtilityCategory category)
        {
            using (var db = new ConnectionContext())
            {
                var newCategory = new UtilityCategory
                {
                    UtilityCategoryName = category.UtilityCategoryName,
                    UtilityCategoryDescription = category.UtilityCategoryDescription
                };

                db.UtilityCategories.Add(newCategory);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
        }

        public ActionResult EditCategory(int? id)
        {
            using (var db = new ConnectionContext())
            {
                var category = db.UtilityCategories.FirstOrDefault(x => x.UtilityCategoryId == id);
                return View(category);
            }
        }

        [HttpPost]
        public ActionResult EditCategory(int? id, UtilityCategory category)
        {
            using (var db = new ConnectionContext())
            {
                if (category == null)
                {
                    ModelState.AddModelError(string.Empty, "Непредвиденная ошибка. Повторите позже");
                }
                if (ModelState.IsValid)
                {
                    db.UtilityCategories.AddOrUpdate(category);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(category);
            }
        }
    }
}