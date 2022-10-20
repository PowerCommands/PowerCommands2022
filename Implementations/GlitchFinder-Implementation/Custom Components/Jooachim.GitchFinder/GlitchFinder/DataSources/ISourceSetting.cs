using System.Collections.Generic;

namespace GlitchFinder.DataSources
{
    public interface ISourceSetting
    {
        DataSourceType DataSourceType { get; set; }
        IEnumerable<string> UniqueKeyFields { get; set; }
    }
}
