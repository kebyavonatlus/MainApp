using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MainApp.Models.TransferModel
{
    public class TransferHistory
    {
        [Key,Column(Order = 1)]
        public int TransferId { get; set; }

        [Key,Column(Order = 2)]
        public int HistoryId { get; set; }
    }
}