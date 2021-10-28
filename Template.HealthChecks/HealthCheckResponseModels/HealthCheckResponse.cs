using System;
using System.Collections.Generic;

namespace Template.HealthChecks.HealthCheckResponseModels
{
    public class HealthCheckResponse
    {
        public string Status { get; set; }
        public List<HealthCheck> HealthChecks { get; set; }
        public TimeSpan Duration { get; set; }
    }
}