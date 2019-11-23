using System;
using System.ComponentModel.DataAnnotations;
using MainApp.Enums;

namespace MainApp.ViewModels
{
    public class PaymentViewModel
    {
        public int UtilityId { get; set; }
        public string UtilityName { get; set; }
        public string UtilityCategoryName { get; set; }
        public int UtilityAccountNumber { get; set; }
        public string UtilityImagePath { get; set; }
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