using System;
using System.Linq;
using System.Threading.Tasks;
using AspCqrs.Application.Common.Interfaces;

namespace AspCqrs.Application.Common
{
    public static class Seeder
    {
        public static void Seed(IServiceProvider provider)
        {
            Console.WriteLine("Seeder");

            var dbInitializers = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsClass && !type.IsAbstract && typeof(IDbInitializer).IsAssignableFrom(type))
                .Select(type => (IDbInitializer) provider.GetService(type))
                .Where(initializer => initializer != null);

            Task.Run(async () =>
            {
                foreach (var dbInitializer in dbInitializers)
                {
                    await dbInitializer.Initialize();
                }
            }).Wait();
        }
    }
}