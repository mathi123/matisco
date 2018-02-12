using Autofac;

namespace Matisco.WebApi.Client
{
    public class WebApiClientAutofacModule : Module
    {
        private readonly bool _registerDefaultExceptionHandler;

        public WebApiClientAutofacModule(bool registerDefaultExceptionHandler)
        {
            _registerDefaultExceptionHandler = registerDefaultExceptionHandler;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConfigurationManager>().As<IConfigurationManager>().SingleInstance();
            builder.RegisterType<HttpClientFactory>().As<IHttpClientFactory>();
            builder.RegisterType<JsonWebApi>().As<IJsonWebApiClient>();

            if (_registerDefaultExceptionHandler)
            {
                builder.RegisterType<WebApiExceptionHandler>().As<IWebApiExceptionHandler>();
            }
        }
    }
}
