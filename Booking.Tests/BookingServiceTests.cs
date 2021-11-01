using AutoFixture;
using AutoMapper;
using BookingAPI.Contracts.Requests;
using BookingAPI.Contracts.Responses;
using BookingAPI.Exceptions;
using BookingAPI.Repositories;
using BookingAPI.Services;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Booking.Tests
{
    public class BookingServiceTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly BookingService _sut;

        public BookingServiceTests()
        {
            _fixture = new Fixture();
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _mapperMock = new Mock<IMapper>();
            _sut = new BookingService(_bookingRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public void CheckBookingAvailibility_NoExistingBookings_ReturnsTrue()
        {
            var startDate = _fixture.Create<DateTime>();
            var endDate = _fixture.Create<DateTime>();
            var bookings = new List<BookingAPI.Entities.Booking>();

            _bookingRepositoryMock.Setup(b => b.GetBookingByDates(startDate, endDate)).ReturnsAsync(bookings);

            var actual = _sut.CheckBookingAvailibility(startDate, endDate).Result;

            actual.Should().BeTrue();
        }

        [Fact]
        public void CheckBookingAvailibility_ExistingBookings_ReturnsFalse()
        {
            var startDate = _fixture.Create<DateTime>();
            var endDate = _fixture.Create<DateTime>();
            var bookings = _fixture.CreateMany<BookingAPI.Entities.Booking>(1);

            _bookingRepositoryMock.Setup(b => b.GetBookingByDates(startDate, endDate)).ReturnsAsync(bookings);

            var actual = _sut.CheckBookingAvailibility(startDate, endDate).Result;

            actual.Should().BeFalse();
        }

        [Fact]
        public void CreateBooking_CanCreateFalse_ReturnsException()
        {
            var startDate = _fixture.Create<DateTime>();
            var endDate = _fixture.Create<DateTime>();
            var request = _fixture.Build<BookingCreationRequest>()
                .With(r => r.StartDate, startDate)
                .With(r => r.EndDate, endDate)
                .Create();
            var bookings = _fixture.CreateMany<BookingAPI.Entities.Booking>(1);

            _bookingRepositoryMock.Setup(b => b.GetBookingByDates(startDate, endDate)).ReturnsAsync(bookings);

            try
            {
                var actual = _sut.CreateBooking(request).Result;
            }
            catch (Exception ex)
            {
                ex.InnerException.GetType().Should().Be(typeof(BookingDatesNotAvailableException));
            }
        }

        [Fact]
        public void CreateBooking_CanCreateTrue_ReturnsBookingResponse()
        {
            var startDate = _fixture.Create<DateTime>();
            var endDate = _fixture.Create<DateTime>();
            var request = _fixture.Build<BookingCreationRequest>()
                .With(r => r.StartDate, startDate)
                .With(r => r.EndDate, endDate)
                .Create();
            var bookings = _fixture.CreateMany<BookingAPI.Entities.Booking>(1);
            var addBooking = _fixture.Create<BookingAPI.Entities.Booking>();
            var createdBooking = _fixture.Create<BookingAPI.Entities.Booking>();
            var createdBookingResponse = _fixture.Create<BookingResponse>();
            
            _bookingRepositoryMock.Setup(b => b.GetBookingByDates(startDate, endDate)).ReturnsAsync(new List<BookingAPI.Entities.Booking>());
            _mapperMock.Setup(m => m.Map<BookingAPI.Entities.Booking>(request)).Returns(addBooking);
            _mapperMock.Setup(m => m.Map<BookingResponse>(createdBooking)).Returns(createdBookingResponse);
            _bookingRepositoryMock.Setup(b => b.Add(addBooking)).ReturnsAsync(createdBooking);

            var actual = _sut.CreateBooking(request).Result;

            _bookingRepositoryMock.Verify(b => b.Add(addBooking), Times.Once);

            actual.CreationDate.Should().Be(createdBookingResponse.CreationDate);
            actual.EndDate.Should().Be(createdBookingResponse.EndDate);
            actual.Id.Should().Be(createdBookingResponse.Id);
            actual.ModifyDate.Should().Be(createdBookingResponse.ModifyDate);
            actual.StartDate.Should().Be(createdBookingResponse.StartDate);
        }

        [Fact]
        public void Get_ReturnsBookingResponse()
        {
            var contextBooking = _fixture.Create<BookingAPI.Entities.Booking>();
            var createdBookingResponse = _fixture.Create<BookingResponse>();

            _bookingRepositoryMock.Setup(b => b.Get(It.IsAny<int>())).ReturnsAsync(contextBooking);
            _mapperMock.Setup(m => m.Map<BookingResponse>(contextBooking)).Returns(createdBookingResponse);

            var actual = _sut.Get(_fixture.Create<int>()).Result;

            _bookingRepositoryMock.Verify(b => b.Get(It.IsAny<int>()), Times.Once);

            actual.CreationDate.Should().Be(createdBookingResponse.CreationDate);
            actual.EndDate.Should().Be(createdBookingResponse.EndDate);
            actual.Id.Should().Be(createdBookingResponse.Id);
            actual.ModifyDate.Should().Be(createdBookingResponse.ModifyDate);
            actual.StartDate.Should().Be(createdBookingResponse.StartDate);
        }

        [Fact]
        public void DeleteBooking_ReturnsTrue()
        {
            var contextBooking = _fixture.Create<BookingAPI.Entities.Booking>();
            var createdBookingResponse = _fixture.Create<BookingResponse>();

            _bookingRepositoryMock.Setup(b => b.Delete(It.IsAny<int>())).ReturnsAsync(1);

            var actual = _sut.DeleteBooking(_fixture.Create<int>()).Result;

            _bookingRepositoryMock.Verify(b => b.Delete(It.IsAny<int>()), Times.Once);

            actual.Should().BeTrue();
        }

        [Fact]
        public void DeleteBooking_ReturnsFalse()
        {
            var contextBooking = _fixture.Create<BookingAPI.Entities.Booking>();
            var createdBookingResponse = _fixture.Create<BookingResponse>();

            _bookingRepositoryMock.Setup(b => b.Delete(It.IsAny<int>())).ReturnsAsync(0);

            var actual = _sut.DeleteBooking(_fixture.Create<int>()).Result;

            _bookingRepositoryMock.Verify(b => b.Delete(It.IsAny<int>()), Times.Once);

            actual.Should().BeFalse();
        }

        [Fact]
        public void ModifyBooking_CanCreateFalse_ReturnsException()
        {
            var startDate = _fixture.Create<DateTime>();
            var endDate = _fixture.Create<DateTime>();
            var request = _fixture.Build<BookingModifyRequest>()
                .With(r => r.StartDate, startDate)
                .With(r => r.EndDate, endDate)
                .Create();
            var bookings = _fixture.CreateMany<BookingAPI.Entities.Booking>(1);

            _bookingRepositoryMock.Setup(b => b.GetBookingByDates(startDate, endDate)).ReturnsAsync(bookings);

            try
            {
                var actual = _sut.ModifyBooking(_fixture.Create<int>(), request).Result;
            }
            catch (Exception ex)
            {
                ex.InnerException.GetType().Should().Be(typeof(BookingDatesNotAvailableException));
            }
        }

        [Fact]
        public void ModifyBooking_CanCreateTrue_ReturnsBookingResponse()
        {
            var startDate = _fixture.Create<DateTime>();
            var endDate = _fixture.Create<DateTime>();
            var request = _fixture.Build<BookingModifyRequest>()
                .With(r => r.StartDate, startDate)
                .With(r => r.EndDate, endDate)
                .Create();
            var bookings = _fixture.CreateMany<BookingAPI.Entities.Booking>(1);
            var updateBooking = _fixture.Create<BookingAPI.Entities.Booking>();
            var modifyBooking = _fixture.Create<BookingAPI.Entities.Booking>();
            var modifyBookingResponse = _fixture.Create<BookingResponse>();

            _bookingRepositoryMock.Setup(b => b.GetBookingByDates(startDate, endDate)).ReturnsAsync(new List<BookingAPI.Entities.Booking>());
            _mapperMock.Setup(m => m.Map<BookingAPI.Entities.Booking>(request)).Returns(updateBooking);
            _mapperMock.Setup(m => m.Map<BookingResponse>(modifyBooking)).Returns(modifyBookingResponse);
            _bookingRepositoryMock.Setup(b => b.Update(updateBooking)).ReturnsAsync(modifyBooking);

            var id = _fixture.Create<int>();
            var actual = _sut.ModifyBooking(id, request).Result;

            _bookingRepositoryMock.Verify(b => b.Update(updateBooking), Times.Once);

            actual.CreationDate.Should().Be(modifyBookingResponse.CreationDate);
            actual.EndDate.Should().Be(modifyBookingResponse.EndDate);
            actual.Id.Should().Be(modifyBookingResponse.Id);
            actual.ModifyDate.Should().Be(modifyBookingResponse.ModifyDate);
            actual.StartDate.Should().Be(modifyBookingResponse.StartDate);
        }
    }
}
