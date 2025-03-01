
using Microsoft.Extensions.DependencyInjection;

namespace IpValidation.Services
{
    public class TemporaryCleanupBlockingCountries : BackgroundService
    {
        private readonly ILogger<TemporaryCleanupBlockingCountries> logger;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public TemporaryCleanupBlockingCountries(ILogger<TemporaryCleanupBlockingCountries> logger, IServiceScopeFactory serviceScopeFactory)
        {
            this.logger = logger;
            this.serviceScopeFactory = serviceScopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    // to make autounbblockservices scoped
                    var blockService = scope.ServiceProvider.GetRequiredService<AutoUnblockServices>();
                    blockService.RemoveExpiredBlocks();
                    logger.LogInformation("Expired country blocks removed at {Time}", DateTime.Now);
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}
