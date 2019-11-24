using System;
using System.ComponentModel.DataAnnotations;

namespace MainApp.ViewModels
{
    public class HistoriesViewModel
    {
        public int HistoryId { get; set; }
        [Required(ErrorMessage = "Введите дебет счет")]
        [Display(Name = "Дебет счет")]
        public int DtAccount { get; set; }

        [Required(ErrorMessage = "Введите кредит счет")]
        [Display(Name = "Кредит счет")]
        public int CtAccount { get; set; }

        [Required(ErrorMessage = "Введите сумму")]
        [Display(Name = "Сумма")]
        public decimal Sum { get; set; }

        [Required(ErrorMessage = "Введите комментарий")]
        [Display(Name = "Комментарий")]
        public string Comment { get; set; }

        [Required(ErrorMessage = "Введите дату операций")]
        [Display(Name = "Дата операции")]
        public DateTime OperationDate { get; set; }

        [Required]
        [Display(Name = "Пользователь")]
        public string UserName { get; set; }
    }
}