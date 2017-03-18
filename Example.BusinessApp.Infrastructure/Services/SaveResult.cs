using System.Collections.Generic;

namespace Example.BusinessApp.Infrastructure.Services
{
    public class SaveResult
    {
        public bool Succes { get; set; }

        public List<string> ValidationErrors { get; set; }

        public SaveResult()
        {
            ValidationErrors = new List<string>();
        }
    }
}