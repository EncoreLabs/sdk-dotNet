using System;
using System.Linq;

namespace EncoreTickets.SDK.Utilities.Exceptions
{
    public class BadArgumentsException : Exception
    {
        public BadArgumentsException() : base(GetMessage())
        {
        }

        public BadArgumentsException(params string[] descriptions) : base(GetMessage(descriptions))
        {
        }

        private static string GetMessage(string[] descriptions = null)
        {
            const string message = "Invalid arguments";
            if (descriptions == null || !descriptions.Any())
            {
                return message;
            }

            return $"{message}: {string.Join("; ", descriptions)}";
        }
    }
}
