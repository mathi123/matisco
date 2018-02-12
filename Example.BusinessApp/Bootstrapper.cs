using System;
using System.Collections.Generic;
using Autofac;
using Example.BusinessApp.Infrastructure;
using Example.BusinessApp.ItAdmin;
using Example.BusinessApp.Sales;
using Matisco.Core;
using Matisco.WebApi.Client;
using Matisco.Wpf;

namespace Example.BusinessApp
{
    public class Bootstrapper : WpfBootstrapper
    {
        public override IEnumerable<Type> GetPrismModuleTypes()
        {
            yield return typeof(MatiscoPrismModule);
            yield return typeof(InfrastructurePrismModule);
            yield return typeof(SalesPrismModule);
            yield return typeof(ItAdminPrismModule);
        }

        public override IEnumerable<Module> GetAutofacModules()
        {
            yield return new MatiscoCoreAutofacModule();
            yield return new WebApiClientAutofacModule(true);
            yield return new MatiscoAutofacModule();
            yield return new InfrastructureAutofacModule();
        }
    }
}
