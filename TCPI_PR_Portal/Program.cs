using Client.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using System.Net;
using TCPI_PR_Portal;
using TCPI_PR_Portal.Client;
using Blazor.SubtleCrypto;
using MudBlazor;
using Solutaris.InfoWARE.ProtectedBrowserStorage.Extensions;
using TCPI_PR_Portal.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSubtleCrypto(opt => opt.Key = "ELE9xOyAyJHCsIPLMbbZHQ7pVy7WUlvZ60y5WkKDGMSw5xh5IM54kUPlycKmHF9VGtYUilglL8iePLwr");

builder.Services.AddTransient<CookieHandler>()
    .AddScoped(sp => sp
        .GetRequiredService<IHttpClientFactory>()
        .CreateClient("ServiceLayer"))
    .AddHttpClient("ServiceLayer", client => client.BaseAddress = new Uri("http://172.31.24.203:50001/b1s/v1/")).AddHttpMessageHandler<CookieHandler>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;

    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 10000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Outlined;
});

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddAuthorizationCore();
builder.Services.AddMudServices();
builder.Services.AddIWProtectedBrowserStorageAsSingleton(); //optionally pass an encryption key (as string)

await builder.Build().RunAsync();