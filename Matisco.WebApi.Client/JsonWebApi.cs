using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Matisco.WebApi.Client
{
    public class JsonWebApi : IJsonWebApi
    {
        private readonly IWebApiExceptionHandler _webApiExceptionHandler;
        private readonly IHttpClientFactory _clientFactory;

        protected HttpClient Client => _clientFactory.GetHttpClient();

        public JsonWebApi(IHttpClientFactory clientFactory, IWebApiExceptionHandler webApiExceptionHandler)
        {
            _clientFactory = clientFactory;
            _webApiExceptionHandler = webApiExceptionHandler;
        }

        public async Task<ServiceResult<T>> GetAsync<T>(string route)
        {
            var result = new ServiceResult<T>();

            try
            {
                var response = await Client.GetAsync(route);

                if (response.IsSuccessStatusCode)
                {
                    result.Status = ServiceStatusEnum.Success;
                    result.Data = await response.Content.ReadAsAsync<T>();
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result.Status = ServiceStatusEnum.UnAuthorized;
                    _webApiExceptionHandler.Unauthorized(route);
                }
                else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    result.Status = ServiceStatusEnum.InternalError;
                    _webApiExceptionHandler.InteralException(route, null);
                }
                else
                {
                    result.Status = ServiceStatusEnum.Undefined;
                    _webApiExceptionHandler.UnexpectedException(route, null);
                }
            }
            catch (TaskCanceledException)
            {
                result.Status = ServiceStatusEnum.Timeout;
                _webApiExceptionHandler.Timeout(route);
            }
            catch (Exception ex)
            {
                result.Status = ServiceStatusEnum.Undefined;
                _webApiExceptionHandler.UnexpectedException(route, ex);
            }

            return result;
        }

        public async Task<ServiceResult<T>> PostAsync<T>(string route, object argument)
        {
            var result = new ServiceResult<T>();

            try
            {
                var response = await Client.PostAsJsonAsync(route, argument);

                if (response.IsSuccessStatusCode)
                {
                    result.Status = ServiceStatusEnum.Success;
                    result.Data = await response.Content.ReadAsAsync<T>();
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result.Status = ServiceStatusEnum.UnAuthorized;
                    _webApiExceptionHandler.Unauthorized(route);
                }
                else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    result.Status = ServiceStatusEnum.InternalError;
                    _webApiExceptionHandler.InteralException(route, null);
                }
                else
                {
                    result.Status = ServiceStatusEnum.Undefined;
                    _webApiExceptionHandler.UnexpectedException(route, null);
                }
            }
            catch (TaskCanceledException)
            {
                result.Status = ServiceStatusEnum.Timeout;
                _webApiExceptionHandler.Timeout(route);
            }
            catch (Exception exception)
            {
                result.Status = ServiceStatusEnum.Undefined;
                _webApiExceptionHandler.UnexpectedException(route, exception);
            }

            return result;
        }

        public async Task<ServiceResult> PostAsync(string route, object argument)
        {
            var result = new ServiceResult();

            try
            {
                var response = await Client.PostAsJsonAsync(route, argument);

                if (response.IsSuccessStatusCode)
                {
                    result.Status = ServiceStatusEnum.Success;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result.Status = ServiceStatusEnum.UnAuthorized;
                    _webApiExceptionHandler.Unauthorized(route);
                }
                else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    result.Status = ServiceStatusEnum.InternalError;
                    _webApiExceptionHandler.InteralException(route, null);
                }
                else
                {
                    result.Status = ServiceStatusEnum.Undefined;
                    _webApiExceptionHandler.UnexpectedException(route, null);
                }
            }
            catch (TaskCanceledException)
            {
                result.Status = ServiceStatusEnum.Timeout;
                _webApiExceptionHandler.Timeout(route);
            }
            catch (Exception exception)
            {
                result.Status = ServiceStatusEnum.Undefined;
                _webApiExceptionHandler.UnexpectedException(route, exception);
            }

            return result;
        }

        public async Task<ServiceResult<T>> PutAsync<T>(string route, object argument)
        {
            var result = new ServiceResult<T>();

            try
            {
                var response = await Client.PutAsJsonAsync(route, argument);

                if (response.IsSuccessStatusCode)
                {
                    result.Status = ServiceStatusEnum.Success;
                    result.Data = await response.Content.ReadAsAsync<T>();
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result.Status = ServiceStatusEnum.UnAuthorized;
                    _webApiExceptionHandler.Unauthorized(route);
                }
                else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    result.Status = ServiceStatusEnum.InternalError;
                    _webApiExceptionHandler.InteralException(route, null);
                }
                else
                {
                    result.Status = ServiceStatusEnum.Undefined;
                    _webApiExceptionHandler.UnexpectedException(route, null);
                }
            }
            catch (TaskCanceledException)
            {
                result.Status = ServiceStatusEnum.Timeout;
                _webApiExceptionHandler.Timeout(route);
            }
            catch (Exception exception)
            {
                result.Status = ServiceStatusEnum.Undefined;
                _webApiExceptionHandler.UnexpectedException(route, exception);
            }

            return result;
        }

        public async Task<ServiceResult> PutAsync(string route, object argument)
        {
            var result = new ServiceResult();

            try
            {
                var response = await Client.PutAsJsonAsync(route, argument);

                if (response.IsSuccessStatusCode)
                {
                    result.Status = ServiceStatusEnum.Success;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result.Status = ServiceStatusEnum.UnAuthorized;
                    _webApiExceptionHandler.Unauthorized(route);
                }
                else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    result.Status = ServiceStatusEnum.InternalError;
                    _webApiExceptionHandler.InteralException(route, null);
                }
                else
                {
                    result.Status = ServiceStatusEnum.Undefined;
                    _webApiExceptionHandler.UnexpectedException(route, null);
                }
            }
            catch (TaskCanceledException)
            {
                result.Status = ServiceStatusEnum.Timeout;
                _webApiExceptionHandler.Timeout(route);
            }
            catch (Exception exception)
            {
                result.Status = ServiceStatusEnum.Undefined;
                _webApiExceptionHandler.UnexpectedException(route, exception);
            }

            return result;
        }

        public async Task<ServiceResult> DeleteAsync(string route)
        {
            var result = new ServiceResult();

            try
            {
                var response = await Client.DeleteAsync(route);

                if (response.IsSuccessStatusCode)
                {
                    result.Status = ServiceStatusEnum.Success;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result.Status = ServiceStatusEnum.UnAuthorized;
                    _webApiExceptionHandler.Unauthorized(route);
                }
                else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    result.Status = ServiceStatusEnum.InternalError;
                    _webApiExceptionHandler.InteralException(route, null);
                }
                else
                {
                    result.Status = ServiceStatusEnum.Undefined;
                    _webApiExceptionHandler.UnexpectedException(route, null);
                }
            }
            catch (TaskCanceledException)
            {
                result.Status = ServiceStatusEnum.Timeout;
                _webApiExceptionHandler.Timeout(route);
            }
            catch (Exception exception)
            {
                result.Status = ServiceStatusEnum.Undefined;
                _webApiExceptionHandler.UnexpectedException(route, exception);
            }

            return result;
        }
    }
}
