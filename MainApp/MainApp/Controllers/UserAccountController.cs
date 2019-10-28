using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MainApp.Models;
using MainApp.Models.UserModel;
using MainApp.ViewModels;

namespace MainApp.Controllers
{
    public class UserAccountController : Controller
    {
        /// <summary>
        /// Авторищация пользователя на сайт
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var hashPassword = GetHash(model.Password);
                User user = null;
                using (ConnectionContext db = new ConnectionContext())
                {
                    user = db.Users.FirstOrDefault(u => u.Login == model.Name && u.Password == hashPassword);
                }
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Name, true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Неверный логин или пароль.");
                }
            }

            return View(model);
        }

        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                using (ConnectionContext db = new ConnectionContext())
                {
                    user = db.Users.FirstOrDefault(u => u.Login == model.Login);
                }
                var hashPassword = GetHash(model.Password);
                if (user == null)
                {
                    using (ConnectionContext db = new ConnectionContext())
                    {
                        db.Users.Add(new User { Login = model.Login, Email = model.Email, Password = hashPassword, FullName = model.FullName, IsCustomer = true});
                        try
                        {
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            return Json(new { StatusCode = 204, message = "Что-то пошло не так!" });
                        }
                        user = db.Users.FirstOrDefault(u => u.Login == model.Login && u.Password == hashPassword);
                    }
                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.Login, true);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует");
                }
            }
            return View(model);
        }
        /// <summary>
        /// Генерирует хэш пароля на основе переданного значения
        /// </summary>
        /// <param name="input">string</param>
        /// <returns></returns>
        private string GetHash(string input)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}