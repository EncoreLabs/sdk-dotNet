using System;
using AutoMapper;
using EncoreTickets.SDK.Utilities.Enums;
using RestSharp;
using DataFormat = EncoreTickets.SDK.Utilities.Enums.DataFormat;

namespace EncoreTickets.SDK.Utilities.Mapping.Profiles
{
    internal class CommonProfile : Profile
    {
        public CommonProfile()
        {
            AllowNullCollections = true;
            AllowNullDestinationValues = true;
            CreateMap<RequestMethod, Method>().ConvertUsing(source => ConvertRequestMethod(source));
            CreateMap<DataFormat, RestSharp.DataFormat>().ConvertUsing(source => ConvertDataFormat(source));
        }

        private Method ConvertRequestMethod(RequestMethod source)
        {
            switch (source)
            {
                case RequestMethod.Get:
                    return Method.GET;
                case RequestMethod.Post:
                    return Method.POST;
                case RequestMethod.Put:
                    return Method.PUT;
                case RequestMethod.Patch:
                    return Method.PATCH;
                case RequestMethod.Delete:
                    return Method.DELETE;
                default:
                    throw new ArgumentOutOfRangeException(nameof(source), source, null);
            }
        }

        private RestSharp.DataFormat ConvertDataFormat(DataFormat source)
        {
            switch (source)
            {
                case DataFormat.Xml:
                    return RestSharp.DataFormat.Xml;
                case DataFormat.Json:
                    return RestSharp.DataFormat.Json;
                default:
                    throw new ArgumentOutOfRangeException(nameof(source), source, null);
            }
        }
    }
}
