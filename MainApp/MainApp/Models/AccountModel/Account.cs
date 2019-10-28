using System;
using System.ComponentModel.DataAnnotations;

namespace MainApp.Models.AccountModel
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }
        [Required]
        [Display(Name = "Номер счета")]
        public string AccountNumber { get; set; }

        [Required]
        [Display(Name = "Наименование")]
        public string AccountName { get; set; }

        [Required]
        [Display(Name = "Дата открытия")]
        public DateTime AccountOpenDate { get; set; }
        
        [Display(Name = "Дата закрытия")]
        public DateTime? AccountCloseDate { get; set; }
        
        [Display(Name = "Баланс")]
        public decimal Balance { get; set; }

        [Required]
        [Display(Name = "Валюта")]
        public string Currency { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}