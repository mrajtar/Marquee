using Marquee.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Marquee.Infrastructure.BackgroundJobs;

public class TrendingScoreUpdaterService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<TrendingScoreUpdaterService> _logger;

    public TrendingScoreUpdaterService(IServiceScopeFactory scopeFactory, ILogger<TrendingScoreUpdaterService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(30));

        do
        {
            _logger.LogInformation("Starting background calculation for Trending Scores...");

            try
            {
                using var scope = _scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<MarqueeDbContext>();

                var since = DateTime.UtcNow.AddDays(-14);

                var rowsAffected = await dbContext.Database.ExecuteSqlInterpolatedAsync($@"
                    UPDATE [Media]
                    SET [TrendingScore] = (
                        (SELECT COUNT(*) FROM [Ratings] WHERE [MediaId] = [Media].[Id] AND [CreatedAt] >= {since}) * 3.0 +
                        (SELECT COUNT(*) FROM [Reviews] WHERE [MediaId] = [Media].[Id] AND [CreatedAt] >= {since}) * 6.0 +
                        (SELECT COUNT(*) FROM [MediaInteractions] WHERE [MediaId] = [Media].[Id] AND [CreatedAt] >= {since}) * 2.0 +
                        (SELECT COUNT(*) FROM [Reviews] r INNER JOIN [ReviewLikes] rl ON r.[Id] = rl.[ReviewId] WHERE r.[MediaId] = [Media].[Id] AND rl.[CreatedAt] >= {since}) * 1.5 +
                        (CASE
                            WHEN [ReleaseDate] IS NOT NULL AND DATEDIFF(day, [ReleaseDate], GETUTCDATE()) BETWEEN 0 AND 30 
                            THEN 15.0 * (1.0 - (CAST(DATEDIFF(day, [ReleaseDate], GETUTCDATE()) AS float) / 30.0))
                            WHEN [ReleaseDate] IS NOT NULL AND DATEDIFF(day, [ReleaseDate], GETUTCDATE()) < 0 
                            THEN 15.0 
                            ELSE 0.0 
                    END)
                )", stoppingToken);

                _logger.LogInformation("Successfully updated Trending Scores for {Count} media items.", rowsAffected);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating trending scores.");
            }
        } while (await timer.WaitForNextTickAsync(stoppingToken));
    }
}