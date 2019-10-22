using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MainApp.Models
{
    public class UserRoles
    {
        [Key, Column(Order = 0)]
        public int RoleId { get; set; }
        [Key, Column(Order = 1)]
        public int UserId { get; set; }
    }
}