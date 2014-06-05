using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MVVMSidekick.Views;
using ShanghaiFilmCenters.WP8.ViewModels;

namespace ShanghaiFilmCenters.WP8
{
    public partial class About : MVVMPage
    {
        public About()
        {
            InitializeComponent();
        }

        public About(About_Model model)
            : base(model)
        {
            InitializeComponent();
        }
    }
}