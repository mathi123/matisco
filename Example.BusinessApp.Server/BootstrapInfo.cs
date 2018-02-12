using Autofac;
using Example.BusinessApp.Sales.Business;
using Example.BusinessApp.Sales.Services;
using Matisco.Server.Host;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Example.BusinessApp.Server
{
    public class BootstrapInfo : FrameworkServerStartup
    {
        public BootstrapInfo(IHostingEnvironment env) : base(env)
        {
        }

        public override void ConfigureContainer(ContainerBuilder builder)
        {
            base.ConfigureContainer(builder);

            builder.RegisterModule(new SalesBusinessAutofacModule());
            builder.RegisterModule(new SalesServicesAutofacModule());
        }
    }
}
