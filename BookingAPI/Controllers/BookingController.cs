using BookingAPI.Constants.ErrorMessages;
using BookingAPI.Contracts.Requests;
using BookingAPI.Contracts.Responses;
using BookingAPI.Exceptions;
using BookingAPI.Routes;
using BookingAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BookingAPI.Controllers
{
    
    [ApiController]
    [Route("api/booking")]
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService ?? throw new ArgumentNullException(nameof(bookingService));
        }

        [HttpGet]
        [ResponseCache(Duration = 1000, VaryByQueryKeys = new string[] { "format" })]
        [Route(RouteConstants.CheckAvailibility)]
        public async Task<ActionResult<bool>> CheckAvailibilityAsync([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate == null || endDate == null)
            {
                return BadRequest();
            }

            var isAvailable = await _bookingService.CheckBookingAvailibility(startDate, endDate);
            return Ok(isAvailable);
        }

        [HttpPost]
        public async Task<ActionResult<BookingResponse>> CreateBooking([FromBody] BookingCreationRequest request) 
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            try
            {
                var createdBooking = await _bookingService.CreateBooking(request);
                return Ok(createdBooking);
            }
            catch (BookingDatesNotAvailableException)
            {
                return BadRequest(ErrorMessages.DatesNotAvailable);
            }                    
        }

        [HttpDelete]
        public async Task<ActionResult<BookingResponse>> DeleteBooking(int id)
        {
            var existingBooking = await _bookingService.Get(id);
            if (existingBooking == null)
            {
                return NotFound();
            }

            var success = await _bookingService.DeleteBooking(id);
            if (!success)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult<BookingResponse>> ModifyBooking(int id, [FromBody] BookingModifyRequest request)
        {
            var existingBooking = await _bookingService.Get(id);
            if (existingBooking == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            try
            {
                var updatedBooking = await _bookingService.ModifyBooking(id, request);
                return Ok(updatedBooking);
            }
            catch (BookingDatesNotAvailableException)
            {
                return BadRequest(ErrorMessages.DatesNotAvailable);
            }
        }
    }
}
