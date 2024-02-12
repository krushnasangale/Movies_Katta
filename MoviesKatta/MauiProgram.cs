using MoviesKatta.Services.TmdbServices;
using VideoDetailsPageViewModel = MoviesKatta.ViewModels.YtViewModels.VideoDetailsPageViewModel;

namespace MoviesKatta
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            ThemeManager.ApplyThemeToSystemBars = true;
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseDevExpress(useLocalization: true)
                .UseMauiCompatibility()
                .UseMauiCommunityToolkit()
                .UseMauiCommunityToolkitMediaElement()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("FiraSans-Light.ttf", "RegularFont");
                    fonts.AddFont("FiraSans-Medium.ttf", "MediumFont");
                }).ConfigureLifecycleEvents(events =>
                {
#if ANDROID
                    events.AddAndroid(android =>
                        android.OnCreate((activity, _) => MakeStatusBarTranslucent(activity)));

                    static void MakeStatusBarTranslucent(Android.App.Activity activity)
                    {
                        if (activity.Window == null) return;
                        activity.Window.SetFlags(Android.Views.WindowManagerFlags.LayoutNoLimits,
                            Android.Views.WindowManagerFlags.LayoutNoLimits);

                        activity.Window.ClearFlags(Android.Views.WindowManagerFlags.TranslucentStatus);

                        activity.Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
                    }
#endif
                });

            RegisterServices(builder.Services);
            return builder.Build();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            //Add Platform specific Dependencies
            services.AddSingleton(Connectivity.Current);

            //Register Cache Barrel
            Barrel.ApplicationId = Constants.ApplicationId;
            services.AddSingleton(Barrel.Current);
            services.AddHttpClient(TmdbService.TmdbHttpClientName,
                httpClient => httpClient.BaseAddress = new Uri("https://api.themoviedb.org"));

            //Register API Service
            services.AddSingleton<IApiService, YoutubeService>();
            services.AddSingleton<TmdbService>();

            // Register FileDownloadService
            services.AddSingleton<IDownloadFileService, FileDownloadService>();

            // //Register View Models
            services.AddSingleton<StartPageViewModel>();
            services.AddTransient<VideoDetailsPageViewModel>();
        }
    }
}