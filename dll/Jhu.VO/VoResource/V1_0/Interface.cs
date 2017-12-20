using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Jhu.VO.VoResource.V1_0
{
    [XmlType(Namespace = Constants.VoResourceNamespaceV1_0)]
    [XmlInclude(typeof(WebBrowser))]
    [XmlInclude(typeof(WebService))]
    [XmlInclude(typeof(VoDataService.V1_0.ParamHttp))]
    public class Interface
    {
        [XmlElement(Constants.TagAccessUrl, Form = XmlSchemaForm.Unqualified)]
        public AccessUrl[] AccessUrlList { get; set; }

        [XmlElement(Constants.TagSecurityMethod, Form = XmlSchemaForm.Unqualified)]
        public SecurityMethod[] SecurityMethodList { get; set; }

        [XmlAttribute(Constants.AttributeVersion, Form = XmlSchemaForm.Unqualified)]
        public string Version { get; set; }

        [XmlAttribute(Constants.AttributeRole, Form = XmlSchemaForm.Unqualified)]
        public string Role { get; set; }
    }
}
