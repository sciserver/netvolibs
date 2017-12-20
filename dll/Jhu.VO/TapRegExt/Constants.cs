using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO.TapRegExt
{
    public class Constants
    {
        public const string TapRegExtNamespace = "http://www.ivoa.net/xml/TAPRegExt/v1.0";

        public const string TypeTapCapRestriction = "TapCapRestriction";
        public const string TypeTableAccess = "TableAccess";

        public const string TagDefault = "default";
        public const string TagHard = "hard";
        public const string TagForm = "form";
        public const string TagFeature = "feature";
        public const string TagDataModel = "dataModel";
        public const string TagLanguage = "language";
        public const string TagLanguageFeatures = "languageFeatures";
        public const string TagOutputFormat = "outputFormat";
        public const string TagOutputLimit = "outputLimit";
        public const string TagUploadMethod = "uploadMethod";
        public const string TagUploadLimit = "uploadLimit";
        public const string TagRetentionPeriod = "retentionPeriod";
        public const string TagExecutionDuration = "executionDuration";
        
        public const string AttributeType = "type";
        public const string AttributeUnit = "unit";
        public const string AttributeMime = "mime";
        public const string AttributeAlias = "alias";
    }
}
