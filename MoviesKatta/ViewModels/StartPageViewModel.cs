using MoviesKatta.Models.TmdbModels;
using MoviesKatta.Services.TmdbServices;

namespace MoviesKatta.ViewModels;

public partial class StartPageViewModel : AppViewModelBase
{
    [ObservableProperty] string _nextToken = string.Empty;
    [ObservableProperty] string _searchTerm = "Bollywood songs";
    [ObservableProperty] ObservableCollection<Item> _youtubeVideos;
    [ObservableProperty] bool _isLoadingMore;
    private readonly TmdbService _tmdbService;
    [ObservableProperty] ObservableCollection<MovieResult> _trendingMovies;
    [ObservableProperty] ObservableCollection<MovieResult> _netflixMovies;
    [ObservableProperty] ObservableCollection<MovieResult> _actionMovies;
    [ObservableProperty] ObservableCollection<MovieResult> _topRatedMovies;
    [ObservableProperty] MovieResult _trendingMovie;

    public StartPageViewModel(IApiService appApiService, TmdbService tmdbService) : base(appApiService)
    {
        Title = "Movies Katta";
        _tmdbService = tmdbService;
    }

    public override async void OnNavigatedTo(object parameters)
    {
        await Search();
    }

    private async Task Search()
    {
        SetDataLoadingIndicators();
        LoadingText = "Hold on while we search for youtube videos...";
        YoutubeVideos = [];
        try
        {
            IsLoadingMore = true;
            await GetYoutubeVideo();
            await GetTrendingMovies();
            await GetActionMovies();
            await GetTopRatedMovies();
            await GetNetflixMovies();
            IsLoadingMore = false;
            DataLoaded = true;
        }
        catch (InternetConnectionException ex)
        {
            IsErrorState = true;
            ErrorMessage = "Slow or no Internet connect." + Environment.NewLine +
                           "Please check your connection and try again.";
            ErrorImage = "nointernet.png";
            Console.WriteLine(ex);
        }
        catch (Exception ex)
        {
            IsErrorState = true;
            ErrorMessage =
                $"Something went wrong. If the problem persists, plz contact support at {Constants.EmailAddress} with the error message:" +
                Environment.NewLine + Environment.NewLine + ex.Message;
            ErrorImage = "error.png";
        }
        finally
        {
            SetDataLoadingIndicators(false);
        }
    }

    private async Task GetTrendingMovies()
    {
        var result = await _tmdbService.GetTrendingMovies();
        if (result == null) return;
        TrendingMovies = new ObservableCollection<MovieResult>(result);
        if (TrendingMovies is { Count: > 0 }) TrendingMovie = TrendingMovies[0];
    }

    private async Task GetActionMovies()
    {
        var result = await _tmdbService.GetActionMovies();
        if (result == null) return;
        ActionMovies = new ObservableCollection<MovieResult>(result);
    }

    private async Task GetTopRatedMovies()
    {
        var result = await _tmdbService.GetTopRatedMovies();
        if (result == null) return;
        TopRatedMovies = new ObservableCollection<MovieResult>(result);
    }

    private async Task GetNetflixMovies()
    {
        var result = await _tmdbService.GetNetflixMovies();
        if (result == null) return;
        NetflixMovies = new ObservableCollection<MovieResult>(result);
    }

    private async Task GetYoutubeVideo()
    {
        var videoSearchResult = await _appApiService.SearchVideos(SearchTerm, NextToken);
        NextToken = videoSearchResult.nextPageToken;
        //Get Channel URLs
        var channelIDs = string.Join(",",
            videoSearchResult.items.Select(video => video.snippet.channelId).Distinct());

        var channelSearchResult = await _appApiService.GetChannels(channelIDs);

        //Set Channel URL in the Video
        videoSearchResult.items.ForEach(video =>
            video.snippet.channelImageUrl = channelSearchResult.Items
                .First(channel => channel.Id == video.snippet.channelId).Snippet.thumbnails.high.url);

        YoutubeVideos.AddRange(videoSearchResult.items);
    }

    [RelayCommand]
    private async Task OpenSettingsPage()
    {
        await PageService.DisplayAlert("Alert", "Coming soon", "Cancel");
    }

    [RelayCommand]
    private async Task LoadMoreVideos()
    {
        if (string.IsNullOrEmpty(NextToken)) return;
        IsLoadingMore = true;
        await Task.Delay(500);
        await GetYoutubeVideo();
        IsLoadingMore = false;
    }

    [RelayCommand]
    private async Task SearchVideos(string searchQuery)
    {
        IsLoadingMore = true;
        NextToken = string.Empty;
        SearchTerm = searchQuery.Trim();
        Preferences.Set("SearchQueryText", SearchTerm);
        await Search();
        IsLoadingMore = false;
    }

    [RelayCommand]
    private async Task NavigateToVideoDetailsPage(string videoId)
    {
        await NavigationService.PushAsync(new VideoDetailsPage(videoId), false);
    }
}