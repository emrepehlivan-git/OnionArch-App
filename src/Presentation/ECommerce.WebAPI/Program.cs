using ECommerce.WebAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddWebApiServices(builder.Configuration);

var app = builder.Build();

app.UseWebApiServices(app.Environment);
await app.ApplyMigrations();
app.Run();