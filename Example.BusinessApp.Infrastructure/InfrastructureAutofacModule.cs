using Autofac;
using Example.BusinessApp.Infrastructure.Services;
using Example.BusinessApp.Sales.Services;

namespace Example.BusinessApp.Infrastructure
{
    public class InfrastructureAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CustomerService>().As<ICustomerService>().SingleInstance();
        }
    }
}
