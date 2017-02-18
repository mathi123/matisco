using System.Windows;
using Autofac;
using Matisco.Wpf.Services;

namespace Example.BusinessApp
{
    public partial class App
    {
        private void AppStartup(object sender, StartupEventArgs e)
        {
            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();

            var windowSerivce = bootstrapper.Container.Resolve<IWindowService>();
            windowSerivce.Open(nameof(Infrastructure.Views.StartUpView));
        }
    }
}
