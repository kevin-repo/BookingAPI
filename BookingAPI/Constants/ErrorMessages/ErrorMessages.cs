namespace BookingAPI.Constants.ErrorMessages
{
    public static class ErrorMessages
    {
        public const string StartDateMustBeGreaterThanEndDate = "Start date must be greater than end date";
        public const string StartDateMustBeGreaterThanToday = "Start date must be greater than today";
        public const string BookingNoLongerThanThreeDays = "Booking can't be longer than 3 days";
        public const string BookingMustBeLessOrEqualThanThirtyDaysInAdvance = "Booking must be less or equal than 30 days in advance";
        public const string DatesNotAvailable = "Booking dates are not available";
    }
}
