using AutoMapper;
using BookingAPI.Contracts.Requests;
using BookingAPI.Contracts.Responses;
using BookingAPI.Entities;

namespace BookingAPI.Profiles
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<BookingCreationRequest, Booking>();
            CreateMap<BookingModifyRequest, Booking>();
            CreateMap<Booking, BookingResponse>();
        }
    }
}
