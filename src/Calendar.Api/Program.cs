using Calendar.Identity.Core;
using Calendar.Shared.Abstractions.Modules;
using Calendar.Shared.Infrastructure;

var modules = new List<IModule>
{
    new IdentityModule()
};

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
foreach (var module in modules)
{
    module.Add(builder.Services, builder.Configuration);
}

var app = builder.Build();

app.UseInfrastructure();
foreach (var module in modules)
{
    await module.Use(app, builder.Configuration);
}

app.MapControllers();

app.Run();
