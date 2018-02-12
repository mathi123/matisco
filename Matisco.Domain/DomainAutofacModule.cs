using Autofac;

namespace Matisco.Core
{
    public class DomainAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TranslationService>().As<ITranslationService>().SingleInstance();
        }
    }
}
