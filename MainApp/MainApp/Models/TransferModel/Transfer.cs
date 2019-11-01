namespace MainApp.Models.TransferModel
{
    public class Transfer
    {
        public int TransferId { get; set; }
        public string AccountFrom { get; set; }
        public string AccountTo { get; set; }
        public int SenderUserId { get; set; }
        public int ReceiverUserId { get; set; }
        public string Comment { get; set; }
        public decimal TransferSum { get; set; }
        public decimal Comission { get; set; }
    }
}