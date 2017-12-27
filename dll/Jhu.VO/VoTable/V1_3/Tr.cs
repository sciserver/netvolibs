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
    [XmlType(Namespace = Constants.NamespaceVoTableV1_3)]
    public class Tr
    {
        [XmlElement(Constants.TagTD)]
        public List<Td> TdList { get; set; }

        [XmlAttribute(Constants.AttributeID)]
        public string ID { get; set; }
    }
}
