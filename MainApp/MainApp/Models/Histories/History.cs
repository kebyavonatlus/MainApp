using System;
using System.ComponentModel.DataAnnotations;

namespace MainApp.Models.Histories
{
    public class History
    {
        [Key]
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
        [Display(Name = "Id пользователя")]
        public int UserId { get; set; }
    }
}