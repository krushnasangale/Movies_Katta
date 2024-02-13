using System.Net.Http.Headers;
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
        var trendingUrl = $"https://api.themoviedb.org/3/discover/movie?include_adult=false&include_video=true&language=en-US&page=1&sort_by=popularity.desc";
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiJkZWRhMzBjMWQ2YzMzNGZkMWQ5ODZlYzAwY2E5NWFlYyIsInN1YiI6IjY1YmI1YmFmNDU5YWQ2MDE0NzZhZmI5YiIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.Oz7Fxt-sG21jCyjOP6oEj2nUVTc_GqIe6AGi_UCcuXE");
        var trendingResult = await httpClient.GetFromJsonAsync<Movies>(trendingUrl);
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