using Microsoft.EntityFrameworkCore;
using TatBlog.Data.Seeders;
using TatBlog.WebApp.Extensions;
using TatBlog.WebApp.Mapsters;


var builder = WebApplication.CreateBuilder(args);
{
    builder.ConfigureMvc().ConfigureServices().ConfigureMapster();
}
var app = builder.Build();
{
    app.UseRequestPipeline();
    app.UseBlogRoutes();
    app.UseDataSeeder();
}

// Dữ liệu mẫu trước khi Run App
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
    seeder.Initialize();
}

app.Run();
