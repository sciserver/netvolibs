using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Jhu.VO.TapRegExt.V1_0
{
    [XmlType(TypeName = Constants.TypeTableAccess, Namespace = Constants.TapRegExtNamespace)]
    [XmlRoot(VoResource.Constants.TagCapability, Namespace = Constants.TapRegExtNamespace)]
    public class TableAccess : TapCapRestriction
    {
        [XmlElement(Constants.TagDataModel, Form = XmlSchemaForm.Unqualified)]
        public List<DataModelType> DataModelList { get; set; }

        [XmlElement(Constants.TagLanguage, Form = XmlSchemaForm.Unqualified)]
        public List<Language> LanguageList { get; set; }

        [XmlElement(Constants.TagOutputFormat, Form = XmlSchemaForm.Unqualified)]
        public List<OutputFormat> OutputFormatList { get; set; }

        [XmlElement(Constants.TagUploadMethod, Form = XmlSchemaForm.Unqualified)]
        public List<UploadMethod> UploadMethodList { get; set; }

        [XmlElement(Constants.TagRetentionPeriod, Form = XmlSchemaForm.Unqualified)]
        public TimeLimits RetentionPeriod { get; set; }

        [XmlElement(Constants.TagExecutionDuration, Form = XmlSchemaForm.Unqualified)]
        public TimeLimits ExecutionDuration { get; set; }

        [XmlElement(Constants.TagOutputLimit, Form = XmlSchemaForm.Unqualified)]
        public DataLimits OutputLimit { get; set; }

        [XmlElement(Constants.TagUploadLimit, Form = XmlSchemaForm.Unqualified)]
        public DataLimits UploadLimit { get; set; }
    }
}
