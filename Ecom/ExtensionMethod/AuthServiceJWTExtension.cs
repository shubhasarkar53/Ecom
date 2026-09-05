using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Ecom.ExtensionMethod
{
    public static class AuthServiceJWTExtension
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>{ var jwt = configuration["Jwt:Key"];
                    options.TokenValidationParameters = new TokenValidationParameters
                    {

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt!)),
                        ValidateLifetime = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,

                        ClockSkew = TimeSpan.Zero

                    };
                });

            return services;

        }
    }
}
