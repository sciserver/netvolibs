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
    public class Validation
    {
        [XmlText]
        public int Value { get; set; }

        [XmlAttribute(Constants.AttributeValidatedBy, Form = XmlSchemaForm.Unqualified)]
        public string ValidatedBy { get; set; }
    }
}
