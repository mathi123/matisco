using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Matisco.WebApi.Client
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly IConfigurationManager _configurationManager;

        public HttpClientFactory(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }
        
        public HttpClient GetHttpClient()
        {
            HttpClient client;

            if (_configurationManager.ShouldUseProxy())
            {
                var proxy = new WebProxy(_configurationManager.GetProxyUri(), false)
                {
                    UseDefaultCredentials = false,
                    Credentials = CredentialCache.DefaultNetworkCredentials
                };

                var httpClientHandler = new HttpClientHandler
                {
                    Proxy = proxy,
                    PreAuthenticate = true,
                    UseDefaultCredentials = false,
                    Credentials = CredentialCache.DefaultNetworkCredentials
                };

                client = new HttpClient(httpClientHandler);
            }
            else
            {
                client = new HttpClient();
            }

            client.Timeout = _configurationManager.GetDefaultTimeout();
            client.BaseAddress = new Uri(_configurationManager.GetBaseUrl());
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent",
                                 @"Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; 
                                  WOW64; Trident / 6.0)");

            if (_configurationManager.IsAuthenticated())
            {
                client.DefaultRequestHeaders.Add(_configurationManager.GetTokenHeaderCode(), _configurationManager.GetJsonWebTokenValue());
            }

            ServicePointManager.ServerCertificateValidationCallback = _configurationManager.ValidateServerCertificate;

            return client;
        }
    }
}