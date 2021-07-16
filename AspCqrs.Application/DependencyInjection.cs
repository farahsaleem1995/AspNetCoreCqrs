using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace AspCqrs.Application
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}