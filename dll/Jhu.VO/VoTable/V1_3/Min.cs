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
    public class Min
    {
        [XmlAttribute(Constants.AttributeValue, Form = XmlSchemaForm.Unqualified)]
        public string Value { get; set; }

        [XmlAttribute(Constants.AttributeInclusive, Form = XmlSchemaForm.Unqualified)]
        public string Inclusive { get; set; }
    }
}
