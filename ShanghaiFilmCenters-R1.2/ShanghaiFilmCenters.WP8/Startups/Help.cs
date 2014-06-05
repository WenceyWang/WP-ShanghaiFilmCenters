using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVVMSidekick.Views;
using ShanghaiFilmCenters.WP8.ViewModels;

namespace ShanghaiFilmCenters.WP8.Startups
{
    public static partial class StartupFunctions
    {
        public static void ConfigHelp()
        {
            ViewModelLocator<Help_Model>
                .Instance
                .Register(new Help_Model())
                .GetViewMapper()
                .MapToDefault<Help>();
        }
    }
}
