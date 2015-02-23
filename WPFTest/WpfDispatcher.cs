using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Threading;
using Lind.WPFTest.ViewModels;

namespace Lind.WPFTest
{
    public class WpfDispatcher : IDispatcher
    {
        public void Invoke(Action action)
        {
            Dispatcher dispatcher = Application.Current.Dispatcher;
            if (dispatcher == null || dispatcher.CheckAccess())
                action();
            else
                dispatcher.Invoke(action);
        }
    }
}
