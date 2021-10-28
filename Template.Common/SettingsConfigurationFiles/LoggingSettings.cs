using System.Text.Json.Serialization;

namespace Template.Common.SettingsConfigurationFiles
{
    public class LoggingSettings
    {
        public LogLevelSettings LogLevel { get; set; }
        public class LogLevelSettings
        {
            public string Default { get; set; }

            public string Microsoft { get; set; }

            [JsonPropertyName("Microsoft.Hosting.Lifetime")]
            public string Microsoft_Hosting_Lifetime { get; set; }
        }
    }
}