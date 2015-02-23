using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Commands;
using System.Collections.Concurrent;
using System.Threading;
using Lind.WPFTest.Data;
using Microsoft.Practices.Unity;
using System.Runtime.CompilerServices;

namespace Lind.WPFTest.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        public ObservableCollection<NavigationItem> NavigationItems { get; private set; }
        private NavigationItem selectedNavigationItem;
        public NavigationItem SelectedNavigationItem
        {
            get 
            {
                return selectedNavigationItem;
            }
            set
            {
                SetNavigationItem(ref selectedNavigationItem, value);
            }
        }
        public MainWindowViewModel(IUnityContainer container, IEnumerable<NavigationData> navigationData) : base()
        {
            
            NavigationItems = new ObservableCollection<NavigationItem>();
            foreach (var data in navigationData)
            {
                var item = container.Resolve<NavigationItem>(data.Name, new ParameterOverride("data", data));
                if (item != null)
                {
                    item.Closed += NavigationItem_Closed;
                    item.NavigationItemAdded += NavigationItemAdded;
                    NavigationItems.Add(item);
                }
            }
            SelectedNavigationItem = NavigationItems.First();

        }

        private void NavigationItemAdded(object sender, NavigationItemAddedEventArgs e)
        {
            e.NewItem.Closed += NavigationItem_Closed;
            e.NewItem.NavigationItemAdded += NavigationItemAdded;
            DispatcherLocator.Dispatcher.Invoke(() => NavigationItems.Add(e.NewItem));
            SelectedNavigationItem = e.NewItem;
        }

        private void NavigationItem_Closed(object sender, EventArgs e)
        {
            var item = sender as NavigationItem;
            if (item != null)
            {
                if (SelectedNavigationItem == item)
                    SelectedNavigationItem = NavigationItems.FirstOrDefault();
                DispatcherLocator.Dispatcher.Invoke(() => NavigationItems.Remove(item));
            }
        }
    }

    public abstract class ViewModel : BindableBase
    {
        protected bool SetNavigationItem(ref NavigationItem storage, NavigationItem value, [CallerMemberName] string propertyName = null)
        {
            if (storage != value)
            {
                if (storage != null)
                {
                    Task t = storage.Unload();
                }
                SetProperty(ref storage, value, propertyName);
                if (value != null)
                {
                    Task t = value.Load();
                }
                return true;
            }
            return false;
        }
        public ViewModel()
        {
            if (DispatcherLocator.Dispatcher == null)
                throw new ArgumentException("The dispatcher must be set before using this class");
        }
    }
    
    
}
