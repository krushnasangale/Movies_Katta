namespace MoviesKatta.IServices;

public interface IApiService
{
    Task<YoutubeSearchResult> SearchVideos(string searchQuery, string nextPageToken = "");
    Task<ChannelSearchResult> GetChannels(string channelIds);
}