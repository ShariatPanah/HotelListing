using HotelListing.Common;
using HotelListing.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace HotelListing.Data
{
    public static class ServiceExtensions
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentityCore<AppUser>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 6;
            });

            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
            builder.AddEntityFrameworkStores<ApplicationDbContext>();
        }

        public static void AddJwtAuthentication(this IServiceCollection services, JwtSettings jwtSettings)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)); // should be equal or longer than 16 chars
                    var encryptKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.EncryptKey));

                    var validationParams = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.Zero, // default: 5mins
                        RequireSignedTokens = true,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = securityKey,

                        RequireExpirationTime = true,
                        ValidateLifetime = true,

                        ValidateAudience = true, // default: false
                        ValidAudience = jwtSettings.Audience,

                        ValidateIssuer = true, // default: false
                        ValidIssuer = jwtSettings.Issuer,

                        TokenDecryptionKey = encryptKey
                    };

                    options.TokenValidationParameters = validationParams;
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                });
        }
    }
}
