using Windows.Storage.Pickers;
using Windows.Storage;
using WinRT.Interop;

namespace RemoveBgAPI.Utils
{
    public class FileSaver : IFileSaver
    {
        public async Task SaveFileAsync(byte[] bytes, string filename)
        {
            FileSavePicker picker = new FileSavePicker();
            nint hwnd = WindowNative.GetWindowHandle(Application.Current.Windows[0].Handler.PlatformView);
            InitializeWithWindow.Initialize(picker, hwnd);

            picker.FileTypeChoices.Add("PNG Image", new List<string>() { ".png" });
            picker.SuggestedFileName = filename;

            StorageFile file = await picker.PickSaveFileAsync();
            if (file != null)
            {
                await FileIO.WriteBytesAsync(file, bytes);
            }
        }
    }
}
