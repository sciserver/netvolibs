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
    [XmlInclude(typeof(TapRegExt.V1_0.TapCapRestriction))]
    [XmlInclude(typeof(TapRegExt.V1_0.TableAccess))]
    public class Capability
    {
        [XmlElement(Constants.TagValidationLevel, Form = XmlSchemaForm.Unqualified)]
        public Validation[] ValidationLevelList { get; set; }

        [XmlElement(Constants.TagDescription, Form = XmlSchemaForm.Unqualified)]
        public string Description { get; set; }

        [XmlElement(Constants.TagInterface, Form = XmlSchemaForm.Unqualified)]
        public Interface[] InterfaceList { get; set; }

        [XmlAttribute(Constants.AttributeStandardID)]
        public string StandardID { get; set; }
    }
}
