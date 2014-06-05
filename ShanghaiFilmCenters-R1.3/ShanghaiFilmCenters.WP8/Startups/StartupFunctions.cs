﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace ShanghaiFilmCenters.WP8.Startups
{
    public static partial class StartupFunctions
    {

        public static void RunAllConfig()
        {
            typeof(StartupFunctions)
                    .GetMethods()
                    .Where(m => m.Name.StartsWith("Config") && m.IsStatic)
                    .Select(
                        m => m.Invoke(null, Enumerable.Empty<object>().ToArray()))
                    .ToArray();


        }

    }
}
