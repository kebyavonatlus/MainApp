using System;
using System.ComponentModel.DataAnnotations;
using MainApp.Enums;

namespace MainApp.ViewModels
{
    public class PaymentViewModel
    {
        [Display(Name = "UtilityId")]
        public int UtilityId { get; set; }

        [Display(Name = "Наименование услуги")]
        public string UtilityName { get; set; }

        [Display(Name = "Категория")]
        public string UtilityCategoryName { get; set; }

        [Display(Name = "Счет получателя")]
        public int UtilityAccountNumber { get; set; }
        public string UtilityImagePath { get; set; }

        [Display(Name = "Счет отправителя")]
        public int AccountFrom { get; set; }
    }

    public class PaymentViewModelShow
    {
        [Display(Name = "Наименование услуги")]
        public string UtilityName { get; set; }

        [Display(Name = "Сумма")]
        public decimal PaymentSum { get; set; }

        [Display(Name = "Комментарий")]
        public string PaymentComment { get; set; }

        [Display(Name = "Дата операции")]
        public DateTime PaymentDate { get; set; }

        [Display(Name = "Статус")]
        public string PaymentStatus { get; set; }

        [Display(Name = "Пользователь")]
        public string userName { get; set; }
    }
}