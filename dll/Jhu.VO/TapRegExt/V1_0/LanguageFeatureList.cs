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
    [XmlType(Namespace = Constants.TapRegExtNamespaceV1_0)]
    public class LanguageFeatureList : ILanguageFeatureList
    {
        [XmlElement(Constants.TagFeature, Form = XmlSchemaForm.Unqualified)]
        public List<LanguageFeature> FeatureList { get; set; }

        [XmlIgnore]
        ElementList<ILanguageFeature> ILanguageFeatureList.FeatureList
        {
            get { return new ElementList<ILanguageFeature>(FeatureList); }
        }

        [XmlAttribute(Constants.AttributeType, Form = XmlSchemaForm.Unqualified)]
        public string Type { get; set; }
    }
}
