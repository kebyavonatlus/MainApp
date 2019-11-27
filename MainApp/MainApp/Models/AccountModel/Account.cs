using System;
using System.ComponentModel.DataAnnotations;
using MainApp.Enums;

namespace MainApp.Models.AccountModel
{
    public class Account
    {
        [Key]
        [Display(Name = "Номер счета")]
        public int AccountNumber { get; set; }

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

        [Display(Name = "Валюта")]
        public CurrencyId? Currency { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}