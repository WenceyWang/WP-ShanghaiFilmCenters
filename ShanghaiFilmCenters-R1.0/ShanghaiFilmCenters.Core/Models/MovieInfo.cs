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

    public class MovieInfo
    {
        //[Display(Name = "放映影院")]
        public string FilmCenterName { get; set; }

        //[Display(Name = "影片名")]
        public string Title { get; set; }

        //[Display(Name = "语言")]
        public string Language { get; set; }

        //[Display(Name = "开映时间")]
        public DateTime? StartTime { get; set; }

        //[Display(Name = "放映日期")]
        public DateTime? Date { get; set; }

        //[Display(Name = "票价")]
        public string Price { get; set; }

        //[Display(Name = "放映厅")]
        public string Hall { get; set; }

        public MovieInfo()
        {

        }

        public string GetSMSContent()
        {
            return string.Format("影片《{0}》{6}地点：{1}({2}){6}日期：{3}{6}时间：{4}{6}票价：{5}",
                Title,
                FilmCenterName,
                Hall,
                Date.GetValueOrDefault().ToString("yyyy-M-d dddd"),
                StartTime.GetValueOrDefault().ToString("HH:mm"),
                Price,
                Environment.NewLine
                );
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
