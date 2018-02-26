using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jhu.VO.VoResource.Common;

namespace Jhu.VO.TapRegExt.Common
{
    public interface ITableAccess : ICapability
    {
        ElementList<IDataModelType> DataModelList { get; }
        ElementList<ILanguage> LanguageList { get; }
        ElementList<IOutputFormat> OutputFormatList { get; }
        ElementList<IUploadMethod> UploadMethodList { get; }
        ITimeLimits RetentionPeriod { get; set; }
        ITimeLimits ExecutionDuration { get; set; }
        IDataLimits OutputLimit { get; set; }
        IDataLimits UploadLimit { get; set; }
    }
}
