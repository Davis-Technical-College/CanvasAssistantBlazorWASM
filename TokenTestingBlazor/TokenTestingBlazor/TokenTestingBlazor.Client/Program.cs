using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TokenTestingBlazor.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddSingleton<CookieStorageAccessor>();
builder.Services.AddSingleton<CanvasAuthAccessor>();
builder.Services.AddSingleton<CanvasProfileAccessor>();
builder.Services.AddSingleton<CanvasCourseAccessor>();
builder.Services.AddSingleton<AzureOAuth>();
builder.Services.AddScoped<HttpClient>();

await builder.Build().RunAsync();
