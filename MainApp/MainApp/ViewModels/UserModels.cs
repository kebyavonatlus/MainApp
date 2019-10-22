using System.ComponentModel.DataAnnotations;

namespace MainApp.Models
{
    public class LoginModel
    {
        [Display()]
        public string Name { get; set; }
        public string Password { get; set; }
    }

    public class RegisterModel
    {
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string SureName { get; set; }
    }
}