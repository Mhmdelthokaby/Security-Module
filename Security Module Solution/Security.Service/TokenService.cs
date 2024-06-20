using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Security.Core.Entities.Identity;
using Security.Core.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Security.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> CreateTokenAsync(AppUser user ,UserManager<AppUser> userManager)
        {
            var keyString = _configuration["JWT:key"];
            var keyBytes = Encoding.UTF8.GetBytes(keyString.PadRight(32)); // Pad or truncate to ensure 32 bytes

            var AuthKey = new SymmetricSecurityKey(keyBytes);

            var AuthClaims = new List<Claim>()
    {
        new Claim(ClaimTypes.GivenName, user.DisplayName),
        new Claim(ClaimTypes.Email, user.Email)
    };

            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                AuthClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var Token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIsser"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(double.Parse(_configuration["JWT:DurationDays"])),
                claims: AuthClaims,
                signingCredentials: new SigningCredentials(AuthKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(Token);

        }
    }
}
