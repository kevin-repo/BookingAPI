namespace BookingAPI.Repositories.Queries
{
    public static class QueriesBooking
    {
        public const string GetByDates = @"SELECT * FROM Booking WHERE StartDate >= @StartDate AND EndDate <= @EndDate";
        public const string Get = @"SELECT * FROM Booking WHERE Id = @Id";
        public const string Delete = @"DELETE FROM Booking WHERE Id = @Id";
        public const string Add = @"INSERT INTO Booking (StartDate, EndDate, CreatedDate) VALUES (@StartDate, @EndDate, NOW());
            SELECT * FROM Booking WHERE Id = (SELECT CAST(SCOPE_IDENTITY() AS int));";
        public const string Update = @"UPDATE Booking SET StartDate = @StartDate, EndDate = @EndDate, ModifyDate = NOW() WHERE Id = @Id;
            SELECT * FROM Booking WHERE Id = @Id;";
    }
}
