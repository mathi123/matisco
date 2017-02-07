namespace Matisco.Domain
{
    public class ValidationError
    {
        public string ErrorMessage { get; }

        public ValidationError(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
