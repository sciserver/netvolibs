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
    public class Source
    {
        [XmlText]
        public string Value { get; set; }

        [XmlAttribute(Constants.AttributeFormat, Form = XmlSchemaForm.Unqualified)]
        public string Format { get; set; }
    }
}
