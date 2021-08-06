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
            services.AddApi(Configuration, CorsPolicy);

            services.AddInfrastructure(Configuration);

            services.AddApplication();
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