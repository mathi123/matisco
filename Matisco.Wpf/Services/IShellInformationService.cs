using System;

namespace Matisco.Wpf.Services
{
    public interface IShellInformationService
    {
        void SetShellType(Type type);

        Type GetShellType();

        void SetDefaultTitle(string title);

        string GetDefaultTitle();

        void SetDefaultIcon(string iconPath);

        string GetDefaultIconPath();
    }
}
