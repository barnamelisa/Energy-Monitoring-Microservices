using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MonitoringService.Config
{
    public static class SecurityConfig
    {
        public static IServiceCollection AddJwtSecurity(this IServiceCollection services, IConfiguration config)
        {
            var secretKey = config["Jwt:Secret"];

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
                    };
                });

            services.AddAuthorization();

            return services;
        }
    }
}
