namespace RemoveBgAPI.Utils
{
    public class FileSaver : IFileSaver
    {
        public async Task SaveFileAsync(byte[] bytes, string filename)
        {
            Java.IO.File dir = Android.App.Application.Context.GetExternalFilesDir(
                Android.OS.Environment.DirectoryPictures);
            string path = Path.Combine(dir!.AbsolutePath, filename);

            await File.WriteAllBytesAsync(path, bytes);

            // So Gallery apps (Google Photos, etc.) see it
            Java.IO.File javaFile = new Java.IO.File(path);
            Android.Net.Uri? uri = Android.Net.Uri.FromFile(javaFile);
            Android.App.Application.Context.SendBroadcast(
                new Android.Content.Intent(Android.Content.Intent.ActionMediaScannerScanFile, uri)
            );
        }
    }
}