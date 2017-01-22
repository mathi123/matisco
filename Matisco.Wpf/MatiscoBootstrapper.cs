using System;
using Autofac;
using Matisco.Wpf.Exceptions;
using Matisco.Wpf.Prism;
using Matisco.Wpf.Services;
using Matisco.Wpf.Views;
using Prism.Autofac;
using Prism.Mvvm;
using Prism.Regions;

namespace Matisco.Wpf
{
    public class MatiscoBootstrapper : AutofacBootstrapper
    {
        private readonly Type _shellType;
        private readonly Action<ContainerBuilder> _typeRegistrationAction;
        private readonly Action<IRegionManager> _regionRegistrationAction;

        public MatiscoBootstrapper(Action<ContainerBuilder> typeRegistrationAction, Action<IRegionManager> regionRegistrationAction, Type shellType = null)
        {
            _shellType = shellType ?? typeof(ShellView);
            _typeRegistrationAction = typeRegistrationAction;
            _regionRegistrationAction = regionRegistrationAction;
        }

        public new IWindowService Run()
        {
            base.Run();

            var shellTypeProvider = Container.Resolve<IShellInformationService>();
            shellTypeProvider.SetShellType(_shellType);

            var regionManager = Container.Resolve<IRegionManager>();
            _regionRegistrationAction(regionManager);

            regionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(Views.UnsavedChangesView));

            var windowManager = Container.Resolve<IWindowService>();

            return windowManager;
        }
         
        protected override void ConfigureContainerBuilder(ContainerBuilder builder)
        {
            base.ConfigureContainerBuilder(builder);

            builder.RegisterType<WindowService>().As<IWindowService>().SingleInstance();
            builder.RegisterType<ApplicationShutdownService>().As<IApplicationShutdownService>().SingleInstance();
            builder.RegisterType<ShellInformationService>().As<IShellInformationService>().SingleInstance();

            _typeRegistrationAction(builder);
        }

        protected override IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
        {
            var behaviours = base.ConfigureDefaultRegionBehaviors();

            behaviours.AddIfMissing(RegionManagerAwareBehaviour.BehaviourKey, typeof(RegionManagerAwareBehaviour));

            return behaviours;
        }

        protected override void ConfigureViewModelLocator()
        {
            ViewModelLocationProvider.SetDefaultViewModelFactory(type => Container.Resolve(type));
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(GetViewModel);
        }

        private Type GetViewModel(Type viewType)
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
