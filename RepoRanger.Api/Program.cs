using RepoRanger.Api;
using RepoRanger.Application;
using RepoRanger.Infrastructure;
using RepoRanger.Persistence;
using RepoRanger.SourceParsing;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddSourceParsingServices(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseHealthChecks("/_health");
app.MapControllers();

app.Run();