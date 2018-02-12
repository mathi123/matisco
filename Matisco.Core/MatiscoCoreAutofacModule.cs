using Autofac;

namespace Matisco.Core
{
    public class MatiscoCoreAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TranslationService>().As<ITranslationService>().SingleInstance();
        }
    }
}
