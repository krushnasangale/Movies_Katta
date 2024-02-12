using Newtonsoft.Json;

namespace MoviesKatta.Models.TmdbModels;

public class Movies
{
    [JsonProperty("page")] public int Page { get; set; }

    [JsonProperty("results")] public List<MovieResult> Results { get; set; }

    [JsonProperty("total_pages")] public int TotalPages { get; set; }

    [JsonProperty("total_results")] public int TotalResults { get; set; }
}

public class MovieResult
{
    [JsonProperty("adult")] public bool Adult { get; set; }
    [JsonProperty("backdrop_path")] public string BackdropPath { get; set; }

    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("title")] public string Title { get; set; }

    [JsonProperty("original_language")] public string OriginalLanguage { get; set; }
    [JsonProperty("original_title")] public string OriginalTitle { get; set; }

    [JsonProperty("overview")] public string Overview { get; set; }
    [JsonProperty("poster_path")] public string PosterPath { get; set; }

    [JsonProperty("media_type")] public string MediaType { get; set; }

    [JsonProperty("genre_ids")] public List<int> GenreIds { get; set; }

    [JsonProperty("popularity")] public double Popularity { get; set; }

    [JsonProperty("release_date")] public string ReleaseDate { get; set; }

    [JsonProperty("video")] public bool Video { get; set; }

    [JsonProperty("vote_average")] public double VoteAverage { get; set; }

    [JsonProperty("vote_count")] public int VoteCount { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("original_name")] public string OriginalName { get; set; }

    [JsonProperty("first_air_date")] public string FirstAirDate { get; set; }

    [JsonProperty("origin_country")] public List<string> OriginCountry { get; set; }
    [JsonIgnore] public string ThumbnailPath => PosterPath ?? BackdropPath;
    
    [JsonIgnore] public string Thumbnail => $"https://image.tmdb.org/t/p/w600_and_h900_bestv2/{ThumbnailPath}";
    
    [JsonIgnore] public string ThumbnailSmall => $"https://image.tmdb.org/t/p/w220_and_h330_face/{ThumbnailPath}";
    
    [JsonIgnore] public string ThumbnailUrl => $"https://image.tmdb.org/t/p/original/{ThumbnailPath}";
    
    [JsonIgnore] public string DisplayTitle => Title ?? Name ?? OriginalTitle ?? OriginalName;
}