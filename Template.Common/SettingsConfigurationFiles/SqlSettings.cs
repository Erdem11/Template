namespace Template.Common.SettingsConfigurationFiles
{
    public class SqlSettings
    {
        // Main data store
        public MyDbOptions PrimaryDb { get; set; }

        // Additional data store
        public MyDbOptions SecondaryDb { get; set; }

        // Additional data store
        public MyDbOptions AlternateDb { get; set; }

        public class MyDbOptions
        {
            public bool Enabled { get; set; }
            public DbTypes DbType { get; set; }
            public string ConnectionString { get; set; }
        }

        public MyDbOptions GetAlternate() =>
            AlternateDb.Enabled ? AlternateDb : GetSecondary();
        public MyDbOptions GetSecondary() =>
            SecondaryDb.Enabled ? SecondaryDb : PrimaryDb;
        public MyDbOptions GetPrimary() =>
            PrimaryDb.Enabled ? PrimaryDb : SecondaryDb;
    }
    
    public enum DbTypes
    {
        Mssql = 1,
        Npgsql = 2,
        Sqlite = 3,
    }
}