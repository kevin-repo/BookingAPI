using System;
using System.ComponentModel.DataAnnotations;

namespace BookingAPI.Entities
{
    public class Booking
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ModifyDate { get; set; }
    }
}
