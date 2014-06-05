using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ShanghaiFilmCenters.WP8.Models
{
    public class MovieQuery
    {
        public FilmCenter SelectedFilmCenter { get; set; }
        public DateTime SelectedDate { get; set; }

        public MovieQuery()
        {
            SelectedDate = DateTime.Today;
        }
    }

    public static class CollectionExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> coll)
        {
            var c = new ObservableCollection<T>();
            foreach (var e in coll)
            {
                c.Add(e);
            }
            return c;
        }
    }
}
