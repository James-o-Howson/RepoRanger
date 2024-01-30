using RepoRanger.Api.Configuration.AzureDevOps;
using RepoRanger.Api.Configuration.ExceptionHandlers;
using RepoRanger.Api.Configuration.Logging;
using RepoRanger.Api.Configuration.Quartz;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

builder.Services.AddQuartzServices(builder.Configuration);
builder.Services.AddAzureDevOpsServices(builder.Configuration);
builder.Services.AddExceptionHandlerServices();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();
builder.Services.AddSwaggerGen();

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