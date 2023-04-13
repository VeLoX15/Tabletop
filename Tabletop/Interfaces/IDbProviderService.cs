using DbController;

namespace Tabletop.Interfaces
{
    public interface IDbProviderService
    {
        IDbController GetDbController(string connectionString);
    }
}