namespace EncoreTickets.SDK.Utilities.Common.Serializers
{
    class SingleOrListJsonSerializer<T> : DefaultJsonSerializer
    {
        protected override Newtonsoft.Json.JsonSerializer Serializer
        {
            get
            {
                var serializer = base.Serializer;
                serializer.Converters.Add(new SingleOrListConverter<T>());
                return serializer;
            }
        }
    }
}
