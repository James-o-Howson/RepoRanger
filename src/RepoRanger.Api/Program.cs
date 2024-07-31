using RepoRanger.Api;
using RepoRanger.BackgroundJobs;
using RepoRanger.Domain;
using RepoRanger.Infrastructure;
using RepoRanger.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

builder.Services.AddApi(builder.Configuration, builder.Environment);
builder.Services.AddBackgroundJobs(builder.Configuration, builder.Environment);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddDomain();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(settings =>
    {
        settings.Path = "/api";
        settings.DocumentPath = "/api/specification.json";
    });
}

app.UseSerilogRequestLogging();
app.UseCors(c => c.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseHttpsRedirection();
app.UseHealthChecks("/_health");
app.MapControllers();

app.Run();

// ReSharper disable once ClassNeverInstantiated.Global
namespace RepoRanger.Api
{
    public class Program {}
}
