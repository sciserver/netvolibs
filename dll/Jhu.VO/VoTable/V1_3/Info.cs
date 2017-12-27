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
    public class Info
    {
        [XmlText]
        public string Text { get; set; }

        [XmlAttribute(Constants.AttributeID, Form = XmlSchemaForm.Unqualified)]
        public string ID { get; set; }

        [XmlAttribute(Constants.AttributeName, Form = XmlSchemaForm.Unqualified)]
        public string Name { get; set; }

        [XmlAttribute(Constants.AttributeValue, Form = XmlSchemaForm.Unqualified)]
        public string Value { get; set; }

        [XmlAttribute(Constants.AttributeUnit, Form = XmlSchemaForm.Unqualified)]
        public string Unit { get; set; }

        [XmlAttribute(Constants.AttributeXType, Form = XmlSchemaForm.Unqualified)]
        public string XType { get; set; }

        [XmlAttribute(Constants.AttributeRef, Form = XmlSchemaForm.Unqualified)]
        public string Ref { get; set; }

        [XmlAttribute(Constants.AttributeUcd, Form = XmlSchemaForm.Unqualified)]
        public string Ucd { get; set; }

        [XmlAttribute(Constants.AttributeUType, Form = XmlSchemaForm.Unqualified)]
        public string UType { get; set; }
    }
}
