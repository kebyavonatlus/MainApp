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

        [Required(ErrorMessage = "Выберите пол")]
        [Display(Name = "Пол")]
        public Gender Gender { get; set; }
    }
}