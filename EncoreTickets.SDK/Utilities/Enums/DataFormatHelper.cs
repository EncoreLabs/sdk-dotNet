using System;
using RestSharp.Serialization;

namespace EncoreTickets.SDK.Utilities.Enums
{
    internal static class DataFormatHelper
    {
        public static string ToContentType(DataFormat dataFormat)
        {
            switch (dataFormat)
            {
                case DataFormat.Xml:
                    return ContentType.Xml;
                case DataFormat.Json:
                    return ContentType.Json;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataFormat), dataFormat, null);
            }
        }
    }
}
