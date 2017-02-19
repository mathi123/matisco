using Autofac;
using Matisco.Wpf.Services;

namespace Matisco.Wpf
{
    public class MatiscoAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WindowService>().As<IWindowService>().SingleInstance();
            builder.RegisterType<ApplicationShutdownService>().As<IApplicationShutdownService>().SingleInstance();
            builder.RegisterType<ShellInformationService>().As<IShellInformationService>().SingleInstance();
            builder.RegisterType<ExceptionHandler>().As<IExceptionHandler>().SingleInstance();
            builder.RegisterType<ModalsService>().As<IModalsService>().SingleInstance();
        }
    }
}
