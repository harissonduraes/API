using API;

static IHostBuilder CreateBuilder(string[] args) =>
  Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>();
    });

var builder = CreateBuilder(args);
var app = builder.Build();

app.Run();
