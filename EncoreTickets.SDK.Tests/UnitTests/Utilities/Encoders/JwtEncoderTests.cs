using System.Linq;
using EncoreTickets.SDK.Utilities.Encoders;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Utilities.Encoders
{
    [TestFixture]
    internal class JwtEncoderTests
    {
        [TestCase(33, "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOCIsInZjIjoiR0IiLCJwaSI6IjE1ODciLCJpaSI6IkNJUkNMRX5YNDQ7NTAiLCJpYiI6IkRDIiwiaXIiOiJYIiwiaXNuIjoiNDQiLCJpc2xkIjoiQ2lyY2xlIiwiaXBpIjpudWxsLCJpZCI6IjIwMjAtMTAtMjNUMTk6MzA6MDArMDA6MDAiLCJlc2kiOiJJTlRFUk5BTCIsImVyaSI6bnVsbCwiZXNlaSI6bnVsbCwiZWJpIjpudWxsLCJlcGkiOm51bGwsImVkY3QiOm51bGwsInBhaSI6IjM1MzgiLCJjcHYiOjAsImNwYyI6IkdCUCIsIm9zcHYiOjMyMDAsIm9zcGMiOiJHQlAiLCJvZnZ2IjoyNTAwLCJvZnZjIjoiR0JQIiwic3NwdiI6MzIwMCwic3NwYyI6IkdCUCIsInNmdnYiOjI1MDAsInNmdmMiOiJHQlAiLCJvdHNzcGZyIjoxLCJzdG9zcGZyIjoxLCJpYyI6NCwicG1jIjpudWxsLCJyZWQiOiIxODU4MTExNyIsInBydiI6MH0.L-E7HTETVnPRzkr6ghsFVTL4X62rSycnF-S_PtIH8KM")]
        public void Decode_IfJwtIsCorrect_ReturnsCorrectData(int expectedClaimsCount, string jwtToken)
        {
            var encoder = new JwtEncoder();

            var actual = encoder.Decode(jwtToken);

            Assert.AreEqual(expectedClaimsCount, actual.Claims.Count());
        }

        [TestCase("eyJ0eXAiOiJKVCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOCIsInZjIjoiR0IiLCJwaSI6IjE1ODciLCJpaSI6IkNJUkNMRX5YNDQ7NTAiLCJpYiI6IkRDIiwiaXIiOiJYIiwiaXNuIjoiNDQiLCJpc2xkIjoiQ2lyY2xlIiwiaXBpIjpudWxsLCJpZCI6IjIwMjAtMTAtMjNUMTk6MzA6MDArMDA6MDAiLCJlc2kiOiJJTlRFUk5BTCIsImVyaSI6bnVsbCwiZXNlaSI6bnVsbCwiZWJpIjpudWxsLCJlcGkiOm51bGwsImVkY3QiOm51bGwsInBhaSI6IjM1MzgiLCJjcHYiOjAsImNwYyI6IkdCUCIsIm9zcHYiOjMyMDAsIm9zcGMiOiJHQlAiLCJvZnZ2IjoyNTAwLCJvZnZjIjoiR0JQIiwic3NwdiI6MzIwMCwic3NwYyI6IkdCUCIsInNmdnYiOjI1MDAsInNmdmMiOiJHQlAiLCJvdHNzcGZyIjoxLCJzdG9zcGZyIjoxLCJpYyI6NCwicG1jIjpudWxsLCJyZWQiOiIxODU4MTExNyIsInBydiI6MH0.L-E7HTETVnPRzkr6ghsFVTL4X62rSycnF-S_PtIH8KM")]
        public void Decode_IfJwtIsIncorrect_ReturnsNull(string jwtToken)
        {
            var encoder = new JwtEncoder();

            var actual = encoder.Decode(jwtToken);

            Assert.IsNull(actual);
        }
    }
}
