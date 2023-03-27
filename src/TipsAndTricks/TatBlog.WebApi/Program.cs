using TatBlog.WebApi.Extensions;
using TatBlog.WebApp.Mapsters;

var builder = WebApplication.CreateBuilder(args);
{
    builder
        .ConfigureCors()
        .ConfigureNLog()
        .ConfigureServices()
        .ConfigureSwaggerOpenApi()
        .ConfigureMapster();
}
var app = builder.Build();
{
    app.SetupRequestPipeLine();

    app.Run();
}
app.Run();