using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lind.WPFTest.ViewModels;
using System.Threading;
using System.Threading.Tasks;

namespace Lind.WPFTest.Tests
{
    /// <summary>
    /// Summary description for MainWindowViewModelTests
    /// </summary>
    [TestClass]
    public class MainWindowViewModelTests
    {
        public MainWindowViewModelTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            DispatcherLocator.Dispatcher = new MockDispatcher();
        }
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        //[TestMethod]
        //public void TestRapidLoadUnload()
        //{
        //    //MainWindowViewModel vm = new MainWindowViewModel();
        //    //ManualResetEvent mre = new ManualResetEvent(false);
            
        //    //int carsUnloaded = 0, carsLoaded = 0, widgetsUnloaded = 0, widgetsLoaded = 0;
        //    //Action checkValues = () => { if (carsLoaded == 1 && carsUnloaded == 2 && widgetsUnloaded == 2 && widgetsLoaded == 0)mre.Set(); };
        //    //vm.NavigationItems[0].Loaded += (s, e) => { carsLoaded++; checkValues(); };
        //    //vm.NavigationItems[0].Unloaded += (s, e) => { carsUnloaded++; checkValues(); };
        //    //vm.NavigationItems[1].Loaded += (s, e) => { widgetsLoaded++; checkValues(); };
        //    //vm.NavigationItems[1].Unloaded += (s, e) => { widgetsUnloaded++; checkValues(); };
        //    //vm.SelectedNavigationItem = vm.NavigationItems[1];
        //    //vm.SelectedNavigationItem = vm.NavigationItems[0];
        //    //vm.SelectedNavigationItem = vm.NavigationItems[1];
        //    //vm.SelectedNavigationItem = vm.NavigationItems[0];
        //    //Task mT = Task.Run(() => mre.WaitOne());
        //    //Task d = Task.Delay(10000);
        //    //Task.WaitAny(mT, d);
        //    //Task.Delay(5000).Wait();
        //    //Assert.AreEqual(1, carsLoaded);
        //    //Assert.AreEqual(2, carsUnloaded);
        //    //Assert.AreEqual(2, widgetsUnloaded);
        //    //Assert.AreEqual(0, widgetsLoaded);
        //    //NavigationItem<Car> carsItem = (NavigationItem<Car>)vm.SelectedNavigationItem;
        //    //Assert.AreNotEqual(0, carsItem.Items.Count);
        //    //NavigationItem<Widget> widgetItem = (NavigationItem<Widget>)vm.NavigationItems[1];
        //    //Assert.AreEqual(0, widgetItem.Items.Count);
        //}
    }
    public class MockDispatcher : IDispatcher
    {
        public void Invoke(Action action)
        {
            action();
        }
    }
}
