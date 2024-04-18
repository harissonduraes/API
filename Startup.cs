using API.Configurations;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddControllers();
            services.AddDependencyInjection();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

            app.UseCors(configurePolicy: c =>
            {
                c.AllowAnyHeader();
                c.AllowAnyMethod();
                c.AllowAnyOrigin();
            });

            app.UseHttpsRedirection();

            app.UseEndpoints(configure: end =>
            {
                end.MapDefaultControllerRoute();
                end.MapGet(pattern: "/", handler: () => $"Started");
            });
        }
    }
}
