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
    public class Organisation : Resource
    {
        [XmlElement(Constants.TagFacility, Form = XmlSchemaForm.Unqualified)]
        public List<ResourceName> FacilityList { get; set; }

        [XmlElement(Constants.TagInstrument, Form = XmlSchemaForm.Unqualified)]
        public List<ResourceName> InstrumentList { get; set; }
    }
}
