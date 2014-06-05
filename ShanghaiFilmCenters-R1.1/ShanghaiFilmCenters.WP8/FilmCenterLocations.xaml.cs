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
using ShanghaiFilmCenters.WP8.Models;
using ShanghaiFilmCenters.WP8.ViewModels;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace ShanghaiFilmCenters.WP8
{
    public partial class FilmCenterLocations : MVVMPage
    {
        public FilmCenterLocations()
        {
            InitializeComponent();
        }

        public FilmCenterLocations(FilmCenterLocations_Model model)
            : base(model)
        {
            InitializeComponent();
        }

        private void AboutBarIconButton_OnClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }

        private void MenuSendToSMS_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                var selected = menuItem.DataContext as FilmCenter;
                if (null != selected)
                {
                    var smsComposeTask = new SmsComposeTask
                    {
                        Body = string.Format("{0}{3}{1}{3}{2}", selected.DisplayName, selected.DetailAddress, selected.Phone, Environment.NewLine),
                    };

                    smsComposeTask.Show();
                }
            }
        }

        private void MenuSendToEmail_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                var selected = menuItem.DataContext as FilmCenter;
                if (null != selected)
                {
                    var emailComposeTask = new EmailComposeTask
                    {
                        Subject = selected.DisplayName,
                        Body = string.Format("{0}{1}{2}", selected.DetailAddress, Environment.NewLine, selected.Phone),
                    };

                    emailComposeTask.Show();
                }
            }
        }

        private void ImgPhone_OnTap(object sender, GestureEventArgs e)
        {
            var selectedFilmCenter = ((FrameworkElement)(sender)).DataContext as FilmCenter;
            if (null != selectedFilmCenter)
            {
                var phoneCallTask = new PhoneCallTask
                {
                    PhoneNumber = selectedFilmCenter.Phone,
                    DisplayName = selectedFilmCenter.DisplayName
                };

                phoneCallTask.Show();
            }
        }
    }
}