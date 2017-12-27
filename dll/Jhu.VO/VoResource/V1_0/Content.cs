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
    public class Content
    {
        [XmlElement(Constants.TagSubject, Form = XmlSchemaForm.Unqualified)]
        public List<string> SubjectList { get; set; }

        [XmlElement(Constants.TagDescription, Form = XmlSchemaForm.Unqualified)]
        public string Description { get; set; }

        [XmlElement(Constants.TagSource, Form = XmlSchemaForm.Unqualified)]
        public Source Source { get; set; }

        [XmlElement(Constants.TagReferenceUrl, Form = XmlSchemaForm.Unqualified)]
        public string ReferenceUrl { get; set; }

        [XmlElement(Constants.TagType, Form = XmlSchemaForm.Unqualified)]
        public List<string> TypeList { get; set; }

        [XmlElement(Constants.TagContentLevel, Form = XmlSchemaForm.Unqualified)]
        public List<string> ContentLevelList { get; set; }

        [XmlElement(Constants.TagRelationship, Form = XmlSchemaForm.Unqualified)]
        public List<Relationship> RelationshipList { get; set; }
    }
}
