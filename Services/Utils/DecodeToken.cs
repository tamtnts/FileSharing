using System.IdentityModel.Tokens.Jwt;

namespace Services.Utils
{
    public class DecodeToken
    {
        public int Decode(string token, string claimType)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var claim = jwtToken.Claims.FirstOrDefault(c => c.Type == claimType);

            if (claim != null && int.TryParse(claim.Value, out int claimValue))
            {
                return claimValue;
            }

            throw new ArgumentException("Claim not found or invalid claim type");
        }
    }
}
