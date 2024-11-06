using RepoRanger.Api;
using RepoRanger.BackgroundJobs;
using RepoRanger.Data;
using RepoRanger.Domain;
using RepoRanger.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.AddLogging();

builder.Services.AddApiServices();
builder.Services.AddBackgroundJobServices(builder.Configuration, builder.Environment);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddDomainServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.UseSerilogRequestLogging();
app.UseCors(c => c.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseHttpsRedirection();
app.UseHealthChecks("/health");
app.MapControllers();

await app.RunAsync();

// ReSharper disable once ClassNeverInstantiated.Global
namespace RepoRanger.Api
{
    public class Program;
}
