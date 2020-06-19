using EncoreTickets.SDK.Payment.Models;
using EncoreTickets.SDK.Utilities.Serializers;
using EncoreTickets.SDK.Utilities.Serializers.Converters;

namespace EncoreTickets.SDK.Payment.Serializers
{
    internal class JsonResponseToOrderDeserializer : DefaultJsonSerializer
    {
        public JsonResponseToOrderDeserializer()
            : base(new[] { new SingleOrListToSingleConverter<RiskData>() })
        {
        }
    }
}
