using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using MVVMSidekick.Views;
using ShanghaiFilmCenters.WP8.FilmCenterService;
using ShanghaiFilmCenters.WP8.Models;
using ShanghaiFilmCenters.WP8.ViewModels;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace ShanghaiFilmCenters.WP8
{
    public partial class MovieListView : MVVMPage
    {
        public MovieListView()
            : base(null)
        {
            this.InitializeComponent();
        }
        public MovieListView(MovieListView_Model model)
            : base(model)
        {
            this.InitializeComponent();
        }

        private void MenuSaveToCalendar_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                var selected = menuItem.DataContext as MovieInfo;
                if (null != selected)
                {
                    var saveAppointmentTask = new SaveAppointmentTask
                    {
                        StartTime = selected.StartTime,
                        //EndTime = null,
                        Subject = selected.Title,
                        Location = string.Format("{0} - {1}", selected.FilmCenterName, selected.Hall),
                        Details = string.Format("票价：{0} 语言：{1}", selected.Price, selected.Language),
                        IsAllDayEvent = false,
                        Reminder = Reminder.FifteenMinutes,
                        AppointmentStatus = Microsoft.Phone.UserData.AppointmentStatus.OutOfOffice
                    };

                    saveAppointmentTask.Show();
                }
            }
        }

        private void MenuSendToSMS_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                var selected = menuItem.DataContext as MovieInfo;
                if (null != selected)
                {
                    var smsComposeTask = new SmsComposeTask
                    {
                        Body = selected.GetSMSContent()
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
                var selected = menuItem.DataContext as MovieInfo;
                if (null != selected)
                {
                    var emailComposeTask = new EmailComposeTask
                    {
                        Subject = string.Format("你的朋友分享给你电影《{0}》", selected.Title),
                        Body = selected.GetSMSContent(),
                    };

                    emailComposeTask.Show();
                }
            }
        }

        private void ButtonRefresh_OnClick(object sender, EventArgs e)
        {
            var movieListViewModel = this.ViewModel as MovieListView_Model;
            if (movieListViewModel != null) movieListViewModel.Refresh();
        }

        private void SearchSelectedMovie(object sender, GestureEventArgs e)
        {
            var longListSelector = sender as LongListSelector;
            if (longListSelector != null)
            {
                var item = longListSelector.SelectedItem as MovieInfo;
                if (null != item)
                {
                    var searchTerm = item.GetSearchableTitle();
                    var searchPage = string.Format(@"http://wap.mtime.com/search/?q={0}", searchTerm);

                    // open browser to search
                    var webBrowserTask = new WebBrowserTask()
                    {
                        Uri = new Uri(searchPage, UriKind.Absolute)
                    };
                    webBrowserTask.Show();
                }
            }
        }
    }
}