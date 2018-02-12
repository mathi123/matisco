using Autofac;
using Matisco.Core;

namespace Matisco.Server.Host
{
    public class MatiscoServerAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ServerSideLogger>().As<ILogger>().InstancePerLifetimeScope();
        }
    }
}
