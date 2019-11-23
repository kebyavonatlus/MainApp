using System;
using System.ComponentModel.DataAnnotations;
using MainApp.Enums;

namespace MainApp.ViewModels
{
    public class AccountViewModel
    {
        [Display(Name = "Номер счета")]
        public int AccountNumber { get; set; }

        [Display(Name = "Наименование счета")]
        public string AccountName { get; set; }

        [Display(Name = "Дата открытия")]
        public DateTime AccountOpenDate { get; set; }

        [Display(Name = "Баланс")]
        public decimal Balance { get; set; }

        [Display(Name = "Валюта")]
        public string Currency { get; set; }
    }

    public class CreateAccountView
    {
        [Display(Name = "Пользователь")]
        public string userName { get; set; }

        [Display(Name = "Дата открытия")]
        public DateTime OpenDate { get; set; }
    }
}