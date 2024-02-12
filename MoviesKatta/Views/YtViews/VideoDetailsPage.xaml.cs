using VideoDetailsPageViewModel = MoviesKatta.ViewModels.YtViewModels.VideoDetailsPageViewModel;

namespace MoviesKatta.Views.YtViews;

public partial class VideoDetailsPage
{
    public VideoDetailsPage(object initParams) : base(initParams)
    {
        InitializeComponent();

        ViewModelInitialized += (_, _) =>
        {
            ((VideoDetailsPageViewModel)BindingContext).DownloadCompleted += VideoDetailsPage_DownloadCompleted;
        };
    }

    protected override void OnDisappearing()
    {
        ((VideoDetailsPageViewModel)BindingContext).DownloadCompleted -= VideoDetailsPage_DownloadCompleted;
        VideoPlayer.Stop();
        base.OnDisappearing();
    }

    private void VideoDetailsPage_DownloadCompleted(object sender, EventArgs e)
    {
        //Video information downloaded. Start showing items.
        if (((VideoDetailsPageViewModel)BindingContext).IsErrorState) return;
        if (this.AnimationIsRunning("TransitionAnimation"))return;
        var parentAnimation = new Animation();

        //Poster Image Animation
        parentAnimation.Add(0.0, 0.7,
            new Animation(v => HeaderView.Opacity = v, 0, 1, Easing.CubicIn));

        //Video Title Container Animation
        parentAnimation.Add(0.3, 0.7,
            new Animation(v => VideoTitle.Opacity = v, 0, 1, Easing.CubicIn));

        //ChannelDetails Container Animation
        parentAnimation.Add(0.4, 0.7,
            new Animation(v => ChannelDetails.Opacity = v, 0, 1, Easing.CubicIn));

        //Video Icons Animation
        parentAnimation.Add(0.5, 0.7,
            new Animation(v => VideoIcons.Opacity = v, 0, 1, Easing.CubicIn));
        
        // Channel Details Animation
        parentAnimation.Add(0.6, 0.8,
             new Animation(v => LstVideos.Opacity = v, 0, 1, Easing.CubicIn));
        
        // Tags Animation
        parentAnimation.Add(0.65, 0.85,
            new Animation(v => TagsView.Opacity = v, 0, 1, Easing.CubicIn));
        
         //Comments Button Animation
         parentAnimation.Add(0.8, 1,
             new Animation(v => LstVideos.Opacity = v, 0, 1, Easing.CubicIn));

        //Commit the animation
        parentAnimation.Commit(this, "TransitionAnimation", 16, Constants.ExtraLongDuration, null,
            (_, _) =>
            {
                //Action to perform on completion (if any)
            });
    }

    private async void BtnComments_OnClicked(object sender, EventArgs e)
    {
        DrawerIndicator.IsRunning = true;
        DrawerIndicator.IsVisible = true;
        CommentsBottomSheet.HalfExpandedRatio = 0.6;
        CommentsBottomSheet.State = BottomSheetState.HalfExpanded;
        await Task.Delay(500);
        await ViewModel.GetComments();
        await Task.Delay(500);
        DrawerIndicator.IsRunning = false;
        DrawerIndicator.IsVisible = false;
    }
    private void BtnDescription_OnClicked(object sender, EventArgs e)
    {
        DescriptionDrawerIndicator.IsRunning = true;
        DescriptionDrawerIndicator.IsVisible = true;
        DescriptionBottomSheet.HalfExpandedRatio = 0.6;
        DescriptionBottomSheet.State = BottomSheetState.HalfExpanded;
        DescriptionDrawerIndicator.IsRunning = false;
        DescriptionDrawerIndicator.IsVisible = false;
    }
    async void VideoPlayerButton_Clicked(Object sender, EventArgs e)
    {
        VideoIndicator.IsVisible = true;
        VideoIndicator.IsRunning = true;
        HeaderView.IsVisible = false;
        Duration.IsVisible = false;
        PlayButton.IsVisible = false;
        await ViewModel.GetVideoUrl();
        VideoIndicator.IsRunning = false;
        VideoIndicator.IsVisible = false;
        VideoPlayer.IsVisible = true;
        VideoPlayer.Play();
    }
}