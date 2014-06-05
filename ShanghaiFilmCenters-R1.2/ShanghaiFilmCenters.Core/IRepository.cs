using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShanghaiFilmCenters.WP8;

namespace ShanghaiFilmCenters.Core
{
    public interface IRepository
    {
        IEnumerable<FilmCenter> GetFilmCenters();
    }
}
