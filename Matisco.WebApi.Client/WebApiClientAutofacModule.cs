using Autofac;

namespace Matisco.WebApi.Client
{
    public class WebApiClientAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConfigurationManager>().As<IConfigurationManager>().SingleInstance();
            builder.RegisterType<HttpClientFactory>().As<IHttpClientFactory>();
            builder.RegisterType<JsonWebApi>().As<IJsonWebApi>();
            builder.RegisterType<WebApiExceptionHandler>().As<IWebApiExceptionHandler>();
        }
    }
}
