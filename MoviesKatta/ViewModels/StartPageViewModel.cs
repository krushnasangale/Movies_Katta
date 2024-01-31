namespace MoviesKatta.ViewModels;

public partial class StartPageViewModel : AppViewModelBase
{
    [ObservableProperty] string _nextToken = string.Empty;
    [ObservableProperty] string _searchTerm = "Bollywood songs";
    [ObservableProperty] ObservableCollection<Item> _youtubeVideos;
    [ObservableProperty] bool _isLoadingMore;

    public StartPageViewModel(IApiService appApiService) : base(appApiService)
    {
        Title = "Movies Katta";
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
            await GetYoutubeVideo();
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
        if (IsLoadingMore || string.IsNullOrEmpty(NextToken)) return;
        IsLoadingMore = true;
        await Task.Delay(500);
        await GetYoutubeVideo();
        IsLoadingMore = false;
    }

    [RelayCommand]
    private async Task SearchVideos(string searchQuery)
    {
        NextToken = string.Empty;
        SearchTerm = searchQuery.Trim();
        await Search();
    }

    [RelayCommand]
    private async Task NavigateToVideoDetailsPage(string videoId)
    {
        await NavigationService.PushAsync(new VideoDetailsPage(videoId), false);
    }
}