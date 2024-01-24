namespace MoviesKatta.Models;

public class Constants
{
    public static string ApplicationName = "Movies Katta";
    public static string EmailAddress = @"krushnasangale7447@gmail.com";
    public static string ApplicationId = "com.KD.MoviesKatta";
    public static string ApiServiceUrl = @"https://youtube.googleapis.com/youtube/v3/";
    public static string ApiKey = @"AIzaSyAb0WtMf4weEkSNs5lBZbd7QzQ0uNwpJW4";


    public static uint MicroDuration { get; set; } = 100;
    public static uint SmallDuration { get; set; } = 300;
    public static uint MediumDuration { get; set; } = 600;
    public static uint LongDuration { get; set; } = 1200;
    public static uint ExtraLongDuration { get; set; } = 1800;
    
    
    // Search types 
    public static string SearchVideo = "video";
    public static string SearchChannel = "channel";
    public static string SearchPlaylist = "playlist";
}