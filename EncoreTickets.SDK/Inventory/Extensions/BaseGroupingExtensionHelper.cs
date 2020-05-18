using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using EncoreTickets.SDK.Inventory.Models;
using EncoreTickets.SDK.Utilities.Encoders;
using EncoreTickets.SDK.Utilities.Serializers;

namespace EncoreTickets.SDK.Inventory.Extensions
{
    internal static class BaseGroupingExtensionHelper
    {
        private static readonly IDecoder<string, JwtSecurityToken> JwtDecoder = new JwtEncoder();

        public static string GetPropertyFromAggregateReference(BaseGrouping grouping, string propertyName)
        {
            var claim = GetClaimWithValueOrNull(grouping, propertyName);
            return claim?.Value;
        }

        public static T GetPropertyFromAggregateReference<T>(BaseGrouping grouping, string propertyName)
        {
            var claim = GetClaimWithValueOrNull(grouping, propertyName);
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

        private static Claim GetClaimWithValueOrNull(BaseGrouping grouping, string propertyName)
        {
            var readableJwtToken = JwtDecoder.Decode(grouping.AggregateReference);
            var claim = readableJwtToken?.Claims?.FirstOrDefault(x => x.Type == propertyName);
            var nullTypes = new List<string> { "JSON_NULL" };
            return claim == null || nullTypes.Contains(claim.ValueType)
                ? null
                : claim;
        }
    }
}