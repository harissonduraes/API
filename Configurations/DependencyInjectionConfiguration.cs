using API.Contexts;

namespace API.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            //services.AddScoped<Contexts.MongoDBContext>();
            services.AddScoped<Repositories.Interfaces.ICadPessoaRepository, Repositories.Implementations.CadPessoaRepository>();
            return services;
        }
    }
}
