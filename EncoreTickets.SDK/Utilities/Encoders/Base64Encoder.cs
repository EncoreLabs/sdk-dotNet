using System;
using System.Text;

namespace EncoreTickets.SDK.Utilities.Encoders
{
    internal class Base64Encoder : IEncoder<string, string>, IDecoder<string, string>
    {
        public string Encode(string text)
        {
            var plainTextBytes = text == null ? new byte[0] : Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(plainTextBytes);
        }

        public string Decode(string encodedText)
        {
            var base64EncodedBytes = encodedText == null ? new byte[0] : Convert.FromBase64String(encodedText);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}