using BookingAPI.Entities;
using BookingAPI.Requests;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingAPI.Repositories
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> GetBookingByDates(DateTime startDate, DateTime endDate);
        Task<Booking> Add(Booking addBooking);
        Task<Booking> Get(int id);
        Task<int> Delete(int id);
        Task<Booking> Update(Booking updateBooking);
    }
}
