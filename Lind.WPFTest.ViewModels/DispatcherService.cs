using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lind.WPFTest.ViewModels
{
    public interface IDispatcher
    {
        void Invoke(Action action);
    }
    public static class DispatcherLocator
    {
        public static IDispatcher Dispatcher { get; set; }
    }
}
