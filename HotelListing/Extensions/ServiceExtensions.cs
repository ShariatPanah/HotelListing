using HotelListing.Common;
using HotelListing.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HotelListing.Data
{
    public static class ServiceExtensions
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, IdentityRole>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequiredLength = 6;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            //var builder = services.AddIdentityCore<AppUser>(options =>
            //{
            //    options.User.RequireUniqueEmail = true;
            //    options.Password.RequiredLength = 6;
            //});

            //builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
            //builder.AddEntityFrameworkStores<ApplicationDbContext>();
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

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            //var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                            //logger.LogError("Authentication failed.", context.Exception);

                            if (context.Exception != null)
                                throw context.Exception;

                            return Task.CompletedTask;
                        },
                        OnTokenValidated = async context =>
                        {
                            var signInManager = context.HttpContext.RequestServices.GetRequiredService<SignInManager<AppUser>>();
                            //var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<AppUser>>();

                            var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                            if ((bool)!claimsIdentity.Claims?.Any())
                                context.Fail("This token has no claims!");

                            var stct = new ClaimsIdentityOptions().SecurityStampClaimType;
                            var securityStampInToken = claimsIdentity.FindFirst(c => c.Type == stct);
                            if (securityStampInToken == null)
                                context.Fail("This token has no Security Stamp!");

                            var validatedUser = await signInManager.ValidateSecurityStampAsync(context.Principal);
                            if (validatedUser == null)
                                context.Fail("Token security stamp is not valid.");
                        },
                        OnChallenge = context =>
                        {
                            //var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                            //logger.LogError("OnChallenge error", context.Error, context.ErrorDescription);

                            if (context.AuthenticateFailure != null)
                                throw context.AuthenticateFailure;

                            throw new UnauthorizedAccessException();
                        }
                    };
                });
        }
    }
}
