using RemoveBgAPI.ViewModels;

namespace RemoveBgAPI.Views;

public partial class SavePage : ContentPage
{
	public SavePage(SavePageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}