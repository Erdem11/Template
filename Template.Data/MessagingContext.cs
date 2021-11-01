using System;
using Microsoft.EntityFrameworkCore;
using Template.Common.SettingsConfigurationFiles;

namespace Template.Data
{
    /// <summary>
    /// Secondary DB
    /// </summary>
    public class MessagingContext : DbContext
    {
        private readonly SettingsHolder _settingsHolder;
        public DbSet<Message> Messages { get; set; }
    
        public MessagingContext(SettingsHolder settingsHolder)
        {
            _settingsHolder = settingsHolder;
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbOptions = _settingsHolder.SqlSettings.GetSecondary();
            
            switch (dbOptions.DbType)
            {
                case DbTypes.Mssql:
                    optionsBuilder.UseSqlServer(dbOptions.ConnectionString);
                    break;
                case DbTypes.Npgsql:
                    optionsBuilder.UseNpgsql(dbOptions.ConnectionString);
                    break;
                case DbTypes.Sqlite:
                    optionsBuilder.UseSqlite(dbOptions.ConnectionString);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public class Message
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Text { get; set; }
    }
}