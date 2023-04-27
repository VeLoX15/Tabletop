using DbController.MySql;
using DbController;

namespace Tabletop.Core.Interfaces
{
    public sealed class MySqlProviderService : IDbProviderService
    {
        public IDbController GetDbController(string connectionString) => new MySqlController(connectionString);
    }
}
