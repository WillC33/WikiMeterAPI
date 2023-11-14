using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using WikiMeterApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IWikipediaDataService, WikipediaDataService>();
builder.Services.AddHttpClient();
builder.Services.AddCors();
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddFixedWindowLimiter(policyName: "fixed", fixedOptions =>
    {
        fixedOptions.PermitLimit = 100;
        fixedOptions.Window = TimeSpan.FromSeconds(10);
        fixedOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        fixedOptions.QueueLimit = 2;
    });
});


var app = builder.Build();

app.UseCors(options => options.AllowAnyOrigin());

app.UseRateLimiter();

app.MapGet("/api/wikipedia", async (IWikipediaDataService wikipediaDataService) =>
        await wikipediaDataService.GetWikipediaDataAsync()
    )
    .RequireRateLimiting("fixed");

app.Run();