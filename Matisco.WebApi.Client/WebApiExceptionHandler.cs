using System;
using System.Diagnostics;
using System.Security.Authentication;

namespace Matisco.WebApi.Client
{
    public class WebApiExceptionHandler : IWebApiExceptionHandler
    {
        private readonly bool _rethrowExceptions;

        public WebApiExceptionHandler(bool rethrowExceptions)
        {
            _rethrowExceptions = rethrowExceptions;
        }

        public void Unauthorized(string route)
        {
            Debug.WriteLine($"Unauthorized {route}");
            if(_rethrowExceptions)
                throw new AuthenticationException();
        }

        public void InteralException(string route, Exception exception)
        {
            Debug.WriteLine($"InteralException {route}");
            if (_rethrowExceptions && exception != null)
                throw exception;
        }

        public void UnexpectedException(string route, Exception exception)
        {
            Debug.WriteLine($"UnexpectedException {route}");
            if (_rethrowExceptions && exception != null)
                throw exception;
        }

        public void Timeout(string route)
        {
            Debug.WriteLine($"Timeout {route}");
            if (_rethrowExceptions)
                throw new TimeoutException();
        }
    }
}
