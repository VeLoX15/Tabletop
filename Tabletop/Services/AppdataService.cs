using DbController;
using Tabletop.Interfaces;

namespace Tabletop.Services
{
    public static class AppdatenService
    {
        private static IConfiguration? _configuration;

        public static async Task InitAsync(IConfiguration configuration, IDbController dbProviderService)
        {
            _configuration = configuration;
            using IDbController dbController = dbProviderService.GetDbController(ConnectionString);
        }
        public static string ConnectionString => _configuration?.GetConnectionString("Default") ?? string.Empty;

    }
}
