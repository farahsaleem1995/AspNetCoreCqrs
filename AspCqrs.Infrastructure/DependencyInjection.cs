using System;
using System.Linq;
using System.Reflection;
using System.Text;
using AspCqrs.Application.Common.Interfaces;
using AspCqrs.Application.Options;
using AspCqrs.Infrastructure.Identity;
using AspCqrs.Infrastructure.Persistence;
using AspCqrs.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

namespace AspCqrs.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connection = configuration.GetConnectionString("DefaultConnection");

            var jwtSettings = new JwtSettings();
            configuration.GetSection(JwtSettings.Section).Bind(jwtSettings);

            services.AddDbContext<ApplicationDbContext>(builder =>
                builder.UseSqlServer(connection,
                    optionsBuilder =>
                        optionsBuilder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;

                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

                options.SignIn.RequireConfirmedAccount = true;
            });

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                    };
                });

            services.AddScoped<IIdentityService, IdentityService>();

            services.AddScoped<IDomainEventService, DomainEventService>();

            services.AddScoped<IDateTimeService, DateTimeService>();

            services.AddScoped<IJwtService, JwtService>();

            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.Section));

            AddDbInitializers(services);
            
            AddRedis(services, configuration);
        }

        private static void AddDbInitializers(IServiceCollection services)
        {
            Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && typeof(IDbInitializer).IsAssignableFrom(type))
                .ToList()
                .ForEach(type => services.AddScoped(type));
        }

        private static void AddRedis(IServiceCollection services, IConfiguration configuration)
        {
            var redisSettings = new RedisSettings();
            configuration.GetSection("RedisSettings").Bind(redisSettings);

            services.Configure<RedisSettings>(configuration.GetSection(RedisSettings.Section));
            
            services.AddSingleton<IConnectionMultiplexer>(provider =>
                ConnectionMultiplexer.Connect(new ConfigurationOptions
                {
                    Password = redisSettings.Password,
                    AllowAdmin = redisSettings.AllowAdmin,
                    Ssl = redisSettings.Ssl,
                    ConnectTimeout = redisSettings.ConnectTimeout,
                    ConnectRetry = redisSettings.ConnectRetry,
                    AbortOnConnectFail = redisSettings.AbortOnConnectFail,
                    EndPoints = {redisSettings.Connection}
                }));

            services.AddSingleton<ICacheService, RedisCacheService>();
        }
    }
}