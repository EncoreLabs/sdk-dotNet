using AutoMapper;
using EncoreTickets.SDK.Basket.Models;
using EncoreTickets.SDK.Basket.Models.RequestModels;

namespace EncoreTickets.SDK.Utilities.Mapping.Profiles
{
    internal class BasketProfile : Profile
    {
        public BasketProfile()
        {
            AllowNullCollections = true;
            AllowNullDestinationValues = true;
            CreateMap<Seat, ItemRequest>();
            CreateMap<Reservation, ReservationRequest>();
            CreateMap<Basket.Models.Basket, UpsertBasketRequest>()
                .ForMember(b => b.HasFlexiTickets, src => src.MapFrom(b => b.AllowFlexiTickets));
        }
    }
}
