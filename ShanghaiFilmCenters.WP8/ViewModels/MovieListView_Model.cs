using System.Collections.Generic;
using MVVMSidekick.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using ShanghaiFilmCenters.WP8.FilmCenterService;
using ShanghaiFilmCenters.WP8.Helpers;
using ShanghaiFilmCenters.WP8.Models;
using ShanghaiFilmCenters.WP8.Resources;
using MovieInfo = ShanghaiFilmCenters.WP8.FilmCenterService.MovieInfo;

namespace ShanghaiFilmCenters.WP8.ViewModels
{
    public class MovieListView_Model : ViewModelBase<MovieListView_Model>
    {
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

        private void SearchMovie()
        {
            Busy = true;
            Message = AppResources.Searching;

            try
            {
                var client = new FilmCenterServiceClient();
                client.GetMoviesCompleted += (o, e) =>
                {
                    Busy = false;

                    if (null != e.Result)
                    {
                        if (e.Result.IsSuccess)
                        {
                            QueryResult = e.Result.Item.ToObservableCollection();
                        }
                        else
                        {
                            MessageBox.Show(e.Result.Message, "", MessageBoxButton.OK);
                        }
                    }
                    else
                    {
                        MessageBox.Show(e.Error.Message, AppResources.BlowUpTitle, MessageBoxButton.OK);
                    }
                };

                client.GetMoviesAsync(SelectedFilmCenter == null ? string.Empty : SelectedFilmCenter.Code, SelectedDate);
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