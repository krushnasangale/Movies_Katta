using System.Net;
using Maui.Apps.Framework.Services;
using MonkeyCache;
using MoviesKatta.IServices;

namespace MoviesKatta.Services;

public class YoutubeService : RestServiceBase, IApiService
{
    public YoutubeService(IConnectivity connectivity, IBarrel cacheBarrel) : base(connectivity, cacheBarrel)
    {
        SetBaseUrl(Constants.ApiServiceUrl);
    }

    public async Task<YoutubeSearchResult> SearchVideos(string searchQuery, string nextPageToken = "")
    {
        var resourceUri =
            $"search?part=snippet&maxResults=10&type=video&key={Constants.ApiKey}&q={WebUtility.UrlEncode(searchQuery)}"
            +
            (!string.IsNullOrEmpty(nextPageToken) ? $"&pageToken={nextPageToken}" : "");

        var result = await GetAsync<YoutubeSearchResult>(resourceUri, 4);
        return result;
    }

    public async Task<ChannelSearchResult> GetChannels(string channelIds)
    {
        var resourceUri = $"channels?part=snippet&id={channelIds}&key={Constants.ApiKey}";
        var result = await GetAsync<ChannelSearchResult>(resourceUri, 4);
        return result;
    }

    public async Task<YoutubeVideoDetail> GetVideoDetails(string videoId)
    {
        var resourceUri = $"videos?part=contentDetails,id,snippet,statistics&key={Constants.ApiKey}&id={videoId}";
        var result = await GetAsync<VideoDetailsResult>(resourceUri, 24); //Cached for 24 hours
        return result.Items.First();
    }

    public async Task<CommentsSearchResult> GetComments(string videoId)
    {
        var resourceUri = $"commentThreads?part=snippet&maxResults=100&key={Constants.ApiKey}&videoId={videoId}";
        var result = await GetAsync<CommentsSearchResult>(resourceUri, 4); //Cached for 4 hours
        return result;
    }
}