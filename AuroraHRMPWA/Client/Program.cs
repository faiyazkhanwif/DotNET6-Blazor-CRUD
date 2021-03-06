global using AuroraHRMPWA.Client.Services.AuthService;
global using AuroraHRMPWA.Client.Services.EmployeeDetailsService;
global using AuroraHRMPWA.Shared;
global using Microsoft.AspNetCore.Components.Authorization;
global using System.Net.Http.Json;
using AuroraHRMPWA.Client;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IAuthServiceClient, AuthServiceClient>();
builder.Services.AddScoped<IEmployeeDetailsServiceClient, EmployeeDetailsServiceClient>();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

await builder.Build().RunAsync();
