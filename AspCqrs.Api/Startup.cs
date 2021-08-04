using System;
using AspCqrs.Api.Hubs;
using AspCqrs.Api.OpenApi;
using AspCqrs.Api.Services;
using AspCqrs.Application;
using AspCqrs.Application.Common;
using AspCqrs.Application.Common.Interfaces;
using AspCqrs.Application.TodoItems;
using AspCqrs.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace AspCqrs.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            CorsPolicy = configuration.GetSection("Cors").GetValue<string>("Policy");
        }

        public IConfiguration Configuration { get; }

        public string CorsPolicy { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
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
                options.AddPolicy(CorsPolicy, builder =>
                {
                    builder.AllowAnyHeader()
                        .AllowAnyMethod()
                        .SetIsOriginAllowed(host => true)
                        .AllowCredentials();
                });
            });

            services.AddSignalR();

            services.AddInfrastructure(Configuration);

            services.AddApplication();

            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddScoped<IStreamService<TodoItemDto>, TodoItemStreamService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AspCqrs.Api v1"));
            }

            Seeder.Seed(app.ApplicationServices.CreateScope().ServiceProvider);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(CorsPolicy);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");

                endpoints.MapHub<TodoItemHub>("Hubs/TodoItems");
            });
        }
    }
}