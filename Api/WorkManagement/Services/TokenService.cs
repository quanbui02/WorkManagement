using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Work.DataContext;
using WorkManagement.Model;
using WorkManagement.Models.Request;

namespace WorkManagement.Services
{
    public class TokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly AppDbContext _context;

        public TokenService(IOptions<JwtSettings> jwtOptions, AppDbContext context)
        {
            _jwtSettings = jwtOptions.Value;
            _context = context;
        }

        public async Task<TokenResponse> GenerateTokensAsync(AppUser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim("username", user.UserName),
            new Claim("isSuperUser", user.IsSuperUser.ToString().ToLower())
        };

            var accessToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.ExpiresInMinutes),
                signingCredentials: creds
            );

            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                ExpiryDate = DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpiresInDays),
                UserId = user.Id,
                CreatedDate = DateTime.Now
            };

            // Lưu refresh token vào DB
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return new TokenResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = refreshToken.Token,
                Expires = accessToken.ValidTo
            };
        }
    }


}
