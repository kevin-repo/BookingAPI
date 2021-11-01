using System;

namespace BookingAPI.Requests
{
    public class BaseBookingRequest
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
