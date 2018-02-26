using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO.TapRegExt.Common
{
    public interface ILanguage
    {
        string Name { get; set; }
        ElementList<IVersion> VersionList { get; }
        string Description { get; set; }
        ElementList<ILanguageFeatureList> LanguageFeaturesList { get; }
    }
}
