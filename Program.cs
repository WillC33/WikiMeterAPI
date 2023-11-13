using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using WikiMeterApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ITotalArticlesService, TotalArticlesService>();
builder.Services.AddScoped<IWikipediaDataService, WikipediaDataService>();
builder.Services.AddHttpClient();
builder.Services.AddCors();
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddTokenBucketLimiter("token", bucketOptions =>
    {
        bucketOptions.TokenLimit = 100;
        bucketOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        bucketOptions.QueueLimit = 5;
        bucketOptions.ReplenishmentPeriod = TimeSpan.FromSeconds(10);
        bucketOptions.TokensPerPeriod = 20;
        bucketOptions.AutoReplenishment = true;
    });
});


var app = builder.Build();

app.UseCors(options => options.AllowAnyOrigin());

app.UseRateLimiter();

app.MapGet("/api/wikipedia", async (IWikipediaDataService wikipediaDataService) =>
        await wikipediaDataService.GetWikipediaDataAsync()
    )
    .RequireRateLimiting("token");

app.Run();