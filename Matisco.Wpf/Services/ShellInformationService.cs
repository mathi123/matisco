using System;

namespace Matisco.Wpf.Services
{
    public class ShellInformationService : IShellInformationService
    {
        private Type _shellType;
        private string _title = "Use ShellInformationService to set a title";
        private string _iconPath = "pack://application:,,,/Matisco.Wpf;component/Resources/PrismLogo128x128.ico";

        public void SetShellType(Type type)
        {
            _shellType = type;
        }

        public Type GetShellType()
        {
            return _shellType;
        }

        public void SetDefaultTitle(string title)
        {
            _title = title;
        }

        public string GetDefaultTitle()
        {
            return _title;
        }

        public void SetDefaultIcon(string iconPath)
        {
            _iconPath = iconPath;
        }

        public string GetDefaultIconPath()
        {
            return _iconPath;
        }
    }
}