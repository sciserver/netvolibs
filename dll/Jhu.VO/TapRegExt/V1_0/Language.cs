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
    public class Language : ILanguage
    {
        [XmlElement(VoResource.Constants.TagName, Form = XmlSchemaForm.Unqualified)]
        public string Name { get; set; }

        [XmlElement(VoResource.Constants.TagVersion, Form = XmlSchemaForm.Unqualified)]
        public List<Version> VersionList { get; set; }

        [XmlIgnore]
        ElementList<IVersion> ILanguage.VersionList
        {
            get { return new ElementList<IVersion>(VersionList); }
        }

        [XmlElement(VoResource.Constants.TagDescription, Form = XmlSchemaForm.Unqualified)]
        public string Description { get; set; }

        [XmlElement(Constants.TagLanguageFeatures, Form = XmlSchemaForm.Unqualified)]
        public List<LanguageFeatureList> LanguageFeaturesList { get; set; }

        [XmlIgnore]
        ElementList<ILanguageFeatureList> ILanguage.LanguageFeaturesList
        {
            get { return new ElementList<ILanguageFeatureList>(LanguageFeaturesList); }
        }
    }
}
