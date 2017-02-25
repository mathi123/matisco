namespace Matisco.Wpf.Interfaces
{
    public interface IResultData
    {
        string RegionName { get; }

        object[] Results { get; }
    }
}