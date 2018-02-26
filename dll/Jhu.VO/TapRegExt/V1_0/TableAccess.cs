using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Jhu.VO.TapRegExt.Common;

namespace Jhu.VO.TapRegExt.V1_0
{
    [XmlType(TypeName = Constants.TypeTableAccess, Namespace = Constants.TapRegExtNamespaceV1_0)]
    [XmlRoot(VoResource.Constants.TagCapability, Namespace = Constants.TapRegExtNamespaceV1_0)]
    public class TableAccess : TapCapRestriction, ITableAccess
    {
        [XmlElement(Constants.TagDataModel, Form = XmlSchemaForm.Unqualified)]
        public List<DataModelType> DataModelList { get; set; }

        [XmlIgnore]
        ElementList<IDataModelType> ITableAccess.DataModelList
        {
            get { return new ElementList<IDataModelType>(DataModelList); }
        }

        [XmlElement(Constants.TagLanguage, Form = XmlSchemaForm.Unqualified)]
        public List<Language> LanguageList { get; set; }

        [XmlIgnore]
        ElementList<ILanguage> ITableAccess.LanguageList
        {
            get { return new ElementList<ILanguage>(LanguageList); }
        }

        [XmlElement(Constants.TagOutputFormat, Form = XmlSchemaForm.Unqualified)]
        public List<OutputFormat> OutputFormatList { get; set; }

        [XmlIgnore]
        ElementList<IOutputFormat> ITableAccess.OutputFormatList
        {
            get { return new ElementList<IOutputFormat>(OutputFormatList); }
        }

        [XmlElement(Constants.TagUploadMethod, Form = XmlSchemaForm.Unqualified)]
        public List<UploadMethod> UploadMethodList { get; set; }

        [XmlIgnore]
        ElementList<IUploadMethod> ITableAccess.UploadMethodList
        {
            get { return new ElementList<IUploadMethod>(UploadMethodList); }
        }

        [XmlElement(Constants.TagRetentionPeriod, Form = XmlSchemaForm.Unqualified)]
        public TimeLimits RetentionPeriod { get; set; }

        [XmlIgnore]
        ITimeLimits ITableAccess.RetentionPeriod
        {
            get { return RetentionPeriod; }
            set { RetentionPeriod = (TimeLimits)value; }
        }

        [XmlElement(Constants.TagExecutionDuration, Form = XmlSchemaForm.Unqualified)]
        public TimeLimits ExecutionDuration { get; set; }

        [XmlIgnore]
        ITimeLimits ITableAccess.ExecutionDuration
        {
            get { return ExecutionDuration; }
            set { ExecutionDuration = (TimeLimits)value; }
        }

        [XmlElement(Constants.TagOutputLimit, Form = XmlSchemaForm.Unqualified)]
        public DataLimits OutputLimit { get; set; }

        [XmlIgnore]
        IDataLimits ITableAccess.OutputLimit
        {
            get { return OutputLimit; }
            set { OutputLimit = (DataLimits)value; }
        }

        [XmlElement(Constants.TagUploadLimit, Form = XmlSchemaForm.Unqualified)]
        public DataLimits UploadLimit { get; set; }

        [XmlIgnore]
        IDataLimits ITableAccess.UploadLimit
        {
            get { return UploadLimit; }
            set { UploadLimit = (DataLimits)value; }
        }
    }
}
