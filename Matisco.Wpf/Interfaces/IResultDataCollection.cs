using System.Collections.Generic;

namespace Matisco.Wpf.Interfaces
{
    public interface IResultDataCollection
    {
        IEnumerable<IResultData> GetResults();
    }
}