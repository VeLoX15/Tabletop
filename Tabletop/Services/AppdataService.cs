using DbController;
using Tabletop.Interfaces;

namespace Tabletop.Services
{
    public static class AppdatenService
    {
        private static IConfiguration? _configuration;

        public static Task Init(IConfiguration configuration, IDbProviderService dbProviderService)
        {
            _configuration = configuration;
            using IDbController dbController = dbProviderService.GetDbController(ConnectionString);
            return Task.CompletedTask;
        }

        public static string ConnectionString => _configuration?.GetConnectionString("Default") ?? string.Empty;

    }
}
