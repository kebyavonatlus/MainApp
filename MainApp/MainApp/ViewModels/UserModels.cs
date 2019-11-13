using System.ComponentModel.DataAnnotations;

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
}