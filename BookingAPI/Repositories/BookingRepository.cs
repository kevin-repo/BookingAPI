using BookingAPI.Entities;
using BookingAPI.Repositories.Providers;
using BookingAPI.Repositories.Queries;
using Dapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingAPI.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly IConnectionProvider _provider;

        public BookingRepository(IConnectionProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider)); ;
        }

        public Task<Booking> Add(Booking addBooking)
        {
            using (var connection = _provider.CreateConnection())
            {
                return connection.QuerySingleAsync<Booking>(QueriesBooking.Add, addBooking);
            }
        }

        public Task<int> Delete(int id)
        {
            using (var connection = _provider.CreateConnection())
            {
                return connection.ExecuteAsync(QueriesBooking.Delete, new { @Id = id });
            }
        }

        public Task<Booking> Get(int id)
        {
            using (var connection = _provider.CreateConnection())
            {
                return connection.QueryFirstAsync<Booking>(QueriesBooking.Get, new { @Id = id });
            }
        }

        public Task<IEnumerable<Booking>> GetBookingByDates(DateTime startDate, DateTime endDate)
        {
            using (var connection = _provider.CreateConnection())
            {
                return connection.QueryAsync<Booking>(QueriesBooking.GetByDates, new { @StartDate = startDate, @EndDate = endDate });
            }
        }

        public Task<Booking> Update(Booking updateBooking)
        {
            using (var connection = _provider.CreateConnection())
            {
                return connection.QuerySingleAsync<Booking>(QueriesBooking.Update, updateBooking);
            }
        }
    }
}
