namespace Matisco.Wpf.Interfaces
{
    public interface IControlWindowProperties
    {
        event WindowPropertiesChangedDelegate WindowPropertiesChanged;
        WindowPropertyOverrides GetWindowPropertyOverrides();
    }

    public delegate void WindowPropertiesChangedDelegate(IControlWindowProperties sender);
}
