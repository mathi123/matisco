using Matisco.Wpf.Interfaces;

namespace Matisco.Wpf.Models
{
    public class ResultData : IResultData
    {
        public string RegionName { get; set; }

        public object[] Results { get; set; }

        public ResultData(string regionName, object[] results)
        {
            RegionName = regionName;
            Results = results;
        }
    }
}