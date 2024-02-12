namespace MoviesKatta.Models;

public class Constants
{
    public static string ApplicationName = "Movies Katta";
    public static string EmailAddress = @"krushnasangale7447@gmail.com";
    public static string ApplicationId = "com.KD.MoviesKatta";
    public static string ApiServiceUrl = @"https://youtube.googleapis.com/youtube/v3/";
    public static string ApiKey = "AIzaSyBFyMSvgeb3kkSx8Yx4oPZJQCbnP8fOxOI";


    public static uint MicroDuration { get; set; } = 100;
    public static uint SmallDuration { get; set; } = 300;
    public static uint MediumDuration { get; set; } = 600;
    public static uint LongDuration { get; set; } = 1200;
    public static uint ExtraLongDuration { get; set; } = 1800;
    
    
    // Search types 
    public static string SearchVideo = "video";
    public static string SearchChannel = "channel";
    public static string SearchPlaylist = "playlist";
    
    
    //Tmdb strings
    public const string Trending = "3/trending/all/week?language=en-US";
    public const string NetflixOriginals = "3/discover/tv?language=en-US&with_networks=213";
    public const string TopRated = "3/movie/top_rated?language=en-US";
    public const string Action = "3/discover/movie?language=en-US&with_genres=28";
    public const string MovieGenres = "3/genre/movie/list?language=en-US";

    public static string GetTrailers(int movieId, string type = "movie") => $"3/{type ?? "movie"}/{movieId}/videos?language=en-US";
    public static string GetMovieDetails(int movieId, string type = "movie") => $"3/{type ?? "movie"}/{movieId}?language=en-US";
    public static string GetSimilar(int movieId, string type = "movie") => $"3/{type ?? "movie"}/{movieId}/similar?language=en-US";
}