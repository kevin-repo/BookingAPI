using AutoFixture;
using BookingAPI.Contracts.Requests;
using BookingAPI.Validators;
using FluentAssertions;
using System;
using Xunit;

namespace Booking.Tests
{
    public class BookingCreationRequestValidatorTests
    {
        private readonly Fixture _fixture;
        private readonly BookingCreationRequestValidator _sut;

        public BookingCreationRequestValidatorTests()
        {
            _fixture = new Fixture();
            _sut = new BookingCreationRequestValidator();
        }

        [Fact]
        public void Validate_StartDate_Null_ReturnsInvalid()
        {
            var request = _fixture.Build<BookingCreationRequest>()
                .Without(r => r.StartDate)
                .Create();

            var actual = _sut.Validate(request);

            actual.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Validate_EndDate_Null_ReturnsInvalid()
        {
            var request = _fixture.Build<BookingCreationRequest>()
                .Without(r => r.EndDate)
                .Create();

            var actual = _sut.Validate(request);

            actual.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Validate_StartDateLessToday_ReturnsInvalid()
        {
            var request = _fixture.Build<BookingCreationRequest>()
                .With(r => r.StartDate, DateTime.Now)
                .Create();

            var actual = _sut.Validate(request);

            actual.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Validate_StartDateGreaterThanEndDate_ReturnsInvalid()
        {
            var request = _fixture.Build<BookingCreationRequest>()
                .With(r => r.StartDate, DateTime.Now.AddDays(2))
                .With(r => r.EndDate, DateTime.Now.AddDays(1))
                .Create();

            var actual = _sut.Validate(request);

            actual.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Validate_BookingDaysGreaterThanThreeDays_ReturnsInvalid()
        {
            var request = _fixture.Build<BookingCreationRequest>()
                .With(r => r.StartDate, DateTime.Now.AddDays(1))
                .With(r => r.EndDate, DateTime.Now.AddDays(5))
                .Create();

            var actual = _sut.Validate(request);

            actual.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Validate_StartDateOverThirtyDays_ReturnsInvalid()
        {
            var request = _fixture.Build<BookingCreationRequest>()
                .With(r => r.StartDate, DateTime.Now.AddDays(31))
                .With(r => r.EndDate, DateTime.Now.AddDays(1))
                .Create();

            var actual = _sut.Validate(request);

            actual.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Validate_EndDateOverThirtyDays_ReturnsInvalid()
        {
            var request = _fixture.Build<BookingCreationRequest>()
                .With(r => r.StartDate, DateTime.Now.AddDays(1))
                .With(r => r.EndDate, DateTime.Now.AddDays(31))
                .Create();

            var actual = _sut.Validate(request);

            actual.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Validate_Success()
        {
            var request = _fixture.Build<BookingCreationRequest>()
                .With(r => r.StartDate, DateTime.Now.AddDays(1))
                .With(r => r.EndDate, DateTime.Now.AddDays(3))
                .Create();

            var actual = _sut.Validate(request);

            actual.IsValid.Should().BeTrue();
        }
    }
}
