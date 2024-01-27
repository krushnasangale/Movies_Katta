using System.Text.Json.Serialization;

namespace MoviesKatta.Models;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class Default
{
    public string url { get; set; }
    public int width { get; set; }
    public int height { get; set; }
}

public class High
{
    public string url { get; set; }
    public int width { get; set; }
    public int height { get; set; }
}

public class Id
{
    public string kind { get; set; }
    public string videoId { get; set; }
}

public class Item
{
    public string kind { get; set; }
    public string etag { get; set; }
    public Id id { get; set; }
    public Snippet snippet { get; set; }
}

public class Medium
{
    public string url { get; set; }
    public int width { get; set; }
    public int height { get; set; }
}

public class PageInfo
{
    public int totalResults { get; set; }
    public int resultsPerPage { get; set; }
}

public class YoutubeSearchResult
{
    public string kind { get; set; }
    public string etag { get; set; }
    public string nextPageToken { get; set; }
    public string regionCode { get; set; }
    public PageInfo pageInfo { get; set; }
    public List<Item> items { get; set; }
}

public class Snippet
{
    public DateTime publishedAt { get; set; }
    public string channelId { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public Thumbnails thumbnails { get; set; }
    public string channelTitle { get; set; }
    public string liveBroadcastContent { get; set; }
    public DateTime publishTime { get; set; }
    [Newtonsoft.Json.JsonIgnore] public string channelImageUrl { get; set; }
    //For Details
    [JsonPropertyName("tags")]
    public List<string> Tags { get; set; }

    //For Comments
    [JsonPropertyName("topLevelComment")]
    public TopLevelComment TopLevelComment { get; set; }

    [JsonPropertyName("textDisplay")]
    public string TextDisplay { get; set; }

    [JsonPropertyName("authorDisplayName")]
    public string AuthorDisplayName { get; set; }

    [JsonPropertyName("authorProfileImageUrl")]
    public string AuthorProfileImageUrl { get; set; }

    [JsonPropertyName("likeCount")]
    public int LikeCount { get; set; }
}

public class Thumbnails
{
    public Default @default { get; set; }
    public Medium medium { get; set; }
    public High high { get; set; }
}

//Channel related models

public class ChannelSearchResult
{
    [JsonPropertyName("items")] public List<Channel> Items { get; set; }
}

public class Channel
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonPropertyName("snippet")] public Snippet Snippet { get; set; }

    [JsonPropertyName("statistics")] public Statistics Statistics { get; set; }

    // public string SubscribersCount
    // {
    //     get => $"{Statistics.SubscriberCount} subscribers";
    // }
}

//Video Details related models
public class VideoDetailsResult
{
    [JsonPropertyName("items")]
    public List<YoutubeVideoDetail> Items { get; set; }
}

public class YoutubeVideoDetail
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("snippet")]
    public Snippet Snippet { get; set; }

    //For Details
    [JsonPropertyName("statistics")]
    public Statistics Statistics { get; set; }

    [JsonPropertyName("contentDetails")]
    public ContentDetails ContentDetails { get; set; }

    public string VideoSubtitle
    {
        get => $"{Statistics.ViewCount.FormattedNumber()} views | {Snippet.publishedAt.ToTimeAgo()}";
    }
    
    public string LikesCount
    {
        get => Statistics.LikeCount.FormattedNumber();
    }
    
    public string VideoDuration
    {
        get => ContentDetails.Duration.ToTimeSpan().ToReadableString();
    }
    
    public string CommentsCount
    {
        get => Statistics.CommentCount.FormattedNumber();
    }
}

public class ContentDetails
{
    [JsonPropertyName("duration")]
    public string Duration { get; set; }
}

public class Statistics
{
    [JsonPropertyName("viewCount")]
    public string ViewCount { get; set; }

    [JsonPropertyName("likeCount")]
    public string LikeCount { get; set; }

    [JsonPropertyName("commentCount")]
    public string CommentCount { get; set; }

    [JsonPropertyName("subscriberCount")]
    public string SubscriberCount { get; set; }

}
//Comments related models
public class CommentsSearchResult
{
    [JsonPropertyName("nextPageToken")]
    public string NextPageToken { get; set; }

    [JsonPropertyName("items")]
    public List<Comment> Items { get; set; }
}

public class Comment
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("snippet")]
    public Snippet Snippet { get; set; }
}
public class TopLevelComment
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("snippet")]
    public Snippet Snippet { get; set; }
}