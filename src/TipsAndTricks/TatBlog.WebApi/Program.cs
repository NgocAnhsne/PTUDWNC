using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Validations;
using TatBlog.WebApp.Mapsters;

var builder = WebApplication.CreateBuilder(args);
{
    builder
        .ConfigureCors()
        .ConfigureNLog()
        .ConfigureServices()
        .ConfigureSwaggerOpenApi()
        .ConfigureMapster()
        .ConfigureFluentValidation();
}
var app = builder.Build();
{
    app.SetupRequestPipeLine();

    app.Run();
}
app.Run();