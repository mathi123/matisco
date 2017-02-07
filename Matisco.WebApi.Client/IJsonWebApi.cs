using System.Threading.Tasks;

namespace Matisco.WebApi.Client
{
    public interface IJsonWebApi
    {
        Task<ServiceResult<T>> GetAsync<T>(string route);
        Task<ServiceResult> DeleteAsync(string route);
        Task<ServiceResult> PostAsync(string route, object argument);
        Task<ServiceResult<T>> PostAsync<T>(string route, object argument);
        Task<ServiceResult> PutAsync(string route, object argument);
        Task<ServiceResult<T>> PutAsync<T>(string route, object argument);
    }
}