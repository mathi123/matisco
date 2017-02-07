using System.Collections.Generic;

namespace Matisco.Domain
{
    public interface ICanValidate
    {
        IEnumerable<ValidationError> GetValidationErrors();
        bool IsValid();
    }
}
