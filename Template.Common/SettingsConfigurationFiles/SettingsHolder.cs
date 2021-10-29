using AspNetCoreRateLimit;
using Microsoft.Extensions.Configuration;

namespace Template.Common.SettingsConfigurationFiles
{
    public class SettingsHolder
    {
        public IConfiguration Configuration { get; set; }
        public MyServices MyServices { get; set; } = new();
        public CacheSettings CacheSettings { get; set; } = new();
        public JwtSettings JwtSettings { get; set; } = new();
        public LoggingSettings LoggingSettings { get; set; } = new();
        public SqlSettings SqlSettings { get; set; } = new();
        public RedisSettings RedisSettings { get; set; } = new();
        public IpRateLimitOptions IpRateLimitOptions { get; set; } = new();
        public IpRateLimitPolicies IpRateLimitPolicies { get; set; } = new();
        public ClientRateLimitOptions ClientRateLimitOptions { get; set; } = new();
        public ClientRateLimitPolicies ClientRateLimitPolicies { get; set; } = new();
    }
}