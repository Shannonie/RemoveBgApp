namespace RemoveBgAPI.Utils
{
    public class FileSaver : IFileSaver
    {
        public Task SaveFileAsync(byte[] bytes, string filename)
        {
            return Application.Current.MainPage.DisplayAlert(
                "Not supported",
                "Saving images is not implemented on iOS",
                "OK"
            );
        }
    }
}