using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShanghaiFilmCenters.WP8
{
    public class FilmCenter
    {
        public FilmCenter(string displayName, string code)
        {
            this.DisplayName = displayName;
            this.Code = code;
        }

        public string Code { get; set; }

        public string DisplayName { get; set; }

        public string DetailAddress { get; set; }

        public string Phone { get; set; }
    }
}
