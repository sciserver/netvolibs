using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Jhu.VO.VoResource.V1_0;

namespace Jhu.VO.Vosi.Capabilities.V1_0
{
    [Serializable]
    [XmlType(AnonymousType = true, Namespace = Constants.VosiCapabilitiesNamespaceV1_0)]
    [XmlRoot(Constants.TagCapabilities, Namespace = Constants.VosiCapabilitiesNamespaceV1_0, IsNullable = false)]
    public class Capabilities
    {
        [XmlElement(VoResource.Constants.TagCapability, Form = XmlSchemaForm.Unqualified)]
        public Capability[] CapabilityList { get; set; }
    }
}
