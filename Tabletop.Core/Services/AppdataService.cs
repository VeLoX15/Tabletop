using DbController;
using Microsoft.Extensions.Configuration;
using Tabletop.Core.Interfaces;

namespace Tabletop.Core.Services
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

        public static string ConnectionString => _configuration?["ConnectionString"] ?? string.Empty;
        public static int PageLimit => _configuration?.GetValue<int>("PageLimit") ?? 30;

    }
}
