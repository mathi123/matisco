using Autofac;
using Example.BusinessApp.Infrastructure.Services;
using Example.BusinessApp.Sales.Services;

namespace Example.BusinessApp.Infrastructure
{
    public class InfrastructureAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserService>().As<IUserService>().SingleInstance();
            builder.RegisterType<CustomerService>().As<ICustomerService>().SingleInstance();
            builder.RegisterType<RoleService>().As<IRoleService>().SingleInstance();
        }
    }
}
