using CommunityToolkit.Mvvm.ComponentModel;

namespace RemoveBgAPI.Models
{
    public partial class ImageItem : ObservableObject
    {
        [ObservableProperty] private string? fileName;
        [ObservableProperty] private ImageSource? originSource;
        [ObservableProperty] private ImageSource? resultSource;
        [ObservableProperty] private bool isSelected;

        public byte[]? resultBytes { get; set; }
    }
}
