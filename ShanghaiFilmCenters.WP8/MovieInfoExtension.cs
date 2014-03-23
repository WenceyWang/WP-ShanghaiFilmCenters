using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShanghaiFilmCenters.WP8.FilmCenterService;

namespace ShanghaiFilmCenters.WP8
{
    public static class MovieInfoExtension
    {
        public static string GetSMSContent(this MovieInfo movieInfo)
        {
            return string.Format("影片《{0}》{6}地点：{1}({2}){6}日期：{3}{6}时间：{4}{6}票价：{5}",
                movieInfo.Title,
                movieInfo.FilmCenterName,
                movieInfo.Hall,
                movieInfo.Date.GetValueOrDefault().ToString("yyyy-M-d dddd"),
                movieInfo.StartTime.GetValueOrDefault().ToString("HH:mm"),
                movieInfo.Price,
                Environment.NewLine
                );
        }

        public static string GetSearchableTitle(this MovieInfo movieInfo)
        {
            return movieInfo.Title.Trim()
                                  .Replace("（数字3D）", string.Empty)
                                  .Replace("（原版）", string.Empty);
        }
    }
}