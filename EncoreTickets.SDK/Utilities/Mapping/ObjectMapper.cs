using System;
using AutoMapper;
using EncoreTickets.SDK.Utilities.Mapping.Profiles;

namespace EncoreTickets.SDK.Utilities.Mapping
{
    internal static class ObjectMapper
    {
        private static readonly Lazy<IMapper> Mapper = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<CommonProfile>();
                cfg.AddProfile<BasketProfile>();
                cfg.AddProfile<InventoryProfile>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        });

        public static TTarget Map<TSource, TTarget>(this TSource source) => Mapper.Value.Map<TSource, TTarget>(source);
    }
}
