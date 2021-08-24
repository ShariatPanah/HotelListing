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
        private readonly SignInManager<AppUser> _signInManager;
        private readonly SiteSettings _siteSettings;

        public JwtService(SignInManager<AppUser> signInManager, IOptionsSnapshot<SiteSettings> siteSettings)
        {
            _signInManager = signInManager;
            _siteSettings = siteSettings.Value;
        }

        public async Task<string> GenerateAsync(AppUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_siteSettings.JwtSettings.SecretKey)); // should be equal or longer than 16 chars
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var encryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_siteSettings.JwtSettings.EncryptKey)); // should be 16 chars exactly
            var encryptingCredentials = new EncryptingCredentials(encryptionKey, SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

            var claims = await GetClaimsAsync(user);

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _siteSettings.JwtSettings.Issuer,
                Audience = _siteSettings.JwtSettings.Audience,
                IssuedAt = DateTime.Now,
                NotBefore = DateTime.Now.AddMinutes(_siteSettings.JwtSettings.NotBeforeInMinutes), // این یعنی توکنی که تولید کردیم از لحظه تولید تا قبل از پنج دقیقه قابل استفاده نیست و بعد از پنج دقیقه معتبر و قابل استفاده خواهد بود
                Expires = DateTime.Now.AddDays(_siteSettings.JwtSettings.ExpirationInDays),
                SigningCredentials = signingCredentials,
                EncryptingCredentials = encryptingCredentials,

                //Claims = claims,
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(descriptor);
            var jwtToken = tokenHandler.WriteToken(securityToken);

            return jwtToken;
        }

        private async Task<IEnumerable<Claim>> GetClaimsAsync(AppUser user)
        {
            var result = await _signInManager.ClaimsFactory.CreateAsync(user);
            var claimsList = new List<Claim>(result.Claims);
            claimsList.Add(new Claim(ClaimTypes.MobilePhone, user.PhoneNumber));
            claimsList.Add(new Claim(ClaimTypes.Role, "Admin"));

            return claimsList;

            //var dictionary = new Dictionary<string, object>();
            //foreach (var claim in result.Claims)
            //{
            //    dictionary.Add(claim.Type, claim.Value);
            //}

            //dictionary.Add(ClaimTypes.MobilePhone, user.PhoneNumber);
            //dictionary.Add(ClaimTypes.Role, "Admin");
            ////dictionary.Add(ClaimTypes.Role, "User");

            //return dictionary;

            ////JwtRegisteredClaimNames.Email; // میتونی از این تایپ ها هم استفاده کنی به جای کلیم تایپس
            //var securityClaimType = new ClaimsIdentityOptions().SecurityStampClaimType;

            //var dictionary = new Dictionary<string, object>
            //{
            //    { ClaimTypes.Name, user.UserName },
            //    { ClaimTypes.NameIdentifier, user.Id },
            //    { ClaimTypes.MobilePhone, user.PhoneNumber },
            //    { securityClaimType, user.SecurityStamp }
            //};

            //var roles = await _userManager.GetRolesAsync(user);
            //foreach (var role in roles)
            //{
            //    dictionary.Add(ClaimTypes.Role, role);
            //}

            //return dictionary;
        }
    }
}
