namespace Matisco.WebApi.Client
{
    public class ServiceResult
    {
        public ServiceStatusEnum Status { get; set; }
    }

    public class ServiceResult<T> : ServiceResult
    {
        public T Data { get; set; }

        public ServiceResult() 
        {
            Data = default(T);
        }
    }
}
