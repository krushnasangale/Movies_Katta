﻿namespace MoviesKatta.ViewModels;

public partial class VideoDetailsPageViewModel : AppViewModelBase
{
    [ObservableProperty] private YoutubeVideoDetail _theVideo;
    [ObservableProperty] private List<Item> _similarVideos;
    [ObservableProperty] private Channel _theChannel;
    [ObservableProperty] private List<Comment> _comments;
    [ObservableProperty] private string _videoSource;
    public event EventHandler DownloadCompleted;

    public VideoDetailsPageViewModel(IApiService apiService) : base(apiService)
    {
        Title = "Movies Katta";
    }

    public override async void OnNavigatedTo(object parameters)
    {
        var videoId = (string)parameters;
        SetDataLoadingIndicators();
        LoadingText = "Hold on while we load the video details...";
        try
        {
            SimilarVideos = new();
            //Get Video Details
            TheVideo = await _appApiService.GetVideoDetails(videoId);

            //Get Channel URL and set in the video
            var channelSearchResult = await _appApiService.GetChannels(TheVideo.Snippet.channelId);
            TheChannel = channelSearchResult.Items.First();

            //Find Similar Videos
            if (TheVideo.Snippet.Tags is not null)
            {
                var similarVideosSearchResult = 
                    await _appApiService.SearchVideos(TheVideo.Snippet.Tags.First(), "");
                var nextPageToken = similarVideosSearchResult.nextPageToken;
                Console.WriteLine(nextPageToken);
                SimilarVideos = similarVideosSearchResult.items;
            }
            
            //Get Comments
            await GetComments(videoId);
            
            //Raise Data Load completed event to the UI
            DataLoaded = true;

            //Raise the Event to notify the UI of download completion
            DownloadCompleted?.Invoke(this, new EventArgs());
        }
        catch (InternetConnectionException ex)
        {
            IsErrorState = true;
            ErrorMessage = "Slow or no Internet connect." + Environment.NewLine +
                           "Please check your connection and try again.";
            ErrorImage = "nointernet.png";
            Console.WriteLine(ex);
        }
        catch (Exception exception)
        {
            IsErrorState = true;
            ErrorMessage =
                $"Something went wrong. If the problem persists, plz contact support at {Constants.EmailAddress} with the error message:" +
                Environment.NewLine + Environment.NewLine + exception.Message;
            ErrorImage = "error.png";
        }
        finally
        {
            SetDataLoadingIndicators(false);
        }
    }

    private async Task GetComments(string videoId)
    {
        try
        {
            var commentsSearchResult = await _appApiService.GetComments(videoId);
            Comments = commentsSearchResult.Items;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    [RelayCommand]
    private async Task UnlikeVideo()
    {
        await PageService.DisplayAlert("Coming Soon",
            "The unlike option is coming soon, once we implement the OAuth login functionality.",
            "OK");
    }

    [RelayCommand]
    private async Task ShareVideo()
    {
        var textToShare =
            $"Hey, I found this amazing video. check it out: https://www.youtube.com/watch?v={TheVideo.Id}";

        //Share
        await Share.RequestAsync(new ShareTextRequest
        {
            Text = textToShare,
            Title = TheVideo.Snippet.title
        });
    }

    [RelayCommand]
    private async Task DownloadVideo()
    {
        
    }
    
    [RelayCommand]
    private async Task SubscribeChannel()
    {
        await PageService.DisplayAlert(
            "Coming Soon",
            "The subscribe to channel option is coming soon, once we implement the OAuth login functionality.",
            "OK");
    }
    
    [RelayCommand]
    private async Task NavigateToVideoDetailsPage(string videoID)
    {
        await NavigationService.PushAsync(new VideoDetailsPage(videoID));
    }
}