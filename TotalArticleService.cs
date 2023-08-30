namespace WikiMeterApi;

public interface ITotalArticlesService
{
    int TotalArticles { get; set; }
}

public class TotalArticlesService : ITotalArticlesService
{
    public int TotalArticles { get; set; }
}