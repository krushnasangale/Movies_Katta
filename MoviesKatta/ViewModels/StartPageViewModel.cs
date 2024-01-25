using Maui.Apps.Framework.Exceptions;

namespace MoviesKatta.ViewModels;

public class StartPageViewModel : AppViewModelBase
{
    public StartPageViewModel(IApiService appApiService) : base(appApiService)
    {
        Title = "Movies Katta";
    }

    public override async void OnNavigatedTo(object parameters)
    {
        SetDataLoadingIndicators(true);
        LoadingText = "Hold on we are loading";

        try
        {
            await Task.Delay(5000);
            throw new InternetConnectionException();
        }
        catch (InternetConnectionException ex)
        {
            IsErrorState = true;
            ErrorMessage = "Slow or no Internet connect." + Environment.NewLine +
                           "Please check your connection and try again.";
            ErrorImage = "nointernet.png";
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
}