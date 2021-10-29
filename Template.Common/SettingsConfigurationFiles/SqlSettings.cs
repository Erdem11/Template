namespace Template.Common.SettingsConfigurationFiles
{
    public class SqlSettings
    {
        // Main data store
        public MyDbOptions PrimaryDb { get; set; }

        // Additional data store
        public MyDbOptions SecondaryDb { get; set; }

        public class MyDbOptions
        {
            public bool Enabled { get; set; }
            public bool Mssql { get; set; }
            public bool Npgsql { get; set; }
            public string ConnectionString { get; set; }
        }
    }
}