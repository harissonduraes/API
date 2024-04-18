using API.Contexts;

namespace API.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IMongoDBContext, MongoDBContext>();
            return services;
        }
    }
}
