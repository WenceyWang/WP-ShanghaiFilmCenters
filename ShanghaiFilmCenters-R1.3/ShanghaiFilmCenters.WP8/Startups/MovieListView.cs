using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
using ShanghaiFilmCenters.WP8.ViewModels;
using System;
using System.Net;
using System.Windows;


namespace ShanghaiFilmCenters.WP8.Startups
{
    public static partial class StartupFunctions
    {
        public static void ConfigMovieListView()
        {
            ViewModelLocator<MovieListView_Model>
                .Instance
                .Register(new MovieListView_Model())
                .GetViewMapper()
                .MapToDefault<MovieListView>();
        }
    }
}
