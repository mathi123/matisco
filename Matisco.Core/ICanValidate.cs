using System.Collections.Generic;

namespace Matisco.Core
{
    public interface ICanValidate
    {
        IEnumerable<ValidationError> GetValidationErrors();
        bool IsValid();
    }
}
