using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace BookingAPI.Repositories.Providers
{
    public class ConnectionProvider : IConnectionProvider
    {
        private readonly IConfiguration _configuration;

        public ConnectionProvider(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IDbConnection CreateConnection()
        {
            var connectionString = new SqlConnectionStringBuilder
            {
                DataSource = _configuration["Database:Source"],
                UserID = _configuration["Database:User"],
                Password = _configuration["Database:Password"],
                InitialCatalog = _configuration["Database:Name"],
                IntegratedSecurity = bool.Parse(_configuration["Database:IntegratedSecurity"])
            };

            return new SqlConnection(connectionString.ConnectionString);
        }
    }
}
