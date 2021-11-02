using System;
using System.Net.Http;
using System.Threading.Tasks;
using Template.Common.SettingsConfigurationFiles;

namespace Template.BackgroundTasks
{
    public class HangfireCronJobs
    {
        private readonly SettingsHolder _settingsHolder;
        public HangfireCronJobs(SettingsHolder settingsHolder)
        {
            _settingsHolder = settingsHolder;
        }

        public async Task UnPauseApp()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(_settingsHolder.MyServices.BaseUri);
            var response = await client.GetAsync("/");
        }
    }
}