using System.Text.Json;

namespace WikiMeterApi;
public interface IWikipediaDataService
{
    Task<JsonElement> GetWikipediaDataAsync();
}

public class WikipediaDataService : IWikipediaDataService
{
    private readonly HttpClient _httpClient;

    public WikipediaDataService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<JsonElement> GetWikipediaDataAsync()
    {
        var wikipediaApiUrl = "https://en.wikipedia.org/w/api.php?action=query&meta=siteinfo&siprop=statistics&format=json";
        var response = await _httpClient.GetStringAsync(wikipediaApiUrl);
        var json = JsonSerializer.Deserialize<JsonElement>(response);
        return json;
    }
}