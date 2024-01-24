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

    public async Task<YoutubeModels> SearchVideos(string searchQuery, string nextPageToken = "")
    {
        var resourceUri =
            $"search?part=snippet&maxResults=10&q={WebUtility.UrlEncode(searchQuery)}&type=video&key={Constants.ApiKey}"
            + (!string.IsNullOrEmpty(nextPageToken)? $"&pageToken={nextPageToken}" : "");

        var result = await GetAsync<YoutubeModels>(resourceUri, 4);
        return result;
    }
}