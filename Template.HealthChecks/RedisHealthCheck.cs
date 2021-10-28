using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;
using Template.Common.SettingsConfigurationFiles;
using Template.Data;

namespace Template.HealthChecks
{
    public class RedisHealthCheck : IHealthCheck
    {
        private readonly SettingsHolder _settingsHolder;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        public RedisHealthCheck(SettingsHolder settingsHolder, IConnectionMultiplexer connectionMultiplexer)
        {
            _settingsHolder = settingsHolder;
            _connectionMultiplexer = connectionMultiplexer;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            if (!_settingsHolder.RedisSettings.Enabled)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy("Is not enabled"));
            }
            try
            {
                var db = _connectionMultiplexer.GetDatabase();
                db.StringGet("");
                
                return Task.FromResult(HealthCheckResult.Healthy());
            }
            catch (Exception e)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy(e.Message));
            }
        }
    }

    public class DbContextHealthCheck : IHealthCheck
    {
        private readonly TemplateContext _context;
        public DbContextHealthCheck(TemplateContext context)
        {
            _context = context;
        }
        
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                await _context.Books.FirstOrDefaultAsync(cancellationToken);
                return await Task.FromResult(HealthCheckResult.Healthy());
            }
            catch (Exception e)
            {
                return await Task.FromResult(HealthCheckResult.Unhealthy(e.Message));
            }
        }
    }
}