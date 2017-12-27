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
    public class FieldRef
    {
        [XmlAttribute(Constants.AttributeRef, Form = XmlSchemaForm.Unqualified)]
        public string Ref { get; set; }

        [XmlAttribute(Constants.AttributeUcd, Form = XmlSchemaForm.Unqualified)]
        public string Ucd { get; set; }

        [XmlAttribute(Constants.AttributeUType, Form = XmlSchemaForm.Unqualified)]
        public string UType { set; get; }
    }
}
