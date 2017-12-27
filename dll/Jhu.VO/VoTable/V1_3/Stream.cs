using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Jhu.VO.VoTable.V1_3
{
    [XmlType(Namespace = Constants.VOTableNamespaceV1_3)]
    public class Stream
    {
        [XmlText]
        public string Value { get; set; }

        [XmlAttribute(Constants.AttributeType, Form = XmlSchemaForm.Unqualified)]
        public string Type { get; set; }

        [XmlAttribute(Constants.AttributeHref, Form = XmlSchemaForm.Unqualified)]
        public string Href { get; set; }

        [XmlAttribute(Constants.AttributeActuate, Form = XmlSchemaForm.Unqualified)]
        public string Actuate { get; set; }

        [XmlAttribute(Constants.AttributeEncoding, Form = XmlSchemaForm.Unqualified)]
        public string Encoding { get; set; }

        [XmlAttribute(Constants.AttributeExpires, Form = XmlSchemaForm.Unqualified)]
        public DateTime Expires { get; set; }

        [XmlAttribute(Constants.AttributeRights, Form = XmlSchemaForm.Unqualified)]
        public string Rights { get; set; }
    }
}
