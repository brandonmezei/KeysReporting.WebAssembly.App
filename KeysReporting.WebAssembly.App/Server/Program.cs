using KeysReporting.WebAssembly.App.Server.Configurations;
using KeysReporting.WebAssembly.App.Server.Data;
using KeysReporting.WebAssembly.App.Server.Services;
using KeysReporting.WebAssembly.App.Server.Services.CPHReport;
using KeysReporting.WebAssembly.App.Server.Services.Lists;
using KeysReporting.WebAssembly.App.Server.Services.LiveVoxAPI;
using KeysReporting.WebAssembly.App.Server.Services.Reports.AgentReport;
using KeysReporting.WebAssembly.App.Server.Services.Reports.TermCodes;
using KeysReporting.WebAssembly.App.Server.Services.System.FTP;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

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
builder.Services.AddScoped<ILiveVoxAPIService, LiveVoxAPIService>();
builder.Services.AddScoped<ICPHReportService, CPHReportService>();
builder.Services.AddScoped<ISourceTableService, SourceTableService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IAgentReportService, AgentReportService>();
builder.Services.AddScoped<IAgentService, AgentService>();
builder.Services.AddScoped<ITermCodeReportService, TermCodeReportService>();
builder.Services.AddScoped<ITermCodeService, TermCodeService>();
builder.Services.AddScoped<IFTPService, FTPService>();

//Use for Mapper
builder.Services.AddAutoMapper(typeof(MapperConfig));

//Use For Logging
builder.Host.UseSerilog((ctx, lc) =>
    lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));

//Use for Cors
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll",
        b => b.AllowAnyMethod().
        AllowAnyHeader().
        AllowAnyOrigin());
});

//Use For Auth
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
    };
});

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

app.UseCors("AllowAll");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
