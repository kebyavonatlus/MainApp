using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MainApp.Models.PaymentModel
{
    public class PaymentHistory
    {
        [Key, Column(Order = 1)]
        public int PaymentId { get; set; }

        [Key, Column(Order = 2)]
        public int HistoryId { get; set; }
    }
}