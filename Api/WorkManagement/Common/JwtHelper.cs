using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using WorkManagement.Model;

namespace WorkManagement.Common
{
    public static class JwtHelper
    {
        // <summary>
        // <source>https://stackoverflow.com/questions/40281050/jwt-authentication-for-asp-net-web-api</source>
        // <des>Copy code here</des>
        // <author>HaiHN</author>
        // </summary>

        /// <summary>
        /// use the below code to generate symmetric secret key
        /// var hmac = new hmacsha256();
        /// var key = convert.tobase64string(hmac.key);
        /// </summary>
        public static string GenerateToken(string username, string dataToken = "", string secretKey = "", bool hasExpire = false, int expireMinutes = 20)
        {
            var symmetricKey = Convert.FromBase64String(secretKey);
            if (secretKey != "")
                symmetricKey = Convert.FromBase64String(secretKey);
            var tokenHandler = new JwtSecurityTokenHandler();
            var now = DateTime.UtcNow;

            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            // claimsIdentity.AddClaim(New Claim(ClaimTypes.Email, user.EmailAddress))
            // claimsIdentity.AddClaim(New Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()))
            //claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, username));

            if (dataToken != "")
                claimsIdentity.AddClaim(new Claim("dataToken", dataToken));

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = claimsIdentity,
                Expires = (hasExpire == false ? new DateTime(2038, 1, 1) : now.AddMinutes(Convert.ToInt32(expireMinutes))),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };
            // .Expires = now.AddMinutes(Convert.ToInt32(expireMinutes)),

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);
            return token;
        }

        public static List<Claim> GenerateTokenOmicall(string dataToken = "")
        {
            var TokenInfo = new Dictionary<string, string>();

            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(dataToken);
            var claims = jwtSecurityToken.Claims.ToList();
            return claims;
        }

        public static bool ValidateToken(string token, ref string username, ref string dataToken, string secretKey = "")
        {
            try
            {
                username = null;
                var simplePrinciple = GetPrincipal(token, secretKey);
                var identity = simplePrinciple.Identity as ClaimsIdentity;
                if (identity == null)
                    return false;
                if (!identity.IsAuthenticated)
                    return false;
                var usernameClaim = identity.FindFirst(ClaimTypes.Name);
                username = usernameClaim?.Value;

                var dataTokenClaim = identity.FindFirst("dataToken");
                dataToken = dataTokenClaim?.Value;
				
                if (string.IsNullOrEmpty(username))
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static ClaimsPrincipal GetPrincipal(string token, string secretKey = "")
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
                if (jwtToken == null)
                    return null/* TODO Change to default(_) if this is not a reference type */;
                var symmetricKey = Convert.FromBase64String(secretKey);
                if (secretKey != "")
                    symmetricKey = Convert.FromBase64String(secretKey);
                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
                };
                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);
                return principal;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
