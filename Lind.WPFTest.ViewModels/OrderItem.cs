using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lind.WPFTest.Data;
using System.Threading;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace Lind.WPFTest.ViewModels
{
    public class OrderItem : NavigationItem<Order_Detail>
    {
        private Order order;
        public Order Order
        {
            get { return order; }
            private set { SetProperty(ref order, value); }
        }
        public int OrderID { get; private set; }
        protected IRepository<Order> Repository { get; private set; }
        public OrderItem(NavigationData data, IRepository<Order> repository, int orderID) : base(data)
        {
            OrderID = orderID;
            Repository = repository;
        }
        protected override async Task DoLoad(CancellationToken token)
        {
            Order = await Repository.GetEntityAsync(OrderID, token);
            await base.DoLoad(token);
        }
        protected override async Task DoUnload()
        {
            await base.DoUnload();            
            Order = null;
        }
        protected override Task<IEnumerable<Order_Detail>> GetItems(CancellationToken token)
        {
            if (Order == null)
                return Task.FromResult<IEnumerable<Order_Detail>>(null);
            return Task.FromResult(Order.Order_Details.AsEnumerable());
        }
    }
    public class OrdersItem : RepositoryNavigationItem<Order>
    {
        public ICommand OpenOrder { get; private set; }
        public OrdersItem(NavigationData data, IRepository<Order> repository)
            : base(data, repository)
        {
            OpenOrder = new DelegateCommand<Order>(DoOpenOrder);
        }
        private void DoOpenOrder(Order order)
        {   
            NavigationData orderData = new NavigationData("OrderItem", string.Format("Order# {0}", order.OrderID), true);
            OrderItem item = new OrderItem(orderData, Repository, order.OrderID);
            AddNavigationItem(item);
        }
    }
}
