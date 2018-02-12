using System;
using System.Collections.Generic;
using Autofac;

namespace Matisco.Wpf
{
    public abstract class WpfBootstrapper : IDefinesModules
    {
        private PrismAutofacBootstrapper _bootstrapper;

        public IContainer Container => _bootstrapper.Container;
        
        public void Run()
        {
            _bootstrapper = new PrismAutofacBootstrapper(this);
            _bootstrapper.Run();
        }

        public abstract IEnumerable<Type> GetPrismModuleTypes();

        public abstract IEnumerable<Type> GetAutofacModuleTypes();
    }
}
