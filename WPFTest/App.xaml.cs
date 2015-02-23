using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Lind.WPFTest.ViewModels;
using Lind.WPFTest;

namespace WPFTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            DispatcherLocator.Dispatcher = new WpfDispatcher();
        }
    }
}
