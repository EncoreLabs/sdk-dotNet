using System;
using AutoMapper;

namespace EncoreTickets.SDK.Utilities.Common.Mapping
{
    internal static class ObjectMapper
    {
        private static readonly Lazy<IMapper> Mapper = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<BasketProfile>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        });

        public static TTarget Map<TSource, TTarget>(this TSource source) => Mapper.Value.Map<TSource, TTarget>(source);
    }
}
