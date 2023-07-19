using API.Extensions;
using API.MiddleWare;
using Application.Activities;
using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CorsPolicy");
app.UseAuthorization();

app.MapControllers();

using var scope  = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
var context= services.GetRequiredService<DataContext>();
await  context.Database.MigrateAsync();
await Seed.SeedData(context);
}
catch(Exception ex)
{
var logger = services.GetRequiredService<ILogger<Program>>();
logger.LogError(ex,"an error occured during migration");
}

app.Run();
