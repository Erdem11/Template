using Microsoft.Extensions.Configuration;

namespace Template.Common.SettingsConfigurationFiles
{
    public class SettingsHolder
    {
        public IConfiguration Configuration { get; set; }
        public CacheSettings CacheSettings { get; set; } = new();
        public JwtSettings JwtSettings { get; set; } = new();
        public LoggingSettings LoggingSettings { get; set; } = new();
        public MsSqlSettings MsSqlSettings { get; set; } = new();
        public RedisSettings RedisSettings { get; set; } = new();
    }
}