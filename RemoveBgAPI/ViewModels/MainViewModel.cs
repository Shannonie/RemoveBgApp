using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RemoveBgAPI.Models;
using RemoveBgAPI.Services;
using RemoveBgAPI.Views;

namespace RemoveBgAPI.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public ObservableCollection<ImageItem> SelectedImages => imgService.ImageItems;

        private readonly RemoveBgService removeBgService;
        private ImageService imgService;
        private List<byte[]> imgBytes;

        public MainViewModel(RemoveBgService removeBackgroundService, ImageService imageService)
        {
            removeBgService = removeBackgroundService;
            imgService = imageService;
            imgBytes = [];
        }

        [ObservableProperty]
        private bool _isBusy;
        [ObservableProperty]
        private bool _isImgLabelsVisible;
        [ObservableProperty]
        private bool _isImgVisible;
        [ObservableProperty]
        private bool _isSaveButtonVisible;

        [RelayCommand]
        async Task LoadPhotosAndStartProcess()
        {
            try
            {
                IsImgLabelsVisible = IsImgVisible = IsSaveButtonVisible = false;
                SelectedImages.Clear();
                imgBytes.Clear();

                IEnumerable<FileResult>? results =
                    await FilePicker.Default.PickMultipleAsync(new PickOptions
                    {
                        PickerTitle = "Select image(s)",
                        FileTypes = FilePickerFileType.Images
                    });

                if (results == null || results.Count() == 0)
                    return;

                IsBusy = true;

                foreach (FileResult file in results)
                {
                    byte[] bytes = await CreateImageBytesAsync(file);
                    imgBytes.Add(bytes);
                    SelectedImages.Add(new ImageItem
                    {
                        FileName = file.FileName,
                        OriginSource = ImageSource.FromStream(() => new MemoryStream(bytes))
                    });
                }

                IsImgLabelsVisible = IsImgVisible = SelectedImages.Count > 0;

                await RemoveBgAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LoadPhotosAndStartProcess threw: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert(
                    "Notice",
                    ex.Message,
                    "OK");
            }
            finally
            {
                IsBusy = false;
                IsSaveButtonVisible = SelectedImages.Any(img => img.ResultSource != null);
            }
        }

        [RelayCommand]
        async Task SavePhotos()
        {
            try
            {
                await Shell.Current.GoToAsync(nameof(SavePage));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private static async Task<byte[]> CreateImageBytesAsync(FileResult file)
        {
            using Stream stream = await file.OpenReadAsync();
            using MemoryStream ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            byte[] bytes = ms.ToArray();

            return bytes;
        }

        private async Task RemoveBgAsync()
        {
            for (int id = 0; id < imgBytes.Count; id++)
            {
                Tuple<string, byte[]> result = await removeBgService.RemoveBackgroundAsync(
                    SelectedImages[id].FileName, imgBytes[id]);

                if (result.Item2 == null)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Notice",
                        "Something went wrong while processing the image.",
                        "OK");
                    return;
                }

                ImageItem? imgItem = SelectedImages.FirstOrDefault(
                    img => img.FileName.Equals(result.Item1, StringComparison.OrdinalIgnoreCase));
                imgItem.resultBytes = result.Item2;
                imgItem.ResultSource = ImageSource.FromStream(() => new MemoryStream(imgItem.resultBytes));
            }
        }
    }
}
