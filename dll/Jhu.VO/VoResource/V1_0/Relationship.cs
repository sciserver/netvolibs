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
    public class Relationship
    {
        [XmlElement(Constants.TagRelationshipType, Form = XmlSchemaForm.Unqualified)]
        public string RelationshipType { get; set; }

        [XmlElement(Constants.TagRelatedResource, Form = XmlSchemaForm.Unqualified)]
        public ResourceName[] RelatedResourceList { get; set; }
    }
}
