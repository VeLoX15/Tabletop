using DbController.MySql;
using DbController;

namespace Tabletop.Interfaces
{
    public sealed class MySqlProviderService : IDbProviderService
    {
        public IDbController GetDbController(string connectionString) => new MySqlController(connectionString);
    }
}
