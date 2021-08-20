using HotelListing.Common;
using HotelListing.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HotelListing.Services.Jwt
{
    public class JwtService : IJwtService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SiteSettings _siteSettings;

        public JwtService(UserManager<AppUser> userManager, IOptionsSnapshot<SiteSettings> siteSettings)
        {
            _userManager = userManager;
            _siteSettings = siteSettings.Value;
        }

        public async Task<string> Generate(AppUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_siteSettings.JwtSettings.SecretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var claims = await GetClaims(user);

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _siteSettings.JwtSettings.Issuer,
                Audience = _siteSettings.JwtSettings.Audience,
                IssuedAt = DateTime.Now,
                NotBefore = DateTime.Now.AddMinutes(_siteSettings.JwtSettings.NotBeforeInMinutes), // این یعنی توکنی که تولید کردیم از لحظه تولید تا قبل از پنج دقیقه قابل استفاده نیست و بعد از پنج دقیقه معتبر و قابل استفاده خواهد بود
                Expires = DateTime.Now.AddDays(_siteSettings.JwtSettings.ExpirationInDays),
                SigningCredentials = signingCredentials,
                Claims = claims,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(descriptor);
            var jwtToken = tokenHandler.WriteToken(securityToken);

            return jwtToken;
        }

        private async Task<IDictionary<string, object>> GetClaims(AppUser user)
        {
            //JwtRegisteredClaimNames.Email; // میتونی از این تایپ ها هم استفاده کنی به جای کلیم تایپس
            var dictionary = new Dictionary<string, object>
            {
                { ClaimTypes.Name, user.UserName },
                { ClaimTypes.NameIdentifier, user.Id },
                { ClaimTypes.MobilePhone, user.PhoneNumber }
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                dictionary.Add(ClaimTypes.Role, role);
            }

            return dictionary;
        }
    }
}
