using System.ComponentModel.DataAnnotations;

namespace MainApp.Models.UserModel
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        [Display(Name = "Имя роли")]
        public string RoleName { get; set; }
    }
}