﻿using System;
using System.Diagnostics;
using System.Linq;
using Autofac;
using Matisco.Wpf.Exceptions;
using Matisco.Wpf.Prism;
using Matisco.Wpf.Services;
using Prism.Autofac;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;

namespace Matisco.Wpf
{
    public class MatiscoBootstrapper : AutofacBootstrapper
    {
        private readonly BootstrapperBase _bootstrapper;

        public MatiscoBootstrapper(BootstrapperBase bootstrapper)
        {
            _bootstrapper = bootstrapper;
        }

        protected override IContainer CreateContainer(ContainerBuilder builder)
        {
            foreach (var moduleType in _bootstrapper.GetAutofacModuleTypes())
            {
                var module = Activator.CreateInstance(moduleType) as Autofac.Core.IModule;

                if (module != null)
                {
                    builder.RegisterModule(module);
                }
                else
                {
                    Trace.TraceWarning($"{moduleType.FullName} is not an Autofac module");
                }
            }

            return base.CreateContainer(builder);
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();

            foreach (var module in _bootstrapper.GetPrismModuleTypes())
            {
                var catalog = (ModuleCatalog)ModuleCatalog;
                catalog.AddModule(module);
            }   
        }

        //protected override IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
        //{
        //    var behaviours = base.ConfigureDefaultRegionBehaviors();
        //    behaviours.Where(beh => beh.)
        //    //behaviours.AddIfMissing(RegionManagerAwareBehaviour.BehaviourKey, typeof(RegionManagerAwareBehaviour));

        //    return behaviours;
        //}

        protected override void ConfigureViewModelLocator()
        {
            ViewModelLocationProvider.SetDefaultViewModelFactory(ResolveViewModel);
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(GetViewModel);
        }

        private object ResolveViewModel(object arg1, Type arg2)
        {
            var windowService = Container.Resolve<IWindowService>();

            var regionManager = windowService?.GetCurrentRegionManager();

            if (regionManager == null)
            {
                return Container.Resolve(arg2);
            }

            return Container.Resolve(arg2, new TypedParameter(typeof(IRegionManager), regionManager));
        }


        protected Type GetViewModel(Type viewType)
        {
            var name = viewType.FullName;

            // Step 1: try loading from the same directory
            var viewModel = name + "Model";
            var type = viewType.Assembly.GetType(viewModel, false);
            if (type != null) return type;

            // Step 2: try loading form a sibling directory named 'ViewModels'
            viewModel = name.Replace(".Views.", ".ViewModels.") + "Model";
            type = viewType.Assembly.GetType(viewModel, false); 
            if (type != null) return type;

            throw new ViewModelNotFoundException(viewType);
        }
    }
}
