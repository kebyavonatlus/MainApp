using System;
using System.ComponentModel.DataAnnotations;
using MainApp.Enums;
using Microsoft.VisualBasic;

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

    public class Refill
    {
        [Required(ErrorMessage = "Введите номер счета")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Номер счета только число")]
        [Display(Name = "Номер счета")]
        public int AccountNum { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Введите число")]
        [Display(Name = "Сумма")]
        public decimal RefillSum { get; set; }
    }
}