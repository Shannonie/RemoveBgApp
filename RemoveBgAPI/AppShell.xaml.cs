using RemoveBgAPI.Views;

namespace RemoveBgAPI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(SavePage), typeof(SavePage));
        }
    }
}
