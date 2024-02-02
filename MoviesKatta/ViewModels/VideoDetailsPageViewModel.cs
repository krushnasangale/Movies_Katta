using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace MoviesKatta.ViewModels;

public partial class VideoDetailsPageViewModel : AppViewModelBase
{
    [ObservableProperty] private YoutubeVideoDetail _theVideo;
    [ObservableProperty] string _nextToken = string.Empty;
    [ObservableProperty] private List<Item> _similarVideos;
    [ObservableProperty] private Channel _theChannel;
    [ObservableProperty] private List<Comment> _comments;
    [ObservableProperty] private string _videoSource;
    private IEnumerable<MuxedStreamInfo> streamInfos;
    [ObservableProperty] private double _progressValue;
    [ObservableProperty] private bool _isDownloading;
    [ObservableProperty] private string _videoId;
    public event EventHandler DownloadCompleted;
    private IDownloadFileService _fileDownloadService;
    private ToastService _toastService = new();
    [ObservableProperty] bool _isLoadingMore;

    public VideoDetailsPageViewModel(IApiService apiService, IDownloadFileService fileDownloadService) : base(apiService)
    {
        _fileDownloadService = fileDownloadService;
        Title = "Movies Katta";
    }

    public override async void OnNavigatedTo(object parameters)
    {
        await Task.Delay(500);
        VideoId = (string)parameters;
        SetDataLoadingIndicators();
        LoadingText = "Hold on while we load the video details...";
        try
        {
            SimilarVideos = new();
            //Get Video Details
            TheVideo = await _appApiService.GetVideoDetails(VideoId);

            //Get Channel URL and set in the video
            var channelSearchResult = await _appApiService.GetChannels(TheVideo.Snippet.channelId);
            TheChannel = channelSearchResult.Items.First();

            //Find Similar Videos
            if (TheVideo.Snippet.Tags is not null)
            {
                await GetYoutubeVideo(TheVideo.Snippet.Tags.First(), "");
            }
            else
            {
                var searchText = Preferences.Get("SearchQueryText", string.Empty);
                await GetYoutubeVideo(searchText, "");
            }

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

    public async Task GetComments()
    {
        try
        {
            var commentsSearchResult = await _appApiService.GetComments(VideoId);
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
        if (IsDownloading) return;

        var progressIndicator = new Progress<double>(value => ProgressValue = value);
        var cancellationToken = new CancellationTokenSource().Token;
        try
        {
            IsDownloading = true;

            // Create a folder in local storage
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "DownloadedVideos");
            Directory.CreateDirectory(folderPath);

            if (streamInfos is null) await GetVideoUrl();
            // Get the highest resolution video URL
            var urlToDownload = streamInfos.OrderByDescending(video => video.VideoResolution.Area).First().Url;

            // Download the file
            var downloadedFilePath = await _fileDownloadService.DownloadFileAsync(
                urlToDownload,
                Path.Combine(folderPath, TheVideo.Snippet.title.CleanCacheKey() + ".mp4"),
                progressIndicator, cancellationToken);

            // save file in local storage
            await _toastService.ShowToast("Download Completed!");

            // Save the file
            await Share.RequestAsync(new ShareFileRequest
            {
                File = new ShareFile(downloadedFilePath),
                Title = TheVideo.Snippet.title,
            });
        }
        catch (OperationCanceledException ex)
        {
            await _toastService.ShowToast("Download Cancelled. Please try again.");
        }
        catch (Exception exception)
        {
            await _toastService.ShowToast("Download Cancelled. Please try again.");
        }
        finally
        {
            IsDownloading = false;
        }
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
    private async Task NavigateToVideoDetailsPage(string videoId)
    {
        await NavigationService.PushAsync(new VideoDetailsPage(videoId));
    }

    public async Task GetVideoUrl()
    {
        try
        {
            await Task.Run(async () =>
            {
                var youtube = new YoutubeClient();
                var videoUrl = $"https://youtube.com/watch?v={TheVideo.Id}";
                var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoUrl);

                // Get highest quality muxed stream
                streamInfos = streamManifest.GetMuxedStreams();
                var videoPlayerStream =
                    streamInfos.First(video => video.VideoQuality.Label is "240p" or "360p" or "480p");

                // Update UI elements or perform any actions with the obtained data
                Device.BeginInvokeOnMainThread(() => { VideoSource = videoPlayerStream.Url; });
            });
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

    [RelayCommand]
    private async Task LoadMoreVideos()
    {
        if (string.IsNullOrEmpty(NextToken)) return;
        IsLoadingMore = true;
        await Task.Delay(500);
        await GetYoutubeVideo(TheVideo.Snippet.Tags.First(), NextToken);
        IsLoadingMore = false;
    }
    private async Task GetYoutubeVideo(string searchQuery, string nextToken)
    {
        var similarVideosSearchResult =
            await _appApiService.SearchVideos(searchQuery, nextToken);
        NextToken = similarVideosSearchResult.nextPageToken;
        SimilarVideos = similarVideosSearchResult.items;
    }
}