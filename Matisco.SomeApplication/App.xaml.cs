using System;
using System.Windows;
using Autofac;
using Matisco.SomeApplication.CustomerManagement;
using Matisco.Wpf;
using Prism.Regions;

namespace Matisco.SomeApplication
{
    public partial class App
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var bootstrapper = new MatiscoBootstrapper(RegisterTypes, RegisterViews);
            var windowManager = bootstrapper.Run();

            windowManager.Open(nameof(StartUpView));
        }

        private void RegisterTypes(ContainerBuilder builder)
        {
            builder.RegisterType<CustomerService>().As<ICustomerService>().SingleInstance();
        }

        private void RegisterViews(IRegionManager regionManager)
        {
            regionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(StartUpView));
            regionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(CustomerOverview));
            regionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(CustomerView));
            regionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(ConfirmView));
        }
    }
}
