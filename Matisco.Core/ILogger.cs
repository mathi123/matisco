namespace Matisco.Core
{
    public interface ILogger
    {
        void Verbose(string source, string message, object data = null);
        void Info(string source, string message, object data = null);
        void Warning(string source, string message, object data = null);
        void Error(string source, string message, object data = null);
    }
}
