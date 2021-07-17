using Contract.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Extensions
{
    public static class CoreExtension
    {
        public static void GetCoreExtension(this IServiceCollection services)
        {
            services.AddScoped<IRepository, BookRepository>();
            services.AddScoped<ISqsService, SqsService>();
        }
    }
}
