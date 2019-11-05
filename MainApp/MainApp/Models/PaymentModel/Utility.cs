using System.ComponentModel.DataAnnotations;
using MainApp.Enums;

namespace MainApp.Models.PaymentModel
{
    public abstract class Utility 
    {
        [Key]
        public int UtilityId { get; set; }

        [Required(ErrorMessage = "Обзательное поле для заполенения \"Наименование\"")]
        [Display(Name = "Наименование")]
        public string UtilityName { get; set; }

        [Display(Name = "Описание")]
        public string UtilityDescription { get; set; }

        [Required(ErrorMessage = "Обзательное поле для заполенения \"ID категории\"")]
        [Display(Name = "ID категории")]
        public int UtilityCategoryId { get; set; }

        [Required(ErrorMessage = "Обзательное поле для заполенения \"Номер счета\"")]
        [Display(Name = "Номер счета")]
        public int UtilityAccountNumber { get; set; }

        [Required(ErrorMessage = "Обзательное поле для заполенения \"Валюта\"")]
        [Display(Name = "Валюта")]
        public CurrencyId UtilityCurrencyId { get; set; }
    }
}