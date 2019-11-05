using System.ComponentModel;

namespace MainApp.Enums
{
    public enum TransferStatus
    {
        [Description("Создан")]
        Created = 1,
        [Description("Принят")]
        Confirmed = 2
    }
}