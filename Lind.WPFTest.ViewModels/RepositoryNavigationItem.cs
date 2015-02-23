using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lind.WPFTest.Data;
using System.Threading;

namespace Lind.WPFTest.ViewModels
{
    public class RepositoryNavigationItem<TEntity> : NavigationItem<TEntity>
        where TEntity : class
    {
        protected IRepository<TEntity> Repository { get; private set; }
        public RepositoryNavigationItem(NavigationData data, IRepository<TEntity> repository)
            : base(data)
        {
            this.Repository = repository;
        }

        protected override Task<IEnumerable<TEntity>> GetItems(CancellationToken token)
        {
            return Repository.GetEntitiesAsync(token);
        }
    }
    public class ProductsItem : RepositoryNavigationItem<Product>
    {
        public ProductsItem(NavigationData data, IRepository<Product> repository) : base(data, repository) { }
    }
    
    public class EmployeesItem : RepositoryNavigationItem<Employee>
    {
        public EmployeesItem(NavigationData data, IRepository<Employee> repository) : base(data, repository) { }
    }
    public class SuppliersItem : RepositoryNavigationItem<Supplier>
    {
        public SuppliersItem(NavigationData data, IRepository<Supplier> repository) : base(data, repository) { }
    }
    public class CustomersItem : RepositoryNavigationItem<Customer>
    {
        public CustomersItem(NavigationData data, IRepository<Customer> repository) : base(data, repository) { }
    }
}
