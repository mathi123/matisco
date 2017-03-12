using System.Windows;
using Autofac;
using Matisco.Wpf.Services;
using Prism.Regions;

namespace Example.BusinessApp
{
    public partial class App
    {
        private void AppStartup(object sender, StartupEventArgs e)
        {
            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();

            var windowSerivce = bootstrapper.Container.Resolve<IWindowService>();

            var navigationParameter = new NavigationParameters
                {
                    { "Id", 1 }
                };

            windowSerivce.Open("UserView", navigationParameter);
        }
    }
}
