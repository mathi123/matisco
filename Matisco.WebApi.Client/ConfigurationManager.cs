using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Matisco.WebApi.Client
{
    public class ConfigurationManager : IConfigurationManager
    {
        private readonly string _url;
        private TimeSpan _timeout = new TimeSpan(0, 0, 0, 30);

        public ConfigurationManager(string url)
        {
            _url = url;
        }

        public virtual string GetBaseUrl()
        {
            return _url;
        }

        public virtual bool ShouldUseProxy()
        {
            return false;
        }

        public virtual string GetProxyUri()
        {
            return null;
        }

        public virtual void SetDefaultTimeoutInSeconds(int sec)
        {
            _timeout = new TimeSpan(0, 0, 0, sec);
        }

        public virtual TimeSpan GetDefaultTimeout()
        {
            return _timeout;
        }

        public virtual bool IsAuthenticated()
        {
            return false;
        }

        public virtual string GetJsonWebTokenValue()
        {
            return null;
        }

        public virtual bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain,
            SslPolicyErrors sslpolicyerrors)
        {
            return true;
        }

        public virtual string GetTokenHeaderCode()
        {
            return "Token";
        }
    }
}
