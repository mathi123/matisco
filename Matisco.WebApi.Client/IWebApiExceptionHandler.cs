using System;

namespace Matisco.WebApi.Client
{
    public interface IWebApiExceptionHandler
    {
        void Unauthorized(string route);
        void InteralException(string route, Exception exception);
        void UnexpectedException(string route, Exception exception);
        void Timeout(string route);
    }
}