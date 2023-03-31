using TatBlog.WebApi.Endpoints;
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
    // Configure the HTTP request pipeline
    app.SetupRequestPipeLine();

    // Configure API Endponts
    app.MapAuthorEndpoints();
    app.MapCategoryEndpoints();
    app.MapTagEndpoints();

    app.Run();
}