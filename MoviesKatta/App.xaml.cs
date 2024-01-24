using MoviesKatta.Views;

namespace MoviesKatta
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            //Enable version tracking
            VersionTracking.Track();
            MainPage = new NavigationPage(new StartPage());
        }
    }
}