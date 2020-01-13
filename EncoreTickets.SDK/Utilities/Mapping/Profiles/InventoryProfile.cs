using AutoMapper;
using EncoreTickets.SDK.Inventory.Extensions;

namespace EncoreTickets.SDK.Utilities.Mapping.Profiles
{
    internal class InventoryProfile : Profile
    {
        public InventoryProfile()
        {
            AllowNullCollections = true;
            AllowNullDestinationValues = true;
            CreateMap<string, ProductType>().ConvertUsing(source => ConvertStringToProductType(source));
        }

        private static ProductType ConvertStringToProductType(string source)
        {
            switch (source?.ToLower())
            {
                case "show":
                    return ProductType.Show;
                case "attraction":
                    return ProductType.Attraction;
                case "event":
                    return ProductType.Event;
                default:
                    return string.IsNullOrWhiteSpace(source)
                        ? ProductType.NotSet
                        : ProductType.Other;
            }
        }
    }
}