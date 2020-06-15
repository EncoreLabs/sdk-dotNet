using System.Text.RegularExpressions;

namespace EncoreTickets.SDK.Tests.Helpers
{
    public static class StringExtension
    {
        public static string StripWhitespace(this string source)
        {
            return Regex.Replace(source, @"\s+", "");
        }
    }
}
