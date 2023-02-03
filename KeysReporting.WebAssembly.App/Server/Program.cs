using KeysReporting.WebAssembly.App.Client.Providers;
using KeysReporting.WebAssembly.App.Server.Data;
using KeysReporting.WebAssembly.App.Server.Providers;
using KeysReporting.WebAssembly.App.Server.Providers.LiveVoxAPI;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#if DEBUG
var connString = builder.Configuration.GetConnectionString("DBTest");
#else
var connString = builder.Configuration.GetConnectionString("DB");
#endif

builder.Services.AddDbContext<CallDispositionContext>(options => options.UseSqlServer(connString));

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

//Scope
builder.Services.AddScoped<IHttpClientFactory, HttpFactoryWithProxy>();
builder.Services.AddScoped<ILiveVoxAPI, LiveVoxAPI>();

//Use For Logging
builder.Host.UseSerilog((ctx, lc) =>
    lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
