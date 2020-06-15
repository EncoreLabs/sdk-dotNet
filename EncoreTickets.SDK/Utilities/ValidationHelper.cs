using System;
using System.Collections.Generic;

namespace EncoreTickets.SDK.Utilities
{
    internal static class ValidationHelper
    {
        public static void ThrowArgumentExceptionIfNotSet(params (string Name, object Value)[] properties)
        {
            ThrowArgumentExceptionIfNotSet("{0} must be set", properties);
        }

        public static void ThrowArgumentExceptionIfNotSet(string messageFormat, params (string Name, object Value)[] properties)
        {
            foreach (var property in properties)
            {
                if (property.Value is string propertyValue)
                {
                    ThrowArgumentExceptionIfNotSet(propertyValue, property.Name, messageFormat);
                }
                else
                {
                    ThrowArgumentExceptionIfNotSet(property.Value, property.Name, messageFormat);
                }
            }
        }

        private static void ThrowArgumentExceptionIfNotSet(string specificString, string name, string messageFormat)
        {
            if (string.IsNullOrWhiteSpace(specificString))
            {
                ThrowArgumentExceptionIfNotSet(name, messageFormat);
            }
        }

        private static void ThrowArgumentExceptionIfNotSet(object specificObject, string name, string messageFormat)
        {
            if (specificObject == null)
            {
                ThrowArgumentExceptionIfNotSet(name, messageFormat);
            }
        }

        private static void ThrowArgumentExceptionIfNotSet(string name, string messageFormat)
        {
            var message = string.Format(messageFormat, name);
            throw new ArgumentException(message);
        }
    }
}
