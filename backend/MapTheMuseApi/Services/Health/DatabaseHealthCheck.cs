using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using MapTheMuseApi.Data;

public sealed class DatabaseHealthCheck : IHealthCheck
{
    private readonly MapTheMuseContext _db;

    public DatabaseHealthCheck(MapTheMuseContext db) => _db = db;

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Lightweight connectivity probe
            await _db.Database.ExecuteSqlRawAsync("SELECT 1", cancellationToken);
            return HealthCheckResult.Healthy("DB reachable");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("DB unreachable", ex);
        }
    }
}
