using System;
using System.ComponentModel.DataAnnotations;
using MainApp.Enums;

namespace MainApp.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Нужно ввести логин.")]
        [Display(Name = "Логин")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Нужно ввести пароль.")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class RegisterModel
    {
        [Required(ErrorMessage = "Нужно ввести логин.")]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Нужно ввести Email.")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Нужно ввести пароль.")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Нужно ввести ФИО.")]
        [Display(Name = "ФИО")]
        public string FullName { get; set; }
    }

    public class UserProfile
    {
        [Key]
        public int UserId { get; set; }
        
        [Required(ErrorMessage = "Введите ФИО")]
        [Display(Name = "ФИО")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Введите ИНН")]
        [Display(Name = "ИНН")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "ИНН должен состоять только из цифр")]
        public string IdentificationNumber { get; set; }

        [Required(ErrorMessage = "Введите дату рождения")]
        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessage = "Выберите параметр")]
        [Display(Name = "Активирован Email")]
        public bool? EmailAccepted { get; set; }

        [Required(ErrorMessage = "Выберите параметр")]
        [Display(Name = "Получать рассылку")]
        public bool? SendToEmail { get; set; }

        [Display(Name = "Адрес")]
        public string Address { get; set; }
    }

    public class UserChangePassword
    {
        public string UserName { get; set; }
        [Required(ErrorMessage = "Введите новый пароль")]
        [Display(Name = "Новый пароль")]
        [DataType(DataType.Password)]

        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Повторите новый пароль")]
        [Display(Name = "Повтор нового пароля")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmNewPassword { get; set; }

    }
}