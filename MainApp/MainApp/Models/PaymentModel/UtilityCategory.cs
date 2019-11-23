using System.ComponentModel.DataAnnotations;

namespace MainApp.Models.PaymentModel
{
    public class UtilityCategory
    {
        [Key]
        public int UtilityCategoryId { get; set; }

        [Required(ErrorMessage = "Обзательное поле для заполенения \"Наименование категории\"")]
        [Display(Name = "Наименование категории")]
        public string UtilityCategoryName { get; set; }

        [Display(Name = "Описание категории")]
        public string UtilityCategoryDescription { get; set; }
    }
}