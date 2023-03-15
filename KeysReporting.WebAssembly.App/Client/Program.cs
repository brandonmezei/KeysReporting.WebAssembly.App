using Blazored.LocalStorage;
using KeysReporting.WebAssembly.App.Client;
using KeysReporting.WebAssembly.App.Client.Providers;
using KeysReporting.WebAssembly.App.Client.Services.Agent;
using KeysReporting.WebAssembly.App.Client.Services.ApiError;
using KeysReporting.WebAssembly.App.Client.Services.Auth;
using KeysReporting.WebAssembly.App.Client.Services.Base;
using KeysReporting.WebAssembly.App.Client.Services.CPH;
using KeysReporting.WebAssembly.App.Client.Services.Lists;
using KeysReporting.WebAssembly.App.Client.Services.TermCodes;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddBlazoredLocalStorage();

//Auth
builder.Services.AddScoped<ApiAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(p =>
    p.GetRequiredService<ApiAuthenticationStateProvider>()
);

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ICPHReportService, CPHReportService>();
builder.Services.AddScoped<ISourceTableService, SourceTableService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IAgentService, AgentService>();
builder.Services.AddScoped<IAgentReportService, AgentReportService>();
builder.Services.AddScoped<ITermCodeService, TermCodeService>();
builder.Services.AddScoped<ITermCodeListService, TermCodeListService>();
builder.Services.AddScoped<IApiErrorService, ApiErrorService>();

await builder.Build().RunAsync();
