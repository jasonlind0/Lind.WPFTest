using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Commands;
using System.Windows.Input;

namespace Lind.WPFTest.ViewModels
{
    public class NavigationData
    {
        public string Name { get; private set; }
        public string DisplayName { get; private set; }
        public bool IsCloseable { get; private set; }
        public NavigationData(string name, string displayName = null, bool isCloseable = false)
        {
            displayName = displayName ?? name;
            Name = name;
            DisplayName = displayName;
            IsCloseable = isCloseable;
        }
    }
    public enum NavigationItemState
    {
        Loading,
        Loaded,
        Unloading,
        Unloaded
    }
    public abstract class NavigationItem : BindableBase
    {
        public NavigationData Data { get; private set; }
        public string DisplayName
        {
            get { return Data.DisplayName; }
        }
        public bool IsCloseable
        {
            get { return Data.IsCloseable; }
        }
        public NavigationItem(NavigationData data)
        {
            Data = data;
            State = NavigationItemState.Unloaded;
            Close = new DelegateCommand(() =>
            {
                Task t = Unload();
                EventHandler closed = Closed;
                if (closed != null)
                    closed(this, EventArgs.Empty);
            });
        }
        private Task LoadWorker { get; set; }
        private Task UnloadWorker { get; set; }
        private NavigationItemState state;
        public NavigationItemState State
        {
            get { return state; }
            set
            {
                if (SetProperty(ref state, value))
                {
                    OnPropertyChanged(() => IsUnloading);
                    OnPropertyChanged(() => IsUnloaded);
                    OnPropertyChanged(() => IsLoading);
                    OnPropertyChanged(() => IsLoaded);
                }
            }
        }
        public bool IsUnloading { get { return State == NavigationItemState.Unloading; } }
        public bool IsUnloaded { get { return State == NavigationItemState.Unloaded; } }
        public bool IsLoading { get { return State == NavigationItemState.Loading; } }
        public bool IsLoaded { get { return State == NavigationItemState.Loaded; } }
        public event EventHandler Unloading;
        public event EventHandler Unloaded;
        public event EventHandler Loading;
        public event EventHandler Loaded;
        public event EventHandler<NavigationItemAddedEventArgs> NavigationItemAdded;
        public ICommand Close { get; private set; }
        public event EventHandler Closed;
        private void RaiseUnloading()
        {
            State = NavigationItemState.Unloading;
            var evt = Unloading;
            if (evt != null)
                evt(this, EventArgs.Empty);
        }
        private void RaiseUnloaded()
        {
            State = NavigationItemState.Unloaded;
            var evt = Unloaded;
            if (evt != null)
                evt(this, EventArgs.Empty);
        }
        private void RaiseLoading()
        {
            State = NavigationItemState.Loading;
            var evt = Loading;
            if (evt != null)
                evt(this, EventArgs.Empty);
        }
        private void RaiseLoaded()
        {
            State = NavigationItemState.Loaded;
            var evt = Loaded;
            if (evt != null)
                evt(this, EventArgs.Empty);
        }
        protected void AddNavigationItem(NavigationItem item)
        {
            var evt = NavigationItemAdded;
            if (evt != null)
                evt(this, new NavigationItemAddedEventArgs(item));
        }
        private async Task WaitLoad()
        {
            if (LoadWorker != null)
            {
                try
                {
                    await LoadWorker;
                }
                catch (OperationCanceledException) { } //task canceled
                catch { Task unloader = Unload(); }
            }
        }
        private async Task WaitUnload()
        {
            if (UnloadWorker != null)
                await UnloadWorker;
        }
        protected abstract Task DoLoad(CancellationToken token);
        public async Task Load()
        {
            await Task.FromResult(false);
            if (State == NavigationItemState.Loading)
                await Unload();
            else if(State == NavigationItemState.Unloading)
                await WaitUnload();
            RaiseLoading();
            LoadCancelationTokenSource = new CancellationTokenSource();
            LoadWorker = DoLoadWorker();
            await WaitLoad();
        }
        private CancellationTokenSource LoadCancelationTokenSource { get; set; }
        private CancellationToken LoadCancelationToken { get { return LoadCancelationTokenSource.Token; } }
        private async Task DoLoadWorker()
        {
            LoadCancelationToken.ThrowIfCancellationRequested();
            await DoLoad(LoadCancelationToken);
            LoadCancelationToken.ThrowIfCancellationRequested();
            RaiseLoaded();
        }
        protected abstract Task DoUnload();
        public async Task Unload()
        {
            await WaitUnload();
            RaiseUnloading();
            UnloadWorker = DoUnloadWorker();
            await WaitUnload();
        }
        private async Task DoUnloadWorker()
        {
            await Task.FromResult(false);
            if (LoadWorker != null)
                LoadCancelationTokenSource.Cancel(true);
            await WaitLoad();
            await DoUnload();
            RaiseUnloaded();
        }
    }
    public abstract class NavigationItem<T> : NavigationItem
    {
        public NavigationItem(NavigationData data)
            : base(data)
        {
            Items = new ObservableCollection<T>();
        }
        public ObservableCollection<T> Items { get; private set; }
        protected abstract Task<IEnumerable<T>> GetItems(CancellationToken token);
        protected override async Task DoLoad(CancellationToken token)
        {
            var items = await GetItems(token);
            token.ThrowIfCancellationRequested();
            if (items != null)
            {
                DispatcherLocator.Dispatcher.Invoke(() =>
                {
                    foreach (var item in items)
                    {
                        Items.Add(item);
                        if (token.IsCancellationRequested)
                            break;
                    }
                });
            }
        }
        protected override Task DoUnload()
        {
            DispatcherLocator.Dispatcher.Invoke(() => Items.Clear());
            return Task.FromResult(false);
        }
    }
    public class NavigationItemAddedEventArgs : EventArgs
    {
        public NavigationItem NewItem { get; private set; }
        public NavigationItemAddedEventArgs(NavigationItem newItem)
        {
            NewItem = newItem;
        }
    }
}
