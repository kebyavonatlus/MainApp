namespace MainApp.Models.Histories
{
    public class Histories
    {
        public int HistoryId { get; set; }
        public string DtAccount { get; set; }
        public string CtAccount { get; set; }
        public decimal Sum { get; set; }
        public string Comment { get; set; }
        public int UserId { get; set; }
    }
}