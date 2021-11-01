using System.Data;

namespace BookingAPI.Repositories.Providers
{
    public interface IConnectionProvider
    {
        IDbConnection CreateConnection();
    }
}
