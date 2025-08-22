namespace RemoveBgAPI.Utils
{
    public interface IFileSaver
    {
        Task SaveFileAsync(byte[] bytes, string filename);
    }
}
