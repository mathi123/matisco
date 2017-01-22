using System.Collections.Generic;

namespace Matisco.Wpf.Services
{
    public interface IApplicationShutdownService
    {
        List<string> GetBlockingWindows();

        void ExitApplication(bool forceExit);
    }
}
