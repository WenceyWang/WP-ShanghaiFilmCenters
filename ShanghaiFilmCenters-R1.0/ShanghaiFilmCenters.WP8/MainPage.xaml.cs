using System;
using MVVMSidekick.Views;
using ShanghaiFilmCenters.WP8.ViewModels;

namespace ShanghaiFilmCenters.WP8
{
    public partial class MainPage : MVVMPage
    {
        public MainPage()
            : base(null)
        {
            InitializeComponent();
        }

        public MainPage(MainPage_Model model)
            : base(model)
        {
            InitializeComponent();
        }

        private void AboutBarIconButton_OnClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }

        private void MenuFilmCenterLocation_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/FilmCenterLocations.xaml", UriKind.Relative));
        }
    }
}
