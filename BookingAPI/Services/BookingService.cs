using AutoMapper;
using BookingAPI.Contracts.Requests;
using BookingAPI.Contracts.Responses;
using BookingAPI.Entities;
using BookingAPI.Exceptions;
using BookingAPI.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAPI.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookRepository;
        private readonly IMapper _mapper;


        public BookingService(IBookingRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Create a booking.
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Booking if create success.</returns>
        public async Task<BookingResponse> CreateBooking(BookingCreationRequest request)
        {
            var canCreate = await CheckBookingAvailibility(request.StartDate, request.EndDate);
            if (!canCreate)
            {
                throw new BookingDatesNotAvailableException();
            }

            var addBooking = _mapper.Map<Booking>(request);
            var createdBooking = await _bookRepository.Add(addBooking);
            return _mapper.Map<BookingResponse>(createdBooking);
        }

        /// <summary>
        /// Check if there is no booking according to dates.
        /// </summary>
        /// <param name="startDate">Booking start date</param>
        /// <param name="endDate">Booking end date</param>
        /// <returns>True if the room is available, else false.</returns>
        public async Task<bool> CheckBookingAvailibility(DateTime startDate, DateTime endDate)
        {
            var bookings = await _bookRepository.GetBookingByDates(startDate, endDate);
            return !bookings.Any();
        }

        /// <summary>
        /// Get booking according to id.
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Booking.</returns>
        public async Task<BookingResponse> Get(int id)
        {
            var booking = await _bookRepository.Get(id);
            return _mapper.Map<BookingResponse>(booking);
        }

        /// <summary>
        /// Delete a booking.
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>True if deleted success.</returns>
        public async Task<bool> DeleteBooking(int id)
        {
            var deletedRow = await _bookRepository.Delete(id);
            return deletedRow == 1;
        }

        /// <summary>
        /// Modify a booking.
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="request">Request</param>
        /// <returns>Booking if update success.</returns>
        public async Task<BookingResponse> ModifyBooking(int id, BookingModifyRequest request)
        {
            var canModify = await CheckBookingAvailibility(request.StartDate, request.EndDate);
            if (!canModify)
            {
                throw new BookingDatesNotAvailableException();
            }

            var updateBooking = _mapper.Map<Booking>(request);
            updateBooking.Id = id;
            var updatedBooking = await _bookRepository.Update(updateBooking);
            return _mapper.Map<BookingResponse>(updatedBooking);
        }
    }
}
