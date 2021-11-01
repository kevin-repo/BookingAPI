using BookingAPI.Constants.ErrorMessages;
using BookingAPI.Requests;
using FluentValidation;
using System;

namespace BookingAPI.Validators
{
    public class BaseRequestValidator<T> : AbstractValidator<T> where T : BaseBookingRequest
    {
        public BaseRequestValidator()
        {
            RuleFor(b => b.StartDate)
                .NotEmpty();

            RuleFor(b => b.EndDate)
                .NotEmpty();

            RuleFor(b => b.StartDate.Date)
                .GreaterThan(DateTime.Now.Date)
                .WithMessage(ErrorMessages.StartDateMustBeGreaterThanToday);

            RuleFor(b => b.StartDate.Date.AddDays(3).Date)
                .GreaterThanOrEqualTo(b => b.EndDate.Date)
                .WithMessage(ErrorMessages.BookingNoLongerThanThreeDays);

            RuleFor(b => b.StartDate.Date)
                .LessThanOrEqualTo(DateTime.Now.AddDays(30).Date)
                .WithMessage(ErrorMessages.BookingMustBeLessOrEqualThanThirtyDaysInAdvance);
            
            RuleFor(b => b.EndDate.Date)
                .LessThanOrEqualTo(DateTime.Now.AddDays(30).Date)
                .WithMessage(ErrorMessages.BookingMustBeLessOrEqualThanThirtyDaysInAdvance);

            RuleFor(b => b.StartDate)
                .LessThan(b => b.EndDate)
                .WithMessage(ErrorMessages.StartDateMustBeGreaterThanEndDate);
        }
    }
}
