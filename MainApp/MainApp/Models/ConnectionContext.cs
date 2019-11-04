﻿using System.Data.Entity;
using MainApp.Models.AccountModel;
using MainApp.Models.Histories;
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
    }
}