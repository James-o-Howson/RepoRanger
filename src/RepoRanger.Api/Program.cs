using RepoRanger.Api;
using RepoRanger.Application;
using RepoRanger.Domain;
using RepoRanger.Infrastructure;
using RepoRanger.Persistence;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddDomainServices();

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

app.UseCors(c => c.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseHttpsRedirection();
app.UseHealthChecks("/_health");
app.MapControllers();

app.Run();