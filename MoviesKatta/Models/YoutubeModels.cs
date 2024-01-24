namespace MoviesKatta.Models;

public class YoutubeModels
{
    [JsonProperty("nextPageToken")] public string NextPageToken { get; set; }

    [JsonProperty("pageInfo")] public PageInfo PageInfo { get; set; }

    [JsonProperty("items")] public List<YoutubeVideo> Items { get; set; }
}

public class Thumbnail
{
    [JsonProperty("url")] public string Url { get; set; }
}

public class Id
{
    [JsonProperty("videoId")] public string VideoId { get; set; }
}

public class YoutubeVideo
{
    [JsonProperty("id")] public Id Id { get; set; }

    [JsonProperty("snippet")] public Snippet Snippet { get; set; }
}

public class PageInfo
{
    [JsonProperty("totalResults")] public int TotalResults { get; set; }

    [JsonProperty("resultsPerPage")] public int ResultsPerPage { get; set; }
}

public class Snippet
{
    [JsonProperty("publishedAt")] public DateTime PublishedAt { get; set; }

    [JsonProperty("channelId")] public string ChannelId { get; set; }

    [JsonProperty("title")] public string Title { get; set; }

    [JsonProperty("description")] public string Description { get; set; }

    [JsonProperty("thumbnails")] public Thumbnails Thumbnails { get; set; }

    [JsonProperty("channelTitle")] public string ChannelTitle { get; set; }
}

public class Thumbnails
{
    [JsonProperty("medium")] public Thumbnail Medium { get; set; }

    [JsonProperty("high")] public Thumbnail High { get; set; }
}