using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Jhu.VO.TapRegExt.V1_0
{
    [XmlType(TypeName = Constants.TypeTableAccess, Namespace = Constants.TapRegExtNamespace)]
    [XmlRoot(VoResource.Constants.TagCapability, Namespace = Constants.TapRegExtNamespace)]
    public class TableAccess : TapCapRestriction
    {
        public DataModelType[] DataModelList { get; set; }
        public Language[] LanguageList { get; set; }
        public OutputFormat[] OutputFormatList { get; set; }
        public UploadMethod[] UploadMethodList { get; set; }
        public TimeLimits RetentionPeriod { get; set; }
        public TimeLimits ExecutionDuration { get; set; }
        public DataLimits OutputLimit { get; set; }
        public DataLimits UploadLimit { get; set; }
    }
}
