using System;
using System.Linq;
using System.Reflection;
using AspCqrs.Api.OpenApi;
using AspCqrs.Api.Services;
using AspCqrs.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace AspCqrs.Api
{
    public static class DependencyInjection
    {
        public static void AddApi(this IServiceCollection services, IConfiguration configuration, string corsPolicy)
        {
            services.AddControllers();

            services.AddHttpContextAccessor();
            
            services.AddSwaggerGen(c =>
            {
                c.SchemaFilter<SwaggerKeyPropertySchemaFilter>();
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "AspCqrs.Api", Version = "v1"});

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description =
                        "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
            
            services.AddCors(options =>
            {
                options.AddPolicy(corsPolicy, builder =>
                {
                    builder.AllowAnyHeader()
                        .AllowAnyMethod()
                        .SetIsOriginAllowed(host => true)
                        .AllowCredentials();
                });
            });
            
            services.AddSignalR();
            
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            
            AddStreams(services);
        }

        private static void AddStreams(IServiceCollection services)
        {
            Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && type.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IStreamService<>)))
                .ToList()
                .ForEach(type =>
                {
                    var streamDataType = type.GetInterfaces()
                        .FirstOrDefault(
                            i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IStreamService<>))
                        ?.GetGenericArguments().FirstOrDefault();

                    if (streamDataType == null) return;
                    
                    var streamAbstraction = typeof(IStreamService<>).MakeGenericType(streamDataType);

                    services.AddScoped(streamAbstraction, type);
                });
        }
    }
}