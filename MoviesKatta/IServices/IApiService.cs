namespace MoviesKatta.IServices;

public interface IApiService
{
    Task<YoutubeModels> SearchVideos(string searchQuery, string nextPageToken = "");
}