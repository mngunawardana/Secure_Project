using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using SecureProject.Client;
using SecureProject.Client.Auth;
using SecureProject.Client.Service;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<ProductRecommendationService>();

builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<CustomAuthenticationStateProvider>());


builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddHttpClient("SecureProject.ServerAPI", client =>
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
builder.Services.AddHttpClient<ProductRecommendationService>(client =>
{
client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
});



builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("SecureProject.ServerAPI"));
builder.Services.AddMudServices();
builder.Services.AddBlazorBootstrap();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddMudServices();

await builder.Build().RunAsync();
