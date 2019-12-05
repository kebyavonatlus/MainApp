using System;
using System.ComponentModel.DataAnnotations;
using MainApp.Enums;

namespace MainApp.Models.TransferModel
{
    public class Transfer
    {
        [Key]
        public int TransferId { get; set; }
        
        [Required(ErrorMessage = "Обзательное поле для заполенения \"Со счета\"")]
        [Display(Name = "Со счета")]
        public int AccountFrom { get; set; }

        [Required(ErrorMessage = "Обзательное поле для заполенения \"На счет\"")]
        [Display(Name = "На счет")]
        public int AccountTo { get; set; }
        
        [Required(ErrorMessage = "Обзательное поле для заполенения \"UserId отправителя\"")]
        [Display(Name = "UserId отправителя")]
        public int SenderUserId { get; set; }
        
        [Required(ErrorMessage = "Обзательное поле для заполенения \"UserId получателя\"")]
        [Display(Name = "UserId получателя")]
        public int ReceiverUserId { get; set; }
        
        [Required(ErrorMessage = "Обзательное поле для заполенения \"Комментарий\"")]
        [Display(Name = "Комментарий")]
        public string Comment { get; set; }
        
        [Required(ErrorMessage = "Обзательное поле для заполенения \"Сумма\"")]
        [Display(Name = "Сумма")]
        public decimal TransferSum { get; set; }

        [Required(ErrorMessage = "Обзательное поле для заполенения \"Дата перевода\"")]
        [Display(Name = "Дата перевода")]
        public DateTime TransferDate { get; set; }

        [Required(ErrorMessage = "Обзательное поле для заполенения \"Комиссия\"")]
        [Display(Name = "Комиссия")]
        public decimal Comission { get; set; }

        [Required(ErrorMessage = "Обзательное поле для заполенения \"Статус\"")]
        [Display(Name = "Статус")]
        public int? TransferStatus { get; set; }
    }
}