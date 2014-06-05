using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using MVVMSidekick.Views;
using ShanghaiFilmCenters.WP8.ViewModels;

namespace ShanghaiFilmCenters.WP8
{
    public partial class About : MVVMPage
    {
        public About()
        {
            InitializeComponent();
        }

        public About(About_Model model)
            : base(model)
        {
            InitializeComponent();
        }

        private void Help_OnClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Help.xaml", UriKind.Relative));
        }

        private void MarketReview_Click(object sender, EventArgs e)
        {
            var rev = new MarketplaceReviewTask();
            rev.Show();
        }
    }
}