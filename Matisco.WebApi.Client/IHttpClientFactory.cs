using System.Net.Http;

namespace Matisco.WebApi.Client
{
    public interface IHttpClientFactory
    {
        HttpClient GetHttpClient();
    }
}
