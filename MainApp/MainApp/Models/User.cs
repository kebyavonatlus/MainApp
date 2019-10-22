using System;
using System.ComponentModel.DataAnnotations;

namespace MainApp.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string SureName { get; set; }
        public string IdentificationNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Address { get; set; }
    }
}