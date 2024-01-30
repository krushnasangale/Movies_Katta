using DevExpress.Maui.Controls;

namespace MoviesKatta.Views;

public partial class VideoDetailsPage
{
    public VideoDetailsPage(object initParams) : base(initParams)
    {
        InitializeComponent();

        ViewModelInitialized += (s, e) =>
        {
            ((VideoDetailsPageViewModel)BindingContext).DownloadCompleted += VideoDetailsPage_DownloadCompleted;
        };
    }

    protected override void OnDisappearing()
    {
        ((VideoDetailsPageViewModel)BindingContext).DownloadCompleted -= VideoDetailsPage_DownloadCompleted;
        base.OnDisappearing();
    }

    private void VideoDetailsPage_DownloadCompleted(object sender, EventArgs e)
    {
        //Video information downloaded. Start showing items.

        if (((VideoDetailsPageViewModel)BindingContext).IsErrorState)
            return;

        if (this.AnimationIsRunning("TransitionAnimation"))
            return;

        var parentAnimation = new Animation();

        //Poster Image Animation
        // parentAnimation.Add(0.0, 0.7,
        //     new Animation(v => HeaderView.Opacity = v, 0, 1, Easing.CubicIn));

        //Video Title Container Animation
        parentAnimation.Add(0.4, 0.7,
            new Animation(v => VideoTitle.Opacity = v, 0, 1, Easing.CubicIn));

        //Video Icons Animation
        parentAnimation.Add(0.5, 0.7,
            new Animation(v => VideoIcons.Opacity = v, 0, 1, Easing.CubicIn));

        //Channel Details Animation
        parentAnimation.Add(0.6, 0.8,
            new Animation(v => ChannelDetails.Opacity = v, 0, 1, Easing.CubicIn));
        
        //Channel Details Animation
        // parentAnimation.Add(0.6, 0.8,
        //     new Animation(v => LstVideos.Opacity = v, 0, 1, Easing.CubicIn));

         //Similar Videos Animation
         parentAnimation.Add(0.7, 0.9,
             new Animation(v => SimilarVideos.Opacity = v, 0, 1, Easing.CubicIn));
        
         //Tags Animation
         parentAnimation.Add(0.65, 0.85,
             new Animation(v => TagsView.Opacity = v, 0, 1, Easing.CubicIn));

        // Description View Animation
         parentAnimation.Add(0.8, 1,
             new Animation(v => DescriptionView.Opacity = v, 0, 1, Easing.CubicIn));

         //Comments Button Animation
         parentAnimation.Add(0.8, 1,
             new Animation(v => BtnComments.Opacity = v, 0, 1, Easing.CubicIn));


        //Commit the animation
        parentAnimation.Commit(this, "TransitionAnimation", 16, Constants.ExtraLongDuration, null,
            (v, c) =>
            {
                //Action to perform on completion (if any)
            });
    }

    private void BtnComments_OnClicked(object sender, EventArgs e)
    {
        CommentsBottomSheet.HalfExpandedRatio = 0.6;
        CommentsBottomSheet.State = BottomSheetState.HalfExpanded;
    }
}