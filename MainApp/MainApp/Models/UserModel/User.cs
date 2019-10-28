using System;
using System.ComponentModel.DataAnnotations;

namespace MainApp.Models.UserModel
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Нужно ввести Логин")]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Нужно ввести Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Нужно ввести пароль")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Нужно ввести ФИО")]
        [Display(Name = "ФИО")]
        public string FullName { get; set; }

        [Display(Name = "ИНН")]
        public string IdentificationNumber { get; set; }

        [Display(Name = "Дата рождения")]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Адрес")]
        public string Address { get; set; }

        public bool? IsCustomer { get; set; }
    }
}