using System;
using System.IdentityModel.Tokens.Jwt;

namespace EncoreTickets.SDK.Utilities.Encoders
{
    internal class JwtEncoder : IDecoder<string, JwtSecurityToken>
    {
        public JwtSecurityToken Decode(string encodedData)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                return handler.ReadJwtToken(encodedData);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
