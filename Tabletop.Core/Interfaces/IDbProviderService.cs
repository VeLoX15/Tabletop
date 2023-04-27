using DbController;

namespace Tabletop.Core.Interfaces
{
    public interface IDbProviderService
    {
        IDbController GetDbController(string connectionString);
    }
}