using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using EncoreTickets.SDK.Utilities.Encoders;
using EncoreTickets.SDK.Utilities.Serializers;

namespace EncoreTickets.SDK.Utilities
{
    public static class JwtHelper
    {
        private static readonly IDecoder<string, JwtSecurityToken> JwtDecoder = new JwtEncoder();

        public static bool CheckIfJwtStringContainsData(string jwtString)
        {
            var readableJwtToken = JwtDecoder.Decode(jwtString);
            return readableJwtToken?.Claims != null && readableJwtToken.Claims.Any();
        }

        public static string GetDecodedPropertyFromJwtString(string jwtString, string propertyName)
        {
            var claim = GetClaimWithValueOrNull(jwtString, propertyName);
            return claim?.Value;
        }

        public static T GetDecodedPropertyFromJwtString<T>(string jwtString, string propertyName)
        {
            var claim = GetClaimWithValueOrNull(jwtString, propertyName);
            if (claim == null)
            {
                return default;
            }

            try
            {
                var deserializer = new DefaultJsonSerializer();
                return deserializer.Deserialize<T>(claim.Value);
            }
            catch (Exception)
            {
                return default;
            }
        }

        private static Claim GetClaimWithValueOrNull(string jwtString, string propertyName)
        {
            var readableJwtToken = JwtDecoder.Decode(jwtString);
            var claim = readableJwtToken?.Claims?.FirstOrDefault(x => x.Type == propertyName);
            var nullTypes = new List<string> { "JSON_NULL" };
            return claim == null || nullTypes.Contains(claim.ValueType)
                ? null
                : claim;
        }
    }
}