using Autofac;

namespace Example.BusinessApp.Sales.Business
{
    public class SalesBusinessAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SalesDbContext>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerManager>().As<ICustomerManager>();
        }
    }
}
