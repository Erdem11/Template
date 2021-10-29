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
            if (_settingsHolder.SqlSettings.SecondaryDb.Enabled)
            {
                if (_settingsHolder.SqlSettings.SecondaryDb.Npgsql)
                {
                    optionsBuilder.UseNpgsql(_settingsHolder.SqlSettings.SecondaryDb.ConnectionString);
                    return;
                }
                
                optionsBuilder.UseSqlServer(_settingsHolder.SqlSettings.SecondaryDb.ConnectionString);
                return;
            }
            
            if (_settingsHolder.SqlSettings.PrimaryDb.Enabled)
            {
                if (_settingsHolder.SqlSettings.PrimaryDb.Mssql)
                {
                    optionsBuilder.UseSqlServer(_settingsHolder.SqlSettings.PrimaryDb.ConnectionString);
                    return;
                }
                
                optionsBuilder.UseNpgsql(_settingsHolder.SqlSettings.PrimaryDb.ConnectionString);
                return;
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