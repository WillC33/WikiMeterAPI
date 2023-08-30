using Microsoft.AspNetCore.RateLimiting;
using WikiMeterApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ITotalArticlesService, TotalArticlesService>();
builder.Services.AddScoped<IWikipediaDataService, WikipediaDataService>();
builder.Services.AddHttpClient();
builder.Services.AddCors();
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();

//TODO: configure rate limiting


var app = builder.Build();

app.UseCors(options => options.AllowAnyOrigin());

app.MapGet("/api/wikipedia", async (IWikipediaDataService wikipediaDataService) =>
await wikipediaDataService.GetWikipediaDataAsync()
);

app.Run();