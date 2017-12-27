using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Jhu.VO.Vosi.Capabilities.V1_0
{
    [Serializable]
    [XmlType(AnonymousType = true, Namespace = Constants.VosiCapabilitiesNamespaceV1_0)]
    [XmlRoot(Constants.TagCapabilities, Namespace = Constants.VosiCapabilitiesNamespaceV1_0, IsNullable = false)]
    public class Capabilities
    {
        [XmlElement(VoResource.Constants.TagCapability, Form = XmlSchemaForm.Unqualified)]
        public List<VoResource.V1_0.Capability> CapabilityList { get; set; }
    }
}
