namespace MoviesKatta.Services;

public class ToastService
{
    public async Task ShowToast(string msg)
    {
        ToastDuration duration = ToastDuration.Long;
        double fontSize = 14;
        var toast = Toast.Make(msg, duration, fontSize);
        await toast.Show();
    }
}