using System.ComponentModel.DataAnnotations;
using System.Web;

namespace MainApp.ViewModels
{
    public class UtilityViewModel
    {
        [Required(ErrorMessage = "Обзательное поле для заполенения \"Наименование\"")]
        [Display(Name = "Наименование")]
        public string UtilityName { get; set; }

        [Display(Name = "Описание")]
        public string UtilityDescription { get; set; }

        [Required(ErrorMessage = "Обзательное поле для заполенения \"Категория\"")]
        [Display(Name = "Категория")]
        public int UtilityCategoryId { get; set; }

        [Required(ErrorMessage = "Обзательное поле для заполенения \"Номер счета\"")]
        [Display(Name = "Номер счета")]
        public int UtilityAccountNumber { get; set; }

        public string UtilityImagePath { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }
    }
}