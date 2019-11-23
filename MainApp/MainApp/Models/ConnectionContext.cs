using System.Data.Entity;
using MainApp.Models.AccountModel;
using MainApp.Models.Histories;
using MainApp.Models.PaymentModel;
using MainApp.Models.TransferModel;
using MainApp.Models.UserModel;

namespace MainApp.Models
{
    public class ConnectionContext : DbContext
    {
        public ConnectionContext() : base("DefaultConnection")
        {
        }

        /// <summary>
        /// Таблица пользователей
        /// </summary>
        public DbSet<User> Users { get; set; }
        
        /// <summary>
        /// Таблица ролей
        /// </summary>
        public DbSet<Role> Roles { get; set; }

        /// <summary>
        /// Таблица пользователей и ролей
        /// </summary>
        public DbSet<UserRoles> UserRoles { get; set; }
        
        /// <summary>
        /// Таблица счетов
        /// </summary>
        public DbSet<Account> Accounts { get; set; }

        /// <summary>
        /// История платежей
        /// </summary>
        public DbSet<History> Histories { get; set; }

        /// <summary>
        /// История переводов
        /// </summary>
        public DbSet<Transfer> Transfers { get; set; }

        /// <summary>
        /// Промежуточная таблица между историей и переводом
        /// </summary>
        public DbSet<TransferHistory> TransferHistories { get; set; }

        /// <summary>
        /// Таблица всех услуг
        /// </summary>
        public DbSet<Utility> Utilities { get; set; }

        /// <summary>
        /// Категория услуг
        /// </summary>
        public DbSet<UtilityCategory> UtilityCategories { get; set; }

        /// <summary>
        /// Лицевые счета клиентов сторонних сервисов
        /// </summary>
        public DbSet<PersonalAccount> PersonalAccounts { get; set; }


        /// <summary>
        /// Все платежи
        /// </summary>
        public DbSet<Payment> Payments { get; set; }

        /// <summary>
        /// Промежуточная таблица
        /// </summary>
        public DbSet<PaymentHistory> PaymentHistories { get; set; }
    }
}