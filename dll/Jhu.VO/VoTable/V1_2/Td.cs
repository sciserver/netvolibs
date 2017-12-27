using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Jhu.VO.VoTable.V1_2
{
    [XmlType(Namespace = Constants.NamespaceVoTableV1_2)]
    public class Td
    {
        [XmlText]
        public string Value { get; set; }

        [XmlAttribute(Constants.AttributeEncoding)]
        public string Encoding { get; set; }
    }
}
