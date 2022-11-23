using Client.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using System.Net;
using TCPI_PR_Portal;
using TCPI_PR_Portal.Client;
using Blazor.SubtleCrypto;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddSubtleCrypto(opt =>
    opt.Key = "ELE9xOyAyJHCsIPLMbbZHQ7pVy7WUlvZ60y5WkKDGMSw5xh5IM54kUPlycKmHF9VGtYUilglL8iePLwr"
);

builder.Services.AddTransient<CookieHandler>()
    .AddScoped(sp => sp
        .GetRequiredService<IHttpClientFactory>()
        .CreateClient("ServiceLayer"))
    .AddHttpClient("ServiceLayer", client => client.BaseAddress = new Uri("http://172.31.24.203:50001")).AddHttpMessageHandler<CookieHandler>();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddAuthorizationCore();
builder.Services.AddMudServices();

await builder.Build().RunAsync();