using EGTDigital.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EGTDigital
{
    public static class DBConnectionManager
    {
        private static IApplicationBuilder appBuilder;

        public static void SetAppBuilder(IApplicationBuilder builder)
        {
            appBuilder = builder;
        }

        public static EgtDbContext GetDBContext()
        {
            var serviceScopeFactory = appBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            var serviceScope = serviceScopeFactory.CreateScope();

            return serviceScope.ServiceProvider.GetService<EgtDbContext>();
        }
    }
}
