using BookingAPI.Contracts.Requests;
using BookingAPI.Contracts.Responses;
using System;
using System.Threading.Tasks;

namespace BookingAPI.Services
{
    public interface IBookingService
    {
        Task<BookingResponse> Get(int id);
        Task<bool> CheckBookingAvailibility(DateTime startDate, DateTime endDate);
        Task<BookingResponse> CreateBooking(BookingCreationRequest request);
        Task<bool> DeleteBooking(int id);
        Task<BookingResponse> ModifyBooking(int id, BookingModifyRequest request);
    }
}
