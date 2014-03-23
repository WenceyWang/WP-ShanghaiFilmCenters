using MVVMSidekick.Views;
using ShanghaiFilmCenters.WP8.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShanghaiFilmCenters.WP8.Startups
{
    public static partial class StartupFunctions
    {
        public static void ConfigAbout()
        {
            ViewModelLocator<About_Model>
                .Instance
                .Register(new About_Model())
                .GetViewMapper()
                .MapToDefault<About>();
        }
    }
}
