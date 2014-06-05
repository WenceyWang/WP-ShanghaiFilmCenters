using System.Collections.Generic;
using DBCSCodePage;
using HtmlAgilityPack;
using Microsoft.Phone.Tasks;
using MVVMSidekick.Reactive;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using ShanghaiFilmCenters.WP8.Helpers;
using ShanghaiFilmCenters.WP8.Models;
using ShanghaiFilmCenters.WP8.Resources;

namespace ShanghaiFilmCenters.WP8.ViewModels
{
    public class MovieListView_Model : ViewModelBase<MovieListView_Model>
    {
        private const string URLTemplate = @"http://211.152.35.35/SH_SHO.cfm?loc={0}&film={1}&sdate={2}";

        public MovieListView_Model()
        {
            if (IsInDesignMode)
            {
                SelectedDate = DateTime.Now;
                SelectedFilmCenter = new FilmCenter("软粉影城", "101");
                GroupedQueryResults = new ObservableCollection<Group<MovieInfo>>(new List<Group<MovieInfo>>()
                {
                    new Group<MovieInfo>("微软：人类的希望", new List<MovieInfo>()
                    {
                        new MovieInfo(){ Date = DateTime.Now, FilmCenterName = "软粉影城", Hall = "1厅", Language = "国语", Price = "￥50", StartTime = DateTime.Now, Title = "微软：人类的希望"},
                        new MovieInfo(){ Date = DateTime.Now, FilmCenterName = "软粉影城", Hall = "2厅", Language = "国语", Price = "￥50", StartTime = DateTime.Now.AddHours(1), Title = "微软：人类的希望"},
                        new MovieInfo(){ Date = DateTime.Now, FilmCenterName = "软粉影城", Hall = "3厅", Language = "国语", Price = "￥50", StartTime = DateTime.Now.AddHours(2), Title = "微软：人类的希望"}
                    }),
                    new Group<MovieInfo>("削肾客的救赎", new List<MovieInfo>()
                    {
                        new MovieInfo(){ Date = DateTime.Now, FilmCenterName = "软粉影城", Hall = "1厅", Language = "英语", Price = "￥120", StartTime = DateTime.Now, Title = "削肾客的救赎"},
                        new MovieInfo(){ Date = DateTime.Now, FilmCenterName = "软粉影城", Hall = "2厅", Language = "英语", Price = "￥120", StartTime = DateTime.Now.AddHours(1), Title = "削肾客的救赎"},
                        new MovieInfo(){ Date = DateTime.Now, FilmCenterName = "软粉影城", Hall = "9厅", Language = "英语", Price = "￥40", StartTime = DateTime.Now.AddHours(2), Title = "削肾客的救赎"}
                    })
                });
            }
        }

        public void Refresh()
        {
            this.SearchMovie();
        }

        private async void SearchMovie()
        {
            Busy = true;
            Message = AppResources.Searching;

            // create url
            var url = BuildUrl(SelectedFilmCenter == null ? string.Empty : SelectedFilmCenter.Code, string.Empty, SelectedDate);

            try
            {
                var data = await GetHtmlStringAsync(url);
                Busy = false;

                ParseHtml(data);
            }
            catch (Exception e)
            {
                if (e is TimeoutException)
                {
                    MessageBox.Show("查询超时，请检查网络是否稳定。", AppResources.BlowUpTitle, MessageBoxButton.OK);
                }
                else
                {
                    MessageBox.Show(e.Message, AppResources.BlowUpTitle, MessageBoxButton.OK);
                }
            }
        }

        private static Task<string> GetHtmlStringAsync(string url)
        {
            var tcs = new TaskCompletionSource<string>();

            var client = new WebClient()
            {
                Encoding = DBCSEncoding.GetDBCSEncoding("gb2312") 
            };
            client.DownloadStringCompleted += (s, e) =>
            {
                if (e.Error == null)
                {
                    tcs.SetResult(e.Result);
                }
                else
                {
                    tcs.SetException(e.Error);
                }
            };

            client.DownloadStringAsync(new Uri(url));
            return tcs.Task;
        }

        private void ParseHtml(string html)
        {
            try
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                var movieContext = new HtmlDocument();

                // parse html
                var movieTableNode = doc.DocumentNode.SelectSingleNode("/html[1]/head[1]/body[1]/table[1]/tr[5]/td[1]/table[1]");
                if (null != movieTableNode)
                {
                    movieContext.LoadHtml(movieTableNode.InnerHtml);

                    // Moive table headers
                    var headerNodes = movieContext.DocumentNode.SelectNodes("./tr[3]/td").Select(td => td.InnerText).ToList();

                    // Skip first 3 rows which are not data rows
                    var dataNodes = movieContext.DocumentNode.SelectNodes("./tr").Skip(3)
                                    .Select(n => n.SelectNodes("./td").Select(td => td.InnerText).ToList())
                                    .ToList();

                    if (dataNodes.Count == 1 && dataNodes[0][0] == AppResources.NoMovieFound)
                    {
                        MessageBox.Show(AppResources.NoMovieFound, "", MessageBoxButton.OK);
                    }
                    else
                    {
                        var list = dataNodes.Select(p => new MovieInfo()
                        {
                            FilmCenterName = p[0] ?? string.Empty,
                            Title = p[1] ?? string.Empty,
                            Language = p[2] ?? string.Empty,
                            StartTime = string.IsNullOrEmpty(p[3]) ? new DateTime?() : DateTime.Parse(p[3]),
                            Date = string.IsNullOrEmpty(p[4]) ? new DateTime?() : DateTime.Parse(p[4]),
                            Price = p[5] ?? string.Empty,
                            Hall = p[6] ?? string.Empty
                        });

                        QueryResult = list.ToObservableCollection();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, AppResources.BlowUpTitle, MessageBoxButton.OK);
            }
        }

        private static string BuildUrl(string centerCode, string movieCode, DateTime showDate)
        {
            return string.Format(URLTemplate, centerCode, string.Empty, showDate.ToString("yyyy-MM-dd"));
        }

        public FilmCenter SelectedFilmCenter
        {
            get { return _SelectedFilmCenterLocator(this).Value; }
            set { _SelectedFilmCenterLocator(this).SetValueAndTryNotify(value); }
        }

        #region Property FilmCenter SelectedFilmCenter Setup

        protected Property<FilmCenter> _SelectedFilmCenter = new Property<FilmCenter> { LocatorFunc = _SelectedFilmCenterLocator };
        private static Func<BindableBase, ValueContainer<FilmCenter>> _SelectedFilmCenterLocator = RegisterContainerLocator<FilmCenter>("SelectedFilmCenter", model => model.Initialize("SelectedFilmCenter", ref model._SelectedFilmCenter, ref _SelectedFilmCenterLocator, _SelectedFilmCenterDefaultValueFactory));
        private static Func<FilmCenter> _SelectedFilmCenterDefaultValueFactory = null;

        #endregion Property FilmCenter SelectedFilmCenter Setup

        public DateTime SelectedDate
        {
            get { return _SelectedDateLocator(this).Value; }
            set { _SelectedDateLocator(this).SetValueAndTryNotify(value); }
        }

        #region Property DateTime SelectedDate Setup

        protected Property<DateTime> _SelectedDate = new Property<DateTime> { LocatorFunc = _SelectedDateLocator };
        private static Func<BindableBase, ValueContainer<DateTime>> _SelectedDateLocator = RegisterContainerLocator<DateTime>("SelectedDate", model => model.Initialize("SelectedDate", ref model._SelectedDate, ref _SelectedDateLocator, _SelectedDateDefaultValueFactory));
        private static Func<DateTime> _SelectedDateDefaultValueFactory = null;

        #endregion Property DateTime SelectedDate Setup

        public ObservableCollection<MovieInfo> QueryResult
        {
            get { return _QueryResultLocator(this).Value; }
            set
            {
                _QueryResultLocator(this).SetValueAndTryNotify(value);
                GroupedQueryResults = Group<MovieInfo>.GetItemGroups(value, m => m.Title).ToObservableCollection();
            }
        }

        #region Property ObservableCollection<MovieInfo> QueryResult Setup

        protected Property<ObservableCollection<MovieInfo>> _QueryResult = new Property<ObservableCollection<MovieInfo>> { LocatorFunc = _QueryResultLocator };
        private static Func<BindableBase, ValueContainer<ObservableCollection<MovieInfo>>> _QueryResultLocator = RegisterContainerLocator<ObservableCollection<MovieInfo>>("QueryResult", model => model.Initialize("QueryResult", ref model._QueryResult, ref _QueryResultLocator, _QueryResultDefaultValueFactory));
        private static Func<ObservableCollection<MovieInfo>> _QueryResultDefaultValueFactory = null;

        #endregion Property ObservableCollection<MovieInfo> QueryResult Setup

        public string Message
        {
            get { return _MessageLocator(this).Value; }
            set { _MessageLocator(this).SetValueAndTryNotify(value); }
        }

        #region Property string Message Setup

        protected Property<string> _Message = new Property<string> { LocatorFunc = _MessageLocator };
        private static Func<BindableBase, ValueContainer<string>> _MessageLocator = RegisterContainerLocator<string>("Message", model => model.Initialize("Message", ref model._Message, ref _MessageLocator, _MessageDefaultValueFactory));
        private static Func<string> _MessageDefaultValueFactory = null;

        #endregion Property string Message Setup

        public ObservableCollection<Group<MovieInfo>> GroupedQueryResults
        {
            get { return _GroupedQueryResultsLocator(this).Value; }
            set { _GroupedQueryResultsLocator(this).SetValueAndTryNotify(value); }
        }

        #region Property ObservableCollection<Group<MovieInfo>> GroupedQueryResults Setup

        protected Property<ObservableCollection<Group<MovieInfo>>> _GroupedQueryResults = new Property<ObservableCollection<Group<MovieInfo>>> { LocatorFunc = _GroupedQueryResultsLocator };
        private static Func<BindableBase, ValueContainer<ObservableCollection<Group<MovieInfo>>>> _GroupedQueryResultsLocator = RegisterContainerLocator<ObservableCollection<Group<MovieInfo>>>("GroupedQueryResults", model => model.Initialize("GroupedQueryResults", ref model._GroupedQueryResults, ref _GroupedQueryResultsLocator, _GroupedQueryResultsDefaultValueFactory));
        private static Func<ObservableCollection<Group<MovieInfo>>> _GroupedQueryResultsDefaultValueFactory = null;

        #endregion Property ObservableCollection<Group<MovieInfo>> GroupedQueryResults Setup

        public bool Busy
        {
            get { return _BusyLocator(this).Value; }
            set { _BusyLocator(this).SetValueAndTryNotify(value); }
        }

        #region Property bool Busy Setup

        protected Property<bool> _Busy = new Property<bool> { LocatorFunc = _BusyLocator };
        private static Func<BindableBase, ValueContainer<bool>> _BusyLocator = RegisterContainerLocator<bool>("Busy", model => model.Initialize("Busy", ref model._Busy, ref _BusyLocator, _BusyDefaultValueFactory));
        private static Func<bool> _BusyDefaultValueFactory = null;

        #endregion Property bool Busy Setup

        public void InitData(DateTime selectedDate, FilmCenter selectedFilmCenter)
        {
            SelectedDate = selectedDate;
            SelectedFilmCenter = selectedFilmCenter;

            if (null == selectedFilmCenter)
            {
                SelectedFilmCenter = new FilmCenter(AppResources.AllFilmCenters, string.Empty);
            }

            SearchMovie();
        }
    }
}