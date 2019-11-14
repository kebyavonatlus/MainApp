using System;
using System.ComponentModel.DataAnnotations;
using MainApp.Enums;

namespace MainApp.ViewModels
{
    public class TransferViewModel
    {
        [Required(ErrorMessage = "Обзательное поле для заполенения \"Номер операции\"")]
        [Display(Name = "Номер операции")]
        public int TransferId { get; set; }

        [Required(ErrorMessage = "Обзательное поле для заполенения \"Номер счета от\"")]
        [Display(Name = "Со счета")]
        public int AccountFrom { get; set; }

        [Required(ErrorMessage = "Обзательное поле для заполенения \"На счет\"")]
        [Display(Name = "На счет")]
        public int AccountTo { get; set; }

        [Required(ErrorMessage = "Обзательное поле для заполенения \"Отправитель\"")]
        [Display(Name = "Отправитель")]
        public string SenderName { get; set; }

        [Required(ErrorMessage = "Обзательное поле для заполенения \"Комментарий\"")]
        [Display(Name = "Комментарий")]
        public string Comment { get; set; }

        [Required(ErrorMessage = "Обзательное поле для заполенения \"Сумма операции\"")]
        [Display(Name = "Сумма операции")]
        public decimal TransferSum { get; set; }

        [Required(ErrorMessage = "Обзательное поле для заполенения \"Дата перевода\"")]
        [Display(Name = "Дата перевода")]
        public DateTime TransferDate { get; set; }

        [Required(ErrorMessage = "Обзательное поле для заполенения \"Статус\"")]
        [Display(Name = "Статус")]
        public string TransferStatus { get; set; }
    }
}