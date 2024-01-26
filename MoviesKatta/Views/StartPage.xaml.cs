namespace MoviesKatta.Views;

public partial class StartPage : ViewBase<StartPageViewModel>
{
    public static BindableProperty ItemsHeightProperty = BindableProperty.Create(
        nameof(ItemsHeight),
        typeof(double),
        typeof(StartPage),
        null,
        BindingMode.OneWay);

    public double ItemsHeight
    {
        get => (double)GetValue(ItemsHeightProperty);
        set => SetValue(ItemsHeightProperty, value);
    }
    public StartPage()
    {
        InitializeComponent();
    }
    
    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        ItemsHeight = 60d + (width - LstVideos.Margin.Right - LstVideos.Margin.Left) / 1.8d;
    }

    async void SearchBar_OnCompleted(object sender, EventArgs e)
    {
        await ViewModel.SearchVideosCommand.ExecuteAsync(SearchBar.Text);
    }
}