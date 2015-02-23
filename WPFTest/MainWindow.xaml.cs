using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Lind.WPFTest.ViewModels;
using Lind.WPFTest.Data;
using Microsoft.Practices.Unity;

namespace WPFTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var container = new UnityContainer();
            container.RegisterType<NavigationItem, CustomersItem>("Customers", new InjectionConstructor(typeof(NavigationData), new NorthwindRepository<Customer>()));
            container.RegisterType<NavigationItem, ProductsItem>("Products", new InjectionConstructor(typeof(NavigationData), new ProductRepository()));
            container.RegisterType<NavigationItem, EmployeesItem>("Employees", new InjectionConstructor(typeof(NavigationData), new NorthwindRepository<Employee>()));
            container.RegisterType<NavigationItem, SuppliersItem>("Suppliers", new InjectionConstructor(typeof(NavigationData), new NorthwindRepository<Supplier>()));
            container.RegisterType<NavigationItem, OrdersItem>("Orders", new InjectionConstructor(typeof(NavigationData), new OrderRepository()));

            InitializeComponent();
            this.DataContext = new MainWindowViewModel(container, new NavigationData[] { new NavigationData("Customers"), new NavigationData("Employees"), new NavigationData("Suppliers"), new NavigationData("Products"), new NavigationData("Orders") });
        }
    }
}
