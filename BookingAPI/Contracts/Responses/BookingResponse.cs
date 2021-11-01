using System;

namespace BookingAPI.Contracts.Responses
{
    public class BookingResponse
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ModifyDate { get; set; }
    }
}
