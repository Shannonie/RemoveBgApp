using RemoveBgAPI.Utils;

public class FileSaverService
{
    private readonly IFileSaver _fileSaver;

    /*public FileSaverService()
    {
        _fileSaver = FileSaver.Default;
    }

    public async Task<string?> SaveFileAsync(byte[] bytes, string filename)
    {
        using MemoryStream stream = new MemoryStream(bytes);
        var result = await _fileSaver.SaveAsync(filename, stream, CancellationToken.None);

        if (result.IsSuccessful)
            return result.FilePath;  // Path where file is saved
        else
            throw result.Exception ?? new Exception("Failed to save file");
    }*/
}
