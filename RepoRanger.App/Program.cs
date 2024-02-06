using RepoRanger.App;
using RepoRanger.App.Components;
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

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseHealthChecks("/_health");
app.MapControllers();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
