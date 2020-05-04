using System;
using EncoreTickets.SDK.Utilities.BaseTypesExtensions;
using EncoreTickets.SDK.Venue.Models;
using Attribute = EncoreTickets.SDK.Venue.Models.Attribute;

namespace EncoreTickets.SDK.Venue.Extensions
{
    public static class AttributeExtension
    {
        private const string ExtraAttributeTitle = "Other";

        public static bool IsValid(this Attribute attribute)
        {
            return attribute != null &&
                   !string.IsNullOrEmpty(attribute.Title) &&
                   !string.IsNullOrEmpty(attribute.Description) &&
                   EnumExtension.GetEnumValues<Intention>().Contains(attribute.Intention);
        }

        public static Attribute CreateExtraAttribute(string description, string intentionAsStr)
        {
            try
            {
                var intention = EnumExtension.GetEnumFromString<Intention>(intentionAsStr);
                return CreateExtraAttribute(description, intention);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Attribute CreateExtraAttribute(string description, Intention intention)
        {
            return new Attribute
            {
                Title = ExtraAttributeTitle,
                Description = description,
                Intention = intention
            };
        }
    }
}
