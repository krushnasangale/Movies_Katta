using System.Net.Http.Json;
using MoviesKatta.Models.TmdbModels;

namespace MoviesKatta.Services.TmdbServices;

public partial class TmdbService
{
    private const string ApiKey = "deda30c1d6c334fd1d986ec00ca95aec";
    public const string TmdbHttpClientName = "TmdbClientKD";
    private readonly IHttpClientFactory _httpClientFactory;

    public TmdbService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    private HttpClient HttpClient => _httpClientFactory.CreateClient(TmdbHttpClientName);

    public async Task<IEnumerable<MovieResult>> GetTrendingMovies()
    {
        var trendingUrl = $"{Constants.Trending}&api_key={ApiKey}";
        var trendingResult = await HttpClient.GetFromJsonAsync<Movies>(trendingUrl);
        return trendingResult.Results;
    }
    
    public async Task<IEnumerable<MovieResult>> GetTopRatedMovies()
    {
        var trendingUrl = $"{Constants.TopRated}&api_key={ApiKey}";
        var trendingResult = await HttpClient.GetFromJsonAsync<Movies>(trendingUrl);
        return trendingResult.Results;
    }
    
    public async Task<IEnumerable<MovieResult>> GetNetflixMovies()
    {
        var trendingUrl = $"{Constants.TopRated}&api_key={ApiKey}";
        var trendingResult = await HttpClient.GetFromJsonAsync<Movies>(trendingUrl);
        return trendingResult.Results;
    }
    
    public async Task<IEnumerable<MovieResult>> GetActionMovies()
    {
        var trendingUrl = $"{Constants.TopRated}&api_key={ApiKey}";
        var trendingResult = await HttpClient.GetFromJsonAsync<Movies>(trendingUrl);
        return trendingResult.Results;
    }
}