using System.ComponentModel.DataAnnotations;

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
}