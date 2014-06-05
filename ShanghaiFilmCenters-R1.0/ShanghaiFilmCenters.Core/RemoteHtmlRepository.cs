using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShanghaiFilmCenters.WP8;

namespace ShanghaiFilmCenters.Core
{
    public class RemoteHtmlRepository : IRepository
    {
        public IEnumerable<FilmCenter> GetFilmCenters()
        {
            return new List<FilmCenter>()
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
        }
    }
}
