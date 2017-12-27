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
    public class Link
    {
        [XmlAttribute(Constants.AttributeID, Form = XmlSchemaForm.Unqualified)]
        public string ID { get; set; }

        [XmlAttribute(Constants.AttributeContentRole, Form = XmlSchemaForm.Unqualified)]
        public string ContentRole { get; set; }

        [XmlAttribute(Constants.AttributeContentType, Form = XmlSchemaForm.Unqualified)]
        public string ContentType { get; set; }

        [XmlAttribute(Constants.AttributeTitle, Form = XmlSchemaForm.Unqualified)]
        public string Title { get; set; }

        [XmlAttribute(Constants.AttributeValue, Form = XmlSchemaForm.Unqualified)]
        public string Value { get; set; }

        [XmlAttribute(Constants.AttributeHref, Form = XmlSchemaForm.Unqualified)]
        public string Href { get; set; }

        [XmlAttribute(Constants.AttributeGref, Form = XmlSchemaForm.Unqualified)]
        public string Gref { get; set; }

        [XmlAttribute(Constants.AttributeAction, Form = XmlSchemaForm.Unqualified)]
        public string Action { get; set; }
    }
}
