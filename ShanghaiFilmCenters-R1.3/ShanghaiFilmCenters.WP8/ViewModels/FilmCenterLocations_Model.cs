using System.Collections.ObjectModel;
using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShanghaiFilmCenters.Core;
using ShanghaiFilmCenters.WP8.Models;

namespace ShanghaiFilmCenters.WP8.ViewModels
{
    public class FilmCenterLocations_Model : ViewModelBase<FilmCenterLocations_Model>
    {
        private readonly IRepository _repository = new RemoteHtmlRepository();

        public FilmCenterLocations_Model()
        {
            FilmCenterList = _repository.GetFilmCenters().ToObservableCollection();
        }

        public ObservableCollection<FilmCenter> FilmCenterList
        {
            get { return _FilmCenterListLocator(this).Value; }
            set { _FilmCenterListLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ObservableCollection<FilmCenter> FilmCenterList Setup
        protected Property<ObservableCollection<FilmCenter>> _FilmCenterList = new Property<ObservableCollection<FilmCenter>> { LocatorFunc = _FilmCenterListLocator };
        static Func<BindableBase, ValueContainer<ObservableCollection<FilmCenter>>> _FilmCenterListLocator = RegisterContainerLocator<ObservableCollection<FilmCenter>>("FilmCenterList", model => model.Initialize("FilmCenterList", ref model._FilmCenterList, ref _FilmCenterListLocator, _FilmCenterListDefaultValueFactory));
        static Func<ObservableCollection<FilmCenter>> _FilmCenterListDefaultValueFactory = () => { return default(ObservableCollection<FilmCenter>); };
        #endregion

        public void NavigateToMapView(FilmCenter filmCenter)
        {
            
        }
    }
}
