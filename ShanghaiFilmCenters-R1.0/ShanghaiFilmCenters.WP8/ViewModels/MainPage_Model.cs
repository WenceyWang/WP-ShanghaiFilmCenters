using System.Linq;
using System.Windows;
using Microsoft.Phone.Tasks;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Reactive;
using System;
using System.Collections.Generic;
using Microsoft.Phone.Net.NetworkInformation;
using ShanghaiFilmCenters.Core;
using ShanghaiFilmCenters.WP8.Resources;

namespace ShanghaiFilmCenters.WP8.ViewModels
{
    public class MainPage_Model : ViewModelBase<MainPage_Model>
    {
        private readonly IRepository _repository = new RemoteHtmlRepository();

        public MainPage_Model()
        {
            var list = _repository.GetFilmCenters().ToList();
            list.Insert(0, new FilmCenter(AppResources.AllFilmCenters, string.Empty));

            var filmCenters = list;

            this.FilmCenters = filmCenters;
            this.SelectedDate = DateTime.Now.Date;

            CheckNetwork();
        }

        private void CheckNetwork()
        {
            IsSearchButtonEnabled = true;
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                IsSearchButtonEnabled = false;

                var result = MessageBox.Show(
                    NetworkInterface.NetworkInterfaceType + AppResources.NetworkBlowUp, 
                    AppResources.NoConnection, 
                    MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.OK)
                {
                    var connectionSettingsTask = new ConnectionSettingsTask
                    {
                        ConnectionSettingsType = ConnectionSettingsType.WiFi
                    };
                    connectionSettingsTask.Show();
                }
            }
        }

        public bool IsSearchButtonEnabled
        {
            get { return _IsSearchButtonEnabledLocator(this).Value; }
            set { _IsSearchButtonEnabledLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property bool IsSearchButtonEnabled Setup
        protected Property<bool> _IsSearchButtonEnabled = new Property<bool> { LocatorFunc = _IsSearchButtonEnabledLocator };
        static Func<BindableBase, ValueContainer<bool>> _IsSearchButtonEnabledLocator = RegisterContainerLocator<bool>("IsSearchButtonEnabled", model => model.Initialize("IsSearchButtonEnabled", ref model._IsSearchButtonEnabled, ref _IsSearchButtonEnabledLocator, _IsSearchButtonEnabledDefaultValueFactory));
        static Func<bool> _IsSearchButtonEnabledDefaultValueFactory = () => { return default(bool); };
        #endregion


        public CommandModel<ReactiveCommand, String> CommandSearchMovie
        {
            get { return _CommandSearchMovieLocator(this).Value; }
            set { _CommandSearchMovieLocator(this).SetValueAndTryNotify(value); }
        }

        #region Property CommandModel<ReactiveCommand, String> CommandSearchMovie Setup

        protected Property<CommandModel<ReactiveCommand, String>> _CommandSearchMovie = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandSearchMovieLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandSearchMovieLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>("CommandSearchMovie", model => model.Initialize("CommandSearchMovie", ref model._CommandSearchMovie, ref _CommandSearchMovieLocator, _CommandSearchMovieDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandSearchMovieDefaultValueFactory =
            model =>
            {
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model };
                var vm = CastToCurrentType(model);
                cmd.Subscribe(async _ =>
                    {
                        var targetvm = new MovieListView_Model();
                        targetvm.InitData(vm.SelectedDate, vm.SelectedFilmCenter);
                        await vm.StageManager.DefaultStage.Show(targetvm);
                    }).DisposeWith(model);
                return cmd.CreateCommandModel("SearchMovie");
            };

        #endregion

        public FilmCenter SelectedFilmCenter
        {
            get { return _SelectedFilmCenterLocator(this).Value; }
            set { _SelectedFilmCenterLocator(this).SetValueAndTryNotify(value); }
        }

        #region Property FilmCenter SelectedFilmCenter Setup
        protected Property<FilmCenter> _SelectedFilmCenter = new Property<FilmCenter> { LocatorFunc = _SelectedFilmCenterLocator };
        static Func<BindableBase, ValueContainer<FilmCenter>> _SelectedFilmCenterLocator = RegisterContainerLocator<FilmCenter>("SelectedFilmCenter", model => model.Initialize("SelectedFilmCenter", ref model._SelectedFilmCenter, ref _SelectedFilmCenterLocator, _SelectedFilmCenterDefaultValueFactory));
        static Func<FilmCenter> _SelectedFilmCenterDefaultValueFactory = null;
        #endregion

        public DateTime SelectedDate
        {
            get { return _SelectedDateLocator(this).Value; }
            set { _SelectedDateLocator(this).SetValueAndTryNotify(value); }
        }

        #region Property DateTime SelectedDate Setup

        protected Property<DateTime> _SelectedDate = new Property<DateTime> { LocatorFunc = _SelectedDateLocator };
        static Func<BindableBase, ValueContainer<DateTime>> _SelectedDateLocator = RegisterContainerLocator<DateTime>("SelectedDate", model => model.Initialize("SelectedDate", ref model._SelectedDate, ref _SelectedDateLocator, _SelectedDateDefaultValueFactory));
        static Func<DateTime> _SelectedDateDefaultValueFactory = null;

        #endregion

        public IEnumerable<FilmCenter> FilmCenters
        {
            get { return _FilmCentersLocator(this).Value; }
            set { _FilmCentersLocator(this).SetValueAndTryNotify(value); }
        }

        #region Property IEnumerable<FilmCenter> FilmCenters Setup

        protected Property<IEnumerable<FilmCenter>> _FilmCenters = new Property<IEnumerable<FilmCenter>> { LocatorFunc = _FilmCentersLocator };
        static Func<BindableBase, ValueContainer<IEnumerable<FilmCenter>>> _FilmCentersLocator = RegisterContainerLocator<IEnumerable<FilmCenter>>("FilmCenters", model => model.Initialize("FilmCenters", ref model._FilmCenters, ref _FilmCentersLocator, _FilmCentersDefaultValueFactory));
        static Func<IEnumerable<FilmCenter>> _FilmCentersDefaultValueFactory = null;

        #endregion
    }
}

