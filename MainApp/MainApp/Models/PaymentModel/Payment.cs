using System;
using System.ComponentModel.DataAnnotations;

namespace MainApp.Models.PaymentModel
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [Required(ErrorMessage = "Обзательное поле для заполенения \"Utility ID\"")]
        [Display(Name = "Utility ID")]
        public int UtilityId { get; set; }

        [Required(ErrorMessage = "Обзательное поле для заполенения \"Сумма\"")]
        [Display(Name = "Сумма")]
        public decimal PaymentSum { get; set; }

        //[Required(ErrorMessage = "Обзательное поле для заполенения \"Комиссия\"")]
        [Display(Name = "Комиссия")]
        public decimal PaymentComission { get; set; }

        [Required(ErrorMessage = "Обзательное поле для заполенения \"Комментарий\"")]
        [Display(Name = "Комментарий")]
        public string PaymentComment { get; set; }

        [Required(ErrorMessage = "Обзательное поле для заполенения \"Дата операции\"")]
        [Display(Name = "Дата операции")]
        public DateTime PaymentDate { get; set; }

        [Required(ErrorMessage = "Обзательное поле для заполенения \"User ID\"")]
        [Display(Name = "User ID")]
        public int UserId { get; set; }
    }
}