namespace Template.Common.SettingsConfigurationFiles
{
    public class SqlSettings
    {
        public bool Mssql { get; set; }
        public bool Npgsql { get; set; }
        public string MssqlConnectionString { get; set; }
        public string NpgsqlConnectionString { get; set; }
    }
}