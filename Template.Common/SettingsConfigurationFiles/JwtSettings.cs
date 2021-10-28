using System;

namespace Template.Common.SettingsConfigurationFiles
{
    public class JwtSettings
    {
        public string Secret { get; set; }
        public TimeSpan TokenLifetime { get; set; }
        public TimeSpan RefreshTokenLifeTime { get; set; }
    }
}