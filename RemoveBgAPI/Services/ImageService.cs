using RemoveBgAPI.Models;
using System.Collections.ObjectModel;

namespace RemoveBgAPI.Services
{
    public class ImageService
    {
        public ObservableCollection<ImageItem> ImageItems { get; } = [];
    }
}
