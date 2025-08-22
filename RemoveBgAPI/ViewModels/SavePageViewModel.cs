using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RemoveBgAPI.Models;
using RemoveBgAPI.Services;
using RemoveBgAPI.Utils;

namespace RemoveBgAPI.ViewModels
{
    public partial class SavePageViewModel : ObservableObject
    {
        private ImageService imgService;
        private readonly IFileSaver fileSaver;

        public SavePageViewModel(ImageService imageService, IFileSaver saver)
        {
            imgService = imageService;
            fileSaver = saver;

            ResultImgItems =  new ObservableCollection<ImageItem>
                (imgService.ImageItems.Where(item => item.ResultSource != null)!);

            foreach (ImageItem item in ResultImgItems)
            {
                item.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(ImageItem.IsSelected))
                        OnPropertyChanged(nameof(IsSelectedButtonEnable));
                };
            }
            ResultImgItems.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(IsSelectedButtonEnable));
            };
        }

        public bool IsSelectedButtonEnable => ResultImgItems.Any(item => item.IsSelected);

        [ObservableProperty]
        private ObservableCollection<ImageItem> _resultImgItems;

        [RelayCommand]
        async Task SaveSelectedImageAsync()
        {
            try
            {
                List<ImageItem> selectedItem = ResultImgItems.Where(item => item.IsSelected).ToList();

                if (!selectedItem.Any())
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Notice",
                        "None image selected!",
                        "OK");
                }

                await SaveImagesAndGoBack(selectedItem);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task SaveAllImageAsync()
        {
            try
            {
                List<ImageItem> all = ResultImgItems.Where(item => item.resultBytes != null).ToList();
                await SaveImagesAndGoBack(all);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task SaveImagesAndGoBack(List<ImageItem> items)
        {
            try
            {
                foreach (ImageItem item in items)
                {
                    item.IsSelected = false;
                    string saveFileName = $"removedBG_{item.FileName}.png";
                    await fileSaver.SaveFileAsync(item.resultBytes, saveFileName);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Images saved!", "OK");
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}
