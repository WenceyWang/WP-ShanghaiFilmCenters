using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ShanghaiFilmCenters.Service
{
    public class FilmCenterService : IFilmCenterService
    {
        private const string URLTemplate = @"http://211.152.35.35/SH_SHO.cfm?loc={0}&film={1}&sdate={2}";

        public Response<List<MovieInfo>> GetMovies(string filmCenterCode, DateTime selectedDate)
        {
            // create url
            var url = BuildUrl(filmCenterCode, string.Empty, selectedDate);

            var client = new WebClient()
            {
                //Encoding = Encoding.Unicode //DBCSEncoding.GetDBCSEncoding("gb2312")
            };

            var data = client.DownloadString(url);

            return ParseHtml(data);
        }

        public Response<List<FilmCenter>> GetAllFilmCenters()
        {
            var list = new List<FilmCenter>()
            {
                new FilmCenter("上海影城", "801")
                {
                    DetailAddress = "上海市长宁区新华路160号（番禺路口）",
                    Phone = "62806088"
                },
                new FilmCenter("新世纪影城", "802")
                {
                    DetailAddress = "上海市浦东新区张杨路501号（第一八佰伴商场10楼）",
                    Phone = "58362988"
                },
                new FilmCenter("西南影城", "803")
                {
                    DetailAddress = "上海市徐汇区（长桥）罗香路237号（龙临路口）",
                    Phone = "54416166"
                },
                new FilmCenter("世博国际影城", "857")
                {
                    DetailAddress = "上海市浦东新区世博大道1200号",
                    Phone = "62817017"
                },
                new FilmCenter("上海宝山影城", "962")
                {
                    DetailAddress = "永清路700号(永乐路6号门进)",
                    Phone = "62806088"
                },
            };

            return new Response<List<FilmCenter>>()
            {
                IsSuccess = true,
                Item = list
            };
        }

        private static string BuildUrl(string centerCode, string movieCode, DateTime showDate)
        {
            return string.Format(URLTemplate, centerCode, string.Empty, showDate.ToString("yyyy-MM-dd"));
        }

        private Response<List<MovieInfo>> ParseHtml(string html)
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

                    if (dataNodes.Count == 1 && dataNodes[0][0] == "查无符合条件的放映场次")
                    {
                        return new Response<List<MovieInfo>>()
                        {
                            IsSuccess = false,
                            Message = "查无符合条件的放映场次，官方网站一般只更新未来3-5天的排片表"
                        };
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
                        }).ToList();

                        return new Response<List<MovieInfo>>()
                        {
                            IsSuccess = true,
                            Item = list
                        };
                    }
                }
                return new Response<List<MovieInfo>>()
                {
                    IsSuccess = false,
                    Message = "数据源解析失败，可能是官网改版了。"
                };
            }
            catch (Exception ex)
            {
                return new Response<List<MovieInfo>>()
                {
                    IsSuccess = false, 
                    Message = ex.Message,
                    Item = null
                };
            }
        }
    }
}
