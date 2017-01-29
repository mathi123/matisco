using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Matisco.WebApi.Client
{
    public interface IConfigurationManager
    {
        string GetBaseUrl();
        bool ShouldUseProxy();
        string GetProxyUri();
        void SetDefaultTimeoutInSeconds(int seconds);
        TimeSpan GetDefaultTimeout();
        bool IsAuthenticated();
        string GetJsonWebTokenValue();
        bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors);
        string GetTokenHeaderCode();
    }
}