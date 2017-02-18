using Autofac;

namespace Matisco.Domain
{
    public class DomainAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TranslationService>().As<ITranslationService>().SingleInstance();
        }
    }
}
